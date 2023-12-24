Imports System.Runtime.Serialization
Imports Framework

<DataContract>
Public Class CValueDouble : Inherits CValue
    'Data
    <DataMember(Order:=1)> Public ValueDouble As Double

    'Constructor
    Public Sub New(name As String, data As Double)
        MyBase.New(name)
        Me.ValueDouble = data
    End Sub
    Private Sub New()
    End Sub

    'properties
    Public Overrides ReadOnly Property Type As EValueType
        Get
            Return EValueType.Double
        End Get
    End Property
    Public Overrides ReadOnly Property Value As Object
        Get
            Return ValueDouble
        End Get
    End Property
    Public Overrides Function CompareTo(other As CValue) As Integer
        If other.Type = EValueType.DbNull Then
            If Me.Type = EValueType.DbNull Then Return 0
            Return 1
        End If
        Return ValueDouble.CompareTo(CType(other, CValueDouble).ValueDouble)
    End Function
    Public Overrides Function Serialise() As Byte()
        Return CProto.Serialise(ValueDouble)
    End Function
End Class
