using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;

using Framework;

namespace SchemaDeploy
{
    [Serializable()]
    public partial class CReportHistory : CBaseDynamic, IComparable<CReportHistory>
    {
        #region Constructors
        //Public (Copy Constructor)
        public CReportHistory(CReportHistory original, CDataSrc target)
        {
            //Database Connection
            m_dataSrc = target;

            //Copy fields
            _reportInstanceId = original.ReportInstanceId;
            _reportInitialVersionId = original.ReportInitialVersionId;
            _reportInitialSchemaMD5 = original.ReportInitialSchemaMD5;
            _reportAppStarted = original.ReportAppStarted;
            _reportAppStopped = original.ReportAppStopped;
        }

        //Protected (Datareader/Dataset)
        protected CReportHistory(CDataSrc dataSrc, IDataReader dr) : base(dataSrc, dr) {}
        protected CReportHistory(CDataSrc dataSrc, DataRow     dr) : base(dataSrc, dr) {}
        #endregion

        #region Default Values
        protected override void InitValues_Auto()
        {
            //Null-Equivalent values (except String.Empty, as nulls tend to be inconvenient for string types)
            _reportId = int.MinValue;
            _reportInstanceId = int.MinValue;
            _reportInitialVersionId = int.MinValue;
            _reportInitialSchemaMD5 = Guid.Empty;
            _reportAppStarted = DateTime.MinValue;
            _reportAppStopped = DateTime.MinValue;
        }
        #endregion

        #region Members
        protected int _reportId;
        protected int _reportInstanceId;
        protected int _reportInitialVersionId;
        protected Guid _reportInitialSchemaMD5;
        protected DateTime _reportAppStarted;
        protected DateTime _reportAppStopped;
        #endregion

        #region Properties - Column Values
        //Primary Key Column (ReadOnly)
        public int ReportId    {   get { return _reportId;   } }

        //Table Columns (Read/Write)
        public int ReportInstanceId    {   get { return _reportInstanceId; } set { _reportInstanceId = value; }    }
        public int ReportInitialVersionId    {   get { return _reportInitialVersionId; } set { _reportInitialVersionId = value; }    }
        public Guid ReportInitialSchemaMD5    {   get { return _reportInitialSchemaMD5; } set { _reportInitialSchemaMD5 = value; }    }
        public DateTime ReportAppStarted    {   get { return _reportAppStarted; } set { _reportAppStarted = value; }    }
        public DateTime ReportAppStopped    {   get { return _reportAppStopped; } set { _reportAppStopped = value; }    }

        //View Columns (ReadOnly)

        #endregion

        #region MustOverride Methods
        //Schema Information
        public const string TABLE_NAME      = "Deploy_ReportHistory";
        public const string VIEW_NAME       = "";         //Used to override this.ViewName { get }
        public const string ORDER_BY_COLS   = "ReportId DESC";   //See the CompareTo method below (Sql-based sorting should match In-Memory sorting)
        public const string SORTING_COLUMN  = "";
        public override string TableName {    get { return TABLE_NAME; } }
        protected override string OrderByColumns {    get { return ORDER_BY_COLS; } }

        //CompareTo Interface (Default Sort Order)
        public int CompareTo(CReportHistory other) {   return this.ReportId.CompareTo(other.ReportId) *-1;  }

        //Primary Key Information
        public const string PRIMARY_KEY_NAME = "ReportId";
        protected override bool InsertPrimaryKey {  get { return false;    }   }
        protected override string PrimaryKeyName {  get { return PRIMARY_KEY_NAME;    }    }
        protected override object PrimaryKeyValue 
        {
              get { return _reportId; }
              set { _reportId = (int)value; }
        }

        //Factory Methods - Object
        protected override CBase MakeFrom(DataRow row)      {   return new CReportHistory(this.DataSrc, row);  }
        protected override CBase MakeFrom(IDataReader dr)   {   return new CReportHistory(this.DataSrc, dr);   }

