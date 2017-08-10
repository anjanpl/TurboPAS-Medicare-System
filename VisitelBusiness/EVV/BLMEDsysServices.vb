#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: MEDsys Services Data Fetch, Insert, Update
' Author: Anjan Kumar Paul
' Start Date: 10 Jan 2016
' End Date: 10 Jan 2016
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                10 Jan 2016     Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelDA.VisitelDA
Imports VisitelBusiness.VisitelBusiness.DataObject.EVV

Namespace VisitelBusiness
    Public Class BLMEDsysServices

        Public Sub InsertMEDsysServicesInfo(ConVisitel As SqlConnection, objMEDsysServicesDataObject As MEDsysServicesDataObject)

            Dim parameters As New HybridDictionary()

            SetParameters(parameters, objMEDsysServicesDataObject)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertMEDsysServicesInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub UpdateMEDsysServicesInfo(ConVisitel As SqlConnection, objMEDsysServicesDataObject As MEDsysServicesDataObject)

            Dim parameters As New HybridDictionary()

            parameters.Add("@Id", objMEDsysServicesDataObject.Id)

            SetParameters(parameters, objMEDsysServicesDataObject)

            parameters.Add("@UpdateBy", objMEDsysServicesDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateMEDsysServicesInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Private Sub SetParameters(parameters As HybridDictionary, objMEDsysServicesDataObject As MEDsysServicesDataObject)
            parameters.Add("@AccountId", objMEDsysServicesDataObject.AccountId)
            parameters.Add("@ServiceCode", objMEDsysServicesDataObject.ServiceCode)
            parameters.Add("@ServiceName", objMEDsysServicesDataObject.ServiceName)
        End Sub

        Public Sub DeleteMEDsysServicesInfo(ConVisitel As SqlConnection, Id As Int64, DeletedBy As String)

            Dim parameters As New HybridDictionary()

            parameters.Add("@Id", Id)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteMEDsysServicesInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Function SelectMEDsysServicesInfo(ConVisitel As SqlConnection) As List(Of MEDsysServicesDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SelectMEDsysServices]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim MEDsysServices As New List(Of MEDsysServicesDataObject)()

            If drSql.HasRows Then
                Dim objMEDsysServicesDataObject As MEDsysServicesDataObject
                While drSql.Read()
                    objMEDsysServicesDataObject = New MEDsysServicesDataObject()

                    objMEDsysServicesDataObject.Id = If((DBNull.Value.Equals(drSql("ID"))), objMEDsysServicesDataObject.Id, Convert.ToInt32(drSql("ID")))

                    objMEDsysServicesDataObject.AccountId = Convert.ToString(drSql("AccountId"), Nothing)
                    objMEDsysServicesDataObject.ServiceCode = Convert.ToString(drSql("ServiceCode"), Nothing)
                    objMEDsysServicesDataObject.ServiceName = Convert.ToString(drSql("ServiceName"), Nothing)

                    objMEDsysServicesDataObject.UpdateDate = Convert.ToString(drSql("UpdateDate"), Nothing)
                    objMEDsysServicesDataObject.UpdateBy = Convert.ToString(drSql("UpdateBy"), Nothing)

                    MEDsysServices.Add(objMEDsysServicesDataObject)

                End While
                objMEDsysServicesDataObject = Nothing
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return MEDsysServices

        End Function
    End Class
End Namespace

