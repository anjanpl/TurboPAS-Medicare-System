Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports VisitelBusiness.VisitelBusiness.DataObject

Namespace Visitel.UserControl.ClientInfo

    Public Class EmergencyContractControl
        Inherits BaseUserControl

        Private ControlName As String, ValidationGroup As String
        Private ValidationEnable As Boolean

        Private objShared As SharedWebControls

        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "EmergencyContractControl"
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
        ''' Binding Emergency Disaster Category Drop Down List
        ''' </summary>
        Protected Sub BindEmergencyDisasterCategoryDropDownList()
            DropDownListEmergencyDisasterCategory.DataSource = EnumDataObject.Enumeration.GetAll(Of EnumDataObject.EmergencyDisasterCategory)()
            DropDownListEmergencyDisasterCategory.DataTextField = "Key"
            DropDownListEmergencyDisasterCategory.DataValueField = "Key"
            DropDownListEmergencyDisasterCategory.DataBind()

            DropDownListEmergencyDisasterCategory.Items.Insert(0, New ListItem("", "-1"))
        End Sub

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("ClientInfo", ControlName & Convert.ToString(".resx"))

            LabelEmergencyDisasterCategory.Text = Convert.ToString(ResourceTable("LabelEmergencyDisasterCategory"), Nothing).Trim()
            LabelEmergencyDisasterCategory.Text = If(String.IsNullOrEmpty(LabelEmergencyDisasterCategory.Text), "Emerg Disaster Category", LabelEmergencyDisasterCategory.Text)

            
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

            ValidationGroup = Convert.ToString(ResourceTable("ValidationGroup"), Nothing).Trim()
            ValidationGroup = If(String.IsNullOrEmpty(ValidationGroup), "Client", ValidationGroup)

            Dim ResultOut As Boolean

            ValidationEnable = If((Boolean.TryParse(ResourceTable("ValidationEnable"), ResultOut)), ResultOut, True)

            ResourceTable = Nothing
        End Sub

        Private Sub GetData()
            BindEmergencyDisasterCategoryDropDownList()
        End Sub

        Public Sub LoadData()
            GetData()
        End Sub

        ''' <summary>
        ''' This is for control events regristering and instantiate object globally
        ''' </summary>
        Public Sub InitializeControl()
            TextBoxEmergencyContactOnePhone.ClientIDMode = objShared.InlineAssignHelper(TextBoxEmergencyContactTwoPhone.ClientIDMode, ClientIDMode.Static)

            SetControlTabIndex()
            SetRegularExpressionSetting()
            DefineControlTextLength()
        End Sub

        Private Sub SetControlTabIndex()
            TextBoxEmergencyContactOneName.TabIndex = 30
            TextBoxEmergencyContactOnePhone.TabIndex = 31
            TextBoxEmergencyContactOneRelationship.TabIndex = 32
            TextBoxEmergencyContactTwoName.TabIndex = 33
            TextBoxEmergencyContactTwoPhone.TabIndex = 34
            TextBoxEmergencyContactTwoRelationship.TabIndex = 35
            DropDownListEmergencyDisasterCategory.TabIndex = 36
        End Sub

        Private Sub SetRegularExpressionSetting()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("Setup", ControlName & Convert.ToString(".resx"))

            Dim ErrorMessage As String = String.Empty, ErrorText As String = String.Empty

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

            ResourceTable = Nothing

            ErrorMessage = Nothing
            ErrorText = Nothing

        End Sub

        Private Sub DefineControlTextLength()
            objShared.SetControlTextLength(TextBoxEmergencyContactOneName, 50)
            objShared.SetControlTextLength(TextBoxEmergencyContactOneRelationship, 50)
            objShared.SetControlTextLength(TextBoxEmergencyContactTwoRelationship, 50)
            objShared.SetControlTextLength(TextBoxEmergencyContactTwoName, 50)
            objShared.SetControlTextLength(TextBoxEmergencyContactOnePhone, 16)
            objShared.SetControlTextLength(TextBoxEmergencyContactTwoPhone, 16)
        End Sub

        ''' <summary>
        ''' Controls Filling Out with data
        ''' </summary>
        ''' <param name="objClientTreatmentInfoDataObject"></param>
        ''' <remarks></remarks>
        Public Sub FillOutData(ByRef objClientTreatmentInfoDataObject As ClientTreatmentInfoDataObject)
            DropDownListEmergencyDisasterCategory.SelectedIndex = DropDownListEmergencyDisasterCategory.Items.IndexOf(DropDownListEmergencyDisasterCategory.Items.FindByValue(
                                                                    Convert.ToString(objClientTreatmentInfoDataObject.DisasterCategory, Nothing)))
        End Sub

        ''' <summary>
        ''' Controls Filling Out with data
        ''' </summary>
        ''' <param name="objClientEmergencyContactInfoDataObject"></param>
        ''' <remarks></remarks>
        Public Sub FillOutData(ByRef objClientEmergencyContactInfoDataObject As ClientEmergencyContactInfoDataObject)
            TextBoxEmergencyContactOneName.Text = objClientEmergencyContactInfoDataObject.EmergencyContact
            TextBoxEmergencyContactOnePhone.Text = objClientEmergencyContactInfoDataObject.EmergencyContactPhone
            TextBoxEmergencyContactOneRelationship.Text = objClientEmergencyContactInfoDataObject.EmergencyContactRelationship
            TextBoxEmergencyContactTwoName.Text = objClientEmergencyContactInfoDataObject.EmergencyContactTwo
            TextBoxEmergencyContactTwoPhone.Text = objClientEmergencyContactInfoDataObject.EmergencyContactPhoneTwo
            TextBoxEmergencyContactTwoRelationship.Text = objClientEmergencyContactInfoDataObject.EmergencyContactRelationshipTwo
        End Sub

        ''' <summary>
        ''' Clearing controls value
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ClearControls()
            DropDownListEmergencyDisasterCategory.SelectedIndex = 0

            TextBoxEmergencyContactOneName.Text = objShared.InlineAssignHelper(TextBoxEmergencyContactOnePhone.Text,
                                                  objShared.InlineAssignHelper(TextBoxEmergencyContactOneRelationship.Text,
                                                  objShared.InlineAssignHelper(TextBoxEmergencyContactTwoName.Text,
                                                  objShared.InlineAssignHelper(TextBoxEmergencyContactTwoPhone.Text,
                                                  objShared.InlineAssignHelper(TextBoxEmergencyContactTwoPhone.Text,
                                                  objShared.InlineAssignHelper(TextBoxEmergencyContactTwoRelationship.Text,
                                                  String.Empty))))))
        End Sub

        ''' <summary>
        ''' This is for validating data in server side before submitting the form
        ''' </summary>
        ''' <returns></returns>
        Public Function ValidateData() As Boolean
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
            Return True
        End Function

        Public Sub DataFoSave(ByRef objClientEmergencyContactInfoDataObject As ClientEmergencyContactInfoDataObject)
            objClientEmergencyContactInfoDataObject.EmergencyContact = Convert.ToString(TextBoxEmergencyContactOneName.Text, Nothing).Trim()
            objClientEmergencyContactInfoDataObject.EmergencyContactPhone = Convert.ToString(TextBoxEmergencyContactOnePhone.Text, Nothing).Trim()
            objClientEmergencyContactInfoDataObject.EmergencyContactRelationship = Convert.ToString(TextBoxEmergencyContactOneRelationship.Text, Nothing).Trim()
            objClientEmergencyContactInfoDataObject.EmergencyContactTwo = Convert.ToString(TextBoxEmergencyContactTwoName.Text, Nothing).Trim()
            objClientEmergencyContactInfoDataObject.EmergencyContactPhoneTwo = Convert.ToString(TextBoxEmergencyContactTwoPhone.Text, Nothing).Trim()
            objClientEmergencyContactInfoDataObject.EmergencyContactRelationshipTwo = Convert.ToString(TextBoxEmergencyContactTwoRelationship.Text, Nothing).Trim()
        End Sub

        Public Sub DataFoSave(ByRef objClientTreatmentInfoDataObject As ClientTreatmentInfoDataObject)
            objClientTreatmentInfoDataObject.DisasterCategory = Convert.ToString(DropDownListEmergencyDisasterCategory.SelectedValue, Nothing).Trim()
        End Sub

    End Class

End Namespace
