using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Framework;

public partial class pages_self_SchemaView : CPageWithTableHelpers
{
    public CDataSrc LOCAL_DB = CDataSrc.Default;
    public CDataSrc PROD_DB = new CWebSrcBinary(CSession.Home_ProdUrl);

    #region Querystring
    public int InstanceId {  get { return CWeb.RequestInt("instanceId"); } }
    #endregion


    #region Data
    private CSchemaInfo _schema;
    public CSchemaInfo Schema
    {
        get
        {
            if (null == _schema)
                _schema = rbl.SelectedIndex > 0 ? PROD_DB.SchemaInfo() : LOCAL_DB.SchemaInfo();
            return _schema;
        }
    }
    #endregion

    #region Navigation
    private void ReturnToList() { Response.Redirect(CSitemap.SelfSchemaSync()); }
    #endregion

    #region Event Handlers - Page
    protected override void PageInit()
    {
        MenuSelected = "Self";

        AddMenuSide("Deploy", CSitemap.SelfDeploy());
        AddMenuSide("Schema", CSitemap.SelfSchemaSync());
        AddMenuSide("Data", CSitemap.SelfDataSync());
        AddMenuSide("Sql", CSitemap.SelfSql());
        AddMenuSide("*Local", CSitemap.SchemaView((int)ESource.Local), InstanceId == (int)ESource.Local);
        AddMenuSide("*Prod",  CSitemap.SchemaView((int)ESource.Prod),  InstanceId == (int)ESource.Prod);

        CDropdown.SetValue(rbl, InstanceId);
    }
    protected override void PagePreRender()
    {
        litTables.Text = CUtilities.NameAndCount(litTables.Text, Schema.Tables.Count);
        litViews.Text = CUtilities.NameAndCount(litViews.Text, Schema.Views.Count);
        litProcs.Text = CUtilities.NameAndCount(litProcs.Text, Schema.Procs.StoredProcs.Count);
        litFunc.Text = CUtilities.NameAndCount(litFunc.Text, Schema.Procs.Functions.Count);
        //lblDefs.Text = CUtilities.NameAndCount(litDefs.Text, Schema.DefaultValues.Count);

        var m = Schema.Migration;
        if (!string.IsNullOrEmpty(m.MigrationId))
            lblMigration.Text = string.Concat("#", m.RowNumber, ": ", m.MigrationId);
        else
            tdMig.Visible = false;

        lblTables.Text = CBinary.ToBase64(Schema.Tables.MD5, 10);
        lblViews.Text = CBinary.ToBase64(Schema.Views.MD5, 10);
        lblProcs.Text = CBinary.ToBase64(Schema.Procs.MD5, 10);
        lblIndexes.Text = CBinary.ToBase64(Schema.Indexes.MD5, 10);
        lblPks.Text = CBinary.ToBase64(Schema.PrimaryKeys.MD5, 10);
        lblFks.Text = CBinary.ToBase64(Schema.ForeignKeys.MD5, 10);
        //lblDefs.Text = CBinary.ToBase64(Schema.DefaultValues.MD5, 10);

        bool detail = chkDetail.Checked;
        if (!detail)
        {
            tblViews.Visible = true;
            divViews.Visible = false;
            foreach (var i in Schema.Views)
            {
                var tr = Row(tblViews);
                Cell(tr, (Schema.Views.IndexOf(i) + 1) + ".");
                Cell(tr, i.ViewName, CUtilities.ListToString(i.Columns.NamesAbc), true);
                Cell(tr, CBinary.ToBase64(i.MD5, 10));
            }
            lblViewsHash.Text = CBinary.ToBase64(Schema.Views.MD5, 10);


            tblTables.Visible = true;
            divTables.Visible = false;
            foreach (var i in Schema.Tables)
            {
                var tr = Row(tblTables);
                Cell(tr, (Schema.Tables.IndexOf(i) + 1) + ".");
                Cell(tr, i.TableName, CUtilities.ListToString(i.Columns.NamesAbc), true);
                Cell(tr, CBinary.ToBase64(i.MD5, 10));
            }
            lblTablesHash.Text = CBinary.ToBase64(Schema.Tables.MD5, 10);


            tblProcs.Visible = true;
            plhProcs.Visible = false;
            foreach (var i in Schema.Procs)
            {
                var tr = Row(tblProcs);
                Cell(tr, (Schema.Procs.IndexOf(i) + 1) + ".");
                Cell(tr, i.Name, i.Name, true);
                Cell(tr, CBinary.ToBase64(i.MD5, 10));
                Cell(tr, i.IsStoredProc ? "StoredProc" : "Function");
            }
            lblProcsHash.Text = CBinary.ToBase64(Schema.Procs.MD5, 10);

            tblFks.Visible = true;
            divFKs.Visible = false;
            foreach (var i in Schema.ForeignKeys)
            {
                var tr = Row(tblFks);
                Cell(tr, (Schema.ForeignKeys.IndexOf(i) + 1) + ".");
                Cell(tr, i.KeyName, i.KeyName, true);
                Cell(tr, CBinary.ToBase64(i.MD5, 10));
                Cell(tr, i.CascadeUpdate ? "Cascade-Update" : i.CascadeDelete ? "Cascade-Delete" : "");
            }
            lblFkHash.Text = CBinary.ToBase64(Schema.ForeignKeys.MD5, 10);

            //tblDefs.Visible = true;
            //divDefs.Visible = false;
            //foreach (var i in Schema.DefaultValues)
            //{
            //    var tr = Row(tblDefs);
            //    Cell(tr, (Schema.DefaultValues.IndexOf(i) + 1) + ".");
            //    Cell(tr, i.Name);
            //    Cell(tr, CBinary.ToBase64(i.MD5, 10));
            //    Cell(tr, i.TableName);
            //    Cell(tr, i.ColumnName);
            //    Cell(tr, i.Definition, i.ToString());
            //}
            //lblDefHash.Text = CBinary.ToBase64(Schema.ForeignKeys.MD5, 10);

        }
        else
        {
            foreach (var i in Schema.Views)
                UCView(plhViews).Display(i, Schema);


            foreach (var i in Schema.Tables)
                UCTableInfo(plhTables).Display(i, Schema, detail);

            foreach (var i in Schema.Procs)
                UCStoredProc(plhScript).Display(i, Schema, detail);

            foreach (var i in Schema.ForeignKeys)
                UCForeignKey(plhFks).Display(i, Schema, detail);

            //foreach (var i in Schema.DefaultValues)
            //    UCDefaultValue(plhDefVals).Display(i, Schema, detail);
        }


        if (Schema.Procs.Count == 0)
        {
            plhProcs.Visible = false;
            tblProcs.Visible = false;
        }


    }
    #endregion

    

