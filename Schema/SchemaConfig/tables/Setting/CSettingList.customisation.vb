Imports System
Imports System.Data
Imports System.Collections.Generic

Imports Framework

'Collection Class (Customisable half)
Partial Public Class CSettingList

#Region "Get - Utilities"
    'Throw exception if missing
    Public Function GetBool(ByVal key As String) As Boolean
        Return GetSetting(key).SettingValueBoolean
    End Function
    Public Function GetDbl(ByVal key As String) As Double
        Return GetSetting(key).SettingValueDouble
    End Function
    Public Function GetDate(ByVal key As String) As DateTime
        Return GetSetting(key).SettingValueDate
    End Function
    Public Function GetInt(ByVal key As String) As Integer
        Return GetSetting(key).SettingValueInteger
    End Function
    Public Function GetStr(ByVal key As String) As String
        Return GetSetting(key).SettingValueString
    End Function
    Public Function GetEmail(ByVal key As String) As String
        Return GetSetting(key).SettingValueString
    End Function
    Public Function GetMoney(ByVal key As String) As Decimal
        Return GetSetting(key).SettingValueMoney
    End Function
    Public Function GetListValueInt(ByVal key As String) As Decimal
        Return GetSetting(key).SettingValueInteger
    End Function

    'Create if missing
    Public Function GetBoolOrCreate(ByVal key As String, Optional ByVal defaultValue As Boolean = False, Optional ByVal group As String = CGroup.DEFAULT, Optional ByVal clientCanEdit As Boolean = True) As Boolean
        Return GetSetting(key, EConfigType.Boolean, defaultValue, group, clientCanEdit).SettingValueBoolean
    End Function
    Public Function GetDblOrCreate(ByVal key As String, Optional ByVal defaultValue As Double = Double.NaN, Optional ByVal group As String = CGroup.DEFAULT, Optional ByVal clientCanEdit As Boolean = True) As Double
        Return GetSetting(key, EConfigType.Double, defaultValue, group, clientCanEdit).SettingValueDouble
    End Function
    Public Function GetDateOrCreate(ByVal key As String, Optional ByVal defaultValue As DateTime = Nothing, Optional ByVal group As String = CGroup.DEFAULT, Optional ByVal clientCanEdit As Boolean = True) As DateTime
        Return GetSetting(key, EConfigType.Date, defaultValue, group, clientCanEdit).SettingValueDate
    End Function
    Public Function GetIntOrCreate(ByVal key As String, Optional ByVal defaultValue As Integer = 0, Optional ByVal group As String = CGroup.DEFAULT, Optional ByVal clientCanEdit As Boolean = True) As Integer
        Return GetSetting(key, EConfigType.Int, defaultValue, group, clientCanEdit).SettingValueInteger
    End Function
    Public Function GetStrOrCreate(ByVal key As String, Optional ByVal defaultValue As String = "", Optional ByVal group As String = CGroup.DEFAULT, Optional ByVal clientCanEdit As Boolean = True) As String
        Return GetSetting(key, EConfigType.String, defaultValue, group, clientCanEdit).SettingValueString
    End Function
    Public Function GetMoneyOrCreate(ByVal key As String, Optional ByVal defaultValue As Decimal = 0, Optional ByVal group As String = CGroup.DEFAULT, Optional ByVal clientCanEdit As Boolean = True) As Decimal
        Return GetSetting(key, EConfigType.Money, defaultValue, group, clientCanEdit).SettingValueMoney
    End Function
    Public Function GetListIntOrCreate(ByVal key As String, ByVal list As EList, Optional ByVal defaultValue As Integer = Integer.MinValue, Optional ByVal group As String = CGroup.DEFAULT, Optional ByVal clientCanEdit As Boolean = True) As Integer
        Return GetSetting(key, EConfigType.ListAsInteger, defaultValue, group, clientCanEdit, list).SettingValueInteger
    End Function
    Public Function GetListStrOrCreate(ByVal key As String, ByVal list As EList, Optional ByVal defaultValue As Integer = Integer.MinValue, Optional ByVal group As String = CGroup.DEFAULT, Optional ByVal clientCanEdit As Boolean = True) As String
        Return GetSetting(key, EConfigType.ListAsString, defaultValue, group, clientCanEdit, list).SettingValueString
    End Function

    'Missing setting? Options are throw exception or auto-create
    Private Function GetSetting(ByVal key As String) As CSetting
        Dim setting As CSetting = GetByName(key)
        If IsNothing(setting) Then Throw New Exception("Setting not found: " & key)
        Return setting
    End Function
    Private Function GetSetting(ByVal key As String, ByVal type As EConfigType, ByVal defaultValue As Object, ByVal groupName As String, ByVal clientCanEdit As Boolean, Optional ByVal listId As Integer = Integer.MinValue) As CSetting
        Dim setting As CSetting = GetByName(key)
        If IsNothing(setting) Then
            'Default Group (if none were created)
            Dim group As CGroup = CGroup.Cache.GetByName(groupName)
            If IsNothing(group) Then
                group = New CGroup
                group.GroupName = groupName
                group.Save()
            End If

            setting = New CSetting()
            With setting
                'Config setting definition
                .SettingName = key
                .SettingTypeId = type
                .SettingListId = listId

                'Standard defaults - Developer can regroup or repermission later
                .SettingGroupId = group.GroupId
                .SettingClientCanEdit = clientCanEdit

                'Default values
                If Not IsNothing(defaultValue) Then
                    Select Case type
                        Case EConfigType.Boolean : .SettingValueBoolean = CBool(defaultValue)
                        Case EConfigType.Date : .SettingValueDate = CDate(defaultValue)
                        Case EConfigType.Double : .SettingValueDouble = CDbl(defaultValue)
                        Case EConfigType.Int : .SettingValueInteger = CInt(defaultValue)
                        Case EConfigType.Money : .SettingValueMoney = CDec(defaultValue)
                        Case EConfigType.String : .SettingValueString = CStr(defaultValue)
                    End Select
                End If

                .Save()
            End With

        End If
        Return setting
    End Function
