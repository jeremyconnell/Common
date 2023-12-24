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
    public partial class CValueList
    {

        #region Filters

        public CValue GetOrCreate(CInstance i, CKey k, bool? valueBool, int? valueInt, string valueString, bool overwrite = false)
        {
            if (null == i || null == k)
                return null;
            return GetOrCreate(i.InstanceId, k.KeyName, valueBool, valueInt, valueString, overwrite);
        }
        public CValue GetOrCreate(int instanceId,  string keyName)
        {
            return GetOrCreate(instanceId, keyName, null, null, null);
        }
        public CValue GetOrCreate(int instanceId, string keyName, bool? valueBool, int? valueInt, string valueString, bool overwrite = false)
        {
            var v =  GetById(instanceId,  keyName);
            if (null == v)
                v = new CValue() { ValueInstanceId=instanceId, ValueKeyName = keyName };
            if (v.ValueId <= 0 || overwrite)
            {
                v.ValueString = valueString;
                v.ValueInteger = valueInt.HasValue ? valueInt.Value : int.MinValue;
                v.ValueBoolean = valueBool;
                v.Save();
            }
            return v;
        }
        #endregion

        #region Aggregation
        #endregion

        #region Searching (Optional)
        //Represents a simple search box to search PK and any string columns (add overloads as required, based on the pattern below)
        //e.g. public CValueList Search(string nameOrId, ) { ...
        public CValueList Search(string nameOrId,  int instanceId, string keyName)
        {
            //1. Normalisation
            nameOrId = (nameOrId??string.Empty).Trim().ToLower();
            
            //2. Start with a complete list
            CValueList results = this;

            //3. Use any available index, such as those generated for fk/bool columns
            //Normal Case - non-unique index (e.g. foreign key)
            if (int.MinValue != instanceId)
                results = results.GetByInstanceId(instanceId);
            if (!string.IsNullOrEmpty(keyName))
                results = results.GetByKeyName(keyName);

            
            //4. Exit early if remaining (non-index) filters are blank
            if (string.IsNullOrEmpty(nameOrId)) return results; 
            
            //5. Manually search each record using custom match logic, building a shortlist
            CValueList shortList = new CValueList();
            foreach (CValue i in results)
                if (Match(nameOrId, i))
                    shortList.Add(i); 
            return shortList;
        }
        //Manual Searching e.g for string-based columns i.e. anything not indexed (add more params if required)
        private bool Match(string name, CValue obj)
        {
            if (!string.IsNullOrEmpty(name)) //Match any string column
            {
                if (null != obj.ValueKeyName && obj.ValueKeyName.ToLower().Contains(name))   return true;
                if (null != obj.ValueKeyName && obj.ValueKeyName.ToLower().Contains(name))   return true;
                if (null != obj.ValueString && obj.ValueString.ToLower().Contains(name))   return true;
                return false;   //If filter is active, reject any items that dont match
            }
            return true;    //No active filters (should catch this in step #4)
        }
        #endregion

        #region Cloning
        public CValueList Clone(CDataSrc target) //, int parentId)
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
                    CValueList clone = Clone(target, tx); //, parentId);
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
        public CValueList Clone(CDataSrc target, IDbTransaction txOrNull) //, int parentId)
        {
            CValueList list = new CValueList(this.Count);
            foreach (CValue i in this)
                list.Add(i.Clone(target, txOrNull)); //, parentId));  *Child entities must reference the new parent
            return list;
        }
        #endregion
        
        #region Export to Csv
        //Web - Need to add a project reference to System.Web, or comment out these two methods
        public void ExportToCsv(HttpResponse response) { ExportToCsv(response, "Values.csv"); }
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
            string[] headings = new string[] {"ValueId", "ValueInstanceId", "ValueKeyName", "ValueString", "ValueBoolean", "ValueInteger"};
            CDataSrc.ExportToCsv(headings, sw);
            foreach (CValue i in this)
            {
                object[] data = new object[] {i.ValueId, i.ValueInstanceId, i.ValueKeyName, i.ValueString, i.ValueBoolean, i.ValueInteger};
                CDataSrc.ExportToCsv(data, sw);
            }
        }
        #endregion


        #region Cache-Control
        //Main Logic
        public new void Add(CValue v)
        {
            if (null != _index && !_index.ContainsKey(v.ValueId))
                _index[v.ValueId] = v;

            _indexByInstanceId = null;
            _indexByKeyName = null;
            base.Add(v);
        }
        public new void Remove(CValue v)
        {
            if (null != _index && _index.ContainsKey(v.ValueId))
                _index.Remove(v.ValueId);

            _indexByInstanceId = null;
            _indexByKeyName = null;
            base.Remove(v);
        }
        #endregion

        #region Unique Index (on ValueClientId, ValueKeyName)
        public CValue GetById(int instanceId, string keyName)
        {
            CValueList vv = GetByInstanceId(instanceId).GetByKeyName(keyName);
            if (vv.Count == 0)
                return null;
            return vv[0];
        }
        #endregion
    }
}
