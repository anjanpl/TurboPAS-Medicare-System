Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports System.Data.SqlClient
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelCommon
Imports VisitelBusiness.VisitelBusiness.DataObject.EVV

Namespace Visitel.UserControl.EVV
    Public Class EmployeesControl
        Inherits CommonDataControl

        Private ContractNumber As String
        Private objShared As SharedWebControls

        Private CheckBoxSelect As CheckBox, chkAll As CheckBox
        Private ItemChecked As Boolean = False

        Private LabelHeaderSerial As Label, LabelHeaderSelect As Label, LabelHeaderMyUniqueId As Label, LabelHeaderEVVId As Label, LabelHeaderEmployeeNumberVesta As Label, _
            LabelHeaderEmployeeLastName As Label, LabelHeaderEmployeeFirstName As Label, LabelHeaderEmployeePhone As Label, LabelHeaderEmployeeSSNumber As Label, _
            LabelHeaderEmployeePassport As Label, LabelHeaderEmployeeStartDate As Label, LabelHeaderEmployeeEndDate As Label, LabelHeaderEmployeeDiscipline As Label, _
            LabelHeaderBranchName As Label, LabelHeaderError As Label, LabelHeaderEmployeeId As Label, LabelHeaderUpdateDate As Label, LabelHeaderUpdateBy As Label,
            LabelId As Label, LabelEmployeeId As Label

        Private TextBoxMyUniqueId As TextBox, TextBoxEVVId As TextBox, TextBoxEmployeeNumberVesta As TextBox, TextBoxEmployeeLastName As TextBox, _
            TextBoxEmployeeFirstName As TextBox, TextBoxEmployeePhone As TextBox, TextBoxEmployeeSSNumber As TextBox, TextBoxEmployeePassport As TextBox, _
            TextBoxEmployeeStartDate As TextBox, TextBoxEmployeeEndDate As TextBox, TextBoxEmployeeDiscipline As TextBox, TextBoxBranchName As TextBox, _
            TextBoxError As TextBox, TextBoxEmployeeId As TextBox, TextBoxUpdateDate As TextBox, TextBoxUpdateBy As TextBox

        Private ControlName As String, ValidationGroup As String, SaveMessage As String, DeleteMessage As String, DeleteConfirmationMessage As String, ListFor As String
        Private ValidationEnable As Boolean
        Private CurrentRow As GridViewRow

        Private Employees As List(Of EmployeeDataObject), EmployeesErrorList As List(Of EmployeeDataObject)

        Private SaveFailedCounter As Int16, DeleteFailedCounter As Int16, RecordSaveCount As Int16


        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "EmployeesControl"
            objShared = New SharedWebControls()
            objShared.ConnectionOpen()
            GetCaptionFromResource()
            InitializeControl()
        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)

            GetData()

            If Not IsPostBack Then
                BindGridViewEmployees()
            End If

        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadCss("EVV/" & ControlName)
            LoadJScript()
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing
            Employees = Nothing
        End Sub


        Private Sub ButtonSave_Click(sender As Object, e As EventArgs)

            Page.Validate()
            Page.Validate(ValidationGroup)

            Employees = New List(Of EmployeeDataObject)()
            EmployeesErrorList = New List(Of EmployeeDataObject)()
            SaveFailedCounter = 0
            RecordSaveCount = 0

            CurrentRow = GridViewEmployees.FooterRow

            CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)

            LabelEmployeeId = DirectCast(CurrentRow.FindControl("LabelEmployeeId"), Label)

            If ((CheckBoxSelect.Checked) And (String.IsNullOrEmpty(LabelEmployeeId.Text.Trim()))) Then
                SaveData(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT))
            End If

            SaveData(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE))

            If (SaveFailedCounter > 0) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} Records Failed to Save out of {1}", SaveFailedCounter, Employees.Count))
                ViewState("SaveFailedRecord") = objShared.ToDataTable(EmployeesErrorList)
                ViewState("ListFor") = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)
            Else
                ViewState("SaveFailedRecord") = Nothing

                If (RecordSaveCount > 0) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} record(s) {1}.", RecordSaveCount, SaveMessage))
                    ViewState.Clear()
                    GetData()
                    BindGridViewEmployees()
                End If

            End If

            ButtonViewError.Visible = If((SaveFailedCounter > 0), True, False)

            Employees = Nothing
            EmployeesErrorList = Nothing

        End Sub

        Private Sub ButtonEmployeeDetail_Click(sender As Object, e As EventArgs)
            Response.Redirect("~/Pages/EmployeeInfo.aspx?EmployeeId=" & HiddenFieldEmployeeId.Value)
        End Sub


        Private Sub ButtonDelete_Click(sender As Object, e As EventArgs)
            Return
            Employees = New List(Of EmployeeDataObject)()
            EmployeesErrorList = New List(Of EmployeeDataObject)()

            MakeList(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE))

            If (Employees.Count.Equals(0)) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please select items to delete")
                Return
            End If

            DeleteFailedCounter = 0

            TakeAction(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE))

            If (DeleteFailedCounter > 0) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} Records Failed to Delete out of {1}", DeleteFailedCounter, Employees.Count))
                ViewState("DeleteFailedRecord") = objShared.ToDataTable(EmployeesErrorList)
                ViewState("ListFor") = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE)
            Else
                ViewState("DeleteFailedRecord") = Nothing

                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} record(s) {1}.", Employees.Count, DeleteMessage))
                ViewState.Clear()
                GetData()
                BindGridViewEmployees()
            End If

            ButtonViewError.Visible = If((DeleteFailedCounter > 0), True, False)

            Employees = Nothing
            EmployeesErrorList = Nothing
        End Sub

        Private Sub ButtonClear_Click(sender As Object, e As EventArgs)
            CurrentRow = GridViewEmployees.FooterRow
            ClearControl(CurrentRow)
        End Sub

        Protected Sub OnCheckedChanged(sender As Object, e As EventArgs)
            Dim isUpdateVisible As Boolean = False

            Dim chk As CheckBox = TryCast(sender, CheckBox)

            'ButtonSave.Enabled = If((chk.Checked), True, False)

            If chk.ID = "chkAll" Then
                For Each row As GridViewRow In GridViewEmployees.Rows
                    If row.RowType = DataControlRowType.DataRow Then
                        row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked = chk.Checked
                    End If
                Next

                CurrentRow = GridViewEmployees.FooterRow
                CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)
                CheckBoxSelect.Checked = chk.Checked

            End If

            Dim chkAll As CheckBox = TryCast(GridViewEmployees.HeaderRow.FindControl("chkAll"), CheckBox)
            'chkAll.Checked = True
            For Each row As GridViewRow In GridViewEmployees.Rows
                If row.RowType = DataControlRowType.DataRow Then

                    Dim isChecked As Boolean = row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked

                    ControlsOnSelect(row, isChecked)

                    If (isChecked) Then
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

            ItemChecked = DetermineButtonInActivity(GridViewEmployees, "CheckBoxSelect")

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

            If (Employees.Count.Equals(0)) Then
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

            Dim objEmployeeDataObject As EmployeeDataObject

            Select Case Action
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT)

                    'DropDownListEmployee = DirectCast(GridViewVisits.FooterRow.FindControl("DropDownListEmployee"), DropDownList)
                    'If (DropDownListEmployee.SelectedIndex < 1) Then
                    '    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please select one employee")
                    '    Return
                    'End If

                    objEmployeeDataObject = New EmployeeDataObject()
                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)

                    AddToList(CurrentRow, Employees, objEmployeeDataObject)
                    RecordSaveCount = RecordSaveCount + 1
                    objEmployeeDataObject = Nothing

                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE),
                    EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE)
                    Employees.Clear()
                    For Each row As GridViewRow In GridViewEmployees.Rows
                        If (row.RowType.Equals(DataControlRowType.DataRow)) Then
                            Dim isChecked As Boolean = row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked

                            If (isChecked) Then

                                objEmployeeDataObject = New EmployeeDataObject()

                                CurrentRow = DirectCast(GridViewEmployees.Rows(row.RowIndex), GridViewRow)

                                'DropDownListEmployee = DirectCast(CurrentRow.FindControl("DropDownListEmployee"), DropDownList)
                                'If (DropDownListEmployee.SelectedIndex < 1) Then
                                '    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please select one employee")
                                '    Return
                                'End If

                                If (Action.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE))) Then
                                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)
                                End If

                                If (Action.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE))) Then
                                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE)
                                End If

                                AddToList(CurrentRow, Employees, objEmployeeDataObject)
                                RecordSaveCount = RecordSaveCount + 1
                                objEmployeeDataObject = Nothing

                            End If

                        End If
                    Next

                    Exit Select
            End Select

            objEmployeeDataObject = Nothing

        End Sub

        ''' <summary>
        ''' Making List in order to save records or delete
        ''' </summary>
        ''' <param name="CurrentRow"></param>
        ''' <param name="Employees"></param>
        ''' <param name="objEmployeeDataObject"></param>
        ''' <remarks></remarks>
        Private Sub AddToList(ByRef CurrentRow As GridViewRow, ByRef Employees As List(Of EmployeeDataObject), ByRef objEmployeeDataObject As EmployeeDataObject)

            Dim Int32Result As Int32

            LabelId = DirectCast(CurrentRow.FindControl("LabelId"), Label)
            LabelEmployeeId = DirectCast(CurrentRow.FindControl("LabelEmployeeId"), Label)

            If (ListFor.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE))) Then
                ControlsFind(CurrentRow)
            End If

            If (Page.IsValid) Then

                objEmployeeDataObject.Id = If(Int32.TryParse(LabelId.Text.Trim(), Int32Result), Int32Result, objEmployeeDataObject.Id)

                If (ListFor.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE))) Then

                    TextBoxEmployeeId = DirectCast(CurrentRow.FindControl("TextBoxEmployeeId"), TextBox)

                    objEmployeeDataObject.MyUniqueId = TextBoxMyUniqueId.Text.Trim()
                    objEmployeeDataObject.EVVId = TextBoxEVVId.Text.Trim()
                    objEmployeeDataObject.EmployeeNumberVesta = TextBoxEmployeeNumberVesta.Text.Trim()
                    objEmployeeDataObject.EmployeeLastName = TextBoxEmployeeLastName.Text.Trim()
                    objEmployeeDataObject.EmployeeFirstName = TextBoxEmployeeFirstName.Text.Trim()
                    objEmployeeDataObject.EmployeePhone = TextBoxEmployeePhone.Text.Trim()
                    objEmployeeDataObject.EmployeeSSNumber = TextBoxEmployeeSSNumber.Text.Trim()
                    objEmployeeDataObject.EmployeePassport = TextBoxEmployeePassport.Text.Trim()
                    objEmployeeDataObject.EmployeeStartDate = TextBoxEmployeeStartDate.Text.Trim()
                    objEmployeeDataObject.EmployeeEndDate = TextBoxEmployeeEndDate.Text.Trim()
                    objEmployeeDataObject.EmployeeDiscipline = TextBoxEmployeeDiscipline.Text.Trim()
                    objEmployeeDataObject.BranchName = TextBoxBranchName.Text.Trim()
                    objEmployeeDataObject.Error = TextBoxError.Text.Trim()
                    objEmployeeDataObject.EmployeeId = TextBoxEmployeeId.Text.Trim()

                End If

                objEmployeeDataObject.UpdateBy = objShared.UserId

                Employees.Add(objEmployeeDataObject)

            End If

        End Sub

        ''' <summary>
        ''' Make Database operation either for record saving or deleting
        ''' </summary>
        ''' <param name="Action"></param>
        ''' <remarks></remarks>
        Private Sub TakeAction(Action As String)

            Dim objBLVestaEmployee As New BLVestaEmployee()

            For Each Employee In Employees
                Try
                    Select Case Action
                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT)
                            objBLVestaEmployee.InsertEmployeeInfo(objShared.ConVisitel, Employee)
                            Exit Select

                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE)
                            objBLVestaEmployee.UpdateEmployeeInfo(objShared.ConVisitel, Employee)
                            Exit Select

                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE)
                            objBLVestaEmployee.DeleteEmployeeInfo(objShared.ConVisitel, Employee.Id, Employee.UpdateBy)
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

                    Employee.Remarks = ex.Message
                    EmployeesErrorList.Add(Employee)

                Finally

                End Try
            Next

            objBLVestaEmployee = Nothing

        End Sub

        Private Sub ControlsFind(ByRef CurrentRow As GridViewRow)
            TextBoxMyUniqueId = DirectCast(CurrentRow.FindControl("TextBoxMyUniqueId"), TextBox)
            TextBoxEVVId = DirectCast(CurrentRow.FindControl("TextBoxEVVId"), TextBox)
            TextBoxEmployeeNumberVesta = DirectCast(CurrentRow.FindControl("TextBoxEmployeeNumberVesta"), TextBox)
            TextBoxEmployeeLastName = DirectCast(CurrentRow.FindControl("TextBoxEmployeeLastName"), TextBox)
            TextBoxEmployeeFirstName = DirectCast(CurrentRow.FindControl("TextBoxEmployeeFirstName"), TextBox)
            TextBoxEmployeePhone = DirectCast(CurrentRow.FindControl("TextBoxEmployeePhone"), TextBox)
            TextBoxEmployeeSSNumber = DirectCast(CurrentRow.FindControl("TextBoxEmployeeSSNumber"), TextBox)
            TextBoxEmployeePassport = DirectCast(CurrentRow.FindControl("TextBoxEmployeePassport"), TextBox)
            TextBoxEmployeeStartDate = DirectCast(CurrentRow.FindControl("TextBoxEmployeeStartDate"), TextBox)
            TextBoxEmployeeEndDate = DirectCast(CurrentRow.FindControl("TextBoxEmployeeEndDate"), TextBox)
            TextBoxEmployeeDiscipline = DirectCast(CurrentRow.FindControl("TextBoxEmployeeDiscipline"), TextBox)
            TextBoxBranchName = DirectCast(CurrentRow.FindControl("TextBoxBranchName"), TextBox)
            TextBoxError = DirectCast(CurrentRow.FindControl("TextBoxError"), TextBox)
            TextBoxEmployeeId = DirectCast(CurrentRow.FindControl("TextBoxEmployeeId"), TextBox)
        End Sub

        ''' <summary>
        ''' Gridview row selection and go for edit mode or record deletion
        ''' </summary>
        ''' <param name="CurrentRow"></param>
        ''' <param name="isChecked"></param>
        ''' <remarks></remarks>
        Private Sub ControlsOnSelect(ByRef CurrentRow As GridViewRow, ByRef isChecked As Boolean)

            ControlsFind(CurrentRow)

            If (Not TextBoxMyUniqueId Is Nothing) Then
                TextBoxMyUniqueId.ReadOnly = Not isChecked
                TextBoxMyUniqueId.CssClass = If((TextBoxMyUniqueId.ReadOnly), "TextBoxMyUniqueId", "TextBoxMyUniqueIdEdit")
            End If

            If (Not TextBoxEVVId Is Nothing) Then
                TextBoxEVVId.ReadOnly = Not isChecked
                TextBoxEVVId.CssClass = If((TextBoxEVVId.ReadOnly), "TextBoxEVVId", "TextBoxEVVIdEdit")
            End If

            If (Not TextBoxEmployeeNumberVesta Is Nothing) Then
                TextBoxEmployeeNumberVesta.ReadOnly = Not isChecked
                TextBoxEmployeeNumberVesta.CssClass = If((TextBoxEmployeeNumberVesta.ReadOnly), "TextBoxEmployeeNumberVesta", "TextBoxEmployeeNumberVestaEdit")
            End If

            If (Not TextBoxEmployeeLastName Is Nothing) Then
                TextBoxEmployeeLastName.ReadOnly = Not isChecked
                TextBoxEmployeeLastName.CssClass = If((TextBoxEmployeeLastName.ReadOnly), "TextBoxEmployeeLastName", "TextBoxEmployeeLastNameEdit")
            End If

            If (Not TextBoxEmployeeFirstName Is Nothing) Then
                TextBoxEmployeeFirstName.ReadOnly = Not isChecked
                TextBoxEmployeeFirstName.CssClass = If((TextBoxEmployeeFirstName.ReadOnly), "TextBoxEmployeeFirstName", "TextBoxEmployeeFirstNameEdit")
            End If

            If (Not TextBoxEmployeePhone Is Nothing) Then
                TextBoxEmployeePhone.ReadOnly = Not isChecked
                TextBoxEmployeePhone.CssClass = If((TextBoxEmployeePhone.ReadOnly), "TextBoxEmployeePhone", "TextBoxEmployeePhoneEdit")
            End If

            If (Not TextBoxEmployeeSSNumber Is Nothing) Then
                TextBoxEmployeeSSNumber.ReadOnly = Not isChecked
                TextBoxEmployeeSSNumber.CssClass = If((TextBoxEmployeeSSNumber.ReadOnly), "TextBoxEmployeeSSNumber", "TextBoxEmployeeSSNumberEdit")
            End If

            If (Not TextBoxEmployeePassport Is Nothing) Then
                TextBoxEmployeePassport.ReadOnly = Not isChecked
                TextBoxEmployeePassport.CssClass = If((TextBoxEmployeePassport.ReadOnly), "TextBoxEmployeePassport", "TextBoxEmployeePassportEdit")
            End If

            If (Not TextBoxEmployeeStartDate Is Nothing) Then
                TextBoxEmployeeStartDate.ReadOnly = Not isChecked
                TextBoxEmployeeStartDate.CssClass = If((TextBoxEmployeeStartDate.ReadOnly), "TextBoxEmployeeStartDate", "TextBoxEmployeeStartDateEdit")
            End If

            If (Not TextBoxEmployeeEndDate Is Nothing) Then
                TextBoxEmployeeEndDate.ReadOnly = Not isChecked
                TextBoxEmployeeEndDate.CssClass = If((TextBoxEmployeeEndDate.ReadOnly), "TextBoxEmployeeEndDate", "TextBoxEmployeeEndDateEdit")
            End If

            If (Not TextBoxEmployeeDiscipline Is Nothing) Then
                TextBoxEmployeeDiscipline.ReadOnly = Not isChecked
                TextBoxEmployeeDiscipline.CssClass = If((TextBoxEmployeeDiscipline.ReadOnly), "TextBoxEmployeeDiscipline", "TextBoxEmployeeDisciplineEdit")
            End If

            If (Not TextBoxBranchName Is Nothing) Then
                TextBoxBranchName.ReadOnly = Not isChecked
                TextBoxBranchName.CssClass = If((TextBoxBranchName.ReadOnly), "TextBoxBranchName", "TextBoxBranchNameEdit")
            End If

            If (Not TextBoxError Is Nothing) Then
                TextBoxError.ReadOnly = Not isChecked
                TextBoxError.CssClass = If((TextBoxError.ReadOnly), "TextBoxError", "TextBoxErrorEdit")
            End If

            If (Not TextBoxEmployeeId Is Nothing) Then
                TextBoxEmployeeId.ReadOnly = Not isChecked
                TextBoxEmployeeId.CssClass = If((TextBoxEmployeeId.ReadOnly), "TextBoxEmployeeId", "TextBoxEmployeeIdEdit")
            End If

        End Sub

        Private Sub GridViewEmployees_RowDataBound(sender As Object, e As GridViewRowEventArgs)
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

                TextBoxMyUniqueId = DirectCast(CurrentRow.FindControl("TextBoxMyUniqueId"), TextBox)
                TextBoxMyUniqueId.ReadOnly = True

                TextBoxEVVId = DirectCast(CurrentRow.FindControl("TextBoxEVVId"), TextBox)
                TextBoxEVVId.ReadOnly = True

                TextBoxEmployeeNumberVesta = DirectCast(CurrentRow.FindControl("TextBoxEmployeeNumberVesta"), TextBox)
                TextBoxEmployeeNumberVesta.ReadOnly = True

                TextBoxEmployeeLastName = DirectCast(CurrentRow.FindControl("TextBoxEmployeeLastName"), TextBox)
                TextBoxEmployeeLastName.ReadOnly = True

                TextBoxEmployeeFirstName = DirectCast(CurrentRow.FindControl("TextBoxEmployeeFirstName"), TextBox)
                TextBoxEmployeeFirstName.ReadOnly = True

                TextBoxEmployeePhone = DirectCast(CurrentRow.FindControl("TextBoxEmployeePhone"), TextBox)
                TextBoxEmployeePhone.ReadOnly = True

                TextBoxEmployeeSSNumber = DirectCast(CurrentRow.FindControl("TextBoxEmployeeSSNumber"), TextBox)
                TextBoxEmployeeSSNumber.ReadOnly = True

                TextBoxEmployeePassport = DirectCast(CurrentRow.FindControl("TextBoxEmployeePassport"), TextBox)
                TextBoxEmployeePassport.ReadOnly = True

                TextBoxEmployeeStartDate = DirectCast(CurrentRow.FindControl("TextBoxEmployeeStartDate"), TextBox)
                TextBoxEmployeeStartDate.ReadOnly = True

                TextBoxEmployeeEndDate = DirectCast(CurrentRow.FindControl("TextBoxEmployeeEndDate"), TextBox)
                TextBoxEmployeeEndDate.ReadOnly = True

                TextBoxEmployeeDiscipline = DirectCast(CurrentRow.FindControl("TextBoxEmployeeDiscipline"), TextBox)
                TextBoxEmployeeDiscipline.ReadOnly = True

                TextBoxBranchName = DirectCast(CurrentRow.FindControl("TextBoxBranchName"), TextBox)
                TextBoxBranchName.ReadOnly = True

                TextBoxError = DirectCast(CurrentRow.FindControl("TextBoxError"), TextBox)
                TextBoxError.ReadOnly = True

                TextBoxEmployeeId = DirectCast(CurrentRow.FindControl("TextBoxEmployeeId"), TextBox)
                TextBoxEmployeeId.ReadOnly = True

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

        Private Sub GridViewEmployees_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
            GridViewEmployees.PageIndex = e.NewPageIndex
            BindGridViewEmployees()
        End Sub

        ''' <summary>
        ''' Gridview Row Hover Color Set
        ''' </summary>
        ''' <param name="writer"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub Render(writer As HtmlTextWriter)
            For Each r As GridViewRow In GridViewEmployees.Rows
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

            LabelEmployees.Text = Convert.ToString(ResourceTable("LabelEmployees"), Nothing)
            LabelEmployees.Text = If(String.IsNullOrEmpty(LabelEmployees.Text), "Vesta Employees", LabelEmployees.Text)

            ButtonSave.Text = Convert.ToString(ResourceTable("ButtonSave"), Nothing)
            ButtonSave.Text = If(String.IsNullOrEmpty(ButtonSave.Text), "Save", ButtonSave.Text)

            ButtonClear.Text = Convert.ToString(ResourceTable("ButtonClear"), Nothing)
            ButtonClear.Text = If(String.IsNullOrEmpty(ButtonClear.Text), "Clear", ButtonClear.Text)

            ButtonDelete.Text = Convert.ToString(ResourceTable("ButtonDelete"), Nothing)
            ButtonDelete.Text = If(String.IsNullOrEmpty(ButtonDelete.Text), "Delete", ButtonDelete.Text)

            ButtonEmployeeDetail.Text = Convert.ToString(ResourceTable("ButtonEmployeeDetail"), Nothing)
            ButtonEmployeeDetail.Text = If(String.IsNullOrEmpty(ButtonEmployeeDetail.Text), "Employee Detail", ButtonEmployeeDetail.Text)

            ButtonViewError.Text = Convert.ToString(ResourceTable("ButtonViewError"), Nothing)
            ButtonViewError.Text = If(String.IsNullOrEmpty(ButtonViewError.Text), "View Detail Error", ButtonViewError.Text)

            SaveMessage = Convert.ToString(ResourceTable("SaveMessage"), Nothing)
            SaveMessage = If(String.IsNullOrEmpty(SaveMessage), "Saved Successfully", SaveMessage)

            DeleteMessage = Convert.ToString(ResourceTable("DeleteMessage"), Nothing)
            DeleteMessage = If(String.IsNullOrEmpty(DeleteMessage), "Deleted Successfully", DeleteMessage)

            ValidationGroup = Convert.ToString(ResourceTable("ValidationGroup"), Nothing)
            ValidationGroup = If(String.IsNullOrEmpty(ValidationGroup), "VestaEmployees", ValidationGroup)

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

            LabelHeaderMyUniqueId = DirectCast(CurrentRow.FindControl("LabelHeaderMyUniqueId"), Label)
            LabelHeaderMyUniqueId.Text = Convert.ToString(ResourceTable("LabelHeaderMyUniqueId"), Nothing).Trim()
            LabelHeaderMyUniqueId.Text = If(String.IsNullOrEmpty(LabelHeaderMyUniqueId.Text), "My Unique ID", LabelHeaderMyUniqueId.Text)

            LabelHeaderEVVId = DirectCast(CurrentRow.FindControl("LabelHeaderEVVId"), Label)
            LabelHeaderEVVId.Text = Convert.ToString(ResourceTable("LabelHeaderEVVId"), Nothing).Trim()
            LabelHeaderEVVId.Text = If(String.IsNullOrEmpty(LabelHeaderEVVId.Text), "EVV ID", LabelHeaderEVVId.Text)

            LabelHeaderEmployeeNumberVesta = DirectCast(CurrentRow.FindControl("LabelHeaderEmployeeNumberVesta"), Label)
            LabelHeaderEmployeeNumberVesta.Text = Convert.ToString(ResourceTable("LabelHeaderEmployeeNumberVesta"), Nothing).Trim()
            LabelHeaderEmployeeNumberVesta.Text = If(String.IsNullOrEmpty(LabelHeaderEmployeeNumberVesta.Text), "Emp Nbr Vesta", LabelHeaderEmployeeNumberVesta.Text)

            LabelHeaderEmployeeLastName = DirectCast(CurrentRow.FindControl("LabelHeaderEmployeeLastName"), Label)
            LabelHeaderEmployeeLastName.Text = Convert.ToString(ResourceTable("LabelHeaderEmployeeLastName"), Nothing).Trim()
            LabelHeaderEmployeeLastName.Text = If(String.IsNullOrEmpty(LabelHeaderEmployeeLastName.Text), "Last Name", LabelHeaderEmployeeLastName.Text)

            LabelHeaderEmployeeFirstName = DirectCast(CurrentRow.FindControl("LabelHeaderEmployeeFirstName"), Label)
            LabelHeaderEmployeeFirstName.Text = Convert.ToString(ResourceTable("LabelHeaderEmployeeFirstName"), Nothing).Trim()
            LabelHeaderEmployeeFirstName.Text = If(String.IsNullOrEmpty(LabelHeaderEmployeeFirstName.Text), "First Name", LabelHeaderEmployeeFirstName.Text)

            LabelHeaderEmployeePhone = DirectCast(CurrentRow.FindControl("LabelHeaderEmployeePhone"), Label)
            LabelHeaderEmployeePhone.Text = Convert.ToString(ResourceTable("LabelHeaderEmployeePhone"), Nothing).Trim()
            LabelHeaderEmployeePhone.Text = If(String.IsNullOrEmpty(LabelHeaderEmployeePhone.Text), "Phone", LabelHeaderEmployeePhone.Text)

            LabelHeaderEmployeeSSNumber = DirectCast(CurrentRow.FindControl("LabelHeaderEmployeeSSNumber"), Label)
            LabelHeaderEmployeeSSNumber.Text = Convert.ToString(ResourceTable("LabelHeaderEmployeeSSNumber"), Nothing).Trim()
            LabelHeaderEmployeeSSNumber.Text = If(String.IsNullOrEmpty(LabelHeaderEmployeeSSNumber.Text), "Employee SS Number", LabelHeaderEmployeeSSNumber.Text)

            LabelHeaderEmployeePassport = DirectCast(CurrentRow.FindControl("LabelHeaderEmployeePassport"), Label)
            LabelHeaderEmployeePassport.Text = Convert.ToString(ResourceTable("LabelHeaderEmployeePassport"), Nothing).Trim()
            LabelHeaderEmployeePassport.Text = If(String.IsNullOrEmpty(LabelHeaderEmployeePassport.Text), "Employee Passport", LabelHeaderEmployeePassport.Text)

            LabelHeaderEmployeeStartDate = DirectCast(CurrentRow.FindControl("LabelHeaderEmployeeStartDate"), Label)
            LabelHeaderEmployeeStartDate.Text = Convert.ToString(ResourceTable("LabelHeaderEmployeeStartDate"), Nothing).Trim()
            LabelHeaderEmployeeStartDate.Text = If(String.IsNullOrEmpty(LabelHeaderEmployeeStartDate.Text), "Start Date", LabelHeaderEmployeeStartDate.Text)

            LabelHeaderEmployeeEndDate = DirectCast(CurrentRow.FindControl("LabelHeaderEmployeeEndDate"), Label)
            LabelHeaderEmployeeEndDate.Text = Convert.ToString(ResourceTable("LabelHeaderEmployeeEndDate"), Nothing).Trim()
            LabelHeaderEmployeeEndDate.Text = If(String.IsNullOrEmpty(LabelHeaderEmployeeEndDate.Text), "End Date", LabelHeaderEmployeeEndDate.Text)

            LabelHeaderEmployeeDiscipline = DirectCast(CurrentRow.FindControl("LabelHeaderEmployeeDiscipline"), Label)
            LabelHeaderEmployeeDiscipline.Text = Convert.ToString(ResourceTable("LabelHeaderEmployeeDiscipline"), Nothing).Trim()
            LabelHeaderEmployeeDiscipline.Text = If(String.IsNullOrEmpty(LabelHeaderEmployeeDiscipline.Text), "Discipline", LabelHeaderEmployeeDiscipline.Text)

            LabelHeaderBranchName = DirectCast(CurrentRow.FindControl("LabelHeaderBranchName"), Label)
            LabelHeaderBranchName.Text = Convert.ToString(ResourceTable("LabelHeaderBranchName"), Nothing).Trim()
            LabelHeaderBranchName.Text = If(String.IsNullOrEmpty(LabelHeaderBranchName.Text), "Branch Name", LabelHeaderBranchName.Text)

            LabelHeaderError = DirectCast(CurrentRow.FindControl("LabelHeaderError"), Label)
            LabelHeaderError.Text = Convert.ToString(ResourceTable("LabelHeaderError"), Nothing).Trim()
            LabelHeaderError.Text = If(String.IsNullOrEmpty(LabelHeaderError.Text), "Error", LabelHeaderError.Text)

            LabelHeaderEmployeeId = DirectCast(CurrentRow.FindControl("LabelHeaderEmployeeId"), Label)
            LabelHeaderEmployeeId.Text = Convert.ToString(ResourceTable("LabelHeaderEmployeeId"), Nothing).Trim()
            LabelHeaderEmployeeId.Text = If(String.IsNullOrEmpty(LabelHeaderEmployeeId.Text), "Emp Id Turbo", LabelHeaderEmployeeId.Text)

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

            GridViewEmployees.AutoGenerateColumns = False
            GridViewEmployees.ShowHeaderWhenEmpty = True
            GridViewEmployees.AllowPaging = True
            GridViewEmployees.AllowSorting = True

            GridViewEmployees.ShowFooter = True

            If (GridViewEmployees.AllowPaging) Then
                GridViewEmployees.PageSize = objShared.GridViewDefaultPageSize
            End If

            AddHandler GridViewEmployees.RowDataBound, AddressOf GridViewEmployees_RowDataBound
            AddHandler GridViewEmployees.PageIndexChanging, AddressOf GridViewEmployees_PageIndexChanging

            ButtonClear.ClientIDMode = UI.ClientIDMode.Static
            ButtonSave.ClientIDMode = UI.ClientIDMode.Static
            ButtonDelete.ClientIDMode = UI.ClientIDMode.Static
            ButtonEmployeeDetail.ClientIDMode = UI.ClientIDMode.Static

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
                          & " var DeleteDialogHeader ='Vesta Employee'; " _
                          & " var DeleteDialogConfirmMsg ='" & DeleteConfirmationMessage & "'; " _
                          & " var prm =''; " _
                          & " jQuery(document).ready(function () {" _
                          & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                          & "     prm.add_beginRequest(SetButtonActionProgress); " _
                          & "     prm.add_endRequest(EndRequest); " _
                          & "     DataDelete();" _
                          & "     prm.add_endRequest(DataDelete); " _
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
            BindGridViewEmployees()
        End Sub

        Private Sub GetData()
            GetEmployeeData()
        End Sub

        Private Sub GetEmployeeData()

            Dim objBLVestaEmployee As New BLVestaEmployee()

            Try
                Employees = objBLVestaEmployee.SelectEmployeeInfo(objShared.ConVisitel)
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to fetch Employee Data. Message: {0}", ex.Message))
            Finally
                objBLVestaEmployee = Nothing
            End Try
        End Sub

        Private Sub BindGridViewEmployees()
            GridViewEmployees.DataSource = Employees
            GridViewEmployees.DataBind()

            ItemChecked = DetermineButtonInActivity(GridViewEmployees, "CheckBoxSelect")

            ButtonSave.Enabled = ItemChecked
            ButtonDelete.Enabled = ButtonSave.Enabled
            ButtonEmployeeDetail.Enabled = ButtonSave.Enabled
        End Sub

    End Class
End Namespace