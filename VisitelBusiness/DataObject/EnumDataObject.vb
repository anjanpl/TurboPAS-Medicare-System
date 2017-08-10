Imports System.Collections.Generic
Imports System.Reflection
Imports System.ComponentModel

Namespace VisitelBusiness.DataObject
    Public Class EnumDataObject

        Public Enum EVVName
            <Description("Vesta")> _
            Vesta = 1
            <Description("MEDsys")> _
            MEDsys = 2
        End Enum

        Public Enum UserFilter
            <Description("User Name")> _
            UserName = 1
            <Description("Email")> _
            Email = 2
        End Enum

        Public Enum FTPOperationType
            <Description("UploadFile")> _
            UploadFile = 0
            <Description("DownloadFile")> _
            DownloadFile = 1
            <Description("DeleteFile")> _
            DeleteFile = 2
            <Description("RenameFile")> _
            RenameFile = 3
            <Description("CreateDirectory")> _
            CreateDirectory = 4
            <Description("GetFileCreatedDateTime")> _
            GetFileCreatedDateTime = 5
            <Description("GetFileSize")> _
            GetFileSize = 6
            <Description("DirectoryListSimple")> _
            DirectoryListSimple = 7
            <Description("DirectoryListDetailed")> _
            DirectoryListDetailed = 8
        End Enum

        Public Enum EdiCodesType
            <Description("EDICentral")> _
            EDICentral = 0
            <Description("EDICorrectedClaims")> _
            EDICorrectedClaims = 1
        End Enum

        Public Enum PayPeriodFor
            <Description("CareSummary")> _
            CareSummary = 0
            <Description("EDISubmission")> _
            EDISubmission = 1
        End Enum

        Public Enum ListFor
            <Description("SAVE")> _
            SAVE = 1
            <Description("DELETE")> _
            DELETE = 2
        End Enum

        Public Enum DBOperation
            <Description("INSERT")> _
            INSERT = 1
            <Description("UPDATE")> _
            UPDATE = 2
            <Description("DELETE")> _
            DELETE = 3
        End Enum

        Public Enum TimeSheet
            Yes = 1
            No = 0
            All = -1
        End Enum

        Public Enum Billed
            Yes = 1
            No = 0
            All = -1
        End Enum

        Public Enum STATUS
            I = 0
            A = 1
        End Enum

        Public Enum PRIORITY
            <EnumDescription("Priority")> _
            Yes = 1
            <EnumDescription("NON")> _
            No = 0
        End Enum

        Public Enum REHIRE
            Yes = 1
            No = 0
        End Enum

        Public Enum ProviderSignatureOnFile
            Y = 1
            N = 0
        End Enum

        Public Enum AssignmentOfBenefitsIndicator
            Y = 1
            N = 0
            W = 2
        End Enum

        Public Enum EVVPriority
            One = 1
            Two = 2
            Three = 3
        End Enum

        Public Enum GENDER
            Male = 1
            Female = 2
        End Enum

        Public Enum EmergencyDisasterCategory
            One = 1
            Two = 2
            Three = 3
            Four = 4
            Five = 5
            Six = 6
            Seven = 7
            Eight = 8
            Nine = 9
            Ten = 10
        End Enum

        Public Enum CalendarScheduleStatus
            A = 1
            I = 0
        End Enum

        Public Enum EDILoginStatus
            A = 1
            I = 0
        End Enum

        Public Enum EmploymentStatus
            <Description("EXPIRED")> _
            EXPIRED = 0
            <Description("CURRENT")> _
            CURRENT = 1
        End Enum

        Public Enum ControlType
            <Description("Label")> _
            Label = 0
            <Description("TextBox")> _
            TextBox = 1
            <Description("DropDownList")> _
            DropDownList = 2
            <Description("CheckBox")> _
            CheckBox = 3
        End Enum

        Public Enum ClientListFor
            <Description("Individual")> _
            Individual = 0
            <Description("PayPeriod")> _
            PayPeriod = 1
            <Description("CareSummary")> _
            CareSummary = 2
            <Description("EDICorrectedClaims")> _
            EDICorrectedClaims = 3
            <Description("VestaClient")> _
            VestaClient = 4

        End Enum

        Public Enum ClientTypeListFor
            <Description("Individual")> _
            Individual = 0
            <Description("CareSummary")> _
            CareSummary = 1
            <Description("Vesta")> _
            Vesta = 2
        End Enum

        Public Enum CareSummaryReportType
            <EnumDescription("Cumilative Timesheet Report")> _
            CumilativeTimesheetReport = 0
            <EnumDescription("Payroll Report")> _
            PayrollReport = 1
            <EnumDescription("Service Hours")> _
            ServiceHours = 2
            <EnumDescription("Employee Totals / Time Details")> _
            EmployeeTotals = 3
            <EnumDescription("Individual Totals / Individual Hrs")> _
            IndividualTotals = 4
            <EnumDescription("Time Summary")> _
            TimeSummary = 5
            <EnumDescription("Claim Submission History")> _
            ClaimSubmissionHistory = 6
        End Enum

        Public Enum ReportName
            <Description("Case Information")> _
            CaseInformation

            <Description("Fax Cover Page")> _
            FaxCoverPage

            <Description("Complaint Log")> _
            ComplaintLog

            <Description("Client Comment")> _
            ClientComment

            <Description("Employee Comment")> _
            EmployeeComment

            <Description("Coordination Of Care")> _
            CoordinationOfCare

            <Description("Oig Federal Exclusions")> _
            OigFederalExclusions

            <Description("Oig State Exclusions")> _
            OigStateExclusions

            <Description("Care Summary Cumulative Timesheet Report")> _
            CareSummaryCumulativeTimesheetReport

            <Description("Care Summary Payroll Report")> _
            CareSummaryPayrollReport

            <Description("Care Summary Service Hours Report")> _
            CareSummaryServiceHoursReport

            <Description("Care Summary Employee Totals Report")> _
            CareSummaryEmployeeTotalsReport

            <Description("Care Summary Individual Totals Report")> _
            CareSummaryIndividualTotalsReport

            <Description("Care Summary Time Summary Report")> _
            CareSummaryTimeSummaryReport

            <Description("Care Summary Claim Submission History Report")> _
            CareSummaryClaimSubmissionHistoryReport

            <Description("Individuals Birthday Report")> _
            IndividualBirthday

            <Description("Individual Care Summary Activity Census")> _
            IndividualCareSummaryActivityCensus

            <Description("Individual Annual ReCert")> _
            IndividualAnnualReCert

            <Description("Individual Detail Report")> _
            IndividualDetailReport

            <Description("Individual List")> _
            IndividualList

            <Description("Individual Labels")> _
            IndividualLabels

            <Description("Individual Practitioners Statement")> _
            IndividualPractitionersStatement

            <Description("Individual Doctor Fax")> _
            IndividualDoctorFax

            <Description("Individual Referral Or Intake")> _
            IndividualReferralOrIntake

            <Description("Individual Folder Cover")> _
            IndividualFolderCover

            <Description("Individual ReAuthorizations Due")> _
            IndividualReAuthorizationsDue

            <Description("Individual Health Assessments Due")> _
            IndividualHealthAssessmentsDue

            <Description("Individual Supervisory Visits Due")> _
            IndividualSupervisoryVisitsDue

            <Description("Individual Weekly Units")> _
            IndividualWeeklyUnits

            <Description("Individual Doctors Clients")> _
            IndividualDoctorsClients

            <Description("Employee Eval")> _
            EmployeeEval

            <Description("Employee Annual Crimcheck")> _
            EmployeeAnnualCrimcheck

            <Description("Employee Birthday")> _
            EmployeeBirthday

            <Description("Employee OIG")> _
            EmployeeOig

            <Description("Employee List")> _
            EmployeeList

            <Description("Employee Unlicenseds with Individual Contact")> _
            EmployeeUnlicensedsWithIndividualContact

            <Description("Employee Licenseds with Individual Contact")> _
            EmployeeLicensedsWithIndividualContact

            <Description("Employee New Payroll")> _
            EmployeeNewPayroll

            <Description("Employee Label")> _
            EmployeeLabel

            <Description("Employee Separation")> _
            EmployeeSeparation

            <Description("Employee OIG Monthly Exclusion List")> _
            EmployeeOigMonthlyExclusionList

            <Description("Service Forms Attendant Orientation/Supervisor Visit")> _
            ServiceFormsAttendantOrientationOrSupervisorVisit

            <Description("Service Forms Attendant Orientation")> _
            ServiceFormsAttendantOrientation

            <Description("Service Forms Attendant Orientation (MDCP)")> _
            ServiceFormsAttendantOrientationMdcp

            <Description("Service Forms Supervisory Visit")> _
            ServiceFormsSupervisoryVisit

            <Description("Service Forms Health Assessment/Service Delivery Plan")> _
            ServiceFormsHealthAssessmentOrServiceDeliveryPlan

            <Description("Service Forms Service Delivery Plan")> _
            ServiceFormsServiceDeliveryPlan

            <Description("Service Forms Service Delivery Plan (MDCP)")> _
            ServiceFormsServiceDeliveryPlanMdcp

            <Description("Service Forms Individual Evaluation")> _
            ServiceFormsIndividualEvaluation

            <Description("Service Forms Attendant's Individuals")> _
            ServiceFormsAttendantsIndividuals

            <Description("Service Forms Individual's Attendants")> _
            ServiceFormsIndividualsAttendants

            <Description("Service Forms Calendar")> _
            ServiceFormsCalendar

            <Description("Timesheet Regular (Sign Required)")> _
            TimesheetRequiredRegular

            <Description("Timesheet Regular (Sign Optional)")> _
            TimesheetOptionalRegular

            <Description("Timesheet Sample (Sign Required)")> _
            TimesheetRequiredSample

            <Description("Timesheet Sample (Sign Optional)")> _
            TimesheetOptionalSample

            <Description("Timesheet Pre-Printed (Sign Required)")> _
            TimesheetRequiredPrePrinted

            <Description("Timesheet Pre-Printed (Sign Optional)")> _
            TimesheetOptionalPrePrinted

            <Description("Timesheet Daily Schedule (Sign Required)")> _
            TimesheetRequiredDailySchedule

            <Description("Timesheet Daily Schedule (Sign Optional)")> _
            TimesheetOptionalDailySchedule

            <Description("Timesheet MDCP (Sign Required)")> _
            TimesheetRequiredMdcp

            <Description("Timesheet MDCP (Sign Optional)")> _
            TimesheetOptionalMdcp

            <Description("Timesheet Daily Task Sheet (Sign Optional)")> _
            TimesheetRequiredDailyTaskSheet

            <Description("Timesheet CBA (Sign Required)")> _
            TimesheetRequiredCba

            <Description("Timesheet CBA (Sign Optional)")> _
            TimesheetOptionalCba

        End Enum

        Public Enum EdiCodes

            <Description("Authorization Information Qualifier")> _
            AuthorizationInformationQualifier

            <Description("Security Information Qualifier")> _
            SecurityInformationQualifier

            <Description("Interchange ID Qualifier")> _
            InterchangeIdQualifier

            <Description("Acknowledgement Requested")> _
            AcknowledgementRequested

            <Description("Usage Indicator")> _
            UsageIndicator

            <Description("Entity ID Code")> _
            EntityIdCode

            <Description("Entity Type Qualifier")> _
            EntityTypeQualifier

            <Description("Reference ID Qualifier")> _
            ReferenceIdQualifier

            <Description("ID Code Qualifier")> _
            IdCodeQualifier

            <Description("Entity Identifier Code")> _
            EntityIdentifierCode

            <Description("Payer Responsibility Sequence Number Code")> _
            PayerResponsibilitySequenceNumberCode

            <Description("Relationship Code")> _
            RelationshipCode

            <Description("Claim Filing Indicator Code")> _
            ClaimFilingIndicatorCode

        End Enum

        Public Enum EdiCodesTableName

            <Description("Interchange Control Header")> _
            InterchangeControlHeader

            <Description("Referring Provider")> _
            ReferringProvider

            <Description("Provider")> _
            Provider

            <Description("Billing Provider")> _
            BillingProvider

            <Description("Rendering Provider")> _
            RenderingProvider

            <Description("Submitter")> _
            Submitter

            <Description("Subscriber")> _
            Subscriber

        End Enum

        Public Enum EdiCodesColumnName

            <Description("ISA01")> _
            ISA01

            <Description("ISA03")> _
            ISA03

            <Description("ISA05")> _
            ISA05

            <Description("ISA14")> _
            ISA14

            <Description("ISA15")> _
            ISA15

            <Description("NM101")> _
            NM101

            <Description("NM102")> _
            NM102

            <Description("REF01")> _
            REF01

            <Description("NM108")> _
            NM108

            <Description("SBR01")> _
            SBR01

            <Description("SBR02")> _
            SBR02

            <Description("SBR09")> _
            SBR09

        End Enum

        Public Enum ProviderCode

            <Description("BI")> _
            BI

            <Description("PT")> _
            PT

        End Enum


        Public NotInheritable Class EnumHelper

            ''' <summary>
            ''' Retrieve the description on the enum, e.g.
            ''' Description("EXPIRED")
            '''  EXPIRED = 0
            ''' Then when you pass in the enum, it will retrieve the description
            ''' </summary>
            ''' <param name="enumWithDescription"></param>
            ''' <returns>A string representing the friendly name</returns>
            ''' <remarks></remarks>
            Public Shared Function GetDescription(enumWithDescription As [Enum]) As String
                '#Region "test"
                Dim type As Type = enumWithDescription.[GetType]()

                Dim memInfo As MemberInfo() = type.GetMember(enumWithDescription.ToString())

                If memInfo IsNot Nothing AndAlso memInfo.Length > 0 Then
                    Dim attrs As Object() = memInfo(0).GetCustomAttributes(GetType(DescriptionAttribute), False)

                    If attrs IsNot Nothing AndAlso attrs.Length > 0 Then
                        Return DirectCast(attrs(0), DescriptionAttribute).Description
                    End If
                End If
                '#End Region

                Return enumWithDescription.ToString()
            End Function

        End Class

        Public NotInheritable Class Enumeration

            Public Shared Function GetAll(Of TEnum As Structure)() As IDictionary(Of Integer, String)
                Dim enumerationType = GetType(TEnum)

                If Not enumerationType.IsEnum Then
                    Throw New ArgumentException("Enumeration type is expected.")
                End If

                Dim dictionary = New Dictionary(Of Integer, String)()

                For Each value As Integer In [Enum].GetValues(enumerationType)
                    Dim name = [Enum].GetName(enumerationType, value)
                    dictionary.Add(value, name)
                Next

                Return dictionary
            End Function
        End Class

    End Class
End Namespace

