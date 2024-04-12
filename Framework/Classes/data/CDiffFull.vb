Public Class CDiffFull
    'Data
    Public SourceOnly As List(Of CRow)
    Public TargetOnly As List(Of CRow)
    Public Matching As List(Of CRow)
    Public Different As List(Of CRow)

    Public Sub New()
        SourceOnly = New List(Of CRow)
        TargetOnly = New List(Of CRow)
        Matching = New List(Of CRow)
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
