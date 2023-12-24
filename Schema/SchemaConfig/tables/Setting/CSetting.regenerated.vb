Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic

Imports Framework

<Serializable(), CLSCompliant(True)> _
Partial Public Class CSetting
    Inherits SchemaAudit.CBaseDynamicAudited
    Implements IComparable(Of CSetting)

#Region "Constructors"
    'Public (Copy Constructor)
    Public Sub New(ByVal original as CSetting)
        m_settingName = original.SettingName
        m_settingGroupId = original.SettingGroupId
        m_settingTypeId = original.SettingTypeId
        m_settingListId = original.SettingListId
        m_settingSortOrder = original.SettingSortOrder
        m_settingClientCanEdit = original.SettingClientCanEdit
        m_settingValueBoolean = original.SettingValueBoolean
        m_settingValueString = original.SettingValueString
        m_settingValueInteger = original.SettingValueInteger
        m_settingValueDouble = original.SettingValueDouble
        m_settingValueDate = original.SettingValueDate
        m_settingValueMoney = original.SettingValueMoney
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
        m_settingId = Integer.MinValue
        m_settingName = String.Empty
        m_settingGroupId = Integer.MinValue
        m_settingTypeId = Integer.MinValue
        m_settingListId = Integer.MinValue
        m_settingSortOrder = Integer.MinValue
        m_settingClientCanEdit = False
        m_settingValueBoolean = False
        m_settingValueString = String.Empty
        m_settingValueInteger = Integer.MinValue
        m_settingValueDouble = Double.NaN
        m_settingValueDate = DateTime.MinValue
        m_settingValueMoney = Decimal.MinValue
    End Sub
#End Region

#Region "Members"
    Protected m_settingId As Integer
    Protected m_settingName As String
    Protected m_settingGroupId As Integer
    Protected m_settingTypeId As Integer
    Protected m_settingListId As Integer
    Protected m_settingSortOrder As Integer
    Protected m_settingClientCanEdit As Boolean
    Protected m_settingValueBoolean As Boolean
    Protected m_settingValueString As String
    Protected m_settingValueInteger As Integer
    Protected m_settingValueDouble As Double
    Protected m_settingValueDate As DateTime
    Protected m_settingValueMoney As Decimal
#End Region

#Region "Properties - Column Values"
    'Primary Key Column (ReadOnly)
    Public ReadOnly Property SettingId() As Integer
        Get
            Return m_settingId
        End Get
    End Property

    'Table Columns (Read/Write)
    Public Property SettingName() As String
        Get
            Return m_settingName
        End Get
        Set(ByVal value As String)
            m_settingName = value
        End Set
    End Property
    Public Property SettingGroupId() As Integer
        Get
            Return m_settingGroupId
        End Get
        Set(ByVal value As Integer)
            m_settingGroupId = value
        End Set
    End Property
    Public Property SettingTypeId() As Integer
        Get
            Return m_settingTypeId
        End Get
        Set(ByVal value As Integer)
            m_settingTypeId = value
        End Set
    End Property
    Public Property SettingListId() As Integer
        Get
            Return m_settingListId
        End Get
        Set(ByVal value As Integer)
            m_settingListId = value
        End Set
    End Property
    Public Property SettingSortOrder() As Integer
        Get
            Return m_settingSortOrder
        End Get
        Set(ByVal value As Integer)
            m_settingSortOrder = value
        End Set
    End Property
    Public Property SettingClientCanEdit() As Boolean
        Get
            Return m_settingClientCanEdit
        End Get
        Set(ByVal value As Boolean)
            m_settingClientCanEdit = value
        End Set
    End Property
    Public Property SettingValueBoolean() As Boolean
        Get
            Return m_settingValueBoolean
        End Get
        Set(ByVal value As Boolean)
            m_settingValueBoolean = value
        End Set
    End Property
    Public Property SettingValueString() As String
        Get
            Return m_settingValueString
        End Get
        Set(ByVal value As String)
            m_settingValueString = value
        End Set
    End Property
    Public Property SettingValueInteger() As Integer
        Get
            Return m_settingValueInteger
        End Get
        Set(ByVal value As Integer)
            m_settingValueInteger = value
        End Set
    End Property
    Public Property SettingValueDouble() As Double
        Get
            Return m_settingValueDouble
        End Get
        Set(ByVal value As Double)
            m_settingValueDouble = value
        End Set
    End Property
    Public Property SettingValueDate() As DateTime
        Get
            Return m_settingValueDate
        End Get
        Set(ByVal value As DateTime)
            m_settingValueDate = value
        End Set
    End Property
    Public Property SettingValueMoney() As Decimal
        Get
            Return m_settingValueMoney
        End Get
        Set(ByVal value As Decimal)
            m_settingValueMoney = value
        End Set
    End Property

    'View Columns (ReadOnly)

