
Partial Class usercontrols_extensions_UCTextArea : Inherits CCustomControl

#Region "Members"
    Private m_maxLength As Integer = Integer.MinValue
#End Region

#Region "String Formatting"
    Public Enum EFormat 'Not yet used
        [String] = 1
        [Email] = 2
        [Phone] = 3
        [Currency] = 4
        [Integer] = 5
        [Float] = 6
        [Url] = 7
    End Enum
#End Region

#Region "Appearance"
    Public Property Width() As Unit
        Get
            Return ctrl.Width
        End Get
        Set(ByVal value As Unit)
            ctrl.Width = value
        End Set
    End Property
    Public Property Height() As Unit
        Get
            Return ctrl.Height
        End Get
        Set(ByVal value As Unit)
            ctrl.Height = value
        End Set
    End Property
#End Region

#Region "Behaviour"
    Public Property AutoPostback() As Boolean
        Get
            Return ctrl.AutoPostBack
        End Get
        Set(ByVal value As Boolean)
            ctrl.AutoPostBack = value
        End Set
    End Property
    Public Property MaxLength() As Integer
        Get
            Return m_maxLength
        End Get
        Set(ByVal value As Integer)
            m_maxLength = value
            valMaxLength.Visible = value > 0
        End Set
    End Property
    Public Property Wrap() As Boolean
        Get
            Return ctrl.Wrap
        End Get
        Set(ByVal value As Boolean)
            ctrl.Wrap = value
        End Set
    End Property
#End Region

#Region "Value"
    Public Property Text() As String
        Get
            Return ctrl.Text
        End Get
        Set(ByVal value As String)
            ctrl.Text = value
        End Set
    End Property
    Public Property Value() As String
        Get
            Return ctrl.Text
        End Get
        Set(ByVal value As String)
            ctrl.Text = value
        End Set
    End Property
#End Region

#Region "Control"
    Public ReadOnly Property Textarea() As TextBox
        Get
            Return ctrl
        End Get
    End Property
#End Region

#Region "MustOverrides"
    'Presentation Logic
    Protected Overrides Function GetLockedText() As String
        Return ctrl.Text
    End Function
    Protected Overrides ReadOnly Property DefaultFormatRequiredText() As String
        Get
            Return "Some text must be entered for '{0}'"
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
    Protected Overrides ReadOnly Property CtrlHidden() As System.Web.UI.WebControls.HiddenField
        Get
            Return _h
        End Get
    End Property
    Protected Overrides ReadOnly Property CtrlDescription() As Label
        Get
            Return lblDescription
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

#Region "Validation"
    Protected Sub valMaxLength_ServerValidate(ByVal source As Object, ByVal args As ServerValidateEventArgs) Handles valMaxLength.ServerValidate
        If Me.Value.Length <= MaxLength Then
            args.IsValid = True
        Else
            args.IsValid = False
            valMaxLength.ErrorMessage = String.Concat("Warning: ", Label, " data was truncated because MaxLength (", MaxLength, ") was been exceeded")
            Textarea.Text = CUtilities.Truncate(Textarea.Text, MaxLength)
        End If
    End Sub
#End Region

#Region "Event Handlers"
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        plhScript.Visible = Not Wrap
        If Wrap Then
            Textarea.Attributes.Remove("onKeyDown")
        Else
            Textarea.Attributes("onKeyDown") = "return UCTextarea_AllowTab(this, event)"
        End If
    End Sub
#End Region

End Class
