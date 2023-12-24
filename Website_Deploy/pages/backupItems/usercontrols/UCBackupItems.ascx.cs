using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;

partial class pages_BackupItems_usercontrols_UCBackupItems : UserControl
{
    #region Events
    public event ExportClickEventHandler ExportClick;
    public event ResortClickEventHandler ResortClick;
    
    public delegate void ExportClickEventHandler();
    public delegate void ResortClickEventHandler(string sortBy, bool descending, int pageNumber);
    #endregion

    #region Interface
    public void Display(CBackupItemList backupItems)
    {
        //Short list due to sql-based paging (by exposing Me.Info)
        colNumber.Visible = backupItems.Count > 0;
        foreach (CBackupItem i in backupItems)
            UCBackupItem(plh).Display(i, backupItems, this.PagingInfo);
    }
    public CPagingInfo PagingInfo   { get { return ctrlPaging.Info; }    }
    #endregion
    
    #region Event Handlers
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
    private static pages_backupitems_usercontrols_UCBackupItem UCBackupItem(Control target)
    {
        Control ctrl = target.Page.LoadControl(CSitemap.UCBackupItem());
        target.Controls.Add(ctrl);
        return (pages_backupitems_usercontrols_UCBackupItem)ctrl;
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
