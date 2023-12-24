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
    public partial class CVersionFileList
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
                    list.Add(i.VersionAndPath);
                return list;
            }
        }
        public long TotalBytes
        {
            get
            {
                long b = 0;
                foreach (var i in this)
                    b += i.BinaryFile.Size;
                return b;
            }
        }

        public DateTime MaxExpiresDate
        {
            get
            {
                var d = DateTime.MinValue;
                foreach (var i in this)
                {
                    var vfd = i.Version.ReleasesBranch.MaxExpiresDate;
                    if (vfd > d)
                        d = vfd;
                }
                return d;
            }
        }
        #endregion


        #region Members
        private CVersionList _versions;
        private List<int> _versionIds;

        private CBinaryFileList _binaryFiles;
        private List<Guid> _binaryFileIds;

        private CFolderHash _folderHash;
        #endregion


        public CFolderHash FolderHash
        {
            get
            {
                if (null == _folderHash || this.Count != _folderHash.Count)
                {
                    var dict = new Dictionary<string, Guid>();
                    foreach (var i in this)
                        dict.Add(i.VFPath, i.VFBinaryMD5);
                    _folderHash = new CFolderHash(dict);
                }
                return _folderHash;
            }
        }
        public CFilesList Diff(CVersionFileList target)
        {
            var list = new CFilesList();

            //Deletes
            foreach (var i in this.Paths)
                if (!target.HasPath(i))
                    list.Add(new CFileNameAndContent(i));

            //Inserts, Updates
            foreach (var i in target.IndexByPath)
                if (!this.HasPath(i.Key) || this.GetByPath(i.Key).VFBinaryMD5 != i.Value.VFBinaryMD5)
                    list.Add(new CFileNameAndContent(i.Key, i.Value.BinaryFile.GetFile()));
            return list;
        }
        public CFilesList Diff(CFolderHash old)
        {
            var list = new CFilesList();

            //Deletes
            foreach (var i in old.Names)
                if (!this.HasPath(i))
                    list.Add(new CFileNameAndContent(i));

            //Inserts, Updates
            foreach (var i in this.IndexByPath)
                if (!old.Has(i.Key) || old.GetFile(i.Key).MD5 != i.Value.VFBinaryMD5)
                    list.Add(new CFileNameAndContent(i.Key, i.Value.BinaryFile.GetFile()));
            return list;
        }
        public CFilesList Diff(CFilesList old)
        {
            var list = new CFilesList();

            //Deletes
            foreach (var i in old)
                if (!this.HasPath(i.Name))
                    list.Add(new CFileNameAndContent(i.Name));

            //Inserts, Updates
            foreach (var i in this.IndexByPath)
                if (!old.Has(i.Key) || old.GetFile(i.Key).Md5 != i.Value.VFBinaryMD5)
                    list.Add(new CFileNameAndContent(i.Key, i.Value.BinaryFile.GetFile()));
            return list;
        }


        #region BulkSave
        public void TrimCommonPathPrefix()
        {
            string prefix = null;
            foreach (var i in this)
            {
                if (null == prefix)
                {
                    prefix = i.VFPath;
                    continue;
                }

                int j = 0;
                while (j < prefix.Length && j < i.VFPath.Length && prefix[j] == i.VFPath[j])
                    j++;
                prefix = prefix.Substring(0, j);
            }
            if (null != prefix && prefix.Length > 0)
                foreach (var i in this)
                    i.VFPath = i.VFPath.Substring(prefix.Length);
        }
        public void BulkInsert(CDataSrc db = null)
        {
            if (this.Count == 0)
                return;

            if (db == null)
                db = this[0].DataSrc;

            var dt = this.ToDataTable();

            CVersionFile.Cache = null;
            db.BulkInsertOffsetBy1(dt, CVersionFile.TABLE_NAME);
            CVersionFile.Cache = null;
        }
        public DataTable ToDataTable()
        {
            var dt = new DataTable();

            dt.TableName = CVersionFile.TABLE_NAME;

            dt.Columns.Add("VFVersionId", typeof(int));
            dt.Columns.Add("VFBinaryMD5", typeof(Guid));
            dt.Columns.Add("VFPath", typeof(string));

            foreach (var i in this)
                dt.Rows.Add(i.VFVersionId, i.VFBinaryMD5, i.VFPath);

            return dt;
        }
        #endregion


        #region Resolve Associative table (and sort)
        public CVersionList Versions
        {
            get
            {
                if (_versions == null)
                {
                    lock (this)
                    {
                        if (null == _versions)
                        {
                            _versions = new CVersionList(this.Count);
                            foreach (CVersionFile i in this)
                                if (null == _versions.GetById(i.VFVersionId))
                                    _versions.Add(i.Version);
                            _versions.Sort();
                        }
                    }
                }
                return _versions;
            }
        }
        public CVersionList RemainingVersions(string search, int appId)
        {
            CVersionList temp = new CVersionList(CVersion.Cache.Search(search, appId));
            temp.Remove(this.Versions);
            return temp;
        }


        public CBinaryFileList BinaryFiles
        {
            get
            {
                if (_binaryFiles == null)
                {
                    lock (this)
                    {
                        if (null == _binaryFiles)
                        {
                            _binaryFiles = new CBinaryFileList(this.Count);
                            foreach (CVersionFile i in this)
                                if (! _binaryFiles.Contains(i.BinaryFile))
                                    _binaryFiles.Add(i.BinaryFile);
                            _binaryFiles.Sort();
                        }
                    }
                }
                return _binaryFiles;
            }
        }
        public CBinaryFileList RemainingBinaryFiles(string search)
        {
            CBinaryFileList temp = new CBinaryFileList(CBinaryFile.Cache.Search(search));
            temp.Remove(this.BinaryFiles);
            return temp;
        }
        #endregion

        #region Resolve/Isolate PKs
        public List<int> VersionIds
        {
            get
            {
                if (null == _versionIds)
                {
                    lock (this)
                    {
                        if (null == _versionIds)
                        {
                            _versionIds = new List<int>(this.Count);
                            foreach (CVersionFile i in this)
                                _versionIds.Add(i.VFVersionId);
                        }
                    }
                }
                return _versionIds;
            }
        }
        public List<Guid> BinaryFileIds
        {
            get
            {
                if (null == _binaryFileIds)
                {
                    lock (this)
                    {
                        if (null == _binaryFileIds)
                        {
                            _binaryFileIds = new List<Guid>(this.Count);
                            foreach (CVersionFile i in this)
                                _binaryFileIds.Add(i.VFBinaryMD5);
                        }
                    }
                }
                return _binaryFileIds;
            }
        }
        #endregion

        #region Searching (Optional)
        //Represents a simple search box to search PK and any string columns (add overloads as required, based on the pattern below)
        //e.g. public CVersionFileList Search(string nameOrId, Guid binaryMD5) { ...
        public CVersionFileList Search(string nameOrId = null, bool? isSchema = null)
        {
            //1. Normalisation
            nameOrId = (nameOrId??string.Empty).Trim().ToLower();
            
            //2. Start with a complete list
            CVersionFileList results = this;

            if (null != isSchema)
                results = results.GetByIsSchema(isSchema.Value);

            
            //4. Exit early if remaining (non-index) filters are blank
            if (string.IsNullOrEmpty(nameOrId)) return results; 
            
            //5. Manually search each record using custom match logic, building a shortlist
            CVersionFileList shortList = new CVersionFileList();
            foreach (CVersionFile i in results)
                if (Match(nameOrId, i))
                    shortList.Add(i); 
            return shortList;
        }
        //Manual Searching e.g for string-based columns i.e. anything not indexed (add more params if required)
        private bool Match(string name, CVersionFile obj)
        {
            if (!string.IsNullOrEmpty(name)) //Match any string column
            {
                if (null != obj.VFPath && obj.VFPath.ToLower().Contains(name))   return true;
                return false;   //If filter is active, reject any items that dont match
            }
            return true;    //No active filters (should catch this in step #4)
        }
        #endregion

        #region Cloning
        public CVersionFileList Clone(CDataSrc target) //, int parentId)
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
                    CVersionFileList clone = Clone(target, tx); //, parentId);
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
        public CVersionFileList Clone(CDataSrc target, IDbTransaction txOrNull) //, int parentId)
        {
            CVersionFileList list = new CVersionFileList(this.Count);
            foreach (CVersionFile i in this)
                list.Add(i.Clone(target, txOrNull)); //, parentId));  *Child entities must reference the new parent
            return list;
        }
        #endregion
        
        #region Export to Csv
        //Web - Need to add a project reference to System.Web, or comment out these two methods
        public void ExportToCsv(HttpResponse response) { ExportToCsv(response, "VersionFiles.csv"); }
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
            string[] headings = new string[] {"VFId", "VFVersionId", "VFBinaryMD5", "VFPath"};
            CDataSrc.ExportToCsv(headings, sw);
            foreach (CVersionFile i in this)
            {
                object[] data = new object[] {i.VFId, i.VFVersionId, i.VFBinaryMD5, i.VFPath};
                CDataSrc.ExportToCsv(data, sw);
            }
        }
        #endregion


        #region Caching

        //Main Logic
        public new void Add(CVersionFile item)
        {
            if (null != _index && !_index.ContainsKey(item.VFVersionId))
                _index[item.VFVersionId] = item;

            _indexByBinaryMD5 = null;
            _indexByVersionId = null;

            base.Add(item);
        }
        public new void Remove(CVersionFile item)
        {
            if (null == item)
                return;
            if (null != _index && _index.ContainsKey(item.VFVersionId))
                _index.Remove(item.VFVersionId);

            _indexByBinaryMD5 = null;
            _indexByVersionId = null;

            base.Remove(item);
        }
        #endregion


        //Index by VFPath
        public List<string> Paths {  get { return new List<string>(IndexByPath.Keys); } }
        public bool HasPath(string path) { return IndexByPath.ContainsKey(path.ToLower()); }
        public CVersionFile GetByPath(string path)
        {
            path = path.ToLower();
            CVersionFile temp = null;
            IndexByPath.TryGetValue(path, out temp);
            return temp;
        }

        [NonSerialized]
        private Dictionary<string, CVersionFile> _indexByPath;
        private Dictionary<string, CVersionFile> IndexByPath
        {
            get
            {
                if (null == _indexByPath || _indexByPath.Count != this.Count)
                {
                    var index = new Dictionary<string, CVersionFile>();
                    foreach (CVersionFile i in this)
                        index.Add(i.VFPath.ToLower(), i);
                    _indexByPath = index;
                }
                return _indexByPath;
            }
        }



        //Index by VFVersionId
        public CVersionFileList GetByIsSchema(bool isSchema)
        {
            CVersionFileList temp = null;
            if (!IndexByIsSchema.TryGetValue(isSchema, out temp))
            {
                temp = new CVersionFileList();
                IndexByIsSchema.Add(isSchema, temp);
            }
            return temp;
        }

        [NonSerialized]
        private Dictionary<bool, CVersionFileList> _indexByIsSchema;
        private Dictionary<bool, CVersionFileList> IndexByIsSchema
        {
            get
            {
                if (null == _indexByIsSchema)
                {
                    var index = new Dictionary<bool, CVersionFileList>();
                    CVersionFileList temp = null;
                    foreach (CVersionFile i in this)
                    {
                        if (!index.TryGetValue(i.BinaryFile.IsSchema, out temp))
                        {
                            temp = new CVersionFileList();
                            index[i.BinaryFile.IsSchema] = temp;
                        }
                        temp.Add(i);
                    }
                    _indexByIsSchema = index;
                }
                return _indexByIsSchema;
            }
        }

    }
}
