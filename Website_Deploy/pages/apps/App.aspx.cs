using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;

partial class pages_Apps_App : CPageDeploy
{
    #region Querystring
    public int AppId { get { return CWeb.RequestInt("appId"); } }
    public bool IsEdit { get { return AppId != int.MinValue; } }
    #endregion

    #region Members
    private CApp _app;
    #endregion
    
    #region Data
    public CApp App 
    {
        get 
        {
            if (_app == null) 
            {
                if (IsEdit) 
                {
                    _app = CApp.Cache.GetById(AppId);
                    if (_app == null)
                        CSitemap.RecordNotFound("App", AppId); 
                }
                else 
                {
                    _app = new CApp();
                }
            }
            return _app;
        }
    }
    #endregion
    
    #region Navigation
    private void Refresh()      { Response.Redirect(CSitemap.AppEdit(this.App.AppId)); }
    private void ReturnToList() { Response.Redirect(CSitemap.Apps()); }
    #endregion

    #region Event Handlers - Page
    protected override void PageInit()
    {
        //Populate Dropdowns
        ddApp.DataSource = CApp.Cache;
        ddApp.DataBind();
        CDropdown.BlankItem(ddApp, "-- New App --");
        CDropdown.SetValue(ddApp, AppId);

        ddAppMainVersionId.DataTextField = "VersionName";
        ddAppMainVersionId.DataValueField = "VersionId";
        ddAppMainVersionId.DataSource = App.Versions;
        ddAppMainVersionId.DataBind();
        ddAppMainVersionId.BlankItem("-- Select Version --");

        //Page title
        if (IsEdit) 
            this.Title = "Application";
        else 
            this.Title = "Create New App";

        //Textbox logic
        this.Form.DefaultFocus  = txtAppName.Textbox.ClientID;
        this.Form.DefaultButton = btnSave.UniqueID;   //txtAppName.OnReturnPress(btnSave);


		UnbindSideMenu();
		MenuAppDetails(AppId);


		//Button Text
		btnDelete.Visible = IsEdit;
        if (IsEdit)
        {
			btnCancel.Text = "Back";
            AddButton(CSitemap.AppAdd(), "Create a new App");
            txtAppCreated.Visible = true;
		}
		else
        {
			btnSave.Text = "Create App";
		}

		ddAppMainVersionId.Visible = App.VersionCount > 0;
    }
    protected override void PageLoad()
    {
        LoadApp();
    }
    #endregion

    #region Event Handlers - Form
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!this.IsValid)
            return;

        SaveApp();
    //CCache.ClearCache();  //e.g. if you have more than one application, need to request the clearcache page on the other app
        Refresh();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ReturnToList();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        this.App.Delete();
    //CCache.ClearCache();  //e.g. if you have more than one application, need to request the clearcache page on the other app
        ReturnToList();
    }
    #endregion
    
    #region Private - Load/Save
    protected void LoadApp()
    {

        txtAppName.Text = this.App.AppName;
        ddAppMainVersionId.ValueInt = this.App.AppMainVersionId;
        txtAppKeepOldFilesForDays.ValueInt = this.App.AppKeepOldFilesForDays;
        txtAppCreated.Text = CUtilities.Timespan( this.App.AppCreated);
    }
    protected void SaveApp()
    {
        var a = this.App;
        var oldVerId = a.AppMainVersionId;
        var newVerId = ddAppMainVersionId.ValueInt;

        a.AppName = txtAppName.Text;
        a.AppMainVersionId = newVerId;
        a.AppKeepOldFilesForDays = txtAppKeepOldFilesForDays.ValueInt;

        a.Save();
    }
    #endregion


    protected void ddApp_SelectedIndexChanged(object sender, EventArgs e)
    {
        var appId = CDropdown.GetInt(ddApp);
        if (appId > 0)
            Response.Redirect(CSitemap.AppEdit(appId));
        else
           Response.Redirect(CSitemap.AppAdd());
    }
}
