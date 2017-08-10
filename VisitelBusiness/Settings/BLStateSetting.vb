
#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: State Information Inserting, Updating, Deleting, Filtering, & Fetching
' Author: Anjan Kumar Paul
' Start Date: 23 Aug 2014
' End Date: 23 Aug 2014
' History:
'      Version                  Author                      Date             Reason 
'      1.0.0                                                23 Aug 2014      Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.DataObject.Settings
Imports VisitelDA.VisitelDA

Namespace VisitelBusiness.Settings
    Public Class BLStateSetting
        Public Sub InsertStateInfo(ConVisitel As SqlConnection, objStateSettingDataObject As StateSettingDataObject)
            Dim parameters As New HybridDictionary()
            parameters.Add("@StateShortName", objStateSettingDataObject.StateShortName)
            parameters.Add("@StateFullName", objStateSettingDataObject.StateFullName)
            parameters.Add("@CompanyId", objStateSettingDataObject.CompanyId)
            parameters.Add("@UserId", objStateSettingDataObject.UserId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertStateInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        Public Sub UpdateStateInfo(ConVisitel As SqlConnection, objStateSettingDataObject As StateSettingDataObject)
            Dim parameters As New HybridDictionary()

            parameters.Add("@StateId", objStateSettingDataObject.StateId)
            parameters.Add("@StateShortName", objStateSettingDataObject.StateShortName)
            parameters.Add("@StateFullName", objStateSettingDataObject.StateFullName)
            parameters.Add("@UpdateBy", objStateSettingDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateStateInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub DeleteStateInfo(ConVisitel As SqlConnection, StateId As Integer, DeletedBy As Integer)
            Dim parameters As New HybridDictionary()
            parameters.Add("@StateId", StateId)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteStateInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        ''' <summary>
        ''' Retrieving State Info
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SelectStateInfo(ConVisitel As SqlConnection, CompanyId As Integer) As List(Of StateSettingDataObject)

            Dim drSql As SqlDataReader = Nothing
            Dim parameters As New HybridDictionary()

            parameters.Add("@CompanyId", CompanyId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SelectStateInfo]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim StateSettingList As New List(Of StateSettingDataObject)()

            FillDataList(drSql, StateSettingList)

            Return StateSettingList

        End Function

        ''' <summary>
        ''' Searching State Info
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <param name="StateShortName"></param>
        ''' <param name="StateFullName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SearchStateInfo(ConVisitel As SqlConnection, CompanyId As Integer, StateShortName As String, StateFullName As String) As List(Of StateSettingDataObject)

            Dim drSql As SqlDataReader = Nothing
            Dim parameters As New HybridDictionary()

            parameters.Add("@CompanyId", CompanyId)

            If (Not String.IsNullOrEmpty(StateShortName)) Then
                parameters.Add("@StateShortName", StateShortName)
            End If

            If (Not String.IsNullOrEmpty(StateFullName)) Then
                parameters.Add("@StateFullName", StateFullName)
            End If

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SearchStateInfo]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim StateSettingList As New List(Of StateSettingDataObject)()
            
            FillDataList(drSql, StateSettingList)

            Return StateSettingList

        End Function

        Private Sub FillDataList(ByRef drSql As SqlDataReader, ByRef StateSettingList As List(Of StateSettingDataObject))

            If drSql.HasRows Then
                Dim objStateSettingDataObject As StateSettingDataObject
                While drSql.Read()
                    objStateSettingDataObject = New StateSettingDataObject()

                    objStateSettingDataObject.StateId = If((Not DBNull.Value.Equals(drSql("StateId"))),
                                                                     Convert.ToInt32(drSql("StateId"), Nothing),
                                                                     objStateSettingDataObject.StateId)

                    objStateSettingDataObject.StateShortName = Convert.ToString(drSql("StateShortName"), Nothing)
                    objStateSettingDataObject.StateFullName = Convert.ToString(drSql("StateFullName"), Nothing)
                    objStateSettingDataObject.UpdateBy = Convert.ToString(drSql("UpdateBy"), Nothing)
                    objStateSettingDataObject.UpdateDate = Convert.ToString(drSql("UpdateDate"), Nothing)

                    StateSettingList.Add(objStateSettingDataObject)
                End While
                objStateSettingDataObject = Nothing
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing
        End Sub

    End Class
End Namespace

