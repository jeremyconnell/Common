using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;

public partial class pages_binaryFiles_Schema : CPageWithTableHelpers
{
    #region Querystring
    public Guid MD5 { get { return null != Version ? Version.VersionSchemaMD5 : CWeb.RequestGuid("md5"); } }
	public int VersionId { get { return CWeb.RequestInt("versionId"); } }
	public int InstanceId { get { return CWeb.RequestInt("instanceId"); } }
	#endregion

	#region Members
	private CSchemaInfo _schema;
	#endregion

	#region Data
	private CVersion Version { get { return CVersion.Cache.GetById(VersionId); } }
	private CBinaryFile Binary { get { return CBinaryFile.Cache.GetById(MD5); } }
	private CInstance Instance { get { return CInstance.Cache.GetById(InstanceId); } }


	private static CWebSrcBinary PROD_DB = new CWebSrcBinary("https://admin.controltrackonline.com");
	public CSchemaInfo Schema
    {
        get
        {
			if (null == _schema)
			{
				try
				{
					if ((int)ESource.Local == InstanceId)
						_schema = CDataSrc.Default.SchemaInfo();
					else if ((int)ESource.Prod == InstanceId)
						_schema = PROD_DB.SchemaInfo();
					else if (null != Instance)
						_schema = Instance.SchemaInfo;//.DatabaseDirectOrWeb.SchemaInfo();
					else if (MD5 != Guid.Empty)
						_schema = CProto.Deserialise<CSchemaInfo>(Binary.GetFile());
					else
						Response.Redirect(CSitemap.AppSchema());
				}
				catch (Exception ex)
				{
					lblMigration.Text = ex.Message;
					lblMigration.Font.Size = new FontUnit(FontSize.Smaller);
					lblMigration.ForeColor = System.Drawing.Color.Red;
					_schema = new CSchemaInfo(CDataSrc.Default);
				}
			}
            return _schema;
        }
    }
    #endregion

    #region Navigation
    private void ReturnToList() { Response.Redirect(CSitemap.BinaryFiles()); }
    #endregion

    #region Event Handlers - Page
    protected override void PageInit()
    {
        //Populate Dropdowns
        ddVersion.DataSource = CVersion.Cache.GetBySchemaMD5(MD5);
        ddVersion.DataBind();
        CDropdown.BlankItem(ddVersion, "-- Versions --");
		CDropdown.SetValue(ddVersion, VersionId);

		ddInstance.DataSource = CInstance.Cache;
		ddInstance.DataBind();
		//CDropdown.AddEnums(ddInstance, typeof(ESource));
		CDropdown.BlankItem(ddInstance, "PROD", ((int)ESource.Prod).ToString());
		CDropdown.BlankItem(ddInstance, "LOCAL", ((int)ESource.Local).ToString());
		CDropdown.BlankItem(ddInstance, "-- Deploys --");
		CDropdown.SetValue(ddInstance, InstanceId);


		//Button Text
		var verName = CUtilities.Truncate(Version?.VersionName??"", 20);
		var insName = CUtilities.Truncate(Instance?.NameAndSuffix ?? "", 20);
		AddMenuSide("Apps...", CSitemap.Apps());
        AddMenuSide("Binaries...", CSitemap.BinaryFiles());
		AddMenuSide("Admin...", CSitemap.SelfSchemaSync());
		AddMenuSide("Clients...", CSitemap.AppSchema((int)EApp.ControlTrack), (VersionId + InstanceId )== int.MinValue);
		if (VersionId > 0)
			AddMenuSide(verName, CSitemap.VersionEdit(VersionId), true);

		if (InstanceId > 0)
		{
			AddMenuSide("View", CSitemap.SchemaForInstance(InstanceId), true);
			AddMenuSide("Diff", CSitemap.InstanceSchema(InstanceId));
		}
		else if (InstanceId != int.MinValue)
		{
			AddMenuSide("View", CSitemap.SchemaForInstance(InstanceId), true);
			AddMenuSide("Diff", CSitemap.SelfSchemaSync_Diff(InstanceId));
		}

		if (InstanceId > 0)
			Page.Title = string.Concat(Page.Title, " (", insName, ") ", Schema.MD5_);
		else if (InstanceId != int.MinValue)
			Page.Title = string.Concat(Page.Title, " (", ddInstance.SelectedItem.Text, ") ", Schema.MD5_);
		else if (VersionId > 0)
			Page.Title = string.Concat(Page.Title, " (", verName, ") ", Schema.MD5_);
		else
			Page.Title += " (stored) " + Schema.MD5_;
    }
    protected override void PagePreRender()
    {
        litTables.Text = CUtilities.NameAndCount(litTables.Text, Schema.Tables.Count);
        litViews.Text = CUtilities.NameAndCount(litViews.Text, Schema.Views.Count);
        litProcs.Text = CUtilities.NameAndCount(litProcs.Text, Schema.Procs.StoredProcs.Count);
        litFunc.Text = CUtilities.NameAndCount(litFunc.Text, Schema.Procs.Functions.Count);
        lblDefs.Text = CUtilities.NameAndCount(litDefs.Text, Schema.DefaultValues.Count);

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
        lblDefs.Text = CBinary.ToBase64(Schema.DefaultValues.MD5, 10);

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

            tblDefs.Visible = true;
            divDefs.Visible = false;
            foreach (var i in Schema.DefaultValues)
            {
                var tr = Row(tblDefs);
                Cell(tr, (Schema.DefaultValues.IndexOf(i) + 1) + ".");
                Cell(tr, i.Name);
                Cell(tr, CBinary.ToBase64(i.MD5, 10));
                Cell(tr, i.TableName);
                Cell(tr, i.ColumnName);
                Cell(tr, i.Definition, i.ToString());
            }
            lblDefHash.Text = CBinary.ToBase64(Schema.ForeignKeys.MD5, 10);

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

            foreach (var i in Schema.DefaultValues)
                UCDefaultValue(plhDefVals).Display(i, Schema, detail);
        }


