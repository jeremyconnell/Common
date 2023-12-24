Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic

Imports Framework

<Serializable(), CLSCompliant(True)> _
Partial Public Class CAudit_Trail
    Inherits CBaseDynamic
    Implements IComparable(Of CAudit_Trail)

#Region "Constructors"
    'Public (Copy Constructor)
    Public Sub New(ByVal original as CAudit_Trail, target As CDataSrc)
        m_dataSrc = target
        m_auditTypeId = original.AuditTypeId
        m_auditDate = original.AuditDate
        m_auditUserLoginName = original.AuditUserLoginName
        m_auditUrl = original.AuditUrl
        m_auditUrlNoQuerystring = original.AuditUrlNoQuerystring
        m_auditDataTableName = original.AuditDataTableName
        m_auditDataPrimaryKey = original.AuditDataPrimaryKey
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
        m_auditId = Integer.MinValue
        m_auditTypeId = Integer.MinValue
        m_auditDate = DateTime.MinValue
        m_auditUserLoginName = String.Empty
        m_auditUrl = String.Empty
        m_auditUrlNoQuerystring = String.Empty
        m_auditDataTableName = String.Empty
        m_auditDataPrimaryKey = String.Empty
    End Sub
#End Region

#Region "Members"
    Protected m_auditId As Integer
    Protected m_auditTypeId As Integer
    Protected m_auditDate As DateTime
    Protected m_auditUserLoginName As String
    Protected m_auditUrl As String
    Protected m_auditUrlNoQuerystring As String
    Protected m_auditDataTableName As String
    Protected m_auditDataPrimaryKey As String
#End Region

#Region "Properties - Column Values"
    'Primary Key Column (ReadOnly)
    Public ReadOnly Property [AuditId]() As Integer
        Get
            Return m_auditId
        End Get
    End Property

    'Table Columns (Read/Write)
    Public Property [AuditDate]() As DateTime
        Get
            Return m_auditDate
        End Get
        Set(ByVal value As DateTime)
            m_auditDate = value
        End Set
    End Property
    Public Property [AuditUserLoginName]() As String
        Get
            Return m_auditUserLoginName
        End Get
        Set(ByVal value As String)
            m_auditUserLoginName = value
        End Set
    End Property
    Public Property [AuditUrl]() As String
        Get
            Return m_auditUrl
        End Get
        Set(ByVal value As String)
            m_auditUrl = value
        End Set
    End Property
    Public Property [AuditUrlNoQuerystring]() As String
        Get
            Return m_auditUrlNoQuerystring
        End Get
        Set(ByVal value As String)
            m_auditUrlNoQuerystring = value
        End Set
    End Property
    Public Property [AuditDataTableName]() As String
        Get
            Return m_auditDataTableName
        End Get
        Set(ByVal value As String)
            m_auditDataTableName = value
        End Set
    End Property
    Public Property [AuditDataPrimaryKey]() As String
        Get
            Return m_auditDataPrimaryKey
        End Get
        Set(ByVal value As String)
            m_auditDataPrimaryKey = value
        End Set
    End Property

    'View Columns (ReadOnly)

#End Region

