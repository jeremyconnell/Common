using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using SchemaAdmin;
using Framework;

public partial class pages_instances_usercontrols_UCInstance : UserControl 
{     
    #region Members
    private CInstance _instance; 
    private IList _sortedList;
    #endregion

    #region Interface
    public void Display(CInstance instance, IList sortedList)
    {
        if (Parent.Controls.Count % 2 == 0)
            row.Attributes.Add("class", "alt_row");

        _instance = instance;
        _sortedList = sortedList;

		if (_instance.InstanceClientId == int.MinValue)
		{
			ddClient.DataSource = CClient.Cache.SortBy("InstanceCount", false);
			ddClient.DataBind();
			CDropdown.BlankItem(ddClient);
		}
		else
			ddClient.Visible = false;

        CInstance ins = _instance;

        litNumber.Text = Convert.ToString(sortedList.IndexOf(ins) + 1);
		//litInstanceAppId.Text = ins.App.AppName;
		lnkInstanceName.ToolTip = ins.IdAndName;
        lnkInstanceName.Text = ins.NameAndSuffix;
        lnkInstanceName.NavigateUrl = CSitemap.Instance(ins.InstanceId);
        litInstanceClientId.NavigateUrl = CSitemap.ClientEdit(ins.InstanceClientId);

        var v = ins.TargetVersion;
		lnkTargetVersionId.ToolTip = v?.VersionName?? string.Empty;
		lnkTargetVersionId.Text = v?.VersionName ?? "none";
		lnkTargetVersionId.NavigateUrl = null != v ? CSitemap.AppEdit(v.VersionId) : CSitemap.InstanceVersion(ins.InstanceId);

		lnkTargetVersionSchema.Text = v?.VersionSchemaB64 ?? "n/a";
		lnkTargetVersionSchema.NavigateUrl = CSitemap.InstanceSchema(ins.InstanceId);

        lblLogin.NavigateUrl = ins.UrlWebApp;
        lblLogin.Text = ins.InstanceAppLogin;
        lblPass.Text = ins.InstanceAppPassword;

		litInstanceCreated.Text = CUtilities.Timespan(ins.InstanceCreated);
        var r = ins.LastReport();
        if (null != r)
        {
            litLastReport.Text = CUtilities.Timespan(r.ReportAppStarted);
            litLastReport.NavigateUrl = CSitemap.InstanceMonitor(ins.InstanceId);
            //litFor.Text = r.RanFor_;
            var vr = r.InitialVersion;
            if (null != vr)
            {
                litLastVersion.ToolTip = vr.VersionName;
				litLastVersion.Text = vr.VersionName;
				litLastVersion.NavigateUrl = CSitemap.BinaryFiles(vr.VersionAppId, vr.VersionId);
				if (null != v && vr.VersionId != v.VersionId)
					litLastVersion.ForeColor = System.Drawing.Color.Red;

				litLastSchema.Text = r.ReportInitialSchemaB64;
				litLastSchema.NavigateUrl = CSitemap.Schema(vr.VersionSchemaMD5);
				if (null != v && r.ReportInitialSchemaMD5 != v.VersionSchemaMD5)
					litLastSchema.ForeColor = System.Drawing.Color.Red;
			}
		}


        if (ins.Values.Count > 0)
            lnkValuesCount.Text = CUtilities.CountSummary(ins.Values, "value");
        lnkValuesCount.NavigateUrl = CSitemap.InstanceSettings(ins.InstanceClientId, ins.InstanceId);

		litInstanceClientId.Text = ins.InstanceClientCode; ;
		litInstanceClientId.ToolTip = ins.InstanceClientName;

    }
    #endregion

    #region Event Handlers 
    protected void btnDelete_Click(object sender, ImageClickEventArgs e) 
    {
        _instance.Delete(); 
        Refresh(); 
    } 
    #endregion 
    
    #region Private
    private void Refresh() 
    { 
        Response.Redirect(Request.RawUrl); 
    }
	#endregion

	protected void ddClient_SelectedIndexChanged(object sender, EventArgs e)
	{
		var clientId = CDropdown.GetInt(ddClient);
		var c = CClient.Cache.GetById(clientId);

		if (_instance.InstanceClientId == int.MinValue)
		{
			c.SetClient(_instance);
			_instance.Save();
		}
		Response.Redirect(Request.RawUrl);
	}
} 
