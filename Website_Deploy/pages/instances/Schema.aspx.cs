using Framework;
using SchemaDeploy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pages_instances_Schema : CPageDeploy
{
	#region Querystring
	public int InstanceId { get { return CWeb.RequestInt("instanceId"); } }
	public int AppId { get { return null != Instance ? Instance.InstanceAppId : CWeb.RequestInt("appId"); } }

	public int DiffInstanceId { get { return CWeb.RequestInt("diffInstanceId"); } }
	public EFix FixId { get { return (EFix)CWeb.RequestInt("fix"); } }
	#endregion

	#region Data
	public CInstance Instance { get { return CInstance.Cache.GetById(InstanceId) ?? CInstance.Cache.GetById(DiffInstanceId); } }
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

		MenuInstanceSchema(AppId, InstanceId);
		AddMenuSide("All Schema...", CSitemap.AppSchema());
		AddMenuSide("Admin Schema...", CSitemap.SelfSchemaSync());

		ctrlSchema.Display_Instance(Instance, DiffInstanceId, FixId);
	}
	#endregion

	#region Form Events
	protected void ddApp_SelectedIndexChanged(object sender, EventArgs e)
	{
		var app = CApp.Cache.GetById(CDropdown.GetInt(ddApp));
		Response.Redirect(CSitemap.InstanceSchema(app.Instances[0].InstanceId), true);
	}
	protected void ddIns_SelectedIndexChanged(object sender, EventArgs e)
	{
		Response.Redirect(CSitemap.InstanceSchema(CDropdown.GetInt(ddIns)), true);
	}
	#endregion

}