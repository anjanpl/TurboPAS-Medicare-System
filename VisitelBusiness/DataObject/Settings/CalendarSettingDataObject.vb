
Namespace VisitelBusiness.DataObject.Settings
    Public Class CalendarSettingDataObject
        Inherits BaseDataObject

        Public Property ScheduleId As Integer

        Public Property ClientId As Integer

        Public Property EmployeeId As Integer

        Public Property SundayHourMinute As String

        Public Property MondayHourMinute As String

        Public Property TuesdayHourMinute As String

        Public Property WednesdayHourMinute As String

        Public Property ThursdayHourMinute As String

        Public Property FridayHourMinute As String

        Public Property SaturdayHourMinute As String

        Public Property Status As String

        Public Property SpecialRate As Decimal

        Public Property Comments As String

        Public Property StartDate As String

        Public Property EndDate As String

        Public Property SundayInTime As String

        Public Property SundayOutTime As String

        Public Property MondayInTime As String

        Public Property MondayOutTime As String

        Public Property TuesdayInTime As String

        Public Property TuesdayOutTime As String

        Public Property WednesdayInTime As String

        Public Property WednesdayOutTime As String

        Public Property ThursdayInTime As String

        Public Property ThursdayOutTime As String

        Public Property FridayInTime As String

        Public Property FridayOutTime As String

        Public Property SaturdayInTime As String

        Public Property SaturdayOutTime As String

        Public Property WeeklyScheduleMinutes As Int32

        Public Property UpdateBy As String

        Public Property UpdateDate As String

        Public Property CompanyId As Integer

        Public Property UserId As Integer

        Public Sub New()
            Me.ScheduleId = InlineAssignHelper(Me.ClientId, InlineAssignHelper(Me.EmployeeId, InlineAssignHelper(Me.WeeklyScheduleMinutes,
                            InlineAssignHelper(Me.CompanyId, InlineAssignHelper(Me.UserId, Int32.MinValue)))))

            Me.SundayHourMinute = InlineAssignHelper(Me.MondayHourMinute, InlineAssignHelper(Me.TuesdayHourMinute,
                                  InlineAssignHelper(Me.WednesdayHourMinute, InlineAssignHelper(Me.ThursdayHourMinute,
                                  InlineAssignHelper(Me.FridayHourMinute, InlineAssignHelper(Me.SaturdayHourMinute,
                                  InlineAssignHelper(Me.SundayInTime, InlineAssignHelper(Me.SundayOutTime,
                                  InlineAssignHelper(Me.MondayInTime, InlineAssignHelper(Me.MondayOutTime,
                                  InlineAssignHelper(Me.TuesdayInTime, InlineAssignHelper(Me.TuesdayOutTime,
                                  InlineAssignHelper(Me.WednesdayInTime, InlineAssignHelper(Me.WednesdayOutTime,
                                  InlineAssignHelper(Me.ThursdayInTime, InlineAssignHelper(Me.ThursdayOutTime,
                                  InlineAssignHelper(Me.FridayInTime, InlineAssignHelper(Me.FridayOutTime,
                                  InlineAssignHelper(Me.SaturdayInTime, InlineAssignHelper(Me.SaturdayOutTime,
                                  InlineAssignHelper(Me.Comments, InlineAssignHelper(Me.UpdateBy, InlineAssignHelper(Me.UpdateDate, String.Empty)))))))))))))))))))))))

            Me.SpecialRate = Decimal.MinValue
        End Sub
    End Class
End Namespace

