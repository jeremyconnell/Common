Imports System.Runtime.Serialization

<DataContract()>
Public Class CUpgradeResponse
    'Request Client Info
    <DataMember(Order:=1)> Public SendSchemaInfo As Boolean
    <DataMember(Order:=2)> Public SendFolderHash As Boolean
    <DataMember(Order:=3)> Public SendBinaries As Boolean

    'Supply Info to Client
    <DataMember(Order:=4)> Public YourName As CYourInfo
    <DataMember(Order:=5)> Public UpgradeTo As CFilesList
    <DataMember(Order:=6)> Public RunScripts As List(Of String)

    'Pre-Cons
    Shared Sub New()
        CProto.Prepare(Of COnAppStopped)()
    End Sub
End Class
