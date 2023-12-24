
Imports System.Data
Imports System.Threading.Tasks
Imports SchemaDeploy




Partial Class pages_self_usercontrols_UCSchemaDiff
	Inherits CUserControlWithTableHelpers

#Region "Source: Admin Local/Prod"
	Private LOCAL_DB As CDataSrc = CDataSrc.Default

	Private PROD_DB As New CWebSrcBinary("https://admin.controltrackonline.com")
	Private PROD_DB_SPARE As New CSqlClient("Server=tcp:controltracksaas.database.windows.net,1433;Initial Catalog=controltrackadmin;Persist Security Info=False;User ID=controltrackadmin;Password=5uGZ5kjaQ6+z;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;")


	Private _LOCAL As CSchemaInfo = Nothing
	Private _PROD As CSchemaInfo = Nothing
	Public ReadOnly Property Local As CSchemaInfo
		Get
			If IsNothing(_LOCAL) Then
				Try
					_LOCAL = New CSchemaInfo(LOCAL_DB)
				Catch ex As Exception
					CSession.PageMessageEx = ex
				End Try
			End If
			Return _LOCAL
		End Get
	End Property
	Public ReadOnly Property PROD As CSchemaInfo
		Get
			If IsNothing(_PROD) Then
				Try
					_PROD = New CSchemaInfo(PROD_DB)
				Catch ex As Exception
					Try
						_PROD = New CSchemaInfo(PROD_DB_SPARE)
					Catch ex2 As Exception
						CSession.PageMessage = ex.Message & vbCrLf & ex2.Message
					End Try
				End Try
			End If
			Return _PROD
		End Get
	End Property
#End Region


#Region "Data"
	Private ReadOnly Property RefInstanceId As Integer
		Get
			Return CDropdown.GetInt(ddRefInstance)
		End Get
	End Property
	Private ReadOnly Property RefSchema As CSchemaInfo
		Get
			If RefInstanceId = ESource.Local Then Return Local
			If RefInstanceId = ESource.Prod Then Return PROD
			Return RefInstance.SchemaInfo
		End Get
	End Property
	Private ReadOnly Property RefInstance As CInstance
		Get
			Return CInstance.Cache.GetById(RefInstanceId)
		End Get
	End Property
#End Region


#Region "Event Handlers: ddInstance/Refresh"
	Private Sub btnRefreshSchema_Click(sender As Object, e As EventArgs) Handles btnRefreshSchema.Click
		If m_admin Then Exit Sub

		For Each i As CInstance In CInstance.Cache
			i.SchemaInfo = Nothing
		Next
		Response.Redirect(Request.RawUrl)
	End Sub
	Private Sub ddRefInstance_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddRefInstance.SelectedIndexChanged
		Dim id As Integer = CDropdown.GetInt(ddRefInstance)

		If id = ESource.Local OrElse id = ESource.Prod Then
			CSession.Home_Schema_RefSourceId = id
		Else
			CSession.Home_Schema_RefInstanceId = id
		End If
		Response.Redirect(Request.RawUrl)
	End Sub
#End Region


