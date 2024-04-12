Imports System.Text
Imports System.Text.ASCIIEncoding
Imports System.IO
Imports System.IO.Compression
Imports System.Collections.Generic
Imports System.Security.Cryptography
Imports System.Configuration.ConfigurationManager
Imports System.Runtime.Serialization.Formatters.Binary

Public Class CBinary

#Region "Shared - Serialization"
    'Object to Stream
    Public Shared Function Deserialise(ByVal data As Stream) As Object
        Dim bf As New BinaryFormatter
        Deserialise = bf.Deserialize(data)
        data.Close()
    End Function
    Public Shared Function Serialise(ByVal data As Object) As Stream
        Dim ms As New IO.MemoryStream
        Dim bf As New BinaryFormatter
        bf.Serialize(ms, data)
        Return ms
    End Function

    'Object to Bytes
    Public Shared Function SerialiseToBytes(ByVal data As Object) As Byte()
        Dim ms As New IO.MemoryStream
        Dim bf As New BinaryFormatter
        bf.Serialize(ms, data)
        SerialiseToBytes = ms.ToArray()
        ms.Close()
    End Function
    Public Shared Function DeserialiseFromBytes(ByVal bytes As Byte()) As Object
        Dim ms As New IO.MemoryStream(bytes)
        Dim bf As New BinaryFormatter
        DeserialiseFromBytes = bf.Deserialize(ms)
        ms.Close()
    End Function

    'Object to File
    Public Shared Sub SerialiseToFile(ByVal filePath As String, ByVal data As Object)
        Dim bf As New BinaryFormatter
        Dim fs As IO.FileStream = IO.File.Create(filePath)
        bf.Serialize(fs, data)
        fs.Close()
    End Sub
    Public Shared Function DeserialiseFromFile(ByVal filePath As String) As Object
        Dim bf As New BinaryFormatter
        Dim fs As IO.FileStream = IO.File.OpenRead(filePath)
        DeserialiseFromFile = bf.Deserialize(fs)
        fs.Close()
    End Function

    'Object to zip
    Public Shared Function SerialiseToBytesAndZip(ByVal data As Object) As Byte()
        Return Zip(SerialiseToBytes(data))
    End Function
    Public Shared Function DeserialiseFromBytesAndUnzip(ByVal bytes As Byte()) As Object
        Return DeserialiseFromBytes(Unzip(bytes))
    End Function

    'Object to file
    Public Shared Sub SerialiseToBytesAndZip(ByVal data As Object, ByVal filePath As String)
        IO.File.WriteAllBytes(filePath, SerialiseToBytesAndZip(data))
    End Sub
    Public Shared Function DeserialiseFromBytesAndUnzip(ByVal filePath As String) As Object
        Return DeserialiseFromBytesAndUnzip(IO.File.ReadAllBytes(filePath))
    End Function

    'Object encrypted
    Public Shared Function SerialiseAndEncrypt(ByVal data As Object) As Byte()
        Return SerialiseAndEncrypt(data, EncryptionProvider)
    End Function
    Public Shared Function DeserialiseAndDecrypt(ByVal bytes As Byte()) As Object
        Return DeserialiseAndDecrypt(bytes, EncryptionProvider)
    End Function
    Public Shared Function SerialiseAndEncrypt(ByVal data As Object, ByVal provider As SymmetricAlgorithm) As Byte()
        Return Encrypt(SerialiseToBytes(data), provider)
    End Function
    Public Shared Function DeserialiseAndDecrypt(ByVal bytes As Byte(), ByVal provider As SymmetricAlgorithm) As Object
        Return DeserialiseFromBytes(Decrypt(bytes, provider))
    End Function
#End Region

