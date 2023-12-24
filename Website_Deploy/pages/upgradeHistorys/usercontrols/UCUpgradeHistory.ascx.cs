using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;

public partial class pages_upgradehistorys_usercontrols_UCUpgradeHistory : System.Web.UI.UserControl 
{     
    #region Interface
    public void Display(CUpgradeHistory upgradeHistory, CUpgradeHistoryList list, CPagingInfo pi) 
    { 
        if (Parent.Controls.Count % 2 == 0) row.Attributes.Add("class", "alt_row"); 
        
        CUpgradeHistory c = upgradeHistory;
        litNumber.Text = Convert.ToString(list.IndexOf(c) + 1 + pi.PageIndex * pi.PageSize);
        litChangeReportId.Text = CUtilities.Timespan( c.ReportHistory.ReportAppStarted);
        litChangeNewVersionId.Text = c.NewVersion.VersionName;
        litChangeNewSchemaMD5.Text = CBinary.ToBase64( c.ChangeNewSchemaMD5);
        litChangeStarted.Text = CUtilities.Timespan(c.ChangeStarted);
        litChangeStarted.ToolTip = CUtilities.LongDateTime(c.ChangeStarted);
        litChangeFinished.Text = CUtilities.Timespan(c.ChangeFinished);
        litChangeFinished.ToolTip = CUtilities.LongDateTime(c.ChangeFinished);
    } 
    #endregion
} 
