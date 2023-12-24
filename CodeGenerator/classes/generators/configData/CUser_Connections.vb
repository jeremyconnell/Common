Imports System.Configuration

Public Class CUser_Connections : Inherits ApplicationSettingsBase

#Region "Constants"
    Private Const OLD_FILE_NAME As String = "C:\\Program Files\\Picasso.net.nz\\ClassGenerator - Relational\\Connections.bin"
#End Region

#Region "Members"
    Private Shared m_connections As CConnections
    Private Shared m_singleton As CUser_Connections
#End Region

#Region "Add Connection"
    Public Shared Function AddMsAccess(ByVal path As String) As CSchemaData
        Dim c As CConnections = Storage
        c.LastConnectionType = EDriverTab.Access

        Dim con As New CMSAccessConnection(c)
        con.Path = path

        For Each i As CMSAccessConnection In c.MSAccessConnections
            If i.Equals(con) Then
                c.LastConnectionIndex = c.MSAccessConnections.IndexOf(i)
                i.SchemaInfo.LastConnectionDate = DateTime.Now
                Storage = c
                Return i.SchemaInfo
            End If
        Next
        c.MSAccessConnections.Add(con)
        c.LastConnectionIndex = c.MSAccessConnections.Count - 1
        Storage = c
        Return con.SchemaInfo
    End Function
    Public Shared Function AddMsExcel(ByVal path As String) As CSchemaData
        Dim c As CConnections = Storage
        c.LastConnectionType = EDriverTab.Excel

        Dim con As New CMSExcelConnection(c)
        con.Path = path

        For Each i As CMSExcelConnection In c.MSExcelConnections
            If i.Equals(con) Then
                c.LastConnectionIndex = c.MSExcelConnections.IndexOf(i)
                i.SchemaInfo.LastConnectionDate = DateTime.Now
                Storage = c
                Return i.SchemaInfo
            End If
        Next
        c.MSExcelConnections.Add(con)
        c.LastConnectionIndex = c.MSExcelConnections.Count - 1
        Storage = c
        Return con.SchemaInfo
    End Function
    Public Shared Function AddTextFile(ByVal path As String) As CSchemaData
        Dim c As CConnections = Storage
        c.LastConnectionType = EDriverTab.TextFile

        Dim con As New CTextFileConnection(c)
        con.Path = path

        For Each i As CTextFileConnection In c.TextFileConnections
            If i.Equals(con) Then
                c.LastConnectionIndex = c.TextFileConnections.IndexOf(i)
                i.SchemaInfo.LastConnectionDate = DateTime.Now
                Storage = c
                Return i.SchemaInfo
            End If
        Next
        c.TextFileConnections.Add(con)
        c.LastConnectionIndex = c.TextFileConnections.Count - 1
        Storage = c
        Return con.SchemaInfo
    End Function
    Public Shared Function AddSqlServer(ByVal server As String, ByVal database As String, ByVal username As String, ByVal password As String, ByVal winAuth As Boolean) As CSchemaData
        Dim c As CConnections = Storage
        c.LastConnectionType = EDriverTab.SqlServer

        Dim con As New CSqlServerConnection(c)
        con.Server = server
        con.Database = database
        con.WindowsAuthentication = winAuth
        con.UserName = username
        con.Password = password

        For Each i As CSqlServerConnection In c.SqlServerConnections
            If i.Equals(con) Then
                c.LastConnectionIndex = c.SqlServerConnections.IndexOf(i)
                i.SchemaInfo.LastConnectionDate = DateTime.Now
                Storage = c
                Return i.SchemaInfo
            End If
        Next
        c.SqlServerConnections.Add(con)
        c.LastConnectionIndex = c.SqlServerConnections.Count - 1

        Storage = c
        Return con.SchemaInfo
    End Function
    Public Shared Function AddMySql(ByVal server As String, ByVal database As String, ByVal username As String, ByVal password As String, ByVal port As Integer) As CSchemaData
        Dim c As CConnections = Storage
        c.LastConnectionType = EDriverTab.MySql
        Storage = c

        Dim con As New CMySqlConnection(c)
        con.Server = server
        con.Database = database
        con.UserName = username
        con.Password = password
        con.Port = port

        For Each i As CMySqlConnection In c.MySqlConnections
            If i.Equals(con) Then
                c.LastConnectionIndex = c.MySqlConnections.IndexOf(i)
                i.SchemaInfo.LastConnectionDate = DateTime.Now
                Storage = c
                Return i.SchemaInfo
            End If
        Next
        c.MySqlConnections.Add(con)
        c.LastConnectionIndex = c.MySqlConnections.Count - 1

        Storage = c
        Return con.SchemaInfo
    End Function
    Public Shared Function AddOracle(ByVal server As String, ByVal username As String, ByVal password As String) As CSchemaData
        Dim c As CConnections = Storage
        c.LastConnectionType = EDriverTab.Oracle
        Storage = c

        Dim con As New COracleConnection(c)
        con.Server = server
        con.UserName = username
        con.Password = password

        For Each i As COracleConnection In c.OracleConnections
            If i.Equals(con) Then
                c.LastConnectionIndex = c.OracleConnections.IndexOf(i)
                i.SchemaInfo.LastConnectionDate = DateTime.Now
                Storage = c
                Return i.SchemaInfo
            End If
        Next
        c.OracleConnections.Add(con)
        c.LastConnectionIndex = c.OracleConnections.Count - 1

        Storage = c
        Return con.SchemaInfo
    End Function
    Public Shared Function AddOleDb(ByVal connectionString As String) As CSchemaData
        Dim c As CConnections = Storage
        c.LastConnectionType = EDriverTab.OleDb
        Storage = c

        Dim con As New COleDbConnection(c)
        con.ConnectionString = connectionString

        For Each i As COleDbConnection In c.OleDbConnections
            If i.Equals(con) Then
                c.LastConnectionIndex = c.OleDbConnections.IndexOf(i)
                Storage = c
                Return i.SchemaInfo
            End If
        Next
        c.OleDbConnections.Add(con)
        c.LastConnectionIndex = c.OleDbConnections.Count - 1

        Storage = c
        Return con.SchemaInfo
    End Function
    Public Shared Function AddOdbc(ByVal connectionString As String) As CSchemaData
        Dim c As CConnections = Storage
        c.LastConnectionType = EDriverTab.Odbc
        Storage = c

        Dim con As New COdbcConnection(c)
        con.ConnectionString = connectionString

        For Each i As COdbcConnection In c.OdbcConnections
            If i.Equals(con) Then
                c.LastConnectionIndex = c.OdbcConnections.IndexOf(i)
                i.SchemaInfo.LastConnectionDate = DateTime.Now
                Storage = c
                Return i.SchemaInfo
            End If
        Next
        c.OdbcConnections.Add(con)
        c.LastConnectionIndex = c.OdbcConnections.Count - 1

        Storage = c
        Return con.SchemaInfo
    End Function
