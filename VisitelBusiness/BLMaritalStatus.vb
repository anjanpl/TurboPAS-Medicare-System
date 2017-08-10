#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Marital Status Information Data Fetching
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
    Public Class BLMaritalStatus

        ''' <summary>
        ''' Get Marital Status Information
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SelectMaritalStatus(ConVisitel As SqlConnection, CompanyId As Integer) As List(Of MaritalStatusDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()
            parameters.Add("@CompanyId", CompanyId)

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectMaritalStatus]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim MaritalStatusList As New List(Of MaritalStatusDataObject)()

            If drSql.HasRows Then
                Dim objMaritalStatusDataObject As MaritalStatusDataObject
                While drSql.Read()
                    objMaritalStatusDataObject = New MaritalStatusDataObject()

                    objMaritalStatusDataObject.MaritalStatusId = If((DBNull.Value.Equals(drSql("MaritalStatusId"))),
                                                                 objMaritalStatusDataObject.MaritalStatusId,
                                                                 Convert.ToInt32(drSql("MaritalStatusId")))

                    objMaritalStatusDataObject.MaritalStatusName = Convert.ToString(drSql("MaritalStatusName"), Nothing)

                    MaritalStatusList.Add(objMaritalStatusDataObject)
                End While
                objMaritalStatusDataObject = Nothing
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return MaritalStatusList

        End Function

    End Class
End Namespace

