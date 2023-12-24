using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;

public partial class pages_versions_usercontrols_UCVersion : UserControl 
{     
    #region Members
    private CVersion _version; 
    private IList _sortedList; 
    #endregion 
    
    #region Interface
    public void Display(CVersion version, IList sortedList) 
    { 
        if (Parent.Controls.Count % 2 == 0) 
            row.Attributes.Add("class", "alt_row"); 
        
        _version = version; 
        _sortedList = sortedList;

        CVersion next = null;
        int index = sortedList.IndexOf(version);
        if (index + 1 < sortedList.Count)
            next = (CVersion)sortedList[index + 1];


        CVersion c = _version; 
        litNumber.Text = Convert.ToString(sortedList.IndexOf(c) + 1);
        lnkVersionName.Text = c.NumberAndName;
        lnkVersionName.NavigateUrl = CSitemap.VersionEdit(c.VersionId);
        if (c.VersionId == version.App.AppMainVersionId)
        {
            lnkVersionName.Text += "*";
            lnkVersionName.ToolTip = "Main Version";
        }
        litVersionTotalBytes.Text = c.VersionTotalFileSize;
        litVersionFileCount.Text = c.VersionFileCount_;
        litVersionFileCount.NavigateUrl = CSitemap.BinaryFiles(c.VersionAppId, c.VersionId);
        litVersionSchemaMD5.Text = c.VersionSchemaB64;
        litVersionFilesMD5.Text = c.VersionFilesB64;

        if (null != next)
        {
            var delta = version.Diff(next);

            litDiff.Text = CUtilities.CountSummary(delta.Count, "file", "none");
            litDiff.NavigateUrl = CSitemap.VersionDiff(c.VersionId, next.VersionId);
            litDiff.ToolTip = CUtilities.ListToString(delta.Names, "\r\n");

            litDiffSize.Text = CUtilities.FileSize(delta.Total());
        }

        //litVersionDeltaFileCount.Text = c.VersionDeltaFileCount_;
        //litVersionDeltaFileBytes.Text = c.VersionDeltaFileSize;

        lnkUsage.Text = c.Usage_.Replace("\r\n", "<br/>");
        if (c.MainCount + c.BranchCount > 0)
            lnkUsage.NavigateUrl = CSitemap.InstancesForTargetVersion(c.App.AppId, c.VersionId);

        litVersionCreated.ToolTip = CUtilities.LongDateTime(c.VersionCreated);
        litVersionExpires.ToolTip = CUtilities.LongDateTime(c.VersionExpires);
        litVersionCreated.Text = CUtilities.Timespan(c.VersionCreated).Replace(" ago", "");
        litVersionExpires.Text = CUtilities.Timespan(c.VersionExpires);

        if (c.VersionFilesMD5 == Guid.Empty)
            if (c.VersionFiles.Count > 0)
            {
                c.VersionFilesMD5 = c.BinaryFiles.TotalHash;
                c.Save();
            }

        if (c.VersionFiles.Count > 0)
            btnDelete.Visible = false;

		var lastReport = new CInstanceList();
		foreach (var i in CInstance.Cache)
			if (null != i.LastReport_ && i.LastReport_.ReportInitialVersionId == c.VersionId)
				lastReport.Add(i);
		if (lastReport.Count > 0)
		{
			lnkActive.Text = CUtilities.CountSummary(lastReport, "deploy", "", "deploys");
			lnkActive.ToolTip = CUtilities.ListToString(lastReport.Names, "\r\n");
            lnkActive.NavigateUrl = CSitemap.InstancesForLastVersion(c.App.AppId, c.VersionId);

		}

	}
	#endregion

	#region Event Handlers 
	protected void btnDelete_Click(object sender, ImageClickEventArgs e)
    {
        if (_version.VersionId == _version.App.AppMainVersionId)
        {
            _version.App.AppMainVersionId = int.MinValue;
            _version.App.Save();
        }
        _version.Delete(); 
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
