Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic

Imports Framework

<Serializable(), CLSCompliant(True)>
Partial Public Class CAudit_Sql
    Inherits CBaseDynamic
    Implements IComparable(Of CAudit_Sql)

#Region "Constructors"
    'Public (Copy Constructor)
    Public Sub New(ByVal original As CAudit_Sql, target As CDataSrc)
        m_dataSrc = target
        m_sqlText = original.SqlText
        m_sqlCreated = original.SqlCreated
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
        m_sqlId = Integer.MinValue
        m_sqlText = String.Empty
        m_sqlCreated = DateTime.MinValue
    End Sub
#End Region

#Region "Members"
    Protected m_sqlId As Integer
    Protected m_sqlText As String
    Protected m_sqlCreated As DateTime
#End Region

#Region "Properties - Column Values"
    'Primary Key Column (ReadOnly)
    Public ReadOnly Property [SqlId]() As Integer
        Get
            Return m_sqlId
        End Get
    End Property

    'Table Columns (Read/Write)
    Public Property [SqlText]() As String
        Get
            Return m_sqlText
        End Get
        Set(ByVal value As String)
            m_sqlText = value
        End Set
    End Property
    Public Property [SqlCreated]() As DateTime
        Get
            Return m_sqlCreated
        End Get
        Set(ByVal value As DateTime)
            m_sqlCreated = value
        End Set
    End Property

    'View Columns (ReadOnly)

#End Region

#Region "MustOverride Methods"
    'Schema Information
    Public Const TABLE_NAME As String = "tblAudit_Sql"
    Public Const VIEW_NAME As String = ""          'Used to override Me.ViewName
    Public Const ORDER_BY_COLS As String = "SqlId" 'See the CompareTo method below (Sql-based sorting should match In-Memory sorting)
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
    Public Function CompareTo(other As CAudit_Sql) As Integer Implements IComparable(Of CAudit_Sql).CompareTo
        Return Me.SqlId.CompareTo(other.SqlId)
    End Function

    'Primary Key Information
    Public Const PRIMARY_KEY_NAME As String = "SqlId"
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
            Return m_sqlId
        End Get
        Set(ByVal value As Object)
            m_sqlId = CType(value, Integer)
        End Set
    End Property

    'Factory Methods - Object
    Protected Overrides Function MakeFrom(ByVal row As DataRow) As CBase
        Return New CAudit_Sql(Me.DataSrc, row)
    End Function
    Protected Overrides Function MakeFrom(ByVal dr As IDataReader) As CBase
        Return New CAudit_Sql(Me.DataSrc, dr)
    End Function

    'Factory Methods - List
    Protected Overrides Function MakeList() As IList
        Return New CAudit_SqlList
    End Function
    Protected Overrides Function MakeList(ByVal capacity As Integer) As IList
        Return New CAudit_SqlList(capacity)
    End Function

    'Convert from ADO to .Net
    Protected Overrides Sub ReadColumns(ByVal dr As IDataReader)
        m_sqlId = CAdoData.GetInt(dr, "SqlId")
        m_sqlText = CAdoData.GetStr(dr, "SqlText")
        m_sqlCreated = CAdoData.GetDate(dr, "SqlCreated")
    End Sub
    Protected Overrides Sub ReadColumns(ByVal dr As DataRow)
        m_sqlId = CAdoData.GetInt(dr, "SqlId")
        m_sqlText = CAdoData.GetStr(dr, "SqlText")
        m_sqlCreated = CAdoData.GetDate(dr, "SqlCreated")
    End Sub

    'Parameters for Insert/Update    
    Protected Overrides Function ColumnNameValues() As CNameValueList
        Dim data As New CNameValueList
        data.Add("SqlId", NullVal(m_sqlId))
        data.Add("SqlText", NullVal(m_sqlText))
        data.Add("SqlCreated", NullVal(m_sqlCreated))
        Return data
    End Function
#End Region

