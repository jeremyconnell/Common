using Framework;
using SchemaControlTrack;
using SchemaDeploy;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;



public partial class pages_instances_dataImport :  CPageDeploy
{
    #region Querystring
    public int InstanceId { get { return CWeb.RequestInt("instanceId"); } }
    public int AppId { get { return null != Instance ? Instance.InstanceAppId : CWeb.RequestInt("appId"); } }
    #endregion

    #region Data
    public CInstance Instance { get { return CInstance.Cache.GetById(InstanceId); } }
    public CApp App { get { return CApp.Cache.GetById(AppId); } }
    #endregion


    protected override void PageInit()
    {
        ddApp.DataSource = CApp.Cache.WithInstances;
        ddApp.DataBind();
        CDropdown.SetValue(ddApp, AppId);

        ddIns.DataSource = App.Instances;
        ddIns.DataBind();
        CDropdown.SetValue(ddIns, InstanceId);


        var db = Instance?.DatabaseDirectOrWeb;
        if (null != db)
        {
            try
            {
                ddRiskRatingScheme.DataSource = db.SelectAll("RiskRatingSchemeID,Name", "[RA].[RiskRatingScheme]", "Name");
                ddRiskRatingScheme.DataBind();

                ddRiskRegister.DataSource = db.SelectAll("RiskRegisterID,Name", "[RA].[RiskRegister]", "Name");
                ddRiskRegister.DataBind();

                dgExisting.DataSource = db.SelectAll_Dataset("ra.riskconsequencedescription");
                dgExisting.DataBind();

                ddCat.DataSource = db.SelectAll_Dataset("ra.ConsequenceCategory");
                ddCat.DataBind();

                ddCon.DataSource = db.SelectAll_Dataset("ra.RiskConsequence");
                ddCon.DataBind();
            }
            catch (Exception ex)
            {
                CSession.PageMessageEx = ex;
            }
        }


        MenuInstanceDataImport(AppId, InstanceId);


        if (null != CSession.DataTable)
        {
            dg.DataSource = CSession.DataTable;
            dg.DataBind();
        }

        divExisting.Visible = true;
        divUpl.Visible = true;
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (!fi.HasFile)
        {
            CSession.PageMessage = "No File!";
            return;
        }

        var f = fi.PostedFile;
        var temp = Server.MapPath("~/App_Data/");
        if (!Directory.Exists(temp))
            Directory.CreateDirectory(temp);

        var name = Path.GetFileName(f.FileName);
        var path = temp + name;
        fi.SaveAs(path);

        DataSet ds = null;
        if (path.EndsWith(".xml"))
        {
            ds = new DataSet();
            ds.ReadXmlSchema(path + ".xsd");
            ds.ReadXml(path);
            File.Delete(path + ".xsd");
            File.Delete(path);
        }
        else
        {
            var db = CDataSrc.OleDbFromExcelPath(path, true, true);
            try
            {
                ds = db.SelectAll_Dataset(CDataSrc.EXCEL_DEFAULT_TABLE_NAME);
            }
            catch
            {
                var tbls = db.AllTableNames();
                ds = db.SelectAll_Dataset(tbls[0]);
            }

            File.Delete(path);

            path += ".xml";
            File.WriteAllText(path, ds.GetXml());
            File.WriteAllText(path + ".xsd", ds.GetXmlSchema());
        }

        CSession.DataTable = ds.Tables[0];
        Response.Redirect(Request.RawUrl);
    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
        var db = Instance.DatabaseDirectOrWeb;
        var dt = CSession.DataTable;


        var ccc = new CConsequenceCategory(db).SelectAll();
        var rrr = new CRiskConsequence(db).SelectAll();



        foreach (DataColumn dc in dt.Columns)
        {
            var idx = dt.Columns.IndexOf(dc);
            if (idx < 1) continue;

            if (ccc.GetByName(dc.ColumnName) != null)
                continue;

            var cc = new CConsequenceCategory(db);
            cc.RiskRegisterID = CDropdown.GetInt(ddRiskRegister);
            cc.Rank = 1;
            cc.Name = dc.ColumnName;

            cc.CreatedDateTime = DateTime.Now;
            cc.UpdatedDateTime = DateTime.Now;
            cc.UpdatedByUserID = 1;
            cc.CreatedByUserID = 1;
            cc.IsActive = true;

            cc.Save();

            ccc.Add(cc);
        }

        var schemaId = CDropdown.GetInt(ddRiskRatingScheme);

        foreach (DataRow dr in dt.Rows)
        {
            if (IsBlank(dr)) continue;

            var level = CAdoData.GetInt(dr, 0);
            var descr = CAdoData.GetStr(dr, 1);


            var c = rrr.GetByName_(descr);
            if (null == c)
            {
                foreach (var i in rrr)
                    foreach (var j in rrr)
                        if (i.Score == level && i.RiskRatingSchemeID == schemaId)
                        {
                            c = i;
                            if (c.Name != descr)
                            {
                                c.Name = descr;
                                c.Save();
                            }
                            break;
                        }
            }
            if (null == c)
            {
                c = new CRiskConsequence(db);

                c.Name = descr;
                c.Score = level;
                c.ScoreLabel = descr;

                c.RiskRatingSchemeID = schemaId;

                c.CreatedDateTime = DateTime.Now;
                c.UpdatedDateTime = DateTime.Now;
                c.UpdatedByUserID = 1;
                c.CreatedByUserID = 1;
                c.IsActive = true;

                c.Save();
            }

            var rcd = new CRiskConsequenceDescription(db).SelectAll();
            rcd = rcd.GetByRiskRegisterID(CDropdown.GetInt(ddRiskRegister));

            foreach (DataColumn dc in dt.Columns)
            {
                var idx = dt.Columns.IndexOf(dc);
                if (idx < 2) continue;


                var d = new CRiskConsequenceDescription(db);
                d.RiskRegisterID = CDropdown.GetInt(ddRiskRegister);
                d.ConsequenceCategoryID = ccc.GetByName(dc.ColumnName).ConsequenceCategoryID;
                d.RiskConsequenceID = c.RiskConsequenceID;

                d.CreatedDateTime = DateTime.Now;
                d.UpdatedDateTime = DateTime.Now;
                d.UpdatedByUserID = 1;
                d.CreatedByUserID = 1;

                var dd = rcd.GetByConsequenceCategoryID(d.ConsequenceCategoryID).GetByRiskConsequenceID(c.RiskConsequenceID);
                if (dd.Count > 0)
                    d = dd[0];

                d.IsActive = true;
                d.Description = CAdoData.GetStr(dr, idx);
                d.Save();
            }
        }

        CSession.DataTable = null;

        Response.Redirect(Request.RawUrl);
    }

