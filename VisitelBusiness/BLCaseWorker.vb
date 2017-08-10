
#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Case Worker Data Fetching 
' Author: Anjan Kumar Paul
' Start Date: 31 Aug 2014
' End Date: 31 Aug 2014
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                31 Aug 2014     Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelDA.VisitelDA

Namespace VisitelBusiness
    Public Class BLCaseWorker
        ''' <summary>
        ''' Case Worker Information
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SelectCaseWorker(ConVisitel As SqlConnection, CompanyId As Integer) As List(Of CaseWorkerDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()
            parameters.Add("@CompanyId", CompanyId)

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectCaseWorker]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim CaseWorkerList As New List(Of CaseWorkerDataObject)()

            If drSql.HasRows Then
                Dim objCaseWorkerDataObject As CaseWorkerDataObject
                While drSql.Read()
                    objCaseWorkerDataObject = New CaseWorkerDataObject()

                    objCaseWorkerDataObject.CaseWorkerId = If((DBNull.Value.Equals(drSql("CaseWorkerId"))), objCaseWorkerDataObject.CaseWorkerId,
                                                              Convert.ToInt32(drSql("CaseWorkerId")))
                    objCaseWorkerDataObject.CaseWorkerName = Convert.ToString(drSql("CaseWorkerName"), Nothing)

                    CaseWorkerList.Add(objCaseWorkerDataObject)
                End While
                objCaseWorkerDataObject = Nothing
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return CaseWorkerList

        End Function
    End Class
End Namespace

