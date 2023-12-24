Imports Framework
Imports System.Xml


#Region "Enums"
<CLSCompliant(True)> _
Public Enum EDriverTab
    Access
    SqlServer
    MySql
    Oracle
    OleDb
    Odbc
    Excel
    TextFile
End Enum
#End Region

Partial Public Class CConnections

#Region "Members"
    Private m_lastConnectionType As EDriverTab = EDriverTab.SqlServer
#End Region

#Region "Properties"
    Public Property LastConnectionType() As EDriverTab
        Get
            Return m_lastConnectionType
        End Get
        Set(ByVal value As EDriverTab)
            m_lastConnectionType = value
        End Set
    End Property
#End Region

#Region "Persistance"
    Protected Overrides Function ImportSelf(ByVal parent As XmlNode) As XmlNode
        Dim node As XmlNode = MyBase.ImportSelf(parent)
        m_lastConnectionType = CType(CXml.AttributeInt(node, "LastConnectionType", m_lastConnectionType), EDriverTab)
        Return node
    End Function
    Protected Overrides Function ExportSelf(ByVal parent As System.Xml.XmlNode) As System.Xml.XmlNode
        Dim node As XmlNode = MyBase.ExportSelf(parent)
        CXml.AttributeSet(node, "LastConnectionType", m_lastConnectionType)
        Return node
    End Function
#End Region

#Region "Encryption"
    'Byte-Array
    Public Function Encrypt() As Byte()
        Return CBinary.Encrypt(Me.ToString)
    End Function
    Public Sub Decrypt(ByVal encrypted As Byte())
        Me.Import(CBinary.DecryptAsStr(encrypted))
    End Sub

    'File-Path
    Public Sub Decrypt(ByVal filePath As String)
        If Not IO.File.Exists(filePath) Then Exit Sub
        Decrypt(IO.File.ReadAllBytes(filePath))
    End Sub
    Public Sub Encrypt(ByVal filePath As String)
        Try
            IO.File.WriteAllBytes(filePath, Encrypt)
        Catch ex As Exception
            MsgBox("Unable to store connection string - Vista users should allow write access", MsgBoxStyle.OkOnly, ex.Message)
        End Try
    End Sub
#End Region

#Region "Count"
    Public Function Count() As Integer
        With Me
            Return _
            .MSAccessConnections.Count + _
            .SqlServerConnections.Count + _
            .MySqlConnections.Count + _
            .OracleConnections.Count + _
            .OleDbConnections.Count
        End With
    End Function
#End Region

