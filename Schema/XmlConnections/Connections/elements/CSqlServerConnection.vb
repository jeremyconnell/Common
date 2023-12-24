Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Xml

Imports Framework

<Serializable(), CLSCompliant(True)> _
Partial Public Class CSqlServerConnection : Inherits CElement

#Region "Constants"
    Public Const TAG_NAME As String = "SqlServerConnection"
    Public Overrides ReadOnly Property TagName As String
        Get
            Return TAG_NAME
        End Get
    End Property
#End Region

#Region "Constructors"
    Public Sub New(parent As CConnections)
        MyBase.New(parent)
    End Sub
    Friend Sub New(parent As CConnections, node As XmlNode)
        MyBase.New(parent, node)
    End Sub
#End Region

#Region "Initial State"
    Protected Overrides Sub ApplyDefaultValues_Auto()
        'Attributes
        m_server = String.Empty
        m_database = String.Empty
        m_windowsAuthentication = False
        m_userName = String.Empty
        m_password = String.Empty
        'ChildNodes - Single
        m_schemaInfo = Nothing
    End Sub
#End Region

#Region "Members"
    'Attributes
    Private m_server As String
    Private m_database As String
    Private m_windowsAuthentication As Boolean
    Private m_userName As String
    Private m_password As String
    'ChildNodes - Single
    Private m_schemaInfo As CSchemaData
#End Region

#Region "Properties"
    'Attributes
    Public Property Server As String
        Get
            Return m_server
        End Get
        Set(ByVal value As String)
            m_server = value
        End Set
    End Property
    Public Property Database As String
        Get
            Return m_database
        End Get
        Set(ByVal value As String)
            m_database = value
        End Set
    End Property
    Public Property WindowsAuthentication As Boolean
        Get
            Return m_windowsAuthentication
        End Get
        Set(ByVal value As Boolean)
            m_windowsAuthentication = value
        End Set
    End Property
    Public Property UserName As String
        Get
            Return m_userName
        End Get
        Set(ByVal value As String)
            m_userName = value
        End Set
    End Property
    Public Property Password As String
        Get
            Return m_password
        End Get
        Set(ByVal value As String)
            m_password = value
        End Set
    End Property
    'ChildNodes - Single
    Public ReadOnly Property SchemaInfo As CSchemaData
        Get
            If IsNothing(m_schemaInfo) Then
                m_schemaInfo = new CSchemaData(Me)
            End If
            Return m_schemaInfo
        End Get
    End Property
#End Region

#Region "Public - Element Level"
    Public ReadOnly Property Parent As CConnections
        Get
            Return CType(MyBase.ParentElement, CConnections)
        End Get
    End Property
    Public ReadOnly Property Root As CConnections
        Get
            Return CType(MyBase.ParentDocument, CConnections)
        End Get
    End Property
#End Region

#Region "Public - Node Level"
    Public Overrides Sub Import(parent As XmlNode)
        Dim node As XmlNode = ImportSelf(parent)

        'Load Attributes
        m_server = CXml.AttributeStr(node, "Server", m_server)
        m_database = CXml.AttributeStr(node, "Database", m_database)
        m_windowsAuthentication = CXml.AttributeBool(node, "WindowsAuthentication", m_windowsAuthentication)
        m_userName = CXml.AttributeStr(node, "UserName", m_userName)
        m_password = CXml.AttributeStr(node, "Password", m_password)
        'Load ChildNodes - Single
		m_schemaInfo = New CSchemaData(Me, node)
    End Sub
    Public Overrides Sub Export(parent As XmlNode)    
        Dim node As XmlNode = ExportSelf(parent)

        'Attributes
        CXml.AttributeSet(node, "Server", m_server)
        CXml.AttributeSet(node, "Database", m_database)
        CXml.AttributeSet(node, "WindowsAuthentication", m_windowsAuthentication)
        CXml.AttributeSet(node, "UserName", m_userName)
        CXml.AttributeSet(node, "Password", m_password)
        'ChildNodes - Single
		If Not IsNothing(m_schemaInfo) Then m_schemaInfo.Export(node)
    End Sub
#End Region

End Class
