Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports System.Data.SqlClient
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelCommon
Imports VisitelBusiness.VisitelBusiness.DataObject.EVV

Namespace Visitel.UserControl.EVV
    Public Class MEDsysStaffsControl
        Inherits CommonDataControl

        Private ContractNumber As String
        Private objShared As SharedWebControls

        Private CheckBoxSelect As CheckBox, chkAll As CheckBox
        Private ItemChecked As Boolean = False

        Private LabelHeaderSerial As Label, LabelHeaderSelect As Label, LabelHeaderAccountId As Label, LabelHeaderExternalId As Label, LabelHeaderStaffId As Label, _
            LabelHeaderStaffNumber As Label, LabelHeaderLastName As Label, LabelHeaderFirstName As Label, LabelHeaderMiddleInitial As Label, LabelHeaderDateOfBirth As Label, _
            LabelHeaderGender As Label, LabelHeaderGIN As Label, LabelHeaderAddress As Label, LabelHeaderCity As Label, LabelHeaderState As Label, LabelHeaderZip As Label, _
            LabelHeaderPhone As Label, LabelHeaderNotes As Label, LabelHeaderPositionCode As Label, LabelHeaderTeamCode As Label, LabelHeaderStatus As Label, _
            LabelHeaderStatusDate As Label, LabelHeaderStartDate As Label, LabelHeaderEndDate As Label, LabelHeaderAction As Label, LabelHeaderEmployeeId As Label, _
            LabelHeaderCompanyCode As Label, LabelHeaderUpdateDate As Label, LabelHeaderUpdateBy As Label, LabelId As Label, LabelStaffId As Label

        Private TextBoxAccountId As TextBox, TextBoxExternalId As TextBox, TextBoxStaffId As TextBox, TextBoxStaffNumber As TextBox, TextBoxLastName As TextBox, TextBoxFirstName As TextBox, _
            TextBoxMiddleInitial As TextBox, TextBoxDateOfBirth As TextBox, TextBoxGender As TextBox, TextBoxGIN As TextBox, TextBoxAddress As TextBox, _
            TextBoxCity As TextBox, TextBoxState As TextBox, TextBoxZip As TextBox, TextBoxPhone As TextBox, TextBoxNotes As TextBox, TextBoxPositionCode As TextBox, _
            TextBoxTeamCode As TextBox, TextBoxStatus As TextBox, TextBoxStatusDate As TextBox, TextBoxStartDate As TextBox, TextBoxEndDate As TextBox, _
            TextBoxAction As TextBox, TextBoxEmployeeId As TextBox, TextBoxCompanyCode As TextBox, TextBoxUpdateDate As TextBox, TextBoxUpdateBy As TextBox

        Private ControlName As String, ValidationGroup As String, SaveMessage As String, DeleteMessage As String, DeleteConfirmationMessage As String, ListFor As String
        Private ValidationEnable As Boolean
        Private CurrentRow As GridViewRow

        Private Staffs As List(Of MEDsysStaffDataObject), StaffsErrorList As List(Of MEDsysStaffDataObject)

        Private SaveFailedCounter As Int16, DeleteFailedCounter As Int16, RecordSaveCount As Int16


        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "MEDsysStaffsControl"
            objShared = New SharedWebControls()
            objShared.ConnectionOpen()
            GetCaptionFromResource()
            InitializeControl()
        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)

            GetData()

            If Not IsPostBack Then
                BindGridViewMEDsysStaffs()
            End If

        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadCss("EVV/" & ControlName)
            LoadJScript()
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing
            Staffs = Nothing
        End Sub


        Private Sub ButtonSave_Click(sender As Object, e As EventArgs)

            Page.Validate()
            Page.Validate(ValidationGroup)

            Staffs = New List(Of MEDsysStaffDataObject)()
            StaffsErrorList = New List(Of MEDsysStaffDataObject)()

            SaveFailedCounter = 0
            RecordSaveCount = 0

            CurrentRow = GridViewMEDsysStaffs.FooterRow

            CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)

            LabelStaffId = DirectCast(CurrentRow.FindControl("LabelStaffId"), Label)

            If ((CheckBoxSelect.Checked) And (String.IsNullOrEmpty(LabelStaffId.Text.Trim()))) Then
                SaveData(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT))
            End If

            SaveData(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE))

            If (SaveFailedCounter > 0) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} Records Failed to Save out of {1}", SaveFailedCounter, Staffs.Count))
                ViewState("SaveFailedRecord") = objShared.ToDataTable(StaffsErrorList)
                ViewState("ListFor") = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)
            Else
                ViewState("SaveFailedRecord") = Nothing

                If (RecordSaveCount > 0) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} record(s) {1}.", RecordSaveCount, SaveMessage))
                    ViewState.Clear()
                    GetData()
                    BindGridViewMEDsysStaffs()
                End If

            End If

            ButtonViewError.Visible = If((SaveFailedCounter > 0), True, False)

            Staffs = Nothing
            StaffsErrorList = Nothing

        End Sub

        Private Sub ButtonResetPositionCode_Click(sender As Object, e As EventArgs)
            Dim objBLMEDsysStaff As New BLMEDsysStaff()

            Try
                objBLMEDsysStaff.ResetMEDsysPositionCode(objShared.ConVisitel, TextBoxResetPositionCode.Text.Trim())
            Catch ex As Exception
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to reset MEDsys Position Code")
            Finally
                objBLMEDsysStaff = Nothing
                GetData()
                BindGridViewMEDsysStaffs()
            End Try
        End Sub

        Private Sub ButtonEmployeeDetail_Click(sender As Object, e As EventArgs)
            Response.Redirect("~/Pages/EmployeeInfo.aspx?EmployeeId=" & HiddenFieldEmployeeId.Value)
        End Sub


        Private Sub ButtonDelete_Click(sender As Object, e As EventArgs)
            Staffs = New List(Of MEDsysStaffDataObject)()
            StaffsErrorList = New List(Of MEDsysStaffDataObject)()

            MakeList(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE))

            If (Staffs.Count.Equals(0)) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please select items to delete")
                Return
            End If

            DeleteFailedCounter = 0

            TakeAction(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE))

            If (DeleteFailedCounter > 0) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} Records Failed to Delete out of {1}", DeleteFailedCounter, Staffs.Count))
                ViewState("DeleteFailedRecord") = objShared.ToDataTable(StaffsErrorList)
                ViewState("ListFor") = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE)
            Else
                ViewState("DeleteFailedRecord") = Nothing

                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} record(s) {1}.", Staffs.Count, DeleteMessage))
                ViewState.Clear()
                GetData()
                BindGridViewMEDsysStaffs()
            End If

            ButtonViewError.Visible = If((DeleteFailedCounter > 0), True, False)

            Staffs = Nothing
            StaffsErrorList = Nothing
        End Sub

        Private Sub ButtonClear_Click(sender As Object, e As EventArgs)
            CurrentRow = GridViewMEDsysStaffs.FooterRow
            ClearControl(CurrentRow)
        End Sub

        Protected Sub OnCheckedChanged(sender As Object, e As EventArgs)
            Dim isUpdateVisible As Boolean = False

            Dim chk As CheckBox = TryCast(sender, CheckBox)

            'ButtonSave.Enabled = If((chk.Checked), True, False)

            If chk.ID = "chkAll" Then
                For Each row As GridViewRow In GridViewMEDsysStaffs.Rows
                    If row.RowType = DataControlRowType.DataRow Then
                        row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked = chk.Checked
                    End If
                Next

                CurrentRow = GridViewMEDsysStaffs.FooterRow
                CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)
                CheckBoxSelect.Checked = chk.Checked

            End If

            Dim chkAll As CheckBox = TryCast(GridViewMEDsysStaffs.HeaderRow.FindControl("chkAll"), CheckBox)
            'chkAll.Checked = True
            For Each row As GridViewRow In GridViewMEDsysStaffs.Rows
                If row.RowType = DataControlRowType.DataRow Then

                    Dim isChecked As Boolean = row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked

                    ControlsOnSelect(row, isChecked)

                    If (isChecked) Then
                        LabelStaffId = DirectCast(row.FindControl("LabelStaffId"), Label)
                        If ((Not HiddenFieldEmployeeId Is Nothing) And (Not LabelStaffId Is Nothing)) Then
                            HiddenFieldEmployeeId.Value = LabelStaffId.Text.Trim()
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

            ItemChecked = DetermineButtonInActivity(GridViewMEDsysStaffs, "CheckBoxSelect")

            ButtonSave.Enabled = ItemChecked
            ButtonDelete.Enabled = ButtonSave.Enabled
            ButtonEmployeeDetail.Enabled = ButtonSave.Enabled

        End Sub

        ''' <summary>
        ''' Saving Data either insert or update after making list
        ''' </summary>
        ''' <param name="Action"></param>
        ''' <remarks></remarks>
        Private Sub SaveData(Action As String)

            MakeList(Action)

            If (Staffs.Count.Equals(0)) Then
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

            Dim objMEDsysStaffDataObject As MEDsysStaffDataObject

            Select Case Action
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT)

                    TextBoxStaffId = DirectCast(GridViewMEDsysStaffs.FooterRow.FindControl("TextBoxStaffId"), TextBox)
                    If (String.IsNullOrEmpty(TextBoxStaffId.Text.Trim())) Then
                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please type one staff id.")
                        Return
                    End If

                    objMEDsysStaffDataObject = New MEDsysStaffDataObject()
                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)

                    AddToList(CurrentRow, Staffs, objMEDsysStaffDataObject)
                    RecordSaveCount = RecordSaveCount + 1
                    objMEDsysStaffDataObject = Nothing

                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE),
                    EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE)
                    Staffs.Clear()
                    For Each row As GridViewRow In GridViewMEDsysStaffs.Rows
                        If (row.RowType.Equals(DataControlRowType.DataRow)) Then
                            Dim isChecked As Boolean = row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked

                            If (isChecked) Then

                                objMEDsysStaffDataObject = New MEDsysStaffDataObject()

                                CurrentRow = DirectCast(GridViewMEDsysStaffs.Rows(row.RowIndex), GridViewRow)

                                TextBoxStaffId = DirectCast(CurrentRow.FindControl("TextBoxStaffId"), TextBox)
                                If (String.IsNullOrEmpty(TextBoxStaffId.Text.Trim())) Then
                                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please type one staff id.")
                                    Return
                                End If

                                If (Action.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE))) Then
                                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)
                                End If

                                If (Action.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE))) Then
                                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE)
                                End If

                                AddToList(CurrentRow, Staffs, objMEDsysStaffDataObject)
                                RecordSaveCount = RecordSaveCount + 1
                                objMEDsysStaffDataObject = Nothing

                            End If

                        End If
                    Next

                    Exit Select
            End Select

            objMEDsysStaffDataObject = Nothing

        End Sub

        ''' <summary>
        ''' Making List in order to save records or delete
        ''' </summary>
        ''' <param name="CurrentRow"></param>
        ''' <param name="Staffs"></param>
        ''' <param name="objMEDsysStaffDataObject"></param>
        ''' <remarks></remarks>
        Private Sub AddToList(ByRef CurrentRow As GridViewRow, ByRef Staffs As List(Of MEDsysStaffDataObject), ByRef objMEDsysStaffDataObject As MEDsysStaffDataObject)

            Dim Int32Result As Int32

            LabelId = DirectCast(CurrentRow.FindControl("LabelId"), Label)
            LabelStaffId = DirectCast(CurrentRow.FindControl("LabelStaffId"), Label)

            If (ListFor.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE))) Then
                ControlsFind(CurrentRow)
            End If

            If (Page.IsValid) Then

                objMEDsysStaffDataObject.Id = If(Int32.TryParse(LabelId.Text.Trim(), Int32Result), Int32Result, objMEDsysStaffDataObject.Id)

                If (ListFor.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE))) Then

                    objMEDsysStaffDataObject.AccountId = TextBoxAccountId.Text.Trim()
                    objMEDsysStaffDataObject.ExternalId = TextBoxExternalId.Text.Trim()
                    objMEDsysStaffDataObject.StaffId = TextBoxStaffId.Text.Trim()
                    objMEDsysStaffDataObject.StaffNumber = TextBoxStaffNumber.Text.Trim()
                    objMEDsysStaffDataObject.FirstName = TextBoxFirstName.Text.Trim()
                    objMEDsysStaffDataObject.MiddleInit = TextBoxMiddleInitial.Text.Trim()
                    objMEDsysStaffDataObject.LastName = TextBoxLastName.Text.Trim()
                    objMEDsysStaffDataObject.Birthdate = TextBoxDateOfBirth.Text.Trim()
                    objMEDsysStaffDataObject.Gender = TextBoxGender.Text.Trim()
                    objMEDsysStaffDataObject.GIN = TextBoxGIN.Text.Trim()
                    objMEDsysStaffDataObject.Address = TextBoxAddress.Text.Trim()
                    objMEDsysStaffDataObject.City = TextBoxCity.Text.Trim()
                    objMEDsysStaffDataObject.State = TextBoxState.Text.Trim()
                    objMEDsysStaffDataObject.Zip = TextBoxZip.Text.Trim()
                    objMEDsysStaffDataObject.Phone = TextBoxPhone.Text.Trim
                    objMEDsysStaffDataObject.Notes = TextBoxNotes.Text.Trim()
                    objMEDsysStaffDataObject.PositionCode = TextBoxPositionCode.Text.Trim()
                    objMEDsysStaffDataObject.TeamCode = TextBoxTeamCode.Text.Trim()
                    objMEDsysStaffDataObject.Status = TextBoxStatus.Text.Trim()
                    objMEDsysStaffDataObject.StatusDate = TextBoxStatusDate.Text.Trim()
                    objMEDsysStaffDataObject.StartDate = TextBoxStartDate.Text.Trim()
                    objMEDsysStaffDataObject.EndDate = TextBoxEndDate.Text.Trim()
                    objMEDsysStaffDataObject.Action = TextBoxAction.Text.Trim()
                    objMEDsysStaffDataObject.EmployeeId = TextBoxEmployeeId.Text.Trim()
                    objMEDsysStaffDataObject.CompanyCode = TextBoxCompanyCode.Text.Trim()
                End If

                objMEDsysStaffDataObject.UpdateBy = objShared.UserId

                Staffs.Add(objMEDsysStaffDataObject)
            End If

        End Sub

        ''' <summary>
        ''' Make Database operation either for record saving or deleting
        ''' </summary>
        ''' <param name="Action"></param>
        ''' <remarks></remarks>
        Private Sub TakeAction(Action As String)

            Dim objBLMEDsysStaff As New BLMEDsysStaff()

            For Each Staff In Staffs
                Try
                    Select Case Action
                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT)
                            objBLMEDsysStaff.InsertMEDsysStaffInfo(objShared.ConVisitel, Staff)
                            Exit Select

                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE)
                            objBLMEDsysStaff.UpdateMEDsysStaffInfo(objShared.ConVisitel, Staff)
                            Exit Select

                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE)
                            objBLMEDsysStaff.DeleteMEDsysStaffInfo(objShared.ConVisitel, Staff.Id, Staff.UpdateBy)
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

                    Staff.Remarks = ex.Message
                    StaffsErrorList.Add(Staff)

                Finally

                End Try
            Next

            objBLMEDsysStaff = Nothing

        End Sub

        Private Sub ControlsFind(ByRef CurrentRow As GridViewRow)
            TextBoxAccountId = DirectCast(CurrentRow.FindControl("TextBoxAccountId"), TextBox)
            TextBoxExternalId = DirectCast(CurrentRow.FindControl("TextBoxExternalId"), TextBox)
            TextBoxStaffId = DirectCast(CurrentRow.FindControl("TextBoxStaffId"), TextBox)
            TextBoxStaffNumber = DirectCast(CurrentRow.FindControl("TextBoxStaffNumber"), TextBox)
            TextBoxLastName = DirectCast(CurrentRow.FindControl("TextBoxLastName"), TextBox)
            TextBoxFirstName = DirectCast(CurrentRow.FindControl("TextBoxFirstName"), TextBox)
            TextBoxMiddleInitial = DirectCast(CurrentRow.FindControl("TextBoxMiddleInitial"), TextBox)
            TextBoxDateOfBirth = DirectCast(CurrentRow.FindControl("TextBoxDateOfBirth"), TextBox)
            TextBoxGender = DirectCast(CurrentRow.FindControl("TextBoxGender"), TextBox)
            TextBoxGIN = DirectCast(CurrentRow.FindControl("TextBoxGIN"), TextBox)
            TextBoxAddress = DirectCast(CurrentRow.FindControl("TextBoxAddress"), TextBox)
            TextBoxCity = DirectCast(CurrentRow.FindControl("TextBoxCity"), TextBox)
            TextBoxState = DirectCast(CurrentRow.FindControl("TextBoxState"), TextBox)
            TextBoxZip = DirectCast(CurrentRow.FindControl("TextBoxZip"), TextBox)
            TextBoxPhone = DirectCast(CurrentRow.FindControl("TextBoxPhone"), TextBox)
            TextBoxNotes = DirectCast(CurrentRow.FindControl("TextBoxNotes"), TextBox)
            TextBoxPositionCode = DirectCast(CurrentRow.FindControl("TextBoxPositionCode"), TextBox)
            TextBoxTeamCode = DirectCast(CurrentRow.FindControl("TextBoxTeamCode"), TextBox)
            TextBoxStatus = DirectCast(CurrentRow.FindControl("TextBoxStatus"), TextBox)
            TextBoxStatusDate = DirectCast(CurrentRow.FindControl("TextBoxStatusDate"), TextBox)
            TextBoxStartDate = DirectCast(CurrentRow.FindControl("TextBoxStartDate"), TextBox)
            TextBoxEndDate = DirectCast(CurrentRow.FindControl("TextBoxEndDate"), TextBox)
            TextBoxAction = DirectCast(CurrentRow.FindControl("TextBoxAction"), TextBox)
            TextBoxEmployeeId = DirectCast(CurrentRow.FindControl("TextBoxEmployeeId"), TextBox)
            TextBoxCompanyCode = DirectCast(CurrentRow.FindControl("TextBoxCompanyCode"), TextBox)
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

            If (Not TextBoxStaffId Is Nothing) Then
                TextBoxStaffId.ReadOnly = Not isChecked
                TextBoxStaffId.CssClass = If((TextBoxStaffId.ReadOnly), "TextBoxStaffId", "TextBoxStaffIdEdit")
            End If

            If (Not TextBoxLastName Is Nothing) Then
                TextBoxLastName.ReadOnly = Not isChecked
                TextBoxLastName.CssClass = If((TextBoxLastName.ReadOnly), "TextBoxLastName", "TextBoxLastNameEdit")
            End If

            If (Not TextBoxFirstName Is Nothing) Then
                TextBoxFirstName.ReadOnly = Not isChecked
                TextBoxFirstName.CssClass = If((TextBoxFirstName.ReadOnly), "TextBoxFirstName", "TextBoxFirstNameEdit")
            End If

            If (Not TextBoxMiddleInitial Is Nothing) Then
                TextBoxMiddleInitial.ReadOnly = Not isChecked
                TextBoxMiddleInitial.CssClass = If((TextBoxMiddleInitial.ReadOnly), "TextBoxMiddleInitial", "TextBoxMiddleInitialEdit")
            End If

            If (Not TextBoxDateOfBirth Is Nothing) Then
                TextBoxDateOfBirth.ReadOnly = Not isChecked
                TextBoxDateOfBirth.CssClass = If((TextBoxDateOfBirth.ReadOnly), "TextBoxDateOfBirth", "TextBoxDateOfBirthEdit")
            End If

            If (Not TextBoxGender Is Nothing) Then
                TextBoxGender.ReadOnly = Not isChecked
                TextBoxGender.CssClass = If((TextBoxGender.ReadOnly), "TextBoxGender", "TextBoxGenderEdit")
            End If

            If (Not TextBoxGIN Is Nothing) Then
                TextBoxGIN.ReadOnly = Not isChecked
                TextBoxGIN.CssClass = If((TextBoxGIN.ReadOnly), "TextBoxGIN", "TextBoxGINEdit")
            End If

            If (Not TextBoxAddress Is Nothing) Then
                TextBoxAddress.ReadOnly = Not isChecked
                TextBoxAddress.CssClass = If((TextBoxAddress.ReadOnly), "TextBoxAddress", "TextBoxAddressEdit")
            End If

            If (Not TextBoxCity Is Nothing) Then
                TextBoxCity.ReadOnly = Not isChecked
                TextBoxCity.CssClass = If((TextBoxCity.ReadOnly), "TextBoxCity", "TextBoxCityEdit")
            End If

            If (Not TextBoxState Is Nothing) Then
                TextBoxState.ReadOnly = Not isChecked
                TextBoxState.CssClass = If((TextBoxState.ReadOnly), "TextBoxState", "TextBoxStateEdit")
            End If

            If (Not TextBoxZip Is Nothing) Then
                TextBoxZip.ReadOnly = Not isChecked
                TextBoxZip.CssClass = If((TextBoxZip.ReadOnly), "TextBoxZip", "TextBoxZipEdit")
            End If

            If (Not TextBoxPhone Is Nothing) Then
                TextBoxPhone.ReadOnly = Not isChecked
                TextBoxPhone.CssClass = If((TextBoxPhone.ReadOnly), "TextBoxPhone", "TextBoxPhoneEdit")
            End If

            If (Not TextBoxNotes Is Nothing) Then
                TextBoxNotes.ReadOnly = Not isChecked
                TextBoxNotes.CssClass = If((TextBoxNotes.ReadOnly), "TextBoxNotes", "TextBoxNotesEdit")
            End If

            If (Not TextBoxPositionCode Is Nothing) Then
                TextBoxPositionCode.ReadOnly = Not isChecked
                TextBoxPositionCode.CssClass = If((TextBoxPositionCode.ReadOnly), "TextBoxPositionCode", "TextBoxPositionCodeEdit")
            End If

            If (Not TextBoxTeamCode Is Nothing) Then
                TextBoxTeamCode.ReadOnly = Not isChecked
                TextBoxTeamCode.CssClass = If((TextBoxTeamCode.ReadOnly), "TextBoxTeamCode", "TextBoxTeamCodeEdit")
            End If

            If (Not TextBoxStatus Is Nothing) Then
                TextBoxStatus.ReadOnly = Not isChecked
                TextBoxStatus.CssClass = If((TextBoxStatus.ReadOnly), "TextBoxStatus", "TextBoxStatusEdit")
            End If

            If (Not TextBoxStatusDate Is Nothing) Then
                TextBoxStatusDate.ReadOnly = Not isChecked
                TextBoxStatusDate.CssClass = If((TextBoxStatusDate.ReadOnly), "TextBoxStatusDate", "TextBoxStatusDateEdit")
            End If

            If (Not TextBoxStartDate Is Nothing) Then
                TextBoxStartDate.ReadOnly = Not isChecked
                TextBoxStartDate.CssClass = If((TextBoxStartDate.ReadOnly), "TextBoxStartDate", "TextBoxStartDateEdit")
            End If

            If (Not TextBoxEndDate Is Nothing) Then
                TextBoxEndDate.ReadOnly = Not isChecked
                TextBoxEndDate.CssClass = If((TextBoxEndDate.ReadOnly), "TextBoxEndDate", "TextBoxEndDateEdit")
            End If

            If (Not TextBoxAction Is Nothing) Then
                TextBoxAction.ReadOnly = Not isChecked
                TextBoxAction.CssClass = If((TextBoxAction.ReadOnly), "TextBoxAction", "TextBoxActionEdit")
            End If

            If (Not TextBoxEmployeeId Is Nothing) Then
                TextBoxEmployeeId.ReadOnly = Not isChecked
                TextBoxEmployeeId.CssClass = If((TextBoxEmployeeId.ReadOnly), "TextBoxEmployeeId", "TextBoxEmployeeIdEdit")
            End If

            If (Not TextBoxCompanyCode Is Nothing) Then
                TextBoxCompanyCode.ReadOnly = Not isChecked
                TextBoxCompanyCode.CssClass = If((TextBoxCompanyCode.ReadOnly), "TextBoxCompanyCode", "TextBoxCompanyCodeEdit")
            End If

        End Sub

        Private Sub GridViewMEDsysStaffs_RowDataBound(sender As Object, e As GridViewRowEventArgs)
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

                TextBoxAccountId = DirectCast(CurrentRow.FindControl("TextBoxAccountId"), TextBox)
                TextBoxAccountId.ReadOnly = True

                TextBoxExternalId = DirectCast(CurrentRow.FindControl("TextBoxExternalId"), TextBox)
                TextBoxExternalId.ReadOnly = True

                TextBoxStaffId = DirectCast(CurrentRow.FindControl("TextBoxStaffId"), TextBox)
                TextBoxStaffId.ReadOnly = True

                TextBoxLastName = DirectCast(CurrentRow.FindControl("TextBoxLastName"), TextBox)
                TextBoxLastName.ReadOnly = True

                TextBoxFirstName = DirectCast(CurrentRow.FindControl("TextBoxFirstName"), TextBox)
                TextBoxFirstName.ReadOnly = True

                TextBoxMiddleInitial = DirectCast(CurrentRow.FindControl("TextBoxMiddleInitial"), TextBox)
                TextBoxMiddleInitial.ReadOnly = True

                TextBoxDateOfBirth = DirectCast(CurrentRow.FindControl("TextBoxDateOfBirth"), TextBox)
                TextBoxDateOfBirth.ReadOnly = True

                TextBoxGender = DirectCast(CurrentRow.FindControl("TextBoxGender"), TextBox)
                TextBoxGender.ReadOnly = True

                TextBoxGIN = DirectCast(CurrentRow.FindControl("TextBoxGIN"), TextBox)
                TextBoxGIN.ReadOnly = True

                TextBoxAddress = DirectCast(CurrentRow.FindControl("TextBoxAddress"), TextBox)
                TextBoxAddress.ReadOnly = True

                TextBoxCity = DirectCast(CurrentRow.FindControl("TextBoxCity"), TextBox)
                TextBoxCity.ReadOnly = True

                TextBoxState = DirectCast(CurrentRow.FindControl("TextBoxState"), TextBox)
                TextBoxState.ReadOnly = True

                TextBoxZip = DirectCast(CurrentRow.FindControl("TextBoxZip"), TextBox)
                TextBoxZip.ReadOnly = True

                TextBoxPhone = DirectCast(CurrentRow.FindControl("TextBoxPhone"), TextBox)
                TextBoxPhone.ReadOnly = True

                TextBoxNotes = DirectCast(CurrentRow.FindControl("TextBoxNotes"), TextBox)
                TextBoxNotes.ReadOnly = True

                TextBoxPositionCode = DirectCast(CurrentRow.FindControl("TextBoxPositionCode"), TextBox)
                TextBoxPositionCode.ReadOnly = True

                TextBoxTeamCode = DirectCast(CurrentRow.FindControl("TextBoxTeamCode"), TextBox)
                TextBoxTeamCode.ReadOnly = True

                TextBoxStatus = DirectCast(CurrentRow.FindControl("TextBoxStatus"), TextBox)
                TextBoxStatus.ReadOnly = True

                TextBoxStatusDate = DirectCast(CurrentRow.FindControl("TextBoxStatusDate"), TextBox)
                TextBoxStatusDate.ReadOnly = True

                TextBoxStartDate = DirectCast(CurrentRow.FindControl("TextBoxStartDate"), TextBox)
                TextBoxStartDate.ReadOnly = True

                TextBoxEndDate = DirectCast(CurrentRow.FindControl("TextBoxEndDate"), TextBox)
                TextBoxEndDate.ReadOnly = True

                TextBoxAction = DirectCast(CurrentRow.FindControl("TextBoxAction"), TextBox)
                TextBoxAction.ReadOnly = True

                TextBoxEmployeeId = DirectCast(CurrentRow.FindControl("TextBoxEmployeeId"), TextBox)
                TextBoxEmployeeId.ReadOnly = True

                TextBoxCompanyCode = DirectCast(CurrentRow.FindControl("TextBoxCompanyCode"), TextBox)
                TextBoxCompanyCode.ReadOnly = True

                TextBoxUpdateDate = DirectCast(CurrentRow.FindControl("TextBoxUpdateDate"), TextBox)
                TextBoxUpdateDate.ReadOnly = True

                TextBoxUpdateBy = DirectCast(CurrentRow.FindControl("TextBoxUpdateBy"), TextBox)
                TextBoxUpdateBy.ReadOnly = True

            End If

            If (CurrentRow.RowType.Equals(DataControlRowType.Footer)) Then

                CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)
                CheckBoxSelect.AutoPostBack = True

                TextBoxUpdateDate = DirectCast(CurrentRow.FindControl("TextBoxUpdateDate"), TextBox)
                TextBoxUpdateDate.ReadOnly = True

                TextBoxUpdateBy = DirectCast(CurrentRow.FindControl("TextBoxUpdateBy"), TextBox)
                TextBoxUpdateBy.ReadOnly = True

            End If

        End Sub

        Private Sub GridViewMEDsysStaffs_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
            GridViewMEDsysStaffs.PageIndex = e.NewPageIndex
            BindGridViewMEDsysStaffs()
        End Sub

        ''' <summary>
        ''' Gridview Row Hover Color Set
        ''' </summary>
        ''' <param name="writer"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub Render(writer As HtmlTextWriter)
            For Each r As GridViewRow In GridViewMEDsysStaffs.Rows
                If r.RowType = DataControlRowType.DataRow Then
                    objShared.SetGridViewRowColor(r)
                End If
            Next
            MyBase.Render(writer)
        End Sub

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("EVV", ControlName & Convert.ToString(".resx"))

            LabelMEDsysStaffs.Text = Convert.ToString(ResourceTable("LabelMEDsysStaffs"), Nothing)
            LabelMEDsysStaffs.Text = If(String.IsNullOrEmpty(LabelMEDsysStaffs.Text), "MEDsys Staffs", LabelMEDsysStaffs.Text)

            ButtonSave.Text = Convert.ToString(ResourceTable("ButtonSave"), Nothing)
            ButtonSave.Text = If(String.IsNullOrEmpty(ButtonSave.Text), "Save", ButtonSave.Text)

            ButtonClear.Text = Convert.ToString(ResourceTable("ButtonClear"), Nothing)
            ButtonClear.Text = If(String.IsNullOrEmpty(ButtonClear.Text), "Clear", ButtonClear.Text)

            ButtonDelete.Text = Convert.ToString(ResourceTable("ButtonDelete"), Nothing)
            ButtonDelete.Text = If(String.IsNullOrEmpty(ButtonDelete.Text), "Delete", ButtonDelete.Text)

            ButtonEmployeeDetail.Text = Convert.ToString(ResourceTable("ButtonEmployeeDetail"), Nothing)
            ButtonEmployeeDetail.Text = If(String.IsNullOrEmpty(ButtonEmployeeDetail.Text), "Employee Detail", ButtonEmployeeDetail.Text)

            ButtonResetPositionCode.Text = Convert.ToString(ResourceTable("ButtonResetPositionCode"), Nothing)
            ButtonResetPositionCode.Text = If(String.IsNullOrEmpty(ButtonResetPositionCode.Text), "Reset Position Code", ButtonResetPositionCode.Text)

            ButtonViewError.Text = Convert.ToString(ResourceTable("ButtonViewError"), Nothing)
            ButtonViewError.Text = If(String.IsNullOrEmpty(ButtonViewError.Text), "View Detail Error", ButtonViewError.Text)

            SaveMessage = Convert.ToString(ResourceTable("SaveMessage"), Nothing)
            SaveMessage = If(String.IsNullOrEmpty(SaveMessage), "Saved Successfully", SaveMessage)

            DeleteMessage = Convert.ToString(ResourceTable("DeleteMessage"), Nothing)
            DeleteMessage = If(String.IsNullOrEmpty(DeleteMessage), "Deleted Successfully", DeleteMessage)

            ValidationGroup = Convert.ToString(ResourceTable("ValidationGroup"), Nothing)
            ValidationGroup = If(String.IsNullOrEmpty(ValidationGroup), "MEDsysStaffs", ValidationGroup)

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

            LabelHeaderAccountId = DirectCast(CurrentRow.FindControl("LabelHeaderAccountId"), Label)
            LabelHeaderAccountId.Text = Convert.ToString(ResourceTable("LabelHeaderAccountId"), Nothing).Trim()
            LabelHeaderAccountId.Text = If(String.IsNullOrEmpty(LabelHeaderAccountId.Text), "Account ID", LabelHeaderAccountId.Text)

            LabelHeaderExternalId = DirectCast(CurrentRow.FindControl("LabelHeaderExternalId"), Label)
            LabelHeaderExternalId.Text = Convert.ToString(ResourceTable("LabelHeaderExternalId"), Nothing).Trim()
            LabelHeaderExternalId.Text = If(String.IsNullOrEmpty(LabelHeaderExternalId.Text), "External ID", LabelHeaderExternalId.Text)

            LabelHeaderStaffId = DirectCast(CurrentRow.FindControl("LabelHeaderStaffId"), Label)
            LabelHeaderStaffId.Text = Convert.ToString(ResourceTable("LabelHeaderStaffId"), Nothing).Trim()
            LabelHeaderStaffId.Text = If(String.IsNullOrEmpty(LabelHeaderStaffId.Text), "Staff ID", LabelHeaderStaffId.Text)

            LabelHeaderStaffNumber = DirectCast(CurrentRow.FindControl("LabelHeaderStaffNumber"), Label)
            LabelHeaderStaffNumber.Text = Convert.ToString(ResourceTable("LabelHeaderStaffNumber"), Nothing).Trim()
            LabelHeaderStaffNumber.Text = If(String.IsNullOrEmpty(LabelHeaderStaffNumber.Text), "Staff Number", LabelHeaderStaffNumber.Text)

            LabelHeaderLastName = DirectCast(CurrentRow.FindControl("LabelHeaderLastName"), Label)
            LabelHeaderLastName.Text = Convert.ToString(ResourceTable("LabelHeaderLastName"), Nothing).Trim()
            LabelHeaderLastName.Text = If(String.IsNullOrEmpty(LabelHeaderLastName.Text), "Last Name", LabelHeaderLastName.Text)

            LabelHeaderFirstName = DirectCast(CurrentRow.FindControl("LabelHeaderFirstName"), Label)
            LabelHeaderFirstName.Text = Convert.ToString(ResourceTable("LabelHeaderFirstName"), Nothing).Trim()
            LabelHeaderFirstName.Text = If(String.IsNullOrEmpty(LabelHeaderFirstName.Text), "First Name", LabelHeaderFirstName.Text)

            LabelHeaderMiddleInitial = DirectCast(CurrentRow.FindControl("LabelHeaderMiddleInitial"), Label)
            LabelHeaderMiddleInitial.Text = Convert.ToString(ResourceTable("LabelHeaderMiddleInitial"), Nothing).Trim()
            LabelHeaderMiddleInitial.Text = If(String.IsNullOrEmpty(LabelHeaderMiddleInitial.Text), "MI", LabelHeaderMiddleInitial.Text)

            LabelHeaderDateOfBirth = DirectCast(CurrentRow.FindControl("LabelHeaderDateOfBirth"), Label)
            LabelHeaderDateOfBirth.Text = Convert.ToString(ResourceTable("LabelHeaderDateOfBirth"), Nothing).Trim()
            LabelHeaderDateOfBirth.Text = If(String.IsNullOrEmpty(LabelHeaderDateOfBirth.Text), "Birthdate", LabelHeaderDateOfBirth.Text)

            LabelHeaderGender = DirectCast(CurrentRow.FindControl("LabelHeaderGender"), Label)
            LabelHeaderGender.Text = Convert.ToString(ResourceTable("LabelHeaderGender"), Nothing).Trim()
            LabelHeaderGender.Text = If(String.IsNullOrEmpty(LabelHeaderGender.Text), "Gender", LabelHeaderGender.Text)

            LabelHeaderGIN = DirectCast(CurrentRow.FindControl("LabelHeaderGIN"), Label)
            LabelHeaderGIN.Text = Convert.ToString(ResourceTable("LabelHeaderGIN"), Nothing).Trim()
            LabelHeaderGIN.Text = If(String.IsNullOrEmpty(LabelHeaderGIN.Text), "GIN", LabelHeaderGIN.Text)

            LabelHeaderAddress = DirectCast(CurrentRow.FindControl("LabelHeaderAddress"), Label)
            LabelHeaderAddress.Text = Convert.ToString(ResourceTable("LabelHeaderAddress"), Nothing).Trim()
            LabelHeaderAddress.Text = If(String.IsNullOrEmpty(LabelHeaderAddress.Text), "Address", LabelHeaderAddress.Text)

            LabelHeaderCity = DirectCast(CurrentRow.FindControl("LabelHeaderCity"), Label)
            LabelHeaderCity.Text = Convert.ToString(ResourceTable("LabelHeaderCity"), Nothing).Trim()
            LabelHeaderCity.Text = If(String.IsNullOrEmpty(LabelHeaderCity.Text), "City", LabelHeaderCity.Text)

            LabelHeaderState = DirectCast(CurrentRow.FindControl("LabelHeaderState"), Label)
            LabelHeaderState.Text = Convert.ToString(ResourceTable("LabelHeaderState"), Nothing).Trim()
            LabelHeaderState.Text = If(String.IsNullOrEmpty(LabelHeaderState.Text), "State", LabelHeaderState.Text)

            LabelHeaderZip = DirectCast(CurrentRow.FindControl("LabelHeaderZip"), Label)
            LabelHeaderZip.Text = Convert.ToString(ResourceTable("LabelHeaderZip"), Nothing).Trim()
            LabelHeaderZip.Text = If(String.IsNullOrEmpty(LabelHeaderZip.Text), "Zip", LabelHeaderZip.Text)

            LabelHeaderPhone = DirectCast(CurrentRow.FindControl("LabelHeaderPhone"), Label)
            LabelHeaderPhone.Text = Convert.ToString(ResourceTable("LabelHeaderPhone"), Nothing).Trim()
            LabelHeaderPhone.Text = If(String.IsNullOrEmpty(LabelHeaderPhone.Text), "Phone", LabelHeaderPhone.Text)

            LabelHeaderNotes = DirectCast(CurrentRow.FindControl("LabelHeaderNotes"), Label)
            LabelHeaderNotes.Text = Convert.ToString(ResourceTable("LabelHeaderNotes"), Nothing).Trim()
            LabelHeaderNotes.Text = If(String.IsNullOrEmpty(LabelHeaderNotes.Text), "Notes", LabelHeaderNotes.Text)

            LabelHeaderPositionCode = DirectCast(CurrentRow.FindControl("LabelHeaderPositionCode"), Label)
            LabelHeaderPositionCode.Text = Convert.ToString(ResourceTable("LabelHeaderPositionCode"), Nothing).Trim()
            LabelHeaderPositionCode.Text = If(String.IsNullOrEmpty(LabelHeaderPositionCode.Text), "Position Code", LabelHeaderPositionCode.Text)

            LabelHeaderTeamCode = DirectCast(CurrentRow.FindControl("LabelHeaderTeamCode"), Label)
            LabelHeaderTeamCode.Text = Convert.ToString(ResourceTable("LabelHeaderTeamCode"), Nothing).Trim()
            LabelHeaderTeamCode.Text = If(String.IsNullOrEmpty(LabelHeaderTeamCode.Text), "Team Code", LabelHeaderTeamCode.Text)

            LabelHeaderStatus = DirectCast(CurrentRow.FindControl("LabelHeaderStatus"), Label)
            LabelHeaderStatus.Text = Convert.ToString(ResourceTable("LabelHeaderStatus"), Nothing).Trim()
            LabelHeaderStatus.Text = If(String.IsNullOrEmpty(LabelHeaderStatus.Text), "Status", LabelHeaderStatus.Text)

            LabelHeaderStatusDate = DirectCast(CurrentRow.FindControl("LabelHeaderStatusDate"), Label)
            LabelHeaderStatusDate.Text = Convert.ToString(ResourceTable("LabelHeaderStatusDate"), Nothing).Trim()
            LabelHeaderStatusDate.Text = If(String.IsNullOrEmpty(LabelHeaderStatusDate.Text), "Status Date", LabelHeaderStatusDate.Text)

            LabelHeaderStartDate = DirectCast(CurrentRow.FindControl("LabelHeaderStartDate"), Label)
            LabelHeaderStartDate.Text = Convert.ToString(ResourceTable("LabelHeaderStartDate"), Nothing).Trim()
            LabelHeaderStartDate.Text = If(String.IsNullOrEmpty(LabelHeaderStartDate.Text), "Start Date", LabelHeaderStartDate.Text)

            LabelHeaderEndDate = DirectCast(CurrentRow.FindControl("LabelHeaderEndDate"), Label)
            LabelHeaderEndDate.Text = Convert.ToString(ResourceTable("LabelHeaderEndDate"), Nothing).Trim()
            LabelHeaderEndDate.Text = If(String.IsNullOrEmpty(LabelHeaderEndDate.Text), "End Date", LabelHeaderEndDate.Text)

            LabelHeaderAction = DirectCast(CurrentRow.FindControl("LabelHeaderAction"), Label)
            LabelHeaderAction.Text = Convert.ToString(ResourceTable("LabelHeaderAction"), Nothing).Trim()
            LabelHeaderAction.Text = If(String.IsNullOrEmpty(LabelHeaderAction.Text), "Action", LabelHeaderAction.Text)

            LabelHeaderEmployeeId = DirectCast(CurrentRow.FindControl("LabelHeaderEmployeeId"), Label)
            LabelHeaderEmployeeId.Text = Convert.ToString(ResourceTable("LabelHeaderEmployeeId"), Nothing).Trim()
            LabelHeaderEmployeeId.Text = If(String.IsNullOrEmpty(LabelHeaderEmployeeId.Text), "Employee Id", LabelHeaderEmployeeId.Text)

            LabelHeaderCompanyCode = DirectCast(CurrentRow.FindControl("LabelHeaderCompanyCode"), Label)
            LabelHeaderCompanyCode.Text = Convert.ToString(ResourceTable("LabelHeaderCompanyCode"), Nothing).Trim()
            LabelHeaderCompanyCode.Text = If(String.IsNullOrEmpty(LabelHeaderCompanyCode.Text), "Company Code", LabelHeaderCompanyCode.Text)

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
            AddHandler ButtonEmployeeDetail.Click, AddressOf ButtonEmployeeDetail_Click
            AddHandler ButtonResetPositionCode.Click, AddressOf ButtonResetPositionCode_Click

            GridViewMEDsysStaffs.AutoGenerateColumns = False
            GridViewMEDsysStaffs.ShowHeaderWhenEmpty = True
            GridViewMEDsysStaffs.AllowPaging = True
            GridViewMEDsysStaffs.AllowSorting = True

            GridViewMEDsysStaffs.ShowFooter = True

            If (GridViewMEDsysStaffs.AllowPaging) Then
                GridViewMEDsysStaffs.PageSize = objShared.GridViewDefaultPageSize
            End If

            AddHandler GridViewMEDsysStaffs.RowDataBound, AddressOf GridViewMEDsysStaffs_RowDataBound
            AddHandler GridViewMEDsysStaffs.PageIndexChanging, AddressOf GridViewMEDsysStaffs_PageIndexChanging

            ButtonClear.ClientIDMode = UI.ClientIDMode.Static
            ButtonSave.ClientIDMode = UI.ClientIDMode.Static
            ButtonDelete.ClientIDMode = UI.ClientIDMode.Static
            ButtonEmployeeDetail.ClientIDMode = UI.ClientIDMode.Static
            ButtonResetPositionCode.ClientIDMode = UI.ClientIDMode.Static

        End Sub

        ''' <summary>
        ''' This would generate excel file containing error occured during database operation
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

        Private Sub LoadJScript()

            LoadJS("JavaScript/jquery.blockUI.js")

            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                          & " var LoaderImagePath ='" & objShared.GetLoaderImagePath & "'; " _
                          & " var DeleteTargetButton ='ButtonDelete'; " _
                          & " var DeleteDialogHeader ='MEDsys Staffs'; " _
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
        ''' Clearing controls value
        ''' </summary>
        Private Sub ClearControl(CurrentRow As GridViewRow)
            ControlsFind(CurrentRow)
            BindGridViewMEDsysStaffs()
            TextBoxResetPositionCode.Text = String.Empty
        End Sub

        Private Sub GetData()
            GetStaffData()
        End Sub

        Private Sub GetStaffData()

            Dim objBLMEDsysStaff As New BLMEDsysStaff()

            Try
                Staffs = objBLMEDsysStaff.SelectMEDsysStaffInfo(objShared.ConVisitel)
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to fetch MEDsys Staff Data. Message: {0}", ex.Message))
            Finally
                objBLMEDsysStaff = Nothing
            End Try
        End Sub

        Private Sub BindGridViewMEDsysStaffs()
            GridViewMEDsysStaffs.DataSource = Staffs
            GridViewMEDsysStaffs.DataBind()

            ItemChecked = DetermineButtonInActivity(GridViewMEDsysStaffs, "CheckBoxSelect")

            ButtonSave.Enabled = ItemChecked
            ButtonDelete.Enabled = ButtonSave.Enabled
            ButtonEmployeeDetail.Enabled = ButtonSave.Enabled
        End Sub

    End Class
End Namespace