Imports System.Web.Security
Imports System.Web.ApplicationServices
Imports Framework
Imports System.Collections.Specialized

Public Class CustomMembershipProvider : Inherits MembershipProvider

#Region "Config - Constructor"
    Public Overrides Sub Initialize(ByVal name As String, ByVal config As NameValueCollection)
        MyBase.Initialize(name, config)

        SetConfig(config, "name", _name)
        SetConfig(config, "description", _description)
        SetConfig(config, "requiresQuestionAndAnswer", _requiresQuestionAndAnswer)
        SetConfig(config, "enablePasswordReset", _enablePasswordReset)
        SetConfig(config, "requiresUniqueEmail", _requiresUniqueEmail)
        SetConfig(config, "maxInvalidPasswordAttempts", _maxInvalidPasswordAttempts)
        SetConfig(config, "minRequiredPasswordLength", _minRequiredPasswordLength)
        SetConfig(config, "passwordStrengthRegularExpression", _passwordStrengthRegularExpression)
    End Sub
#End Region

#Region "Config - Members"
    Private _name As String = "CustomMembershipProvider"
    Private _description As String = "NVInteractive Custom Membership Provider - Stores and retrieves membership data using ORM"
    Private _requiresQuestionAndAnswer As Boolean = False
    Private _enablePasswordReset As Boolean = True
    Private _requiresUniqueEmail As Boolean = False
    Private _maxInvalidPasswordAttempts As Integer = 5
    Private _minRequiredPasswordLength As Integer = 6
    Private _passwordStrengthRegularExpression As String = String.Empty
#End Region

#Region "Config - Public"
    Public Overrides ReadOnly Property Name() As String
        Get
            Return _name
        End Get
    End Property
    Public Overrides ReadOnly Property Description() As String
        Get
            Return _description
        End Get
    End Property
    Public Overrides ReadOnly Property RequiresQuestionAndAnswer() As Boolean
        Get
            Return _requiresQuestionAndAnswer
        End Get
    End Property
    Public Overrides ReadOnly Property EnablePasswordReset() As Boolean
        Get
            Return _enablePasswordReset
        End Get
    End Property
    Public Overrides ReadOnly Property EnablePasswordRetrieval() As Boolean
        Get
            Return False
        End Get
    End Property
    Public Overrides ReadOnly Property RequiresUniqueEmail() As Boolean
        Get
            Return _requiresUniqueEmail
        End Get
    End Property
    Public Overrides ReadOnly Property MaxInvalidPasswordAttempts() As Integer
        Get
            Return _maxInvalidPasswordAttempts
        End Get
    End Property
    Public Overrides ReadOnly Property MinRequiredNonAlphanumericCharacters() As Integer
        Get
            Return 1
        End Get
    End Property
    Public Overrides ReadOnly Property MinRequiredPasswordLength() As Integer
        Get
            Return _minRequiredPasswordLength
        End Get
    End Property
    Public Overrides ReadOnly Property PasswordAttemptWindow() As Integer
        Get
            Return 60
        End Get
    End Property
    Public Overrides ReadOnly Property PasswordFormat() As MembershipPasswordFormat
        Get
            Return MembershipPasswordFormat.Hashed
        End Get
    End Property
    Public Overrides ReadOnly Property PasswordStrengthRegularExpression() As String
        Get
            Return _passwordStrengthRegularExpression
        End Get
    End Property
#End Region

#Region "Interfaces - Not Fully Implemented"
    Public Overrides Function GetPassword(ByVal username As String, ByVal answer As String) As String
        Throw New Exception("Passwords use one-way encryption")
    End Function
    Public Overrides Property ApplicationName() As String
        Get
            Return String.Empty
        End Get
        Set(ByVal value As String)
        End Set
    End Property
    Public Overrides Function GetNumberOfUsersOnline() As Integer
        Return Integer.MinValue
    End Function
    Protected Overrides Function DecryptPassword(ByVal encodedPassword() As Byte) As Byte()
        Throw New Exception("Not Implemented")
    End Function
