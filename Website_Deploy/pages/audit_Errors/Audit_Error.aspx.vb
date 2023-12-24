Partial Class pages_Audit_Errors_Audit_Error : Inherits CPage

#Region "Querystring"
    Public ReadOnly Property ErrorID() As Integer
        Get
            Return CWeb.RequestInt("errorID")
        End Get
    End Property

    Public ReadOnly Property Type1() As Integer
        Get
            Return CWeb.RequestInt("type1")
        End Get
    End Property
    Public ReadOnly Property Message1() As Integer
        Get
            Return CWeb.RequestInt("message1")
        End Get
    End Property
    Public ReadOnly Property Type2() As Integer
        Get
            Return CWeb.RequestInt("type2")
        End Get
    End Property
    Public ReadOnly Property Message2() As Integer
        Get
            Return CWeb.RequestInt("message2")
        End Get
    End Property
#End Region

#Region "Members"
    Private m_audit_Error As CAudit_Error
#End Region

#Region "Data"
    Public ReadOnly Property [Audit_Error]() As CAudit_Error
        Get
            If IsNothing(m_audit_Error) Then
                Try
                    m_audit_Error = New CAudit_Error(ErrorID)
                Catch
                    m_audit_Error = New CAudit_Error(Type1, Message1, Type2, Message2)
                End Try
            End If
            Return m_audit_Error
        End Get
    End Property
#End Region

#Region "Event Handlers - Page"
    Protected Overrides Sub PageInit()
        UnbindSideMenu()
        AddMenuSide("Error Log", CSitemap.Audit_Errors)
        If ErrorID = Integer.MinValue Then
            AddMenuSide("Error Group")
            AddMenuSide(CUtilities.NameAndCount("Individual", [Audit_Error].ErrorID), CSitemap.Audit_ErrorGroup(Type1, Message1, Type2, Message2))
            Me.Title = "Group of Similar Errors"
            txtErrorDateCreated.Label = "Last Time"
            txtErrorUserName.Label = "User"
        Else
            AddMenuSide("Error Details")
            Me.Title = "Error #" & ErrorID
            With Audit_Error
                AddMenuSide(CUtilities.NameAndCount("Similar Errors", .CountSimilar()), CSitemap.Audit_ErrorGroup(.ErrorTypeHash, .ErrorMessageHash, .ErrorInnerTypeHash, .ErrorInnerMessageHash))
            End With
        End If
    End Sub
    Protected Overrides Sub PageLoad()
        With Me.Audit_Error
            txtErrorUserName.Text = .ErrorUserName
            txtErrorUserName.NavigateUrl = CSitemap.UserEdit(.ErrorUserID)
            txtErrorUserName.ToolTip = .ErrorUserID
            If txtErrorUserName.Text = String.Empty Then txtErrorUserName.Text = txtErrorUserName.ToolTip

            txtErrorUrl.Text = .FullUrl
            txtErrorUrl.NavigateUrl = .FullUrl

            txtErrorMachineName.Text = .ErrorMachineName
            txtErrorApplicationName.Text = .ErrorApplicationName
            txtErrorApplicationVersion.Text = .ErrorApplicationVersion
            txtErrorType.Text = .ErrorType
            txtErrorMessage.Text = String.Concat("<b>", .ErrorMessage, "</b>")
            txtErrorInnerType.Text = .ErrorInnerType
            txtErrorInnerMessage.Text = String.Concat("<b>", .ErrorInnerMessage, "</b>")
            txtErrorDateCreated.ValueDateTime = .ErrorDateCreated

            litStacktrace.Text = .ErrorStacktrace
            litInnerStacktrace.Text = .ErrorInnerStacktrace
        End With
    End Sub
#End Region

End Class
