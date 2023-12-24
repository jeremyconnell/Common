using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;

public partial class pages_instances_Data : CPageDeploy
{
	#region Querystring
	public int InstanceId { get { return CWeb.RequestInt("instanceId"); } }
	public int AppId { get { return null != Instance ? Instance.InstanceAppId : CWeb.RequestInt("appId"); } }
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

		MenuInstanceData(AppId, InstanceId);

        ctrlData.Display(Instance);
    }
	#endregion

	#region Form Events
	protected void ddApp_SelectedIndexChanged(object sender, EventArgs e)
	{
		var app = CApp.Cache.GetById(CDropdown.GetInt(ddApp));
		Response.Redirect(CSitemap.InstanceData(app.Instances[0].InstanceId), true);
	}
	protected void ddIns_SelectedIndexChanged(object sender, EventArgs e)
	{
		Response.Redirect(CSitemap.InstanceData(CDropdown.GetInt(ddIns)), true);
	}
	#endregion

}