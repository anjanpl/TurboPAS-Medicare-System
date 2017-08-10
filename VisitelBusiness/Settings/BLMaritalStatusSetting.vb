#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Marital Status Information Inserting, Updating, Deleting, Filtering, & Fetching
' Author: Anjan Kumar Paul
' Start Date: 25 Aug 2014
' End Date: 25 Aug 2014
' History:
'      Version                  Author                      Date             Reason 
'      1.0.0                                                25 Aug 2014      Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.DataObject.Settings
Imports VisitelDA.VisitelDA

Namespace VisitelBusiness.Settings
    Public Class BLMaritalStatusSetting
        Public Sub InsertMaritalStatusInfo(ConVisitel As SqlConnection, objMaritalStatusSettingDataObject As MaritalStatusSettingDataObject)
            Dim parameters As New HybridDictionary()
            parameters.Add("@Name", objMaritalStatusSettingDataObject.Name)
            parameters.Add("@Status", objMaritalStatusSettingDataObject.Status)
            parameters.Add("@CompanyId", objMaritalStatusSettingDataObject.CompanyId)
            parameters.Add("@UserId", objMaritalStatusSettingDataObject.UserId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertMaritalStatusInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        Public Sub UpdateMaritalStatusInfo(ConVisitel As SqlConnection, objMaritalStatusSettingDataObject As MaritalStatusSettingDataObject)
            Dim parameters As New HybridDictionary()

            parameters.Add("@IdNumber", objMaritalStatusSettingDataObject.IdNumber)
            parameters.Add("@Name", objMaritalStatusSettingDataObject.Name)
            parameters.Add("@Status", objMaritalStatusSettingDataObject.Status)
            parameters.Add("@UpdateBy", objMaritalStatusSettingDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateMaritalStatusInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub DeleteMaritalStatusInfo(ConVisitel As SqlConnection, IdNumber As Integer, DeletedBy As Integer)
            Dim parameters As New HybridDictionary()
            parameters.Add("@IdNumber", IdNumber)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteMaritalStatusInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        ''' <summary>
        ''' Retrieving Marital Status Info
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SelectMaritalStatusInfo(ConVisitel As SqlConnection, CompanyId As Integer) As List(Of MaritalStatusSettingDataObject)

            Dim drSql As SqlDataReader = Nothing
            Dim parameters As New HybridDictionary()

            parameters.Add("@CompanyId", CompanyId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SelectMaritalStatusInfo]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim MaritalStatusSettingList As New List(Of MaritalStatusSettingDataObject)()

            FillDataList(drSql, MaritalStatusSettingList)

            Return MaritalStatusSettingList

        End Function

        ''' <summary>
        ''' Searching Marital Status Info
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <param name="Name"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SearchMaritalStatusInfo(ConVisitel As SqlConnection, CompanyId As Integer, Name As String) As List(Of MaritalStatusSettingDataObject)

            Dim drSql As SqlDataReader = Nothing
            Dim parameters As New HybridDictionary()

            parameters.Add("@CompanyId", CompanyId)

            If (Not String.IsNullOrEmpty(Name)) Then
                parameters.Add("@Name", Name)
            End If

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SearchMaritalStatusInfo]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim MaritalStatusSettingList As New List(Of MaritalStatusSettingDataObject)()

            FillDataList(drSql, MaritalStatusSettingList)

            Return MaritalStatusSettingList

        End Function

        Private Sub FillDataList(ByRef drSql As SqlDataReader, ByRef MaritalStatusSettingList As List(Of MaritalStatusSettingDataObject))

            If drSql.HasRows Then
                Dim objMaritalStatusSettingDataObject As MaritalStatusSettingDataObject
                While drSql.Read()
                    objMaritalStatusSettingDataObject = New MaritalStatusSettingDataObject()

                    objMaritalStatusSettingDataObject.IdNumber = If((Not DBNull.Value.Equals(drSql("IdNumber"))),
                                                                     Convert.ToInt32(drSql("IdNumber"), Nothing),
                                                                     objMaritalStatusSettingDataObject.IdNumber)

                    objMaritalStatusSettingDataObject.Name = Convert.ToString(drSql("Name"), Nothing)

                    objMaritalStatusSettingDataObject.Status = If((Not DBNull.Value.Equals(drSql("Status"))),
                                                                    Convert.ToInt16(drSql("Status"), Nothing),
                                                                    objMaritalStatusSettingDataObject.Status)

                    objMaritalStatusSettingDataObject.UpdateBy = Convert.ToString(drSql("UpdateBy"), Nothing)
                    objMaritalStatusSettingDataObject.UpdateDate = Convert.ToString(drSql("UpdateDate"), Nothing)

                    MaritalStatusSettingList.Add(objMaritalStatusSettingDataObject)
                End While
                objMaritalStatusSettingDataObject = Nothing
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing
        End Sub

    End Class
End Namespace

