Imports System
Imports System.Data
Imports System.Collections.Generic
Imports System.Web.Security
Imports Framework

<CLSCompliant(True)> _
Public Enum ELogin
    BadUsername
    Disabled
    LockedOut
    BadPassword
    Success
End Enum


'Table-Row Class (Customisable half)
Partial Public Class CUser

#Region "Constants"
    Public Const PASSWORD_MASK As String = "******"
    Public Shared USER_JOIN_USERROLE As String = String.Concat(CUser.TABLE_NAME, " INNER JOIN ", CUserRole.TABLE_NAME, " ON UserLoginName=URUserLogin")
    Public Shared USER_OUTER_JOIN_USERROLE As String = String.Concat(CUser.TABLE_NAME, " LEFT OUTER JOIN ", CUserRole.TABLE_NAME, " ON UserLoginName=URUserLogin")
#End Region

#Region "Constructors (Public)"
    'Default DataSrc
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal userLoginName As String)
        MyBase.New(userLoginName)
    End Sub

    'Explicit DataSrc (Overloads)
    Public Sub New(ByVal dataSrc As CDataSrc)
        MyBase.New(dataSrc)
    End Sub
    Public Sub New(ByVal dataSrc As CDataSrc, ByVal userLoginName As String)
        MyBase.New(dataSrc, userLoginName)
    End Sub

    'Transactional (shares an open connection)
    Public Sub New(ByVal dataSrc As CDataSrc, ByVal userLoginName As String, ByVal txOrNull As IDbTransaction)
        MyBase.New(dataSrc, userLoginName, txOrNull)
    End Sub
#End Region

#Region "Default Values"
    Protected Overrides Sub InitValues_Custom()
        'Null values
        m_userLoginName = String.Empty
        m_userPasswordHashedSha1 = String.Empty
        m_userFirstName = String.Empty
        m_userLastName = String.Empty
        m_userEmail = String.Empty
        m_userIsDisabled = False
        m_userLastLoginDate = DateTime.MinValue
        m_userLastPasswordChangedDate = DateTime.MinValue
        m_userPasswordQuestion = String.Empty
        m_userPasswordAnswer = String.Empty
        m_userFailedPasswordAttemptCount = Integer.MinValue
        m_userFailedPasswordAttemptStartDate = DateTime.MinValue
        m_userIsLockedOut = False
        m_userLastLockoutDate = DateTime.MinValue
        m_userComments = String.Empty

        'Custom values
        m_userCreatedDate = DateTime.Now
        m_userPasswordSalt = GeneratePassword()
    End Sub
#End Region

#Region "Default DataSrc"
    Protected Overrides Function DefaultDataSrc() As CDataSrc
        Return CDataSrc.Default
    End Function
#End Region

#Region "Members"
    <NonSerialized()> Private m_userRoles As CUserRoleList
    <NonSerialized()> Private m_roles As List(Of String)
#End Region

#Region "Properties - Relationships"
    'Relationships - Foriegn Keys (e.g parent)

    'Relationships - Collections (e.g. children)
    Public Property UserRoles() As CUserRoleList
        Get
            If IsNothing(m_userRoles) Then
                m_userRoles = New CUserRole(Me.DataSrc).SelectByUserLogin(Me.UserLoginName)
            End If
            Return m_userRoles
        End Get
        Set(ByVal value As CUserRoleList)
            m_userRoles = value
        End Set
    End Property
    Public ReadOnly Property Roles() As List(Of String)
        Get
            Return UserRoles.RoleNames
        End Get
    End Property
#End Region

#Region "Properties - Customisation"
    'Derived
    Public ReadOnly Property RoleNames() As String
        Get
            Return CUtilities.ListToString(Roles)
        End Get
    End Property

    'ReadOnly Properties
    Public ReadOnly Property UserFullName() As String
        Get
            Return String.Concat(UserFirstName, " ", UserLastName)
        End Get
    End Property
    Public ReadOnly Property UserLoginNameOrUnderscores() As String
        Get
            If String.IsNullOrEmpty(UserLoginName) Then Return "___"
            Return UserLoginName
        End Get
    End Property
#End Region

