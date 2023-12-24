Imports System
Imports System.Data
Imports System.Collections.Generic

Imports Framework

'Collection Class (Customisable half)
Partial Public Class CListList

#Region "Get Text/Value"
    Public Function GetText(ByVal setting As CSetting) As String
        With setting
            If Integer.MinValue = .SettingValueInteger Then Return String.Empty
            If IsNothing(.List) Then Return "Setting is not associated with a list"
            If Not .List.ListIsExternal Then
                Dim item As CItem = CItem.Cache.GetById(.SettingValueInteger)
                If IsNothing(item) Then Return String.Empty
                Return item.ItemName
            End If
            Try
                With .List
                    Dim sql As String = .ExternalSqlToSelectById(setting.SettingValueInteger)
                    Dim obj As Object = .ExternalDataSrc.ExecuteScalar(sql, Nothing)
                    If TypeOf obj Is DBNull Then Return String.Empty
                    Return Convert.ToString(obj)
                End With
            Catch ex As Exception
                Return ex.Message
            End Try
        End With
    End Function
#End Region

#Region "Search"
    'Custom (build filters using parameters supplied, based around common combinations)
    'Public Function Search(ByVal name As String) As CListList
    '    Return Search(New CList.CSearchFilters(name))
    'End Function

    'Standard
    Public Function Search(ByVal filters As CList.CSearchFilters) As CListList
        Dim results As CListList = Me 'Start with a complete list
        
        With filters
            'Use any available index for fk/bool (e.g results=results.GetByFk)
            If .IsExternalOnly Then results = results.GetByIsExternal(True)   'Customise bool filters to suit UI (e.g. true/false/both tristate)

            'Exit early if remaining filters are n/a
            If .NonIndexedFiltersAreNull Then Return results
        End With
            
        'Use match method for string searching or complex expressions
        Dim shortList As New CListList()
        For Each i As CList In results
            If i.Match(filters) Then shortList.Add(i)
        Next
        Return shortList
    End Function
#End Region

End Class
