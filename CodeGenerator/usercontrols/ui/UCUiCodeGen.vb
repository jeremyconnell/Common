Public Class UCUiCodeGen

#Region "Enums"
    Private Enum ETemplate
        Page_ListSearch
        Page_AddEdit
        UserControl_Item
        UserControl_Container
        Menu_SitemapUrls
    End Enum
#End Region

#Region "Members"
    Private m_info As CMainLogic
    Private m_table As CTable
    Private m_metadata As CMetadata
    Private m_tempRbName1 As String
    Private m_tempRbName2 As String
#End Region

#Region "Events"
    Public Event FolderChanged()
#End Region

#Region "Interface - Display"
    Public Sub Display(ByVal metadata As CMetadata, ByVal table As CTable, ByVal info As CMainLogic)
        'Store info
        m_table = table
        m_info = info
        m_metadata = metadata

        'Enable/Disable
        Me.Enabled = False
        If IsNothing(table) Then Exit Sub
        Me.Enabled = True


        'Show columns, unselect pks
        Try
            With lvHtmlColumns.Items
                .Clear()
                For Each s As String In table.ColumnNames
                    Dim item As New ListViewItem(s)
                    Select Case .Count
                        Case 0 : item.Checked = False
                        Case 1 : item.Checked = Not table.IsAssociative
                        Case 2 : item.Checked = Not table.Is3Way
                        Case Else : item.Checked = True
                    End Select
                    .Add(item)
                Next
            End With
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "Invalid Table Name")
        End Try

        'Hyperlink Column
        With cboHyperlink
            .DataSource = table.ColumnNames
            If .Items.Count > 0 Then .SelectedIndex = 0
            For Each i As String In table.ColumnNames
                If i.ToLower.EndsWith("name") Then
                    .SelectedItem = i
                    Exit For
                End If
            Next
        End With

        'Different layout for special case
        If SpecialCase_Associative Then
            If rbList.Text <> "UserControl - Pair" Then
                'Rename two of the radio buttons
                m_tempRbName1 = rbList.Text
                m_tempRbName2 = rbUrls.Text
                rbList.Text = "UserControl - Pair"
                rbUrls.Text = "Deployment Instructions/Urls"
            End If
            rbDetails.Enabled = m_table.ColumnNames.Count > 2
            gboxPrimaryKeyTable.Visible = True
            gboxSecondaryKeyTable.Visible = True
            gboxHyperlink.Enabled = False
            gboxColumns.Enabled = False
            cboPrimary.DataSource = metadata.Clone()
            cboSecondary.DataSource = metadata.Clone()
            For Each i As CTable In CType(cboPrimary.DataSource, IList)
                If i.IsAssociative Or i.Is3Way Then Continue For
                If m_table.PrimaryKeyName.ToLower.EndsWith(i.PrimaryKeyName.ToLower) Then
                    cboPrimary.SelectedItem = i
                    Exit For
                End If
            Next
            For Each i As CTable In CType(cboSecondary.DataSource, IList)
                If i.IsAssociative Or i.Is3Way Then Continue For
                If m_table.SecondaryKeyName.ToLower.EndsWith(i.PrimaryKeyName.ToLower) Then
                    cboSecondary.SelectedItem = i
                    Exit For
                End If
            Next

            rbEditable.Checked = True
            rbReadonly.Enabled = False
        Else
            If Not IsNothing(m_tempRbName1) Then rbList.Text = m_tempRbName1
            If Not IsNothing(m_tempRbName2) Then rbUrls.Text = m_tempRbName2
            gboxPrimaryKeyTable.Visible = False
            gboxSecondaryKeyTable.Visible = False
            gboxHyperlink.Enabled = True
            gboxColumns.Enabled = True
            rbDetails.Enabled = True
            rbReadonly.Enabled = True
        End If

        'Display
        rb_CheckedChanged(Nothing, Nothing)
    End Sub
#End Region

#Region "Special UI - Associative tables"
    Public ReadOnly Property SpecialCase_Associative() As Boolean
        Get
            Return m_info.IsManyToMany
        End Get
    End Property

    'Use naming convention to guess related table (should end with PK name)
    Public ReadOnly Property Primary() As CTable
        Get
            Return CType(cboPrimary.SelectedItem, CTable)
        End Get
    End Property
    Public ReadOnly Property Secondary() As CTable
        Get
            Return CType(cboSecondary.SelectedItem, CTable)
        End Get
    End Property
#End Region

#Region "Form"
    Private Property OutputFolder() As String
        Get
            If IsEdit Then
                Return OutputFolderEditable
            Else
                Return OutputFolderReadOnly
            End If
        End Get
        Set(ByVal value As String)
            If IsEdit Then
                OutputFolderEditable = value
            Else
                OutputFolderReadOnly = value
            End If
            RaiseEvent FolderChanged()
        End Set
    End Property
    Public Property OutputFolderReadOnly() As String
        Get
            Return txtFolderReadonly.Text
        End Get
        Set(ByVal value As String)
            txtFolderReadonly.Text = value
        End Set
    End Property
    Public Property OutputFolderEditable() As String
        Get
            Return txtFolderEditable.Text
        End Get
        Set(ByVal value As String)
            txtFolderEditable.Text = value
        End Set
    End Property
    Private Property Template() As ETemplate
        Get
            If rbDetails.Checked Then Return ETemplate.Page_AddEdit
            If rbList.Checked Then Return ETemplate.Page_ListSearch
            If rbListItem.Checked Then Return ETemplate.UserControl_Item
            If rbContainer.Checked Then Return ETemplate.UserControl_Container
            If rbUrls.Checked Then Return ETemplate.Menu_SitemapUrls
            Throw New Exception()
        End Get
        Set(ByVal value As ETemplate)
            Select Case value
                Case ETemplate.Page_AddEdit : rbDetails.Checked = True
                Case ETemplate.Page_ListSearch : rbList.Checked = True
                Case ETemplate.UserControl_Item : rbListItem.Checked = True
                Case ETemplate.UserControl_Container : rbContainer.Checked = True
                Case ETemplate.Menu_SitemapUrls : rbUrls.Checked = True
                Case Else : Throw New Exception("Unrecognised template radio button: " & CInt(value).ToString)
            End Select
        End Set
    End Property
    Private ReadOnly Property IsEdit() As Boolean
        Get
            Return rbEditable.Checked
        End Get
    End Property
    Public ReadOnly Property HyperlinkColumn() As String
        Get
            Return cboHyperlink.Text
        End Get
    End Property
    Public ReadOnly Property Selected() As String()
        Get
            With lvHtmlColumns
                Dim s As New List(Of String)(.CheckedItems.Count)
                For Each i As ListViewItem In .CheckedItems
                    s.Add(i.Text)
                Next
                Return s.ToArray
            End With
        End Get
    End Property
    Public ReadOnly Property Selected_HyperlinkColFirst() As String()
        Get
            If String.IsNullOrEmpty(HyperlinkColumn) Then Return Selected
            Dim temp As New List(Of String)(Selected)
            If Not temp.Contains(HyperlinkColumn) Then Return Selected
            temp.Remove(HyperlinkColumn)
            temp.Insert(0, HyperlinkColumn)
            Return temp.ToArray
        End Get
    End Property
