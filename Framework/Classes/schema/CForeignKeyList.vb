
Imports ProtoBuf
Imports System.Runtime.Serialization

<DataContract, ProtoContract>
Public Class CForeignKeyList : Inherits List(Of CForeignKey)
    'Data
    <DataMember(Order:=1)> Public MD5 As Guid

    'Preconstructor
    Shared Sub New()
        CProto.Prepare(Of CForeignKeyList)()
    End Sub

    'Constructors
    Public Sub New()
    End Sub
    Public Sub New(list As IList(Of CForeignKey))
        MyBase.New(list)
        ResetMd5()
    End Sub
    Public Sub New(db As CDataSrc)
		Me.New(db.ForeignKeys.Tables(0))
	End Sub
	Friend Sub New(dt As DataTable)
		For Each dr As DataRow In dt.Rows
			'Read Cols
			Dim tableName As String = CAdoData.GetStr(dr, "TableName")
			Dim keyName As String = CAdoData.GetStr(dr, "ForeignKeyName")
			Dim colName As String = CAdoData.GetStr(dr, "ColumnName_RefTable_RefColumn")
			Dim cascadeUpdate As Boolean = CAdoData.GetBool(dr, "CascadeUpdate")
			Dim cascadeDelete As Boolean = CAdoData.GetBool(dr, "CascadeDelete")

			'Skip Sys tables
			If tableName.StartsWith("sys.") OrElse tableName.StartsWith("dbo.sys") Then Continue For

			'Most fks are single-col
			Dim fk1 As New CForeignKey1(tableName, keyName, colName, cascadeUpdate, cascadeDelete)
			If Not Has(keyName) Then
				Add(fk1)
				Continue For
			End If

			'Repeated rows => convert to multi-col
			Dim fk As CForeignKey = GetByName(keyName)
			Remove(fk)
			If TypeOf fk Is CForeignKey1 Then
				fk1 = CType(fk, CForeignKey1)
				Add(New CForeignKeyN(fk1, colName))
			Else
				Dim fkN As CForeignKeyN = CType(fk, CForeignKeyN)
				Add(New CForeignKeyN(fkN, colName))
			End If

		Next

		ResetMd5()
	End Sub

	Public Shadows Sub Add(fk As CForeignKey)
		MyBase.Add(fk)

		m_index = Nothing
		m_indexByName = Nothing
		m_indexByTable = Nothing

		ResetMd5()
	End Sub
	Public Shadows Sub Remove(fk As CForeignKey)
		MyBase.Remove(fk)

		m_index = Nothing
		m_indexByName = Nothing
		m_indexByTable = Nothing

		ResetMd5()
	End Sub
	Public Sub ResetMd5()

		Me.Sort()

		Dim g As New List(Of Guid)
		For Each i As CForeignKey In Me
			g.Add(i.MD5)
		Next
		Me.MD5 = CBinary.MD5_(g)
	End Sub




	'Diff
	Public Function Diff(ref As CForeignKeyList) As CForeignKeyListDiff
        Dim d As New CForeignKeyListDiff
        Diff(ref, d.Missing, d.Added, d.Same, d.Different)
        Return d
    End Function

    Public Sub Diff(ref As CForeignKeyList, missing As CForeignKeyList, added As CForeignKeyList, same As CForeignKeyList, diff As List(Of CForeignKeyDiff))
        For Each i As String In Me.IndexByName.Keys
            Dim info As CForeignKey = Me.IndexByName(i)
            Dim refInfo As CForeignKey = Nothing

            If Not ref.IndexByName.TryGetValue(i, refInfo) Then
                added.Add(info)   'Name not recog
            Else
                If refInfo.MD5 = info.MD5 Then
                    same.Add(info)  'Name+Nature => Exact match
                Else
                    diff.Add(New CForeignKeyDiff(info, refInfo))  'Nature is different
                End If
            End If
        Next
        For Each i As String In ref.IndexByName.Keys
            Dim refInfo As CForeignKey = ref.IndexByName(i)
            If Not Me.IndexByName.ContainsKey(i) Then
                missing.Add(refInfo)   'Name not recog
            End If
        Next
    End Sub






    Public Function Has(name As String) As Boolean
        Return IndexByName.ContainsKey(name.ToLower)
    End Function
    Public Function GetByName(name As String) As CForeignKey
        Dim pk As CForeignKey = Nothing
        IndexByName.TryGetValue(name.ToLower, pk)
        Return pk
    End Function
    Public Function GetByTable(tableName As String) As CForeignKeyList
        Dim pk As CForeignKeyList = Nothing
        If Not IndexByTable.TryGetValue(tableName.ToLower, pk) Then
            pk = New CForeignKeyList
            IndexByTable.Add(tableName.ToLower, pk)
        End If
        Return pk
    End Function
    Public Function GetByRefTable(tableName As String) As CForeignKeyList
        Dim pk As CForeignKeyList = Nothing
        If Not IndexByRefTable.TryGetValue(tableName.ToLower, pk) Then
            pk = New CForeignKeyList
            IndexByRefTable.Add(tableName.ToLower, pk)
        End If
        Return pk
    End Function

    Private m_index As Dictionary(Of Guid, CForeignKey)
    Private m_indexByName As Dictionary(Of String, CForeignKey)
    Private m_indexByTable As Dictionary(Of String, CForeignKeyList)
    Private m_indexByRefTable As Dictionary(Of String, CForeignKeyList)

    Public ReadOnly Property Index As Dictionary(Of Guid, CForeignKey)
        Get
			If IsNothing(m_index) OrElse m_index.Count <> Me.Count Then
				Dim temp As New Dictionary(Of Guid, CForeignKey)
				For Each i As CForeignKey In Me
					temp.Add(i.MD5, i)
				Next
				m_index = temp
			End If
			Return m_index
        End Get
    End Property
    Public ReadOnly Property IndexByName As Dictionary(Of String, CForeignKey)
        Get
            If IsNothing(m_indexByName) OrElse Me.Count <> m_indexByName.Count Then
                Dim temp As New Dictionary(Of String, CForeignKey)
                For Each i As CForeignKey In Me
                    temp.Add(i.KeyName, i)
                Next
                m_indexByName = temp
            End If
            Return m_indexByName
        End Get
    End Property

    Public ReadOnly Property IndexByTable As Dictionary(Of String, CForeignKeyList)
        Get
            If IsNothing(m_indexByTable) Then
                Dim temp As New Dictionary(Of String, CForeignKeyList)

                For Each i As CForeignKey In Me
                    Dim list As CForeignKeyList = Nothing
                    If Not temp.TryGetValue(i.TableName.ToLower, list) Then
                        list = New CForeignKeyList
                        temp.Add(i.TableName.ToLower, list)
                    End If
                    list.Add(i)
                Next

                m_indexByTable = temp
            End If
            Return m_indexByTable
        End Get
    End Property

    Public ReadOnly Property IndexByRefTable As Dictionary(Of String, CForeignKeyList)
        Get
            If IsNothing(m_indexByRefTable) Then
                Dim temp As New Dictionary(Of String, CForeignKeyList)

                For Each i As CForeignKey In Me
                    Dim list As CForeignKeyList = Nothing
                    If Not temp.TryGetValue(i.ReferenceTable.ToLower, list) Then
                        list = New CForeignKeyList
                        temp.Add(i.ReferenceTable.ToLower, list)
                    End If
                    list.Add(i)
                Next

                m_indexByRefTable = temp
            End If
            Return m_indexByRefTable
        End Get
    End Property
End Class