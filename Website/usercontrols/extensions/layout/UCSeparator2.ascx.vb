
Partial Class usercontrols_extensions_common_UCSeparator2
    Inherits CCustomControlContainer

#Region "MustOverride"
    Protected Overrides ReadOnly Property Flow() As Control
        Get
            Return ctrlCss
        End Get
    End Property
    Protected Overrides ReadOnly Property Horizontal() As Control
        Get
            Return ctrlCols
        End Get
    End Property
    Protected Overrides ReadOnly Property Vertical() As Control
        Get
            Return ctrlRows
        End Get
    End Property
#End Region

End Class
