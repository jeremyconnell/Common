Imports System.Runtime.Serialization

<CLSCompliant(True)>
Public Enum EBoolOperator
    [And] = 1
    [Or] = 2
End Enum


<CLSCompliant(True), DataContract(), Serializable()>
Public Class CCriteriaList : Inherits List(Of CCriteria)

#Region "Members"
    <DataMember(Order:=1)> Public BoolOperator As EBoolOperator = EBoolOperator.And
    <DataMember(Order:=2)> Public Parameters As New CNameValueList
#End Region

#Region "Constructors"
    'Protobuf: cannot serialise object
    'Shared Sub New()
    '    CProto.Prepare(Of CNameValueList)()
    'End Sub

    'Constructors - Standard
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal boolOperator As EBoolOperator)
        MyBase.New()
        Me.BoolOperator = boolOperator
    End Sub
    Public Sub New(ByVal capacity As Integer)
        MyBase.New(capacity)
    End Sub

    'Constructors - Single Criteria
    Public Sub New(ByVal colName As String, ByVal colValue As Object)
        Me.New(New CCriteria(colName, colValue))
    End Sub
    Public Sub New(ByVal colName As String, ByVal sign As ESign, ByVal colValue As Object)
        Me.New(New CCriteria(colName, sign, colValue))
    End Sub
    Public Sub New(ByVal item As CCriteria)
        Me.New(1)
        Me.Add(item)
    End Sub
    Public Sub New(ByVal ParamArray item() As CCriteria)
        Me.New(item.Length)
        AddRange(item)
    End Sub

    'Constructors - Double Criteria
    Public Sub New(ByVal colName1 As String, ByVal colValue1 As Object, colName2 As String, colValue2 As Object)
        Me.New(New CCriteriaList(New CNameValueList(colName1, colValue1, colName2, colValue2)))
    End Sub

    'Constructors - Collections
    Public Sub New(ByVal collection As CCriteriaList)
        MyBase.New(collection)
        Me.BoolOperator = collection.BoolOperator
    End Sub
    Public Sub New(ByVal nameValues As CNameValueList, ByVal boolOperator As EBoolOperator)
        Me.New(nameValues)
        Me.BoolOperator = boolOperator
    End Sub
    Public Sub New(ByVal nameValues As CNameValueList)
        MyBase.New(nameValues.Count)
        For Each i As CNameValue In nameValues
            Me.Add(New CCriteria(i))
        Next
    End Sub
#End Region

#Region "Add Overloads"
    Public Overloads Sub Add(ByVal name As String, ByVal value As Object)
        Me.Add(New CCriteria(name, value))
    End Sub
    Public Overloads Sub Add(ByVal name As String, ByVal sign As ESign, ByVal value As Object)
        Me.Add(New CCriteria(name, sign, value))
    End Sub
    Public Overloads Sub Add(ByVal ParamArray criteria() As CCriteria)
        For Each i As CCriteria In criteria
            Me.Add(i)
        Next
    End Sub
#End Region

#Region "Partitions"
    Public ReadOnly Property Simple() As CCriteriaList
        Get
            Dim temp As New CCriteriaList(Me.Count)
            For Each i As CCriteria In Me
                If TypeOf i Is CCriteriaGroup Then Continue For
                temp.Add(i)
            Next
            Return temp
        End Get
    End Property
    Public ReadOnly Property Complex() As CCriteriaList
        Get
            Dim temp As New CCriteriaList(Me.Count)
            For Each i As CCriteria In Me
                If TypeOf i Is CCriteriaGroup Then temp.Add(i)
            Next
            Return temp
        End Get
    End Property
#End Region

End Class