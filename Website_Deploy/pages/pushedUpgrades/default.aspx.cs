using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;
using SchemaAdmin;

public partial class pages_PushedUpgrades_default : CPageDeploy
{
    #region Querystring
    public string Search { get { return CWeb.RequestStr("search"); } }
    public int InstanceId { get { return CWeb.RequestInt("instanceId"); } }
	public int AppId { get { return null != Instance ? Instance.InstanceAppId : CWeb.RequestInt("appId"); } }
#endregion

    #region Data
    public CApp App { get { return CApp.Cache.GetById(AppId); } }
    public CInstance Instance { get { return CInstance.Cache.GetById(InstanceId); } }
	public CClient Client { get { return CClient.Cache.GetById(Instance.InstanceClientId); } }
	public CPushedUpgradeList PushedUpgrades { get { return CPushedUpgrade.Cache.Search(txtSearch.Text, AppId, InstanceId); } } 
    #endregion 
    
    #region Event Handlers - Page
    protected override void PageInit()
    {
		if (null == App)
			Response.Redirect(CSitemap.PushedUpgrades(), true);

        //Populate Dropdowns
        ddApp.DataSource = CApp.Cache.WithInstances;
        ddApp.DataBind();
        CDropdown.SetValue(ddApp, AppId);

        ddIns.DataSource = App.Instances;
        ddIns.DataBind();
		CDropdown.BlankItem(ddIns, "-- Any Deployment -- ");
		CDropdown.SetValue(ddIns, InstanceId);


        MenuSelected = null != Instance ? "Deploys" : "Push";

		UnbindSideMenu();
		MenuAppPushUpgrades(AppId);





        //Search state (from querystring)
        txtSearch.Text = this.Search;
        
        //Display Results
        ctrlPushedUpgrades.Display(this.PushedUpgrades);

        //Client-side
        this.Form.DefaultFocus  = txtSearch.ClientID;
        this.Form.DefaultButton = btnSearch.UniqueID;   //CTextbox.OnReturnPress(txtSearch, btnSearch);
    }
    #endregion

    #region Event Handlers - Form
    protected void btnSearch_Click(object sender, EventArgs e)
    {   
        Response.Redirect(CSitemap.PushedUpgrades(CDropdown.GetInt(ddApp), CDropdown.GetInt(ddIns), txtSearch.Text));
    }
    protected void ddApp_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect(CSitemap.PushedUpgrades(CDropdown.GetInt(ddApp)));
    }
    #endregion

    #region Event Handlers - UserControls
    protected void ctrl_AddClick()
    {
        Response.Redirect(CSitemap.PushedUpgradeAdd());
    }
    protected void ctrl_ResortClick(string sortBy, bool descending, int pageNumber)
    {
        Response.Redirect(CSitemap.PushedUpgrades(AppId, InstanceId, txtSearch.Text, new CPagingInfo(0, pageNumber - 1, sortBy, descending)));
    }
    #endregion
}
