
Partial Class usercontrols_extensions_form_table_rows_UCFormLabel
    Inherits System.Web.UI.UserControl

    Public Property Text() As String
        Get
            Return litText.Text
        End Get
        Set(ByVal value As String)
            litText.Text = value
        End Set
    End Property

End Class
