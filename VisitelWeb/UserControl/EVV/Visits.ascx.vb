Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports System.Data.SqlClient
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelCommon
Imports VisitelBusiness.VisitelBusiness.DataObject.EVV

Namespace Visitel.UserControl.EVV
    Public Class VisitsControl
        Inherits CommonDataControl

        Private ContractNumber As String
        Private objShared As SharedWebControls

        Private CheckBoxSelect As CheckBox, chkAll As CheckBox
        Private ItemChecked As Boolean = False

        Private LabelHeaderSerial As Label, LabelHeaderSelect As Label, LabelHeaderMyUniqueId As Label, LabelHeaderVestaVisitId As Label, LabelHeaderClientIdVesta As Label, _
            LabelHeaderEmployeeIdVesta As Label, LabelHeaderAuthIdVesta As Label, LabelHeaderVisitDate As Label, LabelHeaderScheduledTimeIn As Label, _
            LabelHeaderScheduledTimeOut As Label, LabelHeaderVisitUnits As Label, LabelHeaderVisitLocation As Label, LabelHeaderDADSServiceGroup As Label, _
            LabelHeaderHCPCSBillCode As Label, LabelHeaderDADSServiceCode As Label, LabelHeaderMod1 As Label, LabelHeaderMod2 As Label, LabelHeaderMod3 As Label, _
            LabelHeaderMod4 As Label, LabelHeaderError As Label, LabelHeaderContract As Label, LabelHeaderCalendarId As Label, LabelHeaderClient As Label, _
            LabelHeaderEmployee As Label, LabelHeaderUpdateDate As Label, LabelHeaderUpdateBy As Label, LabelClientId As Label, LabelEmployeeId As Label, _
            LabelId As Label

        Private TextBoxMyUniqueId As TextBox, TextBoxVestaVisitId As TextBox, TextBoxClientIdVesta As TextBox, TextBoxEmployeeIdVesta As TextBox, TextBoxAuthIdVesta As TextBox, _
            TextBoxVisitDate As TextBox, TextBoxScheduledTimeIn As TextBox, TextBoxScheduledTimeOut As TextBox, TextBoxVisitUnits As TextBox, TextBoxVisitLocation As TextBox, _
            TextBoxDADSServiceGroup As TextBox, TextBoxHCPCSBillCode As TextBox, TextBoxDADSServiceCode As TextBox, TextBoxMod1 As TextBox, TextBoxMod2 As TextBox, _
            TextBoxMod3 As TextBox, TextBoxMod4 As TextBox, TextBoxError As TextBox, TextBoxContract As TextBox, TextBoxCalendarId As TextBox, _
            TextBoxUpdateDate As TextBox, TextBoxUpdateBy As TextBox, TextBoxClientName As TextBox, TextBoxEmployeeName As TextBox

        Private DropDownListClient As DropDownList, DropDownListEmployee As DropDownList

        Private ControlName As String, ValidationGroup As String, SaveMessage As String, DeleteMessage As String, DeleteConfirmationMessage As String, ListFor As String
        Private ValidationEnable As Boolean
        Private CurrentRow As GridViewRow

        Private Visits As List(Of VisitDataObject), VisitsErrorList As List(Of VisitDataObject)

        Private SaveFailedCounter As Int16, DeleteFailedCounter As Int16, RecordSaveCount As Int16
        Public IsLog As Boolean


        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "VisitsControl"
            objShared = New SharedWebControls()
            objShared.ConnectionOpen()
            GetCaptionFromResource()
            InitializeControl()
        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)

            GetData()

            If Not IsPostBack Then
                BindGridViewVisits()
            End If

        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadCss("EVV/" & ControlName)
            LoadJScript()
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing
            Visits = Nothing
        End Sub

        Private Sub ButtonSave_Click(sender As Object, e As EventArgs)

            Page.Validate()
            Page.Validate(ValidationGroup)

            Visits = New List(Of VisitDataObject)()
            VisitsErrorList = New List(Of VisitDataObject)()
            SaveFailedCounter = 0
            RecordSaveCount = 0

            CurrentRow = GridViewVisits.FooterRow

            CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)

            LabelClientId = DirectCast(CurrentRow.FindControl("LabelClientId"), Label)
            LabelEmployeeId = DirectCast(CurrentRow.FindControl("LabelEmployeeId"), Label)

            If ((CheckBoxSelect.Checked) And ((String.IsNullOrEmpty(LabelClientId.Text.Trim())) Or (String.IsNullOrEmpty(LabelEmployeeId.Text.Trim())))) Then
                SaveData(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT))
            End If

            SaveData(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE))

            If (SaveFailedCounter > 0) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} Records Failed to Save out of {1}", SaveFailedCounter, Visits.Count))
                ViewState("SaveFailedRecord") = objShared.ToDataTable(VisitsErrorList)
                ViewState("ListFor") = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)
            Else
                ViewState("SaveFailedRecord") = Nothing

                If (RecordSaveCount > 0) Then
                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} record(s) {1}.", RecordSaveCount, SaveMessage))
                    ViewState.Clear()
                    GetData()
                    BindGridViewVisits()
                End If

            End If

            ButtonViewError.Visible = If((SaveFailedCounter > 0), True, False)

            Visits = Nothing
            VisitsErrorList = Nothing

        End Sub

        Private Sub ButtonIndividualDetail_Click(sender As Object, e As EventArgs)
            Response.Redirect("~/Pages/ClientInfo.aspx?ClientId=" & HiddenFieldClientId.Value)
        End Sub

        Private Sub ButtonEmployeeDetail_Click(sender As Object, e As EventArgs)
            Response.Redirect("~/Pages/EmployeeInfo.aspx?EmployeeId=" & HiddenFieldEmployeeId.Value)
        End Sub

        Private Sub ButtonDelete_Click(sender As Object, e As EventArgs)
            Visits = New List(Of VisitDataObject)()
            VisitsErrorList = New List(Of VisitDataObject)()

            MakeList(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE))

            If (Visits.Count.Equals(0)) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please select items to delete")
                Return
            End If

            DeleteFailedCounter = 0

            TakeAction(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE))

            If (DeleteFailedCounter > 0) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} Records Failed to Delete out of {1}", DeleteFailedCounter, Visits.Count))
                ViewState("DeleteFailedRecord") = objShared.ToDataTable(VisitsErrorList)
                ViewState("ListFor") = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE)
            Else
                ViewState("DeleteFailedRecord") = Nothing

                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(String.Format("{0} record(s) {1}.", Visits.Count, DeleteMessage))
                ViewState.Clear()
                GetData()
                BindGridViewVisits()
            End If

            ButtonViewError.Visible = If((DeleteFailedCounter > 0), True, False)

            Visits = Nothing
            VisitsErrorList = Nothing
        End Sub

        Private Sub ButtonClear_Click(sender As Object, e As EventArgs)
            CurrentRow = GridViewVisits.FooterRow
            ClearControl(CurrentRow)
        End Sub

        Protected Sub OnCheckedChanged(sender As Object, e As EventArgs)
            Dim isUpdateVisible As Boolean = False

            Dim chk As CheckBox = TryCast(sender, CheckBox)

            'ButtonSave.Enabled = If((chk.Checked), True, False)

            If chk.ID = "chkAll" Then
                For Each row As GridViewRow In GridViewVisits.Rows
                    If row.RowType = DataControlRowType.DataRow Then
                        row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked = chk.Checked
                    End If
                Next

                CurrentRow = GridViewVisits.FooterRow
                CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)
                CheckBoxSelect.Checked = chk.Checked

            End If

            Dim chkAll As CheckBox = TryCast(GridViewVisits.HeaderRow.FindControl("chkAll"), CheckBox)
            'chkAll.Checked = True
            For Each row As GridViewRow In GridViewVisits.Rows
                If row.RowType = DataControlRowType.DataRow Then

                    Dim isChecked As Boolean = row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked

                    ControlsOnSelect(row, isChecked)

                    If (isChecked) Then
                        LabelClientId = DirectCast(row.FindControl("LabelClientId"), Label)
                        If ((Not HiddenFieldClientId Is Nothing) And (Not LabelClientId Is Nothing)) Then
                            HiddenFieldClientId.Value = LabelClientId.Text.Trim()
                        End If

                        LabelEmployeeId = DirectCast(row.FindControl("LabelEmployeeId"), Label)
                        If ((Not HiddenFieldEmployeeId Is Nothing) And (Not LabelEmployeeId Is Nothing)) Then
                            HiddenFieldEmployeeId.Value = LabelEmployeeId.Text.Trim()
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

            ItemChecked = DetermineButtonInActivity(GridViewVisits, "CheckBoxSelect")

            ButtonSave.Enabled = ItemChecked
            ButtonDelete.Enabled = ButtonSave.Enabled
            ButtonIndividualDetail.Enabled = ButtonSave.Enabled

        End Sub

        ''' <summary>
        ''' Saving Data either insert or update after making list
        ''' </summary>
        ''' <param name="Action"></param>
        ''' <remarks></remarks>
        Private Sub SaveData(Action As String)

            MakeList(Action)

            If (Visits.Count.Equals(0)) Then
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

            Dim objVisitDataObject As VisitDataObject

            Select Case Action
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT)

                    LabelEmployeeId = DirectCast(GridViewVisits.FooterRow.FindControl("LabelEmployeeId"), Label)

                    DropDownListClient = DirectCast(GridViewVisits.FooterRow.FindControl("DropDownListClient"), DropDownList)
                    If (DropDownListClient.SelectedIndex < 1) Then
                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please select one client")
                        Return
                    End If

                    DropDownListEmployee = DirectCast(GridViewVisits.FooterRow.FindControl("DropDownListEmployee"), DropDownList)
                    If (DropDownListEmployee.SelectedIndex < 1) Then
                        DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please select one employee")
                        Return
                    End If

                    objVisitDataObject = New VisitDataObject()
                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)

                    AddToList(CurrentRow, Visits, objVisitDataObject)
                    RecordSaveCount = RecordSaveCount + 1
                    objVisitDataObject = Nothing

                    Exit Select
                Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE),
                    EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE)
                    Visits.Clear()
                    For Each row As GridViewRow In GridViewVisits.Rows
                        If (row.RowType.Equals(DataControlRowType.DataRow)) Then
                            Dim isChecked As Boolean = row.Cells(1).Controls.OfType(Of CheckBox)().FirstOrDefault().Checked

                            If (isChecked) Then

                                objVisitDataObject = New VisitDataObject()

                                CurrentRow = DirectCast(GridViewVisits.Rows(row.RowIndex), GridViewRow)


                                LabelEmployeeId = DirectCast(CurrentRow.FindControl("LabelEmployeeId"), Label)

                                DropDownListClient = DirectCast(CurrentRow.FindControl("DropDownListClient"), DropDownList)
                                If (DropDownListClient.SelectedIndex < 1) Then
                                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please select one client")
                                    Return
                                End If

                                DropDownListEmployee = DirectCast(CurrentRow.FindControl("DropDownListEmployee"), DropDownList)
                                If (DropDownListEmployee.SelectedIndex < 1) Then
                                    DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Please select one employee")
                                    Return
                                End If

                                If (Action.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE))) Then
                                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE)
                                End If

                                If (Action.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE))) Then
                                    ListFor = EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.DELETE)
                                End If

                                AddToList(CurrentRow, Visits, objVisitDataObject)
                                RecordSaveCount = RecordSaveCount + 1
                                objVisitDataObject = Nothing

                            End If

                        End If
                    Next

                    Exit Select
            End Select

            objVisitDataObject = Nothing

        End Sub

        ''' <summary>
        ''' Making List in order to save records or delete
        ''' </summary>
        ''' <param name="CurrentRow"></param>
        ''' <param name="Visits"></param>
        ''' <param name="objVisitDataObject"></param>
        ''' <remarks></remarks>
        Private Sub AddToList(ByRef CurrentRow As GridViewRow, ByRef Visits As List(Of VisitDataObject), ByRef objVisitDataObject As VisitDataObject)

            Dim Int32Result As Int32

            LabelId = DirectCast(CurrentRow.FindControl("LabelId"), Label)
            LabelClientId = DirectCast(CurrentRow.FindControl("LabelClientId"), Label)

            If (ListFor.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE))) Then
                ControlsFind(CurrentRow)
            End If

            If (Page.IsValid) Then

                objVisitDataObject.Id = If(Int32.TryParse(LabelId.Text.Trim(), Int32Result), Int32Result, objVisitDataObject.Id)

                If (ListFor.Equals(EnumDataObject.EnumHelper.GetDescription(EnumDataObject.ListFor.SAVE))) Then
                    objVisitDataObject.MyUniqueId = TextBoxMyUniqueId.Text.Trim()
                    objVisitDataObject.VestaVisitId = TextBoxVestaVisitId.Text.Trim()
                    objVisitDataObject.ClientIdVesta = TextBoxClientIdVesta.Text.Trim()
                    objVisitDataObject.EmployeeIdVesta = TextBoxEmployeeIdVesta.Text.Trim()
                    objVisitDataObject.AuthIdVesta = TextBoxAuthIdVesta.Text.Trim()
                    objVisitDataObject.VisitDate = TextBoxVisitDate.Text.Trim()
                    objVisitDataObject.ScheduledTimeIn = TextBoxScheduledTimeIn.Text.Trim()
                    objVisitDataObject.ScheduledTimeOut = TextBoxScheduledTimeOut.Text.Trim()
                    objVisitDataObject.VisitUnits = TextBoxVisitUnits.Text.Trim()
                    objVisitDataObject.VisitLocation = TextBoxVisitLocation.Text.Trim()
                    objVisitDataObject.DADSServiceGroup = TextBoxDADSServiceGroup.Text.Trim()
                    objVisitDataObject.HCPCSBillCode = TextBoxHCPCSBillCode.Text.Trim()
                    objVisitDataObject.DADSServiceCode = TextBoxDADSServiceCode.Text.Trim()
                    objVisitDataObject.Mod1 = TextBoxMod1.Text.Trim()
                    objVisitDataObject.Mod2 = TextBoxMod2.Text.Trim()
                    objVisitDataObject.Mod3 = TextBoxMod3.Text.Trim()
                    objVisitDataObject.Mod4 = TextBoxMod4.Text.Trim()
                    objVisitDataObject.Error = TextBoxError.Text.Trim()
                    objVisitDataObject.Contract = TextBoxContract.Text.Trim()
                    objVisitDataObject.CalendarId = TextBoxCalendarId.Text.Trim()
                    objVisitDataObject.ClientId = Convert.ToInt64(DropDownListClient.SelectedValue)
                    objVisitDataObject.EmployeeId = Convert.ToInt64(DropDownListEmployee.SelectedValue)

                End If

                objVisitDataObject.UpdateBy = objShared.UserId

                Visits.Add(objVisitDataObject)

            End If

        End Sub

        ''' <summary>
        ''' Make Database operation either for record saving or deleting
        ''' </summary>
        ''' <param name="Action"></param>
        ''' <remarks></remarks>
        Private Sub TakeAction(Action As String)

            Dim objBLVestaVisit As New BLVestaVisit()

            For Each Visit In Visits
                Try
                    Select Case Action
                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.INSERT)
                            objBLVestaVisit.InsertVisitInfo(objShared.ConVisitel, Visit, IsLog)
                            Exit Select

                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.UPDATE)
                            objBLVestaVisit.UpdateVisitInfo(objShared.ConVisitel, Visit, IsLog)
                            Exit Select

                        Case EnumDataObject.EnumHelper.GetDescription(EnumDataObject.DBOperation.DELETE)
                            objBLVestaVisit.DeleteVisitInfo(objShared.ConVisitel, Visit.Id, Visit.UpdateBy, IsLog)
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

                    Visit.Remarks = ex.Message
                    VisitsErrorList.Add(Visit)

                Finally

                End Try
            Next

            objBLVestaVisit = Nothing

        End Sub

        Private Sub ControlsFind(ByRef CurrentRow As GridViewRow)

            TextBoxMyUniqueId = DirectCast(CurrentRow.FindControl("TextBoxMyUniqueId"), TextBox)
            TextBoxVestaVisitId = DirectCast(CurrentRow.FindControl("TextBoxVestaVisitId"), TextBox)
            TextBoxClientIdVesta = DirectCast(CurrentRow.FindControl("TextBoxClientIdVesta"), TextBox)
            TextBoxEmployeeIdVesta = DirectCast(CurrentRow.FindControl("TextBoxEmployeeIdVesta"), TextBox)
            TextBoxAuthIdVesta = DirectCast(CurrentRow.FindControl("TextBoxAuthIdVesta"), TextBox)
            TextBoxVisitDate = DirectCast(CurrentRow.FindControl("TextBoxVisitDate"), TextBox)
            TextBoxScheduledTimeIn = DirectCast(CurrentRow.FindControl("TextBoxScheduledTimeIn"), TextBox)
            TextBoxScheduledTimeOut = DirectCast(CurrentRow.FindControl("TextBoxScheduledTimeOut"), TextBox)
            TextBoxVisitUnits = DirectCast(CurrentRow.FindControl("TextBoxVisitUnits"), TextBox)
            TextBoxVisitLocation = DirectCast(CurrentRow.FindControl("TextBoxVisitLocation"), TextBox)
            TextBoxDADSServiceGroup = DirectCast(CurrentRow.FindControl("TextBoxDADSServiceGroup"), TextBox)
            TextBoxHCPCSBillCode = DirectCast(CurrentRow.FindControl("TextBoxHCPCSBillCode"), TextBox)
            TextBoxDADSServiceCode = DirectCast(CurrentRow.FindControl("TextBoxDADSServiceCode"), TextBox)
            TextBoxMod1 = DirectCast(CurrentRow.FindControl("TextBoxMod1"), TextBox)
            TextBoxMod2 = DirectCast(CurrentRow.FindControl("TextBoxMod2"), TextBox)
            TextBoxMod3 = DirectCast(CurrentRow.FindControl("TextBoxMod3"), TextBox)
            TextBoxMod4 = DirectCast(CurrentRow.FindControl("TextBoxMod4"), TextBox)
            TextBoxError = DirectCast(CurrentRow.FindControl("TextBoxError"), TextBox)
            TextBoxContract = DirectCast(CurrentRow.FindControl("TextBoxContract"), TextBox)
            TextBoxCalendarId = DirectCast(CurrentRow.FindControl("TextBoxCalendarId"), TextBox)

            If (Not CurrentRow.RowType.Equals(DataControlRowType.Footer)) Then
                TextBoxClientName = DirectCast(CurrentRow.FindControl("TextBoxClientName"), TextBox)
            End If

            DropDownListClient = DirectCast(CurrentRow.FindControl("DropDownListClient"), DropDownList)

            If (Not CurrentRow.RowType.Equals(DataControlRowType.Footer)) Then
                TextBoxEmployeeName = DirectCast(CurrentRow.FindControl("TextBoxEmployeeName"), TextBox)
            End If

            DropDownListEmployee = DirectCast(CurrentRow.FindControl("DropDownListEmployee"), DropDownList)
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

            If (Not TextBoxVestaVisitId Is Nothing) Then
                TextBoxVestaVisitId.ReadOnly = Not isChecked
                TextBoxVestaVisitId.CssClass = If((TextBoxVestaVisitId.ReadOnly), "TextBoxVestaVisitId", "TextBoxVestaVisitIdEdit")
            End If

            If (Not TextBoxClientIdVesta Is Nothing) Then
                TextBoxClientIdVesta.ReadOnly = Not isChecked
                TextBoxClientIdVesta.CssClass = If((TextBoxClientIdVesta.ReadOnly), "TextBoxClientIdVesta", "TextBoxClientIdVestaEdit")
            End If

            If (Not TextBoxEmployeeIdVesta Is Nothing) Then
                TextBoxEmployeeIdVesta.ReadOnly = Not isChecked
                TextBoxEmployeeIdVesta.CssClass = If((TextBoxEmployeeIdVesta.ReadOnly), "TextBoxEmployeeIdVesta", "TextBoxEmployeeIdVestaEdit")
            End If

            If (Not TextBoxAuthIdVesta Is Nothing) Then
                TextBoxAuthIdVesta.ReadOnly = Not isChecked
                TextBoxAuthIdVesta.CssClass = If((TextBoxAuthIdVesta.ReadOnly), "TextBoxAuthIdVesta", "TextBoxAuthIdVestaEdit")
            End If

            If (Not TextBoxVisitDate Is Nothing) Then
                TextBoxVisitDate.ReadOnly = Not isChecked
                TextBoxVisitDate.CssClass = If((TextBoxVisitDate.ReadOnly), "TextBoxVisitDate", "TextBoxVisitDateEdit")
            End If

            If (Not TextBoxScheduledTimeIn Is Nothing) Then
                TextBoxScheduledTimeIn.ReadOnly = Not isChecked
                TextBoxScheduledTimeIn.CssClass = If((TextBoxScheduledTimeIn.ReadOnly), "TextBoxScheduledTimeIn", "TextBoxScheduledTimeInEdit")
            End If

            If (Not TextBoxScheduledTimeOut Is Nothing) Then
                TextBoxScheduledTimeOut.ReadOnly = Not isChecked
                TextBoxScheduledTimeOut.CssClass = If((TextBoxScheduledTimeOut.ReadOnly), "TextBoxScheduledTimeOut", "TextBoxScheduledTimeOutEdit")
            End If

            If (Not TextBoxVisitUnits Is Nothing) Then
                TextBoxVisitUnits.ReadOnly = Not isChecked
                TextBoxVisitUnits.CssClass = If((TextBoxVisitUnits.ReadOnly), "TextBoxVisitUnits", "TextBoxVisitUnitsEdit")
            End If

            If (Not TextBoxVisitLocation Is Nothing) Then
                TextBoxVisitLocation.ReadOnly = Not isChecked
                TextBoxVisitLocation.CssClass = If((TextBoxVisitLocation.ReadOnly), "TextBoxVisitLocation", "TextBoxVisitLocationEdit")
            End If

            If (Not TextBoxDADSServiceGroup Is Nothing) Then
                TextBoxDADSServiceGroup.ReadOnly = Not isChecked
                TextBoxDADSServiceGroup.CssClass = If((TextBoxDADSServiceGroup.ReadOnly), "TextBoxDADSServiceGroup", "TextBoxDADSServiceGroupEdit")
            End If

            If (Not TextBoxHCPCSBillCode Is Nothing) Then
                TextBoxHCPCSBillCode.ReadOnly = Not isChecked
                TextBoxHCPCSBillCode.CssClass = If((TextBoxHCPCSBillCode.ReadOnly), "TextBoxHCPCSBillCode", "TextBoxHCPCSBillCodeEdit")
            End If

            If (Not TextBoxDADSServiceCode Is Nothing) Then
                TextBoxDADSServiceCode.ReadOnly = Not isChecked
                TextBoxDADSServiceCode.CssClass = If((TextBoxDADSServiceCode.ReadOnly), "TextBoxDADSServiceCode", "TextBoxDADSServiceCodeEdit")
            End If

            If (Not TextBoxMod1 Is Nothing) Then
                TextBoxMod1.ReadOnly = Not isChecked
                TextBoxMod1.CssClass = If((TextBoxMod1.ReadOnly), "TextBoxMod1", "TextBoxMod1Edit")
            End If

            If (Not TextBoxMod2 Is Nothing) Then
                TextBoxMod2.ReadOnly = Not isChecked
                TextBoxMod2.CssClass = If((TextBoxMod2.ReadOnly), "TextBoxMod2", "TextBoxMod2Edit")
            End If

            If (Not TextBoxMod3 Is Nothing) Then
                TextBoxMod3.ReadOnly = Not isChecked
                TextBoxMod3.CssClass = If((TextBoxMod3.ReadOnly), "TextBoxMod3", "TextBoxMod3Edit")
            End If

            If (Not TextBoxMod4 Is Nothing) Then
                TextBoxMod4.ReadOnly = Not isChecked
                TextBoxMod4.CssClass = If((TextBoxMod4.ReadOnly), "TextBoxMod4", "TextBoxMod4Edit")
            End If

            If (Not TextBoxError Is Nothing) Then
                TextBoxError.ReadOnly = Not isChecked
                TextBoxError.CssClass = If((TextBoxError.ReadOnly), "TextBoxError", "TextBoxErrorEdit")
            End If

            If (Not TextBoxContract Is Nothing) Then
                TextBoxContract.ReadOnly = Not isChecked
                TextBoxContract.CssClass = If((TextBoxContract.ReadOnly), "TextBoxContract", "TextBoxContractEdit")
            End If

            If (Not TextBoxCalendarId Is Nothing) Then
                TextBoxCalendarId.ReadOnly = Not isChecked
                TextBoxCalendarId.CssClass = If((TextBoxCalendarId.ReadOnly), "TextBoxCalendarId", "TextBoxCalendarIdEdit")
            End If

            TextBoxClientName = DirectCast(CurrentRow.FindControl("TextBoxClientName"), TextBox)
            If (Not TextBoxClientName Is Nothing) Then
                TextBoxClientName.Visible = Not isChecked
            End If

            TextBoxEmployeeName = DirectCast(CurrentRow.FindControl("TextBoxEmployeeName"), TextBox)
            If (Not TextBoxEmployeeName Is Nothing) Then
                TextBoxEmployeeName.Visible = Not isChecked
            End If

        End Sub

        Private Sub GridViewVisits_RowDataBound(sender As Object, e As GridViewRowEventArgs)
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

                TextBoxVestaVisitId = DirectCast(CurrentRow.FindControl("TextBoxVestaVisitId"), TextBox)
                TextBoxVestaVisitId.ReadOnly = True

                TextBoxClientIdVesta = DirectCast(CurrentRow.FindControl("TextBoxClientIdVesta"), TextBox)
                TextBoxClientIdVesta.ReadOnly = True

                TextBoxEmployeeIdVesta = DirectCast(CurrentRow.FindControl("TextBoxEmployeeIdVesta"), TextBox)
                TextBoxEmployeeIdVesta.ReadOnly = True

                TextBoxAuthIdVesta = DirectCast(CurrentRow.FindControl("TextBoxAuthIdVesta"), TextBox)
                TextBoxAuthIdVesta.ReadOnly = True

                TextBoxVisitDate = DirectCast(CurrentRow.FindControl("TextBoxVisitDate"), TextBox)
                TextBoxVisitDate.ReadOnly = True

                TextBoxScheduledTimeIn = DirectCast(CurrentRow.FindControl("TextBoxScheduledTimeIn"), TextBox)
                TextBoxScheduledTimeIn.ReadOnly = True

                TextBoxScheduledTimeOut = DirectCast(CurrentRow.FindControl("TextBoxScheduledTimeOut"), TextBox)
                TextBoxScheduledTimeOut.ReadOnly = True

                TextBoxVisitUnits = DirectCast(CurrentRow.FindControl("TextBoxVisitUnits"), TextBox)
                TextBoxVisitUnits.ReadOnly = True

                TextBoxVisitLocation = DirectCast(CurrentRow.FindControl("TextBoxVisitLocation"), TextBox)
                TextBoxVisitLocation.ReadOnly = True

                TextBoxDADSServiceGroup = DirectCast(CurrentRow.FindControl("TextBoxDADSServiceGroup"), TextBox)
                TextBoxDADSServiceGroup.ReadOnly = True

                TextBoxHCPCSBillCode = DirectCast(CurrentRow.FindControl("TextBoxHCPCSBillCode"), TextBox)
                TextBoxHCPCSBillCode.ReadOnly = True

                TextBoxDADSServiceCode = DirectCast(CurrentRow.FindControl("TextBoxDADSServiceCode"), TextBox)
                TextBoxDADSServiceCode.ReadOnly = True

                TextBoxMod1 = DirectCast(CurrentRow.FindControl("TextBoxMod1"), TextBox)
                TextBoxMod1.ReadOnly = True

                TextBoxMod2 = DirectCast(CurrentRow.FindControl("TextBoxMod2"), TextBox)
                TextBoxMod2.ReadOnly = True

                TextBoxMod3 = DirectCast(CurrentRow.FindControl("TextBoxMod3"), TextBox)
                TextBoxMod3.ReadOnly = True

                TextBoxMod4 = DirectCast(CurrentRow.FindControl("TextBoxMod4"), TextBox)
                TextBoxMod4.ReadOnly = True

                TextBoxError = DirectCast(CurrentRow.FindControl("TextBoxError"), TextBox)
                TextBoxError.ReadOnly = True

                TextBoxContract = DirectCast(CurrentRow.FindControl("TextBoxContract"), TextBox)
                TextBoxContract.ReadOnly = True

                TextBoxCalendarId = DirectCast(CurrentRow.FindControl("TextBoxCalendarId"), TextBox)
                TextBoxCalendarId.ReadOnly = True

                TextBoxUpdateDate = DirectCast(CurrentRow.FindControl("TextBoxUpdateDate"), TextBox)
                TextBoxUpdateDate.ReadOnly = True

                TextBoxUpdateBy = DirectCast(CurrentRow.FindControl("TextBoxUpdateBy"), TextBox)
                TextBoxUpdateBy.ReadOnly = True

                LabelClientId = DirectCast(CurrentRow.FindControl("LabelClientId"), Label)

                TextBoxClientName = DirectCast(CurrentRow.FindControl("TextBoxClientName"), TextBox)
                If ((Not TextBoxClientName Is Nothing) And (Not LabelClientId Is Nothing)) Then
                    TextBoxClientName.ReadOnly = True
                End If

                LabelEmployeeId = DirectCast(CurrentRow.FindControl("LabelEmployeeId"), Label)

                TextBoxEmployeeName = DirectCast(CurrentRow.FindControl("TextBoxEmployeeName"), TextBox)
                If ((Not TextBoxEmployeeName Is Nothing) And (Not LabelEmployeeId Is Nothing)) Then
                    TextBoxEmployeeName.ReadOnly = True
                End If

                DropDownListClient = DirectCast(CurrentRow.FindControl("DropDownListClient"), DropDownList)

                If (Not DropDownListClient Is Nothing) Then
                    DropDownListClient.Visible = False
                End If

                DropDownListEmployee = DirectCast(CurrentRow.FindControl("DropDownListEmployee"), DropDownList)

                If (Not DropDownListEmployee Is Nothing) Then
                    DropDownListEmployee.Visible = False
                End If


                '**************************Fill Out Drop Down and Associate selection on Edit Mode[Start]***********************************
                If ((e.Row.RowState & DataControlRowState.Edit) > 0) Then
                    DropDownListClient = DirectCast(CurrentRow.FindControl("DropDownListClient"), DropDownList)

                    If (Not DropDownListClient Is Nothing) Then

                        objShared.BindClientDropDownList(DropDownListClient, objShared.CompanyId, EnumDataObject.ClientListFor.Individual)

                        DropDownListClient.SelectedIndex = DropDownListClient.Items.IndexOf(
                            DropDownListClient.Items.FindByValue(Convert.ToString(LabelClientId.Text.Trim(), Nothing)))

                    End If

                    DropDownListEmployee = DirectCast(CurrentRow.FindControl("DropDownListEmployee"), DropDownList)

                    If (Not DropDownListEmployee Is Nothing) Then

                        objShared.BindEmployeeDropDownList(DropDownListEmployee, objShared.CompanyId, False)

                        DropDownListEmployee.SelectedIndex = DropDownListEmployee.Items.IndexOf(
                            DropDownListEmployee.Items.FindByValue(Convert.ToString(LabelEmployeeId.Text.Trim(), Nothing)))

                    End If

                    'objShared.BindEmployeeDropDownList(DropDownListEmployee, objShared.CompanyId, False)
                End If
                '**************************Fill Out Drop Down and Associate selection on Edit Mode[End]***********************************

            End If

            If (CurrentRow.RowType.Equals(DataControlRowType.Footer)) Then

                CheckBoxSelect = DirectCast(CurrentRow.FindControl("CheckBoxSelect"), CheckBox)
                CheckBoxSelect.AutoPostBack = True

                DropDownListClient = DirectCast(CurrentRow.FindControl("DropDownListClient"), DropDownList)

                If (Not DropDownListClient Is Nothing) Then
                    objShared.BindClientDropDownList(DropDownListClient, objShared.CompanyId, EnumDataObject.ClientListFor.Individual)
                End If

                DropDownListEmployee = DirectCast(CurrentRow.FindControl("DropDownListEmployee"), DropDownList)

                If (Not DropDownListEmployee Is Nothing) Then
                    objShared.BindEmployeeDropDownList(DropDownListEmployee, objShared.CompanyId, False)
                End If

                TextBoxUpdateDate = DirectCast(CurrentRow.FindControl("TextBoxUpdateDate"), TextBox)
                TextBoxUpdateDate.ReadOnly = True

                TextBoxUpdateBy = DirectCast(CurrentRow.FindControl("TextBoxUpdateBy"), TextBox)
                TextBoxUpdateBy.ReadOnly = True

            End If

        End Sub

        Private Sub GridViewVisits_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
            GridViewVisits.PageIndex = e.NewPageIndex
            BindGridViewVisits()
        End Sub

        ''' <summary>
        ''' Gridview Row Hover Color Set
        ''' </summary>
        ''' <param name="writer"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub Render(writer As HtmlTextWriter)
            For Each r As GridViewRow In GridViewVisits.Rows
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
                          & " var DeleteDialogHeader ='Vesta Visits'; " _
                          & " var DeleteDialogConfirmMsg ='" & DeleteConfirmationMessage & "'; " _
                          & " var prm =''; " _
                          & " jQuery(document).ready(function () {" _
                          & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                          & "     prm.add_beginRequest(SetButtonActionProgress); " _
                          & "     DataDelete();" _
                          & "     prm.add_endRequest(DataDelete); " _
                          & "     prm.add_endRequest(EndRequest); " _
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

            LabelVisits.Text = Convert.ToString(ResourceTable("LabelVisits"), Nothing)
            LabelVisits.Text = If(String.IsNullOrEmpty(LabelVisits.Text), "Vesta Visits", LabelVisits.Text)

            ButtonSave.Text = Convert.ToString(ResourceTable("ButtonSave"), Nothing)
            ButtonSave.Text = If(String.IsNullOrEmpty(ButtonSave.Text), "Save", ButtonSave.Text)

            ButtonClear.Text = Convert.ToString(ResourceTable("ButtonClear"), Nothing)
            ButtonClear.Text = If(String.IsNullOrEmpty(ButtonClear.Text), "Clear", ButtonClear.Text)

            ButtonDelete.Text = Convert.ToString(ResourceTable("ButtonDelete"), Nothing)
            ButtonDelete.Text = If(String.IsNullOrEmpty(ButtonDelete.Text), "Delete", ButtonDelete.Text)

            ButtonIndividualDetail.Text = Convert.ToString(ResourceTable("ButtonIndividualDetail"), Nothing)
            ButtonIndividualDetail.Text = If(String.IsNullOrEmpty(ButtonIndividualDetail.Text), "Individual Detail", ButtonIndividualDetail.Text)

            ButtonEmployeeDetail.Text = Convert.ToString(ResourceTable("ButtonEmployeeDetail"), Nothing)
            ButtonEmployeeDetail.Text = If(String.IsNullOrEmpty(ButtonEmployeeDetail.Text), "Employee Detail", ButtonEmployeeDetail.Text)

            ButtonViewError.Text = Convert.ToString(ResourceTable("ButtonViewError"), Nothing)
            ButtonViewError.Text = If(String.IsNullOrEmpty(ButtonViewError.Text), "View Detail Error", ButtonViewError.Text)

            SaveMessage = Convert.ToString(ResourceTable("SaveMessage"), Nothing)
            SaveMessage = If(String.IsNullOrEmpty(SaveMessage), "Saved Successfully", SaveMessage)

            DeleteMessage = Convert.ToString(ResourceTable("DeleteMessage"), Nothing)
            DeleteMessage = If(String.IsNullOrEmpty(DeleteMessage), "Deleted Successfully", DeleteMessage)

            ValidationGroup = Convert.ToString(ResourceTable("ValidationGroup"), Nothing)
            ValidationGroup = If(String.IsNullOrEmpty(ValidationGroup), "VestaVisits", ValidationGroup)

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

            LabelHeaderClient = DirectCast(CurrentRow.FindControl("LabelHeaderClient"), Label)
            LabelHeaderClient.Text = Convert.ToString(ResourceTable("LabelHeaderClient"), Nothing).Trim()
            LabelHeaderClient.Text = If(String.IsNullOrEmpty(LabelHeaderClient.Text), "Client Id", LabelHeaderClient.Text)

            LabelHeaderEmployee = DirectCast(CurrentRow.FindControl("LabelHeaderEmployee"), Label)
            LabelHeaderEmployee.Text = Convert.ToString(ResourceTable("LabelHeaderEmployee"), Nothing).Trim()
            LabelHeaderEmployee.Text = If(String.IsNullOrEmpty(LabelHeaderEmployee.Text), "Emp Id", LabelHeaderEmployee.Text)

            LabelHeaderMyUniqueId = DirectCast(CurrentRow.FindControl("LabelHeaderMyUniqueId"), Label)
            LabelHeaderMyUniqueId.Text = Convert.ToString(ResourceTable("LabelHeaderMyUniqueId"), Nothing).Trim()
            LabelHeaderMyUniqueId.Text = If(String.IsNullOrEmpty(LabelHeaderMyUniqueId.Text), "My Unique Id", LabelHeaderMyUniqueId.Text)

            LabelHeaderVestaVisitId = DirectCast(CurrentRow.FindControl("LabelHeaderVestaVisitId"), Label)
            LabelHeaderVestaVisitId.Text = Convert.ToString(ResourceTable("LabelHeaderVestaVisitId"), Nothing).Trim()
            LabelHeaderVestaVisitId.Text = If(String.IsNullOrEmpty(LabelHeaderVestaVisitId.Text), "Vesta Visit Id", LabelHeaderVestaVisitId.Text)

            LabelHeaderClientIdVesta = DirectCast(CurrentRow.FindControl("LabelHeaderClientIdVesta"), Label)
            LabelHeaderClientIdVesta.Text = Convert.ToString(ResourceTable("LabelHeaderClientIdVesta"), Nothing).Trim()
            LabelHeaderClientIdVesta.Text = If(String.IsNullOrEmpty(LabelHeaderClientIdVesta.Text), "Client Id Vesta", LabelHeaderClientIdVesta.Text)

            LabelHeaderEmployeeIdVesta = DirectCast(CurrentRow.FindControl("LabelHeaderEmployeeIdVesta"), Label)
            LabelHeaderEmployeeIdVesta.Text = Convert.ToString(ResourceTable("LabelHeaderEmployeeIdVesta"), Nothing).Trim()
            LabelHeaderEmployeeIdVesta.Text = If(String.IsNullOrEmpty(LabelHeaderEmployeeIdVesta.Text), "Emp Id Vesta", LabelHeaderEmployeeIdVesta.Text)

            LabelHeaderAuthIdVesta = DirectCast(CurrentRow.FindControl("LabelHeaderAuthIdVesta"), Label)
            LabelHeaderAuthIdVesta.Text = Convert.ToString(ResourceTable("LabelHeaderAuthIdVesta"), Nothing).Trim()
            LabelHeaderAuthIdVesta.Text = If(String.IsNullOrEmpty(LabelHeaderAuthIdVesta.Text), "Auth Id Vesta", LabelHeaderAuthIdVesta.Text)

            LabelHeaderVisitDate = DirectCast(CurrentRow.FindControl("LabelHeaderVisitDate"), Label)
            LabelHeaderVisitDate.Text = Convert.ToString(ResourceTable("LabelHeaderVisitDate"), Nothing).Trim()
            LabelHeaderVisitDate.Text = If(String.IsNullOrEmpty(LabelHeaderVisitDate.Text), "Visit Date", LabelHeaderVisitDate.Text)

            LabelHeaderScheduledTimeIn = DirectCast(CurrentRow.FindControl("LabelHeaderScheduledTimeIn"), Label)
            LabelHeaderScheduledTimeIn.Text = Convert.ToString(ResourceTable("LabelHeaderScheduledTimeIn"), Nothing).Trim()
            LabelHeaderScheduledTimeIn.Text = If(String.IsNullOrEmpty(LabelHeaderScheduledTimeIn.Text), "Sched TimeIn", LabelHeaderScheduledTimeIn.Text)

            LabelHeaderScheduledTimeOut = DirectCast(CurrentRow.FindControl("LabelHeaderScheduledTimeOut"), Label)
            LabelHeaderScheduledTimeOut.Text = Convert.ToString(ResourceTable("LabelHeaderScheduledTimeOut"), Nothing).Trim()
            LabelHeaderScheduledTimeOut.Text = If(String.IsNullOrEmpty(LabelHeaderScheduledTimeOut.Text), "Sched TimeOut", LabelHeaderScheduledTimeOut.Text)

            LabelHeaderVisitUnits = DirectCast(CurrentRow.FindControl("LabelHeaderVisitUnits"), Label)
            LabelHeaderVisitUnits.Text = Convert.ToString(ResourceTable("LabelHeaderVisitUnits"), Nothing).Trim()
            LabelHeaderVisitUnits.Text = If(String.IsNullOrEmpty(LabelHeaderVisitUnits.Text), "Visit Units", LabelHeaderVisitUnits.Text)

            LabelHeaderVisitLocation = DirectCast(CurrentRow.FindControl("LabelHeaderVisitLocation"), Label)
            LabelHeaderVisitLocation.Text = Convert.ToString(ResourceTable("LabelHeaderVisitLocation"), Nothing).Trim()
            LabelHeaderVisitLocation.Text = If(String.IsNullOrEmpty(LabelHeaderVisitLocation.Text), "Visit Location", LabelHeaderVisitLocation.Text)

            LabelHeaderDADSServiceGroup = DirectCast(CurrentRow.FindControl("LabelHeaderDADSServiceGroup"), Label)
            LabelHeaderDADSServiceGroup.Text = Convert.ToString(ResourceTable("LabelHeaderDADSServiceGroup"), Nothing).Trim()
            LabelHeaderDADSServiceGroup.Text = If(String.IsNullOrEmpty(LabelHeaderDADSServiceGroup.Text), "Service Group", LabelHeaderDADSServiceGroup.Text)

            LabelHeaderHCPCSBillCode = DirectCast(CurrentRow.FindControl("LabelHeaderHCPCSBillCode"), Label)
            LabelHeaderHCPCSBillCode.Text = Convert.ToString(ResourceTable("LabelHeaderHCPCSBillCode"), Nothing).Trim()
            LabelHeaderHCPCSBillCode.Text = If(String.IsNullOrEmpty(LabelHeaderHCPCSBillCode.Text), "HCPCS BillCode", LabelHeaderHCPCSBillCode.Text)

            LabelHeaderDADSServiceCode = DirectCast(CurrentRow.FindControl("LabelHeaderDADSServiceCode"), Label)
            LabelHeaderDADSServiceCode.Text = Convert.ToString(ResourceTable("LabelHeaderDADSServiceCode"), Nothing).Trim()
            LabelHeaderDADSServiceCode.Text = If(String.IsNullOrEmpty(LabelHeaderDADSServiceCode.Text), "Service Code", LabelHeaderDADSServiceCode.Text)

            LabelHeaderMod1 = DirectCast(CurrentRow.FindControl("LabelHeaderMod1"), Label)
            LabelHeaderMod1.Text = Convert.ToString(ResourceTable("LabelHeaderMod1"), Nothing).Trim()
            LabelHeaderMod1.Text = If(String.IsNullOrEmpty(LabelHeaderMod1.Text), "Mod1", LabelHeaderMod1.Text)

            LabelHeaderMod2 = DirectCast(CurrentRow.FindControl("LabelHeaderMod2"), Label)
            LabelHeaderMod2.Text = Convert.ToString(ResourceTable("LabelHeaderMod2"), Nothing).Trim()
            LabelHeaderMod2.Text = If(String.IsNullOrEmpty(LabelHeaderMod2.Text), "Mod2", LabelHeaderMod2.Text)

            LabelHeaderMod3 = DirectCast(CurrentRow.FindControl("LabelHeaderMod3"), Label)
            LabelHeaderMod3.Text = Convert.ToString(ResourceTable("LabelHeaderMod3"), Nothing).Trim()
            LabelHeaderMod3.Text = If(String.IsNullOrEmpty(LabelHeaderMod3.Text), "Mod3", LabelHeaderMod3.Text)

            LabelHeaderMod4 = DirectCast(CurrentRow.FindControl("LabelHeaderMod4"), Label)
            LabelHeaderMod4.Text = Convert.ToString(ResourceTable("LabelHeaderMod4"), Nothing).Trim()
            LabelHeaderMod4.Text = If(String.IsNullOrEmpty(LabelHeaderMod4.Text), "Mod4", LabelHeaderMod4.Text)

            LabelHeaderError = DirectCast(CurrentRow.FindControl("LabelHeaderError"), Label)
            LabelHeaderError.Text = Convert.ToString(ResourceTable("LabelHeaderError"), Nothing).Trim()
            LabelHeaderError.Text = If(String.IsNullOrEmpty(LabelHeaderError.Text), "Error", LabelHeaderError.Text)

            LabelHeaderContract = DirectCast(CurrentRow.FindControl("LabelHeaderContract"), Label)
            LabelHeaderContract.Text = Convert.ToString(ResourceTable("LabelHeaderContract"), Nothing).Trim()
            LabelHeaderContract.Text = If(String.IsNullOrEmpty(LabelHeaderContract.Text), "Contract", LabelHeaderContract.Text)

            LabelHeaderCalendarId = DirectCast(CurrentRow.FindControl("LabelHeaderCalendarId"), Label)
            LabelHeaderCalendarId.Text = Convert.ToString(ResourceTable("LabelHeaderCalendarId"), Nothing).Trim()
            LabelHeaderCalendarId.Text = If(String.IsNullOrEmpty(LabelHeaderCalendarId.Text), "Calendar Id", LabelHeaderCalendarId.Text)

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
            AddHandler ButtonIndividualDetail.Click, AddressOf ButtonIndividualDetail_Click
            AddHandler ButtonEmployeeDetail.Click, AddressOf ButtonEmployeeDetail_Click

            GridViewVisits.AutoGenerateColumns = False
            GridViewVisits.ShowHeaderWhenEmpty = True
            GridViewVisits.AllowPaging = True
            GridViewVisits.AllowSorting = True

            GridViewVisits.ShowFooter = True

            If (GridViewVisits.AllowPaging) Then
                GridViewVisits.PageSize = objShared.GridViewDefaultPageSize
            End If

            AddHandler GridViewVisits.RowDataBound, AddressOf GridViewVisits_RowDataBound
            AddHandler GridViewVisits.PageIndexChanging, AddressOf GridViewVisits_PageIndexChanging

            ButtonClear.ClientIDMode = UI.ClientIDMode.Static
            ButtonSave.ClientIDMode = UI.ClientIDMode.Static
            ButtonDelete.ClientIDMode = UI.ClientIDMode.Static
            ButtonIndividualDetail.ClientIDMode = UI.ClientIDMode.Static
            ButtonEmployeeDetail.ClientIDMode = UI.ClientIDMode.Static

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
            BindGridViewVisits()
        End Sub

        Private Sub GetData()
            GetVisitData()
        End Sub

        Private Sub GetVisitData()

            Dim objBLVestaVisit As New BLVestaVisit()

            Try
                Visits = objBLVestaVisit.SelectVisitInfo(objShared.ConVisitel, IsLog)
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError(String.Format("Unable to fetch Visit Data. Message: {0}", ex.Message))
            Finally
                objBLVestaVisit = Nothing
            End Try
        End Sub

        Private Sub BindGridViewVisits()
            GridViewVisits.DataSource = Visits
            GridViewVisits.DataBind()

            ItemChecked = DetermineButtonInActivity(GridViewVisits, "CheckBoxSelect")

            ButtonSave.Enabled = ItemChecked
            ButtonDelete.Enabled = ButtonSave.Enabled
            ButtonIndividualDetail.Enabled = ButtonSave.Enabled
        End Sub

    End Class
End Namespace