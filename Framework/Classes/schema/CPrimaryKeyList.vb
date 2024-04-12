Imports System.Text
Imports ProtoBuf
Imports System.Runtime.Serialization

<DataContract>
Public Class CPrimaryKeyList : Inherits List(Of CPrimaryKey)

    'Pre-constructors
    Shared Sub New()
        CProto.Prepare(Of CPrimaryKeyList)()
    End Sub

    'Constructor
    Public Sub New()

    End Sub
    Public Sub New(db As CDataSrc)
        Me.New(db.ExecuteDataSet(CDataSrc.SQL_TO_LIST_PKS).Tables(0))
    End Sub
	Public Sub New(dt As DataTable)
		MyBase.New(dt.Rows.Count)

		Dim last As CPrimaryKey = Nothing
		For Each dr As DataRow In dt.Rows
			Dim pk As New CPrimaryKey(dr)
			If pk.SchemaAndTable.StartsWith("sys.") OrElse pk.SchemaAndTable.StartsWith("dbo.sys") Then Continue For

			If Not IsNothing(last) Then
				If pk.KeyName = last.KeyName AndAlso pk.SchemaAndTable = last.SchemaAndTable Then
					last.ColumnNames.Add(pk.ColumnNames(0))
					Continue For
				End If
			End If

			Me.Add(pk)
			last = pk
		Next
	End Sub

	Public ReadOnly Property MD5 As Guid
		Get
			Dim list As New List(Of Guid)(Me.Count)
			For Each i As CPrimaryKey In Me
				list.Add(i.MD5)
			Next
			Return CBinary.MD5_(list)
		End Get
	End Property



	Public Function GetByName(name As String) As CPrimaryKey
        Dim pk As CPrimaryKey = Nothing
        IndexByName.TryGetValue(name.ToLower, pk)
        Return pk
    End Function
    Public Function GetByTable(name As String) As CPrimaryKey
        Dim pk As CPrimaryKey = Nothing
        IndexByTable.TryGetValue(name.ToLower, pk)
        Return pk
    End Function
    Public Function GetByHash(md5 As Guid) As CPrimaryKey
        Dim pk As CPrimaryKey = Nothing
        Index.TryGetValue(md5, pk)
        Return pk
    End Function

	Public Function Has(name As String) As Boolean
		Return IndexByName.ContainsKey(name.ToLower)
	End Function

	Private m_index As Dictionary(Of Guid, CPrimaryKey)
    Private m_indexByName As Dictionary(Of String, CPrimaryKey)
    Private m_indexByTable As Dictionary(Of String, CPrimaryKey)
    Public ReadOnly Property Index As Dictionary(Of Guid, CPrimaryKey)
        Get
            If IsNothing(m_index) Then
                Dim temp As New Dictionary(Of Guid, CPrimaryKey)
                For Each i As CPrimaryKey In Me
                    temp.Add(i.MD5, i)
                Next
                m_index = temp
            End If
            Return m_index
        End Get
    End Property


	Public ReadOnly Property IndexByName As Dictionary(Of String, CPrimaryKey)
        Get
            If IsNothing(m_indexByName) OrElse Me.Count <> m_indexByName.Count Then
                Dim temp As New Dictionary(Of String, CPrimaryKey)
                For Each i As CPrimaryKey In Me
                    temp.Add(i.KeyName.ToLower, i)
                Next
                m_indexByName = temp
            End If
            Return m_indexByName
        End Get
    End Property
    Public ReadOnly Property IndexByTable As Dictionary(Of String, CPrimaryKey)
        Get
            If IsNothing(m_indexByTable) OrElse Me.Count <> m_indexByTable.Count Then
                Dim temp As New Dictionary(Of String, CPrimaryKey)
                For Each i As CPrimaryKey In Me
                    temp.Add(i.SchemaAndTable.ToLower, i)
                Next
                m_indexByTable = temp
            End If
            Return m_indexByTable
        End Get
    End Property
End Class
