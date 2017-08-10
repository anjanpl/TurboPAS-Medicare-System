Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelDA.VisitelDA
Imports System.Web.UI.WebControls

Namespace VisitelBusiness
    Public Class BLClientComments

        Public Function SelectComments(ConVisitel As SqlConnection, comment As ClientCommentsDataObject)

            Dim parameters As New HybridDictionary()
            Dim objSharedSettings As New SharedSettings()
            Dim drSql As SqlDataReader = Nothing
            Dim comments As New List(Of ClientCommentsDataObject)()

            SetParameters(parameters, comment)
            objSharedSettings.GetDataReader("", "[TurboDB.SelectClientComments]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            If drSql.HasRows Then
                While drSql.Read()
                    comment = New ClientCommentsDataObject()
                    comment.CommentId = Convert.ToInt32(drSql("CommentId"), Nothing)
                    comment.ClientId = If((DBNull.Value.Equals(drSql("ClientId"))),
                                           comment.ClientId, Convert.ToInt32(drSql("ClientId"), Nothing))

                    comment.CompanyId = If((DBNull.Value.Equals(drSql("CompanyId"))),
                                           comment.CompanyId, Convert.ToInt32(drSql("CompanyId"), Nothing))
                    comment.UserId = If((DBNull.Value.Equals(drSql("UserId"))),
                                           comment.UserId, Convert.ToInt32(drSql("UserId"), Nothing))

                    comment.Comment = If((DBNull.Value.Equals(drSql("Comment"))),
                                              comment.Comment, Convert.ToString(drSql("Comment"), Nothing))

                    comment.CommentDate = Convert.ToString(drSql("CommentDate"), Nothing)

                    comment.CommunicationNotes = If((DBNull.Value.Equals(drSql("CommunicationNotes"))),
                                              comment.CommunicationNotes, Convert.ToBoolean(drSql("CommunicationNotes"), Nothing))
                    comment.EntryTime = If((DBNull.Value.Equals(drSql("EntryTime"))),
                                              comment.CommentDate, Convert.ToString(drSql("EntryTime"), Nothing))

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

        Public Sub InsertComment(ConVisitel As SqlConnection, comment As ClientCommentsDataObject)

            Dim parameters As New HybridDictionary()
            Dim objSharedSettings As New SharedSettings()

            SetParameters(parameters, comment)
            SetAdditionalParameters(parameters, comment)
            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertClientComment]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub UpdateComment(ConVisitel As SqlConnection, comment As ClientCommentsDataObject)

            Dim parameters As New HybridDictionary()
            Dim objSharedSettings As New SharedSettings()

            parameters.Add("@CommentId", comment.CommentId)
            SetParameters(parameters, comment)
            SetAdditionalParameters(parameters, comment)
            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateClientComment]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub DeleteComment(ConVisitel As SqlConnection, comment As ClientCommentsDataObject)

            Dim parameters As New HybridDictionary()
            Dim objSharedSettings As New SharedSettings()

            parameters.Add("@CommentId", comment.CommentId)
            SetParameters(parameters, comment)
            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteClientComment]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Private Sub SetParameters(parameters As HybridDictionary, comment As ClientCommentsDataObject)

            parameters.Add("@ClientId", comment.ClientId)
            parameters.Add("@CompanyId", comment.CompanyId)
            parameters.Add("@UserId", comment.UserId)

        End Sub

        Private Sub SetAdditionalParameters(parameters As HybridDictionary, comment As ClientCommentsDataObject)

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

            SqlDataSourceCrystalReport.SelectCommand = "[TurboDB.SelectClientCommentReport]"

            Return SqlDataSourceCrystalReport

        End Function
    End Class
End Namespace