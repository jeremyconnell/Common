Imports System.Runtime.Serialization
Imports Framework

<DataContract>
Public Class CValueDecimal : Inherits CValue
    'Data
    <DataMember(Order:=1)> Public ValueDecimal As Decimal

    'Constructor
    Public Sub New(name As String, data As Decimal)
        MyBase.New(name)
        Me.ValueDecimal = data
    End Sub
    Private Sub New()
    End Sub

    'properties
    Public Overrides ReadOnly Property Type As EValueType
        Get
            Return EValueType.Decimal
        End Get
    End Property
    Public Overrides ReadOnly Property Value As Object
        Get
            Return ValueDecimal
        End Get
    End Property
    Public Overrides Function CompareTo(other As CValue) As Integer
        If other.Type = EValueType.DbNull Then
            If Me.Type = EValueType.DbNull Then Return 0
            Return 1
        End If
        Return ValueDecimal.CompareTo(CType(other, CValueDecimal).ValueDecimal)
    End Function
    Public Overrides Function Serialise() As Byte()
        Return CProto.Serialise(ValueDecimal)
    End Function
End Class