#Region "Display - Admin"
	Private m_admin As Boolean = False
	Public Sub Display_Admin(target As ESource, fixId As EFix)
		m_admin = True
		btnRefreshSchema.Visible = False

		If Page.IsPostBack Then Exit Sub

		'Shared
		CDropdown.Add(ddRefInstance, "Local => Prod", ESource.Local)
		CDropdown.Add(ddRefInstance, "Prod => Local", ESource.Prod)
		CDropdown.SetValue(ddRefInstance, CSession.Home_Schema_RefSourceId)

		'Ref-Schema
		Dim trRef As TableRow = Nothing
		DiffAdmin(target, fixId)

		AddRows(tbl)
	End Sub
	Private Sub AddRows(tbl As Table)
		Dim trRef As TableRow = Nothing
		Dim infoRef As CSchemaInfo = Nothing
		Dim errRef As String = Nothing

		Dim prod As New CSqlClient("Server=tcp:controltracksaas.database.windows.net,1433;Initial Catalog=controltrackadmin;Persist Security Info=False;User ID=controltrackadmin;Password=5uGZ5kjaQ6+z;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;")
		Dim local As CDataSrc = CDataSrc.Default

		Dim prod2 As New CWebSrcBinary("https://admin.controltrackonline.com")
		Dim local2 As CWebSrcBinary = Nothing ' New CWebSrcBinary("http://" + Request.Url.Host)

		If RefInstanceId < 0 AndAlso RefInstanceId <> Integer.MinValue Then CSession.Home_Schema_RefSourceId = RefInstanceId
		Dim refId As ESource = CSession.Home_Schema_RefSourceId

		Dim trLocal As TableRow = AddRow(tbl, "Admin (LOCAL)", ESource.Local, trRef, infoRef, errRef, local, local2, "local", refId)
		Dim trAdmin As TableRow = AddRow(tbl, "Admin (PROD)", ESource.Prod, trRef, infoRef, errRef, prod, prod2, "controltrackadmin", refId)
	End Sub
	Private Function AddRow(tbl As Table, name As String, id As Integer, ByRef trRef As TableRow, ByRef infoRef As CSchemaInfo, ByRef errRef As String, db As CDataSrc, db2 As CWebSrc, toolTip As String, refId As Integer) As TableRow
		Dim tr As TableRow = Row(tbl)
		If IsNothing(trRef) Then trRef = tr

		'Admin
		CellH(tr, (tbl.Rows.Count - 1) & ".")
		CellLink(tr, name, CSitemap.ClientEdit(id)).ToolTip = toolTip

		'Data
		Dim info As CSchemaInfo = Nothing
		Dim errM As String = String.Empty
		Try
			If Not IsNothing(db2) Then
				info = db2.SchemaInfo
			Else
				info = db.SchemaInfo()
			End If
		Catch ex As Exception
			Try
				If Not IsNothing(db2) Then
					info = db.SchemaInfo
					errM = Nothing
				Else
					errM = ex.Message
				End If
			Catch ex2 As Exception
				errM = ex2.Message & vbCrLf & ex.Message
			End Try
		End Try

		If IsNothing(infoRef) Then
			infoRef = info
			errRef = errM
		End If

		Dim isRef As Boolean = (id = refId)
		AddRow(tr, id, isRef, info, infoRef, errM, errRef, trRef, db)

		Return tr
	End Function

#End Region

#Region "Display Clients/Instance"
	Public Sub Display_Instance(i As CInstance, diffId As Integer, fixId As EFix)
		'Dropdown
		ddRefInstance.DataSource = CInstance.Cache
		ddRefInstance.DataBind()
		CDropdown.SetValue(ddRefInstance, CSession.Home_Schema_RefInstanceId)
		CSession.Home_Schema_RefInstanceId = CDropdown.GetInt(ddRefInstance)

		'Fix Schema
		DiffInstance(i, fixId)

		cellBin.Visible = True

		'Row
		Dim trRef As TableRow = Nothing
		AddRow(tbl, RefInstance, trRef)
		AddRow(tbl, i, trRef)
	End Sub

	Public Sub Display_Deploys(app As CApp, diffId As Integer, fixId As EFix)
		'Dropdown
		ddRefInstance.DataSource = app.Instances
		ddRefInstance.DataBind()
		CDropdown.SetValue(ddRefInstance, CSession.Home_Schema_RefInstanceId)
		CSession.Home_Schema_RefInstanceId = CDropdown.GetInt(ddRefInstance)

		Dim c As HttpContext = HttpContext.Current
		Parallel.ForEach(app.Instances, Sub(i As CInstance)
											HttpContext.Current = c
											Dim temp = i.SchemaInfo
										End Sub)

		cellBin.Visible = True

		'Ins for App, by Client
		Dim trRef As TableRow = Nothing
		For Each i As CClient In CClient.Cache
			For Each j As CInstance In i.Instances

				If j.InstanceId = diffId Then
					DiffInstance(j, fixId)
				Else
					DiffInstance(j, EFix.None)
				End If

				'Row
				AddRow(tbl, j, trRef)
			Next
		Next
	End Sub

	Private Sub AddRow(tbl As Table, i As CInstance, ByRef trRef As TableRow)
		Dim tr As TableRow = Row(tbl)
		If IsNothing(trRef) Then trRef = tr

		'Instance
		CellH(tr, (tbl.Rows.Count - 1) & ".")
		Dim td As TableCell = CellLink(tr, CUtilities.Truncate(i.NameAndSuffix), CSitemap.InstanceMonitor(i.InstanceId))
		td.ToolTip = i.InstanceDbConnectionString
		td.Wrap = False



		Dim info As CSchemaInfo = i.SchemaInfo
		Dim ref As CSchemaInfo = RefSchema
		Dim infoErr As String = i.ErrorMessage_Schema
		Dim refErr As String = RefInstance.ErrorMessage_Schema

		AddRow(tr, i.InstanceId, i.InstanceId = RefInstanceId, info, ref, infoErr, refErr, trRef, i.Database)
	End Sub
