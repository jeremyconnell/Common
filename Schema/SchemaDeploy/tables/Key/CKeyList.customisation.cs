using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;

using Framework;

namespace SchemaDeploy
{
    //Collection Class (Customisable half)
    public partial class CKeyList
    {
        #region Filters
        public CKey GetOrCreate(string name, CGroup g, CFormat f)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            var k = GetByName(name);
            if (null != k)
            {
                if (g != null && k.KeyGroupId != g.GroupId)
                {
                    k.KeyGroupId = g.GroupId;
                    k.Save();
                }
                return k;
            }

            k = new CKey();
            k.KeyName = name;
            k.KeyFormatId = f.FormatId;
            if (null != g)
                k.KeyGroupId = g.GroupId;
            k.Save();
            return k;
        }
        public CKey GetByName(string name)
        {
            if (null == name)
                return null;
            name = name.ToLower();
            foreach (var i in this)
                if (i.KeyName.ToLower() == name)
                    return i;
            return null;
        }
        #endregion

        #region Aggregation
        #endregion

        #region Searching (Optional)
        //Represents a simple search box to search PK and any string columns (add overloads as required, based on the pattern below)
        //e.g. public CKeyList Search(string nameOrId, int groupId, int formatId, bool? defaultBoolean, bool? isEncrypted) { ...
        public CKeyList Search( string nameOrId, int groupId, int formatId)
        {
            //1. Normalisation
            nameOrId = (nameOrId??string.Empty).Trim().ToLower();
            
            //2. Start with a complete list
            CKeyList results = this;

            //3. Use any available index, such as those generated for fk/bool columns
            //Normal Case - non-unique index (e.g. foreign key)
            if (int.MinValue != groupId)
                if (0 == groupId)
                    results = results.GetByGroupId(int.MinValue);
                else
                    results = results.GetByGroupId(groupId);

            if (int.MinValue != formatId)
                results = results.GetByFormatId(formatId);

            
            //4. Exit early if remaining (non-index) filters are blank
            if (string.IsNullOrEmpty(nameOrId)) return results; 
            
            //5. Manually search each record using custom match logic, building a shortlist
            CKeyList shortList = new CKeyList();
            foreach (CKey i in results)
                if (Match(nameOrId, i))
                    shortList.Add(i); 
            return shortList;
        }
        //Manual Searching e.g for string-based columns i.e. anything not indexed (add more params if required)
        private bool Match(string name, CKey obj)
        {
            if (!string.IsNullOrEmpty(name)) //Match any string column
            {
                if (null != obj.KeyName && obj.KeyName.ToLower().Contains(name))   return true;
                if (null != obj.KeyDefaultString && obj.KeyDefaultString.ToLower().Contains(name))   return true;
                return false;   //If filter is active, reject any items that dont match
            }
            return true;    //No active filters (should catch this in step #4)
        }
        #endregion

        #region Cloning
        public CKeyList Clone(CDataSrc target) //, int parentId)
        {
            //No Transaction
            if (target is CDataSrcRemote)
                return Clone(target, null); //, parentId);

            //Transaction
            using (IDbConnection cn = target.Local.Connection())
            {
                IDbTransaction tx = cn.BeginTransaction();
                try
                {
                    CKeyList clone = Clone(target, tx); //, parentId);
                    tx.Commit();
                    return clone;
                }
                catch
                {
                    tx.Rollback();
                    throw;
                }
            }
        }
        public CKeyList Clone(CDataSrc target, IDbTransaction txOrNull) //, int parentId)
        {
            CKeyList list = new CKeyList(this.Count);
            foreach (CKey i in this)
                list.Add(i.Clone(target, txOrNull)); //, parentId));  *Child entities must reference the new parent
            return list;
        }
        #endregion
        
        #region Export to Csv
        //Web - Need to add a project reference to System.Web, or comment out these two methods
        public void ExportToCsv(HttpResponse response) { ExportToCsv(response, "Keies.csv"); }
        public void ExportToCsv(HttpResponse response, string fileName)
        {
            CDataSrc.ExportToCsv(response, fileName); //Standard response headers
            StreamWriter sw = new StreamWriter(response.OutputStream);
            ExportToCsv(sw);
            sw.Flush();
            response.End();
        }

        //Non-web
        public void ExportToCsv(string filePath)
        {
            StreamWriter sw = new StreamWriter(filePath);
            ExportToCsv(sw);
            sw.Close();
        }

        //Logic
        protected void ExportToCsv(StreamWriter sw)
        {
            string[] headings = new string[] {"KeyName", "KeyGroupId", "KeyFormatId", "KeyDefaultString", "KeyDefaultBoolean", "KeyDefaultInteger", "KeyIsEncrypted"};
            CDataSrc.ExportToCsv(headings, sw);
            foreach (CKey i in this)
            {
                object[] data = new object[] {i.KeyName, i.KeyGroupId, i.KeyFormatId, i.KeyDefaultString, i.KeyDefaultBoolean, i.KeyDefaultInteger, i.KeyIsEncrypted};
                CDataSrc.ExportToCsv(data, sw);
            }
        }
        #endregion

        #region Cache-Control
        //Main Logic
        public new void Add(CKey item)
        {
            if (null != _index && !_index.ContainsKey(item.KeyName))
                _index[item.KeyName] = item;

            _indexByDefaultBoolean = null;
            _indexByFormatId = null;
            _indexByGroupId = null;
            _indexByIsEncrypted = null;

            base.Add(item);
        }
        public new void Remove(CKey item)
        {
            if (null != _index && _index.ContainsKey(item.KeyName))
                _index.Remove(item.KeyName);

            _indexByDefaultBoolean = null;
            _indexByFormatId = null;
            _indexByGroupId = null;
            _indexByIsEncrypted = null;

            base.Remove(item);
        }

        #endregion
    }
}