#Region "Queries - SelectAll/SelectWhere (inherited methods, cast only)"
    'Select Queries - Non-Paged
    Public Shadows Function SelectAll() As CAudit_SqlList
        Return CType(MyBase.SelectAll(), CAudit_SqlList)
    End Function
    Public Shadows Function SelectAll(ByVal orderBy As String) As CAudit_SqlList
        Return CType(MyBase.SelectAll(orderBy), CAudit_SqlList)
    End Function
    Public Shadows Function SelectWhere(ByVal colName As String, ByVal sign As ESign, ByVal colValue As Object) As CAudit_SqlList
        Return CType(MyBase.SelectWhere(colName, sign, colValue), CAudit_SqlList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteria) As CAudit_SqlList
        Return CType(MyBase.SelectWhere(where), CAudit_SqlList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteriaList) As CAudit_SqlList
        Return CType(MyBase.SelectWhere(where), CAudit_SqlList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteriaList, ByVal tableNameOrJoinExpression As String) As CAudit_SqlList
        Return CType(MyBase.SelectWhere(where, tableNameOrJoinExpression, Me.OrderByColumns), CAudit_SqlList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteriaList, ByVal tableNameOrJoinExpression As String, ByVal orderBy As String) As CAudit_SqlList
        Return CType(MyBase.SelectWhere(where, tableNameOrJoinExpression, orderBy), CAudit_SqlList)
    End Function
    <Obsolete("Arbitrary where-clause is not necessary portable or safe from sql injection attacks. Consider using the parameterised query interfaces")>
    Public Shadows Function SelectWhere(ByVal unsafeWhereClause As String) As CAudit_SqlList
        Return CType(MyBase.SelectWhere(unsafeWhereClause), CAudit_SqlList)
    End Function
    Public Shadows Function SelectById(ByVal sqlId As Integer) As CAudit_SqlList
        Return CType(MyBase.SelectById(sqlId), CAudit_SqlList)
    End Function
    Public Shadows Function SelectByIds(ByVal ids As List(Of Integer)) As CAudit_SqlList
        Return CType(MyBase.SelectByIds(ids), CAudit_SqlList)
    End Function

    'Select Queries - Paged
    Public Shadows Function SelectAll(ByVal pi As CPagingInfo) As CAudit_SqlList
        Return CType(MyBase.SelectAll(pi), CAudit_SqlList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal colName As String, ByVal sign As ESign, ByVal colValue As Object) As CAudit_SqlList
        Return CType(MyBase.SelectWhere(pi, colName, sign, colValue), CAudit_SqlList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal criteria As CCriteria) As CAudit_SqlList
        Return CType(MyBase.SelectWhere(pi, criteria), CAudit_SqlList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal criteria As CCriteriaList) As CAudit_SqlList
        Return CType(MyBase.SelectWhere(pi, criteria), CAudit_SqlList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal criteria As CCriteriaList, ByVal tableViewOrJoinExpr As String) As CAudit_SqlList
        Return CType(MyBase.SelectWhere(pi, criteria, tableViewOrJoinExpr), CAudit_SqlList)
    End Function
    Public Shadows Function SelectByIds(ByVal pi As CPagingInfo, ByVal ids As List(Of Integer)) As CAudit_SqlList
        Return CType(MyBase.SelectByIds(pi, ids), CAudit_SqlList)
    End Function

    'Select Queries - Transactional
    Public Shadows Function SelectAll(ByVal tx As IDbTransaction) As CAudit_SqlList
        Return CType(MyBase.SelectAll(tx), CAudit_SqlList)
    End Function
    Public Shadows Function SelectAll(ByVal orderBy As String, ByVal tx As IDbTransaction) As CAudit_SqlList
        Return CType(MyBase.SelectAll(orderBy, tx), CAudit_SqlList)
    End Function
    Public Shadows Function SelectWhere(ByVal columnName As String, columnValue As Object, ByVal tx As IDbTransaction) As CAudit_SqlList
        Return CType(MyBase.SelectWhere(columnName, columnValue, tx), CAudit_SqlList)
    End Function
    Public Shadows Function SelectWhere(ByVal columnName As String, sign As ESign, columnValue As Object, ByVal tx As IDbTransaction) As CAudit_SqlList
        Return CType(MyBase.SelectWhere(columnName, sign, columnValue, tx), CAudit_SqlList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteria, ByVal tx As IDbTransaction) As CAudit_SqlList
        Return CType(MyBase.SelectWhere(criteria, tx), CAudit_SqlList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteriaList, ByVal tx As IDbTransaction) As CAudit_SqlList
        Return CType(MyBase.SelectWhere(criteria, tx), CAudit_SqlList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteriaList, ByVal tableOrJoin As String, ByVal tx As IDbTransaction) As CAudit_SqlList
        Return CType(MyBase.SelectWhere(criteria, tableOrJoin, tx), CAudit_SqlList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteriaList, ByVal tableOrJoin As String, ByVal orderBy As String, ByVal tx As IDbTransaction) As CAudit_SqlList
        Return CType(MyBase.SelectWhere(criteria, tableOrJoin, orderBy, tx), CAudit_SqlList)
    End Function
    Public Shadows Function SelectById(ByVal sqlId As Integer, ByVal tx As IDbTransaction) As CAudit_SqlList
        Return CType(MyBase.SelectById(sqlId, tx), CAudit_SqlList)
    End Function
    Public Shadows Function SelectByIds(ByVal ids As List(Of Integer), ByVal tx As IDbTransaction) As CAudit_SqlList
        Return CType(MyBase.SelectByIds(ids, tx), CAudit_SqlList)
    End Function

    'Select Queries - Stored Procedures
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal txOrNull As IDbTransaction) As CAudit_SqlList
        Return CType(MyBase.MakeList(storedProcName, txOrNull), CAudit_SqlList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal params As Object(), ByVal txOrNull As IDbTransaction) As CAudit_SqlList
        Return CType(MyBase.MakeList(storedProcName, params, txOrNull), CAudit_SqlList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal params As CNameValueList, ByVal txOrNull As IDbTransaction) As CAudit_SqlList
        Return CType(MyBase.MakeList(storedProcName, params, txOrNull), CAudit_SqlList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal params As List(Of Object), ByVal txOrNull As IDbTransaction) As CAudit_SqlList
        Return CType(MyBase.MakeList(storedProcName, params, txOrNull), CAudit_SqlList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal param1 As Integer, ByVal txOrNull As IDbTransaction) As CAudit_SqlList
        Return CType(MyBase.MakeList(storedProcName, param1, txOrNull), CAudit_SqlList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal param1 As String, ByVal txOrNull As IDbTransaction) As CAudit_SqlList
        Return CType(MyBase.MakeList(storedProcName, param1, txOrNull), CAudit_SqlList)
    End Function

    'Query Results
    Public Overloads Function MakeList(ByVal ds As DataSet) As CAudit_SqlList
        Return CType(MyBase.MakeList(ds), CAudit_SqlList)
    End Function
    Public Overloads Function MakeList(ByVal dt As DataTable) As CAudit_SqlList
        Return CType(MyBase.MakeList(dt), CAudit_SqlList)
    End Function
    Public Overloads Function MakeList(ByVal rows As DataRowCollection) As CAudit_SqlList
        Return CType(MyBase.MakeList(rows), CAudit_SqlList)
    End Function
    Public Overloads Function MakeList(ByVal dr As IDataReader) As CAudit_SqlList
        Return CType(MyBase.MakeList(dr), CAudit_SqlList)
    End Function
    Public Overloads Function MakeList(ByVal drOrDs As Object) As CAudit_SqlList
        Return CType(MyBase.MakeList(drOrDs), CAudit_SqlList)
    End Function
    Public Overloads Function MakeList(ByVal gzip As Byte()) As CAudit_SqlList
        Return CType(MyBase.MakeList(gzip), CAudit_SqlList)
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
        Store(w, "SqlId", Me.SqlId)
        Store(w, "SqlText", Me.SqlText)
        Store(w, "SqlCreated", Me.SqlCreated)
    End Sub
#End Region



End Class