#End Region

#Region "Common - AddRow"
	Public Function LinkToFix(fix As EFix, instanceId As Integer) As String
		If m_admin Then
			Return CSitemap.SelfSchemaSync_Diff(instanceId, CInt(fix))
		Else
			Return CSitemap.AppSchema_Diff(instanceId, CInt(fix))
		End If
	End Function


	Private Sub AddRow(tr As TableRow, instanceId As Integer, isRef As Boolean, info As CSchemaInfo, refInfo As CSchemaInfo, errInfo As String, errRefInfo As String, ByRef trRef As TableRow, db As CDataSrc)
		Dim N As Integer = 11

		If IsNothing(info) Then
			Dim tdErr As TableCell = Cell(tr, errInfo)
			tdErr.ColumnSpan = N
			tdErr.Font.Size = New FontUnit(FontSize.Smaller)
			tdErr.ForeColor = Drawing.Color.Red
			Exit Sub
		End If

		If IsNothing(refInfo) Then refInfo = info
		'If IsNothing(refInfo) Then
		'    Cell(tr, errRefInfo).ColumnSpan = 9
		'    Exit Sub
		'End If

		If isRef Then tr.Style.Add("background-color", "‎#FFFFE0")



		Dim ins As CInstance = CInstance.Cache.GetById(instanceId)
		Dim last As CReportHistory = Nothing
		If Not IsNothing(ins) Then last = ins.LastReport_
		Dim ver As CVersion = Nothing
		If Not IsNothing(last) Then ver = last.InitialVersion

		If Not IsNothing(ver) Then
			Cell(tr, ver.VersionFilesB64, ver.NameAndFileCount)
		ElseIf instanceId > 0 Then
			Cell(tr)
		End If

		Dim tdH As TableCell = Cell(tr, info.MD5_, info.MD5.ToString(), True)

		If info.MD5_ <> refInfo.MD5_ Then tdH.ForeColor = Drawing.Color.Red

		Dim diff As CSchemaDiff = info.Diff(refInfo)
		'Tables
		Try
			Dim tables As CTableInfoList = info.Tables
			Dim txt As String = CUtilities.CountSummary(tables, "table", "none")
			Dim ttip As String = CBinary.ToBase64(info.Tables.MD5, 10) 'CUtilities.ListToString(tables.NamesAndHashes, vbCrLf)
			Dim lnk As String = LinkToFix(EFix.Tables, instanceId)
			Dim msg As String = "Fix Tables/Columns Now?"

			If isRef Then
				Cell(tr, txt, ttip, True)
			Else
				ttip = diff.TableDiff.ToString()
				Dim td As TableCell = CellLink(tr, txt, lnk, False, False, ttip, msg)
				Dim hl As HyperLink = td.Controls(0)
				If Not diff.TableDiff.IsExactMatch Then hl.ForeColor = Drawing.Color.Red
			End If

		Catch ex As Exception
			Cell(tr, ex.Message).ColumnSpan = N - 2
			Exit Sub
		End Try

		'Views
		Try
			Dim views As CViewInfoList = info.Views
			Dim txt As String = CUtilities.CountSummary(views, "view", "none")
			Dim ttip As String = CUtilities.ListToString(views.NamesAndHashes, vbCrLf)
			Dim lnk As String = LinkToFix(EFix.Views, instanceId)
			Dim msg As String = "Fix Views Now?"


			If isRef Then
				Cell(tr, txt, ttip, True)
			Else
				ttip = diff.ViewDiff.ToString()
				Dim td As TableCell = CellLink(tr, txt, lnk, False, False, ttip, msg)
				Dim hl As HyperLink = td.Controls(0)
				If Not diff.ViewDiff.IsExactMatch Then hl.ForeColor = Drawing.Color.Red
			End If
		Catch ex As Exception
			Cell(tr, ex.Message).ColumnSpan = N - 3
			Exit Sub
		End Try

		'Columns
		Try
			Dim cols As New CColumnList
			cols.AddRange(info.Tables.AllColumns)
			cols.AddRange(info.Views.AllColumns)
			Dim txt As String = CUtilities.CountSummary(cols, "col", "none")
			Dim ttip As String = CUtilities.ListToString(cols.NamesAndTypes, vbCrLf)
			Dim lnk As String = LinkToFix(EFix.Tables, instanceId)
			Dim msg As String = "Fix Tables/Columns Now?"



			If isRef Then
				Cell(tr, txt, ttip, True)
			Else
				Dim colChanges As New List(Of String)
				For Each i As CTableDiff In diff.TableDiff.Different
					If Not i.ColumnsAreDifferent Then Continue For
					colChanges.Add(i.This.TableName & ": " & i.Columns.ToString)
				Next
				For Each i As CViewInfo In diff.ViewDiff.Different
					colChanges.Add(i.ViewName & ": " & CBinary.ToBase64(i.MD5, 10))
				Next

				ttip = CUtilities.ListToString(colChanges, vbCrLf)

				Dim td As TableCell = CellLink(tr, txt, lnk, False, False, ttip, msg)
				Dim hl As HyperLink = td.Controls(0)
				If colChanges.Count > 0 Then hl.ForeColor = Drawing.Color.Red
			End If
		Catch ex As Exception
			Cell(tr, ex.Message).ColumnSpan = N - 4
			Exit Sub
		End Try

		'stored Procs
		Try
			Dim procs As CProcedureList = info.Procs
			'Dim procsAndFns As List(Of String) = procs.NamesAndHashes 'db.StoredProcAndFunctionHashes
			Dim txt As String = CUtilities.CountSummary(procs, "proc", "none")
			Dim ttip As String = CUtilities.ListToString(procs.NamesAndHashes, vbCrLf)
			Dim lnk As String = LinkToFix(EFix.StoredProcs, instanceId)
			Dim msg As String = "Fix Stored-Procs/fns Now?"



			If isRef Then
				Cell(tr, txt, ttip, True)
			Else
				ttip = diff.ProcDiff.ToString
				Dim td As TableCell = CellLink(tr, txt, lnk, False, False, ttip, msg)
				Dim hl As HyperLink = td.Controls(0)
				If Not diff.ProcDiff.IsExactMatch Then hl.ForeColor = Drawing.Color.Red
			End If
		Catch ex As Exception
			Cell(tr, ex.Message).ColumnSpan = N - 5
			Exit Sub
		End Try

		'Indexes
		Try
			'Dim indexes As List(Of String) = info.IXB ' db.Indexes_()
			Dim indexes As CIndexInfoList = info.Indexes
			Dim txt As String = CUtilities.CountSummary(indexes, "index", "none", "indexes")
			Dim ttip As String = CUtilities.ListToString(indexes, vbCrLf)
			Dim lnk As String = LinkToFix(EFix.Indexes, instanceId)
			Dim msg As String = "Fix Indexes Now?"
			If isRef Then
				Cell(tr, txt, ttip, True)
			Else
				ttip = diff.IndexDiff.ToString
				Dim td As TableCell = CellLink(tr, txt, lnk, False, False, ttip, msg)
				Dim hl As HyperLink = td.Controls(0)
				If Not diff.IndexDiff.IsExactMatch Then hl.ForeColor = Drawing.Color.Red
			End If
		Catch ex As Exception
			Cell(tr, ex.Message).ColumnSpan = N - 6
			Exit Sub
		End Try

		'Foreign Keys
		Try
			Dim fks As CForeignKeyList = info.ForeignKeys ' db.ForeignKeys__()
			Dim txt As String = CUtilities.CountSummary(fks, "FK", "none")
			Dim ttip As String = CUtilities.ListToString(fks, vbCrLf)
			Dim lnk As String = LinkToFix(EFix.ForeignKeys, instanceId)
			Dim msg As String = "Fix Foreign Keys Now?"
			If isRef Then
				Cell(tr, txt, ttip, True)
			Else
				ttip = diff.FKDiff.ToString
				Dim td As TableCell = CellLink(tr, txt, lnk, False, False, ttip, msg)
				Dim hl As HyperLink = td.Controls(0)
				If Not diff.FKDiff.IsExactMatch Then hl.ForeColor = Drawing.Color.Red
			End If
		Catch ex As Exception
			Cell(tr, ex.Message).ColumnSpan = N - 7
			Exit Sub
		End Try

		'Default Vals
		Try
			Dim defs As CDefaultValueList = info.DefaultValues
			Dim txt As String = CUtilities.CountSummary(defs, "Def", "none")
			Dim ttip As String = CUtilities.ListToString(defs, vbCrLf)
			Dim lnk As String = LinkToFix(EFix.DefaultVals, instanceId)
			Dim msg As String = "Fix Default Vals Now?"
			If isRef Then
				Cell(tr, txt, ttip, True)
			Else
				ttip = diff.DefaultDiff.ToString
				Dim td As TableCell = CellLink(tr, txt, lnk, False, False, ttip, msg)
				Dim hl As HyperLink = td.Controls(0)
				If Not diff.DefaultDiff.IsCloseEnough Then hl.ForeColor = Drawing.Color.Red
			End If
		Catch ex As Exception
			Cell(tr, ex.Message).ColumnSpan = N - 7
			Exit Sub
		End Try

		'Migration
		Try
			Dim url As String = LinkToFix(EFix.Migration, instanceId)
			Dim msg As String = "Fix Migration Records Now?"

			Dim m As CMigration = info.Migration
			Dim r As CMigration = refInfo.Migration
			Dim mTxt As String = CBinary.ToBase64(m.ModelMd5, 6).ToUpper ' CUtilities.Truncate(m.MigrationId, 10)
			Dim mTip As String = m.MigrationId ' String.Concat(m.MigrationId, vbTab, CBinary.ToBase64(m.ModelMd5))
			mTxt = String.Concat(m.RowNumber, ".", mTxt)
			If String.IsNullOrEmpty(m.MigrationId) Then mTxt = "N/A"

			If isRef Then
				Cell(tr, mTxt, mTip, True)
			ElseIf m.ModelMd5 = r.ModelMd5 Then
				Compare(CellLink(tr, mTxt, url, False, False, mTip, msg), trRef)
			Else
				Dim mRef As CMigrationHistory = refInfo.MigrationHistory
				If IsNothing(mRef) AndAlso mRef.Count > 0 Then
					mRef = db.MigrationHistory
					refInfo.MigrationHistory = mRef
				End If

				Dim mDif As CMigrationHistory = mRef.GetChanges(m)

				If mDif.Count = 0 Then
					Dim mThis As CMigrationHistory = info.MigrationHistory
					If IsNothing(mThis) AndAlso mThis.Count > 0 Then
						mThis = db.MigrationHistory
						info.MigrationHistory = mThis
					End If

					mDif = mThis.GetChanges(r)
					For Each j As CMigration In mDif
						j.RowNumber = -j.RowNumber
					Next
				End If

				mTip = mDif.ToString

				Dim td As TableCell = CellLink(tr, mTxt, url, False, False, mTip, msg)
				Dim lnk As HyperLink = td.Controls(0)
				lnk.ForeColor = Drawing.Color.Red
			End If
		Catch ex As Exception
			Dim te As TableCell = Cell(tr, "ERROR", ex.Message)
			te.ForeColor = Drawing.Color.Red
			te.ColumnSpan = 3
			Exit Sub
		End Try


		CellLink(tr, "view", CSitemap.SchemaForInstance(instanceId))
		If isRef Then
			Cell(tr, "*", "reference", True)
			Cell(tr, "*", "reference", True)
		Else
			CellLink(tr, "script", LinkToFix(EFix.ScriptOnly, instanceId))
			CellLink(tr, "fix-all", LinkToFix(EFix.All, instanceId))
		End If

		For Each i As TableCell In tr.Cells
			i.Wrap = False
		Next
	End Sub

