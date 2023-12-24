Imports System.Text
Imports Framework


Imports ProtoBuf
Imports System.Runtime.Serialization

<DataContract>
Public Class CIndexInfo : Implements IComparable(Of CIndexInfo)
    'Data
    <DataMember(Order:=1)> Public TableName As String
    <DataMember(Order:=2)> Public IndexName As String
    <DataMember(Order:=3)> Public IsUnique As Boolean
    <DataMember(Order:=4)> Public ColumnNames As List(Of String)
    <DataMember(Order:=5)> Public MD5 As Guid

    'PreConstructor
    Shared Sub New()
        CProto.Prepare(Of CIndexInfo)()
    End Sub

    'Constructor
    Protected Sub New()
    End Sub
    Public Sub New(table As String, name As String, unique As Boolean, cols As List(Of String))
        Me.TableName = table
        Me.IndexName = name
        Me.IsUnique = unique
        Me.ColumnNames = cols
        Me.MD5 = CBinary.MD5_(table, unique.ToString, cols)  'name
    End Sub
    Public Overrides Function ToString() As String
        If IsUnique Then
            Return String.Concat("*UNIQ {", CUtilities.ListToString(ColumnNames), "} - ", IndexName, " ON ", TableName_)
        End If
        Return String.Concat("{", CUtilities.ListToString(ColumnNames), "} - ", IndexName, " ON ", TableName_)
    End Function
    Public ReadOnly Property ColumnNames_ As String
        Get
            Dim sb As New StringBuilder
            For Each i As String In ColumnNames
                If sb.Length > 0 Then sb.Append(",")
                sb.Append("[").Append(i).Append("]")
            Next
            Return sb.ToString
        End Get
    End Property
    Public ReadOnly Property TableName_ As String
        Get
            Dim sb As New StringBuilder
            Dim ss As String() = TableName.Split(CStr(".").ToCharArray)
            If ss.Length = 2 Then
                Return String.Concat("[", ss(0), "].[", ss(1), "]")
            End If
            Return TableName
        End Get
    End Property
    Public ReadOnly Property IndexName_ As String
        Get
            Return String.Concat(TableName_, ".[", IndexName, "]")
        End Get
    End Property

    Public Function CreateScript() As String
        If Not IsUnique Then
            Return String.Concat("CREATE NONCLUSTERED INDEX [", IndexName, "] ON ", TableName_, " (", ColumnNames_, ") ON [PRIMARY]") 'WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) 
        Else
            Return String.Concat("ALTER TABLE ", TableName_, " ADD  CONSTRAINT [", IndexName, "] UNIQUE NONCLUSTERED  (", ColumnNames_, ") ON [PRIMARY]") 'WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) 
        End If
    End Function
    Public Function DropScript() As String
        'Return String.Concat("DROP INDEX [", IndexName, "] ON ", TableName_)

        If IsUnique Then
            Return String.Concat("ALTER TABLE ", TableName_, " DROP CONSTRAINT [", IndexName, "]")
        Else
            Return String.Concat("DROP INDEX [", IndexName, "] ON ", TableName_)
        End If
    End Function

    Public Function CompareTo(other As CIndexInfo) As Integer Implements IComparable(Of CIndexInfo).CompareTo
        Dim i As Integer = Me.TableName.CompareTo(other.TableName)
        If i <> 0 Then Return i
        i = -Me.IsUnique.CompareTo(other.IsUnique)
        If i <> 0 Then Return i
        i = Me.ColumnNames_.CompareTo(other.ColumnNames_)
        If i <> 0 Then Return i
        Return Me.IndexName.CompareTo(other.IndexName)
    End Function
End Class