#Region "GZip/Ungzip"
    Public Shared Function Zip(ByVal input As Byte()) As Byte()
        Dim memStr As New MemoryStream
        Dim zipper As New GZipStream(memStr, CompressionMode.Compress)
        Dim bw As New BinaryWriter(zipper)
        bw.Write(input)
        bw.Close()
        zipper.Dispose()
        Return memStr.ToArray()
    End Function
    Public Shared Function Unzip(ByVal input As Byte()) As Byte()
        Dim memStr As New MemoryStream(input)
        Dim unzipper As New GZipStream(memStr, CompressionMode.Decompress)
        Dim br As New BinaryReader(unzipper)

        Dim output As New List(Of Byte)(4096)
        Dim buffer As Byte() = Nothing
        While True
            buffer = br.ReadBytes(4096)
            If buffer.Length = 0 Then Exit While
            output.AddRange(buffer)
        End While
        br.Close()
        unzipper.Dispose()
        Return output.ToArray()
    End Function
#End Region

#Region "Bytes/String"
    Public Shared Function BytesToStringVb(ByVal binary As Byte()) As String
        Dim c As Char(), i As Integer, bound As Integer = UBound(binary)
        ReDim c(bound)
        For i = 0 To bound
            c(i) = Chr(binary(i))
        Next
        Return New String(c)
    End Function
    Public Shared Function BytesToString(ByVal binary As Byte()) As String
        Return ASCII.GetString(binary)
    End Function
    Public Shared Function StringToBytes(ByVal s As String) As Byte()
        Return ASCII.GetBytes(s)
    End Function
#End Region

#Region "Bytes/Hex"
    Public Shared Function BytesToHex(ByVal data As Byte()) As String
        Return BitConverter.ToString(data).Replace("-", "")
    End Function
    Public Shared Function HexToBytes(ByVal data As String) As Byte()
        Dim binaryLength As Integer = CInt(data.Length / 2)
        Dim result As New List(Of Byte)(binaryLength)
        For i As Integer = 0 To binaryLength - 1
            Try
                result.Add(Byte.Parse(data.Substring(i * 2, 2), Globalization.NumberStyles.HexNumber))
            Catch ex As Exception
                result.Add(Byte.Parse(data.Substring(i * 2, 1), Globalization.NumberStyles.HexNumber))
            End Try
        Next
        Return result.ToArray()
    End Function
#End Region

#Region "Bytes/Stream"
    Public Shared Function BytesToStream(ByVal b As Byte()) As IO.Stream
        Return New IO.MemoryStream(b)
    End Function

    Const BUFFER_SIZE As Integer = 4096
    Public Shared Function StreamToBytes(ByVal s As IO.Stream) As Byte()
        If s.CanSeek Then
            With New IO.BinaryReader(s)
                StreamToBytes = .ReadBytes(CInt(s.Length))
                .Close()
                Exit Function
            End With
        End If

        Dim output As New List(Of Byte)(BUFFER_SIZE)
        Dim buffer As Byte() = Nothing
        Dim br As New BinaryReader(s)
        While True
            buffer = br.ReadBytes(BUFFER_SIZE)
            If buffer.Length = 0 Then Exit While
            output.AddRange(buffer)
        End While
        br.Close()
        Return output.ToArray()
    End Function

    Public Shared Sub StreamToFile(ByVal s As Stream, ByVal filePath As String)
        Dim buffer(BUFFER_SIZE) As Byte
        Dim read As Integer
        With New IO.FileStream(filePath, FileMode.Create)
            While True
                read = s.Read(buffer, 0, buffer.Length)
                If read > 0 Then .Write(buffer, 0, read)
                If read < BUFFER_SIZE Then
                    .Close()
                    Exit Sub
                End If
            End While
        End With
    End Sub
#End Region

