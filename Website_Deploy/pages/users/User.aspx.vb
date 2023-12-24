Partial Class pages_Users_User : Inherits CPage

    Public Const ENCRYPTION As MembershipPasswordFormat = MembershipPasswordFormat.Encrypted

#Region "Members"
    Private m_user As CUser
#End Region

#Region "Querystring"
    Public ReadOnly Property UserLoginName() As String
        Get
            Return CWeb.RequestStr("userLoginName")
        End Get
    End Property
    Public ReadOnly Property IsEdit() As Boolean
        Get
            Return UserLoginName <> String.Empty
        End Get
    End Property
#End Region

#Region "Data"
    Public Shadows ReadOnly Property [User]() As CUser
        Get
            If IsNothing(m_user) Then
                If IsEdit Then
                    m_user = New CUser(UserLoginName)
                    If IsNothing(m_user) Then CSitemap.RecordNotFound("User", UserLoginName)
                Else
                    m_user = New CUser
                    m_user.UserPasswordPlainText(ENCRYPTION) = CUser.GeneratePassword()
                End If
            End If
            Return m_user
        End Get
    End Property
#End Region

#Region "Navigation"
    Private Sub Refresh()
        Response.Redirect(CSitemap.UserEdit(Me.User.UserLoginName))
    End Sub
    Private Sub ReturnToList()
        Response.Redirect(CSitemap.Users)
    End Sub
#End Region

#Region "Event Handlers - Form"
    Protected Sub txtUserLoginName_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles txtUserLoginName.ServerValidate
        Dim u As CUserList = User.SelectByLoginName(txtUserLoginName.Text)
        args.IsValid = (u.Count = 0) OrElse IsEdit AndAlso u.Count = 1 AndAlso u(0).UserLoginName = Me.User.UserLoginName
    End Sub
    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not Me.IsValid() Then Exit Sub
        PageSave()
        Refresh()
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        ReturnToList()
    End Sub
    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Me.User.Delete()
        ReturnToList()
    End Sub
#End Region

#Region "Overrides"
    Protected Overrides Sub PageInit()
        If IsEdit Then
            Me.Title = "User Details"
        Else
            Me.Title = "Create New User"
        End If

        btnDelete.Visible = IsEdit
        If IsEdit Then btnCancel.Text = "Back" Else btnSave.Text = "Create User"

        plhActivity.Visible = IsEdit
        If IsEdit Then txtUserLoginName.Mode = EControlMode.Disabled

        cblRoles.DataSource = CRole.Cache
        cblRoles.DataBind()

        If IsEdit Then
            AddButton(CSitemap.UserAdd(), "Create a new User")
            AddMenuSide("New User", CSitemap.UserAdd)
        End If

        txtUserLoginName.ErrorMessageCustom = "This login name is already in use"
        txtUserLoginName.RequiredCustom = True
    End Sub
    Protected Overrides Sub PageLoad()
        With Me.User
            txtUserLoginName.Text = .UserLoginName
            txtUserPasswordPlainText.Text = .UserPasswordPlainText(ENCRYPTION)
            txtUserFirstName.Text = .UserFirstName
            txtUserLastName.Text = .UserLastName
            txtUserEmail.Text = .UserEmail
            chkUserIsDisabled.Checked = .UserIsDisabled
            txtUserCreatedDate.ValueDateTime = .UserCreatedDate
            txtUserLastLoginDate.ValueDateTime = .UserLastLoginDate
            txtUserLastPasswordChangedDate.ValueDateTime = .UserLastPasswordChangedDate
            txtUserPasswordQuestion.Text = .UserPasswordQuestion
            txtUserPasswordAnswer.Text = .UserPasswordAnswer
            txtUserFailedPasswordAttemptCount.ValueInt = .UserFailedPasswordAttemptCount
            txtUserFailedPasswordAttemptStartDate.ValueDateTime = .UserFailedPasswordAttemptStartDate
            chkUserIsLockedOut.Checked = .UserIsLockedOut
            txtUserLastLockoutDate.ValueDate = .UserLastLockoutDate
            txtUserComments.Text = .UserComments

            cblRoles.SelectedStrings = .UserRoles.RoleNames
        End With
    End Sub
    Protected Overrides Sub PageSave()
        With Me.User
            .UserLoginName = txtUserLoginName.Text
            .UserFirstName = txtUserFirstName.Text
            .UserLastName = txtUserLastName.Text
            .UserEmail = txtUserEmail.Text
            .UserIsDisabled = chkUserIsDisabled.Checked
            .UserPasswordQuestion = txtUserPasswordQuestion.Text
            .UserPasswordAnswer = txtUserPasswordAnswer.Text
            .UserComments = txtUserComments.Text

            If .UserPasswordPlainText(ENCRYPTION) <> txtUserPasswordPlainText.Text Then
                .UserLastPasswordChangedDate = DateTime.Now
                .UserPasswordPlainText(ENCRYPTION) = txtUserPasswordPlainText.Text
            End If

            .Save()

            SaveRoles()
        End With
    End Sub
    Private Sub SaveRoles()
        Dim selectedRoleNames As List(Of String) = cblRoles.SelectedStrings
        With User
            For Each roleName As String In selectedRoleNames
                If Not .UserRoles.RoleNames.Contains(roleName) Then
                    CUserRole.InsertPair(.UserLoginName, roleName)
                End If
            Next
            For Each roleName As String In .UserRoles.RoleNames
                If Not selectedRoleNames.Contains(roleName) Then
                    CUserRole.DeletePair(.UserLoginName, roleName)
                End If
            Next
        End With
    End Sub

#End Region

End Class
