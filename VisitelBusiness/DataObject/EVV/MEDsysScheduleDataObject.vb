
Namespace VisitelBusiness.DataObject.EVV
    Public Class MEDsysScheduleDataObject
        Inherits BaseDataObject

        Public Property Id As Int64

        ''' <summary>
        ''' Required; [MEDsys assigned Account ID]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AccountId As Int64

        ''' <summary>
        ''' Required; [The Unique ID (Primary Key) for this Schedule from the system sending the record. External Primary Key]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ExternalId As String

        ''' <summary>
        ''' [MEDsys assigned Schedule ID. Primary Key]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ScheduleId As Int64

        ''' <summary>
        ''' Required; [Schedule Date]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property [Date] As String

        ''' <summary>
        ''' Required; [Service Code. Acceptable codes are assigned during implementation]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ServiceCode As String

        ''' <summary>
        ''' Required; [Activity Code. Acceptable codes are assigned during implementation]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ActivityCode As String

        ''' <summary>
        ''' Required; [Client’s Program Code. Acceptable codes are assigned during implementation]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ProgramCode As String

        ''' <summary>
        ''' [The Unique ID of the Client assigned to this schedule. External Primary Key]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ClientExternalId As String

        ''' <summary>
        ''' [The Unique ID of the Staff Member assigned to this schedule. External Primary Key]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StaffExternalId As String

        ''' <summary>
        ''' [Bill type for this schedule. Acceptable values are:
        ''' 1 – Hourly
        ''' 2 – Visit
        ''' 3 – Unit
        ''' If no value is supplied, 1 (Hourly) is assumed]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property BillType As String

        ''' <summary>
        ''' [Pay type for this schedule. Acceptable values are:
        ''' 1 – Hourly
        ''' 2 – Visit
        ''' 3 – Unit
        ''' If no value is supplied, 1 (Hourly) is assumed]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PayType As String

        ''' <summary>
        ''' [Planned Start Time]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PlannedTimeIn As String

        ''' <summary>
        ''' [Planned End Time]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PlannedTimeOut As String

        ''' <summary>
        ''' [Planned Duration]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PlannedDuration As String

        ''' <summary>
        ''' [Occurred Time IN. Only populated on Confirmed Schedules]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OccurredTimeIn As String

        ''' <summary>
        ''' [Occurred Time OUT. Only populated on Confirmed Schedules]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OccurredTimeOut As String

        ''' <summary>
        ''' [Occurred Duration. Only populated on Confirmed Schedules]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OccurredDuration As String

        ''' <summary>
        ''' [Schedule Status. Acceptable values are:
        ''' 1 – Planned
        ''' 2 – Confirmed
        ''' 3 – Exception
        ''' 4 – On Hold
        ''' 5 – Cancelled
        ''' 6 – Closed]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Status As String

        ''' <summary>
        ''' [General comments for this Schedule]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Comments As String

        ''' <summary>
        ''' [What action to perform with this record. Acceptable
        ''' values are:
        ''' N – New Record
        ''' U – Updated Record
        ''' V – Void Record]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Action As String

        Public Property ClientId As Int64

        Public Property ClientName As String

        Public Property EmployeeId As Int64

        Public Property EmployeeName As String

        Public Property UpdateDate As String

        Public Property UpdateBy As String

        Public Property Remarks As String

        Public Sub New()
            Me.AccountId = InlineAssignHelper(Me.ScheduleId, InlineAssignHelper(Me.ClientId, InlineAssignHelper(Me.EmployeeId, Int64.MinValue)))

            Me.ExternalId = InlineAssignHelper(Me.[Date], InlineAssignHelper(Me.ServiceCode, InlineAssignHelper(Me.ActivityCode, InlineAssignHelper(Me.ProgramCode, InlineAssignHelper(Me.ClientExternalId, InlineAssignHelper(Me.StaffExternalId, InlineAssignHelper(Me.BillType, InlineAssignHelper(Me.PayType, InlineAssignHelper(Me.PlannedTimeIn, InlineAssignHelper(Me.PlannedTimeOut, InlineAssignHelper(Me.PlannedDuration, InlineAssignHelper(Me.OccurredTimeIn, InlineAssignHelper(Me.OccurredTimeOut, InlineAssignHelper(Me.OccurredDuration, InlineAssignHelper(Me.Status, InlineAssignHelper(Me.Comments, InlineAssignHelper(Me.Action, String.Empty)))))))))))))))))
        End Sub
    End Class
End Namespace











