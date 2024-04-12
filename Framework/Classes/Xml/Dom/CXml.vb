'A collection of shared functions that are commonly used when dealing with xml.
'~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
Imports System.Xml
Imports System.Collections.Generic
Imports System.Text

<Serializable(), CLSCompliant(True)> _
Public Class CXml

#Region "Attributes - Get"
    Public Shared Function AttributeBool(ByVal node As XmlNode, ByVal name As String) As Boolean
        Return AttributeBool(node, name, False)
    End Function
    Public Shared Function AttributeBool(ByVal node As XmlNode, ByVal name As String, ByVal defaultValue As Boolean) As Boolean
        Select Case AttributeStr(node, name, defaultValue.ToString()).ToLower()
            Case "true", "yes", "1"
                Return True
            Case Else
                Return False
        End Select
    End Function
    Public Shared Function AttributeDate(ByVal node As XmlNode, ByVal name As String) As DateTime
        Return AttributeDate(node, name, DateTime.MinValue)
    End Function
    Public Shared Function AttributeDate(ByVal node As XmlNode, ByVal name As String, ByVal defaultValue As DateTime) As DateTime
        Dim s As String = AttributeStr(node, name, defaultValue.Ticks.ToString())
        'Parse as date
        Dim d As DateTime = defaultValue
        If DateTime.TryParse(s, d) Then Return d
        'Parse as ticks
        Dim l As Long = defaultValue.Ticks
        Long.TryParse(s, l)
        Return New DateTime(l)
    End Function
    Public Shared Function AttributeDbl(ByVal node As XmlNode, ByVal name As String) As Double
        Return AttributeDbl(node, name, Double.NaN)
    End Function
    Public Shared Function AttributeDbl(ByVal node As XmlNode, ByVal name As String, ByVal defaultValue As Double) As Double
        Dim s As String = AttributeStr(node, name, defaultValue.ToString())
        Dim d As Double = defaultValue
        If Double.TryParse(s, d) Then Return d
        Return defaultValue
    End Function
    Public Shared Function AttributeDec(ByVal node As XmlNode, ByVal name As String) As Decimal
        Return AttributeDec(node, name, Decimal.MinValue)
    End Function
    Public Shared Function AttributeDec(ByVal node As XmlNode, ByVal name As String, ByVal defaultValue As Decimal) As Decimal
        Dim s As String = AttributeStr(node, name, defaultValue.ToString())
        Dim d As Decimal = defaultValue
        If Decimal.TryParse(s, d) Then Return d
        Return defaultValue
    End Function
    Public Shared Function AttributeLong(ByVal node As XmlNode, ByVal name As String) As Long
        Return AttributeLong(node, name, Long.MinValue)
    End Function
    Public Shared Function AttributeLong(ByVal node As XmlNode, ByVal name As String, ByVal defaultValue As Long) As Long
        Dim s As String = AttributeStr(node, name, defaultValue.ToString())
        Dim i As Long = defaultValue
        If Long.TryParse(s, i) Then Return i
        Return defaultValue
    End Function
    Public Shared Function AttributeInt(ByVal node As XmlNode, ByVal name As String) As Integer
        Return AttributeInt(node, name, Integer.MinValue)
    End Function
    Public Shared Function AttributeInt(ByVal node As XmlNode, ByVal name As String, ByVal defaultValue As Integer) As Integer
        Dim s As String = AttributeStr(node, name, defaultValue.ToString())
        Dim i As Integer = defaultValue
        If Integer.TryParse(s, i) Then Return i
        Return defaultValue
    End Function
    Public Shared Function AttributeGuid(ByVal node As XmlNode, ByVal name As String) As Guid
        Return AttributeGuid(node, name, Guid.Empty)
    End Function
    Public Shared Function AttributeGuid(ByVal node As XmlNode, ByVal name As String, ByVal defaultValue As Guid) As Guid
        Dim s As String = AttributeStr(node, name, defaultValue.ToString())
        Try
            Return New Guid(s)
        Catch ex As Exception
            Return defaultValue
        End Try
    End Function
    Public Shared Function AttributeStr(ByVal node As XmlNode, ByVal name As String) As String
        Return AttributeStr(node, name, String.Empty)
    End Function
    Public Shared Function AttributeStr(ByVal node As XmlNode, ByVal name As String, ByVal defaultValue As String) As String
        If IsNothing(node.Attributes) Then Return defaultValue
        Dim a As XmlAttribute = node.Attributes(name)
        If IsNothing(a) Then Return defaultValue
        Return a.Value
    End Function