#Region "Methods - Save/Delete Overrides"
    Public Overrides Sub Delete(ByVal txOrNull As IDbTransaction)
        'Ensure a transaction is used
        If IsNothing(txOrNull) Then
            BulkDelete(Me)
        Else
            'Cascade delete related records
            UserRoles.DeleteAll(txOrNull)
            MyBase.Delete(txOrNull)
        End If
    End Sub
#End Region

#Region "Methods - Database Queries"
    'Paged
    Public Function SelectByLoginName(ByVal pi As CPagingInfo, ByVal loginName As String) As CUserList
        Return SelectWhere(pi, "UserLoginName", ESign.EqualTo, loginName)
    End Function
    Public Function SelectByEmail(ByVal pi As CPagingInfo, ByVal email As String, ByVal exact As Boolean) As CUserList
        If exact Then
            Return SelectWhere(pi, "UserEmail", ESign.EqualTo, email)
        Else
            Return SelectWhere(pi, "UserEmail", ESign.Like, "%" & email & "%")
        End If
    End Function
    Public Function SelectByRoleOrLogin(ByVal pi As CPagingInfo, ByVal role As String, ByVal login As String) As CUserList
        Dim join As String = TABLE_NAME
        Dim where As CCriteriaList = New CCriteriaList()

        If Not String.IsNullOrEmpty(login) Then
            where.Add("UserLoginName", ESign.Like, "%" & login & "%")
        End If

        If Not String.IsNullOrEmpty(role) Then
            where.Add("URRoleName", role)
            join = USER_JOIN_USERROLE
        End If

        pi.TableName = join
        Return SelectWhere(pi, where)
    End Function
    Public Function SelectSearch(ByVal pi As CPagingInfo, ByVal name As String) As CUserList
        Return SelectWhere(pi, BuildWhere(name))
    End Function
    Public Function SelectSearch_Dataset(ByVal name As String) As DataSet
        Return SelectWhere_Dataset(New CCriteriaList(BuildWhere(name)))
    End Function
    Public Function BuildWhere(ByVal name As String) As CCriteriaGroup
        name = String.Concat("%", name, "%")

        Dim criteria As New CCriteriaGroup(EBoolOperator.Or)
        criteria.Add("UserLoginName", ESign.Like, name)
        criteria.Add("UserFirstName", ESign.Like, name)
        criteria.Add("UserLastName", ESign.Like, name)

        Return criteria
    End Function



    'Non-Paged
    Public Function SelectByLoginName(ByVal loginName As String) As CUserList
        Return SelectByLoginName(Nothing, loginName)
    End Function
    Public Function SelectByEmail(ByVal email As String, ByVal exact As Boolean) As CUserList
        Return SelectByEmail(Nothing, email, exact)
    End Function
    Public Function SelectSearch(ByVal name As String) As CUserList
        Return SelectSearch(Nothing, name)
    End Function

    'Associative Table: 2-Step Walk
    Public Function SelectByRoleName(ByVal roleName As String) As CUserList
        Return SelectByRoleName(Nothing, roleName, String.Empty)
    End Function
    Public Function SelectByRoleName(ByVal pi As CPagingInfo, ByVal roleName As String) As CUserList
        Return SelectByRoleName(pi, roleName, String.Empty)
    End Function
    Public Function SelectByRoleName(ByVal roleName As String, ByVal search As String) As CUserList
        Return SelectByRoleName(Nothing, roleName, search)
    End Function
    Public Function SelectByRoleName(ByVal pi As CPagingInfo, ByVal roleName As String, ByVal search As String) As CUserList
        Dim where As CCriteriaList = New CCriteriaList()
        where.Add(BuildWhere(search))
        where.Add("URRoleName", roleName)
        If IsNothing(pi) Then
            Return SelectWhere(where, USER_JOIN_USERROLE)
        Else
            pi.TableName = USER_JOIN_USERROLE
            Return SelectWhere(pi, where)
        End If
    End Function

    Public Function SelectRemainingRoleName(ByVal roleName As String) As CUserList
        Return SelectRemainingRoleName(Nothing, roleName, String.Empty)
    End Function
    Public Function SelectRemainingRoleName(ByVal roleName As String, ByVal search As String) As CUserList
        Return SelectRemainingRoleName(Nothing, roleName, search)
    End Function
    Public Function SelectRemainingRoleName(ByVal pi As CPagingInfo, ByVal roleName As String) As CUserList
        Return SelectRemainingRoleName(pi, roleName, String.Empty)
    End Function
    Public Function SelectRemainingRoleName(ByVal pi As CPagingInfo, ByVal roleName As String, ByVal search As String) As CUserList
        Dim join As String = String.Concat(USER_OUTER_JOIN_USERROLE, " AND URRoleName=@RoleName")
        Dim where As New CCriteriaList("URUserLogin", Nothing) 'Associated with a Not-In Join
        where.Parameters.Add("RoleName", roleName) 'extra d-sql parameter above (part of join expr, not where expr)
        'where.Add(BuildWhere(search)) 'Search filters

        If IsNothing(pi) Then
            Return SelectWhere(where, join)
        Else
            pi.TableName = join
            Return SelectWhere(pi, where)
        End If
    End Function

