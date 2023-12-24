Public Class UCAccess

    'Events
    Public Event TestClicked(ByVal connection As CDataSrcLocal)

    'Interface
    Public Property FilePath() As String
        Get
            Return txtPath.Text
        End Get
        Set(ByVal value As String)
            txtPath.Text = value
        End Set
    End Property
    Public Sub Test()
        btnTest_Click(Nothing, Nothing)
    End Sub


    Private ReadOnly Property Data() As CMSAccessConnectionList
        Get
            If rbName.Checked Then
                Return CUser_Connections.Storage.MsAccess_ByName
            Else
                Return CUser_Connections.Storage.MsAccess_ByDate
            End If
        End Get
    End Property
    Public Sub Display()
        lvRecent.Items.Clear()
        With CUser_Connections.Storage
            For Each i As CMSAccessConnection In Data
                Dim fileName As String = IO.Path.GetFileName(i.Path)
                Dim folder As String = IO.Path.GetDirectoryName(i.Path)
                With lvRecent.Items.Add(fileName)
                    .Tag = i
                    .SubItems.Add(folder)
                End With

                If .LastConnectionType = EDriverTab.Access Then
                    If .LastConnectionIndex = .MSAccessConnections.IndexOf(i) Then
                        txtPath.Text = i.Path
                        lvRecent.Items(lvRecent.Items.Count - 1).Selected = True
                    End If
                End If
            Next
        End With
    End Sub

    'Event Handlers
    Private Sub UCAccess_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Display()
    End Sub
    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        OpenFileDialog1.ShowDialog()
    End Sub
    Private Sub OpenFileDialog1_FileOk(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles OpenFileDialog1.FileOk
        txtPath.Text = OpenFileDialog1.FileName
        btnTest_Click(sender, e)
    End Sub
    Private Sub btnTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTest.Click
        If txtPath.Text.Length = 0 Then
            MessageBox.Show("Browse to an Access database", "No database selected", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        If Not IO.File.Exists(txtPath.Text) Then
            MessageBox.Show(txtPath.Text, "File not found", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        RaiseEvent TestClicked(CDataSrc.OleDbFromAccessPath(txtPath.Text))
    End Sub

    Private Sub lvRecent_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvRecent.SelectedIndexChanged
        If lvRecent.SelectedItems.Count = 0 Then Exit Sub
        With Data(lvRecent.SelectedIndices(0))
            txtPath.Text = .Path
        End With
    End Sub
    Private Sub lvRecent_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvRecent.DoubleClick
        If lvRecent.SelectedItems.Count = 0 Then Exit Sub
        Test()
    End Sub
    Private Sub rb_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbName.CheckedChanged, rbRecent.CheckedChanged
        Display()
    End Sub

    'Context Menu
    Private Sub ContextMenuStrip1_Opened(ByVal sender As Object, ByVal e As System.EventArgs) Handles ContextMenuStrip1.Opened
        miDelete.Enabled = lvRecent.SelectedIndices.Count > 0
    End Sub
    Private Sub miDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles miDelete.Click
        If lvRecent.SelectedIndices.Count = 0 Then Exit Sub
        With lvRecent.SelectedItems(0)
            Dim con As CMSAccessConnection = CType(.Tag, CMSAccessConnection)
            CUser_Connections.Storage.MSAccessConnections.Remove(con)
            CUser_Connections.Storage = CUser_Connections.Storage
            Display()
        End With
    End Sub
End Class