    private bool IsBlank(DataRow dr)
    {
        foreach (DataColumn dc in dr.Table.Columns)
            if (!string.IsNullOrEmpty(CAdoData.GetStr(dr, dc.ColumnName)))
                return false;
        return true;
    }

    #region Form Events
    protected void ddApp_SelectedIndexChanged(object sender, EventArgs e)
    {
        var app = CApp.Cache.GetById(CDropdown.GetInt(ddApp));
        var ins = app.Instances[0];
        Response.Redirect(CSitemap.InstanceDataImport(ins.InstanceId), true);
    }
    protected void ddIns_SelectedIndexChanged(object sender, EventArgs e)
    {
        Response.Redirect(CSitemap.InstanceDataImport(CDropdown.GetInt(ddIns)), true);
    }
    #endregion



    protected void btnDeleteInactive_Click(object sender, EventArgs e)
    {
        var i = this.Instance;
        var db = i.DatabaseDirectOrWeb;
        var si = i.SchemaInfo;
        var mg = new List<string>();
        var er = new List<CTableInfo>();
        foreach (CTableInfo t in si.Tables)
            if (t.Has("IsActive"))
                try
                {
                    var rows = db.ExecuteNonQuery("DELETE FROM " + t.TableName_ + " WHERE IsActive=0");
                    mg.Add(t.TableName + "\t" + CUtilities.CountSummary(rows, "row", "none"));
                }
                catch (Exception ex)
                {
                    er.Add(t);
                    mg.Add(t.TableName + "\t" + ex.Message);
                }
            else
                mg.Add(t.TableName + "\tSkipped");

        CSession.PageMessage = CUtilities.ListToString(mg, "\r\n");

        //Second try
        foreach (var t in er)
            try
            {
                var rows = db.ExecuteNonQuery("DELETE FROM " + t.TableName_);
                mg.Add(t.TableName + "\t" + CUtilities.CountSummary(rows, "row", "none"));
            }
            catch (Exception ex)
            {
                mg.Add(t.TableName + "\t" + ex.Message);
            }

        CSession.PageMessage = CUtilities.ListToString(mg, "\r\n");
        Response.Redirect(Request.RawUrl);
    }
}