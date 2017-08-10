Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelDA.VisitelDA
Imports System.Web.UI.WebControls

Namespace VisitelBusiness
    Public Class BLEmployeeComments

        Public Function SelectComments(ConVisitel As SqlConnection, comment As EmployeeCommentsDataObject)

            Dim parameters As New HybridDictionary()
            Dim objSharedSettings As New SharedSettings()
            Dim drSql As SqlDataReader = Nothing
            Dim comments As New List(Of EmployeeCommentsDataObject)()

            SetParameters(parameters, comment)
            objSharedSettings.GetDataReader("", "[TurboDB.SelectEmployeeComments]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            If drSql.HasRows Then
                While drSql.Read()
                    comment = New EmployeeCommentsDataObject()
                    comment.CommentId = Convert.ToInt32(drSql("CommentId"), Nothing)
                    comment.EmployeeId = If((DBNull.Value.Equals(drSql("EmployeeId"))),
                                           comment.EmployeeId, Convert.ToInt32(drSql("EmployeeId"), Nothing))

                    comment.CompanyId = If((DBNull.Value.Equals(drSql("CompanyId"))),
                                           comment.CompanyId, Convert.ToInt32(drSql("CompanyId"), Nothing))
                    comment.UserId = If((DBNull.Value.Equals(drSql("UserId"))),
                                           comment.UserId, Convert.ToInt32(drSql("UserId"), Nothing))

                    comment.Comment = If((DBNull.Value.Equals(drSql("Comment"))),
                                              comment.Comment, Convert.ToString(drSql("Comment"), Nothing))

                    comment.CommentDate = Convert.ToString(drSql("CommentDate"), Nothing)

                    comment.CommunicationNotes = If((DBNull.Value.Equals(drSql("CommunicationNotes"))),
                                              comment.CommunicationNotes, Convert.ToBoolean(drSql("CommunicationNotes"), Nothing))
                    comment.EntryTime = Convert.ToString(drSql("EntryTime"), Nothing)

                    comment.UpdateBy = If((DBNull.Value.Equals(drSql("UpdateBy"))),
                                           comment.UpdateBy, Convert.ToString(drSql("UpdateBy"), Nothing))

                    comment.UpdateDate = Convert.ToString(drSql("UpdateDate"), Nothing)

                    comments.Add(comment)
                End While
            End If

            drSql.Close()
            drSql = Nothing

            SelectComments = comments
        End Function

        Public Sub InsertComment(ConVisitel As SqlConnection, comment As EmployeeCommentsDataObject)

            Dim parameters As New HybridDictionary()
            Dim objSharedSettings As New SharedSettings()

            SetParameters(parameters, comment)
            SetAdditionalParameters(parameters, comment)
            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertEmployeeComment]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub UpdateComment(ConVisitel As SqlConnection, comment As EmployeeCommentsDataObject)

            Dim parameters As New HybridDictionary()
            Dim objSharedSettings As New SharedSettings()

            parameters.Add("@CommentId", comment.CommentId)
            SetParameters(parameters, comment)
            SetAdditionalParameters(parameters, comment)
            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateEmployeeComment]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub DeleteComment(ConVisitel As SqlConnection, comment As EmployeeCommentsDataObject)

            Dim parameters As New HybridDictionary()
            Dim objSharedSettings As New SharedSettings()

            parameters.Add("@CommentId", comment.CommentId)
            SetParameters(parameters, comment)
            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteEmployeeComment]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Private Sub SetParameters(parameters As HybridDictionary, comment As EmployeeCommentsDataObject)

            parameters.Add("@EmployeeId", comment.EmployeeId)
            parameters.Add("@CompanyId", comment.CompanyId)
            parameters.Add("@UserId", comment.UserId)

        End Sub

        Private Sub SetAdditionalParameters(parameters As HybridDictionary, comment As EmployeeCommentsDataObject)

            parameters.Add("@Comment", comment.Comment)
            parameters.Add("@CommentDate", comment.CommentDate)
            parameters.Add("@CommunicationNotes", comment.CommunicationNotes)
            parameters.Add("@UpdateBy", comment.UpdateBy)

        End Sub

        Public Function GetCommentReportData(VisitelConnectionString As String, QueryStringCollection As NameValueCollection) As SqlDataSource

            Dim SqlDataSourceCrystalReport As New SqlDataSource()

            SqlDataSourceCrystalReport.ProviderName = "System.Data.SqlClient"
            SqlDataSourceCrystalReport.ConnectionString = VisitelConnectionString

            Dim commentId As Integer = 0, companyId As Integer = 0
            Integer.TryParse(QueryStringCollection("CommentId"), commentId)
            Integer.TryParse(QueryStringCollection("CompanyId"), companyId)

            SqlDataSourceCrystalReport.SelectParameters.Add("Id", commentId)
            SqlDataSourceCrystalReport.SelectParameters.Add("CompanyId", companyId)

            SqlDataSourceCrystalReport.SelectCommand = "[TurboDB.SelectEmployeeCommentReport]"

            Return SqlDataSourceCrystalReport

        End Function
    End Class
End Namespace
