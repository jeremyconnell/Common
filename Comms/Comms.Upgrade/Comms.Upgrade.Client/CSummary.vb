Imports System.Runtime.Serialization

<DataContract(), ProtoContract(), Serializable()>
Public Class CSummary
    'Data
    <ProtoMember(1)> Public Deletes As List(Of String)
    <ProtoMember(2)> Public AddEdits As List(Of String)
    <ProtoMember(3)> Public AppId As Integer
    <ProtoMember(4)> Public Instance As String


    'Derived
    Public ReadOnly Property Count As Integer
        Get
            Return Deletes.Count + AddEdits.Count
        End Get
    End Property

    'Constructors
    Public Sub New(appId As Integer, instance As String)
        Me.AppId = appId
        Me.Instance = instance

        Deletes = New List(Of String)(0)
        AddEdits = New List(Of String)(0)
    End Sub
    Public Sub New(diff As List(Of CFileNameAndContent), appId As Integer, instance As String)
        Me.AppId = appId
        Me.Instance = instance

        Deletes = New List(Of String)(diff.Count)
        AddEdits = New List(Of String)(diff.Count)
        For Each i As CFileNameAndContent In diff
            If IsNothing(i.Content) Then
                Deletes.Add(i.Name)
            Else
                AddEdits.Add(i.Name)
            End If
        Next
    End Sub

    'Deserialise
    Public Shared Function Deserialise(binary As Byte()) As CSummary
        Return CProto.Deserialise(Of CSummary)(binary)
    End Function

    'static constructor
    Shared Sub New()
        CProto.Prepare(Of CSummary)()
    End Sub

    'Presentation

    Public Function Summarise() As String
        Dim name As String = String.Concat("#", AppId, ": ", Instance)
        If Me.Count = 0 Then Return String.Concat("No updates for ", name)

        Dim sb As New Text.StringBuilder("Upgrading ")
        sb.Append(name).Append(" (")
        sb.Append(Me.Count.ToString("n0"))
        sb.AppendLine(" changes)")
        For Each i As String In Me.Deletes
            sb.Append(vbTab).Append("deleted:").Append(vbTab).AppendLine(i)
        Next
        For Each i As String In Me.AddEdits
            sb.Append(vbTab).Append("updated:").Append(vbTab).AppendLine(i)
        Next
        Return sb.ToString
    End Function
End Class
