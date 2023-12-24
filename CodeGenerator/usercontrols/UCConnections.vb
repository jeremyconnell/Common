Imports Framework_MySql


Public Class UCConnections

#Region "Events"
    Public Event TestOk()
#End Region

#Region "State"
    Private m_dataSrc As CDataSrcLocal = Nothing
    Private m_schemaInfo As CSchemaData = Nothing
    Public ReadOnly Property DataSrc() As CDataSrcLocal
        Get
            Return m_dataSrc
        End Get
    End Property
    Public ReadOnly Property SchemaInfo() As CSchemaData
        Get
            Return m_schemaInfo
        End Get
    End Property
    Public ReadOnly Property IsReady() As Boolean
        Get
            Return Not IsNothing(m_dataSrc)
        End Get
    End Property
    Public Property Tab() As EDriverTab
        Get
            Return CType(TabControl1.SelectedIndex, EDriverTab)
        End Get
        Set(ByVal value As EDriverTab)
            TabControl1.SelectedIndex = value
        End Set
    End Property
    Public Sub Test()
        Select Case Tab
            Case EDriverTab.Access : UcAccess1.Test()
            Case EDriverTab.MySql : UcMySql1.Test()
            Case EDriverTab.Odbc : UcOdbc1.Test()
            Case EDriverTab.OleDb : UcOleDb1.Test()
            Case EDriverTab.Oracle : UcOracle1.Test()
            Case EDriverTab.SqlServer : UcSqlServer1.Test()
            Case EDriverTab.Excel : UcExcel1.Test()
            Case EDriverTab.TextFile : UcTextFile1.Test()
            Case Else : Throw New Exception("Unknown tab: " & Tab)
        End Select
    End Sub

    Public ReadOnly Property Platform() As EPlatform
        Get
            Return GetPlatForm(DataSrc)
        End Get
    End Property
    Public Shared Function GetPlatForm(ByVal d As CDataSrc) As EPlatform
        If TypeOf d Is CMySqlClient Then Return EPlatform.MySql
        If TypeOf d Is CSqlClient Then Return EPlatform.SqlServer
        If TypeOf d Is COracleClient Then Return EPlatform.Oracle
        Return EPlatform.Other
    End Function
#End Region

#Region "Event Handlers"
    Private Sub UCConnections_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        TabControl1.SelectedIndex = CUser_Connections.Storage.LastConnectionType
    End Sub
    Private Sub UcConnection1_TestClick(ByVal dataSrc As CDataSrcLocal) Handles UcAccess1.TestClicked, UcMySql1.TestClicked, UcOdbc1.TestClicked, UcOleDb1.TestClicked, UcOracle1.TestClicked, UcSqlServer1.TestClicked, UcExcel1.TestClicked, UcTextFile1.TestClicked
        Try
            'Test
            dataSrc.Connection().Close()

            'Store
            m_dataSrc = dataSrc
            m_schemaInfo = StoreConnection()

            RaiseEvent TestOk()

            'Show new connection
            Select Case Tab
                Case EDriverTab.Access : UcAccess1.Display()
                Case EDriverTab.MySql : UcMySql1.Display()
                Case EDriverTab.Odbc : UcOdbc1.Display()
                Case EDriverTab.OleDb : UcOleDb1.Display()
                Case EDriverTab.Oracle : UcOracle1.Display()
                Case EDriverTab.SqlServer : UcSqlServer1.Display()
                Case EDriverTab.Excel : UcExcel1.Display()
                Case EDriverTab.TextFile : UcTextFile1.Display()
            End Select
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Connection Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        m_dataSrc = Nothing
    End Sub
#End Region

#Region "Store"
    Private Function StoreConnection() As CSchemaData
        Select Case Tab
            Case EDriverTab.Access : Return CUser_Connections.AddMsAccess(UcAccess1.FilePath)
            Case EDriverTab.Excel : Return CUser_Connections.AddMsExcel(UcExcel1.FilePath)
            Case EDriverTab.TextFile : Return CUser_Connections.AddTextFile(UcTextFile1.FilePath)
            Case EDriverTab.Odbc : Return CUser_Connections.AddOdbc(UcOdbc1.ConnectionString)
            Case EDriverTab.OleDb : Return CUser_Connections.AddOleDb(UcOleDb1.ConnectionString)

            Case EDriverTab.MySql
                With UcMySql1
                    Return CUser_Connections.AddMySql(.Server, .Database, .User, .Password, .Port)
                End With

            Case EDriverTab.Oracle
                With UcOracle1
                    Return CUser_Connections.AddOracle(.Server, .User, .Password)
                End With

            Case EDriverTab.SqlServer
                With UcSqlServer1
                    Return CUser_Connections.AddSqlServer(.Server, .Database, .User, .Password, .WindowsAuthentication)
                End With

            Case Else
                Throw New Exception("Unknown driver: " & Tab.ToString())
        End Select
    End Function
#End Region

End Class
