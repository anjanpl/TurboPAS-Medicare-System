
#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: EDI Codes Save & Delete
' Author: Anjan Kumar Paul
' Start Date: 10 July 2015
' End Date: 15 July 2015
' History:
'      Version                  Author                      Date            Reason 
'      1.0.0                                                10 July 2015     Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports System.Data.SqlClient
Imports System.Linq.Expressions

Namespace Visitel.UserControl.EDICodes
    Public Class EDICodesControl
        Inherits BaseUserControl

        Private ControlName As String, ValidationGroup As String, SaveMessage As String, DeleteMessage As String, DeleteConfirmationMessage As String, ListFor As String
        Private ValidationEnable As Boolean
        Private CurrentRow As GridViewRow

        Private objShared As SharedWebControls

        Private LabelId As Label
        Private TextBoxCode As TextBox, TextBoxDefinition As TextBox, TextBoxTable As TextBox, TextBoxColumn As TextBox, TextBoxUpdateDate As TextBox,
            TextBoxUpdateBy As TextBox
        Private CheckBoxSelect As CheckBox, chkAll As CheckBox

        Private ButtonHeaderCode As Button, ButtonHeaderDefinition As Button, ButtonHeaderTable As Button, ButtonHeaderColumn As Button

        Private EDICodesList As List(Of EDICodesDataObject)
        Private EDICodesErrorList As List(Of EDICodesDataObject)
        Private SaveFailedCounter As Int16, DeleteFailedCounter As Int16

        Protected Sub Page_Init(sender As Object, e As EventArgs)

            DirectCast(Me.Page.Master, IMyMasterPage).PageHeaderTitle = "EDI Codes"
            ControlName = "EDICodesControl"
            objShared = New SharedWebControls()
            objShared.ConnectionOpen()
            GetCaptionFromResource()
            InitializeControl()

        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)
            If Not IsPostBack Then
                Session.Add("SortingExpression", "Code")
                GetData()
            End If
        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadCss("EDICodes/" & ControlName)
            LoadJScript()
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing
        End Sub

        Private Sub ButtonClear_Click(sender As Object, e As EventArgs)

            ViewState.Clear()

            ButtonViewError.Visible = False
            BindEDICodesGridView()

        End Sub

        Protected Sub OnCheckedChanged(sender As Object, e As EventArgs)

            Dim isUpdateVisible As Boolean = False

            Dim chk As CheckBox = TryCast(sender, CheckBox)

            'ButtonSave.Enabled = If((chk.Checked), True, False)

            If chk.ID = "chkAll" Then
                For Each row As GridViewRow In GridViewEDICodes.Rows
                    If row.RowType = DataControlRowType.DataRow Then
                        row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked = chk.Checked
                    End If
                Next

                CurrentRow = GridViewEDICodes.FooterRow
                CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)
                CheckBoxSelect.Checked = chk.Checked

            End If

            Dim chkAll As CheckBox = TryCast(GridViewEDICodes.HeaderRow.FindControl("chkAll"), CheckBox)
            'chkAll.Checked = True
            For Each row As GridViewRow In GridViewEDICodes.Rows
                If row.RowType = DataControlRowType.DataRow Then

                    Dim isChecked As Boolean = row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked

                    ControlsOnSelect(row, isChecked)

                    If (isChecked) Then
                        'SetHiddenControlValue(row)
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

            DetermineButtonInActivity()

        End Sub

        Private Sub ButtonSave_Click(sender As Object, e As EventArgs)

            Page.Validate()
            Page.Validate(ValidationGroup)

            EDICodesList = New List(Of EDICodesDataObject)()
            EDICodesErrorList = New List(Of EDICodesDataObject)()
            SaveFailedCounter = 0

            CurrentRow = GridViewEDICodes.FooterRow

            CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)

            TextBoxCode = DirectCast(CurrentRow.FindControl("TextBoxCode"), TextBox)

            If ((CheckBoxSelect.Checked) And (Not String.IsNullOrEmpty(TextBoxCode.Text.Trim()))) Then
                SaveData(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT))
            End If

            SaveData(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE))

            If (SaveFailedCounter > 0) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} Records Failed to Save out of {1}", SaveFailedCounter, EDICodesList.Count))
                ViewState("SaveFailedRecord") = objShared.ToDataTable(EDICodesErrorList)
                ViewState("ListFor") = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)
            Else
                ViewState("SaveFailedRecord") = Nothing

                If (EDICodesList.Count > 0) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} record(s) {1}.", EDICodesList.Count, SaveMessage))
                    ViewState.Clear()
                    BindEDICodesGridView()
                End If

            End If

            ButtonViewError.Visible = If((SaveFailedCounter > 0), True, False)

            EDICodesList = Nothing
            EDICodesErrorList = Nothing

        End Sub

        Private Sub ButtonDelete_Click(sender As Object, e As EventArgs)

            EDICodesList = New List(Of EDICodesDataObject)()
            EDICodesErrorList = New List(Of EDICodesDataObject)()

            MakeList(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE))

            If (EDICodesList.Count.Equals(0)) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please select items to delete")
                Return
            End If

            DeleteFailedCounter = 0

            TakeAction(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE))

            If (DeleteFailedCounter > 0) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} Records Failed to Delete out of {1}", DeleteFailedCounter, EDICodesList.Count))
                ViewState("DeleteFailedRecord") = objShared.ToDataTable(EDICodesErrorList)
                ViewState("ListFor") = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE)
            Else
                ViewState("DeleteFailedRecord") = Nothing

                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} record(s) {1}.", EDICodesList.Count, DeleteMessage))
                ViewState.Clear()
                BindEDICodesGridView()
            End If

            ButtonViewError.Visible = If((DeleteFailedCounter > 0), True, False)

            EDICodesList = Nothing
            EDICodesErrorList = Nothing

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

        Private Sub GridViewEDICodes_RowDataBound(sender As Object, e As GridViewRowEventArgs)

            CurrentRow = DirectCast(e.Row, GridViewRow)

            If (CurrentRow.RowType.Equals(DataControlRowType.Header)) Then
                SetGridViewColumnHeaderText(CurrentRow)

                chkAll = DirectCast(CurrentRow.FindControl("chkAll"), CheckBox)
                chkAll.AutoPostBack = True
                chkAll.ClientIDMode = UI.ClientIDMode.Static

                ButtonHeaderCode = DirectCast(CurrentRow.FindControl("ButtonHeaderCode"), Button)
                ButtonHeaderCode.CommandName = "Sort"
                ButtonHeaderCode.CommandArgument = "Code"

                ButtonHeaderDefinition = DirectCast(CurrentRow.FindControl("ButtonHeaderDefinition"), Button)
                ButtonHeaderDefinition.CommandName = "Sort"
                ButtonHeaderDefinition.CommandArgument = "CodeDefinition"

                ButtonHeaderTable = DirectCast(CurrentRow.FindControl("ButtonHeaderTable"), Button)
                ButtonHeaderTable.CommandName = "Sort"
                ButtonHeaderTable.CommandArgument = "CodeTable"

                ButtonHeaderColumn = DirectCast(CurrentRow.FindControl("ButtonHeaderColumn"), Button)
                ButtonHeaderColumn.CommandName = "Sort"
                ButtonHeaderColumn.CommandArgument = "CodeColumn"

            End If

            If (CurrentRow.RowType.Equals(DataControlRowType.DataRow)) Then

                CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)
                CheckBoxSelect.AutoPostBack = True

                TextBoxCode = DirectCast(CurrentRow.FindControl("TextBoxCode"), TextBox)
                TextBoxCode.ReadOnly = True

                TextBoxDefinition = DirectCast(CurrentRow.FindControl("TextBoxDefinition"), TextBox)
                TextBoxDefinition.ReadOnly = True
                TextBoxDefinition.TextMode = TextBoxMode.MultiLine

                TextBoxTable = DirectCast(CurrentRow.FindControl("TextBoxTable"), TextBox)
                TextBoxTable.ReadOnly = True

                TextBoxColumn = DirectCast(CurrentRow.FindControl("TextBoxColumn"), TextBox)
                TextBoxColumn.ReadOnly = True

                TextBoxUpdateDate = DirectCast(CurrentRow.FindControl("TextBoxUpdateDate"), TextBox)
                TextBoxUpdateDate.ReadOnly = True

                TextBoxUpdateBy = DirectCast(CurrentRow.FindControl("TextBoxUpdateBy"), TextBox)
                TextBoxUpdateBy.ReadOnly = True

            End If

            If (CurrentRow.RowType.Equals(DataControlRowType.Footer)) Then

                CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)
                CheckBoxSelect.AutoPostBack = True

                TextBoxDefinition = DirectCast(CurrentRow.FindControl("TextBoxDefinition"), TextBox)
                TextBoxDefinition.TextMode = TextBoxMode.MultiLine

                TextBoxUpdateDate = DirectCast(CurrentRow.FindControl("TextBoxUpdateDate"), TextBox)
                TextBoxUpdateDate.ReadOnly = True

                TextBoxUpdateBy = DirectCast(CurrentRow.FindControl("TextBoxUpdateBy"), TextBox)
                TextBoxUpdateBy.ReadOnly = True

            End If

        End Sub

        Private Sub GridViewEDICodes_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
            GridViewEDICodes.PageIndex = e.NewPageIndex
            BindEDICodesGridView()
        End Sub

        Private Sub GridViewEDICodes_Sorting(sender As Object, e As GridViewSortEventArgs)
            Session.Add("SortingExpression", e.SortExpression)
            BindEDICodesGridView()
        End Sub

        ''' <summary>
        ''' Gridview Row Hover Color Set
        ''' </summary>
        ''' <param name="writer"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub Render(writer As HtmlTextWriter)
            For Each r As GridViewRow In GridViewEDICodes.Rows
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

            GridViewEDICodes.AutoGenerateColumns = False
            GridViewEDICodes.ShowHeaderWhenEmpty = True
            GridViewEDICodes.AllowPaging = True
            GridViewEDICodes.AllowSorting = True

            GridViewEDICodes.ShowFooter = True

            If (GridViewEDICodes.AllowPaging) Then
                GridViewEDICodes.PageSize = objShared.GridViewDefaultPageSize
            End If

            AddHandler GridViewEDICodes.RowDataBound, AddressOf GridViewEDICodes_RowDataBound
            AddHandler GridViewEDICodes.PageIndexChanging, AddressOf GridViewEDICodes_PageIndexChanging
            AddHandler GridViewEDICodes.Sorting, AddressOf GridViewEDICodes_Sorting

            ButtonDelete.ClientIDMode = UI.ClientIDMode.Static

        End Sub

        Private Sub LoadJScript()

            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                           & " var DeleteTargetButton ='ButtonDelete'; " _
                           & " var DeleteDialogHeader ='EDI Codes'; " _
                           & " var DeleteDialogConfirmMsg ='" & DeleteConfirmationMessage & "'; " _
                           & " var prm =''; " _
                           & " jQuery(document).ready(function () {" _
                           & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                           & "     DataDelete();" _
                           & "     prm.add_endRequest(DataDelete); " _
                           & "}); " _
                    & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

            LoadJS("JavaScript/EDICodes/" & ControlName & ".js")

        End Sub

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("EDICodes", ControlName & Convert.ToString(".resx"))

            LabelEDICodes.Text = Convert.ToString(ResourceTable("LabelEDICodes"), Nothing)
            LabelEDICodes.Text = If(String.IsNullOrEmpty(LabelEDICodes.Text), "EDI Codes", LabelEDICodes.Text)

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
            ValidationGroup = If(String.IsNullOrEmpty(ValidationGroup), "EDICode", ValidationGroup)

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
            BindEDICodesGridView()
        End Sub

        ''' <summary>
        ''' EDI Codes Grid Data Bind
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindEDICodesGridView()

            Dim GridResults As List(Of EDICodesDataObject) = Nothing
            Dim objBLEDICodes As New BLEDICodes()

            Try
                GridResults = objBLEDICodes.SelectEDICodes(objShared.ConVisitel)
            Catch ex As SqlException

                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to fetch EDI Codes")
            Finally
                'GridViewEDICodes.ShowFooter = objBLEDICodes.ShowFooter

                'If (Not objBLEDICodes.ShowFooter) Then
                '    GridResults.Clear()
                'End If

                objBLEDICodes = Nothing
            End Try

            GetSortedEDICodes(GridResults)

            GridViewEDICodes.DataSource = GridResults
            GridViewEDICodes.DataBind()

            DetermineButtonInActivity()

        End Sub

        ''' <summary>
        ''' Making Save and Delete Button Active/InActive
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub DetermineButtonInActivity()

            Dim ItemChecked As Boolean = False

            For Each row As GridViewRow In GridViewEDICodes.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    ItemChecked = row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked
                    If (ItemChecked) Then
                        Exit For
                    End If
                End If
            Next

            If (Not ItemChecked) Then
                If (GridViewEDICodes.Rows.Count > 0) Then
                    CurrentRow = GridViewEDICodes.FooterRow
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

            Dim objEDICodesDataObject As EDICodesDataObject

            Select Case Action
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT)

                    objEDICodesDataObject = New EDICodesDataObject()
                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)
                    AddToList(CurrentRow, EDICodesList, objEDICodesDataObject)
                    objEDICodesDataObject = Nothing

                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE),
                    EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE)
                    For Each row As GridViewRow In GridViewEDICodes.Rows
                        If (row.RowType.Equals(DataControlRowType.DataRow)) Then
                            Dim isChecked As Boolean = row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked

                            If (isChecked) Then

                                objEDICodesDataObject = New EDICodesDataObject()

                                CurrentRow = DirectCast(GridViewEDICodes.Rows(row.RowIndex), GridViewRow)

                                If (Action.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE))) Then
                                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)
                                End If

                                If (Action.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE))) Then
                                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE)
                                End If


                                AddToList(CurrentRow, EDICodesList, objEDICodesDataObject)
                                objEDICodesDataObject = Nothing

                            End If

                        End If
                    Next

                    Exit Select
            End Select

            objEDICodesDataObject = Nothing

        End Sub

        ''' <summary>
        ''' Make Database operation either for record saving or deleting
        ''' </summary>
        ''' <param name="Action"></param>
        ''' <remarks></remarks>
        Private Sub TakeAction(Action As String)

            Dim objBLEDICodes As New BLEDICodes()

            For Each EDICode In EDICodesList
                Try
                    Select Case Action
                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT)
                            objBLEDICodes.InsertEDICodeInfo(objShared.ConVisitel, EDICode)
                            Exit Select

                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE)
                            objBLEDICodes.UpdateEDICodeInfo(objShared.ConVisitel, EDICode)
                            Exit Select

                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE)
                            objBLEDICodes.DeleteEDICodeInfo(objShared.ConVisitel, EDICode.Id, EDICode.UpdateBy)
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

                    EDICode.Remarks = ex.Message
                    EDICodesErrorList.Add(EDICode)
                End Try
            Next

            objBLEDICodes = Nothing

        End Sub

        ''' <summary>
        ''' Saving Data either insert or update after making list
        ''' </summary>
        ''' <param name="Action"></param>
        ''' <remarks></remarks>
        Private Sub SaveData(Action As String)

            MakeList(Action)

            If (EDICodesList.Count.Equals(0)) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please select items to save")
                Return
            End If

            TakeAction(Action)

        End Sub

        ''' <summary>
        ''' Making List in order to save records or delete
        ''' </summary>
        ''' <param name="CurrentRow"></param>
        ''' <param name="EDICodesList"></param>
        ''' <param name="objEDICodesDataObject"></param>
        ''' <remarks></remarks>
        Private Sub AddToList(ByRef CurrentRow As GridViewRow, ByRef EDICodesList As List(Of EDICodesDataObject), ByRef objEDICodesDataObject As EDICodesDataObject)

            Dim Int32Result As Int32

            LabelId = DirectCast(CurrentRow.FindControl("LabelId"), Label)
            TextBoxCode = DirectCast(CurrentRow.FindControl("TextBoxCode"), TextBox)

            If (ListFor.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE))) Then
                TextBoxDefinition = DirectCast(CurrentRow.FindControl("TextBoxDefinition"), TextBox)
                TextBoxTable = DirectCast(CurrentRow.FindControl("TextBoxTable"), TextBox)
                TextBoxColumn = DirectCast(CurrentRow.FindControl("TextBoxColumn"), TextBox)
            End If
       
            If (Page.IsValid) Then

                objEDICodesDataObject.Id = If(Int32.TryParse(LabelId.Text.Trim(), Int32Result), Int32Result, objEDICodesDataObject.Id)
                objEDICodesDataObject.Code = TextBoxCode.Text.Trim()

                If (ListFor.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE))) Then
                    objEDICodesDataObject.CodeDefinition = TextBoxDefinition.Text.Trim()
                    objEDICodesDataObject.CodeTable = TextBoxTable.Text.Trim()
                    objEDICodesDataObject.CodeColumn = TextBoxColumn.Text.Trim()
                End If

                objEDICodesDataObject.UpdateBy = objShared.UserId

                EDICodesList.Add(objEDICodesDataObject)

            End If

        End Sub

        ''' <summary>
        ''' Gridview row selection and go for edit mode or record deletion
        ''' </summary>
        ''' <param name="CurrentRow"></param>
        ''' <param name="isChecked"></param>
        ''' <remarks></remarks>
        Private Sub ControlsOnSelect(ByRef CurrentRow As GridViewRow, ByRef isChecked As Boolean)

            TextBoxCode = DirectCast(CurrentRow.FindControl("TextBoxCode"), TextBox)

            If (Not TextBoxCode Is Nothing) Then
                TextBoxCode.ReadOnly = Not isChecked
                TextBoxCode.CssClass = If((TextBoxCode.ReadOnly), "TextBoxCode", "TextBoxCodeEdit")
            End If

            TextBoxDefinition = DirectCast(CurrentRow.FindControl("TextBoxDefinition"), TextBox)

            If (Not TextBoxDefinition Is Nothing) Then
                TextBoxDefinition.ReadOnly = Not isChecked
                TextBoxDefinition.CssClass = If((TextBoxDefinition.ReadOnly), "TextBoxDefinition", "TextBoxDefinitionEdit")
            End If

            TextBoxTable = DirectCast(CurrentRow.FindControl("TextBoxTable"), TextBox)

            If (Not TextBoxTable Is Nothing) Then
                TextBoxTable.ReadOnly = Not isChecked
                TextBoxTable.CssClass = If((TextBoxTable.ReadOnly), "TextBoxTable", "TextBoxTableEdit")
            End If

            TextBoxColumn = DirectCast(CurrentRow.FindControl("TextBoxColumn"), TextBox)

            If (Not TextBoxColumn Is Nothing) Then
                TextBoxColumn.ReadOnly = Not isChecked
                TextBoxColumn.CssClass = If((TextBoxColumn.ReadOnly), "TextBoxColumn", "TextBoxColumnEdit")
            End If


        End Sub

        ''' <summary>
        ''' Reading GridView header text from resource file
        ''' </summary>
        ''' <param name="CurrentRow"></param>
        ''' <remarks></remarks>
        Private Sub SetGridViewColumnHeaderText(ByRef CurrentRow As GridViewRow)

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("EDICodes", ControlName & Convert.ToString(".resx"))

            Dim LabelHeaderSerial As Label = DirectCast(CurrentRow.FindControl("LabelHeaderSerial"), Label)
            LabelHeaderSerial.Text = Convert.ToString(ResourceTable("LabelHeaderSerial"), Nothing).Trim()
            LabelHeaderSerial.Text = If(String.IsNullOrEmpty(LabelHeaderSerial.Text), "SI.", LabelHeaderSerial.Text)

            Dim LabelHeaderSelect As Label = DirectCast(CurrentRow.FindControl("LabelHeaderSelect"), Label)
            LabelHeaderSelect.Text = Convert.ToString(ResourceTable("LabelHeaderSelect"), Nothing).Trim()
            LabelHeaderSelect.Text = If(String.IsNullOrEmpty(LabelHeaderSelect.Text), "Select", LabelHeaderSelect.Text)

            ButtonHeaderCode = DirectCast(CurrentRow.FindControl("ButtonHeaderCode"), Button)
            ButtonHeaderCode.Text = Convert.ToString(ResourceTable("ButtonHeaderCode"), Nothing).Trim()
            ButtonHeaderCode.Text = If(String.IsNullOrEmpty(ButtonHeaderCode.Text), "Code", ButtonHeaderCode.Text)

            ButtonHeaderDefinition = DirectCast(CurrentRow.FindControl("ButtonHeaderDefinition"), Button)
            ButtonHeaderDefinition.Text = Convert.ToString(ResourceTable("ButtonHeaderDefinition"), Nothing).Trim()
            ButtonHeaderDefinition.Text = If(String.IsNullOrEmpty(ButtonHeaderDefinition.Text), "Definition", ButtonHeaderDefinition.Text)

            ButtonHeaderTable = DirectCast(CurrentRow.FindControl("ButtonHeaderTable"), Button)
            ButtonHeaderTable.Text = Convert.ToString(ResourceTable("ButtonHeaderTable"), Nothing).Trim()
            ButtonHeaderTable.Text = If(String.IsNullOrEmpty(ButtonHeaderTable.Text), "Table", ButtonHeaderTable.Text)

            ButtonHeaderColumn = DirectCast(CurrentRow.FindControl("ButtonHeaderColumn"), Button)
            ButtonHeaderColumn.Text = Convert.ToString(ResourceTable("ButtonHeaderColumn"), Nothing).Trim()
            ButtonHeaderColumn.Text = If(String.IsNullOrEmpty(ButtonHeaderColumn.Text), "Column", ButtonHeaderColumn.Text)

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
        Private Sub GetSortedEDICodes(ByRef GridResults As List(Of EDICodesDataObject))

            Dim SortingExpression As String = Convert.ToString(Session("SortingExpression"), Nothing)

            If GridResults IsNot Nothing Then

                Dim param = Expression.Parameter(GetType(EDICodesDataObject), SortingExpression)
                Dim SortExpression = Expression.Lambda(Of Func(Of EDICodesDataObject, Object))(Expression.Convert(Expression.Property(param, SortingExpression),
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