#End Region

#Region "Helper Methods - AddRole/GeneratePassword"
    Public Shared Function ValidateUser(ByVal username As String, ByVal password As String, ByVal format As MembershipPasswordFormat, ByVal maxInvalidAttempts As Integer) As ELogin
        If String.IsNullOrEmpty(username) Then Return ELogin.BadUsername

        Dim user As CUser = CUser.GetByLogin(username, False)
        If IsNothing(user) Then Return ELogin.BadUsername

        With user
            'Disabled users checked first
            If .UserIsDisabled Then Return ELogin.Disabled

            'Bad password
            If Not .CheckPassword(password, format) Then
                'Up the count
                If .UserFailedPasswordAttemptCount = Integer.MinValue Then
                    .UserFailedPasswordAttemptCount = 1
                    .UserFailedPasswordAttemptStartDate = DateTime.Now
                Else
                    .UserFailedPasswordAttemptCount += 1
                End If

                'Record the window, lockout if too many bad attempts within the window
                If .UserFailedPasswordAttemptCount > maxInvalidAttempts Then
                    .UserIsLockedOut = True
                    .UserLastLockoutDate = DateTime.Now
                    .Save()
                    Return ELogin.LockedOut
                End If
                .Save()
                Return ELogin.BadPassword
            End If

            If .UserIsLockedOut Then Return ELogin.LockedOut 'Successful password no good once locked out

            'Successful - reset lockout params
            .UserLastLoginDate = DateTime.Now
            .UserFailedPasswordAttemptStartDate = DateTime.MinValue
            .UserFailedPasswordAttemptCount = 0
            .Save()

            Return ELogin.Success
        End With
    End Function


    Public Sub AddRole(ByVal roleName As String)
        If Roles.Contains(roleName) Then Exit Sub 'Warning: Case-sensitive

        Dim userRole As New SchemaMembership.CUserRole()
        userRole.URUserLogin = Me.UserLoginName
        userRole.URRoleName = roleName
        userRole.Save()

        Me.UserRoles.Add(userRole)
    End Sub

    Private Shared _random As New System.Random
    Public Shared Function GeneratePassword(Optional ByVal minLength As Integer = 8, Optional ByVal maxLength As Integer = 8) As String
        Dim length As Integer = _random.Next(minLength, maxLength)
        Dim sb As New System.Text.StringBuilder()
        For i As Integer = 0 To length - 1
            Dim randomIndex As Integer = _random.Next(0, PasswordChars.Length - 1)
            sb.Append(PasswordChars(randomIndex))
        Next
        Return sb.ToString
    End Function

    Private Shared m_passwordChars As Char() = Nothing
    Private Shared ReadOnly Property PasswordChars() As Char()
        Get
            If IsNothing(m_passwordChars) Then
                Dim list As New List(Of Integer)

                'Numbers 0-9
                For i As Integer = 48 To 57
                    list.Add(i)
                Next

                'Letters A-Z
                For i As Integer = 65 To 90
                    list.Add(i)
                Next

                'Letters a-z
                For i As Integer = 97 To 122
                    list.Add(i)
                Next

                'Misc valid chars
                'list.Add(33) '!
                'list.Add(64) '@
                'list.Add(35) '#
                'list.Add(36) '$
                'list.Add(37) '%
                'list.Add(94) '^
                'list.Add(38) '&
                'list.Add(42) '*
                'list.Add(40) '(
                'list.Add(41) ')
                'list.Add(43) '+
                'list.Add(45) '-
                'list.Add(61) '=

                Dim chars As New List(Of Char)(list.Count)
                For Each i As Integer In list
                    chars.Add(Chr(i))
                Next
                m_passwordChars = chars.ToArray()
            End If
            Return m_passwordChars
        End Get
    End Property
