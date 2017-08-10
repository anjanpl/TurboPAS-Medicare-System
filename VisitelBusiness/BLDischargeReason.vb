#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Discharge Reason Information Data Fetching
' Author: Anjan Kumar Paul
' Start Date: 05 Sept 2014
' End Date: 05 Sept 2014
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
    Public Class BLDischargeReason

        ''' <summary>
        ''' Get Discharge Reason Information
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SelectDischargeReason(ConVisitel As SqlConnection, CompanyId As Integer) As List(Of DischargeReasonDataObject)

            Dim drSql As SqlDataReader = Nothing
            Dim parameters As New HybridDictionary()

            parameters.Add("@CompanyId", CompanyId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SelectDischargeReason]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim DischargeReasonList As New List(Of DischargeReasonDataObject)()

            If drSql.HasRows Then
                Dim objDischargeReasonDataObject As DischargeReasonDataObject
                While drSql.Read()
                    objDischargeReasonDataObject = New DischargeReasonDataObject()

                    objDischargeReasonDataObject.IdNumber = If((DBNull.Value.Equals(drSql("IdNumber"))), objDischargeReasonDataObject.IdNumber, Convert.ToInt32(drSql("IdNumber")))
                    objDischargeReasonDataObject.Name = Convert.ToString(drSql("Name"), Nothing)

                    DischargeReasonList.Add(objDischargeReasonDataObject)
                End While
                objDischargeReasonDataObject = Nothing
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return DischargeReasonList

        End Function

    End Class
End Namespace

