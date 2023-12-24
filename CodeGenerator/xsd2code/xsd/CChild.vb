Public Class CChild

#Region "Constructors"
    Public Sub New(ByVal many As Boolean, ByVal type As CClass)
        Me.Many = many
        Me.Type = type
    End Sub
#End Region

#Region "Properties"
    Public Many As Boolean
    Public Type As CClass
#End Region

#Region "Properties"
    Public ReadOnly Property TypeName() As String
        Get
            Return CStr(IIf(Many, Type.ClassNameList, Type.ClassName))
        End Get
    End Property
    Public ReadOnly Property NameCamelCase() As String
        Get
            'TagName
            Dim name As String = Type.TagName.Replace("-", "_")
            'CamelCase
            name = name.Substring(0, 1).ToLower & name.Substring(1)
            'plural
            If Many Then
                name &= "s"
                If name.Substring(name.Length - 2, 2) = "ys" Then
                    name = name.Substring(name.Length - 2) & "ies"
                End If
            End If
            Return name
        End Get
    End Property
    Public ReadOnly Property NameProperCase() As String
        Get
            Dim s As String = NameCamelCase
            Return s.Substring(0, 1).ToUpper & s.Substring(1)
        End Get
    End Property
#End Region

End Class
