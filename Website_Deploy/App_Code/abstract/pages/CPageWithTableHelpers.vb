Imports Microsoft.VisualBasic

Public Class CPageWithTableHelpers : Inherits CPage

    Protected Function Row(tbl As Table) As TableRow
        Dim tr As New TableRow
        tbl.Rows.Add(tr)
        If tbl.Rows.Count Mod 2 = 0 Then
            tr.CssClass = "alt_row"
        End If
        Return tr
    End Function
    Protected Function RowH(tbl As Table) As TableHeaderRow
        Dim tr As New TableHeaderRow
        tbl.Rows.Add(tr)
        Return tr
    End Function
    Protected Function RowH(tbl As Table, cellText As String, colspan As Integer) As TableHeaderRow
        Dim tr As TableHeaderRow = RowH(tbl)
        CellH(tr, cellText).ColumnSpan = colspan
        Return tr
    End Function
    Protected Function CellLinkH(row As TableRow, s As String, url As String, Optional black As Boolean = True, Optional bold As Boolean = True, Optional ttip As String = Nothing, Optional confirmMsg As String = Nothing) As TableHeaderCell
        Dim td As New TableHeaderCell
        row.Cells.Add(td)
        CellLink(td, s, url, black, bold, ttip, confirmMsg)
        Return td
    End Function
    Protected Function CellLinkR(row As TableRow, s As String, url As String, Optional black As Boolean = True, Optional bold As Boolean = True, Optional ttip As String = Nothing, Optional confirmMsg As String = Nothing) As TableCell
        Dim td As TableCell = CellLink(row, s, url, black, bold, ttip, confirmMsg)
        td.HorizontalAlign = HorizontalAlign.Right
        Return td
    End Function
    Protected Function CellLink(row As TableRow, s As String, url As String, Optional black As Boolean = True, Optional bold As Boolean = True, Optional ttip As String = Nothing, Optional confirmMsg As String = Nothing) As TableCell
        Dim td As New TableCell
        row.Cells.Add(td)
        Dim lnk As HyperLink = CellLink(td, s, url, black, bold, ttip, confirmMsg)
        Return td
    End Function
    Private Function CellLink(td As TableCell, s As String, url As String, black As Boolean, bold As Boolean, Optional ttip As String = Nothing, Optional confirmMsg As String = Nothing) As HyperLink
        If String.IsNullOrEmpty(s) Then Return New HyperLink

        Dim lnk As New HyperLink
        lnk.Text = s
        lnk.NavigateUrl = url
        td.Controls.Add(lnk)

        If Not String.IsNullOrEmpty(ttip) Then lnk.ToolTip = ttip
        If black Then lnk.ForeColor = Drawing.Color.Black
        If bold Then lnk.Font.Bold = True
        If Not IsNothing(confirmMsg) Then lnk.Attributes.Add("onclick", "return confirm('" & confirmMsg.Replace("'", "\'") & "')")
        Return lnk
    End Function
    Protected Function CellR(row As TableRow, Optional text As String = Nothing, Optional ttip As String = Nothing, Optional bold As Boolean = False) As TableCell
        CellR = Cell(row, text, ttip, bold)
        CellR.HorizontalAlign = HorizontalAlign.Right
    End Function
    Protected Function Cell(row As TableRow, Optional text As String = Nothing, Optional ttip As String = Nothing, Optional bold As Boolean = False) As TableCell
        Dim td As New TableCell
        row.Cells.Add(td)
        If Not String.IsNullOrEmpty(text) Then td.Text = text
        If Not String.IsNullOrEmpty(ttip) Then td.ToolTip = ttip
        If bold Then td.Font.Bold = True
        Return td
    End Function
    Protected Function CellH(row As TableRow, Optional text As String = Nothing, Optional ttip As String = Nothing, Optional bold As Boolean = False) As TableHeaderCell
        Dim th As New TableHeaderCell
        row.Cells.Add(th)
        If Not String.IsNullOrEmpty(text) Then th.Text = text
        If Not String.IsNullOrEmpty(ttip) Then th.ToolTip = ttip
        If bold Then th.Font.Bold = True
        Return th
    End Function
End Class
