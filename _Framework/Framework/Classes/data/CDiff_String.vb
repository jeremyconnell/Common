Imports System.Text

<CLSCompliant(True)>
Public Class CDiff_String
    'Data
    Public SourceOnly As List(Of String)
    Public TargetOnly As List(Of String)
    Public Matching As List(Of String)

    Public Sub New()
        SourceOnly = New List(Of String)
        TargetOnly = New List(Of String)
        Matching = New List(Of String)
    End Sub

    Public Overrides Function GetHashCode() As Integer
        Return ToString.GetHashCode()
    End Function
    Public Overrides Function ToString() As String
        Dim sb As New StringBuilder()
        For Each j As String In Me.Matching
            sb.AppendLine("Updating: " + j)
        Next
        For Each j As String In Me.SourceOnly
            sb.AppendLine("Adding: " + j)
        Next
        For Each j As String In Me.TargetOnly
            sb.AppendLine("Deleting: " + j)
        Next
        Return sb.ToString
    End Function

End Class
