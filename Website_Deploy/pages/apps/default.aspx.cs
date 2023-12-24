using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;

public partial class pages_Apps_default : CPageDeploy
{
    #region Querystring
    public string Search { get { return CWeb.RequestStr("search"); } }
    #endregion
    
    #region Data
    public CAppList Apps { get { return CApp.Cache.Search(txtSearch.Text); } } 
    #endregion 
    
    #region Event Handlers - Page
    protected override void PageInit()
    {
        //Populate Dropdowns
        
    
        //Search state (from querystring)
        txtSearch.Text = this.Search;
        
        //Display Results
        ctrlApps.Display(this.Apps);

        //Client-side
        this.Form.DefaultFocus  = txtSearch.ClientID;
        this.Form.DefaultButton = btnSearch.UniqueID;   //CTextbox.OnReturnPress(txtSearch, btnSearch);

		MenuAppSearch();
	}
    #endregion

    #region Event Handlers - Form
    protected void btnSearch_Click(object sender, EventArgs e)
    {   
        Response.Redirect(CSitemap.Apps(txtSearch.Text));
    }
    #endregion

    #region Event Handlers - UserControls
    protected void ctrl_AddClick()
    {
        Response.Redirect(CSitemap.AppAdd());
    }
    protected void ctrl_ResortClick(string sortBy, bool descending, int pageNumber)
    {
        Response.Redirect(CSitemap.Apps(txtSearch.Text, new CPagingInfo(0, pageNumber - 1, sortBy, descending)));
    }
    #endregion
}
