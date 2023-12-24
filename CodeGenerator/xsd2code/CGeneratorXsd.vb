Imports System.Xml
Imports System.Text

Public Class CGeneratorXsd

#Region "Constructors"
    Public Sub New(ByVal dotnetNameSpace As String, ByVal language As ELanguage)
        _nameSpace = dotnetNameSpace
        _langauge = language

        _templatesPath = String.Concat("/dotnet/", language, "/xsd/")
    End Sub
#End Region

#Region "Members"
    Private _templatesPath As String
    Private _nameSpace As String
    Private _langauge As ELanguage

    Private _outputFolderPath As String
    Private _xsd As XmlDocument
    Private _classes As List(Of CClass)
#End Region

#Region "Properties - Private"
    Private ReadOnly Property CSharp() As Boolean
        Get
            Return _langauge = ELanguage.CSharp
        End Get
    End Property
#End Region

#Region "Public Interface"
    Public Function Generate(ByVal xsdFilePath As String, ByVal outputFolderPath As String) As Integer
        _xsd = New XmlDocument()
        _xsd.Load(xsdFilePath)

        _outputFolderPath = outputFolderPath & "/" & IO.Path.GetFileNameWithoutExtension(xsdFilePath)

        Return Generate()
    End Function
#End Region

#Region "Private - High Level"
    Private Function Generate() As Integer
        _classes = New List(Of CClass)

        FindElements(_xsd)

        Dim count As Integer = 0
        For Each i As CClass In _classes
            WriteClass(i)
            count += 1
            If Not i.IsRoot Then
                WriteClassList(i)
                count += 1
            End If
        Next

        Return count
    End Function
#End Region

#Region "Private - Filenames"
    Private Function OutputFilePath(ByVal c As CClass, ByVal isList As Boolean, ByVal isCustomisable As Boolean) As String
        'Get folder name
        Dim s As New StringBuilder(_outputFolderPath)
        If isCustomisable Then s.Append("/customisation")
        If Not c.IsRoot Then s.Append("/elements")
        If Not IO.Directory.Exists(s.ToString) Then IO.Directory.CreateDirectory(s.ToString)

        'Add the fileName
        s.Append("/")
        If isList Then s.Append(c.ClassNameList) Else s.Append(c.ClassName)
        If _langauge = ELanguage.CSharp Then s.Append(".cs") Else s.Append(".vb")

        Return s.ToString
    End Function
#End Region

