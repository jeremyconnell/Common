Imports System.Runtime.Serialization

<DataContract()>
Public Class CInstanceInfo
    <DataMember(Order:=1)> Public Instance As CIdentity
    <DataMember(Order:=2)> Public Filter As CFilter

    'Pre-Cons
    Shared Sub New()
        CProto.Prepare(Of CInstanceInfo)()
    End Sub
End Class
