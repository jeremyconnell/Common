Partial Class pages_UserRoles_usercontrols_UCUserRolesPair : Inherits UserControl

#Region "Members"
    Private m_autopostback As Boolean = True
    Private m_user As CUser
    Private m_role As CRole
#End Region

#Region "Main Interface (Parent Entity)"
    'Friendly Urls recommended for paging within tabs
    Public Sub Display(ByVal user As CUser, ByVal searchStateFromQuerystring As String, ByVal friendlyUrl As String)
        Display(user, searchStateFromQuerystring)
        Me.FriendlyUrl = friendlyUrl
    End Sub
    Public Sub Display(ByVal role As CRole, ByVal searchStateFromQuerystring As String, ByVal friendlyUrl As String)
        Display(role, searchStateFromQuerystring)
        Me.FriendlyUrl = friendlyUrl
    End Sub

    'Simple Interface
    Public Sub Display(ByVal user As CUser, ByVal searchState As String)
        If Page.IsPostBack Then searchState = CWeb.RequestStr(txtSearch.UniqueID)
        m_user = user
        txtSearch.Text = searchState
        If Title.Length = 0 Then Title = "Roles"
        ctrlRemaining.DisplayRemaining(user, AutoPostback, searchState)
        ctrlSelected.DisplaySelected(user, AutoPostback)
    End Sub
    Public Sub Display(ByVal role As CRole, ByVal searchState As String)
        If Page.IsPostBack Then searchState = CWeb.RequestStr(txtSearch.UniqueID) 'txtSearch - postback value still not set during Page_Init
        m_role = role
        If Title.Length = 0 Then Title = "Users"
        ctrlRemaining.DisplayRemaining(role, AutoPostback, searchState)
        ctrlSelected.DisplaySelected(role, AutoPostback)
    End Sub
#End Region

#Region "Event Handlers"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        If Not IsNothing(m_role) Then
            Response.Redirect(CSitemap.RoleEdit(m_role.RoleName, txtSearch.Text)) 'txtSearch - postback value still not set during Page_Init
        Else
            Response.Redirect(CSitemap.UserEdit(m_user.UserLoginName, txtSearch.Text)) 'txtSearch - postback value still not set during Page_Init
        End If
    End Sub
#End Region

#Region "Appearance"
    Public Property Title() As String
        Get
            Return ctrlRemaining.Title
        End Get
        Set(ByVal value As String)
            ctrlRemaining.Title = value
            ctrlSelected.Title = value
        End Set
    End Property
#End Region

#Region "Behaviour"
    Public WriteOnly Property Enabled() As Boolean
        Set(ByVal value As Boolean)
            ctrlRemaining.Enabled = value
            ctrlSelected.Enabled = value
        End Set
    End Property
    Public Property AutoPostback() As Boolean
        Get
            Return m_autopostback
        End Get
        Set(ByVal value As Boolean)
            m_autopostback = value
        End Set
    End Property
#End Region

#Region "Paging"
    Public Property QueryString() As String
        Get
            Return ctrlRemaining.QueryString.Substring(0, ctrlRemaining.QueryString.Length - 1)
        End Get
        Set(ByVal value As String)
            ctrlRemaining.QueryString = value & "1"
            ctrlSelected.QueryString = value & "2"
        End Set
    End Property
    Public Property PageSize() As Integer
        Get
            Return ctrlRemaining.PageSize
        End Get
        Set(ByVal value As Integer)
            ctrlRemaining.PageSize = value
            ctrlSelected.PageSize = value
        End Set
    End Property
    Public WriteOnly Property FriendlyUrl() As String
        Set(ByVal value As String)
            ctrlRemaining.FriendlyUrl = value
            ctrlSelected.FriendlyUrl = value
        End Set
    End Property
#End Region

End Class