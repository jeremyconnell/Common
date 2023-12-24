using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
    [Serializable()]
    public partial class CApp : CBaseDynamic, IComparable<CApp>
    {
        #region Constructors
        //Public (Copy Constructor)
        public CApp(CApp original, CDataSrc target)
        {
            //Database Connection
            m_dataSrc = target;

            //Copy fields
            _appName = original.AppName;
            _appMainVersionId = original.AppMainVersionId;
            _appKeepOldFilesForDays = original.AppKeepOldFilesForDays;
            _appCreated = original.AppCreated;
        }

        //Protected (Datareader/Dataset)
        protected CApp(CDataSrc dataSrc, IDataReader dr) : base(dataSrc, dr) {}
        protected CApp(CDataSrc dataSrc, DataRow     dr) : base(dataSrc, dr) {}
        #endregion

        #region Default Values
        protected override void InitValues_Auto()
        {
            //Null-Equivalent values (except String.Empty, as nulls tend to be inconvenient for string types)
            _appId = int.MinValue;
            _appName = string.Empty;
            _appMainVersionId = int.MinValue;
            _appKeepOldFilesForDays = int.MinValue;
            _appCreated = DateTime.MinValue;
        }
        #endregion

        #region Members
        protected int _appId;
        protected string _appName;
        protected int _appMainVersionId;
        protected int _appKeepOldFilesForDays;
        protected DateTime _appCreated;
        #endregion

        #region Properties - Column Values
        //Primary Key Column (ReadOnly)
        public int AppId    {   get { return _appId;   } }

        //Table Columns (Read/Write)
        public string AppName    {   get { return _appName; } set { _appName = value; }    }
        public int AppMainVersionId    {   get { return _appMainVersionId; } set { _appMainVersionId = value; }    }
        public int AppKeepOldFilesForDays    {   get { return _appKeepOldFilesForDays; } set { _appKeepOldFilesForDays = value; }    }
        public DateTime AppCreated    {   get { return _appCreated; } set { _appCreated = value; }    }

        //View Columns (ReadOnly)

        #endregion

        #region MustOverride Methods
        //Schema Information
        public const string TABLE_NAME      = "Deploy_Application";
        public const string VIEW_NAME       = "";         //Used to override this.ViewName { get }
        public const string ORDER_BY_COLS   = "AppName";   //See the CompareTo method below (Sql-based sorting should match In-Memory sorting)
        public const string SORTING_COLUMN  = "";
        public override string TableName {    get { return TABLE_NAME; } }
        protected override string OrderByColumns {    get { return ORDER_BY_COLS; } }

        //CompareTo Interface (Default Sort Order)
        public int CompareTo(CApp other) {   return this.AppName.CompareTo(other.AppName) ;  }

        //Primary Key Information
        public const string PRIMARY_KEY_NAME = "AppId";
        protected override bool InsertPrimaryKey {  get { return false;    }   }
        protected override string PrimaryKeyName {  get { return PRIMARY_KEY_NAME;    }    }
        protected override object PrimaryKeyValue 
        {
              get { return _appId; }
              set { _appId = (int)value; }
        }

        //Factory Methods - Object
        protected override CBase MakeFrom(DataRow row)      {   return new CApp(this.DataSrc, row);  }
        protected override CBase MakeFrom(IDataReader dr)   {   return new CApp(this.DataSrc, dr);   }

        //Factory Methods - List
        protected override IList MakeList()                 {   return new CAppList();               }
        protected override IList MakeList(int capacity)     {   return new CAppList(capacity);       }

        //Convert from ADO to .Net
        protected override void ReadColumns(IDataReader dr)
        {
            _appId = CAdoData.GetInt(dr, "AppId");
            _appName = CAdoData.GetStr(dr, "AppName");
            _appMainVersionId = CAdoData.GetInt(dr, "AppMainVersionId");
            _appKeepOldFilesForDays = CAdoData.GetInt(dr, "AppKeepOldFilesForDays");
            _appCreated = CAdoData.GetDate(dr, "AppCreated");
        }
        protected override void ReadColumns(DataRow dr)
        {
            _appId = CAdoData.GetInt(dr, "AppId");
            _appName = CAdoData.GetStr(dr, "AppName");
            _appMainVersionId = CAdoData.GetInt(dr, "AppMainVersionId");
            _appKeepOldFilesForDays = CAdoData.GetInt(dr, "AppKeepOldFilesForDays");
            _appCreated = CAdoData.GetDate(dr, "AppCreated");
        }

        //Parameters for Insert/Update    
        protected override CNameValueList ColumnNameValues()
        {
            CNameValueList data = new CNameValueList();
            data.Add("AppId", CAdoData.NullVal(_appId));
            data.Add("AppName", CAdoData.NullVal(_appName));
            data.Add("AppMainVersionId", CAdoData.NullVal(_appMainVersionId));
            data.Add("AppKeepOldFilesForDays", CAdoData.NullVal(_appKeepOldFilesForDays));
            data.Add("AppCreated", CAdoData.NullVal(_appCreated));
            return data;
        }
        #endregion

        #region Queries - SelectAll/SelectWhere (inherited methods, cast only)
        //Normally used to load the cache
        public    new CAppList SelectAll()                                                                           {   return (CAppList)base.SelectAll();        }
        public    new CAppList SelectAll(string orderBy)                                                             {   return (CAppList)base.SelectAll(orderBy); }

        //Sometimes use a custom query to load the cache
        protected new CAppList SelectWhere(string colName, ESign sign, object colValue)                              {   return (CAppList)base.SelectWhere(colName, sign, colValue);                  }
        protected new CAppList SelectWhere(CCriteria where)                                                          {   return (CAppList)base.SelectWhere(where);                                    }
        protected new CAppList SelectWhere(CCriteriaList where)                                                      {   return (CAppList)base.SelectWhere(where);                                    }
        protected new CAppList SelectWhere(CCriteriaList where, string tableOrJoin)                                  {   return (CAppList)base.SelectWhere(where, tableOrJoin, this.OrderByColumns);  }
        protected new CAppList SelectWhere(CCriteriaList where, string tableOrJoin, string orderBy)                  {   return (CAppList)base.SelectWhere(where, tableOrJoin, orderBy);              }
        [Obsolete("Arbitrary where-clause is not necessary portable or safe from sql injection attacks. Consider using the parameterised query interfaces (or a stored proc)")]
        protected new CAppList SelectWhere(string unsafeWhereClause)                                                 {   return (CAppList)base.SelectWhere(unsafeWhereClause);                        }
        protected     CAppList SelectById(int appId)                                               {   return (CAppList)base.SelectById(appId);                    }
        protected     CAppList SelectByIds(List<int> ids)                                         {   return (CAppList)base.SelectByIds(ids);                                      }

        //Select Queries - Paged
        protected new CAppList SelectAll(  CPagingInfo pi)                                               {    return (CAppList)base.SelectAll(  pi);                          }
        protected new CAppList SelectWhere(CPagingInfo pi, string name, ESign sign, object value)        {    return (CAppList)base.SelectWhere(pi, name, sign, value);       }
        protected new CAppList SelectWhere(CPagingInfo pi, CCriteria criteria)                           {    return (CAppList)base.SelectWhere(pi, criteria);                }
        protected new CAppList SelectWhere(CPagingInfo pi, CCriteriaList criteria)                       {    return (CAppList)base.SelectWhere(pi, criteria);                }
        protected new CAppList SelectWhere(CPagingInfo pi, CCriteriaList criteria, string viewOrJoin)    {    return (CAppList)base.SelectWhere(pi, criteria, viewOrJoin);    }

        //Select Queries - Transactional (Internal scope for use in cascade deletes)
        internal new CAppList SelectAll(                                                                                           IDbTransaction tx)  {    return (CAppList)base.SelectAll(                                                 tx);    }
        internal new CAppList SelectAll(string orderBy,                                                                            IDbTransaction tx)  {    return (CAppList)base.SelectAll(orderBy,                                         tx);    }
        internal new CAppList SelectWhere(string columnName,               object columnValue,                                     IDbTransaction tx)  {    return (CAppList)base.SelectWhere(columnName,              columnValue,          tx);    }
        internal new CAppList SelectWhere(string columnName,   ESign sign, object columnValue,                                     IDbTransaction tx)  {    return (CAppList)base.SelectWhere(columnName,    sign,     columnValue,          tx);    }
        internal new CAppList SelectWhere(                             CCriteria     criteria,                                     IDbTransaction tx)  {    return (CAppList)base.SelectWhere(               criteria,                       tx);    }
        internal new CAppList SelectWhere(                             CCriteriaList criteria,                                     IDbTransaction tx)  {    return (CAppList)base.SelectWhere(               criteria,                       tx);    }
        internal new CAppList SelectWhere(                             CCriteriaList criteria, string tableOrJoin,                 IDbTransaction tx)  {    return (CAppList)base.SelectWhere(               criteria, tableOrJoin,          tx);    }
        internal new CAppList SelectWhere(                             CCriteriaList criteria, string tableOrJoin, string orderBy, IDbTransaction tx)  {    return (CAppList)base.SelectWhere(               criteria, tableOrJoin,          tx);    }
        internal     CAppList SelectById(int appId,                                                              IDbTransaction tx)  {    return (CAppList)base.SelectById(appId,                         tx);    }
        internal     CAppList SelectByIds(List<int> ids,                                                        IDbTransaction tx)  {    return (CAppList)base.SelectByIds(ids,                                           tx);    }

        //Select Queries - Stored Procs
        protected new CAppList MakeList(string storedProcName,                           IDbTransaction txOrNull)    {   return (CAppList)base.MakeList(storedProcName,             txOrNull);  }
        protected new CAppList MakeList(string storedProcName, object[] parameters,      IDbTransaction txOrNull)    {   return (CAppList)base.MakeList(storedProcName, parameters, txOrNull);  }
        protected new CAppList MakeList(string storedProcName, CNameValueList parameters,IDbTransaction txOrNull)    {   return (CAppList)base.MakeList(storedProcName, parameters, txOrNull);  }
        protected new CAppList MakeList(string storedProcName, List<object> parameters,  IDbTransaction txOrNull)    {   return (CAppList)base.MakeList(storedProcName, parameters, txOrNull);  }
        protected new CAppList MakeList(string storedProcName, int param1,               IDbTransaction txOrNull)    {   return (CAppList)base.MakeList(storedProcName, param1,     txOrNull);  }
        protected new CAppList MakeList(string storedProcName, string param1,            IDbTransaction txOrNull)    {   return (CAppList)base.MakeList(storedProcName, param1,     txOrNull);  }

        //Query Results
        protected new CAppList MakeList(DataSet              ds) { return (CAppList)base.MakeList(ds);        }
        protected new CAppList MakeList(DataTable            dt) { return (CAppList)base.MakeList(dt);        }
        protected new CAppList MakeList(DataRowCollection  rows) { return (CAppList)base.MakeList(rows);      }
        protected new CAppList MakeList(IDataReader          dr) { return (CAppList)base.MakeList(dr);        }
        protected new CAppList MakeList(object           drOrDs) { return (CAppList)base.MakeList(drOrDs);    }
        protected new CAppList MakeList(byte[]             gzip) { return (CAppList)base.MakeList(gzip);      }
        #endregion

        #region Queries - SelectBy[FK] (user-nominated fk/bool columns)
        //Note: These methods should only be used for cascade-deletes, or initialising the cache. Use CApp.Cache.GetBy... for reqular queries

        //Non-Paged
        protected CAppList SelectByMainVersionId(int appMainVersionId)    {    return SelectWhere(new CCriteriaList("AppMainVersionId", appMainVersionId));    }

        //Paged
        protected CAppList SelectByMainVersionId(CPagingInfo pi, int appMainVersionId)    {    return SelectWhere(pi, new CCriteriaList("AppMainVersionId", appMainVersionId));    }

        //Count
        protected int SelectCountByMainVersionId(int appMainVersionId)   {   return SelectCount(new CCriteriaList("AppMainVersionId", appMainVersionId));     }

        //Transactional
        internal CAppList SelectByMainVersionId(int appMainVersionId, IDbTransaction tx)    {    return SelectWhere(new CCriteriaList("AppMainVersionId", appMainVersionId), tx);    }
        #endregion

        #region Static - Cache Implementation
        public static CAppList Cache
        {
            get
            {
                CAppList cache = (CAppList)CCache.Get(CACHE_KEY);
                if (cache == null)
                {
                    lock (typeof(CAppList))
                    {
                        cache = (CAppList)CCache.Get(CACHE_KEY);
                        if (cache == null)
                        {
                            cache = LoadCache();
                            CApp.Cache = cache;
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

            lock (typeof(CAppList))
            {
                CAppList temp = new CAppList(Cache);
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

            lock (typeof(CAppList))
            {
                CAppList temp = new CAppList(Cache);
                temp.Add(this);
                SetCache(temp);
            }
        }
        protected override void CacheUpdate()
        {
            if (CacheIsNull)
                return;

            lock (typeof(CAppList))
            {
                CAppList temp = new CAppList(Cache);
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
            Store(w, "AppId", this.AppId);
            Store(w, "AppName", this.AppName);
            Store(w, "AppMainVersionId", this.AppMainVersionId);
            Store(w, "AppKeepOldFilesForDays", this.AppKeepOldFilesForDays);
            Store(w, "AppCreated", this.AppCreated);
        }
        #endregion

    }
}
