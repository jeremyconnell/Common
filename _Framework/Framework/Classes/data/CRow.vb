Public Class CRow : Implements IComparable(Of CRow)

    'Data
    Public PK As CValues
    Public Data As CValues
    Public DataId As Long?

    'Constructor
    Public Sub New(r As CRow)
        Me.New(r.PK, r.Data)
    End Sub
    Public Sub New(pk As CValues, data As CValues, Optional dataId As Long? = Nothing)
        Me.PK = pk
        Me.Data = data
        Me.DataId = dataId
    End Sub
    Public Sub New(dr As IDataReader, t As CTableInfo)
        Me.New(dr, t.PrimaryKey.ColumnNames, t.Columns.Names)
    End Sub
    Public Sub New(dr As DataRow, t As CTableInfo)
        Me.New(dr, t.PrimaryKey.ColumnNames, t.Columns.Names)
    End Sub
    Public Sub New(dr As IDataReader, pkN As List(Of String), cols As List(Of String))
        PK = New CValues(pkN.Count)
        Data = New CValues(cols.Count - pkN.Count)
        For Each i As String In cols
            If pkN.Contains(i) Then
                PK.Add(CValue.Factory(i, dr(i)))
            Else
                Data.Add(CValue.Factory(i, dr(i)))
            End If
        Next
    End Sub
    Public Sub New(dr As DataRow, pkN As List(Of String), cols As List(Of String))
        PK = New CValues(pkN.Count)
        Data = New CValues(cols.Count - pkN.Count)
        For Each i As String In cols
            If pkN.Contains(i) Then
                PK.Add(CValue.Factory(i, dr(i)))
            Else
                Data.Add(CValue.Factory(i, dr(i)))
            End If
        Next
    End Sub
    Shared Sub New()
        CProto.Prepare(Of CRow)()
    End Sub

    'Compare on pk
    Public Function CompareTo(other As CRow) As Integer Implements IComparable(Of CRow).CompareTo
        PK.CompareTo(other.PK)
    End Function


    'Compare on data
    Public ReadOnly Property PkMD5 As Guid
        Get
            Return PK.MD5
        End Get
    End Property
    Public ReadOnly Property DataMD5 As Guid
        Get
            Return Data.MD5
        End Get
    End Property



    Public Function Diff(old As CRow) As CValues
        Return Data.Diff(old.Data)
    End Function
    Public Function Diff_(old As CRow) As CRow
        Return New CRow(Me.PK, Diff(old))
    End Function
    Public Changes As CRow

    Public Function Join() As CValues
        Dim j As New CValues(PK.Count + Data.Count)
        j.AddRange(PK)
        j.AddRange(Data)
        Return j
    End Function

End Class
