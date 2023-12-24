Imports System.Configuration

Public Class CUser_Templates : Inherits ApplicationSettingsBase

#Region "Members"
    Private Shared m_singleton As CUser_Templates
#End Region

#Region "Shared"
    Public Shared Sub SaveSkins()
        Skins_ = Skins_
    End Sub
    Public Shared Property Skins_() As CSkinList
        Get
            Return _Singleton.Skins
        End Get
        Set(ByVal value As CSkinList)
            _Singleton.Skins = value
        End Set
    End Property
    Public Shared Property CurrentSkinId_() As Guid
        Get
            Return _Singleton.CurrentSkinId
        End Get
        Set(ByVal value As Guid)
            _Singleton.CurrentSkinId = value
        End Set
    End Property
    Private Shared ReadOnly Property _Singleton() As CUser_Templates
        Get
            If IsNothing(m_singleton) Then
                SyncLock (GetType(CUser_Templates))
                    If IsNothing(m_singleton) Then
                        m_singleton = New CUser_Templates()
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
    Public Property Skins() As CSkinList
        Get
            Dim list As CSkinList = CType(Me("Skins"), CSkinList)

            'Backwards compat
            If IsNothing(list) Then
                list = New CSkinList()
                Me.Skins = list
            End If

            Return list
        End Get
        Set(ByVal value As CSkinList)
            Me("Skins") = value
            Me.Save()
        End Set
    End Property
    <UserScopedSetting()> _
    <SettingsSerializeAs(System.Configuration.SettingsSerializeAs.Binary)> _
    Public Property CurrentSkinId() As Guid
        Get
            Return CType(Me("CurrentSkinId"), Guid)
        End Get
        Set(ByVal value As Guid)
            Me("CurrentSkinId") = value
            Me.Save()
        End Set
    End Property
#End Region

#Region "Templates"
    Public Shared Sub UpdateFromNetwork()
        If SkinsAndDefault.UpdateFromNetwork Then Skins_ = Skins_
    End Sub
    Public Shared ReadOnly Property SkinsAndDefault() As CSkinList
        Get
            Return Skins_.AndDefault
        End Get
    End Property
    Public Shared Function GetTemplate(ByVal relativePath As String) As String
        Return CurrentSkin.GetTemplate(relativePath)
    End Function
    Public Shared Sub RemoveSkin()
        RemoveSkin(CurrentSkin)
    End Sub
    Public Shared Sub RemoveSkin(ByVal skin As CSkin)
        Skins_.Remove(skin)
        SaveSkins()
    End Sub
    Public Shared Sub AddSkin(ByVal name As String, ByVal description As String)
        CurrentSkin = Skins_.Add(name, description)
    End Sub
    Public Shared Function AddSkin(ByVal url As String) As Boolean
        Dim s As CSkin = Skins_.Add(url)
        If IsNothing(s) Then Return False
        CurrentSkin = s
        Return True
    End Function
    Public Shared Property CurrentSkin() As CSkin
        Get
            With _Singleton
                Dim s As CSkin = SkinsAndDefault.GetById(CurrentSkinId_)
                If IsNothing(s) Then Return CSkin.Default
                Return s
            End With
        End Get
        Set(ByVal value As CSkin)
            CurrentSkinId_ = value.Id
        End Set
    End Property
#End Region

End Class
