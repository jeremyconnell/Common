Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Xml

Imports Framework

<Serializable(), CLSCompliant(True)> _
Public Class CSchemaInfoList : Inherits List(Of CSchemaData)

#Region "Constructors (Import)"
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal copyFrom As IEnumerable(Of CSchemaData))
        MyBase.New(copyFrom)
    End Sub
    Public Sub New(parent As CElement, node As XmlNode)
        MyBase.New(node.ChildNodes.Count)    
        For Each i As XmlNode In CXml.ChildNodes(node, CSchemaData.TAG_NAME)
            Add(New CSchemaData(parent, i))
        Next
    End Sub    
#End Region

#Region "Public (Export)"
    Public Sub Export(node As XmlNode)
        For Each i As CSchemaData In Me
            i.Export(node)
        Next
    End Sub
#End Region

#Region "Properties (Indices)"
    'Sample 1 of 2: Unique Index (Index on the PK), returns a single item (may be Nothing)
'    Public Function GetById(pageId As Integer) As CPage
'        Dim c As CPage = Nothing
'        IndexById.TryGetValue(pageId, c)
'        Return c
'    End Function
'    <NonSerialized()> _
'    Private m_indexById As Dictionary(Of Integer, CPage)
'    Private ReadOnly Property IndexById As Dictionary(Of Integer, CPage)
'        Get
'            If Not IsNothing(m_indexById) Then
'                If Me.Count = m_indexById.Count Then Return m_indexById
'            End If
'
'            m_indexById = New Dictionary(Of Integer, CPage)(Me.Count)
'            For Each i As CPage In Me
'                m_indexById(i.PageId) = i
'            Next
'            Return m_indexById
'        End Get
'    End Property    
    
    
    'Sample 2 of 2: Index - Non-Unique (Index on a FK), returns a subset of the list (may be empty)
'    Public Function GetByCountryId(countryId As Integer) As CStateList
'        Dim temp As CStateList = Nothing
'        If Not IndexByCountryId.TryGetValue(countryId, temp) Then        
'            temp = New CStateList()
'            IndexByCountryId(countryId) = temp
'        End If
'        Return temp
'    End Function
'    <NonSerialized()> _
'    Private m_indexByCountryId As Dictionary(Of Integer, CStateList)
'    Private ReadOnly Property IndexByCountryId As Dictionary(Of Integer, CStateList)
'        Get
'            If IsNothing(m_indexByCountryId) Then            
'                'Instantiate
'                Dim index As New Dictionary(Of Integer, CStateList)()
'
'                'Populate
'                Dim temp As CStateList = Nothing
'                For Each i As CState In Me
'                    If Not index.TryGetValue(i.StateCountryId, temp)
'                        temp = New CStateList()
'                        index(i.StateCountryId) = temp                    
'                    temp.Add(i)
'                Next
'
'                'Store
'                m_indexByCountryId = index
'            End If
'            Return m_indexByCountryId
'        End Get
'    End Property
#End Region

End Class
