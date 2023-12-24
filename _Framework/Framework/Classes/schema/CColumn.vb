Imports ProtoBuf
Imports System.Runtime.Serialization
Imports Framework

<DataContract>
Public Class CColumn : Implements IComparable(Of CColumn)
    'Data
    <DataMember(Order:=1)> Public Name As String
    <DataMember(Order:=2)> Public Type As String
    <DataMember(Order:=3)> Public IsNullable As Boolean

    'Preconstructor
    Shared Sub New()
        CProto.Prepare(Of CColumn)()
    End Sub

    'Constructors
    Public Sub New()
    End Sub
    Public Sub New(name As String, type As String, isNullable As Boolean)
        Me.Name = name
        Me.Type = type
        Me.IsNullable = isNullable
    End Sub
    Public Sub New(nameAndType As String)
        Dim s As String = nameAndType

        Dim i As Integer = s.IndexOf(" ")
        If i < 0 Then
            Me.Name = s
            Me.Type = "ERROR"
            Exit Sub
        End If

        Me.Name = s.Substring(0, i)
		Me.Type = s.Substring(i + 1)
		Me.IsNullable = Not Type.ToUpper.Contains(" NOT NULL")
		Me.Type = Me.Type.Replace("NOT NULL", "").Replace("NULL", "").Trim
    End Sub

	Public ReadOnly Property Name_ As String
		Get
			Return String.Concat("[", Name, "]")
		End Get
	End Property

	'Sorting (for hash only)
	Public Function CompareTo(other As CColumn) As Integer Implements IComparable(Of CColumn).CompareTo
		Return NameLower.CompareTo(other.NameLower)
	End Function
    'For Indexing by name
    Public ReadOnly Property NameLower As String
        Get
            Return Name.ToLower
        End Get
    End Property
    Public ReadOnly Property Script As String
        Get
            Return String.Concat(Name_, " ", Type _
                .Replace("(1073741823)", "") _
                .Replace("(2147483647)", "") _
                .Replace("(-1)", "(max)"), IIf(IsNullable, " NULL", " NOT NULL"))
        End Get
    End Property
	Public ReadOnly Property MD5 As Guid
		Get
			Return CBinary.MD5_(Script.ToLower)
		End Get
	End Property

	Public Overrides Function ToString() As String
		Return Script
	End Function
End Class
