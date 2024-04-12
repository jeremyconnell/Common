Public Class CForeignKeyListDiff
    Public Missing As CForeignKeyList
    Public Added As CForeignKeyList
    Public Same As CForeignKeyList
    Public Different As List(Of CForeignKeyDiff)


    Friend Sub New()
        Me.Missing = New CForeignKeyList()
        Me.Added = New CForeignKeyList()
        Me.Same = New CForeignKeyList()
        Me.Different = New List(Of CForeignKeyDiff)
    End Sub


	Public ReadOnly Property IsExactMatch As Boolean
		Get
			Return Missing.Count + Added.Count + Different.Count = 0
		End Get
	End Property

	Public Overrides Function ToString() As String
        Dim s As New List(Of String)
        If Missing.Count > 0 Then s.Add(String.Concat("Missing: ", Missing.Count))
        If Added.Count > 0 Then s.Add(String.Concat("Added: ", Added.Count))
        If Same.Count > 0 Then s.Add(String.Concat("Same: ", Same.Count))
        If Different.Count > 0 Then s.Add(String.Concat("Different: ", Different.Count))
        For Each i As CForeignKeyDiff In Different
            s.Add(i.ToString)
        Next
        Return CUtilities.ListToString(s, vbCrLf)
    End Function

    Public Function ChangeScriptsAsString() As String
        Return CUtilities.ListToString(ChangeScripts(), vbCrLf)
    End Function
    Public Function ChangeScripts() As List(Of String)
        Dim list As New List(Of String)
        For Each i As CForeignKey In Added
            list.Add(i.DropScript())
        Next

        list.Add(String.Empty)

        For Each i As CForeignKey In Missing
            list.Add(i.CreateScript())
        Next

        list.Add(String.Empty)

        For Each i As CForeignKeyDiff In Different
            list.Add(i.This.DropScript())
            list.Add(i.Ref.CreateScript())
        Next
        Return list
    End Function
End Class
