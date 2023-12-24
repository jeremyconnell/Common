using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
	//Table-Mapping Class (Customisable half)
	public partial class CInstance
	{
		#region Constants
		#endregion

		#region Constructors (Public)
		//Default Connection String
		public CInstance() : base() { }

		//Alternative Connection String
		public CInstance(CDataSrc dataSrc) : base(dataSrc) { }

		//Hidden  (UI code should use cache instead)
		protected internal CInstance(int instanceId) : base(instanceId) { }
		protected internal CInstance(CDataSrc dataSrc, int instanceId) : base(dataSrc, instanceId) { }
		protected internal CInstance(CDataSrc dataSrc, int instanceId, IDbTransaction txOrNull) : base(dataSrc, instanceId, txOrNull) { }
		#endregion

		#region Default Values
		protected override void InitValues_Custom()
		{
			_instanceCreated = DateTime.Now;
			_instanceAppLogin = "admin";
			_instanceAppPassword = "password1234";
			_instanceWebSubDir = "/webapi";
			_instanceWebUseSsl = true;
		}
		#endregion

		#region Default Connection String
		protected override CDataSrc DefaultDataSrc() { return CDataSrc.Default; }
		#endregion

		#region Properties - Relationships
		//Relationships - Foriegn Keys (e.g parent)
		public CApp App { get { return CApp.Cache.GetById(this.InstanceAppId); } }
		public CVersion SpecialVersion { get { return CVersion.Cache.GetById(this.InstanceSpecialVersionId); } }

		//Grand-parent
		public CVersion TargetVersion { get { return CVersion.Cache.GetById(this.TargetVersionId); } }

		//Relationships - Collections (e.g. children)
		public CValueList Values { get { return CValue.Cache.GetByInstanceId(this.InstanceId); } }
		protected CValueList Values_(IDbTransaction tx) { return new CValue(DataSrc).SelectByInstanceId(this.InstanceId, tx); } //Only used for cascade deletes


		public CPushedUpgradeList PushedUpgrades { get { return CPushedUpgrade.Cache.GetByInstanceId(this.InstanceId); } }
		protected CPushedUpgradeList PushedUpgrades_(IDbTransaction tx) { return new CPushedUpgrade(DataSrc).SelectByInstanceId(this.InstanceId, tx); } //Only used for cascade deletes


		public int SelfUpgradesCount { get { return new CUpgradeHistory().SelectCountByInstanceId(this.InstanceId); } }
		public CUpgradeHistoryList SelfUpgrades() { return new CUpgradeHistory().SelectByInstanceId(this.InstanceId); }
		public CUpgradeHistoryList SelfUpgrades(CPagingInfo pi) { return new CUpgradeHistory().SelectByInstanceId(pi, this.InstanceId); }
		protected CUpgradeHistoryList SelfUpgrades_(IDbTransaction tx) { return new CUpgradeHistory(DataSrc).SelectByInstanceId(this.InstanceId, tx); } //Only used for cascade deletes

		public int ReportCount { get { return new CReportHistory().SelectCountByInstanceId(this.InstanceId); } }
		public CReportHistoryList Reports() { return new CReportHistory().SelectByInstanceId(this.InstanceId); }
		public CReportHistoryList Reports(CPagingInfo pi) { return new CReportHistory().SelectByInstanceId(pi, this.InstanceId); }
		protected CReportHistoryList Reports_(IDbTransaction tx) { return new CReportHistory(DataSrc).SelectByInstanceId(this.InstanceId, tx); } //Only used for cascade deletes

		public CReportHistory LastReport()
		{
			var list = Reports(new CPagingInfo(1));
			return list.Count > 0 ? list[0] : null;
		}


		public CReleaseList BranchReleases { get { return CRelease.Cache.GetByInstanceId(this.InstanceId); } }
		protected CReleaseList BranchReleases_(IDbTransaction tx) { return new CRelease(DataSrc).SelectByInstanceId(this.InstanceId, tx); } //Only used for cascade deletes

		public CReleaseList AllReleases
		{
			get
			{
				var temp = new CReleaseList(App.ReleasesMain.Count + BranchReleases.Count);
				temp.AddRange(App.ReleasesMain);
				temp.AddRange(BranchReleases);
				temp.Sort();
				return temp;
			}
		}
		#endregion

		#region Properties - Customisation
		[Obsolete]
		public string InstanceName { get { return NameAndSuffix; } set { var ss = CUtilities.SplitOn(value, "-"); InstanceClientName = ss[0]; if (ss.Count > 1) InstanceSuffix = ss[1]; } }
		public string NameAndSuffix { get { return string.IsNullOrEmpty(InstanceSuffix) ? (string.IsNullOrEmpty(InstanceClientName) ? InstanceWebHostName : InstanceClientName) : string.Concat(InstanceClientName, "-", InstanceSuffix); } }

		public int ValuesCount { get { return Values.Count; } }
		public string NameAndValueCount { get { return CUtilities.NameAndCount(CUtilities.Truncate(NameAndSuffix, 40), Values); } }
		public string NameAndReleaseCount { get { return CUtilities.NameAndCount(CUtilities.Truncate(NameAndSuffix, 40), BranchReleases.Count + App.ReleasesMain.Count); } }

		//Derived/ReadOnly (e.g. xml classes, presentation logic)
		public string IdAndName { get { return string.Concat("#", this.InstanceId, ": ", this.NameAndSuffix); } }
		public int MainVersionId { get { return App.AppMainVersionId; } }
		public int TargetVersionId { get { return IsBranch ? InstanceSpecialVersionId : App.AppMainVersionId; } }
		public bool IsBranch { get { return int.MinValue != InstanceSpecialVersionId; } }


		//Sql

		public string Sql_AddUser_Saas { get { return string.Concat("CREATE LOGIN [", this.InstanceDbUserName, "]  WITH PASSWORD = '", InstanceDbPassword, "', DEFAULT_SCHEMA=[dbo]"); ; } }
		public string Sql_AddUser_Local { get { return string.Concat("CREATE USER [", InstanceDbUserName, "] FOR LOGIN [", InstanceDbUserName, "]"); } }    //todo: sepearate login field
		public string Sql_AddRole_Local { get { return string.Concat("EXEC sp_addrolemember N'db_owner', N'", InstanceDbUserName, "'"); } }

		//Active
		private CSqlClient _db;
		public CSqlClient Database
		{
			get
			{
				if (null == _db)
				{
					string cs = this.InstanceDbConnectionString;
					if (string.IsNullOrEmpty(cs))
						return null;

					if (cs.EndsWith("\""))
					{
						cs = cs.Substring(0, cs.Length - 1);
						this.InstanceDbConnectionString = cs;
						this.Save();
					}

					_db = new CSqlClient(cs);
				}
				return _db;
			}
		}
		private CWebSrcBinary _web;
		private bool _notDeployed = false;
		public CWebSrcBinary DatabaseViaWeb
		{
			get
			{
				if (_notDeployed)
					return null;

				if (null == _web && !_notDeployed)
				{
					if (!string.IsNullOrEmpty(this.InstanceWebHostName))
						_notDeployed = true;
					else
						_web = new CWebSrcBinary("https://" + this.InstanceWebHostName + this.InstanceWebSubDir, CConfigBase.WebServicePassword);

					try
					{
						_web.AllTableNames(true);
					}
					catch
					{
						_notDeployed = true;
						_web = null;
					}
				}
				return _web;
			}
		}

		private string _errorMessageSchema;
		private string _errorMessageMigHist;
		public string ErrorMessage_Schema { get { return _errorMessageSchema; } }
		public string ErrorMessage_MigHist { get { return _errorMessageMigHist; } }

		private CSchemaInfo _schemaInfo;
		public CSchemaInfo SchemaInfo
		{
			get
			{
				if (null == _schemaInfo && null == _errorMessageSchema)
				{
					//Check for connection string (or Url)
					if (null == this.Database)
					{
						_errorMessageSchema = "No Connection string!";
						if (null == this.DatabaseViaWeb)
						{
							_errorMessageSchema += " No Subdomain!";
							return null;
						}
					}

					//Use Connection String (otherwise url)
					try
					{
						if (null == this.Database)
							throw new Exception("No Connection string!");
						_schemaInfo = new CSchemaInfo(this.Database);
						_errorMessageSchema = null;
					}
					catch (Exception ex)
					{
						_errorMessageSchema = ex.Message;
						try
						{
							if (null == this.DatabaseViaWeb)
								throw new Exception("No Subdomain!");
							_schemaInfo = new CSchemaInfo(this.DatabaseViaWeb);
							_errorMessageSchema = null;
						}
						catch (Exception ex2)
						{
							if (ex2.Message.Contains("resource cannot be found"))
								_errorMessageSchema += "\r\nNo Webservice";
							else
								_errorMessageSchema += "\r\n\r\n" + ex2.Message;
						}
					}

				}
				return _schemaInfo;
			}
			set
			{
				_schemaInfo = null;
				_errorMessageSchema = null;
			}
		}

		public CIndexInfoList IndexInfo
		{
			get
			{
				return SchemaInfo.Indexes;
			}
		}
		public CMigration Migration
		{
			get
			{
				if (null == _schemaInfo)
					return new CMigration(this.Database);
				if (null == _schemaInfo.Migration)
					_schemaInfo.Migration = new CMigration(this.Database);
				return _schemaInfo.Migration;
			}
		}
		//public CMigrationHistory FullMigrationHistory
		//{
		//    get
		//    {
		//        var s = SchemaInfo;
		//        if (null == s.MigrationHistory)
		//            if (null == _errorMessageMigHist)
		//                if (null == _errorMessageSchema)
		//                    try
		//                    {
		//                        s.MigrationHistory = this.Database.MigrationHistory();
		//                    }
		//                    catch (Exception ex)
		//                    {
		//                        _errorMessageMigHist = ex.Message;
		//                    }
		//                else
		//                    _errorMessageMigHist = _errorMessageSchema;
		//        return s.MigrationHistory;
		//    }
		//}

		//public bool ForceMigrationHistory(CInstance refVersion)
		//{
		//    var history = refVersion.FullMigrationHistory;
		//    var changes = history.GetChanges(this.Migration);

		//    //Insert
		//    var db = this.Database;
		//    foreach (var i in changes)
		//        i.InsertInto(db);

		//    //Update cache
		//    if (null != _schemaInfo)
		//    {
		//        _schemaInfo.MigrationHistory = null; ;
		//        _schemaInfo.Migration = null;
		//    }
		//    return true;
		//}

		//public List<IDbCommand> ForceMigrationCommands(CInstance refVersion)
		//{
		//    var history = refVersion.FullMigrationHistory;
		//    var changes = history.GetChanges(this.Migration);

		//    //Insert
		//    var db = this.Database;
		//    var list = new List<IDbCommand>();
		//    foreach (var i in changes)
		//        list.Add(i.InsertCmd(db));
		//    return list;
		//}
		//public string ForceMigrationScript(CInstance refVersion)
		//{

		//    //Insert
		//    var sb = new StringBuilder();
		//    foreach (var cmd in ForceMigrationCommands(refVersion))
		//    {
		//        if (sb.Length == 0)
		//            sb.AppendLine(cmd.CommandText);
		//        sb.Append("{");

		//        foreach (IDataParameter p in cmd.Parameters)
		//            if (p.Value is byte[])
		//                sb.Append(CUtilities.CountSummary(((byte[])p.Value).Length, "byte")).Append(",");
		//            else if (p.Value is int)
		//                sb.Append(CUtilities.CountSummary((int)p.Value, "byte")).Append(",");
		//            else
		//                sb.Append(p.Value).Append(",");
		//        sb.Append("}");
		//    }
		//    return sb.ToString();
		//}
		#endregion

		#region Save/Delete Overrides
		public override void Save(IDbTransaction txOrNull)
		{
			var isEdit = !this.m_insertPending;
			var old = isEdit ? new CInstance(InstanceId) : null;

			base.Save(txOrNull);

			if (null != old)
				if (old.InstanceSpecialVersionId != InstanceSpecialVersionId)
				{
					var m = "Main: " + App?.MainVersion?.VersionName;
					var b = "Branch: " + SpecialVersion?.VersionName;

					var h = new CRelease();
					h.ReleaseAppId = App.AppId;
					h.ReleaseAppName = App.AppName;
					h.ReleaseInstanceId = InstanceId;
					h.ReleaseVersionId = InstanceSpecialVersionId;
					h.ReleaseVersionName = (null == SpecialVersion) ? m : b;
					h.ReleaseBranchName = SpecialVersion?.VersionName;
					h.Save(txOrNull);

					//Mark old versions with a time-to-live
					this.BranchReleases.ExpireOldOnes();
					this.App.BinaryFiles.DeleteIfExpiredFor(this.App.AppKeepOldFilesForDays);
				}
		}
		public override void Delete(IDbTransaction txOrNull)
		{
			//Use a transaction if none supplied 
			if (txOrNull == null)
			{
				BulkDelete(this);
				return;
			}

			//Cascade-Delete (all child collections) 
			this.PushedUpgrades_(txOrNull).DeleteAll(txOrNull);
			this.SelfUpgrades_(txOrNull).DeleteAll(txOrNull);
			this.Reports_(txOrNull).DeleteAll(txOrNull);
			this.BranchReleases_(txOrNull).DeleteAll(txOrNull);


			//Normal Delete 
			base.Delete(txOrNull);
		}
		#endregion

		#region Custom Database Queries
		//(Not normally required for cached classes, use list class for searching etc)
		//For Stored Procs can use: MakeList (matching schema), or DataSrc.ExecuteDataset (reports etc)
		//For Dynamic sql, can use: SelectSum, SelectDistinct, SelectCount, SelectWhere (inherited methods)
		#endregion

		#region Searching (Optional)
		//For cached classes, custom seach logic resides in static methods on the list class
		// e.g. CInstance.Cache.Search("...")

		//See also the auto-generated methods based on indexes
		//' e.g. CInstance.Cache.GetBy...
		#endregion

		#region Caching Details
		//Cache Key
		internal static string CACHE_KEY = typeof(CInstance).ToString();    //TABLE_NAME

		//Cache data
		private static CInstanceList LoadCache() { return new CInstance().SelectAll(); }
		//Cache Timeout
		private static void SetCache(CInstanceList value)
		{
			if (null != value)
				value.Sort();
			CCache.Set(CACHE_KEY, value);    //Optional parameter can override timeout (otherwise uses config-settings, which default to 3hrs)
		}
		//Helper Method
		private CInstance CacheGetById(CInstanceList list) { return list.GetById(this.InstanceId); }
		#endregion

		#region Cloning
		public CInstance Clone(CDataSrc target, IDbTransaction txOrNull) //, int parentId)
		{
			//Shallow copy: Copies the immediate record, excluding autogenerated Pks
			CInstance copy = new CInstance(this, target);

			//Deep Copy - Child Entities: Cloned children must reference their cloned parent
			//copy.SampleParentId = parentId;

			copy.Save(txOrNull);

			//Deep Copy - Parent Entities: Cloned parents also clone their child collections
			//this.Children.Clone(target, txOrNull, copy.InstanceId);

			return copy;
		}
		#endregion

		#region ToXml
		protected override void ToXml_Custom(System.Xml.XmlWriter w)
		{
			//Store(w, "Example", this.Example)
		}
		#endregion
	}
}