#Region "MustOverride Methods"
    'Schema Information
    Public Const TABLE_NAME As String = "tblAudit_Trail"
    Public Const VIEW_NAME As String  = ""          'Used to override Me.ViewName
    Public Const ORDER_BY_COLS As String = "AuditId DESC" 'See the CompareTo method below (Sql-based sorting should match In-Memory sorting)
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
    Public Function CompareTo(other As CAudit_Trail) As Integer Implements IComparable(Of CAudit_Trail).CompareTo
        Return Me.AuditId.CompareTo(other.AuditId) *-1
    End Function

    'Primary Key Information
    Public Const PRIMARY_KEY_NAME As String = "AuditId"
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
            Return m_auditId
        End Get
        Set(ByVal value As Object)
            m_auditId = CType(Value, Integer)
        End Set
    End Property

    'Factory Methods - Object
    Protected Overrides Function MakeFrom(ByVal row As DataRow) As CBase
        Return New CAudit_Trail(Me.DataSrc, row)
    End Function
    Protected Overrides Function MakeFrom(ByVal dr As IDataReader) As CBase
        Return New CAudit_Trail(Me.DataSrc, dr)
    End Function
    
    'Factory Methods - List
    Protected Overrides Function MakeList() As IList
        Return New CAudit_TrailList
    End Function
    Protected Overrides Function MakeList(ByVal capacity As Integer) As IList
        Return New CAudit_TrailList(capacity)
    End Function

    'Convert from ADO to .Net
    Protected Overrides Sub ReadColumns(ByVal dr As IDataReader)
        m_auditId = CAdoData.GetInt(dr, "AuditId")
        m_auditTypeId = CAdoData.GetInt(dr, "AuditTypeId")
        m_auditDate = CAdoData.GetDate(dr, "AuditDate")
        m_auditUserLoginName = CAdoData.GetStr(dr, "AuditUserLoginName")
        m_auditUrl = CAdoData.GetStr(dr, "AuditUrl")
        m_auditUrlNoQuerystring = CAdoData.GetStr(dr, "AuditUrlNoQuerystring")
        m_auditDataTableName = CAdoData.GetStr(dr, "AuditDataTableName")
        m_auditDataPrimaryKey = CAdoData.GetStr(dr, "AuditDataPrimaryKey")
    End Sub
    Protected Overrides Sub ReadColumns(ByVal dr As DataRow)
        m_auditId = CAdoData.GetInt(dr, "AuditId")
        m_auditTypeId = CAdoData.GetInt(dr, "AuditTypeId")
        m_auditDate = CAdoData.GetDate(dr, "AuditDate")
        m_auditUserLoginName = CAdoData.GetStr(dr, "AuditUserLoginName")
        m_auditUrl = CAdoData.GetStr(dr, "AuditUrl")
        m_auditUrlNoQuerystring = CAdoData.GetStr(dr, "AuditUrlNoQuerystring")
        m_auditDataTableName = CAdoData.GetStr(dr, "AuditDataTableName")
        m_auditDataPrimaryKey = CAdoData.GetStr(dr, "AuditDataPrimaryKey")
    End Sub

    'Parameters for Insert/Update    
    Protected Overrides Function ColumnNameValues() As CNameValueList
        Dim data As New CNameValueList
        data.Add("AuditId", NullVal(m_auditId))
        data.Add("AuditTypeId", NullVal(m_auditTypeId))
        data.Add("AuditDate", NullVal(m_auditDate))
        data.Add("AuditUserLoginName", NullVal(m_auditUserLoginName))
        data.Add("AuditUrl", NullVal(m_auditUrl))
        data.Add("AuditUrlNoQuerystring", NullVal(m_auditUrlNoQuerystring))
        data.Add("AuditDataTableName", NullVal(m_auditDataTableName))
        data.Add("AuditDataPrimaryKey", NullVal(m_auditDataPrimaryKey))
        Return data
    End Function
#End Region

