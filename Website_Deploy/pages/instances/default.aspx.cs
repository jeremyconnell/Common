using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;
using SchemaAdmin;
using System.Threading.Tasks;
using Comms.PushUpgrade.Client;
using Comms.PushUpgrade.Interface;
using Comms.Upgrade.Client;
using SchemaAudit;

public partial class pages_Instances_default : CPageDeploy
{
    #region Querystring
    public string Search { get { return CWeb.RequestStr("search"); } }
    public int AppId { get { return CWeb.RequestInt("appId"); } }
    public int ClientId { get { return CWeb.RequestInt("clientId"); } }
    public int LastVersionId { get { return CWeb.RequestInt("lastVersionId"); } }
    public int TargetVersionId { get { return CWeb.RequestInt("targetVersionId"); } }
    #endregion

    #region Data
    public CApp App { get { return CApp.Cache.GetById(AppId); } }
    public SchemaAdmin.CClient Client { get { return SchemaAdmin.CClient.Cache.GetById(ClientId); } }
    public CInstanceList Instances { get { return CInstance.Cache.Search(txtSearch.Text, CDropdown.GetInt(ddApp), CDropdown.GetInt(ddClient)); } }
    #endregion

    #region Event Handlers - Page
    protected override void PageInit()
    {
        if (null == App)
            Response.Redirect(CSitemap.Instances(EApp.ControlTrack), true);

        //Fix data - automatch
        var noClient = CInstance.Cache.GetByClientId(int.MinValue);
        foreach (var i in noClient)
        {
            var c = CClient.Cache.GetByCode(i.InstanceClientCode);
            if (null == c && !string.IsNullOrEmpty(i.InstanceClientName))
            {
                var cc = CClient.Cache.Search(i.InstanceClientName, int.MinValue);
                if (cc.Count > 0) c = cc[0];
            }
            if (null != c && int.MinValue == i.InstanceClientId)
            {
                i.InstanceClientId = c.ClientId;
                i.Save();
            }
        }
        //Fix data - Update ClientName on Instance (sepearate project)
        foreach (var i in CInstance.Cache)
        {
            var c = CClient.Cache.GetById(i.InstanceClientId);
            if (null == c) continue;
            if (i.InstanceClientName != c.ClientName || i.InstanceClientCode != c.ClientUniqueCode)
            {
                i.InstanceClientName = c.ClientName;
                i.InstanceClientCode = c.Code;
                i.Save();
            }
        }

        ctrlNewInstances.Display(CInstance.Cache.NoClient);
        ctrlNewInstances.Visible = CInstance.Cache.NoClient.Count > 0;


        //Populate Dropdowns
        ddApp.DataSource = CApp.Cache;
        ddApp.DataBind();
        //CDropdown.BlankItem(ddApp, "-- Any App --");
        CDropdown.SetValue(ddApp, AppId);

        ddClient.DataSource = SchemaAdmin.CClient.Cache;
        ddClient.DataBind();
        CDropdown.BlankItem(ddClient, "-- Any Client --");
        CDropdown.SetValue(ddClient, ClientId);


        //Search state (from querystring)
        txtSearch.Text = this.Search;

        //Display Results
        var ii = this.Instances.HasClient;
        if (LastVersionId > 0)
            ii = ii.GetBySpecialVersionId(LastVersionId);
        if (TargetVersionId > 0)
            ii = ii.GetByTargetVersionId(TargetVersionId);

        ctrlInstances.Display(this.Instances.HasClient);

        //Client-side
        this.Form.DefaultFocus = txtSearch.ClientID;
        this.Form.DefaultButton = btnSearch.UniqueID;   //CTextbox.OnReturnPress(txtSearch, btnSearch);


        this.Title = App.AppName + " Deployments";
        if (ClientId > 0)
            this.Title += " (for " + CUtilities.Truncate(Client.ClientName) + ")";

        //MenuAppDeploys(AppId);
        MenuInstanceSearch(AppId);

        /*
		//Menu
		AddMenuSide("App Details", CSitemap.AppEdit(AppId));

		int count = 0;
		foreach (var i in CApp.Cache)
            AddMenuSide(string.Concat(++count, ". ", i.NameAndInstanceCount), CSitemap.Instances(i.AppId), i.AppId == AppId && ClientId < 0);

		if (ClientId > 0)
			AddMenuSide(Client.NameAndInstanceCount, CSitemap.Instances(null, AppId, ClientId), true);
		AddMenuSide("New Deploy...", CSitemap.InstanceAdd(AppId, ClientId));

		AddMenuSide(CUtilities.NameAndCount("- Schema", App.Instances), CSitemap.InstancesSchema(AppId));
		if (App.Versions.Count > 0) AddMenuSide(CUtilities.NameAndCount("- Versions", App.Versions), CSitemap.Versions(AppId));
		if (App.BinaryFiles.Count > 0) AddMenuSide(CUtilities.NameAndCount("- Files", App.BinaryFiles), CSitemap.BinaryFiles(AppId));
		if (App.ReleasesMain.Count > 0) AddMenuSide(CUtilities.NameAndCount("- Releases", App.ReleasesMain), CSitemap.Releases(AppId));
		*/
    }
    #endregion

