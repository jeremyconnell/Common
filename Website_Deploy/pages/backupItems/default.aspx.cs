using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;

public partial class pages_BackupItems_default : CPageDeploy
{
    #region Querystring
    public string Search { get { return CWeb.RequestStr("search"); } }
    //rename or delete: public int ParentId { get { return CWeb.RequestInt("parentId"); } }

    public int InstanceId { get { return CWeb.RequestInt("instanceId"); } }
    public int AppId { get { return null != Instance && null != Instance.App ? Instance.InstanceAppId : CWeb.RequestInt("appId", (int)EApp.ControlTrack); } }
    public string TableName { get { return CWeb.RequestStr("table"); } }

    #endregion

    #region Members
    private CBackupItemList _backupItems;
    #endregion

    #region Data
    public CApp App { get { return CApp.Cache.GetById(AppId); } }
    public CInstance Instance { get { return CInstance.Cache.GetById(InstanceId); } }
    public CBackupItemList BackupItems
    {
        get 
        {
            if (_backupItems == null)
            {
                _backupItems = new CBackupItem().SelectSearch(ctrlBackupItems.Info, InstanceId, TableName, Search); //Sql-based Paging 
                //_backupItems.PreloadChildren(); //Loads children for page in one hit (where applicable)
            }
            return _backupItems;
        } 
    } 
    public DataSet BackupItemsAsDataSet() { return new CBackupItem().SelectSearch_Dataset(InstanceId, TableName, Search); }
    #endregion 
    
    #region Event Handlers - Page
    protected override void PageInit()
    {
        MenuAppDatasets(App.AppId);

        //Populate Dropdowns
        foreach (var i in new CBackupItem().SelectDistinct("ItemTableName"))
            CDropdown.Add(ddTable, i.Replace("[","").Replace("]", ""), i);
        CDropdown.BlankItem(ddTable, "-- Table --");
        CDropdown.SetValue(ddTable, TableName);

        ddApp.DataSource = CApp.Cache;
        ddApp.DataBind();
        CDropdown.SetValue(ddApp, AppId);

        ddInstance.DataSource = App.Instances;
        ddInstance.DataBind();
        CDropdown.BlankItem(ddInstance, "-- All Deploys --");
        CDropdown.SetValue(ddInstance, InstanceId);


        //Search state (from querystring)
        txtSearch.Text = this.Search;
        
        //Display Results

        //Client-side
        this.Form.DefaultFocus  = txtSearch.ClientID;   //txtCreate.ClientID;
        this.Form.DefaultButton = btnSearch.UniqueID;   //CTextbox.OnReturnPress(txtSearch, btnSearch);
    }
    protected override void PagePreRender()
    {
        ctrlBackupItems.Display(this.BackupItems);
    }
    #endregion 

    #region Event Handlers - Form
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        var appId = CDropdown.GetInt(ddApp);
        var insId = (appId != AppId) ? int.MinValue : CDropdown.GetInt(ddInstance);
        var table = ddTable.SelectedValue;
        var search = txtSearch.Text;
        Response.Redirect(CSitemap.BackupItems(appId, insId, table, search));
    }
    #endregion

    #region Event Handlers - UserControls
    protected void ctrl_ExportClick()
    {
        CDataSrc.ExportToCsv(BackupItemsAsDataSet(), Response, "BackupItems.csv");
    }
    protected void ctrl_ResortClick(string sortBy, bool descending, int pageNumber)
    {
        Response.Redirect(CSitemap.BackupItems(AppId, InstanceId, TableName, Search, new CPagingInfo(0, pageNumber - 1, sortBy, descending)));
    }
    #endregion
}
