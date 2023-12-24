using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;

public partial class pages_groups_usercontrols_UCGroup : UserControl 
{     
    #region Members
    private CGroup _group; 
    private IList _sortedList;
    #endregion

    #region Interface
    public void Display()
    {
        if (Parent.Controls.Count % 2 == 0)
            row.Attributes.Add("class", "alt_row");
        btnDelete.Visible = false;
        litNumber.Text = "#";
        lnkGroupName.Text = "&lt;blank&gt;";
        lnkGroupName.NavigateUrl = "javascript:";
        lnkGroupName.Enabled = false;
        lnkKeys.Text = CUtilities.CountSummary(CKey.Cache.GetByGroupId(int.MinValue).Count, "key", "none", "keys");
        lnkKeys.NavigateUrl = CSitemap.Keys(EApp.ControlTrack);

    }
    public void Display(CGroup group, IList sortedList) 
    { 
        if (Parent.Controls.Count % 2 == 0) 
            row.Attributes.Add("class", "alt_row"); 
        
        _group = group; 
        _sortedList = sortedList;
         
        CGroup c = _group; 
        litNumber.Text = Convert.ToString(sortedList.IndexOf(c) + 1);
        lnkGroupName.Text = (string)(c.GroupName.Length == 0 ? "..." : c.GroupName); 
        lnkGroupName.NavigateUrl = CSitemap.GroupEdit(c.GroupId);

        lnkKeys.Text = CUtilities.CountSummary(c.Keys.Count, "key", "none", "keys");
        lnkKeys.NavigateUrl = CSitemap.Keys((int)EApp.ControlTrack,null,  c.GroupId, int.MinValue);
    } 
    #endregion 
    
    #region Event Handlers 
    protected void btnDelete_Click(object sender, ImageClickEventArgs e) 
    {
        _group.Delete(); 
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
