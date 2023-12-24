
Imports System.Data
Imports System.Threading.Tasks
Imports Comms.PushUpgrade.Client

Partial Class pages_self_data
    Inherits CPageWithTableHelpers



    Private m_chk As New Dictionary(Of String, CheckBox)
    Private m_diff As CDiff
    Private m_diffFull As CDiffFull
    Private m_tbl As CTableInfo


    Public ReadOnly Property FullScan As Boolean
        Get
            Return 1 = CSession.Home_Data_FullScan
        End Get
    End Property
    Public ReadOnly Property TableName As String
        Get
            Return CWeb.RequestStr("t", CSession.TableName)
        End Get
    End Property

#Region "Page Events"
    Protected Overrides Sub PageInit()
        UnbindSideMenu()
        If CConfig.IsDev Then
            AddMenuSide("Deploy", CSitemap.SelfDeploy())
            AddMenuSide("Schema", CSitemap.SelfSchemaSync)
        End If
        AddMenuSide("Data", CSitemap.SelfDataSync, True)
        AddMenuSide("Sql", CSitemap.SelfSql)

        CSession.TableName = TableName


        CDropdown.AddEnums(ddSource, GetType(ESource))
        CDropdown.SetValue(ddSource, CSession.SourceId)
        Select Case CSession.SourceId
            Case ESource.Local : txtSource.Text = CDataSrc.Default.ConnectionString
            Case ESource.Prod : txtSource.Text = CSession.Home_ProdUrl
            Case Else : txtSource.Text = CSession.Home_OtherUrl
        End Select
        txtSource.Enabled = CSession.SourceId <> ESource.Local

        CDropdown.AddEnums(ddTarget, GetType(ESource))
        CDropdown.SetValue(ddTarget, CSession.TargetId)
        Select Case CSession.TargetId
            Case ESource.Local : txtTarget.Text = CDataSrc.Default.ConnectionString
            Case ESource.Prod : txtTarget.Text = CSession.Home_ProdUrl
            Case Else : txtTarget.Text = CSession.Home_OtherUrl
        End Select
        txtTarget.Enabled = CSession.TargetId <> ESource.Local

        ddTable.DataSource = CDataSrc.Default.AllTableNames(True)
        ddTable.DataBind()
        CDropdown.BlankItem(ddTable, "* (all tables)", "*")
        CDropdown.SetValue(ddTable, CSession.TableName)


        btnToggle.Text = String.Concat(CSession.SourceId.ToString, " =&gt; ", CSession.TargetId.ToString)
        btnFixIds.Visible = Not FullScan
        btnFixData.Visible = FullScan

        plh.Visible = "*" <> CSession.TableName

        DisplayDiff()
    End Sub
#End Region





