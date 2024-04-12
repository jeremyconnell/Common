
Imports ProtoBuf
Imports System.Runtime.Serialization

<DataContract, ProtoContract>
Public Class CMigration
    Private Const TBL As String = "dbo.__MigrationHistory"
    Private Const PK As String = "MigrationId"
    Private Const SQL As String = "SELECT TOP 1 MigrationId, ContextKey, Model, ProductVersion, ROW_NUMBER() OVER(ORDER BY MigrationId ASC) AS RowNumber FROM dbo.__MigrationHistory ORDER BY MigrationId DESC"

    <DataMember(Order:=1)> Public MigrationId As String
    <DataMember(Order:=2)> Public ContextKey As String
    <DataMember(Order:=3)> Public Model As Byte()
    <DataMember(Order:=4)> Public ModelLength As Integer
    <DataMember(Order:=5)> Public RowNumber As Integer
    <DataMember(Order:=6)> Public ProductVersion As String
    <DataMember(Order:=7)> Public ModelMd5 As Guid

	'Preconstructor
	Shared Sub New()
		CProto.Prepare(Of CMigration)()
	End Sub

	'Constructors
	Friend Sub New()
    End Sub
    Friend Sub New(dr As IDataReader)
        Load(dr, False)
    End Sub
    Friend Sub New(dr As DataRow)
        Load(dr, False)
    End Sub
    Public Sub New(db As CDataSrcLocal, Optional discardModel As Boolean = True)
        Dim dr As IDataReader = Nothing
        Try
            dr = db.ExecuteReader(SQL)
            If dr.Read() Then Load(dr, discardModel)
        Catch ex As Exception
            Dim ss As String() = ex.Message.Split(CChar(vbCrLf))
            If ss.Length > 1 Then Throw New Exception(ss(1), ex)
            Throw
        Finally
            dr.Close()
        End Try
    End Sub
    Public Sub New(db As CDataSrc, Optional discardModel As Boolean = True)
        Dim dt As DataTable = db.ExecuteDataSet(SQL).Tables(0)
        Try
            If dt.Rows.Count > 0 Then Load(dt.Rows(0), discardModel)
        Catch ex As Exception
            Dim ss As String() = ex.Message.Split(CChar(vbCrLf))
            If ss.Length > 1 Then Throw New Exception(ss(1), ex)
            Throw
        Finally
        End Try
    End Sub

    Private Sub Load(dr As DataRow, discardModel As Boolean)
        MigrationId = CAdoData.GetStr(dr, "MigrationId")
        ContextKey = CAdoData.GetStr(dr, "ContextKey")
        ProductVersion = CAdoData.GetStr(dr, "ProductVersion")
        RowNumber = CAdoData.GetInt(dr, "RowNumber")
        Model = CAdoData.GetBytes(dr, "Model")
        ModelMd5 = CBinary.MD5_(Model)
        ModelLength = Model.Length
        If discardModel Then Model = New Byte() {}
    End Sub
    Private Sub Load(dr As IDataReader, discardModel As Boolean)
        MigrationId = CAdoData.GetStr(dr, "MigrationId")
        ContextKey = CAdoData.GetStr(dr, "ContextKey")
        ProductVersion = CAdoData.GetStr(dr, "ProductVersion")
        RowNumber = CAdoData.GetInt(dr, "RowNumber")
        Model = CAdoData.GetBytes(dr, "Model")
        ModelMd5 = CBinary.MD5_(Model)
        ModelLength = Model.Length
        If discardModel Then Model = New Byte() {}
    End Sub

    Private Function InsertData() As CNameValueList
        If IsNothing(Model) Then Throw New Exception("No Model Data to insert: " & MigrationId)

        Dim nv As New CNameValueList(4)
        nv.Add("MigrationId", MigrationId)
        nv.Add("ContextKey", ContextKey)
        nv.Add("ProductVersion", ProductVersion)
        If IsNothing(Model) Then
            nv.Add("ModelLength", ModelLength)
        Else
            nv.Add("Model", Model)
        End If
        Return nv
    End Function
    Public Function InsertInto(db As CDataSrc) As Object
        Return db.Insert(TBL, PK, True, InsertData, Nothing, Nothing)
    End Function

	Public Function InsertCmd_(db As CDataSrc) As CCommand
		Return New CCommand(InsertCmd(db))
	End Function
	Public Function InsertCmd(db As CDataSrc) As IDbCommand
		If db.IsLocal Then
			Return db.Local.InsertCmd(TBL, PK, True, InsertData, Nothing)
		End If
		Dim loc As New CSqlClient(String.Empty)
		Return loc.InsertCmd(TBL, PK, True, InsertData, Nothing)
	End Function

	Public ReadOnly Property MD5 As Guid
        Get
            Return CBinary.MD5_(MigrationId & Me.ModelMd5.ToString)
        End Get
    End Property
End Class
