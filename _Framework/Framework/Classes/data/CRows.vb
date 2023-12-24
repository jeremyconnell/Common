Public Class CRows : Inherits List(Of CRow)

    Public Sub New()
    End Sub
    Public Sub New(count As Integer)
        MyBase.New(count)
    End Sub
    Public Sub New(list As IEnumerable(Of CRow))
        MyBase.New(list)
    End Sub
    Public Sub New(i As CIterator)
        While i.HasItem
            Me.Add(i.Item)
            i.NextRow()
        End While
    End Sub



    Private m_d As Dictionary(Of Guid, CRow)
    Public ReadOnly Property Dict As Dictionary(Of Guid, CRow)
        Get
            If IsNothing(m_d) OrElse m_d.Count <> Me.Count Then
                Dim t As New Dictionary(Of Guid, CRow)(Me.Count)
                For Each i As CRow In Me
                    t(i.PKMD5) = i
                Next
                m_d = t
            End If
            Return m_d
        End Get
    End Property

End Class
