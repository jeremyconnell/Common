using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Framework;

public partial class pages_self_usercontrols_UCForeignKey : System.Web.UI.UserControl
{
    public void Display(CForeignKey fk, CTableInfo t)
    {
        lblKeyName.Text = CUtilities.Truncate(fk.KeyName, 20);
        lblKeyName.ToolTip = fk.KeyName;

        lblRefTable.Text = fk.ReferenceTable;

        lblColumns.Text = fk.ColumnNames_.Replace(",", "<br/>");
        lblColumns.ToolTip = fk.RefColumnNames_.Replace(",", "\r\n");
    }
    public void Display(CPrimaryKey pk, CTableInfo t)
    {
        lblRefTable.Text = CUtilities.Truncate(pk.KeyName, 20);
        lblRefTable.ToolTip = pk.KeyName;

        lblColumns.Text = CUtilities.ListToHtml(pk.ColumnNames);

        if (pk.IsIdentity)
            lblColumns.Text += " (=" + pk.LastValue + ")";
        else if (pk.ColumnNames.Count == 1)
            lblColumns.Text += "*" + t.Columns[0].Type;
        
    }


    public void Display(CForeignKey fk, CSchemaInfo info, bool detail)
    {
        row.Visible = true;
        div.Visible = false;

        litNumber.Text = Convert.ToString(info.ForeignKeys.IndexOf(fk) + 1);

        if (Parent.Controls.Count % 2 == 0)
            row.Attributes.Add("class", "alt_row");

        lblName.Text = CUtilities.Truncate(fk.KeyName);
        lblName.ToolTip = fk.KeyName;

        litScript.InnerText = fk.CreateScript();

        lblHash.Text = CBinary.ToBase64(fk.MD5, 10);

        lblTable.Text = fk.TableName;
        lblRef.Text = fk.ReferenceTable;

        lblCols.Text = fk.ColumnNames_.Replace(",", "<br/>");
        lblCols.ToolTip = fk.ColumnNames_.Replace(",", "\r\n");

        lblRefCols.Text = fk.RefColumnNames_.Replace(",", "<br/>");
        lblRefCols.ToolTip = fk.RefColumnNames_.Replace(",", "\r\n");

        if (fk.CascadeUpdate) lblCascadeUpdate.Text = "true";
        if (fk.CascadeDelete) lblCascadeDelete.Text = "true";
    }

}