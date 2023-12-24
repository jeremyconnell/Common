Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic

Imports Framework

<Serializable(), CLSCompliant(True)> _
Partial Public Class CClick
    Inherits CBaseDynamic
    Implements IComparable(Of CClick)

#Region "Constructors"
    'Public (Copy Constructor)
    Public Sub New(ByVal original as CClick, target As CDataSrc)
        m_dataSrc = target
        m_clickSessionId = original.ClickSessionId
        m_clickHost = original.ClickHost
        m_clickUrl = original.ClickUrl
        m_clickQuerystring = original.ClickQuerystring
        m_clickDate = original.ClickDate
        m_sessionUserLoginName = original.SessionUserLoginName
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
        m_clickId = Integer.MinValue
        m_clickSessionId = Integer.MinValue
        m_clickHost = String.Empty
        m_clickUrl = String.Empty
        m_clickQuerystring = String.Empty
        m_clickDate = DateTime.MinValue
    End Sub
#End Region

#Region "Members"
    Protected m_clickId As Integer
    Protected m_clickSessionId As Integer
    Protected m_clickHost As String
    Protected m_clickUrl As String
    Protected m_clickQuerystring As String
    Protected m_clickDate As DateTime
    Protected m_sessionUserLoginName As String
#End Region

#Region "Properties - Column Values"
    'Primary Key Column (ReadOnly)
    Public ReadOnly Property [ClickId]() As Integer
        Get
            Return m_clickId
        End Get
    End Property

    'Table Columns (Read/Write)
    Public Property [ClickSessionId]() As Integer
        Get
            Return m_clickSessionId
        End Get
        Set(ByVal value As Integer)
            m_clickSessionId = value
        End Set
    End Property
    Public Property [ClickHost]() As String
        Get
            Return m_clickHost
        End Get
        Set(ByVal value As String)
            m_clickHost = value
        End Set
    End Property
    Public Property [ClickUrl]() As String
        Get
            Return m_clickUrl
        End Get
        Set(ByVal value As String)
            m_clickUrl = value
        End Set
    End Property
    Public Property [ClickQuerystring]() As String
        Get
            Return m_clickQuerystring
        End Get
        Set(ByVal value As String)
            m_clickQuerystring = value
        End Set
    End Property
    Public Property [ClickDate]() As DateTime
        Get
            Return m_clickDate
        End Get
        Set(ByVal value As DateTime)
            m_clickDate = value
        End Set
    End Property

    'View Columns (ReadOnly)
    Public ReadOnly Property [SessionUserLoginName]() As String
        Get
            Return m_sessionUserLoginName
        End Get
    End Property

#End Region

#Region "MustOverride Methods"
    'Schema Information
    Public Const TABLE_NAME As String = "tblMembership_Click"
    Public Const VIEW_NAME As String  = "vwMembership_Click"          'Used to override Me.ViewName
    Public Const ORDER_BY_COLS As String = "ClickId DESC" 'See the CompareTo method below (Sql-based sorting should match In-Memory sorting)
    Public Const SORTING_COLUMN As String = ""
    Public Overrides ReadOnly Property TableName() As String
        Get
            Return TABLE_NAME
        End Get
    End Property
    Protected Overrides ReadOnly Property ViewName() As String
        Get
            Return "vwMembership_Click"
        End Get
    End Property
    Protected Overrides ReadOnly Property OrderByColumns() As String
        Get
            Return ORDER_BY_COLS
        End Get
    End Property

    'CompareTo Interface (Default Sort Order)
    Public Function CompareTo(other As CClick) As Integer Implements IComparable(Of CClick).CompareTo
        Return -1 * Me.ClickId.CompareTo(other.ClickId)
    End Function

    'Primary Key Information
    Public Const PRIMARY_KEY_NAME As String = "ClickId"
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
            Return m_clickId
        End Get
        Set(ByVal value As Object)
            m_clickId = CType(Value, Integer)
        End Set
    End Property

    'Factory Methods - Object
    Protected Overrides Function MakeFrom(ByVal row As DataRow) As CBase
        Return New CClick(Me.DataSrc, row)
    End Function
    Protected Overrides Function MakeFrom(ByVal dr As IDataReader) As CBase
        Return New CClick(Me.DataSrc, dr)
    End Function
    
    'Factory Methods - List
    Protected Overrides Function MakeList() As IList
        Return New CClickList
    End Function
    Protected Overrides Function MakeList(ByVal capacity As Integer) As IList
        Return New CClickList(capacity)
    End Function

    'Convert from ADO to .Net
    Protected Overrides Sub ReadColumns(ByVal dr As IDataReader)
        m_clickId = CAdoData.GetInt(dr, "ClickId")
        m_clickSessionId = CAdoData.GetInt(dr, "ClickSessionId")
        m_clickHost = CAdoData.GetStr(dr, "ClickHost")
        m_clickUrl = CAdoData.GetStr(dr, "ClickUrl")
        m_clickQuerystring = CAdoData.GetStr(dr, "ClickQuerystring")
        m_clickDate = CAdoData.GetDate(dr, "ClickDate")
        m_sessionUserLoginName = CAdoData.GetStr(dr, "SessionUserLoginName")
    End Sub
    Protected Overrides Sub ReadColumns(ByVal dr As DataRow)
        m_clickId = CAdoData.GetInt(dr, "ClickId")
        m_clickSessionId = CAdoData.GetInt(dr, "ClickSessionId")
        m_clickHost = CAdoData.GetStr(dr, "ClickHost")
        m_clickUrl = CAdoData.GetStr(dr, "ClickUrl")
        m_clickQuerystring = CAdoData.GetStr(dr, "ClickQuerystring")
        m_clickDate = CAdoData.GetDate(dr, "ClickDate")
        m_sessionUserLoginName = CAdoData.GetStr(dr, "SessionUserLoginName")
    End Sub

    'Parameters for Insert/Update    
    Protected Overrides Function ColumnNameValues() As CNameValueList
        Dim data As New CNameValueList
        data.Add("ClickId", NullVal(m_clickId))
        data.Add("ClickSessionId", NullVal(m_clickSessionId))
        data.Add("ClickHost", NullVal(m_clickHost))
        data.Add("ClickUrl", NullVal(m_clickUrl))
        data.Add("ClickQuerystring", NullVal(m_clickQuerystring))
        data.Add("ClickDate", NullVal(m_clickDate))
        Return data
    End Function
