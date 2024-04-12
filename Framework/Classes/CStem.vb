Imports System.IO



Public Class CStem
    Private Const INC As Int32 = 50
    Private b(INC) As Char
    Private i As Int32     ' offset into b
    Private i_end As Int32 ' offset to end of stemmed word
    Private j As Int32
    Private k As Int32
    ' unit of size whereby b is increased

    Public Sub New()
        i = 0
        i_end = 0
    End Sub



    Public Shared Function Stem(ByVal word As String) As String
        Dim s As New CStem()
        s.add(word.ToCharArray(), word.Length)
        s.stem()
        Return s.ToString()
    End Function

    ' Add a character to the word being stemmed.  When you are finished
    ' adding characters, you can call stem(void) to stem the word.
    '
    Public Sub add(ByVal ch As Char)

        If (i = b.Length) Then
            Dim new_b(i + INC) As Char
            For c As Int32 = 0 To i - 1
                new_b(c) = b(c)
            Next
            b = new_b
        End If
        b(i) = ch
        i += 1
    End Sub


    ' Adds wLen characters to the word being stemmed contained in a portion
    ' of a char() array. This is like repeated calls of add(char ch), but
    ' faster.
    '
    Public Sub add(ByVal w As Char(), ByVal wLen As Int32)

        If (i + wLen >= b.Length) Then
            Dim new_b(i + wLen + INC) As Char
            For c As Int32 = 0 To i - 1
                new_b(c) = b(c)
            Next
            b = new_b
        End If
        For c As Int32 = 0 To wLen - 1
            b(i) = w(c)
            i += 1
        Next
    End Sub


    ' After a word has been stemmed, it can be retrieved by toString(),
    ' or a reference to the internal buffer can be retrieved by getResultBuffer
    ' and getResultLength (which is generally more efficient.)
    '
    Public Overrides Function ToString() As String
        Return New String(b, 0, i_end)
    End Function


    ' Returns the length of the word resulting from the stemming process.
    '
    Public Function getResultLength() As Int32
        Return i_end
    End Function


    ' Returns a reference to a character buffer containing the results of
    ' the stemming process.  You also need to consult getResultLength()
    ' to determine the length of the result.
    '
    Public Function getResultBuffer() As Char()
        Return b
    End Function

    ' cons(i) is true <=> b(i) is a consonant.
    Private Function cons(ByVal i As Int32) As Boolean
        Select Case (b(i))
            Case "a"c, "e"c, "i"c, "o"c, "u"c
                Return False
            Case "y"c
                If (i = 0) Then
                    Return True
                Else
                    Return Not cons(i - 1)
                End If
            Case Else
                Return True
        End Select
    End Function

    ' m() measures the number of consonant sequences between 0 and j. if c is
    '    a consonant sequence and v a vowel sequence, and <..> indicates arbitrary
    '    presence,
    '
    '     <c><v>       gives 0
    '     <c>vc<v>     gives 1
    '     <c>vcvc<v>   gives 2
    '     <c>vcvcvc<v> gives 3
    '     ....
    '
    Private Function m() As Int32
        Dim n As Int32 = 0
        Dim i As Int32 = 0
        While (True)

            If (i > j) Then Return n
            If (Not cons(i)) Then Exit While
            i += 1
        End While
        i += 1
        While (True)

            While (True)

                If (i > j) Then Return n
                If (cons(i)) Then Exit While
                i += 1
            End While
            i += 1
            n += 1
            While (True)

                If (i > j) Then Return n
                If (Not cons(i)) Then Exit While
                i += 1
            End While
            i += 1
        End While
        Return n
    End Function

    ' vowelinstem() is true <=> 0,...j contains a vowel
    Private Function vowelinstem() As Boolean

        For i As Int32 = 0 To j
            If (Not cons(i)) Then
                Return True
            End If
        Next
        Return False
    End Function

    ' doublec(j) is true <=> j,(j-1) contain a double consonant.
    Private Function doublec(ByVal j As Int32) As Boolean

        If (j < 1) Then
            Return False
        End If
        If (b(j) <> b(j - 1)) Then
            Return False
        End If
        Return cons(j)
    End Function

    ' cvc(i) is true <=> i-2,i-1,i has the form consonant - vowel - consonant
    '   and also if the second c is not w,x or y. this is used when trying to
    '   restore an e at the end of a short word. e.g.
    '
    '    cav(e), lov(e), hop(e), crim(e), but
    '    snow, box, tray.
    '
    Private Function cvc(ByVal i As Int32) As Boolean

        If (i < 2 OrElse Not cons(i) OrElse cons(i - 1) OrElse Not cons(i - 2)) Then
            Return False
        End If
        Dim ch As Char = b(i)
        If (ch = "w"c Or ch = "x"c Or ch = "y"c) Then
            Return False
        End If
        Return True
    End Function

    Private Function ends(ByVal s As String) As Boolean

        Dim l As Int32 = s.Length
        Dim o As Int32 = k - l + 1
        If (o < 0) Then
            Return False
        End If
        Dim sc As Char() = s.ToCharArray()
        For i As Int32 = 0 To l - 1
            If (b(o + i) <> sc(i)) Then
                Return False
            End If
        Next
        j = k - l
        Return True
    End Function

    ' setto(s) sets (j+1),...k to the characters in the string s, readjusting
    '   k.
    Private Sub setto(ByVal s As String)

        Dim l As Int32 = s.Length
        Dim o As Int32 = j + 1
        Dim sc As Char() = s.ToCharArray()
        For i As Int32 = 0 To l - 1
            b(o + i) = sc(i)
        Next
        k = j + l
    End Sub

    ' r(s) is used further down.
    Private Sub r(ByVal s As String)

        If (m() > 0) Then
            setto(s)
        End If
    End Sub

    ' step1() gets rid of plurals and -ed or -ing. e.g.
    '     caresses  ->  caress
    '     ponies    ->  poni
    '     ties      ->  ti
    '     caress    ->  caress
    '     cats      ->  cat
    '
    '     feed      ->  feed
    '     agreed    ->  agree
    '     disabled  ->  disable
    '
    '     matting   ->  mat
    '     mating    ->  mate
    '     meeting   ->  meet
    '     milling   ->  mill
    '     messing   ->  mess
    '
    '     meetings  ->  meet
    '
    Private Sub step1()

        If (b(k) = "s"c) Then

            If (ends("sses")) Then
                k -= 2
            ElseIf (ends("ies")) Then
                setto("i")
            ElseIf (b(k - 1) <> "s"c) Then
                k -= 1
            End If
        End If
        If (ends("eed")) Then
            If (m() > 0) Then
                k -= 1
            End If
        ElseIf ((ends("ed") Or ends("ing")) And vowelinstem()) Then
            k = j
            If (ends("at")) Then
                setto("ate")
            ElseIf (ends("bl")) Then
                setto("ble")
            ElseIf (ends("iz")) Then
                setto("ize")
            ElseIf (doublec(k)) Then
                k -= 1
                Dim ch As Char = b(k)
                If (ch = "l"c Or ch = "s"c Or ch = "z"c) Then
                    k += 1
                End If
            ElseIf (m() = 1 And cvc(k)) Then
                setto("e")
            End If
        End If
    End Sub

    ' step2() turns terminal y to i when there is another vowel in the stem.
    Private Sub step2()

        If (ends("y") And vowelinstem()) Then
            b(k) = "i"c
        End If
    End Sub

    ' step3() maps double suffices to single ones. so -ization ( = -ize plus
    '   -ation) maps to -ize etc. note that the string before the suffix must give
    '   m() > 0.
    Private Sub step3()

        If (k = 0) Then
            Return
        End If

        ' For Bug 1
        Select Case (b(k - 1))

            Case "a"c
                If (ends("ational")) Then
                    r("ate")
                    Exit Select
                End If
                If (ends("tional")) Then
                    r("tion")
                    Exit Select
                End If
                Exit Select
            Case "c"c
                If (ends("enci")) Then
                    r("ence")
                    Exit Select
                End If
                If (ends("anci")) Then
                    r("ance")
                    Exit Select
                End If
                Exit Select
            Case "e"c
                If (ends("izer")) Then
                    r("ize")
                    Exit Select
                End If
                Exit Select
            Case "l"c
                If (ends("bli")) Then
                    r("ble")
                    Exit Select
                End If
                If (ends("alli")) Then
                    r("al")
                    Exit Select
                End If
                If (ends("entli")) Then
                    r("ent")
                    Exit Select
                End If
                If (ends("eli")) Then
                    r("e")
                    Exit Select
                End If
                If (ends("ousli")) Then
                    r("ous")
                    Exit Select
                End If
                Exit Select
            Case "o"c
                If (ends("ization")) Then
                    r("ize")
                    Exit Select
                End If
                If (ends("ation")) Then
                    r("ate")
                    Exit Select
                End If
                If (ends("ator")) Then
                    r("ate")
                    Exit Select
                End If
                Exit Select
            Case "s"c
                If (ends("alism")) Then
                    r("al")
                    Exit Select
                End If
                If (ends("iveness")) Then
                    r("ive")
                    Exit Select
                End If
                If (ends("fulness")) Then
                    r("ful")
                    Exit Select
                End If
                If (ends("ousness")) Then
                    r("ous")
                    Exit Select
                End If
                Exit Select
            Case "t"c
                If (ends("aliti")) Then
                    r("al")
                    Exit Select
                End If
                If (ends("iviti")) Then
                    r("ive")
                    Exit Select
                End If
                If (ends("biliti")) Then
                    r("ble")
                    Exit Select
                End If
                Exit Select
            Case "g"c
                If (ends("logi")) Then
                    r("log")
                    Exit Select
                End If
                Exit Select
            Case Else
                Exit Select
        End Select
    End Sub

    ' step4() deals with -ic-, -full, -ness etc. similar strategy to step3.
    Private Sub step4()

        Select Case (b(k))

            Case "e"c
                If (ends("icate")) Then
                    r("ic")
                    Exit Select
                End If
                If (ends("ative")) Then
                    r("")
                    Exit Select
                End If
                If (ends("alize")) Then
                    r("al")
                    Exit Select
                End If
                Exit Select
            Case "i"c
                If (ends("iciti")) Then
                    r("ic")
                    Exit Select
                End If
                Exit Select
            Case "l"c
                If (ends("ical")) Then
                    r("ic")
                    Exit Select
                End If
                If (ends("ful")) Then
                    r("")
                    Exit Select
                End If
                Exit Select
            Case "s"c
                If (ends("ness")) Then
                    r("")
                    Exit Select
                End If
                Exit Select
        End Select
    End Sub

    ' step5() takes off -ant, -ence etc., in context <c>vcvc<v>.
    Private Sub step5()

        If (k = 0) Then
            Return
        End If
        ' for Bug 1
        Select Case (b(k - 1))

            Case "a"c
                If (ends("al")) Then Exit Select

                Return
            Case "c"c
                If (ends("ance")) Then Exit Select
                If (ends("ence")) Then Exit Select

                Return
            Case "e"c
                If (ends("er")) Then Exit Select

                Return
            Case "i"c
                If (ends("ic")) Then Exit Select

                Return
            Case "l"c
                If (ends("able")) Then Exit Select
                If (ends("ible")) Then Exit Select

                Return
            Case "n"c
                If (ends("ant")) Then Exit Select
                If (ends("ement")) Then Exit Select
                If (ends("ment")) Then Exit Select
                ' element etc. not stripped before the m
                If (ends("ent")) Then Exit Select

                Return
            Case "o"c
                If (ends("ion") AndAlso j >= 0 AndAlso (b(j) = "s"c OrElse b(j) = "t"c)) Then Exit Select
                ' j >= 0 fixes Bug 2
                If (ends("ou")) Then Exit Select

                Return
                ' takes care of -ous
            Case "s"c
                If (ends("ism")) Then Exit Select

                Return
            Case "t"c
                If (ends("ate")) Then Exit Select
                If (ends("iti")) Then Exit Select

                Return
            Case "u"c
                If (ends("ous")) Then Exit Select

                Return
            Case "v"c
                If (ends("ive")) Then Exit Select

                Return
            Case "z"c
                If (ends("ize")) Then Exit Select

                Return
            Case Else
                Return
        End Select
        If (m() > 1) Then
            k = j
        End If
    End Sub

    ' step6() removes a final -e if m() > 1.
    Private Sub step6()

        j = k

        If (b(k) = "e"c) Then

            Dim a As Int32 = m()
            If (a > 1 Or a = 1 And Not cvc(k - 1)) Then
                k -= 1
            End If
        End If
        If (b(k) = "l"c And doublec(k) And m() > 1) Then
            k -= 1
        End If
    End Sub

    ' Stem the word placed into the Stemmer buffer through calls to add().
    ' Returns true if the stemming process resulted in a word different
    ' from the input.  You can retrieve the result with
    ' getResultLength()/getResultBuffer() or toString().
    '
    Public Sub stem()

        k = i - 1
        If (k > 1) Then

            step1()
            step2()
            step3()
            step4()
            step5()
            step6()
        End If
        i_end = k + 1
        i = 0
    End Sub


End Class

