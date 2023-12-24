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
    public partial class CBinaryFileList
    {
        #region Filters
        #endregion

        #region Aggregation
        public Guid TotalHash {  get { this.Sort(); return CBinary.MD5_(this.Ids.ToArray()); } }

        public long TotalBytes
        {
            get
            {
                long b = 0;
                foreach (var i in this)
                    b += i.Size;
                return b;
            }
        }
        #endregion

        #region Searching (Optional)
        //Represents a simple search box to search PK and any string columns (add overloads as required, based on the pattern below)
        //e.g. public CBinaryFileList Search(string nameOrId, bool? isSchema) { ...
        public CBinaryFileList Search(string nameOrId, bool? isSchema = null)
        {
            //1. Normalisation
            nameOrId = (nameOrId??string.Empty).Trim().ToLower();
            
            //2. Start with a complete list
            CBinaryFileList results = this;
            
            //3. Use any available index, such as those generated for fk/bool columns
            //Normal Case - non-unique index (e.g. foreign key)
            if (isSchema.HasValue)
                results = results.GetByIsSchema(isSchema.Value); //Customise bool filters according to UI (e.g. for checkbox, use simple bool and bias in one direction)

            //Special case - unique index (e.g. primary key)
            /*
            if (!string.IsNullOrEmpty(nameOrId)) 
            {
                int id;
                if (int.TryParse(nameOrId, out id))
                {
                    CBinaryFile obj = this.GetById(id);
                    if (null != obj)
                    {
                        results = new CBinaryFileList(1);
                        results.Add(obj);
                        return results;
                    }
                }
            }
            */
            
            //4. Exit early if remaining (non-index) filters are blank
            if (string.IsNullOrEmpty(nameOrId)) return results; 
            
            //5. Manually search each record using custom match logic, building a shortlist
            CBinaryFileList shortList = new CBinaryFileList();
            foreach (CBinaryFile i in results)
                if (Match(nameOrId, i))
                    shortList.Add(i); 
            return shortList;
        }
        //Manual Searching e.g for string-based columns i.e. anything not indexed (add more params if required)
        private bool Match(string name, CBinaryFile obj)
        {
            if (!string.IsNullOrEmpty(name)) //Match any string column
            {
                if (null != obj.Path && obj.Path.ToLower().Contains(name))   return true;
                return false;   //If filter is active, reject any items that dont match
            }
            return true;    //No active filters (should catch this in step #4)
        }
        #endregion

        #region Cloning
        public CBinaryFileList Clone(CDataSrc target) //, int parentId)
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
                    CBinaryFileList clone = Clone(target, tx); //, parentId);
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
        public CBinaryFileList Clone(CDataSrc target, IDbTransaction txOrNull) //, int parentId)
        {
            CBinaryFileList list = new CBinaryFileList(this.Count);
            foreach (CBinaryFile i in this)
                list.Add(i.Clone(target, txOrNull)); //, parentId));  *Child entities must reference the new parent
            return list;
        }
        #endregion


        //Index-caching Logic
        public new void Add(CBinaryFile item)
        {
            this.Add_(item);
            _indexByIsSchema = null;
        }
        public new void Remove(CBinaryFile item)
        {
            this.Remove_(item);
            _indexByIsSchema = null;
        }

        #region Export to Csv
        //Web - Need to add a project reference to System.Web, or comment out these two methods
        public void ExportToCsv(HttpResponse response) { ExportToCsv(response, "Binaries.csv"); }
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
            string[] headings = new string[] {"MD5", "Path",  "Size", "IsSchema", "Created", "Deleted"};
            CDataSrc.ExportToCsv(headings, sw);
            foreach (CBinaryFile i in this)
            {
                object[] data = new object[] {i.MD5, i.Path, i.Size, i.IsSchema, i.Created, i.Deleted};
                CDataSrc.ExportToCsv(data, sw);
            }
        }
        #endregion


        public CBinaryFileList DeleteIfExpiredFor(int days)
        {
            var d = DateTime.Now.AddDays(-days);
            var kill = new CBinaryFileList();
            foreach (var bf in this)
                if (bf.Deleted == DateTime.MinValue)
                    if (bf.VersionFiles.MaxExpiresDate > d)
                        kill.Add(bf);

            foreach (var i in kill)
            {
                i.FileAsBytes = null;
                i.Delete(); //sets the binary file to null, deleted = now
            }
            return kill;
        }
    }
}
