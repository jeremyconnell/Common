

Public Class CGenericActions
    Protected m_table As String
    Protected m_src As CDataSrc
    Protected m_tar As CDataSrc
    Protected m_cols As List(Of String)
    Protected m_isId As Boolean

    'Bulk
    Private m_batchDelete As Integer
    Private m_batchCopy As Integer
    Private m_batchInsert As Integer
    Private m_insert As List(Of CValues)
    Private m_copy As List(Of CValues)
    Private m_del As List(Of CValues)

    Public Sub New(tableName As String, src As CDataSrc, tar As CDataSrc, cols As List(Of String), isId As Boolean, Optional bulkDelete As Integer = 1000, Optional bulkInsert As Integer = 100, Optional bulkCopy As Integer = 100)
        m_table = tableName
        m_src = src
        m_tar = tar
        m_cols = cols
        m_isId = isId

        m_batchCopy = bulkCopy
        m_batchDelete = bulkDelete
        m_batchInsert = bulkInsert
    End Sub

    Public Overridable Sub Insert(row As CRow)
        Insert(row.Join)
    End Sub
    Public Overridable Sub Insert(data As CValues)
        m_tar.InsertId(m_table, data.ToNameValues, m_isId)
    End Sub
    Public Overridable Sub Insert(page As List(Of CNameValueList))
        m_tar.InsertId(m_table, page, m_isId)
    End Sub

    Public Overridable Sub Update(src As CRow, tar As CRow)
        Dim diff As CRow = src.Diff_(tar)
        Update(diff, src.PK)
    End Sub
    Public Overridable Sub Update(diff As CRow, pk As CValues)
        m_tar.Update(diff.Data.ToNameValues, New CWhere(m_table, pk.ToCriteria, Nothing))
    End Sub

    Public Overridable Sub Delete(tar As CRow)
        Delete(tar.PK)
    End Sub
    Public Overridable Sub Delete(pk As CValues)
        m_tar.Delete(New CWhere(m_table, pk.ToCriteria, Nothing))
    End Sub

    Public Overridable Sub Copy(pk As CValues, Optional tx As CIterator_PkOnly.DTransform = Nothing)
        Dim dt As DataTable = m_src.SelectWhere_Dataset(m_table, pk.ToCriteria).Tables(0)
        If dt.Rows.Count = 0 Then Exit Sub

        If IsNothing(m_cols) Then
            Dim temp As New List(Of String)(dt.Columns.Count)
            For Each i As DataColumn In dt.Columns
                temp.Add(i.ColumnName)
            Next
            m_cols = temp
        End If

        'transform
        If Not IsNothing(tx) Then pk = tx(pk)
        Dim r As CRow = New CRow(dt.Rows(0), pk.ToNames, m_cols)

        Insert(r)
    End Sub



    Public Overridable Sub CopyBulk(pk As CValues, Optional tx As CIterator_PkOnly.DTransform = Nothing)
        If IsNothing(m_copy) Then m_copy = New List(Of CValues)(m_batchCopy)
        If Not IsNothing(tx) Then tx(pk)
        m_copy.Add(pk)

        'Insert
        If m_copy.Count >= m_batchCopy Then FlushCopy()
    End Sub
    Public Overridable Sub InsertBulk(data As CValues, Optional tx As CIterator_PkOnly.DTransform = Nothing)
        If IsNothing(m_insert) Then m_insert = New List(Of CValues)(m_batchInsert)
        If Not IsNothing(tx) Then tx(data)
        m_insert.Add(data)

        'Insert
        If m_insert.Count >= m_batchInsert Then FlushInsert()
    End Sub

    Public Overridable Sub DeleteBulk(pk As CValues, Optional tx As CIterator_PkOnly.DTransform = Nothing)
        If IsNothing(m_del) Then m_del = New List(Of CValues)(m_batchCopy)
        If Not IsNothing(pk) Then tx(pk)
        m_del.Add(pk)

        'Insert
        If m_del.Count >= m_batchDelete Then FlushDelete()
    End Sub

    Public Overridable Sub Flush()
        FlushCopy()
        FlushDelete()
        FlushInsert()
    End Sub
    Public Overridable Sub FlushCopy()
        If IsNothing(m_copy) OrElse m_copy.Count = 0 Then Exit Sub

        'Select IN (pks)
        Dim c As CCriteriaList = WhereIn(m_copy)
        Dim dt As DataTable = m_src.SelectWhere_Dataset(m_table, c).Tables(0)
        If dt.Rows.Count = 0 Then Exit Sub

        If IsNothing(m_cols) Then
            Dim temp As New List(Of String)(dt.Columns.Count)
            For Each i As DataColumn In dt.Columns
                temp.Add(i.ColumnName)
            Next
            m_cols = temp
        End If

        'transform
        Dim pkN As New List(Of String)(1)
        pkN.Add(dt.Columns(0).ColumnName)
        Dim list As New List(Of CValues)
        For Each i As DataRow In dt.Rows
            list.Add(New CRow(i, pkN, m_cols).Join)
        Next

        'reset
        m_copy.Clear()

        'Parse, Insert
        m_tar.InsertId(m_table, Convert(list), m_isId)

    End Sub
    Public Overridable Sub FlushInsert()
        If IsNothing(m_insert) OrElse m_insert.Count = 0 Then Exit Sub

        Dim list As List(Of CNameValueList) = Convert(m_insert)

        'reset
        m_insert.Clear()

        'Parse, Insert
        m_tar.InsertId(m_table, list, m_isId)

    End Sub
    Public Overridable Sub FlushDelete()
        If IsNothing(m_del) OrElse m_del.Count = 0 Then Exit Sub

        'Select IN (pks)
        Dim c As CCriteriaList = WhereIn(m_del)

        'reset
        m_del.Clear()

        'Parse, Insert
        m_tar.DeleteWhere(m_table, c)
    End Sub

    'Private
    Private Shared Function Convert(list As List(Of CValues)) As List(Of CNameValueList)
        Dim nv As New List(Of CNameValueList)
        For Each i As CValues In list
            nv.Add(i.ToNameValues)
        Next
        Return nv
    End Function
    Private Shared Function WhereIn(data As List(Of CValues)) As CCriteriaList
        'Select IN (pks)
        Dim pkName As String = data(0)(0).Name
        Dim list As New List(Of Object)(data.Count)
        For Each i As CValues In data
            Dim pk As CValue = i(0)
            list.Add(pk.Value)
        Next
        Return New CCriteriaList(pkName, ESign.IN, list)
    End Function
End Class