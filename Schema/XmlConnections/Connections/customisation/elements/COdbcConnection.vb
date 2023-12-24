Imports Framework
Imports System.Xml

Partial Public Class COdbcConnection

#Region "Equals"
    Public Overrides Function GetHashCode() As Integer
        Return Me.ConnectionString.GetHashCode()
    End Function
    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If Not TypeOf (obj) Is COleDbConnection Then Return False
        With CType(obj, COleDbConnection)
            Return String.Equals(.ConnectionString, Me.ConnectionString, StringComparison.CurrentCultureIgnoreCase)
        End With
    End Function
#End Region

#Region "Members"
#End Region

#Region "Properties"
#End Region

#Region "Persistance"
    '    Protected Overrides Function ImportSelf(ByVal parent As XmlNode) As XmlNode
    '        Dim node As XmlNode = MyBase.ImportSelf(parent)
    '         m_sample = CXml.AttributeInt(node, "Sample", Integer.MinValue)
    '        Return node
    '    End Function
    '    Protected Overrides Function ExportSelf(ByVal parent As System.Xml.XmlNode) As System.Xml.XmlNode
    '        Dim node As XmlNode = MyBase.ExportSelf(parent)
    '        CXml.AttributeSet(node, "Sample", m_sample)
    '        Return node
    '    End Function
#End Region

End Class
