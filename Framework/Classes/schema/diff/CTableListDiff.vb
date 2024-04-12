Public Class CTableListDiff
    Public Missing As CTableInfoList
    Public Added As CTableInfoList
    Public Same As CTableInfoList
    Public Different As List(Of CTableDiff)

    Public ReadOnly Property IsExactMatch As Boolean
        Get
            Return Missing.Count = 0 AndAlso Added.Count = 0 AndAlso Different.Count = 0
        End Get
    End Property

	Friend Sub New()
		Me.Missing = New CTableInfoList()
		Me.Added = New CTableInfoList()
		Me.Same = New CTableInfoList()
		Me.Different = New List(Of CTableDiff)
	End Sub



	Public Overrides Function ToString() As String
		Dim s As New List(Of String)
		If Missing.Count > 0 Then s.Add(String.Concat("Missing: ", Missing.Count))
		If Added.Count > 0 Then s.Add(String.Concat("Added: ", Added.Count))
		If Same.Count > 0 Then s.Add(String.Concat("Same: ", Same.Count))
		If Different.Count > 0 Then s.Add(String.Concat("Different: ", Different.Count))
		For i As Integer = 0 To Different.Count - 1
			s.Add(String.Concat("#", CStr(1 + i), ". ", Different(i)))
		Next
		Return CUtilities.ListToString(s, vbCrLf)
	End Function

	Public Function ChangeScriptsAsString() As String
		Return CUtilities.ListToString(ChangeScripts(), vbCrLf)
	End Function
	Public Function ChangeScripts(Optional isNew As Boolean? = Nothing) As List(Of String)
		If isNew.HasValue Then
			Return ChangeScripts(Not isNew.Value, isNew.Value)
		Else
			Return ChangeScripts(True, True)
		End If
	End Function
	Public Function ChangeScripts(drop As Boolean, create As Boolean) As List(Of String)
		Dim list As New List(Of String)
		If create Then
			For Each i As CTableInfo In Added
				list.Add(i.DropScript())
			Next
		End If

		list.Add(String.Empty)

		For Each i As CTableInfo In Missing
			list.Add(i.CreateScript())
		Next

		list.Add(String.Empty)

		If drop Then
			For Each i As CTableDiff In Different
				list.AddRange(i.ChangeScripts(Missing, Added))
			Next
		End If
		Return list
	End Function
End Class
