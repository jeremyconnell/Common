Public Class CViewListDiff
    Public Missing As CViewInfoList
    Public Added As CViewInfoList
    Public Same As CViewInfoList
    Public Different As List(Of CViewDiff)

    Public ReadOnly Property IsExactMatch As Boolean
        Get
            Return Missing.Count = 0 AndAlso Added.Count = 0 AndAlso Different.Count = 0
        End Get
    End Property

	Friend Sub New()
		Me.Missing = New CViewInfoList()
		Me.Added = New CViewInfoList()
		Me.Same = New CViewInfoList()
        Me.Different = New List(Of CViewDiff)
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
    Public Function ChangeScripts() As List(Of String)
        Dim list As New List(Of String)
        For Each i As CViewInfo In Added
            list.Add(i.DropScript())
        Next

        For Each i As CViewInfo In Missing
            list.Add(i.CreateScript())
        Next

        For Each i As CViewDiff In Different
            If i.NameIsDifferent AndAlso Not i.ScriptIsDifferent Then
            Else
                list.Add(i.This.DropScript())
                list.Add(i.Ref.CreateScript())
            End If
        Next
        Return list
    End Function
End Class
