Imports System.Runtime.Serialization
Imports Framework

<DataContract>
Public Class CValueDateTime : Inherits CValue
    'Data
    <DataMember(Order:=1)> Public ValueDateTime As DateTime

    'Constructor
    Public Sub New(name As String, data As DateTime)
        MyBase.New(name)
        Me.ValueDateTime = data
    End Sub
    Private Sub New()
    End Sub

    'properties
    Public Overrides ReadOnly Property Type As EValueType
        Get
            Return EValueType.DateTime
        End Get
    End Property
    Public Overrides ReadOnly Property Value As Object
        Get
            Return ValueDateTime
        End Get
    End Property
    Public Overrides Function CompareTo(other As CValue) As Integer
        If other.Type = EValueType.DbNull Then
            If Me.Type = EValueType.DbNull Then Return 0
            Return 1
        End If
        Return ValueDateTime.CompareTo(CType(other, CValueDateTime).ValueDateTime)
    End Function
    Public Overrides Function Serialise() As Byte()
        Return CProto.Serialise(ValueDateTime)
    End Function
End Class
