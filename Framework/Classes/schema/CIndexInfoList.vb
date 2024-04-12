
Imports ProtoBuf
Imports System.Runtime.Serialization

<DataContract, ProtoContract>
Public Class CIndexInfoList : Inherits List(Of CIndexInfo)
    'Data
    <DataMember(Order:=1)> Public MD5 As Guid

    'Preconstructor
    Shared Sub New()
        CProto.Prepare(Of CIndexInfoList)()
    End Sub

    'Constructors
    Friend Sub New()

    End Sub
    Public Sub New(count As Integer)
        MyBase.New(count)
    End Sub
    Public Sub New(db As CDataSrc)
        Me.New(db.IndexesByTable(True), db.IndexesByTable(False))
    End Sub

    Public Sub New(unique As Dictionary(Of String, Dictionary(Of String, List(Of String))), nonUnique As Dictionary(Of String, Dictionary(Of String, List(Of String))))
        AddRange(True, unique)
        AddRange(False, nonUnique)
        ComputeHash()
    End Sub
    Public Sub New(table As String, unique As Dictionary(Of String, List(Of String)), nonUnique As Dictionary(Of String, List(Of String)))
        AddRange(True, table, unique)
        AddRange(False, table, nonUnique)
        ComputeHash()
    End Sub
    Public Sub New(isUnique As Boolean, table As String, dict As Dictionary(Of String, List(Of String)))
        AddRange(isUnique, table, dict)
        ComputeHash()
    End Sub

    Private Sub ComputeHash()
        Me.Sort()

        Dim g As New List(Of Guid)
        For Each i As CIndexInfo In Me
            g.Add(i.MD5)
        Next
        Me.MD5 = CBinary.MD5_(g)
    End Sub
    Private Overloads Sub AddRange(isUnique As Boolean, dict As Dictionary(Of String, Dictionary(Of String, List(Of String))))
        For Each i As String In dict.Keys
            If i.StartsWith("sys.") OrElse i.StartsWith("dbo.sys") Then Continue For

            AddRange(isUnique, i, dict(i))
        Next
    End Sub
	Private Overloads Sub AddRange(isUnique As Boolean, table As String, dict As Dictionary(Of String, List(Of String)))
		For Each i As String In dict.Keys
			If i.StartsWith("sys.") OrElse i.StartsWith("dbo.sys") Then Continue For

			Me.Add(New CIndexInfo(table, i, isUnique, dict(i)))
		Next
	End Sub



	Public ReadOnly Property Unique As CIndexInfoList
        Get
            Return GetByIsUnique(True)
        End Get
    End Property
    Public ReadOnly Property Normal As CIndexInfoList
        Get
            Return GetByIsUnique(False)
        End Get
    End Property

    Public Function Has(name As String) As Boolean
        Return IndexByName.ContainsKey(name.ToLower)
    End Function
    Public Function GetByName(name As String) As CIndexInfo
        Dim pk As CIndexInfo = Nothing
        IndexByName.TryGetValue(name.ToLower, pk)
        Return pk
    End Function
    Public Function GetByTable(tableName As String) As CIndexInfoList
        Dim pk As CIndexInfoList = Nothing
        If Not IndexByTable.TryGetValue(tableName.ToLower, pk) Then
            pk = New CIndexInfoList
            IndexByTable.Add(tableName, pk)
        End If
        Return pk
    End Function
    Public Function GetByHash(md5 As Guid) As CIndexInfoList
        Dim pk As CIndexInfoList = Nothing
        Index.TryGetValue(md5, pk)
        Return pk
    End Function
    Public Function GetByIsUnique(isUniq As Boolean) As CIndexInfoList
        Dim pk As CIndexInfoList = Nothing
        IndexByIsUniq.TryGetValue(isUniq, pk)
        Return pk
    End Function

    'Indexes
    Private m_index As Dictionary(Of Guid, CIndexInfoList)
    Private m_indexByName As Dictionary(Of String, CIndexInfo)
    Private m_indexByTable As Dictionary(Of String, CIndexInfoList)
    Private m_indexByIsUnique As Dictionary(Of Boolean, CIndexInfoList)
    Public ReadOnly Property Index As Dictionary(Of Guid, CIndexInfoList)
        Get
			If IsNothing(m_index) OrElse Me.Count <> m_index.Count Then
				Dim temp As New Dictionary(Of Guid, CIndexInfoList)
				For Each i As CIndexInfo In Me
					Dim list As CIndexInfoList = Nothing
					If Not temp.TryGetValue(i.MD5, list) Then
						list = New CIndexInfoList(2)
						temp.Add(i.MD5, list)
					End If
					list.Add(i)
				Next
				m_index = temp
			End If
			Return m_index
        End Get
    End Property
    Public ReadOnly Property IndexByName As Dictionary(Of String, CIndexInfo)
        Get
            If IsNothing(m_indexByName) OrElse Me.Count <> m_indexByName.Count Then
                Dim temp As New Dictionary(Of String, CIndexInfo)
                For Each i As CIndexInfo In Me
                    temp.Add(i.IndexName_.ToLower, i)
                Next
                m_indexByName = temp
            End If
            Return m_indexByName
        End Get
    End Property
    Public ReadOnly Property IndexByTable As Dictionary(Of String, CIndexInfoList)
        Get
            If IsNothing(m_indexByTable) Then
                Dim temp As New Dictionary(Of String, CIndexInfoList)

                For Each i As CIndexInfo In Me
                    Dim list As CIndexInfoList = Nothing
                    If Not temp.TryGetValue(i.TableName.ToLower, list) Then
                        list = New CIndexInfoList
                        temp.Add(i.TableName.ToLower, list)
                    End If
                    list.Add(i)
                Next

                m_indexByTable = temp
            End If
            Return m_indexByTable
        End Get
    End Property

    Public ReadOnly Property IndexByIsUniq As Dictionary(Of Boolean, CIndexInfoList)
        Get
            If IsNothing(m_indexByIsUnique) Then
                Dim isUniq As New CIndexInfoList
                Dim normal As New CIndexInfoList
                For Each i As CIndexInfo In Me
                    If i.IsUnique Then
                        isUniq.Add(i)
                    Else
                        normal.Add(i)
                    End If
                Next

                Dim temp As New Dictionary(Of Boolean, CIndexInfoList)
                temp.Add(True, isUniq)
                temp.Add(False, normal)

                m_indexByIsUnique = temp
            End If
            Return m_indexByIsUnique
        End Get
    End Property

    'Diff
    Public Function Diff(ref As CIndexInfoList) As CIndexListDiff
        Dim d As New CIndexListDiff
        Diff(ref, d.Missing, d.Added, d.Same, d.Different)
        Return d
    End Function

    Public Sub Diff(ref As CIndexInfoList, missing As CIndexInfoList, added As CIndexInfoList, same As CIndexInfoList, diff As List(Of CIndexDiff))
        For Each i As String In Me.IndexByName.Keys
            Dim info As CIndexInfo = Me.IndexByName(i)
            Dim refInfo As CIndexInfo = Nothing

            If Not ref.IndexByName.TryGetValue(i, refInfo) Then
                added.Add(info)   'Name not recog
            Else
                If refInfo.MD5 = info.MD5 Then
                    same.Add(info)  'Name+Nature => Exact match
                Else
                    diff.Add(New CIndexDiff(info, refInfo))  'Nature is different
                End If
            End If
        Next
        For Each i As String In ref.IndexByName.Keys
            Dim refInfo As CIndexInfo = ref.IndexByName(i)
            If Not Me.IndexByName.ContainsKey(i) Then
                missing.Add(refInfo)   'Name not recog
            End If
        Next
    End Sub




End Class