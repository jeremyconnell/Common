<CLSCompliant(True)>
Public Class CDiff_Guid
    'Data
    Public SourceOnly As List(Of Guid)
    Public TargetOnly As List(Of Guid)
    Public Matching As List(Of Guid)

    Public Sub New()
        SourceOnly = New List(Of Guid)
        TargetOnly = New List(Of Guid)
        Matching = New List(Of Guid)
    End Sub
End Class
