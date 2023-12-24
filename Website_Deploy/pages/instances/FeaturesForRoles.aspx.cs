using Framework;
using SchemaControlTrack;
using SchemaDeploy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pages_instances_FeaturesForRoles :  CPageDeploy
{
    #region Querystring
    public int InstanceId { get { return CWeb.RequestInt("instanceId"); } }
    public int AppId { get { return null != Instance ? Instance.InstanceAppId : CWeb.RequestInt("appId"); } }
    public int FeatureId { get { return CWeb.RequestInt("featureId"); } }
    #endregion

    #region Data
    public CInstance Instance { get { return CInstance.Cache.GetById(InstanceId); } }
    public CApp App { get { return CApp.Cache.GetById(AppId); } }
    #endregion

    #region Page Events
    protected override void PageInit()
    {
        CDropdown.SetValue(rbl, CSession.FeaturesAppId);

        ddApp.DataSource = CApp.Cache.WithInstances;
        ddApp.DataBind();
        CDropdown.SetValue(ddApp, AppId);

        ddIns.DataSource = App.Instances;
        ddIns.DataBind();
        CDropdown.SetValue(ddIns, InstanceId);


        MenuInstanceFeatures(AppId, InstanceId);

        if (FeatureId > 0)
            RenameDeleteFeature();

        Display();
    }
    #endregion

    #region Display
    private TextBox _txt;
    private Dictionary<Tuple<int, int>, CheckBox> _dict = new Dictionary<Tuple<int, int>, CheckBox>();
    private CApplicationRoleFeatureList _matrix;
    private CFeature _feature;
    public void Display()
    {
        var db = Instance.DatabaseDirectOrWeb;

        var appId = CSession.FeaturesAppId;

        try
        {
            var features = new CFeature(db).SelectByApplicationId(appId);
            var roles = new CApplicationRole(db).SelectByApplicationId(appId);
            var roleFeat = new CApplicationRoleFeature(db).SelectAll();
            _matrix = roleFeat;

            var rowH = RowH(tbl);
            var th = CellH(rowH, "Features: ");
            foreach (var i in roles)
            {
                var s = i.Name.Replace("Control ", "C").Replace("Risk ", "R").Replace("Manager", "M").Replace("Owner", "O").Replace("User", "U");
                CellH(rowH, s, i.Name);
            }
            foreach (var i in features)
            {
                var row = Row(tbl);
                th = CellLinkH(row, i.Name, CSitemap.InstanceFeatures(InstanceId, i.FeatureID), false);
                foreach (var j in roles)
                {
                    var td = Cell(row);

                    var chk = new CheckBox();
                    chk.ID = string.Concat("chk_", i.FeatureID, "_", j.ApplicationRoleID);

                    chk.Checked = null != roleFeat.GetById(j.ApplicationRoleID, i.FeatureID);

                    td.Controls.Add(chk);

                    _dict.Add(new Tuple<int, int>(j.ApplicationRoleID, i.FeatureID), chk);
                }
            }

            var footer = RowH(tbl);

            var txt = new TextBox();
            txt.ID = "txtAdd";
            txt.Font.Size = new FontUnit(FontSize.Smaller);
            th = CellH(footer);
            th.Controls.Add(txt);

            var btn = new Button();
            btn.Text = "Add";
            btn.Click += Btn_Click;
            btn.Font.Size = new FontUnit(FontSize.Smaller);
            th.Controls.Add(btn);

            th = CellH(footer);
            th.ColumnSpan = roles.Count;
            btn = new Button();
            btn.Text = "Save Rights";
            btn.Width = new Unit("100%");
            btn.Click += btnSave_Click;
            th.Controls.Add(btn);
            th.Style.Add("text-align", "right");

            _txt = txt;
        }
        catch (Exception ex)
        {
            CSession.PageMessageEx = ex;
        }
    }
    public void RenameDeleteFeature()
    {
        tblEdit.Visible = true;

        var db = Instance.DatabaseDirectOrWeb;
        _feature = new CFeature(db).SelectAll().GetById(FeatureId);
        txt.Text = _feature.Name;
    }
    #endregion

    #region Form Events
    protected void rbl_SelectedIndexChanged(object sender, EventArgs e)
    {
        CSession.FeaturesAppId = CDropdown.GetInt(rbl);
        Response.Redirect(Request.RawUrl);
    }
    protected void ddApp_SelectedIndexChanged(object sender, EventArgs e)
    {
        var app = CApp.Cache.GetById(CDropdown.GetInt(ddApp));
        Response.Redirect(CSitemap.InstanceFeatures(app.Instances[0].InstanceId), true);
    }
    protected void ddIns_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect(CSitemap.InstanceFeatures(CDropdown.GetInt(ddIns)), true);
    }
    //Checkboxes
    private void btnSave_Click(object sender, EventArgs e)
    {
        var db = Instance.DatabaseDirectOrWeb;
        foreach (var i in _dict)
        {
            var k = i.Key;
            var chk = i.Value;
            var rId = k.Item1;
            var fId = k.Item2;
            var rf = _matrix.GetById(rId, fId);
            if (chk.Checked && null == rf)
            {
                rf = new CApplicationRoleFeature(db);
                rf.FeatureID = fId;
                rf.ApplicationRoleID = rId;
                rf.Save();
            }
            else if (!chk.Checked && null != rf)
                rf.Delete();
        }
        Response.Redirect(Request.RawUrl);
    }

    //Add Feature
    private void Btn_Click(object sender, EventArgs e)
    {
        if (_txt.Text == string.Empty)
            return;

        var db = Instance.DatabaseDirectOrWeb;

        var f = new CFeature(db);
        f.Name = _txt.Text;
        f.ApplicationID = CSession.FeaturesAppId;
        f.Save();

        Response.Redirect(Request.RawUrl);
    }
    protected void btnRename_Click(object sender, EventArgs e)
    {
        _feature.Name = txt.Text;
        _feature.Save();

        Response.Redirect(CSitemap.InstanceFeatures(InstanceId));
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        foreach (var i in _matrix.GetByFeatureID(FeatureId))
            i.Delete();
        _feature.Delete();

        Response.Redirect(CSitemap.InstanceFeatures(InstanceId));
    }
    #endregion




}