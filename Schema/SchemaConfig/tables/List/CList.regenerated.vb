Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic

Imports Framework

<Serializable(), CLSCompliant(True)> _
Partial Public Class CList
    Inherits SchemaAudit.CBaseDynamicAudited
    Implements IComparable(Of CList)

#Region "Constructors"
    'Public (Copy Constructor)
    Public Sub New(ByVal original as CList)
        m_listName = original.ListName
        m_listIsExternal = original.ListIsExternal
        m_listExternalConnectionString = original.ListExternalConnectionString
        m_listExternalTable = original.ListExternalTable
        m_listExteralPrimaryKey = original.ListExteralPrimaryKey
        m_listExteralNameColumn = original.ListExteralNameColumn
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
        m_listId = Integer.MinValue
        m_listName = String.Empty
        m_listIsExternal = False
        m_listExternalConnectionString = String.Empty
        m_listExternalTable = String.Empty
        m_listExteralPrimaryKey = String.Empty
        m_listExteralNameColumn = String.Empty
    End Sub
#End Region

#Region "Members"
    Protected m_listId As Integer
    Protected m_listName As String
    Protected m_listIsExternal As Boolean
    Protected m_listExternalConnectionString As String
    Protected m_listExternalTable As String
    Protected m_listExteralPrimaryKey As String
    Protected m_listExteralNameColumn As String
#End Region

#Region "Properties - Column Values"
    'Primary Key Column (ReadOnly)
    Public ReadOnly Property ListId() As Integer
        Get
            Return m_listId
        End Get
    End Property

    'Table Columns (Read/Write)
    Public Property ListName() As String
        Get
            Return m_listName
        End Get
        Set(ByVal value As String)
            m_listName = value
        End Set
    End Property
    Public Property ListIsExternal() As Boolean
        Get
            Return m_listIsExternal
        End Get
        Set(ByVal value As Boolean)
            m_listIsExternal = value
        End Set
    End Property
    Public Property ListExternalConnectionString() As String
        Get
            Return m_listExternalConnectionString
        End Get
        Set(ByVal value As String)
            m_listExternalConnectionString = value
        End Set
    End Property
    Public Property ListExternalTable() As String
        Get
            Return m_listExternalTable
        End Get
        Set(ByVal value As String)
            m_listExternalTable = value
        End Set
    End Property
    Public Property ListExteralPrimaryKey() As String
        Get
            Return m_listExteralPrimaryKey
        End Get
        Set(ByVal value As String)
            m_listExteralPrimaryKey = value
        End Set
    End Property
    Public Property ListExteralNameColumn() As String
        Get
            Return m_listExteralNameColumn
        End Get
        Set(ByVal value As String)
            m_listExteralNameColumn = value
        End Set
    End Property

    'View Columns (ReadOnly)

#End Region

#Region "MustOverride Methods"
    'Schema Information
    Public Const TABLE_NAME As String = "tblConfig_List"
    Public Const ORDER_BY_COLS As String = "ListName" 'See the CompareTo method below (Sql-based sorting should match In-Memory sorting)
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
    Public Function CompareTo(other As CList) As Integer Implements IComparable(Of CList).CompareTo
        Return Me.ListName.CompareTo(other.ListName) 
    End Function

    'Primary Key Information
    Protected Overrides ReadOnly Property InsertPrimaryKey() As Boolean
        Get
            Return False
        End Get
    End Property
    Protected Overrides ReadOnly Property PrimaryKeyName() As String
        Get
            Return "ListId"
        End Get
    End Property
    Protected Overrides Property PrimaryKeyValue() As Object
        Get
            Return m_listId
        End Get
        Set(ByVal value As Object)
            m_listId = CType(Value, Integer)
        End Set
    End Property

    'Factory Methods - Object
    Protected Overrides Function MakeFrom(ByVal row As DataRow) As CBase
        Return New CList(Me.DataSrc, row)
    End Function
    Protected Overrides Function MakeFrom(ByVal dr As IDataReader) As CBase
        Return New CList(Me.DataSrc, dr)
    End Function
    
    'Factory Methods - List
    Protected Overrides Function MakeList() As IList
        Return New CListList
    End Function
    Protected Overrides Function MakeList(ByVal capacity As Integer) As IList
        Return New CListList(capacity)
    End Function

    'Convert from ADO to .Net
    Protected Overrides Sub ReadColumns(ByVal dr As IDataReader)
        m_listId = CAdoData.GetInt(dr, "ListId")
        m_listName = CAdoData.GetStr(dr, "ListName")
        m_listIsExternal = CAdoData.GetBool(dr, "ListIsExternal")
        m_listExternalConnectionString = CAdoData.GetStr(dr, "ListExternalConnectionString")
        m_listExternalTable = CAdoData.GetStr(dr, "ListExternalTable")
        m_listExteralPrimaryKey = CAdoData.GetStr(dr, "ListExteralPrimaryKey")
        m_listExteralNameColumn = CAdoData.GetStr(dr, "ListExteralNameColumn")
    End Sub
    Protected Overrides Sub ReadColumns(ByVal dr As DataRow)
        m_listId = CAdoData.GetInt(dr, "ListId")
        m_listName = CAdoData.GetStr(dr, "ListName")
        m_listIsExternal = CAdoData.GetBool(dr, "ListIsExternal")
        m_listExternalConnectionString = CAdoData.GetStr(dr, "ListExternalConnectionString")
        m_listExternalTable = CAdoData.GetStr(dr, "ListExternalTable")
        m_listExteralPrimaryKey = CAdoData.GetStr(dr, "ListExteralPrimaryKey")
        m_listExteralNameColumn = CAdoData.GetStr(dr, "ListExteralNameColumn")
    End Sub

    'Parameters for Insert/Update    
    Protected Overrides Function ColumnNameValues() As CNameValueList
        Dim data As New CNameValueList
        data.Add("ListId", NullVal(m_listId))
        data.Add("ListName", NullVal(m_listName))
        data.Add("ListIsExternal", NullVal(m_listIsExternal))
        data.Add("ListExternalConnectionString", NullVal(m_listExternalConnectionString))
        data.Add("ListExternalTable", NullVal(m_listExternalTable))
        data.Add("ListExteralPrimaryKey", NullVal(m_listExteralPrimaryKey))
        data.Add("ListExteralNameColumn", NullVal(m_listExteralNameColumn))
        Return data
    End Function
