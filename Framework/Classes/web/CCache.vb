Imports System.Web
Imports System.Web.Caching

Public Class CCache

#Region "Public"
    'Simple Getter
    Public Shared Function [Get](ByVal key As String) As Object
        If Not CApplication.IsWebApplication Then Return CApplication.Get(key)
        Return HttpContext.Current.Cache(key)
    End Function

    'Set (no-expire or 3-hr expire)
    Public Shared Sub [Set](ByVal key As String, ByVal value As Object)
        [Set](key, value, CConfigBase.CacheTimeoutDefault) 'Defaults to 3hrs
    End Sub

    'Specific Sliding Expiry
    Public Shared Sub [Set](ByVal key As String, ByVal value As Object, ByVal hours As Integer, Optional ByVal minutes As Integer = 0)
        Dim specificLifeTime As New TimeSpan(hours, minutes, 0)
        [Set](key, value, specificLifeTime)
    End Sub
    Public Shared Sub [Set](ByVal key As String, ByVal value As Object, ByVal lifeTime As TimeSpan)
        If CApplication.IsWebApplication Then
            HttpContext.Current.Cache.Remove(key)
            If IsNothing(value) Then Exit Sub
            If IsNothing(lifeTime) Then
                HttpContext.Current.Cache(key) = value
            Else
                HttpContext.Current.Cache.Add(key, value, Nothing, Cache.NoAbsoluteExpiration, lifeTime, CacheItemPriority.Default, Nothing)
            End If
        Else
            CApplication.Set(key, value)
        End If
    End Sub
    Public Shared Sub ClearAll()
        CApplication.ClearAll()
    End Sub
#End Region

End Class

