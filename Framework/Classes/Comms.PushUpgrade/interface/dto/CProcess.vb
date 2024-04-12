
Imports System.Runtime.Serialization

<DataContract()>
Public Class CProcess

    <DataMember(Order:=1)> Public ProcessName As String
    <DataMember(Order:=2)> Public ExitCode As Integer
    <DataMember(Order:=3)> Public ExitTime As Date
    <DataMember(Order:=4)> Public HasExited As Boolean
    <DataMember(Order:=5)> Public Id As Integer
    <DataMember(Order:=6)> Public MachineName As String
    <DataMember(Order:=7)> Public Responding As Boolean
    <DataMember(Order:=8)> Public StartTime As Date
    <DataMember(Order:=9)> Public Threads As Integer

    Protected Sub New()
    End Sub
    Public Sub New(p As Process)
        Me.ProcessName = p.ProcessName
        Try
            Me.Id = p.Id
            Me.StartTime = p.StartTime.ToUniversalTime()
            Me.MachineName = p.MachineName
            Me.Responding = p.Responding
            Me.Threads = p.Threads.Count
            Me.HasExited = p.HasExited
            If p.HasExited Then
                Me.ExitCode = p.ExitCode
                Me.ExitTime = p.ExitTime.ToUniversalTime()
            End If
        Catch
        End Try
    End Sub

End Class