#Region "Private - Xsd Processing"
    Private Sub FindElements(ByVal node As XmlNode)
        'Root node should always be first (normally only one)
        Dim root As XmlNode = GetFirstRootNode(node)
        If IsNothing(root) Then Exit Sub

        'Instantiate parent early, so refs can link to it
        Dim rootClass As New CClass(root, Nothing, True, False)
        _classes.Add(rootClass)

        'Schema sometimes have other shared elements at the root level (see the "ref" attribute)
        'Process these first, so they can be referred to by the main tree
        Dim refs As List(Of XmlNode) = GetOtherRootNodes(node)
        For Each i As XmlNode In refs
            FindElements(i, rootClass, True)
        Next

        'Process the main tree
        FindElements(root, rootClass, False)
    End Sub
    Private Sub FindElements(ByVal node As XmlNode, ByVal parent As CClass, ByVal isRef As Boolean)
        Select Case node.LocalName
            Case "attribute"
                parent.Attributes.Add(New CAttribute(node))
                Exit Sub

            Case "element"
                'Special case - shallow (text-only) ones are be mapped as attributes of parent (should be modelled that way, but tags are more human-readable)
                If IsShallowElement(node) Then
                    Dim a As New CAttribute(node)
                    If Len(a.Name.Trim) > 0 Then
                        parent.Attributes.Add(a)
                        Exit Sub
                    End If
                End If

                'Normal case - deep child element
                Dim child As New CClass(node, parent, False, isRef)
                If TagExists(child.TagName) Then
                    'If name is not unique, discard this one and extend the existing class instead
                    If parent.IsRoot Then parent.Children.RemoveAt(parent.Children.Count - 1)
                    parent = GetTag(child.TagName)
                    parent.MultipleParents = True
                Else
                    'Otherwise add to the list
                    _classes.Add(child)
                    parent = child
                End If
        End Select

        'Recursion
        If IsNothing(node.ChildNodes) Then Exit Sub
        For Each i As XmlNode In node.ChildNodes
            FindElements(i, parent, False)
        Next
    End Sub
    Private Function GetFirstRootNode(ByVal node As XmlNode) As XmlNode
        For Each i As XmlNode In node.ChildNodes
            If i.LocalName.ToLower = "element" Then
                If i.Prefix.ToLower = "xs" Then Return i
            End If
            Dim fr As XmlNode = GetFirstRootNode(i)
            If Not fr Is Nothing Then Return fr
        Next
        Return Nothing
    End Function
    Private Function GetOtherRootNodes(ByVal node As XmlNode) As List(Of XmlNode)
        Dim firstRoot As XmlNode = GetFirstRootNode(node)
        Dim allRootNodes As New List(Of XmlNode)()
        For Each i As XmlNode In firstRoot.ParentNode.ChildNodes
            allRootNodes.Add(i)
        Next
        allRootNodes.RemoveAt(0)
        Return allRootNodes
    End Function
    Private Function IsShallowElement(ByVal node As XmlNode) As Boolean
        Return Not node.HasChildNodes 'AndAlso (IsNothing(node.Attributes) OrElse node.Attributes.Count = 0)
    End Function
    Private Function TagExists(ByVal tagName As String) As Boolean
        Return Not GetTag(tagName) Is Nothing
    End Function
    Private Function GetTag(ByVal tagName As String) As CClass
        For Each i As CClass In _classes
            If i.TagName.ToLower = tagName.ToLower Then Return i
        Next
        Return Nothing
    End Function
#End Region

