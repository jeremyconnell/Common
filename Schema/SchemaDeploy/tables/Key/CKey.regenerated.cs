using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
    [Serializable()]
    public partial class CKey : SchemaAudit.CBaseDynamicAudited, IComparable<CKey>
    {
        #region Constructors
        //Public (Copy Constructor)
        public CKey(CKey original, CDataSrc target)
        {
            //Database Connection
            m_dataSrc = target;

            //Copy fields
            _keyName = original.KeyName;
            _keyGroupId = original.KeyGroupId;
            _keyFormatId = original.KeyFormatId;
            _keyDefaultString = original.KeyDefaultString;
            _keyDefaultBoolean = original.KeyDefaultBoolean;
            _keyDefaultInteger = original.KeyDefaultInteger;
            _keyIsEncrypted = original.KeyIsEncrypted;
        }

        //Protected (Datareader/Dataset)
        protected CKey(CDataSrc dataSrc, IDataReader dr) : base(dataSrc, dr) {}
        protected CKey(CDataSrc dataSrc, DataRow     dr) : base(dataSrc, dr) {}
        #endregion

        #region Default Values
        protected override void InitValues_Auto()
        {
            //Null-Equivalent values (except String.Empty, as nulls tend to be inconvenient for string types)
            _keyName = string.Empty;
            _keyGroupId = int.MinValue;
            _keyFormatId = int.MinValue;
            _keyDefaultString = string.Empty;
            _keyDefaultBoolean = false;
            _keyDefaultInteger = int.MinValue;
            _keyIsEncrypted = false;
        }
        #endregion

        #region Members
        protected string _keyName;
        protected int _keyGroupId;
        protected int _keyFormatId;
        protected string _keyDefaultString;
        protected bool? _keyDefaultBoolean;
        protected int? _keyDefaultInteger;
        protected bool _keyIsEncrypted;
        #endregion

        #region Properties - Column Values
        //Primary Key Column (ReadOnly)
        public string KeyName
        {   
            get { return _keyName; } 
            set 
            { 
                if (!m_insertPending && _keyName != value)
                {
                    DataSrc.Update(new CNameValueList("KeyName", value), new CWhere(TABLE_NAME, new CCriteria("KeyName", _keyName), null));
                    CacheClear();
                }
                _keyName = value;
            }    
        }

        //Table Columns (Read/Write)
        public int KeyGroupId    {   get { return _keyGroupId; } set { _keyGroupId = value; }    }
        public int KeyFormatId    {   get { return _keyFormatId; } set { _keyFormatId = value; }    }
        public string KeyDefaultString    {   get { return _keyDefaultString; } set { _keyDefaultString = value; }    }
        public bool? KeyDefaultBoolean    {   get { return _keyDefaultBoolean; } set { _keyDefaultBoolean = value; }    }
        public int? KeyDefaultInteger    {   get { return _keyDefaultInteger; } set { _keyDefaultInteger = value; }    }
        public bool KeyIsEncrypted    {   get { return _keyIsEncrypted; } set { _keyIsEncrypted = value; }    }

        //View Columns (ReadOnly)

        #endregion

        #region MustOverride Methods
        //Schema Information
        public const string TABLE_NAME      = "Deploy_Keys";
        public const string VIEW_NAME       = "";         //Used to override this.ViewName { get }
        public const string ORDER_BY_COLS   = "KeyName";   //See the CompareTo method below (Sql-based sorting should match In-Memory sorting)
        public const string SORTING_COLUMN  = "";
        public override string TableName {    get { return TABLE_NAME; } }
        protected override string OrderByColumns {    get { return ORDER_BY_COLS; } }

        //CompareTo Interface (Default Sort Order)
        public int CompareTo(CKey other) {   return this.KeyName.CompareTo(other.KeyName) ;  }

        //Primary Key Information
        public const string PRIMARY_KEY_NAME = "KeyName";
        protected override bool InsertPrimaryKey {  get { return true;    }   }
        protected override string PrimaryKeyName {  get { return PRIMARY_KEY_NAME;    }    }
        protected override object PrimaryKeyValue 
        {
              get { return _keyName; }
              set { _keyName = (string)value; }
        }

        //Factory Methods - Object
        protected override CBase MakeFrom(DataRow row)      {   return new CKey(this.DataSrc, row);  }
        protected override CBase MakeFrom(IDataReader dr)   {   return new CKey(this.DataSrc, dr);   }

        //Factory Methods - List
        protected override IList MakeList()                 {   return new CKeyList();               }
        protected override IList MakeList(int capacity)     {   return new CKeyList(capacity);       }

        //Convert from ADO to .Net
        protected override void ReadColumns(IDataReader dr)
        {
            _keyName = CAdoData.GetStr(dr, "KeyName");
            _keyGroupId = CAdoData.GetInt(dr, "KeyGroupId");
            _keyFormatId = CAdoData.GetInt(dr, "KeyFormatId");
            _keyDefaultString = CAdoData.GetStr(dr, "KeyDefaultString");
            _keyDefaultBoolean = CAdoData.GetBoolNullable(dr, "KeyDefaultBoolean");
            _keyDefaultInteger = CAdoData.GetIntNullable(dr, "KeyDefaultInteger");
            _keyIsEncrypted = CAdoData.GetBool(dr, "KeyIsEncrypted");
        }
        protected override void ReadColumns(DataRow dr)
        {
            _keyName = CAdoData.GetStr(dr, "KeyName");
            _keyGroupId = CAdoData.GetInt(dr, "KeyGroupId");
            _keyFormatId = CAdoData.GetInt(dr, "KeyFormatId");
            _keyDefaultString = CAdoData.GetStr(dr, "KeyDefaultString");
            _keyDefaultBoolean = CAdoData.GetBoolNullable(dr, "KeyDefaultBoolean");
            _keyDefaultInteger = CAdoData.GetIntNullable(dr, "KeyDefaultInteger");
            _keyIsEncrypted = CAdoData.GetBool(dr, "KeyIsEncrypted");
        }

        //Parameters for Insert/Update    
        protected override CNameValueList ColumnNameValues()
        {
            CNameValueList data = new CNameValueList();
            data.Add("KeyName", CAdoData.NullVal(_keyName));
            data.Add("KeyGroupId", CAdoData.NullVal(_keyGroupId));
            data.Add("KeyFormatId", CAdoData.NullVal(_keyFormatId));
            data.Add("KeyDefaultString", CAdoData.NullVal(_keyDefaultString));
            data.Add("KeyDefaultBoolean", CAdoData.NullVal(_keyDefaultBoolean));
            data.Add("KeyDefaultInteger", CAdoData.NullVal(_keyDefaultInteger));
            data.Add("KeyIsEncrypted", CAdoData.NullVal(_keyIsEncrypted));
            return data;
        }
        #endregion

        #region Queries - SelectAll/SelectWhere (inherited methods, cast only)
        //Normally used to load the cache
        public    new CKeyList SelectAll()                                                                           {   return (CKeyList)base.SelectAll();        }
        public    new CKeyList SelectAll(string orderBy)                                                             {   return (CKeyList)base.SelectAll(orderBy); }

        //Sometimes use a custom query to load the cache
        protected new CKeyList SelectWhere(string colName, ESign sign, object colValue)                              {   return (CKeyList)base.SelectWhere(colName, sign, colValue);                  }
        protected new CKeyList SelectWhere(CCriteria where)                                                          {   return (CKeyList)base.SelectWhere(where);                                    }
        protected new CKeyList SelectWhere(CCriteriaList where)                                                      {   return (CKeyList)base.SelectWhere(where);                                    }
        protected new CKeyList SelectWhere(CCriteriaList where, string tableOrJoin)                                  {   return (CKeyList)base.SelectWhere(where, tableOrJoin, this.OrderByColumns);  }
        protected new CKeyList SelectWhere(CCriteriaList where, string tableOrJoin, string orderBy)                  {   return (CKeyList)base.SelectWhere(where, tableOrJoin, orderBy);              }
        [Obsolete("Arbitrary where-clause is not necessary portable or safe from sql injection attacks. Consider using the parameterised query interfaces (or a stored proc)")]
        protected new CKeyList SelectWhere(string unsafeWhereClause)                                                 {   return (CKeyList)base.SelectWhere(unsafeWhereClause);                        }
        protected     CKeyList SelectById(string keyName)                                               {   return (CKeyList)base.SelectById(keyName);                    }
        protected     CKeyList SelectByIds(List<string> ids)                                         {   return (CKeyList)base.SelectByIds(ids);                                      }

        //Select Queries - Paged
        protected new CKeyList SelectAll(  CPagingInfo pi)                                               {    return (CKeyList)base.SelectAll(  pi);                          }
        protected new CKeyList SelectWhere(CPagingInfo pi, string name, ESign sign, object value)        {    return (CKeyList)base.SelectWhere(pi, name, sign, value);       }
        protected new CKeyList SelectWhere(CPagingInfo pi, CCriteria criteria)                           {    return (CKeyList)base.SelectWhere(pi, criteria);                }
        protected new CKeyList SelectWhere(CPagingInfo pi, CCriteriaList criteria)                       {    return (CKeyList)base.SelectWhere(pi, criteria);                }
        protected new CKeyList SelectWhere(CPagingInfo pi, CCriteriaList criteria, string viewOrJoin)    {    return (CKeyList)base.SelectWhere(pi, criteria, viewOrJoin);    }

        //Select Queries - Transactional (Internal scope for use in cascade deletes)
        internal new CKeyList SelectAll(                                                                                           IDbTransaction tx)  {    return (CKeyList)base.SelectAll(                                                 tx);    }
        internal new CKeyList SelectAll(string orderBy,                                                                            IDbTransaction tx)  {    return (CKeyList)base.SelectAll(orderBy,                                         tx);    }
        internal new CKeyList SelectWhere(string columnName,               object columnValue,                                     IDbTransaction tx)  {    return (CKeyList)base.SelectWhere(columnName,              columnValue,          tx);    }
        internal new CKeyList SelectWhere(string columnName,   ESign sign, object columnValue,                                     IDbTransaction tx)  {    return (CKeyList)base.SelectWhere(columnName,    sign,     columnValue,          tx);    }
        internal new CKeyList SelectWhere(                             CCriteria     criteria,                                     IDbTransaction tx)  {    return (CKeyList)base.SelectWhere(               criteria,                       tx);    }
        internal new CKeyList SelectWhere(                             CCriteriaList criteria,                                     IDbTransaction tx)  {    return (CKeyList)base.SelectWhere(               criteria,                       tx);    }
        internal new CKeyList SelectWhere(                             CCriteriaList criteria, string tableOrJoin,                 IDbTransaction tx)  {    return (CKeyList)base.SelectWhere(               criteria, tableOrJoin,          tx);    }
        internal new CKeyList SelectWhere(                             CCriteriaList criteria, string tableOrJoin, string orderBy, IDbTransaction tx)  {    return (CKeyList)base.SelectWhere(               criteria, tableOrJoin,          tx);    }
        internal     CKeyList SelectById(string keyName,                                                              IDbTransaction tx)  {    return (CKeyList)base.SelectById(keyName,                         tx);    }
        internal     CKeyList SelectByIds(List<string> ids,                                                        IDbTransaction tx)  {    return (CKeyList)base.SelectByIds(ids,                                           tx);    }

        //Select Queries - Stored Procs
        protected new CKeyList MakeList(string storedProcName,                           IDbTransaction txOrNull)    {   return (CKeyList)base.MakeList(storedProcName,             txOrNull);  }
        protected new CKeyList MakeList(string storedProcName, object[] parameters,      IDbTransaction txOrNull)    {   return (CKeyList)base.MakeList(storedProcName, parameters, txOrNull);  }
        protected new CKeyList MakeList(string storedProcName, CNameValueList parameters,IDbTransaction txOrNull)    {   return (CKeyList)base.MakeList(storedProcName, parameters, txOrNull);  }
        protected new CKeyList MakeList(string storedProcName, List<object> parameters,  IDbTransaction txOrNull)    {   return (CKeyList)base.MakeList(storedProcName, parameters, txOrNull);  }
        protected new CKeyList MakeList(string storedProcName, int param1,               IDbTransaction txOrNull)    {   return (CKeyList)base.MakeList(storedProcName, param1,     txOrNull);  }
        protected new CKeyList MakeList(string storedProcName, string param1,            IDbTransaction txOrNull)    {   return (CKeyList)base.MakeList(storedProcName, param1,     txOrNull);  }

        //Query Results
        protected new CKeyList MakeList(DataSet              ds) { return (CKeyList)base.MakeList(ds);        }
        protected new CKeyList MakeList(DataTable            dt) { return (CKeyList)base.MakeList(dt);        }
        protected new CKeyList MakeList(DataRowCollection  rows) { return (CKeyList)base.MakeList(rows);      }
        protected new CKeyList MakeList(IDataReader          dr) { return (CKeyList)base.MakeList(dr);        }
        protected new CKeyList MakeList(object           drOrDs) { return (CKeyList)base.MakeList(drOrDs);    }
        protected new CKeyList MakeList(byte[]             gzip) { return (CKeyList)base.MakeList(gzip);      }
        #endregion

        #region Queries - SelectBy[FK] (user-nominated fk/bool columns)
        //Note: These methods should only be used for cascade-deletes, or initialising the cache. Use CKey.Cache.GetBy... for reqular queries

        //Non-Paged
        protected CKeyList SelectByGroupId(int keyGroupId)    {    return SelectWhere(new CCriteriaList("KeyGroupId", keyGroupId));    }
        protected CKeyList SelectByFormatId(int keyFormatId)    {    return SelectWhere(new CCriteriaList("KeyFormatId", keyFormatId));    }
        protected CKeyList SelectByDefaultBoolean(bool keyDefaultBoolean)    {    return SelectWhere(new CCriteriaList("KeyDefaultBoolean", keyDefaultBoolean));    }
        protected CKeyList SelectByDefaultInteger(int keyDefaultInteger)    {    return SelectWhere(new CCriteriaList("KeyDefaultInteger", keyDefaultInteger));    }
        protected CKeyList SelectByIsEncrypted(bool keyIsEncrypted)    {    return SelectWhere(new CCriteriaList("KeyIsEncrypted", keyIsEncrypted));    }

        //Paged
        protected CKeyList SelectByGroupId(CPagingInfo pi, int keyGroupId)    {    return SelectWhere(pi, new CCriteriaList("KeyGroupId", keyGroupId));    }
        protected CKeyList SelectByFormatId(CPagingInfo pi, int keyFormatId)    {    return SelectWhere(pi, new CCriteriaList("KeyFormatId", keyFormatId));    }
        protected CKeyList SelectByDefaultBoolean(CPagingInfo pi, bool keyDefaultBoolean)    {    return SelectWhere(pi, new CCriteriaList("KeyDefaultBoolean", keyDefaultBoolean));    }
        protected CKeyList SelectByDefaultInteger(CPagingInfo pi, int keyDefaultInteger)    {    return SelectWhere(pi, new CCriteriaList("KeyDefaultInteger", keyDefaultInteger));    }
        protected CKeyList SelectByIsEncrypted(CPagingInfo pi, bool keyIsEncrypted)    {    return SelectWhere(pi, new CCriteriaList("KeyIsEncrypted", keyIsEncrypted));    }

        //Count
        protected int SelectCountByGroupId(int keyGroupId)   {   return SelectCount(new CCriteriaList("KeyGroupId", keyGroupId));     }
        protected int SelectCountByFormatId(int keyFormatId)   {   return SelectCount(new CCriteriaList("KeyFormatId", keyFormatId));     }
        protected int SelectCountByDefaultBoolean(bool keyDefaultBoolean)   {   return SelectCount(new CCriteriaList("KeyDefaultBoolean", keyDefaultBoolean));     }
        protected int SelectCountByDefaultInteger(int keyDefaultInteger)   {   return SelectCount(new CCriteriaList("KeyDefaultInteger", keyDefaultInteger));     }
        protected int SelectCountByIsEncrypted(bool keyIsEncrypted)   {   return SelectCount(new CCriteriaList("KeyIsEncrypted", keyIsEncrypted));     }

        //Transactional
        internal CKeyList SelectByGroupId(int keyGroupId, IDbTransaction tx)    {    return SelectWhere(new CCriteriaList("KeyGroupId", keyGroupId), tx);    }
        internal CKeyList SelectByFormatId(int keyFormatId, IDbTransaction tx)    {    return SelectWhere(new CCriteriaList("KeyFormatId", keyFormatId), tx);    }
        internal CKeyList SelectByDefaultBoolean(bool keyDefaultBoolean, IDbTransaction tx)    {    return SelectWhere(new CCriteriaList("KeyDefaultBoolean", keyDefaultBoolean), tx);    }
        internal CKeyList SelectByDefaultInteger(int keyDefaultInteger, IDbTransaction tx)    {    return SelectWhere(new CCriteriaList("KeyDefaultInteger", keyDefaultInteger), tx);    }
        internal CKeyList SelectByIsEncrypted(bool keyIsEncrypted, IDbTransaction tx)    {    return SelectWhere(new CCriteriaList("KeyIsEncrypted", keyIsEncrypted), tx);    }
        #endregion

        #region Static - Cache Implementation
        public static CKeyList Cache
        {
            get
            {
                CKeyList cache = (CKeyList)CCache.Get(CACHE_KEY);
                if (cache == null)
                {
                    lock (typeof(CKeyList))
                    {
                        cache = (CKeyList)CCache.Get(CACHE_KEY);
                        if (cache == null)
                        {
                            cache = LoadCache();
                            CKey.Cache = cache;
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

            lock (typeof(CKeyList))
            {
                CKeyList temp = new CKeyList(Cache);
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

            lock (typeof(CKeyList))
            {
                CKeyList temp = new CKeyList(Cache);
                temp.Add(this);
                SetCache(temp);
            }
        }
        protected override void CacheUpdate()
        {
            if (CacheIsNull)
                return;

            lock (typeof(CKeyList))
            {
                CKeyList temp = new CKeyList(Cache);
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
            Store(w, "KeyName", this.KeyName);
            Store(w, "KeyGroupId", this.KeyGroupId);
            Store(w, "KeyFormatId", this.KeyFormatId);
            Store(w, "KeyDefaultString", this.KeyDefaultString);
            Store(w, "KeyDefaultBoolean", this.KeyDefaultBoolean);
            Store(w, "KeyDefaultInteger", this.KeyDefaultInteger);
            Store(w, "KeyIsEncrypted", this.KeyIsEncrypted);
        }
        #endregion

        #region Audit Trail
        protected override SchemaAudit.CBaseDynamicAudited OriginalState(IDbTransaction txOrNull) 
        { 
            return new CKey(this.DataSrc, this.KeyName, txOrNull); 
        } 
        #endregion
    }
}
