#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Calculating Pay Period
' Author: Anjan Kumar Paul
' Start Date: 12 Dec 2014
' End Date: 12 Dec 2014
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                12 Dec 2014     Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelDA.VisitelDA

Namespace VisitelBusiness
    Public Class BLPayPeriodGenerator

        Public Sub CalculatePayPeriod(ConVisitel As SqlConnection, StartDate As String, EndDate As String, UserId As String, ClientId As Int64)

            Dim parameters As New HybridDictionary()

            parameters.Add("@StartDate", StartDate)
            parameters.Add("@EndDate", EndDate)

            If (Not ClientId.Equals(-1)) Then
                parameters.Add("@ClientId", ClientId)
            End If

            parameters.Add("@UpdateBy", UserId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertPayPeriodInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

    End Class
End Namespace

