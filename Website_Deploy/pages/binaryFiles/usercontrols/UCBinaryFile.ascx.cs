using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using SchemaDeploy;
using Framework;
using System.IO;
using System.Collections;
using System.Web.UI.HtmlControls;

public partial class pages_binaryfiles_usercontrols_UCBinaryFile : UserControl 
{
    #region Members
    private CVersionFile _versionFile;
    private CBinaryFile _binaryFile; 
    private IList _sortedList;
    private Label _litUsage;
    #endregion

    #region Interface
    public void Display(CVersionFile vf, IList sortedList)
    {
        _versionFile = vf;
        var bf = vf.BinaryFile;
        Display(bf, sortedList);
       
        litPath.Text = vf.VFPath;
        litNumber.Text = Convert.ToString(sortedList.IndexOf(vf) + 1);

        //if (vf.Version.App.VersionCount == vf.BinaryFile.VersionFiles.Count)
        //    _litUsage.Text = "All";// " + ver.App.VersionCount;//.BinaryFiles.Count;
        colUsg.Visible = false;
        colDel.Visible = false;
        plhAlso.Visible = false;

        if (bf.IsSchema)
            litPath.Visible = false;
    }
    public void Display(CBinaryFile binaryFile, IList sortedList)
    {
        if (Parent.Controls.Count % 2 == 0)
            row.Attributes.Add("class", "alt_row");

        _binaryFile = binaryFile;
        _sortedList = sortedList;

        CBinaryFile bf = _binaryFile;
        litNumber.Text = Convert.ToString(sortedList.IndexOf(bf) + 1);
        litSize.Text = bf.Size_;

        litCreated.Text = CUtilities.Timespan(bf.Created);
        litDeleted.Text = CUtilities.Timespan(bf.Deleted);

        litCreated.ToolTip = CUtilities.LongDateTime(bf.Created);
        litDeleted.ToolTip = CUtilities.LongDateTime(bf.Deleted);

        lblMd5.ToolTip = CBinary.ToBase64(bf.MD5);
        lblMd5.Text = CUtilities.Truncate(lblMd5.ToolTip, 11).Replace("...", "").ToUpper();

        //foreach (var i in bf.Versions)
        //{
        //    var d = new HtmlGenericControl("div");
        //    plh.Controls.Add(d);

        //    var lnk = new HyperLink();
        //    lnk.Text = i.VersionName;
        //    lnk.NavigateUrl = CSitemap.BinaryFiles(string.Empty, i.VersionAppId, i.VersionId);
        //    d.Controls.Add(lnk);
        //}
        _litUsage = new Label();
        _litUsage.Text = bf.N_; //bf.Usage;
        _litUsage.ToolTip = CUtilities.ListToString(bf.VersionFiles.Names, "\r\n");
        plh.Controls.Add(_litUsage);

        if (bf.IsSchema)
        {
            if (string.IsNullOrEmpty(bf.Path) && bf.VersionFiles.Count > 0)
            {
                bf.Path = bf.VersionFiles[0].VFPath;
                bf.Save();
            }
            lnkSchema.Text = CUtilities.Truncate(bf.Path,50);
            lnkSchema.NavigateUrl = CSitemap.Schema(bf.MD5);
            return;
        }

        //litPath.Text = bf.Path;
        foreach (var i in bf.VersionFiles)
            if (i.VFPath.Contains("\\"))
                i.Save();
        var list = new List<string>();
        foreach (var i in bf.VersionFiles)
            if (! list.Contains(i.VFPath.ToLower()))
            {
                list.Add(i.VFPath.ToLower());

                var div = new Panel();
                plhAlso.Controls.Add(div);

                var lbl = new Label();
                lbl.ToolTip = i.VFPath;
                lbl.Text = CUtilities.Truncate(i.VFPath, 100);
                div.Controls.Add(lbl);
            }

    } 
    #endregion 
    
    
    #region Private
    private void Refresh() 
    {
        Response.Redirect(Request.RawUrl);  
    }
    #endregion 

    protected void litPath_Click(object sender, EventArgs e)
    {
        var b = _binaryFile.GetFile();
        Response.ContentType = "application/octet-stream";
        Response.AddHeader("content-disposition", "attachment;filename=" + Path.GetFileName(_binaryFile.Path));
        Response.BinaryWrite(b);
        Response.End();
    }
}
