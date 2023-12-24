using Microsoft.VisualBasic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public abstract class CMasterPage : MasterPage
{

    protected void Page_PreRender(object sender, System.EventArgs e)
    {
        if (string.IsNullOrEmpty(PageHeading))
        {
            PageHeading = Page.Title;
        }

        if (string.IsNullOrEmpty(MenuTitle))
        {
            MenuTitle = CConfig.MenuTitle();
        }

        if (((Main.Controls.Count == 0) && (FieldSet != null)))
        {
            FieldSet.Visible = false;
        }

        if ((!Page.IsPostBack
                    && CSession.IsLoggedIn))
        {
            SchemaMembership.CClick.Log();
        }

    }

    public abstract ContentPlaceHolder Main { get; }
    public abstract HtmlGenericControl FieldSet { get; }


    public abstract string PageHeading { get; set; }
    public abstract void AddMenuLeft(string name, string url, bool selected, string tooltip );
    public abstract void AddMenuRight(string name, string url, bool selected, string tooltip);
    public abstract void AddMenuSide(string name, string url, bool selected, string tooltip);
    public abstract void AddLinkSide(string name, string url, bool selected, string tooltip);

    public abstract string MenuTitle{ get; set; }
    public abstract string MenuDataSourceId{ get; set; }
    public abstract string MenuSideDataSourceID{ get; set; }
    public abstract string MenuSelected{ get; set; }
    public void UnBindMenu()
    {
        MenuDataSourceId = string.Empty;
        MenuSideDataSourceID = string.Empty;

    }

    public void UnbindSideMenu()
    {
        MenuSideDataSourceID = string.Empty;
    }

    public abstract void AddButton(string url, string tooltip);
}