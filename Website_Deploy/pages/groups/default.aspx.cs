using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;

public partial class pages_Groups_default : CPageDeploy
{
    #region Querystring
    public string Search { get { return CWeb.RequestStr("search"); } }
    public int AppId { get { return CWeb.RequestInt("appId"); } }
    #endregion
    
    #region Data
    public CGroupList Groups { get { return App.Groups.Search(txtSearch.Text); } } 
	public CApp App { get { return CApp.Cache.GetById(AppId); } }
    #endregion 
    
    #region Event Handlers - Page
    protected override void PageInit()
    {
		if (null == App)
			Response.Redirect(CSitemap.Groups((int)EApp.ControlTrack), true);

        //Populate Dropdowns
        ddApp.DataSource = CApp.Cache;
        ddApp.DataBind();
        CDropdown.SetValue(ddApp, AppId);
        
    
        //Search state (from querystring)
        txtSearch.Text = this.Search;
        
        //Display Results
        ctrlGroups.Display(this.Groups);

        //Client-side
        this.Form.DefaultFocus  = txtSearch.ClientID;
        this.Form.DefaultButton = btnSearch.UniqueID;   //CTextbox.OnReturnPress(txtSearch, btnSearch);

		MenuGroupSearch(AppId);
    }
    #endregion

    #region Event Handlers - Form
    protected void btnSearch_Click(object sender, EventArgs e)
    {   
        Response.Redirect(CSitemap.Groups(CDropdown.GetInt(ddApp), txtSearch.Text));
    }
    #endregion

    #region Event Handlers - UserControls
    protected void ctrl_AddClick()
    {
        Response.Redirect(CSitemap.GroupAdd(AppId));
    }
    protected void ctrl_ResortClick(string sortBy, bool descending, int pageNumber)
    {
        Response.Redirect(CSitemap.Groups(AppId, txtSearch.Text, new CPagingInfo(0, pageNumber - 1, sortBy, descending)));
    }
    #endregion
}
