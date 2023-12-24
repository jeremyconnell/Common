using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
    [Serializable()]
    public partial class CGroup : SchemaAudit.CBaseDynamicAudited, IComparable<CGroup>
    {
        #region Constructors
        //Public (Copy Constructor)
        public CGroup(CGroup original, CDataSrc target)
        {
            //Database Connection
            m_dataSrc = target;

            //Copy fields
            _groupAppId = original.GroupAppId;
            _groupName = original.GroupName;
        }

        //Protected (Datareader/Dataset)
        protected CGroup(CDataSrc dataSrc, IDataReader dr) : base(dataSrc, dr) {}
        protected CGroup(CDataSrc dataSrc, DataRow     dr) : base(dataSrc, dr) {}
        #endregion

        #region Default Values
        protected override void InitValues_Auto()
        {
            //Null-Equivalent values (except String.Empty, as nulls tend to be inconvenient for string types)
            _groupId = int.MinValue;
            _groupAppId = int.MinValue;
            _groupName = string.Empty;
        }
        #endregion

        #region Members
        protected int _groupId;
        protected int _groupAppId;
        protected string _groupName;
        #endregion

        #region Properties - Column Values
        //Primary Key Column (ReadOnly)
        public int GroupId    {   get { return _groupId;   } }

        //Table Columns (Read/Write)
        public int GroupAppId    {   get { return _groupAppId; } set { _groupAppId = value; }    }
        public string GroupName    {   get { return _groupName; } set { _groupName = value; }    }

        //View Columns (ReadOnly)

        #endregion

        #region MustOverride Methods
        //Schema Information
        public const string TABLE_NAME      = "Deploy_Groups";
        public const string VIEW_NAME       = "";         //Used to override this.ViewName { get }
        public const string ORDER_BY_COLS   = "GroupName";   //See the CompareTo method below (Sql-based sorting should match In-Memory sorting)
        public const string SORTING_COLUMN  = "";
        public override string TableName {    get { return TABLE_NAME; } }
        protected override string OrderByColumns {    get { return ORDER_BY_COLS; } }

        //CompareTo Interface (Default Sort Order)
        public int CompareTo(CGroup other) {   return this.GroupName.CompareTo(other.GroupName) ;  }

        //Primary Key Information
        public const string PRIMARY_KEY_NAME = "GroupId";
        protected override bool InsertPrimaryKey {  get { return false;    }   }
        protected override string PrimaryKeyName {  get { return PRIMARY_KEY_NAME;    }    }
        protected override object PrimaryKeyValue 
        {
              get { return _groupId; }
              set { _groupId = (int)value; }
        }

        //Factory Methods - Object
        protected override CBase MakeFrom(DataRow row)      {   return new CGroup(this.DataSrc, row);  }
        protected override CBase MakeFrom(IDataReader dr)   {   return new CGroup(this.DataSrc, dr);   }

        //Factory Methods - List
        protected override IList MakeList()                 {   return new CGroupList();               }
        protected override IList MakeList(int capacity)     {   return new CGroupList(capacity);       }

        //Convert from ADO to .Net
        protected override void ReadColumns(IDataReader dr)
        {
            _groupId = CAdoData.GetInt(dr, "GroupId");
            _groupAppId = CAdoData.GetInt(dr, "GroupAppId");
            _groupName = CAdoData.GetStr(dr, "GroupName");
        }
        protected override void ReadColumns(DataRow dr)
        {
            _groupId = CAdoData.GetInt(dr, "GroupId");
            _groupAppId = CAdoData.GetInt(dr, "GroupAppId");
            _groupName = CAdoData.GetStr(dr, "GroupName");
        }

        //Parameters for Insert/Update    
        protected override CNameValueList ColumnNameValues()
        {
            CNameValueList data = new CNameValueList();
            data.Add("GroupId", CAdoData.NullVal(_groupId));
            data.Add("GroupAppId", CAdoData.NullVal(_groupAppId));
            data.Add("GroupName", CAdoData.NullVal(_groupName));
            return data;
        }
        #endregion

        #region Queries - SelectAll/SelectWhere (inherited methods, cast only)
        //Normally used to load the cache
        public    new CGroupList SelectAll()                                                                           {   return (CGroupList)base.SelectAll();        }
        public    new CGroupList SelectAll(string orderBy)                                                             {   return (CGroupList)base.SelectAll(orderBy); }

        //Sometimes use a custom query to load the cache
        protected new CGroupList SelectWhere(string colName, ESign sign, object colValue)                              {   return (CGroupList)base.SelectWhere(colName, sign, colValue);                  }
        protected new CGroupList SelectWhere(CCriteria where)                                                          {   return (CGroupList)base.SelectWhere(where);                                    }
        protected new CGroupList SelectWhere(CCriteriaList where)                                                      {   return (CGroupList)base.SelectWhere(where);                                    }
        protected new CGroupList SelectWhere(CCriteriaList where, string tableOrJoin)                                  {   return (CGroupList)base.SelectWhere(where, tableOrJoin, this.OrderByColumns);  }
        protected new CGroupList SelectWhere(CCriteriaList where, string tableOrJoin, string orderBy)                  {   return (CGroupList)base.SelectWhere(where, tableOrJoin, orderBy);              }
        [Obsolete("Arbitrary where-clause is not necessary portable or safe from sql injection attacks. Consider using the parameterised query interfaces (or a stored proc)")]
        protected new CGroupList SelectWhere(string unsafeWhereClause)                                                 {   return (CGroupList)base.SelectWhere(unsafeWhereClause);                        }
        protected     CGroupList SelectById(int groupId)                                               {   return (CGroupList)base.SelectById(groupId);                    }
        protected     CGroupList SelectByIds(List<int> ids)                                         {   return (CGroupList)base.SelectByIds(ids);                                      }

        //Select Queries - Paged
        protected new CGroupList SelectAll(  CPagingInfo pi)                                               {    return (CGroupList)base.SelectAll(  pi);                          }
        protected new CGroupList SelectWhere(CPagingInfo pi, string name, ESign sign, object value)        {    return (CGroupList)base.SelectWhere(pi, name, sign, value);       }
        protected new CGroupList SelectWhere(CPagingInfo pi, CCriteria criteria)                           {    return (CGroupList)base.SelectWhere(pi, criteria);                }
        protected new CGroupList SelectWhere(CPagingInfo pi, CCriteriaList criteria)                       {    return (CGroupList)base.SelectWhere(pi, criteria);                }
        protected new CGroupList SelectWhere(CPagingInfo pi, CCriteriaList criteria, string viewOrJoin)    {    return (CGroupList)base.SelectWhere(pi, criteria, viewOrJoin);    }

        //Select Queries - Transactional (Internal scope for use in cascade deletes)
        internal new CGroupList SelectAll(                                                                                           IDbTransaction tx)  {    return (CGroupList)base.SelectAll(                                                 tx);    }
        internal new CGroupList SelectAll(string orderBy,                                                                            IDbTransaction tx)  {    return (CGroupList)base.SelectAll(orderBy,                                         tx);    }
        internal new CGroupList SelectWhere(string columnName,               object columnValue,                                     IDbTransaction tx)  {    return (CGroupList)base.SelectWhere(columnName,              columnValue,          tx);    }
        internal new CGroupList SelectWhere(string columnName,   ESign sign, object columnValue,                                     IDbTransaction tx)  {    return (CGroupList)base.SelectWhere(columnName,    sign,     columnValue,          tx);    }
        internal new CGroupList SelectWhere(                             CCriteria     criteria,                                     IDbTransaction tx)  {    return (CGroupList)base.SelectWhere(               criteria,                       tx);    }
        internal new CGroupList SelectWhere(                             CCriteriaList criteria,                                     IDbTransaction tx)  {    return (CGroupList)base.SelectWhere(               criteria,                       tx);    }
        internal new CGroupList SelectWhere(                             CCriteriaList criteria, string tableOrJoin,                 IDbTransaction tx)  {    return (CGroupList)base.SelectWhere(               criteria, tableOrJoin,          tx);    }
        internal new CGroupList SelectWhere(                             CCriteriaList criteria, string tableOrJoin, string orderBy, IDbTransaction tx)  {    return (CGroupList)base.SelectWhere(               criteria, tableOrJoin,          tx);    }
        internal     CGroupList SelectById(int groupId,                                                              IDbTransaction tx)  {    return (CGroupList)base.SelectById(groupId,                         tx);    }
        internal     CGroupList SelectByIds(List<int> ids,                                                        IDbTransaction tx)  {    return (CGroupList)base.SelectByIds(ids,                                           tx);    }

        //Select Queries - Stored Procs
        protected new CGroupList MakeList(string storedProcName,                           IDbTransaction txOrNull)    {   return (CGroupList)base.MakeList(storedProcName,             txOrNull);  }
        protected new CGroupList MakeList(string storedProcName, object[] parameters,      IDbTransaction txOrNull)    {   return (CGroupList)base.MakeList(storedProcName, parameters, txOrNull);  }
        protected new CGroupList MakeList(string storedProcName, CNameValueList parameters,IDbTransaction txOrNull)    {   return (CGroupList)base.MakeList(storedProcName, parameters, txOrNull);  }
        protected new CGroupList MakeList(string storedProcName, List<object> parameters,  IDbTransaction txOrNull)    {   return (CGroupList)base.MakeList(storedProcName, parameters, txOrNull);  }
        protected new CGroupList MakeList(string storedProcName, int param1,               IDbTransaction txOrNull)    {   return (CGroupList)base.MakeList(storedProcName, param1,     txOrNull);  }
        protected new CGroupList MakeList(string storedProcName, string param1,            IDbTransaction txOrNull)    {   return (CGroupList)base.MakeList(storedProcName, param1,     txOrNull);  }

        //Query Results
        protected new CGroupList MakeList(DataSet              ds) { return (CGroupList)base.MakeList(ds);        }
        protected new CGroupList MakeList(DataTable            dt) { return (CGroupList)base.MakeList(dt);        }
        protected new CGroupList MakeList(DataRowCollection  rows) { return (CGroupList)base.MakeList(rows);      }
        protected new CGroupList MakeList(IDataReader          dr) { return (CGroupList)base.MakeList(dr);        }
        protected new CGroupList MakeList(object           drOrDs) { return (CGroupList)base.MakeList(drOrDs);    }
        protected new CGroupList MakeList(byte[]             gzip) { return (CGroupList)base.MakeList(gzip);      }
        #endregion

        #region Queries - SelectBy[FK] (user-nominated fk/bool columns)
        //Note: These methods should only be used for cascade-deletes, or initialising the cache. Use CGroup.Cache.GetBy... for reqular queries

        //Non-Paged
        protected CGroupList SelectByAppId(int groupAppId)    {    return SelectWhere(new CCriteriaList("GroupAppId", groupAppId));    }

        //Paged
        protected CGroupList SelectByAppId(CPagingInfo pi, int groupAppId)    {    return SelectWhere(pi, new CCriteriaList("GroupAppId", groupAppId));    }

        //Count
        protected int SelectCountByAppId(int groupAppId)   {   return SelectCount(new CCriteriaList("GroupAppId", groupAppId));     }

        //Transactional
        internal CGroupList SelectByAppId(int groupAppId, IDbTransaction tx)    {    return SelectWhere(new CCriteriaList("GroupAppId", groupAppId), tx);    }
        #endregion

        #region Static - Cache Implementation
        public static CGroupList Cache
        {
            get
            {
                CGroupList cache = (CGroupList)CCache.Get(CACHE_KEY);
                if (cache == null)
                {
                    lock (typeof(CGroupList))
                    {
                        cache = (CGroupList)CCache.Get(CACHE_KEY);
                        if (cache == null)
                        {
                            cache = LoadCache();
                            CGroup.Cache = cache;
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

            lock (typeof(CGroupList))
            {
                CGroupList temp = new CGroupList(Cache);
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

            lock (typeof(CGroupList))
            {
                CGroupList temp = new CGroupList(Cache);
                temp.Add(this);
                SetCache(temp);
            }
        }
        protected override void CacheUpdate()
        {
            if (CacheIsNull)
                return;

            lock (typeof(CGroupList))
            {
                CGroupList temp = new CGroupList(Cache);
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
            Store(w, "GroupId", this.GroupId);
            Store(w, "GroupAppId", this.GroupAppId);
            Store(w, "GroupName", this.GroupName);
        }
        #endregion

        #region Audit Trail
        protected override SchemaAudit.CBaseDynamicAudited OriginalState(IDbTransaction txOrNull) 
        { 
            return new CGroup(this.DataSrc, this.GroupId, txOrNull); 
        } 
        #endregion
    }
}
