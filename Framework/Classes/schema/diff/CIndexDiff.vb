Public Class CIndexDiff
    Public Sub New(this As CIndexInfo, ref As CIndexInfo)
        Me.This = this
        Me.Ref = ref
        Me.NameIsDiff = this.IndexName <> ref.IndexName
        Me.ColsAreDiff = this.ColumnNames_ <> ref.ColumnNames_
        Me.UniqueIsDiff = this.IsUnique <> ref.IsUnique
        Me.AnyDiffs = NameIsDiff OrElse UniqueIsDiff OrElse ColsAreDiff
    End Sub

    Public NameIsDiff As Boolean
    Public ColsAreDiff As Boolean
    Public UniqueIsDiff As Boolean
    Public AnyDiffs As Boolean

    Public This As CIndexInfo
    Public Ref As CIndexInfo


    Public ReadOnly Property IsExact As Boolean
        Get
            If NameIsDiff Then Return False
            If ColsAreDiff Then Return False
            If UniqueIsDiff Then Return False
            If AnyDiffs Then Return False
            Return True
        End Get
    End Property

    Public Overrides Function ToString() As String
        If Not AnyDiffs Then Return "None"

        Dim list As New List(Of String)
        If NameIsDiff Then list.Add(String.Concat("Name is different: ", This.IndexName, "<>", Ref.IndexName))
        If UniqueIsDiff Then list.Add(String.Concat("Uniq is different: ", This.IsUnique, "<>", Ref.IsUnique))
        If ColsAreDiff Then list.Add(String.Concat("Cols are different: ", This.ColumnNames_, "<>", Ref.ColumnNames_))
        Return CUtilities.ListToString(list)
    End Function
End Class