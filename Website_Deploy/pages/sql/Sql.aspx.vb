
Partial Class pages_sql_Sql
    Inherits CPage



#Region "Page Events"
	Protected Overrides Sub PageInit()
		Dim sql As New CAudit_Sql
		ddHistory.DataSource = sql.Recent()
		ddHistory.DataBind()
		CDropdown.BlankItem(ddHistory, "-- History --")


		AddMenuSide("Old Page")
		AddMenuSide("New Page...", CSitemap.SelfSql)


		sql = sql.Last
		If Not IsNothing(sql) Then txtSql.Text = sql.SqlText


		Try
			ddTables.DataSource = CDataSrc.Default.AllTableNames()
			ddTables.DataBind()
			CDropdown.BlankItem(ddTables)
		Catch ex As Exception
			ddTables.ToolTip = ex.Message
		End Try

		Try
			ddViews.DataSource = CDataSrc.Default.MakeListString("SELECT [Name] FROM [sysobjects] Where xtype='V' ORDER BY [Name]")
			ddViews.DataBind()
			CDropdown.BlankItem(ddViews)
		Catch ex As Exception
			ddViews.ToolTip = ex.Message
		End Try

		Try
			ddFunctions.DataSource = CDataSrc.Default.MakeListString("SELECT [Name] FROM [sysobjects] Where xtype='FN' ORDER BY [Name]")
			ddFunctions.DataBind()
			CDropdown.BlankItem(ddFunctions)
		Catch ex As Exception
			ddFunctions.ToolTip = ex.Message
		End Try

		Try
			ddProcs.DataSource = CDataSrc.Default.MakeListString("SELECT [Name] FROM [sysobjects] Where xtype='P' ORDER BY [Name]")
			ddProcs.DataBind()
			CDropdown.BlankItem(ddProcs)
		Catch ex As Exception
			ddProcs.ToolTip = ex.Message
		End Try
	End Sub
#End Region

#Region "Form Events"
	Protected Sub btnExecute_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExecute.Click
		pnlResults.Visible = True
		CSession.SqlUseConn = Nothing
		CSession.SqlStatement = txtSql.Text
		CSession.SqlIsSelect = rbl.SelectedIndex <> 1
		iframe.Attributes.Add("src", "iframe.aspx")
	End Sub
	Private Sub ddHistory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddHistory.SelectedIndexChanged

		Dim sql As New SchemaAudit.CAudit_Sql(CDropdown.GetInt(ddHistory))
		txtSql.Text = sql.SqlText
	End Sub


	Private Sub ddFunctions_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddFunctions.SelectedIndexChanged
		txtSql.Text = String.Concat(txtSql.Text, " ", ddFunctions.SelectedValue)
	End Sub
	Private Sub ddProcs_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddProcs.SelectedIndexChanged
		txtSql.Text = String.Concat(txtSql.Text, " ", ddProcs.SelectedValue)
	End Sub
	Private Sub ddViews_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddViews.SelectedIndexChanged
		txtSql.Text = String.Concat(txtSql.Text, " ", ddViews.SelectedValue)
	End Sub
	Private Sub ddTables_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddTables.SelectedIndexChanged
		txtSql.Text = String.Concat(txtSql.Text, " ", ddTables.SelectedValue)
	End Sub
#End Region

End Class
