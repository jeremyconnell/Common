'Some specifics about the remote aspx implementation
Public Class CPushUpgradeClient_Http : Inherits CPushUpgradeClient

#Region "Constants (all other sections are template)"
    Private Const PAGENAME As String = "webservices/PushUpgrade.ashx"
    Friend Const DEFAULT_TIMEOUT_MS As Integer = 5 * 60 * 1000 '5mins timeout for upgrades
#End Region

#Region "Properties"
    Private m_wc As Net.WebClient
#End Region

#Region "Constructors" 'TODO: Make Protected, forcing use of Factory methods
    Public Sub New()
        Me.New(Config_.HostName, Config_.UseSsl)
    End Sub
    Public Sub New(hostName As String, useSsl As Boolean)
        Me.New(hostName, useSsl, DEFAULT_TIMEOUT_MS)
    End Sub
    Public Sub New(hostName As String, useSsl As Boolean, timeoutMs As Integer)
        Me.New(hostName, useSsl, timeoutMs, Config_)
    End Sub
    Public Sub New(hostName As String, useSsl As Boolean, password As IConfig_Client)
        Me.New(hostName, useSsl, DEFAULT_TIMEOUT_MS, password)
    End Sub
    Public Sub New(hostAndPassword As IConfig_Client)
        Me.New(hostAndPassword.HostName, hostAndPassword.UseSsl, DEFAULT_TIMEOUT_MS)
    End Sub
    Public Sub New(hostAndPassword As IConfig_Client, timeoutMs As Integer)
        Me.New(hostAndPassword.HostName, timeoutMs, hostAndPassword)
    End Sub
    Public Sub New(hostName As String, useSsl As Boolean, timeoutMs As Integer, password As IConfig)
        MyBase.New(hostName, useSsl, password) 'HostName+RelativePath sets Me.Url
        m_wc = REST_Client(timeoutMs)
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
        Return REST_UploadDownload(m_wc, Me.Url, enum_, input, gzip, encrypt, formatIn, formatOut, Config)
    End Function
    Protected Overrides Async Function TransportAsync(enum_ As Integer, input() As Byte, gzip As EGzip, encrypt As EEncryption, formatIn As ESerialisation, formatOut As ESerialisation) As Task(Of Byte())
        Return Await REST_UploadDownloadAsync(m_wc, Me.Url, enum_, input, gzip, encrypt, formatIn, formatOut, Config)
    End Function
#End Region


End Class
