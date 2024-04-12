Imports System.Runtime.Serialization
Imports ProtoBuf

<DataContract, ProtoContract>
Public Class CVerConfRunningList ': Inherits List(Of CVerConfRunning)

    <DataMember(Order:=1), ProtoMember(1)> Public AllProcesses As List(Of CProcess)
    <DataMember(Order:=2), ProtoMember(2)> Public MachineName As String
    <DataMember(Order:=3), ProtoMember(3)> Public VersionsAndConfigs As List(Of CVerConfRunning)

    Shared Sub New()
        CProto.Prepare(Of CVerConfRunningList)()
    End Sub
End Class
