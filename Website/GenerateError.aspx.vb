
Partial Class GenerateError
    Inherits System.Web.UI.Page

    Protected Sub form1_Load(sender As Object, e As System.EventArgs) Handles form1.Load
        Throw New Exception("Test")
    End Sub

End Class
