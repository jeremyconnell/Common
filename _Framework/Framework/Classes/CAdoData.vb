
<Serializable(), CLSCompliant(True)>
Public Class CAdoData

    Public Delegate Function DGetDateByIndex(ByVal dr As IDataReader, ByVal columnIndex As Integer, ByVal nullValue As Date) As Date
    Public Delegate Function DGetDateByName(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As Date) As Date
    Protected Shared m_customGetDateByIndex As New Dictionary(Of Type, DGetDateByIndex)   'Type of data-reader
    Protected Shared m_customGetDateByName As New Dictionary(Of Type, DGetDateByName)   'Type of data-reader

    Shared Sub Register(type As Type, useMethod As DGetDateByIndex)
        m_customGetDateByIndex.Add(type, useMethod)
    End Sub
    Shared Sub Register(type As Type, useMethod As DGetDateByName)
        m_customGetDateByName.Add(type, useMethod)
    End Sub

    Public Enum EWildCards
        None
        Leading
        Trailing
        Both
    End Enum


#Region "Constants - Null-Equivalent Values"
    Public Const NULL_INTEGER As Integer = Integer.MinValue
    Public Const NULL_LONG As Long = Long.MinValue
    Public Const NULL_SHORT As Short = Short.MinValue
    Public Const NULL_DOUBLE As Double = Double.NaN
    Public Const NULL_SINGLE As Single = Single.NaN
    Public Const NULL_BYTE As Byte = 0
    Public Const NULL_DECIMAL As Decimal = Decimal.MinValue 'Note: Use Decimal.IsNaN instead of equals
    Public Shared NULL_DATE As DateTime = DateTime.MinValue
    Public Shared NULL_GUID As Guid = Guid.Empty
    Public Shared NULL_BOOLEAN As Boolean = False 'One-Way, converts DBNull => bool
#End Region

#Region "Shared - DBNull Substitution (Applied before Insert/Update)"
    Public Shared Function NullVal(ByVal obj As Object) As Object
        If IsNothing(obj) Then Return DBNull.Value 'All reference types, plus 3.5 nullable types

        If TypeOf (obj) Is Guid Then
            If NULL_GUID = CType(obj, Guid) Then Return DBNull.Value Else Return obj
        End If
        If TypeOf (obj) Is Integer Then
            If NULL_INTEGER = CInt(obj) Then Return DBNull.Value Else Return obj
        End If
        If TypeOf (obj) Is Long Then
            If NULL_LONG = CLng(obj) Then Return DBNull.Value Else Return obj
        End If
        If TypeOf (obj) Is Short Then
            If NULL_SHORT = CShort(obj) Then Return DBNull.Value Else Return obj
        End If
        If TypeOf (obj) Is Decimal Then
            If NULL_DECIMAL = CDec(obj) Then Return DBNull.Value Else Return obj
        End If
        If TypeOf (obj) Is DateTime Then
            If NULL_DATE = CDate(obj) Then Return DBNull.Value Else Return obj
        End If
        If TypeOf (obj) Is Byte Then
            If NULL_BYTE = CByte(obj) Then Return DBNull.Value Else Return obj
        End If
        'Special case - IsNan
        If TypeOf (obj) Is Double Then
            If Double.IsNaN(CDbl(obj)) Then Return DBNull.Value Else Return obj
        End If
        If TypeOf (obj) Is Single Then
            If Single.IsNaN(CType(obj, Single)) Then Return DBNull.Value Else Return obj
        End If

        Return obj
    End Function
    Public Shared Function EscapeWildcardsForLIKE(ByVal s As String, ByVal wildCards As EWildCards) As String
        s = EscapeWildcardsForLIKE(s)
        Select Case wildCards
            Case EWildCards.Leading : Return String.Concat("%", s)
            Case EWildCards.Trailing : Return String.Concat(s, "%")
            Case EWildCards.Both : Return String.Concat("%", s, "%")
            Case Else : Return s
        End Select
    End Function
    Public Shared Function EscapeWildcardsForLIKE(ByVal s As String) As String
        If String.IsNullOrEmpty(s) Then Return String.Empty
        s = s.Replace("%", "[%]").Replace("_", "[_]") 'Encode accidental wildcards
        s = s.Replace("*", "%").Replace("?", "_") 'Encode deliberate wildcards
        Return s
    End Function
