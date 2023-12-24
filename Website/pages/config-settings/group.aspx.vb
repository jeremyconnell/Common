
Partial Class pages_group : Inherits CPage

#Region "Querystring "
    Public ReadOnly Property GroupId() As Integer
        Get
            Return CWeb.RequestInt("groupId")
        End Get
    End Property
    Public ReadOnly Property IsEdit() As Boolean
        Get
            Return GroupId > 0
        End Get
    End Property
#End Region

#Region "Member"
    Private m_group As CGroup
#End Region

#Region "Data"
    Public ReadOnly Property Group() As CGroup
        Get
            If IsNothing(m_group) Then
                If IsEdit Then
                    m_group = CGroup.Cache.GetById(GroupId)
                    If IsNothing(m_group) Then Response.Redirect(CSitemap.ConfigSettings)
                Else
                    m_group = New CGroup
                End If
            End If
            Return m_group
        End Get
    End Property
#End Region

#Region "Overrides"
    Protected Overrides Sub PageInit()
        If IsEdit Then btnSave.Text = "Save Group"
        If IsEdit Then PageHeading = "Edit Group" Else PageHeading = "Create Group"
        btnDelete.Visible = IsEdit
    End Sub
    Protected Overrides Sub PageLoad()
        With Group
            txtGroupName.Text = .GroupName
        End With
    End Sub
    Protected Overrides Sub PageSave()
        With Group
            .GroupName = txtGroupName.Text
            .Save()
        End With
    End Sub
#End Region

#Region "Event Handlers"
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        If Not Page.IsValid Then Exit Sub
        PageSave()
        Response.Redirect(CSitemap.ConfigSettings)
    End Sub
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect(CSitemap.ConfigSettings)
    End Sub
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Group.Delete()
        Response.Redirect(CSitemap.ConfigSettings)
    End Sub
#End Region

End Class
