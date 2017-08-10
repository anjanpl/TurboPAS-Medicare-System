#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Client Status Information Inserting, Updating, Deleting, Filtering & Fetching
' Author: Anjan Kumar Paul
' Start Date: 26 Aug 2014
' End Date: 26 Aug 2014
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                26 Aug 2014     Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region


Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.DataObject.Settings
Imports VisitelDA.VisitelDA

Namespace VisitelBusiness.Settings
    Public Class BLClientStatusSetting

        Public Sub InsertClientStatusInfo(ConVisitel As SqlConnection, objClientStatusSettingDataObject As ClientStatusSettingDataObject)
            Dim parameters As New HybridDictionary()
            parameters.Add("@Name", objClientStatusSettingDataObject.Name)
            parameters.Add("@Status", objClientStatusSettingDataObject.Status)
            parameters.Add("@CompanyId", objClientStatusSettingDataObject.CompanyId)
            parameters.Add("@UserId", objClientStatusSettingDataObject.UserId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertClientStatusInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        Public Sub UpdateClientStatusInfo(ConVisitel As SqlConnection, objClientStatusSettingDataObject As ClientStatusSettingDataObject)
            Dim parameters As New HybridDictionary()

            parameters.Add("@IdNumber", objClientStatusSettingDataObject.IdNumber)
            parameters.Add("@Name", objClientStatusSettingDataObject.Name)
            parameters.Add("@Status", objClientStatusSettingDataObject.Status)
            parameters.Add("@UpdateBy", objClientStatusSettingDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateClientStatusInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub DeleteClientStatusInfo(ConVisitel As SqlConnection, IdNumber As Integer, DeletedBy As Integer)
            Dim parameters As New HybridDictionary()
            parameters.Add("@IdNumber", IdNumber)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteClientStatusInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        ''' <summary>
        ''' Retrieving Client Status Info
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SelectClientStatusInfo(ConVisitel As SqlConnection, CompanyId As Integer) As List(Of ClientStatusSettingDataObject)

            Dim drSql As SqlDataReader = Nothing
            Dim parameters As New HybridDictionary()

            parameters.Add("@CompanyId", CompanyId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SelectClientStatusInfo]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim ClientStatusSettingList As New List(Of ClientStatusSettingDataObject)()

            FillDataList(drSql, ClientStatusSettingList)

            Return ClientStatusSettingList

        End Function

        ''' <summary>
        ''' Searching Client Status Info
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <param name="Name"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SearchClientStatusInfo(ConVisitel As SqlConnection, CompanyId As Integer, Name As String) As List(Of ClientStatusSettingDataObject)

            Dim drSql As SqlDataReader = Nothing
            Dim parameters As New HybridDictionary()

            parameters.Add("@CompanyId", CompanyId)

            If (Not String.IsNullOrEmpty(Name)) Then
                parameters.Add("@Name", Name)
            End If

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SearchClientStatusInfo]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim ClientStatusSettingList As New List(Of ClientStatusSettingDataObject)()

            FillDataList(drSql, ClientStatusSettingList)

            Return ClientStatusSettingList

        End Function

        Private Sub FillDataList(ByRef drSql As SqlDataReader, ByRef ClientStatusSettingList As List(Of ClientStatusSettingDataObject))
            If drSql.HasRows Then
                Dim objClientStatusSettingDataObject As ClientStatusSettingDataObject
                While drSql.Read()
                    objClientStatusSettingDataObject = New ClientStatusSettingDataObject()

                    objClientStatusSettingDataObject.IdNumber = If((Not DBNull.Value.Equals(drSql("IdNumber"))),
                                                                     Convert.ToInt32(drSql("IdNumber"), Nothing),
                                                                     objClientStatusSettingDataObject.IdNumber)

                    objClientStatusSettingDataObject.Name = Convert.ToString(drSql("Name"), Nothing)

                    objClientStatusSettingDataObject.Status = If((Not DBNull.Value.Equals(drSql("Status"))),
                                                                     Convert.ToInt32(drSql("Status"), Nothing),
                                                                     objClientStatusSettingDataObject.Status)

                    objClientStatusSettingDataObject.UpdateBy = Convert.ToString(drSql("UpdateBy"), Nothing)
                    objClientStatusSettingDataObject.UpdateDate = Convert.ToString(drSql("UpdateDate"), Nothing)

                    ClientStatusSettingList.Add(objClientStatusSettingDataObject)
                End While
                objClientStatusSettingDataObject = Nothing
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing
        End Sub
    End Class
End Namespace

