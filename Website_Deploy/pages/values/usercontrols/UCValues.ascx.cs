using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;

partial class pages_Values_usercontrols_UCValues : UserControl
{
    #region Events
    public event AddClickEventHandler    AddClick;
    public event ResortClickEventHandler ResortClick;
    
    public delegate void AddClickEventHandler();
    public delegate void ExportClickEventHandler();
    public delegate void ResortClickEventHandler(string sortBy, bool descending, int pageNumber);
    #endregion
    
    #region Members
    private CValueList _list;
    #endregion
    
    #region Interface
    public void Display(CValueList values)  //Complete list, not yet paged
    {
        _list = values;
        
        //Show/Hide Columns
        colNumber.Visible = values.Count > 0;

        //Display
        plh.Controls.Clear();
        IList sorted = null; //Fixes the numbering to reflect a user-sorted list (querystring sortBy)
        IList page = ctrlPaging.Display(values, ref sorted); //In-Memory paging, also outputs the sorted list
        foreach (CValue i in page)
            UCValue(plh).Display(i, sorted);
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
    private static pages_values_usercontrols_UCValue UCValue(Control target)
    {
        Control ctrl = target.Page.LoadControl(CSitemap.UCValue());
        target.Controls.Add(ctrl);
        return (pages_values_usercontrols_UCValue)ctrl;
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