Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls
Imports System.Web.HttpContext

Public Enum EDateRange
    Today
    Yesterday
    WeekToDate
    MonthToDate
    YearToDate
    PreviousWeekToDate
    PreviousMonthToDate
    PreviousYearToDate
    CalendarMonth
    CalendarYear
    PreviousCalendarMonth
    PreviousCalendarYear
End Enum

Public Class CTextbox

#Region "Shared"
    Public Shared Sub RightAlign(ByVal txt As TextBox)
        txt.Style.Add("text-align", "right")
    End Sub
    Public Shared Sub ValidateMoney(ByVal txt As TextBox)
        ValidateNumber(txt)
        txt.Attributes.Add("onfocus", "if (this.value == '') { this.value = '$0.00'; } this.select()")
        txt.Attributes.Add("onblur", "try {this.value=this.value.replace('$', ''); var d = ('' == this.value ? 0 :parseFloat(this.value)); this.value = (0 == this.value ? '' : '$' + this.value); } catch(e) { this.value='';}")
    End Sub
    Public Shared Sub ValidateNumber(ByVal txt As TextBox)
        RightAlign(txt)
        txt.MaxLength = 16
        OnEvent(CType(txt, WebControl), "onfocus", "select()")
        OnKeyPress(CType(txt, WebControl))
    End Sub
    Public Shared Sub ValidateInteger(ByVal txt As TextBox)
        RightAlign(txt)
        txt.MaxLength = 12
        OnEvent(CType(txt, WebControl), "onfocus", "select()")
        OnKeyPress(CType(txt, WebControl), "return ValidateInteger()")
    End Sub
    Public Shared Sub ForceUpperCase(ByVal txt As TextBox)
        OnKeyUp(CType(txt, WebControl), "ForceUpperCase(this)")
    End Sub
    Public Shared Function GetMoney(ByVal txt As TextBox) As Decimal
        Return GetMoney(txt.Text)
    End Function
    Public Shared Function GetMoney(ByVal s As String) As Decimal
        If String.IsNullOrEmpty(s) Then Return Decimal.MinValue
        Dim neg As Boolean = s.Contains("(") AndAlso s.Contains(")")
        Dim d As Double = GetNumber(s.Replace("$", "").Replace("£", "").Replace("(", "").Replace(")", ""))
        If Double.IsNaN(d) Then Return Decimal.MinValue
        If neg Then Return -CDec(d)
        Return CDec(d)
    End Function
    Public Shared Function GetMoneyAsDbl(ByVal txt As TextBox) As Double
        Return AsDbl(GetMoney(txt.Text))
    End Function
    Public Shared Function GetMoneyAsDbl(ByVal s As String) As Double
        Return AsDbl(GetMoney(s))
    End Function
    Public Shared Function GetMoneyAsSingle(ByVal txt As TextBox) As Single
        Return AsSingle(GetMoney(txt.Text))
    End Function
    Public Shared Function GetMoneyAsSingle(ByVal s As String) As Single
        Return AsSingle(GetMoney(s))
    End Function
    Public Shared Function GetPercent(ByVal txt As TextBox) As Double
        Return GetPercent(txt.Text)
    End Function
    Public Shared Function GetPercent(ByVal s As String) As Double
        Return GetNumber(s)
    End Function
    Private Shared Function AsDbl(ByVal f As Decimal) As Double
        If f = Decimal.MinValue Then Return Double.NaN
        Return CDbl(f)
    End Function
    Private Shared Function AsSingle(ByVal f As Decimal) As Single
        If f = Decimal.MinValue Then Return Single.NaN
        Return CType(f, Single)
    End Function
    Public Shared Function GetByte(ByVal txt As TextBox) As Integer
        Return GetByte(txt.Text)
    End Function
    Public Shared Function GetByte(ByVal txt As String) As Byte
        Dim i As Byte
        If Byte.TryParse(txt, i) Then Return i
        Return 0
    End Function
    Public Shared Function GetInteger(ByVal txt As TextBox) As Integer
        Return GetInteger(txt.Text)
    End Function
    Public Shared Function GetInteger(ByVal s As String) As Integer
        If String.IsNullOrEmpty(s) Then Return Integer.MinValue
        s = s.Replace(",", "")
        If s.Contains(".") Then
            s = s.Substring(0, s.IndexOf("."))
        End If

        Dim i As Integer
        If Integer.TryParse(s, i) Then Return i
        Return Integer.MinValue
    End Function
    Public Shared Function GetLong(ByVal txt As TextBox) As Long
        Return GetLong(txt.Text)
    End Function
    Public Shared Function GetLong(ByVal s As String) As Long
        If String.IsNullOrEmpty(s) Then Return Long.MinValue
        Dim i As Long
        If Long.TryParse(s.Replace(",", String.Empty), i) Then Return i
        Return Long.MinValue
    End Function
    Public Shared Function GetNumber(ByVal txt As TextBox) As Double
        Return GetNumber(txt.Text)
    End Function
    Public Shared Function GetNumber(ByVal s As String) As Double
        If String.IsNullOrEmpty(s) Then Return Double.NaN
        s = Trim(s.Replace("$", "").Replace("£", "").Replace("%", "").Replace(",", ""))
        If Len(s) = 0 Then Return Double.NaN
        Dim d As Double
        If Double.TryParse(s, d) Then Return d
        Return Double.NaN
    End Function
    Public Shared Function GetNumberAsSingle(ByVal txt As TextBox) As Single
        Return GetNumberAsSingle(txt.Text)
    End Function
    Public Shared Function GetNumberAsSingle(ByVal s As String) As Single
        If String.IsNullOrEmpty(s) Then Return Single.NaN
        s = Trim(s.Replace("$", "").Replace("£", "").Replace("%", ""))
        If Len(s) = 0 Then Return Single.NaN
        Dim d As Single
        If Single.TryParse(s, d) Then Return d
        Return Single.NaN
    End Function
    Public Shared Function GetDate(ByVal txt As TextBox) As DateTime
        Return GetDate(txt.Text)
    End Function
    Public Shared Function GetDate(ByVal s As String) As DateTime
        s = Trim(s)
        If Len(s) = 0 Then Return DateTime.MinValue
        s = s.Replace("3rd", "3").Replace("2nd", "2").Replace("1st", "1")
        Dim d As DateTime
        If DateTime.TryParse(s, d) Then Return d
        Return DateTime.MinValue
    End Function
    Public Shared Sub SetMoney(ByVal txt As TextBox, ByVal amount As Decimal)
        txt.Text = SetMoney(amount)
    End Sub
    Public Shared Sub SetMoney(ByVal txt As TextBox, ByVal amount As Double)
        txt.Text = SetMoney(amount)
    End Sub
    Public Shared Sub SetMoney(ByVal txt As TextBox, ByVal amount As Single)
        txt.Text = SetMoney(amount)
    End Sub
    Public Shared Sub SetMoney(ByVal txt As TextBox, ByVal amount As Integer)
        txt.Text = SetMoney(amount)
    End Sub
    Public Shared Function SetMoney(ByVal amount As Decimal) As String
        If amount = Decimal.MinValue Then Return String.Empty
        Return amount.ToString("C")
    End Function
    Public Shared Function SetMoney(ByVal amount As Double) As String
        If Double.IsNaN(amount) Then Return String.Empty
        Return amount.ToString("C")
    End Function
    Public Shared Function SetMoney(ByVal amount As Single) As String
        If Single.IsNaN(amount) Then Return String.Empty
        Return amount.ToString("C")
    End Function
    Public Shared Function SetMoney(ByVal amount As Integer) As String
        If Integer.MinValue = amount Then Return String.Empty
        Return amount.ToString("C")
    End Function
    Public Shared Sub SetPercent(ByVal txt As TextBox, ByVal amount As Decimal)
        txt.Text = SetPercent(amount)
    End Sub
    Public Shared Sub SetPercent(ByVal txt As TextBox, ByVal amount As Double)
        txt.Text = SetPercent(amount)
    End Sub
    Public Shared Sub SetPercent(ByVal txt As TextBox, ByVal amount As Single)
        txt.Text = SetPercent(amount)
    End Sub
    Public Shared Sub SetPercent(ByVal txt As TextBox, ByVal amount As Integer)
        txt.Text = SetPercent(amount)
    End Sub
    Public Shared Function SetPercent(ByVal amount As Decimal) As String
        If Decimal.MinValue = amount Then Return String.Empty
        Return amount.ToString() & "%"
    End Function
    Public Shared Function SetPercent(ByVal amount As Double) As String
        If Double.IsNaN(amount) Then Return String.Empty
        Return amount.ToString() & "%"
    End Function
    Public Shared Function SetPercent(ByVal amount As Single) As String
        If Single.IsNaN(amount) Then Return String.Empty
        Return amount.ToString() & "%"
    End Function
    Public Shared Function SetPercent(ByVal amount As Integer) As String
        If Integer.MinValue = amount Then Return String.Empty
        Return amount.ToString() & "%"
    End Function
    Public Shared Sub SetPercent(ByVal txt As TextBox, ByVal amount As Double, ByVal format As String)
        txt.Text = SetPercent(amount, format)
    End Sub
    Public Shared Sub SetPercent(ByVal txt As TextBox, ByVal amount As Single, ByVal format As String)
        txt.Text = SetPercent(amount, format)
    End Sub
    Public Shared Sub SetPercent(ByVal txt As TextBox, ByVal amount As Decimal, ByVal format As String)
        txt.Text = SetPercent(amount, format)
    End Sub
    Public Shared Sub SetPercent(ByVal txt As TextBox, ByVal amount As Integer, ByVal format As String)
        txt.Text = SetPercent(amount, format)
    End Sub
    Public Shared Function SetPercent(ByVal amount As Decimal, ByVal format As String) As String
        If Decimal.MinValue = amount Then Return String.Empty
        Return amount.ToString(format) & "%"
    End Function
    Public Shared Function SetPercent(ByVal amount As Double, ByVal format As String) As String
        If Double.IsNaN(amount) Then Return String.Empty
        Return amount.ToString(format) & "%"
    End Function
    Public Shared Function SetPercent(ByVal amount As Single, ByVal format As String) As String
        If Single.IsNaN(amount) Then Return String.Empty
        Return amount.ToString(format) & "%"
    End Function
    Public Shared Function SetPercent(ByVal amount As Integer, ByVal format As String) As String
        If Integer.MinValue = amount Then Return String.Empty
        Return amount.ToString(format) & "%"
    End Function
    Public Shared Sub SetNumber(ByVal txt As TextBox, ByVal amount As Decimal)
        txt.Text = SetNumber(amount)
    End Sub
    Public Shared Sub SetNumber(ByVal txt As TextBox, ByVal amount As Double)
        txt.Text = SetNumber(amount)
    End Sub
    Public Shared Sub SetNumber(ByVal txt As TextBox, ByVal amount As Single)
        txt.Text = SetNumber(amount)
    End Sub
    Public Shared Sub SetNumber(ByVal txt As TextBox, ByVal amount As Integer)
        txt.Text = SetNumber(amount)
    End Sub
    Public Shared Sub SetNumber(ByVal txt As TextBox, ByVal amount As Long)
        txt.Text = SetNumber(amount)
    End Sub
    Public Shared Function SetNumber(ByVal amount As Decimal) As String
        If amount = Decimal.MinValue Then Return String.Empty
        Return amount.ToString()
    End Function
    Public Shared Function SetNumber(ByVal amount As Double) As String
        If Double.IsNaN(amount) Then Return String.Empty
        Return amount.ToString()
    End Function
    Public Shared Function SetNumber(ByVal amount As Single) As String
        If Single.IsNaN(amount) Then Return String.Empty
        Return amount.ToString()
    End Function
    Public Shared Function SetNumber(ByVal amount As Integer) As String
        If amount = Integer.MinValue Then Return String.Empty
        Return amount.ToString("n0")
    End Function
    Public Shared Function SetNumber(ByVal amount As Long) As String
        If amount = Long.MinValue Then Return String.Empty
        Return amount.ToString()
    End Function
    Public Shared Sub SetDate(ByVal txt As TextBox, ByVal value As DateTime)
        txt.Text = SetDate(value)
    End Sub
    Public Shared Sub SetDate(ByVal txt As TextBox, ByVal value As DateTime, ByVal format As String)
        txt.Text = SetDate(value, format)
    End Sub
    Public Shared Function SetDate(ByVal value As DateTime) As String
        Return SetDate(value, "dd MMM yyyy")
    End Function
    Public Shared Function SetDate(ByVal value As DateTime, ByVal format As String) As String
        If DateTime.MinValue = value Then Return String.Empty
        Return value.ToString(format)
    End Function

    Public Shared Sub SetDates(ByVal range As EDateRange, ByVal txtFromDate As TextBox, ByVal txtToDate As TextBox)
        Dim fromDate As DateTime
        Dim toDate As DateTime
        SetDates(range, fromDate, toDate)
        SetDate(txtFromDate, fromDate)
        SetDate(txtToDate, toDate)
    End Sub
    Public Shared Sub SetDates(ByVal range As EDateRange, ByRef fromDate As DateTime, ByRef toDate As DateTime)
        Dim d As DateTime = DateTime.Now.Date
        Select Case range
            Case EDateRange.CalendarMonth
                d = New DateTime(d.Year, d.Month, 1)
                fromDate = d
                toDate = d.AddMonths(1).AddDays(-1)
                Exit Sub

            Case EDateRange.CalendarYear
                d = New DateTime(d.Year, 1, 1)
                fromDate = d
                toDate = d.AddYears(1).AddDays(-1)
                Exit Sub

            Case EDateRange.MonthToDate
                fromDate = d.AddDays(-30)
                toDate = DateTime.MinValue
                Exit Sub

            Case EDateRange.PreviousCalendarMonth
                d = New DateTime(d.Year, d.Month, 1).AddMonths(-1)
                fromDate = d
                toDate = d.AddMonths(1).AddDays(-1)
                Exit Sub

            Case EDateRange.PreviousCalendarYear
                d = New DateTime(d.Year, 1, 1).AddYears(-1)
                fromDate = d
                toDate = d.AddYears(1).AddDays(-1)
                Exit Sub

            Case EDateRange.PreviousMonthToDate
                d = d.AddDays(-30)
                fromDate = d.AddDays(-30)
                toDate = d
                Exit Sub

            Case EDateRange.PreviousWeekToDate
                d = d.AddDays(-7)
                fromDate = d.AddDays(-7)
                toDate = d
                Exit Sub

            Case EDateRange.PreviousYearToDate
                d = d.AddDays(-365)
                fromDate = d.AddDays(-365)
                toDate = d
                Exit Sub

            Case EDateRange.Today
                fromDate = d
                toDate = DateTime.MinValue

            Case EDateRange.WeekToDate
                fromDate = d.AddDays(-7)
                toDate = DateTime.MinValue
                Exit Sub

            Case EDateRange.YearToDate
                fromDate = d.AddDays(-365)
                toDate = DateTime.MinValue
                Exit Sub

            Case EDateRange.Yesterday
                d = d.AddDays(-1)
                fromDate = d
                toDate = d
                Exit Sub

            Case Else
                fromDate = Nothing
                toDate = Nothing
                'Throw New Exception("Unknown EDateRange: " & range)
        End Select
    End Sub
