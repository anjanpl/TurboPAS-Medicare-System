
Namespace VisitelBusiness.DataObject.EVV
    Public Class MEDsysStaffDataObject
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
        ''' Required; [The Unique ID (Primary Key) for this Staff from the system sending the record. External Primary Key]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ExternalId As String

        ''' <summary>
        ''' [MEDsys assigned Staff ID. Primary Key]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StaffId As Int64

        ''' <summary>
        ''' [The 1-5 digit numeric ID used for this Staff Member during EVV calls.]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StaffNumber As Int64

        ''' <summary>
        ''' [Staff Title (Mr., Mrs., Ms., Dr., etc)]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Title As String

        ''' <summary>
        ''' Required; [Staff First Name]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FirstName As String

        ''' <summary>
        ''' [Staff Middle Initial]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MiddleInit As String

        ''' <summary>
        ''' Required; [Staff Last Name]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LastName As String

        ''' <summary>
        ''' [Staff Suffix (Jr., etc)]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Suffix As String

        ''' <summary>
        ''' [Staff Birthdate]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Birthdate As String

        ''' <summary>
        ''' [Staff Gender (‘M’, ‘F’ or ‘U’)]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Gender As String

        ''' <summary>
        ''' [The SSN/TID for this Staff Member.]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property GIN As String

        ''' <summary>
        ''' [Staff Address]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Address As String

        ''' <summary>
        ''' [Staff Address (Additional Information such as Apt or Suite #)]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AddressTwo As String

        ''' <summary>
        ''' [Staff City]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property City As String

        ''' <summary>
        ''' [Staff State]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property State As String

        ''' <summary>
        ''' [Staff Zip Code]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Zip As String

        ''' <summary>
        ''' [Staff Home Phone]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Phone As String

        ''' <summary>
        ''' [Staff Email Address]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Email As String

        ''' <summary>
        ''' [General Notes for this Staff Member]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Notes As String

        ''' <summary>
        ''' Required; [Position Code. Acceptable codes are assigned during implementation]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PositionCode As String

        ''' <summary>
        ''' [Team Code. Acceptable codes are assigned during implementation]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TeamCode As String

        ''' <summary>
        ''' [A semi-colon (;) separated list of Company Codes to which this staff member should be associated. Each code can be up to 10 characters long.  
        ''' Highly recommended this field be included.]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CompanyCode As String

        ''' <summary>
        ''' [A semi-colon (;) separated list of Location Codes to which this staff member should be associated.  Each code can be up to 10 characters long. 
        ''' Highly recommended this field be included.]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LocationCode As String

        ''' <summary>
        ''' [Employment Status. Acceptable values are:
        ''' 1 – Pending / Referred
        ''' 2 – Active
        ''' 3 – On Hold
        ''' 4 – Inactive / Discharged
        ''' 5 – Cancelled / Refused]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Status As String

        ''' <summary>
        ''' [Date of last status change.]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatusDate As String

        ''' <summary>
        ''' [Staff Employment Start Date]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StartDate As String

        ''' <summary>
        ''' [Staff Employment End Date]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EndDate As String

        ''' <summary>
        ''' [What action to perform with this record. Acceptable
        ''' values are:
        ''' N – New Record
        ''' U – Updated Record]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Action As String

        Public Property EmployeeId As Int64

        Public Property UpdateDate As String

        Public Property UpdateBy As String

        Public Property Remarks As String

        Public Sub New()
            Me.AccountId = InlineAssignHelper(Me.StaffId, InlineAssignHelper(Me.StaffNumber, Int64.MinValue))

            Me.ExternalId = InlineAssignHelper(Me.Title, InlineAssignHelper(Me.FirstName, InlineAssignHelper(Me.MiddleInit,
                            InlineAssignHelper(Me.LastName, InlineAssignHelper(Me.Suffix, InlineAssignHelper(Me.Birthdate,
                            InlineAssignHelper(Me.Gender, InlineAssignHelper(Me.GIN, InlineAssignHelper(Me.Address,
                            InlineAssignHelper(Me.AddressTwo, InlineAssignHelper(Me.City, InlineAssignHelper(Me.State,
                            InlineAssignHelper(Me.Zip, InlineAssignHelper(Me.Phone, InlineAssignHelper(Me.Email,
                            InlineAssignHelper(Me.Notes, InlineAssignHelper(Me.PositionCode, InlineAssignHelper(Me.TeamCode,
                            InlineAssignHelper(Me.CompanyCode, InlineAssignHelper(Me.LocationCode, InlineAssignHelper(Me.Status,
                            InlineAssignHelper(Me.StatusDate, InlineAssignHelper(Me.StartDate, InlineAssignHelper(Me.EndDate,
                            InlineAssignHelper(Me.Action, String.Empty)))))))))))))))))))))))))
        End Sub
    End Class
End Namespace

