Public Class CIteratorRemote_PkOnly : Inherits CIterator_PkOnly

    Private m_dt As DataTable
    Private m_pi As CPagingInfo
    Private m_index As Integer


    Public Sub New(db As CDataSrcRemote, table As String, pks As List(Of String), where As CCriteriaList, pageSize As Integer, pack As DTransform)
        MyBase.New(db, pks, where, pack)

        m_pi = New CPagingInfo(pageSize, 0, CUtilities.ListToString(pks), False, table)
        m_index = 0
        Dim pk As String = CUtilities.ListToString(pks)
        If Not IsNothing(where) Then
            m_dt = db.PagingWithFilters_Dataset(m_pi, pk, where).Tables(0)
        Else
            m_dt = db.Paging_Dataset(m_pi, pk).Tables(0)
        End If

        NextRow()
    End Sub

    Public ReadOnly Property Paging As CPagingInfo
        Get
            Return m_pi
        End Get
    End Property

    Public Overrides Sub NextRow()
        MyBase.NextRow()

        If m_index = m_dt.Rows.Count AndAlso m_dt.Rows.Count > 0 Then
            m_pi.PageIndex += 1
            m_index = 0
            Dim pk As String = CUtilities.ListToString(m_pkNames)
            If Not IsNothing(Where) Then
                m_dt = Db.PagingWithFilters_Dataset(m_pi, pk, Where).Tables(0)
            Else
                m_dt = Db.Paging_Dataset(m_pi, pk).Tables(0)
            End If
        End If

        If m_index < m_dt.Rows.Count Then
            m_item = New CValues(m_dt.Rows(m_index), m_pkNames)
            'If Not IsNothing(m_txForm) Then m_item = m_txForm(m_item)
            m_index += 1
            Else
                m_item = Nothing
            m_dt = Nothing
        End If
    End Sub

End Class
