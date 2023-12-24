using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
    [Serializable()]
    public partial class CRelease : CBaseDynamic, IComparable<CRelease>
    {
        #region Constructors
        //Public (Copy Constructor)
        public CRelease(CRelease original, CDataSrc target)
        {
            //Database Connection
            m_dataSrc = target;

            //Copy fields
            _releaseAppId = original.ReleaseAppId;
            _releaseInstanceId = original.ReleaseInstanceId;
            _releaseVersionId = original.ReleaseVersionId;
            _releaseAppName = original.ReleaseAppName;
            _releaseBranchName = original.ReleaseBranchName;
            _releaseVersionName = original.ReleaseVersionName;
            _releaseCreated = original.ReleaseCreated;
            _releaseExpired = original.ReleaseExpired;
        }

        //Protected (Datareader/Dataset)
        protected CRelease(CDataSrc dataSrc, IDataReader dr) : base(dataSrc, dr) {}
        protected CRelease(CDataSrc dataSrc, DataRow     dr) : base(dataSrc, dr) {}
        #endregion

        #region Default Values
        protected override void InitValues_Auto()
        {
            //Null-Equivalent values (except String.Empty, as nulls tend to be inconvenient for string types)
            _releaseId = int.MinValue;
            _releaseAppId = int.MinValue;
            _releaseInstanceId = int.MinValue;
            _releaseVersionId = int.MinValue;
            _releaseAppName = string.Empty;
            _releaseBranchName = string.Empty;
            _releaseVersionName = string.Empty;
            _releaseCreated = DateTime.MinValue;
            _releaseExpired = DateTime.MinValue;
        }
        #endregion

        #region Members
        protected int _releaseId;
        protected int _releaseAppId;
        protected int _releaseInstanceId;
        protected int _releaseVersionId;
        protected string _releaseAppName;
        protected string _releaseBranchName;
        protected string _releaseVersionName;
        protected DateTime _releaseCreated;
        protected DateTime _releaseExpired;
        #endregion

        #region Properties - Column Values
        //Primary Key Column (ReadOnly)
        public int ReleaseId    {   get { return _releaseId;   } }

        //Table Columns (Read/Write)
        public int ReleaseAppId    {   get { return _releaseAppId; } set { _releaseAppId = value; }    }
        public int ReleaseInstanceId    {   get { return _releaseInstanceId; } set { _releaseInstanceId = value; }    }
        public int ReleaseVersionId    {   get { return _releaseVersionId; } set { _releaseVersionId = value; }    }
        public string ReleaseAppName    {   get { return _releaseAppName; } set { _releaseAppName = value; }    }
        public string ReleaseBranchName    {   get { return _releaseBranchName; } set { _releaseBranchName = value; }    }
        public string ReleaseVersionName    {   get { return _releaseVersionName; } set { _releaseVersionName = value; }    }
        public DateTime ReleaseCreated    {   get { return _releaseCreated; } set { _releaseCreated = value; }    }
        public DateTime ReleaseExpired    {   get { return _releaseExpired; } set { _releaseExpired = value; }    }

        //View Columns (ReadOnly)

        #endregion

        #region MustOverride Methods
        //Schema Information
        public const string TABLE_NAME      = "Deploy_Releases";
        public const string VIEW_NAME       = "";         //Used to override this.ViewName { get }
        public const string ORDER_BY_COLS   = "ReleaseAppName, ReleaseBranchName, ReleaseVersionName";   //See the CompareTo method below (Sql-based sorting should match In-Memory sorting)
        public const string SORTING_COLUMN  = "";
        public override string TableName {    get { return TABLE_NAME; } }
        protected override string OrderByColumns {    get { return ORDER_BY_COLS; } }

        //CompareTo Interface (Default Sort Order)
        public int CompareTo(CRelease other)
        {
            int i = this.ReleaseAppName.CompareTo(other.ReleaseAppName) ;
            if (0 != i)
                return i;
            i = this.ReleaseBranchName.CompareTo(other.ReleaseBranchName) ;
            if (0 != i )
                return i;
            return this.ReleaseVersionName.CompareTo(other.ReleaseVersionName) ;
        }

        //Primary Key Information
        public const string PRIMARY_KEY_NAME = "ReleaseId";
        protected override bool InsertPrimaryKey {  get { return false;    }   }
        protected override string PrimaryKeyName {  get { return PRIMARY_KEY_NAME;    }    }
        protected override object PrimaryKeyValue 
        {
              get { return _releaseId; }
              set { _releaseId = (int)value; }
        }

        //Factory Methods - Object
        protected override CBase MakeFrom(DataRow row)      {   return new CRelease(this.DataSrc, row);  }
        protected override CBase MakeFrom(IDataReader dr)   {   return new CRelease(this.DataSrc, dr);   }

        //Factory Methods - List
        protected override IList MakeList()                 {   return new CReleaseList();               }
        protected override IList MakeList(int capacity)     {   return new CReleaseList(capacity);       }

        //Convert from ADO to .Net
        protected override void ReadColumns(IDataReader dr)
        {
            _releaseId = CAdoData.GetInt(dr, "ReleaseId");
            _releaseAppId = CAdoData.GetInt(dr, "ReleaseAppId");
            _releaseInstanceId = CAdoData.GetInt(dr, "ReleaseInstanceId");
            _releaseVersionId = CAdoData.GetInt(dr, "ReleaseVersionId");
            _releaseAppName = CAdoData.GetStr(dr, "ReleaseAppName");
            _releaseBranchName = CAdoData.GetStr(dr, "ReleaseBranchName");
            _releaseVersionName = CAdoData.GetStr(dr, "ReleaseVersionName");
            _releaseCreated = CAdoData.GetDate(dr, "ReleaseCreated");
            _releaseExpired = CAdoData.GetDate(dr, "ReleaseExpired");
        }
        protected override void ReadColumns(DataRow dr)
        {
            _releaseId = CAdoData.GetInt(dr, "ReleaseId");
            _releaseAppId = CAdoData.GetInt(dr, "ReleaseAppId");
            _releaseInstanceId = CAdoData.GetInt(dr, "ReleaseInstanceId");
            _releaseVersionId = CAdoData.GetInt(dr, "ReleaseVersionId");
            _releaseAppName = CAdoData.GetStr(dr, "ReleaseAppName");
            _releaseBranchName = CAdoData.GetStr(dr, "ReleaseBranchName");
            _releaseVersionName = CAdoData.GetStr(dr, "ReleaseVersionName");
            _releaseCreated = CAdoData.GetDate(dr, "ReleaseCreated");
            _releaseExpired = CAdoData.GetDate(dr, "ReleaseExpired");
        }

        //Parameters for Insert/Update    
        protected override CNameValueList ColumnNameValues()
        {
            CNameValueList data = new CNameValueList();
            data.Add("ReleaseId", CAdoData.NullVal(_releaseId));
            data.Add("ReleaseAppId", CAdoData.NullVal(_releaseAppId));
            data.Add("ReleaseInstanceId", CAdoData.NullVal(_releaseInstanceId));
            data.Add("ReleaseVersionId", CAdoData.NullVal(_releaseVersionId));
            data.Add("ReleaseAppName", CAdoData.NullVal(_releaseAppName));
            data.Add("ReleaseBranchName", CAdoData.NullVal(_releaseBranchName));
            data.Add("ReleaseVersionName", CAdoData.NullVal(_releaseVersionName));
            data.Add("ReleaseCreated", CAdoData.NullVal(_releaseCreated));
            data.Add("ReleaseExpired", CAdoData.NullVal(_releaseExpired));
            return data;
        }
        #endregion

        #region Queries - SelectAll/SelectWhere (inherited methods, cast only)
        //Normally used to load the cache
        public    new CReleaseList SelectAll()                                                                           {   return (CReleaseList)base.SelectAll();        }
        public    new CReleaseList SelectAll(string orderBy)                                                             {   return (CReleaseList)base.SelectAll(orderBy); }

        //Sometimes use a custom query to load the cache
        protected new CReleaseList SelectWhere(string colName, ESign sign, object colValue)                              {   return (CReleaseList)base.SelectWhere(colName, sign, colValue);                  }
        protected new CReleaseList SelectWhere(CCriteria where)                                                          {   return (CReleaseList)base.SelectWhere(where);                                    }
        protected new CReleaseList SelectWhere(CCriteriaList where)                                                      {   return (CReleaseList)base.SelectWhere(where);                                    }
        protected new CReleaseList SelectWhere(CCriteriaList where, string tableOrJoin)                                  {   return (CReleaseList)base.SelectWhere(where, tableOrJoin, this.OrderByColumns);  }
        protected new CReleaseList SelectWhere(CCriteriaList where, string tableOrJoin, string orderBy)                  {   return (CReleaseList)base.SelectWhere(where, tableOrJoin, orderBy);              }
        [Obsolete("Arbitrary where-clause is not necessary portable or safe from sql injection attacks. Consider using the parameterised query interfaces (or a stored proc)")]
        protected new CReleaseList SelectWhere(string unsafeWhereClause)                                                 {   return (CReleaseList)base.SelectWhere(unsafeWhereClause);                        }
        protected     CReleaseList SelectById(int releaseId)                                               {   return (CReleaseList)base.SelectById(releaseId);                    }
        protected     CReleaseList SelectByIds(List<int> ids)                                         {   return (CReleaseList)base.SelectByIds(ids);                                      }

        //Select Queries - Paged
        protected new CReleaseList SelectAll(  CPagingInfo pi)                                               {    return (CReleaseList)base.SelectAll(  pi);                          }
        protected new CReleaseList SelectWhere(CPagingInfo pi, string name, ESign sign, object value)        {    return (CReleaseList)base.SelectWhere(pi, name, sign, value);       }
        protected new CReleaseList SelectWhere(CPagingInfo pi, CCriteria criteria)                           {    return (CReleaseList)base.SelectWhere(pi, criteria);                }
        protected new CReleaseList SelectWhere(CPagingInfo pi, CCriteriaList criteria)                       {    return (CReleaseList)base.SelectWhere(pi, criteria);                }
        protected new CReleaseList SelectWhere(CPagingInfo pi, CCriteriaList criteria, string viewOrJoin)    {    return (CReleaseList)base.SelectWhere(pi, criteria, viewOrJoin);    }

        //Select Queries - Transactional (Internal scope for use in cascade deletes)
        internal new CReleaseList SelectAll(                                                                                           IDbTransaction tx)  {    return (CReleaseList)base.SelectAll(                                                 tx);    }
        internal new CReleaseList SelectAll(string orderBy,                                                                            IDbTransaction tx)  {    return (CReleaseList)base.SelectAll(orderBy,                                         tx);    }
        internal new CReleaseList SelectWhere(string columnName,               object columnValue,                                     IDbTransaction tx)  {    return (CReleaseList)base.SelectWhere(columnName,              columnValue,          tx);    }
        internal new CReleaseList SelectWhere(string columnName,   ESign sign, object columnValue,                                     IDbTransaction tx)  {    return (CReleaseList)base.SelectWhere(columnName,    sign,     columnValue,          tx);    }
        internal new CReleaseList SelectWhere(                             CCriteria     criteria,                                     IDbTransaction tx)  {    return (CReleaseList)base.SelectWhere(               criteria,                       tx);    }
        internal new CReleaseList SelectWhere(                             CCriteriaList criteria,                                     IDbTransaction tx)  {    return (CReleaseList)base.SelectWhere(               criteria,                       tx);    }
        internal new CReleaseList SelectWhere(                             CCriteriaList criteria, string tableOrJoin,                 IDbTransaction tx)  {    return (CReleaseList)base.SelectWhere(               criteria, tableOrJoin,          tx);    }
        internal new CReleaseList SelectWhere(                             CCriteriaList criteria, string tableOrJoin, string orderBy, IDbTransaction tx)  {    return (CReleaseList)base.SelectWhere(               criteria, tableOrJoin,          tx);    }
        internal     CReleaseList SelectById(int releaseId,                                                              IDbTransaction tx)  {    return (CReleaseList)base.SelectById(releaseId,                         tx);    }
        internal     CReleaseList SelectByIds(List<int> ids,                                                        IDbTransaction tx)  {    return (CReleaseList)base.SelectByIds(ids,                                           tx);    }

        //Select Queries - Stored Procs
        protected new CReleaseList MakeList(string storedProcName,                           IDbTransaction txOrNull)    {   return (CReleaseList)base.MakeList(storedProcName,             txOrNull);  }
        protected new CReleaseList MakeList(string storedProcName, object[] parameters,      IDbTransaction txOrNull)    {   return (CReleaseList)base.MakeList(storedProcName, parameters, txOrNull);  }
        protected new CReleaseList MakeList(string storedProcName, CNameValueList parameters,IDbTransaction txOrNull)    {   return (CReleaseList)base.MakeList(storedProcName, parameters, txOrNull);  }
        protected new CReleaseList MakeList(string storedProcName, List<object> parameters,  IDbTransaction txOrNull)    {   return (CReleaseList)base.MakeList(storedProcName, parameters, txOrNull);  }
        protected new CReleaseList MakeList(string storedProcName, int param1,               IDbTransaction txOrNull)    {   return (CReleaseList)base.MakeList(storedProcName, param1,     txOrNull);  }
        protected new CReleaseList MakeList(string storedProcName, string param1,            IDbTransaction txOrNull)    {   return (CReleaseList)base.MakeList(storedProcName, param1,     txOrNull);  }

        //Query Results
        protected new CReleaseList MakeList(DataSet              ds) { return (CReleaseList)base.MakeList(ds);        }
        protected new CReleaseList MakeList(DataTable            dt) { return (CReleaseList)base.MakeList(dt);        }
        protected new CReleaseList MakeList(DataRowCollection  rows) { return (CReleaseList)base.MakeList(rows);      }
        protected new CReleaseList MakeList(IDataReader          dr) { return (CReleaseList)base.MakeList(dr);        }
        protected new CReleaseList MakeList(object           drOrDs) { return (CReleaseList)base.MakeList(drOrDs);    }
        protected new CReleaseList MakeList(byte[]             gzip) { return (CReleaseList)base.MakeList(gzip);      }
        #endregion

        #region Queries - SelectBy[FK] (user-nominated fk/bool columns)
        //Note: These methods should only be used for cascade-deletes, or initialising the cache. Use CRelease.Cache.GetBy... for reqular queries

        //Non-Paged
        protected CReleaseList SelectByAppId(int releaseAppId)    {    return SelectWhere(new CCriteriaList("ReleaseAppId", releaseAppId));    }
        protected CReleaseList SelectByInstanceId(int releaseInstanceId)    {    return SelectWhere(new CCriteriaList("ReleaseInstanceId", releaseInstanceId));    }
        protected CReleaseList SelectByVersionId(int releaseVersionId)    {    return SelectWhere(new CCriteriaList("ReleaseVersionId", releaseVersionId));    }

        //Paged
        protected CReleaseList SelectByAppId(CPagingInfo pi, int releaseAppId)    {    return SelectWhere(pi, new CCriteriaList("ReleaseAppId", releaseAppId));    }
        protected CReleaseList SelectByInstanceId(CPagingInfo pi, int releaseInstanceId)    {    return SelectWhere(pi, new CCriteriaList("ReleaseInstanceId", releaseInstanceId));    }
        protected CReleaseList SelectByVersionId(CPagingInfo pi, int releaseVersionId)    {    return SelectWhere(pi, new CCriteriaList("ReleaseVersionId", releaseVersionId));    }

        //Count
        protected int SelectCountByAppId(int releaseAppId)   {   return SelectCount(new CCriteriaList("ReleaseAppId", releaseAppId));     }
        protected int SelectCountByInstanceId(int releaseInstanceId)   {   return SelectCount(new CCriteriaList("ReleaseInstanceId", releaseInstanceId));     }
        protected int SelectCountByVersionId(int releaseVersionId)   {   return SelectCount(new CCriteriaList("ReleaseVersionId", releaseVersionId));     }

        //Transactional
        internal CReleaseList SelectByAppId(int releaseAppId, IDbTransaction tx)    {    return SelectWhere(new CCriteriaList("ReleaseAppId", releaseAppId), tx);    }
        internal CReleaseList SelectByInstanceId(int releaseInstanceId, IDbTransaction tx)    {    return SelectWhere(new CCriteriaList("ReleaseInstanceId", releaseInstanceId), tx);    }
        internal CReleaseList SelectByVersionId(int releaseVersionId, IDbTransaction tx)    {    return SelectWhere(new CCriteriaList("ReleaseVersionId", releaseVersionId), tx);    }
        #endregion

        #region Static - Cache Implementation
        public static CReleaseList Cache
        {
            get
            {
                CReleaseList cache = (CReleaseList)CCache.Get(CACHE_KEY);
                if (cache == null)
                {
                    lock (typeof(CReleaseList))
                    {
                        cache = (CReleaseList)CCache.Get(CACHE_KEY);
                        if (cache == null)
                        {
                            cache = LoadCache();
                            CRelease.Cache = cache;
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

            lock (typeof(CReleaseList))
            {
                CReleaseList temp = new CReleaseList(Cache);
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

            lock (typeof(CReleaseList))
            {
                CReleaseList temp = new CReleaseList(Cache);
                temp.Add(this);
                SetCache(temp);
            }
        }
        protected override void CacheUpdate()
        {
            if (CacheIsNull)
                return;

            lock (typeof(CReleaseList))
            {
                CReleaseList temp = new CReleaseList(Cache);
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
            Store(w, "ReleaseId", this.ReleaseId);
            Store(w, "ReleaseAppId", this.ReleaseAppId);
            Store(w, "ReleaseInstanceId", this.ReleaseInstanceId);
            Store(w, "ReleaseVersionId", this.ReleaseVersionId);
            Store(w, "ReleaseAppName", this.ReleaseAppName);
            Store(w, "ReleaseBranchName", this.ReleaseBranchName);
            Store(w, "ReleaseVersionName", this.ReleaseVersionName);
            Store(w, "ReleaseCreated", this.ReleaseCreated);
            Store(w, "ReleaseExpired", this.ReleaseExpired);
        }
        #endregion

    }
}
