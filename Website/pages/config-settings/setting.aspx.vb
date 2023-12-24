
Partial Class pages_setting : Inherits CPage

#Region "Querystring"
    Public ReadOnly Property RequestGroupId() As Integer
        Get
            Return CWeb.RequestInt("groupId", 0)
        End Get
    End Property
    Public ReadOnly Property RequestSettingId() As Integer
        Get
            Return CWeb.RequestInt("settingId", 0)
        End Get
    End Property
#End Region

#Region "Members"
    Private m_setting As CSetting
#End Region

#Region "Data"
    Public ReadOnly Property Setting() As CSetting
        Get
            If IsNothing(m_setting) Then
                If IsEdit Then
                    m_setting = CSetting.Cache.GetById(RequestSettingId)
                    If IsNothing(m_setting) Then Throw New Exception("Invalid SettingId: " & RequestSettingId)
                Else
                    m_setting = New CSetting
                    m_setting.SettingGroupId = RequestGroupId
                End If
            End If
            Return m_setting
        End Get
    End Property
    Public ReadOnly Property IsEdit() As Boolean
        Get
            Return RequestSettingId > 0
        End Get
    End Property
#End Region

#Region "Form"
    Public ReadOnly Property TypeId() As EConfigType
        Get
            Return CType(ddType.ValueInt, EConfigType)
        End Get
    End Property
#End Region

#Region "Abstract"
    Protected Overrides Sub PageInit()
        If IsEdit Then btnSave.Text = "Save Setting"
        If IsEdit Then PageTitle = "Edit Setting" Else PageTitle = "Create Setting"
        btnDelete.Visible = IsEdit

        With ddGroup
            .DataTextField = "GroupName"
            .DataValueField = "GroupId"
            .DataSource = CGroup.Cache
            .DataBind()
            .ValueInt = RequestGroupId
        End With

        With ddType
            .DataSource = CType_.Cache
            .DataTextField = "TypeName"
            .DataValueField = "TypeId"
            .DataBind()
            .BlankItem("-- Select Data Type --")
        End With

        With ddList
            .DataSource = CList.Cache
            .DataTextField = "ListName"
            .DataValueField = "ListId"
            .DataBind()
            .BlankItem("-- Select List --")
        End With
    End Sub
    Protected Overrides Sub PageLoad()
        With Setting
            ddGroup.ValueInt = .SettingGroupId
            ddType.ValueInt = .SettingTypeId
            ddList.ValueInt = .SettingListId
            txtName.Text = .SettingName
            chkCanEdit.Checked = .SettingClientCanEdit
        End With
        ddType_SelectedIndexChanged()
    End Sub
    Protected Overrides Sub PageSave()
        With Setting
            .SettingTypeId = TypeId
            .SettingGroupId = ddGroup.ValueInt
            .SettingListId = ddList.ValueInt
            .SettingName = txtName.Text
            .SettingClientCanEdit = chkCanEdit.Checked
            .Save()
        End With
    End Sub
#End Region

#Region "Event Handlers - Form"
    Protected Sub ddType_SelectedIndexChanged() Handles ddType.SelectedIndexChanged
        ddList.Visible = (TypeId = EConfigType.ListAsInteger Or TypeId = EConfigType.ListAsString)
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        If Not Me.IsValid() Then Exit Sub
        PageSave()
        Response.Redirect(CSitemap.ConfigSettings)
    End Sub
    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect(CSitemap.ConfigSettings)
    End Sub
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Setting.Delete()
        Response.Redirect(CSitemap.ConfigSettings)
    End Sub
#End Region

End Class
