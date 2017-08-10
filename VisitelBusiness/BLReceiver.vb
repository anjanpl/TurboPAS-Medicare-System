#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Receiver Information Data Fetching
' Author: Anjan Kumar Paul
' Start Date: 28 Aug 2014
' End Date: 28 Aug 2014
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                28 Aug 2014     Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelDA.VisitelDA

Namespace VisitelBusiness
    Public Class BLReceiver

        ''' <summary>
        ''' Get Receiver Information
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SelectReceiver(ConVisitel As SqlConnection, CompanyId As Integer) As List(Of ReceiverDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()
            parameters.Add("@CompanyId", CompanyId)

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectReceiver]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim ReceiverList As New List(Of ReceiverDataObject)()

            If drSql.HasRows Then
                Dim objReceiverDataObject As ReceiverDataObject
                While drSql.Read()
                    objReceiverDataObject = New ReceiverDataObject()

                    objReceiverDataObject.ReceiverId = If((DBNull.Value.Equals(drSql("ReceiverId"))), objReceiverDataObject.ReceiverId, Convert.ToInt32(drSql("ReceiverId")))
                    objReceiverDataObject.ReceiverName = Convert.ToString(drSql("ReceiverName"), Nothing)

                    ReceiverList.Add(objReceiverDataObject)
                End While
                objReceiverDataObject = Nothing
            End If

            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return ReceiverList

        End Function
    End Class
End Namespace

