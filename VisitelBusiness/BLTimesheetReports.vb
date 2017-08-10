Imports System.Data.SqlClient
Imports System.Collections.Specialized

Namespace VisitelBusiness
    Public Class BLTimesheetReports

        Private sqlConnection As SqlConnection

        Public Function GetServiceFormsReportsData(ReportId As Integer, VisitelConnectionString As String, QueryStringCollection As NameValueCollection, ReportParameters As OrderedDictionary) As DataSet

            Dim CrystalReportDataSet As New DataSet()
            Dim TableNames As StringCollection = New StringCollection()
            TableNames.Add("TimesheetReportData")

            Try
                sqlConnection = New SqlConnection(VisitelConnectionString)
                Dim storedProcedureName As String = String.Empty

                Select Case (ReportId)
                    Case 1
                        storedProcedureName = "[TurboDB.SelectTsRegularSignReqReport]"
                        TableNames.Add("FirstHalfTimeInOutData")
                        TableNames.Add("SecondHalfTimeInOutData")
                        Exit Select
                    Case 2
                        storedProcedureName = "[TurboDB.SelectTsRegularSignOptReport]"
                        TableNames.Add("FirstHalfTimeInOutData")
                        TableNames.Add("SecondHalfTimeInOutData")
                        Exit Select
                    Case 3
                        storedProcedureName = "[TurboDB.SelectTsSampleSignReqReport]"
                        TableNames.Add("FirstHalfTimeInOutData")
                        Exit Select
                    Case 4
                        storedProcedureName = "[TurboDB.SelectTsSampleSignOptReport]"
                        TableNames.Add("FirstHalfTimeInOutData")
                        Exit Select
                    Case 5
                        storedProcedureName = "[TurboDB.SelectTsPrePrintedSignReqReport]"
                        TableNames.Add("FirstHalfTimeInOutData")
                        Exit Select
                    Case 6
                        storedProcedureName = "[TurboDB.SelectTsPrePrintedSignOptReport]"
                        TableNames.Add("FirstHalfTimeInOutData")
                        Exit Select
                    Case 7
                        storedProcedureName = "[TurboDB.SelectTsDailyScheduleSignReqReport]"
                        TableNames.Add("FirstHalfTimeInOutData")
                        TableNames.Add("SecondHalfTimeInOutData")
                        Exit Select
                    Case 8
                        storedProcedureName = "[TurboDB.SelectTsDailyScheduleSignOptReport]"
                        TableNames.Add("FirstHalfTimeInOutData")
                        TableNames.Add("SecondHalfTimeInOutData")
                        Exit Select
                    Case 9
                        storedProcedureName = "[TurboDB.SelectTsMdcpSignReqReport]"
                        TableNames.Add("FirstTimeInOutData")
                        TableNames.Add("SecondTimeInOutData")
                        TableNames.Add("ThirdTimeInOutData")
                        Exit Select
                    Case 10
                        storedProcedureName = "[TurboDB.SelectTsMdcpSignOptReport]"
                        TableNames.Add("FirstTimeInOutData")
                        TableNames.Add("SecondTimeInOutData")
                        TableNames.Add("ThirdTimeInOutData")
                        Exit Select
                    Case 11
                        storedProcedureName = "[TurboDB.SelectTsDailyTaskSheetSignReqReport]"
                        Exit Select
                    Case 12
                        storedProcedureName = "[TurboDB.SelectTsCbaSignReqReport]"
                        Exit Select
                    Case 13
                        storedProcedureName = "[TurboDB.SelectTsCbaSignOptReport]"
                        Exit Select
                End Select

                Dim sqlCommand As SqlCommand = New SqlCommand(storedProcedureName, sqlConnection)
                sqlCommand.CommandText = storedProcedureName
                sqlCommand.CommandType = CommandType.StoredProcedure
                AddParameters(ReportId, sqlCommand, QueryStringCollection, ReportParameters)

                Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter(sqlCommand)
                Dim dataSet As DataSet = New DataSet()
                dataAdapter.Fill(dataSet)

                For index = 0 To TableNames.Count - 1
                    dataSet.Tables(index).TableName = TableNames(index)
                Next

                CrystalReportDataSet = dataSet

            Catch ex As Exception

            Finally

            End Try

            Return CrystalReportDataSet

        End Function

        Private Sub AddParameters(ReportId As Integer, sqlCommand As SqlCommand, QueryStringCollection As NameValueCollection, ReportParameters As OrderedDictionary)
            Dim IndividualId As Integer = -1, AttendantId As Integer = -1, companyId As Integer = -1,
                IndividualStatus As Integer = -1, Priority As Integer = -1, ClientType As Integer = -1,
                SelectedMonth As Integer = -1, SelectedYear As Integer = -1
            Dim IndividualName As String = String.Empty, AttendantName As String = String.Empty,
                IndividualStatusName As String = String.Empty, PriorityName As String = String.Empty,
                ClientTypeName As String = String.Empty, Liaison As String = String.Empty,
                Supervisor As String = String.Empty
            Dim IsGeneric As Boolean = False, IsSignRequired As Boolean = False

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
            Liaison = QueryStringCollection("Liaison")
            Supervisor = QueryStringCollection("Supervisor")

            Integer.TryParse(QueryStringCollection("SelectedMonth"), SelectedMonth)
            Integer.TryParse(QueryStringCollection("SelectedYear"), SelectedYear)
            Boolean.TryParse(QueryStringCollection("IsGeneric"), IsGeneric)
            Integer.TryParse(QueryStringCollection("CompanyId"), companyId)

            sqlCommand.Parameters.AddWithValue("IndividualId", If(IndividualId < 1, DBNull.Value, IndividualId))
            sqlCommand.Parameters.AddWithValue("IndividualName", If(String.IsNullOrEmpty(IndividualName), DBNull.Value, IndividualName))
            sqlCommand.Parameters.AddWithValue("AttendantId", If(AttendantId < 1, DBNull.Value, AttendantId))
            sqlCommand.Parameters.AddWithValue("AttendantName", If(String.IsNullOrEmpty(AttendantName), DBNull.Value, AttendantName))

            sqlCommand.Parameters.AddWithValue("IndividualStatus", If(IndividualStatus < 0, DBNull.Value, IndividualStatus))
            sqlCommand.Parameters.AddWithValue("Priority", If(Priority < 0, DBNull.Value, Priority))
            sqlCommand.Parameters.AddWithValue("ClientType", If(ClientType < 0, DBNull.Value, ClientType))
            sqlCommand.Parameters.AddWithValue("Liaison", If(String.IsNullOrEmpty(Liaison), DBNull.Value, Liaison))

            sqlCommand.Parameters.AddWithValue("Supervisor", If(String.IsNullOrEmpty(Supervisor), DBNull.Value, Supervisor))
            sqlCommand.Parameters.AddWithValue("Month", SelectedMonth)
            sqlCommand.Parameters.AddWithValue("Year", SelectedYear)
            sqlCommand.Parameters.AddWithValue("IsGeneric", IsGeneric)
            sqlCommand.Parameters.AddWithValue("CompanyId", companyId)

            Boolean.TryParse(QueryStringCollection("IsSignRequired"), IsSignRequired)
            ReportParameters.Add("IsSignRequired", IsSignRequired)

            If (ReportId < 14) Then
                Dim MonthAndYearService As String = String.Empty

                If ((ReportId < 3 And Not IsGeneric) Or (ReportId > 11 And Not IsGeneric)) Then
                    MonthAndYearService = SelectedMonth.ToString() + "/" + SelectedYear.ToString()
                ElseIf (ReportId > 11 And IsGeneric) Then
                    MonthAndYearService = String.Empty
                Else
                    MonthAndYearService = SelectedMonth.ToString() + "/01/" + SelectedYear.ToString() + " - " + SelectedMonth.ToString() + "/15/" + SelectedYear.ToString()
                End If

                ReportParameters.Add("MonthYearService", MonthAndYearService)
            End If
            'If (ReportId > 8) Then
            '    ReportParameters.Add("IndividualName", If(IndividualId > 0, IndividualName, "[ All ]"))
            '    ReportParameters.Add("IndividualStatus", If(Not String.IsNullOrEmpty(IndividualStatusName), IndividualStatusName, "[ All ]"))
            '    ReportParameters.Add("AttendantName", If(AttendantId > 0, AttendantName, "[ All ]"))
            '    ReportParameters.Add("ClientType", If(Not String.IsNullOrEmpty(ClientTypeName), ClientTypeName, "[ All ]"))
            '    ReportParameters.Add("Priority", If(Not String.IsNullOrEmpty(PriorityName), PriorityName, "[ All ]"))

            '    If (ReportId <> 11) Then
            '        ReportParameters.Add("ClientGroup", If(Not String.IsNullOrEmpty(ClientGroupName), ClientGroupName, "[ All ]"))
            '    End If

            'End If
        End Sub
    End Class
End Namespace
