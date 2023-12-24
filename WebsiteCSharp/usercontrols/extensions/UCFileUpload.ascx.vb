
Partial Class usercontrols_extensions_UCFileUpload : Inherits CCustomControl

#Region "Events"
    Public Event DeleteClick()
#End Region

#Region "Members"
    Private m_folderPath As String = CSitemap.DefaultUploadsPath
#End Region

#Region "Event Handlers"
    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        MyBase.OnLoad(e)

        If Page.IsPostBack Then
            Me.Required = IIf(String.IsNullOrEmpty(Value), Me.Required, False)
        End If
    End Sub
    Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)
        MyBase.OnPreRender(e)

        lnkLocked.Visible = Len(Value) > 0
        CtrlLocked.Visible = Len(Value) = 0 AndAlso Not Enabled
        btnDelete.Visible = Len(Value) > 0 AndAlso Mode = EControlMode.Editable
        ctrl.Visible = (Len(Value) = 0) AndAlso Enabled AndAlso Mode = EControlMode.Editable
        If lnkLocked.Visible = True Then
            'Sometimes override raw path (e.g. ~/App_Data/...) with a page interface (file.aspx?id=...)
            If String.IsNullOrEmpty(lnkLocked.NavigateUrl) Then lnkLocked.NavigateUrl = String.Concat(FolderPath, "/", FileName)

            'Select Case IO.Path.GetExtension(FileName.ToLower)
            '    Case ".gif", ".jpg", ".png", ".jpeg"
            '        lnkLocked.Text = String.Empty
            '        lnkLocked.ImageUrl = lnkLocked.NavigateUrl 'String.Concat(FolderPath, "/", FileName)
            'End Select
        End If
    End Sub
    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnDelete.Click
        Dim filePath As String = String.Concat(FolderPath, "/", FileName)
        If Not IO.File.Exists(filePath) Then filePath = Server.MapPath(filePath)
        If IO.File.Exists(filePath) Then IO.File.Delete(filePath)

        FileName = String.Empty
        RaiseEvent DeleteClick()
    End Sub
#End Region

#Region "Appearance"
    Public Property Width() As Unit
        Get
            Return ctrl.Width
        End Get
        Set(ByVal value As Unit)
            ctrl.Width = value
        End Set
    End Property
    Public Property NoFileText() As String
        Get
            Return CtrlLocked.Text
        End Get
        Set(ByVal value As String)
            CtrlLocked.Text = value
        End Set
    End Property
#End Region

#Region "Behaviour"
    Public Property FolderPath() As String
        Get
            Return m_folderPath
        End Get
        Set(ByVal value As String)
            m_folderPath = value
        End Set
    End Property
#End Region

#Region "Value"
    Public Property Value() As String
        Get
            Return FileName
        End Get
        Set(ByVal value As String)
            FileName = value
        End Set
    End Property
    Public Property FileName() As String
        Get
            If HasFile Then Return ctrl.PostedFile.FileName

            Dim s As String = lnkLocked.Text
            If String.IsNullOrEmpty(s) Then s = hiddenFileName.Value
            Return s
        End Get
        Set(ByVal value As String)
            lnkLocked.Text = value
            hiddenFileName.Value = value
            Me.Required = IIf(String.IsNullOrEmpty(value), Me.Required, False)
        End Set
    End Property
    Public Property NavigateUrl() As String 'Overrides the direct download path
        Get
            Return lnkLocked.NavigateUrl
        End Get
        Set(ByVal value As String)
            lnkLocked.NavigateUrl = value
        End Set
    End Property

    'Main interface for saving - returns current fileName, or new filename with uniqueness
    Public Function SaveFile() As String
        If Not HasFile() Then Return Me.FileName

        Dim fileName As String = IO.Path.GetFileName(FileUpload.FileName)
        Dim folder As String = FolderPath
        If Not IO.Directory.Exists(folder) AndAlso folder.StartsWith("~") Then folder = Server.MapPath(folder)
        If Not IO.Directory.Exists(folder) Then IO.Directory.CreateDirectory(folder)

        fileName = CUtilities.UniqueFileName(folder, fileName)
        FileUpload.SaveAs(folder & "/" & fileName)
        Return fileName
    End Function

    'Misc visibility helpers (generally not needed)
    Public ReadOnly Property FileUpload() As FileUpload
        Get
            Return ctrl
        End Get
    End Property
    Public ReadOnly Property HasFile() As Boolean
        Get
            Return FileUpload.HasFile
        End Get
    End Property
    Public ReadOnly Property FileBytes() As Byte()
        Get
            Return ctrl.FileBytes
        End Get
    End Property
    Public ReadOnly Property FileContent() As IO.Stream
        Get
            Return ctrl.FileContent
        End Get
    End Property
    Public ReadOnly Property PostedFile() As HttpPostedFile
        Get
            Return FileUpload.PostedFile
        End Get
    End Property
    Public ReadOnly Property ContentType() As String
        Get
            If Not HasFile Then Return String.Empty
            Return PostedFile.ContentType
        End Get
    End Property
    Public ReadOnly Property ContentLength() As String
        Get
            If Not HasFile Then Return String.Empty
            Return PostedFile.ContentLength
        End Get
    End Property
#End Region

#Region "MustOverrides"
    'Logic/Presentation
    Protected Overrides Function GetLockedText() As String
        If Len(FileName) > 0 Then Return FileName
        Return CtrlLocked.Text
    End Function
    Protected Overrides ReadOnly Property DefaultFormatRequiredText() As String
        Get
            Return "'{0}' - browse for a file"
        End Get
    End Property
    Protected Overrides WriteOnly Property SetEnabled() As Boolean
        Set(ByVal value As Boolean)
            ctrl.Enabled = value
        End Set
    End Property
    Public Overrides Property ToolTip() As String
        Get
            Return ctrl.ToolTip
        End Get
        Set(ByVal value As String)
            ctrl.ToolTip = value
            lnkLocked.ToolTip = value
            _l.ToolTip = value
        End Set
    End Property

    'Controls - Active
    Protected Overrides ReadOnly Property CtrlMain() As Control
        Get
            Return ctrl
        End Get
    End Property
    Protected Overrides ReadOnly Property CtrlLabel() As Literal
        Get
            Return litLabel
        End Get
    End Property
    Protected Overrides ReadOnly Property CtrlLocked() As Label
        Get
            Return _l
        End Get
    End Property
    Protected Overrides ReadOnly Property CtrlValidator() As BaseValidator
        Get
            Return rfv
        End Get
    End Property
    Protected Overrides ReadOnly Property CtrlValidatorCustom() As CustomValidator
        Get
            Return cv
        End Get
    End Property
    Protected Overrides ReadOnly Property CtrlValidatorScript() As PlaceHolder
        Get
            Return Nothing
        End Get
    End Property
    Protected Overrides ReadOnly Property CtrlHidden() As System.Web.UI.WebControls.HiddenField
        Get
            Return _h
        End Get
    End Property

    'Controls - Container
    Protected Overrides ReadOnly Property CtrlContainerBegin() As CCustomControlContainer
        Get
            Return _st
        End Get
    End Property
    Protected Overrides ReadOnly Property CtrlContainerEnd() As CCustomControlContainer
        Get
            Return _et
        End Get
    End Property
    Protected Overrides ReadOnly Property CtrlSeparator1() As CCustomControlContainer
        Get
            Return _s1
        End Get
    End Property
    Protected Overrides ReadOnly Property CtrlSeparator2() As CCustomControlContainer
        Get
            Return _s2
        End Get
    End Property
#End Region

End Class
