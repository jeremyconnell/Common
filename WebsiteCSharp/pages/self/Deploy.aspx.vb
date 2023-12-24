
Imports Framework

Partial Class pages_self_Deploy
    Inherits CPage

    Public Const CHUNK = CFolderHash.FOUR_MB
    Public Const MEMORY_LIMIT = 5 * CHUNK

#Region "Page Events"
    Protected Overrides Sub PageInit()
        'If Not CConfig.IsDev Then Response.Redirect(CSitemap.SelfSql)
        'If Not CConfig.IsDev Then Response.Redirect(CSitemap.Home)


        txtExclude.Text = CSession.Home_Ignore
        txtHost.Text = CSession.Home_ProdHost
        txtDev.Text = CSession.Home_DevDir
        txtFolder.Text = CSession.Home_RemoteDir
        chkFast.Checked = CSession.Home_FastHash

        If False Then ' then True Then
            CSession.Home_ProdHost = "http://13.89.245.228/default.ashx"
            CSession.Home_DevDir = "C:\_crypton\Website\App_Data\CoinMarketCap\"
            CSession.Home_RemoteDir = "_coinmarketcap"
        End If



        'Menus
        UnbindSideMenu()
        AddMenuSide("Deploy", CSitemap.SelfDeploy(), True)
        'If CConfig.IsDev Then
        AddMenuSide("Schema", CSitemap.SelfSchemaSync)
        AddMenuSide("Data", CSitemap.SelfDataSync)
        'End If
        AddMenuSide("Sql", CSitemap.SelfSql)

    End Sub
    Protected Overrides Sub PagePreRender()
        'Session vars
        If Page.IsPostBack Then
            CSession.Home_DevDir = txtDev.Text
            CSession.Home_ProdHost = txtHost.Text
            CSession.Home_Ignore = txtExclude.Text
            CSession.Home_FastHash = chkFast.Checked
        End If

        SelfDeploy()
    End Sub
#End Region


#Region "Page State"
    Public ReadOnly Property FolderPath As String
        Get
            Return txtDev.Text
        End Get
    End Property
    Public ReadOnly Property HostName As String
        Get
            Return txtHost.Text
        End Get
    End Property
#End Region

#Region "Local/Remote"
    Private m_local As CFolderHash
    Private m_remote As CFolderHash
    Private m_prod As CPushUpgradeClient
    Private m_diff As CFilesList

    Public ReadOnly Property Local As CFolderHash
        Get
            If IsNothing(m_local) Then
                m_local = New CFolderHash(FolderPath, True, txtExclude.Text, CHUNK, CSession.Home_FastHash)
            End If
            Return m_local
        End Get
    End Property
    Public ReadOnly Property Remote As CFolderHash
        Get
            If IsNothing(m_remote) Then
                If Not String.IsNullOrEmpty(CSession.Home_RemoteDir) AndAlso CSession.Home_RemoteDir <> CFolderHash.WWWROOT Then
                    m_remote = Prod.ExeVersion(CSession.Home_RemoteDir, txtExclude.Text, True, CHUNK, CSession.Home_FastHash)
                Else
                    m_remote = Prod.RequestHash(txtExclude.Text, True, CHUNK, CSession.Home_FastHash)
                End If
            End If
            Return m_remote
        End Get
    End Property

    Public ReadOnly Property Prod As CPushUpgradeClient
        Get
            If IsNothing(m_prod) Then
                m_prod = New CPushUpgradeClient_Http(HostName, False)
            End If
            Return m_prod
        End Get
    End Property
#End Region


#Region "Form EVents"
    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        Framework.CApplication.ClearAll()
        Response.Redirect(Request.RawUrl)
    End Sub
    Private Sub btnDeploy_Click(sender As Object, e As EventArgs) Handles btnDeploy.Click
        Dim diff As CFilesList = Local.ResolveDifferences(Remote, FolderPath, MEMORY_LIMIT, CHUNK, CSession.Home_FastHash)
        m_local = Nothing
        m_remote = Nothing

        'diff = New CFilesList(CShuffle.Shuffle(Of CFileNameAndContent)(diff))

        While diff.Count > 0
            If Not String.IsNullOrEmpty(CSession.Home_RemoteDir) AndAlso CSession.Home_RemoteDir <> CFolderHash.WWWROOT Then
                Prod.ExePushFiles(CSession.Home_RemoteDir, diff, txtExclude.Text, True, CHUNK, True, CSession.Home_FastHash)
            Else
                Prod.PushFiles(diff, txtExclude.Text, True, CHUNK, CSession.Home_FastHash)
            End If

            m_local = Nothing
            m_remote = Nothing
            diff = Local.ResolveDifferences(Remote, FolderPath, MEMORY_LIMIT, CHUNK, CSession.Home_FastHash)
        End While


        Response.Redirect(Request.RawUrl, True)
    End Sub
