'Provider for the remote driver CWebSrcBinary
Partial Class webservices_DataSrc : Inherits CWebPage

    Public Overloads Overrides Sub ClearCache(ByVal tableName As String)
        ClearCache() 'Can be more specific when schema project is in scope
    End Sub
End Class
