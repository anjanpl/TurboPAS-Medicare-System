#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: County Information Inserting, Updating, Deleting, Filtering & Fetching
' Author: Anjan Kumar Paul
' Start Date: 05 Sept 2014
' End Date: 05 Sept 2014
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                05 Sept 2014     Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.DataObject.Settings
Imports VisitelDA.VisitelDA

Namespace VisitelBusiness.Settings
    Public Class BLCountySetting
        Public Sub InsertCountyInfo(ConVisitel As SqlConnection, objCountySettingDataObject As CountySettingDataObject)
            Dim parameters As New HybridDictionary()

            parameters.Add("@CountyCode", objCountySettingDataObject.CountyCode)
            parameters.Add("@CountyName", objCountySettingDataObject.CountyName)
            parameters.Add("@CompanyId", objCountySettingDataObject.CompanyId)
            parameters.Add("@UserId", objCountySettingDataObject.UserId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertCountyInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        Public Sub UpdateCountyInfo(ConVisitel As SqlConnection, objCountySettingDataObject As CountySettingDataObject)
            Dim parameters As New HybridDictionary()

            parameters.Add("@CountyId", objCountySettingDataObject.CountyId)
            parameters.Add("@CountyCode", objCountySettingDataObject.CountyCode)
            parameters.Add("@CountyName", objCountySettingDataObject.CountyName)
            parameters.Add("@UpdateBy", objCountySettingDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateCountyInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub DeleteCountyInfo(ConVisitel As SqlConnection, CountyId As Integer, DeletedBy As Integer)
            Dim parameters As New HybridDictionary()
            parameters.Add("@CountyId", CountyId)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteCountyInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        ''' <summary>
        ''' Retrieving County Info
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SelectCountyInfo(ConVisitel As SqlConnection, CompanyId As Integer) As List(Of CountySettingDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()

            parameters.Add("@CompanyId", CompanyId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SelectCountyInfo]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim CountySettingList As New List(Of CountySettingDataObject)()

            FillDataList(drSql, CountySettingList)

            Return CountySettingList

        End Function

        ''' <summary>
        ''' Searching County Info
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <param name="CountyName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SearchCountyInfo(ConVisitel As SqlConnection, CompanyId As Integer, CountyName As String) As List(Of CountySettingDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()

            parameters.Add("@CompanyId", CompanyId)

            If (Not String.IsNullOrEmpty(CountyName)) Then
                parameters.Add("@CountyName", CountyName)
            End If

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SearchCountyInfo]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim CountySettingList As New List(Of CountySettingDataObject)()
           
            FillDataList(drSql, CountySettingList)

            Return CountySettingList

        End Function

        Private Sub FillDataList(ByRef drSql As SqlDataReader, ByRef CountySettingList As List(Of CountySettingDataObject))

            If drSql.HasRows Then
                Dim objCountySettingDataObject As CountySettingDataObject
                While drSql.Read()
                    objCountySettingDataObject = New CountySettingDataObject()

                    objCountySettingDataObject.CountyId = If((Not DBNull.Value.Equals(drSql("CountyId"))),
                                                                     Convert.ToInt32(drSql("CountyId"), Nothing),
                                                                     objCountySettingDataObject.CountyId)

                    objCountySettingDataObject.CountyCode = Convert.ToString(drSql("CountyCode"), Nothing)
                    objCountySettingDataObject.CountyName = Convert.ToString(drSql("CountyName"), Nothing)
                    objCountySettingDataObject.UpdateBy = Convert.ToString(drSql("UpdateBy"), Nothing)
                    objCountySettingDataObject.UpdateDate = Convert.ToString(drSql("UpdateDate"), Nothing)

                    CountySettingList.Add(objCountySettingDataObject)
                End While
                objCountySettingDataObject = Nothing
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing
        End Sub

    End Class
End Namespace

