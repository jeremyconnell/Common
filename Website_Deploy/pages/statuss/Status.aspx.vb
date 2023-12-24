Partial Class pages_Statuss_Status : Inherits CPage

#Region "Querystring"
    Public ReadOnly Property StatusId() As Integer
        Get
            Return CWeb.RequestInt("statusId")
        End Get
    End Property
    Public ReadOnly Property IsEdit() As Boolean
        Get
            Return StatusId <> Integer.MinValue
        End Get
    End Property
#End Region

#Region "Members"
    Private m_status As CStatus
#End Region

#Region "Data"
    Public ReadOnly Property [Status]() As CStatus
        Get
            If IsNothing(m_status) Then
                If IsEdit Then
                    m_status = CStatus.Cache.GetById(StatusId)
                    If IsNothing(m_status) Then CSitemap.RecordNotFound("Status", StatusId)
                Else
                    m_status = New CStatus
                    'Inserts: set parentId here (if applicable)
                End If
            End If
            Return m_status
        End Get
    End Property
#End Region

#Region "Navigation"
    Private Sub Refresh()
        Response.Redirect(CSitemap.StatusEdit(Me.Status.StatusId))
    End Sub
    Private Sub ReturnToList()
        Response.Redirect(CSitemap.Statuss)
    End Sub
#End Region

#Region "Event Handlers - Page"
    Protected Overrides Sub PageInit()

        'Page Title
        If IsEdit Then            
            Me.Title = "Status Details"
        Else
            Me.Title = "Create New Status"
        End If
                
        'Textbox logic
        'Me.Form.DefaultFocus = txtStatusName.Textbox.ClientID;
        Me.Form.DefaultButton = btnSave.UniqueID   'txtStatusName.OnReturnPress(btnSave)

        'Button Text
        btnDelete.Visible = IsEdit
        If IsEdit Then btnCancel.Text = "Back" Else btnSave.Text = "Create Status"
        If IsEdit Then
			AddButton(CSitemap.StatusAdd(), "Create a new Status")
		End If


        MenuSelected = "Clients"
        AddMenuSide(CUtilities.NameAndCount("Clients", CClient.Cache), CSitemap.Clients())
        If StatusId > 0 Then
            AddMenuSide(CUtilities.NameAndCount(Status.StatusName, Status.Clients), CSitemap.Clients("", StatusId))
        End If
		AddMenuSide("Status Details")
		AddMenuSide("New Status...", CSitemap.StatusAdd)

	End Sub
    Protected Overrides Sub PageLoad()
        If Page.IsPostback Then Exit Sub

        LoadStatus()
    End Sub
#End Region

#Region "Event Handlers - Form"
    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not Me.IsValid() Then Exit Sub
        SaveStatus()
        Refresh()
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ReturnToList()
    End Sub
    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Me.Status.Delete()
            'CCache.ClearCache()
        ReturnToList()        
    End Sub
#End Region

#Region "Private - Load/Save"
    Protected Sub LoadStatus()
        With Me.Status
            txtStatusName.Text = .StatusName
            txtStatusDescription.Text = .StatusDescription
        End With
    End Sub
    Protected Sub SaveStatus()
        With Me.Status
            .StatusName = txtStatusName.Text
            .StatusDescription = txtStatusDescription.Text

            .Save()
        End With
            'CCache.ClearCache()
    End Sub
#End Region

End Class
