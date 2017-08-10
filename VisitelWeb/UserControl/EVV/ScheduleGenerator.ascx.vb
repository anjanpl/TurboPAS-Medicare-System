Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports System.Data.SqlClient
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelCommon

Namespace Visitel.UserControl.EVV
    Public Class ScheduleGeneratorControl
        Inherits CommonDataControl

        Private ContractNumber As String
        Private objShared As SharedWebControls

        Private ValidationGroup As String

        Private ControlName As String
        Private ValidationEnable As Boolean

        Public EVVName As String

        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "ScheduleGeneratorControl"
            objShared = New SharedWebControls()
            objShared.ConnectionOpen()
            InitializeControl()
        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)

            Select Case EVVName
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EVVName.Vesta)
                    GetCaptionFromResource()
                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EVVName.MEDsys)
                    GetCaptionFromResource("")
                    ButtonAutoGenerateEmployeeEVVIDs.Visible = False
                    ButtonAutoGenerateClientEVVIDs.Visible = False
                    Exit Select
            End Select

            If Not IsPostBack Then
                GetData()
            End If

        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadCss("EVV/" & ControlName)
            LoadJScript()
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing
        End Sub

        Private Sub ButtonGenerateVestaSchedule_Click(sender As Object, e As EventArgs)
            If (ParameterCheck()) Then
                Dim objBLScheduleGeneration As New BLScheduleGeneration()

                Try
                    objBLScheduleGeneration.GenerateVestaSchedule(objShared.ConVisitel, objShared.CompanyId, TextBoxPayPeriodToDate.Text.Trim() _
                                , TextBoxPayPeriodToDate.Text.Trim(), If(String.IsNullOrEmpty(TextBoxDays.Text.Trim()), 7, TextBoxDays.Text.Trim()) _
                                , Convert.ToInt64(DropDownListClient.SelectedValue), Convert.ToInt32(DropDownListClientType.SelectedValue) _
                                , Convert.ToInt32(DropDownListClientGroup.SelectedValue), objShared.UserId, EVVName)

                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} Schedule Generation Successful", EVVName))
                Catch ex As SqlException
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to Generate {0} Schedule. Message: {1}", EVVName, ex.Message))
                Finally
                    objBLScheduleGeneration = Nothing
                End Try
            End If
        End Sub

        Private Sub ButtonAttendants_Click(sender As Object, e As EventArgs)
            Select Case EVVName
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EVVName.Vesta)
                    Response.Redirect("~/Pages/Vesta/Employees.aspx")
                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EVVName.MEDsys)
                    Response.Redirect("~/Pages/MEDsys/MedSysStaff.aspx")
                    Exit Select
            End Select
        End Sub

        Private Sub ButtonClients_Click(sender As Object, e As EventArgs)
            Select Case EVVName
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EVVName.Vesta)
                    Response.Redirect("~/Pages/Vesta/Clients.aspx")
                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EVVName.MEDsys)
                    Response.Redirect("~/Pages/MEDsys/MedSysClient.aspx")
                    Exit Select
            End Select
        End Sub

        Private Sub ButtonAuthorizations_Click(sender As Object, e As EventArgs)

            Select Case EVVName
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EVVName.Vesta)
                    Response.Redirect("~/Pages/Vesta/Authorizations.aspx")
                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EVVName.MEDsys)
                    Response.Redirect("~/Pages/MEDsys/MedSysAuthorization.aspx")
                    Exit Select
            End Select

        End Sub

        Private Sub ButtonNewVisits_Click(sender As Object, e As EventArgs)
            Select Case EVVName
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EVVName.Vesta)
                    Response.Redirect("~/Pages/Vesta/Visits.aspx")
                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EVVName.MEDsys)
                    Response.Redirect("~/Pages/MEDsys/MedSysSchedule.aspx")
                    Exit Select
            End Select
        End Sub

        Private Sub ButtonLoggedVisits_Click(sender As Object, e As EventArgs)
            Select Case EVVName
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EVVName.Vesta)
                    Response.Redirect("~/Pages/Vesta/VisitsLog.aspx")
                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.EVVName.MEDsys)
                    Response.Redirect("~/Pages/MEDsys/MedSysScheduleLog.aspx")
                    Exit Select
            End Select
        End Sub

        Private Sub ButtonSyncRecords_Click(sender As Object, e As EventArgs)
            Dim objBLScheduleGeneration As New BLScheduleGeneration()

            Try
                objBLScheduleGeneration.SyncRecords(objShared.ConVisitel, objShared.UserId, EVVName)

                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} Sync Records Successful", EVVName))
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to Sync {0} Records. Message: {1}", EVVName, ex.Message))
            Finally
                objBLScheduleGeneration = Nothing
            End Try
        End Sub

        Private Sub ButtonClearClient_Click(sender As Object, e As EventArgs)
            ClearControl()
        End Sub

        Private Sub ButtonAutoGenerateEmployeeEVVIDs_Click(sender As Object, e As EventArgs)
            Dim objBLScheduleGeneration As New BLScheduleGeneration()

            Try
                objBLScheduleGeneration.GenerateEmployeeEVVId(objShared.ConVisitel)

                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Employee EVV Id Generation Successful")
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to Generate Employee EVV Id. Message: {0}", ex.Message))
            Finally
                objBLScheduleGeneration = Nothing
            End Try
        End Sub

        Private Sub ButtonAutoGenerateClientEVVIDs_Click(sender As Object, e As EventArgs)
            Dim objBLScheduleGeneration As New BLScheduleGeneration()

            Try
                objBLScheduleGeneration.GenerateClientEVVId(objShared.ConVisitel)

                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Client EVV Id Generation Successful")
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to Generate Client EVV Id. Message: {0}", ex.Message))
            Finally
                objBLScheduleGeneration = Nothing
            End Try
        End Sub


        Private Sub LoadJScript()

            LoadJS("JavaScript/jquery.blockUI.js")

            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                         & " var CalendarDateFormat='" & objShared.CalendarDateFormat & "'; " _
                         & " var CalendarImagePath='" & objShared.GetCalendarImagePath & "'; " _
                         & " var LoaderImagePath ='" & objShared.GetLoaderImagePath & "'; " _
                         & " var CustomTargetButton ='ButtonAutoGenerateEmployeeEVVIDs'; " _
                         & " var CustomDialogHeader ='Schedule Generator'; " _
                         & " var CustomDialogConfirmMsg ='Click \'Yes\' to generate Employee EVV IDs.'; " _
                         & " var CustomTargetButton1 ='ButtonAutoGenerateClientEVVIDs'; " _
                         & " var CustomDialogHeader1 ='Schedule Generator'; " _
                         & " var CustomDialogConfirmMsg1 ='Click \'Yes\' to generate Individual EVV IDs.'; " _
                         & " var prm =''; " _
                         & " jQuery(document).ready(function () {" _
                         & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                         & "     prm.add_beginRequest(SetButtonActionProgress); " _
                         & "     prm.add_endRequest(EndRequest); " _
                         & "     DateFieldsEvent();" _
                         & "     prm.add_endRequest(DateFieldsEvent); " _
                         & "     AutoGenerateEmployeeEVVID();" _
                         & "     prm.add_endRequest(AutoGenerateEmployeeEVVID); " _
                         & "     AutoGenerateClientEVVID();" _
                         & "     prm.add_endRequest(AutoGenerateClientEVVID); " _
                         & "}); " _
                  & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

            LoadJS("JavaScript/EVV/" & ControlName & ".js")

        End Sub

        Private Sub InitializeControl()

            ButtonGenerateVestaSchedule.ValidationGroup = ValidationGroup
            ButtonGenerateVestaSchedule.CausesValidation = True

            AddHandler ButtonGenerateVestaSchedule.Click, AddressOf ButtonGenerateVestaSchedule_Click
            AddHandler ButtonAttendants.Click, AddressOf ButtonAttendants_Click
            AddHandler ButtonClients.Click, AddressOf ButtonClients_Click
            AddHandler ButtonAuthorizations.Click, AddressOf ButtonAuthorizations_Click
            AddHandler ButtonNewVisits.Click, AddressOf ButtonNewVisits_Click
            AddHandler ButtonLoggedVisits.Click, AddressOf ButtonLoggedVisits_Click
            AddHandler ButtonSyncRecords.Click, AddressOf ButtonSyncRecords_Click
            AddHandler ButtonClearClient.Click, AddressOf ButtonClearClient_Click
            AddHandler ButtonAutoGenerateEmployeeEVVIDs.Click, AddressOf ButtonAutoGenerateEmployeeEVVIDs_Click
            AddHandler ButtonAutoGenerateClientEVVIDs.Click, AddressOf ButtonAutoGenerateClientEVVIDs_Click

            SetRequiredFieldSetting()
            SetRegularExpressionSetting()

            TextBoxPayPeriodFromDate.ClientIDMode = UI.ClientIDMode.Static
            TextBoxPayPeriodToDate.ClientIDMode = UI.ClientIDMode.Static

            ButtonGenerateVestaSchedule.ClientIDMode = UI.ClientIDMode.Static
            ButtonAttendants.ClientIDMode = UI.ClientIDMode.Static
            ButtonClients.ClientIDMode = UI.ClientIDMode.Static
            ButtonAuthorizations.ClientIDMode = UI.ClientIDMode.Static
            ButtonNewVisits.ClientIDMode = UI.ClientIDMode.Static
            ButtonLoggedVisits.ClientIDMode = UI.ClientIDMode.Static
            ButtonSyncRecords.ClientIDMode = UI.ClientIDMode.Static
            ButtonClearClient.ClientIDMode = UI.ClientIDMode.Static
            ButtonAutoGenerateEmployeeEVVIDs.ClientIDMode = UI.ClientIDMode.Static
            ButtonAutoGenerateClientEVVIDs.ClientIDMode = UI.ClientIDMode.Static

            DropDownListClient.ClientIDMode = UI.ClientIDMode.Static

        End Sub
        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("EVV", ControlName & Convert.ToString(".resx"))

            LabelScheduleGenerator.Text = Convert.ToString(ResourceTable("LabelScheduleGenerator"), Nothing)
            LabelScheduleGenerator.Text = If(String.IsNullOrEmpty(LabelScheduleGenerator.Text), "Vesta Pay Period Generator", LabelScheduleGenerator.Text)

            LabelCompanyName.Text = Convert.ToString(ResourceTable("LabelCompanyName"), Nothing)
            LabelCompanyName.Text = If(String.IsNullOrEmpty(LabelCompanyName.Text), "TurboPAS Company", LabelCompanyName.Text)

            LabelVestaAgencyIdCaption.Text = Convert.ToString(ResourceTable("LabelVestaAgencyIdCaption"), Nothing)
            LabelVestaAgencyIdCaption.Text = If(String.IsNullOrEmpty(LabelVestaAgencyIdCaption.Text), "Agency Vesta Id:", LabelVestaAgencyIdCaption.Text)

            LabelVestaAgencyId.Text = Convert.ToString(ResourceTable("LabelVestaAgencyId"), Nothing)
            LabelVestaAgencyId.Text = If(String.IsNullOrEmpty(LabelVestaAgencyId.Text), "Test", LabelVestaAgencyId.Text)

            LabelTransmissionMessageSentToCaption.Text = Convert.ToString(ResourceTable("LabelTransmissionMessageSentToCaption"), Nothing)
            LabelTransmissionMessageSentToCaption.Text = If(String.IsNullOrEmpty(LabelTransmissionMessageSentToCaption.Text),
                                                            "Transmission Message Sent To:", LabelTransmissionMessageSentToCaption.Text)

            LabelTransmissionMessageSentTo.Text = Convert.ToString(ResourceTable("LabelTransmissionMessageSentTo"), Nothing)
            LabelTransmissionMessageSentTo.Text = If(String.IsNullOrEmpty(LabelTransmissionMessageSentTo.Text),
                                                            "jnosike@kds-tx.com", LabelTransmissionMessageSentTo.Text)

            LabelUploadAllUpdatesCaption1.Text = Convert.ToString(ResourceTable("LabelUploadAllUpdatesCaption1"), Nothing)
            LabelUploadAllUpdatesCaption1.Text = If(String.IsNullOrEmpty(LabelUploadAllUpdatesCaption1.Text),
                                                            "Upload all updates since the past", LabelUploadAllUpdatesCaption1.Text)

            TextBoxDays.Text = Convert.ToString(ResourceTable("TextBoxDays"), Nothing)
            TextBoxDays.Text = If(String.IsNullOrEmpty(TextBoxDays.Text), "31", TextBoxDays.Text)

            LabelUploadAllUpdatesCaption2.Text = Convert.ToString(ResourceTable("LabelUploadAllUpdatesCaption2"), Nothing)
            LabelUploadAllUpdatesCaption2.Text = If(String.IsNullOrEmpty(LabelUploadAllUpdatesCaption2.Text), "day(s)", LabelUploadAllUpdatesCaption2.Text)

            LabelPayPeriod.Text = Convert.ToString(ResourceTable("LabelPayPeriod"), Nothing)
            LabelPayPeriod.Text = If(String.IsNullOrEmpty(LabelPayPeriod.Text), "Pay Period:", LabelPayPeriod.Text)

            LabelClient.Text = Convert.ToString(ResourceTable("LabelClient"), Nothing)
            LabelClient.Text = If(String.IsNullOrEmpty(LabelClient.Text), "Client:", LabelClient.Text)

            LabelClientType.Text = Convert.ToString(ResourceTable("LabelClientType"), Nothing)
            LabelClientType.Text = If(String.IsNullOrEmpty(LabelClientType.Text), "Client Type:", LabelClientType.Text)

            LabelClientGroup.Text = Convert.ToString(ResourceTable("LabelClientGroup"), Nothing)
            LabelClientGroup.Text = If(String.IsNullOrEmpty(LabelClientGroup.Text), "Client Group:", LabelClientGroup.Text)

            ButtonGenerateVestaSchedule.Text = Convert.ToString(ResourceTable("ButtonGenerateVestaSchedule"), Nothing)
            ButtonGenerateVestaSchedule.Text = If(String.IsNullOrEmpty(ButtonGenerateVestaSchedule.Text), "Generate Vesta Schedule", ButtonGenerateVestaSchedule.Text)

            ButtonAttendants.Text = Convert.ToString(ResourceTable("ButtonAttendants"), Nothing)
            ButtonAttendants.Text = If(String.IsNullOrEmpty(ButtonAttendants.Text), "Attendants", ButtonAttendants.Text)

            ButtonClients.Text = Convert.ToString(ResourceTable("ButtonClients"), Nothing)
            ButtonClients.Text = If(String.IsNullOrEmpty(ButtonClients.Text), "Clients", ButtonClients.Text)

            ButtonAuthorizations.Text = Convert.ToString(ResourceTable("ButtonAuthorizations"), Nothing)
            ButtonAuthorizations.Text = If(String.IsNullOrEmpty(ButtonAuthorizations.Text), "Authorizations", ButtonAuthorizations.Text)

            ButtonNewVisits.Text = Convert.ToString(ResourceTable("ButtonNewVisits"), Nothing)
            ButtonNewVisits.Text = If(String.IsNullOrEmpty(ButtonNewVisits.Text), "New Visits", ButtonNewVisits.Text)

            ButtonLoggedVisits.Text = Convert.ToString(ResourceTable("ButtonLoggedVisits"), Nothing)
            ButtonLoggedVisits.Text = If(String.IsNullOrEmpty(ButtonLoggedVisits.Text), "Logged Visits", ButtonLoggedVisits.Text)

            ButtonSyncRecords.Text = Convert.ToString(ResourceTable("ButtonSyncRecords"), Nothing)
            ButtonSyncRecords.Text = If(String.IsNullOrEmpty(ButtonSyncRecords.Text), "Sync Records", ButtonSyncRecords.Text)

            ButtonClearClient.Text = Convert.ToString(ResourceTable("ButtonClearClient"), Nothing)
            ButtonClearClient.Text = If(String.IsNullOrEmpty(ButtonClearClient.Text), "Clear Client", ButtonClearClient.Text)

            ButtonAutoGenerateEmployeeEVVIDs.Text = Convert.ToString(ResourceTable("ButtonAutoGenerateEmployeeEVVIDs"), Nothing)
            ButtonAutoGenerateEmployeeEVVIDs.Text = If(String.IsNullOrEmpty(ButtonAutoGenerateEmployeeEVVIDs.Text),
                                                       String.Format("Auto-Generate{0}{1}", Environment.NewLine, "Employee EVV IDs"), ButtonAutoGenerateEmployeeEVVIDs.Text)

            ButtonAutoGenerateClientEVVIDs.Text = Convert.ToString(ResourceTable("ButtonAutoGenerateClientEVVIDs"), Nothing)
            ButtonAutoGenerateClientEVVIDs.Text = If(String.IsNullOrEmpty(ButtonAutoGenerateClientEVVIDs.Text),
                                                       String.Format("Auto-Generate{0}{1}", Environment.NewLine, "Client EVV IDs"), ButtonAutoGenerateClientEVVIDs.Text)

            Dim ResultOut As Boolean

            ValidationEnable = If((Boolean.TryParse(ResourceTable("ValidationEnable"), ResultOut)), ResultOut, True)

            ValidationGroup = Convert.ToString(ResourceTable("ValidationGroup"), Nothing)
            ValidationGroup = If(String.IsNullOrEmpty(ValidationGroup), "ScheduleGenerator", ValidationGroup)

            ResourceTable = Nothing

        End Sub

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource(MEDsys As String)

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("EVV", ControlName & Convert.ToString(".resx"))

            LabelScheduleGenerator.Text = Convert.ToString(ResourceTable("MEDsysLabelScheduleGenerator"), Nothing)
            LabelScheduleGenerator.Text = If(String.IsNullOrEmpty(LabelScheduleGenerator.Text), "MEDsys Pay Period Generator", LabelScheduleGenerator.Text)

            LabelCompanyName.Text = Convert.ToString(ResourceTable("MEDsysLabelCompanyName"), Nothing)
            LabelCompanyName.Text = If(String.IsNullOrEmpty(LabelCompanyName.Text), "TurboPAS Company", LabelCompanyName.Text)

            LabelVestaAgencyIdCaption.Text = Convert.ToString(ResourceTable("MEDsysLabelVestaAgencyIdCaption"), Nothing)
            LabelVestaAgencyIdCaption.Text = If(String.IsNullOrEmpty(LabelVestaAgencyIdCaption.Text), "Agency MEDsys Id:", LabelVestaAgencyIdCaption.Text)

            LabelVestaAgencyId.Text = Convert.ToString(ResourceTable("MEDsysLabelVestaAgencyId"), Nothing)
            LabelVestaAgencyId.Text = If(String.IsNullOrEmpty(LabelVestaAgencyId.Text), "Test", LabelVestaAgencyId.Text)

            LabelTransmissionMessageSentToCaption.Text = Convert.ToString(ResourceTable("MEDsysLabelTransmissionMessageSentToCaption"), Nothing)
            LabelTransmissionMessageSentToCaption.Text = If(String.IsNullOrEmpty(LabelTransmissionMessageSentToCaption.Text),
                                                            "Transmission Message Sent To:", LabelTransmissionMessageSentToCaption.Text)

            LabelTransmissionMessageSentTo.Text = Convert.ToString(ResourceTable("MEDsysLabelTransmissionMessageSentTo"), Nothing)
            LabelTransmissionMessageSentTo.Text = If(String.IsNullOrEmpty(LabelTransmissionMessageSentTo.Text),
                                                            "jnosike@kds-tx.com", LabelTransmissionMessageSentTo.Text)

            LabelUploadAllUpdatesCaption1.Text = Convert.ToString(ResourceTable("MEDsysLabelUploadAllUpdatesCaption1"), Nothing)
            LabelUploadAllUpdatesCaption1.Text = If(String.IsNullOrEmpty(LabelUploadAllUpdatesCaption1.Text),
                                                            "Upload all updates since the past", LabelUploadAllUpdatesCaption1.Text)

            TextBoxDays.Text = Convert.ToString(ResourceTable("MEDsysTextBoxDays"), Nothing)
            TextBoxDays.Text = If(String.IsNullOrEmpty(TextBoxDays.Text), "31", TextBoxDays.Text)

            LabelUploadAllUpdatesCaption2.Text = Convert.ToString(ResourceTable("MEDsysLabelUploadAllUpdatesCaption2"), Nothing)
            LabelUploadAllUpdatesCaption2.Text = If(String.IsNullOrEmpty(LabelUploadAllUpdatesCaption2.Text), "day(s)", LabelUploadAllUpdatesCaption2.Text)

            LabelPayPeriod.Text = Convert.ToString(ResourceTable("MEDsysLabelPayPeriod"), Nothing)
            LabelPayPeriod.Text = If(String.IsNullOrEmpty(LabelPayPeriod.Text), "Pay Period:", LabelPayPeriod.Text)

            LabelClient.Text = Convert.ToString(ResourceTable("MEDsysLabelClient"), Nothing)
            LabelClient.Text = If(String.IsNullOrEmpty(LabelClient.Text), "Client:", LabelClient.Text)

            LabelClientType.Text = Convert.ToString(ResourceTable("MEDsysLabelClientType"), Nothing)
            LabelClientType.Text = If(String.IsNullOrEmpty(LabelClientType.Text), "Client Type:", LabelClientType.Text)

            LabelClientGroup.Text = Convert.ToString(ResourceTable("MEDsysLabelClientGroup"), Nothing)
            LabelClientGroup.Text = If(String.IsNullOrEmpty(LabelClientGroup.Text), "Client Group:", LabelClientGroup.Text)

            ButtonGenerateVestaSchedule.Text = Convert.ToString(ResourceTable("MEDsysButtonGenerateVestaSchedule"), Nothing)
            ButtonGenerateVestaSchedule.Text = If(String.IsNullOrEmpty(ButtonGenerateVestaSchedule.Text), "Generate MEDsys Schedule", ButtonGenerateVestaSchedule.Text)

            ButtonAttendants.Text = Convert.ToString(ResourceTable("MEDsysButtonAttendants"), Nothing)
            ButtonAttendants.Text = If(String.IsNullOrEmpty(ButtonAttendants.Text), "Staff", ButtonAttendants.Text)

            ButtonClients.Text = Convert.ToString(ResourceTable("MEDsysButtonClients"), Nothing)
            ButtonClients.Text = If(String.IsNullOrEmpty(ButtonClients.Text), "Clients", ButtonClients.Text)

            ButtonAuthorizations.Text = Convert.ToString(ResourceTable("MEDsysButtonAuthorizations"), Nothing)
            ButtonAuthorizations.Text = If(String.IsNullOrEmpty(ButtonAuthorizations.Text), "Authorizations", ButtonAuthorizations.Text)

            ButtonNewVisits.Text = Convert.ToString(ResourceTable("MEDsysButtonNewVisits"), Nothing)
            ButtonNewVisits.Text = If(String.IsNullOrEmpty(ButtonNewVisits.Text), "Schedule", ButtonNewVisits.Text)

            ButtonLoggedVisits.Text = Convert.ToString(ResourceTable("MEDsysButtonLoggedVisits"), Nothing)
            ButtonLoggedVisits.Text = If(String.IsNullOrEmpty(ButtonLoggedVisits.Text), "Schedule Log", ButtonLoggedVisits.Text)

            ButtonSyncRecords.Text = Convert.ToString(ResourceTable("MEDsysButtonSyncRecords"), Nothing)
            ButtonSyncRecords.Text = If(String.IsNullOrEmpty(ButtonSyncRecords.Text), "Sync Records", ButtonSyncRecords.Text)

            ButtonClearClient.Text = Convert.ToString(ResourceTable("MEDsysButtonClearClient"), Nothing)
            ButtonClearClient.Text = If(String.IsNullOrEmpty(ButtonClearClient.Text), "Clear Client", ButtonClearClient.Text)

            Dim ResultOut As Boolean

            ValidationEnable = If((Boolean.TryParse(ResourceTable("MEDsysValidationEnable"), ResultOut)), ResultOut, True)

            ValidationGroup = Convert.ToString(ResourceTable("MEDsysValidationGroup"), Nothing)
            ValidationGroup = If(String.IsNullOrEmpty(ValidationGroup), "ScheduleGenerator", ValidationGroup)

            ResourceTable = Nothing

        End Sub

        ''' <summary>
        ''' Clearing controls value
        ''' </summary>
        Private Sub ClearControl()
            TextBoxDays.Text = String.Empty
            TextBoxPayPeriodFromDate.Text = String.Empty
            TextBoxPayPeriodToDate.Text = String.Empty
            DropDownListClient.SelectedIndex = 0
            DropDownListClientGroup.SelectedIndex = 0
            DropDownListClientType.SelectedIndex = 0
        End Sub

        Private Sub GetData()
            objShared.BindClientDropDownList(DropDownListClient, objShared.CompanyId, EnumDataObject.ClientListFor.VestaClient)
            objShared.BindClientTypeDropDownList(DropDownListClientType, objShared.CompanyId, EnumDataObject.ClientTypeListFor.Vesta)
            objShared.BindClientGroupDropDownList(DropDownListClientGroup, objShared.CompanyId)
            DropDownListClientGroup.Items.RemoveAt(0)
            DropDownListClientGroup.Items.Insert(0, New ListItem("", "-1"))
        End Sub

        ''' <summary>
        ''' Configuring Required Field Controls
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub SetRequiredFieldSetting()

            objShared.SetRequiredFieldValidatorSetting(RequiredFieldValidatorPayPeriodFromDate, "TextBoxPayPeriodFromDate", "*", "Invalid Date", ValidationEnable, ValidationGroup)
            objShared.SetRequiredFieldValidatorSetting(RequiredFieldValidatorPayPeriodToDate, "TextBoxPayPeriodToDate", "*", "Invalid Date", ValidationEnable, ValidationGroup)

        End Sub

        ''' <summary>
        ''' Configuring Regular Expression Field Controls
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub SetRegularExpressionSetting()

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorPayPeriodFromDate, "TextBoxPayPeriodFromDate", objShared.DateValidationExpression,
                                                           "Invalid Date", "Invalid Date", ValidationEnable, ValidationGroup)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorPayPeriodToDate, "TextBoxPayPeriodToDate", objShared.DateValidationExpression,
                                                           "Invalid Date", "Invalid Date", ValidationEnable, ValidationGroup)
        End Sub

        Private Function ParameterCheck() As Boolean

            If ((String.IsNullOrEmpty(TextBoxPayPeriodFromDate.Text.Trim())) Or (String.IsNullOrEmpty(TextBoxPayPeriodToDate.Text.Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("Please correctly set your pay period, {0}", "TurboPAS"))
                Return False
            End If

            If ((Convert.ToDateTime(TextBoxPayPeriodFromDate.Text.Trim(), Nothing)) > (Convert.ToDateTime(TextBoxPayPeriodToDate.Text.Trim(), Nothing))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("Your End Date is earlier than your Start Date {0}", "Error!"))
                Return False
            End If

            If (String.IsNullOrEmpty(TextBoxDays.Text.Trim) Or TextBoxDays.Text.Trim.Equals(0)) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("If you do not specify the time period you want to update TurboPAS {0}{1}",
                                                                                             "will default it to the last 7 days", "TurboPAS"))
            End If

            Return True

        End Function

    End Class
End Namespace