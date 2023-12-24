using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;
using SchemaAdmin.api;
using SchemaAudit;
using Microsoft.WindowsAzure.Management.WebSites.Models;
using System.Drawing;

public partial class pages_instances_Website : CPageDeploy
{
	#region Querystring
	public int InstanceId { get { return CWeb.RequestInt("instanceId"); } }
	public int AppId { get { return null != this.Instance ? this.Instance.InstanceAppId : CWeb.RequestInt("appId"); } }
	#endregion

	#region Data
	public CInstance Instance { get { return CInstance.Cache.GetById(InstanceId); } }
	public CApp App { get { return CApp.Cache.GetById(AppId); } }
	public WebSite AzureWeb
	{
		get
		{
			var i = this.Instance;
			foreach (var j in CAzureManagement.Web.WebSites_Cached())
				if (j.Name.ToLower() == i.InstanceWebNameAzure.ToLower())
					return j;
			return null;
		}
	}
	#endregion

	#region Page Events
	protected override void PageInit()
	{
		ddApp.DataSource = CApp.Cache.WithInstances;
		ddApp.DataBind();
		CDropdown.SetValue(ddApp, AppId);

		ddIns.DataSource = App.Instances;
		ddIns.DataBind();
		CDropdown.SetValue(ddIns, InstanceId);

		ddAzure.DataTextField = "Name";
		ddAzure.DataValueField = "Name";
		ddAzure.DataSource = CAzureManagement.Web.WebSites_Cached();
		ddAzure.DataBind();
		ddAzure.BlankItem("-- Azure Websites --");

		MenuInstanceWebsite(AppId, InstanceId);
	}
	protected override void PageLoad()
	{
		var i = this.Instance;
		txtInstanceWebNameAzure.Text = i.InstanceWebNameAzure;
		txtInstanceWebHostName.Text = i.InstanceWebHostName;
		chkInstanceWebUseSsl.Checked = i.InstanceWebUseSsl;
		txtInstanceWebSubDir.Text = i.InstanceWebSubDir;

		lnkUrl.Text = i.UrlPushUpgrades;
		lnkUrl.NavigateUrl = lnkUrl.Text;
		lnkUrl.Hyperlink.Target = "_Blank";

		ddAzure.Value = i.InstanceWebNameAzure;
		if (ddAzure.SelectedIndex > 0)
		{
			ddAzure.Mode = EControlMode.Disabled;
			txtInstanceWebNameAzure.Mode = EControlMode.Disabled;
			plh.Visible = false;
		}
	}
	protected override void PagePreRender()
	{



		if (null != this.AzureWeb)
		{
			btnCreateWeb.Visible = false;

			var w = AzureWeb;
			txtWebSpaceName.Text = w.WebSpace;
			txtServerFarm.Text = w.ServerFarm;

			txtWebsiteName.Text = w.Name;
			txtWebsiteName.Mode = EControlMode.Locked;
			txtWebsiteName.CtrlLocked_.Font.Bold = true;



			txtAvailState.Text = w.AvailabilityState.ToString();
			txtRuntime.Text = w.RuntimeAvailabilityState.ToString();
			txtUsageState.Text = w.UsageState.ToString();
			txtRuntime.Visible = true;
			txtAvailState.Visible = true;
			txtUsageState.Visible = true;

			if (SetColor(txtRuntime, txtAvailState, txtUsageState))
				CAzureManagement.Web.WebSites_ClearCache();

			txtHostNames.Visible = true;
			txtHostNames.Text = CUtilities.ListToString(w.EnabledHostNames.ToList(), "<br/>\r\n");
			txtWebsiteName.NavigateUrl = "https://" + w.EnabledHostNames[0];
			txtWebsiteName.Hyperlink.Font.Bold = true;
			txtWebsiteName.Hyperlink.ForeColor = Color.Blue;
			txtWebsiteName.Hyperlink.Target = "_blank";




			btnCreateWeb.Visible = false;
		}
		else
		{
			var w = CAzureManagement.Web.WebSite1;  //typical example
			txtWebSpaceName.Text = w.WebSpace.ToString();
			txtServerFarm.Text = w.ServerFarm.ToString();
			CAzureManagement.Sql.List_ClearCache();
			btnRestartWeb.Visible = false;
			btnDeleteWeb.Visible = false;
		}
		var ws = CAzureManagement.Web.WebSpace1;
		txtGeoRegion.Text = string.Concat(ws.GeoRegion, " ~ ", ws.Plan);
		txtWebSpaceName.Mode = EControlMode.Locked;
		txtGeoRegion.Mode = EControlMode.Locked;
		txtServerFarm.Mode = EControlMode.Locked;
		txtHostNames.Mode = EControlMode.Locked;
	}

