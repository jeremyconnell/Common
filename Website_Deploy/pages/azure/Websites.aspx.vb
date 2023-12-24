Imports System.Collections.Generic
Imports System.Data
Imports System.Threading
Imports Comms.PushUpgrade.Client
Imports Comms.Upgrade.Client
Imports Comms.Upgrade.Interface
Imports Comms.Upgrade.Server
Imports Microsoft.WindowsAzure.Management.WebSites.Models
Imports Microsoft.WindowsAzure.Management.WebSites.Models.WebSpacesListResponse
Imports SchemaAdmin.api
Imports SchemaDeploy

Partial Class pages_azure_Websites : Inherits CPageWithTableHelpers

#Region "Page Events"
	Protected Overrides Sub PageInit()
		AddMenuSide("Websites")
		AddMenuSide("Databases", CSitemap.AzureDbs)

		Websites()
	End Sub
#End Region


#Region "Form Events"
	Private Sub btnRestartAll_Click(sender As Object, e As EventArgs) Handles btnRestartAll.Click
		Dim c As HttpContext = HttpContext.Current
		Dim sb As New StringBuilder()
		Tasks.Parallel.ForEach(CAzureManagement.Web.WebSites_Cached,
			Sub(w)
				HttpContext.Current = c

				If w.Name.ToLower.EndsWith("admin") Then
					SyncLock (sb)
						sb.Append("Skipped: ").AppendLine(w.Name)
					End SyncLock
					Exit Sub
				End If

				Try
					CAzureManagement.Web.Restart(w.Name, w.WebSpace)

					SyncLock (sb)
						sb.Append("Restarted: ").AppendLine(w.Name)
					End SyncLock
				Catch ex As Exception
					SyncLock (sb)
						sb.Append("Failed: ").AppendLine(w.Name)
					End SyncLock
				End Try
			End Sub)


		CSession.PageMessage = sb.ToString()
		Response.Redirect(Request.RawUrl)
	End Sub
