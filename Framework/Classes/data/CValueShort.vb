Imports System.Runtime.Serialization
Imports Framework

<DataContract>
Public Class CValueShort : Inherits CValue
    'Data
    <DataMember(Order:=1)> Public ValueShort As Short

    'Constructor
    Public Sub New(name As String, data As Short)
        MyBase.New(name)
        Me.ValueShort = data
    End Sub
    Private Sub New()
    End Sub

    'properties
    Public Overrides ReadOnly Property Type As EValueType
        Get
            Return EValueType.Short
        End Get
    End Property
    Public Overrides ReadOnly Property Value As Object
        Get
            Return ValueShort
        End Get
    End Property
    Public Overrides Function CompareTo(other As CValue) As Integer
        Select Case other.Type
            Case EValueType.Long : Return Convert.ToInt64(ValueShort).CompareTo(CType(other, CValueLong).ValueLong)
            Case EValueType.Integer : Return Convert.ToInt32(ValueShort).CompareTo(CType(other, CValueInt).ValueInt)
            Case EValueType.DbNull
                If Me.Type = EValueType.DbNull Then Return 0
                Return 1
        End Select
        Return ValueShort.CompareTo(CType(other, CValueShort).ValueShort)
    End Function
    Public Overrides Function Serialise() As Byte()
        Return CProto.Serialise(ValueShort)
    End Function
End Class
