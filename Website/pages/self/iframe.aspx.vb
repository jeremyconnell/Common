
Imports System.Threading.Tasks

Partial Class pages_sql_iframe
    Inherits CPageWithTableHelpers



    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Dim db As CDataSrc = CSession.Db(ESource.Local)
        If CSession.SqlUseProd Then db = CSession.Db(ESource.Prod)


        Try
            With New CAudit_Sql(db)
                .SqlText = CSession.SqlStatement
                .Save()
            End With
        Catch
        End Try

        If CSession.SqlIsSelect Then
            Try
                'sql.SqlText = CSession.SqlStatement
                'sql.Save()

                dg.DataSource = db.ExecuteDataSet(CSession.SqlStatement)
                dg.DataBind()
            Catch ex As Exception
                Response.ContentType = "text/plain"
                Response.Write(ex.Message & vbCrLf & vbCrLf & ex.StackTrace)
                If Not IsNothing(ex.InnerException) Then
                    ex = ex.InnerException
                    Response.Write(ex.Message & vbCrLf & vbCrLf & ex.StackTrace)
                End If
            End Try
        Else
            'sql.SqlIsUpdate = Not CSession.SqlIsSelect
            'sql.SqlText = CSession.SqlStatement
            'sql.Save()

            Response.ContentType = "text/plain"
            Try
                Response.Write(db.ExecuteNonQuery(CSession.SqlStatement))
                Response.Write(" rows affected")
            Catch ex As Exception
                Response.Write(ex.Message & vbCrLf & vbCrLf & ex.StackTrace)
                If Not IsNothing(ex.InnerException) Then
                    ex = ex.InnerException
                    Response.Write(ex.Message & vbCrLf & vbCrLf & ex.StackTrace)
                End If
            End Try
            Response.End()
        End If
    End Sub

End Class
