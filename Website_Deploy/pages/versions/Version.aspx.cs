using System;
using System.Collections.Generic;

using SchemaDeploy;
using Framework;
using System.IO.Compression;

partial class pages_Versions_Version : CPageDeploy
{
    #region Querystring
    public int VersionId { get { return CWeb.RequestInt("versionId"); } }
    public int AppId { get { return IsEdit ? Version.VersionAppId : CWeb.RequestInt("appId"); } }
    public bool IsEdit { get { return VersionId != int.MinValue; } }
    public CApp App { get { return CApp.Cache.GetById(AppId); } }
    #endregion

    #region Members
    private CVersion _version;
    #endregion
    
    #region Data
    public CVersion Version 
    {
        get 
        {
            if (_version == null) 
            {
                if (IsEdit) 
                {
                    _version = CVersion.Cache.GetById(VersionId);
                    if (_version == null)
                        CSitemap.RecordNotFound("Version", VersionId); 
                }
                else 
                {
                    _version = new CVersion();
                    _version.VersionAppId = AppId;
                    _version.VersionName = "Version #" + (_version.App.Versions.Count + 1);
                }
            }
            return _version;
        }
    }
    #endregion
    
    #region Navigation
    private void Refresh()      { Response.Redirect(CSitemap.VersionEdit(this.Version.VersionId)); }
    private void ReturnToList() { Response.Redirect(CSitemap.Versions(AppId)); }
    #endregion

    #region Event Handlers - Page
    protected override void PageInit()
    {
        if (AppId < 0)
        {
            Response.Redirect(CSitemap.Apps());
            return;
        }

        fuUpload.FolderPath = Server.MapPath("~/App_Data/" + AppId);

        ddApp.DataSource = CApp.Cache;
        ddApp.DataBind();
        CDropdown.SetValue(ddApp, AppId);

        var a = Version.App;
        ddVer.DataSource = a.Versions;
        ddVer.DataBind();
        CDropdown.BlankItem(ddVer, "-- Upload -- ");
        CDropdown.SetValue(ddVer, VersionId);

        //Page title
        if (IsEdit) 
            this.Title = Version.VersionName;
        else 
            this.Title = "New Version: " + a.AppName;

        //Textbox logic
        this.Form.DefaultFocus  = txtVersionName.Textbox.ClientID;
        this.Form.DefaultButton = btnSave.UniqueID;   //txtVersionName.OnReturnPress(btnSave);


		UnbindSideMenu();
		MenuVersionDetails(AppId, VersionId);


        txtExceptions.Text = CUtilities.ListToString(CFolderHash.DEFAULT_EXCEPT_EXTENSIONS);



        //Button Text
        btnDelete.Visible = IsEdit;
        if (IsEdit)
        {
            btnCancel.Text = "Back";
            AddButton(CSitemap.VersionAdd(AppId), "Upload new Version");
            plhUpload.Visible = false;
            txtVersionName.Mode = EControlMode.Editable;
            txtExceptions.Visible = false;
        }
        else
        {
            plhView.Visible = false;
            plhSchema.Visible = false;
        }
    }
    protected override void PageLoad()
    {
        LoadVersion();
    }
    #endregion

