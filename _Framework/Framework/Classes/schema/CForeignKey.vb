Imports ProtoBuf
Imports System.Runtime.Serialization

<DataContract, ProtoContract,
    ProtoInclude(100, GetType(CForeignKey1)),
    ProtoInclude(101, GetType(CForeignKeyN)),
    KnownType(GetType(CForeignKey1)),
    KnownType(GetType(CForeignKeyN))>
Public MustInherit Class CForeignKey : Implements IComparable(Of CForeignKey)
    'DAta
    <DataMember(Order:=1)> Public KeyName As String
    <DataMember(Order:=2)> Public TableName As String
    <DataMember(Order:=3)> Public ReferenceTable As String
	<DataMember(Order:=4)> Public MD5 As Guid
	<DataMember(Order:=5)> Public CascadeUpdate As Boolean
	<DataMember(Order:=6)> Public CascadeDelete As Boolean

	'Preconstructor
	Shared Sub New()
        CProto.Prepare(Of CForeignKey)()
    End Sub

    'Constructors
    Protected Sub New()
    End Sub
	Protected Sub New(tableName As String, keyName As String, refTable As String, cascadeUpdate As Boolean, cascadeDelete As Boolean)
		Me.TableName = tableName
		Me.KeyName = keyName
		Me.ReferenceTable = refTable
		Me.CascadeUpdate = cascadeUpdate
		Me.CascadeDelete = cascadeDelete
	End Sub

	Public Overrides Function ToString() As String
        Return String.Concat(TableName, " {", ColumnNames_, "} => ", ReferenceTable, "{", RefColumnNames_, "} ", KeyName)
    End Function
    Public Overrides Function GetHashCode() As Integer
        Return MD5.GetHashCode()
    End Function
    Public Overrides Function Equals(obj As Object) As Boolean
        Return Me.MD5.Equals(CType(obj, CForeignKey).MD5)
    End Function

    Public MustOverride ReadOnly Property NumOfCols As Integer
    Public MustOverride ReadOnly Property ColumnNames_ As String
    Public MustOverride ReadOnly Property RefColumnNames_ As String

	Public ReadOnly Property TableName_ As String
		Get
			Return String.Concat("[", TableName.Replace(".", "].["), "]")
		End Get
	End Property
	Public ReadOnly Property ReferenceTable_ As String
		Get
			Return String.Concat("[", ReferenceTable.Replace(".", "].["), "]")
		End Get
	End Property

	Public Function CreateScript() As String
		Return String.Concat("ALTER TABLE ", TableName_, " With CHECK ADD CONSTRAINT [", KeyName, "] FOREIGN KEY(", ColumnNames_, ") REFERENCES ", ReferenceTable_, "(", RefColumnNames_, ")", IIf(CascadeDelete, " ON DELETE CASCADE", ""), IIf(CascadeUpdate, " ON UPDATE CASCADE", ""), vbCrLf, "ALTER TABLE ", TableName_, " CHECK CONSTRAINT [", KeyName, "]")
	End Function
    Public Function DropScript() As String
		Return String.Concat("ALTER TABLE ", TableName_, " DROP CONSTRAINT [", KeyName, "]")
	End Function

    Public Function CompareTo(other As CForeignKey) As Integer Implements IComparable(Of CForeignKey).CompareTo
        Dim i As Integer = Me.TableName.CompareTo(other.TableName)
        If i <> 0 Then Return i
        i = Me.ReferenceTable.CompareTo(other.ReferenceTable)
        If i <> 0 Then Return i
        i = Me.ColumnNames_.CompareTo(other.ColumnNames_)
        If i <> 0 Then Return i
        i = Me.RefColumnNames_.CompareTo(other.RefColumnNames_)
        If i <> 0 Then Return i
        Return Me.KeyName.CompareTo(other.KeyName)
    End Function
End Class


