#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: City Information Inserting, Updating, Deleting, Filtering & Fetching
' Author: Anjan Kumar Paul
' Start Date: 23 Aug 2014
' End Date: 23 Aug 2014
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                23 Aug 2014     Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.DataObject.Settings
Imports VisitelDA.VisitelDA

Namespace VisitelBusiness.Settings
    Public Class BLCitySetting
        Public Sub InsertCityInfo(ConVisitel As SqlConnection, objCitySettingDataObject As CitySettingDataObject)
            Dim parameters As New HybridDictionary()
            parameters.Add("@CityName", objCitySettingDataObject.CityName)
            parameters.Add("@CompanyId", objCitySettingDataObject.CompanyId)
            parameters.Add("@UserId", objCitySettingDataObject.UserId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertCityInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        Public Sub UpdateCityInfo(ConVisitel As SqlConnection, objCitySettingDataObject As CitySettingDataObject)
            Dim parameters As New HybridDictionary()

            parameters.Add("@CityId", objCitySettingDataObject.CityId)
            parameters.Add("@CityName", objCitySettingDataObject.CityName)
            parameters.Add("@UpdateBy", objCitySettingDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateCityInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub DeleteCityInfo(ConVisitel As SqlConnection, CityId As Integer, DeletedBy As Integer)
            Dim parameters As New HybridDictionary()
            parameters.Add("@CityId", CityId)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteCityInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        ''' <summary>
        ''' Retrieving City Info
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SelectCityInfo(ConVisitel As SqlConnection, CompanyId As Integer) As List(Of CitySettingDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()

            parameters.Add("@CompanyId", CompanyId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SelectCityInfo]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim CitySettingList As New List(Of CitySettingDataObject)()

            FillDataList(drSql, CitySettingList)

            Return CitySettingList

        End Function

        ''' <summary>
        ''' Searching City Info
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <param name="CityName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SearchCityInfo(ConVisitel As SqlConnection, CompanyId As Integer, CityName As String) As List(Of CitySettingDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()

            parameters.Add("@CompanyId", CompanyId)

            If (Not String.IsNullOrEmpty(CityName)) Then
                parameters.Add("@CityName", CityName)
            End If

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SearchCityInfo]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim CitySettingList As New List(Of CitySettingDataObject)()

            FillDataList(drSql, CitySettingList)

            Return CitySettingList

        End Function

        Private Sub FillDataList(ByRef drSql As SqlDataReader, ByRef CitySettingList As List(Of CitySettingDataObject))

            If drSql.HasRows Then
                Dim objCitySettingDataObject As CitySettingDataObject
                While drSql.Read()
                    objCitySettingDataObject = New CitySettingDataObject()

                    objCitySettingDataObject.CityId = If((DBNull.Value.Equals(drSql("CityId"))), objCitySettingDataObject.CityId, Convert.ToInt32(drSql("CityId"), Nothing))
                    objCitySettingDataObject.CityName = Convert.ToString(drSql("CityName"), Nothing)
                    objCitySettingDataObject.UpdateBy = Convert.ToString(drSql("UpdateBy"), Nothing)
                    objCitySettingDataObject.UpdateDate = Convert.ToString(drSql("UpdateDate"), Nothing)

                    CitySettingList.Add(objCitySettingDataObject)
                End While
                objCitySettingDataObject = Nothing
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

        End Sub

    End Class
End Namespace

