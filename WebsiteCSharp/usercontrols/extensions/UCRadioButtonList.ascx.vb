﻿
Partial Class usercontrols_extensions_UCRadioButtonList : Inherits CCustomControl

#Region "Events"
    Public Event SelectedIndexChanged()
#End Region

#Region "Members"
    Private m_noSelectionText As String = String.Empty '"-- No Selection Made --"
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
    Public Property NoSelectionText() As String
        Get
            Return m_noSelectionText
        End Get
        Set(ByVal value As String)
            m_noSelectionText = value
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
    Public Sub AddEnums(ByVal enumType As Type)
        CDropdown.AddEnums(ctrl, enumType)
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
    Public Shadows Sub DataBind(ByVal textField As String, ByVal valueField As String, ByVal blankItemText As String)
        DataBind(textField, valueField)
        BlankItem(blankItemText)
    End Sub
    Public Sub BlankItem(ByVal text As String)
        CDropdown.BlankItem(ctrl, text)
    End Sub
    Public Sub BlankItem(ByVal text As String, ByVal noSelectionText As String)
        CDropdown.BlankItem(ctrl, text)
        Me.NoSelectionText = noSelectionText
    End Sub
#End Region

#Region "Value"
    Public Property Text() As String
        Get
            If IsNothing(ctrl.SelectedItem) Then Return String.Empty
            Return ctrl.SelectedItem.Text
        End Get
        Set(ByVal value As String)
            CDropdown.SetText(ctrl, value)
        End Set
    End Property
    Public Property Value() As String
        Get
            Return ctrl.SelectedValue
        End Get
        Set(ByVal value As String)
            CDropdown.SetValue(ctrl, value)
        End Set
    End Property
    Public Property ValueInt() As Integer
        Get
            Return CDropdown.GetInt(ctrl)
        End Get
        Set(ByVal value As Integer)
            CDropdown.SetValue(ctrl, value)
        End Set
    End Property
    Public Property ValueGuid() As Guid
        Get
            Return CDropdown.GetGuid(ctrl)
        End Get
        Set(ByVal value As Guid)
            CDropdown.SetValue(ctrl, value)
        End Set
    End Property
    Public Property SelectedIndex() As Integer
        Get
            Return ctrl.SelectedIndex
        End Get
        Set(ByVal value As Integer)
            ctrl.SelectedIndex = value
        End Set
    End Property
#End Region

#Region "Control"
    Public ReadOnly Property RadioButtonList() As RadioButtonList
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
        If IsNothing(ctrl.SelectedItem) Then Return NoSelectionText
        If ctrl.SelectedValue = String.Empty Then Return NoSelectionText
        Return ctrl.SelectedItem.Text
    End Function
    Protected Overrides ReadOnly Property DefaultFormatRequiredText() As String
        Get
            Return "'{0}' must be selected"
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
