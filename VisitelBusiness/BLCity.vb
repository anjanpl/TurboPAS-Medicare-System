#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: City Information Data Fetching 
' Author: Anjan Kumar Paul
' Start Date: 15 Aug 2014
' End Date: 15 Aug 2014
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                15 Aug 2014     Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelDA.VisitelDA

Namespace VisitelBusiness
    Public Class BLCity

        ''' <summary>
        ''' Get City Information
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SelectCity(ConVisitel As SqlConnection, CompanyId As Integer) As List(Of CityDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()
            parameters.Add("@CompanyId", CompanyId)

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectCity]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim CityList As New List(Of CityDataObject)()

            If drSql.HasRows Then
                Dim objCityDataObject As CityDataObject
                While drSql.Read()
                    objCityDataObject = New CityDataObject()

                    objCityDataObject.CityId = If((DBNull.Value.Equals(drSql("CityId"))), objCityDataObject.CityId, Convert.ToInt32(drSql("CityId")))
                    objCityDataObject.CityName = Convert.ToString(drSql("CityName"), Nothing)

                    CityList.Add(objCityDataObject)
                End While
                objCityDataObject = Nothing
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return CityList

        End Function
    End Class
End Namespace

