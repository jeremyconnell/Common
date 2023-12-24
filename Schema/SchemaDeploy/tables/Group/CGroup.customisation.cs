using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{

    //Table-Mapping Class (Customisable half)
    public partial class CGroup
    {
        #region Constants
        #endregion

        #region Constructors (Public)
        //Default Connection String
        public CGroup() : base() {}

        //Alternative Connection String
        public CGroup(CDataSrc dataSrc) : base(dataSrc) {}
        
        //Hidden  (UI code should use cache instead)
        protected internal CGroup(int groupId) : base(groupId) {}
        protected internal CGroup(CDataSrc dataSrc, int groupId) : base(dataSrc, groupId) {}
        protected internal CGroup(CDataSrc dataSrc, int groupId, IDbTransaction txOrNull) : base(dataSrc, groupId, txOrNull) { }
        #endregion

        #region Default Values
        protected override void InitValues_Custom()
        {
            //_sampleDateCreated = DateTime.Now;
            //_sampleSortOrder   = 0;
        }
        #endregion
        
        #region Default Connection String
        protected override CDataSrc DefaultDataSrc()  { return CDataSrc.Default;  }
        #endregion

        #region Properties - Relationships
        //Relationships - Foriegn Keys (e.g parent)

        //Relationships - Collections (e.g. children)
        public CKeyList Keys { get { return CKey.Cache.GetByGroupId(this.GroupId); } }
        protected CKeyList Keys_(IDbTransaction tx) { return new CKey(DataSrc).SelectByGroupId(this.GroupId, tx); } //Only used for cascade deletes
        public int KeyCount {  get { return Keys.Count; } }
        #endregion

        #region Properties - Customisation
        //Derived/ReadOnly (e.g. xml classes, presentation logic)
        public string NameAndCount {  get { return CUtilities.NameAndCount(GroupName, Keys); } }
        #endregion

        #region Save/Delete Overrides
        //Can Override base.Save/Delete (e.g. Cascade deletes, or insert related records)
        #endregion

        #region Custom Database Queries
        //(Not normally required for cached classes, use list class for searching etc)
        //For Stored Procs can use: MakeList (matching schema), or DataSrc.ExecuteDataset (reports etc)
        //For Dynamic sql, can use: SelectSum, SelectDistinct, SelectCount, SelectWhere (inherited methods)
        #endregion

        #region Searching (Optional)
        //For cached classes, custom seach logic resides in static methods on the list class
        // e.g. CGroup.Cache.Search("...")

        //See also the auto-generated methods based on indexes
        //' e.g. CGroup.Cache.GetBy...
        #endregion

        #region Caching Details
        //Cache Key
        internal static string CACHE_KEY = typeof(CGroup).ToString();    //TABLE_NAME

        //Cache data
        private static CGroupList LoadCache()  {   return new CGroup().SelectAll();   }
        //Cache Timeout
        private static void SetCache(CGroupList value)
        {
            if (null != value)  
                value.Sort(); 
            CCache.Set(CACHE_KEY, value);    //Optional parameter can override timeout (otherwise uses config-settings, which default to 3hrs)
        }
        //Helper Method
        private CGroup CacheGetById(CGroupList list)  { return list.GetById(this.GroupId);    }
        #endregion

        #region Cloning
        public CGroup Clone(CDataSrc target, IDbTransaction txOrNull) //, int parentId)
        {
            //Shallow copy: Copies the immediate record, excluding autogenerated Pks
            CGroup copy = new CGroup(this, target);

            //Deep Copy - Child Entities: Cloned children must reference their cloned parent
            //copy.SampleParentId = parentId;

            copy.Save(txOrNull);

            //Deep Copy - Parent Entities: Cloned parents also clone their child collections
            //this.Children.Clone(target, txOrNull, copy.GroupId);

            return copy;
        }
        #endregion

        #region ToXml
        protected override void ToXml_Custom(System.Xml.XmlWriter w)
        {
            //Store(w, "Example", this.Example)
        }
        #endregion
    }
}