using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Framework;

public partial class pages_self_usercontrols_UCView : System.Web.UI.UserControl
{
    public void Display(CViewInfo view, CSchemaInfo sch)
    {
        litNumber.Text = Convert.ToString(sch.Views.IndexOf(view) + 1);

        lblProc.Text = CUtilities.Truncate(view.ViewName);
        lblProc.ToolTip = view.ViewName;

        lblScript.InnerText = view.Script;

        lblHash.Text = CBinary.ToBase64(view.MD5, 10);


        foreach (var i in view.Columns)
            UCColumn(plhCols).Display(i, view);
    }



    #region User Controls
    private static pages_self_usercontrols_UCColumn UCColumn(Control target)
    {
        Control ctrl = target.Page.LoadControl(CSitemap.UCColumn());
        target.Controls.Add(ctrl);
        return (pages_self_usercontrols_UCColumn)ctrl;
    }
    #endregion
}