
Partial Class usercontrols_UCPageMessage
	Inherits System.Web.UI.UserControl

	Private Sub usercontrols_UCPageMessage_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
		If Not String.IsNullOrEmpty(CSession.PageMessage) Then
			pre.InnerText = CSession.PageMessage '.Replace(vbCrLf, "<br/>")
		End If
		If pre.InnerText.Length > 0 Then pre.Visible = True
		CSession.PageMessage = Nothing
	End Sub
End Class
