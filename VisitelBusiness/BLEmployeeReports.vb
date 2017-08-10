Imports System.Data.SqlClient
Imports System.Collections.Specialized

Namespace VisitelBusiness
    Public Class BLEmployeeReports

        Private sqlConnection As SqlConnection

        Public Function Get12MonthEmployeeEvalReportData(VisitelConnectionString As String, QueryStringCollection As NameValueCollection) As DataSet

            Dim CrystalReportDataSet As New DataSet()

            Try
                Dim fromDate As DateTime = DateTime.MinValue, toDate As DateTime = DateTime.MinValue
                Dim companyId As Integer = 0

                DateTime.TryParse(QueryStringCollection("FromDate"), fromDate)
                DateTime.TryParse(QueryStringCollection("ToDate"), toDate)
                Integer.TryParse(QueryStringCollection("CompanyId"), companyId)

                sqlConnection = New SqlConnection(VisitelConnectionString)

                Dim sqlCommand As SqlCommand = New SqlCommand("[TurboDB.SelectEmployee12MonthEvalReport]", sqlConnection)
                sqlCommand.CommandType = CommandType.StoredProcedure
                sqlCommand.Parameters.AddWithValue("FromDate", fromDate)
                sqlCommand.Parameters.AddWithValue("ToDate", toDate)
                sqlCommand.Parameters.AddWithValue("CompanyId", companyId)

                Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter(sqlCommand)
                Dim dataTable As DataTable = New DataTable("EmployeeEval")
                dataAdapter.Fill(dataTable)
                CrystalReportDataSet.Tables.Add(dataTable)

            Catch ex As Exception

            Finally

            End Try

            Return CrystalReportDataSet

        End Function

        Public Function GetEmployeeAnnualCrimcheckReportData(VisitelConnectionString As String, QueryStringCollection As NameValueCollection) As DataSet

            Dim CrystalReportDataSet As New DataSet()

            Try
                Dim fromDate As DateTime = DateTime.MinValue, toDate As DateTime = DateTime.MinValue
                Dim companyId As Integer = 0

                DateTime.TryParse(QueryStringCollection("FromDate"), fromDate)
                DateTime.TryParse(QueryStringCollection("ToDate"), toDate)
                Integer.TryParse(QueryStringCollection("CompanyId"), companyId)

                sqlConnection = New SqlConnection(VisitelConnectionString)

                Dim sqlCommand As SqlCommand = New SqlCommand("[TurboDB.SelectEmployeeAnnualCrimcheckReport]", sqlConnection)
                sqlCommand.CommandType = CommandType.StoredProcedure
                sqlCommand.Parameters.AddWithValue("FromDate", fromDate)
                sqlCommand.Parameters.AddWithValue("ToDate", toDate)
                sqlCommand.Parameters.AddWithValue("CompanyId", companyId)

                Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter(sqlCommand)
                Dim dataTable As DataTable = New DataTable("EmployeeAnnualCrimcheck")
                dataAdapter.Fill(dataTable)
                CrystalReportDataSet.Tables.Add(dataTable)

            Catch ex As Exception

            Finally

            End Try

            Return CrystalReportDataSet

        End Function

        Public Function GetEmployeeBirthdayReportData(VisitelConnectionString As String, QueryStringCollection As NameValueCollection) As DataSet

            Dim CrystalReportDataSet As New DataSet()

            Try
                Dim fromMonth As Integer = 0, toMonth As Integer = 0, companyId As Integer = 0
                Integer.TryParse(QueryStringCollection("FromMonth"), fromMonth)
                Integer.TryParse(QueryStringCollection("ToMonth"), toMonth)
                Integer.TryParse(QueryStringCollection("CompanyId"), companyId)

                sqlConnection = New SqlConnection(VisitelConnectionString)

                Dim sqlCommand As SqlCommand = New SqlCommand("[TurboDB.SelectEmployeeBirthdayReport]", sqlConnection)
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

        Public Function GetEmployeeOigReportData(VisitelConnectionString As String, QueryStringCollection As NameValueCollection) As DataSet

            Dim CrystalReportDataSet As New DataSet()

            Try
                Dim fromDate As DateTime = DateTime.MinValue, toDate As DateTime = DateTime.MinValue
                Dim companyId As Integer = 0

                DateTime.TryParse(QueryStringCollection("FromDate"), fromDate)
                DateTime.TryParse(QueryStringCollection("ToDate"), toDate)
                Integer.TryParse(QueryStringCollection("CompanyId"), companyId)

                sqlConnection = New SqlConnection(VisitelConnectionString)

                Dim sqlCommand As SqlCommand = New SqlCommand("[TurboDB.SelectEmployeeOigReport]", sqlConnection)
                sqlCommand.CommandType = CommandType.StoredProcedure
                sqlCommand.Parameters.AddWithValue("FromDate", fromDate)
                sqlCommand.Parameters.AddWithValue("ToDate", toDate)
                sqlCommand.Parameters.AddWithValue("CompanyId", companyId)

                Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter(sqlCommand)
                Dim dataTable As DataTable = New DataTable("EmployeeOig")
                dataAdapter.Fill(dataTable)
                CrystalReportDataSet.Tables.Add(dataTable)

            Catch ex As Exception

            Finally

            End Try

            Return CrystalReportDataSet

        End Function

        Public Function GetEmployeeReportsData(ReportId As Integer, VisitelConnectionString As String, QueryStringCollection As NameValueCollection, ReportParameters As OrderedDictionary) As DataSet

            Dim CrystalReportDataSet As New DataSet()

            Try
                sqlConnection = New SqlConnection(VisitelConnectionString)
                Dim storedProcedureName As String = String.Empty

                Select Case (ReportId)
                    Case 1
                        storedProcedureName = "[TurboDB.SelectEmployeeListReport]"
                        Exit Select
                    Case 2
                        storedProcedureName = "[TurboDB.SelectEmployeeUnlicensedIndividualReport]"
                        Exit Select
                    Case 3
                        storedProcedureName = "[TurboDB.SelectEmployeeLicensedIndividualReport]"
                        Exit Select
                    Case 4
                        storedProcedureName = "[TurboDB.SelectEmployeeNewPayrollReport]"
                        Exit Select
                    Case 5
                        storedProcedureName = "[TurboDB.SelectEmployeeLabelsReport]"
                        Exit Select
                    Case 6
                        storedProcedureName = "[TurboDB.SelectEmployeeSeparationReport]"
                        Exit Select
                    Case 7
                        storedProcedureName = "[TurboDB.SelectEmployeeOigMonthlyExclusionListReport]"
                        Exit Select
                End Select

                Dim sqlCommand As SqlCommand = New SqlCommand(storedProcedureName, sqlConnection)
                sqlCommand.CommandText = storedProcedureName
                sqlCommand.CommandType = CommandType.StoredProcedure
                AddParameters(ReportId, sqlCommand, QueryStringCollection, ReportParameters)

                Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter(sqlCommand)
                Dim dataTable As DataTable = New DataTable("EmployeeReportData")
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
            Dim EmployeeId As Integer = -1, companyId As Integer = -1, TitleId As Integer = -1,
                ZipCode As Integer = -1, ClientGroup As Integer = -1
            Dim EmployeeName As String = String.Empty, EmploymentStatus As String = String.Empty,
                OigResult As String = String.Empty

            DateTime.TryParse(QueryStringCollection("StartDateBw"), StartDateBW)
            DateTime.TryParse(QueryStringCollection("StartDateBwTo"), StartDateBWTo)
            DateTime.TryParse(QueryStringCollection("EndDateBw"), EndDateBW)
            DateTime.TryParse(QueryStringCollection("EndDateBwTo"), EndDateBWTo)

            Integer.TryParse(QueryStringCollection("EmployeeId"), EmployeeId)
            EmployeeName = QueryStringCollection("EmployeeName")
            EmploymentStatus = QueryStringCollection("StatusName")
            Integer.TryParse(QueryStringCollection("TitleId"), TitleId)

            Integer.TryParse(QueryStringCollection("ZipCode"), ZipCode)
            Integer.TryParse(QueryStringCollection("GroupId"), ClientGroup)
            OigResult = QueryStringCollection("OigResult")
            Integer.TryParse(QueryStringCollection("CompanyId"), companyId)

            sqlCommand.Parameters.AddWithValue("StartDateBW", If(StartDateBW = DateTime.MinValue, DBNull.Value, StartDateBW))
            sqlCommand.Parameters.AddWithValue("StartDateBWTo", If(StartDateBWTo = DateTime.MinValue, DBNull.Value, StartDateBWTo))
            sqlCommand.Parameters.AddWithValue("EndDateBW", If(EndDateBW = DateTime.MinValue, DBNull.Value, EndDateBW))
            sqlCommand.Parameters.AddWithValue("EndDateBWTo", If(EndDateBWTo = DateTime.MinValue, DBNull.Value, EndDateBWTo))

            sqlCommand.Parameters.AddWithValue("EmployeeId", If(EmployeeId < 1, DBNull.Value, EmployeeId))
            sqlCommand.Parameters.AddWithValue("EmployeeName", If(String.IsNullOrEmpty(EmployeeName), DBNull.Value, EmployeeName))
            sqlCommand.Parameters.AddWithValue("EmploymentStatus", If(String.IsNullOrEmpty(EmploymentStatus), DBNull.Value, EmploymentStatus))
            sqlCommand.Parameters.AddWithValue("TitleId", If(TitleId < 1, DBNull.Value, TitleId))

            sqlCommand.Parameters.AddWithValue("ZipCode", If(ZipCode < 1, DBNull.Value, ZipCode))
            sqlCommand.Parameters.AddWithValue("ClientGroup", If(ClientGroup < 1, DBNull.Value, ClientGroup))
            sqlCommand.Parameters.AddWithValue("OigResult", If(String.IsNullOrEmpty(OigResult), DBNull.Value, OigResult))
            sqlCommand.Parameters.AddWithValue("CompanyId", companyId)

            If (ReportId = 1 Or ReportId = 4 Or ReportId = 7) Then
                ReportParameters.Add("EmployeeName", If(EmployeeId > 0, EmployeeName, "[ All ]"))

                'If (ReportId = 1 Or ReportId = 4) Then
                ReportParameters.Add("EmploymentStatus", If(Not String.IsNullOrEmpty(EmploymentStatus), EmploymentStatus, "[ All ]"))
                'End If


                Dim BeginDateRange As String = If(StartDateBW <> DateTime.MinValue, StartDateBW.ToString("MM/DD/YYYY"), "[ All ]")
                If (StartDateBW <> DateTime.MinValue) Then
                    BeginDateRange = BeginDateRange & " - " & If(StartDateBWTo <> DateTime.MinValue, StartDateBWTo.ToString("MM/DD/YYYY"), StartDateBW.ToString("MM/DD/YYYY"))
                End If

                ReportParameters.Add("BeginDateRange", BeginDateRange)

                'If (ReportId = 1 Or ReportId = 4) Then
                Dim EndDateRange As String = If(EndDateBW <> DateTime.MinValue, EndDateBW.ToString("MM/DD/YYYY"), "[ All ]")

                If (EndDateBW <> DateTime.MinValue) Then
                    EndDateRange = EndDateRange & " - " & If(EndDateBWTo <> DateTime.MinValue, EndDateBWTo.ToString("MM/DD/YYYY"), EndDateBW.ToString("MM/DD/YYYY"))
                End If

                ReportParameters.Add("EndDateRange", EndDateRange)
                'End If

                ReportParameters.Add("TitleName", If(TitleId > 0, QueryStringCollection("TitleName"), "[ All ]"))
                ReportParameters.Add("ZipCode", If(ZipCode > 0, ZipCode.ToString(), "[ All ]"))

                If (ReportId <> 4) Then
                    ReportParameters.Add("OigResult", If(Not String.IsNullOrEmpty(OigResult), OigResult, "[ All ]"))
                End If

            End If
        End Sub
    End Class
End Namespace
