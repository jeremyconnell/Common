
Partial Class usercontrols_extensions_form_UCFormLabel
    Inherits CCustomControlContainer

#Region "Text"
    Public Property Text() As String
        Get
            Return ctrlFlow.Text
        End Get
        Set(ByVal value As String)
            ctrlFlow.Text = value
            ctrlHorizontal.Text = value
            ctrlVertical.Text = value
        End Set
    End Property
#End Region

#Region "MustOverride"
    Protected Overrides ReadOnly Property Flow() As Control
        Get
            Return ctrlFlow
        End Get
    End Property
    Protected Overrides ReadOnly Property Horizontal() As Control
        Get
            Return ctrlHorizontal
        End Get
    End Property
    Protected Overrides ReadOnly Property Vertical() As Control
        Get
            Return ctrlVertical
        End Get
    End Property
#End Region

End Class
