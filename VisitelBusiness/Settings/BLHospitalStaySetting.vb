
#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Hospital Stay Information Inserting, Updating, Deleting, & Fetching
' Author: Anjan Kumar Paul
' Start Date: 04 Jan 2015
' End Date: 04 Jan 2015
' History:
'      Version                  Author                      Date             Reason 
'      1.0.0                                                04 Jan 2015      Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.DataObject.Settings
Imports VisitelDA.VisitelDA
Imports System.Web.UI.WebControls

Namespace VisitelBusiness.Settings
    Public Class BLHospitalStaySetting
        Inherits BLCommon

        Public Sub InsertHospitalStayInfo(ConVisitel As SqlConnection, objHospitalStaySettingDataObject As HospitalStaySettingDataObject)

            Dim parameters As New HybridDictionary()

            SetParameters(parameters, objHospitalStaySettingDataObject)

            parameters.Add("@UserId", objHospitalStaySettingDataObject.UserId)
            parameters.Add("@CompanyId", objHospitalStaySettingDataObject.CompanyId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertHospitalStayInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        Public Sub UpdateHospitalStayInfo(ConVisitel As SqlConnection, objHospitalStaySettingDataObject As HospitalStaySettingDataObject)

            Dim parameters As New HybridDictionary()

            parameters.Add("@CareGapId", objHospitalStaySettingDataObject.HospitalStayId)

            SetParameters(parameters, objHospitalStaySettingDataObject)

            parameters.Add("@UpdateBy", objHospitalStaySettingDataObject.UpdateBy)
            parameters.Add("@CompanyId", objHospitalStaySettingDataObject.CompanyId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateHospitalStayInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub DeleteHospitalStayInfo(ConVisitel As SqlConnection, HospitalStayId As Int64, DeletedBy As Int64, CompanyId As Int32)

            Dim parameters As New HybridDictionary()

            parameters.Add("@CareGapId", HospitalStayId)
            parameters.Add("@CompanyId", CompanyId)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteHospitalStayInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        Public Function SelectHospitalStayInfo(ConVisitel As SqlConnection, CompanyId As Int32) As List(Of HospitalStaySettingDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()

            parameters.Add("@CompanyId", CompanyId)


            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectHospitalStayInfo]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim objHospitalStaySettingDataObject As HospitalStaySettingDataObject
            Dim HospitalStayList As New List(Of HospitalStaySettingDataObject)

            If (drSql.HasRows) Then
                While drSql.Read()

                    objHospitalStaySettingDataObject = New HospitalStaySettingDataObject()

                    objHospitalStaySettingDataObject.HospitalStayId = If((Not DBNull.Value.Equals(drSql("Id"))),
                                                                     Convert.ToInt64(drSql("Id"), Nothing),
                                                                     objHospitalStaySettingDataObject.HospitalStayId)

                    objHospitalStaySettingDataObject.IndividualId = If((Not DBNull.Value.Equals(drSql("IndividualId"))),
                                                                     Convert.ToInt64(drSql("IndividualId"), Nothing),
                                                                     objHospitalStaySettingDataObject.IndividualId)

                    objHospitalStaySettingDataObject.IndividualName = Convert.ToString(drSql("IndividualName"), Nothing)
                    objHospitalStaySettingDataObject.StartDate = Convert.ToString(drSql("StartDate"), Nothing)
                    objHospitalStaySettingDataObject.EndDate = Convert.ToString(drSql("EndDate"), Nothing)
                    objHospitalStaySettingDataObject.Reason = Convert.ToString(drSql("Reason"), Nothing)

                    'objHospitalStaySettingDataObject.MedicaidNo = Convert.ToString(drSql("MedicaidNo"), Nothing)
                    'objHospitalStaySettingDataObject.Comment = Convert.ToString(drSql("Comment"), Nothing)

                    objHospitalStaySettingDataObject.UpdateDate = Convert.ToString(drSql("UpdateDate"), Nothing)
                    objHospitalStaySettingDataObject.UpdateBy = Convert.ToString(drSql("UpdateBy"), Nothing)

                    HospitalStayList.Add(objHospitalStaySettingDataObject)

                End While

            End If

            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            objHospitalStaySettingDataObject = Nothing

            Return HospitalStayList

        End Function

        Public Sub SelectHospitalizedIndividuals(VisitelConnectionString As String, ByRef SqlDataSourceHospitalizedIndividuals As SqlDataSource, CompanyId As Int32,
                                     ByRef QueryStringCollection As NameValueCollection)


            SqlDataSourceHospitalizedIndividuals.ProviderName = "System.Data.SqlClient"
            SqlDataSourceHospitalizedIndividuals.ConnectionString = VisitelConnectionString

            SqlDataSourceHospitalizedIndividuals.SelectCommandType = SqlDataSourceCommandType.StoredProcedure

            Dim StartDate As String = String.Empty
            If (Not QueryStringCollection("FromDate") Is Nothing) Then
                StartDate = Convert.ToString(QueryStringCollection("FromDate"), Nothing)
                StartDate = GetQueryStringFormattedDate(StartDate)
            End If

            Dim EndDate As String = String.Empty
            If (Not QueryStringCollection("ToDate") Is Nothing) Then
                EndDate = Convert.ToString(QueryStringCollection("ToDate"), Nothing)
                EndDate = GetQueryStringFormattedDate(EndDate)
            End If

            SqlDataSourceHospitalizedIndividuals.SelectParameters.Clear()
            SqlDataSourceHospitalizedIndividuals.SelectParameters.Add("CompanyId", CompanyId)
            SqlDataSourceHospitalizedIndividuals.SelectParameters.Add("StartDate", StartDate)
            SqlDataSourceHospitalizedIndividuals.SelectParameters.Add("EndDate", EndDate)


            SqlDataSourceHospitalizedIndividuals.SelectCommand = "[TurboDB.SelectHospitalizedIndividuals]"
            SqlDataSourceHospitalizedIndividuals.DataBind()

        End Sub

        Private Sub SetParameters(parameters As HybridDictionary, objHospitalStaySettingDataObject As HospitalStaySettingDataObject)

            parameters.Add("@ClientId", objHospitalStaySettingDataObject.IndividualId)
            parameters.Add("@StartDate", objHospitalStaySettingDataObject.StartDate)
            parameters.Add("@EndDate", objHospitalStaySettingDataObject.EndDate)
            parameters.Add("@Reason", objHospitalStaySettingDataObject.Reason)

        End Sub
    End Class
End Namespace

