Namespace VisitelCommon
    ''' <summary>
    ''' It contains the static settings
    ''' </summary>
    ''' <remarks></remarks>
    Public Class StaticSettings
#Region "Nested type: AppSettingKey"

        Public Class AppSettingKey
            Public Shared PROJECT_NAME As String = "ProjectName"
            Public Shared LAST_PUBLISH_DATE As String = "LastPublishDate"
            Public Shared CURRENT_VERSION As String = "CurrentVersion"
            Public Const SMTP_SERVER As String = "SmtpServer"
            Public Const SMTP_PORT As String = "SmtpPort"
            Public Const SMTP_PASSWORD As String = "Smtppassword"
            Public Const SMTP_USERNAME As String = "SmtpuserName"
            Public Const SMTP_FROM_EMAIL As String = "SmtpFromEmail"
            Public Const ASYNC_POST_BACK_TIMEOUT As String = "AsyncPostBackTimeout"
        End Class

#End Region

#Region "Nested type: NavigateURL"

        Public Class NavigateURL
            Public Shared UNAUTHORIZED_ACCESS As String = "~/ErrorHandling/UnauthorizedAccess.aspx"
            Public Shared LOGOUT_URL As String = "LoginPage.aspx"
            Public Shared LOGIN_URL As String = "LoginPage.aspx"
        End Class

#End Region

