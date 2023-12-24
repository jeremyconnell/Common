using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaAdmin;
using Framework;
using SchemaDeploy;

public partial class pages_clients_usercontrols_UCClient : UserControl 
{     
    #region Members
    private CClient _client; 
    private IList _sortedList; 
    #endregion 
    
    #region Interface
    public void Display(CClient client, IList sortedList) 
    { 
        if (Parent.Controls.Count % 2 == 0) 
            row.Attributes.Add("class", "alt_row"); 
        
        _client = client; 
        _sortedList = sortedList;
         
        CClient c = _client; 


        litNumber.Text = Convert.ToString(sortedList.IndexOf(c) + 1);
        litClientCode.Text = c.ClientUniqueCode;

        lnkClientName.Text = (string)(c.ClientName.Length == 0 ? "..." : c.ClientName); 
        lnkClientName.NavigateUrl = CSitemap.ClientEdit(c.ClientId);

        lnkInstances.Text = CUtilities.CountSummary(c.Instances, "deployment", "none");
        lnkInstances.NavigateUrl = CSitemap.InstancesByClient(c.ClientId);
        if (c.InstanceCount == 0)
            lnkInstances.NavigateUrl = CSitemap.InstanceAdd(EApp.ControlTrack, c.ClientId);


        foreach (var i in c.Instances)
            UCClientInstance(plhInstances).Display(i);

        if (0 == c.Instances.Count)
            UCClientInstance(plhInstances).Display(c);


        if (null != c.Status)
            litClientStatusId.Text = c.Status.StatusName;

        litClientTrialStarted.Text = CUtilities.ShortDate(c.ClientTrialStarted);
        litClientTrialEnded.Text = CUtilities.ShortDate(c.ClientTrialEnded);
        litClientProductionStarted.Text = CUtilities.ShortDate(c.ClientProductionStarted);

    }
    #endregion

    #region User Controls
    private static pages_clients_usercontrols_UCClientInstance UCClientInstance(Control target)
    {
        Control ctrl = target.Page.LoadControl(CSitemap.UCClientInstance());
        target.Controls.Add(ctrl);
        return (pages_clients_usercontrols_UCClientInstance)ctrl;
    }
    #endregion

    #region Event Handlers 
    protected void btnDelete_Click(object sender, ImageClickEventArgs e) 
    {
        _client.Delete(); 
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
