Imports System.Threading

Public Class ConsoleSpinner
    Private Shared sequence(,) As String = Nothing

    Public Property Delay() As Integer = 200

    Private ReadOnly totalSequences As Integer = 0
    Private counter As Integer

    Public Sub New()
        counter = 0
        sequence = New String(,) {
            {"/", "-", "\", "|"},
            {".", "o", "0", "o"},
            {"+", "x", "+", "x"},
            {"V", "<", "^", ">"},
            {".   ", "..  ", "... ", "...."},
            {"=>   ", "==>  ", "===> ", "====>"}
        }

        totalSequences = sequence.GetLength(0)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sequenceCode"> 0 | 1 | 2 |3 | 4 | 5 </param>
    Public Sub Turn(Optional ByVal displayMsg As String = "", Optional ByVal sequenceCode As Integer = 0)
        counter += 1

        Thread.Sleep(Delay)

        sequenceCode = If(sequenceCode > totalSequences - 1, 0, sequenceCode)

        Dim counterValue As Integer = counter Mod 4

        Dim fullMessage As String = displayMsg & sequence(sequenceCode, counterValue)
        Dim msglength As Integer = fullMessage.Length

        Console.Write(fullMessage)

        Console.SetCursorPosition(Console.CursorLeft - msglength, Console.CursorTop)
    End Sub
End Class