#End Region

#Region "Shared - Get Data From DataRow"
    'Column names, with auto defaults for NULL
    Public Shared Function GetStr(ByVal dr As DataRow, ByVal columnName As String) As String
        Return GetStr(dr, columnName, Nothing)
    End Function
    Public Shared Function GetBool(ByVal dr As DataRow, ByVal columnName As String) As Boolean
        Return GetBool(dr, columnName, NULL_BOOLEAN)
    End Function
    Public Shared Function GetDec(ByVal dr As DataRow, ByVal columnName As String) As Decimal
        Return GetDec(dr, columnName, NULL_DECIMAL)
    End Function
    Public Shared Function GetInt(ByVal dr As DataRow, ByVal columnName As String) As Integer
        Return GetInt(dr, columnName, NULL_INTEGER)
    End Function
    Public Shared Function GetLong(ByVal dr As DataRow, ByVal columnName As String) As Long
        Return GetLong(dr, columnName, NULL_LONG)
    End Function
    Public Shared Function GetDbl(ByVal dr As DataRow, ByVal columnName As String) As Double
        Return GetDbl(dr, columnName, NULL_DOUBLE)
    End Function
    Public Shared Function GetSingle(ByVal dr As DataRow, ByVal columnName As String) As Single
        Return GetSingle(dr, columnName, NULL_SINGLE)
    End Function
    Public Shared Function GetDate(ByVal dr As DataRow, ByVal columnName As String) As DateTime
        Return GetDate(dr, columnName, NULL_DATE)
    End Function
    Public Shared Function GetByte(ByVal dr As DataRow, ByVal columnName As String) As Byte
        Return GetByte(dr, columnName, Nothing)
    End Function
    Public Shared Function GetBytes(ByVal dr As DataRow, ByVal columnName As String) As Byte()
        Return GetBytes(dr, columnName, Nothing)
    End Function
    Public Shared Function GetGuid(ByVal dr As DataRow, ByVal columnName As String) As Guid
        Return GetGuid(dr, columnName, NULL_GUID)
    End Function

    'Column numbers, with auto defaults for NULL
    Public Shared Function GetStr(ByVal dr As DataRow, ByVal columnIndex As Integer) As String
        Return GetStr(dr, columnIndex, Nothing)
    End Function
    Public Shared Function GetBool(ByVal dr As DataRow, ByVal columnIndex As Integer) As Boolean
        Return GetBool(dr, columnIndex, NULL_BOOLEAN)
    End Function
    Public Shared Function GetDec(ByVal dr As DataRow, ByVal columnIndex As Integer) As Decimal
        Return GetDec(dr, columnIndex, NULL_DECIMAL)
    End Function
    Public Shared Function GetInt(ByVal dr As DataRow, ByVal columnIndex As Integer) As Integer
        Return GetInt(dr, columnIndex, NULL_INTEGER)
    End Function
    Public Shared Function GetLong(ByVal dr As DataRow, ByVal columnIndex As Integer) As Long
        Return GetLong(dr, columnIndex, NULL_LONG)
    End Function
    Public Shared Function GetDbl(ByVal dr As DataRow, ByVal columnIndex As Integer) As Double
        Return GetDbl(dr, columnIndex, NULL_DOUBLE)
    End Function
    Public Shared Function GetSingle(ByVal dr As DataRow, ByVal columnIndex As Integer) As Single
        Return GetSingle(dr, columnIndex, NULL_SINGLE)
    End Function
    Public Shared Function GetDate(ByVal dr As DataRow, ByVal columnIndex As Integer) As DateTime
        Return GetDate(dr, columnIndex, NULL_DATE)
    End Function
    Public Shared Function GetByte(ByVal dr As DataRow, ByVal columnIndex As Integer) As Byte
        Return GetByte(dr, columnIndex, NULL_BYTE)
    End Function
    Public Shared Function GetBytes(ByVal dr As DataRow, ByVal columnIndex As Integer) As Byte()
        Return GetBytes(dr, columnIndex, Nothing)
    End Function
    Public Shared Function GetGuid(ByVal dr As DataRow, ByVal columnIndex As Integer) As Guid
        Return GetGuid(dr, columnIndex, NULL_GUID)
    End Function

    'Column names, with custom defaults for NULL
    Public Shared Function GetStr(ByVal dr As DataRow, ByVal columnName As String, ByVal nullValue As String) As String
        Return CStr(GetValue(dr, columnName, nullValue))
    End Function
    Public Shared Function GetBool(ByVal dr As DataRow, ByVal columnName As String, ByVal nullValue As Boolean) As Boolean
        Return CBool(GetValue(dr, columnName, nullValue))
    End Function
    Public Shared Function GetDec(ByVal dr As DataRow, ByVal columnName As String, ByVal nullValue As Decimal) As Decimal
        Return CDec(GetValue(dr, columnName, nullValue))
    End Function
    Public Shared Function GetInt(ByVal dr As DataRow, ByVal columnName As String, ByVal nullValue As Integer) As Integer
        Return CInt(GetValue(dr, columnName, nullValue))
    End Function
    Public Shared Function GetLong(ByVal dr As DataRow, ByVal columnName As String, ByVal nullValue As Long) As Long
        Return CLng(GetValue(dr, columnName, nullValue))
    End Function
    Public Shared Function GetDbl(ByVal dr As DataRow, ByVal columnName As String, ByVal nullValue As Double) As Double
        Return CDbl(GetValue(dr, columnName, nullValue))
    End Function
    Public Shared Function GetSingle(ByVal dr As DataRow, ByVal columnName As String, ByVal nullValue As Single) As Single
        Return CType(GetValue(dr, columnName, nullValue), Single)
    End Function
    Public Shared Function GetDate(ByVal dr As DataRow, ByVal columnName As String, ByVal nullValue As Date) As Date
        Return CDate(GetValue(dr, columnName, nullValue))
    End Function
    Public Shared Function GetByte(ByVal dr As DataRow, ByVal columnName As String, ByVal nullValue As Byte) As Byte
        Return CType(GetValue(dr, columnName, nullValue), Byte)
    End Function
    Public Shared Function GetBytes(ByVal dr As DataRow, ByVal columnName As String, ByVal nullValue As Byte()) As Byte()
        Return CType(GetValue(dr, columnName, nullValue), Byte())
    End Function
    Public Shared Function GetGuid(ByVal dr As DataRow, ByVal columnName As String, ByVal nullValue As Guid) As Guid
        Return CType(GetValue(dr, columnName, nullValue), Guid)
    End Function

    'Column numbers, with custom defaults for NULL
    Public Shared Function GetStr(ByVal dr As DataRow, ByVal columnIndex As Integer, ByVal nullValue As String) As String
        Return CStr(GetValue(dr, columnIndex, nullValue))
    End Function
    Public Shared Function GetBool(ByVal dr As DataRow, ByVal columnIndex As Integer, ByVal nullValue As Boolean) As Boolean
        Return CBool(GetValue(dr, columnIndex, nullValue))
    End Function
    Public Shared Function GetDec(ByVal dr As DataRow, ByVal columnIndex As Integer, ByVal nullValue As Decimal) As Decimal
        Return CDec(GetValue(dr, columnIndex, nullValue))
    End Function
    Public Shared Function GetInt(ByVal dr As DataRow, ByVal columnIndex As Integer, ByVal nullValue As Integer) As Integer
        Return CInt(GetValue(dr, columnIndex, nullValue))
    End Function
    Public Shared Function GetLong(ByVal dr As DataRow, ByVal columnIndex As Integer, ByVal nullValue As Long) As Long
        Return CLng(GetValue(dr, columnIndex, nullValue))
    End Function
    Public Shared Function GetDbl(ByVal dr As DataRow, ByVal columnIndex As Integer, ByVal nullValue As Double) As Double
        Return CDbl(GetValue(dr, columnIndex, nullValue))
    End Function
    Public Shared Function GetSingle(ByVal dr As DataRow, ByVal columnIndex As Integer, ByVal nullValue As Single) As Single
        Return CType(GetValue(dr, columnIndex, nullValue), Single)
    End Function
    Public Shared Function GetDate(ByVal dr As DataRow, ByVal columnIndex As Integer, ByVal nullValue As Date) As Date
        Return CDate(GetValue(dr, columnIndex, nullValue))
    End Function
    Public Shared Function GetByte(ByVal dr As DataRow, ByVal columnIndex As Integer, ByVal nullValue As Byte) As Byte
        Return CType(GetValue(dr, columnIndex, nullValue), Byte)
    End Function
    Public Shared Function GetBytes(ByVal dr As DataRow, ByVal columnIndex As Integer, ByVal nullValue As Byte()) As Byte()
        Return CType(GetValue(dr, columnIndex, nullValue), Byte())
    End Function
    Public Shared Function GetGuid(ByVal dr As DataRow, ByVal columnIndex As Integer, ByVal nullValue As Guid) As Guid
        Dim obj As Object = GetValue(dr, columnIndex, nullValue)
        If TypeOf obj Is String Then Return New Guid(CStr(obj))
        Return CType(obj, Guid)
    End Function
