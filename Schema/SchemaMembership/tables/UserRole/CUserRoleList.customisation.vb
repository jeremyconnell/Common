Imports System
Imports System.Data
Imports System.Collections.Generic

Imports Framework

'Collection Class (Customisable half)
Partial Public Class CUserRoleList

#Region "Members"
    Private m_roles As CRoleList
    Private m_roleNames As List(Of String)
#End Region

#Region "Resolve Associative table (and sort)"
    Public ReadOnly Property [Roles]() As CRoleList
        Get
            If m_roles Is Nothing Then
                SyncLock (Me)
                    If m_roles Is Nothing Then
                        m_roles = New CRoleList(Me.Count)
                        For Each i As CUserRole In Me
                            If IsNothing(i.Role) Then Continue For
                            m_roles.Add(i.Role)
                        Next
                        m_roles.Sort()
                    End If
                End SyncLock
            End If
            Return m_roles
        End Get
    End Property
    Public Function RemainingRoles(ByVal search As String) As CRoleList
        Dim temp As New CRoleList(CRole.Cache.Search(search))
        temp.Remove(Me.Roles)
        Return temp
    End Function
#End Region

#Region "Resolve/Isolate PKs"
    Public ReadOnly Property RoleNames() As List(Of String)
        Get
            If IsNothing(m_roleNames) OrElse m_roleNames.Count <> Me.Count Then
                SyncLock (Me)
                    If IsNothing(m_roleNames) OrElse m_roleNames.Count <> Me.Count Then
                        m_roleNames = New List(Of String)(Me.Count)
                        For Each i As CUserRole In Me
                            m_roleNames.Add(i.URRoleName)
                        Next
                    End If
                End SyncLock
            End If
            Return m_roleNames
        End Get
    End Property
#End Region

#Region "Preload Parent Objects"
    'Efficiency Adjustment: Preloads the common parent for the whole list, to avoid database chatter
    Public WriteOnly Property [User]() As CUser
        Set(ByVal Value As CUser)
            For Each i As CUserRole In Me
                i.User = Value
            Next
        End Set
    End Property
#End Region

#Region "Cloning"
    Public Function Clone(ByVal target As CDataSrc) As CUserRoleList ', parentId As Integer) As CUserRoleList
        'No Transaction
        If TypeOf target Is CDataSrcRemote Then Return Clone(target, Nothing) ', parentId)

        'Transaction
        Dim cn As IDbConnection = target.Local.Connection()
        Dim tx As IDbTransaction = cn.BeginTransaction()
        Try
            Clone = Clone(target, tx) ', parentId)
            tx.Commit()
        Catch
            tx.Rollback()
            Throw
        Finally
            cn.Close()
        End Try
    End Function
    Public Function Clone(ByVal target As CDataSrc, ByVal txOrNull As IDbTransaction) As CUserRoleList ', parentId As Integer) As CUserRoleList
        Dim list As New CUserRoleList(Me.Count)
        For Each i As CUserRole In Me
            list.Add(i.Clone(target, txOrNull))  ', parentId))    *Child entities must reference the new parent
        Next
        Return list
    End Function
#End Region

End Class
