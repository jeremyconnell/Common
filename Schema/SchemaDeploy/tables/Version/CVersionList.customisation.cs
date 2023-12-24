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
    public partial class CVersionList
    {
        #region Filters
        #endregion

        #region Aggregation
        public List<string> Names
        {
            get
            {
                var list = new List<string>(this.Count);
                foreach (var i in this)
                    list.Add(i.VersionName);
                return list;
            }
        }
        public CBinaryFileList BinaryFiles
        {
            get
            {
                var list = new CBinaryFileList(this.Count);
                foreach (var i in this)
                    foreach (var j in i.BinaryFiles)
                        if (!list.Contains(j))
                            list.Add(j);
                return list;
            }
        }
        public string TotalBytes_ {  get { return CUtilities.FileSize(TotalBytes); } }
        public long TotalBytes
        {
            get
            {
                long t = 0;
                foreach (var i in this)
                    t += i.VersionTotalBytes;
                return t;
            }
        }
        #endregion

        #region Index
        public CVersion GetByFilesMD5_(Guid filesMD5)
        {
            var vv = GetByFilesMD5(filesMD5);
            if (vv.Count > 0)
                return vv[0];
            return null;
        }
        #endregion

        #region Searching (Optional)
        //Represents a simple search box to search PK and any string columns (add overloads as required, based on the pattern below)
        //e.g. public CVersionList Search(string nameOrId, int appId) { ...
        public CVersionList Search(string nameOrId, int appId)
        {
            //1. Normalisation
            nameOrId = (nameOrId??string.Empty).Trim().ToLower();
            
            //2. Start with a complete list
            CVersionList results = this;
            
            //3. Use any available index, such as those generated for fk/bool columns
            //Normal Case - non-unique index (e.g. foreign key)
            if (int.MinValue != appId) results = results.GetByAppId(appId);

            //Special case - unique index (e.g. primary key)
            /*
            if (!string.IsNullOrEmpty(nameOrId)) 
            {
                int id;
                if (int.TryParse(nameOrId, out id))
                {
                    CVersion obj = this.GetById(id);
                    if (null != obj)
                    {
                        results = new CVersionList(1);
                        results.Add(obj);
                        return results;
                    }
                }
            }
            */
            
            //4. Exit early if remaining (non-index) filters are blank
            if (string.IsNullOrEmpty(nameOrId)) return results; 
            
            //5. Manually search each record using custom match logic, building a shortlist
            CVersionList shortList = new CVersionList();
            foreach (CVersion i in results)
                if (Match(nameOrId, i))
                    shortList.Add(i); 
            return shortList;
        }
        //Manual Searching e.g for string-based columns i.e. anything not indexed (add more params if required)
        private bool Match(string name, CVersion obj)
        {
            if (!string.IsNullOrEmpty(name)) //Match any string column
            {
                if (null != obj.VersionName && obj.VersionName.ToLower().Contains(name))   return true;
                return false;   //If filter is active, reject any items that dont match
            }
            return true;    //No active filters (should catch this in step #4)
        }
        #endregion

        #region Cloning
        public CVersionList Clone(CDataSrc target) //, int parentId)
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
                    CVersionList clone = Clone(target, tx); //, parentId);
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
        public CVersionList Clone(CDataSrc target, IDbTransaction txOrNull) //, int parentId)
        {
            CVersionList list = new CVersionList(this.Count);
            foreach (CVersion i in this)
                list.Add(i.Clone(target, txOrNull)); //, parentId));  *Child entities must reference the new parent
            return list;
        }
        #endregion
        
        #region Export to Csv
        //Web - Need to add a project reference to System.Web, or comment out these two methods
        public void ExportToCsv(HttpResponse response) { ExportToCsv(response, "Versions.csv"); }
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
            string[] headings = new string[] {"VersionId", "VersionAppId", "VersionName", "VersionTotalBytes", "VersionSchemaMD5",  "VersionCreated", "VersionExpires"};
            CDataSrc.ExportToCsv(headings, sw);
            foreach (CVersion i in this)
            {
                object[] data = new object[] {i.VersionId, i.VersionAppId, i.VersionName, i.VersionTotalBytes, i.VersionSchemaMD5,  i.VersionCreated, i.VersionExpires};
                CDataSrc.ExportToCsv(data, sw);
            }
        }
        #endregion
    }
}
