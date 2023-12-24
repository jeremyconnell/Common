Imports System.Runtime.Serialization
Imports Comms.PushUpgrade.Interface

<DataContract()>
Public Class COnAppStopped
    'Main data
    <DataMember(Order:=1)> Public Id As CIdentity
    <DataMember(Order:=2)> Public LastReportId As Integer

    'Pre-Cons
    Shared Sub New()
        CProto.Prepare(Of COnAppStopped)()
    End Sub
End Class