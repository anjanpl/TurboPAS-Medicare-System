#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: MEDsys Client Data Fetch, Insert, Update
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
    Public Class BLMEDsysClient

        Public Sub InsertMEDsysClientInfo(ConVisitel As SqlConnection, objMEDsysClientDataObject As MEDsysClientDataObject)

            Dim parameters As New HybridDictionary()

            SetParameters(parameters, objMEDsysClientDataObject)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertMEDsysClientsInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub UpdateMEDsysClientInfo(ConVisitel As SqlConnection, objMEDsysClientDataObject As MEDsysClientDataObject)

            Dim parameters As New HybridDictionary()

            parameters.Add("@Id", objMEDsysClientDataObject.Id)

            SetParameters(parameters, objMEDsysClientDataObject)

            parameters.Add("@UpdateBy", objMEDsysClientDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateMEDsysClientsInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Private Sub SetParameters(parameters As HybridDictionary, objMEDsysClientDataObject As MEDsysClientDataObject)
            parameters.Add("@AccountId", objMEDsysClientDataObject.AccountId)
            parameters.Add("@ExternalId", objMEDsysClientDataObject.ExternalId)
            parameters.Add("@ClientId", objMEDsysClientDataObject.ClientId)
            parameters.Add("@ClientNo", objMEDsysClientDataObject.ClientNumber)
            parameters.Add("@LastName", objMEDsysClientDataObject.LastName)
            parameters.Add("@FirstName", objMEDsysClientDataObject.FirstName)
            parameters.Add("@MiddleInit", objMEDsysClientDataObject.MiddleInit)
            parameters.Add("@Birthdate", objMEDsysClientDataObject.Birthdate)
            parameters.Add("@Gender", objMEDsysClientDataObject.Gender)
            parameters.Add("@GIN", objMEDsysClientDataObject.GIN)
            parameters.Add("@Address", objMEDsysClientDataObject.Address)
            parameters.Add("@City", objMEDsysClientDataObject.City)
            parameters.Add("@State", objMEDsysClientDataObject.State)
            parameters.Add("@Zip", objMEDsysClientDataObject.Zip)
            parameters.Add("@Phone", objMEDsysClientDataObject.Phone)
            parameters.Add("@Notes", objMEDsysClientDataObject.Notes)
            parameters.Add("@ProgramCode", objMEDsysClientDataObject.ProgramCode)
            parameters.Add("@Region", objMEDsysClientDataObject.Region)
            parameters.Add("@Status", objMEDsysClientDataObject.Status)
            parameters.Add("@StartDate", objMEDsysClientDataObject.StartDate)
            parameters.Add("@EndDate", objMEDsysClientDataObject.EndDate)
            parameters.Add("@Payor", objMEDsysClientDataObject.Payor)
            parameters.Add("@ServiceGroup", objMEDsysClientDataObject.ServiceGroup)
            parameters.Add("@PayorServiceCode", objMEDsysClientDataObject.PayorServiceCode)
            parameters.Add("@ContractNo", objMEDsysClientDataObject.ContractNumber)
            parameters.Add("@Action", objMEDsysClientDataObject.Action)
            parameters.Add("@Client_Id", objMEDsysClientDataObject.Client_Id)
            parameters.Add("@CompanyCode", objMEDsysClientDataObject.CompanyCode)
            parameters.Add("@LocationCode", objMEDsysClientDataObject.LocationCode)
        End Sub

        Public Sub DeleteMEDsysClientInfo(ConVisitel As SqlConnection, Id As Int64, DeletedBy As String)

            Dim parameters As New HybridDictionary()

            parameters.Add("@Id", Id)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteMEDsysClientInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Function SelectMEDsysClientInfo(ConVisitel As SqlConnection) As List(Of MEDsysClientDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SelectMEDsysClients]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim MEDsysClients As New List(Of MEDsysClientDataObject)()

            If drSql.HasRows Then
                Dim objMEDsysClientDataObject As MEDsysClientDataObject
                While drSql.Read()
                    objMEDsysClientDataObject = New MEDsysClientDataObject()

                    objMEDsysClientDataObject.Id = If((DBNull.Value.Equals(drSql("ID"))), objMEDsysClientDataObject.Id, Convert.ToInt32(drSql("ID")))
                    objMEDsysClientDataObject.AccountId = If((DBNull.Value.Equals(drSql("AccountID"))), objMEDsysClientDataObject.AccountId, Convert.ToInt64(drSql("AccountID")))

                    objMEDsysClientDataObject.ExternalId = Convert.ToString(drSql("ExternalID"), Nothing)

                    objMEDsysClientDataObject.ClientId = If((DBNull.Value.Equals(drSql("ClientID"))), objMEDsysClientDataObject.ClientId, Convert.ToInt64(drSql("ClientID")))
                    objMEDsysClientDataObject.ClientNumber = If((DBNull.Value.Equals(drSql("ClientNo"))), objMEDsysClientDataObject.ClientNumber, Convert.ToInt64(drSql("ClientNo")))

                    objMEDsysClientDataObject.LastName = Convert.ToString(drSql("LastName"), Nothing)
                    objMEDsysClientDataObject.FirstName = Convert.ToString(drSql("FirstName"), Nothing)
                    objMEDsysClientDataObject.MiddleInit = Convert.ToString(drSql("MiddleInit"), Nothing)
                    objMEDsysClientDataObject.Birthdate = Convert.ToString(drSql("Birthdate"), Nothing)
                    objMEDsysClientDataObject.Gender = Convert.ToString(drSql("Gender"), Nothing)
                    objMEDsysClientDataObject.GIN = Convert.ToString(drSql("GIN"), Nothing)
                    objMEDsysClientDataObject.Address = Convert.ToString(drSql("Address"), Nothing)
                    objMEDsysClientDataObject.City = Convert.ToString(drSql("City"), Nothing)
                    objMEDsysClientDataObject.State = Convert.ToString(drSql("State"), Nothing)
                    objMEDsysClientDataObject.Zip = Convert.ToString(drSql("Zip"), Nothing)
                    objMEDsysClientDataObject.Phone = Convert.ToString(drSql("Phone"), Nothing)
                    objMEDsysClientDataObject.Notes = Convert.ToString(drSql("Notes"), Nothing)
                    objMEDsysClientDataObject.ProgramCode = Convert.ToString(drSql("ProgramCode"), Nothing)
                    objMEDsysClientDataObject.Region = Convert.ToString(drSql("Region"), Nothing)
                    objMEDsysClientDataObject.Status = Convert.ToString(drSql("Status"), Nothing)
                    objMEDsysClientDataObject.StartDate = Convert.ToString(drSql("StartDate"), Nothing)
                    objMEDsysClientDataObject.EndDate = Convert.ToString(drSql("EndDate"), Nothing)
                    objMEDsysClientDataObject.Payor = Convert.ToString(drSql("Payor"), Nothing)
                    objMEDsysClientDataObject.ServiceGroup = Convert.ToString(drSql("ServiceGroup"), Nothing)
                    objMEDsysClientDataObject.PayorServiceCode = Convert.ToString(drSql("PayorServiceCode"), Nothing)
                    objMEDsysClientDataObject.ContractNumber = Convert.ToString(drSql("ContractNo"), Nothing)
                    objMEDsysClientDataObject.Action = Convert.ToString(drSql("Action"), Nothing)
                    objMEDsysClientDataObject.Client_Id = If((DBNull.Value.Equals(drSql("Client_Id"))), objMEDsysClientDataObject.Client_Id, Convert.ToInt64(drSql("Client_Id")))
                    objMEDsysClientDataObject.CompanyCode = Convert.ToString(drSql("CompanyCode"), Nothing)
                    objMEDsysClientDataObject.LocationCode = Convert.ToString(drSql("LocationCode"), Nothing)

                    objMEDsysClientDataObject.UpdateDate = Convert.ToString(drSql("Update_Date"), Nothing)
                    objMEDsysClientDataObject.UpdateBy = Convert.ToString(drSql("Update_By"), Nothing)

                    MEDsysClients.Add(objMEDsysClientDataObject)

                End While
                objMEDsysClientDataObject = Nothing
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return MEDsysClients

        End Function
    End Class
End Namespace