#Region "Private - Template logic"
    Private Sub WriteClass(ByVal c As CClass)
        Dim templateName As String = CStr(IIf(c.IsRoot, "classDocument.txt", "classElement.txt"))
        With New CTemplate(templateName, _templatesPath)
            .Replace("namespace", _nameSpace)
            .Replace("className", c.ClassName)
            .Replace("parentClassName", c.ParentClassName)
            .Replace("rootClassName", c.RootClassName)
            .Replace("tagName", c.TagName)
            .Replace("initChildObjects", InitChildObjects(c))
            .Replace("members", Members(c))
            .Replace("properties", Properties(c))
            .Replace("importData", ImportData(c))
            .Replace("exportData", ExportData(c))

            .SaveAs(OutputFilePath(c, False, False))
        End With

        With New CTemplate("customClass.txt", _templatesPath)
            .Replace("namespace", _nameSpace)
            .Replace("className", c.ClassName)
            Dim filePath As String = OutputFilePath(c, False, True)
            If Not IO.File.Exists(filePath) Then .SaveAs(filePath)
        End With
    End Sub
    Private Sub WriteClassList(ByVal c As CClass)
        With New CTemplate("classList.txt", _templatesPath)
            .Replace("namespace", _nameSpace)
            .Replace("className", c.ClassName)
            .Replace("parentClassName", c.ParentClassName)

            .SaveAs(OutputFilePath(c, True, False))
        End With

        With New CTemplate("customClassList.txt", _templatesPath)
            .Replace("namespace", _nameSpace)
            .Replace("className", c.ClassName)
            Dim filePath As String = OutputFilePath(c, True, True)
            If Not IO.File.Exists(filePath) Then .SaveAs(filePath)
        End With
    End Sub
    Private Function Members(ByVal c As CClass) As String
        Dim sb As New StringBuilder
        With New CTemplate("member.txt", _templatesPath)

            If c.Attributes.Count > 0 Then sb.Append(Comment("Attributes"))
            For Each a As CAttribute In c.Attributes
                .Reset()
                .Replace("dataType", a.DataType(CSharp))
                .Replace("nameCamelCase", a.NameCamelCase)
                sb.Append(.Template)
            Next

            If c.Singles.Count > 0 Then sb.Append(Comment("ChildNodes - Single"))
            For Each i As CChild In c.Singles
                .Reset()
                .Replace("dataType", i.TypeName)
                .Replace("nameCamelCase", i.NameCamelCase)
                sb.Append(.Template)
            Next

            If c.Lists.Count > 0 Then sb.Append(Comment("ChildNodes - Collections"))
            For Each i As CChild In c.Lists
                .Reset()
                .Replace("dataType", i.TypeName)
                .Replace("nameCamelCase", i.NameCamelCase)
                sb.Append(.Template)
            Next
        End With
        Return sb.ToString
    End Function
    Private Function InitChildObjects(ByVal c As CClass) As String
        Dim sb As New StringBuilder

        If c.Attributes.Count > 0 Then sb.Append(Comment("Attributes", 1))
        With New CTemplate("initAttribute.txt", _templatesPath)
            For Each i As CAttribute In c.Attributes
                .Reset()
                .Replace("nameCamelCase", i.NameCamelCase)
                .Replace("defaultValue", i.DefaultValue(CSharp))
                sb.Append(.Template)
            Next
        End With

        If c.Singles.Count > 0 Then sb.Append(Comment("ChildNodes - Single", 1))
        With New CTemplate("initChildObject.txt", _templatesPath)
            For Each i As CChild In c.Singles
                .Reset()
                .Replace("nameCamelCase", i.NameCamelCase)
                .Replace("nameOfClass", i.TypeName)
                sb.Append(.Template)
            Next
        End With

        If c.Lists.Count > 0 Then sb.Append(Comment("ChildNodes - Collections", 1))
        With New CTemplate("initChildCollection.txt", _templatesPath)
            For Each i As CChild In c.Lists
                .Reset()
                .Replace("nameCamelCase", i.NameCamelCase)
                .Replace("nameOfClass", i.TypeName)
                sb.Append(.Template)
            Next
        End With
        Return sb.ToString
    End Function
    Private Function Properties(ByVal c As CClass) As String
        Dim sb As New StringBuilder

        If c.Attributes.Count > 0 Then sb.Append(Comment("Attributes"))
        For Each a As CAttribute In c.Attributes
            With New CTemplate("propertyAttribute.txt", _templatesPath)
                .Reset()
                .Replace("dataType", a.DataType(CSharp))
                .Replace("nameCamelCase", a.NameCamelCase)
                .Replace("nameProperCase", a.NameProperCase)
                sb.Append(.Template)
            End With
        Next

        If c.Singles.Count > 0 Then sb.Append(Comment("ChildNodes - Single"))
        With New CTemplate("propertyClass.txt", _templatesPath)
            For Each i As CChild In c.Singles
                .Reset()
                .Replace("dataType", i.TypeName)
                .Replace("nameCamelCase", i.NameCamelCase)
                .Replace("nameProperCase", i.NameProperCase)
                sb.Append(.Template)
            Next
        End With

        If c.Lists.Count > 0 Then sb.Append(Comment("ChildNodes - Collections"))
        With New CTemplate("propertyCollection.txt", _templatesPath)
            For Each i As CChild In c.Lists
                .Reset()
                .Replace("dataType", i.TypeName)
                .Replace("nameCamelCase", i.NameCamelCase)
                .Replace("nameProperCase", i.NameProperCase)
                sb.Append(.Template)
            Next
        End With
        Return sb.ToString
    End Function
    Private Function ImportData(ByVal c As CClass) As String
        Dim sb As New StringBuilder

        If c.AttributesNormal.Count > 0 Then sb.Append(Comment("Load Attributes", 1))
        With New CTemplate("importAttribute.txt", _templatesPath)
            For Each i As CAttribute In c.AttributesNormal
                .Reset()
                .Replace("nameCamelCase", i.NameCamelCase)
                .Replace("defaultValue", i.DefaultValue(CSharp))
                .Replace("name", i.Name)
                .Replace("importFunction", i.ImportFunction)
                sb.Append(.Template)
            Next
        End With

        If c.AttributesAsNodes.Count > 0 Then sb.Append(Comment("Load Attributes (As Text Nodes)", 1))
        With New CTemplate("importAttributeTags.txt", _templatesPath)
            For Each i As CAttribute In c.AttributesAsNodes
                .Reset()
                .Replace("nameCamelCase", i.NameCamelCase)
                .Replace("defaultValue", i.DefaultValue(CSharp))
                .Replace("name", i.Name)
                .Replace("importFunction", i.ImportFunction)
                sb.Append(.Template)
            Next
        End With

        If c.Singles.Count > 0 Then sb.Append(Comment("Load ChildNodes - Single", 1))
        With New CTemplate("importClass.txt", _templatesPath)
            For Each i As CChild In c.Singles
                .Reset()
                .Replace("nameCamelCase", i.NameCamelCase)
                .Replace("className", i.TypeName)
                sb.Append(.Template)
            Next
        End With

        If c.Lists.Count > 0 Then sb.Append(Comment("Load ChildNodes - Collections", 1))
        With New CTemplate("importList.txt", _templatesPath)
            For Each i As CChild In c.Lists
                .Reset()
                .Replace("nameCamelCase", i.NameCamelCase)
                .Replace("className", i.TypeName)
                sb.Append(.Template)
            Next
        End With
        Return sb.ToString
    End Function
    Private Function ExportData(ByVal c As CClass) As String
        Dim sb As New StringBuilder

        If c.AttributesNormal.Count > 0 Then sb.Append(Comment("Attributes", 1))
        With New CTemplate("exportAttribute.txt", _templatesPath)
            For Each i As CAttribute In c.AttributesNormal
                .Reset()
                .Replace("nameCamelCase", i.NameCamelCase)
                .Replace("name", i.Name)
                sb.Append(.Template)
            Next
        End With

        If c.AttributesAsNodes.Count > 0 Then sb.Append(Comment("Attributes (As Text Nodes)", 1))
        With New CTemplate("exportAttributeTags.txt", _templatesPath)
            For Each i As CAttribute In c.AttributesAsNodes
                .Reset()
                .Replace("nameCamelCase", i.NameCamelCase)
                .Replace("name", i.Name)
                sb.Append(.Template)
            Next
        End With

        With New CTemplate("exportClass.txt", _templatesPath)
            If c.Singles.Count > 0 Then sb.Append(Comment("ChildNodes - Single", 1))
            For Each i As CChild In c.Singles
                .Reset()
                .Replace("nameCamelCase", i.NameCamelCase)
                sb.Append(.Template)
            Next

            If c.Lists.Count > 0 Then sb.Append(Comment("ChildNodes - Collections", 1))
            For Each i As CChild In c.Lists
                .Reset()
                .Replace("nameCamelCase", i.NameCamelCase)
                sb.Append(.Template)
            Next
        End With
        Return sb.ToString
    End Function

    Private Function Comment(ByVal msg As String, Optional ByVal extraTabs As Integer = 0) As String
        Dim extra As String = String.Empty
        For i As Integer = 1 To extraTabs
            extra = String.Concat(extra, "    ")
        Next

        If CSharp Then
            Return String.Concat(extra, "        //", msg, vbCrLf)
        Else
            Return String.Concat(extra, "    '", msg, vbCrLf)
        End If
    End Function
#End Region

End Class
