Partial Class pages_Sessions_Session : Inherits CPage

#Region "Querystring"
    Public ReadOnly Property SessionId() As Integer
        Get
            Return CWeb.RequestInt("sessionId")
        End Get
    End Property
    Public ReadOnly Property IsEdit() As Boolean
        Get
            Return SessionId <> Integer.MinValue
        End Get
    End Property
#End Region

#Region "Members"
    Private m_session As SchemaMembership.CSession
#End Region

#Region "Data"
    Public Shadows ReadOnly Property [Session]() As SchemaMembership.CSession
        Get
            If IsNothing(m_session) Then
                If IsEdit Then
                    m_session = New SchemaMembership.CSession(SessionId)
                    If IsNothing(m_session) Then CSitemap.RecordNotFound("Session", SessionId)
                Else
                    m_session = New SchemaMembership.CSession
                    'Inserts: set parentId here (if applicable)
                End If
            End If
            Return m_session
        End Get
    End Property
#End Region

#Region "Navigation"
    Private Sub Refresh()
        Response.Redirect(CSitemap.SessionEdit(Me.Session.SessionId))
    End Sub
    Private Sub ReturnToList()
        Response.Redirect(CSitemap.Sessions)
    End Sub
#End Region

#Region "Event Handlers - Page"
    Protected Overrides Sub PageInit()

        PageHeading = String.Concat("Session Clicks - ", IIf(Len(Session.SessionUserLoginName) > 0, Session.SessionUserLoginName, "Anonymous"), " (", Session.MinDate.ToString("ddd d MMM H:mmtt"), ")")

        UnbindSideMenu()
        AddMenuSide("Sessions", CSitemap.Sessions)
        AddMenuSide("Session")
        AddMenuSide("Clicks", CSitemap.Clicks())

        ctrlClicks.Display(Session)

        'Page Title
        If IsEdit Then
            Me.Title = "Session Details"
        Else
            Me.Title = "Create New Session"
        End If
    End Sub
#End Region

End Class
