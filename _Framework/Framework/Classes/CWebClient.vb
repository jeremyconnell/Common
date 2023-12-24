Public Class CWebClient : Inherits Net.WebClient

    Public Sub New(Optional ByVal timeoutMs As Integer = 100000)
        m_timeout = timeoutMs
    End Sub

    Private m_timeout As Integer

    Protected Overrides Function GetWebRequest(ByVal address As System.Uri) As System.Net.WebRequest
        GetWebRequest = MyBase.GetWebRequest(address)
        GetWebRequest.Timeout = m_timeout
    End Function
End Class