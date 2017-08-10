Imports VisitelCommon.VisitelCommon
Imports VisitelWeb.TurboPasApi
Imports VisitelBusiness.VisitelBusiness.DataObject.EVV
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelBusiness.VisitelBusiness
Imports System.Data.SqlClient

Namespace Visitel.UserControl.TurboPAS
    Public Class UserMappingControl
        Inherits BaseUserControl

        Private ContractNumber As String
        Private objShared As SharedWebControls

        Private CheckBoxSelect As CheckBox, chkAll As CheckBox
        Private ItemChecked As Boolean = False

        Private DropDownListAgentType As DropDownList

        Private LabelHeaderSerial As Label, LabelHeaderSelect As Label, LabelHeaderUserName As Label, LabelHeaderPassword As Label, LabelHeaderEmail As Label, _
            LabelHeaderUserType As Label, LabelHeaderTurboPASUserName As Label, LabelHeaderUpdateDate As Label, LabelHeaderUpdateBy As Label, LabelUserId As Label, _
            LabelUserName As Label, LabelUserTypeId As Label

        Private TextBoxUserName As TextBox, TextBoxPassword As TextBox, TextBoxEmail As TextBox, TextBoxUserType As TextBox, TextBoxTurboPASUserName As TextBox, _
            TextBoxUpdateDate As TextBox, TextBoxUpdateBy As TextBox

        Private ControlName As String, ValidationGroup As String, SaveMessage As String, DeleteMessage As String, DeleteConfirmationMessage As String, ListFor As String
        Private ValidationEnable As Boolean
        Private CurrentRow As GridViewRow

        Private UserMappings As List(Of UserMappingDataObject), UserMappingsErrorList As List(Of UserMappingDataObject)

        Private SaveFailedCounter As Int16, DeleteFailedCounter As Int16, RecordSaveCount As Int16

        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "UserMappingControl"
            objShared = New SharedWebControls()
            objShared.ConnectionOpen()
            GetCaptionFromResource()
            InitializeControl()
        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)
            GetData()

            If Not IsPostBack Then
                BindGridViewUserMapping()
            End If
        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadCss("TurboPAS/" & ControlName)
            LoadJScript()
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing
            UserMappings = Nothing
        End Sub


        Private Sub ButtonSave_Click(sender As Object, e As EventArgs)

            Page.Validate()
            Page.Validate(ValidationGroup)

            UserMappings = New List(Of UserMappingDataObject)()
            UserMappingsErrorList = New List(Of UserMappingDataObject)()

            SaveFailedCounter = 0
            RecordSaveCount = 0

            CurrentRow = GridViewUserMapping.FooterRow

            CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)

            LabelUserName = DirectCast(CurrentRow.FindControl("LabelUserName"), Label)

            If ((CheckBoxSelect.Checked) And (String.IsNullOrEmpty(LabelUserName.Text.Trim()))) Then
                SaveData(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT))
            End If

            SaveData(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE))

            If (SaveFailedCounter > 0) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} Records Failed to Save out of {1}", SaveFailedCounter, UserMappings.Count))
                ViewState("SaveFailedRecord") = objShared.ToDataTable(UserMappingsErrorList)
                ViewState("ListFor") = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)
            Else
                ViewState("SaveFailedRecord") = Nothing

                If (RecordSaveCount > 0) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} record(s) {1}.", RecordSaveCount, SaveMessage))
                    ViewState.Clear()
                    GetData()
                    BindGridViewUserMapping()
                End If

            End If

            ButtonViewError.Visible = If((SaveFailedCounter > 0), True, False)

            UserMappings = Nothing
            UserMappingsErrorList = Nothing

        End Sub

        Private Sub ButtonDelete_Click(sender As Object, e As EventArgs)
            UserMappings = New List(Of UserMappingDataObject)()
            UserMappingsErrorList = New List(Of UserMappingDataObject)()

            MakeList(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE))

            If (UserMappings.Count.Equals(0)) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please select items to delete")
                Return
            End If

            DeleteFailedCounter = 0

            TakeAction(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE))

            If (DeleteFailedCounter > 0) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} Records Failed to Delete out of {1}", DeleteFailedCounter, UserMappings.Count))
                ViewState("DeleteFailedRecord") = objShared.ToDataTable(UserMappingsErrorList)
                ViewState("ListFor") = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE)
            Else
                ViewState("DeleteFailedRecord") = Nothing

                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} record(s) {1}.", UserMappings.Count, DeleteMessage))
                ViewState.Clear()
                GetData()
                BindGridViewUserMapping()
            End If

            ButtonViewError.Visible = If((DeleteFailedCounter > 0), True, False)

            UserMappings = Nothing
            UserMappingsErrorList = Nothing
        End Sub

        Private Sub ButtonClear_Click(sender As Object, e As EventArgs)
            CurrentRow = GridViewUserMapping.FooterRow
            ClearControl(CurrentRow)
        End Sub

        Protected Sub OnCheckedChanged(sender As Object, e As EventArgs)
            Dim isUpdateVisible As Boolean = False

            Dim chk As CheckBox = TryCast(sender, CheckBox)

            'ButtonSave.Enabled = If((chk.Checked), True, False)

            If chk.ID = "chkAll" Then
                For Each row As GridViewRow In GridViewUserMapping.Rows
                    If row.RowType = DataControlRowType.DataRow Then
                        row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked = chk.Checked
                    End If
                Next

                CurrentRow = GridViewUserMapping.FooterRow
                CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)
                CheckBoxSelect.Checked = chk.Checked

            End If

            Dim chkAll As CheckBox = TryCast(GridViewUserMapping.HeaderRow.FindControl("chkAll"), CheckBox)
            'chkAll.Checked = True
            For Each row As GridViewRow In GridViewUserMapping.Rows
                If row.RowType = DataControlRowType.DataRow Then

                    Dim isChecked As Boolean = row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked

                    ControlsOnSelect(row, isChecked)

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

            ItemChecked = DetermineButtonInActivity(GridViewUserMapping, "CheckBoxSelect")

            ButtonSave.Enabled = ItemChecked
            ButtonDelete.Enabled = ButtonSave.Enabled
        End Sub

        ''' <summary>
        ''' Saving Data either insert or update after making list
        ''' </summary>
        ''' <param name="Action"></param>
        ''' <remarks></remarks>
        Private Sub SaveData(Action As String)

            MakeList(Action)

            If (UserMappings.Count.Equals(0)) Then
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

            Dim objUserMappingDataObject As UserMappingDataObject

            Select Case Action
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT)

                    TextBoxUserName = DirectCast(GridViewUserMapping.FooterRow.FindControl("TextBoxUserName"), TextBox)
                    If (String.IsNullOrEmpty(TextBoxUserName.Text.Trim())) Then
                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please type one user id.")
                        Return
                    End If

                    objUserMappingDataObject = New UserMappingDataObject()
                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)

                    AddToList(CurrentRow, UserMappings, objUserMappingDataObject)
                    RecordSaveCount = RecordSaveCount + 1
                    objUserMappingDataObject = Nothing

                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE),
                    EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE)
                    UserMappings.Clear()
                    For Each row As GridViewRow In GridViewUserMapping.Rows
                        If (row.RowType.Equals(DataControlRowType.DataRow)) Then
                            Dim isChecked As Boolean = row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked

                            If (isChecked) Then

                                objUserMappingDataObject = New UserMappingDataObject()

                                CurrentRow = DirectCast(GridViewUserMapping.Rows(row.RowIndex), GridViewRow)

                                TextBoxUserName = DirectCast(CurrentRow.FindControl("TextBoxUserName"), TextBox)
                                If (String.IsNullOrEmpty(TextBoxUserName.Text.Trim())) Then
                                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please type one user id.")
                                    Return
                                End If

                                If (Action.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE))) Then
                                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)
                                End If

                                If (Action.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE))) Then
                                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE)
                                End If

                                AddToList(CurrentRow, UserMappings, objUserMappingDataObject)
                                RecordSaveCount = RecordSaveCount + 1
                                objUserMappingDataObject = Nothing

                            End If

                        End If
                    Next

                    Exit Select
            End Select

            objUserMappingDataObject = Nothing

        End Sub

        ''' <summary>
        ''' Making List in order to save records or delete
        ''' </summary>
        ''' <param name="CurrentRow"></param>
        ''' <param name="UserMappings"></param>
        ''' <param name="objUserMappingDataObject"></param>
        ''' <remarks></remarks>
        Private Sub AddToList(ByRef CurrentRow As GridViewRow, ByRef UserMappings As List(Of UserMappingDataObject),
                              ByRef objUserMappingDataObject As UserMappingDataObject)

            Dim Int32Result As Int32

            LabelUserId = DirectCast(CurrentRow.FindControl("LabelUserId"), Label)
            LabelUserName = DirectCast(CurrentRow.FindControl("LabelUserName"), Label)

            If (ListFor.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE))) Then
                ControlsFind(CurrentRow)
            End If

            If (Page.IsValid) Then

                objUserMappingDataObject.UserId = If(Int32.TryParse(LabelUserId.Text.Trim(), Int32Result), Int32Result, objUserMappingDataObject.UserId)

                If (ListFor.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE))) Then
                    objUserMappingDataObject.UserName = TextBoxUserName.Text.Trim()
                    objUserMappingDataObject.Password = TextBoxPassword.Text.Trim()
                    objUserMappingDataObject.Email = TextBoxEmail.Text.Trim()
                    objUserMappingDataObject.UserTypeId = Convert.ToInt32(DropDownListAgentType.SelectedValue)
                    objUserMappingDataObject.TurboPASUserName = TextBoxTurboPASUserName.Text.Trim()
                End If

                objUserMappingDataObject.UpdateBy = objShared.UserId

                UserMappings.Add(objUserMappingDataObject)
            End If

        End Sub

        ''' <summary>
        ''' Make Database operation either for record saving or deleting
        ''' </summary>
        ''' <param name="Action"></param>
        ''' <remarks></remarks>
        Private Sub TakeAction(Action As String)

            Dim objBLUserMapping As New BLUserMapping()

            For Each UserMapping In UserMappings
                Try
                    Select Case Action
                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT)
                            objBLUserMapping.InsertUserMapping(objShared.ConVisitel, UserMapping)
                            Exit Select

                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE)
                            objBLUserMapping.UpdateUserMapping(objShared.ConVisitel, UserMapping)
                            Exit Select

                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE)
                            objBLUserMapping.DeleteUserMapping(objShared.ConVisitel, UserMapping.UserId, UserMapping.UpdateBy)
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

                    UserMapping.Remarks = ex.Message
                    UserMappingsErrorList.Add(UserMapping)

                Finally

                End Try
            Next

            objBLUserMapping = Nothing

        End Sub

        Private Sub ControlsFind(ByRef CurrentRow As GridViewRow)
            TextBoxUserName = DirectCast(CurrentRow.FindControl("TextBoxUserName"), TextBox)
            TextBoxPassword = DirectCast(CurrentRow.FindControl("TextBoxPassword"), TextBox)
            TextBoxEmail = DirectCast(CurrentRow.FindControl("TextBoxEmail"), TextBox)
            TextBoxTurboPASUserName = DirectCast(CurrentRow.FindControl("TextBoxTurboPASUserName"), TextBox)

            If (Not CurrentRow.RowType.Equals(DataControlRowType.Footer)) Then
                TextBoxUserType = DirectCast(CurrentRow.FindControl("TextBoxUserType"), TextBox)
            End If

            DropDownListAgentType = DirectCast(CurrentRow.FindControl("DropDownListAgentType"), DropDownList)

        End Sub

        ''' <summary>
        ''' Gridview row selection and go for edit mode or record deletion
        ''' </summary>
        ''' <param name="CurrentRow"></param>
        ''' <param name="isChecked"></param>
        ''' <remarks></remarks>
        Private Sub ControlsOnSelect(ByRef CurrentRow As GridViewRow, ByRef isChecked As Boolean)
            ControlsFind(CurrentRow)

            If (Not TextBoxUserName Is Nothing) Then
                TextBoxUserName.ReadOnly = Not isChecked
                TextBoxUserName.CssClass = If((TextBoxUserName.ReadOnly), "TextBoxUserName", "TextBoxUserNameEdit")
            End If

            If (Not TextBoxPassword Is Nothing) Then
                TextBoxPassword.ReadOnly = Not isChecked
                TextBoxPassword.CssClass = If((TextBoxPassword.ReadOnly), "TextBoxPassword", "TextBoxPasswordEdit")
            End If

            If (Not TextBoxEmail Is Nothing) Then
                TextBoxEmail.ReadOnly = Not isChecked
                TextBoxEmail.CssClass = If((TextBoxEmail.ReadOnly), "TextBoxEmail", "TextBoxEmailEdit")
            End If

            If (Not TextBoxUserType Is Nothing) Then
                TextBoxUserType.ReadOnly = Not isChecked
                TextBoxUserType.CssClass = If((TextBoxUserType.ReadOnly), "TextBoxUserType", "TextBoxUserTypeEdit")
            End If

            If (Not TextBoxTurboPASUserName Is Nothing) Then
                TextBoxTurboPASUserName.ReadOnly = Not isChecked
                TextBoxTurboPASUserName.CssClass = If((TextBoxTurboPASUserName.ReadOnly), "TextBoxTurboPASUserName", "TextBoxTurboPASUserNameEdit")
            End If

            TextBoxUserType = DirectCast(CurrentRow.FindControl("TextBoxUserType"), TextBox)
            If (Not TextBoxUserType Is Nothing) Then
                TextBoxUserType.Visible = Not isChecked
            End If
        End Sub

        Private Sub GridViewUserMapping_RowDataBound(sender As Object, e As GridViewRowEventArgs)
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

                TextBoxUserName = DirectCast(CurrentRow.FindControl("TextBoxUserName"), TextBox)
                TextBoxUserName.ReadOnly = True

                TextBoxPassword = DirectCast(CurrentRow.FindControl("TextBoxPassword"), TextBox)
                TextBoxPassword.ReadOnly = True

                TextBoxEmail = DirectCast(CurrentRow.FindControl("TextBoxEmail"), TextBox)
                TextBoxEmail.ReadOnly = True

                TextBoxUserType = DirectCast(CurrentRow.FindControl("TextBoxUserType"), TextBox)
                TextBoxUserType.ReadOnly = True

                TextBoxTurboPASUserName = DirectCast(CurrentRow.FindControl("TextBoxTurboPASUserName"), TextBox)
                TextBoxTurboPASUserName.ReadOnly = True

                TextBoxUpdateDate = DirectCast(CurrentRow.FindControl("TextBoxUpdateDate"), TextBox)
                TextBoxUpdateDate.ReadOnly = True

                TextBoxUpdateBy = DirectCast(CurrentRow.FindControl("TextBoxUpdateBy"), TextBox)
                TextBoxUpdateBy.ReadOnly = True

                LabelUserTypeId = DirectCast(CurrentRow.FindControl("LabelUserTypeId"), Label)

                TextBoxUserType = DirectCast(CurrentRow.FindControl("TextBoxUserType"), TextBox)
                If ((Not TextBoxUserType Is Nothing) And (Not LabelUserTypeId Is Nothing)) Then
                    TextBoxUserType.ReadOnly = True
                End If

                DropDownListAgentType = DirectCast(CurrentRow.FindControl("DropDownListAgentType"), DropDownList)

                If (Not DropDownListAgentType Is Nothing) Then
                    DropDownListAgentType.Visible = False
                End If

                '**************************Fill Out Drop Down and Associate selection on Edit Mode[Start]***********************************
                If ((e.Row.RowState & DataControlRowState.Edit) > 0) Then
                    DropDownListAgentType = DirectCast(CurrentRow.FindControl("DropDownListAgentType"), DropDownList)

                    If (Not DropDownListAgentType Is Nothing) Then

                        objShared.BindAgentTypeDropDownList(DropDownListAgentType)

                        DropDownListAgentType.SelectedIndex = DropDownListAgentType.Items.IndexOf(
                            DropDownListAgentType.Items.FindByValue(Convert.ToString(LabelUserTypeId.Text.Trim(), Nothing)))

                    End If
                End If
                '**************************Fill Out Drop Down and Associate selection on Edit Mode[End]***********************************

            End If

            If (CurrentRow.RowType.Equals(DataControlRowType.Footer)) Then

                CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)
                CheckBoxSelect.AutoPostBack = True

                DropDownListAgentType = DirectCast(CurrentRow.FindControl("DropDownListAgentType"), DropDownList)

                If (Not DropDownListAgentType Is Nothing) Then
                    objShared.BindAgentTypeDropDownList(DropDownListAgentType)
                End If

                TextBoxUpdateDate = DirectCast(CurrentRow.FindControl("TextBoxUpdateDate"), TextBox)
                TextBoxUpdateDate.ReadOnly = True

                TextBoxUpdateBy = DirectCast(CurrentRow.FindControl("TextBoxUpdateBy"), TextBox)
                TextBoxUpdateBy.ReadOnly = True

            End If

        End Sub

        Private Sub GridViewUserMapping_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
            GridViewUserMapping.PageIndex = e.NewPageIndex
            BindGridViewUserMapping()
        End Sub

        ''' <summary>
        ''' Gridview Row Hover Color Set
        ''' </summary>
        ''' <param name="writer"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub Render(writer As HtmlTextWriter)
            For Each r As GridViewRow In GridViewUserMapping.Rows
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

            LabelUserMapping.Text = Convert.ToString(ResourceTable("LabelUserMapping"), Nothing)
            LabelUserMapping.Text = If(String.IsNullOrEmpty(LabelUserMapping.Text), "TurboPAS User Mapping", LabelUserMapping.Text)

            ButtonSave.Text = Convert.ToString(ResourceTable("ButtonSave"), Nothing)
            ButtonSave.Text = If(String.IsNullOrEmpty(ButtonSave.Text), "Save", ButtonSave.Text)

            ButtonClear.Text = Convert.ToString(ResourceTable("ButtonClear"), Nothing)
            ButtonClear.Text = If(String.IsNullOrEmpty(ButtonClear.Text), "Clear", ButtonClear.Text)

            ButtonDelete.Text = Convert.ToString(ResourceTable("ButtonDelete"), Nothing)
            ButtonDelete.Text = If(String.IsNullOrEmpty(ButtonDelete.Text), "Delete", ButtonDelete.Text)

            ButtonViewError.Text = Convert.ToString(ResourceTable("ButtonViewError"), Nothing)
            ButtonViewError.Text = If(String.IsNullOrEmpty(ButtonViewError.Text), "View Detail Error", ButtonViewError.Text)

            SaveMessage = Convert.ToString(ResourceTable("SaveMessage"), Nothing)
            SaveMessage = If(String.IsNullOrEmpty(SaveMessage), "Saved Successfully", SaveMessage)

            DeleteMessage = Convert.ToString(ResourceTable("DeleteMessage"), Nothing)
            DeleteMessage = If(String.IsNullOrEmpty(DeleteMessage), "Deleted Successfully", DeleteMessage)

            ValidationGroup = Convert.ToString(ResourceTable("ValidationGroup"), Nothing)
            ValidationGroup = If(String.IsNullOrEmpty(ValidationGroup), "TurboPASUserMappings", ValidationGroup)

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

            LabelHeaderUserName = DirectCast(CurrentRow.FindControl("LabelHeaderUserName"), Label)
            LabelHeaderUserName.Text = Convert.ToString(ResourceTable("LabelHeaderUserName"), Nothing).Trim()
            LabelHeaderUserName.Text = If(String.IsNullOrEmpty(LabelHeaderUserName.Text), "User Name", LabelHeaderUserName.Text)

            LabelHeaderPassword = DirectCast(CurrentRow.FindControl("LabelHeaderPassword"), Label)
            LabelHeaderPassword.Text = Convert.ToString(ResourceTable("LabelHeaderPassword"), Nothing).Trim()
            LabelHeaderPassword.Text = If(String.IsNullOrEmpty(LabelHeaderPassword.Text), "Password", LabelHeaderPassword.Text)

            LabelHeaderEmail = DirectCast(CurrentRow.FindControl("LabelHeaderEmail"), Label)
            LabelHeaderEmail.Text = Convert.ToString(ResourceTable("LabelHeaderEmail"), Nothing).Trim()
            LabelHeaderEmail.Text = If(String.IsNullOrEmpty(LabelHeaderEmail.Text), "Email", LabelHeaderEmail.Text)

            LabelHeaderUserType = DirectCast(CurrentRow.FindControl("LabelHeaderUserType"), Label)
            LabelHeaderUserType.Text = Convert.ToString(ResourceTable("LabelHeaderUserType"), Nothing).Trim()
            LabelHeaderUserType.Text = If(String.IsNullOrEmpty(LabelHeaderUserType.Text), "User Type", LabelHeaderUserType.Text)

            LabelHeaderTurboPASUserName = DirectCast(CurrentRow.FindControl("LabelHeaderTurboPASUserName"), Label)
            LabelHeaderTurboPASUserName.Text = Convert.ToString(ResourceTable("LabelHeaderTurboPASUserName"), Nothing).Trim()
            LabelHeaderTurboPASUserName.Text = If(String.IsNullOrEmpty(LabelHeaderTurboPASUserName.Text), "TurboPAS User Name", LabelHeaderTurboPASUserName.Text)

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
            AddHandler ButtonViewError.Click, AddressOf ButtonViewError_Click

            GridViewUserMapping.AutoGenerateColumns = False
            GridViewUserMapping.ShowHeaderWhenEmpty = True
            GridViewUserMapping.AllowPaging = True
            GridViewUserMapping.AllowSorting = True

            GridViewUserMapping.ShowFooter = True

            If (GridViewUserMapping.AllowPaging) Then
                GridViewUserMapping.PageSize = objShared.GridViewDefaultPageSize
            End If

            AddHandler GridViewUserMapping.RowDataBound, AddressOf GridViewUserMapping_RowDataBound
            AddHandler GridViewUserMapping.PageIndexChanging, AddressOf GridViewUserMapping_PageIndexChanging

            ButtonClear.ClientIDMode = UI.ClientIDMode.Static
            ButtonSave.ClientIDMode = UI.ClientIDMode.Static
            ButtonDelete.ClientIDMode = UI.ClientIDMode.Static
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
                          & " var DeleteDialogHeader ='User Mapping'; " _
                          & " var DeleteDialogConfirmMsg ='" & DeleteConfirmationMessage & "'; " _
                          & " var prm =''; " _
                          & " jQuery(document).ready(function () {" _
                          & "     DataDelete();" _
                          & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                          & "     prm.add_beginRequest(SetButtonActionProgress); " _
                          & "     prm.add_endRequest(EndRequest); " _
                          & "     DataDelete();" _
                          & "     prm.add_endRequest(DataDelete); " _
                          & "}); " _
                   & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

            LoadJS("JavaScript/TurboPAS/" & ControlName & ".js")

        End Sub

        ''' <summary>
        ''' Clearing controls value
        ''' </summary>
        Private Sub ClearControl(CurrentRow As GridViewRow)
            ControlsFind(CurrentRow)
            BindGridViewUserMapping()
            ButtonViewError.Visible = False
        End Sub

        Private Sub GetData()
            GetAuthorizationData()
        End Sub

        Private Sub GetAuthorizationData()

            Dim objBLUserMapping As New BLUserMapping()

            Try
                UserMappings = objBLUserMapping.SelectUserMapping(objShared.ConVisitel)
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to fetch User Mapping Data. Message: {0}", ex.Message))
            Finally
                objBLUserMapping = Nothing
            End Try
        End Sub

        Private Sub BindGridViewUserMapping()
            GridViewUserMapping.DataSource = UserMappings
            GridViewUserMapping.DataBind()

            ItemChecked = DetermineButtonInActivity(GridViewUserMapping, "CheckBoxSelect")

            ButtonSave.Enabled = ItemChecked
            ButtonDelete.Enabled = ButtonSave.Enabled
        End Sub

    End Class
End Namespace