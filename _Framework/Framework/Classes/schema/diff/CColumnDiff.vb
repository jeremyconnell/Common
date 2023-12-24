Public Class CColumnDiff
    'Constructor
    Public Sub New(this As CColumnList, ref As CColumnList)
        Me.This = this
        Me.Ref = ref

        Me.Same = New CColumnList()
        Me.Diff = New CColumnList()
        Me.Missing = New CColumnList()
        Me.Extra = New CColumnList()

        For Each i As CColumn In this
            If Not ref.Has(i.Name) Then Extra.Add(i)
        Next
        For Each i As CColumn In ref
            If Not this.Has(i.Name) Then
                Missing.Add(i)
                Continue For
            End If

            Dim rCol As CColumn = this.Item(i.Name)
            If rCol.MD5.Equals(i.MD5) Then
                Same.Add(i)
            Else
                Diff.Add(i)
            End If
        Next
    End Sub

    'Diff Info
    Public Same As CColumnList
    Public Diff As CColumnList 'Type or IsNull
    Public Missing As CColumnList
    Public Extra As CColumnList

    Public This As CColumnList
    Public Ref As CColumnList

    Public ReadOnly Property IsExact As Boolean
        Get
            Return Diff.Count + Missing.Count + Extra.Count = 0
        End Get
    End Property

    'Presentation Logic
    Public Overrides Function ToString() As String
        If Same.Count = This.Count Then Return CUtilities.NameAndCount("*Exact Match", Same, "col")

        Dim list As New List(Of String)
		If Same.Count > 0 Then list.Add(CUtilities.NameAndCount("Same", Same))
		If Extra.Count > 0 Then list.Add(CUtilities.NameAndCount("Extra", Extra) & ": " & Diff.NamesAbc_)
		If Missing.Count > 0 Then list.Add(CUtilities.NameAndCount("Missing", Missing) & ": " & Diff.NamesAbc_)
		If Diff.Count > 0 Then list.Add(CUtilities.NameAndCount("Diff", Diff) & ": " & Diff.NamesAbc_)
		Return CUtilities.ListToString(list)
    End Function
End Class
