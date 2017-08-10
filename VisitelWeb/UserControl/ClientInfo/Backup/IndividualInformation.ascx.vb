Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports System.Data.SqlClient

Namespace Visitel.UserControl.ClientInfo

    Public Class IndividualInformationControl
        Inherits BaseUserControl

        Private ControlName As String, EditText As String, DeleteText As String, InsertMessage As String, UpdateMessage As String, DeleteMessage As String, _
        DuplicateNameMessage As String, EmptyNameMessage As String, StatusSelectionMessage As String, ReceiverSelectionMessage As String, PayerSelectionMessage As String, _
        InvalidContractNumberMessage As String, InvalidUnitRateMessage As String, SaveConfirmMessage As String, DuplicateClientMessage As String, _
        DuplicateClientCaseMessage As String, ValidationGroup As String

        Private ValidationEnable As Boolean

        Private CompanyId As Int32, UserId As Int32

        Private IndividualId As Int64

        Private ClientCaseList As New List(Of ClientCaseDataObject)()
        Private ClientCaseCareInfoList As New List(Of ClientCaseCareInfoDataObject)()
        Private ClientCaseBillingInfoList As New List(Of ClientCaseBillingInfoDataObject)()
        Private ClientCaseSantraxInfoList As New List(Of ClientCaseSantraxInfoDataObject)()

        Private objShared As SharedWebControls

        Public hfIsSearched As HiddenField

        Protected Sub Page_Init(sender As Object, e As EventArgs)

            ControlName = "ClientInfoControl"
            objShared = New SharedWebControls()
            objShared.ConnectionOpen()
            InitializeControl()
            GetCaptionFromResource()

        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)

            If Not IsPostBack Then
                HiddenFieldIsNew.Value = Convert.ToString(True, Nothing)
                HiddenFieldIsSearched.Value = Convert.ToString(False, Nothing)

                If (String.IsNullOrEmpty(Request.QueryString("ClientId"))) Then
                    GetData()
                End If
            End If

            ButtonDelete.Enabled = If(HiddenFieldIsSearched.Value.Equals(Convert.ToString(True, Nothing)), True, False)

        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadCss("ClientInfo/" + ControlName)
            LoadJScript()
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing
        End Sub

        Private Sub ButtonSave_Click(sender As Object, e As EventArgs)

            SaveData(False)

            'TextBoxSearchBySocialSecurityNumber.Text = TextBoxSocialSecurityNumber.Text
            'TextBoxSearchByStateClientId.Text = TextBoxStateClientId.Text

            objShared.TabSelection(1)

        End Sub

        Private Sub ButtonDelete_Click(sender As Object, e As EventArgs)

            DeleteData()
            objShared.TabSelection(1)

        End Sub

        Private Sub ButtonClear_Click(sender As Object, e As EventArgs)

            ClearClientInfoControl()
            ClearClientCaseInfoControl()

            'DropDownListSearchByIndividual.SelectedIndex = 0
            'ButtonIndividual.Enabled = If(DropDownListSearchByIndividual.SelectedValue.Equals("-1"), False, True)

            objShared.TabSelection(1)

        End Sub

        Private Sub DropDownListProgramOrService_OnSelectedIndexChanged(sender As Object, e As EventArgs)

            DropDownListServiceGroup.SelectedIndex = DropDownListServiceGroup.Items.IndexOf(DropDownListServiceGroup.Items.FindByValue(
                                                     Convert.ToString(DropDownListProgramOrService.SelectedValue, Nothing)))

            DropDownListServiceCode.SelectedIndex = DropDownListServiceCode.Items.IndexOf(DropDownListServiceCode.Items.FindByValue(
                                                    Convert.ToString(DropDownListProgramOrService.SelectedValue, Nothing)))

            DropDownListServiceCodeDescription.SelectedIndex = DropDownListServiceCodeDescription.Items.IndexOf(DropDownListServiceCodeDescription.Items.FindByValue(
                                                               Convert.ToString(DropDownListProgramOrService.SelectedValue, Nothing)))

            FillSantraxInfo(DropDownListProgramOrService)

        End Sub

        Private Sub DropDownListServiceGroup_OnSelectedIndexChanged(sender As Object, e As EventArgs)

            DropDownListProgramOrService.SelectedIndex = DropDownListProgramOrService.Items.IndexOf(DropDownListProgramOrService.Items.FindByValue(
                                                         Convert.ToString(DropDownListServiceGroup.SelectedValue, Nothing)))

            DropDownListServiceCode.SelectedIndex = DropDownListServiceCode.Items.IndexOf(DropDownListServiceCode.Items.FindByValue(
                                                    Convert.ToString(DropDownListServiceGroup.SelectedValue, Nothing)))

            DropDownListServiceCodeDescription.SelectedIndex = DropDownListServiceCodeDescription.Items.IndexOf(DropDownListServiceCodeDescription.Items.FindByValue(
                                                               Convert.ToString(DropDownListServiceGroup.SelectedValue, Nothing)))

            FillSantraxInfo(DropDownListServiceGroup)

        End Sub

        Private Sub DropDownListServiceCode_OnSelectedIndexChanged(sender As Object, e As EventArgs)

            DropDownListProgramOrService.SelectedIndex = DropDownListProgramOrService.Items.IndexOf(DropDownListProgramOrService.Items.FindByValue(
                                                         Convert.ToString(DropDownListServiceCode.SelectedValue, Nothing)))

            DropDownListServiceGroup.SelectedIndex = DropDownListServiceGroup.Items.IndexOf(DropDownListServiceGroup.Items.FindByValue(
                                                     Convert.ToString(DropDownListServiceCode.SelectedValue, Nothing)))

            DropDownListServiceCodeDescription.SelectedIndex = DropDownListServiceCodeDescription.Items.IndexOf(DropDownListServiceCodeDescription.Items.FindByValue(
                                                               Convert.ToString(DropDownListServiceCode.SelectedValue, Nothing)))

            FillSantraxInfo(DropDownListServiceCode)

        End Sub

        Private Sub DropDownListServiceCodeDescription_OnSelectedIndexChanged(sender As Object, e As EventArgs)

            DropDownListProgramOrService.SelectedIndex = DropDownListProgramOrService.Items.IndexOf(DropDownListProgramOrService.Items.FindByValue(
                                                         Convert.ToString(DropDownListServiceCodeDescription.SelectedValue, Nothing)))

            DropDownListServiceGroup.SelectedIndex = DropDownListServiceGroup.Items.IndexOf(DropDownListServiceGroup.Items.FindByValue(
                                                     Convert.ToString(DropDownListServiceCodeDescription.SelectedValue, Nothing)))

            DropDownListServiceCode.SelectedIndex = DropDownListServiceCode.Items.IndexOf(DropDownListServiceCode.Items.FindByValue(
                                                    Convert.ToString(DropDownListServiceCodeDescription.SelectedValue, Nothing)))

            FillSantraxInfo(DropDownListServiceCodeDescription)

        End Sub

        Private Sub DropDownListClientType_OnSelectedIndexChanged(sender As Object, e As EventArgs)

            If (Not IndividualId.Equals(-1)) Then
                ClearClientCaseInfoControl()
            End If

            If Not DropDownListClientType.SelectedValue.Equals("-1") Then

                '**********************************************Client Case[Start]*********************************

                '#Region "Client Case Information"

                Dim objClientCaseDataObject As New ClientCaseDataObject
                Try
                    If ((ClientCaseList.Count < 1) And (Not Session(StaticSettings.SessionField.CLIENT_CASE_LIST) Is Nothing)) Then
                        ClientCaseList = DirectCast(Session(StaticSettings.SessionField.CLIENT_CASE_LIST), List(Of ClientCaseDataObject))
                    End If
                    objClientCaseDataObject = (From p In ClientCaseList Where p.Type = DropDownListClientType.SelectedValue).SingleOrDefault
                Catch ex As InvalidOperationException

                    If ex.Message.Contains("Sequence contains no elements") Then
                        objClientCaseDataObject = Nothing
                        Return
                    End If
                End Try

                If (Not objClientCaseDataObject Is Nothing) Then
                    HiddenFieldClientCaseId.Value = objClientCaseDataObject.CaseId

                    DropDownListCaseWorker.SelectedValue = If(objClientCaseDataObject.CaseWorkerId.Equals(Int32.MinValue), "-1",
                                                              Convert.ToString(objClientCaseDataObject.CaseWorkerId))

                    TextBoxUnits.Text = Convert.ToString(objClientCaseDataObject.Units, Nothing)
                    TextBoxNumberOfWeekDays.Text = If(objClientCaseDataObject.WeekDays.Equals(Int32.MinValue), String.Empty, Convert.ToString(objClientCaseDataObject.WeekDays))
                    TextBoxInsuranceNumber.Text = objClientCaseDataObject.InsuranceNumber
                End If

                objClientCaseDataObject = Nothing

                '#End Region


                '#Region "Client Case Care Information"

                Dim objClientCaseCareInfoDataObject As New ClientCaseCareInfoDataObject

                If ((ClientCaseCareInfoList.Count < 1) And (Not Session(StaticSettings.SessionField.CLIENT_CASE_CARE_INFO_LIST) Is Nothing)) Then
                    ClientCaseCareInfoList = DirectCast(Session(StaticSettings.SessionField.CLIENT_CASE_CARE_INFO_LIST), List(Of ClientCaseCareInfoDataObject))
                End If

                objClientCaseCareInfoDataObject = (From p In ClientCaseCareInfoList Where p.Type = DropDownListClientType.SelectedValue).SingleOrDefault

                If (Not objClientCaseCareInfoDataObject Is Nothing) Then

                    TextBoxSupervisorLastVisitDate.Text = If((String.IsNullOrEmpty(Convert.ToString(objClientCaseCareInfoDataObject.SupervisorLastVisitDate, Nothing))),
                                                            String.Empty,
                                                            Convert.ToDateTime(objClientCaseCareInfoDataObject.SupervisorLastVisitDate).ToString(objShared.DateFormat))

                    TextBoxSupervisorVisitFrequency.Text = If(objClientCaseCareInfoDataObject.SupervisorVisitFrequency.Equals(Int32.MinValue),
                                                              String.Empty,
                                                              Convert.ToString(objClientCaseCareInfoDataObject.SupervisorVisitFrequency))

                    TextBoxSuppliesOrEquipment.Text = objClientCaseCareInfoDataObject.Supplies

                    TextBoxAuthorizationFromDate.Text = If((String.IsNullOrEmpty(Convert.ToString(objClientCaseCareInfoDataObject.PlanOfCareStartDate, Nothing))),
                                                            String.Empty,
                                                            Convert.ToDateTime(objClientCaseCareInfoDataObject.PlanOfCareStartDate).ToString(objShared.DateFormat))

                    TextBoxAuthorizationToDate.Text = If((String.IsNullOrEmpty(Convert.ToString(objClientCaseCareInfoDataObject.PlanOfCareEndDate, Nothing))),
                                                        String.Empty,
                                                        Convert.ToDateTime(objClientCaseCareInfoDataObject.PlanOfCareEndDate).ToString(objShared.DateFormat))

                    TextBoxAuthorizatioReceivedDate.Text = If((String.IsNullOrEmpty(Convert.ToString(objClientCaseCareInfoDataObject.AuthorizationReceivedDate, Nothing))),
                                                                String.Empty,
                                                                Convert.ToDateTime(objClientCaseCareInfoDataObject.AuthorizationReceivedDate).ToString(objShared.DateFormat))

                    TextBoxPractitionerStatementSentDate.Text = If((String.IsNullOrEmpty(Convert.ToString(objClientCaseCareInfoDataObject.DoctorOrderSentDate, Nothing))),
                                                                   String.Empty,
                                                                   Convert.ToDateTime(objClientCaseCareInfoDataObject.DoctorOrderSentDate).ToString(objShared.DateFormat))

                    TextBoxPractitionerStatementReceivedDate.Text = If((String.IsNullOrEmpty(Convert.ToString(objClientCaseCareInfoDataObject.DoctorOrderReceivedDate, Nothing))),
                                                                        String.Empty,
                                                                        Convert.ToDateTime(objClientCaseCareInfoDataObject.DoctorOrderReceivedDate).ToString(objShared.DateFormat))

                    TextBoxServiceInitializedReportedDate.Text = If((String.IsNullOrEmpty(Convert.ToString(objClientCaseCareInfoDataObject.ServiceInitializedReportedDate, Nothing))),
                                                                    String.Empty,
                                                                    Convert.ToDateTime(objClientCaseCareInfoDataObject.ServiceInitializedReportedDate).ToString(objShared.DateFormat))
                End If

                objClientCaseCareInfoDataObject = Nothing

                '#End Region

                '#Region "Client Case Billing Information"

                Dim objClientCaseBillingInfoDataObject As New ClientCaseBillingInfoDataObject

                If ((ClientCaseBillingInfoList.Count < 1) And (Not Session(StaticSettings.SessionField.CLIENT_CASE_BILLING_INFO_LIST) Is Nothing)) Then
                    ClientCaseBillingInfoList = DirectCast(Session(StaticSettings.SessionField.CLIENT_CASE_BILLING_INFO_LIST), List(Of ClientCaseBillingInfoDataObject))
                End If

                objClientCaseBillingInfoDataObject = (From p In ClientCaseBillingInfoList Where p.Type = DropDownListClientType.SelectedValue).SingleOrDefault

                If (Not objClientCaseBillingInfoDataObject Is Nothing) Then

                    TextBoxAssessmentDate.Text = If((String.IsNullOrEmpty(Convert.ToString(objClientCaseBillingInfoDataObject.AssessmentDate, Nothing))),
                                                String.Empty,
                                                Convert.ToDateTime(objClientCaseBillingInfoDataObject.AssessmentDate).ToString(objShared.DateFormat))

                    DropDownListBillingDiagonosisCodeOne.SelectedIndex = DropDownListBillingDiagonosisCodeOne.Items.IndexOf(
                        DropDownListBillingDiagonosisCodeOne.Items.FindByText(Convert.ToString(objClientCaseBillingInfoDataObject.DiagnosisCodeOne, Nothing)))
                    DropDownListBillingDiagonosisCodeOne_OnSelectedIndexChanged(Nothing, Nothing)

                    DropDownListBillingDiagonosisCodeTwo.SelectedIndex = DropDownListBillingDiagonosisCodeTwo.Items.IndexOf(
                        DropDownListBillingDiagonosisCodeTwo.Items.FindByText(Convert.ToString(objClientCaseBillingInfoDataObject.DiagnosisCodeTwo, Nothing)))
                    DropDownListBillingDiagonosisCodeTwo_OnSelectedIndexChanged(Nothing, Nothing)

                    DropDownListBillingDiagonosisCodeThree.SelectedIndex = DropDownListBillingDiagonosisCodeThree.Items.IndexOf(
                        DropDownListBillingDiagonosisCodeThree.Items.FindByText(Convert.ToString(objClientCaseBillingInfoDataObject.DiagnosisCodeThree, Nothing)))
                    DropDownListBillingDiagonosisCodeThree_OnSelectedIndexChanged(Nothing, Nothing)

                    DropDownListBillingDiagonosisCodeFour.SelectedIndex = DropDownListBillingDiagonosisCodeFour.Items.IndexOf(
                        DropDownListBillingDiagonosisCodeFour.Items.FindByText(Convert.ToString(objClientCaseBillingInfoDataObject.DiagnosisCodeFour, Nothing)))
                    DropDownListBillingDiagonosisCodeFour_OnSelectedIndexChanged(Nothing, Nothing)

                    TextBoxAuthorizationNumber.Text = objClientCaseBillingInfoDataObject.AuthorizationNumber
                    DropDownListProcedureCode.SelectedIndex = DropDownListProcedureCode.Items.IndexOf(DropDownListProcedureCode.Items.FindByValue(
                                                              Convert.ToString(objClientCaseBillingInfoDataObject.ProcedureId, Nothing)))

                    TextBoxModifierOne.Text = objClientCaseBillingInfoDataObject.ModifierOne
                    TextBoxModifierTwo.Text = objClientCaseBillingInfoDataObject.ModifierTwo
                    TextBoxModifierThree.Text = objClientCaseBillingInfoDataObject.ModifierThree
                    TextBoxModifierFour.Text = objClientCaseBillingInfoDataObject.ModifierFour

                    DropDownListPlaceOfService.SelectedIndex = DropDownListPlaceOfService.Items.IndexOf(DropDownListPlaceOfService.Items.FindByValue(
                                                               Convert.ToString(objClientCaseBillingInfoDataObject.PlaceOfServiceId, Nothing)))

                    TextBoxComments.Text = objClientCaseBillingInfoDataObject.Comments
                    TextBoxUnitRate.Text = If(Not objClientCaseBillingInfoDataObject.UnitRate.Equals(0),
                                              objShared.GetFormattedPayrate(objClientCaseBillingInfoDataObject.UnitRate), String.Empty)

                    TextBoxClaimFrequencyTypeCode.Text = objClientCaseBillingInfoDataObject.CLM0503ClmFrequencyTypeCode

                    DropDownListProviderSignatureOnFile.SelectedIndex = DropDownListProviderSignatureOnFile.Items.IndexOf(
                        DropDownListProviderSignatureOnFile.Items.FindByValue(Convert.ToString(objClientCaseBillingInfoDataObject.CLM06ProviderSignatureOnFile, Nothing)))

                    DropDownListMedicareAssignmentCode.SelectedIndex = DropDownListMedicareAssignmentCode.Items.IndexOf(
                        DropDownListMedicareAssignmentCode.Items.FindByValue(Convert.ToString(objClientCaseBillingInfoDataObject.CLM07MedicareAssignmentCode, Nothing)))

                    DropDownListAssignmentOfBenefitsIndicator.SelectedIndex = DropDownListAssignmentOfBenefitsIndicator.Items.IndexOf(
                        DropDownListAssignmentOfBenefitsIndicator.Items.FindByValue(Convert.ToString(objClientCaseBillingInfoDataObject.CLM08AssignmentOfBenefitsIndicator, Nothing)))

                    DropDownListReleaseOfInformationCode.SelectedIndex = DropDownListReleaseOfInformationCode.Items.IndexOf(
                        DropDownListReleaseOfInformationCode.Items.FindByValue(Convert.ToString(objClientCaseBillingInfoDataObject.CLM09ReleaseOfInfoCode, Nothing)))

                    DropDownListPatientSignatureCode.SelectedIndex = DropDownListPatientSignatureCode.Items.IndexOf(
                        DropDownListPatientSignatureCode.Items.FindByValue(Convert.ToString(objClientCaseBillingInfoDataObject.CLM10PatientSignatureCode, Nothing)))

                End If

                objClientCaseBillingInfoDataObject = Nothing

                '#End Region

                '#Region "Client Case Santrax Information"

                Dim objClientCaseSantraxInfoDataObject As New ClientCaseSantraxInfoDataObject

                If ((ClientCaseSantraxInfoList.Count < 1) And (Not Session(StaticSettings.SessionField.CLIENT_CASE_SANTRAX_INFO_LIST) Is Nothing)) Then
                    ClientCaseSantraxInfoList = DirectCast(Session(StaticSettings.SessionField.CLIENT_CASE_SANTRAX_INFO_LIST), List(Of ClientCaseSantraxInfoDataObject))
                End If

                objClientCaseSantraxInfoDataObject = (From p In ClientCaseSantraxInfoList Where p.Type = DropDownListClientType.SelectedValue).SingleOrDefault

                If (Not objClientCaseSantraxInfoDataObject Is Nothing) Then

                    TextBoxSantraxClientId.Text = objClientCaseSantraxInfoDataObject.SantraxClientId

                    DropDownListServiceCode.SelectedIndex = DropDownListServiceCode.Items.IndexOf(DropDownListServiceCode.Items.FindByText(
                                                            Convert.ToString(objClientCaseSantraxInfoDataObject.ServiceCode, Nothing)))
                    DropDownListServiceCode_OnSelectedIndexChanged(Nothing, Nothing)

                    DropDownListProgramOrService.SelectedIndex = DropDownListProgramOrService.Items.IndexOf(DropDownListProgramOrService.Items.FindByText(
                                                                 Convert.ToString(objClientCaseSantraxInfoDataObject.ServiceCode, Nothing)))
                    DropDownListProgramOrService_OnSelectedIndexChanged(Nothing, Nothing)

                    DropDownListServiceCodeDescription.SelectedIndex = DropDownListServiceCodeDescription.Items.IndexOf(DropDownListServiceCodeDescription.Items.FindByText(
                                                                       Convert.ToString(objClientCaseSantraxInfoDataObject.ServiceCodeDescription, Nothing)))
                    DropDownListServiceCodeDescription_OnSelectedIndexChanged(Nothing, Nothing)

                    DropDownListServiceGroup.SelectedIndex = DropDownListServiceGroup.Items.IndexOf(DropDownListServiceGroup.Items.FindByText(
                                                             Convert.ToString(objClientCaseSantraxInfoDataObject.ServiceGroup, Nothing)))
                    DropDownListServiceGroup_OnSelectedIndexChanged(Nothing, Nothing)

                    TextBoxSantraxARNumber.Text = objClientCaseSantraxInfoDataObject.SantraxARNumber

                    DropDownListSantraxPriority.SelectedValue = If(objClientCaseSantraxInfoDataObject.SantraxPriority.Equals(Int32.MinValue), "-1",
                                                                    Convert.ToString(objClientCaseSantraxInfoDataObject.SantraxPriority))

                    TextBoxSantraxBillCode.Text = objClientCaseSantraxInfoDataObject.BillCode
                    TextBoxSantraxProcCodeQualifier.Text = objClientCaseSantraxInfoDataObject.ProcedureCodeQualifier

                End If

                objClientCaseSantraxInfoDataObject = Nothing

                '#End Region

                '**********************************************Client Case[End]*********************************

            End If

        End Sub

        Private Sub DropDownListBillingDiagonosisOne_OnSelectedIndexChanged(sender As Object, e As EventArgs)
            DropDownListBillingDiagonosisCodeOne.SelectedIndex = DropDownListBillingDiagonosisCodeOne.Items.IndexOf(
                DropDownListBillingDiagonosisCodeOne.Items.FindByValue(Convert.ToString(DropDownListBillingDiagonosisOne.SelectedValue, Nothing)))
            FillDiagnosis()
        End Sub

        Private Sub DropDownListBillingDiagonosisCodeOne_OnSelectedIndexChanged(sender As Object, e As EventArgs)
            DropDownListBillingDiagonosisOne.SelectedIndex = DropDownListBillingDiagonosisOne.Items.IndexOf(DropDownListBillingDiagonosisOne.Items.FindByValue(
                Convert.ToString(DropDownListBillingDiagonosisCodeOne.SelectedValue, Nothing)))
            FillDiagnosis()
        End Sub

        Private Sub DropDownListBillingDiagonosisTwo_OnSelectedIndexChanged(sender As Object, e As EventArgs)
            DropDownListBillingDiagonosisCodeTwo.SelectedIndex = DropDownListBillingDiagonosisCodeTwo.Items.IndexOf(DropDownListBillingDiagonosisCodeTwo.Items.FindByValue(
                                                                 Convert.ToString(DropDownListBillingDiagonosisTwo.SelectedValue, Nothing)))
            FillDiagnosis()
        End Sub

        Private Sub DropDownListBillingDiagonosisCodeTwo_OnSelectedIndexChanged(sender As Object, e As EventArgs)
            DropDownListBillingDiagonosisTwo.SelectedIndex = DropDownListBillingDiagonosisTwo.Items.IndexOf(DropDownListBillingDiagonosisTwo.Items.FindByValue(
                                                             Convert.ToString(DropDownListBillingDiagonosisCodeTwo.SelectedValue, Nothing)))
            FillDiagnosis()
        End Sub

        Private Sub DropDownListBillingDiagonosisThree_OnSelectedIndexChanged(sender As Object, e As EventArgs)
            DropDownListBillingDiagonosisCodeThree.SelectedIndex = DropDownListBillingDiagonosisCodeThree.Items.IndexOf(
                DropDownListBillingDiagonosisCodeThree.Items.FindByValue(Convert.ToString(DropDownListBillingDiagonosisThree.SelectedValue, Nothing)))
            FillDiagnosis()
        End Sub

        Private Sub DropDownListBillingDiagonosisCodeThree_OnSelectedIndexChanged(sender As Object, e As EventArgs)
            DropDownListBillingDiagonosisThree.SelectedIndex = DropDownListBillingDiagonosisThree.Items.IndexOf(DropDownListBillingDiagonosisThree.Items.FindByValue(
                                                               Convert.ToString(DropDownListBillingDiagonosisCodeThree.SelectedValue, Nothing)))
            FillDiagnosis()
        End Sub

        Private Sub DropDownListBillingDiagonosisFour_OnSelectedIndexChanged(sender As Object, e As EventArgs)
            DropDownListBillingDiagonosisCodeFour.SelectedIndex = DropDownListBillingDiagonosisCodeFour.Items.IndexOf(
                DropDownListBillingDiagonosisCodeFour.Items.FindByValue(Convert.ToString(DropDownListBillingDiagonosisFour.SelectedValue, Nothing)))
            FillDiagnosis()
        End Sub

        Private Sub DropDownListBillingDiagonosisCodeFour_OnSelectedIndexChanged(sender As Object, e As EventArgs)
            DropDownListBillingDiagonosisFour.SelectedIndex = DropDownListBillingDiagonosisFour.Items.IndexOf(DropDownListBillingDiagonosisFour.Items.FindByValue(
                                                              Convert.ToString(DropDownListBillingDiagonosisCodeFour.SelectedValue, Nothing)))
            FillDiagnosis()
        End Sub

        Private Sub LoadJScript()

            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                           & " var CompanyId = " & CompanyId & "; " _
                           & " var ClientId = ''; " _
                           & " var CalendarImagePath='" & objShared.GetCalendarImagePath & "'; " _
                           & " jQuery(document).ready(function () {" _
                           & "     DateFieldsEvent();" _
                           & "     InAutoGrowEvent();" _
                           & "     Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InAutoGrowEvent); " _
                           & "     Sys.WebForms.PageRequestManager.getInstance().add_endRequest(DateFieldsEvent); " _
                           & "}); " _
                    & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

            LoadJS("JavaScript/jquery.autogrow.js")
            LoadJS("JavaScript/ClientInfo/" & ControlName & ".js")

        End Sub

        Public Sub LoadIndividual()
            GetData()
            LoadSelectedClient()
        End Sub
        ''' <summary>
        ''' This is for retrieving data
        ''' </summary>
        Private Sub GetData()

            Try

                BindClientStatusDropDownList()
                objShared.BindCityDropDownList(DropDownListCity, CompanyId)
                objShared.BindCaseWorkerDropDownList(DropDownListCaseWorker, CompanyId)
                objShared.BindStateDropDownList(DropDownListState, CompanyId)
                objShared.BindMaritalStatusDropDownList(DropDownListMaritalStatus, CompanyId)

                BindDoctorDropDownList()
                objShared.BindPriorityDropDownList(DropDownListPriority)
                objShared.BindSantraxPriorityDropDownList(DropDownListSantraxPriority)
                BindEmergencyDisasterCategoryDropDownList()
                objShared.BindGenderDropDownList(DropDownListGender)
                BindDischargeReasonDropDownList()
                BindCountyDropDownList()
                objShared.BindClientTypeDropDownList(DropDownListClientType, CompanyId, EnumDataObject.ClientTypeListFor.Individual)
                BindDiagonosisDropDownLists()
                BindSantraxDropDownLists()

                BindProcedureCodeDropDownList()
                BindPlaceOfServiceDropDownList()
                BindProviderSignatureOnFileDropDownList()
                BindAssignmentOfBenefitsIndicatorDropDownList()
                BindMedicareAssignmentCodeDropDownList()
                BindReleaseOfInformationCodeDropDownList()
                BindPatientSignatureCodeDropDownList()

            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError("Unable to fetch data.")
            End Try

        End Sub

        ''' <summary>
        ''' Binding Patient Signature Code Drop Down List
        ''' </summary>
        Private Sub BindPatientSignatureCodeDropDownList()

            Dim objBLClientInfo As New BLClientInfo
            objBLClientInfo.GetPatientSignatureCodeData(objShared.VisitelConnectionString, SqlDataSourceDropDownListPatientSignatureCode)
            objBLClientInfo = Nothing

            DropDownListPatientSignatureCode.DataSourceID = "SqlDataSourceDropDownListPatientSignatureCode"
            DropDownListPatientSignatureCode.DataTextField = "Expr1"
            DropDownListPatientSignatureCode.DataValueField = "code"
            DropDownListPatientSignatureCode.DataBind()

        End Sub

        ''' <summary>
        ''' Binding Release Of Information Code Drop Down List
        ''' </summary>
        Private Sub BindReleaseOfInformationCodeDropDownList()

            Dim objBLClientInfo As New BLClientInfo
            objBLClientInfo.GetReleaseOfInformationCodeData(objShared.VisitelConnectionString, SqlDataSourceDropDownListReleaseOfInformationCode)
            objBLClientInfo = Nothing

            DropDownListReleaseOfInformationCode.DataSourceID = "SqlDataSourceDropDownListReleaseOfInformationCode"
            DropDownListReleaseOfInformationCode.DataTextField = "Expr1"
            DropDownListReleaseOfInformationCode.DataValueField = "code"
            DropDownListReleaseOfInformationCode.DataBind()

        End Sub

        ''' <summary>
        ''' Binding Medicare Assignment Code Drop Down List
        ''' </summary>
        Private Sub BindMedicareAssignmentCodeDropDownList()

            Dim objBLClientInfo As New BLClientInfo
            objBLClientInfo.GetMedicareAssignmentCodeData(objShared.VisitelConnectionString, SqlDataSourceDropDownListMedicareAssignmentCode)
            objBLClientInfo = Nothing

            DropDownListMedicareAssignmentCode.DataSourceID = "SqlDataSourceDropDownListMedicareAssignmentCode"
            DropDownListMedicareAssignmentCode.DataTextField = "Expr1"
            DropDownListMedicareAssignmentCode.DataValueField = "code"
            DropDownListMedicareAssignmentCode.DataBind()

        End Sub

        ''' <summary>
        ''' Binding Assignment Of Benefits Indicator Drop Down List
        ''' </summary>
        Private Sub BindAssignmentOfBenefitsIndicatorDropDownList()

            DropDownListAssignmentOfBenefitsIndicator.DataSource = EnumDataObject.Enumeration.GetAll(Of EnumDataObject.AssignmentOfBenefitsIndicator)()
            DropDownListAssignmentOfBenefitsIndicator.DataTextField = "value"
            DropDownListAssignmentOfBenefitsIndicator.DataValueField = "value"
            DropDownListAssignmentOfBenefitsIndicator.DataBind()

            DropDownListAssignmentOfBenefitsIndicator.SelectedIndex = 1

        End Sub

        ''' <summary>
        ''' Binding Provider Signature On File Drop Down List
        ''' </summary>
        Private Sub BindProviderSignatureOnFileDropDownList()

            DropDownListProviderSignatureOnFile.DataSource = EnumDataObject.Enumeration.GetAll(Of EnumDataObject.ProviderSignatureOnFile)()
            DropDownListProviderSignatureOnFile.DataTextField = "value"
            DropDownListProviderSignatureOnFile.DataValueField = "value"
            DropDownListProviderSignatureOnFile.DataBind()

            DropDownListProviderSignatureOnFile.SelectedIndex = 1

        End Sub

        ''' <summary>
        ''' Binding Place Of Service Drop Down List
        ''' </summary>
        Private Sub BindPlaceOfServiceDropDownList()

            Dim objBLClientInfo As New BLClientInfo
            objBLClientInfo.GetPlaceOfServiceData(objShared.VisitelConnectionString, SqlDataSourceDropDownListPlaceOfService)
            objBLClientInfo = Nothing

            DropDownListPlaceOfService.DataSourceID = "SqlDataSourceDropDownListPlaceOfService"
            DropDownListPlaceOfService.DataTextField = "Description"
            DropDownListPlaceOfService.DataValueField = "pos_id"
            DropDownListPlaceOfService.DataBind()

        End Sub

        ''' <summary>
        ''' Binding Procedure Code Drop Down List
        ''' </summary>
        Private Sub BindProcedureCodeDropDownList()

            Dim objBLClientInfo As New BLClientInfo
            objBLClientInfo.GetProcedureCodeData(objShared.VisitelConnectionString, SqlDataSourceDropDownListProcedureCode)
            objBLClientInfo = Nothing

            DropDownListProcedureCode.DataSourceID = "SqlDataSourceDropDownListProcedureCode"
            DropDownListProcedureCode.DataTextField = "Description"
            DropDownListProcedureCode.DataValueField = "ProcedureCode"
            DropDownListProcedureCode.DataBind()

        End Sub

        ''' <summary>
        ''' Binding Santrax Drop Down Lists
        ''' </summary>
        Private Sub BindSantraxDropDownLists()

            Dim ProgramServiceList As New List(Of SantraxDataObject)()
            Dim ServiceGroupList As New List(Of SantraxDataObject)()
            Dim ServiceCodeList As New List(Of SantraxDataObject)()
            Dim ServiceCodeDescriptionList As New List(Of SantraxDataObject)()

            Dim objBLClientInfo As New BLClientInfo
            objBLClientInfo.GetSantrax(objShared.ConVisitel, CompanyId, ProgramServiceList, ServiceGroupList, ServiceCodeList, ServiceCodeDescriptionList,
                                       Nothing, Nothing, Nothing)
            objBLClientInfo = Nothing

            DropDownListProgramOrService.DataSource = ProgramServiceList
            DropDownListProgramOrService.DataTextField = "ProgramService"
            DropDownListProgramOrService.DataValueField = "ID"
            DropDownListProgramOrService.DataBind()

            DropDownListProgramOrService.Items.Insert(0, New ListItem("Please Select...", "-1"))
            ProgramServiceList = Nothing

            DropDownListServiceGroup.DataSource = ServiceGroupList
            DropDownListServiceGroup.DataTextField = "ServiceGroup"
            DropDownListServiceGroup.DataValueField = "ID"
            DropDownListServiceGroup.DataBind()

            DropDownListServiceGroup.Items.Insert(0, New ListItem("Please Select...", "-1"))
            ServiceGroupList = Nothing

            DropDownListServiceCode.DataSource = ServiceCodeList
            DropDownListServiceCode.DataTextField = "ServiceCode"
            DropDownListServiceCode.DataValueField = "ID"
            DropDownListServiceCode.DataBind()

            DropDownListServiceCode.Items.Insert(0, New ListItem("Please Select...", "-1"))
            ServiceCodeList = Nothing

            DropDownListServiceCodeDescription.DataSource = ServiceCodeDescriptionList
            DropDownListServiceCodeDescription.DataTextField = "ServiceCodeDescription"
            DropDownListServiceCodeDescription.DataValueField = "ID"
            DropDownListServiceCodeDescription.DataBind()

            DropDownListServiceCodeDescription.Items.Insert(0, New ListItem("Please Select...", "-1"))
            ServiceCodeDescriptionList = Nothing

        End Sub

        ''' <summary>
        ''' Binding Diagonosis Drop Down Lists
        ''' </summary>
        Private Sub BindDiagonosisDropDownLists()

            Dim DiagonosisOneList As New List(Of DiagonosisDataObject)()
            Dim DiagonosisOneCodeList As New List(Of DiagonosisDataObject)()

            Dim DiagonosisTwoList As New List(Of DiagonosisDataObject)()
            Dim DiagonosisTwoCodeList As New List(Of DiagonosisDataObject)()

            Dim DiagonosisThreeList As New List(Of DiagonosisDataObject)()
            Dim DiagonosisThreeCodeList As New List(Of DiagonosisDataObject)()

            Dim DiagonosisFourList As New List(Of DiagonosisDataObject)()
            Dim DiagonosisFourCodeList As New List(Of DiagonosisDataObject)()

            Dim objBLClientInfo As New BLClientInfo
            objBLClientInfo.GetDiagonosis(objShared.ConVisitel, CompanyId, DiagonosisOneList, DiagonosisOneCodeList,
                                          DiagonosisTwoList, DiagonosisTwoCodeList,
                                          DiagonosisThreeList, DiagonosisThreeCodeList,
                                          DiagonosisFourList, DiagonosisFourCodeList)
            objBLClientInfo = Nothing

            DiagonosisOneList = DiagonosisOneList.OrderBy(Function(x) x.DiagonosisOne).ToList()
            DiagonosisOneCodeList = DiagonosisOneCodeList.OrderBy(Function(x) x.DiagonosisOne).ToList()

            DiagonosisTwoList = DiagonosisTwoList.OrderBy(Function(x) x.DiagonosisTwo).ToList()
            DiagonosisTwoCodeList = DiagonosisTwoCodeList.OrderBy(Function(x) x.DiagonosisTwo).ToList()

            DiagonosisThreeList = DiagonosisThreeList.OrderBy(Function(x) x.DiagonosisThree).ToList()
            DiagonosisThreeCodeList = DiagonosisThreeCodeList.OrderBy(Function(x) x.DiagonosisThree).ToList()

            DiagonosisFourList = DiagonosisFourList.OrderBy(Function(x) x.DiagonosisFour).ToList()
            DiagonosisFourCodeList = DiagonosisFourCodeList.OrderBy(Function(x) x.DiagonosisFour).ToList()
            DropDownListBillingDiagonosisOne.DataSource = DiagonosisOneList
            DropDownListBillingDiagonosisOne.DataTextField = "DiagonosisOne"
            DropDownListBillingDiagonosisOne.DataValueField = "DiagonosisId"
            DropDownListBillingDiagonosisOne.DataBind()

            DropDownListBillingDiagonosisOne.Items.Insert(0, New ListItem("Please Select...", "-1"))
            DiagonosisOneList = Nothing

            DropDownListBillingDiagonosisCodeOne.DataSource = DiagonosisOneCodeList
            DropDownListBillingDiagonosisCodeOne.DataTextField = "DiagonosisOneCode"
            DropDownListBillingDiagonosisCodeOne.DataValueField = "DiagonosisId"
            DropDownListBillingDiagonosisCodeOne.DataBind()

            DropDownListBillingDiagonosisCodeOne.Items.Insert(0, New ListItem("Please Select...", "-1"))
            DiagonosisOneCodeList = Nothing

            DropDownListBillingDiagonosisTwo.DataSource = DiagonosisTwoList
            DropDownListBillingDiagonosisTwo.DataTextField = "DiagonosisTwo"
            DropDownListBillingDiagonosisTwo.DataValueField = "DiagonosisId"
            DropDownListBillingDiagonosisTwo.DataBind()

            DropDownListBillingDiagonosisTwo.Items.Insert(0, New ListItem("Please Select...", "-1"))
            DiagonosisTwoList = Nothing

            DropDownListBillingDiagonosisCodeTwo.DataSource = DiagonosisTwoCodeList
            DropDownListBillingDiagonosisCodeTwo.DataTextField = "DiagonosisTwoCode"
            DropDownListBillingDiagonosisCodeTwo.DataValueField = "DiagonosisId"
            DropDownListBillingDiagonosisCodeTwo.DataBind()

            DropDownListBillingDiagonosisCodeTwo.Items.Insert(0, New ListItem("Please Select...", "-1"))
            DiagonosisTwoCodeList = Nothing

            DropDownListBillingDiagonosisThree.DataSource = DiagonosisThreeList
            DropDownListBillingDiagonosisThree.DataTextField = "DiagonosisThree"
            DropDownListBillingDiagonosisThree.DataValueField = "DiagonosisId"
            DropDownListBillingDiagonosisThree.DataBind()

            DropDownListBillingDiagonosisThree.Items.Insert(0, New ListItem("Please Select...", "-1"))
            DiagonosisThreeList = Nothing

            DropDownListBillingDiagonosisCodeThree.DataSource = DiagonosisThreeCodeList
            DropDownListBillingDiagonosisCodeThree.DataTextField = "DiagonosisThreeCode"
            DropDownListBillingDiagonosisCodeThree.DataValueField = "DiagonosisId"
            DropDownListBillingDiagonosisCodeThree.DataBind()

            DropDownListBillingDiagonosisCodeThree.Items.Insert(0, New ListItem("Please Select...", "-1"))
            DiagonosisThreeCodeList = Nothing

            DropDownListBillingDiagonosisFour.DataSource = DiagonosisFourList
            DropDownListBillingDiagonosisFour.DataTextField = "DiagonosisFour"
            DropDownListBillingDiagonosisFour.DataValueField = "DiagonosisId"
            DropDownListBillingDiagonosisFour.DataBind()

            DropDownListBillingDiagonosisFour.Items.Insert(0, New ListItem("Please Select...", "-1"))
            DiagonosisFourList = Nothing

            DropDownListBillingDiagonosisCodeFour.DataSource = DiagonosisFourCodeList
            DropDownListBillingDiagonosisCodeFour.DataTextField = "DiagonosisFourCode"
            DropDownListBillingDiagonosisCodeFour.DataValueField = "DiagonosisId"
            DropDownListBillingDiagonosisCodeFour.DataBind()

            DropDownListBillingDiagonosisCodeFour.Items.Insert(0, New ListItem("Please Select...", "-1"))
            DiagonosisFourCodeList = Nothing

        End Sub

        ''' <summary>
        ''' Binding County Drop Down List
        ''' </summary>
        Private Sub BindCountyDropDownList()

            Dim objBLCounty As New BLCounty()
            DropDownListCounty.DataSource = objBLCounty.SelectCounty(objShared.ConVisitel, CompanyId)
            objBLCounty = Nothing

            DropDownListCounty.DataTextField = "CountyName"
            DropDownListCounty.DataValueField = "CountyId"
            DropDownListCounty.DataBind()

            DropDownListCounty.Items.Insert(0, New ListItem("Please Select...", "-1"))

        End Sub

        ''' <summary>
        ''' Binding Client Status Drop Down List
        ''' </summary>
        Private Sub BindClientStatusDropDownList()

            DropDownListIndividualStatus.DataSource = objShared.GetClientStatus(objShared.ConVisitel, CompanyId)

            DropDownListIndividualStatus.DataTextField = "ClientStatusName"
            DropDownListIndividualStatus.DataValueField = "ClientStatusId"
            DropDownListIndividualStatus.DataBind()

            DropDownListIndividualStatus.Items.Insert(0, New ListItem("Please Select...", "-1"))

        End Sub

        ''' <summary>
        ''' Binding Doctor Drop Down List
        ''' </summary>
        Private Sub BindDoctorDropDownList()

            Dim objBLDoctor As New BLDoctor()
            DropDownListDoctor.DataSource = objBLDoctor.SelectDoctor(objShared.ConVisitel, CompanyId)
            objBLDoctor = Nothing

            DropDownListDoctor.DataTextField = "DoctorName"
            DropDownListDoctor.DataValueField = "DoctorId"
            DropDownListDoctor.DataBind()

            DropDownListDoctor.Items.Insert(0, New ListItem("Please Select...", "-1"))

        End Sub

        ''' <summary>
        ''' Binding Emergency Disaster Category Drop Down List
        ''' </summary>
        Protected Sub BindEmergencyDisasterCategoryDropDownList()

            DropDownListEmergencyDisasterCategory.DataSource = EnumDataObject.Enumeration.GetAll(Of EnumDataObject.EmergencyDisasterCategory)()
            DropDownListEmergencyDisasterCategory.DataTextField = "Key"
            DropDownListEmergencyDisasterCategory.DataValueField = "Key"
            DropDownListEmergencyDisasterCategory.DataBind()

            DropDownListEmergencyDisasterCategory.Items.Insert(0, New ListItem("Please Select...", "-1"))

        End Sub

        ''' <summary>
        ''' Binding Discharge Reason Drop Down List
        ''' </summary>
        Private Sub BindDischargeReasonDropDownList()

            Dim objBLDischargeReason As New BLDischargeReason()
            DropDownListDischargeReason.DataSource = objBLDischargeReason.SelectDischargeReason(objShared.ConVisitel, CompanyId)
            objBLDischargeReason = Nothing

            DropDownListDischargeReason.DataTextField = "Name"
            DropDownListDischargeReason.DataValueField = "IdNumber"
            DropDownListDischargeReason.DataBind()

            DropDownListDischargeReason.Items.Insert(0, New ListItem("Please Select...", "-1"))

        End Sub

        ''' <summary>
        ''' This is for filling Santrax Information
        ''' </summary>
        ''' <param name="DropDownListSantrax"></param>
        ''' <remarks></remarks>
        Private Sub FillSantraxInfo(DropDownListSantrax As DropDownList)

            If Not DropDownListSantrax.SelectedValue.Equals("-1") Then

                Dim SantraxInfoBillCodeList As New List(Of SantraxDataObject)()
                Dim SantraxProcedureCodeQualifierList As New List(Of SantraxDataObject)()
                Dim SantraxLandPhoneList As New List(Of SantraxDataObject)()

                Dim objBLClientInfo As New BLClientInfo
                objBLClientInfo.GetSantrax(objShared.ConVisitel, CompanyId, Nothing, Nothing, Nothing, Nothing, SantraxInfoBillCodeList, SantraxProcedureCodeQualifierList,
                                           SantraxLandPhoneList)
                objBLClientInfo = Nothing

                Dim objSantraxDataObject As New SantraxDataObject
                objSantraxDataObject = (From p In SantraxInfoBillCodeList Where p.ID = DropDownListSantrax.SelectedValue).SingleOrDefault
                SantraxInfoBillCodeList = Nothing

                TextBoxSantraxBillCode.Text = objSantraxDataObject.BillCode

                objSantraxDataObject = (From p In SantraxProcedureCodeQualifierList Where p.ID = DropDownListSantrax.SelectedValue).SingleOrDefault
                SantraxProcedureCodeQualifierList = Nothing

                TextBoxSantraxProcCodeQualifier.Text = objSantraxDataObject.ProcedureCodeQualifier

                objSantraxDataObject = (From p In SantraxLandPhoneList Where p.ID = DropDownListSantrax.SelectedValue).SingleOrDefault
                SantraxLandPhoneList = Nothing

                TextBoxSantraxLandPhone.Text = objSantraxDataObject.HCPCS
            Else
                TextBoxSantraxBillCode.Text = objShared.InlineAssignHelper(TextBoxSantraxProcCodeQualifier.Text,
                                              objShared.InlineAssignHelper(TextBoxSantraxLandPhone.Text, String.Empty))
            End If

        End Sub

        ''' <summary>
        ''' Clearing controls value
        ''' </summary>
        Public Sub ClearClientCaseInfoControl()

            '**********************************************Client Case[Start]*********************************

            '#Region "Client Case Information"

            DropDownListCaseWorker.SelectedIndex = 0

            TextBoxUnits.Text = objShared.InlineAssignHelper(TextBoxNumberOfWeekDays.Text, objShared.InlineAssignHelper(TextBoxInsuranceNumber.Text, String.Empty))

            '#End Region

            '#Region "Client Case Care Information"

            TextBoxSupervisorLastVisitDate.Text = objShared.InlineAssignHelper(TextBoxSupervisorVisitFrequency.Text,
                                                  objShared.InlineAssignHelper(TextBoxSuppliesOrEquipment.Text,
                                                  objShared.InlineAssignHelper(TextBoxAuthorizatioReceivedDate.Text,
                                                  objShared.InlineAssignHelper(TextBoxPractitionerStatementSentDate.Text,
                                                  objShared.InlineAssignHelper(TextBoxPractitionerStatementReceivedDate.Text,
                                                  objShared.InlineAssignHelper(TextBoxServiceInitializedReportedDate.Text,
                                                  objShared.InlineAssignHelper(TextBoxAuthorizationFromDate.Text,
                                                  objShared.InlineAssignHelper(TextBoxAuthorizationToDate.Text, String.Empty))))))))

            '#End Region

            '#Region "Client Case Billing Information"

            TextBoxAssessmentDate.Text = objShared.InlineAssignHelper(TextBoxAuthorizationNumber.Text, objShared.InlineAssignHelper(TextBoxModifierOne.Text,
                                         objShared.InlineAssignHelper(TextBoxModifierTwo.Text, objShared.InlineAssignHelper(TextBoxModifierThree.Text,
                                         objShared.InlineAssignHelper(TextBoxModifierFour.Text, objShared.InlineAssignHelper(TextBoxComments.Text,
                                         objShared.InlineAssignHelper(TextBoxUnitRate.Text, objShared.InlineAssignHelper(TextBoxClaimFrequencyTypeCode.Text, String.Empty))))))))

            DropDownListBillingDiagonosisOne.SelectedIndex = objShared.InlineAssignHelper(DropDownListBillingDiagonosisTwo.SelectedIndex,
                                                             objShared.InlineAssignHelper(DropDownListBillingDiagonosisThree.SelectedIndex,
                                                             objShared.InlineAssignHelper(DropDownListBillingDiagonosisFour.SelectedIndex, 0)))

            DropDownListBillingDiagonosisCodeOne.SelectedIndex = objShared.InlineAssignHelper(DropDownListBillingDiagonosisCodeTwo.SelectedIndex,
                                                                 objShared.InlineAssignHelper(DropDownListBillingDiagonosisCodeThree.SelectedIndex,
                                                                 objShared.InlineAssignHelper(DropDownListBillingDiagonosisCodeFour.SelectedIndex, 0)))

            ClearDropDownSelectedIndex(DropDownListProcedureCode)
            ClearDropDownSelectedIndex(DropDownListPlaceOfService)
            ClearDropDownSelectedIndex(DropDownListProviderSignatureOnFile)
            ClearDropDownSelectedIndex(DropDownListMedicareAssignmentCode)
            ClearDropDownSelectedIndex(DropDownListAssignmentOfBenefitsIndicator)
            ClearDropDownSelectedIndex(DropDownListReleaseOfInformationCode)
            ClearDropDownSelectedIndex(DropDownListPatientSignatureCode)
            '#End Region

            '#Region "Client Case Santrax Information"

            TextBoxSantraxClientId.Text = objShared.InlineAssignHelper(TextBoxSantraxARNumber.Text,
                                          objShared.InlineAssignHelper(TextBoxSantraxBillCode.Text,
                                          objShared.InlineAssignHelper(TextBoxSantraxProcCodeQualifier.Text,
                                          objShared.InlineAssignHelper(TextBoxSantraxLandPhone.Text, String.Empty))))

            DropDownListServiceCode.SelectedIndex = objShared.InlineAssignHelper(DropDownListProgramOrService.SelectedIndex,
                                                    objShared.InlineAssignHelper(DropDownListServiceCodeDescription.SelectedIndex,
                                                    objShared.InlineAssignHelper(DropDownListServiceGroup.SelectedIndex,
                                                    objShared.InlineAssignHelper(DropDownListSantraxPriority.SelectedIndex, 0))))

            '#End Region

            '**********************************************Client Case[End]*********************************

            HiddenFieldClientCaseId.Value = Convert.ToString(Int32.MinValue)
        End Sub

        Private Sub LoadSelectedClient()

            HiddenFieldIsNew.Value = Convert.ToString(False, Nothing)
            HiddenFieldIsSearched.Value = Convert.ToString(True, Nothing)
            HiddenFieldClientId.Value = IndividualId

            ButtonDelete.Enabled = If(HiddenFieldIsSearched.Value.Equals(Convert.ToString(True, Nothing)), True, False)

            Dim objBLClientInfo As New BLClientInfo()
            Dim Client As HybridDictionary = objBLClientInfo.SelectClientInfo(objShared.ConVisitel, CompanyId, IndividualId)
            objBLClientInfo = Nothing

            '**********************************************Client Info[Start]*********************************

            '#Region "Client Basic Information"

            Dim objClientBasicInfoDataObject As New ClientBasicInfoDataObject
            objClientBasicInfoDataObject = Client.Item("ClientBasicInfoDataObject")

            If objClientBasicInfoDataObject.ClientId.Equals(Int32.MinValue) Then
                Return
            End If

            If (Not objClientBasicInfoDataObject Is Nothing) Then

                TextBoxSocialSecurityNumber.Text = objClientBasicInfoDataObject.SocialSecurityNumber
                TextBoxStateClientId.Text = If(objClientBasicInfoDataObject.StateClientId.Equals(Int32.MinValue), String.Empty,
                                                Convert.ToString(objClientBasicInfoDataObject.StateClientId))
                TextBoxFirstName.Text = objClientBasicInfoDataObject.FirstName
                TextBoxLastName.Text = objClientBasicInfoDataObject.LastName
                TextBoxMiddleInitial.Text = objClientBasicInfoDataObject.MiddleNameInitial
                TextBoxPhone.Text = objClientBasicInfoDataObject.Phone
                TextBoxAlternatePhone.Text = objClientBasicInfoDataObject.AlternatePhone
                DropDownListPriority.SelectedValue = If(objClientBasicInfoDataObject.Priority.Equals(Int16.MinValue), "-1", objClientBasicInfoDataObject.Priority)
                TextBoxDateOfBirth.Text = objClientBasicInfoDataObject.DateOfBirth
                TextBoxAddress.Text = objClientBasicInfoDataObject.Address
                TextBoxApartmentNumber.Text = objClientBasicInfoDataObject.ApartmentNumber

                DropDownListCity.SelectedIndex = DropDownListCity.Items.IndexOf(DropDownListCity.Items.FindByValue(Convert.ToString(objClientBasicInfoDataObject.CityId)))
                DropDownListState.SelectedIndex = DropDownListState.Items.IndexOf(DropDownListState.Items.FindByValue(Convert.ToString(objClientBasicInfoDataObject.StateId)))

                TextBoxZip.Text = objClientBasicInfoDataObject.Zip

                'DropDownListIndividualStatus.SelectedIndex = DropDownListIndividualStatus.Items.IndexOf(DropDownListIndividualStatus.Items.FindByValue(
                '                                             Convert.ToString(HiddenFieldIndividualStatus.Value, Nothing)))

                DropDownListIndividualStatus.SelectedIndex = DropDownListIndividualStatus.Items.IndexOf(DropDownListIndividualStatus.Items.FindByValue(
                                                            Convert.ToString(objClientBasicInfoDataObject.Status)))

                DropDownListGender.SelectedValue = objClientBasicInfoDataObject.Gender
                DropDownListMaritalStatus.SelectedIndex = DropDownListMaritalStatus.Items.IndexOf(DropDownListMaritalStatus.Items.FindByValue(
                                                          Convert.ToString(objClientBasicInfoDataObject.MaritalStatusId)))

                TextBoxUpdateDate.Text = If((String.IsNullOrEmpty(Convert.ToString(objClientBasicInfoDataObject.UpdateDate, Nothing))),
                                                String.Empty,
                                                Convert.ToDateTime(objClientBasicInfoDataObject.UpdateDate).ToString(objShared.DateFormat))

                TextBoxUpdateBy.Text = objClientBasicInfoDataObject.UpdateBy

            End If

            objClientBasicInfoDataObject = Nothing

            '#End Region

            '#Region "Client Treatment Information"

            Dim objClientTreatmentInfoDataObject As New ClientTreatmentInfoDataObject
            objClientTreatmentInfoDataObject = Client.Item("ClientTreatmentInfoDataObject")

            If (Not objClientTreatmentInfoDataObject Is Nothing) Then

                TextBoxServiceStartDate.Text = If((String.IsNullOrEmpty(Convert.ToString(objClientTreatmentInfoDataObject.ServiceStartDate, Nothing))),
                                              String.Empty,
                                              Convert.ToDateTime(objClientTreatmentInfoDataObject.ServiceStartDate).ToString(objShared.DateFormat))

                TextBoxServiceEndDate.Text = If((String.IsNullOrEmpty(Convert.ToString(objClientTreatmentInfoDataObject.ServiceEndDate, Nothing))),
                                                String.Empty,
                                                Convert.ToDateTime(objClientTreatmentInfoDataObject.ServiceEndDate).ToString(objShared.DateFormat))

                DropDownListDischargeReason.SelectedIndex = DropDownListDischargeReason.Items.IndexOf(DropDownListDischargeReason.Items.FindByValue(
                                                             Convert.ToString(objClientTreatmentInfoDataObject.DischargeReason)))

                TextBoxDiagnosis.Text = objClientTreatmentInfoDataObject.Diagnosis

                DropDownListCounty.SelectedIndex = DropDownListCounty.Items.IndexOf(DropDownListCounty.Items.FindByValue(
                                                   Convert.ToString(objClientTreatmentInfoDataObject.CountyId)))

                DropDownListDoctor.SelectedIndex = DropDownListDoctor.Items.IndexOf(DropDownListDoctor.Items.FindByValue(
                                                   Convert.ToString(objClientTreatmentInfoDataObject.DoctorId)))

                DropDownListEmergencyDisasterCategory.SelectedIndex = DropDownListEmergencyDisasterCategory.Items.IndexOf(DropDownListEmergencyDisasterCategory.Items.FindByValue(
                                                                      Convert.ToString(objClientTreatmentInfoDataObject.DisasterCategory, Nothing)))

                TextBoxSupervisor.Text = objClientTreatmentInfoDataObject.Supervisor
                TextBoxLiaison.Text = objClientTreatmentInfoDataObject.Liaison

            End If

            'objClientTreatmentInfoDataObject = Nothing

            '#End Region

            '#Region "Client Emergency Contact Information"
            Dim objClientEmergencyContactInfoDataObject As New ClientEmergencyContactInfoDataObject
            objClientEmergencyContactInfoDataObject = Client.Item("ClientEmergencyContactInfoDataObject")

            If (Not objClientEmergencyContactInfoDataObject Is Nothing) Then
                TextBoxEmergencyContactOneName.Text = objClientEmergencyContactInfoDataObject.EmergencyContact
                TextBoxEmergencyContactOnePhone.Text = objClientEmergencyContactInfoDataObject.EmergencyContactPhone
                TextBoxEmergencyContactOneRelationship.Text = objClientEmergencyContactInfoDataObject.EmergencyContactRelationship
                TextBoxEmergencyContactTwoName.Text = objClientEmergencyContactInfoDataObject.EmergencyContactTwo
                TextBoxEmergencyContactTwoPhone.Text = objClientEmergencyContactInfoDataObject.EmergencyContactPhoneTwo
                TextBoxEmergencyContactTwoRelationship.Text = objClientEmergencyContactInfoDataObject.EmergencyContactRelationshipTwo
            End If

            objClientEmergencyContactInfoDataObject = Nothing
            '#End Region
            '**********************************************Client Info[End]*********************************


            '**********************************************Client Case[Start]*********************************

            '#Region "Client Case Information"

            ClientCaseList = Nothing
            ClientCaseList = Client.Item("ClientCaseList")
            Session.Add(StaticSettings.SessionField.CLIENT_CASE_LIST, ClientCaseList)


            Dim objClientCaseDataObject As New ClientCaseDataObject
            objClientCaseDataObject = (From p In ClientCaseList).FirstOrDefault

            If (Not objClientCaseDataObject Is Nothing) Then

                HiddenFieldClientCaseId.Value = objClientCaseDataObject.CaseId

                DropDownListClientType.SelectedIndex = DropDownListClientType.Items.IndexOf(DropDownListClientType.Items.FindByValue(
                                                       Convert.ToString(objClientCaseDataObject.Type)))

                DropDownListCaseWorker.SelectedIndex = DropDownListCaseWorker.Items.IndexOf(DropDownListCaseWorker.Items.FindByValue(
                                                       Convert.ToString(objClientCaseDataObject.CaseWorkerId)))

                TextBoxUnits.Text = Convert.ToString(objClientCaseDataObject.Units, Nothing)
                TextBoxNumberOfWeekDays.Text = If(objClientCaseDataObject.WeekDays.Equals(Int32.MinValue), String.Empty, Convert.ToString(objClientCaseDataObject.WeekDays))
                TextBoxInsuranceNumber.Text = objClientCaseDataObject.InsuranceNumber

                'valiation for individual status
                IndividualStatus(objClientTreatmentInfoDataObject, objClientCaseDataObject)

                objClientTreatmentInfoDataObject = Nothing
                objClientCaseDataObject = Nothing

            End If

            '#End Region


            '#Region "Client Case Care Information"

            ClientCaseCareInfoList = Nothing
            ClientCaseCareInfoList = Client.Item("ClientCaseCareInfoList")
            Session.Add(StaticSettings.SessionField.CLIENT_CASE_CARE_INFO_LIST, ClientCaseCareInfoList)

            Dim objClientCaseCareInfoDataObject As New ClientCaseCareInfoDataObject
            objClientCaseCareInfoDataObject = (From p In ClientCaseCareInfoList).FirstOrDefault

            If (Not objClientCaseCareInfoDataObject Is Nothing) Then

                TextBoxSupervisorLastVisitDate.Text = If((String.IsNullOrEmpty(Convert.ToString(objClientCaseCareInfoDataObject.SupervisorLastVisitDate, Nothing))),
                                                      String.Empty,
                                                      Convert.ToDateTime(objClientCaseCareInfoDataObject.SupervisorLastVisitDate).ToString(objShared.DateFormat))

                TextBoxSupervisorVisitFrequency.Text = If(objClientCaseCareInfoDataObject.SupervisorVisitFrequency.Equals(Int32.MinValue),
                                                          String.Empty,
                                                          Convert.ToString(objClientCaseCareInfoDataObject.SupervisorVisitFrequency))

                TextBoxSuppliesOrEquipment.Text = objClientCaseCareInfoDataObject.Supplies

                TextBoxAuthorizationFromDate.Text = If((String.IsNullOrEmpty(Convert.ToString(objClientCaseCareInfoDataObject.PlanOfCareStartDate, Nothing))),
                                                       String.Empty,
                                                       Convert.ToDateTime(objClientCaseCareInfoDataObject.PlanOfCareStartDate).ToString(objShared.DateFormat))

                TextBoxAuthorizationToDate.Text = If((String.IsNullOrEmpty(Convert.ToString(objClientCaseCareInfoDataObject.PlanOfCareEndDate, Nothing))),
                                                     String.Empty,
                                                     Convert.ToDateTime(objClientCaseCareInfoDataObject.PlanOfCareEndDate).ToString(objShared.DateFormat))

                TextBoxAuthorizatioReceivedDate.Text = If((String.IsNullOrEmpty(Convert.ToString(objClientCaseCareInfoDataObject.AuthorizationReceivedDate, Nothing))),
                                                          String.Empty,
                                                          Convert.ToDateTime(objClientCaseCareInfoDataObject.AuthorizationReceivedDate).ToString(objShared.DateFormat))

                TextBoxPractitionerStatementSentDate.Text = If((String.IsNullOrEmpty(Convert.ToString(objClientCaseCareInfoDataObject.DoctorOrderSentDate, Nothing))),
                                                               String.Empty,
                                                               Convert.ToDateTime(objClientCaseCareInfoDataObject.DoctorOrderSentDate).ToString(objShared.DateFormat))

                TextBoxPractitionerStatementReceivedDate.Text = If((String.IsNullOrEmpty(Convert.ToString(objClientCaseCareInfoDataObject.DoctorOrderReceivedDate, Nothing))),
                                                                    String.Empty,
                                                                    Convert.ToDateTime(objClientCaseCareInfoDataObject.DoctorOrderReceivedDate).ToString(objShared.DateFormat))

                TextBoxServiceInitializedReportedDate.Text = If((String.IsNullOrEmpty(Convert.ToString(objClientCaseCareInfoDataObject.ServiceInitializedReportedDate, Nothing))),
                                                                String.Empty,
                                                                Convert.ToDateTime(objClientCaseCareInfoDataObject.ServiceInitializedReportedDate).ToString(objShared.DateFormat))

                objClientCaseCareInfoDataObject = Nothing
            End If


            '#End Region

            '#Region "Client Case Billing Information"

            ClientCaseBillingInfoList = Nothing
            ClientCaseBillingInfoList = Client.Item("ClientCaseBillingInfoList")
            Session.Add(StaticSettings.SessionField.CLIENT_CASE_BILLING_INFO_LIST, ClientCaseBillingInfoList)

            Dim objClientCaseBillingInfoDataObject As New ClientCaseBillingInfoDataObject
            objClientCaseBillingInfoDataObject = (From p In ClientCaseBillingInfoList).FirstOrDefault

            If (Not objClientCaseBillingInfoDataObject Is Nothing) Then

                TextBoxAssessmentDate.Text = If((String.IsNullOrEmpty(Convert.ToString(objClientCaseBillingInfoDataObject.AssessmentDate, Nothing))),
                                            String.Empty,
                                            Convert.ToDateTime(objClientCaseBillingInfoDataObject.AssessmentDate).ToString(objShared.DateFormat))

                DropDownListBillingDiagonosisCodeOne.SelectedIndex = DropDownListBillingDiagonosisCodeOne.Items.IndexOf(DropDownListBillingDiagonosisCodeOne.Items.FindByText(
                                                                     Convert.ToString(objClientCaseBillingInfoDataObject.DiagnosisCodeOne, Nothing)))
                DropDownListBillingDiagonosisCodeOne_OnSelectedIndexChanged(Nothing, Nothing)

                DropDownListBillingDiagonosisCodeTwo.SelectedIndex = DropDownListBillingDiagonosisCodeTwo.Items.IndexOf(DropDownListBillingDiagonosisCodeTwo.Items.FindByText(
                                                                     Convert.ToString(objClientCaseBillingInfoDataObject.DiagnosisCodeTwo, Nothing)))
                DropDownListBillingDiagonosisCodeTwo_OnSelectedIndexChanged(Nothing, Nothing)

                DropDownListBillingDiagonosisCodeThree.SelectedIndex = DropDownListBillingDiagonosisCodeThree.Items.IndexOf(
                    DropDownListBillingDiagonosisCodeThree.Items.FindByText(Convert.ToString(objClientCaseBillingInfoDataObject.DiagnosisCodeThree, Nothing)))
                DropDownListBillingDiagonosisCodeThree_OnSelectedIndexChanged(Nothing, Nothing)

                DropDownListBillingDiagonosisCodeFour.SelectedIndex = DropDownListBillingDiagonosisCodeFour.Items.IndexOf(DropDownListBillingDiagonosisCodeFour.Items.FindByText(
                                                                      Convert.ToString(objClientCaseBillingInfoDataObject.DiagnosisCodeFour, Nothing)))
                DropDownListBillingDiagonosisCodeFour_OnSelectedIndexChanged(Nothing, Nothing)

                TextBoxAuthorizationNumber.Text = objClientCaseBillingInfoDataObject.AuthorizationNumber
                DropDownListProcedureCode.SelectedIndex = DropDownListProcedureCode.Items.IndexOf(DropDownListProcedureCode.Items.FindByValue(
                                                          Convert.ToString(objClientCaseBillingInfoDataObject.ProcedureId, Nothing)))

                TextBoxModifierOne.Text = objClientCaseBillingInfoDataObject.ModifierOne
                TextBoxModifierTwo.Text = objClientCaseBillingInfoDataObject.ModifierTwo
                TextBoxModifierThree.Text = objClientCaseBillingInfoDataObject.ModifierThree
                TextBoxModifierFour.Text = objClientCaseBillingInfoDataObject.ModifierFour

                DropDownListPlaceOfService.SelectedIndex = DropDownListPlaceOfService.Items.IndexOf(DropDownListPlaceOfService.Items.FindByValue(
                                                            Convert.ToString(objClientCaseBillingInfoDataObject.PlaceOfServiceId, Nothing)))

                TextBoxComments.Text = objClientCaseBillingInfoDataObject.Comments

                TextBoxUnitRate.Text = If(Not objClientCaseBillingInfoDataObject.UnitRate.Equals(0),
                                          objShared.GetFormattedPayrate(objClientCaseBillingInfoDataObject.UnitRate), String.Empty)

                TextBoxClaimFrequencyTypeCode.Text = objClientCaseBillingInfoDataObject.CLM0503ClmFrequencyTypeCode

                DropDownListProviderSignatureOnFile.SelectedIndex = DropDownListProviderSignatureOnFile.Items.IndexOf(DropDownListProviderSignatureOnFile.Items.FindByValue(
                                                                    Convert.ToString(objClientCaseBillingInfoDataObject.CLM06ProviderSignatureOnFile, Nothing)))
                DropDownListMedicareAssignmentCode.SelectedIndex = DropDownListMedicareAssignmentCode.Items.IndexOf(DropDownListMedicareAssignmentCode.Items.FindByValue(
                                                                   Convert.ToString(objClientCaseBillingInfoDataObject.CLM07MedicareAssignmentCode, Nothing)))
                DropDownListAssignmentOfBenefitsIndicator.SelectedIndex = DropDownListAssignmentOfBenefitsIndicator.Items.IndexOf(
                    DropDownListAssignmentOfBenefitsIndicator.Items.FindByValue(Convert.ToString(objClientCaseBillingInfoDataObject.CLM08AssignmentOfBenefitsIndicator, Nothing)))

                DropDownListReleaseOfInformationCode.SelectedIndex = DropDownListReleaseOfInformationCode.Items.IndexOf(DropDownListReleaseOfInformationCode.Items.FindByValue(
                                                                     Convert.ToString(objClientCaseBillingInfoDataObject.CLM09ReleaseOfInfoCode, Nothing)))

                DropDownListPatientSignatureCode.SelectedIndex = DropDownListPatientSignatureCode.Items.IndexOf(DropDownListPatientSignatureCode.Items.FindByValue(
                                                                 Convert.ToString(objClientCaseBillingInfoDataObject.CLM10PatientSignatureCode, Nothing)))

                objClientCaseBillingInfoDataObject = Nothing

            End If

            '#End Region

            '#Region "Client Case Santrax Information"

            ClientCaseSantraxInfoList = Nothing
            ClientCaseSantraxInfoList = Client.Item("ClientCaseSantraxInfoList")
            Session.Add(StaticSettings.SessionField.CLIENT_CASE_SANTRAX_INFO_LIST, ClientCaseSantraxInfoList)

            Dim objClientCaseSantraxInfoDataObject As New ClientCaseSantraxInfoDataObject
            objClientCaseSantraxInfoDataObject = (From p In ClientCaseSantraxInfoList).FirstOrDefault

            If (Not objClientCaseSantraxInfoDataObject Is Nothing) Then
                TextBoxSantraxClientId.Text = objClientCaseSantraxInfoDataObject.SantraxClientId


                DropDownListProgramOrService.SelectedIndex = DropDownListProgramOrService.Items.IndexOf(DropDownListProgramOrService.Items.FindByText(
                                                             Convert.ToString(objClientCaseSantraxInfoDataObject.ServiceCode, Nothing)))
                DropDownListProgramOrService_OnSelectedIndexChanged(Nothing, Nothing)

                DropDownListServiceCode.SelectedIndex = DropDownListServiceCode.Items.IndexOf(DropDownListServiceCode.Items.FindByText(
                                                        Convert.ToString(objClientCaseSantraxInfoDataObject.ServiceCode, Nothing)))
                DropDownListServiceCode_OnSelectedIndexChanged(Nothing, Nothing)

                DropDownListServiceCodeDescription.SelectedIndex = DropDownListServiceCodeDescription.Items.IndexOf(DropDownListServiceCodeDescription.Items.FindByText(
                                                                   Convert.ToString(objClientCaseSantraxInfoDataObject.ServiceCodeDescription, Nothing)))
                DropDownListServiceCodeDescription_OnSelectedIndexChanged(Nothing, Nothing)

                DropDownListServiceGroup.SelectedIndex = DropDownListServiceGroup.Items.IndexOf(DropDownListServiceGroup.Items.FindByText(
                                                         Convert.ToString(objClientCaseSantraxInfoDataObject.ServiceGroup, Nothing)))
                DropDownListServiceGroup_OnSelectedIndexChanged(Nothing, Nothing)

                TextBoxSantraxARNumber.Text = objClientCaseSantraxInfoDataObject.SantraxARNumber

                DropDownListSantraxPriority.SelectedValue = If(objClientCaseSantraxInfoDataObject.SantraxPriority.Equals(Int32.MinValue),
                                                               "-1",
                                                               Convert.ToString(objClientCaseSantraxInfoDataObject.SantraxPriority))

                TextBoxSantraxBillCode.Text = objClientCaseSantraxInfoDataObject.BillCode
                TextBoxSantraxProcCodeQualifier.Text = objClientCaseSantraxInfoDataObject.ProcedureCodeQualifier
                TextBoxSantraxLandPhone.Text = objClientCaseSantraxInfoDataObject.LandPhone

                objClientCaseSantraxInfoDataObject = Nothing
            End If

            '#End Region

            '**********************************************Client Case[End]*********************************
            Client.Clear()
            Client = Nothing
        End Sub

        ''' <summary>
        ''' Clearing controls value
        ''' </summary>
        Public Sub ClearClientInfoControl()

            '#Region "Client Basic Information"
            TextBoxStateClientId.Text = objShared.InlineAssignHelper(TextBoxSocialSecurityNumber.Text, objShared.InlineAssignHelper(TextBoxFirstName.Text,
                                        objShared.InlineAssignHelper(TextBoxLastName.Text, objShared.InlineAssignHelper(TextBoxMiddleInitial.Text,
                                        objShared.InlineAssignHelper(TextBoxPhone.Text, objShared.InlineAssignHelper(TextBoxAlternatePhone.Text,
                                        objShared.InlineAssignHelper(TextBoxDateOfBirth.Text, objShared.InlineAssignHelper(TextBoxAddress.Text,
                                        objShared.InlineAssignHelper(TextBoxApartmentNumber.Text, objShared.InlineAssignHelper(TextBoxZip.Text,
                                        objShared.InlineAssignHelper(TextBoxZip.Text, objShared.InlineAssignHelper(TextBoxAssessmentDate.Text,
                                        objShared.InlineAssignHelper(TextBoxSupervisorLastVisitDate.Text, objShared.InlineAssignHelper(TextBoxUpdateDate.Text,
                                        objShared.InlineAssignHelper(TextBoxUpdateBy.Text, String.Empty)))))))))))))))

            DropDownListPriority.SelectedIndex = objShared.InlineAssignHelper(DropDownListCity.SelectedIndex, objShared.InlineAssignHelper(DropDownListState.SelectedIndex,
                                                 objShared.InlineAssignHelper(DropDownListIndividualStatus.SelectedIndex, objShared.InlineAssignHelper(DropDownListGender.SelectedIndex,
                                                 objShared.InlineAssignHelper(DropDownListMaritalStatus.SelectedIndex, 0)))))

            'ButtonIndividual.Text = "Search Client"

            '#End Region

            '#Region "Client Treatment Information"
            TextBoxServiceStartDate.Text = objShared.InlineAssignHelper(TextBoxServiceEndDate.Text, objShared.InlineAssignHelper(TextBoxDiagnosis.Text,
                                           objShared.InlineAssignHelper(TextBoxSupervisor.Text, objShared.InlineAssignHelper(TextBoxLiaison.Text, String.Empty))))

            DropDownListDischargeReason.SelectedIndex = objShared.InlineAssignHelper(DropDownListCounty.SelectedIndex,
                                                        objShared.InlineAssignHelper(DropDownListDoctor.SelectedIndex,
                                                        objShared.InlineAssignHelper(DropDownListEmergencyDisasterCategory.SelectedIndex, 0)))

            '#End Region

            '#Region "Client Emergency Contact Information"

            TextBoxEmergencyContactOneName.Text = objShared.InlineAssignHelper(TextBoxEmergencyContactOnePhone.Text,
                                                  objShared.InlineAssignHelper(TextBoxEmergencyContactOneRelationship.Text,
                                                  objShared.InlineAssignHelper(TextBoxEmergencyContactTwoName.Text,
                                                  objShared.InlineAssignHelper(TextBoxEmergencyContactTwoPhone.Text,
                                                  objShared.InlineAssignHelper(TextBoxEmergencyContactTwoPhone.Text,
                                                  objShared.InlineAssignHelper(TextBoxEmergencyContactTwoRelationship.Text,
                                                  String.Empty))))))

            '#End Region

            DropDownListClientType.SelectedIndex = 0

            HiddenFieldIsNew.Value = Convert.ToString(True, Nothing)
            HiddenFieldIsSearched.Value = Convert.ToString(True, Nothing)
            HiddenFieldClientId.Value = Convert.ToString(Int32.MinValue)
            HiddenFieldClientCaseId.Value = Convert.ToString(Int32.MinValue)
        End Sub

        ''' <summary>
        ''' Set Company, User Id, and Individual Id
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub SetClientId(ClientId As Int64)
            IndividualId = ClientId
            CompanyId = objShared.CompanyId
            UserId = objShared.UserId
        End Sub

        Private Sub DeleteData()

            If HiddenFieldIsSearched.Value.Equals(Convert.ToString(True, Nothing)) Then

                Dim IsDeleted As Boolean = False
                Dim objBLClientInfo As New BLClientInfo()

                Try
                    objBLClientInfo.DeleteClientInfo(objShared.ConVisitel, CompanyId, IndividualId, Convert.ToInt32(HiddenFieldClientCaseId.Value), UserId)
                    IsDeleted = True

                Catch ex As SqlException
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to Delete")
                End Try

                objBLClientInfo = Nothing

                If (IsDeleted) Then

                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DeleteMessage)

                    ClearClientInfoControl()
                    ClearClientCaseInfoControl()

                    ButtonDelete.Enabled = False

                    HiddenFieldIsNew.Value = Convert.ToString(True, Nothing)
                    HiddenFieldIsSearched.Value = Convert.ToString(False, Nothing)

                End If

            End If

        End Sub

        ''' <summary>
        ''' This is for control events regristering and instantiate object globally
        ''' </summary>
        Private Sub InitializeControl()

            TextBoxUpdateDate.ReadOnly = objShared.InlineAssignHelper(TextBoxUpdateBy.ReadOnly, True)

            TextBoxSupervisorVisitFrequency.Attributes.Add("onkeypress", "return isNumericKey(event);")
            TextBoxAge.Attributes.Add("onkeypress", "return isNumericKey(event);")
            TextBoxNumberOfWeekDays.Attributes.Add("onkeypress", "return isNumericKey(event);")
            TextBoxPhone.Attributes.Add("onchange", "onChangeOfValue(this);")

            SetControlToolTip()

            SetRegularExpressionSetting()

            DefineControlTextLength()

            LinkButtonAddMoreIndividualStatus.Attributes("href") = "../Settings/Popup/ClientStatusPopup.aspx?Mode=Insert&TB_iframe=true&height=500&width=720"
            LinkButtonAddMoreIndividualStatus.Attributes("title") = "Client Status Setting"

            LinkButtonAddMoreCity.Attributes("href") = "../Settings/Popup/CityPopup.aspx?Mode=Insert&TB_iframe=true&height=450&width=620"
            LinkButtonAddMoreCity.Attributes("title") = "City Setting"

            LinkButtonAddMoreDischargeReason.Attributes("href") = "../Settings/Popup/DischargeReasonPopup.aspx?Mode=Insert&TB_iframe=true&height=450&width=620"
            LinkButtonAddMoreDischargeReason.Attributes("title") = "Discharge Reason Setting"

            LinkButtonAddMoreCaseWorker.Attributes("href") = "../Settings/Popup/CaseWorkerPopup.aspx?Mode=Insert&TB_iframe=true&height=550&width=980"
            LinkButtonAddMoreCaseWorker.Attributes("title") = "Case Worker Setting"

            LinkButtonAddMoreState.Attributes("href") = "../Settings/Popup/StatePopup.aspx?Mode=Insert&TB_iframe=true&height=450&width=620"
            LinkButtonAddMoreState.Attributes("title") = "State Setting"

            LinkButtonAddMoreCounty.Attributes("href") = "../Settings/Popup/CountyPopup.aspx?Mode=Insert&TB_iframe=true&height=450&width=620"
            LinkButtonAddMoreCounty.Attributes("title") = "County Setting"

            LinkButtonAddMoreDoctor.Attributes("href") = "../Settings/Popup/DoctorPopup.aspx?Mode=Insert&TB_iframe=true&height=550&width=980"
            LinkButtonAddMoreDoctor.Attributes("title") = "Doctor Setting"

            LinkButtonAddMoreClientType.Attributes("href") = "../Settings/Popup/ClientTypePopup.aspx?Mode=Insert&TB_iframe=true&height=550&width=980"
            LinkButtonAddMoreClientType.Attributes("title") = "Client Type Setting"

            ButtonDelete.Attributes("onClick") = String.Format("return confirm('Do you want to delete? ')")

            ButtonCoordinationOfCare.Attributes.Add("OnClick", String.Format("ShowCoordinationOfCareReport(); return false; "))
            ButtonCoordinationOfCare.ClientIDMode = ClientIDMode.Static
            ButtonCoordinationOfCare.UseSubmitBehavior = False

            ButtonSave.CausesValidation = True
            ButtonSave.ValidationGroup = ValidationGroup

            ButtonDelete.CausesValidation = objShared.InlineAssignHelper(ButtonClear.CausesValidation, False)

            AddHandler ButtonSave.Click, AddressOf ButtonSave_Click
            AddHandler ButtonDelete.Click, AddressOf ButtonDelete_Click

            AddHandler ButtonClear.Click, AddressOf ButtonClear_Click

            DropDownListBillingDiagonosisOne.ClientIDMode = objShared.InlineAssignHelper(DropDownListBillingDiagonosisCodeOne.ClientIDMode,
                                                          objShared.InlineAssignHelper(DropDownListBillingDiagonosisTwo.ClientIDMode,
                                                          objShared.InlineAssignHelper(DropDownListBillingDiagonosisCodeTwo.ClientIDMode,
                                                          objShared.InlineAssignHelper(DropDownListBillingDiagonosisThree.ClientIDMode,
                                                          objShared.InlineAssignHelper(DropDownListBillingDiagonosisCodeThree.ClientIDMode,
                                                          objShared.InlineAssignHelper(DropDownListBillingDiagonosisFour.ClientIDMode,
                                                          objShared.InlineAssignHelper(DropDownListBillingDiagonosisCodeFour.ClientIDMode,
                                                          objShared.InlineAssignHelper(DropDownListProgramOrService.ClientIDMode,
                                                          objShared.InlineAssignHelper(DropDownListServiceGroup.ClientIDMode,
                                                          objShared.InlineAssignHelper(DropDownListServiceCode.ClientIDMode,
                                                          objShared.InlineAssignHelper(DropDownListServiceCodeDescription.ClientIDMode,
                                                          objShared.InlineAssignHelper(DropDownListClientType.ClientIDMode,
                                                          UI.ClientIDMode.Static))))))))))))

            TextBoxPhone.ClientIDMode = objShared.InlineAssignHelper(TextBoxSantraxLandPhone.ClientIDMode, objShared.InlineAssignHelper(TextBoxAddress.ClientIDMode,
                                        objShared.InlineAssignHelper(TextBoxDiagnosis.ClientIDMode, objShared.InlineAssignHelper(TextBoxSuppliesOrEquipment.ClientIDMode,
                                        objShared.InlineAssignHelper(TextBoxServiceStartDate.ClientIDMode, objShared.InlineAssignHelper(TextBoxServiceEndDate.ClientIDMode,
                                        objShared.InlineAssignHelper(TextBoxDateOfBirth.ClientIDMode, objShared.InlineAssignHelper(TextBoxAuthorizationFromDate.ClientIDMode,
                                        objShared.InlineAssignHelper(TextBoxAuthorizationToDate.ClientIDMode, objShared.InlineAssignHelper(TextBoxAssessmentDate.ClientIDMode,
                                        objShared.InlineAssignHelper(TextBoxSupervisorLastVisitDate.ClientIDMode,
                                        objShared.InlineAssignHelper(TextBoxAuthorizatioReceivedDate.ClientIDMode,
                                        objShared.InlineAssignHelper(TextBoxPractitionerStatementReceivedDate.ClientIDMode,
                                        objShared.InlineAssignHelper(TextBoxPractitionerStatementSentDate.ClientIDMode,
                                        objShared.InlineAssignHelper(TextBoxServiceInitializedReportedDate.ClientIDMode,
                                        UI.ClientIDMode.Static)))))))))))))))

            DropDownListBillingDiagonosisOne.AutoPostBack = objShared.InlineAssignHelper(DropDownListBillingDiagonosisCodeOne.AutoPostBack,
                                                          objShared.InlineAssignHelper(DropDownListBillingDiagonosisTwo.AutoPostBack,
                                                          objShared.InlineAssignHelper(DropDownListBillingDiagonosisCodeTwo.AutoPostBack,
                                                          objShared.InlineAssignHelper(DropDownListBillingDiagonosisThree.AutoPostBack,
                                                          objShared.InlineAssignHelper(DropDownListBillingDiagonosisCodeThree.AutoPostBack,
                                                          objShared.InlineAssignHelper(DropDownListBillingDiagonosisFour.AutoPostBack,
                                                          objShared.InlineAssignHelper(DropDownListBillingDiagonosisCodeFour.AutoPostBack,
                                                          objShared.InlineAssignHelper(DropDownListProgramOrService.AutoPostBack,
                                                          objShared.InlineAssignHelper(DropDownListServiceGroup.AutoPostBack,
                                                          objShared.InlineAssignHelper(DropDownListServiceCode.AutoPostBack,
                                                          objShared.InlineAssignHelper(DropDownListServiceCodeDescription.AutoPostBack,
                                                          objShared.InlineAssignHelper(DropDownListClientType.AutoPostBack,
                                                          True))))))))))))

            AddHandler DropDownListBillingDiagonosisOne.SelectedIndexChanged, AddressOf DropDownListBillingDiagonosisOne_OnSelectedIndexChanged
            AddHandler DropDownListBillingDiagonosisCodeOne.SelectedIndexChanged, AddressOf DropDownListBillingDiagonosisCodeOne_OnSelectedIndexChanged
            AddHandler DropDownListBillingDiagonosisTwo.SelectedIndexChanged, AddressOf DropDownListBillingDiagonosisTwo_OnSelectedIndexChanged
            AddHandler DropDownListBillingDiagonosisCodeTwo.SelectedIndexChanged, AddressOf DropDownListBillingDiagonosisCodeTwo_OnSelectedIndexChanged
            AddHandler DropDownListBillingDiagonosisThree.SelectedIndexChanged, AddressOf DropDownListBillingDiagonosisThree_OnSelectedIndexChanged
            AddHandler DropDownListBillingDiagonosisCodeThree.SelectedIndexChanged, AddressOf DropDownListBillingDiagonosisCodeThree_OnSelectedIndexChanged
            AddHandler DropDownListBillingDiagonosisFour.SelectedIndexChanged, AddressOf DropDownListBillingDiagonosisFour_OnSelectedIndexChanged
            AddHandler DropDownListBillingDiagonosisCodeFour.SelectedIndexChanged, AddressOf DropDownListBillingDiagonosisCodeFour_OnSelectedIndexChanged
            AddHandler DropDownListProgramOrService.SelectedIndexChanged, AddressOf DropDownListProgramOrService_OnSelectedIndexChanged
            AddHandler DropDownListServiceGroup.SelectedIndexChanged, AddressOf DropDownListServiceGroup_OnSelectedIndexChanged
            AddHandler DropDownListServiceCode.SelectedIndexChanged, AddressOf DropDownListServiceCode_OnSelectedIndexChanged
            AddHandler DropDownListServiceCodeDescription.SelectedIndexChanged, AddressOf DropDownListServiceCodeDescription_OnSelectedIndexChanged
            AddHandler DropDownListClientType.SelectedIndexChanged, AddressOf DropDownListClientType_OnSelectedIndexChanged

        End Sub

        Private Sub SetControlToolTip()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("Setup", ControlName & Convert.ToString(".resx"))

            Dim DateToolTip As String = Convert.ToString(ResourceTable("DateFieldToolTip"), Nothing)
            DateToolTip = If(String.IsNullOrEmpty(DateToolTip), "Example: August 21, 2014", DateToolTip)

            TextBoxAssessmentDate.ToolTip = objShared.InlineAssignHelper(TextBoxDateOfBirth.ToolTip,
                                            objShared.InlineAssignHelper(TextBoxServiceStartDate.ToolTip,
                                            objShared.InlineAssignHelper(TextBoxServiceEndDate.ToolTip,
                                            objShared.InlineAssignHelper(TextBoxSupervisorLastVisitDate.ToolTip,
                                            objShared.InlineAssignHelper(TextBoxAuthorizationFromDate.ToolTip,
                                            objShared.InlineAssignHelper(TextBoxAuthorizationToDate.ToolTip,
                                            objShared.InlineAssignHelper(TextBoxAuthorizatioReceivedDate.ToolTip,
                                            objShared.InlineAssignHelper(TextBoxPractitionerStatementReceivedDate.ToolTip,
                                            objShared.InlineAssignHelper(TextBoxPractitionerStatementSentDate.ToolTip,
                                            objShared.InlineAssignHelper(TextBoxServiceInitializedReportedDate.ToolTip,
                                            DateToolTip))))))))))

            '#Region "Client Case Billing Information"

            TextBoxClaimFrequencyTypeCode.ToolTip = Convert.ToString(ResourceTable("TextBoxClaimFrequencyTypeCodeToolTip"), Nothing)
            TextBoxClaimFrequencyTypeCode.ToolTip = If(String.IsNullOrEmpty(TextBoxClaimFrequencyTypeCode.ToolTip),
                                                    "Identifier used to track a claim from creation by the health care provider through payment",
                                                    TextBoxClaimFrequencyTypeCode.ToolTip)

            DropDownListProviderSignatureOnFile.ToolTip = Convert.ToString(ResourceTable("DropDownListProviderSignatureOnFileToolTip"), Nothing)
            DropDownListProviderSignatureOnFile.ToolTip = If(String.IsNullOrEmpty(DropDownListProviderSignatureOnFile.ToolTip),
                                                             "This is provider signature on file indicator. A 'Y' value indicates the provider signature is on file; an'N' value indicates the provider signatue is not on",
                                                             DropDownListProviderSignatureOnFile.ToolTip)

            DropDownListMedicareAssignmentCode.ToolTip = Convert.ToString(ResourceTable("DropDownListMedicareAssignmentCodeToolTip"), Nothing)
            DropDownListMedicareAssignmentCode.ToolTip = If(String.IsNullOrEmpty(DropDownListMedicareAssignmentCode.ToolTip),
                                                            "Indicates whether the provider accepts Medicare",
                                                            DropDownListMedicareAssignmentCode.ToolTip)

            DropDownListAssignmentOfBenefitsIndicator.ToolTip = Convert.ToString(ResourceTable("DropDownListAssignmentOfBenefitsIndicatorToolTip"), Nothing)
            DropDownListAssignmentOfBenefitsIndicator.ToolTip = If(String.IsNullOrEmpty(DropDownListAssignmentOfBenefitsIndicator.ToolTip),
                                                                   "This is assignment of benefits indicator. A 'Y' value indicates insured or authorized person authorizes benefits to be assigned to the provider; an 'N' value indicates benefits have not been assigned to the provider.",
                                                                   DropDownListAssignmentOfBenefitsIndicator.ToolTip)

            DropDownListReleaseOfInformationCode.ToolTip = Convert.ToString(ResourceTable("DropDownListReleaseOfInformationCodeToolTip"), Nothing)
            DropDownListReleaseOfInformationCode.ToolTip = If(String.IsNullOrEmpty(DropDownListReleaseOfInformationCode.ToolTip),
                                                              "Code indicating whether the provider has on file a signed statement by the patient",
                                                              DropDownListReleaseOfInformationCode.ToolTip)

            DropDownListPatientSignatureCode.ToolTip = Convert.ToString(ResourceTable("DropDownListPatientSignatureCodeToolTip"), Nothing)
            DropDownListPatientSignatureCode.ToolTip = If(String.IsNullOrEmpty(DropDownListPatientSignatureCode.ToolTip),
                                                          "Code indicating how the patient or subscriber authorization signatures were obtained and how they are being retained by the provider",
                                                          DropDownListPatientSignatureCode.ToolTip)

            '#End Region

            ResourceTable = Nothing

        End Sub

        Private Sub SetRegularExpressionSetting()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("Setup", ControlName & Convert.ToString(".resx"))

            Dim ErrorMessage As String = String.Empty, ErrorText As String = String.Empty

            ErrorText = Convert.ToString(ResourceTable("InvalidDateCommonMessage"), Nothing)
            ErrorText = If(String.IsNullOrEmpty(ErrorText), "Invalid", ErrorText)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidAssessmentDateMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Assessment Date.", ErrorMessage)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorAssessmentDate, "TextBoxAssessmentDate", objShared.DateValidationExpression, ErrorMessage,
                                                 ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidDateOfBirthMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Date Of Birth.", ErrorMessage)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorDateOfBirth, "TextBoxDateOfBirth", objShared.DateValidationExpression, ErrorMessage,
                                                 ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidServiceStartDateMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Service Start Date.", ErrorMessage)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorStartDate, "TextBoxServiceStartDate", objShared.DateValidationExpression, ErrorMessage,
                                                 ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidServiceEndDateMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Service End Date.", ErrorMessage)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorEndDate, "TextBoxServiceEndDate", objShared.DateValidationExpression, ErrorMessage,
                                                 ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidSupervisorLastVisitDateMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Supervisor Last Visit Date.", ErrorMessage)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorSupervisorLastVisitDate, "TextBoxSupervisorLastVisitDate",
                                                           objShared.DateValidationExpression, ErrorMessage, ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidAuthorizationFromDateMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Authorization From Date.", ErrorMessage)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorAuthorizationFromDate, "TextBoxAuthorizationFromDate",
                                                           objShared.DateValidationExpression, ErrorMessage, ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidAuthorizationToDateMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Authorization To Date.", ErrorMessage)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorAuthorizationToDate, "TextBoxAuthorizationToDate",
                                                           objShared.DateValidationExpression, ErrorMessage, ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidAuthorizationReceivedDateMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Authorization Received Date.", ErrorMessage)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorAuthorizatioReceivedDate, "TextBoxAuthorizatioReceivedDate",
                                                           objShared.DateValidationExpression, ErrorMessage, ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidPractitionerStatementReceivedDateMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Practitioner Statement Received Date.", ErrorMessage)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorPractitionerStatementReceivedDate, "TextBoxPractitionerStatementReceivedDate",
                                                 objShared.DateValidationExpression, ErrorMessage, ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidPractitionerStatementSentDateMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Practitioner Statement Sent Date.", ErrorMessage)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorPractitionerStatementSentDate, "TextBoxPractitionerStatementSentDate",
                                                           objShared.DateValidationExpression, ErrorMessage, ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidServiceInitializedReportedDateMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Service Initialized Reported Date.", ErrorMessage)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorServiceInitializedReportedDate, "TextBoxServiceInitializedReportedDate",
                                                           objShared.DateValidationExpression, ErrorMessage, ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidPhoneMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Phone", ErrorMessage)

            ErrorText = ErrorMessage

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorPhone, "TextBoxPhone", objShared.PhoneValidationExpression, ErrorMessage,
                                                 ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidAlternatePhoneMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Alternate Phone", ErrorMessage)

            ErrorText = ErrorMessage

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorAlternatePhone, "TextBoxAlternatePhone", objShared.PhoneValidationExpression, ErrorMessage,
                                                 ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidEmergencyContactOnePhoneMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Emergency Contact One Phone", ErrorMessage)

            ErrorText = ErrorMessage

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorEmergencyContactOnePhone, "TextBoxEmergencyContactOnePhone",
                                                           objShared.PhoneValidationExpression, ErrorMessage, ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidEmergencyContactTwoPhoneMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Emergency Contact Two Phone", ErrorMessage)

            ErrorText = ErrorMessage

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorEmergencyContactTwoPhone, "TextBoxEmergencyContactTwoPhone",
                                                           objShared.PhoneValidationExpression, ErrorMessage, ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidSantraxLandPhoneMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Santrax Land Phone", ErrorMessage)

            ErrorText = ErrorMessage

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorSantraxLandPhone, "TextBoxSantraxLandPhone", objShared.PhoneValidationExpression,
                                                 ErrorMessage, ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidSocialSecurityNumberMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Social Security Number.", ErrorMessage)

            ErrorText = ErrorMessage

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorSocialSecurityNumber, "TextBoxSocialSecurityNumber",
                                                           objShared.SocialSecurityNumberValidationExpression, ErrorMessage, ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidUnitsMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Units", ErrorMessage)

            ErrorText = ErrorMessage

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorUnits, "TextBoxUnits", objShared.TimeValidationExpression,
                                                 ErrorMessage, ErrorText, ValidationEnable, ValidationGroup)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorUnitRate, "TextBoxUnitRate", objShared.DecimalValueWithDollarSign,
                                                 InvalidUnitRateMessage, InvalidUnitRateMessage, ValidationEnable, ValidationGroup)
            ResourceTable = Nothing

            ErrorMessage = Nothing
            ErrorText = Nothing

        End Sub

        Private Sub DefineControlTextLength()

            '**********************************************Client Info[Start]*********************************

            objShared.SetControlTextLength(TextBoxSocialSecurityNumber, 11)

            objShared.SetControlTextLength(TextBoxFirstName, 20)
            objShared.SetControlTextLength(TextBoxLastName, 20)

            objShared.SetControlTextLength(TextBoxMiddleInitial, 10)
            objShared.SetControlTextLength(TextBoxStateClientId, 10)
            objShared.SetControlTextLength(TextBoxApartmentNumber, 10)

            objShared.SetControlTextLength(TextBoxDateOfBirth, 50)
            objShared.SetControlTextLength(TextBoxSocialSecurityNumber, 50)
            objShared.SetControlTextLength(TextBoxAddress, 50)
            objShared.SetControlTextLength(TextBoxSupervisor, 50)
            objShared.SetControlTextLength(TextBoxEmergencyContactOneName, 50)
            objShared.SetControlTextLength(TextBoxEmergencyContactOneRelationship, 50)
            objShared.SetControlTextLength(TextBoxLiaison, 50)
            objShared.SetControlTextLength(TextBoxEmergencyContactTwoRelationship, 50)
            objShared.SetControlTextLength(TextBoxEmergencyContactTwoName, 50)

            objShared.SetControlTextLength(TextBoxPhone, 14)
            objShared.SetControlTextLength(TextBoxAlternatePhone, 14)

            objShared.SetControlTextLength(TextBoxDiagnosis, 255)

            objShared.SetControlTextLength(TextBoxZip, 16)
            objShared.SetControlTextLength(TextBoxEmergencyContactOnePhone, 16)
            objShared.SetControlTextLength(TextBoxEmergencyContactTwoPhone, 16)

            objShared.SetControlTextLength(TextBoxAge, 3)

            '**********************************************Client Info[End]*********************************


            '**********************************************Client[Start]*********************************

            objShared.SetControlTextLength(TextBoxNumberOfWeekDays, 10)
            objShared.SetControlTextLength(TextBoxSantraxClientId, 10)
            objShared.SetControlTextLength(TextBoxUnitRate, 10)

            objShared.SetControlTextLength(TextBoxInsuranceNumber, 50)
            objShared.SetControlTextLength(TextBoxSantraxProcCodeQualifier, 50)

            objShared.SetControlTextLength(TextBoxSupervisorVisitFrequency, 2)

            objShared.SetControlTextLength(TextBoxSuppliesOrEquipment, 250)
            objShared.SetControlTextLength(TextBoxComments, 250)

            objShared.SetControlTextLength(TextBoxSantraxARNumber, 12)

            objShared.SetControlTextLength(TextBoxSantraxBillCode, 8)
            objShared.SetControlTextLength(TextBoxUnits, 8)

            objShared.SetControlTextLength(TextBoxSantraxLandPhone, 14)

            objShared.SetControlTextLength(TextBoxAuthorizationNumber, 50)

            objShared.SetControlTextLength(TextBoxModifierOne, 2)
            objShared.SetControlTextLength(TextBoxModifierTwo, 2)
            objShared.SetControlTextLength(TextBoxModifierThree, 2)
            objShared.SetControlTextLength(TextBoxModifierFour, 2)

            objShared.SetControlTextLength(TextBoxClaimFrequencyTypeCode, 1)

            '**********************************************Client[End]*********************************

        End Sub

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("ClientInfo", ControlName & Convert.ToString(".resx"))

            '**********************************************Client Info[Start]************************************************************

            LabelClientInformationEntry.Text = Convert.ToString(ResourceTable("LabelClientInformationEntry"), Nothing).Trim()
            LabelClientInformationEntry.Text = If(String.IsNullOrEmpty(LabelClientInformationEntry.Text), "Individual Detail Info", LabelClientInformationEntry.Text)

            '#Region "Client Basic Information"

            LabelIndividualStatus.Text = Convert.ToString(ResourceTable("LabelIndividualStatus"), Nothing).Trim()
            LabelIndividualStatus.Text = If(String.IsNullOrEmpty(LabelIndividualStatus.Text), "Individual Status", LabelIndividualStatus.Text)

            LabelStateClientId.Text = Convert.ToString(ResourceTable("LabelStateClientId"), Nothing).Trim()
            LabelStateClientId.Text = If(String.IsNullOrEmpty(LabelStateClientId.Text), "State Id (Medicaid#):", LabelStateClientId.Text)

            LabelFirstName.Text = Convert.ToString(ResourceTable("LabelFirstName"), Nothing).Trim()
            LabelFirstName.Text = If(String.IsNullOrEmpty(LabelFirstName.Text), "First Name", LabelFirstName.Text)

            LabelLastName.Text = Convert.ToString(ResourceTable("LabelLastName"), Nothing).Trim()
            LabelLastName.Text = If(String.IsNullOrEmpty(LabelLastName.Text), "Last Name", LabelLastName.Text)

            LabelMiddleInitial.Text = Convert.ToString(ResourceTable("LabelMiddleInitial"), Nothing).Trim()
            LabelMiddleInitial.Text = If(String.IsNullOrEmpty(LabelMiddleInitial.Text), "M.I", LabelMiddleInitial.Text)

            LabelPhone.Text = Convert.ToString(ResourceTable("LabelPhone"), Nothing).Trim()
            LabelPhone.Text = If(String.IsNullOrEmpty(LabelPhone.Text), "Phone", LabelPhone.Text)

            LabelAlternatePhone.Text = Convert.ToString(ResourceTable("LabelAlternatePhone"), Nothing).Trim()
            LabelAlternatePhone.Text = If(String.IsNullOrEmpty(LabelAlternatePhone.Text), "Other Phone", LabelAlternatePhone.Text)

            LabelPriority.Text = Convert.ToString(ResourceTable("LabelPriority"), Nothing).Trim()
            LabelPriority.Text = If(String.IsNullOrEmpty(LabelPriority.Text), "Priority", LabelPriority.Text)

            LabelDateOfBirth.Text = Convert.ToString(ResourceTable("LabelDateOfBirth"), Nothing).Trim()
            LabelDateOfBirth.Text = If(String.IsNullOrEmpty(LabelDateOfBirth.Text), "DOB", LabelDateOfBirth.Text)

            LabelSocialSecurityNumber.Text = Convert.ToString(ResourceTable("LabelSocialSecurityNumber"), Nothing).Trim()
            LabelSocialSecurityNumber.Text = If(String.IsNullOrEmpty(LabelSocialSecurityNumber.Text), "SS#", LabelSocialSecurityNumber.Text)

            LabelAddress.Text = Convert.ToString(ResourceTable("LabelAddress"), Nothing).Trim()
            LabelAddress.Text = If(String.IsNullOrEmpty(LabelAddress.Text), "Address", LabelAddress.Text)

            LabelApartmentNumber.Text = Convert.ToString(ResourceTable("LabelApartmentNumber"), Nothing).Trim()
            LabelApartmentNumber.Text = If(String.IsNullOrEmpty(LabelApartmentNumber.Text), "Apt #", LabelApartmentNumber.Text)

            LabelCity.Text = Convert.ToString(ResourceTable("LabelCity"), Nothing).Trim()
            LabelCity.Text = If(String.IsNullOrEmpty(LabelCity.Text), "City", LabelCity.Text)

            LabelState.Text = Convert.ToString(ResourceTable("LabelState"), Nothing).Trim()
            LabelState.Text = If(String.IsNullOrEmpty(LabelState.Text), "State", LabelState.Text)

            LabelZip.Text = Convert.ToString(ResourceTable("LabelZip"), Nothing).Trim()
            LabelZip.Text = If(String.IsNullOrEmpty(LabelZip.Text), "Zip", LabelZip.Text)

            LabelGender.Text = Convert.ToString(ResourceTable("LabelGender"), Nothing).Trim()
            LabelGender.Text = If(String.IsNullOrEmpty(LabelGender.Text), "Gender", LabelGender.Text)

            LabelMaritalStatus.Text = Convert.ToString(ResourceTable("LabelMaritalStatus"), Nothing).Trim()
            LabelMaritalStatus.Text = If(String.IsNullOrEmpty(LabelMaritalStatus.Text), "Marital Status", LabelMaritalStatus.Text)

            '#End Region

            '#Region "Client Treatment Information"

            LabelServiceStartDate.Text = Convert.ToString(ResourceTable("LabelServiceStartDate"), Nothing).Trim()
            LabelServiceStartDate.Text = If(String.IsNullOrEmpty(LabelServiceStartDate.Text), "Service Start Date", LabelServiceStartDate.Text)

            LabelServiceEndDate.Text = Convert.ToString(ResourceTable("LabelServiceEndDate"), Nothing).Trim()
            LabelServiceEndDate.Text = If(String.IsNullOrEmpty(LabelServiceEndDate.Text), "Service End Date", LabelServiceEndDate.Text)

            LabelDischargeReason.Text = Convert.ToString(ResourceTable("LabelDischargeReason"), Nothing).Trim()
            LabelDischargeReason.Text = If(String.IsNullOrEmpty(LabelDischargeReason.Text), "Discharge Reason", LabelDischargeReason.Text)

            LabelDiagnosis.Text = Convert.ToString(ResourceTable("LabelDiagnosis"), Nothing).Trim()
            LabelDiagnosis.Text = If(String.IsNullOrEmpty(LabelDiagnosis.Text), "Diagnosis", LabelDiagnosis.Text)

            LabelCounty.Text = Convert.ToString(ResourceTable("LabelCounty"), Nothing).Trim()
            LabelCounty.Text = If(String.IsNullOrEmpty(LabelCounty.Text), "County", LabelCounty.Text)

            LabelDoctor.Text = Convert.ToString(ResourceTable("LabelDoctor"), Nothing).Trim()
            LabelDoctor.Text = If(String.IsNullOrEmpty(LabelDoctor.Text), "Doctor", LabelDoctor.Text)

            LabelEmergencyDisasterCategory.Text = Convert.ToString(ResourceTable("LabelEmergencyDisasterCategory"), Nothing).Trim()
            LabelEmergencyDisasterCategory.Text = If(String.IsNullOrEmpty(LabelEmergencyDisasterCategory.Text), "Emerg Disaster Category", LabelEmergencyDisasterCategory.Text)

            LabelSupervisor.Text = Convert.ToString(ResourceTable("LabelSupervisor"), Nothing).Trim()
            LabelSupervisor.Text = If(String.IsNullOrEmpty(LabelSupervisor.Text), "Supervisor", LabelSupervisor.Text)

            LabelLiaison.Text = Convert.ToString(ResourceTable("LabelLiaison"), Nothing).Trim()
            LabelLiaison.Text = If(String.IsNullOrEmpty(LabelLiaison.Text), "Liaison", LabelLiaison.Text)

            '#End Region

            '#Region "Client Emergency Contact Information"

            LabelEmergencyContact.Text = Convert.ToString(ResourceTable("LabelEmergencyContact"), Nothing).Trim()
            LabelEmergencyContact.Text = If(String.IsNullOrEmpty(LabelEmergencyContact.Text), "Emergency Contact", LabelEmergencyContact.Text)

            LabelEmergencyContactOneName.Text = Convert.ToString(ResourceTable("LabelEmergencyContactOneName"), Nothing).Trim()
            LabelEmergencyContactOneName.Text = If(String.IsNullOrEmpty(LabelEmergencyContactOneName.Text), "Contact Name1", LabelEmergencyContactOneName.Text)

            LabelEmergencyContactOnePhone.Text = Convert.ToString(ResourceTable("LabelEmergencyContactOnePhone"), Nothing).Trim()
            LabelEmergencyContactOnePhone.Text = If(String.IsNullOrEmpty(LabelEmergencyContactOnePhone.Text), "Contact1 Phone", LabelEmergencyContactOnePhone.Text)

            LabelEmergencyContactOneRelationship.Text = Convert.ToString(ResourceTable("LabelEmergencyContactOneRelationship"), Nothing).Trim()
            LabelEmergencyContactOneRelationship.Text = If(String.IsNullOrEmpty(LabelEmergencyContactOneRelationship.Text), "Relationship",
                                                           LabelEmergencyContactOneRelationship.Text)

            LabelEmergencyContactTwoName.Text = Convert.ToString(ResourceTable("LabelEmergencyContactTwoName"), Nothing).Trim()
            LabelEmergencyContactTwoName.Text = If(String.IsNullOrEmpty(LabelEmergencyContactTwoName.Text), "Contact Name2", LabelEmergencyContactTwoName.Text)

            LabelEmergencyContactTwoPhone.Text = Convert.ToString(ResourceTable("LabelEmergencyContactTwoPhone"), Nothing).Trim()
            LabelEmergencyContactTwoPhone.Text = If(String.IsNullOrEmpty(LabelEmergencyContactTwoPhone.Text), "Contact2 Phone", LabelEmergencyContactTwoPhone.Text)

            LabelEmergencyContactTwoRelationship.Text = Convert.ToString(ResourceTable("LabelEmergencyContactTwoRelationship"), Nothing).Trim()
            LabelEmergencyContactTwoRelationship.Text = If(String.IsNullOrEmpty(LabelEmergencyContactTwoRelationship.Text), "Relationship",
                                                           LabelEmergencyContactTwoRelationship.Text)

            '#End Region

            LabelUpdateDate.Text = Convert.ToString(ResourceTable("LabelUpdateDate"), Nothing).Trim()
            LabelUpdateDate.Text = If(String.IsNullOrEmpty(LabelUpdateDate.Text), "Update Date:", LabelUpdateDate.Text)

            LabelUpdateBy.Text = Convert.ToString(ResourceTable("LabelUpdateBy"), Nothing).Trim()
            LabelUpdateBy.Text = If(String.IsNullOrEmpty(LabelUpdateBy.Text), "Update By:", LabelUpdateBy.Text)

            '**********************************************Client Info[End]************************************************************

            '**********************************************Client Case[Start]*****************************************************

            '#Region "Client Case Information"

            LabelClientType.Text = Convert.ToString(ResourceTable("LabelClientType"), Nothing).Trim()
            LabelClientType.Text = If(String.IsNullOrEmpty(LabelClientType.Text), "Type:", LabelClientType.Text)

            LabelCaseWorker.Text = Convert.ToString(ResourceTable("LabelCaseWorker"), Nothing).Trim()
            LabelCaseWorker.Text = If(String.IsNullOrEmpty(LabelCaseWorker.Text), "Case Worker", LabelCaseWorker.Text)

            LabelUnits.Text = Convert.ToString(ResourceTable("LabelUnits"), Nothing).Trim()
            LabelUnits.Text = If(String.IsNullOrEmpty(LabelUnits.Text), "Units:", LabelUnits.Text)

            LabelNumberOfWeekDays.Text = Convert.ToString(ResourceTable("LabelNumberOfWeekDays"), Nothing).Trim()
            LabelNumberOfWeekDays.Text = If(String.IsNullOrEmpty(LabelNumberOfWeekDays.Text), "# of Days:", LabelNumberOfWeekDays.Text)

            LabelInsuranceNumber.Text = Convert.ToString(ResourceTable("LabelInsuranceNumber"), Nothing).Trim()
            LabelInsuranceNumber.Text = If(String.IsNullOrEmpty(LabelInsuranceNumber.Text), "Insurance Number", LabelInsuranceNumber.Text)

            '#End Region

            '#Region "Client Case Care Information"

            LabelSupervisorLastVisitDate.Text = Convert.ToString(ResourceTable("LabelSupervisorLastVisitDate"), Nothing).Trim()
            LabelSupervisorLastVisitDate.Text = If(String.IsNullOrEmpty(LabelSupervisorLastVisitDate.Text), "Last Sup Visit Date:", LabelSupervisorLastVisitDate.Text)

            LabelSupervisorVisitFrequency.Text = Convert.ToString(ResourceTable("LabelSupervisorVisitFrequency"), Nothing).Trim()
            LabelSupervisorVisitFrequency.Text = If(String.IsNullOrEmpty(LabelSupervisorVisitFrequency.Text), "Sup Visit Freq:", LabelSupervisorVisitFrequency.Text)

            LabelSuppliesOrEquipment.Text = Convert.ToString(ResourceTable("LabelSuppliesOrEquipment"), Nothing).Trim()
            LabelSuppliesOrEquipment.Text = If(String.IsNullOrEmpty(LabelSuppliesOrEquipment.Text), "Supplies/Equipment:", LabelSuppliesOrEquipment.Text)

            LabelAuthorizationDetail.Text = Convert.ToString(ResourceTable("LabelAuthorizationDetail"), Nothing).Trim()
            LabelAuthorizationDetail.Text = If(String.IsNullOrEmpty(LabelAuthorizationDetail.Text), "Service Start Milestones", LabelAuthorizationDetail.Text)

            LabelAuthorizatioReceivedDate.Text = Convert.ToString(ResourceTable("LabelAuthorizatioReceivedDate"), Nothing).Trim()
            LabelAuthorizatioReceivedDate.Text = If(String.IsNullOrEmpty(LabelAuthorizatioReceivedDate.Text), "Auth Received:", LabelAuthorizatioReceivedDate.Text)

            LabelPractitionerStatementReceivedDate.Text = Convert.ToString(ResourceTable("LabelPractitionerStatementReceivedDate"), Nothing).Trim()
            LabelPractitionerStatementReceivedDate.Text = If(String.IsNullOrEmpty(LabelPractitionerStatementReceivedDate.Text), "Prac St. Rec:",
                                                             LabelPractitionerStatementReceivedDate.Text)

            LabelPractitionerStatementSentDate.Text = Convert.ToString(ResourceTable("LabelPractitionerStatementSentDate"), Nothing).Trim()
            LabelPractitionerStatementSentDate.Text = If(String.IsNullOrEmpty(LabelPractitionerStatementSentDate.Text), "Prac St. Sent:", LabelPractitionerStatementSentDate.Text)

            LabelServiceInitializedReportedDate.Text = Convert.ToString(ResourceTable("LabelServiceInitializedReportedDate"), Nothing).Trim()
            LabelServiceInitializedReportedDate.Text = If(String.IsNullOrEmpty(LabelServiceInitializedReportedDate.Text), "Serv Init Re:",
                                                          LabelServiceInitializedReportedDate.Text)

            '#End Region


            '#Region "Client Case Billing Information"

            LabelAssessmentDate.Text = Convert.ToString(ResourceTable("LabelAssessmentDate"), Nothing).Trim()
            LabelAssessmentDate.Text = If(String.IsNullOrEmpty(LabelAssessmentDate.Text), "Assessment Date:", LabelAssessmentDate.Text)

            LabelBillingDiagonosisCode.Text = Convert.ToString(ResourceTable("LabelBillingDiagonosisCode"), Nothing).Trim()
            LabelBillingDiagonosisCode.Text = If(String.IsNullOrEmpty(LabelBillingDiagonosisCode.Text), "Billing Diagonosis Code", LabelBillingDiagonosisCode.Text)


            LabelBillingInfo.Text = Convert.ToString(ResourceTable("LabelBillingInfo"), Nothing).Trim()
            LabelBillingInfo.Text = If(String.IsNullOrEmpty(LabelBillingInfo.Text), "Billing Info", LabelBillingInfo.Text)

            LabelAuthorizationNumber.Text = Convert.ToString(ResourceTable("LabelAuthorizationNumber"), Nothing).Trim()
            LabelAuthorizationNumber.Text = If(String.IsNullOrEmpty(LabelAuthorizationNumber.Text), "Ref No.", LabelAuthorizationNumber.Text)

            LabelProcedureCode.Text = Convert.ToString(ResourceTable("LabelProcedureCode"), Nothing).Trim()
            LabelProcedureCode.Text = If(String.IsNullOrEmpty(LabelProcedureCode.Text), "Procedure Code:", LabelProcedureCode.Text)

            LabelModifiers.Text = Convert.ToString(ResourceTable("LabelModifiers"), Nothing).Trim()
            LabelModifiers.Text = If(String.IsNullOrEmpty(LabelModifiers.Text), "Modifiers:", LabelModifiers.Text)

            LabelPlaceOfService.Text = Convert.ToString(ResourceTable("LabelPlaceOfService"), Nothing).Trim()
            LabelPlaceOfService.Text = If(String.IsNullOrEmpty(LabelPlaceOfService.Text), "Place of Service:", LabelPlaceOfService.Text)

            LabelUnitRate.Text = Convert.ToString(ResourceTable("LabelUnitRate"), Nothing).Trim()
            LabelUnitRate.Text = If(String.IsNullOrEmpty(LabelUnitRate.Text), "Unit Rate:", LabelUnitRate.Text)

            LabelClaimFrequencyTypeCode.Text = Convert.ToString(ResourceTable("LabelClaimFrequencyTypeCode"), Nothing).Trim()
            LabelClaimFrequencyTypeCode.Text = If(String.IsNullOrEmpty(LabelClaimFrequencyTypeCode.Text), "Claim Frequency Type Code:", LabelClaimFrequencyTypeCode.Text)

            LabelProviderSignatureOnFile.Text = Convert.ToString(ResourceTable("LabelProviderSignatureOnFile"), Nothing).Trim()
            LabelProviderSignatureOnFile.Text = If(String.IsNullOrEmpty(LabelProviderSignatureOnFile.Text), "Provider Signature On File:", LabelProviderSignatureOnFile.Text)

            LabelMedicareAssignmentCode.Text = Convert.ToString(ResourceTable("LabelMedicareAssignmentCode"), Nothing).Trim()
            LabelMedicareAssignmentCode.Text = If(String.IsNullOrEmpty(LabelMedicareAssignmentCode.Text), "Medicare Assignment Code:", LabelMedicareAssignmentCode.Text)

            LabelAssignmentOfBenefitsIndicator.Text = Convert.ToString(ResourceTable("LabelAssignmentOfBenefitsIndicator"), Nothing).Trim()
            LabelAssignmentOfBenefitsIndicator.Text = If(String.IsNullOrEmpty(LabelAssignmentOfBenefitsIndicator.Text), "Assignment Of Benefits Indicator:",
                                                         LabelAssignmentOfBenefitsIndicator.Text)

            LabelReleaseOfInformationCode.Text = Convert.ToString(ResourceTable("LabelReleaseOfInformationCode"), Nothing).Trim()
            LabelReleaseOfInformationCode.Text = If(String.IsNullOrEmpty(LabelReleaseOfInformationCode.Text), "Release Of Information Code:", LabelReleaseOfInformationCode.Text)

            LabelPatientSignatureCode.Text = Convert.ToString(ResourceTable("LabelPatientSignatureCode"), Nothing).Trim()
            LabelPatientSignatureCode.Text = If(String.IsNullOrEmpty(LabelPatientSignatureCode.Text), "Patient Signature Code:", LabelPatientSignatureCode.Text)

            '#End Region

            '#Region "Client Case Santrax Information"

            LabelSantrax.Text = Convert.ToString(ResourceTable("LabelSantrax"), Nothing).Trim()
            LabelSantrax.Text = If(String.IsNullOrEmpty(LabelSantrax.Text), "Santrax", LabelSantrax.Text)

            LabelSantraxClientId.Text = Convert.ToString(ResourceTable("LabelSantraxClientId"), Nothing).Trim()
            LabelSantraxClientId.Text = If(String.IsNullOrEmpty(LabelSantraxClientId.Text), "Santrax Client Id:", LabelSantraxClientId.Text)

            LabelSantraxInformation.Text = Convert.ToString(ResourceTable("LabelSantraxInformation"), Nothing).Trim()
            LabelSantraxInformation.Text = If(String.IsNullOrEmpty(LabelSantraxInformation.Text), "Santrax Information", LabelSantraxInformation.Text)

            LabelProgramOrService.Text = Convert.ToString(ResourceTable("LabelProgramOrService"), Nothing).Trim()
            LabelProgramOrService.Text = If(String.IsNullOrEmpty(LabelProgramOrService.Text), "Select Program/Service", LabelProgramOrService.Text)

            LabelServiceGroup.Text = Convert.ToString(ResourceTable("LabelServiceGroup"), Nothing).Trim()
            LabelServiceGroup.Text = If(String.IsNullOrEmpty(LabelServiceGroup.Text), "Service Group", LabelServiceGroup.Text)

            LabelServiceCode.Text = Convert.ToString(ResourceTable("LabelServiceCode"), Nothing).Trim()
            LabelServiceCode.Text = If(String.IsNullOrEmpty(LabelServiceCode.Text), "Service Code", LabelServiceCode.Text)

            LabelServiceCodeDescription.Text = Convert.ToString(ResourceTable("LabelServiceCodeDescription"), Nothing).Trim()
            LabelServiceCodeDescription.Text = If(String.IsNullOrEmpty(LabelServiceCodeDescription.Text), "Service Code Description", LabelServiceCodeDescription.Text)

            LabelSantraxARNumber.Text = Convert.ToString(ResourceTable("LabelSantraxARNumber"), Nothing).Trim()
            LabelSantraxARNumber.Text = If(String.IsNullOrEmpty(LabelSantraxARNumber.Text), "ARNumber:", LabelSantraxARNumber.Text)

            LabelSantraxPriority.Text = Convert.ToString(ResourceTable("LabelSantraxPriority"), Nothing).Trim()
            LabelSantraxPriority.Text = If(String.IsNullOrEmpty(LabelSantraxPriority.Text), "Priority:", LabelSantraxPriority.Text)

            LabelSantraxBillCode.Text = Convert.ToString(ResourceTable("LabelSantraxBillCode"), Nothing).Trim()
            LabelSantraxBillCode.Text = If(String.IsNullOrEmpty(LabelSantraxBillCode.Text), "Bill Code:", LabelSantraxBillCode.Text)

            LabelSantraxProcCodeQualifier.Text = Convert.ToString(ResourceTable("LabelSantraxProcCodeQualifier"), Nothing).Trim()
            LabelSantraxProcCodeQualifier.Text = If(String.IsNullOrEmpty(LabelSantraxProcCodeQualifier.Text), "Proc Code Qualifier:", LabelSantraxProcCodeQualifier.Text)

            LabelSantraxLandPhone.Text = Convert.ToString(ResourceTable("LabelSantraxLandPhone"), Nothing).Trim()
            LabelSantraxLandPhone.Text = If(String.IsNullOrEmpty(LabelSantraxLandPhone.Text), "Land Phone:", LabelSantraxLandPhone.Text)

            '#End Region

            LabelAge.Text = Convert.ToString(ResourceTable("LabelAge"), Nothing).Trim()
            LabelAge.Text = If(String.IsNullOrEmpty(LabelAge.Text), "Age:", LabelAge.Text)

            LabelAuthorization.Text = Convert.ToString(ResourceTable("LabelAuthorization"), Nothing).Trim()
            LabelAuthorization.Text = If(String.IsNullOrEmpty(LabelAuthorization.Text), "Current Authorization Period", LabelAuthorization.Text)

            LabelAuthorizationFromDate.Text = Convert.ToString(ResourceTable("LabelAuthorizationFromDate"), Nothing).Trim()
            LabelAuthorizationFromDate.Text = If(String.IsNullOrEmpty(LabelAuthorizationFromDate.Text), "From:", LabelAuthorizationFromDate.Text)

            LabelAuthorizationToDate.Text = Convert.ToString(ResourceTable("LabelAuthorizationToDate"), Nothing).Trim()
            LabelAuthorizationToDate.Text = If(String.IsNullOrEmpty(LabelAuthorizationToDate.Text), "To:", LabelAuthorizationToDate.Text)

            '**********************************************Client Case[End]*****************************************************

            ButtonSave.Text = Convert.ToString(ResourceTable("ButtonSave"), Nothing).Trim()
            ButtonSave.Text = If(String.IsNullOrEmpty(ButtonSave.Text), "Save", ButtonSave.Text)

            ButtonClear.Text = Convert.ToString(ResourceTable("ButtonClear"), Nothing).Trim()
            ButtonClear.Text = If(String.IsNullOrEmpty(ButtonClear.Text), "Clear", ButtonClear.Text)

            ButtonDelete.Text = Convert.ToString(ResourceTable("ButtonDelete"), Nothing).Trim()
            ButtonDelete.Text = If(String.IsNullOrEmpty(ButtonDelete.Text), "Delete", ButtonDelete.Text)

            EditText = Convert.ToString(ResourceTable("EditText"), Nothing).Trim()
            EditText = If(String.IsNullOrEmpty(EditText), "Edit", EditText)

            DeleteText = Convert.ToString(ResourceTable("DeleteText"), Nothing).Trim()
            DeleteText = If(String.IsNullOrEmpty(DeleteText), "Delete", DeleteText)

            InsertMessage = Convert.ToString(ResourceTable("InsertMessage"), Nothing).Trim()
            InsertMessage = If(String.IsNullOrEmpty(InsertMessage), "Inserted Successfully", InsertMessage)

            UpdateMessage = Convert.ToString(ResourceTable("UpdateMessage"), Nothing).Trim()
            UpdateMessage = If(String.IsNullOrEmpty(UpdateMessage), "Updated Successfully", UpdateMessage)

            DeleteMessage = Convert.ToString(ResourceTable("DeleteMessage"), Nothing).Trim()
            DeleteMessage = If(String.IsNullOrEmpty(DeleteMessage), "Deleted Successfully", DeleteMessage)

            DuplicateClientMessage = Convert.ToString(ResourceTable("DuplicateClientMessage"), Nothing).Trim()
            DuplicateClientMessage = If(String.IsNullOrEmpty(DuplicateClientMessage), "This client is alredy exist along with first name, last name and medicaid id.",
                                        DuplicateClientMessage)

            DuplicateClientCaseMessage = Convert.ToString(ResourceTable("DuplicateClientCaseMessage"), Nothing).Trim()
            DuplicateClientCaseMessage = If(String.IsNullOrEmpty(DuplicateClientCaseMessage), "The same client case is already exist.", DuplicateClientCaseMessage)

            DuplicateNameMessage = Convert.ToString(ResourceTable("DuplicateNameMessage"), Nothing).Trim()
            DuplicateNameMessage = If(String.IsNullOrEmpty(DuplicateNameMessage), "The same name is already exist.", DuplicateNameMessage)

            EmptyNameMessage = Convert.ToString(ResourceTable("EmptyNameMessage"), Nothing).Trim()
            EmptyNameMessage = If(String.IsNullOrEmpty(EmptyNameMessage), "Name Cannot be Empty.", EmptyNameMessage)

            StatusSelectionMessage = Convert.ToString(ResourceTable("StatusSelectionMessage"), Nothing).Trim()
            StatusSelectionMessage = If(String.IsNullOrEmpty(StatusSelectionMessage), "Please select status", StatusSelectionMessage)

            ReceiverSelectionMessage = Convert.ToString(ResourceTable("ReceiverSelectionMessage"), Nothing).Trim()
            ReceiverSelectionMessage = If(String.IsNullOrEmpty(ReceiverSelectionMessage), "Please select receiver", ReceiverSelectionMessage)

            PayerSelectionMessage = Convert.ToString(ResourceTable("PayerSelectionMessage"), Nothing).Trim()
            PayerSelectionMessage = If(String.IsNullOrEmpty(PayerSelectionMessage), "Please select payer", PayerSelectionMessage)

            InvalidContractNumberMessage = Convert.ToString(ResourceTable("InvalidContractNumberMessage"), Nothing).Trim()
            InvalidContractNumberMessage = If(String.IsNullOrEmpty(InvalidContractNumberMessage), "Invalid Contract Number", InvalidContractNumberMessage)

            InvalidUnitRateMessage = Convert.ToString(ResourceTable("InvalidUnitRateMessage"), Nothing).Trim()
            InvalidUnitRateMessage = If(String.IsNullOrEmpty(InvalidUnitRateMessage), "Invalid Unit Rate", InvalidUnitRateMessage)

            SaveConfirmMessage = Convert.ToString(ResourceTable("SaveConfirmMessage"), Nothing).Trim()
            SaveConfirmMessage = If(String.IsNullOrEmpty(SaveConfirmMessage), "Saved Successfully", SaveConfirmMessage)

            ValidationGroup = Convert.ToString(ResourceTable("ValidationGroup"), Nothing).Trim()
            ValidationGroup = If(String.IsNullOrEmpty(ValidationGroup), "Client", ValidationGroup)

            Dim ResultOut As Boolean

            ValidationEnable = If((Boolean.TryParse(ResourceTable("ValidationEnable"), ResultOut)), ResultOut, True)

            ResourceTable = Nothing

        End Sub

        ''' <summary>
        ''' This is for validating data in server side before submitting the form
        ''' </summary>
        ''' <returns></returns>
        Private Function ValidateData() As Boolean

            '**********************************************Client Info[Start]*********************************

            '#Region "Client Basic Information"

            If (String.IsNullOrEmpty(TextBoxStateClientId.Text.Trim())) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("State client id cannot be empty.")
                Return False
            End If

            If (Not objShared.IsInteger(TextBoxStateClientId.Text.Trim())) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Invalid State Client Id")
                Return False
            End If

            If (Not String.IsNullOrEmpty(TextBoxSocialSecurityNumber.Text.Trim())) Then
                If (Not objShared.ValidateSocialSecurityNumber(TextBoxSocialSecurityNumber.Text.Trim())) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorSocialSecurityNumber.ErrorMessage)
                    Return False
                End If
            End If

            If (String.IsNullOrEmpty(TextBoxFirstName.Text.Trim())) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("First name cannot be empty.")
                Return False
            End If

            If (String.IsNullOrEmpty(TextBoxLastName.Text.Trim())) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Last name cannot be empty.")
                Return False
            End If

            If (Not String.IsNullOrEmpty(TextBoxPhone.Text.Trim())) Then
                If (Not objShared.ValidatePhone(TextBoxPhone.Text.Trim())) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorPhone.ErrorMessage)
                    Return False
                End If
            End If

            If (Not String.IsNullOrEmpty(TextBoxAlternatePhone.Text.Trim())) Then
                If (Not objShared.ValidatePhone(TextBoxAlternatePhone.Text.Trim())) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorAlternatePhone.ErrorMessage)
                    Return False
                End If
            End If

            If DropDownListPriority.SelectedValue = "-1" Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Select Priority")
                Return False
            End If

            If (String.IsNullOrEmpty(TextBoxAddress.Text.Trim())) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Address cannot be empty.")
                Return False
            End If

            If (Not String.IsNullOrEmpty(TextBoxAge.Text.Trim())) Then
                If (Not objShared.IsInteger(TextBoxAge.Text.Trim())) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Invalid Age")
                    Return False
                End If
            End If

            If DropDownListCity.SelectedValue = "-1" Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Select City")
                Return False
            End If

            If DropDownListState.SelectedValue = "-1" Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Select State")
                Return False
            End If

            If (String.IsNullOrEmpty(TextBoxZip.Text.Trim())) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Zip cannot be empty.")
                Return False
            End If

            If (Not objShared.ValidatePhone(TextBoxZip.Text.Trim())) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Invalid Zip Code")
                Return False
            End If

            If DropDownListGender.SelectedValue = "-1" Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Select Gender")
                Return False
            End If

            '#End Region

            '#Region "Client Treatment Information"

            If ((Not String.IsNullOrEmpty(TextBoxServiceStartDate.Text.Trim())) And (Not objShared.ValidateDate(TextBoxServiceStartDate.Text.Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorStartDate.ErrorMessage)
                Return False
            End If

            If (Not String.IsNullOrEmpty(TextBoxServiceStartDate.Text.Trim())) Then
                If (DateTime.Compare(DateTime.Now, Convert.ToDateTime(TextBoxServiceStartDate.Text)) < 0) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Service start date should not cross the current date.")
                    Return False
                End If
            End If

            If ((Not String.IsNullOrEmpty(TextBoxServiceEndDate.Text.Trim())) And (Not objShared.ValidateDate(TextBoxServiceEndDate.Text.Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorEndDate.ErrorMessage)
                Return False
            End If

            If (Not String.IsNullOrEmpty(TextBoxServiceStartDate.Text.Trim()) And Not String.IsNullOrEmpty(TextBoxServiceEndDate.Text.Trim())) Then
                If (Convert.ToDateTime(TextBoxServiceStartDate.Text) > Convert.ToDateTime(TextBoxServiceEndDate.Text)) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Service start date should not be greater than service end date.")
                    Return False
                End If
            End If

            '#End Region

            '#Region "Client Emergency Contact Information"

            If (Not String.IsNullOrEmpty(TextBoxEmergencyContactOnePhone.Text.Trim())) Then
                If (Not objShared.ValidatePhone(TextBoxEmergencyContactOnePhone.Text.Trim())) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorEmergencyContactOnePhone.ErrorMessage)
                    Return False
                End If
            End If

            If (Not String.IsNullOrEmpty(TextBoxEmergencyContactTwoPhone.Text.Trim())) Then
                If (Not objShared.ValidatePhone(TextBoxEmergencyContactTwoPhone.Text.Trim())) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorEmergencyContactTwoPhone.ErrorMessage)
                    Return False
                End If
            End If

            '#End Region

            '**********************************************Client Info[End]*********************************

            '**********************************************Client Case[Start]*********************************

            '#Region "Client Case Information"

            If (Not String.IsNullOrEmpty(TextBoxUnits.Text.Trim())) Then
                If (Not objShared.ValidateTime(TextBoxUnits.Text.Trim())) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorUnits.ErrorMessage)
                    Return False
                End If
            End If

            If (Not String.IsNullOrEmpty(TextBoxNumberOfWeekDays.Text.Trim())) Then
                If (Not objShared.IsInteger(TextBoxNumberOfWeekDays.Text.Trim())) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Invalid Weekdays")
                    Return False
                End If
            End If

            '#End Region

            '#Region "Client Case Care Information"

            If (Not String.IsNullOrEmpty(TextBoxSupervisorVisitFrequency.Text.Trim())) Then
                If (Not objShared.IsInteger(TextBoxSupervisorVisitFrequency.Text.Trim())) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Invalid Supervisor Visit Frequency")
                    Return False
                End If
            End If

            If ((Not String.IsNullOrEmpty(TextBoxAuthorizationFromDate.Text.Trim())) And (Not objShared.ValidateDate(TextBoxAuthorizationFromDate.Text.Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorAuthorizationFromDate.ErrorMessage)
                Return False
            End If

            If (Not String.IsNullOrEmpty(TextBoxAuthorizationFromDate.Text.Trim())) Then
                If (DateTime.Compare(DateTime.Now, Convert.ToDateTime(TextBoxAuthorizationFromDate.Text.Trim())) < 0) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Authorization From date should not cross the current date.")
                    Return False
                End If
            End If

            If ((Not String.IsNullOrEmpty(TextBoxAuthorizatioReceivedDate.Text.Trim())) And (Not objShared.ValidateDate(TextBoxAuthorizatioReceivedDate.Text.Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorAuthorizatioReceivedDate.ErrorMessage)
                Return False
            End If

            If (Not String.IsNullOrEmpty(TextBoxAuthorizatioReceivedDate.Text.Trim())) Then
                If (DateTime.Compare(DateTime.Now, Convert.ToDateTime(TextBoxAuthorizatioReceivedDate.Text.Trim())) < 0) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Authorization received date should not cross the current date.")
                    Return False
                End If
            End If

            If ((Not String.IsNullOrEmpty(TextBoxPractitionerStatementSentDate.Text.Trim())) _
                    And (Not objShared.ValidateDate(TextBoxPractitionerStatementSentDate.Text.Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorPractitionerStatementSentDate.ErrorMessage)
                Return False
            End If

            If (Not String.IsNullOrEmpty(TextBoxPractitionerStatementSentDate.Text.Trim())) Then
                If (DateTime.Compare(DateTime.Now, Convert.ToDateTime(TextBoxPractitionerStatementSentDate.Text.Trim())) < 0) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Practitioner statement sent date should not cross the current date.")
                    Return False
                End If
            End If

            If ((Not String.IsNullOrEmpty(TextBoxPractitionerStatementReceivedDate.Text.Trim())) _
                    And (Not objShared.ValidateDate(TextBoxPractitionerStatementReceivedDate.Text.Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorPractitionerStatementReceivedDate.ErrorMessage)
                Return False
            End If

            If (Not String.IsNullOrEmpty(TextBoxPractitionerStatementSentDate.Text.Trim()) _
                    And Not String.IsNullOrEmpty(TextBoxPractitionerStatementReceivedDate.Text.Trim())) Then
                If (Convert.ToDateTime(TextBoxPractitionerStatementSentDate.Text.Trim()) > Convert.ToDateTime(TextBoxPractitionerStatementReceivedDate.Text.Trim())) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Practitioner statement sent date should not be greater than received date.")
                    Return False
                End If
            End If

            '#End Region

            '#Region "Client Case Billing Information"

            If ((Not String.IsNullOrEmpty(TextBoxAssessmentDate.Text.Trim())) And (Not objShared.ValidateDate(TextBoxAssessmentDate.Text.Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorAssessmentDate.ErrorMessage)
                Return False
            End If

            If (Not String.IsNullOrEmpty(TextBoxAssessmentDate.Text.Trim())) Then
                If (DateTime.Compare(DateTime.Now, Convert.ToDateTime(TextBoxAssessmentDate.Text.Trim())) < 0) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Assessment date should not cross the current date.")
                    Return False
                End If
            End If

            '#End Region

            '#Region "Client Case Santrax Information"

            If ((Not String.IsNullOrEmpty(TextBoxUnitRate.Text.Trim()))) And (Not objShared.ValidatePayrate(TextBoxUnitRate.Text.Trim())) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorUnitRate.ErrorMessage)
                Return False
            End If

            If (Not String.IsNullOrEmpty(TextBoxSantraxLandPhone.Text.Trim())) Then
                If (Not objShared.ValidatePhone(TextBoxSantraxLandPhone.Text.Trim())) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorSantraxLandPhone.ErrorMessage)
                    Return False
                End If
            End If

            '#End Region

            '**********************************************Client Case[End]*********************************

            Return True
        End Function

        Private Sub SaveData(IsConfirm As Boolean)

            Page.Validate()
            Page.Validate(ValidationGroup)

            If ((Page.IsValid) And (ValidateData())) Then

                '**********************************************Client Info[Start]*********************************

                Dim objClientInfoDataObject As New ClientInfoDataObject()
                Dim objClientBasicInfoDataObject As New ClientBasicInfoDataObject()
                Dim objClientTreatmentInfoDataObject As New ClientTreatmentInfoDataObject()
                Dim objClientEmergencyContactInfoDataObject As New ClientEmergencyContactInfoDataObject()

                '#Region "Client Basic Information"

                Dim LastSelectedClientId As Integer = IndividualId

                objClientInfoDataObject.StateClientId = Convert.ToInt32(TextBoxStateClientId.Text)
                objClientBasicInfoDataObject.FirstName = Convert.ToString(TextBoxFirstName.Text, Nothing).Trim()
                objClientBasicInfoDataObject.LastName = Convert.ToString(TextBoxLastName.Text, Nothing).Trim()
                objClientBasicInfoDataObject.MiddleNameInitial = Convert.ToString(TextBoxMiddleInitial.Text, Nothing).Trim()
                objClientBasicInfoDataObject.Phone = objShared.GetReFormattedMobileNumber(Convert.ToString(TextBoxPhone.Text, Nothing).Trim())
                objClientBasicInfoDataObject.AlternatePhone = objShared.GetReFormattedMobileNumber(Convert.ToString(TextBoxAlternatePhone.Text, Nothing).Trim())
                objClientBasicInfoDataObject.Priority = Convert.ToInt16(DropDownListPriority.SelectedValue)
                objClientBasicInfoDataObject.DateOfBirth = Convert.ToString(TextBoxDateOfBirth.Text, Nothing).Trim()
                objClientBasicInfoDataObject.SocialSecurityNumber = Convert.ToString(TextBoxSocialSecurityNumber.Text, Nothing).Trim()
                objClientBasicInfoDataObject.Address = Convert.ToString(TextBoxAddress.Text, Nothing).Trim()
                objClientBasicInfoDataObject.ApartmentNumber = Convert.ToString(TextBoxApartmentNumber.Text, Nothing).Trim()
                objClientBasicInfoDataObject.CityId = Convert.ToInt32(DropDownListCity.SelectedValue)
                objClientBasicInfoDataObject.StateId = Convert.ToInt32(DropDownListState.SelectedValue)
                objClientBasicInfoDataObject.Zip = Convert.ToString(TextBoxZip.Text, Nothing).Trim()
                objClientBasicInfoDataObject.Status = Convert.ToInt16(DropDownListIndividualStatus.SelectedValue)
                objClientBasicInfoDataObject.Gender = Convert.ToString(DropDownListGender.SelectedValue, Nothing).Trim()
                objClientBasicInfoDataObject.MaritalStatusId = Convert.ToInt32(DropDownListMaritalStatus.SelectedValue)

                '#End Region

                '#Region "Client Treatment Information"
                objClientTreatmentInfoDataObject.ServiceStartDate = Convert.ToString(TextBoxServiceStartDate.Text, Nothing).Trim()
                objClientTreatmentInfoDataObject.ServiceEndDate = Convert.ToString(TextBoxServiceEndDate.Text, Nothing).Trim()
                objClientTreatmentInfoDataObject.DischargeReason = Convert.ToInt32(DropDownListDischargeReason.SelectedValue)
                objClientTreatmentInfoDataObject.Diagnosis = Convert.ToString(TextBoxDiagnosis.Text, Nothing).Trim()
                objClientTreatmentInfoDataObject.CountyId = Convert.ToInt32(DropDownListCounty.SelectedValue)
                objClientTreatmentInfoDataObject.DoctorId = Convert.ToInt32(DropDownListDoctor.SelectedValue)
                objClientTreatmentInfoDataObject.DisasterCategory = Convert.ToString(DropDownListEmergencyDisasterCategory.SelectedValue, Nothing).Trim()
                objClientTreatmentInfoDataObject.Supervisor = Convert.ToString(TextBoxSupervisor.Text, Nothing).Trim()
                objClientTreatmentInfoDataObject.Liaison = Convert.ToString(TextBoxLiaison.Text, Nothing).Trim()
                '#End Region

                '#Region "Client Emergency Contact Information"
                objClientEmergencyContactInfoDataObject.EmergencyContact = Convert.ToString(TextBoxEmergencyContactOneName.Text, Nothing).Trim()
                objClientEmergencyContactInfoDataObject.EmergencyContactPhone = Convert.ToString(TextBoxEmergencyContactOnePhone.Text, Nothing).Trim()
                objClientEmergencyContactInfoDataObject.EmergencyContactRelationship = Convert.ToString(TextBoxEmergencyContactOneRelationship.Text, Nothing).Trim()
                objClientEmergencyContactInfoDataObject.EmergencyContactTwo = Convert.ToString(TextBoxEmergencyContactTwoName.Text, Nothing).Trim()
                objClientEmergencyContactInfoDataObject.EmergencyContactPhoneTwo = Convert.ToString(TextBoxEmergencyContactTwoPhone.Text, Nothing).Trim()
                objClientEmergencyContactInfoDataObject.EmergencyContactRelationshipTwo = Convert.ToString(TextBoxEmergencyContactTwoRelationship.Text, Nothing).Trim()

                '#End Region
                '**********************************************Client Info[End]*********************************

                '**********************************************Client Case[Start]*********************************

                Dim objClientCaseDataObject As New ClientCaseDataObject()
                Dim objClientCaseCareInfoDataObject As New ClientCaseCareInfoDataObject()
                Dim objClientCaseBillingInfoDataObject As New ClientCaseBillingInfoDataObject()
                Dim objClientCaseSantraxInfoDataObject As New ClientCaseSantraxInfoDataObject()

                If Not DropDownListClientType.SelectedValue = "-1" Then

                    '#Region "Client Case Information"

                    objClientCaseDataObject.Type = Convert.ToInt32(DropDownListClientType.SelectedValue)
                    objClientCaseDataObject.CaseWorkerId = Convert.ToInt32(DropDownListCaseWorker.SelectedValue)

                    objClientCaseDataObject.Units = If((String.IsNullOrEmpty(TextBoxUnits.Text.Trim())), String.Empty, Convert.ToString(TextBoxUnits.Text.Trim(), Nothing))
                    objClientCaseDataObject.WeekDays = If((String.IsNullOrEmpty(TextBoxNumberOfWeekDays.Text.Trim())),
                                                            objClientCaseDataObject.WeekDays,
                                                            Convert.ToInt32(TextBoxNumberOfWeekDays.Text.Trim()))

                    objClientCaseDataObject.InsuranceNumber = Convert.ToString(TextBoxInsuranceNumber.Text, Nothing).Trim()

                    '#End Region

                    '#Region "Client Case Care Information"

                    objClientCaseCareInfoDataObject.SupervisorLastVisitDate = Convert.ToString(TextBoxSupervisorLastVisitDate.Text, Nothing).Trim()

                    objClientCaseCareInfoDataObject.SupervisorVisitFrequency = If((String.IsNullOrEmpty(TextBoxSupervisorVisitFrequency.Text.Trim())),
                                                                                    objClientCaseCareInfoDataObject.SupervisorVisitFrequency,
                                                                                    Convert.ToInt32(TextBoxSupervisorVisitFrequency.Text.Trim()))

                    objClientCaseCareInfoDataObject.Supplies = Convert.ToString(TextBoxSuppliesOrEquipment.Text, Nothing).Trim()

                    objClientCaseCareInfoDataObject.PlanOfCareStartDate = Convert.ToString(TextBoxAuthorizationFromDate.Text, Nothing).Trim()
                    objClientCaseCareInfoDataObject.PlanOfCareEndDate = Convert.ToString(TextBoxAuthorizationToDate.Text, Nothing).Trim()

                    objClientCaseCareInfoDataObject.AuthorizationReceivedDate = Convert.ToString(TextBoxAuthorizatioReceivedDate.Text, Nothing).Trim()
                    objClientCaseCareInfoDataObject.DoctorOrderSentDate = Convert.ToString(TextBoxPractitionerStatementSentDate.Text, Nothing).Trim()
                    objClientCaseCareInfoDataObject.DoctorOrderReceivedDate = Convert.ToString(TextBoxPractitionerStatementReceivedDate.Text, Nothing).Trim()
                    objClientCaseCareInfoDataObject.ServiceInitializedReportedDate = Convert.ToString(TextBoxServiceInitializedReportedDate.Text, Nothing).Trim()

                    '#End Region


                    '#Region "Client Case Billing Information"

                    objClientCaseBillingInfoDataObject.AssessmentDate = Convert.ToString(TextBoxAssessmentDate.Text, Nothing).Trim()
                    objClientCaseBillingInfoDataObject.DiagnosisCodeOne = Convert.ToString(DropDownListBillingDiagonosisCodeOne.SelectedItem, Nothing).Trim()
                    objClientCaseBillingInfoDataObject.DiagnosisCodeTwo = Convert.ToString(DropDownListBillingDiagonosisCodeTwo.SelectedItem, Nothing).Trim()
                    objClientCaseBillingInfoDataObject.DiagnosisCodeThree = Convert.ToString(DropDownListBillingDiagonosisCodeThree.SelectedItem, Nothing).Trim()
                    objClientCaseBillingInfoDataObject.DiagnosisCodeFour = Convert.ToString(DropDownListBillingDiagonosisCodeFour.SelectedItem, Nothing).Trim()
                    objClientCaseBillingInfoDataObject.AuthorizationNumber = Convert.ToString(TextBoxAuthorizationNumber.Text, Nothing).Trim()
                    objClientCaseBillingInfoDataObject.ProcedureId = Convert.ToString(DropDownListProcedureCode.SelectedValue, Nothing).Trim()
                    objClientCaseBillingInfoDataObject.ModifierOne = Convert.ToString(TextBoxModifierOne.Text, Nothing).Trim()
                    objClientCaseBillingInfoDataObject.ModifierTwo = Convert.ToString(TextBoxModifierTwo.Text, Nothing).Trim()
                    objClientCaseBillingInfoDataObject.ModifierThree = Convert.ToString(TextBoxModifierThree.Text, Nothing).Trim()
                    objClientCaseBillingInfoDataObject.ModifierFour = Convert.ToString(TextBoxModifierFour.Text, Nothing).Trim()
                    objClientCaseBillingInfoDataObject.PlaceOfServiceId = Convert.ToString(DropDownListPlaceOfService.SelectedValue, Nothing).Trim()
                    objClientCaseBillingInfoDataObject.Comments = Convert.ToString(TextBoxComments.Text, Nothing).Trim()

                    If (String.IsNullOrEmpty(TextBoxUnitRate.Text.Trim())) Then
                        TextBoxUnitRate.Text = 0
                    End If

                    objClientCaseBillingInfoDataObject.UnitRate = Convert.ToDecimal(objShared.GetReFormattedPayrate(TextBoxUnitRate.Text.Trim()), Nothing)
                    objClientCaseBillingInfoDataObject.CLM0503ClmFrequencyTypeCode = Convert.ToString(TextBoxClaimFrequencyTypeCode.Text, Nothing).Trim()
                    objClientCaseBillingInfoDataObject.CLM06ProviderSignatureOnFile = Convert.ToString(DropDownListProviderSignatureOnFile.SelectedValue, Nothing).Trim()
                    objClientCaseBillingInfoDataObject.CLM07MedicareAssignmentCode = Convert.ToString(DropDownListMedicareAssignmentCode.SelectedValue, Nothing).Trim()
                    objClientCaseBillingInfoDataObject.CLM08AssignmentOfBenefitsIndicator = Convert.ToString(DropDownListAssignmentOfBenefitsIndicator.SelectedValue,
                                                                                            Nothing).Trim()

                    objClientCaseBillingInfoDataObject.CLM09ReleaseOfInfoCode = Convert.ToString(DropDownListReleaseOfInformationCode.SelectedValue, Nothing).Trim()
                    objClientCaseBillingInfoDataObject.CLM10PatientSignatureCode = Convert.ToString(DropDownListPatientSignatureCode.SelectedValue, Nothing).Trim()

                    '#End Region


                    '#Region "Client Case Santrax Information"

                    objClientCaseSantraxInfoDataObject.SantraxClientId = Convert.ToString(TextBoxSantraxClientId.Text, Nothing).Trim()
                    objClientCaseSantraxInfoDataObject.ServiceCode = Convert.ToString(DropDownListServiceCode.SelectedItem, Nothing).Trim()
                    objClientCaseSantraxInfoDataObject.ServiceCodeDescription = Convert.ToString(DropDownListServiceCodeDescription.SelectedItem, Nothing).Trim()
                    objClientCaseSantraxInfoDataObject.ServiceGroup = Convert.ToString(DropDownListServiceGroup.SelectedItem, Nothing).Trim()
                    objClientCaseSantraxInfoDataObject.SantraxARNumber = Convert.ToString(TextBoxSantraxARNumber.Text, Nothing).Trim()

                    objClientCaseSantraxInfoDataObject.SantraxPriority = If((DropDownListSantraxPriority.SelectedValue.Equals("-1")),
                                                                            objClientCaseSantraxInfoDataObject.SantraxPriority,
                                                                            Convert.ToInt32(DropDownListSantraxPriority.SelectedValue))

                    objClientCaseSantraxInfoDataObject.BillCode = Convert.ToString(TextBoxSantraxBillCode.Text, Nothing).Trim()
                    objClientCaseSantraxInfoDataObject.ProcedureCodeQualifier = Convert.ToString(TextBoxSantraxProcCodeQualifier.Text, Nothing).Trim()
                    objClientCaseSantraxInfoDataObject.LandPhone = objShared.GetReFormattedMobileNumber(Convert.ToString(TextBoxSantraxLandPhone.Text, Nothing).Trim())


                    '#End Region

                End If

                '**********************************************Client Case[End]*********************************

                Dim objBLClientInfo As New BLClientInfo()

                Dim IsSaved As Boolean = False

                Select Case Convert.ToBoolean(HiddenFieldIsNew.Value, Nothing)
                    Case True
                        objClientInfoDataObject.UserId = UserId
                        objClientInfoDataObject.CompanyId = CompanyId
                        Try
                            objBLClientInfo.InsertClientInfo(objShared.ConVisitel, objClientInfoDataObject, objClientBasicInfoDataObject, objClientTreatmentInfoDataObject,
                                objClientEmergencyContactInfoDataObject, objClientCaseDataObject, objClientCaseCareInfoDataObject, objClientCaseBillingInfoDataObject,
                                objClientCaseSantraxInfoDataObject)

                            IsSaved = True

                        Catch ex As SqlException

                            If ex.Message.Contains("Duplicate Client") Then
                                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DuplicateClientMessage)
                            Else
                                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to Insert")
                            End If
                        End Try

                        If (IsSaved) Then
                            DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(InsertMessage)
                            'BindClientDropDownList(DropDownListSearchByIndividual, CompanyId, EnumDataObject.ClientListFor.Individual)
                            'DropDownListSearchByIndividual.SelectedValue = LastSelectedClientId
                        End If

                        Exit Select
                    Case False
                        objClientInfoDataObject.ClientId = Convert.ToInt32(HiddenFieldClientId.Value)
                        objClientCaseDataObject.CaseId = Convert.ToInt32(HiddenFieldClientCaseId.Value)
                        objClientInfoDataObject.UpdateBy = Convert.ToString(UserId)
                        objClientInfoDataObject.UserId = UserId
                        objClientInfoDataObject.CompanyId = CompanyId
                        Try
                            objBLClientInfo.UpdateClientInfo(objShared.ConVisitel, objClientInfoDataObject, objClientBasicInfoDataObject, objClientTreatmentInfoDataObject,
                                objClientEmergencyContactInfoDataObject, objClientCaseDataObject, objClientCaseCareInfoDataObject, objClientCaseBillingInfoDataObject,
                                objClientCaseSantraxInfoDataObject)

                            IsSaved = True

                        Catch ex As SqlException

                            If ex.Message.Equals("Duplicate Client") Then
                                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DuplicateClientMessage)
                            ElseIf ex.Message.Equals("Duplicate Client Case") Then
                                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DuplicateClientCaseMessage)
                            Else
                                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to Update")
                            End If
                        End Try

                        If (IsSaved) Then
                            DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(UpdateMessage)
                        End If

                        Exit Select
                End Select
                objBLClientInfo = Nothing

                objClientInfoDataObject = Nothing
                objClientBasicInfoDataObject = Nothing
                objClientTreatmentInfoDataObject = Nothing
                objClientEmergencyContactInfoDataObject = Nothing

                objClientCaseDataObject = Nothing
                objClientCaseCareInfoDataObject = Nothing
                objClientCaseBillingInfoDataObject = Nothing
                objClientCaseSantraxInfoDataObject = Nothing

                If (IsSaved) Then
                    HiddenFieldIsNew.Value = Convert.ToString(True, Nothing)
                    HiddenFieldClientId.Value = Convert.ToString(Int32.MinValue)
                    HiddenFieldClientCaseId.Value = Convert.ToString(Int32.MinValue)
                    HiddenFieldIsSearched.Value = Convert.ToString(False, Nothing)
                End If

            End If
        End Sub

        ''' <summary>
        ''' This is for filling out diagnosis information
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub FillDiagnosis()

            TextBoxDiagnosis.Text = String.Empty

            If (Not Convert.ToString(DropDownListBillingDiagonosisOne.SelectedItem, Nothing).Equals("Please Select...")) Then
                TextBoxDiagnosis.Text &= Convert.ToString(DropDownListBillingDiagonosisOne.SelectedItem, Nothing)
            End If

            If (Not Convert.ToString(DropDownListBillingDiagonosisTwo.SelectedItem, Nothing).Equals("Please Select...")) Then
                TextBoxDiagnosis.Text &= If(String.IsNullOrEmpty(TextBoxDiagnosis.Text.Trim()),
                                        (Convert.ToString(DropDownListBillingDiagonosisTwo.SelectedItem, Nothing)),
                                        ("," & Environment.NewLine & Convert.ToString(DropDownListBillingDiagonosisTwo.SelectedItem, Nothing)))
            End If

            If (Not Convert.ToString(DropDownListBillingDiagonosisThree.SelectedItem, Nothing).Equals("Please Select...")) Then
                TextBoxDiagnosis.Text &= If(String.IsNullOrEmpty(TextBoxDiagnosis.Text.Trim()),
                                        (Convert.ToString(DropDownListBillingDiagonosisThree.SelectedItem, Nothing)),
                                        ("," & Environment.NewLine & Convert.ToString(DropDownListBillingDiagonosisThree.SelectedItem, Nothing)))
            End If

            If (Not Convert.ToString(DropDownListBillingDiagonosisFour.SelectedItem, Nothing).Equals("Please Select...")) Then
                TextBoxDiagnosis.Text &= If(String.IsNullOrEmpty(TextBoxDiagnosis.Text.Trim()),
                                        (Convert.ToString(DropDownListBillingDiagonosisFour.SelectedItem, Nothing)),
                                        ("," & Environment.NewLine & Convert.ToString(DropDownListBillingDiagonosisFour.SelectedItem, Nothing)))
            End If

        End Sub

        Private Sub IndividualStatus(objClientTreatmentInfoDataObject As ClientTreatmentInfoDataObject, objClientCaseDataObject As ClientCaseDataObject)

            If (Not String.IsNullOrEmpty(objClientTreatmentInfoDataObject.ServiceStartDate) And Not String.IsNullOrEmpty(objClientTreatmentInfoDataObject.ServiceEndDate) _
                    And Not String.IsNullOrEmpty(objClientCaseDataObject.BeginDate) And Not String.IsNullOrEmpty(objClientCaseDataObject.EndDate)) Then

                If (Convert.ToDateTime(objClientTreatmentInfoDataObject.ServiceStartDate)) <= DateTime.Today Then
                    HiddenFieldIndividualStatus.Value = "Active"
                Else
                    HiddenFieldIndividualStatus.Value = If(((Convert.ToDateTime(objClientTreatmentInfoDataObject.ServiceEndDate)) <= DateTime.Today), "Inactive", "Active")
                End If

            End If

            If (String.IsNullOrEmpty(objClientTreatmentInfoDataObject.ServiceStartDate)) Then
                HiddenFieldIndividualStatus.Value = "Inactive"
            End If

            If (Not String.IsNullOrEmpty(objClientTreatmentInfoDataObject.ServiceEndDate) And String.IsNullOrEmpty(objClientTreatmentInfoDataObject.ServiceStartDate)) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Select Service Start Date. This is not a valid client.")
            End If
        End Sub

    End Class

End Namespace