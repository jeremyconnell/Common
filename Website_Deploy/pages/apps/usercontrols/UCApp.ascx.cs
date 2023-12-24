using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;
using System.Web.UI.HtmlControls;

public partial class pages_apps_usercontrols_UCApp : UserControl 
{     
    #region Members
    private CApp _app; 
    private IList _sortedList; 
    #endregion 
    
    #region Interface
    public void Display(CApp app, IList sortedList) 
    { 
        if (Parent.Controls.Count % 2 == 0) 
            row.Attributes.Add("class", "alt_row"); 
        
        _app = app; 
        _sortedList = sortedList;
         
        CApp c = _app; 
        litNumber.Text = Convert.ToString(sortedList.IndexOf(c) + 1);
        lnkAppName.Text = (string)(c.AppName.Length == 0 ? "..." : c.AppName); 
        lnkAppName.NavigateUrl = CSitemap.AppEdit(c.AppId);

        var m = c.MainVersion;
        if (null != m)
        {
            litAppMainVersionId.Text = m.VersionName;
            litAppMainVersionId.NavigateUrl = CSitemap.VersionEdit(m.VersionId);
            litMainVerFiles.Text = m.VersionFileCount_ + " (" + m.VersionTotalFileSize + ")";
        }

        litAppKeepOldFilesForDays.Text = CUtilities.CountSummary( c.AppKeepOldFilesForDays, "day", "none", "days");
        //litAppKeepMinVersions.Text = CUtilities.CountSummary(c.AppKeepMinVersions, "ver", "none", "vers");
        litAppCreated.Text = CUtilities.Timespan(c.AppCreated);

        lnkInstances.NavigateUrl = CSitemap.Instances(app.AppId);
        lnkInstances.Text = CUtilities.CountSummary(app.Instances, "deployment", " ");
		AddLink(pnlInstances, "+new deployment&raquo;", CSitemap.InstanceAdd(app.AppId), "Deploy new " + app.AppName);
		foreach (var i in app.Instances)
			AddLink(pnlInstances, i.NameAndSuffix, CSitemap.Instance(i.InstanceId));

        lnkBranches.NavigateUrl = CSitemap.Instances(app.AppId);
        lnkBranches.Text = CUtilities.CountSummary(app.Instances.SideBranch, "branch", " ");

        lnkVersions.NavigateUrl = CSitemap.Versions(app.AppId);
        lnkVersions.Text = CUtilities.CountSummary(app.Versions, "version", " ");
		AddLink(pnlVersions, "+upload new version&raquo;", CSitemap.VersionAdd(app.AppId));
		foreach (var i in app.Versions)
			AddLink(pnlVersions, i.VersionName, CSitemap.VersionEdit(i.VersionId));

		lnkReleases.NavigateUrl = CSitemap.Releases(app.AppId);
		lnkReleases.Text = CUtilities.CountSummary(app.Releases, "release", "");
		foreach (var i in app.Releases)
			AddLink(pnlReleases, i.ReleaseName, CSitemap.ReleaseEdit(i.ReleaseId));

		lnkFiles.NavigateUrl = CSitemap.BinaryFiles(c.AppId, c.AppMainVersionId);
        lnkFiles.Text = CUtilities.CountSummary(app.BinaryFiles, "file", " ") + " (" + app.TotalBytes_ + ")";

    }
	private void AddLink(Control plh, string name, string url,string ttip = null)
	{
		var div = new HtmlGenericControl("li");
		plh.Controls.Add(div);

		var lnk = new HyperLink();
		div.Controls.Add(lnk);

		lnk.Text = CUtilities.Truncate(name);
		lnk.NavigateUrl = url;
		lnk.ToolTip = ttip??name;
	}
    #endregion 
    
    #region Event Handlers 
    protected void btnDelete_Click(object sender, ImageClickEventArgs e) 
    {
        _app.Delete(); 
        Refresh(); 
    } 
    #endregion 
    
    #region Private
    private void Refresh() 
    { 
        Response.Redirect(Request.RawUrl); 
    } 
    #endregion 
} 
