Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic

Imports Framework

<Serializable(), CLSCompliant(True)> _
Partial Public Class CItem
    Inherits SchemaAudit.CBaseDynamicAudited
    Implements IComparable(Of CItem)

#Region "Constructors"
    'Public (Copy Constructor)
    Public Sub New(ByVal original as CItem)
        m_itemListId = original.ItemListId
        m_itemName = original.ItemName
        m_itemSortOrder = original.ItemSortOrder
        m_itemIsDeleted = original.ItemIsDeleted
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
        m_itemId = Integer.MinValue
        m_itemListId = Integer.MinValue
        m_itemName = String.Empty
        m_itemSortOrder = Integer.MinValue
        m_itemIsDeleted = False
    End Sub
#End Region

#Region "Members"
    Protected m_itemId As Integer
    Protected m_itemListId As Integer
    Protected m_itemName As String
    Protected m_itemSortOrder As Integer
    Protected m_itemIsDeleted As Boolean
#End Region

#Region "Properties - Column Values"
    'Primary Key Column (ReadOnly)
    Public ReadOnly Property ItemId() As Integer
        Get
            Return m_itemId
        End Get
    End Property

    'Table Columns (Read/Write)
    Public Property ItemListId() As Integer
        Get
            Return m_itemListId
        End Get
        Set(ByVal value As Integer)
            m_itemListId = value
        End Set
    End Property
    Public Property ItemName() As String
        Get
            Return m_itemName
        End Get
        Set(ByVal value As String)
            m_itemName = value
        End Set
    End Property
    Public Property ItemSortOrder() As Integer
        Get
            Return m_itemSortOrder
        End Get
        Set(ByVal value As Integer)
            m_itemSortOrder = value
        End Set
    End Property
    Public Property ItemIsDeleted() As Boolean
        Get
            Return m_itemIsDeleted
        End Get
        Set(ByVal value As Boolean)
            m_itemIsDeleted = value
        End Set
    End Property

    'View Columns (ReadOnly)

#End Region

#Region "MustOverride Methods"
    'Schema Information
    Public Const TABLE_NAME As String = "tblConfig_Item"
    Public Const ORDER_BY_COLS As String = "ItemSortOrder, ItemName" 'See the CompareTo method below (Sql-based sorting should match In-Memory sorting)
    Public Const SORTING_COLUMN As String = "ItemSortOrder"
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
    Public Function CompareTo(ByVal other As CItem) As Integer Implements IComparable(Of CItem).CompareTo
        Dim i As Integer = Me.ItemSortOrder.CompareTo(other.ItemSortOrder) 
        If 0 <> i Then Return i
        Return Me.ItemName.CompareTo(other.ItemName) 
    End Function

    'Primary Key Information
    Protected Overrides ReadOnly Property InsertPrimaryKey() As Boolean
        Get
            Return False
        End Get
    End Property
    Protected Overrides ReadOnly Property PrimaryKeyName() As String
        Get
            Return "ItemId"
        End Get
    End Property
    Protected Overrides Property PrimaryKeyValue() As Object
        Get
            Return m_itemId
        End Get
        Set(ByVal value As Object)
            m_itemId = CType(Value, Integer)
        End Set
    End Property

    'Factory Methods - Object
    Protected Overrides Function MakeFrom(ByVal row As DataRow) As CBase
        Return New CItem(Me.DataSrc, row)
    End Function
    Protected Overrides Function MakeFrom(ByVal dr As IDataReader) As CBase
        Return New CItem(Me.DataSrc, dr)
    End Function
    
    'Factory Methods - List
    Protected Overrides Function MakeList() As IList
        Return New CItemList
    End Function
    Protected Overrides Function MakeList(ByVal capacity As Integer) As IList
        Return New CItemList(capacity)
    End Function

    'Convert from ADO to .Net
    Protected Overrides Sub ReadColumns(ByVal dr As IDataReader)
        m_itemId = CAdoData.GetInt(dr, "ItemId")
        m_itemListId = CAdoData.GetInt(dr, "ItemListId")
        m_itemName = CAdoData.GetStr(dr, "ItemName")
        m_itemSortOrder = CAdoData.GetInt(dr, "ItemSortOrder")
        m_itemIsDeleted = CAdoData.GetBool(dr, "ItemIsDeleted")
    End Sub
    Protected Overrides Sub ReadColumns(ByVal dr As DataRow)
        m_itemId = CAdoData.GetInt(dr, "ItemId")
        m_itemListId = CAdoData.GetInt(dr, "ItemListId")
        m_itemName = CAdoData.GetStr(dr, "ItemName")
        m_itemSortOrder = CAdoData.GetInt(dr, "ItemSortOrder")
        m_itemIsDeleted = CAdoData.GetBool(dr, "ItemIsDeleted")
    End Sub

    'Parameters for Insert/Update    
    Protected Overrides Function ColumnNameValues() As CNameValueList
        Dim data As New CNameValueList
        data.Add("ItemId", NullVal(m_itemId))
        data.Add("ItemListId", NullVal(m_itemListId))
        data.Add("ItemName", NullVal(m_itemName))
        data.Add("ItemSortOrder", NullVal(m_itemSortOrder))
        data.Add("ItemIsDeleted", NullVal(m_itemIsDeleted))
        Return data
    End Function
