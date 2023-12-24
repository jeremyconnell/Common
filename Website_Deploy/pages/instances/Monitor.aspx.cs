using Comms.PushUpgrade.Client;
using Comms.PushUpgrade.Interface;
using Comms.Upgrade.Server;
using Framework;
using Microsoft.WindowsAzure.Management.Sql.Models;
using Microsoft.WindowsAzure.Management.WebSites.Models;
using SchemaAdmin;
using SchemaAdmin.api;
using SchemaDeploy;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pages_clients_Database : CPageDeploy
{
    #region Querystring
    public int InstanceId { get { return CWeb.RequestInt("instanceId"); } }
	public int AppId { get { return null != Instance ? Instance.InstanceAppId : CWeb.RequestInt("appId"); } }
	public string LogFile { get { return CWeb.RequestStr("logFile"); } }
    #endregion

    #region Data
    public CInstance Instance
    {
        get
        {
            return CInstance.Cache.GetById(InstanceId);
        }
	}
	public CApp App { get { return CApp.Cache.GetById(AppId); } }
	public CClient Client { get { return CClient.Cache.GetById(Instance.InstanceClientId); } }

    //Azure
    private Database _database;
    private WebSite _website;
    public Database Database
    {
        get
        {
            if (null == _database)
                foreach (Database i in CAzureManagement.Sql.List_Cached())
                    if (i.Name.ToLower() == Instance.InstanceDbNameAzure)
                    {
                        _database = i;
                        break;
                    }
            return _database;
        }
    }
    public WebSite WebSite
    {
        get
        {
            if (null == _website)
                foreach (WebSite i in CAzureManagement.Web.WebSites_Cached())
                    if (i.Name.ToLower() == Instance.InstanceWebNameAzure)
                    {
                        _website = i;
                        break;
                    }
            return _website;
        }
    }


    public CPushUpgradeClient Push
    {
        get
        {
            var host = WebSite.EnabledHostNames[0] + "/webapi";
            return CPushUpgradeClient.Factory(host, true);
        }
    }
    #endregion

    #region Event Handlers - Page
    protected override void PageInit()
    {
        if (null == Instance)
            Response.Redirect(CSitemap.Instances(EApp.ControlTrack));


        if (null != LogFile && LogFile.Length > 0)
        {
            ctrlMonitor.Display(this.Instance, LogFile);
            return;
        }


		ddApp.DataSource = CApp.Cache;
		ddApp.DataBind();
		CDropdown.SetValue(ddApp, AppId);

        ddInstance.DataSource = App.Instances;
        ddInstance.DataBind();
        CDropdown.SetValue(ddInstance, InstanceId);



        UnbindSideMenu();
		MenuInstanceMonitor(AppId, InstanceId);

		if (null != this.Instance && ! string.IsNullOrEmpty(this.Instance.InstanceWebHostName))
			ctrlMonitor.Display(this.Instance);
		else if (null != Client && Client.Instances.Count > 0)
			Response.Redirect(CSitemap.InstanceMonitor(Client.Instances[0].InstanceId));
		else
			Response.Redirect(CSitemap.InstanceAdd(EApp.ControlTrack, Client?.ClientId??int.MinValue));
	}
	#endregion


	#region Event Handlers - Form
	protected void ddApp_SelectedIndexChanged(object sender, EventArgs e)
	{
		var a = CApp.Cache.GetById(CDropdown.GetInt(ddApp));
		if (a.Instances.Count > 0)
			Response.Redirect(CSitemap.InstanceMonitor(a.Instances[0].InstanceId));
		else
			Response.Redirect(CSitemap.Instances(a.AppId));
	}
    protected void ddInstance_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect(CSitemap.InstanceMonitor(CDropdown.GetInt(ddInstance)));
    }
    #endregion

}