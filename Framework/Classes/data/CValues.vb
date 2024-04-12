Imports Framework

Public Class CValues : Inherits List(Of CValue) : Implements IComparable(Of CValues)

    'Constructor
    Public Sub New()
    End Sub
    Public Sub New(count As Integer)
        MyBase.New(count)
    End Sub
    Public Sub New(list As IList(Of CValue))
        MyBase.New(list)
    End Sub

    Public Sub New(dr As IDataReader, pkN As List(Of String))
        Me.New(pkN.Count)
        For Each i As String In pkN
            Me.Add(CValue.Factory(i, dr(i)))
        Next
    End Sub
    Public Sub New(dr As DataRow, pkN As List(Of String))
        Me.New(pkN.Count)
        For Each i As String In pkN
            Me.Add(CValue.Factory(i, dr(i)))
        Next
    End Sub
    Shared Sub New()
        CProto.Prepare(Of CValues)()
    End Sub

    'Sorting
    Public Function CompareTo(other As CValues) As Integer Implements IComparable(Of CValues).CompareTo
        Dim c As Integer = 0
        For i As Integer = 0 To Me.Count - 1
            c = Me(i).CompareTo(other(i))
            If c <> 0 Then Return c
        Next
        Return 0
    End Function
    Public Overrides Function Equals(obj As Object) As Boolean
        Return 0 = Me.CompareTo(CType(obj, CValues))
    End Function
    Public Overrides Function GetHashCode() As Integer
        Return Me.MD5.GetHashCode()
    End Function

    'Convert
    Public Function ToCriteria(cols As List(Of String)) As CCriteriaList
        UnpackNames(cols)
        Return ToCriteria()
    End Function
    Public Function ToCriteria() As CCriteriaList
        Dim c As New CCriteriaList(Me.Count)
        For Each i As CValue In Me
            c.Add(i.Name, i.Value)
        Next
        Return c
    End Function
    Public Function ToNameValues() As CNameValueList
        Dim c As New CNameValueList(Me.Count)
        For Each i As CValue In Me
            c.Add(i.Name, i.Value)
        Next
        Return c
    End Function
    Public Function ToNames() As List(Of String)
        Dim c As New List(Of String)(Me.Count)
        For Each i As CValue In Me
            c.Add(i.Name)
        Next
        Return c
    End Function


    'Serialise
    Private m_bin As Byte()
    Public ReadOnly Property Bin As Byte()
        Get
            If IsNothing(m_bin) Then
                m_bin = CProto.Serialise(Me)
            End If
            Return m_bin
        End Get
    End Property

    'Hash
    Private m_md5 As Guid = Guid.Empty
    Public ReadOnly Property MD5 As Guid
        Get
            If Guid.Empty.Equals(m_md5) Then
                m_md5 = CBinary.MD5_(Bin)
            End If
            Return m_md5
        End Get
    End Property

    'Index on name
    Default Public Overloads ReadOnly Property Item(name As String) As CValue
        Get
            name = name.ToLower
            Dim v As CValue = Nothing
            If Dict.TryGetValue(name, v) Then Return v
            Return Nothing
        End Get
    End Property
    Default Public Overloads ReadOnly Property Item(names As List(Of String)) As CValues
        Get
            Dim list As New CValues
            For Each i As String In names
                list.Add(Me(i))
            Next
            Return list
        End Get
    End Property
    Private m_dict As Dictionary(Of String, CValue)
    Private ReadOnly Property Dict As Dictionary(Of String, CValue)
        Get
            If IsNothing(m_dict) OrElse m_dict.Count = 1 Then
                Dim d As New Dictionary(Of String, CValue)
                For Each i As CValue In Me
                    If Not IsNothing(i.Name) Then d.Add(i.Name.ToLower, i)
                Next
                m_dict = d
            End If
            Return m_dict
        End Get
    End Property
    Public Function GetById(id As Integer) As CValue
        Dim v As CValue = Nothing
        DictById.TryGetValue(id, v)
        Return v
    End Function
    Private m_dictById As Dictionary(Of Integer, CValue)
    Private ReadOnly Property DictById As Dictionary(Of Integer, CValue)
        Get
            If IsNothing(m_dictById) Then
                Dim d As New Dictionary(Of Integer, CValue)
                For Each i As CValue In Me
                    If i.Id.HasValue Then d.Add(i.Id.Value, i)
                Next
                m_dictById = d
            End If
            Return m_dictById
        End Get
    End Property


    ''Packing
    Public Sub PackNames()
        For Each i As CValue In Me
            i.Name = Nothing
        Next
    End Sub
    Public Sub UnpackNames(cols As List(Of String))
        Dim j As Integer = 0
        For Each i As CValue In Me
            i.Name = cols(j)
            j += 1
        Next
    End Sub
    'Public Sub NamesAsIds(d As Dictionary(Of String, Integer))
    '    For Each i As String In d.Keys
    '        Dim v As CValue = Me(i)
    '        If Not IsNothing(v) Then
    '            v.Name = Nothing
    '            v.Id = d(i)
    '        End If
    '    Next
    'End Sub


    'Diff
    Public Function Diff(old As CValues) As CValues
        Dim list As New CValues(Me.Count)
        For Each i As CValue In Me
            Dim j As CValue = old(i.Name)
            If IsNothing(j) OrElse i.CompareTo(j) <> 0 Then
                list.Add(i)
            End If
        Next
        Return list
    End Function

    Public Sub Rehash()
        m_bin = Nothing
        m_md5 = Guid.Empty
    End Sub
End Class
