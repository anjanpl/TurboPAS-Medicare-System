Imports System.Data.SqlClient
Imports System.Collections.Specialized

Namespace VisitelBusiness
    Public Class BLIndividualReports

        Private sqlConnection As SqlConnection

        Public Function GetBirthdayReportData(VisitelConnectionString As String, QueryStringCollection As NameValueCollection) As DataSet

            Dim CrystalReportDataSet As New DataSet()

            Try
                Dim fromMonth As Integer = 0, toMonth As Integer = 0, companyId As Integer = 0
                Integer.TryParse(QueryStringCollection("FromMonth"), fromMonth)
                Integer.TryParse(QueryStringCollection("ToMonth"), toMonth)
                Integer.TryParse(QueryStringCollection("CompanyId"), companyId)

                sqlConnection = New SqlConnection(VisitelConnectionString)

                Dim sqlCommand As SqlCommand = New SqlCommand("[TurboDB.SelectIndividualBirthdayReport]", sqlConnection)
                sqlCommand.CommandType = CommandType.StoredProcedure
                sqlCommand.Parameters.AddWithValue("FromMonth", fromMonth)
                sqlCommand.Parameters.AddWithValue("ToMonth", toMonth)
                sqlCommand.Parameters.AddWithValue("CompanyId", companyId)

                Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter(sqlCommand)
                Dim dataTable As DataTable = New DataTable("Birthdays")
                dataAdapter.Fill(dataTable)
                CrystalReportDataSet.Tables.Add(dataTable)

            Catch ex As Exception

            Finally

            End Try

            Return CrystalReportDataSet

        End Function

        Public Function GetCsActivityCensusReportData(VisitelConnectionString As String, QueryStringCollection As NameValueCollection) As DataSet

            Dim CrystalReportDataSet As New DataSet()

            Try
                Dim fromDate As DateTime = DateTime.MinValue, toDate As DateTime = DateTime.MinValue
                Dim companyId As Integer = 0

                DateTime.TryParse(QueryStringCollection("FromDate"), fromDate)
                DateTime.TryParse(QueryStringCollection("ToDate"), toDate)
                Integer.TryParse(QueryStringCollection("CompanyId"), companyId)

                sqlConnection = New SqlConnection(VisitelConnectionString)

                Dim sqlCommand As SqlCommand = New SqlCommand("[TurboDB.SelectIndividualCsActivityCensusReport]", sqlConnection)
                sqlCommand.CommandType = CommandType.StoredProcedure
                sqlCommand.Parameters.AddWithValue("FromDate", fromDate)
                sqlCommand.Parameters.AddWithValue("ToDate", toDate)
                sqlCommand.Parameters.AddWithValue("CompanyId", companyId)

                Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter(sqlCommand)
                Dim dataTable As DataTable = New DataTable("CsActivityCensus")
                dataAdapter.Fill(dataTable)
                CrystalReportDataSet.Tables.Add(dataTable)

            Catch ex As Exception

            Finally

            End Try

            Return CrystalReportDataSet

        End Function

        Public Function GetAnnualReCertReportData(VisitelConnectionString As String, QueryStringCollection As NameValueCollection) As DataSet

            Dim CrystalReportDataSet As New DataSet()

            Try
                Dim fromDate As DateTime = DateTime.MinValue, toDate As DateTime = DateTime.MinValue
                Dim companyId As Integer = 0

                DateTime.TryParse(QueryStringCollection("FromDate"), fromDate)
                DateTime.TryParse(QueryStringCollection("ToDate"), toDate)
                Integer.TryParse(QueryStringCollection("CompanyId"), companyId)

                sqlConnection = New SqlConnection(VisitelConnectionString)

                Dim sqlCommand As SqlCommand = New SqlCommand("[TurboDB.SelectIndividualAnnualReCertReport]", sqlConnection)
                sqlCommand.CommandType = CommandType.StoredProcedure
                sqlCommand.Parameters.AddWithValue("FromDate", fromDate)
                sqlCommand.Parameters.AddWithValue("ToDate", toDate)
                sqlCommand.Parameters.AddWithValue("CompanyId", companyId)

                Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter(sqlCommand)
                Dim dataTable As DataTable = New DataTable("AnnualReCert")
                dataAdapter.Fill(dataTable)
                CrystalReportDataSet.Tables.Add(dataTable)

            Catch ex As Exception

            Finally

            End Try

            Return CrystalReportDataSet

        End Function

        Public Function GetIndividualReportsData(ReportId As Integer, VisitelConnectionString As String, QueryStringCollection As NameValueCollection, ReportParameters As OrderedDictionary) As DataSet

            Dim CrystalReportDataSet As New DataSet()

            Try
                sqlConnection = New SqlConnection(VisitelConnectionString)
                Dim storedProcedureName As String = String.Empty
                Dim IsAuthenticationDue As Boolean = False, IsHealthAssessmentDue As Boolean = False,
                    IsSupervisorVisitDue As Boolean = False

                Select Case (ReportId)
                    Case 1
                        storedProcedureName = "[TurboDB.SelectIndividualDetailReport]"
                        Exit Select
                    Case 2
                        storedProcedureName = "[TurboDB.SelectIndividualListReport]"
                        Exit Select
                    Case 3
                        storedProcedureName = "[TurboDB.SelectIndividualLabelsReport]"
                        Exit Select
                    Case 4
                        storedProcedureName = "[TurboDB.SelectIndividualPractitionersStatementReport]"
                        Exit Select
                    Case 5
                        storedProcedureName = "[TurboDB.SelectIndividualDoctorFaxReport]"
                        Exit Select
                    Case 6
                        storedProcedureName = "[TurboDB.SelectIndividualReferralIntakeReport]"
                        Exit Select
                    Case 7
                        storedProcedureName = "[TurboDB.SelectIndividualFolderCoverReport]"
                        Exit Select
                    Case 8
                        storedProcedureName = "[TurboDB.SelectIndividualReAuthorizationsDueReport]"
                        IsAuthenticationDue = True
                        Exit Select
                    Case 9
                        storedProcedureName = "[TurboDB.SelectIndividualHealthAssessmentsDueReport]"
                        IsHealthAssessmentDue = True
                        Exit Select
                    Case 10
                        storedProcedureName = "[TurboDB.SelectIndividualSupervisoryVisitsDueReport]"
                        IsSupervisorVisitDue = True
                        Exit Select
                    Case 11
                        storedProcedureName = "[TurboDB.SelectIndividualWeeklyUnitsReport]"
                        Exit Select
                    Case 12
                        storedProcedureName = "[TurboDB.SelectIndividualDoctorsClientsReport]"
                        Exit Select
                End Select

                Dim sqlCommand As SqlCommand = New SqlCommand(storedProcedureName, sqlConnection)
                sqlCommand.CommandText = storedProcedureName
                sqlCommand.CommandType = CommandType.StoredProcedure
                AddParameters(ReportId, sqlCommand, QueryStringCollection, ReportParameters)

                If (IsAuthenticationDue) Then
                    sqlCommand.Parameters.AddWithValue("@IsAuthenticationDue", IsAuthenticationDue)
                ElseIf (IsHealthAssessmentDue) Then
                    sqlCommand.Parameters.AddWithValue("@IsHealthAssessmentDue", IsHealthAssessmentDue)
                ElseIf (IsSupervisorVisitDue) Then
                    sqlCommand.Parameters.AddWithValue("@IsSupervisorVisitDue", IsSupervisorVisitDue)
                End If

                Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter(sqlCommand)
                Dim dataTable As DataTable = New DataTable("IndividualReportData")
                dataAdapter.Fill(dataTable)
                CrystalReportDataSet.Tables.Add(dataTable)

            Catch ex As Exception

            Finally

            End Try

            Return CrystalReportDataSet

        End Function

        Private Sub AddParameters(ReportId As Integer, sqlCommand As SqlCommand, QueryStringCollection As NameValueCollection, ReportParameters As OrderedDictionary)
            Dim StartDateBW As DateTime = DateTime.MinValue, StartDateBWTo As DateTime = DateTime.MinValue,
                    EndDateBW As DateTime = DateTime.MinValue, EndDateBWTo As DateTime = DateTime.MinValue
            Dim IndividualId As Integer = -1, companyId As Integer = -1, IndividualStatus As Integer = -1,
                CaseWorker As Integer = -1, Priority As Integer = -1, DischargeReason As Integer = -1,
                ClientType As Integer = -1, ClientGroup As Integer = -1, CountyId As Integer = -1,
                EmergencyDisasterCategory As Integer = -1
            Dim IndividualName As String = String.Empty, ZipCode As String = String.Empty,
                Liaison As String = String.Empty, Supervisor As String = String.Empty
            Dim IsAuthenticationDue As Boolean = False, IsHealthAssessmentDue As Boolean = False,
                IsSupervisorVisitDue As Boolean = False
            Dim AdditionalStartDate As DateTime = DateTime.MinValue, AdditionalEndDate As DateTime = DateTime.MinValue

            DateTime.TryParse(QueryStringCollection("StartDateBw"), StartDateBW)
            DateTime.TryParse(QueryStringCollection("StartDateBwTo"), StartDateBWTo)
            DateTime.TryParse(QueryStringCollection("EndDateBw"), EndDateBW)
            DateTime.TryParse(QueryStringCollection("EndDateBwTo"), EndDateBWTo)

            Integer.TryParse(QueryStringCollection("IndividualId"), IndividualId)
            IndividualName = QueryStringCollection("IndividualName")
            Integer.TryParse(QueryStringCollection("StatusId"), IndividualStatus)
            Integer.TryParse(QueryStringCollection("CaseworkerId"), CaseWorker)

            Integer.TryParse(QueryStringCollection("PriorityId"), Priority)
            Integer.TryParse(QueryStringCollection("DischargeReasonId"), DischargeReason)
            Integer.TryParse(QueryStringCollection("TypeId"), ClientType)
            Integer.TryParse(QueryStringCollection("GroupId"), ClientGroup)
            Integer.TryParse(QueryStringCollection("CountyId"), CountyId)

            ZipCode = QueryStringCollection("ZipCode")
            Integer.TryParse(QueryStringCollection("DisasterCategoryId"), EmergencyDisasterCategory)
            Liaison = QueryStringCollection("Liaison")
            Supervisor = QueryStringCollection("Supervisor")

            DateTime.TryParse(QueryStringCollection("FromDate"), AdditionalStartDate)
            DateTime.TryParse(QueryStringCollection("ToDate"), AdditionalEndDate)
            Integer.TryParse(QueryStringCollection("CompanyId"), companyId)

            sqlCommand.Parameters.AddWithValue("StartDateBW", If(StartDateBW = DateTime.MinValue, DBNull.Value, StartDateBW))
            sqlCommand.Parameters.AddWithValue("StartDateBWTo", If(StartDateBWTo = DateTime.MinValue, DBNull.Value, StartDateBWTo))
            sqlCommand.Parameters.AddWithValue("EndDateBW", If(EndDateBW = DateTime.MinValue, DBNull.Value, EndDateBW))
            sqlCommand.Parameters.AddWithValue("EndDateBWTo", If(EndDateBWTo = DateTime.MinValue, DBNull.Value, EndDateBWTo))

            sqlCommand.Parameters.AddWithValue("IndividualId", If(IndividualId < 0, DBNull.Value, IndividualId))
            sqlCommand.Parameters.AddWithValue("IndividualName", If(String.IsNullOrEmpty(IndividualName), DBNull.Value, IndividualName))
            sqlCommand.Parameters.AddWithValue("IndividualStatus", If(IndividualStatus < 0, DBNull.Value, IndividualStatus))
            sqlCommand.Parameters.AddWithValue("CaseWorker", If(CaseWorker < 0, DBNull.Value, CaseWorker))

            sqlCommand.Parameters.AddWithValue("Priority", If(Priority < 0, DBNull.Value, Priority))
            sqlCommand.Parameters.AddWithValue("DischargeReason", If(DischargeReason < 0, DBNull.Value, DischargeReason))
            sqlCommand.Parameters.AddWithValue("ClientType", If(ClientType < 0, DBNull.Value, ClientType))
            sqlCommand.Parameters.AddWithValue("ClientGroup", If(ClientGroup < 0, DBNull.Value, ClientGroup))
            sqlCommand.Parameters.AddWithValue("CountyId", If(CountyId < 0, DBNull.Value, CountyId))

            sqlCommand.Parameters.AddWithValue("ZipCode", If(String.IsNullOrEmpty(ZipCode), DBNull.Value, ZipCode))
            sqlCommand.Parameters.AddWithValue("EmergencyDisasterCategory", If(EmergencyDisasterCategory < 0, DBNull.Value, EmergencyDisasterCategory))
            sqlCommand.Parameters.AddWithValue("Liaison", If(String.IsNullOrEmpty(Liaison), DBNull.Value, Liaison))
            sqlCommand.Parameters.AddWithValue("Supervisor", If(String.IsNullOrEmpty(Supervisor), DBNull.Value, Supervisor))

            If (ReportId > 7 And ReportId < 11) Then
                sqlCommand.Parameters.AddWithValue("AdditionalStartDate", If(AdditionalStartDate = DateTime.MinValue, DBNull.Value, AdditionalStartDate))
                sqlCommand.Parameters.AddWithValue("AdditionalEndDate", If(AdditionalEndDate = DateTime.MinValue, DBNull.Value, AdditionalEndDate))
            End If
            
            sqlCommand.Parameters.AddWithValue("CompanyId", companyId)


            If (ReportId < 3 Or ReportId > 7) Then
                ReportParameters.Add("Individual", If(IndividualId > 0, IndividualName, "[ All ]"))
                ReportParameters.Add("Status", If(IndividualStatus > 0, QueryStringCollection("StatusName"), "[ All ]"))

                If (ReportId <> 12) Then
                    ReportParameters.Add("CaseWorker", If(CaseWorker > 0, QueryStringCollection("CaseworkerName"), "[ All ]"))
                End If

                ReportParameters.Add("Priority", If(Priority >= 0, QueryStringCollection("PriorityName"), "[ All ]"))
                ReportParameters.Add("DischargeReason", If(DischargeReason > 0, QueryStringCollection("DischargeReasonName"), "[ All ]"))
                ReportParameters.Add("Type", If(ClientType > 0, QueryStringCollection("TypeName"), "[ All ]"))

                If (ReportId = 1 Or ReportId = 12) Then
                    ReportParameters.Add("County", If(CountyId > 0, QueryStringCollection("CountyName"), "[ All ]"))
                End If

                ReportParameters.Add("Zip", If(Not String.IsNullOrEmpty(ZipCode), ZipCode, "[ All ]"))
                ReportParameters.Add("EmergencyDisasterCategory", If(EmergencyDisasterCategory > 0, QueryStringCollection("DisasterCategoryName"), "[ All ]"))
            End If
        End Sub
    End Class
End Namespace
