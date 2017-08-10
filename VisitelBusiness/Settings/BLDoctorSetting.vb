#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Doctor Information Inserting, Updating, Deleting, Filtering & Fetching
' Author: Anjan Kumar Paul
' Start Date: 15 Aug 2014
' End Date: 15 Aug 2014
' History:
'      Version                  Author                      Date             Reason 
'      1.0.0                                                15 Aug 2014      Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.DataObject.Settings
Imports VisitelDA.VisitelDA

Namespace VisitelBusiness.Settings
    Public Class BLDoctorSetting
        Public Sub InsertDoctorInfo(ConVisitel As SqlConnection, objDoctorSettingDataObject As DoctorSettingDataObject)
            Dim parameters As New HybridDictionary()
            parameters.Add("@UpinNo", objDoctorSettingDataObject.UPinNumber)
            parameters.Add("@LicNo", objDoctorSettingDataObject.LicNumber)
            parameters.Add("@FirstName", objDoctorSettingDataObject.FirstName)
            parameters.Add("@LastName", objDoctorSettingDataObject.LastName)
            parameters.Add("@Address", objDoctorSettingDataObject.Address)
            parameters.Add("@Suite", objDoctorSettingDataObject.Suite)
            parameters.Add("@CityId", objDoctorSettingDataObject.CityId)
            parameters.Add("@StateId", objDoctorSettingDataObject.StateId)
            parameters.Add("@Zip", objDoctorSettingDataObject.Zip)
            parameters.Add("@Phone", objDoctorSettingDataObject.Phone)
            parameters.Add("@Fax", objDoctorSettingDataObject.Fax)
            parameters.Add("@Status", objDoctorSettingDataObject.Status)
            parameters.Add("@CompanyId", objDoctorSettingDataObject.CompanyId)
            parameters.Add("@UserId", objDoctorSettingDataObject.UserId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertDoctorInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        Public Sub UpdateDoctorInfo(ConVisitel As SqlConnection, objDoctorSettingDataObject As DoctorSettingDataObject)
            Dim parameters As New HybridDictionary()
            parameters.Add("@DoctorId", objDoctorSettingDataObject.DoctorId)
            parameters.Add("@UpinNo", objDoctorSettingDataObject.UPinNumber)
            parameters.Add("@LicNo", objDoctorSettingDataObject.LicNumber)
            parameters.Add("@FirstName", objDoctorSettingDataObject.FirstName)
            parameters.Add("@LastName", objDoctorSettingDataObject.LastName)
            parameters.Add("@Address", objDoctorSettingDataObject.Address)
            parameters.Add("@Suite", objDoctorSettingDataObject.Suite)
            parameters.Add("@CityId", objDoctorSettingDataObject.CityId)
            parameters.Add("@StateId", objDoctorSettingDataObject.StateId)
            parameters.Add("@Zip", objDoctorSettingDataObject.Zip)
            parameters.Add("@Phone", objDoctorSettingDataObject.Phone)
            parameters.Add("@Fax", objDoctorSettingDataObject.Fax)
            parameters.Add("@Status", objDoctorSettingDataObject.Status)
            parameters.Add("@UpdateBy", objDoctorSettingDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateDoctorInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        Public Sub DeleteDoctorInfo(ConVisitel As SqlConnection, DoctorId As Integer, DeletedBy As Integer)
            Dim parameters As New HybridDictionary()
            parameters.Add("@DoctorId", DoctorId)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteDoctorInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        ''' <summary>
        ''' Retrieving Doctor Info
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SelectDoctorInfo(ConVisitel As SqlConnection, CompanyId As Integer) As List(Of DoctorSettingDataObject)

            Dim drSql As SqlDataReader = Nothing
            Dim parameters As New HybridDictionary()

            parameters.Add("@CompanyId", CompanyId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SelectDoctorInfo]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim DoctorSettingList As New List(Of DoctorSettingDataObject)()

            FillDataList(drSql, DoctorSettingList)

            Return DoctorSettingList

        End Function

        ''' <summary>
        ''' Searching Doctor Info
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <param name="UPinNumber"></param>
        ''' <param name="LicNumber"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SearchDoctorInfo(ConVisitel As SqlConnection, CompanyId As Integer, UPinNumber As String, LicNumber As String) As List(Of DoctorSettingDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()

            parameters.Add("@CompanyId", CompanyId)

            If (Not String.IsNullOrEmpty(UPinNumber)) Then
                parameters.Add("@UpinNo", UPinNumber)
            End If

            If (Not String.IsNullOrEmpty(LicNumber)) Then
                parameters.Add("@LicNo", LicNumber)
            End If

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SearchDoctorInfo]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim DoctorSettingList As New List(Of DoctorSettingDataObject)()

            FillDataList(drSql, DoctorSettingList)

            Return DoctorSettingList

        End Function

        Private Sub FillDataList(ByRef drSql As SqlDataReader, ByRef DoctorSettingList As List(Of DoctorSettingDataObject))

            If drSql.HasRows Then
                Dim objDoctorSettingDataObject As DoctorSettingDataObject
                While drSql.Read()
                    objDoctorSettingDataObject = New DoctorSettingDataObject()

                    objDoctorSettingDataObject.DoctorId = If((Not DBNull.Value.Equals(drSql("DoctorId"))),
                                                                     Convert.ToInt32(drSql("DoctorId"), Nothing),
                                                                     objDoctorSettingDataObject.DoctorId)


                    objDoctorSettingDataObject.UPinNumber = Convert.ToString(drSql("UpinNo"), Nothing)
                    objDoctorSettingDataObject.LicNumber = Convert.ToString(drSql("LicNo"), Nothing)
                    objDoctorSettingDataObject.FirstName = Convert.ToString(drSql("FirstName"), Nothing)
                    objDoctorSettingDataObject.LastName = Convert.ToString(drSql("LastName"), Nothing)
                    objDoctorSettingDataObject.Address = Convert.ToString(drSql("Address"), Nothing)
                    objDoctorSettingDataObject.Suite = Convert.ToString(drSql("Suite"), Nothing)

                    objDoctorSettingDataObject.CityId = If((Not DBNull.Value.Equals(drSql("CityId"))),
                                                                     Convert.ToInt32(drSql("CityId"), Nothing),
                                                                     objDoctorSettingDataObject.CityId)

                    objDoctorSettingDataObject.CityName = Convert.ToString(drSql("CityName"), Nothing)
                    objDoctorSettingDataObject.StateId = Convert.ToString(drSql("StateId"), Nothing)
                    objDoctorSettingDataObject.StateFullName = Convert.ToString(drSql("StateFullName"), Nothing)
                    objDoctorSettingDataObject.Zip = Convert.ToString(drSql("Zip"), Nothing)
                    objDoctorSettingDataObject.Phone = Convert.ToString(drSql("Phone"), Nothing)
                    objDoctorSettingDataObject.Fax = Convert.ToString(drSql("Fax"), Nothing)

                    objDoctorSettingDataObject.Status = If((Not DBNull.Value.Equals(drSql("Status"))),
                                                              Convert.ToInt16(drSql("Status"), Nothing),
                                                              objDoctorSettingDataObject.Status)

                    objDoctorSettingDataObject.UpdateBy = Convert.ToString(drSql("UpdateBy"), Nothing)
                    objDoctorSettingDataObject.UpdateDate = Convert.ToString(drSql("UpdateDate"), Nothing)

                    DoctorSettingList.Add(objDoctorSettingDataObject)
                End While
                objDoctorSettingDataObject = Nothing
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing
        End Sub
    End Class
End Namespace

