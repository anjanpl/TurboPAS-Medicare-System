Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports System.Data.SqlClient
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelCommon
Imports VisitelBusiness.VisitelBusiness.DataObject.EVV

Namespace Visitel.UserControl.EVV
    Public Class MEDsysAuthorizationsControl
        Inherits CommonDataControl

        Private ContractNumber As String
        Private objShared As SharedWebControls

        Private CheckBoxSelect As CheckBox, chkAll As CheckBox
        Private ItemChecked As Boolean = False

        Private LabelHeaderSerial As Label, LabelHeaderSelect As Label, LabelHeaderClientId As Label, LabelHeaderAccountId As Label, LabelHeaderExternalId As Label, _
            LabelHeaderAuthorizationId As Label, LabelHeaderAuthorizationNumber As Label, LabelHeaderControlNumber As Label, LabelHeaderBeginDate As Label, _
            LabelHeaderEndDate As Label, LabelHeaderServiceCode As Label, LabelHeaderActivityCode As Label, LabelHeaderClientExternalId As Label, _
            LabelHeaderAuthType As Label, LabelHeaderMaximum As Label, LabelHeaderLimitBy As Label, LabelHeaderAction As Label, LabelHeaderUpdateDate As Label, _
            LabelHeaderUpdateBy As Label, LabelId As Label, LabelAuthorizationId As Label

        Private TextBoxClientId As TextBox, TextBoxAccountId As TextBox, TextBoxExternalId As TextBox, TextBoxAuthorizationId As TextBox, _
            TextBoxAuthorizationNumber As TextBox, TextBoxControlNumber As TextBox, TextBoxBeginDate As TextBox, TextBoxEndDate As TextBox, _
            TextBoxServiceCode As TextBox, TextBoxActivityCode As TextBox, TextBoxClientExternalId As TextBox, TextBoxAuthType As TextBox, _
            TextBoxMaximum As TextBox, TextBoxLimitBy As TextBox, TextBoxAction As TextBox, TextBoxUpdateDate As TextBox, TextBoxUpdateBy As TextBox

        Private ControlName As String, ValidationGroup As String, SaveMessage As String, DeleteMessage As String, DeleteConfirmationMessage As String, ListFor As String
        Private ValidationEnable As Boolean
        Private CurrentRow As GridViewRow

        Private Authorizations As List(Of MEDsysAuthorizationDataObject), AuthorizationsErrorList As List(Of MEDsysAuthorizationDataObject)

        Private SaveFailedCounter As Int16, DeleteFailedCounter As Int16, RecordSaveCount As Int16


        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "MEDsysAuthorizationsControl"
            objShared = New SharedWebControls()
            objShared.ConnectionOpen()
            GetCaptionFromResource()
            InitializeControl()
        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)

            GetData()

            If Not IsPostBack Then
                BindGridViewMEDsysAuthorizations()
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

            Authorizations = New List(Of MEDsysAuthorizationDataObject)()
            AuthorizationsErrorList = New List(Of MEDsysAuthorizationDataObject)()

            SaveFailedCounter = 0
            RecordSaveCount = 0

            CurrentRow = GridViewMEDsysAuthorizations.FooterRow

            CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)

            LabelAuthorizationId = DirectCast(CurrentRow.FindControl("LabelAuthorizationId"), Label)

            If ((CheckBoxSelect.Checked) And (String.IsNullOrEmpty(LabelAuthorizationId.Text.Trim()))) Then
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
                    BindGridViewMEDsysAuthorizations()
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

            Authorizations = New List(Of MEDsysAuthorizationDataObject)()
            AuthorizationsErrorList = New List(Of MEDsysAuthorizationDataObject)()

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
                BindGridViewMEDsysAuthorizations()
            End If

            ButtonViewError.Visible = If((DeleteFailedCounter > 0), True, False)

            Authorizations = Nothing
            AuthorizationsErrorList = Nothing
        End Sub

        Private Sub ButtonClear_Click(sender As Object, e As EventArgs)
            CurrentRow = GridViewMEDsysAuthorizations.FooterRow
            ClearControl(CurrentRow)
        End Sub

        Protected Sub OnCheckedChanged(sender As Object, e As EventArgs)
            Dim isUpdateVisible As Boolean = False

            Dim chk As CheckBox = TryCast(sender, CheckBox)

            'ButtonSave.Enabled = If((chk.Checked), True, False)

            If chk.ID = "chkAll" Then
                For Each row As GridViewRow In GridViewMEDsysAuthorizations.Rows
                    If row.RowType = DataControlRowType.DataRow Then
                        row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked = chk.Checked
                    End If
                Next

                CurrentRow = GridViewMEDsysAuthorizations.FooterRow
                CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)
                CheckBoxSelect.Checked = chk.Checked

            End If

            Dim chkAll As CheckBox = TryCast(GridViewMEDsysAuthorizations.HeaderRow.FindControl("chkAll"), CheckBox)
            'chkAll.Checked = True
            For Each row As GridViewRow In GridViewMEDsysAuthorizations.Rows
                If row.RowType = DataControlRowType.DataRow Then

                    Dim isChecked As Boolean = row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked

                    ControlsOnSelect(row, isChecked)

                    If (isChecked) Then
                        TextBoxClientId = DirectCast(row.FindControl("TextBoxClientId"), TextBox)
                        If ((Not HiddenFieldClientId Is Nothing) And (Not TextBoxClientId Is Nothing)) Then
                            HiddenFieldClientId.Value = TextBoxClientId.Text.Trim()
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

            ItemChecked = DetermineButtonInActivity(GridViewMEDsysAuthorizations, "CheckBoxSelect")

            ButtonSave.Enabled = ItemChecked
            ButtonDelete.Enabled = ButtonSave.Enabled
            ButtonIndividualDetail.Enabled = ButtonSave.Enabled

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

            Dim objMEDsysAuthorizationDataObject As MEDsysAuthorizationDataObject

            Select Case Action
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT)

                    TextBoxAuthorizationId = DirectCast(GridViewMEDsysAuthorizations.FooterRow.FindControl("TextBoxAuthorizationId"), TextBox)
                    If (String.IsNullOrEmpty(TextBoxAuthorizationId.Text.Trim())) Then
                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please type one staff id.")
                        Return
                    End If

                    objMEDsysAuthorizationDataObject = New MEDsysAuthorizationDataObject()
                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)

                    AddToList(CurrentRow, Authorizations, objMEDsysAuthorizationDataObject)
                    RecordSaveCount = RecordSaveCount + 1
                    objMEDsysAuthorizationDataObject = Nothing

                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE),
                    EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE)
                    Authorizations.Clear()
                    For Each row As GridViewRow In GridViewMEDsysAuthorizations.Rows
                        If (row.RowType.Equals(DataControlRowType.DataRow)) Then
                            Dim isChecked As Boolean = row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked

                            If (isChecked) Then

                                objMEDsysAuthorizationDataObject = New MEDsysAuthorizationDataObject()

                                CurrentRow = DirectCast(GridViewMEDsysAuthorizations.Rows(row.RowIndex), GridViewRow)

                                TextBoxAuthorizationId = DirectCast(CurrentRow.FindControl("TextBoxAuthorizationId"), TextBox)
                                If (String.IsNullOrEmpty(TextBoxAuthorizationId.Text.Trim())) Then
                                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please type one cient id.")
                                    Return
                                End If

                                If (Action.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE))) Then
                                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)
                                End If

                                If (Action.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE))) Then
                                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE)
                                End If

                                AddToList(CurrentRow, Authorizations, objMEDsysAuthorizationDataObject)
                                RecordSaveCount = RecordSaveCount + 1
                                objMEDsysAuthorizationDataObject = Nothing

                            End If

                        End If
                    Next

                    Exit Select
            End Select

            objMEDsysAuthorizationDataObject = Nothing

        End Sub

        ''' <summary>
        ''' Making List in order to save records or delete
        ''' </summary>
        ''' <param name="CurrentRow"></param>
        ''' <param name="Authorizations"></param>
        ''' <param name="objMEDsysAuthorizationDataObject"></param>
        ''' <remarks></remarks>
        Private Sub AddToList(ByRef CurrentRow As GridViewRow, ByRef Authorizations As List(Of MEDsysAuthorizationDataObject),
                              ByRef objMEDsysAuthorizationDataObject As MEDsysAuthorizationDataObject)

            Dim Int32Result As Int32

            LabelId = DirectCast(CurrentRow.FindControl("LabelId"), Label)
            LabelAuthorizationId = DirectCast(CurrentRow.FindControl("LabelAuthorizationId"), Label)

            If (ListFor.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE))) Then
                ControlsFind(CurrentRow)
            End If

            If (Page.IsValid) Then

                objMEDsysAuthorizationDataObject.Id = If(Int32.TryParse(LabelId.Text.Trim(), Int32Result), Int32Result, objMEDsysAuthorizationDataObject.Id)

                If (ListFor.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE))) Then
                    objMEDsysAuthorizationDataObject.ClientId = TextBoxClientId.Text.Trim()
                    objMEDsysAuthorizationDataObject.AccountId = TextBoxAccountId.Text.Trim()
                    objMEDsysAuthorizationDataObject.ExternalId = TextBoxExternalId.Text.Trim()
                    objMEDsysAuthorizationDataObject.AuthorizationId = TextBoxAuthorizationId.Text.Trim()
                    objMEDsysAuthorizationDataObject.AuthorizationNumber = TextBoxAuthorizationNumber.Text.Trim()
                    objMEDsysAuthorizationDataObject.ControlNumber = TextBoxControlNumber.Text.Trim()
                    objMEDsysAuthorizationDataObject.DateBegin = TextBoxBeginDate.Text.Trim()
                    objMEDsysAuthorizationDataObject.DateEnd = TextBoxEndDate.Text.Trim()
                    objMEDsysAuthorizationDataObject.ServiceCode = TextBoxServiceCode.Text.Trim()
                    objMEDsysAuthorizationDataObject.ActivityCode = TextBoxActivityCode.Text.Trim()
                    objMEDsysAuthorizationDataObject.ClientExternalId = TextBoxClientExternalId.Text.Trim()
                    objMEDsysAuthorizationDataObject.AuthType = TextBoxAuthType.Text.Trim()
                    objMEDsysAuthorizationDataObject.Maximum = TextBoxMaximum.Text.Trim()
                    objMEDsysAuthorizationDataObject.LimitBy = TextBoxLimitBy.Text.Trim()
                    objMEDsysAuthorizationDataObject.Action = TextBoxAction.Text.Trim()
                End If

                objMEDsysAuthorizationDataObject.UpdateBy = objShared.UserId

                Authorizations.Add(objMEDsysAuthorizationDataObject)
            End If

        End Sub

        ''' <summary>
        ''' Make Database operation either for record saving or deleting
        ''' </summary>
        ''' <param name="Action"></param>
        ''' <remarks></remarks>
        Private Sub TakeAction(Action As String)

            Dim objBLMEDsysAuthorization As New BLMEDsysAuthorization()

            For Each Authorization In Authorizations
                Try
                    Select Case Action
                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT)
                            objBLMEDsysAuthorization.InsertMEDsysAuthorizationInfo(objShared.ConVisitel, Authorization)
                            Exit Select

                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE)
                            objBLMEDsysAuthorization.UpdateMEDsysAuthorizationInfo(objShared.ConVisitel, Authorization)
                            Exit Select

                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE)
                            objBLMEDsysAuthorization.DeleteMEDsysAuthorizationInfo(objShared.ConVisitel, Authorization.Id, Authorization.UpdateBy)
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

            objBLMEDsysAuthorization = Nothing

        End Sub

        Private Sub ControlsFind(ByRef CurrentRow As GridViewRow)
            TextBoxClientId = DirectCast(CurrentRow.FindControl("TextBoxClientId"), TextBox)
            TextBoxAccountId = DirectCast(CurrentRow.FindControl("TextBoxAccountId"), TextBox)
            TextBoxExternalId = DirectCast(CurrentRow.FindControl("TextBoxExternalId"), TextBox)
            TextBoxAuthorizationId = DirectCast(CurrentRow.FindControl("TextBoxAuthorizationId"), TextBox)
            TextBoxAuthorizationNumber = DirectCast(CurrentRow.FindControl("TextBoxAuthorizationNumber"), TextBox)
            TextBoxControlNumber = DirectCast(CurrentRow.FindControl("TextBoxControlNumber"), TextBox)
            TextBoxBeginDate = DirectCast(CurrentRow.FindControl("TextBoxBeginDate"), TextBox)
            TextBoxEndDate = DirectCast(CurrentRow.FindControl("TextBoxEndDate"), TextBox)
            TextBoxServiceCode = DirectCast(CurrentRow.FindControl("TextBoxServiceCode"), TextBox)
            TextBoxActivityCode = DirectCast(CurrentRow.FindControl("TextBoxActivityCode"), TextBox)
            TextBoxClientExternalId = DirectCast(CurrentRow.FindControl("TextBoxClientExternalId"), TextBox)
            TextBoxAuthType = DirectCast(CurrentRow.FindControl("TextBoxAuthType"), TextBox)
            TextBoxMaximum = DirectCast(CurrentRow.FindControl("TextBoxMaximum"), TextBox)
            TextBoxLimitBy = DirectCast(CurrentRow.FindControl("TextBoxLimitBy"), TextBox)
            TextBoxAction = DirectCast(CurrentRow.FindControl("TextBoxAction"), TextBox)
        End Sub

        ''' <summary>
        ''' Gridview row selection and go for edit mode or record deletion
        ''' </summary>
        ''' <param name="CurrentRow"></param>
        ''' <param name="isChecked"></param>
        ''' <remarks></remarks>
        Private Sub ControlsOnSelect(ByRef CurrentRow As GridViewRow, ByRef isChecked As Boolean)
            ControlsFind(CurrentRow)

            If (Not TextBoxClientId Is Nothing) Then
                TextBoxClientId.ReadOnly = Not isChecked
                TextBoxClientId.CssClass = If((TextBoxClientId.ReadOnly), "TextBoxClientId", "TextBoxClientIdEdit")
            End If

            If (Not TextBoxAccountId Is Nothing) Then
                TextBoxAccountId.ReadOnly = Not isChecked
                TextBoxAccountId.CssClass = If((TextBoxAccountId.ReadOnly), "TextBoxAccountId", "TextBoxAccountIdEdit")
            End If

            If (Not TextBoxExternalId Is Nothing) Then
                TextBoxExternalId.ReadOnly = Not isChecked
                TextBoxExternalId.CssClass = If((TextBoxExternalId.ReadOnly), "TextBoxExternalId", "TextBoxExternalIdEdit")
            End If

            If (Not TextBoxAuthorizationId Is Nothing) Then
                TextBoxAuthorizationId.ReadOnly = Not isChecked
                TextBoxAuthorizationId.CssClass = If((TextBoxAuthorizationId.ReadOnly), "TextBoxAuthorizationId", "TextBoxAuthorizationIdEdit")
            End If

            If (Not TextBoxAuthorizationNumber Is Nothing) Then
                TextBoxAuthorizationNumber.ReadOnly = Not isChecked
                TextBoxAuthorizationNumber.CssClass = If((TextBoxAuthorizationNumber.ReadOnly), "TextBoxAuthorizationNumber", "TextBoxAuthorizationNumberEdit")
            End If

            If (Not TextBoxControlNumber Is Nothing) Then
                TextBoxControlNumber.ReadOnly = Not isChecked
                TextBoxControlNumber.CssClass = If((TextBoxControlNumber.ReadOnly), "TextBoxControlNumber", "TextBoxControlNumberEdit")
            End If

            If (Not TextBoxBeginDate Is Nothing) Then
                TextBoxBeginDate.ReadOnly = Not isChecked
                TextBoxBeginDate.CssClass = If((TextBoxBeginDate.ReadOnly), "TextBoxBeginDate", "TextBoxBeginDateEdit")
            End If

            If (Not TextBoxEndDate Is Nothing) Then
                TextBoxEndDate.ReadOnly = Not isChecked
                TextBoxEndDate.CssClass = If((TextBoxEndDate.ReadOnly), "TextBoxEndDate", "TextBoxEndDateEdit")
            End If

            If (Not TextBoxServiceCode Is Nothing) Then
                TextBoxServiceCode.ReadOnly = Not isChecked
                TextBoxServiceCode.CssClass = If((TextBoxServiceCode.ReadOnly), "TextBoxServiceCode", "TextBoxServiceCodeEdit")
            End If

            If (Not TextBoxActivityCode Is Nothing) Then
                TextBoxActivityCode.ReadOnly = Not isChecked
                TextBoxActivityCode.CssClass = If((TextBoxActivityCode.ReadOnly), "TextBoxActivityCode", "TextBoxActivityCodeEdit")
            End If

            If (Not TextBoxClientExternalId Is Nothing) Then
                TextBoxClientExternalId.ReadOnly = Not isChecked
                TextBoxClientExternalId.CssClass = If((TextBoxClientExternalId.ReadOnly), "TextBoxClientExternalId", "TextBoxClientExternalIdEdit")
            End If

            If (Not TextBoxAuthType Is Nothing) Then
                TextBoxAuthType.ReadOnly = Not isChecked
                TextBoxAuthType.CssClass = If((TextBoxAuthType.ReadOnly), "TextBoxAuthType", "TextBoxAuthTypeEdit")
            End If

            If (Not TextBoxMaximum Is Nothing) Then
                TextBoxMaximum.ReadOnly = Not isChecked
                TextBoxMaximum.CssClass = If((TextBoxMaximum.ReadOnly), "TextBoxMaximum", "TextBoxMaximumEdit")
            End If

            If (Not TextBoxLimitBy Is Nothing) Then
                TextBoxLimitBy.ReadOnly = Not isChecked
                TextBoxLimitBy.CssClass = If((TextBoxLimitBy.ReadOnly), "TextBoxLimitBy", "TextBoxLimitByEdit")
            End If

            If (Not TextBoxAction Is Nothing) Then
                TextBoxAction.ReadOnly = Not isChecked
                TextBoxAction.CssClass = If((TextBoxAction.ReadOnly), "TextBoxAction", "TextBoxActionEdit")
            End If
        End Sub

        Private Sub GridViewMEDsysAuthorizations_RowDataBound(sender As Object, e As GridViewRowEventArgs)
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

                TextBoxClientId = DirectCast(CurrentRow.FindControl("TextBoxClientId"), TextBox)
                TextBoxClientId.ReadOnly = True

                TextBoxAccountId = DirectCast(CurrentRow.FindControl("TextBoxAccountId"), TextBox)
                TextBoxAccountId.ReadOnly = True

                TextBoxExternalId = DirectCast(CurrentRow.FindControl("TextBoxExternalId"), TextBox)
                TextBoxExternalId.ReadOnly = True

                TextBoxAuthorizationId = DirectCast(CurrentRow.FindControl("TextBoxAuthorizationId"), TextBox)
                TextBoxAuthorizationId.ReadOnly = True

                TextBoxAuthorizationNumber = DirectCast(CurrentRow.FindControl("TextBoxAuthorizationNumber"), TextBox)
                TextBoxAuthorizationNumber.ReadOnly = True

                TextBoxControlNumber = DirectCast(CurrentRow.FindControl("TextBoxControlNumber"), TextBox)
                TextBoxControlNumber.ReadOnly = True

                TextBoxBeginDate = DirectCast(CurrentRow.FindControl("TextBoxBeginDate"), TextBox)
                TextBoxBeginDate.ReadOnly = True

                TextBoxEndDate = DirectCast(CurrentRow.FindControl("TextBoxEndDate"), TextBox)
                TextBoxEndDate.ReadOnly = True

                TextBoxServiceCode = DirectCast(CurrentRow.FindControl("TextBoxServiceCode"), TextBox)
                TextBoxServiceCode.ReadOnly = True

                TextBoxActivityCode = DirectCast(CurrentRow.FindControl("TextBoxActivityCode"), TextBox)
                TextBoxActivityCode.ReadOnly = True

                TextBoxClientExternalId = DirectCast(CurrentRow.FindControl("TextBoxClientExternalId"), TextBox)
                TextBoxClientExternalId.ReadOnly = True

                TextBoxAuthType = DirectCast(CurrentRow.FindControl("TextBoxAuthType"), TextBox)
                TextBoxAuthType.ReadOnly = True

                TextBoxMaximum = DirectCast(CurrentRow.FindControl("TextBoxMaximum"), TextBox)
                TextBoxMaximum.ReadOnly = True

                TextBoxLimitBy = DirectCast(CurrentRow.FindControl("TextBoxLimitBy"), TextBox)
                TextBoxLimitBy.ReadOnly = True

                TextBoxAction = DirectCast(CurrentRow.FindControl("TextBoxAction"), TextBox)
                TextBoxAction.ReadOnly = True

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

        Private Sub GridViewMEDsysAuthorizations_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
            GridViewMEDsysAuthorizations.PageIndex = e.NewPageIndex
            BindGridViewMEDsysAuthorizations()
        End Sub

        ''' <summary>
        ''' Gridview Row Hover Color Set
        ''' </summary>
        ''' <param name="writer"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub Render(writer As HtmlTextWriter)
            For Each r As GridViewRow In GridViewMEDsysAuthorizations.Rows
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

            LabelMEDsysAuthorizations.Text = Convert.ToString(ResourceTable("LabelMEDsysAuthorizations"), Nothing)
            LabelMEDsysAuthorizations.Text = If(String.IsNullOrEmpty(LabelMEDsysAuthorizations.Text), "MEDsys Authorizations", LabelMEDsysAuthorizations.Text)

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
            ValidationGroup = If(String.IsNullOrEmpty(ValidationGroup), "MEDsysAuthorizations", ValidationGroup)

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

            LabelHeaderClientId = DirectCast(CurrentRow.FindControl("LabelHeaderClientId"), Label)
            LabelHeaderClientId.Text = Convert.ToString(ResourceTable("LabelHeaderClientId"), Nothing).Trim()
            LabelHeaderClientId.Text = If(String.IsNullOrEmpty(LabelHeaderClientId.Text), "Client Id", LabelHeaderClientId.Text)

            LabelHeaderAccountId = DirectCast(CurrentRow.FindControl("LabelHeaderAccountId"), Label)
            LabelHeaderAccountId.Text = Convert.ToString(ResourceTable("LabelHeaderAccountId"), Nothing).Trim()
            LabelHeaderAccountId.Text = If(String.IsNullOrEmpty(LabelHeaderAccountId.Text), "Account Id", LabelHeaderAccountId.Text)

            LabelHeaderExternalId = DirectCast(CurrentRow.FindControl("LabelHeaderExternalId"), Label)
            LabelHeaderExternalId.Text = Convert.ToString(ResourceTable("LabelHeaderExternalId"), Nothing).Trim()
            LabelHeaderExternalId.Text = If(String.IsNullOrEmpty(LabelHeaderExternalId.Text), "External Id", LabelHeaderExternalId.Text)

            LabelHeaderAuthorizationId = DirectCast(CurrentRow.FindControl("LabelHeaderAuthorizationId"), Label)
            LabelHeaderAuthorizationId.Text = Convert.ToString(ResourceTable("LabelHeaderAuthorizationId"), Nothing).Trim()
            LabelHeaderAuthorizationId.Text = If(String.IsNullOrEmpty(LabelHeaderAuthorizationId.Text), "Authorization Id", LabelHeaderAuthorizationId.Text)

            LabelHeaderAuthorizationNumber = DirectCast(CurrentRow.FindControl("LabelHeaderAuthorizationNumber"), Label)
            LabelHeaderAuthorizationNumber.Text = Convert.ToString(ResourceTable("LabelHeaderAuthorizationNumber"), Nothing).Trim()
            LabelHeaderAuthorizationNumber.Text = If(String.IsNullOrEmpty(LabelHeaderAuthorizationNumber.Text), "Authorization Number", LabelHeaderAuthorizationNumber.Text)

            LabelHeaderControlNumber = DirectCast(CurrentRow.FindControl("LabelHeaderControlNumber"), Label)
            LabelHeaderControlNumber.Text = Convert.ToString(ResourceTable("LabelHeaderControlNumber"), Nothing).Trim()
            LabelHeaderControlNumber.Text = If(String.IsNullOrEmpty(LabelHeaderControlNumber.Text), "Control Number", LabelHeaderControlNumber.Text)

            LabelHeaderBeginDate = DirectCast(CurrentRow.FindControl("LabelHeaderBeginDate"), Label)
            LabelHeaderBeginDate.Text = Convert.ToString(ResourceTable("LabelHeaderBeginDate"), Nothing).Trim()
            LabelHeaderBeginDate.Text = If(String.IsNullOrEmpty(LabelHeaderBeginDate.Text), "Date Begin", LabelHeaderBeginDate.Text)

            LabelHeaderEndDate = DirectCast(CurrentRow.FindControl("LabelHeaderEndDate"), Label)
            LabelHeaderEndDate.Text = Convert.ToString(ResourceTable("LabelHeaderEndDate"), Nothing).Trim()
            LabelHeaderEndDate.Text = If(String.IsNullOrEmpty(LabelHeaderEndDate.Text), "Date End", LabelHeaderEndDate.Text)

            LabelHeaderServiceCode = DirectCast(CurrentRow.FindControl("LabelHeaderServiceCode"), Label)
            LabelHeaderServiceCode.Text = Convert.ToString(ResourceTable("LabelHeaderServiceCode"), Nothing).Trim()
            LabelHeaderServiceCode.Text = If(String.IsNullOrEmpty(LabelHeaderServiceCode.Text), "Service Code", LabelHeaderServiceCode.Text)

            LabelHeaderActivityCode = DirectCast(CurrentRow.FindControl("LabelHeaderActivityCode"), Label)
            LabelHeaderActivityCode.Text = Convert.ToString(ResourceTable("LabelHeaderActivityCode"), Nothing).Trim()
            LabelHeaderActivityCode.Text = If(String.IsNullOrEmpty(LabelHeaderActivityCode.Text), "Activity Code", LabelHeaderActivityCode.Text)

            LabelHeaderClientExternalId = DirectCast(CurrentRow.FindControl("LabelHeaderClientExternalId"), Label)
            LabelHeaderClientExternalId.Text = Convert.ToString(ResourceTable("LabelHeaderClientExternalId"), Nothing).Trim()
            LabelHeaderClientExternalId.Text = If(String.IsNullOrEmpty(LabelHeaderClientExternalId.Text), "Client External Id", LabelHeaderClientExternalId.Text)

            LabelHeaderAuthType = DirectCast(CurrentRow.FindControl("LabelHeaderAuthType"), Label)
            LabelHeaderAuthType.Text = Convert.ToString(ResourceTable("LabelHeaderAuthType"), Nothing).Trim()
            LabelHeaderAuthType.Text = If(String.IsNullOrEmpty(LabelHeaderAuthType.Text), "Auth Type", LabelHeaderAuthType.Text)

            LabelHeaderMaximum = DirectCast(CurrentRow.FindControl("LabelHeaderMaximum"), Label)
            LabelHeaderMaximum.Text = Convert.ToString(ResourceTable("LabelHeaderMaximum"), Nothing).Trim()
            LabelHeaderMaximum.Text = If(String.IsNullOrEmpty(LabelHeaderMaximum.Text), "Maximum", LabelHeaderMaximum.Text)

            LabelHeaderLimitBy = DirectCast(CurrentRow.FindControl("LabelHeaderLimitBy"), Label)
            LabelHeaderLimitBy.Text = Convert.ToString(ResourceTable("LabelHeaderLimitBy"), Nothing).Trim()
            LabelHeaderLimitBy.Text = If(String.IsNullOrEmpty(LabelHeaderLimitBy.Text), "LimitBy", LabelHeaderLimitBy.Text)

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
            AddHandler ButtonViewError.Click, AddressOf ButtonViewError_Click

            GridViewMEDsysAuthorizations.AutoGenerateColumns = False
            GridViewMEDsysAuthorizations.ShowHeaderWhenEmpty = True
            GridViewMEDsysAuthorizations.AllowPaging = True
            GridViewMEDsysAuthorizations.AllowSorting = True

            GridViewMEDsysAuthorizations.ShowFooter = True

            If (GridViewMEDsysAuthorizations.AllowPaging) Then
                GridViewMEDsysAuthorizations.PageSize = objShared.GridViewDefaultPageSize
            End If

            AddHandler GridViewMEDsysAuthorizations.RowDataBound, AddressOf GridViewMEDsysAuthorizations_RowDataBound
            AddHandler GridViewMEDsysAuthorizations.PageIndexChanging, AddressOf GridViewMEDsysAuthorizations_PageIndexChanging

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

        Private Sub LoadJScript()

            LoadJS("JavaScript/jquery.blockUI.js")

            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                          & " var LoaderImagePath ='" & objShared.GetLoaderImagePath & "'; " _
                          & " var DeleteTargetButton ='ButtonDelete'; " _
                          & " var DeleteDialogHeader ='MEDsys Authorization'; " _
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
            BindGridViewMEDsysAuthorizations()
            ButtonViewError.Visible = False
        End Sub

        Private Sub GetData()
            GetAuthorizationData()
        End Sub

        Private Sub GetAuthorizationData()

            Dim objBLMEDsysAuthorization As New BLMEDsysAuthorization()

            Try
                Authorizations = objBLMEDsysAuthorization.SelectMEDsysAuthorizationInfo(objShared.ConVisitel)
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to fetch MEDsys Authorization Data. Message: {0}", ex.Message))
            Finally
                objBLMEDsysAuthorization = Nothing
            End Try
        End Sub

        Private Sub BindGridViewMEDsysAuthorizations()
            GridViewMEDsysAuthorizations.DataSource = Authorizations
            GridViewMEDsysAuthorizations.DataBind()

            ItemChecked = DetermineButtonInActivity(GridViewMEDsysAuthorizations, "CheckBoxSelect")

            ButtonSave.Enabled = ItemChecked
            ButtonDelete.Enabled = ButtonSave.Enabled
            ButtonIndividualDetail.Enabled = ButtonSave.Enabled
        End Sub

    End Class
End Namespace