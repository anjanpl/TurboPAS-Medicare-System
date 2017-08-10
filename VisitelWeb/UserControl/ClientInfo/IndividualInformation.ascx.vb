Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports System.Data.SqlClient

Namespace Visitel.UserControl.ClientInfo

    Public Class IndividualInformationControl
        Inherits BaseUserControl

        Private ControlName As String, EditText As String, DeleteText As String, InsertMessage As String, UpdateMessage As String, DeleteMessage As String, _
        DuplicateNameMessage As String, EmptyNameMessage As String, StatusSelectionMessage As String, ReceiverSelectionMessage As String, PayerSelectionMessage As String, _
        InvalidContractNumberMessage As String, SaveConfirmMessage As String, DuplicateClientMessage As String, _
        DuplicateClientCaseMessage As String, ValidationGroup As String, SocialSecurityNumber As String, StateClientId As String

        Private ValidationEnable As Boolean

        Private CompanyId As Int32, UserId As Int32

        Private IndividualId As Int64

        Private ClientCaseList As New List(Of ClientCaseDataObject)()
        Private ClientCaseCareInfoList As New List(Of ClientCaseCareInfoDataObject)()
        Private ClientCaseBillingInfoList As New List(Of ClientCaseBillingInfoDataObject)()
        Private ClientCaseEVVInfoList As New List(Of ClientCaseEVVInfoDataObject)()

        Private objShared As SharedWebControls
        Public hfIsSearched As HiddenField

        Public HealthAssessOrSDPVisible As Boolean, SupVisitVisible As Boolean, MapquestVisible As Boolean, SaveVisible As Boolean, DeleteVisible As Boolean,
            ClearVisible As Boolean, CoordinationOfCareVisible As Boolean, SynchronizeDiagnosisCodeVisible As Boolean, IndividualDetailVisible As Boolean,
            BillingInfoVisible As Boolean

        Private TextBoxDiagnosis As TextBox
        Private DropDownListClientType As DropDownList

        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "ClientInfoControl"
            objShared = New SharedWebControls()
            objShared.ConnectionOpen()
            GetCaptionFromResource()
            InitializeControl()
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

            Dim currentPageName As String = System.IO.Path.GetFileName(Request.Url.AbsolutePath)

            Select Case currentPageName
                Case "ClientInfo.aspx"
                    LoadCss("ClientInfo/" & ControlName)
                    Exit Select
                Case "ClientDetailInfo.aspx"
                    LoadCss("ClientInfo/" & ControlName)
                    Exit Select
                Case "ClientInfoBilling.aspx"
                    'LoadCss("ClientInfo/ClientInfoBillingControl")
                    LoadCss("ClientInfo/" & ControlName)
                    Exit Select
            End Select

            LoadJScript()

            ButtonHealthAssessOrSDP.Visible = HealthAssessOrSDPVisible
            ButtonSupVisit.Visible = SupVisitVisible
            ButtonMapquest.Visible = MapquestVisible
            ButtonSave.Visible = SaveVisible
            ButtonDelete.Visible = DeleteVisible
            ButtonClear.Visible = ClearVisible
            ButtonCoordinationOfCare.Visible = CoordinationOfCareVisible
            ButtonSynchronizeDiagnosisCode.Visible = SynchronizeDiagnosisCodeVisible
            ButtonIndividualDetail.Visible = IndividualDetailVisible
            ButtonBillingInformation.Visible = BillingInfoVisible

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
            If (Convert.ToInt64(HiddenFieldClientInfoId.Value) > 0) Then
                DeleteData()
            End If
            objShared.TabSelection(1)
        End Sub

        Private Sub ButtonClear_Click(sender As Object, e As EventArgs)
            ClearClientInfoControl()
            ClearClientCaseInfoControl()

            If (ucService.Visible) Then
                ucService.ClearControls()
            End If
            
            'DropDownListSearchByIndividual.SelectedIndex = 0
            'ButtonIndividual.Enabled = If(DropDownListSearchByIndividual.SelectedValue.Equals("-1"), False, True)

            objShared.TabSelection(1)
        End Sub

        Private Sub ButtonIndividualBilling_Click(sender As Object, e As EventArgs)
            Response.Redirect("~/Pages/ClientInfoBilling.aspx?ClientId=" & HiddenFieldClientInfoId.Value)
        End Sub

        Private Sub ButtonIndividualDetail_Click(sender As Object, e As EventArgs)
            Response.Redirect("~/Pages/ClientInfo.aspx?ClientId=" & HiddenFieldClientInfoId.Value)
        End Sub

        Private Sub DropDownListClientType_OnSelectedIndexChanged(sender As Object, e As EventArgs)

            If (Not IndividualId.Equals(-1)) Then
                ClearClientCaseInfoControl()
            End If

            Dim ClientType As String = "-1"
            If (ucIndividualOtherInformation.Visible) Then
                ClientType = ucIndividualOtherInformation.GetClientType()
            End If

            If Not ClientType.Equals("-1") Then

                '**********************************************Client Case[Start]*********************************

                '#Region "Client Case Information"

                Dim objClientCaseDataObject As New ClientCaseDataObject
                Try
                    If ((ClientCaseList.Count < 1) And (Not Session(StaticSettings.SessionField.CLIENT_CASE_LIST) Is Nothing)) Then
                        ClientCaseList = DirectCast(Session(StaticSettings.SessionField.CLIENT_CASE_LIST), List(Of ClientCaseDataObject))
                    End If
                    objClientCaseDataObject = (From p In ClientCaseList Where p.Type = ClientType).SingleOrDefault
                Catch ex As InvalidOperationException

                    If ex.Message.Contains("Sequence contains no elements") Then
                        objClientCaseDataObject = Nothing
                        Return
                    End If
                End Try

                If (Not objClientCaseDataObject Is Nothing) Then
                    If (ucIndividualOtherInformation.Visible) Then
                        ucIndividualOtherInformation.ClientCaseInfoFillOutData(objClientCaseDataObject)
                    End If

                    TextBoxInsuranceNumber.Text = objClientCaseDataObject.InsuranceNumber
                End If

                objClientCaseDataObject = Nothing

                '#End Region


                '#Region "Client Case Care Information"

                Dim objClientCaseCareInfoDataObject As New ClientCaseCareInfoDataObject

                If ((ClientCaseCareInfoList.Count < 1) And (Not Session(StaticSettings.SessionField.CLIENT_CASE_CARE_INFO_LIST) Is Nothing)) Then
                    ClientCaseCareInfoList = DirectCast(Session(StaticSettings.SessionField.CLIENT_CASE_CARE_INFO_LIST), List(Of ClientCaseCareInfoDataObject))
                End If

                objClientCaseCareInfoDataObject = (From p In ClientCaseCareInfoList Where p.Type = ClientType).SingleOrDefault

                If (Not objClientCaseCareInfoDataObject Is Nothing) Then

                    If (ucIndividualOtherInformation.Visible) Then
                        ucIndividualOtherInformation.ClientCaseCareInfoFillOutData(objClientCaseCareInfoDataObject)
                    End If

                    TextBoxSupervisorVisitFrequency.Text = If(objClientCaseCareInfoDataObject.SupervisorVisitFrequency.Equals(Int32.MinValue),
                                                              String.Empty, Convert.ToString(objClientCaseCareInfoDataObject.SupervisorVisitFrequency))

                    If (ucCurrentAuthorizationPeriod.Visible) Then
                        ucCurrentAuthorizationPeriod.FillOutData(objClientCaseCareInfoDataObject)
                    End If

                    If (ucAuthorizationDetail.Visible) Then
                        ucAuthorizationDetail.FillOutData(objClientCaseCareInfoDataObject)
                    End If
                End If

                objClientCaseCareInfoDataObject = Nothing

                '#End Region

                '#Region "Client Case Billing Information"

                Dim objClientCaseBillingInfoDataObject As New ClientCaseBillingInfoDataObject

                If ((ClientCaseBillingInfoList.Count < 1) And (Not Session(StaticSettings.SessionField.CLIENT_CASE_BILLING_INFO_LIST) Is Nothing)) Then
                    ClientCaseBillingInfoList = DirectCast(Session(StaticSettings.SessionField.CLIENT_CASE_BILLING_INFO_LIST), List(Of ClientCaseBillingInfoDataObject))
                End If

                objClientCaseBillingInfoDataObject = (From p In ClientCaseBillingInfoList Where p.Type = ClientType).SingleOrDefault

                If (Not objClientCaseBillingInfoDataObject Is Nothing) Then

                    TextBoxAssessmentDate.Text = objClientCaseBillingInfoDataObject.AssessmentDate

                    If (ucBillingDiagnosisCode.Visible) Then
                        ucBillingDiagnosisCode.FillOutData(objClientCaseBillingInfoDataObject)
                    End If

                    If (ucBillingInfo.Visible) Then
                        ucBillingInfo.FillOutData(objClientCaseBillingInfoDataObject)
                    End If

                End If

                objClientCaseBillingInfoDataObject = Nothing

                '#End Region

                '#Region "Client Case EVV Information"

                Dim objClientCaseEVVInfoDataObject As New ClientCaseEVVInfoDataObject

                If ((ClientCaseEVVInfoList.Count < 1) And (Not Session(StaticSettings.SessionField.CLIENT_CASE_EVV_INFO_LIST) Is Nothing)) Then
                    ClientCaseEVVInfoList = DirectCast(Session(StaticSettings.SessionField.CLIENT_CASE_EVV_INFO_LIST), List(Of ClientCaseEVVInfoDataObject))
                End If

                objClientCaseEVVInfoDataObject = (From p In ClientCaseEVVInfoList Where p.Type = ClientType).SingleOrDefault

                If (Not objClientCaseEVVInfoDataObject Is Nothing) Then
                    If (ucEVVInfo.Visible) Then
                        ucEVVInfo.FillOutData(objClientCaseEVVInfoDataObject)
                    End If

                    If (ucEVV.Visible) Then
                        ucEVV.FillOutData(objClientCaseEVVInfoDataObject)
                    End If
                End If

                objClientCaseEVVInfoDataObject = Nothing

                '#End Region

                '**********************************************Client Case[End]*********************************
            End If
        End Sub

        Private Sub LoadJScript()

            'LoadJS("JavaScript/jquery.blockUI.js")

            'LoadJS("JavaScript/masonry.pkgd.min.js")

            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                           & " var CompanyId = " & CompanyId & "; " _
                           & " var ClientId = ''; " _
                           & " var CalendarDateFormat='" & objShared.CalendarDateFormat & "'; " _
                           & " var CalendarImagePath='" & objShared.GetCalendarImagePath & "'; " _
                           & " var LoaderImagePath ='" & objShared.GetLoaderImagePath & "'; " _
                           & " var DeleteTargetButton ='ButtonDelete'; " _
                           & " var DeleteDialogHeader ='Individual Information'; " _
                           & " var DeleteDialogConfirmMsg ='Do you want to delete?'; " _
                           & " var prm =''; " _
                           & " jQuery(document).ready(function () {" _
                           & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                           & "     prm.add_beginRequest(SetButtonActionProgress); " _
                           & "     prm.add_endRequest(EndRequest); " _
                           & "     DataDelete();" _
                           & "     DateFieldsEvent();" _
                           & "     InAutoGrowEvent();" _
                           & "     InputMasking();" _
                           & "     AgeCalculate();" _
                           & "     RegisterAddressControls();" _
                           & "     prm.add_endRequest(DataDelete); " _
                           & "     prm.add_endRequest(InAutoGrowEvent); " _
                           & "     prm.add_endRequest(DateFieldsEvent); " _
                           & "     prm.add_endRequest(InputMasking); " _
                           & "     prm.add_endRequest(AgeCalculate); " _
                           & "     prm.add_endRequest(RegisterAddressControls); " _
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
                objShared.BindStateDropDownList(DropDownListState, CompanyId)
                objShared.BindMaritalStatusDropDownList(DropDownListMaritalStatus, CompanyId)

                objShared.BindGenderDropDownList(DropDownListGender)
                BindCountyDropDownList()
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError("Unable to fetch data.")
            End Try

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
        End Sub


        ''' <summary>
        ''' Clearing controls value
        ''' </summary>
        Public Sub ClearClientCaseInfoControl()

            '**********************************************Client Case[Start]*********************************

            '#Region "Client Case Information"

            TextBoxInsuranceNumber.Text = String.Empty

            '#End Region

            '#Region "Client Case Care Information"

            TextBoxSupervisorVisitFrequency.Text = String.Empty

            If (ucCurrentAuthorizationPeriod.Visible) Then
                ucCurrentAuthorizationPeriod.ClearControls()
            End If

            If (ucAuthorizationDetail.Visible) Then
                ucAuthorizationDetail.ClearControls()
            End If

            '#End Region

            '#Region "Client Case Billing Information"

            TextBoxAssessmentDate.Text = String.Empty

            If (ucBillingInfo.Visible) Then
                ucBillingInfo.ClearControls()
            End If

            If (ucBillingDiagnosisCode.Visible) Then
                ucBillingDiagnosisCode.ClearControls()
            End If

            '#End Region

            '#Region "Client Case EVV Information"

            If (ucEVVInfo.Visible) Then
                ucEVVInfo.ClearControls()
            End If

            If (ucEVV.Visible) Then
                ucEVV.ClearControls()
            End If

            '#End Region

            '**********************************************Client Case[End]*********************************
        End Sub

        Private Sub LoadSelectedClient()

            HiddenFieldIsNew.Value = Convert.ToString(False, Nothing)
            HiddenFieldIsSearched.Value = Convert.ToString(True, Nothing)
            HiddenFieldClientInfoId.Value = IndividualId

            If (ucService.Visible) Then
                ucService.SetIndividualId(IndividualId)
                ucService.GetData()
            End If
            
            ButtonDelete.Enabled = If(HiddenFieldIsSearched.Value.Equals(Convert.ToString(True, Nothing)), True, False)

            Dim objBLClientInfo As New BLClientInfo()
            Dim Client As HybridDictionary = objBLClientInfo.SelectClientInfo(objShared.ConVisitel, Me.CompanyId, Me.IndividualId, Me.SocialSecurityNumber)
            objBLClientInfo = Nothing

            '**********************************************Client Info[Start]*********************************

            '#Region "Client Basic Information"

            Dim objClientBasicInfoDataObject As New ClientBasicInfoDataObject
            objClientBasicInfoDataObject = Client.Item("ClientBasicInfoDataObject")

            If objClientBasicInfoDataObject.ClientInfoId.Equals(Int64.MinValue) Then
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

                If (ucIndividualOtherInformation.Visible) Then
                    ucIndividualOtherInformation.ClientBasicInfoFillOutData(objClientBasicInfoDataObject)
                End If

            End If

            objClientBasicInfoDataObject = Nothing

            '#End Region

            '#Region "Client Treatment Information"

            Dim objClientTreatmentInfoDataObject As New ClientTreatmentInfoDataObject
            objClientTreatmentInfoDataObject = Client.Item("ClientTreatmentInfoDataObject")

            If (Not objClientTreatmentInfoDataObject Is Nothing) Then

                TextBoxServiceStartDate.Text = If((String.IsNullOrEmpty(Convert.ToString(objClientTreatmentInfoDataObject.ServiceStartDate, Nothing))),
                                              String.Empty, objClientTreatmentInfoDataObject.ServiceStartDate)

                TextBoxServiceEndDate.Text = If((String.IsNullOrEmpty(Convert.ToString(objClientTreatmentInfoDataObject.ServiceEndDate, Nothing))),
                                                String.Empty, objClientTreatmentInfoDataObject.ServiceEndDate)

                DropDownListCounty.SelectedIndex = DropDownListCounty.Items.IndexOf(DropDownListCounty.Items.FindByValue(
                                                   Convert.ToString(objClientTreatmentInfoDataObject.CountyId)))

                If (ucEmergencyContract.Visible) Then
                    ucEmergencyContract.FillOutData(objClientTreatmentInfoDataObject)
                End If

                TextBoxSupervisor.Text = objClientTreatmentInfoDataObject.Supervisor
                TextBoxLiaison.Text = objClientTreatmentInfoDataObject.Liaison

                If (ucIndividualOtherInformation.Visible) Then
                    ucIndividualOtherInformation.ClientTreatmentInfoFillOutData(objClientTreatmentInfoDataObject)
                End If

            End If

            'objClientTreatmentInfoDataObject = Nothing

            '#End Region

            '#Region "Client Emergency Contact Information"
            Dim objClientEmergencyContactInfoDataObject As New ClientEmergencyContactInfoDataObject
            objClientEmergencyContactInfoDataObject = Client.Item("ClientEmergencyContactInfoDataObject")

            If (Not objClientEmergencyContactInfoDataObject Is Nothing) Then
                If (ucEmergencyContract.Visible) Then
                    ucEmergencyContract.FillOutData(objClientEmergencyContactInfoDataObject)
                End If
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

                If (ucIndividualOtherInformation.Visible) Then
                    ucIndividualOtherInformation.ClientCaseInfoFillOutData(objClientCaseDataObject)
                End If
                
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

                If (ucIndividualOtherInformation.Visible) Then
                    ucIndividualOtherInformation.ClientCaseCareInfoFillOutData(objClientCaseCareInfoDataObject)
                End If

                TextBoxSupervisorVisitFrequency.Text = If(objClientCaseCareInfoDataObject.SupervisorVisitFrequency.Equals(Int32.MinValue),
                                                          String.Empty,
                                                          Convert.ToString(objClientCaseCareInfoDataObject.SupervisorVisitFrequency))

                If (ucCurrentAuthorizationPeriod.Visible) Then
                    ucCurrentAuthorizationPeriod.FillOutData(objClientCaseCareInfoDataObject)
                End If

                If (ucAuthorizationDetail.Visible) Then
                    ucAuthorizationDetail.FillOutData(objClientCaseCareInfoDataObject)
                End If

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
                                            String.Empty, objClientCaseBillingInfoDataObject.AssessmentDate)

                If (ucBillingDiagnosisCode.Visible) Then
                    ucBillingDiagnosisCode.FillOutData(objClientCaseBillingInfoDataObject)
                End If

                If (ucBillingInfo.Visible) Then
                    ucBillingInfo.FillOutData(objClientCaseBillingInfoDataObject)
                End If

                objClientCaseBillingInfoDataObject = Nothing

            End If

            '#End Region

            '#Region "Client Case EVV Information"

            ClientCaseEVVInfoList = Nothing
            ClientCaseEVVInfoList = Client.Item("ClientCaseEVVInfoList")
            Session.Add(StaticSettings.SessionField.CLIENT_CASE_EVV_INFO_LIST, ClientCaseEVVInfoList)

            Dim objClientCaseEVVInfoDataObject As New ClientCaseEVVInfoDataObject
            objClientCaseEVVInfoDataObject = (From p In ClientCaseEVVInfoList).FirstOrDefault

            If (Not objClientCaseEVVInfoDataObject Is Nothing) Then
                If (ucEVVInfo.Visible) Then
                    If (ucEVV.Visible) Then
                        ucEVV.LoadData()
                        ucEVV.FillOutData(objClientCaseEVVInfoDataObject)
                    End If
                    ucEVVInfo.LoadData()
                    ucEVVInfo.FillOutData(objClientCaseEVVInfoDataObject)
                End If
                objClientCaseEVVInfoDataObject = Nothing
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
                                        objShared.InlineAssignHelper(TextBoxZip.Text, objShared.InlineAssignHelper(TextBoxAssessmentDate.Text, String.Empty))))))))))))

            DropDownListCity.SelectedIndex = objShared.InlineAssignHelper(DropDownListState.SelectedIndex,
                                             objShared.InlineAssignHelper(DropDownListIndividualStatus.SelectedIndex,
                                             objShared.InlineAssignHelper(DropDownListGender.SelectedIndex,
                                             objShared.InlineAssignHelper(DropDownListMaritalStatus.SelectedIndex, 0))))

            'ButtonIndividual.Text = "Search Client"

            '#End Region

            '#Region "Client Treatment Information"
            TextBoxServiceStartDate.Text = objShared.InlineAssignHelper(TextBoxServiceEndDate.Text, objShared.InlineAssignHelper(TextBoxSupervisor.Text,
                                           objShared.InlineAssignHelper(TextBoxLiaison.Text, String.Empty)))

            DropDownListCounty.SelectedIndex = 0

            '#End Region

            '#Region "Client Emergency Contact Information"

            If (ucEmergencyContract.Visible) Then
                ucEmergencyContract.ClearControls()
            End If

            '#End Region

            If (ucIndividualOtherInformation.Visible) Then
                ucIndividualOtherInformation.ClearClientInfoControl()
            End If

            HiddenFieldIsNew.Value = Convert.ToString(True, Nothing)
            HiddenFieldIsSearched.Value = Convert.ToString(True, Nothing)
            HiddenFieldClientInfoId.Value = Convert.ToString(Int64.MinValue)

        End Sub

        ''' <summary>
        ''' Setting StateClientId from Parent Page
        ''' </summary>
        ''' <param name="StateClientId"></param>
        ''' <remarks></remarks>
        Public Sub SetStateClientId(StateClientId As String)
            Me.StateClientId = StateClientId
        End Sub

        ''' <summary>
        ''' Setting SocialSecurityNumber from Parent Page
        ''' </summary>
        ''' <param name="SocialSecurityNumber"></param>
        ''' <remarks></remarks>
        Public Sub SetSocialSecurityNumber(SocialSecurityNumber As String)
            Me.SocialSecurityNumber = SocialSecurityNumber
        End Sub

        ''' <summary>
        ''' Set Company, User Id, and Individual Id
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub SetClientId(ClientInfoId As Int64)
            IndividualId = ClientInfoId
            HiddenFieldClientInfoId.Value = ClientInfoId
            CompanyId = objShared.CompanyId
            UserId = objShared.UserId
        End Sub

        Private Sub DeleteData()

            Dim ClientCaseId As String = "-1"

            If (ucIndividualOtherInformation.Visible) Then
                ClientCaseId = ucIndividualOtherInformation.GetHiddenFieldClientCaseId()
            End If

            If HiddenFieldIsSearched.Value.Equals(Convert.ToString(True, Nothing)) Then

                Dim IsDeleted As Boolean = False
                Dim objBLClientInfo As New BLClientInfo()

                Try
                    objBLClientInfo.DeleteClientInfo(objShared.ConVisitel, CompanyId, IndividualId, Convert.ToInt32(ClientCaseId), UserId)
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

            Dim currentPageName As String = System.IO.Path.GetFileName(Request.Url.AbsolutePath)

            ucBillingInfo.ClientIDMode = ClientIDMode.Static
            ucEVV.ClientIDMode = ClientIDMode.Static
            ucEVVInfo.ClientIDMode = ClientIDMode.Static

            Select Case currentPageName
                Case "ClientInfo.aspx"
                    ucBillingInfo.Visible = False
                    ucEVV.Visible = True
                    ucEVVInfo.Visible = False
                    ucBillingDiagnosisCode.Visible = False
                    ucAuthorizationDetail.Visible = False
                    ucIndividualOtherInformation.Visible = False
                    ucCurrentAuthorizationPeriod.Visible = False
                    ucService.Visible = True
                    ucEmergencyContract.Visible = True
                    Exit Select
                Case "ClientDetailInfo.aspx"
                    ucBillingInfo.Visible = False
                    ucEVV.Visible = True
                    ucEVVInfo.Visible = True
                    ucBillingDiagnosisCode.Visible = False
                    ucAuthorizationDetail.Visible = True
                    ucIndividualOtherInformation.Visible = True
                    ucCurrentAuthorizationPeriod.Visible = True
                    ucService.Visible = False
                    ucEmergencyContract.Visible = True
                    Exit Select
                Case "ClientInfoBilling.aspx"
                    ucBillingInfo.Visible = True
                    ucEVV.Visible = False
                    ucEVVInfo.Visible = False
                    ucBillingDiagnosisCode.Visible = True
                    ucAuthorizationDetail.Visible = True
                    ucIndividualOtherInformation.Visible = True
                    ucCurrentAuthorizationPeriod.Visible = True
                    ucService.Visible = False
                    ucEmergencyContract.Visible = True
                    Exit Select
            End Select

            If (ucIndividualOtherInformation.Visible) Then
                Me.TextBoxDiagnosis = ucIndividualOtherInformation.GetDiagnosisTextBox()
            End If

            If (ucBillingDiagnosisCode.Visible) Then
                ucBillingDiagnosisCode.TextBoxDiagnosis = Me.TextBoxDiagnosis
            End If

            TextBoxSupervisorVisitFrequency.Attributes.Add("onkeypress", "return isNumericKey(event);")

            TextBoxAge.Attributes.Add("onkeypress", "return isNumericKey(event);")

            TextBoxPhone.Attributes.Add("onchange", "onChangeOfValue(this);")

            SetControlToolTip()

            SetRegularExpressionSetting()

            DefineControlTextLength()

            TextBoxAge.ReadOnly = True

            LinkButtonAddMoreIndividualStatus.Attributes("href") = "../Settings/Popup/ClientStatusPopup.aspx?Mode=Insert&TB_iframe=true&height=500&width=720"
            LinkButtonAddMoreIndividualStatus.Attributes("title") = "Client Status Setting"

            LinkButtonAddMoreCity.Attributes("href") = "../Settings/Popup/CityPopup.aspx?Mode=Insert&TB_iframe=true&height=450&width=620"
            LinkButtonAddMoreCity.Attributes("title") = "City Setting"

            LinkButtonAddMoreState.Attributes("href") = "../Settings/Popup/StatePopup.aspx?Mode=Insert&TB_iframe=true&height=450&width=620"
            LinkButtonAddMoreState.Attributes("title") = "State Setting"

            LinkButtonAddMoreCounty.Attributes("href") = "../Settings/Popup/CountyPopup.aspx?Mode=Insert&TB_iframe=true&height=450&width=620"
            LinkButtonAddMoreCounty.Attributes("title") = "County Setting"

            ButtonCoordinationOfCare.Attributes.Add("OnClick", String.Format("ShowCoordinationOfCareReport(); return false; "))
            ButtonCoordinationOfCare.ClientIDMode = ClientIDMode.Static
            ButtonCoordinationOfCare.UseSubmitBehavior = False

            ButtonSave.CausesValidation = True
            ButtonSave.ValidationGroup = ValidationGroup

            ButtonDelete.CausesValidation = objShared.InlineAssignHelper(ButtonClear.CausesValidation, False)

            AddHandler ButtonSave.Click, AddressOf ButtonSave_Click
            AddHandler ButtonDelete.Click, AddressOf ButtonDelete_Click

            AddHandler ButtonClear.Click, AddressOf ButtonClear_Click

            AddHandler ButtonIndividualDetail.Click, AddressOf ButtonIndividualDetail_Click
            AddHandler ButtonBillingInformation.Click, AddressOf ButtonIndividualBilling_Click

            If (ucIndividualOtherInformation.Visible) Then
                Me.DropDownListClientType = ucIndividualOtherInformation.GetClientTypeDropDownList()

                Me.DropDownListClientType.AutoPostBack = True
                AddHandler DropDownListClientType.SelectedIndexChanged, AddressOf DropDownListClientType_OnSelectedIndexChanged
            End If

            DropDownListCity.ClientIDMode = objShared.InlineAssignHelper(DropDownListState.ClientIDMode, ClientIDMode.Static)

            TextBoxPhone.ClientIDMode = objShared.InlineAssignHelper(TextBoxAddress.ClientIDMode,
                                        objShared.InlineAssignHelper(TextBoxServiceStartDate.ClientIDMode,
                                        objShared.InlineAssignHelper(TextBoxServiceEndDate.ClientIDMode, objShared.InlineAssignHelper(TextBoxDateOfBirth.ClientIDMode,
                                        objShared.InlineAssignHelper(TextBoxAssessmentDate.ClientIDMode,
                                        objShared.InlineAssignHelper(TextBoxSocialSecurityNumber.ClientIDMode,
                                        objShared.InlineAssignHelper(TextBoxAlternatePhone.ClientIDMode,
                                        objShared.InlineAssignHelper(TextBoxAge.ClientIDMode,
                                        objShared.InlineAssignHelper(TextBoxZip.ClientIDMode,
                                        objShared.InlineAssignHelper(TextBoxApartmentNumber.ClientIDMode,
                                        objShared.InlineAssignHelper(TextBoxMiddleInitial.ClientIDMode, ClientIDMode.Static)))))))))))

            If (ucEmergencyContract.Visible) Then
                ucEmergencyContract.InitializeControl()
            End If

            ButtonClear.ClientIDMode = ClientIDMode.Static
            ButtonSave.ClientIDMode = ClientIDMode.Static
            ButtonDelete.ClientIDMode = ClientIDMode.Static
            ButtonSynchronizeDiagnosisCode.ClientIDMode = ClientIDMode.Static
            HiddenFieldClientInfoId.ClientIDMode = ClientIDMode.Static

            SetControlTabIndex()

        End Sub

        Private Sub SetControlTabIndex()
            DropDownListIndividualStatus.TabIndex = 1
            TextBoxStateClientId.TabIndex = 2
            TextBoxFirstName.TabIndex = 3
            TextBoxMiddleInitial.TabIndex = 4
            TextBoxLastName.TabIndex = 5
            DropDownListCounty.TabIndex = 6
            TextBoxAddress.TabIndex = 7
            TextBoxApartmentNumber.TabIndex = 8
            DropDownListCity.TabIndex = 9
            DropDownListState.TabIndex = 10
            TextBoxZip.TabIndex = 11
            TextBoxPhone.TabIndex = 12
            TextBoxAlternatePhone.TabIndex = 13
            TextBoxDateOfBirth.TabIndex = 14
            TextBoxSocialSecurityNumber.TabIndex = 15
            DropDownListGender.TabIndex = 16
            DropDownListMaritalStatus.TabIndex = 17
            TextBoxServiceStartDate.TabIndex = 18
            TextBoxServiceEndDate.TabIndex = 19
            
            TextBoxAssessmentDate.TabIndex = 29
           
            ButtonCoordinationOfCare.TabIndex = 62

            ButtonHealthAssessOrSDP.TabIndex = 63
            ButtonSupVisit.TabIndex = 64
            ButtonMapquest.TabIndex = 65
            ButtonClear.TabIndex = 66
            ButtonSave.TabIndex = 67
            ButtonDelete.TabIndex = 68
            ButtonSynchronizeDiagnosisCode.TabIndex = 69
            ButtonBillingInformation.TabIndex = 70
        End Sub

        Private Sub SetControlToolTip()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("Setup", ControlName & Convert.ToString(".resx"))

            Dim DateToolTip As String = Convert.ToString(ResourceTable("DateFieldToolTip"), Nothing)
            'DateToolTip = If(String.IsNullOrEmpty(DateToolTip), "Example: August 21, 2014", DateToolTip)
            DateToolTip = If(String.IsNullOrEmpty(DateToolTip), "Example: 10/27/2015", DateToolTip)

            TextBoxAssessmentDate.ToolTip = objShared.InlineAssignHelper(TextBoxDateOfBirth.ToolTip,
                                            objShared.InlineAssignHelper(TextBoxServiceStartDate.ToolTip,
                                            objShared.InlineAssignHelper(TextBoxServiceEndDate.ToolTip, DateToolTip)))

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

            ErrorMessage = Convert.ToString(ResourceTable("InvalidSocialSecurityNumberMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Social Security Number.", ErrorMessage)

            ErrorText = ErrorMessage

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorSocialSecurityNumber, "TextBoxSocialSecurityNumber",
                                                           objShared.SocialSecurityNumberValidationExpression, ErrorMessage, ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidUnitsMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Units", ErrorMessage)

            ErrorText = ErrorMessage

            'objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorUnits, "TextBoxUnits", objShared.TimeValidationExpression,
            '                                     ErrorMessage, ErrorText, ValidationEnable, ValidationGroup)

            'objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorUnits, "TextBoxUnits", objShared.TimeValidationExpression,
            '                                     ErrorMessage, ErrorText, ValidationEnable, ValidationGroup)

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
            objShared.SetControlTextLength(TextBoxLiaison, 50)
            objShared.SetControlTextLength(TextBoxPhone, 14)
            objShared.SetControlTextLength(TextBoxAlternatePhone, 14)

            objShared.SetControlTextLength(TextBoxZip, 16)
            

            objShared.SetControlTextLength(TextBoxAge, 3)

            '**********************************************Client Info[End]*********************************


            '**********************************************Client[Start]*********************************

            objShared.SetControlTextLength(TextBoxInsuranceNumber, 50)
            objShared.SetControlTextLength(TextBoxSupervisorVisitFrequency, 2)
            
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

            LabelCounty.Text = Convert.ToString(ResourceTable("LabelCounty"), Nothing).Trim()
            LabelCounty.Text = If(String.IsNullOrEmpty(LabelCounty.Text), "County", LabelCounty.Text)

            LabelSupervisor.Text = Convert.ToString(ResourceTable("LabelSupervisor"), Nothing).Trim()
            LabelSupervisor.Text = If(String.IsNullOrEmpty(LabelSupervisor.Text), "Supervisor", LabelSupervisor.Text)

            LabelLiaison.Text = Convert.ToString(ResourceTable("LabelLiaison"), Nothing).Trim()
            LabelLiaison.Text = If(String.IsNullOrEmpty(LabelLiaison.Text), "Liaison", LabelLiaison.Text)

            '#End Region

            '**********************************************Client Info[End]************************************************************

            '**********************************************Client Case[Start]*****************************************************

            '#Region "Client Case Information"            

            LabelInsuranceNumber.Text = Convert.ToString(ResourceTable("LabelInsuranceNumber"), Nothing).Trim()
            LabelInsuranceNumber.Text = If(String.IsNullOrEmpty(LabelInsuranceNumber.Text), "Insurance Number", LabelInsuranceNumber.Text)

            '#End Region

            '#Region "Client Case Care Information"

            LabelSupervisorVisitFrequency.Text = Convert.ToString(ResourceTable("LabelSupervisorVisitFrequency"), Nothing).Trim()
            LabelSupervisorVisitFrequency.Text = If(String.IsNullOrEmpty(LabelSupervisorVisitFrequency.Text), "Sup Visit Freq:", LabelSupervisorVisitFrequency.Text)

            '#End Region


            ''#Region "Client Case Billing Information"

            LabelAssessmentDate.Text = Convert.ToString(ResourceTable("LabelAssessmentDate"), Nothing).Trim()
            LabelAssessmentDate.Text = If(String.IsNullOrEmpty(LabelAssessmentDate.Text), "Assessment Date:", LabelAssessmentDate.Text)

            '#End Region

            LabelAge.Text = Convert.ToString(ResourceTable("LabelAge"), Nothing).Trim()
            LabelAge.Text = If(String.IsNullOrEmpty(LabelAge.Text), "Age:", LabelAge.Text)


            '**********************************************Client Case[End]*****************************************************

            ButtonCoordinationOfCare.Text = Convert.ToString(ResourceTable("ButtonCoordinationOfCare"), Nothing).Trim()
            ButtonCoordinationOfCare.Text = If(String.IsNullOrEmpty(ButtonCoordinationOfCare.Text),
                                               String.Format("Coordination {0} of Care", Environment.NewLine), ButtonCoordinationOfCare.Text)

            ButtonHealthAssessOrSDP.Text = Convert.ToString(ResourceTable("ButtonHealthAssessOrSDP"), Nothing).Trim()
            ButtonHealthAssessOrSDP.Text = If(String.IsNullOrEmpty(ButtonHealthAssessOrSDP.Text),
                                              String.Format("Health {0} Assess/SDP", Environment.NewLine), ButtonHealthAssessOrSDP.Text)

            ButtonSupVisit.Text = Convert.ToString(ResourceTable("ButtonSupVisit"), Nothing).Trim()
            ButtonSupVisit.Text = If(String.IsNullOrEmpty(ButtonSupVisit.Text), String.Format("Sup Visit - Att {0} Orientation", Environment.NewLine), ButtonSupVisit.Text)

            ButtonMapquest.Text = Convert.ToString(ResourceTable("ButtonMapquest"), Nothing).Trim()
            ButtonMapquest.Text = If(String.IsNullOrEmpty(ButtonMapquest.Text), "Mapquest", ButtonMapquest.Text)

            ButtonClear.Text = Convert.ToString(ResourceTable("ButtonClear"), Nothing).Trim()
            ButtonClear.Text = If(String.IsNullOrEmpty(ButtonClear.Text), "Clear", ButtonClear.Text)

            ButtonSave.Text = Convert.ToString(ResourceTable("ButtonSave"), Nothing).Trim()
            ButtonSave.Text = If(String.IsNullOrEmpty(ButtonSave.Text), "Save", ButtonSave.Text)

            ButtonDelete.Text = Convert.ToString(ResourceTable("ButtonDelete"), Nothing).Trim()
            ButtonDelete.Text = If(String.IsNullOrEmpty(ButtonDelete.Text), "Delete", ButtonDelete.Text)

            ButtonSynchronizeDiagnosisCode.Text = Convert.ToString(ResourceTable("ButtonSynchronizeDiagnosisCode"), Nothing).Trim()
            ButtonSynchronizeDiagnosisCode.Text = If(String.IsNullOrEmpty(ButtonSynchronizeDiagnosisCode.Text),
                                                     String.Format("Synchronize {0} Diagnosis Code", Environment.NewLine), ButtonSynchronizeDiagnosisCode.Text)

            ButtonIndividualDetail.Text = Convert.ToString(ResourceTable("ButtonIndividualDetail"), Nothing).Trim()
            ButtonIndividualDetail.Text = If(String.IsNullOrEmpty(ButtonIndividualDetail.Text),
                                             String.Format("Individual {0} Detail", Environment.NewLine), ButtonIndividualDetail.Text)

            ButtonBillingInformation.Text = Convert.ToString(ResourceTable("ButtonBillingInformation"), Nothing).Trim()
            ButtonBillingInformation.Text = If(String.IsNullOrEmpty(ButtonBillingInformation.Text), "Billing Info",
                                                     ButtonBillingInformation.Text)

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
            DuplicateClientMessage = If(String.IsNullOrEmpty(DuplicateClientMessage), "This client alredy exists along with first name, last name and medicaid id.",
                                        DuplicateClientMessage)

            DuplicateClientCaseMessage = Convert.ToString(ResourceTable("DuplicateClientCaseMessage"), Nothing).Trim()
            DuplicateClientCaseMessage = If(String.IsNullOrEmpty(DuplicateClientCaseMessage), "The same client case already exists.", DuplicateClientCaseMessage)

            DuplicateNameMessage = Convert.ToString(ResourceTable("DuplicateNameMessage"), Nothing).Trim()
            DuplicateNameMessage = If(String.IsNullOrEmpty(DuplicateNameMessage), "The same name already exists.", DuplicateNameMessage)

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

            If (Not objShared.ValidateZip(TextBoxZip.Text.Trim())) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Invalid Zip Code")
                Return False
            End If

            If DropDownListGender.SelectedValue = "0" Then
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

            If (ucEmergencyContract.Visible) Then
                If (Not ucEmergencyContract.ValidateData()) Then
                    Return False
                End If
            End If

            '#End Region

            '**********************************************Client Info[End]*********************************

            '**********************************************Client Case[Start]*********************************

            '#Region "Client Case Information"

            'If (Not String.IsNullOrEmpty(TextBoxUnits.Text.Trim())) Then
            '    If (Not objShared.ValidateTime(TextBoxUnits.Text.Trim())) Then
            '        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorUnits.ErrorMessage)
            '        Return False
            '    End If
            'End If            

            '#End Region

            '#Region "Client Case Care Information"

            If (Not String.IsNullOrEmpty(TextBoxSupervisorVisitFrequency.Text.Trim())) Then
                If (Not objShared.IsInteger(TextBoxSupervisorVisitFrequency.Text.Trim())) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Invalid Supervisor Visit Frequency")
                    Return False
                End If
            End If

            If (ucCurrentAuthorizationPeriod.Visible) Then
                If (Not ucCurrentAuthorizationPeriod.ValidateData()) Then
                    Return False
                End If
            End If

            If (ucAuthorizationDetail.Visible) Then
                If (Not ucAuthorizationDetail.ValidateData()) Then
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

            If (ucBillingInfo.Visible) Then
                If (Not ucBillingInfo.ValidateData()) Then
                    Return False
                End If
            End If
            
            '#End Region

            '#Region "Client Case EVV Information"

            If (Not ucEVV.ValidateData()) Then
                Return False
            End If

            '#End Region

            '**********************************************Client Case[End]*********************************
            Return True
        End Function

        Private Sub SaveData(IsConfirm As Boolean)


            'If (Page.IsPostBack) Then
            '    Page.Validate()
            '    If (Not Page.IsValid) Then
            '        Dim msg As String = String.Empty
            '        ' Loop through all validation controls to see which     
            '        ' generated the error(s).
            '        Dim oValidator As IValidator = Nothing

            'For Each aValidator As IValidator In Page.Validators

            '    If Not aValidator.IsValid Then
            '        msg += "<br />" + aValidator.ErrorMessage
            '    End If
            'Next


            'For Each cValidator As IValidator In Page.GetValidators(Nothing)
            '    Dim bv As BaseValidator = TryCast(cValidator, BaseValidator)
            '    bv.CssClass = "Error"
            '    bv.Display = ValidatorDisplay.Dynamic
            '    bv.ForeColor = System.Drawing.Color.Empty
            'Next


            'For Each aRegularExpressionValidator As RegularExpressionValidator In Page.Validators
            '    If Not aRegularExpressionValidator.IsValid Then
            '        msg += "<br />" & "Id:" & aRegularExpressionValidator.ClientID & "Message: " & aRegularExpressionValidator.ErrorMessage
            '    End If

            'Next

            'For Each aRequiredFieldValidator As RequiredFieldValidator In Page.Validators
            '    If Not aRequiredFieldValidator.IsValid Then
            '        msg += "<br />" & "Id:" & aRequiredFieldValidator.ClientID & "Message: " & aRequiredFieldValidator.ErrorMessage
            '    End If

            'Next


            'Label1.Text = msg
            '    End If
            'End If

            'Page.Validate()
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
                'objClientBasicInfoDataObject.Phone = objShared.GetReFormattedMobileNumber(Convert.ToString(TextBoxPhone.Text, Nothing).Trim())
                objClientBasicInfoDataObject.Phone = Convert.ToString(TextBoxPhone.Text, Nothing).Trim()

                'objClientBasicInfoDataObject.AlternatePhone = objShared.GetReFormattedMobileNumber(Convert.ToString(TextBoxAlternatePhone.Text, Nothing).Trim())
                objClientBasicInfoDataObject.AlternatePhone = Convert.ToString(TextBoxAlternatePhone.Text, Nothing).Trim()

                If (ucIndividualOtherInformation.Visible) Then
                    ucIndividualOtherInformation.DataFoSaveBasicInfo(objClientBasicInfoDataObject)
                End If

                objClientBasicInfoDataObject.DateOfBirth = Convert.ToString(TextBoxDateOfBirth.Text, Nothing).Trim()
                objClientBasicInfoDataObject.SocialSecurityNumber = Convert.ToString(TextBoxSocialSecurityNumber.Text, Nothing).Trim()

                'objClientBasicInfoDataObject.Address = Convert.ToString(TextBoxAddress.Text, Nothing).Trim().Substring(0, Convert.ToString(TextBoxAddress.Text, Nothing).Trim().LastIndexOf("Apt#")).Trim()

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

                If (ucIndividualOtherInformation.Visible) Then
                    ucIndividualOtherInformation.DataFoSaveTreatmentInfo(objClientTreatmentInfoDataObject)
                End If

                objClientTreatmentInfoDataObject.Supervisor = Convert.ToString(TextBoxSupervisor.Text, Nothing).Trim()
                objClientTreatmentInfoDataObject.Liaison = Convert.ToString(TextBoxLiaison.Text, Nothing).Trim()
                '#End Region

                '#Region "Client Emergency Contact Information"

                If (ucEmergencyContract.Visible) Then
                    ucEmergencyContract.DataFoSave(objClientEmergencyContactInfoDataObject)
                    ucEmergencyContract.DataFoSave(objClientTreatmentInfoDataObject)
                End If

                '#End Region
                '**********************************************Client Info[End]*********************************

                '**********************************************Client Case[Start]*********************************

                Dim objClientCaseDataObject As New ClientCaseDataObject()
                Dim objClientCaseCareInfoDataObject As New ClientCaseCareInfoDataObject()
                Dim objClientCaseBillingInfoDataObject As New ClientCaseBillingInfoDataObject()
                Dim objClientCaseEVVInfoDataObject As New ClientCaseEVVInfoDataObject()

                Dim ClientType As String = "-1"
                If (ucIndividualOtherInformation.Visible) Then
                    ClientType = ucIndividualOtherInformation.GetClientType()
                End If

                If Not ClientType.Equals("-1") Then

                    '#Region "Client Case Information"

                    If (ucIndividualOtherInformation.Visible) Then
                        ucIndividualOtherInformation.DataFoSaveClientCaseInfo(objClientCaseDataObject)
                    End If

                    objClientCaseDataObject.InsuranceNumber = Convert.ToString(TextBoxInsuranceNumber.Text, Nothing).Trim()

                    '#End Region

                    '#Region "Client Case Care Information"

                    If (ucIndividualOtherInformation.Visible) Then
                        ucIndividualOtherInformation.DataFoSaveClientCaseCareInfo(objClientCaseCareInfoDataObject)
                    End If

                    objClientCaseCareInfoDataObject.SupervisorVisitFrequency = If((String.IsNullOrEmpty(TextBoxSupervisorVisitFrequency.Text.Trim())),
                                                                                    objClientCaseCareInfoDataObject.SupervisorVisitFrequency,
                                                                                    Convert.ToInt32(TextBoxSupervisorVisitFrequency.Text.Trim()))

                    If (ucCurrentAuthorizationPeriod.Visible) Then
                        ucCurrentAuthorizationPeriod.DataFoSave(objClientCaseCareInfoDataObject)
                    End If

                    If (ucAuthorizationDetail.Visible) Then
                        ucAuthorizationDetail.DataFoSave(objClientCaseCareInfoDataObject)
                    End If

                    '#End Region


                    '#Region "Client Case Billing Information"

                    objClientCaseBillingInfoDataObject.AssessmentDate = Convert.ToString(TextBoxAssessmentDate.Text, Nothing).Trim()

                    If (ucBillingDiagnosisCode.Visible) Then
                        ucBillingDiagnosisCode.DataFoSave(objClientCaseBillingInfoDataObject)
                    End If

                    If (ucBillingInfo.Visible) Then
                        ucBillingInfo.DataFoSave(objClientCaseBillingInfoDataObject)
                    End If

                    '#End Region                   

                End If

                '#Region "Client Case EVV Information"

                If (ucEVVInfo.Visible) Then
                    ucEVVInfo.DataFoSave(objClientCaseEVVInfoDataObject)
                End If

                If (ucEVV.Visible) Then
                    ucEVV.DataFoSave(objClientCaseEVVInfoDataObject)
                End If

                '#End Region

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
                                objClientCaseEVVInfoDataObject)

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
                        objClientInfoDataObject.ClientInfoId = Convert.ToInt64(HiddenFieldClientInfoId.Value)

                        If (ucIndividualOtherInformation.Visible) Then
                            ucIndividualOtherInformation.GetClientCaseId(objClientCaseDataObject)
                        End If

                        objClientInfoDataObject.UpdateBy = Convert.ToString(UserId)
                        objClientInfoDataObject.UserId = UserId
                        objClientInfoDataObject.CompanyId = CompanyId
                        Try
                            objBLClientInfo.UpdateClientInfo(objShared.ConVisitel, objClientInfoDataObject, objClientBasicInfoDataObject, objClientTreatmentInfoDataObject,
                                objClientEmergencyContactInfoDataObject, objClientCaseDataObject, objClientCaseCareInfoDataObject, objClientCaseBillingInfoDataObject,
                                objClientCaseEVVInfoDataObject)

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
                objClientCaseEVVInfoDataObject = Nothing

                If (IsSaved) Then
                    HiddenFieldIsNew.Value = Convert.ToString(True, Nothing)
                    HiddenFieldClientInfoId.Value = Convert.ToString(Int64.MinValue)

                    If (ucIndividualOtherInformation.Visible) Then
                        ucIndividualOtherInformation.SetClientCaseId()
                    End If

                    HiddenFieldIsSearched.Value = Convert.ToString(False, Nothing)
                End If
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