#End Region

#Region "Attributes - Set"
    Public Shared Sub AttributeSet(ByVal node As XmlNode, ByVal name As String, ByVal value As Boolean)
        AttributeSet(node, name, CStr(IIf(False = value, String.Empty, value.ToString())))
    End Sub
    Public Shared Sub AttributeSet(ByVal node As XmlNode, ByVal name As String, ByVal value As DateTime)
        AttributeSet(node, name, CStr(IIf(DateTime.MinValue.Equals(value), String.Empty, value.Ticks.ToString())))
    End Sub
    Public Shared Sub AttributeSet(ByVal node As XmlNode, ByVal name As String, ByVal value As DateTime, ByVal dateOnly As Boolean)
        If dateOnly Then
            AttributeSet(node, name, value, "d MMM yyyy")
        Else
            AttributeSet(node, name, value, "d MMM yyyy HH:mm:ss")
        End If
    End Sub
    Public Shared Sub AttributeSet(ByVal node As XmlNode, ByVal name As String, ByVal value As DateTime, ByVal format As String)
        AttributeSet(node, name, CStr(IIf(DateTime.MinValue.Equals(value), String.Empty, value.ToString(format))))
    End Sub
    Public Shared Sub AttributeSet(ByVal node As XmlNode, ByVal name As String, ByVal value As Double)
        AttributeSet(node, name, CStr(IIf(Double.IsNaN(value), String.Empty, value.ToString())))
    End Sub
    Public Shared Sub AttributeSet(ByVal node As XmlNode, ByVal name As String, ByVal value As Decimal)
        AttributeSet(node, name, CStr(IIf(Decimal.MinValue.Equals(value), String.Empty, value.ToString())))
    End Sub
    Public Shared Sub AttributeSet(ByVal node As XmlNode, ByVal name As String, ByVal value As Integer)
        AttributeSet(node, name, CStr(IIf(Integer.MinValue = value, String.Empty, value.ToString())))
    End Sub
    Public Shared Sub AttributeSet(ByVal node As XmlNode, ByVal name As String, ByVal value As Long)
        AttributeSet(node, name, CStr(IIf(Long.MinValue = value, String.Empty, value.ToString())))
    End Sub
    Public Shared Sub AttributeSet(ByVal node As XmlNode, ByVal name As String, ByVal value As Guid)
        AttributeSet(node, name, CStr(IIf(Guid.Empty.Equals(value), String.Empty, value.ToString())))
    End Sub
    Public Shared Sub AttributeSet(ByVal node As XmlNode, ByVal name As String, ByVal value As String)
        Dim a As XmlAttribute
        If Not IsNothing(node.Attributes) Then
            a = node.Attributes(name)
            If Not IsNothing(a) Then
                If Len(value) = 0 Then
                    node.Attributes.Remove(a)
                Else
                    a.Value = value
                End If
                Return
            End If
        End If

        If Len(value) = 0 Then Exit Sub
        a = node.OwnerDocument.CreateAttribute(name)
        a.Value = value
        node.Attributes.Append(a)
    End Sub
#End Region

#Region "Nodes - Set"
    Public Shared Function AddNode(ByVal parent As XmlNode, ByVal tagName As String) As XmlNode
        If TypeOf parent Is XmlDocument Then Return CreateRoot(CType(parent, XmlDocument), tagName)

        Dim node As XmlNode = parent.OwnerDocument.CreateNode(XmlNodeType.Element, String.Empty, tagName, String.Empty)
        parent.AppendChild(node)
        Return node
    End Function
    Public Shared Function CreateRoot(ByVal tagName As String) As XmlNode
        Return CreateRoot(New XmlDocument(), tagName)
    End Function
    Public Shared Function CreateRoot(ByVal doc As XmlDocument, ByVal tagName As String) As XmlNode
        Dim root As XmlNode = doc.CreateNode(XmlNodeType.Element, String.Empty, tagName, String.Empty)
        doc.AppendChild(root)
        Return root
    End Function
#End Region

