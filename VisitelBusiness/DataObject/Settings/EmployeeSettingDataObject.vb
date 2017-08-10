
Namespace VisitelBusiness.DataObject.Settings
    Public Class EmployeeSettingDataObject
        Inherits BaseDataObject

        Public Property EmployeeId As Integer

        Public Property EmployeeName As String

        Public Property Name As String

        Public Property FirstName As String

        Public Property LastName As String

        Public Property MiddleNameInitial As String

        Public Property Address As String

        Public Property ApartmentNumber As String

        Public Property CityId As Integer

        Public Property StateId As Integer

        Public Property Zip As Integer

        Public Property Phone As String

        Public Property AlternatePhone As String

        Public Property SocialSecurityNumber As String

        Public Property DateOfBirth As String

        Public Property StartDate As String

        Public Property EndDate As String

        Public Property EmploymentStatusId As Integer

        Public Property EmploymentStatus As String

        Public Property NumberOfVerifiedReference As Integer

        Public Property NumberOfDepartment As Integer

        Public Property MaritalStatus As Integer

        Public Property Payrate As Decimal

        Public Property Title As Integer

        Public Property LicenseStatus As Integer

        Public Property ApplicationDate As String

        Public Property HireDate As String

        Public Property SignedJobDescriptionDate As String

        Public Property ReferenceVerificationDate As String

        Public Property OrientationDate As String

        Public Property PolicyOrProcedureSettlementDate As String

        Public Property AssignedTaskEvaluationDate As String

        Public Property CrimcheckDate As String

        Public Property RegistryDate As String

        Public Property LastEvaluationDate As String

        Public Property EndDateTwo As String

        Public Property Gender As String

        Public Property RehireYesNo As Int16

        Public Property Comments As String

        Public Property MailCheck As Int16

        Public Property ClientGroupId As Integer

        Public Property OIGDate As String

        Public Property OIGResult As String

        Public Property OIGReportedToStateDate As String

        Public Property ASGN_EMP_SSN As String

        Public Property SantraxEmployeeId As String

        Public Property SantraxCDSPayrate As Decimal

        Public Property SantraxGPSPhone As String

        Public Property SantraxDiscipline As String

        Public Property SantraxStatus As String

        Public Property PayMethod As String

        Public Property PayrollNumber As String

        Public Property EmployeeEVVId As String

        Public Property VendorEmployeeId As String

        Public Property UpdateBy As String

        Public Property UpdateDate As String

        Public Property CompanyId As Integer

        Public Property UserId As Integer

        Public Sub New()
            Me.EmployeeId = InlineAssignHelper(Me.Zip, InlineAssignHelper(Me.NumberOfVerifiedReference,
                            InlineAssignHelper(Me.NumberOfDepartment, InlineAssignHelper(Me.MaritalStatus,
                            InlineAssignHelper(Me.Title, InlineAssignHelper(Me.LicenseStatus, InlineAssignHelper(Me.CompanyId,
                            InlineAssignHelper(Me.UserId, InlineAssignHelper(Me.CityId, InlineAssignHelper(Me.StateId,
                            InlineAssignHelper(Me.ClientGroupId, InlineAssignHelper(Me.EmploymentStatusId, Int32.MinValue))))))))))))

            Me.FirstName = InlineAssignHelper(Me.LastName, InlineAssignHelper(Me.MiddleNameInitial, InlineAssignHelper(Me.Address,
                           InlineAssignHelper(Me.ApartmentNumber, InlineAssignHelper(Me.Phone, InlineAssignHelper(Me.AlternatePhone,
                           InlineAssignHelper(Me.SocialSecurityNumber, InlineAssignHelper(Me.DateOfBirth, InlineAssignHelper(Me.StartDate,
                           InlineAssignHelper(Me.EndDate, InlineAssignHelper(Me.EmploymentStatus, InlineAssignHelper(Me.ApplicationDate,
                           InlineAssignHelper(Me.HireDate, InlineAssignHelper(Me.SignedJobDescriptionDate,
                           InlineAssignHelper(Me.ReferenceVerificationDate, InlineAssignHelper(Me.OrientationDate,
                           InlineAssignHelper(Me.PolicyOrProcedureSettlementDate, InlineAssignHelper(Me.LastEvaluationDate,
                           InlineAssignHelper(Me.CrimcheckDate, InlineAssignHelper(Me.RegistryDate, InlineAssignHelper(Me.LastEvaluationDate,
                           InlineAssignHelper(Me.EndDateTwo, InlineAssignHelper(Me.Gender, InlineAssignHelper(Me.Comments,
                           InlineAssignHelper(Me.OIGDate, InlineAssignHelper(Me.OIGResult, InlineAssignHelper(Me.OIGReportedToStateDate,
                           InlineAssignHelper(Me.ASGN_EMP_SSN, InlineAssignHelper(Me.ASGN_EMP_SSN, InlineAssignHelper(Me.SantraxEmployeeId,
                           InlineAssignHelper(Me.SantraxGPSPhone, InlineAssignHelper(Me.SantraxDiscipline, InlineAssignHelper(Me.SantraxStatus,
                           InlineAssignHelper(Me.PayMethod, InlineAssignHelper(Me.PayrollNumber, InlineAssignHelper(Me.VendorEmployeeId, InlineAssignHelper(Me.EmployeeEVVId,
                           InlineAssignHelper(Me.UpdateBy, InlineAssignHelper(Me.UpdateDate, String.Empty)))))))))))))))))))))))))))))))))))))))

            Me.Payrate = InlineAssignHelper(Me.SantraxCDSPayrate, Decimal.MinValue)

            Me.MailCheck = InlineAssignHelper(Me.RehireYesNo, Int16.MinValue)
        End Sub
    End Class
End Namespace

