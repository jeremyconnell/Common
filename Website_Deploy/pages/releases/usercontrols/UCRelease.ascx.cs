using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using SchemaAdmin;
using Framework;

public partial class pages_targetversionhistorys_usercontrols_UCRelease : UserControl 
{     
    #region Members
    private CRelease _targetReleasesMain; 
    private IList _sortedList; 
    #endregion 
    
    #region Interface
    public void Display(CRelease targetReleasesMain, IList sortedList) 
    { 
        if (Parent.Controls.Count % 2 == 0) 
            row.Attributes.Add("class", "alt_row"); 
        
        _targetReleasesMain = targetReleasesMain; 
        _sortedList = sortedList;
         

        CRelease h = _targetReleasesMain;
        litNumber.Text = Convert.ToString(sortedList.IndexOf(h) + 1);


        litReleaseCreated.NavigateUrl = CSitemap.ReleaseEdit(h.ReleaseId);

        litReleaseAppId.Text = h.App.AppName;

        if (null != h.Instance)
            litReleaseInstanceId.Text = h.Instance.IdAndName;
        else if (h.ReleaseVersionId == h.App.AppMainVersionId)
            litReleaseInstanceId.Text = CUtilities.NameAndCount("All", h.App.Instances.MainBranch.Count, "instance", "0");

        litReleaseVersionId.Text = h.ReleaseVersionName;
        litReleaseVersionId.ToolTip = h.ReleaseVersionName;
        litReleaseBranchName.Text = h.ReleaseBranchName;
        if (litReleaseBranchName.Text.Length == 0)
            litReleaseBranchName.Text = "Main";
        litReleaseCreated.Text = CUtilities.Timespan(h.ReleaseCreated);
        litReleaseExpired.Text = CUtilities.Timespan(h.ReleaseExpired);

    } 
    #endregion 
    
} 
