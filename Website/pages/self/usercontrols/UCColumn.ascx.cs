using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Framework;

public partial class pages_self_usercontrols_UCColumn : System.Web.UI.UserControl
{
    public void Display(CColumn c, CViewInfo t)
    {
        Display(c, t.Columns, null);
    }
    public void Display(CColumn c, CTableInfo t)
    {
        Display(c, t.Columns, t.PrimaryKey);
    }
    public void Display(CColumn c, CColumnList list, CPrimaryKey pk)
    {
		var idx = list.IndexOf(c);
		if (idx % 2 == 1)
			row.Style.Add("background-color", "#eee");
        litNumber.Text = Convert.ToString(idx + 1);

        lblColumn.Text = c.Name;
        lblColumn.ToolTip = CBinary.ToBase64(c.MD5);
        lblType.Text = c.Type;
        lblNull.Text = !c.IsNullable ? "NOT NULL" : "NULL";

        if (null != pk)
            foreach (var i in pk.ColumnNames)
                if (i.ToLower() == c.Name.ToLower())
                {
                    lblColumn.Font.Bold = true;
                    lblType.Font.Bold = true;
                    lblNull.Font.Bold = true;
                    if (pk.IsIdentity)
                        lblNull.Text = "IDENTITY";
                }
    }
}