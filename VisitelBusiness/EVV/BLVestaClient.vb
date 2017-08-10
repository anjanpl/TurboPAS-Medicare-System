#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Vesta Client Data Fetch, Insert, Update
' Author: Anjan Kumar Paul
' Start Date: 25 Dec 2015
' End Date: 25 Dec 2015
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                25 Dec 2015     Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelDA.VisitelDA
Imports VisitelBusiness.VisitelBusiness.DataObject.EVV

Namespace VisitelBusiness
    Public Class BLVestaClient

        Public Sub InsertClientInfo(ConVisitel As SqlConnection, objClientDataObject As ClientDataObject)

            Dim parameters As New HybridDictionary()

            SetParameters(parameters, objClientDataObject)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertVestaClientInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub UpdateClientInfo(ConVisitel As SqlConnection, objClientDataObject As ClientDataObject)

            Dim parameters As New HybridDictionary()

            parameters.Add("@Id", objClientDataObject.Id)

            SetParameters(parameters, objClientDataObject)

            parameters.Add("@UpdateBy", objClientDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateVestaClientInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Private Sub SetParameters(parameters As HybridDictionary, objClientDataObject As ClientDataObject)

            parameters.Add("@MyUniqueId", objClientDataObject.MyUniqueID)
            parameters.Add("@ClientIDVesta", objClientDataObject.ClientIdVesta)

            If (Not String.IsNullOrEmpty(objClientDataObject.EVVId)) Then
                parameters.Add("@EVVID", objClientDataObject.EVVId)
            End If

            parameters.Add("@LastName", objClientDataObject.LastName)
            parameters.Add("@FirstName", objClientDataObject.FirstName)
            parameters.Add("@Medicaid", objClientDataObject.Medicaid)

            If (Not String.IsNullOrEmpty(objClientDataObject.EVVId)) Then
                parameters.Add("@DOB", objClientDataObject.DateOfBirth)
            End If

            parameters.Add("@Gender", objClientDataObject.Gender)
            parameters.Add("@Phone", objClientDataObject.Phone)

            If (Not String.IsNullOrEmpty(objClientDataObject.PhoneTwo)) Then
                parameters.Add("@PhoneTwo", objClientDataObject.PhoneTwo)
            End If

            If (Not String.IsNullOrEmpty(objClientDataObject.PhoneThree)) Then
                parameters.Add("@PhoneThree", objClientDataObject.PhoneThree)
            End If

            If (Not String.IsNullOrEmpty(objClientDataObject.AddressOne)) Then
                parameters.Add("@AddressOne", objClientDataObject.AddressOne)
            End If

            If (Not String.IsNullOrEmpty(objClientDataObject.AddressTwo)) Then
                parameters.Add("@AddressTwo", objClientDataObject.AddressTwo)
            End If

            parameters.Add("@City", objClientDataObject.City)
            parameters.Add("@State", objClientDataObject.State)
            parameters.Add("@Zip", objClientDataObject.Zip)

            If (Not String.IsNullOrEmpty(objClientDataObject.Priority)) Then
                parameters.Add("@Priority", objClientDataObject.Priority)
            End If

            If (Not String.IsNullOrEmpty(objClientDataObject.ClientDeviceType)) Then
                parameters.Add("@ClientDeviceType", objClientDataObject.ClientDeviceType)
            End If

            If (Not String.IsNullOrEmpty(objClientDataObject.MedicaidStartDate)) Then
                parameters.Add("@MedicaidStartDate", objClientDataObject.MedicaidStartDate)
            End If

            If (Not String.IsNullOrEmpty(objClientDataObject.MedicaidEndDate)) Then
                parameters.Add("@MedicaidEndDate", objClientDataObject.MedicaidEndDate)
            End If

            If (Not String.IsNullOrEmpty(objClientDataObject.DADSIndividualRegion)) Then
                parameters.Add("@DADSIndvRegion", objClientDataObject.DADSIndividualRegion)
            End If

            If (Not String.IsNullOrEmpty(objClientDataObject.IndvMbrProgram)) Then
                parameters.Add("@IndvMbrProgram", objClientDataObject.IndvMbrProgram)
            End If

            If (Not String.IsNullOrEmpty(objClientDataObject.MCOMbrSDA)) Then
                parameters.Add("@MCOMbrSDA", objClientDataObject.MCOMbrSDA)
            End If

            If (Not String.IsNullOrEmpty(objClientDataObject.MbrEnrollStartDate)) Then
                parameters.Add("@MbrEnrollStartDate", objClientDataObject.MbrEnrollStartDate)
            End If

            If (Not String.IsNullOrEmpty(objClientDataObject.MbrEnrollEndDate)) Then
                parameters.Add("@MbrEnrollEndDate", objClientDataObject.MbrEnrollEndDate)
            End If

            If (Not String.IsNullOrEmpty(objClientDataObject.MbrEnrollEndDate)) Then
                parameters.Add("@Error", objClientDataObject.Error)
            End If

            parameters.Add("@ClientId", objClientDataObject.ClientId)
            'parameters.Add("@Payer", objClientDataObject.Payer)

            If (Not String.IsNullOrEmpty(objClientDataObject.BranchName)) Then
                parameters.Add("@BranchName", objClientDataObject.BranchName)
            End If

            parameters.Add("@SupervisorId", objClientDataObject.VendorSupervisorId)

            If (Not String.IsNullOrEmpty(objClientDataObject.VendorSupervisorFirstName)) Then
                parameters.Add("@SupervisonrFirstName", objClientDataObject.VendorSupervisorFirstName)
            End If

            If (Not String.IsNullOrEmpty(objClientDataObject.VendorSupervisorLastName)) Then
                parameters.Add("@SupervisorLastName", objClientDataObject.VendorSupervisorLastName)
            End If

        End Sub

        Public Sub DeleteClientInfo(ConVisitel As SqlConnection, Id As Int64, DeletedBy As String)

            Dim parameters As New HybridDictionary()

            parameters.Add("@Id", Id)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteVestaClientInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Function SelectClientInfo(ConVisitel As SqlConnection, Optional ClientId As Integer = Integer.MinValue) As List(Of ClientDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()

            If (ClientId > 0) Then
                parameters.Add("@ClientId", ClientId)
            End If

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectVestaClient]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim Clients As New List(Of ClientDataObject)()

            If drSql.HasRows Then
                Dim objClientDataObject As ClientDataObject
                While drSql.Read()
                    objClientDataObject = New ClientDataObject()

                    objClientDataObject.Id = If((DBNull.Value.Equals(drSql("ID"))), objClientDataObject.Id, Convert.ToInt32(drSql("ID")))

                    objClientDataObject.MyUniqueID = Convert.ToString(drSql("MyUniqueID"), Nothing)
                    objClientDataObject.ClientIdVesta = Convert.ToString(drSql("ClientID_Vesta"), Nothing)
                    objClientDataObject.EVVId = Convert.ToString(drSql("EVV_ID"), Nothing)
                    objClientDataObject.LastName = Convert.ToString(drSql("Lastname"), Nothing)
                    objClientDataObject.FirstName = Convert.ToString(drSql("Firstname"), Nothing)
                    objClientDataObject.Medicaid = Convert.ToString(drSql("Medicaid"), Nothing)
                    objClientDataObject.DateOfBirth = Convert.ToString(drSql("DOB"), Nothing)
                    objClientDataObject.Gender = Convert.ToString(drSql("Gender"), Nothing)
                    objClientDataObject.Phone = Convert.ToString(drSql("Phone"), Nothing)
                    objClientDataObject.PhoneTwo = Convert.ToString(drSql("Phone_2"), Nothing)
                    objClientDataObject.PhoneThree = Convert.ToString(drSql("Phone_3"), Nothing)
                    objClientDataObject.AddressOne = Convert.ToString(drSql("Address_1"), Nothing)
                    objClientDataObject.AddressTwo = Convert.ToString(drSql("Address_2"), Nothing)
                    objClientDataObject.City = Convert.ToString(drSql("City"), Nothing)
                    objClientDataObject.State = Convert.ToString(drSql("State"), Nothing)
                    objClientDataObject.Zip = Convert.ToString(drSql("Zip"), Nothing)
                    objClientDataObject.Priority = Convert.ToString(drSql("Priority"), Nothing)
                    objClientDataObject.ClientDeviceType = Convert.ToString(drSql("Client_Device_Type"), Nothing)
                    objClientDataObject.MedicaidStartDate = Convert.ToString(drSql("Medicaid_Start_Date"), Nothing)
                    objClientDataObject.MedicaidEndDate = Convert.ToString(drSql("Medicaid_End_Date"), Nothing)
                    objClientDataObject.DADSIndividualRegion = Convert.ToString(drSql("DADS_indv_Region"), Nothing)
                    objClientDataObject.IndvMbrProgram = Convert.ToString(drSql("IndvMbr_Program"), Nothing)
                    objClientDataObject.MCOMbrSDA = Convert.ToString(drSql("MCO_Mbr_SDA"), Nothing)
                    objClientDataObject.MbrEnrollStartDate = Convert.ToString(drSql("Mbr_Enroll_Start_Date"), Nothing)
                    objClientDataObject.MbrEnrollEndDate = Convert.ToString(drSql("Mbr_Enroll_End_Date"), Nothing)
                    objClientDataObject.BranchName = Convert.ToString(drSql("Branch_Name"), Nothing)
                    objClientDataObject.VendorSupervisorId = Convert.ToString(drSql("Sup_ID"), Nothing)
                    objClientDataObject.VendorSupervisorFirstName = Convert.ToString(drSql("SupFname"), Nothing)
                    objClientDataObject.VendorSupervisorLastName = Convert.ToString(drSql("SupLname"), Nothing)
                    objClientDataObject.[Error] = Convert.ToString(drSql("Error"), Nothing)

                    objClientDataObject.ClientId = If((DBNull.Value.Equals(drSql("client_id"))), objClientDataObject.ClientId, Convert.ToInt64(drSql("client_id"), Nothing))

                    objClientDataObject.UpdateDate = Convert.ToString(drSql("update_date"), Nothing)
                    objClientDataObject.UpdateBy = Convert.ToString(drSql("update_by"), Nothing)

                    Clients.Add(objClientDataObject)

                End While
                objClientDataObject = Nothing
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return Clients

        End Function
    End Class
End Namespace

