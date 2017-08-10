
#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Hospital Stay Setup 
' Author: Anjan Kumar Paul
' Start Date: 04 Jan 2015
' End Date: 04 Jan 2015
' History:
'      Version                  Author                      Date             Reason 
'      1.0.0                                                04 Jan 2015      Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports VisitelCommon.VisitelCommon
Imports System.Data.SqlClient
Imports VisitelBusiness.VisitelBusiness.Settings
Imports VisitelBusiness.VisitelBusiness.DataObject.Settings
Imports VisitelBusiness.VisitelBusiness.DataObject

Namespace Visitel.UserControl.Settings

    Public Class HospitalStaySetting
        Inherits BaseUserControl

        Private ValidationEnable As Boolean
        Private ValidationGroup As String

        Private HospitalStayValidationGroup As String

        Private GridViewPageSize As Integer

        Private ControlName As String, EditButtonText As String, UpdateButtonText As String, CancelButtonText As String, DeleteText As String, ButtonAddText As String, _
            InsertMessage As String, UpdateMessage As String, DeleteMessage As String

        Private ButtonEdit As Button, ButtonUpdate As Button, ButtonCancel As Button, RowButtonDelete As Button, ButtonAdd As Button


        Private DropDownListIndividualEdit As DropDownList, DropDownListIndividualInsert As DropDownList

        Private LabelIndividualId As Label, LabelHospitalStayId As Label

        Private TextBoxStartDateEdit As TextBox, TextBoxEndDateEdit As TextBox, TextBoxReasonEdit As TextBox, TextBoxStartDateInsert As TextBox,
            TextBoxEndDateInsert As TextBox, TextBoxReasonInsert As TextBox

        Private CurrentRow As GridViewRow

        Private objShared As SharedWebControls

        Private UserId As Integer
        Private CompanyId As Integer
        Private ConVisitel As SqlConnection

        Protected Sub Page_Init(sender As Object, e As EventArgs)

            ControlName = "HospitalStaySetting"

            DirectCast(Me.Page.Master, IMyMasterPage).PageHeaderTitle = "Hospital Stay"

            objShared = New SharedWebControls()

            UserId = objShared.UserId
            CompanyId = objShared.CompanyId

            objShared.ConnectionOpen()
            ConVisitel = objShared.ConVisitel

            SetGridViewHeaderStyle()
            GetCaptionFromResource()
            InitializeControl()

        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)
            If Not IsPostBack Then
                GetData()
            End If
        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadCss("Settings/" + ControlName)
            LoadJScript()
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
        End Sub

        Private Sub ButtonSave_Click(sender As Object, e As EventArgs)

            'Page.Validate()
            'Page.Validate(ValidationGroup)

            'If ((Page.IsValid) And (ValidateData())) Then
            '    Dim IsSaved As Boolean = False

            '    Try
            '        IsSaved = True
            '    Catch ex As SqlException
            '        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to Save")
            '    End Try

            '    If (IsSaved) Then
            '        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Saved Successfully.")
            '    End If

            'End If

        End Sub

        ''' <summary>
        ''' Reading GridView header text from resource file
        ''' </summary>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub SetGridViewColumnHeaderText(ByRef e As GridViewRowEventArgs)

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("Setup", ControlName & Convert.ToString(".resx"))

            CurrentRow = DirectCast(e.Row, GridViewRow)

            Dim LabelHeaderIndividualName As Label = DirectCast(CurrentRow.FindControl("LabelHeaderIndividualName"), Label)
            LabelHeaderIndividualName.Text = Convert.ToString(ResourceTable("HeaderIndividualName"), Nothing).Trim()
            LabelHeaderIndividualName.Text = If(String.IsNullOrEmpty(LabelHeaderIndividualName.Text), "Individual", LabelHeaderIndividualName.Text)

            Dim LabelHeaderStartDate As Label = DirectCast(CurrentRow.FindControl("LabelHeaderStartDate"), Label)
            LabelHeaderStartDate.Text = Convert.ToString(ResourceTable("HeaderStartDate"), Nothing).Trim()
            LabelHeaderStartDate.Text = If(String.IsNullOrEmpty(LabelHeaderStartDate.Text), "Start Date", LabelHeaderStartDate.Text)

            Dim LabelHeaderEndDate As Label = DirectCast(CurrentRow.FindControl("LabelHeaderEndDate"), Label)
            LabelHeaderEndDate.Text = Convert.ToString(ResourceTable("HeaderEndDate"), Nothing).Trim()
            LabelHeaderEndDate.Text = If(String.IsNullOrEmpty(LabelHeaderEndDate.Text), "End Date", LabelHeaderEndDate.Text)

            'Dim LabelHeaderMedicaidNumber As Label = DirectCast(e.Row.Cells(0).FindControl("LabelHeaderMedicaidNumber"), Label)
            'LabelHeaderMedicaidNumber.Text = Convert.ToString(ResourceTable("HeaderMedicaidNumber"), Nothing).Trim()
            'LabelHeaderMedicaidNumber.Text = If(String.IsNullOrEmpty(LabelHeaderMedicaidNumber.Text), "Medicaid No", LabelHeaderMedicaidNumber.Text)

            'Dim LabelHeaderComment As Label = DirectCast(e.Row.Cells(0).FindControl("LabelHeaderComment"), Label)
            'LabelHeaderComment.Text = Convert.ToString(ResourceTable("HeaderComment"), Nothing).Trim()
            'LabelHeaderComment.Text = If(String.IsNullOrEmpty(LabelHeaderComment.Text), "Comment", LabelHeaderComment.Text)

            'Dim LabelHeaderReason As Label = DirectCast(CurrentRow.FindControl("LabelHeaderReason"), Label)
            'LabelHeaderReason.Text = Convert.ToString(ResourceTable("HeaderReason"), Nothing).Trim()
            'LabelHeaderReason.Text = If(String.IsNullOrEmpty(LabelHeaderReason.Text), "Reason", LabelHeaderReason.Text)

            Dim LabelHeaderUpdateDate As Label = DirectCast(CurrentRow.FindControl("LabelHeaderUpdateDate"), Label)
            LabelHeaderUpdateDate.Text = Convert.ToString(ResourceTable("HeaderUpdateDate"), Nothing).Trim()
            LabelHeaderUpdateDate.Text = If(String.IsNullOrEmpty(LabelHeaderUpdateDate.Text), "Update Date", LabelHeaderUpdateDate.Text)

            Dim LabelHeaderUpdateBy As Label = DirectCast(CurrentRow.FindControl("LabelHeaderUpdateBy"), Label)
            LabelHeaderUpdateBy.Text = Convert.ToString(ResourceTable("HeaderUpdateBy"), Nothing).Trim()
            LabelHeaderUpdateBy.Text = If(String.IsNullOrEmpty(LabelHeaderUpdateBy.Text), "Update By", LabelHeaderUpdateBy.Text)

            Dim LabelHeaderEdit As Label = DirectCast(CurrentRow.FindControl("LabelHeaderEdit"), Label)
            LabelHeaderEdit.Text = Convert.ToString(ResourceTable("HeaderEdit"), Nothing).Trim()
            LabelHeaderEdit.Text = If(String.IsNullOrEmpty(LabelHeaderEdit.Text), "Edit", LabelHeaderEdit.Text)

            Dim LabelHeaderDelete As Label = DirectCast(CurrentRow.FindControl("LabelHeaderDelete"), Label)
            LabelHeaderDelete.Text = Convert.ToString(ResourceTable("HeaderDelete"), Nothing).Trim()
            LabelHeaderDelete.Text = If(String.IsNullOrEmpty(LabelHeaderDelete.Text), "Delete", LabelHeaderDelete.Text)

            ResourceTable = Nothing

        End Sub

        Private Sub GridViewHospitalStay_RowDataBound(sender As Object, e As GridViewRowEventArgs)

            If (e.Row.RowType.Equals(DataControlRowType.Header)) Then
                SetGridViewColumnHeaderText(e)
            End If

            If (e.Row.RowType.Equals(DataControlRowType.DataRow)) Then

                CurrentRow = DirectCast(e.Row, GridViewRow)

                objShared.SetGridViewRowColor(CurrentRow)

                ButtonEdit = DirectCast(CurrentRow.FindControl("ButtonEdit"), Button)

                If (Not ButtonEdit Is Nothing) Then
                    ButtonEdit.CommandName = "Edit"
                    ButtonEdit.Text = EditButtonText
                End If

                ButtonUpdate = DirectCast(CurrentRow.FindControl("ButtonUpdate"), Button)

                If (Not ButtonUpdate Is Nothing) Then
                    ButtonUpdate.CommandName = "Update"
                    ButtonUpdate.Text = UpdateButtonText
                    ButtonUpdate.CausesValidation = True
                    ButtonUpdate.ValidationGroup = HospitalStayValidationGroup
                    ButtonUpdate.OnClientClick = "javascript:return UpdateConfirmationFromGrid(this.name,this.alt);"
                End If

                ButtonCancel = DirectCast(CurrentRow.FindControl("ButtonCancel"), Button)

                If (Not ButtonCancel Is Nothing) Then
                    ButtonCancel.CommandName = "Cancel"
                    ButtonCancel.Text = CancelButtonText
                    ButtonCancel.CausesValidation = False
                End If

                RowButtonDelete = DirectCast(CurrentRow.FindControl("ButtonDelete"), Button)

                If (Not RowButtonDelete Is Nothing) Then
                    RowButtonDelete.CommandName = "Delete"
                    RowButtonDelete.OnClientClick = "javascript:return DeleteConfirmationFromGrid(this.name,this.alt);"
                    RowButtonDelete.Text = DeleteText
                    RowButtonDelete.CausesValidation = False

                End If

                LabelIndividualId = DirectCast(CurrentRow.FindControl("LabelIndividualId"), Label)

                '**************************Fill Out Drop Down and Associate selection on Edit Mode[Start]***********************************
                If ((e.Row.RowState & DataControlRowState.Edit) > 0) Then
                    DropDownListIndividualEdit = DirectCast(CurrentRow.FindControl("DropDownListIndividualEdit"), DropDownList)

                    If (Not DropDownListIndividualEdit Is Nothing) Then

                        objShared.BindClientDropDownList(DropDownListIndividualEdit, CompanyId, EnumDataObject.ClientListFor.Individual)

                        DropDownListIndividualEdit.SelectedIndex = DropDownListIndividualEdit.Items.IndexOf(
                            DropDownListIndividualEdit.Items.FindByValue(Convert.ToString(LabelIndividualId.Text.Trim(), Nothing)))

                    End If
                End If
                '**************************Fill Out Drop Down and Associate selection on Edit Mode[End]***********************************

            End If

            If (e.Row.RowType.Equals(DataControlRowType.Footer)) Then
                ButtonAdd = DirectCast(CurrentRow.FindControl("ButtonAdd"), Button)

                If (Not ButtonAdd Is Nothing) Then
                    ButtonAdd.CommandName = "AddNew"
                    ButtonAdd.ValidationGroup = "validaiton"
                    ButtonAdd.Text = ButtonAddText
                End If
            End If
        End Sub

        Private Sub GridViewHospitalStay_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
            GridViewHospitalStay.PageIndex = e.NewPageIndex
            BindHospitalStayGridView()
        End Sub

        Private Sub GridViewHospitalStay_RowEditing(sender As Object, e As GridViewEditEventArgs)
            GridViewHospitalStay.EditIndex = e.NewEditIndex
            BindHospitalStayGridView()
        End Sub

        Private Sub GridViewHospitalStay_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs)
            GridViewHospitalStay.EditIndex = -1
            BindHospitalStayGridView()
        End Sub

        Private Sub GridViewHospitalStay_RowUpdating(sender As Object, e As GridViewUpdateEventArgs)

            CurrentRow = GridViewHospitalStay.Rows(e.RowIndex)

            DropDownListIndividualEdit = DirectCast(CurrentRow.FindControl("DropDownListIndividualEdit"), DropDownList)
            TextBoxStartDateEdit = DirectCast(CurrentRow.FindControl("TextBoxStartDateEdit"), TextBox)
            TextBoxEndDateEdit = DirectCast(CurrentRow.FindControl("TextBoxEndDateEdit"), TextBox)
            'TextBoxReasonEdit = DirectCast(CurrentRow.FindControl("TextBoxReasonEdit"), TextBox)

            LabelHospitalStayId = DirectCast(CurrentRow.FindControl("LabelHospitalStayId"), Label)

            'RegularExpressionValidatorHourMinute = DirectCast(GridViewHospitalStay.Rows(e.RowIndex).FindControl("RegularExpressionValidatorHourMinute"), 
            '                                            RegularExpressionValidator)
            'RegularExpressionValidatorInTime = DirectCast(GridViewHospitalStay.Rows(e.RowIndex).FindControl("RegularExpressionValidatorInTime"), 
            '                                            RegularExpressionValidator)
            'RegularExpressionValidatorOutTime = DirectCast(GridViewHospitalStay.Rows(e.RowIndex).FindControl("RegularExpressionValidatorOutTime"), 
            '                                            RegularExpressionValidator)
            'RegularExpressionValidatorSpecialRate = DirectCast(GridViewHospitalStay.Rows(e.RowIndex).FindControl("RegularExpressionValidatorSpecialRate"), 
            '                                            RegularExpressionValidator)

            Page.Validate()
            Page.Validate(ValidationGroup)

            If ((Page.IsValid) And (ValidateData())) Then

                Dim objHospitalStaySettingDataObject As New HospitalStaySettingDataObject

                objHospitalStaySettingDataObject.HospitalStayId = Convert.ToInt64(LabelHospitalStayId.Text.Trim())
                objHospitalStaySettingDataObject.IndividualId = Convert.ToInt64(DropDownListIndividualEdit.SelectedValue)
                objHospitalStaySettingDataObject.StartDate = Convert.ToString(TextBoxStartDateEdit.Text.Trim(), Nothing)
                objHospitalStaySettingDataObject.EndDate = Convert.ToString(TextBoxEndDateEdit.Text.Trim(), Nothing)
                'objHospitalStaySettingDataObject.Reason = Convert.ToString(TextBoxReasonEdit.Text.Trim(), Nothing)

                objHospitalStaySettingDataObject.CompanyId = CompanyId
                objHospitalStaySettingDataObject.UpdateBy = UserId

                Dim IsSaved As Boolean = False
                Dim objBLHospitalStaySetting As New BLHospitalStaySetting

                Try
                    objBLHospitalStaySetting.UpdateHospitalStayInfo(ConVisitel, objHospitalStaySettingDataObject)
                    IsSaved = True
                Catch ex As SqlException
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(ex.Message)
                End Try

                objBLHospitalStaySetting = Nothing

                If (IsSaved) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Saved Successfully")
                    GridViewHospitalStay.EditIndex = -1
                    BindHospitalStayGridView()
                End If
            End If
        End Sub

        Private Sub GridViewHospitalStay_RowDeleting(sender As Object, e As GridViewDeleteEventArgs)

            CurrentRow = GridViewHospitalStay.Rows(e.RowIndex)

            LabelHospitalStayId = DirectCast(CurrentRow.FindControl("LabelHospitalStayId"), Label)

            Dim objBLHospitalStaySetting As New BLHospitalStaySetting
            Dim IsDeleted As Boolean = False

            Try
                objBLHospitalStaySetting.DeleteHospitalStayInfo(ConVisitel, Convert.ToInt64(LabelHospitalStayId.Text.Trim()), UserId, CompanyId)
                IsDeleted = True
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(ex.Message)
            End Try

            objBLHospitalStaySetting = Nothing

            If (IsDeleted) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Successfully Deleted")
                BindHospitalStayGridView()
            End If

        End Sub

        Private Sub ButtonDelete_Click(sender As Object, e As EventArgs)

        End Sub

        Protected Sub LinkButtonDelete_Click(sender As Object, e As EventArgs)
            'Dim LinkButtonDelete As LinkButton = DirectCast(sender, LinkButton)
            'Dim dtgItem As GridViewRow = DirectCast(LinkButtonDelete.NamingContainer, GridViewRow)
            'Dim LabelCityId As Label = DirectCast(dtgItem.Cells(0).FindControl("LabelCityId"), Label)

            'Dim objBLCitySetting As New BLCitySetting()
            'objBLCitySetting.DeleteCityInfo(ConVisitel, Convert.ToInt32(LabelCityId.Text), UserId)
            'objBLCitySetting = Nothing
            'Master.DisplayHeaderMessage(DeleteMessage)

            'BindCityInformationGridView()
            'HiddenFieldIsNew.Value = Convert.ToString(False, Nothing)
            'HiddenFieldCityId.Value = Convert.ToString(Int32.MinValue)
        End Sub

        ''' <summary>
        ''' Gridview Row Hover Color Set
        ''' </summary>
        ''' <param name="writer"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub Render(writer As HtmlTextWriter)
            For Each r As GridViewRow In GridViewHospitalStay.Rows
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

            ButtonSave.ValidationGroup = ValidationGroup
            ButtonSave.CausesValidation = True

            AddHandler ButtonSave.Click, AddressOf ButtonSave_Click

            GridViewHospitalStay.AutoGenerateColumns = False
            GridViewHospitalStay.ShowHeaderWhenEmpty = True
            GridViewHospitalStay.AllowPaging = True
            GridViewHospitalStay.ShowFooter = True

            If (GridViewHospitalStay.AllowPaging) Then
                GridViewHospitalStay.PageSize = GridViewPageSize
            End If

            AddHandler GridViewHospitalStay.RowDataBound, AddressOf GridViewHospitalStay_RowDataBound
            AddHandler GridViewHospitalStay.PageIndexChanging, AddressOf GridViewHospitalStay_PageIndexChanging
            'AddHandler GridViewHospitalStay.SelectedIndexChanged, AddressOf GridViewPayPeriodDetail_SelectedIndexChanged

            AddHandler GridViewHospitalStay.RowCommand, AddressOf GridViewHospitalStay_RowCommand

            AddHandler GridViewHospitalStay.RowCreated, AddressOf GridViewHospitalStay_RowCreated
            AddHandler GridViewHospitalStay.RowEditing, AddressOf GridViewHospitalStay_RowEditing
            AddHandler GridViewHospitalStay.RowCancelingEdit, AddressOf GridViewHospitalStay_RowCancelingEdit
            AddHandler GridViewHospitalStay.RowUpdating, AddressOf GridViewHospitalStay_RowUpdating
            AddHandler GridViewHospitalStay.RowDeleting, AddressOf GridViewHospitalStay_RowDeleting

        End Sub

        ''' <summary>
        ''' This is for validating data in server side before submitting the form
        ''' </summary>
        ''' <returns></returns>
        Private Function ValidateData() As Boolean

            Return True

        End Function

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("Setup", ControlName & Convert.ToString(".resx"))

            ButtonSave.Text = Convert.ToString(ResourceTable("ButtonSave"), Nothing).Trim()
            ButtonSave.Text = If(String.IsNullOrEmpty(ButtonSave.Text), "Save", ButtonSave.Text)

            DeleteText = Convert.ToString(ResourceTable("DeleteText"), Nothing).Trim()
            DeleteText = If(String.IsNullOrEmpty(DeleteText), "Delete", DeleteText)

            ButtonAddText = Convert.ToString(ResourceTable("ButtonAddText"), Nothing).Trim()
            ButtonAddText = If(String.IsNullOrEmpty(ButtonAddText), "Add New", ButtonAddText)

            EditButtonText = Convert.ToString(ResourceTable("EditButtonText"), Nothing).Trim()
            EditButtonText = If(String.IsNullOrEmpty(EditButtonText), "Edit", EditButtonText)

            UpdateButtonText = Convert.ToString(ResourceTable("UpdateButtonText"), Nothing).Trim()
            UpdateButtonText = If(String.IsNullOrEmpty(UpdateButtonText), "Update", UpdateButtonText)

            CancelButtonText = Convert.ToString(ResourceTable("CancelButtonText"), Nothing).Trim()
            CancelButtonText = If(String.IsNullOrEmpty(CancelButtonText), "Cancel", CancelButtonText)

            Dim ResultOut As Boolean

            ValidationEnable = If((Boolean.TryParse(ResourceTable("ValidationEnable"), ResultOut)), ResultOut, True)

            ValidationGroup = Convert.ToString(ResourceTable("ValidationGroup"), Nothing)
            ValidationGroup = If(String.IsNullOrEmpty(ValidationGroup), "HospitalStay", ValidationGroup)

            Dim ResultOutInteger As Integer

            GridViewPageSize = If((Integer.TryParse(ResourceTable("PageSize"), ResultOutInteger)), ResultOutInteger, 10)
            GridViewPageSize = If((GridViewPageSize < 1), 10, GridViewPageSize)

            ResourceTable = Nothing

        End Sub

        ''' <summary>
        ''' Setting GridView Header Style Using CSS
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub SetGridViewHeaderStyle()

            GridViewHospitalStay.Columns(0).HeaderStyle.CssClass = "HeaderIndividualName"
            GridViewHospitalStay.Columns(1).HeaderStyle.CssClass = "HeaderStartDate"
            GridViewHospitalStay.Columns(2).HeaderStyle.CssClass = "HeaderEndDate"
            GridViewHospitalStay.Columns(3).HeaderStyle.CssClass = "HeaderReason"
            GridViewHospitalStay.Columns(4).HeaderStyle.CssClass = "HeaderUpdateDate"
            GridViewHospitalStay.Columns(5).HeaderStyle.CssClass = "HeaderUpdateBy"
            GridViewHospitalStay.Columns(6).HeaderStyle.CssClass = "HeaderEdit"

        End Sub

        ''' <summary>
        ''' This is for retrieving data
        ''' </summary>
        Private Sub GetData()

            Try
                BindHospitalStayGridView()
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to Fetch Data")
            End Try

        End Sub

        ''' <summary>
        ''' Loading Javascript
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub LoadJScript()

            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                           & " var DeleteDialogHeader ='Hospital Stay: Delete'; " _
                           & " var DeleteDialogConfirmMsg ='Do you want to delete this record?'; " _
                           & " var UpdateDialogHeader ='Hospital Stay: Update'; " _
                           & " var UpdateDialogConfirmMsg ='Are you sure to update this record?'; " _
                           & " var CalendarDateFormat='dd-M-y'; " _
                           & " var CalendarImagePath='" & objShared.GetCalendarImagePath & "'; " _
                           & " jQuery(document).ready(function () {" _
                           & "     DateFieldsEvent();" _
                           & "     Sys.WebForms.PageRequestManager.getInstance().add_endRequest(DateFieldsEvent); " _
                           & "}); " _
                    & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

            LoadJS("JavaScript/Settings/" & ControlName & ".js")

        End Sub

        ''' <summary>
        ''' Data binding for Hospital Stay GridView
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindHospitalStayGridView()

            Dim GridResults As List(Of HospitalStaySettingDataObject)
            Dim objBLHospitalStaySetting As New BLHospitalStaySetting

            GridResults = objBLHospitalStaySetting.SelectHospitalStayInfo(ConVisitel, CompanyId)

            objBLHospitalStaySetting = Nothing

            GridViewHospitalStay.DataSource = GridResults
            GridViewHospitalStay.DataBind()

        End Sub

        Private Sub GridViewHospitalStay_RowCreated(sender As Object, e As GridViewRowEventArgs)

            CurrentRow = e.Row

            ButtonAdd = DirectCast(CurrentRow.FindControl("ButtonAdd"), Button)

            Dim DropDownListIndividualInsert As DropDownList = DirectCast(CurrentRow.FindControl("DropDownListIndividualInsert"), DropDownList)

            If (Not DropDownListIndividualInsert Is Nothing) Then
                objShared.BindClientDropDownList(DropDownListIndividualInsert, CompanyId, EnumDataObject.ClientListFor.Individual)
            End If

            
        End Sub

        Private Sub GridViewHospitalStay_RowCommand(sender As Object, e As GridViewCommandEventArgs)

            If (e.CommandName.Equals("AddNew")) Then

                CurrentRow = GridViewHospitalStay.FooterRow

                DropDownListIndividualInsert = DirectCast(CurrentRow.FindControl("DropDownListIndividualInsert"), DropDownList)
                TextBoxStartDateInsert = DirectCast(CurrentRow.FindControl("TextBoxStartDateInsert"), TextBox)
                TextBoxEndDateInsert = DirectCast(CurrentRow.FindControl("TextBoxEndDateInsert"), TextBox)
                'TextBoxReasonInsert = DirectCast(CurrentRow.FindControl("TextBoxReasonInsert"), TextBox)

                'RegularExpressionValidatorHourMinute = DirectCast(GridViewHospitalStay.Rows(e.RowIndex).FindControl("RegularExpressionValidatorHourMinute"), 
                '                                            RegularExpressionValidator)
                'RegularExpressionValidatorInTime = DirectCast(GridViewHospitalStay.Rows(e.RowIndex).FindControl("RegularExpressionValidatorInTime"), 
                '                                            RegularExpressionValidator)
                'RegularExpressionValidatorOutTime = DirectCast(GridViewHospitalStay.Rows(e.RowIndex).FindControl("RegularExpressionValidatorOutTime"), 
                '                                            RegularExpressionValidator)
                'RegularExpressionValidatorSpecialRate = DirectCast(GridViewHospitalStay.Rows(e.RowIndex).FindControl("RegularExpressionValidatorSpecialRate"), 
                '                                            RegularExpressionValidator)

                Page.Validate()
                Page.Validate(ValidationGroup)

                If ((Page.IsValid) And (ValidateData())) Then

                    Dim objHospitalStaySettingDataObject As New HospitalStaySettingDataObject

                    objHospitalStaySettingDataObject.IndividualId = Convert.ToInt64(DropDownListIndividualInsert.SelectedValue)
                    objHospitalStaySettingDataObject.StartDate = Convert.ToString(TextBoxStartDateInsert.Text.Trim(), Nothing)
                    objHospitalStaySettingDataObject.EndDate = Convert.ToString(TextBoxEndDateInsert.Text.Trim(), Nothing)
                    'objHospitalStaySettingDataObject.Reason = Convert.ToString(TextBoxReasonInsert.Text.Trim(), Nothing)

                    objHospitalStaySettingDataObject.CompanyId = CompanyId
                    objHospitalStaySettingDataObject.UserId = UserId

                    Dim IsSaved As Boolean = False
                    Dim objBLHospitalStaySetting As New BLHospitalStaySetting

                    Try
                        objBLHospitalStaySetting.InsertHospitalStayInfo(ConVisitel, objHospitalStaySettingDataObject)
                        IsSaved = True
                    Catch ex As SqlException
                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(ex.Message)
                    End Try

                    objBLHospitalStaySetting = Nothing

                    If (IsSaved) Then
                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Saved Successfully")
                        GridViewHospitalStay.EditIndex = -1
                        BindHospitalStayGridView()
                    End If
                End If
            End If

        End Sub

    End Class
End Namespace