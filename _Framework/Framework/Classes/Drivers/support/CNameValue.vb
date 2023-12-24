Imports System.Runtime.Serialization

<Serializable(), DataContract(), CLSCompliant(True)>
Public Class CNameValue
    'Data
    <DataMember(Order:=1)> Public Name As String
    <DataMember(Order:=2)> Public Value As Object
    <DataMember(Order:=3)> Public MarkerName As String

    'Constructors
    'Protobuf: cannot serialise object
    'Shared Sub New()
    '    CProto.Prepare(Of CNameValue)()
    'End Sub
    Public Sub New() 'For Serialisation
    End Sub
    Public Sub New(ByVal name As String, ByVal value As Object)
        Me.Name = name
        Me.Value = value
        If String.IsNullOrEmpty(name) Then Me.MarkerName = String.Empty Else Me.MarkerName = name.ToLower.Replace("#", "")
    End Sub
End Class
