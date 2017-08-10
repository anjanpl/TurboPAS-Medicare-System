#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Service Type Information Data Fetching 
' Author: Anjan Kumar Paul
' Start Date: 25 Mar 2016
' End Date: 25 Mar 2016
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                25 Mar 2016     Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelDA.VisitelDA

Namespace VisitelBusiness
    Public Class BLServiceType

        ''' <summary>
        ''' Get Client Group Information
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function SelectServiceType(ConVisitel As SqlConnection) As List(Of ServiceTypeDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectServiceTypeInfo]", drSql, ConVisitel, Nothing)
            objSharedSettings = Nothing

            Dim ServiceTypeList As New List(Of ServiceTypeDataObject)()

            If drSql.HasRows Then
                Dim objServiceTypeDataObject As ServiceTypeDataObject
                While drSql.Read()
                    objServiceTypeDataObject = New ServiceTypeDataObject()

                    objServiceTypeDataObject.Id = If((DBNull.Value.Equals(drSql("ID"))), objServiceTypeDataObject.Id, Convert.ToInt32(drSql("ID")))
                    objServiceTypeDataObject.ProgramService = Convert.ToString(drSql("Program_Service"), Nothing)
                    objServiceTypeDataObject.Description = Convert.ToString(drSql("Description"), Nothing)
                    objServiceTypeDataObject.ServiceGroup = Convert.ToString(drSql("SERVICE_GROUP"), Nothing)
                    objServiceTypeDataObject.BillCode = Convert.ToString(drSql("Bill_Code"), Nothing)
                    objServiceTypeDataObject.ServiceCode = Convert.ToString(drSql("SERVICE_CODE"), Nothing)
                    objServiceTypeDataObject.ProcedureCodeQualifier = Convert.ToString(drSql("Proc_Code_Qualifier"), Nothing)
                    objServiceTypeDataObject.HCPCS = Convert.ToString(drSql("HCPCS"), Nothing)
                    objServiceTypeDataObject.ModifierOne = Convert.ToString(drSql("Mod1"), Nothing)
                    objServiceTypeDataObject.ModifierTwo = Convert.ToString(drSql("Mod2"), Nothing)
                    objServiceTypeDataObject.ModifierThree = Convert.ToString(drSql("Mod3"), Nothing)
                    objServiceTypeDataObject.ModifierFour = Convert.ToString(drSql("Mod4"), Nothing)

                    ServiceTypeList.Add(objServiceTypeDataObject)
                End While
                objServiceTypeDataObject = Nothing
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return ServiceTypeList

        End Function
    End Class
End Namespace