#Region "Encryption (Symetric)"
    'Simple Overloads (uses the default triple des provider, based on std config settings)
    Public Shared Function Encrypt(ByVal data As String) As Byte()
        Return Encrypt(data, EncryptionProvider)
    End Function
    Public Shared Function DecryptAsStr(ByVal data As Byte()) As String
        If IsNothing(data) Then Return String.Empty
        Return DecryptAsStr(data, EncryptionProvider)
    End Function
    Public Shared Function Encrypt(ByVal data As Byte()) As Byte()
        If IsNothing(data) Then Return Nothing
        Return Encrypt(data, EncryptionProvider)
    End Function
    Public Shared Function Decrypt(ByVal data As Byte()) As Byte()
        If IsNothing(data) Then Return Nothing
        Return Decrypt(data, EncryptionProvider)
    End Function
    Public Shared Function Encrypt(ByVal data As Stream) As Stream
        Return Encrypt(data, EncryptionProvider)
    End Function
    Public Shared Function Decrypt(ByVal data As Stream) As Stream
        Return Decrypt(data, EncryptionProvider)
    End Function

    'String Interface
    Public Shared Function Encrypt(ByVal data As String, ByVal provider As SymmetricAlgorithm) As Byte()
        Return Encrypt(ASCII.GetBytes(data), provider)
    End Function
    Public Shared Function DecryptAsStr(ByVal data As Byte(), ByVal provider As SymmetricAlgorithm) As String
        Return ASCII.GetString(Decrypt(data, provider))
    End Function

    'Byte Interface
    Public Shared Function Encrypt(ByVal data As Byte(), ByVal provider As SymmetricAlgorithm) As Byte()
        Return StreamToBytes(Encrypt(BytesToStream(data), provider))
    End Function
    Public Shared Function Decrypt(ByVal data As Byte(), ByVal provider As SymmetricAlgorithm) As Byte()
        Return StreamToBytes(Decrypt(BytesToStream(data), provider))
    End Function

    'Stream Interface
    Public Shared Function Encrypt(ByVal data As Stream, ByVal provider As SymmetricAlgorithm) As Stream
        Return New CryptoStream(data, provider.CreateEncryptor(), CryptoStreamMode.Read)
    End Function
    Public Shared Function Decrypt(ByVal data As Stream, ByVal provider As SymmetricAlgorithm) As Stream
        Return New CryptoStream(data, provider.CreateDecryptor(), CryptoStreamMode.Read)
    End Function

    'Default Encryption Provider (TripleDes)
    Private Shared _provider As TripleDESCryptoServiceProvider
    Private Shared ReadOnly Property EncryptionProvider() As TripleDESCryptoServiceProvider
        Get
            If IsNothing(_provider) Then
                _provider = New TripleDESCryptoServiceProvider()
                With _provider
                    .IV = HexToBytes(CConfigBase.TripleDesIV)
                    .Key = HexToBytes(CConfigBase.TripleDesKey)
                End With
            End If
            Return _provider
        End Get
    End Property
#End Region

#Region "Encryption (Rijndal)"
    'Default Key (based on CConfigBase.FastEncryptionKey)
    Private Shared _rijndael As RijndaelManaged = Nothing
    Public Shared ReadOnly Property Rijndael() As SymmetricAlgorithm
        Get
            If IsNothing(_rijndael) Then
                Dim rij As New RijndaelManaged()
                Dim key As Byte() = LongerKey(FastEncryptionKey, rij.LegalKeySizes(0))
                rij.Key = key
                rij.IV = LongerKey(FastEncryptionKey, CInt(rij.BlockSize / 8), CInt(rij.BlockSize / 8))
                _rijndael = rij
            End If
            Return _rijndael
        End Get
    End Property

    'Rijndael - Encrypt
    Public Shared Function EncryptRijndael(ByVal data As String) As Byte()
        Return Encrypt(data, Rijndael)
    End Function
    Public Shared Function EncryptRijndael(ByVal data As Byte()) As Byte()
        Return Encrypt(data, Rijndael)
    End Function
    Public Shared Function EncryptRijndaelToBase64(ByVal data As String) As String
        Return ToBase64(Encrypt(data, Rijndael))
    End Function
    Public Shared Function EncryptRijndaelToBase64(ByVal data As Byte()) As String
        Return ToBase64(Encrypt(data, Rijndael))
    End Function

    'Rijndael - Decrypt
    Public Shared Function DecryptRijndael(ByVal data As Byte()) As Byte()
        Return Decrypt(data, Rijndael)
    End Function
    Public Shared Function DecryptRijndaelAsStr(ByVal data As Byte()) As String
        Return DecryptAsStr(data, Rijndael)
    End Function
    Public Shared Function DecryptRijndael(ByVal base64 As String) As Byte()
        Return Decrypt(FromBase64(base64), Rijndael)
    End Function
    Public Shared Function DecryptRijndaelAsStr(ByVal base64 As String) As String
        If String.IsNullOrEmpty(base64) Then Return String.Empty
        Return DecryptAsStr(FromBase64(base64), Rijndael)
    End Function

    'Rijndael - Control key Length (derived from a password)
    Public Shared Function LongerKey(ByVal key As String, ByVal minMax As KeySizes) As Byte()
        Return LongerKey(StringToBytes(key), minMax)
    End Function
    Public Shared Function LongerKey(ByVal key As Byte(), ByVal minMax As KeySizes) As Byte()
        Return LongerKey(key, CInt(minMax.MinSize / 8), CInt(minMax.MaxSize / 8))
    End Function
    Public Shared Function LongerKey(ByVal key As Byte(), ByVal min As Integer, ByVal max As Integer) As Byte()
        If key.Length = 0 Then key = New Byte() {123}
        If key.Length >= min AndAlso key.Length <= max Then Return key
        Dim longer As New List(Of Byte)(max)
        For i As Integer = 0 To max - 1
            longer.Add(key(i Mod key.Length))
        Next
        Return longer.ToArray()
    End Function

    Public Shared Function FromBase64(ByVal base64 As String) As Byte()
        Return System.Convert.FromBase64String(base64)
    End Function
    Public Shared Function ToBase64(ByVal binary As Byte()) As String
        Return System.Convert.ToBase64String(binary)
    End Function
    Public Shared Function ToBase64(ByVal g As Guid) As String
        If Guid.Empty.Equals(g) Then Return String.Empty
        Return ToBase64(g.ToByteArray)
    End Function
    Public Shared Function ToBase64(ByVal g As Guid, show As Integer) As String
        If Guid.Empty.Equals(g) Then Return String.Empty
        Return CUtilities.Truncate(ToBase64(g), show + 3).Replace("...", "").ToUpper
    End Function
