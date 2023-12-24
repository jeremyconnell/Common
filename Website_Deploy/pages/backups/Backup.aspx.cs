using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;

partial class pages_Backups_Backup : CPageDeploy
{
    #region Querystring
    public int BackupId
    {
        get
        {
            int id = CWeb.RequestInt("backupId");
            if (id == int.MinValue)
                Response.Redirect(CSitemap.Backups(), true);
            return id;
        }
    }
    #endregion

    #region Members
    private CBackup _backup;
    #endregion

    #region Data
    public CBackup Backup
    {
        get
        {
            if (_backup == null)
            {
                try
                {
                    _backup = new CBackup(BackupId);
                }
                catch
                {
                    Response.Redirect(CSitemap.Backups(), true);
                }
            }
            return _backup;
        }
    }
    #endregion

    #region Event Handlers - Page
    protected override void PageInit()
    {
        var b = this.Backup;
        var i = b.Instance;
        var n = i.NameAndSuffix ?? "*";

        //Populate Dropdowns
        MenuAppBackup(i.App.AppId, b);

        lnkBackupInstanceId.Text = n;
        lnkBackupInstanceId.NavigateUrl = CSitemap.Backups(int.MinValue, b.BackupInstanceId);

        txtBackupCreated.Value   = CUtilities.Timespan(this.Backup.BackupCreated);
        txtBackupCreated.ToolTip = CUtilities.LongDateTime(this.Backup.BackupCreated);

        txtBackupDescription.Text = this.Backup.BackupDescription.Replace("\r\n", "<Br/>");

        litTables.Text = CUtilities.CountSummary(b.BackupTableCount, "table", "none");
        litBinaries.Text = b.CountSummary;
        litTotalSize.Text = b.TotalSize_;

        ctrlBackupItems.Display(b.BackupItemsAll);
    }
    #endregion
}
