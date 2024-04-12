Public Class CDiff
    'Data
    Public SourceOnly As List(Of CValues)
    Public TargetOnly As List(Of CValues)
    Public Matching As List(Of CValues)
    Public Different As List(Of CRow)

    Public Sub New()
        SourceOnly = New List(Of CValues)
        TargetOnly = New List(Of CValues)
        Matching = New List(Of CValues)
        Different = New List(Of CRow)
    End Sub

    Public ReadOnly Property Source As Integer
        Get
            Return SourceOnly.Count + Matching.Count + Different.Count
        End Get
    End Property
    Public ReadOnly Property Target As Integer
        Get
            Return TargetOnly.Count + Matching.Count + Different.Count
        End Get
    End Property
End Class
