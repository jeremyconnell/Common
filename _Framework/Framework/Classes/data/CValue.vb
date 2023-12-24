Imports ProtoBuf
Imports System.Runtime.Serialization
Imports Framework

Public Enum EValueType
    [Boolean]
    [DateTime]
    [Decimal]
    [Double]
    [Guid]
    [Integer]
    [Long]
    [Short] 'Int16
    [String]
    [Byte]
    [Bytes]
    [DbNull]
End Enum

<DataContract, ProtoContract,
    ProtoInclude(100, GetType(CValueBoolean)),
    ProtoInclude(101, GetType(CValueDateTime)),
    ProtoInclude(102, GetType(CValueDecimal)),
    ProtoInclude(103, GetType(CValueDouble)),
    ProtoInclude(104, GetType(CValueGuid)),
    ProtoInclude(105, GetType(CValueInt)),
    ProtoInclude(106, GetType(CValueLong)),
    ProtoInclude(107, GetType(CValueShort)),
    ProtoInclude(108, GetType(CValueString)),
    ProtoInclude(109, GetType(CValueByte)),
    ProtoInclude(110, GetType(CValueBytes)),
    ProtoInclude(111, GetType(CValueNull)),
    KnownType(GetType(CValueBoolean)),
    KnownType(GetType(CValueDateTime)),
    KnownType(GetType(CValueDecimal)),
    KnownType(GetType(CValueDouble)),
    KnownType(GetType(CValueGuid)),
    KnownType(GetType(CValueInt)),
    KnownType(GetType(CValueLong)),
    KnownType(GetType(CValueShort)),
    KnownType(GetType(CValueNull)),
    KnownType(GetType(CValueByte)),
    KnownType(GetType(CValueBytes)),
    KnownType(GetType(CValueString))>
