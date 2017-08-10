#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: MEDsys Staff Data Fetch, Insert, Update
' Author: Anjan Kumar Paul
' Start Date: 13 Jan 2016
' End Date: 13 Jan 2016
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                13 Jan 2016     Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelDA.VisitelDA
Imports VisitelBusiness.VisitelBusiness.DataObject.EVV

Namespace VisitelBusiness
    Public Class BLMEDsysStaff

        Public Sub InsertMEDsysStaffInfo(ConVisitel As SqlConnection, objMEDsysStaffDataObject As MEDsysStaffDataObject)

            Dim parameters As New HybridDictionary()

            SetParameters(parameters, objMEDsysStaffDataObject)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertMEDsysStaffsInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub UpdateMEDsysStaffInfo(ConVisitel As SqlConnection, objMEDsysStaffDataObject As MEDsysStaffDataObject)

            Dim parameters As New HybridDictionary()

            parameters.Add("@Id", objMEDsysStaffDataObject.Id)

            SetParameters(parameters, objMEDsysStaffDataObject)

            parameters.Add("@UpdateBy", objMEDsysStaffDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateMEDsysStaffsInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub ResetMEDsysPositionCode(ConVisitel As SqlConnection, PositionCode As String)

            Dim parameters As New HybridDictionary()

            parameters.Add("@PositionCode", PositionCode)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateMEDsysPositionCodeInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Private Sub SetParameters(parameters As HybridDictionary, objMEDsysStaffDataObject As MEDsysStaffDataObject)
            parameters.Add("@AccountId", objMEDsysStaffDataObject.AccountId)
            parameters.Add("@ExternalId", objMEDsysStaffDataObject.ExternalId)
            parameters.Add("@StaffId", objMEDsysStaffDataObject.StaffId)
            parameters.Add("@StaffNo", objMEDsysStaffDataObject.StaffNumber)
            parameters.Add("@FirstName", objMEDsysStaffDataObject.FirstName)
            parameters.Add("@MiddleInit", objMEDsysStaffDataObject.MiddleInit)
            parameters.Add("@LastName", objMEDsysStaffDataObject.LastName)
            parameters.Add("@Birthdate", objMEDsysStaffDataObject.Birthdate)
            parameters.Add("@Gender", objMEDsysStaffDataObject.Gender)
            parameters.Add("@GIN", objMEDsysStaffDataObject.GIN)
            parameters.Add("@Address", objMEDsysStaffDataObject.Address)
            parameters.Add("@City", objMEDsysStaffDataObject.City)
            parameters.Add("@State", objMEDsysStaffDataObject.State)
            parameters.Add("@Zip", objMEDsysStaffDataObject.Zip)
            parameters.Add("@Phone", objMEDsysStaffDataObject.Phone)
            parameters.Add("@Email", objMEDsysStaffDataObject.Email)
            parameters.Add("@Notes", objMEDsysStaffDataObject.Notes)
            parameters.Add("@PositionCode", objMEDsysStaffDataObject.PositionCode)
            parameters.Add("@TeamCode", objMEDsysStaffDataObject.TeamCode)
            parameters.Add("@Status", objMEDsysStaffDataObject.Status)
            parameters.Add("@StatusDate", objMEDsysStaffDataObject.StatusDate)
            parameters.Add("@StartDate", objMEDsysStaffDataObject.StartDate)
            parameters.Add("@EndDate", objMEDsysStaffDataObject.EndDate)
            parameters.Add("@Action", objMEDsysStaffDataObject.Action)
            parameters.Add("@EmployeeId", objMEDsysStaffDataObject.EmployeeId)
            parameters.Add("@CompanyCode", objMEDsysStaffDataObject.CompanyCode)
            parameters.Add("@LocationCode", objMEDsysStaffDataObject.LocationCode)
        End Sub

        Public Sub DeleteMEDsysStaffInfo(ConVisitel As SqlConnection, Id As Int64, DeletedBy As String)

            Dim parameters As New HybridDictionary()

            parameters.Add("@Id", Id)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteMEDsysStaffInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Function SelectMEDsysStaffInfo(ConVisitel As SqlConnection) As List(Of MEDsysStaffDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SelectMEDsysStaffs]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim MEDsysStaffs As New List(Of MEDsysStaffDataObject)()

            If drSql.HasRows Then
                Dim objMEDsysStaffDataObject As MEDsysStaffDataObject
                While drSql.Read()
                    objMEDsysStaffDataObject = New MEDsysStaffDataObject()

                    objMEDsysStaffDataObject.Id = If((DBNull.Value.Equals(drSql("ID"))), objMEDsysStaffDataObject.Id, Convert.ToInt32(drSql("ID")))
                    objMEDsysStaffDataObject.AccountId = If((DBNull.Value.Equals(drSql("AccountID"))), objMEDsysStaffDataObject.AccountId, Convert.ToInt64(drSql("AccountID")))

                    objMEDsysStaffDataObject.ExternalId = Convert.ToString(drSql("ExternalID"), Nothing)

                    objMEDsysStaffDataObject.StaffId = If((DBNull.Value.Equals(drSql("StaffID"))), objMEDsysStaffDataObject.StaffId, Convert.ToInt64(drSql("StaffID")))
                    objMEDsysStaffDataObject.StaffNumber = If((DBNull.Value.Equals(drSql("StaffNo"))), objMEDsysStaffDataObject.StaffNumber, Convert.ToInt64(drSql("StaffNo")))

                    objMEDsysStaffDataObject.Title = Convert.ToString(drSql("Title"), Nothing)
                    objMEDsysStaffDataObject.FirstName = Convert.ToString(drSql("FirstName"), Nothing)
                    objMEDsysStaffDataObject.MiddleInit = Convert.ToString(drSql("MiddleInit"), Nothing)
                    objMEDsysStaffDataObject.LastName = Convert.ToString(drSql("LastName"), Nothing)
                    objMEDsysStaffDataObject.Suffix = Convert.ToString(drSql("Suffix"), Nothing)
                    objMEDsysStaffDataObject.Birthdate = Convert.ToString(drSql("Birthdate"), Nothing)
                    objMEDsysStaffDataObject.Gender = Convert.ToString(drSql("Gender"), Nothing)
                    objMEDsysStaffDataObject.GIN = Convert.ToString(drSql("GIN"), Nothing)
                    objMEDsysStaffDataObject.Address = Convert.ToString(drSql("Address"), Nothing)
                    objMEDsysStaffDataObject.AddressTwo = Convert.ToString(drSql("Address2"), Nothing)
                    objMEDsysStaffDataObject.City = Convert.ToString(drSql("City"), Nothing)
                    objMEDsysStaffDataObject.State = Convert.ToString(drSql("State"), Nothing)
                    objMEDsysStaffDataObject.Zip = Convert.ToString(drSql("Zip"), Nothing)
                    objMEDsysStaffDataObject.Phone = Convert.ToString(drSql("Phone"), Nothing)
                    objMEDsysStaffDataObject.Email = Convert.ToString(drSql("Email"), Nothing)
                    objMEDsysStaffDataObject.Notes = Convert.ToString(drSql("Notes"), Nothing)
                    objMEDsysStaffDataObject.PositionCode = Convert.ToString(drSql("PositionCode"), Nothing)
                    objMEDsysStaffDataObject.TeamCode = Convert.ToString(drSql("TeamCode"), Nothing)
                    objMEDsysStaffDataObject.Status = Convert.ToString(drSql("Status"), Nothing)
                    objMEDsysStaffDataObject.StatusDate = Convert.ToString(drSql("StatusDate"), Nothing)
                    objMEDsysStaffDataObject.StartDate = Convert.ToString(drSql("StartDate"), Nothing)
                    objMEDsysStaffDataObject.EndDate = Convert.ToString(drSql("EndDate"), Nothing)
                    objMEDsysStaffDataObject.Action = Convert.ToString(drSql("Action"), Nothing)

                    objMEDsysStaffDataObject.EmployeeId = If((DBNull.Value.Equals(drSql("Emp_Id"))), objMEDsysStaffDataObject.EmployeeId, Convert.ToInt64(drSql("Emp_Id")))

                    objMEDsysStaffDataObject.CompanyCode = Convert.ToString(drSql("CompanyCode"), Nothing)
                    objMEDsysStaffDataObject.LocationCode = Convert.ToString(drSql("LocationCode"), Nothing)

                    objMEDsysStaffDataObject.UpdateDate = Convert.ToString(drSql("Update_Date"), Nothing)
                    objMEDsysStaffDataObject.UpdateBy = Convert.ToString(drSql("Update_By"), Nothing)

                    MEDsysStaffs.Add(objMEDsysStaffDataObject)

                End While
                objMEDsysStaffDataObject = Nothing
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return MEDsysStaffs

        End Function
    End Class
End Namespace

