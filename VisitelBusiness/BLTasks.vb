Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelDA.VisitelDA
Imports System.Web.UI.WebControls

Namespace VisitelBusiness
    Public Class BLTasks
        Inherits BLCommon

        Public Sub SelectTasks(ConVisitel As SqlConnection, ByRef objTasksDataObject As TasksDataObject)

            Dim drSql As SqlDataReader = Nothing
            Dim parameters As New HybridDictionary()

            SetParameters(parameters, objTasksDataObject)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SelectTasks]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            If drSql.HasRows Then
                If drSql.Read() Then

                    objTasksDataObject.TaskId = Convert.ToInt32(drSql("TaskId"), Nothing)

                    objTasksDataObject.ClientId = If((DBNull.Value.Equals(drSql("ClientId"))),
                                           objTasksDataObject.ClientId,
                                           Convert.ToInt32(drSql("ClientId"), Nothing))

                    objTasksDataObject.CompanyId = If((DBNull.Value.Equals(drSql("CompanyId"))),
                                           objTasksDataObject.CompanyId,
                                           Convert.ToInt32(drSql("CompanyId"), Nothing))

                    objTasksDataObject.UserId = If((DBNull.Value.Equals(drSql("UserId"))),
                                           objTasksDataObject.UserId,
                                           Convert.ToInt32(drSql("UserId"), Nothing))

                    objTasksDataObject.TaskBath = If((DBNull.Value.Equals(drSql("TaskBath"))),
                                              objTasksDataObject.TaskBath,
                                              Convert.ToBoolean(drSql("TaskBath"), Nothing))

                    objTasksDataObject.TaskLaundry = If((DBNull.Value.Equals(drSql("TaskLaundry"))),
                                              objTasksDataObject.TaskLaundry,
                                              Convert.ToBoolean(drSql("TaskLaundry"), Nothing))

                    objTasksDataObject.TaskMealPrep = If((DBNull.Value.Equals(drSql("TaskMealPrep"))),
                                              objTasksDataObject.TaskMealPrep,
                                              Convert.ToBoolean(drSql("TaskMealPrep"), Nothing))

                    objTasksDataObject.TaskDress = If((DBNull.Value.Equals(drSql("TaskDress"))),
                                              objTasksDataObject.TaskDress,
                                              Convert.ToBoolean(drSql("TaskDress"), Nothing))

                    objTasksDataObject.TaskToileting = If((DBNull.Value.Equals(drSql("TaskToileting"))),
                                              objTasksDataObject.TaskToileting,
                                              Convert.ToBoolean(drSql("TaskToileting"), Nothing))

                    objTasksDataObject.TaskEscort = If((DBNull.Value.Equals(drSql("TaskEscort"))),
                                              objTasksDataObject.TaskEscort,
                                              Convert.ToBoolean(drSql("TaskEscort"), Nothing))

                    objTasksDataObject.TaskExcercise = If((DBNull.Value.Equals(drSql("TaskExcercise"))),
                                              objTasksDataObject.TaskExcercise,
                                              Convert.ToBoolean(drSql("TaskExcercise"), Nothing))

                    objTasksDataObject.TaskTranAmb = If((DBNull.Value.Equals(drSql("TaskTranAmb"))),
                                              objTasksDataObject.TaskTranAmb,
                                              Convert.ToBoolean(drSql("TaskTranAmb"), Nothing))

                    objTasksDataObject.TaskAmbulation = If((DBNull.Value.Equals(drSql("TaskAmbulation"))),
                                              objTasksDataObject.TaskAmbulation,
                                              Convert.ToBoolean(drSql("TaskAmbulation"), Nothing))

                    objTasksDataObject.TaskShopping = If((DBNull.Value.Equals(drSql("TaskShopping"))),
                                              objTasksDataObject.TaskShopping,
                                              Convert.ToBoolean(drSql("TaskShopping"), Nothing))

                    objTasksDataObject.TaskFeeding = If((DBNull.Value.Equals(drSql("TaskFeeding"))),
                                              objTasksDataObject.TaskFeeding,
                                              Convert.ToBoolean(drSql("TaskFeeding"), Nothing))

                    objTasksDataObject.TaskCleaning = If((DBNull.Value.Equals(drSql("TaskCleaning"))),
                                              objTasksDataObject.TaskCleaning,
                                              Convert.ToBoolean(drSql("TaskCleaning"), Nothing))

                    objTasksDataObject.TaskAsst = If((DBNull.Value.Equals(drSql("TaskAsst"))),
                                              objTasksDataObject.TaskAsst,
                                              Convert.ToBoolean(drSql("TaskAsst"), Nothing))

                    objTasksDataObject.TaskGrooming = If((DBNull.Value.Equals(drSql("TaskGrooming"))),
                                              objTasksDataObject.TaskGrooming,
                                              Convert.ToBoolean(drSql("TaskGrooming"), Nothing))

                    objTasksDataObject.TaskHairSkin = If((DBNull.Value.Equals(drSql("TaskHairSkin"))),
                                              objTasksDataObject.TaskHairSkin,
                                              Convert.ToBoolean(drSql("TaskHairSkin"), Nothing))

                    objTasksDataObject.TaskWalking = If((DBNull.Value.Equals(drSql("TaskWalking"))),
                                              objTasksDataObject.TaskWalking,
                                              Convert.ToBoolean(drSql("TaskWalking"), Nothing))

                    objTasksDataObject.TaskOther = If((DBNull.Value.Equals(drSql("TaskOther"))),
                                              objTasksDataObject.TaskOther,
                                              Convert.ToBoolean(drSql("TaskOther"), Nothing))

                    objTasksDataObject.TaskOtherSpec = If((DBNull.Value.Equals(drSql("TaskOtherSpec"))),
                                              objTasksDataObject.TaskOtherSpec,
                                              Convert.ToString(drSql("TaskOtherSpec"), Nothing))

                    objTasksDataObject.UpdateBy = If((DBNull.Value.Equals(drSql("UpdateBy"))),
                                           objTasksDataObject.UpdateBy,
                                           Convert.ToString(drSql("UpdateBy"), Nothing))

                    objTasksDataObject.UpdateDate = If((DBNull.Value.Equals(drSql("UpdateDate"))),
                                             objTasksDataObject.UpdateDate,
                                             Convert.ToDateTime(drSql("UpdateDate"), Nothing))

                End If
            End If

            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

        End Sub

        Public Sub InsertTasks(ConVisitel As SqlConnection, objTasksDataObject As TasksDataObject)

            Dim parameters As New HybridDictionary()

            SetParameters(parameters, objTasksDataObject)
            SetAdditionalParameters(parameters, objTasksDataObject)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertTasks]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub UpdateTasks(ConVisitel As SqlConnection, objTasksDataObject As TasksDataObject)

            Dim parameters As New HybridDictionary()

            parameters.Add("@TaskId", objTasksDataObject.TaskId)
            SetParameters(parameters, objTasksDataObject)
            SetAdditionalParameters(parameters, objTasksDataObject)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateTasks]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub DeleteTasks(ConVisitel As SqlConnection, objTasksDataObject As TasksDataObject)
            Dim parameters As New HybridDictionary()

            parameters.Add("@TaskId", objTasksDataObject.TaskId)
            SetParameters(parameters, objTasksDataObject)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteTasks]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Private Sub SetParameters(parameters As HybridDictionary, objTasksDataObject As TasksDataObject)

            parameters.Add("@ClientId", objTasksDataObject.ClientId)
            parameters.Add("@CompanyId", objTasksDataObject.CompanyId)
            parameters.Add("@UserId", objTasksDataObject.UserId)

        End Sub

        Private Sub SetAdditionalParameters(parameters As HybridDictionary, objTasksDataObject As TasksDataObject)

            parameters.Add("@TaskBath", objTasksDataObject.TaskBath)
            parameters.Add("@TaskDress", objTasksDataObject.TaskDress)
            parameters.Add("@TaskExcercise", objTasksDataObject.TaskExcercise)
            parameters.Add("@TaskFeeding", objTasksDataObject.TaskFeeding)

            parameters.Add("@TaskGrooming", objTasksDataObject.TaskGrooming)
            parameters.Add("@TaskLaundry", objTasksDataObject.TaskLaundry)
            parameters.Add("@TaskToileting", objTasksDataObject.TaskToileting)
            parameters.Add("@TaskTranAmb", objTasksDataObject.TaskTranAmb)

            parameters.Add("@TaskAmbulation", objTasksDataObject.TaskAmbulation)
            parameters.Add("@TaskCleaning", objTasksDataObject.TaskCleaning)
            parameters.Add("@TaskHairSkin", objTasksDataObject.TaskHairSkin)
            parameters.Add("@TaskMealPrep", objTasksDataObject.TaskMealPrep)

            parameters.Add("@TaskEscort", objTasksDataObject.TaskEscort)
            parameters.Add("@TaskShopping", objTasksDataObject.TaskShopping)
            parameters.Add("@TaskAsst", objTasksDataObject.TaskAsst)
            parameters.Add("@TaskOther", objTasksDataObject.TaskOther)

            parameters.Add("@TaskOtherSpec", objTasksDataObject.TaskOtherSpec)
            parameters.Add("@TaskWalking", objTasksDataObject.TaskWalking)
            parameters.Add("@UpdateBy", objTasksDataObject.UpdateBy)

        End Sub

        Public Function GetCoordinationOfCareReportData(VisitelConnectionString As String, QueryStringCollection As NameValueCollection) As SqlDataSource

            Dim SqlDataSourceCrystalReport As New SqlDataSource()

            SqlDataSourceCrystalReport.ProviderName = "System.Data.SqlClient"
            SqlDataSourceCrystalReport.ConnectionString = VisitelConnectionString

            Dim clientId As Integer = 0, companyId As Integer = 0
            Integer.TryParse(QueryStringCollection("ClientId"), clientId)
            Integer.TryParse(QueryStringCollection("CompanyId"), companyId)

            SqlDataSourceCrystalReport.SelectParameters.Add("Id", clientId)
            SqlDataSourceCrystalReport.SelectParameters.Add("CompanyId", companyId)

            SqlDataSourceCrystalReport.SelectCommand = "[TurboDB.SelectCoordinationOfCareReport]"

            Return SqlDataSourceCrystalReport

        End Function
    End Class
End Namespace
