Imports System
Imports System.Data
Imports System.Collections.Generic

Imports Framework

<CLSCompliant(True)> _
Public Enum EConfigType
    [Boolean] = 1
    [String] = 2
    [Double] = 3
    [Int] = 4
    [Date] = 5
    Money = 7
    ListAsInteger = 8
    ListAsString = 9
End Enum

'Table-Row Class (Customisable half)
Partial Public Class CType_

#Region "Constants"
#End Region

#Region "Constructors (Public)"
    'Default Connection String
    Public Sub New()
        MyBase.New()
    End Sub
    
    'Alternative Connection String
    Public Sub New(ByVal dataSrc as CDataSrc)
        MyBase.New(dataSrc)
    End Sub
    
    'Hidden (UI code should use cache instead)
    Protected Friend Sub New(typeId As Integer)
        MyBase.New(typeId)
    End Sub
    Protected Friend Sub New(ByVal dataSrc As CDataSrc, typeId As Integer)
        MyBase.New(dataSrc, typeId)
    End Sub
    Protected Friend Sub New(ByVal dataSrc As CDataSrc, ByVal typeId As Integer, ByVal txOrNull As IDbTransaction)
        MyBase.New(dataSrc, typeId, txOrNull)
    End Sub
#End Region

#Region "Default Values"
    Protected Overrides Sub InitValues_Custom()
        'e.g. m_sampleDateCreated = DateTime.Now
    End Sub
#End Region

#Region "Default Connection String"
    Protected Overrides Function DefaultDataSrc() As CDataSrc
        Return CDataSrc.Default
    End Function
#End Region

#Region "Properties - Relationships"    
    'Relationships - Foriegn Keys (e.g parent)

    'Relationships - Collections (e.g. children)

#End Region

#Region "Properties - Customisation"
    'Derived/ReadOnly (e.g. xml classes, presentation logic)
#End Region

#Region "Save/Delete Overrides"
     'Can Override MyBase.Save/Delete (e.g. Cascade deletes, or insert related records)
#End Region

#Region "Search Logic"
    'Public Interfaces: Refer to list class for Search (and GetBy*) methods e.g. CType_.Cache.Search

    'Complex Search Logic (e.g string-based searching) Note: Index-based filters (e.g. fk/bool) are handled in the Search method, and can be ignored here
    Friend Function Match(ByVal filters As CSearchFilters) As Boolean
        With filters
            'Apply active filters here i.e. return false if any filter is active and excludes this record
            If Not String.IsNullOrEmpty(.Name) AndAlso Not Me.TypeName.ToLower.StartsWith(.Name) Then Return False 'Starts-with logic (trailing wildcard)
        End With
        Return True
    End Function
#End Region

#Region "Search Filters"
    'Filters i.e. UI-driven collection of potential search filters (e.g. 'name' from textbox could search firstname/lastname columns)
    Public Class CSearchFilters
        'Constructors (add more for common combinations, or introduce more classes)
        Public Sub New(ByVal name As String)
            Me.Name = name.ToLower()
        End Sub

        'Members (filter info)
        Public Name As String = String.Empty

        'Properties (checks if a complex search is required i.e. some filters are active, other than index-based ones)
        Public Readonly Property NonIndexedFiltersAreNull As Boolean
            Get
                Return True 'e.g. Return String.IsNullOrEmpty(Me.Name) AndAlso ...
            End Get
        End Property
End Class
#End Region

#Region "Caching Details"
    'Cache data
    Private Shared Function LoadCache() As CType_List
        Return New CType_().SelectAll()
    End Function
    'Cache Timeout
    Private Shared Sub SetCache(ByVal key As String, ByVal value As CType_List)
        If Not IsNothing(value) Then value.Sort()
        CCache.Set(key, value) 'Optional parameter can override timeout (default is 3 days)
    End Sub
    'Helper Method
    Private Function CacheGetById(ByVal list As CType_List) As CType_
        Return list.GetById(Me.TypeId)
    End Function
#End Region

#Region "ToXml"
    Protected Overrides Sub ToXml_Custom(ByVal w As System.Xml.XmlWriter)
        'Store(w, "Example", Me.Example)
    End Sub
#End Region

End Class
