using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
    #region Enum: Primary Key Values
    public enum EFormat
    {
         String = 1,
         Boolean = 2,
         Integer = 3
    }
    #endregion

    //Table-Mapping Class (Customisable half)
    public partial class CFormat
    {
        #region Constants
        #endregion

        #region Constructors (Public)
        //Default Connection String
        public CFormat() : base() {}

        //Alternative Connection String
        public CFormat(CDataSrc dataSrc) : base(dataSrc) {}
        
        //Hidden  (UI code should use cache instead)
        protected internal CFormat(int formatId) : base(formatId) {}
        protected internal CFormat(CDataSrc dataSrc, int formatId) : base(dataSrc, formatId) {}
        protected internal CFormat(CDataSrc dataSrc, int formatId, IDbTransaction txOrNull) : base(dataSrc, formatId, txOrNull) { }
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
        public    CKeyList Keys                     {  get { return CKey.Cache.GetByFormatId(          this.FormatId); }  }
        protected CKeyList Keys_(IDbTransaction tx) {        return new CKey(DataSrc).SelectByFormatId(this.FormatId, tx); } //Only used for cascade deletes
        #endregion

        #region Properties - Customisation
        //Derived/ReadOnly (e.g. xml classes, presentation logic)
        public string NameAndCount { get { return CUtilities.NameAndCount(FormatName, Keys); } }

        
        public string FormatShort
        {
            get
            {
                switch ((EFormat)FormatId)
                {
                    case EFormat.Boolean: return "Y/N";
                    case EFormat.Integer: return "int";
                    case EFormat.String: return "text";
                }
                return FormatId.ToString();
            }
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
        // e.g. CFormat.Cache.Search("...")

        //See also the auto-generated methods based on indexes
        //' e.g. CFormat.Cache.GetBy...
        #endregion

        #region Caching Details
        //Cache Key
        internal static string CACHE_KEY = typeof(CFormat).ToString();    //TABLE_NAME

        //Cache data
        private static CFormatList LoadCache()  {   return new CFormat().SelectAll();   }
        //Cache Timeout
        private static void SetCache(CFormatList value)
        {
            if (null != value)  
                value.Sort(); 
            CCache.Set(CACHE_KEY, value);    //Optional parameter can override timeout (otherwise uses config-settings, which default to 3hrs)
        }
        //Helper Method
        private CFormat CacheGetById(CFormatList list)  { return list.GetById(this.FormatId);    }
        #endregion

        #region Cloning
        public CFormat Clone(CDataSrc target, IDbTransaction txOrNull) //, int parentId)
        {
            //Shallow copy: Copies the immediate record, excluding autogenerated Pks
            CFormat copy = new CFormat(this, target);

            //Deep Copy - Child Entities: Cloned children must reference their cloned parent
            //copy.SampleParentId = parentId;

            copy.Save(txOrNull);

            //Deep Copy - Parent Entities: Cloned parents also clone their child collections
            //this.Children.Clone(target, txOrNull, copy.FormatId);

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