    #region Event Handlers - Form
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!this.IsValid)
            return;

        SaveVersion();
    //CCache.ClearCache();  //e.g. if you have more than one application, need to request the clearcache page on the other app
        Refresh();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ReturnToList();
    }
    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (this.Version.App.AppMainVersionId == VersionId)
        {
            this.Version.App.AppMainVersionId = int.MinValue;
            this.Version.App.Save();
        }
        this.Version.Delete();
    //CCache.ClearCache();  //e.g. if you have more than one application, need to request the clearcache page on the other app
        ReturnToList();
    }

    protected void ddApp_SelectedIndexChanged(object sender, EventArgs e)
    {
        var appId = CDropdown.GetInt(ddApp);
        var app = CApp.Cache.GetById(appId);
        if (app.Versions.Count == 0)
            Response.Redirect(CSitemap.Versions(appId));
        else
            Response.Redirect(CSitemap.VersionEdit(app.Versions[0].VersionId));
    }
    protected void ddVer_SelectedIndexChanged(object sender, EventArgs e)
    {
        var verId = CDropdown.GetInt(ddVer);
        var appId = CDropdown.GetInt(ddApp);
        if (verId > 0)
            Response.Redirect(CSitemap.VersionEdit(verId));
        else
            Response.Redirect(CSitemap.VersionAdd(appId));
    }
    #endregion

    #region Private - Load/Save
    protected void LoadVersion()
    {
        var v = this.Version;
        txtVersionName.Text = v.VersionName;
        txtVersionCreated.Text = CUtilities.Timespan(v.VersionCreated);
        txtVersionExpires.Text = CUtilities.Timespan(v.VersionExpires);
        txtVersionExpires.Visible = v.VersionExpires != DateTime.MinValue;

        txtVersionTotalBytes.Text = string.Concat(v.VersionFilesB64, v.VersionTotalFileSize, ", ", CUtilities.CountSummary(v.BinaryFiles, "file"));
        txtVersionTotalBytes.NavigateUrl = CSitemap.BinaryFiles(v.VersionAppId, VersionId);

        txtVersionSchemaMD5.Text = v.VersionSchemaB64;
        if (!Guid.Empty.Equals(v.VersionSchemaMD5))
            txtVersionSchemaMD5.NavigateUrl = CSitemap.Schema(v.VersionSchemaMD5);
        else
            txtVersionSchemaMD5.Visible = false;

        plhSchema.Visible = IsEdit && (v.VersionSchemaMD5 == Guid.Empty);
        if (v.App.VersionCount < 2)
        {
            txtVersionDeltaSchemaMd5.Visible = false;
        }
        else
        {
            var v2 = v.App.Versions[1];
            if (v2.VersionSchemaMD5 != v.VersionSchemaMD5)
            {
                txtVersionDeltaSchemaMd5.Text = v2.VersionSchemaB64;
                txtVersionDeltaSchemaMd5.NavigateUrl = CSitemap.BinaryFiles("", v.VersionAppId, VersionId, true);
            }
        }
        if (IsEdit)
        {
            txtIgnored.Text = v.VersionExcludedFiles;
            txtLocation.Text = v.VersionUploadedFrom;
        }

    }
    protected void SaveVersion()
    {
        this.Version.VersionName = txtVersionName.Text;
        this.Version.VersionAppId = AppId;

        this.Version.Save();
    //CCache.ClearCache();  //e.g. if you have more than one application, need to request the clearcache page on the other app


    }
    #endregion

    protected void btnLocal_Click(object sender, EventArgs e)
    {
        var v = Version;
		v.VersionName = txtVersionName.Text;
		v.VersionExcludedFiles = txtExceptions.Text;
        v.VersionUploadedFrom = txtLocalPath.Text;
        SaveVersion();

        var except = CUtilities.StringToListStr(txtExceptions.Text).ToArray();
        var path = txtLocalPath.Text;
        var fi = new CFolderHash(path, except, true);

        var bulk = new CVersionFileList(fi.Count);
        foreach (var i in fi)
        {
            var b = CBinaryFile.Cache.GetById(i.MD5);
            if (null == b)
            {
                b = new CBinaryFile();
                b.IsSchema = false;
                b.SetFile(path, i.Name);
                b.Save();
            }

            var vf = new CVersionFile();
            vf.VFVersionId = v.VersionId;
            vf.VFPath = i.Name;
            vf.VFBinaryMD5 = i.MD5;
            bulk.Add(vf);
        }
        bulk.BulkInsert();


        v.VersionTotalBytes = bulk.TotalBytes;
        v.VersionFilesMD5 = bulk.BinaryFiles.TotalHash;
        v.Save();

        Response.Redirect(CSitemap.VersionEdit(v.VersionId));
    }


    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (!fuUpload.HasFile)
            return;

        if (!fuUpload.FileName.ToLower().EndsWith(".zip"))
            return;


        var except = CUtilities.StringToListStr(txtExceptions.Text.ToLower());
        var zip = new ZipArchive(fuUpload.FileUpload.FileContent);


        var v = Version;
		v.VersionName = txtVersionName.Text;
		v.VersionExcludedFiles = txtExceptions.Text;
        v.VersionUploadedFrom = fuUpload.FileUpload.PostedFile.FileName;

        var bulk = new CVersionFileList((int)zip.Entries.Count);
        var skip = new List<string>();
        int stored = 0;
        foreach (ZipArchiveEntry i in zip.Entries)
        {
            //Exceptions
            foreach (var j in except)
                if (i.Name.ToLower().EndsWith(j))
                {
                    skip.Add(i.Name);
                    continue;
                }

			//Directory
			if (string.IsNullOrEmpty(i.Name))
				continue;

            //Unzip
            var br = new System.IO.BinaryReader(i.Open());
            var bin = br.ReadBytes((int)i.Length);
            br.Close();

            //Hash
            var fh = new CFileHash(i.FullName, CBinary.MD5_(bin));

            //Store
            var b = CBinaryFile.Cache.GetById(fh.MD5);
            if (null == b)
            {
                b = new CBinaryFile();
                b.IsSchema = false;
                b.SetFile(bin, i.FullName);
                b.Save();
                stored++;
            }

            //Link
            var vf = new CVersionFile();
            vf.VFPath = i.FullName;
            vf.VFBinaryMD5 = fh.MD5;
            bulk.Add(vf);
        }

        v.VersionTotalBytes = bulk.TotalBytes;
        v.VersionFilesMD5 = bulk.BinaryFiles.TotalHash;
        v.Save();

        bulk.VersionId = v.VersionId;
        bulk.TrimCommonPathPrefix();
        bulk.BulkInsert();

        CSession.PageMessage = string.Concat("Stored ", CUtilities.CountSummary(stored, "file"), ", Linked ", CUtilities.CountSummary(bulk.Count, "file"), ", ", v.VersionTotalFileSize);
        if (skip.Count > 0)
            CSession.PageMessage = string.Concat(CSession.PageMessage, "\r\n\r\nSkipped:\r\n", CUtilities.ListToString(skip, "\r\n"));

        Response.Redirect(CSitemap.VersionEdit(v.VersionId));
    }

    protected void btnImportSch_Click(object sender, EventArgs e)
    {
        var db = new CSqlClient(txtDatabase.Text);
        var info = db.SchemaInfo();
        var bin = CProto.Serialise(info);
        var hash = info.MD5();

        var b = CBinaryFile.Cache.GetById(hash);
        if (null == b)
        {
            b = new CBinaryFile();
            b.SetFile(info);
            b.IsSchema = true;
            b.Save();
        }

        var vf = new CVersionFile();
        vf.VFBinaryMD5 = info.MD5(); //b.MD5;
        vf.VFVersionId = VersionId;

        var match = CVersionFile.Cache.GetByVersionId(vf.VFVersionId).GetByBinaryMD5(vf.VFBinaryMD5);
        if (0 != match.Count)
            vf = match[0];
        vf.VFPath = info.ToString();
        vf.Save();
        
        Version.VersionSchemaConnStr = txtDatabase.Text;
        Version.VersionSchemaMD5 = hash;
        Version.Save();

        Response.Redirect(Request.RawUrl);
    }
}
