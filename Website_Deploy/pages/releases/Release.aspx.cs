using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;

partial class pages_Releases_Release : CPageDeploy
{
	#region Querystring
	public int AppId { get { return Release.ReleaseAppId ; } }

	public int ReleaseId
    {
        get
        {
            int id = CWeb.RequestInt("releaseId");
            if (id == int.MinValue)
                CSitemap.RecordNotFound("Release", id);
            return id;
        }
    }
    #endregion

    #region Data
    public CRelease Release
    {
        get
        {
            CRelease t = CRelease.Cache.GetById(ReleaseId);
            if (t == null)
                CSitemap.RecordNotFound("Release", ReleaseId);
            return t;
        }
    }
    #endregion

    #region Event Handlers - Page
    protected override void PageInit()
    {
		UnbindSideMenu();
		MenuReleaseDetails(AppId, ReleaseId);

        var r = this.Release;

		txtReleaseCreated.Text = CUtilities.Timespan(r.ReleaseCreated);
        txtReleaseCreated.ToolTip = CUtilities.LongDateTime(r.ReleaseCreated);

        if (null != r.App)
        {
            txtReleaseAppId.Text = r.App.AppName;
            txtReleaseAppId.NavigateUrl = CSitemap.AppEdit(r.ReleaseAppId);
        }

        if (null != r.Instance)
        {
            txtReleaseInstanceId.Text = r.Instance.IdAndName;
            txtReleaseInstanceId.NavigateUrl = CSitemap.Instance(r.ReleaseInstanceId);
        }
        else
            txtReleaseInstanceId.Text = "*";

        if (null != r.Version)
        {
            txtReleaseVersionId.Text = r.ReleaseVersionName;
            txtReleaseVersionId.NavigateUrl = CSitemap.VersionEdit(r.ReleaseVersionId);
        }
        txtReleaseBranchName.Text = r.ReleaseBranchName;
        txtReleaseExpired.Text = CUtilities.Timespan(r.ReleaseExpired);
        txtReleaseExpired.ToolTip = CUtilities.LongDateTime(r.ReleaseExpired);

        var idx = CRelease.Cache.IndexOf(r);
        if (idx < CRelease.Cache.Count - 2)
        {
            var prev = CRelease.Cache[idx + 1];
            while (prev != null && prev.ReleaseInstanceId != int.MinValue)
            {
                idx += 1;
                if (idx >= CRelease.Cache.Count)
                    prev = CRelease.Cache[idx];
                else
                    prev = null;
            }
            if (prev != null)
                txtPrevious.Text = prev.ReleaseVersionName;
        }
    }
    #endregion
}