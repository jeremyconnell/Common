Partial Public Class pages_users_usercontrols_UCUser : Inherits UserControl

#Region "Members"
    Private m_user As CUser
    Private m_list As CUserList
    Private m_pageIndex As Integer
#End Region

#Region "Interface"
    Public Sub Display(ByVal [user] As CUser, list as CUserList, pi As CPagingInfo)
        If Parent.Controls.Count Mod 2 = 0 Then row.Attributes.Add("class", "alt_row")

        m_user = [user]
        m_list = list
        m_pageIndex = pi.PageIndex

        With m_user
            litNumber.Text = CStr(list.IndexOf(m_user) + 1 + pi.PageIndex * pi.PageSize)
            lnkUserLoginName.Text =  CStr(IIF(.UserLoginName.Length = 0, "...", .UserLoginName))
            lnkUserLoginName.NavigateUrl = CSitemap.UserEdit(.UserLoginName)
            litUserFirstName.Text = .UserFirstName
            litUserLastName.Text = .UserLastName
            lnkUserEmail.Text = .UserEmail
            lnkUserEmail.NavigateUrl = "mailto: " + .UserEmail
            chkUserIsDisabled.Checked = .UserIsDisabled
            chkUserIsLockedOut.Checked = .UserIsLockedOut

            litRoles.Text = .RoleNames.Replace(",", ", ")

            litLastLogin.Text = CUtilities.LongDateTime(.UserLastLoginDate)
        End With
    End Sub
#End Region

#Region "Event Handlers"
    Private Sub chkUserIsDisabled_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkUserIsDisabled.CheckedChanged
        With m_user
            .UserIsDisabled = chkUserIsDisabled.Checked
            .Save()
        End With
        Refresh()
    End Sub
    Private Sub chkUserIsLockedOut_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkUserIsLockedOut.CheckedChanged
        With m_user
            .UserIsLockedOut = chkUserIsLockedOut.Checked
            .Save()
        End With
        Refresh()
    End Sub
    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnDelete.Click
        m_user.Delete()
        Refresh()
    End Sub
#End Region

#Region "Private"
    Private Sub Refresh()
        'CCache.ClearCache()

        'If False Then 'Request.RawUrl.ToLower.Contains("myparent.aspx") Then
        '    'Special case: Parent entity owns the list
        '    Response.Redirect(CSitemap.MyParentEdit(m_user.UserParentId, MyParent.ETab.users, m_pageIndex)
        'Else
        '    'Normal case: Search page owns the list
            Response.Redirect(Request.RawUrl) 'includes paging info
        'End If 
    End Sub
#End Region

End Class
