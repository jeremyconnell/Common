Imports System
Imports System.Data
Imports System.Collections.Generic

Imports Framework

#Region "Enum: Primary Key Values"
'Public Enum ESetting
'    Huey = 1
'    Duey = 2
'    Louie = 3
'End Enum
#End Region

'Table-Row Class (Customisable half)
Partial Public Class CSetting

#Region "Constants"
#End Region

#Region "Constructors (Public)"
    'Default Connection String
    Public Sub New()
        MyBase.New()
    End Sub
    
    'Alternative Connection String
    Public Sub New(ByVal dataSrc as CDataSrc)
        MyBase.New(dataSrc)
    End Sub
    
    'Hidden (UI code should use cache instead)
    Protected Friend Sub New(settingId As Integer)
        MyBase.New(settingId)
    End Sub
    Protected Friend Sub New(ByVal dataSrc As CDataSrc, settingId As Integer)
        MyBase.New(dataSrc, settingId)
    End Sub
    Protected Friend Sub New(ByVal dataSrc As CDataSrc, ByVal settingId As Integer, ByVal txOrNull As IDbTransaction)
        MyBase.New(dataSrc, settingId, txOrNull)
    End Sub
#End Region

#Region "Default Values"
    Protected Overrides Sub InitValues_Custom()
        m_settingSortOrder = 0
    End Sub
#End Region

#Region "Default Connection String"
    Protected Overrides Function DefaultDataSrc() As CDataSrc
        Return CDataSrc.Default
    End Function
#End Region

#Region "Properties - Relationships"    
    'Relationships - Foriegn Keys (e.g parent)
    Public ReadOnly Property Group() As CGroup
        Get
            Return CGroup.Cache.GetById(Me.SettingGroupId)
        End Get
    End Property
    Public ReadOnly Property Type() As CType_
        Get
            Return CType_.Cache.GetById(Me.SettingTypeId)
        End Get
    End Property
    Public ReadOnly Property List() As CList
        Get
            Return CList.Cache.GetById(Me.SettingListId)
        End Get
    End Property

    'Relationships - Collections (e.g. children)

#End Region

#Region "Properties - Customisation"
    Public Property SettingTypeId_() As EConfigType
        Get
            Return CType(m_settingTypeId, EConfigType)
        End Get
        Set(ByVal value As EConfigType)
            m_settingTypeId = value
        End Set
    End Property
#End Region

#Region "Save/Delete Overrides"
    'Can Override MyBase.Save/Delete (e.g. Cascade deletes, or insert related records)
#End Region

#Region "Search Logic"
    'Public Interfaces: Refer to list class for Search (and GetBy*) methods e.g. CSetting.Cache.Search

    'Complex Search Logic (e.g string-based searching) Note: Index-based filters (e.g. fk/bool) are handled in the Search method, and can be ignored here
    Friend Function Match(ByVal filters As CSearchFilters) As Boolean
        With filters
            'Apply active filters here i.e. return false if any filter is active and excludes this record
            If Not String.IsNullOrEmpty(.Name) AndAlso Not Me.SettingName.ToLower.StartsWith(.Name) Then Return False 'Starts-with logic (trailing wildcard)
        End With
        Return True
    End Function
#End Region

#Region "Search Filters"
    'Filters i.e. UI-driven collection of potential search filters (e.g. 'name' from textbox could search firstname/lastname columns)
    Public Class CSearchFilters
        'Constructors (add more for common combinations, or introduce more classes)
        Public Sub New(ByVal groupId As Integer, ByVal typeId As Integer, ByVal listId As Integer, ByVal clientCanEditOnly As Boolean, ByVal valueBooleanOnly As Boolean, ByVal name As String)
            Me.GroupId = groupId
            Me.TypeId = typeId
            Me.ListId = listId
            Me.ClientCanEditOnly = clientCanEditOnly
            Me.ValueBooleanOnly = valueBooleanOnly
            Me.Name = name.ToLower()
        End Sub

        'Members (filter info)
        Public GroupId As Integer = Integer.MinValue
        Public TypeId As Integer = Integer.MinValue
        Public ListId As Integer = Integer.MinValue
        Public ClientCanEditOnly As Boolean = False
        Public ValueBooleanOnly As Boolean = False
        Public Name As String = String.Empty

        'Properties (checks if a complex search is required i.e. some filters are active, other than index-based ones)
        Public ReadOnly Property NonIndexedFiltersAreNull() As Boolean
            Get
                Return True 'e.g. Return String.IsNullOrEmpty(Me.Name) AndAlso ...
            End Get
        End Property
    End Class
#End Region

#Region "Caching Details"
    'Cache data
    Private Shared Function LoadCache() As CSettingList
        Return New CSetting().SelectAll()
    End Function
    'Cache Timeout
    Private Shared Sub SetCache(ByVal key As String, ByVal value As CSettingList)
        If Not IsNothing(value) Then value.Sort()
        CCache.Set(key, value) 'Optional parameter can override timeout (default is 3 days)
    End Sub
    'Helper Method
    Private Function CacheGetById(ByVal list As CSettingList) As CSetting
        Return list.GetById(Me.SettingId)
    End Function
#End Region

#Region "ToXml"
    Protected Overrides Sub ToXml_Custom(ByVal w As System.Xml.XmlWriter)
        'Store(w, "Example", Me.Example)
    End Sub
#End Region

#Region "ToString"
    Public Overrides Function ToString() As String
        Select Case Me.SettingTypeId
            Case EConfigType.Boolean
                Return Me.SettingValueBoolean.ToString
            Case EConfigType.Date
                If DateTime.MinValue = Me.SettingValueDate Then Return String.Empty
                Return Me.SettingValueDate.ToString
            Case EConfigType.Double
                If Double.IsNaN(Me.SettingValueDouble) Then Return String.Empty
                Return Me.SettingValueDouble.ToString()
            Case EConfigType.Int
                If Integer.MinValue = Me.SettingValueInteger Then Return String.Empty
                Return Me.SettingValueInteger.ToString
            Case EConfigType.String
                Return Me.SettingValueString
            Case EConfigType.ListAsInteger, EConfigType.ListAsString
                Return CList.Cache.GetText(Me)
        End Select
        Throw New Exception("New datatype: " & Me.SettingTypeId.ToString)
    End Function
#End Region

End Class
