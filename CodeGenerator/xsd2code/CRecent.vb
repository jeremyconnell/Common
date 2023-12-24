Public Class CRecent
    'Public - Update History
    Public Shared Function Add(ByVal filePath As String) As Boolean
        Dim list As List(Of String) = CUser_RecentFiles.Files
        If list.Contains(filePath) Then
            list.Remove(filePath)
            list.Add(filePath)
            CUser_RecentFiles.Files = list
            Return False
        End If
        list.Add(filePath)
        CUser_RecentFiles.Files = list
        Return True
    End Function
    Public Shared Sub Remove(ByVal filePath As String)
        Dim list As List(Of String) = CUser_RecentFiles.Files
        If list.Contains(filePath) Then list.Remove(filePath)
        CUser_RecentFiles.Files = list
    End Sub

    'Public - View History
    Public Shared ReadOnly Property RecentFiles_ByMostRecent() As List(Of String)
        Get
            Dim list As New List(Of String)(CUser_RecentFiles.Files)
            list.Reverse()
            Return list
        End Get
    End Property
    Public Shared ReadOnly Property RecentFiles_ByFileName() As List(Of String)
        Get
            Dim list As New List(Of String)(CUser_RecentFiles.Files)
            list.Sort(New CSortByFileName)
            Return list
        End Get
    End Property
    Public Shared ReadOnly Property RecentFiles_ByFolder() As List(Of String)
        Get
            Dim list As New List(Of String)(CUser_RecentFiles.Files)
            list.Sort(New CSortByFolder)
            Return list
        End Get
    End Property


    'Private - Sorting
    Private Class CSortByFileName : Implements IComparer(Of String)
        Public Function Compare(ByVal x As String, ByVal y As String) As Integer Implements System.Collections.Generic.IComparer(Of String).Compare
            Return IO.Path.GetFileName(x).ToLower.CompareTo(IO.Path.GetFileName(y).ToLower)
        End Function
    End Class
    Private Class CSortByFolder : Implements IComparer(Of String)
        Public Function Compare(ByVal x As String, ByVal y As String) As Integer Implements System.Collections.Generic.IComparer(Of String).Compare
            Return IO.Path.GetDirectoryName(x).ToLower.CompareTo(IO.Path.GetDirectoryName(y).ToLower)
        End Function
    End Class

End Class
