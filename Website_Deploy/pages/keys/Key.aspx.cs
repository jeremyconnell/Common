using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaAdmin;
using SchemaDeploy;
using Framework;

partial class pages_Keys_Key : CPage
{
    #region Querystring
    public string KeyName { get { return CWeb.RequestStr("keyName"); } }
    public bool IsEdit { get { return KeyName != String.Empty; } }
    public int AppId { get { return IsEdit ? Key.Group.GroupAppId : CWeb.RequestInt("appId"); } }
    public int GroupId { get { return IsEdit ? Key.KeyGroupId : CWeb.RequestInt("groupId"); } }
    #endregion

    #region Members
    private CKey _key;
    #endregion

    #region Data
    public CApp App {  get { return CApp.Cache.GetById(AppId); } }
    public CGroup Group { get { return CGroup.Cache.GetById(GroupId); } }
    public CKey Key 
    {
        get 
        {
            if (_key == null) 
            {
                if (IsEdit) 
                {
                    _key = CKey.Cache.GetById(KeyName);
                    if (_key == null)
                        CSitemap.RecordNotFound("Key", KeyName); 
                }
                else 
                {
                    _key = new CKey();
                    _key.KeyGroupId = GroupId;
                }
            }
            return _key;
        }
    }
    #endregion
    
    #region Navigation
    private void Refresh()      { Response.Redirect(CSitemap.KeyEdit(this.Key.KeyName)); }
    private void ReturnToList() { Response.Redirect(CSitemap.Keys()); }
    #endregion

    #region Event Handlers - Page
    protected override void PageInit()
    {
        if (AppId <= 0)
            Response.Redirect(CSitemap.Keys());

        ddKeyGroupId.DataTextField = "GroupName";
        ddKeyGroupId.DataValueField = "GroupId";
        ddKeyGroupId.DataSource = App.Groups;
        ddKeyGroupId.DataBind();
        ddKeyGroupId.BlankItem("-- Select Group --");

        ddKeyFormatId.DataTextField = "FormatName";
        ddKeyFormatId.DataValueField = "FormatId";
        ddKeyFormatId.DataSource = CFormat.Cache;
        ddKeyFormatId.DataBind();
        ddKeyFormatId.BlankItem("-- Select Format --");

        ddKeyDefaultBoolean.Add("NULL", -1);
        ddKeyDefaultBoolean.Add("False", 0);
        ddKeyDefaultBoolean.Add("True", 1);

        //Page title
        if (IsEdit) 
            this.Title = "Key Details";
        else 
            this.Title = "Create New Key";

        //Textbox logic
        //this.Form.DefaultFocus  = txtKeyName.Textbox.ClientID;
        this.Form.DefaultButton = btnSave.UniqueID;   //txtKeyName.OnReturnPress(btnSave);


        ddKeys.DataSource = CKey.Cache;
        if (GroupId > 0)
            ddKeys.DataSource = Group.Keys;
        ddKeys.DataBind();
        CDropdown.BlankItem(ddKeys, "-- New Key --");
        CDropdown.SetValue(ddKeys, KeyName);

        //Button Text
        btnDelete.Visible = IsEdit;
        if (IsEdit)
        {
            btnCancel.Text = "Back";
            AddButton(CSitemap.KeyAdd(Key.Group.GroupAppId, Key.KeyGroupId), "Create a new Key");
            AddMenuSide(CUtilities.NameAndCount("All Clients", Key.Values), CSitemap.KeySetting(KeyName));

            txtKeyDefaultInteger.Visible = (this.Key.KeyFormatId_ == EFormat.Integer);
            txtKeyDefaultString.Visible  = (this.Key.KeyFormatId_ == EFormat.String);
            ddKeyDefaultBoolean.Visible  = (this.Key.KeyFormatId_ == EFormat.Boolean);
        }
        else
        {
            btnSave.Text = "Create Key";

            txtKeyDefaultInteger.Visible = false;
            txtKeyDefaultString.Visible  = false;
            ddKeyDefaultBoolean.Visible  = false;
        }
    }
    protected override void PageLoad()
    {
        LoadKey();
    }
    #endregion

    #region Event Handlers - Form
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!this.IsValid)
            return;

        SaveKey();
    //CCache.ClearCache();  //e.g. if you have more than one application, need to request the clearcache page on the other app
        Refresh();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ReturnToList();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        this.Key.Delete();
    //CCache.ClearCache();  //e.g. if you have more than one application, need to request the clearcache page on the other app
        ReturnToList();
    }
    protected void ddKeys_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect(CSitemap.KeySetting(ddKeys.SelectedValue));
    }
    protected void btnDefault_Click(object sender, EventArgs e)
    {
        this.Key.GuessDefault();
        Response.Redirect(CSitemap.KeySetting(ddKeys.SelectedValue));
    }
    protected void btnApplyAll_Click(object sender, EventArgs e)
    {
        var k = Key;
        foreach (var i in CClient.Cache)
            foreach (var j in i.Instances)
            {
                var v = CValue.Cache.GetOrCreate(j.InstanceId, KeyName);
                v.ValueBoolean = k.KeyDefaultBoolean;
                v.ValueInteger = k.KeyDefaultInteger;
                v.ValueString = k.KeyDefaultString;
                v.Save();
            }
            Response.Redirect(CSitemap.KeySetting(ddKeys.SelectedValue));
    }
    
    #endregion

    #region Private - Load/Save
    protected void LoadKey()
    {
        ddKeyGroupId.ValueInt = this.Key.KeyGroupId;
        ddKeyFormatId.ValueInt = this.Key.KeyFormatId;
        txtKeyDefaultString.Text = this.Key.KeyDefaultString;
        txtKeyDefaultInteger.ValueInt = this.Key.KeyDefaultInteger.HasValue ? this.Key.KeyDefaultInteger.Value : int.MinValue;
        chkKeyIsEncrypted.Checked = this.Key.KeyIsEncrypted;

        if (!this.Key.KeyDefaultBoolean.HasValue)
            ddKeyDefaultBoolean.ValueInt = -1;
        else
            ddKeyDefaultBoolean.ValueInt = this.Key.KeyDefaultBoolean.Value ? 1 : 0;
    }
    protected void SaveKey()
    {
        this.Key.KeyGroupId = ddKeyGroupId.ValueInt;
        this.Key.KeyFormatId = ddKeyFormatId.ValueInt;

        if (this.Key.KeyFormatId_ == EFormat.String)
            this.Key.KeyDefaultString = txtKeyDefaultString.Text;
        if (this.Key.KeyFormatId_ == EFormat.Integer)
            this.Key.KeyDefaultInteger = txtKeyDefaultInteger.ValueInt;
        if (this.Key.KeyFormatId_ == EFormat.Boolean)
            if (ddKeyDefaultBoolean.ValueInt == -1)
                this.Key.KeyDefaultBoolean = null;
            else if (ddKeyDefaultBoolean.ValueInt == 0)
                this.Key.KeyDefaultBoolean = false;
            else
                this.Key.KeyDefaultBoolean = true;

        this.Key.KeyIsEncrypted = chkKeyIsEncrypted.Checked;

        this.Key.Save();
    //CCache.ClearCache();  //e.g. if you have more than one application, need to request the clearcache page on the other app

    }
    #endregion

    
}
