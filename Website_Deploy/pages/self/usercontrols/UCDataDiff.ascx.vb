
Imports System.Data
Imports System.Threading.Tasks
Imports SchemaDeploy

Partial Class pages_self_usercontrols_UCDataDiff
	Inherits CUserControlWithTableHelpers

	Public Enum EPKType
		Int32
		BigInt
		Guid
		Str
		IntInt
	End Enum



	Private m_admin As Boolean = False
	Public Property Admin As Boolean
		Get
			Return m_admin
		End Get
		Set(value As Boolean)
			m_admin = value
		End Set
	End Property


	Private m_chk As New List(Of CheckBox)
	Private m_source As CDataSrc
	Private m_target As CDataSrc

	Private m_type As EPKType
	Private m_diff As Object
	Private m_tbl As CTableInfo


#Region "Interface - Instance"
	Public Sub Display(ins As CInstance)
		CDropdown.SetValue(ddTarget, ins.InstanceId)

		Dim db As CDataSrc = ins.Database
		If IsNothing(db) Then db = ins.DatabaseViaWeb
		If IsNothing(db) Then Exit Sub

		Dim refDb As CDataSrc = RefInstance.Database
		If IsNothing(refDb) Then refDb = RefInstance.Database

		If ddTable.SelectedValue = "*" Then
			DisplayCounts(RefInstanceId, ins.InstanceId, refDb, db)
		Else
			DisplayDiff(RefInstanceId, ins.InstanceId, refDb, db)
		End If
	End Sub
	Public Sub Display()
		If Not CConfig.IsDev Then Exit Sub

		DisplayDiff()
	End Sub
#End Region

#Region "Data"
	Private ReadOnly Property RefInstanceId As Integer
		Get
			Return CSession.Home_Schema_RefInstanceId
		End Get
	End Property
	Private ReadOnly Property RefInstance As CInstance
		Get
			Return CInstance.Cache.GetById(RefInstanceId)
		End Get
	End Property
#End Region

#Region "Page Events"
	Private Sub pages_self_usercontrols_UCDataDiff_Init(sender As Object, e As EventArgs) Handles Me.Init
		If m_admin Then

			If CConfig.IsDev Then
				CDropdown.AddEnums(ddSource, GetType(ESource))
				CDropdown.SetValue(ddSource, ESource.Prod)

				CDropdown.AddEnums(ddTarget, GetType(ESource))
				CDropdown.SetValue(ddTarget, ESource.Local)

				ddTable.DataSource = CDataSrc.Default.AllTableNames
				ddTable.DataBind()
				CDropdown.BlankItem(ddTable, "* (all tables)", "*")
				CDropdown.SetValue(ddTable, CSession.Home_Data_TableName)
			End If
		Else
			ddSource.DataSource = CInstance.Cache
			ddSource.DataBind()

			ddTarget.DataSource = CInstance.Cache
			ddTarget.DataBind()

			CDropdown.SetValue(ddSource, RefInstanceId)

			If IsNothing(RefInstance) Then CSession.Home_Schema_RefInstanceId = CInstance.Cache(0).InstanceId

			ddTable.DataSource = RefInstance.SchemaInfo.Tables.Names
			ddTable.DataBind()
			CDropdown.BlankItem(ddTable, "* (all tables)", "*")
			CDropdown.SetValue(ddTable, CSession.Home_Data_TableName)
		End If
	End Sub

#End Region

