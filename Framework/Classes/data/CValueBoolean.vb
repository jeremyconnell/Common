Imports System.Runtime.Serialization
Imports Framework

<DataContract>
Public Class CValueBoolean : Inherits CValue
    'Data
    <DataMember(Order:=1)> Public ValueBoolean As Boolean

    'Constructor
    Public Sub New(name As String, data As Boolean)
        MyBase.New(name)
        Me.ValueBoolean = data
    End Sub
    Private Sub New()
    End Sub
    Shared Sub New()
        CProto.Prepare(Of CValueBoolean)()
    End Sub

    'properties
    Public Overrides ReadOnly Property Type As EValueType
        Get
            Return EValueType.Boolean
        End Get
    End Property
    Public Overrides ReadOnly Property Value As Object
        Get
            Return ValueBoolean
        End Get
    End Property
    Public Overrides Function CompareTo(other As CValue) As Integer
        If other.Type = EValueType.DbNull Then
            If Me.Type = EValueType.DbNull Then Return 0
            Return 1
        End If
        Return ValueBoolean.CompareTo(CType(other, CValueBoolean).ValueBoolean)
    End Function
    Public Overrides Function Serialise() As Byte()
        Return CProto.Serialise(ValueBoolean)
    End Function
End Class
