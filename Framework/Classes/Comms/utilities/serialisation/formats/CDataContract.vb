Option Strict Off
Imports System.Runtime.Serialization

Public Class CDataContract : Inherits CSerialise

#Region "Shared"
    Public Overloads Shared Function Serialise(obj As Object) As Byte()
        If IsNothing(obj) Then Return CReader.EMPTY 'Special case: Nulls
        If TypeOf obj Is Byte() Then Return obj 'Trivial case: binary data

        'Hack to suppress enums
        If TypeOf obj Is Object() Then obj = RemoveEnums(obj)

        Using ms As New IO.MemoryStream
            Dim ds As New DataContractSerializer(obj.GetType)
            Dim binFormat As Xml.XmlDictionaryWriter = Xml.XmlDictionaryWriter.CreateBinaryWriter(ms)
            ds.WriteObject(binFormat, obj)
            binFormat.Flush()
            Return ms.ToArray
        End Using
    End Function
    Public Overloads Shared Function Deserialise(t As Type, data As Byte()) As Object
        If IsNothing(data) Then Return Nothing 'Trivial case: Null data
        If data.Length = 0 Then Return Nothing 'Special case: Null-equiv value

        Using ms As New IO.MemoryStream(data)
            Dim ds As New DataContractSerializer(t)
            Dim q As New Xml.XmlDictionaryReaderQuotas()
            Dim binFormat As Xml.XmlDictionaryReader = Xml.XmlDictionaryReader.CreateBinaryReader(ms, q)
            Return ds.ReadObject(binFormat)
        End Using
    End Function
    Public Overloads Shared Function Deserialise(Of T)(data As Byte()) As Object
        Return Deserialise(GetType(T), data)
    End Function
#End Region

#Region "MustOverride"
    Public Overrides Function Deserialise_(Of T)(data() As Byte) As T
        Dim tt As Type = GetType(T)
        Return CDataContract.Deserialise(tt, data)
    End Function
    Public Overrides Function Serialise_(obj As Object) As Byte()
        Return Serialise(obj)
    End Function
#End Region

#Region "Remove Enums"
    Private Shared Function RemoveEnums(arr As Object()) As Object()
        For i As Integer = 0 To arr.Length - 1
            If TypeOf arr(i) Is System.Enum Then
                arr(i) = CInt(arr(i))
            End If
        Next
        Return arr
    End Function
#End Region


End Class
