using Framework;
using SchemaAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;

public partial class pages_clients_Sql : CPageDeploy
{
    #region Querystring
    public int InstanceId { get { return CWeb.RequestInt("instanceId"); } }
    #endregion

    #region Data
    public int ClientId_ { get { return Instance.InstanceClientId; } }
    public int AppId { get { return App.AppId; } }
    public CApp App
    {
        get
        {
            return Instance.App;
        }
    }
    public CClient Client
    {
        get
        {
            return CClient.Cache.GetById(ClientId_);
        }
    }
    public CInstance Instance
    {
        get
        {
            return CInstance.Cache.GetById(InstanceId);
        }
    }
    #endregion


    #region Event Handlers - Page
    protected override void PageInit()
    {
        if (null == App)
            Response.Redirect(CSitemap.InstanceSql(CApp.Cache[0].Instances[0].InstanceId));

		ddApp.DataSource = CApp.Cache;
		ddApp.DataBind();
		CDropdown.SetValue(ddApp, AppId);

        ddInstance.DataSource = App.Instances;
        ddInstance.DataBind();
        CDropdown.BlankItem(ddInstance, "-- Select Instance --");
        CDropdown.SetValue(ddInstance, InstanceId);

		ddHistory.DataSource = new SchemaAudit.CAudit_Sql().RecentUnique();
		ddHistory.DataBind();
		ddHistory.BlankItem("-- History -- ");


		if (null != Client)
			AddLinkSide("Client Details...", CSitemap.ClientEdit(AppId), Client.ClientName);


		MenuSelected = "Deploys";
		MenuInstanceSql(AppId, InstanceId);

		var db = Instance.Database;
		if (null == db)
			return;
		CSession.SqlUseConn = db.ConnectionString;
		txtSql.Text = CSession.SqlStatement;


        try
        {
            ddTables.DataSource = db.AllTableNames(true);
            ddTables.DataBind();
            CDropdown.BlankItem(ddTables);
        }
        catch (Exception ex)
        {
            ddTables.ToolTip = ex.Message;
        }
        try
        {
            ddViews.DataSource = db.AllViewNames(true);
            ddViews.DataBind();
            CDropdown.BlankItem(ddViews);
        }
        catch (Exception ex)
        {
            ddTables.ToolTip = ex.Message;
        }
        try
        {
			var list = db.MakeListString(CDataSrc.SQL_TO_LIST_FUNCTIONS_EXCEPT_DBO);
			list.AddRange(db.MakeListString(CDataSrc.SQL_TO_LIST_STORED_PROCS_EXCEPT_DBO));

			ddProcs.DataSource = list;
            ddProcs.DataBind();
            CDropdown.BlankItem(ddProcs);
        }
        catch (Exception ex)
        {
            ddProcs.ToolTip = ex.Message;
        }

    }
    protected override void PagePreRender()
    {
        var c = this.Client;
		
    }
    #endregion

    #region Event Handlers - Form
    protected void ddInstance_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect(CSitemap.InstanceSql(CDropdown.GetInt(ddInstance)));
    }
    protected void btnTest_Click(object sender, EventArgs e)
    {
        try
        {
            CDataSrcLocal db = new CSqlClient(Instance.InstanceDbConnectionString);
            var tables = db.AllTableNames();
			txtSql.Text = db.SqlToListAllTables;
			btnSelect_Click(null, null);
        }
        catch (Exception ex)
        {
            CSession.PageMessageEx = ex;
        }
    }
    protected void btnSelect_Click(object sender, EventArgs e)
    {
        fs2.Visible = true;
		CSession.SqlRunOnAllInstancesOfAppId = chkAll.Checked ? AppId : int.MinValue;

		CSession.SqlUseConn = Instance.InstanceDbConnectionString;
        CSession.SqlStatement = txtSql.Text;
        CSession.SqlIsSelect = true;

        iframe.Attributes.Add("src", "~/pages/sql/iframe.aspx");
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        fs2.Visible = true;
		CSession.SqlRunOnAllInstancesOfAppId = chkAll.Checked ? AppId : int.MinValue;

		CSession.SqlUseConn = Instance.InstanceDbConnectionString;
        CSession.SqlStatement = txtSql.Text;
        CSession.SqlIsSelect = false;

        iframe.Attributes.Add("src", "~/pages/sql/iframe.aspx");
    }
    #endregion

    protected void ddTables_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtSql.Text += " " + ddTables.SelectedValue;
    }

    protected void ddViews_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtSql.Text += " " + ddViews.SelectedValue;
    }

    protected void ddProcs_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtSql.Text += " " + ddProcs.SelectedValue;
    }

	protected void ddHistory_SelectedIndexChanged()
	{
		var sql = new SchemaAudit.CAudit_Sql(ddHistory.ValueInt);
		txtSql.Text = sql.SqlText;
		fs2.Visible = false;
	}

}