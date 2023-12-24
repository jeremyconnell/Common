Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic

Imports Framework

<Serializable(), CLSCompliant(True)> _
Partial Public Class CUserRole
    Inherits SchemaAudit.CBaseDynamicAuditedM2M
    Implements IComparable(Of CUserRole)

#Region "Constructors"
    'Public (Copy Constructor)
    Public Sub New(ByVal original as CUserRole, target As CDataSrc)
        m_dataSrc = target
        m_uRUserLogin = original.URUserLogin
        m_uRRoleName = original.URRoleName
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
        m_uRUserLogin = String.Empty
        m_uRRoleName = String.Empty
    End Sub
#End Region

#Region "Members"
    Protected m_uRUserLogin As String
    Protected m_uRRoleName As String
#End Region

#Region "Properties - Column Values"
    'Primary Key Columns
    Public Property [URUserLogin]() As String
        Get
            Return m_uRUserLogin
        End Get
        Set(ByVal value As String)
            m_uRUserLogin = value
        End Set
    End Property
    Public Property [URRoleName]() As String
        Get
            Return m_uRRoleName
        End Get
        Set(ByVal value As String)
            m_uRRoleName = value
        End Set
    End Property

    'Table Columns

    'View Columns

#End Region

#Region "MustOverride Methods"
    'Schema Information
    Public Const TABLE_NAME As String = "tblMembership_UserRole"
    Public Const VIEW_NAME As String  = ""          'Used to override Me.ViewName
    Public Const ORDER_BY_COLS As String = "URUserLogin,URRoleName" 'See the CompareTo method below (Sql-based sorting should match In-Memory sorting)
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
    Public Function CompareTo(ByVal other As CUserRole) As Integer Implements IComparable(Of CUserRole).CompareTo
        Dim i As Integer = Me.URUserLogin.CompareTo(other.URUserLogin) 
        If 0 <> i Then Return i
        Return Me.URRoleName.CompareTo(other.URRoleName) 
    End Function

    'Primary Key Information
    Protected Overrides ReadOnly Property PrimaryKeyName() As String
        Get
            Return "URUserLogin"
        End Get
    End Property
    Protected Overrides ReadOnly Property SecondaryKeyName() As String
        Get
            Return "URRoleName"
        End Get
    End Property
    Protected Overrides Property PrimaryKeyValue() As Object
        Get
            Return m_uRUserLogin
        End Get
        Set(ByVal value As Object)
            If Not m_insertPending Then 'Note: Use cascade update for relationships
                DataSrc.Update(New CNameValueList("URUserLogin", value), New CWhere(TABLE_NAME, New CCriteria("URUserLogin", Me.URUserLogin), Nothing))
                CacheClear()
            End If
            m_uRUserLogin = CType(Value, String)
        End Set
    End Property
    Protected Overrides Property SecondaryKeyValue() As Object
        Get
            Return m_uRRoleName
        End Get
        Set(ByVal value As Object)
            If Not m_insertPending Then 'Note: Use cascade update for relationships
                DataSrc.Update(New CNameValueList("URRoleName", value), New CWhere(TABLE_NAME, New CCriteria("URRoleName", Me.URRoleName), Nothing))
                CacheClear()
            End If
            m_uRRoleName = CType(Value, String)
        End Set
    End Property

    'Factory Methods - Object
    Protected Overrides Function MakeFrom(ByVal row As DataRow) As CBase
        Return New CUserRole(Me.DataSrc, row)
    End Function
    Protected Overrides Function MakeFrom(ByVal dr As IDataReader) As CBase
        Return New CUserRole(Me.DataSrc, dr)
    End Function
    
    'Factory Methods - List
    Protected Overrides Function MakeList() As IList
        Return New CUserRoleList
    End Function
    Protected Overrides Function MakeList(ByVal capacity As Integer) As IList
        Return New CUserRoleList(capacity)
    End Function

    'Convert from ADO to .Net
    Protected Overrides Sub ReadColumns(ByVal dr As IDataReader)
        m_uRUserLogin = CAdoData.GetStr(dr, "URUserLogin")
        m_uRRoleName = CAdoData.GetStr(dr, "URRoleName")
    End Sub
    Protected Overrides Sub ReadColumns(ByVal dr As DataRow)
        m_uRUserLogin = CAdoData.GetStr(dr, "URUserLogin")
        m_uRRoleName = CAdoData.GetStr(dr, "URRoleName")
    End Sub

    'Parameters for Insert/Update    
    Protected Overrides Function ColumnNameValues() As CNameValueList
        Dim data As New CNameValueList
        data.Add("URUserLogin", NullVal(m_uRUserLogin))
        data.Add("URRoleName", NullVal(m_uRRoleName))
        Return data
    End Function