    #region User Controls
    private static pages_self_usercontrols_UCTable UCTableInfo(Control target)
    {
        Control ctrl = target.Page.LoadControl(CSitemap.UCTableInfo());
        target.Controls.Add(ctrl);
        return (pages_self_usercontrols_UCTable)ctrl;
    }
    private static pages_self_usercontrols_UCStoredProc UCStoredProc(Control target)
    {
        Control ctrl = target.Page.LoadControl(CSitemap.UCStoredProc());
        target.Controls.Add(ctrl);
        return (pages_self_usercontrols_UCStoredProc)ctrl;
    }
    private static pages_self_usercontrols_UCView UCView(Control target)
    {
        Control ctrl = target.Page.LoadControl(CSitemap.UCView());
        target.Controls.Add(ctrl);
        return (pages_self_usercontrols_UCView)ctrl;
    }
    private static pages_self_usercontrols_UCForeignKey UCForeignKey(Control target)
    {
        Control ctrl = target.Page.LoadControl(CSitemap.UCForeignKey());
        target.Controls.Add(ctrl);
        return (pages_self_usercontrols_UCForeignKey)ctrl;
    }
    private static pages_self_usercontrols_UCDefaultValue UCDefaultValue(Control target)
    {
        Control ctrl = target.Page.LoadControl(CSitemap.UCDefaultValue());
        target.Controls.Add(ctrl);
        return (pages_self_usercontrols_UCDefaultValue)ctrl;
    }
    #endregion


    protected void chkDetail_CheckedChanged(object sender, EventArgs e)
    {

    }
}
