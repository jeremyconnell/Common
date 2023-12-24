Imports ProtoBuf
Imports System.Runtime.Serialization

<DataContract, ProtoContract>
Public Class CForeignKey1 : Inherits CForeignKey

    <DataMember(Order:=1)> Public ColumnName As String
    <DataMember(Order:=2)> Public ReferenceColumnName As String

    'Preconstructor
    Shared Sub New()
        CProto.Prepare(Of CForeignKey1)()
    End Sub

    'Constructor
    Protected Sub New()
    End Sub
	Public Sub New(tableName As String, keyName As String, joined As String, cascadeUpdate As Boolean, cascadeDelete As Boolean)
		Me.New(tableName, keyName, CUtilities.SplitOn(joined, "/"), cascadeUpdate, cascadeDelete)
	End Sub
	Private Sub New(tableName As String, keyName As String, parts As List(Of String), cascadeUpdate As Boolean, cascadeDelete As Boolean)
		Me.New(tableName, keyName, parts(0), parts(1), parts(2), cascadeUpdate, cascadeDelete)
	End Sub
	Private Sub New(tableName As String, keyName As String, colName As String, refTable As String, refCol As String, cascadeUpdate As Boolean, cascadeDelete As Boolean)
		MyBase.New(tableName, keyName, refTable, cascadeUpdate, cascadeDelete)

		Me.ColumnName = colName
		Me.ReferenceColumnName = refCol

		Me.MD5 = CBinary.MD5_(Me.ReferenceTable, Me.KeyName, Me.ColumnName, Me.ReferenceColumnName, Me.CascadeUpdate.ToString, Me.CascadeDelete.ToString)
	End Sub


	Public Overrides ReadOnly Property NumOfCols As Integer
        Get
            Return 1
        End Get
    End Property
    Public Overrides ReadOnly Property ColumnNames_ As String
        Get
            Return String.Concat("[", ColumnName, "]")
        End Get
    End Property
    Public Overrides ReadOnly Property RefColumnNames_ As String
        Get
            Return String.Concat("[", ReferenceColumnName, "]")
        End Get
    End Property

End Class