#End Region

#Region "Event Handlers"
    'Generation
    Private Sub rb_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbDetails.CheckedChanged, rbEditable.CheckedChanged, rbList.CheckedChanged, rbListItem.CheckedChanged, rbReadonly.CheckedChanged, rbContainer.CheckedChanged
        txtFolderEditable.Visible = IsEdit
        txtFolderReadonly.Visible = Not IsEdit
        Generate()
    End Sub
    Private Sub lvHtmlColumns_ItemChecked(ByVal sender As Object, ByVal e As EventArgs) Handles lvHtmlColumns.ItemChecked
        Generate()
    End Sub
    Private Sub cboPrimary_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPrimary.SelectedIndexChanged
        Generate()
    End Sub
    Private Sub cboSecondary_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSecondary.SelectedIndexChanged
        Generate()
    End Sub
    Private Sub cboHyperlink_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboHyperlink.SelectedIndexChanged
        Generate()
    End Sub
    Private Sub btnWriteFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWriteFile.Click
        WriteFiles()
    End Sub

    'Config
    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        With FolderBrowserDialog1
            .SelectedPath = OutputFolder
            If .ShowDialog() <> DialogResult.OK Then Exit Sub
            OutputFolder = .SelectedPath
        End With
    End Sub
#End Region

#Region "Private"
    Private Sub Generate()
        If m_info Is Nothing Then Exit Sub

        DisplayFileNames()
        DisplayFileContents()
    End Sub
    Private Sub DisplayFileNames()
        With tabCtrlOutput.TabPages
            'Search-Page name: default.aspx vs plural.aspx
            Dim name As String = String.Empty
            Select Case Template
                Case ETemplate.UserControl_Container
                    name = m_table.Plural
                Case ETemplate.Page_AddEdit, ETemplate.UserControl_Item
                    name = m_table.Singular
                Case ETemplate.Page_ListSearch
                    name = CStr(IIf(IsEdit And m_table.IsAssociative, m_table.Plural, "default"))
            End Select

            'Subfolders and file extensions
            Dim subfolder As String = New CTemplate(CStr(IIf(IsEdit, "subfolder_editable.txt", "subfolder_readonly.txt")), m_info.TemplateFolderWeb).Template
            Dim prefix As String = String.Concat(subfolder, m_table.PluralCamelCase, "/")
            Dim suffix As String = ".aspx"
            Select Case Template
                Case ETemplate.UserControl_Container, ETemplate.UserControl_Item
                    prefix = String.Concat(prefix, "usercontrols/UC")
                    suffix = ".ascx"
                Case ETemplate.Page_ListSearch
                    If SpecialCase_Associative Then
                        prefix = String.Concat(prefix, "usercontrols/UC")
                        suffix = "Pair.ascx"
                    End If
            End Select

            'Display page/usercontrol
            .Item(0).Text = String.Concat(prefix, name, suffix)
            .Item(1).Text = String.Concat(prefix, name, suffix, m_info.FileExtension)

            'Display urls/simtemap
            If Template = ETemplate.Menu_SitemapUrls Then
                Dim templateFolder As String = CStr(IIf(IsEdit, m_info.TemplateFolderWebUrlsEditable, m_info.TemplateFolderWebUrlsReadOnly))
                .Item(0).Text = New CTemplate("sitemap_path.txt", templateFolder).Template
                .Item(1).Text = New CTemplate("urls_path.txt", templateFolder).Template

                If SpecialCase_Associative Then
                    Dim t As New CTemplate("sitemap_path.txt", templateFolder & "associative/")
                    If Not IsNothing(Primary) Then t.Replace("SingularLhs", Primary.Singular)
                    If Not IsNothing(Secondary) Then t.Replace("SingularRhs", Secondary.Singular)
                    .Item(0).Text = t.Template
                End If
            End If
        End With
    End Sub
    Private Sub DisplayFileContents()
        Select Case Template
            Case ETemplate.Page_AddEdit
                Dim folder As String = m_info.TemplateFolderWebDetailsReadOnly
                If IsEdit Then folder = m_info.TemplateFolderWebDetailsEditable
                ShowTemplate_Details(folder)

            Case ETemplate.Page_ListSearch
                Dim folder As String = m_info.TemplateFolderWebListReadOnly
                If IsEdit Then folder = m_info.TemplateFolderWebListEditable
                ShowTemplate_List(folder)

            Case ETemplate.UserControl_Item
                Dim folder As String = m_info.TemplateFolderWebListItemReadOnly
                If IsEdit Then folder = m_info.TemplateFolderWebListItemEditable
                ShowTemplate_ListItem(folder)

            Case ETemplate.UserControl_Container
                Dim folder As String = m_info.TemplateFolderWebContainerReadOnly
                If IsEdit Then folder = m_info.TemplateFolderWebContainerEditable
                ShowTemplate_Container(folder)

            Case ETemplate.Menu_SitemapUrls
                Dim folder As String = m_info.TemplateFolderWebUrlsReadOnly
                If IsEdit Then folder = m_info.TemplateFolderWebUrlsEditable
                ShowTemplate_Urls(folder)
        End Select
    End Sub
#End Region