#Region "Nested type: QueryStringKey"

        ''' <summary>
        ''' Query String keys
        ''' </summary>
        Public Class QueryStringKey
            Public Shared USER_ID As String = "UserID"
            Public Shared ROLE_ID As String = "RoleID"
            Public Shared USER_NAME As String = "UserName"
            Public Shared USER_PASSWORD As String = "Password"
            Public Shared USER_FULL_NAME As String = "User Full Name"
        End Class

#End Region

#Region "Nested type: SessionField"

        ''' <summary>
        ''' Session key constants
        ''' </summary>
        Public Class SessionField
            Public Shared USER_ID As String = "UserId"
            Public Shared ROLE_ID As String = "RoleID"
            Public Shared USER_NAME As String = "UserName"
            Public Shared USER_PASSWORD As String = "Password"
            Public Shared USER_FULL_NAME As String = "USERFULLNAME"

            Public Shared MENU_SOURCE As String = "MenuSource"
            Public Shared PRIVILEGE_COLLECTION As String = "PrivilegeCollection"
            Public Shared SERVICE_USER As String = "ServiceUser"

            Public Shared USER_PREF_THEME As String = "USER_PREF_THEME"

            Public Shared COMPANY_ID As String = "COMPANY_ID"
            Public Shared COMPANY_NAME As String = "COMPANY_NAME"

            Public Shared CLIENT_CASE_LIST As String = "CLIENT_CASE_LIST"
            Public Shared CLIENT_CASE_CARE_INFO_LIST As String = "CLIENT_CASE_CARE_INFO_LIST"
            Public Shared CLIENT_CASE_BILLING_INFO_LIST As String = "CLIENT_CASE_BILLING_INFO_LIST"
            Public Shared CLIENT_CASE_EVV_INFO_LIST As String = "CLIENT_CASE_EVV_INFO_LIST"

            Public Shared INDIVIDUAL_CLEAR_CLICKED As String = "INDIVIDUAL_CLEAR_CLICKED"
            Public Shared WEEKLY_CALENDAR_CLEAR_CLICKED As String = "WEEKLY_CALENDAR_CLEAR_CLICKED"
            Public Shared TASKS_CLEAR_CLICKED As String = "TASKS_CLEAR_CLICKED"
            Public Shared CASE_INFORMATION_CLEAR_CLICKED As String = "CASE_INFORMATION_CLEAR_CLICKED"
            Public Shared COMPLAINT_CLEAR_CLICKED As String = "COMPLAINT_CLEAR_CLICKED"
            Public Shared COMMENT_CLEAR_CLICKED As String = "COMMENT_CLEAR_CLICKED"

            Public Shared EMPLOYEE_CLEAR_CLICKED As String = "EMPLOYEE_CLEAR_CLICKED"

            Public Shared INDIVIDUAL_INFO_TAB_INDEX As String = "INDIVIDUAL_INFO_TAB_INDEX"
            Public Shared EMPLOYEE_INFO_TAB_INDEX As String = "EMPLOYEE_INFO_TAB_INDEX"
            Public Shared EDI_CENTRAL_TAB_INDEX As String = "EDI_CENTRAL_TAB_INDEX"

            Public Shared TEXTBOX_EVV_BILL_CODE As String = "TEXTBOX_EVV_BILL_CODE"
            Public Shared TEXTBOX_EVV_PROC_CODE_QUALIFIER As String = "TEXTBOX_EVV_PROC_CODE_QUALIFIER"
            Public Shared TEXTBOX_EVV_LAND_PHONE As String = "TEXTBOX_EVV_LAND_PHONE"

        End Class

        Public Class ReportFileName
            Public Shared CASE_INFORMATION_REPORT As String = "~\Reports\CaseInformationReport.rpt"
            Public Shared FAX_COVER_PAGE_REPORT As String = "~\Reports\FaxCoverPageReport.rpt"
            Public Shared COMPLAINT_LOG_REPORT As String = "~\Reports\ComplaintLogReport.rpt"
            Public Shared CLIENT_COMMENT_REPORT As String = "~\Reports\ClientCommentReport.rpt"
            Public Shared EMPLOYEE_COMMENT_REPORT As String = "~\Reports\EmployeeCommentReport.rpt"
            Public Shared COORDINATION_OF_CARE_REPORT As String = "~\Reports\CoordinationOfCareReport.rpt"
            Public Shared OIG_FEDERAL_SEARCH_REPORT As String = "~\Reports\OigFederalSearchReport.rpt"
            Public Shared OIG_STATE_SEARCH_REPORT As String = "~\Reports\OigStateSearchReport.rpt"
            Public Shared CARE_SUMMARY_CUMULATIVE_TIMESHEET_REPORT As String = "~\Reports\CareSummary\CareSummaryCumulativeTimesheetReport.rpt"
            Public Shared CARE_SUMMARY_PAYROLL_REPORT As String = "~\Reports\CareSummary\CareSummaryPayrollReport.rpt"
            Public Shared CARE_SUMMARY_SERVICE_HOURS_REPORT As String = "~\Reports\CareSummary\CareSummaryServiceHoursReport.rpt"
            Public Shared CARE_SUMMARY_EMPLOYEE_TOTALS_REPORT As String = "~\Reports\CareSummary\CareSummaryEmployeeTotalsReport.rpt"
            Public Shared CARE_SUMMARY_INDIVIDUAL_TOTALS_REPORT As String = "~\Reports\CareSummary\CareSummaryIndividualTotalsReport.rpt"
            Public Shared CARE_SUMMARY_TIME_SUMMARY_REPORT As String = "~\Reports\CareSummary\CareSummaryTimeSummaryReport.rpt"
            Public Shared CARE_SUMMARY_CLAIM_SUBMISSION_HISTORY_REPORT As String = "~\Reports\CareSummary\CareSummaryClaimSubmissionHistoryReport.rpt"

            Public Shared INDIVIDUAL_BIRTHDAY_REPORT As String = "~\Reports\Individual\IndividualBirthdayReport.rpt"
            Public Shared INDIVIDUAL_CS_ACTIVITY_CENSUS_REPORT As String = "~\Reports\Individual\IndividualCsActivityCensusReport.rpt"
            Public Shared INDIVIDUAL_ANNUAL_RE_CERT_REPORT As String = "~\Reports\Individual\IndividualAnnualReCertReport.rpt"
            Public Shared INDIVIDUAL_DETAIL_REPORT As String = "~\Reports\Individual\IndividualDetailReport.rpt"
            Public Shared INDIVIDUAL_LIST_REPORT As String = "~\Reports\Individual\IndividualListReport.rpt"
            Public Shared INDIVIDUAL_LABELS_REPORT As String = "~\Reports\Individual\IndividualLabelsReport.rpt"
            Public Shared INDIVIDUAL_PRACTITIONERS_STATEMENT_REPORT As String = "~\Reports\Individual\IndividualPractitionersStatementReport.rpt"
            Public Shared INDIVIDUAL_DOCTOR_FAX_REPORT As String = "~\Reports\Individual\IndividualDoctorFaxReport.rpt"
            Public Shared INDIVIDUAL_REFERRAL_INTAKE_REPORT As String = "~\Reports\Individual\IndividualReferralIntakeReport.rpt"
            Public Shared INDIVIDUAL_FOLDER_COVER_REPORT As String = "~\Reports\Individual\IndividualFolderCoverReport.rpt"
            Public Shared INDIVIDUAL_RE_AUTHORIZATIONS_DUE_REPORT As String = "~\Reports\Individual\IndividualReAuthorizationsDueReport.rpt"
            Public Shared INDIVIDUAL_HEALTH_ASSESSMENTS_DUE_REPORT As String = "~\Reports\Individual\IndividualHealthAssessmentsDueReport.rpt"
            Public Shared INDIVIDUAL_SUPERVISORY_VISITS_DUE_REPORT As String = "~\Reports\Individual\IndividualSupervisoryVisitsDueReport.rpt"
            Public Shared INDIVIDUAL_WEEKLY_UNITS_REPORT As String = "~\Reports\Individual\IndividualWeeklyUnitsReport.rpt"
            Public Shared INDIVIDUAL_DOCTORS_CLIENTS_REPORT As String = "~\Reports\Individual\IndividualDoctorsClientsReport.rpt"

            Public Shared EMPLOYEE_EVAL As String = "~\Reports\Employee\Employee12MonthEvalReport.rpt"
            Public Shared EMPLOYEE_ANNUAL_CRIMCHECK As String = "~\Reports\Employee\EmployeeAnnualCrimcheckReport.rpt"
            Public Shared EMPLOYEE_BIRTHDAY As String = "~\Reports\Employee\EmployeeBirthdayReport.rpt"
            Public Shared EMPLOYEE_OIG As String = "~\Reports\Employee\EmployeeOigReport.rpt"
            Public Shared EMPLOYEE_LIST As String = "~\Reports\Employee\EmployeeListReport.rpt"
            Public Shared EMPLOYEE_UNLICENSEDS_WITH_INDIVIDUAL_CONTACT As String = "~\Reports\Employee\EmployeeUnlicensedIndividualReport.rpt"
            Public Shared EMPLOYEE_LICENSEDS_WITH_INDIVIDUAL_CONTACT As String = "~\Reports\Employee\EmployeeLicensedIndividualReport.rpt"
            Public Shared EMPLOYEE_NEW_PAYROLL As String = "~\Reports\Employee\EmployeeNewPayrollReport.rpt"
            Public Shared EMPLOYEE_LABEL As String = "~\Reports\Employee\EmployeeLabelReport.rpt"
            Public Shared EMPLOYEE_SEPARATION As String = "~\Reports\Employee\EmployeeSeparationReport.rpt"
            Public Shared EMPLOYEE_OIG_MONTHLY_EXCLUSION_LIST As String = "~\Reports\Employee\EmployeeOigMonthlyExclusionListReport.rpt"

            Public Shared SERVICE_FORMS_ATTENDANT_ORIENTATION_SUPERVISOR_VISIT As String = "~\Reports\ServiceForms\AttendantOrientationSupVisitReport.rpt"
            Public Shared SERVICE_FORMS_ATTENDANT_ORIENTATION As String = "~\Reports\ServiceForms\AttendantOrientationReport.rpt"
            Public Shared SERVICE_FORMS_ATTENDANT_ORIENTATION_MDCP As String = "~\Reports\ServiceForms\AttendantOrientationMdcpReport.rpt"
            Public Shared SERVICE_FORMS_SUPERVISORY_VISIT As String = "~\Reports\ServiceForms\SupervisoryVisitReport.rpt"
            Public Shared SERVICE_FORMS_HEALTH_ASSESSMENT_SERVICE_DELIVERY_PLAN As String = "~\Reports\ServiceForms\HealthAssessmentServiceDeliveryPlanReport.rpt"
            Public Shared SERVICE_FORMS_SERVICE_DELIVERY_PLAN As String = "~\Reports\ServiceForms\ServiceDeliveryPlanReport.rpt"
            Public Shared SERVICE_FORMS_SERVICE_DELIVERY_PLAN_MDCP As String = "~\Reports\ServiceForms\ServiceDeliveryPlanMdcpReport.rpt"
            Public Shared SERVICE_FORMS_INDIVIDUAL_EVALUATION As String = "~\Reports\ServiceForms\IndividualEvaluationReport.rpt"
            Public Shared SERVICE_FORMS_ATTENDANTS_INDIVIDUALS As String = "~\Reports\ServiceForms\AttendantsIndividualsReport.rpt"
            Public Shared SERVICE_FORMS_INDIVIDUALS_ATTENDANTS As String = "~\Reports\ServiceForms\IndividualsAttendantsReport.rpt"
            Public Shared SERVICE_FORMS_CALENDAR As String = "~\Reports\ServiceForms\CalendarReport.rpt"

            Public Shared TIMESHEET_REQUIRED_REGULAR As String = "~\Reports\Timesheets\TsRegularSignReqReport.rpt"
            Public Shared TIMESHEET_OPTIONAL_REGULAR As String = "~\Reports\Timesheets\TsRegularSignOptReport.rpt"
            Public Shared TIMESHEET_REQUIRED_SAMPLE As String = "~\Reports\Timesheets\TsSampleSignReqReport.rpt"
            Public Shared TIMESHEET_OPTIONAL_SAMPLE As String = "~\Reports\Timesheets\TsSampleSignOptReport.rpt"
            Public Shared TIMESHEET_REQUIRED_PRE_PRINTED As String = "~\Reports\Timesheets\TsPrePrintedSignReqReport.rpt"
            Public Shared TIMESHEET_OPTIONAL_PRE_PRINTED As String = "~\Reports\Timesheets\TsPrePrintedSignOptReport.rpt"
            Public Shared TIMESHEET_REQUIRED_DAILY_SCHEDULE As String = "~\Reports\Timesheets\TsDailyScheduleSignReqReport.rpt"
            Public Shared TIMESHEET_OPTIONAL_DAILY_SCHEDULE As String = "~\Reports\Timesheets\TsDailyScheduleSignOptReport.rpt"
            Public Shared TIMESHEET_REQUIRED_MDCP As String = "~\Reports\Timesheets\TsMdcpSignReqReport.rpt"
            Public Shared TIMESHEET_OPTIONAL_MDCP As String = "~\Reports\Timesheets\TsMdcpSignOptReport.rpt"
            Public Shared TIMESHEET_REQUIRED_DAILY_TASK_SHEET As String = "~\Reports\Timesheets\TsDailyTaskSheetSignReqReport.rpt"
            Public Shared TIMESHEET_REQUIRED_CBA As String = "~\Reports\Timesheets\TsCbaSignReqReport.rpt"
            Public Shared TIMESHEET_OPTIONAL_CBA As String = "~\Reports\Timesheets\TsCbaSignOptReport.rpt"

        End Class

        Public Class VestaUploadProcess
            Public Shared IS_CLIENT_UPLOAD As String = "IS_CLIENT_UPLOAD"
            Public Shared IS_AUTHORIZATION_UPLOAD As String = "IS_AUTHORIZATION_UPLOAD"
            Public Shared IS_EMPLOYEE_UPLOAD As String = "IS_EMPLOYEE_UPLOAD"
            Public Shared IS_VISITS_UPLOAD As String = "IS_VISITS_UPLOAD"
            Public Shared IS_ALL_UPLOAD As String = "IS_ALL_UPLOAD"
            Public Shared AGENCY_ID As String = "AGENCY_ID"
            Public Shared AGENCY_PASSWORD As String = "AGENCY_PASSWORD"
            Public Shared BATCH_ID As String = "BATCH_ID"
            Public Shared API_LOG_DETAIL_INITIAL As String = "API_LOG_DETAIL_INITIAL"


        End Class

        Public Class MedSysUploadProcess
            Public Shared IS_CLIENT_UPLOAD As String = "IS_CLIENT_UPLOAD"
            Public Shared IS_AUTHORIZATION_UPLOAD As String = "IS_AUTHORIZATION_UPLOAD"
            Public Shared IS_STAFF_UPLOAD As String = "IS_STAFF_UPLOAD"
            Public Shared IS_SCHEDULE_UPLOAD As String = "IS_SCHEDULE_UPLOAD"
            Public Shared IS_ALL_UPLOAD As String = "IS_ALL_UPLOAD"
            Public Shared ACCOUNT_ID As String = "ACCOUNT_ID"
            Public Shared AGENCY_PASSWORD As String = "AGENCY_PASSWORD"

        End Class

#End Region
    End Class
End Namespace