#End Region

#Region "Shared - Nullable"
    Public Shared Function GetIntegerNullable(ByVal txt As TextBox) As Integer?
        Return GetIntegerNullable(txt.Text)
    End Function
    Public Shared Function GetIntegerNullable(ByVal s As String) As Integer?
        If String.IsNullOrEmpty(s) Then Return Nothing
        Dim i As Integer
        If Integer.TryParse(s, i) Then Return i
        Return Nothing
    End Function

    Public Shared Function GetNumberNullable(ByVal txt As TextBox) As Double?
        Return GetNumberNullable(txt.Text)
    End Function
    Public Shared Function GetNumberNullable(ByVal s As String) As Double?
        If String.IsNullOrEmpty(s) Then Return Nothing
        s = Trim(s.Replace("$", "").Replace("£", ""))
        If Len(s) = 0 Then Return Nothing
        Dim d As Double
        If Double.TryParse(s, d) Then Return d
        Return Nothing
    End Function

    Public Shared Function GetMoneyNullable(ByVal txt As TextBox) As Decimal?
        Return GetMoneyNullable(txt.Text)
    End Function
    Public Shared Function GetMoneyNullable(ByVal s As String) As Decimal?
        If String.IsNullOrEmpty(s) Then Return Nothing
        Dim d As Double? = GetNumberNullable(s.Replace("$", "").Replace("£", ""))
        If Not d.HasValue Then Return Nothing
        Return CDec(d.Value)
    End Function

    Public Shared Function GetMoneyAsDblNullable(ByVal txt As TextBox) As Double?
        Return GetMoneyAsDblNullable(txt.Text)
    End Function
    Public Shared Function GetMoneyAsDblNullable(ByVal s As String) As Double?
        Return GetNumberNullable(s.Replace("$", "").Replace("£", ""))
    End Function

    Public Shared Function GetDateNullable(ByVal txt As TextBox) As DateTime?
        Return GetDateNullable(txt.Text)
    End Function
    Public Shared Function GetDateNullable(ByVal s As String) As DateTime?
        If String.IsNullOrEmpty(s) Then Return Nothing
        s = Trim(s)
        If Len(s) = 0 Then Return Nothing
        Dim d As DateTime
        If DateTime.TryParse(s, d) Then Return d
        Return Nothing
    End Function

    Public Shared Sub SetMoney(ByVal txt As TextBox, ByVal amount As Decimal?)
        txt.Text = SetMoney(amount)
    End Sub
    Public Shared Function SetMoney(ByVal amount As Decimal?) As String
        If Not amount.HasValue Then Return String.Empty
        Return amount.Value.ToString("C")
    End Function
    Public Shared Sub SetMoney(ByVal txt As TextBox, ByVal amount As Double?)
        txt.Text = SetMoney(amount)
    End Sub
    Public Shared Function SetMoney(ByVal amount As Double?) As String
        If Not amount.HasValue Then Return String.Empty
        Return amount.Value.ToString("C")
    End Function
    Public Shared Sub SetNumber(ByVal txt As TextBox, ByVal amount As Double?)
        txt.Text = SetNumber(amount)
    End Sub
    Public Shared Function SetNumber(ByVal amount As Double?) As String
        If Not amount.HasValue Then Return String.Empty
        Return amount.Value.ToString()
    End Function
    Public Shared Sub SetNumber(ByVal txt As TextBox, ByVal amount As Integer?)
        txt.Text = SetNumber(amount)
    End Sub
    Public Shared Function SetNumber(ByVal amount As Integer?) As String
        If Not amount.HasValue Then Return String.Empty
        Return amount.Value.ToString()
    End Function
    Public Shared Sub SetDate(ByVal txt As TextBox, ByVal value As DateTime?)
        txt.Text = SetDate(value)
    End Sub
    Public Shared Sub SetDate(ByVal txt As TextBox, ByVal value As DateTime?, ByVal format As String)
        txt.Text = SetDate(value, format)
    End Sub
    Public Shared Function SetDate(ByVal amount As DateTime?) As String
        Return SetDate(amount, "dd MMM yyyy")
    End Function
    Public Shared Function SetDate(ByVal amount As DateTime?, ByVal format As String) As String
        If Not amount.HasValue Then Return String.Empty
        Return amount.Value.ToString(format)
    End Function
