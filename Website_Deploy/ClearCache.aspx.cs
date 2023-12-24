using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Framework;
using SchemaMembership;
using System.Text;

public partial class ClearCache : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        //Clear Cache
        foreach (DictionaryEntry i in Cache)
            Cache.Remove(i.Key.ToString());

		Session.Clear();

        //User feedback
        litDate.Text = DateTime.Now.ToString();

        //No-caching for this page
        Response.Expires = 0;
        Response.Cache.SetNoStore();
        Response.AppendHeader("Pragma", "no-cache");
        Response.AddHeader("cache-control", "private");

        //Can use querystring to implement entity-level cache-clear (for a multi-application environment, admin will make calls to front-end's clearcache)
        switch (CWeb.RequestStr("entity"))
        {
            //Example: 
            case CRole.TABLE_NAME: CRole.Cache = null; break;
        }
    }
}
