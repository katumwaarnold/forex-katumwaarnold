Option Strict On
Option Explicit On
Imports System
Imports System.Globalization

Public Module Tests
    Private passed As Integer = 0
    Private Sub Check(condition As Boolean, label As String)
        If Not condition Then Throw New Exception("FAIL: " & label)
        passed += 1
        Console.WriteLine("PASS: " & label)
    End Sub
    Public Sub Main()
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US")
        Check(ForexConverter.ConvertAmount(1, 540D) = 1D, "540 UGX -> 1 yuan")
        Check(ForexConverter.ConvertAmount(2, 2D) = 1080D, "2 yuan -> 1080 UGX")
        Check(ForexConverter.ConvertAmount(3, 110D) = 1D, "110 UGX -> 1 dirham (inferred)")
        Check(ForexConverter.ConvertAmount(4, 2D) = 220D, "2 dirham -> 220 UGX (inferred)")
        Check(ForexConverter.ConvertAmount(1, 0D) = 0D, "zero is accepted")
        Check(ForexConverter.ConvertAmount(1, 1D).ToString("N2", CultureInfo.CurrentCulture) = "0.00", "source N2 display format")
        Check(Math.Abs(ForexConverter.ConvertAmount(1, 540.5D) * 540D - 540.5D) < 0.0000000000000000000001D, "fractional amount (Decimal tolerance)")
        Dim value As Decimal
        Check(ForexConverter.TryReadAmount(" 123.45 ", value) AndAlso value = 123.45D, "valid decimal")
        Check(Not ForexConverter.TryReadAmount("", value), "empty input rejected")
        Check(Not ForexConverter.TryReadAmount("   ", value), "whitespace rejected")
        Check(Not ForexConverter.TryReadAmount("abc", value), "malformed input rejected")
        Check(Not ForexConverter.TryReadAmount("-1", value), "negative input rejected")
        Check(Not ForexConverter.TryReadAmount("1,000", value), "ambiguous grouping rejected")
        Check(Not ForexConverter.TryReadAmount("79228162514264337593543950336", value), "input beyond Decimal range rejected")
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR")
        Check(ForexConverter.TryReadAmount("1,25", value) AndAlso value = 1.25D, "locale decimal separator")
        Dim overflowCaught As Boolean = False
        Try
            ForexConverter.ConvertAmount(2, Decimal.MaxValue)
        Catch ex As OverflowException
            overflowCaught = True
        End Try
        Check(overflowCaught, "multiplication overflow detected")
        Dim badDirection As Boolean = False
        Try
            ForexConverter.ConvertAmount(5, 1D)
        Catch ex As ArgumentOutOfRangeException
            badDirection = True
        End Try
        Check(badDirection, "invalid direction rejected")
        Dim negativeCaught As Boolean = False
        Try
            ForexConverter.ConvertAmount(1, -1D)
        Catch ex As ArgumentOutOfRangeException
            negativeCaught = True
        End Try
        Check(negativeCaught, "negative amount rejected by core")
        Console.WriteLine("TOTAL PASSED: " & passed.ToString(CultureInfo.InvariantCulture))
    End Sub
End Module
