Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelDA.VisitelDA
Imports System.Web.UI.WebControls

Namespace VisitelBusiness
    Public Class BLComplaintLog

        Public Function SelectComplaintLogs(ConVisitel As SqlConnection, complaintLog As ComplaintLogDataObject)

            Dim parameters As New HybridDictionary()
            Dim objSharedSettings As New SharedSettings()
            Dim drSql As SqlDataReader = Nothing
            Dim complaintLogs As New List(Of ComplaintLogDataObject)()

            SetParameters(parameters, complaintLog)
            objSharedSettings.GetDataReader("", "[TurboDB.SelectComplaintLogs]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            If drSql.HasRows Then
                While drSql.Read()
                    complaintLog = New ComplaintLogDataObject()
                    complaintLog.ComplaintId = Convert.ToInt32(drSql("ComplaintId"), Nothing)
                    complaintLog.ClientId = If((DBNull.Value.Equals(drSql("ClientId"))),
                                           complaintLog.ClientId, Convert.ToInt32(drSql("ClientId"), Nothing))

                    complaintLog.CompanyId = If((DBNull.Value.Equals(drSql("CompanyId"))),
                                           complaintLog.CompanyId, Convert.ToInt32(drSql("CompanyId"), Nothing))
                    complaintLog.UserId = If((DBNull.Value.Equals(drSql("UserId"))),
                                           complaintLog.UserId, Convert.ToInt32(drSql("UserId"), Nothing))

                    complaintLog.ComplainantName = If((DBNull.Value.Equals(drSql("ComplainantName"))),
                                              complaintLog.ComplainantName, Convert.ToString(drSql("ComplainantName"), Nothing))
                    complaintLog.ComplaintDate = If((DBNull.Value.Equals(drSql("ComplaintDate"))),
                                              complaintLog.ComplaintDate, Convert.ToDateTime(drSql("ComplaintDate"), Nothing))

                    complaintLog.Regarding = If((DBNull.Value.Equals(drSql("Regarding"))),
                                              complaintLog.Regarding, Convert.ToString(drSql("Regarding"), Nothing))
                    complaintLog.OthersInvolved = If((DBNull.Value.Equals(drSql("OthersInvolved"))),
                                              complaintLog.OthersInvolved, Convert.ToString(drSql("OthersInvolved"), Nothing))

                    complaintLog.ReportedBy = If((DBNull.Value.Equals(drSql("ReportedBy"))),
                                              complaintLog.ReportedBy, Convert.ToString(drSql("ReportedBy"), Nothing))
                    complaintLog.NatureOfProblems = If((DBNull.Value.Equals(drSql("NatureOfProblems"))),
                                              complaintLog.NatureOfProblems, Convert.ToString(drSql("NatureOfProblems"), Nothing))

                    complaintLog.ComplaintReceiver = If((DBNull.Value.Equals(drSql("ComplaintReceiver"))),
                                              complaintLog.ComplaintReceiver, Convert.ToString(drSql("ComplaintReceiver"), Nothing))
                    complaintLog.ActionTaken = If((DBNull.Value.Equals(drSql("ActionTaken"))),
                                              complaintLog.ActionTaken,
                                              Convert.ToString(drSql("ActionTaken"), Nothing))

                    complaintLog.ReportCompletedBy = If((DBNull.Value.Equals(drSql("ReportCompletedBy"))),
                                              complaintLog.ReportCompletedBy, Convert.ToString(drSql("ReportCompletedBy"), Nothing))
                    complaintLog.Agree = If((DBNull.Value.Equals(drSql("Agree"))),
                                              complaintLog.Agree, Convert.ToBoolean(drSql("Agree"), Nothing))

                    complaintLog.Disagree = If((DBNull.Value.Equals(drSql("Disagree"))),
                                              complaintLog.Disagree, Convert.ToBoolean(drSql("Disagree"), Nothing))
                    complaintLog.UpdateBy = If((DBNull.Value.Equals(drSql("UpdateBy"))),
                                           complaintLog.UpdateBy, Convert.ToString(drSql("UpdateBy"), Nothing))
                    complaintLog.UpdateDate = If((DBNull.Value.Equals(drSql("UpdateDate"))),
                                             complaintLog.UpdateDate, Convert.ToDateTime(drSql("UpdateDate"), Nothing))

                    complaintLogs.Add(complaintLog)
                End While
            End If

            drSql.Close()
            drSql = Nothing

            SelectComplaintLogs = complaintLogs
        End Function

        Public Sub InsertComplaintLog(ConVisitel As SqlConnection, objComplaintLog As ComplaintLogDataObject)

            Dim parameters As New HybridDictionary()
            Dim objSharedSettings As New SharedSettings()

            SetParameters(parameters, objComplaintLog)
            SetAdditionalParameters(parameters, objComplaintLog)
            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertComplaintLog]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub UpdateComplaintLog(ConVisitel As SqlConnection, objComplaintLog As ComplaintLogDataObject)

            Dim parameters As New HybridDictionary()
            Dim objSharedSettings As New SharedSettings()

            parameters.Add("@ComplaintId", objComplaintLog.ComplaintId)
            SetParameters(parameters, objComplaintLog)
            SetAdditionalParameters(parameters, objComplaintLog)
            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateComplaintLog]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub DeleteComplaintLog(ConVisitel As SqlConnection, objComplaintLog As ComplaintLogDataObject)

            Dim parameters As New HybridDictionary()
            Dim objSharedSettings As New SharedSettings()

            parameters.Add("@ComplaintId", objComplaintLog.ComplaintId)
            SetParameters(parameters, objComplaintLog)
            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteComplaintLog]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Private Sub SetParameters(parameters As HybridDictionary, objComplaintLog As ComplaintLogDataObject)

            parameters.Add("@ClientId", objComplaintLog.ClientId)
            parameters.Add("@CompanyId", objComplaintLog.CompanyId)
            parameters.Add("@UserId", objComplaintLog.UserId)

        End Sub

        Private Sub SetAdditionalParameters(parameters As HybridDictionary, objComplaintLog As ComplaintLogDataObject)

            parameters.Add("@ComplainantName", objComplaintLog.ComplainantName)
            parameters.Add("@ComplaintDate", objComplaintLog.ComplaintDate)
            parameters.Add("@Regarding", objComplaintLog.Regarding)
            parameters.Add("@OthersInvolved", objComplaintLog.OthersInvolved)

            parameters.Add("@ReportedBy", objComplaintLog.ReportedBy)
            parameters.Add("@NatureOfProblems", objComplaintLog.NatureOfProblems)
            parameters.Add("@ComplaintReceiver", objComplaintLog.ComplaintReceiver)
            parameters.Add("@ActionTaken", objComplaintLog.ActionTaken)

            parameters.Add("@ReportCompletedBy", objComplaintLog.ReportCompletedBy)
            parameters.Add("@Agree", objComplaintLog.Agree)
            parameters.Add("@Disagree", objComplaintLog.Disagree)
            parameters.Add("@UpdateBy", objComplaintLog.UpdateBy)

        End Sub

        Public Function GetComplaintLogReportData(VisitelConnectionString As String, QueryStringCollection As NameValueCollection) As SqlDataSource

            Dim SqlDataSourceCrystalReport As New SqlDataSource()

            SqlDataSourceCrystalReport.ProviderName = "System.Data.SqlClient"
            SqlDataSourceCrystalReport.ConnectionString = VisitelConnectionString

            Dim logId As Integer = 0, companyId As Integer = 0
            Integer.TryParse(QueryStringCollection("LogId"), logId)
            Integer.TryParse(QueryStringCollection("CompanyId"), companyId)

            SqlDataSourceCrystalReport.SelectParameters.Add("Id", logId)
            SqlDataSourceCrystalReport.SelectParameters.Add("CompanyId", companyId)

            SqlDataSourceCrystalReport.SelectCommand = "[TurboDB.SelectComplaintLogReport]"

            Return SqlDataSourceCrystalReport

        End Function
    End Class
End Namespace
