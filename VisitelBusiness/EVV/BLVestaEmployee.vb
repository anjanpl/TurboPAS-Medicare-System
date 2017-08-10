#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Vesta Employee Data Fetch, Insert, Update
' Author: Anjan Kumar Paul
' Start Date: 26 Dec 2015
' End Date: 26 Dec 2015
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                26 Dec 2015     Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelDA.VisitelDA
Imports VisitelBusiness.VisitelBusiness.DataObject.EVV

Namespace VisitelBusiness
    Public Class BLVestaEmployee

        Public Sub InsertEmployeeInfo(ConVisitel As SqlConnection, objEmployeeDataObject As EmployeeDataObject)

            Dim parameters As New HybridDictionary()

            SetParameters(parameters, objEmployeeDataObject)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertVestaEmployeeInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub UpdateEmployeeInfo(ConVisitel As SqlConnection, objEmployeeDataObject As EmployeeDataObject)

            Dim parameters As New HybridDictionary()

            parameters.Add("@Id", objEmployeeDataObject.Id)

            SetParameters(parameters, objEmployeeDataObject)

            parameters.Add("@UpdateBy", objEmployeeDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateVestaEmployeeInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Private Sub SetParameters(parameters As HybridDictionary, objEmployeeDataObject As EmployeeDataObject)

            parameters.Add("@MyUniqueId", objEmployeeDataObject.MyUniqueId)

            If (Not String.IsNullOrEmpty(objEmployeeDataObject.EVVId)) Then
                parameters.Add("@EVVId", objEmployeeDataObject.EVVId)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeDataObject.EmployeeNumberVesta)) Then
                parameters.Add("@EmployeeNumberVesta", objEmployeeDataObject.EmployeeNumberVesta)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeDataObject.EmployeeLastName)) Then
                parameters.Add("@EmployeeLastName", objEmployeeDataObject.EmployeeLastName)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeDataObject.EmployeeFirstName)) Then
                parameters.Add("@EmployeeFirstName", objEmployeeDataObject.EmployeeFirstName)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeDataObject.EmployeePhone)) Then
                parameters.Add("@EmployeePhone", objEmployeeDataObject.EmployeePhone)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeDataObject.EmployeeSSNumber)) Then
                parameters.Add("@EmployeeSSNumber", objEmployeeDataObject.EmployeeSSNumber)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeDataObject.EmployeePassport)) Then
                parameters.Add("@EmployeePassport", objEmployeeDataObject.EmployeePassport)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeDataObject.EmployeeStartDate)) Then
                parameters.Add("@EmployeeStartDate", objEmployeeDataObject.EmployeeStartDate)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeDataObject.EmployeeEndDate)) Then
                parameters.Add("@EmployeeEndDate", objEmployeeDataObject.EmployeeEndDate)
            End If

            parameters.Add("@EmployeeDiscipline", objEmployeeDataObject.EmployeeDiscipline)
            parameters.Add("@EmployeeId", objEmployeeDataObject.EmployeeId)

            'If (Not String.IsNullOrEmpty(objEmployeeDataObject.EmployeeStatus)) Then
            '    parameters.Add("@EmployeeStatus", objEmployeeDataObject.EmployeeStatus)
            'End If

            If (Not String.IsNullOrEmpty(objEmployeeDataObject.BranchName)) Then
                parameters.Add("@BranchName", objEmployeeDataObject.BranchName)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeDataObject.Error)) Then
                parameters.Add("@Error", objEmployeeDataObject.Error)
            End If

        End Sub

        Public Sub DeleteEmployeeInfo(ConVisitel As SqlConnection, Id As Int64, DeletedBy As String)

            Dim parameters As New HybridDictionary()

            parameters.Add("@Id", Id)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteVestaEmployeeInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Function SelectEmployeeInfo(ConVisitel As SqlConnection, Optional EmployeeId As Integer = Integer.MinValue) As List(Of EmployeeDataObject)

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()

            If (EmployeeId > 0) Then
                parameters.Add("@EmployeeId", EmployeeId)
            End If

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectVestaEmployee]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim Employees As New List(Of EmployeeDataObject)()

            If drSql.HasRows Then
                Dim objEmployeeDataObject As EmployeeDataObject
                While drSql.Read()
                    objEmployeeDataObject = New EmployeeDataObject()

                    objEmployeeDataObject.Id = If((DBNull.Value.Equals(drSql("ID"))), objEmployeeDataObject.Id, Convert.ToInt32(drSql("ID")))

                    objEmployeeDataObject.MyUniqueId = Convert.ToString(drSql("MyUniqueID"), Nothing)
                    objEmployeeDataObject.EVVId = Convert.ToString(drSql("EVV_ID"), Nothing)
                    objEmployeeDataObject.EmployeeNumberVesta = Convert.ToString(drSql("Emp_Number_Vesta"), Nothing)
                    objEmployeeDataObject.EmployeeLastName = Convert.ToString(drSql("Emp_LastName"), Nothing)
                    objEmployeeDataObject.EmployeeFirstName = Convert.ToString(drSql("Emp_FirstName"), Nothing)
                    objEmployeeDataObject.EmployeePhone = Convert.ToString(drSql("Emp_Phone"), Nothing)
                    objEmployeeDataObject.EmployeeSSNumber = Convert.ToString(drSql("Emp_SS_Number"), Nothing)
                    objEmployeeDataObject.EmployeePassport = Convert.ToString(drSql("Emp_Passport"), Nothing)
                    objEmployeeDataObject.EmployeeStartDate = Convert.ToString(drSql("Emp_Start_Date"), Nothing)
                    objEmployeeDataObject.EmployeeEndDate = Convert.ToString(drSql("Emp_End_Date"), Nothing)
                    objEmployeeDataObject.EmployeeDiscipline = Convert.ToString(drSql("Emp_Discipline"), Nothing)
                    objEmployeeDataObject.BranchName = Convert.ToString(drSql("Branch_Name"), Nothing)
                    objEmployeeDataObject.[Error] = Convert.ToString(drSql("Error"), Nothing)

                    objEmployeeDataObject.EmployeeId = If((DBNull.Value.Equals(drSql("emp_id"))), objEmployeeDataObject.EmployeeId, Convert.ToInt64(drSql("emp_id"), Nothing))

                    objEmployeeDataObject.UpdateDate = Convert.ToString(drSql("update_date"), Nothing)
                    objEmployeeDataObject.UpdateBy = Convert.ToString(drSql("update_by"), Nothing)

                    Employees.Add(objEmployeeDataObject)

                End While
                objEmployeeDataObject = Nothing
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return Employees

        End Function
    End Class
End Namespace

