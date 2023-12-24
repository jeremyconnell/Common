Imports System.Xml

Public Class CAttribute

#Region "Constructors"
    Public Sub New(ByVal node As XmlNode)
        Select Case node.LocalName
            Case "attribute"
                Me.Name = CXml.AttributeStr(node, "name")
                Me.Type = CXml.AttributeStr(node, "type")
                Me.Default = CXml.AttributeStr(node, "default", Nothing)
                Me.UseTags = False
            Case "element"
                Me.Name = CXml.AttributeStr(node, "name")
                Me.Type = CXml.AttributeStr(node, "type", "xs:string")
                Me.Default = CXml.AttributeStr(node, "default", Nothing)
                Me.UseTags = True
            Case Else
                Throw New Exception("Expected attribute or element")
        End Select
    End Sub
#End Region

#Region "Members"
    Public Name As String
    Public Type As String
    Public UseTags As Boolean
    Public [Default] As String
#End Region

#Region "Properties"
    Public ReadOnly Property SafeName() As String
        Get
            Return Name.Replace("-", "_")
        End Get
    End Property
    Public ReadOnly Property NameProperCase() As String
        Get
            If String.IsNullOrEmpty(SafeName) Then Return String.Empty
            Return SafeName.Substring(0, 1).ToUpper() & SafeName.Substring(1)
        End Get
    End Property
    Public ReadOnly Property NameCamelCase() As String
        Get
            If String.IsNullOrEmpty(SafeName) Then Return String.Empty
            Return SafeName.Substring(0, 1).ToLower() & SafeName.Substring(1)
        End Get
    End Property
    Public ReadOnly Property DataType(ByVal cSharp As Boolean) As String
        Get
            Select Case Type
                Case "xs:string", String.Empty
                    Return CStr(IIf(cSharp, "string", "String"))

                Case "xs:int", "xs:integer", "xs:unsignedByte", "xs:unsignedShort", "xs:byte"
                    Return CStr(IIf(cSharp, "int", "Integer"))

                Case "xs:dateTime", "xs:date"
                    Return "DateTime"

                Case "xs:boolean", "xs:bool"
                    Return CStr(IIf(cSharp, "bool", "Boolean"))

                Case "xs:decimal"
                    Return CStr(IIf(cSharp, "decimal", "Decimal"))

                Case "xs:long"
                    Return CStr(IIf(cSharp, "long", "Long"))

                Case "xs:double", "xs:float"
                    Return CStr(IIf(cSharp, "double", "Double"))

                Case Else
                    If Type.StartsWith("xs:") Then Throw New Exception("CAttribute.DataType - No provision for xml datatype: " & Type)
                    Return CTableInformation.CLASS_PREFIX & NameProperCase
            End Select
        End Get
    End Property
    Public ReadOnly Property DefaultValue(ByVal csharp As Boolean) As String
        Get
            Select Case DataType(True)
                Case "string"
                    If Not IsNothing(Me.Default) Then Return String.Concat("""", Me.Default, """")
                    Return String.Concat(IIf(csharp, "string", "String"), ".Empty")
                Case "int"
                    Return String.Concat(IIf(csharp, "int", "Integer"), ".MinValue")
                Case "DateTime"
                    Return "DateTime.MinValue"
                Case "bool"
                    Return CStr(IIf(csharp, "false", "False"))
                Case "decimal"
                    Return String.Concat(IIf(csharp, "decimal", "Decimal"), ".MinValue")
                Case "long"
                    Return String.Concat(IIf(csharp, "long", "Long"), ".MinValue")
                Case "double"
                    Return String.Concat(IIf(csharp, "double", "Double"), ".NaN")
                Case "single"
                    Return String.Concat(IIf(csharp, "single", "Single"), ".NaN")
                Case Else
                    Throw New Exception("CAttribute.DefaultValue - No provision for datatype: " & DataType(True) & "/" & DataType(False))
            End Select
        End Get
    End Property
    Public ReadOnly Property ImportFunction() As String
        Get
            If Me.UseTags Then
                Select Case DataType(True)
                    Case "string"
                        Return "ChildNodeGetStr"
                    Case "int"
                        Return "ChildNodeGetInt"
                    Case "DateTime"
                        Return "ChildNodeGetDate"
                    Case "bool"
                        Return "ChildNodeGetBool"
                    Case "decimal"
                        Return "ChildNodeGetDec"
                    Case "long"
                        Return "ChildNodeGetLong"
                    Case "double"
                        Return "ChildNodeGetDbl"
                    Case "single"
                        Return "ChildNodeGetSingle"
                    Case Else
                        Throw New Exception("CAttribute.ImportFunction - No provision for datatype: " & DataType(True) & "/" & DataType(False))
                End Select
            Else
                Select Case DataType(True)
                    Case "string"
                        Return "AttributeStr"
                    Case "int"
                        Return "AttributeInt"
                    Case "DateTime"
                        Return "AttributeDate"
                    Case "bool"
                        Return "AttributeBool"
                    Case "decimal"
                        Return "AttributeDec"
                    Case "long"
                        Return "AttributeLong"
                    Case "double"
                        Return "AttributeDbl"
                    Case "single"
                        Return "AttributeSingle"
                    Case Else
                        Throw New Exception("CAttribute.ImportFunction - No provision for datatype: " & DataType(True) & "/" & DataType(False))
                End Select
            End If
        End Get
    End Property

#End Region

End Class
