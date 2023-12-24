Public Class CDiff_Int
    'Data
    Public SourceOnly As List(Of Integer)
    Public TargetOnly As List(Of Integer)
    Public Matching As List(Of Integer)

    'Constructor
    Public Sub New()
        SourceOnly = New List(Of Integer)
        TargetOnly = New List(Of Integer)
        Matching = New List(Of Integer)
    End Sub
End Class
