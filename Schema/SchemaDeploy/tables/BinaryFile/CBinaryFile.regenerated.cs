using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
    [Serializable()]
    public partial class CBinaryFile : CBaseDynamic, IComparable<CBinaryFile>
    {
        #region Constructors
        //Public (Copy Constructor)
        public CBinaryFile(CBinaryFile original, CDataSrc target)
        {
            //Database Connection
            m_dataSrc = target;

            //Copy fields
            _mD5 = original.MD5;
            _path = original.Path;
            _size = original.Size;
            _isSchema = original.IsSchema;
            _created = original.Created;
            _deleted = original.Deleted;
            Bin = original.Bin;
        }

        //Protected (Datareader/Dataset)
        protected CBinaryFile(CDataSrc dataSrc, IDataReader dr) : base(dataSrc, dr) {}
        protected CBinaryFile(CDataSrc dataSrc, DataRow     dr) : base(dataSrc, dr) {}
        #endregion

        #region Default Values
        protected override void InitValues_Auto()
        {
            //Null-Equivalent values (except String.Empty, as nulls tend to be inconvenient for string types)
            _mD5 = Guid.Empty;
            _path = string.Empty;
            _binGz = new byte[]{};
            _size = int.MinValue;
            _isSchema = false;
            _created = DateTime.MinValue;
            _deleted = DateTime.MinValue;
        }
        #endregion

        #region Members
        protected Guid _mD5;
        protected string _path;
        protected byte[] _binGz;
        protected int _size;
        protected bool _isSchema;
        protected DateTime _created;
        protected DateTime _deleted;
        #endregion

        #region Properties - Column Values
        //Primary Key Column (ReadOnly)
        public Guid MD5
        {   
            get { return _mD5; } 
            //set 
            //{ 
            //    if (!m_insertPending && _mD5 != value)
            //    {
            //        DataSrc.Update(new CNameValueList("MD5", value), new CWhere(TABLE_NAME, new CCriteria("MD5", _mD5), null));
            //        CacheClear();
            //    }
            //    _mD5 = value;
            //}    
        }

        //Table Columns (Read/Write)
        public string Path    {   get { return _path; } set { _path = value; }    }
        public bool IsSchema    {   get { return _isSchema; } set { _isSchema = value; }    }
        public DateTime Created    {   get { return _created; } set { _created = value; }    }
        public DateTime Deleted    {   get { return _deleted; } set { _deleted = value; }    }

        //View Columns (ReadOnly)

        #endregion

        #region MustOverride Methods
        //Schema Information
        public const string TABLE_NAME      = "Deploy_Binary";
        public const string VIEW_NAME       = "vwDeploy_Binary_NullBinGz";         //Used to override this.ViewName { get }
        public const string ORDER_BY_COLS   = "Path";   //See the CompareTo method below (Sql-based sorting should match In-Memory sorting)
        public const string SORTING_COLUMN  = "";
        public override string TableName {    get { return TABLE_NAME; } }
        protected override string ViewName {    get {   return VIEW_NAME;    }    }
        protected override string OrderByColumns {    get { return ORDER_BY_COLS; } }

        //CompareTo Interface (Default Sort Order)
        public int CompareTo(CBinaryFile other) {   return this.Path.CompareTo(other.Path) ;  }

        //Primary Key Information
        public const string PRIMARY_KEY_NAME = "MD5";
        protected override bool InsertPrimaryKey {  get { return true;    }   }
        protected override string PrimaryKeyName {  get { return PRIMARY_KEY_NAME;    }    }
        protected override object PrimaryKeyValue 
        {
              get { return _mD5; }
              set { _mD5 = (Guid)value; }
        }

        //Factory Methods - Object
        protected override CBase MakeFrom(DataRow row)      {   return new CBinaryFile(this.DataSrc, row);  }
        protected override CBase MakeFrom(IDataReader dr)   {   return new CBinaryFile(this.DataSrc, dr);   }

        //Factory Methods - List
        protected override IList MakeList()                 {   return new CBinaryFileList();               }
        protected override IList MakeList(int capacity)     {   return new CBinaryFileList(capacity);       }

        //Convert from ADO to .Net
        protected override void ReadColumns(IDataReader dr)
        {
            _mD5 = CAdoData.GetGuid(dr, "MD5");
            _path = CAdoData.GetStr(dr, "Path");
            _binGz = CAdoData.GetBytes(dr, "BinGz");
            _size = CAdoData.GetInt(dr, "Size");
            _isSchema = CAdoData.GetBool(dr, "IsSchema");
            _created = CAdoData.GetDate(dr, "Created");
            _deleted = CAdoData.GetDate(dr, "Deleted");
        }
        protected override void ReadColumns(DataRow dr)
        {
            _mD5 = CAdoData.GetGuid(dr, "MD5");
            _path = CAdoData.GetStr(dr, "Path");
            _binGz = CAdoData.GetBytes(dr, "BinGz");
            _size = CAdoData.GetInt(dr, "Size");
            _isSchema = CAdoData.GetBool(dr, "IsSchema");
            _created = CAdoData.GetDate(dr, "Created");
            _deleted = CAdoData.GetDate(dr, "Deleted");
        }

        #endregion

        #region Queries - SelectAll/SelectWhere (inherited methods, cast only)
        //Normally used to load the cache
        public    new CBinaryFileList SelectAll()                                                                           {   return (CBinaryFileList)base.SelectAll();        }
        public    new CBinaryFileList SelectAll(string orderBy)                                                             {   return (CBinaryFileList)base.SelectAll(orderBy); }

        //Sometimes use a custom query to load the cache
        protected new CBinaryFileList SelectWhere(string colName, ESign sign, object colValue)                              {   return (CBinaryFileList)base.SelectWhere(colName, sign, colValue);                  }
        protected new CBinaryFileList SelectWhere(CCriteria where)                                                          {   return (CBinaryFileList)base.SelectWhere(where);                                    }
        protected new CBinaryFileList SelectWhere(CCriteriaList where)                                                      {   return (CBinaryFileList)base.SelectWhere(where);                                    }
        protected new CBinaryFileList SelectWhere(CCriteriaList where, string tableOrJoin)                                  {   return (CBinaryFileList)base.SelectWhere(where, tableOrJoin, this.OrderByColumns);  }
        protected new CBinaryFileList SelectWhere(CCriteriaList where, string tableOrJoin, string orderBy)                  {   return (CBinaryFileList)base.SelectWhere(where, tableOrJoin, orderBy);              }
        [Obsolete("Arbitrary where-clause is not necessary portable or safe from sql injection attacks. Consider using the parameterised query interfaces (or a stored proc)")]
        protected new CBinaryFileList SelectWhere(string unsafeWhereClause)                                                 {   return (CBinaryFileList)base.SelectWhere(unsafeWhereClause);                        }
        protected     CBinaryFileList SelectById(Guid mD5)                                               {   return (CBinaryFileList)base.SelectById(mD5);                    }
        protected     CBinaryFileList SelectByIds(List<Guid> ids)                                         {   return (CBinaryFileList)base.SelectByIds(ids);                                      }

        //Select Queries - Paged
        protected new CBinaryFileList SelectAll(  CPagingInfo pi)                                               {    return (CBinaryFileList)base.SelectAll(  pi);                          }
        protected new CBinaryFileList SelectWhere(CPagingInfo pi, string name, ESign sign, object value)        {    return (CBinaryFileList)base.SelectWhere(pi, name, sign, value);       }
        protected new CBinaryFileList SelectWhere(CPagingInfo pi, CCriteria criteria)                           {    return (CBinaryFileList)base.SelectWhere(pi, criteria);                }
        protected new CBinaryFileList SelectWhere(CPagingInfo pi, CCriteriaList criteria)                       {    return (CBinaryFileList)base.SelectWhere(pi, criteria);                }
        protected new CBinaryFileList SelectWhere(CPagingInfo pi, CCriteriaList criteria, string viewOrJoin)    {    return (CBinaryFileList)base.SelectWhere(pi, criteria, viewOrJoin);    }

        //Select Queries - Transactional (Internal scope for use in cascade deletes)
        internal new CBinaryFileList SelectAll(                                                                                           IDbTransaction tx)  {    return (CBinaryFileList)base.SelectAll(                                                 tx);    }
        internal new CBinaryFileList SelectAll(string orderBy,                                                                            IDbTransaction tx)  {    return (CBinaryFileList)base.SelectAll(orderBy,                                         tx);    }
        internal new CBinaryFileList SelectWhere(string columnName,               object columnValue,                                     IDbTransaction tx)  {    return (CBinaryFileList)base.SelectWhere(columnName,              columnValue,          tx);    }
        internal new CBinaryFileList SelectWhere(string columnName,   ESign sign, object columnValue,                                     IDbTransaction tx)  {    return (CBinaryFileList)base.SelectWhere(columnName,    sign,     columnValue,          tx);    }
        internal new CBinaryFileList SelectWhere(                             CCriteria     criteria,                                     IDbTransaction tx)  {    return (CBinaryFileList)base.SelectWhere(               criteria,                       tx);    }
        internal new CBinaryFileList SelectWhere(                             CCriteriaList criteria,                                     IDbTransaction tx)  {    return (CBinaryFileList)base.SelectWhere(               criteria,                       tx);    }
        internal new CBinaryFileList SelectWhere(                             CCriteriaList criteria, string tableOrJoin,                 IDbTransaction tx)  {    return (CBinaryFileList)base.SelectWhere(               criteria, tableOrJoin,          tx);    }
        internal new CBinaryFileList SelectWhere(                             CCriteriaList criteria, string tableOrJoin, string orderBy, IDbTransaction tx)  {    return (CBinaryFileList)base.SelectWhere(               criteria, tableOrJoin,          tx);    }
        internal     CBinaryFileList SelectById(Guid mD5,                                                              IDbTransaction tx)  {    return (CBinaryFileList)base.SelectById(mD5,                         tx);    }
        internal     CBinaryFileList SelectByIds(List<Guid> ids,                                                        IDbTransaction tx)  {    return (CBinaryFileList)base.SelectByIds(ids,                                           tx);    }

        //Select Queries - Stored Procs
        protected new CBinaryFileList MakeList(string storedProcName,                           IDbTransaction txOrNull)    {   return (CBinaryFileList)base.MakeList(storedProcName,             txOrNull);  }
        protected new CBinaryFileList MakeList(string storedProcName, object[] parameters,      IDbTransaction txOrNull)    {   return (CBinaryFileList)base.MakeList(storedProcName, parameters, txOrNull);  }
        protected new CBinaryFileList MakeList(string storedProcName, CNameValueList parameters,IDbTransaction txOrNull)    {   return (CBinaryFileList)base.MakeList(storedProcName, parameters, txOrNull);  }
        protected new CBinaryFileList MakeList(string storedProcName, List<object> parameters,  IDbTransaction txOrNull)    {   return (CBinaryFileList)base.MakeList(storedProcName, parameters, txOrNull);  }
        protected new CBinaryFileList MakeList(string storedProcName, int param1,               IDbTransaction txOrNull)    {   return (CBinaryFileList)base.MakeList(storedProcName, param1,     txOrNull);  }
        protected new CBinaryFileList MakeList(string storedProcName, string param1,            IDbTransaction txOrNull)    {   return (CBinaryFileList)base.MakeList(storedProcName, param1,     txOrNull);  }

        //Query Results
        protected new CBinaryFileList MakeList(DataSet              ds) { return (CBinaryFileList)base.MakeList(ds);        }
        protected new CBinaryFileList MakeList(DataTable            dt) { return (CBinaryFileList)base.MakeList(dt);        }
        protected new CBinaryFileList MakeList(DataRowCollection  rows) { return (CBinaryFileList)base.MakeList(rows);      }
        protected new CBinaryFileList MakeList(IDataReader          dr) { return (CBinaryFileList)base.MakeList(dr);        }
        protected new CBinaryFileList MakeList(object           drOrDs) { return (CBinaryFileList)base.MakeList(drOrDs);    }
        protected new CBinaryFileList MakeList(byte[]             gzip) { return (CBinaryFileList)base.MakeList(gzip);      }
        #endregion

        #region Queries - SelectBy[FK] (user-nominated fk/bool columns)
        //Note: These methods should only be used for cascade-deletes, or initialising the cache. Use CBinaryFile.Cache.GetBy... for reqular queries

        //Non-Paged
        protected CBinaryFileList SelectByIsSchema(bool isSchema)    {    return SelectWhere(new CCriteriaList("IsSchema", isSchema));    }

        //Paged
        protected CBinaryFileList SelectByIsSchema(CPagingInfo pi, bool isSchema)    {    return SelectWhere(pi, new CCriteriaList("IsSchema", isSchema));    }

        //Count
        protected int SelectCountByIsSchema(bool isSchema)   {   return SelectCount(new CCriteriaList("IsSchema", isSchema));     }

        //Transactional
        internal CBinaryFileList SelectByIsSchema(bool isSchema, IDbTransaction tx)    {    return SelectWhere(new CCriteriaList("IsSchema", isSchema), tx);    }
        #endregion

        #region Static - Cache Implementation
        public static CBinaryFileList Cache
        {
            get
            {
                CBinaryFileList cache = (CBinaryFileList)CCache.Get(CACHE_KEY);
                if (cache == null)
                {
                    lock (typeof(CBinaryFileList))
                    {
                        cache = (CBinaryFileList)CCache.Get(CACHE_KEY);
                        if (cache == null)
                        {
                            cache = LoadCache();
                            CBinaryFile.Cache = cache;
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

            lock (typeof(CBinaryFileList))
            {
                CBinaryFileList temp = new CBinaryFileList(Cache);
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

            lock (typeof(CBinaryFileList))
            {
                CBinaryFileList temp = new CBinaryFileList(Cache);
                temp.Add(this);
                SetCache(temp);
            }
        }
        protected override void CacheUpdate()
        {
            if (CacheIsNull)
                return;

            lock (typeof(CBinaryFileList))
            {
                CBinaryFileList temp = new CBinaryFileList(Cache);
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
            Store(w, "MD5", this.MD5);
            Store(w, "Path", this.Path);
            Store(w, "BinGz", this._binGz);
            Store(w, "Size", this.Size);
            Store(w, "IsSchema", this.IsSchema);
            Store(w, "Created", this.Created);
            Store(w, "Deleted", this.Deleted);
        }
        #endregion

    }
}
