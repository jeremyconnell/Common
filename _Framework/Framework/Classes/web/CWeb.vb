Imports System.Web.HttpContext
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls

Public Class CWeb

#Region "Querystring"
    Public Shared Function RequestByte(ByVal key As String) As Byte 'tinyint
        Return RequestByte(key, 0)
    End Function
    Public Shared Function RequestByte(ByVal key As String, ByVal defaultValue As Byte) As Byte
        Dim value As String = Current.Request(key)
        If Len(value) > 0 Then
            Dim i As Byte
            If Byte.TryParse(value, i) Then Return i
        End If
        Return defaultValue
    End Function

    Public Shared Function RequestInt(ByVal key As String) As Integer
        Return RequestInt(key, Integer.MinValue)
    End Function
    Public Shared Function RequestInt(ByVal key As String, ByVal defaultValue As Integer) As Integer
        Return RequestInt(key, defaultValue, Current)
    End Function
    Public Shared Function RequestInt(ByVal key As String, ByVal defaultValue As Integer, context As System.Web.HttpContext) As Integer
        Dim value As String = context.Request(key)
        If Len(value) > 0 Then
            Dim i As Integer
            If Integer.TryParse(value, i) Then Return i
        End If
        Return defaultValue
    End Function

    Public Shared Function RequestLong(ByVal key As String) As Long
        Return RequestLong(key, Long.MinValue)
    End Function
    Public Shared Function RequestLong(ByVal key As String, ByVal defaultValue As Long) As Long
        Dim value As String = Current.Request(key)
        If Len(value) > 0 Then
            Dim i As Long
            If Long.TryParse(value, i) Then Return i
        End If
        Return defaultValue
    End Function

    Public Shared Function RequestGuid(ByVal key As String) As Guid
        Return RequestGuid(key, Guid.Empty)
    End Function
    Public Shared Function RequestGuid(ByVal key As String, ByVal defaultValue As Guid) As Guid
        Dim value As String = Current.Request(key)
        If String.IsNullOrEmpty(value) Then Return defaultValue
        Try
            Return New Guid(value)
        Catch ex As Exception
            Return defaultValue
        End Try
    End Function

    Public Shared Function RequestBool(ByVal key As String) As Boolean
        Return RequestBool(key, False)
    End Function
    Public Shared Function RequestBool(ByVal key As String, ByVal defaultValue As Boolean) As Boolean
        Dim value As String = Current.Request(key)
        If String.IsNullOrEmpty(value) Then Return defaultValue
        Select Case value.ToLower
            Case "true", "yes", "1" : Return True
            Case "false", "no", "0" : Return False
            Case Else : Return defaultValue
        End Select
    End Function

    Public Shared Function RequestStr(ByVal key As String) As String
        Return RequestStr(key, String.Empty)
    End Function
    Public Shared Function RequestStr(ByVal key As String, ByVal defaultValue As String) As String
        Return RequestStr(key, defaultValue, Current)
    End Function
    Public Shared Function RequestStr(ByVal key As String, ByVal defaultValue As String, context As System.Web.HttpContext) As String
        Dim value As String = context.Request(key)
        If IsNothing(value) Then Return defaultValue
        Return value
    End Function

    Public Shared Function RequestDate(ByVal key As String) As DateTime
        Return RequestDate(key, DateTime.MinValue)
    End Function
    Public Shared Function RequestDate(ByVal key As String, ByVal defaultValue As DateTime) As DateTime
        Dim value As String = Current.Request(key)
        If String.IsNullOrEmpty(value) Then Return defaultValue
        DateTime.TryParse(value, defaultValue)
        Return defaultValue
    End Function

    Public Shared Function RequestDec(ByVal key As String) As Decimal
        Return RequestDec(key, Decimal.MinValue)
    End Function
    Public Shared Function RequestDec(ByVal key As String, ByVal defaultValue As Decimal) As Decimal
        Dim value As String = Current.Request(key)
        If String.IsNullOrEmpty(value) Then Return defaultValue
        Decimal.TryParse(value, defaultValue)
        Return defaultValue
    End Function

    Public Shared Function RequestDbl(ByVal key As String) As Double
        Return RequestDbl(key, Double.NaN)
    End Function
    Public Shared Function RequestDbl(ByVal key As String, ByVal defaultValue As Double) As Double
        Dim value As String = Current.Request(key)
        If String.IsNullOrEmpty(value) Then Return defaultValue
        Double.TryParse(value, defaultValue)
        Return defaultValue
    End Function

    'Callback
    Public Delegate Sub DCallback()
    Public Shared Function RequestIntOrCallback(ByVal key As String, ByVal callback As DCallback) As Integer
        Dim id As Integer? = RequestIntNullable(key)
        If id.HasValue Then Return id.Value Else callback()
    End Function
    Public Shared Function RequestBoolOrCallback(ByVal key As String, ByVal callback As DCallback) As Boolean
        Dim id As Boolean? = RequestBoolNullable(key)
        If id.HasValue Then Return id.Value Else callback()
    End Function
    Public Shared Function RequestGuidOrCallback(ByVal key As String, ByVal callback As DCallback) As Guid
        Dim id As Guid? = RequestGuidNullable(key)
        If id.HasValue Then Return id.Value Else callback()
    End Function
    Public Shared Function RequestStrOrCallback(ByVal key As String, ByVal callback As DCallback) As String
        Dim id As String = RequestStrNullable(key)
        If IsNothing(id) Then callback()
        Return id
    End Function
    Public Shared Function RequestDateOrCallback(ByVal key As String, ByVal callback As DCallback) As DateTime
        Dim id As DateTime? = RequestDateNullable(key)
        If id.HasValue Then Return id.Value Else callback()
    End Function


    'Nullable-Types
    Public Shared Function RequestIntNullable(ByVal key As String) As Integer?
        Dim value As String = Current.Request(key)
        If Len(value) > 0 Then
            Dim i As Integer
            If Integer.TryParse(value, i) Then Return i
        End If
        Return Nothing
    End Function
    Public Shared Function RequestBoolNullable(ByVal key As String) As Boolean?
        RequestBoolNullable = RequestBool(key)
        If RequestBool(key, True) <> RequestBool(key, False) Then Return Nothing
    End Function
    Public Shared Function RequestGuidNullable(ByVal key As String) As Guid?
        Dim value As String = Current.Request(key)
        If String.IsNullOrEmpty(value) Then Return Nothing
        Try
            Return New Guid(value)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Public Shared Function RequestDateNullable(ByVal key As String) As DateTime?
        Dim value As String = Current.Request(key)
        If String.IsNullOrEmpty(value) Then Return Nothing
        Dim d As DateTime
        If DateTime.TryParse(value, d) Then Return d
        Return Nothing
    End Function
    Public Shared Function RequestStrNullable(ByVal key As String) As String
        Return RequestStr(key, Nothing)
    End Function
    Public Shared Function RequestByteNullable(ByVal key As String) As Byte?
        Dim value As String = Current.Request(key)
        If Len(value) > 0 Then
            Dim i As Byte
            If Byte.TryParse(value, i) Then Return i
        End If
        Return Nothing
    End Function

    'Lists
    Public Shared Function RequestInts(ByVal key As String) As List(Of Integer)
        Dim s As String = Current.Request(key)
        Return CUtilities.StringToListInt(s)
    End Function
    Public Shared Function RequestGuids(ByVal key As String) As List(Of Guid)
        Dim s As String = Current.Request(key)
        If String.IsNullOrEmpty(s) Then Return New List(Of Guid)

        Dim ss As String() = s.Split(CChar(","))
        Dim list As New List(Of Guid)(ss.Length)
        For Each i As String In ss
            list.Add(New Guid(i))
        Next
        Return list
    End Function
    Public Shared Function RequestStrings(ByVal key As String) As List(Of String)
        Dim s As String = Current.Request(key)
        If String.IsNullOrEmpty(s) Then Return New List(Of String)
        Return New List(Of String)(s.Split(CChar(",")))
    End Function

    'Special cases
    Public Shared Function RequestInt_RawUrl(ByVal key As String, ByVal defaultValue As Integer) As Integer
        Dim url As String = Current.Request.RawUrl
        Dim lookFor As String = "?"
        Dim location As Integer = url.IndexOf(lookFor)
        If -1 = location Then Return defaultValue

        Dim querystring As String = url.Substring(location + 1)
        Dim params As String() = querystring.Split(CChar("&"))
        For Each i As String In params
            Dim ss As String() = i.Split(CStr("=").ToCharArray)
            If ss(0).Equals(key) Then
                If ss.Length > 0 Then Integer.TryParse(ss(1), defaultValue)
                Return defaultValue
            End If
        Next
        Return defaultValue
    End Function