#End Region

#Region "Queries - SelectAll/SelectWhere (inherited methods, cast only)"
    'Select Queries - Non-Paged
    Public Shadows Function SelectAll() As CUserRoleList
        Return CType(MyBase.SelectAll(), CUserRoleList)
    End Function
    Public Shadows Function SelectAll(ByVal orderBy As String) As CUserRoleList
        Return CType(MyBase.SelectAll(orderBy), CUserRoleList)
    End Function
    Public Shadows Function SelectWhere(ByVal colName As String, ByVal sign As ESign, ByVal colValue As Object) As CUserRoleList
        Return CType(MyBase.SelectWhere(colName, sign, colValue), CUserRoleList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteria) As CUserRoleList
        Return CType(MyBase.SelectWhere(where), CUserRoleList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteriaList) As CUserRoleList
        Return CType(MyBase.SelectWhere(where), CUserRoleList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteriaList, ByVal tableNameOrJoinExpression As String) As CUserRoleList
        Return CType(MyBase.SelectWhere(where, tableNameOrJoinExpression, Me.OrderByColumns), CUserRoleList)
    End Function
    Public Shadows Function SelectWhere(ByVal where As CCriteriaList, ByVal tableNameOrJoinExpression As String, ByVal orderBy As String) As CUserRoleList
        Return CType(MyBase.SelectWhere(where, tableNameOrJoinExpression, orderBy), CUserRoleList)
    End Function
    <Obsolete("Arbitrary where-clause is not necessary portable or safe from sql injection attacks. Consider using the parameterised query interfaces")> _
    Public Shadows Function SelectWhere(ByVal unsafeWhereClause As String) As CUserRoleList
        Return CType(MyBase.SelectWhere(unsafeWhereClause), CUserRoleList)
    End Function
    Public Shadows Function SelectById(ByVal uRUserLogin As String, ByVal uRRoleName As String) As CUserRoleList
        Return CType(MyBase.SelectById(uRUserLogin, uRRoleName), CUserRoleList)
    End Function
    Public Shadows Function SelectByIds(ByVal ids As List(Of String)) As CUserRoleList
        Return CType(MyBase.SelectByIds(ids), CUserRoleList)
    End Function
    
    'Select Queries - Paged
    Public Shadows Function SelectAll(ByVal pi As CPagingInfo) As CUserRoleList
        Return CType(MyBase.SelectAll(pi), CUserRoleList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal colName As String, ByVal sign As ESign, ByVal colValue As Object) As CUserRoleList
        Return CType(MyBase.SelectWhere(pi, colName, sign, colValue), CUserRoleList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal criteria As CCriteria) As CUserRoleList
        Return CType(MyBase.SelectWhere(pi, criteria), CUserRoleList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal criteria As CCriteriaList) As CUserRoleList
        Return CType(MyBase.SelectWhere(pi, criteria), CUserRoleList)
    End Function
    Public Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal criteria As CCriteriaList, ByVal tableViewOrJoinExpr As String) As CUserRoleList
        Return CType(MyBase.SelectWhere(pi, criteria, tableViewOrJoinExpr), CUserRoleList)
    End Function
    Public Shadows Function SelectByIds(ByVal pi As CPagingInfo, ByVal ids As List(Of String)) As CUserRoleList
        Return CType(MyBase.SelectByIds(pi, ids), CUserRoleList)
    End Function

    'Select Queries - Transactional
    Public Shadows Function SelectAll(ByVal tx As IDbTransaction) As CUserRoleList
        Return CType(MyBase.SelectAll(tx), CUserRoleList)
    End Function
    Public Shadows Function SelectAll(ByVal orderBy As String, ByVal tx As IDbTransaction) As CUserRoleList
        Return CType(MyBase.SelectAll(orderBy, tx), CUserRoleList)
    End Function
    Public Shadows Function SelectWhere(ByVal columnName As String, columnValue As Object, ByVal tx As IDbTransaction) As CUserRoleList
        Return CType(MyBase.SelectWhere(columnName, columnValue, tx), CUserRoleList)
    End Function
    Public Shadows Function SelectWhere(ByVal columnName As String, sign As ESign, columnValue As Object, ByVal tx As IDbTransaction) As CUserRoleList
        Return CType(MyBase.SelectWhere(columnName, sign, columnValue, tx), CUserRoleList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteria, ByVal tx As IDbTransaction) As CUserRoleList
        Return CType(MyBase.SelectWhere(criteria, tx), CUserRoleList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteriaList, ByVal tx As IDbTransaction) As CUserRoleList
        Return CType(MyBase.SelectWhere(criteria, tx), CUserRoleList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteriaList, ByVal tableOrJoin As String, ByVal tx As IDbTransaction) As CUserRoleList
        Return CType(MyBase.SelectWhere(criteria, tableOrJoin, tx), CUserRoleList)
    End Function
    Public Shadows Function SelectWhere(ByVal criteria As CCriteriaList, ByVal tableOrJoin As String, ByVal orderBy As String, ByVal tx As IDbTransaction) As CUserRoleList
        Return CType(MyBase.SelectWhere(criteria, tableOrJoin, orderBy, tx), CUserRoleList)
    End Function
    Public Shadows Function SelectById(ByVal uRUserLogin As String, ByVal uRRoleName As String, ByVal tx As IDbTransaction) As CUserRoleList
        Return CType(MyBase.SelectById(uRUserLogin, uRRoleName, tx), CUserRoleList)
    End Function
    Public Shadows Function SelectByIds(ByVal ids As List(Of String), ByVal tx As IDbTransaction) As CUserRoleList
        Return CType(MyBase.SelectByIds(ids, tx), CUserRoleList)
    End Function
    
    'Select Queries - Stored Procedures
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal txOrNull As IDbTransaction) As CUserRoleList
        Return CType(MyBase.MakeList(storedProcName, txOrNull), CUserRoleList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal params As Object(), ByVal txOrNull As IDbTransaction) As CUserRoleList
        Return CType(MyBase.MakeList(storedProcName, params, txOrNull), CUserRoleList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal params As CNameValueList, ByVal txOrNull As IDbTransaction) As CUserRoleList
        Return CType(MyBase.MakeList(storedProcName, params, txOrNull), CUserRoleList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal params As List(Of Object), ByVal txOrNull As IDbTransaction) As CUserRoleList
        Return CType(MyBase.MakeList(storedProcName, params, txOrNull), CUserRoleList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal param1 As Integer, ByVal txOrNull As IDbTransaction) As CUserRoleList
        Return CType(MyBase.MakeList(storedProcName, param1, txOrNull), CUserRoleList)
    End Function
    Public Overloads Function MakeList(ByVal storedProcName As String, ByVal param1 As String, ByVal txOrNull As IDbTransaction) As CUserRoleList
        Return CType(MyBase.MakeList(storedProcName, param1, txOrNull), CUserRoleList)
    End Function

    'Query Results
    Protected Overloads Function MakeList(ByVal ds As DataSet) As CUserRoleList
        Return CType(MyBase.MakeList(ds), CUserRoleList)
    End Function
    Protected Overloads Function MakeList(ByVal dt As DataTable) As CUserRoleList
        Return CType(MyBase.MakeList(dt), CUserRoleList)
    End Function
    Protected Overloads Function MakeList(ByVal rows As DataRowCollection) As CUserRoleList
        Return CType(MyBase.MakeList(rows), CUserRoleList)
    End Function
    Protected Overloads Function MakeList(ByVal dr As IDataReader) As CUserRoleList
        Return CType(MyBase.MakeList(dr), CUserRoleList)
    End Function
    Protected Overloads Function MakeList(ByVal drOrDs As Object) As CUserRoleList
        Return CType(MyBase.MakeList(drOrDs), CUserRoleList)
    End Function
    Protected Overloads Function MakeList(ByVal gzip As Byte()) As CUserRoleList
        Return CType(MyBase.MakeList(gzip), CUserRoleList)
    End Function