        //Factory Methods - List
        protected override IList MakeList()                 {   return new CReportHistoryList();               }
        protected override IList MakeList(int capacity)     {   return new CReportHistoryList(capacity);       }

        //Convert from ADO to .Net
        protected override void ReadColumns(IDataReader dr)
        {
            _reportId = CAdoData.GetInt(dr, "ReportId");
            _reportInstanceId = CAdoData.GetInt(dr, "ReportInstanceId");
            _reportInitialVersionId = CAdoData.GetInt(dr, "ReportInitialVersionId");
            _reportInitialSchemaMD5 = CAdoData.GetGuid(dr, "ReportInitialSchemaMD5");
            _reportAppStarted = CAdoData.GetDate(dr, "ReportAppStarted");
            _reportAppStopped = CAdoData.GetDate(dr, "ReportAppStopped");
        }
        protected override void ReadColumns(DataRow dr)
        {
            _reportId = CAdoData.GetInt(dr, "ReportId");
            _reportInstanceId = CAdoData.GetInt(dr, "ReportInstanceId");
            _reportInitialVersionId = CAdoData.GetInt(dr, "ReportInitialVersionId");
            _reportInitialSchemaMD5 = CAdoData.GetGuid(dr, "ReportInitialSchemaMD5");
            _reportAppStarted = CAdoData.GetDate(dr, "ReportAppStarted");
            _reportAppStopped = CAdoData.GetDate(dr, "ReportAppStopped");
        }

        //Parameters for Insert/Update    
        protected override CNameValueList ColumnNameValues()
        {
            CNameValueList data = new CNameValueList();
            data.Add("ReportId", CAdoData.NullVal(_reportId));
            data.Add("ReportInstanceId", CAdoData.NullVal(_reportInstanceId));
            data.Add("ReportInitialVersionId", CAdoData.NullVal(_reportInitialVersionId));
            data.Add("ReportInitialSchemaMD5", CAdoData.NullVal(_reportInitialSchemaMD5));
            data.Add("ReportAppStarted", CAdoData.NullVal(_reportAppStarted));
            data.Add("ReportAppStopped", CAdoData.NullVal(_reportAppStopped));
            return data;
        }
        #endregion

        #region Queries - SelectAll/SelectWhere (inherited methods, cast only)
        //Select Queries - Non-Paged
        public new CReportHistoryList SelectAll()                                                          {    return (CReportHistoryList)base.SelectAll();                              }
        public new CReportHistoryList SelectAll(string orderBy)                                            {    return (CReportHistoryList)base.SelectAll(orderBy);                       }
        public new CReportHistoryList SelectWhere(string colName, ESign sign, object colValue)             {    return (CReportHistoryList)base.SelectWhere(colName, sign, colValue);     }
        public new CReportHistoryList SelectWhere(CCriteria where)                                         {    return (CReportHistoryList)base.SelectWhere(where);                       }
        public new CReportHistoryList SelectWhere(CCriteriaList where)                                     {    return (CReportHistoryList)base.SelectWhere(where);                       }
        public new CReportHistoryList SelectWhere(CCriteriaList where, string tableOrJoin)                 {    return (CReportHistoryList)base.SelectWhere(where, tableOrJoin);          }
        public new CReportHistoryList SelectWhere(CCriteriaList where, string tableOrJoin, string orderBy) {    return (CReportHistoryList)base.SelectWhere(where, tableOrJoin, orderBy); }
        [Obsolete("Arbitrary where-clause is not necessary portable or safe from sql injection attacks. Consider using the parameterised query interfaces")]
        public new CReportHistoryList SelectWhere(string unsafeWhereClause)                                {    return (CReportHistoryList)base.SelectWhere(unsafeWhereClause);           }
        public     CReportHistoryList SelectById(int reportId)                              {    return (CReportHistoryList)base.SelectById(reportId);       }
        public     CReportHistoryList SelectByIds(List<int> ids)                        {    return (CReportHistoryList)base.SelectByIds(ids);                         }

