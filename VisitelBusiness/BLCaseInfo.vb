
#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Case Information Data Inserting, Upadating, Fetching & Fetching Report Data
' Author: Anjan Kumar Paul
' Start Date: 29 Nov 2014
' End Date: 
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                29 Nov 2014     Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelDA.VisitelDA
Imports System.Web.UI.WebControls

Namespace VisitelBusiness
    Public Class BLCaseInfo

        Public Sub InsertCaseInfo(ConVisitel As SqlConnection, objCaseInfoDataObject As CaseInfoDataObject)

            Dim parameters As New HybridDictionary()

            SetParameters(parameters, objCaseInfoDataObject)

            parameters.Add("@CompanyId", objCaseInfoDataObject.CompanyId)
            parameters.Add("@UserId", objCaseInfoDataObject.UserId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertCaseInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub UpdateCaseInfo(ConVisitel As SqlConnection, objCaseInfoDataObject As CaseInfoDataObject)

            Dim parameters As New HybridDictionary()

            parameters.Add("@Id", objCaseInfoDataObject.Id)

            SetParameters(parameters, objCaseInfoDataObject)

            parameters.Add("@UpdateBy", objCaseInfoDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateCaseInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub DeleteCaseInfo(ConVisitel As SqlConnection, CaseInfoId As Integer, DeletedBy As Integer)

            Dim parameters As New HybridDictionary()
            parameters.Add("@Id", CaseInfoId)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteCaseInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        Public Function SelectCaseInfo(ConVisitel As SqlConnection, ClientId As Integer, CompanyId As Integer) As List(Of CaseInfoDataObject)

            Dim drSql As SqlDataReader = Nothing
            Dim parameters As New HybridDictionary()

            parameters.Add("@ClientId", ClientId)
            parameters.Add("@CompanyId", CompanyId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SelectCaseInfo]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim CaseInfoList As New List(Of CaseInfoDataObject)()

            If drSql.HasRows Then
                Dim objCaseInfoDataObject As CaseInfoDataObject
                While drSql.Read()

                    objCaseInfoDataObject = New CaseInfoDataObject()

                    objCaseInfoDataObject.Id = Convert.ToInt32(drSql("Id"), Nothing)

                    objCaseInfoDataObject.ClientId = If((DBNull.Value.Equals(drSql("ClientId"))),
                                                               objCaseInfoDataObject.ClientId,
                                                               Convert.ToInt32(drSql("ClientId")))

                    objCaseInfoDataObject.CaseWorkerId = If((DBNull.Value.Equals(drSql("CaseWorkerId"))),
                                                               objCaseInfoDataObject.CaseWorkerId,
                                                               Convert.ToInt32(drSql("CaseWorkerId")))

                    objCaseInfoDataObject.EmployeeId = If((DBNull.Value.Equals(drSql("EmployeeId"))),
                                                               objCaseInfoDataObject.EmployeeId,
                                                               Convert.ToInt32(drSql("EmployeeId")))

                    objCaseInfoDataObject.CaseInfoDate = Convert.ToString(drSql("Date"), Nothing).Trim()

                    objCaseInfoDataObject.ReceiverOrganization = Convert.ToString(drSql("ReceiverOrganization"), Nothing).Trim()

                    objCaseInfoDataObject.Comments = Convert.ToString(drSql("Comments"), Nothing).Trim()

                    objCaseInfoDataObject.UpdateBy = Convert.ToString(drSql("UpdateBy"), Nothing).Trim()
                    objCaseInfoDataObject.UpdateDate = Convert.ToString(drSql("UpdateDate"), Nothing).Trim()

                    CaseInfoList.Add(objCaseInfoDataObject)
                End While
                objCaseInfoDataObject = Nothing
            End If

            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return CaseInfoList

        End Function

        Private Sub SetParameters(parameters As HybridDictionary, objCaseInfoDataObject As CaseInfoDataObject)

            parameters.Add("@ClientId", objCaseInfoDataObject.ClientId)

            If (Not objCaseInfoDataObject.CaseWorkerId.Equals(-1)) Then
                parameters.Add("@CaseWorkerId", objCaseInfoDataObject.CaseWorkerId)
            End If

            If (Not objCaseInfoDataObject.EmployeeId.Equals(-1)) Then
                parameters.Add("@EmployeeId", objCaseInfoDataObject.EmployeeId)
            End If

            If (Not String.IsNullOrEmpty(objCaseInfoDataObject.CaseInfoDate)) Then
                parameters.Add("@Date", objCaseInfoDataObject.CaseInfoDate)
            End If

            If (Not String.IsNullOrEmpty(objCaseInfoDataObject.ReceiverOrganization)) Then
                parameters.Add("@ReceiverOrganization", objCaseInfoDataObject.ReceiverOrganization)
            End If

            If (Not String.IsNullOrEmpty(objCaseInfoDataObject.Comments)) Then
                parameters.Add("@Comments", objCaseInfoDataObject.Comments)
            End If

        End Sub

        Public Function GetCaseInformationReportData(VisitelConnectionString As String, QueryStringCollection As NameValueCollection) As SqlDataSource

            Dim SqlDataSourceCrystalReport As New SqlDataSource()

            SqlDataSourceCrystalReport.ProviderName = "System.Data.SqlClient"
            SqlDataSourceCrystalReport.ConnectionString = VisitelConnectionString

            SqlDataSourceCrystalReport.SelectParameters.Add("Id", QueryStringCollection("CaseInfoId"))
            SqlDataSourceCrystalReport.SelectParameters.Add("CompanyId", QueryStringCollection("CompanyId"))

            SqlDataSourceCrystalReport.SelectCommand = "[TurboDB.SelectCaseInfoReport]"

            Return SqlDataSourceCrystalReport

        End Function

        Public Function GetFaxCoverPageReportData(VisitelConnectionString As String, QueryStringCollection As NameValueCollection) As SqlDataSource

            Dim SqlDataSourceCrystalReport As New SqlDataSource()

            SqlDataSourceCrystalReport.ProviderName = "System.Data.SqlClient"
            SqlDataSourceCrystalReport.ConnectionString = VisitelConnectionString

            SqlDataSourceCrystalReport.SelectParameters.Add("Id", QueryStringCollection("CaseInfoId"))
            SqlDataSourceCrystalReport.SelectParameters.Add("CompanyId", QueryStringCollection("CompanyId"))

            SqlDataSourceCrystalReport.SelectCommand = "[TurboDB.SelectFaxCoverPageReport]"

            Return SqlDataSourceCrystalReport

        End Function

    End Class
End Namespace

