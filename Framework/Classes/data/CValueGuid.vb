Imports System.Data.SqlTypes
Imports System.Runtime.Serialization
Imports Framework

<DataContract>
Public Class CValueGuid : Inherits CValue
    'Data
    <DataMember(Order:=1)>
    Public ValueGuid As Guid
    Public ValueGuid_Sort As SqlGuid

    Public Sub New(name As String, data As Guid)
        MyBase.New(name)
        Me.ValueGuid = data
        Me.ValueGuid_Sort = New SqlGuid(data)
    End Sub
    Private Sub New()
    End Sub

    'properties
    Public Overrides ReadOnly Property Type As EValueType
        Get
            Return EValueType.Guid
        End Get
    End Property
    Public Overrides ReadOnly Property Value As Object
        Get
            Return ValueGuid
        End Get
    End Property
    Public Overrides Function CompareTo(other As CValue) As Integer
        If Me.Type = EValueType.DbNull AndAlso other.Type = EValueType.DbNull Then Return 0
        If other.Type = EValueType.DbNull Then Return 1
        Return ValueGuid_Sort.CompareTo(CType(other, CValueGuid).ValueGuid_Sort)
    End Function
    Public Overrides Function Serialise() As Byte()
        Return CProto.Serialise(ValueGuid)
    End Function
End Class
