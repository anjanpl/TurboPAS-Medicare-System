Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports System.Data.SqlClient
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelCommon
Imports VisitelBusiness.VisitelBusiness.DataObject.EVV

Namespace Visitel.UserControl.EVV
    Public Class MEDsysSchedulesControl
        Inherits CommonDataControl

        Private ContractNumber As String
        Private objShared As SharedWebControls

        Private CheckBoxSelect As CheckBox, chkAll As CheckBox
        Private ItemChecked As Boolean = False

        Private LabelHeaderSerial As Label, LabelHeaderSelect As Label, LabelHeaderClient As Label, LabelHeaderEmployee As Label, LabelHeaderAccountId As Label, _
            LabelHeaderExternalId As Label, LabelHeaderScheduleId As Label, LabelHeaderDate As Label, LabelHeaderPlannedTimeIn As Label, LabelHeaderPlannedTimeOut As Label, _
            LabelHeaderPlannedDuration As Label, LabelHeaderServiceCode As Label, LabelHeaderActivityCode As Label, LabelHeaderProgramCode As Label, _
            LabelHeaderClientExternalId As Label, LabelHeaderBillType As Label, LabelHeaderPayType As Label, LabelHeaderOccurredTimeIn As Label, _
            LabelHeaderOccurredTimeOut As Label, LabelHeaderOccurredDuration As Label, LabelHeaderStatus As Label, LabelHeaderComments As Label, _
            LabelHeaderAction As Label, LabelHeaderUpdateDate As Label, LabelHeaderUpdateBy As Label, LabelClientId As Label, LabelEmployeeId As Label, _
            LabelId As Label

        Private TextBoxClientName As TextBox, TextBoxEmployeeName As TextBox, TextBoxAccountId As TextBox, TextBoxExternalId As TextBox, TextBoxScheduleId As TextBox, _
            TextBoxDate As TextBox, TextBoxPlannedTimeIn As TextBox, TextBoxPlannedTimeOut As TextBox, TextBoxPlannedDuration As TextBox, TextBoxServiceCode As TextBox, _
            TextBoxActivityCode As TextBox, TextBoxProgramCode As TextBox, TextBoxClientExternalId As TextBox, TextBoxStaffExternalId As TextBox, TextBoxBillType As TextBox, _
            TextBoxPayType As TextBox, TextBoxOccurredTimeIn As TextBox, TextBoxOccurredTimeOut As TextBox, TextBoxOccurredDuration As TextBox, TextBoxStatus As TextBox, _
            TextBoxComments As TextBox, TextBoxAction As TextBox, TextBoxUpdateDate As TextBox, TextBoxUpdateBy As TextBox

        Private DropDownListClient As DropDownList, DropDownListEmployee As DropDownList

        Private ControlName As String, ValidationGroup As String, SaveMessage As String, DeleteMessage As String, DeleteConfirmationMessage As String, ListFor As String
        Private ValidationEnable As Boolean
        Private CurrentRow As GridViewRow

        Private MEDsysSchedules As List(Of MEDsysScheduleDataObject), MEDsysSchedulesErrorList As List(Of MEDsysScheduleDataObject)

        Private SaveFailedCounter As Int16, DeleteFailedCounter As Int16, RecordSaveCount As Int16
        Public IsLog As Boolean


        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "MEDsysSchedulesControl"
            objShared = New SharedWebControls()
            objShared.ConnectionOpen()
            GetCaptionFromResource()
            InitializeControl()
        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)

            GetData()

            If Not IsPostBack Then
                BindGridViewMEDsysSchedules()
            End If

        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadCss("EVV/" & ControlName)
            LoadJScript()
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing
            MEDsysSchedules = Nothing
        End Sub

        Private Sub ButtonSave_Click(sender As Object, e As EventArgs)

            Page.Validate()
            Page.Validate(ValidationGroup)

            MEDsysSchedules = New List(Of MEDsysScheduleDataObject)()
            MEDsysSchedulesErrorList = New List(Of MEDsysScheduleDataObject)()
            SaveFailedCounter = 0
            RecordSaveCount = 0

            CurrentRow = GridViewMEDsysSchedules.FooterRow

            CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)

            LabelClientId = DirectCast(CurrentRow.FindControl("LabelClientId"), Label)
            LabelEmployeeId = DirectCast(CurrentRow.FindControl("LabelEmployeeId"), Label)

            If ((CheckBoxSelect.Checked) And ((String.IsNullOrEmpty(LabelClientId.Text.Trim())) Or (String.IsNullOrEmpty(LabelEmployeeId.Text.Trim())))) Then
                SaveData(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT))
            End If

            SaveData(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE))

            If (SaveFailedCounter > 0) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} Records Failed to Save out of {1}", SaveFailedCounter, MEDsysSchedules.Count))
                ViewState("SaveFailedRecord") = objShared.ToDataTable(MEDsysSchedulesErrorList)
                ViewState("ListFor") = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)
            Else
                ViewState("SaveFailedRecord") = Nothing

                If (RecordSaveCount > 0) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} record(s) {1}.", RecordSaveCount, SaveMessage))
                    ViewState.Clear()
                    GetData()
                    BindGridViewMEDsysSchedules()
                End If

            End If

            ButtonViewError.Visible = (SaveFailedCounter > 0)

            MEDsysSchedules = Nothing
            MEDsysSchedulesErrorList = Nothing

        End Sub

        Private Sub ButtonIndividualDetail_Click(sender As Object, e As EventArgs)
            Response.Redirect("~/Pages/ClientInfo.aspx?ClientId=" & HiddenFieldClientId.Value)
        End Sub

        Private Sub ButtonEmployeeDetail_Click(sender As Object, e As EventArgs)
            Response.Redirect("~/Pages/EmployeeInfo.aspx?EmployeeId=" & HiddenFieldEmployeeId.Value)
        End Sub

        Private Sub ButtonDelete_Click(sender As Object, e As EventArgs)
            MEDsysSchedules = New List(Of MEDsysScheduleDataObject)()
            MEDsysSchedulesErrorList = New List(Of MEDsysScheduleDataObject)()

            MakeList(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE))

            If (MEDsysSchedules.Count.Equals(0)) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please select items to delete")
                Return
            End If

            DeleteFailedCounter = 0

            TakeAction(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE))

            If (DeleteFailedCounter > 0) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} Records Failed to Delete out of {1}", DeleteFailedCounter, MEDsysSchedules.Count))
                ViewState("DeleteFailedRecord") = objShared.ToDataTable(MEDsysSchedulesErrorList)
                ViewState("ListFor") = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE)
            Else
                ViewState("DeleteFailedRecord") = Nothing

                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} record(s) {1}.", MEDsysSchedules.Count, DeleteMessage))
                ViewState.Clear()
                GetData()
                BindGridViewMEDsysSchedules()
            End If

            ButtonViewError.Visible = (DeleteFailedCounter > 0)

            MEDsysSchedules = Nothing
            MEDsysSchedulesErrorList = Nothing
        End Sub

        Private Sub ButtonClear_Click(sender As Object, e As EventArgs)
            CurrentRow = GridViewMEDsysSchedules.FooterRow
            ClearControl(CurrentRow)
        End Sub

        Protected Sub OnCheckedChanged(sender As Object, e As EventArgs)
            Dim isUpdateVisible As Boolean = False

            Dim chk As CheckBox = TryCast(sender, CheckBox)

            'ButtonSave.Enabled = If((chk.Checked), True, False)

            If chk.ID = "chkAll" Then
                For Each row As GridViewRow In GridViewMEDsysSchedules.Rows
                    If row.RowType = DataControlRowType.DataRow Then
                        row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked = chk.Checked
                    End If
                Next

                CurrentRow = GridViewMEDsysSchedules.FooterRow
                CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)
                CheckBoxSelect.Checked = chk.Checked

            End If

            Dim chkAll As CheckBox = TryCast(GridViewMEDsysSchedules.HeaderRow.FindControl("chkAll"), CheckBox)
            'chkAll.Checked = True
            For Each row As GridViewRow In GridViewMEDsysSchedules.Rows
                If row.RowType = DataControlRowType.DataRow Then

                    Dim isChecked As Boolean = row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked

                    ControlsOnSelect(row, isChecked)

                    If (isChecked) Then
                        LabelClientId = DirectCast(row.FindControl("LabelClientId"), Label)
                        If ((Not HiddenFieldClientId Is Nothing) And (Not LabelClientId Is Nothing)) Then
                            HiddenFieldClientId.Value = LabelClientId.Text.Trim()
                        End If

                        LabelEmployeeId = DirectCast(row.FindControl("LabelEmployeeId"), Label)
                        If ((Not HiddenFieldEmployeeId Is Nothing) And (Not LabelEmployeeId Is Nothing)) Then
                            HiddenFieldEmployeeId.Value = LabelEmployeeId.Text.Trim()
                        End If
                    End If

                    For i As Integer = 1 To row.Cells.Count - 1

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

            ItemChecked = DetermineButtonInActivity(GridViewMEDsysSchedules, "CheckBoxSelect")

            ButtonSave.Enabled = ItemChecked
            ButtonDelete.Enabled = ButtonSave.Enabled
            ButtonIndividualDetail.Enabled = ButtonSave.Enabled
            ButtonEmployeeDetail.Enabled = ButtonSave.Enabled
        End Sub

        ''' <summary>
        ''' Saving Data either insert or update after making list
        ''' </summary>
        ''' <param name="Action"></param>
        ''' <remarks></remarks>
        Private Sub SaveData(Action As String)

            MakeList(Action)

            If (MEDsysSchedules.Count.Equals(0)) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please select items to save")
                Return
            End If

            TakeAction(Action)

        End Sub

        ''' <summary>
        ''' Making a List of Record Set Either for Save or Delete
        ''' </summary>
        ''' <param name="Action"></param>
        ''' <remarks></remarks>
        Private Sub MakeList(Action As String)

            Dim objMEDsysScheduleDataObject As MEDsysScheduleDataObject

            Select Case Action
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT)

                    LabelEmployeeId = DirectCast(GridViewMEDsysSchedules.FooterRow.FindControl("LabelEmployeeId"), Label)

                    DropDownListClient = DirectCast(GridViewMEDsysSchedules.FooterRow.FindControl("DropDownListClient"), DropDownList)
                    If (DropDownListClient.SelectedIndex < 1) Then
                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please select one client")
                        Return
                    End If

                    DropDownListEmployee = DirectCast(GridViewMEDsysSchedules.FooterRow.FindControl("DropDownListEmployee"), DropDownList)
                    If (DropDownListEmployee.SelectedIndex < 1) Then
                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please select one employee")
                        Return
                    End If

                    objMEDsysScheduleDataObject = New MEDsysScheduleDataObject()
                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)

                    AddToList(CurrentRow, MEDsysSchedules, objMEDsysScheduleDataObject)
                    RecordSaveCount = RecordSaveCount + 1
                    objMEDsysScheduleDataObject = Nothing

                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE),
                    EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE)
                    MEDsysSchedules.Clear()
                    For Each row As GridViewRow In GridViewMEDsysSchedules.Rows
                        If (row.RowType.Equals(DataControlRowType.DataRow)) Then
                            Dim isChecked As Boolean = row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked

                            If (isChecked) Then

                                objMEDsysScheduleDataObject = New MEDsysScheduleDataObject()

                                CurrentRow = DirectCast(GridViewMEDsysSchedules.Rows(row.RowIndex), GridViewRow)


                                LabelEmployeeId = DirectCast(CurrentRow.FindControl("LabelEmployeeId"), Label)

                                DropDownListClient = DirectCast(CurrentRow.FindControl("DropDownListClient"), DropDownList)
                                If (DropDownListClient.SelectedIndex < 1) Then
                                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please select one client")
                                    Return
                                End If

                                DropDownListEmployee = DirectCast(CurrentRow.FindControl("DropDownListEmployee"), DropDownList)
                                If (DropDownListEmployee.SelectedIndex < 1) Then
                                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please select one employee")
                                    Return
                                End If

                                If (Action.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE))) Then
                                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)
                                End If

                                If (Action.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE))) Then
                                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE)
                                End If

                                AddToList(CurrentRow, MEDsysSchedules, objMEDsysScheduleDataObject)
                                RecordSaveCount = RecordSaveCount + 1
                                objMEDsysScheduleDataObject = Nothing

                            End If

                        End If
                    Next

                    Exit Select
            End Select

            objMEDsysScheduleDataObject = Nothing

        End Sub

        ''' <summary>
        ''' Making List in order to save records or delete
        ''' </summary>
        ''' <param name="CurrentRow"></param>
        ''' <param name="MEDsysSchedules"></param>
        ''' <param name="objMEDsysScheduleDataObject"></param>
        ''' <remarks></remarks>
        Private Sub AddToList(ByRef CurrentRow As GridViewRow, ByRef MEDsysSchedules As List(Of MEDsysScheduleDataObject),
                              ByRef objMEDsysScheduleDataObject As MEDsysScheduleDataObject)

            Dim Int32Result As Int32

            LabelId = DirectCast(CurrentRow.FindControl("LabelId"), Label)
            LabelClientId = DirectCast(CurrentRow.FindControl("LabelClientId"), Label)

            If (ListFor.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE))) Then
                ControlsFind(CurrentRow)
            End If

            If (Page.IsValid) Then

                objMEDsysScheduleDataObject.Id = If(Int32.TryParse(LabelId.Text.Trim(), Int32Result), Int32Result, objMEDsysScheduleDataObject.Id)

                If (ListFor.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE))) Then
                    objMEDsysScheduleDataObject.AccountId = TextBoxAccountId.Text.Trim()
                    objMEDsysScheduleDataObject.ExternalId = TextBoxExternalId.Text.Trim()
                    objMEDsysScheduleDataObject.ScheduleId = TextBoxScheduleId.Text.Trim()
                    objMEDsysScheduleDataObject.Date = TextBoxDate.Text.Trim()
                    objMEDsysScheduleDataObject.ServiceCode = TextBoxServiceCode.Text.Trim()
                    objMEDsysScheduleDataObject.ActivityCode = TextBoxActivityCode.Text.Trim()
                    objMEDsysScheduleDataObject.ActivityCode = TextBoxActivityCode.Text.Trim()
                    objMEDsysScheduleDataObject.ProgramCode = TextBoxProgramCode.Text.Trim()
                    objMEDsysScheduleDataObject.ClientExternalId = TextBoxClientExternalId.Text.Trim()
                    objMEDsysScheduleDataObject.StaffExternalId = TextBoxStaffExternalId.Text.Trim()
                    objMEDsysScheduleDataObject.BillType = TextBoxBillType.Text.Trim()
                    objMEDsysScheduleDataObject.PayType = TextBoxPayType.Text.Trim()
                    objMEDsysScheduleDataObject.PlannedTimeIn = TextBoxPlannedTimeIn.Text.Trim()
                    objMEDsysScheduleDataObject.PlannedTimeOut = TextBoxPlannedTimeOut.Text.Trim()
                    objMEDsysScheduleDataObject.PlannedDuration = TextBoxPlannedDuration.Text.Trim()
                    objMEDsysScheduleDataObject.OccurredTimeIn = TextBoxOccurredTimeIn.Text.Trim()
                    objMEDsysScheduleDataObject.OccurredTimeOut = TextBoxOccurredTimeOut.Text.Trim()
                    objMEDsysScheduleDataObject.OccurredDuration = TextBoxOccurredDuration.Text.Trim()
                    objMEDsysScheduleDataObject.Status = TextBoxStatus.Text.Trim()
                    objMEDsysScheduleDataObject.Comments = TextBoxComments.Text.Trim()
                    objMEDsysScheduleDataObject.Action = TextBoxAction.Text.Trim()
                    objMEDsysScheduleDataObject.ClientId = Convert.ToInt64(DropDownListClient.SelectedValue)
                    objMEDsysScheduleDataObject.EmployeeId = Convert.ToInt64(DropDownListEmployee.SelectedValue)
                End If

                objMEDsysScheduleDataObject.UpdateBy = objShared.UserId
                MEDsysSchedules.Add(objMEDsysScheduleDataObject)
            End If
        End Sub

        ''' <summary>
        ''' Make Database operation either for record saving or deleting
        ''' </summary>
        ''' <param name="Action"></param>
        ''' <remarks></remarks>
        Private Sub TakeAction(Action As String)

            Dim objBLMEDsysSchedules As New BLMEDsysSchedules()

            For Each MEDsysSchedule In MEDsysSchedules
                Try
                    Select Case Action
                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT)
                            objBLMEDsysSchedules.InsertMEDsysScheduleInfo(objShared.ConVisitel, MEDsysSchedule, IsLog)
                            Exit Select

                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE)
                            objBLMEDsysSchedules.UpdateMEDsysScheduleInfo(objShared.ConVisitel, MEDsysSchedule, IsLog)
                            Exit Select

                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE)
                            objBLMEDsysSchedules.DeleteMEDsysScheduleInfo(objShared.ConVisitel, MEDsysSchedule.Id, MEDsysSchedule.UpdateBy, IsLog)
                            Exit Select
                    End Select

                Catch ex As SqlException

                    Select Case Action
                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT),
                            EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE)
                            SaveFailedCounter = SaveFailedCounter + 1
                            Exit Select

                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE)
                            DeleteFailedCounter = DeleteFailedCounter + 1
                            Exit Select

                    End Select

                    MEDsysSchedule.Remarks = ex.Message
                    MEDsysSchedulesErrorList.Add(MEDsysSchedule)

                Finally

                End Try
            Next

            objBLMEDsysSchedules = Nothing

        End Sub

        Private Sub ControlsFind(ByRef CurrentRow As GridViewRow)
            TextBoxAccountId = DirectCast(CurrentRow.FindControl("TextBoxAccountId"), TextBox)
            TextBoxExternalId = DirectCast(CurrentRow.FindControl("TextBoxExternalId"), TextBox)
            TextBoxScheduleId = DirectCast(CurrentRow.FindControl("TextBoxScheduleId"), TextBox)
            TextBoxDate = DirectCast(CurrentRow.FindControl("TextBoxDate"), TextBox)
            TextBoxPlannedTimeIn = DirectCast(CurrentRow.FindControl("TextBoxPlannedTimeIn"), TextBox)
            TextBoxPlannedTimeOut = DirectCast(CurrentRow.FindControl("TextBoxPlannedTimeOut"), TextBox)
            TextBoxPlannedDuration = DirectCast(CurrentRow.FindControl("TextBoxPlannedDuration"), TextBox)
            TextBoxServiceCode = DirectCast(CurrentRow.FindControl("TextBoxServiceCode"), TextBox)
            TextBoxActivityCode = DirectCast(CurrentRow.FindControl("TextBoxActivityCode"), TextBox)
            TextBoxProgramCode = DirectCast(CurrentRow.FindControl("TextBoxProgramCode"), TextBox)
            TextBoxClientExternalId = DirectCast(CurrentRow.FindControl("TextBoxClientExternalId"), TextBox)
            TextBoxStaffExternalId = DirectCast(CurrentRow.FindControl("TextBoxStaffExternalId"), TextBox)
            TextBoxBillType = DirectCast(CurrentRow.FindControl("TextBoxBillType"), TextBox)
            TextBoxPayType = DirectCast(CurrentRow.FindControl("TextBoxPayType"), TextBox)
            TextBoxOccurredTimeIn = DirectCast(CurrentRow.FindControl("TextBoxOccurredTimeIn"), TextBox)
            TextBoxOccurredTimeOut = DirectCast(CurrentRow.FindControl("TextBoxOccurredTimeOut"), TextBox)
            TextBoxOccurredDuration = DirectCast(CurrentRow.FindControl("TextBoxOccurredDuration"), TextBox)
            TextBoxStatus = DirectCast(CurrentRow.FindControl("TextBoxStatus"), TextBox)
            TextBoxComments = DirectCast(CurrentRow.FindControl("TextBoxComments"), TextBox)
            TextBoxAction = DirectCast(CurrentRow.FindControl("TextBoxAction"), TextBox)

            If (Not CurrentRow.RowType.Equals(DataControlRowType.Footer)) Then
                TextBoxClientName = DirectCast(CurrentRow.FindControl("TextBoxClientName"), TextBox)
            End If

            DropDownListClient = DirectCast(CurrentRow.FindControl("DropDownListClient"), DropDownList)

            If (Not CurrentRow.RowType.Equals(DataControlRowType.Footer)) Then
                TextBoxEmployeeName = DirectCast(CurrentRow.FindControl("TextBoxEmployeeName"), TextBox)
            End If

            DropDownListEmployee = DirectCast(CurrentRow.FindControl("DropDownListEmployee"), DropDownList)
        End Sub

        ''' <summary>
        ''' Gridview row selection and go for edit mode or record deletion
        ''' </summary>
        ''' <param name="CurrentRow"></param>
        ''' <param name="isChecked"></param>
        ''' <remarks></remarks>
        Private Sub ControlsOnSelect(ByRef CurrentRow As GridViewRow, ByRef isChecked As Boolean)

            ControlsFind(CurrentRow)

            If (Not TextBoxAccountId Is Nothing) Then
                TextBoxAccountId.ReadOnly = Not isChecked
                TextBoxAccountId.CssClass = If((TextBoxAccountId.ReadOnly), "TextBoxAccountId", "TextBoxAccountIdEdit")
            End If

            If (Not TextBoxExternalId Is Nothing) Then
                TextBoxExternalId.ReadOnly = Not isChecked
                TextBoxExternalId.CssClass = If((TextBoxExternalId.ReadOnly), "TextBoxExternalId", "TextBoxExternalIdEdit")
            End If

            If (Not TextBoxScheduleId Is Nothing) Then
                TextBoxScheduleId.ReadOnly = Not isChecked
                TextBoxScheduleId.CssClass = If((TextBoxScheduleId.ReadOnly), "TextBoxScheduleId", "TextBoxScheduleIdEdit")
            End If

            If (Not TextBoxDate Is Nothing) Then
                TextBoxDate.ReadOnly = Not isChecked
                TextBoxDate.CssClass = If((TextBoxDate.ReadOnly), "TextBoxDate", "TextBoxDateEdit")
            End If

            If (Not TextBoxPlannedTimeIn Is Nothing) Then
                TextBoxPlannedTimeIn.ReadOnly = Not isChecked
                TextBoxPlannedTimeIn.CssClass = If((TextBoxPlannedTimeIn.ReadOnly), "TextBoxPlannedTimeIn", "TextBoxPlannedTimeInEdit")
            End If

            If (Not TextBoxPlannedTimeOut Is Nothing) Then
                TextBoxPlannedTimeOut.ReadOnly = Not isChecked
                TextBoxPlannedTimeOut.CssClass = If((TextBoxPlannedTimeOut.ReadOnly), "TextBoxPlannedTimeOut", "TextBoxPlannedTimeOutEdit")
            End If

            If (Not TextBoxPlannedDuration Is Nothing) Then
                TextBoxPlannedDuration.ReadOnly = Not isChecked
                TextBoxPlannedDuration.CssClass = If((TextBoxPlannedDuration.ReadOnly), "TextBoxPlannedDuration", "TextBoxPlannedDurationEdit")
            End If

            If (Not TextBoxServiceCode Is Nothing) Then
                TextBoxServiceCode.ReadOnly = Not isChecked
                TextBoxServiceCode.CssClass = If((TextBoxServiceCode.ReadOnly), "TextBoxServiceCode", "TextBoxServiceCodeEdit")
            End If

            If (Not TextBoxActivityCode Is Nothing) Then
                TextBoxActivityCode.ReadOnly = Not isChecked
                TextBoxActivityCode.CssClass = If((TextBoxActivityCode.ReadOnly), "TextBoxActivityCode", "TextBoxActivityCodeEdit")
            End If

            If (Not TextBoxProgramCode Is Nothing) Then
                TextBoxProgramCode.ReadOnly = Not isChecked
                TextBoxProgramCode.CssClass = If((TextBoxProgramCode.ReadOnly), "TextBoxProgramCode", "TextBoxProgramCodeEdit")
            End If

            If (Not TextBoxClientExternalId Is Nothing) Then
                TextBoxClientExternalId.ReadOnly = Not isChecked
                TextBoxClientExternalId.CssClass = If((TextBoxClientExternalId.ReadOnly), "TextBoxClientExternalId", "TextBoxClientExternalIdEdit")
            End If

            If (Not TextBoxStaffExternalId Is Nothing) Then
                TextBoxStaffExternalId.ReadOnly = Not isChecked
                TextBoxStaffExternalId.CssClass = If((TextBoxStaffExternalId.ReadOnly), "TextBoxStaffExternalId", "TextBoxStaffExternalIdEdit")
            End If

            If (Not TextBoxBillType Is Nothing) Then
                TextBoxBillType.ReadOnly = Not isChecked
                TextBoxBillType.CssClass = If((TextBoxBillType.ReadOnly), "TextBoxBillType", "TextBoxBillTypeEdit")
            End If

            If (Not TextBoxPayType Is Nothing) Then
                TextBoxPayType.ReadOnly = Not isChecked
                TextBoxPayType.CssClass = If((TextBoxPayType.ReadOnly), "TextBoxPayType", "TextBoxPayTypeEdit")
            End If

            If (Not TextBoxOccurredTimeIn Is Nothing) Then
                TextBoxOccurredTimeIn.ReadOnly = Not isChecked
                TextBoxOccurredTimeIn.CssClass = If((TextBoxOccurredTimeIn.ReadOnly), "TextBoxOccurredTimeIn", "TextBoxOccurredTimeInEdit")
            End If

            If (Not TextBoxOccurredTimeOut Is Nothing) Then
                TextBoxOccurredTimeOut.ReadOnly = Not isChecked
                TextBoxOccurredTimeOut.CssClass = If((TextBoxOccurredTimeOut.ReadOnly), "TextBoxOccurredTimeOut", "TextBoxOccurredTimeOutEdit")
            End If

            If (Not TextBoxOccurredDuration Is Nothing) Then
                TextBoxOccurredDuration.ReadOnly = Not isChecked
                TextBoxOccurredDuration.CssClass = If((TextBoxOccurredDuration.ReadOnly), "TextBoxOccurredDuration", "TextBoxOccurredDurationEdit")
            End If

            If (Not TextBoxStatus Is Nothing) Then
                TextBoxStatus.ReadOnly = Not isChecked
                TextBoxStatus.CssClass = If((TextBoxStatus.ReadOnly), "TextBoxStatus", "TextBoxStatusEdit")
            End If

            If (Not TextBoxComments Is Nothing) Then
                TextBoxComments.ReadOnly = Not isChecked
                TextBoxComments.CssClass = If((TextBoxComments.ReadOnly), "TextBoxComments", "TextBoxCommentsEdit")
            End If

            If (Not TextBoxAction Is Nothing) Then
                TextBoxAction.ReadOnly = Not isChecked
                TextBoxAction.CssClass = If((TextBoxAction.ReadOnly), "TextBoxAction", "TextBoxActionEdit")
            End If

            TextBoxClientName = DirectCast(CurrentRow.FindControl("TextBoxClientName"), TextBox)
            If (Not TextBoxClientName Is Nothing) Then
                TextBoxClientName.Visible = Not isChecked
            End If

            TextBoxEmployeeName = DirectCast(CurrentRow.FindControl("TextBoxEmployeeName"), TextBox)
            If (Not TextBoxEmployeeName Is Nothing) Then
                TextBoxEmployeeName.Visible = Not isChecked
            End If

        End Sub

        Private Sub GridViewMEDsysSchedules_RowDataBound(sender As Object, e As GridViewRowEventArgs)
            CurrentRow = DirectCast(e.Row, GridViewRow)

            If (CurrentRow.RowType.Equals(DataControlRowType.Header)) Then
                SetGridViewColumnHeaderText(CurrentRow)

                chkAll = DirectCast(CurrentRow.FindControl("chkAll"), CheckBox)
                chkAll.AutoPostBack = True
                chkAll.ClientIDMode = UI.ClientIDMode.Static
            End If

            If (CurrentRow.RowType.Equals(DataControlRowType.DataRow)) Then

                CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)
                CheckBoxSelect.AutoPostBack = True

                TextBoxClientName = DirectCast(CurrentRow.FindControl("TextBoxClientName"), TextBox)
                TextBoxClientName.ReadOnly = True

                TextBoxEmployeeName = DirectCast(CurrentRow.FindControl("TextBoxEmployeeName"), TextBox)
                TextBoxEmployeeName.ReadOnly = True

                TextBoxAccountId = DirectCast(CurrentRow.FindControl("TextBoxAccountId"), TextBox)
                TextBoxAccountId.ReadOnly = True

                TextBoxExternalId = DirectCast(CurrentRow.FindControl("TextBoxExternalId"), TextBox)
                TextBoxExternalId.ReadOnly = True

                TextBoxScheduleId = DirectCast(CurrentRow.FindControl("TextBoxScheduleId"), TextBox)
                TextBoxScheduleId.ReadOnly = True

                TextBoxDate = DirectCast(CurrentRow.FindControl("TextBoxDate"), TextBox)
                TextBoxDate.ReadOnly = True

                TextBoxPlannedTimeIn = DirectCast(CurrentRow.FindControl("TextBoxPlannedTimeIn"), TextBox)
                TextBoxPlannedTimeIn.ReadOnly = True

                TextBoxPlannedTimeOut = DirectCast(CurrentRow.FindControl("TextBoxPlannedTimeOut"), TextBox)
                TextBoxPlannedTimeOut.ReadOnly = True

                TextBoxPlannedDuration = DirectCast(CurrentRow.FindControl("TextBoxPlannedDuration"), TextBox)
                TextBoxPlannedDuration.ReadOnly = True

                TextBoxServiceCode = DirectCast(CurrentRow.FindControl("TextBoxServiceCode"), TextBox)
                TextBoxServiceCode.ReadOnly = True

                TextBoxActivityCode = DirectCast(CurrentRow.FindControl("TextBoxActivityCode"), TextBox)
                TextBoxActivityCode.ReadOnly = True

                TextBoxProgramCode = DirectCast(CurrentRow.FindControl("TextBoxProgramCode"), TextBox)
                TextBoxProgramCode.ReadOnly = True

                TextBoxClientExternalId = DirectCast(CurrentRow.FindControl("TextBoxClientExternalId"), TextBox)
                TextBoxClientExternalId.ReadOnly = True

                TextBoxStaffExternalId = DirectCast(CurrentRow.FindControl("TextBoxStaffExternalId"), TextBox)
                TextBoxStaffExternalId.ReadOnly = True

                TextBoxBillType = DirectCast(CurrentRow.FindControl("TextBoxBillType"), TextBox)
                TextBoxBillType.ReadOnly = True

                TextBoxPayType = DirectCast(CurrentRow.FindControl("TextBoxPayType"), TextBox)
                TextBoxPayType.ReadOnly = True

                TextBoxOccurredTimeIn = DirectCast(CurrentRow.FindControl("TextBoxOccurredTimeIn"), TextBox)
                TextBoxOccurredTimeIn.ReadOnly = True

                TextBoxOccurredTimeOut = DirectCast(CurrentRow.FindControl("TextBoxOccurredTimeOut"), TextBox)
                TextBoxOccurredTimeOut.ReadOnly = True

                TextBoxOccurredDuration = DirectCast(CurrentRow.FindControl("TextBoxOccurredDuration"), TextBox)
                TextBoxOccurredDuration.ReadOnly = True

                TextBoxStatus = DirectCast(CurrentRow.FindControl("TextBoxStatus"), TextBox)
                TextBoxStatus.ReadOnly = True

                TextBoxComments = DirectCast(CurrentRow.FindControl("TextBoxComments"), TextBox)
                TextBoxComments.ReadOnly = True

                TextBoxAction = DirectCast(CurrentRow.FindControl("TextBoxAction"), TextBox)
                TextBoxAction.ReadOnly = True

                TextBoxUpdateDate = DirectCast(CurrentRow.FindControl("TextBoxUpdateDate"), TextBox)
                TextBoxUpdateDate.ReadOnly = True

                TextBoxUpdateBy = DirectCast(CurrentRow.FindControl("TextBoxUpdateBy"), TextBox)
                TextBoxUpdateBy.ReadOnly = True

                LabelClientId = DirectCast(CurrentRow.FindControl("LabelClientId"), Label)

                TextBoxClientName = DirectCast(CurrentRow.FindControl("TextBoxClientName"), TextBox)
                If ((Not TextBoxClientName Is Nothing) And (Not LabelClientId Is Nothing)) Then
                    TextBoxClientName.ReadOnly = True
                End If

                LabelEmployeeId = DirectCast(CurrentRow.FindControl("LabelEmployeeId"), Label)

                TextBoxEmployeeName = DirectCast(CurrentRow.FindControl("TextBoxEmployeeName"), TextBox)
                If ((Not TextBoxEmployeeName Is Nothing) And (Not LabelEmployeeId Is Nothing)) Then
                    TextBoxEmployeeName.ReadOnly = True
                End If

                DropDownListClient = DirectCast(CurrentRow.FindControl("DropDownListClient"), DropDownList)

                If (Not DropDownListClient Is Nothing) Then
                    DropDownListClient.Visible = False
                End If

                DropDownListEmployee = DirectCast(CurrentRow.FindControl("DropDownListEmployee"), DropDownList)

                If (Not DropDownListEmployee Is Nothing) Then
                    DropDownListEmployee.Visible = False
                End If


                '**************************Fill Out Drop Down and Associate selection on Edit Mode[Start]***********************************
                If ((e.Row.RowState & DataControlRowState.Edit) > 0) Then
                    DropDownListClient = DirectCast(CurrentRow.FindControl("DropDownListClient"), DropDownList)

                    If (Not DropDownListClient Is Nothing) Then

                        objShared.BindClientDropDownList(DropDownListClient, objShared.CompanyId, EnumDataObject.ClientListFor.Individual)

                        DropDownListClient.SelectedIndex = DropDownListClient.Items.IndexOf(
                            DropDownListClient.Items.FindByValue(Convert.ToString(LabelClientId.Text.Trim(), Nothing)))

                    End If

                    DropDownListEmployee = DirectCast(CurrentRow.FindControl("DropDownListEmployee"), DropDownList)

                    If (Not DropDownListEmployee Is Nothing) Then

                        objShared.BindEmployeeDropDownList(DropDownListEmployee, objShared.CompanyId, False)

                        DropDownListEmployee.SelectedIndex = DropDownListEmployee.Items.IndexOf(
                            DropDownListEmployee.Items.FindByValue(Convert.ToString(LabelEmployeeId.Text.Trim(), Nothing)))

                    End If

                    'objShared.BindEmployeeDropDownList(DropDownListEmployee, objShared.CompanyId, False)
                End If
                '**************************Fill Out Drop Down and Associate selection on Edit Mode[End]***********************************

            End If

            If (CurrentRow.RowType.Equals(DataControlRowType.Footer)) Then

                CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)
                CheckBoxSelect.AutoPostBack = True

                DropDownListClient = DirectCast(CurrentRow.FindControl("DropDownListClient"), DropDownList)

                If (Not DropDownListClient Is Nothing) Then
                    objShared.BindClientDropDownList(DropDownListClient, objShared.CompanyId, EnumDataObject.ClientListFor.Individual)
                End If

                DropDownListEmployee = DirectCast(CurrentRow.FindControl("DropDownListEmployee"), DropDownList)

                If (Not DropDownListEmployee Is Nothing) Then
                    objShared.BindEmployeeDropDownList(DropDownListEmployee, objShared.CompanyId, False)
                End If

                TextBoxUpdateDate = DirectCast(CurrentRow.FindControl("TextBoxUpdateDate"), TextBox)
                TextBoxUpdateDate.ReadOnly = True

                TextBoxUpdateBy = DirectCast(CurrentRow.FindControl("TextBoxUpdateBy"), TextBox)
                TextBoxUpdateBy.ReadOnly = True

            End If

        End Sub

        Private Sub GridViewMEDsysSchedules_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
            GridViewMEDsysSchedules.PageIndex = e.NewPageIndex
            BindGridViewMEDsysSchedules()
        End Sub

        ''' <summary>
        ''' Gridview Row Hover Color Set
        ''' </summary>
        ''' <param name="writer"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub Render(writer As HtmlTextWriter)
            For Each r As GridViewRow In GridViewMEDsysSchedules.Rows
                If r.RowType = DataControlRowType.DataRow Then
                    objShared.SetGridViewRowColor(r)
                End If
            Next
            MyBase.Render(writer)
        End Sub

        Private Sub LoadJScript()

            LoadJS("JavaScript/jquery.blockUI.js")

            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                          & " var LoaderImagePath ='" & objShared.GetLoaderImagePath & "'; " _
                          & " var DeleteTargetButton ='ButtonDelete'; " _
                          & " var DeleteDialogHeader ='MEDsys Positions'; " _
                          & " var DeleteDialogConfirmMsg ='" & DeleteConfirmationMessage & "'; " _
                          & " var prm =''; " _
                          & " jQuery(document).ready(function () {" _
                          & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                          & "     prm.add_beginRequest(SetButtonActionProgress); " _
                          & "     DataDelete();" _
                          & "     prm.add_endRequest(DataDelete); " _
                          & "     prm.add_endRequest(EndRequest); " _
                          & "}); " _
                   & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

            LoadJS("JavaScript/EVV/" & ControlName & ".js")

        End Sub

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("EVV", ControlName & Convert.ToString(".resx"))

            LabelMEDsysSchedules.Text = Convert.ToString(ResourceTable("LabelMEDsysSchedules"), Nothing)
            LabelMEDsysSchedules.Text = If(String.IsNullOrEmpty(LabelMEDsysSchedules.Text), "MEDsys Schedules", LabelMEDsysSchedules.Text)

            ButtonSave.Text = Convert.ToString(ResourceTable("ButtonSave"), Nothing)
            ButtonSave.Text = If(String.IsNullOrEmpty(ButtonSave.Text), "Save", ButtonSave.Text)

            ButtonClear.Text = Convert.ToString(ResourceTable("ButtonClear"), Nothing)
            ButtonClear.Text = If(String.IsNullOrEmpty(ButtonClear.Text), "Clear", ButtonClear.Text)

            ButtonDelete.Text = Convert.ToString(ResourceTable("ButtonDelete"), Nothing)
            ButtonDelete.Text = If(String.IsNullOrEmpty(ButtonDelete.Text), "Delete", ButtonDelete.Text)

            ButtonIndividualDetail.Text = Convert.ToString(ResourceTable("ButtonIndividualDetail"), Nothing)
            ButtonIndividualDetail.Text = If(String.IsNullOrEmpty(ButtonIndividualDetail.Text), "Individual Detail", ButtonIndividualDetail.Text)

            ButtonEmployeeDetail.Text = Convert.ToString(ResourceTable("ButtonEmployeeDetail"), Nothing)
            ButtonEmployeeDetail.Text = If(String.IsNullOrEmpty(ButtonEmployeeDetail.Text), "Employee Detail", ButtonEmployeeDetail.Text)

            ButtonViewError.Text = Convert.ToString(ResourceTable("ButtonViewError"), Nothing)
            ButtonViewError.Text = If(String.IsNullOrEmpty(ButtonViewError.Text), "View Detail Error", ButtonViewError.Text)

            SaveMessage = Convert.ToString(ResourceTable("SaveMessage"), Nothing)
            SaveMessage = If(String.IsNullOrEmpty(SaveMessage), "Saved Successfully", SaveMessage)

            DeleteMessage = Convert.ToString(ResourceTable("DeleteMessage"), Nothing)
            DeleteMessage = If(String.IsNullOrEmpty(DeleteMessage), "Deleted Successfully", DeleteMessage)

            ValidationGroup = Convert.ToString(ResourceTable("ValidationGroup"), Nothing)
            ValidationGroup = If(String.IsNullOrEmpty(ValidationGroup), "MEDsysSchedules", ValidationGroup)

            DeleteConfirmationMessage = Convert.ToString(ResourceTable("DeleteConfirmationMessage"), Nothing)
            DeleteConfirmationMessage = If(String.IsNullOrEmpty(DeleteConfirmationMessage), "Are you sure you want to delete this record?", DeleteConfirmationMessage)

            Dim ResultOut As Boolean

            ValidationEnable = If((Boolean.TryParse(ResourceTable("ValidationEnable"), ResultOut)), ResultOut, True)

            ResourceTable = Nothing

        End Sub

        ''' <summary>
        ''' Reading GridView header text from resource file
        ''' </summary>
        ''' <param name="CurrentRow"></param>
        ''' <remarks></remarks>
        Private Sub SetGridViewColumnHeaderText(ByRef CurrentRow As GridViewRow)

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("EVV", ControlName & Convert.ToString(".resx"))

            LabelHeaderSerial = DirectCast(CurrentRow.FindControl("LabelHeaderSerial"), Label)
            LabelHeaderSerial.Text = Convert.ToString(ResourceTable("LabelHeaderSerial"), Nothing).Trim()
            LabelHeaderSerial.Text = If(String.IsNullOrEmpty(LabelHeaderSerial.Text), "SI.", LabelHeaderSerial.Text)

            LabelHeaderSelect = DirectCast(CurrentRow.FindControl("LabelHeaderSelect"), Label)
            LabelHeaderSelect.Text = Convert.ToString(ResourceTable("LabelHeaderSelect"), Nothing).Trim()
            LabelHeaderSelect.Text = If(String.IsNullOrEmpty(LabelHeaderSelect.Text), "Select", LabelHeaderSelect.Text)

            LabelHeaderClient = DirectCast(CurrentRow.FindControl("LabelHeaderClient"), Label)
            LabelHeaderClient.Text = Convert.ToString(ResourceTable("LabelHeaderClient"), Nothing).Trim()
            LabelHeaderClient.Text = If(String.IsNullOrEmpty(LabelHeaderClient.Text), "Client", LabelHeaderClient.Text)

            LabelHeaderEmployee = DirectCast(CurrentRow.FindControl("LabelHeaderEmployee"), Label)
            LabelHeaderEmployee.Text = Convert.ToString(ResourceTable("LabelHeaderEmployee"), Nothing).Trim()
            LabelHeaderEmployee.Text = If(String.IsNullOrEmpty(LabelHeaderEmployee.Text), "Employee", LabelHeaderEmployee.Text)

            LabelHeaderAccountId = DirectCast(CurrentRow.FindControl("LabelHeaderAccountId"), Label)
            LabelHeaderAccountId.Text = Convert.ToString(ResourceTable("LabelHeaderAccountId"), Nothing).Trim()
            LabelHeaderAccountId.Text = If(String.IsNullOrEmpty(LabelHeaderAccountId.Text), "Account Id", LabelHeaderAccountId.Text)

            LabelHeaderExternalId = DirectCast(CurrentRow.FindControl("LabelHeaderExternalId"), Label)
            LabelHeaderExternalId.Text = Convert.ToString(ResourceTable("LabelHeaderExternalId"), Nothing).Trim()
            LabelHeaderExternalId.Text = If(String.IsNullOrEmpty(LabelHeaderExternalId.Text), "External Id", LabelHeaderExternalId.Text)

            LabelHeaderScheduleId = DirectCast(CurrentRow.FindControl("LabelHeaderScheduleId"), Label)
            LabelHeaderScheduleId.Text = Convert.ToString(ResourceTable("LabelHeaderScheduleId"), Nothing).Trim()
            LabelHeaderScheduleId.Text = If(String.IsNullOrEmpty(LabelHeaderScheduleId.Text), "Schedule Id", LabelHeaderScheduleId.Text)

            LabelHeaderDate = DirectCast(CurrentRow.FindControl("LabelHeaderDate"), Label)
            LabelHeaderDate.Text = Convert.ToString(ResourceTable("LabelHeaderDate"), Nothing).Trim()
            LabelHeaderDate.Text = If(String.IsNullOrEmpty(LabelHeaderDate.Text), "Date", LabelHeaderDate.Text)

            LabelHeaderPlannedTimeIn = DirectCast(CurrentRow.FindControl("LabelHeaderPlannedTimeIn"), Label)
            LabelHeaderPlannedTimeIn.Text = Convert.ToString(ResourceTable("LabelHeaderPlannedTimeIn"), Nothing).Trim()
            LabelHeaderPlannedTimeIn.Text = If(String.IsNullOrEmpty(LabelHeaderPlannedTimeIn.Text), "Planned Time In", LabelHeaderPlannedTimeIn.Text)

            LabelHeaderPlannedTimeOut = DirectCast(CurrentRow.FindControl("LabelHeaderPlannedTimeOut"), Label)
            LabelHeaderPlannedTimeOut.Text = Convert.ToString(ResourceTable("LabelHeaderPlannedTimeOut"), Nothing).Trim()
            LabelHeaderPlannedTimeOut.Text = If(String.IsNullOrEmpty(LabelHeaderPlannedTimeOut.Text), "Planned Time Out", LabelHeaderPlannedTimeOut.Text)

            LabelHeaderPlannedDuration = DirectCast(CurrentRow.FindControl("LabelHeaderPlannedDuration"), Label)
            LabelHeaderPlannedDuration.Text = Convert.ToString(ResourceTable("LabelHeaderPlannedDuration"), Nothing).Trim()
            LabelHeaderPlannedDuration.Text = If(String.IsNullOrEmpty(LabelHeaderPlannedDuration.Text), "Planned Duration", LabelHeaderPlannedDuration.Text)

            LabelHeaderServiceCode = DirectCast(CurrentRow.FindControl("LabelHeaderServiceCode"), Label)
            LabelHeaderServiceCode.Text = Convert.ToString(ResourceTable("LabelHeaderServiceCode"), Nothing).Trim()
            LabelHeaderServiceCode.Text = If(String.IsNullOrEmpty(LabelHeaderServiceCode.Text), "Service Code", LabelHeaderServiceCode.Text)

            LabelHeaderActivityCode = DirectCast(CurrentRow.FindControl("LabelHeaderActivityCode"), Label)
            LabelHeaderActivityCode.Text = Convert.ToString(ResourceTable("LabelHeaderActivityCode"), Nothing).Trim()
            LabelHeaderActivityCode.Text = If(String.IsNullOrEmpty(LabelHeaderActivityCode.Text), "Activity Code", LabelHeaderActivityCode.Text)

            LabelHeaderProgramCode = DirectCast(CurrentRow.FindControl("LabelHeaderProgramCode"), Label)
            LabelHeaderProgramCode.Text = Convert.ToString(ResourceTable("LabelHeaderProgramCode"), Nothing).Trim()
            LabelHeaderProgramCode.Text = If(String.IsNullOrEmpty(LabelHeaderProgramCode.Text), "Program Code", LabelHeaderProgramCode.Text)

            LabelHeaderClientExternalId = DirectCast(CurrentRow.FindControl("LabelHeaderClientExternalId"), Label)
            LabelHeaderClientExternalId.Text = Convert.ToString(ResourceTable("LabelHeaderClientExternalId"), Nothing).Trim()
            LabelHeaderClientExternalId.Text = If(String.IsNullOrEmpty(LabelHeaderClientExternalId.Text), "Client External Id", LabelHeaderClientExternalId.Text)

            LabelHeaderBillType = DirectCast(CurrentRow.FindControl("LabelHeaderBillType"), Label)
            LabelHeaderBillType.Text = Convert.ToString(ResourceTable("LabelHeaderBillType"), Nothing).Trim()
            LabelHeaderBillType.Text = If(String.IsNullOrEmpty(LabelHeaderBillType.Text), "Bill Type", LabelHeaderBillType.Text)

            LabelHeaderPayType = DirectCast(CurrentRow.FindControl("LabelHeaderPayType"), Label)
            LabelHeaderPayType.Text = Convert.ToString(ResourceTable("LabelHeaderPayType"), Nothing).Trim()
            LabelHeaderPayType.Text = If(String.IsNullOrEmpty(LabelHeaderPayType.Text), "Pay Type", LabelHeaderPayType.Text)

            LabelHeaderOccurredTimeIn = DirectCast(CurrentRow.FindControl("LabelHeaderOccurredTimeIn"), Label)
            LabelHeaderOccurredTimeIn.Text = Convert.ToString(ResourceTable("LabelHeaderOccurredTimeIn"), Nothing).Trim()
            LabelHeaderOccurredTimeIn.Text = If(String.IsNullOrEmpty(LabelHeaderOccurredTimeIn.Text), "Occurred Time In", LabelHeaderOccurredTimeIn.Text)

            LabelHeaderOccurredTimeOut = DirectCast(CurrentRow.FindControl("LabelHeaderOccurredTimeOut"), Label)
            LabelHeaderOccurredTimeOut.Text = Convert.ToString(ResourceTable("LabelHeaderOccurredTimeOut"), Nothing).Trim()
            LabelHeaderOccurredTimeOut.Text = If(String.IsNullOrEmpty(LabelHeaderOccurredTimeOut.Text), "Occurred Time Out", LabelHeaderOccurredTimeOut.Text)

            LabelHeaderOccurredDuration = DirectCast(CurrentRow.FindControl("LabelHeaderOccurredDuration"), Label)
            LabelHeaderOccurredDuration.Text = Convert.ToString(ResourceTable("LabelHeaderOccurredDuration"), Nothing).Trim()
            LabelHeaderOccurredDuration.Text = If(String.IsNullOrEmpty(LabelHeaderOccurredDuration.Text), "Occurred Duration", LabelHeaderOccurredDuration.Text)

            LabelHeaderStatus = DirectCast(CurrentRow.FindControl("LabelHeaderStatus"), Label)
            LabelHeaderStatus.Text = Convert.ToString(ResourceTable("LabelHeaderStatus"), Nothing).Trim()
            LabelHeaderStatus.Text = If(String.IsNullOrEmpty(LabelHeaderStatus.Text), "Status", LabelHeaderStatus.Text)

            LabelHeaderComments = DirectCast(CurrentRow.FindControl("LabelHeaderComments"), Label)
            LabelHeaderComments.Text = Convert.ToString(ResourceTable("LabelHeaderComments"), Nothing).Trim()
            LabelHeaderComments.Text = If(String.IsNullOrEmpty(LabelHeaderComments.Text), "Comments", LabelHeaderComments.Text)

            LabelHeaderAction = DirectCast(CurrentRow.FindControl("LabelHeaderAction"), Label)
            LabelHeaderAction.Text = Convert.ToString(ResourceTable("LabelHeaderAction"), Nothing).Trim()
            LabelHeaderAction.Text = If(String.IsNullOrEmpty(LabelHeaderAction.Text), "Action", LabelHeaderAction.Text)

            LabelHeaderUpdateDate = DirectCast(CurrentRow.FindControl("LabelHeaderUpdateDate"), Label)
            LabelHeaderUpdateDate.Text = Convert.ToString(ResourceTable("LabelHeaderUpdateDate"), Nothing).Trim()
            LabelHeaderUpdateDate.Text = If(String.IsNullOrEmpty(LabelHeaderUpdateDate.Text), "Update Date", LabelHeaderUpdateDate.Text)

            LabelHeaderUpdateBy = DirectCast(CurrentRow.FindControl("LabelHeaderUpdateBy"), Label)
            LabelHeaderUpdateBy.Text = Convert.ToString(ResourceTable("LabelHeaderUpdateBy"), Nothing).Trim()
            LabelHeaderUpdateBy.Text = If(String.IsNullOrEmpty(LabelHeaderUpdateBy.Text), "Update By", LabelHeaderUpdateBy.Text)

            ResourceTable = Nothing

        End Sub

        ''' <summary>
        ''' This is for control events regristering and instantiate object globally
        ''' </summary>
        Private Sub InitializeControl()

            AddHandler ButtonViewError.Click, AddressOf ButtonViewError_Click
            ButtonViewError.Visible = False

            AddHandler ButtonClear.Click, AddressOf ButtonClear_Click
            AddHandler ButtonSave.Click, AddressOf ButtonSave_Click
            AddHandler ButtonDelete.Click, AddressOf ButtonDelete_Click
            AddHandler ButtonIndividualDetail.Click, AddressOf ButtonIndividualDetail_Click
            AddHandler ButtonEmployeeDetail.Click, AddressOf ButtonEmployeeDetail_Click

            GridViewMEDsysSchedules.AutoGenerateColumns = False
            GridViewMEDsysSchedules.ShowHeaderWhenEmpty = True
            GridViewMEDsysSchedules.AllowPaging = True
            GridViewMEDsysSchedules.AllowSorting = True

            GridViewMEDsysSchedules.ShowFooter = True

            If (GridViewMEDsysSchedules.AllowPaging) Then
                GridViewMEDsysSchedules.PageSize = objShared.GridViewDefaultPageSize
            End If

            AddHandler GridViewMEDsysSchedules.RowDataBound, AddressOf GridViewMEDsysSchedules_RowDataBound
            AddHandler GridViewMEDsysSchedules.PageIndexChanging, AddressOf GridViewMEDsysSchedules_PageIndexChanging

            ButtonClear.ClientIDMode = UI.ClientIDMode.Static
            ButtonSave.ClientIDMode = UI.ClientIDMode.Static
            ButtonDelete.ClientIDMode = UI.ClientIDMode.Static
            ButtonIndividualDetail.ClientIDMode = UI.ClientIDMode.Static
            ButtonEmployeeDetail.ClientIDMode = UI.ClientIDMode.Static

        End Sub

        ''' <summary>
        ''' This would generate excel file containing error Occurred during database operation
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub ButtonViewError_Click(sender As Object, e As EventArgs)

            Dim dt As DataTable

            If (ViewState("ListFor").Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE))) Then
                dt = DirectCast(ViewState("SaveFailedRecord"), DataTable)
                objShared.ExportExcel(dt, "Save Failed Records", "", "SaveFailedRecords")
            End If

            If (ViewState("ListFor").Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE))) Then
                dt = DirectCast(ViewState("DeleteFailedRecord"), DataTable)
                objShared.ExportExcel(dt, "Delete Failed Records", "", "DeleteFailedRecords")
            End If

        End Sub

        ''' <summary>
        ''' Clearing controls value
        ''' </summary>
        Private Sub ClearControl(CurrentRow As GridViewRow)
            ControlsFind(CurrentRow)
            BindGridViewMEDsysSchedules()
            ButtonViewError.Visible = False
        End Sub

        Private Sub GetData()
            GetMEDsysScheduleData()
        End Sub

        Private Sub GetMEDsysScheduleData()

            Dim objBLMEDsysSchedules As New BLMEDsysSchedules()

            Try
                MEDsysSchedules = objBLMEDsysSchedules.SelectMEDsysScheduleInfo(objShared.ConVisitel, IsLog)
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to fetch MEDsys Schedule Data. Message: {0}", ex.Message))
            Finally
                objBLMEDsysSchedules = Nothing
            End Try
        End Sub

        Private Sub BindGridViewMEDsysSchedules()
            GridViewMEDsysSchedules.DataSource = MEDsysSchedules
            GridViewMEDsysSchedules.DataBind()

            ItemChecked = DetermineButtonInActivity(GridViewMEDsysSchedules, "CheckBoxSelect")

            ButtonSave.Enabled = ItemChecked
            ButtonDelete.Enabled = ButtonSave.Enabled
            ButtonIndividualDetail.Enabled = ButtonSave.Enabled
            ButtonEmployeeDetail.Enabled = ButtonSave.Enabled
        End Sub
    End Class
End Namespace