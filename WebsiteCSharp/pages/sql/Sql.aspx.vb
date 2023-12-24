
Partial Class pages_sql_Sql
    Inherits CPage

#Region "Querystring"
#End Region

#Region "Data"
#End Region

#Region "Page Events"
    Protected Overrides Sub PageInit()
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
        CSession.SqlStatement = txtSql.Text
        CSession.SqlIsSelect = rbl.SelectedIndex <> 1
        iframe.Attributes.Add("src", "iframe.aspx")
    End Sub
#End Region

End Class
