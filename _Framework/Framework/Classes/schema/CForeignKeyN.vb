Imports ProtoBuf
Imports System.Runtime.Serialization

<DataContract, ProtoContract>
Public Class CForeignKeyN : Inherits CForeignKey

    <DataMember(Order:=1)> Public ColumnNames As List(Of String)
    <DataMember(Order:=2)> Public ReferenceColumns As List(Of String)

    'Preconstructor
    Shared Sub New()
        CProto.Prepare(Of CForeignKeyN)()
    End Sub

	'Constructor
	Protected Sub New()
	End Sub

	Public Sub New(fk As CForeignKey1, addCol As String)
		MyBase.New(fk.TableName, fk.KeyName, fk.ReferenceTable, fk.CascadeUpdate, fk.CascadeDelete)

		Dim parts As List(Of String) = CUtilities.SplitOn(addCol, "/")

		Me.ColumnNames = New List(Of String)
		Me.ColumnNames.Add(fk.ColumnName)
		Me.ColumnNames.Add(parts(0))

		Me.ReferenceColumns = New List(Of String)
		Me.ReferenceColumns.Add(fk.ReferenceColumnName)
		Me.ReferenceColumns.Add(parts(2))


		Me.MD5 = CBinary.MD5_(Me.ReferenceTable, Me.KeyName, Me.ColumnNames_, Me.RefColumnNames_, Me.CascadeUpdate.ToString, Me.CascadeDelete.ToString)
	End Sub
	Public Sub New(fk As CForeignKeyN, addCol As String)
		MyBase.New(fk.TableName, fk.KeyName, fk.ReferenceTable, fk.CascadeUpdate, fk.CascadeDelete)

		Dim parts As List(Of String) = CUtilities.SplitOn(addCol, "/")

		Me.ColumnNames = fk.ColumnNames
		Me.ColumnNames.Add(parts(0))

		Me.ReferenceColumns = fk.ReferenceColumns
		Me.ReferenceColumns.Add(parts(2))


		Me.MD5 = CBinary.MD5_(Me.ReferenceTable, Me.KeyName, Me.ColumnNames_, Me.RefColumnNames_, Me.CascadeUpdate.ToString, Me.CascadeDelete.ToString)
	End Sub


	Public Overrides ReadOnly Property NumOfCols As Integer
        Get
            Return ColumnNames.Count
        End Get
    End Property
    Public Overrides ReadOnly Property ColumnNames_ As String
        Get
            Return String.Concat("[", CUtilities.ListToString(ColumnNames, "],["), "]")
        End Get
    End Property
    Public Overrides ReadOnly Property RefColumnNames_ As String
        Get
            Return String.Concat("[", CUtilities.ListToString(ReferenceColumns, "],["), "]")
        End Get
    End Property

End Class