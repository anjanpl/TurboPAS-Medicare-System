Imports VisitelCommon.VisitelCommon
Imports System.Data.SqlClient
Imports VisitelBusiness.VisitelBusiness.Settings
Imports VisitelBusiness.VisitelBusiness.DataObject.Settings
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelBusiness.VisitelBusiness

Namespace Visitel.UserControl.EmployeeInfo
    Public Class EmployeeInformationControl
        Inherits BaseUserControl

        Private objShared As SharedWebControls
        Private ControlName As String, EditText As String, DeleteText As String, InsertMessage As String, UpdateMessage As String, DeleteMessage As String,
            DeleteConfirmationMessage As String, AutofillConfirmationMessage As String, ValidationGroup As String, DateFieldToolTip As String, DuplicateEmployeeMessage As String

        Private ValidationEnable As Boolean
        Private EmployeeId As Int32, UserId As Int32, CompanyId As Int32, SocialSecurityNumber As String

        Protected Sub Page_Init(sender As Object, e As EventArgs)

            ControlName = "EmployeeInfoControl"

            objShared = New SharedWebControls()
            objShared.ConnectionOpen()

            Me.CompanyId = objShared.CompanyId
            Me.UserId = objShared.UserId

            GetCaptionFromResource()
            InitializeControl()

        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)
            If Not IsPostBack Then
                HiddenFieldIsNew.Value = Convert.ToString(True, Nothing)
                If (String.IsNullOrEmpty(Request.QueryString("EmployeeId"))) Then
                    GetData()
                End If
            End If
        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadCss("EmployeeInfo/" + ControlName)
            LoadJavaScript()
            ButtonDelete.Enabled = If(HiddenFieldIsNew.Value.Equals(Convert.ToString(True, Nothing)), False, True)
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing
        End Sub

        Private Sub ButtonDelete_Click(sender As Object, e As EventArgs)
            DeleteData()
            objShared.TabSelection(1)
        End Sub

        Private Sub DeleteData()
            If HiddenFieldIsNew.Value.Equals(Convert.ToString(False, Nothing)) Then

                Dim IsDeleted As Boolean = False
                Dim objBLEmployeeSetting As New BLEmployeeSetting()

                Try

                    objBLEmployeeSetting.DeleteEmployeeInfo(objShared.ConVisitel, Me.CompanyId, Convert.ToInt32(HiddenFieldEmployeeId.Value), Me.UserId)
                    IsDeleted = True

                Catch ex As SqlException
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to Delete")
                End Try

                objBLEmployeeSetting = Nothing

                If (IsDeleted) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DeleteMessage)
                    ClearControl()
                    ButtonDelete.Enabled = If(Me.EmployeeId.Equals(-1), False, True)
                    ClearHiddenFieldValue()

                    'objShared.BindEmployeeDropDownList(DropDownListSearchByEmployee, CompanyId, False)
                    'DropDownListSearchByEmployee.SelectedIndex = 0
                End If
            End If

        End Sub

        Private Sub ButtonClear_Click(sender As Object, e As EventArgs)
            ClearControl()
            objShared.TabSelection(1)
        End Sub

        Private Sub ButtonSave_Click(sender As Object, e As EventArgs)
            SaveData(False)
            objShared.TabSelection(1)
        End Sub

        ''' <summary>
        ''' Setting Employee Id from Parent Page
        ''' </summary>
        ''' <param name="EmployeeId"></param>
        ''' <remarks></remarks>
        Public Sub SetEmployeeId(EmployeeId As Integer)
            Me.EmployeeId = EmployeeId
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
        ''' This is for control events regristering and initialize controls
        ''' </summary>
        Private Sub InitializeControl()

            TextBoxUpdateDate.ReadOnly = objShared.InlineAssignHelper(TextBoxUpdateBy.ReadOnly, objShared.InlineAssignHelper(TextBoxTurboPASEmployeeId.ReadOnly, True))

            TextBoxNumberOfDepartment.Attributes.Add("onkeypress", "return isNumericKey(event);")
            TextBoxNumberOfVerifiedReference.Attributes.Add("onkeypress", "return isNumericKey(event);")
            TextBoxAge.Attributes.Add("onkeypress", "return isNumericKey(event);")

            TextBoxAge.ReadOnly = True

            TextBoxDateOfBirth.ToolTip = objShared.InlineAssignHelper(TextBoxApplicationDate.ToolTip,
                                            objShared.InlineAssignHelper(TextBoxReferenceVerificationDate.ToolTip,
                                            objShared.InlineAssignHelper(TextBoxHireDate.ToolTip,
                                            objShared.InlineAssignHelper(TextBoxSignedJobDescriptionDate.ToolTip,
                                            objShared.InlineAssignHelper(TextBoxOrientationDate.ToolTip,
                                            objShared.InlineAssignHelper(TextBoxAssignedTaskEvaluationDate.ToolTip,
                                            objShared.InlineAssignHelper(TextBoxCrimcheckDate.ToolTip,
                                            objShared.InlineAssignHelper(TextBoxRegistryDate.ToolTip,
                                            objShared.InlineAssignHelper(TextBoxLastEvaluationDate.ToolTip,
                                            objShared.InlineAssignHelper(TextBoxPolicyOrProcedureSettlementDate.ToolTip,
                                            objShared.InlineAssignHelper(TextBoxOIGDate.ToolTip,
                                            objShared.InlineAssignHelper(TextBoxOIGReportedToStateDate.ToolTip,
                                            objShared.InlineAssignHelper(TextBoxStartDate.ToolTip,
                                            objShared.InlineAssignHelper(TextBoxEndDate.ToolTip,
                                            DateFieldToolTip))))))))))))))

            SetRegularExpressionSetting()

            DefineControlTextLength()

            LinkButtonAddMoreCity.Attributes("href") = "../Settings/Popup/CityPopup.aspx?Mode=Insert&TB_iframe=true&height=450&width=620"
            LinkButtonAddMoreCity.Attributes("title") = "City Setting"

            LinkButtonAddMoreState.Attributes("href") = "../Settings/Popup/StatePopup.aspx?Mode=Insert&TB_iframe=true&height=450&width=620"
            LinkButtonAddMoreState.Attributes("title") = "State Setting"

            ButtonSave.CausesValidation = True
            ButtonSave.ValidationGroup = ValidationGroup

            ButtonDelete.CausesValidation = objShared.InlineAssignHelper(ButtonClear.CausesValidation, False)

            TextBoxDateOfBirth.ClientIDMode = objShared.InlineAssignHelper(TextBoxApplicationDate.ClientIDMode,
                                              objShared.InlineAssignHelper(TextBoxReferenceVerificationDate.ClientIDMode,
                                              objShared.InlineAssignHelper(TextBoxHireDate.ClientIDMode,
                                              objShared.InlineAssignHelper(TextBoxSignedJobDescriptionDate.ClientIDMode,
                                              objShared.InlineAssignHelper(TextBoxOrientationDate.ClientIDMode,
                                              objShared.InlineAssignHelper(TextBoxAssignedTaskEvaluationDate.ClientIDMode,
                                              objShared.InlineAssignHelper(TextBoxCrimcheckDate.ClientIDMode,
                                              objShared.InlineAssignHelper(TextBoxRegistryDate.ClientIDMode,
                                              objShared.InlineAssignHelper(TextBoxLastEvaluationDate.ClientIDMode,
                                              objShared.InlineAssignHelper(TextBoxPolicyOrProcedureSettlementDate.ClientIDMode,
                                              objShared.InlineAssignHelper(TextBoxOIGDate.ClientIDMode,
                                              objShared.InlineAssignHelper(TextBoxOIGReportedToStateDate.ClientIDMode,
                                              objShared.InlineAssignHelper(TextBoxStartDate.ClientIDMode,
                                              objShared.InlineAssignHelper(TextBoxEndDate.ClientIDMode,
                                              objShared.InlineAssignHelper(TextBoxAge.ClientIDMode, ClientIDMode.Static)))))))))))))))

            AddHandler ButtonAutoFill.Click, AddressOf ButtonAutoFill_Click
            AddHandler ButtonSave.Click, AddressOf ButtonSave_Click
            AddHandler ButtonDelete.Click, AddressOf ButtonDelete_Click
            AddHandler ButtonClear.Click, AddressOf ButtonClear_Click

            ButtonSave.ClientIDMode = ClientIDMode.Static
            ButtonDelete.ClientIDMode = ClientIDMode.Static
            ButtonClear.ClientIDMode = ClientIDMode.Static
            ButtonAutoFill.ClientIDMode = ClientIDMode.Static

            TextBoxSocialSecurityNumber.ClientIDMode = ClientIDMode.Static
            TextBoxPhone.ClientIDMode = ClientIDMode.Static
            TextBoxAlternatePhone.ClientIDMode = ClientIDMode.Static
            'TextBoxSantraxGPSPhone.ClientIDMode = ClientIDMode.Static
            TextBoxZip.ClientIDMode = ClientIDMode.Static
            TextBoxMiddleNameInitial.ClientIDMode = ClientIDMode.Static

            SetControlTabIndex()

            HyperLinkCrimcheckDate.NavigateUrl = Convert.ToString(ConfigurationManager.AppSettings("CrimcheckLink"), Nothing)
            HyperLinkMisConductRequest.NavigateUrl = Convert.ToString(ConfigurationManager.AppSettings("MisConductRequestLink"), Nothing)

        End Sub

        Private Sub SetControlTabIndex()
            DropDownListLicenseStatus.TabIndex = 1
            DropDownListTitle.TabIndex = 2

            'Payroll Id

            TextBoxFirstName.TabIndex = 4
            TextBoxMiddleNameInitial.TabIndex = 5
            TextBoxLastName.TabIndex = 6
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
            TextBoxNumberOfDepartment.TabIndex = 18
            TextBoxNumberOfVerifiedReference.TabIndex = 19
            TextBoxPayrate.TabIndex = 20
            DropDownListRehire.TabIndex = 21
            TextBoxComments.TabIndex = 22
            CheckBoxMailCheck.TabIndex = 23

            TextBoxApplicationDate.TabIndex = 24
            TextBoxReferenceVerificationDate.TabIndex = 25
            TextBoxHireDate.TabIndex = 26
            TextBoxSignedJobDescriptionDate.TabIndex = 27
            TextBoxOrientationDate.TabIndex = 28
            TextBoxAssignedTaskEvaluationDate.TabIndex = 29
            TextBoxCrimcheckDate.TabIndex = 30
            TextBoxRegistryDate.TabIndex = 31
            TextBoxLastEvaluationDate.TabIndex = 32
            TextBoxPolicyOrProcedureSettlementDate.TabIndex = 33
            TextBoxOIGDate.TabIndex = 34
            TextBoxOIGReportedToStateDate.TabIndex = 35
            TextBoxStartDate.TabIndex = 36
            TextBoxEndDate.TabIndex = 37

            'TextBoxSantraxCDSPayrate.TabIndex = 38
            'TextBoxSantraxGPSPhone.TabIndex = 39
            'TextBoxSantraxEmployeeId.TabIndex = 40
            'TextBoxSantraxDiscipline.TabIndex = 41
        End Sub


        ''' <summary>
        ''' This is for validating data in server side before submitting the form
        ''' </summary>
        ''' <returns></returns>
        Private Function ValidateData() As Boolean

            Dim ValidationMessage As String = String.Empty

            If (String.IsNullOrEmpty(TextBoxFirstName.Text.Trim())) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please enter client first name.")
                Return False
            End If

            If (String.IsNullOrEmpty(TextBoxLastName.Text.Trim())) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please enter client last name.")
                Return False
            End If

            If (Not String.IsNullOrEmpty(TextBoxPhone.Text.Trim())) Then
                If (Not objShared.ValidatePhone(TextBoxPhone.Text.Trim)) Then
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

            'If (Not String.IsNullOrEmpty(TextBoxSocialSecurityNumber.Text.Trim())) Then
            '    If (Not objShared.ValidateSocialSecurityNumber(TextBoxSocialSecurityNumber.Text.Trim)) Then
            '        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorSocialSecurityNumber.ErrorMessage)
            '        Return False
            '    End If
            'End If

            If ((Not String.IsNullOrEmpty(TextBoxStartDate.Text.Trim())) And (Not objShared.ValidateDate(TextBoxStartDate.Text.Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorStartDate.ErrorMessage)
                Return False
            End If

            If ((Not String.IsNullOrEmpty(TextBoxEndDate.Text.Trim())) And (Not objShared.ValidateDate(TextBoxEndDate.Text.Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorEndDate.ErrorMessage)
                Return False
            End If

            If ((Not String.IsNullOrEmpty(TextBoxNumberOfVerifiedReference.Text.Trim())) And (Not objShared.IsInteger(TextBoxNumberOfVerifiedReference.Text.Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please enter integer value for number of verified reference.")
                Return False
            End If

            If ((Not String.IsNullOrEmpty(TextBoxNumberOfDepartment.Text.Trim())) And (Not objShared.IsInteger(TextBoxNumberOfDepartment.Text.Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please enter integer value for number of department.")
                Return False
            End If

            If DropDownListMaritalStatus.SelectedValue = "-1" Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Select Marital Status")
                Return False
            End If

            If ((Not String.IsNullOrEmpty(TextBoxPayrate.Text.Trim()))) And (Not objShared.ValidatePayrate(TextBoxPayrate.Text.Trim())) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorPayrate.ErrorMessage)
                Return False
            End If

            If DropDownListTitle.SelectedValue = "-1" Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Select Title")
                Return False
            End If

            If ((Not String.IsNullOrEmpty(TextBoxApplicationDate.Text.Trim())) And (Not objShared.ValidateDate(TextBoxApplicationDate.Text.Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorApplicationDate.ErrorMessage)
                Return False
            End If

            If ((Not String.IsNullOrEmpty(TextBoxHireDate.Text.Trim())) And (Not objShared.ValidateDate(TextBoxHireDate.Text.Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorHireDate.ErrorMessage)
                Return False
            End If

            If ((Not String.IsNullOrEmpty(TextBoxSignedJobDescriptionDate.Text.Trim())) And (Not objShared.ValidateDate(TextBoxSignedJobDescriptionDate.Text.Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorSignedJobDescriptionDate.ErrorMessage)
                Return False
            End If

            If ((Not String.IsNullOrEmpty(TextBoxReferenceVerificationDate.Text.Trim())) And (Not objShared.ValidateDate(TextBoxReferenceVerificationDate.Text.Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorReferenceVerificationDate.ErrorMessage)
                Return False
            End If

            If ((Not String.IsNullOrEmpty(TextBoxOrientationDate.Text.Trim())) And (Not objShared.ValidateDate(TextBoxOrientationDate.Text.Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorOrientationDate.ErrorMessage)
                Return False
            End If

            If ((Not String.IsNullOrEmpty(TextBoxPolicyOrProcedureSettlementDate.Text.Trim())) And (Not objShared.ValidateDate(TextBoxPolicyOrProcedureSettlementDate.Text.Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorPolicyOrProcedureSettlementDate.ErrorMessage)
                Return False
            End If

            If ((Not String.IsNullOrEmpty(TextBoxAssignedTaskEvaluationDate.Text.Trim())) And (Not objShared.ValidateDate(TextBoxAssignedTaskEvaluationDate.Text.Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorAssignedTaskEvaluationDate.ErrorMessage)
                Return False
            End If

            If ((Not String.IsNullOrEmpty(TextBoxCrimcheckDate.Text.Trim())) And (Not objShared.ValidateDate(TextBoxCrimcheckDate.Text.Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorCrimcheckDate.ErrorMessage)
                Return False
            End If

            If ((Not String.IsNullOrEmpty(TextBoxRegistryDate.Text.Trim())) And (Not objShared.ValidateDate(TextBoxRegistryDate.Text.Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorRegistryDate.ErrorMessage)
                Return False
            End If

            If ((Not String.IsNullOrEmpty(TextBoxLastEvaluationDate.Text.Trim())) And (Not objShared.ValidateDate(TextBoxLastEvaluationDate.Text.Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorLastEvaluationDate.ErrorMessage)
                Return False
            End If

            If ((Not String.IsNullOrEmpty(TextBoxOIGReportedToStateDate.Text.Trim())) And (Not objShared.ValidateDate(TextBoxOIGReportedToStateDate.Text.Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorOIGReportedToStateDate.ErrorMessage)
                Return False
            End If

            'If ((Not String.IsNullOrEmpty(TextBoxSantraxCDSPayrate.Text.Trim()))) And (Not objShared.ValidatePayrate(TextBoxSantraxCDSPayrate.Text.Trim())) Then
            '    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorSantraxCDSPayrate.ErrorMessage)
            '    Return False
            'End If

            'If (Not String.IsNullOrEmpty(TextBoxSantraxGPSPhone.Text.Trim())) Then
            '    If (Not objShared.ValidatePhone(TextBoxSantraxGPSPhone.Text.Trim)) Then
            '        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorSantraxGPSPhone.ErrorMessage)
            '        Return False
            '    End If
            'End If

            Return True
        End Function

        ''' <summary>
        ''' Saving Employee Information
        ''' </summary>
        ''' <param name="IsConfirm"></param>
        ''' <remarks></remarks>
        Private Sub SaveData(IsConfirm As Boolean)
            Page.Validate()
            Page.Validate(ValidationGroup)

            If ((Page.IsValid) And (ValidateData())) Then

                Dim objEmployeeSettingDataObject As New EmployeeSettingDataObject()

                objEmployeeSettingDataObject.EmploymentStatus = If((String.IsNullOrEmpty(TextBoxEndDate.Text)),
                                                                   EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EmploymentStatus.CURRENT),
                                                                   EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EmploymentStatus.EXPIRED))

                objEmployeeSettingDataObject.EmploymentStatusId = If((DropDownListEmploymentStatus.SelectedValue = Nothing),
                                                                objEmployeeSettingDataObject.EmploymentStatusId, Convert.ToInt32(DropDownListEmploymentStatus.SelectedValue))

                objEmployeeSettingDataObject.LicenseStatus = If((DropDownListLicenseStatus.SelectedValue = Nothing),
                                                                objEmployeeSettingDataObject.LicenseStatus, Convert.ToInt32(DropDownListLicenseStatus.SelectedValue))

                objEmployeeSettingDataObject.ClientGroupId = If((DropDownListClientGroup.SelectedValue = Nothing),
                                                                objEmployeeSettingDataObject.ClientGroupId, Convert.ToInt32(DropDownListClientGroup.SelectedValue))

                objEmployeeSettingDataObject.OIGResult = Convert.ToString(TextBoxOIGResult.Text, Nothing).Trim()

                objEmployeeSettingDataObject.Title = If((DropDownListTitle.SelectedValue = Nothing),
                                                                objEmployeeSettingDataObject.Title, Convert.ToInt32(DropDownListTitle.SelectedValue))

                objEmployeeSettingDataObject.PayrollNumber = TextBoxPayrollId.Text.Trim()

                objEmployeeSettingDataObject.FirstName = Convert.ToString(TextBoxFirstName.Text, Nothing).Trim()
                objEmployeeSettingDataObject.MiddleNameInitial = Convert.ToString(TextBoxMiddleNameInitial.Text, Nothing).Trim()
                objEmployeeSettingDataObject.LastName = Convert.ToString(TextBoxLastName.Text, Nothing).Trim()
                objEmployeeSettingDataObject.Address = Convert.ToString(TextBoxAddress.Text, Nothing).Trim()
                objEmployeeSettingDataObject.ApartmentNumber = Convert.ToString(TextBoxApartmentNumber.Text, Nothing).Trim()

                objEmployeeSettingDataObject.CityId = If((DropDownListCity.SelectedValue = Nothing),
                                                                objEmployeeSettingDataObject.CityId, Convert.ToInt32(DropDownListCity.SelectedValue))

                objEmployeeSettingDataObject.StateId = If((DropDownListState.SelectedValue = Nothing),
                                                                objEmployeeSettingDataObject.StateId, Convert.ToInt32(DropDownListState.SelectedValue))

                objEmployeeSettingDataObject.Zip = If((Not String.IsNullOrEmpty(TextBoxZip.Text.Trim())),
                                                      Convert.ToString(TextBoxZip.Text, Nothing).Trim(),
                                                      objEmployeeSettingDataObject.Zip)

                'objEmployeeSettingDataObject.Phone = objShared.GetReFormattedMobileNumber(Convert.ToString(TextBoxPhone.Text, Nothing).Trim())
                objEmployeeSettingDataObject.Phone = Convert.ToString(TextBoxPhone.Text, Nothing).Trim()

                'objEmployeeSettingDataObject.AlternatePhone = objShared.GetReFormattedMobileNumber(Convert.ToString(TextBoxAlternatePhone.Text, Nothing).Trim())
                objEmployeeSettingDataObject.AlternatePhone = Convert.ToString(TextBoxAlternatePhone.Text, Nothing).Trim()

                objEmployeeSettingDataObject.DateOfBirth = Convert.ToString(TextBoxDateOfBirth.Text, Nothing).Trim()
                'objEmployeeSettingDataObject.SocialSecurityNumber = objShared.GetReFormattedSocialSecurityNumber(Convert.ToString(TextBoxSocialSecurityNumber.Text, Nothing).Trim())
                objEmployeeSettingDataObject.SocialSecurityNumber = Convert.ToString(TextBoxSocialSecurityNumber.Text, Nothing).Trim()

                objEmployeeSettingDataObject.Gender = If((DropDownListGender.SelectedValue = Nothing),
                                                                objEmployeeSettingDataObject.Gender, Convert.ToInt32(DropDownListGender.SelectedValue))

                objEmployeeSettingDataObject.NumberOfDepartment = If((Not String.IsNullOrEmpty(TextBoxNumberOfDepartment.Text.Trim())),
                                                                    Convert.ToInt32(TextBoxNumberOfDepartment.Text),
                                                                    objEmployeeSettingDataObject.NumberOfDepartment)

                objEmployeeSettingDataObject.NumberOfVerifiedReference = If((Not String.IsNullOrEmpty(TextBoxNumberOfVerifiedReference.Text.Trim())),
                                                                            Convert.ToInt32(TextBoxNumberOfVerifiedReference.Text),
                                                                            objEmployeeSettingDataObject.NumberOfVerifiedReference)

                objEmployeeSettingDataObject.MaritalStatus = If((DropDownListMaritalStatus.SelectedValue = Nothing),
                                                               objEmployeeSettingDataObject.MaritalStatus, Convert.ToInt32(DropDownListMaritalStatus.SelectedValue))

                If (String.IsNullOrEmpty(TextBoxPayrate.Text.Trim())) Then
                    TextBoxPayrate.Text = 0
                End If

                objEmployeeSettingDataObject.Payrate = Convert.ToDecimal(objShared.GetReFormattedPayrate(TextBoxPayrate.Text.Trim()), Nothing)

                objEmployeeSettingDataObject.RehireYesNo = If(DropDownListRehire.SelectedValue.Equals("1"), 1, 0)
                objEmployeeSettingDataObject.Comments = Convert.ToString(TextBoxComments.Text, Nothing).Trim()
                objEmployeeSettingDataObject.MailCheck = If(CheckBoxMailCheck.Checked, 1, 0)

                objEmployeeSettingDataObject.ApplicationDate = Convert.ToString(TextBoxApplicationDate.Text, Nothing).Trim()
                objEmployeeSettingDataObject.ReferenceVerificationDate = Convert.ToString(TextBoxReferenceVerificationDate.Text, Nothing).Trim()
                objEmployeeSettingDataObject.HireDate = Convert.ToString(TextBoxHireDate.Text, Nothing).Trim()
                objEmployeeSettingDataObject.SignedJobDescriptionDate = Convert.ToString(TextBoxSignedJobDescriptionDate.Text, Nothing).Trim()
                objEmployeeSettingDataObject.OrientationDate = Convert.ToString(TextBoxOrientationDate.Text, Nothing).Trim()
                objEmployeeSettingDataObject.AssignedTaskEvaluationDate = Convert.ToString(TextBoxAssignedTaskEvaluationDate.Text, Nothing).Trim()
                objEmployeeSettingDataObject.CrimcheckDate = Convert.ToString(TextBoxCrimcheckDate.Text, Nothing).Trim()
                objEmployeeSettingDataObject.RegistryDate = Convert.ToString(TextBoxRegistryDate.Text, Nothing).Trim()
                objEmployeeSettingDataObject.LastEvaluationDate = Convert.ToString(TextBoxLastEvaluationDate.Text, Nothing).Trim()
                objEmployeeSettingDataObject.PolicyOrProcedureSettlementDate = Convert.ToString(TextBoxPolicyOrProcedureSettlementDate.Text, Nothing).Trim()
                objEmployeeSettingDataObject.OIGDate = Convert.ToString(TextBoxOIGDate.Text, Nothing).Trim()
                objEmployeeSettingDataObject.OIGReportedToStateDate = Convert.ToString(TextBoxOIGReportedToStateDate.Text, Nothing).Trim()
                objEmployeeSettingDataObject.StartDate = Convert.ToString(TextBoxStartDate.Text, Nothing).Trim()
                objEmployeeSettingDataObject.EndDate = Convert.ToString(TextBoxEndDate.Text, Nothing).Trim()


                'objEmployeeSettingDataObject.SantraxCDSPayrate = If((Not String.IsNullOrEmpty(TextBoxSantraxCDSPayrate.Text.Trim())),
                '                                        Convert.ToDecimal(objShared.GetReFormattedPayrate(TextBoxSantraxCDSPayrate.Text.Trim()), Nothing),
                '                                        objEmployeeSettingDataObject.SantraxCDSPayrate)

                'objEmployeeSettingDataObject.SantraxEmployeeId = Convert.ToString(TextBoxSantraxEmployeeId.Text, Nothing).Trim()
                'objEmployeeSettingDataObject.SantraxGPSPhone = objShared.GetReFormattedMobileNumber(Convert.ToString(TextBoxSantraxGPSPhone.Text, Nothing).Trim())
                'objEmployeeSettingDataObject.SantraxGPSPhone = Convert.ToString(TextBoxSantraxGPSPhone.Text, Nothing).Trim()

                'objEmployeeSettingDataObject.SantraxDiscipline = Convert.ToString(TextBoxSantraxDiscipline.Text, Nothing).Trim()

                objEmployeeSettingDataObject.VendorEmployeeId = TextBoxEVVVendorId.Text.Trim()
                objEmployeeSettingDataObject.EmployeeEVVId = TextBoxEmployeeEVVId.Text.Trim()

                Dim IsSaved As Boolean = False
                Dim objBLEmployeeSetting As New BLEmployeeSetting()

                Select Case Convert.ToBoolean(HiddenFieldIsNew.Value, Nothing)
                    Case True
                        objEmployeeSettingDataObject.UserId = Me.UserId
                        objEmployeeSettingDataObject.CompanyId = Me.CompanyId
                        Try
                            objBLEmployeeSetting.InsertEmployeeInfo(objShared.ConVisitel, objEmployeeSettingDataObject)
                            IsSaved = True
                        Catch ex As SqlException

                            If ex.Message.Equals("Duplicate Employee") Then
                                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DuplicateEmployeeMessage)
                            Else
                                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to Insert")
                            End If
                        End Try

                        If (IsSaved) Then
                            'TextBoxSearchBySocialSecurityNumber.Text = TextBoxSocialSecurityNumber.Text
                            DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(InsertMessage)
                        End If

                        Exit Select
                    Case False
                        objEmployeeSettingDataObject.EmployeeId = Convert.ToInt32(HiddenFieldEmployeeId.Value)
                        objEmployeeSettingDataObject.UpdateBy = Convert.ToString(Me.UserId)
                        objEmployeeSettingDataObject.UserId = Me.UserId
                        objEmployeeSettingDataObject.CompanyId = Me.CompanyId
                        Try
                            objBLEmployeeSetting.UpdateEmployeeInfo(objShared.ConVisitel, objEmployeeSettingDataObject)
                            IsSaved = True
                        Catch ex As SqlException

                            If ex.Message.Equals("Duplicate Employee") Then
                                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DuplicateEmployeeMessage)
                            Else
                                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to Update")
                            End If
                        End Try

                        If (IsSaved) Then
                            'TextBoxSearchBySocialSecurityNumber.Text = TextBoxSocialSecurityNumber.Text
                            DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(UpdateMessage)
                        End If
                        Exit Select
                End Select

                objBLEmployeeSetting = Nothing
                objEmployeeSettingDataObject = Nothing

                If (IsSaved) Then
                    'objShared.BindEmployeeDropDownList(DropDownListSearchByEmployee, CompanyId, False)
                    'If ((Not String.IsNullOrEmpty(HiddenFieldEmployeeId.Value)) And (Not HiddenFieldEmployeeId.Value.Equals(Convert.ToString(Int32.MinValue)))) Then
                    '    DropDownListSearchByEmployee.SelectedValue = HiddenFieldEmployeeId.Value
                    'End If

                    ClearHiddenFieldValue()
                End If
            End If
        End Sub

        Public Sub LoadEmployee()
            GetData()
            LoadSelectedEmployee()
        End Sub


        ''' <summary>
        ''' Retrieving Employee Information for the selected employee 
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub LoadSelectedEmployee()

            Dim objBLEmployeeSetting As New BLEmployeeSetting()
            Dim objEmployeeSettingDataObject As EmployeeSettingDataObject = Nothing
            objEmployeeSettingDataObject = objBLEmployeeSetting.SelectEmployeeInfo(objShared.ConVisitel, Me.CompanyId, Me.EmployeeId, Me.SocialSecurityNumber)
            objBLEmployeeSetting = Nothing

            DropDownListEmploymentStatus.SelectedIndex = DropDownListEmploymentStatus.Items.IndexOf(DropDownListEmploymentStatus.Items.FindByValue(
                                                                                            Convert.ToString(objEmployeeSettingDataObject.EmploymentStatusId)))

            DropDownListLicenseStatus.SelectedIndex = DropDownListLicenseStatus.Items.IndexOf(DropDownListLicenseStatus.Items.FindByValue(
                                                                                              Convert.ToString(objEmployeeSettingDataObject.LicenseStatus)))

            DropDownListClientGroup.SelectedIndex = DropDownListClientGroup.Items.IndexOf(DropDownListClientGroup.Items.FindByValue(
                                                                                          Convert.ToString(objEmployeeSettingDataObject.ClientGroupId)))

            DropDownListTitle.SelectedIndex = DropDownListTitle.Items.IndexOf(DropDownListTitle.Items.FindByValue(Convert.ToString(objEmployeeSettingDataObject.Title)))

            TextBoxTurboPASEmployeeId.Text = objEmployeeSettingDataObject.EmployeeId

            TextBoxOIGResult.Text = objEmployeeSettingDataObject.OIGResult

            TextBoxFirstName.Text = objEmployeeSettingDataObject.FirstName
            TextBoxMiddleNameInitial.Text = objEmployeeSettingDataObject.MiddleNameInitial
            TextBoxLastName.Text = objEmployeeSettingDataObject.LastName
            TextBoxAddress.Text = objEmployeeSettingDataObject.Address
            TextBoxApartmentNumber.Text = objEmployeeSettingDataObject.ApartmentNumber

            DropDownListCity.SelectedIndex = DropDownListCity.Items.IndexOf(DropDownListCity.Items.FindByValue(Convert.ToString(objEmployeeSettingDataObject.CityId)))
            DropDownListState.SelectedIndex = DropDownListState.Items.IndexOf(DropDownListState.Items.FindByValue(Convert.ToString(objEmployeeSettingDataObject.StateId)))

            TextBoxZip.Text = objEmployeeSettingDataObject.Zip
            TextBoxPhone.Text = objEmployeeSettingDataObject.Phone
            TextBoxAlternatePhone.Text = objEmployeeSettingDataObject.AlternatePhone

            TextBoxDateOfBirth.Text = objEmployeeSettingDataObject.DateOfBirth

            TextBoxSocialSecurityNumber.Text = objEmployeeSettingDataObject.SocialSecurityNumber

            DropDownListGender.SelectedIndex = DropDownListGender.Items.IndexOf(DropDownListGender.Items.FindByValue(Convert.ToString(objEmployeeSettingDataObject.Gender)))

            DropDownListMaritalStatus.SelectedIndex = DropDownListMaritalStatus.Items.IndexOf(DropDownListMaritalStatus.Items.FindByValue(
                                                          Convert.ToString(objEmployeeSettingDataObject.MaritalStatus)))

            TextBoxNumberOfVerifiedReference.Text = Convert.ToString(objEmployeeSettingDataObject.NumberOfVerifiedReference)
            TextBoxNumberOfDepartment.Text = Convert.ToString(objEmployeeSettingDataObject.NumberOfDepartment)
            'TextBoxPayrate.Text = If(Not objEmployeeSettingDataObject.Payrate.Equals(0), objShared.GetFormattedPayrate(objEmployeeSettingDataObject.Payrate), String.Empty)
            TextBoxPayrate.Text = If(Not objEmployeeSettingDataObject.Payrate.Equals(0), objEmployeeSettingDataObject.Payrate, String.Empty)

            DropDownListRehire.SelectedIndex = DropDownListRehire.Items.IndexOf(DropDownListRehire.Items.FindByValue(Convert.ToString(objEmployeeSettingDataObject.RehireYesNo)))

            TextBoxComments.Text = objEmployeeSettingDataObject.Comments
            CheckBoxMailCheck.Checked = If(Convert.ToString(objEmployeeSettingDataObject.MailCheck).Equals("1"), True, False)

            TextBoxApplicationDate.Text = objEmployeeSettingDataObject.ApplicationDate
            TextBoxReferenceVerificationDate.Text = objEmployeeSettingDataObject.ReferenceVerificationDate
            TextBoxHireDate.Text = objEmployeeSettingDataObject.HireDate
            TextBoxSignedJobDescriptionDate.Text = objEmployeeSettingDataObject.SignedJobDescriptionDate
            TextBoxOrientationDate.Text = objEmployeeSettingDataObject.OrientationDate
            TextBoxAssignedTaskEvaluationDate.Text = objEmployeeSettingDataObject.AssignedTaskEvaluationDate
            TextBoxCrimcheckDate.Text = objEmployeeSettingDataObject.CrimcheckDate
            TextBoxRegistryDate.Text = objEmployeeSettingDataObject.RegistryDate
            TextBoxLastEvaluationDate.Text = objEmployeeSettingDataObject.LastEvaluationDate
            TextBoxPolicyOrProcedureSettlementDate.Text = objEmployeeSettingDataObject.PolicyOrProcedureSettlementDate
            TextBoxOIGDate.Text = objEmployeeSettingDataObject.OIGDate
            TextBoxOIGReportedToStateDate.Text = objEmployeeSettingDataObject.OIGReportedToStateDate
            TextBoxStartDate.Text = objEmployeeSettingDataObject.StartDate
            TextBoxEndDate.Text = objEmployeeSettingDataObject.EndDate

            'TextBoxSantraxCDSPayrate.Text = objShared.GetFormattedPayrate(objEmployeeSettingDataObject.SantraxCDSPayrate)
            'TextBoxSantraxCDSPayrate.Text = objEmployeeSettingDataObject.SantraxCDSPayrate
            'TextBoxSantraxEmployeeId.Text = objEmployeeSettingDataObject.SantraxEmployeeId
            'TextBoxSantraxGPSPhone.Text = objEmployeeSettingDataObject.SantraxGPSPhone
            'TextBoxSantraxDiscipline.Text = objEmployeeSettingDataObject.SantraxDiscipline

            TextBoxPayrollId.Text = objEmployeeSettingDataObject.PayrollNumber
            TextBoxEVVVendorId.Text = objEmployeeSettingDataObject.VendorEmployeeId
            TextBoxEmployeeEVVId.Text = objEmployeeSettingDataObject.EmployeeEVVId

            TextBoxUpdateDate.Text = objEmployeeSettingDataObject.UpdateDate

            TextBoxUpdateBy.Text = objEmployeeSettingDataObject.UpdateBy

            objEmployeeSettingDataObject = Nothing

        End Sub

        ''' <summary>
        ''' Configuring Regular Expression Controls 
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub SetRegularExpressionSetting()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("Setup", ControlName & Convert.ToString(".resx"))

            Dim ErrorMessage As String = String.Empty, ErrorText As String = String.Empty

            ErrorMessage = Convert.ToString(ResourceTable("InvalidDateOfBirthMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Date Of Birth", ErrorMessage)

            ErrorText = Convert.ToString(ResourceTable("InvalidDateCommonMessage"), Nothing)
            ErrorText = If(String.IsNullOrEmpty(ErrorText), "Invalid", ErrorText)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorDateOfBirth, "TextBoxDateOfBirth", objShared.DateValidationExpression, ErrorMessage,
                                               ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidApplicationDateMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Application Date", ErrorMessage)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorApplicationDate, "TextBoxApplicationDate", objShared.DateValidationExpression, ErrorMessage,
                                               ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidReferenceVerificationDateMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Reference Verification Date", ErrorMessage)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorReferenceVerificationDate, "TextBoxReferenceVerificationDate",
                                                           objShared.DateValidationExpression, ErrorMessage, ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidHireDateMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Hire Date", ErrorMessage)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorHireDate, "TextBoxHireDate", objShared.DateValidationExpression, ErrorMessage,
                                               ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidSignedJobDescriptionDateMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Signed Job Description Date", ErrorMessage)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorSignedJobDescriptionDate, "TextBoxSignedJobDescriptionDate",
                                                           objShared.DateValidationExpression, ErrorMessage, ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidOrientationDateMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Orientation Date", ErrorMessage)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorOrientationDate, "TextBoxOrientationDate", objShared.DateValidationExpression, ErrorMessage,
                                               ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidAssignedTaskEvaluationDateMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Assigned Task Evaluation Date", ErrorMessage)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorAssignedTaskEvaluationDate, "TextBoxAssignedTaskEvaluationDate",
                                                           objShared.DateValidationExpression, ErrorMessage, ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidCrimcheckDateMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Crimcheck Date", ErrorMessage)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorCrimcheckDate, "TextBoxCrimcheckDate", objShared.DateValidationExpression, ErrorMessage,
                                               ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidRegistryDateMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Registry Date", ErrorMessage)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorRegistryDate, "TextBoxRegistryDate", objShared.DateValidationExpression, ErrorMessage,
                                               ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidLastEvaluationDateMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Last Evaluation Date", ErrorMessage)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorLastEvaluationDate, "TextBoxLastEvaluationDate", objShared.DateValidationExpression,
                                                           ErrorMessage, ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidPolicyOrProcedureSettlementDateMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Policy Or Procedure Settlement Date", ErrorMessage)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorPolicyOrProcedureSettlementDate, "TextBoxPolicyOrProcedureSettlementDate",
                                                           objShared.DateValidationExpression, ErrorMessage, ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidOIGDateMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid OIG Date", ErrorMessage)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorOIGDate, "TextBoxOIGDate", objShared.DateValidationExpression, ErrorMessage,
                                               ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidOIGReportedToStateDateMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid OIG Reported To State Date", ErrorMessage)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorOIGReportedToStateDate, "TextBoxOIGReportedToStateDate",
                                                           objShared.DateValidationExpression, ErrorMessage, ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidStartDateMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Start Date", ErrorMessage)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorStartDate, "TextBoxStartDate", objShared.DateValidationExpression, ErrorMessage,
                                               ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidEndDateMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid End Date", ErrorMessage)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorEndDate, "TextBoxEndDate", objShared.DateValidationExpression, ErrorMessage,
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

            ErrorMessage = Convert.ToString(ResourceTable("InvalidSantraxGPSPhoneMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Santrax GPS Phone", ErrorMessage)

            ErrorText = ErrorMessage

            'objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorSantraxGPSPhone, "TextBoxSantraxGPSPhone", objShared.PhoneValidationExpression, ErrorMessage,
            '                                   ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidSocialSecurityNumberMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Social Security Number", ErrorMessage)

            ErrorText = ErrorMessage

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorSocialSecurityNumber, "TextBoxSocialSecurityNumber",
                                                           objShared.SocialSecurityNumberValidationExpression, ErrorMessage, ErrorText, ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidPayrateMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Payrate", ErrorMessage)

            ErrorText = ErrorMessage

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorPayrate, "TextBoxPayrate", objShared.DecimalValueWithDollarSign, ErrorMessage, ErrorText,
                                                 ValidationEnable, ValidationGroup)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidCDSPayrateMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid CDS Payrate", ErrorMessage)

            ErrorText = ErrorMessage

            'objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorSantraxCDSPayrate, "TextBoxSantraxCDSPayrate", objShared.DecimalValueWithDollarSign,
            '                                               ErrorMessage, ErrorText, ValidationEnable, ValidationGroup)

            ResourceTable = Nothing


        End Sub

        ''' <summary>
        ''' Defining Text fields text length
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub DefineControlTextLength()

            objShared.SetControlTextLength(TextBoxFirstName, 20)
            objShared.SetControlTextLength(TextBoxPayrate, 20)

            objShared.SetControlTextLength(TextBoxMiddleNameInitial, 1)
            objShared.SetControlTextLength(TextBoxApartmentNumber, 8)

            objShared.SetControlTextLength(TextBoxLastName, 25)

            objShared.SetControlTextLength(TextBoxAddress, 100)

            objShared.SetControlTextLength(TextBoxZip, 10)
            objShared.SetControlTextLength(TextBoxNumberOfVerifiedReference, 10)

            objShared.SetControlTextLength(TextBoxPhone, 14)
            objShared.SetControlTextLength(TextBoxAlternatePhone, 14)
            'objShared.SetControlTextLength(TextBoxSantraxGPSPhone, 14)

            objShared.SetControlTextLength(TextBoxComments, 255)

            objShared.SetControlTextLength(TextBoxSocialSecurityNumber, 11)

            objShared.SetControlTextLength(TextBoxApplicationDate, 16)
            objShared.SetControlTextLength(TextBoxReferenceVerificationDate, 16)
            objShared.SetControlTextLength(TextBoxHireDate, 16)
            objShared.SetControlTextLength(TextBoxSignedJobDescriptionDate, 16)
            objShared.SetControlTextLength(TextBoxOrientationDate, 16)
            objShared.SetControlTextLength(TextBoxAssignedTaskEvaluationDate, 16)
            objShared.SetControlTextLength(TextBoxCrimcheckDate, 16)
            objShared.SetControlTextLength(TextBoxRegistryDate, 16)
            objShared.SetControlTextLength(TextBoxLastEvaluationDate, 16)
            objShared.SetControlTextLength(TextBoxPolicyOrProcedureSettlementDate, 16)
            objShared.SetControlTextLength(TextBoxOIGDate, 16)
            objShared.SetControlTextLength(TextBoxOIGReportedToStateDate, 16)
            objShared.SetControlTextLength(TextBoxStartDate, 16)
            objShared.SetControlTextLength(TextBoxEndDate, 16)
            objShared.SetControlTextLength(TextBoxDateOfBirth, 16)

            'objShared.SetControlTextLength(TextBoxSantraxCDSPayrate, 50)
            'objShared.SetControlTextLength(TextBoxSantraxDiscipline, 50)

            'objShared.SetControlTextLength(TextBoxSantraxEmployeeId, 9)

            'objShared.SetControlTextLength(TextBoxSearchBySocialSecurityNumber, 11)

        End Sub

        ''' <summary>
        ''' Filling out group of date fields with Application date value
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub ButtonAutoFill_Click(sender As Object, e As EventArgs)

            If (Not String.IsNullOrEmpty(TextBoxApplicationDate.Text)) Then

                TextBoxReferenceVerificationDate.Text = objShared.InlineAssignHelper(TextBoxHireDate.Text,
                                                        objShared.InlineAssignHelper(TextBoxSignedJobDescriptionDate.Text,
                                                        objShared.InlineAssignHelper(TextBoxOrientationDate.Text,
                                                        objShared.InlineAssignHelper(TextBoxAssignedTaskEvaluationDate.Text,
                                                        objShared.InlineAssignHelper(TextBoxCrimcheckDate.Text,
                                                        objShared.InlineAssignHelper(TextBoxRegistryDate.Text,
                                                        objShared.InlineAssignHelper(TextBoxLastEvaluationDate.Text,
                                                        objShared.InlineAssignHelper(TextBoxPolicyOrProcedureSettlementDate.Text,
                                                        objShared.InlineAssignHelper(TextBoxStartDate.Text, objShared.InlineAssignHelper(TextBoxOIGDate.Text,
                                                        objShared.InlineAssignHelper(TextBoxOIGReportedToStateDate.Text,
                                                                   TextBoxApplicationDate.Text)))))))))))

            End If

        End Sub

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("EmployeeInfo", ControlName & Convert.ToString(".resx"))

            LabelEmploymentStatus.Text = Convert.ToString(ResourceTable("LabelEmploymentStatus"), Nothing)
            LabelEmploymentStatus.Text = If(String.IsNullOrEmpty(LabelEmploymentStatus.Text), "Employment Status", LabelEmploymentStatus.Text)

            LabelLicenseStatus.Text = Convert.ToString(ResourceTable("LabelLicenseStatus"), Nothing)
            LabelLicenseStatus.Text = If(String.IsNullOrEmpty(LabelLicenseStatus.Text), "License Status", LabelLicenseStatus.Text)

            LabelClientGroup.Text = Convert.ToString(ResourceTable("LabelClientGroup"), Nothing)
            LabelClientGroup.Text = If(String.IsNullOrEmpty(LabelClientGroup.Text), "Group:", LabelClientGroup.Text)

            LabelOIGResult.Text = Convert.ToString(ResourceTable("LabelOIGResult"), Nothing)
            LabelOIGResult.Text = If(String.IsNullOrEmpty(LabelOIGResult.Text), "OIG Result:", LabelOIGResult.Text)

            LabelEmployeeDetailInfo.Text = Convert.ToString(ResourceTable("LabelEmployeeDetailInfo"), Nothing)
            LabelEmployeeDetailInfo.Text = If(String.IsNullOrEmpty(LabelEmployeeDetailInfo.Text), "Employee Detail Info", LabelEmployeeDetailInfo.Text)

            LabelTitle.Text = Convert.ToString(ResourceTable("LabelTitle"), Nothing)
            LabelTitle.Text = If(String.IsNullOrEmpty(LabelTitle.Text), "Title", LabelTitle.Text)

            LabelTurboPASEmployeeId.Text = Convert.ToString(ResourceTable("LabelTurboPASEmployeeId"), Nothing)
            LabelTurboPASEmployeeId.Text = If(String.IsNullOrEmpty(LabelTurboPASEmployeeId.Text), "TurboPAS Employee ID:", LabelTurboPASEmployeeId.Text)

            LabelPayrollId.Text = Convert.ToString(ResourceTable("LabelPayrollId"), Nothing)
            LabelPayrollId.Text = If(String.IsNullOrEmpty(LabelPayrollId.Text), "Payroll ID:", LabelPayrollId.Text)

            TextBoxTurboPASEmployeeId.Text = "(New)"

            LabelFirstName.Text = Convert.ToString(ResourceTable("LabelFirstName"), Nothing)
            LabelFirstName.Text = If(String.IsNullOrEmpty(LabelFirstName.Text), "First Name", LabelFirstName.Text)

            LabelMiddleNameInitial.Text = Convert.ToString(ResourceTable("LabelMiddleNameInitial"), Nothing)
            LabelMiddleNameInitial.Text = If(String.IsNullOrEmpty(LabelMiddleNameInitial.Text), "M.I", LabelMiddleNameInitial.Text)

            LabelLastName.Text = Convert.ToString(ResourceTable("LabelLastName"), Nothing)
            LabelLastName.Text = If(String.IsNullOrEmpty(LabelLastName.Text), "Last Name", LabelLastName.Text)

            LabelAddress.Text = Convert.ToString(ResourceTable("LabelAddress"), Nothing)
            LabelAddress.Text = If(String.IsNullOrEmpty(LabelAddress.Text), "Address", LabelAddress.Text)

            LabelApartmentNumber.Text = Convert.ToString(ResourceTable("LabelApartmentNumber"), Nothing)
            LabelApartmentNumber.Text = If(String.IsNullOrEmpty(LabelApartmentNumber.Text), "Apt#:", LabelApartmentNumber.Text)

            LabelCity.Text = Convert.ToString(ResourceTable("LabelCity"), Nothing)
            LabelCity.Text = If(String.IsNullOrEmpty(LabelCity.Text), "City", LabelCity.Text)

            LabelState.Text = Convert.ToString(ResourceTable("LabelState"), Nothing)
            LabelState.Text = If(String.IsNullOrEmpty(LabelState.Text), "State", LabelState.Text)

            LabelZip.Text = Convert.ToString(ResourceTable("LabelZip"), Nothing)
            LabelZip.Text = If(String.IsNullOrEmpty(LabelZip.Text), "Zip", LabelZip.Text)

            LabelPhone.Text = Convert.ToString(ResourceTable("LabelPhone"), Nothing)
            LabelPhone.Text = If(String.IsNullOrEmpty(LabelPhone.Text), "Mobile Phone", LabelPhone.Text)

            LabelAlternatePhone.Text = Convert.ToString(ResourceTable("LabelAlternatePhone"), Nothing)
            LabelAlternatePhone.Text = If(String.IsNullOrEmpty(LabelAlternatePhone.Text), "Other Phone", LabelAlternatePhone.Text)

            LabelDateOfBirth.Text = Convert.ToString(ResourceTable("LabelDateOfBirth"), Nothing)
            LabelDateOfBirth.Text = If(String.IsNullOrEmpty(LabelDateOfBirth.Text), "DOB:", LabelDateOfBirth.Text)

            LabelAge.Text = Convert.ToString(ResourceTable("LabelAge"), Nothing)
            LabelAge.Text = If(String.IsNullOrEmpty(LabelAge.Text), "Age:", LabelAge.Text)

            LabelSocialSecurityNumber.Text = Convert.ToString(ResourceTable("LabelSocialSecurityNumber"), Nothing)
            LabelSocialSecurityNumber.Text = If(String.IsNullOrEmpty(LabelSocialSecurityNumber.Text), "SS#:", LabelSocialSecurityNumber.Text)

            LabelGender.Text = Convert.ToString(ResourceTable("LabelGender"), Nothing)
            LabelGender.Text = If(String.IsNullOrEmpty(LabelGender.Text), "Gender:", LabelGender.Text)

            LabelMaritalStatus.Text = Convert.ToString(ResourceTable("LabelMaritalStatus"), Nothing)
            LabelMaritalStatus.Text = If(String.IsNullOrEmpty(LabelMaritalStatus.Text), "Marital Status:", LabelMaritalStatus.Text)

            LabelNumberOfDepartment.Text = Convert.ToString(ResourceTable("LabelNumberOfDepartment"), Nothing)
            LabelNumberOfDepartment.Text = If(String.IsNullOrEmpty(LabelNumberOfDepartment.Text), "NbrOfDepndts:", LabelNumberOfDepartment.Text)

            LabelNumberOfVerifiedReference.Text = Convert.ToString(ResourceTable("LabelNumberOfVerifiedReference"), Nothing)
            LabelNumberOfVerifiedReference.Text = If(String.IsNullOrEmpty(LabelNumberOfVerifiedReference.Text), "No. of Verified Ref:", LabelNumberOfVerifiedReference.Text)

            LabelPayrate.Text = Convert.ToString(ResourceTable("LabelPayrate"), Nothing)
            LabelPayrate.Text = If(String.IsNullOrEmpty(LabelPayrate.Text), "Payrate:", LabelPayrate.Text)

            LabelRehire.Text = Convert.ToString(ResourceTable("LabelRehire"), Nothing)
            LabelRehire.Text = If(String.IsNullOrEmpty(LabelRehire.Text), "Rehireable Y/N:", LabelRehire.Text)

            LabelComments.Text = Convert.ToString(ResourceTable("LabelComments"), Nothing)
            LabelComments.Text = If(String.IsNullOrEmpty(LabelComments.Text), "Comments:", LabelComments.Text)

            LabelMailCheck.Text = Convert.ToString(ResourceTable("LabelMailCheck"), Nothing)
            LabelMailCheck.Text = If(String.IsNullOrEmpty(LabelMailCheck.Text), "Mail Check:", LabelMailCheck.Text)

            LabelUpdateDate.Text = Convert.ToString(ResourceTable("LabelUpdateDate"), Nothing)
            LabelUpdateDate.Text = If(String.IsNullOrEmpty(LabelUpdateDate.Text), "Update Date:", LabelUpdateDate.Text)

            LabelUpdateBy.Text = Convert.ToString(ResourceTable("LabelUpdateBy"), Nothing)
            LabelUpdateBy.Text = If(String.IsNullOrEmpty(LabelUpdateBy.Text), "Update By:", LabelUpdateBy.Text)

            LabelDates.Text = Convert.ToString(ResourceTable("LabelDates"), Nothing)
            LabelDates.Text = If(String.IsNullOrEmpty(LabelDates.Text), "Dates:", LabelDates.Text)

            LabelApplicationDate.Text = Convert.ToString(ResourceTable("LabelApplicationDate"), Nothing)
            LabelApplicationDate.Text = If(String.IsNullOrEmpty(LabelApplicationDate.Text), "Application:", LabelApplicationDate.Text)

            LabelReferenceVerificationDate.Text = Convert.ToString(ResourceTable("LabelReferenceVerificationDate"), Nothing)
            LabelReferenceVerificationDate.Text = If(String.IsNullOrEmpty(LabelReferenceVerificationDate.Text), "Ref Verification:", LabelReferenceVerificationDate.Text)

            LabelHireDate.Text = Convert.ToString(ResourceTable("LabelHireDate"), Nothing)
            LabelHireDate.Text = If(String.IsNullOrEmpty(LabelHireDate.Text), "Hire:", LabelHireDate.Text)

            LabelSignedJobDescriptionDate.Text = Convert.ToString(ResourceTable("LabelSignedJobDescriptionDate"), Nothing)
            LabelSignedJobDescriptionDate.Text = If(String.IsNullOrEmpty(LabelSignedJobDescriptionDate.Text), "Signed Job Desc:", LabelSignedJobDescriptionDate.Text)

            LabelOrientationDate.Text = Convert.ToString(ResourceTable("LabelOrientationDate"), Nothing)
            LabelOrientationDate.Text = If(String.IsNullOrEmpty(LabelOrientationDate.Text), "Orientation:", LabelOrientationDate.Text)

            LabelAssignedTaskEvaluationDate.Text = Convert.ToString(ResourceTable("LabelAssignedTaskEvaluationDate"), Nothing)
            LabelAssignedTaskEvaluationDate.Text = If(String.IsNullOrEmpty(LabelAssignedTaskEvaluationDate.Text), "Assigned Task Eval:", LabelAssignedTaskEvaluationDate.Text)

            HyperLinkCrimcheckDate.Text = Convert.ToString(ResourceTable("HyperLinkCrimcheckDate"), Nothing)
            HyperLinkCrimcheckDate.Text = If(String.IsNullOrEmpty(HyperLinkCrimcheckDate.Text), "Crimcheck:", HyperLinkCrimcheckDate.Text)

            HyperLinkMisConductRequest.Text = Convert.ToString(ResourceTable("HyperLinkMisConductRequest"), Nothing)
            HyperLinkMisConductRequest.Text = If(String.IsNullOrEmpty(HyperLinkMisConductRequest.Text), "Misconduct Req:", HyperLinkMisConductRequest.Text)

            LabelLastEvaluationDate.Text = Convert.ToString(ResourceTable("LabelLastEvaluationDate"), Nothing)
            LabelLastEvaluationDate.Text = If(String.IsNullOrEmpty(LabelLastEvaluationDate.Text), "Last Evaluation:", LabelLastEvaluationDate.Text)

            LabelPolicyOrProcedureSettlementDate.Text = Convert.ToString(ResourceTable("LabelPolicyOrProcedureSettlementDate"), Nothing)
            LabelPolicyOrProcedureSettlementDate.Text = If(String.IsNullOrEmpty(LabelPolicyOrProcedureSettlementDate.Text), "Policy/Procedure Stmt:",
                                                           LabelPolicyOrProcedureSettlementDate.Text)

            LabelOIGDate.Text = Convert.ToString(ResourceTable("LabelOIGDate"), Nothing)
            LabelOIGDate.Text = If(String.IsNullOrEmpty(LabelOIGDate.Text), "OIG Date:", LabelOIGDate.Text)

            LabelOIGReportedToStateDate.Text = Convert.ToString(ResourceTable("LabelOIGReportedToStateDate"), Nothing)
            LabelOIGReportedToStateDate.Text = If(String.IsNullOrEmpty(LabelOIGReportedToStateDate.Text), "OIG Reported to State:", LabelOIGReportedToStateDate.Text)

            LabelStartDate.Text = Convert.ToString(ResourceTable("LabelStartDate"), Nothing)
            LabelStartDate.Text = If(String.IsNullOrEmpty(LabelStartDate.Text), "Start:", LabelStartDate.Text)

            LabelEndDate.Text = Convert.ToString(ResourceTable("LabelEndDate"), Nothing)
            LabelEndDate.Text = If(String.IsNullOrEmpty(LabelEndDate.Text), "End:", LabelEndDate.Text)

            DateFieldToolTip = Convert.ToString(ResourceTable("DateFieldToolTip"), Nothing)
            DateFieldToolTip = If(String.IsNullOrEmpty(DateFieldToolTip), "Example: August 21, 2011", DateFieldToolTip)

            'LabelCM2000.Text = Convert.ToString(ResourceTable("LabelCM2000"), Nothing)
            'LabelCM2000.Text = If(String.IsNullOrEmpty(LabelCM2000.Text), "CM2000", LabelCM2000.Text)

            'LabelCareId.Text = Convert.ToString(ResourceTable("LabelCareId"), Nothing)
            'LabelCareId.Text = If(String.IsNullOrEmpty(LabelCareId.Text), "Care Id:", LabelCareId.Text)

            'LabelCMPayrollId.Text = Convert.ToString(ResourceTable("LabelCMPayrollId"), Nothing)
            'LabelCMPayrollId.Text = If(String.IsNullOrEmpty(LabelCMPayrollId.Text), "Payroll Id:", LabelCMPayrollId.Text)

            'LabelSantrax.Text = Convert.ToString(ResourceTable("LabelSantrax"), Nothing)
            'LabelSantrax.Text = If(String.IsNullOrEmpty(LabelSantrax.Text), "Santrax", LabelSantrax.Text)

            'LabelSantraxCDSPayrate.Text = Convert.ToString(ResourceTable("LabelSantraxCDSPayrate"), Nothing)
            'LabelSantraxCDSPayrate.Text = If(String.IsNullOrEmpty(LabelSantraxCDSPayrate.Text), "CDS Payrate:", LabelSantraxCDSPayrate.Text)

            'LabelSantraxEmployeeId.Text = Convert.ToString(ResourceTable("LabelSantraxEmployeeId"), Nothing)
            'LabelSantraxEmployeeId.Text = If(String.IsNullOrEmpty(LabelSantraxEmployeeId.Text), "Santrax Id:", LabelSantraxEmployeeId.Text)

            'LabelSantraxGPSPhone.Text = Convert.ToString(ResourceTable("LabelSantraxGPSPhone"), Nothing)
            'LabelSantraxGPSPhone.Text = If(String.IsNullOrEmpty(LabelSantraxGPSPhone.Text), "GPS Phone:", LabelSantraxGPSPhone.Text)

            'LabelSantraxDiscipline.Text = Convert.ToString(ResourceTable("LabelSantraxDiscipline"), Nothing)
            'LabelSantraxDiscipline.Text = If(String.IsNullOrEmpty(LabelSantraxDiscipline.Text), "Discipline:", LabelSantraxDiscipline.Text)

            LabelEVV.Text = Convert.ToString(ResourceTable("LabelEVV"), Nothing)
            LabelEVV.Text = If(String.IsNullOrEmpty(LabelEVV.Text), "EVV", LabelEVV.Text)

            LabelEVVVendorId.Text = Convert.ToString(ResourceTable("LabelEVVVendorId"), Nothing)
            LabelEVVVendorId.Text = If(String.IsNullOrEmpty(LabelEVVVendorId.Text), "EVV Vendor Id:", LabelEVVVendorId.Text)

            LabelEmployeeEVVId.Text = Convert.ToString(ResourceTable("LabelEmployeeEVVId"), Nothing)
            LabelEmployeeEVVId.Text = If(String.IsNullOrEmpty(LabelEmployeeEVVId.Text), "Employee EVV Id:", LabelEmployeeEVVId.Text)

            ButtonAutoFill.Text = Convert.ToString(ResourceTable("ButtonAutoFill"), Nothing)
            ButtonAutoFill.Text = If(String.IsNullOrEmpty(ButtonAutoFill.Text), "Autofill", ButtonAutoFill.Text)

            ButtonSave.Text = Convert.ToString(ResourceTable("ButtonSave"), Nothing)
            ButtonSave.Text = If(String.IsNullOrEmpty(ButtonSave.Text), "Save", ButtonSave.Text)

            ButtonClear.Text = Convert.ToString(ResourceTable("ButtonClear"), Nothing)
            ButtonClear.Text = If(String.IsNullOrEmpty(ButtonClear.Text), "Clear", ButtonClear.Text)

            ButtonDelete.Text = Convert.ToString(ResourceTable("ButtonDelete"), Nothing)
            ButtonDelete.Text = If(String.IsNullOrEmpty(ButtonDelete.Text), "Delete", ButtonDelete.Text)

            EditText = Convert.ToString(ResourceTable("EditText"), Nothing)
            EditText = If(String.IsNullOrEmpty(EditText), "Edit", EditText)

            DeleteText = Convert.ToString(ResourceTable("DeleteText"), Nothing)
            DeleteText = If(String.IsNullOrEmpty(DeleteText), "Delete", DeleteText)

            InsertMessage = Convert.ToString(ResourceTable("InsertMessage"), Nothing)
            InsertMessage = If(String.IsNullOrEmpty(InsertMessage), "Inserted Successfully", InsertMessage)

            UpdateMessage = Convert.ToString(ResourceTable("UpdateMessage"), Nothing)
            UpdateMessage = If(String.IsNullOrEmpty(UpdateMessage), "Updated Successfully", UpdateMessage)

            DeleteMessage = Convert.ToString(ResourceTable("DeleteMessage"), Nothing)
            DeleteMessage = If(String.IsNullOrEmpty(DeleteMessage), "Deleted Successfully", DeleteMessage)

            ValidationGroup = Convert.ToString(ResourceTable("ValidationGroup"), Nothing)
            ValidationGroup = If(String.IsNullOrEmpty(ValidationGroup), "Employee", ValidationGroup)

            AutofillConfirmationMessage = Convert.ToString(ResourceTable("AutofillConfirmationMessage"), Nothing)
            AutofillConfirmationMessage = If(String.IsNullOrEmpty(AutofillConfirmationMessage), "This action will set date fields to the Application Date. " _
                                              & "Are you sure you want to do this?", AutofillConfirmationMessage)

            DeleteConfirmationMessage = Convert.ToString(ResourceTable("DeleteConfirmationMessage"), Nothing)
            DeleteConfirmationMessage = If(String.IsNullOrEmpty(DeleteConfirmationMessage), "Are you sure to delete this record?", DeleteConfirmationMessage)

            DuplicateEmployeeMessage = Convert.ToString(ResourceTable("DuplicateEmployeeMessage"), Nothing)
            DuplicateEmployeeMessage = If(String.IsNullOrEmpty(DuplicateEmployeeMessage), "This employee info already exists in the system.", DuplicateEmployeeMessage)

            Dim ResultOut As Boolean

            ValidationEnable = If((Boolean.TryParse(ResourceTable("ValidationEnable"), ResultOut)), ResultOut, True)

            ResourceTable = Nothing

        End Sub

        ''' <summary>
        ''' Clear Controls Values
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ClearControl()

            DropDownListTitle.SelectedIndex = objShared.InlineAssignHelper(DropDownListCity.SelectedIndex, objShared.InlineAssignHelper(DropDownListState.SelectedIndex,
                                              objShared.InlineAssignHelper(DropDownListGender.SelectedIndex, objShared.InlineAssignHelper(DropDownListMaritalStatus.SelectedIndex,
                                              objShared.InlineAssignHelper(DropDownListRehire.SelectedIndex, objShared.InlineAssignHelper(DropDownListLicenseStatus.SelectedIndex,
                                              objShared.InlineAssignHelper(DropDownListClientGroup.SelectedIndex,
                                              objShared.InlineAssignHelper(DropDownListEmploymentStatus.SelectedIndex, 0))))))))

            TextBoxFirstName.Text = objShared.InlineAssignHelper(TextBoxMiddleNameInitial.Text, objShared.InlineAssignHelper(TextBoxLastName.Text,
                                    objShared.InlineAssignHelper(TextBoxAddress.Text, objShared.InlineAssignHelper(TextBoxApartmentNumber.Text,
                                    objShared.InlineAssignHelper(TextBoxZip.Text, objShared.InlineAssignHelper(TextBoxPhone.Text,
                                    objShared.InlineAssignHelper(TextBoxAlternatePhone.Text, objShared.InlineAssignHelper(TextBoxDateOfBirth.Text,
                                    objShared.InlineAssignHelper(TextBoxSocialSecurityNumber.Text, objShared.InlineAssignHelper(TextBoxNumberOfVerifiedReference.Text,
                                    objShared.InlineAssignHelper(TextBoxPayrate.Text, objShared.InlineAssignHelper(TextBoxComments.Text,
                                    objShared.InlineAssignHelper(TextBoxApplicationDate.Text, objShared.InlineAssignHelper(TextBoxReferenceVerificationDate.Text,
                                    objShared.InlineAssignHelper(TextBoxHireDate.Text, objShared.InlineAssignHelper(TextBoxSignedJobDescriptionDate.Text,
                                    objShared.InlineAssignHelper(TextBoxOrientationDate.Text, objShared.InlineAssignHelper(TextBoxAssignedTaskEvaluationDate.Text,
                                    objShared.InlineAssignHelper(TextBoxCrimcheckDate.Text, objShared.InlineAssignHelper(TextBoxRegistryDate.Text,
                                    objShared.InlineAssignHelper(TextBoxLastEvaluationDate.Text, objShared.InlineAssignHelper(TextBoxPolicyOrProcedureSettlementDate.Text,
                                    objShared.InlineAssignHelper(TextBoxOIGDate.Text, objShared.InlineAssignHelper(TextBoxOIGReportedToStateDate.Text,
                                    objShared.InlineAssignHelper(TextBoxStartDate.Text, objShared.InlineAssignHelper(TextBoxEndDate.Text,
                                    objShared.InlineAssignHelper(TextBoxNumberOfDepartment.Text,
                                    objShared.InlineAssignHelper(TextBoxUpdateDate.Text, objShared.InlineAssignHelper(TextBoxUpdateBy.Text,
                                    objShared.InlineAssignHelper(TextBoxOIGResult.Text, String.Empty))))))))))))))))))))))))))))))

            'TextBoxSantraxCDSPayrate.Text = objShared.InlineAssignHelper(TextBoxSantraxEmployeeId.Text, objShared.InlineAssignHelper(TextBoxSantraxGPSPhone.Text,
            '                                objShared.InlineAssignHelper(TextBoxSantraxDiscipline.Text, String.Empty)))

            TextBoxPayrollId.Text = objShared.InlineAssignHelper(TextBoxEVVVendorId.Text, objShared.InlineAssignHelper(TextBoxEmployeeEVVId.Text, String.Empty))

            CheckBoxMailCheck.Checked = False

            ClearHiddenFieldValue()

            ButtonDelete.Enabled = If(HiddenFieldIsNew.Value.Equals(Convert.ToString(True, Nothing)), False, True)

        End Sub

        ''' <summary>
        ''' This is for retrieving data
        ''' </summary>
        Public Sub GetData()
            objShared.BindClientGroupDropDownList(DropDownListClientGroup, Me.CompanyId)
            BindLicenseStatusDropDownList()
            BindTitleDropDownList()
            objShared.BindCityDropDownList(DropDownListCity, Me.CompanyId)
            objShared.BindStateDropDownList(DropDownListState, Me.CompanyId)
            objShared.BindMaritalStatusDropDownList(DropDownListMaritalStatus, Me.CompanyId)
            objShared.BindGenderDropDownList(DropDownListGender)
            BindRehireableDropDownList()
            BindEmploymentStatusDropDownList()
        End Sub

        ''' <summary>
        ''' Binding Employment Status Drop Down List
        ''' </summary>
        Private Sub BindEmploymentStatusDropDownList()

            SqlDataSourceDropDownListEmploymentStatus.ProviderName = "System.Data.SqlClient"
            SqlDataSourceDropDownListEmploymentStatus.ConnectionString = objShared.VisitelConnectionString
            SqlDataSourceDropDownListEmploymentStatus.SelectCommandType = SqlDataSourceCommandType.StoredProcedure
            SqlDataSourceDropDownListEmploymentStatus.SelectCommand = "SelectEmploymentStatus"
            SqlDataSourceDropDownListEmploymentStatus.DataBind()

            DropDownListEmploymentStatus.DataSourceID = "SqlDataSourceDropDownListEmploymentStatus"
            DropDownListEmploymentStatus.DataTextField = "Name"
            DropDownListEmploymentStatus.DataValueField = "IdNumber"
            DropDownListEmploymentStatus.DataBind()

        End Sub

        ''' <summary>
        ''' Binding License Status Drop Down List
        ''' </summary>
        Private Sub BindLicenseStatusDropDownList()

            SqlDataSourceDropDownListLicenseStatus.ProviderName = "System.Data.SqlClient"
            SqlDataSourceDropDownListLicenseStatus.ConnectionString = objShared.VisitelConnectionString
            SqlDataSourceDropDownListLicenseStatus.SelectCommandType = SqlDataSourceCommandType.StoredProcedure
            SqlDataSourceDropDownListLicenseStatus.SelectCommand = "SelectLicenseStatus"
            SqlDataSourceDropDownListLicenseStatus.DataBind()

            DropDownListLicenseStatus.DataSourceID = "SqlDataSourceDropDownListLicenseStatus"
            DropDownListLicenseStatus.DataTextField = "name"
            DropDownListLicenseStatus.DataValueField = "id_number"
            DropDownListLicenseStatus.DataBind()

        End Sub

        ''' <summary>
        ''' Binding Title Drop Down List
        ''' </summary>
        Private Sub BindTitleDropDownList()

            Dim blEmployee As New BLEmployee
            blEmployee.GetTitles(objShared.VisitelConnectionString, SqlDataSourceDropDownListTitle)
            blEmployee = Nothing

            DropDownListTitle.DataSourceID = "SqlDataSourceDropDownListTitle"
            DropDownListTitle.DataTextField = "name"
            DropDownListTitle.DataValueField = "id_number"
            DropDownListTitle.DataBind()

        End Sub

        ''' <summary>
        ''' Binding Rehireable Drop Down List
        ''' </summary>
        Private Sub BindRehireableDropDownList()
            DropDownListRehire.DataSource = EnumDataObject.Enumeration.GetAll(Of EnumDataObject.REHIRE)()
            DropDownListRehire.DataTextField = "Value"
            DropDownListRehire.DataValueField = "Key"
            DropDownListRehire.DataBind()

            'DropDownListRehire.Items.Insert(0, New ListItem("Please Select...", "-1"))
        End Sub

        ''' <summary>
        ''' Setting Hidden Fields Controls Value from parent page
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub SetHiddenFieldValue()
            HiddenFieldIsNew.Value = Convert.ToString(False, Nothing)
            HiddenFieldEmployeeId.Value = Me.EmployeeId
            ButtonDelete.Enabled = If(HiddenFieldIsNew.Value.Equals(Convert.ToString(True, Nothing)), False, True)
        End Sub

        ''' <summary>
        ''' Clearing Hidden Fields Control Values
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub ClearHiddenFieldValue()
            HiddenFieldIsNew.Value = Convert.ToString(True, Nothing)
            HiddenFieldEmployeeId.Value = Convert.ToString(Int32.MinValue)
        End Sub

        Public Sub LoadJavaScript()

            Dim scriptBlock As String = String.Empty
            scriptBlock = "<script type='text/javascript'> " _
                            & " var CalendarDateFormat='" & objShared.CalendarDateFormat & "'; " _
                            & " var DeleteTargetButton ='ButtonDelete'; " _
                            & " var DeleteDialogHeader ='Employee Information'; " _
                            & " var DeleteDialogConfirmMsg ='" & DeleteConfirmationMessage & "'; " _
                            & " var CustomTargetButton ='ButtonAutoFill'; " _
                            & " var CustomDialogHeader ='Employee Information Auto Fill'; " _
                            & " var CustomDialogConfirmMsg ='" & AutofillConfirmationMessage & "'; " _
                            & " var LoaderImagePath ='" & objShared.GetLoaderImagePath & "'; " _
                            & " var prm =''; " _
                            & " jQuery(document).ready(function () {" _
                            & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                            & "     prm.add_beginRequest(SetButtonActionProgress); " _
                            & "     prm.add_endRequest(EndRequest); " _
                            & "     DataDelete();" _
                            & "     AutoFill();" _
                            & "     InputMasking();" _
                            & "     SetUnitRateMask();" _
                            & "     AgeCalculate();" _
                            & "     prm.add_endRequest(DataDelete); " _
                            & "     prm.add_endRequest(AutoFill); " _
                            & "     prm.add_endRequest(InputMasking); " _
                            & "     prm.add_endRequest(SetUnitRateMask); " _
                            & "     prm.add_endRequest(AgeCalculate); " _
                            & "}); " _
                    & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

            LoadJS("JavaScript/EmployeeInfo/" & ControlName & ".js")

        End Sub

    End Class
End Namespace