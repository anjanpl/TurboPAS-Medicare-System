#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Client Group Information Inserting, Updating, Deleting, Filtering, & Fetching
' Author: Anjan Kumar Paul
' Start Date: 29 Aug 2014
' End Date: 29 Aug 2014
' History:
'      Version                  Author                      Date             Reason 
'      1.0.0                                                29 Aug 2014      Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.DataObject.Settings
Imports VisitelDA.VisitelDA

Namespace VisitelBusiness.Settings
    Public Class BLClientGroupSetting
        Public Sub InsertClientGroupInfo(ConVisitel As SqlConnection, objClientGroupSettingDataObject As ClientGroupSettingDataObject)
            Dim parameters As New HybridDictionary()
            parameters.Add("@ClientGroupName", objClientGroupSettingDataObject.GroupName)
            parameters.Add("@CompanyId", objClientGroupSettingDataObject.CompanyId)
            parameters.Add("@UserId", objClientGroupSettingDataObject.UserId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertClientGroupInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        Public Sub UpdateClientGroupInfo(ConVisitel As SqlConnection, objClientGroupSettingDataObject As ClientGroupSettingDataObject)
            Dim parameters As New HybridDictionary()

            parameters.Add("@ClientGroupId", objClientGroupSettingDataObject.GroupId)
            parameters.Add("@ClientGroupName", objClientGroupSettingDataObject.GroupName)
            parameters.Add("@UpdateBy", objClientGroupSettingDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateClientGroupInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub DeleteClientGroupInfo(ConVisitel As SqlConnection, ClientGroupId As Integer, DeletedBy As Integer)
            Dim parameters As New HybridDictionary()
            parameters.Add("@ClientGroupId", ClientGroupId)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteClientGroupInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        ''' <summary>
        ''' Retrieving ClientGroup Info
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SelectClientGroupInfo(ConVisitel As SqlConnection, CompanyId As Integer) As List(Of ClientGroupSettingDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()

            parameters.Add("@CompanyId", CompanyId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SelectClientGroupInfo]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim ClientGroupSettingList As New List(Of ClientGroupSettingDataObject)()

            FillDataList(drSql, ClientGroupSettingList)

            Return ClientGroupSettingList

        End Function

        ''' <summary>
        ''' Search ClientGroup Info
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <param name="ClientGroupName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SearchClientGroupInfo(ConVisitel As SqlConnection, CompanyId As Integer, ClientGroupName As String) As List(Of ClientGroupSettingDataObject)

            Dim drSql As SqlDataReader = Nothing
            Dim parameters As New HybridDictionary()

            parameters.Add("@CompanyId", CompanyId)

            If (Not String.IsNullOrEmpty(ClientGroupName)) Then
                parameters.Add("@ClientGroupName", ClientGroupName)
            End If

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SearchClientGroupInfo]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim ClientGroupSettingList As New List(Of ClientGroupSettingDataObject)()

            FillDataList(drSql, ClientGroupSettingList)

            Return ClientGroupSettingList

        End Function

        Private Sub FillDataList(ByRef drSql As SqlDataReader, ClientGroupSettingList As List(Of ClientGroupSettingDataObject))

            If drSql.HasRows Then
                Dim objClientGroupSettingDataObject As ClientGroupSettingDataObject
                While drSql.Read()
                    objClientGroupSettingDataObject = New ClientGroupSettingDataObject()

                    objClientGroupSettingDataObject.GroupId = If((DBNull.Value.Equals(drSql("GroupId"))),
                                                                 objClientGroupSettingDataObject.GroupId,
                                                                 Convert.ToInt32(drSql("GroupId")))

                    objClientGroupSettingDataObject.GroupName = Convert.ToString(drSql("GroupName"), Nothing)
                    objClientGroupSettingDataObject.UpdateBy = Convert.ToString(drSql("UpdateBy"), Nothing)
                    objClientGroupSettingDataObject.UpdateDate = Convert.ToString(drSql("UpdateDate"), Nothing)

                    ClientGroupSettingList.Add(objClientGroupSettingDataObject)
                End While
                objClientGroupSettingDataObject = Nothing
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing
        End Sub
    End Class
End Namespace

