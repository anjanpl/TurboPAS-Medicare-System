Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports VisitelBusiness.VisitelBusiness.DataObject

Namespace Visitel.UserControl.ClientInfo

    Public Class IndividualOtherInformationControl
        Inherits BaseUserControl

        Private ControlName As String, InvalidUnitRateMessage As String, ValidationGroup As String

        Private ValidationEnable As Boolean

        Private objShared As SharedWebControls

        Protected Sub Page_Init(sender As Object, e As EventArgs)

            ControlName = "IndividualOtherInformationControl"
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

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing
        End Sub

        Private Sub DropDownListClientType_OnSelectedIndexChanged(sender As Object, e As EventArgs)

        End Sub

        ''' <summary>
        ''' This is for control events regristering and instantiate object globally
        ''' </summary>
        Private Sub InitializeControl()
            TextBoxUpdateDate.ReadOnly = objShared.InlineAssignHelper(TextBoxUpdateBy.ReadOnly, True)
            TextBoxUnits.Attributes.Add("onkeypress", "return isNumericKey(event);")
            TextBoxNumberOfWeekDays.Attributes.Add("onkeypress", "return isNumericKey(event);")

            LinkButtonAddMoreDischargeReason.Attributes("href") = "../Settings/Popup/DischargeReasonPopup.aspx?Mode=Insert&TB_iframe=true&height=450&width=620"
            LinkButtonAddMoreDischargeReason.Attributes("title") = "Discharge Reason Setting"

            LinkButtonAddMoreCaseWorker.Attributes("href") = "../Settings/Popup/CaseWorkerPopup.aspx?Mode=Insert&TB_iframe=true&height=550&width=980"
            LinkButtonAddMoreCaseWorker.Attributes("title") = "Case Worker Setting"

            LinkButtonAddMoreDoctor.Attributes("href") = "../Settings/Popup/DoctorPopup.aspx?Mode=Insert&TB_iframe=true&height=550&width=980"
            LinkButtonAddMoreDoctor.Attributes("title") = "Doctor Setting"

            LinkButtonAddMoreClientType.Attributes("href") = "../Settings/Popup/ClientTypePopup.aspx?Mode=Insert&TB_iframe=true&height=550&width=1100"
            LinkButtonAddMoreClientType.Attributes("title") = "Client Type Setting"

            DropDownListClientType.ClientIDMode = ClientIDMode.Static

            TextBoxDiagnosis.ClientIDMode = objShared.InlineAssignHelper(TextBoxSuppliesOrEquipment.ClientIDMode,
                                            objShared.InlineAssignHelper(TextBoxSupervisorLastVisitDate.ClientIDMode,
                                            objShared.InlineAssignHelper(TextBoxUnits.ClientIDMode, ClientIDMode.Static)))

            SetControlToolTip()
            SetRegularExpressionSetting()
            DefineControlTextLength()

            SetControlTabIndex()
        End Sub

        Public Function GetClientTypeDropDownList() As DropDownList
            Return DropDownListClientType
        End Function

        Private Sub SetControlTabIndex()
            TextBoxSupervisorLastVisitDate.TabIndex = 20
            DropDownListDischargeReason.TabIndex = 21
            DropDownListCaseWorker.TabIndex = 22
            DropDownListDoctor.TabIndex = 23
            DropDownListPriority.TabIndex = 24
            DropDownListClientType.TabIndex = 25
            TextBoxUnits.TabIndex = 26
            TextBoxNumberOfWeekDays.TabIndex = 27
            TextBoxDiagnosis.TabIndex = 28
            TextBoxSuppliesOrEquipment.TabIndex = 37
        End Sub

        Private Sub SetRegularExpressionSetting()
            Dim ResourceTable As Hashtable = objShared.GetResourceValue("Setup", ControlName & Convert.ToString(".resx"))

            Dim ErrorMessage As String = String.Empty, ErrorText As String = String.Empty

            ErrorMessage = Convert.ToString(ResourceTable("InvalidSupervisorLastVisitDateMessage"), Nothing)
            ErrorMessage = If(String.IsNullOrEmpty(ErrorMessage), "Invalid Supervisor Last Visit Date.", ErrorMessage)

            Dim ResultOut As Boolean

            ValidationEnable = If((Boolean.TryParse(ResourceTable("ValidationEnable"), ResultOut)), ResultOut, True)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorSupervisorLastVisitDate, "TextBoxSupervisorLastVisitDate",
                                                           objShared.DateValidationExpression, ErrorMessage, ErrorText, ValidationEnable, ValidationGroup)

            ResourceTable = Nothing

        End Sub

        ''' <summary>
        ''' This is for validating data in server side before submitting the form
        ''' </summary>
        ''' <returns></returns>
        Public Function ValidateData() As Boolean
            If DropDownListCaseWorker.SelectedValue = "-1" Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Select Case Worker")
                Return False
            End If

            If DropDownListPriority.SelectedValue = "-1" Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please Select Priority")
                Return False
            End If

            If (Not String.IsNullOrEmpty(TextBoxNumberOfWeekDays.Text.Trim())) Then
                If (Not objShared.IsInteger(TextBoxNumberOfWeekDays.Text.Trim())) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Invalid Weekdays")
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

            LabelPriority.Text = Convert.ToString(ResourceTable("LabelPriority"), Nothing).Trim()
            LabelPriority.Text = If(String.IsNullOrEmpty(LabelPriority.Text), "Priority", LabelPriority.Text)

            LabelDischargeReason.Text = Convert.ToString(ResourceTable("LabelDischargeReason"), Nothing).Trim()
            LabelDischargeReason.Text = If(String.IsNullOrEmpty(LabelDischargeReason.Text), "Discharge Reason", LabelDischargeReason.Text)

            LabelDiagnosis.Text = Convert.ToString(ResourceTable("LabelDiagnosis"), Nothing).Trim()
            LabelDiagnosis.Text = If(String.IsNullOrEmpty(LabelDiagnosis.Text), "Diagnosis", LabelDiagnosis.Text)

            LabelDoctor.Text = Convert.ToString(ResourceTable("LabelDoctor"), Nothing).Trim()
            LabelDoctor.Text = If(String.IsNullOrEmpty(LabelDoctor.Text), "Doctor", LabelDoctor.Text)

            LabelUpdateDate.Text = Convert.ToString(ResourceTable("LabelUpdateDate"), Nothing).Trim()
            LabelUpdateDate.Text = If(String.IsNullOrEmpty(LabelUpdateDate.Text), "Update Date:", LabelUpdateDate.Text)

            LabelUpdateBy.Text = Convert.ToString(ResourceTable("LabelUpdateBy"), Nothing).Trim()
            LabelUpdateBy.Text = If(String.IsNullOrEmpty(LabelUpdateBy.Text), "Update By:", LabelUpdateBy.Text)

            LabelClientType.Text = Convert.ToString(ResourceTable("LabelClientType"), Nothing).Trim()
            LabelClientType.Text = If(String.IsNullOrEmpty(LabelClientType.Text), "Type:", LabelClientType.Text)

            LabelCaseWorker.Text = Convert.ToString(ResourceTable("LabelCaseWorker"), Nothing).Trim()
            LabelCaseWorker.Text = If(String.IsNullOrEmpty(LabelCaseWorker.Text), "Case Worker", LabelCaseWorker.Text)

            LabelUnits.Text = Convert.ToString(ResourceTable("LabelUnits"), Nothing).Trim()
            LabelUnits.Text = If(String.IsNullOrEmpty(LabelUnits.Text), "Units:", LabelUnits.Text)

            LabelNumberOfWeekDays.Text = Convert.ToString(ResourceTable("LabelNumberOfWeekDays"), Nothing).Trim()
            LabelNumberOfWeekDays.Text = If(String.IsNullOrEmpty(LabelNumberOfWeekDays.Text), "# of Days:", LabelNumberOfWeekDays.Text)

            LabelSupervisorLastVisitDate.Text = Convert.ToString(ResourceTable("LabelSupervisorLastVisitDate"), Nothing).Trim()
            LabelSupervisorLastVisitDate.Text = If(String.IsNullOrEmpty(LabelSupervisorLastVisitDate.Text), "Last Sup Visit Date:", LabelSupervisorLastVisitDate.Text)

            LabelSuppliesOrEquipment.Text = Convert.ToString(ResourceTable("LabelSuppliesOrEquipment"), Nothing).Trim()
            LabelSuppliesOrEquipment.Text = If(String.IsNullOrEmpty(LabelSuppliesOrEquipment.Text), "Supplies/ Equipment:", LabelSuppliesOrEquipment.Text)

            ResourceTable = Nothing

        End Sub

        ''' <summary>
        ''' Binding Doctor Drop Down List
        ''' </summary>
        Private Sub BindDoctorDropDownList()

            Dim objBLDoctor As New BLDoctor()
            DropDownListDoctor.DataSource = objBLDoctor.SelectDoctor(objShared.ConVisitel, objShared.CompanyId)
            objBLDoctor = Nothing

            DropDownListDoctor.DataTextField = "DoctorName"
            DropDownListDoctor.DataValueField = "DoctorId"
            DropDownListDoctor.DataBind()

            DropDownListDoctor.Items.Insert(0, New ListItem("Please Select...", "-1"))

        End Sub

        ''' <summary>
        ''' Controls Filling Out with data
        ''' </summary>
        ''' <param name="objClientBasicInfoDataObject"></param>
        ''' <remarks></remarks>
        Public Sub ClientBasicInfoFillOutData(ByRef objClientBasicInfoDataObject As ClientBasicInfoDataObject)

            DropDownListPriority.SelectedValue = If(objClientBasicInfoDataObject.Priority.Equals(Int16.MinValue), "-1", objClientBasicInfoDataObject.Priority)

            TextBoxUpdateDate.Text = If((String.IsNullOrEmpty(Convert.ToString(objClientBasicInfoDataObject.UpdateDate, Nothing))),
                                                String.Empty, objClientBasicInfoDataObject.UpdateDate)

            TextBoxUpdateBy.Text = objClientBasicInfoDataObject.UpdateBy

        End Sub

        Public Sub ClientTreatmentInfoFillOutData(ByRef objClientTreatmentInfoDataObject As ClientTreatmentInfoDataObject)

            DropDownListDischargeReason.SelectedIndex = DropDownListDischargeReason.Items.IndexOf(DropDownListDischargeReason.Items.FindByValue(
                                                             Convert.ToString(objClientTreatmentInfoDataObject.DischargeReason)))
            TextBoxDiagnosis.Text = objClientTreatmentInfoDataObject.Diagnosis

            DropDownListDoctor.SelectedIndex = DropDownListDoctor.Items.IndexOf(DropDownListDoctor.Items.FindByValue(
                                                   Convert.ToString(objClientTreatmentInfoDataObject.DoctorId)))


        End Sub

        Public Sub ClientCaseInfoFillOutData(ByRef objClientCaseDataObject As ClientCaseDataObject)

            HiddenFieldClientId.Value = objClientCaseDataObject.ClientId

            DropDownListClientType.SelectedIndex = DropDownListClientType.Items.IndexOf(DropDownListClientType.Items.FindByValue(
                                                   Convert.ToString(objClientCaseDataObject.Type)))

            DropDownListCaseWorker.SelectedIndex = DropDownListCaseWorker.Items.IndexOf(DropDownListCaseWorker.Items.FindByValue(
                                                   Convert.ToString(objClientCaseDataObject.CaseWorkerId)))

            TextBoxUnits.Text = Convert.ToString(objClientCaseDataObject.Units, Nothing)
            TextBoxNumberOfWeekDays.Text = If(objClientCaseDataObject.WeekDays.Equals(Int32.MinValue), String.Empty, Convert.ToString(objClientCaseDataObject.WeekDays))
        End Sub

        Public Sub ClientCaseCareInfoFillOutData(ByRef objClientCaseCareInfoDataObject As ClientCaseCareInfoDataObject)
            TextBoxSupervisorLastVisitDate.Text = If((String.IsNullOrEmpty(Convert.ToString(objClientCaseCareInfoDataObject.SupervisorLastVisitDate, Nothing))),
                                                      String.Empty, objClientCaseCareInfoDataObject.SupervisorLastVisitDate)
            TextBoxSuppliesOrEquipment.Text = objClientCaseCareInfoDataObject.Supplies

        End Sub

        ''' <summary>
        ''' Clearing controls value
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ClearControls()

            
        End Sub

        ''' <summary>
        ''' This is for retrieving data
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub GetData()
            objShared.BindCaseWorkerDropDownList(DropDownListCaseWorker, objShared.CompanyId)
            objShared.BindPriorityDropDownList(DropDownListPriority)
            BindDischargeReasonDropDownList()
            BindDoctorDropDownList()
            objShared.BindClientTypeDropDownList(DropDownListClientType, objShared.CompanyId, EnumDataObject.ClientTypeListFor.Individual)
        End Sub

        Private Sub DefineControlTextLength()
            objShared.SetControlTextLength(TextBoxDiagnosis, 255)
            objShared.SetControlTextLength(TextBoxNumberOfWeekDays, 10)
            objShared.SetControlTextLength(TextBoxSuppliesOrEquipment, 250)
            objShared.SetControlTextLength(TextBoxUnits, 8)
        End Sub

        Public Function GetDiagnosisTextBox() As TextBox
            Return Me.TextBoxDiagnosis
        End Function

        Public Sub DataFoSaveBasicInfo(ByRef objClientBasicInfoDataObject As ClientBasicInfoDataObject)
            objClientBasicInfoDataObject.Priority = Convert.ToInt16(DropDownListPriority.SelectedValue)
        End Sub

        Public Sub DataFoSaveTreatmentInfo(ByRef objClientTreatmentInfoDataObject As ClientTreatmentInfoDataObject)
            '#Region "Client Treatment Information"
            objClientTreatmentInfoDataObject.DischargeReason = Convert.ToInt32(DropDownListDischargeReason.SelectedValue)
            objClientTreatmentInfoDataObject.Diagnosis = Convert.ToString(TextBoxDiagnosis.Text, Nothing).Trim()

            objClientTreatmentInfoDataObject.DoctorId = Convert.ToInt32(DropDownListDoctor.SelectedValue)

            '#End Region
        End Sub

        Public Sub DataFoSaveClientCaseCareInfo(ByRef objClientCaseCareInfoDataObject As ClientCaseCareInfoDataObject)
            '#Region "Client Case Care Information"
            objClientCaseCareInfoDataObject.SupervisorLastVisitDate = Convert.ToString(TextBoxSupervisorLastVisitDate.Text, Nothing).Trim()

            objClientCaseCareInfoDataObject.Supplies = Convert.ToString(TextBoxSuppliesOrEquipment.Text, Nothing).Trim()

            '#End Region
        End Sub

        Public Sub DataFoSaveClientCaseInfo(ByRef objClientCaseDataObject As ClientCaseDataObject)

            '#Region "Client Case Information"

            objClientCaseDataObject.Type = Convert.ToInt32(DropDownListClientType.SelectedValue)
            objClientCaseDataObject.CaseWorkerId = Convert.ToInt32(DropDownListCaseWorker.SelectedValue)

            objClientCaseDataObject.Units = If((String.IsNullOrEmpty(TextBoxUnits.Text.Trim())), String.Empty, Convert.ToString(TextBoxUnits.Text.Trim(), Nothing))
            objClientCaseDataObject.WeekDays = If((String.IsNullOrEmpty(TextBoxNumberOfWeekDays.Text.Trim())),
                                                    objClientCaseDataObject.WeekDays,
                                                    Convert.ToInt32(TextBoxNumberOfWeekDays.Text.Trim()))
            '#End Region

        End Sub

        Public Sub GetClientCaseId(ByRef objClientCaseDataObject As ClientCaseDataObject)
            objClientCaseDataObject.ClientId = Convert.ToInt64(HiddenFieldClientId.Value)
        End Sub

        Public Function GetHiddenFieldClientCaseId() As String
            Return HiddenFieldClientId.Value
        End Function

        Public Sub SetClientCaseId()
            HiddenFieldClientId.Value = Convert.ToString(Int64.MinValue)
        End Sub

        Public Function GetClientType() As String
            Return DropDownListClientType.SelectedValue
        End Function


        Private Sub SetControlToolTip()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("Setup", ControlName & Convert.ToString(".resx"))

            Dim DateToolTip As String = Convert.ToString(ResourceTable("DateFieldToolTip"), Nothing)

            DateToolTip = If(String.IsNullOrEmpty(DateToolTip), "Example: 10/27/2015", DateToolTip)

            TextBoxSupervisorLastVisitDate.ToolTip = DateToolTip

            ResourceTable = Nothing
        End Sub

        ''' <summary>
        ''' Binding Discharge Reason Drop Down List
        ''' </summary>
        Private Sub BindDischargeReasonDropDownList()

            Dim objBLDischargeReason As New BLDischargeReason()
            DropDownListDischargeReason.DataSource = objBLDischargeReason.SelectDischargeReason(objShared.ConVisitel, objShared.CompanyId)
            objBLDischargeReason = Nothing

            DropDownListDischargeReason.DataTextField = "Name"
            DropDownListDischargeReason.DataValueField = "IdNumber"
            DropDownListDischargeReason.DataBind()

            DropDownListDischargeReason.Items.Insert(0, New ListItem("Please Select...", "-1"))

        End Sub

        ''' <summary>
        ''' Clearing controls value
        ''' </summary>
        Public Sub ClearClientCaseInfoControl()

            '**********************************************Client Case[Start]*********************************

            '#Region "Client Case Information"

            DropDownListCaseWorker.SelectedIndex = 0

            TextBoxUnits.Text = objShared.InlineAssignHelper(TextBoxNumberOfWeekDays.Text, String.Empty)

            '#End Region

            '#Region "Client Case Care Information"

            TextBoxSupervisorLastVisitDate.Text = objShared.InlineAssignHelper(TextBoxSuppliesOrEquipment.Text, String.Empty)

            '#End Region

            '**********************************************Client Case[End]*********************************

            HiddenFieldClientId.Value = Convert.ToString(Int64.MinValue)
        End Sub

        ''' <summary>
        ''' Clearing controls value
        ''' </summary>
        Public Sub ClearClientInfoControl()

            '#Region "Client Basic Information"

            TextBoxSupervisorLastVisitDate.Text = objShared.InlineAssignHelper(TextBoxSupervisorLastVisitDate.Text, objShared.InlineAssignHelper(TextBoxUpdateDate.Text,
                                        objShared.InlineAssignHelper(TextBoxUpdateBy.Text, String.Empty)))

            DropDownListPriority.SelectedIndex = 0

            'ButtonIndividual.Text = "Search Client"

            '#End Region

            '#Region "Client Treatment Information"

            TextBoxDiagnosis.Text = String.Empty

            DropDownListDischargeReason.SelectedIndex = objShared.InlineAssignHelper(DropDownListDoctor.SelectedIndex, 0)

            '#End Region

            DropDownListClientType.SelectedIndex = 0

            HiddenFieldClientId.Value = Convert.ToString(Int64.MinValue)
        End Sub
    End Class
End Namespace
