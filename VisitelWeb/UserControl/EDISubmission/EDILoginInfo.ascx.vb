
#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: EDI Login Information Save & Delete
' Author: Anjan Kumar Paul
' Start Date: 26 July 2015
' End Date: 
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                26 July 2015     Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports System.Data.SqlClient
Imports System.Linq.Expressions

Namespace Visitel.UserControl.EDISubmission
    Public Class EDILoginInfoControl
        Inherits BaseUserControl

        Private ControlName As String, ValidationGroup As String, SaveMessage As String, DeleteMessage As String, DeleteConfirmationMessage As String, ListFor As String
        Private ValidationEnable As Boolean

        Private CurrentRow As GridViewRow
        Private objShared As SharedWebControls

        Private LabelIdNumber As Label
        Private TextBoxName As TextBox, TextBoxSubmitterId As TextBox, TextBoxPassword As TextBox, TextBoxFtpSite As TextBox, TextBoxDirectory As TextBox,
            TextBoxUpdateDate As TextBox, TextBoxUpdateBy As TextBox

        Private CheckBoxSelect As CheckBox, chkAll As CheckBox

        Private DropDownListStatus As DropDownList

        Private ButtonHeaderName As Button, ButtonHeaderSubmitterId As Button, ButtonHeaderPassword As Button, ButtonHeaderFtpSite As Button, ButtonHeaderDirectory As Button,
            ButtonHeaderStatus As Button

        Private EDILoginInfoList As List(Of EDILoginDataObject)
        Private EDILoginInfoErrorList As List(Of EDILoginDataObject)
        Private SaveFailedCounter As Int16, DeleteFailedCounter As Int16

        Protected Sub Page_Init(sender As Object, e As EventArgs)

            DirectCast(Me.Page.Master, IMyMasterPage).PageHeaderTitle = "EDI Login Info"
            ControlName = "EDILoginInfoControl"
            objShared = New SharedWebControls()
            objShared.ConnectionOpen()
            GetCaptionFromResource()
            InitializeControl()

        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)
            If Not IsPostBack Then
                Session.Add("SortingExpression", "Name")
                GetData()
            End If
        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadCss("EDISubmission/" + ControlName)
            LoadJScript()
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing

            EDILoginInfoList = Nothing
            EDILoginInfoErrorList = Nothing
        End Sub

        Private Sub ButtonClear_Click(sender As Object, e As EventArgs)

            ViewState.Clear()

            ButtonViewError.Visible = False
            BindEDILoginInfoGridView()

        End Sub

        Protected Sub OnCheckedChanged(sender As Object, e As EventArgs)

            Dim isUpdateVisible As Boolean = False

            Dim chk As CheckBox = TryCast(sender, CheckBox)

            'ButtonSave.Enabled = If((chk.Checked), True, False)

            If chk.ID = "chkAll" Then
                For Each row As GridViewRow In GridViewEDILoginInfo.Rows
                    If row.RowType = DataControlRowType.DataRow Then
                        row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked = chk.Checked
                    End If
                Next

                CurrentRow = GridViewEDILoginInfo.FooterRow
                CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)
                CheckBoxSelect.Checked = chk.Checked

            End If

            Dim chkAll As CheckBox = TryCast(GridViewEDILoginInfo.HeaderRow.FindControl("chkAll"), CheckBox)
            'chkAll.Checked = True
            For Each row As GridViewRow In GridViewEDILoginInfo.Rows
                If row.RowType = DataControlRowType.DataRow Then

                    Dim isChecked As Boolean = row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked

                    ControlsOnSelect(row, isChecked)

                End If
            Next

            DetermineButtonInActivity()

        End Sub

        Private Sub ButtonSave_Click(sender As Object, e As EventArgs)

            Page.Validate()
            Page.Validate(ValidationGroup)

            EDILoginInfoList = New List(Of EDILoginDataObject)()
            EDILoginInfoErrorList = New List(Of EDILoginDataObject)()

            SaveFailedCounter = 0

            CurrentRow = GridViewEDILoginInfo.FooterRow

            CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)

            TextBoxName = DirectCast(CurrentRow.FindControl("TextBoxName"), TextBox)

            If ((CheckBoxSelect.Checked) And (Not String.IsNullOrEmpty(TextBoxName.Text.Trim()))) Then
                SaveData(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT))
            End If

            SaveData(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE))

            If (SaveFailedCounter > 0) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} Records Failed to Save out of {1}", SaveFailedCounter, EDILoginInfoList.Count))
                ViewState("SaveFailedRecord") = objShared.ToDataTable(EDILoginInfoErrorList)
                ViewState("ListFor") = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)
            Else
                ViewState("SaveFailedRecord") = Nothing

                If (EDILoginInfoList.Count > 0) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} record(s) {1}.", EDILoginInfoList.Count, SaveMessage))
                    ViewState.Clear()
                    BindEDILoginInfoGridView()
                End If

            End If

            ButtonViewError.Visible = If((SaveFailedCounter > 0), True, False)

            EDILoginInfoList = Nothing
            EDILoginInfoErrorList = Nothing

        End Sub

        Private Sub ButtonDelete_Click(sender As Object, e As EventArgs)
            Return
            EDILoginInfoList = New List(Of EDILoginDataObject)()
            EDILoginInfoErrorList = New List(Of EDILoginDataObject)()

            MakeList(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE))

            If (EDILoginInfoList.Count.Equals(0)) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please select items to delete")
                Return
            End If

            DeleteFailedCounter = 0

            TakeAction(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE))

            If (DeleteFailedCounter > 0) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} Records Failed to Delete out of {1}", DeleteFailedCounter, EDILoginInfoList.Count))
                ViewState("DeleteFailedRecord") = objShared.ToDataTable(EDILoginInfoErrorList)
                ViewState("ListFor") = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE)
            Else
                ViewState("DeleteFailedRecord") = Nothing

                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} record(s) {1}.", EDILoginInfoList.Count, DeleteMessage))
                ViewState.Clear()
                BindEDILoginInfoGridView()
            End If

            ButtonViewError.Visible = If((DeleteFailedCounter > 0), True, False)

            EDILoginInfoList = Nothing
            EDILoginInfoErrorList = Nothing

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

        Private Sub GridViewEDILoginInfo_RowDataBound(sender As Object, e As GridViewRowEventArgs)

            CurrentRow = DirectCast(e.Row, GridViewRow)

            If (e.Row.RowType.Equals(DataControlRowType.Header)) Then
                SetGridViewColumnHeaderText(CurrentRow)

                chkAll = DirectCast(CurrentRow.FindControl("chkAll"), CheckBox)
                chkAll.AutoPostBack = True
                chkAll.ClientIDMode = UI.ClientIDMode.Static

            End If

            If (e.Row.RowType.Equals(DataControlRowType.DataRow)) Then

                CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)
                CheckBoxSelect.AutoPostBack = True

                LabelIdNumber = DirectCast(CurrentRow.FindControl("LabelIdNumber"), Label)

                TextBoxName = DirectCast(CurrentRow.FindControl("TextBoxName"), TextBox)
                TextBoxName.ReadOnly = True

                TextBoxSubmitterId = DirectCast(CurrentRow.FindControl("TextBoxSubmitterId"), TextBox)
                TextBoxSubmitterId.ReadOnly = True

                TextBoxPassword = DirectCast(CurrentRow.FindControl("TextBoxPassword"), TextBox)
                TextBoxPassword.ReadOnly = True

                TextBoxFtpSite = DirectCast(CurrentRow.FindControl("TextBoxFtpSite"), TextBox)
                TextBoxFtpSite.ReadOnly = True

                TextBoxDirectory = DirectCast(CurrentRow.FindControl("TextBoxDirectory"), TextBox)
                TextBoxDirectory.ReadOnly = True

                DropDownListStatus = DirectCast(CurrentRow.FindControl("DropDownListStatus"), DropDownList)
                BindStatusDropDownList()

                DropDownListStatus.SelectedIndex = DropDownListStatus.Items.IndexOf(DropDownListStatus.Items.FindByValue(
                                                   ((From p In EDILoginInfoList Where p.IdNumber = Convert.ToInt64(LabelIdNumber.Text)).SingleOrDefault).Status))

                DropDownListStatus.Enabled = False

                TextBoxUpdateDate = DirectCast(CurrentRow.FindControl("TextBoxUpdateDate"), TextBox)
                TextBoxUpdateDate.ReadOnly = True

                TextBoxUpdateBy = DirectCast(CurrentRow.FindControl("TextBoxUpdateBy"), TextBox)
                TextBoxUpdateBy.ReadOnly = True

            End If

            If (e.Row.RowType.Equals(DataControlRowType.Footer)) Then

                CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)
                CheckBoxSelect.AutoPostBack = True

                DropDownListStatus = DirectCast(CurrentRow.FindControl("DropDownListStatus"), DropDownList)
                BindStatusDropDownList()

                TextBoxUpdateDate = DirectCast(CurrentRow.FindControl("TextBoxUpdateDate"), TextBox)
                TextBoxUpdateDate.ReadOnly = True

                TextBoxUpdateBy = DirectCast(CurrentRow.FindControl("TextBoxUpdateBy"), TextBox)
                TextBoxUpdateBy.ReadOnly = True

            End If

        End Sub

        Private Sub GridViewEDILoginInfo_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
            GridViewEDILoginInfo.PageIndex = e.NewPageIndex
            BindEDILoginInfoGridView()
        End Sub

        ''' <summary>
        ''' Gridview Row Hover Color Set
        ''' </summary>
        ''' <param name="writer"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub Render(writer As HtmlTextWriter)
            For Each r As GridViewRow In GridViewEDILoginInfo.Rows
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

            AddHandler ButtonClear.Click, AddressOf ButtonClear_Click
            AddHandler ButtonSave.Click, AddressOf ButtonSave_Click
            AddHandler ButtonDelete.Click, AddressOf ButtonDelete_Click

            GridViewEDILoginInfo.AutoGenerateColumns = False
            GridViewEDILoginInfo.ShowHeaderWhenEmpty = True
            GridViewEDILoginInfo.AllowPaging = True
            'GridViewEDILoginInfo.AllowSorting = True

            GridViewEDILoginInfo.ShowFooter = True

            If (GridViewEDILoginInfo.AllowPaging) Then
                GridViewEDILoginInfo.PageSize = objShared.GridViewDefaultPageSize
            End If

            AddHandler GridViewEDILoginInfo.RowDataBound, AddressOf GridViewEDILoginInfo_RowDataBound
            AddHandler GridViewEDILoginInfo.PageIndexChanging, AddressOf GridViewEDILoginInfo_PageIndexChanging

            ButtonDelete.ClientIDMode = UI.ClientIDMode.Static
        End Sub


        Private Sub LoadJScript()

            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                           & " var DeleteTargetButton ='ButtonDelete'; " _
                           & " var DeleteDialogHeader ='EDI Login Info'; " _
                           & " var DeleteDialogConfirmMsg ='" & DeleteConfirmationMessage & "'; " _
                           & " var prm =''; " _
                           & " jQuery(document).ready(function () {" _
                           & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                           & "     DataDelete();" _
                           & "     prm.add_endRequest(DataDelete); " _
                           & "}); " _
                    & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

            LoadJS("JavaScript/EDILoginInfo/" & ControlName & ".js")

        End Sub

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("EDISubmission", ControlName & Convert.ToString(".resx"))

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
            ValidationGroup = If(String.IsNullOrEmpty(ValidationGroup), "EDILogin", ValidationGroup)

            DeleteConfirmationMessage = Convert.ToString(ResourceTable("DeleteConfirmationMessage"), Nothing)
            DeleteConfirmationMessage = If(String.IsNullOrEmpty(DeleteConfirmationMessage), "Are you sure to delete this record?", DeleteConfirmationMessage)

            Dim ResultOut As Boolean

            ValidationEnable = If((Boolean.TryParse(ResourceTable("ValidationEnable"), ResultOut)), ResultOut, True)

            ResourceTable = Nothing

        End Sub

        ''' <summary>
        ''' This is for retrieving data
        ''' </summary>
        Private Sub GetData()
            BindEDILoginInfoGridView()
        End Sub

        ''' <summary>
        ''' Filling out Grid View Template Column Status DropDownList 
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindStatusDropDownList()

            DropDownListStatus.DataSource = EnumDataObject.Enumeration.GetAll(Of EnumDataObject.EDILoginStatus)()
            DropDownListStatus.DataTextField = "Value"
            DropDownListStatus.DataValueField = "Key"
            DropDownListStatus.DataBind()

            DropDownListStatus.SelectedIndex = 1

        End Sub

        ''' <summary>
        ''' EDI Codes Grid Data Bind
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindEDILoginInfoGridView()

            Dim objBLEDISubmission As New BLEDISubmission

            Try
                EDILoginInfoList = objBLEDISubmission.SelectEDILogin(objShared.ConVisitel)
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to fetch EDI Login Info")
            Finally
                objBLEDISubmission = Nothing
            End Try

            GetSortedEDILoginInfo(EDILoginInfoList)

            GridViewEDILoginInfo.DataSource = EDILoginInfoList
            GridViewEDILoginInfo.DataBind()

            DetermineButtonInActivity()

        End Sub

        ''' <summary>
        ''' Making Save and Delete Button Active/InActive
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub DetermineButtonInActivity()

            Dim ItemChecked As Boolean = False

            For Each row As GridViewRow In GridViewEDILoginInfo.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    ItemChecked = row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked
                    If (ItemChecked) Then
                        Exit For
                    End If
                End If
            Next

            If (Not ItemChecked) Then
                If (GridViewEDILoginInfo.Rows.Count > 0) Then
                    CurrentRow = GridViewEDILoginInfo.FooterRow
                    CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)
                    ItemChecked = CheckBoxSelect.Checked
                End If
            End If

            ButtonSave.Enabled = If((ItemChecked), True, False)
            ButtonDelete.Enabled = ButtonSave.Enabled

        End Sub

        ''' <summary>
        ''' Making a List of Record Set Either for Save or Delete
        ''' </summary>
        ''' <param name="Action"></param>
        ''' <remarks></remarks>
        Private Sub MakeList(Action As String)

            Dim objEDILoginDataObject As EDILoginDataObject

            Select Case Action
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT)

                    objEDILoginDataObject = New EDILoginDataObject()
                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)
                    AddToList(CurrentRow, EDILoginInfoList, objEDILoginDataObject)
                    objEDILoginDataObject = Nothing

                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE),
                    EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE)
                    For Each row As GridViewRow In GridViewEDILoginInfo.Rows
                        If (row.RowType.Equals(DataControlRowType.DataRow)) Then
                            Dim isChecked As Boolean = row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked

                            If (isChecked) Then

                                objEDILoginDataObject = New EDILoginDataObject()

                                CurrentRow = DirectCast(GridViewEDILoginInfo.Rows(row.RowIndex), GridViewRow)

                                If (Action.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE))) Then
                                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)
                                End If

                                If (Action.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE))) Then
                                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE)
                                End If


                                AddToList(CurrentRow, EDILoginInfoList, objEDILoginDataObject)
                                objEDILoginDataObject = Nothing

                            End If

                        End If
                    Next

                    Exit Select
            End Select

            objEDILoginDataObject = Nothing

        End Sub

        ''' <summary>
        ''' Make Database operation either for record saving or deleting
        ''' </summary>
        ''' <param name="Action"></param>
        ''' <remarks></remarks>
        Private Sub TakeAction(Action As String)

            Dim objBLEDISubmission As New BLEDISubmission

            For Each EDILogin In EDILoginInfoList
                Try
                    Select Case Action
                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT)
                            objBLEDISubmission.InsertEDILoginInfo(objShared.ConVisitel, EDILogin)
                            Exit Select

                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE)
                            objBLEDISubmission.UpdateEDILoginInfo(objShared.ConVisitel, EDILogin)
                            Exit Select

                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE)
                            objBLEDISubmission.DeleteEDILoginInfo(objShared.ConVisitel, EDILogin.IdNumber, EDILogin.UpdateBy)
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

                    EDILogin.Remarks = ex.Message
                    EDILoginInfoErrorList.Add(EDILogin)
                End Try
            Next

            objBLEDISubmission = Nothing

        End Sub

        ''' <summary>
        ''' Saving Data either insert or update after making list
        ''' </summary>
        ''' <param name="Action"></param>
        ''' <remarks></remarks>
        Private Sub SaveData(Action As String)

            MakeList(Action)

            If (EDILoginInfoList.Count.Equals(0)) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please select items to save")
                Return
            End If

            TakeAction(Action)

        End Sub

        ''' <summary>
        ''' Making List in order to save records or delete
        ''' </summary>
        ''' <param name="CurrentRow"></param>
        ''' <param name="EDILoginInfoList"></param>
        ''' <param name="objEDILoginDataObject"></param>
        ''' <remarks></remarks>
        Private Sub AddToList(ByRef CurrentRow As GridViewRow, ByRef EDILoginInfoList As List(Of EDILoginDataObject), ByRef objEDILoginDataObject As EDILoginDataObject)

            Dim Int64Result As Int64

            LabelIdNumber = DirectCast(CurrentRow.FindControl("LabelIdNumber"), Label)

            If (ListFor.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE))) Then
                TextBoxName = DirectCast(CurrentRow.FindControl("TextBoxName"), TextBox)
                TextBoxSubmitterId = DirectCast(CurrentRow.FindControl("TextBoxSubmitterId"), TextBox)
                TextBoxPassword = DirectCast(CurrentRow.FindControl("TextBoxPassword"), TextBox)
                TextBoxFtpSite = DirectCast(CurrentRow.FindControl("TextBoxFtpSite"), TextBox)
                TextBoxDirectory = DirectCast(CurrentRow.FindControl("TextBoxDirectory"), TextBox)
                DropDownListStatus = DirectCast(CurrentRow.FindControl("DropDownListStatus"), DropDownList)
            End If

            If (Page.IsValid) Then

                objEDILoginDataObject.IdNumber = If(Int64.TryParse(LabelIdNumber.Text.Trim(), Int64Result), Int64Result, objEDILoginDataObject.IdNumber)

                If (ListFor.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE))) Then
                    objEDILoginDataObject.Name = TextBoxName.Text.Trim()
                    objEDILoginDataObject.SubmitterId = TextBoxSubmitterId.Text.Trim()
                    objEDILoginDataObject.Password = TextBoxPassword.Text.Trim()
                    objEDILoginDataObject.FTPAddress = TextBoxFtpSite.Text.Trim()
                    objEDILoginDataObject.Directory = TextBoxDirectory.Text.Trim()
                    objEDILoginDataObject.Status = DropDownListStatus.SelectedValue
                End If

                objEDILoginDataObject.UpdateBy = objShared.UserId

                EDILoginInfoList.Add(objEDILoginDataObject)

            End If

        End Sub

        ''' <summary>
        ''' Gridview row selection and go for edit mode or record deletion
        ''' </summary>
        ''' <param name="CurrentRow"></param>
        ''' <param name="isChecked"></param>
        ''' <remarks></remarks>
        Private Sub ControlsOnSelect(ByRef CurrentRow As GridViewRow, ByRef isChecked As Boolean)

            TextBoxName = DirectCast(CurrentRow.FindControl("TextBoxName"), TextBox)

            If (Not TextBoxName Is Nothing) Then
                TextBoxName.ReadOnly = Not isChecked
                TextBoxName.CssClass = If((TextBoxName.ReadOnly), "TextBoxName", "TextBoxNameEdit")
            End If

            TextBoxSubmitterId = DirectCast(CurrentRow.FindControl("TextBoxSubmitterId"), TextBox)

            If (Not TextBoxSubmitterId Is Nothing) Then
                TextBoxSubmitterId.ReadOnly = Not isChecked
                TextBoxSubmitterId.CssClass = If((TextBoxSubmitterId.ReadOnly), "TextBoxSubmitterId", "TextBoxSubmitterIdEdit")
            End If

            TextBoxPassword = DirectCast(CurrentRow.FindControl("TextBoxPassword"), TextBox)

            If (Not TextBoxPassword Is Nothing) Then
                TextBoxPassword.ReadOnly = Not isChecked
                TextBoxPassword.CssClass = If((TextBoxPassword.ReadOnly), "TextBoxPassword", "TextBoxPasswordEdit")
            End If

            TextBoxFtpSite = DirectCast(CurrentRow.FindControl("TextBoxFtpSite"), TextBox)

            If (Not TextBoxFtpSite Is Nothing) Then
                TextBoxFtpSite.ReadOnly = Not isChecked
                TextBoxFtpSite.CssClass = If((TextBoxFtpSite.ReadOnly), "TextBoxFtpSite", "TextBoxFtpSiteEdit")
            End If

            TextBoxDirectory = DirectCast(CurrentRow.FindControl("TextBoxDirectory"), TextBox)

            If (Not TextBoxDirectory Is Nothing) Then
                TextBoxDirectory.ReadOnly = Not isChecked
                TextBoxDirectory.CssClass = If((TextBoxDirectory.ReadOnly), "TextBoxDirectory", "TextBoxDirectoryEdit")
            End If

            DropDownListStatus = DirectCast(CurrentRow.FindControl("DropDownListStatus"), DropDownList)

            If (Not DropDownListStatus Is Nothing) Then
                DropDownListStatus.Enabled = isChecked
                DropDownListStatus.CssClass = If((DropDownListStatus.Enabled), "DropDownListStatusEdit", "DropDownListStatus")
            End If


        End Sub

        ''' <summary>
        ''' Reading GridView header text from resource file
        ''' </summary>
        ''' <param name="CurrentRow"></param>
        ''' <remarks></remarks>
        Private Sub SetGridViewColumnHeaderText(ByRef CurrentRow As GridViewRow)

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("EDISubmission", ControlName & Convert.ToString(".resx"))

            Dim LabelHeaderSerial As Label = DirectCast(CurrentRow.FindControl("LabelHeaderSerial"), Label)
            LabelHeaderSerial.Text = Convert.ToString(ResourceTable("LabelHeaderSerial"), Nothing).Trim()
            LabelHeaderSerial.Text = If(String.IsNullOrEmpty(LabelHeaderSerial.Text), "SI.", LabelHeaderSerial.Text)

            Dim LabelHeaderSelect As Label = DirectCast(CurrentRow.FindControl("LabelHeaderSelect"), Label)
            LabelHeaderSelect.Text = Convert.ToString(ResourceTable("LabelHeaderSelect"), Nothing).Trim()
            LabelHeaderSelect.Text = If(String.IsNullOrEmpty(LabelHeaderSelect.Text), "Select", LabelHeaderSelect.Text)

            ButtonHeaderName = DirectCast(CurrentRow.FindControl("ButtonHeaderName"), Button)
            ButtonHeaderName.Text = Convert.ToString(ResourceTable("ButtonHeaderName"), Nothing).Trim()
            ButtonHeaderName.Text = If(String.IsNullOrEmpty(ButtonHeaderName.Text), "Name", ButtonHeaderName.Text)

            ButtonHeaderSubmitterId = DirectCast(CurrentRow.FindControl("ButtonHeaderSubmitterId"), Button)
            ButtonHeaderSubmitterId.Text = Convert.ToString(ResourceTable("ButtonHeaderSubmitterId"), Nothing).Trim()
            ButtonHeaderSubmitterId.Text = If(String.IsNullOrEmpty(ButtonHeaderSubmitterId.Text), "Submitter ID", ButtonHeaderSubmitterId.Text)

            ButtonHeaderPassword = DirectCast(CurrentRow.FindControl("ButtonHeaderPassword"), Button)
            ButtonHeaderPassword.Text = Convert.ToString(ResourceTable("ButtonHeaderPassword"), Nothing).Trim()
            ButtonHeaderPassword.Text = If(String.IsNullOrEmpty(ButtonHeaderPassword.Text), "Password", ButtonHeaderPassword.Text)

            ButtonHeaderFtpSite = DirectCast(CurrentRow.FindControl("ButtonHeaderFtpSite"), Button)
            ButtonHeaderFtpSite.Text = Convert.ToString(ResourceTable("ButtonHeaderFtpSite"), Nothing).Trim()
            ButtonHeaderFtpSite.Text = If(String.IsNullOrEmpty(ButtonHeaderFtpSite.Text), "Ftp Site", ButtonHeaderFtpSite.Text)

            ButtonHeaderDirectory = DirectCast(CurrentRow.FindControl("ButtonHeaderDirectory"), Button)
            ButtonHeaderDirectory.Text = Convert.ToString(ResourceTable("ButtonHeaderDirectory"), Nothing).Trim()
            ButtonHeaderDirectory.Text = If(String.IsNullOrEmpty(ButtonHeaderDirectory.Text), "Directory", ButtonHeaderDirectory.Text)

            ButtonHeaderStatus = DirectCast(CurrentRow.FindControl("ButtonHeaderStatus"), Button)
            ButtonHeaderStatus.Text = Convert.ToString(ResourceTable("ButtonHeaderStatus"), Nothing).Trim()
            ButtonHeaderStatus.Text = If(String.IsNullOrEmpty(ButtonHeaderStatus.Text), "Status", ButtonHeaderStatus.Text)

            Dim LabelHeaderUpdateDate As Label = DirectCast(CurrentRow.FindControl("LabelHeaderUpdateDate"), Label)
            LabelHeaderUpdateDate.Text = Convert.ToString(ResourceTable("LabelHeaderUpdateDate"), Nothing).Trim()
            LabelHeaderUpdateDate.Text = If(String.IsNullOrEmpty(LabelHeaderUpdateDate.Text), "Update Date", LabelHeaderUpdateDate.Text)

            Dim LabelHeaderUpdateBy As Label = DirectCast(CurrentRow.FindControl("LabelHeaderUpdateBy"), Label)
            LabelHeaderUpdateBy.Text = Convert.ToString(ResourceTable("LabelHeaderUpdateBy"), Nothing).Trim()
            LabelHeaderUpdateBy.Text = If(String.IsNullOrEmpty(LabelHeaderUpdateBy.Text), "Update By", LabelHeaderUpdateBy.Text)

            ResourceTable = Nothing

        End Sub

        ''' <summary>
        ''' Returns sorted list according to the sorting expression
        ''' </summary>
        ''' <param name="GridResults"></param>
        ''' <remarks></remarks>
        Private Sub GetSortedEDILoginInfo(ByRef GridResults As List(Of EDILoginDataObject))

            Dim SortingExpression As String = Convert.ToString(Session("SortingExpression"), Nothing)

            If GridResults IsNot Nothing Then

                Dim param = Expression.Parameter(GetType(EDILoginDataObject), SortingExpression)
                Dim SortExpression = Expression.Lambda(Of Func(Of EDILoginDataObject, Object))(Expression.Convert(Expression.Property(param, SortingExpression),
                                                                                                                  GetType(Object)), param)

                If GridViewSortDirection = SortDirection.Ascending Then

                    GridResults = GridResults.AsQueryable().OrderBy(SortExpression).ToList()
                    GridViewSortDirection = SortDirection.Descending
                Else
                    GridResults = GridResults.AsQueryable().OrderByDescending(SortExpression).ToList()
                    GridViewSortDirection = SortDirection.Ascending
                End If

            End If

        End Sub

    End Class
End Namespace
