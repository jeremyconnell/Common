
Imports Comms.PushUpgrade.Client
Imports Comms.Upgrade.Client
Imports Comms.Upgrade.Server

Partial Class pages_self_Deploy
	Inherits CPage

	Public Const PUBLISH As String = "C:\Development\CMS\publish\"

#Region "Page Events"
	Protected Overrides Sub PageInit()
		txtExclude.Text = CSession.Home_Ignore
		txtHost.Text = CSession.Home_ProdHost
		txtDev.Text = CSession.Home_DevDir

		chkPublish.Checked = (PUBLISH = FolderPath)

		If Not CConfig.IsDev Then Response.Redirect(CSitemap.SelfSql)



		UnbindSideMenu()
		AddMenuSide("Deploy", CSitemap.SelfDeploy(), True)
		If CConfig.IsDev Then
			AddMenuSide("Schema", CSitemap.SelfSchemaSync)
			AddMenuSide("Data", CSitemap.SelfDataSync)
		End If
		AddMenuSide("Sql", CSitemap.SelfSql)



		SelfDeploy()
	End Sub
#End Region


#Region "Self-Publish"

	Private m_local As CFolderHash
	Private m_remote As CFolderHash
	Private m_self As CPushUpgradeClient
	Private m_prod As CPushUpgradeClient
	Private m_prodOld As CUpgradeClient

	Public ReadOnly Property Local As CFolderHash
		Get
			If IsNothing(m_local) Then
				m_local = New CFolderHash(FolderPath, Ignore, True) ' Self.LocalVersion(FolderPath, Ignore)
			End If
			Return m_local
		End Get
	End Property
	Public ReadOnly Property Remote As CFolderHash
		Get
			If IsNothing(m_remote) Then
				m_remote = Prod.RequestHash()
			End If
			Return m_remote
		End Get
	End Property
	Public ReadOnly Property Self As CPushUpgradeClient
		Get
			If IsNothing(m_self) Then
				Dim THIS As String = Request.Url.Authority
				Dim USE_SSL As Boolean = Not THIS.ToLower.Contains("localhost")
				If Not USE_SSL Then THIS &= "/admin"
				m_self = CPushUpgradeClient.Factory(THIS, USE_SSL)
			End If
			Return m_self
		End Get
	End Property
	Public ReadOnly Property Prod As CPushUpgradeClient
		Get
			If IsNothing(m_prod) Then
				m_prod = CPushUpgradeClient.Factory(txtHost.Text, True)
			End If
			Return m_prod
		End Get
	End Property
	Public ReadOnly Property Ignore As String()
		Get
			Return CUtilities.SplitOn(txtExclude.Text, ",").ToArray
		End Get
	End Property
	Public ReadOnly Property FolderPath As String
		Get
			Return txtDev.Text
		End Get
	End Property
#End Region



#Region "Form EVents"
	Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
		Framework.CApplication.ClearAll()
		Response.Redirect(Request.RawUrl)
	End Sub
	Private Sub btnDeploy_Click(sender As Object, e As EventArgs) Handles btnDeploy.Click
		Dim diff As CFilesList = Local.ResolveDifferences(Remote, FolderPath)
		Prod.PushFiles(diff)
		Response.Redirect(Request.RawUrl)
	End Sub
#End Region

