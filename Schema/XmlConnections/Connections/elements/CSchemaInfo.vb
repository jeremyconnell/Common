Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Xml

Imports Framework

<Serializable(), CLSCompliant(True)>
Partial Public Class CSchemaData : Inherits CElement

#Region "Constants"
    Public Const TAG_NAME As String = "SchemaInfo"
    Public Overrides ReadOnly Property TagName As String
        Get
            Return TAG_NAME
        End Get
    End Property
#End Region

#Region "Constructors"
    Public Sub New(parent As CElement)
        MyBase.New(parent)
    End Sub
    Friend Sub New(parent As CElement, node As XmlNode)
        MyBase.New(parent, node)
    End Sub
#End Region

#Region "Initial State"
    Protected Overrides Sub ApplyDefaultValues_Auto()
        'Attributes
        m_lastConnectionDate = DateTime.MinValue
        m_outputFolder = String.Empty
        m_outputFolderReadonly = String.Empty
        m_outputFolderEditable = String.Empty
        m_cSharp = False
        m_tablePrefix = String.Empty
        m_storedProcPrefix = "up_"
        m_cSharpNamespace = "SchemaSample"
        m_lastTableName = String.Empty
        m_architecture = Integer.MinValue
        m_sortingColumn = String.Empty
    End Sub
#End Region

#Region "Members"
    'Attributes
    Private m_lastConnectionDate As DateTime
    Private m_outputFolder As String
    Private m_outputFolderReadonly As String
    Private m_outputFolderEditable As String
    Private m_cSharp As Boolean
    Private m_tablePrefix As String
    Private m_storedProcPrefix As String
    Private m_cSharpNamespace As String
    Private m_lastTableName As String
    Private m_architecture As Integer
    Private m_sortingColumn As String
#End Region

#Region "Properties"
    'Attributes
    Public Property LastConnectionDate As DateTime
        Get
            Return m_lastConnectionDate
        End Get
        Set(ByVal value As DateTime)
            m_lastConnectionDate = value
        End Set
    End Property
    Public Property OutputFolder As String
        Get
            Return m_outputFolder
        End Get
        Set(ByVal value As String)
            m_outputFolder = value
        End Set
    End Property
    Public Property OutputFolderReadonly As String
        Get
            Return m_outputFolderReadonly
        End Get
        Set(ByVal value As String)
            m_outputFolderReadonly = value
        End Set
    End Property
    Public Property OutputFolderEditable As String
        Get
            Return m_outputFolderEditable
        End Get
        Set(ByVal value As String)
            m_outputFolderEditable = value
        End Set
    End Property
    Public Property CSharp As Boolean
        Get
            Return m_cSharp
        End Get
        Set(ByVal value As Boolean)
            m_cSharp = value
        End Set
    End Property
    Public Property TablePrefix As String
        Get
            Return m_tablePrefix
        End Get
        Set(ByVal value As String)
            m_tablePrefix = value
        End Set
    End Property
    Public Property StoredProcPrefix As String
        Get
            Return m_storedProcPrefix
        End Get
        Set(ByVal value As String)
            m_storedProcPrefix = value
        End Set
    End Property
    Public Property CSharpNamespace As String
        Get
            Return m_cSharpNamespace
        End Get
        Set(ByVal value As String)
            m_cSharpNamespace = value
        End Set
    End Property
    Public Property LastTableName As String
        Get
            Return m_lastTableName
        End Get
        Set(ByVal value As String)
            m_lastTableName = value
        End Set
    End Property
    Public Property Architecture As Integer
        Get
            Return m_architecture
        End Get
        Set(ByVal value As Integer)
            m_architecture = value
        End Set
    End Property
    Public Property SortingColumn As String
        Get
            Return m_sortingColumn
        End Get
        Set(ByVal value As String)
            m_sortingColumn = value
        End Set
    End Property
#End Region

#Region "Public - Element Level"
    Public ReadOnly Property Parent As CElement
        Get
            Return CType(MyBase.ParentElement, CElement)
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
        m_lastConnectionDate = CXml.AttributeDate(node, "LastConnectionDate", m_lastConnectionDate)
        m_outputFolder = CXml.AttributeStr(node, "OutputFolder", m_outputFolder)
        m_outputFolderReadonly = CXml.AttributeStr(node, "OutputFolderReadonly", m_outputFolderReadonly)
        m_outputFolderEditable = CXml.AttributeStr(node, "OutputFolderEditable", m_outputFolderEditable)
        m_cSharp = CXml.AttributeBool(node, "CSharp", m_cSharp)
        m_tablePrefix = CXml.AttributeStr(node, "TablePrefix", m_tablePrefix)
        m_storedProcPrefix = CXml.AttributeStr(node, "StoredProcPrefix", m_storedProcPrefix)
        m_cSharpNamespace = CXml.AttributeStr(node, "CSharpNamespace", m_cSharpNamespace)
        m_lastTableName = CXml.AttributeStr(node, "LastTableName", m_lastTableName)
        m_architecture = CXml.AttributeInt(node, "Architecture", m_architecture)
        m_sortingColumn = CXml.AttributeStr(node, "SortingColumn", m_sortingColumn)
    End Sub
    Public Overrides Sub Export(parent As XmlNode)
        Dim node As XmlNode = ExportSelf(parent)

        'Attributes
        CXml.AttributeSet(node, "LastConnectionDate", m_lastConnectionDate)
        CXml.AttributeSet(node, "OutputFolder", m_outputFolder)
        CXml.AttributeSet(node, "OutputFolderReadonly", m_outputFolderReadonly)
        CXml.AttributeSet(node, "OutputFolderEditable", m_outputFolderEditable)
        CXml.AttributeSet(node, "CSharp", m_cSharp)
        CXml.AttributeSet(node, "TablePrefix", m_tablePrefix)
        CXml.AttributeSet(node, "StoredProcPrefix", m_storedProcPrefix)
        CXml.AttributeSet(node, "CSharpNamespace", m_cSharpNamespace)
        CXml.AttributeSet(node, "LastTableName", m_lastTableName)
        CXml.AttributeSet(node, "Architecture", m_architecture)
        CXml.AttributeSet(node, "SortingColumn", m_sortingColumn)
    End Sub
#End Region

End Class
