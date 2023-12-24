Imports System.Runtime.Serialization

<DataContract()>
Public Class CIdentity
    'MainId
    <DataMember(Order:=1)> Public InstanceId As Integer

    'TempId
    <DataMember(Order:=2)> Public AppId As Integer
    <DataMember(Order:=3)> Public InstanceName As String

    'Protobuf
    Shared Sub New()
        CProto.Prepare(Of CIdentity)()
    End Sub
End Class