#Region "Details"
    Private Sub ShowTemplate_Details(ByVal folderPath As String)
        'Aspx
        Try
            With New CTemplate("aspx.txt", folderPath)
                .Replace("items", ShowTemplate_Details_Aspx(folderPath))
                .Replace("Singular", m_table.Singular)
                .Replace("SingularCamelCase", m_table.SingularCamelCase)
                .Replace("Plural", m_table.Plural)
                txtAspx.Text = .Template
            End With
        Catch ex As Exception
            txtAspx.Text = ex.ToString
        End Try

        'CodeBehind
        Try
            Dim code As CTemplate
            If SpecialCase_Associative Then
                code = New CTemplate("CodeBehind.txt", folderPath & "associative/")
            Else
                code = New CTemplate("CodeBehind.txt", folderPath)
            End If
            code.Replace("Display", ShowTemplate_Details_Display(folderPath))
            If IsEdit Then
                code.Replace("InitKeys", ShowTemplate_Details_InitKeys())
                code.Replace("EventHandlers", ShowTemplate_Details_EventHandlers())
                code.Replace("Store", ShowTemplate_Details_Store(folderPath))
                code.Replace("DisplayBulk", ShowTemplate_Details_DisplayBulk(folderPath))
                code.Replace("StoreBulk", ShowTemplate_Details_StoreBulk(folderPath))
                code.Replace("IsEdit", ShowTemplate_Details_IsEdit(folderPath))
            End If

            code.Replace("Data", ShowTemplate_Details_Data(folderPath))
            code.Replace("ClearCache", ShowTemplate_Details_ClearCache())
            With m_table
                code.Replace("Singular", .Singular)
                code.Replace("SingularCamelCase", .SingularCamelCase)
                code.Replace("Plural", .Plural)
                code.Replace("PluralCamelCase", .PluralCamelCase)
                code.Replace("ClassName", .ClassName)
                code.Replace("PrimaryKey", .PrimaryKeyName)
                code.Replace("PrimaryKeyCamelCase", .PrimaryKeyCamelCase)
                code.Replace("PrimaryKeyType", .PrimaryKeyTypeName(m_info.Language))
                code.Replace("PrimaryKeyTypeShort", m_info.FunctionName(.PrimaryKeyType).Replace("Get", String.Empty))

                If .IsAssociative Then
                    code.Replace("SecondaryKey", .SecondaryKeyName)
                    code.Replace("SecondaryKeyCamelCase", .SecondaryKeyCamelCase)
                    code.Replace("SecondaryKeyType", .SecondaryKeyTypeName(m_info.Language))
                    code.Replace("SecondaryKeyTypeShort", m_info.FunctionName(.SecondaryKeyType).Replace("Get", String.Empty))
                    code.Replace("Primary", Primary.Singular)
                    code.Replace("Secondary", Secondary.Singular)
                End If

                code.Replace("Namespace", m_info.CSharpNamespace)
            End With

            'Display aspx
            txtCodeBehind.Text = code.Template
        Catch ex As Exception
            txtCodeBehind.Text = ex.ToString
        End Try
    End Sub

    'Aspx
    Private Function ShowTemplate_Details_Aspx(ByVal folderPath As String) As String
        Dim basic As New CTemplate("aspx_basic.txt", folderPath)
        Dim bool As New CTemplate("aspx_bool.txt", folderPath)
        Dim dateTime As New CTemplate("aspx_date.txt", folderPath)
        Dim image As New CTemplate("aspx_image.txt", folderPath)
        Dim file As New CTemplate("aspx_file.txt", folderPath)
        Dim int As New CTemplate("aspx_int.txt", folderPath)
        Dim dec As New CTemplate("aspx_dec.txt", folderPath)
        Dim dbl As New CTemplate("aspx_dbl.txt", folderPath)
        Dim key As New CTemplate("aspx_key.txt", folderPath)

        Dim sb As New StringBuilder()
        For Each i As String In Selected_HyperlinkColFirst
            If i = m_table.SortingColumn Then Continue For

            If IsForeignKey(i) Then
                key.Reset()
                key.Replace("Entity", GuessFkEntity(i))
                Template_ReplaceName(sb, key, i, False)
                Continue For
            End If

            Select Case m_table.GetColumnType(i).ToString
                Case GetType(Boolean).ToString : Template_ReplaceName(sb, bool, i)
                Case GetType(DateTime).ToString : Template_ReplaceName(sb, dateTime, i)
                Case GetType(Date).ToString : Template_ReplaceName(sb, dateTime, i)
                Case GetType(Decimal).ToString : Template_ReplaceName(sb, dec, i)
                Case GetType(Double).ToString : Template_ReplaceName(sb, dbl, i)
                Case GetType(Single).ToString : Template_ReplaceName(sb, dbl, i)
                Case GetType(Integer).ToString : Template_ReplaceName(sb, int, i)
                Case GetType(Long).ToString : Template_ReplaceName(sb, int, i)
                Case Else
                    If IsImage(i) Then
                        Template_ReplaceName(sb, image, i)
                    ElseIf IsFile(i) Then
                        Template_ReplaceName(sb, file, i)
                    Else
                        basic.Reset()
                        basic.Replace("DataType", LupoDataType(i)) 'Email, phone
                        Template_ReplaceName(sb, basic, i, False)
                    End If
            End Select
        Next
        Return sb.ToString
    End Function

    'Code behind
    Private Function ShowTemplate_Details_Display(ByVal folderPath As String) As String
        Dim basic As New CTemplate("display.txt", folderPath)
        Dim bool As New CTemplate("display_bool.txt", folderPath)
        Dim dateTime As New CTemplate("display_date.txt", folderPath)
        Dim image As New CTemplate("display_image.txt", folderPath)
        Dim file As New CTemplate("display_file.txt", folderPath)
        Dim int As New CTemplate("display_int.txt", folderPath)
        Dim [long] As New CTemplate("display_long.txt", folderPath)
        Dim dec As New CTemplate("display_dec.txt", folderPath)
        Dim dbl As New CTemplate("display_dbl.txt", folderPath)

        Dim key As CTemplate
        Dim keyint As CTemplate
        Dim keystr As CTemplate
        If IsEdit Then
            keyint = New CTemplate("display_key_int.txt", folderPath)
            keystr = New CTemplate("display_key_str.txt", folderPath)
        Else
            key = New CTemplate("display_key.txt", folderPath)
            keyint = key
            keystr = key
        End If

        Dim sb As New StringBuilder()
        For Each i As String In Selected_HyperlinkColFirst
            If i = m_table.SortingColumn Then Continue For
            If IsForeignKey(i) Then
                key = CType(IIf(m_table.GetColumnType(i) Is GetType(String), keystr, keyint), CTemplate)
                key.Reset()
                key.Replace("Entity", GuessFkEntity(i))
                Template_ReplaceName(sb, key, i, False)
                Continue For
            End If
            Select Case m_table.GetColumnType(i).ToString
                Case GetType(Boolean).ToString : Template_ReplaceName(sb, bool, i)
                Case GetType(DateTime).ToString : Template_ReplaceName(sb, dateTime, i)
                Case GetType(Date).ToString : Template_ReplaceName(sb, dateTime, i)
                Case GetType(Decimal).ToString : Template_ReplaceName(sb, dec, i)
                Case GetType(Double).ToString : Template_ReplaceName(sb, dbl, i)
                Case GetType(Single).ToString : Template_ReplaceName(sb, dbl, i)
                Case GetType(Integer).ToString : Template_ReplaceName(sb, int, i)
                Case GetType(Long).ToString : Template_ReplaceName(sb, [long], i)
                Case Else
                    If IsImage(i) Then
                        Template_ReplaceName(sb, image, i)
                    ElseIf IsFile(i) Then
                        Template_ReplaceName(sb, file, i)
                    Else
                        Template_ReplaceName(sb, basic, i)
                    End If
            End Select
        Next
        Return sb.ToString
    End Function
    Private Function ShowTemplate_Details_DisplayBulk(ByVal folderPath As String) As String
        Dim basic As New CTemplate("displaybulk.txt", folderPath)
        Dim bool As New CTemplate("displaybulk_bool.txt", folderPath)
        Dim dateTime As New CTemplate("displaybulk_date.txt", folderPath)
        Dim image As New CTemplate("displaybulk_image.txt", folderPath)
        Dim file As New CTemplate("displaybulk_file.txt", folderPath)
        Dim int As New CTemplate("displaybulk_int.txt", folderPath)
        Dim dec As New CTemplate("displaybulk_dec.txt", folderPath)
        Dim dbl As New CTemplate("displaybulk_dbl.txt", folderPath)
        Dim key As CTemplate
        Dim keyint As CTemplate = New CTemplate("displaybulk_key_int.txt", folderPath)
        Dim keystr As CTemplate = New CTemplate("displaybulk_key_str.txt", folderPath)

        Dim sb As New StringBuilder()
        For Each i As String In Selected_HyperlinkColFirst
            If i = m_table.SortingColumn Then Continue For
            If IsForeignKey(i) Then
                key = CType(IIf(m_table.GetColumnType(i) Is GetType(String), keystr, keyint), CTemplate)
                key.Reset()
                key.Replace("Entity", GuessFkEntity(i))
                Template_ReplaceName(sb, key, i, False)
                Continue For
            End If
            Select Case m_table.GetColumnType(i).ToString
                Case GetType(Boolean).ToString : Template_ReplaceName(sb, bool, i)
                Case GetType(DateTime).ToString : Template_ReplaceName(sb, dateTime, i)
                Case GetType(Date).ToString : Template_ReplaceName(sb, dateTime, i)
                Case GetType(Decimal).ToString : Template_ReplaceName(sb, dec, i)
                Case GetType(Double).ToString : Template_ReplaceName(sb, dbl, i)
                Case GetType(Single).ToString : Template_ReplaceName(sb, dbl, i)
                Case GetType(Integer).ToString : Template_ReplaceName(sb, int, i)
                Case Else
                    If IsImage(i) Then
                        Template_ReplaceName(sb, image, i)
                    ElseIf IsFile(i) Then
                        Template_ReplaceName(sb, file, i)
                    Else
                        Template_ReplaceName(sb, basic, i)
                    End If
            End Select
        Next
        Return sb.ToString
    End Function
    Private Function ShowTemplate_Details_InitKeys() As String
        Dim init As New CTemplate("codebehind_key.txt", m_info.TemplateFolderWebDetailsEditable)
        Dim sb As New StringBuilder()
        For Each i As String In Selected
            If Not IsForeignKey(i) Then Continue For
            init.Reset()
            init.Replace("Entity", GuessFkEntity(i))
            init.Replace("Name", i)
            sb.Append(init.Template)
        Next

        Dim image As New CTemplate("codebehind_image.txt", m_info.TemplateFolderWebDetailsEditable)
        Dim file As New CTemplate("codebehind_file.txt", m_info.TemplateFolderWebDetailsEditable)
        For Each i As String In Selected
            If IsImage(i) Then
                Template_ReplaceName(sb, image, i)
            ElseIf IsFile(i) Then
                Template_ReplaceName(sb, file, i)
            End If
        Next
        Return sb.ToString
    End Function
    Private Function ShowTemplate_Details_EventHandlers() As String
        Dim sb As New StringBuilder()
        Dim image As New CTemplate("eventhandler_image.txt", m_info.TemplateFolderWebDetailsEditable)
        Dim file As New CTemplate("eventhandler_file.txt", m_info.TemplateFolderWebDetailsEditable)
        For Each i As String In Selected
            If IsImage(i) Then
                Template_ReplaceName(sb, image, i)
            ElseIf IsFile(i) Then
                Template_ReplaceName(sb, file, i)
            End If
        Next
        Return sb.ToString
    End Function

    Private Function ShowTemplate_Details_Store(ByVal folderPath As String) As String
        Dim basic As New CTemplate("store.txt", folderPath)
        Dim bool As New CTemplate("store_bool.txt", folderPath)
        Dim dateTime As New CTemplate("store_date.txt", folderPath)
        Dim image As New CTemplate("store_image.txt", folderPath)
        Dim file As New CTemplate("store_file.txt", folderPath)
        Dim int As New CTemplate("store_int.txt", folderPath)
        Dim lng As New CTemplate("store_long.txt", folderPath)
        Dim dec As New CTemplate("store_dec.txt", folderPath)
        Dim dbl As New CTemplate("store_dbl.txt", folderPath)

        Dim key As CTemplate
        Dim keyint As CTemplate
        Dim keystr As CTemplate
        If IsEdit Then
            keyint = New CTemplate("store_key_int.txt", folderPath)
            keystr = New CTemplate("store_key_str.txt", folderPath)
        Else
            key = New CTemplate("store_key.txt", folderPath)
            keyint = key
            keystr = key
        End If

        Dim sb As New StringBuilder()
        For Each i As String In Selected_HyperlinkColFirst
            If i = m_table.SortingColumn Then Continue For
            If IsForeignKey(i) Then
                key = CType(IIf(m_table.GetColumnType(i) Is GetType(String), keystr, keyint), CTemplate)
                key.Reset()
                key.Replace("Entity", GuessFkEntity(i))
                Template_ReplaceName(sb, key, i)
                Continue For
            End If
            Select Case m_table.GetColumnType(i).ToString
                Case GetType(Boolean).ToString : Template_ReplaceName(sb, bool, i)
                Case GetType(DateTime).ToString : Template_ReplaceName(sb, dateTime, i)
                Case GetType(Date).ToString : Template_ReplaceName(sb, dateTime, i)
                Case GetType(Decimal).ToString : Template_ReplaceName(sb, dec, i)
                Case GetType(Double).ToString : Template_ReplaceName(sb, dbl, i)
                Case GetType(Single).ToString : Template_ReplaceName(sb, dbl, i)
                Case GetType(Integer).ToString : Template_ReplaceName(sb, int, i)
                Case GetType(Long).ToString : Template_ReplaceName(sb, lng, i)
                Case Else
                    If IsImage(i) Then
                        Template_ReplaceName(sb, image, i)
                    ElseIf IsFile(i) Then
                        Template_ReplaceName(sb, file, i)
                    Else
                        Template_ReplaceName(sb, basic, i)
                    End If
            End Select
        Next
        Return sb.ToString
    End Function
    Private Function ShowTemplate_Details_StoreBulk(ByVal folderPath As String) As String
        Dim basic As New CTemplate("storebulk.txt", folderPath)
        Dim bool As New CTemplate("storebulk_bool.txt", folderPath)
        Dim dateTime As New CTemplate("storebulk_date.txt", folderPath)
        Dim image As New CTemplate("storebulk_image.txt", folderPath)
        Dim file As New CTemplate("storebulk_file.txt", folderPath)
        Dim int As New CTemplate("storebulk_int.txt", folderPath)
        Dim lng As New CTemplate("storebulk_long.txt", folderPath)
        Dim dec As New CTemplate("storebulk_dec.txt", folderPath)
        Dim dbl As New CTemplate("storebulk_dbl.txt", folderPath)
        Dim key As CTemplate
        Dim keyint As CTemplate = New CTemplate("storebulk_key_int.txt", folderPath)
        Dim keystr As CTemplate = New CTemplate("storebulk_key_str.txt", folderPath)

        Dim sb As New StringBuilder()
        For Each i As String In Selected_HyperlinkColFirst
            If i = m_table.SortingColumn Then Continue For
            If IsForeignKey(i) Then
                key = CType(IIf(m_table.GetColumnType(i) Is GetType(String), keystr, keyint), CTemplate)
                key.Reset()
                key.Replace("Entity", GuessFkEntity(i))
                Template_ReplaceName(sb, key, i)
                Continue For
            End If
            Select Case m_table.GetColumnType(i).ToString
                Case GetType(Boolean).ToString : Template_ReplaceName(sb, bool, i)
                Case GetType(DateTime).ToString : Template_ReplaceName(sb, dateTime, i)
                Case GetType(Date).ToString : Template_ReplaceName(sb, dateTime, i)
                Case GetType(Decimal).ToString : Template_ReplaceName(sb, dec, i)
                Case GetType(Double).ToString : Template_ReplaceName(sb, dbl, i)
                Case GetType(Single).ToString : Template_ReplaceName(sb, dbl, i)
                Case GetType(Integer).ToString : Template_ReplaceName(sb, int, i)
                Case GetType(Long).ToString : Template_ReplaceName(sb, lng, i)
                Case Else
                    If IsImage(i) Then
                        Template_ReplaceName(sb, image, i)
                    ElseIf IsFile(i) Then
                        Template_ReplaceName(sb, file, i)
                    Else
                        Template_ReplaceName(sb, basic, i)
                    End If
            End Select
        Next
        Return sb.ToString
    End Function
    Private Function ShowTemplate_Details_Data(ByVal folderPath As String) As String
        If SpecialCase_Associative Then folderPath &= "associative/"

        Dim fileName As String = CStr(IIf(m_table.UseCaching, "data_cached.txt", "data_notcached.txt"))
        With New CTemplate(fileName, folderPath)
            .Replace("FirstLetter", m_table.SingularCamelCase.Substring(0, 1))
            Return .Template
        End With
    End Function
    Private Function ShowTemplate_Details_IsEdit(ByVal folderPath As String) As String
        Select Case m_table.PrimaryKeyType.ToString
            Case GetType(String).ToString
                Return New CTemplate("IsEdit_String.txt", folderPath).Template
            Case GetType(Guid).ToString
                Return New CTemplate("IsEdit_Guid.txt", folderPath).Template
            Case Else
                Return New CTemplate("IsEdit_int.txt", folderPath).Template
        End Select
    End Function
    Private Function ShowTemplate_Details_ClearCache() As String
        If Not m_table.UseCaching Then Return String.Empty
        If Not IsEdit Then Return String.Empty
        With New CTemplate("clearcache.txt", m_info.TemplateFolderWebDetailsEditable)
            Return .Template
        End With
    End Function

    'Helpers
    Private Function LupoDataType(ByVal colName As String) As String
        With colName.ToLower
            If .Contains("email") Then Return "Email"
            If .Contains("phone") OrElse .Contains("mobile") Then Return "PhoneNumber"
            Return "String"
        End With
    End Function
    Private Function IsFile(ByVal column As String) As Boolean
        With column.ToLower
            If .Contains("pdf") Then Return True
            If .EndsWith("path") Then Return True
        End With
        'Possible entity name (might match prefix for all columns)
        With m_table.GetShortName(column).ToLower
            If .Contains("file") Then Return True
        End With
        Return False
    End Function
    Private Function IsImage(ByVal column As String) As Boolean
        With column.ToLower
            If .Contains("thumb") Then Return True
            If .Contains("icon") Then Return True
            If .Contains("photo") Then Return True
        End With
        'Possible entity name (might match prefix for all columns)
        With m_table.GetShortName(column).ToLower
            If .Contains("image") Then Return True
        End With
        Return False
    End Function
    Private Function IsForeignKey(ByVal column As String) As Boolean
        If column = m_table.PrimaryKeyName Then Return False
        With column.ToLower
            If .EndsWith("id") OrElse .EndsWith("_fk") OrElse .EndsWith("fk") Then Return True
        End With
        Return False
    End Function
    Private Function GuessFkEntity(ByVal column As String) As String
        Dim shorter As String = m_table.GetShortName(column)
        With shorter.ToLower
            If .EndsWith("id") Then Return shorter.Substring(0, shorter.Length - 2)
            If .EndsWith("_fk") Then Return shorter.Substring(0, shorter.Length - 3)
            If .EndsWith("fk") Then Return shorter.Substring(0, shorter.Length - 2)
        End With
        Return shorter
    End Function
