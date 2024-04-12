Imports ProtoBuf
Imports System.Runtime.Serialization

<DataContract, ProtoContract>
Public Class CProcedureList : Inherits List(Of CProcedure)
    <DataMember(Order:=1)> Public MD5 As Guid

    'Preconstructor
    Shared Sub New()
        CProto.Prepare(Of CViewInfo)()
    End Sub

    'Constructor
    Friend Sub New()

    End Sub
    Public Sub New(db As CDataSrc)
        Me.New(db.StoredProcs, db.Functions)
    End Sub
    Public Sub New(procs As Dictionary(Of String, String), funcs As Dictionary(Of String, String))
        For Each i As String In procs.Keys
            Me.Add(New CProcedure(i, True, procs(i)))
        Next
        For Each i As String In funcs.Keys
            Me.Add(New CProcedure(i, False, funcs(i)))
        Next
        SetHash()
    End Sub
    Public Sub New(isProc As Boolean, data As Dictionary(Of String, String))
        For Each i As String In data.Keys
            Me.Add(New CProcedure(i, isProc, data(i)))
        Next
        SetHash()
    End Sub
    Private Sub SetHash()
        Me.Sort()

        Dim list As New List(Of Guid)
        For Each i As CProcedure In Me
            list.Add(i.MD5)
        Next
        Me.MD5 = CBinary.MD5_(list)
    End Sub


    Public ReadOnly Property StoredProcs As CProcedureList
        Get
            Return GetByIsProc(True)
        End Get
    End Property
    Public ReadOnly Property Functions As CProcedureList
        Get
            Return GetByIsProc(False)
        End Get
    End Property


    Public Function Has(name As String) As Boolean
        name = name.ToLower().Replace("[", "").Replace("]", "").Replace("dbo.", "")
        Return IndexByName.ContainsKey(name)
    End Function
    Public Function GetByName(name As String) As CProcedure
        name = name.ToLower().Replace("[", "").Replace("]", "").Replace("dbo.", "")
        Dim pk As CProcedure = Nothing
        IndexByName.TryGetValue(name, pk)
        Return pk
    End Function
    Public Function GetByHash(md5 As Guid) As CProcedure
        Dim pk As CProcedure = Nothing
        Index.TryGetValue(md5, pk)
        Return pk
    End Function
    Public Function GetByIsProc(isProc As Boolean) As CProcedureList
        Dim pk As CProcedureList = Nothing
        IndexByIsProc.TryGetValue(isProc, pk)
        Return pk
    End Function

    Private m_namesAndHashes As List(Of String)
    Private m_index As Dictionary(Of Guid, CProcedure)
    Private m_indexByName As Dictionary(Of String, CProcedure)
    Private m_indexByIsProc As Dictionary(Of Boolean, CProcedureList)
    Public ReadOnly Property IndexByIsProc As Dictionary(Of Boolean, CProcedureList)
        Get
            If IsNothing(m_indexByIsProc) Then
                Dim isProc As New CProcedureList
                Dim isFunc As New CProcedureList
                For Each i As CProcedure In Me
                    If i.IsStoredProc Then
                        isProc.Add(i)
                    Else
                        isFunc.Add(i)
                    End If
                Next

                Dim temp As New Dictionary(Of Boolean, CProcedureList)
                temp.Add(True, isProc)
                temp.Add(False, isFunc)

                m_indexByIsProc = temp
            End If
            Return m_indexByIsProc
        End Get
    End Property
    Public ReadOnly Property Index As Dictionary(Of Guid, CProcedure)
        Get
            If IsNothing(m_index) Then
                Dim temp As New Dictionary(Of Guid, CProcedure)
                For Each i As CProcedure In Me
                    temp.Add(i.MD5, i)
                Next
                m_index = temp
            End If
            Return m_index
        End Get
    End Property
    Public ReadOnly Property IndexByName As Dictionary(Of String, CProcedure)
        Get
            If IsNothing(m_indexByName) OrElse Me.Count <> m_indexByName.Count Then
                Dim temp As New Dictionary(Of String, CProcedure)
                For Each i As CProcedure In Me
                    temp.Add(i.Name, i)
                Next
                m_indexByName = temp
            End If
            Return m_indexByName
        End Get
    End Property
    Public ReadOnly Property NamesAndHashes As List(Of String)
        Get
            If IsNothing(m_namesAndHashes) OrElse Me.Count <> m_namesAndHashes.Count Then
                Dim temp As New List(Of String)
                For Each i As CProcedure In Me
                    temp.Add(String.Concat(i.Name, IIf(i.IsStoredProc, " (proc) ", " (fn) "), CBinary.ToBase64(i.MD5).Substring(0, 8).ToUpper))
                Next
                m_namesAndHashes = temp
            End If
            Return m_namesAndHashes
        End Get
    End Property

    'Diff
    Public Function Diff(ref As CProcedureList) As CProcedureDiffList
        Dim d As New CProcedureDiffList
        Diff(ref, d.Missing, d.Added, d.Same, d.Different)
        Return d
    End Function

    Public Sub Diff(ref As CProcedureList, missing As CProcedureList, added As CProcedureList, same As CProcedureList, diff As List(Of CProcedureDiff))
        For Each i As String In Me.IndexByName.Keys
            Dim info As CProcedure = Me.IndexByName(i)
            Dim refInfo As CProcedure = Nothing

            If Not ref.IndexByName.TryGetValue(i, refInfo) Then
                added.Add(info)   'Name not recog
            Else
                If refInfo.MD5 = info.MD5 Then
                    same.Add(info)  'Name+Nature => Exact match
                Else
                    diff.Add(New CProcedureDiff(info, refInfo))  'Nature is different
                End If
            End If
        Next
        For Each i As String In ref.IndexByName.Keys
            Dim refInfo As CProcedure = ref.IndexByName(i)
            If Not Me.IndexByName.ContainsKey(i) Then
                missing.Add(refInfo)   'Name not recog
            End If
        Next
    End Sub
End Class