Imports System
Imports System.Text
Imports System.Data
Imports System.Collections.Generic
Imports System.Web
Imports System.IO

Imports Framework

'Collection Class (Customisable half)
Partial Public Class CAudit_LogList

#Region "Filters"
#End Region

#Region "Aggregation"
#End Region

#Region "Searching (Optional)"
    'Represents a simple search box to search PK and any string columns (add overloads as required, based on the pattern below)
    'e.g. Public Function Search(ByVal nameOrId As String, ByVal typeId As Integer) As CAudit_LogList
    Public Function Search(ByVal nameOrId As String) As CAudit_LogList
        '1. Normalisation
        If IsNothing(nameOrId) Then nameOrId = String.Empty
        nameOrId = nameOrId.Trim.ToLower()
        
        '2. Start with a complete list
        Dim results As CAudit_LogList = Me

        '3. Use any available index, such as those generated for fk/bool columns
        'Normal Case - non-unique index (e.g. foreign key)
        'If Integer.MinValue <> typeId Then results = results.GetByTypeId(typeId)

        'Special case - unique index (e.g. primary key)
        If Not String.IsNullOrEmpty(nameOrId) Then
            'Dim id As Integer
            'If Integer.TryParse(nameOrId, id) Then
            '    Dim obj As CAudit_Log = Me.GetById(id)
            '    If Not IsNothing(obj) Then
            '        results = New CAudit_LogList(1)
            '        results.Add(obj)
            '        return results
            '    End If
            'End If
        End If
        
        '4. Exit early if remaining (non-index) filters are blank
        If String.IsNullOrEmpty(nameOrId) Then Return results
        
        '5. Manually search each record using custom match logic, building a shortlist
        Dim shortList As New CAudit_LogList
        For Each i As CAudit_Log In results
            If Match(nameOrId, i) Then shortList.Add(i)
        Next
        Return shortList
    End Function
    'Manual Searching e.g for string-based columns i.e. anything not indexed (add more params if required)
    Private Function Match(ByVal name As String, ByVal obj As CAudit_Log) As Boolean
        If Not String.IsNullOrEmpty(name) Then
            If Not IsNothing(obj.LogMessage) AndAlso obj.LogMessage.ToLower().Contains(name) Then Return True
            Return False 'If filter is active, reject any items that dont match
        End If
        Return True 'No active filters (should catch this in step #4)
    End Function
#End Region

#Region "Cloning"
    Public Function Clone(ByVal target As CDataSrc) As CAudit_LogList ', parentId As Integer) As CAudit_LogList
        'No Transaction
        If TypeOf target Is CDataSrcRemote Then Return Clone(target, Nothing) ', parentId)

        'Transaction
        Dim cn As IDbConnection = target.Local.Connection()
        Dim tx As IDbTransaction = cn.BeginTransaction()
        Try
            Clone = Clone(target, tx) ', parentId)
            tx.Commit()
        Catch
            tx.Rollback()
            Throw
        Finally
            cn.Close()
        End Try
    End Function
    Public Function Clone(target as CDataSrc, txOrNull as IDbTransaction) As CAudit_LogList ', parentId As Integer) As CAudit_LogList
        Dim list As New CAudit_LogList(Me.Count)
        For Each i As CAudit_Log In Me
            list.Add(i.Clone(target, txOrNull))  ', parentId))    *Child entities must reference the new parent
        Next
        Return list
    End Function
#End Region

#Region "Export to Csv"
    'Note: For non-cached classes like this, should normally use CDataSrc.ExportToCsv(SelectWhere_DataSet)
    
    'Web - Need to add a project reference to System.Web, or comment out these two methods
    Public Sub ExportToCsv(ByVal response As HttpResponse)
        ExportToCsv(response, "Logs.csv")
    End Sub
    Public Sub ExportToCsv(ByVal response As HttpResponse, ByVal fileName As String)
        CDataSrc.ExportToCsv(response, fileName) 'Standard response headers
        Dim sw As New StreamWriter(response.OutputStream)
        ExportToCsv(sw)
        sw.Flush()
        response.End()
    End Sub

    'Non-web
    Public Sub ExportToCsv(ByVal filePath As String)
        Dim sw As New StreamWriter(filePath)
        ExportToCsv(sw)
        sw.Close()
    End Sub

    'Logic
    Protected Sub ExportToCsv(ByVal sw As StreamWriter)
        Dim headings As String() = New String() {"LogId", "LogTypeId", "LogMessage", "LogCreated"}
        CDataSrc.ExportToCsv(headings, sw)
        For Each i As CAudit_Log In Me
            Dim data As Object() = New Object() {i.LogId, i.LogTypeId, i.LogMessage, i.LogCreated}
            CDataSrc.ExportToCsv(data, sw)
        Next
    End Sub
#End Region

End Class
