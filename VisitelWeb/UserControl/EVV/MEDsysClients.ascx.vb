Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports System.Data.SqlClient
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelCommon
Imports VisitelBusiness.VisitelBusiness.DataObject.EVV

Namespace Visitel.UserControl.EVV
    Public Class MEDsysClientsControl
        Inherits CommonDataControl

        Private ContractNumber As String
        Private objShared As SharedWebControls

        Private CheckBoxSelect As CheckBox, chkAll As CheckBox
        Private ItemChecked As Boolean = False

        Private LabelHeaderSerial As Label, LabelHeaderSelect As Label, LabelHeaderAccountId As Label, LabelHeaderExternalId As Label, LabelHeaderClientId As Label, _
            LabelHeaderClientNumber As Label, LabelHeaderLastName As Label, LabelHeaderFirstName As Label, LabelHeaderMiddleInitial As Label, LabelHeaderDateOfBirth As Label, _
            LabelHeaderGender As Label, LabelHeaderGIN As Label, LabelHeaderAddress As Label, LabelHeaderCity As Label, LabelHeaderState As Label, LabelHeaderZip As Label, _
            LabelHeaderPhone As Label, LabelHeaderNotes As Label, LabelHeaderProgramCode As Label, LabelHeaderRegion As Label, LabelHeaderStatus As Label, _
            LabelHeaderStartDate As Label, LabelHeaderEndDate As Label, LabelHeaderPayor As Label, LabelHeaderServiceGroup As Label, _
            LabelHeaderServiceCode As Label, LabelHeaderContractNumber As Label, LabelHeaderAction As Label, LabelHeaderClient_Id As Label, _
            LabelHeaderCompanyCode As Label, LabelHeaderLocationCode As Label, LabelHeaderUpdateDate As Label, LabelHeaderUpdateBy As Label, LabelId As Label, _
            LabelClientId As Label

        Private TextBoxAccountId As TextBox, TextBoxExternalId As TextBox, TextBoxClientId As TextBox, TextBoxClientNumber As TextBox, TextBoxLastName As TextBox, _
            TextBoxFirstName As TextBox, TextBoxMiddleInitial As TextBox, TextBoxDateOfBirth As TextBox, TextBoxGender As TextBox, TextBoxGIN As TextBox, _
            TextBoxAddress As TextBox, TextBoxCity As TextBox, TextBoxState As TextBox, TextBoxZip As TextBox, TextBoxPhone As TextBox, TextBoxNotes As TextBox, _
            TextBoxProgramCode As TextBox, TextBoxRegion As TextBox, TextBoxStatus As TextBox, TextBoxStartDate As TextBox, TextBoxEndDate As TextBox, _
            TextBoxPayor As TextBox, TextBoxServiceGroup As TextBox, TextBoxServiceCode As TextBox, TextBoxContractNumber As TextBox, _
            TextBoxAction As TextBox, TextBoxClient_Id As TextBox, TextBoxCompanyCode As TextBox, TextBoxLocationCode As TextBox, TextBoxUpdateDate As TextBox, TextBoxUpdateBy As TextBox

        Private ControlName As String, ValidationGroup As String, SaveMessage As String, DeleteMessage As String, DeleteConfirmationMessage As String, ListFor As String
        Private ValidationEnable As Boolean
        Private CurrentRow As GridViewRow

        Private Clients As List(Of MEDsysClientDataObject), ClientsErrorList As List(Of MEDsysClientDataObject)

        Private SaveFailedCounter As Int16, DeleteFailedCounter As Int16, RecordSaveCount As Int16


        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "MEDsysClientsControl"
            objShared = New SharedWebControls()
            objShared.ConnectionOpen()
            GetCaptionFromResource()
            InitializeControl()
        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)

            GetData()

            If Not IsPostBack Then
                BindGridViewMEDsysClients()
            End If

        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadCss("EVV/" & ControlName)
            LoadJScript()
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing
            Clients = Nothing
        End Sub


        Private Sub ButtonSave_Click(sender As Object, e As EventArgs)

            Page.Validate()
            Page.Validate(ValidationGroup)

            Clients = New List(Of MEDsysClientDataObject)()
            ClientsErrorList = New List(Of MEDsysClientDataObject)()

            SaveFailedCounter = 0
            RecordSaveCount = 0

            CurrentRow = GridViewMEDsysClients.FooterRow

            CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)

            LabelClientId = DirectCast(CurrentRow.FindControl("LabelClientId"), Label)

            If ((CheckBoxSelect.Checked) And (String.IsNullOrEmpty(LabelClientId.Text.Trim()))) Then
                SaveData(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT))
            End If

            SaveData(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE))

            If (SaveFailedCounter > 0) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} Records Failed to Save out of {1}", SaveFailedCounter, Clients.Count))
                ViewState("SaveFailedRecord") = objShared.ToDataTable(ClientsErrorList)
                ViewState("ListFor") = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)
            Else
                ViewState("SaveFailedRecord") = Nothing

                If (RecordSaveCount > 0) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} record(s) {1}.", RecordSaveCount, SaveMessage))
                    ViewState.Clear()
                    GetData()
                    BindGridViewMEDsysClients()
                End If

            End If

            ButtonViewError.Visible = If((SaveFailedCounter > 0), True, False)

            Clients = Nothing
            ClientsErrorList = Nothing

        End Sub

        Private Sub ButtonIndividualDetail_Click(sender As Object, e As EventArgs)
            Response.Redirect("~/Pages/ClientInfo.aspx?ClientId=" & HiddenFieldClientId.Value)
        End Sub


        Private Sub ButtonDelete_Click(sender As Object, e As EventArgs)

            Clients = New List(Of MEDsysClientDataObject)()
            ClientsErrorList = New List(Of MEDsysClientDataObject)()

            MakeList(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE))

            If (Clients.Count.Equals(0)) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please select items to delete")
                Return
            End If

            DeleteFailedCounter = 0

            TakeAction(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE))

            If (DeleteFailedCounter > 0) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} Records Failed to Delete out of {1}", DeleteFailedCounter, Clients.Count))
                ViewState("DeleteFailedRecord") = objShared.ToDataTable(ClientsErrorList)
                ViewState("ListFor") = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE)
            Else
                ViewState("DeleteFailedRecord") = Nothing

                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} record(s) {1}.", Clients.Count, DeleteMessage))
                ViewState.Clear()
                GetData()
                BindGridViewMEDsysClients()
            End If

            ButtonViewError.Visible = If((DeleteFailedCounter > 0), True, False)

            Clients = Nothing
            ClientsErrorList = Nothing
        End Sub

        Private Sub ButtonClear_Click(sender As Object, e As EventArgs)
            CurrentRow = GridViewMEDsysClients.FooterRow
            ClearControl(CurrentRow)
        End Sub

        Protected Sub OnCheckedChanged(sender As Object, e As EventArgs)
            Dim isUpdateVisible As Boolean = False

            Dim chk As CheckBox = TryCast(sender, CheckBox)

            'ButtonSave.Enabled = If((chk.Checked), True, False)

            If chk.ID = "chkAll" Then
                For Each row As GridViewRow In GridViewMEDsysClients.Rows
                    If row.RowType = DataControlRowType.DataRow Then
                        row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked = chk.Checked
                    End If
                Next

                CurrentRow = GridViewMEDsysClients.FooterRow
                CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)
                CheckBoxSelect.Checked = chk.Checked

            End If

            Dim chkAll As CheckBox = TryCast(GridViewMEDsysClients.HeaderRow.FindControl("chkAll"), CheckBox)
            'chkAll.Checked = True
            For Each row As GridViewRow In GridViewMEDsysClients.Rows
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

            ItemChecked = DetermineButtonInActivity(GridViewMEDsysClients, "CheckBoxSelect")

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

            If (Clients.Count.Equals(0)) Then
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

            Dim objMEDsysClientDataObject As MEDsysClientDataObject

            Select Case Action
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT)

                    TextBoxClientId = DirectCast(GridViewMEDsysClients.FooterRow.FindControl("TextBoxClientId"), TextBox)
                    If (String.IsNullOrEmpty(TextBoxClientId.Text.Trim())) Then
                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please type one staff id.")
                        Return
                    End If

                    objMEDsysClientDataObject = New MEDsysClientDataObject()
                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)

                    AddToList(CurrentRow, Clients, objMEDsysClientDataObject)
                    RecordSaveCount = RecordSaveCount + 1
                    objMEDsysClientDataObject = Nothing

                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE),
                    EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE)
                    Clients.Clear()
                    For Each row As GridViewRow In GridViewMEDsysClients.Rows
                        If (row.RowType.Equals(DataControlRowType.DataRow)) Then
                            Dim isChecked As Boolean = row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked

                            If (isChecked) Then

                                objMEDsysClientDataObject = New MEDsysClientDataObject()

                                CurrentRow = DirectCast(GridViewMEDsysClients.Rows(row.RowIndex), GridViewRow)

                                TextBoxClientId = DirectCast(CurrentRow.FindControl("TextBoxClientId"), TextBox)
                                If (String.IsNullOrEmpty(TextBoxClientId.Text.Trim())) Then
                                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please type one cient id.")
                                    Return
                                End If

                                If (Action.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE))) Then
                                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)
                                End If

                                If (Action.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE))) Then
                                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE)
                                End If

                                AddToList(CurrentRow, Clients, objMEDsysClientDataObject)
                                RecordSaveCount = RecordSaveCount + 1
                                objMEDsysClientDataObject = Nothing

                            End If

                        End If
                    Next

                    Exit Select
            End Select

            objMEDsysClientDataObject = Nothing

        End Sub

        ''' <summary>
        ''' Making List in order to save records or delete
        ''' </summary>
        ''' <param name="CurrentRow"></param>
        ''' <param name="Clients"></param>
        ''' <param name="objMEDsysClientDataObject"></param>
        ''' <remarks></remarks>
        Private Sub AddToList(ByRef CurrentRow As GridViewRow, ByRef Clients As List(Of MEDsysClientDataObject), ByRef objMEDsysClientDataObject As MEDsysClientDataObject)

            Dim Int32Result As Int32

            LabelId = DirectCast(CurrentRow.FindControl("LabelId"), Label)
            LabelClientId = DirectCast(CurrentRow.FindControl("LabelClientId"), Label)

            If (ListFor.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE))) Then
                ControlsFind(CurrentRow)
            End If

            If (Page.IsValid) Then

                objMEDsysClientDataObject.Id = If(Int32.TryParse(LabelId.Text.Trim(), Int32Result), Int32Result, objMEDsysClientDataObject.Id)

                If (ListFor.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE))) Then
                    objMEDsysClientDataObject.AccountId = TextBoxAccountId.Text.Trim()
                    objMEDsysClientDataObject.ExternalId = TextBoxExternalId.Text.Trim()
                    objMEDsysClientDataObject.ClientId = TextBoxClientId.Text.Trim()
                    objMEDsysClientDataObject.ClientNumber = TextBoxClientNumber.Text.Trim()
                    objMEDsysClientDataObject.LastName = TextBoxLastName.Text.Trim()
                    objMEDsysClientDataObject.FirstName = TextBoxFirstName.Text.Trim()
                    objMEDsysClientDataObject.MiddleInit = TextBoxMiddleInitial.Text.Trim()
                    objMEDsysClientDataObject.Birthdate = TextBoxDateOfBirth.Text.Trim()
                    objMEDsysClientDataObject.Gender = TextBoxGender.Text.Trim()
                    objMEDsysClientDataObject.GIN = TextBoxGIN.Text.Trim()
                    objMEDsysClientDataObject.Address = TextBoxAddress.Text.Trim()
                    objMEDsysClientDataObject.City = TextBoxCity.Text.Trim()
                    objMEDsysClientDataObject.State = TextBoxState.Text.Trim()
                    objMEDsysClientDataObject.Zip = TextBoxZip.Text.Trim()
                    objMEDsysClientDataObject.Phone = TextBoxPhone.Text.Trim
                    objMEDsysClientDataObject.Notes = TextBoxNotes.Text.Trim()
                    objMEDsysClientDataObject.ProgramCode = TextBoxProgramCode.Text.Trim()
                    objMEDsysClientDataObject.Region = TextBoxRegion.Text.Trim()
                    objMEDsysClientDataObject.Status = TextBoxStatus.Text.Trim()
                    objMEDsysClientDataObject.StartDate = TextBoxStartDate.Text.Trim()
                    objMEDsysClientDataObject.EndDate = TextBoxEndDate.Text.Trim()
                    objMEDsysClientDataObject.Payor = TextBoxPayor.Text.Trim()
                    objMEDsysClientDataObject.ServiceGroup = TextBoxServiceGroup.Text.Trim()
                    objMEDsysClientDataObject.PayorServiceCode = TextBoxServiceCode.Text.Trim()
                    objMEDsysClientDataObject.ContractNumber = TextBoxContractNumber.Text.Trim()
                    objMEDsysClientDataObject.Action = TextBoxAction.Text.Trim()
                    objMEDsysClientDataObject.Client_Id = TextBoxClient_Id.Text.Trim()
                    objMEDsysClientDataObject.CompanyCode = TextBoxCompanyCode.Text.Trim()
                    objMEDsysClientDataObject.LocationCode = TextBoxLocationCode.Text.Trim()
                End If

                objMEDsysClientDataObject.UpdateBy = objShared.UserId

                Clients.Add(objMEDsysClientDataObject)
            End If

        End Sub

        ''' <summary>
        ''' Make Database operation either for record saving or deleting
        ''' </summary>
        ''' <param name="Action"></param>
        ''' <remarks></remarks>
        Private Sub TakeAction(Action As String)

            Dim objBLMEDsysClient As New BLMEDsysClient()

            For Each Client In Clients
                Try
                    Select Case Action
                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT)
                            objBLMEDsysClient.InsertMEDsysClientInfo(objShared.ConVisitel, Client)
                            Exit Select

                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE)
                            objBLMEDsysClient.UpdateMEDsysClientInfo(objShared.ConVisitel, Client)
                            Exit Select

                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE)
                            objBLMEDsysClient.DeleteMEDsysClientInfo(objShared.ConVisitel, Client.Id, Client.UpdateBy)
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

                    Client.Remarks = ex.Message
                    ClientsErrorList.Add(Client)

                Finally

                End Try
            Next

            objBLMEDsysClient = Nothing

        End Sub

        Private Sub ControlsFind(ByRef CurrentRow As GridViewRow)
            TextBoxAccountId = DirectCast(CurrentRow.FindControl("TextBoxAccountId"), TextBox)
            TextBoxExternalId = DirectCast(CurrentRow.FindControl("TextBoxExternalId"), TextBox)
            TextBoxClientId = DirectCast(CurrentRow.FindControl("TextBoxClientId"), TextBox)
            TextBoxClientNumber = DirectCast(CurrentRow.FindControl("TextBoxClientNumber"), TextBox)
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
            TextBoxProgramCode = DirectCast(CurrentRow.FindControl("TextBoxProgramCode"), TextBox)
            TextBoxRegion = DirectCast(CurrentRow.FindControl("TextBoxRegion"), TextBox)
            TextBoxStatus = DirectCast(CurrentRow.FindControl("TextBoxStatus"), TextBox)
            TextBoxStartDate = DirectCast(CurrentRow.FindControl("TextBoxStartDate"), TextBox)
            TextBoxEndDate = DirectCast(CurrentRow.FindControl("TextBoxEndDate"), TextBox)
            TextBoxPayor = DirectCast(CurrentRow.FindControl("TextBoxPayor"), TextBox)
            TextBoxServiceGroup = DirectCast(CurrentRow.FindControl("TextBoxServiceGroup"), TextBox)
            TextBoxServiceCode = DirectCast(CurrentRow.FindControl("TextBoxServiceCode"), TextBox)
            TextBoxContractNumber = DirectCast(CurrentRow.FindControl("TextBoxContractNumber"), TextBox)
            TextBoxAction = DirectCast(CurrentRow.FindControl("TextBoxAction"), TextBox)
            TextBoxClient_Id = DirectCast(CurrentRow.FindControl("TextBoxClient_Id"), TextBox)
            TextBoxCompanyCode = DirectCast(CurrentRow.FindControl("TextBoxCompanyCode"), TextBox)
            TextBoxLocationCode = DirectCast(CurrentRow.FindControl("TextBoxLocationCode"), TextBox)
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

            If (Not TextBoxClientId Is Nothing) Then
                TextBoxClientId.ReadOnly = Not isChecked
                TextBoxClientId.CssClass = If((TextBoxClientId.ReadOnly), "TextBoxClientId", "TextBoxClientIdEdit")
            End If

            If (Not TextBoxClientNumber Is Nothing) Then
                TextBoxClientNumber.ReadOnly = Not isChecked
                TextBoxClientNumber.CssClass = If((TextBoxClientNumber.ReadOnly), "TextBoxClientNumber", "TextBoxClientNumberEdit")
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

            If (Not TextBoxProgramCode Is Nothing) Then
                TextBoxProgramCode.ReadOnly = Not isChecked
                TextBoxProgramCode.CssClass = If((TextBoxProgramCode.ReadOnly), "TextBoxProgramCode", "TextBoxProgramCodeEdit")
            End If

            If (Not TextBoxRegion Is Nothing) Then
                TextBoxRegion.ReadOnly = Not isChecked
                TextBoxRegion.CssClass = If((TextBoxRegion.ReadOnly), "TextBoxRegion", "TextBoxRegionEdit")
            End If

            If (Not TextBoxStatus Is Nothing) Then
                TextBoxStatus.ReadOnly = Not isChecked
                TextBoxStatus.CssClass = If((TextBoxStatus.ReadOnly), "TextBoxStatus", "TextBoxStatusEdit")
            End If

            If (Not TextBoxStartDate Is Nothing) Then
                TextBoxStartDate.ReadOnly = Not isChecked
                TextBoxStartDate.CssClass = If((TextBoxStartDate.ReadOnly), "TextBoxStartDate", "TextBoxStartDateEdit")
            End If

            If (Not TextBoxEndDate Is Nothing) Then
                TextBoxEndDate.ReadOnly = Not isChecked
                TextBoxEndDate.CssClass = If((TextBoxEndDate.ReadOnly), "TextBoxEndDate", "TextBoxEndDateEdit")
            End If

            If (Not TextBoxPayor Is Nothing) Then
                TextBoxPayor.ReadOnly = Not isChecked
                TextBoxPayor.CssClass = If((TextBoxPayor.ReadOnly), "TextBoxPayor", "TextBoxPayorEdit")
            End If

            If (Not TextBoxServiceGroup Is Nothing) Then
                TextBoxServiceGroup.ReadOnly = Not isChecked
                TextBoxServiceGroup.CssClass = If((TextBoxServiceGroup.ReadOnly), "TextBoxServiceGroup", "TextBoxServiceGroupEdit")
            End If

            If (Not TextBoxServiceCode Is Nothing) Then
                TextBoxServiceCode.ReadOnly = Not isChecked
                TextBoxServiceCode.CssClass = If((TextBoxServiceCode.ReadOnly), "TextBoxServiceCode", "TextBoxServiceCodeEdit")
            End If

            If (Not TextBoxContractNumber Is Nothing) Then
                TextBoxContractNumber.ReadOnly = Not isChecked
                TextBoxContractNumber.CssClass = If((TextBoxContractNumber.ReadOnly), "TextBoxContractNumber", "TextBoxContractNumberEdit")
            End If

            If (Not TextBoxAction Is Nothing) Then
                TextBoxAction.ReadOnly = Not isChecked
                TextBoxAction.CssClass = If((TextBoxAction.ReadOnly), "TextBoxAction", "TextBoxActionEdit")
            End If

            If (Not TextBoxClient_Id Is Nothing) Then
                TextBoxClient_Id.ReadOnly = Not isChecked
                TextBoxClient_Id.CssClass = If((TextBoxClient_Id.ReadOnly), "TextBoxClient_Id", "TextBoxClient_IdEdit")
            End If

            If (Not TextBoxCompanyCode Is Nothing) Then
                TextBoxCompanyCode.ReadOnly = Not isChecked
                TextBoxCompanyCode.CssClass = If((TextBoxCompanyCode.ReadOnly), "TextBoxCompanyCode", "TextBoxCompanyCodeEdit")
            End If

            If (Not TextBoxLocationCode Is Nothing) Then
                TextBoxLocationCode.ReadOnly = Not isChecked
                TextBoxLocationCode.CssClass = If((TextBoxLocationCode.ReadOnly), "TextBoxLocationCode", "TextBoxLocationCodeEdit")
            End If
        End Sub

        Private Sub GridViewMEDsysClients_RowDataBound(sender As Object, e As GridViewRowEventArgs)
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

                TextBoxClientId = DirectCast(CurrentRow.FindControl("TextBoxClientId"), TextBox)
                TextBoxClientId.ReadOnly = True

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

                TextBoxStatus = DirectCast(CurrentRow.FindControl("TextBoxStatus"), TextBox)
                TextBoxStatus.ReadOnly = True

                TextBoxStartDate = DirectCast(CurrentRow.FindControl("TextBoxStartDate"), TextBox)
                TextBoxStartDate.ReadOnly = True

                TextBoxEndDate = DirectCast(CurrentRow.FindControl("TextBoxEndDate"), TextBox)
                TextBoxEndDate.ReadOnly = True

                TextBoxAction = DirectCast(CurrentRow.FindControl("TextBoxAction"), TextBox)
                TextBoxAction.ReadOnly = True

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

        Private Sub GridViewMEDsysClients_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
            GridViewMEDsysClients.PageIndex = e.NewPageIndex
            BindGridViewMEDsysClients()
        End Sub

        ''' <summary>
        ''' Gridview Row Hover Color Set
        ''' </summary>
        ''' <param name="writer"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub Render(writer As HtmlTextWriter)
            For Each r As GridViewRow In GridViewMEDsysClients.Rows
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

            LabelMEDsysClients.Text = Convert.ToString(ResourceTable("LabelMEDsysClients"), Nothing)
            LabelMEDsysClients.Text = If(String.IsNullOrEmpty(LabelMEDsysClients.Text), "MEDsys Clients", LabelMEDsysClients.Text)

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
            ValidationGroup = If(String.IsNullOrEmpty(ValidationGroup), "MEDsysClients", ValidationGroup)

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

            LabelHeaderClientId = DirectCast(CurrentRow.FindControl("LabelHeaderClientId"), Label)
            LabelHeaderClientId.Text = Convert.ToString(ResourceTable("LabelHeaderClientId"), Nothing).Trim()
            LabelHeaderClientId.Text = If(String.IsNullOrEmpty(LabelHeaderClientId.Text), "Client Id", LabelHeaderClientId.Text)

            LabelHeaderClientNumber = DirectCast(CurrentRow.FindControl("LabelHeaderClientNumber"), Label)
            LabelHeaderClientNumber.Text = Convert.ToString(ResourceTable("LabelHeaderClientNumber"), Nothing).Trim()
            LabelHeaderClientNumber.Text = If(String.IsNullOrEmpty(LabelHeaderClientNumber.Text), "Client Number", LabelHeaderClientNumber.Text)

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

            LabelHeaderProgramCode = DirectCast(CurrentRow.FindControl("LabelHeaderProgramCode"), Label)
            LabelHeaderProgramCode.Text = Convert.ToString(ResourceTable("LabelHeaderProgramCode"), Nothing).Trim()
            LabelHeaderProgramCode.Text = If(String.IsNullOrEmpty(LabelHeaderProgramCode.Text), "Program Code", LabelHeaderProgramCode.Text)

            LabelHeaderRegion = DirectCast(CurrentRow.FindControl("LabelHeaderRegion"), Label)
            LabelHeaderRegion.Text = Convert.ToString(ResourceTable("LabelHeaderRegion"), Nothing).Trim()
            LabelHeaderRegion.Text = If(String.IsNullOrEmpty(LabelHeaderRegion.Text), "Region", LabelHeaderRegion.Text)

            LabelHeaderStatus = DirectCast(CurrentRow.FindControl("LabelHeaderStatus"), Label)
            LabelHeaderStatus.Text = Convert.ToString(ResourceTable("LabelHeaderStatus"), Nothing).Trim()
            LabelHeaderStatus.Text = If(String.IsNullOrEmpty(LabelHeaderStatus.Text), "Status", LabelHeaderStatus.Text)

            LabelHeaderStartDate = DirectCast(CurrentRow.FindControl("LabelHeaderStartDate"), Label)
            LabelHeaderStartDate.Text = Convert.ToString(ResourceTable("LabelHeaderStartDate"), Nothing).Trim()
            LabelHeaderStartDate.Text = If(String.IsNullOrEmpty(LabelHeaderStartDate.Text), "Start Date", LabelHeaderStartDate.Text)

            LabelHeaderEndDate = DirectCast(CurrentRow.FindControl("LabelHeaderEndDate"), Label)
            LabelHeaderEndDate.Text = Convert.ToString(ResourceTable("LabelHeaderEndDate"), Nothing).Trim()
            LabelHeaderEndDate.Text = If(String.IsNullOrEmpty(LabelHeaderEndDate.Text), "End Date", LabelHeaderEndDate.Text)

            LabelHeaderPayor = DirectCast(CurrentRow.FindControl("LabelHeaderPayor"), Label)
            LabelHeaderPayor.Text = Convert.ToString(ResourceTable("LabelHeaderPayor"), Nothing).Trim()
            LabelHeaderPayor.Text = If(String.IsNullOrEmpty(LabelHeaderPayor.Text), "Payor", LabelHeaderPayor.Text)

            LabelHeaderServiceGroup = DirectCast(CurrentRow.FindControl("LabelHeaderServiceGroup"), Label)
            LabelHeaderServiceGroup.Text = Convert.ToString(ResourceTable("LabelHeaderServiceGroup"), Nothing).Trim()
            LabelHeaderServiceGroup.Text = If(String.IsNullOrEmpty(LabelHeaderServiceGroup.Text), "Service Group", LabelHeaderServiceGroup.Text)

            LabelHeaderServiceCode = DirectCast(CurrentRow.FindControl("LabelHeaderServiceCode"), Label)
            LabelHeaderServiceCode.Text = Convert.ToString(ResourceTable("LabelHeaderServiceCode"), Nothing).Trim()
            LabelHeaderServiceCode.Text = If(String.IsNullOrEmpty(LabelHeaderServiceCode.Text), "Service Code", LabelHeaderServiceCode.Text)

            LabelHeaderContractNumber = DirectCast(CurrentRow.FindControl("LabelHeaderContractNumber"), Label)
            LabelHeaderContractNumber.Text = Convert.ToString(ResourceTable("LabelHeaderContractNumber"), Nothing).Trim()
            LabelHeaderContractNumber.Text = If(String.IsNullOrEmpty(LabelHeaderContractNumber.Text), "Contract Number", LabelHeaderContractNumber.Text)

            LabelHeaderAction = DirectCast(CurrentRow.FindControl("LabelHeaderAction"), Label)
            LabelHeaderAction.Text = Convert.ToString(ResourceTable("LabelHeaderAction"), Nothing).Trim()
            LabelHeaderAction.Text = If(String.IsNullOrEmpty(LabelHeaderAction.Text), "Action", LabelHeaderAction.Text)

            LabelHeaderClient_Id = DirectCast(CurrentRow.FindControl("LabelHeaderClient_Id"), Label)
            LabelHeaderClient_Id.Text = Convert.ToString(ResourceTable("LabelHeaderClient_Id"), Nothing).Trim()
            LabelHeaderClient_Id.Text = If(String.IsNullOrEmpty(LabelHeaderClient_Id.Text), "Client_Id", LabelHeaderClient_Id.Text)

            LabelHeaderCompanyCode = DirectCast(CurrentRow.FindControl("LabelHeaderCompanyCode"), Label)
            LabelHeaderCompanyCode.Text = Convert.ToString(ResourceTable("LabelHeaderCompanyCode"), Nothing).Trim()
            LabelHeaderCompanyCode.Text = If(String.IsNullOrEmpty(LabelHeaderCompanyCode.Text), "Company Code", LabelHeaderCompanyCode.Text)

            LabelHeaderLocationCode = DirectCast(CurrentRow.FindControl("LabelHeaderLocationCode"), Label)
            LabelHeaderLocationCode.Text = Convert.ToString(ResourceTable("LabelHeaderLocationCode"), Nothing).Trim()
            LabelHeaderLocationCode.Text = If(String.IsNullOrEmpty(LabelHeaderLocationCode.Text), "Location Code", LabelHeaderLocationCode.Text)

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

            GridViewMEDsysClients.AutoGenerateColumns = False
            GridViewMEDsysClients.ShowHeaderWhenEmpty = True
            GridViewMEDsysClients.AllowPaging = True
            GridViewMEDsysClients.AllowSorting = True

            GridViewMEDsysClients.ShowFooter = True

            If (GridViewMEDsysClients.AllowPaging) Then
                GridViewMEDsysClients.PageSize = objShared.GridViewDefaultPageSize
            End If

            AddHandler GridViewMEDsysClients.RowDataBound, AddressOf GridViewMEDsysClients_RowDataBound
            AddHandler GridViewMEDsysClients.PageIndexChanging, AddressOf GridViewMEDsysClients_PageIndexChanging

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
                          & " var DeleteDialogHeader ='MEDsys Client'; " _
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
            BindGridViewMEDsysClients()
            ButtonViewError.Visible = False
        End Sub

        Private Sub GetData()
            GetClientData()
        End Sub

        Private Sub GetClientData()

            Dim objBLMEDsysClient As New BLMEDsysClient()

            Try
                Clients = objBLMEDsysClient.SelectMEDsysClientInfo(objShared.ConVisitel)
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to fetch MEDsys Client Data. Message: {0}", ex.Message))
            Finally
                objBLMEDsysClient = Nothing
            End Try
        End Sub

        Private Sub BindGridViewMEDsysClients()
            GridViewMEDsysClients.DataSource = Clients
            GridViewMEDsysClients.DataBind()

            ItemChecked = DetermineButtonInActivity(GridViewMEDsysClients, "CheckBoxSelect")

            ButtonSave.Enabled = ItemChecked
            ButtonDelete.Enabled = ButtonSave.Enabled
            ButtonIndividualDetail.Enabled = ButtonSave.Enabled
        End Sub

    End Class
End Namespace