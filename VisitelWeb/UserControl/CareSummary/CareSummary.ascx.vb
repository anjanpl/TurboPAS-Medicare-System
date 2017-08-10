
#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Care Summary
' Author: Anjan Kumar Paul
' Start Date: 15 July 2015
' End Date: 
' History:
'      Version                  Author                      Date             Reason 
'      1.0.0                                                15 July 2015     Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

'Not Fully Implemented; Need to work more

Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports System.Data.SqlClient
Imports System.Linq.Expressions
Imports System.IO
Imports System.Linq
Imports System.Net
Imports System.Globalization
Imports Microsoft.Security.Application


Namespace Visitel.UserControl.CareSummary
    Public Class CareSummaryControl
        Inherits BaseUserControl

        Private ValidationEnable As Boolean
        Private ValidationGroup As String

        Private ControlName As String, EditButtonText As String, UpdateButtonText As String, CancelButtonText As String, HourMinuteFormat As String, ButtonAddNewText As String

        Private GridViewPageSize As Integer

        Private PayPeriodValidationGroup As String, HourMinute As String

        Private LabelClientId As Label, LabelClientType As Label, LabelAttendantId As Label, LabelCareSummaryId As Label, LabelSerial As Label

        Private TextBoxIndividual As TextBox, TextBoxAttendanceOrOfficeStaff As TextBox, TextBoxCalendarId As TextBox, TextBoxStartDate As TextBox, TextBoxEndDate As TextBox,
            TextBoxBillTime As TextBox, TextBoxAdjustedBillTime As TextBox, TextBoxTimesheet As TextBox, TextBoxClient As TextBox, TextBoxIndividualType As TextBox,
            TextBoxUpdateDate As TextBox, TextBoxUpdateBy As TextBox, TextBoxEDIFile As TextBox, TextBoxEDIUpdateDate As TextBox, TextBoxEDIUpdateBy As TextBox

        Private DropDownListAttendanceOrOfficeStaff As DropDownList

        Private CheckBoxBilled As CheckBox, CheckBoxBilledAll As CheckBox, CheckBoxOriginalTimeSheet As CheckBox, CheckBoxOriginalTimeSheetAll As CheckBox,
            CheckBoxSelect As CheckBox

        Private ButtonCalendar As Button, ButtonHeaderStartDate As Button, ButtonHeaderEndDate As Button, ButtonHeaderClientName As Button, ButtonHeaderAttendantName As Button,
            ButtonAddNew As Button

        Private CurrentRow As GridViewRow
        Private objShared As SharedWebControls

        Protected Sub Page_Init(sender As Object, e As EventArgs)

            ControlName = "CareSummaryControl"

            objShared = New SharedWebControls()
            objShared.ConnectionOpen()

            DirectCast(Me.Page.Master, IMyMasterPage).PageHeaderTitle = "Care Summary"
            GetCaptionFromResource()
            InitializeControl()
        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)
            If Not IsPostBack Then
                Session.Add("SortingExpression", "ClientName")
                GetData()
            End If
        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadCss("CareSummary/" + ControlName)
            LoadJScript()
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing
        End Sub

        Private Sub GridViewCareSummary_RowDataBound(sender As Object, e As GridViewRowEventArgs)

            If (e.Row.RowType.Equals(DataControlRowType.Header)) Then

                SetGridViewColumnHeaderText(e)
                GridControlJavascriptEvent(e)
                GridViewColumnSortingEventRegister(e)

            End If

            If (e.Row.RowType.Equals(DataControlRowType.DataRow)) Then

                CurrentRow = DirectCast(e.Row, GridViewRow)

                CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)
                If (Not CheckBoxSelect Is Nothing) Then
                    CheckBoxSelect.ClientIDMode = ClientIDMode.Static
                    CheckBoxSelect.AutoPostBack = True
                    'AddHandler CheckBoxSelect.CheckedChanged, AddressOf OnCheckedChanged
                End If

                ButtonAddNew = DirectCast(CurrentRow.FindControl("ButtonAddNew"), Button)
                If (Not ButtonAddNew Is Nothing) Then
                    ButtonAddNew.Text = ButtonAddNewText
                End If

                TextBoxStartDate = DirectCast(CurrentRow.FindControl("TextBoxStartDate"), TextBox)
                If (Not TextBoxStartDate Is Nothing) Then
                    TextBoxStartDate.ReadOnly = True
                End If

                TextBoxEndDate = DirectCast(CurrentRow.FindControl("TextBoxEndDate"), TextBox)
                If (Not TextBoxEndDate Is Nothing) Then
                    TextBoxEndDate.ReadOnly = True
                End If

                TextBoxClient = DirectCast(CurrentRow.FindControl("TextBoxClient"), TextBox)
                If (Not TextBoxClient Is Nothing) Then
                    TextBoxClient.ReadOnly = True
                End If

                TextBoxBillTime = DirectCast(CurrentRow.FindControl("TextBoxBillTime"), TextBox)
                If (Not TextBoxBillTime Is Nothing) Then
                    TextBoxBillTime.Attributes.Add("placeholder", HourMinuteFormat)
                    TextBoxBillTime.ReadOnly = True
                End If

                TextBoxAdjustedBillTime = DirectCast(CurrentRow.FindControl("TextBoxAdjustedBillTime"), TextBox)
                If (Not TextBoxAdjustedBillTime Is Nothing) Then
                    TextBoxAdjustedBillTime.Attributes.Add("placeholder", HourMinuteFormat)
                    TextBoxAdjustedBillTime.ReadOnly = True
                End If

                CheckBoxOriginalTimeSheetAll = DirectCast(CurrentRow.FindControl("CheckBoxOriginalTimeSheetAll"), CheckBox)
                If (Not CheckBoxOriginalTimeSheetAll Is Nothing) Then
                    CheckBoxOriginalTimeSheetAll.Checked = False
                End If

                CheckBoxOriginalTimeSheet = DirectCast(CurrentRow.FindControl("CheckBoxOriginalTimeSheet"), CheckBox)
                If (Not CheckBoxOriginalTimeSheet Is Nothing) Then
                    CheckBoxOriginalTimeSheet.ClientIDMode = ClientIDMode.Static
                End If

                TextBoxTimesheet = DirectCast(CurrentRow.FindControl("TextBoxTimesheet"), TextBox)
                If (Not TextBoxTimesheet Is Nothing) Then
                    TextBoxTimesheet.Attributes.Add("placeholder", HourMinuteFormat)
                    TextBoxTimesheet.ReadOnly = True
                    TextBoxTimesheet.AutoPostBack = True
                    'AddHandler TextBoxTimesheet.TextChanged, AddressOf TextBoxTimesheet_TextChanged
                End If

                LabelAttendantId = DirectCast(CurrentRow.FindControl("LabelAttendantId"), Label)

                TextBoxAttendanceOrOfficeStaff = DirectCast(CurrentRow.FindControl("TextBoxAttendanceOrOfficeStaff"), TextBox)
                If ((Not TextBoxAttendanceOrOfficeStaff Is Nothing) And (Not LabelAttendantId Is Nothing)) Then
                    TextBoxAttendanceOrOfficeStaff.Attributes.Add("OnClick", "SetEmployeeId(" & LabelAttendantId.Text.Trim() & ")")
                    TextBoxAttendanceOrOfficeStaff.ReadOnly = True
                End If

                TextBoxIndividualType = DirectCast(CurrentRow.FindControl("TextBoxIndividualType"), TextBox)
                If (Not TextBoxIndividualType Is Nothing) Then
                    TextBoxIndividualType.ReadOnly = True
                End If

                TextBoxUpdateDate = DirectCast(CurrentRow.FindControl("TextBoxUpdateDate"), TextBox)
                If (Not TextBoxUpdateDate Is Nothing) Then
                    TextBoxUpdateDate.ReadOnly = True
                End If

                TextBoxUpdateBy = DirectCast(CurrentRow.FindControl("TextBoxUpdateBy"), TextBox)
                If (Not TextBoxUpdateBy Is Nothing) Then
                    TextBoxUpdateBy.ReadOnly = True
                End If

                TextBoxEDIFile = DirectCast(CurrentRow.FindControl("TextBoxEDIFile"), TextBox)
                If (Not TextBoxEDIFile Is Nothing) Then
                    TextBoxEDIFile.ReadOnly = True
                End If

                TextBoxEDIUpdateDate = DirectCast(CurrentRow.FindControl("TextBoxEDIUpdateDate"), TextBox)
                If (Not TextBoxEDIUpdateDate Is Nothing) Then
                    TextBoxEDIUpdateDate.ReadOnly = True
                End If

                TextBoxEDIUpdateBy = DirectCast(CurrentRow.FindControl("TextBoxEDIUpdateBy"), TextBox)
                If (Not TextBoxEDIUpdateBy Is Nothing) Then
                    TextBoxEDIUpdateBy.ReadOnly = True
                End If

                LabelClientId = DirectCast(CurrentRow.FindControl("LabelClientId"), Label)
                ButtonCalendar = DirectCast(CurrentRow.FindControl("ButtonCalendar"), Button)

                CheckBoxBilled = DirectCast(CurrentRow.FindControl("CheckBoxBilled"), CheckBox)

                If (Not CheckBoxBilled Is Nothing) Then
                    CheckBoxBilled.ClientIDMode = ClientIDMode.Static
                    'CheckBoxBilled.Checked = True
                    If (CheckBoxBilled.Checked) Then
                        CheckBoxBilled.Attributes.Add("OnClick", "confirm('This item has been billed. Unchecking it means it can be re-billed. " _
                                                     & "It also means it can be over-written with an update from Pay Period Detail. Are you sure you want to do this?');")
                    End If
                End If

                ButtonCalendar.Attributes.Add("OnClick", "SetCareSummaryPayPeriodParmeter('" & Convert.ToDateTime(TextBoxStartDate.Text.Trim()).ToString("yyyyMMdd") _
                                              & "', '" & Convert.ToDateTime(TextBoxEndDate.Text.Trim()).ToString("yyyyMMdd") & "', '" & LabelClientId.Text.Trim() _
                                              & "', '" & LabelAttendantId.Text.Trim() & "', '" & ButtonCalendar.Text.Trim() & "'); ShowCareSummaryPayPeriodDetail(); return false;")

                ButtonCalendar.UseSubmitBehavior = False
                ButtonCalendar.ClientIDMode = UI.ClientIDMode.Static

                'RowButtonDelete = DirectCast(e.Row.Cells(0).FindControl("ButtonDelete"), Button)

                'If ((Not RowButtonDelete Is Nothing) And (Not CheckBoxOriginalTimeSheet Is Nothing) And (Not CheckBoxBilled Is Nothing)) Then
                '    RowButtonDelete.Width = Unit.Pixel(45)
                '    RowButtonDelete.Height = Unit.Pixel(20)
                '    RowButtonDelete.CommandName = "Delete"

                '    If ((CheckBoxOriginalTimeSheet.Checked) Or (CheckBoxBilled.Checked)) Then
                '        RowButtonDelete.Attributes.Add("OnClick", "alert('You cannot delete an entry where timesheet is checked or billed.'); return false;")
                '    Else
                '        RowButtonDelete.Attributes.Add("OnClick", "return confirm('Are you sure you want to delete this record?');")
                '    End If
                '    RowButtonDelete.Text = "Delete"
                '    RowButtonDelete.CausesValidation = False
                'End If

                TextBoxIndividual = DirectCast(CurrentRow.FindControl("TextBoxIndividual"), TextBox)

                If ((Not TextBoxIndividual Is Nothing) And (Not LabelClientId Is Nothing)) Then
                    TextBoxIndividual.Attributes.Add("OnClick", "SetClientId(" & LabelClientId.Text.Trim() & ")")
                End If

                DropDownListAttendanceOrOfficeStaff = DirectCast(CurrentRow.FindControl("DropDownListAttendanceOrOfficeStaff"), 
                                                                   DropDownList)

                If (Not DropDownListAttendanceOrOfficeStaff Is Nothing) Then
                    DropDownListAttendanceOrOfficeStaff.Visible = False
                End If

                '**************************Selecting First Row by Default[Start]***********************************

                If (GridViewCareSummary.Rows.Count > 0) Then
                    GridViewCareSummary.Rows(0).RowState = DataControlRowState.Selected
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

        Private Sub GridViewCareSummary_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
            GridViewCareSummary.PageIndex = e.NewPageIndex
            BindCareSummaryGridView()
        End Sub

        Private Sub GridViewCareSummary_Sorting(sender As Object, e As GridViewSortEventArgs)
            Session.Add("SortingExpression", e.SortExpression)
            BindCareSummaryGridView()
        End Sub

        ''' <summary>
        ''' Not impletemented yet
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub GridViewCareSummary_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)

            CurrentRow = DirectCast(GridViewCareSummary.Rows(e.RowIndex), GridViewRow)

            LabelCareSummaryId = DirectCast(CurrentRow.FindControl("LabelCareSummaryId"), Label)
            TextBoxStartDate = DirectCast(CurrentRow.FindControl("TextBoxStartDate"), TextBox)
            TextBoxEndDate = DirectCast(CurrentRow.FindControl("TextBoxEndDate"), TextBox)
            LabelClientId = DirectCast(CurrentRow.FindControl("LabelClientId"), Label)
            CheckBoxBilled = DirectCast(CurrentRow.FindControl("CheckBoxBilled"), CheckBox)
            CheckBoxOriginalTimeSheet = DirectCast(CurrentRow.FindControl("CheckBoxOriginalTimeSheet"), CheckBox)
            LabelAttendantId = DirectCast(CurrentRow.FindControl("LabelAttendantId"), Label)

            Dim objBLCareSummary As New BLCareSummary
            Dim IsDeleted As Boolean = False

            Try
                Dim Description As String = String.Empty

                Description = Convert.ToString(TextBoxStartDate.Text.Trim(), Nothing) & "-" & Convert.ToString(TextBoxEndDate.Text.Trim(), Nothing) & " Individual:" _
                    & Convert.ToString(LabelClientId.Text.Trim(), Nothing) & " Attendant:" & Convert.ToString(LabelAttendantId.Text.Trim(), Nothing)

                'strDelete = " insert into delete_log(form_name, id_number,descr,action,userid, update_date) " & _
                '         " values('" & strName & "','" & txt_id & "',""" & strDescription & """,'Deleted','" & CurrentUser & "','" & Now() & "')"

                objBLCareSummary.DeleteCareSummaryInfo(objShared.ConVisitel, Convert.ToInt64(LabelCareSummaryId.Text.Trim()), objShared.UserId, objShared.CompanyId, Description)
                IsDeleted = True
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(ex.Message)
            End Try

            objBLCareSummary = Nothing

            If (IsDeleted) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Successfully Deleted")
                BindCareSummaryGridView()
            End If

        End Sub

        ''' <summary>
        ''' Gridview Select checkbox check/un-check event
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub OnCheckedChanged(sender As Object, e As EventArgs)

            Dim isUpdateVisible As Boolean = False

            Dim chk As CheckBox = DirectCast(sender, CheckBox)

            ButtonSave.Enabled = If((chk.Checked), True, False)

            If chk.ID = "chkAll" Then
                For Each row As GridViewRow In GridViewCareSummary.Rows
                    If row.RowType = DataControlRowType.DataRow Then
                        row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked = chk.Checked
                    End If
                Next
            End If

            Dim chkAll As CheckBox = DirectCast(GridViewCareSummary.HeaderRow.FindControl("chkAll"), CheckBox)
            chkAll.Checked = True
            For Each row As GridViewRow In GridViewCareSummary.Rows
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

        ''' <summary>
        ''' Gridview Row Hover Color Set
        ''' </summary>
        ''' <param name="writer"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub Render(writer As HtmlTextWriter)
            For Each r As GridViewRow In GridViewCareSummary.Rows
                If r.RowType = DataControlRowType.DataRow Then
                    r.Attributes("onmouseover") = "this.style.cursor='pointer';this.style.textDecoration='underline';this.style.backgroundColor='#D2E6F8';"
                    r.Attributes("onmouseout") = "this.style.textDecoration='none';this.style.backgroundColor='#ffffff';"
                    'r.ToolTip = "Click to select row"

                    'e.Row.Attributes.Add("onmouseover", "this.style.cursor='pointer';this.style.backgroundColor='#D2E6F8'")
                    'e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#ffffff'")

                    'r.Attributes("onclick") = Me.Page.ClientScript.GetPostBackClientHyperlink(Me.GridViewCareSummary, "Select$" & Convert.ToString(r.RowIndex), True)
                End If
            Next

            MyBase.Render(writer)
        End Sub

        Private Sub ButtonSearch_Click(sender As Object, e As EventArgs)
            BindCareSummaryGridView()
        End Sub

        Private Sub ButtonClearFilter_Click(sender As Object, e As EventArgs)

            DropDownListSearchByClient.SelectedIndex = 0
            DropDownListPayPeriod.SelectedIndex = 0
            DropDownListSearchByContract.SelectedIndex = 0
            ButtonViewError.Visible = False
            ListBoxTimeSheet.SelectedIndex = 2
            ListBoxBilled.SelectedIndex = 2

            BindCareSummaryGridView()

        End Sub

        Private Sub ButtonSave_Click(sender As Object, e As EventArgs)

            Page.Validate()
            Page.Validate(ValidationGroup)

            Dim CareSummaryList As New List(Of CareSummaryDataObject)
            Dim CareSummaryErrorList As New List(Of CareSummaryDataObject)

            Dim objCareSummaryDataObject As CareSummaryDataObject

            Dim chkAll As CheckBox = TryCast(GridViewCareSummary.HeaderRow.FindControl("chkAll"), CheckBox)

            For Each row As GridViewRow In GridViewCareSummary.Rows
                If row.RowType = DataControlRowType.DataRow Then

                    Dim isChecked As Boolean = row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked

                    If (isChecked) Then

                        objCareSummaryDataObject = New CareSummaryDataObject()

                        CurrentRow = DirectCast(GridViewCareSummary.Rows(row.RowIndex), GridViewRow)
                        AddToList(CurrentRow, CareSummaryList, objCareSummaryDataObject)
                        objCareSummaryDataObject = Nothing

                    End If
                End If
            Next

            If (CareSummaryList.Count.Equals(0)) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please select items to save")
                Return
            End If

            Dim objBLCareSummary As New BLCareSummary

            Dim SaveFailedCounter As Int16 = 0

            For Each CareSummary1 In CareSummaryList
                Try
                    objBLCareSummary.UpdateCareSummaryInfo(objShared.ConVisitel, CareSummary1, chkAll.Checked)
                Catch ex As SqlException
                    SaveFailedCounter = SaveFailedCounter + 1

                    If ex.Message.Contains("Duplicate Attendant") Then
                        CareSummary1.Remarks = "Unable to add new row due to same attendant on same client for the specified pay period."
                    Else
                        CareSummary1.Remarks = ex.Message
                    End If

                    CareSummaryErrorList.Add(CareSummary1)
                End Try
            Next

            objBLCareSummary = Nothing

            If (SaveFailedCounter > 0) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(SaveFailedCounter & " Records Failed to Save out of " & CareSummaryList.Count)
                ViewState("SaveFailedRecord") = objShared.ToDataTable(CareSummaryErrorList)
            Else
                ViewState("SaveFailedRecord") = Nothing
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Saved Successfully")
                GridViewCareSummary.EditIndex = -1
                BindCareSummaryGridView()
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

        Private Sub ButtonRefresh_Click(sender As Object, e As EventArgs)

            DropDownListSearchByClient.SelectedIndex = 0
            DropDownListPayPeriod.SelectedIndex = 0
            DropDownListSearchByContract.SelectedIndex = 0
            ButtonViewError.Visible = False
            BindCareSummaryGridView()

        End Sub

        ''' <summary>
        ''' Adding new row on Gridview  as a copy of current row
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub ButtonAddNew_OnClick(sender As Object, e As EventArgs)

            Dim ButtonAddNew As Button = DirectCast(sender, Button)
            CurrentRow = DirectCast(ButtonAddNew.NamingContainer, GridViewRow)

            LabelCareSummaryId = DirectCast(CurrentRow.FindControl("LabelCareSummaryId"), Label)

            If (LabelCareSummaryId Is Nothing) Then
                Return
            End If

            Dim OutResult As Int64
            Dim IntCareSummaryId As Int64
            Dim CareSummaryId As String = LabelCareSummaryId.Text.Trim()

            If (Int64.TryParse(CareSummaryId, OutResult)) Then
                IntCareSummaryId = Convert.ToInt64(CareSummaryId)
            Else
                Return
            End If

            Dim objBLCareSummary As New BLCareSummary
            Dim IsAdded As Boolean = False
            Try
                objBLCareSummary.InsertCareSummaryInfo(objShared.ConVisitel, IntCareSummaryId, objShared.UserId)
                IsAdded = True
            Catch ex As SqlException
                IsAdded = False
                If ex.Message.Contains("Duplicate Attendant") Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to add new row due to same attendant on same client for the specified pay period.")
                Else
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to record add.")
                End If

            Finally
                objBLCareSummary = Nothing
            End Try

            If (IsAdded) Then
                BindCareSummaryGridView()
            End If

        End Sub

        Private Sub ButtonIndividualDetail_Click(sender As Object, e As EventArgs)
            Response.Redirect("~/Pages/ClientInfo.aspx?ClientId=" & HiddenFieldClientId.Value)
        End Sub

        Private Sub ButtonEmployeeDetail_Click(sender As Object, e As EventArgs)
            Response.Redirect("~/Pages/EmployeeInfo.aspx?EmployeeId=" & HiddenFieldEmployeeId.Value)
        End Sub


        Private Sub ButtonAdjust_Click(sender As Object, e As EventArgs)

        End Sub

        ''' <summary>
        ''' Not Implemented yet
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub CheckBoxOriginalTimeSheetAll_CheckedChanged(sender As Object, e As EventArgs)

            Dim objBLCareSummary As New BLCareSummary
            Dim IsSaved As Boolean = False

            Dim objCareSummaryDataObject As New CareSummaryDataObject

            Try
                objBLCareSummary.UpdateCareSummaryInfo(objShared.ConVisitel, objCareSummaryDataObject, CheckBoxOriginalTimeSheetAll.Checked)
                IsSaved = True
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(ex.Message)
            Finally
                objBLCareSummary = Nothing
                objCareSummaryDataObject = Nothing
            End Try

            If (IsSaved) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Successfully Updated Original Timesheet")
                BindCareSummaryGridView()
            End If

        End Sub

        ''' <summary>
        ''' Timesheet text change; fill adjusted bill time and check original timesheet checkbox
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub TextBoxTimesheet_TextChanged(sender As Object, e As EventArgs)

            CurrentRow = DirectCast((DirectCast(sender, TextBox)).NamingContainer, GridViewRow)

            Dim TextBoxAdjustedBillTime As TextBox = DirectCast(CurrentRow.FindControl("TextBoxAdjustedBillTime"), TextBox)

            If (Not TextBoxAdjustedBillTime Is Nothing) Then
                TextBoxAdjustedBillTime.Text = DirectCast(sender, TextBox).Text
            End If

            Dim CheckBoxOriginalTimeSheet As CheckBox = DirectCast(CurrentRow.FindControl("CheckBoxOriginalTimeSheet"), CheckBox)
            If (Not CheckBoxOriginalTimeSheet Is Nothing) Then
                CheckBoxOriginalTimeSheet.Checked = True
            End If

        End Sub

        ''' <summary>
        ''' This is for control events regristering and instantiate object globally
        ''' </summary>
        Private Sub InitializeControl()

            HiddenFieldClientId.ClientIDMode = ClientIDMode.Static
            HiddenFieldEmployeeId.ClientIDMode = ClientIDMode.Static
            HiddenFieldStartDate.ClientIDMode = ClientIDMode.Static
            HiddenFieldEndDate.ClientIDMode = ClientIDMode.Static
            HiddenFieldScheduleId.ClientIDMode = ClientIDMode.Static


            ListBoxTimeSheet.SelectionMode = ListSelectionMode.Single
            ListBoxBilled.SelectionMode = ListSelectionMode.Single

            ButtonSave.ClientIDMode = ClientIDMode.Static

            'ButtonSave.ValidationGroup = ValidationGroup
            ButtonSave.CausesValidation = True
            ButtonSave.Enabled = False
            ButtonRefresh.CausesValidation = False

            AddHandler ButtonViewError.Click, AddressOf ButtonViewError_Click
            ButtonViewError.Visible = False
            AddHandler ButtonSearch.Click, AddressOf ButtonSearch_Click

            AddHandler ButtonClearFilter.Click, AddressOf ButtonClearFilter_Click
            AddHandler ButtonRefresh.Click, AddressOf ButtonRefresh_Click

            AddHandler ButtonSave.Click, AddressOf ButtonSave_Click
            AddHandler ButtonIndividualDetail.Click, AddressOf ButtonIndividualDetail_Click
            AddHandler ButtonEmployeeDetail.Click, AddressOf ButtonEmployeeDetail_Click
            AddHandler ButtonAdjust.Click, AddressOf ButtonAdjust_Click

            GridViewCareSummary.AutoGenerateColumns = False
            GridViewCareSummary.ShowHeaderWhenEmpty = True
            GridViewCareSummary.AllowPaging = True
            GridViewCareSummary.AllowSorting = True

            If (GridViewCareSummary.AllowPaging) Then
                GridViewCareSummary.PageSize = GridViewPageSize
            End If

            AddHandler GridViewCareSummary.RowDataBound, AddressOf GridViewCareSummary_RowDataBound
            AddHandler GridViewCareSummary.PageIndexChanging, AddressOf GridViewCareSummary_PageIndexChanging
            AddHandler GridViewCareSummary.Sorting, AddressOf GridViewCareSummary_Sorting

            ButtonRefresh.ClientIDMode = ClientIDMode.Static
            ButtonSave.ClientIDMode = ClientIDMode.Static
            ButtonIndividualDetail.ClientIDMode = ClientIDMode.Static
            ButtonEmployeeDetail.ClientIDMode = ClientIDMode.Static
            ButtonResetBatch.ClientIDMode = ClientIDMode.Static
            ButtonAdjust.ClientIDMode = ClientIDMode.Static
        End Sub

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("CareSummary", ControlName & Convert.ToString(".resx"))

            LabelCareSummaryInfo.Text = Convert.ToString(ResourceTable("LabelCareSummaryInfo"), Nothing).Trim()
            LabelCareSummaryInfo.Text = If(String.IsNullOrEmpty(LabelCareSummaryInfo.Text), "Care Summary", LabelCareSummaryInfo.Text)

            ButtonViewError.Text = Convert.ToString(ResourceTable("ButtonViewError"), Nothing).Trim()
            ButtonViewError.Text = If(String.IsNullOrEmpty(ButtonViewError.Text), "View Detail Error", ButtonViewError.Text)

            LabelSearchByClient.Text = Convert.ToString(ResourceTable("LabelSearchByClient"), Nothing).Trim()
            LabelSearchByClient.Text = If(String.IsNullOrEmpty(LabelSearchByClient.Text), "Client:", LabelSearchByClient.Text)

            LabelPayPeriodCaption.Text = Convert.ToString(ResourceTable("LabelPayPeriodCaption"), Nothing).Trim()
            LabelPayPeriodCaption.Text = If(String.IsNullOrEmpty(LabelPayPeriodCaption.Text), "Pay Period:", LabelPayPeriodCaption.Text)

            LabelContract.Text = Convert.ToString(ResourceTable("LabelContract"), Nothing).Trim()
            LabelContract.Text = If(String.IsNullOrEmpty(LabelContract.Text), "Contract", LabelContract.Text)

            LabelTimeSheet.Text = Convert.ToString(ResourceTable("LabelTimeSheet"), Nothing).Trim()
            LabelTimeSheet.Text = If(String.IsNullOrEmpty(LabelTimeSheet.Text), "Timesheet:", LabelTimeSheet.Text)

            LabelBilled.Text = Convert.ToString(ResourceTable("LabelBilled"), Nothing).Trim()
            LabelBilled.Text = If(String.IsNullOrEmpty(LabelBilled.Text), "Billed:", LabelBilled.Text)

            ButtonClearFilter.Text = Convert.ToString(ResourceTable("ButtonClearFilter"), Nothing).Trim()
            ButtonClearFilter.Text = If(String.IsNullOrEmpty(ButtonClearFilter.Text), "Clear", ButtonClearFilter.Text)

            ButtonSearch.Text = Convert.ToString(ResourceTable("ButtonSearch"), Nothing).Trim()
            ButtonSearch.Text = If(String.IsNullOrEmpty(ButtonSearch.Text), "Search", ButtonSearch.Text)

            ButtonRefresh.Text = Convert.ToString(ResourceTable("ButtonRefresh"), Nothing).Trim()
            ButtonRefresh.Text = If(String.IsNullOrEmpty(ButtonRefresh.Text), "Refresh", ButtonRefresh.Text)

            ButtonSave.Text = Convert.ToString(ResourceTable("ButtonSave"), Nothing).Trim()
            ButtonSave.Text = If(String.IsNullOrEmpty(ButtonSave.Text), "Save", ButtonSave.Text)

            ButtonIndividualDetail.Text = Convert.ToString(ResourceTable("ButtonIndividualDetail"), Nothing).Trim()
            ButtonIndividualDetail.Text = If(String.IsNullOrEmpty(ButtonIndividualDetail.Text), "Individual Detail", ButtonIndividualDetail.Text)

            ButtonEmployeeDetail.Text = Convert.ToString(ResourceTable("ButtonEmployeeDetail"), Nothing).Trim()
            ButtonEmployeeDetail.Text = If(String.IsNullOrEmpty(ButtonEmployeeDetail.Text), "Employee Detail", ButtonEmployeeDetail.Text)

            ButtonResetBatch.Text = Convert.ToString(ResourceTable("ButtonResetBatch"), Nothing).Trim()
            ButtonResetBatch.Text = If(String.IsNullOrEmpty(ButtonResetBatch.Text), "Reset Batch", ButtonResetBatch.Text)

            ButtonAdjust.Text = Convert.ToString(ResourceTable("ButtonAdjust"), Nothing).Trim()
            ButtonAdjust.Text = If(String.IsNullOrEmpty(ButtonAdjust.Text), "Adjust", ButtonAdjust.Text)

            CheckBoxSavePrompt.Text = Convert.ToString(ResourceTable("CheckBoxSavePrompt"), Nothing).Trim()
            CheckBoxSavePrompt.Text = If(String.IsNullOrEmpty(CheckBoxSavePrompt.Text), "Save Prompt", CheckBoxSavePrompt.Text)

            ButtonAddNewText = Convert.ToString(ResourceTable("ButtonAddNewText"), Nothing).Trim()
            ButtonAddNewText = If(String.IsNullOrEmpty(ButtonAddNewText), "Add New", ButtonAddNewText)

            EditButtonText = Convert.ToString(ResourceTable("EditButtonText"), Nothing).Trim()
            EditButtonText = If(String.IsNullOrEmpty(EditButtonText), "Edit", EditButtonText)

            UpdateButtonText = Convert.ToString(ResourceTable("UpdateButtonText"), Nothing).Trim()
            UpdateButtonText = If(String.IsNullOrEmpty(UpdateButtonText), "Update", UpdateButtonText)

            CancelButtonText = Convert.ToString(ResourceTable("CancelButtonText"), Nothing).Trim()
            CancelButtonText = If(String.IsNullOrEmpty(CancelButtonText), "Cancel", CancelButtonText)

            LabelTotalAdjustedTime.Text = Convert.ToString(ResourceTable("LabelTotalAdjustedTime"), Nothing).Trim()
            LabelTotalAdjustedTime.Text = If(String.IsNullOrEmpty(LabelTotalAdjustedTime.Text), "Total Adjusted Time:", LabelTotalAdjustedTime.Text)

            LabelTotalActualTime.Text = Convert.ToString(ResourceTable("LabelTotalActualTime"), Nothing).Trim()
            LabelTotalActualTime.Text = If(String.IsNullOrEmpty(LabelTotalActualTime.Text), "Total Actual Time:", LabelTotalActualTime.Text)

            Dim ResultOut As Boolean

            ValidationEnable = If((Boolean.TryParse(ResourceTable("ValidationEnable"), ResultOut)), ResultOut, True)

            ValidationGroup = Convert.ToString(ResourceTable("ValidationGroup"), Nothing)
            ValidationGroup = If(String.IsNullOrEmpty(ValidationGroup), "CareSummary", ValidationGroup)

            HourMinuteFormat = Convert.ToString(ResourceTable("HourMinuteFormat"), Nothing).Trim()
            HourMinuteFormat = If(String.IsNullOrEmpty(HourMinuteFormat), "hh:mm", HourMinuteFormat)

            Dim ResultOutInteger As Integer

            GridViewPageSize = If((Integer.TryParse(ResourceTable("PageSize"), ResultOutInteger)), ResultOutInteger, 10)
            GridViewPageSize = If((GridViewPageSize < 1), 10, GridViewPageSize)


            'Dim LabelNoDataFound As Label = DirectCast(GridViewCareSummary.Controls(0).Controls(0).FindControl("LabelNoDataFound"), Label)
            'LabelNoDataFound.Text = "Currently there are no records in system."

            ResourceTable = Nothing

        End Sub

        ''' <summary>
        ''' Loading Javascript
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub LoadJScript()

            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                        & " var CareSummaryPayPeriodDetailPath ='" & objShared.GetPopupUrl("Pages/Popup/CareSummaryPayPeriodDetailPopup.aspx") & "'; " _
                        & " var LoaderImagePath ='" & objShared.GetLoaderImagePath & "'; " _
                        & " var prm =''; " _
                        & " jQuery(document).ready(function () {" _
                        & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                        & "     prm.add_beginRequest(SetButtonActionProgress); " _
                        & "     prm.add_endRequest(EndRequest); " _
                        & "}); " _
                 & "</script>"
            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

            LoadJS("JavaScript/CareSummary/" & ControlName & ".js")
        End Sub

        ''' <summary>
        ''' This is for retrieving data
        ''' </summary>
        Private Sub GetData()

            Try
                objShared.BindClientDropDownList(DropDownListSearchByClient, objShared.CompanyId, EnumDataObject.ClientListFor.CareSummary)
                BindPayPeriodDropDownList()
                BindClientTypeDropDownList()
                BindTimeSheetListBox()
                BindBilledListBox()
                'BindCareSummaryGridView()
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to Fetch Data")
            End Try

        End Sub

        ''' <summary>
        ''' Data binding for Pay Period DropDownList
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindPayPeriodDropDownList()

            Dim objBLPayPeriodDetail As New BLPayPeriodDetail
            objBLPayPeriodDetail.GetPayPeriodData(objShared.VisitelConnectionString, SqlDataSourcePayPeriod,
                                                  EnumDataObject.EnumHelper.GetDescription(EnumDataObject.PayPeriodFor.CareSummary))
            objBLPayPeriodDetail = Nothing

            DropDownListPayPeriod.DataSourceID = "SqlDataSourcePayPeriod"
            DropDownListPayPeriod.DataTextField = "PayPeriod"
            DropDownListPayPeriod.DataValueField = "PayPeriod"
            DropDownListPayPeriod.DataBind()

            DropDownListPayPeriod.Items.Insert(0, New ListItem("Please Select...", "-1"))

        End Sub

        ''' <summary>
        ''' Binding Contract(Client Type) Drop Down List
        ''' </summary>
        Private Sub BindClientTypeDropDownList()

            DropDownListSearchByContract.DataSource = objShared.GetClientType(objShared.ConVisitel, objShared.CompanyId, EnumDataObject.ClientTypeListFor.CareSummary)

            DropDownListSearchByContract.DataTextField = "Name"
            DropDownListSearchByContract.DataValueField = "IdNumber"
            DropDownListSearchByContract.DataBind()

            DropDownListSearchByContract.Items.Insert(0, New ListItem("Please Select...", "-1"))

        End Sub

        ''' <summary>
        ''' Binding Time Sheet List Box
        ''' </summary>
        Private Sub BindTimeSheetListBox()

            Dim sortedDict = (From entry In EnumDataObject.Enumeration.GetAll(Of EnumDataObject.TimeSheet)() Order By entry.Value Descending Select entry)

            ListBoxTimeSheet.DataSource = sortedDict
            ListBoxTimeSheet.DataValueField = "Key"
            ListBoxTimeSheet.DataTextField = "Value"
            ListBoxTimeSheet.DataBind()

            ListBoxTimeSheet.SelectedIndex = 2

        End Sub

        ''' <summary>
        ''' Binding Billed ListBox
        ''' </summary>
        Private Sub BindBilledListBox()

            Dim sortedDict = (From entry In EnumDataObject.Enumeration.GetAll(Of EnumDataObject.Billed)() Order By entry.Value Descending Select entry)

            ListBoxBilled.DataSource = sortedDict
            ListBoxBilled.DataTextField = "Value"
            ListBoxBilled.DataValueField = "Key"
            ListBoxBilled.DataBind()

            ListBoxBilled.SelectedIndex = 2

        End Sub

        Private Sub AddToList(ByRef CurrentRow As GridViewRow, ByRef CareSummaryList As List(Of CareSummaryDataObject), ByRef objCareSummaryDataObject As CareSummaryDataObject)

            Dim OutResult As Int64
            Dim RowSerial As Int16

            LabelSerial = DirectCast(CurrentRow.FindControl("LabelSerial"), Label)
            RowSerial = Convert.ToInt16(LabelSerial.Text)

            LabelCareSummaryId = DirectCast(CurrentRow.FindControl("LabelCareSummaryId"), Label)
            LabelClientId = DirectCast(CurrentRow.FindControl("LabelClientId"), Label)
            LabelClientType = DirectCast(CurrentRow.FindControl("LabelClientType"), Label)
            TextBoxStartDate = DirectCast(CurrentRow.FindControl("TextBoxStartDate"), TextBox)
            TextBoxEndDate = DirectCast(CurrentRow.FindControl("TextBoxEndDate"), TextBox)
            TextBoxEndDate = DirectCast(CurrentRow.FindControl("TextBoxEndDate"), TextBox)
            CheckBoxBilled = DirectCast(CurrentRow.FindControl("CheckBoxBilled"), CheckBox)
            TextBoxAdjustedBillTime = DirectCast(CurrentRow.FindControl("TextBoxAdjustedBillTime"), TextBox)
            CheckBoxOriginalTimeSheet = DirectCast(CurrentRow.FindControl("CheckBoxOriginalTimeSheet"), CheckBox)
            TextBoxTimesheet = DirectCast(CurrentRow.FindControl("TextBoxTimesheet"), TextBox)
            DropDownListAttendanceOrOfficeStaff = DirectCast(CurrentRow.FindControl("DropDownListAttendanceOrOfficeStaff"), 
                                                            DropDownList)

            If ((Page.IsValid) And (ValidateData(RowSerial))) Then
                objCareSummaryDataObject.CareSummaryId = LabelCareSummaryId.Text.Trim()
                objCareSummaryDataObject.ClientId = LabelClientId.Text.Trim()
                objCareSummaryDataObject.ClientType = LabelClientType.Text.Trim()
                objCareSummaryDataObject.StartDate = TextBoxStartDate.Text.Trim()
                objCareSummaryDataObject.EndDate = TextBoxEndDate.Text.Trim()
                objCareSummaryDataObject.EndDate = TextBoxEndDate.Text.Trim()
                objCareSummaryDataObject.Billed = If((CheckBoxBilled.Checked), 1, 0)
                objCareSummaryDataObject.AdjustedBillTime = TextBoxAdjustedBillTime.Text.Trim()
                objCareSummaryDataObject.OriginalTimesheet = If((CheckBoxOriginalTimeSheet.Checked), 1, 0)
                objCareSummaryDataObject.TimesheetHourMinute = TextBoxTimesheet.Text.Trim()
                objCareSummaryDataObject.AttendantId = If((Int64.TryParse(DropDownListAttendanceOrOfficeStaff.SelectedValue, OutResult)),
                                                          Convert.ToInt64(DropDownListAttendanceOrOfficeStaff.SelectedValue),
                                                          objCareSummaryDataObject.AttendantId)

                objCareSummaryDataObject.CompanyId = objShared.CompanyId
                objCareSummaryDataObject.UpdateBy = objShared.UserId

                CareSummaryList.Add(objCareSummaryDataObject)

            End If
        End Sub

        Private Sub GridViewColumnSortingEventRegister(ByRef e As GridViewRowEventArgs)

            CurrentRow = DirectCast(e.Row, GridViewRow)

            ButtonHeaderStartDate = DirectCast(CurrentRow.FindControl("ButtonHeaderStartDate"), Button)

            If (Not ButtonHeaderStartDate Is Nothing) Then
                ButtonHeaderStartDate.CommandName = "Sort"
                ButtonHeaderStartDate.CommandArgument = "StartDate"
            End If

            ButtonHeaderEndDate = DirectCast(CurrentRow.FindControl("ButtonHeaderEndDate"), Button)

            If (Not ButtonHeaderEndDate Is Nothing) Then
                ButtonHeaderEndDate.CommandName = "Sort"
                ButtonHeaderEndDate.CommandArgument = "EndDate"
            End If

            ButtonHeaderClientName = DirectCast(CurrentRow.FindControl("ButtonHeaderClientName"), Button)

            If (Not ButtonHeaderClientName Is Nothing) Then
                ButtonHeaderClientName.CommandName = "Sort"
                ButtonHeaderClientName.CommandArgument = "ClientName"
            End If

            ButtonHeaderAttendantName = DirectCast(CurrentRow.FindControl("ButtonHeaderAttendantName"), Button)

            If (Not ButtonHeaderAttendantName Is Nothing) Then
                ButtonHeaderAttendantName.CommandName = "Sort"
                ButtonHeaderAttendantName.CommandArgument = "AttendantName"
            End If

        End Sub

        Private Sub CalculateTimeTotal(ByRef GridResults As List(Of CareSummaryDataObject))

            Dim TotalAdjusteHour As Int32 = 0, TotalAdjustedMinute As Int32 = 0, TotalActualHour As Int32 = 0, TotalActualMinute As Int32 = 0
            For Each CareSummary1 In GridResults
                If (Not String.IsNullOrEmpty(CareSummary1.AdjustedBillTime)) Then
                    TotalAdjusteHour = TotalAdjusteHour + Convert.ToInt32(CareSummary1.AdjustedBillTime.Split(":")(0))
                    TotalAdjustedMinute = TotalAdjustedMinute + Convert.ToInt32(CareSummary1.AdjustedBillTime.Split(":")(1))
                End If

                If (Not String.IsNullOrEmpty(CareSummary1.BillTime)) Then
                    TotalActualHour = TotalActualHour + Convert.ToInt32(CareSummary1.BillTime.Split(":")(0))
                    TotalActualMinute = TotalActualMinute + Convert.ToInt32(CareSummary1.BillTime.Split(":")(1))
                End If
            Next

            If (TotalAdjustedMinute >= 60) Then
                TotalAdjusteHour = TotalAdjusteHour + (TotalAdjustedMinute / 60)
            End If

            TotalAdjustedMinute = TotalAdjustedMinute Mod 60

            TextBoxTotalAdjustedTime.Text = TotalAdjusteHour & ":" & TotalAdjustedMinute

            If (TotalActualMinute >= 60) Then
                TotalActualHour = TotalActualHour + (TotalActualMinute / 60)
            End If

            TotalActualMinute = TotalActualMinute Mod 60

            TextBoxTotalAcutualTime.Text = TotalActualHour & ":" & TotalActualMinute

        End Sub

        ''' <summary>
        ''' Returns sorted list according to the sorting expression
        ''' </summary>
        ''' <param name="GridResults"></param>
        ''' <remarks></remarks>
        Private Sub GetSortedCareSummary(ByRef GridResults As List(Of CareSummaryDataObject))

            Dim SortingExpression As String = Convert.ToString(Session("SortingExpression"), Nothing)

            TextBoxTotalAdjustedTime.Text = String.Empty
            TextBoxTotalAcutualTime.Text = String.Empty

            If GridResults IsNot Nothing Then
                If (GridResults.Count > 0) Then
                    CalculateTimeTotal(GridResults)
                End If

                Dim param = Expression.Parameter(GetType(CareSummaryDataObject), SortingExpression)
                Dim SortExpression = Expression.Lambda(Of Func(Of CareSummaryDataObject, Object))(Expression.Convert(Expression.Property(param, SortingExpression),
                                                                                                                         GetType(Object)), param)

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
        ''' Gridview row selection and go for edit mode
        ''' </summary>
        ''' <param name="CurrentRow"></param>
        ''' <param name="isChecked"></param>
        ''' <remarks></remarks>
        Private Sub ControlsOnSelect(ByRef CurrentRow As GridViewRow, ByRef isChecked As Boolean)

            'LabelAttendantId = DirectCast(GridViewCareSummary.Rows(row.RowIndex).FindControl("LabelAttendantId"), Label)

            DropDownListAttendanceOrOfficeStaff = DirectCast(CurrentRow.FindControl("DropDownListAttendanceOrOfficeStaff"), 
                                                            DropDownList)

            If (Not DropDownListAttendanceOrOfficeStaff Is Nothing) Then
                DropDownListAttendanceOrOfficeStaff.Visible = isChecked
                DropDownListAttendanceOrOfficeStaff.CssClass = "DropDownListAttendanceOrOfficeStaff"

                LabelAttendantId = DirectCast(CurrentRow.FindControl("LabelAttendantId"), Label)

                If (Not LabelAttendantId Is Nothing) Then
                    DropDownListAttendanceOrOfficeStaff.SelectedIndex = DropDownListAttendanceOrOfficeStaff.Items.IndexOf(
                      DropDownListAttendanceOrOfficeStaff.Items.FindByValue(Convert.ToString(LabelAttendantId.Text.Trim(), Nothing)))
                End If

            End If

            TextBoxAdjustedBillTime = DirectCast(CurrentRow.FindControl("TextBoxAdjustedBillTime"), TextBox)
            If (Not TextBoxAdjustedBillTime Is Nothing) Then
                TextBoxAdjustedBillTime.ReadOnly = Not isChecked
                TextBoxAdjustedBillTime.CssClass = If((TextBoxAdjustedBillTime.ReadOnly), "TextBoxAdjustedBillTime", "TextBoxAdjustedBillTimeEdit")
            End If

            TextBoxTimesheet = DirectCast(CurrentRow.FindControl("TextBoxTimesheet"), TextBox)
            If (Not TextBoxTimesheet Is Nothing) Then
                TextBoxTimesheet.ReadOnly = Not isChecked
                TextBoxTimesheet.CssClass = If((TextBoxTimesheet.ReadOnly), "TextBoxTimesheet", "TextBoxTimesheetEdit")
            End If

            TextBoxAttendanceOrOfficeStaff = DirectCast(CurrentRow.FindControl("TextBoxAttendanceOrOfficeStaff"), TextBox)
            If (Not TextBoxAttendanceOrOfficeStaff Is Nothing) Then
                TextBoxAttendanceOrOfficeStaff.Visible = Not isChecked
            End If

        End Sub

        ''' <summary>
        ''' Set Hidden Control value 
        ''' </summary>
        ''' <param name="CurrentRow"></param>
        ''' <remarks></remarks>
        Private Sub SetHiddenControlValue(ByRef CurrentRow As GridViewRow)

            'TextBoxCalendarId = DirectCast(GridViewCareSummary.Rows(row.RowIndex).FindControl("TextBoxCalendarId"), TextBox)

            TextBoxCalendarId = DirectCast(CurrentRow.FindControl("TextBoxCalendarId"), TextBox)
            TextBoxStartDate = DirectCast(CurrentRow.FindControl("TextBoxStartDate"), TextBox)
            TextBoxEndDate = DirectCast(CurrentRow.FindControl("TextBoxEndDate"), TextBox)
            LabelClientId = DirectCast(CurrentRow.FindControl("LabelClientId"), Label)
            LabelAttendantId = DirectCast(CurrentRow.FindControl("LabelAttendantId"), Label)

            If ((Not HiddenFieldClientId Is Nothing) And (Not LabelClientId Is Nothing)) Then
                HiddenFieldClientId.Value = LabelClientId.Text.Trim()
            End If

            If ((Not HiddenFieldEmployeeId Is Nothing) And (Not LabelAttendantId Is Nothing)) Then
                HiddenFieldEmployeeId.Value = LabelAttendantId.Text.Trim()
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
        ''' This returns Original Timesheet Check Status
        ''' </summary>
        ''' <param name="str"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function GetStatus(str As String) As Boolean

            If (str.Equals("1")) Then
                Return True
            Else
                Return False
            End If

        End Function

        ''' <summary>
        ''' Gridview Control Javascript Event Registering
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub GridControlJavascriptEvent(ByRef e As GridViewRowEventArgs)

            CurrentRow = DirectCast(e.Row, GridViewRow)

            CheckBoxOriginalTimeSheetAll = DirectCast(CurrentRow.FindControl("CheckBoxOriginalTimeSheetAll"), CheckBox)

            If (Not CheckBoxOriginalTimeSheetAll Is Nothing) Then
                CheckBoxOriginalTimeSheetAll.Attributes.Add("OnClick", "javascript:SelectAllOriginalTimeSheet(this);")
            End If

            CheckBoxBilledAll = DirectCast(CurrentRow.FindControl("CheckBoxBilledAll"), CheckBox)

            If (Not CheckBoxBilledAll Is Nothing) Then
                CheckBoxBilledAll.Attributes.Add("OnClick", "javascript:SelectAllBilled(this);")
            End If

        End Sub

        ''' <summary>
        ''' Data binding for Pay Period Detail GridView
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindCareSummaryGridView()

            Dim GridResults As List(Of CareSummaryDataObject) = Nothing

            Dim objBLCareSummary As New BLCareSummary

            Try
                GridResults = objBLCareSummary.SelectCareSummaryInfo(objShared.ConVisitel, objShared.CompanyId,
                                                                Convert.ToInt64(DropDownListSearchByClient.SelectedValue),
                                                                Convert.ToInt64(DropDownListSearchByContract.SelectedValue),
                                                                If((DropDownListPayPeriod.SelectedIndex > 0),
                                                                    Convert.ToString(DropDownListPayPeriod.SelectedValue.Split("-")(0), Nothing),
                                                                    String.Empty),
                                                                If((DropDownListPayPeriod.SelectedIndex > 0),
                                                                    Convert.ToString(DropDownListPayPeriod.SelectedValue.Split("-")(1), Nothing),
                                                                    String.Empty),
                                                                Convert.ToInt16(ListBoxTimeSheet.SelectedValue),
                                                                Convert.ToInt16(ListBoxBilled.SelectedValue))

            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to fetch care summary")
            Finally
                objBLCareSummary = Nothing
            End Try

            GetSortedCareSummary(GridResults)

            GridViewCareSummary.DataSource = GridResults
            GridViewCareSummary.DataBind()

            GridResults = Nothing

        End Sub

        ''' <summary>
        ''' Reading GridView header text from resource file
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub SetGridViewColumnHeaderText(ByRef e As GridViewRowEventArgs)

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("CareSummary", ControlName & Convert.ToString(".resx"))

            CurrentRow = DirectCast(e.Row, GridViewRow)

            'Dim ButtonHeaderStartDate As Button = DirectCast(e.Row.Cells(0).FindControl("ButtonHeaderStartDate"), Button)

            Dim LabelSerial As Label = DirectCast(CurrentRow.FindControl("LabelSerial"), Label)
            LabelSerial.Text = Convert.ToString(ResourceTable("HeaderSerial"), Nothing).Trim()
            LabelSerial.Text = If(String.IsNullOrEmpty(LabelSerial.Text), "Sl No", LabelSerial.Text)

            Dim LabelSelect As Label = DirectCast(CurrentRow.FindControl("LabelSelect"), Label)
            LabelSelect.Text = Convert.ToString(ResourceTable("HeaderSelect"), Nothing).Trim()
            LabelSelect.Text = If(String.IsNullOrEmpty(LabelSelect.Text), "Select", LabelSelect.Text)

            Dim LabelAddNew As Label = DirectCast(CurrentRow.FindControl("LabelAddNew"), Label)
            LabelAddNew.Text = Convert.ToString(ResourceTable("HeaderAddNew"), Nothing).Trim()
            LabelAddNew.Text = If(String.IsNullOrEmpty(LabelAddNew.Text), "Add New", LabelAddNew.Text)

            Dim LabelBilledAll As Label = DirectCast(CurrentRow.FindControl("LabelBilledAll"), Label)
            LabelBilledAll.Text = Convert.ToString(ResourceTable("HeaderBilledAll"), Nothing).Trim()
            LabelBilledAll.Text = If(String.IsNullOrEmpty(LabelBilledAll.Text), "Billed(Y/N)", LabelBilledAll.Text)

            Dim ButtonHeaderStartDate As Button = DirectCast(CurrentRow.FindControl("ButtonHeaderStartDate"), Button)
            ButtonHeaderStartDate.Text = Convert.ToString(ResourceTable("HeaderStartDate"), Nothing).Trim()
            ButtonHeaderStartDate.Text = If(String.IsNullOrEmpty(ButtonHeaderStartDate.Text), "Start Date", ButtonHeaderStartDate.Text)

            Dim ButtonHeaderEndDate As Button = DirectCast(CurrentRow.FindControl("ButtonHeaderEndDate"), Button)
            ButtonHeaderEndDate.Text = Convert.ToString(ResourceTable("HeaderEndDate"), Nothing).Trim()
            ButtonHeaderEndDate.Text = If(String.IsNullOrEmpty(ButtonHeaderEndDate.Text), "End Date", ButtonHeaderEndDate.Text)

            Dim ButtonHeaderClientName As Button = DirectCast(CurrentRow.FindControl("ButtonHeaderClientName"), Button)
            ButtonHeaderClientName.Text = Convert.ToString(ResourceTable("HeaderClient"), Nothing).Trim()
            ButtonHeaderClientName.Text = If(String.IsNullOrEmpty(ButtonHeaderClientName.Text), "Client", ButtonHeaderClientName.Text)

            Dim LabelBillTime As Label = DirectCast(CurrentRow.FindControl("LabelBillTime"), Label)
            LabelBillTime.Text = Convert.ToString(ResourceTable("HeaderBillTime"), Nothing).Trim()
            LabelBillTime.Text = If(String.IsNullOrEmpty(LabelBillTime.Text), "Bill Time", LabelBillTime.Text)

            Dim LabelAdjustedBillTime As Label = DirectCast(CurrentRow.FindControl("LabelAdjustedBillTime"), Label)
            LabelAdjustedBillTime.Text = Convert.ToString(ResourceTable("HeaderAdjustedBillTime"), Nothing).Trim()
            LabelAdjustedBillTime.Text = If(String.IsNullOrEmpty(LabelAdjustedBillTime.Text), "Adjusted Bill Time", LabelAdjustedBillTime.Text)

            Dim LabelOriginalTimeSheetAll As Label = DirectCast(CurrentRow.FindControl("LabelOriginalTimeSheetAll"), Label)
            LabelOriginalTimeSheetAll.Text = Convert.ToString(ResourceTable("HeaderOriginalTimesheetAll"), Nothing).Trim()
            LabelOriginalTimeSheetAll.Text = If(String.IsNullOrEmpty(LabelOriginalTimeSheetAll.Text), "Orig Time-sheet", LabelOriginalTimeSheetAll.Text)

            Dim LabelTimesheet As Label = DirectCast(CurrentRow.FindControl("LabelTimesheet"), Label)
            LabelTimesheet.Text = Convert.ToString(ResourceTable("HeaderTimesheet"), Nothing).Trim()
            LabelTimesheet.Text = If(String.IsNullOrEmpty(LabelTimesheet.Text), "Timesheet", LabelTimesheet.Text)

            Dim LabelTimesheetHrsMins As Label = DirectCast(CurrentRow.FindControl("LabelTimesheetHrsMins"), Label)
            LabelTimesheetHrsMins.Text = Convert.ToString(ResourceTable("HeaderTimesheet"), Nothing).Trim()
            LabelTimesheetHrsMins.Text = If(String.IsNullOrEmpty(LabelTimesheetHrsMins.Text), "Hrs:Mins", LabelTimesheetHrsMins.Text)

            Dim ButtonHeaderAttendantName As Button = DirectCast(CurrentRow.FindControl("ButtonHeaderAttendantName"), Button)
            ButtonHeaderAttendantName.Text = Convert.ToString(ResourceTable("HeaderAttendanceOrOfficeStaff"), Nothing).Trim()
            ButtonHeaderAttendantName.Text = If(String.IsNullOrEmpty(ButtonHeaderAttendantName.Text), "Attendance/Office Staff", ButtonHeaderAttendantName.Text)

            Dim LabelHeaderCalendarId As Label = DirectCast(CurrentRow.FindControl("LabelHeaderCalendarId"), Label)
            LabelHeaderCalendarId.Text = Convert.ToString(ResourceTable("HeaderCalendarId"), Nothing).Trim()
            LabelHeaderCalendarId.Text = If(String.IsNullOrEmpty(LabelHeaderCalendarId.Text), "Cal Id", LabelHeaderCalendarId.Text)

            Dim LabelIndividualType As Label = DirectCast(CurrentRow.FindControl("LabelIndividualType"), Label)
            LabelIndividualType.Text = Convert.ToString(ResourceTable("HeaderIndividualType"), Nothing).Trim()
            LabelIndividualType.Text = If(String.IsNullOrEmpty(LabelIndividualType.Text), "Individual Type", LabelIndividualType.Text)

            Dim LabelHeaderUpdateDate As Label = DirectCast(CurrentRow.FindControl("LabelHeaderUpdateDate"), Label)
            LabelHeaderUpdateDate.Text = Convert.ToString(ResourceTable("HeaderUpdateDate"), Nothing).Trim()
            LabelHeaderUpdateDate.Text = If(String.IsNullOrEmpty(LabelHeaderUpdateDate.Text), "Update Date", LabelHeaderUpdateDate.Text)

            Dim LabelHeaderUpdateBy As Label = DirectCast(CurrentRow.FindControl("LabelHeaderUpdateBy"), Label)
            LabelHeaderUpdateBy.Text = Convert.ToString(ResourceTable("HeaderUpdateBy"), Nothing).Trim()
            LabelHeaderUpdateBy.Text = If(String.IsNullOrEmpty(LabelHeaderUpdateBy.Text), "Update By", LabelHeaderUpdateBy.Text)

            Dim LabelEDIFile As Label = DirectCast(CurrentRow.FindControl("LabelEDIFile"), Label)
            LabelEDIFile.Text = Convert.ToString(ResourceTable("HeaderEDIFile"), Nothing).Trim()
            LabelEDIFile.Text = If(String.IsNullOrEmpty(LabelEDIFile.Text), "EDI File", LabelEDIFile.Text)

            Dim LabelHeaderEDIUpdateDate As Label = DirectCast(CurrentRow.FindControl("LabelHeaderEDIUpdateDate"), Label)
            LabelHeaderEDIUpdateDate.Text = Convert.ToString(ResourceTable("HeaderEDIUpdateDate"), Nothing).Trim()
            LabelHeaderEDIUpdateDate.Text = If(String.IsNullOrEmpty(LabelHeaderEDIUpdateDate.Text), "EDI Update Date", LabelHeaderEDIUpdateDate.Text)

            Dim LabelHeaderEDIUpdateBy As Label = DirectCast(CurrentRow.FindControl("LabelHeaderEDIUpdateBy"), Label)
            LabelHeaderEDIUpdateBy.Text = Convert.ToString(ResourceTable("HeaderEDIUpdateBy"), Nothing).Trim()
            LabelHeaderEDIUpdateBy.Text = If(String.IsNullOrEmpty(LabelHeaderEDIUpdateBy.Text), "EDI Update By", LabelHeaderEDIUpdateBy.Text)

            ResourceTable = Nothing

        End Sub

        ''' <summary>
        ''' This is for validating data in server side before submitting the form
        ''' </summary>
        ''' <returns></returns>
        Private Function ValidateData(RowSerial As Int16) As Boolean

            Dim RowSerialPointerText As String

            RowSerialPointerText = " on Row Serial" & RowSerial

            If (LabelCareSummaryId Is Nothing) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Care Summary Id Not Found" & RowSerialPointerText)
                Return False
            End If

            If (LabelClientId Is Nothing) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Client Id Not Found" & RowSerialPointerText)
                Return False
            End If

            If (LabelClientType Is Nothing) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Client Type Not Found" & RowSerialPointerText)
            End If

            If (TextBoxStartDate Is Nothing) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Start Date Not Found" & RowSerialPointerText)
            End If

            If (TextBoxEndDate Is Nothing) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("End Date Not Found" & RowSerialPointerText)
            End If

            If (CheckBoxBilled Is Nothing) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Billed Value Not Found" & RowSerialPointerText)
            End If

            If (TextBoxAdjustedBillTime Is Nothing) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Billed Value Not Found" & RowSerialPointerText)
            End If

            If (CheckBoxOriginalTimeSheet Is Nothing) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Original Timesheet Value Not Found" & RowSerialPointerText)
            End If

            If (TextBoxTimesheet Is Nothing) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Timesheet Value Not Found" & RowSerialPointerText)
            End If

            If (DropDownListAttendanceOrOfficeStaff Is Nothing) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Attendant Id Not Found" & RowSerialPointerText)
                Return False
            End If

            Return True

        End Function

        'Private Sub txt_bill_time_DblClick(Cancel As Integer)

        '    If MsgBox("This double-click action will set the hours to timesheet hours which is zero. " & _
        '        "Are you sure you want to do this?", vbYesNo, "TurboPAS") = vbNo Then

        '        Me.Undo()
        '        Exit Sub

        '    Else

        '        If txt_ts_hrs = 0 And txt_ts_mins = 0 Then txt_actual_mins = 0
        '    End If
        'End Sub

    End Class
End Namespace