#End Region

#Region "Common Javascript"
    Public Shared Sub Alert(ByVal page As Page, ByVal msg As String)
        Register(page, Alert(msg))
    End Sub

    Private Shared Sub Register(ByVal page As Page, ByVal script As String)
        page.ClientScript.RegisterStartupScript(GetType(String), Guid.NewGuid.ToString, script, True)
    End Sub
    Public Shared Sub Redirect(ByVal url As String)
        Current.Response.Write(Location(url))
        Current.Response.End()
    End Sub

    Public Shared Sub ShowIfChecked(ByVal target As WebControl, ByVal ctrlToShow As Control, ByVal clientIdOfCtrlChecked As String, Optional ByVal className As String = "hidden")
        OnClick(target, ShowIfChecked(ctrlToShow, clientIdOfCtrlChecked, className))
    End Sub
    Public Shared Sub ShowIfChecked(ByVal target As HtmlControl, ByVal ctrlToShow As Control, ByVal clientIdOfCtrlChecked As String, Optional ByVal className As String = "hidden")
        OnClick(target, ShowIfChecked(ctrlToShow, clientIdOfCtrlChecked, className))
    End Sub
    Public Shared Function ShowIfChecked(ByVal ctrlToShow As Control, ByVal clientIdOfCtrlChecked As String, Optional ByVal className As String = "hidden") As String
        Return ShowIfChecked(ctrlToShow.ClientID, clientIdOfCtrlChecked, className)
    End Function
    Public Shared Function ShowIfChecked(ByVal clientIdOfCtrlToShow As String, ByVal clientIdOfCtrlChecked As String, Optional ByVal className As String = "hidden") As String
        Return String.Concat(GetById(clientIdOfCtrlToShow), ".className=", GetById(clientIdOfCtrlChecked), ".checked ? '' : '", className, "';")
    End Function

    Public Shared Function GetById(ByVal ctrl As Control) As String
        Return GetById(ctrl.ClientID)
    End Function
    Public Shared Function GetById(ByVal clientIdOfCtrl As String) As String
        Return String.Concat("document.getElementById('", clientIdOfCtrl, "')")
    End Function
