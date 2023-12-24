Partial Public Class COracleConnection : Implements IComparable(Of COracleConnection)

#Region "Equals"
    Public Overrides Function GetHashCode() As Integer
        Return String.Concat(Server, UserName, Password).GetHashCode()
    End Function
    Public Function CompareTo(ByVal other As COracleConnection) As Integer Implements System.IComparable(Of COracleConnection).CompareTo
        Dim i As Integer = Server.ToLower.CompareTo(other.Server.ToLower)
        If i = 0 Then i = UserName.ToLower.CompareTo(other.UserName.ToLower)
        If i = 0 Then i = Password.ToLower.CompareTo(other.Password.ToLower)
        Return i
    End Function
    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If Not TypeOf (obj) Is COracleConnection Then Return False
        With CType(obj, COracleConnection)
            If Not String.Equals(.Server, Me.Server, StringComparison.CurrentCultureIgnoreCase) Then Return False
            If Not String.Equals(.UserName, Me.UserName, StringComparison.CurrentCultureIgnoreCase) Then Return False
            If Not String.Equals(.Password, Me.Password, StringComparison.CurrentCultureIgnoreCase) Then Return False
            Return True
        End With
    End Function
#End Region

End Class
