using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.IO;

using Framework;

namespace SchemaDeploy
{
    //Collection Class (Customisable half)
    public partial class CReportHistoryList
    {
        #region Filters
        #endregion

        #region Aggregation
        #endregion
        
        #region Searching (Optional)
        //Represents a simple search box to search PK and any string columns (add overloads as required, based on the pattern below)
        //e.g. public CReportHistoryList Search(string nameOrId, int instanceId, int initialVersionId) { ...
        public CReportHistoryList Search(string nameOrId)
        {
            //1. Normalisation
            nameOrId = (nameOrId??string.Empty).Trim().ToLower();
            
            //2. Start with a complete list
            CReportHistoryList results = this;
            
            //3. Use any available index, such as those generated for fk/bool columns
            //Normal Case - non-unique index (e.g. foreign key)
            //if (int.MinValue != instanceId) results = results.GetByInstanceId(instanceId);
            //if (int.MinValue != initialVersionId) results = results.GetByInitialVersionId(initialVersionId);

            //Special case - unique index (e.g. primary key)
            /*
            if (!string.IsNullOrEmpty(nameOrId)) 
            {
                int id;
                if (int.TryParse(nameOrId, out id))
                {
                    CReportHistory obj = this.GetById(id);
                    if (null != obj)
                    {
                        results = new CReportHistoryList(1);
                        results.Add(obj);
                        return results;
                    }
                }
            }
            */
            
            //4. Exit early if remaining (non-index) filters are blank
            if (string.IsNullOrEmpty(nameOrId)) return results; 
            
            //5. Manually search each record using custom match logic, building a shortlist
            CReportHistoryList shortList = new CReportHistoryList();
            foreach (CReportHistory i in results)
                if (Match(nameOrId, i))
                    shortList.Add(i); 
            return shortList;
        }
        //Manual Searching e.g for string-based columns i.e. anything not indexed (add more params if required)
        private bool Match(string name, CReportHistory obj)
        {
            if (!string.IsNullOrEmpty(name)) //Match any string column
            {
                return false;   //If filter is active, reject any items that dont match
            }
            return true;    //No active filters (should catch this in step #4)
        }
        #endregion

        #region Cloning
        public CReportHistoryList Clone(CDataSrc target) //, int parentId)
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
                    CReportHistoryList clone = Clone(target, tx); //, parentId);
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
        public CReportHistoryList Clone(CDataSrc target, IDbTransaction txOrNull) //, int parentId)
        {
            CReportHistoryList list = new CReportHistoryList(this.Count);
            foreach (CReportHistory i in this)
                list.Add(i.Clone(target, txOrNull)); //, parentId));  *Child entities must reference the new parent
            return list;
        }
        #endregion
        
        #region Export to Csv
        //Note: For non-cached classes like this, should normally use CDataSrc.ExportToCsv(SelectWhere_DataSet)

        //Web 
        //Web - Need to add a project reference to System.Web, or comment out these two methods
        public void ExportToCsv(HttpResponse response) { ExportToCsv(response, "ReportHistories.csv"); }
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
            string[] headings = new string[] {"ReportId", "ReportInstanceId", "ReportInitialVersionId", "ReportInitialSchemaMD5", "ReportAppStarted", "ReportAppStopped"};
            CDataSrc.ExportToCsv(headings, sw);
            foreach (CReportHistory i in this)
            {
                object[] data = new object[] {i.ReportId, i.ReportInstanceId, i.ReportInitialVersionId, i.ReportInitialSchemaMD5, i.ReportAppStarted, i.ReportAppStopped};
                CDataSrc.ExportToCsv(data, sw);
            }
        }
        #endregion

    }
}
