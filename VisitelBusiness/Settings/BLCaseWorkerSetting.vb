#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Case Worker Information Inserting, Updating, Deleting, Filtering and Fetching
' Author: Anjan Kumar Paul
' Start Date: 19 Aug 2014
' End Date: 19 Aug 2014
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                19 Aug 2014     Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.DataObject.Settings
Imports VisitelDA.VisitelDA

Namespace VisitelBusiness.Settings
    Public Class BLCaseWorkerSetting
        Public Sub InsertCaseWorkerInfo(ConVisitel As SqlConnection, objCaseWorkerSettingDataObject As CaseWorkerSettingDataObject, IsConfirm As Boolean)
            Dim parameters As New HybridDictionary()
            parameters.Add("@FirstName", objCaseWorkerSettingDataObject.FirstName)
            parameters.Add("@LastName", objCaseWorkerSettingDataObject.LastName)
            parameters.Add("@Phone", objCaseWorkerSettingDataObject.Phone)
            parameters.Add("@AlternatePhone", objCaseWorkerSettingDataObject.AlternatePhone)
            parameters.Add("@Address", objCaseWorkerSettingDataObject.Address)
            parameters.Add("@Suite", objCaseWorkerSettingDataObject.Suite)
            parameters.Add("@CityId", objCaseWorkerSettingDataObject.CityId)
            parameters.Add("@State", objCaseWorkerSettingDataObject.State)
            parameters.Add("@Zip", objCaseWorkerSettingDataObject.Zip)
            parameters.Add("@Fax", objCaseWorkerSettingDataObject.Fax)
            parameters.Add("@MailCode", objCaseWorkerSettingDataObject.MailCode)
            parameters.Add("@Status", objCaseWorkerSettingDataObject.Status)
            parameters.Add("@Comments", objCaseWorkerSettingDataObject.Comments)
            parameters.Add("@Email", objCaseWorkerSettingDataObject.Email)
            parameters.Add("@FullAddress", objCaseWorkerSettingDataObject.FullAddress)
            parameters.Add("@CompanyId", objCaseWorkerSettingDataObject.CompanyId)
            parameters.Add("@UserId", objCaseWorkerSettingDataObject.UserId)

            If (IsConfirm) Then
                parameters.Add("@CaseWorkerCode", objCaseWorkerSettingDataObject.CaseWorkerCode)
            End If

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertCaseWorkerInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        Public Sub UpdateCaseWorkerInfo(ConVisitel As SqlConnection, objCaseWorkerSettingDataObject As CaseWorkerSettingDataObject, IsConfirm As Boolean)
            Dim parameters As New HybridDictionary()
            parameters.Add("@CaseWorkerId", objCaseWorkerSettingDataObject.CaseWorkerId)
            parameters.Add("@FirstName", objCaseWorkerSettingDataObject.FirstName)
            parameters.Add("@LastName", objCaseWorkerSettingDataObject.LastName)
            parameters.Add("@Phone", objCaseWorkerSettingDataObject.Phone)
            parameters.Add("@AlternatePhone", objCaseWorkerSettingDataObject.AlternatePhone)
            parameters.Add("@Address", objCaseWorkerSettingDataObject.Address)
            parameters.Add("@Suite", objCaseWorkerSettingDataObject.Suite)
            parameters.Add("@CityId", objCaseWorkerSettingDataObject.CityId)
            parameters.Add("@State", objCaseWorkerSettingDataObject.State)
            parameters.Add("@Zip", objCaseWorkerSettingDataObject.Zip)
            parameters.Add("@Fax", objCaseWorkerSettingDataObject.Fax)
            parameters.Add("@MailCode", objCaseWorkerSettingDataObject.MailCode)
            parameters.Add("@Status", objCaseWorkerSettingDataObject.Status)
            parameters.Add("@Comments", objCaseWorkerSettingDataObject.Comments)
            parameters.Add("@Email", objCaseWorkerSettingDataObject.Email)
            parameters.Add("@FullAddress", objCaseWorkerSettingDataObject.FullAddress)
            parameters.Add("@UpdateBy", objCaseWorkerSettingDataObject.UpdateBy)

            If (IsConfirm) Then
                parameters.Add("@CaseWorkerCode", objCaseWorkerSettingDataObject.CaseWorkerCode)
            End If

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateCaseWorkerInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        Public Sub DeleteCaseWorkerInfo(ConVisitel As SqlConnection, CaseWorkerId As Integer, DeletedBy As Integer)
            Dim parameters As New HybridDictionary()
            parameters.Add("@CaseWorkerId", CaseWorkerId)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteCaseWorkerInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        ''' <summary>
        ''' Retrieving Case Worker Info
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SelectCaseWorkerInfo(ConVisitel As SqlConnection, CompanyId As Integer) As List(Of CaseWorkerSettingDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()

            parameters.Add("@CompanyId", CompanyId)

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectCaseWorkerInfo]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim CaseWorkerSettingList As New List(Of CaseWorkerSettingDataObject)()

            FillDataList(drSql, CaseWorkerSettingList)

            Return CaseWorkerSettingList

        End Function

        ''' <summary>
        ''' Searching Case Worker Info
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <param name="FirstName"></param>
        ''' <param name="LastName"></param>
        ''' <param name="Phone"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SearchCaseWorkerInfo(ConVisitel As SqlConnection, CompanyId As Integer, FirstName As String, LastName As String,
                                             Phone As String) As List(Of CaseWorkerSettingDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()

            parameters.Add("@CompanyId", CompanyId)

            If (Not String.IsNullOrEmpty(FirstName)) Then
                parameters.Add("@FirstName", FirstName)
            End If

            If (Not String.IsNullOrEmpty(LastName)) Then
                parameters.Add("@LastName", LastName)
            End If

            If (Not String.IsNullOrEmpty(Phone)) Then
                parameters.Add("@Phone", Phone)
            End If

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SearchCaseWorkerInfo]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim CaseWorkerSettingList As New List(Of CaseWorkerSettingDataObject)()

            FillDataList(drSql, CaseWorkerSettingList)

            Return CaseWorkerSettingList

        End Function

        Private Sub FillDataList(ByRef drSql As SqlDataReader, ByRef CaseWorkerSettingList As List(Of CaseWorkerSettingDataObject))

            If drSql.HasRows Then
                Dim objCaseWorkerSettingDataObject As CaseWorkerSettingDataObject
                While drSql.Read()
                    objCaseWorkerSettingDataObject = New CaseWorkerSettingDataObject()

                    objCaseWorkerSettingDataObject.CaseWorkerId = If((Not DBNull.Value.Equals(drSql("CwId"))),
                                                                     Convert.ToInt32(drSql("CwId"), Nothing),
                                                                     objCaseWorkerSettingDataObject.CaseWorkerId)

                    objCaseWorkerSettingDataObject.FirstName = Convert.ToString(drSql("FirstName"), Nothing)
                    objCaseWorkerSettingDataObject.LastName = Convert.ToString(drSql("LastName"), Nothing)
                    objCaseWorkerSettingDataObject.Phone = Convert.ToString(drSql("Phone"), Nothing)
                    objCaseWorkerSettingDataObject.Fax = Convert.ToString(drSql("Fax"), Nothing)

                    objCaseWorkerSettingDataObject.Status = If((Not DBNull.Value.Equals(drSql("Status"))),
                                                                     Convert.ToInt16(drSql("Status"), Nothing),
                                                                     objCaseWorkerSettingDataObject.Status)

                    objCaseWorkerSettingDataObject.MailCode = Convert.ToString(drSql("Mailcode"), Nothing)

                    objCaseWorkerSettingDataObject.CityId = If((Not DBNull.Value.Equals(drSql("CityId"))),
                                                                     Convert.ToInt32(drSql("CityId"), Nothing),
                                                                     objCaseWorkerSettingDataObject.CityId)

                    objCaseWorkerSettingDataObject.CityName = Convert.ToString(drSql("CityName"), Nothing)
                    objCaseWorkerSettingDataObject.Email = Convert.ToString(drSql("Email"), Nothing)
                    objCaseWorkerSettingDataObject.Address = Convert.ToString(drSql("Address"), Nothing)
                    objCaseWorkerSettingDataObject.Comments = Convert.ToString(drSql("Comments"), Nothing)
                    objCaseWorkerSettingDataObject.UpdateDate = Convert.ToString(drSql("UpdateDate"), Nothing)
                    objCaseWorkerSettingDataObject.UpdateBy = Convert.ToString(drSql("UpdateBy"), Nothing)

                    CaseWorkerSettingList.Add(objCaseWorkerSettingDataObject)

                End While
                objCaseWorkerSettingDataObject = Nothing
            End If

            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

        End Sub

    End Class
End Namespace
