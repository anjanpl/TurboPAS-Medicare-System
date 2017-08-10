Namespace VisitelBusiness.DataObject
    Public Class ClientCaseDataObject
        Inherits BaseDataObject

        Public Property ClientId As Int64

        Public Property StateClientId As Integer

        Public Property BeginDate As String

        Public Property EndDate As String

        Public Property DoctorId As Integer

        Public Property Type As Integer

        Public Property CaseWorkerId As Integer

        Public Property Units As String

        Public Property WeekDays As Integer

        Public Property InsuranceNumber As String

        Public Property ReferralNumber As String

        Public Property UpdateBy As String

        Public Property UpdateDate As String

        Public Property CompanyId As Integer

        Public Property UserId As Integer

        Public Sub New()

            Me.StateClientId = InlineAssignHelper(Me.DoctorId, InlineAssignHelper(Me.Type, InlineAssignHelper(Me.CaseWorkerId, InlineAssignHelper(Me.WeekDays,
                         InlineAssignHelper(Me.CompanyId, InlineAssignHelper(Me.UserId, Int32.MinValue))))))

            Me.ClientId = Int64.MinValue

            Me.BeginDate = InlineAssignHelper(Me.EndDate, InlineAssignHelper(Me.EndDate, InlineAssignHelper(Me.InsuranceNumber,
                           InlineAssignHelper(Me.ReferralNumber, InlineAssignHelper(Me.UpdateBy, InlineAssignHelper(Me.UpdateDate,
                           InlineAssignHelper(Me.Units, String.Empty)))))))
        End Sub
    End Class

    Public Class ClientCaseCareInfoDataObject : Inherits ClientCaseDataObject

        Public Property SupervisorLastVisitDate As String

        Public Property PlanOfCareStartDate As String

        Public Property PlanOfCareEndDate As String

        Public Property PlanOfCareUnits As Double

        Public Property SupervisorVisitFrequency As Integer

        Public Property Supplies As String

        Public Property AuthorizationReceivedDate As String

        Public Property DoctorOrderSentDate As String

        Public Property DoctorOrderReceivedDate As String

        Public Property ServiceInitializedReportedDate As String

        Public Sub New()
            Me.SupervisorLastVisitDate = InlineAssignHelper(PlanOfCareStartDate, InlineAssignHelper(PlanOfCareEndDate,
                                         InlineAssignHelper(Supplies, InlineAssignHelper(AuthorizationReceivedDate,
                                         InlineAssignHelper(DoctorOrderSentDate, InlineAssignHelper(DoctorOrderReceivedDate,
                                         InlineAssignHelper(ServiceInitializedReportedDate, String.Empty)))))))

            Me.PlanOfCareUnits = Double.MinValue
            Me.SupervisorVisitFrequency = Int32.MinValue
        End Sub
    End Class

    Public Class ClientCaseBillingInfoDataObject : Inherits ClientCaseDataObject

        Public Property AuthorizationNumber As String

        Public Property AssessmentDate As String

        Public Property DiagnosisCodeOne As String

        Public Property Comments As String

        Public Property DiagnosisCodeTwo As String

        Public Property DiagnosisCodeThree As String

        Public Property DiagnosisCodeFour As String

        Public Property ProcedureId As String

        Public Property PlaceOfServiceId As String

        Public Property ModifierOne As String

        Public Property ModifierTwo As String

        Public Property ModifierThree As String

        Public Property ModifierFour As String

        Public Property UnitRate As Decimal

        Public Property TransactionRegistryStatus As String

        Public Property CLM0503ClmFrequencyTypeCode As String

        Public Property CLM06ProviderSignatureOnFile As String

        Public Property CLM07MedicareAssignmentCode As String

        Public Property CLM08AssignmentOfBenefitsIndicator As String

        Public Property CLM09ReleaseOfInfoCode As String

        Public Property CLM10PatientSignatureCode As String

        Public Property RightsNotificationDate As String

        Public Property EligibilityExpireDate As String

        Public Property AssignRecipientId As String

        Public Sub New()

            Me.AssessmentDate = InlineAssignHelper(DiagnosisCodeOne, InlineAssignHelper(DiagnosisCodeTwo, InlineAssignHelper(DiagnosisCodeThree,
                                InlineAssignHelper(DiagnosisCodeFour, InlineAssignHelper(ProcedureId, InlineAssignHelper(PlaceOfServiceId,
                                InlineAssignHelper(ModifierOne, InlineAssignHelper(ModifierTwo, InlineAssignHelper(ModifierThree,
                                InlineAssignHelper(ModifierFour, InlineAssignHelper(TransactionRegistryStatus,
                                InlineAssignHelper(CLM0503ClmFrequencyTypeCode, InlineAssignHelper(CLM06ProviderSignatureOnFile,
                                InlineAssignHelper(CLM07MedicareAssignmentCode, InlineAssignHelper(CLM08AssignmentOfBenefitsIndicator,
                                InlineAssignHelper(CLM09ReleaseOfInfoCode, InlineAssignHelper(CLM10PatientSignatureCode,
                                InlineAssignHelper(RightsNotificationDate, InlineAssignHelper(EligibilityExpireDate,
                                InlineAssignHelper(AssignRecipientId, InlineAssignHelper(AuthorizationNumber,
                                InlineAssignHelper(Comments, String.Empty))))))))))))))))))))))

            Me.UnitRate = Decimal.MinValue
        End Sub
    End Class

    Public Class ClientCaseEVVInfoDataObject : Inherits ClientCaseDataObject

        Public Property EVVClientId As String

        Public Property ServiceCode As String

        Public Property ServiceCodeDescription As String

        Public Property ServiceGroup As String

        Public Property EVVARNumber As String

        Public Property EVVProgramCode As String

        Public Property EVVPriority As Integer

        Public Property EVVStatus As String

        Public Property BillCode As String

        Public Property ProcedureCodeQualifier As String

        Public Property LandPhone As String

        Public Sub New()
            Me.EVVClientId = InlineAssignHelper(Me.ServiceCode, InlineAssignHelper(Me.ServiceCodeDescription,
                             InlineAssignHelper(Me.ServiceGroup, InlineAssignHelper(Me.EVVARNumber,
                             InlineAssignHelper(Me.EVVProgramCode, InlineAssignHelper(Me.EVVStatus,
                             InlineAssignHelper(Me.BillCode, InlineAssignHelper(Me.ProcedureCodeQualifier, String.Empty))))))))

            Me.EVVPriority = Int32.MinValue
        End Sub
    End Class
End Namespace

