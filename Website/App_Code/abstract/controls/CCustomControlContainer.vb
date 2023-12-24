Imports Microsoft.VisualBasic

Public Enum ELayout
    Flow
    Horizontal
    Vertical
    None
End Enum

Public MustInherit Class CCustomControlContainer : Inherits UserControl

#Region "Interface"
    Public Property Layout() As ELayout
        Get
            If Flow.Visible Then Return ELayout.Flow
            If Horizontal.Visible Then Return ELayout.Horizontal
            If Vertical.Visible Then Return ELayout.Vertical
            Return ELayout.None
        End Get
        Set(ByVal value As ELayout)
            Flow.Visible = (value = ELayout.Flow)
            Horizontal.Visible = (value = ELayout.Horizontal)
            Vertical.Visible = (value = ELayout.Vertical)
        End Set
    End Property
#End Region

#Region "Abstract"
    Protected MustOverride ReadOnly Property Flow() As Control
    Protected MustOverride ReadOnly Property Horizontal() As Control
    Protected MustOverride ReadOnly Property Vertical() As Control
#End Region

#Region "Shared"
    Public Shared Sub SetLayout(ByVal parent As Control, ByVal layout As ELayout)
        If TypeOf parent Is CCustomControl AndAlso CType(parent, CCustomControl).Layout <> ELayout.None Then CType(parent, CCustomControl).Layout = layout
        If TypeOf parent Is CCustomControlContainer AndAlso CType(parent, CCustomControlContainer).Layout <> ELayout.None Then CType(parent, CCustomControlContainer).Layout = layout
        For Each i As Control In parent.Controls
            SetLayout(i, layout)
        Next
    End Sub
#End Region

End Class
