using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;
using System.Threading.Tasks;
using System.Threading;
using SchemaAudit;
using System.Text;

public partial class pages_Backups_default : CPageDeploy
{
    #region Constants
    public const int PARALLEL_TBLS = 2;
    public const int PARALLEL_DBS = 5;
    #endregion

    #region Querystring
    public string Search { get { return CWeb.RequestStr("search"); } }

    public int AppId { get { return null != Instance ? Instance.InstanceAppId : CWeb.RequestInt("appId", (int)EApp.ControlTrack); } }
    public int InstanceId { get { return CWeb.RequestInt("instanceId"); } }
    public string TableName { get { return CWeb.RequestStr("tableName"); } }
    #endregion

    #region Members
    private CBackupList _backups;
    #endregion

    #region Data
    public CApp App { get { return CApp.Cache.GetById(AppId); } }
    public CInstance Instance { get { return CInstance.Cache.GetById(InstanceId); } }
    public CBackupList Backups
    {
        get
        {
            //Sql-based Paging
            if (_backups == null)
            {
                _backups = new CBackup().SelectSearch(ctrlBackups.PagingInfo, txtSearch.Text);
                //_backups.PreloadChildren(); //Loads children for page in one hit (where applicable)
            }
            return _backups;
        }
    }
    public DataSet BackupsAsDataSet() { return new CBackup().SelectSearch_Dataset(txtSearch.Text); }
    #endregion

    #region Event Handlers - Page
    protected override void PageInit()
    {
        ddApp.DataSource = CApp.Cache;
        ddApp.DataBind();
        CDropdown.SetValue(ddApp, AppId);

        ddInstance.DataSource = App.Instances;
        ddInstance.DataBind();
        CDropdown.BlankItem(ddInstance, "-- All Deploys --");
        CDropdown.SetValue(ddInstance, InstanceId);

        //var tables = new CBackupItem().TableNameAndCount(InstanceId);
        //foreach (var i in tables)
        //    CDropdown.Add(ddTableName, CUtilities.NameAndCount(i.Key, i.Value));
        //CDropdown.BlankItem(ddTableName);
        //CDropdown.SetValue(ddTableName, TableName);

        btnBackupAll.Text = CUtilities.NameAndCount(btnBackupAll.Text, App.InstanceCount);

        btnBackupIns.Visible = InstanceId > 0;

        //Populate Dropdowns
        MenuAppBackups(AppId);

        //Search state (from querystring)
        txtSearch.Text = this.Search;

        //Display Results
        ctrlBackups.Display(this.Backups, new CBackup().SelectSum("TotalSize"));

        //Client-side
        this.Form.DefaultFocus = txtSearch.ClientID;
        this.Form.DefaultButton = btnSearch.UniqueID;   //CTextbox.OnReturnPress(txtSearch, btnSearch);


        //Except cbls
        if (App.InstanceCount == 0)
        {
            btnBackupAll.Enabled = false;
            txtExclude.Visible = false;
            return;
        }

        if (null != Instance)
            btnBackupIns.OnClientClick = btnBackupIns.OnClientClick.Replace("THIS", Instance.NameAndSuffix);
    }
    #endregion 

    #region Event Handlers - Form
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        var appId = CDropdown.GetInt(ddApp);
        var instanceId = (appId != AppId) ? int.MinValue : CDropdown.GetInt(ddInstance);
        Response.Redirect(CSitemap.Backups(appId, instanceId, txtSearch.Text));
    }
    protected void btnBackupAll_Click(object sender, EventArgs e)
    {
        Page.AsyncTimeout = TimeSpan.FromHours(2);
        Server.ScriptTimeout = 3600;

        var excludeSchemas = CUtilities.StringToListStr(txtExclude.Text);


        var c = HttpContext.Current;
        var batchDate = DateTime.Now;

        var po = new ParallelOptions() { MaxDegreeOfParallelism = PARALLEL_DBS };
        Parallel.ForEach(App.Instances, po, ins =>
        {
            var failed = new Dictionary<string, Exception>();
            var skipped = new List<string>();

            CBackup.BackupInstance(ins, c, excludeSchemas, failed, skipped, batchDate, PARALLEL_TBLS);

            lock (po)
            {
                if (skipped.Count > 0)
                    CSession.PageMessage = "Skipped: " + CUtilities.ListToString(skipped);
                foreach (var i in failed)
                    CSession.PageMessage += string.Concat(i.Key, " FAILED: ", i.Value.Message + "\r\n");
            }
        });


        Response.Redirect(Request.RawUrl);
    }
    protected void btnBackupIns_Click(object sender, EventArgs e)
    {
        var c = HttpContext.Current;


        var excludeSchemas = CUtilities.StringToListStr(txtExclude.Text);
        var failed  = new Dictionary<string, Exception>();
        var skipped = new List<string>();

        CBackup.BackupInstance(Instance, c, excludeSchemas, failed, skipped, DateTime.Now, PARALLEL_TBLS);


        //Report summary
        if (skipped.Count > 0)
            CSession.PageMessage = "Skipped: " + CUtilities.ListToString(skipped);

        foreach (var i in failed)
            CSession.PageMessage += string.Concat(i.Key, " FAILED: ", i.Value.Message + "\r\n");

        Response.Redirect(Request.RawUrl, true);
    }

    protected void btnDeleteOld_Click(object sender, EventArgs e)
    {
        var MAX_PER_CLIENT = CDropdown.GetInt(ddKeep);

        var bb = new CBackup().SelectAll();
        foreach (var i in CInstance.Cache)
        {
            var forClient = bb.GetByInstanceId(i.InstanceId);
            foreach (var j in forClient)
            {
                var num = forClient.IndexOf(j) + 1;
                if (num <= MAX_PER_CLIENT)
                    continue;
                j.Delete();
            }
        }
        Response.Redirect(Request.RawUrl, true);
    }
    #endregion

    #region Event Handlers - Usercontrols
    protected void ctrl_ExportClick()
    {
        CDataSrc.ExportToCsv(BackupsAsDataSet(), Response, "Backups.csv");
    }
    protected void ctrl_ResortClick(string sortBy, bool descending, int pageNumber)
    {
        Response.Redirect(CSitemap.Backups(AppId, InstanceId, Search, new CPagingInfo(0, pageNumber - 1, sortBy, descending)));
    }
    #endregion





}
