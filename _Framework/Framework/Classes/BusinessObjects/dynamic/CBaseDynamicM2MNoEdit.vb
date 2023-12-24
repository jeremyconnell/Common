
' Supports any CDataSrc including CWebSrc
' Uses data readers internally, unless the driver is CWebSrc (otherwise datasets)
' Calls to datareader functions will throw an error if the driver is CWebSrc (AppCode should use Business Objects or DataSets)
<Serializable(), CLSCompliant(True)> _
Public MustInherit Class CBaseDynamicM2MNoEdit : Inherits CBaseDynamicM2M

#Region "Constructors (smaller set)"
    'Main Constructors
    Protected Sub New()    'Used for Insert and Select-Multiple
    End Sub
    Protected Sub New(ByVal dr As IDataReader) 'Used for Select-Multiple
        Me.Load(dr)
    End Sub
    Protected Sub New(ByVal dr As DataRow) 'Used for Select-Multiple
        Me.Load(dr)
    End Sub

    'As above, with CDataSrc
    Protected Sub New(ByVal dataSrc As CDataSrc)
        MyBase.New(dataSrc)
    End Sub
    Protected Sub New(ByVal dataSrc As CDataSrc, ByVal dr As IDataReader) 'Used for Select-Multiple
        MyBase.New(dataSrc, dr)
    End Sub
    Protected Sub New(ByVal dataSrc As CDataSrc, ByVal dr As DataRow) 'Used for Select-Multiple
        MyBase.New(dataSrc, dr)
    End Sub
#End Region

#Region "ReadColumns (table only contains PKs)"
    Protected Overrides Sub ReadColumns(ByVal dr As IDataReader)
        PrimaryKeyValue = dr(PrimaryKeyName)
        SecondaryKeyValue = dr(SecondaryKeyName)
    End Sub
    Protected Overrides Sub ReadColumns(ByVal row As DataRow)
        PrimaryKeyValue = row.Item(PrimaryKeyName)
        SecondaryKeyValue = row.Item(SecondaryKeyName)
    End Sub
#End Region

#Region "Save Override"
    Public Overrides Sub Save(ByVal txOrNull As System.Data.IDbTransaction)
        If Not m_insertPending Then Exit Sub
        MyBase.Save(txOrNull)
    End Sub
#End Region

End Class