#End Region

#Region "Common Attributes"
    Public Shared Sub Tooltip(ByVal ctrl As WebControl, ByVal title As String)
        ctrl.Attributes.Add("title", title)
    End Sub
    Public Shared Sub Tooltip(ByVal ctrl As HtmlControl, ByVal title As String)
        ctrl.Attributes.Add("title", title)
    End Sub

    Public Shared Sub OnClick(ByVal ctrl As WebControl, ByVal jscript As String)
        ctrl.Attributes.Add("onclick", jscript)
    End Sub
    Public Shared Sub OnClick(ByVal ctrl As HtmlControl, ByVal jscript As String)
        ctrl.Attributes.Add("onclick", jscript)
    End Sub

    Public Shared Sub OnClickGoTo(ByVal ctrl As HtmlControls.HtmlControl, ByVal url As String)
        OnClick(ctrl, Location(url))
    End Sub

    Public Shared Sub OnClickConfirm(ByVal ctrl As HtmlControl, ByVal msg As String)
        OnClick(ctrl, Confirm(msg))
    End Sub
    Public Shared Sub OnClickConfirm(ByVal ctrl As WebControl, ByVal msg As String)
        OnClick(ctrl, Confirm(msg))
    End Sub

    Public Shared Function Alert(ByVal msg As String) As String
        Return String.Concat("alert('", Encode(msg), "');")
    End Function
    Public Shared Function Confirm(ByVal msg As String) As String
        Return String.Concat("return confirm('", Encode(msg), "');")
    End Function
    Public Shared Function Location(ByVal url As String) As String
        Return String.Concat("window.location='", url, "'")
    End Function

    Private Shared Function Encode(ByVal msg As String) As String
        Return msg.Replace("'", "\'").Replace(vbCr, "\r").Replace(vbLf, "\n")
    End Function
#End Region

#Region "Disable"
    Public Shared Sub Enable(ByVal ctrl As Control)
        SetEnabled(ctrl, True)
    End Sub
    Public Shared Sub Disable(ByVal ctrl As Control)
        SetEnabled(ctrl, False)
    End Sub
    Public Shared Sub SetEnabled(ByVal ctrl As Control, ByVal isEnabled As Boolean)
        'Set state
        If TypeOf ctrl Is WebControl Then
            CType(ctrl, WebControl).Enabled = isEnabled
        ElseIf TypeOf ctrl Is HtmlButton Then
            CType(ctrl, HtmlButton).Disabled = Not isEnabled
        End If

        'Recurse
        For Each i As Control In ctrl.Controls
            SetEnabled(i, isEnabled)
        Next
    End Sub
#End Region

#Region "Saving Files"
    Public Shared Function UniqueFileName(ByVal folderUrl As String, ByVal upload As FileUpload) As String
        Dim folderPath As String = Current.Server.MapPath(folderUrl)
        Dim fileName As String = CUtilities.UniqueFileName(folderPath, upload.FileName)
        IO.File.WriteAllBytes(String.Concat(folderPath, fileName), upload.FileBytes)
        Return String.Concat(folderUrl, fileName)
    End Function
#End Region

#Region "UI Layout"
    Public Shared Sub EvenUp(ByVal ParamArray c As Control())
        If c.Length < 2 Then Exit Sub

        Dim ctrls As New List(Of Control)()
        For Each i As Control In c
            While i.Controls.Count > 0
                Dim j As Control = i.Controls(i.Controls.Count - 1)
                ctrls.Insert(0, j)
                i.Controls.Remove(j)
            End While
        Next

        Dim size As Integer = CInt(Math.Ceiling(CDbl(ctrls.Count) / CDbl(c.Length)))

        For Each i As Control In c
            While i.Controls.Count < size AndAlso ctrls.Count > 0
                Dim j As Control = ctrls(0)
                i.Controls.Add(j)
                ctrls.RemoveAt(0)
            End While
        Next
    End Sub
#End Region

End Class