#End Region

#Region "Queries - SelectBy[FK] (user-nominated fk/bool columns)"
    'Non-Paged
    Public Function SelectByUserLogin(ByVal uRUserLogin As String) As CUserRoleList
        Return SelectWhere(new CCriteriaList("URUserLogin", uRUserLogin))
    End Function
    Public Function SelectByRoleName(ByVal uRRoleName As String) As CUserRoleList
        Return SelectWhere(new CCriteriaList("URRoleName", uRRoleName))
    End Function

    'Paged
    Public Function SelectByUserLogin(pi as CPagingInfo, ByVal uRUserLogin As String) As CUserRoleList
        Return SelectWhere(pi, New CCriteriaList("URUserLogin", uRUserLogin))
    End Function
    Public Function SelectByRoleName(pi as CPagingInfo, ByVal uRRoleName As String) As CUserRoleList
        Return SelectWhere(pi, New CCriteriaList("URRoleName", uRRoleName))
    End Function

    'Count
    Public Function SelectCountByUserLogin(ByVal uRUserLogin As String) As Integer
        Return SelectCount(New CCriteriaList("URUserLogin", uRUserLogin))
    End Function
    Public Function SelectCountByRoleName(ByVal uRRoleName As String) As Integer
        Return SelectCount(New CCriteriaList("URRoleName", uRRoleName))
    End Function

    'Transactional
    Public Function SelectByUserLogin(ByVal uRUserLogin As String, tx As IDbTransaction) As CUserRoleList
        Return SelectWhere(New CCriteriaList("URUserLogin", uRUserLogin), tx)
    End Function
    Public Function SelectByRoleName(ByVal uRRoleName As String, tx As IDbTransaction) As CUserRoleList
        Return SelectWhere(New CCriteriaList("URRoleName", uRRoleName), tx)
    End Function
