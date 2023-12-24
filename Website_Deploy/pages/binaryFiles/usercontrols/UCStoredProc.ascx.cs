using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Framework;

public partial class pages_binaryFiles_usercontrols_UCStoredProc : System.Web.UI.UserControl
{
    public void Display(CProcedure proc, CSchemaInfo sch, bool detail)
    {
        litNumber.Text = Convert.ToString(sch.Procs.IndexOf(proc) + 1);

        lblProc.Text = CUtilities.Truncate(proc.Name);
        lblProc.ToolTip = proc.Name;

        lblScript.InnerText = proc.Text;

        lblHash.Text = CBinary.ToBase64(proc.MD5, 10);
    }
}