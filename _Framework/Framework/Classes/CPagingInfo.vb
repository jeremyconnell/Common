<CLSCompliant(True), Serializable()> _
Public Class CPagingInfo

#Region "Constructors"
    Public Sub New()
        Me.New(10)
    End Sub
    Public Sub New(ByVal pageSize As Integer)
        Me.New(pageSize, 0)
    End Sub
    Public Sub New(ByVal pageSize As Integer, ByVal pageIndex As Integer)
        Me.New(pageSize, pageIndex, Nothing)
    End Sub
    Public Sub New(ByVal pageSize As Integer, ByVal pageIndex As Integer, ByVal sortByColumn As String)
        Me.New(pageSize, pageIndex, sortByColumn, False)
    End Sub
    Public Sub New(ByVal pageSize As Integer, ByVal pageIndex As Integer, ByVal sortByColumn As String, ByVal descending As Boolean)
        Me.New(pageSize, pageIndex, sortByColumn, descending, Nothing)
    End Sub
    Public Sub New(ByVal pageSize As Integer, ByVal pageIndex As Integer, ByVal sortByColumn As String, ByVal descending As Boolean, ByVal tableName As String)
        Me.PageSize = pageSize
        Me.PageIndex = pageIndex
        Me.SortByColumn = sortByColumn
        Me.Descending = descending
        Me.TableName = tableName
    End Sub
#End Region

#Region "Data"
    'Required
    Public PageSize As Integer  'Input parameter, Property of pager control (editable from aspx page)
    Public PageIndex As Integer 'Input parameter, Zero-based, state variable from pager control

    'Optional
    Public SortByColumn As String = Nothing 'Takes busines object's default order-by if not supplied
    Public Descending As Boolean = False
    Public TableName As String = Nothing

    'Output
    Public Count As Integer     'Count of pagable records, used by pager to count buttons
#End Region

#Region "Derived"
    Public ReadOnly Property Offset() As Integer
        Get
            Return PageIndex * PageSize
        End Get
    End Property
    Public ReadOnly Property IsDefaultSort() As Boolean
        Get
            Return String.IsNullOrEmpty(Me.SortByColumn)
        End Get
    End Property
#End Region

End Class
