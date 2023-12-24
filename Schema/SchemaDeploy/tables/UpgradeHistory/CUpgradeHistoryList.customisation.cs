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
    public partial class CUpgradeHistoryList
    {
        #region Filters
        #endregion

        #region Aggregation
        #endregion
        
        #region Searching (Optional)
        //Represents a simple search box to search PK and any string columns (add overloads as required, based on the pattern below)
        //e.g. public CUpgradeHistoryList Search(string nameOrId, int reportId, int newVersionId) { ...
        public CUpgradeHistoryList Search(string nameOrId)
        {
            //1. Normalisation
            nameOrId = (nameOrId??string.Empty).Trim().ToLower();
            
            //2. Start with a complete list
            CUpgradeHistoryList results = this;
            
            //3. Use any available index, such as those generated for fk/bool columns
            //Normal Case - non-unique index (e.g. foreign key)
            //if (int.MinValue != reportId) results = results.GetByReportId(reportId);
            //if (int.MinValue != newVersionId) results = results.GetByNewVersionId(newVersionId);

            //Special case - unique index (e.g. primary key)
            /*
            if (!string.IsNullOrEmpty(nameOrId)) 
            {
                int id;
                if (int.TryParse(nameOrId, out id))
                {
                    CUpgradeHistory obj = this.GetById(id);
                    if (null != obj)
                    {
                        results = new CUpgradeHistoryList(1);
                        results.Add(obj);
                        return results;
                    }
                }
            }
            */
            
            //4. Exit early if remaining (non-index) filters are blank
            if (string.IsNullOrEmpty(nameOrId)) return results; 
            
            //5. Manually search each record using custom match logic, building a shortlist
            CUpgradeHistoryList shortList = new CUpgradeHistoryList();
            foreach (CUpgradeHistory i in results)
                if (Match(nameOrId, i))
                    shortList.Add(i); 
            return shortList;
        }
        //Manual Searching e.g for string-based columns i.e. anything not indexed (add more params if required)
        private bool Match(string name, CUpgradeHistory obj)
        {
            if (!string.IsNullOrEmpty(name)) //Match any string column
            {
                return false;   //If filter is active, reject any items that dont match
            }
            return true;    //No active filters (should catch this in step #4)
        }
        #endregion

        #region Cloning
        public CUpgradeHistoryList Clone(CDataSrc target) //, int parentId)
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
                    CUpgradeHistoryList clone = Clone(target, tx); //, parentId);
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
        public CUpgradeHistoryList Clone(CDataSrc target, IDbTransaction txOrNull) //, int parentId)
        {
            CUpgradeHistoryList list = new CUpgradeHistoryList(this.Count);
            foreach (CUpgradeHistory i in this)
                list.Add(i.Clone(target, txOrNull)); //, parentId));  *Child entities must reference the new parent
            return list;
        }
        #endregion
        
        #region Export to Csv
        //Note: For non-cached classes like this, should normally use CDataSrc.ExportToCsv(SelectWhere_DataSet)

        //Web 
        //Web - Need to add a project reference to System.Web, or comment out these two methods
        public void ExportToCsv(HttpResponse response) { ExportToCsv(response, "UpgradeHistories.csv"); }
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
            string[] headings = new string[] {"ChangeId", "ChangeReportId", "ChangeNewVersionId", "ChangeNewSchemaMD5", "ChangeStarted", "ChangeFinished"};
            CDataSrc.ExportToCsv(headings, sw);
            foreach (CUpgradeHistory i in this)
            {
                object[] data = new object[] {i.ChangeId, i.ChangeReportId, i.ChangeNewVersionId, i.ChangeNewSchemaMD5, i.ChangeStarted, i.ChangeFinished};
                CDataSrc.ExportToCsv(data, sw);
            }
        }
        #endregion

        #region Preload Parent Objects
        //Efficiency Adjustment: Preloads the common parent for the whole list, to avoid database chatter 
        public CReportHistory ReportHistory { set { foreach (CUpgradeHistory i in this) i.ReportHistory = value; } }
        public CBinaryFile NewSchema { set { foreach (CUpgradeHistory i in this) i.NewSchema = value; } }
        #endregion

    }
}
