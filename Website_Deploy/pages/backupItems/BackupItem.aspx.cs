using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;

partial class pages_BackupItems_BackupItem : CPageDeploy
{
    #region Querystring
    public bool IsIframe { get { return CWeb.RequestBool("iframe"); } }
    public int ItemId 
    {
        get 
        {
            int id = CWeb.RequestInt("itemId");
            if (id == int.MinValue) 
                CSitemap.RecordNotFound("BackupItem", id); 
            return id;
        }
    }
    #endregion
    
    #region Members
    private CBackupItem _backupItem;
    #endregion

    #region Data
    public CApp App { get { return Instance.App; } }
    public CInstance Instance { get { return Backup.Instance; } }
    public CBackup Backup { get { return BackupItem.Backup; } }
    public CBackupItem BackupItem
    {
        get 
        {
            if (_backupItem == null) 
            {
                try 
                {
                    _backupItem = new CBackupItem(ItemId);
                }
                catch 
                {
                    CSitemap.RecordNotFound("BackupItem", ItemId);
                }
            }
            return _backupItem;
        }
    }
    #endregion

    #region Event Handlers - Page
    protected override void PageInit()
    {
        var bi = BackupItem;
        foreach (var i in bi.Backup.BackupItems)
            ddTable.Add(i.ItemTableName, i.ItemId);
        ddTable.ValueInt = ItemId;
       

        if (IsIframe)
        {
            Response.ContentType = "text/xml";
            Response.Write(@"<?xml version=""1.0""?>
<!DOCTYPE NewDataSet >
");
            //Response.Write(@"<?xml version=""1.0"" encoding=""UTF-8"" ?>\r\n");
            var ds = bi.Dataset;
            Response.Write(ds.GetXml());
            Response.End();
            return;
        }

        MenuAppDataset(App.AppId, bi);

        txtDeploy.Text = bi.InstanceClientName;
        txtCreated.Text = CUtilities.Timespan(bi.BackupCreated);


        txtItemDatasetXmlGz.Text = CUtilities.FileSize(bi.DataBytes);
        txtItemDatasetXmlGz.ToolTip = CBinary.ToBase64(bi.ItemDatasetMD5, 8);
        txtItemSchemaXmlGz.Text  = CUtilities.FileSize(bi.DataBytes);
        txtItemSchemaXmlGz.ToolTip = CBinary.ToBase64(bi.ItemSchemaMD5, 8);

        //iframe.Src = CSitemap.BackupItem(ItemId) + "&iframe=true";
    }
    protected override void PagePreRender()
    {
        var bi = BackupItem;
        dg.DataSource = bi.Dataset;
        dg.DataBind();
    }
    #endregion

    protected void ddTable_SelectedIndexChanged()
    {
        Response.Redirect(CSitemap.BackupItem(ddTable.ValueInt), true);
    }
}