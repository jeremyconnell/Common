Imports System.Threading.Tasks

Public MustInherit Class CClient_RestOrWcf : Implements IPassthru

#Region "Properties"
    Public ReadOnly Property Config As IConfig_Client
        Get
            Return m_config
        End Get
    End Property
    Public Property Url As String

    Private m_config As IConfig_Client
    Private m_defaultZip As EGzip
    Private m_defaultFormat As ESerialisation
#End Region

#Region "Constructors"
    'WCF & Rest both have a url (ip or subdomain supplied, but rest of url is hardcoded)
    'WCF (derived class) can also have an endpoint
    Private Sub New(zip As EGzip, format As ESerialisation)
        Me.m_defaultZip = zip
        Me.m_defaultFormat = format
    End Sub
    Protected Sub New(c As IConfig_Client, Optional zip As EGzip = EGzip.None, Optional format As ESerialisation = ESerialisation.DataContract)
        Me.New(c.HostName, c.UseSsl, c, zip, format)
    End Sub
    Protected Sub New(hostName As String, useSsl As Boolean, password As IConfig_Client, Optional zip As EGzip = EGzip.None, Optional format As ESerialisation = ESerialisation.DataContract)
        Me.New(zip, format)

        'Validate hostname
        If String.IsNullOrEmpty(hostName) Then Throw New Exception("HostName must be supplied")


        If hostName.StartsWith("http://") Or hostName.StartsWith("https://") Then
            'Use url as-is
            Me.Url = hostName
        Else
            'Build url
            Dim h As String = CStr(IIf(useSsl, "https://", "http://"))
            Me.Url = String.Concat(h, hostName, "/", RelativePath)
        End If

        'Password, +choice of encryption method
        m_config = password
    End Sub
#End Region

#Region "MustOverride"
    'Name of aspx/svc page (can unlock this by using full urls instead, but this is generally cleaner)
    Protected MustOverride ReadOnly Property RelativePath As String

    'Binary exchange, with one or more params
    Protected MustOverride Function Transport(enum_ As Integer, input As Byte(), gzip As EGzip, algorithm As EEncryption, formatIn As ESerialisation, formatOut As ESerialisation) As Byte() Implements IPassthru.TransportInterface

    'Async version
    Protected MustOverride Function TransportAsync(enum_ As Integer, input As Byte(), gzip As EGzip, algorithm As EEncryption, formatIn As ESerialisation, formatOut As ESerialisation) As Threading.Tasks.Task(Of Byte())
#End Region

