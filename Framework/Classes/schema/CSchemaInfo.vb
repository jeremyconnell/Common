Imports ProtoBuf
Imports System.Runtime.Serialization
Imports System.Text

<DataContract, ProtoContract>
Public Class CSchemaInfo
	'High-level
	<DataMember(Order:=11)> Public Tables As CTableInfoList
	<DataMember(Order:=12)> Public Views As CViewInfoList
	<DataMember(Order:=13)> Public AllColumns As CColumnList
	<DataMember(Order:=14)> Public Procs As CProcedureList
	<DataMember(Order:=15)> Public PrimaryKeys As CPrimaryKeyList
	<DataMember(Order:=17)> Public ForeignKeys As CForeignKeyList
	<DataMember(Order:=18)> Public Indexes As CIndexInfoList


	'Data
	<DataMember(Order:=21)> Public Migration As CMigration
	<DataMember(Order:=22)> Public MigrationHistory As CMigrationHistory

	'Preconstructor
	Shared Sub New()
		CProto.Prepare(Of CSchemaInfo)()
	End Sub

    'Constructors    
    Public Sub New(db As CDataSrc, Optional fullMigrationHistory As Boolean = False, Optional sysTables As Boolean = False, Optional onlyTablesWithPrefix As List(Of String) = Nothing)
        'Processed
        Me.Tables = New CTableInfoList(db, sysTables)
        Me.Views = New CViewInfoList(db)
        Me.Procs = New CProcedureList(db)

        'trim list of tables (optional)
        If Not IsNothing(onlyTablesWithPrefix) AndAlso onlyTablesWithPrefix.Count > 0 Then
            Dim temp As New CTableInfoList()

            For Each i As CTableInfo In Tables
                Dim match As Boolean = False
                For Each j As String In onlyTablesWithPrefix
                    If i.TableName.ToLower.Contains("." & j.ToLower()) Then
                        match = True
                        Exit For
                    End If
                Next

                If match Then
                    temp.Add(i)

                    For Each pk As CPrimaryKey In Me.Tables.PrimaryKeys
                        If pk.SchemaAndTable = i.TableName Then temp.PrimaryKeys.Add(pk)
                    Next
                    For Each ind As CIndexInfo In Me.Tables.Indexes
                        If ind.TableName = i.TableName Then temp.Indexes.Add(ind)
                    Next
                    For Each fk As CForeignKey In Me.Tables.ForeignKeys
                        If fk.TableName = i.TableName Or fk.ReferenceTable = i.TableName Then temp.ForeignKeys.Add(fk)
                    Next
                End If
            Next
            Tables = temp
        End If

        Me.PrimaryKeys = Me.Tables.PrimaryKeys
        Me.Indexes = Me.Tables.Indexes
        Me.ForeignKeys = Me.Tables.ForeignKeys
        Me.AllColumns = New CColumnList(Me.Tables.AllColumns)
        Me.AllColumns.AddRange(Me.Views.AllColumns)


        'Migrations
        Me.Migration = New CMigration
        Try
            If Me.Tables.Has("dbo.__MigrationHistory") Then Me.Migration = db.LatestMigration
        Catch
        End Try

        Me.MigrationHistory = New CMigrationHistory()
        Try
            If fullMigrationHistory Then
                If Me.Tables.Has("dbo.__MigrationHistory") Then Me.MigrationHistory = db.MigrationHistory
            End If
        Catch
        End Try
    End Sub
    Friend Sub New()
		Me.Tables = New CTableInfoList
		Me.Views = New CViewInfoList
		Me.Procs = New CProcedureList
		Me.Indexes = New CIndexInfoList
		Me.PrimaryKeys = New CPrimaryKeyList
		Me.ForeignKeys = New CForeignKeyList
		Me.Migration = New CMigration
	End Sub

	Public Function Diff(ref As CSchemaInfo) As CSchemaDiff
		Dim d As New CSchemaDiff

		Diff(Me.PrimaryKeys, ref.PrimaryKeys, d.Missing.PrimaryKeys, d.Extra.PrimaryKeys, d.Same.PrimaryKeys, d.Diff.PrimaryKeys)
		Diff(Me.Tables, ref.Tables, d.Missing.Tables, d.Extra.Tables, d.Same.Tables, d.Diff.Tables)
		Diff(Me.Views, ref.Views, d.Missing.Views, d.Extra.Views, d.Same.Views, d.Diff.Views)
		Diff(Me.Procs, ref.Procs, d.Missing.Procs, d.Extra.Procs, d.Same.Procs, d.Diff.Procs)

		d.IndexDiff = Me.Indexes.Diff(ref.Indexes)
		d.ProcDiff = Me.Procs.Diff(ref.Procs)
		d.FKDiff = Me.ForeignKeys.Diff(ref.ForeignKeys)
		d.ViewDiff = Me.Views.Diff(ref.Views)
		d.TableDiff = Me.Tables.Diff(ref.Tables)

		Return d
	End Function

	Public ReadOnly Property MD5_() As String
		Get
			Return CBinary.ToBase64(MD5, 10)
		End Get
	End Property

	Public Function MD5() As Guid
		Dim guids As New List(Of Guid)
		guids.Add(Me.Tables.MD5)
		guids.Add(Me.Views.MD5)
		guids.Add(Me.Procs.MD5)
		guids.Add(Me.ForeignKeys.MD5)
		guids.Add(Me.Indexes.MD5)
		guids.Add(Me.Migration.MD5)

		Return CBinary.MD5_(guids)
	End Function

	Private Shared Sub Diff(this As CViewInfoList, ref As CViewInfoList, ByRef missing As CViewInfoList, ByRef added As CViewInfoList, ByRef same As CViewInfoList, ByRef diff As CViewInfoList)
		For Each i As CViewInfo In this
			If ref.Index.ContainsKey(i.MD5) Then
				same.Add(i)
			ElseIf ref.Has(i.ViewName) Then
				diff.Add(i)
			Else
				added.Add(i)
			End If
		Next
		For Each i As CViewInfo In ref
			If Not this.Index.ContainsKey(i.MD5) Then
				If Not this.Has(i.ViewName) Then
					missing.Add(i)
				Else
					If Not diff.Has(i.ViewName) Then diff.Add(i)
				End If
			End If
		Next
	End Sub
	Private Shared Sub Diff(this As CPrimaryKeyList, ref As CPrimaryKeyList, ByRef missing As CPrimaryKeyList, ByRef added As CPrimaryKeyList, ByRef same As CPrimaryKeyList, ByRef diff As CPrimaryKeyList)
		For Each i As CPrimaryKey In this
			If ref.Index.ContainsKey(i.MD5) Then
				same.Add(i)
			ElseIf ref.Has(i.KeyName) Then
				diff.Add(i)
			Else
				added.Add(i)
			End If
		Next
		For Each i As CPrimaryKey In ref
			If Not this.Index.ContainsKey(i.MD5) Then
				If Not this.Has(i.KeyName) Then
					missing.Add(i)
				Else
					If Not diff.Has(i.KeyName) Then diff.Add(i)
				End If
			End If
		Next
	End Sub
	Private Shared Sub Diff(this As CTableInfoList, ref As CTableInfoList, ByRef missing As CTableInfoList, ByRef added As CTableInfoList, ByRef same As CTableInfoList, ByRef diff As CTableInfoList)
		For Each i As CTableInfo In this
			If ref.Index.ContainsKey(i.MD5) Then
				same.Add(i)
			ElseIf ref.Has(i.TableName) Then
				diff.Add(i)
			Else
				added.Add(i)
			End If
		Next
		For Each i As CTableInfo In ref
			If Not this.Index.ContainsKey(i.MD5) Then
				If Not this.Has(i.TableName) Then
					missing.Add(i)
				Else
					If Not diff.Has(i.TableName) Then diff.Add(i)
				End If
			End If
		Next
	End Sub
	Private Shared Sub Diff(this As CProcedureList, ref As CProcedureList, ByRef missing As CProcedureList, ByRef added As CProcedureList, ByRef same As CProcedureList, ByRef diff As CProcedureList)
		For Each i As CProcedure In this
			If ref.Index.ContainsKey(i.MD5) Then
				same.Add(i)
			ElseIf ref.Has(i.Name) Then
				diff.Add(i)
			Else
				added.Add(i)
			End If
		Next
		For Each i As CProcedure In ref
			If Not this.Index.ContainsKey(i.MD5) Then
				If Not this.Has(i.Name) Then
					missing.Add(i)
				Else
					If Not diff.Has(i.Name) Then diff.Add(i)
				End If
			End If
		Next
	End Sub


	Public Function MigrationHistory_Lazy(db As CDataSrc) As CMigrationHistory
		If IsNothing(Me.MigrationHistory) OrElse Me.MigrationHistory.Count = 0 Then
			Me.MigrationHistory = New CMigrationHistory(db)
		End If
		Return Me.MigrationHistory
	End Function


	Public Function ForceMigrationHistory(refVersion As CSchemaInfo, db As CDataSrc, refDb As CDataSrc) As Boolean

		Dim history As CMigrationHistory = refDb.MigrationHistory()
		Dim changes As CMigrationHistory = history.GetChanges(Me.Migration)

		For Each i As CMigration In changes
			i.InsertInto(db)
		Next

		'Update cache
		Me.MigrationHistory = Nothing
		Me.Migration = Nothing
		Return True
	End Function

	Public Function ForceMigrationScript(ref As CSchemaInfo, db As CDataSrc, dbRef As CDataSrc) As String
		Dim sb As New StringBuilder()
		For Each cmd As CCommand In ForceMigrationCommands(ref, db, dbRef)
			If sb.Length = 0 Then sb.AppendLine(cmd.Text)
			sb.Append("{")

			For Each p As IDataParameter In cmd.ParametersNamed
				If TypeOf (p.Value) Is Byte() Then
					sb.Append(CUtilities.CountSummary(CType(p.Value, Byte()).Length, "byte")).Append(",")
				ElseIf TypeOf (p.Value) Is Integer Then
					sb.Append(CUtilities.CountSummary(CInt(p.Value), "byte")).Append(",")
				Else
					sb.Append(p.Value).Append(",")
				End If
			Next
			sb.Append("}")
		Next
		Return sb.ToString()
	End Function

	Public Function ForceMigrationCommands(ref As CSchemaInfo, db As CDataSrc, refDb As CDataSrc) As List(Of CCommand)
		Dim refHistory As CMigrationHistory = ref.MigrationHistory_Lazy(refDb)
		Dim changes As CMigrationHistory = refHistory.GetChanges(Me.Migration)

		Dim list As New List(Of CCommand)()
		For Each i As CMigration In changes
			list.Add(i.InsertCmd_(db))
		Next
		Return list

	End Function




    Public Overrides Function ToString() As String
        Dim sb As New Text.StringBuilder
        If Me.Tables.Count > 0 Then
            sb.Append(CUtilities.NameAndCount("Table", Me.Tables.Names))
        End If
        If Me.Views.Count > 0 Then
            sb.Append(CUtilities.NameAndCount("View", Me.Views.Names))
        End If
        If Me.Tables.AllColumns.Count > 0 Then
            If sb.Length > 0 Then sb.Append(", ")
            sb.Append(CUtilities.NameAndCount("Columns", Me.Tables.AllColumns.Count + Me.Views.AllColumns.Count))
        End If
        If Me.Procs.Count > 0 Then
            If sb.Length > 0 Then sb.Append(", ")
            sb.Append(CUtilities.NameAndCount("Proc", Me.Procs.StoredProcs))
        End If
        If Me.Procs.Functions.Count > 0 Then
            If sb.Length > 0 Then sb.Append(", ")
            sb.Append(CUtilities.NameAndCount("Fn", Me.Procs.Functions))
        End If
        If Me.ForeignKeys.Count > 0 Then
            If sb.Length > 0 Then sb.Append(", ")
            sb.Append(CUtilities.NameAndCount("FK", Me.ForeignKeys))
        End If
        If Me.PrimaryKeys.Count > 0 Then
            If sb.Length > 0 Then sb.Append(", ")
            sb.Append(CUtilities.NameAndCount("PK", Me.PrimaryKeys))
        End If
        If Me.Indexes.Normal.Count > 0 Then
            If sb.Length > 0 Then sb.Append(", ")
            sb.Append(CUtilities.NameAndCount("Index", Me.Indexes.Normal))
        End If
        If Me.Indexes.Unique.Count > 0 Then
            If sb.Length > 0 Then sb.Append(", ")
            sb.Append(CUtilities.NameAndCount("Unique", Me.Indexes.Unique))
        End If
        If Not IsNothing(Me.Migration) Then
            If sb.Length > 0 Then sb.Append(", ")
            sb.Append("Migration: ").Append(Me.Migration.MigrationId)
        End If
        Return sb.ToString
    End Function


