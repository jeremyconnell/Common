Imports System
Imports System.Data
Imports System.Collections.Generic

Imports Framework

#Region "Enum: Primary Key Values"
<CLSCompliant(True)> _
Public Enum EList
    EmailQueueOwner = 1
End Enum
#End Region

'Table-Row Class (Customisable half)
Partial Public Class CList

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
    Protected Friend Sub New(listId As Integer)
        MyBase.New(listId)
    End Sub
    Protected Friend Sub New(ByVal dataSrc As CDataSrc, listId As Integer)
        MyBase.New(dataSrc, listId)
    End Sub
    Protected Friend Sub New(ByVal dataSrc As CDataSrc, ByVal listId As Integer, ByVal txOrNull As IDbTransaction)
        MyBase.New(dataSrc, listId, txOrNull)
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
    Public ReadOnly Property [Items]() As CItemList
        Get
            Return CItem.Cache.GetByListId(Me.ListId)
        End Get
    End Property
#End Region

#Region "Properties - Customisation"
    Friend Function ExternalDataSrc() As CDataSrc
        Return New CSqlClient(Me.ListExternalConnectionString)
    End Function
    Friend Function ExternalColumns() As String
        Return String.Concat(Me.ListExteralNameColumn, ",", Me.ListExteralPrimaryKey)
    End Function
    Friend Function ExternalSqlToSelectAll() As String
        Return String.Concat("SELECT ", ExternalColumns, " FROM ", Me.ListExternalTable)
    End Function
    Friend Function ExternalSqlToSelectById(ByVal id As Integer) As String
        Return String.Concat(Me.ListExteralNameColumn, " WHERE ", Me.ListExteralPrimaryKey, "=", id)
    End Function
    Public Function ExternalData() As DataSet
        Return ExternalDataSrc.SelectAll_Dataset(ExternalColumns, Me.ListExternalTable, Me.ListExteralNameColumn)
    End Function
#End Region

#Region "Save/Delete Overrides"
    Public Overrides Sub Delete(ByVal txOrNull As IDbTransaction)
        'Use a transaction if none supplied
        If txOrNull Is Nothing Then
            BulkDelete(Me)
            Exit Sub
        End If

        'Cascade-Delete (all child collections)
        Me.Items.DeleteAll(txOrNull)

        'Normal Delete
        MyBase.Delete(txOrNull)
    End Sub
#End Region

#Region "Search Logic"
    'Public Interfaces: Refer to list class for Search (and GetBy*) methods e.g. CList.Cache.Search

    'Complex Search Logic (e.g string-based searching) Note: Index-based filters (e.g. fk/bool) are handled in the Search method, and can be ignored here
    Friend Function Match(ByVal filters As CSearchFilters) As Boolean
        With filters
            'Apply active filters here i.e. return false if any filter is active and excludes this record
            If Not String.IsNullOrEmpty(.Name) AndAlso Not Me.ListName.ToLower.StartsWith(.Name) Then Return False 'Starts-with logic (trailing wildcard)
        End With
        Return True
    End Function
#End Region

#Region "Search Filters"
    'Filters i.e. UI-driven collection of potential search filters (e.g. 'name' from textbox could search firstname/lastname columns)
    Public Class CSearchFilters
        'Constructors (add more for common combinations, or introduce more classes)
        Public Sub New(ByVal isExternalOnly As Boolean, ByVal name As String)
            Me.IsExternalOnly = isExternalOnly
            Me.Name = name.ToLower()
        End Sub

        'Members (filter info)
        Public IsExternalOnly As Boolean = False
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
    Private Shared Function LoadCache() As CListList
        Return New CList().SelectAll()
    End Function
    'Cache Timeout
    Private Shared Sub SetCache(ByVal key As String, ByVal value As CListList)
        If Not IsNothing(value) Then value.Sort()
        CCache.Set(key, value) 'Optional parameter can override timeout (default is 3 days)
    End Sub
    'Helper Method
    Private Function CacheGetById(ByVal list As CListList) As CList
        Return list.GetById(Me.ListId)
    End Function
#End Region

#Region "ToXml"
    Protected Overrides Sub ToXml_Custom(ByVal w As System.Xml.XmlWriter)
        'Store(w, "Example", Me.Example)
    End Sub
#End Region

End Class
