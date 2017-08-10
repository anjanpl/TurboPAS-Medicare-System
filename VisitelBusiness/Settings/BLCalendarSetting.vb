#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Calendar Information Inserting, Updating, & Fetching
' Author: Anjan Kumar Paul
' Start Date: 25 Oct 2014
' End Date: 25 Oct 2014
' History:
'      Version                  Author                      Date             Reason 
'      1.0.0                                                25 Oct 2014      Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.DataObject.Settings
Imports VisitelDA.VisitelDA

Namespace VisitelBusiness.Settings
    Public Class BLCalendarSetting

        Public Sub InsertCalendarInfo(ConVisitel As SqlConnection, objCalendarSettingDataObject As CalendarSettingDataObject)

            Dim parameters As New HybridDictionary()

            SetParameters(parameters, objCalendarSettingDataObject)

            parameters.Add("@CompanyId", objCalendarSettingDataObject.CompanyId)
            parameters.Add("@UserId", objCalendarSettingDataObject.UserId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertCalendarInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        Public Sub UpdateCalendarInfo(ConVisitel As SqlConnection, objCalendarSettingDataObject As CalendarSettingDataObject)
            Dim parameters As New HybridDictionary()

            parameters.Add("@ScheduleId", objCalendarSettingDataObject.ScheduleId)

            SetParameters(parameters, objCalendarSettingDataObject)

            parameters.Add("@UpdateBy", objCalendarSettingDataObject.UpdateBy)
            parameters.Add("@CompanyId", objCalendarSettingDataObject.CompanyId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateCalendarInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        ''' <summary>
        ''' Selecting Calendar Schedule Info by Client Id and Client Status
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <param name="ClientId"></param>
        ''' <param name="EmployeeId"></param>
        ''' <param name="Status"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SelectCalendarInfo(ConVisitel As SqlConnection, CompanyId As Integer, ClientId As Integer, EmployeeId As Integer,
                                           Status As String) As List(Of CalendarSettingDataObject)

            Dim drSql As SqlDataReader = Nothing
            Dim parameters As New HybridDictionary()

            parameters.Add("@CompanyId", CompanyId)

            If Not ClientId.Equals(-1) Then
                parameters.Add("@ClientId", ClientId)
            End If

            If Not EmployeeId.Equals(-1) Then
                parameters.Add("@EmployeeId", EmployeeId)
            End If

            If Not Status.Equals("-1") Then
                parameters.Add("@Status", Status)
            End If

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SelectCalendarInfo]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim CalendarSettingList As New List(Of CalendarSettingDataObject)()

            If drSql.HasRows Then
                Dim objCalendarSettingDataObject As CalendarSettingDataObject
                While drSql.Read()

                    objCalendarSettingDataObject = New CalendarSettingDataObject()

                    objCalendarSettingDataObject.ScheduleId = If((DBNull.Value.Equals(drSql("ScheduleId"))),
                                                              objCalendarSettingDataObject.ScheduleId,
                                                              Convert.ToInt32(drSql("ScheduleId")))

                    objCalendarSettingDataObject.ClientId = If((DBNull.Value.Equals(drSql("ClientId"))),
                                                               objCalendarSettingDataObject.ClientId,
                                                               Convert.ToInt32(drSql("ClientId")))

                    objCalendarSettingDataObject.EmployeeId = If((DBNull.Value.Equals(drSql("EmployeeId"))),
                                                               objCalendarSettingDataObject.EmployeeId,
                                                               Convert.ToInt32(drSql("EmployeeId")))

                    objCalendarSettingDataObject.SundayHourMinute = Convert.ToString(drSql("SundayHourMinute"), Nothing).Trim()
                    objCalendarSettingDataObject.MondayHourMinute = Convert.ToString(drSql("MondayHourMinute"), Nothing).Trim()
                    objCalendarSettingDataObject.TuesdayHourMinute = Convert.ToString(drSql("TuesdayHourMinute"), Nothing).Trim()
                    objCalendarSettingDataObject.WednesdayHourMinute = Convert.ToString(drSql("WednesdayHourMinute"), Nothing).Trim()
                    objCalendarSettingDataObject.ThursdayHourMinute = Convert.ToString(drSql("ThursdayHourMinute"), Nothing).Trim()
                    objCalendarSettingDataObject.FridayHourMinute = Convert.ToString(drSql("FridayHourMinute"), Nothing).Trim()
                    objCalendarSettingDataObject.SaturdayHourMinute = Convert.ToString(drSql("SaturdayHourMinute"), Nothing).Trim()

                    objCalendarSettingDataObject.Status = Convert.ToString(drSql("Status"), Nothing).Trim()

                    objCalendarSettingDataObject.UpdateBy = Convert.ToString(drSql("UpdateBy"), Nothing).Trim()
                    objCalendarSettingDataObject.UpdateDate = Convert.ToString(drSql("UpdateDate"), Nothing).Trim()

                    objCalendarSettingDataObject.SpecialRate = If((Not DBNull.Value.Equals(drSql("SpecialRate"))),
                                                                   Convert.ToDecimal(drSql("SpecialRate"), Nothing),
                                                                   objCalendarSettingDataObject.SpecialRate)

                    objCalendarSettingDataObject.Comments = Convert.ToString(drSql("Comments"), Nothing).Trim()
                    objCalendarSettingDataObject.StartDate = Convert.ToString(drSql("StartDate"), Nothing).Trim()
                    objCalendarSettingDataObject.EndDate = Convert.ToString(drSql("EndDate"), Nothing).Trim()

                    objCalendarSettingDataObject.SundayInTime = Convert.ToString(drSql("SundayInTime"), Nothing).Trim()
                    objCalendarSettingDataObject.SundayOutTime = Convert.ToString(drSql("SundayOutTime"), Nothing).Trim()
                    objCalendarSettingDataObject.MondayInTime = Convert.ToString(drSql("MondayInTime"), Nothing).Trim()
                    objCalendarSettingDataObject.MondayOutTime = Convert.ToString(drSql("MondayOutTime"), Nothing).Trim()
                    objCalendarSettingDataObject.TuesdayInTime = Convert.ToString(drSql("TuesdayInTime"), Nothing).Trim()
                    objCalendarSettingDataObject.TuesdayOutTime = Convert.ToString(drSql("TuesdayOutTime"), Nothing).Trim()
                    objCalendarSettingDataObject.WednesdayInTime = Convert.ToString(drSql("WednesdayInTime"), Nothing).Trim()
                    objCalendarSettingDataObject.WednesdayOutTime = Convert.ToString(drSql("WednesdayOutTime"), Nothing).Trim()
                    objCalendarSettingDataObject.ThursdayInTime = Convert.ToString(drSql("ThursdayInTime"), Nothing).Trim()
                    objCalendarSettingDataObject.ThursdayOutTime = Convert.ToString(drSql("ThursdayOutTime"), Nothing).Trim()
                    objCalendarSettingDataObject.FridayInTime = Convert.ToString(drSql("FridayInTime"), Nothing).Trim()
                    objCalendarSettingDataObject.FridayOutTime = Convert.ToString(drSql("FridayOutTime"), Nothing).Trim()
                    objCalendarSettingDataObject.SaturdayInTime = Convert.ToString(drSql("SaturdayInTime"), Nothing).Trim()
                    objCalendarSettingDataObject.SaturdayOutTime = Convert.ToString(drSql("SaturdayOutTime"), Nothing).Trim()
                    objCalendarSettingDataObject.SaturdayOutTime = Convert.ToString(drSql("SaturdayOutTime"), Nothing).Trim()

                    CalendarSettingList.Add(objCalendarSettingDataObject)
                End While
                objCalendarSettingDataObject = Nothing
            End If

            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return CalendarSettingList

        End Function

        Private Sub SetParameters(parameters As HybridDictionary, objCalendarSettingDataObject As CalendarSettingDataObject)

            If Not objCalendarSettingDataObject.ClientId.Equals(-1) Then
                parameters.Add("@ClientId", objCalendarSettingDataObject.ClientId)
            End If

            If Not objCalendarSettingDataObject.EmployeeId.Equals(-1) Then
                parameters.Add("@EmployeeId", objCalendarSettingDataObject.EmployeeId)
            End If

            If Not String.IsNullOrEmpty(objCalendarSettingDataObject.SundayHourMinute) Then
                parameters.Add("@SundayHourMinute", New TimeSpan(objCalendarSettingDataObject.SundayHourMinute.Split(":")(0),
                                                                 objCalendarSettingDataObject.SundayHourMinute.Split(":")(1), 0))
            End If

            If Not String.IsNullOrEmpty(objCalendarSettingDataObject.MondayHourMinute) Then
              
                parameters.Add("@MondayHourMinute", New TimeSpan(objCalendarSettingDataObject.MondayHourMinute.Split(":")(0),
                                                                objCalendarSettingDataObject.MondayHourMinute.Split(":")(1), 0))
            End If

            If Not String.IsNullOrEmpty(objCalendarSettingDataObject.TuesdayHourMinute) Then
                parameters.Add("@TuesdayHourMinute", New TimeSpan(objCalendarSettingDataObject.TuesdayHourMinute.Split(":")(0),
                                                                  objCalendarSettingDataObject.TuesdayHourMinute.Split(":")(1), 0))
            End If

            If Not String.IsNullOrEmpty(objCalendarSettingDataObject.WednesdayHourMinute) Then
                parameters.Add("@WednesdayHourMinute", New TimeSpan(objCalendarSettingDataObject.WednesdayHourMinute.Split(":")(0),
                                                                    objCalendarSettingDataObject.WednesdayHourMinute.Split(":")(1), 0))
            End If

            If Not String.IsNullOrEmpty(objCalendarSettingDataObject.ThursdayHourMinute) Then
                parameters.Add("@ThursdayHourMinute", New TimeSpan(objCalendarSettingDataObject.ThursdayHourMinute.Split(":")(0),
                                                                   objCalendarSettingDataObject.ThursdayHourMinute.Split(":")(1), 0))
            End If

            If Not String.IsNullOrEmpty(objCalendarSettingDataObject.FridayHourMinute) Then
                parameters.Add("@FridayHourMinute", New TimeSpan(objCalendarSettingDataObject.FridayHourMinute.Split(":")(0),
                                                                objCalendarSettingDataObject.FridayHourMinute.Split(":")(1), 0))
            End If

            If Not String.IsNullOrEmpty(objCalendarSettingDataObject.SaturdayHourMinute) Then
                parameters.Add("@SaturdayHourMinute", New TimeSpan(objCalendarSettingDataObject.SaturdayHourMinute.Split(":")(0),
                                                                   objCalendarSettingDataObject.SaturdayHourMinute.Split(":")(1), 0))
            End If

            parameters.Add("@Status", objCalendarSettingDataObject.Status)

            If Not objCalendarSettingDataObject.SpecialRate.Equals(Decimal.MinValue) Then
                parameters.Add("@SpecialRate", objCalendarSettingDataObject.SpecialRate)
            End If

            If Not String.IsNullOrEmpty(objCalendarSettingDataObject.Comments) Then
                parameters.Add("@Comments", objCalendarSettingDataObject.Comments)
            End If

            If Not String.IsNullOrEmpty(objCalendarSettingDataObject.StartDate) Then
                parameters.Add("@StartDate", objCalendarSettingDataObject.StartDate)
            End If

            If Not String.IsNullOrEmpty(objCalendarSettingDataObject.EndDate) Then
                parameters.Add("@EndDate", objCalendarSettingDataObject.EndDate)
            End If

            If Not String.IsNullOrEmpty(objCalendarSettingDataObject.SundayInTime) Then
                parameters.Add("@SundayInTime", New TimeSpan(objCalendarSettingDataObject.SundayInTime.Split(":")(0),
                                                             objCalendarSettingDataObject.SundayInTime.Split(":")(1), 0))
            End If

            If Not String.IsNullOrEmpty(objCalendarSettingDataObject.SundayOutTime) Then
                parameters.Add("@SundayOutTime", New TimeSpan(objCalendarSettingDataObject.SundayOutTime.Split(":")(0),
                                                             objCalendarSettingDataObject.SundayOutTime.Split(":")(1), 0))
            End If

            If Not String.IsNullOrEmpty(objCalendarSettingDataObject.MondayInTime) Then
                parameters.Add("@MondayInTime", New TimeSpan(objCalendarSettingDataObject.MondayInTime.Split(":")(0),
                                                            objCalendarSettingDataObject.MondayInTime.Split(":")(1), 0))
            End If

            If Not String.IsNullOrEmpty(objCalendarSettingDataObject.MondayOutTime) Then
                parameters.Add("@MondayOutTime", New TimeSpan(objCalendarSettingDataObject.MondayOutTime.Split(":")(0),
                                                             objCalendarSettingDataObject.MondayOutTime.Split(":")(1), 0))
            End If

            If Not String.IsNullOrEmpty(objCalendarSettingDataObject.TuesdayInTime) Then
                parameters.Add("@TuesdayInTime", New TimeSpan(objCalendarSettingDataObject.TuesdayInTime.Split(":")(0),
                                                             objCalendarSettingDataObject.TuesdayInTime.Split(":")(1), 0))
            End If

            If Not String.IsNullOrEmpty(objCalendarSettingDataObject.TuesdayOutTime) Then
                parameters.Add("@TuesdayOutTime", New TimeSpan(objCalendarSettingDataObject.TuesdayOutTime.Split(":")(0),
                                                              objCalendarSettingDataObject.TuesdayOutTime.Split(":")(1), 0))
            End If

            If Not String.IsNullOrEmpty(objCalendarSettingDataObject.WednesdayInTime) Then
                parameters.Add("@WednesdayInTime", New TimeSpan(objCalendarSettingDataObject.WednesdayInTime.Split(":")(0),
                                                                objCalendarSettingDataObject.WednesdayInTime.Split(":")(1), 0))
            End If

            If Not String.IsNullOrEmpty(objCalendarSettingDataObject.WednesdayOutTime) Then
                parameters.Add("@WednesdayOutTime", New TimeSpan(objCalendarSettingDataObject.WednesdayOutTime.Split(":")(0),
                                                                objCalendarSettingDataObject.WednesdayOutTime.Split(":")(1), 0))
            End If

            If Not String.IsNullOrEmpty(objCalendarSettingDataObject.ThursdayInTime) Then
                parameters.Add("@ThursdayInTime", New TimeSpan(objCalendarSettingDataObject.ThursdayInTime.Split(":")(0),
                                                               objCalendarSettingDataObject.ThursdayInTime.Split(":")(1), 0))
            End If

            If Not String.IsNullOrEmpty(objCalendarSettingDataObject.ThursdayOutTime) Then
                parameters.Add("@ThursdayOutTime", New TimeSpan(objCalendarSettingDataObject.ThursdayOutTime.Split(":")(0),
                                                                objCalendarSettingDataObject.ThursdayOutTime.Split(":")(1), 0))
            End If

            If Not String.IsNullOrEmpty(objCalendarSettingDataObject.FridayInTime) Then
                parameters.Add("@FridayInTime", New TimeSpan(objCalendarSettingDataObject.FridayInTime.Split(":")(0),
                                                             objCalendarSettingDataObject.FridayInTime.Split(":")(1), 0))
            End If

            If Not String.IsNullOrEmpty(objCalendarSettingDataObject.FridayOutTime) Then
                parameters.Add("@FridayOutTime", New TimeSpan(objCalendarSettingDataObject.FridayOutTime.Split(":")(0),
                                                             objCalendarSettingDataObject.FridayOutTime.Split(":")(1), 0))
            End If

            If Not String.IsNullOrEmpty(objCalendarSettingDataObject.SaturdayInTime) Then
                parameters.Add("@SaturdayInTime", New TimeSpan(objCalendarSettingDataObject.SaturdayInTime.Split(":")(0),
                                                               objCalendarSettingDataObject.SaturdayInTime.Split(":")(1), 0))
            End If

            If Not String.IsNullOrEmpty(objCalendarSettingDataObject.SaturdayOutTime) Then
                parameters.Add("@SaturdayOutTime", New TimeSpan(objCalendarSettingDataObject.SaturdayOutTime.Split(":")(0),
                                                                objCalendarSettingDataObject.SaturdayOutTime.Split(":")(1), 0))
            End If

            If Not objCalendarSettingDataObject.WeeklyScheduleMinutes.Equals(Int32.MinValue) Then
                parameters.Add("@WeeklyScheduleMinutes", objCalendarSettingDataObject.WeeklyScheduleMinutes)
            End If

        End Sub

    End Class
End Namespace

