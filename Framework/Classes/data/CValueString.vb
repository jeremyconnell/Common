Imports System.Runtime.Serialization
Imports Framework

<DataContract>
Public Class CValueString : Inherits CValue
    'Data
    <DataMember(Order:=1)> Public ValueString As String

    'Constructor
    Public Sub New(name As String, data As String)
        MyBase.New(name)
        Me.ValueString = data
    End Sub
    Private Sub New()
    End Sub

    'properties
    Public Overrides ReadOnly Property Type As EValueType
        Get
            Return EValueType.String
        End Get
    End Property
    Public Overrides ReadOnly Property Value As Object
        Get
            Return ValueString
        End Get
    End Property
    Public Overrides Function CompareTo(other As CValue) As Integer
        If TypeOf other Is CValueNull Then
            If Me.Type = EValueType.DbNull Then Return 0
            Return 1
        End If

        Return ValueString.CompareTo(CType(other, CValueString).ValueString)
    End Function
    Public Overrides Function Serialise() As Byte()
        Return CProto.Serialise(ValueString)
    End Function
End Class
