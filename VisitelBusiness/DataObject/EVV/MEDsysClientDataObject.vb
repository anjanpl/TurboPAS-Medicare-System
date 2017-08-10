
Namespace VisitelBusiness.DataObject.EVV
    Public Class MEDsysClientDataObject
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
        ''' Required; [The Unique ID (Primary Key) for this Client from the system sending the record. External Primary Key]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ExternalId As String

        ''' <summary>
        ''' [MEDsys assigned Client ID. Primary Key]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ClientId As Int64

        ''' <summary>
        ''' [The 1-5 digit numeric ID used for this Client during EVV calls.]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ClientNumber As Int64

        ''' <summary>
        ''' [Client’s Title (Mr., Mrs., Ms., Dr., etc)]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Title As String

        ''' <summary>
        ''' Required; [Client’s First Name]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FirstName As String

        ''' <summary>
        ''' [Client’s Middle Initial]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MiddleInit As String

        ''' <summary>
        ''' Required; [Client’s Last Name]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LastName As String

        ''' <summary>
        ''' [Client’s Suffix (Jr., etc)]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Suffix As String

        ''' <summary>
        ''' [Client’s Birthdate]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Birthdate As String

        ''' <summary>
        ''' [Client’s Gender (‘M’, ‘F’ or ‘U’)]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Gender As String

        ''' <summary>
        ''' [The SSN/TID for this Client.]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property GIN As String

        ''' <summary>
        ''' [Client’s Address]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Address As String

        ''' <summary>
        ''' [Client’s Address (Additional Information such as Apt or Suite #)]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AddressTwo As String

        ''' <summary>
        ''' [Client’s City]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property City As String

        ''' <summary>
        ''' [Client’s State]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property State As String

        ''' <summary>
        ''' [Client’s Zip Code]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Zip As String

        ''' <summary>
        ''' [Client’s Home Phone]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Phone As String

        ''' <summary>
        ''' [Client’s Email Address]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Email As String

        ''' <summary>
        ''' [General Notes for this Client]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Notes As String

        ''' <summary>
        ''' [The serial number of Token in use for this client]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TokenSN As String

        ''' <summary>
        ''' [MEDsys assigned Admission ID. Primary Key]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property AdmissionId As Int64

        ''' <summary>
        ''' [Company the Admission is assigned to. Acceptable codes are assigned during implementation]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CompanyCode As String

        ''' <summary>
        ''' [Location the Admission is assigned to. Acceptable codes are assigned during implementation]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property LocationCode As String

        ''' <summary>
        ''' Required; [Program Code. Acceptable codes are assigned during implementation]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ProgramCode As String

        ''' <summary>
        ''' [Team Code. Acceptable codes are assigned during implementation]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property TeamCode As String

        ''' <summary>
        ''' [Region Code. Acceptable codes are assigned during implementation]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Region As String

        ''' <summary>
        ''' [MEDsys assigned Chart ID. Used to easily identify a particular Client / Admission.]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ChartId As String

        ''' <summary>
        ''' [Admission Status. Acceptable values are:
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
        ''' [Date of last status change]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StatusDate As String

        ''' <summary>
        ''' [Date of Referral]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ReferralDate As String

        ''' <summary>
        ''' [Admission Start Date]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StartDate As String

        ''' <summary>
        ''' [Admission End Date]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EndDate As String

        ''' <summary>
        ''' [Medicaid Id]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property MedicaidId As String

        ''' <summary>
        ''' [Primary Payor]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Payor As String

        ''' <summary>
        ''' [Payor Id]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PayorId As String

        ''' <summary>
        ''' [Payor Customer Number]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CustomerNumber As String

        ''' <summary>
        ''' [Payor Group Number]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property GroupNumber As String

        ''' <summary>
        ''' [Payor Service Group]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ServiceGroup As String

        ''' <summary>
        ''' [Payor Service Code. Not related to Service Code found on the Schedule Import specification]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PayorServiceCode As String

        ''' <summary>
        ''' [Payor assigned Contract Number]
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ContractNumber As String

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

        Public Property Client_Id As Int64

        Public Property UpdateDate As String

        Public Property UpdateBy As String

        Public Property Remarks As String

        Public Sub New()
            Me.AccountId = InlineAssignHelper(Me.ClientId, InlineAssignHelper(Me.ClientNumber, InlineAssignHelper(Me.AdmissionId,
                           InlineAssignHelper(Me.Client_Id, Int64.MinValue))))

            Me.ExternalId = InlineAssignHelper(Me.Title, InlineAssignHelper(Me.FirstName, InlineAssignHelper(Me.MiddleInit,
                            InlineAssignHelper(Me.LastName, InlineAssignHelper(Me.Suffix, InlineAssignHelper(Me.Birthdate,
                            InlineAssignHelper(Me.Gender, InlineAssignHelper(Me.GIN, InlineAssignHelper(Me.Address,
                            InlineAssignHelper(Me.AddressTwo, InlineAssignHelper(Me.City, InlineAssignHelper(Me.State,
                            InlineAssignHelper(Me.Zip, InlineAssignHelper(Me.Phone, InlineAssignHelper(Me.Email,
                            InlineAssignHelper(Me.Notes, InlineAssignHelper(Me.TokenSN, InlineAssignHelper(Me.CompanyCode,
                            InlineAssignHelper(Me.LocationCode, InlineAssignHelper(Me.ProgramCode, InlineAssignHelper(Me.TeamCode,
                            InlineAssignHelper(Me.Region, InlineAssignHelper(Me.ChartId, InlineAssignHelper(Me.Status,
                            InlineAssignHelper(Me.StatusDate, InlineAssignHelper(Me.ReferralDate, InlineAssignHelper(Me.StartDate,
                            InlineAssignHelper(Me.EndDate, InlineAssignHelper(Me.MedicaidId, InlineAssignHelper(Me.Payor,
                            InlineAssignHelper(Me.PayorId, InlineAssignHelper(Me.CustomerNumber, InlineAssignHelper(Me.GroupNumber,
                            InlineAssignHelper(Me.ServiceGroup, InlineAssignHelper(Me.PayorServiceCode, InlineAssignHelper(Me.ContractNumber,
                            InlineAssignHelper(Me.Action, String.Empty)))))))))))))))))))))))))))))))))))))
        End Sub
    End Class
End Namespace


