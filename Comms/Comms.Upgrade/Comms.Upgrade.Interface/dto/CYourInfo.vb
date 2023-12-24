Imports System.Runtime.Serialization
Imports Comms.PushUpgrade.Interface

<DataContract()>
Public Class CYourInfo
    <DataMember(Order:=1)> Public Id As CIdentity
    <DataMember(Order:=2)> Public Filter As CFilter
    <DataMember(Order:=3)> Public LastReportId As Integer

    'Pre-Cons
    Shared Sub New()
        CProto.Prepare(Of CYourInfo)()
    End Sub
End Class