#End Region

#Region "Associative"
    Private Sub ShowTemplate_SpecialCaseAssociative(ByVal folderPath As String)
        'Special folder
        folderPath &= "associative/"

        'Aspx
        Try
            With New CTemplate("aspx.txt", folderPath)
                .Replace("Singular", m_table.Singular)
                .Replace("Plural", m_table.Plural)
                txtAspx.Text = .Template
            End With
        Catch ex As Exception
            txtAspx.Text = ex.ToString
        End Try

        'CodeBehind
        Try
            With New CTemplate("CodeBehind.txt", folderPath)
                If folderPath.ToLower.Contains("container") Then
                    Dim lhs As New CTemplate(CStr(IIf(Primary.UseCaching, "", "Not")) & "Cached_lhs.txt", folderPath & "codebehind/")
                    Dim rhs As New CTemplate(CStr(IIf(Secondary.UseCaching, "", "Not")) & "Cached_rhs.txt", folderPath & "codebehind/")
                    .Replace("Lhs", lhs.Template)
                    .Replace("Rhs", rhs.Template)
                End If
                .Replace("Singular", m_table.Singular)
                .Replace("Plural", m_table.Plural)
                .Replace("ClassName", m_table.ClassName)
                .Replace("Namespace", m_info.CSharpNamespace)
                .Replace("PrimaryKey", m_table.PrimaryKeyName)
                .Replace("SecondaryKey", m_table.SecondaryKeyName)

                If Not IsNothing(Primary) Then
                    .Replace("ClassNameLhs", Primary.ClassName)
                    .Replace("SingularLhs", Primary.Singular)
                    .Replace("PluralLhs", Primary.Plural)
                    .Replace("PrimaryKeyLhs", Primary.PrimaryKeyName)
                    .Replace("PrimaryKeyCamelCaseLhs", Primary.PrimaryKeyCamelCase)
                    .Replace("PrimaryKeyTypeLhs", Primary.PrimaryKeyTypeName(m_info.Language))
                    .Replace("SingularCamelCaseLhs", Primary.SingularCamelCase)
                    .Replace("PluralCamelCaseLhs", Primary.PluralCamelCase)
                End If
                If Not IsNothing(Secondary) Then
                    .Replace("ClassNameRhs", Secondary.ClassName)
                    .Replace("SingularRhs", Secondary.Singular)
                    .Replace("PluralRhs", Secondary.Plural)
                    .Replace("PrimaryKeyRhs", Secondary.PrimaryKeyName)
                    .Replace("PrimaryKeyCamelCaseRhs", Secondary.PrimaryKeyCamelCase)
                    .Replace("PrimaryKeyTypeRhs", Secondary.PrimaryKeyTypeName(m_info.Language))
                    .Replace("SingularCamelCaseRhs", Secondary.SingularCamelCase)
                    .Replace("PluralCamelCaseRhs", Secondary.PluralCamelCase)
                End If

                txtCodeBehind.Text = .Template
            End With
        Catch ex As Exception
            txtCodeBehind.Text = ex.ToString
        End Try
    End Sub
