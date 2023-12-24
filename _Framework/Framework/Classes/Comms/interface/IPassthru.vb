Imports System.ServiceModel
'Generic Service Interface for: WCF/REST/EXE/INPROC
' - Abstracts any REST-like transport mechanism
' - Sits below an application-specific Interface
' - Optional: can expose the more specific interface (normal wcf), which replaces the ProtoGen/Encryption layer with: DataContract serialisation, Https/x509 encryption, IIS compression
<ServiceContract()>
Public Interface IPassthru 'Binary Input/Output, plus switches for Gzip, Encryption, and I/O Formats
    <OperationContract()> _
    Function TransportInterface(enum_ As Integer,
                                input As Byte(),
                                gzip As EGzip,
                                algorithm As EEncryption,
                                formatIn As ESerialisation,
                                formatOut As ESerialisation) As Byte()
End Interface