#Region "Nodes - Get"
    Public Shared Function ChildNode(ByVal parent As XmlNode, ByVal tagName As String) As XmlNode
        Return ChildNode(parent, tagName, True)
    End Function
    Public Shared Function ChildNode(ByVal parent As XmlNode, ByVal tagName As String, ByVal create As Boolean) As XmlNode
        Dim i As XmlNode
        For Each i In parent.ChildNodes
            If i.LocalName = tagName Then Return i
        Next
        If create Then
            Return AddNode(parent, tagName)
        Else
            Return Nothing
        End If
        'Throw New Exception("Child node '" & tagName & "' not found in node '" & parent.LocalName & "'")
    End Function
    Public Shared Function ChildNodes(ByVal parent As XmlNode, ByVal tagName As String) As List(Of XmlNode)
        Return ChildNodes(parent, tagName, True)
    End Function
    Public Shared Function ChildNodes(ByVal parent As XmlNode, ByVal tagName As String, ByVal ignoreCase As Boolean) As List(Of XmlNode)
        Dim nodes As New List(Of XmlNode)()
        Dim i As XmlNode
        For Each i In parent.ChildNodes
            If 0 = String.Compare(i.LocalName, tagName, ignoreCase) Then nodes.Add(i)
        Next
        Return nodes
    End Function
#End Region

