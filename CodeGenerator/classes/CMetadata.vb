Imports System.Collections.Generic

Public Class CMetadata : Inherits List(Of CTable)

#Region "Constructor"
    Public Sub New(ByVal folderPath As String, ByVal dataSrc As CDataSrcLocal)
        m_folderPath = folderPath


        If String.IsNullOrEmpty(folderPath) Then Exit Sub
        If Not IO.Directory.Exists(SubFolder) Then Exit Sub

        Try
            For Each i As String In IO.Directory.GetDirectories(SubFolder)
                Dim files As String() = IO.Directory.GetFiles(i, "*.regenerated.*")
                If files.Length < 2 Then Continue For
                Me.Add(New CTable(files(0), dataSrc))
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Failed to view existing classes", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Protected Sub New(ByVal m As CMetadata)
        MyBase.New(m)
    End Sub
#End Region

#Region "Members"
    Private m_folderPath As String
#End Region

#Region "Properties"
    Public ReadOnly Property FolderPath() As String
        Get
            Return m_folderPath
        End Get
    End Property
    Public ReadOnly Property SubFolder() As String
        Get
            Return String.Concat(FolderPath, "/tables")
        End Get
    End Property
#End Region

#Region "GetByTableName"
    Public Function ContainsTable(ByVal tableName As String) As Boolean
        Return Not IsNothing(GetByTableName(tableName))
    End Function
    Public Function GetByTableName(ByVal tableName As String) As CTable
        tableName = tableName.ToLower.Replace("[", "").Replace("]", "")
        For Each i As CTable In Me
            If i.TableName.ToLower.Replace("[", "").Replace("]", "").Equals(tableName) Then Return i
        Next
        Return Nothing
    End Function
    Public Function GetByClassName(ByVal className As String) As CTable
        className = className.ToLower
        For Each i As CTable In Me
            If i.ClassName.ToLower.Equals(className) Then Return i
        Next
        Return Nothing
    End Function
    Public Function Clone() As CMetadata
        Return New CMetadata(Me)
    End Function
#End Region

End Class
