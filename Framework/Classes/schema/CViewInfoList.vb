Imports ProtoBuf
Imports System.Runtime.Serialization

<DataContract, ProtoContract>
Public Class CViewInfoList : Inherits List(Of CViewInfo)
    'Data
    <DataMember(Order:=1)> Public MD5 As Guid


    'Preconstructor
    Shared Sub New()
        CProto.Prepare(Of CViewInfoList)()
    End Sub

    'Constructor
    Public Sub New(db As CDataSrc)
        Me.New(db.AllViewNames(True), db.AllViewColumnsAndTypesAsDict, db)
    End Sub
    Private Sub New(viewNames As List(Of String), cols As Dictionary(Of String, List(Of String)), db As CDataSrc)
        Me.New(viewNames, cols, db.StoredProcText(viewNames))
    End Sub

    Public Sub New(viewNames As List(Of String), cols As Dictionary(Of String, List(Of String)), scripts As Dictionary(Of String, String))
        MyBase.New(viewNames.Count)

        For Each i As String In viewNames
            If i.StartsWith("sys.") OrElse i.StartsWith("dbo.sys") Then Continue For

            Add(New CViewInfo(i, cols(i), scripts(i)))
        Next

        Me.Sort()
        Dim g As New List(Of Guid)
        For Each i As CViewInfo In Me
            g.Add(i.MD5)
        Next
        Me.MD5 = CBinary.MD5_(g)
    End Sub
    Friend Sub New()

    End Sub




	Private m_namesAndHashes As List(Of String)
	Public ReadOnly Property NamesAndHashes As List(Of String)
		Get
			If IsNothing(m_namesAndHashes) OrElse Me.Count <> m_namesAndHashes.Count Then
				Dim temp As New List(Of String)
				For Each i As CViewInfo In Me
					temp.Add(String.Concat(i.ViewName, vbTab, CBinary.ToBase64(i.MD5).Substring(0, 8).ToUpper))
				Next
				m_namesAndHashes = temp
			End If
			Return m_namesAndHashes
		End Get
	End Property



	Public ReadOnly Property AllColumns As List(Of CColumn)
        Get
            Dim temp As New List(Of CColumn)
            For Each i As CViewInfo In Me
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
    Public Function Has(name As String) As Boolean
        Return IndexByName.ContainsKey(name.ToLower.Replace("dbo.", ""))
    End Function
    Public Function GetByName(name As String) As CViewInfo
        Dim pk As CViewInfo = Nothing
        IndexByName.TryGetValue(name.ToLower.Replace("dbo.", ""), pk)
        Return pk
    End Function
    Public Function GetByHash(md5 As Guid) As CViewInfo
        Dim pk As CViewInfo = Nothing
        Index.TryGetValue(md5, pk)
        Return pk
    End Function

    Private m_index As Dictionary(Of Guid, CViewInfo)
    Private m_indexByName As Dictionary(Of String, CViewInfo)
    Public ReadOnly Property Index As Dictionary(Of Guid, CViewInfo)
        Get
            If IsNothing(m_index) Then
                Dim temp As New Dictionary(Of Guid, CViewInfo)
                For Each i As CViewInfo In Me
                    temp.Add(i.MD5, i)
                Next
                m_index = temp
            End If
            Return m_index
        End Get
    End Property
    Public ReadOnly Property IndexByName As Dictionary(Of String, CViewInfo)
        Get
            If IsNothing(m_indexByName) OrElse Me.Count <> m_indexByName.Count Then
                Dim temp As New Dictionary(Of String, CViewInfo)
                For Each i As CViewInfo In Me
                    temp.Add(i.ViewName.ToLower.Replace("dbo.", ""), i)
                Next
                m_indexByName = temp
            End If
            Return m_indexByName
        End Get
    End Property



    'Diff
    Public Function Diff(ref As CViewInfoList) As CViewListDiff
        Dim d As New CViewListDiff
        Diff(ref, d.Missing, d.Added, d.Same, d.Different)
        Return d
    End Function

    Public Sub Diff(ref As CViewInfoList, ByRef missing As CViewInfoList, ByRef added As CViewInfoList, ByRef same As CViewInfoList, ByRef diff As List(Of CViewDiff))
        For Each i As String In Me.IndexByName.Keys
            Dim info As CViewInfo = Me.IndexByName(i)
            Dim refInfo As CViewInfo = Nothing

            If Not ref.IndexByName.TryGetValue(i, refInfo) Then
                added.Add(info)   'Name not recog
            Else
                If refInfo.MD5 = info.MD5 Then
                    same.Add(info)  'Name+Nature => Exact match
                Else
                    diff.Add(New CViewDiff(info, refInfo))  'Nature is different
                End If
            End If
        Next
        For Each i As String In ref.IndexByName.Keys
            Dim refInfo As CViewInfo = ref.IndexByName(i)
            If Not Me.IndexByName.ContainsKey(i) Then
                missing.Add(refInfo)   'Name not recog
            End If
        Next
    End Sub
End Class