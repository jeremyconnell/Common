Partial Class pages_Audit_Logs_Audit_Log : Inherits CPage

#Region "Querystring"
    Public ReadOnly Property LogId() As Integer
        Get
            Return CWeb.RequestInt("logId")
        End Get
    End Property
    Public ReadOnly Property IsEdit() As Boolean
        Get
            Return LogId <> Integer.MinValue
        End Get
    End Property
#End Region

#Region "Members"
    Private m_audit_Log As CAudit_Log
#End Region

#Region "Data"
    Public ReadOnly Property [Audit_Log]() As CAudit_Log
        Get
            If IsNothing(m_audit_Log) Then
                If IsEdit Then
                    m_audit_Log = New CAudit_Log(LogId)
                    If IsNothing(m_audit_Log) Then CSitemap.RecordNotFound("Audit_Log", LogId)
                Else
                    m_audit_Log = New CAudit_Log
                    'Inserts: set parentId here (if applicable)
                End If
            End If
            Return m_audit_Log
        End Get
    End Property
#End Region

#Region "Navigation"
    Private Sub Refresh()
        Response.Redirect(CSitemap.Audit_LogEdit(Me.Audit_Log.LogId))
    End Sub
    Private Sub ReturnToList()
        Response.Redirect(CSitemap.Audit_Logs)
    End Sub
#End Region

#Region "Event Handlers - Page"
    Protected Overrides Sub PageInit()
        With ddLogTypeId
            .AddEnums(GetType(ELogType))
            .BlankItem("-- Select Type --")
        End With



        'Page Title
        If IsEdit Then
            Me.Title = "Log Details"
        Else
            Me.Title = "New Log"
        End If
    End Sub
    Protected Overrides Sub PageLoad()
        If Page.IsPostback Then Exit Sub

        LoadAudit_Log()
    End Sub
#End Region


#Region "Private - Load/Save"
    Protected Sub LoadAudit_Log()
        With Me.Audit_Log
            ddLogTypeId.ValueInt = .LogTypeId
            txtLogMessage.Text = .LogMessage
            txtLogCreated.Text = CUtilities.Timespan(.LogCreated)
        End With
    End Sub
#End Region


End Class
