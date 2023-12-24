Public Class CSchemaDiff
    Public Missing As New CSchemaInfo
    Public Extra As New CSchemaInfo
    Public Same As New CSchemaInfo
    Public Diff As New CSchemaInfo

    Public IndexDiff As New CIndexListDiff
    Public ProcDiff As New CProcedureDiffList
	Public FKDiff As New CForeignKeyListDiff
	Public ViewDiff As New CViewListDiff
	Public TableDiff As New CTableListDiff

	Public Sub New(this As CSchemaInfo, ref As CSchemaInfo)
        Dim d As CSchemaDiff = this.Diff(ref)
        Me.Missing = d.Missing
        Me.Extra = d.Extra
        Me.Same = d.Same
		Me.Diff = d.Diff

		Me.IndexDiff = d.IndexDiff
        Me.ProcDiff = d.ProcDiff
		Me.FKDiff = d.FKDiff
		Me.ViewDiff = d.ViewDiff
		Me.TableDiff = d.TableDiff
	End Sub
    Friend Sub New()
    End Sub

    Public Overrides Function ToString() As String
        Dim sb As New Text.StringBuilder

        Dim m As String = Missing.ToString
        Dim e As String = Extra.ToString
        Dim d As String = Diff.ToString

        If m.Length > 0 Then
            sb.AppendLine("MISSING: ")
            sb.AppendLine(m)
            sb.AppendLine()
        End If

        If e.Length > 0 Then
            sb.AppendLine("EXTRA: ")
            sb.AppendLine(e)
            sb.AppendLine()
        End If

        If d.Length > 0 Then
            sb.AppendLine("DIFFERENT: ")
            sb.AppendLine(d)
            sb.AppendLine()
        End If

        Return sb.ToString
    End Function
End Class