        //Select Queries - Paged
        public new CReportHistoryList SelectAll(  CPagingInfo pi)                                              {    return (CReportHistoryList)base.SelectAll(  pi);                              }
        public new CReportHistoryList SelectWhere(CPagingInfo pi, string name, ESign sign, object value)       {    return (CReportHistoryList)base.SelectWhere(pi, name, sign, value);           }
        public new CReportHistoryList SelectWhere(CPagingInfo pi, CCriteria criteria)                          {    return (CReportHistoryList)base.SelectWhere(pi, criteria);                    }
        public new CReportHistoryList SelectWhere(CPagingInfo pi, CCriteriaList criteria)                      {    return (CReportHistoryList)base.SelectWhere(pi, criteria);                    }
        public new CReportHistoryList SelectWhere(CPagingInfo pi, CCriteriaList criteria, string viewOrJoin)   {    return (CReportHistoryList)base.SelectWhere(pi, criteria, viewOrJoin);        }
        public     CReportHistoryList SelectByIds(CPagingInfo pi, List<int> ids)            {    return (CReportHistoryList)base.SelectByIds(pi, ids);                         }

        //Select Queries - Transactional
        public new CReportHistoryList SelectAll(                                                                                           IDbTransaction tx)  {    return (CReportHistoryList)base.SelectAll(tx);                                                     }
        public new CReportHistoryList SelectAll(string orderBy,                                                                            IDbTransaction tx)  {    return (CReportHistoryList)base.SelectAll(orderBy,                                         tx);    }
        public new CReportHistoryList SelectWhere(string columnName,               object columnValue,                                     IDbTransaction tx)  {    return (CReportHistoryList)base.SelectWhere(columnName,              columnValue,          tx);    }
        public new CReportHistoryList SelectWhere(string columnName,   ESign sign, object columnValue,                                     IDbTransaction tx)  {    return (CReportHistoryList)base.SelectWhere(columnName,    sign,     columnValue,          tx);    }
        public new CReportHistoryList SelectWhere(                             CCriteria     criteria,                                     IDbTransaction tx)  {    return (CReportHistoryList)base.SelectWhere(               criteria,                       tx);    }
        public new CReportHistoryList SelectWhere(                             CCriteriaList criteria,                                     IDbTransaction tx)  {    return (CReportHistoryList)base.SelectWhere(               criteria,                       tx);    }
        public new CReportHistoryList SelectWhere(                             CCriteriaList criteria, string tableOrJoin,                 IDbTransaction tx)  {    return (CReportHistoryList)base.SelectWhere(               criteria, tableOrJoin,          tx);    }
        public new CReportHistoryList SelectWhere(                             CCriteriaList criteria, string tableOrJoin, string orderBy, IDbTransaction tx)  {    return (CReportHistoryList)base.SelectWhere(               criteria, tableOrJoin,          tx);    }
        public     CReportHistoryList SelectById(int reportId,                                                              IDbTransaction tx)  {    return (CReportHistoryList)base.SelectById(reportId,                         tx);    }
        public     CReportHistoryList SelectByIds(List<int> ids,                                                        IDbTransaction tx)  {    return (CReportHistoryList)base.SelectByIds(ids,                                           tx);    }