#End Region

#Region "Many-To-Many Helper Functions"
    'Insert/Delete - NonTransactional
    Public Shared Function InsertPair(ByVal uRUserLogin As String, ByVal uRRoleName As String) As Integer
        Return InsertPair(uRUserLogin, uRRoleName, Nothing)
    End Function
    Public Shared Function DeletePair(ByVal uRUserLogin As String, ByVal uRRoleName As String) As Integer
        Return DeletePair(uRUserLogin, uRRoleName, Nothing)
    End Function
    Public Shared Function ExistsPair(ByVal uRUserLogin As String, ByVal uRRoleName As String) As Boolean
        Return ExistsPair(uRUserLogin, uRRoleName, Nothing)
    End Function
    
    'Insert/Delete - Default Datasrc
    Public Shared Function InsertPair(ByVal uRUserLogin As String, ByVal uRRoleName As String, ByVal txOrNull As IDbTransaction) As Integer
        Return InsertPair(uRUserLogin, uRRoleName, txOrNull, CDataSrc.Default)
    End Function
    Public Shared Function DeletePair(ByVal uRUserLogin As String, ByVal uRRoleName As String, ByVal txOrNull As IDbTransaction) As Integer
        Return DeletePair(uRUserLogin, uRRoleName, txOrNull, CDataSrc.Default)
    End Function
    Public Shared Function ExistsPair(ByVal uRUserLogin As String, ByVal uRRoleName As String, ByVal txOrNull As IDbTransaction) As Boolean
        Return ExistsPair(uRUserLogin, uRRoleName, txOrNull, CDataSrc.Default)
    End Function

    'Insert/Delete - Transactional
    Public Shared Function InsertPair(ByVal uRUserLogin As String, ByVal uRRoleName As String, ByVal txOrNull As IDbTransaction, ByVal dataSrc as CDatasrc) As Integer
        With New CUserRole(dataSrc)
            .URUserLogin = uRUserLogin
            .URRoleName = uRRoleName
            .Save(txOrNull)
        End With
    End Function
    Public Shared Function DeletePair(ByVal uRUserLogin As String, ByVal uRRoleName As String, ByVal txOrNull As IDbTransaction, ByVal dataSrc as CDatasrc) As Integer
        Dim where As New CCriteriaList()
        where.Add("URUserLogin", uRUserLogin)
        where.Add("URRoleName", uRRoleName)

        'Faster, but not audited
        'Return New CUserRole(dataSrc).DeleteWhere(where, txOrNull)
        
        Dim list As CUserRoleList = New CUserRole(dataSrc).SelectWhere(where, txOrNull)
        list.DeleteAll(txOrNull)
        return list.Count
    End Function
    Public Shared Function ExistsPair(ByVal uRUserLogin As String, ByVal uRRoleName As String, ByVal txOrNull As IDbTransaction, ByVal dataSrc as CDatasrc) As Boolean
        With New CUserRole(dataSrc)
            Return .SelectById(uRUserLogin, uRRoleName, txOrNull).Count > 0
        End With
    End Function
#End Region

#Region "ToXml"
    Protected Overrides Sub ToXml_Autogenerated(ByVal w As System.Xml.XmlWriter)
        Store(w, "URUserLogin", Me.URUserLogin)
        Store(w, "URRoleName", Me.URRoleName)
    End Sub
#End Region

#Region "Audit Trail"
    Protected Overrides Function OriginalState(ByVal txOrNull As IDbTransaction) As SchemaAudit.CBaseDynamicAuditedM2M
        Return New CUserRole(Me.DataSrc, Me.URUserLogin, Me.URRoleName, txOrNull)
    End Function
#End Region

End Class
