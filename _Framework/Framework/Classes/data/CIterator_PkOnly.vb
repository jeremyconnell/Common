Public MustInherit Class CIterator_PkOnly
    'Members
    Protected m_db As CDataSrc
    Protected m_pkNames As List(Of String)
    Protected m_item As CValues
    Private m_counter As Integer = 0
    Private m_where As CCriteriaList
    Protected m_txForm As DTransform

    Public Delegate Function DTransform(r As CValues) As CValues

    'Constructor
    Protected Sub New(db As CDataSrc, pks As List(Of String), where As CCriteriaList, pack As DTransform)
        m_db = db
        m_pkNames = pks
        m_where = where
        m_txForm = pack
    End Sub

    'Shared
    Public Shared Function Factory(db As CDataSrc, table As String, pks As List(Of String), Optional where As CCriteriaList = Nothing, Optional pageSize As Integer = 10000, Optional pack As DTransform = Nothing) As CIterator_PkOnly
        If db.IsRemote Then
            Return New CIteratorRemote_PkOnly(db.Remote, table, pks, where, pageSize, pack)
        Else
            Return New CIteratorLocal_PkOnly(db.Local, table, pks, where, pack)
        End If
    End Function

    'Properties
    Public ReadOnly Property Tx As DTransform
        Get
            Return m_txForm
        End Get
    End Property
    Public ReadOnly Property ItemTx As CValues
        Get
            If IsNothing(m_item) Then Return m_item
            If IsNothing(Tx) Then Return m_item
            Return m_txForm(m_item)
        End Get
    End Property

    Public ReadOnly Property Item As CValues
        Get
            Return m_item
        End Get
    End Property

    Public ReadOnly Property Where As CCriteriaList
        Get
            Return m_where
        End Get
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
