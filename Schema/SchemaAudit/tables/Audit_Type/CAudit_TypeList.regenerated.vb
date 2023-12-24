Imports System
Imports System.Data
Imports System.Collections.Generic

Imports Framework

<Serializable(), CLSCompliant(True)> _
Partial Public Class CAudit_TypeList : Inherits List(Of CAudit_Type)

#Region "Constructors"
    Public Sub New()
        MyBase.New()  
    End Sub
    Public Sub New(ByVal capacity As Integer)
        MyBase.New(capacity)  
    End Sub
    Public Sub New(ByVal list As CAudit_TypeList)
        MyBase.New(list)
        _index = list._index 
    End Sub
#End Region

#Region "Main Index (on TypeId)"
    'Only uncomment if type is NOT an Integer (e.g. string/guid)
    'Default Public Overloads ReadOnly Property Item(ByVal typeId As Integer) As CAudit_Type
    '    Get
    '        Return GetById(typeId)
    '    End Get
    'End Property
    Public Function GetById(ByVal typeId As Integer) As CAudit_Type
        Dim c As CAudit_Type = Nothing
        Index.TryGetValue(typeId, c)
        Return c
    End Function
    <NonSerialized()> _
    Private _index As Dictionary(Of Integer, CAudit_Type)
    Private ReadOnly Property Index() As Dictionary(Of Integer, CAudit_Type)
        Get
            If Not IsNothing(_index) Then
                If _index.Count = Me.Count Then
                    Return _Index
                End If
            End If
            _index = New Dictionary(Of Integer, CAudit_Type)(Me.Count)
            For Each i As CAudit_Type in Me
                _index(i.TypeId) = i
            Next
            Return _index
        End Get
    End Property
#End Region

#Region "Cache-Control"
    Public Shadows Sub Add(ByVal item As CAudit_Type)
        If Not IsNothing(_index) Then
            If Not _index.ContainsKey(item.TypeId) Then
                _index(item.TypeId) = item
            End If
        End If
        MyBase.Add(item)
    End Sub
    Public Shadows Sub Remove(ByVal item As CAudit_Type)
        If Not IsNothing(_index) Then
            If _index.ContainsKey(item.TypeId) Then
                _index.Remove(item.TypeId)
            End If
        End If
        MyBase.Remove(item)
    End Sub
#End Region
    
#Region "Foreign-Key Indices (Subsets)"
#End Region

End Class