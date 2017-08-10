#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Vesta Authorization Data Fetch, Insert, Update
' Author: Anjan Kumar Paul
' Start Date: 19 Dec 2015
' End Date: 19 Dec 2015
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                19 Dec 2015     Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelDA.VisitelDA
Imports VisitelBusiness.VisitelBusiness.DataObject.EVV

Namespace VisitelBusiness
    Public Class BLVestaAuthorization

        Public Sub InsertAuthorizationInfo(ConVisitel As SqlConnection, objAuthorizationDataObject As AuthorizationDataObject)

            Dim parameters As New HybridDictionary()

            SetParameters(parameters, objAuthorizationDataObject)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertVestaAuthorizationInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub UpdateAuthorizationInfo(ConVisitel As SqlConnection, objAuthorizationDataObject As AuthorizationDataObject)

            Dim parameters As New HybridDictionary()

            parameters.Add("@Id", objAuthorizationDataObject.Id)

            SetParameters(parameters, objAuthorizationDataObject)

            parameters.Add("@UpdateBy", objAuthorizationDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateVestaAuthorizationInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Private Sub SetParameters(parameters As HybridDictionary, objAuthorizationDataObject As AuthorizationDataObject)

            parameters.Add("@MyUniqueID", objAuthorizationDataObject.MyUniqueId)
            parameters.Add("@AuthId", objAuthorizationDataObject.AuthorizationId)
            parameters.Add("@ClientIdVesta", objAuthorizationDataObject.ClientIdVesta)
            parameters.Add("@DadsContractNo", objAuthorizationDataObject.DadsContractNo)
            parameters.Add("@ProgramType", objAuthorizationDataObject.ProgramType)
            parameters.Add("@AuthPayer", objAuthorizationDataObject.AuthorizationPayer)
            parameters.Add("@AuthStartDate", objAuthorizationDataObject.AuthorizationStartDate)
            parameters.Add("@AuthEndDate", objAuthorizationDataObject.AuthorizationEndDate)
            parameters.Add("@AuthUnits", objAuthorizationDataObject.AuthorizationUnits)
            parameters.Add("@AuthUnitsType", objAuthorizationDataObject.AuthorizationUnitsType)
            parameters.Add("@AuthNumber", objAuthorizationDataObject.AuthorizationNumber)
            parameters.Add("@ClientId", objAuthorizationDataObject.ClientId)
            parameters.Add("@PayerId", objAuthorizationDataObject.PayerId)

            If (objAuthorizationDataObject.ClientInfoId > 0) Then
                parameters.Add("@ClientInfoId", objAuthorizationDataObject.ClientInfoId)
            End If

        End Sub

        Public Sub DeleteAuthorizationInfo(ConVisitel As SqlConnection, Id As Int64, DeletedBy As String)

            Dim parameters As New HybridDictionary()

            parameters.Add("@Id", Id)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteVestaAuthorizationInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub
       
        Public Function SelectAuthorization(ConVisitel As SqlConnection, Optional ClientId As Integer = Integer.MinValue) As List(Of AuthorizationDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()

            If (ClientId > 0) Then
                parameters.Add("@ClientId", ClientId)
            End If

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectVestaAuthorization]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim Authorizations As New List(Of AuthorizationDataObject)()

            If drSql.HasRows Then
                Dim objAuthorizationDataObject As AuthorizationDataObject
                While drSql.Read()
                    objAuthorizationDataObject = New AuthorizationDataObject()

                    objAuthorizationDataObject.Id = If((DBNull.Value.Equals(drSql("ID"))), objAuthorizationDataObject.Id, Convert.ToInt32(drSql("ID")))

                    objAuthorizationDataObject.MyUniqueId = Convert.ToString(drSql("MyUniqueID"), Nothing)
                    objAuthorizationDataObject.AuthorizationId = Convert.ToString(drSql("Auth_ID"), Nothing)
                    objAuthorizationDataObject.ClientIdVesta = Convert.ToString(drSql("Client_ID_Vesta"), Nothing)
                    objAuthorizationDataObject.DadsContractNo = Convert.ToString(drSql("Dads_ContractNo"), Nothing)
                    objAuthorizationDataObject.ProgramType = Convert.ToString(drSql("Program_Type"), Nothing)
                    objAuthorizationDataObject.AuthorizationPayer = Convert.ToString(drSql("Auth_Payer"), Nothing)
                    objAuthorizationDataObject.AuthorizationStartDate = Convert.ToString(drSql("Auth_Start_Date"), Nothing)
                    objAuthorizationDataObject.AuthorizationEndDate = Convert.ToString(drSql("Auth_End_Date"), Nothing)
                    objAuthorizationDataObject.AuthorizationUnits = Convert.ToString(drSql("Auth_Units"), Nothing)
                    objAuthorizationDataObject.AuthorizationUnitsType = Convert.ToString(drSql("Auth_Units_Type"), Nothing)
                    objAuthorizationDataObject.AuthorizationNumber = Convert.ToString(drSql("Auth_Number"), Nothing)
                    objAuthorizationDataObject.[Error] = Convert.ToString(drSql("Error"), Nothing)

                    objAuthorizationDataObject.ClientId = If((DBNull.Value.Equals(drSql("Client_Id"))),
                                                             objAuthorizationDataObject.ClientId, Convert.ToInt64(drSql("Client_Id"), Nothing))

                    objAuthorizationDataObject.ClientName = Convert.ToString(drSql("ClientName"), Nothing)

                    objAuthorizationDataObject.PayerId = If((DBNull.Value.Equals(drSql("Payer_Id"))),
                                                            objAuthorizationDataObject.PayerId, Convert.ToInt64(drSql("Payer_Id"), Nothing))

                    objAuthorizationDataObject.UpdateDate = Convert.ToString(drSql("update_date"), Nothing)
                    objAuthorizationDataObject.UpdateBy = Convert.ToString(drSql("update_by"), Nothing)

                    Authorizations.Add(objAuthorizationDataObject)

                End While
                objAuthorizationDataObject = Nothing
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return Authorizations

        End Function
    End Class
End Namespace

