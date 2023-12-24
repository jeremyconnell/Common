using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;
using SchemaAdmin;

public partial class pages_UpgradeHistorys_default : CPageDeploy
{ 
    #region Querystring
    public string Search { get { return CWeb.RequestStr("search"); } }
    public int InstanceId { get { return CWeb.RequestInt("instanceId"); } }
	public int AppId { get { return null != Instance ? Instance.InstanceAppId : CWeb.RequestInt("appId"); } }
    #endregion

    #region Members
    private CUpgradeHistoryList _upgradeHistorys;
	#endregion

	#region Data
	public CApp App { get { return CApp.Cache.GetById(AppId); } }
	public CInstance Instance { get { return CInstance.Cache.GetById(InstanceId); } }
	public SchemaAdmin.CClient Client { get { return null == Instance ? null : SchemaAdmin.CClient.Cache.GetById(Instance.InstanceClientId); } }
	public CUpgradeHistoryList UpgradeHistorys
    {
        get
        {
            //Sql-based Paging
            if (_upgradeHistorys == null)
            {
                _upgradeHistorys = new CUpgradeHistory().SelectSearch(ctrlUpgradeHistorys.PagingInfo, txtSearch.Text, AppId, InstanceId);
                //_upgradeHistorys.PreloadChildren(); //Loads children for page in one hit (where applicable)
            }
            return _upgradeHistorys;
        }
    }
    public DataSet UpgradeHistorysAsDataSet() { return new CUpgradeHistory().SelectSearch_Dataset(txtSearch.Text, AppId, InstanceId); }
    #endregion
    
    #region Event Handlers - Page
    protected override void PageInit()
	{
		if (null == App)
			Response.Redirect(CSitemap.UpgradeHistorys(CApp.Cache.WithInstances[0].AppId), true);
		if (null == Instance)
			Response.Redirect(CSitemap.UpgradeHistorys(AppId, App.Instances[0].InstanceId), true);

		//Populate Dropdowns
		var app = Instance.App;

        //Populate Dropdowns
        ddApp.DataSource = CApp.Cache.WithInstances;
        ddApp.DataBind();
        CDropdown.SetValue(ddApp, AppId);

        ddIns.DataSource = app.Instances;
        ddIns.DataBind();
        CDropdown.SetValue(ddIns, InstanceId);


        //Search state (from querystring)
        txtSearch.Text = this.Search;
        
        //Display Results
        ctrlUpgradeHistorys.Display(this.UpgradeHistorys);

        //Client-side
        this.Form.DefaultFocus  = txtSearch.ClientID;
        this.Form.DefaultButton = btnSearch.UniqueID;   //CTextbox.OnReturnPress(txtSearch, btnSearch);





		UnbindSideMenu();
		MenuAppAutoUpgrades(AppId);



    }
    #endregion 

    #region Event Handlers - Form
    protected void btnSearch_Click(object sender, EventArgs e)
    {   
        Response.Redirect(CSitemap.UpgradeHistorys(AppId, CDropdown.GetInt(ddIns), txtSearch.Text));
	}
	protected void ddApp_SelectedIndexChanged(object sender, EventArgs e)
	{
		Response.Redirect(CSitemap.UpgradeHistorys(CDropdown.GetInt(ddApp)));
	}
	#endregion

	#region Event Handlers - Usercontrols
	protected void ctrl_ExportClick()
    {
        CDataSrc.ExportToCsv(UpgradeHistorysAsDataSet(), Response, "UpgradeHistorys.csv");
    }
    protected void ctrl_ResortClick(string sortBy, bool descending, int pageNumber)
    {
        Response.Redirect(CSitemap.UpgradeHistorys(AppId, InstanceId, txtSearch.Text, new CPagingInfo(0, pageNumber - 1, sortBy, descending)));
    }
    #endregion
}
