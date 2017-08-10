#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: MEDsys Authorization Data Fetch, Insert, Update
' Author: Anjan Kumar Paul
' Start Date: 15 Jan 2016
' End Date: 15 Jan 2016
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                15 Jan 2016     Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelDA.VisitelDA
Imports VisitelBusiness.VisitelBusiness.DataObject.EVV

Namespace VisitelBusiness
    Public Class BLMEDsysAuthorization

        Public Sub InsertMEDsysAuthorizationInfo(ConVisitel As SqlConnection, objMEDsysAuthorizationDataObject As MEDsysAuthorizationDataObject)

            Dim parameters As New HybridDictionary()

            SetParameters(parameters, objMEDsysAuthorizationDataObject)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertMEDsysAuthorizationsInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub UpdateMEDsysAuthorizationInfo(ConVisitel As SqlConnection, objMEDsysAuthorizationDataObject As MEDsysAuthorizationDataObject)

            Dim parameters As New HybridDictionary()

            parameters.Add("@Id", objMEDsysAuthorizationDataObject.Id)

            SetParameters(parameters, objMEDsysAuthorizationDataObject)

            parameters.Add("@UpdateBy", objMEDsysAuthorizationDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateMEDsysAuthorizationsInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Private Sub SetParameters(parameters As HybridDictionary, objMEDsysAuthorizationDataObject As MEDsysAuthorizationDataObject)
            parameters.Add("@Client_Id", objMEDsysAuthorizationDataObject.ClientId)
            parameters.Add("@AccountID", objMEDsysAuthorizationDataObject.AccountId)
            parameters.Add("@ExternalID", objMEDsysAuthorizationDataObject.ExternalId)
            parameters.Add("@AuthorizationID", objMEDsysAuthorizationDataObject.AuthorizationId)
            parameters.Add("@AuthorizationNo", objMEDsysAuthorizationDataObject.AuthorizationNumber)
            parameters.Add("@ControlNo", objMEDsysAuthorizationDataObject.ControlNumber)
            parameters.Add("@DateBegin", objMEDsysAuthorizationDataObject.DateBegin)
            parameters.Add("@DateEnd", objMEDsysAuthorizationDataObject.DateEnd)
            parameters.Add("@ServiceCode", objMEDsysAuthorizationDataObject.ServiceCode)
            parameters.Add("@ActivityCode", objMEDsysAuthorizationDataObject.ActivityCode)
            parameters.Add("@ClientExternalID", objMEDsysAuthorizationDataObject.ClientExternalId)
            parameters.Add("@AuthType", objMEDsysAuthorizationDataObject.AuthType)
            parameters.Add("@Maximum", objMEDsysAuthorizationDataObject.Maximum)
            parameters.Add("@LimitBy", objMEDsysAuthorizationDataObject.LimitBy)
            parameters.Add("@Action", objMEDsysAuthorizationDataObject.Action)
        End Sub

        Public Sub DeleteMEDsysAuthorizationInfo(ConVisitel As SqlConnection, Id As Int64, DeletedBy As String)

            Dim parameters As New HybridDictionary()

            parameters.Add("@Id", Id)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteMEDsysAuthorizationInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Function SelectMEDsysAuthorizationInfo(ConVisitel As SqlConnection) As List(Of MEDsysAuthorizationDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SelectMEDsysAuthorizations]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim MEDsysAuthorizations As New List(Of MEDsysAuthorizationDataObject)()

            If drSql.HasRows Then
                Dim objMEDsysAuthorizationDataObject As MEDsysAuthorizationDataObject
                While drSql.Read()
                    objMEDsysAuthorizationDataObject = New MEDsysAuthorizationDataObject()

                    objMEDsysAuthorizationDataObject.Id = If((DBNull.Value.Equals(drSql("ID"))), objMEDsysAuthorizationDataObject.Id, Convert.ToInt32(drSql("ID")))
                    objMEDsysAuthorizationDataObject.ClientId = If((DBNull.Value.Equals(drSql("Client_Id"))), objMEDsysAuthorizationDataObject.ClientId,
                                                                   Convert.ToInt64(drSql("Client_Id")))
                    objMEDsysAuthorizationDataObject.AccountId = If((DBNull.Value.Equals(drSql("AccountID"))), objMEDsysAuthorizationDataObject.AccountId,
                                                                  Convert.ToInt64(drSql("AccountID")))

                    objMEDsysAuthorizationDataObject.ExternalId = Convert.ToString(drSql("ExternalID"), Nothing)

                    objMEDsysAuthorizationDataObject.AuthorizationId = If((DBNull.Value.Equals(drSql("AuthorizationID"))), objMEDsysAuthorizationDataObject.AuthorizationId,
                                                                 Convert.ToInt64(drSql("AuthorizationID")))

                    objMEDsysAuthorizationDataObject.AuthorizationNumber = Convert.ToString(drSql("AuthorizationNo"), Nothing)
                    objMEDsysAuthorizationDataObject.ControlNumber = Convert.ToString(drSql("ControlNo"), Nothing)
                    objMEDsysAuthorizationDataObject.DateBegin = Convert.ToString(drSql("DateBegin"), Nothing)
                    objMEDsysAuthorizationDataObject.DateEnd = Convert.ToString(drSql("DateEnd"), Nothing)
                    objMEDsysAuthorizationDataObject.ServiceCode = Convert.ToString(drSql("ServiceCode"), Nothing)
                    objMEDsysAuthorizationDataObject.ActivityCode = Convert.ToString(drSql("ActivityCode"), Nothing)
                    objMEDsysAuthorizationDataObject.ClientExternalId = Convert.ToString(drSql("ClientExternalID"), Nothing)
                    objMEDsysAuthorizationDataObject.AuthType = Convert.ToString(drSql("AuthType"), Nothing)
                    objMEDsysAuthorizationDataObject.Maximum = Convert.ToString(drSql("Maximum"), Nothing)
                    objMEDsysAuthorizationDataObject.LimitBy = Convert.ToString(drSql("LimitBy"), Nothing)
                    objMEDsysAuthorizationDataObject.Action = Convert.ToString(drSql("Action"), Nothing)

                    objMEDsysAuthorizationDataObject.UpdateDate = Convert.ToString(drSql("Update_Date"), Nothing)
                    objMEDsysAuthorizationDataObject.UpdateBy = Convert.ToString(drSql("Update_By"), Nothing)

                    MEDsysAuthorizations.Add(objMEDsysAuthorizationDataObject)

                End While
                objMEDsysAuthorizationDataObject = Nothing
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return MEDsysAuthorizations

        End Function
    End Class
End Namespace