#End Region


	Private Sub Websites()

		Dim space As WebSpace = CAzureManagement.Web.WebSpace1
		If IsNothing(space) Then Exit Sub



		Dim temp As New CInstanceList(CInstance.Cache)
		Dim miss As New List(Of String)
		Dim num As Integer = 1
		For Each i As WebSite In CAzureManagement.Web.WebSites_Cached()
			Dim code As String = CUtilities.Truncate(i.Name.ToLower.Replace("controltrack", "").ToLower, 10)
			Dim n As String = "controltrack" & code.ToUpper

			Dim ins As CInstance = CInstance.Cache.GetByWebNameAzure_(i.Name)

			Dim host As String = i.HostNames(0)
			If IsNothing(ins) Then
				ins = CInstance.Cache.GetByHost(host)
			End If

			num = num + 1

			Dim s As String = String.Empty
			For Each j In i.HostNameSslStates
				If Not j.SslState.HasValue Then Continue For
				If j.SslState.Value = WebSiteSslState.Disabled Then Continue For
				s = String.Concat(s, "https://" & j.Name)
			Next


			'Database #, Name
			Dim tr As TableRow = Row(tblWebApps)
			CellH(tr, num & ".")

			Dim th As TableCell = CellLink(tr, i.Name, s, False)
			th.Width = New Unit("150")

			Dim lnk1 As HyperLink = th.Controls(0)
			lnk1.ToolTip = CUtilities.ListToString(i.HostNames, vbCrLf)
			lnk1.Target = "_blank"



			'Match with client
			If Not IsNothing(ins) Then
				temp.Remove(ins)
				Cell(tr, code.ToUpper, ins.IdAndName, True)
				CellLink(tr, ins.IdAndName, CSitemap.InstanceWebsite(ins.InstanceId))
				Cell(tr, ins.InstanceSuffix)
			ElseIf code = "admin" Then
				Cell(tr, "*" & code.ToLower & "*")
				Cell(tr, "*SELF*")
				Cell(tr)
			ElseIf code = "saas" Then
				Cell(tr, "*" & code.ToLower & "*")
				Cell(tr, "*SYSTEM*")
				Cell(tr)
			Else
				miss.Add(code)
				Cell(tr, code.ToUpper, code.ToUpper, True).ForeColor = Drawing.Color.Red
				Cell(tr)
				Cell(tr)
				lnk1.ForeColor = Drawing.Color.Red
			End If


			'State
			Cell(tr, i.State.ToString)

			'AvailabilityState
			If i.AvailabilityState.HasValue Then
				Cell(tr, i.AvailabilityState.Value.ToString)
			Else
				Cell(tr)
			End If

			'UsageState
			If i.UsageState.HasValue Then
				Cell(tr, i.UsageState.Value.ToString)
			Else
				Cell(tr)
			End If


			'Cell(tr, i.ServerFarm) 'AUS_SE_S1
			lblWebSpaceName.Text = i.ServerFarm



			Dim sb As New StringBuilder()
			Try
				For Each j In CAzureManagement.Web.PublishProfile_Cached(i.Name)
					If j.SqlServerConnectionString.Length = 0 Then Continue For
					sb.AppendLine(j.SqlServerConnectionString)
					sb.Append(j.Databases(0).Name).Append("\t").Append(j.SqlServerConnectionString)
				Next

				'For Each u As WebSiteGetUsageMetricsResponse.UsageMetric In CAzureManagement.Web.UsageMetrics(i.Name)
				'	sb.Append(u.Name).Append("\t").Append(u.CurrentValue).AppendLine("<Br")
				'Next
				Dim lnk As HyperLink = tr.Cells(1).Controls(0)
				lnk.ToolTip = sb.ToString
				'Cell(tr, sb.ToString)
			Catch ex As Exception
				Cell(tr, ex.Message)
			End Try

			'Dim h = CAzureManagement.Web.HistoricalUsageMetrics(i.Name)

			Dim td = Cell(tr)
			td.Style.Add("padding", "0px 0px")
			Dim btnDel = New ImageButton()
			btnDel.ID = "btndel" & i.Name
			btnDel.CommandArgument = i.Name
			btnDel.ImageUrl = "~/images/delete.png"
			btnDel.OnClientClick = "return confirm('Delete WebApp: " + i.Name + "?');"
			AddHandler btnDel.Click, AddressOf btnDel_Click
			td.Controls.Add(btnDel)
		Next


		Dim fr As TableRow = Row(tblWebApps)
		CellH(fr, "#")
		Cell(fr, CUtilities.ListToHtml(miss), Nothing, True).ForeColor = Drawing.Color.Red
		Cell(fr)
		Dim tdc As TableCell = Cell(fr)
		For Each i As CInstance In temp
			Dim p As New Panel
			tdc.Controls.Add(p)

			Dim lnkC As New HyperLink
			p.Controls.Add(lnkC)
			lnkC.ForeColor = Drawing.Color.Red
			lnkC.Text = CUtilities.Truncate(i.IdAndName)
			lnkC.NavigateUrl = CSitemap.Instance(i.InstanceId)
			lnkC.Font.Bold = True
		Next
		Cell(fr).ColumnSpan = 5
	End Sub



	Public Sub btnDel_Click(sender As Object, e As EventArgs)
		Dim btn As ImageButton = sender
		Dim name As String = btn.CommandArgument
		Try
			If name.ToLower() = "controltracksaas" Then Exit Sub
			CAzureManagement.Web.Delete(name)
			CSession.PageMessage = "Deleted WebApp: " & name
			CAzureManagement.Web.WebSites_ClearCache()
		Catch ex As Exception
			CSession.PageMessage = "Delete Failed: " + ex.Message
		End Try
		Response.Redirect(Request.RawUrl)
	End Sub
End Class