Partial Public Class pages_UserRoles_usercontrols_UCUserRoles : Inherits UserControl

#Region "Constants"
    Public Const REMAINING_TEXT As String = "Remaining"
    Public Const SELECTED_TEXT  As String = "Selected"
#End Region

#Region "Interface for Roles page"
    'Remaining Users
    Public Sub DisplayRemaining(ByVal role As CRole, ByVal autoPostback As Boolean, ByVal search As String)
        litSelected.Text = REMAINING_TEXT
        litCount.Text = New CUser().SelectCount().ToString()
        colDate.Visible = False

        Dim page As CUserList = role.RemainingUsers(PagingInfo, search)
        For Each i As CUser In page
            Dim index As Integer = page.IndexOf(i) + 1 + PagingInfo.Offset
            UCUserRole(plh).Display(i, role.RoleName, Nothing, autoPostback, index)
        Next
    End Sub
    'Selected Users
    Public Sub DisplaySelected(ByVal role As CRole, ByVal autoPostback As Boolean)
        litSelected.Text = SELECTED_TEXT
        litCount.Text = role.UserRolesCount().ToString()
        Dim page As CUserRoleList = role.UserRoles(PagingInfo)
        For Each i As CUserRole in page
            Dim number As Integer = page.IndexOf(i) + 1 + PagingInfo.Offset
            UCUserRole(plh).Display(i.User, i.URRoleName, i, autoPostback, number)
        Next
    End Sub
#End Region

#Region "Interface for Users page"
    'Remaining Roles
    Public Sub DisplayRemaining(ByVal user As CUser, ByVal autoPostback As Boolean, ByVal search As String)
        litSelected.Text = REMAINING_TEXT
        litCount.Text = CRole.Cache.Count.ToString()
        colDate.Visible = False

        Dim filtered As CRoleList = user.UserRoles.RemainingRoles(search)
        For Each i As CRole In ctrlPaging.Display(filtered)
            UCUserRole(plh).Display(i, user.UserLoginName, Nothing, autoPostback, filtered.IndexOf(i) + 1)
        Next
    End Sub
    'Selected Roles
    Public Sub DisplaySelected(ByVal user As CUser, ByVal autoPostback As Boolean)
        litSelected.Text = SELECTED_TEXT
        Dim selected As CUserRoleList = user.UserRoles
        litCount.Text = selected.Count.ToString()
        For Each i As CUserRole In ctrlPaging.Display(selected)
            UCUserRole(plh).Display(i.Role, i.URUserLogin, i, autoPostback, selected.IndexOf(i) + 1)
        Next
    End Sub
#End Region

#Region "User Controls"
    Private Function UCUserRole(ByVal target As Control) As pages_UserRoles_usercontrols_UCUserRole
        Dim ctrl As pages_UserRoles_usercontrols_UCUserRole = Page.LoadControl(CSitemap.UCUserRole)
        target.Controls.Add(ctrl)
        Return ctrl
    End Function
#End Region

#Region "Appearance"
    Public Property Title() As String
        Get
            Return litTitle.Text
        End Get
        Set(ByVal value As String)
            litTitle.Text = value
        End Set
    End Property
#End Region

#Region "Behaviour"
    Public WriteOnly Property Enabled() As Boolean
        Set(ByVal value As Boolean)
            For Each i As pages_UserRoles_usercontrols_UCUserRole In plh.Controls
                i.Enabled = value
            Next
        End Set
    End Property
#End Region

#Region "Paging"
    Public ReadOnly Property PagingInfo() As CPagingInfo
        Get
            Return ctrlPaging.Info
        End Get
    End Property
    Public Property QueryString() As String
        Get
            Return ctrlPaging.QueryString
        End Get
        Set(ByVal value As String)
            ctrlPaging.QueryString = value
        End Set
    End Property
    Public Property PageSize() As Integer
        Get
            Return ctrlPaging.PageSize
        End Get
        Set(ByVal value As Integer)
            ctrlPaging.PageSize = value
        End Set
    End Property
    Public WriteOnly Property FriendlyUrl() As String
        Set(ByVal value As String)
            ctrlPaging.FriendlyUrl = String.Concat(value, IIf(value.Contains("?"), "&", "?"), QueryString, "={0}")
        End Set
    End Property
#End Region

End Class