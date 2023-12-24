Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic

Imports Framework

<Serializable(), CLSCompliant(True)> _
Partial Public Class CAudit_Log
    Inherits CBaseDynamic
    Implements IComparable(Of CAudit_Log)

#Region "Constructors"
    'Public (Copy Constructor)
    Public Sub New(ByVal original As CAudit_Log, target As CDataSrc)
        m_dataSrc = target
        m_logTypeId = original.LogTypeId
        m_logMessage = original.LogMessage
        m_logCreated = original.LogCreated
    End Sub

    'Protected (Datareader/Dataset)
    Protected Sub New(ByVal dataSrc As CDataSrc, ByVal dr As IDataReader)
        MyBase.New(dataSrc, dr)
    End Sub
    Protected Sub New(ByVal dataSrc As CDataSrc, ByVal dr As DataRow)
        MyBase.New(dataSrc, dr)
    End Sub
#End Region

#Region "Default Values"
    Protected Overrides Sub InitValues_Auto()
        'Null-Equivalent values (except String.Empty, as nulls tend to be inconvenient for string types)
        m_logId = Integer.MinValue
        m_logTypeId = Integer.MinValue
        m_logMessage = String.Empty
        m_logCreated = DateTime.MinValue
    End Sub
#End Region

#Region "Members"
    Protected m_logId As Integer
    Protected m_logTypeId As Integer
    Protected m_logMessage As String
    Protected m_logCreated As DateTime
#End Region

#Region "Properties - Column Values"
    'Primary Key Column (ReadOnly)
    Public ReadOnly Property [LogId]() As Integer
        Get
            Return m_logId
        End Get
    End Property

    'Table Columns (Read/Write)
    Public Property [LogTypeId]() As Integer
        Get
            Return m_logTypeId
        End Get
        Set(ByVal value As Integer)
            m_logTypeId = value
        End Set
    End Property
    Public Property [LogMessage]() As String
        Get
            Return m_logMessage
        End Get
        Set(ByVal value As String)
            m_logMessage = value
        End Set
    End Property
    Public Property [LogCreated]() As DateTime
        Get
            Return m_logCreated
        End Get
        Set(ByVal value As DateTime)
            m_logCreated = value
        End Set
    End Property

    'View Columns (ReadOnly)

#End Region

#Region "MustOverride Methods"
    'Schema Information
    Public Const TABLE_NAME As String = "tblAudit_Log"
    Public Const VIEW_NAME As String = ""          'Used to override Me.ViewName
    Public Const ORDER_BY_COLS As String = "LogId DESC" 'See the CompareTo method below (Sql-based sorting should match In-Memory sorting)
    Public Const SORTING_COLUMN As String = ""
    Public Overrides ReadOnly Property TableName() As String
        Get
            Return TABLE_NAME
        End Get
    End Property
    Protected Overrides ReadOnly Property OrderByColumns() As String
        Get
            Return ORDER_BY_COLS
        End Get
    End Property

    'CompareTo Interface (Default Sort Order)
    Public Function CompareTo(other As CAudit_Log) As Integer Implements IComparable(Of CAudit_Log).CompareTo
        Return Me.LogId.CompareTo(other.LogId) * -1
    End Function

    'Primary Key Information
    Public Const PRIMARY_KEY_NAME As String = "LogId"
    Protected Overrides ReadOnly Property InsertPrimaryKey() As Boolean
        Get
            Return False
        End Get
    End Property
    Protected Overrides ReadOnly Property PrimaryKeyName() As String
        Get
            Return PRIMARY_KEY_NAME
        End Get
    End Property
    Protected Overrides Property PrimaryKeyValue() As Object
        Get
            Return m_logId
        End Get
        Set(ByVal value As Object)
            m_logId = CType(Value, Integer)
        End Set
    End Property

    'Factory Methods - Object
    Protected Overrides Function MakeFrom(ByVal row As DataRow) As CBase
        Return New CAudit_Log(Me.DataSrc, row)
    End Function
    Protected Overrides Function MakeFrom(ByVal dr As IDataReader) As CBase
        Return New CAudit_Log(Me.DataSrc, dr)
    End Function

    'Factory Methods - List
    Protected Overrides Function MakeList() As IList
        Return New CAudit_LogList
    End Function
    Protected Overrides Function MakeList(ByVal capacity As Integer) As IList
        Return New CAudit_LogList(capacity)
    End Function

    'Convert from ADO to .Net
    Protected Overrides Sub ReadColumns(ByVal dr As IDataReader)
        m_logId = CAdoData.GetInt(dr, "LogId")
        m_logTypeId = CAdoData.GetInt(dr, "LogTypeId")
        m_logMessage = CAdoData.GetStr(dr, "LogMessage")
        m_logCreated = CAdoData.GetDate(dr, "LogCreated")
    End Sub
    Protected Overrides Sub ReadColumns(ByVal dr As DataRow)
        m_logId = CAdoData.GetInt(dr, "LogId")
        m_logTypeId = CAdoData.GetInt(dr, "LogTypeId")
        m_logMessage = CAdoData.GetStr(dr, "LogMessage")
        m_logCreated = CAdoData.GetDate(dr, "LogCreated")
    End Sub

    'Parameters for Insert/Update    
    Protected Overrides Function ColumnNameValues() As CNameValueList
        Dim data As New CNameValueList
        data.Add("LogId", NullVal(m_logId))
        data.Add("LogTypeId", NullVal(m_logTypeId))
        data.Add("LogMessage", NullVal(m_logMessage))
        data.Add("LogCreated", NullVal(m_logCreated))
        Return data
    End Function