#Region "Queries - SelectAll/SelectWhere (inherited methods, cast only)"
    'Select Queries - Non-Paged
    Public Shadows Function SelectAll() As CAudit_TrailList
        Return CType(MyBase.SelectAll(), CAudit_TrailList)
    End Function
    Public Shadows Function SelectAll(ByVal orderBy As String) As CAudit_TrailList
        Return CType(MyBase.SelectAll(orderBy), CAudit_TrailList)
    End Function
    Public Shadows Function SelectWhere(ByVal colName As String, ByVal sign As ESign, ByVal colValue As Object) As CAudit_TrailList
        Return CType(MyBase.SelectWhere(colName, sign, colValue), CAudit_TrailList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteria) As CAudit_TrailList
        Return CType(MyBase.SelectWhere(where), CAudit_TrailList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteriaList) As CAudit_TrailList
        Return CType(MyBase.SelectWhere(where), CAudit_TrailList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteriaList, ByVal tableNameOrJoinExpression As String) As CAudit_TrailList
        Return CType(MyBase.SelectWhere(where, tableNameOrJoinExpression, Me.OrderByColumns), CAudit_TrailList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteriaList, ByVal tableNameOrJoinExpression As String, ByVal orderBy As String) As CAudit_TrailList
        Return CType(MyBase.SelectWhere(where, tableNameOrJoinExpression, orderBy), CAudit_TrailList)
    End Function
    <Obsolete("Arbitrary where-clause is not necessary portable or safe from sql injection attacks. Consider using the parameterised query interfaces")> _
    Public Shadows Function SelectWhere(ByVal unsafeWhereClause As String) As CAudit_TrailList
        Return CType(MyBase.SelectWhere(unsafeWhereClause), CAudit_TrailList)
    End Function
    Public Shadows Function SelectById(ByVal auditId As Integer) As CAudit_TrailList
        Return CType(MyBase.SelectById(auditId), CAudit_TrailList)
    End Function
    Public Shadows Function SelectByIds(ByVal ids As List(Of Integer)) As CAudit_TrailList
        Return CType(MyBase.SelectByIds(ids), CAudit_TrailList)
    End Function
    
    'Select Queries - Paged
    Public Shadows Function SelectAll(ByVal pi As CPagingInfo) As CAudit_TrailList
        Return CType(MyBase.SelectAll(pi), CAudit_TrailList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal colName As String, ByVal sign As ESign, ByVal colValue As Object) As CAudit_TrailList
        Return CType(MyBase.SelectWhere(pi, colName, sign, colValue), CAudit_TrailList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal criteria As CCriteria) As CAudit_TrailList
        Return CType(MyBase.SelectWhere(pi, criteria), CAudit_TrailList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal criteria As CCriteriaList) As CAudit_TrailList
        Return CType(MyBase.SelectWhere(pi, criteria), CAudit_TrailList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal criteria As CCriteriaList, ByVal tableViewOrJoinExpr As String) As CAudit_TrailList
        Return CType(MyBase.SelectWhere(pi, criteria, tableViewOrJoinExpr), CAudit_TrailList)
    End Function
    Public Shadows Function SelectByIds(ByVal pi As CPagingInfo, ByVal ids As List(Of Integer)) As CAudit_TrailList
        Return CType(MyBase.SelectByIds(pi, ids), CAudit_TrailList)
    End Function

    'Select Queries - Transactional
    Public Shadows Function SelectAll(ByVal tx As IDbTransaction) As CAudit_TrailList
        Return CType(MyBase.SelectAll(tx), CAudit_TrailList)
    End Function
    Public Shadows Function SelectAll(ByVal orderBy As String, ByVal tx As IDbTransaction) As CAudit_TrailList
        Return CType(MyBase.SelectAll(orderBy, tx), CAudit_TrailList)
    End Function
    Public Shadows Function SelectWhere(ByVal columnName As String, columnValue As Object, ByVal tx As IDbTransaction) As CAudit_TrailList
        Return CType(MyBase.SelectWhere(columnName, columnValue, tx), CAudit_TrailList)
    End Function
    Public Shadows Function SelectWhere(ByVal columnName As String, sign As ESign, columnValue As Object, ByVal tx As IDbTransaction) As CAudit_TrailList
        Return CType(MyBase.SelectWhere(columnName, sign, columnValue, tx), CAudit_TrailList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteria, ByVal tx As IDbTransaction) As CAudit_TrailList
        Return CType(MyBase.SelectWhere(criteria, tx), CAudit_TrailList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteriaList, ByVal tx As IDbTransaction) As CAudit_TrailList
        Return CType(MyBase.SelectWhere(criteria, tx), CAudit_TrailList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteriaList, ByVal tableOrJoin As String, ByVal tx As IDbTransaction) As CAudit_TrailList
        Return CType(MyBase.SelectWhere(criteria, tableOrJoin, tx), CAudit_TrailList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteriaList, ByVal tableOrJoin As String, ByVal orderBy As String, ByVal tx As IDbTransaction) As CAudit_TrailList
        Return CType(MyBase.SelectWhere(criteria, tableOrJoin, orderBy, tx), CAudit_TrailList)
    End Function
    Public Shadows Function SelectById(ByVal auditId As Integer, ByVal tx As IDbTransaction) As CAudit_TrailList
        Return CType(MyBase.SelectById(auditId, tx), CAudit_TrailList)
    End Function
    Public Shadows Function SelectByIds(ByVal ids As List(Of Integer), ByVal tx As IDbTransaction) As CAudit_TrailList
        Return CType(MyBase.SelectByIds(ids, tx), CAudit_TrailList)
    End Function
    
    'Select Queries - Stored Procedures
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal txOrNull As IDbTransaction) As CAudit_TrailList
        Return CType(MyBase.MakeList(storedProcName, txOrNull), CAudit_TrailList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal params As Object(), ByVal txOrNull As IDbTransaction) As CAudit_TrailList
        Return CType(MyBase.MakeList(storedProcName, params, txOrNull), CAudit_TrailList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal params As CNameValueList, ByVal txOrNull As IDbTransaction) As CAudit_TrailList
        Return CType(MyBase.MakeList(storedProcName, params, txOrNull), CAudit_TrailList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal params As List(Of Object), ByVal txOrNull As IDbTransaction) As CAudit_TrailList
        Return CType(MyBase.MakeList(storedProcName, params, txOrNull), CAudit_TrailList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal param1 As Integer, ByVal txOrNull As IDbTransaction) As CAudit_TrailList
        Return CType(MyBase.MakeList(storedProcName, param1, txOrNull), CAudit_TrailList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal param1 As String, ByVal txOrNull As IDbTransaction) As CAudit_TrailList
        Return CType(MyBase.MakeList(storedProcName, param1, txOrNull), CAudit_TrailList)
    End Function

    'Query Results
    Protected Overloads Function MakeList(ByVal ds As DataSet) As CAudit_TrailList
        Return CType(MyBase.MakeList(ds), CAudit_TrailList)
    End Function
    Protected Overloads Function MakeList(ByVal dt As DataTable) As CAudit_TrailList
        Return CType(MyBase.MakeList(dt), CAudit_TrailList)
    End Function
    Protected Overloads Function MakeList(ByVal rows As DataRowCollection) As CAudit_TrailList
        Return CType(MyBase.MakeList(rows), CAudit_TrailList)
    End Function
    Protected Overloads Function MakeList(ByVal dr As IDataReader) As CAudit_TrailList
        Return CType(MyBase.MakeList(dr), CAudit_TrailList)
    End Function
    Protected Overloads Function MakeList(ByVal drOrDs As Object) As CAudit_TrailList
        Return CType(MyBase.MakeList(drOrDs), CAudit_TrailList)
    End Function
    Protected Overloads Function MakeList(ByVal gzip As Byte()) As CAudit_TrailList
        Return CType(MyBase.MakeList(gzip), CAudit_TrailList)
    End Function