#End Region

#Region "Shared - Get Data From DataReader"
    'Column names, with auto defaults for NULL
    Public Shared Function GetStr(ByVal dr As IDataReader, ByVal columnName As String) As String
        Return GetStr(dr, columnName, Nothing)
    End Function
    Public Shared Function GetBool(ByVal dr As IDataReader, ByVal columnName As String) As Boolean
        Return GetBool(dr, columnName, NULL_BOOLEAN)
    End Function
    Public Shared Function GetDec(ByVal dr As IDataReader, ByVal columnName As String) As Decimal
        Return GetDec(dr, columnName, NULL_DECIMAL)
    End Function
    Public Shared Function GetInt(ByVal dr As IDataReader, ByVal columnName As String) As Integer
        Return GetInt(dr, columnName, NULL_INTEGER)
    End Function
    Public Shared Function GetLong(ByVal dr As IDataReader, ByVal columnName As String) As Long
        Return GetLong(dr, columnName, NULL_LONG)
    End Function
    Public Shared Function GetDbl(ByVal dr As IDataReader, ByVal columnName As String) As Double
        Return GetDbl(dr, columnName, NULL_DOUBLE)
    End Function
    Public Shared Function GetSingle(ByVal dr As IDataReader, ByVal columnName As String) As Single
        Return GetSingle(dr, columnName, NULL_SINGLE)
    End Function
    Public Shared Function GetDate(ByVal dr As IDataReader, ByVal columnName As String) As DateTime
        Return GetDate(dr, columnName, NULL_DATE)
    End Function
    Public Shared Function GetByte(ByVal dr As IDataReader, ByVal columnName As String) As Byte
        Return GetByte(dr, columnName, NULL_BYTE)
    End Function
    Public Shared Function GetBytes(ByVal dr As IDataReader, ByVal columnName As String) As Byte()
        Return GetBytes(dr, columnName, Nothing)
    End Function
    Public Shared Function GetGuid(ByVal dr As IDataReader, ByVal columnName As String) As Guid
        Return GetGuid(dr, columnName, NULL_GUID)
    End Function

    'Column names, with custom defaults for NULL
    Public Shared Function GetStr(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As String) As String
        Return CStr(GetValue(dr, columnName, nullValue))
    End Function
    Public Shared Function GetBool(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As Boolean) As Boolean
        Return CBool(GetValue(dr, columnName, nullValue))
    End Function
    Public Shared Function GetDec(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As Decimal) As Decimal
        Return CDec(GetValue(dr, columnName, nullValue))
    End Function
    Public Shared Function GetInt(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As Integer) As Integer
        Return CInt(GetValue(dr, columnName, nullValue))
    End Function
    Public Shared Function GetLong(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As Long) As Long
        Return CLng(GetValue(dr, columnName, nullValue))
    End Function
    Public Shared Function GetDbl(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As Double) As Double
        Return CDbl(GetValue(dr, columnName, nullValue))
    End Function
    Public Shared Function GetSingle(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As Single) As Single
        Return CType(GetValue(dr, columnName, nullValue), Single)
    End Function
    Public Shared Function GetDate(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As Date) As Date
        Dim useMethod As DGetDateByName = Nothing
        If m_customGetDateByName.TryGetValue(dr.GetType(), useMethod) Then Return useMethod(dr, columnName, nullValue)
        Return CDate(GetValue(dr, columnName, nullValue))
    End Function
    Public Shared Function GetByte(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As Byte) As Byte
        Return CType(GetValue(dr, columnName, nullValue), Byte)
    End Function
    Public Shared Function GetBytes(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As Byte()) As Byte()
        Return CType(GetValue(dr, columnName, nullValue), Byte())
    End Function
    Public Shared Function GetGuid(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As Guid) As Guid
        Return CType(GetValue(dr, columnName, nullValue), Guid)
    End Function


    'Column indexes, with auto defaults for NULL
    Public Shared Function GetStr(ByVal dr As IDataReader, ByVal columnIndex As Integer) As String
        Return GetStr(dr, columnIndex, Nothing)
    End Function
    Public Shared Function GetBool(ByVal dr As IDataReader, ByVal columnIndex As Integer) As Boolean
        Return GetBool(dr, columnIndex, NULL_BOOLEAN)
    End Function
    Public Shared Function GetDec(ByVal dr As IDataReader, ByVal columnIndex As Integer) As Decimal
        Return GetDec(dr, columnIndex, NULL_DECIMAL)
    End Function
    Public Shared Function GetInt(ByVal dr As IDataReader, ByVal columnIndex As Integer) As Integer
        Return GetInt(dr, columnIndex, NULL_INTEGER)
    End Function
    Public Shared Function GetLong(ByVal dr As IDataReader, ByVal columnIndex As Integer) As Long
        Return GetLong(dr, columnIndex, NULL_LONG)
    End Function
    Public Shared Function GetDbl(ByVal dr As IDataReader, ByVal columnIndex As Integer) As Double
        Return GetDbl(dr, columnIndex, NULL_DOUBLE)
    End Function
    Public Shared Function GetSingle(ByVal dr As IDataReader, ByVal columnIndex As Integer) As Single
        Return GetSingle(dr, columnIndex, NULL_SINGLE)
    End Function
    Public Shared Function GetDate(ByVal dr As IDataReader, ByVal columnIndex As Integer) As DateTime
        Return GetDate(dr, columnIndex, NULL_DATE)
    End Function
    Public Shared Function GetByte(ByVal dr As IDataReader, ByVal columnIndex As Integer) As Byte
        Return GetByte(dr, columnIndex, NULL_BYTE)
    End Function
    Public Shared Function GetBytes(ByVal dr As IDataReader, ByVal columnIndex As Integer) As Byte()
        Return GetBytes(dr, columnIndex, Nothing)
    End Function
    Public Shared Function GetGuid(ByVal dr As IDataReader, ByVal columnIndex As Integer) As Guid
        Return GetGuid(dr, columnIndex, NULL_GUID)
    End Function


    'Column indexes, with custom defaults for NULL
    Public Shared Function GetStr(ByVal dr As IDataReader, ByVal columnIndex As Integer, ByVal nullValue As String) As String
        Return CStr(GetValue(dr, columnIndex, nullValue))
    End Function
    Public Shared Function GetBool(ByVal dr As IDataReader, ByVal columnIndex As Integer, ByVal nullValue As Boolean) As Boolean
        Return CBool(GetValue(dr, columnIndex, nullValue))
    End Function
    Public Shared Function GetDec(ByVal dr As IDataReader, ByVal columnIndex As Integer, ByVal nullValue As Decimal) As Decimal
        Return CDec(GetValue(dr, columnIndex, nullValue))
    End Function
    Public Shared Function GetInt(ByVal dr As IDataReader, ByVal columnIndex As Integer, ByVal nullValue As Integer) As Integer
        Return CInt(GetValue(dr, columnIndex, nullValue))
    End Function
    Public Shared Function GetLong(ByVal dr As IDataReader, ByVal columnIndex As Integer, ByVal nullValue As Long) As Long
        Return CLng(GetValue(dr, columnIndex, nullValue))
    End Function
    Public Shared Function GetDbl(ByVal dr As IDataReader, ByVal columnIndex As Integer, ByVal nullValue As Double) As Double
        Return CDbl(GetValue(dr, columnIndex, nullValue))
    End Function
    Public Shared Function GetSingle(ByVal dr As IDataReader, ByVal columnIndex As Integer, ByVal nullValue As Single) As Single
        Return CType(GetValue(dr, columnIndex, nullValue), Single)
    End Function
    Public Shared Function GetDate(ByVal dr As IDataReader, ByVal columnIndex As Integer, ByVal nullValue As Date) As Date
        Dim useMethod As DGetDateByIndex = Nothing
        If m_customGetDateByIndex.TryGetValue(dr.GetType(), useMethod) Then Return useMethod(dr, columnIndex, nullValue)
        Return CDate(GetValue(dr, columnIndex, nullValue))
    End Function
    Public Shared Function GetByte(ByVal dr As IDataReader, ByVal columnIndex As Integer, ByVal nullValue As Byte) As Byte
        Return CType(GetValue(dr, columnIndex, nullValue), Byte)
    End Function
    Public Shared Function GetBytes(ByVal dr As IDataReader, ByVal columnIndex As Integer, ByVal nullValue As Byte()) As Byte()
        Return CType(GetValue(dr, columnIndex, nullValue), Byte())
    End Function
    Public Shared Function GetGuid(ByVal dr As IDataReader, ByVal columnIndex As Integer, ByVal nullValue As Guid) As Guid
        Dim obj As Object = GetValue(dr, columnIndex, nullValue)
        If TypeOf obj Is String Then Return New Guid(CStr(obj))
        Return CType(obj, Guid)
    End Function
