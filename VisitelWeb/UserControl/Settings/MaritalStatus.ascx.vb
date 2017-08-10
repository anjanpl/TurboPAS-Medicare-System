#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: City Setup
' Author: Anjan Kumar Paul
' Start Date: 23 Aug 2014
' End Date: 23 Aug 2014
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                23 Aug 2014     Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness.Settings
Imports System.Data.SqlClient
Imports VisitelBusiness.VisitelBusiness.DataObject.Settings
Imports VisitelBusiness.VisitelBusiness.DataObject

Namespace Visitel.UserControl.Settings
    Public Class MaritalStatusSetting
        Inherits BaseUserControl

        Private ControlName As String, EditText As String, DeleteText As String, InsertMessage As String, UpdateMessage As String, DeleteMessage As String, _
        DuplicateNameMessage As String, EmptyNameMessage As String, StatusSelectionMessage As String

        Private objShared As SharedWebControls

        Protected Sub Page_Init(sender As Object, e As EventArgs)
            DirectCast(Me.Page.Master, IMyMasterPage).PageHeaderTitle = "Marital Status Setting"
            ControlName = "MaritalStatusSetting"

            objShared = New SharedWebControls()
            objShared.ConnectionOpen()

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
            LoadCss("Settings/" + ControlName)
            LoadJavascript()
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing
        End Sub

        Protected Sub LinkButtonEdit_Click(sender As Object, e As EventArgs)
            Dim LinkButtonDelete As LinkButton = DirectCast(sender, LinkButton)
            Dim GridViewItemRow As GridViewRow = DirectCast(LinkButtonDelete.NamingContainer, GridViewRow)

            TextBoxName.Text = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelName"), Label).Text, Nothing)

            HiddenFieldIdNumber.Value = Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelIdNumber"), Label).Text, Nothing)
            DropDownListStatus.SelectedIndex = DropDownListStatus.Items.IndexOf(DropDownListStatus.Items.FindByText(Convert.ToString(DirectCast(GridViewItemRow.Cells(0).FindControl("LabelStatus"), Label).Text, Nothing)))

            HiddenFieldIsNew.Value = Convert.ToString(False, Nothing)
        End Sub

        Protected Sub ButtonDelete_Click(sender As Object, e As EventArgs)
            Dim ButtonDelete As Button = DirectCast(sender, Button)
            Dim dtgItem As GridViewRow = DirectCast(ButtonDelete.NamingContainer, GridViewRow)
            Dim LabelIdNumber As Label = DirectCast(dtgItem.Cells(0).FindControl("LabelIdNumber"), Label)

            Dim objBLMaritalStatusSetting As New BLMaritalStatusSetting()
            Try
                objBLMaritalStatusSetting.DeleteMaritalStatusInfo(objShared.ConVisitel, Convert.ToInt32(LabelIdNumber.Text), objShared.UserId)
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DeleteMessage)
            Catch ex As Exception

            Finally
                objBLMaritalStatusSetting = Nothing
            End Try

            BindMaritalStatusInformationGridView()
            HiddenFieldIsNew.Value = Convert.ToString(False, Nothing)
            HiddenFieldIdNumber.Value = Convert.ToString(Int32.MinValue)
        End Sub

        Private Sub ButtonSave_Click(sender As Object, e As EventArgs)
            If ValidateData() Then
                Dim objMaritalStatusSettingDataObject As New MaritalStatusSettingDataObject()

                objMaritalStatusSettingDataObject.Name = Convert.ToString(TextBoxName.Text, Nothing).Trim()
                objMaritalStatusSettingDataObject.Status = Convert.ToInt16(DropDownListStatus.SelectedValue)

                Dim objBLMaritalStatusSetting As New BLMaritalStatusSetting()

                Select Case Convert.ToBoolean(HiddenFieldIsNew.Value, Nothing)
                    Case True
                        objMaritalStatusSettingDataObject.UserId = objShared.UserId
                        objMaritalStatusSettingDataObject.CompanyId = objShared.CompanyId
                        Try
                            objBLMaritalStatusSetting.InsertMaritalStatusInfo(objShared.ConVisitel, objMaritalStatusSettingDataObject)

                        Catch ex As SqlException
                            If ex.Message.Contains("Violation of UNIQUE KEY constraint 'UQ_Name'") Then
                                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DuplicateNameMessage)
                                Return
                            End If
                        End Try

                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(InsertMessage)
                        Exit Select
                    Case False
                        objMaritalStatusSettingDataObject.IdNumber = Convert.ToInt32(HiddenFieldIdNumber.Value)
                        objMaritalStatusSettingDataObject.UpdateBy = Convert.ToString(objShared.UserId)
                        Try
                            objBLMaritalStatusSetting.UpdateMaritalStatusInfo(objShared.ConVisitel, objMaritalStatusSettingDataObject)
                        Catch ex As SqlException
                            If ex.Message.Contains("Violation of UNIQUE KEY constraint 'UQ_Name'") Then
                                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(DuplicateNameMessage)
                                Return
                            End If
                        End Try

                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(UpdateMessage)
                        Exit Select
                End Select
                objBLMaritalStatusSetting = Nothing

                HiddenFieldIsNew.Value = Convert.ToString(True, Nothing)
                HiddenFieldIdNumber.Value = Convert.ToString(Int32.MinValue)
                BindMaritalStatusInformationGridView()
            End If
        End Sub

        Private Sub ButtonClear_Click(sender As Object, e As EventArgs)
            ClearControl()
        End Sub

        Private Sub ButtonSearch_Click(sender As Object, e As EventArgs)
            HiddenFieldIsSearched.Value = Convert.ToString(True, Nothing)
            BindMaritalStatusInformationGridView()
            HiddenFieldIsSearched.Value = Convert.ToString(False, Nothing)
        End Sub

        Private Sub GridViewMaritalStatusInformation_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
            GridViewMaritalStatusInformation.PageIndex = e.NewPageIndex
            BindMaritalStatusInformationGridView()
        End Sub

        Private Sub GridViewMaritalStatusInformation_RowDataBound(sender As [Object], e As GridViewRowEventArgs)
            If e.Row.RowType.Equals(DataControlRowType.DataRow) Then

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
            TextBoxName.MaxLength = 50

            AddHandler ButtonSave.Click, AddressOf ButtonSave_Click
            AddHandler ButtonClear.Click, AddressOf ButtonClear_Click
            AddHandler ButtonSearch.Click, AddressOf ButtonSearch_Click

            AddHandler GridViewMaritalStatusInformation.PageIndexChanging, AddressOf GridViewMaritalStatusInformation_PageIndexChanging
            AddHandler GridViewMaritalStatusInformation.RowDataBound, AddressOf GridViewMaritalStatusInformation_RowDataBound

            GridViewMaritalStatusInformation.AutoGenerateColumns = False
            GridViewMaritalStatusInformation.ShowHeaderWhenEmpty = True
            GridViewMaritalStatusInformation.AllowPaging = True
            GridViewMaritalStatusInformation.PageSize = objShared.GridViewDefaultPageSize

            ButtonSave.ClientIDMode = ClientIDMode.Static
            ButtonClear.ClientIDMode = ClientIDMode.Static
            ButtonSearch.ClientIDMode = ClientIDMode.Static
        End Sub

        Private Sub LoadJavascript()
            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                              & " var DeleteDialogHeader ='Marital Status'; " _
                              & " var DeleteDialogConfirmMsg ='Do you want to delete this record?'; " _
                              & " var LoaderImagePath ='" & objShared.GetLoaderImagePath & "'; " _
                              & " var prm =''; " _
                              & " jQuery(document).ready(function () {" _
                              & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
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

            LabelName.Text = Convert.ToString(ResourceTable("LabelName"), Nothing)
            LabelName.Text = If(String.IsNullOrEmpty(LabelName.Text), "Name", LabelName.Text)

            LabelSearchByName.Text = LabelName.Text

            ButtonSave.Text = Convert.ToString(ResourceTable("ButtonSave"), Nothing)
            ButtonSave.Text = If(String.IsNullOrEmpty(ButtonSave.Text), "Save", ButtonSave.Text)

            ButtonClear.Text = Convert.ToString(ResourceTable("ButtonClear"), Nothing)
            ButtonClear.Text = If(String.IsNullOrEmpty(ButtonClear.Text), "Clear", ButtonClear.Text)

            ButtonSearch.Text = Convert.ToString(ResourceTable("ButtonSearch"), Nothing)
            ButtonSearch.Text = If(String.IsNullOrEmpty(ButtonSearch.Text), "Search", ButtonSearch.Text)

            LabelMaritalStatusInformationEntry.Text = Convert.ToString(ResourceTable("LabelMaritalStatusInformationEntry"), Nothing)
            LabelMaritalStatusInformationEntry.Text = If(String.IsNullOrEmpty(LabelMaritalStatusInformationEntry.Text),
                                                         "Marital Status Information Entry", LabelMaritalStatusInformationEntry.Text)

            LabelSearchMaritalStatusInfo.Text = Convert.ToString(ResourceTable("LabelSearchMaritalStatusInfo"), Nothing)
            LabelSearchMaritalStatusInfo.Text = If(String.IsNullOrEmpty(LabelSearchMaritalStatusInfo.Text),
                                                   "Search Marital Status Information", LabelSearchMaritalStatusInfo.Text)

            LabelMaritalStatusInformationList.Text = Convert.ToString(ResourceTable("LabelMaritalStatusInformationList"), Nothing)
            LabelMaritalStatusInformationList.Text = If(String.IsNullOrEmpty(LabelMaritalStatusInformationList.Text),
                                                        "Marital Status Information List", LabelMaritalStatusInformationList.Text)

            EditText = Convert.ToString(ResourceTable("EditText"), Nothing)
            DeleteText = Convert.ToString(ResourceTable("DeleteText"), Nothing)

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

            ResourceTable = Nothing
        End Sub

        ''' <summary>
        ''' This is for validating data in server side before submitting the form
        ''' </summary>
        ''' <returns></returns>
        Private Function ValidateData() As Boolean

            If String.IsNullOrEmpty(TextBoxName.Text.Trim()) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(EmptyNameMessage)
                Return False
            End If

            If DropDownListStatus.SelectedValue = "-1" Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(StatusSelectionMessage)
                Return False
            End If

            Return True
        End Function

        ''' <summary>
        ''' This is for retrieving data
        ''' </summary>
        Private Sub GetData()
            objShared.BindStatusDropDownList(DropDownListStatus)
            BindMaritalStatusInformationGridView()
        End Sub

        ''' <summary>
        ''' Clearing controls value
        ''' </summary>
        Private Sub ClearControl()
            TextBoxName.Text = String.Empty
            TextBoxSearchByName.Text = String.Empty

            DropDownListStatus.SelectedIndex = 1

            HiddenFieldIsNew.Value = Convert.ToString(True, Nothing)
            HiddenFieldIsSearched.Value = Convert.ToString(False, Nothing)
            HiddenFieldIdNumber.Value = Convert.ToString(Int32.MinValue)
        End Sub

        ''' <summary>
        ''' Binding Marital Status Information GridView
        ''' </summary>
        Private Sub BindMaritalStatusInformationGridView()
            Dim MaritalStatusSettingList As List(Of MaritalStatusSettingDataObject)

            Dim objBLMaritalStatusSetting As New BLMaritalStatusSetting()

            MaritalStatusSettingList = If((Convert.ToBoolean(HiddenFieldIsSearched.Value, Nothing)),
                                            objBLMaritalStatusSetting.SearchMaritalStatusInfo(objShared.ConVisitel, objShared.CompanyId, TextBoxSearchByName.Text.Trim()),
                                            objBLMaritalStatusSetting.SelectMaritalStatusInfo(objShared.ConVisitel, objShared.CompanyId))

            objBLMaritalStatusSetting = Nothing

            GridViewMaritalStatusInformation.DataSource = MaritalStatusSettingList
            GridViewMaritalStatusInformation.DataBind()

            MaritalStatusSettingList = Nothing
        End Sub
    End Class
End Namespace