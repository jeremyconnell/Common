Public Class CDiff_Dec
    'Data
    Public SourceOnly As List(Of Decimal)
    Public TargetOnly As List(Of Decimal)
    Public Matching As List(Of Decimal)
    Public Different As List(Of Decimal)

    'Constructor
    Public Sub New()
        SourceOnly = New List(Of Decimal)
        TargetOnly = New List(Of Decimal)
        Matching = New List(Of Decimal)
        Different = New List(Of Decimal)
    End Sub
End Class
