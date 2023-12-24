
Partial Class usercontrols_audit_trail_UCTrail
    Inherits System.Web.UI.UserControl


	Public Sub Display(ByVal trail As CAudit_Trail, ByVal showAllColumns As Boolean)
		If Parent.Controls.Count Mod 2 = 0 Then row.Attributes.Add("class", "alt_row")

		With trail
			lnkDate.Text = .AuditDate.ToString("dd-MMM-yy")
			lnkTime.Text = .AuditDate.ToString("h:mm:ss tt")
			lblTable.Text = CAudit_Trail.ShortenTableName(.ShorterTableName)
			lblTable.ToolTip = CAudit_Trail.ShortenTableName(.AuditDataTableName)

			litPrimaryKey.Text = .AuditDataPrimaryKey
			lnkUser.Text = .AuditUserLoginName

			lnkType.Text = .AuditTypeId.ToString()
			lnkType.NavigateUrl = .AuditUrl

			With .Differences(showAllColumns)
				For Each i As CChange In .Added
					UCDiff(tbody).DisplayAdded(i)
				Next
				For Each i As CChange In .Changed
					UCDiff(tbody).DisplayChanged(i)
				Next
				For Each i As CChange In .Removed
					UCDiff(tbody).DisplayRemoved(i)
				Next
				If showAllColumns Then
					For Each i As CChange In .Same
						UCDiff(tbody).DisplaySame(i)
					Next
				End If
			End With
		End With
	End Sub

	Private Function UCDiff(ByVal target As Control) As usercontrols_audit_trail_UCDiff
        Dim ctrl As Control = LoadControl("~/pages/audit-trail/usercontrols/UCDiff.ascx")
        target.Controls.Add(ctrl)
        Return CType(ctrl, usercontrols_audit_trail_UCDiff)
    End Function
End Class
