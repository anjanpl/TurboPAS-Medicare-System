#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Client Type Setup
' Author: Anjan Kumar Paul
' Start Date: 27 Aug 2014
' End Date: 27 Aug 2014
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                27 Aug 2014     Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports VisitelBusiness.VisitelBusiness.DataObject.Settings
Imports VisitelBusiness.VisitelBusiness.Settings
Imports VisitelCommon.VisitelCommon
Imports System.Data.SqlClient
Imports VisitelBusiness.VisitelBusiness.DataObject

Namespace Visitel.UserControl.Settings

    Public Class ClientTypeSetting
        Inherits BaseUserControl

        Private CheckBoxSelect As CheckBox, chkAll As CheckBox
        Private ItemChecked As Boolean = False

        Private ControlName As String, ValidationGroup As String, SaveMessage As String, DeleteMessage As String, DeleteConfirmationMessage As String, ListFor As String

        Private EditText As String, DeleteText As String, InsertMessage As String, UpdateMessage As String, DuplicateNameMessage As String, EmptyNameMessage As String, _
            StatusSelectionMessage As String, ReceiverSelectionMessage As String, PayerSelectionMessage As String, InvalidContractNumberMessage As String, _
            InvalidUnitRateMessage As String, SaveConfirmMessage As String, DuplicateClientMessage As String

        Private LabelHeaderSerial As Label, LabelHeaderSelect As Label, LabelHeaderName As Label, LabelHeaderContractNo As Label, LabelHeaderUnitRate As Label, _
            LabelHeaderGroup As Label, LabelHeaderReceiverName As Label, LabelHeaderPayerName As Label, LabelHeaderStatus As Label, LabelHeaderSantrax As Label, _
            LabelHeaderUpdateDate As Label, LabelHeaderUpdateBy As Label, LabelIdNumber As Label, LabelRegionId As Label, LabelServiceTypeId As Label, _
            LabelClientGroupId As Label, LabelReceiverId As Label, LabelPayerId As Label, LabelStatusId As Label, LabelSantraxId As Label, _
            LabelHeaderCM As Label, LabelHeaderRegion As Label, LabelHeaderServiceType As Label, LabelHeaderProgramType As Label, LabelCM2000Id As Label

        Private TextBoxName As TextBox, TextBoxContractNumber As TextBox, TextBoxRegion As TextBox, TextBoxServiceType As TextBox, TextBoxProgramType As TextBox, _
            TextBoxUnitRate As TextBox, TextBoxClientGroupName As TextBox, TextBoxReceiverName As TextBox, _
            TextBoxPayerName As TextBox, TextBoxStatus As TextBox, TextBoxUpdateDate As TextBox, TextBoxUpdateBy As TextBox

        Private DropDownListRegion As DropDownList, DropDownListServiceType As DropDownList, DropDownListClientGroup As DropDownList, _
            DropDownListReceiver As DropDownList, DropDownListPayer As DropDownList, DropDownListStatus As DropDownList

        Private CheckBoxSantrax As CheckBox, CheckBoxCM As CheckBox

        Private ClientTypeList As List(Of ClientTypeSettingDataObject), ClientTypeErrorList As List(Of ClientTypeSettingDataObject)

        Private SaveFailedCounter As Int16, DeleteFailedCounter As Int16, RecordSaveCount As Int16

        Private ValidationEnable As Boolean
        Private CurrentRow As GridViewRow

        Private objShared As SharedWebControls

        Private UserId As Integer
        Private CompanyId As Integer
        Private ConVisitel As SqlConnection

        Protected Sub Page_Init(sender As Object, e As EventArgs)

            ControlName = "ClientTypeSetting"

            objShared = New SharedWebControls()

            UserId = objShared.UserId
            CompanyId = objShared.CompanyId

            objShared.ConnectionOpen()
            ConVisitel = objShared.ConVisitel

            InitializeControl()
            GetCaptionFromResource()

        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)
            If Not IsPostBack Then
                HiddenFieldIsNew.Value = Convert.ToString(True, Nothing)
                HiddenFieldIsSearched.Value = Convert.ToString(False, Nothing)
            End If

            GetData()

            If Not IsPostBack Then
                BindClientTypeInformationGridView()
            End If
        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadJavascript()
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing
            ClientTypeList = Nothing
        End Sub

        Protected Sub LinkButtonEdit_Click(sender As Object, e As EventArgs)
            Dim LinkButtonDelete As LinkButton = DirectCast(sender, LinkButton)
            Dim GridViewItemRow As GridViewRow = DirectCast(LinkButtonDelete.NamingContainer, GridViewRow)

            TextBoxName.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelName"), Label).Text, Nothing)

            Dim ContractNumber As String = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelContractNumber"), Label).Text, Nothing)
            TextBoxContractNumber.Text = objShared.GetReFormattedMobileNumber(Convert.ToString(ContractNumber, Nothing))

            Dim LabelUnitRate As String = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelUnitRate"), Label).Text, Nothing)
            TextBoxUnitRate.Text = LabelUnitRate

            DropDownListClientGroup.SelectedIndex = DropDownListClientGroup.Items.IndexOf(DropDownListClientGroup.Items.FindByValue(
                                                    Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelGroupId"), Label).Text, Nothing)))
            DropDownListReceiver.SelectedIndex = DropDownListReceiver.Items.IndexOf(DropDownListReceiver.Items.FindByValue(
                                                 Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelReceiverId"), Label).Text, Nothing)))
            DropDownListPayer.SelectedIndex = DropDownListPayer.Items.IndexOf(DropDownListPayer.Items.FindByValue(
                                              Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelPayerId"), Label).Text, Nothing)))

            CheckBoxSantrax.Checked = If(Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelSantrax"), Label).Text, Nothing).Equals("Y"), True, False)

            HiddenFieldIdNumber.Value = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelIdNumber"), Label).Text, Nothing)

            DropDownListStatus.SelectedIndex = DropDownListStatus.Items.IndexOf(DropDownListStatus.Items.FindByText(
                                               Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelStatus"), Label).Text, Nothing)))

            HiddenFieldIsNew.Value = Convert.ToString(False, Nothing)

            'ButtonSaveWithConfirmation.Visible = False
            'ButtonSave.Visible = True

        End Sub

        Protected Sub ButtonDelete_Click(sender As Object, e As EventArgs)
            Dim ButtonDelete As Button = DirectCast(sender, Button)
            Dim dtgItem As GridViewRow = DirectCast(ButtonDelete.NamingContainer, GridViewRow)
            Dim LabelIdNumber As Label = DirectCast(dtgItem.Cells(0).FindControl("LabelIdNumber"), Label)

            Dim objBLClientTypeSetting As New BLClientTypeSetting()

            Try
                objBLClientTypeSetting.DeleteClientTypeInfo(ConVisitel, Convert.ToInt32(LabelIdNumber.Text), UserId)
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DeleteMessage)
            Catch ex As Exception
                If (ex.Message.Contains("REFERENCE constraint")) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("Unable to delete Client Type Data. Message: {0}",
                                                                                                 "This Client Type is already used."))
                Else
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to delete Client Type Data. Message: {0}", ex.Message))
                End If
            Finally
                objBLClientTypeSetting = Nothing
            End Try

            GetClientTypeData()
            BindClientTypeInformationGridView()

            HiddenFieldIsNew.Value = Convert.ToString(False, Nothing)
            HiddenFieldIdNumber.Value = Convert.ToString(Int32.MinValue)
        End Sub

        Private Sub ButtonSave_Click(sender As Object, e As EventArgs)
            DataSaving(False)
        End Sub

        Private Sub ButtonSaveWithConfirmation_Click(sender As Object, e As EventArgs)
            DataSaving(True)
        End Sub

        Private Sub ButtonClear_Click(sender As Object, e As EventArgs)
            CurrentRow = GridViewClientTypeInformation.FooterRow
            ClearControl(CurrentRow)
        End Sub

        Private Sub ButtonSearch_Click(sender As Object, e As EventArgs)
            HiddenFieldIsSearched.Value = Convert.ToString(True, Nothing)
            GetClientTypeData()
            BindClientTypeInformationGridView()
            HiddenFieldIsSearched.Value = Convert.ToString(False, Nothing)
        End Sub

        Private Sub ButtonUpdateUnitRate_Click(sender As Object, e As EventArgs)

            If (String.IsNullOrEmpty(HiddenFieldContractName.Value)) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please enter contract name")
                Return
            End If

            If (String.IsNullOrEmpty(HiddenFieldContractNumber.Value)) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please enter contract number")
                Return
            End If

            Dim objBLClientTypeSetting As New BLClientTypeSetting()

            Try
                objBLClientTypeSetting.UpdateUnitRateInfo(ConVisitel, Convert.ToInt64(HiddenFieldIdNumber.Value), objShared.UserId)
                GetClientTypeData()
                BindClientTypeInformationGridView()
            Catch ex As Exception
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to update Unit Rate Information. Message:{0}", ex.Message))
            Finally
                objBLClientTypeSetting = Nothing
            End Try
        End Sub

        Private Sub ButtonUpdateServiceType_Click(sender As Object, e As EventArgs)

            Dim objBLClientTypeSetting As New BLClientTypeSetting()

            Try
                objBLClientTypeSetting.UpdateServiceTypeInfo(ConVisitel, Convert.ToInt64(HiddenFieldIdNumber.Value), objShared.UserId)
                GetClientTypeData()
                BindClientTypeInformationGridView()
            Catch ex As Exception
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to update Service Type Information. Message:{0}", ex.Message))
            Finally
                objBLClientTypeSetting = Nothing
            End Try
        End Sub


        Private Sub GridViewClientTypeInformation_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
            GridViewClientTypeInformation.PageIndex = e.NewPageIndex
            BindClientTypeInformationGridView()
        End Sub

        Private Sub GridViewClientTypeInformation_RowDataBound(sender As [Object], e As GridViewRowEventArgs)

            CurrentRow = DirectCast(e.Row, GridViewRow)

            If (CurrentRow.RowType.Equals(DataControlRowType.Header)) Then
                SetGridViewColumnHeaderText(CurrentRow)

                chkAll = DirectCast(CurrentRow.FindControl("chkAll"), CheckBox)
                chkAll.AutoPostBack = True
                chkAll.ClientIDMode = UI.ClientIDMode.Static
            End If

            If e.Row.RowType.Equals(DataControlRowType.DataRow) Then

                CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)
                CheckBoxSelect.AutoPostBack = True

                TextBoxContractNumber = DirectCast(CurrentRow.FindControl("TextBoxContractNumber"), TextBox)
                If (Not TextBoxContractNumber Is Nothing) Then
                    TextBoxContractNumber.ReadOnly = True
                    TextBoxContractNumber.TextMode = TextBoxMode.MultiLine
                End If

                TextBoxName = DirectCast(CurrentRow.FindControl("TextBoxName"), TextBox)
                If (Not TextBoxName Is Nothing) Then
                    TextBoxName.ReadOnly = True
                    TextBoxName.TextMode = TextBoxMode.MultiLine
                End If

                LabelRegionId = DirectCast(CurrentRow.FindControl("LabelRegionId"), Label)

                TextBoxRegion = DirectCast(CurrentRow.FindControl("TextBoxRegion"), TextBox)
                If ((Not TextBoxRegion Is Nothing) And (Not LabelRegionId Is Nothing)) Then
                    TextBoxRegion.ReadOnly = True
                    TextBoxRegion.TextMode = TextBoxMode.MultiLine
                End If

                DropDownListRegion = DirectCast(CurrentRow.FindControl("DropDownListRegion"), DropDownList)

                If (Not DropDownListRegion Is Nothing) Then
                    DropDownListRegion.Visible = False
                End If


                LabelServiceTypeId = DirectCast(CurrentRow.FindControl("LabelServiceTypeId"), Label)

                TextBoxServiceType = DirectCast(CurrentRow.FindControl("TextBoxServiceType"), TextBox)
                If ((Not TextBoxServiceType Is Nothing) And (Not LabelServiceTypeId Is Nothing)) Then
                    TextBoxServiceType.ReadOnly = True
                    TextBoxServiceType.TextMode = TextBoxMode.MultiLine
                End If

                DropDownListServiceType = DirectCast(CurrentRow.FindControl("DropDownListServiceType"), DropDownList)

                If (Not DropDownListServiceType Is Nothing) Then
                    DropDownListServiceType.Visible = False
                End If

                TextBoxUnitRate = DirectCast(CurrentRow.FindControl("TextBoxUnitRate"), TextBox)
                'TextBoxUnitRate.Text = Convert.ToString(Convert.ToDecimal(TextBoxUnitRate.Text, Nothing).ToString("0.##"), Nothing)
                'TextBoxUnitRate.TextMode = TextBoxMode.MultiLine
                'TextBoxUnitRate.ClientIDMode = ClientIDMode.Static
                'TextBoxUnitRate.Attributes.Add("onclick", "SetUnitRateMask('" & TextBoxUnitRate.ClientID & "')")

                LabelClientGroupId = DirectCast(CurrentRow.FindControl("LabelClientGroupId"), Label)

                TextBoxClientGroupName = DirectCast(CurrentRow.FindControl("TextBoxClientGroupName"), TextBox)
                If ((Not TextBoxClientGroupName Is Nothing) And (Not LabelClientGroupId Is Nothing)) Then
                    TextBoxClientGroupName.ReadOnly = True
                    TextBoxClientGroupName.TextMode = TextBoxMode.MultiLine
                End If

                DropDownListClientGroup = DirectCast(CurrentRow.FindControl("DropDownListClientGroup"), DropDownList)

                If (Not DropDownListClientGroup Is Nothing) Then
                    DropDownListClientGroup.Visible = False
                End If

                LabelReceiverId = DirectCast(CurrentRow.FindControl("LabelReceiverId"), Label)

                TextBoxReceiverName = DirectCast(CurrentRow.FindControl("TextBoxReceiverName"), TextBox)
                If ((Not TextBoxReceiverName Is Nothing) And (Not LabelReceiverId Is Nothing)) Then
                    TextBoxReceiverName.ReadOnly = True
                    TextBoxReceiverName.TextMode = TextBoxMode.MultiLine
                End If

                DropDownListReceiver = DirectCast(CurrentRow.FindControl("DropDownListReceiver"), DropDownList)

                If (Not DropDownListReceiver Is Nothing) Then
                    DropDownListReceiver.Visible = False
                End If

                LabelPayerId = DirectCast(CurrentRow.FindControl("LabelPayerId"), Label)

                TextBoxPayerName = DirectCast(CurrentRow.FindControl("TextBoxPayerName"), TextBox)
                If ((Not TextBoxPayerName Is Nothing) And (Not LabelPayerId Is Nothing)) Then
                    TextBoxPayerName.ReadOnly = True
                    TextBoxPayerName.TextMode = TextBoxMode.MultiLine
                End If

                DropDownListPayer = DirectCast(CurrentRow.FindControl("DropDownListPayer"), DropDownList)

                If (Not DropDownListPayer Is Nothing) Then
                    DropDownListPayer.Visible = False
                End If


                LabelStatusId = DirectCast(CurrentRow.FindControl("LabelStatusId"), Label)

                DropDownListStatus = DirectCast(CurrentRow.FindControl("DropDownListStatus"), DropDownList)

                If (Not DropDownListStatus Is Nothing) Then
                    DropDownListStatus.Visible = False
                End If

                TextBoxStatus = DirectCast(CurrentRow.FindControl("TextBoxStatus"), TextBox)
                TextBoxStatus.Text = [Enum].GetName(GetType(EnumDataObject.STATUS), Convert.ToInt16(TextBoxStatus.Text))
                TextBoxStatus.ReadOnly = True

                LabelSantraxId = DirectCast(CurrentRow.FindControl("LabelSantraxId"), Label)
                CheckBoxSantrax = DirectCast(CurrentRow.FindControl("CheckBoxSantrax"), CheckBox)
                CheckBoxSantrax.Checked = IIf(LabelSantraxId.Text.Trim().Equals("1"), True, False)

                LabelCM2000Id = DirectCast(CurrentRow.FindControl("LabelCM2000Id"), Label)
                CheckBoxCM = DirectCast(CurrentRow.FindControl("CheckBoxCM"), CheckBox)
                CheckBoxCM.Checked = IIf(LabelCM2000Id.Text.Trim().Equals("1"), True, False)

                TextBoxUpdateDate = DirectCast(CurrentRow.FindControl("TextBoxUpdateDate"), TextBox)
                TextBoxUpdateDate.ReadOnly = True

                TextBoxUpdateBy = DirectCast(CurrentRow.FindControl("TextBoxUpdateBy"), TextBox)
                TextBoxUpdateBy.ReadOnly = True

                Dim LinkButtonEdit As LinkButton = DirectCast(CurrentRow.FindControl("LinkButtonEdit"), LinkButton)
                LinkButtonEdit.Text = EditText
                LinkButtonEdit.CssClass = "ui-state-default ui-corner-all buttonLink"

                Dim ButtonDelete As Button = DirectCast(e.Row.Cells(0).FindControl("ButtonDelete"), Button)
                ButtonDelete.Text = DeleteText

                '**************************Fill Out Drop Down and Associate selection on Edit Mode[Start]***********************************
                If ((e.Row.RowState & DataControlRowState.Edit) > 0) Then

                    BindRegionDropDownListFromGrid(CurrentRow)

                    BindDropDownListServiceTypeFromGrid(CurrentRow)

                    BindDropDownListClientGroupFromGrid(CurrentRow)

                    BindDropDownListReceiverFromGrid(CurrentRow)

                    BindDropDownListPayerFromGrid(CurrentRow)

                    BindDropDownListStatusFromGrid(CurrentRow)
                End If
                '**************************Fill Out Drop Down and Associate selection on Edit Mode[End]***********************************

            End If

            If (CurrentRow.RowType.Equals(DataControlRowType.Footer)) Then

                CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)
                CheckBoxSelect.AutoPostBack = True

                BindRegionDropDownListFromGrid(CurrentRow)

                BindDropDownListServiceTypeFromGrid(CurrentRow)

                BindDropDownListClientGroupFromGrid(CurrentRow)

                BindDropDownListReceiverFromGrid(CurrentRow)

                BindDropDownListPayerFromGrid(CurrentRow)

                BindDropDownListStatusFromGrid(CurrentRow)

                TextBoxUpdateDate = DirectCast(CurrentRow.FindControl("TextBoxUpdateDate"), TextBox)
                TextBoxUpdateDate.ReadOnly = True

                TextBoxUpdateBy = DirectCast(CurrentRow.FindControl("TextBoxUpdateBy"), TextBox)
                TextBoxUpdateBy.ReadOnly = True

            End If
        End Sub

        Protected Sub OnCheckedChanged(sender As Object, e As EventArgs)
            Dim isUpdateVisible As Boolean = False

            Dim chk As CheckBox = TryCast(sender, CheckBox)

            'ButtonSave.Enabled = If((chk.Checked), True, False)

            If chk.ID = "chkAll" Then
                For Each row As GridViewRow In GridViewClientTypeInformation.Rows
                    If row.RowType = DataControlRowType.DataRow Then
                        row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked = chk.Checked
                    End If
                Next

                CurrentRow = GridViewClientTypeInformation.FooterRow
                CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)
                CheckBoxSelect.Checked = chk.Checked

            End If

            Dim chkAll As CheckBox = TryCast(GridViewClientTypeInformation.HeaderRow.FindControl("chkAll"), CheckBox)
            'chkAll.Checked = True
            For Each row As GridViewRow In GridViewClientTypeInformation.Rows
                If row.RowType = DataControlRowType.DataRow Then

                    Dim isChecked As Boolean = row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked

                    ControlsOnSelect(row, isChecked)

                    'If (isChecked) Then
                    '    LabelClientId = DirectCast(row.FindControl("LabelClientId"), Label)
                    '    If ((Not HiddenFieldClientId Is Nothing) And (Not LabelClientId Is Nothing)) Then
                    '        HiddenFieldClientId.Value = LabelClientId.Text.Trim()
                    '    End If

                    '    LabelEmployeeId = DirectCast(row.FindControl("LabelEmployeeId"), Label)
                    '    If ((Not HiddenFieldEmployeeId Is Nothing) And (Not LabelEmployeeId Is Nothing)) Then
                    '        HiddenFieldEmployeeId.Value = LabelEmployeeId.Text.Trim()
                    '    End If
                    'End If

                    For i As Integer = 1 To row.Cells.Count - 1
                        If row.Cells(i).Controls.OfType(Of DropDownList)().ToList().Count > 0 Then
                            row.Cells(i).Controls.OfType(Of DropDownList)().FirstOrDefault().Visible = isChecked

                            If (Not DropDownListRegion Is Nothing) Then
                                DropDownListRegion.CssClass = If((DropDownListRegion.Visible), "DropDownListRegionEdit", "DropDownListRegion")
                            End If

                            If (Not DropDownListServiceType Is Nothing) Then
                                DropDownListServiceType.CssClass = If((DropDownListServiceType.Visible), "DropDownListServiceTypeEdit", "DropDownListServiceType")
                            End If

                            If (Not DropDownListClientGroup Is Nothing) Then
                                DropDownListClientGroup.CssClass = If((DropDownListClientGroup.Visible), "DropDownListClientGroupEdit", "DropDownListClientGroup")
                            End If

                            If (Not DropDownListPayer Is Nothing) Then
                                DropDownListPayer.CssClass = If((DropDownListPayer.Visible), "DropDownListPayerEdit", "DropDownListPayer")
                            End If

                            If (Not DropDownListReceiver Is Nothing) Then
                                DropDownListReceiver.CssClass = If((DropDownListReceiver.Visible), "DropDownListReceiverEdit", "DropDownListReceiver")
                            End If

                            If (Not DropDownListStatus Is Nothing) Then
                                DropDownListStatus.CssClass = If((DropDownListStatus.Visible), "DropDownListStatusEdit", "DropDownListStatus")
                            End If
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

            ItemChecked = DetermineButtonInActivity(GridViewClientTypeInformation, "CheckBoxSelect")

            ButtonSave.Enabled = ItemChecked
            ButtonUpdateUnitRate.Enabled = ButtonSave.Enabled
            ButtonUpdateServiceType.Enabled = ButtonSave.Enabled
        End Sub

        ''' <summary>
        ''' Gridview Row Hover Color Set
        ''' </summary>
        ''' <param name="writer"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub Render(writer As HtmlTextWriter)
            For Each r As GridViewRow In GridViewClientTypeInformation.Rows
                If r.RowType = DataControlRowType.DataRow Then
                    objShared.SetGridViewRowColor(r)
                End If
            Next
            MyBase.Render(writer)
        End Sub

        ''' <summary>
        ''' This is for control events regristering and instantiate object globally
        ''' </summary>
        Private Sub InitializeControl()

            AddHandler ButtonViewError.Click, AddressOf ButtonViewError_Click
            ButtonViewError.Visible = False

            AddHandler ButtonSave.Click, AddressOf ButtonSave_Click
            AddHandler ButtonSaveWithConfirmation.Click, AddressOf ButtonSaveWithConfirmation_Click
            AddHandler ButtonClear.Click, AddressOf ButtonClear_Click
            AddHandler ButtonSearch.Click, AddressOf ButtonSearch_Click
            AddHandler ButtonUpdateUnitRate.Click, AddressOf ButtonUpdateUnitRate_Click
            AddHandler ButtonUpdateServiceType.Click, AddressOf ButtonUpdateServiceType_Click

            GridViewClientTypeInformation.AutoGenerateColumns = False
            GridViewClientTypeInformation.ShowHeaderWhenEmpty = True
            GridViewClientTypeInformation.AllowPaging = True
            GridViewClientTypeInformation.AllowSorting = True

            GridViewClientTypeInformation.ShowFooter = True

            If (GridViewClientTypeInformation.AllowPaging) Then
                GridViewClientTypeInformation.PageSize = objShared.GridViewDefaultPageSize
            End If

            AddHandler GridViewClientTypeInformation.PageIndexChanging, AddressOf GridViewClientTypeInformation_PageIndexChanging
            AddHandler GridViewClientTypeInformation.RowDataBound, AddressOf GridViewClientTypeInformation_RowDataBound

            'ButtonSaveWithConfirmation.Visible = False

            HiddenFieldContractName.ClientIDMode = ClientIDMode.Static
            HiddenFieldContractNumber.ClientIDMode = ClientIDMode.Static
            HiddenFieldUnitRate.ClientIDMode = ClientIDMode.Static
            HiddenFieldIsDuplicate.ClientIDMode = ClientIDMode.Static

            ButtonSave.ClientIDMode = ClientIDMode.Static
            ButtonSaveWithConfirmation.ClientIDMode = ClientIDMode.Static
            ButtonSearch.ClientIDMode = ClientIDMode.Static
            ButtonClear.ClientIDMode = ClientIDMode.Static
            ButtonUpdateUnitRate.ClientIDMode = ClientIDMode.Static
            ButtonUpdateServiceType.ClientIDMode = ClientIDMode.Static

            TextBoxSearchByContractNumber.ClientIDMode = ClientIDMode.Static
        End Sub

        Private Sub DataSaving(IsConfirm As Boolean)
            Page.Validate()
            Page.Validate(ValidationGroup)

            ClientTypeList = New List(Of ClientTypeSettingDataObject)()
            ClientTypeErrorList = New List(Of ClientTypeSettingDataObject)()

            SaveFailedCounter = 0
            RecordSaveCount = 0

            CurrentRow = GridViewClientTypeInformation.FooterRow

            CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)

            LabelIdNumber = DirectCast(CurrentRow.FindControl("LabelIdNumber"), Label)


            If ((CheckBoxSelect.Checked) And (String.IsNullOrEmpty(LabelIdNumber.Text.Trim()))) Then
                SaveData(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT), IsConfirm)
            End If

            SaveData(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE), IsConfirm)

            If (SaveFailedCounter > 0) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} Records Failed to Save out of {1}", SaveFailedCounter, ClientTypeList.Count))
                ViewState("SaveFailedRecord") = objShared.ToDataTable(ClientTypeErrorList)
                ViewState("ListFor") = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)
            ElseIf (SaveFailedCounter = 0 And HiddenFieldIsDuplicate.Value.Equals("1")) Then

            Else
                ViewState("SaveFailedRecord") = Nothing

                If (RecordSaveCount > 0) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} record(s) {1}.", RecordSaveCount, SaveMessage))
                    ViewState.Clear()
                    GetClientTypeData()
                    BindClientTypeInformationGridView()
                End If

            End If

            ButtonViewError.Visible = (SaveFailedCounter > 0)

            ClientTypeList = Nothing
            ClientTypeErrorList = Nothing

            HiddenFieldIsNew.Value = Convert.ToString(True, Nothing)
            HiddenFieldIdNumber.Value = Convert.ToString(Int32.MinValue)
        End Sub

        ''' <summary>
        ''' Gridview row selection and go for edit mode or record deletion
        ''' </summary>
        ''' <param name="CurrentRow"></param>
        ''' <param name="isChecked"></param>
        ''' <remarks></remarks>
        Private Sub ControlsOnSelect(ByRef CurrentRow As GridViewRow, ByRef isChecked As Boolean)
            ControlsFind(CurrentRow)

            If (Not TextBoxName Is Nothing) Then
                TextBoxName.ReadOnly = Not isChecked
                TextBoxName.CssClass = If((TextBoxName.ReadOnly), "TextBoxName", "TextBoxNameEdit")
            End If

            If (Not TextBoxContractNumber Is Nothing) Then
                TextBoxContractNumber.ReadOnly = Not isChecked
                TextBoxContractNumber.CssClass = If((TextBoxContractNumber.ReadOnly), "TextBoxContractNumber", "TextBoxContractNumberEdit")
            End If

            If (Not TextBoxRegion Is Nothing) Then
                TextBoxRegion.Visible = Not isChecked
                TextBoxRegion.CssClass = If((TextBoxRegion.ReadOnly), "TextBoxRegion", "TextBoxRegionEdit")
            End If

            If (Not TextBoxServiceType Is Nothing) Then
                TextBoxServiceType.Visible = Not isChecked
                TextBoxServiceType.CssClass = If((TextBoxServiceType.ReadOnly), "TextBoxServiceType", "TextBoxServiceTypeEdit")
            End If

            If (Not TextBoxProgramType Is Nothing) Then
                TextBoxProgramType.ReadOnly = Not isChecked
                TextBoxProgramType.CssClass = If((TextBoxProgramType.ReadOnly), "TextBoxProgramType", "TextBoxProgramTypeEdit")
            End If

            If (Not TextBoxUnitRate Is Nothing) Then
                TextBoxUnitRate.ReadOnly = Not isChecked
                TextBoxUnitRate.CssClass = If((TextBoxUnitRate.ReadOnly), "TextBoxUnitRate", "TextBoxUnitRateEdit")
            End If

            If (Not TextBoxClientGroupName Is Nothing) Then
                TextBoxClientGroupName.Visible = Not isChecked
                TextBoxClientGroupName.CssClass = If((TextBoxClientGroupName.ReadOnly), "TextBoxClientGroupName", "TextBoxClientGroupNameEdit")
            End If

            If (Not TextBoxReceiverName Is Nothing) Then
                TextBoxReceiverName.Visible = Not isChecked
                TextBoxReceiverName.CssClass = If((TextBoxReceiverName.ReadOnly), "TextBoxReceiverName", "TextBoxReceiverNameEdit")
            End If

            If (Not TextBoxPayerName Is Nothing) Then
                TextBoxPayerName.Visible = Not isChecked
                TextBoxPayerName.CssClass = If((TextBoxPayerName.ReadOnly), "TextBoxPayerName", "TextBoxPayerNameEdit")
            End If

            If (Not TextBoxStatus Is Nothing) Then
                TextBoxStatus.Visible = Not isChecked
                TextBoxStatus.CssClass = If((TextBoxStatus.ReadOnly), "TextBoxStatus", "TextBoxStatusEdit")
            End If

            If (isChecked) Then
                SetHiddenFieldValue()
            End If
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

        Private Sub LoadJavascript()

            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                                  & " var prm =''; " _
                                  & " jQuery(document).ready(function () {" _
                                  & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                                  & "     SetUnitRateMask();" _
                                  & "     prm.add_endRequest(SetUnitRateMask); " _
                                  & "}); " _
                                  & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))


            scriptBlock = "<script type='text/javascript'> " _
                              & " var CustomTargetButton ='ButtonSaveWithConfirmation'; " _
                              & " var CustomDialogHeader ='Client Type: Save Forcefully'; " _
                              & " var CustomDialogConfirmMsg ='" & SaveConfirmMessage & "'; " _
                              & " var prm =''; " _
                              & " jQuery(document).ready(function () {" _
                              & "   jQuery('#ButtonSaveWithConfirmation').hide(); " _
                              & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                              & "     ClientTypeSave();" _
                              & "     prm.add_endRequest(ClientTypeSave); " _
                              & "}); " _
                              & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))


            'scriptBlock = "<script type='text/javascript'> " _
            '                 & " var DeleteDialogHeader ='Client Type'; " _
            '                 & " var DeleteDialogConfirmMsg ='Do you want to delete this record?'; " _
            '                 & " var LoaderImagePath ='" & objShared.GetLoaderImagePath & "'; " _
            '                 & " var prm =''; " _
            '                 & " jQuery(document).ready(function () {" _
            '                 & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
            '                 & "     prm.add_beginRequest(SetButtonActionProgress); " _
            '                 & "     prm.add_endRequest(EndRequest); " _
            '                 & "}); " _
            '                 & "</script>"

            scriptBlock = "<script type='text/javascript'> " _
                             & " var DeleteDialogHeader ='Client Type'; " _
                             & " var DeleteDialogConfirmMsg ='Do you want to delete this record?'; " _
                             & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))


            scriptBlock = "<script type='text/javascript'> " _
                         & " var CustomTargetButton1 ='ButtonUpdateUnitRate'; " _
                         & " var CustomDialogHeader1 ='Client Type: Update Unit Rate'; " _
                         & " var CustomDialogConfirmMsg1 ='Clicking OK will set all active " & Convert.ToString(HiddenFieldContractName.Value, Nothing) _
                         & " individuals to a unit rate of $" & Convert.ToString(HiddenFieldUnitRate.Value, Nothing) & ". Are you sure you want to do this?'; " _
                         & " var prm =''; " _
                         & " jQuery(document).ready(function () {" _
                         & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                         & "     UpdateUnitRate();" _
                         & "     prm.add_endRequest(UpdateUnitRate); " _
                         & "     prm.add_endRequest(EndRequest); " _
                         & "}); " _
                         & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

            scriptBlock = "<script type='text/javascript'> " _
                       & " var CustomTargetButton2 ='ButtonUpdateServiceType'; " _
                       & " var CustomDialogHeader2 ='Client Type: Update Service Type'; " _
                       & " var CustomDialogConfirmMsg2 ='Clicking OK will set or reset the Program/Service Types of all clients. " _
                       & " Are you sure you want to do this?'; " _
                       & " var prm =''; " _
                       & " jQuery(document).ready(function () {" _
                       & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                       & "     UpdateServiceType();" _
                       & "     prm.add_endRequest(UpdateServiceType); " _
                       & "}); " _
                       & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

            LoadJS("JavaScript/Settings/" & ControlName & ".js")

        End Sub

        ''' <summary>
        ''' Saving Data either insert or update after making list
        ''' </summary>
        ''' <param name="Action"></param>
        ''' <remarks></remarks>
        Private Sub SaveData(Action As String, IsConfirm As Boolean)

            MakeList(Action)

            If (ClientTypeList.Count.Equals(0)) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please select items to save")
                Return
            End If

            TakeAction(Action, IsConfirm)

        End Sub

        ''' <summary>
        ''' Make Database operation either for record saving or deleting
        ''' </summary>
        ''' <param name="Action"></param>
        ''' <remarks></remarks>
        Private Sub TakeAction(Action As String, IsConfirm As Boolean)

            Dim objBLClientTypeSetting As New BLClientTypeSetting()

            For Each ClientType In ClientTypeList
                Try
                    Select Case Action
                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT)
                            objBLClientTypeSetting.InsertClientTypeInfo(ConVisitel, ClientType, IsConfirm)
                            HiddenFieldIsDuplicate.Value = ""
                            Exit Select

                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE)
                            objBLClientTypeSetting.UpdateClientTypeInfo(ConVisitel, ClientType, IsConfirm)
                            HiddenFieldIsDuplicate.Value = ""
                            Exit Select
                            'Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE)
                            '    objBLClientTypeSetting.DeleteMEDsysScheduleInfo(objShared.ConVisitel, MEDsysSchedule.Id, MEDsysSchedule.UpdateBy, IsLog)
                            '    Exit Select
                    End Select

                Catch ex As SqlException

                    Select Case Action
                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT)
                            If ex.Message.Contains("Duplicate Client Code") Then
                                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0}, Client Name: {1}, Contract No.: {2}",
                                                                                                             DuplicateClientMessage, ClientType.Name, ClientType.ContractNo))

                                HiddenFieldIsDuplicate.Value = "1"

                                'ButtonSaveWithConfirmation.Visible = True
                                'ButtonSave.Visible = False

                                'Dim scriptBlock As String = Nothing

                                'scriptBlock = "<script type='text/javascript'> " _
                                '  & " var CustomTargetButton ='ButtonSaveWithConfirmation'; " _
                                '  & " var CustomDialogHeader ='Client Type: Save Forcefully'; " _
                                '  & " var CustomDialogConfirmMsg ='" & SaveConfirmMessage & "'; " _
                                '  & " var prm =''; " _
                                '  & " jQuery(document).ready(function () {" _
                                '  & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                                '  & "     ClientTypeSave();" _
                                '  & "     prm.add_endRequest(ClientTypeSave); " _
                                '  & "}); " _
                                '  & "</script>"

                                'Page.Header.Controls.Add(New LiteralControl(scriptBlock))

                                Return
                            End If
                            SaveFailedCounter = SaveFailedCounter + 1
                            Exit Select

                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE)
                            If ex.Message.Contains("Duplicate Client Code") Then
                                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0}, Client Name: {1}, Contract No.: {2}",
                                                                                                             DuplicateClientMessage, ClientType.Name, ClientType.ContractNo))

                                HiddenFieldIsDuplicate.Value = "1"

                                'ButtonSaveWithConfirmation.Visible = True
                                'ButtonSave.Visible = False

                                'Page.ClientScript.RegisterClientScriptBlock(Me.[GetType](), "script", "alert('Success!');", True)

                                'Dim scriptBlock As String = Nothing

                                'scriptBlock = "<script type='text/javascript'> " _
                                '  & " var CustomTargetButton ='ButtonSaveWithConfirmation'; " _
                                '  & " var CustomDialogHeader ='Client Type: Save Forcefully'; " _
                                '  & " var CustomDialogConfirmMsg ='" & SaveConfirmMessage & "'; " _
                                '  & " var prm =''; " _
                                '  & " jQuery(document).ready(function () {" _
                                '  & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                                '  & "     ClientTypeSave();" _
                                '  & "     prm.add_endRequest(ClientTypeSave); " _
                                '  & "}); " _
                                '  & "</script>"

                                'Page.Header.Controls.Add(New LiteralControl(scriptBlock))

                                Return
                            End If
                            SaveFailedCounter = SaveFailedCounter + 1
                            Exit Select

                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE)
                            DeleteFailedCounter = DeleteFailedCounter + 1
                            Exit Select

                    End Select

                    ClientType.Remarks = ex.Message
                    ClientTypeErrorList.Add(ClientType)

                Finally

                End Try
            Next

            objBLClientTypeSetting = Nothing

            HiddenFieldIsNew.Value = Convert.ToString(True, Nothing)
            HiddenFieldIdNumber.Value = Convert.ToString(Int32.MinValue)

        End Sub

        ''' <summary>
        ''' Making a List of Record Set Either for Save or Delete
        ''' </summary>
        ''' <param name="Action"></param>
        ''' <remarks></remarks>
        Private Sub MakeList(Action As String)

            Dim objClientTypeSettingDataObject As ClientTypeSettingDataObject

            Select Case Action
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT)
                    'LabelClientGroupId = DirectCast(GridViewClientTypeInformation.FooterRow.FindControl("LabelClientGroupId"), Label)

                    'DropDownListClientGroup = DirectCast(GridViewClientTypeInformation.FooterRow.FindControl("DropDownListClientGroup"), DropDownList)
                    'If (DropDownListClientGroup.SelectedIndex < 1) Then
                    '    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please select one Client Group")
                    '    Return
                    'End If

                    objClientTypeSettingDataObject = New ClientTypeSettingDataObject()
                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)

                    AddToList(CurrentRow, ClientTypeList, objClientTypeSettingDataObject)
                    RecordSaveCount = RecordSaveCount + 1
                    objClientTypeSettingDataObject = Nothing

                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE),
                    EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE)
                    ClientTypeList.Clear()
                    For Each row As GridViewRow In GridViewClientTypeInformation.Rows
                        If (row.RowType.Equals(DataControlRowType.DataRow)) Then
                            Dim isChecked As Boolean = row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked

                            If (isChecked) Then

                                objClientTypeSettingDataObject = New ClientTypeSettingDataObject()

                                CurrentRow = DirectCast(GridViewClientTypeInformation.Rows(row.RowIndex), GridViewRow)

                                'LabelClientGroupId = DirectCast(GridViewClientTypeInformation.FooterRow.FindControl("LabelClientGroupId"), Label)

                                'DropDownListClientGroup = DirectCast(GridViewClientTypeInformation.FooterRow.FindControl("DropDownListClientGroup"), DropDownList)
                                'If (DropDownListClientGroup.SelectedIndex < 1) Then
                                '    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please select one Client Group")
                                '    Return
                                'End If

                                If (Action.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE))) Then
                                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)
                                End If

                                If (Action.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE))) Then
                                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE)
                                End If

                                AddToList(CurrentRow, ClientTypeList, objClientTypeSettingDataObject)
                                RecordSaveCount = RecordSaveCount + 1
                                objClientTypeSettingDataObject = Nothing

                            End If

                        End If
                    Next

                    Exit Select
            End Select

            objClientTypeSettingDataObject = Nothing

        End Sub

        ''' <summary>
        ''' Making List in order to save records or delete
        ''' </summary>
        ''' <param name="CurrentRow"></param>
        ''' <param name="ClientTypeList"></param>
        ''' <param name="objClientTypeSettingDataObject"></param>
        ''' <remarks></remarks>
        Private Sub AddToList(ByRef CurrentRow As GridViewRow, ByRef ClientTypeList As List(Of ClientTypeSettingDataObject),
                              ByRef objClientTypeSettingDataObject As ClientTypeSettingDataObject)

            Dim Int32Result As Int32

            LabelIdNumber = DirectCast(CurrentRow.FindControl("LabelIdNumber"), Label)

            If (ListFor.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE))) Then
                ControlsFind(CurrentRow)
            End If

            If (Page.IsValid) Then
                If ValidateData() Then
                    objClientTypeSettingDataObject.IdNumber = If(Int32.TryParse(LabelIdNumber.Text.Trim(), Int32Result), Int32Result, objClientTypeSettingDataObject.IdNumber)

                    If (ListFor.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE))) Then
                        objClientTypeSettingDataObject.Name = Convert.ToString(TextBoxName.Text, Nothing).Trim()
                        objClientTypeSettingDataObject.ContractNo = Convert.ToString(TextBoxContractNumber.Text, Nothing).Trim()
                        objClientTypeSettingDataObject.Region = Convert.ToString(DropDownListRegion.SelectedValue, Nothing)
                        objClientTypeSettingDataObject.ServiceTypeId = Convert.ToInt32(DropDownListServiceType.SelectedValue)
                        objClientTypeSettingDataObject.ProgramType = Convert.ToString(TextBoxProgramType.Text, Nothing).Trim()
                        objClientTypeSettingDataObject.Status = Convert.ToInt16(DropDownListStatus.SelectedValue)
                        objClientTypeSettingDataObject.ClientGroupId = Convert.ToString(DropDownListClientGroup.SelectedValue, Nothing).Trim()
                        objClientTypeSettingDataObject.ReceiverId = Convert.ToInt32(DropDownListReceiver.SelectedValue)
                        objClientTypeSettingDataObject.PayerId = Convert.ToInt32(DropDownListPayer.SelectedValue)
                        objClientTypeSettingDataObject.UnitRate = Convert.ToDecimal(TextBoxUnitRate.Text, Nothing)
                        objClientTypeSettingDataObject.SantraxYN = IIf(CheckBoxSantrax.Checked, 1, 0)
                        objClientTypeSettingDataObject.CM2000YN = IIf(CheckBoxCM.Checked, 1, 0)
                    End If

                    objClientTypeSettingDataObject.UpdateBy = objShared.UserId
                    objClientTypeSettingDataObject.CompanyId = objShared.CompanyId
                    ClientTypeList.Add(objClientTypeSettingDataObject)
                End If
            End If
        End Sub

        Private Sub ControlsFind(ByRef CurrentRow As GridViewRow)
            LabelIdNumber = DirectCast(CurrentRow.FindControl("LabelIdNumber"), Label)
            TextBoxName = DirectCast(CurrentRow.FindControl("TextBoxName"), TextBox)
            TextBoxContractNumber = DirectCast(CurrentRow.FindControl("TextBoxContractNumber"), TextBox)
            TextBoxUnitRate = DirectCast(CurrentRow.FindControl("TextBoxUnitRate"), TextBox)

            If (Not CurrentRow.RowType.Equals(DataControlRowType.Footer)) Then
                TextBoxRegion = DirectCast(CurrentRow.FindControl("TextBoxRegion"), TextBox)
            End If

            DropDownListRegion = DirectCast(CurrentRow.FindControl("DropDownListRegion"), DropDownList)

            If (Not CurrentRow.RowType.Equals(DataControlRowType.Footer)) Then
                TextBoxServiceType = DirectCast(CurrentRow.FindControl("TextBoxServiceType"), TextBox)
            End If

            DropDownListServiceType = DirectCast(CurrentRow.FindControl("DropDownListServiceType"), DropDownList)

            TextBoxProgramType = DirectCast(CurrentRow.FindControl("TextBoxProgramType"), TextBox)

            If (Not CurrentRow.RowType.Equals(DataControlRowType.Footer)) Then
                TextBoxClientGroupName = DirectCast(CurrentRow.FindControl("TextBoxClientGroupName"), TextBox)
            End If

            DropDownListClientGroup = DirectCast(CurrentRow.FindControl("DropDownListClientGroup"), DropDownList)

            If (Not CurrentRow.RowType.Equals(DataControlRowType.Footer)) Then
                TextBoxReceiverName = DirectCast(CurrentRow.FindControl("TextBoxReceiverName"), TextBox)
            End If

            DropDownListReceiver = DirectCast(CurrentRow.FindControl("DropDownListReceiver"), DropDownList)

            If (Not CurrentRow.RowType.Equals(DataControlRowType.Footer)) Then
                TextBoxPayerName = DirectCast(CurrentRow.FindControl("TextBoxPayerName"), TextBox)
            End If

            DropDownListPayer = DirectCast(CurrentRow.FindControl("DropDownListPayer"), DropDownList)

            If (Not CurrentRow.RowType.Equals(DataControlRowType.Footer)) Then
                TextBoxStatus = DirectCast(CurrentRow.FindControl("TextBoxStatus"), TextBox)
            End If

            DropDownListStatus = DirectCast(CurrentRow.FindControl("DropDownListStatus"), DropDownList)

            CheckBoxSantrax = DirectCast(CurrentRow.FindControl("CheckBoxSantrax"), CheckBox)

            CheckBoxCM = DirectCast(CurrentRow.FindControl("CheckBoxCM"), CheckBox)

        End Sub

        Private Sub SetHiddenFieldValue()
            HiddenFieldIdNumber.Value = LabelIdNumber.Text.Trim()
            HiddenFieldContractName.Value = TextBoxName.Text.Trim()
            HiddenFieldContractNumber.Value = TextBoxContractNumber.Text.Trim()
            HiddenFieldUnitRate.Value = TextBoxUnitRate.Text.Trim()
        End Sub

        Private Sub SaveData(IsConfirm As Boolean)

            If ValidateData() Then
                Dim objClientTypeSettingDataObject As New ClientTypeSettingDataObject()

                objClientTypeSettingDataObject.Name = Convert.ToString(TextBoxName.Text, Nothing).Trim()
                objClientTypeSettingDataObject.ContractNo = Convert.ToString(TextBoxContractNumber.Text, Nothing).Trim()
                objClientTypeSettingDataObject.Status = Convert.ToInt16(DropDownListStatus.SelectedValue)
                objClientTypeSettingDataObject.ClientGroupId = Convert.ToString(DropDownListClientGroup.SelectedValue, Nothing).Trim()
                objClientTypeSettingDataObject.ReceiverId = Convert.ToInt32(DropDownListReceiver.SelectedValue)
                objClientTypeSettingDataObject.PayerId = Convert.ToInt32(DropDownListPayer.SelectedValue)
                objClientTypeSettingDataObject.UnitRate = Convert.ToDecimal(TextBoxUnitRate.Text, Nothing)
                objClientTypeSettingDataObject.SantraxYN = IIf(CheckBoxSantrax.Checked, 1, 0)


                Dim objBLClientTypeSetting As New BLClientTypeSetting()

                Select Case Convert.ToBoolean(HiddenFieldIsNew.Value, Nothing)
                    Case True
                        objClientTypeSettingDataObject.UserId = UserId
                        objClientTypeSettingDataObject.CompanyId = CompanyId
                        Try
                            objBLClientTypeSetting.InsertClientTypeInfo(ConVisitel, objClientTypeSettingDataObject, IsConfirm)

                        Catch ex As SqlException

                            If ex.Message.Contains("Duplicate Client Code") Then
                                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DuplicateClientMessage)
                                'ButtonSaveWithConfirmation.Visible = True
                                'ButtonSave.Visible = False

                                Dim scriptBlock As String = Nothing

                                scriptBlock = "<script type='text/javascript'> " _
                                  & " var CustomTargetButton ='ButtonSaveWithConfirmation'; " _
                                  & " var CustomDialogHeader ='Client Type: Save Forcefully'; " _
                                  & " var CustomDialogConfirmMsg ='" & SaveConfirmMessage & "'; " _
                                  & " var prm =''; " _
                                  & " jQuery(document).ready(function () {" _
                                  & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                                  & "     ClientTypeSave();" _
                                  & "     prm.add_endRequest(ClientTypeSave); " _
                                  & "}); " _
                                  & "</script>"

                                Page.Header.Controls.Add(New LiteralControl(scriptBlock))

                                Return
                            End If

                        End Try
                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(InsertMessage)
                        Exit Select
                    Case False
                        objClientTypeSettingDataObject.IdNumber = Convert.ToInt32(HiddenFieldIdNumber.Value)
                        objClientTypeSettingDataObject.UpdateBy = Convert.ToString(UserId)
                        Try
                            objBLClientTypeSetting.UpdateClientTypeInfo(ConVisitel, objClientTypeSettingDataObject, IsConfirm)
                        Catch ex As SqlException

                            If ex.Message.Contains("Duplicate Client Code") Then
                                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DuplicateClientMessage)
                                'ButtonSaveWithConfirmation.Visible = True
                                'ButtonSave.Visible = False

                                Dim scriptBlock As String = Nothing

                                scriptBlock = "<script type='text/javascript'> " _
                                  & " var CustomTargetButton ='ButtonSaveWithConfirmation'; " _
                                  & " var CustomDialogHeader ='Client Type: Save Forcefully'; " _
                                  & " var CustomDialogConfirmMsg ='" & SaveConfirmMessage & "'; " _
                                  & " var prm =''; " _
                                  & " jQuery(document).ready(function () {" _
                                  & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                                  & "     ClientTypeSave();" _
                                  & "     prm.add_endRequest(ClientTypeSave); " _
                                  & "}); " _
                                  & "</script>"

                                Page.Header.Controls.Add(New LiteralControl(scriptBlock))

                                Return
                            End If
                        End Try
                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(UpdateMessage)
                        Exit Select
                End Select
                objBLClientTypeSetting = Nothing

                HiddenFieldIsNew.Value = Convert.ToString(True, Nothing)
                HiddenFieldIdNumber.Value = Convert.ToString(Int32.MinValue)
                BindClientTypeInformationGridView()
            End If
        End Sub

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("EVV", ControlName & Convert.ToString(".resx"))

            LabelSearchClientTypeInfo.Text = Convert.ToString(ResourceTable("LabelSearchClientTypeInfo"), Nothing)
            LabelSearchClientTypeInfo.Text = If(String.IsNullOrEmpty(LabelSearchClientTypeInfo.Text), "Search Client Type Information", LabelSearchClientTypeInfo.Text)

            LabelSearchByName.Text = Convert.ToString(ResourceTable("LabelSearchByName"), Nothing)
            LabelSearchByName.Text = If(String.IsNullOrEmpty(LabelSearchByName.Text), "Name", LabelSearchByName.Text)

            LabelSearchByContractNumber.Text = Convert.ToString(ResourceTable("LabelHeaderContractNo"), Nothing)
            LabelSearchByContractNumber.Text = If(String.IsNullOrEmpty(LabelSearchByContractNumber.Text), "ContractNumber", LabelSearchByContractNumber.Text)

            LabelClientTypeInformationList.Text = Convert.ToString(ResourceTable("LabelClientTypeInformationList"), Nothing)
            LabelClientTypeInformationList.Text = If(String.IsNullOrEmpty(LabelClientTypeInformationList.Text),
                                                     "Client Type Information List", LabelClientTypeInformationList.Text)

            ButtonSave.Text = Convert.ToString(ResourceTable("ButtonSave"), Nothing)
            ButtonSave.Text = If(String.IsNullOrEmpty(ButtonSave.Text), "Save", ButtonSave.Text)

            ButtonSaveWithConfirmation.Text = ButtonSave.Text

            ButtonClear.Text = Convert.ToString(ResourceTable("ButtonClear"), Nothing)
            ButtonClear.Text = If(String.IsNullOrEmpty(ButtonClear.Text), "Clear", ButtonClear.Text)

            ButtonSearch.Text = Convert.ToString(ResourceTable("ButtonSearch"), Nothing)
            ButtonSearch.Text = If(String.IsNullOrEmpty(ButtonSearch.Text), "Search", ButtonSearch.Text)

            ButtonUpdateUnitRate.Text = Convert.ToString(ResourceTable("ButtonUpdateUnitRate"), Nothing)
            ButtonUpdateUnitRate.Text = If(String.IsNullOrEmpty(ButtonUpdateUnitRate.Text), "Update Unit Rate", ButtonUpdateUnitRate.Text)

            ButtonUpdateServiceType.Text = Convert.ToString(ResourceTable("ButtonUpdateServiceType"), Nothing)
            ButtonUpdateServiceType.Text = If(String.IsNullOrEmpty(ButtonUpdateServiceType.Text), "Update Service Type", ButtonUpdateServiceType.Text)

            ButtonViewError.Text = Convert.ToString(ResourceTable("ButtonViewError"), Nothing)
            ButtonViewError.Text = If(String.IsNullOrEmpty(ButtonViewError.Text), "View Detail Error", ButtonViewError.Text)

            SaveMessage = Convert.ToString(ResourceTable("SaveMessage"), Nothing)
            SaveMessage = If(String.IsNullOrEmpty(SaveMessage), "Saved Successfully", SaveMessage)

            DeleteMessage = Convert.ToString(ResourceTable("DeleteMessage"), Nothing)
            DeleteMessage = If(String.IsNullOrEmpty(DeleteMessage), "Deleted Successfully", DeleteMessage)

            ValidationGroup = Convert.ToString(ResourceTable("ValidationGroup"), Nothing)
            ValidationGroup = If(String.IsNullOrEmpty(ValidationGroup), "ClientType", ValidationGroup)

            DeleteConfirmationMessage = Convert.ToString(ResourceTable("DeleteConfirmationMessage"), Nothing)
            DeleteConfirmationMessage = If(String.IsNullOrEmpty(DeleteConfirmationMessage), "Are you sure to delete this record?", DeleteConfirmationMessage)

            SaveConfirmMessage = Convert.ToString(ResourceTable("SaveConfirmMessage"), Nothing)
            SaveConfirmMessage = If(String.IsNullOrEmpty(SaveConfirmMessage), "Are you sure to save this forcefully?", SaveConfirmMessage)

            DuplicateClientMessage = Convert.ToString(ResourceTable("DuplicateClientMessage"), Nothing)
            DuplicateClientMessage = If(String.IsNullOrEmpty(DuplicateClientMessage),
                                        "This client type already exists in TurboPAS for this agency along with name, and contract number", DuplicateClientMessage)
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

            LabelHeaderName = DirectCast(CurrentRow.FindControl("LabelHeaderName"), Label)
            LabelHeaderName.Text = Convert.ToString(ResourceTable("LabelHeaderName"), Nothing).Trim()
            LabelHeaderName.Text = If(String.IsNullOrEmpty(LabelHeaderName.Text), "Name", LabelHeaderName.Text)

            LabelHeaderContractNo = DirectCast(CurrentRow.FindControl("LabelHeaderContractNo"), Label)
            LabelHeaderContractNo.Text = Convert.ToString(ResourceTable("LabelHeaderContractNo"), Nothing).Trim()
            LabelHeaderContractNo.Text = If(String.IsNullOrEmpty(LabelHeaderContractNo.Text), "Contract Number", LabelHeaderContractNo.Text)

            LabelHeaderRegion = DirectCast(CurrentRow.FindControl("LabelHeaderRegion"), Label)
            LabelHeaderRegion.Text = Convert.ToString(ResourceTable("LabelHeaderRegion"), Nothing).Trim()
            LabelHeaderRegion.Text = If(String.IsNullOrEmpty(LabelHeaderRegion.Text), "Region", LabelHeaderRegion.Text)

            LabelHeaderServiceType = DirectCast(CurrentRow.FindControl("LabelHeaderServiceType"), Label)
            LabelHeaderServiceType.Text = Convert.ToString(ResourceTable("LabelHeaderServiceType"), Nothing).Trim()
            LabelHeaderServiceType.Text = If(String.IsNullOrEmpty(LabelHeaderServiceType.Text), "Service Type", LabelHeaderServiceType.Text)

            LabelHeaderProgramType = DirectCast(CurrentRow.FindControl("LabelHeaderProgramType"), Label)
            LabelHeaderProgramType.Text = Convert.ToString(ResourceTable("LabelHeaderProgramType"), Nothing).Trim()
            LabelHeaderProgramType.Text = If(String.IsNullOrEmpty(LabelHeaderProgramType.Text), "Program Type", LabelHeaderProgramType.Text)

            LabelHeaderUnitRate = DirectCast(CurrentRow.FindControl("LabelHeaderUnitRate"), Label)
            LabelHeaderUnitRate.Text = Convert.ToString(ResourceTable("LabelHeaderUnitRate"), Nothing).Trim()
            LabelHeaderUnitRate.Text = If(String.IsNullOrEmpty(LabelHeaderUnitRate.Text), "Unit Rate", LabelHeaderUnitRate.Text)

            LabelHeaderGroup = DirectCast(CurrentRow.FindControl("LabelHeaderGroup"), Label)
            LabelHeaderGroup.Text = Convert.ToString(ResourceTable("LabelHeaderGroup"), Nothing).Trim()
            LabelHeaderGroup.Text = If(String.IsNullOrEmpty(LabelHeaderGroup.Text), "Group", LabelHeaderGroup.Text)

            LabelHeaderReceiverName = DirectCast(CurrentRow.FindControl("LabelHeaderReceiverName"), Label)
            LabelHeaderReceiverName.Text = Convert.ToString(ResourceTable("LabelHeaderReceiverName"), Nothing).Trim()
            LabelHeaderReceiverName.Text = If(String.IsNullOrEmpty(LabelHeaderReceiverName.Text), "Receiver", LabelHeaderReceiverName.Text)

            LabelHeaderPayerName = DirectCast(CurrentRow.FindControl("LabelHeaderPayerName"), Label)
            LabelHeaderPayerName.Text = Convert.ToString(ResourceTable("LabelHeaderPayerName"), Nothing).Trim()
            LabelHeaderPayerName.Text = If(String.IsNullOrEmpty(LabelHeaderPayerName.Text), "Payer", LabelHeaderPayerName.Text)

            LabelHeaderStatus = DirectCast(CurrentRow.FindControl("LabelHeaderStatus"), Label)
            LabelHeaderStatus.Text = Convert.ToString(ResourceTable("LabelHeaderStatus"), Nothing).Trim()
            LabelHeaderStatus.Text = If(String.IsNullOrEmpty(LabelHeaderStatus.Text), "Status", LabelHeaderStatus.Text)

            LabelHeaderStatus = DirectCast(CurrentRow.FindControl("LabelHeaderStatus"), Label)
            LabelHeaderStatus.Text = Convert.ToString(ResourceTable("LabelHeaderStatus"), Nothing).Trim()
            LabelHeaderStatus.Text = If(String.IsNullOrEmpty(LabelHeaderStatus.Text), "Status", LabelHeaderStatus.Text)

            LabelHeaderSantrax = DirectCast(CurrentRow.FindControl("LabelHeaderSantrax"), Label)
            LabelHeaderSantrax.Text = Convert.ToString(ResourceTable("LabelHeaderSantrax"), Nothing).Trim()
            LabelHeaderSantrax.Text = If(String.IsNullOrEmpty(LabelHeaderSantrax.Text), "MEDsys / Vesta", LabelHeaderSantrax.Text)

            LabelHeaderCM = DirectCast(CurrentRow.FindControl("LabelHeaderCM"), Label)
            LabelHeaderCM.Text = Convert.ToString(ResourceTable("LabelHeaderCM"), Nothing).Trim()
            LabelHeaderCM.Text = If(String.IsNullOrEmpty(LabelHeaderCM.Text), "CM", LabelHeaderCM.Text)

            LabelHeaderUpdateDate = DirectCast(CurrentRow.FindControl("LabelHeaderUpdateDate"), Label)
            LabelHeaderUpdateDate.Text = Convert.ToString(ResourceTable("LabelHeaderUpdateDate"), Nothing).Trim()
            LabelHeaderUpdateDate.Text = If(String.IsNullOrEmpty(LabelHeaderUpdateDate.Text), "Update Date", LabelHeaderUpdateDate.Text)

            LabelHeaderUpdateBy = DirectCast(CurrentRow.FindControl("LabelHeaderUpdateBy"), Label)
            LabelHeaderUpdateBy.Text = Convert.ToString(ResourceTable("LabelHeaderUpdateBy"), Nothing).Trim()
            LabelHeaderUpdateBy.Text = If(String.IsNullOrEmpty(LabelHeaderUpdateBy.Text), "Update By", LabelHeaderUpdateBy.Text)

            EditText = Convert.ToString(ResourceTable("EditText"), Nothing)
            EditText = If(String.IsNullOrEmpty(EditText), "Edit", EditText)

            DeleteText = Convert.ToString(ResourceTable("DeleteText"), Nothing)
            DeleteText = If(String.IsNullOrEmpty(DeleteText), "Delete", DeleteText)

            InsertMessage = Convert.ToString(ResourceTable("InsertMessage"), Nothing)
            InsertMessage = If(String.IsNullOrEmpty(InsertMessage), "Inserted Successfully", InsertMessage)

            UpdateMessage = Convert.ToString(ResourceTable("UpdateMessage"), Nothing)
            UpdateMessage = If(String.IsNullOrEmpty(UpdateMessage), "Updated Successfully", UpdateMessage)

            DeleteMessage = Convert.ToString(ResourceTable("DeleteMessage"), Nothing)
            DeleteMessage = If(String.IsNullOrEmpty(DeleteMessage), "Deleted Successfully", DeleteMessage)

            DuplicateNameMessage = Convert.ToString(ResourceTable("DuplicateNameMessage"), Nothing)
            DuplicateNameMessage = If(String.IsNullOrEmpty(DuplicateNameMessage), "The same name is already exist.", DuplicateNameMessage)

            EmptyNameMessage = Convert.ToString(ResourceTable("EmptyNameMessage"), Nothing)
            EmptyNameMessage = If(String.IsNullOrEmpty(EmptyNameMessage), "Name Cannot be Blank", EmptyNameMessage)

            StatusSelectionMessage = Convert.ToString(ResourceTable("StatusSelectionMessage"), Nothing)
            StatusSelectionMessage = If(String.IsNullOrEmpty(StatusSelectionMessage), "Please select status", StatusSelectionMessage)

            ReceiverSelectionMessage = Convert.ToString(ResourceTable("ReceiverSelectionMessage"), Nothing)
            ReceiverSelectionMessage = If(String.IsNullOrEmpty(ReceiverSelectionMessage), "Please select receiver", ReceiverSelectionMessage)

            PayerSelectionMessage = Convert.ToString(ResourceTable("PayerSelectionMessage"), Nothing)
            PayerSelectionMessage = If(String.IsNullOrEmpty(PayerSelectionMessage), "Please select payer", PayerSelectionMessage)

            InvalidContractNumberMessage = Convert.ToString(ResourceTable("InvalidContractNumberMessage"), Nothing)
            InvalidContractNumberMessage = If(String.IsNullOrEmpty(InvalidContractNumberMessage), "Invalid Contact Number", InvalidContractNumberMessage)

            InvalidUnitRateMessage = Convert.ToString(ResourceTable("InvalidUnitRateMessage"), Nothing)
            InvalidUnitRateMessage = If(String.IsNullOrEmpty(InvalidUnitRateMessage), "Please enter decimal value for unit rate", InvalidUnitRateMessage)

            ResourceTable = Nothing

        End Sub

        ''' <summary>
        ''' This is for validating data in server side before submitting the form
        ''' </summary>
        ''' <returns></returns>
        Private Function ValidateData() As Boolean

            'If String.IsNullOrEmpty(TextBoxName.Text.Trim()) Then
            '    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(EmptyNameMessage)
            '    Return False
            'End If

            'If DropDownListStatus.SelectedValue = "-1" Then
            '    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(StatusSelectionMessage)
            '    Return False
            'End If

            'If DropDownListReceiver.SelectedValue = "-1" Then
            '    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(ReceiverSelectionMessage)
            '    Return False
            'End If

            'If DropDownListPayer.SelectedValue = "-1" Then
            '    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(PayerSelectionMessage)

            '    Return False
            'End If

            'If (Not objShared.ValidatePhone(TextBoxContractNumber.Text.Trim)) Then
            '    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(InvalidContractNumberMessage)
            '    Return False
            'End If

            If (Not objShared.IsDecimal(TextBoxUnitRate.Text.Trim)) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(InvalidUnitRateMessage)
                Return False
            End If

            Return True
        End Function

        ''' <summary>
        ''' This is for retrieving data
        ''' </summary>
        Private Sub GetData()
            GetClientTypeData()
        End Sub

        ''' <summary>
        ''' Clearing controls value
        ''' </summary>
        Private Sub ClearControl(CurrentRow As GridViewRow)
            ControlsFind(CurrentRow)
            BindClientTypeInformationGridView()

            TextBoxSearchByName.Text = String.Empty
            TextBoxSearchByContractNumber.Text = String.Empty

            HiddenFieldIsNew.Value = Convert.ToString(True, Nothing)
            HiddenFieldIsSearched.Value = Convert.ToString(False, Nothing)
            HiddenFieldIdNumber.Value = Convert.ToString(Int32.MinValue)
            HiddenFieldContractName.Value = String.Empty
            HiddenFieldContractNumber.Value = String.Empty

            'ButtonSaveWithConfirmation.Visible = False
            'ButtonSave.Visible = True
        End Sub

        Private Sub GetClientTypeData()

            Dim objBLClientTypeSetting As New BLClientTypeSetting()

            Try
                If (Convert.ToBoolean(HiddenFieldIsSearched.Value, Nothing)) Then
                    ClientTypeList = objBLClientTypeSetting.SearchClientTypeInfo(ConVisitel, CompanyId, TextBoxSearchByName.Text.Trim(), TextBoxSearchByContractNumber.Text.Trim())
                Else
                    ClientTypeList = objBLClientTypeSetting.SelectClientTypeInfo(ConVisitel, CompanyId)
                End If

            Catch ex As Exception
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to fetch Client Type Data. Message: {0}", ex.Message))
            Finally
                objBLClientTypeSetting = Nothing
            End Try
        End Sub

        ''' <summary>
        ''' Binding Client Type Information GridView
        ''' </summary>
        Private Sub BindClientTypeInformationGridView()

            GridViewClientTypeInformation.DataSource = ClientTypeList
            GridViewClientTypeInformation.DataBind()

            ItemChecked = DetermineButtonInActivity(GridViewClientTypeInformation, "CheckBoxSelect")

            ButtonSave.Enabled = ItemChecked
            ButtonUpdateUnitRate.Enabled = ButtonSave.Enabled
            ButtonUpdateServiceType.Enabled = ButtonSave.Enabled
        End Sub

        Private Sub BindRegionDropDownListFromGrid(ByRef CurrentRow As GridViewRow)

            DropDownListRegion = DirectCast(CurrentRow.FindControl("DropDownListRegion"), DropDownList)

            If (Not DropDownListRegion Is Nothing) Then
                objShared.BindRegionDropDownList(DropDownListRegion)

                If (Not (CurrentRow.RowType.Equals(DataControlRowType.Footer))) Then
                    DropDownListRegion.SelectedIndex = DropDownListRegion.Items.IndexOf(
                    DropDownListRegion.Items.FindByValue(Convert.ToString(LabelRegionId.Text.Trim(), Nothing)))
                End If
            End If
        End Sub

        Private Sub BindDropDownListServiceTypeFromGrid(ByRef CurrentRow As GridViewRow)
            DropDownListServiceType = DirectCast(CurrentRow.FindControl("DropDownListServiceType"), DropDownList)

            If (Not DropDownListServiceType Is Nothing) Then
                objShared.BindServiceTypeDropDownList(DropDownListServiceType)

                If (Not (CurrentRow.RowType.Equals(DataControlRowType.Footer))) Then
                    DropDownListServiceType.SelectedIndex = DropDownListServiceType.Items.IndexOf(
                    DropDownListServiceType.Items.FindByValue(Convert.ToString(LabelServiceTypeId.Text.Trim(), Nothing)))
                End If
            End If
        End Sub

        Private Sub BindDropDownListClientGroupFromGrid(ByRef CurrentRow As GridViewRow)
            DropDownListClientGroup = DirectCast(CurrentRow.FindControl("DropDownListClientGroup"), DropDownList)

            If (Not DropDownListClientGroup Is Nothing) Then
                objShared.BindClientGroupDropDownList(DropDownListClientGroup, objShared.CompanyId)

                If (Not (CurrentRow.RowType.Equals(DataControlRowType.Footer))) Then
                    DropDownListClientGroup.SelectedIndex = DropDownListClientGroup.Items.IndexOf(
                    DropDownListClientGroup.Items.FindByValue(Convert.ToString(LabelClientGroupId.Text.Trim(), Nothing)))
                End If
            End If
        End Sub

        Private Sub BindDropDownListReceiverFromGrid(ByRef CurrentRow As GridViewRow)
            DropDownListReceiver = DirectCast(CurrentRow.FindControl("DropDownListReceiver"), DropDownList)

            If (Not DropDownListReceiver Is Nothing) Then
                objShared.BindReceiverDropDownList(DropDownListReceiver, objShared.CompanyId)

                If (Not (CurrentRow.RowType.Equals(DataControlRowType.Footer))) Then
                    DropDownListReceiver.SelectedIndex = DropDownListReceiver.Items.IndexOf(
                    DropDownListReceiver.Items.FindByValue(Convert.ToString(LabelReceiverId.Text.Trim(), Nothing)))
                End If
            End If
        End Sub

        Private Sub BindDropDownListPayerFromGrid(ByRef CurrentRow As GridViewRow)
            DropDownListPayer = DirectCast(CurrentRow.FindControl("DropDownListPayer"), DropDownList)

            If (Not DropDownListPayer Is Nothing) Then
                objShared.BindPayerDropDownList(DropDownListPayer)

                If (Not (CurrentRow.RowType.Equals(DataControlRowType.Footer))) Then
                    DropDownListPayer.SelectedIndex = DropDownListPayer.Items.IndexOf(
                    DropDownListPayer.Items.FindByValue(Convert.ToString(LabelPayerId.Text.Trim(), Nothing)))
                End If
            End If
        End Sub

        Private Sub BindDropDownListStatusFromGrid(ByRef CurrentRow As GridViewRow)
            DropDownListStatus = DirectCast(CurrentRow.FindControl("DropDownListStatus"), DropDownList)

            If (Not DropDownListStatus Is Nothing) Then
                objShared.BindStatusDropDownList(DropDownListStatus)

                If (Not (CurrentRow.RowType.Equals(DataControlRowType.Footer))) Then
                    DropDownListStatus.SelectedIndex = DropDownListStatus.Items.IndexOf(
                    DropDownListStatus.Items.FindByValue(Convert.ToString(LabelStatusId.Text.Trim(), Nothing)))
                End If
            End If
        End Sub

    End Class
End Namespace