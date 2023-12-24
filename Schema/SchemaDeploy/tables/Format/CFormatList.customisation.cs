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
    public partial class CFormatList
    {
        #region Filters
        public CFormat GetByName(string name)
        {
            if (null == name)
                return null;
            name = name.ToLower();
            foreach (var i in this)
                if (i.FormatName.ToLower() == name)
                    return i;
            return null;
        }
        #endregion

        #region Aggregation
        #endregion

        #region Searching (Optional)
        //Represents a simple search box to search PK and any string columns (add overloads as required, based on the pattern below)
        //e.g. public CFormatList Search(string nameOrId, ) { ...
        public CFormatList Search(string nameOrId)
        {
            //1. Normalisation
            nameOrId = (nameOrId??string.Empty).Trim().ToLower();
            
            //2. Start with a complete list
            CFormatList results = this;
            
            //3. Use any available index, such as those generated for fk/bool columns
            //Normal Case - non-unique index (e.g. foreign key)
            //if (int.MinValue != foreignKey) results = results.GetByForeignKey(foreignKey);

            //Special case - unique index (e.g. primary key)
            /*
            if (!string.IsNullOrEmpty(nameOrId)) 
            {
                int id;
                if (int.TryParse(nameOrId, out id))
                {
                    CFormat obj = this.GetById(id);
                    if (null != obj)
                    {
                        results = new CFormatList(1);
                        results.Add(obj);
                        return results;
                    }
                }
            }
            */
            
            //4. Exit early if remaining (non-index) filters are blank
            if (string.IsNullOrEmpty(nameOrId)) return results; 
            
            //5. Manually search each record using custom match logic, building a shortlist
            CFormatList shortList = new CFormatList();
            foreach (CFormat i in results)
                if (Match(nameOrId, i))
                    shortList.Add(i); 
            return shortList;
        }
        //Manual Searching e.g for string-based columns i.e. anything not indexed (add more params if required)
        private bool Match(string name, CFormat obj)
        {
            if (!string.IsNullOrEmpty(name)) //Match any string column
            {
                if (null != obj.FormatName && obj.FormatName.ToLower().Contains(name))   return true;
                return false;   //If filter is active, reject any items that dont match
            }
            return true;    //No active filters (should catch this in step #4)
        }
        #endregion

        #region Cloning
        public CFormatList Clone(CDataSrc target) //, int parentId)
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
                    CFormatList clone = Clone(target, tx); //, parentId);
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
        public CFormatList Clone(CDataSrc target, IDbTransaction txOrNull) //, int parentId)
        {
            CFormatList list = new CFormatList(this.Count);
            foreach (CFormat i in this)
                list.Add(i.Clone(target, txOrNull)); //, parentId));  *Child entities must reference the new parent
            return list;
        }
        #endregion
        
        #region Export to Csv
        //Web - Need to add a project reference to System.Web, or comment out these two methods
        public void ExportToCsv(HttpResponse response) { ExportToCsv(response, "Formats.csv"); }
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
            string[] headings = new string[] {"FormatId", "FormatName"};
            CDataSrc.ExportToCsv(headings, sw);
            foreach (CFormat i in this)
            {
                object[] data = new object[] {i.FormatId, i.FormatName};
                CDataSrc.ExportToCsv(data, sw);
            }
        }
        #endregion
    }
}
