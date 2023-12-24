
Public Class CSortByColumn : Implements IComparer(Of CBaseSmart)
    'Constructor
    Public Sub New(ByVal columnName As String, ByVal descending As Boolean)
        m_columnName = columnName
        m_descending = descending
    End Sub

    'Members
    Protected m_columnName As String
    Protected m_descending As Boolean

    'Interface
    Public Function Compare(ByVal a As CBaseSmart, ByVal b As CBaseSmart) As Integer Implements IComparer(Of CBaseSmart).Compare
        If m_descending Then
            Dim temp As CBaseSmart = a
            a = b
            b = temp
        End If

        Dim objA As Object = a(m_columnName)
        Dim objB As Object = b(m_columnName)

        If IsNothing(objA) Then
            If IsNothing(objB) Then Return 0
            Return -1
        ElseIf IsNothing(objB) Then
            Return 1
        End If

        If TypeOf (objA) Is String Then Return CStr(objA).CompareTo(CStr(objB))
        If TypeOf (objA) Is Integer Then Return CInt(objA).CompareTo(objB)
        If TypeOf (objA) Is Double Then Return CDbl(objA).CompareTo(objB)
        If TypeOf (objA) Is Decimal Then Return CDec(objA).CompareTo(objB)
        If TypeOf (objA) Is Guid Then Return CType(objA, Guid).CompareTo(objB)
        If TypeOf (objA) Is Boolean Then Return CInt(IIf(CBool(objA) = CBool(objB), 0, IIf(CBool(objB), 1, -1)))
        Throw New Exception("CBase.CSortByKeyGeneric.Compare - Unhandled Type: " & objA.GetType.ToString)
    End Function
End Class