Option Strict Off

Public Class CReader

    Public Shared EMPTY As Byte() = New Byte() {}

    Public Sub New(packed As Byte(), format As CSerialise)
        'First deserialise layer (to a list)
        m_data = format.Deserialise_(Of List(Of Byte()))(packed)
        m_index = 0
        m_format = format
    End Sub

    Private m_format As CSerialise
    Private m_data As List(Of Byte())
    Private m_index As Integer

    Public Function Unpack(Of T)() As T
        Return m_format.Deserialise_(Of T)(ReadNext)
    End Function

    Public Function Guid() As Guid
        Return Unpack(Of Guid)()
    End Function
    Public Function Int() As Integer
        Return Unpack(Of Integer)()
    End Function
    Public Function Str() As String
        Return Unpack(Of String)()
    End Function
    Public Function Bool() As Boolean
        Return Unpack(Of Boolean)()
    End Function
    Public Function Bytes() As Byte()
        Return ReadNext()
    End Function
    Public Function [Date]() As DateTime
        Return Unpack(Of DateTime)()
    End Function
    Public Function Dbl() As Double
        Return Unpack(Of Double)()
    End Function
    Public Function Ex() As CException
		Return Unpack(Of CException)()
	End Function
	Public Function Schema() As CSchemaInfo
		Return Unpack(Of CSchemaInfo)()
	End Function
	Public Function StrArray() As String()
        Return Unpack(Of String())()
    End Function
    Public Function StrList() As List(Of String)
        Return Unpack(Of List(Of String))()
    End Function
    Public Function GuidList() As List(Of Guid)
        Return Unpack(Of List(Of Guid))()
    End Function
    Public Function DictStrStr() As Dictionary(Of String, String)
        Return Unpack(Of Dictionary(Of String, String))()
    End Function


    Private Function ReadNext() As Byte()
        If IsNothing(m_data) OrElse m_index >= m_data.Count Then Return EMPTY
        ReadNext = m_data(m_index)
        m_index += 1
    End Function


    Public Shared Function Pack(params As Object(), format As CSerialise) As Byte()
        If params.Length = 0 Then Return EMPTY

        'First serialise individually
        Dim bin As New List(Of Byte())
        For Each i As Object In params
            If IsNothing(i) Then
                bin.Add(EMPTY)
            Else
                bin.Add(format.Serialise_(i))
            End If
        Next

        'Then serialise again as a list
        Return format.Serialise_(bin)
    End Function

End Class
