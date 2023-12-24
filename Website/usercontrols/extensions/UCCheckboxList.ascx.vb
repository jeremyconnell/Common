
Partial Class usercontrols_extensions_UCCheckboxList : Inherits CCustomControl

#Region "Events"
    Public Event SelectedIndexChanged()
#End Region

#Region "Event Handlers"
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        rfv.ClientValidationFunction = String.Concat("Validate_", ctrl.ClientID)
    End Sub
#End Region

#Region "Members"
    Private m_noItems As String = "None"
#End Region

#Region "Appearance"
    Public Property RepeatDirection() As RepeatDirection
        Get
            Return ctrl.RepeatDirection
        End Get
        Set(ByVal value As RepeatDirection)
            ctrl.RepeatDirection = value
        End Set
    End Property
    Public Property RepeatLayout() As RepeatLayout
        Get
            Return ctrl.RepeatLayout
        End Get
        Set(ByVal value As RepeatLayout)
            ctrl.RepeatLayout = value
        End Set
    End Property
    Public Property RepeatColumns() As Integer
        Get
            Return ctrl.RepeatColumns
        End Get
        Set(ByVal value As Integer)
            ctrl.RepeatColumns = value
        End Set
    End Property
    Public Property NoItemsText() As String
        Get
            Return m_noItems
        End Get
        Set(ByVal value As String)
            m_noItems = value
        End Set
	End Property

	Public Property CellPadding() As Integer
		Get
			Return ctrl.CellPadding
		End Get
		Set(ByVal value As Integer)
			ctrl.CellPadding = value
		End Set
	End Property
	Public Property CellSpacing() As Integer
		Get
			Return ctrl.CellSpacing
		End Get
		Set(ByVal value As Integer)
			ctrl.CellSpacing = value
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
#End Region

#Region "Helpers - Add/Remove"
    Public Sub Clear()
        ctrl.Items.Clear()
    End Sub
    Public Function Add(ByVal text As String) As ListItem
        Return CDropdown.Add(ctrl, text)
    End Function
    Public Function Add(ByVal text As String, ByVal value As Integer) As ListItem
        Return CDropdown.Add(ctrl, text, value)
    End Function
    Public Function Add(ByVal text As String, ByVal value As String) As ListItem
        Return CDropdown.Add(ctrl, text, value)
    End Function
    Public Sub SelectAll()
        CDropdown.SelectAll(CheckBoxList)
    End Sub
#End Region

#Region "Helpers - Binding"
    Public Property DataTextField() As String
        Get
            Return ctrl.DataTextField
        End Get
        Set(ByVal value As String)
            ctrl.DataTextField = value
        End Set
    End Property
    Public Property DataValueField() As String
        Get
            Return ctrl.DataValueField
        End Get
        Set(ByVal value As String)
            ctrl.DataValueField = value
        End Set
    End Property
    Public WriteOnly Property DataSource() As Object
        Set(ByVal value As Object)
            ctrl.DataSource = value
        End Set
    End Property
    Public Shadows Sub DataBind()
        ctrl.SelectedValue = Nothing
        ctrl.DataBind()
    End Sub
    Public Shadows Sub DataBind(ByVal textAndValueField As String)
        DataTextField = textAndValueField
        DataValueField = textAndValueField
        DataBind()
    End Sub
    Public Shadows Sub DataBind(ByVal textField As String, ByVal valueField As String)
        DataTextField = textField
        DataValueField = valueField
        DataBind()
    End Sub
#End Region

#Region "Value"
    Public Property SelectedValues() As ICollection
        Get
            Return SelectedStrings
        End Get
        Set(ByVal value As ICollection)
            CDropdown.SetValues(ctrl, value)
        End Set
    End Property
    Public Property SelectedStrings() As List(Of String)
        Get
            Return CDropdown.SelectedValues(ctrl)
        End Get
        Set(ByVal value As List(Of String))
            SelectedValues = value
        End Set
    End Property
    Public Property SelectedInts() As List(Of Integer)
        Get
            Return CDropdown.SelectedInts(ctrl)
        End Get
        Set(ByVal value As List(Of Integer))
            CDropdown.SetValues(ctrl, value)
        End Set
    End Property
    Public Property SelectedGuids() As List(Of Guid)
        Get
            Return CDropdown.SelectedGuids(ctrl)
        End Get
        Set(ByVal value As List(Of Guid))
            CDropdown.SetValues(ctrl, value)
        End Set
    End Property
#End Region

#Region "Control"
    Public ReadOnly Property CheckBoxList() As CheckBoxList
        Get
            Return ctrl
        End Get
    End Property
    Public ReadOnly Property Items() As ListItemCollection
        Get
            Return ctrl.Items
        End Get
    End Property
#End Region

#Region "MustOverrides"
    'Presentation Logic
    Protected Overrides Function GetLockedText() As String
        Dim sb As New StringBuilder()
        For Each i As ListItem In ctrl.Items
            If Not i.Selected Then Continue For
            If sb.Length > 0 Then sb.Append(", ")
            sb.Append(i.Text)
        Next
        If sb.Length = 0 Then sb.Append(NoItemsText)
        Return sb.ToString
    End Function
    Protected Overrides ReadOnly Property DefaultFormatRequiredText() As String
        Get
            Return "At least one '{0}' must be checked"
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
    Protected Sub ctrl_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ctrl.SelectedIndexChanged
        If CWeb.RequestStr("__EVENTTARGET").StartsWith(Me.UniqueID) Then RaiseEvent SelectedIndexChanged()
    End Sub
#End Region

End Class
