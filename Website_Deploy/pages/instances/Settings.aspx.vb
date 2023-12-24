
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports SchemaDeploy

Partial Class pages_clients_Settings
	Inherits CPageDeploy

	'Querystring
	Public ReadOnly Property InstanceId As Integer
        Get
            Return CWeb.RequestInt("instanceId")
        End Get
    End Property
    Public ReadOnly Property GroupId As Integer
        Get
            Return CWeb.RequestInt("groupId")
        End Get
    End Property

    'Entity
    Public ReadOnly Property AppId As Integer
        Get
            Return App.AppId
        End Get
    End Property
    Public ReadOnly Property App As CApp
        Get
            Return Instance.App
        End Get
    End Property
    Public ReadOnly Property Client As CClient
        Get
            Return CClient.Cache.GetById(ClientId_)
        End Get
    End Property
    Public ReadOnly Property ClientId_ As Integer
        Get
            Return Instance.InstanceClientId
        End Get
    End Property
    Public ReadOnly Property Instance As SchemaDeploy.CInstance
        Get
            Return CInstance.Cache.GetById(InstanceId)
        End Get
    End Property

    'Page Events
    Private m_dict As New Dictionary(Of String, WebControl)

	Protected Overrides Sub PageInit()
		UnbindSideMenu()
		MenuInstanceSettings(AppId, InstanceId)

		ddApp.DataSource = CApp.Cache
		ddApp.DataBind()
		CDropdown.SetValue(ddApp, AppId)


		If IsNothing(Client) Then
			ddInstance.DataSource = CInstance.Cache
		Else
			ddInstance.DataSource = App.Instances
		End If
		ddInstance.DataBind()
		CDropdown.BlankItem(ddInstance, "-- Select Instance --")
		CDropdown.SetValue(ddInstance, InstanceId)

		ddGroups.DataSource = SchemaDeploy.CGroup.Cache
		ddGroups.DataBind()
		CDropdown.BlankItem(ddGroups, "-- All Settings --")
		CDropdown.SetValue(ddGroups, GroupId)

		CDropdown.SetValue(rbl, CSession.Home_ViewOrEdit)

		Dim isEdit As Boolean = 1 = CSession.Home_ViewOrEdit

		Dim c As CClient = Client
		Dim ins As CInstance = Instance

		If IsNothing(c) Then Exit Sub
		If IsNothing(ins) Then
			If Client.InstanceCount > 0 Then
				Response.Redirect(CSitemap.InstanceSettings(ClientId_, Client.Instances(0).InstanceId))
			Else
				AddLinkSide("New Instance", CSitemap.InstanceAdd(EApp.ControlTrack, ClientId_))
			End If
			Exit Sub
		End If

		Dim total As Integer = App.Groups.Keys.Count 'ins.Values.Count
		Dim half As Integer = total / 2
		'Dim third As Integer = total / 3
		Dim left As Integer = 0
		'Dim middle As Integer = 0

		For Each g As CGroup In CGroup.Cache
			If GroupId > 0 AndAlso g.GroupId <> GroupId Then Continue For

			'Fill left side, then right
			Dim plh As PlaceHolder = plh1
			'If left > third Then plh = plh2
			'If middle > third Then plh = plh3
			If left > half Then plh = plh2

			'New Group
			Dim div As New Panel
			plh.Controls.Add(div)

			Dim tbl As New Table
			tbl.CssClass = "datagrid"
			div.Controls.Add(tbl)

			Dim tr As TableRow = RowH(tbl)
			Dim td1 As TableCell = CellLinkH(tr, g.GroupName, CSitemap.InstanceSettings(ClientId_, g.GroupId), False)
			td1.ColumnSpan = 2

			If isEdit Then
				td1.ColumnSpan = 1

				Dim th As TableCell = CellH(tr, String.Empty)
				th.Style.Add("text-align", "right")

				Dim btn As New Button
				btn.ID = "btn" & g.GroupId
				btn.Text = "Save"
				btn.Font.Size = New FontUnit(FontSize.Smaller)
				th.Controls.Add(btn)
				AddHandler btn.Click, AddressOf btn_Click
			End If



			'Settings
			For Each k In g.Keys
				tr = Row(tbl)
				left += 1 'If plh.ID = plh1.ID Then left += 1 Else middle += 1

				Dim tdk As TableCell = CellLink(tr, k.KeyName, CSitemap.KeySetting(k.KeyName))

				'May have a value
				Dim vv As CValueList = ins.Values.GetByKeyName(k.KeyName)
				Dim v As CValue = Nothing
				If vv.Count > 0 Then v = vv(0)
				If Not isEdit Then
					'Viewer
					If Not IsNothing(v) Then
						CellLink(tr, v.ValueAsString, CSitemap.ValueEdit(ins, k), False, False).Style.Add("min-width", "30px")
					Else
						CellLink(tr, "add»", CSitemap.ValueEdit(ins, k), False, False).Style.Add("min-width", "30px")
					End If
				Else
					'Editor
					tdk.Style.Add("min-width", "300px")

					Dim td As TableCell = Cell(tr)
					td.Style.Add("min-width", "200px")
					Select Case k.KeyFormatId_
						Case EFormat.Boolean
							Dim rbl As New RadioButtonList
							rbl.ID = "rbl" & k.KeyName
							m_dict.Add(k.KeyName, rbl)
							rbl.RepeatDirection = RepeatDirection.Horizontal
							rbl.RepeatLayout = RepeatLayout.Flow
							CDropdown.Add(rbl, "NULL", -1)
							CDropdown.Add(rbl, "False", 0)
							CDropdown.Add(rbl, "True", 1)
							td.Controls.Add(rbl)

							If Not IsNothing(v) Then
								If v.ValueBoolean.HasValue Then
									CDropdown.SetValue(rbl, IIf(v.ValueBoolean.Value, 1, 0))
								Else
									CDropdown.SetValue(rbl, -1)
								End If
							Else
								CDropdown.SetValue(rbl, -1)
							End If

						Case EFormat.String
							Dim txt As New TextBox
							txt.ID = "txt" & k.KeyName
							txt.Width = New Unit("200px")
							txt.Font.Size = New FontUnit(FontSize.Smaller)

							td.Controls.Add(txt)
							m_dict.Add(k.KeyName, txt)

							If Not IsNothing(v) Then txt.Text = v.ValueString
							txt.ToolTip = txt.Text

						Case EFormat.Integer
							Dim txt As New TextBox
							txt.ID = "txt" & k.KeyName
							txt.Width = New Unit("50px")
							txt.Font.Size = New FontUnit(FontSize.Smaller)
							txt.Style.Add("text-align", "right")
							td.Controls.Add(txt)
							m_dict.Add(k.KeyName, txt)

							If Not IsNothing(v) Then CTextbox.SetNumber(txt, v.ValueInteger)
					End Select

				End If
			Next
		Next

	End Sub

	Protected Overrides Sub PageLoad()
		Dim i As CInstance = Me.Instance

		txtInstanceSettingsImported.Text = CUtilities.Timespan(i.InstanceSettingsImported)
		txtInstanceSettingsExported.Text = CUtilities.Timespan(i.InstanceSettingsExported)

	End Sub


	'Form Events
	Sub ddApp_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddApp.SelectedIndexChanged
		Response.Redirect(CSitemap.Instances(CDropdown.GetInt(ddApp)))
	End Sub
	Sub ddInstance_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddInstance.SelectedIndexChanged, ddGroups.SelectedIndexChanged
        Response.Redirect(CSitemap.InstanceSettings(CDropdown.GetInt(ddInstance), CDropdown.GetInt(ddGroups)))
    End Sub
    Private Sub btn_Click(sender As Object, e As EventArgs)
        Dim btn As Button = sender
        Dim id As String = btn.ID.Replace("btn", String.Empty)
        Dim groupId As Integer = Integer.Parse(id)
        Dim values As CValueList = Instance.Values
        For Each i As CKey In CKey.Cache.GetByGroupId(groupId)
            Dim ctrl As WebControl = Nothing
            If Not m_dict.TryGetValue(i.KeyName, ctrl) Then Continue For

            Dim v As CValue = Nothing
            Dim vv As CValueList = values.GetByKeyName(i.KeyName)
            If vv.Count > 0 Then
                v = vv(0)
            Else
                v = New CValue
                v.ValueInstanceId = InstanceId
                v.ValueKeyName = i.KeyName
            End If

            If i.KeyFormatId_ = EFormat.Boolean Then
                Dim rbl As RadioButtonList = ctrl
                Dim val As Integer = CDropdown.GetInt(rbl)
                If val = -1 Then
                    v.ValueBoolean = Nothing
                ElseIf val = 0 Then
                    v.ValueBoolean = False
                Else
                    v.ValueBoolean = True
                End If
            ElseIf i.KeyFormatId_ = EFormat.String Then
                Dim txt As TextBox = ctrl
                v.ValueString = txt.Text
            ElseIf i.KeyFormatId_ = EFormat.Integer Then
                Dim txt As TextBox = ctrl
                v.ValueInteger = CTextbox.GetIntegerNullable(txt)
            End If
            v.Save()
        Next
        Response.Redirect(CSitemap.InstanceSettings(ClientId_, groupId))
    End Sub




	Private Sub rbl_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rbl.SelectedIndexChanged
        CSession.Home_ViewOrEdit = CDropdown.GetInt(rbl)
        Response.Redirect(Request.RawUrl)
    End Sub

End Class
