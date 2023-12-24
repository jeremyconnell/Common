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
using Microsoft.WindowsAzure.Management.Sql.Models;
using Microsoft.WindowsAzure.Management.WebSites.Models;
using System.Drawing;
using System.Text;
using SchemaAdmin;

public partial class pages_instances_Database : CPageDeploy
{
	#region Querystring
	public int InstanceId { get { return CWeb.RequestInt("instanceId"); } }
	public int AppId { get { return null != Instance ? Instance.InstanceAppId : CWeb.RequestInt("appId"); } }
	#endregion

	#region Data
	public CInstance Instance { get { return CInstance.Cache.GetById(InstanceId); } }
	public CApp App { get { return CApp.Cache.GetById(AppId); } }
	public CClient Client { get { return CClient.Cache.GetById(Instance.InstanceClientId); } }
	public Database AzureDb
	{
		get
		{
			foreach (var i in CAzureManagement.Sql.List_Cached())
				if (i.Name.ToLower() == Instance.InstanceDbNameAzure.ToLower())
					return i;
			return null;
		}
	}
	public WebSite AzureWeb
	{
		get
		{
			foreach (var i in CAzureManagement.Web.WebSites_Cached())
				if (i.Name.ToLower() == Instance.InstanceWebNameAzure.ToLower())
					return i;
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
		ddAzure.DataSource = CAzureManagement.Sql.List_Cached();
		ddAzure.DataBind();
		ddAzure.BlankItem("-- Azure Databases --");

		MenuInstanceDatabase(AppId, InstanceId);
	}
	protected override void PageLoad()
	{
		var i = this.Instance;
		txtInstanceDbNameAzure.Text = i.InstanceDbNameAzure;
		txtInstanceDbLogin.Text = i.InstanceDbLogin;
		txtInstanceDbUserName.Text = i.InstanceDbUserName;
		txtInstanceDbPassword.Text = i.InstanceDbPassword;
		txtInstanceDbConnectionString.Text = i.InstanceDbConnectionString;


		txtSqlUser.Text = i.Sql_AddUser_Saas;
		txtSqlRole.Text = i.Sql_AddRole_Saas;

		if (null != this.AzureDb)
			if (AzureDb.Name != i.InstanceDbNameAzure)
			{
				i.InstanceDbNameAzure = AzureDb.Name;
				i.Save();
			}

			txtDatabaseName.Text = i.InstanceDbNameAzure;

		ddAzure.Value = i.InstanceDbNameAzure;
		if (ddAzure.SelectedIndex > 0)
		{
			ddAzure.Mode = EControlMode.Disabled;
			txtInstanceDbNameAzure.Mode = EControlMode.Disabled;
			plh.Visible = false;
		}
		else
			plh2.Visible = false;

		if (string.IsNullOrEmpty(i.InstanceDbConnectionString) && null != AzureWeb)
		{
			var w = this.AzureWeb;
			var pp = CAzureManagement.Web.PublishProfile_Cached(w.Name, w.WebSpace);
				foreach (var p in pp)
					if (p.SqlServerConnectionString.Length > 0)
					{
						i.InstanceDbConnectionString = p.SqlServerConnectionString;
						i.Save();
						break;
					}
		}




		if (null == this.AzureDb)
		{
			ddUsers.Visible = false;
			txtEdition.Text = string.Concat(CAzureManagement.Sql.EDITION_NAME, " ~ ", CAzureManagement.Sql.OBJECTIVE);
			txtDatabaseName.Visible = false;
			return;
		}
		btnCreate.Visible = false;

		var db = i.Database;
		if (null == db)
			db = CConfig.SaasDb(i.InstanceDbNameAzure);
		try
		{
			var ss = db.MakeListString("SELECT * FROM sys.database_principals where (type='S' or type = 'U')");
			foreach (var s in ss)
				switch (s)
				{
					case "dbo": break;
					default: ddUsers.Add(s); break;
				}
			//if (ss.Contains(i.InstanceDbUserName))
			//	plhSetup.Visible = false;

		}
		catch (Exception ex)
		{
			ddUsers.Add(ex.Message);
		}


		//plhSetup.Visible = false;

		txtServerName.Mode = EControlMode.Locked;
		txtEdition.Mode = EControlMode.Locked;
		txtDatabaseName.Mode = EControlMode.Locked;

		var d = AzureDb;
		try
		{
			var s = CAzureManagement.Sql.Usage_Cached(d.Name)[0].CurrentValue;
			d.SizeMB = CUtilities.FileSize(long.Parse(s));
		}
		catch { }

		//txtCreated.ValueDateTime = d.CreationDate;
		txtCreated.Text = CUtilities.Timespan(d.CreationDate);
		txtCreated.ToolTip = CUtilities.LongDateTime(d.CreationDate);

		txtState.Text = d.State;
		txtSize.Text = d.SizeMB;
		txtCreated.Visible = true;
		txtState.Visible = txtState.Text.Length > 0;
		txtSize.Visible = txtSize.Text.Length > 0;
		txtSize.CtrlLocked_.Font.Bold = true;
		txtDatabaseName.CtrlLocked_.Font.Bold = true;
		if (SetColor(txtState))
			CAzureManagement.Sql.List_ClearCache();
		txtDatabaseName.CtrlLocked_.ForeColor = txtState.CtrlLocked_.ForeColor;
		txtState.Visible = false;
		txtEdition.Text = string.Concat(d.Edition, " ~ ", CAzureManagement.Sql.OBJECTIVE);

		if (txtInstanceDbConnectionString.Text.Length == 0)
		{
			//btnCreateRole.Enabled = false;
			//btnCreateUser.Enabled = false;
		}

		rbl.Add("SAAS",0);
		rbl.Add(Client.ClientName, 1);
		rbl.ValueInt = 0;
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
			txt.ToolTip = "Error: " + txt.Text;
			txt.CtrlLocked_.ForeColor = Color.Red;
			return true;
		}
		txt.CtrlLocked_.ForeColor = Color.Green;
		txt.ToolTip = "Normal";
		return false;
	}
	#endregion

