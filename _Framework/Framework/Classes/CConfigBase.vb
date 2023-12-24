Imports System.Configuration
Imports System.Configuration.ConfigurationManager
Imports System.Web.Configuration

<CLSCompliant(True)> _
Public Class CConfigBase

    'Standard Connection String
    Public Shared Function Driver() As String
        Return Config("Driver")
    End Function
    Public Shared Function ConnectionString() As String
        If ConnectionStrings.Count = 0 Then
            Return Config("ConnectionString")
        Else
            Return Config("ConnectionString", ConnectionStrings(0).ConnectionString) 'Use the first connnection string
        End If
    End Function
    Public Shared Function ConnectionString(ByVal name As String) As String
        Dim cs As ConnectionStringSettings = ConnectionStrings(name)
        If IsNothing(cs) Then Return String.Empty
        Return cs.ConnectionString
    End Function

    'Shorter Alternatives
    Public Shared Function SqlExpressPath() As String
        Return Config("SqlExpressPath", Config("SqlExpress"))
    End Function
    Public Shared Function AccessDatabasePath() As String
        Return Config("AccessDatabasePath", Config("AccessDatabase"))
    End Function
    Public Shared Function ExcelDatabasePath() As String
        Return Config("ExcelDatabasePath", Config("ExcelDatabase"))
    End Function
    Public Shared Function SqlCEPath() As String
        Return Config("SqlCEPath", Config("SqlCE"))
    End Function
    Public Shared Function WebSite() As String
        Return Config("WebSite")
    End Function

    'Named connection string (refers to Connection Strings node)
    Public Shared Function ActiveConnectionString() As String
        Return Config("ActiveConnectionString")
    End Function

    Public Shared Function CommandTimeoutSecs() As Integer
        Return ConfigInt("CommandTimeoutSecs", 30)
    End Function


    'Default Encryption key(s)
    Friend Shared Function FastEncryptionKey() As String
        Return Config("EncryptionKey", WebServicePassword)
    End Function
    Friend Shared ReadOnly Property TripleDesIV() As String
        Get
            Return Config("TripleDesIV", "D08601BA9F91BA88")
        End Get
    End Property
    Friend Shared ReadOnly Property TripleDesKey() As String
        Get
            Return Config("TripleDesKey", "8B682A81B28348D69231136CA376A90C2D3D25CC046B6406")
        End Get
    End Property


    'WebService Driver (and optional Proxy)
    Public Shared Sub CheckPassword(ByVal password As String)
        If Not String.IsNullOrEmpty(CConfigBase.WebServicePassword) Then
            If password <> CConfigBase.WebServicePassword Then Throw New Exception("Invalid webservice password")
        End If
    End Sub
    Private Shared m_password As Byte() = Nothing
    Public Shared ReadOnly Property WebServicePassword() As String
        Get
            Return Config("WebServicePassword", "db75794fe12147539b9596b523d916e87bcbeb06cb35480abca1d98d66b403727b4cd6d4cd344e9eb79cf511ed4f04d9")
        End Get
    End Property
    Public Shared ReadOnly Property UseRawPassword() As Boolean 'Backwards-compat xor encryption (no sha512)
        Get
            Return ConfigBool("UseRawPassword")
        End Get
    End Property
    Public Shared ReadOnly Property WebServicePasswordBytes() As Byte()
        Get
            If IsNothing(m_password) Then
                m_password = CBinary.StringToBytes(WebServicePassword)
                If Not UseRawPassword Then 'Switch to true for backwards-compat
                    m_password = CBinary.Sha512(m_password) 'Stronger password for pack/unpack
                End If
            End If
            Return m_password
        End Get
    End Property
    Friend Shared ReadOnly Property ProxyAddress() As String
        Get
            Return Config("ProxyAddress")
        End Get
    End Property
    Friend Shared ReadOnly Property ProxyUser() As String
        Get
            Return Config("ProxyUser")
        End Get
    End Property
    Friend Shared ReadOnly Property ProxyPassword() As String
        Get
            Return Config("ProxyPassword")
        End Get
    End Property
    Friend Shared ReadOnly Property ProxyDomain() As String
        Get
            Return Config("ProxyDomain")
        End Get
    End Property

    'Default Cache Timeout
    Private Shared m_cacheTimeOut As TimeSpan = TimeSpan.MinValue
    Public Shared ReadOnly Property CacheTimeoutDefault() As TimeSpan
        Get
            If m_cacheTimeOut = TimeSpan.MinValue Then
                m_cacheTimeOut = New TimeSpan(CacheTimeoutHours, CacheTimeoutMinutes, CacheTimeoutSeconds)
            End If
            Return m_cacheTimeOut
        End Get
    End Property
    Private Shared ReadOnly Property CacheTimeoutHours() As Integer
        Get
            Return ConfigInt("CacheTimeoutHours", 3)
        End Get
    End Property
    Private Shared ReadOnly Property CacheTimeoutMinutes() As Integer
        Get
            Return ConfigInt("CacheTimeoutMinutes", 0)
        End Get
    End Property
    Private Shared ReadOnly Property CacheTimeoutSeconds() As Integer
        Get
            Return ConfigInt("CacheTimeoutSeconds", 0)
        End Get
    End Property




    'Utilities (private)
    Protected Shared Function Config(ByVal key As String) As String
        Try
            Dim s As String = EncryptedSettings(key)
            If Not IsNothing(s) Then Return s

            If m_wrote.TryGetValue(key, s) Then Return s

            s = AppSettings(key)
            If Not IsNothing(s) Then Return s

            Return String.Empty
        Catch ex As Exception
            If IsNothing(ex.InnerException) Then Throw ex
            Throw ex.InnerException
        End Try
    End Function
    Protected Shared Function Config(ByVal key As String, ByVal defaultValue As String) As String
        Config = Config(key)
        If Len(Config) = 0 Then Config = defaultValue
    End Function
    Protected Shared Function ConfigOrEx(ByVal key As String) As String
        Dim s As String = Config(key)
        If Len(s) = 0 Then Throw New Exception("Missing Config setting: " & key)
        Return s
    End Function
    Protected Shared m_wrote As New Dictionary(Of String, String)


    Protected Shared Function ConfigBool(ByVal key As String) As Boolean
        Return ConfigBool(key, False)
    End Function
    Protected Shared Function ConfigBool(ByVal key As String, ByVal defaultValue As Boolean) As Boolean
        Dim s As String = Config(key)
        If Len(s) = 0 Then Return defaultValue
        If 0 = String.Compare("true", s, True) Then Return True
        If 0 = String.Compare("yes", s, True) Then Return True
        Return False
    End Function
    Protected Shared Function ConfigInt(ByVal key As String) As Integer
        Return ConfigInt(key, Integer.MinValue)
    End Function
    Protected Shared Function ConfigInt(ByVal key As String, ByVal defaultValue As Integer) As Integer
        Dim s As String = Config(key)
        Dim i As Integer
        If Not Integer.TryParse(s, i) Then Return defaultValue
        Return i
    End Function
    Protected Shared Function ConfigLong(ByVal key As String) As Long
        Return ConfigLong(key, Long.MinValue)
    End Function
    Protected Shared Function ConfigLong(ByVal key As String, ByVal defaultValue As Long) As Long
        Dim s As String = Config(key)
        Dim i As Long
        If Not Long.TryParse(s, i) Then Return defaultValue
        Return i
    End Function
    Protected Shared Function ConfigGuid(ByVal key As String) As Guid
        Return ConfigGuid(key, Guid.Empty)
    End Function
    Protected Shared Function ConfigGuid(ByVal key As String, ByVal defaultValue As Guid) As Guid
        Dim s As String = Config(key)
        If s.Length = 36 OrElse s.Length = 38 Then
            Try
                Return New Guid(s)
            Catch
                Return defaultValue
            End Try
        End If
        Return defaultValue
    End Function


    '        <section name="encryptedSettings" type="System.Configuration.NameValueSectionHandler"/>
    '    </configSections>
    '   <encryptedSettings>
    '    <add key="EncryptionKey" value="asdfasdf" />
    '   </encryptedSettings>
    Private Shared m_hasEncr As Boolean = True
    Private Shared Function EncryptedSettings(ByVal key As String) As String
        If Not m_hasEncr Then Return Nothing
        Dim e As IEnumerable = CType(ConfigurationManager.GetSection("encryptedSettings"), IEnumerable)
        If IsNothing(e) Then
            m_hasEncr = False
            Return Nothing 'Section not found
        End If

        EncryptedSettings = CStr(e.GetType().GetProperties()(0).GetGetMethod().Invoke(e, New Object() {key}))
        If IsNothing(EncryptedSettings) Then Return Nothing 'Key not found
    End Function


End Class
