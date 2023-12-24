Imports System.Runtime.Serialization

#Region "Enums"
<DataContract>
Public Enum EGzip
    None = 0 'Default (can control from config or method)
    Input = 1
    Output = 2
    Both = 3
End Enum
#End Region

Public Class CGzip

#Region "Shared - Compress/Decompress"
    Public Shared Function Compress(input As Byte(), gzip As EGzip, isInput As Boolean) As Byte()
        Select Case gzip
            Case EGzip.Both : Return CBinary.Zip(input)
            Case EGzip.Input : If isInput Then Return CBinary.Zip(input)
            Case EGzip.Output : If Not isInput Then Return CBinary.Zip(input)
        End Select
        Return input
    End Function
    Public Shared Function Decompress(input As Byte(), gzip As EGzip, isInput As Boolean) As Byte()
        Select Case gzip
            Case EGzip.Both : Return CBinary.Unzip(input)
            Case EGzip.Input : If isInput Then Return CBinary.Unzip(input)
            Case EGzip.Output : If Not isInput Then Return CBinary.Unzip(input)
        End Select
        Return input
    End Function
#End Region

End Class
