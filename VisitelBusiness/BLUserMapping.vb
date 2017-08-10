Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelDA.VisitelDA
Imports System.Web.UI.WebControls
Imports VisitelBusiness.VisitelBusiness.DataObject.EVV

Namespace VisitelBusiness
    Public Class BLUserMapping
        Inherits BLCommon

        Public Function SelectUserMapping(ConVisitel As SqlConnection) As List(Of UserMappingDataObject)

            Dim drSql As SqlDataReader = Nothing
            Dim parameters As New HybridDictionary()

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SelectUserMapping]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim UserMappings As New List(Of UserMappingDataObject)
            Dim objUserMappingDataObject As UserMappingDataObject

            If drSql.HasRows Then
                While drSql.Read()
                    objUserMappingDataObject = New UserMappingDataObject()

                    objUserMappingDataObject.UserId = If((DBNull.Value.Equals(drSql("UserId"))),
                                                                 objUserMappingDataObject.UserId, Convert.ToInt32(drSql("UserId")))

                    objUserMappingDataObject.UserName = Convert.ToString(drSql("UserName"), Nothing)
                    objUserMappingDataObject.Password = Convert.ToString(drSql("Password"), Nothing)
                    objUserMappingDataObject.Email = Convert.ToString(drSql("Email"), Nothing)
                    objUserMappingDataObject.UserType = Convert.ToString(drSql("UserType"), Nothing)

                    objUserMappingDataObject.UserTypeId = If((DBNull.Value.Equals(drSql("UserTypeId"))),
                                                                 objUserMappingDataObject.UserTypeId, Convert.ToInt32(drSql("UserTypeId")))

                    objUserMappingDataObject.TurboPASUserName = Convert.ToString(drSql("TurboPASUserName"), Nothing)

                    objUserMappingDataObject.UpdateBy = Convert.ToString(drSql("update_by"), Nothing)
                    objUserMappingDataObject.UpdateDate = Convert.ToString(drSql("update_date"), Nothing)

                    UserMappings.Add(objUserMappingDataObject)
                End While
                objUserMappingDataObject = Nothing
            End If

            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return UserMappings

        End Function

        Public Sub InsertUserMapping(ConVisitel As SqlConnection, ByRef objUserMappingDataObject As UserMappingDataObject)

            Dim parameters As New HybridDictionary()

            SetParameters(parameters, objUserMappingDataObject)
            parameters.Add("@UpdateBy", objUserMappingDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertUserMapping]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub UpdateUserMapping(ConVisitel As SqlConnection, ByRef objUserMappingDataObject As UserMappingDataObject)

            Dim parameters As New HybridDictionary()

            parameters.Add("@UserId", objUserMappingDataObject.UserId)

            SetParameters(parameters, objUserMappingDataObject)

            parameters.Add("@UpdateBy", objUserMappingDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateUserMapping]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub DeleteUserMapping(ConVisitel As SqlConnection, UserId As Int64, DeletedBy As String)

            Dim parameters As New HybridDictionary()

            parameters.Add("@UserId", UserId)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteUserMapping]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Private Sub SetParameters(parameters As HybridDictionary, ByRef objUserMappingDataObject As UserMappingDataObject)
            parameters.Add("@UserName", objUserMappingDataObject.UserName)
            parameters.Add("@Password", objUserMappingDataObject.Password)
            parameters.Add("@Email", objUserMappingDataObject.Email)
            parameters.Add("@UserTypeId", objUserMappingDataObject.UserTypeId)
            parameters.Add("@TurboPASUserName", objUserMappingDataObject.TurboPASUserName)
        End Sub
    End Class
End Namespace
