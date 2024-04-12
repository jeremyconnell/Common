Option Strict Off

Imports System.Runtime.Serialization

#Region "Enums"
<DataContract>
Public Enum ESerialisation
    Protobuf = 0 'Default
    BinarySer = 1
    DataContract = 2
    Json = 3
    Xml = 4
End Enum
#End Region

Public MustInherit Class CSerialise

#Region "MustOverride"
    Public MustOverride Function Serialise_(obj As Object) As Byte()
    Public MustOverride Function Deserialise_(Of T)(data As Byte()) As T
#End Region

#Region "Generic"
    Public Function AsInt(packed As Byte()) As Integer
        Return Deserialise_(Of Integer)(packed)
    End Function
    Public Function AsLong(packed As Byte()) As Long
        Return Deserialise_(Of Long)(packed)
    End Function
    Public Function AsStr(packed As Byte()) As String
        Return Deserialise_(Of String)(packed)
    End Function
    Public Function AsGuid(packed As Byte()) As Guid
        Return Deserialise_(Of Guid)(packed)
    End Function
    Public Function AsTimespan(packed As Byte()) As TimeSpan
        Return Deserialise_(Of TimeSpan)(packed)
    End Function
    Public Function AsBool(packed As Byte()) As Boolean
        Return Deserialise_(Of Boolean)(packed)
    End Function
    Public Function AsListStr(packed As Byte()) As List(Of String)
        Return Deserialise_(Of List(Of String))(packed)
    End Function
    Public Function AsListInt(packed As Byte()) As List(Of Integer)
        Return Deserialise_(Of List(Of Integer))(packed)
    End Function

    Public Function AsListOfGuid(packed As Byte()) As List(Of Guid)
        Return Deserialise_(Of List(Of Guid))(packed)
    End Function
    Public Function AsDictStrGuid(packed As Byte()) As Dictionary(Of String, Guid)
        Return Deserialise_(Of Dictionary(Of String, Guid))(packed)
    End Function
    Public Function AsDictStrStr(packed As Byte()) As Dictionary(Of String, String)
        Return Deserialise_(Of Dictionary(Of String, String))(packed)
    End Function
    Public Function AsDictStrInt(packed As Byte()) As Dictionary(Of String, Integer)
        Return Deserialise_(Of Dictionary(Of String, Integer))(packed)
    End Function
    Public Function AsDictStrLong(packed As Byte()) As Dictionary(Of String, Long)
        Return Deserialise_(Of Dictionary(Of String, Long))(packed)
    End Function

    Public Function AsListOfFiles(packed As Byte()) As Dictionary(Of String, Byte())
        Return Deserialise_(Of Dictionary(Of String, Byte()))(packed)
    End Function
    Public Function AsFolderHash(packed As Byte()) As CFolderHash
        Return Deserialise_(Of CFolderHash)(packed)
    End Function

	Public Function AsEx(packed As Byte()) As CException
		Return Deserialise_(Of CException)(packed)
	End Function
	Public Function AsExList(packed As Byte()) As List(Of CException)
		Return Deserialise_(Of List(Of CException))(packed)
	End Function
#End Region

#Region "Shared - ESerialisation cases"
	Public Shared Function Serialise(obj As Object, format As ESerialisation) As Byte()
        Select Case format
            Case ESerialisation.Protobuf : Return CProto.Serialise(obj)
            Case ESerialisation.BinarySer : Return CBinary.SerialiseToBytes(obj)
            Case ESerialisation.DataContract : Return CDataContract.Serialise(obj)
            Case Else : Throw New Exception("Serialisation format not supported: " & format.ToString)
        End Select
    End Function
    Public Shared Function Deserialise(Of T)(data As Byte(), format As ESerialisation) As T
        Select Case format
            Case ESerialisation.Protobuf : Return CProto.Deserialise(Of T)(data)
            Case ESerialisation.BinarySer : Return CType(CBinary.DeserialiseFromBytes(data), T) 'Type is self-specified
            Case ESerialisation.DataContract : Return CDataContract.Deserialise(Of T)(data)
            Case Else : Throw New Exception("Serialisation format not supported: " & format.ToString)
        End Select
    End Function
    Public Shared Function Formatter(format As ESerialisation) As CSerialise
        Select Case format
            Case ESerialisation.Protobuf : Return Proto
            Case ESerialisation.BinarySer : Return Old
            Case ESerialisation.DataContract : Return DC
            Case Else : Throw New Exception("Serialisation format not supported: " & format.ToString)
        End Select
    End Function
#End Region

#Region "Singletons"
    Public Shared Old As New CBinarySer
    Public Shared DC As New CDataContract
    Public Shared Proto As New CProto

    Public Shared Function Choose(format As ESerialisation) As CSerialise
        Select Case format
            Case ESerialisation.BinarySer : Return Old
            Case ESerialisation.DataContract : Return DC
            Case ESerialisation.Protobuf : Return Proto
        End Select
        Throw New Exception("TODO: add format " & format.ToString)
    End Function
#End Region

#Region "Pack/Unpack"
    'ESerialisation overloads
    Public Shared Function Pack(params As Object(), f As ESerialisation) As Byte()
        Return Pack(params, Formatter(f))
    End Function
    Public Shared Function Unpack(binary As Byte(), f As ESerialisation) As CReader
        Return Unpack(binary, Formatter(f))
    End Function

    'CReader shortcuts
    Private Shared Function Pack(params As Object(), format As CSerialise) As Byte()
        Return CReader.Pack(params, format)
    End Function
    Private Shared Function Unpack(binary As Byte(), format As CSerialise) As CReader
        Return New CReader(binary, format)
    End Function
#End Region
End Class