#Region "RemoteCall Helpers - everything except deserialisation of the output (which requires knowledge of the type)"


    'Overloads: defaults from config/constructor settings
    Protected Function Invoke(methodNameEnum As Integer, ParamArray params As Object()) As Byte() 'Most common overload (compression/encryption is config-driven, and format is always protobuf)
        Return Invoke(methodNameEnum, m_defaultZip, params) 'Gzip Default
    End Function
    Protected Function Invoke(methodNameEnum As Integer, gzip As EGzip, ParamArray params As Object()) As Byte() 'Method-level compression options (occasionally want to control that at the method-level)
        Return Invoke(methodNameEnum, gzip, m_defaultFormat, m_defaultFormat, params) 'Serialisation defaults (in)
    End Function
    Protected Function Invoke(methodNameEnum As Integer, gzip As EGzip, formatIn As ESerialisation, ParamArray params As Object()) As Byte()  'Method-level compression+encryption options (rarely used)
        Return Invoke(methodNameEnum, gzip, formatIn, m_defaultFormat, Config.Encryption, params) 'Serialisation defaults (out)
    End Function
    Protected Function Invoke(methodNameEnum As Integer, gzip As EGzip, formatIn As ESerialisation, formatOut As ESerialisation, ParamArray params As Object()) As Byte()  'Method-level compression+encryption options (rarely used)
        Return Invoke(methodNameEnum, gzip, formatIn, formatOut, Config.Encryption, params) 'Encryption defaults
    End Function
    Protected Function Invoke(methodNameEnum As Integer, gzip As EGzip, formatIn As ESerialisation, formatOut As ESerialisation, encryption As EEncryption, ParamArray params As Object()) As Byte() 'encryption parameter is last here, as that is the least likely to change
        Return InvokeGeneric(methodNameEnum, gzip, formatIn, formatOut, encryption, params) 'Full version of the method
    End Function

    'Overloads: Async versions of above
    Protected Async Function InvokeAsync(methodNameEnum As Integer, ParamArray params As Object()) As Task(Of Byte()) 'Most common overload (compression/encryption is config-driven, and format is always protobuf)
        Return Await InvokeAsync(methodNameEnum, m_defaultZip, params) 'Gzip Default
    End Function
    Protected Async Function InvokeAsync(methodNameEnum As Integer, gzip As EGzip, ParamArray params As Object()) As Task(Of Byte()) 'Method-level compression options (occasionally want to control that at the method-level)
        Return Await InvokeAsync(methodNameEnum, gzip, m_defaultFormat, m_defaultFormat, params) 'Serialisation defaults (in)
    End Function
    Protected Async Function InvokeAsync(methodNameEnum As Integer, gzip As EGzip, formatIn As ESerialisation, ParamArray params As Object()) As Task(Of Byte())  'Method-level compression+encryption options (rarely used)
        Return Await InvokeAsync(methodNameEnum, gzip, formatIn, m_defaultFormat, Config.Encryption, params) 'Serialisation defaults (out)
    End Function
    Protected Async Function InvokeAsync(methodNameEnum As Integer, gzip As EGzip, formatIn As ESerialisation, formatOut As ESerialisation, ParamArray params As Object()) As Task(Of Byte())  'Method-level compression+encryption options (rarely used)
        Return Await InvokeAsync(methodNameEnum, gzip, formatIn, formatOut, Config.Encryption, params) 'Encryption defaults
    End Function
    Protected Async Function InvokeAsync(methodNameEnum As Integer, gzip As EGzip, formatIn As ESerialisation, formatOut As ESerialisation, encryption As EEncryption, ParamArray params As Object()) As Task(Of Byte()) 'encryption parameter is last here, as that is the least likely to change
        Return Await InvokeGenericAsync(methodNameEnum, gzip, formatIn, formatOut, encryption, params) 'Full version of the method
    End Function

    Public ReadOnly Property Deserialiser As CSerialise
        Get
            Select Case m_defaultFormat
                Case ESerialisation.BinarySer : Return CSerialise.Old
                Case ESerialisation.DataContract : Return CSerialise.DC
                Case ESerialisation.Protobuf : Return CSerialise.Proto
            End Select
            Throw New Exception("TODO: support " & m_defaultFormat.ToString & " serialisation")
        End Get
    End Property
#End Region

#Region "Implementation (Compression/Encryption)"
    Protected Function InvokeGeneric(enum_ As Integer, gzip As EGzip, formatIn As ESerialisation, formatOut As ESerialisation, encryption As EEncryption, params As Object()) As Byte()
        'Serialise, Compress, Encrypt
        Dim request As Byte() = CSerialise.Pack(params, formatIn)   'Double-serialise
        request = CGzip.Compress(request, gzip, True)
        request = Config.Encrypt(request, encryption)

        'Request/Response
        Dim response As Byte() = Transport(enum_, request, gzip, encryption, formatIn, formatOut)

        'Decrypt, Decompress (but no deserialise, type/enum dependant)
        response = Config.Decrypt(response, encryption)
        response = CGzip.Decompress(response, gzip, False)

        'Try deserialse as an exception
        Dim ex As CException = Nothing
        Try
            ex = CSerialise.Deserialise(Of CException)(response, formatOut)
        Catch
        End Try
        If Not IsNothing(ex) AndAlso Not IsNothing(ex.Message) Then Throw New CDeserialisedException(ex)

        Return response
    End Function
    Protected Async Function InvokeGenericAsync(enum_ As Integer, gzip As EGzip, formatIn As ESerialisation, formatOut As ESerialisation, encryption As EEncryption, params As Object()) As Task(Of Byte())
        'Serialise, Compress, Encrypt
        Dim request As Byte() = CSerialise.Pack(params, formatIn)   'Double-serialise
        request = CGzip.Compress(request, gzip, True)
        request = Config.Encrypt(request, encryption)

        'Request/Response
        Dim response As Byte() = Await TransportAsync(enum_, request, gzip, encryption, formatIn, formatOut)

        'Decrypt, Decompress (but no deserialise, type/enum dependant)
        response = Config.Decrypt(response, encryption)
        response = CGzip.Decompress(response, gzip, False)

        'Try deserialse as an exception
        Dim ex As CException = Nothing
        Try
            ex = CSerialise.Deserialise(Of CException)(response, formatOut)
        Catch
        End Try
        If Not IsNothing(ex) AndAlso Not IsNothing(ex.Message) Then Throw New CDeserialisedException(ex)

        Return response
    End Function
#End Region

