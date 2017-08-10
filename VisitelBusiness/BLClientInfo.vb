#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Client Information & Client Case Information Data Inserting, Updating, & Fetching 
' Author: Anjan Kumar Paul
' Start Date: 12 Sept 2014
' End Date: 
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                12 Sept 2014     Initial Development

'-----------------------------------------------------------------------------------------------------------------------------------
#End Region

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelDA.VisitelDA
Imports System.Web.UI.WebControls

Namespace VisitelBusiness
    Public Class BLClientInfo
        Inherits BLCommon

        Public Sub InsertClientInfo(ConVisitel As SqlConnection, objClientInfoDataObject As ClientInfoDataObject, objClientBasicInfoDataObject As ClientBasicInfoDataObject,
            objClientTreatmentInfoDataObject As ClientTreatmentInfoDataObject, objClientEmergencyContactInfoDataObject As ClientEmergencyContactInfoDataObject,
            objClientCaseDataObject As ClientCaseDataObject, objClientCaseCareInfoDataObject As ClientCaseCareInfoDataObject,
            objClientCaseBillingInfoDataObject As ClientCaseBillingInfoDataObject, objClientCaseEVVInfoDataObject As ClientCaseEVVInfoDataObject)

            Dim parameters As New HybridDictionary()

            SetParameters(parameters, objClientInfoDataObject, objClientBasicInfoDataObject, objClientTreatmentInfoDataObject, objClientEmergencyContactInfoDataObject,
                          objClientCaseDataObject, objClientCaseCareInfoDataObject, objClientCaseBillingInfoDataObject, objClientCaseEVVInfoDataObject)

            parameters.Add("@CompanyId", objClientInfoDataObject.CompanyId)
            parameters.Add("@UserId", objClientInfoDataObject.UserId)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.InsertClientInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        Public Sub UpdateClientInfo(ConVisitel As SqlConnection, objClientInfoDataObject As ClientInfoDataObject, objClientBasicInfoDataObject As ClientBasicInfoDataObject,
            objClientTreatmentInfoDataObject As ClientTreatmentInfoDataObject, objClientEmergencyContactInfoDataObject As ClientEmergencyContactInfoDataObject,
            objClientCaseDataObject As ClientCaseDataObject, objClientCaseCareInfoDataObject As ClientCaseCareInfoDataObject,
            objClientCaseBillingInfoDataObject As ClientCaseBillingInfoDataObject, objClientCaseEVVInfoDataObject As ClientCaseEVVInfoDataObject)

            Dim parameters As New HybridDictionary()

            parameters.Add("@ClientInfoId", objClientInfoDataObject.ClientInfoId)
            parameters.Add("@ClientId", objClientCaseDataObject.ClientId)

            SetParameters(parameters, objClientInfoDataObject, objClientBasicInfoDataObject, objClientTreatmentInfoDataObject, objClientEmergencyContactInfoDataObject,
                          objClientCaseDataObject, objClientCaseCareInfoDataObject, objClientCaseBillingInfoDataObject, objClientCaseEVVInfoDataObject)

            parameters.Add("@CompanyId", objClientInfoDataObject.CompanyId)
            parameters.Add("@UserId", objClientInfoDataObject.UserId)
            parameters.Add("@UpdateBy", objClientInfoDataObject.UpdateBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.UpdateClientInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

        End Sub

        Public Sub DeleteClientInfo(ConVisitel As SqlConnection, CompanyId As Integer, ClientInfoId As Int64, ClientId As Int64, DeletedBy As Integer)
            Dim parameters As New HybridDictionary()

            parameters.Add("@CompanyId", CompanyId)
            parameters.Add("@ClientInfoId", ClientInfoId)
            parameters.Add("@ClientId", ClientId)
            parameters.Add("@DeletedBy", DeletedBy)

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.ExecuteQuery("", "[TurboDB.DeleteClientInfo]", ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing
        End Sub

        Public Function SelectClientInfo(ConVisitel As SqlConnection, CompanyId As Integer, ClientInfoId As Int64, SocialSecurityNumber As String) As HybridDictionary

            Dim drSql As SqlDataReader = Nothing

            Dim parameters As New HybridDictionary()

            parameters.Add("@CompanyId", CompanyId)

            If (ClientInfoId > 0) Then
                parameters.Add("@ClientInfoId", ClientInfoId)
            End If

            If (Not String.IsNullOrEmpty(SocialSecurityNumber)) Then
                parameters.Add("@SocialSecurityNumber", SocialSecurityNumber)
            End If

            Dim objSharedSettings As New SharedSettings()

            objSharedSettings.GetDataReader("", "[TurboDB.SelectClientInfo]", drSql, ConVisitel, parameters)

            objSharedSettings = Nothing
            parameters = Nothing

            Dim objClientBasicInfoDataObject As New ClientBasicInfoDataObject()
            Dim objClientTreatmentInfoDataObject As New ClientTreatmentInfoDataObject()
            Dim objClientEmergencyContactInfoDataObject As New ClientEmergencyContactInfoDataObject()

            Dim objClientCaseDataObject As New ClientCaseDataObject()
            Dim objClientCaseCareInfoDataObject As New ClientCaseCareInfoDataObject()
            Dim objClientCaseBillingInfoDataObject As New ClientCaseBillingInfoDataObject()
            Dim objClientCaseEVVInfoDataObject As New ClientCaseEVVInfoDataObject()

            Dim ClientCaseList As New List(Of ClientCaseDataObject)()
            Dim ClientCaseCareInfoList As New List(Of ClientCaseCareInfoDataObject)()
            Dim ClientCaseBillingInfoList As New List(Of ClientCaseBillingInfoDataObject)()
            Dim ClientCaseEVVInfoList As New List(Of ClientCaseEVVInfoDataObject)()

            Dim Client As New HybridDictionary()

            Dim PreviousClientInfoId As Integer = Int32.MinValue

            If drSql.HasRows Then
                While drSql.Read()

                    '**********************************************Client Info[Start]*********************************

                    objClientBasicInfoDataObject.ClientInfoId = Convert.ToInt64(drSql("ClientInfoId"))

                    If (Not objClientBasicInfoDataObject.ClientInfoId.Equals(PreviousClientInfoId)) Then
                        '#Region "Client Basic Information"

                        PreviousClientInfoId = objClientBasicInfoDataObject.ClientInfoId

                        objClientBasicInfoDataObject.SocialSecurityNumber = Convert.ToString(drSql("SocialSecurityNumber"), Nothing).Trim()

                        objClientBasicInfoDataObject.StateClientId = If((DBNull.Value.Equals(drSql("StateClientId"))),
                                                                        objClientBasicInfoDataObject.StateClientId,
                                                                        Convert.ToInt32(drSql("StateClientId")))

                        objClientBasicInfoDataObject.FirstName = Convert.ToString(drSql("FirstName"), Nothing).Trim()
                        objClientBasicInfoDataObject.LastName = Convert.ToString(drSql("LastName"), Nothing).Trim()
                        objClientBasicInfoDataObject.MiddleNameInitial = Convert.ToString(drSql("MiddleNameInitial"), Nothing).Trim()
                        'objClientBasicInfoDataObject.Phone = GetFormattedMobileNumber(Convert.ToString(drSql("Phone"), Nothing).Trim())
                        objClientBasicInfoDataObject.Phone = Convert.ToString(drSql("Phone"), Nothing).Trim()
                        'objClientBasicInfoDataObject.AlternatePhone = GetFormattedMobileNumber(Convert.ToString(drSql("AlternatePhone"), Nothing).Trim())
                        objClientBasicInfoDataObject.AlternatePhone = Convert.ToString(drSql("AlternatePhone"), Nothing).Trim()

                        objClientBasicInfoDataObject.Priority = If((DBNull.Value.Equals(drSql("Priority"))),
                                                                    objClientBasicInfoDataObject.Priority,
                                                                    Convert.ToInt16(drSql("Priority")))

                        objClientBasicInfoDataObject.DateOfBirth = Convert.ToString(drSql("DateOfBirth"), Nothing).Trim()
                        objClientBasicInfoDataObject.Address = Convert.ToString(drSql("Address"), Nothing).Trim()
                        objClientBasicInfoDataObject.ApartmentNumber = Convert.ToString(drSql("ApartmentNumber"), Nothing).Trim()
                        objClientBasicInfoDataObject.CityId = If((DBNull.Value.Equals(drSql("CityId"))), objClientBasicInfoDataObject.CityId, Convert.ToInt32(drSql("CityId")))
                        objClientBasicInfoDataObject.StateId = If((DBNull.Value.Equals(drSql("StateId"))), objClientBasicInfoDataObject.StateId, Convert.ToInt32(drSql("StateId")))
                        objClientBasicInfoDataObject.Zip = Convert.ToString(drSql("Zip"), Nothing).Trim()
                        objClientBasicInfoDataObject.Status = If((DBNull.Value.Equals(drSql("Status"))), objClientBasicInfoDataObject.Status, Convert.ToInt16(drSql("Status")))
                        objClientBasicInfoDataObject.Gender = Convert.ToString(drSql("Gender"), Nothing).Trim()

                        objClientBasicInfoDataObject.MaritalStatusId = If((DBNull.Value.Equals(drSql("MaritalStatusId"))),
                                                                          objClientBasicInfoDataObject.MaritalStatusId,
                                                                          Convert.ToInt32(drSql("MaritalStatusId")))

                        objClientBasicInfoDataObject.UpdateDate = If((DBNull.Value.Equals(drSql("UpdateDate"))),
                                                                     objClientBasicInfoDataObject.UpdateDate,
                                                                     Convert.ToString(drSql("UpdateDate"), Nothing))

                        objClientBasicInfoDataObject.UpdateBy = If((DBNull.Value.Equals(drSql("UpdateBy"))),
                                                                    objClientBasicInfoDataObject.UpdateBy,
                                                                    Convert.ToString(drSql("UpdateBy"), Nothing))

                        '#End Region

                        '#Region "Client Treatment Information"
                        objClientTreatmentInfoDataObject.ServiceStartDate = Convert.ToString(drSql("BeginDate"), Nothing).Trim()
                        objClientTreatmentInfoDataObject.ServiceEndDate = Convert.ToString(drSql("EndDate"), Nothing).Trim()

                        objClientTreatmentInfoDataObject.DischargeReason = If((DBNull.Value.Equals(drSql("Reason"))),
                                                                              objClientTreatmentInfoDataObject.DischargeReason,
                                                                              Convert.ToString(drSql("Reason"), Nothing).Trim())


                        objClientTreatmentInfoDataObject.Diagnosis = Convert.ToString(drSql("Diagnosis"), Nothing).Trim()

                        objClientTreatmentInfoDataObject.CountyId = If((DBNull.Value.Equals(drSql("CountyId"))),
                                                                       objClientTreatmentInfoDataObject.CountyId,
                                                                       Convert.ToInt32(drSql("CountyId")))

                        objClientTreatmentInfoDataObject.DoctorId = If((DBNull.Value.Equals(drSql("DoctorId"))),
                                                                       objClientTreatmentInfoDataObject.DoctorId,
                                                                       Convert.ToInt32(drSql("DoctorId")))

                        objClientTreatmentInfoDataObject.DisasterCategory = Convert.ToString(drSql("DisasterCategory"), Nothing).Trim()
                        objClientTreatmentInfoDataObject.Supervisor = Convert.ToString(drSql("Supervisor"), Nothing).Trim()
                        objClientTreatmentInfoDataObject.Liaison = Convert.ToString(drSql("Liaison"), Nothing).Trim()

                        '#End Region

                        '#Region "Client Emergency Contact Information"
                        objClientEmergencyContactInfoDataObject.EmergencyContact = Convert.ToString(drSql("EmergencyContact"), Nothing).Trim()
                        objClientEmergencyContactInfoDataObject.EmergencyContactPhone = Convert.ToString(drSql("EmergencyContactPhone"), Nothing).Trim()
                        objClientEmergencyContactInfoDataObject.EmergencyContactRelationship = Convert.ToString(drSql("EmergencyContactRelationship"), Nothing).Trim()
                        objClientEmergencyContactInfoDataObject.EmergencyContactTwo = Convert.ToString(drSql("EmergencyContactTwo"), Nothing).Trim()
                        objClientEmergencyContactInfoDataObject.EmergencyContactPhoneTwo = Convert.ToString(drSql("EmergencyContactPhoneTwo"), Nothing).Trim()
                        objClientEmergencyContactInfoDataObject.EmergencyContactRelationshipTwo = Convert.ToString(drSql("EmergencyContactRelationshipTwo"), Nothing).Trim()
                        '#End Region
                    End If

                    '**********************************************Client Info[End]*********************************

                    '**********************************************Client[Start]*********************************

                    '#Region "Client Case Information"

                    objClientCaseDataObject = New ClientCaseDataObject()

                    objClientCaseDataObject.ClientId = If((DBNull.Value.Equals(drSql("ClientId"))), objClientCaseDataObject.ClientId, Convert.ToInt64(drSql("ClientId")))
                    objClientCaseDataObject.Type = If((DBNull.Value.Equals(drSql("Type"))), objClientCaseDataObject.Type, Convert.ToInt32(drSql("Type")))
                    objClientCaseDataObject.CaseWorkerId = If((DBNull.Value.Equals(drSql("CaseWorker"))), objClientCaseDataObject.CaseWorkerId, Convert.ToInt32(drSql("CaseWorker")))
                    objClientCaseDataObject.Units = If((DBNull.Value.Equals(drSql("Units"))), objClientCaseDataObject.Units, Convert.ToString(drSql("Units"), Nothing))
                    objClientCaseDataObject.WeekDays = If((DBNull.Value.Equals(drSql("WeekDays"))), objClientCaseDataObject.WeekDays, Convert.ToInt32(drSql("WeekDays")))
                    objClientCaseDataObject.BeginDate = Convert.ToString(drSql("BeginDate"), Nothing).Trim()
                    objClientCaseDataObject.EndDate = Convert.ToString(drSql("EndDate"), Nothing).Trim()
                    objClientCaseDataObject.InsuranceNumber = Convert.ToString(drSql("InsuranceNumber"), Nothing).Trim()

                    ClientCaseList.Add(objClientCaseDataObject)

                    '#End Region

                    '#Region "Client Case Care Information"

                    objClientCaseCareInfoDataObject = New ClientCaseCareInfoDataObject()

                    objClientCaseCareInfoDataObject.ClientId = If((DBNull.Value.Equals(drSql("ClientId"))), objClientCaseCareInfoDataObject.ClientId _
                                                                  , Convert.ToInt64(drSql("CaseId")))

                    objClientCaseCareInfoDataObject.Type = If((DBNull.Value.Equals(drSql("Type"))), objClientCaseCareInfoDataObject.Type, Convert.ToInt32(drSql("Type")))
                    objClientCaseCareInfoDataObject.SupervisorLastVisitDate = Convert.ToString(drSql("LastEval"), Nothing).Trim()

                    objClientCaseCareInfoDataObject.SupervisorVisitFrequency = If((DBNull.Value.Equals(drSql("SupervisorVisitFrequency"))),
                                                                                    objClientCaseCareInfoDataObject.SupervisorVisitFrequency,
                                                                                    Convert.ToInt32(drSql("SupervisorVisitFrequency")))

                    objClientCaseCareInfoDataObject.Supplies = Convert.ToString(drSql("Supplies"), Nothing).Trim()
                    objClientCaseCareInfoDataObject.PlanOfCareStartDate = Convert.ToString(drSql("PlanOfCareStartDate"), Nothing).Trim()
                    objClientCaseCareInfoDataObject.PlanOfCareEndDate = Convert.ToString(drSql("PlanOfCareEndDate"), Nothing).Trim()
                    objClientCaseCareInfoDataObject.AuthorizationReceivedDate = Convert.ToString(drSql("AuthorizationReceived"), Nothing).Trim()
                    objClientCaseCareInfoDataObject.DoctorOrderSentDate = Convert.ToString(drSql("DoctorOrderSent"), Nothing).Trim()
                    objClientCaseCareInfoDataObject.DoctorOrderReceivedDate = Convert.ToString(drSql("DoctorOrderReceived"), Nothing).Trim()
                    objClientCaseCareInfoDataObject.ServiceInitializedReportedDate = Convert.ToString(drSql("ServiceInitializedReported"), Nothing).Trim()

                    ClientCaseCareInfoList.Add(objClientCaseCareInfoDataObject)

                    '#End Region


                    '#Region "Client Case Billing Information"

                    objClientCaseBillingInfoDataObject = New ClientCaseBillingInfoDataObject()

                    objClientCaseBillingInfoDataObject.ClientId = If((DBNull.Value.Equals(drSql("ClientId"))),
                                                                   objClientCaseBillingInfoDataObject.ClientId,
                                                                   Convert.ToInt64(drSql("ClientId")))

                    objClientCaseBillingInfoDataObject.Type = If((DBNull.Value.Equals(drSql("Type"))), objClientCaseBillingInfoDataObject.Type, Convert.ToInt32(drSql("Type")))
                    objClientCaseBillingInfoDataObject.AssessmentDate = Convert.ToString(drSql("AssessmentDate"), Nothing).Trim()
                    objClientCaseBillingInfoDataObject.DiagnosisCodeOne = Convert.ToString(drSql("DiagnosisCodeOne"), Nothing)
                    objClientCaseBillingInfoDataObject.DiagnosisCodeTwo = Convert.ToString(drSql("DiagnosisCodeTwo"), Nothing)
                    objClientCaseBillingInfoDataObject.DiagnosisCodeThree = Convert.ToString(drSql("DiagnosisCodeThree"), Nothing)
                    objClientCaseBillingInfoDataObject.DiagnosisCodeFour = Convert.ToString(drSql("DiagnosisCodeFour"), Nothing)
                    objClientCaseBillingInfoDataObject.AuthorizationNumber = Convert.ToString(drSql("AuthorizationNumber"), Nothing).Trim()
                    objClientCaseBillingInfoDataObject.ProcedureId = Convert.ToString(drSql("ProcedureId"), Nothing).Trim()
                    objClientCaseBillingInfoDataObject.ModifierOne = Convert.ToString(drSql("ModifierOne"), Nothing).Trim()
                    objClientCaseBillingInfoDataObject.ModifierTwo = Convert.ToString(drSql("ModifierTwo"), Nothing).Trim()
                    objClientCaseBillingInfoDataObject.ModifierThree = Convert.ToString(drSql("ModifierThree"), Nothing).Trim()
                    objClientCaseBillingInfoDataObject.ModifierFour = Convert.ToString(drSql("ModifierFour"), Nothing).Trim()
                    objClientCaseBillingInfoDataObject.PlaceOfServiceId = Convert.ToString(drSql("PlaceOfServiceId"), Nothing).Trim()
                    objClientCaseBillingInfoDataObject.Comments = Convert.ToString(drSql("Comments"), Nothing).Trim()

                    objClientCaseBillingInfoDataObject.UnitRate = If((Not DBNull.Value.Equals(drSql("UnitRate"))),
                                                                     Convert.ToDecimal(drSql("UnitRate"), Nothing),
                                                                     objClientCaseBillingInfoDataObject.UnitRate)

                    objClientCaseBillingInfoDataObject.CLM0503ClmFrequencyTypeCode = Convert.ToString(drSql("CLM0503ClmFrequencyTypeCode"), Nothing).Trim()
                    objClientCaseBillingInfoDataObject.CLM06ProviderSignatureOnFile = Convert.ToString(drSql("CLM06ProviderSignatureOnFile"), Nothing).Trim()
                    objClientCaseBillingInfoDataObject.CLM07MedicareAssignmentCode = Convert.ToString(drSql("CLM07MedicareAssignmentCode"), Nothing).Trim()
                    objClientCaseBillingInfoDataObject.CLM08AssignmentOfBenefitsIndicator = Convert.ToString(drSql("CLM08AssignmentOfBenefitsIndicator"), Nothing).Trim()
                    objClientCaseBillingInfoDataObject.CLM09ReleaseOfInfoCode = Convert.ToString(drSql("CLM09ReleaseOfInfoCode"), Nothing).Trim()
                    objClientCaseBillingInfoDataObject.CLM10PatientSignatureCode = Convert.ToString(drSql("CLM10PatientSignatureCode"), Nothing).Trim()

                    ClientCaseBillingInfoList.Add(objClientCaseBillingInfoDataObject)

                    '#End Region

                    '#Region "Client Case EVV Information"

                    objClientCaseEVVInfoDataObject = New ClientCaseEVVInfoDataObject()

                    objClientCaseEVVInfoDataObject.ClientId = If((DBNull.Value.Equals(drSql("ClientId"))),
                                                                   objClientCaseEVVInfoDataObject.ClientId,
                                                                   Convert.ToInt64(drSql("ClientId")))

                    objClientCaseEVVInfoDataObject.Type = If((DBNull.Value.Equals(drSql("Type"))), objClientCaseEVVInfoDataObject.Type, Convert.ToInt32(drSql("Type")))
                    objClientCaseEVVInfoDataObject.EVVClientId = Convert.ToString(drSql("EVVClientId"), Nothing).Trim()
                    objClientCaseEVVInfoDataObject.ServiceCode = Convert.ToString(drSql("ServiceCode"), Nothing).Trim()
                    objClientCaseEVVInfoDataObject.ServiceCodeDescription = Convert.ToString(drSql("ServiceCodeDescription"), Nothing).Trim()
                    objClientCaseEVVInfoDataObject.ServiceGroup = Convert.ToString(drSql("ServiceGroup"), Nothing).Trim()
                    objClientCaseEVVInfoDataObject.EVVARNumber = Convert.ToString(drSql("EVVARNumber"), Nothing).Trim()

                    objClientCaseEVVInfoDataObject.EVVPriority = If((DBNull.Value.Equals(drSql("EVVPriority"))),
                                                                            objClientCaseEVVInfoDataObject.EVVPriority,
                                                                            Convert.ToInt32(drSql("EVVPriority")))

                    objClientCaseEVVInfoDataObject.BillCode = Convert.ToString(drSql("BillCode"), Nothing).Trim()
                    objClientCaseEVVInfoDataObject.ProcedureCodeQualifier = Convert.ToString(drSql("ProcedureCodeQualifier"), Nothing).Trim()
                    'objClientCaseEVVInfoDataObject.LandPhone = GetFormattedMobileNumber(Convert.ToString(drSql("LandPhone"), Nothing).Trim())
                    objClientCaseEVVInfoDataObject.LandPhone = Convert.ToString(drSql("LandPhone"), Nothing).Trim()

                    ClientCaseEVVInfoList.Add(objClientCaseEVVInfoDataObject)

                    '#End Region

                    '**********************************************Client[End]*********************************

                End While
            End If

            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Client.Add("ClientBasicInfoDataObject", objClientBasicInfoDataObject)
            Client.Add("ClientTreatmentInfoDataObject", objClientTreatmentInfoDataObject)
            Client.Add("ClientEmergencyContactInfoDataObject", objClientEmergencyContactInfoDataObject)

            Client.Add("ClientCaseList", ClientCaseList)
            Client.Add("ClientCaseCareInfoList", ClientCaseCareInfoList)
            Client.Add("ClientCaseBillingInfoList", ClientCaseBillingInfoList)
            Client.Add("ClientCaseEVVInfoList", ClientCaseEVVInfoList)

            objClientBasicInfoDataObject = Nothing
            objClientTreatmentInfoDataObject = Nothing
            objClientEmergencyContactInfoDataObject = Nothing

            'ClientCaseList.Clear()
            ClientCaseList = Nothing

            'ClientCaseCareInfoList.Clear()
            ClientCaseCareInfoList = Nothing

            'ClientCaseBillingInfoList.Clear()
            ClientCaseBillingInfoList = Nothing

            'ClientCaseEVVInfoList.Clear()
            ClientCaseEVVInfoList = Nothing

            objClientCaseDataObject = Nothing
            objClientCaseCareInfoDataObject = Nothing
            objClientCaseBillingInfoDataObject = Nothing
            objClientCaseEVVInfoDataObject = Nothing

            Return Client

        End Function

        Private Sub SetParameters(parameters As HybridDictionary, objClientInfoDataObject As ClientInfoDataObject, objClientBasicInfoDataObject As ClientBasicInfoDataObject,
            objClientTreatmentInfoDataObject As ClientTreatmentInfoDataObject, objClientEmergencyContactInfoDataObject As ClientEmergencyContactInfoDataObject,
            objClientCaseDataObject As ClientCaseDataObject, objClientCaseCareInfoDataObject As ClientCaseCareInfoDataObject,
            objClientCaseBillingInfoDataObject As ClientCaseBillingInfoDataObject, objClientCaseEVVInfoDataObject As ClientCaseEVVInfoDataObject)

            '**********************************************Client Info[Start]*********************************

            '#Region "Client Basic Information"

            If (Not (objClientBasicInfoDataObject.Status = -1)) Then
                parameters.Add("@Status", objClientBasicInfoDataObject.Status)
            End If

            parameters.Add("@StateClientId", objClientInfoDataObject.StateClientId)
            parameters.Add("@FirstName", objClientBasicInfoDataObject.FirstName)
            parameters.Add("@LastName", objClientBasicInfoDataObject.LastName)

            If (Not String.IsNullOrEmpty(objClientBasicInfoDataObject.MiddleNameInitial)) Then
                parameters.Add("@MiddleNameInitial", objClientBasicInfoDataObject.MiddleNameInitial)
            End If

            If (Not String.IsNullOrEmpty(objClientBasicInfoDataObject.Phone)) Then
                parameters.Add("@Phone", objClientBasicInfoDataObject.Phone)
            End If

            If (Not String.IsNullOrEmpty(objClientBasicInfoDataObject.AlternatePhone)) Then
                parameters.Add("@AlternatePhone", objClientBasicInfoDataObject.AlternatePhone)
            End If

            If (Not String.IsNullOrEmpty(objClientBasicInfoDataObject.LandPhone)) Then
                parameters.Add("@LandPhone", objClientBasicInfoDataObject.LandPhone)
            End If

            parameters.Add("@Priority", objClientBasicInfoDataObject.Priority)

            If (Not String.IsNullOrEmpty(objClientBasicInfoDataObject.DateOfBirth)) Then
                parameters.Add("@DateOfBirth", objClientBasicInfoDataObject.DateOfBirth)
            End If

            If (Not String.IsNullOrEmpty(objClientBasicInfoDataObject.SocialSecurityNumber)) Then
                parameters.Add("@SocialSecurityNumber", objClientBasicInfoDataObject.SocialSecurityNumber)
            End If

            parameters.Add("@Address", objClientBasicInfoDataObject.Address)

            If (Not String.IsNullOrEmpty(objClientBasicInfoDataObject.ApartmentNumber)) Then
                parameters.Add("@ApartmentNumber", objClientBasicInfoDataObject.ApartmentNumber)
            End If

            parameters.Add("@CityId", objClientBasicInfoDataObject.CityId)
            parameters.Add("@StateId", objClientBasicInfoDataObject.StateId)
            parameters.Add("@Zip", objClientBasicInfoDataObject.Zip)
            parameters.Add("@Gender", objClientBasicInfoDataObject.Gender)

            If (objClientBasicInfoDataObject.MaritalStatusId > 0) Then
                parameters.Add("@MaritalStatusId", objClientBasicInfoDataObject.MaritalStatusId)
            End If

            '#End Region

            '#Region "Client Treatment Information"

            If (Not String.IsNullOrEmpty(objClientTreatmentInfoDataObject.ServiceStartDate)) Then
                parameters.Add("@BeginDate", objClientTreatmentInfoDataObject.ServiceStartDate)
            End If
            If (Not String.IsNullOrEmpty(objClientTreatmentInfoDataObject.ServiceEndDate)) Then
                parameters.Add("@EndDate", objClientTreatmentInfoDataObject.ServiceEndDate)
            End If

            If (objClientTreatmentInfoDataObject.DischargeReason > 0) Then
                parameters.Add("@Reason", objClientTreatmentInfoDataObject.DischargeReason)
            End If

            If (Not String.IsNullOrEmpty(objClientTreatmentInfoDataObject.Diagnosis)) Then
                parameters.Add("@Diagnosis", objClientTreatmentInfoDataObject.Diagnosis)
            End If

            If (objClientTreatmentInfoDataObject.CountyId > 0) Then
                parameters.Add("@CountyId", objClientTreatmentInfoDataObject.CountyId)
            End If

            If (objClientTreatmentInfoDataObject.DoctorId > 0) Then
                parameters.Add("@DoctorId", objClientTreatmentInfoDataObject.DoctorId)
            End If

            If (Not objClientTreatmentInfoDataObject.DisasterCategory.Equals("-1")) Then
                parameters.Add("@DisasterCategory", objClientTreatmentInfoDataObject.DisasterCategory)
            End If

            If (Not String.IsNullOrEmpty(objClientTreatmentInfoDataObject.Supervisor)) Then
                parameters.Add("@Supervisor", objClientTreatmentInfoDataObject.Supervisor)
            End If

            If (Not String.IsNullOrEmpty(objClientTreatmentInfoDataObject.Supervisor)) Then
                parameters.Add("@Liaison", objClientTreatmentInfoDataObject.Liaison)
            End If

            '#End Region

            '#Region "Client Emergency Contact Information"

            If (Not String.IsNullOrEmpty(objClientEmergencyContactInfoDataObject.EmergencyContact)) Then
                parameters.Add("@EmergencyContact", objClientEmergencyContactInfoDataObject.EmergencyContact)
            End If

            If (Not String.IsNullOrEmpty(objClientEmergencyContactInfoDataObject.EmergencyContactPhone)) Then
                parameters.Add("@EmergencyContactPhone", objClientEmergencyContactInfoDataObject.EmergencyContactPhone)
            End If

            If (Not String.IsNullOrEmpty(objClientEmergencyContactInfoDataObject.EmergencyContactRelationship)) Then
                parameters.Add("@EmergencyContactRelationship", objClientEmergencyContactInfoDataObject.EmergencyContactRelationship)
            End If

            If (Not String.IsNullOrEmpty(objClientEmergencyContactInfoDataObject.EmergencyContactTwo)) Then
                parameters.Add("@EmergencyContactTwo", objClientEmergencyContactInfoDataObject.EmergencyContactTwo)
            End If

            If (Not String.IsNullOrEmpty(objClientEmergencyContactInfoDataObject.EmergencyContactPhoneTwo)) Then
                parameters.Add("@EmergencyContactPhoneTwo", objClientEmergencyContactInfoDataObject.EmergencyContactPhoneTwo)
            End If

            If (Not String.IsNullOrEmpty(objClientEmergencyContactInfoDataObject.EmergencyContactRelationshipTwo)) Then
                parameters.Add("@EmergencyContactRelationshipTwo", objClientEmergencyContactInfoDataObject.EmergencyContactRelationshipTwo)
            End If

            '#End Region



            '**********************************************Client Info[End]************************************************************

            '**********************************************Client Case[Start]*****************************************************

            '#Region "Client Case Information"

            If (Not objClientCaseDataObject.Type.Equals(Int32.MinValue)) Then
                parameters.Add("@Type", objClientCaseDataObject.Type)
            End If

            If (Not objClientCaseDataObject.CaseWorkerId.Equals(-1)) Then
                parameters.Add("@CaseWorker", objClientCaseDataObject.CaseWorkerId)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseDataObject.Units)) Then
                'parameters.Add("@Units", New TimeSpan(objClientCaseDataObject.Units.Split(":")(0), objClientCaseDataObject.Units.Split(":")(1), objClientCaseDataObject.Units.Split(":")(2)))
                parameters.Add("@Units", objClientCaseDataObject.Units)
            End If

            If (Not objClientCaseDataObject.WeekDays.Equals(Int32.MinValue)) Then
                parameters.Add("@WeekDays", objClientCaseDataObject.WeekDays)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseDataObject.InsuranceNumber)) Then
                parameters.Add("@InsuranceNumber", objClientCaseDataObject.InsuranceNumber)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseDataObject.ReferralNumber)) Then
                parameters.Add("@ReferralNumber", objClientCaseDataObject.ReferralNumber)
            End If

            '#End Region

            '#Region "Client Case Care Information"

            If (Not String.IsNullOrEmpty(objClientCaseCareInfoDataObject.SupervisorLastVisitDate)) Then
                parameters.Add("@LastEval", objClientCaseCareInfoDataObject.SupervisorLastVisitDate)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseCareInfoDataObject.PlanOfCareStartDate)) Then
                parameters.Add("@PlanOfCareStartDate", objClientCaseCareInfoDataObject.PlanOfCareStartDate)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseCareInfoDataObject.PlanOfCareEndDate)) Then
                parameters.Add("@PlanOfCareEndDate", objClientCaseCareInfoDataObject.PlanOfCareEndDate)
            End If

            If (Not objClientCaseCareInfoDataObject.PlanOfCareUnits.Equals(Double.MinValue)) Then
                parameters.Add("@PlanOfCareUnits", objClientCaseCareInfoDataObject.PlanOfCareUnits)
            End If

            If (Not objClientCaseCareInfoDataObject.SupervisorVisitFrequency.Equals(Int32.MinValue)) Then
                parameters.Add("@SupervisorVisitFrequency", objClientCaseCareInfoDataObject.SupervisorVisitFrequency)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseCareInfoDataObject.Supplies)) Then
                parameters.Add("@Supplies", objClientCaseCareInfoDataObject.Supplies)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseCareInfoDataObject.AuthorizationReceivedDate)) Then
                parameters.Add("@AuthorizationReceived", objClientCaseCareInfoDataObject.AuthorizationReceivedDate)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseCareInfoDataObject.DoctorOrderSentDate)) Then
                parameters.Add("@DoctorOrderSent", objClientCaseCareInfoDataObject.DoctorOrderSentDate)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseCareInfoDataObject.DoctorOrderReceivedDate)) Then
                parameters.Add("@DoctorOrderReceived", objClientCaseCareInfoDataObject.DoctorOrderReceivedDate)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseCareInfoDataObject.ServiceInitializedReportedDate)) Then
                parameters.Add("@ServiceInitializedReported", objClientCaseCareInfoDataObject.ServiceInitializedReportedDate)
            End If

            '#End Region

            '#Region "Client Case Billing Information"

            If (Not String.IsNullOrEmpty(objClientCaseBillingInfoDataObject.AuthorizationNumber)) Then
                parameters.Add("@AuthorizationNumber", objClientCaseBillingInfoDataObject.AuthorizationNumber)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseBillingInfoDataObject.AssessmentDate)) Then
                parameters.Add("@AssessmentDate", objClientCaseBillingInfoDataObject.AssessmentDate)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseBillingInfoDataObject.Comments)) Then
                parameters.Add("@Comments", objClientCaseBillingInfoDataObject.Comments)
            End If

            If (Not objClientCaseBillingInfoDataObject.DiagnosisCodeOne.Equals("Please Select...")) Then
                parameters.Add("@DiagnosisCodeOne", objClientCaseBillingInfoDataObject.DiagnosisCodeOne)
            End If

            If (Not objClientCaseBillingInfoDataObject.DiagnosisCodeTwo.Equals("Please Select...")) Then
                parameters.Add("@DiagnosisCodeTwo", objClientCaseBillingInfoDataObject.DiagnosisCodeTwo)
            End If

            If (Not objClientCaseBillingInfoDataObject.DiagnosisCodeThree.Equals("Please Select...")) Then
                parameters.Add("@DiagnosisCodeThree", objClientCaseBillingInfoDataObject.DiagnosisCodeThree)
            End If

            If (Not objClientCaseBillingInfoDataObject.DiagnosisCodeFour.Equals("Please Select...")) Then
                parameters.Add("@DiagnosisCodeFour", objClientCaseBillingInfoDataObject.DiagnosisCodeFour)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseBillingInfoDataObject.ProcedureId)) Then
                parameters.Add("@ProcedureId", objClientCaseBillingInfoDataObject.ProcedureId)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseBillingInfoDataObject.PlaceOfServiceId)) Then
                parameters.Add("@PlaceOfServiceId", objClientCaseBillingInfoDataObject.PlaceOfServiceId)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseBillingInfoDataObject.ModifierOne)) Then
                parameters.Add("@ModifierOne", objClientCaseBillingInfoDataObject.ModifierOne)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseBillingInfoDataObject.ModifierTwo)) Then
                parameters.Add("@ModifierTwo", objClientCaseBillingInfoDataObject.ModifierTwo)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseBillingInfoDataObject.ModifierThree)) Then
                parameters.Add("@ModifierThree", objClientCaseBillingInfoDataObject.ModifierThree)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseBillingInfoDataObject.ModifierFour)) Then
                parameters.Add("@ModifierFour", objClientCaseBillingInfoDataObject.ModifierFour)
            End If

            If (Not objClientCaseBillingInfoDataObject.UnitRate.Equals(Decimal.MinValue)) Then
                parameters.Add("@UnitRate", objClientCaseBillingInfoDataObject.UnitRate)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseBillingInfoDataObject.TransactionRegistryStatus)) Then
                parameters.Add("@TransactionRegistryStatus", objClientCaseBillingInfoDataObject.TransactionRegistryStatus)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseBillingInfoDataObject.CLM0503ClmFrequencyTypeCode)) Then
                parameters.Add("@CLM0503ClmFrequencyTypeCode", objClientCaseBillingInfoDataObject.CLM0503ClmFrequencyTypeCode)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseBillingInfoDataObject.CLM06ProviderSignatureOnFile)) Then
                parameters.Add("@CLM06ProviderSignatureOnFile", objClientCaseBillingInfoDataObject.CLM06ProviderSignatureOnFile)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseBillingInfoDataObject.CLM07MedicareAssignmentCode)) Then
                parameters.Add("@CLM07MedicareAssignmentCode", objClientCaseBillingInfoDataObject.CLM07MedicareAssignmentCode)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseBillingInfoDataObject.CLM08AssignmentOfBenefitsIndicator)) Then
                parameters.Add("@CLM08AssignmentOfBenefitsIndicator", objClientCaseBillingInfoDataObject.CLM08AssignmentOfBenefitsIndicator)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseBillingInfoDataObject.CLM09ReleaseOfInfoCode)) Then
                parameters.Add("@CLM09ReleaseOfInfoCode", objClientCaseBillingInfoDataObject.CLM09ReleaseOfInfoCode)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseBillingInfoDataObject.CLM10PatientSignatureCode)) Then
                parameters.Add("@CLM10PatientSignatureCode", objClientCaseBillingInfoDataObject.CLM10PatientSignatureCode)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseBillingInfoDataObject.RightsNotificationDate)) Then
                parameters.Add("@RightsNotificationDate", objClientCaseBillingInfoDataObject.RightsNotificationDate)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseBillingInfoDataObject.EligibilityExpireDate)) Then
                parameters.Add("@EligibilityExpireDate", objClientCaseBillingInfoDataObject.EligibilityExpireDate)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseBillingInfoDataObject.AssignRecipientId)) Then
                parameters.Add("@AssignRecipientId", objClientCaseBillingInfoDataObject.AssignRecipientId)
            End If

            '#End Region

            '#Region "Client Case EVV Information"

            If (Not String.IsNullOrEmpty(objClientCaseEVVInfoDataObject.EVVClientId)) Then
                parameters.Add("@EVVClientId", objClientCaseEVVInfoDataObject.EVVClientId)
            End If

            If (Not objClientCaseEVVInfoDataObject.ServiceCode.Equals("Please Select...")) Then
                parameters.Add("@ServiceCode", objClientCaseEVVInfoDataObject.ServiceCode)
            End If

            If (Not objClientCaseEVVInfoDataObject.ServiceCodeDescription.Equals("Please Select...")) Then
                parameters.Add("@ServiceCodeDescription", objClientCaseEVVInfoDataObject.ServiceCodeDescription)
            End If

            If (Not objClientCaseEVVInfoDataObject.ServiceGroup.Equals("Please Select...")) Then
                parameters.Add("@ServiceGroup", objClientCaseEVVInfoDataObject.ServiceGroup)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseEVVInfoDataObject.EVVARNumber)) Then
                parameters.Add("@EVVARNumber", objClientCaseEVVInfoDataObject.EVVARNumber)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseEVVInfoDataObject.EVVProgramCode)) Then
                parameters.Add("@EVVProgramCode", objClientCaseEVVInfoDataObject.EVVProgramCode)
            End If

            If (Not objClientCaseEVVInfoDataObject.EVVPriority.Equals(Int32.MinValue)) Then
                parameters.Add("@EVVPriority", objClientCaseEVVInfoDataObject.EVVPriority)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseEVVInfoDataObject.EVVStatus)) Then
                parameters.Add("@EVVStatus", objClientCaseEVVInfoDataObject.EVVStatus)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseEVVInfoDataObject.BillCode)) Then
                parameters.Add("@BillCode", objClientCaseEVVInfoDataObject.BillCode)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseEVVInfoDataObject.ProcedureCodeQualifier)) Then
                parameters.Add("@ProcedureCodeQualifier", objClientCaseEVVInfoDataObject.ProcedureCodeQualifier)
            End If

            If (Not String.IsNullOrEmpty(objClientCaseEVVInfoDataObject.LandPhone)) Then
                parameters.Add("@LandPhone", objClientCaseEVVInfoDataObject.LandPhone)
            End If


            '#End Region

            '**********************************************Client Case[End]*********************************

        End Sub

        ''' <summary>
        ''' Get EVV Information
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <param name="ProgramServiceList"></param>
        ''' <param name="ServiceGroupList"></param>
        ''' <param name="ServiceCodeList"></param>
        ''' <param name="ServiceCodeDescriptionList"></param>
        ''' <param name="EVVInfoBillCodeList"></param>
        ''' <param name="EVVProcedureCodeQualifierList"></param>
        ''' <param name="EVVLandPhoneList"></param>
        ''' <remarks></remarks>
        Public Sub GetEVV(ConVisitel As SqlConnection, CompanyId As Integer,
                                   ByRef ProgramServiceList As List(Of EVVDataObject),
                                   ByRef ServiceGroupList As List(Of EVVDataObject),
                                   ByRef ServiceCodeList As List(Of EVVDataObject),
                                   ByRef ServiceCodeDescriptionList As List(Of EVVDataObject),
                                   ByRef EVVInfoBillCodeList As List(Of EVVDataObject),
                                   ByRef EVVProcedureCodeQualifierList As List(Of EVVDataObject),
                                   ByRef EVVLandPhoneList As List(Of EVVDataObject))

            Dim drSql As SqlDataReader = Nothing
            Dim objBLEVV As New BLEVV()
            objBLEVV.SelectEVV(ConVisitel, drSql)
            objBLEVV = Nothing

            Dim objEVVDataObject As EVVDataObject

            If drSql.HasRows Then
                While drSql.Read()
                    objEVVDataObject = New EVVDataObject()

                    objEVVDataObject.ID = If((DBNull.Value.Equals(drSql("ID"))), objEVVDataObject.ID, Convert.ToInt32(drSql("ID")))

                    If (Not ProgramServiceList Is Nothing) Then
                        objEVVDataObject.ProgramService = Convert.ToString(drSql("ProgramService"), Nothing)
                        ProgramServiceList.Add(objEVVDataObject)
                    End If

                    If (Not ServiceGroupList Is Nothing) Then
                        objEVVDataObject.ServiceGroup = Convert.ToString(drSql("ServiceGroup"), Nothing)
                        ServiceGroupList.Add(objEVVDataObject)
                    End If

                    If (Not ServiceCodeList Is Nothing) Then
                        objEVVDataObject.ServiceCode = Convert.ToString(drSql("ServiceCode"), Nothing)
                        ServiceCodeList.Add(objEVVDataObject)
                    End If

                    If (Not ServiceCodeDescriptionList Is Nothing) Then
                        objEVVDataObject.ServiceCodeDescription = Convert.ToString(drSql("Description"), Nothing)
                        ServiceCodeDescriptionList.Add(objEVVDataObject)
                    End If

                    If (Not EVVInfoBillCodeList Is Nothing) Then
                        objEVVDataObject.BillCode = Convert.ToString(drSql("BillCode"), Nothing)
                        EVVInfoBillCodeList.Add(objEVVDataObject)
                    End If

                    If (Not EVVProcedureCodeQualifierList Is Nothing) Then
                        objEVVDataObject.ProcedureCodeQualifier = Convert.ToString(drSql("ProcedureCodeQualifier"), Nothing)
                        EVVProcedureCodeQualifierList.Add(objEVVDataObject)
                    End If

                    If (Not EVVLandPhoneList Is Nothing) Then
                        objEVVDataObject.HCPCS = Convert.ToString(drSql("HCPCS"), Nothing)
                        EVVLandPhoneList.Add(objEVVDataObject)
                    End If

                End While

            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            objEVVDataObject = Nothing

        End Sub

        ''' <summary>
        ''' Get Diagnosis Information
        ''' </summary>
        ''' <param name="ConVisitel"></param>
        ''' <param name="CompanyId"></param>
        ''' <param name="DiagnosisCodeList"></param>
        ''' <remarks></remarks>
        Public Sub GetDiagnosis(ConVisitel As SqlConnection, CompanyId As Integer, ByRef DiagnosisCodeList As List(Of DiagnosisDataObject))

            Dim drSql As SqlDataReader = Nothing
            Dim objBLDiagnosis As New BLDiagnosis()
            objBLDiagnosis.SelectDiagnosis(ConVisitel, drSql)
            objBLDiagnosis = Nothing

            If drSql.HasRows Then
                Dim objDiagnosisDataObject As DiagnosisDataObject
                While drSql.Read()
                    objDiagnosisDataObject = New DiagnosisDataObject()

                    objDiagnosisDataObject.DiagnosisId = If((DBNull.Value.Equals(drSql("Id"))),
                                                              objDiagnosisDataObject.DiagnosisId, Convert.ToInt32(drSql("Id")))

                    objDiagnosisDataObject.DiagnosisCode = Convert.ToString(drSql("Code"), Nothing)

                    objDiagnosisDataObject.DiagnosisDescription = Convert.ToString(drSql("Description"), Nothing)

                    DiagnosisCodeList.Add(objDiagnosisDataObject)
                End While
                objDiagnosisDataObject = Nothing
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing
        End Sub

        ''' <summary>
        ''' Get Doctor Information
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetDoctor(ConVisitel As SqlConnection, CompanyId As Integer) As List(Of DoctorDataObject)

            Dim objBLDoctor As New BLDoctor()
            objBLDoctor.SelectDoctor(ConVisitel, CompanyId)
            objBLDoctor = Nothing

        End Function

        ''' <summary>
        ''' Get County Information
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetCounty(ConVisitel As SqlConnection, CompanyId As Integer) As List(Of CountyDataObject)

            Dim CountyList As List(Of CountyDataObject)

            Dim objBLCounty As New BLCounty()
            CountyList = objBLCounty.SelectCounty(ConVisitel, CompanyId)
            objBLCounty = Nothing

            Return CountyList
            
        End Function

        Public Sub GetProcedureCodeData(VisitelConnectionString As String, ByRef SqlDataSourceDropDownListProcedureCode As SqlDataSource)
            SqlDataSourceDropDownListProcedureCode.ProviderName = "System.Data.SqlClient"
            SqlDataSourceDropDownListProcedureCode.ConnectionString = VisitelConnectionString
            SqlDataSourceDropDownListProcedureCode.SelectCommandType = SqlDataSourceCommandType.StoredProcedure
            SqlDataSourceDropDownListProcedureCode.SelectCommand = "SelectProcedureCode"
            SqlDataSourceDropDownListProcedureCode.DataBind()
        End Sub

        Public Sub GetPlaceOfServiceData(VisitelConnectionString As String, ByRef SqlDataSourceDropDownListPlaceOfService As SqlDataSource)
            SqlDataSourceDropDownListPlaceOfService.ProviderName = "System.Data.SqlClient"
            SqlDataSourceDropDownListPlaceOfService.ConnectionString = VisitelConnectionString
            SqlDataSourceDropDownListPlaceOfService.SelectCommandType = SqlDataSourceCommandType.StoredProcedure
            SqlDataSourceDropDownListPlaceOfService.SelectCommand = "SelectPlaceOfService"
            SqlDataSourceDropDownListPlaceOfService.DataBind()
        End Sub

        Public Sub GetMedicareAssignmentCodeData(VisitelConnectionString As String, ByRef SqlDataSourceDropDownListMedicareAssignmentCode As SqlDataSource)
            SqlDataSourceDropDownListMedicareAssignmentCode.ProviderName = "System.Data.SqlClient"
            SqlDataSourceDropDownListMedicareAssignmentCode.ConnectionString = VisitelConnectionString
            SqlDataSourceDropDownListMedicareAssignmentCode.SelectCommandType = SqlDataSourceCommandType.StoredProcedure
            SqlDataSourceDropDownListMedicareAssignmentCode.SelectCommand = "SelectMedicareAssignmentCode"
            SqlDataSourceDropDownListMedicareAssignmentCode.DataBind()
        End Sub

        Public Sub GetReleaseOfInformationCodeData(VisitelConnectionString As String, ByRef SqlDataSourceDropDownListReleaseOfInformationCode As SqlDataSource)
            SqlDataSourceDropDownListReleaseOfInformationCode.ProviderName = "System.Data.SqlClient"
            SqlDataSourceDropDownListReleaseOfInformationCode.ConnectionString = VisitelConnectionString
            SqlDataSourceDropDownListReleaseOfInformationCode.SelectCommandType = SqlDataSourceCommandType.StoredProcedure
            SqlDataSourceDropDownListReleaseOfInformationCode.SelectCommand = "SelectReleaseOfInformationCode"
            SqlDataSourceDropDownListReleaseOfInformationCode.DataBind()
        End Sub

        Public Sub GetPatientSignatureCodeData(VisitelConnectionString As String, ByRef SqlDataSourceDropDownListPatientSignatureCode As SqlDataSource)
            SqlDataSourceDropDownListPatientSignatureCode.ProviderName = "System.Data.SqlClient"
            SqlDataSourceDropDownListPatientSignatureCode.ConnectionString = VisitelConnectionString
            SqlDataSourceDropDownListPatientSignatureCode.SelectCommandType = SqlDataSourceCommandType.StoredProcedure
            SqlDataSourceDropDownListPatientSignatureCode.SelectCommand = "SelectPatientSignatureCode"
            SqlDataSourceDropDownListPatientSignatureCode.DataBind()
        End Sub

        Public Function SelectClientsZipcode(ConVisitel As SqlConnection, CompanyId As Integer) As List(Of String)

            Dim drSql As SqlDataReader = Nothing
            Dim parameters As New HybridDictionary()
            parameters.Add("@CompanyId", CompanyId)

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectClientsZipcode]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim ClientZipcodeList As New List(Of String)()

            If drSql.HasRows Then
                While drSql.Read()
                    ClientZipcodeList.Add(Convert.ToString(drSql("Zip"), Nothing))
                End While
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return ClientZipcodeList

        End Function

        Public Function SelectClientsLiaison(ConVisitel As SqlConnection, CompanyId As Integer) As List(Of String)

            Dim drSql As SqlDataReader = Nothing
            Dim parameters As New HybridDictionary()
            parameters.Add("@CompanyId", CompanyId)

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectClientsLiaison]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim ClientLiaisonList As New List(Of String)()

            If drSql.HasRows Then
                While drSql.Read()
                    ClientLiaisonList.Add(Convert.ToString(drSql("Liaison"), Nothing))
                End While
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return ClientLiaisonList

        End Function

        Public Function SelectClientsSupervisor(ConVisitel As SqlConnection, CompanyId As Integer) As List(Of String)

            Dim drSql As SqlDataReader = Nothing
            Dim parameters As New HybridDictionary()
            parameters.Add("@CompanyId", CompanyId)

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectClientsSupervisor]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim ClientSupervisorList As New List(Of String)()

            If drSql.HasRows Then
                While drSql.Read()
                    ClientSupervisorList.Add(Convert.ToString(drSql("Supervisor"), Nothing))
                End While
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return ClientSupervisorList

        End Function

        Public Function SelectClientService(ConVisitel As SqlConnection, ClientInfoId As Int64) As List(Of ClientServiceInfoDataObject)

            Dim drSql As SqlDataReader = Nothing
            Dim parameters As New HybridDictionary()
            parameters.Add("@ClientInfoId", ClientInfoId)

            Dim objSharedSettings As New SharedSettings()
            objSharedSettings.GetDataReader("", "[TurboDB.SelectClientServiceInfo]", drSql, ConVisitel, parameters)
            objSharedSettings = Nothing
            parameters = Nothing

            Dim ClientServiceInfo As New List(Of ClientServiceInfoDataObject)()

            Dim objClientServiceInfoDataObject As ClientServiceInfoDataObject
            If drSql.HasRows Then
                While drSql.Read()
                    objClientServiceInfoDataObject = New ClientServiceInfoDataObject()

                    objClientServiceInfoDataObject.Name = Convert.ToString(drSql("Name"), Nothing)
                    objClientServiceInfoDataObject.ServiceCodeDescription = Convert.ToString(drSql("ServiceCodeDescription"), Nothing)



                    objClientServiceInfoDataObject.ClientInfoId = If((DBNull.Value.Equals(drSql("ClientInfoId"))),
                                                                        objClientServiceInfoDataObject.ClientInfoId,
                                                                        Convert.ToInt32(drSql("ClientInfoId")))

                    objClientServiceInfoDataObject.ClientId = If((DBNull.Value.Equals(drSql("ClientId"))),
                                                                       objClientServiceInfoDataObject.ClientId,
                                                                       Convert.ToInt32(drSql("ClientId")))

                    objClientServiceInfoDataObject.Status = Convert.ToString(drSql("Status"), Nothing)
                    objClientServiceInfoDataObject.AuthorizationNumber = Convert.ToString(drSql("AuthorizationNumber"), Nothing)

                    objClientServiceInfoDataObject.AuthorizationStartDate = Convert.ToString(drSql("PlanOfCareStartDate"), Nothing)
                    objClientServiceInfoDataObject.AuthorizationEndDate = Convert.ToString(drSql("PlanOfCareEndDate"), Nothing)

                    ClientServiceInfo.Add(objClientServiceInfoDataObject)
                End While
            End If
            drSql.Close()
            drSql.Dispose()
            drSql = Nothing

            Return ClientServiceInfo

        End Function

    End Class
End Namespace

