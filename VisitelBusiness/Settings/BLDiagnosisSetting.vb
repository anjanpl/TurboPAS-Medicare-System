#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Diagnosis Information Inserting, Updating, Deleting, Filtering & Fetching
' Author: Anjan Kumar Paul
' Start Date: 06 Sept 2014
' End Date: 06 Sept 2014
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                06 Sept 2014     Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.DataObject.Settings
Imports VisitelDA.VisitelDA

Namespace VisitelBusiness.Settings
    Public Class BLDiagnosisSetting
        Public Sub InsertDiagnosisInfo(ConVisitel As SqlConnection, objDiagnosisSettingDataObject As DiagnosisSettingDataObject)
            Dim parameters As New HybridDictionary()
            parameters.Add("@Code", objDiagnosisSettingDataObject.Code)
            parameters.Add("@Description", objDiagnosisSettingDataObject.Description)
            parameters.Add("@UserId", objDiagnosisSettingDataObject.UserId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertDiagnosisInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        Public Sub UpdateDiagnosisInfo(ConVisitel As SqlConnection, objDiagnosisSettingDataObject As DiagnosisSettingDataObject)
            Dim parameters As New HybridDictionary()

            parameters.Add("@Id", objDiagnosisSettingDataObject.DiagnosisId)
            parameters.Add("@Code", objDiagnosisSettingDataObject.Code)
            parameters.Add("@Description", objDiagnosisSettingDataObject.Description)
            parameters.Add("@UpdateBy", objDiagnosisSettingDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateDiagnosisInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub DeleteDiagnosisInfo(ConVisitel As SqlConnection, DiagnosisId As Integer, DeletedBy As Integer)
            Dim parameters As New HybridDictionary()
            parameters.Add("@Id", DiagnosisId)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteDiagnosisInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        ''' <summary>
        ''' Retrieving Diagnosis Info
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SelectDiagnosisInfo(ConVisitel As SqlConnection, CompanyId As Integer) As List(Of DiagnosisSettingDataObject)

            Dim drSql As SqlDataReader = Nothing
            'Dim parameters As New HybridDictionary()

            'parameters.Add("@CompanyId", CompanyId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SelectDiagnosisInfo]", drSql, ConVisitel, Nothing)

            objSharedSettings = Nothing
            'parameters = Nothing

            Dim DiagnosisSettingList As New List(Of DiagnosisSettingDataObject)()

            FillDataList(drSql, DiagnosisSettingList)

            Return DiagnosisSettingList

        End Function

        ''' <summary>
        ''' Searching Diagnosis Info
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="objDiagnosisSettingDataObject"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SearchDiagnosisInfo(ConVisitel As SqlConnection,
                                             ByRef objDiagnosisSettingDataObject As DiagnosisSettingDataObject) As List(Of DiagnosisSettingDataObject)

            Dim drSql As SqlDataReader = Nothing
            Dim parameters As New HybridDictionary()

            If (Not String.IsNullOrEmpty(objDiagnosisSettingDataObject.Code)) Then
                parameters.Add("@Code", objDiagnosisSettingDataObject.Code)
            End If

            If (Not String.IsNullOrEmpty(objDiagnosisSettingDataObject.Description)) Then
                parameters.Add("@Description", objDiagnosisSettingDataObject.Description)
            End If

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SearchDiagnosisInfo]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
            objDiagnosisSettingDataObject = Nothing

            Dim DiagnosisSettingList As New List(Of DiagnosisSettingDataObject)()

            FillDataList(drSql, DiagnosisSettingList)

            Return DiagnosisSettingList

        End Function

        Private Sub FillDataList(ByRef drSql As SqlDataReader, ByRef DiagnosisSettingList As List(Of DiagnosisSettingDataObject))

            If drSql.HasRows Then
                Dim objDiagnosisSettingDataObject As DiagnosisSettingDataObject
                While drSql.Read()
                    objDiagnosisSettingDataObject = New DiagnosisSettingDataObject()

                    objDiagnosisSettingDataObject.DiagnosisId = If((Not DBNull.Value.Equals(drSql("Id"))),
                                                                     Convert.ToInt32(drSql("Id"), Nothing),
                                                                     objDiagnosisSettingDataObject.DiagnosisId)

                    objDiagnosisSettingDataObject.Code = Convert.ToString(drSql("Code"), Nothing)
                    objDiagnosisSettingDataObject.Description = Convert.ToString(drSql("Description"), Nothing)

                    objDiagnosisSettingDataObject.UpdateBy = Convert.ToString(drSql("UpdateBy"), Nothing)
                    objDiagnosisSettingDataObject.UpdateDate = Convert.ToString(drSql("UpdateDate"), Nothing)

                    DiagnosisSettingList.Add(objDiagnosisSettingDataObject)
                End While
                objDiagnosisSettingDataObject = Nothing
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing
        End Sub

    End Class
End Namespace

