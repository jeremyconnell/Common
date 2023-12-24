using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaAdmin;

public partial class pages_clients_usercontrols_UCClientInstance : System.Web.UI.UserControl
{
    public void Display(SchemaDeploy.CInstance i)
    {
        lnk.Text = i.IdAndName;
        lnk.NavigateUrl = CSitemap.Instance(i.InstanceId);
    }
    public void Display(SchemaAdmin.CClient c)
    {
        lnk.Text = "<new deployment>";
        lnk.NavigateUrl = CSitemap.InstanceAdd(1, c.ClientId);
    }
}