#Region "Form Events"
	Private Sub btnFixData_Click(sender As Object, e As EventArgs) Handles btnFixData.Click
		Select Case m_type
			Case EPKType.BigInt
				Dim d As CDataDiff_PkLong = m_diff
				d.ResolveByHash()

			Case EPKType.Guid
				Dim d As CDataDiff_PkGuid = m_diff

				If d.TableName = CBinaryFile.VIEW_NAME Then
					For Each i As Guid In d.DiffOnRowHash.SourceOnly
						Dim dr As DataRow = d.SourcePkIndex(i)
						dr("BinGz") = d.SourceDb.ExecuteScalar(New CCommand("SELECT BinGz FROM " & CBinaryFile.TABLE_NAME & " WHERE MD5=@md5", New CNameValueList("md5", i), False))
					Next
					d.TableName = CBinaryFile.TABLE_NAME
				End If


				d.ResolveByHash()

			Case EPKType.Int32
				Dim d As CDataDiff_PkInt = m_diff
				If d.TableName.Contains(CBackupItem.VIEW_NAME) Then
					For Each i As Integer In d.DiffOnPk.SourceOnly
						Dim dr As DataRow = d.SourcePkIndex(i)
						Dim id As New CNameValueList("id", i)
						dr("ItemDatasetXmlGz") = d.SourceDb.ExecuteScalar(New CCommand("SELECT ItemDatasetXmlGz FROM " & CBackupItem.TABLE_NAME & " WHERE ItemId=@id", id, False))
						dr("ItemSchemaXmlGz") = d.SourceDb.ExecuteScalar(New CCommand("SELECT ItemSchemaXmlGz FROM " & CBackupItem.TABLE_NAME & " WHERE ItemId=@id", id, False))
					Next
					d.TableName = CBackupItem.TABLE_NAME
				End If
				d.ResolveByHash()

			Case EPKType.Str
				Dim d As CDataDiff_PkString = m_diff
				d.ResolveByHash()

			Case EPKType.IntInt
				Dim d As CDataDiff_PkIntInt = m_diff
				d.ResolveByHash()
		End Select

		Response.Redirect(Request.RawUrl)
	End Sub
	Private Sub btnFixIds_Click(sender As Object, e As EventArgs) Handles btnFixIds.Click
		Select Case m_type
			Case EPKType.BigInt
				Dim d As CDataDiff_PkLong = m_diff
				d.Resolve(m_tbl.PrimaryKey.IsIdentity)

			Case EPKType.Guid
				Dim d As CDataDiff_PkGuid = m_diff

				If d.TableName = CBinaryFile.VIEW_NAME Then
					For Each i As Guid In d.DiffOnRowHash.SourceOnly
						Dim dr As DataRow = d.SourcePkIndex(i)
						dr("BinGz") = d.SourceDb.ExecuteScalar(New CCommand("SELECT BinGz FROM " & CBinaryFile.TABLE_NAME & " WHERE MD5=@md5", New CNameValueList("md5", i), False))
					Next
					d.TableName = CBinaryFile.TABLE_NAME
				End If


				d.Resolve()

			Case EPKType.Int32
				Dim d As CDataDiff_PkInt = m_diff


				If d.TableName.Contains(CBackupItem.VIEW_NAME) Then
					For Each i As Integer In d.DiffOnPk.SourceOnly
						Dim dr As DataRow = d.SourcePkIndex(i)
						Dim id As New CNameValueList("id", i)
						dr("ItemDatasetXmlGz") = d.SourceDb.ExecuteScalar(New CCommand("SELECT ItemDatasetXmlGz FROM " & CBackupItem.TABLE_NAME & " WHERE ItemId=@id", id, False))
						dr("ItemSchemaXmlGz") = d.SourceDb.ExecuteScalar(New CCommand("SELECT ItemSchemaXmlGz FROM " & CBackupItem.TABLE_NAME & " WHERE ItemId=@id", id, False))
					Next
					d.TableName = CBackupItem.TABLE_NAME
				End If

				d.Resolve(m_tbl.PrimaryKey.IsIdentity)

			Case EPKType.Str
				Dim d As CDataDiff_PkString = m_diff
				d.Resolve()

			Case EPKType.IntInt
				Dim d As CDataDiff_PkIntInt = m_diff
				d.Resolve()
		End Select

		Response.Redirect(Request.RawUrl)
	End Sub
#End Region

