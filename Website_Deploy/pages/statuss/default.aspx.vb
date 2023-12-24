Partial Public Class pages_Statuss_default : Inherits CPage

#region "Querystring (Filters)"
    Public ReadOnly Property Search As String
        Get
            Return CWeb.RequestStr("search")
        End Get
    End Property
    
    'Rename or Delete:
    'Public ReadOnly Property ParentId As Integer
    '    Get
    '        Return CWeb.RequestStr("parentId")
    '    End Get
    'End Property
#End Region

#Region "Data"
    Public ReadOnly Property [Statuss]() As CStatusList
        Get
            Return CStatus.Cache.Search(txtSearch.Text)
        End Get
    End Property
#End Region

#Region "Event Handlers - Page"
    Protected Overrides Sub PageInit()
        'Populate Dropdowns
        
    
        'Search state (from querystring)
        txtSearch.Text = Me.Search
        
        'Display Results
        ctrlStatuss.Display(Me.Statuss)

        'Client-side
        Me.Form.DefaultFocus  = txtSearch.ClientID  'txtCreate.ClientID
        Me.Form.DefaultButton = btnSearch.UniqueID  'CTextbox.OnReturnPress(txtSearch, btnSearch)

        MenuSelected = "Clients"
        AddMenuSide(CUtilities.NameAndCount("Clients", CClient.Cache), CSitemap.Clients())
        AddMenuSide(CUtilities.NameAndCount("States", CStatus.Cache))
		AddMenuSide("+New Status", CSitemap.StatusAdd())
	End Sub
#End Region

#Region "Event Handlers - Form"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Response.Redirect(CSitemap.Statuss(txtSearch.Text))
    End Sub
    Protected Sub btnCreate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreate.Click
        Dim obj As New CStatus()
        obj.StatusName = txtSearch.Text
        obj.Save()
        Response.Redirect(Request.RawUrl)
    End Sub
#End Region

#Region "Event Handlers - UserControl"
    Private Sub ctrl_AddClick() Handles ctrlStatuss.AddClick
        Response.Redirect(CSitemap.StatusAdd())
    End Sub
    Private Sub ctrl_ResortClick(ByVal sortBy As String, ByVal descending As Boolean, ByVal pageNumber As Integer) Handles ctrlStatuss.ResortClick
        Response.Redirect(CSitemap.Statuss(txtSearch.Text, New CPagingInfo(0, pageNumber - 1, sortBy, descending)))
    End Sub
#End Region

End Class