#Region "SelfDeploy"
	Private Sub SelfDeploy()
		If Not CConfig.IsDev Then Response.Redirect(CSitemap.Clients)

		'Checkbox Action, Session vars
		chkPublish.Enabled = Request.Url.Host.ToLower.Contains("localhost")
		If Page.IsPostBack Then
			If chkPublish.UniqueID = Request.Form("__EVENTTARGET") Then
				If chkPublish.Checked Then
					txtDev.Text = PUBLISH
				Else
					txtDev.Text = CPushUpgradeClient_Config.SELF_FOLDER
				End If
			End If

			CSession.Home_DevDir = txtDev.Text
			CSession.Home_ProdHost = txtHost.Text
			CSession.Home_Ignore = txtExclude.Text
		End If


		btnDeploy.Enabled = False
		plhTbl.Visible = False

		'Serve local via the webservice
		Try
			lblHash.Text = Local.Base64Trunc 'Self.PollVersion_Base64
		Catch ex As Exception
			lblHash.ToolTip = ex.Message
			'Bypass webservice
			lblHash.Text = Local.Base64Trunc
		End Try

		'Download Prod version
		Try
			lblRemote.Text = Prod.PollVersion_Base64() 'Self.PollVersion_Base64()  '
		Catch ex As Exception
			lblRemote.Text = ex.Message
		End Try

		If lblRemote.Text = lblHash.Text Then Exit Sub
		lblRemote.ForeColor = Drawing.Color.Red

		btnDeploy.Enabled = True
		plhTbl.Visible = True

		Try
			ShowDiff(Me.Local, Me.Remote)
		Catch ex As Exception
			CSession.PageMessageEx = ex
		End Try
	End Sub
	Private Sub ShowDiff(local As CFolderHash, remote As CFolderHash)
		Dim byHash As CDiff_Guid = local.DiffOnHash(remote)
		Dim byName As CDiff_String = local.DiffOnName(remote)

		Dim byH As New CDiff_FileHash_ByHash(local, remote)
		Dim byN As New CDiff_FileHash_ByName(local, remote)


		'Diff on hash-only
		Dim locOnly As List(Of String) = byN.SourceOnly ' local.DetectMissing(remote).Names '33
		Dim common As List(Of String) = byN.Matching ' local.DetectCommon(remote).Names '301
		Dim differ As List(Of String) = byN.ChangedNames ' local.DetectDifferent(remote).Names '8
		Dim remOnly As List(Of String) = byN.TargetOnly ' local.DetectNew(remote).Names '0

		'Summarise
		colLoc.InnerText = String.Concat(colLoc.InnerText, ": ", locOnly.Count, " of ", local.Count)
		colDif.InnerText = String.Concat(colDif.InnerText, ": ", differ.Count, " of ", common.Count)
		colRem.InnerText = String.Concat(colRem.InnerText, ": ", remOnly.Count, " of ", remote.Count)

		'Add file info
		Dim localOnly As List(Of String) = AddSizes(locOnly)
		Dim different As List(Of String) = AddSizes(differ)
		Dim remoteOnly As List(Of String) = remOnly


		'Display
		ShowDiff(localOnly, plhLoc)
		ShowDiff(different, plhDif)
		ShowDiff(remoteOnly, plhRem)

		'Diff again (using filesystem)
		Dim diff As CFilesList = local.ResolveDifferences(remote, FolderPath)
		Dim localTotalBytes As Long = CUtilities.FolderSize(FolderPath)
		Dim localOnlyBytes As Long = diff.TotalFor(locOnly)
		Dim differentBytes As Long = diff.TotalFor(differ)



		'Totals
		lblLocalTotal.Text = CUtilities.FileSize(localTotalBytes)
		lblToDeploy.Text = CUtilities.FileSize(localOnlyBytes + differentBytes)
		colLoc.InnerText = CUtilities.FileNameAndSize(colLoc.InnerText, localOnlyBytes)
		colDif.InnerText = CUtilities.FileNameAndSize(colDif.InnerText, differentBytes)
	End Sub
	Private Sub ShowDiff(names As List(Of String), plh As Control)
		Dim pre As New HtmlGenericControl("pre")
		pre.InnerText = CUtilities.ListToString(names, vbCrLf)
		plh.Controls.Add(pre)
	End Sub
	Private Function AddSizes(names As List(Of String)) As List(Of String)
		Dim dir As String = FolderPath
		If Not dir.EndsWith("\") AndAlso Not dir.EndsWith("/") Then dir &= "\"
		Dim list As New List(Of String)(names.Count)
		For Each i As String In names
			Dim path As String = dir & i
			list.Add(String.Concat(i, " (", CUtilities.FileSize(path), ")"))
		Next
		Return list
	End Function
#End Region
End Class
