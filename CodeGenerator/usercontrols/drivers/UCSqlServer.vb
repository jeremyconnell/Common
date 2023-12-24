Public Class UCSqlServer
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
    Public Property WindowsAuthentication() As Boolean
        Get
            Return chkWinAuth.Checked
        End Get
        Set(ByVal value As Boolean)
            chkWinAuth.Checked = value
        End Set
    End Property
    Public Sub Test()
        If Server = "" Then Exit Sub
        btnTest_Click(Nothing, Nothing)
    End Sub

    Private ReadOnly Property Data() As CSqlServerConnectionList
        Get
            If rbName.Checked Then
                Return CUser_Connections.Storage.SqlServer_ByName
            Else
                Return CUser_Connections.Storage.SqlServer_ByDate
            End If
        End Get
    End Property
    Public Sub Display()
        lvRecent.Items.Clear()
        With CUser_Connections.Storage
            For Each i As CSqlServerConnection In Data
                With lvRecent.Items.Add(i.Server.ToUpper)
                    .Tag = i
                    With .SubItems
                        .Add(i.Database.ToUpper)
                        .Add(CStr(IIf(i.WindowsAuthentication, "Yes", "No")))
                        .Add(i.UserName)
                        .Add(i.Password)
                        .Add(i.SchemaInfo.OutputFolder)
                    End With
                End With

                If .LastConnectionType = EDriverTab.SqlServer Then
                    If .LastConnectionIndex = .SqlServerConnections.IndexOf(i) Then
                        Me.Server = i.Server
                        Me.Database = i.Database
                        Me.WindowsAuthentication = i.WindowsAuthentication
                        Me.User = i.UserName
                        Me.Password = i.Password
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

    Private Sub btnTest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTest.Click
        Dim s As String = String.Concat("Server=", txtServer.Text, ";Database=", txtDatabase.Text, ";")
        If chkWinAuth.Checked Then
            s = String.Concat(s, "Integrated Security=SSPI;", ";timeout=2")
        Else
            s = String.Concat(s, "UID=", txtUser.Text, ";PWD=", txtPassword.Text, ";timeout=2")
        End If
        RaiseEvent TestClicked(New CSqlClient(s))
    End Sub



    Private Sub lvRecent_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvRecent.SelectedIndexChanged
        If lvRecent.SelectedItems.Count = 0 Then Exit Sub
        With Data(lvRecent.SelectedIndices(0))
            txtServer.Text = .Server
            txtDatabase.Text = .Database
            txtUser.Text = .UserName
            txtPassword.Text = .Password
        End With
    End Sub
    Private Sub lvRecent_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvRecent.DoubleClick
        If lvRecent.SelectedItems.Count = 0 Then Exit Sub
        Test()
    End Sub

    Private Sub chkWinAuth_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkWinAuth.CheckedChanged
        txtUser.Enabled = Not chkWinAuth.Checked
        txtPassword.Enabled = Not chkWinAuth.Checked
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
            Dim con As CSqlServerConnection = CType(.Tag, CSqlServerConnection)
            CUser_Connections.Storage.SqlServerConnections.Remove(con)
            CUser_Connections.Storage = CUser_Connections.Storage
            Display()
        End With
    End Sub
End Class
