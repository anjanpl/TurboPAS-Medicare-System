#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Pay Period Information Data Updating, Fetching & Populating Care Summary
' Author: Anjan Kumar Paul
' Start Date: 25 Dec 2014
' End Date: 
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                25 Dec 2014     Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelDA.VisitelDA
Imports System.Web.UI.WebControls

Namespace VisitelBusiness
    Public Class BLPayPeriodDetail
        Inherits BLCommon

        Public Function SelectPayPeriod(ConVisitel As SqlConnection, ClientId As Int64, ScheduleId As Int64, ServiceDate As String) As List(Of PayPeriodDetailDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()

            If (Not ClientId.Equals(-1)) Then
                parameters.Add("@ClientId", ClientId)
            End If

            If (Not ScheduleId.Equals(-1)) Then
                parameters.Add("@ScheduleId", ScheduleId)
            End If

            If (Not ServiceDate.Equals("-1")) Then
                parameters.Add("@ServiceDate", ServiceDate)
            End If

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectPayPeriodDetail]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing

            parameters = Nothing


            Dim PayPeriodDetailList As New List(Of PayPeriodDetailDataObject)

            Dim HourMinute As String = String.Empty

            If (drSql.HasRows) Then
                Dim objPayPeriodDetailDataObject As PayPeriodDetailDataObject
                While drSql.Read()

                    objPayPeriodDetailDataObject = New PayPeriodDetailDataObject()

                    objPayPeriodDetailDataObject.PayPeriodId = If((Not DBNull.Value.Equals(drSql("PayPeriodId"))),
                                                                     Convert.ToInt32(drSql("PayPeriodId"), Nothing),
                                                                     objPayPeriodDetailDataObject.PayPeriodId)

                    objPayPeriodDetailDataObject.StartDate = Convert.ToString(drSql("StartDate"), Nothing)
                    objPayPeriodDetailDataObject.EndDate = Convert.ToString(drSql("EndDate"), Nothing)

                    objPayPeriodDetailDataObject.CalendarId = If((Not DBNull.Value.Equals(drSql("CalendarId"))),
                                                                    Convert.ToInt32(drSql("CalendarId"), Nothing),
                                                                    objPayPeriodDetailDataObject.CalendarId)

                    objPayPeriodDetailDataObject.IndividualId = If((Not DBNull.Value.Equals(drSql("IndividualId"))),
                                                                    Convert.ToInt64(drSql("IndividualId"), Nothing),
                                                                    objPayPeriodDetailDataObject.IndividualId)

                    objPayPeriodDetailDataObject.IndividualName = Convert.ToString(drSql("IndividualName"), Nothing)

                    objPayPeriodDetailDataObject.AttendantId = If((Not DBNull.Value.Equals(drSql("AttendantId"))),
                                                                    Convert.ToInt64(drSql("AttendantId"), Nothing),
                                                                    objPayPeriodDetailDataObject.AttendantId)

                    objPayPeriodDetailDataObject.AttendantName = Convert.ToString(drSql("AttendantName"), Nothing)
                    objPayPeriodDetailDataObject.ServiceDate = Convert.ToString(drSql("ServiceDate"), Nothing)
                    objPayPeriodDetailDataObject.DayName = Convert.ToString(drSql("DayName"), Nothing)

                    HourMinute = Convert.ToString(drSql("HourMinute"), Nothing)
                    objPayPeriodDetailDataObject.HourMinutes = If((String.IsNullOrEmpty(HourMinute)), "00:00", HourMinute.Split(":")(0) & ":" & HourMinute.Split(":")(1))
                    objPayPeriodDetailDataObject.InTime = Convert.ToString(drSql("InTime"), Nothing)
                    objPayPeriodDetailDataObject.OutTime = Convert.ToString(drSql("OutTime"), Nothing)

                    objPayPeriodDetailDataObject.SpecialRate = If((Not DBNull.Value.Equals(drSql("SpecialRate"))),
                                                                     Convert.ToDecimal(drSql("SpecialRate"), Nothing),
                                                                     objPayPeriodDetailDataObject.SpecialRate)

                    objPayPeriodDetailDataObject.Comments = Convert.ToString(drSql("Comments"), Nothing)
                    objPayPeriodDetailDataObject.UpdateDate = Convert.ToString(drSql("UpdateDate"), Nothing)
                    objPayPeriodDetailDataObject.UpdateBy = Convert.ToString(drSql("UpdateBy"), Nothing)

                    PayPeriodDetailList.Add(objPayPeriodDetailDataObject)

                End While
                objPayPeriodDetailDataObject = Nothing
            End If

            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return PayPeriodDetailList

        End Function

        Public Sub UpdatePayPeriod(ConVisitel As SqlConnection, objPayPeriodDetailDataObject As PayPeriodDetailDataObject)
            Dim parameters As New HybridDictionary()

            parameters.Add("@PayPeriodId", objPayPeriodDetailDataObject.PayPeriodId)

            SetParameters(parameters, objPayPeriodDetailDataObject)

            parameters.Add("@UpdateBy", objPayPeriodDetailDataObject.UpdateBy)
            parameters.Add("@CompanyId", objPayPeriodDetailDataObject.CompanyId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdatePayPeriodDetail]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Private Sub SetParameters(parameters As HybridDictionary, ByRef objPayPeriodDetailDataObject As PayPeriodDetailDataObject)

            If (Not objPayPeriodDetailDataObject.AttendantId.Equals(-1)) Then
                parameters.Add("@AttendantId", objPayPeriodDetailDataObject.AttendantId)
            End If

            If Not String.IsNullOrEmpty(objPayPeriodDetailDataObject.HourMinutes) Then
                parameters.Add("@HourMinutes", New TimeSpan(objPayPeriodDetailDataObject.HourMinutes.Split(":")(0),
                                                                 objPayPeriodDetailDataObject.HourMinutes.Split(":")(1), 0))
            End If

            If Not String.IsNullOrEmpty(objPayPeriodDetailDataObject.InTime) Then
                parameters.Add("@InTime", New TimeSpan(objPayPeriodDetailDataObject.InTime.Split(":")(0),
                                                             objPayPeriodDetailDataObject.InTime.Split(":")(1), 0))
            End If

            If Not String.IsNullOrEmpty(objPayPeriodDetailDataObject.OutTime) Then
                parameters.Add("@OutTime", New TimeSpan(objPayPeriodDetailDataObject.OutTime.Split(":")(0),
                                                             objPayPeriodDetailDataObject.OutTime.Split(":")(1), 0))
            End If

            'If Not String.IsNullOrEmpty(objPayPeriodDetailDataObject.SpecialRate.Replace("$", "")) Then
            'parameters.Add("@SpecialRate", Convert.ToDecimal(objPayPeriodDetailDataObject.SpecialRate.Replace("$", ""), Nothing))
            'End If

            If (Not objPayPeriodDetailDataObject.SpecialRate.Equals(Decimal.MinValue)) Then
                parameters.Add("@SpecialRate", objPayPeriodDetailDataObject.SpecialRate)
            End If

        End Sub

        Public Sub SelectCareSummaryPayPeriod(VisitelConnectionString As String, ByRef SqlDataSourceCareSummaryPayPeriod As SqlDataSource,
                                              ByRef QueryStringCollection As NameValueCollection)

            SqlDataSourceCareSummaryPayPeriod.ProviderName = "System.Data.SqlClient"
            SqlDataSourceCareSummaryPayPeriod.ConnectionString = VisitelConnectionString

            SqlDataSourceCareSummaryPayPeriod.SelectCommandType = SqlDataSourceCommandType.StoredProcedure

            Dim StartDate As String = String.Empty

            If (Not QueryStringCollection("StartDate") Is Nothing) Then
                StartDate = Convert.ToString(QueryStringCollection("StartDate"), Nothing)
                StartDate = GetQueryStringFormattedDate(StartDate)
            End If

            Dim EndDate As String = String.Empty
            If (Not QueryStringCollection("EndDate") Is Nothing) Then
                EndDate = Convert.ToString(QueryStringCollection("EndDate"), Nothing)
                EndDate = GetQueryStringFormattedDate(EndDate)
            End If

            SqlDataSourceCareSummaryPayPeriod.SelectParameters.Clear()
            SqlDataSourceCareSummaryPayPeriod.SelectParameters.Add("StartDate", StartDate)
            SqlDataSourceCareSummaryPayPeriod.SelectParameters.Add("EndDate", EndDate)
            SqlDataSourceCareSummaryPayPeriod.SelectParameters.Add("ClientId", Convert.ToInt64(QueryStringCollection("ClientId")))

            If (Not Convert.ToInt64(QueryStringCollection("AttendantId")).Equals(Int64.MinValue)) Then
                SqlDataSourceCareSummaryPayPeriod.SelectParameters.Add("AttendantId", Convert.ToInt64(QueryStringCollection("AttendantId")))
            End If

            SqlDataSourceCareSummaryPayPeriod.SelectParameters.Add("ScheduleId", Convert.ToInt64(QueryStringCollection("ScheduleId")))

            SqlDataSourceCareSummaryPayPeriod.SelectCommand = "[TurboDB.SelectCareSummaryPayPeriodDetail]"
            SqlDataSourceCareSummaryPayPeriod.DataBind()


        End Sub

        Public Sub GetCalendarIdData(VisitelConnectionString As String, ByRef SqlDataSourceCalendarId As SqlDataSource)
            SqlDataSourceCalendarId.ProviderName = "System.Data.SqlClient"
            SqlDataSourceCalendarId.ConnectionString = VisitelConnectionString
            SqlDataSourceCalendarId.SelectCommandType = SqlDataSourceCommandType.StoredProcedure
            SqlDataSourceCalendarId.SelectCommand = "[TurboDB.SelectCalendarId]"
            SqlDataSourceCalendarId.DataBind()
        End Sub

        Public Sub GetPayPeriodData(VisitelConnectionString As String, ByRef SqlDataSourcePayPeriod As SqlDataSource, PayPeriodFor As String)
            SqlDataSourcePayPeriod.ProviderName = "System.Data.SqlClient"
            SqlDataSourcePayPeriod.ConnectionString = VisitelConnectionString
            SqlDataSourcePayPeriod.SelectCommandType = SqlDataSourceCommandType.StoredProcedure

            SqlDataSourcePayPeriod.SelectParameters.Clear()
            SqlDataSourcePayPeriod.SelectParameters.Add("PayPeriodFor", PayPeriodFor)

            SqlDataSourcePayPeriod.SelectCommand = "[TurboDB.SelectAllPayPeriods]"
            SqlDataSourcePayPeriod.DataBind()
        End Sub

        Public Function GetPayPeriodData(ConVisitel As SqlConnection, PayPeriodFor As String) As List(Of PayPeriodDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()
            parameters.Add("@PayPeriodFor", PayPeriodFor)

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectAllPayPeriods]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim PayPeriodList As New List(Of PayPeriodDataObject)
            Dim objPayPeriodDataObject As PayPeriodDataObject

            If drSql.HasRows Then

                While drSql.Read()
                    objPayPeriodDataObject = New PayPeriodDataObject()

                    objPayPeriodDataObject.StartDate = Convert.ToString(drSql("StartDate"), Nothing)
                    objPayPeriodDataObject.EndDate = Convert.ToString(drSql("EndDate"), Nothing)

                    PayPeriodList.Add(objPayPeriodDataObject)

                End While
                objPayPeriodDataObject = Nothing
            Else
                objPayPeriodDataObject = New PayPeriodDataObject()
                PayPeriodList.Add(objPayPeriodDataObject)
                objPayPeriodDataObject = Nothing
            End If

            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return PayPeriodList

        End Function

        Public Sub GetServiceDateData(VisitelConnectionString As String, ByRef SqlDataSourceServiceDate As SqlDataSource)
            SqlDataSourceServiceDate.ProviderName = "System.Data.SqlClient"
            SqlDataSourceServiceDate.ConnectionString = VisitelConnectionString
            SqlDataSourceServiceDate.SelectCommandType = SqlDataSourceCommandType.StoredProcedure
            SqlDataSourceServiceDate.SelectCommand = "[TurboDB.SelectServiceDate]"
            SqlDataSourceServiceDate.DataBind()
        End Sub

        Public Sub PopulateCareSummary(ConVisitel As SqlConnection, objPayPeriodDetailDataObject As PayPeriodDetailDataObject, blLoneClientExport As Int16,
                                       blLoneCalendarExport As Int16)

            Dim parameters As New HybridDictionary()

            parameters.Add("@StartDate", objPayPeriodDetailDataObject.StartDate)
            parameters.Add("@EndDate", objPayPeriodDetailDataObject.EndDate)
            parameters.Add("@ClientId", objPayPeriodDetailDataObject.IndividualId)
            parameters.Add("@ScheduleId", objPayPeriodDetailDataObject.CalendarId)

            parameters.Add("@blLoneClientExport", blLoneClientExport)
            parameters.Add("@blLoneCalendarExport", blLoneCalendarExport)

            parameters.Add("@UpdateBy", objPayPeriodDetailDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.PopulateCareSummary]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

    End Class

    Public Class PayPeriodDataObject

        Private m_StartDate As String
        Public Property StartDate() As String
            Get
                Return m_StartDate
            End Get
            Set(value As String)
                m_StartDate = value
            End Set
        End Property

        Private m_EndDate As String
        Public Property EndDate() As String
            Get
                Return m_EndDate
            End Get
            Set(value As String)
                m_EndDate = value
            End Set
        End Property

        Public Sub New()

            Me.StartDate = String.Empty
            Me.EndDate = String.Empty

        End Sub

    End Class
End Namespace

