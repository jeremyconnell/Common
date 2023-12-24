
Partial Class usercontrols_UCYearMonth
    Inherits System.Web.UI.UserControl

    Public Event Changed As EventHandler

    Public Property Label As string
        Get
            Return lbl.Text
        End Get
        Set(value As String)
            lbl.Text = value
        End Set
    End Property

    Public Property Year As Integer
        Get
            Return CDropdown.GetInt(ddYear)
        End Get
        Set(value As Integer)
            CDropdown.SetValue(ddYear, value)
        End Set
    End Property
    Public Property Month As Integer
        Get
            Return CDropdown.GetInt(ddMonth)
        End Get
        Set(value As Integer)
            CDropdown.SetValue(ddMonth, value)
        End Set
    End Property
    Public Property Bold As Boolean
        Get
            Return ddYear.Font.Bold
        End Get
        Set(value As Boolean)
            ddYear.Font.Bold = value
            ddMonth.Font.Bold = value
            lbl.Font.Bold = value
        End Set
    End Property


    Public ReadOnly Property FromDate As DateTime 'Greater than or equal to
        Get
            If Year < 0 Then Return DateTime.MinValue
            If Month < 0 Then Return New DateTime(Year, 1, 1)
            Return New DateTime(Year, Month, 1)
        End Get
    End Property
    Public ReadOnly Property ToDate As DateTime 'Less than
        Get
            If Year < 0 Then Return DateTime.MinValue
            If Month < 0 Then Return New DateTime(Year, 12, 1).AddMonths(1)
            Return New DateTime(Year, Month, 1).AddMonths(1)
        End Get
    End Property


    Protected Sub Page_Init(sender As Object, e As System.EventArgs) Handles Me.Init
        For i As Integer = DateTime.Now.Year To DateTime.Now.AddYears(-20).Year Step -1
            CDropdown.Add(ddYear, i)
        Next
        CDropdown.BlankItem(ddYear)

        For i As Integer = 1 To 12
            CDropdown.Add(ddMonth, New DateTime(2000, i, 1).ToString("MMM"), i)
        Next
        CDropdown.BlankItem(ddMonth)
    End Sub

    Protected Sub Page_PreRender(sender As Object, e As System.EventArgs) Handles Me.PreRender
        ddMonth.Visible = Year > 0
    End Sub

    Protected Sub ddMonth_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddMonth.SelectedIndexChanged
        RaiseEvent Changed(Me, e)
    End Sub
    Protected Sub ddYear_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddYear.SelectedIndexChanged
        RaiseEvent Changed(Me, e)
    End Sub
End Class
