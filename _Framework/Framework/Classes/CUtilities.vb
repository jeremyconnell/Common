Imports System.Collections

Public Class CUtilities

#Region "Count Description"
    Public Shared Function FolderSizeAsStr(path As String) As String
        Return FileSize(FolderSize(path))
    End Function
    Public Shared Function FolderSize(path As String) As Long
        Return FolderSize(New IO.DirectoryInfo(path))
    End Function
    Public Shared Function FolderSize(d As IO.DirectoryInfo) As Long
        Dim size As Long = 0
        For Each i As IO.FileInfo In d.GetFiles
            size += i.Length
        Next
        For Each i As IO.DirectoryInfo In d.GetDirectories
            size += FolderSize(i)
        Next
        Return size
    End Function

    Public Shared Function FileNameAndSize(name As String, ByVal bin As Byte()) As String
        Return String.Concat(name, " (", FileSize(bin), ")")
    End Function
    Public Shared Function FileNameAndSize(name As String, ByVal size As Long) As String
        Return String.Concat(name, " (", FileSize(size), ")")
    End Function
    Public Shared Function FileSize(ByVal filePath As String) As String
        If Not IO.File.Exists(filePath) Then Return String.Empty
        Return FileSize(New IO.FileInfo(filePath).Length)
    End Function
    Public Shared Function FileSize(ByVal bin As Byte()) As String
        Return FileSize(bin.Length)
    End Function
    Public Shared Function FileSize(ByVal size As Integer) As String
        Return FileSize(CLng(size))
    End Function
    Public Shared Function FileSize(ByVal size As Decimal) As String
        Return FileSize(CLng(size))
    End Function
    Public Shared Function FileSize(ByVal size As Long) As String
        If size > 1000000000 Then Return CDbl(size / 1000000000).ToString("f2") & "Gb"
        If size > 100000000 Then Return CDbl(size / 1000000).ToString("f0") & "Mb"
        If size > 10000000 Then Return CDbl(size / 1000000).ToString("f1") & "Mb"
        If size > 1000000 Then Return CDbl(size / 1000000).ToString("f2") & "Mb"
        If size > 100000 Then Return CDbl(size / 1000).ToString("f0") & "kB"
        If size > 10000 Then Return CDbl(size / 1000).ToString("f1") & "kB"
        If size > 1000 Then Return CInt(size / 1000).ToString("f2") & "kB"
        Return size & "B"
    End Function

    Public Shared Function CountSummary(ByVal list As IList, ByVal entityName As String) As String
        Return CountSummary(list, entityName, String.Empty)
    End Function
    Public Shared Function CountSummary(ByVal list As IList, ByVal entityName As String, ByVal zeroCase As String) As String
        Return CountSummary(list.Count, entityName, zeroCase)
    End Function
    Public Shared Function CountSummary(ByVal list As IList, ByVal entityName As String, ByVal zeroCase As String, ByVal plural As String) As String
        Return CountSummary(list.Count, entityName, zeroCase, plural)
    End Function

    Public Shared Function ToUnixDate(d As DateTime) As Integer
        Return CInt((d.ToUniversalTime - New DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds)
    End Function

    Public Shared Function ToUnixDate_(d As DateTime) As Long
        Return CLng((d.ToUniversalTime - New DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds)
    End Function


    Public Shared Function FromUnix(d As Long) As DateTime
        Return New DateTime(d * 10000000 + New DateTime(1970, 1, 1, 0, 0, 0).Ticks).ToLocalTime
    End Function
    Public Shared Function FromUnix_(d As Long) As DateTime
        Return New DateTime(d * 10000 + New DateTime(1970, 1, 1, 0, 0, 0).Ticks).ToLocalTime
    End Function

    Public Shared Function CountSummary(ByVal count As Integer, ByVal entityName As String) As String
        Return CountSummary(count, entityName, String.Empty)
    End Function
    Public Shared Function CountSummary(ByVal count As Integer, ByVal entityName As String, ByVal zeroCase As String) As String
        'Guess the plural
        Dim plural As String = CStr(IIf(IsNothing(entityName), String.Empty, entityName))
        If Not String.IsNullOrEmpty(plural) AndAlso Not plural.EndsWith("s") Then
            If plural.EndsWith("y") Then
                plural = String.Concat(plural.Substring(0, plural.Length - 1), "ies")
            Else
                plural = String.Concat(plural, "s")
            End If
        End If
        Return CountSummary(count, entityName, zeroCase, plural)
    End Function
    Public Shared Function CountSummary(ByVal count As Integer, ByVal entityName As String, ByVal zeroCase As String, ByVal plural As String) As String
        If String.IsNullOrEmpty(zeroCase) Then zeroCase = String.Concat("no ", plural)
        Select Case count
            Case 0, Integer.MinValue : Return zeroCase
            Case 1 : Return String.Concat("1 ", entityName).Trim()
            Case Else : Return String.Concat(count.ToString("n0"), " ", plural).Trim()
        End Select
    End Function
    Public Shared Function CountSummary(ByVal count As Long, ByVal entityName As String) As String
        Return CountSummary(count, entityName, String.Empty)
    End Function
    Public Shared Function CountSummary(ByVal count As Long, ByVal entityName As String, ByVal zeroCase As String) As String
        'Guess the plural
        Dim plural As String = CStr(IIf(IsNothing(entityName), String.Empty, entityName))
        If Not String.IsNullOrEmpty(plural) AndAlso Not plural.EndsWith("s") Then
            If plural.EndsWith("y") Then
                plural = String.Concat(plural.Substring(0, plural.Length - 1), "ies")
            Else
                plural = String.Concat(plural, "s")
            End If
        End If
        Return CountSummary(count, entityName, zeroCase, plural)
    End Function
    Public Shared Function CountSummary(ByVal count As Long, ByVal entityName As String, ByVal zeroCase As String, ByVal plural As String) As String
        If String.IsNullOrEmpty(zeroCase) Then zeroCase = String.Concat("no ", plural)
        Select Case count
            Case 0, Long.MinValue : Return zeroCase
            Case 1 : Return String.Concat("1 ", entityName).Trim()
            Case Else : Return String.Concat(count.ToString("n0"), " ", plural).Trim()
        End Select
    End Function
    Public Shared Function NameAndCount(ByVal name As String, ByVal dict As IDictionary) As String
        Return NameAndCount(name, dict, String.Empty)
    End Function
    Public Shared Function NameAndCount(ByVal name As String, ByVal list As IList) As String
        Return NameAndCount(name, list, String.Empty)
    End Function
    Public Shared Function NameAndCount(ByVal name As String, ByVal count As Integer) As String
        Return NameAndCount(name, count, String.Empty)
    End Function
    Public Shared Function NameAndCount(ByVal name As String, ByVal dict As IDictionary, ByVal childEntityName As String) As String
        Return NameAndCount(name, dict.Count, childEntityName)
    End Function
    Public Shared Function NameAndCount(ByVal name As String, ByVal list As IList, ByVal childEntityName As String) As String
        Return NameAndCount(name, list.Count, childEntityName)
    End Function
    Public Shared Function NameAndCount(ByVal name As String, ByVal count As Integer, ByVal childEntityName As String) As String
        Return NameAndCount(name, count, childEntityName, String.Empty)
    End Function
    Public Shared Function NameAndCount(ByVal name As String, ByVal list As IList, ByVal childEntityName As String, ByVal zeroCase As String) As String
        Return NameAndCount(name, list.Count, childEntityName, zeroCase)
    End Function
    Public Shared Function NameAndCount(ByVal name As String, ByVal count As Integer, ByVal childEntityName As String, ByVal zeroCase As String) As String
        If count = 0 OrElse count = Integer.MinValue Then
            If String.IsNullOrEmpty(zeroCase) Then Return name
            If String.IsNullOrEmpty(childEntityName) Then Return String.Concat(name, " (", zeroCase, ")")
        End If
        Return String.Concat(name, " (", CountSummary(count, childEntityName, zeroCase), ")")
    End Function
#End Region

#Region "Paging"
    Public Shared Function Page_(ByVal list As IList, ByVal size As Integer, ByVal index As Integer) As IList
        If 0 = index AndAlso list.Count <= size Then Return New ArrayList(list)

        If index < 0 Then index = 0
        If (index - 1) * size > list.Count Then Return New ArrayList()

        Dim minIndex As Integer = size * index
        Dim maxIndex As Integer = size * (index + 1) - 1
        If maxIndex >= list.Count Then maxIndex = list.Count - 1

        Dim subset As New List(Of Object)(size)
        For i As Integer = minIndex To maxIndex Step 1
            subset.Add(list(i))
        Next
        Return subset
    End Function
    Public Shared Function Page(Of T)(ByVal list As IList(Of T), ByVal size As Integer, ByVal index As Integer) As List(Of T)
        If 0 = index AndAlso list.Count <= size Then Return New List(Of T)(list)

        If index < 0 Then index = 0
        If (index - 1) * size > list.Count Then Return New List(Of T)(0)

        Dim minIndex As Integer = size * index
        Dim maxIndex As Integer = size * (index + 1) - 1
        If maxIndex >= list.Count Then maxIndex = list.Count - 1

        Dim subset As New List(Of T)(size)
        For i As Integer = minIndex To maxIndex Step 1
            subset.Add(list(i))
        Next
        Return subset
    End Function
#End Region

#Region "Truncation"
    Public Shared Function Truncate(ByVal original As String) As String
        Return Truncate(original, 30)
    End Function
    Public Shared Function Truncate(ByVal original As String, ByVal maxLength As Integer) As String
        If String.IsNullOrEmpty(original) Then Return String.Empty
        If maxLength < 1 Then Return original
        If maxLength < 3 Then maxLength = 3
        If original.Length <= maxLength Then Return original
        Return original.Substring(0, maxLength - 3) & "..."
    End Function
#End Region

#Region "Saving Files - UniqueName"
    'Modifies fileName slightly until its unique
    Public Shared Function UniqueFileName(ByVal folderPath As String, ByVal fileName As String) As String
        If String.IsNullOrEmpty(fileName) Then Return String.Empty
        fileName = IO.Path.GetFileName(fileName)

        While IO.File.Exists(String.Concat(folderPath, "\", fileName))
            fileName = UniqueNameGuess(fileName)
        End While
        Return fileName
    End Function
    'Suggests a friendly fileName that might be unique
    Private Shared Function UniqueNameGuess(ByVal fileName As String) As String
        Dim extension As String = IO.Path.GetExtension(fileName)
        Dim baseName As String = fileName.Substring(0, fileName.LastIndexOf(extension))

        If baseName.Length = 0 Then Return "_" & baseName

        Dim len As Integer = baseName.Length
        Dim suffix As String = String.Concat("(", 1, ")", extension)
        If ")" <> baseName.Substring(len - 1, 1) Then Return String.Concat(baseName, suffix)

        Dim startAt As Integer = baseName.LastIndexOf("(")
        If -1 = startAt Then Return baseName + suffix

        Dim number As String = baseName.Substring(startAt + 1, len - startAt - 2)
        Try
            Dim nextNumber As Integer = Integer.Parse(number) + 1
            Return String.Concat(baseName.Substring(0, startAt), "(", nextNumber, ")", extension)
        Catch
            Return String.Concat(baseName, suffix)
        End Try
    End Function
#End Region

#Region "Date Formats"
    Public Shared Function LongDateTime(ByVal d As DateTime) As String
        Return LongDateTime(d, "ddd d", " MMM yyyy h:mm:ss tt").Replace(" AM", "am").Replace(" PM", "pm")
    End Function
    Public Shared Function LongDate(ByVal d As DateTime) As String
        Return LongDate(d, "ddd d", " MMM yyyy")
    End Function
    Public Shared Function ShortDate(ByVal d As DateTime) As String
        If DateTime.MinValue = d Then Return String.Empty
        Return d.ToString("d-MMM-yyyy")
    End Function
    Public Shared Function ShortDateTime(ByVal d As DateTime) As String
        If DateTime.MinValue = d Then Return String.Empty
        Return d.ToString("d-MMM-yyyy h:mm tt")
    End Function
    Public Shared Function LongTime(ByVal d As DateTime) As String
        If DateTime.MinValue = d Then Return String.Empty
        Return d.ToString("h:mm:ss tt").Replace(" AM", "am").Replace(" PM", "pm")
    End Function
    Public Shared Function LongDateTime(ByVal d As DateTime, ByVal beforeFormat As String, ByVal afterFormat As String) As String
        If DateTime.MinValue = d Then Return String.Empty
        Return String.Concat(d.ToString(beforeFormat), NumberSuffix(d.Day), d.ToString(afterFormat))
    End Function
    Public Shared Function LongDate(ByVal d As DateTime, ByVal beforeFormat As String, ByVal afterFormat As String) As String
        If DateTime.MinValue = d Then Return String.Empty
        Return String.Concat(d.ToString(beforeFormat), NumberSuffix(d.Day), d.ToString(afterFormat))
    End Function
    Public Shared Function NumberSuffix(ByVal i As Integer) As String
        Select Case i
            Case 1 : Return "st"
            Case 2 : Return "nd"
            Case 3 : Return "rd"
            Case 4 : Return "th"
            Case Else : Return String.Empty
        End Select
    End Function

    Private Const DAYS_IN_YEAR As Integer = 365
    Private Const MONTHS_IN_YEAR As Integer = 12
    Private Const DAYS_IN_MONTH As Double = CDbl(DAYS_IN_YEAR) / CDbl(MONTHS_IN_YEAR)
    Public Shared Function TimespanShort(ByVal d As DateTime) As String
        Return TimespanShort(DateTime.Now.Subtract(d))
    End Function
    Public Shared Function TimespanShort(ByVal t As TimeSpan) As String
        If t.Milliseconds < 500 Then
            Return Timespan(t.Subtract(New TimeSpan(0, 0, 0, 0, t.Milliseconds))).Replace(" mins", "m").Replace(" min", "m").Replace(" secs", "s").Replace(" sec", "s").Replace(" hrs", "h").Replace(" hr", "h") 'Remove Ms (up)
        Else
            Return Timespan(t.Add(New TimeSpan(0, 0, 0, 0, 1000 - t.Milliseconds))).Replace(" mins", "m").Replace(" min", "m").Replace(" secs", "s").Replace(" sec", "s").Replace(" hrs", "h").Replace(" hr", "h")
        End If
    End Function
    Public Shared Function TimespanMs(ByVal i As Long) As String
        If Long.MinValue = i Then Return String.Empty
        Return Timespan(System.TimeSpan.FromMilliseconds(i))
    End Function
    Public Shared Function TimespanMs(ByVal i As Integer) As String
        If Integer.MinValue = i Then Return String.Empty
        Return Timespan(System.TimeSpan.FromMilliseconds(i))
    End Function
    Public Shared Function TimespanMs(ByVal i As Double) As String
        If Double.IsNaN(i) Then Return String.Empty
        Return Timespan(System.TimeSpan.FromMilliseconds(i))
    End Function
    Public Shared Function Timespan(ByVal d As DateTime, Optional agoOrTime As Boolean = True) As String
        If agoOrTime Then Return Timespan(d, " ago", " time")
        Return Timespan(DateTime.Now.Subtract(d))
    End Function
    Public Shared Function Timespan(ByVal d As DateTime, ByVal suffixIfInPast As String, ByVal suffixIfInFuture As String) As String
        If DateTime.MinValue = d Then Return String.Empty
        If d > DateTime.Now Then Return Timespan(d.Subtract(DateTime.Now)) & suffixIfInFuture
        Return Timespan(DateTime.Now.Subtract(d)) & suffixIfInPast
    End Function
    Public Shared Function Timespan(ByVal t As TimeSpan) As String
        Return TimespanLogic(t).Trim.Replace("  ", " ")
    End Function
    Private Shared Function TimespanLogic(ByVal t As TimeSpan) As String
        If System.TimeSpan.MinValue = t Then Return String.Empty
        Dim sb As New System.Text.StringBuilder

        If t.Ticks < 0 Then
            t = -t
            sb.Append("-")
        End If


        Dim days As Integer = CInt(Math.Floor(t.TotalDays))
        Dim years As Integer = CInt(Math.Floor(days / DAYS_IN_YEAR))
        If years > 0 Then
            sb.Append(CountSummary(years, "year"))
            days -= 365 * years
        End If
        Dim months As Integer = CInt(Math.Floor(days / DAYS_IN_MONTH))
        If months > 0 Then
            If sb.Length > 0 Then sb.Append(" ")
            sb.Append(CountSummary(months, "month"))
            days -= CInt(Math.Floor(DAYS_IN_MONTH * months))
        End If
        If t.TotalDays > DAYS_IN_YEAR Then Return sb.ToString 'Years, months
        If days > 0 Then sb.Append(" ").Append(CountSummary(days, " day", "", "days"))
        If months > 0 Then Return sb.ToString 'Months, days
        If t.Hours > 0 Then sb.Append(" ").Append(CountSummary(t.Hours, " hr"))
        If t.TotalDays > 1 Then Return sb.ToString 'Days, hours
        If t.Minutes > 0 Then sb.Append(" ").Append(CountSummary(t.Minutes, " min"))
        If t.TotalHours > 1 Then Return sb.ToString 'Hours, Mins
        If t.Seconds > 0 Then sb.Append(" ").Append(CountSummary(t.Seconds, " sec"))
        If t.TotalMinutes > 1 Then Return sb.ToString 'Mins, Secs
        If t.Milliseconds > 0 Then sb.Append(" ").Append(t.Milliseconds).Append(" msec")
        Return sb.ToString() 'Secs, Ms
    End Function
#End Region

#Region "Split"
    Public Shared Function SplitOn(ByVal s As String, ByVal lookFor As String) As List(Of String)
        Return SplitOn(s, lookFor, False)
    End Function
    Public Shared Function SplitOn(ByVal s As String, ByVal lookFor As String, ByVal caseSensitive As Boolean) As List(Of String)
        Dim list As New List(Of String)()

        'Trivial checks
        If String.IsNullOrEmpty(lookFor) Then
            list.Add(s)
            Return list
        End If

        Return SplitOn(s, lookFor, caseSensitive, list)
    End Function
    Private Shared Function SplitOn(ByVal s As String, ByVal lookFor As String, ByVal caseSensitive As Boolean, ByVal list As List(Of String)) As List(Of String)
        'Trivial checks
        If String.IsNullOrEmpty(s) Then Return list

        'Split once and recurse
        Dim index As Integer = s.IndexOf(lookFor)
        If index = -1 And Not caseSensitive Then index = s.ToLower.IndexOf(lookFor.ToLower)
        If index = -1 Then
            list.Add(s)
            Return list
        End If

        'First bit - add to list
        list.Add(s.Substring(0, index))

        'Last bit - recurse
        Return SplitOn(s.Substring(index + lookFor.Length), lookFor, caseSensitive, list)
    End Function
    Public Shared Function ReplaceAll(ByVal s As String, ByVal lookFor As String, ByVal replaceWith As String) As String
        'eg. "blah---blah" => "blah-blah" can be achieved with ReplaceAll(s, "--", "-")
        If String.IsNullOrEmpty(s) Then Return String.Empty
        While s.Contains(lookFor)
            s = s.Replace(lookFor, replaceWith)
        End While
        Return s
    End Function
#End Region

#Region "Comma-Separated"
    Private Const DELIMITER As String = ","

    'Integers
    Public Shared Function StringToListInt(ByVal s As String) As List(Of Integer)
        Return StringToListInt(s, DELIMITER)
    End Function
    Public Shared Function StringToListInt(ByVal s As String, ByVal delim As String) As List(Of Integer)
        If String.IsNullOrEmpty(s) OrElse Trim(s).Length = 0 Then Return New List(Of Integer)(0)
        Dim ss As String() = s.Split(delim.ToCharArray())
        Dim list As New List(Of Integer)(ss.Length)
        Dim int As Integer
        For Each i As String In ss
            If Integer.TryParse(i, int) Then list.Add(int)
        Next
        Return list
    End Function

    'Big Integers
    Public Shared Function StringToListLong(ByVal s As String) As List(Of Long)
        Return StringToListLong(s, DELIMITER)
    End Function
    Public Shared Function StringToListLong(ByVal s As String, ByVal delim As String) As List(Of Long)
        If String.IsNullOrEmpty(s) OrElse Trim(s).Length = 0 Then Return New List(Of Long)(0)
        Dim ss As String() = s.Split(delim.ToCharArray())
        Dim list As New List(Of Long)(ss.Length)
        Dim int As Long
        For Each i As String In ss
            If Long.TryParse(i, int) Then list.Add(int)
        Next
        Return list
    End Function

    'Doubles
    Public Shared Function StringToListDbl(ByVal s As String) As List(Of Double)
        Return StringToListDbl(s, DELIMITER)
    End Function
    Public Shared Function StringToListDbl(ByVal s As String, ByVal delim As String) As List(Of Double)
        If String.IsNullOrEmpty(s) OrElse Trim(s).Length = 0 Then Return New List(Of Double)(0)
        Dim ss As String() = s.Split(delim.ToCharArray())
        Dim list As New List(Of Double)(ss.Length)
        Dim int As Double
        For Each i As String In ss
            If Double.TryParse(i, int) Then list.Add(int)
        Next
        Return list
    End Function

    'Dates
    Public Shared Function StringToListDate(ByVal s As String) As List(Of DateTime)
        Return StringToListDate(s, DELIMITER)
    End Function
    Public Shared Function StringToListDate(ByVal s As String, ByVal delim As String) As List(Of DateTime)
        If String.IsNullOrEmpty(s) OrElse Trim(s).Length = 0 Then Return New List(Of DateTime)(0)
        Dim ss As String() = s.Split(delim.ToCharArray())
        Dim list As New List(Of DateTime)(ss.Length)
        Dim int As DateTime
        For Each i As String In ss
            If DateTime.TryParse(i, int) Then list.Add(int)
        Next
        Return list
    End Function

    'Guids
    Public Shared Function StringToListGuid(ByVal s As String) As List(Of Guid)
        Return StringToListGuid(s, DELIMITER)
    End Function
    Public Shared Function StringToListGuid(ByVal s As String, ByVal delim As String) As List(Of Guid)
        If String.IsNullOrEmpty(s) OrElse Trim(s).Length = 0 Then Return New List(Of Guid)(0)
        Dim ss As String() = s.Split(delim.ToCharArray())
        Dim list As New List(Of Guid)(ss.Length)
        For Each i As String In ss
            If i.Length <> 36 Then i = i.Substring(0, 36)
            Try
                list.Add(New Guid(i))
            Catch
            End Try
        Next
        Return list
    End Function

    'Strings
    Public Shared Function StringToListStr(ByVal s As String) As List(Of String)
        Return StringToListStr(s, DELIMITER)
    End Function
    Public Shared Function StringToListStr(ByVal s As String, ByVal delim As String) As List(Of String)
        If String.IsNullOrEmpty(s) OrElse Trim(s).Length = 0 Then Return New List(Of String)(0)
        Dim ss As String() = s.Split(delim.ToCharArray())
        Dim list As New List(Of String)(ss.Length)
        For Each i As String In ss
            list.Add(Trim(i))
        Next
        Return list
    End Function

    'General
    Public Shared Function ListToString(ByVal list As ICollection, Optional ByVal delimiter As String = DELIMITER) As String
        If IsNothing(list) Then Return String.Empty
        Dim sb As New System.Text.StringBuilder
        For Each i As Object In list
            If sb.Length > 0 Then sb.Append(delimiter)
            sb.Append(i)
        Next
        Return sb.ToString
    End Function


    'Html
    Public Shared Function ListToHtml(ByVal list As IList) As String
        Return ListToHtml(list, "<br/>")
    End Function
    Public Shared Function ListToHtml(ByVal list As IList, ByVal delimiter As String) As String
        Dim sb As New System.Text.StringBuilder
        For Each i As Object In list
            If sb.Length > 0 Then sb.Append(delimiter)
            sb.Append(System.Web.HttpUtility.HtmlEncode(i.ToString()))
        Next
        Return sb.ToString
    End Function
#End Region

#Region "Zero-Pad"
    Public Shared Function ZeroPad(ByVal int As Integer, ByVal requiredLength As Integer) As String
        Return ZeroPad(int.ToString(), requiredLength)
    End Function
    Public Shared Function ZeroPad(ByVal int As String, ByVal requiredLength As Integer) As String
        If int.Length > requiredLength Then Throw New Exception(String.Concat("CUtilities.ZeroPad: required length of ", requiredLength, " already exceeded for '", int, "'"))
        Dim sb As New System.Text.StringBuilder(int)
        While sb.Length < requiredLength
            sb.Insert(0, "0")
        End While
        Return sb.ToString
    End Function
#End Region

#Region "Numerical Partitioning"
    Public Const LT As String = "less than {0}"
    Public Const BT As String = "{0} to {1}"
    Public Const GT As String = "greater than {0}"

    Public Shared Function Partition(ByVal value As Integer, ByVal p As List(Of Integer)) As String
        Return Partition(value, p, LT, BT, GT)
    End Function
    Public Shared Function Partition(ByVal value As Double, ByVal p As List(Of Double)) As String
        Return Partition(value, p, LT, BT, GT)
    End Function
    Public Shared Function Partition(ByVal value As Decimal, ByVal p As List(Of Decimal)) As String
        Return Partition(value, p, LT, BT, GT)
    End Function
    Public Shared Function Partition(ByVal value As DateTime, ByVal p As List(Of DateTime)) As String
        Return Partition(value, p, LT, BT, GT)
    End Function

    Public Shared Function Partition(ByVal value As Integer, ByVal p As List(Of Integer), ByVal lessThan As String, ByVal between As String, ByVal greaterThan As String) As String
        If p.Count = 0 Then Return value.ToString()
        p.Sort()
        If value < p(0) Then Return String.Format(lessThan, p(0))
        Dim i As Integer
        For i = 1 To p.Count - 1
            If value >= p(i - 1) And value < p(i) Then
                Return String.Format(between, p(i - 1), p(i))
            End If
        Next
        Return String.Format(greaterThan, p(p.Count - 1))
    End Function
    Public Shared Function Partition(ByVal value As Double, ByVal p As List(Of Double), ByVal lessThan As String, ByVal between As String, ByVal greaterThan As String) As String
        If p.Count = 0 Then Return value.ToString()
        p.Sort()
        If value < p(0) Then Return String.Format(lessThan, p(0))
        Dim i As Integer
        For i = 1 To p.Count - 1
            If value >= p(i - 1) And value < p(i) Then
                Return String.Format(between, p(i - 1), p(i))
            End If
        Next
        Return String.Format(greaterThan, p(p.Count - 1))
    End Function
    Public Shared Function Partition(ByVal value As Decimal, ByVal p As List(Of Decimal), ByVal lessThan As String, ByVal between As String, ByVal greaterThan As String) As String
        If p.Count = 0 Then Return value.ToString()
        p.Sort()
        If value < p(0) Then Return String.Format(lessThan, p(0))
        Dim i As Integer
        For i = 1 To p.Count - 1
            If value >= p(i - 1) And value < p(i) Then
                Return String.Format(between, p(i - 1), p(i))
            End If
        Next
        Return String.Format(greaterThan, p(p.Count - 1))
    End Function
    Public Shared Function Partition(ByVal value As DateTime, ByVal p As List(Of DateTime), ByVal lessThan As String, ByVal between As String, ByVal greaterThan As String) As String
        If p.Count = 0 Then Return value.ToString()
        p.Sort()
        If value < p(0) Then Return String.Format(lessThan, p(0))
        Dim i As Integer
        For i = 1 To p.Count - 1
            If value >= p(i - 1) And value < p(i) Then
                Return String.Format(between, p(i - 1), p(i))
            End If
        Next
        Return String.Format(greaterThan, p(p.Count - 1))
    End Function

#End Region

#Region "Moving Files/Folders (across-drives)"
    Public Shared Sub CopyDir(ByVal fromPath As String, ByVal toPath As String, Optional ByVal deleteExisting As Boolean = False)
        If deleteExisting AndAlso IO.Directory.Exists(toPath) Then IO.Directory.Delete(toPath, True)
        If Not IO.Directory.Exists(toPath) Then IO.Directory.CreateDirectory(toPath)
        For Each i As String In IO.Directory.GetFiles(fromPath)
            Dim toFile As String = String.Concat(toPath, "\", i.Substring(fromPath.Length))
            If Not IO.File.Exists(toFile) Then IO.File.Copy(i, toFile)
        Next
        For Each i As String In IO.Directory.GetDirectories(fromPath)
            CopyDir(i, String.Concat(toPath, "\", i.Substring(fromPath.Length)))
        Next
    End Sub
    Public Shared Sub MoveDir(ByVal fromPath As String, ByVal toPath As String)
        If IO.Directory.Exists(toPath) Then IO.Directory.Delete(toPath, True)
        IO.Directory.CreateDirectory(toPath)
        For Each i As String In IO.Directory.GetFiles(fromPath)
            Dim toFile As String = String.Concat(toPath, "\", i.Substring(fromPath.Length))
            MoveFile(i, toFile)
        Next
        For Each i As String In IO.Directory.GetDirectories(fromPath)
            MoveDir(i, String.Concat(toPath, "\", i.Substring(fromPath.Length)))
        Next
        IO.Directory.Delete(fromPath)
    End Sub
    Public Shared Sub MoveFile(ByVal fromPath As String, ByVal toPath As String)
        If IO.File.Exists(toPath) Then IO.File.Delete(toPath)
        If IO.Directory.Exists(toPath) Then IO.Directory.Delete(toPath, True)
        IO.File.Copy(fromPath, toPath)
        IO.File.Delete(fromPath) 'cannot move across volumes apparently
    End Sub
#End Region

End Class
