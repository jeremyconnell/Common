
Partial Class usercontrols_extensions_UCCheckbox : Inherits CCustomControl

#Region "Events"
    Public Event CheckedChanged()
#End Region

#Region "Event Handlers"
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'Unique name for custom javascript function
        rfv.ClientValidationFunction = String.Concat("Validate_", ctrl.ClientID)
    End Sub
#End Region

#Region "Members"
    Private m_trueCase As String = "Yes"
    Private m_falseCase As String = "No"
#End Region

#Region "Appearance"
    'Checkbox text (optional)
    Public Property Text() As String
        Get
            Return ctrl.Text
        End Get
        Set(ByVal value As String)
            ctrl.Text = value
        End Set
    End Property
    Public Property TrueCase() As String
        Get
            Return m_trueCase
        End Get
        Set(ByVal value As String)
            m_trueCase = value
        End Set
    End Property
    Public Property FalseCase() As String
        Get
            Return m_falseCase
        End Get
        Set(ByVal value As String)
            m_falseCase = value
        End Set
    End Property

    Public Overloads Property Description() As String
        Get
            Return Me.lblDescription.Text
        End Get
        Set(ByVal value As String)
            Me.lblDescription.Text = "(" & value & ")"
        End Set
    End Property

#End Region

#Region "Behaviour"
    Public Property AutoPostBack() As Boolean
        Get
            Return ctrl.AutoPostBack
        End Get
        Set(ByVal value As Boolean)
            ctrl.AutoPostBack = value
        End Set
    End Property
#End Region

#Region "Value"
    Public Property Checked() As Boolean
        Get
            Return ctrl.Checked
        End Get
        Set(ByVal value As Boolean)
            ctrl.Checked = value
        End Set
    End Property
    Public Property Value() As Boolean
        Get
            Return ctrl.Checked
        End Get
        Set(ByVal value As Boolean)
            ctrl.Checked = value
        End Set
    End Property
#End Region

#Region "Control"
    Public ReadOnly Property Checkbox() As CheckBox
        Get
            Return ctrl
        End Get
    End Property
#End Region

#Region "MustOverrides"
    'Logic/Presentation
    Protected Overrides Function GetLockedText() As String
        If Checkbox.Checked Then
            Return TrueCase
        Else
            Return FalseCase
        End If
    End Function
    Protected Overrides ReadOnly Property DefaultFormatRequiredText() As String
        Get
            Return "'{0}' must be checked"
        End Get
    End Property
    Protected Overrides WriteOnly Property SetEnabled() As Boolean
        Set(ByVal value As Boolean)
            ctrl.Enabled = value
        End Set
    End Property
    Public Overrides Property ToolTip() As String
        Get
            Return ctrl.ToolTip
        End Get
        Set(ByVal value As String)
            ctrl.ToolTip = value
            _l.ToolTip = value
        End Set
    End Property

    'Controls - Active
    Protected Overrides ReadOnly Property CtrlMain() As Control
        Get
            Return ctrl
        End Get
    End Property
    Protected Overrides ReadOnly Property CtrlLabel() As Literal
        Get
            Return litLabel
        End Get
    End Property
    Protected Overrides ReadOnly Property CtrlLocked() As Label
        Get
            Return _l
        End Get
    End Property
    Protected Overrides ReadOnly Property CtrlValidator() As BaseValidator
        Get
            Return rfv
        End Get
    End Property
    Protected Overrides ReadOnly Property CtrlValidatorCustom() As CustomValidator
        Get
            Return cv
        End Get
    End Property
    Protected Overrides ReadOnly Property CtrlValidatorScript() As PlaceHolder
        Get
            Return plhScript
        End Get
    End Property
    Protected Overrides ReadOnly Property CtrlHidden() As System.Web.UI.WebControls.HiddenField
        Get
            Return _h
        End Get
    End Property

    'Controls - Container
    Protected Overrides ReadOnly Property CtrlContainerBegin() As CCustomControlContainer
        Get
            Return _st
        End Get
    End Property
    Protected Overrides ReadOnly Property CtrlContainerEnd() As CCustomControlContainer
        Get
            Return _et
        End Get
    End Property
    Protected Overrides ReadOnly Property CtrlSeparator1() As CCustomControlContainer
        Get
            Return _s1
        End Get
    End Property
    Protected Overrides ReadOnly Property CtrlSeparator2() As CCustomControlContainer
        Get
            Return _s2
        End Get
    End Property
#End Region

#Region "Event Handlers"
    Protected Sub ctrl_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ctrl.CheckedChanged
        RaiseEvent CheckedChanged()
    End Sub
#End Region

End Class