#End Region

#Region "Queries - SelectAll/SelectWhere (inherited methods, cast only)"
    'Select Queries - Non-Paged
    Public Shadows Function SelectAll() As CAudit_LogList
        Return CType(MyBase.SelectAll(), CAudit_LogList)
    End Function
    Public Shadows Function SelectAll(ByVal orderBy As String) As CAudit_LogList
        Return CType(MyBase.SelectAll(orderBy), CAudit_LogList)
    End Function
    Public Shadows Function SelectWhere(ByVal colName As String, ByVal sign As ESign, ByVal colValue As Object) As CAudit_LogList
        Return CType(MyBase.SelectWhere(colName, sign, colValue), CAudit_LogList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteria) As CAudit_LogList
        Return CType(MyBase.SelectWhere(where), CAudit_LogList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteriaList) As CAudit_LogList
        Return CType(MyBase.SelectWhere(where), CAudit_LogList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteriaList, ByVal tableNameOrJoinExpression As String) As CAudit_LogList
        Return CType(MyBase.SelectWhere(where, tableNameOrJoinExpression, Me.OrderByColumns), CAudit_LogList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteriaList, ByVal tableNameOrJoinExpression As String, ByVal orderBy As String) As CAudit_LogList
        Return CType(MyBase.SelectWhere(where, tableNameOrJoinExpression, orderBy), CAudit_LogList)
    End Function
    <Obsolete("Arbitrary where-clause is not necessary portable or safe from sql injection attacks. Consider using the parameterised query interfaces")> _
    Public Shadows Function SelectWhere(ByVal unsafeWhereClause As String) As CAudit_LogList
        Return CType(MyBase.SelectWhere(unsafeWhereClause), CAudit_LogList)
    End Function
    Public Shadows Function SelectById(ByVal logId As Integer) As CAudit_LogList
        Return CType(MyBase.SelectById(logId), CAudit_LogList)
    End Function
    Public Shadows Function SelectByIds(ByVal ids As List(Of Integer)) As CAudit_LogList
        Return CType(MyBase.SelectByIds(ids), CAudit_LogList)
    End Function

    'Select Queries - Paged
    Public Shadows Function SelectAll(ByVal pi As CPagingInfo) As CAudit_LogList
        Return CType(MyBase.SelectAll(pi), CAudit_LogList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal colName As String, ByVal sign As ESign, ByVal colValue As Object) As CAudit_LogList
        Return CType(MyBase.SelectWhere(pi, colName, sign, colValue), CAudit_LogList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal criteria As CCriteria) As CAudit_LogList
        Return CType(MyBase.SelectWhere(pi, criteria), CAudit_LogList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal criteria As CCriteriaList) As CAudit_LogList
        Return CType(MyBase.SelectWhere(pi, criteria), CAudit_LogList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal criteria As CCriteriaList, ByVal tableViewOrJoinExpr As String) As CAudit_LogList
        Return CType(MyBase.SelectWhere(pi, criteria, tableViewOrJoinExpr), CAudit_LogList)
    End Function
    Public Shadows Function SelectByIds(ByVal pi As CPagingInfo, ByVal ids As List(Of Integer)) As CAudit_LogList
        Return CType(MyBase.SelectByIds(pi, ids), CAudit_LogList)
    End Function

    'Select Queries - Transactional
    Public Shadows Function SelectAll(ByVal tx As IDbTransaction) As CAudit_LogList
        Return CType(MyBase.SelectAll(tx), CAudit_LogList)
    End Function
    Public Shadows Function SelectAll(ByVal orderBy As String, ByVal tx As IDbTransaction) As CAudit_LogList
        Return CType(MyBase.SelectAll(orderBy, tx), CAudit_LogList)
    End Function
    Public Shadows Function SelectWhere(ByVal columnName As String, columnValue As Object, ByVal tx As IDbTransaction) As CAudit_LogList
        Return CType(MyBase.SelectWhere(columnName, columnValue, tx), CAudit_LogList)
    End Function
    Public Shadows Function SelectWhere(ByVal columnName As String, sign As ESign, columnValue As Object, ByVal tx As IDbTransaction) As CAudit_LogList
        Return CType(MyBase.SelectWhere(columnName, sign, columnValue, tx), CAudit_LogList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteria, ByVal tx As IDbTransaction) As CAudit_LogList
        Return CType(MyBase.SelectWhere(criteria, tx), CAudit_LogList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteriaList, ByVal tx As IDbTransaction) As CAudit_LogList
        Return CType(MyBase.SelectWhere(criteria, tx), CAudit_LogList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteriaList, ByVal tableOrJoin As String, ByVal tx As IDbTransaction) As CAudit_LogList
        Return CType(MyBase.SelectWhere(criteria, tableOrJoin, tx), CAudit_LogList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteriaList, ByVal tableOrJoin As String, ByVal orderBy As String, ByVal tx As IDbTransaction) As CAudit_LogList
        Return CType(MyBase.SelectWhere(criteria, tableOrJoin, orderBy, tx), CAudit_LogList)
    End Function
    Public Shadows Function SelectById(ByVal logId As Integer, ByVal tx As IDbTransaction) As CAudit_LogList
        Return CType(MyBase.SelectById(logId, tx), CAudit_LogList)
    End Function
    Public Shadows Function SelectByIds(ByVal ids As List(Of Integer), ByVal tx As IDbTransaction) As CAudit_LogList
        Return CType(MyBase.SelectByIds(ids, tx), CAudit_LogList)
    End Function

    'Select Queries - Stored Procedures
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal txOrNull As IDbTransaction) As CAudit_LogList
        Return CType(MyBase.MakeList(storedProcName, txOrNull), CAudit_LogList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal params As Object(), ByVal txOrNull As IDbTransaction) As CAudit_LogList
        Return CType(MyBase.MakeList(storedProcName, params, txOrNull), CAudit_LogList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal params As CNameValueList, ByVal txOrNull As IDbTransaction) As CAudit_LogList
        Return CType(MyBase.MakeList(storedProcName, params, txOrNull), CAudit_LogList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal params As List(Of Object), ByVal txOrNull As IDbTransaction) As CAudit_LogList
        Return CType(MyBase.MakeList(storedProcName, params, txOrNull), CAudit_LogList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal param1 As Integer, ByVal txOrNull As IDbTransaction) As CAudit_LogList
        Return CType(MyBase.MakeList(storedProcName, param1, txOrNull), CAudit_LogList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal param1 As String, ByVal txOrNull As IDbTransaction) As CAudit_LogList
        Return CType(MyBase.MakeList(storedProcName, param1, txOrNull), CAudit_LogList)
    End Function

    'Query Results
    Public Overloads Function MakeList(ByVal ds As DataSet) As CAudit_LogList
        Return CType(MyBase.MakeList(ds), CAudit_LogList)
    End Function
    Public Overloads Function MakeList(ByVal dt As DataTable) As CAudit_LogList
        Return CType(MyBase.MakeList(dt), CAudit_LogList)
    End Function
    Public Overloads Function MakeList(ByVal rows As DataRowCollection) As CAudit_LogList
        Return CType(MyBase.MakeList(rows), CAudit_LogList)
    End Function
    Public Overloads Function MakeList(ByVal dr As IDataReader) As CAudit_LogList
        Return CType(MyBase.MakeList(dr), CAudit_LogList)
    End Function
    Public Overloads Function MakeList(ByVal drOrDs As Object) As CAudit_LogList
        Return CType(MyBase.MakeList(drOrDs), CAudit_LogList)
    End Function
    Public Overloads Function MakeList(ByVal gzip As Byte()) As CAudit_LogList
        Return CType(MyBase.MakeList(gzip), CAudit_LogList)
    End Function
