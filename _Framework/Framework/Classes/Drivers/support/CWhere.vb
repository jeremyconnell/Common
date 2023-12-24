Imports System.Runtime.Serialization
Imports ProtoBuf

<CLSCompliant(True), Serializable()>
Public Enum EWhereType
    All
    Column
    Columns
    Unsafe
End Enum

<CLSCompliant(True), DataContract(), KnownType(GetType(CSelectWhere)), Serializable()>
Public Class CWhere

#Region "Constructor"
    'Protobuf: cannot serialise object
    'Shared Sub New()
    '    CProto.Prepare(Of CWhere)()
    'End Sub
    Protected Sub New()
    End Sub

    'Basic Combinations
    Public Sub New(ByVal tableName As String, ByVal txOrNull As IDbTransaction)
        Me.TableName = tableName
        Me.TxOrNull = txOrNull
    End Sub
    Public Sub New(ByVal tableName As String, ByVal criteria As CCriteria, ByVal txOrNull As IDbTransaction)
        Me.New(tableName, txOrNull)
        Me.Criteria = criteria
    End Sub
    Public Sub New(ByVal tableName As String, ByVal criteriaList As CCriteriaList, ByVal txOrNull As IDbTransaction)
        Me.New(tableName, txOrNull)
        Me.CriteriaList = criteriaList
    End Sub
    Public Sub New(ByVal tableName As String, ByVal unsafeWhereClause As String, ByVal txOrNull As IDbTransaction)
        Me.New(tableName, txOrNull)
        Me.UnsafeWhereClause = unsafeWhereClause
    End Sub
    'Overloads
    Public Sub New(ByVal tableName As String, ByVal values As CNameValueList, ByVal txOrNull As IDbTransaction)
        Me.New(tableName, New CCriteriaList(values), txOrNull)
    End Sub
    Public Sub New(ByVal tableName As String, ByVal name As String, ByVal value As Object, ByVal txOrNull As IDbTransaction)
        Me.New(tableName, New CCriteria(name, value), txOrNull)
    End Sub
#End Region

#Region "Data"
    'Required
    <DataMember(Order:=1)> Public TableName As String
    <NonSerialized()> _
    Public TxOrNull As IDbTransaction

    'Optional
    <DataMember(Order:=2)> Public UnsafeWhereClause As String
    <DataMember(Order:=3)> Public Criteria As CCriteria
    <DataMember(Order:=4)> Public CriteriaList As CCriteriaList
#End Region

#Region "Public"
    Public ReadOnly Property Type() As EWhereType
        Get
            If Not IsNothing(Criteria) Then Return EWhereType.Column
            If Not IsNothing(CriteriaList) Then Return EWhereType.Columns
            If Not IsNothing(UnsafeWhereClause) Then Return EWhereType.Unsafe
            Return EWhereType.All
        End Get
    End Property
#End Region

End Class