#End Region

#Region "MustOverride Methods"
    'Schema Information
    Public Const TABLE_NAME As String = "tblConfig_Setting"
    Public Const ORDER_BY_COLS As String = "SettingSortOrder, SettingName" 'See the CompareTo method below (Sql-based sorting should match In-Memory sorting)
    Public Const SORTING_COLUMN As String = "SettingSortOrder"
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
    Public Function CompareTo(ByVal other As CSetting) As Integer Implements IComparable(Of CSetting).CompareTo
        Dim i As Integer = Me.SettingSortOrder.CompareTo(other.SettingSortOrder) 
        If 0 <> i Then Return i
        Return Me.SettingName.CompareTo(other.SettingName) 
    End Function

    'Primary Key Information
    Protected Overrides ReadOnly Property InsertPrimaryKey() As Boolean
        Get
            Return False
        End Get
    End Property
    Protected Overrides ReadOnly Property PrimaryKeyName() As String
        Get
            Return "SettingId"
        End Get
    End Property
    Protected Overrides Property PrimaryKeyValue() As Object
        Get
            Return m_settingId
        End Get
        Set(ByVal value As Object)
            m_settingId = CType(Value, Integer)
        End Set
    End Property

    'Factory Methods - Object
    Protected Overrides Function MakeFrom(ByVal row As DataRow) As CBase
        Return New CSetting(Me.DataSrc, row)
    End Function
    Protected Overrides Function MakeFrom(ByVal dr As IDataReader) As CBase
        Return New CSetting(Me.DataSrc, dr)
    End Function
    
    'Factory Methods - List
    Protected Overrides Function MakeList() As IList
        Return New CSettingList
    End Function
    Protected Overrides Function MakeList(ByVal capacity As Integer) As IList
        Return New CSettingList(capacity)
    End Function

    'Convert from ADO to .Net
    Protected Overrides Sub ReadColumns(ByVal dr As IDataReader)
        m_settingId = CAdoData.GetInt(dr, "SettingId")
        m_settingName = CAdoData.GetStr(dr, "SettingName")
        m_settingGroupId = CAdoData.GetInt(dr, "SettingGroupId")
        m_settingTypeId = CAdoData.GetInt(dr, "SettingTypeId")
        m_settingListId = CAdoData.GetInt(dr, "SettingListId")
        m_settingSortOrder = CAdoData.GetInt(dr, "SettingSortOrder")
        m_settingClientCanEdit = CAdoData.GetBool(dr, "SettingClientCanEdit")
        m_settingValueBoolean = CAdoData.GetBool(dr, "SettingValueBoolean")
        m_settingValueString = CAdoData.GetStr(dr, "SettingValueString")
        m_settingValueInteger = CAdoData.GetInt(dr, "SettingValueInteger")
        m_settingValueDouble = CAdoData.GetDbl(dr, "SettingValueDouble")
        m_settingValueDate = CAdoData.GetDate(dr, "SettingValueDate")
        m_settingValueMoney = CAdoData.GetDec(dr, "SettingValueMoney")
    End Sub
    Protected Overrides Sub ReadColumns(ByVal dr As DataRow)
        m_settingId = CAdoData.GetInt(dr, "SettingId")
        m_settingName = CAdoData.GetStr(dr, "SettingName")
        m_settingGroupId = CAdoData.GetInt(dr, "SettingGroupId")
        m_settingTypeId = CAdoData.GetInt(dr, "SettingTypeId")
        m_settingListId = CAdoData.GetInt(dr, "SettingListId")
        m_settingSortOrder = CAdoData.GetInt(dr, "SettingSortOrder")
        m_settingClientCanEdit = CAdoData.GetBool(dr, "SettingClientCanEdit")
        m_settingValueBoolean = CAdoData.GetBool(dr, "SettingValueBoolean")
        m_settingValueString = CAdoData.GetStr(dr, "SettingValueString")
        m_settingValueInteger = CAdoData.GetInt(dr, "SettingValueInteger")
        m_settingValueDouble = CAdoData.GetDbl(dr, "SettingValueDouble")
        m_settingValueDate = CAdoData.GetDate(dr, "SettingValueDate")
        m_settingValueMoney = CAdoData.GetDec(dr, "SettingValueMoney")
    End Sub

    'Parameters for Insert/Update    
    Protected Overrides Function ColumnNameValues() As CNameValueList
        Dim data As New CNameValueList
        data.Add("SettingId", NullVal(m_settingId))
        data.Add("SettingName", NullVal(m_settingName))
        data.Add("SettingGroupId", NullVal(m_settingGroupId))
        data.Add("SettingTypeId", NullVal(m_settingTypeId))
        data.Add("SettingListId", NullVal(m_settingListId))
        data.Add("SettingSortOrder", NullVal(m_settingSortOrder))
        data.Add("SettingClientCanEdit", NullVal(m_settingClientCanEdit))
        data.Add("SettingValueBoolean", NullVal(m_settingValueBoolean))
        data.Add("SettingValueString", NullVal(m_settingValueString))
        data.Add("SettingValueInteger", NullVal(m_settingValueInteger))
        data.Add("SettingValueDouble", NullVal(m_settingValueDouble))
        data.Add("SettingValueDate", NullVal(m_settingValueDate))
        data.Add("SettingValueMoney", NullVal(m_settingValueMoney))
        Return data
    End Function