    #region Event Handlers - Form
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Response.Redirect(CSitemap.Instances(txtSearch.Text, CDropdown.GetInt(ddApp), CDropdown.GetInt(ddClient)));
    }
    #endregion

    #region Event Handlers - UserControls
    protected void ctrl_AddClick()
    {
        Response.Redirect(CSitemap.InstanceAdd(AppId, ClientId));
    }
    protected void ctrl_ResortClick(string sortBy, bool descending, int pageNumber)
    {
        Response.Redirect(CSitemap.Instances(txtSearch.Text, AppId, ClientId, new CPagingInfo(0, pageNumber - 1, sortBy, descending)));
    }
    #endregion

    protected void ddApp_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect(CSitemap.Instances(CDropdown.GetInt(ddApp)));
    }

    protected void btnFixAll_Click(object sender, EventArgs e)
    {
        Server.ScriptTimeout = 3600;

        var list = new List<string>();

        var c = HttpContext.Current;
        Parallel.ForEach(App.Instances, i =>
        {
            HttpContext.Current = c;


            try
            {
                var ws = CPushUpgradeClient.Factory(i.InstanceWebHostName + i.InstanceWebSubDir, i.InstanceWebUseSsl);
                var target = i.TargetVersion;


                var m = ws.PollVersion(true, 3);

                CreateIfNotSeenBefore(i, m, ws);

                if (m.SchemaMD5 != target.VersionSchemaMD5)
                {
                    var s = CProto.Deserialise<CSchemaInfo>(target.Schema.GetFile());
                    if (null != s && s.MD5() == m.SchemaMD5)
                        ws.PushSchema(s, 3);
                }

                if (m.MD5 != target.VersionFilesMD5)
                {

                    var p = new CPushedUpgrade();
                    p.PushInstanceId = i.InstanceId;

                    var r = i.LastReport_;
                    if (null != r)
                    {
                        p.PushOldSchemaMD5 = r.ReportInitialSchemaMD5;
                        p.PushOldVersionId = r.ReportInitialVersionId;
                    }
                    p.PushNewVersionId = i.TargetVersionId;
                    p.PushNewSchemaMD5 = i.TargetVersion.VersionSchemaMD5;
                    p.PushUserName = CSession.User.UserLoginName;
                    p.Save();

                    var changes = target.VersionFiles.Diff(p.OldVersion.VersionFiles);
                    ws.PushFiles(changes, 3);
                    m = ws.PollVersion(true, 3);
                }


                if (string.IsNullOrEmpty(m.Id.ClientName))
                {
                    var cc = CClient.Cache.GetByCode(i.InstanceClientCode);
                    if (null != cc)
                        ws.SetInstance(new CInstanceInfo()
                        {
                            Filter = new CFilter(target.VersionExcludedFiles, true),
                            Instance = new CIdentity()
                            {
                                AppId = i.InstanceAppId,
                                ClientName = cc.ClientName,
                                ClientSuffix = cc.Code,
                                InstanceId = i.InstanceId,
                                HostName = i.InstanceWebHostName,
                                ConnectionString = i.InstanceDbConnectionString
                            }
                        });
                }

                lock (list)
                {
                    list.Add(i.IdAndName + ": OK");
                }
            }
            catch (Exception ex)
            {
                lock (list)
                {
                    list.Add(i.IdAndName + ": " + ex.Message);
                    CAudit_Error.Log(ex);
                }
            }
        });

        CSession.PageMessage = CUtilities.ListToString(list, "\r\n");
        Response.Redirect(Request.RawUrl, true);
    }

    private static void CreateIfNotSeenBefore(CInstance i, CMyVersion ver, CPushUpgradeClient p)
    {
        //Store Schema
        var b = CBinaryFile.Cache.GetById(ver.SchemaMD5);
        if (null == b && ver.Schema != null)
        {
            b = new CBinaryFile();
            b.FileAsBytes = CProto.Serialise(ver.Schema);
            b.IsSchema = true;
            b.Path = i.IdAndName;
            try
            {
                b.Save();
            }
            catch (Exception ex)
            {
                SchemaAudit.CAudit_Error.Log(ex);
                CBinaryFile.Cache = null;
            }
        }

        //Store version
        var v = CVersion.Cache.GetByFilesMD5_(ver.MD5);
        CFilesList files = null;
        if (null == v)
        {
            files = p.RequestFiles();
            foreach (CFileNameAndContent f in CShuffle.Shuffle(files))
            {
                b = CBinaryFile.Cache.GetById(f.Md5);
                if (null == b)
                {
                    b = new CBinaryFile();
                    b.FileAsBytes = f.Content;
                    b.Path = f.Name;
                    b.Save();
                }
            }
        }
        v = CVersion.Cache.GetByFilesMD5_(ver.MD5);
        if (null == v)
        {
            v = new CVersion();
            v.VersionFilesMD5 = ver.MD5;
            v.VersionAppId = ver.Id.AppId;
            v.VersionExcludedFiles = CUpgradeClient_Config.Shared.ExtensionsToIgnore;
            v.VersionName = i.NameAndSuffix;
            v.VersionSchemaMD5 = ver.SchemaMD5;
            v.VersionUploadedFrom = i.InstanceWebHostName;
            v.VersionTotalBytes = files.Total();
            v.Save();

            var list = new CVersionFileList();
            foreach (var f in files)
            {
                var vf = new CVersionFile();
                vf.VFVersionId = v.VersionId;
                vf.VFBinaryMD5 = f.Md5;
                vf.VFPath = f.Name;
                //vf.Save();
                list.Add(vf);
            }
            list.BulkInsert();
        }
        else if (v.VersionName == string.Concat("#", int.MinValue, ": -"))
        {
            v.VersionName = i.IdAndName;
            v.Save();
        }
        else if (v.VersionTotalBytes == 0)
        {
            files = p.RequestFiles();
            v.VersionTotalBytes = files.Total();
            v.Save();
        }

        var r = new CReportHistory();
        r.ReportInitialSchemaMD5 = ver.SchemaMD5;
        r.ReportInitialVersionId = null == v ? int.MinValue : v.VersionId;
        r.ReportInstanceId = i.InstanceId;
        r.ReportAppStopped = r.ReportAppStarted;
        r.Save();

        i.LastReport_ = r;
    }

    protected void btnTapAll_Click(object sender, EventArgs e)
    {
        Server.ScriptTimeout = 3600;

        var list = new List<string>();

        var c = HttpContext.Current;
        Parallel.ForEach(App.Instances, i =>
        {
            HttpContext.Current = c;


            try
            {
                var ws = CPushUpgradeClient.Factory(i.InstanceWebHostName + i.InstanceWebSubDir, i.InstanceWebUseSsl);
                var target = i.TargetVersion;


                var m = ws.PollVersion(true, 3);
                CreateIfNotSeenBefore(i, m, ws);

                lock (list)
                {
                    list.Add(i.IdAndName + ": OK");
                }
            }
            catch (Exception ex)
            {
                lock (list)
                {
                    list.Add(i.IdAndName + ": " + ex.Message);
                    CAudit_Error.Log(ex);
                }
            }
        });

        CSession.PageMessage = CUtilities.ListToString(list, "\r\n");
        Response.Redirect(Request.RawUrl, true);
    }
}
