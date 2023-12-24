using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;

public partial class pages_reporthistorys_usercontrols_UCReportHistory : UserControl 
{     
    #region Members
    private CReportHistory _reportHistory; 
    private CReportHistoryList _sortedList; 
    private int _pageIndex;
    #endregion 
    
    #region Interface
    public void Display(CReportHistory reportHistory, CReportHistoryList sortedList, CPagingInfo pi) 
    { 
        if (Parent.Controls.Count % 2 == 0) 
            row.Attributes.Add("class", "alt_row"); 
        
        _reportHistory = reportHistory; 
        _sortedList = sortedList; 
        _pageIndex = pi.PageIndex;
         
        CReportHistory c = _reportHistory;
		CInstance i = c.Instance;

        litNumber.Text = Convert.ToString(sortedList.IndexOf(c) + 1 + pi.PageIndex * pi.PageSize);

        litReportInstanceId.Text = i.IdAndName;
		litReportInstanceId.NavigateUrl = CSitemap.ReportHistorys(i.InstanceAppId, i.InstanceId);

		litReportInitialVersionId.ToolTip = c.InitialVersion?.VersionName ?? "#" + c.ReportInitialVersionId.ToString();
		litReportInitialVersionId.Text = c.InitialVersion?.NumberAndName ?? "#" + c.ReportInitialVersionId.ToString();
		litReportInitialVersionId.NavigateUrl = CSitemap.VersionEdit(c.ReportInitialVersionId);
        litReportInitialVersionId.Enabled = null != c.InitialVersion;

		litReportInitialSchemaMD5.Text = c.ReportInitialSchemaB64;
		litReportInitialSchemaMD5.NavigateUrl = CSitemap.Schema(c.ReportInitialSchemaMD5);

        litReportAppStarted.Text = CUtilities.Timespan(c.ReportAppStarted);
		litReportAppStarted.ToolTip = CUtilities.LongDateTime	(c.ReportAppStarted);
		litReportAppStopped.Text = c.RanFor_;

    } 
    #endregion 
    
    #region Event Handlers 
    protected void btnDelete_Click(object sender, ImageClickEventArgs e) 
    {
        _reportHistory.Delete(); 
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
