#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Discharge Reason Information Inserting, Updating, Deleting, Filtering & Fetching
' Author: Anjan Kumar Paul
' Start Date: 04 Sept 2014
' End Date: 04 Sept 2014
' History:
'      Version                  Author                      Date             Reason 
'      1.0.0                                                04 Sept 2014     Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.DataObject.Settings
Imports VisitelDA.VisitelDA

Namespace VisitelBusiness.Settings
    Public Class BLDischargeReasonSetting

        Public Sub InsertDischargeReasonInfo(ConVisitel As SqlConnection, objDischargeReasonSettingDataObject As DischargeReasonSettingDataObject)
            Dim parameters As New HybridDictionary()
            parameters.Add("@Name", objDischargeReasonSettingDataObject.Name)
            parameters.Add("@Status", objDischargeReasonSettingDataObject.Status)
            parameters.Add("@CompanyId", objDischargeReasonSettingDataObject.CompanyId)
            parameters.Add("@UserId", objDischargeReasonSettingDataObject.UserId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertDischargeReasonInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        Public Sub UpdateDischargeReasonInfo(ConVisitel As SqlConnection, objDischargeReasonSettingDataObject As DischargeReasonSettingDataObject)
            Dim parameters As New HybridDictionary()

            parameters.Add("@IdNumber", objDischargeReasonSettingDataObject.IdNumber)
            parameters.Add("@Name", objDischargeReasonSettingDataObject.Name)
            parameters.Add("@Status", objDischargeReasonSettingDataObject.Status)
            parameters.Add("@UpdateBy", objDischargeReasonSettingDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateDischargeReasonInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub DeleteDischargeReasonInfo(ConVisitel As SqlConnection, IdNumber As Integer, DeletedBy As Integer)
            Dim parameters As New HybridDictionary()
            parameters.Add("@IdNumber", IdNumber)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteDischargeReasonInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        ''' <summary>
        ''' Retrieving Discharge Reason Info
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SelectDischargeReasonInfo(ConVisitel As SqlConnection, CompanyId As Integer) As List(Of DischargeReasonSettingDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()

            parameters.Add("@CompanyId", CompanyId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SelectDischargeReasonInfo]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim DischargeReasonSettingList As New List(Of DischargeReasonSettingDataObject)()

            FillDataList(drSql, DischargeReasonSettingList)

            Return DischargeReasonSettingList

        End Function

        ''' <summary>
        ''' Searching Discharge Reason Info
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <param name="Name"></param>
        ''' <param name="Status"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SearchDischargeReasonInfo(ConVisitel As SqlConnection, CompanyId As Integer, Name As String, Status As Int16) As List(Of DischargeReasonSettingDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()

            parameters.Add("@CompanyId", CompanyId)

            If (Not String.IsNullOrEmpty(Name)) Then
                parameters.Add("@Name", Name)
            End If

            If (Status <> -1) Then
                parameters.Add("@Status", Status)
            End If


            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SearchDischargeReasonInfo]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim DischargeReasonSettingList As New List(Of DischargeReasonSettingDataObject)()

            FillDataList(drSql, DischargeReasonSettingList)

            Return DischargeReasonSettingList

        End Function

        Private Sub FillDataList(ByRef drSql As SqlDataReader, ByRef DischargeReasonSettingList As List(Of DischargeReasonSettingDataObject))

            If drSql.HasRows Then
                Dim objDischargeReasonSettingDataObject As DischargeReasonSettingDataObject
                While drSql.Read()
                    objDischargeReasonSettingDataObject = New DischargeReasonSettingDataObject()

                    objDischargeReasonSettingDataObject.IdNumber = If((Not DBNull.Value.Equals(drSql("IdNumber"))),
                                                                     Convert.ToInt32(drSql("IdNumber"), Nothing),
                                                                     objDischargeReasonSettingDataObject.IdNumber)

                    objDischargeReasonSettingDataObject.Name = Convert.ToString(drSql("Name"), Nothing)

                    objDischargeReasonSettingDataObject.Status = If((Not DBNull.Value.Equals(drSql("Status"))),
                                                                    Convert.ToInt16(drSql("Status"), Nothing),
                                                                    objDischargeReasonSettingDataObject.Status)

                    objDischargeReasonSettingDataObject.UpdateBy = Convert.ToString(drSql("UpdateBy"), Nothing)
                    objDischargeReasonSettingDataObject.UpdateDate = Convert.ToString(drSql("UpdateDate"), Nothing)

                    DischargeReasonSettingList.Add(objDischargeReasonSettingDataObject)

                End While
                objDischargeReasonSettingDataObject = Nothing
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing
        End Sub

    End Class
End Namespace

