using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;

public partial class pages_pushedupgrades_usercontrols_UCPushedUpgrade : UserControl 
{     
    #region Members
    private CPushedUpgrade _pushedUpgrade; 
    private IList _sortedList; 
    #endregion 
    
    #region Interface
    public void Display(CPushedUpgrade pushedUpgrade, IList sortedList) 
    { 
        if (Parent.Controls.Count % 2 == 0) 
            row.Attributes.Add("class", "alt_row"); 
        
        _pushedUpgrade = pushedUpgrade; 
        _sortedList = sortedList;
         
        CPushedUpgrade c = _pushedUpgrade; 
        litNumber.Text = Convert.ToString(sortedList.IndexOf(c) + 1);
        litPushInstanceId.Text = c.Instance.NameAndSuffix;
        lnkPushUserName.Text = (string)(c.PushUserName.Length == 0 ? "..." : c.PushUserName); 
        lnkPushUserName.NavigateUrl = CSitemap.PushedUpgradeEdit(c.PushId); 
        litPushOldVersionId.Text = c.OldVersion?.NumberAndName ?? string.Concat("#", c.PushOldVersionId);
		litPushOldSchemaMD5.Text = c.PushOldSchemaB64;
        litPushNewVersionId.Text = c.NewVersion?.NumberAndName ?? string.Concat("#", c.PushNewVersionId);
        litPushNewSchemaMD5.Text = c.PushNewSchemaB64;
        litPushStarted.Text = CUtilities.Timespan(c.PushStarted);
		if (c.PushCompleted != DateTime.MinValue)
			litPushCompleted.Text = CUtilities.Timespan(c.PushCompleted.Subtract(c.PushStarted));

    } 
    #endregion 
    
    #region Event Handlers 
    protected void btnDelete_Click(object sender, ImageClickEventArgs e) 
    {
        _pushedUpgrade.Delete(); 
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
