using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;
using SchemaAdmin;

public partial class pages_Releases_default : CPageDeploy
{
    #region Querystring
    public string Search { get { return CWeb.RequestStr("search"); } }
    public int AppId { get { return null != Instance ? Instance.InstanceAppId : CWeb.RequestInt("appId"); } }
    public int InstanceId { get { return CWeb.RequestInt("instanceId"); } }
    #endregion

    #region Data
    public CApp App { get { return  CApp.Cache.GetById(AppId); } }
    public CInstance Instance { get { return CInstance.Cache.GetById(InstanceId); } }
	public SchemaAdmin.CClient Client { get { return null == Instance ? null : SchemaAdmin.CClient.Cache.GetById(Instance.InstanceClientId); } }
	public CReleaseList Releases
    {
        get
        {
            return CRelease.Cache.Search(txtSearch.Text, AppId, InstanceId);
        }
    } 
    #endregion 
    
    #region Event Handlers - Page
    protected override void PageInit()
    {
		if (AppId <= 0)
			Response.Redirect(CSitemap.Releases(), true);

        //Populate Dropdowns
        ddApp.DataSource = CApp.Cache;
        ddApp.DataBind();
		CDropdown.SetValue(ddApp, AppId);

        ddIns.DataSource = null == App ? CInstance.Cache : App.Instances;
        ddIns.DataBind();
        CDropdown.BlankItem(ddIns, "-- Main Branch --");
        CDropdown.SetValue(ddIns, InstanceId);


        //Search state (from querystring)
        txtSearch.Text = this.Search;
        
        //Display Results
        ctrlReleases.Display(this.Releases);

        //Client-side
        this.Form.DefaultFocus  = txtSearch.ClientID;
        this.Form.DefaultButton = btnSearch.UniqueID;   //CTextbox.OnReturnPress(txtSearch, btnSearch);


		UnbindSideMenu();
		MenuAppReleases(AppId);

	}
    #endregion

    #region Event Handlers - Form
    protected void btnSearch_Click(object sender, EventArgs e)
    {   
        Response.Redirect(CSitemap.Releases(CDropdown.GetInt(ddApp), CDropdown.GetInt(ddIns), txtSearch.Text));
	}
	protected void ddApp_SelectedIndexChanged(object sender, EventArgs e)
	{
		Response.Redirect(CSitemap.PushedUpgrades(CDropdown.GetInt(ddApp)));
	}
	#endregion

	#region Event Handlers - UserControls
	protected void ctrl_ResortClick(string sortBy, bool descending, int pageNumber)
    {
        Response.Redirect(CSitemap.Releases(AppId, InstanceId, txtSearch.Text, new CPagingInfo(0, pageNumber - 1, sortBy, descending)));
    }
    #endregion
}
