Imports System.Data
Imports SchemaMembership

Partial Public Class pages_Sessions_default : Inherits CPage

#region "Querystring (Filters)"
    Public ReadOnly Property Search As String
        Get
            If IsNothing(Request.QueryString("search")) Then Return "*"
            Return CWeb.RequestStr("search")
        End Get
    End Property
#End Region

#Region "Members"
    Private m_sessions As CSessionList
#End Region

#Region "Data"
    Public ReadOnly Property [Sessions]() As CSessionList
        Get
            If IsNothing(m_sessions) Then
                m_sessions = New SchemaMembership.CSession().SelectSearch(ctrlSessions.PagingInfo, ddUserName.SelectedValue) 'Sql-based Paging
                'm_sessions.PreloadChildren() 'Loads children for page in one hit (where applicable)
            End If
            Return m_sessions
        End Get
    End Property
    Public Function SessionsAsDataset() As System.Data.DataSet
        Return (New SchemaMembership.CSession()).SelectSearch_Dataset(ddUserName.SelectedValue)
    End Function
#End Region

#Region "Overrides"
    Protected Overrides Sub PageInit
        'Populate Dropdowns
        CDropdown.Add(ddUserName, "-- Any User --", "*")
        For Each i As CNameValue In New SchemaMembership.CSession().UserNamesAndSessionCounts()
            CDropdown.Add(ddUserName, CUtilities.NameAndCount(IIf(Len(i.Name) > 0, i.Name, "Anonymous"), CInt(i.Value)), i.Name)
        Next
        CDropdown.SetValue(ddUserName, Me.Search)


        'Display Results
        ctrlSessions.Display(Me.Sessions)


        UnbindSideMenu()
        AddMenuSide("Sessions")
        AddMenuSide("Clicks", CSitemap.Clicks(Search))
    End Sub
#End Region

#Region "Event Handlers - Form"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddUserName.SelectedIndexChanged
        Response.Redirect(CSitemap.Sessions(ddUserName.SelectedValue))
    End Sub
#End Region

#Region "Event Handlers - UserControl"
    Private Sub ctrl_ExportClick() Handles ctrlSessions.ExportClick
        CDataSrc.ExportToCsv(SessionsAsDataset(), Response, "Sessions.csv")
    End Sub
    Private Sub ctrl_ResortClick(ByVal sortBy As String, ByVal descending As Boolean, ByVal pageNumber As Integer) Handles ctrlSessions.ResortClick
        Response.Redirect(CSitemap.Sessions(ddUserName.SelectedValue, New CPagingInfo(0, pageNumber - 1, sortBy, descending)))
    End Sub
#End Region

End Class