#End Region

#Region "Attributes.Add"
    Public Shared Sub Confirm(ByVal target As WebControl, ByVal msg As String)
        OnClick(target, "return confirm('" & Escape(msg) & "')")
    End Sub
    Public Shared Sub OnEvent(ByVal target As WebControl, ByVal eventName As String, ByVal script As String)
        target.Attributes.Add(eventName, script)
    End Sub
    Public Shared Sub OnClick(ByVal target As WebControl, ByVal script As String)
        OnEvent(target, "onclick", script)
    End Sub
    Public Shared Sub OnKeyUp(ByVal target As WebControl, Optional ByVal script As String = "return ValidateNumber()")
        OnEvent(target, "onkeyup", script)
    End Sub
    Public Shared Sub OnKeyPress(ByVal target As WebControl, Optional ByVal script As String = "return ValidateNumber()")
        OnEvent(target, "onkeypress", script)
    End Sub
    Public Shared Sub OnReturnPress(ByVal textBox As WebControl, Optional ByVal button As WebControl = Nothing)
        Dim action As String = "__doPostBack('" & textBox.UniqueID & "','')"
        If Not IsNothing(button) Then action = DhtmlName(button) & ".focus(); " & DhtmlName(button) & ".click()"
        textBox.Attributes.Add("onkeypress", "if (13 == event.keyCode) { " & action & "; return false; }")
    End Sub
    Public Shared Sub OnDblClick(ByVal target As WebControl, ByVal script As String)
        target.Attributes.Add("ondblclick", script)
    End Sub
    Public Shared Sub OnFocus(ByVal target As WebControl, ByVal script As String)
        target.Attributes.Add("onfocus", script)
    End Sub
    Public Shared Sub OnFocusSelect(ByVal target As WebControl)
        target.Attributes.Add("onfocus", "this.select()")
    End Sub

