Imports Framework
Imports ProtoBuf
Imports System.Runtime.Serialization

<DataContract, ProtoContract>
Public Class CProcedure : Implements IComparable(Of CProcedure)

    'Data
    <DataMember(Order:=1)> Public Name As String
    <DataMember(Order:=2)> Public IsStoredProc As Boolean
    <DataMember(Order:=3)> Public Text As String
    <DataMember(Order:=4)> Public MD5 As Guid

    'Preconstructor
    Shared Sub New()
        CProto.Prepare(Of CProcedure)()
    End Sub

    'Constructors
    Protected Sub New()
    End Sub
    Public Sub New(name As String, isProc As Boolean, text As String)
        Me.Name = name
        Me.IsStoredProc = isProc
        Me.Text = text
		Me.MD5 = CBinary.MD5_(isProc.ToString, CViewInfo.Normalise(text))
	End Sub


    Public Function CreateScript() As String
        Return String.Concat(Text)
    End Function
    Public Function DropScript() As String
        Return String.Concat("DROP PROCEDURE ", Name)
    End Function

    Public Function CompareTo(other As CProcedure) As Integer Implements IComparable(Of CProcedure).CompareTo
        Dim i As Integer = Me.Name.CompareTo(other.Name)
        If i <> 0 Then Return i
        i = -Me.IsStoredProc.CompareTo(other.IsStoredProc)
        If i <> 0 Then Return i
        Return Me.MD5.CompareTo(other.MD5)
    End Function

    Public Overrides Function ToString() As String
        Return String.Concat(Name, vbTab, IIf(IsStoredProc, "SP", "Fn"), vbTab, CBinary.ToBase64(Me.MD5).ToUpper.Substring(0, 6))
    End Function
End Class