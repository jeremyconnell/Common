using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Framework;
using System.Web.UI.HtmlControls;
using System.Text;

public partial class pages_self_usercontrols_UCTable : System.Web.UI.UserControl
{
    public void Display(CTableInfo t, CSchemaInfo s, bool detail)
    {
        if (Parent.Controls.Count % 2 == 0)
            row.Attributes.Add("class", "alt_row");



		if (null == t.Indexes)
			t.Indexes = s.Indexes.GetByTable(t.TableName);

		if (null == t.ForeignKeys)
			t.ForeignKeys = s.ForeignKeys.GetByTable(t.TableName);

		litNumber.Text = Convert.ToString(s.Tables.IndexOf(t) + 1);

        lblTable.Text = t.TableName;

        foreach (var i in t.Columns)
            UCColumn(plhCols).Display(i, t);

        UCForeignKey(plhFks).Display(t.PrimaryKey, t);
        if (t.ForeignKeys.Count > 0)
            plhFks.Controls.Add(new HtmlGenericControl("hr"));
        foreach (var i in t.ForeignKeys)
            UCForeignKey(plhFks).Display(i, t);

		foreach (var i in t.Indexes.Unique)
			UCIndex(plhFks).Display(i, t);
		if (t.Indexes.Normal.Count > 0)
			foreach (var i in t.Indexes.Normal)
				UCIndex(plhIdx).Display(i, t);


        lblHash.ToolTip = CBinary.ToBase64(t.MD5);
        lblHash.Text = CUtilities.Truncate(lblHash.ToolTip.ToUpper(), 11).Replace("...", "");

        var sb = new StringBuilder();
        sb.AppendLine(t.CreateScript());

        sb.AppendLine();
        foreach (var i in t.ForeignKeys)
            sb.AppendLine(i.CreateScript());


		sb.AppendLine();
		foreach (var i in t.Indexes)
			sb.AppendLine(i.CreateScript());


        txtScript.InnerText = sb.ToString(); ;
    }


    #region User Controls
    private static pages_self_usercontrols_UCColumn UCColumn(Control target)
    {
        Control ctrl = target.Page.LoadControl(CSitemap.UCColumn());
        target.Controls.Add(ctrl);
        return (pages_self_usercontrols_UCColumn)ctrl;
    }
    private static pages_self_usercontrols_UCForeignKey UCForeignKey(Control target)
    {
        Control ctrl = target.Page.LoadControl(CSitemap.UCForeignKey());
        target.Controls.Add(ctrl);
        return (pages_self_usercontrols_UCForeignKey)ctrl;
    }
    private static pages_self_usercontrols_UCIndex UCIndex(Control target)
    {
        Control ctrl = target.Page.LoadControl(CSitemap.UCIndex());
        target.Controls.Add(ctrl);
        return (pages_self_usercontrols_UCIndex)ctrl;
    }
    #endregion
}