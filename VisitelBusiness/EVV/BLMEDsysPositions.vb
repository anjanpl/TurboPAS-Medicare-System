#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: MEDsys Positions Data Fetch, Insert, Update
' Author: Anjan Kumar Paul
' Start Date: 09 Jan 2016
' End Date: 09 Jan 2016
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                09 Jan 2016     Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelDA.VisitelDA
Imports VisitelBusiness.VisitelBusiness.DataObject.EVV

Namespace VisitelBusiness
    Public Class BLMEDsysPositions

        Public Sub InsertMEDsysPositionsInfo(ConVisitel As SqlConnection, objMEDsysPositionsDataObject As MEDsysPositionsDataObject)

            Dim parameters As New HybridDictionary()

            SetParameters(parameters, objMEDsysPositionsDataObject)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertMEDsysPositionsInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub UpdateMEDsysPositionsInfo(ConVisitel As SqlConnection, objMEDsysPositionsDataObject As MEDsysPositionsDataObject)

            Dim parameters As New HybridDictionary()

            parameters.Add("@Id", objMEDsysPositionsDataObject.Id)

            SetParameters(parameters, objMEDsysPositionsDataObject)

            parameters.Add("@UpdateBy", objMEDsysPositionsDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateMEDsysPositionsInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Private Sub SetParameters(parameters As HybridDictionary, objMEDsysPositionsDataObject As MEDsysPositionsDataObject)
            parameters.Add("@AccountId", objMEDsysPositionsDataObject.AccountId)
            parameters.Add("@PositionCode", objMEDsysPositionsDataObject.PositionCode)
            parameters.Add("@PositionName", objMEDsysPositionsDataObject.PositionName)
        End Sub

        Public Sub DeleteMEDsysPositionsInfo(ConVisitel As SqlConnection, Id As Int64, DeletedBy As String)

            Dim parameters As New HybridDictionary()

            parameters.Add("@Id", Id)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteMEDsysPositionsInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Function SelectMEDsysPositionsInfo(ConVisitel As SqlConnection) As List(Of MEDsysPositionsDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SelectMEDsysPositions]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim MEDsysPositions As New List(Of MEDsysPositionsDataObject)()

            If drSql.HasRows Then
                Dim objMEDsysPositionsDataObject As MEDsysPositionsDataObject
                While drSql.Read()
                    objMEDsysPositionsDataObject = New MEDsysPositionsDataObject()

                    objMEDsysPositionsDataObject.Id = If((DBNull.Value.Equals(drSql("ID"))), objMEDsysPositionsDataObject.Id, Convert.ToInt32(drSql("ID")))

                    objMEDsysPositionsDataObject.AccountId = Convert.ToString(drSql("AccountId"), Nothing)
                    objMEDsysPositionsDataObject.PositionCode = Convert.ToString(drSql("PositionCode"), Nothing)
                    objMEDsysPositionsDataObject.PositionName = Convert.ToString(drSql("PositionName"), Nothing)

                    objMEDsysPositionsDataObject.UpdateDate = Convert.ToString(drSql("UpdateDate"), Nothing)
                    objMEDsysPositionsDataObject.UpdateBy = Convert.ToString(drSql("UpdateBy"), Nothing)

                    MEDsysPositions.Add(objMEDsysPositionsDataObject)

                End While
                objMEDsysPositionsDataObject = Nothing
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return MEDsysPositions

        End Function
    End Class
End Namespace

