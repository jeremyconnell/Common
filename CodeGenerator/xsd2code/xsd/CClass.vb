Imports System.Xml

Public Class CClass

#Region "Constructor"
    Public Sub New(ByVal node As XmlNode, ByVal parent As CClass, ByVal isRoot As Boolean, ByVal isRef As Boolean)
        'Read tagname
        _tagName = CXml.AttributeStr(node, "name")
        If String.IsNullOrEmpty(_tagName) Then
            _tagName = CXml.AttributeStr(node, "ref")
        End If

        'Store/derive parent info
        _isRoot = isRoot
        _isRef = isRef
        _parent = parent

        'Init members
        _attributes = New List(Of CAttribute)
        _children = New List(Of CChild)

        'Update parent
        If Not IsNothing(parent) Then
            Dim isMany As Boolean = ("unbounded" = CXml.AttributeStr(node, "maxOccurs"))
            parent.Children.Add(New CChild(isMany, Me))
        End If
    End Sub
#End Region

#Region "Members"
    Private _tagName As String

    Private _parent As CClass
    Private _isRoot As Boolean
    Private _isRef As Boolean

    Private _attributes As List(Of CAttribute)
    Private _children As List(Of CChild)

    Private _multipleParents As Boolean = False
#End Region

#Region "public"
    'Direct Accessors
    Public ReadOnly Property TagName() As String
        Get
            Return _tagName
        End Get
    End Property
    Public ReadOnly Property IsRoot() As Boolean
        Get
            Return _isRoot
        End Get
    End Property
    Public ReadOnly Property Attributes() As List(Of CAttribute)
        Get
            Return _attributes
        End Get
    End Property
    Public ReadOnly Property Parent() As CClass
        Get
            Return _parent
        End Get
    End Property
    Public ReadOnly Property Children() As List(Of CChild)
        Get
            Return _children
        End Get
    End Property
    Public Property MultipleParents() As Boolean
        Get
            Return _multipleParents
        End Get
        Set(ByVal value As Boolean)
            _multipleParents = value
        End Set
    End Property

    'Derived
    Public ReadOnly Property Singles() As List(Of CChild)
        Get
            Return ChildrenByType(False)
        End Get
    End Property
    Public ReadOnly Property Lists() As List(Of CChild)
        Get
            Return ChildrenByType(True)
        End Get
    End Property
    Private ReadOnly Property ChildrenByType(ByVal isList As Boolean) As List(Of CChild)
        Get
            Dim list As New List(Of CChild)(_children.Count)
            For Each i As CChild In _children
                If isList = i.Many Then list.Add(i)
            Next
            Return list
        End Get
    End Property
    Public ReadOnly Property AttributesNormal() As List(Of CAttribute)
        Get
            Return AttributesByType(False)
        End Get
    End Property
    Public ReadOnly Property AttributesAsNodes() As List(Of CAttribute)
        Get
            Return AttributesByType(True)
        End Get
    End Property
    Private ReadOnly Property AttributesByType(ByVal useTags As Boolean) As List(Of CAttribute)
        Get
            Dim list As New List(Of CAttribute)(_attributes.Count)
            For Each i As CAttribute In _attributes
                If useTags = i.UseTags Then list.Add(i)
            Next
            Return list
        End Get
    End Property
    Public ReadOnly Property ParentClassName() As String
        Get
            If IsNothing(Parent) Or _multipleParents Then Return "CElement"
            Return Parent.ClassName
        End Get
    End Property
    Public ReadOnly Property RootClassName() As String
        Get
            If IsNothing(Parent) Then Return ClassName
            Return Parent.RootClassName
        End Get
    End Property
    Public ReadOnly Property ClassName() As String
        Get
            Dim safe As String = _tagName.Replace("-", "_")
            Return CTableInformation.CLASS_PREFIX & safe.Substring(0, 1).ToUpper & safe.Substring(1)
        End Get
    End Property
    Public ReadOnly Property ClassNameList() As String
        Get
            Return ClassName & "List".Replace("-", "_")
        End Get
    End Property
#End Region

End Class
