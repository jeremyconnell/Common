Imports System.Xml

<Serializable(), CLSCompliant(True)> _
Partial Public MustInherit Class CElement

#Region "Constructors"
    Protected Sub New() 'Eg. For rare Serialisation cases
    End Sub
    Public Sub New(ByVal parent As CElement)
        m_parent = parent
        ApplyDefaultValues()
    End Sub
    Public Sub New(ByVal parent As CElement, ByVal node As XmlNode)
        Me.New(parent)
        Me.Import(node)
        ImportCompleted()
    End Sub
#End Region

#Region "Abstract/Virtual"
    Public MustOverride ReadOnly Property TagName() As String
    Public MustOverride Sub Import(ByVal node As XmlNode)
    Public MustOverride Sub Export(ByVal parent As XmlNode)

    Protected Overridable Sub ApplyDefaultValues_Auto()
    End Sub
    Protected Overridable Sub ApplyDefaultValues_Custom()
    End Sub
    Protected Overridable Sub ImportCompleted()
    End Sub
    Private Sub ApplyDefaultValues()
        ApplyDefaultValues_Auto()
        ApplyDefaultValues_Custom()
    End Sub
#End Region

#Region "Parent/Root"
    Private m_parent As CElement
    Public ReadOnly Property ParentElement() As CElement
        Get
            Return m_parent
        End Get
    End Property
    Public ReadOnly Property ParentDocument() As CDocument
        Get
            If IsNothing(m_parent) Then Return CType(Me, CDocument)
            Return CType(m_parent.ParentDocument, CDocument)
        End Get
    End Property
#End Region

#Region "Utilities"
    'Import/Export - Can service a master index (e.g. on id attribute) from here
    Protected Overridable Function ImportSelf(ByVal node As XmlNode) As XmlNode
        If String.Compare(node.LocalName, Me.TagName, True) = 0 Then Return node
        'Try tagname
        ImportSelf = CXml.ChildNode(node, Me.TagName, False)
        'Try lowercase
        If IsNothing(ImportSelf) Then ImportSelf = CXml.ChildNode(node, Me.TagName.ToLower, False)
        'Try uppercase
        If IsNothing(ImportSelf) Then ImportSelf = CXml.ChildNode(node, Me.TagName.ToUpper, False)
        'Create
        If IsNothing(ImportSelf) Then ImportSelf = CXml.ChildNode(node, Me.TagName.ToUpper, True)
    End Function
    Protected Overridable Function ExportSelf(ByVal parent As XmlNode) As XmlNode
        Return CXml.AddNode(parent, Me.TagName)
    End Function
    Public Overloads Function AddNode(ByVal parent As XmlNode) As XmlNode
        Return CXml.AddNode(parent, TagName)
    End Function
#End Region

End Class