#End Region

#Region "Queries - Generic (SelectAll/SelectWhere - Cast only)"
    'Used to load the cache
    Protected Shadows Function SelectAll() As CSettingList
        Return CType(MyBase.SelectAll(), CSettingList)
    End Function

    'Sometimes use a custom query to load the cache
    Protected Shadows Function SelectWhere(ByVal colName As String, ByVal sign As ESign, ByVal colValue As Object) As CSettingList
        Return CType(MyBase.SelectWhere(colName, sign, colValue), CSettingList)
    End Function
    Protected Shadows Function SelectWhere(ByVal where As CCriteria) As CSettingList
        Return CType(MyBase.SelectWhere(where), CSettingList)
    End Function
    Protected Shadows Function SelectWhere(ByVal where As CCriteriaList) As CSettingList
        Return CType(MyBase.SelectWhere(where), CSettingList)
    End Function
    Protected Shadows Function SelectWhere(ByVal where As CSelectWhere) As CSettingList
        Return CType(MyBase.SelectWhere(where), CSettingList)
    End Function
#End Region

#Region "Static - Cache Implementation"
    Private Shared CACHE_KEY As String = GetType(CSetting).ToString
    Public Shared Property Cache() As CSettingList
        Get
            Cache = CType(CCache.Get(CACHE_KEY), CSettingList)
            If IsNothing(cache) Then
                SyncLock (CACHE_KEY)
                    Cache = CType(CCache.Get(CACHE_KEY), CSettingList)
                    If IsNothing(cache) Then
                        Cache = LoadCache()
                        CSetting.Cache = Cache
                    End If
                End SyncLock
            End If
            Return cache
        End Get
        Set(ByVal value As CSettingList)
            CCache.Set(CACHE_KEY, value)
        End Set
    End Property

    'Change Management:
    'Clone, modify, and then replace the cache (threadsafe for anything iterating the collection)
    'Note that internal indices are dicarded each time unless handled in constructor and add/remove overrides
    Protected Overrides Sub CacheDelete()
        If CacheIsNull Then Exit Sub
        SyncLock (CACHE_KEY)
            Dim temp As CSettingList = New CSettingList(Cache)
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
            Dim temp As CSettingList = New CSettingList(Cache)
            temp.Add(Me)
            temp.Sort()
            Cache = temp
        End SyncLock
    End Sub
    Protected Overrides Sub CacheUpdate()
        If CacheIsNull Then Exit Sub
        SyncLock (CACHE_KEY)
            Dim temp As CSettingList = New CSettingList(Cache)
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
        Store(w, "SettingId", Me.SettingId)
        Store(w, "SettingName", Me.SettingName)
        Store(w, "SettingGroupId", Me.SettingGroupId)
        Store(w, "SettingTypeId", Me.SettingTypeId)
        Store(w, "SettingListId", Me.SettingListId)
        Store(w, "SettingSortOrder", Me.SettingSortOrder)
        Store(w, "SettingClientCanEdit", Me.SettingClientCanEdit)
        Store(w, "SettingValueBoolean", Me.SettingValueBoolean)
        Store(w, "SettingValueString", Me.SettingValueString)
        Store(w, "SettingValueInteger", Me.SettingValueInteger)
        Store(w, "SettingValueDouble", Me.SettingValueDouble)
        Store(w, "SettingValueDate", Me.SettingValueDate)
        Store(w, "SettingValueMoney", Me.SettingValueMoney)
    End Sub
#End Region

#Region "Audit Trail"
    Protected Overrides Function OriginalState(ByVal txOrNull As IDbTransaction) As SchemaAudit.CBaseDynamicAudited
        Return New CSetting(Me.DataSrc, Me.SettingId, txOrNull)
    End Function
#End Region


End Class