#End Region

#Region "Shared - Nullable Types (Not normally used due to small performance hit)"
    'DataReader
    Public Shared Function GetIntNullable(ByVal dr As IDataReader, ByVal columnName As String) As Integer?
        Return CType(GetValue(dr, columnName, Nothing), Integer?)
    End Function
    Public Shared Function GetLongNullable(ByVal dr As IDataReader, ByVal columnName As String) As Long?
        Return CType(GetValue(dr, columnName, Nothing), Long?)
    End Function
    Public Shared Function GetDecNullable(ByVal dr As IDataReader, ByVal columnName As String) As Decimal?
        Return CType(GetValue(dr, columnName, Nothing), Decimal?)
    End Function
    Public Shared Function GetDateNullable(ByVal dr As IDataReader, ByVal columnName As String) As DateTime?
        Return CType(GetValue(dr, columnName, Nothing), DateTime?)
    End Function
    Public Shared Function GetDblNullable(ByVal dr As IDataReader, ByVal columnName As String) As Double?
        Return CType(GetValue(dr, columnName, Nothing), Double?)
    End Function
    Public Shared Function GetSingleNullable(ByVal dr As IDataReader, ByVal columnName As String) As Single?
        Return CType(GetValue(dr, columnName, Nothing), Single?)
    End Function
    Public Shared Function GetGuidNullable(ByVal dr As IDataReader, ByVal columnName As String) As Guid?
        Dim obj As Object = GetValue(dr, columnName, Nothing)
        If TypeOf obj Is String Then Return New Guid(CStr(obj))
        Return CType(obj, Guid?)
    End Function
    Public Shared Function GetBoolNullable(ByVal dr As IDataReader, ByVal columnName As String) As Boolean?
        Return CType(GetValue(dr, columnName, Nothing), Boolean?)
    End Function

    'DataSet
    Public Shared Function GetIntNullable(ByVal dr As DataRow, ByVal columnName As String) As Integer?
        Return CType(GetValue(dr, columnName, Nothing), Integer?)
    End Function
    Public Shared Function GetLongNullable(ByVal dr As DataRow, ByVal columnName As String) As Long?
        Return CType(GetValue(dr, columnName, Nothing), Long?)
    End Function
    Public Shared Function GetDecNullable(ByVal dr As DataRow, ByVal columnName As String) As Decimal?
        Return CType(GetValue(dr, columnName, Nothing), Decimal?)
    End Function
    Public Shared Function GetDateNullable(ByVal dr As DataRow, ByVal columnName As String) As DateTime?
        Return CType(GetValue(dr, columnName, Nothing), DateTime?)
    End Function
    Public Shared Function GetDblNullable(ByVal dr As DataRow, ByVal columnName As String) As Double?
        Return CType(GetValue(dr, columnName, Nothing), Double?)
    End Function
    Public Shared Function GetSingleNullable(ByVal dr As DataRow, ByVal columnName As String) As Single?
        Return CType(GetValue(dr, columnName, Nothing), Single?)
    End Function
    Public Shared Function GetGuidNullable(ByVal dr As DataRow, ByVal columnName As String) As Guid?
        Dim obj As Object = GetValue(dr, columnName, Nothing)
        If TypeOf obj Is String Then Return New Guid(CStr(obj))
        Return CType(obj, Guid?)
    End Function
    Public Shared Function GetBoolNullable(ByVal dr As DataRow, ByVal columnName As String) As Boolean?
        Return CType(GetValue(dr, columnName, Nothing), Boolean?)
    End Function
