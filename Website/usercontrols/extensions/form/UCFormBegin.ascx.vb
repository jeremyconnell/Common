
Partial Class usercontrols_extensions_form_UCFormBegin
    Inherits CCustomControlContainer

#Region "MustOverride"
    Protected Overrides ReadOnly Property Flow() As System.Web.UI.Control
        Get
            Return ctrlFlow
        End Get
    End Property
    Protected Overrides ReadOnly Property Horizontal() As System.Web.UI.Control
        Get
            Return ctrlHorizontal
        End Get
    End Property
    Protected Overrides ReadOnly Property Vertical() As System.Web.UI.Control
        Get
            Return ctrlVertical
        End Get
    End Property
#End Region

End Class
