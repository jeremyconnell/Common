using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;

partial class pages_Releases_usercontrols_UCReleases : UserControl
{
    #region Events
    public event AddClickEventHandler    AddClick;
    public event ResortClickEventHandler ResortClick;
    
    public delegate void AddClickEventHandler();
    public delegate void ExportClickEventHandler();
    public delegate void ResortClickEventHandler(string sortBy, bool descending, int pageNumber);
    #endregion
    
    #region Members
    private CReleaseList _list;
    #endregion
    
    #region Interface
    public void Display(CReleaseList releases)  //Complete list, not yet paged
    {
        _list = releases;
        
        //Show/Hide Columns
        colNumber.Visible = releases.Count > 0;

        //Display
        plh.Controls.Clear();
        IList sorted = null; //Fixes the numbering to reflect a user-sorted list (querystring sortBy)
        IList page = ctrlPaging.Display(releases, ref sorted); //In-Memory paging, also outputs the sorted list
        foreach (CRelease i in page)
            UCRelease(plh).Display(i, sorted);
    }
    #endregion
    
    #region Event Handlers
    public void btnAdd_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        if (null != AddClick) AddClick();
    }
    public void btnExport_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        _list.ExportToCsv(Response);
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
    private static pages_targetversionhistorys_usercontrols_UCRelease UCRelease(Control target)
    {
        Control ctrl = target.Page.LoadControl(CSitemap.UCRelease());
        target.Controls.Add(ctrl);
        return (pages_targetversionhistorys_usercontrols_UCRelease)ctrl;
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