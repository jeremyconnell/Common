using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaAdmin;
using Framework;
using System.Text;
using System.Threading.Tasks;
using SchemaDeploy;

public partial class pages_Clients_default : CPage
{
    #region Querystring
    public string Search { get { return CWeb.RequestStr("search"); } }
    public int StatusId { get { return CWeb.RequestInt("statusId"); } }
    #endregion

    #region Data
    public CStatus Status {  get { return CStatus.Cache.GetById(StatusId); } }
    public CClientList Clients
    {
        get
        {
            return CClient.Cache.Search(txtSearch.Text, StatusId);
        }
    } 
    #endregion 
    
    #region Event Handlers - Page
    protected override void PageInit()
    {
        //Populate Dropdowns
        ddStatus.DataSource = CStatus.Cache;
        ddStatus.DataBind();
        CDropdown.BlankItem(ddStatus);
        CDropdown.SetValue(ddStatus, StatusId);
    
        //Search state (from querystring)
        txtSearch.Text = this.Search;
        
        //Display Results
        ctrlClients.Display(this.Clients);

        //Client-side
        this.Form.DefaultFocus  = txtSearch.ClientID;
        this.Form.DefaultButton = btnSearch.UniqueID;   //CTextbox.OnReturnPress(txtSearch, btnSearch);

        AddMenuSide(CUtilities.NameAndCount("Clients", CClient.Cache), CSitemap.Clients(), int.MinValue == StatusId);


		//foreach (var i in CStatus.Cache)
  //          if (i.ClientCount > 0)
  //              AddMenuSide("- " + i.StatusNameAndCount, CSitemap.Clients(null, i.StatusId), i.StatusId == StatusId);

		AddMenuSide("New Client...", CSitemap.ClientAdd());
		AddMenuSide("Schema...", CSitemap.AppSchema());
		AddMenuSide(CUtilities.NameAndCount("States", CStatus.Cache) + "...", CSitemap.Statuss());
	}
	#endregion

	#region Event Handlers - Form
	protected void btnSearch_Click(object sender, EventArgs e)
    {   
        Response.Redirect(CSitemap.Clients(txtSearch.Text, CDropdown.GetInt(ddStatus)));
    }
    protected void btnTouch_Click(object sender, EventArgs e)
    {
        var c = HttpContext.Current;

        var dict = new Dictionary<int, string>();
        Parallel.ForEach(CClient.Cache, client =>
        {
            HttpContext.Current = c;

            var sbOk = new StringBuilder();
            var sbEr = new StringBuilder();
            foreach (var ins in client.Instances)
            {
                try
                {
                    var api = client.Api(ins);
                    api.TimeoutSecs = 10; //Ten secs
                    var values = api.ConfigSettingsAll();
                    sbOk.Append(values.Count).Append(" values retrieved from: ").AppendLine(client.ClientName);
                }
                catch (Exception ex)
                {
                    sbEr.Append(client.ClientName).Append(": ").AppendLine(ex.Message);
                }
                dict.Add(ins.InstanceId, sbOk.Append(sbEr).ToString());
            }
        });

        pre.Visible = true;
        pre.InnerText = CUtilities.ListToString(new List<string>(dict.Values));
    }
    protected void btnImport_Click(object sender, EventArgs e)
    {
        var c = HttpContext.Current;

        var dict = new Dictionary<int, string>();
        Parallel.ForEach(CClient.Cache, client =>
        {
            HttpContext.Current = c;

            var sbOk = new StringBuilder();
            var sbEr = new StringBuilder();
            foreach (var ins in client.Instances)
            {
                try
                {
                    var api = client.Api(ins);
                    api.TimeoutMins = 3;
                    var values = client.ImportConfig(ins);
                    sbOk.Append(values.Count).Append(" values retrieved from: ").AppendLine(client.ClientName);
                }
                catch (Exception ex)
                {
                    sbEr.Append(client.ClientName).Append(": ").AppendLine(ex.Message);
                }
                dict.Add(ins.InstanceId, sbOk.Append(sbEr).ToString());
            }
        });

        pre.Visible = true;
        pre.InnerText = CUtilities.ListToString(new List<string>(dict.Values));
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        var c = HttpContext.Current;

        var dict = new Dictionary<int, string>();
        Parallel.ForEach(CClient.Cache, client =>
        {
            HttpContext.Current = c;

            var sbOk = new StringBuilder();
            var sbEr = new StringBuilder();
            foreach (var ins in client.Instances)
            {
                try
                {
                    var api = client.Api(ins);
                    api.TimeoutMins = 3;
                    var values = client.ExportConfig(ins);
                    sbOk.Append(values.Count).Append(" values returned from: ").AppendLine(client.ClientName);
                }
                catch (Exception ex)
                {
                    sbEr.Append(client.ClientName).Append(": ").AppendLine(ex.Message);
                }
                dict.Add(client.ClientId, sbOk.Append(sbEr).ToString());
            }
        });

        pre.Visible = true;
        pre.InnerText = CUtilities.ListToString(new List<string>(dict.Values));
    }

    /*
    protected void btnReadCS_Click(object sender, EventArgs e)
    {
        var ss = CUtilities.SplitOn(txtCS.Text, "\r\n");
        foreach (var i in ss)
        {
            if (!i.Contains("ControlTrackContext"))
                continue;
            string lookFor1 = "Database=";
            string lookFor2 = ";User";

            int idx1 = i.IndexOf(lookFor1, StringComparison.CurrentCultureIgnoreCase);
            if (-1 == idx1)
            {
                lookFor1 = "Initial Catalog=";
                idx1 = i.IndexOf(lookFor1, StringComparison.CurrentCultureIgnoreCase);
            }
            if (-1 == idx1)
                continue;
            idx1 += lookFor1.Length;
            int idx2 = i.IndexOf(lookFor2, idx1, StringComparison.CurrentCultureIgnoreCase);
            if (-1 == idx2)
                continue;
            string databaseName = i.Substring(idx1, idx2 - idx1);
            idx2 = databaseName.IndexOf(";");
            if (-1 != idx2)
                databaseName = databaseName.Substring(0, idx2);

            string code = databaseName.Replace("controltrack", string.Empty);
            CClient c = CClient.Cache.GetByCode(code);
            if (null == c)
                continue;

            lookFor1 = "connectionString=\"";
            lookFor2 = " />";

            idx1 = i.IndexOf(lookFor1, StringComparison.CurrentCultureIgnoreCase);
            if (-1 == idx1)
                continue;
            idx1 += lookFor1.Length;
            idx2 = i.IndexOf(lookFor2, idx1, StringComparison.CurrentCultureIgnoreCase);
            if (-1 == idx2)
                continue;
            string connectionString = i.Substring(idx1, idx2 - idx1).Trim();
            if (connectionString.EndsWith("\""))
                connectionString = connectionString.Substring(0, connectionString.Length - 1);

            if (null == c.ClientConnectionString || c.ClientConnectionString != connectionString)
            {
                c.ClientConnectionString = connectionString;
                c.Save();
            }
        }
    }
        */
    #endregion

    #region Event Handlers - UserControls
    protected void ctrl_AddClick()
    {
        Response.Redirect(CSitemap.ClientAdd());
    }
    protected void ctrl_ResortClick(string sortBy, bool descending, int pageNumber)
    {
        Response.Redirect(CSitemap.Clients(txtSearch.Text, StatusId, new CPagingInfo(0, pageNumber - 1, sortBy, descending)));
    }
    #endregion


    
}