#Region "Form Events"
    Private Sub ddTable_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddTable.SelectedIndexChanged
        CSession.TableName = ddTable.SelectedValue
        Response.Redirect(CSitemap.SelfDataSync, True)
    End Sub

    Private Sub ddSource_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddSource.SelectedIndexChanged
        CSession.SourceId = CDropdown.GetInt(ddSource)
        Response.Redirect(CSitemap.SelfDataSync, True)
    End Sub

    Private Sub ddTarget_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddTarget.SelectedIndexChanged
        CSession.TargetId = CDropdown.GetInt(ddTarget)
        Response.Redirect(CSitemap.SelfDataSync, True)
    End Sub

    Private Sub rblFullScan_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblFullScan.SelectedIndexChanged
        CSession.Home_Data_FullScan = rblFullScan.SelectedIndex
        Response.Redirect(Request.RawUrl, True)
    End Sub
    Private Sub btnDropFKs_click(sender As Object, e As EventArgs)
        Dim schema As CSchemaInfo = CSession.DbTar.SchemaInfo
        Dim tryAgain As New List(Of String)
        For Each i As Framework.CForeignKey In schema.ForeignKeys
            Try
                CSession.DbTar.ExecuteNonQuery(i.DropScript)
            Catch
                tryAgain.Add(i.DropScript)
            End Try
        Next
        For Each i As String In tryAgain
            Try
                CSession.DbTar.ExecuteNonQuery(i)
            Catch
            End Try
        Next
        Response.Redirect(Request.RawUrl)
    End Sub



    Private Sub btnSaveSrc_Click(sender As Object, e As EventArgs) Handles btnSaveSrc.Click
        Select Case CSession.SourceId
            Case ESource.Prod : CSession.Home_ProdUrl = txtSource.Text
            Case ESource.Local : CSession.Home_OtherUrl = txtSource.Text
        End Select
        Response.Redirect(Request.RawUrl)
    End Sub

    Private Sub btnSaveTar_Click(sender As Object, e As EventArgs) Handles btnSaveTar.Click
        Select Case CSession.TargetId
            Case ESource.Prod : CSession.Home_ProdUrl = txtTarget.Text
            Case ESource.Local : CSession.Home_OtherUrl = txtTarget.Text
        End Select
        Response.Redirect(Request.RawUrl)
    End Sub

    Private Sub btnToggle_Click(sender As Object, e As EventArgs) Handles btnToggle.Click
        Dim temp As ESource = CSession.SourceId
        CSession.SourceId = CSession.TargetId
        CSession.TargetId = temp
        Response.Redirect(Request.RawUrl)
    End Sub

    Private Sub btnFixIds_Click(sender As Object, e As EventArgs) Handles btnFixData.Click
        Dim isId As Boolean = m_tbl.PrimaryKey.IsIdentity
        Dim g As New CGenericActions(m_tbl.TableName_, CSession.DbSrc, CSession.DbTar, m_tbl.Columns.Names, isId)
        If CSession.DbTar.IsLocal Then
            If Not IsNothing(m_diffFull) Then
                'local db: individual inserts
                For Each i As CRow In m_diffFull.SourceOnly
                    Try
                        g.Insert(i.Join)
                    Catch
                    End Try
                Next
                'local db: individual updates
                For Each i As CRow In m_diffFull.Different
                    Try
                        g.Update(i.Changes, i.PK)
                    Catch
                    End Try
                Next
            Else
                'Pks only - select page of 20
                For Each i As CValues In m_diff.SourceOnly
                    Try
                        g.CopyBulk(i)
                    Catch
                    End Try
                    Try
                        g.Flush()
                    Catch
                    End Try
                Next
            End If
            'Delete indiv
            For Each i As CValues In m_diff.TargetOnly
                Try
                    g.Delete(i)
                Catch
                End Try
            Next
        Else
            If Not IsNothing(m_diffFull) Then
                'insert 20 at a time (or can do in parallel)
                For Each i As CRow In m_diffFull.SourceOnly
                    Try
                        g.InsertBulk(i.Join)
                    Catch
                    End Try
                Next
                Try
                    g.Flush()
                Catch
                End Try

                'Parallel updates
                Parallel.ForEach(m_diffFull.Different,
                Sub(i As CRow)
                    Try
                        g.Update(i.Changes, i.PK)
                    Catch
                    End Try
                End Sub
                )
            Else
                'select then insert 20 at a time
                For Each i As CValues In m_diff.SourceOnly
                    Try
                        g.CopyBulk(i)
                    Catch
                    End Try
                Next
                Try
                    g.Flush()
                Catch
                End Try
            End If
            'delete 20 at a time
            For Each i As CValues In m_diff.TargetOnly
                Try
                    g.DeleteBulk(i)
                Catch
                End Try
            Next
            Try
                g.Flush()
            Catch
            End Try
        End If
        'CDiffLogic.RowByRow_InsertAndDelete(CSession.DbSrc, CSession.DbTar, m_tbl, True)

        Response.Redirect(Request.RawUrl)
    End Sub


    Private Sub btnFixAll_click(sender As Object, e As EventArgs) Handles btnFixIds.Click
        Dim schema As CSchemaInfo = CSession.SchemaSrc

        Server.ScriptTimeout = 3600 'hour

        'Inserts/Updates
        Dim tables As CTableInfoList = schema.Tables_InsertOrder
        For Each t As CTableInfo In tables
            If Not m_chk(t.TableName.ToLower).Checked Then Continue For

            Dim isId As Boolean = t.PrimaryKey.IsIdentity
            Dim pkN As List(Of String) = t.PrimaryKey.ColumnNames

            Dim d As New CDataDiff_FullScan(CSession.DbSrc, CSession.DbTar, t.TableName, pkN)


            Dim g As New CGenericActions(t.TableName_, CSession.DbSrc, CSession.DbTar, t.Columns.Names, isId)
            If CSession.DbTar.IsLocal OrElse t.PrimaryKey.ColumnNames.Count > 1 Then
                For Each i As CRow In d.Diff.SourceOnly 'todo; crow
                    Try
                        g.Insert(i)
                    Catch
                    End Try
                Next
                For Each i As CRow In d.Diff.Different
                    Try
                        g.Update(i.Changes, i.PK)
                    Catch
                    End Try
                Next
            Else
                Dim po As New ParallelOptions()
                po.MaxDegreeOfParallelism = 20
                Parallel.ForEach(d.Diff.SourceOnly, po,
                    Sub(i As CRow)
                        Try
                            'g.InsertBulk(i.Join)
                            g.Insert(i) 'par or bulk, not both
                        Catch
                        End Try
                    End Sub
                    )
                g.Flush()
                Parallel.ForEach(d.Diff.Different, po,
                    Sub(i As CRow)
                        Try
                            g.Update(i.Changes, i.PK)
                        Catch
                        End Try
                    End Sub
                    )
            End If
        Next

        tables.Reverse()

        'Deletes
        For Each t As CTableInfo In tables
            If Not m_chk(t.TableName.ToLower).Checked Then Continue For
            Dim pkN As List(Of String) = t.PrimaryKey.ColumnNames
            Dim isId As Boolean = t.PrimaryKey.IsIdentity

            Dim d As New CDataDiff_PksOnly(CSession.DbSrc, CSession.DbTar, t.TableName, pkN)


            Dim g As New CGenericActions(t.TableName_, CSession.DbSrc, CSession.DbTar, t.Columns.Names, isId)
            If CSession.DbTar.IsLocal Then
                For Each i As CValues In d.DiffOnPk.TargetOnly
                    Try
                        g.Delete(i)
                    Catch
                    End Try
                Next
            ElseIf t.PrimaryKey.ColumnNames.Count > 1 Then
                Parallel.ForEach(d.DiffOnPk.TargetOnly,
                    Sub(i As CValues)
                        Try
                            g.Delete(i)
                        Catch
                        End Try
                    End Sub
                    )
                g.Flush()
            Else
                For Each i As CValues In d.DiffOnPk.TargetOnly
                    Try
                        g.DeleteBulk(i)
                    Catch
                    End Try
                Next
                Try
                    g.Flush()
                Catch
                End Try
            End If

        Next

        Response.Redirect(Request.RawUrl)
    End Sub
