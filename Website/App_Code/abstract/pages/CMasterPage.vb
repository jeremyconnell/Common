Imports Microsoft.VisualBasic

Public MustInherit Class CMasterPage : Inherits MasterPage

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If String.IsNullOrEmpty(PageHeading) Then PageHeading = Page.Title
        If String.IsNullOrEmpty(MenuTitle) Then MenuTitle = CConfig.MenuTitle
        If Main.Controls.Count = 0 AndAlso Not IsNothing(Fieldset) Then Fieldset.Visible = False
        If Not Page.IsPostBack AndAlso CSession.IsLoggedIn Then SchemaMembership.CClick.Log()
    End Sub

    Public MustOverride ReadOnly Property Main() As ContentPlaceHolder
    Public MustOverride ReadOnly Property Fieldset() As HtmlControls.HtmlGenericControl

    Public MustOverride Property PageHeading() As String
    Public MustOverride Sub AddMenuLeft(ByVal name As String, ByVal url As String, ByVal selected As Boolean, ByVal tooltip As String)
    Public MustOverride Sub AddMenuRight(ByVal name As String, ByVal url As String, ByVal selected As Boolean, ByVal tooltip As String)
    Public MustOverride Sub AddMenuSide(ByVal name As String, ByVal url As String, ByVal selected As Boolean, ByVal tooltip As String)
    Public MustOverride Sub AddLinkSide(ByVal name As String, ByVal url As String, ByVal selected As Boolean, ByVal tooltip As String)

    Public MustOverride Property MenuTitle() As String
    Public MustOverride Property MenuDataSourceId() As String
    Public MustOverride Property MenuSideDataSourceID() As String
    Public MustOverride Property MenuSelected() As String
    Public Sub UnBindMenu()
        MenuDataSourceId = String.Empty
        MenuSideDataSourceID = String.Empty
    End Sub
    Public Sub UnbindSideMenu()
        MenuSideDataSourceID = String.Empty
    End Sub

    Public MustOverride Sub AddButton(ByVal url As String, ByVal tooltip As String)
End Class
