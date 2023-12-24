Imports System
Imports System.Data
Imports System.Collections.Generic

Imports Framework

'Collection Class (Customisable half)
Partial Public Class CType_List

#Region "Search"
    'Custom (build filters using parameters supplied, based around common combinations)
    'Public Function Search(ByVal name As String) As CType_List
    '    Return Search(New CType_.CSearchFilters(name))
    'End Function

    'Standard
    Public Function Search(ByVal filters As CType_.CSearchFilters) As CType_List
        Dim results As CType_List = Me 'Start with a complete list
        
        With filters
            'Use any available index for fk/bool (e.g results=results.GetByFk)

            'Exit early if remaining filters are n/a
            If .NonIndexedFiltersAreNull Then Return results
        End With
            
        'Use match method for string searching or complex expressions
        Dim shortList As New CType_List()
        For Each i As CType_ In results
            If i.Match(filters) Then shortList.Add(i)
        Next
        Return shortList
    End Function
#End Region

End Class
