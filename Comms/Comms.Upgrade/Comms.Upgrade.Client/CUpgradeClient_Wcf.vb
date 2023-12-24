'Some specifics about the remote svc implementation (and local bindings)
Public Class CUpgradeClient_Wcf : Inherits CUpgradeClient

#Region "Constants"
    Private Const PAGENAME As String = "Upgrade.svc"
    Friend Const DEFAULT_ENDPOINT As String = "BasicHttpBinding_IUpgrade"
#End Region

#Region "WCF Reference"
    Private m_wcf As WcfRefPassthru.PassthruClient 'Can replace with a complete interface, to enforce more wcf-ness
#End Region

#Region "Constructors" 'TODO: Make Protected, forcing use of Factory methods
    Public Sub New()
        Me.New(Config_.Endpoint)
    End Sub
    Public Sub New(endpoint As String)
        Me.New(endpoint, Config_.HostName, Config_.UseSsl)
    End Sub
    Public Sub New(endpoint As String, hostName As String, useSsl As Boolean)
        Me.New(endpoint, hostName, useSsl, Config_)
    End Sub
    Public Sub New(hostAndPassword As IConfig_Client)
        Me.New(hostAndPassword.Endpoint, hostAndPassword.HostName, hostAndPassword.UseSsl, hostAndPassword)
    End Sub
    Public Sub New(endpoint As String, hostName As String, useSsl As Boolean, password As IConfig)
        MyBase.New(hostName, useSsl, password) 'Builds Me.Url (from ip+pagename)

        If String.IsNullOrEmpty(endpoint) Then endpoint = DEFAULT_ENDPOINT

        If String.IsNullOrEmpty(Me.Url) Then
            m_wcf = New WcfRefPassthru.PassthruClient(endpoint)
        Else
            m_wcf = New WcfRefPassthru.PassthruClient(endpoint, Me.Url)
        End If
    End Sub
#End Region

#Region "MustOverride"
    Protected Overrides ReadOnly Property RelativePath As String
        Get
            Return PAGENAME
        End Get
    End Property
#End Region

#Region "WCF Implementation"
    Protected Overrides Function Transport(enum_ As Integer, input() As Byte, gzip As EGzip, encrypt As EEncryption, formatIn As ESerialisation, formatOut As ESerialisation) As Byte()
        Return m_wcf.TransportInterface(enum_, input, gzip, encrypt, formatIn, formatOut)
    End Function
    Protected Overrides Async Function TransportAsync(enum_ As Integer, input() As Byte, gzip As EGzip, encrypt As EEncryption, formatIn As ESerialisation, formatOut As ESerialisation) As Threading.Tasks.Task(Of Byte())
        'TODO - proper async
        Return Await Threading.Tasks.Task.Run(Function() m_wcf.TransportInterface(enum_, input, gzip, encrypt, formatIn, formatOut))
    End Function
#End Region

End Class