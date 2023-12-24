Imports System.IO

Public Class UCHistory

    Public Enum ESortBy
        Recent
        FileName
        Folder
    End Enum


#Region "Events"
    Public Event Selected(ByVal filePath As String)
#End Region

#Region "Form"
    Public ReadOnly Property SortBy() As ESortBy
        Get
            If rbByFileName.Checked Then Return ESortBy.FileName
            If rbByFolder.Checked Then Return ESortBy.Folder
            Return ESortBy.Recent
        End Get
    End Property
    Public ReadOnly Property Recent() As List(Of String)
        Get
            Select Case SortBy
                Case ESortBy.FileName : Return CRecent.RecentFiles_ByFileName
                Case ESortBy.Folder : Return CRecent.RecentFiles_ByFolder
                Case ESortBy.Recent : Return CRecent.RecentFiles_ByMostRecent
                Case Else : Throw New Exception()
            End Select
        End Get
    End Property
#End Region

#Region "Event Handlers"
    Private Sub UCHistory_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Display()

        If ListView1.Items.Count > 0 Then ListView1.Items(0).Selected = True
    End Sub
    Private Sub rb_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbByFileName.CheckedChanged, rbRecent.CheckedChanged, rbByFolder.CheckedChanged
        Display()
    End Sub
    Private Sub ListView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged
        With ListView1.SelectedItems
            If .Count = 0 Then Exit Sub
            RaiseEvent Selected(CStr(.Item(0).Tag))
        End With
    End Sub
    Private Sub ContextMenuStrip1_ContextMenuChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ContextMenuStrip1.Opened
        miRemove.Enabled = ListView1.SelectedIndices.Count > 0
    End Sub
    Private Sub miRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles miRemove.Click
        Dim filePath As String = CStr(ListView1.SelectedItems(0).Tag)
        CRecent.Remove(filePath)
        Display()
    End Sub
#End Region

#Region "Private"
    Public Sub Display()
        With ListView1.Items
            .Clear()

            For Each i As String In Recent
                Dim li As New ListViewItem(Path.GetFileName(i))
                li.SubItems.Add(Path.GetDirectoryName(i))
                li.Tag = i
                .Add(li)
            Next
        End With
    End Sub
#End Region

End Class