#End Region

#Region "Queries - SelectAll/SelectWhere (inherited methods, cast only)"
    'Select Queries - Non-Paged
    Public Shadows Function SelectAll() As CClickList
        Return CType(MyBase.SelectAll(), CClickList)
    End Function
    Public Shadows Function SelectAll(ByVal orderBy As String) As CClickList
        Return CType(MyBase.SelectAll(orderBy), CClickList)
    End Function
    Public Shadows Function SelectWhere(ByVal colName As String, ByVal sign As ESign, ByVal colValue As Object) As CClickList
        Return CType(MyBase.SelectWhere(colName, sign, colValue), CClickList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteria) As CClickList
        Return CType(MyBase.SelectWhere(where), CClickList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteriaList) As CClickList
        Return CType(MyBase.SelectWhere(where), CClickList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteriaList, ByVal tableNameOrJoinExpression As String) As CClickList
        Return CType(MyBase.SelectWhere(where, tableNameOrJoinExpression, Me.OrderByColumns), CClickList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteriaList, ByVal tableNameOrJoinExpression As String, ByVal orderBy As String) As CClickList
        Return CType(MyBase.SelectWhere(where, tableNameOrJoinExpression, orderBy), CClickList)
    End Function
    <Obsolete("Arbitrary where-clause is not necessary portable or safe from sql injection attacks. Consider using the parameterised query interfaces")> _
    Public Shadows Function SelectWhere(ByVal unsafeWhereClause As String) As CClickList
        Return CType(MyBase.SelectWhere(unsafeWhereClause), CClickList)
    End Function
    Public Shadows Function SelectById(ByVal clickId As Integer) As CClickList
        Return CType(MyBase.SelectById(clickId), CClickList)
    End Function
    Public Shadows Function SelectByIds(ByVal ids As List(Of Integer)) As CClickList
        Return CType(MyBase.SelectByIds(ids), CClickList)
    End Function
    
    'Select Queries - Paged
    Public Shadows Function SelectAll(ByVal pi As CPagingInfo) As CClickList
        Return CType(MyBase.SelectAll(pi), CClickList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal colName As String, ByVal sign As ESign, ByVal colValue As Object) As CClickList
        Return CType(MyBase.SelectWhere(pi, colName, sign, colValue), CClickList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal criteria As CCriteria) As CClickList
        Return CType(MyBase.SelectWhere(pi, criteria), CClickList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal criteria As CCriteriaList) As CClickList
        Return CType(MyBase.SelectWhere(pi, criteria), CClickList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal criteria As CCriteriaList, ByVal tableViewOrJoinExpr As String) As CClickList
        Return CType(MyBase.SelectWhere(pi, criteria, tableViewOrJoinExpr), CClickList)
    End Function
    Public Shadows Function SelectByIds(ByVal pi As CPagingInfo, ByVal ids As List(Of Integer)) As CClickList
        Return CType(MyBase.SelectByIds(pi, ids), CClickList)
    End Function

    'Select Queries - Transactional
    Public Shadows Function SelectAll(ByVal tx As IDbTransaction) As CClickList
        Return CType(MyBase.SelectAll(tx), CClickList)
    End Function
    Public Shadows Function SelectAll(ByVal orderBy As String, ByVal tx As IDbTransaction) As CClickList
        Return CType(MyBase.SelectAll(orderBy, tx), CClickList)
    End Function
    Public Shadows Function SelectWhere(ByVal columnName As String, columnValue As Object, ByVal tx As IDbTransaction) As CClickList
        Return CType(MyBase.SelectWhere(columnName, columnValue, tx), CClickList)
    End Function
    Public Shadows Function SelectWhere(ByVal columnName As String, sign As ESign, columnValue As Object, ByVal tx As IDbTransaction) As CClickList
        Return CType(MyBase.SelectWhere(columnName, sign, columnValue, tx), CClickList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteria, ByVal tx As IDbTransaction) As CClickList
        Return CType(MyBase.SelectWhere(criteria, tx), CClickList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteriaList, ByVal tx As IDbTransaction) As CClickList
        Return CType(MyBase.SelectWhere(criteria, tx), CClickList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteriaList, ByVal tableOrJoin As String, ByVal tx As IDbTransaction) As CClickList
        Return CType(MyBase.SelectWhere(criteria, tableOrJoin, tx), CClickList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteriaList, ByVal tableOrJoin As String, ByVal orderBy As String, ByVal tx As IDbTransaction) As CClickList
        Return CType(MyBase.SelectWhere(criteria, tableOrJoin, orderBy, tx), CClickList)
    End Function
    Public Shadows Function SelectById(ByVal clickId As Integer, ByVal tx As IDbTransaction) As CClickList
        Return CType(MyBase.SelectById(clickId, tx), CClickList)
    End Function
    Public Shadows Function SelectByIds(ByVal ids As List(Of Integer), ByVal tx As IDbTransaction) As CClickList
        Return CType(MyBase.SelectByIds(ids, tx), CClickList)
    End Function
    
    'Select Queries - Stored Procedures
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal txOrNull As IDbTransaction) As CClickList
        Return CType(MyBase.MakeList(storedProcName, txOrNull), CClickList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal params As Object(), ByVal txOrNull As IDbTransaction) As CClickList
        Return CType(MyBase.MakeList(storedProcName, params, txOrNull), CClickList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal params As CNameValueList, ByVal txOrNull As IDbTransaction) As CClickList
        Return CType(MyBase.MakeList(storedProcName, params, txOrNull), CClickList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal params As List(Of Object), ByVal txOrNull As IDbTransaction) As CClickList
        Return CType(MyBase.MakeList(storedProcName, params, txOrNull), CClickList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal param1 As Integer, ByVal txOrNull As IDbTransaction) As CClickList
        Return CType(MyBase.MakeList(storedProcName, param1, txOrNull), CClickList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal param1 As String, ByVal txOrNull As IDbTransaction) As CClickList
        Return CType(MyBase.MakeList(storedProcName, param1, txOrNull), CClickList)
    End Function

    'Query Results
    Protected Overloads Function MakeList(ByVal ds As DataSet) As CClickList
        Return CType(MyBase.MakeList(ds), CClickList)
    End Function
    Protected Overloads Function MakeList(ByVal dt As DataTable) As CClickList
        Return CType(MyBase.MakeList(dt), CClickList)
    End Function
    Protected Overloads Function MakeList(ByVal rows As DataRowCollection) As CClickList
        Return CType(MyBase.MakeList(rows), CClickList)
    End Function
    Protected Overloads Function MakeList(ByVal dr As IDataReader) As CClickList
        Return CType(MyBase.MakeList(dr), CClickList)
    End Function
    Protected Overloads Function MakeList(ByVal drOrDs As Object) As CClickList
        Return CType(MyBase.MakeList(drOrDs), CClickList)
    End Function
    Protected Overloads Function MakeList(ByVal gzip As Byte()) As CClickList
        Return CType(MyBase.MakeList(gzip), CClickList)
    End Function
