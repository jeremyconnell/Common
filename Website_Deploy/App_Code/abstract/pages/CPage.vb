Imports Microsoft.VisualBasic

Public MustInherit Class CPage : Inherits System.Web.UI.Page

#Region "Default Events"
    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.Layout = CConfig.layout
        PageInit()
    End Sub
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack then Exit Sub
        PageLoad()
    End Sub
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        PagePreRender()
    End Sub
#End Region

#Region "Virtual"
    'Called Automatically
    Protected Overridable Sub PageInit()
    End Sub
    Protected Overridable Sub PageLoad()
    End Sub
    Protected Overridable Sub PagePreRender()
    End Sub

    'Name standardisation only
    Protected Overridable Sub PageSave()
    End Sub
#End Region

#Region "MasterPage"
    'Casting
    Public Shadows ReadOnly Property Master() As CMasterPage
        Get
            Return CType(MyBase.Master, CMasterPage)
        End Get
    End Property

    'Shortcuts
    Protected Property PageHeading() As String
        Get
            Return Master.PageHeading
        End Get
        Set(ByVal value As String)
            Master.PageHeading = value
        End Set
    End Property
    Protected Property MenuTitle() As String
        Get
            Return Master.MenuTitle
        End Get
        Set(ByVal value As String)
            Master.MenuTitle = value
        End Set
    End Property
    Protected Property MenuDataSourceId() As String
        Get
            Return Master.MenuDataSourceId
        End Get
        Set(ByVal value As String)
            Master.MenuDataSourceId = value
        End Set
    End Property
    Protected Property MenuSideDataSourceId() As String
        Get
            Return Master.MenuSideDataSourceID
        End Get
        Set(ByVal value As String)
            Master.MenuSideDataSourceID = value
        End Set
    End Property
    Protected Property MenuSelected() As String
        Get
            Return Master.MenuSelected
        End Get
        Set(ByVal value As String)
            Master.MenuSelected = value
        End Set
    End Property
    Public Sub UnbindMenu()
        Master.UnBindMenu()
    End Sub
    Public Sub UnbindSideMenu()
        Master.UnbindSideMenu()
    End Sub
    Public Sub AddButton(ByVal url As String, ByVal toolTip As String)
        Master.AddButton(url, toolTip)
    End Sub



    Public Sub AddMenuLeft(ByVal name As String, ByVal url As String, ByVal selected As Boolean, ByVal tooltip As String)
        Master.AddMenuLeft(name, url, selected, tooltip)
    End Sub
    Public Sub AddMenuRight(ByVal name As String, ByVal url As String, ByVal selected As Boolean, ByVal tooltip As String)
        Master.AddMenuRight(name, url, selected, tooltip)
    End Sub
    Public Sub AddMenuSide(ByVal name As String, ByVal url As String, ByVal selected As Boolean, ByVal tooltip As String)
        Master.AddMenuSide(name, url, selected, tooltip)
    End Sub
    Public Sub AddLinkSide(ByVal name As String, ByVal url As String, selected As Boolean, tooltip As String)
        Master.AddLinkSide(name, url, selected, tooltip)
    End Sub
    Public Sub AddLinkSide(ByVal name As String, ByVal onclick As EventHandler, Optional selected As Boolean = False, Optional tooltip As String = "")
        Master.AddLinkSide(name, onclick, selected, tooltip)
    End Sub

    'Overloads (shorter)
    Public Sub AddMenuLeft()
        AddMenuLeft(Page.Title)
    End Sub
    Public Sub AddMenuLeft(ByVal name As String)
        AddMenuLeft(name, Request.RawUrl, True, name)
    End Sub
    Public Sub AddMenuLeft(ByVal name As String, ByVal url As String)
        AddMenuLeft(name, url, False)
    End Sub
    Public Sub AddMenuLeft(ByVal name As String, ByVal url As String, ByVal selected As Boolean)
        AddMenuLeft(name, url, selected, name)
    End Sub

    Public Sub AddMenuRight()
        AddMenuRight(Page.Title)
    End Sub
    Public Sub AddMenuRight(ByVal name As String)
        AddMenuRight(name, Request.RawUrl, True, name)
    End Sub
    Public Sub AddMenuRight(ByVal name As String, ByVal url As String)
        AddMenuRight(name, url, False)
    End Sub
    Public Sub AddMenuRight(ByVal name As String, ByVal url As String, ByVal selected As Boolean)
        AddMenuRight(name, url, selected, name)
    End Sub

    Public Sub AddMenuSide()
        AddMenuSide(Page.Title)
    End Sub
    Public Sub AddMenuSide(ByVal name As String)
        AddMenuSide(name, Request.RawUrl, True, name)
    End Sub
    Public Sub AddMenuSide(ByVal name As String, ByVal url As String)
        AddMenuSide(name, url, False)
    End Sub
    Public Sub AddMenuSide(ByVal name As String, ByVal url As String, ByVal selected As Boolean)
        AddMenuSide(name, url, selected, name)
    End Sub

    Public Sub AddLinkSide(ByVal name As String, ByVal url As String)
        AddLinkSide(name, url, String.Empty)
    End Sub
    Public Sub AddLinkSide(ByVal name As String, ByVal url As String, tooltip As String)
        AddLinkSide(name, url, False, tooltip)
    End Sub
    Public Sub AddLinkSide(ByVal name As String, ByVal url As String, selected As Boolean)
        AddLinkSide(name, url, selected, String.Empty)
    End Sub
#End Region

#Region "Utilities"
    Public Property PageTitle() As String
        Get
            Return MyBase.Title
        End Get
        Set(ByVal value As String)
            MyBase.Title = value
            Master.PageHeading = value
        End Set
    End Property
#End Region

#Region "Custom Controls - Mode/Layout"
    'Page - Editable/Disabled/Locked
    Public Overridable WriteOnly Property Mode() As EControlMode
        Set(ByVal value As EControlMode)
            SetControlMode(Me, value)
        End Set
    End Property
    'Page - Horizontal/Vertical/Flow
    Public WriteOnly Property Layout() As ELayout
        Set(ByVal value As ELayout)
            SetLayout(Me, value)
        End Set
    End Property

    'Overloads for smaller subsets
    Public Sub SetControlMode(ByVal parent As Control, ByVal value As EControlMode)
        CCustomControl.SetControlMode(Me, value)
    End Sub
    Public Sub SetLayout(ByVal parent As Control, ByVal value As ELayout)
        CCustomControlContainer.SetLayout(Me, value)
    End Sub
#End Region

End Class
