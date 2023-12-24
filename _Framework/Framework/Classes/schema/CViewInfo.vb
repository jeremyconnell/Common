Imports Framework
Imports ProtoBuf
Imports System.Runtime.Serialization

<DataContract>
Public Class CViewInfo : Implements IComparable(Of CViewInfo)

    'Data
    <DataMember(Order:=1)> Public ViewName As String
    <DataMember(Order:=2)> Public Columns As CColumnList
    <DataMember(Order:=3)> Public Script As String
    <DataMember(Order:=4)> Public MD5 As Guid

    'Preconstructor
    Shared Sub New()
        CProto.Prepare(Of CViewInfo)()
    End Sub

    'Constructor
    Protected Sub New()
    End Sub
    Public Sub New(viewName As String, cols As List(Of String), script As String)
        Me.ViewName = viewName
        Me.Script = script
        Me.Columns = New CColumnList(cols)
		Me.MD5 = CBinary.MD5_(Normalise(Me.Script))
	End Sub


    Public Overrides Function ToString() As String
        Return String.Concat(ViewName, " {", Columns.NamesAbc, "}")
    End Function

    'Scripting

    Public Overridable Function CreateScript() As String
        Return Script
    End Function
    Public Overridable Function DropScript() As String
        Return String.Concat("DROP VIEW ", ViewName)
    End Function

	Public Function CompareTo(other As CViewInfo) As Integer Implements IComparable(Of CViewInfo).CompareTo
		Dim i As Integer = Me.ViewName.CompareTo(other.ViewName)
		If i <> 0 Then Return i
		i = Me.Columns.MD5.CompareTo(other.Columns.MD5)
		If i <> 0 Then Return i
		Return Me.MD5.CompareTo(other.MD5)
	End Function



	Friend Shared Function Normalise(s As String) As String
		Return s.Replace(vbCrLf, " ").Replace(vbCr, " ").Replace(vbLf, " ").Replace(vbTab, " ").Trim().Replace("  ", " ").Replace("  ", " ").Replace("  ", " ")
	End Function
End Class