using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;

public partial class pages_Keys_default : CPage
{
    #region Querystring
    public string Search { get { return CWeb.RequestStr("search"); } }
    public int AppId { get { return CWeb.RequestInt("appId"); } }
    public int GroupId { get { return CWeb.RequestInt("groupId"); } }
    public int FormatId { get { return CWeb.RequestInt("formatId"); } }
    #endregion

    #region Data
    public CApp App { get { return CApp.Cache.GetById(AppId); } }
    public CKeyList Keys { get { return CKey.Cache.Search(txtSearch.Text, GroupId, FormatId); } } 
    #endregion 
    
    #region Event Handlers - Page
    protected override void PageInit()
    {
        if (AppId <= 0)
            Response.Redirect(CSitemap.Keys(EApp.ControlTrack));

        //Populate Dropdowns
        ddApp.DataSource = CApp.Cache;
        ddApp.DataBind();
        CDropdown.SetValue(ddApp, AppId);

        ddGroup.DataSource = App.Groups;
        ddGroup.DataBind();
        CDropdown.BlankItem(ddGroup, "<Blank>", "0");
        CDropdown.BlankItem(ddGroup, "-- Any Group --");
        CDropdown.SetValue(ddGroup, GroupId);

        ddFormat.DataSource = CFormat.Cache;
        ddFormat.DataBind();
        CDropdown.BlankItem(ddFormat, "-- Any Format --");
        CDropdown.SetValue(ddFormat, FormatId);


        //Search state (from querystring)
        txtSearch.Text = this.Search;
        
        //Display Results
        ctrlKeys.Display(this.Keys);

        //Client-side
        this.Form.DefaultFocus  = txtSearch.ClientID;
        this.Form.DefaultButton = btnSearch.UniqueID;   //CTextbox.OnReturnPress(txtSearch, btnSearch);

        AddMenuSide(CUtilities.NameAndCount("Keys", CKey.Cache), CSitemap.Keys(AppId), int.MinValue == AppId);

        if (GroupId > 0)
        {
            AddMenuSide(App.NameAndGroupCount, CSitemap.Keys(AppId), GroupId == int.MinValue);
            foreach (var i in App.Groups)
                AddMenuSide( i.NameAndCount, CSitemap.Keys(AppId, i.GroupId), GroupId == i.GroupId);
        }
        else
        {
            foreach (var i in CApp.Cache)
                AddMenuSide(CUtilities.NameAndCount(i.AppName,i.Groups, "group"), CSitemap.Keys(i.AppId), AppId == i.AppId);
            foreach (var i in CFormat.Cache)
                AddMenuSide(i.NameAndCount, CSitemap.Keys(AppId, null, GroupId, FormatId), FormatId == i.FormatId);
        }

        AddLinkSide("New Group", CSitemap.GroupAdd(AppId));
        AddLinkSide("New Key", CSitemap.KeyAdd(AppId, GroupId));
    }
    #endregion

    #region Event Handlers - Form
    protected void btnSearch_Click(object sender, EventArgs e)
    {   
        Response.Redirect(CSitemap.Keys(CDropdown.GetInt(ddApp), txtSearch.Text, CDropdown.GetInt(ddGroup), CDropdown.GetInt(ddFormat)));
    }
    protected void btnSetDefaults_Click(object sender, EventArgs e)
    {
        foreach (var k in CKey.Cache)
            k.GuessDefault();
        Response.Redirect(Request.RawUrl);
    }
    
    #endregion

    #region Event Handlers - UserControls
    protected void ctrl_AddClick()
    {
        Response.Redirect(CSitemap.KeyAdd(AppId, GroupId));
    }
    protected void ctrl_ResortClick(string sortBy, bool descending, int pageNumber)
    {
        Response.Redirect(CSitemap.Keys(AppId, txtSearch.Text, GroupId, FormatId, new CPagingInfo(0, pageNumber - 1, sortBy, descending)));
    }
    #endregion
}
