Imports ProtoBuf

<ProtoContract()> _
Public Class CTimestamp
    'Data
    <ProtoMember(1)> Public ControlEnum As Integer
    <ProtoMember(2)> Public Created As DateTime

    'Constructor
    Public Sub New(enum_ As Integer)
        ControlEnum = enum_
        Created = DateTime.Now
    End Sub
    Protected Sub New()
        Created = DateTime.Now
    End Sub

    'Shared Constructor
    Shared Sub New()
        CProto.Prepare(Of CTimestamp)()
    End Sub

    'Deserialise
    Public Shared Function Deserialise(binary As Byte()) As CTimestamp
        If IsNothing(binary) Then Return Nothing
        Return Deserialise(New IO.MemoryStream(binary))
    End Function
    Private Shared Function Deserialise(ms As IO.Stream) As CTimestamp
        Return Serializer.Deserialize(Of CTimestamp)(ms)
    End Function
End Class