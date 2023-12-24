using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
    [Serializable()]
    public partial class CPushedUpgrade : CBaseDynamic, IComparable<CPushedUpgrade>
    {
        #region Constructors
        //Public (Copy Constructor)
        public CPushedUpgrade(CPushedUpgrade original, CDataSrc target)
        {
            //Database Connection
            m_dataSrc = target;

            //Copy fields
            _pushInstanceId = original.PushInstanceId;
            _pushUserName = original.PushUserName;
            _pushOldVersionId = original.PushOldVersionId;
            _pushOldSchemaMD5 = original.PushOldSchemaMD5;
            _pushNewVersionId = original.PushNewVersionId;
            _pushNewSchemaMD5 = original.PushNewSchemaMD5;
            _pushStarted = original.PushStarted;
            _pushCompleted = original.PushCompleted;
        }

        //Protected (Datareader/Dataset)
        protected CPushedUpgrade(CDataSrc dataSrc, IDataReader dr) : base(dataSrc, dr) {}
        protected CPushedUpgrade(CDataSrc dataSrc, DataRow     dr) : base(dataSrc, dr) {}
        #endregion

        #region Default Values
        protected override void InitValues_Auto()
        {
            //Null-Equivalent values (except String.Empty, as nulls tend to be inconvenient for string types)
            _pushId = int.MinValue;
            _pushInstanceId = int.MinValue;
            _pushUserName = string.Empty;
            _pushOldVersionId = int.MinValue;
            _pushOldSchemaMD5 = Guid.Empty;
            _pushNewVersionId = int.MinValue;
            _pushNewSchemaMD5 = Guid.Empty;
            _pushStarted = DateTime.MinValue;
            _pushCompleted = DateTime.MinValue;
        }
        #endregion

        #region Members
        protected int _pushId;
        protected int _pushInstanceId;
        protected string _pushUserName;
        protected int _pushOldVersionId;
        protected Guid _pushOldSchemaMD5;
        protected int _pushNewVersionId;
        protected Guid _pushNewSchemaMD5;
        protected DateTime _pushStarted;
        protected DateTime _pushCompleted;
        #endregion

        #region Properties - Column Values
        //Primary Key Column (ReadOnly)
        public int PushId    {   get { return _pushId;   } }

        //Table Columns (Read/Write)
        public int PushInstanceId    {   get { return _pushInstanceId; } set { _pushInstanceId = value; }    }
        public string PushUserName    {   get { return _pushUserName; } set { _pushUserName = value; }    }
        public int PushOldVersionId    {   get { return _pushOldVersionId; } set { _pushOldVersionId = value; }    }
        public Guid PushOldSchemaMD5    {   get { return _pushOldSchemaMD5; } set { _pushOldSchemaMD5 = value; }    }
        public int PushNewVersionId    {   get { return _pushNewVersionId; } set { _pushNewVersionId = value; }    }
        public Guid PushNewSchemaMD5    {   get { return _pushNewSchemaMD5; } set { _pushNewSchemaMD5 = value; }    }
        public DateTime PushStarted    {   get { return _pushStarted; } set { _pushStarted = value; }    }
        public DateTime PushCompleted    {   get { return _pushCompleted; } set { _pushCompleted = value; }    }

        //View Columns (ReadOnly)

        #endregion

        #region MustOverride Methods
        //Schema Information
        public const string TABLE_NAME      = "Deploy_PushedUpgrades";
        public const string VIEW_NAME       = "";         //Used to override this.ViewName { get }
        public const string ORDER_BY_COLS   = "PushId DESC";   //See the CompareTo method below (Sql-based sorting should match In-Memory sorting)
        public const string SORTING_COLUMN  = "";
        public override string TableName {    get { return TABLE_NAME; } }
        protected override string OrderByColumns {    get { return ORDER_BY_COLS; } }

        //CompareTo Interface (Default Sort Order)
        public int CompareTo(CPushedUpgrade other) {   return this.PushId.CompareTo(other.PushId) *-1;  }

        //Primary Key Information
        public const string PRIMARY_KEY_NAME = "PushId";
        protected override bool InsertPrimaryKey {  get { return false;    }   }
        protected override string PrimaryKeyName {  get { return PRIMARY_KEY_NAME;    }    }
        protected override object PrimaryKeyValue 
        {
              get { return _pushId; }
              set { _pushId = (int)value; }
        }

        //Factory Methods - Object
        protected override CBase MakeFrom(DataRow row)      {   return new CPushedUpgrade(this.DataSrc, row);  }
        protected override CBase MakeFrom(IDataReader dr)   {   return new CPushedUpgrade(this.DataSrc, dr);   }

        //Factory Methods - List
        protected override IList MakeList()                 {   return new CPushedUpgradeList();               }
        protected override IList MakeList(int capacity)     {   return new CPushedUpgradeList(capacity);       }

        //Convert from ADO to .Net
        protected override void ReadColumns(IDataReader dr)
        {
            _pushId = CAdoData.GetInt(dr, "PushId");
            _pushInstanceId = CAdoData.GetInt(dr, "PushInstanceId");
            _pushUserName = CAdoData.GetStr(dr, "PushUserName");
            _pushOldVersionId = CAdoData.GetInt(dr, "PushOldVersionId");
            _pushOldSchemaMD5 = CAdoData.GetGuid(dr, "PushOldSchemaMD5");
            _pushNewVersionId = CAdoData.GetInt(dr, "PushNewVersionId");
            _pushNewSchemaMD5 = CAdoData.GetGuid(dr, "PushNewSchemaMD5");
            _pushStarted = CAdoData.GetDate(dr, "PushStarted");
            _pushCompleted = CAdoData.GetDate(dr, "PushCompleted");
        }
        protected override void ReadColumns(DataRow dr)
        {
            _pushId = CAdoData.GetInt(dr, "PushId");
            _pushInstanceId = CAdoData.GetInt(dr, "PushInstanceId");
            _pushUserName = CAdoData.GetStr(dr, "PushUserName");
            _pushOldVersionId = CAdoData.GetInt(dr, "PushOldVersionId");
            _pushOldSchemaMD5 = CAdoData.GetGuid(dr, "PushOldSchemaMD5");
            _pushNewVersionId = CAdoData.GetInt(dr, "PushNewVersionId");
            _pushNewSchemaMD5 = CAdoData.GetGuid(dr, "PushNewSchemaMD5");
            _pushStarted = CAdoData.GetDate(dr, "PushStarted");
            _pushCompleted = CAdoData.GetDate(dr, "PushCompleted");
        }

        //Parameters for Insert/Update    
        protected override CNameValueList ColumnNameValues()
        {
            CNameValueList data = new CNameValueList();
            data.Add("PushId", CAdoData.NullVal(_pushId));
            data.Add("PushInstanceId", CAdoData.NullVal(_pushInstanceId));
            data.Add("PushUserName", CAdoData.NullVal(_pushUserName));
            data.Add("PushOldVersionId", CAdoData.NullVal(_pushOldVersionId));
            data.Add("PushOldSchemaMD5", CAdoData.NullVal(_pushOldSchemaMD5));
            data.Add("PushNewVersionId", CAdoData.NullVal(_pushNewVersionId));
            data.Add("PushNewSchemaMD5", CAdoData.NullVal(_pushNewSchemaMD5));
            data.Add("PushStarted", CAdoData.NullVal(_pushStarted));
            data.Add("PushCompleted", CAdoData.NullVal(_pushCompleted));
            return data;
        }
        #endregion

        #region Queries - SelectAll/SelectWhere (inherited methods, cast only)
        //Normally used to load the cache
        public    new CPushedUpgradeList SelectAll()                                                                           {   return (CPushedUpgradeList)base.SelectAll();        }
        public    new CPushedUpgradeList SelectAll(string orderBy)                                                             {   return (CPushedUpgradeList)base.SelectAll(orderBy); }

        //Sometimes use a custom query to load the cache
        protected new CPushedUpgradeList SelectWhere(string colName, ESign sign, object colValue)                              {   return (CPushedUpgradeList)base.SelectWhere(colName, sign, colValue);                  }
        protected new CPushedUpgradeList SelectWhere(CCriteria where)                                                          {   return (CPushedUpgradeList)base.SelectWhere(where);                                    }
        protected new CPushedUpgradeList SelectWhere(CCriteriaList where)                                                      {   return (CPushedUpgradeList)base.SelectWhere(where);                                    }
        protected new CPushedUpgradeList SelectWhere(CCriteriaList where, string tableOrJoin)                                  {   return (CPushedUpgradeList)base.SelectWhere(where, tableOrJoin, this.OrderByColumns);  }
        protected new CPushedUpgradeList SelectWhere(CCriteriaList where, string tableOrJoin, string orderBy)                  {   return (CPushedUpgradeList)base.SelectWhere(where, tableOrJoin, orderBy);              }
        [Obsolete("Arbitrary where-clause is not necessary portable or safe from sql injection attacks. Consider using the parameterised query interfaces (or a stored proc)")]
        protected new CPushedUpgradeList SelectWhere(string unsafeWhereClause)                                                 {   return (CPushedUpgradeList)base.SelectWhere(unsafeWhereClause);                        }
        protected     CPushedUpgradeList SelectById(int pushId)                                               {   return (CPushedUpgradeList)base.SelectById(pushId);                    }
        protected     CPushedUpgradeList SelectByIds(List<int> ids)                                         {   return (CPushedUpgradeList)base.SelectByIds(ids);                                      }

        //Select Queries - Paged
        protected new CPushedUpgradeList SelectAll(  CPagingInfo pi)                                               {    return (CPushedUpgradeList)base.SelectAll(  pi);                          }
        protected new CPushedUpgradeList SelectWhere(CPagingInfo pi, string name, ESign sign, object value)        {    return (CPushedUpgradeList)base.SelectWhere(pi, name, sign, value);       }
        protected new CPushedUpgradeList SelectWhere(CPagingInfo pi, CCriteria criteria)                           {    return (CPushedUpgradeList)base.SelectWhere(pi, criteria);                }
        protected new CPushedUpgradeList SelectWhere(CPagingInfo pi, CCriteriaList criteria)                       {    return (CPushedUpgradeList)base.SelectWhere(pi, criteria);                }
        protected new CPushedUpgradeList SelectWhere(CPagingInfo pi, CCriteriaList criteria, string viewOrJoin)    {    return (CPushedUpgradeList)base.SelectWhere(pi, criteria, viewOrJoin);    }

        //Select Queries - Transactional (Internal scope for use in cascade deletes)
        internal new CPushedUpgradeList SelectAll(                                                                                           IDbTransaction tx)  {    return (CPushedUpgradeList)base.SelectAll(                                                 tx);    }
        internal new CPushedUpgradeList SelectAll(string orderBy,                                                                            IDbTransaction tx)  {    return (CPushedUpgradeList)base.SelectAll(orderBy,                                         tx);    }
        internal new CPushedUpgradeList SelectWhere(string columnName,               object columnValue,                                     IDbTransaction tx)  {    return (CPushedUpgradeList)base.SelectWhere(columnName,              columnValue,          tx);    }
        internal new CPushedUpgradeList SelectWhere(string columnName,   ESign sign, object columnValue,                                     IDbTransaction tx)  {    return (CPushedUpgradeList)base.SelectWhere(columnName,    sign,     columnValue,          tx);    }
        internal new CPushedUpgradeList SelectWhere(                             CCriteria     criteria,                                     IDbTransaction tx)  {    return (CPushedUpgradeList)base.SelectWhere(               criteria,                       tx);    }
        internal new CPushedUpgradeList SelectWhere(                             CCriteriaList criteria,                                     IDbTransaction tx)  {    return (CPushedUpgradeList)base.SelectWhere(               criteria,                       tx);    }
        internal new CPushedUpgradeList SelectWhere(                             CCriteriaList criteria, string tableOrJoin,                 IDbTransaction tx)  {    return (CPushedUpgradeList)base.SelectWhere(               criteria, tableOrJoin,          tx);    }
        internal new CPushedUpgradeList SelectWhere(                             CCriteriaList criteria, string tableOrJoin, string orderBy, IDbTransaction tx)  {    return (CPushedUpgradeList)base.SelectWhere(               criteria, tableOrJoin,          tx);    }
        internal     CPushedUpgradeList SelectById(int pushId,                                                              IDbTransaction tx)  {    return (CPushedUpgradeList)base.SelectById(pushId,                         tx);    }
        internal     CPushedUpgradeList SelectByIds(List<int> ids,                                                        IDbTransaction tx)  {    return (CPushedUpgradeList)base.SelectByIds(ids,                                           tx);    }

        //Select Queries - Stored Procs
        protected new CPushedUpgradeList MakeList(string storedProcName,                           IDbTransaction txOrNull)    {   return (CPushedUpgradeList)base.MakeList(storedProcName,             txOrNull);  }
        protected new CPushedUpgradeList MakeList(string storedProcName, object[] parameters,      IDbTransaction txOrNull)    {   return (CPushedUpgradeList)base.MakeList(storedProcName, parameters, txOrNull);  }
        protected new CPushedUpgradeList MakeList(string storedProcName, CNameValueList parameters,IDbTransaction txOrNull)    {   return (CPushedUpgradeList)base.MakeList(storedProcName, parameters, txOrNull);  }
        protected new CPushedUpgradeList MakeList(string storedProcName, List<object> parameters,  IDbTransaction txOrNull)    {   return (CPushedUpgradeList)base.MakeList(storedProcName, parameters, txOrNull);  }
        protected new CPushedUpgradeList MakeList(string storedProcName, int param1,               IDbTransaction txOrNull)    {   return (CPushedUpgradeList)base.MakeList(storedProcName, param1,     txOrNull);  }
        protected new CPushedUpgradeList MakeList(string storedProcName, string param1,            IDbTransaction txOrNull)    {   return (CPushedUpgradeList)base.MakeList(storedProcName, param1,     txOrNull);  }

        //Query Results
        protected new CPushedUpgradeList MakeList(DataSet              ds) { return (CPushedUpgradeList)base.MakeList(ds);        }
        protected new CPushedUpgradeList MakeList(DataTable            dt) { return (CPushedUpgradeList)base.MakeList(dt);        }
        protected new CPushedUpgradeList MakeList(DataRowCollection  rows) { return (CPushedUpgradeList)base.MakeList(rows);      }
        protected new CPushedUpgradeList MakeList(IDataReader          dr) { return (CPushedUpgradeList)base.MakeList(dr);        }
        protected new CPushedUpgradeList MakeList(object           drOrDs) { return (CPushedUpgradeList)base.MakeList(drOrDs);    }
        protected new CPushedUpgradeList MakeList(byte[]             gzip) { return (CPushedUpgradeList)base.MakeList(gzip);      }
        #endregion

        #region Queries - SelectBy[FK] (user-nominated fk/bool columns)
        //Note: These methods should only be used for cascade-deletes, or initialising the cache. Use CPushedUpgrade.Cache.GetBy... for reqular queries

        //Non-Paged
        protected CPushedUpgradeList SelectByInstanceId(int pushInstanceId)    {    return SelectWhere(new CCriteriaList("PushInstanceId", pushInstanceId));    }
        protected CPushedUpgradeList SelectByOldVersionId(int pushOldVersionId)    {    return SelectWhere(new CCriteriaList("PushOldVersionId", pushOldVersionId));    }
        protected CPushedUpgradeList SelectByNewVersionId(int pushNewVersionId)    {    return SelectWhere(new CCriteriaList("PushNewVersionId", pushNewVersionId));    }

        //Paged
        protected CPushedUpgradeList SelectByInstanceId(CPagingInfo pi, int pushInstanceId)    {    return SelectWhere(pi, new CCriteriaList("PushInstanceId", pushInstanceId));    }
        protected CPushedUpgradeList SelectByOldVersionId(CPagingInfo pi, int pushOldVersionId)    {    return SelectWhere(pi, new CCriteriaList("PushOldVersionId", pushOldVersionId));    }
        protected CPushedUpgradeList SelectByNewVersionId(CPagingInfo pi, int pushNewVersionId)    {    return SelectWhere(pi, new CCriteriaList("PushNewVersionId", pushNewVersionId));    }

        //Count
        protected int SelectCountByInstanceId(int pushInstanceId)   {   return SelectCount(new CCriteriaList("PushInstanceId", pushInstanceId));     }
        protected int SelectCountByOldVersionId(int pushOldVersionId)   {   return SelectCount(new CCriteriaList("PushOldVersionId", pushOldVersionId));     }
        protected int SelectCountByNewVersionId(int pushNewVersionId)   {   return SelectCount(new CCriteriaList("PushNewVersionId", pushNewVersionId));     }

        //Transactional
        internal CPushedUpgradeList SelectByInstanceId(int pushInstanceId, IDbTransaction tx)    {    return SelectWhere(new CCriteriaList("PushInstanceId", pushInstanceId), tx);    }
        internal CPushedUpgradeList SelectByOldVersionId(int pushOldVersionId, IDbTransaction tx)    {    return SelectWhere(new CCriteriaList("PushOldVersionId", pushOldVersionId), tx);    }
        internal CPushedUpgradeList SelectByNewVersionId(int pushNewVersionId, IDbTransaction tx)    {    return SelectWhere(new CCriteriaList("PushNewVersionId", pushNewVersionId), tx);    }
        #endregion

        #region Static - Cache Implementation
        public static CPushedUpgradeList Cache
        {
            get
            {
                CPushedUpgradeList cache = (CPushedUpgradeList)CCache.Get(CACHE_KEY);
                if (cache == null)
                {
                    lock (typeof(CPushedUpgradeList))
                    {
                        cache = (CPushedUpgradeList)CCache.Get(CACHE_KEY);
                        if (cache == null)
                        {
                            cache = LoadCache();
                            CPushedUpgrade.Cache = cache;
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

            lock (typeof(CPushedUpgradeList))
            {
                CPushedUpgradeList temp = new CPushedUpgradeList(Cache);
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

            lock (typeof(CPushedUpgradeList))
            {
                CPushedUpgradeList temp = new CPushedUpgradeList(Cache);
                temp.Add(this);
                SetCache(temp);
            }
        }
        protected override void CacheUpdate()
        {
            if (CacheIsNull)
                return;

            lock (typeof(CPushedUpgradeList))
            {
                CPushedUpgradeList temp = new CPushedUpgradeList(Cache);
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
            Store(w, "PushId", this.PushId);
            Store(w, "PushInstanceId", this.PushInstanceId);
            Store(w, "PushUserName", this.PushUserName);
            Store(w, "PushOldVersionId", this.PushOldVersionId);
            Store(w, "PushOldSchemaMD5", this.PushOldSchemaMD5);
            Store(w, "PushNewVersionId", this.PushNewVersionId);
            Store(w, "PushNewSchemaMD5", this.PushNewSchemaMD5);
            Store(w, "PushStarted", this.PushStarted);
            Store(w, "PushCompleted", this.PushCompleted);
        }
        #endregion
        
    }
}
