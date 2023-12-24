using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;

public partial class pages_Values_default : CPage
{
    #region Querystring
    public string Search { get { return CWeb.RequestStr("search"); } }
    public int InstanceId { get { return CWeb.RequestInt("instanceId"); } }
    public int AppId { get { return null != Instance ? Instance.InstanceAppId : CWeb.RequestInt("appId"); } }
    public string KeyName { get { return CWeb.RequestStr("keyName"); } }
    #endregion

    #region Data
    public CApp App { get { return CApp.Cache.GetById(AppId); } }
    public CInstance Instance {  get { return SchemaDeploy.CInstance.Cache.GetById(InstanceId); } }
    public CValueList Values { get { return CValue.Cache.Search(txtSearch.Text,  InstanceId, KeyName); } } 
    #endregion 
    
    #region Event Handlers - Page
    protected override void PageInit()
    {
        //Populate Dropdowns

        ddInstance.DataSource = AppId > 0 ? App.Instances : CInstance.Cache;
        ddInstance.DataBind();
        CDropdown.BlankItem(ddInstance, "-- Any Instance --");
        CDropdown.SetValue(ddInstance, InstanceId);

        ddKey.DataSource = CKey.Cache;
        ddKey.DataBind();
        CDropdown.BlankItem(ddKey, "-- Any Key --");
        CDropdown.SetValue(ddKey, KeyName);


        //Search state (from querystring)
        txtSearch.Text = this.Search;
        
        //Display Results
        ctrlValues.Display(this.Values);

        //Client-side
        this.Form.DefaultFocus  = txtSearch.ClientID;
        this.Form.DefaultButton = btnSearch.UniqueID;   //CTextbox.OnReturnPress(txtSearch, btnSearch);
    }
    #endregion

    #region Event Handlers - Form
    protected void btnSearch_Click(object sender, EventArgs e)
    {   
        Response.Redirect(CSitemap.Values(txtSearch.Text, CDropdown.GetInt(ddInstance), ddKey.SelectedValue));
    }
    protected void btnCreate_Click(object sender, EventArgs e)
    {
    //    CValue obj = new CValue();
    //    obj.ValueName = txtSearch.Text;
    //    obj.Save();
    //    Response.Redirect(Request.RawUrl);
    }
    #endregion

    #region Event Handlers - UserControls
    protected void ctrl_AddClick()
    {
        Response.Redirect(CSitemap.ValueAdd(InstanceId, KeyName));
    }
    protected void ctrl_ResortClick(string sortBy, bool descending, int pageNumber)
    {
        Response.Redirect(CSitemap.Values(txtSearch.Text, InstanceId, KeyName, new CPagingInfo(0, pageNumber - 1, sortBy, descending)));
    }
    #endregion
}