#Region "Guess PK/FK/Index"
    Public Function GuessPrimaryKeys() As CPrimaryKeyList
        Dim pks As New CPrimaryKeyList
        For Each i As CTableInfo In Tables
            Dim pk As CPrimaryKey = i.PrimaryKey
            If pk.ColumnNames.Count > 0 Then Continue For
            Dim first As CColumn = i.Columns(0)
            pk.ColumnNames.Add(first.Name)
            pk.IsIdentity = (first.Type = "INT")
            pk.KeyName = "PK_" + i.TableName
            pks.Add(pk)
        Next
        Return pks
    End Function
    Public Function GuessForeignKeys() As CForeignKeyList
        Dim pks As New Dictionary(Of String, CTableInfo)
        For Each i As CTableInfo In Tables
            Dim pk As CPrimaryKey = i.PrimaryKey
            If pk.ColumnNames.Count <> 1 Then Continue For
            Dim col As String = pk.ColumnNames(0)
            If pks.ContainsKey(col) Then pks.Remove(col)
            pks.Add(col, i)
        Next

        Dim fks As New CForeignKeyList
        For Each i As CTableInfo In Tables
            Dim pk As CPrimaryKey = i.PrimaryKey
            For Each j As CColumn In i.Columns
                If pk.ColumnNames.Contains(j.Name) Then Continue For
                For Each k As String In pks.Keys
                    If j.Name.EndsWith(k) AndAlso j.Name <> k Then
                        Dim col_reftbl_refcol As String = String.Concat(j.Name, "/", pks(k).TableName, "/", k)
                        Dim name As String = "FK_" & i.TableName & "_" & j.Name
                        Dim fk As New CForeignKey1(i.TableName, name, col_reftbl_refcol, False, True)
                        fks.Add(fk)
                    End If
                Next
            Next
        Next
        Return fks
    End Function
    Public Function CreateIndexes(fks As CForeignKeyList) As CIndexInfoList
        Dim list As New CIndexInfoList(fks.Count)
        For Each i As CForeignKey In fks
            If i.NumOfCols > 1 Then Continue For
            If TypeOf i Is CForeignKey1 Then
                Dim fk1 As CForeignKey1 = CType(i, CForeignKey1)
                Dim fkcols As New List(Of String)
                fkcols.Add(fk1.ColumnName)
                Dim name As String = "IDX_" & i.TableName & "_" & fk1.ColumnName
                Dim idx As New CIndexInfo(i.TableName, name, False, fkcols)
                list.Add(idx)
            Else
                Dim fk As CForeignKeyN = CType(i, CForeignKeyN)
                Dim name As String = "IDX_" & i.TableName & "_" & CUtilities.ListToString(fk.ColumnNames)
                Dim idx As New CIndexInfo(i.TableName, name, False, fk.ColumnNames)
                list.Add(idx)
            End If
        Next
        Return list
    End Function