#End Region

#Region "ToXml"
    Protected Overrides Sub ToXml_Custom(ByVal w As System.Xml.XmlWriter)
        'Store(w, "Example", Me.Example)
    End Sub
#End Region

#Region "Shared"
    Public Function IsInRole(ByVal roles As IList) As Boolean
        If IsNothing(roles) OrElse roles.Count = 0 Then Return True
        For Each i As String In Current.Roles
            For Each j As String In roles
                If j = "*" Then Return True
                If LCase(i) = LCase(j) AndAlso Len(i) > 0 Then Return True
            Next
        Next
        Return False
    End Function
    Public Function IsInRole(ByVal role As String) As Boolean
        If role = "*" Then Return True
        For Each i As String In Me.Roles
            If LCase(i) = LCase(role) AndAlso Len(i) > 0 Then Return True
        Next
        Return False
    End Function
    Public Shared Function CanSee(ByVal role As String) As Boolean
        If role = "*" Then Return True
        If Not IsLoggedIn Then Return False
        Return Current.IsInRole(role)
    End Function
    Public Shared Function CanSee(ByVal roles As IList) As Boolean
        If IsNothing(roles) OrElse roles.Count = 0 Then Return True
        If Not IsLoggedIn Then Return roles.Contains("*")
        Return Current.IsInRole(roles)
    End Function
    Private Const SESSION_KEY As String = "SchemaMembership.User.Current"
    Public Shared ReadOnly Property IsLoggedIn() As Boolean
        Get
            If ISNothing(System.Web.HttpContext.Current) Then Return False
            If Not My.User.IsAuthenticated Then Return False
            If IsNothing(Current) Then
                FormsAuthentication.SignOut()
                Return False
            End If
            Return True
        End Get
    End Property
    Public Shared Function CurrentLogin() As String
        If IsNothing(Current) Then Return String.Empty
        Return Current.UserLoginName
    End Function
    Public Shared Function Current() As CUser
        If Not My.User.IsAuthenticated Then Return Nothing

        Dim user As CUser = Nothing
        Try
            user = CType(CSessionBase.Get(SESSION_KEY), CUser) 'Fails if website still initialising
        Catch
            Return CUser.GetByLogin(My.User.Name, False)
        End Try

        If IsNothing(user) Then
            user = CUser.GetByLogin(My.User.Name, False)
            If IsNothing(user) Then Return Nothing
            If user.UserLastLoginDate.AddMinutes(1) < DateTime.Now Then
                user.UserLastLoginDate = DateTime.Now 'Records cookie-based logins
                user.Save()
            End If
            CSessionBase.Set(SESSION_KEY, user)
        End If
        Return user
    End Function
    Public Shared Function IsEmailInUse(ByVal email As String) As Boolean
        Return Not IsNothing(GetByEmail(email, False))
    End Function
    Public Shared Function GetByEmail(ByVal email As String, ByVal throwEx As Boolean) As CUser
        Dim list As CUserList = New CUser().SelectByEmail(email, True)
        If list.Count = 1 Then Return list(0)
        If list.Count > 1 Then Throw New Exception(String.Concat(list.Count, " users found having email: ", email))
        If Not throwEx Then Return Nothing
        Throw New Exception("Could not find user having email: " & email)
    End Function
    Public Shared Function GetByLogin(ByVal userName As String, ByVal throwEx As Boolean) As CUser
        Dim list As CUserList = (New CUser).SelectByLoginName(Nothing, userName)
        If list.Count = 1 Then Return list(0)
        If list.Count > 1 Then Throw New Exception(String.Concat(list.Count, " users found having userName: ", userName))

        'standard asp.net change-password control supplies a userfullname instead of userLoginName 
        For Each i As CUser In (New CUser).SelectAll()
            If String.Equals(i.UserFullName, userName, StringComparison.CurrentCultureIgnoreCase) Then
                Return i
            End If
        Next

        If throwEx Then
            Throw New Exception("Invalid username")
        Else
            Return Nothing
        End If
    End Function
