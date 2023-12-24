Imports System.Text

<Serializable>
<CLSCompliant(True)>
Public Class CFilesList : Inherits List(Of CFileNameAndContent)

#Region "Constructors"
    Public Sub New()
    End Sub
    Public Sub New(count As Integer)
        MyBase.New(count)
    End Sub
    Public Sub New(list As IList(Of CFileNameAndContent))
        MyBase.New(list)
    End Sub


    Public Sub New(ByVal folderPath As String, recursive As Boolean, ByVal ignoreExt As String)
        Me.New(GetFiles(folderPath, recursive), CUtilities.StringToListStr(ignoreExt).ToArray(), folderPath)
    End Sub

    Public Sub New(ByVal folderPath As String, Optional ByVal exceptExtensions As String() = Nothing, Optional ByVal recursive As Boolean = False)
        Me.New(GetFiles(folderPath, recursive), exceptExtensions, folderPath)
    End Sub

    Public Sub New(ByVal files As String(), Optional ByVal exceptExtensions As String() = Nothing, Optional ByVal baseFolder As String = Nothing)
        MyBase.New(files.Length)

        If IsNothing(exceptExtensions) Then exceptExtensions = CFolderHash.IGNORE_DLL_

        If Not IsNothing(baseFolder) Then
            If Not baseFolder.EndsWith("/") AndAlso Not baseFolder.EndsWith("\") Then baseFolder &= "\"
        End If

        'Hash the files
        For Each i As String In files
            i = i.ToLower

            If i.Contains("vshost.") Then Exit Sub
            If EndsWith(i, exceptExtensions) Then Exit Sub

            Dim f As New CFileNameAndContent(i, IO.File.ReadAllBytes(i), New IO.FileInfo(i).LastWriteTimeUtc)
            Add(f)

            'Add any parent subfolders to filenames 
            If Not IsNothing(baseFolder) Then
                Dim thisFolder As String = String.Concat(IO.Path.GetDirectoryName(i), "\")
                If thisFolder.Length > baseFolder.Length Then
                    Dim subfolder As String = thisFolder.Substring(baseFolder.Length)
                    f.Name = String.Concat(subfolder, f.Name)
                End If
            End If
        Next
    End Sub
#End Region


#Region "Index On Name"
    Public Function Has(key As String) As Boolean
        Return Dict.ContainsKey(key.ToLower)
    End Function
    Public Function Has(key As Guid) As Boolean
        Return DictByHash.ContainsKey(key)
    End Function

    Public Function GetFile(key As String) As CFileNameAndContent
        Dim c As CFileNameAndContent = Nothing
        Dict.TryGetValue(key.ToLower, c)
        Return c
    End Function
    Public Function GetFile(md5 As Guid) As CFileNameAndContent
        Dim c As CFileNameAndContent = Nothing
        DictByHash.TryGetValue(md5, c)
        Return c
    End Function

    <NonSerialized()> Private m_dict As Dictionary(Of String, CFileNameAndContent)
    Public ReadOnly Property Dict As Dictionary(Of String, CFileNameAndContent)
        Get
            If IsNothing(m_dict) OrElse m_dict.Count <> Me.Count Then
                m_dict = New Dictionary(Of String, CFileNameAndContent)(Me.Count)
                For Each i As CFileNameAndContent In Me
                    m_dict(i.Name.ToLower) = i
                Next
            End If
            Return m_dict
        End Get
    End Property

    <NonSerialized()> Private m_dictByHash As Dictionary(Of Guid, CFileNameAndContent)
    Public ReadOnly Property DictByHash As Dictionary(Of Guid, CFileNameAndContent)
        Get
            If IsNothing(m_dictByHash) OrElse m_dictByHash.Count <> Me.Count Then
                m_dictByHash = New Dictionary(Of Guid, CFileNameAndContent)(Me.Count)
                For Each i As CFileNameAndContent In Me
                    m_dictByHash(i.Md5) = i
                Next
            End If
            Return m_dictByHash
        End Get
    End Property

    Public ReadOnly Property Names As List(Of String)
        Get
            Dim list As New List(Of String)(Dict.Keys)
            list.Sort()
            Return list
        End Get
    End Property
#End Region



    Public Sub Pack()
        Dim first As New List(Of Guid)(Me.Count)
        For Each i As CFileNameAndContent In Me
            If Not first.Contains(i.Md5) Then
                first.Add(i.Md5)
            ElseIf Not i.Name.StartsWith(":") Then
                i.Content = Nothing 'Compression
                i.Name = ":" & i.Name 'prevent accidental delete
            End If
        Next
    End Sub
    Public Sub Unpack()
        Dim dict As New Dictionary(Of Guid, Byte())(Me.Count)
        For Each i As CFileNameAndContent In Me
            If Not dict.ContainsKey(i.Md5) Then
                dict.Add(i.Md5, i.Content)
            ElseIf i.Name.StartsWith(":") Then
                i.Content = dict(i.Md5)
                While i.Name.StartsWith(":")
                    i.Name = i.Name.Substring(1)
                End While
            End If
        Next
    End Sub



    Public Overrides Function GetHashCode() As Integer
        Return ToString.GetHashCode()
    End Function
    Public Overrides Function ToString() As String
        Dim sb As New StringBuilder()
        For Each j As CFileNameAndContent In Me
            If IsNothing(j.Content) Then
                sb.AppendLine("Delete: " + j.Name)
            Else
                sb.AppendLine("Update: " + CUtilities.FileNameAndSize(j.Name, j.Content))
            End If
        Next
        Return sb.ToString()
    End Function


    Private Shared Function EndsWith(s As String, ss As String()) As Boolean
        For Each i As String In ss
            If s.EndsWith(i) Then Return True
        Next
        Return False
    End Function
    Private Shared Function GetFiles(ByVal folderPath As String, ByVal recursive As Boolean) As String()
        If Not IO.Directory.Exists(folderPath) Then Return New String() {} 'Suppress exceptions
        If Not recursive Then Return IO.Directory.GetFiles(folderPath)

        Dim list As New List(Of String)
        list.AddRange(GetFiles(folderPath, False))
        For Each i As String In IO.Directory.GetDirectories(folderPath)
            list.AddRange(GetFiles(i, True))
        Next
        Return list.ToArray
    End Function

    Public ReadOnly Property Total As Long
        Get
            Dim t As Long = 0
            For Each i As CFileNameAndContent In Me
                If Not IsNothing(i.Content) Then
                    t += i.Content.Length
                End If
            Next
            Return t
        End Get
    End Property
    Public Function TotalFor(list As List(Of String)) As Long

        Dim t As Long = 0
        For Each i As CFileNameAndContent In Me
            If list.Contains(i.Name) AndAlso Not IsNothing(i.Content) Then t += i.Content.Length
        Next
        Return t

    End Function
End Class
