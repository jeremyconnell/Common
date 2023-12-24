using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
    [Serializable()]
    public partial class CVersionFile : CBaseDynamic, IComparable<CVersionFile>
    {
        #region Constructors
        //Public (Copy Constructor)
        public CVersionFile(CVersionFile original, CDataSrc target)
        {
            //Database Connection
            m_dataSrc = target;

            //Copy fields
            _vFVersionId = original.VFVersionId;
            _vFBinaryMD5 = original.VFBinaryMD5;
            _vFPath = original.VFPath;
        }

        //Protected (Datareader/Dataset)
        protected CVersionFile(CDataSrc dataSrc, IDataReader dr) : base(dataSrc, dr) {}
        protected CVersionFile(CDataSrc dataSrc, DataRow     dr) : base(dataSrc, dr) {}
        #endregion

        #region Default Values
        protected override void InitValues_Auto()
        {
            //Null-Equivalent values (except String.Empty, as nulls tend to be inconvenient for string types)
            _vFId = int.MinValue;
            _vFVersionId = int.MinValue;
            _vFBinaryMD5 = Guid.Empty;
            _vFPath = string.Empty;
        }
        #endregion

        #region Members
        protected int _vFId;
        protected int _vFVersionId;
        protected Guid _vFBinaryMD5;
        protected string _vFPath;
        #endregion

        #region Properties - Column Values
        //Primary Key Column (ReadOnly)
        public int VFId    {   get { return _vFId;   } }

        //Table Columns (Read/Write)
        public int VFVersionId    {   get { return _vFVersionId; } set { _vFVersionId = value; }    }
        public Guid VFBinaryMD5    {   get { return _vFBinaryMD5; } set { _vFBinaryMD5 = value; }    }
        public string VFPath    {   get { return _vFPath; } set { _vFPath = value; }    }

        //View Columns (ReadOnly)

        #endregion

        #region MustOverride Methods
        //Schema Information
        public const string TABLE_NAME      = "Deploy_VersionFile";
        public const string VIEW_NAME       = "";         //Used to override this.ViewName { get }
        public const string ORDER_BY_COLS   = "VFVersionId, VFPath";   //See the CompareTo method below (Sql-based sorting should match In-Memory sorting)
        public const string SORTING_COLUMN  = "";
        public override string TableName {    get { return TABLE_NAME; } }
        protected override string OrderByColumns {    get { return ORDER_BY_COLS; } }

        //CompareTo Interface (Default Sort Order)
        public int CompareTo(CVersionFile other)
        {
            int i = this.VFVersionId.CompareTo(other.VFVersionId) ;
            if (0 != i)
                return i;
            return this.VFPath.CompareTo(other.VFPath) ;
        }

        //Primary Key Information
        public const string PRIMARY_KEY_NAME = "VFId";
        protected override bool InsertPrimaryKey {  get { return false;    }   }
        protected override string PrimaryKeyName {  get { return PRIMARY_KEY_NAME;    }    }
        protected override object PrimaryKeyValue 
        {
              get { return _vFId; }
              set { _vFId = (int)value; }
        }

        //Factory Methods - Object
        protected override CBase MakeFrom(DataRow row)      {   return new CVersionFile(this.DataSrc, row);  }
        protected override CBase MakeFrom(IDataReader dr)   {   return new CVersionFile(this.DataSrc, dr);   }

        //Factory Methods - List
        protected override IList MakeList()                 {   return new CVersionFileList();               }
        protected override IList MakeList(int capacity)     {   return new CVersionFileList(capacity);       }

        //Convert from ADO to .Net
        protected override void ReadColumns(IDataReader dr)
        {
            _vFId = CAdoData.GetInt(dr, "VFId");
            _vFVersionId = CAdoData.GetInt(dr, "VFVersionId");
            _vFBinaryMD5 = CAdoData.GetGuid(dr, "VFBinaryMD5");
            _vFPath = CAdoData.GetStr(dr, "VFPath");
        }
        protected override void ReadColumns(DataRow dr)
        {
            _vFId = CAdoData.GetInt(dr, "VFId");
            _vFVersionId = CAdoData.GetInt(dr, "VFVersionId");
            _vFBinaryMD5 = CAdoData.GetGuid(dr, "VFBinaryMD5");
            _vFPath = CAdoData.GetStr(dr, "VFPath");
        }

        //Parameters for Insert/Update    
        protected override CNameValueList ColumnNameValues()
        {
            CNameValueList data = new CNameValueList();
            data.Add("VFId", CAdoData.NullVal(_vFId));
            data.Add("VFVersionId", CAdoData.NullVal(_vFVersionId));
            data.Add("VFBinaryMD5", CAdoData.NullVal(_vFBinaryMD5));
            data.Add("VFPath", CAdoData.NullVal(_vFPath));
            return data;
        }
        #endregion

        #region Queries - SelectAll/SelectWhere (inherited methods, cast only)
        //Normally used to load the cache
        public    new CVersionFileList SelectAll()                                                                           {   return (CVersionFileList)base.SelectAll();        }
        public    new CVersionFileList SelectAll(string orderBy)                                                             {   return (CVersionFileList)base.SelectAll(orderBy); }

        //Sometimes use a custom query to load the cache
        protected new CVersionFileList SelectWhere(string colName, ESign sign, object colValue)                              {   return (CVersionFileList)base.SelectWhere(colName, sign, colValue);                  }
        protected new CVersionFileList SelectWhere(CCriteria where)                                                          {   return (CVersionFileList)base.SelectWhere(where);                                    }
        protected new CVersionFileList SelectWhere(CCriteriaList where)                                                      {   return (CVersionFileList)base.SelectWhere(where);                                    }
        protected new CVersionFileList SelectWhere(CCriteriaList where, string tableOrJoin)                                  {   return (CVersionFileList)base.SelectWhere(where, tableOrJoin, this.OrderByColumns);  }
        protected new CVersionFileList SelectWhere(CCriteriaList where, string tableOrJoin, string orderBy)                  {   return (CVersionFileList)base.SelectWhere(where, tableOrJoin, orderBy);              }
        [Obsolete("Arbitrary where-clause is not necessary portable or safe from sql injection attacks. Consider using the parameterised query interfaces (or a stored proc)")]
        protected new CVersionFileList SelectWhere(string unsafeWhereClause)                                                 {   return (CVersionFileList)base.SelectWhere(unsafeWhereClause);                        }
        protected     CVersionFileList SelectById(int vFId)                                               {   return (CVersionFileList)base.SelectById(vFId);                    }
        protected     CVersionFileList SelectByIds(List<int> ids)                                         {   return (CVersionFileList)base.SelectByIds(ids);                                      }

        //Select Queries - Paged
        protected new CVersionFileList SelectAll(  CPagingInfo pi)                                               {    return (CVersionFileList)base.SelectAll(  pi);                          }
        protected new CVersionFileList SelectWhere(CPagingInfo pi, string name, ESign sign, object value)        {    return (CVersionFileList)base.SelectWhere(pi, name, sign, value);       }
        protected new CVersionFileList SelectWhere(CPagingInfo pi, CCriteria criteria)                           {    return (CVersionFileList)base.SelectWhere(pi, criteria);                }
        protected new CVersionFileList SelectWhere(CPagingInfo pi, CCriteriaList criteria)                       {    return (CVersionFileList)base.SelectWhere(pi, criteria);                }
        protected new CVersionFileList SelectWhere(CPagingInfo pi, CCriteriaList criteria, string viewOrJoin)    {    return (CVersionFileList)base.SelectWhere(pi, criteria, viewOrJoin);    }

        //Select Queries - Transactional (Internal scope for use in cascade deletes)
        internal new CVersionFileList SelectAll(                                                                                           IDbTransaction tx)  {    return (CVersionFileList)base.SelectAll(                                                 tx);    }
        internal new CVersionFileList SelectAll(string orderBy,                                                                            IDbTransaction tx)  {    return (CVersionFileList)base.SelectAll(orderBy,                                         tx);    }
        internal new CVersionFileList SelectWhere(string columnName,               object columnValue,                                     IDbTransaction tx)  {    return (CVersionFileList)base.SelectWhere(columnName,              columnValue,          tx);    }
        internal new CVersionFileList SelectWhere(string columnName,   ESign sign, object columnValue,                                     IDbTransaction tx)  {    return (CVersionFileList)base.SelectWhere(columnName,    sign,     columnValue,          tx);    }
        internal new CVersionFileList SelectWhere(                             CCriteria     criteria,                                     IDbTransaction tx)  {    return (CVersionFileList)base.SelectWhere(               criteria,                       tx);    }
        internal new CVersionFileList SelectWhere(                             CCriteriaList criteria,                                     IDbTransaction tx)  {    return (CVersionFileList)base.SelectWhere(               criteria,                       tx);    }
        internal new CVersionFileList SelectWhere(                             CCriteriaList criteria, string tableOrJoin,                 IDbTransaction tx)  {    return (CVersionFileList)base.SelectWhere(               criteria, tableOrJoin,          tx);    }
        internal new CVersionFileList SelectWhere(                             CCriteriaList criteria, string tableOrJoin, string orderBy, IDbTransaction tx)  {    return (CVersionFileList)base.SelectWhere(               criteria, tableOrJoin,          tx);    }
        internal     CVersionFileList SelectById(int vFId,                                                              IDbTransaction tx)  {    return (CVersionFileList)base.SelectById(vFId,                         tx);    }
        internal     CVersionFileList SelectByIds(List<int> ids,                                                        IDbTransaction tx)  {    return (CVersionFileList)base.SelectByIds(ids,                                           tx);    }

        //Select Queries - Stored Procs
        protected new CVersionFileList MakeList(string storedProcName,                           IDbTransaction txOrNull)    {   return (CVersionFileList)base.MakeList(storedProcName,             txOrNull);  }
        protected new CVersionFileList MakeList(string storedProcName, object[] parameters,      IDbTransaction txOrNull)    {   return (CVersionFileList)base.MakeList(storedProcName, parameters, txOrNull);  }
        protected new CVersionFileList MakeList(string storedProcName, CNameValueList parameters,IDbTransaction txOrNull)    {   return (CVersionFileList)base.MakeList(storedProcName, parameters, txOrNull);  }
        protected new CVersionFileList MakeList(string storedProcName, List<object> parameters,  IDbTransaction txOrNull)    {   return (CVersionFileList)base.MakeList(storedProcName, parameters, txOrNull);  }
        protected new CVersionFileList MakeList(string storedProcName, int param1,               IDbTransaction txOrNull)    {   return (CVersionFileList)base.MakeList(storedProcName, param1,     txOrNull);  }
        protected new CVersionFileList MakeList(string storedProcName, string param1,            IDbTransaction txOrNull)    {   return (CVersionFileList)base.MakeList(storedProcName, param1,     txOrNull);  }

        //Query Results
        protected new CVersionFileList MakeList(DataSet              ds) { return (CVersionFileList)base.MakeList(ds);        }
        protected new CVersionFileList MakeList(DataTable            dt) { return (CVersionFileList)base.MakeList(dt);        }
        protected new CVersionFileList MakeList(DataRowCollection  rows) { return (CVersionFileList)base.MakeList(rows);      }
        protected new CVersionFileList MakeList(IDataReader          dr) { return (CVersionFileList)base.MakeList(dr);        }
        protected new CVersionFileList MakeList(object           drOrDs) { return (CVersionFileList)base.MakeList(drOrDs);    }
        protected new CVersionFileList MakeList(byte[]             gzip) { return (CVersionFileList)base.MakeList(gzip);      }
        #endregion

        #region Queries - SelectBy[FK] (user-nominated fk/bool columns)
        //Note: These methods should only be used for cascade-deletes, or initialising the cache. Use CVersionFile.Cache.GetBy... for reqular queries

        //Non-Paged
        protected CVersionFileList SelectByVersionId(int vFVersionId)    {    return SelectWhere(new CCriteriaList("VFVersionId", vFVersionId));    }
        protected CVersionFileList SelectByBinaryMD5(Guid vFBinaryMD5)    {    return SelectWhere(new CCriteriaList("VFBinaryMD5", vFBinaryMD5));    }
        protected CVersionFileList SelectByPath(string vFPath)    {    return SelectWhere(new CCriteriaList("VFPath", vFPath));    }

        //Paged
        protected CVersionFileList SelectByVersionId(CPagingInfo pi, int vFVersionId)    {    return SelectWhere(pi, new CCriteriaList("VFVersionId", vFVersionId));    }
        protected CVersionFileList SelectByBinaryMD5(CPagingInfo pi, Guid vFBinaryMD5)    {    return SelectWhere(pi, new CCriteriaList("VFBinaryMD5", vFBinaryMD5));    }
        protected CVersionFileList SelectByPath(CPagingInfo pi, string vFPath)    {    return SelectWhere(pi, new CCriteriaList("VFPath", vFPath));    }

        //Count
        protected int SelectCountByVersionId(int vFVersionId)   {   return SelectCount(new CCriteriaList("VFVersionId", vFVersionId));     }
        protected int SelectCountByBinaryMD5(Guid vFBinaryMD5)   {   return SelectCount(new CCriteriaList("VFBinaryMD5", vFBinaryMD5));     }
        protected int SelectCountByPath(string vFPath)   {   return SelectCount(new CCriteriaList("VFPath", vFPath));     }

        //Transactional
        internal CVersionFileList SelectByVersionId(int vFVersionId, IDbTransaction tx)    {    return SelectWhere(new CCriteriaList("VFVersionId", vFVersionId), tx);    }
        internal CVersionFileList SelectByBinaryMD5(Guid vFBinaryMD5, IDbTransaction tx)    {    return SelectWhere(new CCriteriaList("VFBinaryMD5", vFBinaryMD5), tx);    }
        internal CVersionFileList SelectByPath(string vFPath, IDbTransaction tx)    {    return SelectWhere(new CCriteriaList("VFPath", vFPath), tx);    }
        #endregion

        #region Static - Cache Implementation
        public static CVersionFileList Cache
        {
            get
            {
                CVersionFileList cache = (CVersionFileList)CCache.Get(CACHE_KEY);
                if (cache == null)
                {
                    lock (typeof(CVersionFileList))
                    {
                        cache = (CVersionFileList)CCache.Get(CACHE_KEY);
                        if (cache == null)
                        {
                            cache = LoadCache();
                            CVersionFile.Cache = cache;
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

            lock (typeof(CVersionFileList))
            {
                CVersionFileList temp = new CVersionFileList(Cache);
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

            lock (typeof(CVersionFileList))
            {
                CVersionFileList temp = new CVersionFileList(Cache);
                temp.Add(this);
                SetCache(temp);
            }
        }
        protected override void CacheUpdate()
        {
            if (CacheIsNull)
                return;

            lock (typeof(CVersionFileList))
            {
                CVersionFileList temp = new CVersionFileList(Cache);
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
            Store(w, "VFId", this.VFId);
            Store(w, "VFVersionId", this.VFVersionId);
            Store(w, "VFBinaryMD5", this.VFBinaryMD5);
            Store(w, "VFPath", this.VFPath);
        }
        #endregion

    }
}
