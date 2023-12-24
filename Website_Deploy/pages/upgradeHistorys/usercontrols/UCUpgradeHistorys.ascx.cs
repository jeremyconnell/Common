using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;

partial class pages_UpgradeHistorys_usercontrols_UCUpgradeHistorys : UserControl
{
    #region Events
    public event ExportClickEventHandler ExportClick;
    public event ResortClickEventHandler ResortClick;
    
    public delegate void ExportClickEventHandler();
    public delegate void ResortClickEventHandler(string sortBy, bool descending, int pageNumber);
    #endregion

    #region Interface
    public void Display(CUpgradeHistoryList upgradeHistorys)
    {
        //Short list due to sql-based paging (by exposing Me.Info)
        colNumber.Visible = upgradeHistorys.Count > 0;
        foreach (CUpgradeHistory i in upgradeHistorys)
            UCUpgradeHistory(plh).Display(i, upgradeHistorys, this.PagingInfo);
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
    private static pages_upgradehistorys_usercontrols_UCUpgradeHistory UCUpgradeHistory(Control target)
    {
        Control ctrl = target.Page.LoadControl(CSitemap.UCUpgradeHistory());
        target.Controls.Add(ctrl);
        return (pages_upgradehistorys_usercontrols_UCUpgradeHistory)ctrl;
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
