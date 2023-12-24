using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;

public partial class pages_instances_Version :  CPageDeploy
{
	#region Querystring
	public int InstanceId { get { return CWeb.RequestInt("instanceId"); } }
	public int AppId { get { return null != Instance ? Instance.InstanceAppId : CWeb.RequestInt("appId"); } }
	#endregion

	#region Data
	public CInstance Instance	{ get { return CInstance.Cache.GetById(InstanceId); } }
	public CApp App				{ get { return CApp.Cache.GetById(AppId); } }
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

		ddInstanceSpecialVersionId.DataTextField = "VersionName";
		ddInstanceSpecialVersionId.DataValueField = "VersionId";
		ddInstanceSpecialVersionId.DataSource = Instance.App.Versions;
		ddInstanceSpecialVersionId.DataBind();
		ddInstanceSpecialVersionId.BlankItem("-- Custom Version (Branch) --");

		MenuInstanceVersion(AppId, InstanceId);
	}
	protected override void PageLoad()
	{
		var a = this.App;
		var v = a.MainVersion;
		if (null != v)
		{
			txtMainVersion.Text = v.VersionName + " " + v.VersionFilesB64;
			txtMainVersion.NavigateUrl = CSitemap.VersionEdit(v.VersionId);
			txtMainSchema.Text = v.VersionSchemaB64;
			txtMainSchema.NavigateUrl = CSitemap.Schema(v.VersionSchemaMD5);
		}
		else
		{
			txtMainVersion.Text = a.AppName + " - No Version Released!";
			txtMainSchema.Visible = false;
		}
		txtMainVersion.NavigateUrl = CSitemap.AppEdit(a.AppId);

		var i = this.Instance;
		ddInstanceSpecialVersionId.ValueInt = i.InstanceSpecialVersionId;
		txtInstanceSpecialVersionName.Text = i.InstanceSpecialVersionName;

		var r = i.LastReport_;
		if (null == r)
		{
			txtLastReportedSchema.Visible = false;
			txtLastReportedVersion.Visible = false;
		}
		else
		{
			v = r.InitialVersion;

			txtLastReportedVersion.Text = v.VersionName + " " + v.VersionFilesB64;
			txtLastReportedVersion.NavigateUrl = CSitemap.VersionEdit(v.VersionId);

			txtLastReportedSchema.Text = v.VersionSchemaB64;
			txtLastReportedSchema.ToolTip = CBinaryFile.Cache.GetById(v.VersionSchemaMD5)?.Path;
			txtLastReportedSchema.NavigateUrl = CSitemap.Schema(v.VersionSchemaMD5);
		}
	}
	#endregion

	#region Form Events
	protected void ddApp_SelectedIndexChanged(object sender, EventArgs e)
	{
		var app = CApp.Cache.GetById(CDropdown.GetInt(ddApp));
		Response.Redirect(CSitemap.InstanceVersion(app.Instances[0].InstanceId), true);
	}
	protected void ddIns_SelectedIndexChanged(object sender, EventArgs e)
	{
		Response.Redirect(CSitemap.InstanceVersion(CDropdown.GetInt(ddIns)), true);
	}
	protected void btnSave_Click(object sender, EventArgs e)
	{
		var i = this.Instance;

		i.InstanceSpecialVersionId = ddInstanceSpecialVersionId.ValueInt;
		i.InstanceSpecialVersionName = txtInstanceSpecialVersionName.Text;

		i.Save();
		Response.Redirect(Request.RawUrl);
	}
	#endregion

}