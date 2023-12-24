using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaAdmin;
using SchemaDeploy;
using Framework;

partial class pages_Values_Value : CPage
{
    #region Querystring
    public int ValueId { get { return CWeb.RequestInt("valueId"); } }
    public bool IsEdit { get { return ValueId != int.MinValue; } }
    public string KeyName { get { return CWeb.RequestStr("keyName"); } }
    public int InstanceId { get { return CWeb.RequestInt("instanceId"); } }
    #endregion

    #region Members
    private CValue _value;
    #endregion
    
    #region Data
    public CValue Value 
    {
        get 
        {
            if (_value == null) 
            {
                _value = CValue.Cache.GetById(ValueId);
				if (_value == null)
				{
					_value = CValue.Cache.GetOrCreate(InstanceId, KeyName);
					Response.Redirect(CSitemap.ValueEdit(_value.ValueId), true);
				}
            }
            return _value;
        }
    }
    #endregion
    
    #region Navigation
    private void Refresh()      { Response.Redirect(CSitemap.ValueEdit(Value.ValueId)); }
    private void ReturnToList() { Response.Redirect(CSitemap.Values()); }
    #endregion

    #region Event Handlers - Page
    protected override void PageInit()
    {
        ddValueInstanceId.DataTextField = "IdAndName";
        ddValueInstanceId.DataValueField = "InstanceId";
        ddValueInstanceId.DataSource = CInstance.Cache;
        ddValueInstanceId.DataBind();
        ddValueInstanceId.BlankItem("-- Select Deploy --");

        ddKeyName.DataTextField = "KeyName";
        ddKeyName.DataValueField = "KeyName";
        ddKeyName.DataSource = CKey.Cache;
        ddKeyName.DataBind();
        ddKeyName.BlankItem("-- Select Key --");

        ddValueBoolean.Add("NULL", -1);
        ddValueBoolean.Add("False", 0);
        ddValueBoolean.Add("True", 1);


        //Page title
        if (IsEdit)
        {
            this.Title = "Value Details";
            ddKeyName.Mode = EControlMode.Disabled;
            ddValueInstanceId.Mode = EControlMode.Disabled;
            txtValueInteger.Visible = (Value.Key.KeyFormatId_ == EFormat.Integer);
            txtValueString.Visible = (Value.Key.KeyFormatId_ == EFormat.String);
            ddValueBoolean.Visible = (Value.Key.KeyFormatId_ == EFormat.Boolean);
        }
        else
        {
            this.Title = "Create New Value";
            txtValueInteger.Visible = false;
            txtValueString.Visible = false;
            ddValueBoolean.Visible = false;
        }

        //Textbox logic
        //this.Form.DefaultFocus  = txtValueName.Textbox.ClientID;
        this.Form.DefaultButton = btnSave.UniqueID;   //txtValueName.OnReturnPress(btnSave);

        //Button Text
        btnDelete.Visible = IsEdit;
        if (IsEdit)
        {
            btnCancel.Text = "Back";
            //AddButton(CSitemap.ValueAdd(), "Create a new Value");

            AddMenuSide(CUtilities.Truncate(Value.Instance.NameAndValueCount, 20), CSitemap.InstanceSettings(Value.ValueInstanceId, Value.ValueInstanceId), false, Value.Instance.NameAndSuffix + " (all keys)");
            AddMenuSide(CUtilities.Truncate(Value.Key.NameAndCount, 20),           CSitemap.KeySetting(Value.ValueKeyName),      false, Value.ValueKeyName + " (all clients)");
        }
        else 
            btnSave.Text = "Create Value";
    }
    protected override void PageLoad()
    {
        LoadValue();
    }
    #endregion

    #region Event Handlers - Form
    protected void btnSave_Click(object sender, EventArgs e)
    {

        SaveValue();
    //CCache.ClearCache();  //e.g. if you have more than one application, need to request the clearcache page on the other app
        Refresh();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ReturnToList();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        this.Value.Delete();
    //CCache.ClearCache();  //e.g. if you have more than one application, need to request the clearcache page on the other app
        ReturnToList();
    }
    #endregion
    
    #region Private - Load/Save
    protected void LoadValue()
    {
        var v = this.Value;
        ddKeyName.Value = v.ValueKeyName;
        ddValueInstanceId.ValueInt = v.ValueInstanceId;
        txtValueString.Text = v.ValueString;
        if (!v.ValueBoolean.HasValue)
            ddValueBoolean.ValueInt = -1;
        else
            ddValueBoolean.ValueInt = v.ValueBoolean.Value ? 1 : 0;
        txtValueInteger.ValueInt = v.ValueInteger.HasValue ? v.ValueInteger.Value : int.MinValue;
    }
    protected void SaveValue()
    {
        var v = this.Value;
        if (!IsEdit)
        {
            v.ValueKeyName = ddKeyName.Value;
            v.ValueInstanceId = ddValueInstanceId.ValueInt;
        }
        else
        {
            if (txtValueString.Visible)  v.ValueString  = txtValueString.Text;
            if (txtValueInteger.Visible) v.ValueInteger = txtValueInteger.ValueInt;
            if (ddValueBoolean.Visible)
                if (ddValueBoolean.ValueInt == -1)
                    v.ValueBoolean = null;
                else if (ddValueBoolean.ValueInt == 0)
                    v.ValueBoolean = false;
                else
                    v.ValueBoolean = true;
        }

        v.Save();
    }
    #endregion
}