#End Region


#Region "Shared - Diff/Fix Admin/Instance"
	Private Function DiffInstance(ins As CInstance, Optional fixId As EFix = EFix.None) As CSchemaDiff
		If fixId = EFix.None Then Return Nothing 'No diff yet


		Dim r As CInstance = RefInstance

		Dim db As CDataSrc = ins.DatabaseDirectOrWeb
		Dim refDb As CDataSrc = r.DatabaseDirectOrWeb

		Dim ref As CSchemaInfo = RefSchema
		Dim sch As CSchemaInfo = ins.SchemaInfo

		If IsNothing(ref) OrElse IsNothing(sch) Then Return Nothing

		Dim d As CSchemaDiff = sch.Diff(RefSchema)
		FixInstance(d, ins.IdAndName, db, sch, ref, refDb, fixId)

		If fixId <> EFix.None AndAlso fixId <> EFix.ScriptOnly Then
			ins.SchemaInfo = Nothing
			Response.Redirect(CSitemap.InstanceSchema(ins.InstanceId))
		End If

		Return d
	End Function


	Private Function DiffAdmin(source As ESource, Optional fixId As EFix = EFix.None) As CSchemaDiff
		'Local
		Dim db As CDataSrc = LOCAL_DB
		Dim sch As CSchemaInfo = Local
		Dim name As String = "Prod => Local"

		'Prod 
		Dim target As ESource = ESource.Prod
		Dim refDb As CDataSrc = PROD_DB
		Dim ref As CSchemaInfo = PROD

		'Switch (prod/local)
		If source = ESource.Prod Then
			db = PROD_DB
			sch = PROD
			name = "Local => Prod"

			target = ESource.Local
			refDb = LOCAL_DB
			ref = Local
		End If

		'Action
		Dim d As CSchemaDiff = sch.Diff(ref)
		FixInstance(d, name, db, sch, ref, refDb, fixId)

		If fixId <> EFix.None AndAlso fixId <> EFix.ScriptOnly Then
			_PROD = Nothing
			_LOCAL = Nothing
			'Response.Redirect(CSitemap.SelfSchemaSync())
		End If

		Return d
	End Function
