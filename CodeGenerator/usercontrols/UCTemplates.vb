Public Class UCTemplates

#Region "Members"
    Private m_template As CPathAndTemplate
#End Region

#Region "Form"
    Public ReadOnly Property Skin() As CSkin
        Get
            Return CType(ddSkins.SelectedItem, CSkin)
        End Get
    End Property
    Friend Property Template() As CPathAndTemplate
        Get
            Return m_template
        End Get
        Set(ByVal value As CPathAndTemplate)
            m_template = value

            SetSelected(tvTemplates.Nodes, value)

            If IsNothing(value) Then
                lblRelativePath.Text = String.Empty
            Else
                lblRelativePath.Text = value.RelativePath
            End If
        End Set
    End Property
    Friend ReadOnly Property TemplateSelected() As Boolean
        Get
            Return Not IsNothing(Me.Template)
        End Get
    End Property
    Public Property Locked() As Boolean
        Get
            Return btnCancel.Enabled
        End Get
        Set(ByVal value As Boolean)
            btnCancel.Enabled = value
            btnSave.Enabled = value
            txtTemplate.ReadOnly = Not value
            ddSkins.Enabled = Not value
            btnEdit.Enabled = Not value 'AndAlso Not Me.Skin.IsDefault
            btnLock.Enabled = Not value AndAlso Not Me.Skin.IsDefault
            tvTemplates.Enabled = Not value
        End Set
    End Property
#End Region

#Region "Event Handlers"
    Private Sub UCTemplates_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim s As CSkin = CUser_Templates.CurrentSkin
        ddSkins.DataSource = CUser_Templates.SkinsAndDefault 'reset currentskin
        ddSkins.SelectedItem = s
    End Sub
    Private Sub ddSkins_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddSkins.SelectedIndexChanged
        CUser_Templates.CurrentSkin = Me.Skin

        btnEdit.Enabled = Not Me.Skin.IsDefault
        chkModifiedOnly.Enabled = Not Me.Skin.IsDefault

        If sender Is ddSkins Then chkModifiedOnly.Checked = Not Me.Skin.IsDefault

        If Not chkModifiedOnly.Checked Then
            LoadTree(Me.Skin.AllTemplates)
        Else
            LoadTree(Me.Skin)
        End If
        If txtSearch.Text.Length > 0 Then tvTemplates.ExpandAll()
        tvTemplates_AfterSelect(Nothing, Nothing)
    End Sub
    Private Sub chkModifiedOnly_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkModifiedOnly.CheckedChanged
        ddSkins_SelectedIndexChanged(Nothing, Nothing)
    End Sub
    Private Sub tvTemplates_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvTemplates.AfterSelect
        If Not IsNothing(e) AndAlso Not IsNothing(e.Node) AndAlso Not IsNothing(e.Node.Tag) Then
            Me.Template = CType(e.Node.Tag, CPathAndTemplate)
        Else
            If Not IsNothing(sender) Then
                Me.Template = Nothing
                lblRelativePath.Text = "/" & e.Node.FullPath
            End If
        End If

        Me.Locked = False

        If TemplateSelected Then
            txtTemplate.Text = Template.Template
        Else
            btnLock.Enabled = False
            txtTemplate.Text = String.Empty
        End If

        btnRestore.Enabled = Not Me.Skin.IsDefault AndAlso Not IsNothing(Me.Template) AndAlso Not Me.Template Is CSkin.Default(Me.Template.RelativePath) 'txtTemplate.Text <> CSkin.Default(Me.Template.RelativePath).Template
    End Sub
    Private Sub btnCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreate.Click
        With New FormTemplate(False)
            If .ShowDialog() <> DialogResult.OK Then Exit Sub
            txtSearch.Text = String.Empty
            UCTemplates_Load(sender, e)
        End With
    End Sub
    Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        With New FormTemplate(True)
            If .ShowDialog() <> DialogResult.OK Then Exit Sub
            UCTemplates_Load(sender, e)
        End With
    End Sub
    Private Sub btnLock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLock.Click
        Me.Locked = True
    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Locked = False
    End Sub
    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Dim t As CPathAndTemplate = Me.Template

        If Me.Skin.Contains(Me.Template) Then
            Me.Template.Template = txtTemplate.Text
        Else
            t = Me.Skin.Add(t.RelativePath, txtTemplate.Text)
        End If
        CUser_Templates.SaveSkins()
        Me.Locked = False

        Me.Template = t
        ddSkins_SelectedIndexChanged(Nothing, Nothing)
    End Sub
    Private Sub btnRestore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRestore.Click
        Dim msg As String = "Do you want to reset this custom template?" & vbCrLf & "This will discard your changes and restore it to the default state"
        If MessageBox.Show(msg, "Confirm Reset Template?", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) = DialogResult.OK Then
            Dim path As String = Me.Template.RelativePath
            Me.Skin.Remove(Me.Template)
            tvTemplates.Nodes.Clear()
            CUser_Templates.CurrentSkin = Me.Skin
            ddSkins_SelectedIndexChanged(Nothing, Nothing)
        End If
    End Sub
    Private Sub txtSearch_TextChanged(ByVal sender As System.Object, ByVal e As KeyEventArgs) Handles txtSearch.KeyDown
        If e.KeyValue = 13 Then btnSearch_Click(Nothing, Nothing)
    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        txtSearch.Text = txtSearch.Text.ToLower
        ddSkins_SelectedIndexChanged(sender, e)
    End Sub
