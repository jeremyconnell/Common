using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaAdmin;
using Framework;
using SchemaAdmin.client;
using Newtonsoft.Json;
using SchemaAdmin.dto;
using SchemaAdmin.dto.azure;
using SchemaAdmin.api;
using SchemaDeploy;

/*
CREATE USER controltrackadmin WITH PASSWORD = '5uGZ5kjaQ6+z';
ALTER ROLE [db_owner] ADD MEMBER [controltrackadmin]; 
 */


partial class pages_Clients_Client : CPage
{
    #region Querystring
    public int ClientId_ { get { return CWeb.RequestInt("clientId"); } }
    public bool IsEdit { get { return ClientId_ != int.MinValue; } }
    #endregion

    #region Members
    private CClient _client;
    #endregion
    
    #region Data
    public CClient Client 
    {
        get 
        {
            if (_client == null) 
            {
                if (IsEdit) 
                {
                    _client = CClient.Cache.GetById(ClientId_);
                    if (_client == null)
                        CSitemap.RecordNotFound("Client", ClientId_); 
                }
                else 
                {
                    _client = new CClient();
                }
            }
            return _client;
        }
    }
    #endregion
    
    #region Navigation
    private void Refresh()      { Response.Redirect(CSitemap.ClientEdit(this.Client.ClientId)); }
    private void ReturnToList() { Response.Redirect(CSitemap.Clients()); }
    #endregion

    #region Event Handlers - Page
    protected override void PageInit()
    {
        ddClientStatusId.DataTextField = "StatusName";
        ddClientStatusId.DataValueField = "StatusId";
        ddClientStatusId.DataSource = CStatus.Cache;
        ddClientStatusId.DataBind();
        //ddClientStatusId.BlankItem("--  Status --");

        ddClients.DataSource = CClient.Cache;
        ddClients.DataBind();
        CDropdown.BlankItem(ddClients, "-- New Client --");
        CDropdown.SetValue(ddClients, ClientId_);
 

        //Textbox logic
        //this.Form.DefaultFocus  = txtClientName.Textbox.ClientID;
        this.Form.DefaultButton = btnSave.UniqueID;   //txtClientName.OnReturnPress(btnSave);

        //Button Text
        UnbindSideMenu();
        AddMenuSide(CUtilities.NameAndCount("Clients", CClient.Cache), CSitemap.Clients());
        AddMenuSide(IsEdit ? "Client Details" : "New Client...");

        if (IsEdit)
        {
			if (Client.Instances.Count > 0)
				foreach (var i in Client.Instances)
					AddMenuSide(string.Concat(string.IsNullOrEmpty(i.InstanceSuffix) ? "Deployment" : "Deploy - " + i.InstanceSuffix), CSitemap.Instance(i.InstanceId));
			else
				AddMenuSide(CUtilities.NameAndCount("Deploys", Client.Instances), 1 == Client.InstanceCount ? CSitemap.Instance(Client.InstanceFirst.InstanceId) : CSitemap.InstancesByClient(ClientId_));


			AddMenuSide("New Deploy...", CSitemap.InstanceAdd(EApp.ControlTrack, ClientId_));
			AddMenuSide("New Client...", CSitemap.ClientAdd());
			if (Client.Instances.Count == 1)
				AddMenuSide("Schema...", CSitemap.InstanceData(Client.Instances[0].InstanceId));
			else
				AddMenuSide("Schema...", CSitemap.AppSchema());
			AddMenuSide(CUtilities.NameAndCount("States", CStatus.Cache) + "...", CSitemap.Statuss());


			this.Title = "Client Details";

            btnCancel.Text = "Back";
            AddButton(CSitemap.ClientAdd(), "Create a new Client");
        }
        else
        {
            this.Title = "Create New Client";
            btnSave.Text = "Create Client";

            btnDelete.Visible = IsEdit;
        }


    }
    protected override void PageLoad()
    {
        LoadClient();
    }
    #endregion

    #region Event Handlers - Form
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!this.IsValid)
            return;

        SaveClient();
    //CCache.ClearCache();  //e.g. if you have more than one application, need to request the clearcache page on the other app
        Refresh();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ReturnToList();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        this.Client.Delete();
    //CCache.ClearCache();  //e.g. if you have more than one application, need to request the clearcache page on the other app
        ReturnToList();
    }

    protected void ddClients_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect(CSitemap.ClientEdit(CDropdown.GetInt(ddClients)));
    }
    #endregion

    #region Private - Load/Save
    protected void LoadClient()
    {
        var c = Client;

        txtClientName.Text = c.ClientName;
        txtClientEmail.Text = c.ClientEmail;
        txtClientUniqueCode.Text = c.ClientUniqueCode;
        ddClientStatusId.ValueInt = c.ClientStatusId;
        txtClientTrialStarted.Text = CUtilities.Timespan(c.ClientTrialStarted);
		txtClientTrialStarted.ToolTip = CUtilities.LongDateTime(c.ClientTrialStarted);
        txtClientTrialEnded.ValueDate = c.ClientTrialEnded;
        txtClientProductionStarted.ValueDate = c.ClientProductionStarted;
    }
    protected void SaveClient()
    {
        var c = this.Client;

        if (IsEdit && !string.IsNullOrEmpty(c.Code))
        {
            var i = c.InstanceNew();
        }

        c.ClientName = txtClientName.Text;
        c.ClientEmail = txtClientEmail.Text;
        c.ClientUniqueCode = txtClientUniqueCode.Text.ToUpper();
        c.ClientStatusId = ddClientStatusId.ValueInt;
        c.ClientTrialStarted = txtClientTrialStarted.ValueDate;
        c.ClientTrialEnded = txtClientTrialEnded.ValueDate;
        c.ClientProductionStarted = txtClientProductionStarted.ValueDate;

        c.Save();

        if (c.InstanceCount == 0 && !string.IsNullOrEmpty(c.Code))
            c.InstanceNew().Save();

    }
    #endregion






}
