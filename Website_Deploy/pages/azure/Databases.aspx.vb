Imports System.Collections.Generic
Imports System.Data
Imports System.Threading
Imports Comms.PushUpgrade.Client
Imports Comms.Upgrade.Client
Imports Comms.Upgrade.Interface
Imports Comms.Upgrade.Server
Imports Microsoft.WindowsAzure.Management.Sql.Models
Imports SchemaAdmin.api
Imports SchemaDeploy

Partial Class pages_azure_Databases : Inherits CPageWithTableHelpers

#Region "Querystring"
	Public ReadOnly Property DatabaseName As String
		Get
			Return CWeb.RequestStr("database")
		End Get
	End Property
#End Region

#Region "Page Events"
	Protected Overrides Sub PageInit()
		If "*" = DatabaseName Then CAzureManagement.Sql.PreloadUsageN()


		Databases()
	End Sub


	Protected Overrides Sub PagePreRender()
		AddMenuSide("Websites", CSitemap.AzureWebs)
		AddMenuSide("Databases")

	End Sub

#End Region

	Private Sub Databases()
		lblServerName.Text = CAzureManagement.Sql.SERVER_NAME

		Dim total As Long = 0

		Dim temp As New CInstanceList(CInstance.Cache)
		Dim miss As New List(Of String)
		For Each i As Database In CAzureManagement.Sql.List_Cached
			Dim code As String = CUtilities.Truncate(i.Name.ToLower.Replace("controltrack", "").ToLower, 10)
			Dim n As String = "controltrack" & code.ToUpper

			Dim ins As CInstance = CInstance.Cache.GetByDbNameAzure_(i.Name)

			Dim tr As TableRow = Row(tblDatabases)

			'Database Id
			CellH(tr, i.Id.ToString)

			'Database Name, link

			Dim isSelected As Boolean = DatabaseName = i.Name OrElse DatabaseName = "*"


			Dim url As String = CSitemap.InstanceAdd(EApp.ControlTrack)
			If Not IsNothing(ins) Then
				url = CSitemap.InstanceDatabase(ins.InstanceId)
			Else
				Dim c As CClient = CClient.Cache.GetByCode(code)
				If Not IsNothing(c) Then url = CSitemap.InstancesByClient(c.ClientId)
			End If
			Dim th As TableCell = CellLink(tr, n, url, isSelected)
			th.Width = New Unit("250")
			Dim lnk1 As HyperLink = th.Controls(0)



			'Match with client
			If Not IsNothing(ins) Then
				temp.Remove(ins)
				Cell(tr, code.ToUpper, ins.IdAndName, True)
				CellLink(tr, ins.IdAndName, url)
			ElseIf code = "admin" Then
				Cell(tr, "*" & code.ToLower & "*")
				Cell(tr, "*SELF*")
			ElseIf i.Name = "master" Then
				Cell(tr, "*" & code.ToLower & "*")
				Cell(tr, "*SYSTEM*")
			Else
				miss.Add(code)
				Cell(tr, CUtilities.Truncate(code.ToUpper, 10), code.ToUpper, True).ForeColor = Drawing.Color.Red
				Cell(tr)
				lnk1.ForeColor = Drawing.Color.Red
			End If

			'DB-Size
			Dim usage As List(Of DatabaseUsageMetric) = CAzureManagement.Sql.Usage_Cached(i.Name, isSelected)
			If IsNothing(usage) Then
				CellLink(tr, "load", CSitemap.AzureDbs(i.Name))
			Else
				Dim size As Long = Long.Parse(usage(0).CurrentValue)
				Dim fileSize As String = CUtilities.FileSize(size)
				total += size
				CellR(tr, fileSize)
			End If

			'Created
			Cell(tr, CUtilities.Timespan(i.CreationDate).Replace("ago", ""))


			Dim td = Cell(tr)
			td.Style.Add("padding", "0px 0px")

			Dim btnDel = New ImageButton()
			btnDel.ID = "btndel" & i.Name
			btnDel.CommandArgument = i.Name
			btnDel.ImageUrl = "~/images/delete.png"
			btnDel.OnClientClick = "return confirm('Delete database: " + i.Name + "?');"
			AddHandler btnDel.Click, AddressOf btnDel_Click
			td.Controls.Add(btnDel)
		Next

		Dim urlAll As String = CSitemap.AzureDbs("*")

		Dim fr As TableRow = Row(tblDatabases)
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
			lnkC.NavigateUrl = CSitemap.InstanceDatabase(i.InstanceId)
		Next
		CellLink(fr, IIf(0 = total, "ALL", CUtilities.FileSize(total)), urlAll)
		Cell(fr)
		Cell(fr)
	End Sub


	Public Sub btnDel_Click(sender As Object, e As EventArgs)
		Dim btn As ImageButton = sender
		Dim name As String = btn.CommandArgument
		Try
			If name.ToLower() = "controltracksaas" Then Exit Sub
			CAzureManagement.Sql.Delete(name)
			CSession.PageMessage = "Deleted Database: " & name
			CAzureManagement.Sql.List_ClearCache()

		Catch ex As Exception
			CSession.PageMessage = "Delete Failed: " + ex.Message
		End Try
		Response.Redirect(Request.RawUrl)
	End Sub
End Class