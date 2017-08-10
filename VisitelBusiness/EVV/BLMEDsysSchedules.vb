#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: MEDsys Schedules Data Fetch, Insert, Update
' Author: Anjan Kumar Paul
' Start Date: 16 Jan 2016
' End Date: 16 Jan 2016
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                16 Jan 2016     Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelDA.VisitelDA
Imports VisitelBusiness.VisitelBusiness.DataObject.EVV

Namespace VisitelBusiness
    Public Class BLMEDsysSchedules

        Public Sub InsertMEDsysScheduleInfo(ConVisitel As SqlConnection, objMEDsysScheduleDataObject As MEDsysScheduleDataObject, IsLog As Boolean)

            Dim parameters As New HybridDictionary()

            SetParameters(parameters, objMEDsysScheduleDataObject)

            Dim objSharedSettings As New SharedSettings()

            If (IsLog) Then
                objSharedSettings.ExecuteQuery("", "[TurboDB.InsertMEDsysScheduleLogInfo]", ConVisitel, parameters)
            Else
                objSharedSettings.ExecuteQuery("", "[TurboDB.InsertMEDsysScheduleInfo]", ConVisitel, parameters)
            End If

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub UpdateMEDsysScheduleInfo(ConVisitel As SqlConnection, objMEDsysScheduleDataObject As MEDsysScheduleDataObject, IsLog As Boolean)

            Dim parameters As New HybridDictionary()

            parameters.Add("@Id", objMEDsysScheduleDataObject.Id)

            SetParameters(parameters, objMEDsysScheduleDataObject)

            parameters.Add("@UpdateBy", objMEDsysScheduleDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            If (IsLog) Then
                objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateMEDsysScheduleLogInfo]", ConVisitel, parameters)
            Else
                objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateMEDsysScheduleInfo]", ConVisitel, parameters)
            End If

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Private Sub SetParameters(parameters As HybridDictionary, objMEDsysScheduleDataObject As MEDsysScheduleDataObject)
            parameters.Add("@AccountId", objMEDsysScheduleDataObject.AccountId)
            parameters.Add("@ExternalId", objMEDsysScheduleDataObject.ExternalId)
            parameters.Add("@ScheduleId", objMEDsysScheduleDataObject.ScheduleId)
            parameters.Add("@Date", objMEDsysScheduleDataObject.Date)
            parameters.Add("@ServiceCode", objMEDsysScheduleDataObject.ServiceCode)
            parameters.Add("@ActivityCode", objMEDsysScheduleDataObject.ActivityCode)
            parameters.Add("@ProgramCode", objMEDsysScheduleDataObject.ProgramCode)
            parameters.Add("@ClientExternalId", objMEDsysScheduleDataObject.ClientExternalId)
            parameters.Add("@StaffExternalId", objMEDsysScheduleDataObject.StaffExternalId)
            parameters.Add("@BillType", objMEDsysScheduleDataObject.BillType)
            parameters.Add("@PayType", objMEDsysScheduleDataObject.PayType)
            parameters.Add("@PlannedTimeIn", objMEDsysScheduleDataObject.PlannedTimeIn)
            parameters.Add("@PlannedTimeOut", objMEDsysScheduleDataObject.PlannedTimeOut)
            parameters.Add("@PlannedDuration", objMEDsysScheduleDataObject.PlannedDuration)
            parameters.Add("@OccurredTimeIn", objMEDsysScheduleDataObject.OccurredTimeIn)
            parameters.Add("@OccurredTimeOut", objMEDsysScheduleDataObject.OccurredTimeOut)
            parameters.Add("@OccurredDuration", objMEDsysScheduleDataObject.OccurredDuration)
            parameters.Add("@Status", objMEDsysScheduleDataObject.Status)
            parameters.Add("@Comments", objMEDsysScheduleDataObject.Comments)
            parameters.Add("@Action", objMEDsysScheduleDataObject.Action)
            parameters.Add("@ClientId", objMEDsysScheduleDataObject.ClientId)
            parameters.Add("@EmployeeId", objMEDsysScheduleDataObject.EmployeeId)
            parameters.Add("@SubmitToSandata", 0)
        End Sub

        Public Sub DeleteMEDsysScheduleInfo(ConVisitel As SqlConnection, Id As Int64, DeletedBy As String, IsLog As Boolean)

            Dim parameters As New HybridDictionary()

            parameters.Add("@Id", Id)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            If (IsLog) Then
                objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteMEDsysScheduleLogInfo]", ConVisitel, parameters)
            Else
                objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteMEDsysScheduleInfo]", ConVisitel, parameters)
            End If

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Function SelectMEDsysScheduleInfo(ConVisitel As SqlConnection, IsLog As Boolean,
                                                 Optional EmployeeId As Integer = Integer.MinValue) As List(Of MEDsysScheduleDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()

            If (EmployeeId > 0) Then
                parameters.Add("@EmployeeId", EmployeeId)
            End If

            Dim objSharedSettings As New SharedSettings()
            If (IsLog) Then
                objSharedSettings.GetDataReader("", "[TurboDB.SelectMEDsysSchedulesLog]", drSql, ConVisitel, parameters)
            Else
                objSharedSettings.GetDataReader("", "[TurboDB.SelectMEDsysSchedules]", drSql, ConVisitel, parameters)
            End If

            objSharedSettings = Nothing
            parameters = Nothing

            Dim MEDsysSchedules As New List(Of MEDsysScheduleDataObject)()

            If drSql.HasRows Then
                Dim objMEDsysScheduleDataObject As MEDsysScheduleDataObject
                While drSql.Read()
                    objMEDsysScheduleDataObject = New MEDsysScheduleDataObject()

                    objMEDsysScheduleDataObject.Id = If((DBNull.Value.Equals(drSql("Id"))), objMEDsysScheduleDataObject.Id, Convert.ToInt32(drSql("Id")))

                    objMEDsysScheduleDataObject.AccountId = If((DBNull.Value.Equals(drSql("AccountID"))),
                                                               objMEDsysScheduleDataObject.AccountId, Convert.ToInt32(drSql("AccountID")))

                    objMEDsysScheduleDataObject.ExternalId = Convert.ToString(drSql("ExternalID"), Nothing)

                    objMEDsysScheduleDataObject.ScheduleId = If((DBNull.Value.Equals(drSql("ScheduleID"))),
                                                               objMEDsysScheduleDataObject.ScheduleId, Convert.ToInt32(drSql("ScheduleID")))

                    objMEDsysScheduleDataObject.Date = Convert.ToString(drSql("Date"), Nothing)
                    objMEDsysScheduleDataObject.ServiceCode = Convert.ToString(drSql("ServiceCode"), Nothing)
                    objMEDsysScheduleDataObject.ActivityCode = Convert.ToString(drSql("ActivityCode"), Nothing)
                    objMEDsysScheduleDataObject.ProgramCode = Convert.ToString(drSql("ProgramCode"), Nothing)
                    objMEDsysScheduleDataObject.ClientExternalId = Convert.ToString(drSql("ClientExternalID"), Nothing)
                    objMEDsysScheduleDataObject.StaffExternalId = Convert.ToString(drSql("StaffExternalID"), Nothing)
                    objMEDsysScheduleDataObject.BillType = Convert.ToString(drSql("BillType"), Nothing)
                    objMEDsysScheduleDataObject.PayType = Convert.ToString(drSql("PayType"), Nothing)
                    objMEDsysScheduleDataObject.PlannedTimeIn = Convert.ToString(drSql("PlannedTimeIN"), Nothing)
                    objMEDsysScheduleDataObject.PlannedTimeOut = Convert.ToString(drSql("PlannedTimeOUT"), Nothing)
                    objMEDsysScheduleDataObject.PlannedDuration = Convert.ToString(drSql("PlannedDuration"), Nothing)
                    objMEDsysScheduleDataObject.OccurredTimeIn = Convert.ToString(drSql("OccurredTimeIN"), Nothing)
                    objMEDsysScheduleDataObject.OccurredTimeOut = Convert.ToString(drSql("OccurredTimeOUT"), Nothing)
                    objMEDsysScheduleDataObject.OccurredDuration = Convert.ToString(drSql("OccurredDuration"), Nothing)
                    objMEDsysScheduleDataObject.Status = Convert.ToString(drSql("Status"), Nothing)
                    objMEDsysScheduleDataObject.Comments = Convert.ToString(drSql("Comments"), Nothing)
                    objMEDsysScheduleDataObject.Action = Convert.ToString(drSql("Action"), Nothing)

                    objMEDsysScheduleDataObject.ClientId = If((DBNull.Value.Equals(drSql("Client_Id"))),
                                                               objMEDsysScheduleDataObject.ClientId, Convert.ToInt32(drSql("Client_Id")))

                    objMEDsysScheduleDataObject.ClientName = Convert.ToString(drSql("ClientName"), Nothing)

                    objMEDsysScheduleDataObject.EmployeeId = If((DBNull.Value.Equals(drSql("Emp_Id"))),
                                                               objMEDsysScheduleDataObject.EmployeeId, Convert.ToInt32(drSql("Emp_Id")))

                    objMEDsysScheduleDataObject.EmployeeName = Convert.ToString(drSql("EmployeeName"), Nothing)

                    objMEDsysScheduleDataObject.UpdateDate = Convert.ToString(drSql("update_date"), Nothing)
                    objMEDsysScheduleDataObject.UpdateBy = Convert.ToString(drSql("update_by"), Nothing)

                    MEDsysSchedules.Add(objMEDsysScheduleDataObject)

                End While
                objMEDsysScheduleDataObject = Nothing
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return MEDsysSchedules

        End Function
    End Class
End Namespace