#End Region

#Region "Shared (Private) - Get Data From DataReader/DataRow"
    Public Shared Function GetValue(ByVal dr As DataRow, ByVal columnName As String, ByVal nullValue As Object) As Object
        Dim index As Integer = dr.Table.Columns.IndexOf(columnName)
        If index = -1 Then Return nullValue
        Return GetValue(dr, index, nullValue)
    End Function
    Public Shared Function GetValue(ByVal dr As DataRow, ByVal columnNumber As Integer, ByVal nullValue As Object) As Object
        GetValue = dr(columnNumber)
        If TypeOf (GetValue) Is System.DBNull Then Return nullValue
        If IsNothing(GetValue) Then Return nullValue
    End Function
    Public Shared Function GetValue(ByVal dr As IDataReader, ByVal columnName As String, ByVal nullValue As Object) As Object
        Dim i As Integer = GetOrdinal(dr, columnName)

        If dr.IsDBNull(i) Then Return nullValue

        Try
            Return dr.Item(i)
        Catch ex As System.Exception
            Throw New Exception("Invalid column name/number: " & columnName & " (" & ex.Message & ")")
        End Try
    End Function
    Public Shared Function GetValue(ByVal dr As IDataReader, ByVal columnNumber As Integer, ByVal nullValue As Object) As Object
        If dr.IsDBNull(columnNumber) Then Return nullValue
        Return dr.Item(columnNumber)
    End Function
    Public Shared Function GetOrdinal(ByVal dr As IDataReader, ByVal columnName As String) As Integer
        Try
            Return dr.GetOrdinal(columnName)
        Catch ex As IndexOutOfRangeException
            'Might be a column number
            Dim i As Integer
            If Integer.TryParse(columnName, i) Then Return i

            'Or a CSV file with whitespace around the column name
            For i = 0 To dr.FieldCount - 1
                If dr.GetName(i).Trim.ToLower = columnName.Trim.ToLower Then Return i
            Next

            Throw New Exception("Invalid columnName '" & columnName & "'.")
        End Try
    End Function
#End Region

End Class
