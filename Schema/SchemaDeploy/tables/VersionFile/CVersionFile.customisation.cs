using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
    //Table-Mapping Class (Customisable half)
    public partial class CVersionFile
    {
        #region Constants
        public const string JOIN_VERSION = TABLE_NAME + " INNER JOIN " + CVersion.TABLE_NAME + " ON VFVersionId=VersionId";
        #endregion

        #region Constructors (Public)
        //Default Connection String
        public CVersionFile() : base() { }

        //Alternative Connection String
        public CVersionFile(CDataSrc dataSrc) : base(dataSrc) { }

        //Hidden  (UI code should use cache instead)
        protected internal CVersionFile(int vFVersionId) : base(vFVersionId) { }
        protected internal CVersionFile(CDataSrc dataSrc, int vFVersionId) : base(dataSrc, vFVersionId) { }
        protected internal CVersionFile(CDataSrc dataSrc, int vFVersionId, IDbTransaction txOrNull) : base(dataSrc, vFVersionId, txOrNull) { }
        #endregion

        #region Default Values
        protected override void InitValues_Custom()
        {
            //_sampleDateCreated = DateTime.Now;
            //_sampleSortOrder   = 0;
        }
        #endregion

        #region Default Connection String
        protected override CDataSrc DefaultDataSrc() { return CDataSrc.Default; }
        #endregion

        #region Properties - Relationships
        //Relationships - Foriegn Keys (e.g parent)
        public CVersion Version { get { return CVersion.Cache.GetById(this.VFVersionId); } }
        public CBinaryFile BinaryFile { get { return CBinaryFile.Cache.GetById(this.VFBinaryMD5); } }

        //Relationships - Collections (e.g. children)
        #endregion

        #region Properties - Customisation
        public string VersionAndPath {  get { return string.Concat(Version.VersionName, ": ", VFPath); } }
        #endregion

        #region Save/Delete Overrides
        public override void Save(IDbTransaction txOrNull)
        {
            if (string.IsNullOrEmpty(VFPath))
                VFPath = BinaryFile.Path;
            VFPath = VFPath.Replace("\\", "/");

            base.Save(txOrNull);
        }
        #endregion

        #region Custom Database Queries
        //(Not normally required for cached classes, use list class for searching etc)
        //For Stored Procs can use: MakeList (matching schema), or DataSrc.ExecuteDataset (reports etc)
        //For Dynamic sql, can use: SelectSum, SelectDistinct, SelectCount, SelectWhere (inherited methods)
        #endregion

        #region Searching (Optional)
        //For cached classes, custom seach logic resides in static methods on the list class
        // e.g. CVersionFile.Cache.Search("...")

        //See also the auto-generated methods based on indexes
        //' e.g. CVersionFile.Cache.GetBy...
        public List<int> VersionIdsForAppId(int appId, IDbTransaction tx)
        {
            return DataSrc.MakeListInteger(new CSelectWhere("VersionId", JOIN_VERSION, new CCriteria("VersionAppId", appId), null, tx));
        }
        public int DeleteForAppId(int appId, IDbTransaction tx)
        {
            var versionIds = VersionIdsForAppId(appId, tx);
            return DeleteWhere("VFVersionId", ESign.IN, versionIds, tx);
        }
        #endregion

        #region Caching Details
        //Cache Key
        internal static string CACHE_KEY = typeof(CVersionFile).ToString();    //TABLE_NAME

        //Cache data
        private static CVersionFileList LoadCache()  {   return new CVersionFile().SelectAll();   }
        //Cache Timeout
        private static void SetCache(CVersionFileList value)
        {
            if (null != value)  
                value.Sort(); 
            CCache.Set(CACHE_KEY, value);    //Optional parameter can override timeout (otherwise uses config-settings, which default to 3hrs)
        }
        //Helper Method
        private CVersionFile CacheGetById(CVersionFileList list)  { return list.GetById(this.VFId);    }
        #endregion

        #region Cloning
        public CVersionFile Clone(CDataSrc target, IDbTransaction txOrNull) //, int parentId)
        {
            //Shallow copy: Copies the immediate record, excluding autogenerated Pks
            CVersionFile copy = new CVersionFile(this, target);

            //Deep Copy - Child Entities: Cloned children must reference their cloned parent
            //copy.SampleParentId = parentId;

            copy.Save(txOrNull);

            //Deep Copy - Parent Entities: Cloned parents also clone their child collections
            //this.Children.Clone(target, txOrNull, copy.VFVersionId);

            return copy;
        }
        #endregion

    }
}