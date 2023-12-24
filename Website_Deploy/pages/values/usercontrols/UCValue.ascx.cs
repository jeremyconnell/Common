using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;

public partial class pages_values_usercontrols_UCValue : UserControl 
{     
    #region Members
    private CValue _value; 
    private IList _sortedList; 
    #endregion 
    
    #region Interface
    public void Display(CValue value, IList sortedList) 
    { 
        if (Parent.Controls.Count % 2 == 0) 
            row.Attributes.Add("class", "alt_row"); 
        
        _value = value; 
        _sortedList = sortedList;
         
        CValue v = _value; 
        litNumber.Text = Convert.ToString(sortedList.IndexOf(v) + 1);
        var c = v.Instance;
        litValueInstanceId.Text = null == c? string.Empty : c.IdAndName;
        lnkValueKeyName.Text = (string)(v.GroupAndKey.Length == 0 ? "..." : v.GroupAndKey); 

        lnkValueKeyName.NavigateUrl = CSitemap.ValueEdit(v.ValueId);
        litValueInstanceId.NavigateUrl = CSitemap.ValueEdit(v.ValueId);

        litValueAsString.ToolTip = v.ValueAsString;
        litValueAsString.Text = CUtilities.Truncate(v.ValueAsString, 50);
    } 
    #endregion 
    
    #region Event Handlers 
    protected void btnDelete_Click(object sender, ImageClickEventArgs e) 
    {
        _value.Delete(); 
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