#End Region

#Region "Queries - SelectBy[FK] (user-nominated fk/bool columns)"
    'Non-Paged
    Public Function SelectByTypeId(ByVal logTypeId As Integer) As CAudit_LogList
        Return SelectWhere(New CCriteriaList("LogTypeId", logTypeId))
    End Function

    'Paged
    Public Function SelectByTypeId(pi As CPagingInfo, ByVal logTypeId As Integer) As CAudit_LogList
        Return SelectWhere(pi, New CCriteriaList("LogTypeId", logTypeId))
    End Function

    'Count
    Public Function SelectCountByTypeId(ByVal logTypeId As Integer) As Integer
        Return SelectCount(New CCriteriaList("LogTypeId", logTypeId))
    End Function

    'Transactional
    Public Function SelectByTypeId(ByVal logTypeId As Integer, tx As IDbTransaction) As CAudit_LogList
        Return SelectWhere(New CCriteriaList("LogTypeId", logTypeId), tx)
    End Function
#End Region

#Region "ToXml"
    Protected Overrides Sub ToXml_Autogenerated(ByVal w As System.Xml.XmlWriter)
        Store(w, "LogId", Me.LogId)
        Store(w, "LogTypeId", Me.LogTypeId)
        Store(w, "LogMessage", Me.LogMessage)
        Store(w, "LogCreated", Me.LogCreated)
    End Sub
#End Region


End Class