#End Region

#Region "Queries - SelectBy[FK] (user-nominated fk/bool columns)"
    'Non-Paged
    Public Function SelectBySessionId(ByVal clickSessionId As Integer) As CClickList
        Return SelectWhere(new CCriteriaList("ClickSessionId", clickSessionId))
    End Function

    'Paged
    Public Function SelectBySessionId(pi as CPagingInfo, ByVal clickSessionId As Integer) As CClickList
        Return SelectWhere(pi, New CCriteriaList("ClickSessionId", clickSessionId))
    End Function

    'Count
    Public Function SelectCountBySessionId(ByVal clickSessionId As Integer) As Integer
        Return SelectCount(New CCriteriaList("ClickSessionId", clickSessionId))
    End Function

    'Transactional
    Public Function SelectBySessionId(ByVal clickSessionId As Integer, tx As IDbTransaction) As CClickList
        Return SelectWhere(New CCriteriaList("ClickSessionId", clickSessionId), tx)
    End Function
#End Region

#Region "ToXml"
    Protected Overrides Sub ToXml_Autogenerated(ByVal w As System.Xml.XmlWriter)
        Store(w, "ClickId", Me.ClickId)
        Store(w, "ClickSessionId", Me.ClickSessionId)
        Store(w, "ClickHost", Me.ClickHost)
        Store(w, "ClickUrl", Me.ClickUrl)
        Store(w, "ClickQuerystring", Me.ClickQuerystring)
        Store(w, "ClickDate", Me.ClickDate)
    End Sub
#End Region


End Class