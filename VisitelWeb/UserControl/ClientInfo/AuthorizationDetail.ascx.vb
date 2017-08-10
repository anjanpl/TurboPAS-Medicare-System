Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports VisitelBusiness.VisitelBusiness.DataObject

Namespace Visitel.UserControl.ClientInfo

    Public Class AuthorizationDetailControl
        Inherits BaseUserControl

        Private ControlName As String, InvalidUnitRateMessage As String, ValidationGroup As String

        Private ValidationEnable As Boolean

        Public TextBoxDiagnosis As TextBox

        Private objShared As SharedWebControls

        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "AuthorizationDetailControl"
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
            TextBoxAuthorizatioReceivedDate.ClientIDMode = objShared.InlineAssignHelper(TextBoxPractitionerStatementReceivedDate.ClientIDMode,
                                                           objShared.InlineAssignHelper(TextBoxPractitionerStatementSentDate.ClientIDMode,
                                                           objShared.InlineAssignHelper(TextBoxServiceInitializedReportedDate.ClientIDMode, ClientIDMode.Static)))

            SetRegularExpressionSetting()
            SetControlToolTip()
            SetControlTabIndex()
        End Sub

        Private Sub SetControlTabIndex()
            TextBoxAuthorizatioReceivedDate.TabIndex = 40
            TextBoxPractitionerStatementSentDate.TabIndex = 41
            TextBoxPractitionerStatementReceivedDate.TabIndex = 42
            TextBoxServiceInitializedReportedDate.TabIndex = 43
        End Sub

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("ClientInfo", ControlName & Convert.ToString(".resx"))

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

            ResourceTable = Nothing

        End Sub

        ''' <summary>
        ''' This is for validating data in server side before submitting the form
        ''' </summary>
        ''' <returns></returns>
        Public Function ValidateData() As Boolean

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

            Return True
        End Function

       

        ''' <summary>
        ''' Clearing controls value
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ClearControls()
            TextBoxAuthorizatioReceivedDate.Text = objShared.InlineAssignHelper(TextBoxAuthorizatioReceivedDate.Text,
                                                    objShared.InlineAssignHelper(TextBoxPractitionerStatementSentDate.Text,
                                                    objShared.InlineAssignHelper(TextBoxPractitionerStatementReceivedDate.Text,
                                                    objShared.InlineAssignHelper(TextBoxServiceInitializedReportedDate.Text, String.Empty))))
        End Sub

        ''' <summary>
        ''' This is for retrieving data
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub GetData()

        End Sub

        Public Sub DataFoSave(ByRef objClientCaseCareInfoDataObject As ClientCaseCareInfoDataObject)
            objClientCaseCareInfoDataObject.AuthorizationReceivedDate = Convert.ToString(TextBoxAuthorizatioReceivedDate.Text, Nothing).Trim()
            objClientCaseCareInfoDataObject.DoctorOrderSentDate = Convert.ToString(TextBoxPractitionerStatementSentDate.Text, Nothing).Trim()
            objClientCaseCareInfoDataObject.DoctorOrderReceivedDate = Convert.ToString(TextBoxPractitionerStatementReceivedDate.Text, Nothing).Trim()
            objClientCaseCareInfoDataObject.ServiceInitializedReportedDate = Convert.ToString(TextBoxServiceInitializedReportedDate.Text, Nothing).Trim()
        End Sub

        Private Sub SetControlToolTip()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("Setup", ControlName & Convert.ToString(".resx"))

            Dim DateToolTip As String = Convert.ToString(ResourceTable("DateFieldToolTip"), Nothing)
            'DateToolTip = If(String.IsNullOrEmpty(DateToolTip), "Example: August 21, 2014", DateToolTip)
            DateToolTip = If(String.IsNullOrEmpty(DateToolTip), "Example: 10/27/2015", DateToolTip)

            
            TextBoxAuthorizatioReceivedDate.ToolTip = objShared.InlineAssignHelper(TextBoxPractitionerStatementReceivedDate.ToolTip,
                                                      objShared.InlineAssignHelper(TextBoxPractitionerStatementSentDate.ToolTip,
                                                      objShared.InlineAssignHelper(TextBoxServiceInitializedReportedDate.ToolTip, DateToolTip)))

            ResourceTable = Nothing

        End Sub

        Private Sub SetRegularExpressionSetting()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("Setup", ControlName & Convert.ToString(".resx"))

            Dim ErrorMessage As String = String.Empty, ErrorText As String = String.Empty

            ErrorText = Convert.ToString(ResourceTable("InvalidDateCommonMessage"), Nothing)
            ErrorText = If(String.IsNullOrEmpty(ErrorText), "Invalid", ErrorText)

            ErrorMessage = Convert.ToString(ResourceTable("InvalidAssessmentDateMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Assessment Date.", ErrorMessage)

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

        ''' <summary>
        ''' Controls Filling Out with data
        ''' </summary>
        ''' <param name="objClientCaseCareInfoDataObject"></param>
        ''' <remarks></remarks>
        Public Sub FillOutData(ByRef objClientCaseCareInfoDataObject As ClientCaseCareInfoDataObject)

            TextBoxAuthorizatioReceivedDate.Text = If((String.IsNullOrEmpty(Convert.ToString(objClientCaseCareInfoDataObject.AuthorizationReceivedDate, Nothing))),
                                                          String.Empty, objClientCaseCareInfoDataObject.AuthorizationReceivedDate)

            TextBoxPractitionerStatementSentDate.Text = If((String.IsNullOrEmpty(Convert.ToString(objClientCaseCareInfoDataObject.DoctorOrderSentDate, Nothing))),
                                                           String.Empty, objClientCaseCareInfoDataObject.DoctorOrderSentDate)

            TextBoxPractitionerStatementReceivedDate.Text = If((String.IsNullOrEmpty(Convert.ToString(objClientCaseCareInfoDataObject.DoctorOrderReceivedDate, Nothing))),
                                                                String.Empty, objClientCaseCareInfoDataObject.DoctorOrderReceivedDate)

            TextBoxServiceInitializedReportedDate.Text = If((String.IsNullOrEmpty(Convert.ToString(objClientCaseCareInfoDataObject.ServiceInitializedReportedDate, Nothing))),
                                                            String.Empty, objClientCaseCareInfoDataObject.ServiceInitializedReportedDate)

        End Sub

        

       

    End Class
End Namespace
