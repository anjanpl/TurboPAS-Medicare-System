#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Doctor Setup 
' Author: Anjan Kumar Paul
' Start Date: 15 Aug 2014
' End Date: 15 Aug 2014
' History:
'      Version                  Author                      Date             Reason 
'      1.0.0                                                15 Aug 2014      Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports VisitelCommon.VisitelCommon
Imports System.Data.SqlClient
Imports VisitelBusiness.VisitelBusiness.Settings
Imports VisitelBusiness.VisitelBusiness.DataObject.Settings
Imports VisitelBusiness.VisitelBusiness.DataObject

Namespace Visitel.UserControl.Settings
    Public Class DoctorSetting
        Inherits BaseUserControl

        Private objShared As SharedWebControls

        Private UserId As Integer
        Private CompanyId As Integer
        Private ConVisitel As SqlConnection

        Private ControlName As String, EditText As String, DeleteText As String, InsertMessage As String, UpdateMessage As String, DeleteMessage As String, _
         CitySelectionMessage As String, StateSelectionMessage As String, StatusSelectionMessage As String, DuplicateUpinMessage As String, _
         DuplicateLicenseMessage As String, InvalidPhoneMessage As String, InvalidFaxMessage As String


        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "DoctorSetting"
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
                GetData()
            End If

        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadJavascript()
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
        End Sub

        Protected Sub LinkButtonEdit_Click(sender As Object, e As EventArgs)
            Dim LinkButtonDelete As LinkButton = DirectCast(sender, LinkButton)
            Dim GridViewItemRow As GridViewRow = DirectCast(LinkButtonDelete.NamingContainer, GridViewRow)

            TextBoxUPinNumber.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelUPinNumber"), Label).Text, Nothing)
            TextBoxLicNumber.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelLicNumber"), Label).Text, Nothing)
            TextBoxFirstName.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelFirstName"), Label).Text, Nothing)
            TextBoxLastName.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelLastName"), Label).Text, Nothing)
            TextBoxAddress.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelAddress"), Label).Text, Nothing)
            TextBoxSuite.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelSuite"), Label).Text, Nothing)
            TextBoxZip.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelZip"), Label).Text, Nothing)

            Dim Phone As String = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelPhone"), Label).Text, Nothing)
            TextBoxPhone.Text = objShared.GetReFormattedMobileNumber(Convert.ToString(Phone, Nothing))

            Dim Fax As String = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelFax"), Label).Text, Nothing)
            TextBoxFax.Text = objShared.GetReFormattedMobileNumber(Convert.ToString(Fax, Nothing))

            DropDownListStatus.SelectedIndex = DropDownListStatus.Items.IndexOf(DropDownListStatus.Items.FindByText(
                                               Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelStatus"), Label).Text, Nothing)))

            DropDownListCity.SelectedIndex = DropDownListCity.Items.IndexOf(DropDownListCity.Items.FindByValue(
                                             Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelCityId"), Label).Text, Nothing)))

            DropDownListState.SelectedIndex = DropDownListState.Items.IndexOf(DropDownListState.Items.FindByValue(
                                              Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelStateId"), Label).Text, Nothing)))

            HiddenFieldDoctorId.Value = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelDoctorId"), Label).Text, Nothing)

            HiddenFieldIsNew.Value = Convert.ToString(False, Nothing)

        End Sub

        Protected Sub ButtonDelete_Click(sender As Object, e As EventArgs)
            Dim ButtonDelete As Button = DirectCast(sender, Button)
            Dim dtgItem As GridViewRow = DirectCast(ButtonDelete.NamingContainer, GridViewRow)
            Dim LabelDoctorId As Label = DirectCast(dtgItem.Cells(0).FindControl("LabelDoctorId"), Label)

            Dim objBLDoctorSetting As New BLDoctorSetting()
            Try
                objBLDoctorSetting.DeleteDoctorInfo(ConVisitel, Convert.ToInt32(LabelDoctorId.Text), UserId)
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DeleteMessage)
            Catch ex As Exception
                If (ex.Message.Contains("REFERENCE constraint")) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("Unable to delete Doctor Data. Message: {0}",
                                                                                                 "This Doctor is already used."))
                Else
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to delete Doctor Data. Message: {0}", ex.Message))
                End If
            Finally
                objBLDoctorSetting = Nothing
            End Try

            BindDoctorInformationGridView()
            HiddenFieldIsNew.Value = Convert.ToString(False, Nothing)
            HiddenFieldDoctorId.Value = Convert.ToString(Int32.MinValue)
        End Sub

        Private Sub ButtonSave_Click(sender As Object, e As EventArgs)
            If ValidateData() Then
                Dim objDoctorSettingDataObject As New DoctorSettingDataObject()

                objDoctorSettingDataObject.UPinNumber = Convert.ToString(TextBoxUPinNumber.Text, Nothing).Trim()
                objDoctorSettingDataObject.LicNumber = Convert.ToString(TextBoxLicNumber.Text, Nothing).Trim()
                objDoctorSettingDataObject.FirstName = Convert.ToString(TextBoxFirstName.Text, Nothing).Trim()
                objDoctorSettingDataObject.LastName = Convert.ToString(TextBoxLastName.Text, Nothing).Trim()
                objDoctorSettingDataObject.Address = Convert.ToString(TextBoxAddress.Text, Nothing).Trim()
                objDoctorSettingDataObject.Suite = Convert.ToString(TextBoxSuite.Text, Nothing).Trim()
                objDoctorSettingDataObject.CityId = Convert.ToInt32(DropDownListCity.SelectedValue)
                objDoctorSettingDataObject.StateId = Convert.ToInt32(DropDownListState.SelectedValue)
                objDoctorSettingDataObject.Zip = Convert.ToString(TextBoxZip.Text, Nothing).Trim()
                objDoctorSettingDataObject.Phone = Convert.ToString(TextBoxPhone.Text, Nothing).Trim()
                objDoctorSettingDataObject.Fax = Convert.ToString(TextBoxFax.Text, Nothing).Trim()
                objDoctorSettingDataObject.Status = Convert.ToInt16(DropDownListStatus.SelectedValue)


                Dim objBLDoctorSetting As New BLDoctorSetting()

                Select Case Convert.ToBoolean(HiddenFieldIsNew.Value, Nothing)
                    Case True
                        objDoctorSettingDataObject.UserId = UserId
                        objDoctorSettingDataObject.CompanyId = CompanyId
                        Try
                            objBLDoctorSetting.InsertDoctorInfo(ConVisitel, objDoctorSettingDataObject)

                        Catch ex As SqlException
                            If ex.Message.Contains("Violation of UNIQUE KEY constraint 'UQ_UpinNo'") Then
                                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DuplicateUpinMessage)
                                Return
                            End If
                            If ex.Message.Contains("Violation of UNIQUE KEY constraint 'UQ_LicNo'") Then
                                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DuplicateLicenseMessage)
                                Return
                            End If

                        End Try
                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(InsertMessage)
                        Exit Select
                    Case False
                        objDoctorSettingDataObject.DoctorId = Convert.ToInt32(HiddenFieldDoctorId.Value)
                        objDoctorSettingDataObject.UpdateBy = Convert.ToString(UserId)
                        Try
                            objBLDoctorSetting.UpdateDoctorInfo(ConVisitel, objDoctorSettingDataObject)
                        Catch ex As SqlException
                            If ex.Message.Contains("Violation of UNIQUE KEY constraint 'UQ_UpinNo'") Then
                                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DuplicateUpinMessage)
                                Return
                            End If
                            If ex.Message.Contains("Violation of UNIQUE KEY constraint 'UQ_LicNo'") Then
                                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DuplicateLicenseMessage)
                                Return
                            End If

                        End Try
                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(UpdateMessage)
                        Exit Select
                End Select
                objBLDoctorSetting = Nothing

                HiddenFieldIsNew.Value = Convert.ToString(True, Nothing)
                HiddenFieldDoctorId.Value = Convert.ToString(Int32.MinValue)
                BindDoctorInformationGridView()
            End If
        End Sub

        Private Sub ButtonClear_Click(sender As Object, e As EventArgs)
            ClearControl()
        End Sub

        Private Sub ButtonSearch_Click(sender As Object, e As EventArgs)
            HiddenFieldIsSearched.Value = Convert.ToString(True, Nothing)
            BindDoctorInformationGridView()
            HiddenFieldIsSearched.Value = Convert.ToString(False, Nothing)
        End Sub

        Private Sub GridViewDoctorInformation_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
            GridViewDoctorInformation.PageIndex = e.NewPageIndex
            BindDoctorInformationGridView()
        End Sub

        Private Sub GridViewDoctorInformation_RowDataBound(sender As [Object], e As GridViewRowEventArgs)
            If e.Row.RowType.Equals(DataControlRowType.DataRow) Then

                Dim LabelPhone As Label = DirectCast(e.Row.Cells(0).FindControl("LabelPhone"), Label)
                LabelPhone.Text = Convert.ToString(LabelPhone.Text.Trim(), Nothing)

                Dim LabelFax As Label = DirectCast(e.Row.Cells(0).FindControl("LabelFax"), Label)
                LabelFax.Text = Convert.ToString(LabelFax.Text.Trim(), Nothing)

                Dim LabelStatus As Label = DirectCast(e.Row.Cells(0).FindControl("LabelStatus"), Label)
                LabelStatus.Text = [Enum].GetName(GetType(EnumDataObject.STATUS), Convert.ToInt16(LabelStatus.Text))

                Dim LinkButtonEdit As LinkButton = DirectCast(e.Row.Cells(0).FindControl("LinkButtonEdit"), LinkButton)
                LinkButtonEdit.Text = EditText
                LinkButtonEdit.CssClass = "ui-state-default ui-corner-all buttonLink"

                Dim ButtonDelete As Button = DirectCast(e.Row.Cells(0).FindControl("ButtonDelete"), Button)
                ButtonDelete.Text = DeleteText
            End If
        End Sub

        ''' <summary>
        ''' This is for control events regristering and instantiate object globally
        ''' </summary>
        Private Sub InitializeControl()
            objShared.SetControlTextLength(TextBoxUPinNumber, 20)
            objShared.SetControlTextLength(TextBoxLicNumber, 20)
            objShared.SetControlTextLength(TextBoxFirstName, 20)
            objShared.SetControlTextLength(TextBoxLastName, 20)

            objShared.SetControlTextLength(TextBoxAddress, 50)
            objShared.SetControlTextLength(TextBoxSuite, 50)

            objShared.SetControlTextLength(TextBoxZip, 12)

            objShared.SetControlTextLength(TextBoxPhone, 16)
            objShared.SetControlTextLength(TextBoxFax, 16)

            AddHandler ButtonSave.Click, AddressOf ButtonSave_Click
            AddHandler ButtonClear.Click, AddressOf ButtonClear_Click
            AddHandler ButtonSearch.Click, AddressOf ButtonSearch_Click

            AddHandler GridViewDoctorInformation.PageIndexChanging, AddressOf GridViewDoctorInformation_PageIndexChanging
            AddHandler GridViewDoctorInformation.RowDataBound, AddressOf GridViewDoctorInformation_RowDataBound

            TextBoxPhone.ClientIDMode = ClientIDMode.Static
            TextBoxFax.ClientIDMode = ClientIDMode.Static

            GridViewDoctorInformation.AutoGenerateColumns = False
            GridViewDoctorInformation.ShowHeaderWhenEmpty = True
            GridViewDoctorInformation.AllowPaging = True
            GridViewDoctorInformation.PageSize = objShared.GridViewDefaultPageSize

            ButtonSave.ClientIDMode = ClientIDMode.Static
            ButtonClear.ClientIDMode = ClientIDMode.Static
            ButtonSearch.ClientIDMode = ClientIDMode.Static
        End Sub

        Private Sub LoadJavascript()
            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                              & " var DeleteDialogHeader ='Doctor'; " _
                              & " var DeleteDialogConfirmMsg ='Do you want to delete this record?'; " _
                              & " var LoaderImagePath ='" & objShared.GetLoaderImagePath & "'; " _
                              & " var prm =''; " _
                              & " jQuery(document).ready(function () {" _
                              & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                              & "     InputMasking();" _
                              & "     prm.add_endRequest(InputMasking); " _
                              & "     prm.add_beginRequest(SetButtonActionProgress); " _
                              & "     prm.add_endRequest(EndRequest); " _
                              & "}); " _
                          & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

            LoadJS("JavaScript/Settings/" & ControlName & ".js")
        End Sub

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource()
            Dim ResourceTable As Hashtable = objShared.GetResourceValue("Setup", ControlName & Convert.ToString(".resx"))

            LabelDoctorInformationEntry.Text = Convert.ToString(ResourceTable("LabelDoctorInformationEntry"), Nothing)
            LabelDoctorInformationEntry.Text = If(String.IsNullOrEmpty(LabelDoctorInformationEntry.Text), "Doctor Information Entry", LabelDoctorInformationEntry.Text)

            LabelUPinNumber.Text = Convert.ToString(ResourceTable("LabelUPinNumber"), Nothing)
            LabelUPinNumber.Text = If(String.IsNullOrEmpty(LabelUPinNumber.Text), "UPIN #", LabelUPinNumber.Text)

            LabelLicNumber.Text = Convert.ToString(ResourceTable("LabelLicNumber"), Nothing)
            LabelLicNumber.Text = If(String.IsNullOrEmpty(LabelLicNumber.Text), "Lic #", LabelLicNumber.Text)

            LabelFirstName.Text = Convert.ToString(ResourceTable("LabelFirstName"), Nothing)
            LabelFirstName.Text = If(String.IsNullOrEmpty(LabelFirstName.Text), "First Name", LabelFirstName.Text)

            LabelLastName.Text = Convert.ToString(ResourceTable("LabelLastName"), Nothing)
            LabelLastName.Text = If(String.IsNullOrEmpty(LabelLastName.Text), "Last Name", LabelLastName.Text)

            LabelAddress.Text = Convert.ToString(ResourceTable("LabelAddress"), Nothing)
            LabelAddress.Text = If(String.IsNullOrEmpty(LabelAddress.Text), "Address", LabelAddress.Text)

            LabelSuite.Text = Convert.ToString(ResourceTable("LabelSuite"), Nothing)
            LabelSuite.Text = If(String.IsNullOrEmpty(LabelSuite.Text), "Suite", LabelSuite.Text)

            LabelCity.Text = Convert.ToString(ResourceTable("LabelCity"), Nothing)
            LabelCity.Text = If(String.IsNullOrEmpty(LabelCity.Text), "City", LabelCity.Text)

            LabelState.Text = Convert.ToString(ResourceTable("LabelState"), Nothing)
            LabelState.Text = If(String.IsNullOrEmpty(LabelState.Text), "State", LabelState.Text)

            LabelZip.Text = Convert.ToString(ResourceTable("LabelZip"), Nothing)
            LabelZip.Text = If(String.IsNullOrEmpty(LabelZip.Text), "Zip", LabelZip.Text)

            LabelPhone.Text = Convert.ToString(ResourceTable("LabelPhone"), Nothing)
            LabelPhone.Text = If(String.IsNullOrEmpty(LabelPhone.Text), "Phone", LabelPhone.Text)

            LabelFax.Text = Convert.ToString(ResourceTable("LabelFax"), Nothing)
            LabelFax.Text = If(String.IsNullOrEmpty(LabelFax.Text), "Fax", LabelFax.Text)

            LabelStatus.Text = Convert.ToString(ResourceTable("LabelStatus"), Nothing)
            LabelStatus.Text = If(String.IsNullOrEmpty(LabelStatus.Text), "Status", LabelStatus.Text)

            LabelSearchDoctorInfo.Text = Convert.ToString(ResourceTable("LabelSearchDoctorInfo"), Nothing)
            LabelSearchDoctorInfo.Text = If(String.IsNullOrEmpty(LabelSearchDoctorInfo.Text), "Search Doctor Information", LabelSearchDoctorInfo.Text)

            LabelDoctorInformationList.Text = Convert.ToString(ResourceTable("LabelDoctorInformationList"), Nothing)
            LabelDoctorInformationList.Text = If(String.IsNullOrEmpty(LabelDoctorInformationList.Text), "Doctor Information List", LabelDoctorInformationList.Text)

            LabelSearchByUpinNumber.Text = LabelUPinNumber.Text
            LabelSearchByLicNumber.Text = LabelLicNumber.Text

            ButtonSave.Text = Convert.ToString(ResourceTable("ButtonSave"), Nothing)
            ButtonSave.Text = If(String.IsNullOrEmpty(ButtonSave.Text), "Save", ButtonSave.Text)

            ButtonClear.Text = Convert.ToString(ResourceTable("ButtonClear"), Nothing)
            ButtonClear.Text = If(String.IsNullOrEmpty(ButtonClear.Text), "Clear", ButtonClear.Text)

            ButtonSearch.Text = Convert.ToString(ResourceTable("ButtonSearch"), Nothing)
            ButtonSearch.Text = If(String.IsNullOrEmpty(ButtonSearch.Text), "Search", ButtonSearch.Text)

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

            CitySelectionMessage = Convert.ToString(ResourceTable("CitySelectionMessage"), Nothing)
            CitySelectionMessage = If(String.IsNullOrEmpty(CitySelectionMessage), "Please select city", CitySelectionMessage)

            StateSelectionMessage = Convert.ToString(ResourceTable("StateSelectionMessage"), Nothing)
            StateSelectionMessage = If(String.IsNullOrEmpty(StateSelectionMessage), "Please select state", StateSelectionMessage)

            StatusSelectionMessage = Convert.ToString(ResourceTable("StatusSelectionMessage"), Nothing)
            StatusSelectionMessage = If(String.IsNullOrEmpty(StatusSelectionMessage), "Please select status", StatusSelectionMessage)

            DuplicateUpinMessage = Convert.ToString(ResourceTable("DuplicateUpinMessage"), Nothing)
            DuplicateUpinMessage = If(String.IsNullOrEmpty(DuplicateUpinMessage), "The same Upin Number already exists.", DuplicateUpinMessage)

            DuplicateLicenseMessage = Convert.ToString(ResourceTable("DuplicateLicenseMessage"), Nothing)
            DuplicateLicenseMessage = If(String.IsNullOrEmpty(DuplicateLicenseMessage), "The same License Number already exists.", DuplicateLicenseMessage)

            InvalidPhoneMessage = Convert.ToString(ResourceTable("InvalidPhoneMessage"), Nothing)
            InvalidPhoneMessage = If(String.IsNullOrEmpty(InvalidPhoneMessage), "Invalid Phone", InvalidPhoneMessage)

            InvalidFaxMessage = Convert.ToString(ResourceTable("InvalidFaxMessage"), Nothing)
            InvalidFaxMessage = If(String.IsNullOrEmpty(InvalidFaxMessage), "Invalid Fax", InvalidFaxMessage)

            ResourceTable = Nothing
        End Sub

        ''' <summary>
        ''' This is for validating data in server side before submitting the form
        ''' </summary>
        ''' <returns></returns>
        Private Function ValidateData() As Boolean
            If DropDownListCity.SelectedValue = "-1" Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(CitySelectionMessage)
                Return False
            End If
            If DropDownListStatus.SelectedValue = "-1" Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(StatusSelectionMessage)
                Return False
            End If
            If DropDownListState.SelectedValue = "-1" Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(StateSelectionMessage)
                Return False
            End If

            If (Not objShared.ValidatePhone(TextBoxPhone.Text.Trim)) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(InvalidPhoneMessage)
                Return False
            End If

            If (Not objShared.ValidatePhone(TextBoxFax.Text.Trim)) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(InvalidFaxMessage)
                Return False
            End If

            Return True
        End Function

        ''' <summary>
        ''' This is for retrieving data
        ''' </summary>
        Private Sub GetData()
            objShared.BindStatusDropDownList(DropDownListStatus)
            objShared.BindCityDropDownList(DropDownListCity, CompanyId)
            objShared.BindStateDropDownList(DropDownListState, CompanyId)
            BindDoctorInformationGridView()
        End Sub

        ''' <summary>
        ''' Clearing controls value
        ''' </summary>
        Private Sub ClearControl()

            TextBoxUPinNumber.Text = objShared.InlineAssignHelper(TextBoxLicNumber.Text, objShared.InlineAssignHelper(TextBoxFirstName.Text,
                                     objShared.InlineAssignHelper(TextBoxLastName.Text, objShared.InlineAssignHelper(TextBoxAddress.Text,
                                     objShared.InlineAssignHelper(TextBoxSuite.Text, objShared.InlineAssignHelper(TextBoxZip.Text,
                                     objShared.InlineAssignHelper(TextBoxPhone.Text, objShared.InlineAssignHelper(TextBoxFax.Text,
                                     objShared.InlineAssignHelper(TextBoxSearchByUpinNumber.Text, objShared.InlineAssignHelper(TextBoxSearchByLicNumber.Text,
                                                                                                                               String.Empty))))))))))

            DropDownListCity.SelectedIndex = objShared.InlineAssignHelper(DropDownListState.SelectedIndex, 0)

            DropDownListStatus.SelectedIndex = 1

            HiddenFieldIsNew.Value = Convert.ToString(True, Nothing)
            HiddenFieldIsSearched.Value = Convert.ToString(False, Nothing)
            HiddenFieldDoctorId.Value = Convert.ToString(Int32.MinValue)

        End Sub

        ''' <summary>
        ''' Binding Doctor Information GridView
        ''' </summary>
        Private Sub BindDoctorInformationGridView()

            Dim DoctorSettingList As List(Of DoctorSettingDataObject)

            Dim objBLDoctorSetting As New BLDoctorSetting()

            DoctorSettingList = If((Convert.ToBoolean(HiddenFieldIsSearched.Value, Nothing)),
                                    objBLDoctorSetting.SearchDoctorInfo(ConVisitel, CompanyId, TextBoxSearchByUpinNumber.Text.Trim(), TextBoxSearchByLicNumber.Text.Trim()),
                                    objBLDoctorSetting.SelectDoctorInfo(ConVisitel, CompanyId))

            objBLDoctorSetting = Nothing

            GridViewDoctorInformation.DataSource = DoctorSettingList
            GridViewDoctorInformation.DataBind()

            DoctorSettingList = Nothing
        End Sub

    End Class
End Namespace