#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Client Status Information Data Fetching 
' Author: Anjan Kumar Paul
' Start Date: 31 Aug 2014
' End Date: 
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
    Public Class BLClientStatus

        ''' <summary>
        ''' Select Client Status Information
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SelectClientStatus(ConVisitel As SqlConnection, CompanyId As Integer) As List(Of ClientStatusDataObject)

            Dim drSql As SqlDataReader = Nothing
            Dim parameters As New HybridDictionary()
            parameters.Add("@CompanyId", CompanyId)

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectClientStatus]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim ClientStatusList As New List(Of ClientStatusDataObject)()

            If drSql.HasRows Then
                Dim objClientStatusDataObject As ClientStatusDataObject
                While drSql.Read()
                    objClientStatusDataObject = New ClientStatusDataObject()

                    objClientStatusDataObject.ClientStatusId = If((DBNull.Value.Equals(drSql("ClientStatusId"))),
                                                                  objClientStatusDataObject.ClientStatusId, Convert.ToInt32(drSql("ClientStatusId")))

                    objClientStatusDataObject.ClientStatusName = Convert.ToString(drSql("ClientStatusName"), Nothing)

                    ClientStatusList.Add(objClientStatusDataObject)
                End While
                objClientStatusDataObject = Nothing
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return ClientStatusList

        End Function
    End Class
End Namespace

