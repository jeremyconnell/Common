Imports SchemaConfig

Partial Class ConfigSettings : Inherits CPage

#Region "Security"
    Public ReadOnly Property IsDeveloper() As Boolean
        Get
            Return True 'User.IsInRole(CRole.ROLE_DEVELOPERS)
        End Get
    End Property
#End Region

#Region "Event Handlers - Page"
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        lnkAdd.NavigateUrl = CSitemap.GroupAdd

        For Each i As CGroup In CGroup.Cache
            UCGroup(plh).Display(i, CGroup.Cache)
        Next
    End Sub
#End Region

#Region "User Controls"
    Private Shared Function UCGroup(ByVal target As Control) As usercontrols_config_settings_UCGroup
        Dim ctrl As Control = target.Page.LoadControl(CSitemap.UCGroup)
        target.Controls.Add(ctrl)
        Return CType(ctrl, usercontrols_config_settings_UCGroup)
    End Function
#End Region

#Region "Event Handlers - Buttons"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        Dim list As New CSettingList(plh.Controls.Count)

        For Each i As usercontrols_config_settings_UCGroup In plh.Controls
            list.AddRange(i.Store())
        Next
        CSetting.Cache = Nothing
        CDataSrc.Default.BulkSave(list)
        Response.Redirect(Request.RawUrl)
    End Sub
#End Region

End Class
