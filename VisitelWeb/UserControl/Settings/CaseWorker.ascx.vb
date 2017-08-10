#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Case Worker Setup
' Author: Anjan Kumar Paul
' Start Date: 19 Aug 2014
' End Date: 19 Aug 2014
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                19 Aug 2014     Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness.Settings
Imports System.Data.SqlClient
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelBusiness.VisitelBusiness.DataObject.Settings

Namespace Visitel.UserControl.Settings
    Public Class CaseWorkerSetting
        Inherits BaseUserControl

        Private ControlName As String, EditText As String, DeleteText As String, InsertMessage As String, UpdateMessage As String, DeleteMessage As String, _
            StatusSelectionMessage As String, CitySelectionMessage As String, DuplicateCaseWorkerMessage As String, SaveConfirmMessage As String, _
            InvalidEmailMessage As String, InvalidPhoneMessage As String, InvalidFaxMessage As String

        Private UserId As Integer
        Private CompanyId As Integer
        Private ConVisitel As SqlConnection
        Private objShared As SharedWebControls

        Protected Sub Page_Init(sender As Object, e As EventArgs)

            ControlName = "CaseWorkerSetting"
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

            TextBoxFirstName.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelFirstName"), Label).Text, Nothing)
            TextBoxLastName.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelLastName"), Label).Text, Nothing)

            Dim Phone As String = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelPhone"), Label).Text, Nothing)
            TextBoxPhone.Text = Convert.ToString(Phone, Nothing)

            Dim Fax As String = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelFax"), Label).Text, Nothing)
            TextBoxFax.Text = Convert.ToString(Fax, Nothing)

            TextBoxMailCode.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelMailCode"), Label).Text, Nothing)
            TextBoxEmail.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelEmail"), Label).Text, Nothing)
            TextBoxAddress.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelAddress"), Label).Text, Nothing)
            TextBoxComments.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelComments"), Label).Text, Nothing)

            DropDownListStatus.SelectedIndex = DropDownListStatus.Items.IndexOf(DropDownListStatus.Items.FindByText(
                                                Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelStatus"), Label).Text, Nothing)))

            'DropDownListCity.SelectedIndex = DropDownListCity.Items.IndexOf(DropDownListCity.Items.FindByValue(Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelCityId"), Label).Text, Nothing)))

            HiddenFieldCaseWorkerId.Value = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelCaseWorkerId"), Label).Text, Nothing)

            HiddenFieldIsNew.Value = Convert.ToString(False, Nothing)

            ButtonSaveWithConfirmation.Visible = False
            ButtonSave.Visible = True

        End Sub

        Protected Sub ButtonDelete_Click(sender As Object, e As EventArgs)
            Dim ButtonDelete As Button = DirectCast(sender, Button)
            Dim dtgItem As GridViewRow = DirectCast(ButtonDelete.NamingContainer, GridViewRow)
            Dim LabelCaseWorkerId As Label = DirectCast(dtgItem.Cells(0).FindControl("LabelCaseWorkerId"), Label)

            Dim objBLCaseWorkerSetting As New BLCaseWorkerSetting()

            Try
                objBLCaseWorkerSetting.DeleteCaseWorkerInfo(ConVisitel, Convert.ToInt32(LabelCaseWorkerId.Text), UserId)
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DeleteMessage)
            Catch ex As Exception
                If (ex.Message.Contains("REFERENCE constraint")) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("Unable to delete Case Worker Data. Message: {0}",
                                                                                                 "This caseworker is already used."))
                Else
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to delete Case Worker Data. Message: {0}", ex.Message))
                End If
            Finally
                objBLCaseWorkerSetting = Nothing
            End Try

            BindCaseWorkerInformationGridView()
            HiddenFieldIsNew.Value = Convert.ToString(False, Nothing)
            HiddenFieldCaseWorkerId.Value = Convert.ToString(Int32.MinValue)
        End Sub

        Private Sub ButtonSaveWithConfirmation_Click(sender As Object, e As EventArgs)
            SaveData(True)
            ButtonSaveWithConfirmation.Visible = False
            ButtonSave.Visible = True
        End Sub
        Private Sub ButtonSave_Click(sender As Object, e As EventArgs)
            SaveData(False)
        End Sub

        Private Sub ButtonClear_Click(sender As Object, e As EventArgs)
            ClearControl()
        End Sub

        Private Sub ButtonSearch_Click(sender As Object, e As EventArgs)
            HiddenFieldIsSearched.Value = Convert.ToString(True, Nothing)
            BindCaseWorkerInformationGridView()
            HiddenFieldIsSearched.Value = Convert.ToString(False, Nothing)
        End Sub

        Private Sub GridViewCaseWorkerInformation_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
            GridViewCaseWorkerInformation.PageIndex = e.NewPageIndex
            BindCaseWorkerInformationGridView()
        End Sub

        Private Sub GridViewCaseWorkerInformation_RowDataBound(sender As [Object], e As GridViewRowEventArgs)
            If e.Row.RowType.Equals(DataControlRowType.DataRow) Then

                'Dim LabelPhone As Label = DirectCast(e.Row.Cells(0).FindControl("LabelPhone"), Label)
                'LabelPhone.Text = objShared.GetFormattedMobileNumber(Convert.ToString(LabelPhone.Text.Trim(), Nothing))

                'Dim LabelFax As Label = DirectCast(e.Row.Cells(0).FindControl("LabelFax"), Label)
                'LabelFax.Text = objShared.GetFormattedMobileNumber(Convert.ToString(LabelFax.Text.Trim(), Nothing))

                Dim LabelStatus As Label = DirectCast(e.Row.Cells(0).FindControl("LabelStatus"), Label)
                LabelStatus.Text = [Enum].GetName(GetType(EnumDataObject.STATUS), Convert.ToInt16(LabelStatus.Text))

                Dim LinkButtonEdit As LinkButton = DirectCast(e.Row.Cells(0).FindControl("LinkButtonEdit"), LinkButton)
                LinkButtonEdit.Text = EditText
                LinkButtonEdit.CssClass = "ui-state-default ui-corner-all buttonLink"

                Dim ButtonDelete As Button = DirectCast(e.Row.Cells(0).FindControl("ButtonDelete"), Button)
                ButtonDelete.Text = DeleteText
            End If
        End Sub

        Private Sub LoadJavascript()

            'LoadJS("JavaScript/jquery-1.8.3.min.js")

            'LoadJS("JavaScript/CommonFunctions.js")

            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                              & " var DeleteDialogHeader ='Case Worker'; " _
                              & " var DeleteDialogConfirmMsg ='Do you want to delete this record?'; " _
                              & " var LoaderImagePath ='" & objShared.GetLoaderImagePath & "'; " _
                              & " var prm =''; " _
                              & " jQuery(document).ready(function () {" _
                              & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                              & "     prm.add_beginRequest(SetButtonActionProgress); " _
                              & "     prm.add_endRequest(EndRequest); " _
                              & "     InputMasking();" _
                              & "     prm.add_endRequest(InputMasking); " _
                              & "}); " _
                          & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

            LoadJS("JavaScript/Settings/" & ControlName & ".js")
        End Sub

        Private Sub SaveData(IsConfirm As Boolean)

            If ValidateData() Then
                Dim objCaseWorkerSettingDataObject As New CaseWorkerSettingDataObject()

                objCaseWorkerSettingDataObject.FirstName = Convert.ToString(TextBoxFirstName.Text, Nothing).Trim()
                objCaseWorkerSettingDataObject.LastName = Convert.ToString(TextBoxLastName.Text, Nothing).Trim()
                objCaseWorkerSettingDataObject.Address = Convert.ToString(TextBoxAddress.Text, Nothing).Trim()
                objCaseWorkerSettingDataObject.Phone = Convert.ToString(TextBoxPhone.Text, Nothing).Trim()
                objCaseWorkerSettingDataObject.Fax = Convert.ToString(TextBoxFax.Text, Nothing).Trim()
                objCaseWorkerSettingDataObject.Status = Convert.ToInt16(DropDownListStatus.SelectedValue)
                'objCaseWorkerSettingDataObject.CityId = Convert.ToInt32(DropDownListCity.SelectedValue)
                objCaseWorkerSettingDataObject.MailCode = Convert.ToString(TextBoxMailCode.Text, Nothing).Trim()
                objCaseWorkerSettingDataObject.Email = Convert.ToString(TextBoxEmail.Text, Nothing).Trim()
                objCaseWorkerSettingDataObject.Address = Convert.ToString(TextBoxAddress.Text, Nothing).Trim()
                objCaseWorkerSettingDataObject.Comments = Convert.ToString(TextBoxComments.Text, Nothing).Trim()

                Dim objBLCaseWorkerSetting As New BLCaseWorkerSetting()

                Select Case Convert.ToBoolean(HiddenFieldIsNew.Value, Nothing)
                    Case True
                        objCaseWorkerSettingDataObject.UserId = UserId
                        objCaseWorkerSettingDataObject.CompanyId = CompanyId

                        Try
                            objBLCaseWorkerSetting.InsertCaseWorkerInfo(ConVisitel, objCaseWorkerSettingDataObject, IsConfirm)

                        Catch ex As SqlException
                            If ex.Message.Contains("Duplicate Case Worker Code") Then
                                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DuplicateCaseWorkerMessage)
                                ButtonSaveWithConfirmation.Visible = True
                                ButtonSave.Visible = False

                                Dim scriptBlock As String = Nothing

                                scriptBlock = "<script type='text/javascript'> " _
                                  & " var CustomTargetButton ='ButtonSaveWithConfirmation'; " _
                                  & " var CustomDialogHeader ='Case Worker: Save Forcefully'; " _
                                  & " var CustomDialogConfirmMsg ='" & SaveConfirmMessage & "'; " _
                                  & " var prm =''; " _
                                  & " jQuery(document).ready(function () {" _
                                  & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                                  & "     CaseWorkerSave();" _
                                  & "     prm.add_endRequest(CaseWorkerSave); " _
                                  & "}); " _
                                  & "</script>"

                                Page.Header.Controls.Add(New LiteralControl(scriptBlock))

                                Return
                            End If
                        End Try
                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(InsertMessage)
                        Exit Select
                    Case False
                        objCaseWorkerSettingDataObject.CaseWorkerId = Convert.ToInt32(HiddenFieldCaseWorkerId.Value)
                        objCaseWorkerSettingDataObject.UpdateBy = Convert.ToString(UserId)

                        Try
                            objBLCaseWorkerSetting.UpdateCaseWorkerInfo(ConVisitel, objCaseWorkerSettingDataObject, IsConfirm)

                        Catch ex As SqlException
                            If ex.Message.Contains("Duplicate Case Worker Code") Then
                                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DuplicateCaseWorkerMessage)
                                ButtonSaveWithConfirmation.Visible = True
                                ButtonSave.Visible = False

                                Dim scriptBlock As String = Nothing

                                scriptBlock = "<script type='text/javascript'> " _
                                  & " var CustomTargetButton ='ButtonSaveWithConfirmation'; " _
                                  & " var CustomDialogHeader ='Case Worker: Save Forcefully'; " _
                                  & " var CustomDialogConfirmMsg ='" & SaveConfirmMessage & "'; " _
                                  & " var prm =''; " _
                                  & " jQuery(document).ready(function () {" _
                                  & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                                  & "     CaseWorkerSave();" _
                                  & "     prm.add_endRequest(CaseWorkerSave); " _
                                  & "}); " _
                                  & "</script>"

                                Page.Header.Controls.Add(New LiteralControl(scriptBlock))

                                Return
                            End If
                        End Try
                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(UpdateMessage)
                        Exit Select
                End Select
                objBLCaseWorkerSetting = Nothing

                HiddenFieldIsNew.Value = Convert.ToString(True, Nothing)
                HiddenFieldCaseWorkerId.Value = Convert.ToString(Int32.MinValue)
                BindCaseWorkerInformationGridView()
            End If
        End Sub

        ''' <summary>
        ''' This is for control events regristering and instantiate object globally
        ''' </summary>
        Private Sub InitializeControl()

            objShared.SetControlTextLength(TextBoxMailCode, 50)
            objShared.SetControlTextLength(TextBoxAddress, 50)

            objShared.SetControlTextLength(TextBoxComments, 250)

            objShared.SetControlTextLength(TextBoxFirstName, 20)
            objShared.SetControlTextLength(TextBoxLastName, 20)

            objShared.SetControlTextLength(TextBoxPhone, 16)
            objShared.SetControlTextLength(TextBoxFax, 16)

            AddHandler ButtonSave.Click, AddressOf ButtonSave_Click
            AddHandler ButtonSaveWithConfirmation.Click, AddressOf ButtonSaveWithConfirmation_Click
            AddHandler ButtonClear.Click, AddressOf ButtonClear_Click
            AddHandler ButtonSearch.Click, AddressOf ButtonSearch_Click
            AddHandler GridViewCaseWorkerInformation.PageIndexChanging, AddressOf GridViewCaseWorkerInformation_PageIndexChanging
            AddHandler GridViewCaseWorkerInformation.RowDataBound, AddressOf GridViewCaseWorkerInformation_RowDataBound

            ButtonSaveWithConfirmation.Visible = False

            TextBoxPhone.ClientIDMode = ClientIDMode.Static
            TextBoxSearchByPhone.ClientIDMode = ClientIDMode.Static
            TextBoxFax.ClientIDMode = ClientIDMode.Static
            ButtonSaveWithConfirmation.ClientIDMode = ClientIDMode.Static

            ButtonSave.ClientIDMode = ClientIDMode.Static
            ButtonClear.ClientIDMode = ClientIDMode.Static
            ButtonSearch.ClientIDMode = ClientIDMode.Static

            TextBoxAddress.TextMode = TextBoxMode.MultiLine
        End Sub


        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("Setup", ControlName & Convert.ToString(".resx"))

            LabelCaseWorkerInformationEntry.Text = Convert.ToString(ResourceTable("LabelCaseWorkerInformationEntry"), Nothing)
            LabelCaseWorkerInformationEntry.Text = If(String.IsNullOrEmpty(LabelCaseWorkerInformationEntry.Text),
                                                      "Case Worker Information Entry", LabelCaseWorkerInformationEntry.Text)

            LabelFirstName.Text = Convert.ToString(ResourceTable("LabelFirstName"), Nothing)
            LabelFirstName.Text = If(String.IsNullOrEmpty(LabelFirstName.Text), "First Name", LabelFirstName.Text)
            LabelSearchByFirstName.Text = LabelFirstName.Text

            LabelLastName.Text = Convert.ToString(ResourceTable("LabelLastName"), Nothing)
            LabelLastName.Text = If(String.IsNullOrEmpty(LabelLastName.Text), "Last Name", LabelLastName.Text)
            LabelSearchByLastName.Text = LabelLastName.Text

            LabelAddress.Text = Convert.ToString(ResourceTable("LabelAddress"), Nothing)
            LabelAddress.Text = If(String.IsNullOrEmpty(LabelAddress.Text), "Address", LabelAddress.Text)

            LabelPhone.Text = Convert.ToString(ResourceTable("LabelPhone"), Nothing)
            LabelPhone.Text = If(String.IsNullOrEmpty(LabelPhone.Text), "Phone", LabelPhone.Text)
            LabelSearchByPhone.Text = LabelPhone.Text

            LabelFax.Text = Convert.ToString(ResourceTable("LabelFax"), Nothing)
            LabelFax.Text = If(String.IsNullOrEmpty(LabelFax.Text), "Fax", LabelFax.Text)

            LabelStatus.Text = Convert.ToString(ResourceTable("LabelStatus"), Nothing)
            LabelStatus.Text = If(String.IsNullOrEmpty(LabelStatus.Text), "Status", LabelStatus.Text)

            LabelComments.Text = Convert.ToString(ResourceTable("LabelComments"), Nothing)
            LabelComments.Text = If(String.IsNullOrEmpty(LabelComments.Text), "Comments", LabelComments.Text)

            LabelEmail.Text = Convert.ToString(ResourceTable("LabelEmail"), Nothing)
            LabelEmail.Text = If(String.IsNullOrEmpty(LabelEmail.Text), "Email", LabelEmail.Text)

            LabelMailCode.Text = Convert.ToString(ResourceTable("LabelMailCode"), Nothing)
            LabelMailCode.Text = If(String.IsNullOrEmpty(LabelMailCode.Text), "Mail Code", LabelMailCode.Text)

            'LabelCity.Text = Convert.ToString(ResourceTable("LabelCity"), Nothing)
            'LabelCity.Text = If(String.IsNullOrEmpty(LabelCity.Text), "City", LabelCity.Text)

            LabelSearchCaseWorkerInfo.Text = Convert.ToString(ResourceTable("LabelSearchCaseWorkerInfo"), Nothing)
            LabelSearchCaseWorkerInfo.Text = If(String.IsNullOrEmpty(LabelSearchCaseWorkerInfo.Text), "Search Case Worker Information", LabelSearchCaseWorkerInfo.Text)


            LabelCaseWorkerInformationList.Text = Convert.ToString(ResourceTable("LabelCaseWorkerInformationList"), Nothing)
            LabelCaseWorkerInformationList.Text = If(String.IsNullOrEmpty(LabelCaseWorkerInformationList.Text),
                                                     "Case Worker Information List", LabelCaseWorkerInformationList.Text)

            ButtonSave.Text = Convert.ToString(ResourceTable("ButtonSave"), Nothing)
            ButtonSave.Text = If(String.IsNullOrEmpty(ButtonSave.Text), "Save", ButtonSave.Text)

            ButtonSaveWithConfirmation.Text = ButtonSave.Text

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

            StatusSelectionMessage = Convert.ToString(ResourceTable("StatusSelectionMessage"), Nothing)
            StatusSelectionMessage = If(String.IsNullOrEmpty(StatusSelectionMessage), "Please select status", StatusSelectionMessage)

            CitySelectionMessage = Convert.ToString(ResourceTable("CitySelectionMessage"), Nothing)
            CitySelectionMessage = If(String.IsNullOrEmpty(CitySelectionMessage), "Please select city", CitySelectionMessage)

            DuplicateCaseWorkerMessage = Convert.ToString(ResourceTable("DuplicateCaseWorkerMessage"), Nothing)
            DuplicateCaseWorkerMessage = If(String.IsNullOrEmpty(DuplicateCaseWorkerMessage),
                                            "This Case Worker already exists along with the first name, last name and phone.", DuplicateCaseWorkerMessage)

            SaveConfirmMessage = Convert.ToString(ResourceTable("SaveConfirmMessage"), Nothing)
            SaveConfirmMessage = If(String.IsNullOrEmpty(SaveConfirmMessage), "Are you sure?", SaveConfirmMessage)

            InvalidEmailMessage = Convert.ToString(ResourceTable("InvalidEmailMessage"), Nothing)
            InvalidEmailMessage = If(String.IsNullOrEmpty(InvalidEmailMessage), "Invalid Email", InvalidEmailMessage)

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

            If DropDownListStatus.SelectedValue = "-1" Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(StatusSelectionMessage)
                Return False
            End If

            'If DropDownListCity.SelectedValue = "-1" Then
            '    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(CitySelectionMessage)
            '    Return False
            'End If

            If (Not objShared.ValidateEmail(TextBoxEmail.Text.Trim)) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(InvalidEmailMessage)
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
            'objShared.BindCityDropDownList(DropDownListCity, CompanyId)
            BindCaseWorkerInformationGridView()
        End Sub

        ''' <summary>
        ''' Clearing controls value
        ''' </summary>
        Private Sub ClearControl()

            TextBoxFirstName.Text = objShared.InlineAssignHelper(TextBoxLastName.Text, objShared.InlineAssignHelper(TextBoxPhone.Text,
                                    objShared.InlineAssignHelper(TextBoxFax.Text, objShared.InlineAssignHelper(TextBoxMailCode.Text,
                                    objShared.InlineAssignHelper(TextBoxEmail.Text, objShared.InlineAssignHelper(TextBoxAddress.Text,
                                    objShared.InlineAssignHelper(TextBoxComments.Text, objShared.InlineAssignHelper(TextBoxSearchByFirstName.Text,
                                    objShared.InlineAssignHelper(TextBoxSearchByLastName.Text, objShared.InlineAssignHelper(TextBoxSearchByPhone.Text, String.Empty))))))))))

            'DropDownListCity.SelectedIndex = objShared.InlineAssignHelper(DropDownListStatus.SelectedIndex, 0)

            DropDownListStatus.SelectedIndex = 1

            HiddenFieldIsNew.Value = Convert.ToString(True, Nothing)
            HiddenFieldIsSearched.Value = Convert.ToString(False, Nothing)
            HiddenFieldCaseWorkerId.Value = Convert.ToString(Int32.MinValue)

            ButtonSaveWithConfirmation.Visible = False
            ButtonSave.Visible = True
        End Sub

        ''' <summary>
        ''' Binding Doctor Information GridView
        ''' </summary>
        Private Sub BindCaseWorkerInformationGridView()

            Dim objBLCaseWorkerSetting As New BLCaseWorkerSetting()

            Dim CaseWorkerSettingList As New List(Of CaseWorkerSettingDataObject)

            CaseWorkerSettingList = If((Convert.ToBoolean(HiddenFieldIsSearched.Value, Nothing)),
                                       objBLCaseWorkerSetting.SearchCaseWorkerInfo(ConVisitel, CompanyId, TextBoxSearchByFirstName.Text.Trim(),
                                       TextBoxSearchByLastName.Text.Trim(), TextBoxSearchByPhone.Text.Trim()),
                                        objBLCaseWorkerSetting.SelectCaseWorkerInfo(ConVisitel, CompanyId))

            objBLCaseWorkerSetting = Nothing

            GridViewCaseWorkerInformation.DataSource = CaseWorkerSettingList
            GridViewCaseWorkerInformation.DataBind()

            CaseWorkerSettingList = Nothing

        End Sub

    End Class
End Namespace