Public Class CIteratorLocal : Inherits CIterator

    Private m_dr As IDataReader


    Public Sub New(db As CDataSrcLocal, table As String, pks As String)
        Me.New(db, table, CUtilities.StringToListStr(pks))
    End Sub
    Public Sub New(db As CDataSrcLocal, table As String, pks As List(Of String), Optional cols As List(Of String) = Nothing, Optional where As CCriteriaList = Nothing, Optional pack As DTransform = Nothing)
        MyBase.New(db, pks, cols, where, pack)

        If IsNothing(where) Then
            m_dr = db.SelectAll_DataReader(table, CUtilities.ListToString(pks))
        Else
            m_dr = db.SelectWhere_DataReader(table, CUtilities.ListToString(pks), where)
        End If

        If IsNothing(m_colNames) Then
            m_colNames = New List(Of String)(m_dr.FieldCount)
            For i As Integer = 0 To m_dr.FieldCount - 1
                m_colNames.Add(m_dr.GetName(i))
            Next
        End If

        NextRow()
    End Sub


    Public Overrides Sub NextRow()
        MyBase.NextRow()

        If m_dr.Read() Then
            m_item = New CRow(m_dr, m_pkNames, m_colNames)
            If Not IsNothing(m_txForm) Then m_txForm(m_item)
        Else
            m_item = Nothing
            m_dr.Close()
        End If
    End Sub

End Class
