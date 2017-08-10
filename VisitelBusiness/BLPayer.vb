#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Payer Information Data Fetching
' Author: Anjan Kumar Paul
' Start Date: 28 Aug 2014
' End Date: 28 Aug 2014
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                28 Aug 2014     Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Imports System.Data.SqlClient
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelDA.VisitelDA

Namespace VisitelBusiness
    Public Class BLPayer

        ''' <summary>
        ''' Get Payer Information
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SelectPayer(ConVisitel As SqlConnection) As List(Of PayerDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectPayer]", drSql, ConVisitel, Nothing)
            objSharedSettings = Nothing

            Dim PayerList As New List(Of PayerDataObject)()

            If drSql.HasRows Then
                Dim objPayerDataObject As PayerDataObject
                While drSql.Read()
                    objPayerDataObject = New PayerDataObject()

                    objPayerDataObject.PayerId = If((DBNull.Value.Equals(drSql("PayerId"))), objPayerDataObject.PayerId, Convert.ToInt32(drSql("PayerId")))
                    objPayerDataObject.PayerName = Convert.ToString(drSql("PayerName"), Nothing)

                    PayerList.Add(objPayerDataObject)
                End While
                objPayerDataObject = Nothing
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return PayerList

        End Function
    End Class
End Namespace

