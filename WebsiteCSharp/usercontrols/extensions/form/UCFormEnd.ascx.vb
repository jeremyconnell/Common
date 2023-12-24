
Partial Class usercontrols_extensions_form_UCFormEnd
    Inherits CCustomControlContainer

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
