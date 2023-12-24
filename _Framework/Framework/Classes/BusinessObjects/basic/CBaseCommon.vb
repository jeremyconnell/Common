'Note - Included for backwards-compat only, code has moved to CAdoData
<Serializable(), CLSCompliant(True)> _
Public Class CBaseCommon


#Region "Shared - Null-Equivalent Values"
    Protected Shared Function NullVal(ByVal obj As Object) As Object
        Return CAdoData.NullVal(obj)
    End Function
#End Region

#Region "Shared - Get Data From DataRow"
    Protected Shared Function GetStr(ByVal dr As DataRow, ByVal columnName As String) As String
        Return CAdoData.GetStr(dr, columnName, Nothing)
    End Function
    Protected Shared Function GetBool(ByVal dr As DataRow, ByVal columnName As String) As Boolean
        Return CAdoData.GetBool(dr, columnName, False)
    End Function
    Protected Shared Function GetDec(ByVal dr As DataRow, ByVal columnName As String) As Decimal
        Return CAdoData.GetDec(dr, columnName, CAdoData.NULL_DECIMAL)
    End Function
    Protected Shared Function GetInt(ByVal dr As DataRow, ByVal columnName As String) As Integer
        Return CAdoData.GetInt(dr, columnName, CAdoData.NULL_INTEGER)
    End Function
    Protected Shared Function GetLong(ByVal dr As DataRow, ByVal columnName As String) As Long
        Return CAdoData.GetLong(dr, columnName, CAdoData.NULL_LONG)
    End Function
    Protected Shared Function GetDbl(ByVal dr As DataRow, ByVal columnName As String) As Double
        Return CAdoData.GetDbl(dr, columnName, CAdoData.NULL_DOUBLE)
    End Function
    Protected Shared Function GetSingle(ByVal dr As DataRow, ByVal columnName As String) As Single
        Return CAdoData.GetSingle(dr, columnName, CAdoData.NULL_SINGLE)
    End Function
    Protected Shared Function GetDate(ByVal dr As DataRow, ByVal columnName As String) As DateTime
        Return CAdoData.GetDate(dr, columnName, CAdoData.NULL_DATE)
    End Function
    Protected Shared Function GetBytes(ByVal dr As DataRow, ByVal columnName As String) As Byte()
        Return CAdoData.GetBytes(dr, columnName, Nothing)
    End Function
    Protected Shared Function GetGuid(ByVal dr As DataRow, ByVal columnName As String) As Guid
        Return CAdoData.GetGuid(dr, columnName, CAdoData.NULL_GUID)
    End Function


    Protected Shared Function GetStr(ByVal dr As DataRow, ByVal columnName As String, ByVal nullValue As String) As String
        Return CStr(GetValue(dr, columnName, nullValue))
    End Function
    Protected Shared Function GetBool(ByVal dr As DataRow, ByVal columnName As String, ByVal nullValue As Boolean) As Boolean
        Return CBool(GetValue(dr, columnName, nullValue))
    End Function
    Protected Shared Function GetDec(ByVal dr As DataRow, ByVal columnName As String, ByVal nullValue As Decimal) As Decimal
        Return CDec(GetValue(dr, columnName, nullValue))
    End Function
    Protected Shared Function GetInt(ByVal dr As DataRow, ByVal columnName As String, ByVal nullValue As Integer) As Integer
        Return CInt(GetValue(dr, columnName, nullValue))
    End Function
    Protected Shared Function GetLong(ByVal dr As DataRow, ByVal columnName As String, ByVal nullValue As Long) As Long
        Return CLng(GetValue(dr, columnName, nullValue))
    End Function
    Protected Shared Function GetDbl(ByVal dr As DataRow, ByVal columnName As String, ByVal nullValue As Double) As Double
        Return CDbl(GetValue(dr, columnName, nullValue))
    End Function
    Protected Shared Function GetSingle(ByVal dr As DataRow, ByVal columnName As String, ByVal nullValue As Double) As Single
        Return CType(GetValue(dr, columnName, nullValue), Single)
    End Function
    Protected Shared Function GetDate(ByVal dr As DataRow, ByVal columnName As String, ByVal nullValue As Date) As Date
        Return CDate(GetValue(dr, columnName, nullValue))
    End Function
    Protected Shared Function GetBytes(ByVal dr As DataRow, ByVal columnName As String, ByVal nullValue As Byte()) As Byte()
        Return CType(GetValue(dr, columnName, nullValue), Byte())
    End Function
    Protected Shared Function GetGuid(ByVal dr As DataRow, ByVal columnName As String, ByVal nullValue As Guid) As Guid
        Return CType(GetValue(dr, columnName, nullValue), Guid)
    End Function
