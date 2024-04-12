Option Strict Off

'REST implementation - Implements any interface 
' - Enum identifies the method in the associated interface
' - Custom classes store parameters for each method
' - Protogen serialisation (plus custom encryption layer) on all inputs/outputs
Public MustInherit Class CServer_Http_Aspx : Inherits Web.UI.Page : Implements IPassthru

#Region "MustOverride" 'Switch on enum; deserialisation requires knowledge of the type (method-specific parameters)
    'Application-specific logic. Enum of functions in custom interface - deserialisation requires knowledge of the type (method-specific parameters)
    Public MustOverride Function ExecuteMethod(methodNameEnum_ As Integer, params As CReader) As Object

    'Config settings (encryption password)
    Protected MustOverride ReadOnly Property Config As IConfig_Server
#End Region

#Region "Interface - IPassthru (generic method marshalling)"
    Public Function ServerSideProcessing(methodNameEnum_ As Integer, data As Byte(), gzip As EGzip, encrypt As EEncryption, formatIn As ESerialisation, formatOut As ESerialisation) As Byte() Implements IPassthru.TransportInterface
        If Config.RequireEncryption AndAlso encrypt = EEncryption.None Then Throw New Exception("Server Requires Encryption (config setting: " & Config.RequireEncryptionConfigKey & ")")

        Dim obj As Object = Nothing
        Try
            'Decrypt, Decompress
            data = Config.Decrypt(data, encrypt)
            data = CGzip.Decompress(data, gzip, True)

            'Deserialise
            Dim params As CReader = CSerialise.Unpack(data, formatIn)

            'Execute (type/enum-dependant)
            obj = ExecuteMethod(methodNameEnum_, params)
        Catch ex As Exception
            obj = New CException(ex)
        End Try

        'Serialise, Compress, Encrypt
        data = CSerialise.Serialise(obj, formatOut)
        data = CGzip.Compress(data, gzip, False)
        Return Config.Encrypt(data, encrypt)
    End Function
#End Region

#Region "Querystring"
    'Required
    Public ReadOnly Property Enum_ As String 'Identifies the function to execute
        Get
            'Normal case - unencrypted
            If Encryption = EEncryption.None Then Return PlainEnum_

            'Decrypt
            Dim bin As Byte() = CBinary.FromBase64(EncryptedEnum)
            bin = Config.Decrypt(bin, Encryption)

            'Validate
            Dim ts As CTimestamp = CTimestamp.Deserialise(bin)
            If ts.Created.AddDays(1).AddMinutes(1) < DateTime.Now Then Throw New Exception("Timestamp is old") 'allows 24hrs for timezones

            Return ts.ControlEnum
        End Get
    End Property
    Public ReadOnly Property PlainEnum_ As Integer 'Identifies the function to execute (no-encryption, or backwards-compat)
        Get
            Return Framework.CWeb.RequestInt("enum")
        End Get
    End Property
    Public ReadOnly Property EncryptedEnum As String 'Identifies the function to execute (encrypted case)
        Get
            Return Framework.CWeb.RequestStr("x")
        End Get
    End Property

    'Optional (introduced later)
    Public ReadOnly Property GZip As EGzip 'Input/Output compresion layer
        Get
            Return Framework.CWeb.RequestInt("g", EGzip.None)
        End Get
    End Property
    Public ReadOnly Property Encryption As EEncryption 'Input/Output encryption layer
        Get
            Return Framework.CWeb.RequestInt("e", EEncryption.None)
        End Get
    End Property
    Public ReadOnly Property FormatIn As ESerialisation 'Input/Output serialisation layer (not yet implemented)
        Get
            Return Framework.CWeb.RequestInt("fi", ESerialisation.Protobuf)
        End Get
    End Property
    Public ReadOnly Property FormatOut As ESerialisation 'Input/Output serialisation layer (not yet implemented)
        Get
            Return Framework.CWeb.RequestInt("fo", ESerialisation.Protobuf)
        End Get
    End Property
#End Region

#Region "Asp.Net - Event Handlers"
    Protected Overridable Function CustomGetBasedLogic() As Boolean
        Return False
    End Function
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'Ignore random requests
        If Request.QueryString.Count = 0 OrElse Integer.MinValue = Enum_ Then
            If Not CustomGetBasedLogic() Then
                Response.Write("Hello World")
                Response.End()
            End If
        End If

        'Read posted data (todo: support a GET option)
        Dim bin As Byte() = Request.BinaryRead(Request.ContentLength)

        'Encryption, Compression, Serialisation (and enum-driven logic)
        bin = ServerSideProcessing(Enum_, bin, GZip, Encryption, FormatIn, FormatOut)

        Response.BinaryWrite(bin)
        Response.End()
    End Sub
#End Region

#Region "Asp.Net Housekeeping"
    Public Overrides Property EnableTheming As Boolean
        Get
            Return False
        End Get
        Set(value As Boolean)
            MyBase.EnableTheming = value
        End Set
    End Property
    Public Overrides Property EnableViewState As Boolean
        Get
            Return False
        End Get
        Set(value As Boolean)
            MyBase.EnableTheming = value
        End Set
    End Property
#End Region

End Class