#Region "NodeAttributes - Get"
    Public Shared Function ChildNodeGetListDate(ByVal node As XmlNode, ByVal tagName As String) As List(Of DateTime)
        Return ChildNodeGetListDate(node, tagName, DateTime.MinValue)
    End Function
    Public Shared Function ChildNodeGetListDate(ByVal node As XmlNode, ByVal tagName As String, ByVal defaultValue As DateTime) As List(Of DateTime)
        Dim nodes As List(Of XmlNode) = ChildNodes(node, tagName)
        Dim list As New List(Of DateTime)(nodes.Count)
        For Each i As XmlNode In nodes
            list.Add(ChildNodeGetDate(i, defaultValue))
        Next
        Return list
    End Function
    Public Shared Function ChildNodeGetDate(ByVal node As XmlNode, ByVal tagName As String) As DateTime
        Return ChildNodeGetDate(node, tagName, DateTime.MinValue)
    End Function
    Public Shared Function ChildNodeGetDate(ByVal node As XmlNode, ByVal tagName As String, ByVal defValue As DateTime) As DateTime
        Dim child As XmlNode = ChildNode(node, tagName)
        Return ChildNodeGetDate(child, defValue)
    End Function
    Public Shared Function ChildNodeGetDate(ByVal child As XmlNode) As DateTime
        Return ChildNodeGetDate(child, DateTime.MinValue)
    End Function
    Public Shared Function ChildNodeGetDate(ByVal child As XmlNode, ByVal defValue As DateTime) As DateTime
        If IsNothing(child) Then Return defValue
        DateTime.TryParse(child.InnerText, defValue)
        Return defValue
    End Function


    Public Shared Function ChildNodeGetListBool(ByVal node As XmlNode, ByVal tagName As String) As List(Of Boolean)
        Return ChildNodeGetListBool(node, tagName, False)
    End Function
    Public Shared Function ChildNodeGetListBool(ByVal node As XmlNode, ByVal tagName As String, ByVal defaultValue As Boolean) As List(Of Boolean)
        Dim nodes As List(Of XmlNode) = ChildNodes(node, tagName)
        Dim list As New List(Of Boolean)(nodes.Count)
        For Each i As XmlNode In nodes
            list.Add(ChildNodeGetBool(i, defaultValue))
        Next
        Return list
    End Function
    Public Shared Function ChildNodeGetBool(ByVal node As XmlNode, ByVal tagName As String) As Boolean
        Return ChildNodeGetBool(node, tagName, False)
    End Function
    Public Shared Function ChildNodeGetBool(ByVal node As XmlNode, ByVal tagName As String, ByVal defValue As Boolean) As Boolean
        Dim child As XmlNode = ChildNode(node, tagName)
        Return ChildNodeGetBool(child, defValue)
    End Function
    Public Shared Function ChildNodeGetBool(ByVal child As XmlNode) As Boolean
        Return ChildNodeGetBool(child, False)
    End Function
    Public Shared Function ChildNodeGetBool(ByVal child As XmlNode, ByVal defValue As Boolean) As Boolean
        If IsNothing(child) Then Return defValue
        Boolean.TryParse(child.InnerText, defValue)
        Return defValue
    End Function


    Public Shared Function ChildNodeGetListGuid(ByVal node As XmlNode, ByVal tagName As String) As List(Of Guid)
        Return ChildNodeGetListGuid(node, tagName, Guid.Empty)
    End Function
    Public Shared Function ChildNodeGetListGuid(ByVal node As XmlNode, ByVal tagName As String, ByVal defaultValue As Guid) As List(Of Guid)
        Dim nodes As List(Of XmlNode) = ChildNodes(node, tagName)
        Dim list As New List(Of Guid)(nodes.Count)
        For Each i As XmlNode In nodes
            list.Add(ChildNodeGetGuid(i, defaultValue))
        Next
        Return list
    End Function
    Public Shared Function ChildNodeGetGuid(ByVal node As XmlNode, ByVal tagName As String) As Guid
        Return ChildNodeGetGuid(node, tagName, Guid.Empty)
    End Function
    Public Shared Function ChildNodeGetGuid(ByVal node As XmlNode, ByVal tagName As String, ByVal defValue As Guid) As Guid
        Dim child As XmlNode = ChildNode(node, tagName)
        Return ChildNodeGetGuid(child, defValue)
    End Function
    Public Shared Function ChildNodeGetGuid(ByVal child As XmlNode) As Guid
        Return ChildNodeGetGuid(child, Guid.Empty)
    End Function
    Public Shared Function ChildNodeGetGuid(ByVal child As XmlNode, ByVal defValue As Guid) As Guid
        If IsNothing(child) Then Return defValue
        Try
            Return New Guid(child.InnerText)
        Catch ex As Exception
            Return defValue
        End Try
    End Function


    Public Shared Function ChildNodeGetListDbl(ByVal node As XmlNode, ByVal tagName As String) As List(Of Double)
        Return ChildNodeGetListDbl(node, tagName, Double.NaN)
    End Function
    Public Shared Function ChildNodeGetListDbl(ByVal node As XmlNode, ByVal tagName As String, ByVal defaultValue As Double) As List(Of Double)
        Dim nodes As List(Of XmlNode) = ChildNodes(node, tagName)
        Dim list As New List(Of Double)(nodes.Count)
        For Each i As XmlNode In nodes
            list.Add(ChildNodeGetDbl(i, defaultValue))
        Next
        Return list
    End Function
    Public Shared Function ChildNodeGetDbl(ByVal node As XmlNode, ByVal tagName As String) As Double
        Return ChildNodeGetDbl(node, tagName, Double.NaN)
    End Function
    Public Shared Function ChildNodeGetDbl(ByVal node As XmlNode, ByVal tagName As String, ByVal defValue As Double) As Double
        Dim child As XmlNode = ChildNode(node, tagName)
        Return ChildNodeGetDbl(child, defValue)
    End Function
    Public Shared Function ChildNodeGetDbl(ByVal child As XmlNode) As Double
        Return ChildNodeGetDbl(child, Double.NaN)
    End Function
    Public Shared Function ChildNodeGetDbl(ByVal child As XmlNode, ByVal defValue As Double) As Double
        If IsNothing(child) Then Return defValue
        Double.TryParse(child.InnerText, defValue)
        Return defValue
    End Function


    Public Shared Function ChildNodeGetListDec(ByVal node As XmlNode, ByVal tagName As String) As List(Of Decimal)
        Return ChildNodeGetListDec(node, tagName, Decimal.MinValue)
    End Function
    Public Shared Function ChildNodeGetListDec(ByVal node As XmlNode, ByVal tagName As String, ByVal defaultValue As Decimal) As List(Of Decimal)
        Dim nodes As List(Of XmlNode) = ChildNodes(node, tagName)
        Dim list As New List(Of Decimal)(nodes.Count)
        For Each i As XmlNode In nodes
            list.Add(ChildNodeGetDec(i, defaultValue))
        Next
        Return list
    End Function
    Public Shared Function ChildNodeGetDec(ByVal node As XmlNode, ByVal tagName As String) As Decimal
        Return ChildNodeGetDec(node, tagName, Decimal.MinValue)
    End Function
    Public Shared Function ChildNodeGetDec(ByVal node As XmlNode, ByVal tagName As String, ByVal defValue As Decimal) As Decimal
        Dim child As XmlNode = ChildNode(node, tagName)
        Return ChildNodeGetDec(child, defValue)
    End Function
    Public Shared Function ChildNodeGetDec(ByVal child As XmlNode) As Decimal
        Return ChildNodeGetDec(child, Decimal.MinValue)
    End Function
    Public Shared Function ChildNodeGetDec(ByVal child As XmlNode, ByVal defValue As Decimal) As Decimal
        If IsNothing(child) Then Return defValue
        Decimal.TryParse(child.InnerText, defValue)
        Return defValue
    End Function


    Public Shared Function ChildNodeGetListLong(ByVal node As XmlNode, ByVal tagName As String) As List(Of Long)
        Return ChildNodeGetListLong(node, tagName, Long.MinValue)
    End Function
    Public Shared Function ChildNodeGetListLong(ByVal node As XmlNode, ByVal tagName As String, ByVal defaultValue As Long) As List(Of Long)
        Dim nodes As List(Of XmlNode) = ChildNodes(node, tagName)
        Dim list As New List(Of Long)(nodes.Count)
        For Each i As XmlNode In nodes
            list.Add(ChildNodeGetLong(i, defaultValue))
        Next
        Return list
    End Function
    Public Shared Function ChildNodeGetLong(ByVal node As XmlNode, ByVal tagName As String) As Long
        Return ChildNodeGetLong(node, tagName, Long.MinValue)
    End Function
    Public Shared Function ChildNodeGetLong(ByVal node As XmlNode, ByVal tagName As String, ByVal defValue As Long) As Long
        Dim child As XmlNode = ChildNode(node, tagName)
        Return ChildNodeGetLong(child, defValue)
    End Function
    Public Shared Function ChildNodeGetLong(ByVal child As XmlNode) As Long
        Return ChildNodeGetLong(child, Long.MinValue)
    End Function
    Public Shared Function ChildNodeGetLong(ByVal child As XmlNode, ByVal defValue As Long) As Long
        If IsNothing(child) Then Return defValue
        Long.TryParse(child.InnerText, defValue)
        Return defValue
    End Function


    Public Shared Function ChildNodeGetListInt(ByVal node As XmlNode, ByVal tagName As String) As List(Of Integer)
        Return ChildNodeGetListInt(node, tagName, Integer.MinValue)
    End Function
    Public Shared Function ChildNodeGetListInt(ByVal node As XmlNode, ByVal tagName As String, ByVal defaultValue As Integer) As List(Of Integer)
        Dim nodes As List(Of XmlNode) = ChildNodes(node, tagName)
        Dim list As New List(Of Integer)(nodes.Count)
        For Each i As XmlNode In nodes
            list.Add(ChildNodeGetInt(i, defaultValue))
        Next
        Return list
    End Function
    Public Shared Function ChildNodeGetInt(ByVal node As XmlNode, ByVal tagName As String) As Integer
        Return ChildNodeGetInt(node, tagName, Integer.MinValue)
    End Function
    Public Shared Function ChildNodeGetInt(ByVal node As XmlNode, ByVal tagName As String, ByVal defValue As Integer) As Integer
        Dim child As XmlNode = ChildNode(node, tagName)
        Return ChildNodeGetInt(child, defValue)
    End Function
    Public Shared Function ChildNodeGetInt(ByVal child As XmlNode) As Integer
        Return ChildNodeGetInt(child, Integer.MinValue)
    End Function
    Public Shared Function ChildNodeGetInt(ByVal child As XmlNode, ByVal defValue As Integer) As Integer
        If IsNothing(child) Then Return defValue
        Integer.TryParse(child.InnerText, defValue)
        Return defValue
    End Function


    Public Shared Function ChildNodeGetListStr(ByVal node As XmlNode, ByVal tagName As String) As List(Of String)
        Return ChildNodeGetListStr(node, tagName, String.Empty)
    End Function
    Public Shared Function ChildNodeGetListStr(ByVal node As XmlNode, ByVal tagName As String, ByVal defaultValue As String) As List(Of String)
        Dim nodes As List(Of XmlNode) = ChildNodes(node, tagName)
        Dim list As New List(Of String)(nodes.Count)
        For Each i As XmlNode In nodes
            list.Add(ChildNodeGetStr(i, defaultValue))
        Next
        Return list
    End Function
    Public Shared Function ChildNodeGetStr(ByVal node As XmlNode, ByVal tagName As String) As String
        Return ChildNodeGetStr(node, tagName, String.Empty)
    End Function
    Public Shared Function ChildNodeGetStr(ByVal node As XmlNode, ByVal tagName As String, ByVal defValue As String) As String
        Dim child As XmlNode = ChildNode(node, tagName, False)
        If IsNothing(child) Then Return defValue
        Return child.InnerText
    End Function
    Public Shared Function ChildNodeGetStr(ByVal child As XmlNode) As String
        If IsNothing(child) Then Return String.Empty
        Return child.InnerText
    End Function
