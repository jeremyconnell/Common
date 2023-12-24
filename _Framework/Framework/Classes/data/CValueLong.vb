Imports System.Runtime.Serialization
Imports Framework

<DataContract>
Public Class CValueLong : Inherits CValue
    'Data
    <DataMember(Order:=1)> Public ValueLong As Long

    'Constructor
    Public Sub New(name As String, data As Long)
        MyBase.New(name)
        Me.ValueLong = data
    End Sub
    Private Sub New()
    End Sub

    'properties
    Public Overrides ReadOnly Property Type As EValueType
        Get
            Return EValueType.Long
        End Get
    End Property
    Public Overrides ReadOnly Property Value As Object
        Get
            Return ValueLong
        End Get
    End Property
    Public Overrides Function CompareTo(other As CValue) As Integer
        Select Case other.Type
            Case EValueType.Integer : ValueLong.CompareTo(Convert.ToInt64(CType(other, CValueInt).ValueInt))
            Case EValueType.DbNull
                If Me.Type = EValueType.DbNull Then Return 0
                Return 1
        End Select
        Return ValueLong.CompareTo(CType(other, CValueLong).ValueLong)
    End Function
    Public Overrides Function Serialise() As Byte()
        Return CProto.Serialise(ValueLong)
    End Function
End Class
