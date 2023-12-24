using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;

public partial class pages_ReportHistorys_default : CPageDeploy
{ 
    #region Querystring
    public string Search { get { return CWeb.RequestStr("search"); } }
    public int InstanceId { get { return CWeb.RequestInt("instanceId"); } }
	public int AppId { get { return null != Instance ? Instance.InstanceAppId : CWeb.RequestInt("appId"); } }
	#endregion

	#region Members
	private CReportHistoryList _reportHistorys;
	#endregion

	#region Data
	public CApp App { get { return CApp.Cache.GetById(AppId); } }
	public CInstance Instance { get { return CInstance.Cache.GetById(InstanceId); } }
	public SchemaAdmin.CClient Client { get { return null == Instance ? null : SchemaAdmin.CClient.Cache.GetById(Instance.InstanceClientId); } }
	public CReportHistoryList ReportHistorys
    {
        get
        {
            //Sql-based Paging
            if (_reportHistorys == null)
            {
                _reportHistorys = new CReportHistory().SelectSearch(ctrlReportHistorys.PagingInfo, txtSearch.Text, AppId, InstanceId);
                //_reportHistorys.PreloadChildren(); //Loads children for page in one hit (where applicable)
            }
            return _reportHistorys;
        }
    }
    public DataSet ReportHistorysAsDataSet() { return new CReportHistory().SelectSearch_Dataset(txtSearch.Text, AppId, InstanceId); }
    #endregion
    
    #region Event Handlers - Page
    protected override void PageInit()
	{
		if (null == App)
			Response.Redirect(CSitemap.ReportHistorys(), true);

		//Populate Dropdowns
        //Populate Dropdowns
        ddApp.DataSource = CApp.Cache.WithInstances;
        ddApp.DataBind();
        CDropdown.SetValue(ddApp, AppId);

        ddIns.DataSource = App.Instances;
        ddIns.DataBind();
		CDropdown.BlankItem(ddIns, "");
        CDropdown.SetValue(ddIns, InstanceId);




		UnbindSideMenu();
		MenuAppReports(AppId);





		//Search state (from querystring)
		txtSearch.Text = this.Search;
        
        //Display Results
        ctrlReportHistorys.Display(this.ReportHistorys);

        //Client-side
        this.Form.DefaultFocus  = txtSearch.ClientID;
        this.Form.DefaultButton = btnSearch.UniqueID;   //CTextbox.OnReturnPress(txtSearch, btnSearch);
    }
    #endregion 

    #region Event Handlers - Form
    protected void btnSearch_Click(object sender, EventArgs e)
    {   
        Response.Redirect(CSitemap.ReportHistorys(AppId, CDropdown.GetInt(ddIns), txtSearch.Text));
	}
	protected void ddApp_SelectedIndexChanged(object sender, EventArgs e)
	{
		Response.Redirect(CSitemap.ReportHistorys(CDropdown.GetInt(ddApp)));
	}
	#endregion

	#region Event Handlers - Usercontrols
	protected void ctrl_ExportClick()
    {
        CDataSrc.ExportToCsv(ReportHistorysAsDataSet(), Response, "ReportHistorys.csv");
    }
    protected void ctrl_ResortClick(string sortBy, bool descending, int pageNumber)
    {
        Response.Redirect(CSitemap.ReportHistorys(AppId, InstanceId, txtSearch.Text, new CPagingInfo(0, pageNumber - 1, sortBy, descending)));
    }
    #endregion
}
