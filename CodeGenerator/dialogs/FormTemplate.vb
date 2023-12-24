Imports System.Windows.Forms

Public Class FormTemplate
    'Constructors
    Public Sub New(ByVal isEdit As Boolean)
        InitializeComponent()

        btnDelete.Enabled = isEdit
        If isEdit Then
            Me.Text = "Edit Skin"
            With CUser_Templates.CurrentSkin
                txtName.Text = .Name
                txtDescription.Text = .Description
                txtUrl.Text = .Url

                rbNetwork.Checked = Not String.IsNullOrEmpty(.Url)
                rb_CheckedChanged(Nothing, Nothing)

                txtName.Enabled = Not .IsDefault
                txtUrl.Enabled = Not .IsDefault
                OK_Button.Enabled = Not .IsDefault
                btnDelete.Enabled = Not .IsDefault
                rbNetwork.Enabled = Not .IsDefault
                rbLocal.Enabled = Not .IsDefault
                btnImport.Enabled = Not .IsDefault

                btnExport.Enabled = True
            End With
        End If
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        If txtName.Text.Trim.Length = 0 And rbLocal.Checked Then
            MessageBox.Show("A Name is Requred", "Required Field Missing", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If
        If btnDelete.Enabled Then
            With CUser_Templates.CurrentSkin
                If rbLocal.Checked Then
                    .Url = String.Empty
                    If .IsDefault Then CUser_Templates.CurrentSkin = CSkin.ImportDefaultFromFile
                    .Name = txtName.Text
                    .Description = txtDescription.Text
                Else
                    .Name = txtName.Text
                    .Url = txtUrl.Text
                    Me.Cursor = Cursors.WaitCursor
                    .UpdateFromNetwork()
                    Me.Cursor = Cursors.Arrow
                End If
            End With
            CUser_Templates.CurrentSkin = CUser_Templates.CurrentSkin
        Else
            If rbLocal.Checked Then
                CUser_Templates.AddSkin(txtName.Text, txtDescription.Text)
            Else
                If Not CUser_Templates.AddSkin(txtUrl.Text) Then Return
                CUser_Templates.CurrentSkin.Name = txtName.Text
            End If
        End If
        CUser_Templates.SaveSkins()
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If MessageBox.Show("Delete this Skin?", "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) = Windows.Forms.DialogResult.OK Then
            CUser_Templates.RemoveSkin()
            CUser_Templates.CurrentSkin = CSkin.Default
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Dim name As String = CUser_Templates.CurrentSkin.Name
        name = name.Replace("-", "").Trim.Replace(" ", "_") & ".zip" 'gz
        SaveFileDialog1.FileName = name
        If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
            Try
                CUser_Templates.CurrentSkin.ToZip(SaveFileDialog1.FileName)
                'CBinary.SerialiseToBytesAndZip(CUser_Templates.CurrentSkin, SaveFileDialog1.FileName)
            Catch ex As Exception
                MessageBox.Show(ex.ToString, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnImport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnImport.Click
        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            Try
                Dim skin As CSkin
                If ".gz" = IO.Path.GetExtension(OpenFileDialog1.FileName.ToLower) Then
                    skin = CType(CBinary.DeserialiseFromBytesAndUnzip(OpenFileDialog1.FileName), CSkin)
                Else
                    skin = New CSkin()
                    skin.FromZip(OpenFileDialog1.FileName)
                End If
                With CUser_Templates.CurrentSkin
                    .Clear()
                    .AddRange(skin)
                End With
                CUser_Templates.SaveSkins()

                'todo - prevent editing of linked templates, allow import from create dialog

                'Preserve existing name etc
                'txtDescription.Text = skin.Description
                'txtName.Text = skin.Name
                'txtUrl.Text = skin.Url
                'chkExternal.Checked = Not String.IsNullOrEmpty(skin.Url)

                OK_Button_Click(sender, e)
            Catch ex As Exception
                MessageBox.Show(ex.ToString, "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub


    Private Sub rb_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbLocal.CheckedChanged, rbNetwork.CheckedChanged
        txtUrl.Enabled = rbNetwork.Checked
        txtDescription.Enabled = rbLocal.Checked
        btnImport.Enabled = Not rbNetwork.Checked And btnDelete.Enabled
    End Sub
End Class
