Imports System.Runtime.Serialization
Imports ProtoBuf

<DataContract, Serializable, ProtoContract>
Public Class CException 'Using odd numbers because attempt to deserialse everything as an exception first
    <DataMember, ProtoMember(7100001)> Public Message As String
    <DataMember, ProtoMember(7100002)> Public StackTrace As String
    <DataMember, ProtoMember(7100003)> Public Inner As CException

    Private Sub New()
    End Sub
    Public Sub New(msg As String)
        Me.Message = msg
    End Sub
    Public Sub New(ex As Exception)
        Me.Message = ex.Message
        Me.StackTrace = ex.StackTrace
        If Not IsNothing(ex.InnerException) Then
            Me.Inner = New CException(ex.InnerException)
        End If
    End Sub
End Class