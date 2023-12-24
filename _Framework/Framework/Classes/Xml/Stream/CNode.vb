Imports System.Xml

<Serializable(), CLSCompliant(True)> _
Public MustInherit Class CNode

#Region "Constructors"
    Public Sub New(ByVal parent As CNode)
        _parent = parent
    End Sub
    Public Sub New(ByVal parent As CNode, ByVal reader As XmlReader)
        Me.New(parent)
        Me.Read(reader)
    End Sub
#End Region

#Region "Abstract"
    Public MustOverride ReadOnly Property TagName() As String
    Public MustOverride Sub Read(ByVal reader As XmlReader)
    Public MustOverride Sub Write(ByVal writer As XmlWriter)
#End Region

#Region "Parent/Root"
    Private _parent As CNode
    Public ReadOnly Property ParentElement() As CNode
        Get
            Return _parent
        End Get
    End Property
    Public ReadOnly Property ParentDocument() As CRoot
        Get
            If IsNothing(_parent) Then
                Return CType(Me, CRoot)
            Else
                Return CType(_parent, CRoot)
            End If
        End Get
    End Property
#End Region

End Class
