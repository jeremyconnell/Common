Public Class UCOdbc
    'Events
    Public Event TestClicked(ByVal connection As CDataSrcLocal)

    'Interface
    Public Property ConnectionString() As String
        Get
            Return txtConnectionString.Text
        End Get
        Set(ByVal value As String)
            txtConnectionString.Text = value
        End Set
    End Property
    Public Sub Test()
        btnTest_Click(Nothing, Nothing)
    End Sub
    Public Sub Display()
        lvRecent.Items.Clear()
        With CUser_Connections.Storage
            For Each i As COdbcConnection In .Odbc_ByDate
                lvRecent.Items.Add(i.ConnectionString).Tag = i

                If .LastConnectionType = EDriverTab.Odbc Then
                    If .LastConnectionIndex = .OdbcConnections.IndexOf(i) Then
                        txtConnectionString.Text = i.ConnectionString
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
    Private Sub lvRecent_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvRecent.SelectedIndexChanged
        If lvRecent.SelectedItems.Count = 0 Then Exit Sub
        With CUser_Connections.Storage.Odbc_ByDate(lvRecent.SelectedIndices(0))
            txtConnectionString.Text = .ConnectionString
        End With
    End Sub
    Private Sub lvRecent_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvRecent.DoubleClick
        If lvRecent.SelectedItems.Count = 0 Then Exit Sub
        Test()
    End Sub
    Private Sub btnTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTest.Click
        RaiseEvent TestClicked(New COleDb(txtConnectionString.Text))
    End Sub

    'Context Menu
    Private Sub ContextMenuStrip1_Opened(ByVal sender As Object, ByVal e As System.EventArgs) Handles ContextMenuStrip1.Opened
        miDelete.Enabled = lvRecent.SelectedIndices.Count > 0
    End Sub
    Private Sub miDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles miDelete.Click
        If lvRecent.SelectedIndices.Count = 0 Then Exit Sub
        With lvRecent.SelectedItems(0)
            Dim con As COdbcConnection = CType(.Tag, COdbcConnection)
            CUser_Connections.Storage.OdbcConnections.Remove(con)
            CUser_Connections.Storage = CUser_Connections.Storage
            Display()
        End With
    End Sub

End Class
