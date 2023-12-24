Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic

Imports Framework

<Serializable(), CLSCompliant(True)> _
Partial Public Class CRole
    Inherits SchemaAudit.CBaseDynamicAudited
    Implements IComparable(Of CRole)

#Region "Constructors"
    'Public (Copy Constructor)
    Public Sub New(ByVal original as CRole, target As CDataSrc)
        m_dataSrc = target
        m_roleName = original.RoleName
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
        m_roleName = String.Empty
    End Sub
#End Region

#Region "Members"
    Protected m_roleName As String
#End Region

#Region "Properties - Column Values"
    'Primary Key Column (ReadOnly)
    Public Property [RoleName]() As String
        Get
            Return m_roleName
        End Get
        Set(ByVal value As String)
            If Not m_insertPending Then
                DataSrc.Update(New CNameValueList("RoleName", value), New CWhere(TABLE_NAME, New CCriteria("RoleName", m_roleName), Nothing))
            End If
            m_roleName = value
            CacheClear()
        End Set
    End Property

    'Table Columns (Read/Write)

    'View Columns (ReadOnly)

#End Region

#Region "MustOverride Methods"
    'Schema Information
    Public Const TABLE_NAME As String = "tblMembership_Role"
    Public Const VIEW_NAME As String  = ""          'Used to override Me.ViewName
    Public Const ORDER_BY_COLS As String = "RoleName" 'See the CompareTo method below (Sql-based sorting should match In-Memory sorting)
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
    Public Function CompareTo(other As CRole) As Integer Implements IComparable(Of CRole).CompareTo
        Return Me.RoleName.CompareTo(other.RoleName) 
    End Function

    'Primary Key Information
    Public Const PRIMARY_KEY_NAME As String = "RoleName"
    Protected Overrides ReadOnly Property InsertPrimaryKey() As Boolean
        Get
            Return True
        End Get
    End Property
    Protected Overrides ReadOnly Property PrimaryKeyName() As String
        Get
            Return PRIMARY_KEY_NAME
        End Get
    End Property
    Protected Overrides Property PrimaryKeyValue() As Object
        Get
            Return m_roleName
        End Get
        Set(ByVal value As Object)
            If Not m_insertPending Then 'Note: Use cascade update for relationships
                DataSrc.Update(New CNameValueList("RoleName", value), New CWhere(TABLE_NAME, New CCriteria("RoleName", Me.RoleName), Nothing))
                CacheClear()
            End If
            m_roleName = CType(value, String)
        End Set
    End Property

    'Factory Methods - Object
    Protected Overrides Function MakeFrom(ByVal row As DataRow) As CBase
        Return New CRole(Me.DataSrc, row)
    End Function
    Protected Overrides Function MakeFrom(ByVal dr As IDataReader) As CBase
        Return New CRole(Me.DataSrc, dr)
    End Function
    
    'Factory Methods - List
    Protected Overrides Function MakeList() As IList
        Return New CRoleList
    End Function
    Protected Overrides Function MakeList(ByVal capacity As Integer) As IList
        Return New CRoleList(capacity)
    End Function

    'Convert from ADO to .Net
    Protected Overrides Sub ReadColumns(ByVal dr As IDataReader)
        m_roleName = CAdoData.GetStr(dr, "RoleName")
    End Sub
    Protected Overrides Sub ReadColumns(ByVal dr As DataRow)
        m_roleName = CAdoData.GetStr(dr, "RoleName")
    End Sub

    'Parameters for Insert/Update    
    Protected Overrides Function ColumnNameValues() As CNameValueList
        Dim data As New CNameValueList
        data.Add("RoleName", NullVal(m_roleName))
        Return data
    End Function
#End Region

