using Framework;
using SchemaAdmin;
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
	public int AppId { get { return null != Instance ? Instance.InstanceAppId : CWeb.RequestInt("appId"); } }
	public int InstanceId { get { return CWeb.RequestInt("instanceId"); } }
	public int DiffInstanceId { get { return CWeb.RequestInt("diffInstanceId"); } }
	public EFix FixId { get { return (EFix)CWeb.RequestInt("fix"); } }
	#endregion

	#region Data
	public CApp App { get { return CApp.Cache.GetById(AppId); } }
	public CInstance Instance { get { return CInstance.Cache.GetById(InstanceId) ?? CInstance.Cache.GetById(DiffInstanceId); } }
	public CClient Client
	{
		get
		{
			return null == Instance ? null : CClient.Cache.GetById(Instance.InstanceClientId);
		}
	}
	#endregion

	protected override void PageInit()
	{
		if (null == App)
			Response.Redirect(CSitemap.AppSchema());
		MenuAppSchema(AppId);
		PageTitle = string.Concat("Schema: ", App.NameAndInstanceCount);
		ctrl.Display_Deploys(App, DiffInstanceId, FixId);
	}

}