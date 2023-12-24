Public Class CUpgradeServer_Wcf_DistributionPoint : Inherits CUpgradeServer_Wcf

    Protected Overrides ReadOnly Property IsDistributionPoint As Boolean
        Get
            Return True
        End Get
    End Property

End Class