Imports SchemaDeploy
Partial Class pages_keys_Settings
    Inherits CPageWithTableHelpers

    Public ReadOnly Property KeyName As String
        Get
            Return CWeb.RequestStr("keyName")
        End Get
    End Property
    Public ReadOnly Property Key As CKey
        Get
            Return CKey.Cache.GetById(KeyName)
        End Get
    End Property

    Protected Overrides Sub PageInit()
        Me.Title &= KeyName

        MenuSelected = "Keys"
        AddMenuSide("Keys...", CSitemap.Keys)
        AddMenuSide("Key Details", CSitemap.KeyEdit(KeyName))
        AddMenuSide(CUtilities.NameAndCount("All Clients", Key.Values))

        ddKeys.DataSource = CKey.Cache
        ddKeys.DataBind()
        CDropdown.SetValue(ddKeys, KeyName)

        If IsNothing(Key) Then
            Response.Redirect(CSitemap.Keys)
            Exit Sub
        End If
    End Sub

    Protected Overrides Sub PagePreRender()
        Dim div As New Panel
        plh.Controls.Add(div)

        Dim tbl As New Table
        tbl.CssClass = "datagrid"
        plh.Controls.Add(tbl)

        Dim tr As TableRow = RowH(tbl)
        CellH(tr, "Client")
        CellH(tr, "Value").ColumnSpan = 2

        Dim k As CKey = Key
        For Each c As CClient In CClient.Cache
            For Each i As SchemaDeploy.CInstance In c.Instances
                tr = Row(tbl)

                CellLink(tr, c.ClientName, CSitemap.InstanceSettings(c.ClientId, i.InstanceId))

                Dim vv As CValueList = k.Values.GetByInstanceId(i.InstanceId)
                If vv.Count > 0 Then
                    Dim v As CValue = vv(0)
                    Cell(tr, v.ValueAsString)
                    CellLink(tr, "edit»", CSitemap.ValueEdit(v.ValueId), False)
                Else
                    Cell(tr)
                    CellLink(tr, "edit»", CSitemap.ValueEdit(i, k), False)
                End If
            Next
        Next

        tr = RowH(tbl)
        CellH(tr, "Default Value")
        CellH(tr, k.DefaultValue).Style.Add("font-weight", "normal")
        CellLinkH(tr, "edit»", CSitemap.KeyEdit(KeyName), False, False)

    End Sub


    Sub ddKeys_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddKeys.SelectedIndexChanged
        Response.Redirect(CSitemap.KeySetting(ddKeys.SelectedValue))
    End Sub
End Class
