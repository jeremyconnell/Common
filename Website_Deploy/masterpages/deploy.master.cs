using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class masterpages_deploy :   CMasterPage
{
    protected override PlaceHolder SideLinks { get { return plhSideLinks; } }
    public override HtmlGenericControl Fieldset { get { return fs; } }
    public override ContentPlaceHolder Main { get { return body; } }
    public override string PageHeading {  get { return hgcHeading.InnerText; } set { hgcHeading.InnerText = value; } }
    public override string MenuDataSourceId {  get { return ctrlMenu.DataSourceID; } set { ctrlMenu.DataSourceID = value; } }
    public override string MenuSideDataSourceID { get { return ctrlSide.DataSourceID; } set { ctrlSide.DataSourceID = value; } }
    public override string MenuTitle { get { return ctrlMenu.Title; } set { ctrlMenu.Title = value; } }
    public override string MenuSelected { get { return ctrlMenu.Selected; } set { ctrlMenu.Selected = value; } }

    public void Page_PreRender(object obj, EventArgs e)
	{
		ctrlMenu.AddRight("Deployments", CSitemap.Clients(), true, "Deployments");
		ctrlMenu.AddRight("System", CSitemap.Users(), false, "System");
    }
    
    public override void AddButton(string url, string tooltip)
    {
        lnk.Visible = true;
        lnk.NavigateUrl = url;
        lnk.ToolTip = tooltip;
    }
    public override void AddMenuLeft(string name, string url, bool selected, string tooltip)
    {
        ctrlMenu.AddLeft(name, url, selected, tooltip);
    }
    public override void AddMenuRight(string name, string url, bool selected, string tooltip)
    {
        ctrlMenu.AddRight(name, url, selected, tooltip);
    }
    public override void AddMenuSide(string name, string url, bool selected, string tooltip)
    {
        ctrlSide.Add(name, url, selected, tooltip);
    }
}
