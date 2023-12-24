Partial Public Class CSqlServerConnection : Implements IComparable(Of CSqlServerConnection)


#Region "Equals"
    Public Overrides Function GetHashCode() As Integer
        Return String.Concat(Server, Database, UserName, Password, WindowsAuthentication).GetHashCode()
    End Function
    Public Function CompareTo(ByVal other As CSqlServerConnection) As Integer Implements System.IComparable(Of CSqlServerConnection).CompareTo
        Dim i As Integer = String.Compare(Server, other.Server, True)
        If i = 0 Then i = Database.ToLower.CompareTo(other.Database.ToLower)
        If i = 0 Then i = UserName.ToLower.CompareTo(other.UserName.ToLower)
        If i = 0 Then i = Password.ToLower.CompareTo(other.Password.ToLower)
        Return i
    End Function
    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        If Not TypeOf (obj) Is CSqlServerConnection Then Return False
        With CType(obj, CSqlServerConnection)
            If .WindowsAuthentication <> Me.WindowsAuthentication Then Return False
            If Not String.Equals(.Server, Me.Server, StringComparison.CurrentCultureIgnoreCase) Then Return False
            If Not String.Equals(.Database, Me.Database, StringComparison.CurrentCultureIgnoreCase) Then Return False
            If Not String.Equals(.UserName, Me.UserName, StringComparison.CurrentCultureIgnoreCase) Then Return False
            If Not String.Equals(.Password, Me.Password, StringComparison.CurrentCultureIgnoreCase) Then Return False
            Return True
        End With
    End Function
#End Region

End Class
