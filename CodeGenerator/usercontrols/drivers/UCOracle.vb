Imports Framework_OracleOdp

Public Class UCOracle
    'Events
    Public Event TestClicked(ByVal connection As CDataSrcLocal)

    'Interface
    Public Property Server() As String
        Get
            Return txtServer.Text
        End Get
        Set(ByVal value As String)
            txtServer.Text = value
        End Set
    End Property
    Public Property User() As String
        Get
            Return txtUser.Text
        End Get
        Set(ByVal value As String)
            txtUser.Text = value
        End Set
    End Property
    Public Property Password() As String
        Get
            Return txtPassword.Text
        End Get
        Set(ByVal value As String)
            txtPassword.Text = value
        End Set
    End Property
    Public Sub Test()
        btnTest_Click(Nothing, Nothing)
    End Sub
    Public Sub Display()
        lvRecent.Items.Clear()
        With CUser_Connections.Storage
            For Each i As COracleConnection In .Oracle_ByDate
                With lvRecent.Items.Add(i.Server)
                    .Tag = i
                    With .SubItems
                        .Add(i.UserName)
                        .Add(i.Password)
                    End With
                End With

                If .LastConnectionType = EDriverTab.Oracle Then
                    If .LastConnectionIndex = .OracleConnections.IndexOf(i) Then
                        txtServer.Text = i.Server
                        txtUser.Text = i.UserName
                        txtPassword.Text = i.Password
                        lvRecent.Items(lvRecent.Items.Count - 1).Selected = True
                    End If
                End If
            Next
        End With
    End Sub

    'Event Handlers
    Private Sub UCSqlServer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Display()
    End Sub

    Private Sub lvRecent_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvRecent.SelectedIndexChanged
        If lvRecent.SelectedItems.Count = 0 Then Exit Sub
        With CUser_Connections.Storage.OracleConnections(lvRecent.SelectedIndices(0))
            txtServer.Text = .Server
            txtUser.Text = .UserName
            txtPassword.Text = .Password
        End With
    End Sub
    Private Sub lvRecent_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvRecent.DoubleClick
        If lvRecent.SelectedItems.Count = 0 Then Exit Sub
        Test()
    End Sub

    Private Sub btnTest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTest.Click
        Dim s As String = String.Concat("Data Source=", txtServer.Text, ";User Id=", txtUser.Text, ";Password=", txtPassword.Text, ";")
        RaiseEvent TestClicked(New COracleClientMs(s))
    End Sub

    'Context Menu
    Private Sub ContextMenuStrip1_Opened(ByVal sender As Object, ByVal e As System.EventArgs) Handles ContextMenuStrip1.Opened
        miDelete.Enabled = lvRecent.SelectedIndices.Count > 0
    End Sub
    Private Sub miDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles miDelete.Click
        If lvRecent.SelectedIndices.Count = 0 Then Exit Sub
        With lvRecent.SelectedItems(0)
            Dim con As COracleConnection = CType(.Tag, COracleConnection)
            CUser_Connections.Storage.OracleConnections.Remove(con)
            CUser_Connections.Storage = CUser_Connections.Storage
            Display()
        End With
    End Sub
End Class
