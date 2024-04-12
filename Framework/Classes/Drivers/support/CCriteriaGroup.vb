<CLSCompliant(True)> <Serializable()> _
Public Class CCriteriaGroup : Inherits CCriteria

#Region "Constructors"
    'Simple
    Public Sub New()
        Me.New(New CCriteriaList, EBoolOperator.Or)
    End Sub
    Public Sub New(ByVal logic As EBoolOperator)
        Me.New(New CCriteriaList, logic)
    End Sub
    Public Sub New(ByVal group As CCriteriaList)
        Me.New(group, EBoolOperator.Or)
    End Sub
    Public Sub New(ByVal group As CCriteriaList, ByVal logic As EBoolOperator)
        MyBase.New(Nothing, Nothing)

        Me.m_group = group
        Me.Logic = logic
    End Sub

    'Criteria-orientated
    Public Sub New(ByVal colName As String, ByVal colValue As Object, Optional ByVal logic As EBoolOperator = EBoolOperator.Or)
        Me.New(New CCriteria(colName, colValue), logic)
    End Sub
    Public Sub New(ByVal colName As String, ByVal sign As ESign, ByVal colValue As Object, Optional ByVal logic As EBoolOperator = EBoolOperator.Or)
        Me.New(New CCriteria(colName, sign, colValue), logic)
    End Sub
    Public Sub New(ByVal item As CCriteria, Optional ByVal logic As EBoolOperator = EBoolOperator.Or)
        Me.New(New CCriteriaList(item), logic)
    End Sub
    Public Sub New(ByVal logic As EBoolOperator, ByVal ParamArray item() As CCriteria)
        Me.New(New CCriteriaList(item), logic)
    End Sub
#End Region

#Region "Add-Criteria"
    Public Overloads Sub Add(ByVal name As String, ByVal value As Object)
        Group.Add(New CCriteria(name, value))
    End Sub
    Public Overloads Sub Add(ByVal name As String, ByVal sign As ESign, ByVal value As Object)
        Group.Add(New CCriteria(name, sign, value))
    End Sub
    Public Overloads Sub Add(ByVal criteria As CCriteria)
        Group.Add(criteria)
    End Sub
    Public Overloads Sub Add(ByVal ParamArray criteria() As CCriteria)
        For Each i As CCriteria In criteria
            Me.Add(i)
        Next
    End Sub
#End Region

#Region "Members"
    Public Logic As EBoolOperator
    Private m_group As CCriteriaList
#End Region

#Region "Properties"
    Public ReadOnly Property Group() As CCriteriaList
        Get
            Return m_group
        End Get
    End Property
#End Region

End Class
