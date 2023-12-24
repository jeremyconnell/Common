Imports System.Data

Partial Class pages_Audit_Errors_default : Inherits CPage

#Region "Members"
    Private m_audit_Errors As CAudit_ErrorList
#End Region

#Region "Data"
    Public ReadOnly Property [Audit_Errors]() As CAudit_ErrorList
        Get
            If IsNothing(m_audit_Errors) Then
                m_audit_Errors = New CAudit_Error().SelectSearch(ctrlAudit_Errors.PagingInfo, txtSearch.Text, chkUniqueOnly.Checked) 'Sql-based Paging
            End If
            Return m_audit_Errors
        End Get
    End Property
    Public ReadOnly Property Audit_ErrorsAsDataSet() As DataSet
        Get
            Return New CAudit_Error().SelectSearch_Dataset(txtSearch.Text, chkUniqueOnly.Checked)
        End Get
    End Property
#End Region

#Region "Event Handlers - Page "
    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'Restore state
        txtSearch.Text = CWeb.RequestStr("search")
        chkUniqueOnly.Checked = CWeb.RequestBool("uniqueOnly", True)

        'Display 
        Try
            ctrlAudit_Errors.Display(Me.Audit_Errors, chkUniqueOnly.Checked)
        Catch ex As Exception
            btnDelete_Click(Nothing, Nothing)
        End Try
    End Sub
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        'Catch-all for auto-postback search filters
        If Page.IsPostBack Then Response.Redirect(CSitemap.Audit_Errors(txtSearch.Text, chkUniqueOnly.Checked))
    End Sub
#End Region

#Region "Event Handlers - Form"
    Private Sub ctrl_ExportClick() Handles ctrlAudit_Errors.ExportClick
        CDataSrc.ExportToCsv(Audit_ErrorsAsDataSet, Response, "Audit_Errors.csv")
    End Sub
    Private Sub ctrl_SortClick(ByVal sortBy As String, ByVal descending As Boolean, ByVal pageNumber As Integer) Handles ctrlAudit_Errors.SortClick
        Response.Redirect(CSitemap.Audit_Errors(txtSearch.Text, chkUniqueOnly.Checked, sortBy, descending, pageNumber))
    End Sub
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        With New CAudit_Error
            .DeleteAll()
        End With
        Response.Redirect(CSitemap.Audit_Errors)
    End Sub
#End Region

End Class
