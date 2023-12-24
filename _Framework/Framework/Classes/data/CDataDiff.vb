
Public MustInherit Class CDataDiff
    'constructor
    Public Sub New(source As CDataSrc, target As CDataSrc, tableName As String, pk As List(Of String))
        If Not tableName.Contains("[") Then tableName = String.Concat("[", tableName.Replace(".", "].["), "]")

        Me.SourceDb = source
        Me.TargetDb = target
        Me.TableName = tableName
        Me.PrimaryKey = pk
    End Sub

    'Inputs
    Public SourceDb As CDataSrc
    Public TargetDb As CDataSrc
    Public TableName As String
    Public PrimaryKey As List(Of String)
End Class

Public Class CDataDiff_PksOnly : Inherits CDataDiff
    Public Sub New(source As CDataSrc, target As CDataSrc, tableName As String, pks As List(Of String))
        MyBase.New(source, target, tableName, pks)

        'Output
        DiffOnPk = New CDiff()

        'Iterators
        Dim src As CIterator_PkOnly = CIterator_PkOnly.Factory(source, tableName, pks)
        Dim tar As CIterator_PkOnly = CIterator_PkOnly.Factory(target, tableName, pks)

        'Logic
        CDiffLogic.RowByRow_InsertAndDelete_PksOnly(src, tar, AddressOf Copy, AddressOf Delete, AddressOf Matching)
    End Sub

    'outputs
    Public DiffOnPk As CDiff

    Private Sub Copy(pk As CValues, tx As CIterator_PkOnly.DTransform)
        If Not IsNothing(tx) Then pk = tx(pk)
        DiffOnPk.SourceOnly.Add(pk)
    End Sub
    Private Sub Delete(pk As CValues)
        DiffOnPk.TargetOnly.Add(pk)
    End Sub
    Private Sub Matching(row As CValues)
        DiffOnPk.Matching.Add(row)
    End Sub
End Class

Public Class CDataDiff_FullScan : Inherits CDataDiff
    Public Sub New(source As CDataSrc, target As CDataSrc, tableName As String, pks As List(Of String), Optional cols As List(Of String) = Nothing)
        MyBase.New(source, target, tableName, pks)

        'Output
        Diff = New CDiffFull()

        'Iterators
        Dim src As CIterator = CIterator.Factory(source, tableName, pks)
        Dim tar As CIterator = CIterator.Factory(target, tableName, pks)

        'Logic
        CDiffLogic.RowByRow_Full(src, tar, AddressOf Insert, AddressOf Update, AddressOf Delete, AddressOf Matching)
    End Sub

    'outputs
    Public Diff As CDiffFull

    Private Sub Insert(row As CRow)
        Diff.SourceOnly.Add(row)
    End Sub
    Private Sub Update(src As CRow, tar As CRow)
        Dim c As CRow = src.Diff_(tar)
        tar.Changes = c
        Diff.Different.Add(tar)
    End Sub
    Private Sub Delete(row As CRow)
        Diff.TargetOnly.Add(row)
    End Sub
    Private Sub Matching(row As CRow)
        Diff.Matching.Add(row)
    End Sub

    Public Function Diff_() As CDiff
        Dim dd As New CDiff
        dd.Different = Me.Diff.Different
        dd.Matching = Convert(Me.Diff.Matching)
        dd.SourceOnly = Convert(Me.Diff.SourceOnly)
        dd.TargetOnly = Convert(Me.Diff.TargetOnly)
        Return dd
    End Function
    Private Function Convert(rows As List(Of CRow)) As List(Of CValues)
        Dim t As New List(Of CValues)(rows.Count)
        For Each i As CRow In rows
            t.Add(i.PK)
        Next
        Return t
    End Function
End Class
