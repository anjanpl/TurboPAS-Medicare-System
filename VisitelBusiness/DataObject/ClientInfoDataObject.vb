Namespace VisitelBusiness.DataObject

    Public Class ClientInfoDataObject
        Inherits BaseDataObject

        Public Property ClientInfoId As Int64

        Public Property StateClientId As Integer

        Public Property Comments As String

        Public Property UpdateDate As String

        Public Property UpdateBy As String

        Public Property CompanyId As Integer

        Public Property UserId As Integer

        Public Sub New()
            Me.StateClientId = InlineAssignHelper(Me.CompanyId, InlineAssignHelper(Me.UserId, Int32.MinValue))

            Me.ClientInfoId = Int64.MinValue

            Me.UpdateDate = InlineAssignHelper(Me.Comments, InlineAssignHelper(Me.UpdateDate, InlineAssignHelper(Me.UpdateBy, String.Empty)))
        End Sub
    End Class

    Public Class ClientBasicInfoDataObject : Inherits ClientInfoDataObject

        Public Property FirstName As String

        Public Property LastName As String

        Public Property MiddleNameInitial As String

        Public Property Phone As String

        Public Property AlternatePhone As String

        Public Property LandPhone As String

        Public Property Priority As Int16

        Public Property DateOfBirth As String

        Public Property SocialSecurityNumber As String

        Public Property Address As String

        Public Property ApartmentNumber As String

        Public Property CityId As Integer

        Public Property StateId As Integer

        Public Property Zip As String

        Public Property Status As Int16

        Public Property Gender As String

        Public Property MaritalStatusId As Integer

        Public Sub New()
            Me.MaritalStatusId = InlineAssignHelper(Me.CityId, Int32.MinValue)

            Me.FirstName = InlineAssignHelper(Me.LastName, InlineAssignHelper(Me.Address, InlineAssignHelper(Me.AlternatePhone,
                           InlineAssignHelper(Me.ApartmentNumber, InlineAssignHelper(Me.Comments, InlineAssignHelper(Me.DateOfBirth,
                           InlineAssignHelper(Me.Gender, InlineAssignHelper(Me.LandPhone, String.Empty))))))))

            Me.Status = Int16.MinValue
        End Sub
    End Class

    Public Class ClientTreatmentInfoDataObject : Inherits ClientInfoDataObject

        Public Property ServiceStartDate As String

        Public Property ServiceEndDate As String

        Public Property DischargeReason As Integer

        Public Property Diagnosis As String

        Public Property CountyId As Integer

        Public Property DoctorId As Integer

        Public Property DisasterCategory As String

        Public Property Supervisor As String

        Public Property Liaison As String

        Public Property Comments As String

        Public Sub New()
            Me.DoctorId = InlineAssignHelper(Me.DischargeReason, InlineAssignHelper(Me.CountyId, Int32.MinValue))

            Me.ServiceStartDate = InlineAssignHelper(Me.Comments, InlineAssignHelper(Me.Diagnosis, InlineAssignHelper(Me.DisasterCategory,
                                  InlineAssignHelper(Me.ServiceEndDate, InlineAssignHelper(Me.Liaison, String.Empty)))))
        End Sub
    End Class

    Public Class ClientEmergencyContactInfoDataObject : Inherits ClientInfoDataObject

        Public Property EmergencyContact As String

        Public Property EmergencyContactPhone As String

        Public Property EmergencyContactRelationship As String

        Public Property EmergencyContactTwo As String

        Public Property EmergencyContactPhoneTwo As String

        Public Property EmergencyContactRelationshipTwo As String

        Public Sub New()
            Me.EmergencyContact = InlineAssignHelper(Me.EmergencyContactPhone, InlineAssignHelper(Me.EmergencyContactPhoneTwo,
                                  InlineAssignHelper(Me.EmergencyContactRelationship, InlineAssignHelper(Me.EmergencyContactRelationshipTwo,
                                  InlineAssignHelper(Me.EmergencyContactTwo, InlineAssignHelper(Me.UpdateDate, InlineAssignHelper(Me.UpdateBy, String.Empty)))))))
        End Sub
    End Class

    Public Class ClientServiceInfoDataObject : Inherits ClientInfoDataObject

        Public Property Name As String

        Public Property ServiceCodeDescription As String

        Public Property ClientId As Integer

        Public Property Status As String

        Public Property AuthorizationNumber As String

        Public Property AuthorizationStartDate As String

        Public Property AuthorizationEndDate As String

        Public Sub New()
            Me.Name = InlineAssignHelper(Me.ServiceCodeDescription, InlineAssignHelper(Me.Status, InlineAssignHelper(Me.AuthorizationNumber, _
                      InlineAssignHelper(Me.AuthorizationStartDate, InlineAssignHelper(Me.AuthorizationEndDate, String.Empty)))))

            Me.ClientInfoId = Int32.MinValue
        End Sub
    End Class

End Namespace

