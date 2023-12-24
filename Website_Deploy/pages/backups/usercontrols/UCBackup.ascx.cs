using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;

public partial class pages_backups_usercontrols_UCBackup : UserControl 
{     
    #region Members
    private CBackup _backup; 
    private CBackupList _sortedList; 
    private int _pageIndex;
    #endregion 
    
    #region Interface
    public void Display(CBackup backup, CBackupList sortedList, CPagingInfo pi) 
    { 
        if (Parent.Controls.Count % 2 == 0) 
            row.Attributes.Add("class", "alt_row"); 
        
        _backup = backup; 
        _sortedList = sortedList; 
        _pageIndex = pi.PageIndex;
         
        CBackup bi = _backup; 
        litNumber.Text = Convert.ToString(sortedList.IndexOf(bi) + 1 + pi.PageIndex * pi.PageSize);
        lnkBackupInstanceId.Text = bi.InstanceName;
        lnkBackupInstanceId.NavigateUrl = CSitemap.Backups(int.MinValue, bi.BackupInstanceId, null);
        lnkBackupCreated.ToolTip = CUtilities.LongDateTime(bi.BackupCreated);
        lnkBackupCreated.Text = CUtilities.Timespan(bi.BackupCreated);
        lnkBackupCreated.NavigateUrl = CSitemap.Backup(bi.BackupId);
        litBackupDescription.Text = bi.BackupDescription;
        if (bi.BackupTableCount > 0)
            lblStored.Text = bi.BackupTableCount.ToString("n0");
        if (bi.CountTables > 0)
            lnkTables.Text = bi.CountTables.ToString("n0");
        lnkTables.NavigateUrl = CSitemap.BackupItems(int.MinValue, bi.BackupInstanceId, null, null);
        if (bi.TotalSize > 0)
            lblTotalSize.Text = bi.TotalSize.ToString("n0");
        lblTotalSize.ToolTip = bi.TotalSize_;

        var p = bi.Prev();
        if (null != p && p.CountTables != backup.CountTables)
            btnDelete.Visible = false;

        btnDelete.ID = "Btn_" + backup.BackupId.ToString();
    } 
    #endregion 
    
    #region Event Handlers 
    protected void btnDelete_Click(object sender, ImageClickEventArgs e) 
    {
        _backup.Delete(); 
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
