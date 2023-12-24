using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Framework;
using SchemaDeploy;
using Comms.PushUpgrade.Client;
using Comms.PushUpgrade.Interface;
using Comms.Upgrade.Server;
using System.Drawing;
using System.Text;
using System.Web.UI.HtmlControls;
using Comms.Upgrade.Client;
using SchemaAudit;

public partial class pages_instances_usercontrols_UCMonitor : CUserControlWithTableHelpers
{
    private CInstance _ins;
    private CPushUpgradeClient _push;



    public string Label {  get { return lblMonitoring.Text; } set { lblMonitoring.Text = value; } }
    public void Display(CInstance i, string logFile)
    {	

		_push = CPushUpgradeClient.Factory(i.InstanceWebHostName + i.InstanceWebSubDir, i.InstanceWebUseSsl);

		var isIframe = CWeb.RequestBool("iframe");

		string s = string.Empty;
		switch (logFile)
		{
			case "*"		  : s = CSession.TextForIframe; CSession.TextForIframe = null;	break;
			case "web.config" : s = _push.RequestWebConfig();		break;
			default           : s = _push.RequestLogFile(logFile);	break;
		}

        Response.ContentType =  "text/plain";
        Response.AddHeader("content-disposition", (isIframe ? "inline" : "attachment") + "; fileName=" + logFile);
        Response.Write(s);
        Response.End();
    }
    public void Display(CInstance i)
    {
        this.Visible = true;

		rbl.SelectedIndex = CSession.MonitorTab;
		plhSummary.Visible = rbl.SelectedIndex == 0;
		plhViewer.Visible = rbl.SelectedIndex == 1;
		plhAppSettings.Visible = rbl.SelectedIndex == 2;


		_ins = i;
        _push = CPushUpgradeClient.Factory(i.InstanceWebHostName + i.InstanceWebSubDir, i.InstanceWebUseSsl);


		var name = CWeb.RequestStr("showing", "web.config");

		txtInstance.Text = i.IdAndName;
        txtInstance.NavigateUrl = CSitemap.Instance(i.InstanceId);
        try
        {
            if (!Page.IsPostBack)
                CSession.MyVersionReset(_push);
            var ver =  CSession.MyVersion(_push, true, 3);
            ShowId(ver);
            ShowLogs(ver);
            ShowAppSettings(ver);
            ShowConnStr(ver);

			CDropdown.SetValue(ddFile, name);

			iframe.Src = CSitemap.InstanceMonitorIframe(i.InstanceId, name);
			iframe.Visible = !string.IsNullOrEmpty(name);

			if (!String.IsNullOrEmpty(CSession.TextForIframe))
			{
				iframe.Visible = true;
				iframe.Src = CSitemap.InstanceMonitorIframe(_ins.InstanceId, "*");
				CDropdown.Add(ddFile, "*", "*");
			}

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

			var v = StoreVersion(ver, i);

			var r = new CReportHistory();
			r.ReportInitialSchemaMD5 = ver.SchemaMD5;
			r.ReportInitialVersionId = null == v ? int.MinValue : v.VersionId;
			r.ReportInstanceId = i.InstanceId;
			r.ReportAppStopped = r.ReportAppStarted;
			r.Save();

			i.LastReport_ = r;
		}
        catch (Exception ex)
        {
			CBinaryFile.Cache = null;
			CVersionFile.Cache = null;

			CSession.PageMessageEx = ex;
            HideId(ex);
        }
	}


	private CVersion StoreVersion(CMyVersion ver, CInstance i)
	{
		CBinaryFile b = null;
		CFilesList files = null;

		var v = CVersion.Cache.GetByFilesMD5_(ver.MD5);
		if (null == v)
		{
			//Store Binaries
			files = _push.RequestFiles();
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
			//Record the version
			v = new CVersion();
			v.VersionFilesMD5 = ver.MD5;
			v.VersionAppId = ver.Id.AppId;
			v.VersionExcludedFiles = CUpgradeClient_Config.Shared.ExtensionsToIgnore;
			v.VersionName = i.IdAndName;
			v.VersionSchemaMD5 = ver.SchemaMD5;
			v.VersionUploadedFrom = i.InstanceWebHostName;
			v.VersionTotalBytes = files.Total();
			v.Save();

			//Connect the files
			foreach (var f in files)
			{
				var vf = new CVersionFile();
				vf.VFVersionId = v.VersionId;
				vf.VFBinaryMD5 = f.Md5;
				vf.VFPath = f.Name;
				vf.Save();
			}
		}
		else if (v.VersionName == string.Concat("#", int.MinValue, ": -"))
		{
			v.VersionName = i.IdAndName;
			v.Save();
		}
		return v;
	}


