'Wcf implementation - Implements any interface (without actually exposing it), using Protogen serialisation and custom encryption
Public MustInherit Class CServer_Wcf : Implements IPassthru

    'Hooks into application-specific Logic 
    Protected MustOverride Function ExecuteMethod(methodNameEnum As Integer, params As CReader) As Object

    'Config settings (instance reqd)
    Protected MustOverride ReadOnly Property Config As IConfig_Server

    'Generic Service to emulate REST (can optionally ignore those parameters, in place of WCF bindings)
    Public Overridable Function Process(methodNameEnum_ As Integer, data As Byte(), gzip As EGzip, encrypt As EEncryption, formatIn As ESerialisation, formatOut As ESerialisation) As Byte() Implements IPassthru.TransportInterface
        'Decrypt/decompress the input
        data = Config.Decrypt(data, encrypt)
        data = CGzip.Decompress(data, gzip, True)

        'Deserialise
        Dim params As CReader = CSerialise.Unpack(data, formatIn)

        'Execute (type/enum-dependant)
        Dim obj As Object = Nothing
        Try
            obj = ExecuteMethod(methodNameEnum_, params)
        Catch ex As Exception
            obj = New CException(ex)
        End Try

        'Serialise back to bytes
        data = CSerialise.Serialise(obj, formatOut)

        'Compress/encrypt the output
        data = CGzip.Compress(data, gzip, False)
        data = Config.Encrypt(data, encrypt)

        Return data
    End Function

End Class