#End Region

#Region "List"
    Private Sub ShowTemplate_List(ByVal folderPath As String)
        'Alternative representation
        If SpecialCase_Associative Then
            ShowTemplate_SpecialCaseAssociative(folderPath)
            Exit Sub
        End If

        'Aspx
        Try
            With New CTemplate("aspx.txt", folderPath)
                .Replace("Plural", m_table.Plural)
                .Replace("Singular", m_table.Singular)
                .Replace("PluralCamelCase", m_table.PluralCamelCase)
                txtAspx.Text = .Template
            End With
        Catch ex As Exception
            txtAspx.Text = ex.ToString
        End Try

        'CodeBehind
        Try
            With New CTemplate(CStr(IIf(m_table.UseCaching, "codebehind_cached.txt", "codebehind_notcached.txt")), folderPath)
                .Replace("ClassName", m_table.ClassName)
                .Replace("Singular", m_table.Singular)
                .Replace("Plural", m_table.Plural)
                .Replace("PluralCamelCase", m_table.PluralCamelCase)
                .Replace("SingularCamelCase", m_table.SingularCamelCase)
                .Replace("Namespace", m_info.CSharpNamespace)
                txtCodeBehind.Text = .Template
            End With
        Catch ex As Exception
            txtCodeBehind.Text = ex.ToString
        End Try
    End Sub
