Partial Public Class pages_audit_errors_usercontrols_UCAudit_Error : Inherits UserControl

#Region "Interface"
    Public Sub Display(ByVal [audit_Error] As CAudit_Error, ByVal list As CAudit_ErrorList, ByVal pi As CPagingInfo, ByVal isUnique As Boolean)
        If Parent.Controls.Count Mod 2 = 0 Then row.Attributes.Add("class", "alt_row")

        With [audit_Error]
            litNumber.Text = CStr(list.IndexOf([audit_Error]) + 1 + pi.PageIndex * pi.PageSize) & "."
            If isUnique Then
                litErrorId.Text = .ErrorID.ToString 'Count
                lnkErrorDateCreated.NavigateUrl = CSitemap.Audit_Error(.ErrorTypeHash, .ErrorMessageHash, .ErrorInnerTypeHash, .ErrorInnerMessageHash)
            Else
                litErrorId.Text = String.Concat("#", .ErrorID) 'ErrorId
                lnkErrorDateCreated.NavigateUrl = CSitemap.Audit_Error(.ErrorID)
            End If
            lnkErrorDateCreated.ToolTip = CUtilities.LongDateTime(.ErrorDateCreated)
            lnkErrorDateCreated.Text = CUtilities.Timespan(.ErrorDateCreated)
            litErrorUserName.Text = CStr(IIf(.ErrorUserName.Length = 0, "...", .ErrorUserName))
            lnkErrorWebsite.Text = .ErrorWebsite
            lnkErrorWebsite.NavigateUrl = .FullUrl
            litErrorMachineName.Text = .ErrorMachineName
            litErrorType.Text = .ErrorType
            litErrorMessage.Text = Server.HtmlEncode(.ErrorMessage)
            litErrorInnerType.Text = .ErrorInnerType
            litErrorInnerMessage.Text = Server.HtmlEncode(.ErrorInnerMessage)
        End With
    End Sub
#End Region


End Class

