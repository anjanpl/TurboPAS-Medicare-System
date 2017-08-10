
Namespace VisitelBusiness.DataObject.EVV
    Public Class VisitDataObject
        Inherits BaseDataObject

        Public Property Id As Int64

        ' This was the Activity for SANDATA

        ' This was the Activity for SANDATA
        ''' <summary>
        ''' Required; [Vendor Visit ID]
        ''' </summary>
        Public Property MyUniqueId As String

        ''' <summary>
        ''' Required; [Send -1 for new records and actual value for updates.]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property VestaVisitId As String

        ''' <summary>
        ''' Required; [Link to Client Record]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ClientIdVesta As String

        ''' <summary>
        ''' Required; [Link to Employee Record]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EmployeeIdVesta As String

        ''' <summary>
        ''' Required; [Link to Authorization Record]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AuthIdVesta As String

        ''' <summary>
        ''' Required; [Format:  MM/DD/YYYY]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property VisitDate As String

        ''' <summary>
        ''' Required; [Format: HH:MM:SS]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ScheduledTimeIn As String

        ''' <summary>
        ''' Required; [Format: HH:MM:SS]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ScheduledTimeOut As String

        ''' <summary>
        ''' Required
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property VisitUnits As String

        ''' <summary>
        ''' Required; [Scheduled location where services are being provided- LOV TBD- (Member Home, Community Facility, Family Home, Neighbor Home, Other)]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property VisitLocation As String

        ''' <summary>
        ''' Required; [DADS Service Group – Required for DADS clients]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DADSServiceGroup As String

        ''' <summary>
        ''' Required; [DADS Bill Code or MCO HCPCS Code]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property HCPCSBillCode As String

        ''' <summary>
        ''' Required; [DADS Service Code – Required for DADS clients]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DADSServiceCode As String

        ''' <summary>
        ''' Required; [Modifier 1 – Only required if applicable.]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Mod1 As String

        ''' <summary>
        ''' Required; [Modifier 2 – Only required if applicable.]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Mod2 As String

        ''' <summary>
        ''' Required; [Modifier 3 – Only required if applicable.]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Mod3 As String

        ''' <summary>
        ''' Required; [Modifier 4 – Only required if applicable.]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Mod4 As String

        Public Property Contract As String

        Public Property [Error] As String

        Public Property CalendarId As Int64

        Public Property ClientId As Int64

        Public Property ClientName As String

        Public Property EmployeeId As Int64

        Public Property EmployeeName As String

        Public Property UpdateDate As String

        Public Property UpdateBy As String

        Public Property Remarks As String

        Public Sub New()
            Me.MyUniqueId = InlineAssignHelper(Me.VestaVisitId, InlineAssignHelper(Me.ClientIdVesta,
                            InlineAssignHelper(Me.EmployeeIdVesta, InlineAssignHelper(Me.AuthIdVesta,
                            InlineAssignHelper(Me.VisitDate, InlineAssignHelper(Me.ScheduledTimeIn,
                            InlineAssignHelper(Me.ScheduledTimeOut, InlineAssignHelper(Me.VisitUnits,
                            InlineAssignHelper(Me.VisitLocation, InlineAssignHelper(Me.DADSServiceGroup,
                            InlineAssignHelper(Me.HCPCSBillCode, InlineAssignHelper(Me.DADSServiceCode,
                            InlineAssignHelper(Me.Mod1, InlineAssignHelper(Me.Mod2, InlineAssignHelper(Me.Mod3,
                            InlineAssignHelper(Me.Mod4, InlineAssignHelper(Me.Contract, InlineAssignHelper(Me.[Error],
                            InlineAssignHelper(Me.UpdateDate, InlineAssignHelper(Me.UpdateBy, InlineAssignHelper(Me.ClientName,
                            InlineAssignHelper(Me.Remarks, InlineAssignHelper(Me.EmployeeName, String.Empty)))))))))))))))))))))))

            Me.CalendarId = InlineAssignHelper(Me.ClientId, InlineAssignHelper(Me.EmployeeId, Int64.MinValue))
        End Sub
    End Class
End Namespace