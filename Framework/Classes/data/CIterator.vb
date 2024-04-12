Public MustInherit Class CIterator
    'Members
    Protected m_db As CDataSrc
    Protected m_pkNames As List(Of String)
    Protected m_colNames As List(Of String)
    Protected m_item As CRow
    Protected m_itemTx As CRow
    Protected m_txForm As DTransform
    Private m_counter As Integer = 0
    Private m_where As CCriteriaList

    Public Delegate Sub DTransform(r As CRow)

    'Constructor
    Protected Sub New(db As CDataSrc, pks As List(Of String), cols As List(Of String), where As CCriteriaList, tx As DTransform)
        m_db = db
        m_pkNames = pks
        m_colNames = cols
        m_where = where
        m_txForm = tx
    End Sub

    'Shared
    Public Shared Function Factory(db As CDataSrc, table As String, pks As List(Of String), Optional cols As List(Of String) = Nothing, Optional where As CCriteriaList = Nothing, Optional pageSize As Integer = 1000, Optional tx As DTransform = Nothing) As CIterator
        If db.IsRemote Then
            Return New CIteratorRemote(db, table, pks, cols, where, pageSize, tx)
        Else
            Return New CIteratorLocal(db.Local, table, pks, cols, where, tx)
        End If
    End Function

    'Properties
    Public ReadOnly Property Tx As DTransform
        Get
            Return m_txForm
        End Get
    End Property

    Public ReadOnly Property Item As CRow
        Get
            Return m_item
        End Get
    End Property

    Public Property Where As CCriteriaList
        Get
            Return m_where
        End Get
        Set
            m_where = Value
        End Set
    End Property
    Public ReadOnly Property Db As CDataSrc
        Get
            Return m_db
        End Get
    End Property
    Public ReadOnly Property PkNames As List(Of String)
        Get
            Return m_pkNames
        End Get
    End Property
    Public ReadOnly Property ColNames As List(Of String)
        Get
            Return m_colNames
        End Get
    End Property
    Public ReadOnly Property Counter As Integer
        Get
            Return m_counter
        End Get
    End Property

    'Derived
    Public ReadOnly Property HasItem As Boolean
        Get
            Return Not IsNothing(m_item)
        End Get
    End Property

    'Methoesw
    Public Overridable Sub NextRow()
        m_counter += 1
    End Sub


End Class