#End Region

#Region "Queries - SelectBy[FK] (user-nominated fk/bool columns)"
    'Non-Paged
    Public Function SelectByTypeId(ByVal auditTypeId As Integer) As CAudit_TrailList
        Return SelectWhere(new CCriteriaList("AuditTypeId", auditTypeId))
    End Function

    'Paged
    Public Function SelectByTypeId(pi as CPagingInfo, ByVal auditTypeId As Integer) As CAudit_TrailList
        Return SelectWhere(pi, New CCriteriaList("AuditTypeId", auditTypeId))
    End Function

    'Count
    Public Function SelectCountByTypeId(ByVal auditTypeId As Integer) As Integer
        Return SelectCount(New CCriteriaList("AuditTypeId", auditTypeId))
    End Function

    'Transactional
    Public Function SelectByTypeId(ByVal auditTypeId As Integer, tx As IDbTransaction) As CAudit_TrailList
        Return SelectWhere(New CCriteriaList("AuditTypeId", auditTypeId), tx)
    End Function
#End Region

#Region "ToXml"
    Protected Overrides Sub ToXml_Autogenerated(ByVal w As System.Xml.XmlWriter)
        Store(w, "AuditId", Me.AuditId)
        Store(w, "AuditTypeId", Me.AuditTypeId)
        Store(w, "AuditDate", Me.AuditDate)
        Store(w, "AuditUserLoginName", Me.AuditUserLoginName)
        Store(w, "AuditUrl", Me.AuditUrl)
        Store(w, "AuditUrlNoQuerystring", Me.AuditUrlNoQuerystring)
        Store(w, "AuditDataTableName", Me.AuditDataTableName)
        Store(w, "AuditDataPrimaryKey", Me.AuditDataPrimaryKey)
    End Sub
#End Region


End Class