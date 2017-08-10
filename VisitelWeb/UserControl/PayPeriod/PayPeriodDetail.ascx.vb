
#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Pay Period Detail
' Author: Anjan Kumar Paul
' Start Date: 02 Feb 2016
' End Date: 
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                02 Feb 2016     Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports System.Data.SqlClient
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports System.Linq.Expressions

Namespace Visitel.UserControl.PayPeriod
    Public Class PayPeriodDetailControl
        Inherits BaseUserControl

        Private ValidationEnable As Boolean
        Private ValidationGroup As String

        Private ControlName As String, EditButtonText As String, UpdateButtonText As String, CancelButtonText As String

        Private GridViewPageSize As Integer

        Private HourMinuteFormat As String, TimeFormat As String, HourMinute As String

        Private blLoneClientExport As Boolean
        Private blLoneCalendarExport As Boolean

        Private LabelIndividualId As Label, LabelAttendantId As Label, LabelPayPeriodId As Label, LabelSerial As Label
        Private TextBoxIndividual As TextBox, TextBoxAttendanceOrOfficeStaff As TextBox, TextBoxCalendarId As TextBox, TextBoxStartDate As TextBox, TextBoxEndDate As TextBox,
            TextBoxSpecialRate As TextBox, TextBoxInTime As TextBox, TextBoxOutTime As TextBox, TextBoxHourMinute As TextBox

        Private DropDownListAttendanceOrOfficeStaff As DropDownList

        Private RegularExpressionValidatorSpecialRate As RegularExpressionValidator, RegularExpressionValidatorHourMinute As RegularExpressionValidator,
            RegularExpressionValidatorInTime As RegularExpressionValidator, RegularExpressionValidatorOutTime As RegularExpressionValidator

        Private objShared As SharedWebControls

        Private CurrentRow As GridViewRow

        Protected Sub Page_Init(sender As Object, e As EventArgs)

            DirectCast(Me.Page.Master, IMyMasterPage).PageHeaderTitle = "Pay Period Detail"

            ControlName = "PayPeriodDetailControl"
            objShared = New SharedWebControls()
            objShared.ConnectionOpen()
            GetCaptionFromResource()
            InitializeControl()
        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)
            If Not IsPostBack Then
                Session.Add("SortingExpression", "IndividualName")
                CheckBoxSavePrompt.Checked = True
                GetData()
            End If

            blLoneClientExport = False
            blLoneCalendarExport = False

        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadCss("PayPeriod/" + ControlName)
            LoadJScript()

            If ((Not String.IsNullOrEmpty(HiddenFieldStartDate.Value)) And (Not String.IsNullOrEmpty(HiddenFieldEndDate.Value))) Then
                ButtonHospitalizedIndividuals.Attributes.Add("OnClick", "ShowHospitalizedIndividuals('" & Convert.ToDateTime(HiddenFieldStartDate.Value).ToString("yyyyMMdd") _
                                                         & "', '" & Convert.ToDateTime(HiddenFieldEndDate.Value).ToString("yyyyMMdd") & "'); return false;")
            End If

            ButtonHospitalizedIndividuals.UseSubmitBehavior = False
            ButtonHospitalizedIndividuals.ClientIDMode = UI.ClientIDMode.Static

        End Sub

        Private Sub btnAdd_Click(sender As Object, e As EventArgs)
            Response.Write("Hello World!")
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing
        End Sub

        Private Sub ButtonAll_Click(sender As Object, e As EventArgs)
            DropDownListSearchByIndividual.SelectedIndex = 0
            DropDownListCalendarId.SelectedIndex = 0
            DropDownListServiceDate.SelectedIndex = 0

            blLoneClientExport = False
            blLoneCalendarExport = False

            BindPayPeriodDetailGridView()
        End Sub

        Protected Sub OnCheckedChanged(sender As Object, e As EventArgs)

            Dim isUpdateVisible As Boolean = False

            Dim chk As CheckBox = TryCast(sender, CheckBox)

            ButtonSave.Enabled = If((chk.Checked), True, False)

            If chk.ID = "chkAll" Then
                For Each row As GridViewRow In GridViewPayPeriodDetail.Rows
                    If row.RowType = DataControlRowType.DataRow Then
                        row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked = chk.Checked
                    End If
                Next
            End If

            Dim chkAll As CheckBox = TryCast(GridViewPayPeriodDetail.HeaderRow.FindControl("chkAll"), CheckBox)
            chkAll.Checked = True
            For Each row As GridViewRow In GridViewPayPeriodDetail.Rows
                If row.RowType = DataControlRowType.DataRow Then

                    Dim isChecked As Boolean = row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked

                    ControlsOnSelect(row, isChecked)

                    If (isChecked) Then
                        SetHiddenControlValue(row)
                    End If

                    For i As Integer = 1 To row.Cells.Count - 1
                        'row.Cells(i).Controls.OfType(Of Label)().FirstOrDefault().Visible = Not isChecked
                        If row.Cells(i).Controls.OfType(Of TextBox)().ToList().Count > 0 Then
                            'row.Cells(i).Controls.OfType(Of TextBox)().FirstOrDefault().ReadOnly = Not isChecked

                            'LabelPayPeriodId = DirectCast(GridViewPayPeriodDetail.Rows(e.RowIndex).FindControl("LabelPayPeriodId"), Label)

                        End If
                        If row.Cells(i).Controls.OfType(Of DropDownList)().ToList().Count > 0 Then
                            row.Cells(i).Controls.OfType(Of DropDownList)().FirstOrDefault().Visible = isChecked
                        End If
                        If isChecked AndAlso Not isUpdateVisible Then
                            isUpdateVisible = True
                        End If
                        If Not isChecked Then
                            chkAll.Checked = False
                        End If
                    Next
                End If
            Next
            'btnUpdate.Visible = isUpdateVisible
        End Sub

        Private Sub ButtonSave_Click(sender As Object, e As EventArgs)

            Page.Validate()
            Page.Validate(ValidationGroup)

            Dim PayPeriodDetailList As New List(Of PayPeriodDetailDataObject)
            Dim PayPeriodDetailListErrorList As New List(Of PayPeriodDetailDataObject)

            Dim objPayPeriodDetailDataObject As PayPeriodDetailDataObject

            For Each row As GridViewRow In GridViewPayPeriodDetail.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    Dim isChecked As Boolean = row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked

                    If (isChecked) Then

                        objPayPeriodDetailDataObject = New PayPeriodDetailDataObject()

                        CurrentRow = DirectCast(GridViewPayPeriodDetail.Rows(row.RowIndex), GridViewRow)
                        AddToList(CurrentRow, PayPeriodDetailList, objPayPeriodDetailDataObject)
                        objPayPeriodDetailDataObject = Nothing

                    End If

                End If
            Next

            If (PayPeriodDetailList.Count.Equals(0)) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please select items to save")
                Return
            End If

            Dim objBLPayPeriodDetail As New BLPayPeriodDetail

            Dim SaveFailedCounter As Int16 = 0

            For Each vPayPeriod In PayPeriodDetailList
                Try
                    objBLPayPeriodDetail.UpdatePayPeriod(objShared.ConVisitel, vPayPeriod)
                Catch ex As SqlException

                    SaveFailedCounter = SaveFailedCounter + 1
                    vPayPeriod.Remarks = ex.Message
                    PayPeriodDetailListErrorList.Add(vPayPeriod)
                End Try
            Next

            objBLPayPeriodDetail = Nothing

            If (SaveFailedCounter > 0) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(SaveFailedCounter & " Records Failed to Save out of " & PayPeriodDetailList.Count)
                ViewState("SaveFailedRecord") = objShared.ToDataTable(PayPeriodDetailListErrorList)
            Else
                ViewState("SaveFailedRecord") = Nothing
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Saved Successfully")
                GridViewPayPeriodDetail.EditIndex = -1
                BindPayPeriodDetailGridView()
            End If

            ButtonViewError.Visible = If((SaveFailedCounter > 0), True, False)

        End Sub

        ''' <summary>
        ''' This would generate excel file containing error occured during database operation
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub ButtonViewError_Click(sender As Object, e As EventArgs)
            Dim dt As DataTable = DirectCast(ViewState("SaveFailedRecord"), DataTable)
            objShared.ExportExcel(dt, "Save Failed Records", "", "SaveFailedRecords")
        End Sub

        Private Sub ButtonClear_Click(sender As Object, e As EventArgs)

            DropDownListSearchByIndividual.SelectedIndex = 0
            DropDownListCalendarId.SelectedIndex = 0
            DropDownListServiceDate.SelectedIndex = 0
            ButtonViewError.Visible = False
            BindPayPeriodDetailGridView()

        End Sub

        Private Sub CheckBoxSavePrompt_CheckedChanged(sender As Object, e As EventArgs)
            BindPayPeriodDetailGridView()
        End Sub

        Private Sub DropDownListSearchByIndividual_OnSelectedIndexChanged(sender As Object, e As EventArgs)

            blLoneClientExport = True
            blLoneCalendarExport = False

            DropDownListCalendarId.SelectedIndex = 0
            DropDownListServiceDate.SelectedIndex = 0

            HiddenFieldClientId.Value = String.Empty
            HiddenFieldEmployeeId.Value = String.Empty

            BindPayPeriodDetailGridView()

        End Sub

        Private Sub DropDownListCalendarId_OnSelectedIndexChanged(sender As Object, e As EventArgs)

            BindPayPeriodDetailGridView()

            blLoneCalendarExport = True
            blLoneClientExport = False

            DropDownListSearchByIndividual.SelectedIndex = 0

        End Sub

        Private Sub DropDownListServiceDate_OnSelectedIndexChanged(sender As Object, e As EventArgs)

            DropDownListSearchByIndividual.SelectedIndex = 0
            DropDownListCalendarId.SelectedIndex = 0

            BindPayPeriodDetailGridView()

        End Sub

        Private Sub ButtonIndividualDetail_Click(sender As Object, e As EventArgs)
            Response.Redirect("~/Pages/ClientInfo.aspx?ClientId=" & HiddenFieldClientId.Value)
        End Sub

        Private Sub ButtonEmployeeDetail_Click(sender As Object, e As EventArgs)
            Response.Redirect("~/Pages/EmployeeInfo.aspx?EmployeeId=" & HiddenFieldEmployeeId.Value)
        End Sub

        Private Sub ButtonPopulateCareSummary_Click(sender As Object, e As EventArgs)

            Dim objPayPeriodDetailDataObject As New PayPeriodDetailDataObject

            objPayPeriodDetailDataObject.StartDate = Convert.ToString(HiddenFieldStartDate.Value, Nothing)
            objPayPeriodDetailDataObject.EndDate = Convert.ToString(HiddenFieldEndDate.Value, Nothing)
            objPayPeriodDetailDataObject.IndividualId = Convert.ToInt64(HiddenFieldClientId.Value)
            objPayPeriodDetailDataObject.CalendarId = Convert.ToInt64(HiddenFieldScheduleId.Value)
            objPayPeriodDetailDataObject.UpdateBy = Convert.ToString(objShared.UserId)

            Dim objBLPayPeriodDetail As New BLPayPeriodDetail
            Dim IsSuccess As Boolean = False

            Try
                objBLPayPeriodDetail.PopulateCareSummary(objShared.ConVisitel, objPayPeriodDetailDataObject, If((blLoneClientExport), 1, 0), If((blLoneCalendarExport), 1, 0))
                IsSuccess = True
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError("Unable to populate care summary")
            Finally
                objPayPeriodDetailDataObject = Nothing
                objBLPayPeriodDetail = Nothing
            End Try

            If (IsSuccess) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Care Summary populated successfully")
            End If

        End Sub

        Private Sub GridViewPayPeriodDetail_RowDataBound(sender As Object, e As GridViewRowEventArgs)

            If (e.Row.RowType.Equals(DataControlRowType.Header)) Then
                SetGridViewColumnHeaderText(e)
            End If

            If (e.Row.RowType.Equals(DataControlRowType.DataRow)) Then

                CurrentRow = DirectCast(e.Row, GridViewRow)

                GridControlJavascriptEvent(e)

                SetRegularExpression(CurrentRow)

                TextBoxHourMinute = DirectCast(CurrentRow.FindControl("TextBoxHourMinute"), TextBox)
                objShared.SetShadowTextIndicatorHourMinute(TextBoxHourMinute, HourMinuteFormat)

                If (Not TextBoxHourMinute Is Nothing) Then
                    HourMinute = TextBoxHourMinute.Text.Trim()
                    TextBoxHourMinute.Text = If((String.IsNullOrEmpty(HourMinute)), String.Empty, HourMinute.Split(":")(0) & ":" & HourMinute.Split(":")(1))
                End If

                objShared.SetControlTextLength(TextBoxHourMinute, 5)

                TextBoxInTime = DirectCast(CurrentRow.FindControl("TextBoxInTime"), TextBox)
                objShared.SetShadowTextIndicatorTime(TextBoxInTime, TimeFormat)

                objShared.SetControlTextLength(TextBoxInTime, 8)

                TextBoxOutTime = DirectCast(CurrentRow.FindControl("TextBoxOutTime"), TextBox)
                objShared.SetShadowTextIndicatorTime(TextBoxOutTime, TimeFormat)

                objShared.SetControlTextLength(TextBoxOutTime, 8)

                LabelAttendantId = DirectCast(CurrentRow.FindControl("LabelAttendantId"), Label)

                DropDownListAttendanceOrOfficeStaff = DirectCast(CurrentRow.FindControl("DropDownListAttendanceOrOfficeStaff"), DropDownList)

                If (Not DropDownListAttendanceOrOfficeStaff Is Nothing) Then

                    objShared.BindEmployeeDropDownList(DropDownListAttendanceOrOfficeStaff, objShared.CompanyId, False)

                    DropDownListAttendanceOrOfficeStaff.SelectedIndex = DropDownListAttendanceOrOfficeStaff.Items.IndexOf(
                        DropDownListAttendanceOrOfficeStaff.Items.FindByValue(Convert.ToString(LabelAttendantId.Text.Trim(), Nothing)))

                    DropDownListAttendanceOrOfficeStaff.Visible = False

                End If

                TextBoxSpecialRate = DirectCast(CurrentRow.FindControl("TextBoxSpecialRate"), TextBox)
                objShared.SetControlTextLength(TextBoxSpecialRate, 6)

                Dim SpecialRate As Decimal = 0
                If (Not TextBoxSpecialRate Is Nothing) Then
                    SpecialRate = TextBoxSpecialRate.Text.Trim()
                End If

                If (Not TextBoxSpecialRate Is Nothing) Then
                    TextBoxSpecialRate.Text = If(Not SpecialRate.Equals(0), objShared.GetFormattedPayrate(SpecialRate), String.Empty)
                End If

                '**************************Selecting First Row by Default[Start]***********************************
                If (GridViewPayPeriodDetail.Rows.Count > 0) Then
                    GridViewPayPeriodDetail.Rows(0).RowState = DataControlRowState.Selected
                    SetHiddenControlValue(CurrentRow)
                End If
                '**************************Selecting First Row by Default[End]***********************************

                '**************************Fill Out Drop Down and Associate selection on Edit Mode[Start]***********************************
                If ((e.Row.RowState & DataControlRowState.Edit) > 0) Then
                    DropDownListAttendanceOrOfficeStaff = DirectCast(CurrentRow.FindControl("DropDownListAttendanceOrOfficeStaff"), DropDownList)

                    If (Not DropDownListAttendanceOrOfficeStaff Is Nothing) Then

                        objShared.BindEmployeeDropDownList(DropDownListAttendanceOrOfficeStaff, objShared.CompanyId, False)

                        DropDownListAttendanceOrOfficeStaff.SelectedIndex = DropDownListAttendanceOrOfficeStaff.Items.IndexOf(
                            DropDownListAttendanceOrOfficeStaff.Items.FindByValue(Convert.ToString(LabelAttendantId.Text.Trim(), Nothing)))

                    End If
                End If
                '**************************Fill Out Drop Down and Associate selection on Edit Mode[End]***********************************
            End If

        End Sub

        Private Sub GridViewPayPeriodDetail_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
            GridViewPayPeriodDetail.PageIndex = e.NewPageIndex
            BindPayPeriodDetailGridView()
        End Sub

        Private Sub GridViewPayPeriodDetail_Sorting(sender As Object, e As GridViewSortEventArgs)
            Session.Add("SortingExpression", e.SortExpression)
            BindPayPeriodDetailGridView()
        End Sub

        ''' <summary>
        ''' Gridview Row Hover Color Set
        ''' </summary>
        ''' <param name="writer"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub Render(writer As HtmlTextWriter)
            For Each r As GridViewRow In GridViewPayPeriodDetail.Rows
                If r.RowType = DataControlRowType.DataRow Then
                    objShared.SetGridViewRowColor(r)
                End If
            Next
            MyBase.Render(writer)
        End Sub

        Private Sub AddToList(ByRef CurrentRow As GridViewRow, ByRef PayPeriodDetailList As List(Of PayPeriodDetailDataObject),
                              ByRef objPayPeriodDetailDataObject As PayPeriodDetailDataObject)

            Dim RowSerial As Int16

            LabelSerial = DirectCast(CurrentRow.FindControl("LabelSerial"), Label)
            RowSerial = Convert.ToInt16(LabelSerial.Text)

            DropDownListAttendanceOrOfficeStaff = DirectCast(CurrentRow.FindControl("DropDownListAttendanceOrOfficeStaff"), DropDownList)
            TextBoxHourMinute = DirectCast(CurrentRow.FindControl("TextBoxHourMinute"), TextBox)
            TextBoxInTime = DirectCast(CurrentRow.FindControl("TextBoxInTime"), TextBox)
            TextBoxOutTime = DirectCast(CurrentRow.FindControl("TextBoxOutTime"), TextBox)
            TextBoxSpecialRate = DirectCast(CurrentRow.FindControl("TextBoxSpecialRate"), TextBox)
            LabelPayPeriodId = DirectCast(CurrentRow.FindControl("LabelPayPeriodId"), Label)

            RegularExpressionValidatorHourMinute = DirectCast(CurrentRow.FindControl("RegularExpressionValidatorHourMinute"), RegularExpressionValidator)
            RegularExpressionValidatorInTime = DirectCast(CurrentRow.FindControl("RegularExpressionValidatorInTime"), RegularExpressionValidator)
            RegularExpressionValidatorOutTime = DirectCast(CurrentRow.FindControl("RegularExpressionValidatorOutTime"), RegularExpressionValidator)
            RegularExpressionValidatorSpecialRate = DirectCast(CurrentRow.FindControl("RegularExpressionValidatorSpecialRate"), RegularExpressionValidator)

            If ((Page.IsValid) And (ValidateData(RowSerial))) Then

                objPayPeriodDetailDataObject.PayPeriodId = Convert.ToInt32(LabelPayPeriodId.Text.Trim())
                objPayPeriodDetailDataObject.AttendantId = Convert.ToInt64(DropDownListAttendanceOrOfficeStaff.SelectedValue)
                objPayPeriodDetailDataObject.HourMinutes = TextBoxHourMinute.Text.Trim()

                objPayPeriodDetailDataObject.InTime = If(String.IsNullOrEmpty(Convert.ToString(TextBoxInTime.Text, Nothing).Trim()),
                                                            objPayPeriodDetailDataObject.InTime,
                                                            objShared.FormatTime(TextBoxInTime.Text.Trim()))

                objPayPeriodDetailDataObject.OutTime = If(String.IsNullOrEmpty(Convert.ToString(TextBoxOutTime.Text, Nothing).Trim()),
                                                           objPayPeriodDetailDataObject.OutTime,
                                                           objShared.FormatTime(TextBoxOutTime.Text.Trim()))

                If (String.IsNullOrEmpty(TextBoxSpecialRate.Text.Trim())) Then
                    TextBoxSpecialRate.Text = 0
                End If

                objPayPeriodDetailDataObject.SpecialRate = Convert.ToDecimal(objShared.GetReFormattedPayrate(TextBoxSpecialRate.Text.Trim()), Nothing)

                objPayPeriodDetailDataObject.CompanyId = objShared.CompanyId
                objPayPeriodDetailDataObject.UpdateBy = objShared.UserId

                PayPeriodDetailList.Add(objPayPeriodDetailDataObject)

            End If

        End Sub

        ''' <summary>
        ''' Gridview Control Javascript Event Registering
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub GridControlJavascriptEvent(ByRef e As GridViewRowEventArgs)

            CurrentRow = DirectCast(e.Row, GridViewRow)

            'ButtonUpdate = DirectCast(CurrentRow.FindControl("ButtonUpdate"), Button)

            'If (CheckBoxSavePrompt.Checked) Then
            '    If (Not ButtonUpdate Is Nothing) Then
            '        ButtonUpdate.Attributes.Add("OnClick", "return confirm('Are you sure?');")
            '    End If
            'End If

            TextBoxIndividual = DirectCast(CurrentRow.FindControl("TextBoxIndividual"), TextBox)

            If ((Not TextBoxIndividual Is Nothing) And (Not LabelIndividualId Is Nothing)) Then
                TextBoxIndividual.Attributes.Add("OnClick", "SetClientId(" & LabelIndividualId.Text.Trim() & ")")
            End If

            TextBoxAttendanceOrOfficeStaff = DirectCast(CurrentRow.FindControl("TextBoxAttendanceOrOfficeStaff"), TextBox)

            If ((Not TextBoxAttendanceOrOfficeStaff Is Nothing) And (Not LabelAttendantId Is Nothing)) Then
                TextBoxAttendanceOrOfficeStaff.Attributes.Add("OnClick", "SetEmployeeId(" & LabelAttendantId.Text.Trim() & ")")
            End If

        End Sub

        Private Sub SetRegularExpression(ByRef CurrentRow As GridViewRow)

            RegularExpressionValidatorInTime = DirectCast(CurrentRow.FindControl("RegularExpressionValidatorInTime"), RegularExpressionValidator)
            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorInTime, "TextBoxInTime", objShared.TimeValidationWithAMPMExpression, "*", "*")

            RegularExpressionValidatorOutTime = DirectCast(CurrentRow.FindControl("RegularExpressionValidatorOutTime"), RegularExpressionValidator)
            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorOutTime, "TextBoxOutTime", objShared.TimeValidationWithAMPMExpression, "*", "*")

            RegularExpressionValidatorHourMinute = DirectCast(CurrentRow.FindControl("RegularExpressionValidatorHourMinute"), RegularExpressionValidator)
            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorHourMinute, "TextBoxHourMinute", objShared.HourMinuteValidationExpression, "*", "*")

            RegularExpressionValidatorSpecialRate = DirectCast(CurrentRow.FindControl("RegularExpressionValidatorSpecialRate"), RegularExpressionValidator)
            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorSpecialRate, "TextBoxSpecialRate", objShared.DecimalValueWithDollarSign, "*", "*")

        End Sub

        ''' <summary>
        ''' Set Hidden Control value 
        ''' </summary>
        ''' <param name="CurrentRow"></param>
        ''' <remarks></remarks>
        Private Sub SetHiddenControlValue(ByRef CurrentRow As GridViewRow)

            TextBoxCalendarId = DirectCast(CurrentRow.FindControl("TextBoxCalendarId"), TextBox)
            TextBoxStartDate = DirectCast(CurrentRow.FindControl("TextBoxStartDate"), TextBox)
            TextBoxEndDate = DirectCast(CurrentRow.FindControl("TextBoxEndDate"), TextBox)
            LabelAttendantId = DirectCast(CurrentRow.FindControl("LabelAttendantId"), Label)
            LabelIndividualId = DirectCast(CurrentRow.FindControl("LabelIndividualId"), Label)

            If ((Not HiddenFieldEmployeeId Is Nothing) And (Not LabelAttendantId Is Nothing)) Then
                HiddenFieldEmployeeId.Value = LabelAttendantId.Text.Trim()
            End If

            If ((Not HiddenFieldClientId Is Nothing) And (Not LabelIndividualId Is Nothing)) Then
                HiddenFieldClientId.Value = LabelIndividualId.Text.Trim()
            End If

            If ((Not HiddenFieldScheduleId Is Nothing) And (Not TextBoxCalendarId Is Nothing)) Then
                HiddenFieldScheduleId.Value = Convert.ToInt64(TextBoxCalendarId.Text)
            End If

            If ((Not HiddenFieldStartDate Is Nothing) And (Not TextBoxStartDate Is Nothing)) Then
                HiddenFieldStartDate.Value = Convert.ToString(TextBoxStartDate.Text, Nothing).Trim()
            End If

            If ((Not HiddenFieldEndDate Is Nothing) And (Not TextBoxEndDate Is Nothing)) Then
                HiddenFieldEndDate.Value = Convert.ToString(TextBoxEndDate.Text, Nothing).Trim()
            End If

        End Sub

        ''' <summary>
        ''' This is for control events regristering and instantiate object globally
        ''' </summary>
        Private Sub InitializeControl()

            ButtonClear.ClientIDMode = ClientIDMode.Static
            ButtonSave.ClientIDMode = ClientIDMode.Static
            ButtonPopulateCareSummary.ClientIDMode = ClientIDMode.Static
            ButtonIndividualDetail.ClientIDMode = ClientIDMode.Static
            ButtonEmployeeDetail.ClientIDMode = ClientIDMode.Static

            'ButtonSave.ValidationGroup = ValidationGroup
            ButtonSave.CausesValidation = True
            ButtonSave.Enabled = False
            ButtonClear.CausesValidation = False

            DropDownListSearchByIndividual.AutoPostBack = True
            DropDownListCalendarId.AutoPostBack = True
            DropDownListServiceDate.AutoPostBack = True

            CheckBoxSavePrompt.AutoPostBack = True
            AddHandler CheckBoxSavePrompt.CheckedChanged, AddressOf CheckBoxSavePrompt_CheckedChanged

            AddHandler DropDownListSearchByIndividual.SelectedIndexChanged, AddressOf DropDownListSearchByIndividual_OnSelectedIndexChanged
            AddHandler DropDownListCalendarId.SelectedIndexChanged, AddressOf DropDownListCalendarId_OnSelectedIndexChanged
            AddHandler DropDownListServiceDate.SelectedIndexChanged, AddressOf DropDownListServiceDate_OnSelectedIndexChanged

            AddHandler ButtonViewError.Click, AddressOf ButtonViewError_Click
            ButtonViewError.Visible = False

            AddHandler ButtonAll.Click, AddressOf ButtonAll_Click
            AddHandler ButtonSave.Click, AddressOf ButtonSave_Click
            AddHandler ButtonClear.Click, AddressOf ButtonClear_Click

            'AddHandler btnAdd.Click, AddressOf btnAdd_Click

            AddHandler ButtonPopulateCareSummary.Click, AddressOf ButtonPopulateCareSummary_Click
            AddHandler ButtonIndividualDetail.Click, AddressOf ButtonIndividualDetail_Click
            AddHandler ButtonEmployeeDetail.Click, AddressOf ButtonEmployeeDetail_Click

            GridViewPayPeriodDetail.AutoGenerateColumns = False
            GridViewPayPeriodDetail.ShowHeaderWhenEmpty = True
            GridViewPayPeriodDetail.AllowPaging = True
            GridViewPayPeriodDetail.AllowSorting = True

            If (GridViewPayPeriodDetail.AllowPaging) Then
                GridViewPayPeriodDetail.PageSize = GridViewPageSize
            End If

            AddHandler GridViewPayPeriodDetail.RowDataBound, AddressOf GridViewPayPeriodDetail_RowDataBound
            AddHandler GridViewPayPeriodDetail.PageIndexChanging, AddressOf GridViewPayPeriodDetail_PageIndexChanging
            AddHandler GridViewPayPeriodDetail.Sorting, AddressOf GridViewPayPeriodDetail_Sorting

            ButtonPopulateCareSummary.ClientIDMode = ClientIDMode.Static

        End Sub

        ''' <summary>
        ''' In Time and Out Time data validation
        ''' </summary>
        ''' <param name="TextBoxInTime"></param>
        ''' <param name="TextBoxOutTime"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function InTimeOutTimeValidation(ByRef TextBoxInTime As TextBox, ByRef TextBoxOutTime As TextBox) As Boolean

            If (Not String.IsNullOrEmpty(Convert.ToString(TextBoxInTime.Text, Nothing).Trim()) _
                    And Not String.IsNullOrEmpty(Convert.ToString(TextBoxOutTime.Text, Nothing).Trim())) Then

                Dim InTime As String = String.Empty, OutTime As String = String.Empty
                Dim InTimeHour As String = String.Empty, InTimeMinute As String = String.Empty
                Dim OutTimeHour As String = String.Empty, OutTimeMinute As String = String.Empty

                InTime = TextBoxInTime.Text.Trim().ToUpper()
                OutTime = TextBoxOutTime.Text.Trim().ToUpper()

                InTimeHour = InTime.Split(" ")(0).Split(":")(0)
                InTimeMinute = InTime.Split(" ")(0).Split(":")(1)

                OutTimeHour = OutTime.Split(" ")(0).Split(":")(0)
                OutTimeMinute = OutTime.Split(" ")(0).Split(":")(1)

                Dim InTimePMCheck As Boolean = InTime.Split(" ")(1).Equals("PM")
                Dim OutTimePMCheck As Boolean = OutTime.Split(" ")(1).Equals("PM")
                Dim InTimeAMCheck As Boolean = InTime.Split(" ")(1).Equals("AM")
                Dim OutTimeAMCheck As Boolean = OutTime.Split(" ")(1).Equals("AM")

                If (Not InTimeAMCheck And Not InTimePMCheck) Then
                    Return False
                End If

                If (Not OutTimeAMCheck And Not OutTimePMCheck) Then
                    Return False
                End If

                If (InTimePMCheck) Then
                    InTimeHour = Convert.ToString(Convert.ToInt32(InTimeHour) + 12)
                End If

                If (OutTimePMCheck) Then
                    OutTimeHour = Convert.ToString(Convert.ToInt32(OutTimeHour) + 12)
                End If

                If (((InTimePMCheck And OutTimePMCheck) Or (InTimeAMCheck And OutTimeAMCheck)) _
                        And (New TimeSpan(InTimeHour, InTimeMinute, 0) > New TimeSpan(OutTimeHour, OutTimeMinute, 0))) Then

                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("In Time cannot be greater than Out Time")

                    Return False
                End If

            End If

            Return True

        End Function

        ''' <summary>
        ''' This is for retrieving data
        ''' </summary>
        Private Sub GetData()
            Try
                objShared.BindClientDropDownList(DropDownListSearchByIndividual, objShared.CompanyId, EnumDataObject.ClientListFor.PayPeriod)
                BindCalendarIdDropDownList()
                BindServiceDateDropDownList()
                BindPayPeriodDetailGridView()
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError("Unable to Fetch Data")
            End Try
        End Sub

        ''' <summary>
        ''' Gridview row selection and go for edit mode
        ''' </summary>
        ''' <param name="CurrentRow"></param>
        ''' <param name="isChecked"></param>
        ''' <remarks></remarks>
        Private Sub ControlsOnSelect(ByRef CurrentRow As GridViewRow, ByRef isChecked As Boolean)

            DropDownListAttendanceOrOfficeStaff = DirectCast(CurrentRow.FindControl("DropDownListAttendanceOrOfficeStaff"), 
                                                                    DropDownList)

            If (Not DropDownListAttendanceOrOfficeStaff Is Nothing) Then
                DropDownListAttendanceOrOfficeStaff.Visible = isChecked
                DropDownListAttendanceOrOfficeStaff.CssClass = "DropDownListAttendanceOrOfficeStaffEdit"
            End If

            TextBoxAttendanceOrOfficeStaff = DirectCast(CurrentRow.FindControl("TextBoxAttendanceOrOfficeStaff"), TextBox)
            If (Not TextBoxAttendanceOrOfficeStaff Is Nothing) Then
                TextBoxAttendanceOrOfficeStaff.Visible = Not isChecked
            End If

            TextBoxHourMinute = DirectCast(CurrentRow.FindControl("TextBoxHourMinute"), TextBox)
            If (Not TextBoxHourMinute Is Nothing) Then
                TextBoxHourMinute.ReadOnly = Not isChecked
                TextBoxHourMinute.CssClass = If((TextBoxHourMinute.ReadOnly), "TextBoxHourMinute", "TextBoxHourMinuteEdit")
            End If

            TextBoxInTime = DirectCast(CurrentRow.FindControl("TextBoxInTime"), TextBox)
            If (Not TextBoxInTime Is Nothing) Then
                TextBoxInTime.ReadOnly = Not isChecked
                TextBoxInTime.CssClass = If((TextBoxInTime.ReadOnly), "TextBoxInTime", "TextBoxInTimeEdit")
            End If

            TextBoxOutTime = DirectCast(CurrentRow.FindControl("TextBoxOutTime"), TextBox)
            If (Not TextBoxOutTime Is Nothing) Then
                TextBoxOutTime.ReadOnly = Not isChecked
                TextBoxOutTime.CssClass = If((TextBoxOutTime.ReadOnly), "TextBoxOutTime", "TextBoxOutTimeEdit")
            End If

            TextBoxSpecialRate = DirectCast(CurrentRow.FindControl("TextBoxSpecialRate"), TextBox)
            If (Not TextBoxSpecialRate Is Nothing) Then
                TextBoxSpecialRate.ReadOnly = Not isChecked
                TextBoxSpecialRate.CssClass = If((TextBoxSpecialRate.ReadOnly), "TextBoxSpecialRate", "TextBoxSpecialRateEdit")
            End If

        End Sub

        ''' <summary>
        ''' Data binding for Calendar Id DropDownList
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindCalendarIdDropDownList()

            Dim objBLPayPeriodDetail As New BLPayPeriodDetail

            Try
                objBLPayPeriodDetail.GetCalendarIdData(objShared.VisitelConnectionString, SqlDataSourceCalendarId)
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError("Unable to Fetch Calendar Id data")
            Finally
                objBLPayPeriodDetail = Nothing
            End Try

            DropDownListCalendarId.DataSourceID = "SqlDataSourceCalendarId"
            DropDownListCalendarId.DataTextField = "ScheduleId"
            DropDownListCalendarId.DataValueField = "ScheduleId"
            DropDownListCalendarId.DataBind()

            DropDownListCalendarId.Items.Insert(0, New ListItem("Please Select...", "-1"))

        End Sub

        ''' <summary>
        ''' Data binding for Service Date DropDownList
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindServiceDateDropDownList()

            Dim objBLPayPeriodDetail As New BLPayPeriodDetail

            Try
                objBLPayPeriodDetail.GetServiceDateData(objShared.VisitelConnectionString, SqlDataSourceServiceDate)
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError("Unable to Fetch Service Date data")
            Finally
                objBLPayPeriodDetail = Nothing
            End Try

            DropDownListServiceDate.DataSourceID = "SqlDataSourceServiceDate"
            DropDownListServiceDate.DataTextField = "ServiceDate"
            DropDownListServiceDate.DataValueField = "ServiceDate"
            DropDownListServiceDate.DataBind()

            DropDownListServiceDate.Items.Insert(0, New ListItem("Please Select...", "-1"))

        End Sub

        Private Sub CalculateTimeTotal(ByRef GridResults As List(Of PayPeriodDetailDataObject))

            Dim TotalHour As Int32 = 0, TotalMinute As Int32 = 0
            For Each vPayPeriod In GridResults
                TotalHour = TotalHour + Convert.ToInt32(vPayPeriod.HourMinutes.Split(":")(0))
                TotalMinute = TotalMinute + Convert.ToInt32(vPayPeriod.HourMinutes.Split(":")(1))
            Next

            If (TotalMinute >= 60) Then
                TotalHour = TotalHour + (TotalMinute / 60)
            End If

            TotalMinute = TotalMinute Mod 60

            TextBoxTotalTime.Text = TotalHour & ":" & TotalMinute

        End Sub

        ''' <summary>
        ''' Returns sorted list according to the sorting expression
        ''' </summary>
        ''' <param name="GridResults"></param>
        ''' <remarks></remarks>
        Private Sub GetSortedPeriodDetail(ByRef GridResults As List(Of PayPeriodDetailDataObject))

            Dim SortingExpression As String = Convert.ToString(Session("SortingExpression"), Nothing)

            TextBoxTotalTime.Text = String.Empty

            If GridResults IsNot Nothing Then
                If (GridResults.Count > 0) Then
                    CalculateTimeTotal(GridResults)
                End If

                Dim param = Expression.Parameter(GetType(PayPeriodDetailDataObject), SortingExpression)
                'Dim SortExpression = Expression.Lambda(Of Func(Of PayPeriodDetailDataObject, Object))(Expression.Convert(Expression.[Property](param, SortingExpression), GetType(Object)), param)
                Dim SortExpression = Expression.Lambda(Of Func(Of PayPeriodDetailDataObject, Object))(Expression.Convert(Expression.Property(param, SortingExpression), GetType(Object)), param)

                If GridViewSortDirection = SortDirection.Ascending Then

                    GridResults = GridResults.AsQueryable().OrderBy(SortExpression).ToList()
                    GridViewSortDirection = SortDirection.Descending
                Else
                    GridResults = GridResults.AsQueryable().OrderByDescending(SortExpression).ToList()
                    GridViewSortDirection = SortDirection.Ascending
                End If

            End If

        End Sub

        ''' <summary>
        ''' Data binding for Pay Period Detail GridView
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindPayPeriodDetailGridView()

            Dim GridResults As List(Of PayPeriodDetailDataObject) = Nothing
            Dim objBLPayPeriodDetail As New BLPayPeriodDetail

            Try
                GridResults = objBLPayPeriodDetail.SelectPayPeriod(objShared.ConVisitel,
                                                                Convert.ToInt64(DropDownListSearchByIndividual.SelectedValue),
                                                                Convert.ToInt64(DropDownListCalendarId.SelectedValue),
                                                                Convert.ToString(DropDownListServiceDate.SelectedValue, Nothing))

            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to fetch pay period detail")
            Finally
                objBLPayPeriodDetail = Nothing
            End Try

            GetSortedPeriodDetail(GridResults)

            GridViewPayPeriodDetail.DataSource = GridResults
            GridViewPayPeriodDetail.DataBind()

            GridResults = Nothing

        End Sub

        ''' <summary>
        ''' Loading Javascript
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub LoadJScript()

            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                        & " var LoaderImagePath ='" & objShared.GetLoaderImagePath & "'; " _
                        & " var CompanyId=" & objShared.CompanyId & "; " _
                        & " var PagePath ='" & objShared.GetPopupUrl("Pages/Popup/PayPeriodDetailHospitalizedIndividualsPopup.aspx") & "'; " _
                        & " var prm =''; " _
                        & " jQuery(document).ready(function () {" _
                        & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                        & "     prm.add_beginRequest(SetButtonActionProgress); " _
                        & "     prm.add_endRequest(EndRequest); " _
                        & "}); " _
                 & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

            LoadJS("JavaScript/PayPeriod/" & ControlName & ".js")

            If (blLoneClientExport) Then

                scriptBlock = "<script type='text/javascript'> " _
                          & " var CustomTargetButton ='ButtonPopulateCareSummary'; " _
                          & " var CustomDialogHeader ='Populate Care Summary'; " _
                          & " var CustomDialogConfirmMsg ='If data already exists for this pay period in Care Summary, " _
                          & "   they will be deleted before new records are inserted. Do you wish to continue?'; " _
                          & "</script>"
            Else
                scriptBlock = "<script type='text/javascript'> " _
                         & " var CustomTargetButton ='ButtonPopulateCareSummary'; " _
                         & " var CustomDialogHeader ='Populate Care Summary'; " _
                         & " var CustomDialogConfirmMsg ='If data already exists for this client, for this pay period in Care Summary, " _
                         & "    it will be deleted before new record is inserted. Do you wish to continue?'; " _
                         & "</script>"
            End If

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))
        End Sub

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("PayPeriod", ControlName & Convert.ToString(".resx"))

            LabelSearchByIndividual.Text = Convert.ToString(ResourceTable("LabelSearchByIndividual"), Nothing).Trim()
            LabelSearchByIndividual.Text = If(String.IsNullOrEmpty(LabelSearchByIndividual.Text), "Individual:", LabelSearchByIndividual.Text)

            ButtonAll.Text = Convert.ToString(ResourceTable("ButtonAll"), Nothing).Trim()
            ButtonAll.Text = If(String.IsNullOrEmpty(ButtonAll.Text), "All", ButtonAll.Text)

            LabelCalendarIdCaption.Text = Convert.ToString(ResourceTable("LabelCalendarIdCaption"), Nothing).Trim()
            LabelCalendarIdCaption.Text = If(String.IsNullOrEmpty(LabelCalendarIdCaption.Text), "Calendar Id:", LabelCalendarIdCaption.Text)

            LabelServiceDateCaption.Text = Convert.ToString(ResourceTable("LabelServiceDateCaption"), Nothing).Trim()
            LabelServiceDateCaption.Text = If(String.IsNullOrEmpty(LabelServiceDateCaption.Text), "Service Date:", LabelServiceDateCaption.Text)

            ButtonClear.Text = Convert.ToString(ResourceTable("ButtonClear"), Nothing).Trim()
            ButtonClear.Text = If(String.IsNullOrEmpty(ButtonClear.Text), "Clear", ButtonClear.Text)

            ButtonSave.Text = Convert.ToString(ResourceTable("ButtonSave"), Nothing).Trim()
            ButtonSave.Text = If(String.IsNullOrEmpty(ButtonSave.Text), "Save", ButtonSave.Text)

            EditButtonText = Convert.ToString(ResourceTable("EditButtonText"), Nothing).Trim()
            EditButtonText = If(String.IsNullOrEmpty(EditButtonText), "Edit", EditButtonText)

            UpdateButtonText = Convert.ToString(ResourceTable("UpdateButtonText"), Nothing).Trim()
            UpdateButtonText = If(String.IsNullOrEmpty(UpdateButtonText), "Update", UpdateButtonText)

            CancelButtonText = Convert.ToString(ResourceTable("CancelButtonText"), Nothing).Trim()
            CancelButtonText = If(String.IsNullOrEmpty(CancelButtonText), "Cancel", CancelButtonText)

            ButtonPopulateCareSummary.Text = Convert.ToString(ResourceTable("ButtonPopulateCareSummary"), Nothing).Trim()
            ButtonPopulateCareSummary.Text = If(String.IsNullOrEmpty(ButtonPopulateCareSummary.Text), "Populate Care Summary", ButtonPopulateCareSummary.Text)

            ButtonIndividualDetail.Text = Convert.ToString(ResourceTable("ButtonIndividualDetail"), Nothing).Trim()
            ButtonIndividualDetail.Text = If(String.IsNullOrEmpty(ButtonIndividualDetail.Text), "Individual Detail", ButtonIndividualDetail.Text)

            ButtonEmployeeDetail.Text = Convert.ToString(ResourceTable("ButtonEmployeeDetail"), Nothing).Trim()
            ButtonEmployeeDetail.Text = If(String.IsNullOrEmpty(ButtonEmployeeDetail.Text), "Employee Detail", ButtonEmployeeDetail.Text)

            LabelTotalTime.Text = Convert.ToString(ResourceTable("LabelTotalTime"), Nothing).Trim()
            LabelTotalTime.Text = If(String.IsNullOrEmpty(LabelTotalTime.Text), "Total Time:", LabelTotalTime.Text)

            CheckBoxSavePrompt.Text = Convert.ToString(ResourceTable("CheckBoxSavePrompt"), Nothing).Trim()
            CheckBoxSavePrompt.Text = If(String.IsNullOrEmpty(CheckBoxSavePrompt.Text), "Save Prompt", CheckBoxSavePrompt.Text)

            ButtonHospitalizedIndividuals.Text = Convert.ToString(ResourceTable("ButtonHospitalizedIndividuals"), Nothing).Trim()
            ButtonHospitalizedIndividuals.Text = If(String.IsNullOrEmpty(ButtonHospitalizedIndividuals.Text), "Hospitalized Individuals", ButtonHospitalizedIndividuals.Text)

            Dim ResultOut As Boolean

            ValidationEnable = If((Boolean.TryParse(ResourceTable("ValidationEnable"), ResultOut)), ResultOut, True)

            ValidationGroup = Convert.ToString(ResourceTable("ValidationGroup"), Nothing)
            ValidationGroup = If(String.IsNullOrEmpty(ValidationGroup), "PayPeriod", ValidationGroup)

            Dim ResultOutInteger As Integer

            GridViewPageSize = If((Integer.TryParse(ResourceTable("PageSize"), ResultOutInteger)), ResultOutInteger, 10)
            GridViewPageSize = If((GridViewPageSize < 1), 10, GridViewPageSize)

            TimeFormat = Convert.ToString(ResourceTable("TimeFormat"), Nothing).Trim()
            TimeFormat = If(String.IsNullOrEmpty(TimeFormat), "hh:mm am|pm", TimeFormat)

            HourMinuteFormat = Convert.ToString(ResourceTable("HourMinuteFormat"), Nothing).Trim()
            HourMinuteFormat = If(String.IsNullOrEmpty(HourMinuteFormat), "hh:mm", HourMinuteFormat)

            ResourceTable = Nothing

        End Sub

        ''' <summary>
        ''' Reading GridView header text from resource file
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub SetGridViewColumnHeaderText(ByRef e As GridViewRowEventArgs)

            CurrentRow = DirectCast(e.Row, GridViewRow)

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("PayPeriod", ControlName & Convert.ToString(".resx"))

            Dim LabelHeaderStartDate As Label = DirectCast(CurrentRow.FindControl("LabelHeaderStartDate"), Label)
            LabelHeaderStartDate.Text = Convert.ToString(ResourceTable("HeaderStartDate"), Nothing).Trim()
            LabelHeaderStartDate.Text = If(String.IsNullOrEmpty(LabelHeaderStartDate.Text), "Start Date", LabelHeaderStartDate.Text)

            Dim LabelHeaderEndDate As Label = DirectCast(CurrentRow.FindControl("LabelHeaderEndDate"), Label)
            LabelHeaderEndDate.Text = Convert.ToString(ResourceTable("HeaderEndDate"), Nothing).Trim()
            LabelHeaderEndDate.Text = If(String.IsNullOrEmpty(LabelHeaderEndDate.Text), "End Date", LabelHeaderEndDate.Text)

            Dim LabelHeaderCalendarId As Label = DirectCast(CurrentRow.FindControl("LabelHeaderCalendarId"), Label)
            LabelHeaderCalendarId.Text = Convert.ToString(ResourceTable("HeaderCalendarId"), Nothing).Trim()
            LabelHeaderCalendarId.Text = If(String.IsNullOrEmpty(LabelHeaderCalendarId.Text), "Cal ID", LabelHeaderCalendarId.Text)

            Dim ButtonHeaderIndividualName As Button = DirectCast(CurrentRow.FindControl("ButtonHeaderIndividualName"), Button)
            ButtonHeaderIndividualName.Text = Convert.ToString(ResourceTable("HeaderIndividual"), Nothing).Trim()
            ButtonHeaderIndividualName.Text = If(String.IsNullOrEmpty(ButtonHeaderIndividualName.Text), "Individual", ButtonHeaderIndividualName.Text)

            Dim ButtonHeaderAttendantName As Button = DirectCast(CurrentRow.FindControl("ButtonHeaderAttendantName"), Button)
            ButtonHeaderAttendantName.Text = Convert.ToString(ResourceTable("HeaderAttendanceOrOfficeStaff"), Nothing).Trim()
            ButtonHeaderAttendantName.Text = If(String.IsNullOrEmpty(ButtonHeaderAttendantName.Text), "Attendance/Office Staff", ButtonHeaderAttendantName.Text)

            Dim LabelHeaderServiceDate As Label = DirectCast(CurrentRow.FindControl("LabelHeaderServiceDate"), Label)
            LabelHeaderServiceDate.Text = Convert.ToString(ResourceTable("HeaderServiceDate"), Nothing).Trim()
            LabelHeaderServiceDate.Text = If(String.IsNullOrEmpty(LabelHeaderServiceDate.Text), "SVC Date", LabelHeaderServiceDate.Text)

            Dim LabelHeaderDayName As Label = DirectCast(CurrentRow.FindControl("LabelHeaderDayName"), Label)
            LabelHeaderDayName.Text = Convert.ToString(ResourceTable("HeaderDayName"), Nothing).Trim()
            LabelHeaderDayName.Text = If(String.IsNullOrEmpty(LabelHeaderDayName.Text), "Day", LabelHeaderDayName.Text)

            Dim LabelHeaderHourMinutes As Label = DirectCast(CurrentRow.FindControl("LabelHeaderHourMinutes"), Label)
            LabelHeaderHourMinutes.Text = Convert.ToString(ResourceTable("HeaderHourMinute"), Nothing).Trim()
            LabelHeaderHourMinutes.Text = If(String.IsNullOrEmpty(LabelHeaderHourMinutes.Text), "Hour Minute", LabelHeaderHourMinutes.Text)

            Dim LabelHeaderInTime As Label = DirectCast(CurrentRow.FindControl("LabelHeaderInTime"), Label)
            LabelHeaderInTime.Text = Convert.ToString(ResourceTable("HeaderInTime"), Nothing).Trim()
            LabelHeaderInTime.Text = If(String.IsNullOrEmpty(LabelHeaderInTime.Text), "In", LabelHeaderInTime.Text)

            Dim LabelHeaderOutTime As Label = DirectCast(CurrentRow.FindControl("LabelHeaderOutTime"), Label)
            LabelHeaderOutTime.Text = Convert.ToString(ResourceTable("HeaderOutTime"), Nothing).Trim()
            LabelHeaderOutTime.Text = If(String.IsNullOrEmpty(LabelHeaderOutTime.Text), "Out", LabelHeaderOutTime.Text)

            Dim LabelHeaderSpecialRate As Label = DirectCast(CurrentRow.FindControl("LabelHeaderSpecialRate"), Label)
            LabelHeaderSpecialRate.Text = Convert.ToString(ResourceTable("HeaderSpecialRate"), Nothing).Trim()
            LabelHeaderSpecialRate.Text = If(String.IsNullOrEmpty(LabelHeaderSpecialRate.Text), "Special Rate", LabelHeaderSpecialRate.Text)

            Dim LabelHeaderComments As Label = DirectCast(CurrentRow.FindControl("LabelHeaderComments"), Label)
            LabelHeaderComments.Text = Convert.ToString(ResourceTable("HeaderComments"), Nothing).Trim()
            LabelHeaderComments.Text = If(String.IsNullOrEmpty(LabelHeaderComments.Text), "Comments", LabelHeaderComments.Text)

            Dim LabelHeaderUpdateDate As Label = DirectCast(CurrentRow.FindControl("LabelHeaderUpdateDate"), Label)
            LabelHeaderUpdateDate.Text = Convert.ToString(ResourceTable("HeaderUpdateDate"), Nothing).Trim()
            LabelHeaderUpdateDate.Text = If(String.IsNullOrEmpty(LabelHeaderUpdateDate.Text), "Update Date", LabelHeaderUpdateDate.Text)

            Dim LabelHeaderUpdateBy As Label = DirectCast(CurrentRow.FindControl("LabelHeaderUpdateBy"), Label)
            LabelHeaderUpdateBy.Text = Convert.ToString(ResourceTable("HeaderUpdateBy"), Nothing).Trim()
            LabelHeaderUpdateBy.Text = If(String.IsNullOrEmpty(LabelHeaderUpdateBy.Text), "Update By", LabelHeaderUpdateBy.Text)

            ResourceTable = Nothing

        End Sub

        ''' <summary>
        ''' This is for validating data in server side before submitting the form
        ''' </summary>
        ''' <returns></returns>
        Private Function ValidateData(RowSerial As Int16) As Boolean

            Dim RowSerialPointerText As String

            RowSerialPointerText = " on Row Serial" & RowSerial

            If (LabelPayPeriodId Is Nothing) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Pay Period Id Not Found" & RowSerialPointerText)
                Return False
            End If

            If (DropDownListAttendanceOrOfficeStaff Is Nothing) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Attendant Id Not Found" & RowSerialPointerText)
                Return False
            End If

            If ((Not TextBoxHourMinute Is Nothing) And (Not RegularExpressionValidatorHourMinute Is Nothing)) Then
                If ((Not String.IsNullOrEmpty(TextBoxHourMinute.Text.Trim())) And (Not objShared.ValidateHourMinte(TextBoxHourMinute.Text.Trim()))) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Invalid Hour Minute" & RowSerialPointerText)
                    Return False
                End If
            Else
                Return False
            End If

            If ((Not TextBoxInTime Is Nothing) And (Not RegularExpressionValidatorInTime Is Nothing)) Then
                If ((Not String.IsNullOrEmpty(TextBoxInTime.Text.Trim())) And (Not objShared.ValidateTimeWithAMPM(TextBoxInTime.Text.Trim()))) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Invalid In Time" & RowSerialPointerText)
                    TextBoxInTime.Focus()
                    Return False
                End If
            Else
                Return False
            End If

            If ((Not TextBoxOutTime Is Nothing) And (Not RegularExpressionValidatorOutTime Is Nothing)) Then
                If ((Not String.IsNullOrEmpty(TextBoxOutTime.Text.Trim())) And (Not objShared.ValidateTimeWithAMPM(TextBoxOutTime.Text.Trim()))) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Invalid Out Time" & RowSerialPointerText)
                    TextBoxOutTime.Focus()
                    Return False
                End If
            Else
                Return False
            End If

            If ((Not TextBoxInTime Is Nothing) And (Not TextBoxOutTime Is Nothing)) Then
                If ((Not InTimeOutTimeValidation(TextBoxInTime, TextBoxOutTime))) Then
                    TextBoxInTime.Focus()
                    Return False
                End If
            Else
                Return False
            End If

            If ((Not TextBoxSpecialRate Is Nothing) And (Not RegularExpressionValidatorSpecialRate Is Nothing)) Then
                If ((Not String.IsNullOrEmpty(TextBoxSpecialRate.Text.Trim()))) And (Not objShared.ValidatePayrate(TextBoxSpecialRate.Text.Trim())) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Invalid Special Rate" & RowSerialPointerText)
                    TextBoxSpecialRate.Focus()
                    Return False
                End If
            Else
                Return False
            End If

            Return True

        End Function
        
    End Class
End Namespace