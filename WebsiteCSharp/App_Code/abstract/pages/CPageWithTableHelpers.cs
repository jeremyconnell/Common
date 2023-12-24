using Microsoft.VisualBasic;
using System.Web.UI.WebControls;

public class CPageWithTableHelpers : CPage
{

    protected TableRow Row(Table tbl)
    {
        TableRow tr = new TableRow();
        tbl.Rows.Add(tr);
        if (((tbl.Rows.Count % 2)
                    == 0))
        {
            tr.CssClass = "alt_row";
        }

        return tr;
    }

    protected TableHeaderRow RowH(Table tbl)
    {
        TableHeaderRow tr = new TableHeaderRow();
        tbl.Rows.Add(tr);
        return tr;
    }

    protected TableHeaderRow RowH(Table tbl, string cellText, int colspan)
    {
        TableHeaderRow tr = this.RowH(tbl);
        this.CellH(tr, cellText).ColumnSpan = colspan;
        return tr;
    }

    protected TableHeaderCell CellLinkH(TableRow row, string s, string url, bool black = true, bool bold = true, string ttip = null, string confirmMsg = null)
    {
        TableHeaderCell td = new TableHeaderCell();
        // Warning!!! Optional parameters not supported
        // Warning!!! Optional parameters not supported
        // Warning!!! Optional parameters not supported
        // Warning!!! Optional parameters not supported
        row.Cells.Add(td);
        this.CellLink(td, s, url, black, bold, ttip, confirmMsg);
        return td;
    }

    protected TableCell CellLinkR(TableRow row, string s, string url, bool black = true, bool bold = true, string ttip = null, string confirmMsg = null)
    {
        TableCell td = this.CellLink(row, s, url, black, bold, ttip, confirmMsg);
        // Warning!!! Optional parameters not supported
        // Warning!!! Optional parameters not supported
        // Warning!!! Optional parameters not supported
        // Warning!!! Optional parameters not supported
        td.HorizontalAlign = HorizontalAlign.Right;
        return td;
    }

    protected TableCell CellLink(TableRow row, string s, string url, bool black = true, bool bold = true, string ttip = null, string confirmMsg = null)
    {
        TableCell td = new TableCell();
        // Warning!!! Optional parameters not supported
        // Warning!!! Optional parameters not supported
        // Warning!!! Optional parameters not supported
        // Warning!!! Optional parameters not supported
        row.Cells.Add(td);
        HyperLink lnk = this.CellLink(td, s, url, black, bold, ttip, confirmMsg);
        return td;
    }

    private HyperLink CellLink(TableCell td, string s, string url, bool black, bool bold, string ttip = null, string confirmMsg = null)
    {
        if (string.IsNullOrEmpty(s))
        {
            return new HyperLink();
        }

        // Warning!!! Optional parameters not supported
        // Warning!!! Optional parameters not supported
        HyperLink lnk = new HyperLink();
        lnk.Text = s;
        lnk.NavigateUrl = url;
        td.Controls.Add(lnk);
        if (!string.IsNullOrEmpty(ttip))
        {
            lnk.ToolTip = ttip;
        }

        if (black)
        {
            lnk.ForeColor = System.Drawing.Color.Black;
        }

        if (bold)
        {
            lnk.Font.Bold = true;
        }

        if (!(confirmMsg == null))
        {
            lnk.Attributes.Add("onclick", ("return confirm(\'"
                            + (confirmMsg.Replace("\'", "\\\'") + "\')")));
        }

        return lnk;
    }

    protected TableCell CellR(TableRow row,  string text= null, string ttip = null, bool bold = false)
    {
        var td = this.Cell(row, text, ttip, bold);
        td.HorizontalAlign = HorizontalAlign.Right;
        return td;
    }

    protected TableCell Cell(TableRow row, string text= null, string ttip = null, bool bold = false)
    {
        TableCell td = new TableCell();
        row.Cells.Add(td);
        if (!string.IsNullOrEmpty(text))
        {
            td.Text = text;
        }

        if (!string.IsNullOrEmpty(ttip))
        {
            td.ToolTip = ttip;
        }

        if (bold)
        {
            td.Font.Bold = true;
        }

        return td;
    }

    protected TableHeaderCell CellH(TableRow row, string text= null, string ttip = null, bool bold = false)
    {
        TableHeaderCell th = new TableHeaderCell();
        // Warning!!! Optional parameters not supported
        // Warning!!! Optional parameters not supported
        // Warning!!! Optional parameters not supported
        row.Cells.Add(th);
        if (!string.IsNullOrEmpty(text))
        {
            th.Text = text;
        }

        if (!string.IsNullOrEmpty(ttip))
        {
            th.ToolTip = ttip;
        }

        if (bold)
        {
            th.Font.Bold = true;
        }

        return th;
    }
}