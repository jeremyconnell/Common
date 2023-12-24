using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;
using System.Net;
using SchemaAdmin.api;
using Microsoft.WindowsAzure.Management.WebSites.Models;
using System.IO;
using static Microsoft.WindowsAzure.Management.WebSites.Models.WebSiteGetPublishProfileResponse;
using System.Text;
using SchemaAudit;
using System.Threading.Tasks;
using System.Threading;

public partial class pages_instances_ftp : CPageDeploy
{
	#region Querystring
	public int InstanceId { get { return CWeb.RequestInt("instanceId"); } }
	public int AppId { get { return null != Instance ? Instance.InstanceAppId : CWeb.RequestInt("appId"); } }
	public string SubDir { get { return CWeb.RequestStr("subDir"); } }
	public string View { get { return CWeb.RequestStr("view"); } }
	public string Edit { get { return CWeb.RequestStr("edit"); } }
	#endregion

	#region Data
	public CInstance Instance { get { return CInstance.Cache.GetById(InstanceId); } }
	public CApp App { get { return CApp.Cache.GetById(AppId); } }
	public WebSite AzureWeb
	{
		get
		{
			foreach (var i in CAzureManagement.Web.WebSites_Cached())
				if (i.Name.ToLower() == Instance.InstanceWebNameAzure.ToLower())
					return i;
			return null;
		}
	}
	private WebSite _website;
	public WebSite WebSite
	{
		get
		{
			if (null == _website)
				foreach (WebSite i in CAzureManagement.Web.WebSites_Cached())
					if (i.Name.ToLower() == Instance.InstanceWebNameAzure)
					{
						_website = i;
						break;
					}
			return _website;
		}
	}
	public WebSiteGetPublishProfileResponse.PublishProfile FtpProfile
	{
		get
		{
			var w = WebSite;
			if (null == w) return null;
			foreach (var i in CAzureManagement.Web.PublishProfile_Cached(w.Name, w.WebSpace))
				if (i.PublishMethod == "FTP")
					return i;
			return null;
		}
	}
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

		ddVersion.DataSource = CApp.Cache.ControlTrack.Versions;
		ddVersion.DataBind();
		CDropdown.BlankItem(ddVersion, "-- Select a Version to Push --");
		//CDropdown.SetValue(ddVersion, )

