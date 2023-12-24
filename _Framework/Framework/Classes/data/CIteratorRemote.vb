Public Class CIteratorRemote : Inherits CIterator

    Private m_dt As DataTable
    Private m_pi As CPagingInfo
    Private m_index As Integer
    Private m_counter As Integer


    Public Sub New(db As CDataSrc, table As String, pks As String)
        Me.New(db, table, CUtilities.StringToListStr(pks))
    End Sub
    Public Sub New(db As CDataSrc, table As String, pks As List(Of String), Optional cols As List(Of String) = Nothing, Optional where As CCriteriaList = Nothing, Optional pageSize As Integer = 1000, Optional tx As DTransform = Nothing)
        MyBase.New(db, pks, cols, where, tx)

        m_pi = New CPagingInfo(pageSize, 0, CUtilities.ListToString(pks), False, table)
        m_index = 0

        NextPage()

        If IsNothing(m_colNames) Then
            m_colNames = New List(Of String)(m_dt.Columns.Count)
            For Each i As DataColumn In m_dt.Columns
                m_colNames.Add(i.ColumnName)
            Next
        End If

        NextRow()
    End Sub

    Private Sub NextPage()
        '10k,1k,100
        Try
            If Not IsNothing(Where) Then
                m_dt = Db.PagingWithFilters_Dataset(m_pi, Where).Tables(0)
            Else
                m_dt = Db.Paging_Dataset(m_pi).Tables(0)
            End If
        Catch
            m_pi.PageSize = CInt(Math.Ceiling(m_pi.PageSize / 10))
            m_counter = 0


            Try
                If Not IsNothing(Where) Then
                    m_dt = Db.PagingWithFilters_Dataset(m_pi, Where).Tables(0)
                Else
                    m_dt = Db.Paging_Dataset(m_pi).Tables(0)
                End If
            Catch

                m_pi.PageSize = CInt(Math.Ceiling(m_pi.PageSize / 10))
                m_counter = 0

                If Not IsNothing(Where) Then
                    m_dt = Db.PagingWithFilters_Dataset(m_pi, Where).Tables(0)
                Else
                    m_dt = Db.Paging_Dataset(m_pi).Tables(0)
                End If
            End Try
        End Try
        Db.ShiftDates(m_dt)
        m_counter += 1

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
            NextPage()
        End If

        If m_index < m_dt.Rows.Count Then
            m_item = New CRow(m_dt.Rows(m_index), m_pkNames, m_colNames)
            If Not IsNothing(m_txForm) Then m_txForm(m_item)
            m_index += 1
        Else
            m_item = Nothing
            m_dt = Nothing
        End If
    End Sub

End Class
