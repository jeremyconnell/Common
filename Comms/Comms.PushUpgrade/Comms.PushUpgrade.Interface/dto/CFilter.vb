Imports System.Runtime.Serialization

<DataContract()>
Public Class CFilter
    'Data
    <DataMember(Order:=1)> Public IgnoreExtensions As String
    <DataMember(Order:=2)> Public Recursive As Boolean

    'Protobuf
    Shared Sub New()
        CProto.Prepare(Of CFilter)()
    End Sub

    'Constructors
    Public Sub New(ignore As String, recurse As Boolean)
        Me.IgnoreExtensions = ignore
        Me.Recursive = recurse
    End Sub
    Private Sub New()
    End Sub

    'Properties
    Public ReadOnly Property Ignore As String()
        Get
            Return CUtilities.StringToListStr(IgnoreExtensions).ToArray()
        End Get
    End Property
End Class
