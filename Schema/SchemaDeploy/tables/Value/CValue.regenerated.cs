using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
    [Serializable()]
    public partial class CValue : SchemaAudit.CBaseDynamicAudited, IComparable<CValue>
    {
        #region Constructors
        //Public (Copy Constructor)
        public CValue(CValue original, CDataSrc target)
        {
            //Database Connection
            m_dataSrc = target;

            //Copy fields
            _valueInstanceId = original.ValueInstanceId;
            _valueKeyName = original.ValueKeyName;
            _valueString = original.ValueString;
            _valueBoolean = original.ValueBoolean;
            _valueInteger = original.ValueInteger;
        }

        //Protected (Datareader/Dataset)
        protected CValue(CDataSrc dataSrc, IDataReader dr) : base(dataSrc, dr) {}
        protected CValue(CDataSrc dataSrc, DataRow     dr) : base(dataSrc, dr) {}
        #endregion

        #region Default Values
        protected override void InitValues_Auto()
        {
            //Null-Equivalent values (except String.Empty, as nulls tend to be inconvenient for string types)
            _valueId = int.MinValue;
            _valueInstanceId = int.MinValue;
            _valueKeyName = string.Empty;
            _valueString = string.Empty;
            _valueBoolean = null;
            _valueInteger = int.MinValue;
        }
        #endregion

        #region Members
        protected int _valueId;
        protected int _valueInstanceId;
        protected string _valueKeyName;
        protected string _valueString;
        protected bool? _valueBoolean;
        protected int? _valueInteger;
        #endregion

        #region Properties - Column Values
        //Primary Key Column (ReadOnly)
        public int ValueId    {   get { return _valueId;   } }

        //Table Columns (Read/Write)
        public int ValueInstanceId    {   get { return _valueInstanceId; } set { _valueInstanceId = value; }    }
        public string ValueKeyName    {   get { return _valueKeyName; } set { _valueKeyName = value; }    }
        public string ValueString    {   get { return _valueString; } set { _valueString = value; }    }
        public bool? ValueBoolean    {   get { return _valueBoolean; } set { _valueBoolean = value; }    }
        public int? ValueInteger    {   get { return _valueInteger; } set { _valueInteger = value; }    }

        //View Columns (ReadOnly)

        #endregion

        #region MustOverride Methods
        //Schema Information
        public const string TABLE_NAME      = "Deploy_Values";
        public const string VIEW_NAME       = "";         //Used to override this.ViewName { get }
        public const string ORDER_BY_COLS   = "ValueKeyName";   //See the CompareTo method below (Sql-based sorting should match In-Memory sorting)
        public const string SORTING_COLUMN  = "";
        public override string TableName {    get { return TABLE_NAME; } }
        protected override string OrderByColumns {    get { return ORDER_BY_COLS; } }

        //CompareTo Interface (Default Sort Order)
        public int CompareTo(CValue other) {   return this.ValueKeyName.CompareTo(other.ValueKeyName) ;  }

        //Primary Key Information
        public const string PRIMARY_KEY_NAME = "ValueId";
        protected override bool InsertPrimaryKey {  get { return false;    }   }
        protected override string PrimaryKeyName {  get { return PRIMARY_KEY_NAME;    }    }
        protected override object PrimaryKeyValue 
        {
              get { return _valueId; }
              set { _valueId = (int)value; }
        }

        //Factory Methods - Object
        protected override CBase MakeFrom(DataRow row)      {   return new CValue(this.DataSrc, row);  }
        protected override CBase MakeFrom(IDataReader dr)   {   return new CValue(this.DataSrc, dr);   }

        //Factory Methods - List
        protected override IList MakeList()                 {   return new CValueList();               }
        protected override IList MakeList(int capacity)     {   return new CValueList(capacity);       }

        //Convert from ADO to .Net
        protected override void ReadColumns(IDataReader dr)
        {
            _valueId = CAdoData.GetInt(dr, "ValueId");
            _valueInstanceId = CAdoData.GetInt(dr, "ValueInstanceId");
            _valueKeyName = CAdoData.GetStr(dr, "ValueKeyName");
            _valueString = CAdoData.GetStr(dr, "ValueString");
            _valueBoolean = CAdoData.GetBoolNullable(dr, "ValueBoolean");
            _valueInteger = CAdoData.GetIntNullable(dr, "ValueInteger");
        }
        protected override void ReadColumns(DataRow dr)
        {
            _valueId = CAdoData.GetInt(dr, "ValueId");
            _valueInstanceId = CAdoData.GetInt(dr, "ValueInstanceId");
            _valueKeyName = CAdoData.GetStr(dr, "ValueKeyName");
            _valueString = CAdoData.GetStr(dr, "ValueString");
            _valueBoolean = CAdoData.GetBoolNullable(dr, "ValueBoolean");
            _valueInteger = CAdoData.GetIntNullable(dr, "ValueInteger");
        }

        //Parameters for Insert/Update    
        protected override CNameValueList ColumnNameValues()
        {
            CNameValueList data = new CNameValueList();
            data.Add("ValueId", CAdoData.NullVal(_valueId));
            data.Add("ValueInstanceId", CAdoData.NullVal(_valueInstanceId));
            data.Add("ValueKeyName", CAdoData.NullVal(_valueKeyName));
            data.Add("ValueString", CAdoData.NullVal(_valueString));
            data.Add("ValueBoolean", CAdoData.NullVal(_valueBoolean));
            data.Add("ValueInteger", CAdoData.NullVal(_valueInteger));
            return data;
        }
        #endregion

        #region Queries - SelectAll/SelectWhere (inherited methods, cast only)
        //Normally used to load the cache
        public    new CValueList SelectAll()                                                                           {   return (CValueList)base.SelectAll();        }
        public    new CValueList SelectAll(string orderBy)                                                             {   return (CValueList)base.SelectAll(orderBy); }

        //Sometimes use a custom query to load the cache
        protected new CValueList SelectWhere(string colName, ESign sign, object colValue)                              {   return (CValueList)base.SelectWhere(colName, sign, colValue);                  }
        protected new CValueList SelectWhere(CCriteria where)                                                          {   return (CValueList)base.SelectWhere(where);                                    }
        protected new CValueList SelectWhere(CCriteriaList where)                                                      {   return (CValueList)base.SelectWhere(where);                                    }
        protected new CValueList SelectWhere(CCriteriaList where, string tableOrJoin)                                  {   return (CValueList)base.SelectWhere(where, tableOrJoin, this.OrderByColumns);  }
        protected new CValueList SelectWhere(CCriteriaList where, string tableOrJoin, string orderBy)                  {   return (CValueList)base.SelectWhere(where, tableOrJoin, orderBy);              }
        [Obsolete("Arbitrary where-clause is not necessary portable or safe from sql injection attacks. Consider using the parameterised query interfaces (or a stored proc)")]
        protected new CValueList SelectWhere(string unsafeWhereClause)                                                 {   return (CValueList)base.SelectWhere(unsafeWhereClause);                        }
        protected     CValueList SelectById(int valueId)                                               {   return (CValueList)base.SelectById(valueId);                    }
        protected     CValueList SelectByIds(List<int> ids)                                         {   return (CValueList)base.SelectByIds(ids);                                      }

        //Select Queries - Paged
        protected new CValueList SelectAll(  CPagingInfo pi)                                               {    return (CValueList)base.SelectAll(  pi);                          }
        protected new CValueList SelectWhere(CPagingInfo pi, string name, ESign sign, object value)        {    return (CValueList)base.SelectWhere(pi, name, sign, value);       }
        protected new CValueList SelectWhere(CPagingInfo pi, CCriteria criteria)                           {    return (CValueList)base.SelectWhere(pi, criteria);                }
        protected new CValueList SelectWhere(CPagingInfo pi, CCriteriaList criteria)                       {    return (CValueList)base.SelectWhere(pi, criteria);                }
        protected new CValueList SelectWhere(CPagingInfo pi, CCriteriaList criteria, string viewOrJoin)    {    return (CValueList)base.SelectWhere(pi, criteria, viewOrJoin);    }

        //Select Queries - Transactional (Internal scope for use in cascade deletes)
        internal new CValueList SelectAll(                                                                                           IDbTransaction tx)  {    return (CValueList)base.SelectAll(                                                 tx);    }
        internal new CValueList SelectAll(string orderBy,                                                                            IDbTransaction tx)  {    return (CValueList)base.SelectAll(orderBy,                                         tx);    }
        internal new CValueList SelectWhere(string columnName,               object columnValue,                                     IDbTransaction tx)  {    return (CValueList)base.SelectWhere(columnName,              columnValue,          tx);    }
        internal new CValueList SelectWhere(string columnName,   ESign sign, object columnValue,                                     IDbTransaction tx)  {    return (CValueList)base.SelectWhere(columnName,    sign,     columnValue,          tx);    }
        internal new CValueList SelectWhere(                             CCriteria     criteria,                                     IDbTransaction tx)  {    return (CValueList)base.SelectWhere(               criteria,                       tx);    }
        internal new CValueList SelectWhere(                             CCriteriaList criteria,                                     IDbTransaction tx)  {    return (CValueList)base.SelectWhere(               criteria,                       tx);    }
        internal new CValueList SelectWhere(                             CCriteriaList criteria, string tableOrJoin,                 IDbTransaction tx)  {    return (CValueList)base.SelectWhere(               criteria, tableOrJoin,          tx);    }
        internal new CValueList SelectWhere(                             CCriteriaList criteria, string tableOrJoin, string orderBy, IDbTransaction tx)  {    return (CValueList)base.SelectWhere(               criteria, tableOrJoin,          tx);    }
        internal     CValueList SelectById(int valueId,                                                              IDbTransaction tx)  {    return (CValueList)base.SelectById(valueId,                         tx);    }
        internal     CValueList SelectByIds(List<int> ids,                                                        IDbTransaction tx)  {    return (CValueList)base.SelectByIds(ids,                                           tx);    }

        //Select Queries - Stored Procs
        protected new CValueList MakeList(string storedProcName,                           IDbTransaction txOrNull)    {   return (CValueList)base.MakeList(storedProcName,             txOrNull);  }
        protected new CValueList MakeList(string storedProcName, object[] parameters,      IDbTransaction txOrNull)    {   return (CValueList)base.MakeList(storedProcName, parameters, txOrNull);  }
        protected new CValueList MakeList(string storedProcName, CNameValueList parameters,IDbTransaction txOrNull)    {   return (CValueList)base.MakeList(storedProcName, parameters, txOrNull);  }
        protected new CValueList MakeList(string storedProcName, List<object> parameters,  IDbTransaction txOrNull)    {   return (CValueList)base.MakeList(storedProcName, parameters, txOrNull);  }
        protected new CValueList MakeList(string storedProcName, int param1,               IDbTransaction txOrNull)    {   return (CValueList)base.MakeList(storedProcName, param1,     txOrNull);  }
        protected new CValueList MakeList(string storedProcName, string param1,            IDbTransaction txOrNull)    {   return (CValueList)base.MakeList(storedProcName, param1,     txOrNull);  }

        //Query Results
        protected new CValueList MakeList(DataSet              ds) { return (CValueList)base.MakeList(ds);        }
        protected new CValueList MakeList(DataTable            dt) { return (CValueList)base.MakeList(dt);        }
        protected new CValueList MakeList(DataRowCollection  rows) { return (CValueList)base.MakeList(rows);      }
        protected new CValueList MakeList(IDataReader          dr) { return (CValueList)base.MakeList(dr);        }
        protected new CValueList MakeList(object           drOrDs) { return (CValueList)base.MakeList(drOrDs);    }
        protected new CValueList MakeList(byte[]             gzip) { return (CValueList)base.MakeList(gzip);      }
        #endregion

        #region Queries - SelectBy[FK] (user-nominated fk/bool columns)
        //Note: These methods should only be used for cascade-deletes, or initialising the cache. Use CValue.Cache.GetBy... for reqular queries

        //Non-Paged
        protected CValueList SelectByClientId(int valueClientId)    {    return SelectWhere(new CCriteriaList("ValueClientId", valueClientId));    }
        protected CValueList SelectByInstanceId(int valueInstanceId)    {    return SelectWhere(new CCriteriaList("ValueInstanceId", valueInstanceId));    }
        protected CValueList SelectByKeyName(string valueKeyName)    {    return SelectWhere(new CCriteriaList("ValueKeyName", valueKeyName));    }

        //Paged
        protected CValueList SelectByClientId(CPagingInfo pi, int valueClientId)    {    return SelectWhere(pi, new CCriteriaList("ValueClientId", valueClientId));    }
        protected CValueList SelectByInstanceId(CPagingInfo pi, int valueInstanceId)    {    return SelectWhere(pi, new CCriteriaList("ValueInstanceId", valueInstanceId));    }
        protected CValueList SelectByKeyName(CPagingInfo pi, string valueKeyName)    {    return SelectWhere(pi, new CCriteriaList("ValueKeyName", valueKeyName));    }

        //Count
        protected int SelectCountByClientId(int valueClientId)   {   return SelectCount(new CCriteriaList("ValueClientId", valueClientId));     }
        protected int SelectCountByInstanceId(int valueInstanceId)   {   return SelectCount(new CCriteriaList("ValueInstanceId", valueInstanceId));     }
        protected int SelectCountByKeyName(string valueKeyName)   {   return SelectCount(new CCriteriaList("ValueKeyName", valueKeyName));     }

        //Transactional
        internal CValueList SelectByClientId(int valueClientId, IDbTransaction tx)    {    return SelectWhere(new CCriteriaList("ValueClientId", valueClientId), tx);    }
        internal CValueList SelectByInstanceId(int valueInstanceId, IDbTransaction tx)    {    return SelectWhere(new CCriteriaList("ValueInstanceId", valueInstanceId), tx);    }
        internal CValueList SelectByKeyName(string valueKeyName, IDbTransaction tx)    {    return SelectWhere(new CCriteriaList("ValueKeyName", valueKeyName), tx);    }
        #endregion

        #region Static - Cache Implementation
        public static CValueList Cache
        {
            get
            {
                CValueList cache = (CValueList)CCache.Get(CACHE_KEY);
                if (cache == null)
                {
                    lock (typeof(CValueList))
                    {
                        cache = (CValueList)CCache.Get(CACHE_KEY);
                        if (cache == null)
                        {
                            cache = LoadCache();
                            CValue.Cache = cache;
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

            lock (typeof(CValueList))
            {
                CValueList temp = new CValueList(Cache);
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

            lock (typeof(CValueList))
            {
                CValueList temp = new CValueList(Cache);
                temp.Add(this);
                SetCache(temp);
            }
        }
        protected override void CacheUpdate()
        {
            if (CacheIsNull)
                return;

            lock (typeof(CValueList))
            {
                CValueList temp = new CValueList(Cache);
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
            Store(w, "ValueId", this.ValueId);
            Store(w, "ValueInstanceId", this.ValueInstanceId);
            Store(w, "ValueKeyName", this.ValueKeyName);
            Store(w, "ValueString", this.ValueString);
            Store(w, "ValueBoolean", this.ValueBoolean);
            Store(w, "ValueInteger", this.ValueInteger);
        }
        #endregion

        #region Audit Trail
        protected override SchemaAudit.CBaseDynamicAudited OriginalState(IDbTransaction txOrNull) 
        { 
            return new CValue(this.DataSrc, this.ValueId, txOrNull); 
        } 
        #endregion
    }
}
