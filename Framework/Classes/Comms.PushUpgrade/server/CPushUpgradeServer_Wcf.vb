Imports System.ServiceModel

<ServiceContract()>
Public Interface IWcfPushUpgrade
    <OperationContract()> Function TransportInterface(enum_ As Integer,
                                input As Byte(),
                                gzip As EGZip_,
                                algorithm As EEncryption_,
                                formatIn As ESerialisation_,
                                formatOut As ESerialisation_) As Byte()

    Enum EGZip_
        None = 0
        Input = 1
        Output = 2
        Both = 3
    End Enum
    Enum EEncryption_
        None = 0
        [Xor] = 1 'Faster
        Rij = 2 'More secure
    End Enum
    Enum ESerialisation_
        Protobuf = 0
        BinarySer = 1
        DataContract = 2
        Json = 3
        Xml = 4
    End Enum
End Interface

Public Class WcfPushUpgrade : Inherits CServer_Wcf : Implements IWcfPushUpgrade
    Function TransportInterface(enum_ As Integer,
                                    input As Byte(),
                                    gzip As IWcfPushUpgrade.EGZip_,
                                    algorithm As IWcfPushUpgrade.EEncryption_,
                                    formatIn As IWcfPushUpgrade.ESerialisation_,
                                    formatOut As IWcfPushUpgrade.ESerialisation_) As Byte() Implements IWcfPushUpgrade.TransportInterface
        Return Process(enum_, input, CType(gzip, EGzip), CType(algorithm, EEncryption), CType(formatIn, ESerialisation), CType(formatOut, ESerialisation))
    End Function


    'Config
    Protected Overrides ReadOnly Property Config As IConfig_Server
        Get
            Return CPushUpgradeServer_Config.Shared
        End Get
    End Property


    'Action - For Generic Handler (Deserialise params & Exec) 
    Protected Overrides Function ExecuteMethod(methodNameEnum_ As Integer, params As CReader) As Object
        Return New CPushUpgradeServer().ExecuteMethod(methodNameEnum_, params)
    End Function
End Class