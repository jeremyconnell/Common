Imports System.Runtime.Serialization

<DataContract(), Serializable()>
Public Class CVersionSchemaConfig
    'Data
    <DataMember(Order:=1)> Public MachineName As String

    <DataMember(Order:=2)> Public VersionMD5 As Guid
    <DataMember(Order:=3)> Public SchemaMD5 As Guid

    <DataMember(Order:=4)> Public Version As CFolderHash
    <DataMember(Order:=5)> Public Schema As CSchemaInfo

    <DataMember(Order:=6)> Public AppSettings As Dictionary(Of String, String)
    <DataMember(Order:=7)> Public ConnectionStrings As Dictionary(Of String, String)
    <DataMember(Order:=8)> Public LogFiles As Dictionary(Of String, Long)

    'Pre-Cons
    Shared Sub New()
        CProto.Prepare(Of CVersionSchemaConfig)()
    End Sub


    'Presentation Logic
    Public ReadOnly Property MD5_Base64 As String
        Get
            Return CBinary.ToBase64(VersionMD5, 10)
        End Get
    End Property
    Public ReadOnly Property MD5_Base64Trunc As String
        Get
            Return CBinary.ToBase64(VersionMD5, 10)
        End Get
    End Property
    Public ReadOnly Property SchemaMD55_Base64 As String
        Get
            Return CBinary.ToBase64(SchemaMD5, 10)
        End Get
    End Property
    Public ReadOnly Property SchemaMD5_Base64Trunc As String
        Get
            Return CBinary.ToBase64(SchemaMD5, 10)
        End Get
    End Property

End Class
