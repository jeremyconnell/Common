using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;

partial class pages_PushedUpgrades_PushedUpgrade : CPage
{
    #region Querystring
    public int PushId { get { return CWeb.RequestInt("pushId"); } }
    public int InstanceId { get { return CWeb.RequestInt("instanceId", PushedUpgrade.Instance.InstanceId); } }
    public bool IsEdit { get { return PushId != int.MinValue; } }
    #endregion

    #region Members
    private CPushedUpgrade _pushedUpgrade;
    #endregion
    
    #region Data
    public CInstance Instance {  get { return CInstance.Cache.GetById(InstanceId); } }
    public CPushedUpgrade PushedUpgrade 
    {
        get 
        {
            if (_pushedUpgrade == null) 
            {
                if (IsEdit) 
                {
                    _pushedUpgrade = CPushedUpgrade.Cache.GetById(PushId);
                    if (_pushedUpgrade == null)
                        CSitemap.RecordNotFound("PushedUpgrade", PushId); 
                }
                else 
                {
                    _pushedUpgrade = new CPushedUpgrade();
                    _pushedUpgrade.PushInstanceId = InstanceId;
                }
            }
            return _pushedUpgrade;
        }
    }
    #endregion
    
    #region Navigation
    private void Refresh()      { Response.Redirect(CSitemap.PushedUpgradeEdit(this.PushedUpgrade.PushId)); }
    private void ReturnToList() { Response.Redirect(CSitemap.PushedUpgrades(InstanceId)); }
    #endregion

    #region Event Handlers - Page
    protected override void PageInit()
    {

        ddPushInstanceId.DataTextField = "InstanceName";
        ddPushInstanceId.DataValueField = "InstanceId";
        ddPushInstanceId.DataSource = CInstance.Cache;
        ddPushInstanceId.DataBind();
        ddPushInstanceId.BlankItem("-- Select Instance --");

        ddPushOldVersionId.DataTextField = "VersionName";
        ddPushOldVersionId.DataValueField = "VersionId";
        ddPushOldVersionId.DataSource = Instance.App.Versions;
        ddPushOldVersionId.DataBind();
        ddPushOldVersionId.BlankItem("-- Old Version --");

        ddPushNewVersionId.DataTextField = "VersionName";
        ddPushNewVersionId.DataValueField = "VersionId";
        ddPushNewVersionId.DataSource = Instance.App.Versions;
        ddPushNewVersionId.DataBind();
        ddPushNewVersionId.BlankItem("-- New Version --");





        //Page title
        if (IsEdit) 
            this.Title = "Pushed-Upgrade Details";
        else 
            this.Title = "Create New Pushed-Upgrade";

        //Textbox logic
        //this.Form.DefaultFocus  = txtPushedUpgradeName.Textbox.ClientID;
        this.Form.DefaultButton = btnSave.UniqueID;   //txtPushedUpgradeName.OnReturnPress(btnSave);

        //Button Text
        btnDelete.Visible = IsEdit;
        if (IsEdit)
        {
            btnCancel.Text = "Back";
            AddButton(CSitemap.PushedUpgradeAdd(), "Create a new Pushed-Upgrade");
        }
        else 
            btnSave.Text = "Create Pushed-Upgrade";
    }
    protected override void PageLoad()
    {
        LoadPushedUpgrade();
    }
    #endregion

    #region Event Handlers - Form
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!this.IsValid)
            return;

        SavePushedUpgrade();
    //CCache.ClearCache();  //e.g. if you have more than one application, need to request the clearcache page on the other app
        Refresh();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ReturnToList();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        this.PushedUpgrade.Delete();
    //CCache.ClearCache();  //e.g. if you have more than one application, need to request the clearcache page on the other app
        ReturnToList();
    }
    #endregion
    
    #region Private - Load/Save
    protected void LoadPushedUpgrade()
    {
        txtPushUserName.Text = this.PushedUpgrade.PushUserName;
        ddPushInstanceId.ValueInt = this.PushedUpgrade.PushInstanceId;
        ddPushOldVersionId.ValueInt = this.PushedUpgrade.PushOldVersionId;
        txtPushOldSchemaMD5.Text = CBinary.ToBase64(this.PushedUpgrade.PushOldSchemaMD5);
        ddPushNewVersionId.ValueInt = this.PushedUpgrade.PushNewVersionId;
        txtPushNewSchemaMD5.Text = CBinary.ToBase64(this.PushedUpgrade.PushNewSchemaMD5);
        txtPushStarted.ValueDate = this.PushedUpgrade.PushStarted;
        txtPushCompleted.ValueDate = this.PushedUpgrade.PushCompleted;
    }
    protected void SavePushedUpgrade()
    {
        this.PushedUpgrade.PushUserName = txtPushUserName.Text;
        this.PushedUpgrade.PushInstanceId = ddPushInstanceId.ValueInt;
        this.PushedUpgrade.PushOldVersionId = ddPushOldVersionId.ValueInt;
        this.PushedUpgrade.PushNewVersionId = ddPushNewVersionId.ValueInt;
        this.PushedUpgrade.PushStarted = txtPushStarted.ValueDate;
        this.PushedUpgrade.PushCompleted = txtPushCompleted.ValueDate;

        this.PushedUpgrade.Save();
    //CCache.ClearCache();  //e.g. if you have more than one application, need to request the clearcache page on the other app

    }
    #endregion
    
}
