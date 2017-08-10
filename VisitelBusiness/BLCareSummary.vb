
#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Care Summary Data Inserting, Upadating, Fetching, & Searching
' Author: Anjan Kumar Paul
' Start Date: 09 Jan 2015
' End Date: 
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                09 Jan 2015     Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelDA.VisitelDA
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports System.Web.UI.WebControls


Namespace VisitelBusiness
    Public Class BLCareSummary
        Inherits BLCommon

        Public Sub InsertCareSummaryInfo(ConVisitel As SqlConnection, CareSummaryId As Int64, UserId As Int32)

            Dim parameters As New HybridDictionary()

            parameters.Add("@CareSummaryId", CareSummaryId)
            parameters.Add("@UserId", UserId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertCareSummary]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub UpdateCareSummaryInfo(ConVisitel As SqlConnection, objCareSummaryDataObject As CareSummaryDataObject, Optional OriginalTimeSheetAll As Boolean = False)

            Dim parameters As New HybridDictionary()

            parameters.Add("@CareSummaryId", objCareSummaryDataObject.CareSummaryId)
            parameters.Add("@OriginalTimeSheetAll", If((OriginalTimeSheetAll), 1, 0))

            SetParameters(parameters, objCareSummaryDataObject)

            parameters.Add("@UpdateBy", objCareSummaryDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateCareSummary]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub UpdateCareSummaryInfo(ConVisitel As SqlConnection, UpdateBy As String, SaveLocation As String)

            Dim parameters As New HybridDictionary()

            parameters.Add("@CareSummaryId", -1)
            parameters.Add("@OriginalTimeSheetAll", 1)
            parameters.Add("@OriginalTimeSheet", 1)
            parameters.Add("@Billed", 1)
            parameters.Add("@SaveLocation", SaveLocation)

            parameters.Add("@UpdateBy", UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateCareSummary]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub DeleteCareSummaryInfo(ConVisitel As SqlConnection, CareSummaryId As Int64, DeletedBy As Int64, CompanyId As Int32, Description As String)

            Dim parameters As New HybridDictionary()

            parameters.Add("@CareSummaryId", CareSummaryId)
            parameters.Add("@CompanyId", CompanyId)
            parameters.Add("@DeletedBy", DeletedBy)
            parameters.Add("@Description", Description)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteCareSummaryInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        Public Function SelectCareSummaryInfo(ConVisitel As SqlConnection, CompanyId As Int32, ClientId As Int64, ClientType As Int64, PayPeriodStartDate As String,
                                              PayPeriodEndDate As String, OriginalTimeSheet As Int16, Billed As Int16) As List(Of CareSummaryDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()

            If (Not ClientId.Equals(-1)) Then
                parameters.Add("@ClientId", ClientId)
            End If

            If (Not ClientType.Equals(-1)) Then
                parameters.Add("@ClientType", ClientType)
            End If

            If (Not String.IsNullOrEmpty(PayPeriodStartDate)) Then
                parameters.Add("@PayPeriodStartDate", PayPeriodStartDate)
            End If

            If (Not String.IsNullOrEmpty(PayPeriodEndDate)) Then
                parameters.Add("@PayPeriodEndDate", PayPeriodEndDate)
            End If

            If (Not (OriginalTimeSheet = -1)) Then
                parameters.Add("@OriginalTimeSheet", OriginalTimeSheet)
            End If

            If (Not (Billed = -1)) Then
                parameters.Add("@Billed", Billed)
            End If

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectCareSummaryInfo]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim CareSummaryList As New List(Of CareSummaryDataObject)

            FillDataList(drSql, CareSummaryList)

            Return CareSummaryList

        End Function

        Private Sub SetParameters(parameters As HybridDictionary, objCareSummaryDataObject As CareSummaryDataObject)

            If (Not objCareSummaryDataObject.ClientId.Equals(Int64.MinValue)) Then
                parameters.Add("@ClientId", objCareSummaryDataObject.ClientId)
            End If

            If (Not objCareSummaryDataObject.ClientType.Equals(Int64.MinValue)) Then
                parameters.Add("@ClientType", objCareSummaryDataObject.ClientType)
            End If

            If (Not String.IsNullOrEmpty(objCareSummaryDataObject.StartDate)) Then
                parameters.Add("@PayPeriodStartDate", objCareSummaryDataObject.StartDate)
            End If

            If (Not String.IsNullOrEmpty(objCareSummaryDataObject.EndDate)) Then
                parameters.Add("@PayPeriodEndDate", objCareSummaryDataObject.EndDate)
            End If

            If (Not objCareSummaryDataObject.OriginalTimesheet.Equals(Int16.MinValue)) Then
                parameters.Add("@OriginalTimeSheet", objCareSummaryDataObject.OriginalTimesheet)
            End If

            If (Not objCareSummaryDataObject.OriginalTimesheet.Equals(Int16.MinValue)) Then
                parameters.Add("@Billed", objCareSummaryDataObject.Billed)
            End If

            If Not String.IsNullOrEmpty(objCareSummaryDataObject.TimesheetHourMinute) Then
                parameters.Add("@TimesheetHourMinute", objCareSummaryDataObject.TimesheetHourMinute)
            End If

            If (Not objCareSummaryDataObject.AttendantId.Equals(-1)) Then
                parameters.Add("@AttendantId", objCareSummaryDataObject.AttendantId)
            End If

        End Sub

        Private Sub FillDataList(ByRef drSql As SqlDataReader, ByRef CareSummaryList As List(Of CareSummaryDataObject))

            If (drSql.HasRows) Then
                Dim objCareSummaryDataObject As CareSummaryDataObject
                While drSql.Read()

                    objCareSummaryDataObject = New CareSummaryDataObject()

                    objCareSummaryDataObject.CareSummaryId = If((Not DBNull.Value.Equals(drSql("CareSummaryId"))),
                                                                     Convert.ToInt64(drSql("CareSummaryId"), Nothing),
                                                                     objCareSummaryDataObject.CareSummaryId)

                    objCareSummaryDataObject.StartDate = Convert.ToString(drSql("StartDate"), Nothing)
                    objCareSummaryDataObject.EndDate = Convert.ToString(drSql("EndDate"), Nothing)

                    objCareSummaryDataObject.ClientId = If((Not DBNull.Value.Equals(drSql("ClientId"))),
                                                                     Convert.ToInt64(drSql("ClientId"), Nothing),
                                                                     objCareSummaryDataObject.ClientId)

                    objCareSummaryDataObject.ClientType = If((Not DBNull.Value.Equals(drSql("ClientType"))),
                                                                Convert.ToInt64(drSql("ClientType"), Nothing),
                                                                objCareSummaryDataObject.ClientType)

                    objCareSummaryDataObject.ClientName = Convert.ToString(drSql("ClientName"), Nothing)

                    objCareSummaryDataObject.ClientTypeName = Convert.ToString(drSql("ClientTypeName"), Nothing)

                    objCareSummaryDataObject.AttendantId = If((Not DBNull.Value.Equals(drSql("AttendantId"))),
                                                                     Convert.ToInt64(drSql("AttendantId"), Nothing),
                                                                     objCareSummaryDataObject.AttendantId)

                    objCareSummaryDataObject.AttendantName = Convert.ToString(drSql("AttendantName"), Nothing)

                    objCareSummaryDataObject.Priority = Convert.ToString(drSql("Priority"), Nothing)

                    objCareSummaryDataObject.SpecialRate = If((Not DBNull.Value.Equals(drSql("SpecialRate"))),
                                                                     Convert.ToDecimal(drSql("SpecialRate"), Nothing),
                                                                     objCareSummaryDataObject.SpecialRate)

                    objCareSummaryDataObject.OriginalTimesheet = If((Not DBNull.Value.Equals(drSql("OriginalTimesheet"))),
                                                                     Convert.ToInt16(drSql("OriginalTimesheet"), Nothing),
                                                                     objCareSummaryDataObject.OriginalTimesheet)

                    objCareSummaryDataObject.AssignedMinutes = If((Not DBNull.Value.Equals(drSql("AssignedMinutes"))),
                                                                     Convert.ToInt64(drSql("AssignedMinutes"), Nothing),
                                                                     objCareSummaryDataObject.AssignedMinutes)

                    objCareSummaryDataObject.BillTime = Convert.ToString(drSql("ActualMinutes"), Nothing)

                    objCareSummaryDataObject.TimesheetHourMinute = Convert.ToString(drSql("TimesheetHourMinute"), Nothing)

                    objCareSummaryDataObject.AdjustedBillTime = Convert.ToString(drSql("AdjustedMinutes"), Nothing)

                    objCareSummaryDataObject.CalendarId = If((Not DBNull.Value.Equals(drSql("CalendarID"))),
                                                                     Convert.ToInt64(drSql("CalendarID"), Nothing),
                                                                     objCareSummaryDataObject.CalendarId)

                    objCareSummaryDataObject.EDIUpdateDate = Convert.ToString(drSql("EDIUpdateDate"), Nothing)
                    objCareSummaryDataObject.EDIUpdateBy = Convert.ToString(drSql("EDIUpdateBy"), Nothing)
                    objCareSummaryDataObject.UpdateDate = Convert.ToString(drSql("UpdateDate"), Nothing)
                    objCareSummaryDataObject.UpdateBy = Convert.ToString(drSql("UpdateBy"), Nothing)

                    CareSummaryList.Add(objCareSummaryDataObject)

                End While
                objCareSummaryDataObject = Nothing
            End If

            drSql.Close()
            drSql.Dispose()
            drSql = Nothing
        End Sub

        Public Function GetCareSummaryReportData(VisitelConnectionString As String, QueryStringCollection As NameValueCollection) As SqlDataSource

            Dim SqlDataSourceCrystalReport As New SqlDataSource()

            SqlDataSourceCrystalReport.ProviderName = "System.Data.SqlClient"
            SqlDataSourceCrystalReport.ConnectionString = VisitelConnectionString

            SqlDataSourceCrystalReport.SelectParameters.Add("CompanyId", QueryStringCollection("CompanyId"))
            SqlDataSourceCrystalReport.SelectParameters.Add("StartDate", QueryStringCollection("StartDate"))
            SqlDataSourceCrystalReport.SelectParameters.Add("EndDate", QueryStringCollection("EndDate"))

            If (Not QueryStringCollection("IndividualId").Equals("-1") And Not String.IsNullOrEmpty(QueryStringCollection("IndividualId"))) Then
                SqlDataSourceCrystalReport.SelectParameters.Add("ClientId", QueryStringCollection("IndividualId"))
            End If

            If (Not QueryStringCollection("EmployeeId").Equals("-1") And Not String.IsNullOrEmpty(QueryStringCollection("EmployeeId"))) Then
                SqlDataSourceCrystalReport.SelectParameters.Add("AttendantId", QueryStringCollection("EmployeeId"))
            End If

            If (Not QueryStringCollection("IndividualTypeId").Equals("-1") And Not String.IsNullOrEmpty(QueryStringCollection("IndividualTypeId"))) Then
                SqlDataSourceCrystalReport.SelectParameters.Add("IndividualType", QueryStringCollection("IndividualTypeId"))
            End If

            If (Not String.IsNullOrEmpty(QueryStringCollection("PriorityId").Trim())) Then
                SqlDataSourceCrystalReport.SelectParameters.Add("Priority", QueryStringCollection("PriorityId"))
            End If

            If (Not QueryStringCollection("OriginalTimesheetFilterId").Equals("-1") And Not String.IsNullOrEmpty(QueryStringCollection("OriginalTimesheetFilterId").Trim())) Then
                SqlDataSourceCrystalReport.SelectParameters.Add("OriginalTimesheetId", QueryStringCollection("OriginalTimesheetFilterId"))
            End If

            SqlDataSourceCrystalReport.SelectCommand = "[TurboDB.SelectCareSummaryReport]"

            Return SqlDataSourceCrystalReport

        End Function

        Public Sub GetCareSummaryReportExportData(ConVisitel As SqlConnection, ByRef ds As DataSet, CompanyId As Int64, StartDate As String, EndDate As String)

            Dim parameters As New HybridDictionary()

            parameters.Add("@CompanyId", CompanyId)
            parameters.Add("@StartDate", StartDate)
            parameters.Add("@EndDate", EndDate)

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.ExecuteDataSet("", "[TurboDB.SelectCareSummaryReportExport]", ds, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

    End Class
End Namespace

