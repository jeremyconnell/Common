Public Partial Class pages_UserRoles_usercontrols_UCUserRole : Inherits UserControl

#Region "Constants"
    Public Const CSS_CHECKBOX_CHANGED As String = "checkboxchanged"
    Private Shared JSCRIPT As String = String.Concat("this.className=(this.checked==this.defaultChecked?'':'", CSS_CHECKBOX_CHANGED, "')")
#End Region

#Region "Members"
    Private m_userLoginName As String
    Private m_roleName As String
    Private m_isRolePage As Boolean
#End Region

#Region "Behaviour"
    Public Property Enabled() As Boolean
        Get
            Return chk.Enabled
        End Get
        Set(ByVal value As Boolean)
            chk.Enabled = value
        End Set
    End Property
    Public Property AutoPostBack() As Boolean
        Get
            Return chk.AutoPostBack
        End Get
        Set(ByVal value As Boolean)
            chk.AutoPostBack = value
        End Set
    End Property
#End Region

#Region "Interfaces"
    'User-orientated, for Role page
    Public Sub Display(ByVal user As CUser, ByVal roleName As String, ByVal assnOrNull As CUserRole, ByVal autoPostback As Boolean, ByVal number As Integer)
        If Parent.Controls.Count Mod 2 = 0 Then row.Attributes.Add("class", "alt_row")

        m_userLoginName = user.UserLoginName
        m_roleName = roleName
        m_isRolePage = True

        lnkTarget.Text = user.UserFullName
        lnkTarget.NavigateUrl = CSitemap.UserEdit(user.UserLoginName)

        litNum.Text = number.ToString()
        
        If Not IsNothing(assnOrNull) Then 'Selected vs Remaining
            chk.Checked = True
            chk.ToolTip = "Uncheck to remove User from this Role"

            'colDateCreated.Visible = True
            'lblDateCreated.Text    = CUtilities.LongDate(    assnOrNull.XYDateCreated)
            'lblDateCreated.ToolTip = CUtilities.LongDateTime(assnOrNull.XYDateCreated)
        Else
            chk.ToolTip = "Check to Add User to this Role"
        End If
    End Sub
    'Role-orientated, for User page
    Public Sub Display(ByVal role As CRole, ByVal userLoginName As String, ByVal assnOrNull As CUserRole, ByVal autoPostback As Boolean, ByVal number As Integer)
        If Parent.Controls.Count Mod 2 = 0 Then row.Attributes.Add("class", "alt_row")

        m_userLoginName = userLoginName
        m_roleName = role.RoleName
        m_isRolePage = False

        lnkTarget.Text = role.RoleName
        lnkTarget.NavigateUrl = CSitemap.RoleEdit(role.RoleName)

        litNum.Text = number.ToString()
        
        If Not IsNothing(assnOrNull) Then 'Selected vs Remaining
            chk.Checked = True
            chk.ToolTip = "Uncheck to remove Role from this User"

            'colDateCreated.Visible = True
            'lblDateCreated.Text    = CUtilities.LongDate(    assnOrNull.XYDateCreated)
            'lblDateCreated.ToolTip = CUtilities.LongDateTime(assnOrNull.XYDateCreated)
        Else
            chk.ToolTip = "Check to Add Role to this User"
        End If
    End Sub
#End Region

#Region "Events"
    Private Sub chk_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chk.CheckedChanged
        If chk.Checked Then
            CUserRole.InsertPair(m_userLoginName, m_roleName)
        Else
            CUserRole.DeletePair(m_userLoginName, m_roleName)
        End If
        Refresh()
    End Sub
    Private Sub Refresh()
        If Not AutoPostBack Then Exit Sub
        Response.Redirect(Request.RawUrl)
    End Sub
#End Region

End Class