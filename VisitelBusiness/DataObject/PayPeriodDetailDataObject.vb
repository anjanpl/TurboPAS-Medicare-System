Namespace VisitelBusiness.DataObject
    Public Class PayPeriodDetailDataObject
        Public Property PayPeriodId As Int32

        Public Property StartDate As String

        Public Property EndDate As String

        Public Property CalendarId As Integer

        Public Property IndividualId As Int64

        Public Property IndividualName As String

        Public Property AttendantId As Int64

        Public Property AttendantName As String

        Public Property ServiceDate As String

        Public Property DayName As String

        Public Property HourMinutes As String

        Public Property InTime As String

        Public Property OutTime As String

        Public Property SpecialRate As Decimal

        Public Property Comments As String

        Public Property UpdateDate As String

        Public Property UpdateBy As String

        Public Property CompanyId As Integer

        Public Property UserId As Integer

        Public Property Remarks As String

        Public Sub New()
            Me.PayPeriodId = Int32.MinValue
            Me.StartDate = String.Empty
            Me.EndDate = String.Empty
            Me.CalendarId = Integer.MinValue
            Me.IndividualId = Int64.MinValue
            Me.IndividualName = String.Empty
            Me.AttendantId = Int64.MinValue
            Me.AttendantName = String.Empty
            Me.ServiceDate = String.Empty
            Me.DayName = String.Empty
            Me.HourMinutes = String.Empty
            Me.InTime = String.Empty
            Me.OutTime = String.Empty
            Me.SpecialRate = Decimal.MinValue
            Me.Comments = String.Empty
            Me.UpdateDate = Nothing
            Me.UpdateBy = String.Empty
            Me.CompanyId = Integer.MinValue
            Me.UserId = Integer.MinValue
            Me.Remarks = String.Empty
        End Sub
    End Class
End Namespace