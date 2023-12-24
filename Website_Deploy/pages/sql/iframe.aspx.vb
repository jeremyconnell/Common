
Imports System.Threading.Tasks
Imports SchemaDeploy

Partial Class pages_sql_iframe
	Inherits CPageWithTableHelpers



	Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
		If Not String.IsNullOrEmpty(CSession.TextForIframe) Then
			Response.ContentType = "text/plain"
			Response.Write(CSession.TextForIframe)
			CSession.TextForIframe = Nothing
			Response.End()
		End If


		Dim sql As New CAudit_Sql

		Dim db As CDataSrcLocal = CDataSrcLocal.Default
		If Not String.IsNullOrEmpty(CSession.SqlUseConn) Then
			db = New CSqlClient(CSession.SqlUseConn)
			sql.SqlConnectionString = CSession.SqlUseConn
		End If

		If CSession.SqlRunOnAllInstancesOfAppId > 0 Then   'All clients
			Dim dict As New Dictionary(Of String, String)

			Dim cc As HttpContext = HttpContext.Current
			Parallel.ForEach(CInstance.Cache.GetByAppId(CSession.SqlRunOnAllInstancesOfAppId),
					Sub(i As CInstance)
						HttpContext.Current = cc

						Dim key As String = i.IdAndName
						If IsNothing(i.Database) Then
							SyncLock (dict)
								dict.Add(key, Nothing)
							End SyncLock
							Exit Sub
						Else
							Try
								sql.SqlIsUpdate = Not CSession.SqlIsSelect
								sql.SqlConnectionString = i.Database.ConnectionString
								sql.SqlText = CSession.SqlStatement
								sql.Save()
								sql = New CAudit_Sql

								If CSession.SqlIsSelect Then
									Dim scalar As Object = i.Database.ExecuteScalar(CSession.SqlStatement)
									SyncLock (dict)
										dict.Add(key, String.Concat("Scalar: ", scalar))
									End SyncLock
								Else
									Dim rowsAffected As Integer = i.Database.ExecuteNonQuery(CSession.SqlStatement)
									SyncLock (dict)
										dict.Add(key, String.Concat(rowsAffected, " rows affected"))
									End SyncLock
								End If
							Catch ex As Exception
								SyncLock (dict)
									dict.Add(key, ex.Message)
								End SyncLock
							End Try
						End If
					End Sub)


			Display(dict)
			CSession.SqlRunOnAllInstancesOfAppId = Integer.MinValue
			Exit Sub
		End If


		If CSession.SqlIsSelect Then
			Try
				sql.SqlText = CSession.SqlStatement
				sql.Save()

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
			sql.SqlIsUpdate = Not CSession.SqlIsSelect
			sql.SqlText = CSession.SqlStatement
			sql.Save()

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
	Private Sub Display(dict As Dictionary(Of String, String))
		dg.Visible = False

		Dim tbl As New Table
		tbl.CssClass = "datagrid"
		plh.Controls.Add(tbl)

		Dim tr As TableHeaderRow = RowH(tbl)
		CellH(tr, "Client")
		CellH(tr, "Result")
		For Each i As String In dict.Keys
			Display(tbl, i, dict(i))
		Next
	End Sub
	Private Sub Display(tbl As Table, client As String, result As String)
		Dim tr As TableRow = Row(tbl)
		CellH(tr, client)
		Cell(tr, result)
	End Sub

End Class