    private void ShowId(CMyVersion v) { ShowId(v.Id, v.MD5, v.SchemaMD5); }
    private void ShowId(CIdentity id, Guid md5, Guid schemaMd5)
    {
        if (int.MinValue == id.InstanceId)
            txtRemote.Text = string.Concat(id.ClientName, " ", id.ClientSuffix).Trim();
		else
			txtRemote.Text = string.Concat("#", id.InstanceId, ": ", id.ClientName, " ", id.ClientSuffix).Trim();

		txtMachine.Text = id.MachineName;
		txtHost.Text = id.HostName;

		var a = CApp.Cache.GetById(id.AppId);
        if (null != a)
            txtRemote.Text += " (" + a.AppName + ")";
        else
            txtRemote.Text += " (AppId=" + id.AppId + ")";

		var tv = a.MainVersion;
		if (null != tv)
		{
			txtTargetVer.ToolTip = tv.VersionFilesB64;
			txtTargetVer.Text = tv.NumberAndName;
			txtTargetSch.Text = tv.VersionSchemaB64;
			txtTargetVer.NavigateUrl = CSitemap.BinaryFiles(tv.VersionAppId, tv.VersionId);
			txtTargetSch.NavigateUrl = CSitemap.InstanceSchema(id.InstanceId);
			if (tv.VersionFilesMD5 != md5) txtTargetVer.Hyperlink.ForeColor = Color.Red;
			if (tv.VersionSchemaMD5 != schemaMd5) txtTargetSch.Hyperlink.ForeColor = Color.Red;
			btnPushFiles.Enabled = tv.VersionFilesMD5 != md5;
		}

		txtVersion.Text = CBinary.ToBase64(md5, 10);
        txtSchema.Text = CBinary.ToBase64(schemaMd5, 10);
        txtSchema.Visible = txtSchema.Text.Length > 0;

        var vv = CVersion.Cache.GetByFilesMD5(md5);
        var v = (vv.Count > 0) ? vv[0] : null;
		if (null != v)
		{
			txtVersion.NavigateUrl = CSitemap.VersionEdit(v.VersionId);
			txtVersion.ToolTip = v.VersionFilesB64;
			txtVersion.Text = v.NumberAndName;
		}

        var sch = CBinaryFile.Cache.GetById(schemaMd5);
        if (null != sch)
            txtSchema.NavigateUrl = CSitemap.Schema(schemaMd5);
	}
    private void ShowLogs(CMyVersion v)
    {
        if (null == v.LogFiles)
            return;

		CDropdown.Add(ddFile, "web.config");
        foreach (var i in v.LogFiles)
			CDropdown.Add(ddFile, i.Key);
    }
    private void ShowAppSettings(CMyVersion v)
    {
        if (null == v.AppSettings)
        {
            tblConfig.Visible = false;
            return;
        }

        var tr = Row(tblConfig);
        CellH(tr, "AppSettings");
        CellH(tr, "Value");

        foreach (var i in v.AppSettings)
        {
            var row = Row(tblConfig);
            Cell(row, i.Key, i.Key, true).Style.Add("padding", "2px 10px");
            var td = Cell(row, i.Value);
            td.HorizontalAlign = HorizontalAlign.Left;
            td.Style.Add("padding", "2px 10px");
        }
    }
    private void ShowConnStr(CMyVersion v)
    {
        if (null == v.ConnectionStrings)
        {
            tblConnStr.Visible = false;
            return;
        }

        var tr = Row(tblConnStr);
        CellH(tr, "Conn.Str.");
        CellH(tr, "Value");

        foreach (var i in v.ConnectionStrings)
        {
            var row = Row(tblConnStr);
            Cell(row, i.Key, i.Key, true).Style.Add("padding","2px 10px");
            var td = Cell(row, i.Value);
            td.HorizontalAlign = HorizontalAlign.Left;
            td.Style.Add("padding", "2px 10px");
        }
    }
    
    private void HideId(Exception ex)
    {
        txtSchema.Visible = txtSchema.Text.Length > 0;
        txtVersion.Visible = txtVersion.Text.Length > 0;
		txtTargetSch.Visible = txtSchema.Visible;
		txtTargetVer.Visible = txtVersion.Visible;

		txtRemote.Visible = txtRemote.Text.Length > 0;
        txtInstance.ToolTip = ex.Message;
        txtInstance.Hyperlink.ForeColor = Color.Red;
	}


