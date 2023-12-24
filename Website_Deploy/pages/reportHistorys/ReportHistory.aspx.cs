using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;

partial class pages_ReportHistorys_ReportHistory : CPageDeploy
{
    #region Querystring
    public int ReportId { get { return CWeb.RequestInt("reportId"); } }
    public int InstanceId { get { return ReportHistory.ReportInstanceId; } }
	public int AppId { get { return ReportHistory.Instance.InstanceAppId; } }
	public bool IsEdit { get { return ReportId != int.MinValue; } }
    #endregion

    #region Members
    private CReportHistory _reportHistory;
    #endregion
    
    #region Data
    public CReportHistory ReportHistory
    {
        get 
        {
            if (_reportHistory == null) 
            {
                if (IsEdit) 
                {
                    _reportHistory = new CReportHistory(ReportId);
                    if (_reportHistory == null)
                        CSitemap.RecordNotFound("ReportHistory", ReportId); 
                }
                else 
                {
                    _reportHistory = new CReportHistory();
                }
            }
            return _reportHistory;
        }
    }
    #endregion
    
    #region Navigation
    private void Refresh()      { Response.Redirect(CSitemap.ReportHistoryEdit(this.ReportHistory.ReportId)); }
    private void ReturnToList() { Response.Redirect(CSitemap.ReportHistorys(InstanceId)); }
    #endregion

    #region Event Handlers - Page
    protected override void PageInit()
    {
        ddReportInstanceId.DataTextField = "IdAndName";
        ddReportInstanceId.DataValueField = "InstanceId";
        ddReportInstanceId.DataSource = CInstance.Cache;
        ddReportInstanceId.DataBind();
        ddReportInstanceId.BlankItem("-- Select Instance --");

        ddReportInitialVersionId.DataTextField = "VersionName";
        ddReportInitialVersionId.DataValueField = "VersionId";
        ddReportInitialVersionId.DataSource = CVersion.Cache;
        ddReportInitialVersionId.DataBind();
        ddReportInitialVersionId.BlankItem("-- Initial Version --");


        //Page title
        if (IsEdit) 
            this.Title = "Report History Details";
        else 
            this.Title = "Create New Report History";



		UnbindSideMenu();
		MenuReportDetails(AppId, ReportId);

		//Textbox logic
		//this.Form.DefaultFocus  = txtReportHistoryName.Textbox.ClientID;
		this.Form.DefaultButton = btnSave.UniqueID;   //txtReportHistoryName.OnReturnPress(btnSave);

        //Button Text
        btnDelete.Visible = IsEdit;
        if (IsEdit)
            btnCancel.Text = "Back";
        else
            Response.Redirect(CSitemap.ReportHistorys(InstanceId));
    }
    protected override void PageLoad()
    {
        LoadReportHistory();
    }
    #endregion

    #region Event Handlers - Form
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!this.IsValid)
            return;

        SaveReportHistory();
        Refresh();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ReturnToList();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        this.ReportHistory.Delete();
        ReturnToList();
    }
    #endregion
    
    #region Private - Load/Save
    protected void LoadReportHistory()
    {
        ddReportInstanceId.ValueInt = this.ReportHistory.ReportInstanceId;
        ddReportInitialVersionId.ValueInt = this.ReportHistory.ReportInitialVersionId;
        txtReportInitialSchemaMD5.Text = CBinary.ToBase64(this.ReportHistory.ReportInitialSchemaMD5);
        txtReportAppStarted.ValueDate = this.ReportHistory.ReportAppStarted;
        txtReportAppStopped.ValueDate = this.ReportHistory.ReportAppStopped;
    }
    protected void SaveReportHistory()
    {
        this.ReportHistory.ReportInstanceId = ddReportInstanceId.ValueInt;
        this.ReportHistory.ReportInitialVersionId = ddReportInitialVersionId.ValueInt;
        //this.ReportHistory.ReportInitialSchemaMD5 = txtReportInitialSchemaMD5.Text;
        this.ReportHistory.ReportAppStarted = txtReportAppStarted.ValueDate;
        this.ReportHistory.ReportAppStopped = txtReportAppStopped.ValueDate;

        this.ReportHistory.Save();

    }
    #endregion
    
}
