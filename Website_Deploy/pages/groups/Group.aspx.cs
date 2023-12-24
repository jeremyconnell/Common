using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;

partial class pages_Groups_Group : CPage
{
    #region Querystring
    public int GroupId { get { return CWeb.RequestInt("groupId"); } }
    public int AppId { get { return CWeb.RequestInt("appId"); } }
    public bool IsEdit { get { return GroupId != int.MinValue; } }
    #endregion

    #region Members
    private CGroup _group;
    #endregion
    
    #region Data
    public CGroup Group 
    {
        get 
        {
            if (_group == null) 
            {
                if (IsEdit) 
                {
                    _group = CGroup.Cache.GetById(GroupId);
                    if (_group == null)
                        CSitemap.RecordNotFound("Group", GroupId); 
                }
                else 
                {
                    _group = new CGroup();
                }
            }
            return _group;
        }
    }
    #endregion
    
    #region Navigation
    private void Refresh()      { Response.Redirect(CSitemap.GroupEdit(this.Group.GroupId)); }
    private void ReturnToList() { Response.Redirect(CSitemap.Groups()); }
    #endregion

    #region Event Handlers - Page
    protected override void PageInit()
    {

        //Page title
        if (IsEdit) 
            this.Title = "Group Details";
        else 
            this.Title = "Create New Group";

        //Textbox logic
        //this.Form.DefaultFocus  = txtGroupName.Textbox.ClientID;
        this.Form.DefaultButton = btnSave.UniqueID;   //txtGroupName.OnReturnPress(btnSave);

        //Button Text
        btnDelete.Visible = IsEdit;
        if (IsEdit)
        {
            btnCancel.Text = "Back";
            AddButton(CSitemap.GroupAdd(AppId), "Create a new Group");
        }
        else 
            btnSave.Text = "Create Group";
    }
    protected override void PageLoad()
    {
        LoadGroup();
    }
    #endregion

    #region Event Handlers - Form
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!this.IsValid)
            return;

        SaveGroup();
    //CCache.ClearCache();  //e.g. if you have more than one application, need to request the clearcache page on the other app
        Refresh();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ReturnToList();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        this.Group.Delete();
    //CCache.ClearCache();  //e.g. if you have more than one application, need to request the clearcache page on the other app
        ReturnToList();
    }
    #endregion
    
    #region Private - Load/Save
    protected void LoadGroup()
    {
        txtGroupName.Text = this.Group.GroupName;
    }
    protected void SaveGroup()
    {
        this.Group.GroupName = txtGroupName.Text;

        this.Group.Save();
    //CCache.ClearCache();  //e.g. if you have more than one application, need to request the clearcache page on the other app

    }
    #endregion
    
    #region Private - Load/Save Bulk
//(Sample Only, Collection property not implemented - use checkboxes/ajax/session etc)
/*  protected void LoadGroups()
    {
        txtGroupName.Text = this.Groups.HaveSameValue("GroupName") ? this.Groups[0].GroupName : BULKEDIT_NOCHANGE;
    }
    protected void SaveGroups()
    {
            if (BULKEDIT_NOCHANGE != txtGroupName.Text) { this.Groups.SetSameValue("GroupName", txtGroupName.Text); }

        this.Groups.SaveAll();
    //CCache.ClearCache();  //e.g. if you have more than one application, need to request the clearcache page on the other app

    } */
    #endregion
}
