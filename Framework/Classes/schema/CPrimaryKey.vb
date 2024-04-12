Imports System.Text
Imports ProtoBuf
Imports System.Runtime.Serialization

<DataContract>
Public Class CPrimaryKey
    'Data
    <DataMember(Order:=1)> Public KeyName As String
    <DataMember(Order:=2)> Public ColumnNames As List(Of String)
    <DataMember(Order:=3)> Public MD5 As Guid
    <DataMember(Order:=4)> Public SchemaAndTable As String
    <DataMember(Order:=5)> Public IsIdentity As Boolean
    <DataMember(Order:=6)> Public LastValue As Long

    'Pre-constructors
    Shared Sub New()
        CProto.Prepare(Of CPrimaryKey)()
    End Sub

    'Constructor
    Public Sub New()
        Me.SchemaAndTable = String.Empty
        Me.KeyName = String.Empty
        Me.ColumnNames = New List(Of String)(0)
    End Sub
    Public Sub New(dr As DataRow)
        Me.SchemaAndTable = CAdoData.GetStr(dr, "SchemaAndTableName")
        Me.KeyName = CAdoData.GetStr(dr, "PKName")
        Me.ColumnNames = New List(Of String)(1)
        Me.ColumnNames.Add(CAdoData.GetStr(dr, "ColumnName"))
        Me.IsIdentity = CAdoData.GetBool(dr, "IsIdentity")
        Me.LastValue = CAdoData.GetLong(dr, "LastValue") 'Not hashed

        Me.MD5 = CBinary.MD5_(Me.SchemaAndTable, Me.KeyName, Me.ColumnNames_, Me.IsIdentity.ToString)
    End Sub

	Public ReadOnly Property ColumnNames_ As String
		Get
			Dim sb As New StringBuilder
			For Each i As String In ColumnNames
				If sb.Length > 0 Then sb.Append(",")
				sb.Append("[").Append(i).Append("] ASC")
			Next
			Return sb.ToString
		End Get
	End Property

	Public Function CreateScript() As String
		Return String.Concat("ALTER TABLE ", SchemaAndTable, " ADD  CONSTRAINT [", KeyName, "] PRIMARY KEY CLUSTERED (", ColumnNames_, ") ON [PRIMARY]")
	End Function
	Public Function DropScript() As String
		Return String.Concat("ALTER TABLE ", SchemaAndTable, " DROP CONSTRAINT [", KeyName, "]")
	End Function

	Public Overrides Function ToString() As String
        Return String.Concat("{", ColumnNames, "}")
    End Function
End Class