    protected void btnPushName_Click(object sender, EventArgs e)
    {
        var id = new CIdentity();
        id.AppId = _ins.InstanceAppId;
        id.InstanceId = _ins.InstanceId;
        id.ClientName = _ins.InstanceClientName;
		id.ClientSuffix = _ins.InstanceSuffix;
		id.HostName = _ins.InstanceWebHostName;

        var info = new CInstanceInfo();
        info.Instance = id;
        info.Filter = new CFilter(CUpgradeServer_Config.Shared.ExceptExtensions, true);

        try
        {
            var myVer = _push.SetInstance(info);
        }
        catch (Exception ex)
        {
			CSession.PageMessageEx = ex;
        }
		Response.Redirect(Request.RawUrl);
    }

    protected void btnTrigger_Click(object sender, EventArgs e)
    {
        try
        {
            CException ex2 = _push.TriggerUpgrade();
            if (null == ex2)
                return;

			CSession.PageMessageEx2 = ex2;
		}
        catch (Exception ex)
		{
			CSession.PageMessageEx = ex;
		}
        Response.Redirect(Request.RawUrl);
    }

    protected void btnReqFiles_Click(object sender, EventArgs e)
    {

    }

    protected void btnReqSchema_Click(object sender, EventArgs e)
    {
    }

    protected void btnReqConfig_Click(object sender, EventArgs e)
    {
        var s = _push.RequestWebConfig();
        Response.ContentType = "text/plain";
        Response.AddHeader("content-disposition", "attachment;filename=web.config");
        Response.Write(s);
        Response.End();
    }

    protected void btnReqHash_Click(object sender, EventArgs e)
    {
        var h = _push.RequestHash();
        var sb = new StringBuilder();
        foreach (var i in h)
        {
            sb.Append(i.Name).Append("\t").Append(CBinary.ToBase64(i.MD5, 8)).Append("\t");
            var b = CBinaryFile.Cache.GetById(i.MD5);
            if (null == b)
                sb.AppendLine("NOT FOUND!");
            else
                sb.AppendLine(b.Size_);
        }
        CSession.TextForIframe = sb.ToString();
        Response.Redirect(Request.RawUrl);
    }

	protected void ddFile_SelectedIndexChanged(object sender, EventArgs e)
	{
		var edit = CWeb.RequestBool("edit");

		if (edit)
			Response.Redirect(CSitemap.InstanceMonitorEdit(_ins.InstanceId, ddFile.SelectedValue));
		else
			Response.Redirect(CSitemap.InstanceMonitorView(_ins.InstanceId, ddFile.SelectedValue));
	}

	protected void btnPushFiles_Click(object sender, EventArgs e)
	{
		var r = _ins.LastReport_;

		var p = new CPushedUpgrade();
		p.PushInstanceId = _ins.InstanceId;
		if (null != r)
		{
			p.PushOldSchemaMD5 = r.ReportInitialSchemaMD5;
			p.PushOldVersionId = r.ReportInitialVersionId;
		}
		p.PushNewVersionId = _ins.TargetVersionId;
		p.PushNewSchemaMD5 = _ins.TargetVersion.VersionSchemaMD5;
		p.PushUserName = CSession.User.UserLoginName;
		try
		{
			p.Save();
		}
		catch (Exception ex2)
		{
			CSession.PageMessageEx = ex2;
		}

		var tv = _ins.TargetVersion;
		var av = r.InitialVersion;
		var changes = av.VersionFiles.Diff(tv.VersionFiles); //dodges the 4mb limit
        while (changes.Count > 200)
        {
            var temp = new CFilesList(changes);
            while (temp.Count > 200)
                temp.RemoveRange(200, temp.Count - 200);
            foreach (var i in temp)
                changes.Remove(i);
            _push.PushFiles(changes);
        }


		var ex = _push.PushFiles(changes);
		if (null != ex)
		{
			var l = new CAudit_Error();
			l.ErrorUserID = CSession.User.UserLoginName;
			l.ErrorType = "webservice";
			l.ErrorStacktrace = ex.StackTrace;
			l.ErrorMessage = ex.Message;
			if (null != ex.Inner)
			{
				l.ErrorInnerMessage = ex.Inner.Message;
				l.ErrorInnerStacktrace = ex.Inner.StackTrace;
			}
			l.ErrorApplicationName = "Admin";
			l.ErrorUrl = Request.RawUrl;
			l.Save();
		}
		Response.Redirect(Request.RawUrl);
	}

	protected void rbl_SelectedIndexChanged(object sender, EventArgs e)
	{
		CSession.MonitorTab = rbl.SelectedIndex;
		Response.Redirect(Request.RawUrl, true);
	}
	protected void btnRefresh_Click(object sender, EventArgs e)
	{
		CSession.MyVersionReset(_push);
		Response.Redirect(Request.RawUrl, true);
	}
}