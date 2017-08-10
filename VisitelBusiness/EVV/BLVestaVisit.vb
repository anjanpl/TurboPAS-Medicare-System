#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Vesta Visit Data Fetch, Insert, Update
' Author: Anjan Kumar Paul
' Start Date: 26 Dec 2015
' End Date: 26 Dec 2015
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                26 Dec 2015     Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelDA.VisitelDA
Imports VisitelBusiness.VisitelBusiness.DataObject.EVV

Namespace VisitelBusiness
    Public Class BLVestaVisit

        Public Sub InsertVisitInfo(ConVisitel As SqlConnection, objVisitDataObject As VisitDataObject, IsLog As Boolean)

            Dim parameters As New HybridDictionary()

            SetParameters(parameters, objVisitDataObject)

            Dim objSharedSettings As New SharedSettings()

            If (IsLog) Then
                objSharedSettings.ExecuteQuery("", "[TurboDB.InsertVestaVisitLogInfo]", ConVisitel, parameters)
            Else
                objSharedSettings.ExecuteQuery("", "[TurboDB.InsertVestaVisitInfo]", ConVisitel, parameters)
            End If

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub UpdateVisitInfo(ConVisitel As SqlConnection, objVisitDataObject As VisitDataObject, IsLog As Boolean)

            Dim parameters As New HybridDictionary()

            parameters.Add("@Id", objVisitDataObject.Id)

            SetParameters(parameters, objVisitDataObject)

            parameters.Add("@UpdateBy", objVisitDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            If (IsLog) Then
                objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateVestaVisitLogInfo]", ConVisitel, parameters)
            Else
                objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateVestaVisitInfo]", ConVisitel, parameters)
            End If

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Private Sub SetParameters(parameters As HybridDictionary, objVisitDataObject As VisitDataObject)

            parameters.Add("@MyUniqueId", objVisitDataObject.MyUniqueId)

            If (Not String.IsNullOrEmpty(objVisitDataObject.VestaVisitId)) Then
                parameters.Add("@VestaVisitId", objVisitDataObject.VestaVisitId)
            End If

            If (Not String.IsNullOrEmpty(objVisitDataObject.ClientIdVesta)) Then
                parameters.Add("@ClientIdVesta", objVisitDataObject.ClientIdVesta)
            End If

            If (Not String.IsNullOrEmpty(objVisitDataObject.EmployeeIdVesta)) Then
                parameters.Add("@EmployeeIdVesta", objVisitDataObject.EmployeeIdVesta)
            End If

            If (Not String.IsNullOrEmpty(objVisitDataObject.AuthIdVesta)) Then
                parameters.Add("@AuthIdVesta", objVisitDataObject.AuthIdVesta)
            End If

            If (Not String.IsNullOrEmpty(objVisitDataObject.VisitDate)) Then
                parameters.Add("@VisitDate", objVisitDataObject.VisitDate)
            End If

            If (Not String.IsNullOrEmpty(objVisitDataObject.ScheduledTimeIn)) Then
                parameters.Add("@SchedTimeIn", objVisitDataObject.ScheduledTimeIn)
            End If

            If (Not String.IsNullOrEmpty(objVisitDataObject.ScheduledTimeOut)) Then
                parameters.Add("@SchedTimeOut", objVisitDataObject.ScheduledTimeOut)
            End If

            If (Not String.IsNullOrEmpty(objVisitDataObject.VisitUnits)) Then
                parameters.Add("@VisitUnits", objVisitDataObject.VisitUnits)
            End If

            If (Not String.IsNullOrEmpty(objVisitDataObject.VisitLocation)) Then
                parameters.Add("@VisitLocation", objVisitDataObject.VisitLocation)
            End If

            parameters.Add("@DADSServiceGroup", objVisitDataObject.DADSServiceGroup)
            parameters.Add("@HCPCSBillCode", objVisitDataObject.HCPCSBillCode)
            parameters.Add("@DADSServiceCode", objVisitDataObject.DADSServiceCode)

            If (Not String.IsNullOrEmpty(objVisitDataObject.Mod1)) Then
                parameters.Add("@ModOne", objVisitDataObject.Mod1)
            End If

            If (Not String.IsNullOrEmpty(objVisitDataObject.Mod2)) Then
                parameters.Add("@ModTwo", objVisitDataObject.Mod2)
            End If

            If (Not String.IsNullOrEmpty(objVisitDataObject.Mod3)) Then
                parameters.Add("@ModThree", objVisitDataObject.Mod3)
            End If

            If (Not String.IsNullOrEmpty(objVisitDataObject.Mod4)) Then
                parameters.Add("@ModFour", objVisitDataObject.Mod4)
            End If

            If (Not String.IsNullOrEmpty(objVisitDataObject.Contract)) Then
                parameters.Add("@Contract", objVisitDataObject.Contract)
            End If

            If (objVisitDataObject.CalendarId > 0) Then
                parameters.Add("@CalendarId", objVisitDataObject.CalendarId)
            End If

            If (objVisitDataObject.ClientId > 0) Then
                parameters.Add("@ClientId", objVisitDataObject.ClientId)
            End If

            If (objVisitDataObject.EmployeeId > 0) Then
                parameters.Add("@EmployeeId", objVisitDataObject.EmployeeId)
            End If

            parameters.Add("@SubmitToSandata", 0)

            'If (objVisitDataObject.ClientInfoId > 0) Then
            '    parameters.Add("@ClientInfoId", objVisitDataObject.ClientInfoId)
            'End If

            If (Not String.IsNullOrEmpty(objVisitDataObject.Error)) Then
                parameters.Add("@Error", objVisitDataObject.Error)
            End If
           
        End Sub

        Public Sub DeleteVisitInfo(ConVisitel As SqlConnection, Id As Int64, DeletedBy As String, IsLog As Boolean)

            Dim parameters As New HybridDictionary()

            parameters.Add("@Id", Id)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            If (IsLog) Then
                objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteVestaVisitLogInfo]", ConVisitel, parameters)
            Else
                objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteVestaVisitInfo]", ConVisitel, parameters)
            End If


            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Function SelectVisitInfo(ConVisitel As SqlConnection, IsLog As Boolean, Optional EmployeeId As Integer = Integer.MinValue) As List(Of VisitDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()

            If (EmployeeId > 0) Then
                parameters.Add("@EmployeeId", EmployeeId)
            End If

            Dim objSharedSettings As New SharedSettings()
            If (IsLog) Then
                objSharedSettings.GetDataReader("", "[TurboDB.SelectVestaVisitsLog]", drSql, ConVisitel, parameters)
            Else
                objSharedSettings.GetDataReader("", "[TurboDB.SelectVestaVisits]", drSql, ConVisitel, parameters)
            End If

            objSharedSettings = Nothing
            parameters = Nothing

            Dim Visits As New List(Of VisitDataObject)()

            If drSql.HasRows Then
                Dim objVisitDataObject As VisitDataObject
                While drSql.Read()
                    objVisitDataObject = New VisitDataObject()

                    objVisitDataObject.Id = If((DBNull.Value.Equals(drSql("ID"))), objVisitDataObject.Id, Convert.ToInt32(drSql("ID")))

                    objVisitDataObject.MyUniqueId = Convert.ToString(drSql("MyUniqueID"), Nothing)
                    objVisitDataObject.VestaVisitId = Convert.ToString(drSql("Vesta_Visit_ID"), Nothing)
                    objVisitDataObject.ClientIdVesta = Convert.ToString(drSql("Client_ID_Vesta"), Nothing)
                    objVisitDataObject.EmployeeIdVesta = Convert.ToString(drSql("Employee_ID_Vesta"), Nothing)
                    objVisitDataObject.AuthIdVesta = Convert.ToString(drSql("Auth_ID_Vesta"), Nothing)
                    objVisitDataObject.VisitDate = Convert.ToString(drSql("Visit_Date"), Nothing)
                    objVisitDataObject.ScheduledTimeIn = Convert.ToString(drSql("Sched_TimeIn"), Nothing)
                    objVisitDataObject.ScheduledTimeOut = Convert.ToString(drSql("Sched_TimeOut"), Nothing)
                    objVisitDataObject.VisitUnits = Convert.ToString(drSql("Visit_Units"), Nothing)
                    objVisitDataObject.VisitLocation = Convert.ToString(drSql("Visit_Location"), Nothing)
                    objVisitDataObject.DADSServiceGroup = Convert.ToString(drSql("DADS_Service_Group"), Nothing)
                    objVisitDataObject.HCPCSBillCode = Convert.ToString(drSql("HCPCS_BillCode"), Nothing)
                    objVisitDataObject.DADSServiceCode = Convert.ToString(drSql("DADS_Service_Code"), Nothing)
                    objVisitDataObject.Mod1 = Convert.ToString(drSql("Mod1"), Nothing)
                    objVisitDataObject.Mod2 = Convert.ToString(drSql("Mod2"), Nothing)
                    objVisitDataObject.Mod3 = Convert.ToString(drSql("Mod3"), Nothing)
                    objVisitDataObject.Mod4 = Convert.ToString(drSql("Mod4"), Nothing)

                    objVisitDataObject.[Error] = Convert.ToString(drSql("Error"), Nothing)

                    objVisitDataObject.Contract = Convert.ToString(drSql("contract"), Nothing)

                    objVisitDataObject.CalendarId = If((DBNull.Value.Equals(drSql("cal_id"))), objVisitDataObject.CalendarId, Convert.ToInt64(drSql("cal_id"), Nothing))
                    objVisitDataObject.ClientId = If((DBNull.Value.Equals(drSql("client_id"))), objVisitDataObject.ClientId, Convert.ToInt64(drSql("client_id"), Nothing))
                    objVisitDataObject.ClientName = Convert.ToString(drSql("ClientName"), Nothing)
                    objVisitDataObject.EmployeeId = If((DBNull.Value.Equals(drSql("emp_id"))), objVisitDataObject.EmployeeId, Convert.ToInt64(drSql("emp_id"), Nothing))
                    objVisitDataObject.EmployeeName = Convert.ToString(drSql("EmployeeName"), Nothing)

                    objVisitDataObject.UpdateDate = Convert.ToString(drSql("update_date"), Nothing)
                    objVisitDataObject.UpdateBy = Convert.ToString(drSql("update_by"), Nothing)

                    Visits.Add(objVisitDataObject)

                End While
                objVisitDataObject = Nothing
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return Visits

        End Function
    End Class
End Namespace

