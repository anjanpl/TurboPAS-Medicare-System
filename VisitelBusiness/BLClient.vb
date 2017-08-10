#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Client Information Data Fetching 
' Author: Anjan Kumar Paul
' Start Date: 12 Sept 2014
' End Date: 12 Sept 2014
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                12 Sept 2014     Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelDA.VisitelDA

Namespace VisitelBusiness
    Public Class BLClient

        ''' <summary>
        ''' Get Client Information
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SelectClient(ConVisitel As SqlConnection, CompanyId As Integer, ClientListFor As EnumDataObject.ClientListFor) As List(Of ClientDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()
            parameters.Add("@CompanyId", CompanyId)

            Select Case ClientListFor
                Case EnumDataObject.ClientListFor.Individual
                    parameters.Add("@ClientListFor", "Individual")
                    Exit Select
                Case EnumDataObject.ClientListFor.PayPeriod
                    parameters.Add("@ClientListFor", "PayPeriod")
                    Exit Select
                Case EnumDataObject.ClientListFor.CareSummary
                    parameters.Add("@ClientListFor", "CareSummary")
                    Exit Select
                Case EnumDataObject.ClientListFor.EDICorrectedClaims
                    parameters.Add("@ClientListFor", "EDICorrectedClaims")
                    Exit Select
                Case EnumDataObject.ClientListFor.VestaClient
                    parameters.Add("@ClientListFor", "VestaClient")
                    Exit Select
            End Select

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectClient]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim ClientList As New List(Of ClientDataObject)

            If drSql.HasRows Then
                Dim objClientDataObject As ClientDataObject
                While drSql.Read()
                    objClientDataObject = New ClientDataObject()

                    objClientDataObject.ClientInfoId = If((DBNull.Value.Equals(drSql("ClientInfoId"))), objClientDataObject.ClientInfoId _
                                                          , Convert.ToInt64(drSql("ClientInfoId"), Nothing))
                    objClientDataObject.ClientName = Convert.ToString(drSql("ClientName"), Nothing)
                    objClientDataObject.Address = Convert.ToString(drSql("Address"), Nothing)
                    objClientDataObject.StateClientId = If((DBNull.Value.Equals(drSql("StateClientId"))), objClientDataObject.StateClientId,
                                                           Convert.ToInt32(drSql("StateClientId"), Nothing))
                    objClientDataObject.SocialSecurityNumber = Convert.ToString(drSql("SocialSecurityNumber"), Nothing)
                    objClientDataObject.Name = Convert.ToString(drSql("Name"), Nothing)
                    objClientDataObject.CaseWorkerId = If((DBNull.Value.Equals(drSql("CaseWorkerId"))), objClientDataObject.CaseWorkerId,
                                                          Convert.ToInt32(drSql("CaseWorkerId"), Nothing))

                    objClientDataObject.InsuranceNumber = Convert.ToString(drSql("InsuranceNumber"), Nothing)

                    ClientList.Add(objClientDataObject)
                End While
                objClientDataObject = Nothing
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return ClientList

        End Function
    End Class
End Namespace

