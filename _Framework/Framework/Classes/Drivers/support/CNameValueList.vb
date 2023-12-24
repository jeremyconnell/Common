Imports System.Runtime.Serialization

<Serializable(), DataContract(), CLSCompliant(True)>
Public Class CNameValueList : Inherits List(Of CNameValue)

#Region "Constructors"
    'Protobuf: cannot serialise object
    'Shared Sub New()
    '    CProto.Prepare(Of CNameValueList)()
    'End Sub

    'Standard
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal capacity As Integer)
        MyBase.New(capacity)
    End Sub
    Public Sub New(ByVal collection As IEnumerable(Of CNameValue))
        MyBase.New(collection)
    End Sub

    'Alternative
    Public Sub New(ByVal dictionary As Dictionary(Of String, Object))
        MyBase.New()
        If IsNothing(dictionary) Then Exit Sub
        For Each i As String In dictionary.Keys
            Me.Add(i, dictionary(i))
        Next
    End Sub

    Public Sub New(ByVal pName As String, ByVal pValue As Object)
        MyBase.New(1)
        Add(New CNameValue(pName, pValue))
    End Sub
    Public Sub New(ByVal pName1 As String, ByVal pValue1 As Object, ByVal pName2 As String, ByVal pValue2 As Object)
        MyBase.New(2)
        Add(New CNameValue(pName1, pValue1))
        Add(New CNameValue(pName2, pValue2))
    End Sub
    Public Sub New(ByVal pName1 As String, ByVal pValue1 As Object, ByVal pName2 As String, ByVal pValue2 As Object, ByVal pName3 As String, ByVal pValue3 As Object)
        MyBase.New(3)
        Add(New CNameValue(pName1, pValue1))
        Add(New CNameValue(pName2, pValue2))
        Add(New CNameValue(pName3, pValue3))
    End Sub
    Public Sub New(ByVal params As CCriteriaList)
        MyBase.New(params.Count)
        For Each i As CCriteria In params
            If TypeOf (i) Is CCriteriaGroup Then
                AddRange(New CNameValueList(CType(i, CCriteriaGroup).Group))
            Else
                Add(i.MarkerName, i.ColumnValue)
            End If
        Next
    End Sub
#End Region

#Region "Overloads"
    Public Overloads Sub Add(ByVal name As String, ByVal value As Object)
        Me.Add(New CNameValue(name, value))
    End Sub
    Public Overloads Sub Remove(ByVal name As String)
        Dim match As CNameValue = Nothing
        For Each i As CNameValue In Me
            If String.Compare(i.Name, name, True) = 0 Then
                match = i
                Exit For
            End If
        Next
        If Not IsNothing(match) Then Me.Remove(match)
    End Sub
    Default Public Overloads ReadOnly Property Item(ByVal name As String, ByVal create As Boolean) As CNameValue
        Get
            For Each i As CNameValue In Me
                If i.Name = name Then Return i
            Next
            If create Then
                Dim nv As New CNameValue(name, Nothing)
                Me.Add(nv)
                Return nv
            End If
            Return Nothing
        End Get
    End Property
#End Region

#Region "Sort"
    Public Sub SortByName()
        Sort(New CSortyByName)
    End Sub
    Public Sub SortAsIntegerDesc()
        Sort(New CSortAsIntegerDesc)
    End Sub
    Public Sub SortAsDecimalDesc()
        Sort(New CSortAsDecimalDesc)
    End Sub

    Private Class CSortAsIntegerDesc : Implements IComparer(Of CNameValue)
        Public Function Compare(ByVal x As Framework.CNameValue, ByVal y As Framework.CNameValue) As Integer Implements System.Collections.Generic.IComparer(Of Framework.CNameValue).Compare
            Return CInt(y.Value).CompareTo(CInt(x.Value))
        End Function
    End Class
    Private Class CSortAsDecimalDesc : Implements IComparer(Of CNameValue)
        Public Function Compare(ByVal x As Framework.CNameValue, ByVal y As Framework.CNameValue) As Integer Implements System.Collections.Generic.IComparer(Of Framework.CNameValue).Compare
            Return CDec(y.Value).CompareTo(CDec(x.Value))
        End Function
    End Class
    Private Class CSortyByName : Implements IComparer(Of CNameValue)
        Public Function Compare(ByVal x As Framework.CNameValue, ByVal y As Framework.CNameValue) As Integer Implements System.Collections.Generic.IComparer(Of Framework.CNameValue).Compare
            Return x.Name.CompareTo(y.Name)
        End Function
    End Class
#End Region

#Region "Sum"
    Public Function SumAsInteger() As Integer
        Dim total As Integer = 0
        For Each i As CNameValue In Me
            total += CInt(i.Value)
        Next
        Return total
    End Function
    Public Function SumAsDecimal() As Decimal
        Dim total As Decimal = 0
        For Each i As CNameValue In Me
            total += CDec(i.Value)
        Next
        Return total
    End Function
#End Region

End Class