#End Region

#Region "Queries - Generic (SelectAll/SelectWhere - Cast only)"
    'Used to load the cache
    Protected Shadows Function SelectAll() As CItemList
        Return CType(MyBase.SelectAll(), CItemList)
    End Function

    'Sometimes use a custom query to load the cache
    Protected Shadows Function SelectWhere(ByVal colName As String, ByVal sign As ESign, ByVal colValue As Object) As CItemList
        Return CType(MyBase.SelectWhere(colName, sign, colValue), CItemList)
    End Function
    Protected Shadows Function SelectWhere(ByVal where As CCriteria) As CItemList
        Return CType(MyBase.SelectWhere(where), CItemList)
    End Function
    Protected Shadows Function SelectWhere(ByVal where As CCriteriaList) As CItemList
        Return CType(MyBase.SelectWhere(where), CItemList)
    End Function
    Protected Shadows Function SelectWhere(ByVal where As CSelectWhere) As CItemList
        Return CType(MyBase.SelectWhere(where), CItemList)
    End Function
#End Region

#Region "Static - Cache Implementation"
    Private Shared CACHE_KEY As String = GetType(CItem).ToString
    Public Shared Property Cache() As CItemList
        Get
            Cache = CType(CCache.Get(CACHE_KEY), CItemList)
            If IsNothing(cache) Then
                SyncLock (CACHE_KEY)
                    Cache = CType(CCache.Get(CACHE_KEY), CItemList)
                    If IsNothing(cache) Then
                        Cache = LoadCache()
                        CItem.Cache = Cache
                    End If
                End SyncLock
            End If
            Return cache
        End Get
        Set(ByVal value As CItemList)
            CCache.Set(CACHE_KEY, value)
        End Set
    End Property

    'Change Management:
    'Clone, modify, and then replace the cache (threadsafe for anything iterating the collection)
    'Note that internal indices are dicarded each time unless handled in constructor and add/remove overrides
    Protected Overrides Sub CacheDelete()
        If CacheIsNull Then Exit Sub
        SyncLock (CACHE_KEY)
            Dim temp As CItemList = New CItemList(Cache)
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
            Dim temp As CItemList = New CItemList(Cache)
            temp.Add(Me)
            temp.Sort()
            Cache = temp
        End SyncLock
    End Sub
    Protected Overrides Sub CacheUpdate()
        If CacheIsNull Then Exit Sub
        SyncLock (CACHE_KEY)
            Dim temp As CItemList = New CItemList(Cache)
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
        Store(w, "ItemId", Me.ItemId)
        Store(w, "ItemListId", Me.ItemListId)
        Store(w, "ItemName", Me.ItemName)
        Store(w, "ItemSortOrder", Me.ItemSortOrder)
        Store(w, "ItemIsDeleted", Me.ItemIsDeleted)
    End Sub
#End Region

#Region "Audit Trail"
    Protected Overrides Function OriginalState(ByVal txOrNull As IDbTransaction) As SchemaAudit.CBaseDynamicAudited
        Return New CItem(Me.DataSrc, Me.ItemId, txOrNull)
    End Function
#End Region


End Class