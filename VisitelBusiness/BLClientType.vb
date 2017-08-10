#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Client Type Information Data Fetching 
' Author: Anjan Kumar Paul
' Start Date: 06 Sept 2014
' End Date: 
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                06 Sept 2014     Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelDA.VisitelDA

Namespace VisitelBusiness
    Public Class BLClientType

        ''' <summary>
        ''' Select Client Type Information
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SelectClientType(ConVisitel As SqlConnection, CompanyId As Integer, ClientTypeListFor As EnumDataObject.ClientTypeListFor) As List(Of ClientTypeDataObject)

            Dim drSql As SqlDataReader = Nothing
            Dim parameters As New HybridDictionary()

            parameters.Add("@CompanyId", CompanyId)

            Select Case ClientTypeListFor
                Case EnumDataObject.ClientTypeListFor.Individual
                    parameters.Add("@ClientTypeListFor", "Individual")
                    Exit Select
                Case EnumDataObject.ClientTypeListFor.CareSummary
                    parameters.Add("@ClientTypeListFor", "CareSummary")
                    Exit Select
                Case EnumDataObject.ClientTypeListFor.Vesta
                    parameters.Add("@ClientTypeListFor", "Vesta")
                    Exit Select
            End Select


            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectClientType]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim ClientTypeList As New List(Of ClientTypeDataObject)()

            If drSql.HasRows Then
                Dim objClientTypeDataObject As ClientTypeDataObject
                While drSql.Read()
                    objClientTypeDataObject = New ClientTypeDataObject()

                    objClientTypeDataObject.IdNumber = If((DBNull.Value.Equals(drSql("IdNumber"))), objClientTypeDataObject.IdNumber, Convert.ToInt32(drSql("IdNumber")))
                    objClientTypeDataObject.Name = Convert.ToString(drSql("Name"), Nothing)

                    ClientTypeList.Add(objClientTypeDataObject)
                End While
                objClientTypeDataObject = Nothing
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return ClientTypeList

        End Function
    End Class
End Namespace