#End Region

#Region "Container"
    Private Sub ShowTemplate_Container(ByVal folderPath As String)
        If SpecialCase_Associative Then
            ShowTemplate_SpecialCaseAssociative(folderPath)
            Exit Sub
        End If

        'Aspx
        Try
            With New CTemplate("aspx.txt", folderPath)
                .Replace("HeaderColumns", ShowTemplate_Container_HeaderCols(folderPath))
                .Replace("Singular", m_table.Singular)
                .Replace("Plural", m_table.Plural)
                .Replace("SingularCamelCase", m_table.SingularCamelCase)
                .Replace("PluralCamelCase", m_table.PluralCamelCase)

                txtAspx.Text = .Template
            End With
        Catch ex As Exception
            txtAspx.Text = ex.ToString
        End Try

        'CodeBehind
        Try
            Dim fileName As String = CStr(IIf(m_table.UseCaching, "codebehind_cached.txt", "codebehind_notcached.txt"))
            With New CTemplate(fileName, folderPath)
                .Replace("Namespace", m_info.CSharpNamespace)
                .Replace("ClassName", m_table.ClassName)
                .Replace("Singular", m_table.Singular)
                .Replace("SingularCamelCase", m_table.SingularCamelCase)
                .Replace("Plural", m_table.Plural)
                .Replace("PluralCamelCase", m_table.PluralCamelCase)
                .Replace("PluralLowerCase", m_table.Plural.ToLower())
                .Replace("HideUpDown", ShowTemplate_Container_HideUpDownCode(folderPath))
                .Replace("ResortToAlphaEventHandler", ShowTemplate_Container_ResortToAlphaEventHandler(folderPath))
                If m_info.Language = ELanguage.VbNet Then
                    .Replace("SortButtonEvents", ShowTemplate_Container_SortButtonEvents(folderPath))
                End If

                txtCodeBehind.Text = .Template
            End With
        Catch ex As Exception
            txtCodeBehind.Text = ex.ToString
        End Try
    End Sub
    Private Function ShowTemplate_Container_HeaderCols(ByVal folderPath As String) As String
        Dim t As New CTemplate("aspx_headerColumn.txt", folderPath)
        Dim t2 As CTemplate = Nothing
        If IsEdit Then t2 = New CTemplate("aspx_headerColumn_upDown.txt", folderPath)

        Dim sb As New StringBuilder()
        For Each i As String In Selected_HyperlinkColFirst
            If i = m_table.SortingColumn Then
                'Sorting column (up/down arrows)
                If IsEdit Then sb.Append(t2.Template)
            ElseIf i = HyperlinkColumn Then
                Template_ReplaceNameAndExpression(sb, t, m_table.Singular, i)
            Else
                'Regular header columns
                If IsForeignKey(i) Then
                    Template_ReplaceNameAndExpression(sb, t, GuessFkEntity(i), i)
                Else
                    Template_ReplaceNameAndExpression(sb, t, i, i)
                End If
            End If
        Next
        Return sb.ToString
    End Function
    Private Function ShowTemplate_Container_SortButtonEvents(ByVal folderPath As String) As String
        Dim t As New CTemplate("aspx_headerColumn_event.txt", folderPath)

        Dim sb As New StringBuilder()
        For Each i As String In Selected_HyperlinkColFirst
            If i = m_table.SortingColumn Then
            Else
                If sb.Length > 0 Then sb.Append(", ")
                If IsForeignKey(i) Then i = GuessFkEntity(i)
                Template_ReplaceNameAndExpression(sb, t, i, i)
            End If
        Next
        Return sb.ToString
    End Function
    Private Function ShowTemplate_Container_HideUpDownCode(ByVal folderPath As String) As String
        If Not IsEdit OrElse m_table.SortingColumn.Length = 0 Then Return String.Empty
        With New CTemplate("codebehind_hideupdown.txt", folderPath)
            .Replace("Plural", m_table.Plural)
            .Replace("PluralCamelCase", m_table.PluralCamelCase)
            Return .Template
        End With
    End Function
    Private Function ShowTemplate_Container_ResortToAlphaEventHandler(ByVal folderPath As String) As String
        If Not IsEdit OrElse m_table.SortingColumn.Length = 0 Then Return String.Empty
        With New CTemplate("codebehind_resortToAlpha.txt", folderPath)
            .Replace("ClassName", m_table.ClassName)
            Return .Template
        End With
    End Function
#End Region

