'Modifies upgradeserver to act as a distribution point
Public Class CUpgradeServer_Rest_DistributionPoint : Inherits CUpgradeServer_Rest_Aspx

    Protected Overrides ReadOnly Property IsDistributionPoint As Boolean
        Get
            Return True
        End Get
    End Property

End Class