#End Region

#Region "Interfaces - Implemented"
    Public Overrides Function ValidateUser(ByVal username As String, ByVal password As String) As Boolean
        Return ELogin.Success = CUser.ValidateUser(username, password, Me.PasswordFormat, Me.MaxInvalidPasswordAttempts)
    End Function
    Public Overrides Function ChangePassword(ByVal username As String, ByVal oldPassword As String, ByVal newPassword As String) As Boolean
        Dim user As CUser = CheckLoginAndPassword(username, oldPassword)
        user.UserPasswordPlainText(PasswordFormat) = newPassword
        user.Save()
        Return True
    End Function
    Public Overrides Function ChangePasswordQuestionAndAnswer(ByVal username As String, ByVal password As String, ByVal newPasswordQuestion As String, ByVal newPasswordAnswer As String) As Boolean
        Dim user As CUser = CheckLoginAndPassword(username, password)
        user.UserPasswordQuestion = newPasswordQuestion
        user.UserPasswordAnswer = newPasswordAnswer
        user.Save()
        Return True
    End Function
    Public Overrides Function DeleteUser(ByVal username As String, ByVal deleteAllRelatedData As Boolean) As Boolean
        Dim user As New CUser()
        Dim users As CUserList = user.SelectByLoginName(Nothing, username)
        If users.Count = 0 Then Return False
        users(0).Delete()
        Return True
    End Function
    Protected Overrides Function EncryptPassword(ByVal password() As Byte) As Byte()
        Return CBinary.StringToBytes(CUser.Hash(CBinary.BytesToString(password)))
    End Function
    Public Overrides Function CreateUser(ByVal username As String, ByVal password As String, ByVal email As String, ByVal passwordQuestion As String, ByVal passwordAnswer As String, ByVal isApproved As Boolean, ByVal providerUserKey As Object, ByRef status As MembershipCreateStatus) As MembershipUser
        Dim u As New CUser
        With u
            .UserLoginName = username
            .UserFirstName = username
            .UserPasswordPlainText(PasswordFormat) = password
            .UserEmail = email
            .UserPasswordQuestion = passwordQuestion
            .UserPasswordAnswer = passwordAnswer
            .UserIsDisabled = isApproved
            .Save()
            status = MembershipCreateStatus.Success
        End With
        Return Cast(u)
    End Function
    Public Overrides Function GetAllUsers(ByVal pageIndex As Integer, ByVal pageSize As Integer, ByRef totalRecords As Integer) As MembershipUserCollection
        Dim pi As New CPagingInfo(pageSize, pageIndex)
        GetAllUsers = Cast(New CUser().SelectAll(pi))
        totalRecords = pi.Count
    End Function
    Public Overrides Function FindUsersByEmail(ByVal emailToMatch As String, ByVal pageIndex As Integer, ByVal pageSize As Integer, ByRef totalRecords As Integer) As MembershipUserCollection
        If Len(emailToMatch) = 0 Then Return New MembershipUserCollection

        Dim pi As New CPagingInfo(pageSize, pageIndex)
        Dim results As CUserList = (New CUser).SelectByEmail(pi, emailToMatch, False)
        totalRecords = pi.Count

        Return Cast(results)
    End Function
    Public Overrides Function FindUsersByName(ByVal usernameToMatch As String, ByVal pageIndex As Integer, ByVal pageSize As Integer, ByRef totalRecords As Integer) As MembershipUserCollection
        If Len(usernameToMatch) = 0 Then Return New MembershipUserCollection

        Dim pi As New CPagingInfo(pageSize, pageIndex)
        Dim results As CUserList = (New CUser).SelectSearch(pi, usernameToMatch)
        totalRecords = pi.Count
        Return Cast(results)
    End Function
    Public Overrides Function GetUser(ByVal providerUserKey As Object, ByVal userIsOnline As Boolean) As MembershipUser
        Dim userName As String = CStr(providerUserKey)
        Return Cast(New CUser(userName))
    End Function
    Public Overrides Function GetUser(ByVal username As String, ByVal userIsOnline As Boolean) As MembershipUser
        Dim user As CUser = CUser.GetByLogin(username, True)
        Return Cast(user)
    End Function
    Public Overrides Function GetUserNameByEmail(ByVal email As String) As String
        Dim user As CUser = CUser.GetByEmail(email, False)
        If IsNothing(user) Then Return String.Empty
        Return user.UserLoginName
    End Function
    Public Overrides Function ResetPassword(ByVal username As String, ByVal answer As String) As String
        Dim user As CUser = CUser.GetByLogin(username, True)
        If LCase(Trim(answer)) <> LCase(Trim(user.UserPasswordAnswer)) Then Throw New Exception("Invalid password answer")
        Dim newPass As String = Guid.NewGuid.ToString.Replace("-", "").Substring(0, 12)
        user.UserPasswordPlainText(PasswordFormat) = newPass
        user.UserLastPasswordChangedDate = DateTime.Now
        user.Save()
        Return newPass
    End Function
    Public Overrides Function UnlockUser(ByVal userName As String) As Boolean
        Dim user As New CUser()
        Dim list As CUserList = user.SelectByLoginName(Nothing, userName)
        If list.Count = 0 Then Return False
        user.UserIsLockedOut = False
        user.Save()
        Return True
    End Function
    Public Overrides Sub UpdateUser(ByVal user As MembershipUser)
        With CUser.GetByLogin(user.UserName, True)
            .UserEmail = user.Email
            .UserIsLockedOut = user.IsLockedOut
            .UserIsDisabled = Not user.IsApproved
            .UserPasswordQuestion = user.PasswordQuestion
            .UserComments = user.Comment
            .Save()
        End With
    End Sub
