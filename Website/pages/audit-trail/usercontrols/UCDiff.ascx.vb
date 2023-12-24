
Partial Class usercontrols_audit_trail_UCDiff
    Inherits System.Web.UI.UserControl

    Public Sub DisplayAdded(ByVal change As CChange)
        row.Attributes.Add("class", "added")
        With change
            litColumnName.Text = .ColumnName
            litBefore.Text = "&nbsp;"
            litAfter.Text = Encode(.NewValue)
        End With
    End Sub
    Public Sub DisplayRemoved(ByVal change As CChange)
        row.Attributes.Add("class", "removed")
        With change
            litColumnName.Text = .ColumnName
            litBefore.Text = Encode(.OldValue)
            litAfter.Text = "&nbsp;"
        End With
    End Sub
    Public Sub DisplayChanged(ByVal change As CChange)
        row.Attributes.Add("class", "changed")
        With change
            litColumnName.Text = .ColumnName
            litBefore.Text = Encode(.OldValue)
            litAfter.Text = Encode(.NewValue)
        End With
    End Sub
    Public Sub DisplaySame(ByVal change As CChange)
        row.Attributes.Add("class", "same")
        With change
            litColumnName.Text = .ColumnName
            litBefore.Text = Encode(.OldValue)
            litAfter.Text = Encode(.NewValue)
        End With
    End Sub

    Private Function Encode(ByVal s As String) As String
        If String.IsNullOrEmpty(s) Then Return String.Empty
        s = CUtilities.Truncate(s, 1000)
        Return Server.HtmlEncode(s) 's.Replace(" ", "&nbsp;").Replace(vbCrLf, "<br>")
    End Function

End Class
