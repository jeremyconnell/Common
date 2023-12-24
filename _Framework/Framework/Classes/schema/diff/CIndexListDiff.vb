Imports System.Text

Public Class CIndexListDiff
    Public Missing As CIndexInfoList
    Public Added As CIndexInfoList
    Public Same As CIndexInfoList
    Public Different As List(Of CIndexDiff)

    Public ReadOnly Property IsExactMatch As Boolean
        Get
            Return Missing.Count = 0 AndAlso Added.Count = 0 AndAlso Different.Count = 0
        End Get
    End Property

    Friend Sub New()
        Me.Missing = New CIndexInfoList()
        Me.Added = New CIndexInfoList()
        Me.Different = New List(Of CIndexDiff)
        Me.Same = New CIndexInfoList()
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
        For Each i As CIndexInfo In Added
            list.Add(i.DropScript())
        Next

        list.Add(String.Empty)

        For Each i As CIndexInfo In Missing
            list.Add(i.CreateScript())
        Next

        list.Add(String.Empty)

        For Each i As CIndexDiff In Different
            list.Add(i.This.DropScript())
            list.Add(i.Ref.CreateScript())
        Next
        Return list
    End Function
End Class