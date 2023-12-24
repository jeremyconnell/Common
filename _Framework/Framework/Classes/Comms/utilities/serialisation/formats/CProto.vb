Option Strict Off

Public Class CProto : Inherits CSerialise

#Region "Shared"
    Public Overloads Shared Function Serialise(obj As Object) As Byte()
        If IsNothing(obj) Then Return CReader.EMPTY 'Special case: Nulls
        If TypeOf obj Is Byte() Then Return obj 'Trivial case: binary data

        Using ms As New IO.MemoryStream
            ProtoBuf.Serializer.Serialize(ms, obj)
            Return ms.ToArray
        End Using
    End Function

    Public Overloads Shared Function Deserialise(Of T)(data As Byte()) As T
        If IsNothing(data) Then Return Nothing 'Trivial case: Null data
        If data.Length = 0 Then Return Nothing 'Special case: Null-equiv value

        Using ms As New IO.MemoryStream(data)
            Return ProtoBuf.Serializer.Deserialize(Of T)(ms)
        End Using
    End Function


    'Public Shared Function Deserialise(t As Type, data As Byte()) As Object
    '    Return Deserialise(Of T)(data)
    'End Function

    'Important: For safe multi-threading, call this from the static constructor of the type
    Public Shared Sub Prepare(Of T)()
        ProtoBuf.Serializer.PrepareSerializer(Of T)()
    End Sub
#End Region

#Region "MustOverride"
    Public Overrides Function Deserialise_(Of T)(data() As Byte) As T
        Return CProto.Deserialise(Of T)(data)
    End Function
    Public Overrides Function Serialise_(obj As Object) As Byte()
        Return CProto.Serialise(obj)
    End Function
#End Region

End Class