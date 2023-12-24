
Partial Class usercontrols_config_settings_UCGroup
    Inherits System.Web.UI.UserControl

#Region "Members"
    Private m_group As CGroup
    Private m_list As CGroupList
#End Region

#Region "Interface"
    Public Sub Display(ByVal g As CGroup, ByVal list As CGroupList)
        m_group = g
        m_list = list
        With g
            litGroupName.Text = .GroupName

            lnk.NavigateUrl = CSitemap.GroupEdit(.GroupId)

            For Each i As CSetting In .Settings
                UCSetting(plh).Display(i, .Settings)
            Next

            lnkAdd.NavigateUrl = CSitemap.SettingAdd(.GroupId)
        End With
    End Sub
    Public Function Store() As CSettingList
        Dim list As New CSettingList(plh.Controls.Count)
        For Each i As usercontrols_config_settings_UCSetting In plh.Controls
            list.Add(i.Store())
        Next
        Return list
    End Function
#End Region

#Region "User Controls"
    Private Shared Function UCSetting(ByVal target As Control) As usercontrols_config_settings_UCSetting
        Dim ctrl As Control = target.Page.LoadControl(CSitemap.UCSetting)
        target.Controls.Add(ctrl)
        Return CType(ctrl, usercontrols_config_settings_UCSetting)
    End Function
#End Region

#Region "Event Handlers"
    Private Sub btnMoveUp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnMoveUp.Click
        m_list.MoveUp(m_group)
        Refresh()
    End Sub
    Private Sub btnMoveDn_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnMoveDn.Click
        m_list.MoveDown(m_group)
        Refresh()
    End Sub
#End Region

#Region "Private"
    Private Sub Refresh()
        Response.Redirect(CSitemap.ConfigSettings)
    End Sub
#End Region

End Class
