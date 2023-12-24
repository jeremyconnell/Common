using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
    [Serializable()]
    public partial class CUpgradeHistory : CBaseDynamic, IComparable<CUpgradeHistory>
    {
        #region Constructors
        //Public (Copy Constructor)
        public CUpgradeHistory(CUpgradeHistory original, CDataSrc target)
        {
            //Database Connection
            m_dataSrc = target;

            //Copy fields
            _reportId = original.ReportId;
            _reportInstanceId = original.ReportInstanceId;
            _reportInitialVersionId = original.ReportInitialVersionId;
            _reportInitialSchemaMD5 = original.ReportInitialSchemaMD5;
            _reportAppStarted = original.ReportAppStarted;
            _reportAppStopped = original.ReportAppStopped;
            _changeReportId = original.ChangeReportId;
            _changeNewVersionId = original.ChangeNewVersionId;
            _changeNewSchemaMD5 = original.ChangeNewSchemaMD5;
            _changeStarted = original.ChangeStarted;
            _changeFinished = original.ChangeFinished;
        }

        //Protected (Datareader/Dataset)
        protected CUpgradeHistory(CDataSrc dataSrc, IDataReader dr) : base(dataSrc, dr) {}
        protected CUpgradeHistory(CDataSrc dataSrc, DataRow     dr) : base(dataSrc, dr) {}
        #endregion

        #region Default Values
        protected override void InitValues_Auto()
        {
            //Null-Equivalent values (except String.Empty, as nulls tend to be inconvenient for string types)
            _changeId = int.MinValue;
            _changeReportId = int.MinValue;
            _changeNewVersionId = int.MinValue;
            _changeNewSchemaMD5 = Guid.Empty;
            _changeStarted = DateTime.MinValue;
            _changeFinished = DateTime.MinValue;
        }
        #endregion

        #region Members
        protected int _reportId;
        protected int _reportInstanceId;
        protected int _reportInitialVersionId;
        protected Guid _reportInitialSchemaMD5;
        protected DateTime _reportAppStarted;
        protected DateTime _reportAppStopped;
        protected int _changeId;
        protected int _changeReportId;
        protected int _changeNewVersionId;
        protected Guid _changeNewSchemaMD5;
        protected DateTime _changeStarted;
        protected DateTime _changeFinished;
        #endregion

        #region Properties - Column Values
        //Primary Key Column (ReadOnly)
        public int ChangeId    {   get { return _changeId;   } }

        //Table Columns (Read/Write)
        public int ChangeReportId    {   get { return _changeReportId; } set { _changeReportId = value; }    }
        public int ChangeNewVersionId    {   get { return _changeNewVersionId; } set { _changeNewVersionId = value; }    }
        public Guid ChangeNewSchemaMD5    {   get { return _changeNewSchemaMD5; } set { _changeNewSchemaMD5 = value; }    }
        public DateTime ChangeStarted    {   get { return _changeStarted; } set { _changeStarted = value; }    }
        public DateTime ChangeFinished    {   get { return _changeFinished; } set { _changeFinished = value; }    }

        //View Columns (ReadOnly)
        public int ReportId    {   get { return _reportId;   } }
        public int ReportInstanceId    {   get { return _reportInstanceId;   } }
        public int ReportInitialVersionId    {   get { return _reportInitialVersionId;   } }
        public Guid ReportInitialSchemaMD5    {   get { return _reportInitialSchemaMD5;   } }
        public DateTime ReportAppStarted    {   get { return _reportAppStarted;   } }
        public DateTime ReportAppStopped    {   get { return _reportAppStopped;   } }

        #endregion

        #region MustOverride Methods
        //Schema Information
        public const string TABLE_NAME      = "Deploy_UpgradeHistory";
        public const string VIEW_NAME       = "vwDeploy_UpgradeHistory";         //Used to override this.ViewName { get }
        public const string ORDER_BY_COLS   = "ChangeId DESC";   //See the CompareTo method below (Sql-based sorting should match In-Memory sorting)
        public const string SORTING_COLUMN  = "";
        public override string TableName {    get { return TABLE_NAME; } }
        protected override string ViewName {    get {   return VIEW_NAME;    }    }
        protected override string OrderByColumns {    get { return ORDER_BY_COLS; } }

        //CompareTo Interface (Default Sort Order)
        public int CompareTo(CUpgradeHistory other) {   return this.ChangeId.CompareTo(other.ChangeId) *-1;  }

        //Primary Key Information
        public const string PRIMARY_KEY_NAME = "ChangeId";
        protected override bool InsertPrimaryKey {  get { return false;    }   }
        protected override string PrimaryKeyName {  get { return PRIMARY_KEY_NAME;    }    }
        protected override object PrimaryKeyValue 
        {
              get { return _changeId; }
              set { _changeId = (int)value; }
        }

        //Factory Methods - Object
        protected override CBase MakeFrom(DataRow row)      {   return new CUpgradeHistory(this.DataSrc, row);  }
        protected override CBase MakeFrom(IDataReader dr)   {   return new CUpgradeHistory(this.DataSrc, dr);   }

        //Factory Methods - List
        protected override IList MakeList()                 {   return new CUpgradeHistoryList();               }
        protected override IList MakeList(int capacity)     {   return new CUpgradeHistoryList(capacity);       }

        //Convert from ADO to .Net
        protected override void ReadColumns(IDataReader dr)
        {
            _reportId = CAdoData.GetInt(dr, "ReportId");
            _reportInstanceId = CAdoData.GetInt(dr, "ReportInstanceId");
            _reportInitialVersionId = CAdoData.GetInt(dr, "ReportInitialVersionId");
            _reportInitialSchemaMD5 = CAdoData.GetGuid(dr, "ReportInitialSchemaMD5");
            _reportAppStarted = CAdoData.GetDate(dr, "ReportAppStarted");
            _reportAppStopped = CAdoData.GetDate(dr, "ReportAppStopped");
            _changeId = CAdoData.GetInt(dr, "ChangeId");
            _changeReportId = CAdoData.GetInt(dr, "ChangeReportId");
            _changeNewVersionId = CAdoData.GetInt(dr, "ChangeNewVersionId");
            _changeNewSchemaMD5 = CAdoData.GetGuid(dr, "ChangeNewSchemaMD5");
            _changeStarted = CAdoData.GetDate(dr, "ChangeStarted");
            _changeFinished = CAdoData.GetDate(dr, "ChangeFinished");
        }
        protected override void ReadColumns(DataRow dr)
        {
            _reportId = CAdoData.GetInt(dr, "ReportId");
            _reportInstanceId = CAdoData.GetInt(dr, "ReportInstanceId");
            _reportInitialVersionId = CAdoData.GetInt(dr, "ReportInitialVersionId");
            _reportInitialSchemaMD5 = CAdoData.GetGuid(dr, "ReportInitialSchemaMD5");
            _reportAppStarted = CAdoData.GetDate(dr, "ReportAppStarted");
            _reportAppStopped = CAdoData.GetDate(dr, "ReportAppStopped");
            _changeId = CAdoData.GetInt(dr, "ChangeId");
            _changeReportId = CAdoData.GetInt(dr, "ChangeReportId");
            _changeNewVersionId = CAdoData.GetInt(dr, "ChangeNewVersionId");
            _changeNewSchemaMD5 = CAdoData.GetGuid(dr, "ChangeNewSchemaMD5");
            _changeStarted = CAdoData.GetDate(dr, "ChangeStarted");
            _changeFinished = CAdoData.GetDate(dr, "ChangeFinished");
        }

        //Parameters for Insert/Update    
        protected override CNameValueList ColumnNameValues()
        {
            CNameValueList data = new CNameValueList();
            data.Add("ChangeId", CAdoData.NullVal(_changeId));
            data.Add("ChangeReportId", CAdoData.NullVal(_changeReportId));
            data.Add("ChangeNewVersionId", CAdoData.NullVal(_changeNewVersionId));
            data.Add("ChangeNewSchemaMD5", CAdoData.NullVal(_changeNewSchemaMD5));
            data.Add("ChangeStarted", CAdoData.NullVal(_changeStarted));
            data.Add("ChangeFinished", CAdoData.NullVal(_changeFinished));
            return data;
        }
        #endregion

        #region Queries - SelectAll/SelectWhere (inherited methods, cast only)
        //Select Queries - Non-Paged
        public new CUpgradeHistoryList SelectAll()                                                          {    return (CUpgradeHistoryList)base.SelectAll();                              }
        public new CUpgradeHistoryList SelectAll(string orderBy)                                            {    return (CUpgradeHistoryList)base.SelectAll(orderBy);                       }
        public new CUpgradeHistoryList SelectWhere(string colName, ESign sign, object colValue)             {    return (CUpgradeHistoryList)base.SelectWhere(colName, sign, colValue);     }
        public new CUpgradeHistoryList SelectWhere(CCriteria where)                                         {    return (CUpgradeHistoryList)base.SelectWhere(where);                       }
        public new CUpgradeHistoryList SelectWhere(CCriteriaList where)                                     {    return (CUpgradeHistoryList)base.SelectWhere(where);                       }
        public new CUpgradeHistoryList SelectWhere(CCriteriaList where, string tableOrJoin)                 {    return (CUpgradeHistoryList)base.SelectWhere(where, tableOrJoin);          }
        public new CUpgradeHistoryList SelectWhere(CCriteriaList where, string tableOrJoin, string orderBy) {    return (CUpgradeHistoryList)base.SelectWhere(where, tableOrJoin, orderBy); }
        [Obsolete("Arbitrary where-clause is not necessary portable or safe from sql injection attacks. Consider using the parameterised query interfaces")]
        public new CUpgradeHistoryList SelectWhere(string unsafeWhereClause)                                {    return (CUpgradeHistoryList)base.SelectWhere(unsafeWhereClause);           }
        public     CUpgradeHistoryList SelectById(int changeId)                              {    return (CUpgradeHistoryList)base.SelectById(changeId);       }
        public     CUpgradeHistoryList SelectByIds(List<int> ids)                        {    return (CUpgradeHistoryList)base.SelectByIds(ids);                         }

        //Select Queries - Paged
        public new CUpgradeHistoryList SelectAll(  CPagingInfo pi)                                              {    return (CUpgradeHistoryList)base.SelectAll(  pi);                              }
        public new CUpgradeHistoryList SelectWhere(CPagingInfo pi, string name, ESign sign, object value)       {    return (CUpgradeHistoryList)base.SelectWhere(pi, name, sign, value);           }
        public new CUpgradeHistoryList SelectWhere(CPagingInfo pi, CCriteria criteria)                          {    return (CUpgradeHistoryList)base.SelectWhere(pi, criteria);                    }
        public new CUpgradeHistoryList SelectWhere(CPagingInfo pi, CCriteriaList criteria)                      {    return (CUpgradeHistoryList)base.SelectWhere(pi, criteria);                    }
        public new CUpgradeHistoryList SelectWhere(CPagingInfo pi, CCriteriaList criteria, string viewOrJoin)   {    return (CUpgradeHistoryList)base.SelectWhere(pi, criteria, viewOrJoin);        }
        public     CUpgradeHistoryList SelectByIds(CPagingInfo pi, List<int> ids)            {    return (CUpgradeHistoryList)base.SelectByIds(pi, ids);                         }

        //Select Queries - Transactional
        public new CUpgradeHistoryList SelectAll(                                                                                           IDbTransaction tx)  {    return (CUpgradeHistoryList)base.SelectAll(tx);                                                     }
        public new CUpgradeHistoryList SelectAll(string orderBy,                                                                            IDbTransaction tx)  {    return (CUpgradeHistoryList)base.SelectAll(orderBy,                                         tx);    }
        public new CUpgradeHistoryList SelectWhere(string columnName,               object columnValue,                                     IDbTransaction tx)  {    return (CUpgradeHistoryList)base.SelectWhere(columnName,              columnValue,          tx);    }
        public new CUpgradeHistoryList SelectWhere(string columnName,   ESign sign, object columnValue,                                     IDbTransaction tx)  {    return (CUpgradeHistoryList)base.SelectWhere(columnName,    sign,     columnValue,          tx);    }
        public new CUpgradeHistoryList SelectWhere(                             CCriteria     criteria,                                     IDbTransaction tx)  {    return (CUpgradeHistoryList)base.SelectWhere(               criteria,                       tx);    }
        public new CUpgradeHistoryList SelectWhere(                             CCriteriaList criteria,                                     IDbTransaction tx)  {    return (CUpgradeHistoryList)base.SelectWhere(               criteria,                       tx);    }
        public new CUpgradeHistoryList SelectWhere(                             CCriteriaList criteria, string tableOrJoin,                 IDbTransaction tx)  {    return (CUpgradeHistoryList)base.SelectWhere(               criteria, tableOrJoin,          tx);    }
        public new CUpgradeHistoryList SelectWhere(                             CCriteriaList criteria, string tableOrJoin, string orderBy, IDbTransaction tx)  {    return (CUpgradeHistoryList)base.SelectWhere(               criteria, tableOrJoin,          tx);    }
        public     CUpgradeHistoryList SelectById(int changeId,                                                              IDbTransaction tx)  {    return (CUpgradeHistoryList)base.SelectById(changeId,                         tx);    }
        public     CUpgradeHistoryList SelectByIds(List<int> ids,                                                        IDbTransaction tx)  {    return (CUpgradeHistoryList)base.SelectByIds(ids,                                           tx);    }

        //Select Queries - Stored Procs
        public new CUpgradeHistoryList MakeList(string storedProcName,                             IDbTransaction txOrNull)    {   return (CUpgradeHistoryList)base.MakeList(storedProcName,             txOrNull);  }
        public new CUpgradeHistoryList MakeList(string storedProcName, object[] parameters,        IDbTransaction txOrNull)    {   return (CUpgradeHistoryList)base.MakeList(storedProcName, parameters, txOrNull);  }
        public new CUpgradeHistoryList MakeList(string storedProcName, CNameValueList parameters,  IDbTransaction txOrNull)    {   return (CUpgradeHistoryList)base.MakeList(storedProcName, parameters, txOrNull);  }
        public new CUpgradeHistoryList MakeList(string storedProcName, List<object> parameters,    IDbTransaction txOrNull)    {   return (CUpgradeHistoryList)base.MakeList(storedProcName, parameters, txOrNull);  }
        public new CUpgradeHistoryList MakeList(string storedProcName, int param1,                 IDbTransaction txOrNull)    {   return (CUpgradeHistoryList)base.MakeList(storedProcName, param1,     txOrNull);  }
        public new CUpgradeHistoryList MakeList(string storedProcName, string param1,              IDbTransaction txOrNull)    {   return (CUpgradeHistoryList)base.MakeList(storedProcName, param1,     txOrNull);  }

        //Query Results
        protected new CUpgradeHistoryList MakeList(DataSet              ds) { return (CUpgradeHistoryList)base.MakeList(ds);        }
        protected new CUpgradeHistoryList MakeList(DataTable            dt) { return (CUpgradeHistoryList)base.MakeList(dt);        }
        protected new CUpgradeHistoryList MakeList(DataRowCollection  rows) { return (CUpgradeHistoryList)base.MakeList(rows);      }
        protected new CUpgradeHistoryList MakeList(IDataReader          dr) { return (CUpgradeHistoryList)base.MakeList(dr);        }
        protected new CUpgradeHistoryList MakeList(object           drOrDs) { return (CUpgradeHistoryList)base.MakeList(drOrDs);    }        
        protected new CUpgradeHistoryList MakeList(byte[]             gzip) { return (CUpgradeHistoryList)base.MakeList(gzip);      }
        #endregion

        #region Queries - SelectBy[FK] (user-nominated fk/bool columns)
        //Non-Paged
        public CUpgradeHistoryList SelectByReportId(int changeReportId)    {    return SelectWhere(new CCriteriaList("ChangeReportId", changeReportId));    }
        public CUpgradeHistoryList SelectByNewVersionId(int changeNewVersionId)    {    return SelectWhere(new CCriteriaList("ChangeNewVersionId", changeNewVersionId));    }

        //Paged
        public CUpgradeHistoryList SelectByReportId(CPagingInfo pi, int changeReportId)    {    return SelectWhere(pi, new CCriteriaList("ChangeReportId", changeReportId));    }
        public CUpgradeHistoryList SelectByNewVersionId(CPagingInfo pi, int changeNewVersionId)    {    return SelectWhere(pi, new CCriteriaList("ChangeNewVersionId", changeNewVersionId));    }

        //Count
        public int SelectCountByReportId(int changeReportId)   {   return SelectCount(new CCriteriaList("ChangeReportId", changeReportId));     }
        public int SelectCountByNewVersionId(int changeNewVersionId)   {   return SelectCount(new CCriteriaList("ChangeNewVersionId", changeNewVersionId));     }

        //Transactional
        public CUpgradeHistoryList SelectByReportId(int changeReportId, IDbTransaction tx)    {    return SelectWhere(new CCriteriaList("ChangeReportId", changeReportId), tx);    }
        public CUpgradeHistoryList SelectByNewVersionId(int changeNewVersionId, IDbTransaction tx)    {    return SelectWhere(new CCriteriaList("ChangeNewVersionId", changeNewVersionId), tx);    }
        #endregion

        #region ToXml
        protected override void ToXml_Autogenerated(System.Xml.XmlWriter w)
        {
            Store(w, "ChangeId", this.ChangeId);
            Store(w, "ChangeReportId", this.ChangeReportId);
            Store(w, "ChangeNewVersionId", this.ChangeNewVersionId);
            Store(w, "ChangeNewSchemaMD5", this.ChangeNewSchemaMD5);
            Store(w, "ChangeStarted", this.ChangeStarted);
            Store(w, "ChangeFinished", this.ChangeFinished);
        }
        #endregion
    }
}
