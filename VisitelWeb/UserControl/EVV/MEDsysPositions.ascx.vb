Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports System.Data.SqlClient
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelCommon
Imports VisitelBusiness.VisitelBusiness.DataObject.EVV

Namespace Visitel.UserControl.EVV
    Public Class MEDsysPositionsControl
        Inherits CommonDataControl

        Private ContractNumber As String
        Private objShared As SharedWebControls

        Private CheckBoxSelect As CheckBox, chkAll As CheckBox
        Private ItemChecked As Boolean = False

        Private LabelHeaderSerial As Label, LabelHeaderSelect As Label, LabelHeaderAccountId As Label, LabelHeaderPositionCode As Label, LabelHeaderPositionName As Label, _
            LabelHeaderUpdateDate As Label, LabelHeaderUpdateBy As Label, LabelId As Label, LabelAccountId As Label

        Private TextBoxAccountId As TextBox, TextBoxPositionCode As TextBox, TextBoxPositionName As TextBox, TextBoxUpdateDate As TextBox, TextBoxUpdateBy As TextBox

        Private ControlName As String, ValidationGroup As String, SaveMessage As String, DeleteMessage As String, DeleteConfirmationMessage As String, ListFor As String
        Private ValidationEnable As Boolean
        Private CurrentRow As GridViewRow

        Private MEDsysPositions As List(Of MEDsysPositionsDataObject), MEDsysPositionsErrorList As List(Of MEDsysPositionsDataObject)

        Private SaveFailedCounter As Int16, DeleteFailedCounter As Int16, RecordSaveCount As Int16
        Public IsLog As Boolean


        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "MEDsysPositionsControl"
            objShared = New SharedWebControls()
            objShared.ConnectionOpen()
            GetCaptionFromResource()
            InitializeControl()
        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)

            GetData()

            If Not IsPostBack Then
                BindGridViewMEDsysPositions()
            End If

        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadCss("EVV/" & ControlName)
            LoadJScript()
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing
            MEDsysPositions = Nothing
        End Sub

        Private Sub ButtonSave_Click(sender As Object, e As EventArgs)

            Page.Validate()
            Page.Validate(ValidationGroup)

            MEDsysPositions = New List(Of MEDsysPositionsDataObject)()
            MEDsysPositionsErrorList = New List(Of MEDsysPositionsDataObject)()
            SaveFailedCounter = 0
            RecordSaveCount = 0

            CurrentRow = GridViewMEDsysPositions.FooterRow

            CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)

            LabelAccountId = DirectCast(CurrentRow.FindControl("LabelAccountId"), Label)


            If ((CheckBoxSelect.Checked) And ((String.IsNullOrEmpty(LabelAccountId.Text.Trim())))) Then
                SaveData(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT))
            End If

            SaveData(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE))

            If (SaveFailedCounter > 0) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} Records Failed to Save out of {1}", SaveFailedCounter, MEDsysPositions.Count))
                ViewState("SaveFailedRecord") = objShared.ToDataTable(MEDsysPositionsErrorList)
                ViewState("ListFor") = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)
            Else
                ViewState("SaveFailedRecord") = Nothing

                If (RecordSaveCount > 0) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} record(s) {1}.", RecordSaveCount, SaveMessage))
                    ViewState.Clear()
                    GetData()
                    BindGridViewMEDsysPositions()
                End If

            End If

            ButtonViewError.Visible = If((SaveFailedCounter > 0), True, False)

            MEDsysPositions = Nothing
            MEDsysPositionsErrorList = Nothing

        End Sub


        Private Sub ButtonDelete_Click(sender As Object, e As EventArgs)

            MEDsysPositions = New List(Of MEDsysPositionsDataObject)()
            MEDsysPositionsErrorList = New List(Of MEDsysPositionsDataObject)()

            MakeList(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE))

            If (MEDsysPositions.Count.Equals(0)) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please select items to delete")
                Return
            End If

            DeleteFailedCounter = 0

            TakeAction(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE))

            If (DeleteFailedCounter > 0) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} Records Failed to Delete out of {1}", DeleteFailedCounter, MEDsysPositions.Count))
                ViewState("DeleteFailedRecord") = objShared.ToDataTable(MEDsysPositionsErrorList)
                ViewState("ListFor") = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE)
            Else
                ViewState("DeleteFailedRecord") = Nothing

                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} record(s) {1}.", MEDsysPositions.Count, DeleteMessage))
                ViewState.Clear()
                GetData()
                BindGridViewMEDsysPositions()
            End If

            ButtonViewError.Visible = If((DeleteFailedCounter > 0), True, False)

            MEDsysPositions = Nothing
            MEDsysPositionsErrorList = Nothing
        End Sub

        Private Sub ButtonClear_Click(sender As Object, e As EventArgs)
            CurrentRow = GridViewMEDsysPositions.FooterRow
            ClearControl(CurrentRow)
        End Sub

        Protected Sub OnCheckedChanged(sender As Object, e As EventArgs)
            Dim isUpdateVisible As Boolean = False

            Dim chk As CheckBox = TryCast(sender, CheckBox)

            'ButtonSave.Enabled = If((chk.Checked), True, False)

            If chk.ID = "chkAll" Then
                For Each row As GridViewRow In GridViewMEDsysPositions.Rows
                    If row.RowType = DataControlRowType.DataRow Then
                        row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked = chk.Checked
                    End If
                Next

                CurrentRow = GridViewMEDsysPositions.FooterRow
                CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)
                CheckBoxSelect.Checked = chk.Checked

            End If

            Dim chkAll As CheckBox = TryCast(GridViewMEDsysPositions.HeaderRow.FindControl("chkAll"), CheckBox)
            'chkAll.Checked = True
            For Each row As GridViewRow In GridViewMEDsysPositions.Rows
                If row.RowType = DataControlRowType.DataRow Then

                    Dim isChecked As Boolean = row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked

                    ControlsOnSelect(row, isChecked)

                    'If (isChecked) Then

                    'End If

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

            ItemChecked = DetermineButtonInActivity(GridViewMEDsysPositions, "CheckBoxSelect")

            ButtonSave.Enabled = ItemChecked
            ButtonDelete.Enabled = ButtonSave.Enabled

        End Sub

        ''' <summary>
        ''' Saving Data either insert or update after making list
        ''' </summary>
        ''' <param name="Action"></param>
        ''' <remarks></remarks>
        Private Sub SaveData(Action As String)

            MakeList(Action)

            If (MEDsysPositions.Count.Equals(0)) Then
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

            Dim objMEDsysPositionsDataObject As MEDsysPositionsDataObject

            Select Case Action
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT)

                    TextBoxAccountId = DirectCast(GridViewMEDsysPositions.FooterRow.FindControl("TextBoxAccountId"), TextBox)
                    If (String.IsNullOrEmpty(TextBoxAccountId.Text.Trim())) Then
                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please write account id.")
                        Return
                    End If

                    objMEDsysPositionsDataObject = New MEDsysPositionsDataObject()
                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)

                    AddToList(CurrentRow, MEDsysPositions, objMEDsysPositionsDataObject)
                    RecordSaveCount = RecordSaveCount + 1
                    objMEDsysPositionsDataObject = Nothing

                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE),
                    EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE)
                    MEDsysPositions.Clear()
                    For Each row As GridViewRow In GridViewMEDsysPositions.Rows
                        If (row.RowType.Equals(DataControlRowType.DataRow)) Then
                            Dim isChecked As Boolean = row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked

                            If (isChecked) Then

                                objMEDsysPositionsDataObject = New MEDsysPositionsDataObject()

                                CurrentRow = DirectCast(GridViewMEDsysPositions.Rows(row.RowIndex), GridViewRow)

                                TextBoxAccountId = DirectCast(CurrentRow.FindControl("TextBoxAccountId"), TextBox)
                                If (String.IsNullOrEmpty(TextBoxAccountId.Text.Trim())) Then
                                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please write account id.")
                                    Return
                                End If

                                If (Action.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE))) Then
                                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)
                                End If

                                If (Action.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE))) Then
                                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE)
                                End If

                                AddToList(CurrentRow, MEDsysPositions, objMEDsysPositionsDataObject)
                                RecordSaveCount = RecordSaveCount + 1
                                objMEDsysPositionsDataObject = Nothing

                            End If

                        End If
                    Next

                    Exit Select
            End Select

            objMEDsysPositionsDataObject = Nothing

        End Sub

        ''' <summary>
        ''' Making List in order to save records or delete
        ''' </summary>
        ''' <param name="CurrentRow"></param>
        ''' <param name="MEDsysPositions"></param>
        ''' <param name="objMEDsysPositionsDataObject"></param>
        ''' <remarks></remarks>
        Private Sub AddToList(ByRef CurrentRow As GridViewRow, ByRef MEDsysPositions As List(Of MEDsysPositionsDataObject), ByRef objMEDsysPositionsDataObject As MEDsysPositionsDataObject)

            Dim Int32Result As Int32

            LabelId = DirectCast(CurrentRow.FindControl("LabelId"), Label)

            If (ListFor.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE))) Then
                ControlsFind(CurrentRow)
            End If

            If (Page.IsValid) Then

                objMEDsysPositionsDataObject.Id = If(Int32.TryParse(LabelId.Text.Trim(), Int32Result), Int32Result, objMEDsysPositionsDataObject.Id)

                If (ListFor.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE))) Then
                    objMEDsysPositionsDataObject.AccountId = TextBoxAccountId.Text.Trim()
                    objMEDsysPositionsDataObject.PositionCode = TextBoxPositionCode.Text.Trim()
                    objMEDsysPositionsDataObject.PositionName = TextBoxPositionName.Text.Trim()
                End If

                objMEDsysPositionsDataObject.UpdateBy = objShared.UserId

                MEDsysPositions.Add(objMEDsysPositionsDataObject)

            End If

        End Sub

        ''' <summary>
        ''' Make Database operation either for record saving or deleting
        ''' </summary>
        ''' <param name="Action"></param>
        ''' <remarks></remarks>
        Private Sub TakeAction(Action As String)

            Dim objBLMEDsysPositions As New BLMEDsysPositions()

            For Each MEDsysPosition In MEDsysPositions
                Try
                    Select Case Action
                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT)
                            objBLMEDsysPositions.InsertMEDsysPositionsInfo(objShared.ConVisitel, MEDsysPosition)
                            Exit Select

                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE)
                            objBLMEDsysPositions.UpdateMEDsysPositionsInfo(objShared.ConVisitel, MEDsysPosition)
                            Exit Select

                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE)
                            objBLMEDsysPositions.DeleteMEDsysPositionsInfo(objShared.ConVisitel, MEDsysPosition.Id, MEDsysPosition.UpdateBy)
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

                    MEDsysPosition.Remarks = ex.Message
                    MEDsysPositionsErrorList.Add(MEDsysPosition)

                Finally

                End Try
            Next

            objBLMEDsysPositions = Nothing

        End Sub

        Private Sub ControlsFind(ByRef CurrentRow As GridViewRow)
            TextBoxAccountId = DirectCast(CurrentRow.FindControl("TextBoxAccountId"), TextBox)
            TextBoxPositionCode = DirectCast(CurrentRow.FindControl("TextBoxPositionCode"), TextBox)
            TextBoxPositionName = DirectCast(CurrentRow.FindControl("TextBoxPositionName"), TextBox)
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

            If (Not TextBoxPositionCode Is Nothing) Then
                TextBoxPositionCode.ReadOnly = Not isChecked
                TextBoxPositionCode.CssClass = If((TextBoxPositionCode.ReadOnly), "TextBoxPositionCode", "TextBoxPositionCodeEdit")
            End If

            If (Not TextBoxPositionName Is Nothing) Then
                TextBoxPositionName.ReadOnly = Not isChecked
                TextBoxPositionName.CssClass = If((TextBoxPositionName.ReadOnly), "TextBoxPositionName", "TextBoxPositionNameEdit")
            End If

        End Sub

        Private Sub GridViewMEDsysPositions_RowDataBound(sender As Object, e As GridViewRowEventArgs)
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

                TextBoxPositionCode = DirectCast(CurrentRow.FindControl("TextBoxPositionCode"), TextBox)
                TextBoxPositionCode.ReadOnly = True

                TextBoxPositionName = DirectCast(CurrentRow.FindControl("TextBoxPositionName"), TextBox)
                TextBoxPositionName.ReadOnly = True

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

        Private Sub GridViewMEDsysPositions_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
            GridViewMEDsysPositions.PageIndex = e.NewPageIndex
            BindGridViewMEDsysPositions()
        End Sub

        ''' <summary>
        ''' Gridview Row Hover Color Set
        ''' </summary>
        ''' <param name="writer"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub Render(writer As HtmlTextWriter)
            For Each r As GridViewRow In GridViewMEDsysPositions.Rows
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
                          & " var DeleteDialogHeader ='MEDsys Positions'; " _
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
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("EVV", ControlName & Convert.ToString(".resx"))

            LabelMEDsysPositions.Text = Convert.ToString(ResourceTable("LabelMEDsysPositions"), Nothing)
            LabelMEDsysPositions.Text = If(String.IsNullOrEmpty(LabelMEDsysPositions.Text), "MEDsys Positions", LabelMEDsysPositions.Text)

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
            ValidationGroup = If(String.IsNullOrEmpty(ValidationGroup), "MEDsysPositions", ValidationGroup)

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

            LabelHeaderPositionCode = DirectCast(CurrentRow.FindControl("LabelHeaderPositionCode"), Label)
            LabelHeaderPositionCode.Text = Convert.ToString(ResourceTable("LabelHeaderPositionCode"), Nothing).Trim()
            LabelHeaderPositionCode.Text = If(String.IsNullOrEmpty(LabelHeaderPositionCode.Text), "Position Code", LabelHeaderPositionCode.Text)

            LabelHeaderPositionName = DirectCast(CurrentRow.FindControl("LabelHeaderPositionName"), Label)
            LabelHeaderPositionName.Text = Convert.ToString(ResourceTable("LabelHeaderPositionName"), Nothing).Trim()
            LabelHeaderPositionName.Text = If(String.IsNullOrEmpty(LabelHeaderPositionName.Text), "Position Name", LabelHeaderPositionName.Text)

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
            
            GridViewMEDsysPositions.AutoGenerateColumns = False
            GridViewMEDsysPositions.ShowHeaderWhenEmpty = True
            GridViewMEDsysPositions.AllowPaging = True
            GridViewMEDsysPositions.AllowSorting = True

            GridViewMEDsysPositions.ShowFooter = True

            If (GridViewMEDsysPositions.AllowPaging) Then
                GridViewMEDsysPositions.PageSize = objShared.GridViewDefaultPageSize
            End If

            AddHandler GridViewMEDsysPositions.RowDataBound, AddressOf GridViewMEDsysPositions_RowDataBound
            AddHandler GridViewMEDsysPositions.PageIndexChanging, AddressOf GridViewMEDsysPositions_PageIndexChanging

            ButtonClear.ClientIDMode = UI.ClientIDMode.Static
            ButtonSave.ClientIDMode = UI.ClientIDMode.Static
            ButtonDelete.ClientIDMode = UI.ClientIDMode.Static
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
        ''' Clearing controls value
        ''' </summary>
        Private Sub ClearControl(CurrentRow As GridViewRow)
            ControlsFind(CurrentRow)
            BindGridViewMEDsysPositions()
        End Sub

        Private Sub GetData()
            GetVisitData()
        End Sub

        Private Sub GetVisitData()

            Dim objBLMEDsysPositions As New BLMEDsysPositions()

            Try
                MEDsysPositions = objBLMEDsysPositions.SelectMEDsysPositionsInfo(objShared.ConVisitel)
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to fetch MEDsys Positions Data. Message: {0}", ex.Message))
            Finally
                objBLMEDsysPositions = Nothing
            End Try
        End Sub

        Private Sub BindGridViewMEDsysPositions()
            GridViewMEDsysPositions.DataSource = MEDsysPositions
            GridViewMEDsysPositions.DataBind()

            ItemChecked = DetermineButtonInActivity(GridViewMEDsysPositions, "CheckBoxSelect")

            ButtonSave.Enabled = ItemChecked
            ButtonDelete.Enabled = ButtonSave.Enabled

        End Sub

    End Class
End Namespace