#End Region

#Region "Shared"
    Public Shared Property Storage() As CConnections
        Get
            If IsNothing(m_connections) Then
                m_connections = New CConnections()
                Try
                    m_connections.Decrypt(_Singleton.EncryptedXml)
                Catch ex As Exception
                    MsgBox(ex.Message, MsgBoxStyle.OkOnly, "Error loading connection settings")
                End Try
            End If
            Return m_connections
        End Get
        Set(ByVal value As CConnections)
            Try
                m_connections = value
                _Singleton.EncryptedXml = m_connections.Encrypt
            Catch ex As Exception
                MsgBox(ex.Message, MsgBoxStyle.OkOnly, "Error saving connection settings")
            End Try
        End Set
    End Property

    Private Shared ReadOnly Property _Singleton() As CUser_Connections
        Get
            If IsNothing(m_singleton) Then
                SyncLock (GetType(CUser_Connections))
                    If IsNothing(m_singleton) Then
                        m_singleton = New CUser_Connections()
                        'm_singleton.SettingsKey = "CUser_Connections"
                        'm_singleton.Upgrade()
                    End If
                End SyncLock
            End If
            Return m_singleton
        End Get
    End Property
#End Region

#Region "Setting"
    <UserScopedSetting()> _
    <SettingsSerializeAs(System.Configuration.SettingsSerializeAs.Binary)> _
    Public Property EncryptedXml() As Byte()
        Get
            'System.Threading.Thread.Sleep(10000)
            Dim encryptedData As Byte() = CType(Me("EncryptedXml"), Byte()) ' Bytes("EncryptedXml")

            'Backwards compat
            If IsNothing(encryptedData) Then
                Try
                    If IO.File.Exists(OLD_FILE_NAME) Then
                        encryptedData = IO.File.ReadAllBytes(OLD_FILE_NAME)
                        Me.EncryptedXml = encryptedData
                    End If
                Catch
                End Try
            End If

            Return encryptedData
        End Get
        Set(ByVal value As Byte())
            Me("EncryptedXml") = value
            Me.Save()

            'Backwards compat (double-persist)
            If Not IsNothing(value) Then
                Try
                    If IO.File.Exists(OLD_FILE_NAME) Then
                        IO.File.WriteAllBytes(OLD_FILE_NAME, value)
                    End If
                Catch
                End Try
            End If
        End Set
    End Property
#End Region

End Class