#End Region

#Region "Interfaces - Non-Standard"
    Public Function OverridePassword(ByVal userName As String, ByVal newPassword As String) As Boolean
        Dim user As CUser = CUser.GetByLogin(userName, False)
        If IsNothing(user) Then Return False
        With user
            .UserPasswordPlainText(PasswordFormat) = newPassword
            .Save()
        End With
        Return True
    End Function
#End Region

#Region "Private - Casting"
    Private Function Cast(ByVal users As IList) As MembershipUserCollection
        Dim m As New MembershipUserCollection()
        For Each i As CUser In users
            m.Add(Cast(i))
        Next
        Return m
    End Function
    Private Function Cast(ByVal user As CUser) As MembershipUser
        If IsNothing(user) Then Return Nothing
        With user
            Return New MembershipUser(Me.Name, .UserFirstName & " " & .UserLastName, .UserLoginName, .UserEmail, .UserPasswordQuestion, .UserComments, .UserIsDisabled, .UserIsLockedOut, .UserCreatedDate, .UserLastLoginDate, DateTime.MinValue, .UserLastPasswordChangedDate, .UserLastLockoutDate)
        End With
    End Function
#End Region

#Region "Private - Config"
    Private Sub SetConfig(ByVal config As NameValueCollection, ByVal key As String, ByRef value As Integer)
        Dim s As String = config(key)
        If Len(s) > 0 Then Integer.TryParse(s, value)
    End Sub
    Private Sub SetConfig(ByVal config As NameValueCollection, ByVal key As String, ByRef value As String)
        Dim s As String = config(key)
        If Len(s) > 0 Then value = s
    End Sub
    Private Sub SetConfig(ByVal config As NameValueCollection, ByVal key As String, ByRef value As Boolean)
        Dim s As String = config(key)
        If Len(s) > 0 Then value = ("true" = s)
    End Sub
#End Region

#Region "Private - Hash/Validate"
    Private Function CheckLoginAndPassword(ByVal username As String, ByVal oldPassword As String) As CUser
        Dim user As CUser = CUser.GetByLogin(username, True)
        If Not user.CheckPassword(oldPassword, PasswordFormat) Then Throw New Exception("Invalid old password")
        Return user
    End Function
#End Region

End Class
