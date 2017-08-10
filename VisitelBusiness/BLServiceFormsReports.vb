Imports System.Data.SqlClient
Imports System.Collections.Specialized

Namespace VisitelBusiness
    Public Class BLServiceFormsReports

        Private sqlConnection As SqlConnection

        Public Function GetServiceFormsReportsData(ReportId As Integer, VisitelConnectionString As String, QueryStringCollection As NameValueCollection, ReportParameters As OrderedDictionary) As DataSet

            Dim CrystalReportDataSet As New DataSet()

            Try
                sqlConnection = New SqlConnection(VisitelConnectionString)
                Dim storedProcedureName As String = String.Empty

                Select Case (ReportId)
                    Case 1
                        storedProcedureName = "[TurboDB.SelectServiceFormsAttendantOrientationSupervisorVisitReport]"
                        Exit Select
                    Case 2
                        storedProcedureName = "[TurboDB.SelectServiceFormsAttendantOrientationReport]"
                        Exit Select
                    Case 3
                        storedProcedureName = "[TurboDB.SelectServiceFormsAttendantOrientationMdcpReport]"
                        Exit Select
                    Case 4
                        storedProcedureName = "[TurboDB.SelectServiceFormsSupervisoryVisitReport]"
                        Exit Select
                    Case 5
                        storedProcedureName = "[TurboDB.SelectServiceFormsHealthAssessmentServiceDeliveryPlanReport]"
                        Exit Select
                    Case 6
                        storedProcedureName = "[TurboDB.SelectServiceFormsServiceDeliveryPlanReport]"
                        Exit Select
                    Case 7
                        storedProcedureName = "[TurboDB.SelectServiceFormsServiceDeliveryPlanMdcpReport]"
                        Exit Select
                    Case 8
                        storedProcedureName = "[TurboDB.SelectServiceFormsIndividualEvaluationReport]"
                        Exit Select
                    Case 9
                        storedProcedureName = "[TurboDB.SelectServiceFormsAttendantsIndividualsReport]"
                        Exit Select
                    Case 10
                        storedProcedureName = "[TurboDB.SelectServiceFormsIndividualsAttendantsReport]"
                        Exit Select
                    Case 11
                        storedProcedureName = "[TurboDB.SelectServiceFormsCalendarReport]"
                        Exit Select
                End Select

                Dim sqlCommand As SqlCommand = New SqlCommand(storedProcedureName, sqlConnection)
                sqlCommand.CommandText = storedProcedureName
                sqlCommand.CommandType = CommandType.StoredProcedure
                AddParameters(ReportId, sqlCommand, QueryStringCollection, ReportParameters)

                Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter(sqlCommand)
                Dim dataTable As DataTable = New DataTable("ServiceFormsReportData")
                dataAdapter.Fill(dataTable)
                CrystalReportDataSet.Tables.Add(dataTable)

            Catch ex As Exception

            Finally

            End Try

            Return CrystalReportDataSet

        End Function

        Private Sub AddParameters(ReportId As Integer, sqlCommand As SqlCommand, QueryStringCollection As NameValueCollection, ReportParameters As OrderedDictionary)
            Dim IndividualId As Integer = -1, AttendantId As Integer = -1, companyId As Integer = -1,
                IndividualStatus As Integer = -1, Priority As Integer = -1,
                ClientType As Integer = -1, ClientGroup As Integer = -1
            Dim IndividualName As String = String.Empty, AttendantName As String = String.Empty,
                IndividualStatusName As String = String.Empty, PriorityName As String = String.Empty,
                ClientTypeName As String = String.Empty, ClientGroupName As String = String.Empty

            Integer.TryParse(QueryStringCollection("IndividualId"), IndividualId)
            IndividualName = QueryStringCollection("IndividualName")
            Integer.TryParse(QueryStringCollection("AttendantId"), AttendantId)
            AttendantName = QueryStringCollection("AttendantName")

            Integer.TryParse(QueryStringCollection("StatusId"), IndividualStatus)
            IndividualStatusName = QueryStringCollection("StatusName")
            Integer.TryParse(QueryStringCollection("PriorityId"), Priority)
            PriorityName = QueryStringCollection("PriorityName")

            Integer.TryParse(QueryStringCollection("TypeId"), ClientType)
            ClientTypeName = QueryStringCollection("TypeName")
            Integer.TryParse(QueryStringCollection("GroupId"), ClientGroup)
            ClientGroupName = QueryStringCollection("GroupName")

            Integer.TryParse(QueryStringCollection("CompanyId"), companyId)

            sqlCommand.Parameters.AddWithValue("IndividualId", If(IndividualId < 1, DBNull.Value, IndividualId))
            sqlCommand.Parameters.AddWithValue("IndividualName", If(String.IsNullOrEmpty(IndividualName), DBNull.Value, IndividualName))
            sqlCommand.Parameters.AddWithValue("AttendantId", If(AttendantId < 1, DBNull.Value, AttendantId))
            sqlCommand.Parameters.AddWithValue("AttendantName", If(String.IsNullOrEmpty(AttendantName), DBNull.Value, AttendantName))

            sqlCommand.Parameters.AddWithValue("IndividualStatus", If(IndividualStatus < 0, DBNull.Value, IndividualStatus))
            sqlCommand.Parameters.AddWithValue("Priority", If(Priority < 0, DBNull.Value, Priority))
            sqlCommand.Parameters.AddWithValue("ClientType", If(ClientType < 0, DBNull.Value, ClientType))
            sqlCommand.Parameters.AddWithValue("ClientGroup", If(ClientGroup < 0, DBNull.Value, ClientGroup))
            sqlCommand.Parameters.AddWithValue("CompanyId", companyId)

            If (ReportId > 8) Then
                ReportParameters.Add("IndividualName", If(IndividualId > 0, IndividualName, "[ All ]"))
                ReportParameters.Add("IndividualStatus", If(Not String.IsNullOrEmpty(IndividualStatusName), IndividualStatusName, "[ All ]"))
                ReportParameters.Add("AttendantName", If(AttendantId > 0, AttendantName, "[ All ]"))
                ReportParameters.Add("ClientType", If(Not String.IsNullOrEmpty(ClientTypeName), ClientTypeName, "[ All ]"))
                ReportParameters.Add("Priority", If(Not String.IsNullOrEmpty(PriorityName), PriorityName, "[ All ]"))

                If (ReportId <> 11) Then
                    ReportParameters.Add("ClientGroup", If(Not String.IsNullOrEmpty(ClientGroupName), ClientGroupName, "[ All ]"))
                End If

            End If
        End Sub
    End Class
End Namespace
