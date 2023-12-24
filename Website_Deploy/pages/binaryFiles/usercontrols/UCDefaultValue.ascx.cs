using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pages_binaryFiles_usercontrols_UCDefaultValue :  System.Web.UI.UserControl
{

    public void Display(CDefaultValue defVal, CSchemaInfo sch, bool detail)
    {
        litNumber.Text = Convert.ToString(sch.DefaultValues.IndexOf(defVal) + 1);

        lblProc.Text = CUtilities.Truncate(defVal.Name);
        lblProc.ToolTip = defVal.Name;

        lblScript.InnerText = defVal.Definition;

        lblHash.Text = CBinary.ToBase64(defVal.MD5, 10);
    }
}