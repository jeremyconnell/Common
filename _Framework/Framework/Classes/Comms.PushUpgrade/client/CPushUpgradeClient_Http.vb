'Some specifics about the remote aspx implementation
'Same baseurl, different hostnames, ssl option (password is better)
Public Class CPushUpgradeClient_Http : Inherits CPushUpgradeClient

#Region "Constants (all other sections are template)"
    Private Const PAGENAME As String = "webservices/PushUpgrade.ashx"
    Public Const DEFAULT_TIMEOUT_MS As Integer = 15 * 60 * 1000 '15mins timeout for upgrades
#End Region

#Region "Properties"
    'Private m_wc As Net.WebClient
    Private m_timeout As Integer
#End Region

#Region "Constructors" 'TODO: Make Protected, forcing use of Factory methods
    'Config-driven host/pass
    Public Sub New()
        Me.New(Config_)
    End Sub

    'explicit host/ssl; config-driven password
    Public Sub New(hostName As String, useSsl As Boolean, Optional timeoutMs As Integer = DEFAULT_TIMEOUT_MS)
        Me.New(hostName, useSsl, timeoutMs, Config_)
    End Sub

    'Explicit host/ssl/password
    Public Sub New(hostName As String, useSsl As Boolean, password As IConfig_Client)
        Me.New(hostName, useSsl, DEFAULT_TIMEOUT_MS, password)
    End Sub
    Public Sub New(hostAndPass As IConfig_Client, Optional timeoutMs As Integer = DEFAULT_TIMEOUT_MS)
        Me.New(hostAndPass.HostName, hostAndPass.UseSsl, timeoutMs, hostAndPass)
    End Sub


    'Explicit host/ssl/timeout/pass
    Public Sub New(hostName As String, useSsl As Boolean, timeoutMs As Integer, password As IConfig_Client)
        MyBase.New(hostName, useSsl, password) 'HostName+RelativePath sets Me.Url
        m_timeout = timeoutMs
    End Sub
#End Region

#Region "Overrides (RelativePath)"
    Protected Overrides ReadOnly Property RelativePath As String
        Get
            Return PAGENAME
        End Get
    End Property
#End Region

#Region "REST Implementation"
    Protected Overrides Function Transport(enum_ As Integer, input() As Byte, gzip As EGzip, encrypt As EEncryption, formatIn As ESerialisation, formatOut As ESerialisation) As Byte()
        Return REST_UploadDownload(New CWebClient(m_timeout), Me.Url, enum_, input, gzip, encrypt, formatIn, formatOut, Config)
    End Function
    Protected Overrides Async Function TransportAsync(enum_ As Integer, input() As Byte, gzip As EGzip, encrypt As EEncryption, formatIn As ESerialisation, formatOut As ESerialisation) As Threading.Tasks.Task(Of Byte())
        Return Await REST_UploadDownloadAsync(New CWebClient(m_timeout), Me.Url, enum_, input, gzip, encrypt, formatIn, formatOut, Config)
    End Function
#End Region


End Class
