
Namespace VisitelBusiness.DataObject

    Public Class CareSummaryDataObject
        Inherits BaseDataObject

        Public Property CareSummaryId As Int64

        Public Property StartDate As String

        Public Property EndDate As String

        Public Property ClientId As Int64

        Public Property ClientType As Int64

        Public Property ClientTypeName As String

        Public Property ClientName As String

        Public Property AttendantId As Int64

        Public Property AttendantName As String

        Public Property Priority As String

        Public Property SpecialRate As Decimal

        Public Property TimesheetHourMinute As String

        Public Property AdjustedBillTime As String

        Public Property OriginalTimesheet As Int16

        Public Property Billed As Int16

        Public Property AssignedMinutes As Int64

        Public Property BillTime As String

        Public Property CalendarId As Int64

        Public Property EDIUpdateBy As String

        Public Property EDIUpdateDate As String

        Public Property UpdateDate As String

        Public Property UpdateBy As String

        Public Property CompanyId As Integer

        Public Property UserId As Integer

        Public Property Remarks As String

        Public Sub New()
            Me.CareSummaryId = InlineAssignHelper(Me.ClientId, InlineAssignHelper(Me.AttendantId, InlineAssignHelper(Me.CalendarId,
                               InlineAssignHelper(Me.AssignedMinutes, Int64.MinValue))))

            Me.StartDate = InlineAssignHelper(Me.EndDate, InlineAssignHelper(Me.ClientName, InlineAssignHelper(Me.AttendantName,
                           InlineAssignHelper(Me.Priority, InlineAssignHelper(Me.EDIUpdateDate, InlineAssignHelper(Me.EDIUpdateBy,
                           InlineAssignHelper(Me.UpdateDate, InlineAssignHelper(Me.UpdateBy, InlineAssignHelper(Me.TimesheetHourMinute,
                           InlineAssignHelper(Me.AdjustedBillTime, InlineAssignHelper(Me.Remarks, InlineAssignHelper(Me.BillTime, String.Empty))))))))))))

            Me.SpecialRate = Decimal.MinValue

            Me.OriginalTimesheet = Int16.MinValue
            Me.Billed = Int16.MinValue

            Me.CompanyId = Integer.MinValue
            Me.UserId = Integer.MinValue
        End Sub
    End Class
End Namespace

