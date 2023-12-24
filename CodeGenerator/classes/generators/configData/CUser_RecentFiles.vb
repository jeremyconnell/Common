Imports System.Configuration

Public Class CUser_RecentFiles : Inherits ApplicationSettingsBase

#Region "Constants"
    Private Const OLD_FILE_NAME_XML As String = "C:\\Program Files\\Picasso.net.nz\\ClassGenenerator - XSD\\recent.txt"
#End Region

#Region "Members"
    Private Shared m_singleton As CUser_RecentFiles
#End Region

#Region "Shared"
    Public Shared Property Files() As List(Of String)
        Get
            Return _Singleton.RecentFiles
        End Get
        Set(ByVal value As List(Of String))
            _Singleton.RecentFiles = value
        End Set
    End Property
    Private Shared ReadOnly Property _Singleton() As CUser_RecentFiles
        Get
            If IsNothing(m_singleton) Then
                SyncLock (GetType(CUser_RecentFiles))
                    If IsNothing(m_singleton) Then
                        m_singleton = New CUser_RecentFiles()
                    End If
                End SyncLock
            End If
            Return m_singleton
        End Get
    End Property
#End Region

#Region "Settings"
    <UserScopedSetting()> _
    <SettingsSerializeAs(System.Configuration.SettingsSerializeAs.Binary)> _
    Public Property RecentFiles() As List(Of String)
        Get
            Dim list As List(Of String) = CType(Me("RecentFiles"), List(Of String))

            'Backwards compat
            If IsNothing(list) Then
                Try
                    If IO.File.Exists(OLD_FILE_NAME_XML) Then
                        list = New List(Of String)(IO.File.ReadAllLines(OLD_FILE_NAME_XML))
                    End If
                Catch
                End Try
                If IsNothing(list) Then list = New List(Of String)

                Me.RecentFiles = list
            End If

            Return list
        End Get
        Set(ByVal value As List(Of String))
            Me("RecentFiles") = value
            Me.Save()
        End Set
    End Property
#End Region




End Class
