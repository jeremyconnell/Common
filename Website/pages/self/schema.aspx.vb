

Partial Class pages_self_schema
    Inherits CPageWithTableHelpers


#Region "Querystring"
    Public ReadOnly Property DiffInstanceId As Integer
        Get
            Return CWeb.RequestInt("diffInstanceId", ESource.Prod)
        End Get
    End Property
    Public ReadOnly Property FixId As EFix
        Get
            Return CWeb.RequestInt("fix")
        End Get
    End Property
#End Region

#Region "Form Events"
    Protected Overrides Sub PageInit()
        'Menu
        UnbindSideMenu()
        If CConfig.IsDev Then
            AddMenuSide("Deploy", CSitemap.SelfDeploy())
            AddMenuSide("Schema", CSitemap.SelfSchemaSync, True)
            AddMenuSide("Data", CSitemap.SelfDataSync)
        End If
        AddMenuSide("Sql", CSitemap.SelfSql)



        If CConfig.IsDev Then
            CDropdown.AddEnums(ddSource, GetType(ESource))
            CDropdown.SetValue(ddSource, CSession.SourceId)
            txtSource.Text = CSession.DbSrc.ConnectionString
            txtSource.Enabled = CSession.SourceId <> ESource.Local

            CDropdown.AddEnums(ddTarget, GetType(ESource))
            CDropdown.SetValue(ddTarget, CSession.TargetId)
            txtTarget.Text = CSession.DbTar.ConnectionString
            txtTarget.Enabled = CSession.TargetId <> ESource.Local

        End If

        'Session
        btnToggle.Text = String.Concat(CSession.SourceId.ToString, " =&gt; ", CSession.TargetId.ToString)


    End Sub
    Protected Overrides Sub PagePreRender()

        If IsNothing(CSession.SchemaSrc) OrElse IsNothing(CSession.SchemaTar) Then Exit Sub

        'Display
        Display_Admin(DiffInstanceId, FixId)
    End Sub
#End Region



#Region "Event Handlers: ddInstance/Refresh"
    Private Sub ddSource_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddSource.SelectedIndexChanged
        CSession.SourceId = CDropdown.GetInt(ddSource)
        Response.Redirect(Request.RawUrl, True)
    End Sub

    Private Sub ddTarget_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddTarget.SelectedIndexChanged
        CSession.TargetId = CDropdown.GetInt(ddTarget)
        Response.Redirect(Request.RawUrl, True)
    End Sub
    Private Sub btnSaveSrc_Click(sender As Object, e As EventArgs) Handles btnSaveSrc.Click
        Select Case CSession.SourceId
            Case ESource.Prod : CSession.Home_ProdUrl = txtSource.Text
            Case ESource.Local : CSession.Home_OtherUrl = txtSource.Text
        End Select
        CSession.SchemaSrc = Nothing
        Response.Redirect(Request.RawUrl)
    End Sub

    Private Sub btnSaveTar_Click(sender As Object, e As EventArgs) Handles btnSaveTar.Click
        Select Case CSession.TargetId
            Case ESource.Prod : CSession.Home_ProdUrl = txtTarget.Text
            Case ESource.Local : CSession.Home_OtherUrl = txtTarget.Text
        End Select
        CSession.SchemaTar = Nothing
        Response.Redirect(Request.RawUrl)
    End Sub
    Private Sub btnToggle_Click(sender As Object, e As EventArgs) Handles btnToggle.Click
        Dim temp As ESource = CSession.SourceId
        CSession.SourceId = CSession.TargetId
        CSession.TargetId = temp
        Response.Redirect(Request.RawUrl)
    End Sub
#End Region


#Region "Display - Admin"
    Public Sub Display_Admin(target As ESource, fixId As EFix)
        'Ref-Schema
        Dim trRef As TableRow = Nothing
        DiffAdmin(target, fixId)

        AddRows(tbl)
    End Sub
    Private Sub AddRows(tbl As Table)
        Dim trRef As TableRow = Nothing
        Dim errRef As String = Nothing

        Dim refId As ESource = CSession.SourceId
        Dim tarId As ESource = CSession.TargetId

        AddRow(tbl, refId, CSession.SchemaSrc, trRef, errRef, CSession.DbSrc, refId)
        AddRow(tbl, tarId, CSession.SchemaTar, trRef, errRef, CSession.DbTar, refId)
    End Sub
    Private Function AddRow(tbl As Table, id As ESource, info As CSchemaInfo, ByRef trRef As TableRow, ByRef errRef As String, db As CDataSrc, refId As Integer) As TableRow
        Dim tr As TableRow = Row(tbl)
        If IsNothing(trRef) Then trRef = tr

        'Admin
        CellH(tr, (tbl.Rows.Count - 1) & ".")
        CellH(tr, id.ToString) '.ToolTip = toolTip

        Dim errM As String = String.Empty
        If IsNothing(CSession.SchemaSrc) Then errRef = errM

        'Data
        AddRow(tr, id, info, CSession.SchemaSrc, errM, errRef, trRef, db)

        Return tr
    End Function
