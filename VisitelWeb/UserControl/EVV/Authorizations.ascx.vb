Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports System.Data.SqlClient
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelBusiness.VisitelBusiness.DataObject.EVV

Namespace Visitel.UserControl.EVV
    Public Class AuthorizationsControl
        Inherits BaseUserControl

        Private ContractNumber As String
        Private objShared As SharedWebControls

        Private CheckBoxSelect As CheckBox, chkAll As CheckBox
        Private ItemChecked As Boolean = False

        Private LabelHeaderClient As Label, LabelHeaderMyUniqueId As Label, LabelHeaderAuthorizationId As Label, LabelHeaderVestaClientId As Label,
            LabelHeaderDadsContractNumber As Label, LabelHeaderProgramType As Label, LabelHeaderAuthorizationPayer As Label, LabelHeaderAuthorizationStartDate As Label,
            LabelHeaderAuthorizationEndDate As Label, LabelHeaderAuthorizationUnits As Label, LabelHeaderAuthorizationUnitsType As Label,
            LabelHeaderAuthorizationNumber As Label, LabelHeaderPayerId As Label, LabelHeaderUpdateDate As Label, LabelHeaderUpdateBy As Label,
            LabelHeaderSerial As Label, LabelHeaderSelect As Label, LabelClientId As Label, LabelId As Label

        Private TextBoxMyUniqueId As TextBox, TextBoxAuthorizationId As TextBox, TextBoxVestaClientId As TextBox, TextBoxDadsContractNumber As TextBox,
            TextBoxProgramType As TextBox, TextBoxAuthorizationPayer As TextBox, TextBoxAuthorizationStartDate As TextBox, TextBoxAuthorizationEndDate As TextBox,
            TextBoxAuthorizationUnits As TextBox, TextBoxAuthorizationUnitsType As TextBox, TextBoxAuthorizationNumber As TextBox, TextBoxPayerId As TextBox,
            TextBoxUpdateDate As TextBox, TextBoxUpdateBy As TextBox, TextBoxClientName As TextBox

        Private DropDownListClient As DropDownList

        Private ControlName As String, ValidationGroup As String, SaveMessage As String, DeleteMessage As String, DeleteConfirmationMessage As String, ListFor As String
        Private ValidationEnable As Boolean
        Private CurrentRow As GridViewRow

        Private Authorizations As List(Of AuthorizationDataObject), AuthorizationsErrorList As List(Of AuthorizationDataObject)

        Private SaveFailedCounter As Int16, DeleteFailedCounter As Int16, RecordSaveCount As Int16

        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "AuthorizationsControl"
            objShared = New SharedWebControls()
            objShared.ConnectionOpen()
            GetCaptionFromResource()
            InitializeControl()
        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)

            GetData()

            If Not IsPostBack Then
                BindGridViewAuthorizations()
            End If

        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadCss("EVV/" & ControlName)
            LoadJScript()
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing
            Authorizations = Nothing
        End Sub


        Private Sub ButtonSave_Click(sender As Object, e As EventArgs)

            Page.Validate()
            Page.Validate(ValidationGroup)

            Authorizations = New List(Of AuthorizationDataObject)()
            AuthorizationsErrorList = New List(Of AuthorizationDataObject)()
            SaveFailedCounter = 0
            RecordSaveCount = 0

            CurrentRow = GridViewAuthorizations.FooterRow

            CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)

            LabelClientId = DirectCast(CurrentRow.FindControl("LabelClientId"), Label)

            If ((CheckBoxSelect.Checked) And (String.IsNullOrEmpty(LabelClientId.Text.Trim()))) Then
                SaveData(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT))
            End If

            SaveData(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE))

            If (SaveFailedCounter > 0) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} Records Failed to Save out of {1}", SaveFailedCounter, Authorizations.Count))
                ViewState("SaveFailedRecord") = objShared.ToDataTable(AuthorizationsErrorList)
                ViewState("ListFor") = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)
            Else
                ViewState("SaveFailedRecord") = Nothing

                If (RecordSaveCount > 0) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} record(s) {1}.", RecordSaveCount, SaveMessage))
                    ViewState.Clear()
                    GetData()
                    BindGridViewAuthorizations()
                End If

            End If

            ButtonViewError.Visible = If((SaveFailedCounter > 0), True, False)

            Authorizations = Nothing
            AuthorizationsErrorList = Nothing

        End Sub

        Private Sub ButtonIndividualDetail_Click(sender As Object, e As EventArgs)
            Response.Redirect("~/Pages/ClientInfo.aspx?ClientId=" & HiddenFieldClientId.Value)
        End Sub


        Private Sub ButtonDelete_Click(sender As Object, e As EventArgs)

            Authorizations = New List(Of AuthorizationDataObject)()
            AuthorizationsErrorList = New List(Of AuthorizationDataObject)()

            MakeList(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE))

            If (Authorizations.Count.Equals(0)) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please select items to delete")
                Return
            End If

            DeleteFailedCounter = 0

            TakeAction(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE))

            If (DeleteFailedCounter > 0) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} Records Failed to Delete out of {1}", DeleteFailedCounter, Authorizations.Count))
                ViewState("DeleteFailedRecord") = objShared.ToDataTable(AuthorizationsErrorList)
                ViewState("ListFor") = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE)
            Else
                ViewState("DeleteFailedRecord") = Nothing

                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} record(s) {1}.", Authorizations.Count, DeleteMessage))
                ViewState.Clear()
                GetData()
                BindGridViewAuthorizations()
            End If

            ButtonViewError.Visible = If((DeleteFailedCounter > 0), True, False)

            Authorizations = Nothing
            AuthorizationsErrorList = Nothing
        End Sub

        Private Sub ButtonClear_Click(sender As Object, e As EventArgs)
            CurrentRow = GridViewAuthorizations.FooterRow
            ClearControl(CurrentRow)
        End Sub

        Protected Sub OnCheckedChanged(sender As Object, e As EventArgs)
            Dim isUpdateVisible As Boolean = False

            Dim chk As CheckBox = TryCast(sender, CheckBox)

            'ButtonSave.Enabled = If((chk.Checked), True, False)

            If chk.ID = "chkAll" Then
                For Each row As GridViewRow In GridViewAuthorizations.Rows
                    If row.RowType = DataControlRowType.DataRow Then
                        row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked = chk.Checked
                    End If
                Next

                CurrentRow = GridViewAuthorizations.FooterRow
                CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)
                CheckBoxSelect.Checked = chk.Checked

            End If

            Dim chkAll As CheckBox = TryCast(GridViewAuthorizations.HeaderRow.FindControl("chkAll"), CheckBox)
            'chkAll.Checked = True
            For Each row As GridViewRow In GridViewAuthorizations.Rows
                If row.RowType = DataControlRowType.DataRow Then

                    Dim isChecked As Boolean = row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked

                    ControlsOnSelect(row, isChecked)

                    If (isChecked) Then
                        LabelClientId = DirectCast(row.FindControl("LabelClientId"), Label)
                        If ((Not HiddenFieldClientId Is Nothing) And (Not LabelClientId Is Nothing)) Then
                            HiddenFieldClientId.Value = LabelClientId.Text.Trim()
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

            ItemChecked = DetermineButtonInActivity(GridViewAuthorizations, "CheckBoxSelect")

            ButtonSave.Enabled = ItemChecked
            ButtonDelete.Enabled = ButtonSave.Enabled
            ButtonIndividualDetail.Enabled = ButtonSave.Enabled

        End Sub

        Private Sub LoadJScript()

            LoadJS("JavaScript/jquery.blockUI.js")

            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                          & " var DeleteTargetButton ='ButtonDelete'; " _
                          & " var DeleteDialogHeader ='Vesta Authorization'; " _
                          & " var DeleteDialogConfirmMsg ='" & DeleteConfirmationMessage & "'; " _
                          & " var LoaderImagePath ='" & objShared.GetLoaderImagePath & "'; " _
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
        ''' Saving Data either insert or update after making list
        ''' </summary>
        ''' <param name="Action"></param>
        ''' <remarks></remarks>
        Private Sub SaveData(Action As String)

            MakeList(Action)

            If (Authorizations.Count.Equals(0)) Then
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

            Dim objAuthorizationDataObject As AuthorizationDataObject

            Select Case Action
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT)

                    DropDownListClient = DirectCast(GridViewAuthorizations.FooterRow.FindControl("DropDownListClient"), DropDownList)
                    If (DropDownListClient.SelectedIndex < 1) Then
                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please select one client")
                        Return
                    End If

                    objAuthorizationDataObject = New AuthorizationDataObject()
                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)

                    AddToList(CurrentRow, Authorizations, objAuthorizationDataObject)
                    RecordSaveCount = RecordSaveCount + 1
                    objAuthorizationDataObject = Nothing

                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE),
                    EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE)
                    Authorizations.Clear()
                    For Each row As GridViewRow In GridViewAuthorizations.Rows
                        If (row.RowType.Equals(DataControlRowType.DataRow)) Then
                            Dim isChecked As Boolean = row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked

                            If (isChecked) Then

                                objAuthorizationDataObject = New AuthorizationDataObject()

                                CurrentRow = DirectCast(GridViewAuthorizations.Rows(row.RowIndex), GridViewRow)

                                DropDownListClient = DirectCast(CurrentRow.FindControl("DropDownListClient"), DropDownList)
                                If (DropDownListClient.SelectedIndex < 1) Then
                                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please select one client")
                                    Return
                                End If

                                If (Action.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE))) Then
                                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)
                                End If

                                If (Action.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE))) Then
                                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE)
                                End If

                                AddToList(CurrentRow, Authorizations, objAuthorizationDataObject)
                                RecordSaveCount = RecordSaveCount + 1
                                objAuthorizationDataObject = Nothing

                            End If

                        End If
                    Next

                    Exit Select
            End Select

            objAuthorizationDataObject = Nothing

        End Sub

        ''' <summary>
        ''' Making List in order to save records or delete
        ''' </summary>
        ''' <param name="CurrentRow"></param>
        ''' <param name="Authorizations"></param>
        ''' <param name="objAuthorizationDataObject"></param>
        ''' <remarks></remarks>
        Private Sub AddToList(ByRef CurrentRow As GridViewRow, ByRef Authorizations As List(Of AuthorizationDataObject), _
                              ByRef objAuthorizationDataObject As AuthorizationDataObject)

            Dim Int32Result As Int32

            LabelId = DirectCast(CurrentRow.FindControl("LabelId"), Label)
            LabelClientId = DirectCast(CurrentRow.FindControl("LabelClientId"), Label)

            If (ListFor.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE))) Then
                ControlsFind(CurrentRow)
            End If

            If (Page.IsValid) Then

                objAuthorizationDataObject.Id = If(Int32.TryParse(LabelId.Text.Trim(), Int32Result), Int32Result, objAuthorizationDataObject.Id)

                If (ListFor.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE))) Then
                    objAuthorizationDataObject.MyUniqueId = TextBoxMyUniqueId.Text.Trim()
                    objAuthorizationDataObject.AuthorizationId = TextBoxAuthorizationId.Text.Trim()
                    objAuthorizationDataObject.ClientIdVesta = TextBoxVestaClientId.Text.Trim()
                    objAuthorizationDataObject.DadsContractNo = TextBoxDadsContractNumber.Text.Trim()
                    objAuthorizationDataObject.ProgramType = TextBoxProgramType.Text.Trim()
                    objAuthorizationDataObject.AuthorizationPayer = TextBoxAuthorizationPayer.Text.Trim()
                    objAuthorizationDataObject.AuthorizationStartDate = TextBoxAuthorizationStartDate.Text.Trim()
                    objAuthorizationDataObject.AuthorizationEndDate = TextBoxAuthorizationEndDate.Text.Trim()
                    objAuthorizationDataObject.AuthorizationUnits = TextBoxAuthorizationUnits.Text.Trim()
                    objAuthorizationDataObject.AuthorizationUnitsType = TextBoxAuthorizationUnitsType.Text.Trim()
                    objAuthorizationDataObject.AuthorizationNumber = TextBoxAuthorizationNumber.Text.Trim()
                    objAuthorizationDataObject.PayerId = Convert.ToInt64(TextBoxPayerId.Text.Trim())
                    objAuthorizationDataObject.ClientId = Convert.ToInt64(DropDownListClient.SelectedValue)
                End If

                objAuthorizationDataObject.UpdateBy = objShared.UserId

                Authorizations.Add(objAuthorizationDataObject)

            End If

        End Sub

        ''' <summary>
        ''' Make Database operation either for record saving or deleting
        ''' </summary>
        ''' <param name="Action"></param>
        ''' <remarks></remarks>
        Private Sub TakeAction(Action As String)

            Dim objBLVestaAuthorization As New BLVestaAuthorization()

            For Each Authorization In Authorizations
                Try
                    Select Case Action
                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT)
                            objBLVestaAuthorization.InsertAuthorizationInfo(objShared.ConVisitel, Authorization)
                            Exit Select

                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE)
                            objBLVestaAuthorization.UpdateAuthorizationInfo(objShared.ConVisitel, Authorization)
                            Exit Select

                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE)
                            objBLVestaAuthorization.DeleteAuthorizationInfo(objShared.ConVisitel, Authorization.Id, Authorization.UpdateBy)
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

                    Authorization.Remarks = ex.Message
                    AuthorizationsErrorList.Add(Authorization)

                Finally
                    
                End Try
            Next

            objBLVestaAuthorization = Nothing

        End Sub

        Private Sub ControlsFind(ByRef CurrentRow As GridViewRow)
            TextBoxMyUniqueId = DirectCast(CurrentRow.FindControl("TextBoxMyUniqueId"), TextBox)
            TextBoxAuthorizationId = DirectCast(CurrentRow.FindControl("TextBoxAuthorizationId"), TextBox)
            TextBoxVestaClientId = DirectCast(CurrentRow.FindControl("TextBoxVestaClientId"), TextBox)
            TextBoxDadsContractNumber = DirectCast(CurrentRow.FindControl("TextBoxDadsContractNumber"), TextBox)
            TextBoxProgramType = DirectCast(CurrentRow.FindControl("TextBoxProgramType"), TextBox)
            TextBoxAuthorizationPayer = DirectCast(CurrentRow.FindControl("TextBoxAuthorizationPayer"), TextBox)
            TextBoxAuthorizationStartDate = DirectCast(CurrentRow.FindControl("TextBoxAuthorizationStartDate"), TextBox)
            TextBoxAuthorizationEndDate = DirectCast(CurrentRow.FindControl("TextBoxAuthorizationEndDate"), TextBox)
            TextBoxAuthorizationUnits = DirectCast(CurrentRow.FindControl("TextBoxAuthorizationUnits"), TextBox)
            TextBoxAuthorizationUnitsType = DirectCast(CurrentRow.FindControl("TextBoxAuthorizationUnitsType"), TextBox)
            TextBoxAuthorizationNumber = DirectCast(CurrentRow.FindControl("TextBoxAuthorizationNumber"), TextBox)
            TextBoxPayerId = DirectCast(CurrentRow.FindControl("TextBoxPayerId"), TextBox)

            If (Not CurrentRow.RowType.Equals(DataControlRowType.Footer)) Then
                TextBoxClientName = DirectCast(CurrentRow.FindControl("TextBoxClientName"), TextBox)
            End If

            DropDownListClient = DirectCast(CurrentRow.FindControl("DropDownListClient"), DropDownList)
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

            If (Not TextBoxAuthorizationId Is Nothing) Then
                TextBoxAuthorizationId.ReadOnly = Not isChecked
                TextBoxAuthorizationId.CssClass = If((TextBoxAuthorizationId.ReadOnly), "TextBoxAuthorizationId", "TextBoxAuthorizationIdEdit")
            End If

            If (Not TextBoxVestaClientId Is Nothing) Then
                TextBoxVestaClientId.ReadOnly = Not isChecked
                TextBoxVestaClientId.CssClass = If((TextBoxVestaClientId.ReadOnly), "TextBoxVestaClientId", "TextBoxVestaClientIdEdit")
            End If

            If (Not TextBoxDadsContractNumber Is Nothing) Then
                TextBoxDadsContractNumber.ReadOnly = Not isChecked
                TextBoxDadsContractNumber.CssClass = If((TextBoxDadsContractNumber.ReadOnly), "TextBoxDadsContractNumber", "TextBoxDadsContractNumberEdit")
            End If

            If (Not TextBoxProgramType Is Nothing) Then
                TextBoxProgramType.ReadOnly = Not isChecked
                TextBoxProgramType.CssClass = If((TextBoxProgramType.ReadOnly), "TextBoxProgramType", "TextBoxProgramTypeEdit")
            End If

            If (Not TextBoxAuthorizationPayer Is Nothing) Then
                TextBoxAuthorizationPayer.ReadOnly = Not isChecked
                TextBoxAuthorizationPayer.CssClass = If((TextBoxAuthorizationPayer.ReadOnly), "TextBoxAuthorizationPayer", "TextBoxAuthorizationPayerEdit")
            End If

            If (Not TextBoxAuthorizationStartDate Is Nothing) Then
                TextBoxAuthorizationStartDate.ReadOnly = Not isChecked
                TextBoxAuthorizationStartDate.CssClass = If((TextBoxAuthorizationStartDate.ReadOnly), "TextBoxAuthorizationStartDate", "TextBoxAuthorizationStartDateEdit")
            End If

            If (Not TextBoxAuthorizationEndDate Is Nothing) Then
                TextBoxAuthorizationEndDate.ReadOnly = Not isChecked
                TextBoxAuthorizationEndDate.CssClass = If((TextBoxAuthorizationEndDate.ReadOnly), "TextBoxAuthorizationEndDate", "TextBoxAuthorizationEndDateEdit")
            End If

            If (Not TextBoxAuthorizationUnits Is Nothing) Then
                TextBoxAuthorizationUnits.ReadOnly = Not isChecked
                TextBoxAuthorizationUnits.CssClass = If((TextBoxAuthorizationUnits.ReadOnly), "TextBoxAuthorizationUnits", "TextBoxAuthorizationUnitsEdit")
            End If

            If (Not TextBoxAuthorizationUnitsType Is Nothing) Then
                TextBoxAuthorizationUnitsType.ReadOnly = Not isChecked
                TextBoxAuthorizationUnitsType.CssClass = If((TextBoxAuthorizationUnitsType.ReadOnly), "TextBoxAuthorizationUnitsType", "TextBoxAuthorizationUnitsTypeEdit")
            End If

            If (Not TextBoxAuthorizationNumber Is Nothing) Then
                TextBoxAuthorizationNumber.ReadOnly = Not isChecked
                TextBoxAuthorizationNumber.CssClass = If((TextBoxAuthorizationNumber.ReadOnly), "TextBoxAuthorizationNumber", "TextBoxAuthorizationNumberEdit")
            End If

            If (Not TextBoxPayerId Is Nothing) Then
                TextBoxPayerId.ReadOnly = Not isChecked
                TextBoxPayerId.CssClass = If((TextBoxPayerId.ReadOnly), "TextBoxPayerId", "TextBoxPayerIdEdit")
            End If

            TextBoxClientName = DirectCast(CurrentRow.FindControl("TextBoxClientName"), TextBox)
            If (Not TextBoxClientName Is Nothing) Then
                TextBoxClientName.Visible = Not isChecked
            End If
        End Sub

        Private Sub GridViewAuthorizations_RowDataBound(sender As Object, e As GridViewRowEventArgs)
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

                TextBoxAuthorizationId = DirectCast(CurrentRow.FindControl("TextBoxAuthorizationId"), TextBox)
                TextBoxAuthorizationId.ReadOnly = True

                TextBoxVestaClientId = DirectCast(CurrentRow.FindControl("TextBoxVestaClientId"), TextBox)
                TextBoxVestaClientId.ReadOnly = True

                TextBoxDadsContractNumber = DirectCast(CurrentRow.FindControl("TextBoxDadsContractNumber"), TextBox)
                TextBoxDadsContractNumber.ReadOnly = True

                TextBoxProgramType = DirectCast(CurrentRow.FindControl("TextBoxProgramType"), TextBox)
                TextBoxProgramType.ReadOnly = True

                TextBoxAuthorizationPayer = DirectCast(CurrentRow.FindControl("TextBoxAuthorizationPayer"), TextBox)
                TextBoxAuthorizationPayer.ReadOnly = True

                TextBoxAuthorizationStartDate = DirectCast(CurrentRow.FindControl("TextBoxAuthorizationStartDate"), TextBox)
                TextBoxAuthorizationStartDate.ReadOnly = True

                TextBoxAuthorizationEndDate = DirectCast(CurrentRow.FindControl("TextBoxAuthorizationEndDate"), TextBox)
                TextBoxAuthorizationEndDate.ReadOnly = True

                TextBoxAuthorizationUnits = DirectCast(CurrentRow.FindControl("TextBoxAuthorizationUnits"), TextBox)
                TextBoxAuthorizationUnits.ReadOnly = True

                TextBoxAuthorizationUnitsType = DirectCast(CurrentRow.FindControl("TextBoxAuthorizationUnitsType"), TextBox)
                TextBoxAuthorizationUnitsType.ReadOnly = True

                TextBoxAuthorizationNumber = DirectCast(CurrentRow.FindControl("TextBoxAuthorizationNumber"), TextBox)
                TextBoxAuthorizationNumber.ReadOnly = True

                TextBoxPayerId = DirectCast(CurrentRow.FindControl("TextBoxPayerId"), TextBox)
                TextBoxPayerId.ReadOnly = True

                TextBoxUpdateDate = DirectCast(CurrentRow.FindControl("TextBoxUpdateDate"), TextBox)
                TextBoxUpdateDate.ReadOnly = True

                TextBoxUpdateBy = DirectCast(CurrentRow.FindControl("TextBoxUpdateBy"), TextBox)
                TextBoxUpdateBy.ReadOnly = True

                LabelClientId = DirectCast(CurrentRow.FindControl("LabelClientId"), Label)

                TextBoxClientName = DirectCast(CurrentRow.FindControl("TextBoxClientName"), TextBox)
                If ((Not TextBoxClientName Is Nothing) And (Not LabelClientId Is Nothing)) Then
                    'TextBoxClientName.Attributes.Add("OnClick", "SetClientId(" & LabelClientId.Text.Trim() & ")")
                    TextBoxClientName.ReadOnly = True
                End If

                DropDownListClient = DirectCast(CurrentRow.FindControl("DropDownListClient"), DropDownList)

                If (Not DropDownListClient Is Nothing) Then
                    DropDownListClient.Visible = False
                End If

                '**************************Fill Out Drop Down and Associate selection on Edit Mode[Start]***********************************
                If ((e.Row.RowState & DataControlRowState.Edit) > 0) Then
                    DropDownListClient = DirectCast(CurrentRow.FindControl("DropDownListClient"), DropDownList)

                    If (Not DropDownListClient Is Nothing) Then

                        objShared.BindClientDropDownList(DropDownListClient, objShared.CompanyId, EnumDataObject.ClientListFor.Individual)

                        DropDownListClient.SelectedIndex = DropDownListClient.Items.IndexOf(
                            DropDownListClient.Items.FindByValue(Convert.ToString(LabelClientId.Text.Trim(), Nothing)))

                    End If
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

                TextBoxUpdateDate = DirectCast(CurrentRow.FindControl("TextBoxUpdateDate"), TextBox)
                TextBoxUpdateDate.ReadOnly = True

                TextBoxUpdateBy = DirectCast(CurrentRow.FindControl("TextBoxUpdateBy"), TextBox)
                TextBoxUpdateBy.ReadOnly = True

            End If

        End Sub

        Private Sub GridViewAuthorizations_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
            GridViewAuthorizations.PageIndex = e.NewPageIndex
            BindGridViewAuthorizations()
        End Sub

        ''' <summary>
        ''' Gridview Row Hover Color Set
        ''' </summary>
        ''' <param name="writer"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub Render(writer As HtmlTextWriter)
            For Each r As GridViewRow In GridViewAuthorizations.Rows
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

            LabelAuthorizations.Text = Convert.ToString(ResourceTable("LabelAuthorizations"), Nothing)
            LabelAuthorizations.Text = If(String.IsNullOrEmpty(LabelAuthorizations.Text), "Vesta Authorizations", LabelAuthorizations.Text)

            ButtonSave.Text = Convert.ToString(ResourceTable("ButtonSave"), Nothing)
            ButtonSave.Text = If(String.IsNullOrEmpty(ButtonSave.Text), "Save", ButtonSave.Text)

            ButtonClear.Text = Convert.ToString(ResourceTable("ButtonClear"), Nothing)
            ButtonClear.Text = If(String.IsNullOrEmpty(ButtonClear.Text), "Clear", ButtonClear.Text)

            ButtonDelete.Text = Convert.ToString(ResourceTable("ButtonDelete"), Nothing)
            ButtonDelete.Text = If(String.IsNullOrEmpty(ButtonDelete.Text), "Delete", ButtonDelete.Text)

            ButtonIndividualDetail.Text = Convert.ToString(ResourceTable("ButtonIndividualDetail"), Nothing)
            ButtonIndividualDetail.Text = If(String.IsNullOrEmpty(ButtonIndividualDetail.Text), "Individual Detail", ButtonIndividualDetail.Text)

            ButtonViewError.Text = Convert.ToString(ResourceTable("ButtonViewError"), Nothing)
            ButtonViewError.Text = If(String.IsNullOrEmpty(ButtonViewError.Text), "View Detail Error", ButtonViewError.Text)

            SaveMessage = Convert.ToString(ResourceTable("SaveMessage"), Nothing)
            SaveMessage = If(String.IsNullOrEmpty(SaveMessage), "Saved Successfully", SaveMessage)

            DeleteMessage = Convert.ToString(ResourceTable("DeleteMessage"), Nothing)
            DeleteMessage = If(String.IsNullOrEmpty(DeleteMessage), "Deleted Successfully", DeleteMessage)

            ValidationGroup = Convert.ToString(ResourceTable("ValidationGroup"), Nothing)
            ValidationGroup = If(String.IsNullOrEmpty(ValidationGroup), "VestaAuthorizations", ValidationGroup)

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
            LabelHeaderClient.Text = If(String.IsNullOrEmpty(LabelHeaderClient.Text), "Name", LabelHeaderClient.Text)

            LabelHeaderMyUniqueId = DirectCast(CurrentRow.FindControl("LabelHeaderMyUniqueId"), Label)
            LabelHeaderMyUniqueId.Text = Convert.ToString(ResourceTable("LabelHeaderMyUniqueId"), Nothing).Trim()
            LabelHeaderMyUniqueId.Text = If(String.IsNullOrEmpty(LabelHeaderMyUniqueId.Text), "Unique ID", LabelHeaderMyUniqueId.Text)

            LabelHeaderAuthorizationId = DirectCast(CurrentRow.FindControl("LabelHeaderAuthorizationId"), Label)
            LabelHeaderAuthorizationId.Text = Convert.ToString(ResourceTable("LabelHeaderAuthorizationId"), Nothing).Trim()
            LabelHeaderAuthorizationId.Text = If(String.IsNullOrEmpty(LabelHeaderAuthorizationId.Text), "Authorization ID", LabelHeaderAuthorizationId.Text)

            LabelHeaderVestaClientId = DirectCast(CurrentRow.FindControl("LabelHeaderVestaClientId"), Label)
            LabelHeaderVestaClientId.Text = Convert.ToString(ResourceTable("LabelHeaderVestaClientId"), Nothing).Trim()
            LabelHeaderVestaClientId.Text = If(String.IsNullOrEmpty(LabelHeaderVestaClientId.Text), "Vesta Client Id", LabelHeaderVestaClientId.Text)

            LabelHeaderDadsContractNumber = DirectCast(CurrentRow.FindControl("LabelHeaderDadsContractNumber"), Label)
            LabelHeaderDadsContractNumber.Text = Convert.ToString(ResourceTable("LabelHeaderDadsContractNumber"), Nothing).Trim()
            LabelHeaderDadsContractNumber.Text = If(String.IsNullOrEmpty(LabelHeaderDadsContractNumber.Text), "Contract No", LabelHeaderDadsContractNumber.Text)

            LabelHeaderProgramType = DirectCast(CurrentRow.FindControl("LabelHeaderProgramType"), Label)
            LabelHeaderProgramType.Text = Convert.ToString(ResourceTable("LabelHeaderProgramType"), Nothing).Trim()
            LabelHeaderProgramType.Text = If(String.IsNullOrEmpty(LabelHeaderProgramType.Text), "Program Type", LabelHeaderProgramType.Text)

            LabelHeaderAuthorizationPayer = DirectCast(CurrentRow.FindControl("LabelHeaderAuthorizationPayer"), Label)
            LabelHeaderAuthorizationPayer.Text = Convert.ToString(ResourceTable("LabelHeaderAuthorizationPayer"), Nothing).Trim()
            LabelHeaderAuthorizationPayer.Text = If(String.IsNullOrEmpty(LabelHeaderAuthorizationPayer.Text), "Payer", LabelHeaderAuthorizationPayer.Text)

            LabelHeaderAuthorizationStartDate = DirectCast(CurrentRow.FindControl("LabelHeaderAuthorizationStartDate"), Label)
            LabelHeaderAuthorizationStartDate.Text = Convert.ToString(ResourceTable("LabelHeaderAuthorizationStartDate"), Nothing).Trim()
            LabelHeaderAuthorizationStartDate.Text = If(String.IsNullOrEmpty(LabelHeaderAuthorizationStartDate.Text), "Auth Start Date", LabelHeaderAuthorizationStartDate.Text)

            LabelHeaderAuthorizationEndDate = DirectCast(CurrentRow.FindControl("LabelHeaderAuthorizationEndDate"), Label)
            LabelHeaderAuthorizationEndDate.Text = Convert.ToString(ResourceTable("LabelHeaderAuthorizationEndDate"), Nothing).Trim()
            LabelHeaderAuthorizationEndDate.Text = If(String.IsNullOrEmpty(LabelHeaderAuthorizationEndDate.Text), "Auth End Date", LabelHeaderAuthorizationEndDate.Text)

            LabelHeaderAuthorizationUnits = DirectCast(CurrentRow.FindControl("LabelHeaderAuthorizationUnits"), Label)
            LabelHeaderAuthorizationUnits.Text = Convert.ToString(ResourceTable("LabelHeaderAuthorizationUnits"), Nothing).Trim()
            LabelHeaderAuthorizationUnits.Text = If(String.IsNullOrEmpty(LabelHeaderAuthorizationUnits.Text), "Auth Units", LabelHeaderAuthorizationUnits.Text)

            LabelHeaderAuthorizationUnitsType = DirectCast(CurrentRow.FindControl("LabelHeaderAuthorizationUnitsType"), Label)
            LabelHeaderAuthorizationUnitsType.Text = Convert.ToString(ResourceTable("LabelHeaderAuthorizationUnitsType"), Nothing).Trim()
            LabelHeaderAuthorizationUnitsType.Text = If(String.IsNullOrEmpty(LabelHeaderAuthorizationUnitsType.Text), "Auth Units Type", LabelHeaderAuthorizationUnitsType.Text)

            LabelHeaderAuthorizationNumber = DirectCast(CurrentRow.FindControl("LabelHeaderAuthorizationNumber"), Label)
            LabelHeaderAuthorizationNumber.Text = Convert.ToString(ResourceTable("LabelHeaderAuthorizationNumber"), Nothing).Trim()
            LabelHeaderAuthorizationNumber.Text = If(String.IsNullOrEmpty(LabelHeaderAuthorizationNumber.Text), "Auth Number", LabelHeaderAuthorizationNumber.Text)

            LabelHeaderPayerId = DirectCast(CurrentRow.FindControl("LabelHeaderPayerId"), Label)
            LabelHeaderPayerId.Text = Convert.ToString(ResourceTable("LabelHeaderPayerId"), Nothing).Trim()
            LabelHeaderPayerId.Text = If(String.IsNullOrEmpty(LabelHeaderPayerId.Text), "Payer Id", LabelHeaderPayerId.Text)

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

            GridViewAuthorizations.AutoGenerateColumns = False
            GridViewAuthorizations.ShowHeaderWhenEmpty = True
            GridViewAuthorizations.AllowPaging = True
            GridViewAuthorizations.AllowSorting = True

            GridViewAuthorizations.ShowFooter = True

            If (GridViewAuthorizations.AllowPaging) Then
                GridViewAuthorizations.PageSize = objShared.GridViewDefaultPageSize
            End If

            AddHandler GridViewAuthorizations.RowDataBound, AddressOf GridViewAuthorizations_RowDataBound
            AddHandler GridViewAuthorizations.PageIndexChanging, AddressOf GridViewAuthorizations_PageIndexChanging

            ButtonClear.ClientIDMode = UI.ClientIDMode.Static
            ButtonSave.ClientIDMode = UI.ClientIDMode.Static
            ButtonDelete.ClientIDMode = UI.ClientIDMode.Static
            ButtonIndividualDetail.ClientIDMode = UI.ClientIDMode.Static

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

        ''' <summary>
        ''' Clearing controls value
        ''' </summary>
        Private Sub ClearControl(CurrentRow As GridViewRow)
            ControlsFind(CurrentRow)
            BindGridViewAuthorizations()
        End Sub

        Private Sub GetData()
            GetAuthorizationData()
        End Sub

        Private Sub GetAuthorizationData()

            Dim objBLVestaAuthorization As New BLVestaAuthorization()

            Try
                Authorizations = objBLVestaAuthorization.SelectAuthorization(objShared.ConVisitel)
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to fetch Authorization Data. Message: {0}", ex.Message))
            Finally
                objBLVestaAuthorization = Nothing
            End Try
        End Sub

        Private Sub BindGridViewAuthorizations()
            GridViewAuthorizations.DataSource = Authorizations
            GridViewAuthorizations.DataBind()

            ItemChecked = DetermineButtonInActivity(GridViewAuthorizations, "CheckBoxSelect")

            ButtonSave.Enabled = ItemChecked
            ButtonDelete.Enabled = ButtonSave.Enabled
            ButtonIndividualDetail.Enabled = ButtonSave.Enabled
        End Sub

    End Class
End Namespace