#End Region


#Region "Fixing Logic"

	Private Sub FixInstance(diff As CSchemaDiff, name As String, db As CDataSrc, info As CSchemaInfo, ref As CSchemaInfo, refDb As CDataSrc, fixId As EFix)
		'View-Only
		If fixId = EFix.None Then Exit Sub

		'Prepare sql/cmd
		Dim sql As New List(Of String)
		FixInstance_Defaults(diff, fixId, sql, False)
		FixInstance_Tables(diff, fixId, sql, False)
		FixInstance_Indexes(diff, fixId, sql)
		FixInstance_FKs(diff, fixId, sql)
		FixInstance_Tables(diff, fixId, sql, True)
		FixInstance_Defaults(diff, fixId, sql, True)
		FixInstance_Views(diff, fixId, sql)
		FixInstance_Procs(diff, fixId, sql)

		Dim cmd As New List(Of CCommand)
		FixInstance_Migration(fixId, cmd, info, ref, db, refDb)


		'Script only
		Dim sb As New StringBuilder()
		sb.AppendLine(name)
		If fixId = EFix.ScriptOnly Then
			For Each i As String In sql
				sb.AppendLine(i)
			Next
			For Each i As CCommand In cmd
				sb.Append(i.Text).Append(vbTab)
				For Each j In i.ParametersNamed
					sb.Append(j.Name).Append("=").Append(j.Value).Append(", ")
				Next
				sb.AppendLine()
			Next

			CSession.PageMessage = sb.ToString
			Exit Sub
		End If


		'Execute; Record results
		For Each i As String In sql
			If String.IsNullOrEmpty(i) Then
				sb.AppendLine()
				Continue For
			End If

			Try
				db.ExecuteNonQuery(i)
				sb.Append("RAN OK: ").AppendLine(i)
			Catch ex As Exception
				sb.AppendLine()
				sb.Append("FAILED: ").AppendLine(ex.Message)
				sb.AppendLine()
			End Try
		Next


		'Execute; Record results
		For Each i As CCommand In cmd
			Try
				db.ExecuteNonQuery(i)
				Dim p As CNameValue = i.ParametersNamed(0)
				sb.Append("RAN OK: ").Append(p.Value).Append(" ").AppendLine(i.Text)
			Catch ex As Exception
				sb.AppendLine()
				sb.Append("FAILED: ").AppendLine(ex.Message)
				sb.AppendLine()
			End Try
		Next

		_LOCAL = Nothing
		_PROD = Nothing

		CSession.PageMessage = sb.ToString()
	End Sub

	Private Shared Sub FixInstance_Indexes(diff As CSchemaDiff, f As EFix, sqlList As List(Of String))
		If f <> EFix.Indexes AndAlso f <> EFix.All AndAlso f <> EFix.ScriptOnly Then Exit Sub
		sqlList.AddRange(diff.IndexDiff.ChangeScripts)
	End Sub
	Private Shared Sub FixInstance_Procs(diff As CSchemaDiff, f As EFix, sqlList As List(Of String))
		If f <> EFix.StoredProcs AndAlso f <> EFix.All AndAlso f <> EFix.ScriptOnly Then Exit Sub
		sqlList.AddRange(diff.ProcDiff.ChangeScripts)
	End Sub
	Private Shared Sub FixInstance_Tables(diff As CSchemaDiff, f As EFix, sqlList As List(Of String), isNew As Boolean)
		If f <> EFix.Tables AndAlso f <> EFix.Cols AndAlso f <> EFix.All AndAlso f <> EFix.ScriptOnly Then Exit Sub
		sqlList.AddRange(diff.TableDiff.ChangeScripts(isNew))
	End Sub
	Private Shared Sub FixInstance_Defaults(diff As CSchemaDiff, f As EFix, sqlList As List(Of String), create As Boolean)
		If f <> EFix.Tables AndAlso f <> EFix.Cols AndAlso f <> EFix.All AndAlso f <> EFix.ScriptOnly Then Exit Sub
		sqlList.AddRange(diff.DefaultDiff.ChangeScripts(Not create))
	End Sub

	Private Shared Sub FixInstance_Views(diff As CSchemaDiff, f As EFix, sqlList As List(Of String))
		If f <> EFix.Views AndAlso f <> EFix.Cols AndAlso f <> EFix.All AndAlso f <> EFix.ScriptOnly Then Exit Sub
		sqlList.AddRange(diff.ViewDiff.ChangeScripts)
	End Sub
	Private Shared Sub FixInstance_FKs(diff As CSchemaDiff, f As EFix, sqlList As List(Of String))
		If f <> EFix.ForeignKeys AndAlso f <> EFix.All AndAlso f <> EFix.ScriptOnly Then Exit Sub
		sqlList.AddRange(diff.FKDiff.ChangeScripts)
	End Sub
	Private Shared Sub FixInstance_Migration(f As EFix, cmd As List(Of CCommand), i As CSchemaInfo, ref As CSchemaInfo, db As CDataSrc, refDb As CDataSrc)
		If f <> EFix.Migration AndAlso f <> EFix.All AndAlso f <> EFix.ScriptOnly Then Exit Sub 'AndAlso f <> EFix.All
		If ref.Migration.MigrationId = i.Migration.MigrationId Then Exit Sub
		cmd.AddRange(i.ForceMigrationCommands(ref, db, refDb))
	End Sub

