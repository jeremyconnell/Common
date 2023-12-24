using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
    //Table-Mapping Class (Customisable half)
    public partial class CVersion
    {
        #region Constants
        #endregion

        #region Constructors (Public)
        //Default Connection String
        public CVersion() : base() {}

        //Alternative Connection String
        public CVersion(CDataSrc dataSrc) : base(dataSrc) {}
        
        //Hidden  (UI code should use cache instead)
        protected internal CVersion(int versionId) : base(versionId) {}
        protected internal CVersion(CDataSrc dataSrc, int versionId) : base(dataSrc, versionId) {}
        protected internal CVersion(CDataSrc dataSrc, int versionId, IDbTransaction txOrNull) : base(dataSrc, versionId, txOrNull) { }
        #endregion

        #region Default Values
        protected override void InitValues_Custom()
        {
            _versionCreated = DateTime.Now;
            _versionTotalBytes = 0;
        }
        #endregion
        
        #region Default Connection String
        protected override CDataSrc DefaultDataSrc()  { return CDataSrc.Default;  }
        #endregion

        #region Properties - Relationships
        //Relationships - Foriegn Keys (e.g parent)
        public CApp App { get { return CApp.Cache.GetById(this.VersionAppId); } }
        public CBinaryFile Schema { get { return CBinaryFile.Cache.GetById(this.VersionSchemaMD5); } }


        //Relationships - Collections (e.g. children)
        //public CInstanceList Usage
        //{
        //    get
        //    {
        //        var join = new CInstanceList();
        //        join.AddRange(BranchInstances);
        //        join.AddRange(App.Instances.MainBranch);
        //        return join;
        //    }
        //}

        public CInstanceList BranchInstances                     {  get { return CInstance.Cache.GetBySpecialVersionId(          this.VersionId); }  }
        protected CInstanceList BranchInstances_(IDbTransaction tx) {        return new CInstance(DataSrc).SelectBySpecialVersionId(this.VersionId, tx); } //Only used for cascade deletes

        public    CVersionFileList VersionFiles                     {  get { return CVersionFile.Cache.GetByVersionId(          this.VersionId); }  }
        protected CVersionFileList VersionFiles_(IDbTransaction tx) {        return new CVersionFile(DataSrc).SelectByVersionId(this.VersionId, tx); } //Only used for cascade deletes

        public    CReleaseList ReleasesAll                        {  get { return CRelease.Cache.GetByVersionIds(         this.VersionId, int.MinValue); }  }
        public    CReleaseList ReleasesBranch                     {  get { return CRelease.Cache.GetByVersionId(          this.VersionId); }  }
        protected CReleaseList ReleasesBranch_(IDbTransaction tx) {        return new CRelease(DataSrc).SelectByVersionId(this.VersionId, tx); } //Only used for cascade deletes

        public   CUpgradeHistoryList Changes()                  {   return new CUpgradeHistory(DataSrc).SelectByNewVersionId(     this.VersionId    ); } 
        public   CUpgradeHistoryList Changes(CPagingInfo pi)    {   return new CUpgradeHistory(DataSrc).SelectByNewVersionId(pi,  this.VersionId    ); } 
        internal CUpgradeHistoryList Changes_(IDbTransaction tx){   return new CUpgradeHistory(DataSrc).SelectByNewVersionId(     this.VersionId, tx); } 
        public   int ChangesCount()                             {   return new CUpgradeHistory(DataSrc).SelectCountByNewVersionId(this.VersionId    ); }


        //Relationships - 2-Step Walk (On-Demand) 
        public CBinaryFileList BinaryFiles {  get { return VersionFiles.BinaryFiles; } }
        public CBinaryFileList BinaryFiles_WithFile() { return new CBinaryFile(DataSrc).SelectByVersionId_WithFile(this.VersionId); }

        #endregion

        #region Properties - Customisation
        public string NameAndFileCount {  get { return CUtilities.NameAndCount(VersionName, VersionFiles); } }
        //Derived/ReadOnly (e.g. xml classes, presentation logic)
        public string VersionFilesB64 { get { return CBinary.ToBase64(VersionFilesMD5, 10); } }
        public string VersionSchemaB64 {  get { return CBinary.ToBase64(VersionSchemaMD5, 10); } }
        public string VersionTotalFileSize { get { return CUtilities.FileSize(VersionTotalBytes); } }
        public string VersionFileCount_ { get { return CUtilities.CountSummary(VersionFiles, "file"); } }
        public string AppNameAndVersion {  get { return string.Concat(App.AppName, ": ", VersionName); } }
        public int BranchCount { get { return BranchInstances.Count; } }
        public int MainCount { get { return App.AppMainVersionId == this.VersionId ? App.Instances.MainBranch.Count : 0; } }
        public string BranchUsage_ { get { return CUtilities.CountSummary(BranchCount, "deploy"); } }
        public string MainUsage_ { get { return CUtilities.CountSummary(MainCount, "deploy"); } }
        public string Usage_
        {
            get
            {
                var sb = new StringBuilder();

                if (VersionId == App.AppMainVersionId)
                    sb.Append(CUtilities.NameAndCount("Main", MainCount));

                if (BranchCount > 0)
                    sb.Append("Branch: ").Append(BranchInstances.Names_);

                return sb.ToString();
            }
        }

        public CFolderHash FolderHash {  get { return VersionFiles.FolderHash; } }
        public CFilesList Diff(CVersion target)
        {
            return VersionFiles.Diff(target.VersionFiles);
        }
        public CFilesList Diff(CFolderHash old)
        {
            return VersionFiles.Diff(old);
        }
        public CFilesList Diff(CFilesList old)
        {
            return VersionFiles.Diff(old);
        }
        #endregion

        #region Save/Delete Overrides
        public override void Save(IDbTransaction txOrNull)
        {
            var vv = App.Versions;

            if (string.IsNullOrEmpty(VersionName))
                VersionName = "Version #" + (1 + vv.Count).ToString();

            //if (vv.Count > 0)
            //{
            //    var prev = vv[0];

            //    var sameFiles = prev.BinaryFiles.GetByIds(this.BinaryFiles.Ids);
            //    var diffFiles = new CBinaryFileList();
            //    foreach (var i in this.BinaryFiles)
            //        if (sameFiles.GetById(i.MD5) == null)
            //            diffFiles.Add(i);

            //}

            base.Save(txOrNull);
        }
        public override void Delete(IDbTransaction txOrNull)
        {
            //Use a transaction if none supplied 
            if (txOrNull == null)
            {
                BulkDelete(this);
                return;
            }

            //Cascade-Delete (all child collections) 
            //this.BranchInstances_(txOrNull).DeleteAll(txOrNull);
            this.VersionFiles_(txOrNull).DeleteAll(txOrNull);
            this.ReleasesBranch_(txOrNull).DeleteAll(txOrNull);
            this.Changes_(txOrNull).DeleteAll(txOrNull);

            //Normal Delete 
            base.Delete(txOrNull);
        }
        #endregion

        #region Custom Database Queries
        //(Not normally required for cached classes, use list class for searching etc)
        //For Stored Procs can use: MakeList (matching schema), or DataSrc.ExecuteDataset (reports etc)
        //For Dynamic sql, can use: SelectSum, SelectDistinct, SelectCount, SelectWhere (inherited methods)
        #endregion

        #region Searching (Optional)
        //For cached classes, custom seach logic resides in static methods on the list class
        // e.g. CVersion.Cache.Search("...")

        //See also the auto-generated methods based on indexes
        //' e.g. CVersion.Cache.GetBy...
        #endregion

        #region Caching Details
        //Cache Key
        internal static string CACHE_KEY = typeof(CVersion).ToString();    //TABLE_NAME

        //Cache data
        private static CVersionList LoadCache()  {   return new CVersion().SelectAll();   }
        //Cache Timeout
        private static void SetCache(CVersionList value)
        {
            if (null != value)  
                value.Sort(); 
            CCache.Set(CACHE_KEY, value);    //Optional parameter can override timeout (otherwise uses config-settings, which default to 3hrs)
        }
        //Helper Method
        private CVersion CacheGetById(CVersionList list)  { return list.GetById(this.VersionId);    }
        #endregion

        #region Cloning
        public CVersion Clone(CDataSrc target, IDbTransaction txOrNull) //, int parentId)
        {
            //Shallow copy: Copies the immediate record, excluding autogenerated Pks
            CVersion copy = new CVersion(this, target);

            //Deep Copy - Child Entities: Cloned children must reference their cloned parent
            //copy.SampleParentId = parentId;

            copy.Save(txOrNull);

            //Deep Copy - Parent Entities: Cloned parents also clone their child collections
            //this.Children.Clone(target, txOrNull, copy.VersionId);

            return copy;
        }
        #endregion
    }
}