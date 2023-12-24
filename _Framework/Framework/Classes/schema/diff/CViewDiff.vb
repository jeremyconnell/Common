Public Class CViewDiff
    'Constructor
    Public Sub New(this As CViewInfo, ref As CViewInfo)
        Me.This = this
        Me.Ref = ref

        Me.Columns = New CColumnDiff(this.Columns, ref.Columns)
    End Sub

    'Data
    Public This As CViewInfo
    Public Ref As CViewInfo
    Public Columns As CColumnDiff

    'Derived
    Public ReadOnly Property NameIsDifferent As Boolean
        Get
            Return This.ViewName <> Ref.ViewName
        End Get
    End Property
	Public ReadOnly Property ScriptIsDifferent As Boolean
		Get
			Return This.MD5 <> Ref.MD5
		End Get
	End Property
	Public ReadOnly Property ColumnsAreDifferent As Boolean
		Get
			Return Not Me.Columns.IsExact
		End Get
	End Property
	Public ReadOnly Property IsDifferent As Boolean
        Get
			Return NameIsDifferent OrElse ScriptIsDifferent OrElse ColumnsAreDifferent
		End Get
    End Property

	'Scripting
	Public Function ChangeScripts() As List(Of String)
		If Not IsDifferent Then Return New List(Of String)(0)

		Dim list As New List(Of String)(2)
		list.Add(This.DropScript)
		list.Add(Ref.CreateScript)
		Return list
	End Function

	Public Overrides Function ToString() As String
		Dim list As New List(Of String)
		If NameIsDifferent Then list.Add(String.Concat("Name is different: ", This.ViewName, "<>", Ref.ViewName))
		If ScriptIsDifferent Then list.Add(String.Concat("Script is different"))
		If ColumnsAreDifferent Then list.Add(String.Concat("Cols are different"))
		If list.Count = 0 Then list.Add("None")

		Return This.ViewName & " : " & CUtilities.ListToString(list)
	End Function

End Class
