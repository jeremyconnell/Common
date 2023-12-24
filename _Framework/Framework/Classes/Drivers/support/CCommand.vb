Imports System.Runtime.Serialization

<CLSCompliant(True)>
Public Enum ECmdType
    Sql
    StoredProcedure
    ParameterisedSql
    InsertThenSelectIdentity
End Enum


<Serializable(), DataContract(), CLSCompliant(True)>
Public Class CCommand

#Region "Data"
    'Query
    <DataMember(Order:=1)> Public CommandType As ECmdType
    <DataMember(Order:=2)> Public Text As String

    'Either/or (if stored proc or param-query)
    <DataMember(Order:=3)> Public ParametersNamed As CNameValueList
    <DataMember(Order:=4)> Public ParametersUnnamed As Object()

    'Optional
    <NonSerialized()> Public Transaction As IDbTransaction
    <DataMember(Order:=5)> Public TimeoutSecs As Integer = CConfigBase.CommandTimeoutSecs  'Defaults to 30, which matches IDbConnection default value
#End Region

#Region "Constructors"
    'Protobuf: cannot serialise object
    'Shared Sub New()
    '    CProto.Prepare(Of CCommand)()
    'End Sub
    Private Sub New()
    End Sub

    'Simple
    Public Sub New(ByVal sql As String)
        Me.CommandType = ECmdType.Sql
        Me.Text = sql
    End Sub
    Public Sub New(ByVal spName As String, ByVal parameters As CNameValueList)
        Me.New(spName, parameters, True)
    End Sub
    Public Sub New(ByVal spName As String, ByVal parameters As CNameValueList, ByVal isStoredProcedure As Boolean)
        Me.Text = spName
        Me.ParametersNamed = parameters

        If isStoredProcedure Then
            Me.CommandType = ECmdType.StoredProcedure
        Else
            Me.CommandType = ECmdType.ParameterisedSql
        End If
    End Sub
    Public Sub New(ByVal spName As String, ByVal parameters As Object())
        Me.Text = spName
        Me.ParametersUnnamed = parameters
        Me.CommandType = ECmdType.StoredProcedure
    End Sub

    'Transaction Supplied
    Public Sub New(ByVal sql As String, ByVal tx As IDbTransaction)
        Me.New(sql)
        Me.Transaction = tx
    End Sub
    Public Sub New(ByVal spName As String, ByVal parameters As CNameValueList, ByVal tx As IDbTransaction)
        Me.New(spName, parameters, True)
    End Sub
    Public Sub New(ByVal spName As String, ByVal parameters As CNameValueList, ByVal isStoredProcedure As Boolean, ByVal tx As IDbTransaction)
        Me.New(spName, parameters, isStoredProcedure)
        Me.Transaction = tx
    End Sub
    Public Sub New(ByVal spName As String, ByVal parameters As Object(), ByVal tx As IDbTransaction)
        Me.New(spName, parameters)
        Me.Transaction = tx
    End Sub

    'Timeout Supplied
    Public Sub New(ByVal sql As String, ByVal timeoutSecs As Integer)
        Me.New(sql)
        Me.TimeoutSecs = timeoutSecs
    End Sub
    Public Sub New(ByVal spName As String, ByVal parameters As CNameValueList, ByVal timeoutSecs As Integer)
        Me.New(spName, parameters)
        Me.TimeoutSecs = timeoutSecs
    End Sub
    Public Sub New(ByVal spName As String, ByVal parameters As CNameValueList, ByVal isStoredProcedure As Boolean, ByVal timeoutSecs As Integer)
        Me.New(spName, parameters, isStoredProcedure)
        Me.TimeoutSecs = timeoutSecs
    End Sub
	Public Sub New(ByVal spName As String, ByVal parameters As Object(), ByVal timeoutSecs As Integer)
		Me.New(spName, parameters)
		Me.TimeoutSecs = timeoutSecs
	End Sub
	Public Sub New(cmd As IDbCommand)
		If cmd.CommandType = Data.CommandType.StoredProcedure Then
			Me.CommandType = ECmdType.StoredProcedure
		ElseIf Not IsNothing(cmd.Parameters) AndAlso cmd.Parameters.Count > 0 Then
			Me.CommandType = ECmdType.ParameterisedSql
		Else
			Me.CommandType = ECmdType.Sql
		End If

		Me.Text = cmd.CommandText
		Me.TimeoutSecs = cmd.CommandTimeout
		Me.Transaction = cmd.Transaction

		If IsNothing(cmd.Parameters) Then Exit Sub

		Me.ParametersNamed = New CNameValueList(cmd.Parameters.Count)
		For Each i As IDbDataParameter In cmd.Parameters
			Me.ParametersNamed.Add(i.ParameterName, i.Value)
		Next
	End Sub
#End Region

End Class
