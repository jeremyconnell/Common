Public Class CDiff_Long
	'Data
	Public SourceOnly As List(Of Long)
	Public TargetOnly As List(Of Long)
	Public Matching As List(Of Long)

	'Constructor
	Public Sub New()
		SourceOnly = New List(Of Long)
		TargetOnly = New List(Of Long)
		Matching = New List(Of Long)
	End Sub
End Class
