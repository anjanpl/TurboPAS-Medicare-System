Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Collections
Imports System.Data.SqlClient
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports System.Web.UI.WebControls
Imports VisitelBusiness.VisitelBusiness.DataObject.Settings
Imports System.Collections.Generic
Imports VisitelBusiness.VisitelBusiness.Settings

Namespace VisitelCommon

    Public Class CaseWorkerSetting
        Inherits SharedWebControls

        Protected ControlName As String, EditText As String, DeleteText As String, InsertMessage As String, UpdateMessage As String, DeleteMessage As String, _
            StatusSelectionMessage As String, CitySelectionMessage As String, DuplicateCaseWorkerMessage As String, SaveConfirmMessage As String, _
            InvalidEmailMessage As String, InvalidPhoneMessage As String, InvalidFaxMessage As String
        Private UserId As Integer = 100 'This would come from Login Session
        Private CompanyId As Integer = 1 'This would come from Login Session

#Region "Controls"
        Protected HiddenFieldIsNew As HiddenField, HiddenFieldIsSearched As HiddenField, HiddenFieldCaseWorkerId As HiddenField
        Protected LabelCaseWorkerInformationEntryCaption As Label, LabelFirstName As Label, LabelLastName As Label, LabelAddress As Label, LabelPhone As Label, LabelFax As Label,
            LabelStatus As Label, LabelComments As Label, LabelEmail As Label, LabelMailCode As Label, LabelCity As Label, LabelSearchCaseWorkerInfo As Label,
            LabelSearchByFirstName As Label, LabelSearchByLastName As Label, LabelSearchByPhone As Label, LabelCaseWorkerInformationList As Label
        Protected TextBoxSearchByFirstName As TextBox, TextBoxSearchByLastName As TextBox, TextBoxSearchByPhone As TextBox, TextBoxFirstName As TextBox,
            TextBoxLastName As TextBox, TextBoxPhone As TextBox, TextBoxFax As TextBox, TextBoxMailCode As TextBox, TextBoxEmail As TextBox, TextBoxAddress As TextBox,
            TextBoxComments As TextBox
        Protected DropDownListStatus As DropDownList, DropDownListCity As DropDownList
        Protected ButtonSaveWithConfirmation As Button, ButtonSave As Button, ButtonClear As Button, ButtonSearch As Button
        Protected GridViewCaseWorkerInformation As GridView

        Protected ucCaseWorkerSetting As UserControl