#Region "ListItem"
    Private Sub ShowTemplate_ListItem(ByVal folderPath As String)
        If SpecialCase_Associative Then
            ShowTemplate_SpecialCaseAssociative(folderPath)
            Exit Sub
        End If

        'Aspx
        Try
            With New CTemplate("ascx.txt", folderPath)
                .Replace("Rows", ShowTemplate_ListItem_Rows(folderPath))
                .Replace("Singular", m_table.Singular)
                .Replace("SingularCamelCase", m_table.SingularCamelCase)
                .Replace("Plural", m_table.Plural.ToLower)
                txtAspx.Text = .Template
            End With
        Catch ex As Exception
            txtAspx.Text = ex.ToString
        End Try

        'CodeBehind
        Try
            Dim path As String = CStr(IIf(m_table.UseCaching, "codebehind_cached.txt", "codebehind_notCached.txt"))
            With New CTemplate(path, folderPath)
                .Replace("Namespace", m_info.CSharpNamespace)
                .Replace("UpDown", ShowTemplate_ListItem_MoveUpDownCode(folderPath))
                .Replace("HideUpDown", ShowTemplate_ListItem_HideUpDownCode(folderPath))
                .Replace("Checkboxes", ShowTemplate_ListItem_Checkboxes(folderPath))
                .Replace("Display", ShowTemplate_ListItem_Display(folderPath))
                .Replace("ClassName", m_table.ClassName)
                .Replace("Singular", m_table.Singular)
                .Replace("SingularCamelCase", m_table.SingularCamelCase)
                .Replace("Plural", m_table.Plural.ToLower)
                .Replace("PrimaryKeyName", m_table.PrimaryKeyName)
                txtCodeBehind.Text = .Template
            End With

        Catch ex As Exception
            txtCodeBehind.Text = ex.ToString
        End Try
    End Sub
    Private Function ShowTemplate_ListItem_Rows(ByVal folderPath As String) As String
        'Templates
        Dim basic As New CTemplate("ascx_basic.txt", folderPath)
        Dim link As New CTemplate("ascx_link.txt", folderPath)
        Dim bool As New CTemplate("ascx_bool.txt", folderPath)
        Dim dateT As New CTemplate("ascx_date.txt", folderPath)
        Dim upDown As CTemplate = Nothing
        If IsEdit Then upDown = New CTemplate("ascx_updown.txt", folderPath)

        Dim sb As New StringBuilder()
        For Each i As String In Selected_HyperlinkColFirst
            Select Case i
                Case m_table.SortingColumn
                    'Sorting column (up/down arrows)
                    If IsEdit Then sb.Append(upDown.Template)

                Case HyperlinkColumn
                    'Hyperlink columns
                    Template_ReplaceName(sb, link, i)

                Case Else
                    'Regular columns
                    Select Case m_table.GetColumnType(i).ToString
                        Case GetType(Boolean).ToString : Template_ReplaceName(sb, bool, i)
                        Case GetType(DateTime).ToString : Template_ReplaceName(sb, dateT, i)
                        Case Else : Template_ReplaceName(sb, basic, i)
                    End Select
            End Select
        Next

        Return sb.ToString
    End Function
    Private Function ShowTemplate_ListItem_MoveUpDownCode(ByVal folderPath As String) As String
        If Not IsEdit OrElse m_table.SortingColumn.Length = 0 Then Return String.Empty
        With New CTemplate("codebehind_updown.txt", folderPath)
            .Replace("SingularCamelCase", m_table.SingularCamelCase)
            Return .Template
        End With
    End Function
    Private Function ShowTemplate_ListItem_HideUpDownCode(ByVal folderPath As String) As String
        If Not IsEdit OrElse m_table.SortingColumn.Length = 0 Then Return String.Empty
        With New CTemplate("codebehind_hideupdown.txt", folderPath)
            Return .Template
        End With
    End Function
    Private Function ShowTemplate_ListItem_Checkboxes(ByVal folderPath As String) As String
        If Not IsEdit Then Return String.Empty
        Dim t As New CTemplate("codebehind_checkbox.txt", folderPath)
        Dim sb As New StringBuilder()
        For Each i As String In Selected
            If m_table.GetColumnType(i) Is GetType(Boolean) Then
                t.Reset()
                t.Replace("Name", i)
                t.Replace("SingularCamelCase", m_table.SingularCamelCase)
                sb.Append(t.Template)
            End If
        Next
        Return sb.ToString
    End Function
    Private Function ShowTemplate_ListItem_Display(ByVal folderPath As String) As String
        Dim basic As New CTemplate("codebehind_basic.txt", folderPath)
        Dim checkbox As New CTemplate("codebehind_bool.txt", folderPath)
        Dim link As New CTemplate("codebehind_link.txt", folderPath)
        Dim datet As New CTemplate("codebehind_date.txt", folderPath)
        Dim dec As New CTemplate("codebehind_dec.txt", folderPath)

        Dim sb As New StringBuilder()
        For Each i As String In Selected
            Dim expr As String = CStr(IIf(IsForeignKey(i), String.Concat(GuessFkEntity(i), ".", GuessFkEntity(i), "Name"), i))
            Select Case i
                Case HyperlinkColumn
                    'Hyperlink
                    Template_ReplaceNameAndExpression(sb, link, i, expr)

                Case m_table.SortingColumn
                    'Sort column - do nothing

                Case Else
                    'Regular columns
                    Select Case m_table.GetColumnType(i).ToString
                        Case GetType(Boolean).ToString
                            Template_ReplaceName(sb, checkbox, i)
                        Case GetType(String).ToString
                            Template_ReplaceNameAndExpression(sb, basic, i, expr)
                        Case GetType(DateTime).ToString
                            Template_ReplaceNameAndExpression(sb, datet, i, expr)
                        Case GetType(Decimal).ToString
                            Template_ReplaceNameAndExpression(sb, dec, i, expr)
                        Case Else
                            If Not IsForeignKey(i) Then expr = String.Concat(expr, ".ToString()")
                            Template_ReplaceNameAndExpression(sb, basic, i, expr)
                    End Select
            End Select
        Next
        Return sb.ToString
    End Function
#End Region

#Region "Urls"
    Public Sub ShowTemplate_Urls(ByVal folderPath As String)
        If SpecialCase_Associative Then folderPath &= "associative/"

        Dim outputSubFolder As String = New CTemplate(CStr(IIf(IsEdit, "subfolder_editable.txt", "subfolder_readonly.txt")), m_info.TemplateFolderWeb).Template

        'Aspx
        Try
            With New CTemplate("sitemap.txt", folderPath)
                .Replace("ClassName", m_table.ClassName)
                .Replace("Plural", m_table.Plural)
                .Replace("Singular", m_table.Singular)
                .Replace("PluralCamelCase", m_table.PluralCamelCase)
                .Replace("Subfolder", outputSubFolder)

                If Not IsNothing(Primary) Then
                    .Replace("SingularLhs", Primary.Singular)
                    .Replace("ClassNameLhs", Primary.ClassName)
                    .Replace("PluralLhs", Primary.Plural)
                    .Replace("PrimaryKeyLhs", Primary.PrimaryKeyName)
                    .Replace("PrimaryKeyCamelCaseLhs", Primary.PrimaryKeyCamelCase)
                End If
                If Not IsNothing(Secondary) Then
                    .Replace("SingularRhs", Secondary.Singular)
                    .Replace("ClassNameRhs", Secondary.ClassName)
                    .Replace("PluralRhs", Secondary.Plural)
                    .Replace("PrimaryKeyRhs", Secondary.PrimaryKeyName)
                    .Replace("PrimaryKeyCamelCaseRhs", Secondary.PrimaryKeyCamelCase)
                End If
                txtAspx.Text = .Template
            End With
        Catch ex As Exception
            txtAspx.Text = ex.ToString
        End Try


        'CodeBehind
        Try
            With New CTemplate("urls.txt", folderPath)
                .Replace("Namespace", m_info.CSharpNamespace)
                .Replace("ClassName", m_table.ClassName)
                .Replace("Plural", m_table.Plural)
                .Replace("PluralCamelCase", m_table.PluralCamelCase)
                .Replace("Singular", m_table.Singular)
                .Replace("SingularCamelCase", m_table.SingularCamelCase)
                .Replace("PrimaryKeyCamelCase", m_table.PrimaryKeyCamelCase)
                .Replace("PrimaryKeyType", m_table.PrimaryKeyTypeName(m_info.Language))
                .Replace("Subfolder", outputSubFolder)

                If SpecialCase_Associative Then
                    .Replace("SecondaryKeyCamelCase", m_table.SecondaryKeyCamelCase)
                    .Replace("SecondaryKeyType", m_table.SecondaryKeyTypeName(m_info.Language))
                    If Not IsNothing(Primary) Then .Replace("SingularLhs", Primary.Singular)
                    If Not IsNothing(Secondary) Then .Replace("SingularRhs", Secondary.Singular)
                    If Not IsNothing(Primary) Then .Replace("PrimaryKeyCamelCaseLhs", Primary.PrimaryKeyCamelCase)
                    If Not IsNothing(Secondary) Then .Replace("PrimaryKeyCamelCaseRhs", Secondary.PrimaryKeyCamelCase)
                End If

                'Display aspx
                txtCodeBehind.Text = .Template
            End With
        Catch ex As Exception
            txtCodeBehind.Text = ex.ToString
        End Try
    End Sub
