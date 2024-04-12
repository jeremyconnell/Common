Imports System.Runtime.Serialization
Imports Framework

<DataContract>
Public Class CValueInt : Inherits CValue
    'Data
    <DataMember(Order:=1)> Public ValueInt As Integer

    'Constructor
    Public Sub New(name As String, data As Integer)
        MyBase.New(name)
        Me.ValueInt = data
    End Sub
    Private Sub New()
    End Sub
    Shared Sub New()
        CProto.Prepare(Of CValueInt)()
    End Sub

    'properties
    Public Overrides ReadOnly Property Type As EValueType
        Get
            Return EValueType.Integer
        End Get
    End Property
    Public Overrides ReadOnly Property Value As Object
        Get
            Return ValueInt
        End Get
    End Property
    Public Overrides Function CompareTo(other As CValue) As Integer
        Select Case other.Type
            Case EValueType.Long : Return Convert.ToInt64(ValueInt).CompareTo(CType(other, CValueLong).ValueLong)
            Case EValueType.DbNull
                If Me.Type = EValueType.DbNull Then Return 0
                Return 1
        End Select
        Return ValueInt.CompareTo(CType(other, CValueInt).ValueInt)
    End Function
    Public Overrides Function Serialise() As Byte()
        Return CProto.Serialise(ValueInt)
    End Function
End Class
