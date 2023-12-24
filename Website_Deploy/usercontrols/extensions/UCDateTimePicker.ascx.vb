
Partial Class usercontrols_extensions_UCDateTimePicker : Inherits CCustomControl

#Region "Appearance"
    Public Property Width() As String
        Get
            Return tbl.Width
        End Get
        Set(ByVal value As String)
            tbl.Width = value
        End Set
    End Property
    Public Property CssClass() As String
        Get
            Return TextboxDate.CssClass
        End Get
        Set(ByVal value As String)
            TextboxDate.CssClass = value
        End Set
    End Property
#End Region

#Region "Behaviour"
    Public Property ShowMinutes() As Boolean
        Get
            Return ctrlTime.ShowMinutes
        End Get
        Set(ByVal value As Boolean)
            ctrlTime.ShowMinutes = value
            If Not value Then ShowSeconds = False
        End Set
    End Property
    Public Property ShowSeconds() As Boolean
        Get
            Return ctrlTime.ShowSeconds
        End Get
        Set(ByVal value As Boolean)
            ctrlTime.ShowSeconds = value
        End Set
    End Property
#End Region

#Region "Value"
    Public Property DateAndTime() As DateTime
        Get
            Return Value
        End Get
        Set(ByVal value As DateTime)
            Me.Value = value
        End Set
    End Property
    Public Property Value() As DateTime
        Get
            Dim d As DateTime = CTextbox.GetDate(ctrlDate)
            If DateTime.MinValue = d Then Return d
            Return d.Date.AddHours(ctrlTime.Hours).AddMinutes(ctrlTime.Mins).AddSeconds(ctrlTime.Secs)
        End Get
        Set(ByVal value As DateTime)
            CTextbox.SetDate(ctrlDate, value)
            ctrlTime.Value = value
        End Set
    End Property
#End Region

#Region "Control"
    Public ReadOnly Property TextboxDate() As TextBox
        Get
            Return CtrlDate
        End Get
    End Property
    Public ReadOnly Property TextboxTime() As usercontrols_UCTimepicker
        Get
            Return ctrlTime
        End Get
    End Property
#End Region

#Region "MustOverrides"
    'Presentation Logic
    Protected Overrides Function GetLockedText() As String
        Return CUtilities.LongDate(Value)
    End Function
    Protected Overrides ReadOnly Property DefaultFormatRequiredText() As String
        Get
            Return "'{0}' is required"
        End Get
    End Property
    Protected Overrides WriteOnly Property SetEnabled() As Boolean
        Set(ByVal value As Boolean)
            ctrlDate.Enabled = value
            ctrlTime.Enabled = value
        End Set
    End Property
    Public Overrides Property ToolTip() As String
        Get
            Return ctrlDate.ToolTip
        End Get
        Set(ByVal value As String)
            ctrlTime.ToolTip = value
            ctrlDate.ToolTip = value
            _l.ToolTip = value
        End Set
    End Property

    'Controls - Active
    Protected Overrides ReadOnly Property CtrlMain() As Control
        Get
            Return CType(ctrl, PlaceHolder)
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

#Region "Event Handlers"
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If String.IsNullOrEmpty(ToolTip) Then ToolTip = CUtilities.LongDateTime(Value)
    End Sub
#End Region

End Class
