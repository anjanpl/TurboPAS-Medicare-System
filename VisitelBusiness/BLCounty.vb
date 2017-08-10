#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: County Information Data Fetching
' Author: Anjan Kumar Paul
' Start Date: 05 Sept 2014
' End Date: 
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                05 Sept 2014     Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelDA.VisitelDA

Namespace VisitelBusiness
    Public Class BLCounty

        ''' <summary>
        ''' Select County Information
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SelectCounty(ConVisitel As SqlConnection, CompanyId As Integer) As List(Of CountyDataObject)

            Dim drSql As SqlDataReader = Nothing
            Dim parameters As New HybridDictionary()
            parameters.Add("@CompanyId", CompanyId)

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectCounty]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim CountyList As New List(Of CountyDataObject)()

            If drSql.HasRows Then
                Dim objCountyDataObject As CountyDataObject
                While drSql.Read()
                    objCountyDataObject = New CountyDataObject()

                    objCountyDataObject.CountyId = If((DBNull.Value.Equals(drSql("CountyId"))), objCountyDataObject.CountyId, Convert.ToInt32(drSql("CountyId")))
                    objCountyDataObject.CountyName = Convert.ToString(drSql("CountyName"), Nothing)

                    CountyList.Add(objCountyDataObject)
                End While
                objCountyDataObject = Nothing
            End If

            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return CountyList

        End Function
    End Class
End Namespace