		MenuInstanceFtp(AppId, InstanceId);
	}
	protected override void PageLoad()
	{
		ListFilesAndFolders();


		colViewEdit.Visible = (Edit.Length + View.Length > 0);
		try
		{
			if (Edit.Length > 0)
			{
				txtEdit.Text = DownloadFile(FtpProfile, SubDir, Edit);
				txtEdit.Enabled = true;
				btnEdit.Visible = false;
				btnSave.Visible = true;
				btnCancel.Visible = true;
				txtFileName.Visible = true;
				txtFileName.Text = Edit;
			}
			else if (View.Length > 0)
				txtEdit.Text = DownloadFile(FtpProfile, SubDir, View);
		}
		catch
		{ Response.Redirect(CSitemap.InstanceFtp(InstanceId, SubDir)); }

	}
	protected override void PagePreRender()
	{
		var versionId = CDropdown.GetInt(ddVersion);
		var v = CVersion.Cache.GetById(versionId);
		btnPush.Visible = (null != v);

		var w = this.AzureWeb;
		if (null == w)
			return;
		var pp = CAzureManagement.Web.PublishProfile_Cached(w.Name, w.WebSpace);
		if (pp.Count == 2)
		{
			UCPublishProfile(pnlProfiles).Display(pp[1]);
			UCPublishProfile(pnlProfiles).Display(pp[0]);
		}
		else
			foreach (var j in pp)
				UCPublishProfile(pnlProfiles).Display(j);
	}
	#endregion

	#region Form Events
	protected void ddApp_SelectedIndexChanged(object sender, EventArgs e)
	{
		var app = CApp.Cache.GetById(CDropdown.GetInt(ddApp));
		Response.Redirect(CSitemap.InstanceFtp(app.Instances[0].InstanceId), true);
	}
	protected void ddIns_SelectedIndexChanged(object sender, EventArgs e)
	{
		Response.Redirect(CSitemap.InstanceFtp(CDropdown.GetInt(ddIns)), true);
	}
	protected void ddVersion_SelectedIndexChanged(object sender, EventArgs e)
	{
		var versionId = CDropdown.GetInt(ddVersion);
		var v = CVersion.Cache.GetById(versionId);
		ctrl.Display(v.VersionFiles);
		ctrl.Visible = v.VersionFiles.Count > 0;
		divFiles.Visible = v.VersionFiles.Count > 0;
	}


	protected void btnEdit_Click(object sender, EventArgs e)
	{
		Response.Redirect(CSitemap.InstanceFtpEdit(InstanceId, SubDir, View));
	}

	protected void btnSave_Click(object sender, EventArgs e)
	{
		string path = string.Concat(SubDir, "/", txtFileName.Text);
		UploadText_WithRetry(FtpProfile, path, txtEdit.Text);
		Response.Redirect(CSitemap.InstanceFtpView(InstanceId, SubDir, txtFileName.Text));
	}

	protected void btnCancel_Click(object sender, EventArgs e)
	{
		Response.Redirect(CSitemap.InstanceFtpView(InstanceId, SubDir, Edit));
	}


	protected void btnPush_Click(object sender, EventArgs e)
	{
		var versionId = CDropdown.GetInt(ddVersion);
		var v = CVersion.Cache.GetById(versionId);
		if (null == v)
			return;

		var ftp = FtpProfile;
		var sb = new StringBuilder();
		try
		{
			Push(v.VersionFiles.Root, ftp, sb);
		}
		catch (Exception ex)
		{
			CAudit_Error.Log(ex);
			CSession.PageMessageEx = ex;
		}


		CSession.PageMessage = sb.ToString();
		Response.Redirect(Request.RawUrl);
	}
	#endregion

	#region "Push Files"
	private void Push(CDir version, PublishProfile ftp, StringBuilder sb = null, string subDir = "webapi")
	{
        var pi = new ParallelOptions();
        pi.MaxDegreeOfParallelism = 1;

        //Create Dirs
        Parallel.ForEach(version.DirNamesRecursive, pi, dirName => 
            {
                var path = string.Concat(subDir, "/", dirName).Replace("\\", "/");
                try
                {
                    var existing = ListDir(ftp, path);
                    sb.Append(path).AppendLine("\tExists");
                }
                catch
                {
                    try
                    {
                        CreateDir_WithRetry(ftp, path);
                        sb.Append(path).AppendLine("\tCreated");
                    }
                    catch (Exception ex)
                    {
                        sb.Append(path).AppendLine("\tCREATE-DIR ERROR: " + ex.Message);
                    }
                }
            });

        //Create Files
        Parallel.ForEach(version.FilesRecursive, pi, pathAndFile => 
            {
                var path = string.Concat(subDir, "/", pathAndFile.Key).Replace("\\", "/");
                try
                {
                    //DeleteFile(ftp, path);
                    sb.Append(path).Append("\t").AppendLine("Deleted (old)");
                } catch { }

                Thread.Sleep(500);
                var bin = pathAndFile.Value.FileAsBytes;
                try
                {
                    UploadFile_WithRetry(ftp, path, bin, 1);
                    sb.Append(path).Append("\t").AppendLine("Uploaded");
                }
                catch (Exception ex)
                {
                    sb.Append(path).Append("\t").AppendLine("UPLOAD ERROR: " + ex.Message);
                }
            });


		//TODO: Clean-up
	}
	#endregion


	#region "List Files/Folders"
	public void ListFilesAndFolders()
	{
		if (null == FtpProfile)
			return;


		var tr = Row(tblDir);
		CellH(tr, "Folders");
		if (SubDir.Length > 0)
		{
			tr = Row(tblDir);
			CellLink(tr, "..", CSitemap.InstanceFtp(InstanceId), false).Style.Add("padding", "2px 4px");
		}

		var f = FtpProfile;
		CFtpFiles files = null;
		try
		{
			files = ListDir(f, SubDir);
		}
		catch
		{
			if (string.IsNullOrEmpty(SubDir))
			{
				files = ListDir(FtpUrl(f), f.UserName, f.UserPassword);
				CreateDir(FtpUrl(f), f.UserName, f.UserPassword);
				Response.Redirect(CSitemap.InstanceFtp(InstanceId, SubDir));
				files = ListDir_WithRetry(f, SubDir, 3);
			}
		}

		try
		{
			colFiles.Visible = false;
			foreach (var i in files)
			{
				if (i.IsDir)
				{
					tr = Row(tblDir);
					tr.CssClass = "";
					CellLink(tr, "/" + i.Name, CSitemap.InstanceFtp(InstanceId, SubDir + "/" + i.Name), false);
				}
				else
				{
					if (tblFile.Rows.Count == 0)
					{
						tr = Row(tblFile);
						CellH(tr, "Files");
						CellH(tr, "Size");
						CellH(tr, "Edit&raquo;");
					}
					tr = Row(tblFile);
					tr.CssClass = "";
					CellLink(tr, i.Name, CSitemap.InstanceFtpView(InstanceId, SubDir, i.Name),false, false, i.Date);
					Cell(tr, CUtilities.FileSize(i.Size), i.Size.ToString("n0"));
					CellLinkR(tr, "Edit", CSitemap.InstanceFtpEdit(InstanceId, SubDir, i.Name), false, false, i.Size.ToString("n0"));
					colFiles.Visible = true;
				}
			}
		}
		catch (Exception ex)
		{
			CSession.PageMessageEx = ex;
			tblDir.Visible = false;
			colFiles.Visible = false;
		}
	}
	#endregion


	#region User Controls
	private static pages_instances_usercontrols_UCPublishProfile UCPublishProfile(Control target)
	{
		Control ctrl = target.Page.LoadControl(CSitemap.UCPublishProfile());
		target.Controls.Add(ctrl);
		return (pages_instances_usercontrols_UCPublishProfile)ctrl;
	}
	#endregion


	#region FTP
	private CFtpFiles ListDir_WithRetry(WebSiteGetPublishProfileResponse.PublishProfile i, string relPath, int maxAttempts = 3)
	{
		try { return ListDir(i, relPath); }
		catch { if (maxAttempts == 0) throw; return ListDir_WithRetry(i, relPath, maxAttempts - 1); }
	}
	private bool CreateDir_WithRetry(WebSiteGetPublishProfileResponse.PublishProfile i, string relPath, int maxAttempts = 3)
	{
		try { return CreateDir(i, relPath); }
		catch { if (maxAttempts == 0) throw; return CreateDir_WithRetry(i, relPath, maxAttempts - 1); }
	}
	private bool DeleteFile_WithRetry(WebSiteGetPublishProfileResponse.PublishProfile i, string relPath, int maxAttempts = 3)
	{
		try { return DeleteFile(i, relPath); }
		catch { if (maxAttempts == 0) throw; return DeleteFile_WithRetry(i, relPath, maxAttempts - 1); }
	}

	private bool UploadFile_WithRetry(WebSiteGetPublishProfileResponse.PublishProfile i, string relPath, byte[] content, int maxAttempts = 3)
	{
		try { return UploadFile(i, relPath, content); }
		catch { if (maxAttempts == 0) throw; return UploadFile_WithRetry(i, relPath, content, maxAttempts - 1); }
	}
	private bool UploadText_WithRetry(WebSiteGetPublishProfileResponse.PublishProfile i, string relPath, string content, int maxAttempts = 3)
	{
		try { return DeleteFile(i, relPath); } catch { }

		try { return UploadText(i, relPath, content); }
		catch
		{
			if (maxAttempts == 0) throw;
			return UploadText_WithRetry(i, relPath, content, maxAttempts - 1);
		}
	}



	private string DownloadFile(WebSiteGetPublishProfileResponse.PublishProfile i, string relPath, string fileName)
	{
		relPath = string.Concat(relPath, "/", fileName);
		return DownloadFile(FtpUrl(i) + relPath, i.UserName, i.UserPassword);
	}
	private bool UploadFile(WebSiteGetPublishProfileResponse.PublishProfile i, string relPath, byte[] content)
	{
		return UploadFile(FtpUrl(i) + relPath, i.UserName, i.UserPassword, content);
	}
	private bool UploadText(WebSiteGetPublishProfileResponse.PublishProfile i, string relPath, string content)
	{
		return UploadText(FtpUrl(i) + relPath, i.UserName, i.UserPassword, content);
	}
	private bool CreateDir(WebSiteGetPublishProfileResponse.PublishProfile i, string checkedPath)
	{
		return CreateDir(FtpUrl(i) + checkedPath, i.UserName, i.UserPassword);
	}
	private bool CheckOrCreate(WebSiteGetPublishProfileResponse.PublishProfile i, string relPath)
	{
		var bits = relPath.Split('/').ToList<string>();
		return CheckOrCreate_Recurse(i, bits, string.Empty);
	}
	private bool CheckOrCreate_Recurse(WebSiteGetPublishProfileResponse.PublishProfile i, List<string> bits, string checkedPath)
	{
		if (bits.Count == 0)
			return CheckOrCreate_SingleStep(i, checkedPath);

		checkedPath = string.Concat(checkedPath, "/", bits[0]);
		bits.RemoveAt(0);
		return CheckOrCreate_Recurse(i, bits, checkedPath);
	}
	private bool CheckOrCreate_SingleStep(WebSiteGetPublishProfileResponse.PublishProfile i, string relPath)
	{
		try { ListDir(i, relPath); return false; }
		catch { CreateDir(i, relPath); return true; }
	}

	private bool DeleteDir(WebSiteGetPublishProfileResponse.PublishProfile i, string relPath)
	{
		return DeleteDir(FtpUrl(i) + relPath, i.UserName, i.UserPassword);
	}
	private bool DeleteFile(WebSiteGetPublishProfileResponse.PublishProfile i, string relPath)
	{
		return DeleteFile(FtpUrl(i) + relPath, i.UserName, i.UserPassword);
	}
	private CFtpFiles ListDir(WebSiteGetPublishProfileResponse.PublishProfile i, string subDir = "")
	{
		return ListDir(FtpUrl(i) + subDir, i.UserName, i.UserPassword);
	}

	private const string _WEBAPI  = "webapi";
	private const string _WWWROOT = "wwwroot";
	private const string _JSON = "wwwroot/app/settings/config";
	private string FtpUrl(WebSiteGetPublishProfileResponse.PublishProfile i, string subDir)
	{
		if (subDir.StartsWith("/")) subDir = subDir.Substring(1);
		if (!subDir.EndsWith("/")) subDir = subDir + "/";
		return string.Concat(FtpUrl(i), subDir);	//Trailing slash convention
	}
	private string FtpUrl(WebSiteGetPublishProfileResponse.PublishProfile i)	{	return i.PublishUrl.Replace(_WWWROOT, string.Empty);	}



	private bool UploadFile(string host, string user, string pass, byte[] content)
	{
		FtpWebRequest req = Prepare(host, user, pass, WebRequestMethods.Ftp.UploadFile);
		var bw = new BinaryWriter(req.GetRequestStream());
		bw.Write(content);
		bw.Close();

		FtpWebResponse resp = (FtpWebResponse)req.GetResponse();
		string d = resp.StatusDescription;
		string c = resp.StatusCode.ToString();

		Stream respStr = resp.GetResponseStream();
		StreamReader sr = new StreamReader(respStr);
		string s = sr.ReadToEnd();

		sr.Close();
		resp.Close();

		return true;
	}
	private bool UploadText(string host, string user, string pass, string content)
	{
		FtpWebRequest req = Prepare(host, user, pass, WebRequestMethods.Ftp.UploadFile);
		var sw = new StreamWriter(req.GetRequestStream());
		sw.Write(content);
		sw.Close();

		//var bw = new BinaryWriter(req.GetRequestStream());
		//bw.Write(content);
		//bw.Close();

		FtpWebResponse resp = (FtpWebResponse)req.GetResponse();
		string d = resp.StatusDescription;
		string c = resp.StatusCode.ToString();

		//sr.Close();
		resp.Close();

		return true;
	}
	private string DownloadFile(string host, string user, string pass)
	{
		FtpWebRequest req = Prepare(host, user, pass, WebRequestMethods.Ftp.DownloadFile);

		FtpWebResponse resp = (FtpWebResponse)req.GetResponse();
		string d = resp.StatusDescription;
		string c = resp.StatusCode.ToString();

		Stream respStr = resp.GetResponseStream();
		StreamReader sr = new StreamReader(respStr);
		string s = sr.ReadToEnd();

		sr.Close();
		resp.Close();

		return s;
	}
	private bool CreateDir(string host, string user, string pass)
	{
		FtpWebRequest req = Prepare(host, user, pass, WebRequestMethods.Ftp.MakeDirectory);

		FtpWebResponse resp = (FtpWebResponse)req.GetResponse();
		string d = resp.StatusDescription;
		string c = resp.StatusCode.ToString();

		Stream respStr = resp.GetResponseStream();
		StreamReader sr = new StreamReader(respStr);
		string s = sr.ReadToEnd();

		sr.Close();
		resp.Close();

		return true;
	}
	private bool DeleteDir(string host, string user, string pass)
	{
		FtpWebRequest req = Prepare(host, user, pass, WebRequestMethods.Ftp.RemoveDirectory);

		FtpWebResponse resp = (FtpWebResponse)req.GetResponse();
		string d = resp.StatusDescription;
		string c = resp.StatusCode.ToString();

		Stream respStr = resp.GetResponseStream();
		StreamReader sr = new StreamReader(respStr);
		string s = sr.ReadToEnd();

		sr.Close();
		resp.Close();

		return true;
	}
	private bool DeleteFile(string host, string user, string pass)
	{
		FtpWebRequest req = Prepare(host, user, pass, WebRequestMethods.Ftp.DeleteFile);

		FtpWebResponse resp = (FtpWebResponse)req.GetResponse();
		string d = resp.StatusDescription;
		string c = resp.StatusCode.ToString();

		Stream respStr = resp.GetResponseStream();
		StreamReader sr = new StreamReader(respStr);
		string s = sr.ReadToEnd();

		sr.Close();
		resp.Close();

		return true;
	}
	private CFtpFiles ListDir(string host, string user, string pass)
	{
		FtpWebRequest req = Prepare(host, user, pass, WebRequestMethods.Ftp.ListDirectoryDetails);

		FtpWebResponse resp = (FtpWebResponse)req.GetResponse();
		string d = resp.StatusDescription;
		string c = resp.StatusCode.ToString();

		Stream respStr = resp.GetResponseStream();
		StreamReader sr = new StreamReader(respStr);
		string s = sr.ReadToEnd();

		sr.Close();
		resp.Close();

		return new CFtpFiles(s);
	}

	private FtpWebRequest Prepare(string host, string user, string pass, string method)
	{
		FtpWebRequest req = (FtpWebRequest)WebRequest.Create(host); 
		req.Credentials = new NetworkCredential(user, pass);
		req.Method = method;
		return req;
	}

	private class CFtpFiles : List<CFtpFile>
	{
		public CFtpFiles() : base() { }
		public CFtpFiles(int count) : base(count) { }
		public CFtpFiles(string s) : this(CUtilities.StringToListStr(s, "\r\n")) { }
		public CFtpFiles(List<string> list)
		{
			foreach (var i in list)
				if (!String.IsNullOrEmpty(i))
					Add(new CFtpFile(i));
		}

		public bool ContainsName(string name)
		{
			if (name.StartsWith("/")) name = name.Substring(1);
			return this.Names.Contains(name.ToLower());
		}
		public bool ContainsFile(string name)
		{
			name = name.ToLower();
			foreach (var i in this)
				if (!i.IsDir && i.Name.ToLower() == name)
					return true;
			return false;
		}

		private List<string> _names;
		public List<string> Names
		{
			get
			{
                if (null == _names)
                {
                    var list = new List<string>(this.Count);
                    foreach (var i in this)
                        list.Add(i.Name.ToLower());
                    _names = list;
                }
				return _names;
			}
		}
		private CFtpFiles _dirs;
		public CFtpFiles Dirs
		{
			get
			{
				if (null == _dirs)
				{
					_dirs = new CFtpFiles(this.Count);
					foreach (var i in this)
						if (i.IsDir)
							_dirs.Add(i);
				}
				return _dirs;
			}
		}
		private CFtpFiles _files;
		public CFtpFiles Files
		{
			get
			{
				if (null == _files)
				{
					_files = new CFtpFiles(this.Count);
					foreach (var i in this)
						if (!i.IsDir)
							_files.Add(i);
				}
				return _files;
			}
		}
	}
	private class CFtpFile
	{
		public CFtpFile(string s)
		{
			var list = CUtilities.SplitOn(s, "       ");
			Date = list[0].Trim();
			if (list.Count > 1)
				TypeOrSize = list[1].Trim();
			if (list.Count > 2)
				Name = list[2].Trim();

			if (!IsDir && 0 == Size)
			{
				list = CUtilities.SplitOn(Name, " ");
				long size = 0;
				if (list.Count > 1 && long.TryParse(list[0], out size))
				{
					this.TypeOrSize = size.ToString();
					Name = Name.Substring(Name.IndexOf(" ") + 1);
				}
			}
			
		}


		public string Date;
		public string TypeOrSize;
		public string Name;

		public bool IsDir { get { return "<DIR>" == TypeOrSize; } }
		public long Size { get { long l = 0; long.TryParse(TypeOrSize, out l); return l; } }
		public DateTime Date_ { get { DateTime d = DateTime.MinValue; DateTime.TryParse(Date.Replace("AM", " AM").Replace("PM", " PM"), out d); return d; } }

		public override string ToString()
		{
			if (IsDir)
				return "DIR: " + Name;
			else
				return CUtilities.FileNameAndSize(Name, Size);
		}
	}
	#endregion


	protected void btnMakeDir_Click(object sender, EventArgs e)
	{
		if (txtNewDir.Text.Length > 0)
			try
			{
				CreateDir(FtpProfile, string.Concat(SubDir, "/", txtNewDir.Text));
				CSession.PageMessage = "Created Dir: " + txtNewDir.Text;
			}
			catch (Exception ex)
			{
				CSession.PageMessageEx = ex;
			}

		Response.Redirect(Request.RawUrl);
	}
}