#End Region

#Region "NodeAttributes - Set"
    Public Shared Function ChildNodeSet(ByVal node As XmlNode, ByVal tagName As String, ByVal value As DateTime) As XmlNode
        If DateTime.MinValue = value Then Return Nothing
        Return ChildNodeSet(node, tagName, value.ToString())
    End Function
    Public Shared Function ChildNodeSet(ByVal node As XmlNode, ByVal tagName As String, ByVal value As Boolean) As XmlNode
        Return ChildNodeSet(node, tagName, value.ToString())
    End Function
    Public Shared Function ChildNodeSet(ByVal node As XmlNode, ByVal tagName As String, ByVal value As Guid) As XmlNode
        If value = Guid.Empty Then Return Nothing
        Return ChildNodeSet(node, tagName, value.ToString())
    End Function
    Public Shared Function ChildNodeSet(ByVal node As XmlNode, ByVal tagName As String, ByVal value As Double) As XmlNode
        If Double.IsNaN(value) Then Return Nothing
        Return ChildNodeSet(node, tagName, value.ToString())
    End Function
    Public Shared Function ChildNodeSet(ByVal node As XmlNode, ByVal tagName As String, ByVal value As Decimal) As XmlNode
        If value = Decimal.MinValue Then Return Nothing
        Return ChildNodeSet(node, tagName, value.ToString())
    End Function
    Public Shared Function ChildNodeSet(ByVal node As XmlNode, ByVal tagName As String, ByVal value As Integer) As XmlNode
        If value = Integer.MinValue Then Return Nothing
        Return ChildNodeSet(node, tagName, value.ToString())
    End Function
    Public Shared Function ChildNodeSet(ByVal node As XmlNode, ByVal tagName As String, ByVal value As String) As XmlNode
        Dim child As XmlNode = AddNode(node, tagName)
        child.InnerText = value
        Return child
    End Function


    Public Shared Sub ChildNodeSet(ByVal node As XmlNode, ByVal tagName As String, ByVal list As List(Of DateTime))
        If IsNothing(list) Then Return
        For Each i As DateTime In list
            ChildNodeSet(node, tagName, i)
        Next
    End Sub
    Public Shared Sub ChildNodeSet(ByVal node As XmlNode, ByVal tagName As String, ByVal list As List(Of Boolean))
        If IsNothing(list) Then Return
        For Each i As Boolean In list
            ChildNodeSet(node, tagName, i)
        Next
    End Sub
    Public Shared Sub ChildNodeSet(ByVal node As XmlNode, ByVal tagName As String, ByVal list As List(Of Guid))
        If IsNothing(list) Then Return
        For Each i As Guid In list
            ChildNodeSet(node, tagName, i)
        Next
    End Sub
    Public Shared Sub ChildNodeSet(ByVal node As XmlNode, ByVal tagName As String, ByVal list As List(Of Double))
        If IsNothing(list) Then Return
        For Each i As Double In list
            ChildNodeSet(node, tagName, i)
        Next
    End Sub
    Public Shared Sub ChildNodeSet(ByVal node As XmlNode, ByVal tagName As String, ByVal list As List(Of Decimal))
        If IsNothing(list) Then Return
        For Each i As Decimal In list
            ChildNodeSet(node, tagName, i)
        Next
    End Sub
    Public Shared Sub ChildNodeSet(ByVal node As XmlNode, ByVal tagName As String, ByVal list As List(Of Integer))
        If IsNothing(list) Then Return
        For Each i As Integer In list
            ChildNodeSet(node, tagName, i)
        Next
    End Sub
    Public Shared Sub ChildNodeSet(ByVal node As XmlNode, ByVal tagName As String, ByVal list As List(Of String))
        If IsNothing(list) Then Return
        For Each i As String In list
            ChildNodeSet(node, tagName, i)
        Next
    End Sub
#End Region

#Region "Utilities"
    Public Shared Function RemoveInvalidChars(ByVal xml As String) As String
        'Source: http://cse-mjmcl.cse.bris.ac.uk/blog/2007/02/14/1171465494443.html
        'SeeAlso: http://www.ascii.cl/htmlcodes.htm
        If String.IsNullOrEmpty(xml) Then Return String.Empty

        Dim sb As New StringBuilder(xml.Length, xml.Length)
        For Each current As Char In xml.ToCharArray()
            Dim i As Integer = AscW(current)
            If (i >= 32 AndAlso i <= 55295) OrElse (i >= 57344 AndAlso i <= 65533) OrElse i = 9 OrElse i = 10 OrElse i = 13 Then
                sb.Append(current)
            End If
        Next
        Return sb.ToString()
    End Function
#End Region

End Class