#End Region

#Region "Tooltip Compare"
	Private Shared Sub Compare(td As TableCell, ref As TableRow)
		Dim index As Integer = CType(td.Parent, TableRow).Cells.GetCellIndex(td)
		If index >= ref.Cells.Count Then Exit Sub
		Dim tdRef As TableCell = ref.Cells(index)

		If tdRef.ClientID = td.ClientID Then Exit Sub

		Dim txt As String = td.Text
		Dim tip As String = td.ToolTip
		Dim lnk1 As HyperLink = Nothing
		If td.Controls.Count > 0 AndAlso TypeOf td.Controls(0) Is HyperLink Then
			lnk1 = td.Controls(0)
			txt = lnk1.Text
			tip = lnk1.ToolTip
		End If

		Dim txtR As String = tdRef.Text
		Dim tipR As String = tdRef.ToolTip
		If tdRef.Controls.Count > 0 AndAlso TypeOf tdRef.Controls(0) Is HyperLink Then
			Dim lnk As HyperLink = tdRef.Controls(0)
			txtR = lnk.Text
			tipR = lnk.ToolTip
		End If

		If txt <> txtR Then
			If IsNothing(lnk1) Then td.ForeColor = Drawing.Color.Red Else lnk1.ForeColor = Drawing.Color.Red
		End If
		If tip <> tipR Then
			If IsNothing(lnk1) Then td.ForeColor = Drawing.Color.Red Else lnk1.ForeColor = Drawing.Color.Red

			Dim left As List(Of String) = CUtilities.SplitOn(tip, vbCrLf)
			Dim right As List(Of String) = CUtilities.SplitOn(tipR, vbCrLf)

			Dim leftOnly As New List(Of String)
			For Each i As String In left
				If Not right.Contains(i) Then leftOnly.Add(i)
			Next
			Dim rightOnly As New List(Of String)
			Dim both As New List(Of String)
			For Each i As String In right
				If Not left.Contains(i) Then rightOnly.Add(i) Else both.Add(i)
			Next

			Dim sb As New StringBuilder()
			If leftOnly.Count > 0 OrElse rightOnly.Count > 0 Then
				If leftOnly.Count > 0 Then sb.AppendLine(CUtilities.NameAndCount("New", leftOnly)).AppendLine(CUtilities.ListToString(leftOnly, vbCrLf)).AppendLine()
				If rightOnly.Count > 0 Then sb.AppendLine(CUtilities.NameAndCount("Missing", rightOnly)).AppendLine(CUtilities.ListToString(rightOnly, vbCrLf)).AppendLine()
			Else
				If both.Count > 0 Then sb.Append(CUtilities.NameAndCount(" Same", both))
			End If

			If leftOnly.Count = 0 AndAlso rightOnly.Count = 0 Then
				lnk1.ForeColor = Drawing.Color.Blue
			Else
				If IsNothing(lnk1) Then
					td.ToolTip = sb.ToString
				Else
					lnk1.ToolTip = sb.ToString
				End If
			End If
		End If
	End Sub
#End Region

End Class
