Public Class UCCopyAndPaste

    Public Shadows Property Text() As String
        Get
            Return txtCopyAndPaste.Text
        End Get
        Set(ByVal value As String)
            txtCopyAndPaste.Text = value
        End Set
    End Property

    Private Sub btnCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopy.Click
        Windows.Forms.Clipboard.SetText(txtCopyAndPaste.Text)
    End Sub

End Class
