Partial Public Class pages_audit_logs_usercontrols_UCAudit_Log : Inherits UserControl

#Region "Members"
    Private m_audit_Log As CAudit_Log
    Private m_sortedList As CAudit_LogList
    Private m_pageIndex As Integer
#End Region

#Region "Interface"
	Public Sub Display(ByVal [audit_Log] As CAudit_Log, sortedList As CAudit_LogList, pi As CPagingInfo, instanceId As Integer?)
		If Parent.Controls.Count Mod 2 = 0 Then row.Attributes.Add("class", "alt_row")

		m_audit_Log = [audit_Log]
		m_sortedList = sortedList
		m_pageIndex = pi.PageIndex

		With [audit_Log]
			litNumber.Text = CStr(sortedList.IndexOf(m_audit_Log) + 1 + pi.PageIndex * pi.PageSize)
			litLogTypeId.Text = .LogTypeName
			litLogMessage.Text = .LogMessage
			lnkLogCreated.ToolTip = CUtilities.LongDateTime(.LogCreated)
			lnkLogCreated.Text = CUtilities.Timespan(.LogCreated)
			If instanceId.HasValue Then
				lnkLogCreated.NavigateUrl = CSitemap.InstanceAuditTrailLog(instanceId.Value, .LogId)
			Else
				lnkLogCreated.NavigateUrl = CSitemap.Audit_LogEdit(.LogId)
			End If
		End With
	End Sub
#End Region


End Class