Public MustInherit Class CValue : Implements IComparable(Of CValue)

    'Properties
    <DataMember(Order:=1)> Public Name As String
    <DataMember(Order:=2)> Public Id As Integer?

    'Constructors
    Public Sub New()
    End Sub
    Public Sub New(name As String)
        Me.Name = name
    End Sub
    Public Sub New(id As Integer)
        Me.Id = id
    End Sub
    Shared Sub New()
        CProto.Prepare(Of CValue)()
    End Sub

    'MustOverride
    Public MustOverride ReadOnly Property Type As EValueType
    Public MustOverride ReadOnly Property Value As Object


    'Factory - object
    Public Shared Function Factory(name As String, value As Object) As CValue
        Dim t As EValueType = GetValueType(value)
        Select Case t
            Case EValueType.Integer : Return Factory(name, CInt(value))
            Case EValueType.String : Return Factory(name, CType(value, String))
            Case EValueType.Long : Return Factory(name, CLng(value))
            Case EValueType.Double : Return Factory(name, CDbl(value))
            Case EValueType.Decimal : Return Factory(name, CDec(value))
            Case EValueType.DateTime : Return Factory(name, CDate(value))
            Case EValueType.Boolean : Return Factory(name, CBool(value))
            Case EValueType.Guid : Return Factory(name, CType(value, Guid))
            Case EValueType.Short : Return Factory(name, CType(value, Short))
            Case EValueType.Byte : Return Factory(name, CType(value, Byte))
            Case EValueType.Bytes : Return Factory(name, CType(value, Byte()))
            Case EValueType.DbNull : Return Factory(name)
        End Select
        Throw New Exception("New Type: " & value.GetType().ToString)
    End Function

    'Factory - typed
    Public Shared Function Factory(name As String, data As Integer) As CValue
        Return New CValueInt(name, data)
    End Function
    Public Shared Function Factory(name As String, data As String) As CValue
        Return New CValueString(name, data)
    End Function
    Public Shared Function Factory(name As String, data As Long) As CValue
        Return New CValueLong(name, data)
    End Function
    Public Shared Function Factory(name As String, data As Double) As CValue
        Return New CValueDouble(name, data)
    End Function
    Public Shared Function Factory(name As String, data As Decimal) As CValue
        Return New CValueDecimal(name, data)
    End Function
    Public Shared Function Factory(name As String, data As DateTime) As CValue
        Return New CValueDateTime(name, data)
    End Function
    Public Shared Function Factory(name As String, data As Guid) As CValue
        Return New CValueGuid(name, data)
    End Function
    Public Shared Function Factory(name As String, data As Boolean) As CValue
        Return New CValueBoolean(name, data)
    End Function
    Public Shared Function Factory(name As String, data As Short) As CValue
        Return New CValueShort(name, data)
    End Function
    Public Shared Function Factory(name As String, data As Byte) As CValue
        Return New CValueByte(name, data)
    End Function
    Public Shared Function Factory(name As String, data As Byte()) As CValue
        Return New CValueBytes(name, data)
    End Function
    Public Shared Function Factory(name As String) As CValue
        Return New CValueNull(name)
    End Function

    'Resolve type
    Private Shared Function GetValueType(value As Object) As EValueType
        If TypeOf value Is Integer Then Return EValueType.Integer
        If TypeOf value Is String Then Return EValueType.String
        If TypeOf value Is Boolean Then Return EValueType.Boolean
        If TypeOf value Is DateTime Then Return EValueType.DateTime
        If TypeOf value Is Double Then Return EValueType.Double
        If TypeOf value Is Decimal Then Return EValueType.Decimal
        If TypeOf value Is Long Then Return EValueType.Long
        If TypeOf value Is Short Then Return EValueType.Short
        If TypeOf value Is Guid Then Return EValueType.Guid
        If TypeOf value Is Byte Then Return EValueType.Byte
        If TypeOf value Is Byte() Then Return EValueType.Bytes
        If TypeOf value Is DBNull Then Return EValueType.DbNull
        Throw New Exception("New type: " & value.GetType.ToString)
    End Function


    Public ReadOnly Property AsInt As Integer
        Get
            If Me.Type = EValueType.Byte Then
                Return Convert.ToInt32(CType(Me, CValueByte).ValueByte)
            ElseIf Me.Type = EValueType.DbNull Then
                Return Integer.MinValue
            Else
                Return CType(Me, CValueInt).ValueInt
            End If
        End Get
    End Property
    Public ReadOnly Property AsShort As Short
        Get
            If Me.Type = EValueType.Byte Then
                Return Convert.ToInt16(CType(Me, CValueByte).ValueByte)
            ElseIf Me.Type = EValueType.DbNull Then
                Return Int16.MinValue
            ElseIf Me.Type = EValueType.Integer Then
                Return Convert.ToInt16(CType(Me, CValueInt).ValueInt)
            Else
                Return CType(Me, CValueShort).ValueShort
            End If
        End Get
    End Property
    Public ReadOnly Property AsLong As Long
        Get
            If Me.Type = EValueType.Integer Then
                Return Convert.ToInt64(CType(Me, CValueInt).ValueInt)
            ElseIf Me.Type = EValueType.Short Then
                Return Convert.ToInt64(CType(Me, CValueShort).ValueShort)
            ElseIf Me.Type = EValueType.DbNull Then
                Return Long.MinValue
            Else
                Return CType(Me, CValueLong).ValueLong
            End If
        End Get
    End Property
    Public ReadOnly Property AsDateTime As DateTime
        Get
            Return CType(Me, CValueDateTime).ValueDateTime
        End Get
    End Property
    Public ReadOnly Property AsBool As Boolean
        Get
            Return CType(Me, CValueBoolean).ValueBoolean
        End Get
    End Property
    Public ReadOnly Property AsGuid As Guid
        Get
            Return CType(Me, CValueGuid).ValueGuid
        End Get
    End Property
    Public ReadOnly Property AsString As String
        Get
            Return CType(Me, CValueString).ValueString
        End Get
    End Property
    Public ReadOnly Property AsByte As Byte
        Get
            Return CType(Me, CValueByte).ValueByte
        End Get
    End Property
    Public ReadOnly Property AsBytes As Byte()
        Get
            Return CType(Me, CValueBytes).ValueBytes
        End Get
    End Property

    'Comparer
    Public MustOverride Function CompareTo(other As CValue) As Integer Implements IComparable(Of CValue).CompareTo

    'Serialise
    Public MustOverride Function Serialise() As Byte()

    Public Overrides Function ToString() As String
        Return Value.ToString
    End Function
    Public Overrides Function GetHashCode() As Integer
        Return ToString.GetHashCode
    End Function
End Class
