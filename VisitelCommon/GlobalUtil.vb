
Namespace VisitelCommon
    Public NotInheritable Class GlobalUtil
        Private Sub New()
        End Sub
        Public Shared Function MonthDifference(lValue As DateTime, rValue As DateTime) As Integer
            Return (lValue.Month - rValue.Month) + 12 * (lValue.Year - rValue.Year)
        End Function

        Public Shared Function DayDifference(lValue As DateTime, rValue As DateTime) As Integer
            Return Convert.ToInt32((lValue - rValue).TotalDays)
        End Function

        Public Shared Function MinutesDifference(lValue As DateTime, rValue As DateTime) As Integer
            Return Convert.ToInt32((lValue - rValue).TotalMinutes)
        End Function

    End Class
End Namespace
