Public Class UCXsdClassGenerator

#Region "Form - Language"
    Public Property Language() As ELanguage
        Get
            If rbCSharp.Checked Then
                Return ELanguage.CSharp
            Else
                Return ELanguage.VbNet
            End If
        End Get
        Set(ByVal value As ELanguage)
            If value = ELanguage.CSharp Then
                rbCSharp.Checked = True
            Else
                rbVB.Checked = True
            End If
        End Set
    End Property
    Public Property DefaultNameSpace() As String
        Get
            Return txtDotNetNamespace.Text
        End Get
        Set(ByVal value As String)
            txtDotNetNamespace.Text = value
        End Set
    End Property
#End Region

#Region "Event Handlers"
    'History
    Private Sub UcHistory1_Selected(ByVal filePath As String) Handles UcHistory1.Selected
        OpenFileDialog1.FileName = filePath
        OpenFileDialog1_FileOk(Nothing, Nothing)
    End Sub

    'Browse Folder
    Private Sub btnBrowseFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseFolder.Click
        Dim result As DialogResult = FolderBrowserDialog1.ShowDialog()
        If result = Windows.Forms.DialogResult.OK Then
            txtOutputPath.Text = FolderBrowserDialog1.SelectedPath
        End If
    End Sub

    'Browse File
    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        OpenFileDialog1.ShowDialog()
    End Sub
    Private Sub OpenFileDialog1_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        txtFilePath.Text = OpenFileDialog1.FileName

        'Default outpath to xsd folder
        txtOutputPath.Text = IO.Path.GetDirectoryName(txtFilePath.Text)


        If CRecent.Add(OpenFileDialog1.FileName) Then UcHistory1.Display()
    End Sub

    'Language
    Private Sub rb_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbCSharp.CheckedChanged, rbVB.CheckedChanged
        txtDotNetNamespace.Enabled = rbCSharp.Checked
    End Sub

    'Generate
    Private Sub btnGenerate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        Try
            With New CGeneratorXsd(DefaultNameSpace, Language)
                Dim count As Integer = .Generate(txtFilePath.Text, txtOutputPath.Text)
                MsgBox("Successfully generated " & count & " classes. Include these files in your project and compile", MsgBoxStyle.OkOnly, count & " files generated")
            End With
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "Error")
        End Try
    End Sub
#End Region

End Class
