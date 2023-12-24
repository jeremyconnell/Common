Imports ProtoBuf
Imports System.Runtime.Serialization

<DataContract, ProtoContract>
Public Class CTableInfoList : Inherits List(Of CTableInfo)
    'Data
    <DataMember(Order:=1)> Public MD5 As Guid

    Public PrimaryKeys As CPrimaryKeyList
    Public ForeignKeys As CForeignKeyList
    Public Indexes As CIndexInfoList

    'Preconstructor
    Shared Sub New()
        CProto.Prepare(Of CTableInfoList)()
    End Sub

    'Constructor
    Friend Sub New()
		Me.PrimaryKeys = New CPrimaryKeyList()
		Me.ForeignKeys = New CForeignKeyList()
		Me.Indexes = New CIndexInfoList()
	End Sub
	Public Sub New(db As CDataSrc, sysTables As Boolean)
		Me.New(db.AllTableNames(True), db.AllTableColumnsAndTypesAsDict, New CPrimaryKeyList(db), New CForeignKeyList(db), New CIndexInfoList(db), sysTables)
	End Sub
	Public Sub New(names As List(Of String),
				  cols As Dictionary(Of String, List(Of String)),
				  pks As CPrimaryKeyList,
				  fks As CForeignKeyList,
				  indexes As CIndexInfoList, sysTables As Boolean)

		MyBase.New(names.Count)

		'Store
		Me.PrimaryKeys = pks
		Me.ForeignKeys = fks
		Me.Indexes = indexes


		'Build
		For Each i As String In names
			If Not sysTables Then
				If i.StartsWith("sys.") OrElse i.StartsWith("dbo.sys") Then Continue For
			End If

			Dim col As List(Of String) = Nothing
			If Not cols.TryGetValue(i, col) Then col = New List(Of String)(0)

			Add(New CTableInfo(i, col, pks.GetByTable(i), fks.GetByTable(i), indexes.GetByTable(i)))
		Next

		'Hash
		Me.Sort()
		Dim g As New List(Of Guid)
		For Each i As CTableInfo In Me
			g.Add(i.MD5)
		Next
		Me.MD5 = CBinary.MD5_(g)
	End Sub


	Public ReadOnly Property AllColumns As List(Of CColumn)
        Get
            Dim temp As New List(Of CColumn)
            For Each i As CTableInfo In Me
                temp.AddRange(i.Columns)
            Next
            Return temp
        End Get
    End Property
	Public ReadOnly Property Names As List(Of String)
		Get
			Dim temp As New List(Of String)(IndexByName.Keys)
			temp.Sort()
			Return temp
		End Get
	End Property

	Public ReadOnly Property Names_ As List(Of String)
		Get
			Dim temp As New List(Of String)(Me.Count)
			For Each tbl As String In Names
				temp.Add(String.Concat("[", tbl.Replace(".", "].["), "]"))
			Next
			Return temp
		End Get
	End Property

    Public Function Has(name As String) As Boolean
        name = name.ToLower().Replace("[", "").Replace("]", "").Replace("dbo.", "")
        Return IndexByName.ContainsKey(name.ToLower)
    End Function
    Public Function GetByName(name As String) As CTableInfo
        name = name.ToLower().Replace("[", "").Replace("]", "").Replace("dbo.", "")
        Dim pk As CTableInfo = Nothing
        IndexByName.TryGetValue(name.ToLower, pk)
        Return pk
    End Function
    Public Function GetByHash(md5 As Guid) As CTableInfo
        Dim pk As CTableInfo = Nothing
        Index.TryGetValue(md5, pk)
        Return pk
    End Function




	Private m_namesAndHashes As List(Of String)
	Public ReadOnly Property NamesAndHashes As List(Of String)
		Get
			If IsNothing(m_namesAndHashes) OrElse Me.Count <> m_namesAndHashes.Count Then
				Dim temp As New List(Of String)
				For Each i As CTableInfo In Me
					temp.Add(String.Concat(i.TableName, vbTab, CBinary.ToBase64(i.MD5).Substring(0, 8).ToUpper))
				Next
				m_namesAndHashes = temp
			End If
			Return m_namesAndHashes
		End Get
	End Property



	'Index
	Private m_index As Dictionary(Of Guid, CTableInfo)
    Private m_indexByName As Dictionary(Of String, CTableInfo)
    Public ReadOnly Property Index As Dictionary(Of Guid, CTableInfo)
        Get
            If IsNothing(m_index) Then
                Dim temp As New Dictionary(Of Guid, CTableInfo)
                For Each i As CTableInfo In Me
                    temp.Add(i.MD5, i)
                Next
                m_index = temp
            End If
            Return m_index
        End Get
    End Property
    Public ReadOnly Property IndexByName As Dictionary(Of String, CTableInfo)
        Get
            If IsNothing(m_indexByName) OrElse Me.Count <> m_indexByName.Count Then
                Dim temp As New Dictionary(Of String, CTableInfo)
                For Each i As CTableInfo In Me
                    temp.Add(i.TableName.ToLower().Replace("[", "").Replace("]", "").Replace("dbo.", ""), i)
                Next
                m_indexByName = temp
            End If
            Return m_indexByName
        End Get
    End Property

    'Diff
    Public Function Diff(ref As CTableInfoList) As CTableListDiff
        Dim d As New CTableListDiff
        Diff(ref, d.Missing, d.Added, d.Same, d.Different)
        Return d
    End Function

    Public Sub Diff(ref As CTableInfoList, missing As CTableInfoList, added As CTableInfoList, same As CTableInfoList, diff As List(Of CTableDiff))
        For Each i As String In Me.IndexByName.Keys
			Dim info As CTableInfo = Me.GetByName(i)
			Dim refInfo As CTableInfo = Nothing

			If Not ref.Has(i) Then
				added.Add(info)   'Name not recog
			Else
				refInfo = ref.GetByName(i)
				If refInfo.MD5 = info.MD5 Then
                    same.Add(info)  'Name+Nature => Exact match
                Else
                    diff.Add(New CTableDiff(info, refInfo))  'Nature is different
                End If
            End If
        Next
		For Each i As String In ref.IndexByName.Keys
			Dim refInfo As CTableInfo = ref.IndexByName(i)
			If Not Me.Has(i) Then missing.Add(refInfo)   'Name not recog
		Next
	End Sub
End Class