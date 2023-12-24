Imports Framework_MySql

Public Class UCMySql
    'Events
    Public Event TestClicked(ByVal connection As CDataSrcLocal)

    'Interface
    Public Sub Test()
        btnTest_Click(Nothing, Nothing)
    End Sub
    Public Property Server() As String
        Get
            Return txtServer.Text
        End Get
        Set(ByVal value As String)
            txtServer.Text = value
        End Set
    End Property
    Public Property Database() As String
        Get
            Return txtDatabase.Text
        End Get
        Set(ByVal value As String)
            txtDatabase.Text = value
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
    Public Property Port() As Integer
        Get
            Dim i As Integer
            If Integer.TryParse(txtPort.Text, i) Then Return i
            txtPort.Text = "3306"
            Return 3306S
        End Get
        Set(ByVal value As Integer)
            txtPort.Text = value.ToString
        End Set
    End Property


    Private ReadOnly Property Data() As CMySqlConnectionList
        Get
            If rbName.Checked Then
                Return CUser_Connections.Storage.MySql_ByName
            Else
                Return CUser_Connections.Storage.MySql_ByDate
            End If
        End Get
    End Property
    Public Sub Display()
        lvRecent.Items.Clear()
        With CUser_Connections.Storage
            For Each i As CMySqlConnection In Data
                With lvRecent.Items.Add(i.Server)
                    .Tag = i
                    With .SubItems
                        .Add(i.Port.ToString)
                        .Add(i.UserName)
                        .Add(i.Password)
                        .Add(i.Database)
                    End With
                End With

                If .LastConnectionType = EDriverTab.MySql Then
                    If .LastConnectionIndex = .MySqlConnections.IndexOf(i) Then
                        txtServer.Text = i.Server
                        txtDatabase.Text = i.Database
                        txtUser.Text = i.UserName
                        txtPassword.Text = i.Password
                        txtPort.Text = i.Port.ToString
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
        With Data(lvRecent.SelectedIndices(0))
            txtServer.Text = .Server
            txtDatabase.Text = .Database
            txtUser.Text = .UserName
            txtPassword.Text = .Password
            txtPort.Text = .Port.ToString
        End With
    End Sub
    Private Sub lvRecent_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvRecent.DoubleClick
        If lvRecent.SelectedItems.Count = 0 Then Exit Sub
        Test()
    End Sub

    Private Sub btnTest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTest.Click
        RaiseEvent TestClicked(New CMySqlClient(Server, Database, User, Password, Port))
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
            Dim con As CMySqlConnection = CType(.Tag, CMySqlConnection)
            CUser_Connections.Storage.MySqlConnections.Remove(con)
            CUser_Connections.Storage = CUser_Connections.Storage
            Display()
        End With
    End Sub

End Class