#End Region

#Region "Queries - Generic (SelectAll/SelectWhere - Cast only)"
    'Used to load the cache
    Protected Shadows Function SelectAll() As CListList
        Return CType(MyBase.SelectAll(), CListList)
    End Function

    'Sometimes use a custom query to load the cache
    Protected Shadows Function SelectWhere(ByVal colName As String, ByVal sign As ESign, ByVal colValue As Object) As CListList
        Return CType(MyBase.SelectWhere(colName, sign, colValue), CListList)
    End Function
    Protected Shadows Function SelectWhere(ByVal where As CCriteria) As CListList
        Return CType(MyBase.SelectWhere(where), CListList)
    End Function
    Protected Shadows Function SelectWhere(ByVal where As CCriteriaList) As CListList
        Return CType(MyBase.SelectWhere(where), CListList)
    End Function
    Protected Shadows Function SelectWhere(ByVal where As CSelectWhere) As CListList
        Return CType(MyBase.SelectWhere(where), CListList)
    End Function
#End Region

#Region "Static - Cache Implementation"
    Private Shared CACHE_KEY As String = GetType(CList).ToString
    Public Shared Property Cache() As CListList
        Get
            Cache = CType(CCache.Get(CACHE_KEY), CListList)
            If IsNothing(cache) Then
                SyncLock (CACHE_KEY)
                    Cache = CType(CCache.Get(CACHE_KEY), CListList)
                    If IsNothing(cache) Then
                        Cache = LoadCache()
                        CList.Cache = Cache
                    End If
                End SyncLock
            End If
            Return cache
        End Get
        Set(ByVal value As CListList)
            CCache.Set(CACHE_KEY, value)
        End Set
    End Property

    'Change Management:
    'Clone, modify, and then replace the cache (threadsafe for anything iterating the collection)
    'Note that internal indices are dicarded each time unless handled in constructor and add/remove overrides
    Protected Overrides Sub CacheDelete()
        If CacheIsNull Then Exit Sub
        SyncLock (CACHE_KEY)
            Dim temp As CListList = New CListList(Cache)
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
            Dim temp As CListList = New CListList(Cache)
            temp.Add(Me)
            temp.Sort()
            Cache = temp
        End SyncLock
    End Sub
    Protected Overrides Sub CacheUpdate()
        If CacheIsNull Then Exit Sub
        SyncLock (CACHE_KEY)
            Dim temp As CListList = New CListList(Cache)
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
        Store(w, "ListId", Me.ListId)
        Store(w, "ListName", Me.ListName)
        Store(w, "ListIsExternal", Me.ListIsExternal)
        Store(w, "ListExternalConnectionString", Me.ListExternalConnectionString)
        Store(w, "ListExternalTable", Me.ListExternalTable)
        Store(w, "ListExteralPrimaryKey", Me.ListExteralPrimaryKey)
        Store(w, "ListExteralNameColumn", Me.ListExteralNameColumn)
    End Sub
#End Region

#Region "Audit Trail"
    Protected Overrides Function OriginalState(ByVal txOrNull As IDbTransaction) As SchemaAudit.CBaseDynamicAudited
        Return New CList(Me.DataSrc, Me.ListId, txOrNull)
    End Function
#End Region


End Class