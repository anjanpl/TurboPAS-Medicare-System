Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports System.Data.SqlClient
Imports VisitelCommon
Imports VisitelBusiness.VisitelBusiness.DataObject

Namespace Visitel.UserControl.EVV
    Public Class ClientsControl
        Inherits CommonDataControl

        Private CheckBoxSelect As CheckBox, chkAll As CheckBox
        Private ItemChecked As Boolean = False

        Private ContractNumber As String
        Private objShared As SharedWebControls

        Private ControlName As String, ValidationGroup As String, SaveMessage As String, DeleteMessage As String, DeleteConfirmationMessage As String, ListFor As String

        Private ValidationEnable As Boolean
        Private CurrentRow As GridViewRow

        Private LabelHeaderSerial As Label, LabelHeaderSelect As Label, LabelHeaderMyUniqueId As Label, LabelHeaderClientIdVesta As Label, _
            LabelHeaderEVVId As Label, LabelHeaderLastName As Label, LabelHeaderFirstName As Label, LabelHeaderMedicaid As Label, LabelHeaderDateOfBirth As Label, _
            LabelHeaderGender As Label, LabelHeaderPhone As Label, LabelHeaderPhoneTwo As Label, LabelHeaderPhoneThree As Label, LabelHeaderAddressOne As Label, _
            LabelHeaderAddressTwo As Label, LabelHeaderCity As Label, LabelHeaderState As Label, LabelHeaderZip As Label, LabelHeaderPriority As Label, _
            LabelHeaderClientDeviceType As Label, LabelHeaderMedicaidStartDate As Label, LabelHeaderMedicaidEndDate As Label, LabelHeaderDADSIndividualRegion As Label, _
            LabelHeaderIndvMbrProgram As Label, LabelHeaderMCOMbrSDA As Label, LabelHeaderMbrEnrollStartDate As Label, LabelHeaderMbrEnrollEndDate As Label, _
            LabelHeaderError As Label, LabelHeaderClientId As Label, LabelHeaderUpdateDate As Label, LabelHeaderUpdateBy As Label, LabelId As Label, LabelClientId As Label

        Private TextBoxMyUniqueId As TextBox, TextBoxClientIdVesta As TextBox, TextBoxEVVId As TextBox, TextBoxLastName As TextBox, TextBoxFirstName As TextBox, _
            TextBoxMedicaid As TextBox, TextBoxDateOfBirth As TextBox, TextBoxGender As TextBox, TextBoxPhone As TextBox, TextBoxPhoneTwo As TextBox, _
            TextBoxPhoneThree As TextBox, TextBoxAddressOne As TextBox, TextBoxAddressTwo As TextBox, TextBoxCity As TextBox, TextBoxState As TextBox, _
            TextBoxZip As TextBox, TextBoxPriority As TextBox, TextBoxClientDeviceType As TextBox, TextBoxMedicaidStartDate As TextBox, _
            TextBoxMedicaidEndDate As TextBox, TextBoxDADSIndividualRegion As TextBox, TextBoxIndvMbrProgram As TextBox, TextBoxMCOMbrSDA As TextBox, _
            TextBoxMbrEnrollStartDate As TextBox, TextBoxMbrEnrollEndDate As TextBox, TextBoxError As TextBox, TextBoxClientId As TextBox, TextBoxUpdateDate As TextBox, _
            TextBoxUpdateBy As TextBox

        Private Clients As List(Of VisitelBusiness.VisitelBusiness.DataObject.EVV.ClientDataObject), ClientsErrorList As List(Of VisitelBusiness.VisitelBusiness.DataObject.EVV.ClientDataObject)

        Private SaveFailedCounter As Int16, DeleteFailedCounter As Int16, RecordSaveCount As Int16

        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "ClientsControl"
            objShared = New SharedWebControls()
            objShared.ConnectionOpen()
            GetCaptionFromResource()
            InitializeControl()
        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)

            GetData()

            If Not IsPostBack Then
                BindGridViewClients()
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

        Private Sub ButtonClear_Click(sender As Object, e As EventArgs)
            CurrentRow = GridViewClients.FooterRow
            ClearControl(CurrentRow)
        End Sub

        Private Sub ButtonSave_Click(sender As Object, e As EventArgs)

            Page.Validate()
            Page.Validate(ValidationGroup)

            Clients = New List(Of VisitelBusiness.VisitelBusiness.DataObject.EVV.ClientDataObject)()
            ClientsErrorList = New List(Of VisitelBusiness.VisitelBusiness.DataObject.EVV.ClientDataObject)()
            SaveFailedCounter = 0
            RecordSaveCount = 0

            CurrentRow = GridViewClients.FooterRow

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
                    BindGridViewClients()
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

            Clients = New List(Of VisitelBusiness.VisitelBusiness.DataObject.EVV.ClientDataObject)()
            ClientsErrorList = New List(Of VisitelBusiness.VisitelBusiness.DataObject.EVV.ClientDataObject)()

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
                BindGridViewClients()
            End If

            ButtonViewError.Visible = If((DeleteFailedCounter > 0), True, False)

            Clients = Nothing
            ClientsErrorList = Nothing
        End Sub

        Private Sub GridViewClients_RowDataBound(sender As Object, e As GridViewRowEventArgs)
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

                TextBoxClientIdVesta = DirectCast(CurrentRow.FindControl("TextBoxClientIdVesta"), TextBox)
                TextBoxClientIdVesta.ReadOnly = True

                TextBoxEVVId = DirectCast(CurrentRow.FindControl("TextBoxEVVId"), TextBox)
                TextBoxEVVId.ReadOnly = True

                TextBoxLastName = DirectCast(CurrentRow.FindControl("TextBoxLastName"), TextBox)
                TextBoxLastName.ReadOnly = True

                TextBoxFirstName = DirectCast(CurrentRow.FindControl("TextBoxFirstName"), TextBox)
                TextBoxFirstName.ReadOnly = True

                TextBoxMedicaid = DirectCast(CurrentRow.FindControl("TextBoxMedicaid"), TextBox)
                TextBoxMedicaid.ReadOnly = True

                TextBoxDateOfBirth = DirectCast(CurrentRow.FindControl("TextBoxDateOfBirth"), TextBox)
                TextBoxDateOfBirth.ReadOnly = True

                TextBoxGender = DirectCast(CurrentRow.FindControl("TextBoxGender"), TextBox)
                TextBoxGender.ReadOnly = True

                TextBoxPhone = DirectCast(CurrentRow.FindControl("TextBoxPhone"), TextBox)
                TextBoxPhone.ReadOnly = True

                TextBoxPhoneTwo = DirectCast(CurrentRow.FindControl("TextBoxPhoneTwo"), TextBox)
                TextBoxPhoneTwo.ReadOnly = True

                TextBoxPhoneThree = DirectCast(CurrentRow.FindControl("TextBoxPhoneThree"), TextBox)
                TextBoxPhoneThree.ReadOnly = True

                TextBoxAddressOne = DirectCast(CurrentRow.FindControl("TextBoxAddressOne"), TextBox)
                TextBoxAddressOne.ReadOnly = True

                TextBoxAddressTwo = DirectCast(CurrentRow.FindControl("TextBoxAddressTwo"), TextBox)
                TextBoxAddressTwo.ReadOnly = True

                TextBoxCity = DirectCast(CurrentRow.FindControl("TextBoxCity"), TextBox)
                TextBoxCity.ReadOnly = True

                TextBoxState = DirectCast(CurrentRow.FindControl("TextBoxState"), TextBox)
                TextBoxState.ReadOnly = True

                TextBoxZip = DirectCast(CurrentRow.FindControl("TextBoxZip"), TextBox)
                TextBoxZip.ReadOnly = True

                TextBoxPriority = DirectCast(CurrentRow.FindControl("TextBoxPriority"), TextBox)
                TextBoxPriority.ReadOnly = True

                TextBoxClientDeviceType = DirectCast(CurrentRow.FindControl("TextBoxClientDeviceType"), TextBox)
                TextBoxClientDeviceType.ReadOnly = True

                TextBoxMedicaidStartDate = DirectCast(CurrentRow.FindControl("TextBoxMedicaidStartDate"), TextBox)
                TextBoxMedicaidStartDate.ReadOnly = True

                TextBoxMedicaidEndDate = DirectCast(CurrentRow.FindControl("TextBoxMedicaidEndDate"), TextBox)
                TextBoxMedicaidEndDate.ReadOnly = True

                TextBoxDADSIndividualRegion = DirectCast(CurrentRow.FindControl("TextBoxDADSIndividualRegion"), TextBox)
                TextBoxDADSIndividualRegion.ReadOnly = True

                TextBoxIndvMbrProgram = DirectCast(CurrentRow.FindControl("TextBoxIndvMbrProgram"), TextBox)
                TextBoxIndvMbrProgram.ReadOnly = True

                TextBoxMCOMbrSDA = DirectCast(CurrentRow.FindControl("TextBoxMCOMbrSDA"), TextBox)
                TextBoxMCOMbrSDA.ReadOnly = True

                TextBoxMbrEnrollStartDate = DirectCast(CurrentRow.FindControl("TextBoxMbrEnrollStartDate"), TextBox)
                TextBoxMbrEnrollStartDate.ReadOnly = True

                TextBoxMbrEnrollEndDate = DirectCast(CurrentRow.FindControl("TextBoxMbrEnrollEndDate"), TextBox)
                TextBoxMbrEnrollEndDate.ReadOnly = True

                TextBoxError = DirectCast(CurrentRow.FindControl("TextBoxError"), TextBox)
                TextBoxError.ReadOnly = True

                TextBoxClientId = DirectCast(CurrentRow.FindControl("TextBoxClientId"), TextBox)
                TextBoxClientId.ReadOnly = True

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

        Private Sub GridViewClients_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
            GridViewClients.PageIndex = e.NewPageIndex
            BindGridViewClients()
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
        ''' Gridview Row Hover Color Set
        ''' </summary>
        ''' <param name="writer"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub Render(writer As HtmlTextWriter)
            For Each r As GridViewRow In GridViewClients.Rows
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
                          & " var DeleteDialogHeader ='Vesta Client'; " _
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

            LabelHeaderClientIdVesta = DirectCast(CurrentRow.FindControl("LabelHeaderClientIdVesta"), Label)
            LabelHeaderClientIdVesta.Text = Convert.ToString(ResourceTable("LabelHeaderClientIdVesta"), Nothing).Trim()
            LabelHeaderClientIdVesta.Text = If(String.IsNullOrEmpty(LabelHeaderClientIdVesta.Text), "Client ID Vesta", LabelHeaderClientIdVesta.Text)

            LabelHeaderEVVId = DirectCast(CurrentRow.FindControl("LabelHeaderEVVId"), Label)
            LabelHeaderEVVId.Text = Convert.ToString(ResourceTable("LabelHeaderEVVId"), Nothing).Trim()
            LabelHeaderEVVId.Text = If(String.IsNullOrEmpty(LabelHeaderEVVId.Text), "EVV ID", LabelHeaderEVVId.Text)

            LabelHeaderLastName = DirectCast(CurrentRow.FindControl("LabelHeaderLastName"), Label)
            LabelHeaderLastName.Text = Convert.ToString(ResourceTable("LabelHeaderLastName"), Nothing).Trim()
            LabelHeaderLastName.Text = If(String.IsNullOrEmpty(LabelHeaderLastName.Text), "Lastname", LabelHeaderLastName.Text)

            LabelHeaderFirstName = DirectCast(CurrentRow.FindControl("LabelHeaderFirstName"), Label)
            LabelHeaderFirstName.Text = Convert.ToString(ResourceTable("LabelHeaderFirstName"), Nothing).Trim()
            LabelHeaderFirstName.Text = If(String.IsNullOrEmpty(LabelHeaderFirstName.Text), "Firstname", LabelHeaderFirstName.Text)

            LabelHeaderMedicaid = DirectCast(CurrentRow.FindControl("LabelHeaderMedicaid"), Label)
            LabelHeaderMedicaid.Text = Convert.ToString(ResourceTable("LabelHeaderMedicaid"), Nothing).Trim()
            LabelHeaderMedicaid.Text = If(String.IsNullOrEmpty(LabelHeaderMedicaid.Text), "Medicaid", LabelHeaderMedicaid.Text)

            LabelHeaderDateOfBirth = DirectCast(CurrentRow.FindControl("LabelHeaderDateOfBirth"), Label)
            LabelHeaderDateOfBirth.Text = Convert.ToString(ResourceTable("LabelHeaderDateOfBirth"), Nothing).Trim()
            LabelHeaderDateOfBirth.Text = If(String.IsNullOrEmpty(LabelHeaderDateOfBirth.Text), "DOB", LabelHeaderDateOfBirth.Text)

            LabelHeaderGender = DirectCast(CurrentRow.FindControl("LabelHeaderGender"), Label)
            LabelHeaderGender.Text = Convert.ToString(ResourceTable("LabelHeaderGender"), Nothing).Trim()
            LabelHeaderGender.Text = If(String.IsNullOrEmpty(LabelHeaderGender.Text), "Gender", LabelHeaderGender.Text)

            LabelHeaderPhone = DirectCast(CurrentRow.FindControl("LabelHeaderPhone"), Label)
            LabelHeaderPhone.Text = Convert.ToString(ResourceTable("LabelHeaderPhone"), Nothing).Trim()
            LabelHeaderPhone.Text = If(String.IsNullOrEmpty(LabelHeaderPhone.Text), "Phone", LabelHeaderPhone.Text)

            LabelHeaderPhoneTwo = DirectCast(CurrentRow.FindControl("LabelHeaderPhoneTwo"), Label)
            LabelHeaderPhoneTwo.Text = Convert.ToString(ResourceTable("LabelHeaderPhoneTwo"), Nothing).Trim()
            LabelHeaderPhoneTwo.Text = If(String.IsNullOrEmpty(LabelHeaderPhoneTwo.Text), "Phone_2", LabelHeaderPhoneTwo.Text)

            LabelHeaderPhoneThree = DirectCast(CurrentRow.FindControl("LabelHeaderPhoneThree"), Label)
            LabelHeaderPhoneThree.Text = Convert.ToString(ResourceTable("LabelHeaderPhoneThree"), Nothing).Trim()
            LabelHeaderPhoneThree.Text = If(String.IsNullOrEmpty(LabelHeaderPhoneThree.Text), "Phone_3", LabelHeaderPhoneThree.Text)

            LabelHeaderAddressOne = DirectCast(CurrentRow.FindControl("LabelHeaderAddressOne"), Label)
            LabelHeaderAddressOne.Text = Convert.ToString(ResourceTable("LabelHeaderAddressOne"), Nothing).Trim()
            LabelHeaderAddressOne.Text = If(String.IsNullOrEmpty(LabelHeaderAddressOne.Text), "Address_1", LabelHeaderAddressOne.Text)

            LabelHeaderAddressTwo = DirectCast(CurrentRow.FindControl("LabelHeaderAddressTwo"), Label)
            LabelHeaderAddressTwo.Text = Convert.ToString(ResourceTable("LabelHeaderAddressTwo"), Nothing).Trim()
            LabelHeaderAddressTwo.Text = If(String.IsNullOrEmpty(LabelHeaderAddressTwo.Text), "Apt", LabelHeaderAddressTwo.Text)

            LabelHeaderCity = DirectCast(CurrentRow.FindControl("LabelHeaderCity"), Label)
            LabelHeaderCity.Text = Convert.ToString(ResourceTable("LabelHeaderCity"), Nothing).Trim()
            LabelHeaderCity.Text = If(String.IsNullOrEmpty(LabelHeaderCity.Text), "City", LabelHeaderCity.Text)

            LabelHeaderState = DirectCast(CurrentRow.FindControl("LabelHeaderState"), Label)
            LabelHeaderState.Text = Convert.ToString(ResourceTable("LabelHeaderState"), Nothing).Trim()
            LabelHeaderState.Text = If(String.IsNullOrEmpty(LabelHeaderState.Text), "State", LabelHeaderState.Text)

            LabelHeaderZip = DirectCast(CurrentRow.FindControl("LabelHeaderZip"), Label)
            LabelHeaderZip.Text = Convert.ToString(ResourceTable("LabelHeaderZip"), Nothing).Trim()
            LabelHeaderZip.Text = If(String.IsNullOrEmpty(LabelHeaderZip.Text), "Zip", LabelHeaderZip.Text)

            LabelHeaderPriority = DirectCast(CurrentRow.FindControl("LabelHeaderPriority"), Label)
            LabelHeaderPriority.Text = Convert.ToString(ResourceTable("LabelHeaderPriority"), Nothing).Trim()
            LabelHeaderPriority.Text = If(String.IsNullOrEmpty(LabelHeaderPriority.Text), "Priority", LabelHeaderPriority.Text)

            LabelHeaderClientDeviceType = DirectCast(CurrentRow.FindControl("LabelHeaderClientDeviceType"), Label)
            LabelHeaderClientDeviceType.Text = Convert.ToString(ResourceTable("LabelHeaderClientDeviceType"), Nothing).Trim()
            LabelHeaderClientDeviceType.Text = If(String.IsNullOrEmpty(LabelHeaderClientDeviceType.Text), "Device Type", LabelHeaderClientDeviceType.Text)

            LabelHeaderMedicaidStartDate = DirectCast(CurrentRow.FindControl("LabelHeaderMedicaidStartDate"), Label)
            LabelHeaderMedicaidStartDate.Text = Convert.ToString(ResourceTable("LabelHeaderMedicaidStartDate"), Nothing).Trim()
            LabelHeaderMedicaidStartDate.Text = If(String.IsNullOrEmpty(LabelHeaderMedicaidStartDate.Text), "Medicaid Start Date", LabelHeaderMedicaidStartDate.Text)

            LabelHeaderMedicaidEndDate = DirectCast(CurrentRow.FindControl("LabelHeaderMedicaidEndDate"), Label)
            LabelHeaderMedicaidEndDate.Text = Convert.ToString(ResourceTable("LabelHeaderMedicaidEndDate"), Nothing).Trim()
            LabelHeaderMedicaidEndDate.Text = If(String.IsNullOrEmpty(LabelHeaderMedicaidEndDate.Text), "Medicaid End Date", LabelHeaderMedicaidEndDate.Text)

            LabelHeaderDADSIndividualRegion = DirectCast(CurrentRow.FindControl("LabelHeaderDADSIndividualRegion"), Label)
            LabelHeaderDADSIndividualRegion.Text = Convert.ToString(ResourceTable("LabelHeaderDADSIndividualRegion"), Nothing).Trim()
            LabelHeaderDADSIndividualRegion.Text = If(String.IsNullOrEmpty(LabelHeaderDADSIndividualRegion.Text), "DADS Region", LabelHeaderDADSIndividualRegion.Text)

            LabelHeaderIndvMbrProgram = DirectCast(CurrentRow.FindControl("LabelHeaderIndvMbrProgram"), Label)
            LabelHeaderIndvMbrProgram.Text = Convert.ToString(ResourceTable("LabelHeaderIndvMbrProgram"), Nothing).Trim()
            LabelHeaderIndvMbrProgram.Text = If(String.IsNullOrEmpty(LabelHeaderIndvMbrProgram.Text), "Member Program", LabelHeaderIndvMbrProgram.Text)

            LabelHeaderMCOMbrSDA = DirectCast(CurrentRow.FindControl("LabelHeaderMCOMbrSDA"), Label)
            LabelHeaderMCOMbrSDA.Text = Convert.ToString(ResourceTable("LabelHeaderMCOMbrSDA"), Nothing).Trim()
            LabelHeaderMCOMbrSDA.Text = If(String.IsNullOrEmpty(LabelHeaderMCOMbrSDA.Text), "MOO Mbr SDA", LabelHeaderMCOMbrSDA.Text)

            LabelHeaderMbrEnrollStartDate = DirectCast(CurrentRow.FindControl("LabelHeaderMbrEnrollStartDate"), Label)
            LabelHeaderMbrEnrollStartDate.Text = Convert.ToString(ResourceTable("LabelHeaderMbrEnrollStartDate"), Nothing).Trim()
            LabelHeaderMbrEnrollStartDate.Text = If(String.IsNullOrEmpty(LabelHeaderMbrEnrollStartDate.Text), "Mbr Enroll Start Date", LabelHeaderMbrEnrollStartDate.Text)

            LabelHeaderMbrEnrollEndDate = DirectCast(CurrentRow.FindControl("LabelHeaderMbrEnrollEndDate"), Label)
            LabelHeaderMbrEnrollEndDate.Text = Convert.ToString(ResourceTable("LabelHeaderMbrEnrollEndDate"), Nothing).Trim()
            LabelHeaderMbrEnrollEndDate.Text = If(String.IsNullOrEmpty(LabelHeaderMbrEnrollEndDate.Text), "Mbr Enroll End Date", LabelHeaderMbrEnrollEndDate.Text)

            LabelHeaderError = DirectCast(CurrentRow.FindControl("LabelHeaderError"), Label)
            LabelHeaderError.Text = Convert.ToString(ResourceTable("LabelHeaderError"), Nothing).Trim()
            LabelHeaderError.Text = If(String.IsNullOrEmpty(LabelHeaderError.Text), "Error", LabelHeaderError.Text)

            LabelHeaderClientId = DirectCast(CurrentRow.FindControl("LabelHeaderClientId"), Label)
            LabelHeaderClientId.Text = Convert.ToString(ResourceTable("LabelHeaderClientId"), Nothing).Trim()
            LabelHeaderClientId.Text = If(String.IsNullOrEmpty(LabelHeaderClientId.Text), "Turbo ID", LabelHeaderClientId.Text)

            LabelHeaderUpdateDate = DirectCast(CurrentRow.FindControl("LabelHeaderUpdateDate"), Label)
            LabelHeaderUpdateDate.Text = Convert.ToString(ResourceTable("LabelHeaderUpdateDate"), Nothing).Trim()
            LabelHeaderUpdateDate.Text = If(String.IsNullOrEmpty(LabelHeaderUpdateDate.Text), "Update Date", LabelHeaderUpdateDate.Text)

            LabelHeaderUpdateBy = DirectCast(CurrentRow.FindControl("LabelHeaderUpdateBy"), Label)
            LabelHeaderUpdateBy.Text = Convert.ToString(ResourceTable("LabelHeaderUpdateBy"), Nothing).Trim()
            LabelHeaderUpdateBy.Text = If(String.IsNullOrEmpty(LabelHeaderUpdateBy.Text), "Update By", LabelHeaderUpdateBy.Text)

            ResourceTable = Nothing

        End Sub

        Protected Sub OnCheckedChanged(sender As Object, e As EventArgs)
            Dim isUpdateVisible As Boolean = False

            Dim chk As CheckBox = TryCast(sender, CheckBox)

            'ButtonSave.Enabled = If((chk.Checked), True, False)

            If chk.ID = "chkAll" Then
                For Each row As GridViewRow In GridViewClients.Rows
                    If row.RowType = DataControlRowType.DataRow Then
                        row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked = chk.Checked
                    End If
                Next

                CurrentRow = GridViewClients.FooterRow
                CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)
                CheckBoxSelect.Checked = chk.Checked

            End If

            Dim chkAll As CheckBox = TryCast(GridViewClients.HeaderRow.FindControl("chkAll"), CheckBox)
            'chkAll.Checked = True
            For Each row As GridViewRow In GridViewClients.Rows
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

            ItemChecked = DetermineButtonInActivity(GridViewClients, "CheckBoxSelect")

            ButtonSave.Enabled = ItemChecked
            ButtonDelete.Enabled = ButtonSave.Enabled
            ButtonIndividualDetail.Enabled = ButtonSave.Enabled

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

            If (Not TextBoxClientIdVesta Is Nothing) Then
                TextBoxClientIdVesta.ReadOnly = Not isChecked
                TextBoxClientIdVesta.CssClass = If((TextBoxClientIdVesta.ReadOnly), "TextBoxClientIdVesta", "TextBoxClientIdVestaEdit")
            End If

            If (Not TextBoxEVVId Is Nothing) Then
                TextBoxEVVId.ReadOnly = Not isChecked
                TextBoxEVVId.CssClass = If((TextBoxEVVId.ReadOnly), "TextBoxEVVId", "TextBoxEVVIdEdit")
            End If

            If (Not TextBoxLastName Is Nothing) Then
                TextBoxLastName.ReadOnly = Not isChecked
                TextBoxLastName.CssClass = If((TextBoxLastName.ReadOnly), "TextBoxLastName", "TextBoxLastNameEdit")
            End If

            If (Not TextBoxFirstName Is Nothing) Then
                TextBoxFirstName.ReadOnly = Not isChecked
                TextBoxFirstName.CssClass = If((TextBoxFirstName.ReadOnly), "TextBoxFirstName", "TextBoxFirstNameEdit")
            End If

            If (Not TextBoxMedicaid Is Nothing) Then
                TextBoxMedicaid.ReadOnly = Not isChecked
                TextBoxMedicaid.CssClass = If((TextBoxMedicaid.ReadOnly), "TextBoxMedicaid", "TextBoxMedicaidEdit")
            End If

            If (Not TextBoxDateOfBirth Is Nothing) Then
                TextBoxDateOfBirth.ReadOnly = Not isChecked
                TextBoxDateOfBirth.CssClass = If((TextBoxDateOfBirth.ReadOnly), "TextBoxDateOfBirth", "TextBoxDateOfBirthEdit")
            End If

            If (Not TextBoxGender Is Nothing) Then
                TextBoxGender.ReadOnly = Not isChecked
                TextBoxGender.CssClass = If((TextBoxGender.ReadOnly), "TextBoxGender", "TextBoxGenderEdit")
            End If

            If (Not TextBoxPhone Is Nothing) Then
                TextBoxPhone.ReadOnly = Not isChecked
                TextBoxPhone.CssClass = If((TextBoxPhone.ReadOnly), "TextBoxPhone", "TextBoxPhoneEdit")
            End If

            If (Not TextBoxPhoneTwo Is Nothing) Then
                TextBoxPhoneTwo.ReadOnly = Not isChecked
                TextBoxPhoneTwo.CssClass = If((TextBoxPhoneTwo.ReadOnly), "TextBoxPhoneTwo", "TextBoxPhoneTwoEdit")
            End If

            If (Not TextBoxPhoneThree Is Nothing) Then
                TextBoxPhoneThree.ReadOnly = Not isChecked
                TextBoxPhoneThree.CssClass = If((TextBoxPhoneThree.ReadOnly), "TextBoxPhoneThree", "TextBoxPhoneThreeEdit")
            End If

            If (Not TextBoxAddressOne Is Nothing) Then
                TextBoxAddressOne.ReadOnly = Not isChecked
                TextBoxAddressOne.CssClass = If((TextBoxAddressOne.ReadOnly), "TextBoxAddressOne", "TextBoxAddressOneEdit")
            End If

            If (Not TextBoxAddressTwo Is Nothing) Then
                TextBoxAddressTwo.ReadOnly = Not isChecked
                TextBoxAddressTwo.CssClass = If((TextBoxAddressTwo.ReadOnly), "TextBoxAddressTwo", "TextBoxAddressTwoEdit")
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

            If (Not TextBoxPriority Is Nothing) Then
                TextBoxPriority.ReadOnly = Not isChecked
                TextBoxPriority.CssClass = If((TextBoxPriority.ReadOnly), "TextBoxPriority", "TextBoxPriorityEdit")
            End If

            If (Not TextBoxClientDeviceType Is Nothing) Then
                TextBoxClientDeviceType.ReadOnly = Not isChecked
                TextBoxClientDeviceType.CssClass = If((TextBoxClientDeviceType.ReadOnly), "TextBoxClientDeviceType", "TextBoxClientDeviceTypeEdit")
            End If

            If (Not TextBoxMedicaidStartDate Is Nothing) Then
                TextBoxMedicaidStartDate.ReadOnly = Not isChecked
                TextBoxMedicaidStartDate.CssClass = If((TextBoxMedicaidStartDate.ReadOnly), "TextBoxMedicaidStartDate", "TextBoxMedicaidStartDateEdit")
            End If

            If (Not TextBoxMedicaidEndDate Is Nothing) Then
                TextBoxMedicaidEndDate.ReadOnly = Not isChecked
                TextBoxMedicaidEndDate.CssClass = If((TextBoxMedicaidEndDate.ReadOnly), "TextBoxMedicaidEndDate", "TextBoxMedicaidEndDateEdit")
            End If

            If (Not TextBoxDADSIndividualRegion Is Nothing) Then
                TextBoxDADSIndividualRegion.ReadOnly = Not isChecked
                TextBoxDADSIndividualRegion.CssClass = If((TextBoxDADSIndividualRegion.ReadOnly), "TextBoxDADSIndividualRegion", "TextBoxDADSIndividualRegionEdit")
            End If

            If (Not TextBoxIndvMbrProgram Is Nothing) Then
                TextBoxIndvMbrProgram.ReadOnly = Not isChecked
                TextBoxIndvMbrProgram.CssClass = If((TextBoxIndvMbrProgram.ReadOnly), "TextBoxIndvMbrProgram", "TextBoxIndvMbrProgramEdit")
            End If

            If (Not TextBoxMCOMbrSDA Is Nothing) Then
                TextBoxMCOMbrSDA.ReadOnly = Not isChecked
                TextBoxMCOMbrSDA.CssClass = If((TextBoxMCOMbrSDA.ReadOnly), "TextBoxMCOMbrSDA", "TextBoxMCOMbrSDAEdit")
            End If

            If (Not TextBoxMbrEnrollStartDate Is Nothing) Then
                TextBoxMbrEnrollStartDate.ReadOnly = Not isChecked
                TextBoxMbrEnrollStartDate.CssClass = If((TextBoxMbrEnrollStartDate.ReadOnly), "TextBoxMbrEnrollStartDate", "TextBoxMbrEnrollStartDateEdit")
            End If

            If (Not TextBoxMbrEnrollEndDate Is Nothing) Then
                TextBoxMbrEnrollEndDate.ReadOnly = Not isChecked
                TextBoxMbrEnrollEndDate.CssClass = If((TextBoxMbrEnrollEndDate.ReadOnly), "TextBoxMbrEnrollEndDate", "TextBoxMbrEnrollEndDateEdit")
            End If

            If (Not TextBoxError Is Nothing) Then
                TextBoxError.ReadOnly = Not isChecked
                TextBoxError.CssClass = If((TextBoxError.ReadOnly), "TextBoxError", "TextBoxErrorEdit")
            End If

            If (Not TextBoxClientId Is Nothing) Then
                TextBoxClientId.ReadOnly = Not isChecked
                TextBoxClientId.CssClass = If((TextBoxError.ReadOnly), "TextBoxClientId", "TextBoxClientIdEdit")
            End If

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

            Dim objClientDataObject As VisitelBusiness.VisitelBusiness.DataObject.EVV.ClientDataObject

            Select Case Action
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT)

                    TextBoxClientId = DirectCast(GridViewClients.FooterRow.FindControl("TextBoxClientId"), TextBox)
                    If (String.IsNullOrEmpty(TextBoxClientId.Text)) Then
                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please enter one client id")
                        Return
                    End If

                    TextBoxMyUniqueId = DirectCast(GridViewClients.FooterRow.FindControl("TextBoxMyUniqueId"), TextBox)
                    If (String.IsNullOrEmpty(TextBoxMyUniqueId.Text)) Then
                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please enter one My Unique Id")
                        Return
                    End If

                    objClientDataObject = New VisitelBusiness.VisitelBusiness.DataObject.EVV.ClientDataObject()
                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)

                    AddToList(CurrentRow, Clients, objClientDataObject)
                    RecordSaveCount = RecordSaveCount + 1
                    objClientDataObject = Nothing

                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE),
                    EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE)
                    Clients.Clear()
                    For Each row As GridViewRow In GridViewClients.Rows
                        If (row.RowType.Equals(DataControlRowType.DataRow)) Then
                            Dim isChecked As Boolean = row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked

                            If (isChecked) Then

                                objClientDataObject = New VisitelBusiness.VisitelBusiness.DataObject.EVV.ClientDataObject()

                                CurrentRow = DirectCast(GridViewClients.Rows(row.RowIndex), GridViewRow)

                                TextBoxClientId = DirectCast(CurrentRow.FindControl("TextBoxClientId"), TextBox)
                                If (String.IsNullOrEmpty(TextBoxClientId.Text)) Then
                                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please enter one client id")
                                    Return
                                End If

                                TextBoxMyUniqueId = DirectCast(CurrentRow.FindControl("TextBoxMyUniqueId"), TextBox)
                                If (String.IsNullOrEmpty(TextBoxMyUniqueId.Text)) Then
                                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please enter one My Unique Id")
                                    Return
                                End If

                                If (Action.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE))) Then
                                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)
                                End If

                                If (Action.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE))) Then
                                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE)
                                End If

                                AddToList(CurrentRow, Clients, objClientDataObject)
                                RecordSaveCount = RecordSaveCount + 1
                                objClientDataObject = Nothing

                            End If

                        End If
                    Next

                    Exit Select
            End Select

            objClientDataObject = Nothing

        End Sub

        ''' <summary>
        ''' Make Database operation either for record saving or deleting
        ''' </summary>
        ''' <param name="Action"></param>
        ''' <remarks></remarks>
        Private Sub TakeAction(Action As String)

            Dim objBLVestaClient As New BLVestaClient()

            For Each Client In Clients
                Try
                    Select Case Action
                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT)
                            objBLVestaClient.InsertClientInfo(objShared.ConVisitel, Client)
                            Exit Select

                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE)
                            objBLVestaClient.UpdateClientInfo(objShared.ConVisitel, Client)
                            Exit Select

                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE)
                            objBLVestaClient.DeleteClientInfo(objShared.ConVisitel, Client.Id, Client.UpdateBy)
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

            objBLVestaClient = Nothing

        End Sub

        ''' <summary>
        ''' Making List in order to save records or delete
        ''' </summary>
        ''' <param name="CurrentRow"></param>
        ''' <param name="Clients"></param>
        ''' <param name="objClientDataObject"></param>
        ''' <remarks></remarks>
        Private Sub AddToList(ByRef CurrentRow As GridViewRow, ByRef Clients As List(Of VisitelBusiness.VisitelBusiness.DataObject.EVV.ClientDataObject), _
                              ByRef objClientDataObject As VisitelBusiness.VisitelBusiness.DataObject.EVV.ClientDataObject)

            Dim Int32Result As Int32

            LabelId = DirectCast(CurrentRow.FindControl("LabelId"), Label)

            If (ListFor.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE))) Then
                ControlsFind(CurrentRow)
            End If

            If (Page.IsValid) Then

                objClientDataObject.Id = If(Int32.TryParse(LabelId.Text.Trim(), Int32Result), Int32Result, objClientDataObject.Id)

                If (ListFor.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE))) Then

                    objClientDataObject.MyUniqueID = TextBoxMyUniqueId.Text.Trim()
                    objClientDataObject.ClientIdVesta = TextBoxClientIdVesta.Text.Trim()
                    objClientDataObject.EVVId = TextBoxEVVId.Text.Trim()
                    objClientDataObject.LastName = TextBoxLastName.Text.Trim()
                    objClientDataObject.FirstName = TextBoxFirstName.Text.Trim()
                    objClientDataObject.Medicaid = TextBoxMedicaid.Text.Trim()
                    objClientDataObject.DateOfBirth = TextBoxDateOfBirth.Text.Trim()
                    objClientDataObject.Gender = TextBoxGender.Text.Trim()
                    objClientDataObject.Phone = TextBoxPhone.Text.Trim()
                    objClientDataObject.PhoneTwo = TextBoxPhoneTwo.Text.Trim()
                    objClientDataObject.PhoneThree = TextBoxPhoneThree.Text.Trim()
                    objClientDataObject.AddressOne = TextBoxAddressOne.Text.Trim()
                    objClientDataObject.AddressTwo = TextBoxAddressTwo.Text.Trim()
                    objClientDataObject.City = TextBoxCity.Text.Trim()
                    objClientDataObject.State = TextBoxState.Text.Trim()
                    objClientDataObject.Zip = TextBoxZip.Text.Trim()
                    objClientDataObject.Priority = TextBoxPriority.Text.Trim()
                    objClientDataObject.ClientDeviceType = TextBoxClientDeviceType.Text.Trim()
                    objClientDataObject.MedicaidStartDate = TextBoxMedicaidStartDate.Text.Trim()
                    objClientDataObject.MedicaidEndDate = TextBoxMedicaidEndDate.Text.Trim()
                    objClientDataObject.DADSIndividualRegion = TextBoxDADSIndividualRegion.Text.Trim()
                    objClientDataObject.IndvMbrProgram = TextBoxIndvMbrProgram.Text.Trim()
                    objClientDataObject.MCOMbrSDA = TextBoxMCOMbrSDA.Text.Trim()
                    objClientDataObject.MbrEnrollStartDate = TextBoxMbrEnrollStartDate.Text.Trim()
                    objClientDataObject.MbrEnrollEndDate = TextBoxMbrEnrollEndDate.Text.Trim()
                    objClientDataObject.Error = TextBoxError.Text.Trim()
                    objClientDataObject.ClientId = TextBoxClientId.Text.Trim()

                End If

                objClientDataObject.UpdateBy = objShared.UserId

                Clients.Add(objClientDataObject)

            End If

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

            GridViewClients.AutoGenerateColumns = False
            GridViewClients.ShowHeaderWhenEmpty = True
            GridViewClients.AllowPaging = True
            GridViewClients.AllowSorting = True

            GridViewClients.ShowFooter = True

            If (GridViewClients.AllowPaging) Then
                GridViewClients.PageSize = objShared.GridViewDefaultPageSize
            End If

            AddHandler GridViewClients.RowDataBound, AddressOf GridViewClients_RowDataBound
            AddHandler GridViewClients.PageIndexChanging, AddressOf GridViewClients_PageIndexChanging

            ButtonClear.ClientIDMode = UI.ClientIDMode.Static
            ButtonSave.ClientIDMode = UI.ClientIDMode.Static
            ButtonDelete.ClientIDMode = UI.ClientIDMode.Static
            ButtonIndividualDetail.ClientIDMode = UI.ClientIDMode.Static

        End Sub

        Private Sub BindGridViewClients()
            GridViewClients.DataSource = Clients
            GridViewClients.DataBind()

            ItemChecked = DetermineButtonInActivity(GridViewClients, "CheckBoxSelect")

            ButtonSave.Enabled = ItemChecked
            ButtonDelete.Enabled = ButtonSave.Enabled
            ButtonIndividualDetail.Enabled = ButtonSave.Enabled
        End Sub

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("EVV", ControlName & Convert.ToString(".resx"))

            LabelClients.Text = Convert.ToString(ResourceTable("LabelClients"), Nothing)
            LabelClients.Text = If(String.IsNullOrEmpty(LabelClients.Text), "Vesta Clients", LabelClients.Text)

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
            ValidationGroup = If(String.IsNullOrEmpty(ValidationGroup), "VestaClients", ValidationGroup)

            DeleteConfirmationMessage = Convert.ToString(ResourceTable("DeleteConfirmationMessage"), Nothing)
            DeleteConfirmationMessage = If(String.IsNullOrEmpty(DeleteConfirmationMessage), "Are you sure you want to delete this record?", DeleteConfirmationMessage)

            Dim ResultOut As Boolean

            ValidationEnable = If((Boolean.TryParse(ResourceTable("ValidationEnable"), ResultOut)), ResultOut, True)

            ResourceTable = Nothing

        End Sub

        Private Sub ControlsFind(ByRef CurrentRow As GridViewRow)
            TextBoxMyUniqueId = DirectCast(CurrentRow.FindControl("TextBoxMyUniqueId"), TextBox)
            TextBoxClientIdVesta = DirectCast(CurrentRow.FindControl("TextBoxClientIdVesta"), TextBox)
            TextBoxEVVId = DirectCast(CurrentRow.FindControl("TextBoxEVVId"), TextBox)
            TextBoxLastName = DirectCast(CurrentRow.FindControl("TextBoxLastName"), TextBox)
            TextBoxFirstName = DirectCast(CurrentRow.FindControl("TextBoxFirstName"), TextBox)
            TextBoxMedicaid = DirectCast(CurrentRow.FindControl("TextBoxMedicaid"), TextBox)
            TextBoxDateOfBirth = DirectCast(CurrentRow.FindControl("TextBoxDateOfBirth"), TextBox)
            TextBoxGender = DirectCast(CurrentRow.FindControl("TextBoxGender"), TextBox)
            TextBoxPhone = DirectCast(CurrentRow.FindControl("TextBoxPhone"), TextBox)
            TextBoxPhoneTwo = DirectCast(CurrentRow.FindControl("TextBoxPhoneTwo"), TextBox)
            TextBoxPhoneThree = DirectCast(CurrentRow.FindControl("TextBoxPhoneThree"), TextBox)
            TextBoxAddressOne = DirectCast(CurrentRow.FindControl("TextBoxAddressOne"), TextBox)
            TextBoxAddressTwo = DirectCast(CurrentRow.FindControl("TextBoxAddressTwo"), TextBox)
            TextBoxCity = DirectCast(CurrentRow.FindControl("TextBoxCity"), TextBox)
            TextBoxState = DirectCast(CurrentRow.FindControl("TextBoxState"), TextBox)
            TextBoxZip = DirectCast(CurrentRow.FindControl("TextBoxZip"), TextBox)
            TextBoxPriority = DirectCast(CurrentRow.FindControl("TextBoxPriority"), TextBox)
            TextBoxClientDeviceType = DirectCast(CurrentRow.FindControl("TextBoxClientDeviceType"), TextBox)
            TextBoxMedicaidStartDate = DirectCast(CurrentRow.FindControl("TextBoxMedicaidStartDate"), TextBox)
            TextBoxMedicaidEndDate = DirectCast(CurrentRow.FindControl("TextBoxMedicaidEndDate"), TextBox)
            TextBoxDADSIndividualRegion = DirectCast(CurrentRow.FindControl("TextBoxDADSIndividualRegion"), TextBox)
            TextBoxIndvMbrProgram = DirectCast(CurrentRow.FindControl("TextBoxIndvMbrProgram"), TextBox)
            TextBoxMCOMbrSDA = DirectCast(CurrentRow.FindControl("TextBoxMCOMbrSDA"), TextBox)
            TextBoxMbrEnrollStartDate = DirectCast(CurrentRow.FindControl("TextBoxMbrEnrollStartDate"), TextBox)
            TextBoxMbrEnrollEndDate = DirectCast(CurrentRow.FindControl("TextBoxMbrEnrollEndDate"), TextBox)
            TextBoxError = DirectCast(CurrentRow.FindControl("TextBoxError"), TextBox)
            TextBoxClientId = DirectCast(CurrentRow.FindControl("TextBoxClientId"), TextBox)

        End Sub

        ''' <summary>
        ''' Clearing controls value
        ''' </summary>
        Private Sub ClearControl(CurrentRow As GridViewRow)
            ControlsFind(CurrentRow)
            BindGridViewClients()
        End Sub

        Private Sub GetData()
            GetClientData()
        End Sub

        Private Sub GetClientData()

            Dim objBLVestaClient As New BLVestaClient()

            Try
                Clients = objBLVestaClient.SelectClientInfo(objShared.ConVisitel)
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to fetch Client Data. Message: {0}", ex.Message))
            Finally
                objBLVestaClient = Nothing
            End Try
        End Sub

    End Class
End Namespace