#End Region

#Region "Alternative PK (on SettingName)"
    Default Public Overloads ReadOnly Property Item(ByVal name As String) As CSetting
        Get
            Return GetByName(name)
        End Get
    End Property
    Public Function GetByName(ByVal name As String) As CSetting
        Dim c As CSetting = Nothing
        IndexByName.TryGetValue(Normalise(name), c)
        Return c
    End Function
    <NonSerialized()> _
    Private _indexByName As Dictionary(Of String, CSetting)
    Private Function Normalise(ByVal key As String) As String
        Return LCase(Trim(key))
    End Function
    Private ReadOnly Property IndexByName() As Dictionary(Of String, CSetting)
        Get
            If Not IsNothing(_indexByName) Then
                If _indexByName.Count = Me.Count Then
                    Return _indexByName
                End If
            End If
            _indexByName = New Dictionary(Of String, CSetting)(Me.Count)
            For Each i As CSetting In Me
                _indexByName(Normalise(i.SettingName)) = i
            Next
            Return _indexByName
        End Get
    End Property
#End Region

#Region "Search"
    'Custom (build filters using parameters supplied, based around common combinations)
    'Public Function Search(ByVal name As String) As CSettingList
    '    Return Search(New CSetting.CSearchFilters(name))
    'End Function

    'Standard
    Public Function Search(ByVal filters As CSetting.CSearchFilters) As CSettingList
        Dim results As CSettingList = Me 'Start with a complete list

        With filters
            'Use any available index for fk/bool (e.g results=results.GetByFk)
            If .GroupId <> Integer.MinValue Then results = results.GetByGroupId(.GroupId)
            If .TypeId <> Integer.MinValue Then results = results.GetByTypeId(.TypeId)
            If .ListId <> Integer.MinValue Then results = results.GetByListId(.ListId)
            If .ClientCanEditOnly Then results = results.GetByClientCanEdit(True) 'Customise bool filters to suit UI (e.g. true/false/both tristate)
            If .ValueBooleanOnly Then results = results.GetByValueBoolean(True) 'Customise bool filters to suit UI (e.g. true/false/both tristate)

            'Exit early if remaining filters are n/a
            If .NonIndexedFiltersAreNull Then Return results
        End With

        'Use match method for string searching or complex expressions
        Dim shortList As New CSettingList()
        For Each i As CSetting In results
            If i.Match(filters) Then shortList.Add(i)
        Next
        Return shortList
    End Function
#End Region

End Class