#End Region

#Region "WriteFiles"
    Private Sub WriteFiles()
        Try
            Select Case Me.Template
                Case ETemplate.Page_AddEdit : WriteFiles_Details()
                Case ETemplate.Page_ListSearch : WriteFiles_List()
                Case ETemplate.UserControl_Item : WriteFiles_ListItem()
                Case ETemplate.UserControl_Container : WriteFiles_Container()
                Case ETemplate.Menu_SitemapUrls : WriteFilesUrlsAndSitemap()
            End Select
            MsgBox("Refresh your solution explorer to include the new files", MsgBoxStyle.OkOnly, "Files generate Successfully")
        Catch ex As Exception
            MsgBox(ex.Message, MsgBoxStyle.OkOnly, "Error writing files")
        End Try
    End Sub

    Private Sub WriteFiles_Details()
        Dim folderPath As String = m_info.TemplateFolderWebDetailsReadOnly
        If IsEdit Then folderPath = m_info.TemplateFolderWebDetailsEditable
        WriteFiles_Common(folderPath)
    End Sub
    Private Sub WriteFiles_List()
        Dim folderPath As String = m_info.TemplateFolderWebListReadOnly
        If IsEdit Then folderPath = m_info.TemplateFolderWebListEditable
        WriteFiles_Common(folderPath)
    End Sub
    Private Sub WriteFiles_ListItem()
        Dim folderPath As String = m_info.TemplateFolderWebListItemReadOnly
        If IsEdit Then folderPath = m_info.TemplateFolderWebListItemEditable
        WriteFiles_Common(folderPath)
    End Sub
    Private Sub WriteFiles_Container()
        Dim folderPath As String = m_info.TemplateFolderWebContainerReadOnly
        If IsEdit Then folderPath = m_info.TemplateFolderWebContainerEditable
        WriteFiles_Common(folderPath)
    End Sub
    Private Sub WriteFilesUrlsAndSitemap()
        Dim folderPath As String = m_info.TemplateFolderWebUrlsReadOnly
        If IsEdit Then folderPath = m_info.TemplateFolderWebUrlsEditable

        Dim sMapPath As String = OutputFolder & tabCtrlOutput.TabPages.Item(0).Text
        Dim urlsPath As String = OutputFolder & tabCtrlOutput.TabPages.Item(1).Text

        WriteFileUrls(urlsPath)
        WriteFileSitemap(folderPath, sMapPath)

    End Sub
    Private Sub WriteFileUrls(ByVal urlsPath As String)
        'Use template if no urls class, otherwise modify existing
        Dim urls As String = New CTemplate("class.txt", m_info.TemplateFolderWebUrls).Template
        urls.Replace("Namespace", m_info.CSharpNamespace)
        If IO.File.Exists(urlsPath) Then
            urls = IO.File.ReadAllText(urlsPath)

            'Find a spot to insert the region
            Dim firstRegion As Integer = urls.ToLower().IndexOf("#region")
            If -1 = firstRegion Then Throw New Exception("Urls class needs at least one region" & vbCrLf & urlsPath)

            'Create or Modify the class
            urls = urls.Insert(firstRegion, txtCodeBehind.Text & vbCrLf)
        End If
        IO.File.WriteAllText(urlsPath, urls)
    End Sub
    Private Sub WriteFileSitemap(ByVal folderPath As String, ByVal sitemapPath As String)
        If SpecialCase_Associative Then Exit Sub

        Dim folder As String = m_info.TemplateFolderWebUrlsReadOnly
        If IsEdit Then
            folder = m_info.TemplateFolderWebUrlsEditable
        End If
        Dim lookFor As String = New CTemplate("sitemap_lookfor.txt", folder).Template

        'Use template if no urls class, otherwise modify existing
        Dim sitemap As String = New CTemplate("xml.txt", folderPath).Template
        If IO.File.Exists(sitemapPath) Then
            sitemap = IO.File.ReadAllText(sitemapPath)

            'Find a spot to insert the region
            Dim foundAt As Integer = sitemap.ToLower().LastIndexOf(lookFor.ToLower)
            If -1 = foundAt Then Throw New Exception("Sitemap class should contain " & lookFor & vbCrLf & sitemapPath)

            'Create or Modify the class
            sitemap = sitemap.Insert(foundAt, vbCrLf & txtAspx.Text)
        End If
        IO.File.WriteAllText(sitemapPath, sitemap)
    End Sub
    Private Sub WriteFiles_Common(ByVal folderPath As String)
        'Process template for designer file
        Dim designer As New CTemplate("designer.txt", folderPath)
        designer.Replace("Singular", m_table.Singular)
        designer.Replace("Plural", m_table.Plural)

        'Get the output paths
        Dim aspxPath As String = OutputFolder & tabCtrlOutput.TabPages.Item(0).Text
        Dim codeBehindPath As String = OutputFolder & tabCtrlOutput.TabPages.Item(1).Text
        Dim designerPath As String = codeBehindPath.Replace(m_info.FileExtension, ".designer" & m_info.FileExtension)

        'Create the directory
        Dim newDirPath As String = IO.Path.GetDirectoryName(aspxPath)
        If Not IO.Directory.Exists(newDirPath) Then IO.Directory.CreateDirectory(newDirPath)

        'Write the files
        IO.File.WriteAllText(aspxPath, txtAspx.Text)
        IO.File.WriteAllText(codeBehindPath, txtCodeBehind.Text)
        If designer.Template.Trim.Length > 0 Then
            IO.File.WriteAllText(designerPath, designer.Template)
        End If
    End Sub
#End Region

#Region "Utilities"
    Private Function Template_ReplaceNames(ByVal t As CTemplate) As String
        Dim sb As New StringBuilder()
        For Each i As String In Selected
            If i = m_table.SortingColumn Then Continue For
            Template_ReplaceName(sb, t, i)
        Next
        Return sb.ToString
    End Function
    Private Sub Template_ReplaceNameAndExpression(ByVal sb As StringBuilder, ByVal t As CTemplate, ByVal columnName As String, ByVal expression As String, Optional ByVal reset As Boolean = True)
        If reset Then t.Reset()
        t.Replace("Name", columnName)
        t.Replace("NameShorter", CTable.Shorter(columnName, m_table.ColumnNames))
        t.Replace("PrimaryKeyName", m_table.PrimaryKeyName)
        t.Replace("Expression", expression)
        sb.Append(t.Template)
    End Sub
    Private Sub Template_ReplaceName(ByVal sb As StringBuilder, ByVal t As CTemplate, ByVal columnName As String, Optional ByVal reset As Boolean = True)
        If reset Then t.Reset()
        t.Replace("Name", columnName)
        t.Replace("NameShorter", CTable.Shorter(columnName, m_table.ColumnNames))
        t.Replace("PrimaryKeyName", m_table.PrimaryKeyName)
        sb.Append(t.Template)
    End Sub
#End Region

End Class
