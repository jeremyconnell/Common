using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
	[Serializable()]
	public partial class CInstance : CBaseDynamic, IComparable<CInstance>
	{
		#region Constructors
		//Public (Copy Constructor)
		public CInstance(CInstance original, CDataSrc target)
		{
			//Database Connection
			m_dataSrc = target;

			//Copy fields
			_instanceClientId = original.InstanceClientId;
			_instanceClientName = original.InstanceClientName;
			_instanceClientCode = original.InstanceClientCode;
			_instanceSuffix = original.InstanceSuffix;
			_instanceAppId = original.InstanceAppId;
			_instanceSpecialVersionId = original.InstanceSpecialVersionId;
			_instanceSpecialVersionName = original.InstanceSpecialVersionName;
			_instanceWebNameAzure = original.InstanceWebNameAzure;
			_instanceWebHostName = original.InstanceWebHostName;
			_instanceWebUseSsl = original.InstanceWebUseSsl;
			_instanceWebSubDir = original.InstanceWebSubDir;
			_instanceDbNameAzure = original.InstanceDbNameAzure;
			_instanceDbLogin = original.InstanceDbLogin;
			_instanceDbUserName = original.InstanceDbUserName;
			_instanceDbPassword = original.InstanceDbPassword;
			_instanceDbConnectionString = original.InstanceDbConnectionString;
			_instanceAppLogin = original.InstanceAppLogin;
			_instanceAppPassword = original.InstanceAppPassword;
			_instanceSettingsImported = original.InstanceSettingsImported;
			_instanceSettingsExported = original.InstanceSettingsExported;
			_instanceCreated = original.InstanceCreated;
		}

		//Protected (Datareader/Dataset)
		protected CInstance(CDataSrc dataSrc, IDataReader dr) : base(dataSrc, dr) { }
		protected CInstance(CDataSrc dataSrc, DataRow dr) : base(dataSrc, dr) { }
		#endregion

		#region Default Values
		protected override void InitValues_Auto()
		{
			//Null-Equivalent values (except String.Empty, as nulls tend to be inconvenient for string types)
			_instanceId = int.MinValue;
			_instanceClientId = int.MinValue;
			_instanceClientName = string.Empty;
			_instanceClientCode = string.Empty;
			_instanceSuffix = string.Empty;
			_instanceAppId = int.MinValue;
			_instanceSpecialVersionId = int.MinValue;
			_instanceSpecialVersionName = string.Empty;
			_instanceWebNameAzure = string.Empty;
			_instanceWebHostName = string.Empty;
			_instanceWebUseSsl = false;
			_instanceWebSubDir = string.Empty;
			_instanceDbNameAzure = string.Empty;
			_instanceDbLogin = string.Empty;
			_instanceDbUserName = string.Empty;
			_instanceDbPassword = string.Empty;
			_instanceDbConnectionString = string.Empty;
			_instanceAppLogin = string.Empty;
			_instanceAppPassword = string.Empty;
			_instanceSettingsImported = DateTime.MinValue;
			_instanceSettingsExported = DateTime.MinValue;
			_instanceCreated = DateTime.MinValue;
		}
		#endregion

		#region Members
		protected int _instanceId;
		protected int _instanceClientId;
		protected string _instanceClientName;
		protected string _instanceClientCode;
		protected string _instanceSuffix;
		protected int _instanceAppId;
		protected int _instanceSpecialVersionId;
		protected string _instanceSpecialVersionName;
		protected string _instanceWebNameAzure;
		protected string _instanceWebHostName;
		protected bool _instanceWebUseSsl;
		protected string _instanceWebSubDir;
		protected string _instanceDbNameAzure;
		protected string _instanceDbLogin;
		protected string _instanceDbUserName;
		protected string _instanceDbPassword;
		protected string _instanceDbConnectionString;
		protected string _instanceAppLogin;
		protected string _instanceAppPassword;
		protected DateTime _instanceSettingsImported;
		protected DateTime _instanceSettingsExported;
		protected DateTime _instanceCreated;
		#endregion

		#region Properties - Column Values
		//Primary Key Column (ReadOnly)
		public int InstanceId { get { return _instanceId; } }

		//Table Columns (Read/Write)
		public int InstanceClientId { get { return _instanceClientId; } set { _instanceClientId = value; } }
		public string InstanceClientName { get { return _instanceClientName; } set { _instanceClientName = value; } }
		public string InstanceClientCode { get { return _instanceClientCode; } set { _instanceClientCode = value; } }
		public string InstanceSuffix { get { return _instanceSuffix; } set { _instanceSuffix = value; } }
		public int InstanceAppId { get { return _instanceAppId; } set { _instanceAppId = value; } }
		public int InstanceSpecialVersionId { get { return _instanceSpecialVersionId; } set { _instanceSpecialVersionId = value; } }
		public string InstanceSpecialVersionName { get { return _instanceSpecialVersionName; } set { _instanceSpecialVersionName = value; } }
		public string InstanceWebNameAzure { get { return _instanceWebNameAzure; } set { _instanceWebNameAzure = value; } }
		public string InstanceWebHostName { get { return _instanceWebHostName; } set { _instanceWebHostName = value; } }
		public bool InstanceWebUseSsl { get { return _instanceWebUseSsl; } set { _instanceWebUseSsl = value; } }
		public string InstanceWebSubDir { get { return _instanceWebSubDir; } set { _instanceWebSubDir = value; } }
		public string InstanceDbNameAzure { get { return _instanceDbNameAzure; } set { _instanceDbNameAzure = value; } }
		public string InstanceDbLogin { get { return _instanceDbLogin; } set { _instanceDbLogin = value; } }
		public string InstanceDbUserName { get { return _instanceDbUserName; } set { _instanceDbUserName = value; } }
		public string InstanceDbPassword { get { return _instanceDbPassword; } set { _instanceDbPassword = value; } }
		public string InstanceDbConnectionString { get { return _instanceDbConnectionString; } set { _instanceDbConnectionString = value; } }
		public string InstanceAppLogin { get { return _instanceAppLogin; } set { _instanceAppLogin = value; } }
		public string InstanceAppPassword { get { return _instanceAppPassword; } set { _instanceAppPassword = value; } }
		public DateTime InstanceSettingsImported { get { return _instanceSettingsImported; } set { _instanceSettingsImported = value; } }
		public DateTime InstanceSettingsExported { get { return _instanceSettingsExported; } set { _instanceSettingsExported = value; } }
		public DateTime InstanceCreated { get { return _instanceCreated; } set { _instanceCreated = value; } }

		//View Columns (ReadOnly)

		#endregion

		#region MustOverride Methods
		//Schema Information
		public const string TABLE_NAME = "Deploy_Instance";
		public const string VIEW_NAME = "";         //Used to override this.ViewName { get }
		public const string ORDER_BY_COLS = "InstanceClientName, InstanceSuffix";   //See the CompareTo method below (Sql-based sorting should match In-Memory sorting)
		public const string SORTING_COLUMN = "";
		public override string TableName { get { return TABLE_NAME; } }
		protected override string OrderByColumns { get { return ORDER_BY_COLS; } }

		//CompareTo Interface (Default Sort Order)
		public int CompareTo(CInstance other) { return this.InstanceName.CompareTo(other.InstanceName); }

		//Primary Key Information
		public const string PRIMARY_KEY_NAME = "InstanceId";
		protected override bool InsertPrimaryKey { get { return false; } }
		protected override string PrimaryKeyName { get { return PRIMARY_KEY_NAME; } }
		protected override object PrimaryKeyValue
		{
			get { return _instanceId; }
			set { _instanceId = (int)value; }
		}

		//Factory Methods - Object
		protected override CBase MakeFrom(DataRow row) { return new CInstance(this.DataSrc, row); }
		protected override CBase MakeFrom(IDataReader dr) { return new CInstance(this.DataSrc, dr); }

		//Factory Methods - List
		protected override IList MakeList() { return new CInstanceList(); }
		protected override IList MakeList(int capacity) { return new CInstanceList(capacity); }

		//Convert from ADO to .Net
		protected override void ReadColumns(IDataReader dr)
		{
			_instanceId = CAdoData.GetInt(dr, "InstanceId");
			_instanceClientId = CAdoData.GetInt(dr, "InstanceClientId");
			_instanceClientName = CAdoData.GetStr(dr, "InstanceClientName");
			_instanceClientCode = CAdoData.GetStr(dr, "InstanceClientCode");
			_instanceSuffix = CAdoData.GetStr(dr, "InstanceSuffix");
			_instanceAppId = CAdoData.GetInt(dr, "InstanceAppId");
			_instanceSpecialVersionId = CAdoData.GetInt(dr, "InstanceSpecialVersionId");
			_instanceSpecialVersionName = CAdoData.GetStr(dr, "InstanceSpecialVersionName");
			_instanceWebNameAzure = CAdoData.GetStr(dr, "InstanceWebNameAzure");
			_instanceWebHostName = CAdoData.GetStr(dr, "InstanceWebHostName");
			_instanceWebUseSsl = CAdoData.GetBool(dr, "InstanceWebUseSsl");
			_instanceWebSubDir = CAdoData.GetStr(dr, "InstanceWebSubDir");
			_instanceDbNameAzure = CAdoData.GetStr(dr, "InstanceDbNameAzure");
			_instanceDbLogin = CAdoData.GetStr(dr, "InstanceDbLogin");
			_instanceDbUserName = CAdoData.GetStr(dr, "InstanceDbUserName");
			_instanceDbPassword = CAdoData.GetStr(dr, "InstanceDbPassword");
			_instanceDbConnectionString = CAdoData.GetStr(dr, "InstanceDbConnectionString");
			_instanceAppLogin = CAdoData.GetStr(dr, "InstanceAppLogin");
			_instanceAppPassword = CAdoData.GetStr(dr, "InstanceAppPassword");
			_instanceSettingsImported = CAdoData.GetDate(dr, "InstanceSettingsImported");
			_instanceSettingsExported = CAdoData.GetDate(dr, "InstanceSettingsExported");
			_instanceCreated = CAdoData.GetDate(dr, "InstanceCreated");
		}
		protected override void ReadColumns(DataRow dr)
		{
			_instanceId = CAdoData.GetInt(dr, "InstanceId");
			_instanceClientId = CAdoData.GetInt(dr, "InstanceClientId");
			_instanceClientName = CAdoData.GetStr(dr, "InstanceClientName");
			_instanceClientCode = CAdoData.GetStr(dr, "InstanceClientCode");
			_instanceSuffix = CAdoData.GetStr(dr, "InstanceSuffix");
			_instanceAppId = CAdoData.GetInt(dr, "InstanceAppId");
			_instanceSpecialVersionId = CAdoData.GetInt(dr, "InstanceSpecialVersionId");
			_instanceSpecialVersionName = CAdoData.GetStr(dr, "InstanceSpecialVersionName");
			_instanceWebNameAzure = CAdoData.GetStr(dr, "InstanceWebNameAzure");
			_instanceWebHostName = CAdoData.GetStr(dr, "InstanceWebHostName");
			_instanceWebUseSsl = CAdoData.GetBool(dr, "InstanceWebUseSsl");
			_instanceWebSubDir = CAdoData.GetStr(dr, "InstanceWebSubDir");
			_instanceDbNameAzure = CAdoData.GetStr(dr, "InstanceDbNameAzure");
			_instanceDbLogin = CAdoData.GetStr(dr, "InstanceDbLogin");
			_instanceDbUserName = CAdoData.GetStr(dr, "InstanceDbUserName");
			_instanceDbPassword = CAdoData.GetStr(dr, "InstanceDbPassword");
			_instanceDbConnectionString = CAdoData.GetStr(dr, "InstanceDbConnectionString");
			_instanceAppLogin = CAdoData.GetStr(dr, "InstanceAppLogin");
			_instanceAppPassword = CAdoData.GetStr(dr, "InstanceAppPassword");
			_instanceSettingsImported = CAdoData.GetDate(dr, "InstanceSettingsImported");
			_instanceSettingsExported = CAdoData.GetDate(dr, "InstanceSettingsExported");
			_instanceCreated = CAdoData.GetDate(dr, "InstanceCreated");
		}

		//Parameters for Insert/Update    
		protected override CNameValueList ColumnNameValues()
		{
			CNameValueList data = new CNameValueList();
			data.Add("InstanceId", CAdoData.NullVal(_instanceId));
			data.Add("InstanceClientId", CAdoData.NullVal(_instanceClientId));
			data.Add("InstanceClientName", CAdoData.NullVal(_instanceClientName));
			data.Add("InstanceClientCode", CAdoData.NullVal(_instanceClientCode));
			data.Add("InstanceSuffix", CAdoData.NullVal(_instanceSuffix));
			data.Add("InstanceAppId", CAdoData.NullVal(_instanceAppId));
			data.Add("InstanceSpecialVersionId", CAdoData.NullVal(_instanceSpecialVersionId));
			data.Add("InstanceSpecialVersionName", CAdoData.NullVal(_instanceSpecialVersionName));
			data.Add("InstanceWebNameAzure", CAdoData.NullVal(_instanceWebNameAzure));
			data.Add("InstanceWebHostName", CAdoData.NullVal(_instanceWebHostName));
			data.Add("InstanceWebUseSsl", CAdoData.NullVal(_instanceWebUseSsl));
			data.Add("InstanceWebSubDir", CAdoData.NullVal(_instanceWebSubDir));
			data.Add("InstanceDbNameAzure", CAdoData.NullVal(_instanceDbNameAzure));
			data.Add("InstanceDbLogin", CAdoData.NullVal(_instanceDbLogin));
			data.Add("InstanceDbUserName", CAdoData.NullVal(_instanceDbUserName));
			data.Add("InstanceDbPassword", CAdoData.NullVal(_instanceDbPassword));
			data.Add("InstanceDbConnectionString", CAdoData.NullVal(_instanceDbConnectionString));
			data.Add("InstanceAppLogin", CAdoData.NullVal(_instanceAppLogin));
			data.Add("InstanceAppPassword", CAdoData.NullVal(_instanceAppPassword));
			data.Add("InstanceSettingsImported", CAdoData.NullVal(_instanceSettingsImported));
			data.Add("InstanceSettingsExported", CAdoData.NullVal(_instanceSettingsExported));
			data.Add("InstanceCreated", CAdoData.NullVal(_instanceCreated));
			return data;
		}
		#endregion

		#region Queries - SelectAll/SelectWhere (inherited methods, cast only)
		//Normally used to load the cache
		public new CInstanceList SelectAll() { return (CInstanceList)base.SelectAll(); }
		public new CInstanceList SelectAll(string orderBy) { return (CInstanceList)base.SelectAll(orderBy); }

		//Sometimes use a custom query to load the cache
		protected new CInstanceList SelectWhere(string colName, ESign sign, object colValue) { return (CInstanceList)base.SelectWhere(colName, sign, colValue); }
		protected new CInstanceList SelectWhere(CCriteria where) { return (CInstanceList)base.SelectWhere(where); }
		protected new CInstanceList SelectWhere(CCriteriaList where) { return (CInstanceList)base.SelectWhere(where); }
		protected new CInstanceList SelectWhere(CCriteriaList where, string tableOrJoin) { return (CInstanceList)base.SelectWhere(where, tableOrJoin, this.OrderByColumns); }
		protected new CInstanceList SelectWhere(CCriteriaList where, string tableOrJoin, string orderBy) { return (CInstanceList)base.SelectWhere(where, tableOrJoin, orderBy); }
		[Obsolete("Arbitrary where-clause is not necessary portable or safe from sql injection attacks. Consider using the parameterised query interfaces (or a stored proc)")]
		protected new CInstanceList SelectWhere(string unsafeWhereClause) { return (CInstanceList)base.SelectWhere(unsafeWhereClause); }
		protected CInstanceList SelectById(int instanceId) { return (CInstanceList)base.SelectById(instanceId); }
		protected CInstanceList SelectByIds(List<int> ids) { return (CInstanceList)base.SelectByIds(ids); }

		//Select Queries - Paged
		protected new CInstanceList SelectAll(CPagingInfo pi) { return (CInstanceList)base.SelectAll(pi); }
		protected new CInstanceList SelectWhere(CPagingInfo pi, string name, ESign sign, object value) { return (CInstanceList)base.SelectWhere(pi, name, sign, value); }
		protected new CInstanceList SelectWhere(CPagingInfo pi, CCriteria criteria) { return (CInstanceList)base.SelectWhere(pi, criteria); }
		protected new CInstanceList SelectWhere(CPagingInfo pi, CCriteriaList criteria) { return (CInstanceList)base.SelectWhere(pi, criteria); }
		protected new CInstanceList SelectWhere(CPagingInfo pi, CCriteriaList criteria, string viewOrJoin) { return (CInstanceList)base.SelectWhere(pi, criteria, viewOrJoin); }

		//Select Queries - Transactional (Internal scope for use in cascade deletes)
		internal new CInstanceList SelectAll(IDbTransaction tx) { return (CInstanceList)base.SelectAll(tx); }
		internal new CInstanceList SelectAll(string orderBy, IDbTransaction tx) { return (CInstanceList)base.SelectAll(orderBy, tx); }
		internal new CInstanceList SelectWhere(string columnName, object columnValue, IDbTransaction tx) { return (CInstanceList)base.SelectWhere(columnName, columnValue, tx); }
		internal new CInstanceList SelectWhere(string columnName, ESign sign, object columnValue, IDbTransaction tx) { return (CInstanceList)base.SelectWhere(columnName, sign, columnValue, tx); }
		internal new CInstanceList SelectWhere(CCriteria criteria, IDbTransaction tx) { return (CInstanceList)base.SelectWhere(criteria, tx); }
		internal new CInstanceList SelectWhere(CCriteriaList criteria, IDbTransaction tx) { return (CInstanceList)base.SelectWhere(criteria, tx); }
		internal new CInstanceList SelectWhere(CCriteriaList criteria, string tableOrJoin, IDbTransaction tx) { return (CInstanceList)base.SelectWhere(criteria, tableOrJoin, tx); }
		internal new CInstanceList SelectWhere(CCriteriaList criteria, string tableOrJoin, string orderBy, IDbTransaction tx) { return (CInstanceList)base.SelectWhere(criteria, tableOrJoin, tx); }
		internal CInstanceList SelectById(int instanceId, IDbTransaction tx) { return (CInstanceList)base.SelectById(instanceId, tx); }
		internal CInstanceList SelectByIds(List<int> ids, IDbTransaction tx) { return (CInstanceList)base.SelectByIds(ids, tx); }

		//Select Queries - Stored Procs
		protected new CInstanceList MakeList(string storedProcName, IDbTransaction txOrNull) { return (CInstanceList)base.MakeList(storedProcName, txOrNull); }
		protected new CInstanceList MakeList(string storedProcName, object[] parameters, IDbTransaction txOrNull) { return (CInstanceList)base.MakeList(storedProcName, parameters, txOrNull); }
		protected new CInstanceList MakeList(string storedProcName, CNameValueList parameters, IDbTransaction txOrNull) { return (CInstanceList)base.MakeList(storedProcName, parameters, txOrNull); }
		protected new CInstanceList MakeList(string storedProcName, List<object> parameters, IDbTransaction txOrNull) { return (CInstanceList)base.MakeList(storedProcName, parameters, txOrNull); }
		protected new CInstanceList MakeList(string storedProcName, int param1, IDbTransaction txOrNull) { return (CInstanceList)base.MakeList(storedProcName, param1, txOrNull); }
		protected new CInstanceList MakeList(string storedProcName, string param1, IDbTransaction txOrNull) { return (CInstanceList)base.MakeList(storedProcName, param1, txOrNull); }

		//Query Results
		protected new CInstanceList MakeList(DataSet ds) { return (CInstanceList)base.MakeList(ds); }
		protected new CInstanceList MakeList(DataTable dt) { return (CInstanceList)base.MakeList(dt); }
		protected new CInstanceList MakeList(DataRowCollection rows) { return (CInstanceList)base.MakeList(rows); }
		protected new CInstanceList MakeList(IDataReader dr) { return (CInstanceList)base.MakeList(dr); }
		protected new CInstanceList MakeList(object drOrDs) { return (CInstanceList)base.MakeList(drOrDs); }
		protected new CInstanceList MakeList(byte[] gzip) { return (CInstanceList)base.MakeList(gzip); }
		#endregion

		#region Queries - SelectBy[FK] (user-nominated fk/bool columns)
		//Note: These methods should only be used for cascade-deletes, or initialising the cache. Use CInstance.Cache.GetBy... for reqular queries

		//Non-Paged
		protected CInstanceList SelectByClientId(int instanceClientId) { return SelectWhere(new CCriteriaList("InstanceClientId", instanceClientId)); }
		protected CInstanceList SelectByClientName(string instanceClientName) { return SelectWhere(new CCriteriaList("InstanceClientName", instanceClientName)); }
		protected CInstanceList SelectByClientCode(string instanceClientCode) { return SelectWhere(new CCriteriaList("InstanceClientCode", instanceClientCode)); }
		protected CInstanceList SelectBySuffix(string instanceSuffix) { return SelectWhere(new CCriteriaList("InstanceSuffix", instanceSuffix)); }
		protected CInstanceList SelectByAppId(int instanceAppId) { return SelectWhere(new CCriteriaList("InstanceAppId", instanceAppId)); }
		protected CInstanceList SelectBySpecialVersionId(int instanceSpecialVersionId) { return SelectWhere(new CCriteriaList("InstanceSpecialVersionId", instanceSpecialVersionId)); }
		protected CInstanceList SelectByWebNameAzure(string instanceWebNameAzure) { return SelectWhere(new CCriteriaList("InstanceWebNameAzure", instanceWebNameAzure)); }
		protected CInstanceList SelectByWebHostName(string instanceWebHostName) { return SelectWhere(new CCriteriaList("InstanceWebHostName", instanceWebHostName)); }
		protected CInstanceList SelectByWebUseSsl(bool instanceWebUseSsl) { return SelectWhere(new CCriteriaList("InstanceWebUseSsl", instanceWebUseSsl)); }
		protected CInstanceList SelectByDbNameAzure(string instanceDbNameAzure) { return SelectWhere(new CCriteriaList("InstanceDbNameAzure", instanceDbNameAzure)); }

		//Paged
		protected CInstanceList SelectByClientId(CPagingInfo pi, int instanceClientId) { return SelectWhere(pi, new CCriteriaList("InstanceClientId", instanceClientId)); }
		protected CInstanceList SelectByClientName(CPagingInfo pi, string instanceClientName) { return SelectWhere(pi, new CCriteriaList("InstanceClientName", instanceClientName)); }
		protected CInstanceList SelectByClientCode(CPagingInfo pi, string instanceClientCode) { return SelectWhere(pi, new CCriteriaList("InstanceClientCode", instanceClientCode)); }
		protected CInstanceList SelectBySuffix(CPagingInfo pi, string instanceSuffix) { return SelectWhere(pi, new CCriteriaList("InstanceSuffix", instanceSuffix)); }
		protected CInstanceList SelectByAppId(CPagingInfo pi, int instanceAppId) { return SelectWhere(pi, new CCriteriaList("InstanceAppId", instanceAppId)); }
		protected CInstanceList SelectBySpecialVersionId(CPagingInfo pi, int instanceSpecialVersionId) { return SelectWhere(pi, new CCriteriaList("InstanceSpecialVersionId", instanceSpecialVersionId)); }
		protected CInstanceList SelectByWebNameAzure(CPagingInfo pi, string instanceWebNameAzure) { return SelectWhere(pi, new CCriteriaList("InstanceWebNameAzure", instanceWebNameAzure)); }
		protected CInstanceList SelectByWebHostName(CPagingInfo pi, string instanceWebHostName) { return SelectWhere(pi, new CCriteriaList("InstanceWebHostName", instanceWebHostName)); }
		protected CInstanceList SelectByWebUseSsl(CPagingInfo pi, bool instanceWebUseSsl) { return SelectWhere(pi, new CCriteriaList("InstanceWebUseSsl", instanceWebUseSsl)); }
		protected CInstanceList SelectByDbNameAzure(CPagingInfo pi, string instanceDbNameAzure) { return SelectWhere(pi, new CCriteriaList("InstanceDbNameAzure", instanceDbNameAzure)); }

		//Count
		protected int SelectCountByClientId(int instanceClientId) { return SelectCount(new CCriteriaList("InstanceClientId", instanceClientId)); }
		protected int SelectCountByClientName(string instanceClientName) { return SelectCount(new CCriteriaList("InstanceClientName", instanceClientName)); }
		protected int SelectCountByClientCode(string instanceClientCode) { return SelectCount(new CCriteriaList("InstanceClientCode", instanceClientCode)); }
		protected int SelectCountBySuffix(string instanceSuffix) { return SelectCount(new CCriteriaList("InstanceSuffix", instanceSuffix)); }
		protected int SelectCountByAppId(int instanceAppId) { return SelectCount(new CCriteriaList("InstanceAppId", instanceAppId)); }
		protected int SelectCountBySpecialVersionId(int instanceSpecialVersionId) { return SelectCount(new CCriteriaList("InstanceSpecialVersionId", instanceSpecialVersionId)); }
		protected int SelectCountByWebNameAzure(string instanceWebNameAzure) { return SelectCount(new CCriteriaList("InstanceWebNameAzure", instanceWebNameAzure)); }
		protected int SelectCountByWebHostName(string instanceWebHostName) { return SelectCount(new CCriteriaList("InstanceWebHostName", instanceWebHostName)); }
		protected int SelectCountByWebUseSsl(bool instanceWebUseSsl) { return SelectCount(new CCriteriaList("InstanceWebUseSsl", instanceWebUseSsl)); }
		protected int SelectCountByDbNameAzure(string instanceDbNameAzure) { return SelectCount(new CCriteriaList("InstanceDbNameAzure", instanceDbNameAzure)); }

		//Transactional
		internal CInstanceList SelectByClientId(int instanceClientId, IDbTransaction tx) { return SelectWhere(new CCriteriaList("InstanceClientId", instanceClientId), tx); }
		internal CInstanceList SelectByClientName(string instanceClientName, IDbTransaction tx) { return SelectWhere(new CCriteriaList("InstanceClientName", instanceClientName), tx); }
		internal CInstanceList SelectByClientCode(string instanceClientCode, IDbTransaction tx) { return SelectWhere(new CCriteriaList("InstanceClientCode", instanceClientCode), tx); }
		internal CInstanceList SelectBySuffix(string instanceSuffix, IDbTransaction tx) { return SelectWhere(new CCriteriaList("InstanceSuffix", instanceSuffix), tx); }
		internal CInstanceList SelectByAppId(int instanceAppId, IDbTransaction tx) { return SelectWhere(new CCriteriaList("InstanceAppId", instanceAppId), tx); }
		internal CInstanceList SelectBySpecialVersionId(int instanceSpecialVersionId, IDbTransaction tx) { return SelectWhere(new CCriteriaList("InstanceSpecialVersionId", instanceSpecialVersionId), tx); }
		internal CInstanceList SelectByWebNameAzure(string instanceWebNameAzure, IDbTransaction tx) { return SelectWhere(new CCriteriaList("InstanceWebNameAzure", instanceWebNameAzure), tx); }
		internal CInstanceList SelectByWebHostName(string instanceWebHostName, IDbTransaction tx) { return SelectWhere(new CCriteriaList("InstanceWebHostName", instanceWebHostName), tx); }
		internal CInstanceList SelectByWebUseSsl(bool instanceWebUseSsl, IDbTransaction tx) { return SelectWhere(new CCriteriaList("InstanceWebUseSsl", instanceWebUseSsl), tx); }
		internal CInstanceList SelectByDbNameAzure(string instanceDbNameAzure, IDbTransaction tx) { return SelectWhere(new CCriteriaList("InstanceDbNameAzure", instanceDbNameAzure), tx); }
		#endregion

		#region Static - Cache Implementation
		public static CInstanceList Cache
		{
			get
			{
				CInstanceList cache = (CInstanceList)CCache.Get(CACHE_KEY);
				if (cache == null)
				{
					lock (typeof(CInstanceList))
					{
						cache = (CInstanceList)CCache.Get(CACHE_KEY);
						if (cache == null)
						{
							cache = LoadCache();
							CInstance.Cache = cache;
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

			lock (typeof(CInstanceList))
			{
				CInstanceList temp = new CInstanceList(Cache);
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

			lock (typeof(CInstanceList))
			{
				CInstanceList temp = new CInstanceList(Cache);
				temp.Add(this);
				SetCache(temp);
			}
		}
		protected override void CacheUpdate()
		{
			if (CacheIsNull)
				return;

			lock (typeof(CInstanceList))
			{
				CInstanceList temp = new CInstanceList(Cache);
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
			Store(w, "InstanceId", this.InstanceId);
			Store(w, "InstanceClientId", this.InstanceClientId);
			Store(w, "InstanceClientName", this.InstanceClientName);
			Store(w, "InstanceClientCode", this.InstanceClientCode);
			Store(w, "InstanceSuffix", this.InstanceSuffix);
			Store(w, "InstanceAppId", this.InstanceAppId);
			Store(w, "InstanceSpecialVersionId", this.InstanceSpecialVersionId);
			Store(w, "InstanceSpecialVersionName", this.InstanceSpecialVersionName);
			Store(w, "InstanceWebNameAzure", this.InstanceWebNameAzure);
			Store(w, "InstanceWebHostName", this.InstanceWebHostName);
			Store(w, "InstanceWebUseSsl", this.InstanceWebUseSsl);
			Store(w, "InstanceWebSubDir", this.InstanceWebSubDir);
			Store(w, "InstanceDbNameAzure", this.InstanceDbNameAzure);
			Store(w, "InstanceDbLogin", this.InstanceDbLogin);
			Store(w, "InstanceDbUserName", this.InstanceDbUserName);
			Store(w, "InstanceDbPassword", this.InstanceDbPassword);
			Store(w, "InstanceDbConnectionString", this.InstanceDbConnectionString);
			Store(w, "InstanceAppLogin", this.InstanceAppLogin);
			Store(w, "InstanceAppPassword", this.InstanceAppPassword);
			Store(w, "InstanceSettingsImported", this.InstanceSettingsImported);
			Store(w, "InstanceSettingsExported", this.InstanceSettingsExported);
			Store(w, "InstanceCreated", this.InstanceCreated);
		}
		#endregion

	}
}
