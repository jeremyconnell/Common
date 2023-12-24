using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;

public partial class pages_backupitems_usercontrols_UCBackupItem : System.Web.UI.UserControl 
{     
    #region Interface
    public void Display(CBackupItem backupItem, CBackupItemList list, CPagingInfo pi) 
    { 
        if (Parent.Controls.Count % 2 == 0) row.Attributes.Add("class", "alt_row"); 
        
        CBackupItem bi = backupItem;

        litNumber.Text = Convert.ToString(list.IndexOf(bi) + 1 + pi.Offset);

        lnkCreated.Text = CUtilities.Timespan(bi.BackupCreated);
        lnkCreated.ToolTip = CUtilities.LongDateTime(bi.BackupCreated);
        lnkCreated.NavigateUrl = CSitemap.BackupItem(bi.ItemId);

        lnkItemTableName.ToolTip = bi.ItemTableName;
        lnkItemTableName.Text = bi.ItemTableName.Replace("[","").Replace("]","");
        lnkInstance.NavigateUrl = CSitemap.BackupItems(bi.Backup.Instance.App.AppId, int.MinValue, bi.TableName, null);

        lnkInstance.Text = bi.InstanceClientName;
        lnkInstance.NavigateUrl = CSitemap.BackupItems(int.MinValue, bi.BackupInstanceId);

        if (bi.ItemRowCount > 0)
        {
            lblItemRowCount.Text = bi.ItemRowCount.ToString("n0");

            if (bi.SchemaBytes > 0) litItemDatasetMD5.Text = CBinary.ToBase64(bi.ItemDatasetMD5, 6);
            if (bi.SchemaBytes > 0) litItemSchemaMD5.Text  = CBinary.ToBase64(bi.ItemSchemaMD5,  6);
            if (bi.SchemaBytes > 0) litItemSchema.Text     = CUtilities.FileSize(bi.SchemaBytes);
            if (bi.DataBytes   > 0) litItemDataset.Text    = CUtilities.FileSize(bi.DataBytes);
        }

    } 
    #endregion
} 
