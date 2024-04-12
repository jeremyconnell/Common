Option Strict Off

Public Class CBinarySer : Inherits CSerialise

#Region "MustOverride"
    Public Overrides Function Serialise_(obj As Object) As Byte()
        If IsNothing(obj) Then Return CReader.EMPTY 'Special case: Nulls
        If TypeOf obj Is Byte() Then Return obj 'Trivial case: already is binary data

        Return CBinary.SerialiseToBytes(obj)
    End Function

    Public Overrides Function Deserialise_(Of T)(data() As Byte) As T
        If IsNothing(data) Then Return Nothing 'Trivial case: Null data
        If data.Length = 0 Then Return Nothing 'Special case: Null-equiv value

        Return CBinary.DeserialiseFromBytes(data)
    End Function
#End Region

#Region "Shared"
    Public Overloads Shared Function Serialise(obj As Object) As Byte()
        Return CBinary.SerialiseToBytes(obj)
    End Function
    Public Overloads Shared Function Deserialise(t As Type, data As Byte()) As Object
        Return CBinary.DeserialiseFromBytes(data)
    End Function
    Public Overloads Shared Function Deserialise(Of T)(data As Byte()) As Object
        Return CBinary.DeserialiseFromBytes(data)
    End Function
#End Region

End Class
