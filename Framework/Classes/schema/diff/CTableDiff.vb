Imports System.Text

Public Class CTableDiff
    'Constructor
    Public Sub New(this As CTableInfo, ref As CTableInfo)
        Me.This = this
        Me.Ref = ref

		Me.Columns = New CColumnDiff(this.Columns, ref.Columns)
		Me.Indexes = this.Indexes.Diff(ref.Indexes)
		Me.Foreign = this.ForeignKeys.Diff(ref.ForeignKeys)

	End Sub

    'Data
    Public This As CTableInfo
    Public Ref As CTableInfo

    Public Columns As CColumnDiff
	Public Indexes As CIndexListDiff
	Public Foreign As CForeignKeyListDiff


	'Derived
	Public ReadOnly Property IsExact As Boolean
		Get
			Return This.MD5 = Ref.MD5
		End Get
	End Property
	Public ReadOnly Property IsDifferent As Boolean
		Get
			Return NameIsDifferent OrElse ColumnsAreDifferent OrElse IndexesAreDifferent OrElse ForeignKeysAreDifferent OrElse PrimaryKeysIsDifferent
		End Get
	End Property
	Public ReadOnly Property NameIsDifferent As Boolean
		Get
			Return This.TableName <> Ref.TableName
		End Get
	End Property
	Public ReadOnly Property ColumnsAreDifferent As Boolean
		Get
			Return Not Columns.IsExact
		End Get
	End Property
	Public ReadOnly Property IndexesAreDifferent As Boolean
		Get
			Return Not Indexes.IsExactMatch
		End Get
	End Property
	Public ReadOnly Property ForeignKeysAreDifferent As Boolean
		Get
			Return Not Foreign.IsExactMatch
		End Get
	End Property
	Public ReadOnly Property PrimaryKeysIsDifferent As Boolean
		Get
			Return This.PrimaryKey.MD5 <> Ref.PrimaryKey.MD5 OrElse This.PrimaryKey.KeyName <> Ref.PrimaryKey.KeyName
		End Get
	End Property

    'Scripting
    Public Function ChangeScripts() As List(Of String)
        If Not IsDifferent Then Return New List(Of String)(0)

        Dim list As New List(Of String)

        'Column changes
        If Columns.Missing.Count > 0 Then
            For Each j As CColumn In Columns.Missing
                list.Add(String.Concat("ALTER TABLE ", Ref.TableName_, " ADD ", j.Script))
            Next
        End If

        If Columns.Extra.Count > 0 Then
            For Each j As CColumn In Columns.Extra
                list.Add(String.Concat("ALTER TABLE ", Ref.TableName_, " DROP COLUMN ", j.Name_))
            Next
        End If

        For Each i As CColumn In Columns.Diff
            Dim r As CColumn = Ref.Columns.Item(i.Name)
            For Each j As CColumn In Columns.Diff
                list.Add(String.Concat("ALTER TABLE ", Ref.TableName_, " ALTER COLUMN ", j.Script))
            Next
            For Each j As CColumn In Columns.Missing
                list.Add(String.Concat("ALTER TABLE ", Ref.TableName_, " ADD COLUMN ", j.Script))
            Next
            For Each j As CColumn In Columns.Extra
                list.Add(String.Concat("ALTER TABLE ", Ref.TableName_, " DROP COLUMN ", j.Script))
            Next
        Next

        'PK changes
        If PrimaryKeysIsDifferent Then
            If This.PrimaryKey.KeyName.Length > 0 Then
                list.Add(This.PrimaryKey.DropScript)
            End If
            list.Add(Ref.PrimaryKey.CreateScript)
        End If

        list.AddRange(Indexes.ChangeScripts())
        list.AddRange(Foreign.ChangeScripts())

        Return list

    End Function
    Public Function ChangeScripts(missingTbls As CTableInfoList, extraTbls As CTableInfoList) As List(Of String)
		If Not IsDifferent Then Return New List(Of String)(0)

		Dim list As New List(Of String)

		'Adjust cols for whole missing tables
		Dim missing As New CColumnList(Columns.Missing)
		For Each tbl As CTableInfo In missingTbls
			For Each col As CColumn In tbl.Columns
				If missing.Has(col.Name) Then missing.Remove(col)
			Next
		Next

		Dim extra As New CColumnList(Columns.Extra)
		For Each tbl As CTableInfo In extraTbls
			For Each col As CColumn In tbl.Columns
				If extra.Has(col.Name) Then extra.Remove(col)
			Next
		Next

		'Column changes
		If missing.Count > 0 Then
			For Each j As CColumn In missing
				list.Add(String.Concat("ALTER TABLE ", Ref.TableName_, " ADD ", j.Script))
			Next
		End If

		If extra.Count > 0 Then
			For Each j As CColumn In extra
				list.Add(String.Concat("ALTER TABLE ", Ref.TableName_, " DROP COLUMN ", j.Name_))
			Next
		End If

		For Each i As CColumn In Columns.Diff
			Dim r As CColumn = Ref.Columns.Item(i.Name)
			For Each j As CColumn In Columns.Diff
				list.Add(String.Concat("ALTER TABLE ", Ref.TableName_, " ALTER COLUMN ", j.Script))
			Next
			For Each j As CColumn In Columns.Missing
				list.Add(String.Concat("ALTER TABLE ", Ref.TableName_, " ADD COLUMN ", j.Script))
			Next
			For Each j As CColumn In Columns.Extra
				list.Add(String.Concat("ALTER TABLE ", Ref.TableName_, " DROP COLUMN ", j.Script))
			Next
		Next

        'PK changes
        If PrimaryKeysIsDifferent Then
            If This.PrimaryKey.KeyName.Length > 0 Then
                list.Add(This.PrimaryKey.DropScript)
            End If
            list.Add(Ref.PrimaryKey.CreateScript)
        End If

        Return list
	End Function

	Public Overrides Function ToString() As String
		Dim list As New List(Of String)
		If NameIsDifferent Then list.Add(String.Concat("Name is different: ", This.TableName, "<>", Ref.TableName))
		If IndexesAreDifferent Then list.Add(String.Concat("Index is different"))
		If ColumnsAreDifferent Then list.Add(String.Concat("Cols are different"))
		If ForeignKeysAreDifferent Then list.Add(String.Concat("FKs are different"))
		If PrimaryKeysIsDifferent Then list.Add(String.Concat("PKs are different"))
		If list.Count = 0 Then list.Add("None")

		Return This.TableName & " : " & CUtilities.ListToString(list)
	End Function

End Class
