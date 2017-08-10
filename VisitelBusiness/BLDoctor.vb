#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Doctor Information Data Fetching
' Author: Anjan Kumar Paul
' Start Date: 31 Aug 2014
' End Date: 31 Aug 2014
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                31 Aug 2014     Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelDA.VisitelDA

Namespace VisitelBusiness
    Public Class BLDoctor

        Public Function SelectDoctor(ConVisitel As SqlConnection, CompanyId As Integer) As List(Of DoctorDataObject)

            Dim drSql As SqlDataReader = Nothing
            Dim parameters As New HybridDictionary()
            parameters.Add("@CompanyId", CompanyId)

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectDoctor]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim DoctorList As New List(Of DoctorDataObject)()

            If drSql.HasRows Then
                Dim objDoctorDataObject As DoctorDataObject
                While drSql.Read()
                    objDoctorDataObject = New DoctorDataObject()

                    objDoctorDataObject.DoctorId = If((DBNull.Value.Equals(drSql("DoctorId"))), objDoctorDataObject.DoctorId, Convert.ToInt32(drSql("DoctorId")))
                    objDoctorDataObject.DoctorName = Convert.ToString(drSql("DoctorName"), Nothing)

                    DoctorList.Add(objDoctorDataObject)
                End While
                objDoctorDataObject = Nothing
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return DoctorList

        End Function
    End Class
End Namespace

