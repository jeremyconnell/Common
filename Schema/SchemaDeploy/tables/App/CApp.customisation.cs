using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
	//Table-Mapping Class (Customisable half)
	public enum EApp
	{
		ControlTrack = 1
	}
	public partial class CApp
	{
		#region Constants
		#endregion

		#region Constructors (Public)
		//Default Connection String
		public CApp() : base() { }

		//Alternative Connection String
		public CApp(CDataSrc dataSrc) : base(dataSrc) { }

		//Hidden  (UI code should use cache instead)
		protected internal CApp(int appId) : base(appId) { }
		protected internal CApp(CDataSrc dataSrc, int appId) : base(dataSrc, appId) { }
		protected internal CApp(CDataSrc dataSrc, int appId, IDbTransaction txOrNull) : base(dataSrc, appId, txOrNull) { }
		#endregion

		#region Default Values
		protected override void InitValues_Custom()
		{
			_appCreated = DateTime.Now;
			_appKeepOldFilesForDays = 60;
		}
		#endregion

		#region Default Connection String
		protected override CDataSrc DefaultDataSrc() { return CDataSrc.Default; }
		#endregion

		#region Properties - Relationships
		//Relationships - Foriegn Keys (e.g parent)
		public CVersion MainVersion { get { return CVersion.Cache.GetById(this.AppMainVersionId); } }
		public int MainVersionFileCount { get { return null == Versions ? int.MinValue : MainVersion.VersionFiles.Count; } }



		//Relationships - Collections (e.g. children)
		public CGroupList Groups { get { return CGroup.Cache.GetByAppId(this.AppId); } }
		protected CGroupList Groups_(IDbTransaction tx) { return new CGroup(DataSrc).SelectByAppId(this.AppId, tx); } //Only used for cascade deletes

		public CVersionList Versions { get { return CVersion.Cache.GetByAppId(this.AppId); } }
		protected CVersionList Versions_(IDbTransaction tx) { return new CVersion(DataSrc).SelectByAppId(this.AppId, tx); } //Only used for cascade deletes
		public int VersionCount { get { return Versions.Count; } }

		public CInstanceList Instances { get { return CInstance.Cache.GetByAppId(this.AppId); } }
		protected CInstanceList Instances_(IDbTransaction tx) { return new CInstance(DataSrc).SelectByAppId(this.AppId, tx); } //Only used for cascade deletes
		public int InstanceCount { get { return Instances.Count; } }

		public CReleaseList Releases { get { return CRelease.Cache.GetByAppId(this.AppId); } }
		public CReleaseList ReleasesMain { get { return CRelease.Cache.GetByAppId(this.AppId).Main; } }
		protected CReleaseList ReleasesMain_(IDbTransaction tx) { return new CRelease(DataSrc).SelectByAppId_MainOnly(this.AppId, tx); } //Only used for cascade deletes

		//Relationships - 2-step
		public CBinaryFileList BinaryFiles { get { return Versions.BinaryFiles; } }
		protected int DeleteVersionFiles_(IDbTransaction tx) { return new CVersionFile(DataSrc).DeleteForAppId(this.AppId, tx); } //Only used for cascade deletes

		public long TotalBytes { get { return Versions.TotalBytes; } }
		public string TotalBytes_ { get { return Versions.TotalBytes_; } }
		#endregion

		#region Properties - Customisation
		//Derived/ReadOnly (e.g. xml classes, presentation logic)
		public string NameAndInstanceCount { get { return CUtilities.NameAndCount(AppName, InstanceCount); } }
		public string NameAndVersionCount { get { return CUtilities.NameAndCount(AppName, VersionCount); } }
		public string NameAndFileCount { get { return CUtilities.NameAndCount(AppName, BinaryFiles); } }
		public string NameAndReleaseCount { get { return CUtilities.NameAndCount(AppName, ReleasesMain); } }
		public string NameAndGroupCount { get { return CUtilities.NameAndCount(AppName, Groups); } }


		#endregion

		#region Save/Delete Overrides
		public override void Save(IDbTransaction txOrNull)
		{
			var isEdit = !this.m_insertPending;
			var old = isEdit ? new CApp(AppId) : null;

			base.Save(txOrNull);

			if (null != old)
				if (old.AppMainVersionId != AppMainVersionId)
				{
					//Record the release history
					var h = new CRelease();
					h.ReleaseAppId = AppId;
					h.ReleaseAppName = AppName;
					h.ReleaseVersionId = AppMainVersionId;
					if (null != MainVersion)
						h.ReleaseVersionName = MainVersion.VersionName;
					else
						h.ReleaseVersionName = "*none";
					h.Save(txOrNull);

					this.ReleasesMain.ExpireOldOnes();
					this.BinaryFiles.DeleteIfExpiredFor(AppKeepOldFilesForDays);
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
			this.Instances_(txOrNull).DeleteAll(txOrNull);  //Not files (4 other children)
			this.Versions_(txOrNull).DeleteAll(txOrNull);
			this.Groups_(txOrNull).DeleteAll(txOrNull);

			//Normal Delete 
			base.Delete(txOrNull);

			var orphans = new CBinaryFile().DeleteOrphans(txOrNull);
		}

		#endregion

		#region Custom Database Queries
		//(Not normally required for cached classes, use list class for searching etc)
		//For Stored Procs can use: MakeList (matching schema), or DataSrc.ExecuteDataset (reports etc)
		//For Dynamic sql, can use: SelectSum, SelectDistinct, SelectCount, SelectWhere (inherited methods)
		private int _reportsCount = int.MinValue;
		private int _upgradesCount = int.MinValue;
		private int _pushedCount = int.MinValue;
		public int ReportsCount_ { get { if (int.MinValue == _reportsCount) _reportsCount = this.ReportsCount(); return _reportsCount; } }
		public int UpgradesCount_ { get { if (int.MinValue == _upgradesCount) _upgradesCount = this.UpgradesCount(); return _upgradesCount; } }
		public int PushedCount_ { get { if (int.MinValue == _pushedCount) _pushedCount = this.PushedCount(); return _pushedCount; } }
		public int ReportsCount() { return new CReportHistory().SelectCountByAppId(AppId); }
		public int UpgradesCount() { return new CUpgradeHistory().SelectCountByAppId(AppId); }
		public int PushedCount() { return new CPushedUpgrade().SelectCountByAppId(AppId); }
		#endregion

		#region Searching (Optional)
		//For cached classes, custom seach logic resides in static methods on the list class
		// e.g. CApp.Cache.Search("...")

		//See also the auto-generated methods based on indexes
		//' e.g. CApp.Cache.GetBy...
		#endregion

		#region Caching Details
		//Cache Key
		internal static string CACHE_KEY = typeof(CApp).ToString();    //TABLE_NAME

		//Cache data
		private static CAppList LoadCache() { return new CApp().SelectAll(); }
		//Cache Timeout
		private static void SetCache(CAppList value)
		{
			if (null != value)
				value.Sort();
			CCache.Set(CACHE_KEY, value);    //Optional parameter can override timeout (otherwise uses config-settings, which default to 3hrs)
		}
		//Helper Method
		private CApp CacheGetById(CAppList list) { return list.GetById(this.AppId); }
		#endregion

		#region Cloning
		public CApp Clone(CDataSrc target, IDbTransaction txOrNull) //, int parentId)
		{
			//Shallow copy: Copies the immediate record, excluding autogenerated Pks
			CApp copy = new CApp(this, target);

			//Deep Copy - Child Entities: Cloned children must reference their cloned parent
			//copy.SampleParentId = parentId;

			copy.Save(txOrNull);

			//Deep Copy - Parent Entities: Cloned parents also clone their child collections
			//this.Children.Clone(target, txOrNull, copy.AppId);

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