Imports System.Runtime.Serialization

<DataContract(), Serializable()>
Public Class CMyVersion
    'Data
    <DataMember(Order:=1)> Public MD5 As Guid
    <DataMember(Order:=2)> Public Id As CIdentity
    <DataMember(Order:=3)> Public SchemaMD5 As Guid
    <DataMember(Order:=4)> Public AppSettings As Dictionary(Of String, String)
    <DataMember(Order:=5)> Public ConnectionStrings As Dictionary(Of String, String)
    <DataMember(Order:=6)> Public LogFiles As Dictionary(Of String, Long)
    <DataMember(Order:=7)> Public Schema As CSchemaInfo
    <DataMember(Order:=8)> Public Folder As CFolderHash

    'Pre-Cons
    Shared Sub New()
        CProto.Prepare(Of CMyVersion)()
    End Sub

    Public ReadOnly Property MD5_Base64 As String
        Get
            Return CBinary.ToBase64(MD5, 10)
        End Get
    End Property
    Public ReadOnly Property MD5_Base64Trunc As String
        Get
            Return CBinary.ToBase64(MD5, 10)
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