	#region Form Events
	protected void ddApp_SelectedIndexChanged(object sender, EventArgs e)
	{
		var app = CApp.Cache.GetById(CDropdown.GetInt(ddApp));
		Response.Redirect(CSitemap.InstanceDatabase(app.Instances[0].InstanceId), true);
	}
	protected void ddIns_SelectedIndexChanged(object sender, EventArgs e)
	{
		Response.Redirect(CSitemap.InstanceDatabase(CDropdown.GetInt(ddIns)), true);
	}
	protected void btnSave_Click(object sender, EventArgs e)
	{
		var i = this.Instance;

		i.InstanceDbNameAzure = txtInstanceDbNameAzure.Text;
		i.InstanceDbLogin = txtInstanceDbLogin.Text;
		i.InstanceDbUserName = txtInstanceDbUserName.Text;
		i.InstanceDbPassword = txtInstanceDbPassword.Text;
		i.InstanceDbConnectionString = txtInstanceDbConnectionString.Text;

		i.Save();
		Response.Redirect(Request.RawUrl);
	}
	protected void btnCreate_Click(object sender, EventArgs e)
	{
		var name = txtDatabaseName.Text;
		if (string.IsNullOrEmpty(name))
		{
			CSession.PageMessage = "Database Name is Required";
			return;
		}

		var response = CAzureManagement.Sql.Create(name);
		if (response.IsError)
		{
			CSession.PageMessageEx = response.Error;
			return;
		}
		CSession.PageMessage = "Created Database #" + response.Id + ": " + response.Name;
		CAzureManagement.Sql.List_ClearCache();
		Response.Redirect(Request.RawUrl);
	}

	protected void btnCreateRole_Click(object sender, EventArgs e)
	{
		var i = this.Instance;
		var c = this.Client;

		var sb = new StringBuilder();


		var db = CConfig.SaasDb(i.InstanceDbNameAzure);
		try
		{
			var sql = i.Sql_AddRole_Saas;
			db.ExecuteNonQuery(sql);
			sb.AppendLine(c.Code + ": Added [db_owner] Role to [" + i.InstanceDbUserName + "]");
		}
		catch (Exception ex)
		{
			sb.AppendLine(c.Code + ": Failed to Add [db_owner] Role to  [" + i.InstanceDbUserName + "]: " + ex.Message);
			sb.AppendLine(db.ConnectionString);
		}


		CSession.PageMessage = sb.ToString();
		Response.Redirect(Request.RawUrl);
	}

	protected void btnCreateUser_Click(object sender, EventArgs e)
	{
		var i = this.Instance;
		var c = this.Client;

		var sb = new StringBuilder();

		try
		{
			var db = CConfig.SaasDb(i.InstanceDbNameAzure);
			var sql = i.Sql_AddUser_Saas;
			db.ExecuteNonQuery(sql);
			sb.AppendLine("SAAS: Added Login [" + i.InstanceDbUserName + "@controltracksaas]");
		}
		catch (Exception ex)
		{
			sb.AppendLine("SAAS: Failed to Create Login [" + i.InstanceDbUserName + "]: " + ex.Message); //TODO
		}

		CSession.PageMessage = sb.ToString();
		Response.Redirect(Request.RawUrl);
	}



	protected void btnSelect_Click(object sender, EventArgs e)
	{
		var i = this.Instance;

		CSession.SqlStatement = txtSql.Text;
		CSession.SqlIsSelect = true;
		CSession.SqlUseConn = CConfig.ControlTrackSaas((rbl.ValueInt > 0 ? i.InstanceDbNameAzure : "controltracksaas"));

		iframe.Attributes.Add("src", "~/pages/sql/iframe.aspx");
		divIframe.Visible = true;
	}

	protected void btnUpdate_Click(object sender, EventArgs e)
	{
		var i = this.Instance;
		CSession.SqlStatement = txtSql.Text;
		CSession.SqlIsSelect = false;
		CSession.SqlUseConn = CConfig.ControlTrackSaas((rbl.ValueInt > 0 ? i.InstanceDbNameAzure : "controltracksaas"));

		iframe.Attributes.Add("src", "~/pages/sql/iframe.aspx");
		divIframe.Visible = true;
	}
	protected void btnGuessCS_Click(object sender, EventArgs e)
	{
		Instance.InstanceDbConnectionString = Instance.ConnectionString;
		Instance.Save();
		Response.Redirect(Request.RawUrl);
	}
    #endregion


    protected void btnDeleteDB_Click(object sender, EventArgs e)
    {
        try
        {
            if (AzureDb.Name.ToLower() == "controltracksaas")
                return;
            CAzureManagement.Sql.Delete(AzureDb);
            CSession.PageMessage = "Deleted Database: " + AzureDb.Name;
        }
        catch (Exception ex)
        {
            CSession.PageMessage = "Delete Failed: " + ex.Message;
        }
        Response.Redirect(Request.RawUrl);
    }
}