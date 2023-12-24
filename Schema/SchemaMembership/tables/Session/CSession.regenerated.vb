Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic

Imports Framework

<Serializable(), CLSCompliant(True)> _
Partial Public Class CSession
    Inherits CBaseDynamic
    Implements IComparable(Of CSession)

#Region "Constructors"
    'Public (Copy Constructor)
    Public Sub New(ByVal original as CSession, target As CDataSrc)
        m_dataSrc = target
        m_sessionUserLoginName = original.SessionUserLoginName
        m_clickCount = original.ClickCount
        m_maxDate = original.MaxDate
        m_minDate = original.MinDate
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
        m_sessionId = Integer.MinValue
        m_sessionUserLoginName = String.Empty
    End Sub
#End Region

#Region "Members"
    Protected m_sessionUserLoginName As String
    Protected m_sessionId As Integer
    Protected m_clickCount As Integer
    Protected m_maxDate As DateTime
    Protected m_minDate As DateTime
#End Region

#Region "Properties - Column Values"
    'Primary Key Column (ReadOnly)
    Public ReadOnly Property [SessionId]() As Integer
        Get
            Return m_sessionId
        End Get
    End Property

    'Table Columns (Read/Write)
    Public Property [SessionUserLoginName]() As String
        Get
            Return m_sessionUserLoginName
        End Get
        Set(ByVal value As String)
            m_sessionUserLoginName = value
        End Set
    End Property

    'View Columns (ReadOnly)
    Public ReadOnly Property [ClickCount]() As Integer
        Get
            Return m_clickCount
        End Get
    End Property
    Public ReadOnly Property [MaxDate]() As DateTime
        Get
            Return m_maxDate
        End Get
    End Property
    Public ReadOnly Property [MinDate]() As DateTime
        Get
            Return m_minDate
        End Get
    End Property

#End Region

#Region "MustOverride Methods"
    'Schema Information
    Public Const TABLE_NAME As String = "tblMembership_Session"
    Public Const VIEW_NAME As String  = "vwMembership_Session"          'Used to override Me.ViewName
    Public Const ORDER_BY_COLS As String = "SessionID DESC" 'See the CompareTo method below (Sql-based sorting should match In-Memory sorting)
    Public Const SORTING_COLUMN As String = ""
    Public Overrides ReadOnly Property TableName() As String
        Get
            Return TABLE_NAME
        End Get
    End Property
    Protected Overrides ReadOnly Property ViewName() As String
        Get
            Return "vwMembership_Session"
        End Get
    End Property
    Protected Overrides ReadOnly Property OrderByColumns() As String
        Get
            Return ORDER_BY_COLS
        End Get
    End Property

    'CompareTo Interface (Default Sort Order)
    Public Function CompareTo(other As CSession) As Integer Implements IComparable(Of CSession).CompareTo
        Return Me.SessionID.CompareTo(other.SessionID) *-1
    End Function

    'Primary Key Information
    Public Const PRIMARY_KEY_NAME As String = "SessionId"
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
            Return m_sessionId
        End Get
        Set(ByVal value As Object)
            m_sessionId = CType(Value, Integer)
        End Set
    End Property

    'Factory Methods - Object
    Protected Overrides Function MakeFrom(ByVal row As DataRow) As CBase
        Return New CSession(Me.DataSrc, row)
    End Function
    Protected Overrides Function MakeFrom(ByVal dr As IDataReader) As CBase
        Return New CSession(Me.DataSrc, dr)
    End Function
    
    'Factory Methods - List
    Protected Overrides Function MakeList() As IList
        Return New CSessionList
    End Function
    Protected Overrides Function MakeList(ByVal capacity As Integer) As IList
        Return New CSessionList(capacity)
    End Function

    'Convert from ADO to .Net
    Protected Overrides Sub ReadColumns(ByVal dr As IDataReader)
        m_sessionUserLoginName = CAdoData.GetStr(dr, "SessionUserLoginName")
        m_sessionId = CAdoData.GetInt(dr, "SessionId")
        m_clickCount = CAdoData.GetInt(dr, "ClickCount")
        m_maxDate = CAdoData.GetDate(dr, "MaxDate")
        m_minDate = CAdoData.GetDate(dr, "MinDate")
    End Sub
    Protected Overrides Sub ReadColumns(ByVal dr As DataRow)
        m_sessionUserLoginName = CAdoData.GetStr(dr, "SessionUserLoginName")
        m_sessionId = CAdoData.GetInt(dr, "SessionId")
        m_clickCount = CAdoData.GetInt(dr, "ClickCount")
        m_maxDate = CAdoData.GetDate(dr, "MaxDate")
        m_minDate = CAdoData.GetDate(dr, "MinDate")
    End Sub

    'Parameters for Insert/Update    
    Protected Overrides Function ColumnNameValues() As CNameValueList
        Dim data As New CNameValueList
        data.Add("SessionId", NullVal(m_sessionId))
        data.Add("SessionUserLoginName", NullVal(m_sessionUserLoginName))
        Return data
    End Function
