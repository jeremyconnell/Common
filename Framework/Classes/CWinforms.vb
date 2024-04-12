Public Class CWinforms

    Public Shared Sub SetValue(ByVal dd As System.Windows.Forms.ComboBox, ByVal val As String)
        For i As Integer = 0 To dd.Items.Count - 1
            If dd.Items(i).ToString = val Then
                dd.SelectedIndex = i
                Exit Sub
            End If
        Next
    End Sub

End Class
