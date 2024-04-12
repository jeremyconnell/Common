Imports ProtoBuf
Imports System.Runtime.Serialization
Imports System.Text

<DataContract>
Public Class CColumnList : Inherits List(Of CColumn)

    'Constructors
    Public Sub New()
        MyBase.New
    End Sub
    Public Sub New(count As Integer)
        MyBase.New(count)
    End Sub
    Public Sub New(list As ICollection(Of CColumn))
        MyBase.New(list)
    End Sub
    Public Sub New(list As List(Of String))
        MyBase.New(list.Count)

        For Each i As String In list
            Me.Add(New CColumn(i))
        Next
    End Sub

    'Aggregation
    Public ReadOnly Property NamesAbc_ As String
        Get
            Return CUtilities.ListToString(NamesAbc)
        End Get
    End Property
    Public ReadOnly Property NamesAbc As List(Of String)
        Get
            NamesAbc = Names
            NamesAbc.Sort()
        End Get
    End Property
	Public ReadOnly Property Names As List(Of String)
		Get
			Dim list As New List(Of String)(Me.Count)
			For Each i As CColumn In Me
				list.Add(i.Name)
			Next
			Return list
		End Get
	End Property
	Public ReadOnly Property NamesAndTypes As List(Of String)
		Get
			Dim list As New List(Of String)(Me.Count)
			For Each i As CColumn In Me
				list.Add(i.Script)
			Next
			Return list
		End Get
	End Property
    Public ReadOnly Property Script As String
        Get
            Dim sb As New StringBuilder
            For Each i As CColumn In Me
                If sb.Length > 0 Then sb.Append(", ")
                sb.Append(i.Script)
            Next
            Return sb.ToString
        End Get
    End Property
    Public ReadOnly Property HasVarBinary As Boolean
        Get
            For Each i As CColumn In Me
                If i.Type.StartsWith("VARBINARY") Then Return True
            Next
            Return False
        End Get
    End Property


    'Index/Hash
    Private m_index As Dictionary(Of String, CColumn)
    Private m_md5 As Guid = Guid.Empty

	Public Function Has(name As String) As Boolean
		If IsNothing(name) Then Return Nothing
		Return Index.ContainsKey(name.ToLower)
	End Function
	Public Overloads Function Remove(name As String) As Boolean
		If IsNothing(name) Then Return Nothing
		Dim c As CColumn = Nothing
		If Not Index.TryGetValue(name.ToLower, c) Then Return False
		Index.Remove(name.ToLower)
		MyBase.Remove(c)
		Return True
	End Function
	Default Public Overloads ReadOnly Property Item(name As String) As CColumn
		Get
			If IsNothing(name) Then Return Nothing
			Dim c As CColumn = Nothing
			Index.TryGetValue(name.ToLower, c)
			Return c
		End Get
	End Property
	Public ReadOnly Property Index As Dictionary(Of String, CColumn)
        Get
            If IsNothing(m_index) OrElse m_index.Count <> Me.Count Then
                Dim temp As New Dictionary(Of String, CColumn)
                For Each i As CColumn In Me
					temp(i.NameLower) = i
				Next
                m_index = temp
            End If
            Return m_index
        End Get
    End Property
    Public ReadOnly Property MD5 As Guid
        Get
            If m_md5 = Guid.Empty Then
                Dim temp As New CColumnList(Me)
                temp.Sort()

                Dim list As New List(Of Guid)
                For Each i As CColumn In temp
                    list.Add(i.MD5)
                Next
                m_md5 = CBinary.MD5_(list)
            End If
            Return m_md5
        End Get
    End Property

End Class
