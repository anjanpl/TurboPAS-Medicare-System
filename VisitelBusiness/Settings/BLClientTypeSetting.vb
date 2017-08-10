#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Client Type Information Inserting, Updating, Deleting, Filtering & Fetching
' Author: Anjan Kumar Paul
' Start Date: 27 Aug 2014
' End Date: 27 Aug 2014
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                27 Aug 2014     Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.DataObject.Settings
Imports VisitelDA.VisitelDA

Namespace VisitelBusiness.Settings
    Public Class BLClientTypeSetting

        Public Sub InsertClientTypeInfo(ConVisitel As SqlConnection, objClientTypeSettingDataObject As ClientTypeSettingDataObject, IsConfirm As Boolean)
            Dim parameters As New HybridDictionary()

            SetParameter(parameters, objClientTypeSettingDataObject)

            parameters.Add("@CompanyId", objClientTypeSettingDataObject.CompanyId)
            parameters.Add("@UserId", objClientTypeSettingDataObject.UpdateBy)

            If (IsConfirm) Then
                parameters.Add("@ClientCode", objClientTypeSettingDataObject.ClientCode)
            End If

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertClientTypeInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        Public Sub UpdateClientTypeInfo(ConVisitel As SqlConnection, objClientTypeSettingDataObject As ClientTypeSettingDataObject, IsConfirm As Boolean)
            Dim parameters As New HybridDictionary()

            parameters.Add("@IdNumber", objClientTypeSettingDataObject.IdNumber)

            SetParameter(parameters, objClientTypeSettingDataObject)

            parameters.Add("@UpdateBy", objClientTypeSettingDataObject.UpdateBy)

            If (IsConfirm) Then
                parameters.Add("@ClientCode", objClientTypeSettingDataObject.ClientCode)
            End If

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateClientTypeInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub UpdateUnitRateInfo(ConVisitel As SqlConnection, Id As Int64, UpdateBy As String)
            Dim parameters As New HybridDictionary()

            parameters.Add("@IdNumber", Id)
            parameters.Add("@UpdateBy", UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateUnitRateInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        Public Sub UpdateServiceTypeInfo(ConVisitel As SqlConnection, Id As Int64, UpdateBy As String)
            Dim parameters As New HybridDictionary()

            parameters.Add("@IdNumber", Id)
            parameters.Add("@UpdateBy", UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateServiceTypeInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        Private Sub SetParameter(ByRef parameters As HybridDictionary, ByRef objClientTypeSettingDataObject As ClientTypeSettingDataObject)
            parameters.Add("@Name", objClientTypeSettingDataObject.Name)
            parameters.Add("@ContractNo", objClientTypeSettingDataObject.ContractNo)
            parameters.Add("@Region", objClientTypeSettingDataObject.Region)
            parameters.Add("@ServiceType", objClientTypeSettingDataObject.ServiceTypeId)
            parameters.Add("@ProgramType", objClientTypeSettingDataObject.ProgramType)
            parameters.Add("@UnitRate", objClientTypeSettingDataObject.UnitRate)
            parameters.Add("@ClientGroupId", objClientTypeSettingDataObject.ClientGroupId)
            parameters.Add("@ReceiverId", objClientTypeSettingDataObject.ReceiverId)
            parameters.Add("@PayerId", objClientTypeSettingDataObject.PayerId)
            parameters.Add("@Status", objClientTypeSettingDataObject.Status)
            parameters.Add("@SantraxYN", objClientTypeSettingDataObject.SantraxYN)
            parameters.Add("@CM2000YN", objClientTypeSettingDataObject.CM2000YN)
        End Sub


        Public Sub DeleteClientTypeInfo(ConVisitel As SqlConnection, IdNumber As Integer, DeletedBy As Integer)
            Dim parameters As New HybridDictionary()
            parameters.Add("@IdNumber", IdNumber)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteClientTypeInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        ''' <summary>
        ''' Get Client Type Information
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SelectClientTypeInfo(ConVisitel As SqlConnection, CompanyId As Integer) As List(Of ClientTypeSettingDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()

            parameters.Add("@CompanyId", CompanyId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SelectClientTypeInfo]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim ClientTypeSettingList As New List(Of ClientTypeSettingDataObject)()

            FillDataList(drSql, ClientTypeSettingList)

            Return ClientTypeSettingList

        End Function

        ''' <summary>
        ''' Get Searched Client Type Information
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <param name="Name"></param>
        ''' <param name="ContractNumber"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SearchClientTypeInfo(ConVisitel As SqlConnection, CompanyId As Integer, Name As String, ContractNumber As String) As List(Of ClientTypeSettingDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()

            parameters.Add("@CompanyId", CompanyId)

            If (Not String.IsNullOrEmpty(Name)) Then
                parameters.Add("@Name", Name)
            End If

            If (Not String.IsNullOrEmpty(ContractNumber)) Then
                parameters.Add("@ContractNo", ContractNumber)
            End If

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SearchClientTypeInfo]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim ClientTypeSettingList As New List(Of ClientTypeSettingDataObject)()

            FillDataList(drSql, ClientTypeSettingList)

            Return ClientTypeSettingList

        End Function

        Private Sub FillDataList(ByRef drSql As SqlDataReader, ByRef ClientTypeSettingList As List(Of ClientTypeSettingDataObject))

            If drSql.HasRows Then
                Dim objClientTypeSettingDataObject As ClientTypeSettingDataObject
                While drSql.Read()
                    objClientTypeSettingDataObject = New ClientTypeSettingDataObject()

                    objClientTypeSettingDataObject.IdNumber = If((DBNull.Value.Equals(drSql("IdNumber"))),
                                                                 objClientTypeSettingDataObject.IdNumber,
                                                                 Convert.ToInt32(drSql("IdNumber")))

                    objClientTypeSettingDataObject.Name = Convert.ToString(drSql("Name"), Nothing)
                    objClientTypeSettingDataObject.ContractNo = Convert.ToString(drSql("ContractNo"), Nothing)
                    objClientTypeSettingDataObject.Region = Convert.ToString(drSql("Region"), Nothing)

                    objClientTypeSettingDataObject.ServiceTypeId = If((DBNull.Value.Equals(drSql("ServiceTypeId"))),
                                                                 objClientTypeSettingDataObject.ServiceTypeId,
                                                                 Convert.ToInt32(drSql("ServiceTypeId")))

                    objClientTypeSettingDataObject.ServiceType = Convert.ToString(drSql("ServiceType"), Nothing)

                    objClientTypeSettingDataObject.ProgramType = Convert.ToString(drSql("ProgramType"), Nothing)

                    objClientTypeSettingDataObject.UnitRate = Convert.ToString(drSql("UnitRate"), Nothing)
                    objClientTypeSettingDataObject.ClientGroupId = Convert.ToString(drSql("ClientGroupId"), Nothing)
                    objClientTypeSettingDataObject.ClientGroup = Convert.ToString(drSql("ClientGroup"), Nothing)
                    objClientTypeSettingDataObject.ReceiverId = Convert.ToString(drSql("ReceiverId"), Nothing)
                    objClientTypeSettingDataObject.ReceiverName = Convert.ToString(drSql("ReceiverName"), Nothing)
                    objClientTypeSettingDataObject.PayerId = Convert.ToString(drSql("PayerId"), Nothing)
                    objClientTypeSettingDataObject.Payer = Convert.ToString(drSql("PayerName"), Nothing)

                    objClientTypeSettingDataObject.Status = If((DBNull.Value.Equals(drSql("Status"))),
                                                               objClientTypeSettingDataObject.Status,
                                                               Convert.ToInt16(drSql("Status"), Nothing))

                    objClientTypeSettingDataObject.SantraxYN = If((DBNull.Value.Equals(drSql("SantraxYN"))),
                                                               objClientTypeSettingDataObject.SantraxYN,
                                                               Convert.ToInt16(drSql("SantraxYN"), Nothing))

                    objClientTypeSettingDataObject.CM2000YN = If((DBNull.Value.Equals(drSql("CM2000YN"))),
                                                               objClientTypeSettingDataObject.CM2000YN,
                                                               Convert.ToInt16(drSql("CM2000YN"), Nothing))

                    objClientTypeSettingDataObject.UpdateBy = Convert.ToString(drSql("UpdateBy"), Nothing)
                    objClientTypeSettingDataObject.UpdateDate = Convert.ToString(drSql("UpdateDate"), Nothing)

                    ClientTypeSettingList.Add(objClientTypeSettingDataObject)
                End While
                objClientTypeSettingDataObject = Nothing
            End If

            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

        End Sub

    End Class
End Namespace