#End Region

#Region "Common - AddRow"
    Public Function LinkToFix(fix As EFix, instanceId As Integer) As String
        Return CSitemap.SelfSchemaSync_Diff(instanceId, CInt(fix))
    End Function


    Private Sub AddRow(tr As TableRow, instanceId As Integer, info As CSchemaInfo, refInfo As CSchemaInfo, errInfo As String, errRefInfo As String, ByRef trRef As TableRow, db As CDataSrc)
        Dim N As Integer = 11

        If IsNothing(info) Then
            Dim tdErr As TableCell = Cell(tr, errInfo)
            tdErr.ColumnSpan = N
            tdErr.Font.Size = New FontUnit(FontSize.Smaller)
            tdErr.ForeColor = Drawing.Color.Red
            Exit Sub
        End If

        If IsNothing(refInfo) Then refInfo = info


        Dim isRef As Boolean = (CSession.DbSrc.ConnectionString = db.ConnectionString)
        If isRef Then tr.Style.Add("background-color", "‎#FFFFE0")





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
                For Each i As CViewDiff In diff.ViewDiff.Different
                    colChanges.Add(i.This.ViewName & ": " & CBinary.ToBase64(i.This.MD5, 10))
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
            Dim fks As Framework.CForeignKeyList = info.ForeignKeys ' db.ForeignKeys__()
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

        ''Default Vals
        'Try
        '    Dim defs As CDefaultValueList = info.DefaultValues
        '    Dim txt As String = CUtilities.CountSummary(defs, "Def", "none")
        '    Dim ttip As String = CUtilities.ListToString(defs, vbCrLf)
        '    Dim lnk As String = LinkToFix(EFix.DefaultVals, instanceId)
        '    Dim msg As String = "Fix Default Vals Now?"
        '    If isRef Then
        '        Cell(tr, txt, ttip, True)
        '    Else
        '        ttip = diff.DefaultDiff.ToString
        '        Dim td As TableCell = CellLink(tr, txt, lnk, False, False, ttip, msg)
        '        Dim hl As HyperLink = td.Controls(0)
        '        If Not diff.DefaultDiff.IsCloseEnough Then hl.ForeColor = Drawing.Color.Red
        '    End If
        'Catch ex As Exception
        '    Cell(tr, ex.Message).ColumnSpan = N - 7
        '    Exit Sub
        'End Try

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


        CellLink(tr, "view", CSitemap.SchemaView(instanceId))
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

    Private Function DiffAdmin(target As ESource, Optional fixId As EFix = EFix.None) As CSchemaDiff
        'Local
        Dim name As String = "Local => Prod"
        If target = ESource.Local Then name = "Prod => Local"

        'Prod 
        If IsNothing(CSession.SchemaTar) Then Return Nothing


        'Action
        '       Dim d As CSchemaDiff = CSession.SchemaTar.Diff(CSession.SchemaSrc)
        Dim d As CSchemaDiff = CSession.SchemaTar.Diff(CSession.SchemaSrc)

        FixInstance(d, name, CSession.DbTar, CSession.SchemaTar, CSession.SchemaSrc, CSession.DbSrc, fixId)

        If fixId <> EFix.None AndAlso fixId <> EFix.ScriptOnly Then
            CSession.SchemaTar = Nothing
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
        'FixInstance_Defaults(diff, fixId, sql, False)
        FixInstance_Tables(diff, fixId, sql, False)
        FixInstance_Indexes(diff, fixId, sql)
        FixInstance_FKs(diff, fixId, sql)
        FixInstance_Tables(diff, fixId, sql, True)
        'FixInstance_Defaults(diff, fixId, sql, True)
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

        If CSession.TargetId = ESource.Local Then
            CSession.SchemaLocal = Nothing
        Else
            CSession.SchemaProd = Nothing
        End If

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
    'Private Shared Sub FixInstance_Defaults(diff As CSchemaDiff, f As EFix, sqlList As List(Of String), create As Boolean)
    '    If f <> EFix.Tables AndAlso f <> EFix.Cols AndAlso f <> EFix.All AndAlso f <> EFix.ScriptOnly Then Exit Sub
    '    sqlList.AddRange(diff.DefaultDiff.ChangeScripts(Not create))
    'End Sub

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