#End Region

#Region "SelfDeploy"
    Private Sub SelfDeploy()

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
            lblRemote.Text = Remote.Base64Trunc 'Prod.RequestHash_Base64Trunc() 'Self.PollVersion_Base64()  '
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


        'Diff on hash-only
        Dim locOnly As List(Of String) = byName.SourceOnly ' local.DetectMissing(remote).Names '33
        Dim common As List(Of String) = byName.Matching ' local.DetectCommon(remote).Names '301
        Dim remOnly As List(Of String) = byName.TargetOnly ' local.DetectNew(remote).Names '0
        Dim differ As New List(Of String)
        For Each i As String In byName.Matching
            If local.GetFile(i).MD5 <> remote.GetFile(i).MD5 Then differ.Add(i)
        Next

        'Summarise
        colLoc.InnerText = String.Concat("Local-Only: ", locOnly.Count, " of ", local.Count)
        colDif.InnerText = String.Concat("Different: ", differ.Count, " of ", common.Count)
        colRem.InnerText = String.Concat("Remote-Only: ", remOnly.Count, " of ", remote.Count)

        'Add file info
        Dim localOnly As List(Of String) = AddSizes(locOnly, local)
        Dim different As List(Of String) = AddSizes(differ, local)
        Dim remoteOnly As List(Of String) = AddSizes(remOnly, remote)


        'Display
        ShowDiff(localOnly, plhLoc)
        ShowDiff(different, plhDif)
        ShowDiff(remoteOnly, plhRem)

        'Diff again (using filesystem)
        Dim diff As CFilesList = local.ResolveDifferences(remote, FolderPath, MEMORY_LIMIT, CHUNK, CSession.Home_FastHash)

        Dim localTotalBytes As Long = local.Total
        If localTotalBytes = 0 Then localTotalBytes = CUtilities.FolderSize(FolderPath)
        Dim localOnlyBytes As Long = diff.TotalFor(locOnly)
        Dim differentBytes As Long = diff.TotalFor(differ)
        Dim remoteOnlyBytes As Long = remote.TotalFor(remOnly)



        'Totals
        lblLocalTotal.Text = CUtilities.FileSize(localTotalBytes)
        lblToDeploy.Text = CUtilities.FileSize(localOnlyBytes + differentBytes)
        If (localOnlyBytes > 0) Then colLoc.InnerText = CUtilities.FileNameAndSize(colLoc.InnerText, localOnlyBytes)
        If (differentBytes > 0) Then colDif.InnerText = CUtilities.FileNameAndSize(colDif.InnerText, differentBytes)
        If (remoteOnlyBytes > 0) Then colRem.InnerText = CUtilities.FileNameAndSize(colRem.InnerText, remoteOnlyBytes)
    End Sub
    Private Sub ShowDiff(names As List(Of String), plh As Control)
        Dim pre As New HtmlGenericControl("pre")
        pre.InnerText = CUtilities.ListToString(names, vbCrLf)
        plh.Controls.Add(pre)
    End Sub
    Private Function AddSizes(names As List(Of String), folder As CFolderHash) As List(Of String)
        Dim dir As String = FolderPath
        If Not dir.EndsWith("\") AndAlso Not dir.EndsWith("/") Then dir &= "\"
        Dim list As New List(Of String)(names.Count)
        For Each i As String In names
            Dim fh As CFileHash = folder.GetFile(i)
            If Not IsNothing(fh) AndAlso fh.Size > 0 Then
                Dim size As Long = fh.Size
                If Not IsNothing(fh.Chunks) AndAlso size = 0 Then
                    For Each j As CFileHash In fh.Chunks
                        size += j.Size
                    Next
                End If
                list.Add(String.Concat(i, " (", CUtilities.FileSize(size), ")"))
            Else
                list.Add(String.Concat(i, " (", CUtilities.FileSize(dir & i), ")"))
            End If
        Next
        Return list
    End Function
#End Region

    Protected Sub chkFast_CheckedChanged(sender As Object, e As EventArgs)
        CSession.Home_FastHash = chkFast.Checked
    End Sub
End Class