#End Region

#Region "Encryption (Fast)"
    'Zip combos (used for winforms/webservices comms)
    Public Shared Function Pack(ByVal obj As Object) As Byte()
        Return Pack(obj, CConfigBase.WebServicePasswordBytes)
    End Function
    Public Shared Function Unpack(ByVal data As Byte()) As Object
        Return Unpack(data, CConfigBase.WebServicePasswordBytes)
    End Function
    Public Shared Function Pack(ByVal obj As Object, ByVal password As String) As Byte()
        Return Pack(obj, StringToBytes(password))
    End Function
    Public Shared Function Unpack(ByVal data As Byte(), ByVal password As String) As Object
        Return Unpack(data, StringToBytes(password))
    End Function
    Public Shared Function Pack(ByVal obj As Object, ByVal password As Byte()) As Byte()
        If IsNothing(obj) Then Return Nothing
        Dim data As Byte() = CBinary.SerialiseToBytesAndZip(obj)
        If password.Length > 0 Then data = CBinary.EncryptFast(data, password)
        Return data
    End Function
    Public Shared Function Unpack(ByVal data As Byte(), ByVal password As Byte()) As Object
        If IsNothing(data) OrElse data.Length = 0 Then Return Nothing
        If password.Length > 0 Then data = CBinary.EncryptFast(data, password)
        Try
            Return CBinary.DeserialiseFromBytesAndUnzip(data)
        Catch ex As Exception
            CBinary.EncryptFast(data, password)
            Throw New Exception("Failed to unpack data - check password (and see inner exception for server response)", New Exception(BytesToString(data)))
        End Try
    End Function


    'Default Key
    Private Const ENCRYPTED_MARKER As String = "0x"
    Private Shared _encryptionKey As Byte() = Nothing
    Private Shared ReadOnly Property FastEncryptionKey() As Byte()
        Get
            If IsNothing(_encryptionKey) Then
                Dim s As String = CConfigBase.FastEncryptionKey
                If Len(s) > 0 Then
                    If IsHexOnly(s) Then
                        _encryptionKey = HexToBytes(s)
                    Else
                        _encryptionKey = ASCII.GetBytes(s)
                    End If
                Else
                    _encryptionKey = New Byte() {234, 26, 58, 19, 200, 206, 94, 201, 238, 15, 1, 117}
                End If
                'Important: Switch to true for backwards-compat
                If Not CConfigBase.UseRawPassword Then _encryptionKey = CBinary.Sha512(_encryptionKey) 'longest readily-available hash
            End If
            Return _encryptionKey
        End Get
    End Property
    Private Shared Function IsHexOnly(ByVal s As String) As Boolean
        If IsNothing(s) Then Return True
        Dim hex As New List(Of Char)(CStr("0123456789abcdef").ToCharArray())
        For Each i As Char In s.ToLower().ToCharArray()
            If Not hex.Contains(i) Then Return False
        Next
        Return True
    End Function

    'Binary Versions (has no '0x' marker to distinguish encrypted vs decrypted, and decrypt function is also encrypt function)
    Public Shared Function EncryptFast(ByVal b As Byte(), ByVal key As String) As Byte()
        Return EncryptFast(b, StringToBytes(key))
    End Function
    Public Shared Function EncryptFast(ByVal b As Byte(), ByVal key As Byte()) As Byte()
        If key.Length > 0 Then
            For i As Integer = 0 To b.Length - 1
                b(i) = b(i) Xor key(i Mod key.Length)
            Next
        End If
        Return b
    End Function


    'String versions (encrypts string to a hex string with a marker)
    Public Shared Function IsEncrypted(ByVal s As String) As Boolean
        If Len(s) > 1 Then Return s.Substring(0, 2) = ENCRYPTED_MARKER
        Return False
    End Function
    Public Shared Function EncryptFast(ByVal s As String) As String
        Return EncryptFast(s, CConfigBase.WebServicePasswordBytes)
    End Function
    Public Shared Function DecryptFast(ByVal s As String) As String
        Return DecryptFast(s, CConfigBase.WebServicePasswordBytes)
    End Function
    Public Shared Function EncryptFast(ByVal s As String, ByVal key As Byte()) As String
        If IsEncrypted(s) Then Return s

        Dim bytes As Byte() = ASCII.GetBytes(s)
        EncryptFast(bytes, key)
        Return ENCRYPTED_MARKER & BytesToHex(bytes)
    End Function
    Public Shared Function DecryptFast(ByVal s As String, ByVal key As Byte()) As String
        If Not IsEncrypted(s) Then Return s

        Dim bytes As Byte() = HexToBytes(s.Substring(2))
        EncryptFast(bytes, key)
        Return ASCII.GetString(bytes)
    End Function



