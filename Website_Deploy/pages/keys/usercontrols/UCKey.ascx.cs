using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;

public partial class pages_keys_usercontrols_UCKey : UserControl 
{     
    #region Members
    private CKey _key; 
    private IList _sortedList; 
    #endregion 
    
    #region Interface
    public void Display(CKey key, IList sortedList) 
    { 
        if (Parent.Controls.Count % 2 == 0) 
            row.Attributes.Add("class", "alt_row"); 
        
        _key = key; 
        _sortedList = sortedList;
         
        CKey c = _key; 
        litNumber.Text = Convert.ToString(sortedList.IndexOf(c) + 1);
        lnkKeyName.Text = c.KeyName;
        lnkKeyName.NavigateUrl = CSitemap.KeyEdit(c.KeyName);

        litKeyDefault.Text = CUtilities.Truncate(c.DefaultValue);
        litKeyDefault.ToolTip = c.DefaultValue;
        litKeyFormatId.Text = c.Format.FormatShort;
        litKeyGroupId.Text = c.GroupName;
        litKeyGroupId.NavigateUrl = CSitemap.Keys((int)EApp.ControlTrack, null, key.KeyGroupId, int.MinValue);

        var d = c.DistinctValues;
        if (d.Count > 0)
        {
            lnkKeyDistinct.Text = CUtilities.CountSummary(d, "value");
            lnkKeyDistinct.ToolTip = CUtilities.ListToString(d, "\r\n");
        }
        else
            lnkKeyDistinct.Text = "no values";
        lnkKeyDistinct.NavigateUrl = CSitemap.KeySetting(key.KeyName);


        lnkClients.Text = CUtilities.CountSummary(c.Values, "client", string.Empty);
        lnkClients.NavigateUrl = CSitemap.KeySetting(key.KeyName);

    } 
    #endregion 
    
    #region Event Handlers 
    protected void btnDelete_Click(object sender, ImageClickEventArgs e) 
    {
        _key.Delete(); 
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
