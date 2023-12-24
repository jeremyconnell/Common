Imports System
Imports System.Data
Imports System.Collections.Generic

Imports Framework

<CLSCompliant(True)> _
Public Enum EAuditType
    Delete = 1
    Insert = 2
    Update = 3
End Enum

'Table-Row Class (Customisable half)
Partial Public Class CAudit_Type

#Region "Constants"
#End Region

#Region "Constructors (Public)"
    'Default DataSrc
    Public Sub New()
        MyBase.New()
    End Sub
#End Region

#Region "Default Values"
    Protected Overrides Sub InitValues()
        'Null values
        m_typeId = Integer.MinValue
        m_typeName = String.Empty

        'Custom values
    End Sub
#End Region

#Region "Default DataSrc"
    Protected Overrides Function DefaultDataSrc() As CDataSrc
        Return CDataSrc.Default
    End Function
#End Region

#Region "Members"
    'Rare, Non-Serialized stuff e.g. xml classes, lazy loading
#End Region

#Region "Properties"    
    'Derived/ReadOnly (e.g. xml classes, presentation logic)
    
    'Relationships - Foriegn Keys (e.g parent)

    'Relationships - Collections (e.g. children)
#End Region

#Region "Methods - Save/Delete Overrides"
     'e.g. Cascade deletes or insert triggers
#End Region

#Region "Caching Details"
    'Cache Timeout
    Private Shared Sub SetCache(ByVal key As String, ByVal value As CAudit_TypeList)
        If Not IsNothing(value) Then value.Sort()
        CCache.Set(key, value) 'Optional parameter can override timeout (default is 3 days)
    End Sub
    'Helper Method
    Private Function CacheGetById(ByVal list As CAudit_TypeList) As CAudit_Type
        Return list.GetById(Me.TypeId)
    End Function
#End Region

End Class