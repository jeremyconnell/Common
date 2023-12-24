
Partial Class usercontrols_audit_trail_UCFilter
    Inherits System.Web.UI.UserControl

    Public Sub Display(ByVal name As String, ByVal value As String)
        litName.Text = name
        txtValue.Text = value
    End Sub
    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnDelete.Click
        CSession.AuditTrailFilters.Custom.Remove(litName.Text)
        Response.Redirect(Request.RawUrl)
    End Sub

    Protected Sub txtValue_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtValue.TextChanged
        With CSession.AuditTrailFilters.Custom
            If txtValue.Text.Length = 0 Then
                .Remove(litName.Text)
                Response.Redirect(Request.RawUrl)
            Else
                .Item(litName.Text) = txtValue.Text
            End If
        End With
    End Sub
End Class