#End Region

#Region "Shared - Get Data From DataReader"
    Protected Shared Function GetStr(ByVal dr As IDataReader, ByVal columnName As String) As String
        Return CAdoData.GetStr(dr, columnName, Nothing)
    End Function
    Protected Shared Function GetBool(ByVal dr As IDataReader, ByVal columnName As String) As Boolean
        Return CAdoData.GetBool(dr, columnName, False)
    End Function
    Protected Shared Function GetDec(ByVal dr As IDataReader, ByVal columnName As String) As Decimal
        Return CAdoData.GetDec(dr, columnName, CAdoData.NULL_DECIMAL)
    End Function
    Protected Shared Function GetInt(ByVal dr As IDataReader, ByVal columnName As String) As Integer
        Return CAdoData.GetInt(dr, columnName, CAdoData.NULL_INTEGER)
    End Function
    Protected Shared Function GetLong(ByVal dr As IDataReader, ByVal columnName As String) As Long
        Return CAdoData.GetLong(dr, columnName, CAdoData.NULL_LONG)
    End Function
    Protected Shared Function GetDbl(ByVal dr As IDataReader, ByVal columnName As String) As Double
        Return CAdoData.GetDbl(dr, columnName, CAdoData.NULL_DOUBLE)
    End Function
    Protected Shared Function GetSingle(ByVal dr As IDataReader, ByVal columnName As String) As Single
        Return CAdoData.GetSingle(dr, columnName, CAdoData.NULL_SINGLE)
    End Function
    Protected Shared Function GetDate(ByVal dr As IDataReader, ByVal columnName As String) As DateTime
        Return CAdoData.GetDate(dr, columnName, CAdoData.NULL_DATE)
    End Function
    Protected Shared Function GetBytes(ByVal dr As IDataReader, ByVal columnName As String) As Byte()
        Return CAdoData.GetBytes(dr, columnName, Nothing)
    End Function
    Protected Shared Function GetGuid(ByVal dr As IDataReader, ByVal columnName As String) As Guid
        Return CAdoData.GetGuid(dr, columnName, CAdoData.NULL_GUID)
    End Function


    Protected Shared Function GetStr(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As String) As String
        Return CStr(GetValue(dr, columnName, nullValue))
    End Function
    Protected Shared Function GetBool(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As Boolean) As Boolean
        Return CBool(GetValue(dr, columnName, nullValue))
    End Function
    Protected Shared Function GetDec(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As Decimal) As Decimal
        Return CDec(GetValue(dr, columnName, nullValue))
    End Function
    Protected Shared Function GetInt(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As Integer) As Integer
        Return CInt(GetValue(dr, columnName, nullValue))
    End Function
    Protected Shared Function GetLong(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As Long) As Long
        Return CLng(GetValue(dr, columnName, nullValue))
    End Function
    Protected Shared Function GetSingle(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As Single) As Single
        Return CSng(GetValue(dr, columnName, nullValue))
    End Function
    Protected Shared Function GetDbl(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As Double) As Double
        Return CDbl(GetValue(dr, columnName, nullValue))
    End Function
    Protected Shared Function GetDate(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As Date) As Date
        Return CDate(GetValue(dr, columnName, nullValue))
    End Function
    Protected Shared Function GetBytes(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As Byte()) As Byte()
        Return CType(GetValue(dr, columnName, nullValue), Byte())
    End Function
    Protected Shared Function GetGuid(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As Guid) As Guid
        Return CType(GetValue(dr, columnName, nullValue), Guid)
    End Function
#End Region

#Region "Shared (Private) - Get Data From DataReader/DataRow"
    Protected Shared Function GetValue(ByVal dr As DataRow, ByVal columnName As String, ByVal nullValue As Object) As Object
        Return CAdoData.GetValue(dr, columnName, nullValue)
    End Function
    Protected Shared Function GetValue(ByVal dr As DataRow, ByVal columnNumber As Integer, ByVal nullValue As Object) As Object
        Return CAdoData.GetValue(dr, columnNumber, nullValue)
    End Function
    Protected Shared Function GetValue(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As Object) As Object
        Return CAdoData.GetValue(dr, columnName, nullValue)
    End Function
    Protected Shared Function GetValue(ByVal dr As IDataReader, ByVal columnNumber As Integer, ByVal nullValue As Object) As Object
        Return CAdoData.GetValue(dr, columnNumber, nullValue)
    End Function
    Protected Shared Function GetOrdinal(ByVal dr As IDataReader, ByVal columnName As String) As Integer
        Return CAdoData.GetOrdinal(dr, columnName)
    End Function
#End Region

End Class
