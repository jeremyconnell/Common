Imports System
Imports System.Data
Imports System.Collections.Generic

Imports Framework

'Collection Class (Customisable half)
Partial Public Class CGroupList

#Region "Alternative PK (on GroupName)"
    Default Public Overloads ReadOnly Property Item(ByVal name As String) As CGroup
        Get
            Return GetByName(name)
        End Get
    End Property
    Public Function GetByName(ByVal name As String) As CGroup
        Dim c As CGroup = Nothing
        IndexByName.TryGetValue(Normalise(name), c)
        Return c
    End Function
    <NonSerialized()> _
    Private _indexByName As Dictionary(Of String, CGroup)
    Private Function Normalise(ByVal key As String) As String
        Return LCase(Trim(key))
    End Function
    Private ReadOnly Property IndexByName() As Dictionary(Of String, CGroup)
        Get
            If Not IsNothing(_indexByName) Then
                If _indexByName.Count = Me.Count Then
                    Return _indexByName
                End If
            End If
            _indexByName = New Dictionary(Of String, CGroup)(Me.Count)
            For Each i As CGroup In Me
                _indexByName(Normalise(i.GroupName)) = i
            Next
            Return _indexByName
        End Get
    End Property
#End Region

#Region "Search"
    'Custom (build filters using parameters supplied, based around common combinations)
    'Public Function Search(ByVal name As String) As CGroupList
    '    Return Search(New CGroup.CSearchFilters(name))
    'End Function

    'Standard
    Public Function Search(ByVal filters As CGroup.CSearchFilters) As CGroupList
        Dim results As CGroupList = Me 'Start with a complete list
        
        With filters
            'Use any available index for fk/bool (e.g results=results.GetByFk)

            'Exit early if remaining filters are n/a
            If .NonIndexedFiltersAreNull Then Return results
        End With
            
        'Use match method for string searching or complex expressions
        Dim shortList As New CGroupList()
        For Each i As CGroup In results
            If i.Match(filters) Then shortList.Add(i)
        Next
        Return shortList
    End Function
#End Region

End Class
