Public Class CProcedureDiff
    Public Sub New(this As CProcedure, ref As CProcedure)
        Me.This = this
        Me.Ref = ref
        Me.NameIsDiff = this.Name <> ref.Name
        Me.MD5IsDiff = this.MD5 <> ref.MD5
        Me.IsStoredProcIsDiff = this.IsStoredProc <> ref.IsStoredProc
    End Sub

    Public This As CProcedure
    Public Ref As CProcedure
    Public NameIsDiff As Boolean
    Public MD5IsDiff As Boolean
    Public IsStoredProcIsDiff As Boolean


    Public ReadOnly Property AnyDiffs As Boolean
        Get
            Return NameIsDiff OrElse MD5IsDiff OrElse IsStoredProcIsDiff
        End Get
    End Property
    Public Function TextDiff() As String
        Dim one As List(Of String) = CUtilities.SplitOn(This.Text, vbCrLf)
        Dim two As List(Of String) = CUtilities.SplitOn(Ref.Text, vbCrLf)
        Dim maxRows As Integer = one.Count
        If two.Count > maxRows Then maxRows = two.Count
        Dim maxCols As Integer = 0
        For Each i As String In one
            If i.Length > maxCols Then maxCols = i.Length
        Next
        Dim list As New List(Of String)
        For i As Integer = 0 To maxRows - 1
            Dim a As String = String.Empty
            Dim b As String = String.Empty
            If one.Count > i Then a = one(i).Replace(vbTab, "    ")
            If two.Count > i Then b = two(i).Replace(vbTab, "    ")
            Dim sb As New Text.StringBuilder(a)
            While sb.Length < maxCols
                sb.Append(" ")
            End While
            If a = b Then
                sb.Insert(0, "# ")
            Else
                sb.Insert(0, "* ")
            End If
            list.Add(String.Concat(sb, " | ", b))
        Next
        Return CUtilities.ListToString(list, vbCrLf)
    End Function

    Public Overrides Function ToString() As String
        If Not AnyDiffs Then Return "None"

        Dim list As New List(Of String)
        If IsStoredProcIsDiff Then list.Add(String.Concat("Type is different: ", This.IsStoredProc, "<>", Ref.IsStoredProc))
        If NameIsDiff Then list.Add(String.Concat("Name is different: ", This.Name, "<>", Ref.Name))
        If MD5IsDiff Then list.Add(String.Concat("Text is different: ", This.Name, vbCrLf, TextDiff))
        Return CUtilities.ListToString(list)
    End Function
End Class