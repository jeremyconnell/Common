Imports System.Data

Partial Class pages_audit_Errors_group : Inherits CPage

#Region "Querystring"
    Public ReadOnly Property Type1() As Integer
        Get
            Return CWeb.RequestInt("type1")
        End Get
    End Property
    Public ReadOnly Property Message1() As Integer
        Get
            Return CWeb.RequestInt("message1")
        End Get
    End Property
    Public ReadOnly Property Type2() As Integer
        Get
            Return CWeb.RequestInt("type2")
        End Get
    End Property
    Public ReadOnly Property Message2() As Integer
        Get
            Return CWeb.RequestInt("message2")
        End Get
    End Property
#End Region

#Region "Members"
    Private m_audit_Errors As CAudit_ErrorList
#End Region

#Region "Data"
    Public ReadOnly Property [Audit_Errors]() As CAudit_ErrorList
        Get
            If IsNothing(m_audit_Errors) Then
                m_audit_Errors = New CAudit_Error().SelectGroup(ctrlAudit_Errors.PagingInfo, Type1, Message1, Type2, Message2)
            End If
            Return m_audit_Errors
        End Get
    End Property
    Public ReadOnly Property Audit_ErrorsAsDataSet() As DataSet
        Get
            Return New CAudit_Error().SelectGroup_DataSet(Type1, Message1, Type1, Message2)
        End Get
    End Property
#End Region

#Region "Event Handlers - Page "
    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ctrlAudit_Errors.Display(Me.Audit_Errors, False)

        UnbindSideMenu()
        AddMenuSide("Error Log", CSitemap.Audit_Errors)
        AddMenuSide("Error Group", CSitemap.Audit_Error(Type1, Message1, Type2, Message2))
        AddMenuSide(CUtilities.NameAndCount("Individual", ctrlAudit_Errors.PagingInfo.Count))
    End Sub
#End Region

#Region "Event Handlers - UserControl"
    Private Sub ctrl_ExportClick() Handles ctrlAudit_Errors.ExportClick
        CDataSrc.ExportToCsv(Audit_ErrorsAsDataSet, Response, "Audit_Errors.csv")
    End Sub
    Private Sub ctrl_SortClick(ByVal sortBy As String, ByVal descending As Boolean, ByVal pageNumber As Integer) Handles ctrlAudit_Errors.SortClick
        Response.Redirect(CSitemap.Audit_ErrorGroup(Type1, Message1, Type1, Message2, sortBy, descending, pageNumber))
    End Sub
#End Region

End Class
