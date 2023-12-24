using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;

partial class pages_Backups_usercontrols_UCBackups : UserControl
{
    #region Events
    public event AddClickEventHandler    AddClick;
    public event ExportClickEventHandler ExportClick;
    public event ResortClickEventHandler ResortClick;
    
    public delegate void AddClickEventHandler();
    public delegate void ExportClickEventHandler();
    public delegate void ResortClickEventHandler(string sortBy, bool descending, int pageNumber);
    #endregion
    
    #region Interface
    public void Display(CBackupList backups, decimal size)  //Short list due to sql-based paging (by exposing Me.Info)
    {
        //Show/Hide Columns
        colNumber.Visible = backups.Count > 0;

        //Display
        foreach (CBackup i in backups)
            UCBackup(plh).Display(i, backups, this.PagingInfo);

        if (size > 0)
            btnSize.Text = CUtilities.FileSize(Convert.ToInt64(size));
    }
    public CPagingInfo PagingInfo { get { return ctrlPaging.Info; } }
    #endregion
    
    #region Event Handlers
    public void btnAdd_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        if (null != AddClick) AddClick();
    }
    public void btnExport_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        if (null != ExportClick) ExportClick();
    }
    public void btnResort_Click(object sender, EventArgs e)
    {
        if (null == ResortClick) 
            return; //Check for event listeners

        string sortBy = ((LinkButton)sender).CommandArgument;
        bool descending = ctrlPaging.IsDescending;
        string currentSort = ctrlPaging.SortColumn;
        if (!string.IsNullOrEmpty(currentSort))
            if (currentSort == sortBy)
                descending = !descending;

        //Bubble up as event, search page will add filter info then redirect
        ResortClick(sortBy, descending, ctrlPaging.Info.PageIndex+1);
    }
    #endregion
    
    #region User Controls
    private static pages_backups_usercontrols_UCBackup UCBackup(Control target)
    {
        Control ctrl = target.Page.LoadControl(CSitemap.UCBackup());
        target.Controls.Add(ctrl);
        return (pages_backups_usercontrols_UCBackup)ctrl;
    }
    #endregion
    
    #region Paging
    public CPagingInfo Info 
    { 
        get { return ctrlPaging.Info; } 
    }
    public string QueryString 
    {
        get { return ctrlPaging.QueryString; }
        set { ctrlPaging.QueryString = value; }
    }
    public int PageSize 
    {
        get { return ctrlPaging.PageSize; }
        set { ctrlPaging.PageSize = value; }
    }
    #endregion     
}