#Region "Common Overloads"
    Public Shared Sub OnDblClick(ByVal target As WebControl, ByVal clickMe As WebControl)
        target.Attributes.Add("ondblclick", clickMe.UniqueID & ".click()")
    End Sub
#End Region

#End Region

#Region "Utilities"
    Public Shared Function DhtmlId(ByVal target As HtmlControl, Optional ByVal includeDocumentDotAll As Boolean = True) As String
        Dim id As String = Replace(target.UniqueID, ":", "_")
        If includeDocumentDotAll Then Return DhtmlName(id)
        Return id
    End Function
    Public Shared Function DhtmlId(ByVal target As WebControl, Optional ByVal includeDocumentDotAll As Boolean = True) As String
        Dim id As String = Replace(target.UniqueID, ":", "_")
        If includeDocumentDotAll Then Return DhtmlName(id)
        Return id
    End Function
    Public Shared Function DhtmlName(ByVal id As String) As String
        Return "document.getElementById('" & id & "')"
    End Function
    Public Shared Function DhtmlName(ByVal target As WebControl) As String
        Return DhtmlName(target.ClientID)
    End Function
    Public Shared Function RequestOrDefault(ByVal formElementName As String, ByVal defaultValue As String) As String
        Dim s As String = Current.Request(formElementName)
        If Len(s) > 0 Then Return s
        Return defaultValue
    End Function
    Public Shared Function RequestArray(ByVal formElementName As String) As String()
        Try
            Return Current.Request("ddFiles").Split(CStr(",").ToCharArray)
        Catch ex As System.NullReferenceException
            Return New String() {}
        End Try
    End Function
    Public Shared Function Escape(ByVal str As String, Optional ByVal character As String = "'", Optional ByVal escaped As String = "\'") As String
        Return Replace(str, character, escaped)
    End Function
#End Region

End Class
