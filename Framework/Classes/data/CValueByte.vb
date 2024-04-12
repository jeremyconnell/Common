
Imports System.Runtime.Serialization
Imports Framework

<DataContract>
Public Class CValueByte : Inherits CValue
    'Data
    <DataMember(Order:=1)> Public ValueByte As Byte

    'Constructor
    Public Sub New(name As String, data As Byte)
        MyBase.New(name)
        Me.ValueByte = data
    End Sub
    Private Sub New()
    End Sub

    'properties
    Public Overrides ReadOnly Property Type As EValueType
        Get
            Return EValueType.Byte
        End Get
    End Property
    Public Overrides ReadOnly Property Value As Object
        Get
            Return ValueByte
        End Get
    End Property
    Public Overrides Function CompareTo(other As CValue) As Integer
        If other.Type = EValueType.DbNull Then
            If Me.Type = EValueType.DbNull Then Return 0
            Return 1
        End If
        If TypeOf other Is CValueInt Then
            Return Convert.ToInt32(ValueByte).CompareTo(CType(other, CValueInt).ValueInt)
        End If
        Return ValueByte.CompareTo(CType(other, CValueByte).ValueByte)
    End Function
    Public Overrides Function Serialise() As Byte()
        Return CProto.Serialise(ValueByte)
    End Function
End Class