        //Select Queries - Stored Procs
        public new CReportHistoryList MakeList(string storedProcName,                             IDbTransaction txOrNull)    {   return (CReportHistoryList)base.MakeList(storedProcName,             txOrNull);  }
        public new CReportHistoryList MakeList(string storedProcName, object[] parameters,        IDbTransaction txOrNull)    {   return (CReportHistoryList)base.MakeList(storedProcName, parameters, txOrNull);  }
        public new CReportHistoryList MakeList(string storedProcName, CNameValueList parameters,  IDbTransaction txOrNull)    {   return (CReportHistoryList)base.MakeList(storedProcName, parameters, txOrNull);  }
        public new CReportHistoryList MakeList(string storedProcName, List<object> parameters,    IDbTransaction txOrNull)    {   return (CReportHistoryList)base.MakeList(storedProcName, parameters, txOrNull);  }
        public new CReportHistoryList MakeList(string storedProcName, int param1,                 IDbTransaction txOrNull)    {   return (CReportHistoryList)base.MakeList(storedProcName, param1,     txOrNull);  }
        public new CReportHistoryList MakeList(string storedProcName, string param1,              IDbTransaction txOrNull)    {   return (CReportHistoryList)base.MakeList(storedProcName, param1,     txOrNull);  }

        //Query Results
        protected new CReportHistoryList MakeList(DataSet              ds) { return (CReportHistoryList)base.MakeList(ds);        }
        protected new CReportHistoryList MakeList(DataTable            dt) { return (CReportHistoryList)base.MakeList(dt);        }
        protected new CReportHistoryList MakeList(DataRowCollection  rows) { return (CReportHistoryList)base.MakeList(rows);      }
        protected new CReportHistoryList MakeList(IDataReader          dr) { return (CReportHistoryList)base.MakeList(dr);        }
        protected new CReportHistoryList MakeList(object           drOrDs) { return (CReportHistoryList)base.MakeList(drOrDs);    }        
        protected new CReportHistoryList MakeList(byte[]             gzip) { return (CReportHistoryList)base.MakeList(gzip);      }
        #endregion

        #region Queries - SelectBy[FK] (user-nominated fk/bool columns)
        //Non-Paged
        public CReportHistoryList SelectByInstanceId(int reportInstanceId)    {    return SelectWhere(new CCriteriaList("ReportInstanceId", reportInstanceId));    }
        public CReportHistoryList SelectByInitialVersionId(int reportInitialVersionId)    {    return SelectWhere(new CCriteriaList("ReportInitialVersionId", reportInitialVersionId));    }

        //Paged
        public CReportHistoryList SelectByInstanceId(CPagingInfo pi, int reportInstanceId)    {    return SelectWhere(pi, new CCriteriaList("ReportInstanceId", reportInstanceId));    }
        public CReportHistoryList SelectByInitialVersionId(CPagingInfo pi, int reportInitialVersionId)    {    return SelectWhere(pi, new CCriteriaList("ReportInitialVersionId", reportInitialVersionId));    }

        //Count
        public int SelectCountByInstanceId(int reportInstanceId)   {   return SelectCount(new CCriteriaList("ReportInstanceId", reportInstanceId));     }
        public int SelectCountByInitialVersionId(int reportInitialVersionId)   {   return SelectCount(new CCriteriaList("ReportInitialVersionId", reportInitialVersionId));     }

        //Transactional
        public CReportHistoryList SelectByInstanceId(int reportInstanceId, IDbTransaction tx)    {    return SelectWhere(new CCriteriaList("ReportInstanceId", reportInstanceId), tx);    }
        public CReportHistoryList SelectByInitialVersionId(int reportInitialVersionId, IDbTransaction tx)    {    return SelectWhere(new CCriteriaList("ReportInitialVersionId", reportInitialVersionId), tx);    }
        #endregion

        #region ToXml
        protected override void ToXml_Autogenerated(System.Xml.XmlWriter w)
        {
            Store(w, "ReportId", this.ReportId);
            Store(w, "ReportInstanceId", this.ReportInstanceId);
            Store(w, "ReportInitialVersionId", this.ReportInitialVersionId);
            Store(w, "ReportInitialSchemaMD5", this.ReportInitialSchemaMD5);
            Store(w, "ReportAppStarted", this.ReportAppStarted);
            Store(w, "ReportAppStopped", this.ReportAppStopped);
        }
        #endregion
    }
}
