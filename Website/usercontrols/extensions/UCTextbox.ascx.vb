
Partial Class usercontrols_extensions_UCTextbox : Inherits CCustomControl



#Region "Enums"
    Public Enum ETextMode
        [String]
        [Integer]
        [Number]
        [Money]
        [Date]
        [Percent]
    End Enum
#End Region

#Region "Members"
    Private m_textMode As ETextMode = ETextMode.String
#End Region

#Region "Appearance"
    Public Property IsPassword() As Boolean
        Get
            Return ctrl.TextMode = TextBoxMode.Password
        End Get
        Set(ByVal value As Boolean)
            If value Then
                ctrl.TextMode = TextBoxMode.Password
            Else
                ctrl.TextMode = TextBoxMode.SingleLine
            End If
        End Set
    End Property
    Public Property Width() As Unit
        Get
            Return ctrl.Width
        End Get
        Set(ByVal value As Unit)
            ctrl.Width = value
        End Set
    End Property
    Public Property NavigateUrl() As String
        Get
            Return lnk.NavigateUrl
        End Get
        Set(ByVal value As String)
            lnk.NavigateUrl = value
        End Set
    End Property
    Public Property ImageUrl() As String
        Get
            Return lnk.ImageUrl
        End Get
        Set(ByVal value As String)
            lnk.NavigateUrl = value
        End Set
    End Property
    Public Property Target() As String
        Get
            Return lnk.Target
        End Get
        Set(ByVal value As String)
            lnk.Target = value
        End Set
    End Property
    Public Property CssClass() As String
        Get
            Return Textbox.CssClass
        End Get
        Set(ByVal value As String)
            Textbox.CssClass = value
        End Set
    End Property
    Public Sub RightAlign()
        CTextbox.RightAlign(Textbox)
    End Sub
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
            Return ctrl.MaxLength
        End Get
        Set(ByVal value As Integer)
            ctrl.MaxLength = value
        End Set
    End Property
    Public Property TextMode() As ETextMode
        Get
            Return m_textMode
        End Get
        Set(ByVal value As ETextMode)
            m_textMode = value
        End Set
    End Property
    Public Sub OnReturnPress(ByVal btn As WebControl)
        CTextbox.OnReturnPress(ctrl, btn)
    End Sub
    Public Sub OnFocusSelect()
        CTextbox.OnFocusSelect(ctrl)
    End Sub
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
    Public Property ValueInt() As Integer
        Get
            Return CTextbox.GetInteger(ctrl)
        End Get
        Set(ByVal value As Integer)
            CTextbox.SetNumber(ctrl, value)
        End Set
    End Property
    Public Property ValueLong() As Long
        Get
            Return CTextbox.GetLong(ctrl)
        End Get
        Set(ByVal value As Long)
            CTextbox.SetNumber(ctrl, value)
        End Set
    End Property
    Public Property ValueDbl() As Double
        Get
            Return CTextbox.GetNumber(ctrl)
        End Get
        Set(ByVal value As Double)
            CTextbox.SetNumber(ctrl, value)
        End Set
    End Property
    Public Property ValueDate() As DateTime
        Get
            'Return CTextbox.GetDate(ctrl)

            'Javascript Datepicker has a specific date format
            If String.IsNullOrEmpty(Text) OrElse Not Text.Contains("-") Then Return DateTime.MinValue
            Dim ss As String() = Text.Split("-")
            Try
                Return New DateTime(Integer.Parse(ss(2)), Integer.Parse(ss(0)), Integer.Parse(ss(1)))
            Catch ex As Exception
                Return DateTime.MinValue
            End Try
        End Get
        Set(ByVal value As DateTime)
            CTextbox.SetDate(ctrl, value, "MM-dd-yyyy")
        End Set
    End Property
    Public Property ValueDateTime() As DateTime
        Get
            Return CTextbox.GetDate(ctrl)
        End Get
        Set(ByVal value As DateTime)
            ctrl.Text = CUtilities.LongDateTime(value)
        End Set
    End Property
    Public Property ValueMoney() As Decimal
        Get
            Return CTextbox.GetMoney(ctrl)
        End Get
        Set(ByVal value As Decimal)
            CTextbox.SetMoney(ctrl, value)
        End Set
    End Property
    Public Property ValueMoneyAsDbl() As Double
        Get
            Dim d As Decimal = CTextbox.GetMoney(ctrl)
            If d = Decimal.MinValue Then Return Double.NaN
            Return CDbl(d)
        End Get
        Set(ByVal value As Double)
            Dim d As Decimal = Decimal.MinValue
            If Not Double.IsNaN(value) Then d = CDec(value)
            ValueMoney = d
        End Set
    End Property
    Public Property ValuePercent() As Double
        Get
            Return CTextbox.GetPercent(ctrl)
        End Get
        Set(ByVal value As Double)
            CTextbox.SetPercent(ctrl, value)
        End Set
    End Property
#End Region

#Region "Control"
    Public ReadOnly Property Textbox() As TextBox
        Get
            Return ctrl
        End Get
    End Property
    Public ReadOnly Property Hyperlink() As HyperLink
        Get
            Return lnk
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
            Return "'{0}' is required"
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
            lnk.ToolTip = value
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

    Public ReadOnly Property CtrlLocked_ As Label
        Get
            Return CtrlLocked
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

#Region "Event Handlers"
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Select Case TextMode
            Case ETextMode.Percent
                RightAlign()
                ctrl.Attributes.Add("onKeyDown", "return ValidatePercentage(this) || IncrementOrDecrementNumber(this)")
                ctrl.Attributes.Add("onBlur", "BlurPercent(this)")

            Case ETextMode.Number
                CTextbox.RightAlign(ctrl)
                ctrl.Attributes.Add("onKeyDown", "return ValidateNumber(this) || IncrementOrDecrementNumber(this)")
                ctrl.Attributes.Add("onBlur", "BlurNumber(this)")

            Case ETextMode.Integer
                CTextbox.RightAlign(ctrl)
                ctrl.Attributes.Add("onKeyDown", "return ValidateInteger(this) || IncrementOrDecrementNumber(this)")
                ctrl.Attributes.Add("onBlur", "BlurNumber(this)")

            Case ETextMode.Money
                CTextbox.RightAlign(ctrl)
                ctrl.Attributes.Add("onKeyDown", "return ValidateMoney(this) || IncrementOrDecrementNumber(this)")
                ctrl.Attributes.Add("onBlur", "BlurMoney(this)")

            Case ETextMode.Date
                plhDate.Visible = True
                ctrl.Attributes.Add("datepicker", "true")
                ctrl.Attributes.Add("datepicker_format", "MM-DD-YYYY")
                ctrl.Width = New Unit(110)
        End Select

        If Len(NavigateUrl) > 0 Then
            lnk.Visible = True
            lnk.Text = Value
            Value = String.Empty
            ctrl.Visible = False
        End If
    End Sub
#End Region

End Class