#End Region

        Protected DivInfoMessage As HtmlGenericControl, LabelMessage As Label, DivErrorMesg As HtmlGenericControl, LabelErrMsg As Label

        Protected Sub PageIntitContent()
            ControlName = "CaseWorkerSetting"
            SetControl()

            ConnectionOpen()
            LoadCss("Settings/" + ControlName)
            InitializeControl()
            GetCaptionFromResource()
        End Sub

        Protected Sub SetControl()

            HiddenFieldIsNew = DirectCast(ucCaseWorkerSetting.FindControl("HiddenFieldIsNew"), HiddenField)
            HiddenFieldIsSearched = DirectCast(ucCaseWorkerSetting.FindControl("HiddenFieldIsSearched"), HiddenField)
            HiddenFieldCaseWorkerId = DirectCast(ucCaseWorkerSetting.FindControl("HiddenFieldCaseWorkerId"), HiddenField)

            LabelFirstName = DirectCast(ucCaseWorkerSetting.FindControl("LabelFirstName"), Label)
            LabelLastName = DirectCast(ucCaseWorkerSetting.FindControl("LabelLastName"), Label)
            LabelAddress = DirectCast(ucCaseWorkerSetting.FindControl("LabelAddress"), Label)
            LabelPhone = DirectCast(ucCaseWorkerSetting.FindControl("LabelPhone"), Label)
            LabelFax = DirectCast(ucCaseWorkerSetting.FindControl("LabelFax"), Label)
            LabelStatus = DirectCast(ucCaseWorkerSetting.FindControl("LabelStatus"), Label)
            LabelComments = DirectCast(ucCaseWorkerSetting.FindControl("LabelComments"), Label)
            LabelEmail = DirectCast(ucCaseWorkerSetting.FindControl("LabelEmail"), Label)
            LabelMailCode = DirectCast(ucCaseWorkerSetting.FindControl("LabelMailCode"), Label)
            LabelCity = DirectCast(ucCaseWorkerSetting.FindControl("LabelCity"), Label)
            LabelSearchCaseWorkerInfo = DirectCast(ucCaseWorkerSetting.FindControl("LabelSearchCaseWorkerInfo"), Label)
            LabelSearchByFirstName = DirectCast(ucCaseWorkerSetting.FindControl("LabelSearchByFirstName"), Label)
            LabelSearchByLastName = DirectCast(ucCaseWorkerSetting.FindControl("LabelSearchByLastName"), Label)
            LabelSearchByPhone = DirectCast(ucCaseWorkerSetting.FindControl("LabelSearchByPhone"), Label)
            LabelCaseWorkerInformationList = DirectCast(ucCaseWorkerSetting.FindControl("LabelCaseWorkerInformationList"), Label)

            TextBoxSearchByFirstName = DirectCast(ucCaseWorkerSetting.FindControl("TextBoxSearchByFirstName"), TextBox)
            TextBoxSearchByLastName = DirectCast(ucCaseWorkerSetting.FindControl("TextBoxSearchByLastName"), TextBox)
            TextBoxSearchByPhone = DirectCast(ucCaseWorkerSetting.FindControl("TextBoxSearchByPhone"), TextBox)
            TextBoxFirstName = DirectCast(ucCaseWorkerSetting.FindControl("TextBoxFirstName"), TextBox)
            TextBoxLastName = DirectCast(ucCaseWorkerSetting.FindControl("TextBoxLastName"), TextBox)
            TextBoxPhone = DirectCast(ucCaseWorkerSetting.FindControl("TextBoxPhone"), TextBox)
            TextBoxFax = DirectCast(ucCaseWorkerSetting.FindControl("TextBoxFax"), TextBox)
            TextBoxMailCode = DirectCast(ucCaseWorkerSetting.FindControl("TextBoxMailCode"), TextBox)
            TextBoxEmail = DirectCast(ucCaseWorkerSetting.FindControl("TextBoxEmail"), TextBox)
            TextBoxAddress = DirectCast(ucCaseWorkerSetting.FindControl("TextBoxAddress"), TextBox)
            TextBoxComments = DirectCast(ucCaseWorkerSetting.FindControl("TextBoxComments"), TextBox)

            DropDownListStatus = DirectCast(ucCaseWorkerSetting.FindControl("DropDownListStatus"), DropDownList)
            DropDownListCity = DirectCast(ucCaseWorkerSetting.FindControl("DropDownListCity"), DropDownList)

            ButtonSaveWithConfirmation = DirectCast(ucCaseWorkerSetting.FindControl("ButtonSaveWithConfirmation"), Button)
            ButtonSave = DirectCast(ucCaseWorkerSetting.FindControl("ButtonSave"), Button)
            ButtonClear = DirectCast(ucCaseWorkerSetting.FindControl("ButtonClear"), Button)
            ButtonSearch = DirectCast(ucCaseWorkerSetting.FindControl("ButtonSearch"), Button)

            GridViewCaseWorkerInformation = DirectCast(ucCaseWorkerSetting.FindControl("GridViewCaseWorkerInformation"), GridView)

        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)
            If Not IsPostBack Then
                HiddenFieldIsNew.Value = Convert.ToString(True, Nothing)
                HiddenFieldIsSearched.Value = Convert.ToString(False, Nothing)
                GetData()
            End If
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            ConnectionClose()
        End Sub

        Private Sub LinkButtonEdit_Click(sender As Object, e As EventArgs)
            Dim LinkButtonDelete As LinkButton = DirectCast(sender, LinkButton)
            Dim GridViewItemRow As GridViewRow = DirectCast(LinkButtonDelete.NamingContainer, GridViewRow)

            TextBoxFirstName.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelFirstName"), Label).Text, Nothing)
            TextBoxLastName.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelLastName"), Label).Text, Nothing)

            Dim Phone As String = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelPhone"), Label).Text, Nothing)
            TextBoxPhone.Text = GetReFormattedMobileNumber(Convert.ToString(Phone, Nothing))

            Dim Fax As String = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelFax"), Label).Text, Nothing)
            TextBoxFax.Text = GetReFormattedMobileNumber(Convert.ToString(Fax, Nothing))

            TextBoxMailCode.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelMailCode"), Label).Text, Nothing)
            TextBoxEmail.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelEmail"), Label).Text, Nothing)
            TextBoxAddress.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelAddress"), Label).Text, Nothing)
            TextBoxComments.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelComments"), Label).Text, Nothing)

            DropDownListStatus.SelectedIndex = DropDownListStatus.Items.IndexOf(DropDownListStatus.Items.FindByText(Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelStatus"), Label).Text, Nothing)))
            DropDownListCity.SelectedIndex = DropDownListCity.Items.IndexOf(DropDownListCity.Items.FindByValue(Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelCityId"), Label).Text, Nothing)))

            HiddenFieldCaseWorkerId.Value = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelCaseWorkerId"), Label).Text, Nothing)

            HiddenFieldIsNew.Value = Convert.ToString(False, Nothing)

            ButtonSaveWithConfirmation.Visible = False
            ButtonSave.Visible = True

        End Sub

        Protected Sub LinkButtonDelete_Click(sender As Object, e As EventArgs)
            Dim LinkButtonDelete As LinkButton = DirectCast(sender, LinkButton)
            Dim dtgItem As GridViewRow = DirectCast(LinkButtonDelete.NamingContainer, GridViewRow)
            Dim LabelCaseWorkerId As Label = DirectCast(dtgItem.Cells(0).FindControl("LabelCaseWorkerId"), Label)

            Dim objBLCaseWorkerSetting As New BLCaseWorkerSetting()
            objBLCaseWorkerSetting.DeleteCaseWorkerInfo(ConVisitel, Convert.ToInt32(LabelCaseWorkerId.Text), UserId)
            objBLCaseWorkerSetting = Nothing
            DisplayHeaderMessage(DeleteMessage)

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

                Dim LabelPhone As Label = DirectCast(e.Row.Cells(0).FindControl("LabelPhone"), Label)
                LabelPhone.Text = GetFormattedMobileNumber(Convert.ToString(LabelPhone.Text.Trim(), Nothing))

                Dim LabelFax As Label = DirectCast(e.Row.Cells(0).FindControl("LabelFax"), Label)
                LabelFax.Text = GetFormattedMobileNumber(Convert.ToString(LabelFax.Text.Trim(), Nothing))

                Dim LabelStatus As Label = DirectCast(e.Row.Cells(0).FindControl("LabelStatus"), Label)
                LabelStatus.Text = [Enum].GetName(GetType(EnumDataObject.STATUS), Convert.ToInt16(LabelStatus.Text))

                Dim LinkButtonEdit As LinkButton = DirectCast(e.Row.Cells(0).FindControl("LinkButtonEdit"), LinkButton)
                AddHandler LinkButtonEdit.Click, AddressOf LinkButtonEdit_Click
                LinkButtonEdit.Text = EditText
                LinkButtonEdit.CssClass = "ui-state-default ui-corner-all buttonLink"
                e.Row.Cells(14).Controls.Add(LinkButtonEdit)

                Dim LinkButtonDelete As LinkButton = DirectCast(e.Row.Cells(0).FindControl("LinkButtonDelete"), LinkButton)
                LinkButtonDelete.Text = DeleteText
                LinkButtonDelete.CssClass = "ui-state-default ui-corner-all buttonLink"
            End If
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
                objCaseWorkerSettingDataObject.CityId = Convert.ToInt32(DropDownListCity.SelectedValue)
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
                                DisplayHeaderMessage(DuplicateCaseWorkerMessage)
                                ButtonSaveWithConfirmation.Visible = True
                                ButtonSave.Visible = False
                                ButtonSaveWithConfirmation.Attributes.Add("OnClick", "return confirm('" + SaveConfirmMessage + "')")
                                Return
                            End If
                        End Try

                        DisplayHeaderMessage(InsertMessage)
                        Exit Select
                    Case False
                        objCaseWorkerSettingDataObject.CaseWorkerId = Convert.ToInt32(HiddenFieldCaseWorkerId.Value)
                        objCaseWorkerSettingDataObject.UpdateBy = Convert.ToString(UserId)

                        Try
                            objBLCaseWorkerSetting.UpdateCaseWorkerInfo(ConVisitel, objCaseWorkerSettingDataObject, IsConfirm)

                        Catch ex As SqlException
                            If ex.Message.Contains("Duplicate Case Worker Code") Then
                                DisplayHeaderMessage(DuplicateCaseWorkerMessage)
                                ButtonSaveWithConfirmation.Visible = True
                                ButtonSave.Visible = False
                                ButtonSaveWithConfirmation.Attributes.Add("OnClick", "return confirm('" + SaveConfirmMessage + "')")
                                Return
                            End If
                        End Try

                        DisplayHeaderMessage(UpdateMessage)
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

            TextBoxMailCode.MaxLength = InlineAssignHelper(TextBoxAddress.MaxLength, 50)
            TextBoxComments.MaxLength = 250
            TextBoxFirstName.MaxLength = InlineAssignHelper(TextBoxLastName.MaxLength, 20)
            TextBoxPhone.MaxLength = InlineAssignHelper(TextBoxFax.MaxLength, 16)

            AddHandler ButtonSave.Click, AddressOf ButtonSave_Click
            AddHandler ButtonSaveWithConfirmation.Click, AddressOf ButtonSaveWithConfirmation_Click
            AddHandler ButtonClear.Click, AddressOf ButtonClear_Click
            AddHandler ButtonSearch.Click, AddressOf ButtonSearch_Click
            AddHandler GridViewCaseWorkerInformation.PageIndexChanging, AddressOf GridViewCaseWorkerInformation_PageIndexChanging
            AddHandler GridViewCaseWorkerInformation.RowDataBound, AddressOf GridViewCaseWorkerInformation_RowDataBound

            ButtonSaveWithConfirmation.Visible = False

        End Sub

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource()
            Dim ResourceTable As Hashtable = GetResourceValue("Setup", ControlName & Convert.ToString(".resx"))

            LabelCaseWorkerInformationEntryCaption.Text = Convert.ToString(ResourceTable("LabelCaseWorkerInformationEntry"), Nothing)
            LabelFirstName.Text = Convert.ToString(ResourceTable("LabelFirstName"), Nothing)
            LabelLastName.Text = Convert.ToString(ResourceTable("LabelLastName"), Nothing)
            LabelAddress.Text = Convert.ToString(ResourceTable("LabelAddress"), Nothing)
            LabelPhone.Text = Convert.ToString(ResourceTable("LabelPhone"), Nothing)
            LabelFax.Text = Convert.ToString(ResourceTable("LabelFax"), Nothing)
            LabelStatus.Text = Convert.ToString(ResourceTable("LabelStatus"), Nothing)
            LabelComments.Text = Convert.ToString(ResourceTable("LabelComments"), Nothing)
            LabelEmail.Text = Convert.ToString(ResourceTable("LabelEmail"), Nothing)
            LabelMailCode.Text = Convert.ToString(ResourceTable("LabelMailCode"), Nothing)
            LabelCity.Text = Convert.ToString(ResourceTable("LabelCity"), Nothing)

            LabelSearchCaseWorkerInfo.Text = Convert.ToString(ResourceTable("LabelSearchCaseWorkerInfo"), Nothing)

            LabelSearchByFirstName.Text = LabelFirstName.Text
            LabelSearchByLastName.Text = LabelLastName.Text
            LabelSearchByPhone.Text = LabelPhone.Text

            LabelCaseWorkerInformationList.Text = Convert.ToString(ResourceTable("LabelCaseWorkerInformationList"), Nothing)

            ButtonSave.Text = Convert.ToString(ResourceTable("ButtonSave"), Nothing)
            ButtonSaveWithConfirmation.Text = Convert.ToString(ResourceTable("ButtonSave"), Nothing)

            ButtonClear.Text = Convert.ToString(ResourceTable("ButtonClear"), Nothing)
            ButtonSearch.Text = Convert.ToString(ResourceTable("ButtonSearch"), Nothing)

            EditText = Convert.ToString(ResourceTable("EditText"), Nothing)
            DeleteText = Convert.ToString(ResourceTable("DeleteText"), Nothing)

            InsertMessage = Convert.ToString(ResourceTable("InsertMessage"), Nothing)
            UpdateMessage = Convert.ToString(ResourceTable("UpdateMessage"), Nothing)
            DeleteMessage = Convert.ToString(ResourceTable("DeleteMessage"), Nothing)
            StatusSelectionMessage = Convert.ToString(ResourceTable("StatusSelectionMessage"), Nothing)
            CitySelectionMessage = Convert.ToString(ResourceTable("CitySelectionMessage"), Nothing)

            DuplicateCaseWorkerMessage = Convert.ToString(ResourceTable("DuplicateCaseWorkerMessage"), Nothing)
            SaveConfirmMessage = Convert.ToString(ResourceTable("SaveConfirmMessage"), Nothing)
            InvalidEmailMessage = Convert.ToString(ResourceTable("InvalidEmailMessage"), Nothing)
            InvalidPhoneMessage = Convert.ToString(ResourceTable("InvalidPhoneMessage"), Nothing)
            InvalidFaxMessage = Convert.ToString(ResourceTable("InvalidFaxMessage"), Nothing)

            ResourceTable = Nothing
        End Sub

        ''' <summary>
        ''' This is for validating data in server side before submitting the form
        ''' </summary>
        ''' <returns></returns>
        Private Function ValidateData() As Boolean

            If DropDownListStatus.SelectedValue = "-1" Then
                DisplayHeaderMessage(StatusSelectionMessage)
                Return False
            End If

            If DropDownListCity.SelectedValue = "-1" Then
                DisplayHeaderMessage(CitySelectionMessage)
                Return False
            End If

            If (Not ValidateEmail(TextBoxEmail.Text.Trim)) Then
                DisplayHeaderMessage(InvalidEmailMessage)
                Return False
            End If

            If (Not ValidatePhone(TextBoxPhone.Text.Trim)) Then
                DisplayHeaderMessage(InvalidPhoneMessage)
                Return False
            End If

            If (Not ValidatePhone(TextBoxFax.Text.Trim)) Then
                DisplayHeaderMessage(InvalidFaxMessage)
                Return False
            End If

            Return True
        End Function

        ''' <summary>
        ''' This is for retrieving data
        ''' </summary>
        Private Sub GetData()
            BindStatusDropDownList(DropDownListStatus)
            BindCityDropDownList(DropDownListCity, CompanyId)
            BindCaseWorkerInformationGridView()
        End Sub

        ''' <summary>
        ''' Clearing controls value
        ''' </summary>
        Private Sub ClearControl()

            TextBoxFirstName.Text = InlineAssignHelper(TextBoxLastName.Text, InlineAssignHelper(TextBoxPhone.Text, InlineAssignHelper(TextBoxFax.Text,
                                    InlineAssignHelper(TextBoxMailCode.Text, InlineAssignHelper(TextBoxEmail.Text, InlineAssignHelper(TextBoxAddress.Text,
                                    InlineAssignHelper(TextBoxComments.Text, String.Empty)))))))

            DropDownListCity.SelectedIndex = InlineAssignHelper(DropDownListStatus.SelectedIndex, 0)

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

