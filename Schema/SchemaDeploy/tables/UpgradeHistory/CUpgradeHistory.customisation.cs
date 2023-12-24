using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
	//Table-Row Class (Customisable half)
	public partial class CUpgradeHistory
	{
		#region Constants
		//Join Expressions
		private static string JOIN_BINARYFILE = string.Concat(TABLE_NAME, " INNER JOIN ", CBinaryFile.TABLE_NAME, " ON ChangeNewSchemaMD5=MD5"); //Nullable => Use LEFT OUTER JOIN
		private static string JOIN_REPORT = string.Concat(TABLE_NAME, " INNER JOIN ", CReportHistory.TABLE_NAME, " ON ChangeReportId=ReportId"); //Nullable => Use LEFT OUTER JOIN
		private static string JOIN_INSTANCE = string.Concat(JOIN_REPORT, " INNER JOIN ", CInstance.TABLE_NAME, " ON ReportInstanceId=InstanceId"); //Nullable => Use LEFT OUTER JOIN

		#endregion

		#region Constructors (Public)
		//Default Connection String
		public CUpgradeHistory() : base() { }
		public CUpgradeHistory(int changeId) : base(changeId) { }

		//Alternative Connection String
		public CUpgradeHistory(CDataSrc dataSrc) : base(dataSrc) { }

		public CUpgradeHistory(CDataSrc dataSrc, int changeId) : base(dataSrc, changeId) { }

		//Transactional (shares an open connection)
		protected internal CUpgradeHistory(CDataSrc dataSrc, int changeId, IDbTransaction txOrNull) : base(dataSrc, changeId, txOrNull) { }
		#endregion

		#region Default Values
		protected override void InitValues_Custom()
		{
			_changeStarted = DateTime.Now;
		}
		#endregion

		#region Default Connection String
		protected override CDataSrc DefaultDataSrc() { return CDataSrc.Default; }
		#endregion

		#region Members
		//Foreign Keys 
		[NonSerialized()] private CReportHistory _reportHistory;
		[NonSerialized()] private CBinaryFile _binaryFile;

		//Child Collections  

		//Xml Data (as high-level objects)

		#endregion

		#region Properties - Relationships
		//Relationships - Foriegn Keys (e.g parent)
		public CInstance Instance { get { return CInstance.Cache.GetById(this.ReportInstanceId); } }

		public CVersion NewVersion { get { return CVersion.Cache.GetById(this.ChangeNewVersionId); } }
		public CBinaryFile NewSchema
		{
			get
			{
				if (_binaryFile == null)
				{
					lock (this)
					{
						if (_binaryFile == null)
							_binaryFile = new CBinaryFile(this.ChangeNewSchemaMD5);
					}
				}
				return _binaryFile;
			}
			set
			{
				_binaryFile = value;
				_changeNewSchemaMD5 = null != value ? value.MD5 : Guid.Empty;
			}
		}

		//Relationships - Collections (e.g. children)

		public CReportHistory ReportHistory
		{
			get
			{
				if (_reportHistory == null)
				{
					lock (this)
					{
						if (_reportHistory == null)
							_reportHistory = new CReportHistory(this.ChangeReportId);
					}
				}
				return _reportHistory;
			}
			set
			{
				_reportHistory = value;
				_changeReportId = null != value ? value.ReportId : int.MinValue;
			}
		}
		#endregion

		#region Properties - Customisation
		//Derived/ReadOnly (e.g. xml classes, presentation logic)
		#endregion

		#region Save/Delete Overrides
		//Can Override base.Save/Delete (e.g. Cascade deletes, or insert related records)
		public string ChangeNewSchemaB64 { get { return CBinary.ToBase64(ChangeNewSchemaMD5, 8); } }
		#endregion

		#region Custom Database Queries
		//For Stored Procs can use: MakeList (matching schema), or DataSrc.ExecuteDataset (reports etc)
		//For Dynamic sql, can use: SelectSum, SelectDistinct, SelectCount, SelectWhere (inherited methods)
		//                see also: SelectBy[FK], Search and Count (auto-generated sample queries)
		public CUpgradeHistoryList SelectByReportIds(List<int> reportIds) { return SelectWhere(new CCriteriaList("ChangeReportId", ESign.IN, reportIds)); }
		public CUpgradeHistoryList SelectByMD5s(List<Guid> mD5s) { return SelectWhere(new CCriteriaList("ChangeNewSchemaMD5", ESign.IN, mD5s)); }


		#region Queries - SelectBy[FK] (user-nominated fk/bool columns)
		public CUpgradeHistoryList SelectByInstanceId(int reportInstanceId) { return SelectWhere(new CCriteriaList("ReportInstanceId", reportInstanceId)); }
		public CUpgradeHistoryList SelectByInstanceId(CPagingInfo pi, int reportInstanceId) { return SelectWhere(pi, new CCriteriaList("ReportInstanceId", reportInstanceId)); }
		public int SelectCountByInstanceId(int reportInstanceId) { return SelectCount(new CCriteriaList("ReportInstanceId", reportInstanceId)); }
		public CUpgradeHistoryList SelectByInstanceId(int reportInstanceId, IDbTransaction tx) { return SelectWhere(new CCriteriaList("ReportInstanceId", reportInstanceId), tx); }

		public int SelectCountByAppId(int appId) { return SelectCount(new CCriteriaList("InstanceAppId", appId), JOIN_INSTANCE); }
		#endregion
		#endregion

		#region Searching (Optional)
		//Dynamic search methods: (overload as required for common search patterns, cascade the BuildWhere overloads)
		//   Public  x5 - Simple, Paged, Transactional, Count, and Dataset
		//   Private x1 - BuildWhere
		//See also in-memory search options in list class, such as GetBy[FK] and Search

		//Simple
		public CUpgradeHistoryList SelectSearch(string nameOrId, int appId, int instanceId) { return SelectWhere(BuildWhere(nameOrId, appId, instanceId)); } //, JOIN_OR_VIEW); }

		//Paged
		public CUpgradeHistoryList SelectSearch(CPagingInfo pi, string nameOrId, int appId, int instanceId)
		{
			//pi.TableName = JOIN_OR_VIEW
			return SelectWhere(pi, BuildWhere(nameOrId, appId, instanceId));
		}

		//Transactional
		public CUpgradeHistoryList SelectSearch(string nameOrId, int appId, int instanceId, IDbTransaction tx) { return SelectWhere(BuildWhere(nameOrId, appId, instanceId), tx); }   //, JOIN_OR_VIEW, tx); }

		//Dataset (e.g. ExportToCsv)
		public DataSet SelectSearch_Dataset(string nameOrId, int appId, int instanceId) { return SelectWhere_Dataset(BuildWhere(nameOrId, appId, instanceId)); }   //, JOIN_OR_VIEW); }

		//Count
		public int SelectCount(string nameOrId, int appId, int instanceId) { return SelectCount(BuildWhere(nameOrId, appId, instanceId)); }   //, JOIN_OR_VIEW); }

		//Filter Logic
		private CCriteriaList BuildWhere(string nameOrId, int appId, int instanceId)
		{
			CCriteriaList where = new CCriteriaList();  //Defaults to AND logic

			//Simple search box UI
			if (!string.IsNullOrEmpty(nameOrId))
			{
				//Interpret search string in various ways using OR sub-expression
				CCriteriaGroup orExpr = new CCriteriaGroup(EBoolOperator.Or);

				//Special case - search by PK
				/* 
                int id = 0;
                if (int.TryParse(nameOrId, out id))
                    orExpr.Add("ChangeId", id); 
                */

				//Search a range of string columns
				string wildCards = string.Concat("%", nameOrId, "%");

				//Conclude
				if (orExpr.Group.Count > 0)
					where.Add(orExpr);
			}

			//Other search Colums (customise as required)
			if (int.MinValue != instanceId) where.Add("ReportInstanceId", instanceId);
			//if (int.MinValue != reportId)   where.Add("ChangeReportId", reportId);
			//if (int.MinValue != newVersionId)   where.Add("ChangeNewVersionId", newVersionId);

			return where;
		}
		#endregion


		#region Cloning
		public CUpgradeHistory Clone(CDataSrc target, IDbTransaction txOrNull) //, int parentId)
		{
			//Shallow copy: Copies the immediate record, excluding autogenerated Pks
			CUpgradeHistory copy = new CUpgradeHistory(this, target);

			//Deep Copy - Child Entities: Cloned children must reference their cloned parent
			//copy.SampleParentId = parentId;

			copy.Save(txOrNull);

			//Deep Copy - Parent Entities: Cloned parents also clone their child collections
			//this.Children.Clone(target, txOrNull, copy.ChangeId);

			return copy;
		}
		#endregion

		#region ToXml
		protected override void ToXml_Custom(System.Xml.XmlWriter w)
		{
			//Store(w, "Example", this..Example)
		}
		#endregion
	}
}