
#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Employee Information Inserting, Updating, Deleting, & Fetching
' Author: Anjan Kumar Paul
' Start Date: 03 Oct 2014
' End Date: 
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                03 Oct 2014     Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.DataObject.Settings
Imports VisitelDA.VisitelDA

Namespace VisitelBusiness.Settings
    Public Class BLEmployeeSetting
        Inherits BLCommon
        Public Sub InsertEmployeeInfo(ConVisitel As SqlConnection, objEmployeeSettingDataObject As EmployeeSettingDataObject)
            Dim parameters As New HybridDictionary()

            SetParameters(parameters, objEmployeeSettingDataObject)

            parameters.Add("@CompanyId", objEmployeeSettingDataObject.CompanyId)
            parameters.Add("@UserId", objEmployeeSettingDataObject.UserId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertEmployeeInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        Public Sub UpdateEmployeeInfo(ConVisitel As SqlConnection, objEmployeeSettingDataObject As EmployeeSettingDataObject)
            Dim parameters As New HybridDictionary()

            parameters.Add("@EmployeeId", objEmployeeSettingDataObject.EmployeeId)

            SetParameters(parameters, objEmployeeSettingDataObject)

            parameters.Add("@UpdateBy", objEmployeeSettingDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateEmployeeInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub DeleteEmployeeInfo(ConVisitel As SqlConnection, CompanyId As Integer, EmployeeId As Integer, DeletedBy As Integer)
            Dim parameters As New HybridDictionary()

            parameters.Add("@CompanyId", CompanyId)
            parameters.Add("@EmployeeId", EmployeeId)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteEmployeeInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        Public Function SelectEmployeeInfo(ConVisitel As SqlConnection, CompanyId As Integer, EmployeeId As Integer, SocialSecurityNumber As String) As EmployeeSettingDataObject

            Dim drSql As SqlDataReader = Nothing
            Dim parameters As New HybridDictionary()

            parameters.Add("@CompanyId", CompanyId)

            If (EmployeeId > 0) Then
                parameters.Add("@EmployeeId", EmployeeId)
            End If

            If (Not String.IsNullOrEmpty(SocialSecurityNumber)) Then
                parameters.Add("@SocialSecurityNumber", SocialSecurityNumber)
            End If


            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SelectEmployeeInfo]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim objEmployeeSettingDataObject As New EmployeeSettingDataObject()

            If drSql.HasRows Then
                If drSql.Read() Then

                    objEmployeeSettingDataObject.EmployeeId = Convert.ToInt32(drSql("EmployeeId"))

                    objEmployeeSettingDataObject.EmploymentStatusId = If((Not DBNull.Value.Equals(drSql("EmploymentStatusId"))),
                                                                Convert.ToString(drSql("EmploymentStatusId"), Nothing),
                                                                objEmployeeSettingDataObject.EmploymentStatusId)

                    objEmployeeSettingDataObject.LicenseStatus = If((Not DBNull.Value.Equals(drSql("LicenseStatus"))),
                                                                    Convert.ToInt32(drSql("LicenseStatus")),
                                                                    objEmployeeSettingDataObject.LicenseStatus)

                    objEmployeeSettingDataObject.ClientGroupId = If((Not DBNull.Value.Equals(drSql("ClientGroupId"))),
                                                                    Convert.ToInt32(drSql("ClientGroupId"), Nothing),
                                                                    objEmployeeSettingDataObject.ClientGroupId)

                    objEmployeeSettingDataObject.OIGResult = Convert.ToString(drSql("OIGResult"), Nothing).Trim()

                    objEmployeeSettingDataObject.Title = If((Not DBNull.Value.Equals(drSql("Title"))), Convert.ToInt32(drSql("Title")), objEmployeeSettingDataObject.Title)

                    objEmployeeSettingDataObject.FirstName = Convert.ToString(drSql("FirstName"), Nothing).Trim()
                    objEmployeeSettingDataObject.LastName = Convert.ToString(drSql("LastName"), Nothing).Trim()
                    objEmployeeSettingDataObject.MiddleNameInitial = Convert.ToString(drSql("MiddleNameInitial"), Nothing).Trim()
                    objEmployeeSettingDataObject.Address = Convert.ToString(drSql("Address"), Nothing).Trim()
                    objEmployeeSettingDataObject.ApartmentNumber = Convert.ToString(drSql("ApartmentNumber"), Nothing).Trim()

                    objEmployeeSettingDataObject.CityId = If((Not DBNull.Value.Equals(drSql("CityId"))), Convert.ToInt32(drSql("CityId")), objEmployeeSettingDataObject.CityId)
                    objEmployeeSettingDataObject.StateId = If((Not DBNull.Value.Equals(drSql("StateId"))), Convert.ToInt32(drSql("StateId")), objEmployeeSettingDataObject.StateId)
                    objEmployeeSettingDataObject.Zip = If((Not DBNull.Value.Equals(drSql("Zip"))), Convert.ToInt32(drSql("Zip")), objEmployeeSettingDataObject.Zip)

                    'objEmployeeSettingDataObject.Phone = GetFormattedMobileNumber(Convert.ToString(drSql("Phone"), Nothing).Trim())
                    objEmployeeSettingDataObject.Phone = Convert.ToString(drSql("Phone"), Nothing).Trim()

                    'objEmployeeSettingDataObject.AlternatePhone = GetFormattedMobileNumber(Convert.ToString(drSql("AlternatePhone"), Nothing).Trim())
                    objEmployeeSettingDataObject.AlternatePhone = Convert.ToString(drSql("AlternatePhone"), Nothing).Trim()

                    'objEmployeeSettingDataObject.SocialSecurityNumber = GetFormattedSocialSecurityNumber(Convert.ToString(drSql("SocialSecurityNumber"), Nothing).Trim())
                    objEmployeeSettingDataObject.SocialSecurityNumber = Convert.ToString(drSql("SocialSecurityNumber"), Nothing).Trim()


                    objEmployeeSettingDataObject.DateOfBirth = If((Not DBNull.Value.Equals(drSql("DateOfBirth"))),
                                                                  Convert.ToString(drSql("DateOfBirth"), Nothing).Trim(),
                                                                  objEmployeeSettingDataObject.DateOfBirth)

                    objEmployeeSettingDataObject.Gender = If((Not DBNull.Value.Equals(drSql("Gender"))), Convert.ToInt32(drSql("Gender")), objEmployeeSettingDataObject.Gender)

                    objEmployeeSettingDataObject.MaritalStatus = If((Not DBNull.Value.Equals(drSql("MaritalStatus"))),
                                                                    Convert.ToInt32(drSql("MaritalStatus")),
                                                                    objEmployeeSettingDataObject.MaritalStatus)

                    objEmployeeSettingDataObject.NumberOfVerifiedReference = If((Not DBNull.Value.Equals(drSql("NumberOfVerifiedReference"))),
                                                                                Convert.ToInt32(drSql("NumberOfVerifiedReference"), Nothing),
                                                                                objEmployeeSettingDataObject.NumberOfVerifiedReference)

                    objEmployeeSettingDataObject.NumberOfDepartment = If((Not DBNull.Value.Equals(drSql("NumberOfDepartment"))),
                                                                         Convert.ToInt32(drSql("NumberOfDepartment"), Nothing),
                                                                         objEmployeeSettingDataObject.NumberOfDepartment)

                    objEmployeeSettingDataObject.Payrate = If((Not DBNull.Value.Equals(drSql("Payrate"))),
                                                              Convert.ToDecimal(drSql("Payrate"), Nothing),
                                                              objEmployeeSettingDataObject.Payrate)

                    objEmployeeSettingDataObject.RehireYesNo = If((Not DBNull.Value.Equals(drSql("RehireYesNo"))),
                                                                  Convert.ToInt16(drSql("RehireYesNo"), Nothing),
                                                                  objEmployeeSettingDataObject.RehireYesNo)

                    objEmployeeSettingDataObject.Comments = Convert.ToString(drSql("Comments"), Nothing).Trim()

                    objEmployeeSettingDataObject.MailCheck = If((Not DBNull.Value.Equals(drSql("MailCheck"))),
                                                                Convert.ToInt16(drSql("MailCheck"), Nothing),
                                                                objEmployeeSettingDataObject.MailCheck)

                    objEmployeeSettingDataObject.ApplicationDate = If((Not DBNull.Value.Equals(drSql("ApplicationDate"))),
                                                                      Convert.ToString(drSql("ApplicationDate"), Nothing).Trim(),
                                                                      objEmployeeSettingDataObject.ApplicationDate)

                    objEmployeeSettingDataObject.ReferenceVerificationDate = If((Not DBNull.Value.Equals(drSql("ReferenceVerificationDate"))),
                                                                                Convert.ToString(drSql("ReferenceVerificationDate"), Nothing).Trim(),
                                                                                objEmployeeSettingDataObject.ReferenceVerificationDate)

                    objEmployeeSettingDataObject.HireDate = If((Not DBNull.Value.Equals(drSql("HireDate"))),
                                                               Convert.ToString(drSql("HireDate"), Nothing).Trim(),
                                                               objEmployeeSettingDataObject.HireDate)

                    objEmployeeSettingDataObject.SignedJobDescriptionDate = If((Not DBNull.Value.Equals(drSql("SignedJobDescriptionDate"))),
                                                                               Convert.ToString(drSql("SignedJobDescriptionDate"), Nothing).Trim(),
                                                                               objEmployeeSettingDataObject.SignedJobDescriptionDate)

                    objEmployeeSettingDataObject.OrientationDate = If((Not DBNull.Value.Equals(drSql("OrientationDate"))),
                                                                      Convert.ToString(drSql("OrientationDate"), Nothing).Trim(),
                                                                      objEmployeeSettingDataObject.OrientationDate)

                    objEmployeeSettingDataObject.AssignedTaskEvaluationDate = If((Not DBNull.Value.Equals(drSql("AssignedTaskEvaluationDate"))),
                                                                                 Convert.ToString(drSql("AssignedTaskEvaluationDate"), Nothing).Trim(),
                                                                                 objEmployeeSettingDataObject.AssignedTaskEvaluationDate)

                    objEmployeeSettingDataObject.CrimcheckDate = If((Not DBNull.Value.Equals(drSql("CrimcheckDate"))),
                                                                    Convert.ToString(drSql("CrimcheckDate"), Nothing).Trim(),
                                                                    objEmployeeSettingDataObject.CrimcheckDate)

                    objEmployeeSettingDataObject.RegistryDate = If((Not DBNull.Value.Equals(drSql("RegistryDate"))),
                                                                   Convert.ToString(drSql("RegistryDate"), Nothing).Trim(),
                                                                   objEmployeeSettingDataObject.RegistryDate)

                    objEmployeeSettingDataObject.LastEvaluationDate = If((Not DBNull.Value.Equals(drSql("LastEvaluationDate"))),
                                                                         Convert.ToString(drSql("LastEvaluationDate"), Nothing).Trim(),
                                                                         objEmployeeSettingDataObject.LastEvaluationDate)

                    objEmployeeSettingDataObject.PolicyOrProcedureSettlementDate = If((Not DBNull.Value.Equals(drSql("PolicyOrProcedureSettlementDate"))),
                                                                                      Convert.ToString(drSql("PolicyOrProcedureSettlementDate"), Nothing).Trim(),
                                                                                      objEmployeeSettingDataObject.PolicyOrProcedureSettlementDate)

                    objEmployeeSettingDataObject.OIGDate = If((Not DBNull.Value.Equals(drSql("OIGDate"))),
                                                              Convert.ToString(drSql("OIGDate"), Nothing).Trim(),
                                                              objEmployeeSettingDataObject.OIGDate)

                    objEmployeeSettingDataObject.OIGReportedToStateDate = If((Not DBNull.Value.Equals(drSql("OIGReportedToStateDate"))),
                                                                             Convert.ToString(drSql("OIGReportedToStateDate"), Nothing).Trim(),
                                                                             objEmployeeSettingDataObject.OIGReportedToStateDate)

                    objEmployeeSettingDataObject.StartDate = If((Not DBNull.Value.Equals(drSql("StartDate"))),
                                                                Convert.ToString(drSql("StartDate"), Nothing).Trim(),
                                                                objEmployeeSettingDataObject.StartDate)

                    objEmployeeSettingDataObject.EndDate = If((Not DBNull.Value.Equals(drSql("EndDate"))),
                                                              Convert.ToString(drSql("EndDate"), Nothing).Trim(),
                                                              objEmployeeSettingDataObject.EndDate)

                    objEmployeeSettingDataObject.SantraxCDSPayrate = If((Not DBNull.Value.Equals(drSql("SantraxCDSPayrate"))),
                                                              Convert.ToDecimal(drSql("SantraxCDSPayrate"), Nothing),
                                                              objEmployeeSettingDataObject.SantraxCDSPayrate)

                    objEmployeeSettingDataObject.SantraxEmployeeId = Convert.ToString(drSql("SantraxEmployeeId"), Nothing).Trim()
                    'objEmployeeSettingDataObject.SantraxGPSPhone = GetFormattedMobileNumber(Convert.ToString(drSql("SantraxGPSPhone"), Nothing).Trim())
                    objEmployeeSettingDataObject.SantraxGPSPhone = Convert.ToString(drSql("SantraxGPSPhone"), Nothing).Trim()
                    objEmployeeSettingDataObject.SantraxDiscipline = Convert.ToString(drSql("SantraxDiscipline"), Nothing).Trim()

                    objEmployeeSettingDataObject.PayrollNumber = Convert.ToString(drSql("PayrollNumber"), Nothing).Trim()
                    objEmployeeSettingDataObject.EmployeeEVVId = Convert.ToString(drSql("EmployeeEVVId"), Nothing).Trim()
                    objEmployeeSettingDataObject.VendorEmployeeId = Convert.ToString(drSql("VendorEmployeeId"), Nothing).Trim()

                    objEmployeeSettingDataObject.UpdateDate = If((Not DBNull.Value.Equals(drSql("UpdateDate"))),
                                                                 Convert.ToString(drSql("UpdateDate"), Nothing),
                                                                 objEmployeeSettingDataObject.UpdateDate)

                    objEmployeeSettingDataObject.UpdateBy = If((Not DBNull.Value.Equals(drSql("UpdateBy"))),
                                                               Convert.ToString(drSql("UpdateBy"), Nothing),
                                                               objEmployeeSettingDataObject.UpdateBy)

                End If

            End If

            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return objEmployeeSettingDataObject

        End Function

        Private Sub SetParameters(parameters As HybridDictionary, objEmployeeSettingDataObject As EmployeeSettingDataObject)

            If (Not String.IsNullOrEmpty(objEmployeeSettingDataObject.FirstName)) Then
                parameters.Add("@FirstName", objEmployeeSettingDataObject.FirstName)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeSettingDataObject.LastName)) Then
                parameters.Add("@LastName", objEmployeeSettingDataObject.LastName)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeSettingDataObject.MiddleNameInitial)) Then
                parameters.Add("@MiddleNameInitial", objEmployeeSettingDataObject.MiddleNameInitial)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeSettingDataObject.Address)) Then
                parameters.Add("@Address", objEmployeeSettingDataObject.Address)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeSettingDataObject.ApartmentNumber)) Then
                parameters.Add("@ApartmentNumber", objEmployeeSettingDataObject.ApartmentNumber)
            End If

            If (Not objEmployeeSettingDataObject.CityId.Equals(-1)) Then
                parameters.Add("@CityId", objEmployeeSettingDataObject.CityId)
            End If

            If (Not objEmployeeSettingDataObject.StateId.Equals(-1)) Then
                parameters.Add("@StateId", objEmployeeSettingDataObject.StateId)
            End If

            If (Not objEmployeeSettingDataObject.Zip.Equals(Int32.MinValue)) Then
                parameters.Add("@Zip", objEmployeeSettingDataObject.Zip)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeSettingDataObject.Phone)) Then
                parameters.Add("@Phone", objEmployeeSettingDataObject.Phone)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeSettingDataObject.AlternatePhone)) Then
                parameters.Add("@AlternatePhone", objEmployeeSettingDataObject.AlternatePhone)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeSettingDataObject.SocialSecurityNumber)) Then
                parameters.Add("@SocialSecurityNumber", objEmployeeSettingDataObject.SocialSecurityNumber)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeSettingDataObject.DateOfBirth)) Then
                parameters.Add("@DateOfBirth", objEmployeeSettingDataObject.DateOfBirth)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeSettingDataObject.StartDate)) Then
                parameters.Add("@StartDate", objEmployeeSettingDataObject.StartDate)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeSettingDataObject.EndDate)) Then
                parameters.Add("@EndDate", objEmployeeSettingDataObject.EndDate)
            End If

            If (Not objEmployeeSettingDataObject.EmploymentStatusId.Equals(Int32.MinValue)) Then
                parameters.Add("@EmploymentStatusId", objEmployeeSettingDataObject.EmploymentStatusId)
            End If

            If (Not objEmployeeSettingDataObject.NumberOfVerifiedReference.Equals(Int32.MinValue)) Then
                parameters.Add("@NumberOfVerifiedReference", objEmployeeSettingDataObject.NumberOfVerifiedReference)
            End If

            If (Not objEmployeeSettingDataObject.NumberOfDepartment.Equals(Int32.MinValue)) Then
                parameters.Add("@NumberOfDepartment", objEmployeeSettingDataObject.NumberOfDepartment)
            End If

            parameters.Add("@MaritalStatus", objEmployeeSettingDataObject.MaritalStatus)

            If (Not objEmployeeSettingDataObject.Payrate.Equals(Decimal.MinValue)) Then
                parameters.Add("@Payrate", objEmployeeSettingDataObject.Payrate)
            End If

            If (Not objEmployeeSettingDataObject.Title.Equals(Int32.MinValue)) Then
                parameters.Add("@Title", objEmployeeSettingDataObject.Title)
            End If

            If (Not objEmployeeSettingDataObject.LicenseStatus.Equals(-1)) Then
                parameters.Add("@LicenseStatus", objEmployeeSettingDataObject.LicenseStatus)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeSettingDataObject.ApplicationDate)) Then
                parameters.Add("@ApplicationDate", objEmployeeSettingDataObject.ApplicationDate)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeSettingDataObject.HireDate)) Then
                parameters.Add("@HireDate", objEmployeeSettingDataObject.HireDate)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeSettingDataObject.SignedJobDescriptionDate)) Then
                parameters.Add("@SignedJobDescriptionDate", objEmployeeSettingDataObject.SignedJobDescriptionDate)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeSettingDataObject.ReferenceVerificationDate)) Then
                parameters.Add("@ReferenceVerificationDate", objEmployeeSettingDataObject.ReferenceVerificationDate)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeSettingDataObject.OrientationDate)) Then
                parameters.Add("@OrientationDate", objEmployeeSettingDataObject.OrientationDate)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeSettingDataObject.PolicyOrProcedureSettlementDate)) Then
                parameters.Add("@PolicyOrProcedureSettlementDate", objEmployeeSettingDataObject.PolicyOrProcedureSettlementDate)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeSettingDataObject.AssignedTaskEvaluationDate)) Then
                parameters.Add("@AssignedTaskEvaluationDate", objEmployeeSettingDataObject.AssignedTaskEvaluationDate)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeSettingDataObject.CrimcheckDate)) Then
                parameters.Add("@CrimcheckDate", objEmployeeSettingDataObject.CrimcheckDate)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeSettingDataObject.RegistryDate)) Then
                parameters.Add("@RegistryDate", objEmployeeSettingDataObject.RegistryDate)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeSettingDataObject.LastEvaluationDate)) Then
                parameters.Add("@LastEvaluationDate", objEmployeeSettingDataObject.LastEvaluationDate)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeSettingDataObject.EndDateTwo)) Then
                parameters.Add("@EndDateTwo", objEmployeeSettingDataObject.EndDateTwo)
            End If

            If (Not objEmployeeSettingDataObject.Gender.Equals("-1")) Then
                parameters.Add("@Gender", objEmployeeSettingDataObject.Gender)
            End If

            If (Not objEmployeeSettingDataObject.RehireYesNo.Equals(Int16.MinValue)) Then
                parameters.Add("@RehireYesNo", objEmployeeSettingDataObject.RehireYesNo)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeSettingDataObject.Comments)) Then
                parameters.Add("@Comments", objEmployeeSettingDataObject.Comments)
            End If

            If (Not objEmployeeSettingDataObject.MailCheck.Equals(Int16.MinValue)) Then
                parameters.Add("@MailCheck", objEmployeeSettingDataObject.MailCheck)
            End If

            If (Not objEmployeeSettingDataObject.ClientGroupId.Equals(-1)) Then
                parameters.Add("@ClientGroupId", objEmployeeSettingDataObject.ClientGroupId)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeSettingDataObject.OIGDate)) Then
                parameters.Add("@OIGDate", objEmployeeSettingDataObject.OIGDate)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeSettingDataObject.OIGResult)) Then
                parameters.Add("@OIGResult", objEmployeeSettingDataObject.OIGResult)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeSettingDataObject.OIGReportedToStateDate)) Then
                parameters.Add("@OIGReportedToStateDate", objEmployeeSettingDataObject.OIGReportedToStateDate)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeSettingDataObject.ASGN_EMP_SSN)) Then
                parameters.Add("@ASGN_EMP_SSN", objEmployeeSettingDataObject.ASGN_EMP_SSN)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeSettingDataObject.SantraxEmployeeId)) Then
                parameters.Add("@SantraxEmployeeId", objEmployeeSettingDataObject.SantraxEmployeeId)
            End If

            If (Not objEmployeeSettingDataObject.SantraxCDSPayrate.Equals(Decimal.MinValue)) Then
                parameters.Add("@SantraxCDSPayrate", objEmployeeSettingDataObject.SantraxCDSPayrate)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeSettingDataObject.SantraxGPSPhone)) Then
                parameters.Add("@SantraxGPSPhone", objEmployeeSettingDataObject.SantraxGPSPhone)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeSettingDataObject.SantraxDiscipline)) Then
                parameters.Add("@SantraxDiscipline", objEmployeeSettingDataObject.SantraxDiscipline)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeSettingDataObject.SantraxStatus)) Then
                parameters.Add("@SantraxStatus", objEmployeeSettingDataObject.SantraxStatus)
            End If

            If (Not String.IsNullOrEmpty(objEmployeeSettingDataObject.PayMethod)) Then
                parameters.Add("@PayMethod", objEmployeeSettingDataObject.PayMethod)
            End If

            parameters.Add("@PayrollNumber", objEmployeeSettingDataObject.PayrollNumber)
            parameters.Add("@VendorEmployeeId", objEmployeeSettingDataObject.VendorEmployeeId)
            parameters.Add("@EmployeeEVVId", objEmployeeSettingDataObject.EmployeeEVVId)

        End Sub

    End Class
End Namespace

