Imports System
Imports System.Data
Imports System.Collections.Generic

Imports Framework

'Collection Class (Customisable half)
Partial Public Class CAudit_TrailList

#Region "Preload Child collections"
    'Allows the option to preload all child collections across the set (using in-memory index), avoiding database chatter
    Public Sub PreloadDatas() 'Loads children for page of results (this list)
        PreloadDatas((New CAudit_Data()).SelectByTrailIds(Me.Ids))
    End Sub
    Public Sub PreloadDatas(ByVal allDatas As CAudit_DataList) 'Load children from a known universe (retrieved earlier)
        For Each i As CAudit_Trail In Me
            i.Datas = allDatas.GetByTrailId(i.AuditId)
            i.Datas.Audit_Trail = i
        Next
    End Sub
#End Region

End Class