		if (Schema.Procs.Count == 0)
		{
			plhProcs.Visible = false;
			tblProcs.Visible = false;
		}


    }
    #endregion


    protected void ddVersion_SelectedIndexChanged(object sender, EventArgs e)
    {
        var verId = CDropdown.GetInt(ddVersion);
            Response.Redirect(CSitemap.Schema(verId), true);
    }

	protected void ddInstance_SelectedIndexChanged(object sender, EventArgs e)
	{
		var instanceId = CDropdown.GetInt(ddInstance);
			Response.Redirect(CSitemap.SchemaForInstance(instanceId));

	}

	#region User Controls
	private static pages_binaryFiles_usercontrols_UCTable UCTableInfo(Control target)
    {
        Control ctrl = target.Page.LoadControl(CSitemap.UCTableInfo());
        target.Controls.Add(ctrl);
        return (pages_binaryFiles_usercontrols_UCTable)ctrl;
    }
    private static pages_binaryFiles_usercontrols_UCStoredProc UCStoredProc(Control target)
    {
        Control ctrl = target.Page.LoadControl(CSitemap.UCStoredProc());
        target.Controls.Add(ctrl);
        return (pages_binaryFiles_usercontrols_UCStoredProc)ctrl;
    }
    private static pages_binaryFiles_usercontrols_UCView UCView(Control target)
    {
        Control ctrl = target.Page.LoadControl(CSitemap.UCView());
        target.Controls.Add(ctrl);
        return (pages_binaryFiles_usercontrols_UCView)ctrl;
    }
    private static pages_binaryFiles_usercontrols_UCForeignKey UCForeignKey(Control target)
    {
        Control ctrl = target.Page.LoadControl(CSitemap.UCForeignKey());
        target.Controls.Add(ctrl);
        return (pages_binaryFiles_usercontrols_UCForeignKey)ctrl;
    }
    private static pages_binaryFiles_usercontrols_UCDefaultValue UCDefaultValue(Control target)
    {
        Control ctrl = target.Page.LoadControl(CSitemap.UCDefaultValue());
        target.Controls.Add(ctrl);
        return (pages_binaryFiles_usercontrols_UCDefaultValue)ctrl;
    }
    #endregion


    protected void chkDetail_CheckedChanged(object sender, EventArgs e)
	{

	}
}