#End Region

#Region "Display"
    Private Sub DisplayCounts(source As CDataSrc, target As CDataSrc)
        plhSingle.Visible = False

        btnFixData.Visible = False
        btnFixIds.Visible = False


        CSession.SourceId = CDropdown.GetInt(ddSource)
        CSession.TargetId = CDropdown.GetInt(ddTarget)

        Dim si As CSchemaInfo = CSession.SchemaSrc

        Dim tbls As New List(Of String)(si.Tables.Names)
        tbls.Add("sysdiagrams")

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
        Dim chkAll As New CheckBox
        chkAll.Checked = True
        chkAll.Visible = False
        chkAll.AutoPostBack = True
        Dim trH As TableRow = RowH(tblCounts)
        Dim tdH As TableCell = CellH(trH, "Table")
        tdH.Controls.Add(btnFixAll)
        tdH.Controls.Add(btnDropFKs)
        CellH(trH, sourceName)
        CellH(trH, targetName)
        CellH(trH, "&Delta;")
        CellH(trH).Controls.Add(chkAll)


        'Data Rows
        For Each i As String In tbls
            Dim tr = Row(tblCounts)
            Dim td = CellLinkH(tr, i, "data.aspx?t=" & i, False)

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



            Dim td3 As TableCell
            Dim c3 As Integer = c1 - c2
            If c3 = 0 OrElse c1 < 0 OrElse c2 < 0 Then
                td3 = Cell(tr)
            Else
                td3 = Cell(tr, c3.ToString("n0"), , True)
                td3.HorizontalAlign = HorizontalAlign.Right
            End If


            Dim td4 = Cell(tr)
            td4.Style.Add("padding", "0px")
            Dim chk As New CheckBox()
            chk.ID = "chk_" + i
            chk.Checked = (c1 > 0 OrElse c2 > 0) AndAlso Not i.ToLower.Contains("hangfire") AndAlso c1 <> c2
            chk.Enabled = chk.Checked
            td4.Controls.Add(chk)
            m_chk.Add(i, chk)

            'Dim ti As CTableInfo = si.Tables.GetByName(i)
            'If Not IsNothing(ti) AndAlso ti.PrimaryKey.ColumnNames.Count > 1 Then
            '	chk.Checked = False
            '	chk.Enabled = False
            'End If
        Next

    End Sub

    Private Sub DisplayDiff()
        If Not CConfig.IsDev Then Exit Sub

        'Selected
        CSession.SourceId = CDropdown.GetInt(ddSource)
        CSession.TargetId = CDropdown.GetInt(ddTarget)

        'Translate
        Dim source As CDataSrc = CSession.DbSrc
        Dim target As CDataSrc = CSession.DbTar

        'Set timeouts
        If TypeOf source Is CWebSrcBinary Then
            Dim b As CWebSrcBinary = source
            CWebSrcBinary.TimeOutMs = 3600 * 1000
        End If
        If TypeOf target Is CWebSrcBinary Then
            Dim b As CWebSrcBinary = target
            CWebSrcBinary.TimeOutMs = 3600 * 1000
        End If

        'Action
        If CSession.TableName = "*" Then
            DisplayCounts(source, target)
        Else
            DisplayDiff(source, target)
        End If
    End Sub
    Private Sub DisplayDiff(source As CDataSrc, target As CDataSrc)

        'Selection
        Dim tableName As String = CSession.TableName

        'Switch on PK type
        Dim tbl As CTableInfo = CSession.SchemaLocal.Tables.GetByName(tableName)
        If IsNothing(tbl) Then tbl = New CTableInfoList(CDataSrc.Default, True).GetByName(tableName)
        If IsNothing(tbl) OrElse 0 = tbl.Columns.Count Then Exit Sub

        Dim pks As List(Of String) = tbl.PrimaryKey.ColumnNames
        Dim cols As List(Of String) = tbl.Columns.Names


        'New data-diff
        Dim diff As CDiff = Nothing
        If Not FullScan Then
            Dim d As New CDataDiff_PksOnly(source, target, tableName, pks)
            diff = d.DiffOnPk
        Else
            Dim d As New CDataDiff_FullScan(source, target, tableName, pks, cols)
            diff = d.Diff_
            m_diffFull = d.Diff
        End If
        rblFullScan.SelectedIndex = CSession.Home_Data_FullScan


        Dim tr As TableRow = Row(tblCounts)
        If FullScan Then
            CellH(tr, "Source-Only")
            CellR(tr, diff.SourceOnly.Count.ToString("n0"))
            Display(tblSourceOnly, lblSourceOnly, pnlSourceOnly, m_diffFull.SourceOnly, pks, cols)
            Cell(tr, "inserts")

            tr = Row(tblCounts)
            CellH(tr, "Target-Only")
            CellR(tr, diff.TargetOnly.Count.ToString("n0"))
            Display(tblTargetOnly, lblTargetOnly, pnlTargetOnly, m_diffFull.TargetOnly, pks, cols)
            Cell(tr, "deletes")

            tr = Row(tblCounts)
            CellH(tr, "Different")
            CellR(tr, diff.Different.Count.ToString("n0"))
            Display(tblDifferent, lblDifferent, pnlDifferent, m_diffFull.Different, pks, cols)
            Cell(tr, "updates")

            tr = Row(tblCounts)
            CellH(tr, "Matching")
            CellR(tr, diff.Matching.Count.ToString("n0"))
            Display(tblMatching, lblMatching, pnlBoth, m_diffFull.Matching, pks, cols)
            Cell(tr)
        Else
            CellH(tr, "Source-Only")
            CellR(tr, diff.SourceOnly.Count.ToString("n0"))
            Display(tblSourceOnly, lblSourceOnly, pnlSourceOnly, diff.SourceOnly, pks, cols, source)
            Cell(tr, "inserts")

            tr = Row(tblCounts)
            CellH(tr, "Target-Only")
            CellR(tr, diff.TargetOnly.Count.ToString("n0"))
            Display(tblTargetOnly, lblTargetOnly, pnlTargetOnly, diff.TargetOnly, pks, cols, target)
            Cell(tr, "deletes")

            tr = Row(tblCounts)
            CellH(tr, "Different")
            CellR(tr, "??")
            Cell(tr, "updates")
            pnlDifferent.Visible = False

            tr = Row(tblCounts)
            CellH(tr, "Matching")
            CellR(tr, diff.Matching.Count.ToString("n0"))
            Display(tblMatching, lblMatching, pnlBoth, diff.Matching, pks, cols, source)
            Cell(tr)
        End If

        tr = Row(tblCounts)
        CellH(tr, "Source (total)")
        CellR(tr, diff.Source.ToString("n0"),, True)
        Cell(tr, "rows")

        tr = Row(tblCounts)
        CellH(tr, "Target (total)")
        CellR(tr, diff.Target.ToString("n0"),, True)
        Cell(tr, "rows")


        m_diff = diff
        m_tbl = tbl
    End Sub
    Private Sub Display(tbl As Table, lbl As Label, pnl As Control, rows As List(Of CValues), pks As List(Of String), cols As List(Of String), db As CDataSrc, Optional pageSize As Integer = 10)
        pnl.Visible = rows.Count > 0
        If Not pnl.Visible Then Exit Sub

        If pageSize < rows.Count Then lbl.Text = CUtilities.CountSummary(rows, "row")
        If Not FullScan AndAlso pks.Count = 1 Then
            lbl.Text = String.Concat(pageSize, " of ", lbl.Text)
            If pageSize > rows.Count Then lbl.Text = CUtilities.CountSummary(rows, "row")

            'get ids
            Dim ids As New List(Of Object)(rows.Count)
            For Each i As CValues In rows
                ids.Add(i(0).Value)
                If ids.Count = pageSize Then Exit For
            Next

            'expand
            Dim dt As DataTable = db.SelectWhere_Dataset(CSession.TableName, pks(0), New CCriteriaList(pks(0), ESign.IN, ids)).Tables(0)
            db.ShiftDates(dt)

            cols = New List(Of String)(dt.Columns.Count)
            For Each i As DataColumn In dt.Columns
                cols.Add(i.ColumnName)
            Next

            rows = New List(Of CValues)(dt.Rows.Count)
            For Each dr As DataRow In dt.Rows
                rows.Add(New CRow(dr, pks, cols).Join)
            Next
        End If


        Dim tr As TableRow = Row(tbl)
        For Each i As String In cols
            CellH(tr, i, i, pks.Contains(i))
        Next

        For Each i As CValues In rows
            tr = Row(tbl)
            For Each j As String In cols
                Dim v As CValue = i(j)
                If IsNothing(v) Then
                    Cell(tr)
                Else
                    Cell(tr, v.ToString, , pks.Contains(j))
                End If
            Next
        Next
    End Sub
    Private Sub Display(tbl As Table, lbl As Label, pnl As Control, rows As List(Of CRow), pks As List(Of String), cols As List(Of String), Optional pageSize As Integer = 1000)
        pnl.Visible = rows.Count > 0
        If Not pnl.Visible Then Exit Sub

        lbl.Text = CUtilities.CountSummary(rows, "row")


        Dim tr As TableRow = Row(tbl)
        For Each i As String In cols
            CellH(tr, i, i)
        Next

        For Each i As CRow In rows
            If rows.IndexOf(i) = pageSize Then Exit For

            tr = Row(tbl)
            For Each j As String In cols
                Dim v As CValue = i.Data(j)
                If IsNothing(v) Then v = i.PK(j)
                If IsNothing(v) Then
                    Cell(tr)
                Else
                    Dim s As String = v.ToString
                    If Not IsNothing(i.Changes) Then
                        Dim jj As CValue = i.Changes.Data(j)
                        If Not IsNothing(jj) Then
                            s = String.Concat("<b>", jj.ToString, "</b><br/>", s)
                            If TypeOf jj.Value Is DateTime And TypeOf v.Value Is DateTime Then
                                Dim ts As TimeSpan = CType(jj.Value, DateTime).Subtract(CType(v.Value, DateTime))
                                s = String.Concat(s, " (", CUtilities.Timespan(ts), ")")
                            End If
                        End If
                    End If
                    Cell(tr, s, , pks.Contains(j))
                End If
            Next
        Next
    End Sub



#End Region

End Class