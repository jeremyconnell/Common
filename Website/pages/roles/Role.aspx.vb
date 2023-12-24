Partial Class pages_Roles_Role : Inherits CPage

#Region "Members"
    Private m_role As CRole
#End Region

#Region "Querystring"
    Public ReadOnly Property RoleName() As String
        Get
            Return CWeb.RequestStr("roleName")
        End Get
    End Property
    Public ReadOnly Property IsEdit() As Boolean
        Get
            Return RoleName.Length > 0
        End Get
    End Property
    Public ReadOnly Property Search() As String
        Get
            Return CWeb.RequestStr("search")
        End Get
    End Property
#End Region

#Region "Data"
    Public ReadOnly Property [Role]() As CRole
        Get
            If IsNothing(m_role) Then
                If IsEdit Then
                    m_role = CRole.Cache.GetById(RoleName)
                    If IsNothing(m_role) Then CSitemap.RecordNotFound("Role", RoleName)
                Else
                    m_role = New CRole
                    'Inserts: set parentId here (if applicable)
                End If
            End If
            Return m_role
        End Get
    End Property
#End Region

#Region "Navigation"
    Private Sub Refresh()
        Response.Redirect(CSitemap.RoleEdit(Me.Role.RoleName))
    End Sub
    Private Sub ReturnToList()
        Response.Redirect(CSitemap.Roles)
    End Sub
#End Region

#Region "Event Handlers - Form"
    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not Me.IsValid() Then Exit Sub
        PageSave()
        Refresh()
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ReturnToList()
    End Sub
    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Me.Role.Delete()
            'CCache.ClearCache()
        ReturnToList()        
    End Sub
#End Region

#Region "Overrides"
    Protected Overrides Sub PageInit()
        'Page title
        If IsEdit Then            
            Me.Title = "Role Details"
        Else
            Me.Title = "Add A New Role"
        End If

        'Buttons
        btnDelete.Visible = IsEdit
        If IsEdit Then btnCancel.Text = "Back" Else btnSave.Text = "Create Role"
        If IsEdit Then AddButton(CSitemap.RoleAdd(), "Create a new Role")

        'Data
        ctrlUsers.Display(Role, Search)
    End Sub
    Protected Overrides Sub PageLoad()
        With Me.Role
                txtRoleName.Text = .RoleName
        End With
    End Sub
    Protected Overrides Sub PageSave()
        With Me.Role
            .RoleName = txtRoleName.Text

            .Save()
        End With
            'CCache.ClearCache()

    End Sub
#End Region

End Class