#End Region

#Region "Encryption"
    Public Function CheckPassword(ByVal attempt As String, ByVal format As MembershipPasswordFormat) As Boolean
        Select Case format
            Case MembershipPasswordFormat.Clear : Return UserPasswordHashedSha1 = attempt
            Case MembershipPasswordFormat.Hashed : Return UserPasswordHashedSha1 = Hash(attempt, UserPasswordSalt) OrElse UserPasswordHashedSha1.ToLower = HashAsMD5(attempt, UserPasswordSalt).ToLower
            Case MembershipPasswordFormat.Encrypted : Return UserPasswordHashedSha1 = Encrypt(attempt, UserPasswordSalt)
        End Select
    End Function
    Public Property UserPasswordPlainText(ByVal format As MembershipPasswordFormat) As String
        Get
            Select Case format
                Case MembershipPasswordFormat.Clear : Return UserPasswordHashedSha1
                Case MembershipPasswordFormat.Encrypted : Return UserPasswordPlainText_2Way
            End Select
            Return PASSWORD_MASK
        End Get
        Set(ByVal value As String)
            Select Case format
                Case MembershipPasswordFormat.Clear : UserPasswordHashedSha1 = value
                Case MembershipPasswordFormat.Hashed : UserPasswordPlainText_1Way = value
                Case MembershipPasswordFormat.Encrypted : UserPasswordPlainText_2Way = value
            End Select
        End Set
    End Property

    Public WriteOnly Property UserPasswordPlainText_1Way() As String
        Set(ByVal value As String)
            If value <> PASSWORD_MASK Then
                UserPasswordHashedSha1 = Hash(value, UserPasswordSalt)
            End If
        End Set
    End Property
    Public Property UserPasswordPlainText_2Way() As String
        Get
            Return Decrypt(UserPasswordHashedSha1, UserPasswordSalt)
        End Get
        Set(ByVal value As String)
            UserPasswordHashedSha1 = Encrypt(value, UserPasswordSalt)
        End Set
    End Property

    Public Function CheckPassword_1Way(ByVal oldPassword As String) As Boolean
        Return Encrypt(oldPassword, UserPasswordSalt) = UserPasswordHashedSha1
    End Function
    Public Function CheckPassword_2Way(ByVal oldPassword As String) As Boolean
        Return Encrypt(oldPassword, UserPasswordSalt) = UserPasswordHashedSha1
    End Function

    Public Shared Function Hash(ByVal password As String, ByVal salt As String) As String
        Return Hash(String.Concat(password, salt))
    End Function
    Public Shared Function HashAsMD5(ByVal password As String, ByVal salt As String) As String
        Return CBinary.MD5(String.Concat(salt, password)) 'for backwards-compat with php sample
    End Function
    Public Shared Function Encrypt(ByVal password As String, ByVal salt As String) As String
        Return Encrypt(String.Concat(password, salt))
    End Function
    Public Shared Function Decrypt(ByVal password As String, ByVal salt As String) As String
        Dim s As String = Decrypt(password)
        If Len(salt) > 0 AndAlso Len(salt) < s.Length Then s = s.Substring(0, s.Length - salt.Length)
        Return s
    End Function

    Public Shared Function Hash(ByVal password As String) As String
        Return FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1")
    End Function
    Public Shared Function Encrypt(ByVal password As String) As String
        Return CBinary.EncryptRijndaelToBase64(password)
    End Function
    Public Shared Function Decrypt(ByVal password As String) As String
        Try
            Return CBinary.DecryptRijndaelAsStr(password)
        Catch
            Return password
        End Try
    End Function
#End Region

End Class
