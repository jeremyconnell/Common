Imports System.Runtime.Serialization

<DataContract()>
Public Class COnAppStarted
    'Config
    <DataMember(Order:=1)> Public Id As CIdentity
    <DataMember(Order:=2)> Public Filters As CFilter

    'Lite
    <DataMember(Order:=4)> Public BinMd5 As Guid
    <DataMember(Order:=5)> Public SchemaMd5 As Guid

    'Full (Optional, at server-request)
    <DataMember(Order:=44)> Public Schema As CSchemaInfo
    <DataMember(Order:=55)> Public Folder As CFolderHash
    <DataMember(Order:=66)> Public BinFiles As CFilesList

    'Optional (Upgrade/OnAppStopped events)
    <DataMember(Order:=60)> Public LastReportId As Integer



    'Pre-Cons
    Shared Sub New()
        CProto.Prepare(Of COnAppStarted)()
    End Sub
End Class