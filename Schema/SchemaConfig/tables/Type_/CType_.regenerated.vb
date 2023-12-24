Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic

Imports Framework

<Serializable(), CLSCompliant(True)> _
Partial Public Class CType_
    Inherits SchemaAudit.CBaseDynamicAudited
    Implements IComparable(Of CType_)

#Region "Constructors"
    'Public (Copy Constructor)
    Public Sub New(ByVal original as CType_)
        m_typeName = original.TypeName
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
        m_typeId = Integer.MinValue
        m_typeName = String.Empty
    End Sub
#End Region

#Region "Members"
    Protected m_typeId As Integer
    Protected m_typeName As String
#End Region

#Region "Properties - Column Values"
    'Primary Key Column (ReadOnly)
    Public ReadOnly Property TypeId() As Integer
        Get
            Return m_typeId
        End Get
    End Property

    'Table Columns (Read/Write)
    Public Property TypeName() As String
        Get
            Return m_typeName
        End Get
        Set(ByVal value As String)
            m_typeName = value
        End Set
    End Property

    'View Columns (ReadOnly)

#End Region

#Region "MustOverride Methods"
    'Schema Information
    Public Const TABLE_NAME As String = "tblConfig_Type"
    Public Const ORDER_BY_COLS As String = "TypeName" 'See the CompareTo method below (Sql-based sorting should match In-Memory sorting)
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
    Public Function CompareTo(other As CType_) As Integer Implements IComparable(Of CType_).CompareTo
        Return Me.TypeName.CompareTo(other.TypeName) 
    End Function

    'Primary Key Information
    Protected Overrides ReadOnly Property InsertPrimaryKey() As Boolean
        Get
            Return False
        End Get
    End Property
    Protected Overrides ReadOnly Property PrimaryKeyName() As String
        Get
            Return "TypeId"
        End Get
    End Property
    Protected Overrides Property PrimaryKeyValue() As Object
        Get
            Return m_typeId
        End Get
        Set(ByVal value As Object)
            m_typeId = CType(Value, Integer)
        End Set
    End Property

    'Factory Methods - Object
    Protected Overrides Function MakeFrom(ByVal row As DataRow) As CBase
        Return New CType_(Me.DataSrc, row)
    End Function
    Protected Overrides Function MakeFrom(ByVal dr As IDataReader) As CBase
        Return New CType_(Me.DataSrc, dr)
    End Function
    
    'Factory Methods - List
    Protected Overrides Function MakeList() As IList
        Return New CType_List
    End Function
    Protected Overrides Function MakeList(ByVal capacity As Integer) As IList
        Return New CType_List(capacity)
    End Function

    'Convert from ADO to .Net
    Protected Overrides Sub ReadColumns(ByVal dr As IDataReader)
        m_typeId = CAdoData.GetInt(dr, "TypeId")
        m_typeName = CAdoData.GetStr(dr, "TypeName")
    End Sub
    Protected Overrides Sub ReadColumns(ByVal dr As DataRow)
        m_typeId = CAdoData.GetInt(dr, "TypeId")
        m_typeName = CAdoData.GetStr(dr, "TypeName")
    End Sub

    'Parameters for Insert/Update    
    Protected Overrides Function ColumnNameValues() As CNameValueList
        Dim data As New CNameValueList
        data.Add("TypeId", NullVal(m_typeId))
        data.Add("TypeName", NullVal(m_typeName))
        Return data
    End Function
#End Region

#Region "Queries - Generic (SelectAll/SelectWhere - Cast only)"
    'Used to load the cache
    Protected Shadows Function SelectAll() As CType_List
        Return CType(MyBase.SelectAll(), CType_List)
    End Function

    'Sometimes use a custom query to load the cache
    Protected Shadows Function SelectWhere(ByVal colName As String, ByVal sign As ESign, ByVal colValue As Object) As CType_List
        Return CType(MyBase.SelectWhere(colName, sign, colValue), CType_List)
    End Function
    Protected Shadows Function SelectWhere(ByVal where As CCriteria) As CType_List
        Return CType(MyBase.SelectWhere(where), CType_List)
    End Function
    Protected Shadows Function SelectWhere(ByVal where As CCriteriaList) As CType_List
        Return CType(MyBase.SelectWhere(where), CType_List)
    End Function
    Protected Shadows Function SelectWhere(ByVal where As CSelectWhere) As CType_List
        Return CType(MyBase.SelectWhere(where), CType_List)
    End Function
#End Region

#Region "Static - Cache Implementation"
    Private Shared CACHE_KEY As String = GetType(CType_).ToString
    Public Shared Property Cache() As CType_List
        Get
            Cache = CType(CCache.Get(CACHE_KEY), CType_List)
            If IsNothing(cache) Then
                SyncLock (CACHE_KEY)
                    Cache = CType(CCache.Get(CACHE_KEY), CType_List)
                    If IsNothing(cache) Then
                        Cache = LoadCache()
                        CType_.Cache = Cache
                    End If
                End SyncLock
            End If
            Return cache
        End Get
        Set(ByVal value As CType_List)
            CCache.Set(CACHE_KEY, value)
        End Set
    End Property

    'Change Management:
    'Clone, modify, and then replace the cache (threadsafe for anything iterating the collection)
    'Note that internal indices are dicarded each time unless handled in constructor and add/remove overrides
    Protected Overrides Sub CacheDelete()
        If CacheIsNull Then Exit Sub
        SyncLock (CACHE_KEY)
            Dim temp As CType_List = New CType_List(Cache)
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
            Dim temp As CType_List = New CType_List(Cache)
            temp.Add(Me)
            temp.Sort()
            Cache = temp
        End SyncLock
    End Sub
    Protected Overrides Sub CacheUpdate()
        If CacheIsNull Then Exit Sub
        SyncLock (CACHE_KEY)
            Dim temp As CType_List = New CType_List(Cache)
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
        Store(w, "TypeId", Me.TypeId)
        Store(w, "TypeName", Me.TypeName)
    End Sub
#End Region

#Region "Audit Trail"
    Protected Overrides Function OriginalState(ByVal txOrNull As IDbTransaction) As SchemaAudit.CBaseDynamicAudited
        Return New CType_(Me.DataSrc, Me.TypeId, txOrNull)
    End Function
#End Region


End Class