#Region "Queries - SelectAll/SelectWhere (inherited methods, cast only)"
    'Select Queries - Non-Paged
    Public Shadows Function SelectAll() As CRoleList
        Return CType(MyBase.SelectAll(), CRoleList)
    End Function
    Public Shadows Function SelectAll(ByVal orderBy As String) As CRoleList
        Return CType(MyBase.SelectAll(orderBy), CRoleList)
    End Function
    Protected Shadows Function SelectWhere(ByVal colName As String, ByVal sign As ESign, ByVal colValue As Object) As CRoleList
        Return CType(MyBase.SelectWhere(colName, sign, colValue), CRoleList)
    End Function
    Protected Shadows Function SelectWhere(ByVal where As CCriteria) As CRoleList
        Return CType(MyBase.SelectWhere(where), CRoleList)
    End Function
    Protected Shadows Function SelectWhere(ByVal where As CCriteriaList) As CRoleList
        Return CType(MyBase.SelectWhere(where), CRoleList)
    End Function
    Protected Shadows Function SelectWhere(ByVal where As CCriteriaList, ByVal tableNameOrJoinExpression As String) As CRoleList
        Return CType(MyBase.SelectWhere(where, tableNameOrJoinExpression, Me.OrderByColumns), CRoleList)
    End Function
    Protected Shadows Function SelectWhere(ByVal where As CCriteriaList, ByVal tableNameOrJoinExpression As String, ByVal orderBy As String) As CRoleList
        Return CType(MyBase.SelectWhere(where, tableNameOrJoinExpression, orderBy), CRoleList)
    End Function
    <Obsolete("Arbitrary where-clause is not necessary portable or safe from sql injection attacks. Consider using the parameterised query interfaces")> _
    Protected Shadows Function SelectWhere(ByVal unsafeWhereClause As String) As CRoleList
        Return CType(MyBase.SelectWhere(unsafeWhereClause), CRoleList)
    End Function
    Protected Shadows Function SelectById(ByVal roleName As String) As CRoleList
        Return CType(MyBase.SelectById(roleName), CRoleList)
    End Function
    Protected Shadows Function SelectByIds(ByVal ids As List(Of String)) As CRoleList
        Return CType(MyBase.SelectByIds(ids), CRoleList)
    End Function

    'Select Queries - Paged
    Public Shadows Function SelectAll(ByVal pi As CPagingInfo) As CRoleList
        Return CType(MyBase.SelectAll(pi), CRoleList)
    End Function
    Protected Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal colName As String, ByVal sign As ESign, ByVal colValue As Object) As CRoleList
        Return CType(MyBase.SelectWhere(pi, colName, sign, colValue), CRoleList)
    End Function
    Protected Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal criteria As CCriteria) As CRoleList
        Return CType(MyBase.SelectWhere(pi, criteria), CRoleList)
    End Function
    Protected Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal criteria As CCriteriaList) As CRoleList
        Return CType(MyBase.SelectWhere(pi, criteria), CRoleList)
    End Function
    Protected Shadows Function SelectWhere(ByVal pi As CPagingInfo, ByVal criteria As CCriteriaList, ByVal tableViewOrJoinExpr As String) As CRoleList
        Return CType(MyBase.SelectWhere(pi, criteria, tableViewOrJoinExpr), CRoleList)
    End Function
    Protected Shadows Function SelectByIds(ByVal pi As CPagingInfo, ByVal ids As List(Of String)) As CRoleList
        Return CType(MyBase.SelectByIds(pi, ids), CRoleList)
    End Function

    'Select Queries - Transactional (Friend-scoped for use in cascade-deletes)
    Friend Shadows Function SelectAll(ByVal tx As IDbTransaction) As CRoleList
        Return CType(MyBase.SelectAll(tx), CRoleList)
    End Function
    Friend Shadows Function SelectWhere(ByVal columnName As String, columnValue As Object, ByVal tx As IDbTransaction) As CRoleList
        Return CType(MyBase.SelectWhere(columnName, columnValue, tx), CRoleList)
    End Function
    Friend Shadows Function SelectWhere(ByVal columnName As String, sign As ESign, columnValue As Object, ByVal tx As IDbTransaction) As CRoleList
        Return CType(MyBase.SelectWhere(columnName, sign, columnValue, tx), CRoleList)
    End Function
    Friend Shadows Function SelectWhere(ByVal criteria As CCriteria, ByVal tx As IDbTransaction) As CRoleList
        Return CType(MyBase.SelectWhere(criteria, tx), CRoleList)
    End Function
    Friend Shadows Function SelectWhere(ByVal criteria As CCriteriaList, ByVal tx As IDbTransaction) As CRoleList
        Return CType(MyBase.SelectWhere(criteria, tx), CRoleList)
    End Function
    Friend Shadows Function SelectWhere(ByVal criteria As CCriteriaList, ByVal tableOrJoin As String, ByVal tx As IDbTransaction) As CRoleList
        Return CType(MyBase.SelectWhere(criteria, tableOrJoin, tx), CRoleList)
    End Function
    Friend Shadows Function SelectWhere(ByVal criteria As CCriteriaList, ByVal tableOrJoin As String, ByVal orderBy As String, ByVal tx As IDbTransaction) As CRoleList
        Return CType(MyBase.SelectWhere(criteria, tableOrJoin, orderBy, tx), CRoleList)
    End Function
    Friend Shadows Function SelectById(ByVal roleName As String, ByVal tx As IDbTransaction) As CRoleList
        Return CType(MyBase.SelectById(roleName, tx), CRoleList)
    End Function
    Friend Shadows Function SelectByIds(ByVal ids As List(Of String), ByVal tx As IDbTransaction) As CRoleList
        Return CType(MyBase.SelectByIds(ids, tx), CRoleList)
    End Function
    
    'Select Queries - Stored Procedures
    Protected Overloads Function MakeList(ByVal storedProcName As String, ByVal txOrNull As IDbTransaction) As CRoleList
        Return CType(MyBase.MakeList(storedProcName,         txOrNull), CRoleList)
    End Function
    Protected Overloads Function MakeList(ByVal storedProcName As String, ByVal params As Object(), ByVal txOrNull As IDbTransaction) As CRoleList
        Return CType(MyBase.MakeList(storedProcName, params, txOrNull), CRoleList)
    End Function
    Protected Overloads Function MakeList(ByVal storedProcName As String, ByVal params As CNameValueList, ByVal txOrNull As IDbTransaction) As CRoleList
        Return CType(MyBase.MakeList(storedProcName, params, txOrNull), CRoleList)
    End Function
    Protected Overloads Function MakeList(ByVal storedProcName As String, ByVal params As List(Of Object), ByVal txOrNull As IDbTransaction) As CRoleList
        Return CType(MyBase.MakeList(storedProcName, params, txOrNull), CRoleList)
    End Function
    Protected Overloads Function MakeList(ByVal storedProcName As String, ByVal param1 As Integer, ByVal txOrNull As IDbTransaction) As CRoleList
        Return CType(MyBase.MakeList(storedProcName, param1, txOrNull), CRoleList)
    End Function
    Protected Overloads Function MakeList(ByVal storedProcName As String, ByVal param1 As String, ByVal txOrNull As IDbTransaction) As CRoleList
        Return CType(MyBase.MakeList(storedProcName, param1, txOrNull), CRoleList)
    End Function

    'Query Results
    Protected Overloads Function MakeList(ByVal ds As DataSet) As CRoleList
        Return CType(MyBase.MakeList(ds), CRoleList)
    End Function
    Protected Overloads Function MakeList(ByVal dt As DataTable) As CRoleList
        Return CType(MyBase.MakeList(dt), CRoleList)
    End Function
    Protected Overloads Function MakeList(ByVal rows As DataRowCollection) As CRoleList
        Return CType(MyBase.MakeList(rows), CRoleList)
    End Function
    Protected Overloads Function MakeList(ByVal dr As IDataReader) As CRoleList
        Return CType(MyBase.MakeList(dr), CRoleList)
    End Function
    Protected Overloads Function MakeList(ByVal drOrDs As Object) As CRoleList
        Return CType(MyBase.MakeList(drOrDs), CRoleList)
    End Function
    Protected Overloads Function MakeList(ByVal gzip As Byte()) As CRoleList
        Return CType(MyBase.MakeList(gzip), CRoleList)
    End Function
