using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Framework;

public partial class pages_binaryFiles_usercontrols_UCIndex : System.Web.UI.UserControl
{
    public void Display(CIndexInfo idx, CTableInfo t)
    {
        lblName.Text = CUtilities.Truncate(idx.IndexName, 20);
        lblName.ToolTip = idx.IndexName;

        lblColumns.Text = idx.ColumnNames_.Replace(",", "<br/>");

        if (idx.IsUnique)
            lblKeyExtras.Text = "*Unique";
    }
}