#End Region

#Region "Queries - SelectAll/SelectWhere (inherited methods, cast only)"
    'Select Queries - Non-Paged
    Public Shadows Function SelectAll() As CSessionList
        Return CType(MyBase.SelectAll(), CSessionList)
    End Function
    Public Shadows Function SelectAll(ByVal orderBy As String) As CSessionList
        Return CType(MyBase.SelectAll(orderBy), CSessionList)
    End Function
    Public Shadows Function SelectWhere(ByVal colName As String, ByVal sign As ESign, ByVal colValue As Object) As CSessionList
        Return CType(MyBase.SelectWhere(colName, sign, colValue), CSessionList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteria) As CSessionList
        Return CType(MyBase.SelectWhere(where), CSessionList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteriaList) As CSessionList
        Return CType(MyBase.SelectWhere(where), CSessionList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteriaList, ByVal tableNameOrJoinExpression As String) As CSessionList
        Return CType(MyBase.SelectWhere(where, tableNameOrJoinExpression, Me.OrderByColumns), CSessionList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteriaList, ByVal tableNameOrJoinExpression As String, ByVal orderBy As String) As CSessionList
        Return CType(MyBase.SelectWhere(where, tableNameOrJoinExpression, orderBy), CSessionList)
    End Function
    <Obsolete("Arbitrary where-clause is not necessary portable or safe from sql injection attacks. Consider using the parameterised query interfaces")> _
    Public Shadows Function SelectWhere(ByVal unsafeWhereClause As String) As CSessionList
        Return CType(MyBase.SelectWhere(unsafeWhereClause), CSessionList)
    End Function
    Public Shadows Function SelectById(ByVal sessionId As Integer) As CSessionList
        Return CType(MyBase.SelectById(sessionId), CSessionList)
    End Function
    Public Shadows Function SelectByIds(ByVal ids As List(Of Integer)) As CSessionList
        Return CType(MyBase.SelectByIds(ids), CSessionList)
    End Function
    
    'Select Queries - Paged
    Public Shadows Function SelectAll(ByVal pi As CPagingInfo) As CSessionList
        Return CType(MyBase.SelectAll(pi), CSessionList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal colName As String, ByVal sign As ESign, ByVal colValue As Object) As CSessionList
        Return CType(MyBase.SelectWhere(pi, colName, sign, colValue), CSessionList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal criteria As CCriteria) As CSessionList
        Return CType(MyBase.SelectWhere(pi, criteria), CSessionList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal criteria As CCriteriaList) As CSessionList
        Return CType(MyBase.SelectWhere(pi, criteria), CSessionList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal criteria As CCriteriaList, ByVal tableViewOrJoinExpr As String) As CSessionList
        Return CType(MyBase.SelectWhere(pi, criteria, tableViewOrJoinExpr), CSessionList)
    End Function
    Public Shadows Function SelectByIds(ByVal pi As CPagingInfo, ByVal ids As List(Of Integer)) As CSessionList
        Return CType(MyBase.SelectByIds(pi, ids), CSessionList)
    End Function

    'Select Queries - Transactional
    Public Shadows Function SelectAll(ByVal tx As IDbTransaction) As CSessionList
        Return CType(MyBase.SelectAll(tx), CSessionList)
    End Function
    Public Shadows Function SelectAll(ByVal orderBy As String, ByVal tx As IDbTransaction) As CSessionList
        Return CType(MyBase.SelectAll(orderBy, tx), CSessionList)
    End Function
    Public Shadows Function SelectWhere(ByVal columnName As String, columnValue As Object, ByVal tx As IDbTransaction) As CSessionList
        Return CType(MyBase.SelectWhere(columnName, columnValue, tx), CSessionList)
    End Function
    Public Shadows Function SelectWhere(ByVal columnName As String, sign As ESign, columnValue As Object, ByVal tx As IDbTransaction) As CSessionList
        Return CType(MyBase.SelectWhere(columnName, sign, columnValue, tx), CSessionList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteria, ByVal tx As IDbTransaction) As CSessionList
        Return CType(MyBase.SelectWhere(criteria, tx), CSessionList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteriaList, ByVal tx As IDbTransaction) As CSessionList
        Return CType(MyBase.SelectWhere(criteria, tx), CSessionList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteriaList, ByVal tableOrJoin As String, ByVal tx As IDbTransaction) As CSessionList
        Return CType(MyBase.SelectWhere(criteria, tableOrJoin, tx), CSessionList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteriaList, ByVal tableOrJoin As String, ByVal orderBy As String, ByVal tx As IDbTransaction) As CSessionList
        Return CType(MyBase.SelectWhere(criteria, tableOrJoin, orderBy, tx), CSessionList)
    End Function
    Public Shadows Function SelectById(ByVal sessionId As Integer, ByVal tx As IDbTransaction) As CSessionList
        Return CType(MyBase.SelectById(sessionId, tx), CSessionList)
    End Function
    Public Shadows Function SelectByIds(ByVal ids As List(Of Integer), ByVal tx As IDbTransaction) As CSessionList
        Return CType(MyBase.SelectByIds(ids, tx), CSessionList)
    End Function
    
    'Select Queries - Stored Procedures
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal txOrNull As IDbTransaction) As CSessionList
        Return CType(MyBase.MakeList(storedProcName, txOrNull), CSessionList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal params As Object(), ByVal txOrNull As IDbTransaction) As CSessionList
        Return CType(MyBase.MakeList(storedProcName, params, txOrNull), CSessionList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal params As CNameValueList, ByVal txOrNull As IDbTransaction) As CSessionList
        Return CType(MyBase.MakeList(storedProcName, params, txOrNull), CSessionList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal params As List(Of Object), ByVal txOrNull As IDbTransaction) As CSessionList
        Return CType(MyBase.MakeList(storedProcName, params, txOrNull), CSessionList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal param1 As Integer, ByVal txOrNull As IDbTransaction) As CSessionList
        Return CType(MyBase.MakeList(storedProcName, param1, txOrNull), CSessionList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal param1 As String, ByVal txOrNull As IDbTransaction) As CSessionList
        Return CType(MyBase.MakeList(storedProcName, param1, txOrNull), CSessionList)
    End Function

    'Query Results
    Protected Overloads Function MakeList(ByVal ds As DataSet) As CSessionList
        Return CType(MyBase.MakeList(ds), CSessionList)
    End Function
    Protected Overloads Function MakeList(ByVal dt As DataTable) As CSessionList
        Return CType(MyBase.MakeList(dt), CSessionList)
    End Function
    Protected Overloads Function MakeList(ByVal rows As DataRowCollection) As CSessionList
        Return CType(MyBase.MakeList(rows), CSessionList)
    End Function
    Protected Overloads Function MakeList(ByVal dr As IDataReader) As CSessionList
        Return CType(MyBase.MakeList(dr), CSessionList)
    End Function
    Protected Overloads Function MakeList(ByVal drOrDs As Object) As CSessionList
        Return CType(MyBase.MakeList(drOrDs), CSessionList)
    End Function
    Protected Overloads Function MakeList(ByVal gzip As Byte()) As CSessionList
        Return CType(MyBase.MakeList(gzip), CSessionList)
    End Function
#End Region

#Region "Queries - SelectBy[FK] (user-nominated fk/bool columns)"
    'Non-Paged

    'Paged

    'Count

    'Transactional
#End Region

#Region "ToXml"
    Protected Overrides Sub ToXml_Autogenerated(ByVal w As System.Xml.XmlWriter)
        Store(w, "SessionId", Me.SessionId)
        Store(w, "SessionUserLoginName", Me.SessionUserLoginName)
    End Sub
#End Region


End Class