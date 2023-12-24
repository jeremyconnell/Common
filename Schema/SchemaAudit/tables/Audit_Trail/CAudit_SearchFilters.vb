<CLSCompliant(True)> Public Class CAudit_SearchFilters
    'Indexed
    Public Table As String = String.Empty
    Public PrimaryKey As String = String.Empty

    'Not Indexed
    Public TypeId As Integer = Integer.MinValue
    Public Url As String = String.Empty
    Public Login As String = String.Empty
    Public SearchDate As DateTime = Nothing

    Private m_custom As New Dictionary(Of String, String)
    Public ReadOnly Property Custom() As Dictionary(Of String, String)
        Get
            Return m_custom
        End Get
    End Property
End Class
