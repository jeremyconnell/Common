Imports System.Text
Imports System.Collections.Generic

' Supports any CDataSrc including CWebSrc
' Uses data readers internally, unless the driver is CWebSrc (otherwise datasets)
' Calls to datareader functions will throw an error if the driver is CWebSrc (AppCode should use Business Objects or DataSets)
<Serializable(), CLSCompliant(True)> _
Public MustInherit Class CBaseStoredProcs : Inherits CBase

#Region "Constructors"
    'Main Constructors
    Protected Sub New()    'Used for Insert and Select-Multiple
        MyBase.New()
    End Sub
    Protected Sub New(ByVal primaryKey As Object) 'Used for Update and Select-Single
        MyBase.New(primaryKey, Nothing)
    End Sub
    Protected Sub New(ByVal primaryKey As Object, ByVal txOrNull As IDbTransaction) 'Used for Update and Select-Single within a transaction
        MyBase.New(primaryKey, txOrNull)
    End Sub
    Protected Sub New(ByVal dr As IDataReader) 'Used for Select-Multiple
        MyBase.New(dr)
    End Sub
    Protected Sub New(ByVal dr As DataRow) 'Used for Select-Multiple
        MyBase.new(dr)
    End Sub

    'As above, with CDataSrc
    Protected Sub New(ByVal dataSrc As CDataSrc)
        MyBase.New(dataSrc)
    End Sub
    Protected Sub New(ByVal dataSrc As CDataSrc, ByVal primaryKey As Object)
        MyBase.New(dataSrc, primaryKey)
    End Sub
    Protected Sub New(ByVal dataSrc As CDataSrc, ByVal primaryKey As Object, ByVal txOrNull As IDbTransaction)
        MyBase.New(dataSrc, primaryKey, txOrNull)
    End Sub
    Protected Sub New(ByVal dataSrc As CDataSrc, ByVal dr As IDataReader)
        MyBase.New(dataSrc, dr)
    End Sub
    Protected Sub New(ByVal dataSrc As CDataSrc, ByVal row As DataRow)
        MyBase.New(dataSrc, row)
    End Sub
#End Region

#Region "MustOverride/Overridable"
    'Stored Procs
    Protected MustOverride ReadOnly Property SpName_Insert() As String
    Protected MustOverride ReadOnly Property SpName_Update() As String
    Protected MustOverride ReadOnly Property SpName_DeleteId() As String
    Protected MustOverride ReadOnly Property SpName_SelectId() As String
    Protected MustOverride ReadOnly Property SpName_SelectAll() As String

    Protected MustOverride ReadOnly Property InsertPrimaryKey() As Boolean

#End Region

#Region "New Behaviour"
    'Public
    Public Function SelectAll(ByVal txOrNull As IDbTransaction) As IList
        Return MakeList(SpName_SelectAll, txOrNull)
    End Function
    Public Function SelectAll() As IList
        Return SelectAll(Nothing)
    End Function

    'Protected 
    Protected Overrides Function PrimaryKeys() As CNameValueList
        Dim pks As New CNameValueList
        pks.Add(PrimaryKeyName, PrimaryKeyValue)
        Return pks
    End Function
    Protected Overrides Sub Insert(ByVal txOrNull As System.Data.IDbTransaction)
        Dim obj As Object = DataSrc.ExecuteScalar(SpName_Insert, SaveParameters(InsertPrimaryKey), txOrNull)
        If Not InsertPrimaryKey Then Me.PrimaryKeyValue = obj
    End Sub
    Protected Overrides Function Update(ByVal txOrNull As System.Data.IDbTransaction) As Integer
        Return DataSrc.ExecuteNonQuery(SpName_Update, SaveParameters(True), txOrNull)
    End Function
    Protected Overrides Function DeleteId(ByVal txOrNull As System.Data.IDbTransaction) As Integer
        Return DataSrc.ExecuteNonQuery(SpName_DeleteId, PrimaryKeys, txOrNull)
    End Function
    Protected Overrides Function SelectIdAsDs(ByVal txOrNull As System.Data.IDbTransaction) As DataSet
        Return DataSrc.ExecuteDataSet(SpName_SelectId, PrimaryKeys, txOrNull)
    End Function
    Protected Overrides Function SelectIdAsDr(ByVal txOrNull As System.Data.IDbTransaction) As IDataReader
        Return DataSrc.Local.ExecuteReader(SpName_SelectId, PrimaryKeys, txOrNull)
    End Function
#End Region

End Class

