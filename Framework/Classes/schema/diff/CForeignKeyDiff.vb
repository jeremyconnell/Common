Public Class CForeignKeyDiff

    Friend Sub New()
    End Sub
    Public Sub New(this As CForeignKey, ref As CForeignKey)
        Me.This = this
        Me.Ref = ref
    End Sub

    Public This As CForeignKey
    Public Ref As CForeignKey

    Public ReadOnly Property IsDifferent As Boolean
        Get
            Return This.MD5 <> Ref.MD5
        End Get
    End Property

    Public Overrides Function ToString() As String
        If Not IsDifferent Then Return "None"

        Dim list As New List(Of String)
        If This.KeyName <> Ref.KeyName Then list.Add(String.Concat("KeyName is different: ", This.KeyName, "<>", Ref.KeyName))
        If This.ReferenceTable <> Ref.ReferenceTable Then list.Add(String.Concat("ReferenceTable is different: ", This.ReferenceTable, "<>", Ref.ReferenceTable))
        If This.ColumnNames_ <> Ref.ColumnNames_ Then list.Add(String.Concat("ColumnName(s) different: ", This.ColumnNames_, "<>", Ref.ColumnNames_))
		If This.RefColumnNames_ <> Ref.RefColumnNames_ Then list.Add(String.Concat("RefColumnName(s) different: ", This.RefColumnNames_, "<>", Ref.RefColumnNames_))
		If This.CascadeUpdate <> Ref.CascadeUpdate Then list.Add(String.Concat("CascadeUpdate different: ", This.CascadeUpdate, "<>", Ref.CascadeUpdate))
		If This.CascadeDelete <> Ref.CascadeDelete Then list.Add(String.Concat("CascadeDelete different: ", This.CascadeDelete, "<>", Ref.CascadeDelete))
		Return CUtilities.ListToString(list)
    End Function

End Class