#Region "Display"
	Private Sub DisplayDiff()

		'Avail
		Dim local As CDataSrc = CDataSrc.Default
		Dim prod As New CWebSrcBinary("https://admin.controltrackonline.com")

		'Selected
		Dim sourceId As ESource = CDropdown.GetInt(ddSource)
		Dim targetId As ESource = CDropdown.GetInt(ddTarget)

		'Translate
		Dim source As CDataSrc = IIf(sourceId = ESource.Local, local, prod)
		Dim target As CDataSrc = IIf(targetId = ESource.Local, local, prod)

		If TypeOf source Is CDataSrcRemote Then
			Dim b As CWebSrcBinary = source
			CWebSrcBinary.TimeOutMs = 3600 * 1000
		End If

		If ddTable.SelectedValue = "*" Then
			DisplayCounts(sourceId, targetId, source, target)
		Else
			DisplayDiff(sourceId, targetId, source, target)
		End If
	End Sub
	Private Sub DisplayDiff(sourceId As Integer, targetId As Integer, source As CDataSrc, target As CDataSrc)

		'Selection
		Dim tableName As String = ddTable.SelectedValue
		CSession.Home_Data_TableName = tableName

		Dim local As CDataSrc = source
		If target.IsLocal Then local = target


		'Switch on PK type
		Dim schema As CSchemaInfo = local.SchemaInfo
		Dim tbl As CTableInfo = schema.Tables.GetByName(tableName)
		If IsNothing(tbl) Then tbl = New CTableInfoList(local, True).GetByName(tableName)
		If IsNothing(tbl) OrElse 0 = tbl.Columns.Count Then Exit Sub
		m_tbl = tbl

		Dim pks As List(Of String) = tbl.PrimaryKey.ColumnNames

		Dim type As String = tbl.Columns.Item(pks(0)).Type
		If type.StartsWith("NVARCHAR") Then type = "NVARCHAR"
		If type.StartsWith("VARCHAR") Then type = "VARCHAR"

		If pks.Count > 1 Then
			Dim type2 As String = tbl.Columns.Item(pks(1)).Type
			If type2.StartsWith("NVARCHAR") Then type = "NVARCHAR"
			If type2.StartsWith("VARCHAR") Then type = "VARCHAR"
			If "INT" = type AndAlso "INT" = type2 Then
				'Data-diff
				Dim d As New CDataDiff_PkIntInt(source, target, tableName, pks(0), pks(1))
				Dim pk As CDiff_String = d.DiffOnPk
				Dim md5 As CDiff_Guid = d.DiffOnRowHash

				m_type = EPKType.IntInt
				m_diff = d

				'Present
				If d.TargetTable.Rows.Count > 0 Then lblTargetTotal.Text = CTextbox.SetNumber(d.TargetTable.Rows.Count)
				If d.SourceTable.Rows.Count > 0 Then lblSourceTotal.Text = CTextbox.SetNumber(d.SourceTable.Rows.Count)
				If d.ExactMatchPks.Count > 0 Then lblExactMatch.Text = CTextbox.SetNumber(d.ExactMatchPks.Count)
				If d.ExactMatchPks.Count <> d.SourceTable.Rows.Count Then lblExactMatch.ForeColor = Drawing.Color.Red

				If pk.SourceOnly.Count > 0 Then lblSourceOnlyById.Text = CTextbox.SetNumber(pk.SourceOnly.Count)
				If pk.TargetOnly.Count > 0 Then lblTargetOnlyById.Text = CTextbox.SetNumber(pk.TargetOnly.Count)
				If pk.Matching.Count > 0 Then lblMatchingById.Text = CTextbox.SetNumber(pk.Matching.Count)

				If md5.SourceOnly.Count > 0 Then lblSourceOnlyByMd5.Text = CTextbox.SetNumber(md5.SourceOnly.Count)
				If md5.TargetOnly.Count > 0 Then lblTargetOnlyByMd5.Text = CTextbox.SetNumber(md5.TargetOnly.Count)
				If md5.Matching.Count > 0 Then lblMatchingByMd5.Text = CTextbox.SetNumber(md5.Matching.Count)

				If md5.SourceOnly.Count + md5.TargetOnly.Count > 0 Then lblMatchingByMd5.ForeColor = Drawing.Color.Red


				dgSourceOnlyByHash.DataSource = d.SourceOnlyRows_ByMD5
				dgSourceOnlyByHash.DataBind()
				dgSourceOnlyByHash.Visible = dgSourceOnlyByHash.Items.Count > 0
				pnlSourceOnlyByMd5.Visible = dgSourceOnlyByHash.Visible
				lblSourceOnlyByMD5Count.Text = CUtilities.CountSummary(d.SourceOnlyRows_ByMD5.Rows.Count, "row")

				dgSourceOnlyById.DataSource = d.SourceOnlyRows_ByPk
				dgSourceOnlyById.DataBind()
				dgSourceOnlyById.Visible = dgSourceOnlyById.Items.Count > 0
				pnlSourceOnlyByPK.Visible = dgSourceOnlyById.Visible
				lblSourceOnlyByPkCount.Text = CUtilities.CountSummary(d.SourceOnlyRows_ByPk.Rows.Count, "row")


				dgTargetOnlyByHash.DataSource = d.TargetOnlyRows_ByMD5
				dgTargetOnlyByHash.DataBind()
				dgTargetOnlyByHash.Visible = dgTargetOnlyByHash.Items.Count > 0
				pnlTargetOnlyByMd5.Visible = dgTargetOnlyByHash.Visible
				lblTargetOnlyByMd5Count.Text = CUtilities.CountSummary(d.TargetOnlyRows_ByMD5.Rows.Count, "row")

				dgTargetOnlyById.DataSource = d.TargetOnlyRows_ByPk
				dgTargetOnlyById.DataBind()
				dgTargetOnlyById.Visible = dgTargetOnlyById.Items.Count > 0
				pnlTargetOnlyByPK.Visible = dgTargetOnlyById.Visible
				lblTargetOnlyByPkCount.Text = CUtilities.CountSummary(d.TargetOnlyRows_ByPk.Rows.Count, "row")

				dgBoth.DataSource = d.MatchingRows_ByMD5
				dgBoth.DataBind()


				pnlSourceOnly.Visible = dgSourceOnlyById.Items.Count + dgSourceOnlyByHash.Items.Count > 0
				pnlTargetOnly.Visible = dgTargetOnlyById.Items.Count + dgTargetOnlyByHash.Items.Count > 0
				pnlBoth.Visible = dgBoth.Items.Count > 0

			Else
				pnlSourceOnly.Visible = False
				pnlTargetOnly.Visible = False
				pnlBoth.Visible = False
				plh.Visible = False
			End If
			Exit Sub
		End If

		Select Case type
			Case = "INT"
				If tableName = CBackupItem.TABLE_NAME Then tableName = CBackupItem.VIEW_NAME

				'Data-diff
				Dim d As New CDataDiff_PkInt(source, target, tableName)
				Dim pk As CDiff_Int = d.DiffOnPk
				Dim md5 As CDiff_Guid = d.DiffOnRowHash

				m_type = EPKType.Int32
				m_diff = d

				'Present
				If d.TargetTable.Rows.Count > 0 Then lblTargetTotal.Text = CTextbox.SetNumber(d.TargetTable.Rows.Count)
				If d.SourceTable.Rows.Count > 0 Then lblSourceTotal.Text = CTextbox.SetNumber(d.SourceTable.Rows.Count)
				If d.ExactMatchPks.Count > 0 Then lblExactMatch.Text = CTextbox.SetNumber(d.ExactMatchPks.Count)
				If d.ExactMatchPks.Count <> d.SourceTable.Rows.Count Then lblExactMatch.ForeColor = Drawing.Color.Red

				If pk.SourceOnly.Count > 0 Then lblSourceOnlyById.Text = CTextbox.SetNumber(pk.SourceOnly.Count)
				If pk.TargetOnly.Count > 0 Then lblTargetOnlyById.Text = CTextbox.SetNumber(pk.TargetOnly.Count)
				If pk.Matching.Count > 0 Then lblMatchingById.Text = CTextbox.SetNumber(pk.Matching.Count)

				If md5.SourceOnly.Count > 0 Then lblSourceOnlyByMd5.Text = CTextbox.SetNumber(md5.SourceOnly.Count)
				If md5.TargetOnly.Count > 0 Then lblTargetOnlyByMd5.Text = CTextbox.SetNumber(md5.TargetOnly.Count)
				If md5.Matching.Count > 0 Then lblMatchingByMd5.Text = CTextbox.SetNumber(md5.Matching.Count)

				If md5.SourceOnly.Count + md5.TargetOnly.Count > 0 Then lblMatchingByMd5.ForeColor = Drawing.Color.Red


				dgSourceOnlyByHash.DataSource = d.SourceOnlyRows_ByMD5
				dgSourceOnlyByHash.DataBind()
				dgSourceOnlyByHash.Visible = dgSourceOnlyByHash.Items.Count > 0
				pnlSourceOnlyByMd5.Visible = dgSourceOnlyByHash.Visible
				lblSourceOnlyByMD5Count.Text = CUtilities.CountSummary(d.SourceOnlyRows_ByMD5.Rows.Count, "row")

				dgSourceOnlyById.DataSource = d.SourceOnlyRows_ByPk
				dgSourceOnlyById.DataBind()
				dgSourceOnlyById.Visible = dgSourceOnlyById.Items.Count > 0
				pnlSourceOnlyByPK.Visible = dgSourceOnlyById.Visible
				lblSourceOnlyByPkCount.Text = CUtilities.CountSummary(d.SourceOnlyRows_ByPk.Rows.Count, "row")


				dgTargetOnlyByHash.DataSource = d.TargetOnlyRows_ByMD5
				dgTargetOnlyByHash.DataBind()
				dgTargetOnlyByHash.Visible = dgTargetOnlyByHash.Items.Count > 0
				pnlTargetOnlyByMd5.Visible = dgTargetOnlyByHash.Visible
				lblTargetOnlyByMd5Count.Text = CUtilities.CountSummary(d.TargetOnlyRows_ByMD5.Rows.Count, "row")

				dgTargetOnlyById.DataSource = d.TargetOnlyRows_ByPk
				dgTargetOnlyById.DataBind()
				dgTargetOnlyById.Visible = dgTargetOnlyById.Items.Count > 0
				pnlTargetOnlyByPK.Visible = dgTargetOnlyById.Visible
				lblTargetOnlyByPkCount.Text = CUtilities.CountSummary(d.TargetOnlyRows_ByPk.Rows.Count, "row")

				dgBoth.DataSource = d.MatchingRows_ByMD5
				dgBoth.DataBind()


				pnlSourceOnly.Visible = dgSourceOnlyById.Items.Count + dgSourceOnlyByHash.Items.Count > 0
				pnlTargetOnly.Visible = dgTargetOnlyById.Items.Count + dgTargetOnlyByHash.Items.Count > 0
				pnlBoth.Visible = dgBoth.Items.Count > 0


			Case "UNIQUEIDENTIFIER"

				'Data-diff
				Dim viewName As String = tableName
				If viewName = CBinaryFile.TABLE_NAME Then viewName = CBinaryFile.VIEW_NAME
				Dim d As New CDataDiff_PkGuid(source, target, viewName)
				Dim pk As CDiff_Guid = d.DiffOnPk
				Dim md5 As CDiff_Guid = d.DiffOnRowHash

				m_type = EPKType.Guid
				m_diff = d

				'Present
				If d.TargetTable.Rows.Count > 0 Then lblTargetTotal.Text = CTextbox.SetNumber(d.TargetTable.Rows.Count)
				If d.SourceTable.Rows.Count > 0 Then lblSourceTotal.Text = CTextbox.SetNumber(d.SourceTable.Rows.Count)
				If d.ExactMatchPks.Count > 0 Then lblExactMatch.Text = CTextbox.SetNumber(d.ExactMatchPks.Count)

				If pk.SourceOnly.Count > 0 Then lblSourceOnlyById.Text = CTextbox.SetNumber(pk.SourceOnly.Count)
				If pk.TargetOnly.Count > 0 Then lblTargetOnlyById.Text = CTextbox.SetNumber(pk.TargetOnly.Count)
				If pk.Matching.Count > 0 Then lblMatchingById.Text = CTextbox.SetNumber(pk.Matching.Count)

				If md5.SourceOnly.Count > 0 Then lblSourceOnlyByMd5.Text = CTextbox.SetNumber(md5.SourceOnly.Count)
				If md5.TargetOnly.Count > 0 Then lblTargetOnlyByMd5.Text = CTextbox.SetNumber(md5.TargetOnly.Count)
				If md5.Matching.Count > 0 Then lblMatchingByMd5.Text = CTextbox.SetNumber(md5.Matching.Count)

				dgSourceOnlyByHash.DataSource = d.SourceOnlyRows_ByMD5
				dgSourceOnlyByHash.DataBind()
				dgSourceOnlyByHash.Visible = dgSourceOnlyByHash.Items.Count > 0
				pnlSourceOnlyByMd5.Visible = dgSourceOnlyByHash.Visible
				lblSourceOnlyByMD5Count.Text = CUtilities.CountSummary(d.SourceOnlyRows_ByMD5.Rows.Count, "row")

				dgSourceOnlyById.DataSource = d.SourceOnlyRows_ByPk
				dgSourceOnlyById.DataBind()
				dgSourceOnlyById.Visible = dgSourceOnlyById.Items.Count > 0
				pnlSourceOnlyByPK.Visible = dgSourceOnlyById.Visible
				lblSourceOnlyByPkCount.Text = CUtilities.CountSummary(d.SourceOnlyRows_ByPk.Rows.Count, "row")


				dgTargetOnlyByHash.DataSource = d.TargetOnlyRows_ByMD5
				dgTargetOnlyByHash.DataBind()
				dgTargetOnlyByHash.Visible = dgTargetOnlyByHash.Items.Count > 0
				pnlTargetOnlyByMd5.Visible = dgTargetOnlyByHash.Visible
				lblTargetOnlyByMd5Count.Text = CUtilities.CountSummary(d.TargetOnlyRows_ByMD5.Rows.Count, "row")

				dgTargetOnlyById.DataSource = d.TargetOnlyRows_ByPk
				dgTargetOnlyById.DataBind()
				dgTargetOnlyById.Visible = dgTargetOnlyById.Items.Count > 0
				pnlTargetOnlyByPK.Visible = dgTargetOnlyById.Visible
				lblTargetOnlyByPkCount.Text = CUtilities.CountSummary(d.TargetOnlyRows_ByPk.Rows.Count, "row")

				dgBoth.DataSource = d.MatchingRows_ByMD5
				dgBoth.DataBind()

				pnlSourceOnly.Visible = dgSourceOnlyById.Items.Count + dgSourceOnlyByHash.Items.Count > 0
				pnlTargetOnly.Visible = dgTargetOnlyById.Items.Count + dgTargetOnlyByHash.Items.Count > 0
				pnlBoth.Visible = dgBoth.Items.Count > 0



			Case "NVARCHAR"

				'Data-diff
				Dim d As New CDataDiff_PkString(source, target, tableName)
				Dim pk As CDiff_String = d.DiffOnPk
				Dim md5 As CDiff_Guid = d.DiffOnRowHash

				m_type = EPKType.Str
				m_diff = d

				'Present
				If d.TargetTable.Rows.Count > 0 Then lblTargetTotal.Text = CTextbox.SetNumber(d.TargetTable.Rows.Count)
				If d.SourceTable.Rows.Count > 0 Then lblSourceTotal.Text = CTextbox.SetNumber(d.SourceTable.Rows.Count)
				If d.ExactMatchPks.Count > 0 Then lblExactMatch.Text = CTextbox.SetNumber(d.ExactMatchPks.Count)

				If pk.SourceOnly.Count > 0 Then lblSourceOnlyById.Text = CTextbox.SetNumber(pk.SourceOnly.Count)
				If pk.TargetOnly.Count > 0 Then lblTargetOnlyById.Text = CTextbox.SetNumber(pk.TargetOnly.Count)
				If pk.Matching.Count > 0 Then lblMatchingById.Text = CTextbox.SetNumber(pk.Matching.Count)

				If md5.SourceOnly.Count > 0 Then lblSourceOnlyByMd5.Text = CTextbox.SetNumber(md5.SourceOnly.Count)
				If md5.TargetOnly.Count > 0 Then lblTargetOnlyByMd5.Text = CTextbox.SetNumber(md5.TargetOnly.Count)
				If md5.Matching.Count > 0 Then lblMatchingByMd5.Text = CTextbox.SetNumber(md5.Matching.Count)

				dgSourceOnlyByHash.DataSource = d.SourceOnlyRows_ByMD5
				dgSourceOnlyByHash.DataBind()
				dgSourceOnlyByHash.Visible = dgSourceOnlyByHash.Items.Count > 0
				pnlSourceOnlyByMd5.Visible = dgSourceOnlyByHash.Visible
				lblSourceOnlyByMD5Count.Text = CUtilities.CountSummary(d.SourceOnlyRows_ByMD5.Rows.Count, "row")

				dgSourceOnlyById.DataSource = d.SourceOnlyRows_ByPK
				dgSourceOnlyById.DataBind()
				dgSourceOnlyById.Visible = dgSourceOnlyById.Items.Count > 0
				pnlSourceOnlyByPK.Visible = dgSourceOnlyById.Visible
				lblSourceOnlyByPkCount.Text = CUtilities.CountSummary(d.SourceOnlyRows_ByPK.Rows.Count, "row")


				dgTargetOnlyByHash.DataSource = d.TargetOnlyRows_ByMD5
				dgTargetOnlyByHash.DataBind()
				dgTargetOnlyByHash.Visible = dgTargetOnlyByHash.Items.Count > 0
				pnlTargetOnlyByMd5.Visible = dgTargetOnlyByHash.Visible
				lblTargetOnlyByMd5Count.Text = CUtilities.CountSummary(d.TargetOnlyRows_ByMD5.Rows.Count, "row")

				dgTargetOnlyById.DataSource = d.TargetOnlyRows_ByPK
				dgTargetOnlyById.DataBind()
				dgTargetOnlyById.Visible = dgTargetOnlyById.Items.Count > 0
				pnlTargetOnlyByPK.Visible = dgTargetOnlyById.Visible
				lblTargetOnlyByPkCount.Text = CUtilities.CountSummary(d.TargetOnlyRows_ByPK.Rows.Count, "row")

				dgBoth.DataSource = d.MatchingRows_ByMD5
				dgBoth.DataBind()

				pnlSourceOnly.Visible = dgSourceOnlyById.Items.Count + dgSourceOnlyByHash.Items.Count > 0
				pnlTargetOnly.Visible = dgTargetOnlyById.Items.Count + dgTargetOnlyByHash.Items.Count > 0
				pnlBoth.Visible = dgBoth.Items.Count > 0



			Case Else
				pnlSourceOnly.Visible = False
				pnlTargetOnly.Visible = False
				pnlBoth.Visible = False
				plh.Visible = False
		End Select



	End Sub

	Private Sub ddTable_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddTable.SelectedIndexChanged
		CSession.Home_Data_TableName = ddTable.SelectedValue
		Response.Redirect(Request.RawUrl)
	End Sub

	Private Sub ddSource_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddSource.SelectedIndexChanged
		CSession.Home_Schema_RefInstanceId = CDropdown.GetInt(ddSource)
		Response.Redirect(Request.RawUrl)
	End Sub


	Private Sub DisplayCounts(sourceId As Integer, targetId As Integer, source As CDataSrc, target As CDataSrc)
		plhSingle.Visible = False
		colSummary.Visible = False

		btnFixData.Visible = False
		btnFixIds.Visible = False

		m_source = source
		m_target = target

		Dim si As CSchemaInfo = source.SchemaInfo
		Dim tbls As List(Of String) = si.Tables.Names

		'Get Counts
		Dim dSource As New Dictionary(Of String, Integer)(tbls.Count)
		Dim dTarget As New Dictionary(Of String, Integer)(tbls.Count)

		Parallel.ForEach(Of String)(tbls,
			Sub(tbl)
				Dim count As Integer
				Dim tbl_ As String = String.Concat("[", tbl.Replace(".", "].["), "]")
				Try
					count = source.SelectCount(tbl_, Nothing)
					SyncLock dSource
						dSource.Add(tbl, count)
					End SyncLock
				Catch
					SyncLock dSource
						dSource.Add(tbl, -1)
					End SyncLock
				End Try


				Try
					count = target.SelectCount(tbl_, Nothing)
					SyncLock dTarget
						dTarget.Add(tbl, count)
					End SyncLock
				Catch
					SyncLock dTarget
						dTarget.Add(tbl, -1)
					End SyncLock
				End Try
			End Sub)

		'Get names
		Dim sourceName As String = ddSource.SelectedItem.Text
		Dim targetName As String = ddTarget.SelectedItem.Text

		Dim btnFixAll As New Button
		btnFixAll.Text = "Fix Data (Selected Tables)"
		btnFixAll.OnClientClick = "return confirm('Sync data for all selected tables?')"
		btnFixAll.Font.Size = New FontUnit(FontSize.Smaller)
		AddHandler btnFixAll.Click, AddressOf btnFixAll_click

		Dim btnDropFKs As New Button
		btnDropFKs.Text = "Drop FKs"
		btnDropFKs.OnClientClick = "return confirm('Drop all FKs?')"
		btnDropFKs.Font.Size = New FontUnit(FontSize.Smaller)
		AddHandler btnDropFKs.Click, AddressOf btnDropFKs_click


		'Header row
		Dim trH As TableRow = RowH(tblCounts)
		Dim tdH As TableCell = CellH(trH, "Table")
		tdH.Controls.Add(btnFixAll)
		tdH.Controls.Add(btnDropFKs)
		CellH(trH, sourceName)
		CellH(trH, targetName)
		CellH(trH)


		'Data Rows
		For Each i As String In tbls
			Dim tr = Row(tblCounts)
			Dim td = CellH(tr, i)

			Dim td1 As TableCell
			Dim c1 As Integer = 0
			If Not dSource.TryGetValue(i, c1) Then c1 = -1
			If c1 <= 0 Then
				td1 = Cell(tr)
			Else
				td1 = Cell(tr, c1.ToString("n0"))
				td1.HorizontalAlign = HorizontalAlign.Right
			End If

			Dim td2 As TableCell
			Dim c2 As Integer = 0
			If Not dTarget.TryGetValue(i, c2) Then c2 = -1
			If c2 <= 0 Then
				td2 = Cell(tr)
			Else
				td2 = Cell(tr, c2.ToString("n0"))
				td2.HorizontalAlign = HorizontalAlign.Right
			End If

			If c1 <> c2 Then
				td1.Font.Bold = True
				td2.Font.Bold = True
			End If

			Dim td3 = Cell(tr)
			td3.Style.Add("padding", "0px")
			Dim chk As New CheckBox()
			chk.ID = "chk_" + i
			chk.Checked = (c1 > 0 OrElse c2 > 0) AndAlso Not i.ToLower.Contains("hangfire") AndAlso c1 <> c2
			td3.Controls.Add(chk)
			m_chk.Add(chk)

			'Dim ti As CTableInfo = si.Tables.GetByName(i)
			'If Not IsNothing(ti) AndAlso ti.PrimaryKey.ColumnNames.Count > 1 Then
			'	chk.Checked = False
			'	chk.Enabled = False
			'End If
		Next

	End Sub
	Private Sub btnDropFKs_click(sender As Object, e As EventArgs)
		Dim schema As CSchemaInfo = m_target.SchemaInfo
		Dim tryAgain As New List(Of String)
		For Each i As CForeignKey In schema.ForeignKeys
			Try
				m_target.ExecuteNonQuery(i.DropScript)
			Catch
				tryAgain.Add(i.DropScript)
			End Try
		Next
		For Each i As String In tryAgain
			Try
				m_target.ExecuteNonQuery(i)
			Catch
			End Try
		Next
		Response.Redirect(Request.RawUrl)
	End Sub

	Private Sub btnFixAll_click(sender As Object, e As EventArgs)
		Dim schema As CSchemaInfo = m_source.SchemaInfo

		Server.ScriptTimeout = 3600 'hour

		Dim fks As New CForeignKeyList()
		For Each i As CheckBox In m_chk
			Try
				If Not i.Checked Then Continue For
				Dim tableName As String = i.ID.Substring(4)
				Dim tableName_ As String = String.Concat("[", tableName.Replace(".", "].["), "]")

				'Switch on PK type
				Dim tbl As CTableInfo = schema.Tables.GetByName(tableName)
				If IsNothing(tbl) Then tbl = New CTableInfoList(m_source, True).GetByName(tableName)
				If IsNothing(tbl) OrElse 0 = tbl.Columns.Count Then Exit Sub
				Dim pks As List(Of String) = tbl.PrimaryKey.ColumnNames
				If pks.Count > 1 Then
					m_target.DeleteAll(tableName_, Nothing)
					m_target.BulkInsert(m_source.SelectAll_Dataset(tableName_).Tables(0), tableName_)
					Continue For
				End If

				Dim type As String = tbl.Columns.Item(pks(0)).Type
				If type.StartsWith("NVARCHAR") Then type = "NVARCHAR"
				If type.StartsWith("VARCHAR") Then type = "VARCHAR"


				Select Case type
					Case = "INT"
						Dim d As New CDataDiff_PkInt(m_source, m_target, tableName_)
						Try
							d.Resolve(tbl.PrimaryKey.IsIdentity)
						Catch
							fks.AddRange(tbl.ForeignKeys)
							For Each fk As CForeignKey In tbl.ForeignKeys
								Try
									m_target.ExecuteNonQuery(fk.DropScript())
								Catch
								End Try
							Next
							d.Resolve(tbl.PrimaryKey.IsIdentity)
						End Try

					Case = "BIGINT"
						Dim d As New CDataDiff_PkLong(m_source, m_target, tableName_)
						Try
							d.Resolve(tbl.PrimaryKey.IsIdentity)
						Catch
							fks.AddRange(tbl.ForeignKeys)
							For Each fk As CForeignKey In tbl.ForeignKeys
								Try
									m_target.ExecuteNonQuery(fk.DropScript())
								Catch
								End Try
							Next
							d.Resolve(tbl.PrimaryKey.IsIdentity)
						End Try


					Case = "NVARCHAR"
						Dim d As New CDataDiff_PkString(m_source, m_target, tableName_)
						Try
							d.Resolve()
						Catch
							fks.AddRange(tbl.ForeignKeys)
							For Each fk As CForeignKey In tbl.ForeignKeys
								Try
									m_target.ExecuteNonQuery(fk.DropScript())
								Catch
								End Try
							Next
							d.Resolve()
						End Try


					Case = "UNIQUEIDENTIFIER"
						Dim d As New CDataDiff_PkGuid(m_source, m_target, tableName_)
						Try
							d.Resolve()
						Catch
							fks.AddRange(tbl.ForeignKeys)
							For Each fk As CForeignKey In tbl.ForeignKeys
								Try
									m_target.ExecuteNonQuery(fk.DropScript())
								Catch
								End Try
							Next
							d.Resolve()
						End Try
				End Select
			Catch
				Throw
			Finally
				For Each fk As CForeignKey In fks
					Try
						m_target.ExecuteNonQuery(fk.CreateScript())
					Catch
					End Try
				Next
			End Try
		Next
		Response.Redirect(Request.RawUrl)
	End Sub

#End Region

End Class