#End Region

#Region "Insert Order"
    Public ReadOnly Property Tables_InsertOrder As CTableInfoList
        Get
            Dim temp As New CTableInfoList()
            Dim copyTb As New List(Of CTableInfo)(Me.Tables)
            Dim copyFk As New CForeignKeyList(Me.ForeignKeys)
            Dim loopCount As Integer = 0
            While copyTb.Count > 0
                'Exit if stuck on mutually-referencing tables
                loopCount += 1
                If loopCount = 10 Then
                    temp.AddRange(copyTb)
                    Return temp
                End If

                'Take tables with no foreign keys
                For Each i As CTableInfo In copyTb
                    If copyFk.GetByTable(i.TableName).Count = 0 Then
                        temp.Add(i)
                    End If
                Next
                'Remove fks for those ref-tables
                Dim tempFk As New CForeignKeyList()
                For Each i As CTableInfo In temp
                    If copyTb.Contains(i) Then
                        'Remove the table
                        copyTb.Remove(i)
                        'identify fks that point to it
                        tempFk.AddRange(copyFk.GetByRefTable(i.TableName))
                    End If
                Next
                'Remove those fks
                For Each i As CForeignKey In tempFk
                    copyFk.Remove(i)
                Next
                'Reset the index
                copyFk = New CForeignKeyList(copyFk)

            End While
            Return temp
        End Get
    End Property
#End Region

End Class