Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic

Imports Framework

<Serializable(), CLSCompliant(True)> _
Partial Public Class CAudit_Type : Inherits CBaseDynamic : Implements IComparable(Of CAudit_Type)

#Region "Constructors"
    'Public (Copy Constructor)
    Public Sub New(ByVal original as CAudit_Type)
        m_typeName = original.typeName
    End Sub

    'Protected (Datareader/Dataset)
    Protected Sub New(ByVal dataSrc As CDataSrc, ByVal dr As IDataReader)
        MyBase.New(dataSrc, dr)
    End Sub
    Protected Sub New(ByVal dataSrc As CDataSrc, ByVal dr As DataRow)
        MyBase.New(dataSrc, dr)
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
            m_typeName = Value
        End Set
    End Property

    'View Columns (ReadOnly)

#End Region

#Region "MustOverride Methods"
    'Schema Information
    Public Const TABLE_NAME As String = "tblAudit_Type"
    Public Const ORDER_BY_COLS As String = "TypeName"
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

    'CompareTo Interface (Default Sort Order)
    Public Function CompareTo(other As CAudit_Type) As Integer Implements IComparable(Of CAudit_Type).CompareTo
        Return Me.TypeName.CompareTo(other.TypeName) 
    End Function
    
    'Object Factory
    Protected Overrides Function MakeFrom(ByVal row As DataRow) As CBase
        Return New CAudit_Type(Me.DataSrc, row)
    End Function
    Protected Overrides Function MakeFrom(ByVal dr As IDataReader) As CBase
        Return New CAudit_Type(Me.DataSrc, dr)
    End Function
    Protected Overrides Function MakeList() As IList
        Return New CAudit_TypeList
    End Function
    Protected Overrides Function MakeList(ByVal capacity As Integer) As IList
        Return New CAudit_TypeList(capacity)
    End Function

    'Convert from ADO to .Net
    Protected Overrides Sub ReadColumns(ByVal dr As IDataReader)
        m_typeId = GetInt(dr, "TypeId")
        m_typeName = GetStr(dr, "TypeName")
    End Sub
    Protected Overrides Sub ReadColumns(ByVal dr As DataRow)
        m_typeId = GetInt(dr, "TypeId")
        m_typeName = GetStr(dr, "TypeName")
    End Sub

    'Parameters for Insert/Update    
    Protected Overrides Function ColumnNameValues() As CNameValueList
        Dim data As New CNameValueList
        data.Add("TypeId", NullVal(m_typeId))
        data.Add("TypeName", NullVal(m_typeName))
        Return data
    End Function
#End Region

#Region "Select Functions (cast only)"
    'Non-Paged
    Public Shadows Function SelectAll() As CAudit_TypeList
        Return SelectAll(Nothing)
    End Function

    'Paged
    Public Shadows Function SelectAll(ByVal pi As CPagingInfo) As CAudit_TypeList
        Return CType(MyBase.SelectAll(pi), CAudit_TypeList)
    End Function
#End Region

#Region "Static - Cache Implementation"
    Private Shared CACHE_KEY As String = GetType(CAudit_Type).ToString
    Public Shared Property Cache() As CAudit_TypeList
        Get
            Cache = CType(CCache.Get(CACHE_KEY), CAudit_TypeList)
            If IsNothing(cache) Then
                SyncLock (GetType(CAudit_TypeList))
                    Cache = CType(CCache.Get(CACHE_KEY), CAudit_TypeList)
                    If IsNothing(cache) Then
                        Cache = New CAudit_Type().SelectAll()
                        CAudit_Type.Cache = Cache
                    End If
                End SyncLock
            End If
            Return cache
        End Get
        Set(ByVal value As CAudit_TypeList)
            SetCache(CACHE_KEY, value)   'Not locked, because cache gets cleared at anytime anyway
        End Set
    End Property

    'Change Management:
    'Clone, modify, and then replace the cache (threadsafe for anything iterating the collection)
    'Note that internal indices are dicarded each time unless handled in constructor and add/remove overrides
    Protected Overrides Sub CacheDelete()
        If CacheIsNull Then Exit Sub
        SyncLock (GetType(CAudit_TypeList))
            Dim temp As CAudit_TypeList = New CAudit_TypeList(Cache)
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
        SyncLock (GetType(CAudit_TypeList))
            Dim temp As CAudit_TypeList = New CAudit_TypeList(Cache)
            temp.Add(Me)
            temp.Sort()
            Cache = temp
        End SyncLock
    End Sub
    Protected Overrides Sub CacheUpdate()
        If CacheIsNull Then Exit Sub
        SyncLock (GetType(CAudit_TypeList))
            Dim temp As CAudit_TypeList = New CAudit_TypeList(Cache)
            If Not temp.Contains(Me) Then
                temp.Remove(CacheGetById(temp))
                temp.Add(Me)
            Else
                temp.Sort()
            End If
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

End Class