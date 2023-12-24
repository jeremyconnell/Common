using System;
using System.Text;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
	//Table-Row Class (Customisable half)
	public partial class CReportHistory
	{
		#region Constants
		//Join Expressions        
		private static string JOIN_BINARYFILE = string.Concat(TABLE_NAME, " INNER JOIN ", CBinaryFile.TABLE_NAME, " ON ReportInitialSchemaMD5=MD5"); //Nullable => Use LEFT OUTER JOIN
		private static string JOIN_INSTANCE = string.Concat(TABLE_NAME, " INNER JOIN ", CInstance.TABLE_NAME, " ON ReportInstanceId=InstanceId"); //Nullable => Use LEFT OUTER JOIN
		private static string View(int appId) { return appId == int.MinValue ? TABLE_NAME : JOIN_INSTANCE; }
		#endregion

		#region Constructors (Public)
		//Default Connection String
		public CReportHistory() : base() { }
		public CReportHistory(int reportId) : base(reportId) { }

		//Alternative Connection String
		public CReportHistory(CDataSrc dataSrc) : base(dataSrc) { }

		public CReportHistory(CDataSrc dataSrc, int reportId) : base(dataSrc, reportId) { }

		//Transactional (shares an open connection)
		protected internal CReportHistory(CDataSrc dataSrc, int reportId, IDbTransaction txOrNull) : base(dataSrc, reportId, txOrNull) { }
		#endregion

		#region Default Values
		protected override void InitValues_Custom()
		{
			_reportAppStarted = DateTime.Now;
		}
		#endregion

		#region Default Connection String
		protected override CDataSrc DefaultDataSrc() { return CDataSrc.Default; }
		#endregion

		#region Members
		//Foreign Keys
		[NonSerialized()] private CBinaryFile _binaryFile;

		//Child Collections  

		//Xml Data (as high-level objects)

		#endregion

		#region Properties - Relationships
		//Relationships - Foriegn Keys (e.g parent)
		public CInstance Instance { get { return CInstance.Cache.GetById(this.ReportInstanceId); } }
		public CVersion InitialVersion { get { return CVersion.Cache.GetById(this.ReportInitialVersionId); } }

		public CBinaryFile BinaryFile
		{
			get
			{
				if (_binaryFile == null)
				{
					lock (this)
					{
						if (_binaryFile == null)
							_binaryFile = new CBinaryFile(this.ReportInitialSchemaMD5);
					}
				}
				return _binaryFile;
			}
			set
			{
				_binaryFile = value;
				_reportInitialSchemaMD5 = null != value ? value.MD5 : Guid.Empty;
			}
		}

		//Relationships - Collections (e.g. children)

		#endregion

		#region Properties - Customisation
		//Derived/ReadOnly (e.g. xml classes, presentation logic)
		public TimeSpan RanFor { get { return DateTime.MinValue == ReportAppStopped ? DateTime.Now.Subtract(ReportAppStarted) : ReportAppStopped.Subtract(ReportAppStarted); } }
		public string RanFor_ { get { return CUtilities.Timespan(RanFor); } }
		public string ReportInitialSchemaB64 { get { return CBinary.ToBase64(ReportInitialSchemaMD5, 8); } }
		#endregion

		#region Save/Delete Overrides
		//Can Override base.Save/Delete (e.g. Cascade deletes, or insert related records)
		#endregion

		#region Custom Database Queries
		//For Stored Procs can use: MakeList (matching schema), or DataSrc.ExecuteDataset (reports etc)
		//For Dynamic sql, can use: SelectSum, SelectDistinct, SelectCount, SelectWhere (inherited methods)
		//                see also: SelectBy[FK], Search and Count (auto-generated sample queries)
		public CReportHistoryList SelectByMD5s(List<Guid> mD5s) { return SelectWhere(new CCriteriaList("ReportInitialSchemaMD5", ESign.IN, mD5s)); }

		public int SelectCountByAppId(int appId) { return SelectCount(new CCriteriaList("InstanceAppId", appId), JOIN_INSTANCE); }
		#endregion

		#region Searching (Optional)
		//Dynamic search methods: (overload as required for common search patterns, cascade the BuildWhere overloads)
		//   Public  x5 - Simple, Paged, Transactional, Count, and Dataset
		//   Private x1 - BuildWhere
		//See also in-memory search options in list class, such as GetBy[FK] and Search

		//Simple
		public CReportHistoryList SelectSearch(string nameOrId, int appId, int instanceId, int initialVersionId = int.MinValue) { return SelectWhere(BuildWhere(nameOrId, appId, instanceId, initialVersionId), View(appId)); }

		//Paged
		public CReportHistoryList SelectSearch(CPagingInfo pi, string nameOrId, int appId, int instanceId, int initialVersionId = int.MinValue)
		{
			pi.TableName = View(appId);
			return SelectWhere(pi, BuildWhere(nameOrId, appId, instanceId, initialVersionId));
		}

		//Transactional
		public CReportHistoryList SelectSearch(string nameOrId, int appId, int instanceId, int initialVersionId, IDbTransaction tx) { return SelectWhere(BuildWhere(nameOrId, appId, instanceId, initialVersionId), View(appId), tx); }

		//Dataset (e.g. ExportToCsv)
		public DataSet SelectSearch_Dataset(string nameOrId, int appId, int instanceId, int initialVersionId = int.MinValue) { return SelectWhere_Dataset(BuildWhere(nameOrId, appId, instanceId, initialVersionId), View(appId)); }
		//Count
		public int SelectCount(string nameOrId, int appId, int instanceId, int initialVersionId = int.MinValue) { return SelectCount(BuildWhere(nameOrId, appId, instanceId, initialVersionId), View(appId)); }

		//Filter Logic
		private CCriteriaList BuildWhere(string nameOrId, int appId, int instanceId, int initialVersionId)
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
                    orExpr.Add("ReportId", id); 
                */

				//Search a range of string columns
				string wildCards = string.Concat("%", nameOrId, "%");

				//Conclude
				if (orExpr.Group.Count > 0)
					where.Add(orExpr);
			}

			//Other search Colums (customise as required)
			if (int.MinValue != instanceId)
				where.Add("ReportInstanceId", instanceId);
			else if (int.MinValue != appId)
				where.Add("InstanceAppId", appId);

			if (int.MinValue != initialVersionId)
				where.Add("ReportInitialVersionId", initialVersionId);

			return where;
		}
		#endregion


		#region Cloning
		public CReportHistory Clone(CDataSrc target, IDbTransaction txOrNull) //, int parentId)
		{
			//Shallow copy: Copies the immediate record, excluding autogenerated Pks
			CReportHistory copy = new CReportHistory(this, target);

			//Deep Copy - Child Entities: Cloned children must reference their cloned parent
			//copy.SampleParentId = parentId;

			copy.Save(txOrNull);

			//Deep Copy - Parent Entities: Cloned parents also clone their child collections
			//this.Children.Clone(target, txOrNull, copy.ReportId);

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