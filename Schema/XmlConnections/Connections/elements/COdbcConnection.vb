Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Xml

Imports Framework

<Serializable(), CLSCompliant(True)> _
Partial Public Class COdbcConnection : Inherits CElement

#Region "Constants"
    Public Const TAG_NAME As String = "OdbcConnection"
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
        m_connectionString = String.Empty
        'ChildNodes - Single
        m_schemaInfo = Nothing
    End Sub
#End Region

#Region "Members"
    'Attributes
    Private m_connectionString As String
    'ChildNodes - Single
    Private m_schemaInfo As CSchemaData
#End Region

#Region "Properties"
    'Attributes
    Public Property ConnectionString As String
        Get
            Return m_connectionString
        End Get
        Set(ByVal value As String)
            m_connectionString = value
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
        m_connectionString = CXml.AttributeStr(node, "ConnectionString", m_connectionString)
        'Load ChildNodes - Single
		m_schemaInfo = New CSchemaData(Me, node)
    End Sub
    Public Overrides Sub Export(parent As XmlNode)    
        Dim node As XmlNode = ExportSelf(parent)

        'Attributes
        CXml.AttributeSet(node, "ConnectionString", m_connectionString)
        'ChildNodes - Single
		If Not IsNothing(m_schemaInfo) Then m_schemaInfo.Export(node)
    End Sub
#End Region

End Class
