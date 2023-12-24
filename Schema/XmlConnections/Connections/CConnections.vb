Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Xml

Imports Framework

<Serializable(), CLSCompliant(True)> _
Partial Public Class CConnections : Inherits CDocument

#Region "Constants"
    Public Const TAG_NAME As String = "Connections"
    Public Overrides ReadOnly Property TagName As String
        Get
            Return TAG_NAME
        End Get
    End Property
#End Region

#Region "Constructors"
    Public Sub New()
        MyBase.New()
		InitValues()
    End Sub
    Public Sub New(xml As String)
        MyBase.New(xml)
    End Sub
#End Region

#Region "Members"
    'Attributes
    Private m_lastConnectionIndex As Integer
    'ChildNodes - Single
    Private m_schemaInfo As CSchemaData
    'ChildNodes - Collections
    Private m_sqlServerConnections As CSqlServerConnectionList
    Private m_oleDbConnections As COleDbConnectionList
    Private m_odbcConnections As COdbcConnectionList
    Private m_mySqlConnections As CMySqlConnectionList
    Private m_oracleConnections As COracleConnectionList
    Private m_mSAccessConnections As CMSAccessConnectionList
    Private m_mSExcelConnections As CMSExcelConnectionList
    Private m_textFileConnections As CTextFileConnectionList
#End Region

#Region "Properties"
    'Attributes
    Public Property LastConnectionIndex As Integer
        Get
            Return m_lastConnectionIndex
        End Get
        Set(ByVal value As Integer)
            m_lastConnectionIndex = value
        End Set
    End Property
    'ChildNodes - Single
    Public ReadOnly Property SchemaInfo As CSchemaData
        Get
            If IsNothing(m_schemaInfo) Then
                m_schemaInfo = New CSchemaData(Me)
            End If
            Return m_schemaInfo
        End Get
    End Property
    'ChildNodes - Collections
    Public ReadOnly Property SqlServerConnections As CSqlServerConnectionList
        Get
            If IsNothing(m_sqlServerConnections) Then
                m_sqlServerConnections = New CSqlServerConnectionList
            End If
            Return m_sqlServerConnections
        End Get
    End Property
    Public ReadOnly Property OleDbConnections As COleDbConnectionList
        Get
            If IsNothing(m_oleDbConnections) Then
                m_oleDbConnections = New COleDbConnectionList
            End If
            Return m_oleDbConnections
        End Get
    End Property
    Public ReadOnly Property OdbcConnections As COdbcConnectionList
        Get
            If IsNothing(m_odbcConnections) Then
                m_odbcConnections = New COdbcConnectionList
            End If
            Return m_odbcConnections
        End Get
    End Property
    Public ReadOnly Property MySqlConnections As CMySqlConnectionList
        Get
            If IsNothing(m_mySqlConnections) Then
                m_mySqlConnections = New CMySqlConnectionList
            End If
            Return m_mySqlConnections
        End Get
    End Property
    Public ReadOnly Property OracleConnections As COracleConnectionList
        Get
            If IsNothing(m_oracleConnections) Then
                m_oracleConnections = New COracleConnectionList
            End If
            Return m_oracleConnections
        End Get
    End Property
    Public ReadOnly Property MSAccessConnections As CMSAccessConnectionList
        Get
            If IsNothing(m_mSAccessConnections) Then
                m_mSAccessConnections = New CMSAccessConnectionList
            End If
            Return m_mSAccessConnections
        End Get
    End Property
    Public ReadOnly Property MSExcelConnections As CMSExcelConnectionList
        Get
            If IsNothing(m_mSExcelConnections) Then
                m_mSExcelConnections = New CMSExcelConnectionList
            End If
            Return m_mSExcelConnections
        End Get
    End Property
    Public ReadOnly Property TextFileConnections As CTextFileConnectionList
        Get
            If IsNothing(m_textFileConnections) Then
                m_textFileConnections = New CTextFileConnectionList
            End If
            Return m_textFileConnections
        End Get
    End Property
#End Region

#Region "Protected - Document Level"
    Protected Overrides Sub Import(ByVal xml As String)
        If Not String.IsNullOrEmpty(xml) Then
            MyBase.Import(xml)
        Else
            InitValues()
        End If
    End Sub
    Protected Sub InitValues()
        'Attributes
        m_lastConnectionIndex = Integer.MinValue
        'ChildNodes - Single
        m_schemaInfo = Nothing
        'ChildNodes - Collections
        m_sqlServerConnections = Nothing
        m_oleDbConnections = Nothing
        m_odbcConnections = Nothing
        m_mySqlConnections = Nothing
        m_oracleConnections = Nothing
        m_mSAccessConnections = Nothing
        m_mSExcelConnections = Nothing
        m_textFileConnections = Nothing
    End Sub
#End Region

#Region "Public - Node Level"
    Public Overrides Sub Import(parent As XmlNode)
        Dim node As XmlNode = ImportSelf(parent)

        'Load Attributes
        m_lastConnectionIndex = CXml.AttributeInt(node, "LastConnectionIndex", m_lastConnectionIndex)
        'Load ChildNodes - Single
        m_schemaInfo = New CSchemaData(Me, node)
        'Load ChildNodes - Collections
        m_sqlServerConnections = New CSqlServerConnectionList(Me, node)
		m_oleDbConnections = New COleDbConnectionList(Me, node)
		m_odbcConnections = New COdbcConnectionList(Me, node)
		m_mySqlConnections = New CMySqlConnectionList(Me, node)
		m_oracleConnections = New COracleConnectionList(Me, node)
		m_mSAccessConnections = New CMSAccessConnectionList(Me, node)
		m_mSExcelConnections = New CMSExcelConnectionList(Me, node)
		m_textFileConnections = New CTextFileConnectionList(Me, node)
    End Sub
    Public Overrides Sub Export(parent As XmlNode)    
        Dim node As XmlNode = ExportSelf(parent)

        'Attributes
        CXml.AttributeSet(node, "LastConnectionIndex", m_lastConnectionIndex)
        'ChildNodes - Single
		If Not IsNothing(m_schemaInfo) Then m_schemaInfo.Export(node)
        'ChildNodes - Collections
		If Not IsNothing(m_sqlServerConnections) Then m_sqlServerConnections.Export(node)
		If Not IsNothing(m_oleDbConnections) Then m_oleDbConnections.Export(node)
		If Not IsNothing(m_odbcConnections) Then m_odbcConnections.Export(node)
		If Not IsNothing(m_mySqlConnections) Then m_mySqlConnections.Export(node)
		If Not IsNothing(m_oracleConnections) Then m_oracleConnections.Export(node)
		If Not IsNothing(m_mSAccessConnections) Then m_mSAccessConnections.Export(node)
		If Not IsNothing(m_mSExcelConnections) Then m_mSExcelConnections.Export(node)
		If Not IsNothing(m_textFileConnections) Then m_textFileConnections.Export(node)
    End Sub
#End Region

End Class