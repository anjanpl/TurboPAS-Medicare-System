Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports VisitelBusiness.VisitelBusiness.DataObject

Namespace Visitel.UserControl.ClientInfo

    Public Class BillingInfoControl
        Inherits BaseUserControl

        Private ControlName As String, InvalidUnitRateMessage As String, ValidationGroup As String

        Private ValidationEnable As Boolean

        Private objShared As SharedWebControls

        Protected Sub Page_Init(sender As Object, e As EventArgs)

            ControlName = "BillingInfoControl"
            objShared = New SharedWebControls()
            GetCaptionFromResource()

            objShared.ConnectionOpen()
            InitializeControl()

        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)
            If (Not IsPostBack) Then
                GetData()
            End If
        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadCss("ClientInfo/" & ControlName)
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing
        End Sub

        ''' <summary>
        ''' This is for control events regristering and instantiate object globally
        ''' </summary>
        Private Sub InitializeControl()
            SetControlToolTip()
            SetRegularExpressionSetting()
            DefineControlTextLength()

            HyperLinkTexMedConnect.NavigateUrl = Convert.ToString(ConfigurationManager.AppSettings("TexMedConnectLink"), Nothing)

        End Sub

        Private Sub SetRegularExpressionSetting()
            If (Not RegularExpressionValidatorUnitRate Is Nothing) Then
                objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorUnitRate, "TextBoxUnitRate", objShared.DecimalValueWithDollarSign,
                                                 InvalidUnitRateMessage, InvalidUnitRateMessage, ValidationEnable, ValidationGroup)
            End If
        End Sub

        ''' <summary>
        ''' This is for validating data in server side before submitting the form
        ''' </summary>
        ''' <returns></returns>
        Public Function ValidateData() As Boolean
            If ((Not TextBoxUnitRate Is Nothing) And (Not RegularExpressionValidatorUnitRate Is Nothing)) Then
                If ((Not String.IsNullOrEmpty(TextBoxUnitRate.Text.Trim()))) And (Not objShared.ValidatePayrate(TextBoxUnitRate.Text.Trim())) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorUnitRate.ErrorMessage)
                    Return False
                End If
            End If
            Return True
        End Function

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("ClientInfo", ControlName & Convert.ToString(".resx"))

            '#Region "Client Case Billing Information"

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

            InvalidUnitRateMessage = Convert.ToString(ResourceTable("InvalidUnitRateMessage"), Nothing).Trim()
            InvalidUnitRateMessage = If(String.IsNullOrEmpty(InvalidUnitRateMessage), "Invalid Unit Rate", InvalidUnitRateMessage)

            ValidationGroup = Convert.ToString(ResourceTable("ValidationGroup"), Nothing).Trim()
            ValidationGroup = If(String.IsNullOrEmpty(ValidationGroup), "Client", ValidationGroup)

            Dim ResultOut As Boolean

            ValidationEnable = If((Boolean.TryParse(ResourceTable("ValidationEnable"), ResultOut)), ResultOut, True)

            '#End Region

            ResourceTable = Nothing

        End Sub

        ''' <summary>
        ''' Controls Filling Out with data
        ''' </summary>
        ''' <param name="objClientCaseBillingInfoDataObject"></param>
        ''' <remarks></remarks>
        Public Sub FillOutData(ByRef objClientCaseBillingInfoDataObject As ClientCaseBillingInfoDataObject)

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

            TextBoxUnitRate.Text = If(((Not objClientCaseBillingInfoDataObject.UnitRate.Equals(0)) And (Not objClientCaseBillingInfoDataObject.UnitRate.Equals(Decimal.MinValue))),
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

        End Sub

        ''' <summary>
        ''' Clearing controls value
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ClearControls()

            TextBoxAuthorizationNumber.Text = String.Empty
            TextBoxModifierOne.Text = String.Empty
            TextBoxModifierTwo.Text = String.Empty
            TextBoxModifierThree.Text = String.Empty
            TextBoxModifierFour.Text = String.Empty
            TextBoxComments.Text = String.Empty
            TextBoxUnitRate.Text = String.Empty
            TextBoxClaimFrequencyTypeCode.Text = String.Empty

            ClearDropDownSelectedIndex(DropDownListProcedureCode)
            ClearDropDownSelectedIndex(DropDownListPlaceOfService)
            ClearDropDownSelectedIndex(DropDownListProviderSignatureOnFile)
            ClearDropDownSelectedIndex(DropDownListMedicareAssignmentCode)
            ClearDropDownSelectedIndex(DropDownListAssignmentOfBenefitsIndicator)
            ClearDropDownSelectedIndex(DropDownListReleaseOfInformationCode)
            ClearDropDownSelectedIndex(DropDownListPatientSignatureCode)

        End Sub

        ''' <summary>
        ''' This is for retrieving data
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub GetData()
            BindProcedureCodeDropDownList()
            BindPlaceOfServiceDropDownList()
            BindProviderSignatureOnFileDropDownList()
            BindAssignmentOfBenefitsIndicatorDropDownList()
            BindMedicareAssignmentCodeDropDownList()
            BindReleaseOfInformationCodeDropDownList()
            BindPatientSignatureCodeDropDownList()
        End Sub

        Private Sub DefineControlTextLength()

            objShared.SetControlTextLength(TextBoxUnitRate, 10)
            objShared.SetControlTextLength(TextBoxComments, 250)
            objShared.SetControlTextLength(TextBoxAuthorizationNumber, 50)
            objShared.SetControlTextLength(TextBoxModifierOne, 2)
            objShared.SetControlTextLength(TextBoxModifierTwo, 2)
            objShared.SetControlTextLength(TextBoxModifierThree, 2)
            objShared.SetControlTextLength(TextBoxModifierFour, 2)
            objShared.SetControlTextLength(TextBoxClaimFrequencyTypeCode, 1)

        End Sub

        Public Sub DataFoSave(ByRef objClientCaseBillingInfoDataObject As ClientCaseBillingInfoDataObject)

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

        End Sub

        ''' <summary>
        ''' Binding Procedure Code Drop Down List
        ''' </summary>
        Private Sub BindProcedureCodeDropDownList()

            If (Not DropDownListProcedureCode Is Nothing) Then
                Dim objBLClientInfo As New BLClientInfo
                objBLClientInfo.GetProcedureCodeData(objShared.VisitelConnectionString, SqlDataSourceDropDownListProcedureCode)
                objBLClientInfo = Nothing

                DropDownListProcedureCode.DataSourceID = "SqlDataSourceDropDownListProcedureCode"
                DropDownListProcedureCode.DataTextField = "Description"
                DropDownListProcedureCode.DataValueField = "ProcedureCode"
                DropDownListProcedureCode.DataBind()
            End If
        End Sub

        ''' <summary>
        ''' Binding Place Of Service Drop Down List
        ''' </summary>
        Private Sub BindPlaceOfServiceDropDownList()

            If (Not DropDownListPlaceOfService Is Nothing) Then
                Dim objBLClientInfo As New BLClientInfo
                objBLClientInfo.GetPlaceOfServiceData(objShared.VisitelConnectionString, SqlDataSourceDropDownListPlaceOfService)
                objBLClientInfo = Nothing

                DropDownListPlaceOfService.DataSourceID = "SqlDataSourceDropDownListPlaceOfService"
                DropDownListPlaceOfService.DataTextField = "Description"
                DropDownListPlaceOfService.DataValueField = "pos_id"
                DropDownListPlaceOfService.DataBind()
            End If

        End Sub

        ''' <summary>
        ''' Binding Provider Signature On File Drop Down List
        ''' </summary>
        Private Sub BindProviderSignatureOnFileDropDownList()

            If (Not DropDownListProviderSignatureOnFile Is Nothing) Then
                DropDownListProviderSignatureOnFile.DataSource = EnumDataObject.Enumeration.GetAll(Of EnumDataObject.ProviderSignatureOnFile)()
                DropDownListProviderSignatureOnFile.DataTextField = "value"
                DropDownListProviderSignatureOnFile.DataValueField = "value"
                DropDownListProviderSignatureOnFile.DataBind()

                DropDownListProviderSignatureOnFile.SelectedIndex = 1
            End If
        End Sub

        ''' <summary>
        ''' Binding Medicare Assignment Code Drop Down List
        ''' </summary>
        Private Sub BindMedicareAssignmentCodeDropDownList()

            If (Not DropDownListMedicareAssignmentCode Is Nothing) Then
                Dim objBLClientInfo As New BLClientInfo
                objBLClientInfo.GetMedicareAssignmentCodeData(objShared.VisitelConnectionString, SqlDataSourceDropDownListMedicareAssignmentCode)
                objBLClientInfo = Nothing

                DropDownListMedicareAssignmentCode.DataSourceID = "SqlDataSourceDropDownListMedicareAssignmentCode"
                DropDownListMedicareAssignmentCode.DataTextField = "Expr1"
                DropDownListMedicareAssignmentCode.DataValueField = "code"
                DropDownListMedicareAssignmentCode.DataBind()
            End If
        End Sub

        ''' <summary>
        ''' Binding Assignment Of Benefits Indicator Drop Down List
        ''' </summary>
        Private Sub BindAssignmentOfBenefitsIndicatorDropDownList()

            If (Not DropDownListAssignmentOfBenefitsIndicator Is Nothing) Then
                DropDownListAssignmentOfBenefitsIndicator.DataSource = EnumDataObject.Enumeration.GetAll(Of EnumDataObject.AssignmentOfBenefitsIndicator)()
                DropDownListAssignmentOfBenefitsIndicator.DataTextField = "value"
                DropDownListAssignmentOfBenefitsIndicator.DataValueField = "value"
                DropDownListAssignmentOfBenefitsIndicator.DataBind()

                DropDownListAssignmentOfBenefitsIndicator.SelectedIndex = 1
            End If

        End Sub

        ''' <summary>
        ''' Binding Release Of Information Code Drop Down List
        ''' </summary>
        Private Sub BindReleaseOfInformationCodeDropDownList()
            If (Not DropDownListReleaseOfInformationCode Is Nothing) Then
                Dim objBLClientInfo As New BLClientInfo
                objBLClientInfo.GetReleaseOfInformationCodeData(objShared.VisitelConnectionString, SqlDataSourceDropDownListReleaseOfInformationCode)
                objBLClientInfo = Nothing

                DropDownListReleaseOfInformationCode.DataSourceID = "SqlDataSourceDropDownListReleaseOfInformationCode"
                DropDownListReleaseOfInformationCode.DataTextField = "Expr1"
                DropDownListReleaseOfInformationCode.DataValueField = "code"
                DropDownListReleaseOfInformationCode.DataBind()
            End If
        End Sub

        ''' <summary>
        ''' Binding Patient Signature Code Drop Down List
        ''' </summary>
        Private Sub BindPatientSignatureCodeDropDownList()

            If (Not DropDownListPatientSignatureCode Is Nothing) Then
                Dim objBLClientInfo As New BLClientInfo
                objBLClientInfo.GetPatientSignatureCodeData(objShared.VisitelConnectionString, SqlDataSourceDropDownListPatientSignatureCode)
                objBLClientInfo = Nothing

                DropDownListPatientSignatureCode.DataSourceID = "SqlDataSourceDropDownListPatientSignatureCode"
                DropDownListPatientSignatureCode.DataTextField = "Expr1"
                DropDownListPatientSignatureCode.DataValueField = "code"
                DropDownListPatientSignatureCode.DataBind()
            End If


        End Sub

        Private Sub SetControlToolTip()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("Setup", ControlName & Convert.ToString(".resx"))

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

    End Class
End Namespace
