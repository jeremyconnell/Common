
Partial Class pages_sql_iframe
    Inherits System.Web.UI.Page



    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If CSession.SqlIsSelect Then
            dg.DataSource = CDataSrc.Default.ExecuteDataSet(CSession.SqlStatement)
            dg.DataBind()
        Else
            Response.ContentType = "text/plain"
            Response.Write(CDataSrc.Default.ExecuteNonQuery(CSession.SqlStatement))
            Response.Write(" rows affected")
            Response.End()
        End If
    End Sub
End Class