#End Region


#Region "Hashing"
    Private Shared m_md5 As New System.Security.Cryptography.MD5CryptoServiceProvider
    Private Shared m_sha512 As New System.Security.Cryptography.SHA512Managed
    Private Shared m_sha256 As New System.Security.Cryptography.SHA256Managed

    'Public Shared Function Sha1(ByVal plainText As String) As String
    '    Return FormsAuthentication.HashPasswordForStoringInConfigFile(plainText, "SHA1")
    'End Function

    Public Shared Function MD5(ByVal plainText As String) As String
        Return ASCII.GetString(MD5(ASCII.GetBytes(plainText)))
        'Return FormsAuthentication.HashPasswordForStoringInConfigFile(plainText, "MD5")
    End Function
    Public Shared Function MD5(ByVal bytes As Byte()) As Byte()
        SyncLock m_md5
            Return m_md5.ComputeHash(bytes)
        End SyncLock
    End Function
    Public Shared Function MD5_(ByVal bytes As Byte()) As Guid
        Return New Guid(MD5(bytes))
    End Function
    Public Shared Function MD5_(ByVal s As String) As Guid
        Return New Guid(MD5(StringToBytes(s)))
    End Function
    Public Shared Function MD5_(s1 As String, s2 As String, ParamArray l() As List(Of String)) As Guid
        Dim sb As New StringBuilder(s1)
        sb.Append(s2)
        For Each i As List(Of String) In l
            sb.Append(CUtilities.ListToString(i))
        Next
        Return MD5_(sb.ToString)
    End Function
    Public Shared Function MD5_(ParamArray s() As String) As Guid
        Return MD5_(String.Concat(s))
    End Function
    Public Shared Function MD5_(ParamArray g() As Guid) As Guid
        Dim list As New List(Of Byte)(16 * g.Length)
        For Each i As Guid In g
            list.AddRange(i.ToByteArray)
        Next
        Return MD5_(list.ToArray)
    End Function
    Public Shared Function MD5_(s As List(Of String)) As Guid
        Return MD5_(s.ToArray)
    End Function
    Public Shared Function MD5_(g As List(Of Guid)) As Guid
        Return MD5_(g.ToArray)
    End Function





    Public Shared Function MD5_FromFile(path As String) As Guid
        Using sp As New MD5CryptoServiceProvider()
            Try
                Using f As FileStream = File.OpenRead(path)
                    Dim md5 As New Guid(sp.ComputeHash(f))
                    f.Close()
                    Return md5
                End Using
            Catch ex As Exception
                Threading.Thread.Sleep(1000)

                Using f As FileStream = File.OpenRead(path)
                    Dim md5 As New Guid(sp.ComputeHash(f))
                    f.Close()
                    Return md5
                End Using
            End Try
        End Using
    End Function
    Public Shared Function MD5_Cached(filePath As String, chunkSize As Integer) As CFileHash
        'Chunks also computed for large files
        If filePath.EndsWith(CFolderHash.CHUNKFILE) Then
            'Get Parent name
            Dim s As String = filePath
            Dim i As Integer = s.LastIndexOf(".")
            s = s.Substring(0, i) 'chunkfile
            i = s.LastIndexOf(".") '
            s = s.Substring(0, i) ''total
            i = s.LastIndexOf(".") '
            s = s.Substring(0, i) ''of
            i = s.LastIndexOf(".") '
            Dim index As Integer = Integer.Parse(s.Substring(i + 1))
            s = s.Substring(0, i) ''index
            filePath = s

            'Hash of chunk is stored with parent
            Return MD5_Cached(filePath, chunkSize).Chunks(index)
        End If

        'Normal file (under 4mb)
        Return MD5_FromCache(New FileInfo(filePath), chunkSize)
    End Function
    Public Shared Function MD5_FromCache(fi As FileInfo, chunkSize As Integer) As CFileHash
        Dim key As String = String.Format("MD5_FromCache.{0}.{1}.{2}.{3}", fi.Name, fi.Length, fi.LastWriteTimeUtc, chunkSize)

        'From cache
        Dim hash As CFileHash = CType(CCache.Get(key), CFileHash)
        If Not IsNothing(hash) Then Return hash

        'Create from file
        Dim md5 As Guid = MD5_FromFile(fi.FullName)

        'Store
        hash = New CFileHash(fi.Name, md5, fi.Length)
        CCache.Set(key, hash)

        'Compute chunks
        If hash.Size > chunkSize Then
            hash.Chunks = CFileHash.HashAsChunks(fi.FullName, fi.Length, fi.LastWriteTimeUtc, chunkSize, False)
        End If

        Return hash
    End Function


    Public Shared Function Sha512(ByVal b As Byte()) As Byte()
        SyncLock m_sha256
            Return m_sha512.ComputeHash(b)
        End SyncLock
    End Function
    Public Shared Function Sha512Base64(ByVal b As Byte()) As String
        Return System.Convert.ToBase64String(Sha512(b))
    End Function

    Public Shared Function Sha256(ByVal b As Byte()) As Byte()
        SyncLock m_sha256
            Return m_sha256.ComputeHash(b)
        End SyncLock
    End Function
    Public Shared Function Sha256Base64(ByVal b As Byte()) As String
        Return System.Convert.ToBase64String(Sha256(b))
    End Function


    Public Shared Function Sha128_(ByVal b As Byte()) As Guid
        Return New Guid(Sha128(b))
    End Function
    Public Shared Function Sha128(ByVal b As Byte()) As Byte()
        Dim s256 As Byte() = Nothing
        SyncLock m_sha256
            s256 = m_sha256.ComputeHash(b)
        End SyncLock
        Dim list As New List(Of Byte)(s256)
        Return list.GetRange(0, 16).ToArray
    End Function
#End Region



End Class