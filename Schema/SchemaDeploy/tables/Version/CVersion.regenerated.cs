using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
    [Serializable()]
    public partial class CVersion : CBaseDynamic, IComparable<CVersion>
    {
        #region Constructors
        //Public (Copy Constructor)
        public CVersion(CVersion original, CDataSrc target)
        {
            //Database Connection
            m_dataSrc = target;

            //Copy fields
            _versionAppId = original.VersionAppId;
            _versionName = original.VersionName;
            _versionTotalBytes = original.VersionTotalBytes;
            _versionUploadedFrom = original.VersionUploadedFrom;
            _versionExcludedFiles = original.VersionExcludedFiles;
            _versionFilesMD5 = original.VersionFilesMD5;
            _versionSchemaConnStr = original.VersionSchemaConnStr;
            _versionSchemaMD5 = original.VersionSchemaMD5;
            _versionCreated = original.VersionCreated;
            _versionExpires = original.VersionExpires;
        }

        //Protected (Datareader/Dataset)
        protected CVersion(CDataSrc dataSrc, IDataReader dr) : base(dataSrc, dr) {}
        protected CVersion(CDataSrc dataSrc, DataRow     dr) : base(dataSrc, dr) {}
        #endregion

        #region Default Values
        protected override void InitValues_Auto()
        {
            //Null-Equivalent values (except String.Empty, as nulls tend to be inconvenient for string types)
            _versionId = int.MinValue;
            _versionAppId = int.MinValue;
            _versionName = string.Empty;
            _versionTotalBytes = long.MinValue;
            _versionUploadedFrom = string.Empty;
            _versionExcludedFiles = string.Empty;
            _versionFilesMD5 = Guid.Empty;
            _versionSchemaConnStr = string.Empty;
            _versionSchemaMD5 = Guid.Empty;
            _versionCreated = DateTime.MinValue;
            _versionExpires = DateTime.MinValue;
        }
        #endregion

        #region Members
        protected int _versionId;
        protected int _versionAppId;
        protected string _versionName;
        protected long _versionTotalBytes;
        protected string _versionUploadedFrom;
        protected string _versionExcludedFiles;
        protected Guid _versionFilesMD5;
        protected string _versionSchemaConnStr;
        protected Guid _versionSchemaMD5;
        protected DateTime _versionCreated;
        protected DateTime _versionExpires;
        #endregion

        #region Properties - Column Values
        //Primary Key Column (ReadOnly)
        public int VersionId    {   get { return _versionId;   } }

        //Table Columns (Read/Write)
        public int VersionAppId    {   get { return _versionAppId; } set { _versionAppId = value; }    }
        public string VersionName    {   get { return _versionName; } set { _versionName = value; }    }
        public long VersionTotalBytes    {   get { return _versionTotalBytes; } set { _versionTotalBytes = value; }    }
        public string VersionUploadedFrom    {   get { return _versionUploadedFrom; } set { _versionUploadedFrom = value; }    }
        public string VersionExcludedFiles    {   get { return _versionExcludedFiles; } set { _versionExcludedFiles = value; }    }
        public Guid VersionFilesMD5    {   get { return _versionFilesMD5; } set { _versionFilesMD5 = value; }    }
        public string VersionSchemaConnStr    {   get { return _versionSchemaConnStr; } set { _versionSchemaConnStr = value; }    }
        public Guid VersionSchemaMD5    {   get { return _versionSchemaMD5; } set { _versionSchemaMD5 = value; }    }
        public DateTime VersionCreated    {   get { return _versionCreated; } set { _versionCreated = value; }    }
        public DateTime VersionExpires    {   get { return _versionExpires; } set { _versionExpires = value; }    }

        //View Columns (ReadOnly)

        #endregion

        #region MustOverride Methods
        //Schema Information
        public const string TABLE_NAME      = "Deploy_Version";
        public const string VIEW_NAME       = "";         //Used to override this.ViewName { get }
        public const string ORDER_BY_COLS   = "VersionId DESC";   //See the CompareTo method below (Sql-based sorting should match In-Memory sorting)
        public const string SORTING_COLUMN  = "";
        public override string TableName {    get { return TABLE_NAME; } }
        protected override string OrderByColumns {    get { return ORDER_BY_COLS; } }

        //CompareTo Interface (Default Sort Order)
        public int CompareTo(CVersion other) {   return this.VersionId.CompareTo(other.VersionId) *-1;  }

        //Primary Key Information
        public const string PRIMARY_KEY_NAME = "VersionId";
        protected override bool InsertPrimaryKey {  get { return false;    }   }
        protected override string PrimaryKeyName {  get { return PRIMARY_KEY_NAME;    }    }
        protected override object PrimaryKeyValue 
        {
              get { return _versionId; }
              set { _versionId = (int)value; }
        }

        //Factory Methods - Object
        protected override CBase MakeFrom(DataRow row)      {   return new CVersion(this.DataSrc, row);  }
        protected override CBase MakeFrom(IDataReader dr)   {   return new CVersion(this.DataSrc, dr);   }

        //Factory Methods - List
        protected override IList MakeList()                 {   return new CVersionList();               }
        protected override IList MakeList(int capacity)     {   return new CVersionList(capacity);       }

        //Convert from ADO to .Net
        protected override void ReadColumns(IDataReader dr)
        {
            _versionId = CAdoData.GetInt(dr, "VersionId");
            _versionAppId = CAdoData.GetInt(dr, "VersionAppId");
            _versionName = CAdoData.GetStr(dr, "VersionName");
            _versionTotalBytes = CAdoData.GetLong(dr, "VersionTotalBytes");
            _versionUploadedFrom = CAdoData.GetStr(dr, "VersionUploadedFrom");
            _versionExcludedFiles = CAdoData.GetStr(dr, "VersionExcludedFiles");
            _versionFilesMD5 = CAdoData.GetGuid(dr, "VersionFilesMD5");
            _versionSchemaConnStr = CAdoData.GetStr(dr, "VersionSchemaConnStr");
            _versionSchemaMD5 = CAdoData.GetGuid(dr, "VersionSchemaMD5");
            _versionCreated = CAdoData.GetDate(dr, "VersionCreated");
            _versionExpires = CAdoData.GetDate(dr, "VersionExpires");
        }
        protected override void ReadColumns(DataRow dr)
        {
            _versionId = CAdoData.GetInt(dr, "VersionId");
            _versionAppId = CAdoData.GetInt(dr, "VersionAppId");
            _versionName = CAdoData.GetStr(dr, "VersionName");
            _versionTotalBytes = CAdoData.GetLong(dr, "VersionTotalBytes");
            _versionUploadedFrom = CAdoData.GetStr(dr, "VersionUploadedFrom");
            _versionExcludedFiles = CAdoData.GetStr(dr, "VersionExcludedFiles");
            _versionFilesMD5 = CAdoData.GetGuid(dr, "VersionFilesMD5");
            _versionSchemaConnStr = CAdoData.GetStr(dr, "VersionSchemaConnStr");
            _versionSchemaMD5 = CAdoData.GetGuid(dr, "VersionSchemaMD5");
            _versionCreated = CAdoData.GetDate(dr, "VersionCreated");
            _versionExpires = CAdoData.GetDate(dr, "VersionExpires");
        }

        //Parameters for Insert/Update    
        protected override CNameValueList ColumnNameValues()
        {
            CNameValueList data = new CNameValueList();
            data.Add("VersionId", CAdoData.NullVal(_versionId));
            data.Add("VersionAppId", CAdoData.NullVal(_versionAppId));
            data.Add("VersionName", CAdoData.NullVal(_versionName));
            data.Add("VersionTotalBytes", CAdoData.NullVal(_versionTotalBytes));
            data.Add("VersionUploadedFrom", CAdoData.NullVal(_versionUploadedFrom));
            data.Add("VersionExcludedFiles", CAdoData.NullVal(_versionExcludedFiles));
            data.Add("VersionFilesMD5", CAdoData.NullVal(_versionFilesMD5));
            data.Add("VersionSchemaConnStr", CAdoData.NullVal(_versionSchemaConnStr));
            data.Add("VersionSchemaMD5", CAdoData.NullVal(_versionSchemaMD5));
            data.Add("VersionCreated", CAdoData.NullVal(_versionCreated));
            data.Add("VersionExpires", CAdoData.NullVal(_versionExpires));
            return data;
        }
        #endregion

        #region Queries - SelectAll/SelectWhere (inherited methods, cast only)
        //Normally used to load the cache
        public    new CVersionList SelectAll()                                                                           {   return (CVersionList)base.SelectAll();        }
        public    new CVersionList SelectAll(string orderBy)                                                             {   return (CVersionList)base.SelectAll(orderBy); }

        //Sometimes use a custom query to load the cache
        protected new CVersionList SelectWhere(string colName, ESign sign, object colValue)                              {   return (CVersionList)base.SelectWhere(colName, sign, colValue);                  }
        protected new CVersionList SelectWhere(CCriteria where)                                                          {   return (CVersionList)base.SelectWhere(where);                                    }
        protected new CVersionList SelectWhere(CCriteriaList where)                                                      {   return (CVersionList)base.SelectWhere(where);                                    }
        protected new CVersionList SelectWhere(CCriteriaList where, string tableOrJoin)                                  {   return (CVersionList)base.SelectWhere(where, tableOrJoin, this.OrderByColumns);  }
        protected new CVersionList SelectWhere(CCriteriaList where, string tableOrJoin, string orderBy)                  {   return (CVersionList)base.SelectWhere(where, tableOrJoin, orderBy);              }
        [Obsolete("Arbitrary where-clause is not necessary portable or safe from sql injection attacks. Consider using the parameterised query interfaces (or a stored proc)")]
        protected new CVersionList SelectWhere(string unsafeWhereClause)                                                 {   return (CVersionList)base.SelectWhere(unsafeWhereClause);                        }
        protected     CVersionList SelectById(int versionId)                                               {   return (CVersionList)base.SelectById(versionId);                    }
        protected     CVersionList SelectByIds(List<int> ids)                                         {   return (CVersionList)base.SelectByIds(ids);                                      }

        //Select Queries - Paged
        protected new CVersionList SelectAll(  CPagingInfo pi)                                               {    return (CVersionList)base.SelectAll(  pi);                          }
        protected new CVersionList SelectWhere(CPagingInfo pi, string name, ESign sign, object value)        {    return (CVersionList)base.SelectWhere(pi, name, sign, value);       }
        protected new CVersionList SelectWhere(CPagingInfo pi, CCriteria criteria)                           {    return (CVersionList)base.SelectWhere(pi, criteria);                }
        protected new CVersionList SelectWhere(CPagingInfo pi, CCriteriaList criteria)                       {    return (CVersionList)base.SelectWhere(pi, criteria);                }
        protected new CVersionList SelectWhere(CPagingInfo pi, CCriteriaList criteria, string viewOrJoin)    {    return (CVersionList)base.SelectWhere(pi, criteria, viewOrJoin);    }

        //Select Queries - Transactional (Internal scope for use in cascade deletes)
        internal new CVersionList SelectAll(                                                                                           IDbTransaction tx)  {    return (CVersionList)base.SelectAll(                                                 tx);    }
        internal new CVersionList SelectAll(string orderBy,                                                                            IDbTransaction tx)  {    return (CVersionList)base.SelectAll(orderBy,                                         tx);    }
        internal new CVersionList SelectWhere(string columnName,               object columnValue,                                     IDbTransaction tx)  {    return (CVersionList)base.SelectWhere(columnName,              columnValue,          tx);    }
        internal new CVersionList SelectWhere(string columnName,   ESign sign, object columnValue,                                     IDbTransaction tx)  {    return (CVersionList)base.SelectWhere(columnName,    sign,     columnValue,          tx);    }
        internal new CVersionList SelectWhere(                             CCriteria     criteria,                                     IDbTransaction tx)  {    return (CVersionList)base.SelectWhere(               criteria,                       tx);    }
        internal new CVersionList SelectWhere(                             CCriteriaList criteria,                                     IDbTransaction tx)  {    return (CVersionList)base.SelectWhere(               criteria,                       tx);    }
        internal new CVersionList SelectWhere(                             CCriteriaList criteria, string tableOrJoin,                 IDbTransaction tx)  {    return (CVersionList)base.SelectWhere(               criteria, tableOrJoin,          tx);    }
        internal new CVersionList SelectWhere(                             CCriteriaList criteria, string tableOrJoin, string orderBy, IDbTransaction tx)  {    return (CVersionList)base.SelectWhere(               criteria, tableOrJoin,          tx);    }
        internal     CVersionList SelectById(int versionId,                                                              IDbTransaction tx)  {    return (CVersionList)base.SelectById(versionId,                         tx);    }
        internal     CVersionList SelectByIds(List<int> ids,                                                        IDbTransaction tx)  {    return (CVersionList)base.SelectByIds(ids,                                           tx);    }

        //Select Queries - Stored Procs
        protected new CVersionList MakeList(string storedProcName,                           IDbTransaction txOrNull)    {   return (CVersionList)base.MakeList(storedProcName,             txOrNull);  }
        protected new CVersionList MakeList(string storedProcName, object[] parameters,      IDbTransaction txOrNull)    {   return (CVersionList)base.MakeList(storedProcName, parameters, txOrNull);  }
        protected new CVersionList MakeList(string storedProcName, CNameValueList parameters,IDbTransaction txOrNull)    {   return (CVersionList)base.MakeList(storedProcName, parameters, txOrNull);  }
        protected new CVersionList MakeList(string storedProcName, List<object> parameters,  IDbTransaction txOrNull)    {   return (CVersionList)base.MakeList(storedProcName, parameters, txOrNull);  }
        protected new CVersionList MakeList(string storedProcName, int param1,               IDbTransaction txOrNull)    {   return (CVersionList)base.MakeList(storedProcName, param1,     txOrNull);  }
        protected new CVersionList MakeList(string storedProcName, string param1,            IDbTransaction txOrNull)    {   return (CVersionList)base.MakeList(storedProcName, param1,     txOrNull);  }

        //Query Results
        protected new CVersionList MakeList(DataSet              ds) { return (CVersionList)base.MakeList(ds);        }
        protected new CVersionList MakeList(DataTable            dt) { return (CVersionList)base.MakeList(dt);        }
        protected new CVersionList MakeList(DataRowCollection  rows) { return (CVersionList)base.MakeList(rows);      }
        protected new CVersionList MakeList(IDataReader          dr) { return (CVersionList)base.MakeList(dr);        }
        protected new CVersionList MakeList(object           drOrDs) { return (CVersionList)base.MakeList(drOrDs);    }
        protected new CVersionList MakeList(byte[]             gzip) { return (CVersionList)base.MakeList(gzip);      }
        #endregion

        #region Queries - SelectBy[FK] (user-nominated fk/bool columns)
        //Note: These methods should only be used for cascade-deletes, or initialising the cache. Use CVersion.Cache.GetBy... for reqular queries

        //Non-Paged
        protected CVersionList SelectByAppId(int versionAppId)    {    return SelectWhere(new CCriteriaList("VersionAppId", versionAppId));    }
        protected CVersionList SelectByFilesMD5(Guid versionFilesMD5)    {    return SelectWhere(new CCriteriaList("VersionFilesMD5", versionFilesMD5));    }
        protected CVersionList SelectBySchemaMD5(Guid versionSchemaMD5)    {    return SelectWhere(new CCriteriaList("VersionSchemaMD5", versionSchemaMD5));    }

        //Paged
        protected CVersionList SelectByAppId(CPagingInfo pi, int versionAppId)    {    return SelectWhere(pi, new CCriteriaList("VersionAppId", versionAppId));    }
        protected CVersionList SelectByFilesMD5(CPagingInfo pi, Guid versionFilesMD5)    {    return SelectWhere(pi, new CCriteriaList("VersionFilesMD5", versionFilesMD5));    }
        protected CVersionList SelectBySchemaMD5(CPagingInfo pi, Guid versionSchemaMD5)    {    return SelectWhere(pi, new CCriteriaList("VersionSchemaMD5", versionSchemaMD5));    }

        //Count
        protected int SelectCountByAppId(int versionAppId)   {   return SelectCount(new CCriteriaList("VersionAppId", versionAppId));     }
        protected int SelectCountByFilesMD5(Guid versionFilesMD5)   {   return SelectCount(new CCriteriaList("VersionFilesMD5", versionFilesMD5));     }
        protected int SelectCountBySchemaMD5(Guid versionSchemaMD5)   {   return SelectCount(new CCriteriaList("VersionSchemaMD5", versionSchemaMD5));     }

        //Transactional
        internal CVersionList SelectByAppId(int versionAppId, IDbTransaction tx)    {    return SelectWhere(new CCriteriaList("VersionAppId", versionAppId), tx);    }
        internal CVersionList SelectByFilesMD5(Guid versionFilesMD5, IDbTransaction tx)    {    return SelectWhere(new CCriteriaList("VersionFilesMD5", versionFilesMD5), tx);    }
        internal CVersionList SelectBySchemaMD5(Guid versionSchemaMD5, IDbTransaction tx)    {    return SelectWhere(new CCriteriaList("VersionSchemaMD5", versionSchemaMD5), tx);    }
        #endregion

        #region Static - Cache Implementation
        public static CVersionList Cache
        {
            get
            {
                CVersionList cache = (CVersionList)CCache.Get(CACHE_KEY);
                if (cache == null)
                {
                    lock (typeof(CVersionList))
                    {
                        cache = (CVersionList)CCache.Get(CACHE_KEY);
                        if (cache == null)
                        {
                            cache = LoadCache();
                            CVersion.Cache = cache;
                        }
                    }
                }
                return cache;
            }
            set
            {
                SetCache(value);   //Not locked, because cache gets cleared at anytime anyway
            }
        }

        //Change Management:
        //Clone, modify, and then replace the cache (threadsafe for anything iterating the collection)
        //Note that internal indices are dicarded each time unless handled in constructor and add/remove overrides
        protected override void CacheDelete()
        {
            if (CacheIsNull)
                return;

            lock (typeof(CVersionList))
            {
                CVersionList temp = new CVersionList(Cache);
                int size = temp.Count;
                temp.Remove(this);
                if (size == temp.Count) //Remove might fail if cache was refreshed with new instances. Use old index
                    temp.Remove(CacheGetById(temp));
                Cache = temp;
            }
        }
        protected override void CacheInsert()
        {
            if (CacheIsNull)
                return;

            lock (typeof(CVersionList))
            {
                CVersionList temp = new CVersionList(Cache);
                temp.Add(this);
                SetCache(temp);
            }
        }
        protected override void CacheUpdate()
        {
            if (CacheIsNull)
                return;

            lock (typeof(CVersionList))
            {
                CVersionList temp = new CVersionList(Cache);
                if (!temp.Contains(this))
                {
                    temp.Remove(CacheGetById(temp));
                    temp.Add(this);
                }
                SetCache(temp);
            }
        }
        protected override void CacheClear()
        {
            SetCache(null);
        }
        protected static bool CacheIsNull
        {
            get
            {
                return null == CCache.Get(CACHE_KEY);
            }
        }
        #endregion
        #region ToXml
        protected override void ToXml_Autogenerated(System.Xml.XmlWriter w)
        {
            Store(w, "VersionId", this.VersionId);
            Store(w, "VersionAppId", this.VersionAppId);
            Store(w, "VersionName", this.VersionName);
            Store(w, "VersionTotalBytes", this.VersionTotalBytes);
            Store(w, "VersionUploadedFrom", this.VersionUploadedFrom);
            Store(w, "VersionExcludedFiles", this.VersionExcludedFiles);
            Store(w, "VersionFilesMD5", this.VersionFilesMD5);
            Store(w, "VersionSchemaConnStr", this.VersionSchemaConnStr);
            Store(w, "VersionSchemaMD5", this.VersionSchemaMD5);
            Store(w, "VersionCreated", this.VersionCreated);
            Store(w, "VersionExpires", this.VersionExpires);
        }
        #endregion

    }
}