	private bool SetColor(params usercontrols_extensions_UCTextbox[] txt)
	{
		bool b = false;
		foreach (var i in txt)
			b = SetColor(i) || b;
		return b;
	}
	private bool SetColor(usercontrols_extensions_UCTextbox txt)
	{
		txt.CtrlLocked_.Font.Bold = true;
		if (txt.Text != "Normal")
		{
			txt.CtrlLocked_.ForeColor = Color.Red;
			return true;
		}
		txt.CtrlLocked_.ForeColor = Color.Green;
		return false;
	}
	#endregion

	#region Form Events
	protected void ddApp_SelectedIndexChanged(object sender, EventArgs e)
	{
		var app = CApp.Cache.GetById(CDropdown.GetInt(ddApp));
		Response.Redirect(CSitemap.InstanceWebsite(app.Instances[0].InstanceId), true);
	}
	protected void ddIns_SelectedIndexChanged(object sender, EventArgs e)
	{
		Response.Redirect(CSitemap.InstanceWebsite(CDropdown.GetInt(ddIns)), true);
	}
	protected void btnSave_Click(object sender, EventArgs e)
	{
		var i = this.Instance;
		i.InstanceWebNameAzure = txtInstanceWebNameAzure.Text;
		i.InstanceWebHostName = txtInstanceWebHostName.Text;
		i.InstanceWebUseSsl = chkInstanceWebUseSsl.Checked;
		i.InstanceWebSubDir = txtInstanceWebSubDir.Text;
		i.Save();
		Response.Redirect(Request.RawUrl);
	}


	protected void btnCreateWeb_Click(object sender, EventArgs e)
	{
		var name = txtInstanceWebNameAzure.Text;
		if (string.IsNullOrEmpty(name))
		{
			CSession.PageMessage = "Website Name is Required";
			return;
		}

		try
		{
			var web = CAzureManagement.Web.Create(name);
			CSession.PageMessage = "Created Website: " + web.Name;
			CAzureManagement.Web.WebSites_Cached().Add(web);
			//CAzureManagement.Web.WebSites_ClearCache();
		}
		catch (Exception ex)
		{
			CSession.PageMessageEx = ex;
		}
		Response.Redirect(Request.RawUrl);
	}

	protected void btnRestartWeb_Click(object sender, EventArgs e)
	{
		try
		{
			CAzureManagement.Web.Restart(AzureWeb.Name);
			CSession.PageMessage = "Restarted WebApp";
		}
		catch (Exception ex)
		{
			CSession.PageMessage = "Restart Failed: " + ex.Message;
		}
		Response.Redirect(Request.RawUrl);
	}

	protected void btnDeleteWeb_Click(object sender, EventArgs e)
	{
		try
		{
			CAzureManagement.Web.Delete(AzureWeb);
            CSession.PageMessage = "Deleted WebApp: " + AzureWeb;
        }
		catch (Exception ex)
		{
			CSession.PageMessage = "Delete Failed: " + ex.Message;
		}
		Response.Redirect(Request.RawUrl);
	}
	#endregion

}