#Region "Sorted Collections"
    Private m_sqlServer_ByDate As CSqlServerConnectionList
    Public ReadOnly Property SqlServer_ByDate() As CSqlServerConnectionList
        Get
            If IsNothing(m_sqlServer_ByDate) OrElse m_sqlServer_ByDate.Count <> m_sqlServerConnections.Count Then
                m_sqlServer_ByDate = New CSqlServerConnectionList(SqlServerConnections)
                m_sqlServer_ByDate.Sort(New SortByLastAccessed)
            End If
            Return m_sqlServer_ByDate
        End Get
    End Property
    Private m_sqlServer_ByName As CSqlServerConnectionList
    Public ReadOnly Property SqlServer_ByName() As CSqlServerConnectionList
        Get
            If IsNothing(m_sqlServer_ByName) OrElse m_sqlServer_ByName.Count <> m_sqlServerConnections.Count Then
                m_sqlServer_ByName = New CSqlServerConnectionList(SqlServerConnections)
                m_sqlServer_ByName.Sort()
            End If
            Return m_sqlServer_ByName
        End Get
    End Property


    Private m_msAccess_ByDate As CMSAccessConnectionList
    Public ReadOnly Property MsAccess_ByDate() As CMSAccessConnectionList
        Get
            If IsNothing(m_msAccess_ByDate) OrElse m_msAccess_ByDate.Count <> m_mSAccessConnections.Count Then
                m_msAccess_ByDate = New CMSAccessConnectionList(MSAccessConnections)
                m_msAccess_ByDate.Sort(New SortByLastAccessed)
            End If
            Return m_msAccess_ByDate
        End Get
    End Property
    Private m_msAccess_ByName As CMSAccessConnectionList
    Public ReadOnly Property MsAccess_ByName() As CMSAccessConnectionList
        Get
            If IsNothing(m_msAccess_ByName) OrElse m_msAccess_ByName.Count <> m_mSAccessConnections.Count Then
                m_msAccess_ByName = New CMSAccessConnectionList(MSAccessConnections)
                m_msAccess_ByName.Sort()
            End If
            Return m_msAccess_ByName
        End Get
    End Property




    Private m_msExcel_ByDate As CMSExcelConnectionList
    Public ReadOnly Property MsExcel_ByDate() As CMSExcelConnectionList
        Get
            If IsNothing(m_msExcel_ByDate) OrElse m_msExcel_ByDate.Count <> m_mSExcelConnections.Count Then
                m_msExcel_ByDate = New CMSExcelConnectionList(MSExcelConnections)
                m_msExcel_ByDate.Sort(New SortByLastAccessed)
            End If
            Return m_msExcel_ByDate
        End Get
    End Property
    Private m_msExcel_ByName As CMSExcelConnectionList
    Public ReadOnly Property MsExcel_ByName() As CMSExcelConnectionList
        Get
            If IsNothing(m_msExcel_ByName) OrElse m_msExcel_ByName.Count <> m_mSExcelConnections.Count Then
                m_msExcel_ByName = New CMSExcelConnectionList(MSExcelConnections)
                m_msExcel_ByName.Sort()
            End If
            Return m_msExcel_ByName
        End Get
    End Property




    Private m_textFile_ByDate As CTextFileConnectionList
    Public ReadOnly Property TextFile_ByDate() As CTextFileConnectionList
        Get
            If IsNothing(m_textFile_ByDate) OrElse m_textFile_ByDate.Count <> m_mSAccessConnections.Count Then
                m_textFile_ByDate = New CTextFileConnectionList(TextFileConnections)
                m_textFile_ByDate.Sort(New SortByLastAccessed)
            End If
            Return m_textFile_ByDate
        End Get
    End Property
    Private m_textFile_ByName As CTextFileConnectionList
    Public ReadOnly Property TextFile_ByName() As CTextFileConnectionList
        Get
            If IsNothing(m_textFile_ByName) OrElse m_textFile_ByName.Count <> m_mSAccessConnections.Count Then
                m_textFile_ByName = New CTextFileConnectionList(TextFileConnections)
                m_textFile_ByName.Sort()
            End If
            Return m_textFile_ByName
        End Get
    End Property


    Private m_mySql_ByDate As CMySqlConnectionList
    Public ReadOnly Property MySql_ByDate() As CMySqlConnectionList
        Get
            If IsNothing(m_mySql_ByDate) OrElse m_mySql_ByDate.Count <> m_mySqlConnections.Count Then
                m_mySql_ByDate = New CMySqlConnectionList(MySqlConnections)
                m_mySql_ByDate.Sort(New SortByLastAccessed)
            End If
            Return m_mySql_ByDate
        End Get
    End Property
    Private m_mySql_ByName As CMySqlConnectionList
    Public ReadOnly Property MySql_ByName() As CMySqlConnectionList
        Get
            If IsNothing(m_mySql_ByName) OrElse m_mySql_ByName.Count <> m_mySqlConnections.Count Then
                m_mySql_ByName = New CMySqlConnectionList(MySqlConnections)
                m_mySql_ByName.Sort()
            End If
            Return m_mySql_ByName
        End Get
    End Property

    Private m_odbc_ByDate As COdbcConnectionList
    Public ReadOnly Property Odbc_ByDate() As COdbcConnectionList
        Get
            If IsNothing(m_odbc_ByDate) OrElse m_odbc_ByDate.Count <> m_odbcConnections.Count Then
                m_odbc_ByDate = New COdbcConnectionList(OdbcConnections)
                m_odbc_ByDate.Sort(New SortByLastAccessed)
            End If
            Return m_odbc_ByDate
        End Get
    End Property

    Private m_oledb_ByDate As COleDbConnectionList
    Public ReadOnly Property OleDb_ByDate() As COleDbConnectionList
        Get
            If IsNothing(m_oledb_ByDate) OrElse m_oledb_ByDate.Count <> m_oleDbConnections.Count Then
                m_oledb_ByDate = New COleDbConnectionList(OleDbConnections)
                m_oledb_ByDate.Sort(New SortByLastAccessed)
            End If
            Return m_oledb_ByDate
        End Get
    End Property

    Private m_oracle_ByDate As COracleConnectionList
    Public ReadOnly Property Oracle_ByDate() As COracleConnectionList
        Get
            If IsNothing(m_oracle_ByDate) OrElse m_oracle_ByDate.Count <> m_odbcConnections.Count Then
                m_oracle_ByDate = New COracleConnectionList(OracleConnections)
                m_oracle_ByDate.Sort(New SortByLastAccessed)
            End If
            Return m_oracle_ByDate
        End Get
    End Property
#End Region

End Class
