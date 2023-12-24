Imports System.Reflection

Public Class CReflection

#Region "Public"
    Public Shared Function HaveSameValue(ByVal list As IList, ByVal propertyName As String) As Boolean
        If list.Count = 0 Then Return False
        Dim pi As PropertyInfo = GetPropertyInfo(list, propertyName)
        Dim first As Object = pi.GetValue(list(0), Nothing)
        For Each i As Object In list
            Dim compareWith As Object = pi.GetValue(i, Nothing)
            If IsNothing(first) Then
                If IsNothing(compareWith) Then Continue For Else Return False
            End If
            If IsNothing(compareWith) Then Return False
            If Not compareWith.Equals(first) Then Return False
        Next
        Return True
    End Function
    Public Shared Sub SetSameValue(ByVal list As IList, ByVal propertyName As String, ByVal value As Object)
        If list.Count = 0 Then Return
        Dim pi As PropertyInfo = GetPropertyInfo(list, propertyName)
        For Each i As Object In list
            pi.SetValue(i, value, Nothing)
        Next
    End Sub

    Public Class GenericSortBy : Implements IComparer
        'Constructor
        Public Sub New(ByVal propertyName As String, ByVal descending As Boolean, ByVal list As IList)
            Me.New(propertyName, descending, GetTypeFromList(list))
        End Sub
        Public Sub New(ByVal propertyName As String, ByVal descending As Boolean, ByVal type As Type)
            _descending = descending
            If IsNothing(type) Then Exit Sub
            _pi = type.GetProperty(propertyName)
            If IsNothing(_pi) Then Throw New Exception([String].Concat("'", propertyName, "' is not a property of class '", type, "' (Property names are case-sensitive)"))
        End Sub

        'Members
        Private _pi As System.Reflection.PropertyInfo
        Private _descending As Boolean

        'Public
        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
            If _descending Then
                Dim temp As Object = x
                x = y
                y = temp
            End If

            Dim px As Object = _pi.GetValue(x, Nothing)
            Dim py As Object = _pi.GetValue(y, Nothing)

            If IsNothing(px) Then
                If IsNothing(py) Then Return 0 Else Return -1
            Else
                If IsNothing(py) Then Return 1
            End If

            If _pi.PropertyType.Equals(GetType(String)) Then Return DirectCast(px, String).CompareTo(DirectCast(py, String))
            If _pi.PropertyType.Equals(GetType(Integer)) Then Return DirectCast(px, Integer).CompareTo(DirectCast(py, Integer))
            If _pi.PropertyType.Equals(GetType(Long)) Then Return DirectCast(px, Long).CompareTo(DirectCast(py, Long))
            If _pi.PropertyType.Equals(GetType(Double)) Then Return DirectCast(px, Double).CompareTo(DirectCast(py, Double))
            If _pi.PropertyType.Equals(GetType(Decimal)) Then Return DirectCast(px, Decimal).CompareTo(DirectCast(py, Decimal))
            If _pi.PropertyType.Equals(GetType(Boolean)) Then Return DirectCast(px, Boolean).CompareTo(DirectCast(py, Boolean))
            If _pi.PropertyType.Equals(GetType(DateTime)) Then Return DirectCast(px, DateTime).CompareTo(DirectCast(py, DateTime))
            If _pi.PropertyType.Equals(GetType(TimeSpan)) Then Return DirectCast(px, TimeSpan).CompareTo(DirectCast(py, TimeSpan))

            Try
                If _pi.PropertyType.Equals(GetType(Integer?)) Then Return DirectCast(px, Integer?).Value.CompareTo(DirectCast(py, Integer?).Value)
                If _pi.PropertyType.Equals(GetType(Long?)) Then Return DirectCast(px, Long?).Value.CompareTo(DirectCast(py, Long?).Value)
                If _pi.PropertyType.Equals(GetType(Double?)) Then Return DirectCast(px, Double?).Value.CompareTo(DirectCast(py, Double?).Value)
                If _pi.PropertyType.Equals(GetType(Decimal?)) Then Return DirectCast(px, Decimal?).Value.CompareTo(DirectCast(py, Decimal?).Value)
                If _pi.PropertyType.Equals(GetType(Boolean?)) Then Return DirectCast(px, Boolean?).Value.CompareTo(DirectCast(py, Boolean?).Value)
            Catch
            End Try

            Return 0
        End Function
    End Class
#End Region

#Region "Private"
    Private Shared Function GetTypeFromList(ByVal list As IList) As Type
        If list.Count = 0 Then Return Nothing
        Return list(0).GetType()
    End Function
    Private Shared Function GetPropertyInfo(ByVal list As IList, ByVal propertyName As String) As PropertyInfo
        Dim firstObject As Object = list(0)
        Dim type As Type = firstObject.GetType
        Dim pi As PropertyInfo = type.GetProperty(propertyName)
        If IsNothing(pi) Then Throw New Exception(String.Concat("'", propertyName, "' is not a property of class '", type, "' (Property names are case-sensitive)"))
        Return pi
    End Function
#End Region

End Class