#End Region

#Region "Queries - SelectBy[FK] (user-nominated fk/bool columns)"
    'Note: These methods should only be used for cascade-deletes, or initialising the cache. Use CRole.Cache.GetBy... for reqular queries
    
    'Non-Paged

    'Paged

    'Count

    'Transactional
#End Region

#Region "Static - Cache Implementation"
    Public Shared Property Cache() As CRoleList
        Get
            Cache = CType(CCache.Get(CACHE_KEY), CRoleList)
            If IsNothing(cache) Then
                SyncLock (CACHE_KEY)
                    Cache = CType(CCache.Get(CACHE_KEY), CRoleList)
                    If IsNothing(cache) Then
                        Cache = LoadCache()
                        CRole.Cache = Cache
                    End If
                End SyncLock
            End If
            Return cache
        End Get
        Set(ByVal value As CRoleList)
            SetCache(CACHE_KEY, value)
        End Set
    End Property

    'Change Management:
    'Clone, modify, and then replace the cache (threadsafe for anything iterating the collection)
    'Note that internal indices are dicarded each time unless handled in constructor and add/remove overrides
    Protected Overrides Sub CacheDelete()
        If CacheIsNull Then Exit Sub
        SyncLock (CACHE_KEY)
            Dim temp As CRoleList = New CRoleList(Cache)
            Dim size As Integer = temp.Count
            temp.Remove(Me)
            If size = temp.Count Then 'Remove might fail If cache was refreshed with New instances. Use old index                    
                temp.Remove(CacheGetById(temp))
            End If
            Cache = temp
        End SyncLock
    End Sub
    Protected Overrides Sub CacheInsert()
        If CacheIsNull Then Exit Sub
        SyncLock (CACHE_KEY)
            Dim temp As CRoleList = New CRoleList(Cache)
            temp.Add(Me)
            temp.Sort()
            Cache = temp
        End SyncLock
    End Sub
    Protected Overrides Sub CacheUpdate()
        If CacheIsNull Then Exit Sub
        SyncLock (CACHE_KEY)
            Dim temp As CRoleList = New CRoleList(Cache)
            If Not temp.Contains(Me) Then
                temp.Remove(CacheGetById(temp))
                temp.Add(Me)
            End If
            temp.Sort()
            Cache = temp
        End SyncLock
    End Sub
    Protected Overrides Sub CacheClear()
        Cache = Nothing
    End Sub
    Public Shared ReadOnly Property CacheIsNull() As Boolean
        Get
            Return IsNothing(CCache.Get(CACHE_KEY))
        End Get
    End Property
#End Region

#Region "ToXml"
    Protected Overrides Sub ToXml_Autogenerated(ByVal w As System.Xml.XmlWriter)
        Store(w, "RoleName", Me.RoleName)
    End Sub
#End Region

#Region "Audit Trail"
    Protected Overrides Function OriginalState(ByVal txOrNull As IDbTransaction) As SchemaAudit.CBaseDynamicAudited
        Return New CRole(Me.DataSrc, Me.RoleName, txOrNull)
    End Function
#End Region


End Class