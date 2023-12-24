using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;

partial class pages_UpgradeHistorys_UpgradeHistory : CPageDeploy
{    
    #region Querystring
    public int ChangeId 
    {
        get 
        {
            int id = CWeb.RequestInt("changeId");
            if (id == int.MinValue) 
                CSitemap.RecordNotFound("UpgradeHistory", id); 
            return id;
        }
    }
	public int AppId { get { return UpgradeHistory.Instance.InstanceAppId; } }
    #endregion
    
    #region Members
    private CUpgradeHistory _upgradeHistory;
    #endregion

    #region Data
    public CUpgradeHistory UpgradeHistory
    {
        get 
        {
            if (_upgradeHistory == null) 
            {
                try 
                {
                    _upgradeHistory = new CUpgradeHistory(ChangeId);
                }
                catch 
                {
                    CSitemap.RecordNotFound("UpgradeHistory", ChangeId);
                }
            }
            return _upgradeHistory;
        }
    }
    #endregion
    
    #region Event Handlers - Page
    protected override void PageInit()
    {        

        if (null != this.UpgradeHistory.ReportHistory)
        {
            txtChangeReportId.ValueDateTime = this.UpgradeHistory.ReportHistory.ReportAppStarted;
            txtChangeReportId.NavigateUrl = CSitemap.ReportHistoryEdit(this.UpgradeHistory.ChangeReportId);
        }

        if (null != this.UpgradeHistory.NewVersion)
        {
            txtChangeNewVersionId.Text = this.UpgradeHistory.NewVersion.VersionName;
            txtChangeNewVersionId.NavigateUrl = CSitemap.VersionEdit(this.UpgradeHistory.ChangeNewVersionId);
        }
        txtChangeNewSchemaMD5.Text = CBinary.ToBase64(this.UpgradeHistory.ChangeNewSchemaMD5);
        txtChangeStarted.ValueDate = this.UpgradeHistory.ChangeStarted;
        txtChangeFinished.ValueDate = this.UpgradeHistory.ChangeFinished;


		UnbindSideMenu();
		MenuAutoUpgradeSearch(AppId);
	}
    #endregion
}