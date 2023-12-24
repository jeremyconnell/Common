Partial Class pages_roles_usercontrols_UCRole : Inherits UserControl

#Region "Members"
    Private m_role As CRole
    Private m_list As CRoleList
    Private m_pageIndex As Integer
#End Region

#Region "Interface"
    Public Sub Display(ByVal [role] As CRole, list as CRoleList, pi As CPagingInfo)
        If Parent.Controls.Count Mod 2 = 0 Then row.Attributes.Add("class", "alt_row")

        m_role = [role]
        m_list = list
        m_pageIndex = pi.PageIndex

        With m_role
            litNumber.Text = CStr(list.IndexOf(m_role) + 1)
            lnkRoleName.Text =  CStr(IIF(.RoleName.Length = 0, "...", .RoleName))
            lnkRoleName.NavigateUrl = CSitemap.RoleEdit(.RoleName)
            litUserCount.Text = CUtilities.CountSummary(.UserRolesCount(), "user")
        End With
    End Sub
#End Region

#Region "Event Handlers"
    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnDelete.Click
        m_role.Delete()
        Refresh()
    End Sub
#End Region

#Region "Private"
    Private Sub Refresh()
        'CCache.ClearCache()

        'If False Then 'Request.RawUrl.ToLower.Contains("myparent.aspx") Then
        '    'Special case: Parent entity owns the list
        '    Response.Redirect(CSitemap.MyParentEdit(m_role.RoleParentId, MyParent.ETab.roles, m_pageIndex)
        'Else
        '    'Normal case: Search page owns the list
            Response.Redirect(Request.RawUrl) 'includes paging info
        'End If 
    End Sub
#End Region

End Class
