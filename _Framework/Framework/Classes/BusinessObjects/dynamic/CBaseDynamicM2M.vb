
' Supports any CDataSrc including CWebSrc
' Uses data readers internally, unless the driver is CWebSrc (otherwise datasets)
' Calls to datareader functions will throw an error if the driver is CWebSrc (AppCode should use Business Objects or DataSets)
<Serializable(), CLSCompliant(True)> _
Public MustInherit Class CBaseDynamicM2M : Inherits CBaseDynamic

#Region "Constructors"
    'Main Constructors
    Protected Sub New()    'Used for Insert and Select-Multiple
        MyBase.New()
    End Sub
    Protected Sub New(ByVal primaryKey As Object, ByVal secondaryKey As Object) 'Used for Update and Select-Single
        Me.Load(primaryKey, secondaryKey, Nothing)
    End Sub
    Protected Sub New(ByVal primaryKey As Object, ByVal secondaryKey As Object, ByVal txOrNull As IDbTransaction) 'Used for Update and Select-Single within a transaction
        Me.Load(primaryKey, secondaryKey, txOrNull)
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
    Protected Sub New(ByVal dataSrc As CDataSrc, ByVal primaryKey As Object, ByVal secondaryKey As Object) 'Used for Update and Select-Single
        Me.DataSrc = dataSrc
        Me.Load(primaryKey, secondaryKey, Nothing)
    End Sub
    Protected Sub New(ByVal dataSrc As CDataSrc, ByVal primaryKey As Object, ByVal secondaryKey As Object, ByVal txOrNull As IDbTransaction) 'Used for Update and Select-Single within a transaction
        Me.DataSrc = dataSrc
        Me.Load(primaryKey, secondaryKey, txOrNull)
    End Sub
    Protected Sub New(ByVal dataSrc As CDataSrc, ByVal dr As IDataReader) 'Used for Select-Multiple
        MyBase.New(dataSrc, dr)
    End Sub
    Protected Sub New(ByVal dataSrc As CDataSrc, ByVal dr As DataRow) 'Used for Select-Multiple
        MyBase.New(dataSrc, dr)
    End Sub
#End Region

#Region "MustOverride"
    Protected MustOverride ReadOnly Property SecondaryKeyName() As String
    Protected MustOverride Property SecondaryKeyValue() As Object

    Protected Overrides ReadOnly Property InsertPrimaryKey() As Boolean
        Get
            Return True
        End Get
    End Property
    Protected Overrides Function PrimaryKeys() As CNameValueList
        Dim d As New CNameValueList(2)
        d.Add(PrimaryKeyName, PrimaryKeyValue)
        d.Add(SecondaryKeyName, SecondaryKeyValue)
        Return d
    End Function
    Protected Overrides Function PrimaryKeyNames() As String()
        Return New String() {PrimaryKeyName, SecondaryKeyName}
    End Function
#End Region

#Region "Protected"
    Protected Overloads Sub Load(ByVal primaryKeyVal As Object, ByVal secondaryKeyVal As Object, ByVal txOrNull As IDbTransaction)
        Me.PrimaryKeyValue = primaryKeyVal
        Me.SecondaryKeyValue = secondaryKeyVal
        Me.Reload(txOrNull)
    End Sub
#End Region

End Class
