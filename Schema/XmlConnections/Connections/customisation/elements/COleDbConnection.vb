Partial Public Class COleDbConnection

#Region "Equals"
    Public Overrides Function GetHashCode() As Integer
        Return Me.ConnectionString.GetHashCode()
    End Function
    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If Not TypeOf (obj) Is COleDbConnection Then Return False
        With CType(obj, COleDbConnection)
            Return String.Equals(.ConnectionString, Me.ConnectionString, StringComparison.CurrentCultureIgnoreCase)
        End With
    End Function
#End Region

End Class
