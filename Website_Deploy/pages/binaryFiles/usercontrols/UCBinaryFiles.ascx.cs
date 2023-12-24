using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;
using System.Collections;

using System.IO;
using System.IO.Compression;

partial class pages_BinaryFiles_usercontrols_UCBinaryFiles : UserControl
{
    #region Events
    public event ResortClickEventHandler ResortClick;
    
    public delegate void ResortClickEventHandler(string sortBy, bool descending, int pageNumber);
    #endregion

    #region Members
    private CBinaryFileList  _binaryFiles;
    private CVersionFileList _versionFiles;
    #endregion

    #region Interface
    public string Title
    {
        get { return h3.InnerText; }
        set { h3.InnerText = value; h3.Visible = !string.IsNullOrEmpty(value); }
    }
    public void Display(CBinaryFileList binaryFiles) 
    {
        _binaryFiles = binaryFiles;

        //Show/Hide Columns
        colNumber.Visible = binaryFiles.Count > 0;

        //Display
        plh.Controls.Clear();
        IList sorted = null; //Fixes the numbering to reflect a user-sorted list (querystring sortBy)
        IList page = ctrlPaging.Display(binaryFiles, ref sorted); //In-Memory paging, also outputs the sorted list
        foreach (CBinaryFile i in page)
            UCBinaryFile(plh).Display(i, sorted);
    }
    public void Display(CVersionFileList versionFiles)
    {
        _versionFiles = versionFiles;

        //Show/Hide Columns
        colNumber.Visible = versionFiles.Count > 0;


		btnSortByPath.CommandArgument = "VFPath";

		//Display
		plh.Controls.Clear();
        IList sorted = null; //Fixes the numbering to reflect a user-sorted list (querystring sortBy)
        IList page = ctrlPaging.Display(versionFiles, ref sorted); //In-Memory paging, also outputs the sorted list
        foreach (CVersionFile i in page)
            UCBinaryFile(plh).Display(i, sorted);

        colUsg.Visible = false;
        colDel.Visible = false;
    }
    #endregion

    #region Event Handlers
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
    private static pages_binaryfiles_usercontrols_UCBinaryFile UCBinaryFile(Control target)
    {
        Control ctrl = target.Page.LoadControl(CSitemap.UCBinaryFile());
        target.Controls.Add(ctrl);
        return (pages_binaryfiles_usercontrols_UCBinaryFile)ctrl;
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
