
Imports System.Runtime.Serialization
Imports Framework

<DataContract>
Public Class CValueBytes : Inherits CValue
    'Data
    <DataMember(Order:=1)> Public ValueBytes As Byte()

    'Constructor
    Public Sub New(name As String, data As Byte())
        MyBase.New(name)
        Me.ValueBytes = data
    End Sub
    Private Sub New()
    End Sub

    'properties
    Public Overrides ReadOnly Property Type As EValueType
        Get
            Return EValueType.Bytes
        End Get
    End Property
    Public Overrides ReadOnly Property Value As Object
        Get
            Return ValueBytes
        End Get
    End Property
    Public Overrides Function CompareTo(other As CValue) As Integer
        If other.Type = EValueType.DbNull Then
            If Me.Type = EValueType.DbNull Then Return 0
            Return 1
        End If
        Return CBinary.ToBase64(ValueBytes).CompareTo(CBinary.ToBase64(other.AsBytes))
    End Function
    Public Overrides Function Serialise() As Byte()
        Return CProto.Serialise(ValueBytes)
    End Function
End Class
