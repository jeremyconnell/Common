using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;
using System.Data;

public partial class pages_instances_AppLogin : CPageDeploy
{
	#region Querystring
	public int InstanceId { get { return CWeb.RequestInt("instanceId"); } }
	public int AppId { get { return null != Instance ? Instance.InstanceAppId : CWeb.RequestInt("appId"); } }
	public string UserName { get { return CWeb.RequestStr("userName", "admin"); } }
	#endregion

	#region Data
	public CInstance Instance { get { return CInstance.Cache.GetById(InstanceId); } }
	public CApp App { get { return CApp.Cache.GetById(AppId); } }
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

		MenuInstanceAppLogin(AppId, InstanceId);


		ShowUser();
	}
	protected override void PageLoad()
	{
		var i = this.Instance;
		txtInstanceAppLogin.Text = i.InstanceAppLogin;
		txtInstanceAppPassword.Text = i.InstanceAppPassword;

	}




	#endregion
	private int _userId;
	private const string TABLE = "[OS].[User]";
	private const string PK = "UserID";
	private const string LOGIN = "UserName";
	private void ShowUser()
	{
		try
		{
			var i = this.Instance;

			plh.Visible = true;

			var userNames = CSession.get_Instance_UserLogins(InstanceId);
			if (null == userNames)
				userNames = i.DatabaseDirectOrWeb.MakeListString(string.Concat("SELECT ", LOGIN, " from " , TABLE));
			foreach (var userName in userNames)
				CDropdown.Add(ddLogin, CUtilities.Truncate(userName), userName);
			CDropdown.BlankItem(ddLogin, "-- UserName --");
			CDropdown.SetValue(ddLogin, UserName);
			lbl.Text = "Logins (" + i.NameAndSuffix + "): ";


			var dt = i.DatabaseDirectOrWeb.SelectWhere_Dataset(TABLE, new CCriteriaList(LOGIN, UserName)).Tables[0];
			foreach (DataRow dr in dt.Rows)
			{
				foreach (DataColumn c in dt.Columns)
				{
					var tr = Row(tbl);

					var th = CellH(tr, c.ColumnName);
					var obj = dr[c.ColumnName];
					TableCell td = null;
					if (obj is DateTimeOffset)
					{
						var dto = (DateTimeOffset)obj;
						td = Cell(tr, CUtilities.Timespan(dto.DateTime));
					}
					else if (obj is byte[])
					{
						var bytes = CAdoData.GetBytes(dr, c.ColumnName);
						td = Cell(tr, CUtilities.CountSummary(bytes, "byte"));
					}
					else
						td = Cell(tr, CAdoData.GetStr(dr, c.ColumnName));

					switch (c.ColumnName)
					{
						case "IsSysAdministrator":
						case "IsAccAdministrator":
						case "IsLocked":
						case "IsActive":
							var chk = new CheckBox();
							td.Text = string.Empty;
							td.Controls.Add(chk);

							chk.ID = "chk_" + c.ColumnName;
							chk.Text = c.ColumnName;
							chk.Checked = CAdoData.GetBool(dr, c.ColumnName);
							chk.ToolTip = c.ColumnName;
							chk.AutoPostBack = true;
							chk.CheckedChanged += Chk_CheckedChanged;
							break;

						case PK:
							_userId = CAdoData.GetInt(dr, PK);
							break;

						case "PasswordHash":
							th.RowSpan = 2;
							tr = Row(tbl);
							td = Cell(tr);
							var dd = new DropDownList();
							td.Controls.Add(dd);

							dd.Font.Size = new FontUnit(FontSize.Smaller);
							dd.DataTextField = "NameAndPass";
							dd.DataValueField = "InstanceId";
							dd.DataSource = i.App.Instances;
							dd.DataBind();
							CDropdown.BlankItem(dd, "-- Copy From --");
							dd.AutoPostBack = true;
							dd.SelectedIndexChanged += Dd_SelectedIndexChanged;
							break;
					}
				}
			}
		}
		catch (Exception ex)
		{
			CSession.PageMessageEx = ex;
			plh.Visible = false;
		}
	}

	#region Form Events

	private void Dd_SelectedIndexChanged(object sender, EventArgs e)
	{
		var dd = (DropDownList)sender;

		var id = CDropdown.GetInt(dd);
		var ins = CInstance.Cache.GetById(id);
		var pass = ins.Database.MakeListString(string.Concat("SELECT PasswordHash FROM ", TABLE, " WHERE ", LOGIN, "='", ins.InstanceAppLogin, "'"));
		if (pass.Count == 0)
			pass = ins.Database.MakeListString(string.Concat("SELECT PasswordHash FROM ", TABLE, " WHERE ", LOGIN, "='", UserName, "'"));
		if (pass.Count > 0)
		{
			var data = new CNameValueList("PasswordHash", pass[0]);
			var row = new CWhere(TABLE, new CCriteria(PK, _userId), null);

			var i = this.Instance;
			i.Database.Update(data, row);
			CSession.PageMessage = "SET PasswordHash to be same as " + ins.IdAndName + ": " + pass[0];
			Response.Redirect(Request.RawUrl);
		}
	}
	private void Chk_CheckedChanged(object sender, EventArgs e)
	{
		var chk = (CheckBox)sender;

		var col = chk.Text;
		var val = chk.Checked;

		var data = new CNameValueList(col, val);
		var row = new CWhere(TABLE, new CCriteria(PK, _userId), null);

		var i = this.Instance;
		i.Database.Update(data, row);

		Response.Redirect(Request.RawUrl);
	}
	protected void ddApp_SelectedIndexChanged(object sender, EventArgs e)
	{
		var app = CApp.Cache.GetById(CDropdown.GetInt(ddApp));
		Response.Redirect(CSitemap.InstanceAppLogin(app.Instances[0].InstanceId), true);
	}
	protected void ddIns_SelectedIndexChanged(object sender, EventArgs e)
	{
		Response.Redirect(CSitemap.InstanceAppLogin(CDropdown.GetInt(ddIns)), true);
	}
	protected void ddLogin_SelectedIndexChanged(object sender, EventArgs e)
	{
		Response.Redirect(CSitemap.InstanceAppLogin(InstanceId, ddLogin.SelectedValue), true);
	}
	protected void btnSave_Click(object sender, EventArgs e)
	{
		var i = this.Instance;

		i.InstanceAppLogin = txtInstanceAppLogin.Text;
		i.InstanceAppPassword = txtInstanceAppPassword.Text;

		i.Save();
		Response.Redirect(Request.RawUrl);
	}
	#endregion


}