Imports System.Data
Imports SchemaMembership

Partial Public Class pages_Clicks_default : Inherits CPage

#region "Querystring (Filters)"
    Public ReadOnly Property UserName() As String
        Get
            If IsNothing(Request.QueryString("userName")) Then Return "*"
            Return CWeb.RequestStr("userName")
        End Get
    End Property
    Public ReadOnly Property Url() As String
        Get
            Return Request.QueryString("Url")
        End Get
    End Property
#End Region


#Region "Members"
    Private m_clicks As CClickList
#End Region

#Region "Data"
    Public ReadOnly Property [Clicks]() As CClickList
        Get
            If IsNothing(m_clicks) Then
                m_clicks = New CClick().SelectSearch(ctrlClicks.PagingInfo, ddUserName.SelectedValue, ddUrl.SelectedValue) 'Sql-based Paging
                'm_clicks.PreloadChildren() 'Loads children for page in one hit (where applicable)
            End If
            Return m_clicks
        End Get
    End Property
    Public Function ClicksAsDataset() As System.Data.DataSet
        Return (New CClick()).SelectSearch_Dataset(ddUserName.SelectedValue, ddUrl.SelectedValue)
    End Function
#End Region

#Region "Overrides"
    Protected Overrides Sub PageInit
        'Populate Dropdowns
        CDropdown.Add(ddUserName, "-- Any User --", "*")
        For Each i As CNameValue In New CClick().UserNamesAndClickCounts()
            CDropdown.Add(ddUserName, CUtilities.NameAndCount(IIf(Len(i.Name) > 0, i.Name, "Anonymous"), CInt(i.Value)), i.Name)
        Next
        CDropdown.SetValue(ddUserName, Me.UserName)

        For Each i As CNameValue In New CClick().UrlsAndClickCounts()
            CDropdown.Add(ddUrl, CUtilities.NameAndCount(i.Name, CInt(i.Value)), i.Name)
        Next
        CDropdown.BlankItem(ddUrl, "-- Any Url --")
        CDropdown.SetValue(ddUrl, Me.Url)
        

        UnbindSideMenu()
        AddMenuSide("Sessions", CSitemap.Sessions(UserName))
        AddMenuSide("Clicks")
    End Sub
    Protected Overrides Sub PagePreRender()
        ctrlClicks.Display(Me.Clicks, chkHost.Checked)
    End Sub
#End Region

#Region "Event Handlers - Form"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddUserName.SelectedIndexChanged, ddUrl.SelectedIndexChanged
        Response.Redirect(CSitemap.Clicks(ddUserName.SelectedValue, ddUrl.SelectedValue))
    End Sub
#End Region

#Region "Event Handlers - UserControl"
    Private Sub ctrl_ExportClick() Handles ctrlClicks.ExportClick
        CDataSrc.ExportToCsv(ClicksAsDataset(), Response, "Clicks.csv")
    End Sub
    Private Sub ctrl_ResortClick(ByVal sortBy As String, ByVal descending As Boolean, ByVal pageNumber As Integer) Handles ctrlClicks.ResortClick
        Response.Redirect(CSitemap.Clicks(ddUserName.SelectedValue, ddUrl.SelectedValue, New CPagingInfo(0, pageNumber - 1, sortBy, descending)))
    End Sub
#End Region

End Class
