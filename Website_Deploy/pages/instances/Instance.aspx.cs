using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using SchemaAdmin;
using Framework;
using Comms.PushUpgrade.Client;

public enum ESuffix
{
    STAGING = 1,
    QA = 2,
    PROD = 3,
}

partial class pages_Instances_Instance : CPageDeploy
{
    #region Querystring
    public int InstanceId { get { return CWeb.RequestInt("instanceId"); } }
    public int ClientId { get { return IsEdit ? Instance.InstanceClientId : CWeb.RequestInt("clientId"); } }
    public int AppId { get { return IsEdit ? Instance.InstanceAppId : CWeb.RequestInt("appId"); } }
    public bool IsEdit { get { return InstanceId != int.MinValue; } }
    #endregion

    #region Members
    private CInstance _instance;
    #endregion
    
    #region Data
    public CInstance Instance 
    {
        get 
        {
            if (_instance == null) 
            {
                if (IsEdit) 
                {
                    _instance = CInstance.Cache.GetById(InstanceId);
                    if (_instance == null)
                        CSitemap.RecordNotFound("Instance", InstanceId); 
                }
                else 
                {
                    _instance = new CInstance();
                    _instance.InstanceAppId = AppId;
                    _instance.InstanceClientId = ClientId;
                    if (null != Client)
						Client.SetClient(_instance);
                }
            }
            return _instance;
        }
    }
    public CClient Client
	{
		get { return CClient.Cache.GetById(ClientId); }
		set
		{
			if (null == value)
			{
				Instance.InstanceClientId = int.MinValue;
				value = new CClient();
			}

			//typical
			value.SetClient(Instance);
		}
	}

    public CValueList Settings {  get { return CValue.Cache.GetByInstanceId(InstanceId); } }
    #endregion
    
    #region Navigation
    private void Refresh()      { Response.Redirect(CSitemap.Instance(this.Instance.InstanceId)); }
    private void ReturnToList() { Response.Redirect(CSitemap.Instances(AppId)); }
    #endregion

    #region Event Handlers - Page
    protected override void PageInit()
    {
        var App = Instance.App;
		if (null == App)
			Response.Redirect(CSitemap.InstanceAdd((int)EApp.ControlTrack), true);

        //Populate Dropdowns
        ddApp.DataSource = CApp.Cache;
        ddApp.DataBind();
        CDropdown.SetValue(ddApp, AppId);


        ddIns.DataSource = App.Instances;
        ddIns.DataBind();
        CDropdown.BlankItem(ddIns, "-- New Instance --");
        CDropdown.SetValue(ddIns, InstanceId);

        ddInstanceClientId.DataTextField = "NameAndInstanceCount";
        ddInstanceClientId.DataValueField = "ClientId";
        ddInstanceClientId.DataSource = CClient.Cache;
        ddInstanceClientId.DataBind();
        ddInstanceClientId.BlankItem("-- Select Client --");

        ddInstanceSuffix.AddEnums(typeof(ESuffix));
        ddInstanceSuffix.BlankItem();



        //Page title
        if (IsEdit) 
            this.Title = "Instance Details";
        else 
            this.Title = "Create New Instance";

        UnbindSideMenu();
		MenuInstanceDetails(AppId, InstanceId, true ||Client != null);
		

		//Textbox logic
		//this.Form.DefaultFocus  = txtInstanceClientName.Textbox.ClientID;
		this.Form.DefaultButton = btnSave.UniqueID;   //txtInstanceClientName.OnReturnPress(btnSave);

        //Button Text
        btnDelete.Visible = IsEdit;
        if (IsEdit)
        {
            btnCancel.Text = "Back";
            AddButton(CSitemap.InstanceAdd(AppId), "Create a new Instance");
			if (int.MinValue != Instance.InstanceClientId)
	            ddInstanceClientId.Mode = EControlMode.Locked;
            ddInstanceClientId.CtrlLocked_.Font.Bold = true;
            if (! string.IsNullOrEmpty(Instance.InstanceSuffix))
            {
                ddInstanceSuffix.Mode = EControlMode.Locked;
                ddInstanceSuffix.CtrlLocked_.Font.Bold = true;
            }

		}
        else
        {
            plhIsEdit.Visible = false;
            btnSave.Text = "Create Instance";
        }
    }
    protected override void PageLoad()
    {
        LoadInstance();
    }
    protected override void PagePreRender()
    {   
    }
    #endregion

    #region Event Handlers - Form
    protected void btnSave_Click(object sender, EventArgs e)
    {


        SaveInstance();
    //CCache.ClearCache();  //e.g. if you have more than one application, need to request the clearcache page on the other app
        Refresh();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ReturnToList();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        this.Instance.Delete();
    //CCache.ClearCache();  //e.g. if you have more than one application, need to request the clearcache page on the other app
        ReturnToList();
    }

    protected void ddApp_SelectedIndexChanged(object sender, EventArgs e)
    {
        var appId = CDropdown.GetInt(ddApp);
		if (!IsEdit)
		{
			Response.Redirect(CSitemap.InstanceAdd(appId, ClientId));
			return;
		}

		var ii = CInstance.Cache;
        if (appId > 0) ii = ii.GetByAppId(appId);
        if (ClientId > 0) ii = ii.GetByClientId(ClientId);

        if (ii.Count > 0)
            Response.Redirect(CSitemap.Instance(ii[0].InstanceId, ClientId));
        else
            Response.Redirect(CSitemap.InstanceAdd(appId, ClientId));

    }
	protected void ddIns_SelectedIndexChanged(object sender, EventArgs e)
    {
        var instanceId = CDropdown.GetInt(ddIns);
        if (instanceId > 0)
            Response.Redirect(CSitemap.Instance(instanceId));
        else
            Response.Redirect(CSitemap.InstanceAdd(CDropdown.GetInt(ddApp), ClientId));
    }
    #endregion

    #region Private - Load/Save
    protected void LoadInstance()
    {
        var i = this.Instance;

        ddInstanceClientId.ValueInt = i.InstanceClientId;
		ddInstanceSuffix.Text = i.InstanceSuffix;

        txtInstanceCreated.Text = CUtilities.Timespan(i.InstanceCreated);
    }
    protected void SaveInstance()
    {
        var i = this.Instance;


		txtMachineName.Text = Instance.InstanceMachineName;

		if (! IsEdit || i.InstanceClientId == int.MinValue)
        {
			if (IsEdit) i.InstanceAppId = AppId;

			i.InstanceClientId = ddInstanceClientId.ValueInt;
			i.InstanceSuffix = ddInstanceSuffix.ValueInt == int.MinValue ? string.Empty : ddInstanceSuffix.Text;

			var c = CClient.Cache.GetById(i.InstanceClientId);
			if (null != c)
				c.SetClient(i);
		}
		else
        {
			if (string.IsNullOrEmpty(i.InstanceSuffix) && ddInstanceSuffix.SelectedIndex > 0)
				i.InstanceSuffix = ddInstanceSuffix.Text;

			if (i.InstanceSpecialVersionId > 0 && string.IsNullOrEmpty(i.InstanceSpecialVersionName))
                i.InstanceSpecialVersionName = null == i.SpecialVersion ? string.Empty : i.SpecialVersion.VersionName;
        }


        i.Save();
    //CCache.ClearCache();  //e.g. if you have more than one application, need to request the clearcache page on the other app

    }
    #endregion
    
}
