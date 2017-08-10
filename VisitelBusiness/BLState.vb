#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: State Information Data Fetching
' Author: Anjan Kumar Paul
' Start Date: 22 Aug 2014
' End Date: 22 Aug 2014
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                22 Aug 2014     Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelDA.VisitelDA

Namespace VisitelBusiness
    Public Class BLState

        ''' <summary>
        ''' Get State Information
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SelectState(ConVisitel As SqlConnection, CompanyId As Integer) As List(Of StateDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()
            parameters.Add("@CompanyId", CompanyId)

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectState]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim StateList As New List(Of StateDataObject)()

            If drSql.HasRows Then
                Dim objStateDataObject As StateDataObject
                While drSql.Read()
                    objStateDataObject = New StateDataObject()

                    objStateDataObject.StateId = If((DBNull.Value.Equals(drSql("StateId"))), objStateDataObject.StateId, Convert.ToInt32(drSql("StateId")))
                    objStateDataObject.StateFullName = Convert.ToString(drSql("StateFullName"), Nothing)

                    StateList.Add(objStateDataObject)
                End While
                objStateDataObject = Nothing
            End If

            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return StateList

        End Function
    End Class
End Namespace

