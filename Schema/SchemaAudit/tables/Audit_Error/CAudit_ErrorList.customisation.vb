Imports System
Imports System.Data
Imports System.Collections.Generic
Imports System.Web
Imports System.IO

Imports Framework

'Collection Class (Customisable half)
Partial Public Class CAudit_ErrorList

#Region "Cloning"
    Public Function Clone(ByVal target As CDataSrc) As CAudit_ErrorList ', parentId As Integer) As CAudit_ErrorList
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
    Public Function Clone(target as CDataSrc, txOrNull as IDbTransaction) As CAudit_ErrorList ', parentId As Integer) As CAudit_ErrorList
        Dim list As New CAudit_ErrorList(Me.Count)
        For Each i As CAudit_Error In Me
            list.Add(i.Clone(target, txOrNull))  ', parentId))    *Child entities must reference the new parent
        Next
        Return list
    End Function
#End Region

#Region "Export to Csv" '*Normally use CDataSrc.ExportToCsv(this.SelectWhere_DataSet)
    'Web
    Public Sub ExportToCsv(ByVal response As HttpResponse)
        ExportToCsv(response, "TblAudit_Errors.csv")
    End Sub
    Public Sub ExportToCsv(ByVal response As HttpResponse, ByVal fileName As String)
        CDataSrc.ExportToCsv(response, fileName)
        'Standard response headers
        Dim sw As New StreamWriter(response.OutputStream)
        ExportToCsv(sw)
        sw.Flush()
        response.[End]()
    End Sub

    'Non-web
    Public Sub ExportToCsv(ByVal filePath As String)
        Dim sw As New StreamWriter(filePath)
        ExportToCsv(sw)
        sw.Close()
    End Sub

    'Logic
    Protected Sub ExportToCsv(ByVal sw As StreamWriter)
        Dim headings As String() = New String() {"ErrorID", "ErrorUserID", "ErrorUserName", "ErrorWebsite", "ErrorUrl", "ErrorMachineName", "ErrorApplicationName", "ErrorApplicationVersion", "ErrorType", "ErrorMessage", "ErrorStacktrace", "ErrorInnerType", "ErrorInnerMessage", "ErrorInnerStacktrace", "ErrorDateCreated"}
        CDataSrc.ExportToCsv(headings, sw)
        For Each i As CAudit_Error In Me
            Dim data As Object() = New Object() {i.ErrorID, i.ErrorUserID, i.ErrorUserName, i.ErrorWebsite, i.ErrorUrl, i.ErrorMachineName, i.ErrorApplicationName, i.ErrorApplicationVersion, i.ErrorType, i.ErrorMessage, i.ErrorStacktrace, i.ErrorInnerType, i.ErrorInnerMessage, i.ErrorInnerStacktrace, i.ErrorDateCreated}
            CDataSrc.ExportToCsv(data, sw)
        Next
    End Sub
#End Region

End Class
