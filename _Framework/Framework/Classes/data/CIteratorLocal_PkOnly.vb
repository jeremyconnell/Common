Public Class CIteratorLocal_PkOnly : Inherits CIterator_PkOnly

    Private m_dr As IDataReader


    Public Sub New(db As CDataSrcLocal, table As String, pks As List(Of String), where As CCriteriaList, pack As DTransform)
        MyBase.New(db, pks, where, pack)

        Dim pk As String = CUtilities.ListToString(pks)
        If IsNothing(where) Then
            m_dr = db.SelectAll_DataReader(pk, table, pk)
        Else
            m_dr = db.SelectWhere_DataReader(pk, table, pk, where)
        End If

        NextRow()
    End Sub


    Public Overrides Sub NextRow()
        MyBase.NextRow()

        If m_dr.Read() Then
            m_item = New CValues(m_dr, m_pkNames)
        Else
            m_item = Nothing
            m_dr.Close()
        End If
    End Sub

End Class
