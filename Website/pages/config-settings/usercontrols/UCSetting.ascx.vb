
Partial Class usercontrols_config_settings_UCSetting
    Inherits System.Web.UI.UserControl

#Region "Members"
    Private m_setting As CSetting
    Private m_list As CSettingList
#End Region

#Region "Constants"
    Public Const CSS_SHORT As String = "control short"
#End Region

#Region "Interface"
    Public Sub Display(ByVal setting As CSetting, ByVal list As CSettingList)
        m_setting = setting
        m_list = list

        If Parent.Controls.Count Mod 2 = 0 Then row.Attributes.Add("class", "alt_row")

        With setting
            lblName.Text = .SettingName

            lnk.NavigateUrl = CSitemap.SettingEdit(.SettingId)

            'Otherwise, show the appropriate control
            Select Case .SettingTypeId
                Case EConfigType.String
                    txt.Visible = True
                    txt.Text = .SettingValueString

                Case EConfigType.Int
                    txt.Visible = True
                    txt.CssClass = CSS_SHORT
                    CTextbox.SetNumber(txt, .SettingValueInteger)
                    CTextbox.RightAlign(txt)

                Case EConfigType.Double
                    txt.Visible = True
                    txt.CssClass = CSS_SHORT
                    CTextbox.SetNumber(txt, .SettingValueDouble)
                    CTextbox.RightAlign(txt)

                Case EConfigType.Money
                    txt.Visible = True
                    txt.CssClass = CSS_SHORT
                    CTextbox.SetMoney(txt, .SettingValueMoney)
                    CTextbox.RightAlign(txt)

                Case EConfigType.Boolean
                    chk.Visible = True
                    chk.Text = .SettingName
                    chk.Checked = .SettingValueBoolean

                    'Case EConfigType.Date
                    '    txt.Visible = False
                    '    cal.Visible = True
                    '    If DateTime.MinValue <> .SettingValueDate Then cal.SelectedDate = .SettingValueDate.ToString()


                Case EConfigType.ListAsInteger, EConfigType.ListAsString
                    'Show the dropdown
                    txt.Visible = False
                    ddList.Visible = True

                    'Load the dropdown
                    If IsNothing(setting.List) Then Exit Sub
                    With setting.List
                        If Not .ListIsExternal Then
                            For Each i As CItem In .Items
                                CDropdown.Add(ddList, i.ItemName, i.ItemId)
                            Next
                        Else
                            ddList.DataTextField = .ListExteralNameColumn
                            ddList.DataValueField = .ListExteralPrimaryKey
                            ddList.DataSource = .ExternalData
                            ddList.DataBind()
                        End If
                        CDropdown.BlankItem(ddList, String.Concat("-- Select ", .ListName, " --"))
                    End With

                    'Set the value
                    If .SettingTypeId_ = EConfigType.ListAsInteger Then
                        CDropdown.SetValue(ddList, .SettingValueInteger)
                    Else
                        CDropdown.SetValue(ddList, .SettingValueString)
                    End If
            End Select
        End With
    End Sub
    Public Function Store() As CSetting
        With m_setting
            Select Case .SettingTypeId
                Case EConfigType.Boolean
                    .SettingValueBoolean = chk.Checked
                    'Case EConfigType.Date
                    '    .SettingValueDate = DateTime.Parse(cal.SelectedDate)
                Case EConfigType.Double
                    .SettingValueDouble = CTextbox.GetNumber(txt)
                Case EConfigType.Money
                    .SettingValueMoney = CDec(CTextbox.GetMoney(txt))
                Case EConfigType.Int
                    .SettingValueInteger = CTextbox.GetInteger(txt)
                Case EConfigType.String
                    .SettingValueString = txt.Text
                Case EConfigType.ListAsInteger
                    .SettingValueInteger = CDropdown.GetInt(ddList)
                Case EConfigType.ListAsString
                    If ddList.SelectedIndex > 0 Then
                        .SettingValueString = ddList.SelectedItem.Text
                    Else
                        .SettingValueString = String.Empty
                    End If
            End Select
        End With
        Return m_setting
    End Function
#End Region

#Region "Event Handlers - Buttons"
    Private Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnDelete.Click
        m_setting.Delete()
        Refresh()
    End Sub
    Private Sub btnMoveUp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnMoveUp.Click
        m_list.MoveUp(m_setting)
        Refresh()
    End Sub
    Private Sub btnMoveDn_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnMoveDn.Click
        m_list.MoveDown(m_setting)
        Refresh()
    End Sub
#End Region

#Region "Private"
    Private Sub Refresh()
        Response.Redirect(CSitemap.ConfigSettings)
    End Sub
#End Region

End Class
