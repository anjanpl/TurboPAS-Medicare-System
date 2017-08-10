Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports VisitelBusiness.VisitelBusiness.DataObject

Namespace Visitel.UserControl.ClientInfo

    Public Class EVVControl
        Inherits BaseUserControl

        Private ControlName As String, ValidationGroup As String
        Private ValidationEnable As Boolean

        Private objShared As SharedWebControls

        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "EVVControl"
            objShared = New SharedWebControls()
            objShared.ConnectionOpen()
            GetCaptionFromResource()
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
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("ClientInfo", ControlName & Convert.ToString(".resx"))

            LabelEVV.Text = Convert.ToString(ResourceTable("LabelEVV"), Nothing).Trim()
            LabelEVV.Text = If(String.IsNullOrEmpty(LabelEVV.Text), "EVV", LabelEVV.Text)

            LabelEVVClientId.Text = Convert.ToString(ResourceTable("LabelEVVClientId"), Nothing).Trim()
            LabelEVVClientId.Text = If(String.IsNullOrEmpty(LabelEVVClientId.Text), "EVV Client Id:", LabelEVVClientId.Text)

            LabelEVVARNumber.Text = Convert.ToString(ResourceTable("LabelEVVARNumber"), Nothing).Trim()
            LabelEVVARNumber.Text = If(String.IsNullOrEmpty(LabelEVVARNumber.Text), "ARNumber:", LabelEVVARNumber.Text)

            LabelEVVPriority.Text = Convert.ToString(ResourceTable("LabelEVVPriority"), Nothing).Trim()
            LabelEVVPriority.Text = If(String.IsNullOrEmpty(LabelEVVPriority.Text), "Priority:", LabelEVVPriority.Text)

            LabelEVVBillCode.Text = Convert.ToString(ResourceTable("LabelEVVBillCode"), Nothing).Trim()
            LabelEVVBillCode.Text = If(String.IsNullOrEmpty(LabelEVVBillCode.Text), "Bill Code:", LabelEVVBillCode.Text)

            LabelEVVProcCodeQualifier.Text = Convert.ToString(ResourceTable("LabelEVVProcCodeQualifier"), Nothing).Trim()
            LabelEVVProcCodeQualifier.Text = If(String.IsNullOrEmpty(LabelEVVProcCodeQualifier.Text), "Proc Code Qualifier:", LabelEVVProcCodeQualifier.Text)

            LabelEVVLandPhone.Text = Convert.ToString(ResourceTable("LabelEVVLandPhone"), Nothing).Trim()
            LabelEVVLandPhone.Text = If(String.IsNullOrEmpty(LabelEVVLandPhone.Text), "Land Phone:", LabelEVVLandPhone.Text)

            ValidationGroup = Convert.ToString(ResourceTable("ValidationGroup"), Nothing).Trim()
            ValidationGroup = If(String.IsNullOrEmpty(ValidationGroup), "Client", ValidationGroup)

            Dim ResultOut As Boolean

            ValidationEnable = If((Boolean.TryParse(ResourceTable("ValidationEnable"), ResultOut)), ResultOut, True)

            ResourceTable = Nothing
        End Sub

        Private Sub GetData()
            objShared.BindEVVPriorityDropDownList(DropDownListEVVPriority)
        End Sub

        Public Sub LoadData()
            GetData()
        End Sub

        ''' <summary>
        ''' This is for control events regristering and instantiate object globally
        ''' </summary>
        Private Sub InitializeControl()
            Session.Add(StaticSettings.SessionField.TEXTBOX_EVV_BILL_CODE, TextBoxEVVBillCode)
            Session.Add(StaticSettings.SessionField.TEXTBOX_EVV_PROC_CODE_QUALIFIER, TextBoxEVVProcCodeQualifier)
            Session.Add(StaticSettings.SessionField.TEXTBOX_EVV_LAND_PHONE, TextBoxEVVLandPhone)

            TextBoxEVVLandPhone.ClientIDMode = UI.ClientIDMode.Static

            SetRegularExpressionSetting()
            DefineControlTextLength()

            SetControlTabIndex()

        End Sub

        Private Sub SetControlTabIndex()
            TextBoxEVVClientId.TabIndex = 52
            TextBoxEVVARNumber.TabIndex = 53
            DropDownListEVVPriority.TabIndex = 54
            TextBoxEVVBillCode.TabIndex = 55
            TextBoxEVVProcCodeQualifier.TabIndex = 56

            TextBoxEVVLandPhone.TabIndex = 61
        End Sub

        Private Sub SetRegularExpressionSetting()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("Setup", ControlName & Convert.ToString(".resx"))

            Dim ErrorMessage As String = String.Empty, ErrorText As String = String.Empty

            ErrorMessage = Convert.ToString(ResourceTable("InvalidEVVLandPhoneMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid EVV Land Phone", ErrorMessage)

            ErrorText = ErrorMessage

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorEVVLandPhone, "TextBoxEVVLandPhone", objShared.PhoneValidationExpression,
                                                 ErrorMessage, ErrorText, ValidationEnable, ValidationGroup)

            ResourceTable = Nothing

            ErrorMessage = Nothing
            ErrorText = Nothing

        End Sub

        Private Sub DefineControlTextLength()
            objShared.SetControlTextLength(TextBoxEVVClientId, 10)
            objShared.SetControlTextLength(TextBoxEVVProcCodeQualifier, 50)
            objShared.SetControlTextLength(TextBoxEVVARNumber, 12)
            objShared.SetControlTextLength(TextBoxEVVBillCode, 8)
            objShared.SetControlTextLength(TextBoxEVVLandPhone, 14)
        End Sub

        ''' <summary>
        ''' Controls Filling Out with data
        ''' </summary>
        ''' <param name="objClientCaseEVVInfoDataObject"></param>
        ''' <remarks></remarks>
        Public Sub FillOutData(ByRef objClientCaseEVVInfoDataObject As ClientCaseEVVInfoDataObject)

            TextBoxEVVClientId.Text = objClientCaseEVVInfoDataObject.EVVClientId
            TextBoxEVVARNumber.Text = objClientCaseEVVInfoDataObject.EVVARNumber

            DropDownListEVVPriority.SelectedIndex = DropDownListEVVPriority.Items.IndexOf(DropDownListEVVPriority.Items.FindByValue(
                                                                                          Convert.ToString(objClientCaseEVVInfoDataObject.EVVPriority)))
            TextBoxEVVBillCode.Text = objClientCaseEVVInfoDataObject.BillCode
            TextBoxEVVProcCodeQualifier.Text = objClientCaseEVVInfoDataObject.ProcedureCodeQualifier
            TextBoxEVVLandPhone.Text = objClientCaseEVVInfoDataObject.LandPhone

        End Sub

        ''' <summary>
        ''' Clearing controls value
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ClearControls()
            TextBoxEVVClientId.Text = objShared.InlineAssignHelper(TextBoxEVVARNumber.Text,
                                          objShared.InlineAssignHelper(TextBoxEVVBillCode.Text,
                                          objShared.InlineAssignHelper(TextBoxEVVProcCodeQualifier.Text,
                                          objShared.InlineAssignHelper(TextBoxEVVLandPhone.Text, String.Empty))))

            DropDownListEVVPriority.SelectedIndex = 0
        End Sub

        ''' <summary>
        ''' This is for validating data in server side before submitting the form
        ''' </summary>
        ''' <returns></returns>
        Public Function ValidateData() As Boolean
            If (Not String.IsNullOrEmpty(TextBoxEVVLandPhone.Text.Trim())) Then
                If (Not objShared.ValidatePhone(TextBoxEVVLandPhone.Text.Trim())) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorEVVLandPhone.ErrorMessage)
                    Return False
                End If
            End If

            Return True

        End Function

        Public Sub DataFoSave(ByRef objClientCaseEVVInfoDataObject As ClientCaseEVVInfoDataObject)
            objClientCaseEVVInfoDataObject.EVVClientId = Convert.ToString(TextBoxEVVClientId.Text, Nothing).Trim()
            objClientCaseEVVInfoDataObject.EVVARNumber = Convert.ToString(TextBoxEVVARNumber.Text, Nothing).Trim()

            objClientCaseEVVInfoDataObject.EVVPriority = If((DropDownListEVVPriority.SelectedValue.Equals("-1")),
                                                                           objClientCaseEVVInfoDataObject.EVVPriority,
                                                                           Convert.ToInt32(DropDownListEVVPriority.SelectedValue))

            objClientCaseEVVInfoDataObject.BillCode = Convert.ToString(TextBoxEVVBillCode.Text, Nothing).Trim()

            objClientCaseEVVInfoDataObject.ProcedureCodeQualifier = Convert.ToString(TextBoxEVVProcCodeQualifier.Text, Nothing).Trim()
            'objClientCaseEVVInfoDataObject.LandPhone = objShared.GetReFormattedMobileNumber(Convert.ToString(TextBoxEVVLandPhone.Text, Nothing).Trim())
            objClientCaseEVVInfoDataObject.LandPhone = Convert.ToString(TextBoxEVVLandPhone.Text, Nothing).Trim()
        End Sub

    End Class

End Namespace