#Region "Shared - REST helper"
    Public Shared Function REST_Client(timeoutMs As Integer) As Net.WebClient
        Return New CWebClient(timeoutMs)
    End Function

    'Timeout Overloads
    Public Shared Function REST_UploadDownload(timeoutMs As Integer, url As String, enum_ As Integer, data As Byte(), gzip As EGzip, encryption As EEncryption, formatIn As ESerialisation, formatOut As ESerialisation, password As IConfig) As Byte()
        Using wc As New CWebClient(timeoutMs)
            Return REST_UploadDownload(wc, url, enum_, data, gzip, encryption, formatIn, formatOut, password)
        End Using
    End Function
    Public Shared Async Function REST_UploadDownloadAsync(timeoutMs As Integer, url As String, enum_ As Integer, data As Byte(), gzip As EGzip, encryption As EEncryption, formatIn As ESerialisation, formatOut As ESerialisation, password As IConfig) As Threading.Tasks.Task(Of Byte())
        Using wc As New CWebClient(timeoutMs)
            Return Await REST_UploadDownloadAsync(wc, url, enum_, data, gzip, encryption, formatIn, formatOut, password)
        End Using
    End Function

    'Main methods
    Public Shared Function REST_UploadDownload(wc As Net.WebClient, url As String, enum_ As Integer, data As Byte(), gzip As EGzip, encryption As EEncryption, formatIn As ESerialisation, formatOut As ESerialisation, password As IConfig) As Byte()
        'Build the Url
        Dim urlAndQuerystring As String = REST_Url(url, enum_, gzip, encryption, formatIn, formatOut, password)

        'Post the data
        Try
            Return wc.UploadData(urlAndQuerystring, data)

        Catch ex As Exception
            Dim msg As String = "RemoteCall failed"
            If TypeOf ex Is Net.WebException Then
                Try
                    Dim wex As Net.WebException = CType(ex, Net.WebException)
                    If Not IsNothing(wex.Response) Then
                        msg = vbCrLf & New IO.StreamReader(wex.Response.GetResponseStream()).ReadToEnd
                    Else
                        msg = msg & vbTab & ex.Message
                    End If
                Catch
                End Try
            End If
            Throw New Exception(msg)
        End Try
    End Function
    Public Shared Async Function REST_UploadDownloadAsync(wc As Net.WebClient, url As String, enum_ As Integer, data As Byte(), gzip As EGzip, encryption As EEncryption, formatIn As ESerialisation, formatOut As ESerialisation, password As IConfig) As Threading.Tasks.Task(Of Byte())
        'Build the Url
        Dim urlAndQuerystring As String = REST_Url(url, enum_, gzip, encryption, formatIn, formatOut, password)

        'Post the data
        Try
            Return Await wc.UploadDataTaskAsync(urlAndQuerystring, data)
        Catch ex As Exception
            Dim msg As String = "RemoteCall failed"
            If TypeOf ex Is Net.WebException Then
                Try
                    Dim wex As Net.WebException = CType(ex, Net.WebException)
                    If Not IsNothing(wex.Response) Then
                        msg = vbCrLf & New IO.StreamReader(wex.Response.GetResponseStream()).ReadToEnd
                    Else
                        msg = msg & vbTab & ex.Message
                    End If
                Catch
                End Try
            End If
            Throw New Exception(msg)
        End Try
    End Function


    'Helpers
    Public Shared Function REST_Url(url As String, enum_ As Integer, gzip As EGzip, encryption As EEncryption, formatIn As ESerialisation, formatOut As ESerialisation, password As IConfig) As String
        'Required
        Dim sb As New Text.StringBuilder(url)

        'Control param (sometimes encrypted)
        If encryption = EEncryption.None Then
            sb.Append("?enum=").Append(CInt(enum_))
        Else
            sb.Append("?x=").Append(Encode(enum_, password, encryption)) 'url-encode
        End If

        'Optional
        If gzip <> EGzip.None Then
            sb.Append("&g=").Append(CInt(gzip))
        End If
        If encryption <> EEncryption.None Then
            sb.Append("&e=").Append(CInt(encryption))
        End If
        If formatIn <> ESerialisation.Protobuf Then
            sb.Append("&fi=").Append(CInt(formatIn))
        End If
        If formatOut <> ESerialisation.Protobuf Then
            sb.Append("&fo=").Append(CInt(formatOut))
        End If

        Return sb.ToString
    End Function
    Public Shared Function Encode(enum_ As Integer, password As IConfig, encryption As EEncryption) As String
        Dim data As New CTimestamp(enum_)
        Dim bin As Byte() = CProto.Serialise(data) 'Always use protobuf
        bin = password.Encrypt(bin, encryption) 'vary the encryption
        Return Web.HttpUtility.UrlEncode(CBinary.ToBase64(bin))
    End Function
#End Region

End Class

