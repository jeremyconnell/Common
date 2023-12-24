Public Class CConfig : Inherits CConfigBase

    Public Shared ReadOnly Property DefaultTemplatesUrl() As String
        Get
            Return Config("DefaultTemplatesUrl")
        End Get
    End Property

End Class
