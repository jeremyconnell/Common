Partial Public Class CMySqlConnection : Implements IComparable(Of CMySqlConnection)

#Region "Equals"
    Public Overrides Function GetHashCode() As Integer
        Return String.Concat(Server, Database, UserName, Password, Port).GetHashCode()
    End Function
    Public Function CompareTo(ByVal other As CMySqlConnection) As Integer Implements System.IComparable(Of CMySqlConnection).CompareTo
        Dim i As Integer = Server.ToLower.CompareTo(other.Server.ToLower)
        If i = 0 Then i = Database.ToLower.CompareTo(other.Database.ToLower)
        If i = 0 Then i = UserName.ToLower.CompareTo(other.UserName.ToLower)
        If i = 0 Then i = Password.ToLower.CompareTo(other.Password.ToLower)
        If i = 0 Then i = Port.CompareTo(other.Port)
        Return i
    End Function
    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If Not TypeOf (obj) Is CMySqlConnection Then Return False
        With CType(obj, CMySqlConnection)
            If Not String.Equals(.Server, Me.Server, StringComparison.CurrentCultureIgnoreCase) Then Return False
            If Not String.Equals(.Database, Me.Database, StringComparison.CurrentCultureIgnoreCase) Then Return False
            If Not String.Equals(.UserName, Me.UserName, StringComparison.CurrentCultureIgnoreCase) Then Return False
            If Not String.Equals(.Password, Me.Password, StringComparison.CurrentCultureIgnoreCase) Then Return False
            If .Port <> Me.Port Then Return False
            Return True
        End With
    End Function
#End Region

End Class
