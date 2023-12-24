using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
    //Table-Mapping Class (Customisable half)
    public partial class CKey
    {
        #region Constants
        //Primary Key Values
//        public const string _HUEY   = "HUEY";
//        public const string _DUEY   = "DUEY";
//        public const string _LOUIE  = "LOUIE";
        #endregion

        #region Constructors (Public)
        //Default Connection String
        public CKey() : base() {}

        //Alternative Connection String
        public CKey(CDataSrc dataSrc) : base(dataSrc) {}
        
        //Hidden  (UI code should use cache instead)
        protected internal CKey(string keyName) : base(keyName) {}
        protected internal CKey(CDataSrc dataSrc, string keyName) : base(dataSrc, keyName) {}
        protected internal CKey(CDataSrc dataSrc, string keyName, IDbTransaction txOrNull) : base(dataSrc, keyName, txOrNull) { }
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
        public CGroup Group { get { return CGroup.Cache.GetById(this.KeyGroupId); } }
        public CFormat Format { get { return CFormat.Cache.GetById(this.KeyFormatId); } }

        //Relationships - Collections (e.g. children)
        public    CValueList Values                     {  get { return CValue.Cache.GetByKeyName(          this.KeyName); }  }
        protected CValueList Values_(IDbTransaction tx) {        return new CValue(DataSrc).SelectByKeyName(this.KeyName, tx); } //Only used for cascade deletes


        #endregion

        #region Properties - Customisation
        public string GroupName { get { return null == Group ? string.Empty : Group.GroupName; } }
        public string GroupAndKey {  get { return null == Group ? string.Concat(" ", KeyName) : string.Concat(GroupName, " :: ", KeyName); } }
        //Derived/ReadOnly (e.g. xml classes, presentation logic)
        public string NameAndCount { get { return CUtilities.NameAndCount(CUtilities.Truncate(GroupAndKey, 50), Values); } }
        public EFormat KeyFormatId_ { get { return (EFormat)KeyFormatId; } }
        public string DefaultValue
        {
            get
            {
                switch (KeyFormatId_)
                {
                    case EFormat.Boolean: return KeyDefaultBoolean.ToString();
                    case EFormat.Integer: return int.MinValue == KeyDefaultInteger ? string.Empty : KeyDefaultInteger.ToString();
                    case EFormat.String: return KeyDefaultString ?? string.Empty;
                    default: throw new Exception("New Format: " + KeyFormatId.ToString());
                }
            }
        }
        public int Distinct {  get { return DistinctValues.Count; } }
        public List<string> DistinctValues
        {
            get
            {
                var list = new List<string>();
                foreach (var i in Values)
                {
                    var s = i.ValueAsString;
                    if (!string.IsNullOrEmpty(s))
                        if (!list.Contains(s))
                            list.Add(s);
                }
                return list;
            }
        }

        public void GuessDefault()
        {
            var dict = new Dictionary<string, int>();
            foreach (var v in this.Values)
            {
                var s = v.ValueAsString;
                if (string.IsNullOrEmpty(s))
                    continue;
                if (dict.ContainsKey(s))
                    dict[s] = dict[s] + 1;
                else
                    dict[s] = 1;
            }
            if (dict.Count == 0)
                return;

            var max = string.Empty;
            if (dict.Count == 1)
                foreach (var i in dict)
                    max = i.Key;
            else
            {
                int m = 0;
                foreach (var i in dict)
                    if (i.Value > m)
                    {
                        m = i.Value;
                        max = i.Key;
                    }
            }

            switch (this.KeyFormatId_)
            {
                case EFormat.String:
                    this.KeyDefaultString = max;
                    break;

                case EFormat.Boolean:
                    if (string.IsNullOrEmpty(max))
                        this.KeyDefaultBoolean = null;
                    else
                        this.KeyDefaultBoolean = "true" == max.ToLower();
                    break;

                case EFormat.Integer:
                    if (string.IsNullOrEmpty(max))
                        this.KeyDefaultInteger = int.MinValue;
                    else
                        this.KeyDefaultInteger = int.Parse(max);
                    break;
            }
            this.Save();
        }
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
        // e.g. CKey.Cache.Search("...")

        //See also the auto-generated methods based on indexes
        //' e.g. CKey.Cache.GetBy...
        #endregion

        #region Caching Details
        //Cache Key
        internal static string CACHE_KEY = typeof(CKey).ToString();    //TABLE_NAME

        //Cache data
        private static CKeyList LoadCache()  {   return new CKey().SelectAll();   }
        //Cache Timeout
        private static void SetCache(CKeyList value)
        {
            if (null != value)  
                value.Sort(); 
            CCache.Set(CACHE_KEY, value);    //Optional parameter can override timeout (otherwise uses config-settings, which default to 3hrs)
        }
        //Helper Method
        private CKey CacheGetById(CKeyList list)  { return list.GetById(this.KeyName);    }
        #endregion

        #region Cloning
        public CKey Clone(CDataSrc target, IDbTransaction txOrNull) //, int parentId)
        {
            //Shallow copy: Copies the immediate record, excluding autogenerated Pks
            CKey copy = new CKey(this, target);

            //Deep Copy - Child Entities: Cloned children must reference their cloned parent
            //copy.SampleParentId = parentId;

            copy.Save(txOrNull);

            //Deep Copy - Parent Entities: Cloned parents also clone their child collections
            //this.Children.Clone(target, txOrNull, copy.KeyName);

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