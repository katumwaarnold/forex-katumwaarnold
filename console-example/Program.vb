Option Strict On
Option Explicit On

Imports System
Imports System.Globalization

' Portable console adaptation, NOT a reconstruction of the missing Windows Form.
' The source gives these fixed rates but supplies no date. They are NOT live rates.
Public Module ForexConverter
    Public Const UgxPerYuan As Decimal = 540D
    Public Const UgxPerDirham As Decimal = 110D

    ' Returns False for empty, malformed, negative, or out-of-range input.
    ' Uses the current locale's decimal separator; grouping is deliberately disallowed.
    Public Function TryReadAmount(text As String, ByRef amount As Decimal) As Boolean
        If String.IsNullOrWhiteSpace(text) Then Return False
        Dim styles As NumberStyles = NumberStyles.AllowLeadingWhite Or NumberStyles.AllowTrailingWhite Or NumberStyles.AllowLeadingSign Or NumberStyles.AllowDecimalPoint
        If Not Decimal.TryParse(text, styles, CultureInfo.CurrentCulture, amount) Then Return False
        Return amount >= 0D
    End Function

    ' Directions 1 and 2 follow the source's formulas; 3 and 4 are inferred
    ' from the stated UGX per dirham rate and the named form fields.
    Public Function ConvertAmount(direction As Integer, amount As Decimal) As Decimal
        If amount < 0D Then Throw New ArgumentOutOfRangeException(NameOf(amount))
        Select Case direction
            Case 1 : Return amount / UgxPerYuan
            Case 2 : Return amount * UgxPerYuan
            Case 3 : Return amount / UgxPerDirham
            Case 4 : Return amount * UgxPerDirham
            Case Else : Throw New ArgumentOutOfRangeException(NameOf(direction))
        End Select
    End Function

    Public Sub Main()
        Console.WriteLine("KABBAYA FOREX BUREAU - console adaptation")
        Console.WriteLine("Fixed source rates: 1 yuan = 540 UGX; 1 UAE dirham = 110 UGX.")
        Console.WriteLine("Rate date is unknown. These are NOT current or live rates; do not use for transactions.")
        Do
            Console.WriteLine()
            Console.WriteLine("1 UGX to yuan  |  2 yuan to UGX  |  3 UGX to dirham  |  4 dirham to UGX  |  0 exit")
            Console.Write("Choose: ")
            Dim choiceText As String = Console.ReadLine()
            If choiceText Is Nothing Then Exit Sub
            Dim choice As Integer
            If Not Integer.TryParse(choiceText, choice) OrElse choice < 0 OrElse choice > 4 Then
                Console.WriteLine("Choose 0, 1, 2, 3, or 4.")
                Continue Do
            End If
            If choice = 0 Then Exit Sub
            Console.Write("Enter a non-negative amount (no thousands separators): ")
            Dim input As String = Console.ReadLine()
            If input Is Nothing Then Exit Sub
            Dim amount As Decimal
            If Not TryReadAmount(input, amount) Then
                Console.WriteLine("Please enter a valid non-negative amount.")
                Continue Do
            End If
            Try
                Dim result As Decimal = ConvertAmount(choice, amount)
                Dim unit As String = If(choice = 1, "yuan", If(choice = 3, "UAE dirham", "UGX"))
                Console.WriteLine("Result: " & result.ToString("N2", CultureInfo.CurrentCulture) & " " & unit)
            Catch ex As OverflowException
                Console.WriteLine("The result is too large to represent.")
            End Try
        Loop
    End Sub
End Module
