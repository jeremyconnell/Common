Imports ProtoBuf
Imports System.Runtime.Serialization
Imports Framework

<DataContract>
Public Class CTableInfo : Implements IComparable(Of CTableInfo)
    'Data
    <DataMember(Order:=1)> Public TableName As String
    <DataMember(Order:=2)> Public Columns As CColumnList
    <DataMember(Order:=3)> Public PrimaryKey As CPrimaryKey
    <DataMember(Order:=4)> Public ForeignKeys As CForeignKeyList
    <DataMember(Order:=5)> Public Indexes As CIndexInfoList
    <DataMember(Order:=6)> Public MD5 As Guid

    'Preconstructor
    Shared Sub New()
        CProto.Prepare(Of CTableInfo)()
    End Sub

    'Constructors
    Protected Sub New()
    End Sub
    Public Sub New(table As String, cols As List(Of String), pk As CPrimaryKey, fks As CForeignKeyList, indexes As CIndexInfoList)
        If IsNothing(pk) Then pk = New CPrimaryKey()
        Me.TableName = table
            Me.Columns = New CColumnList(cols)
        Me.PrimaryKey = pk
        Me.ForeignKeys = fks
        Me.Indexes = indexes

		MD5 = CBinary.MD5_(CBinary.MD5_(TableName.ToLower()), Me.Columns.MD5, Me.PrimaryKey.MD5, Me.ForeignKeys.MD5, Me.Indexes.MD5)
	End Sub
    Public Overrides Function ToString() As String
        Return String.Concat(TableName, ": PK=", Me.PrimaryKey.ColumnNames_, " FKs=", ForeignKeys.Count, ", IDXs=", Indexes.Count, " COLS=", Columns.Count)
    End Function

	Public ReadOnly Property TableName_ As String
		Get
			Return String.Concat("[", TableName.Replace(".", "].["), "]")
		End Get
	End Property

	Public Function CreateScript() As String
		Dim sb As New Text.StringBuilder("CREATE TABLE ")
		sb.AppendLine(TableName_)
		sb.AppendLine(" (")
        For Each i As CColumn In Columns
            sb.Append(i.Script)
            If Me.PrimaryKey.IsIdentity AndAlso Me.PrimaryKey.ColumnNames.Contains(i.Name) Then
                sb.Append(" IDENTITY(1,1)")
            End If
            sb.AppendLine(",")
        Next
        sb.Remove(sb.Length - 1, 1)
        With PrimaryKey
            If .ColumnNames.Count > 0 Then
                sb.Append(" CONSTRAINT ")
                sb.Append("[").Append(.KeyName).Append("] PRIMARY KEY CLUSTERED (")
                sb.Append(.ColumnNames_).AppendLine(") ON [PRIMARY]")
            End If
        End With
        sb.AppendLine(")  ON [PRIMARY]")    ' TEXTIMAGE_ON [PRIMARY]
        Return sb.ToString
    End Function
    Public Function DropScript() As String
		Return String.Concat("DROP TABLE [", TableName.Replace(".", "].["), "]")
	End Function

    Public Function CompareTo(other As CTableInfo) As Integer Implements IComparable(Of CTableInfo).CompareTo
        Dim i As Integer = Me.TableName.CompareTo(other.TableName)
        If i <> 0 Then Return i
        Return Me.MD5.CompareTo(other.MD5)
    End Function
End Class