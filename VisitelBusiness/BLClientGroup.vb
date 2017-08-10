#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Client Group Information Data Fetching 
' Author: Anjan Kumar Paul
' Start Date: 29 Aug 2014
' End Date: 29 Aug 2014
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                29 Aug 2014     Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelDA.VisitelDA

Namespace VisitelBusiness
    Public Class BLClientGroup

        ''' <summary>
        ''' Get Client Group Information
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SelectClientGroup(ConVisitel As SqlConnection, CompanyId As Integer) As List(Of ClientGroupDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()
            parameters.Add("@CompanyId", CompanyId)

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectClientGroup]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim ClientGroupList As New List(Of ClientGroupDataObject)()

            If drSql.HasRows Then
                Dim objClientGroupDataObject As ClientGroupDataObject
                While drSql.Read()
                    objClientGroupDataObject = New ClientGroupDataObject()

                    objClientGroupDataObject.ClientGroupId = If((DBNull.Value.Equals(drSql("GroupId"))), objClientGroupDataObject.ClientGroupId, Convert.ToInt32(drSql("GroupId")))
                    objClientGroupDataObject.ClientGroupName = Convert.ToString(drSql("GroupName"), Nothing)

                    ClientGroupList.Add(objClientGroupDataObject)
                End While
                objClientGroupDataObject = Nothing
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return ClientGroupList

        End Function
    End Class
End Namespace

