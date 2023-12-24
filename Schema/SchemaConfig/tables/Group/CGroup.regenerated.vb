Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic

Imports Framework

<Serializable(), CLSCompliant(True)> _
Partial Public Class CGroup
    Inherits SchemaAudit.CBaseDynamicAudited
    Implements IComparable(Of CGroup)

#Region "Constructors"
    'Public (Copy Constructor)
    Public Sub New(ByVal original as CGroup)
        m_groupName = original.GroupName
        m_groupSortOrder = original.GroupSortOrder
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
        m_groupId = Integer.MinValue
        m_groupName = String.Empty
        m_groupSortOrder = Integer.MinValue
    End Sub
#End Region

#Region "Members"
    Protected m_groupId As Integer
    Protected m_groupName As String
    Protected m_groupSortOrder As Integer
#End Region

#Region "Properties - Column Values"
    'Primary Key Column (ReadOnly)
    Public ReadOnly Property GroupId() As Integer
        Get
            Return m_groupId
        End Get
    End Property

    'Table Columns (Read/Write)
    Public Property GroupName() As String
        Get
            Return m_groupName
        End Get
        Set(ByVal value As String)
            m_groupName = value
        End Set
    End Property
    Public Property GroupSortOrder() As Integer
        Get
            Return m_groupSortOrder
        End Get
        Set(ByVal value As Integer)
            m_groupSortOrder = value
        End Set
    End Property

    'View Columns (ReadOnly)

#End Region

#Region "MustOverride Methods"
    'Schema Information
    Public Const TABLE_NAME As String = "tblConfig_Group"
    Public Const ORDER_BY_COLS As String = "GroupSortOrder, GroupName" 'See the CompareTo method below (Sql-based sorting should match In-Memory sorting)
    Public Const SORTING_COLUMN As String = "GroupSortOrder"
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
    Public Function CompareTo(ByVal other As CGroup) As Integer Implements IComparable(Of CGroup).CompareTo
        Dim i As Integer = Me.GroupSortOrder.CompareTo(other.GroupSortOrder) 
        If 0 <> i Then Return i
        Return Me.GroupName.CompareTo(other.GroupName) 
    End Function

    'Primary Key Information
    Protected Overrides ReadOnly Property InsertPrimaryKey() As Boolean
        Get
            Return False
        End Get
    End Property
    Protected Overrides ReadOnly Property PrimaryKeyName() As String
        Get
            Return "GroupId"
        End Get
    End Property
    Protected Overrides Property PrimaryKeyValue() As Object
        Get
            Return m_groupId
        End Get
        Set(ByVal value As Object)
            m_groupId = CType(Value, Integer)
        End Set
    End Property

    'Factory Methods - Object
    Protected Overrides Function MakeFrom(ByVal row As DataRow) As CBase
        Return New CGroup(Me.DataSrc, row)
    End Function
    Protected Overrides Function MakeFrom(ByVal dr As IDataReader) As CBase
        Return New CGroup(Me.DataSrc, dr)
    End Function
    
    'Factory Methods - List
    Protected Overrides Function MakeList() As IList
        Return New CGroupList
    End Function
    Protected Overrides Function MakeList(ByVal capacity As Integer) As IList
        Return New CGroupList(capacity)
    End Function

    'Convert from ADO to .Net
    Protected Overrides Sub ReadColumns(ByVal dr As IDataReader)
        m_groupId = CAdoData.GetInt(dr, "GroupId")
        m_groupName = CAdoData.GetStr(dr, "GroupName")
        m_groupSortOrder = CAdoData.GetInt(dr, "GroupSortOrder")
    End Sub
    Protected Overrides Sub ReadColumns(ByVal dr As DataRow)
        m_groupId = CAdoData.GetInt(dr, "GroupId")
        m_groupName = CAdoData.GetStr(dr, "GroupName")
        m_groupSortOrder = CAdoData.GetInt(dr, "GroupSortOrder")
    End Sub

    'Parameters for Insert/Update    
    Protected Overrides Function ColumnNameValues() As CNameValueList
        Dim data As New CNameValueList
        data.Add("GroupId", NullVal(m_groupId))
        data.Add("GroupName", NullVal(m_groupName))
        data.Add("GroupSortOrder", NullVal(m_groupSortOrder))
        Return data
    End Function
#End Region

#Region "Queries - Generic (SelectAll/SelectWhere - Cast only)"
    'Used to load the cache
    Protected Shadows Function SelectAll() As CGroupList
        Return CType(MyBase.SelectAll(), CGroupList)
    End Function

    'Sometimes use a custom query to load the cache
    Protected Shadows Function SelectWhere(ByVal colName As String, ByVal sign As ESign, ByVal colValue As Object) As CGroupList
        Return CType(MyBase.SelectWhere(colName, sign, colValue), CGroupList)
    End Function
    Protected Shadows Function SelectWhere(ByVal where As CCriteria) As CGroupList
        Return CType(MyBase.SelectWhere(where), CGroupList)
    End Function
    Protected Shadows Function SelectWhere(ByVal where As CCriteriaList) As CGroupList
        Return CType(MyBase.SelectWhere(where), CGroupList)
    End Function
    Protected Shadows Function SelectWhere(ByVal where As CSelectWhere) As CGroupList
        Return CType(MyBase.SelectWhere(where), CGroupList)
    End Function
#End Region

#Region "Static - Cache Implementation"
    Private Shared CACHE_KEY As String = GetType(CGroup).ToString
    Public Shared Property Cache() As CGroupList
        Get
            Cache = CType(CCache.Get(CACHE_KEY), CGroupList)
            If IsNothing(cache) Then
                SyncLock (CACHE_KEY)
                    Cache = CType(CCache.Get(CACHE_KEY), CGroupList)
                    If IsNothing(cache) Then
                        Cache = LoadCache()
                        CGroup.Cache = Cache
                    End If
                End SyncLock
            End If
            Return cache
        End Get
        Set(ByVal value As CGroupList)
            CCache.Set(CACHE_KEY, value)
        End Set
    End Property

    'Change Management:
    'Clone, modify, and then replace the cache (threadsafe for anything iterating the collection)
    'Note that internal indices are dicarded each time unless handled in constructor and add/remove overrides
    Protected Overrides Sub CacheDelete()
        If CacheIsNull Then Exit Sub
        SyncLock (CACHE_KEY)
            Dim temp As CGroupList = New CGroupList(Cache)
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
            Dim temp As CGroupList = New CGroupList(Cache)
            temp.Add(Me)
            temp.Sort()
            Cache = temp
        End SyncLock
    End Sub
    Protected Overrides Sub CacheUpdate()
        If CacheIsNull Then Exit Sub
        SyncLock (CACHE_KEY)
            Dim temp As CGroupList = New CGroupList(Cache)
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
        Store(w, "GroupId", Me.GroupId)
        Store(w, "GroupName", Me.GroupName)
        Store(w, "GroupSortOrder", Me.GroupSortOrder)
    End Sub
#End Region

#Region "Audit Trail"
    Protected Overrides Function OriginalState(ByVal txOrNull As IDbTransaction) As SchemaAudit.CBaseDynamicAudited
        Return New CGroup(Me.DataSrc, Me.GroupId, txOrNull)
    End Function
#End Region


End Class