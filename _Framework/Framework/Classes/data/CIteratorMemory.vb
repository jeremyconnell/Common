Public Class CIteratorMemory : Inherits CIterator


    Private m_index As Integer
    Private m_rows As List(Of CRow)


    Public Sub New(i As CIterator, repack As CIterator.DTransform)
        MyBase.New(i.Db, i.PkNames, i.ColNames, i.Where, repack)

        m_rows = New List(Of CRow)()
        While i.HasItem
            m_rows.Add(i.Item)
            i.NextRow()
        End While
        m_rows.Sort()
        m_index = 0
        If m_rows.Count > m_index Then m_item = m_rows(0)
    End Sub



    Public Overrides Sub NextRow()
        If m_index = Integer.MaxValue Then Exit Sub
        m_item = m_rows(m_index)
        m_index += 1
        If m_index = m_rows.Count Then
            m_item = Nothing
            m_index = Integer.MaxValue
        End If
    End Sub


    Private m_d As Dictionary(Of Guid, CRow)
    Private m_dTx As Dictionary(Of Guid, CRow)
    Public ReadOnly Property Dict As Dictionary(Of Guid, CRow)
        Get
            If IsNothing(m_d) OrElse m_d.Count <> m_rows.Count Then
                Dim t As New Dictionary(Of Guid, CRow)
                For Each i As CRow In m_rows
                    t(i.PK.MD5) = i
                Next
                m_d = t
            End If
            Return m_d
        End Get
    End Property

End Class
