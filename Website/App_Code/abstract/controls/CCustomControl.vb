Imports Microsoft.VisualBasic

Public Enum EControlMode
    Editable = 0
    Disabled = 1
    Locked = 2
End Enum

Public MustInherit Class CCustomControl : Inherits UserControl


#Region "Events"
    Public Event ServerValidate As ServerValidateEventHandler
#End Region

#Region "Members"
    Private m_errorMessage As String
    Private m_errorMessageCustom As String
#End Region

#Region "Event Handlers"
    Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
        MyBase.OnInit(e)
        RestoreMode()

        AddHandler CtrlValidatorCustom.ServerValidate, AddressOf CtrlValidatorCustom_ServerValidate
    End Sub
    Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)
        MyBase.OnPreRender(e)

        CtrlMain.Visible = Not Locked
        CtrlValidator.Visible = Not Locked
        CtrlValidatorCustom.Visible = Not Locked
        If Locked Then
            CtrlLocked.Text = GetLockedText()
            CtrlMain.Visible = False
            CtrlLocked.Visible = True
        End If


        CtrlLocked.Visible = Locked And CtrlLocked.Text.Length > 0
        If Locked Or Disabled Then Required = False
        SetEnabled = Enabled
        CtrlLabel.Visible = (Layout <> ELayout.None)

        If Not IsNothing(CtrlDescription) AndAlso 0 = CtrlDescription.Text.Length Then CtrlDescription.Visible = False
        If Not IsNothing(CtrlValidator) Then CtrlValidator.ErrorMessage = String.Format(ErrorMessage, Label)
        If Not IsNothing(CtrlValidatorCustom) Then CtrlValidatorCustom.ErrorMessage = String.Format(ErrorMessageCustom, Label)
        If Not IsNothing(CtrlValidatorScript) Then CtrlValidatorScript.Visible = Required
    End Sub
    Protected Sub CtrlValidatorCustom_ServerValidate(ByVal sender As Object, ByVal e As ServerValidateEventArgs)
        RaiseEvent ServerValidate(Me, e)
    End Sub
#End Region

#Region "Appearance"
    Public Property Layout() As ELayout
        Get
            Return CtrlContainerBegin.Layout
        End Get
        Set(ByVal value As ELayout)
            CtrlContainerBegin.Layout = value
            CtrlContainerEnd.Layout = value
            CtrlSeparator1.Layout = value
            CtrlSeparator2.Layout = value
        End Set
    End Property
    Public Property Label() As String
        Get
            Return CtrlLabel.Text
        End Get
        Set(ByVal value As String)
            CtrlLabel.Text = value
        End Set
    End Property
    Public Property ShowLabel() As Boolean
        Get
            Return CtrlLabel.Visible
        End Get
        Set(ByVal value As Boolean)
            CtrlLabel.Visible = value
        End Set
    End Property
    Public Property ErrorMessage() As String
        Get
            If String.IsNullOrEmpty(m_errorMessage) Then
                Return DefaultFormatRequiredText
            End If
            Return m_errorMessage
        End Get
        Set(ByVal value As String)
            m_errorMessage = value
        End Set
    End Property
    Public Property ErrorMessageCustom() As String
        Get
            If String.IsNullOrEmpty(m_errorMessageCustom) Then
                Return DefaultFormatRequiredText
            End If
            Return m_errorMessageCustom
        End Get
        Set(ByVal value As String)
            m_errorMessageCustom = value
        End Set
    End Property
    Public Property Description() As String
        Get
            Return IIf((CtrlDescription Is Nothing), String.Empty, CtrlDescription.Text)
        End Get
        Set(ByVal value As String)
            If CtrlDescription IsNot Nothing Then
                CtrlDescription.Text = value
            End If
        End Set
    End Property
#End Region

#Region "Behaviour"
    Public Property Mode() As EControlMode
        Get
            Return CType(CTextbox.GetInteger(CtrlHidden.Value), EControlMode)
        End Get
        Set(ByVal value As EControlMode)
            CtrlHidden.Value = CInt(value).ToString
        End Set
    End Property
    Public Property Required() As Boolean
        Get
            Return CtrlValidator.Enabled
        End Get
        Set(ByVal value As Boolean)
            CtrlValidator.Enabled = value
        End Set
    End Property
    Public Property RequiredCustom() As Boolean
        Get
            Return CtrlValidatorCustom.Enabled
        End Get
        Set(ByVal value As Boolean)
            CtrlValidatorCustom.Enabled = value
        End Set
    End Property

    'Derived
    Public Property Disabled() As Boolean
        Get
            Return Mode = EControlMode.Disabled
        End Get
        Set(ByVal value As Boolean)
            Enabled = Not value
        End Set
    End Property
    Public Property Enabled() As Boolean
        Get
            Return Not Disabled
        End Get
        Set(ByVal value As Boolean)
            If value Then Mode = EControlMode.Editable Else Mode = EControlMode.Disabled
        End Set
    End Property
    Public ReadOnly Property Locked() As Boolean
        Get
            Return Mode = EControlMode.Locked
        End Get
    End Property
#End Region

#Region "Abstract/Virtual"
    'Presentation Logic
    Protected MustOverride Function GetLockedText() As String
    Protected MustOverride ReadOnly Property DefaultFormatRequiredText() As String

    Protected MustOverride WriteOnly Property SetEnabled() As Boolean
    Public MustOverride Property ToolTip() As String

    'Active Controls
    Protected MustOverride ReadOnly Property CtrlLabel() As Literal
    Protected MustOverride ReadOnly Property CtrlLocked() As Label
    Protected MustOverride ReadOnly Property CtrlMain() As Control
    Protected MustOverride ReadOnly Property CtrlValidator() As BaseValidator
    Protected MustOverride ReadOnly Property CtrlValidatorCustom() As CustomValidator
    Protected MustOverride ReadOnly Property CtrlHidden() As HiddenField
    'Optional
    Protected Overridable ReadOnly Property CtrlDescription() As Label
        Get
            Return Nothing
        End Get
    End Property
    Protected Overridable ReadOnly Property CtrlValidatorScript() As PlaceHolder
        Get
            Return Nothing
        End Get
    End Property

    'Layout Controls
    Protected MustOverride ReadOnly Property CtrlContainerBegin() As CCustomControlContainer
    Protected MustOverride ReadOnly Property CtrlContainerEnd() As CCustomControlContainer
    Protected MustOverride ReadOnly Property CtrlSeparator1() As CCustomControlContainer
    Protected MustOverride ReadOnly Property CtrlSeparator2() As CCustomControlContainer
#End Region

#Region "Private"
    Private Sub RestoreMode()
        'in lieu of viewstate
        If Not Page.IsPostBack Then
            Exit Sub
        End If

        Dim key As String = CtrlHidden.UniqueID
        Dim value As String = Request.Form(key)
        Dim mode As Integer = 0
        If Integer.TryParse(value, mode) Then
            Me.Mode = DirectCast(mode, EControlMode)
        End If
    End Sub
#End Region

#Region "Shared"
    'Editable/Disabled/Locked
    Public Shared Sub SetControlMode(ByVal parent As Control, ByVal mode As EControlMode)
        If TypeOf parent Is CCustomControl Then
            DirectCast(parent, CCustomControl).Mode = mode
        End If
        For Each i As Control In parent.Controls
            SetControlMode(i, mode)
        Next
    End Sub
#End Region

End Class
