using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
    [Serializable()]
    public partial class CFormat : SchemaAudit.CBaseDynamicAudited, IComparable<CFormat>
    {
        #region Constructors
        //Public (Copy Constructor)
        public CFormat(CFormat original, CDataSrc target)
        {
            //Database Connection
            m_dataSrc = target;

            //Copy fields
            _formatName = original.FormatName;
        }

        //Protected (Datareader/Dataset)
        protected CFormat(CDataSrc dataSrc, IDataReader dr) : base(dataSrc, dr) {}
        protected CFormat(CDataSrc dataSrc, DataRow     dr) : base(dataSrc, dr) {}
        #endregion

        #region Default Values
        protected override void InitValues_Auto()
        {
            //Null-Equivalent values (except String.Empty, as nulls tend to be inconvenient for string types)
            _formatId = int.MinValue;
            _formatName = string.Empty;
        }
        #endregion

        #region Members
        protected int _formatId;
        protected string _formatName;
        #endregion

        #region Properties - Column Values
        //Primary Key Column (ReadOnly)
        public int FormatId    {   get { return _formatId;   } }

        //Table Columns (Read/Write)
        public string FormatName    {   get { return _formatName; } set { _formatName = value; }    }

        //View Columns (ReadOnly)
        #endregion

        #region MustOverride Methods
        //Schema Information
        public const string TABLE_NAME      = "Deploy_Formats";
        public const string VIEW_NAME       = "";         //Used to override this.ViewName { get }
        public const string ORDER_BY_COLS   = "FormatName";   //See the CompareTo method below (Sql-based sorting should match In-Memory sorting)
        public const string SORTING_COLUMN  = "";
        public override string TableName {    get { return TABLE_NAME; } }
        protected override string OrderByColumns {    get { return ORDER_BY_COLS; } }

        //CompareTo Interface (Default Sort Order)
        public int CompareTo(CFormat other) {   return this.FormatName.CompareTo(other.FormatName) ;  }

        //Primary Key Information
        public const string PRIMARY_KEY_NAME = "FormatId";
        protected override bool InsertPrimaryKey {  get { return false;    }   }
        protected override string PrimaryKeyName {  get { return PRIMARY_KEY_NAME;    }    }
        protected override object PrimaryKeyValue 
        {
              get { return _formatId; }
              set { _formatId = (int)value; }
        }

        //Factory Methods - Object
        protected override CBase MakeFrom(DataRow row)      {   return new CFormat(this.DataSrc, row);  }
        protected override CBase MakeFrom(IDataReader dr)   {   return new CFormat(this.DataSrc, dr);   }

        //Factory Methods - List
        protected override IList MakeList()                 {   return new CFormatList();               }
        protected override IList MakeList(int capacity)     {   return new CFormatList(capacity);       }

        //Convert from ADO to .Net
        protected override void ReadColumns(IDataReader dr)
        {
            _formatId = CAdoData.GetInt(dr, "FormatId");
            _formatName = CAdoData.GetStr(dr, "FormatName");
        }
        protected override void ReadColumns(DataRow dr)
        {
            _formatId = CAdoData.GetInt(dr, "FormatId");
            _formatName = CAdoData.GetStr(dr, "FormatName");
        }

        //Parameters for Insert/Update    
        protected override CNameValueList ColumnNameValues()
        {
            CNameValueList data = new CNameValueList();
            data.Add("FormatId", CAdoData.NullVal(_formatId));
            data.Add("FormatName", CAdoData.NullVal(_formatName));
            return data;
        }
        #endregion

        #region Queries - SelectAll/SelectWhere (inherited methods, cast only)
        //Normally used to load the cache
        public    new CFormatList SelectAll()                                                                           {   return (CFormatList)base.SelectAll();        }
        public    new CFormatList SelectAll(string orderBy)                                                             {   return (CFormatList)base.SelectAll(orderBy); }

        //Sometimes use a custom query to load the cache
        protected new CFormatList SelectWhere(string colName, ESign sign, object colValue)                              {   return (CFormatList)base.SelectWhere(colName, sign, colValue);                  }
        protected new CFormatList SelectWhere(CCriteria where)                                                          {   return (CFormatList)base.SelectWhere(where);                                    }
        protected new CFormatList SelectWhere(CCriteriaList where)                                                      {   return (CFormatList)base.SelectWhere(where);                                    }
        protected new CFormatList SelectWhere(CCriteriaList where, string tableOrJoin)                                  {   return (CFormatList)base.SelectWhere(where, tableOrJoin, this.OrderByColumns);  }
        protected new CFormatList SelectWhere(CCriteriaList where, string tableOrJoin, string orderBy)                  {   return (CFormatList)base.SelectWhere(where, tableOrJoin, orderBy);              }
        [Obsolete("Arbitrary where-clause is not necessary portable or safe from sql injection attacks. Consider using the parameterised query interfaces (or a stored proc)")]
        protected new CFormatList SelectWhere(string unsafeWhereClause)                                                 {   return (CFormatList)base.SelectWhere(unsafeWhereClause);                        }
        protected     CFormatList SelectById(int formatId)                                               {   return (CFormatList)base.SelectById(formatId);                    }
        protected     CFormatList SelectByIds(List<int> ids)                                         {   return (CFormatList)base.SelectByIds(ids);                                      }

        //Select Queries - Paged
        protected new CFormatList SelectAll(  CPagingInfo pi)                                               {    return (CFormatList)base.SelectAll(  pi);                          }
        protected new CFormatList SelectWhere(CPagingInfo pi, string name, ESign sign, object value)        {    return (CFormatList)base.SelectWhere(pi, name, sign, value);       }
        protected new CFormatList SelectWhere(CPagingInfo pi, CCriteria criteria)                           {    return (CFormatList)base.SelectWhere(pi, criteria);                }
        protected new CFormatList SelectWhere(CPagingInfo pi, CCriteriaList criteria)                       {    return (CFormatList)base.SelectWhere(pi, criteria);                }
        protected new CFormatList SelectWhere(CPagingInfo pi, CCriteriaList criteria, string viewOrJoin)    {    return (CFormatList)base.SelectWhere(pi, criteria, viewOrJoin);    }

        //Select Queries - Transactional (Internal scope for use in cascade deletes)
        internal new CFormatList SelectAll(                                                                                           IDbTransaction tx)  {    return (CFormatList)base.SelectAll(                                                 tx);    }
        internal new CFormatList SelectAll(string orderBy,                                                                            IDbTransaction tx)  {    return (CFormatList)base.SelectAll(orderBy,                                         tx);    }
        internal new CFormatList SelectWhere(string columnName,               object columnValue,                                     IDbTransaction tx)  {    return (CFormatList)base.SelectWhere(columnName,              columnValue,          tx);    }
        internal new CFormatList SelectWhere(string columnName,   ESign sign, object columnValue,                                     IDbTransaction tx)  {    return (CFormatList)base.SelectWhere(columnName,    sign,     columnValue,          tx);    }
        internal new CFormatList SelectWhere(                             CCriteria     criteria,                                     IDbTransaction tx)  {    return (CFormatList)base.SelectWhere(               criteria,                       tx);    }
        internal new CFormatList SelectWhere(                             CCriteriaList criteria,                                     IDbTransaction tx)  {    return (CFormatList)base.SelectWhere(               criteria,                       tx);    }
        internal new CFormatList SelectWhere(                             CCriteriaList criteria, string tableOrJoin,                 IDbTransaction tx)  {    return (CFormatList)base.SelectWhere(               criteria, tableOrJoin,          tx);    }
        internal new CFormatList SelectWhere(                             CCriteriaList criteria, string tableOrJoin, string orderBy, IDbTransaction tx)  {    return (CFormatList)base.SelectWhere(               criteria, tableOrJoin,          tx);    }
        internal     CFormatList SelectById(int formatId,                                                              IDbTransaction tx)  {    return (CFormatList)base.SelectById(formatId,                         tx);    }
        internal     CFormatList SelectByIds(List<int> ids,                                                        IDbTransaction tx)  {    return (CFormatList)base.SelectByIds(ids,                                           tx);    }

        //Select Queries - Stored Procs
        protected new CFormatList MakeList(string storedProcName,                           IDbTransaction txOrNull)    {   return (CFormatList)base.MakeList(storedProcName,             txOrNull);  }
        protected new CFormatList MakeList(string storedProcName, object[] parameters,      IDbTransaction txOrNull)    {   return (CFormatList)base.MakeList(storedProcName, parameters, txOrNull);  }
        protected new CFormatList MakeList(string storedProcName, CNameValueList parameters,IDbTransaction txOrNull)    {   return (CFormatList)base.MakeList(storedProcName, parameters, txOrNull);  }
        protected new CFormatList MakeList(string storedProcName, List<object> parameters,  IDbTransaction txOrNull)    {   return (CFormatList)base.MakeList(storedProcName, parameters, txOrNull);  }
        protected new CFormatList MakeList(string storedProcName, int param1,               IDbTransaction txOrNull)    {   return (CFormatList)base.MakeList(storedProcName, param1,     txOrNull);  }
        protected new CFormatList MakeList(string storedProcName, string param1,            IDbTransaction txOrNull)    {   return (CFormatList)base.MakeList(storedProcName, param1,     txOrNull);  }

        //Query Results
        protected new CFormatList MakeList(DataSet              ds) { return (CFormatList)base.MakeList(ds);        }
        protected new CFormatList MakeList(DataTable            dt) { return (CFormatList)base.MakeList(dt);        }
        protected new CFormatList MakeList(DataRowCollection  rows) { return (CFormatList)base.MakeList(rows);      }
        protected new CFormatList MakeList(IDataReader          dr) { return (CFormatList)base.MakeList(dr);        }
        protected new CFormatList MakeList(object           drOrDs) { return (CFormatList)base.MakeList(drOrDs);    }
        protected new CFormatList MakeList(byte[]             gzip) { return (CFormatList)base.MakeList(gzip);      }
        #endregion

        #region Queries - SelectBy[FK] (user-nominated fk/bool columns)
        //Note: These methods should only be used for cascade-deletes, or initialising the cache. Use CFormat.Cache.GetBy... for reqular queries

        //Non-Paged

        //Paged

        //Count

        //Transactional
        #endregion

        #region Static - Cache Implementation
        public static CFormatList Cache
        {
            get
            {
                CFormatList cache = (CFormatList)CCache.Get(CACHE_KEY);
                if (cache == null)
                {
                    lock (typeof(CFormatList))
                    {
                        cache = (CFormatList)CCache.Get(CACHE_KEY);
                        if (cache == null)
                        {
                            cache = LoadCache();
                            CFormat.Cache = cache;
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

            lock (typeof(CFormatList))
            {
                CFormatList temp = new CFormatList(Cache);
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

            lock (typeof(CFormatList))
            {
                CFormatList temp = new CFormatList(Cache);
                temp.Add(this);
                SetCache(temp);
            }
        }
        protected override void CacheUpdate()
        {
            if (CacheIsNull)
                return;

            lock (typeof(CFormatList))
            {
                CFormatList temp = new CFormatList(Cache);
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
            Store(w, "FormatId", this.FormatId);
            Store(w, "FormatName", this.FormatName);
        }
        #endregion

        #region Audit Trail
        protected override SchemaAudit.CBaseDynamicAudited OriginalState(IDbTransaction txOrNull) 
        { 
            return new CFormat(this.DataSrc, this.FormatId, txOrNull); 
        } 
        #endregion
    }
}
