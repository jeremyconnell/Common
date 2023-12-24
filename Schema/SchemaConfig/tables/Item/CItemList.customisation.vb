Imports System
Imports System.Data
Imports System.Collections.Generic

Imports Framework

'Collection Class (Customisable half)
Partial Public Class CItemList

#Region "Subsets"
    Public ReadOnly Property Active() As CItemList
        Get
            Return GetByIsDeleted(False)
        End Get
    End Property
#End Region

#Region "Search"
    'Custom (build filters using parameters supplied, based around common combinations)
    'Public Function Search(ByVal name As String) As CItemList
    '    Return Search(New CItem.CSearchFilters(name))
    'End Function

    'Standard
    Public Function Search(ByVal filters As CItem.CSearchFilters) As CItemList
        Dim results As CItemList = Me 'Start with a complete list
        
        With filters
            'Use any available index for fk/bool (e.g results=results.GetByFk)
            If .ListId <> Integer.MinValue Then results = results.GetByListId(.ListId)

            'Exit early if remaining filters are n/a
            If .NonIndexedFiltersAreNull Then Return results
        End With
            
        'Use match method for string searching or complex expressions
        Dim shortList As New CItemList()
        For Each i As CItem In results
            If i.Match(filters) Then shortList.Add(i)
        Next
        Return shortList
    End Function
#End Region

End Class