#End Region

#Region "LoadTree"
    Private Sub LoadTree(ByVal skin As CSkin)
        tvTemplates.Nodes.Clear()
        For Each i As CPathAndTemplate In skin
            'Search filter
            If txtSearch.Text.Length > 0 Then
                If Not i.Template.ToLower.Contains(txtSearch.Text) Then Continue For
            End If

            'Add items
            Dim n As TreeNode = AddNode(i)
            n.Tag = i
        Next

        SetCounts(tvTemplates.Nodes)

        'Expand top 2 levels
        For Each i As TreeNode In tvTemplates.Nodes
            i.Expand()
            If IsNothing(i.NextNode) Then Exit For
            For Each j As TreeNode In i.Nodes
                j.Expand()
            Next
        Next

        Me.Template = Me.Template
    End Sub
    Private Function AddNode(ByVal i As CPathAndTemplate) As TreeNode
        Dim path As New List(Of String)(i.RelativePath.Split(CChar("/")))
        path.RemoveAt(0)
        Return AddNode(tvTemplates.Nodes, i, path)
    End Function
    Private Function AddNode(ByVal nodes As TreeNodeCollection, ByVal i As CPathAndTemplate, ByVal path As List(Of String)) As TreeNode
        Dim node As TreeNode = FindNode(nodes, path(0))
        path.RemoveAt(0)
        If path.Count = 0 Then Return node
        Return AddNode(node.Nodes, i, path)
    End Function
    Private Function FindNode(ByVal nodes As TreeNodeCollection, ByVal name As String) As TreeNode
        For Each i As TreeNode In nodes
            If i.Text = name Then Return i
        Next
        Dim node As New TreeNode(name)
        nodes.Add(node)
        Return node
    End Function
    Private Sub SetSelected(ByVal nodes As TreeNodeCollection, ByVal value As CPathAndTemplate)
        If IsNothing(value) Then Exit Sub
        For Each i As TreeNode In nodes
            If Not IsNothing(i.Tag) Then
                If CType(i.Tag, CPathAndTemplate).Equals(value) Then
                    i.EnsureVisible()
                    Exit Sub
                End If
            End If
            SetSelected(i.Nodes, value)
        Next
    End Sub
    Private Sub SetCounts(ByVal nodes As TreeNodeCollection)
        For Each i As TreeNode In nodes
            If i.Nodes.Count > 0 Then
                i.Text = String.Concat(i.Text, " (", CountChildren(i), ")")
                SetCounts(i.Nodes)
            End If
        Next
    End Sub
    Private Function CountChildren(ByVal node As TreeNode) As Integer
        Dim i As Integer = 0
        CountChildren(node, i)
        Return i
    End Function
    Private Sub CountChildren(ByVal node As TreeNode, ByRef total As Integer)
        For Each i As TreeNode In node.Nodes
            If i.Nodes.Count = 0 Then total += 1
            CountChildren(i, total)
        Next
    End Sub
#End Region

End Class
