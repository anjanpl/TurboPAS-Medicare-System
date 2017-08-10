Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Collections
Imports System.Data.SqlClient
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports System.Web.UI.WebControls
Imports VisitelBusiness.VisitelBusiness.DataObject.Settings
Imports System.Collections.Generic
Imports VisitelBusiness.VisitelBusiness.Settings
Imports VisitelCommon.VisitelCommon.DataObject

Namespace VisitelCommon

    Public Class CalendarSchedule
        'Inherits SharedWebControls
        Inherits BaseUserControl
        Implements IScheduleClientEmployee
        Implements IScheduleClient
        Implements IScheduleEmployee

        Protected objShared As SharedWebControls
        Public ScheduleValidationGroup As String, HourMinuteFormat As String, TimeFormat As String

        Private ValidationEnable As Boolean

        Protected RowCount As Integer = 0

        Public ScheduleList As New List(Of CalendarSettingDataObject)()
        Public AllScheduleList As New List(Of CalendarSettingDataObject)()

        Private DropDownListSearchByClient As DropDownList, DropDownListSearchByScheduleStatus As DropDownList

        Private ButtonActiveOnly As Button, ButtonInActiveOnly As Button, ButtonAll As Button, ButtonSaveSchedule As Button, ButtonEmployeeDetail As Button,
            ButtonClearSchedule As Button, ButtonIndividualDetail As Button

        Private LabelWeeklyCalendar As Label, LabelClientStatus As Label, LabelHoursMinutesCaption As Label, LabelIndividualCaption As Label, LabelEmployeeCaption As Label,
            LabelSunday As Label, LabelMonday As Label, LabelTuesday As Label, LabelWednesday As Label, LabelThursday As Label, LabelFriday As Label, LabelSaturday As Label,
            LabelTotal As Label, LabelStatus As Label, LabelScheduleUpdateDate As Label, LabelScheduleUpdateBy As Label, LabelDetailInfo As Label

        Private IsClientVisible As Boolean = False, IsEmployeeVisible As Boolean = False

#Region "Controls"

        Private UpdatePanelManage As UpdatePanel

        Protected DropDownListClient As DropDownList, DropDownListEmployee As DropDownList, DropDownListScheduleStatus As DropDownList

        Private TextBoxSundayHourMinute As TextBox, TextBoxMondayHourMinute As TextBox, TextBoxTuesdayHourMinute As TextBox, TextBoxWednesdayHourMinute As TextBox,
            TextBoxThursdayHourMinute As TextBox, TextBoxFridayHourMinute As TextBox, TextBoxSaturdayHourMinute As TextBox, TextBoxTotalHourMinute As TextBox,
            TextBoxScheduleUpdateDate As TextBox, TextBoxScheduleUpdateBy As TextBox

        Private LabelDayCaptionFirstColumn As Label, LabelInCaptionFirstColumn As Label, LabelOutCaptionFirstColumn As Label
        Private LabelDayCaptionSecondColumn As Label, LabelInCaptionSecondColumn As Label, LabelOutCaptionSecondColumn As Label

        Private LabelSundayInOutCaption As Label, LabelMondayInOutCaption As Label, LabelTuesdayInOutCaption As Label, LabelWednesdayInOutCaption As Label,
            LabelThursdayInOutCaption As Label, LabelFridayInOutCaption As Label, LabelSaturdayInOutCaption As Label

        Private LabelSpecialPayrate As Label, LabelStartDate As Label, LabelEndDate As Label

        Private TextBoxSundayInTime As TextBox, TextBoxSundayOutTime As TextBox, TextBoxMondayInTime As TextBox, TextBoxMondayOutTime As TextBox, _
            TextBoxTuesdayInTime As TextBox, TextBoxTuesdayOutTime As TextBox, TextBoxWednesdayInTime As TextBox, TextBoxWednesdayOutTime As TextBox, _
            TextBoxThursdayInTime As TextBox, TextBoxThursdayOutTime As TextBox, TextBoxFridayInTime As TextBox, TextBoxFridayOutTime As TextBox, _
            TextBoxSaturdayInTime As TextBox, TextBoxSaturdayOutTime As TextBox

        Private TextBoxSpecialPayrate As TextBox, TextBoxStartDate As TextBox, TextBoxEndDate As TextBox, TextBoxScheduleComments As TextBox

        Private RequiredFieldValidatorSundayInTime As RequiredFieldValidator, RequiredFieldValidatorSundayOutTime As RequiredFieldValidator,
                RequiredFieldValidatorMondayInTime As RequiredFieldValidator, RequiredFieldValidatorMondayOutTime As RequiredFieldValidator,
                RequiredFieldValidatorTuesdayInTime As RequiredFieldValidator, RequiredFieldValidatorTuesdayOutTime As RequiredFieldValidator,
                RequiredFieldValidatorWednesdayInTime As RequiredFieldValidator, RequiredFieldValidatorWednesdayOutTime As RequiredFieldValidator,
                RequiredFieldValidatorThursdayInTime As RequiredFieldValidator, RequiredFieldValidatorThursdayOutTime As RequiredFieldValidator,
                RequiredFieldValidatorFridayInTime As RequiredFieldValidator, RequiredFieldValidatorFridayOutTime As RequiredFieldValidator,
                RequiredFieldValidatorSaturdayInTime As RequiredFieldValidator, RequiredFieldValidatorSaturdayOutTime As RequiredFieldValidator

        Private RegularExpressionValidatorSpecialPayrate As RegularExpressionValidator, RegularExpressionValidatorStartDate As RegularExpressionValidator,
            RegularExpressionValidatorEndDate As RegularExpressionValidator

#End Region

        Private newRow As HtmlGenericControl, SecondColumn As HtmlGenericControl, CalendarServiceBox As HtmlGenericControl, ServiceDaysInOut As HtmlGenericControl

        Private PlaceHolderSchedule As PlaceHolder
        Private hfScheduleId As HiddenField

        ''' <summary>
        ''' Binding Search By Schedule Status Drop Down List
        ''' </summary>
        Protected Sub BindSearchByScheduleStatusDropDownList(ByRef DropDownListSearchByScheduleStatus As DropDownList)

            DropDownListSearchByScheduleStatus.DataSource = EnumDataObject.Enumeration.GetAll(Of EnumDataObject.CalendarScheduleStatus)()
            DropDownListSearchByScheduleStatus.DataTextField = "Value"
            DropDownListSearchByScheduleStatus.DataValueField = "Key"
            DropDownListSearchByScheduleStatus.DataBind()

            DropDownListSearchByScheduleStatus.Items.Insert(0, New ListItem("Please Select...", "-1"))

        End Sub

        ''' <summary>
        ''' Binding Schedule Status Drop Down List
        ''' </summary>
        Private Sub BindScheduleStatusDropDownList(ByRef DropDownListScheduleStatus As DropDownList)

            DropDownListScheduleStatus.DataSource = EnumDataObject.Enumeration.GetAll(Of EnumDataObject.CalendarScheduleStatus)()
            DropDownListScheduleStatus.DataTextField = "Value"
            DropDownListScheduleStatus.DataValueField = "Key"
            DropDownListScheduleStatus.DataBind()

            'DropDownListScheduleStatus.Items.Insert(0, New ListItem("Please Select...", "-1"))

            DropDownListScheduleStatus.SelectedIndex = 1
        End Sub

        Private Sub EmployeeSetControl(ByRef objScheduleControlDataObject As ScheduleControlDataObject) Implements IScheduleEmployee.SetControl
            SetControl(objScheduleControlDataObject)
        End Sub

        Private Sub ClientSetControl(ByRef objScheduleControlDataObject As ScheduleControlDataObject) Implements IScheduleClient.SetControl
            SetControl(objScheduleControlDataObject)
        End Sub

        Private Sub ClientEmployeeSetControl(ByRef objScheduleControlDataObject As ScheduleControlDataObject) Implements IScheduleClientEmployee.SetControl
            SetControl(objScheduleControlDataObject)
        End Sub

        Private Sub SetControl(ByRef objScheduleControlDataObject As ScheduleControlDataObject)

            'UserId = objScheduleControlDataObject.UserId
            'CompanyId = objScheduleControlDataObject.CompanyId

            hfScheduleId = objScheduleControlDataObject.HiddenFieldScheduleId
            PlaceHolderSchedule = objScheduleControlDataObject.PlaceHolderCalendarSetting

            DropDownListSearchByClient = objScheduleControlDataObject.DropDownListSearchByClient
            DropDownListSearchByScheduleStatus = objScheduleControlDataObject.DropDownListSearchByScheduleStatus

            IsClientVisible = objScheduleControlDataObject.IsClientVisible
            IsEmployeeVisible = objScheduleControlDataObject.IsEmployeeVisible

            LabelWeeklyCalendar = objScheduleControlDataObject.LabelWeeklyCalendar

            ButtonActiveOnly = objScheduleControlDataObject.ButtonActiveOnly
            ButtonInActiveOnly = objScheduleControlDataObject.ButtonInActiveOnly
            ButtonAll = objScheduleControlDataObject.ButtonAll
            LabelClientStatus = objScheduleControlDataObject.LabelClientStatus
            LabelHoursMinutesCaption = objScheduleControlDataObject.LabelHoursMinutesCaption
            LabelIndividualCaption = objScheduleControlDataObject.LabelIndividualCaption
            LabelEmployeeCaption = objScheduleControlDataObject.LabelEmployeeCaption

            LabelSunday = objScheduleControlDataObject.LabelSunday
            LabelMonday = objScheduleControlDataObject.LabelMonday
            LabelTuesday = objScheduleControlDataObject.LabelTuesday
            LabelWednesday = objScheduleControlDataObject.LabelWednesday
            LabelThursday = objScheduleControlDataObject.LabelThursday
            LabelFriday = objScheduleControlDataObject.LabelFriday
            LabelSaturday = objScheduleControlDataObject.LabelSaturday

            LabelTotal = objScheduleControlDataObject.LabelTotal
            LabelStatus = objScheduleControlDataObject.LabelStatus
            LabelScheduleUpdateDate = objScheduleControlDataObject.LabelUpdateDate
            LabelScheduleUpdateBy = objScheduleControlDataObject.LabelUpdateBy
            LabelDetailInfo = objScheduleControlDataObject.LabelDetailInfo

            ButtonSaveSchedule = objScheduleControlDataObject.ButtonSaveSchedule
            ButtonIndividualDetail = objScheduleControlDataObject.ButtonIndividualDetail
            ButtonEmployeeDetail = objScheduleControlDataObject.ButtonEmployeeDetail
            ButtonClearSchedule = objScheduleControlDataObject.ButtonClearSchedule

        End Sub

        ''' <summary>
        ''' This event is being used from Calendar Setting Window
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub DropDownListSearchByClient_OnSelectedIndexChanged(sender As Object, e As EventArgs)

            DirectCast(Me, IScheduleClient).GetAll(Convert.ToInt32(DropDownListSearchByClient.SelectedValue))

        End Sub

        Protected Sub DropDownListSearchByScheduleStatus_OnSelectedIndexChanged(sender As Object, e As EventArgs)

            If (DropDownListSearchByScheduleStatus.SelectedItem.Text.Equals("A")) Then
                DirectCast(Me, IScheduleClient).GetActiveOnly()
            ElseIf (DropDownListSearchByScheduleStatus.SelectedItem.Text.Equals("I")) Then
                DirectCast(Me, IScheduleClient).GetInActiveOnly()
            End If

        End Sub

        Private Sub LoadEmployeeScheduleJavaScript() Implements IScheduleEmployee.LoadScheduleJavaScript
            ScheduleJavaScript()
        End Sub

        Private Sub LoadClientScheduleJavaScript() Implements IScheduleClient.LoadScheduleJavaScript
            ScheduleJavaScript()
        End Sub

        Private Sub LoadClientEmployeeScheduleJavaScript() Implements IScheduleClientEmployee.LoadScheduleJavaScript
            ScheduleJavaScript()
        End Sub

        Private Sub ScheduleJavaScript()

            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                        & " var RowCounter = " & RowCount & "; " _
                        & " var CalendarImagePath='" & objShared.GetCalendarImagePath & "'; " _
                        & " var CalendarDateFormat='" & objShared.CalendarDateFormat & "'; " _
                        & " var prm =''; " _
                        & " jQuery(document).ready(function () {" _
                        & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
                        & "     ScheduleDateFieldsEvent();" _
                        & "     CalendarInputMasking();" _
                        & "     prm.add_endRequest(ScheduleDateFieldsEvent); " _
                        & "     prm.add_endRequest(CalendarInputMasking); " _
                        & "}); " _
                 & "</script>"

            'Page.ClientScript.RegisterStartupScript(Me.[GetType](), "CalendarSetting" & "LocalVariable", scriptBlock)

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

            LoadJS("JavaScript/Settings/" & "CalendarSetting" & ".js")

        End Sub

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetEmployeeScheduleCaptionFromResource() Implements IScheduleEmployee.GetScheduleCaptionFromResource
            ScheduleCaptionFromResource()
        End Sub

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetClientScheduleCaptionFromResource() Implements IScheduleClient.GetScheduleCaptionFromResource
            ScheduleCaptionFromResource()
        End Sub

        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub GetClientEmployeeScheduleCaptionFromResource() Implements IScheduleClientEmployee.GetScheduleCaptionFromResource
            ScheduleCaptionFromResource()
        End Sub


        ''' <summary>
        ''' This is for reading page controls text from resource file
        ''' </summary>
        Private Sub ScheduleCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("Setup", "CalendarSetting" & ".resx")

            LabelWeeklyCalendar.Text = Convert.ToString(ResourceTable("LabelWeeklyCalendar"), Nothing).Trim()
            LabelWeeklyCalendar.Text = If(String.IsNullOrEmpty(LabelWeeklyCalendar.Text), "Calendar Info", LabelWeeklyCalendar.Text)

            If (Not ButtonActiveOnly Is Nothing) Then
                ButtonActiveOnly.Text = Convert.ToString(ResourceTable("ButtonActiveOnly"), Nothing).Trim()
                ButtonActiveOnly.Text = If(String.IsNullOrEmpty(ButtonActiveOnly.Text), "Active Only", ButtonActiveOnly.Text)
            End If

            If (Not ButtonInActiveOnly Is Nothing) Then
                ButtonInActiveOnly.Text = Convert.ToString(ResourceTable("ButtonInActiveOnly"), Nothing).Trim()
                ButtonInActiveOnly.Text = If(String.IsNullOrEmpty(ButtonInActiveOnly.Text), "InActive Only", ButtonInActiveOnly.Text)
            End If

            ButtonAll.Text = Convert.ToString(ResourceTable("ButtonAll"), Nothing).Trim()
            ButtonAll.Text = If(String.IsNullOrEmpty(ButtonAll.Text), "All", ButtonAll.Text)

            If (Not LabelClientStatus Is Nothing) Then
                LabelClientStatus.Text = Convert.ToString(ResourceTable("LabelClientStatus"), Nothing).Trim()
                LabelClientStatus.Text = If(String.IsNullOrEmpty(LabelClientStatus.Text), "Status", LabelClientStatus.Text)
            End If

            LabelHoursMinutesCaption.Text = Convert.ToString(ResourceTable("LabelHoursMinutesCaption"), Nothing).Trim()
            LabelHoursMinutesCaption.Text = If(String.IsNullOrEmpty(LabelHoursMinutesCaption.Text), "Hrs|Mins", LabelHoursMinutesCaption.Text)

            If (Not LabelIndividualCaption Is Nothing) Then
                LabelIndividualCaption.Text = Convert.ToString(ResourceTable("LabelIndividualCaption"), Nothing).Trim()
                LabelIndividualCaption.Text = If(String.IsNullOrEmpty(LabelIndividualCaption.Text), "Individual", LabelIndividualCaption.Text)
            End If

            If (Not LabelEmployeeCaption Is Nothing) Then
                LabelEmployeeCaption.Text = Convert.ToString(ResourceTable("LabelEmployeeCaption"), Nothing).Trim()
                LabelEmployeeCaption.Text = If(String.IsNullOrEmpty(LabelEmployeeCaption.Text), "Attendant/Office Staff", LabelEmployeeCaption.Text)
            End If

            LabelSunday.Text = Convert.ToString(ResourceTable("LabelSunday"), Nothing).Trim()
            LabelSunday.Text = If(String.IsNullOrEmpty(LabelSunday.Text), "Sun", LabelSunday.Text)

            LabelMonday.Text = Convert.ToString(ResourceTable("LabelMonday"), Nothing).Trim()
            LabelMonday.Text = If(String.IsNullOrEmpty(LabelMonday.Text), "Mon", LabelMonday.Text)

            LabelTuesday.Text = Convert.ToString(ResourceTable("LabelTuesday"), Nothing).Trim()
            LabelTuesday.Text = If(String.IsNullOrEmpty(LabelTuesday.Text), "Tue", LabelTuesday.Text)

            LabelWednesday.Text = Convert.ToString(ResourceTable("LabelWednesday"), Nothing).Trim()
            LabelWednesday.Text = If(String.IsNullOrEmpty(LabelWednesday.Text), "Wed", LabelWednesday.Text)

            LabelThursday.Text = Convert.ToString(ResourceTable("LabelThursday"), Nothing).Trim()
            LabelThursday.Text = If(String.IsNullOrEmpty(LabelThursday.Text), "Thu", LabelThursday.Text)

            LabelFriday.Text = Convert.ToString(ResourceTable("LabelFriday"), Nothing).Trim()
            LabelFriday.Text = If(String.IsNullOrEmpty(LabelFriday.Text), "Fri", LabelFriday.Text)

            LabelSaturday.Text = Convert.ToString(ResourceTable("LabelSaturday"), Nothing).Trim()
            LabelSaturday.Text = If(String.IsNullOrEmpty(LabelSaturday.Text), "Sat", LabelSaturday.Text)

            LabelTotal.Text = Convert.ToString(ResourceTable("LabelTotal"), Nothing).Trim()
            LabelTotal.Text = If(String.IsNullOrEmpty(LabelTotal.Text), "Total", LabelTotal.Text)

            LabelStatus.Text = Convert.ToString(ResourceTable("LabelStatus"), Nothing).Trim()
            LabelStatus.Text = If(String.IsNullOrEmpty(LabelStatus.Text), "Status", LabelStatus.Text)

            LabelScheduleUpdateDate.Text = Convert.ToString(ResourceTable("LabelUpdateDate"), Nothing).Trim()
            LabelScheduleUpdateDate.Text = If(String.IsNullOrEmpty(LabelScheduleUpdateDate.Text), "Update Date", LabelScheduleUpdateDate.Text)

            LabelScheduleUpdateBy.Text = Convert.ToString(ResourceTable("LabelUpdateBy"), Nothing).Trim()
            LabelScheduleUpdateBy.Text = If(String.IsNullOrEmpty(LabelScheduleUpdateBy.Text), "Update By", LabelScheduleUpdateBy.Text)

            LabelDetailInfo.Text = Convert.ToString(ResourceTable("LabelDetailInfo"), Nothing).Trim()
            LabelDetailInfo.Text = If(String.IsNullOrEmpty(LabelDetailInfo.Text), "Detail Info", LabelDetailInfo.Text)

            ButtonSaveSchedule.Text = Convert.ToString(ResourceTable("ButtonSaveSchedule"), Nothing).Trim()
            ButtonSaveSchedule.Text = If(String.IsNullOrEmpty(ButtonSaveSchedule.Text), "Save", ButtonSaveSchedule.Text)

            If (Not ButtonIndividualDetail Is Nothing) Then
                ButtonIndividualDetail.Text = Convert.ToString(ResourceTable("ButtonIndividualDetail"), Nothing).Trim()
                ButtonIndividualDetail.Text = If(String.IsNullOrEmpty(ButtonIndividualDetail.Text), "Individual Detail", ButtonIndividualDetail.Text)
            End If

            If (Not ButtonEmployeeDetail Is Nothing) Then
                ButtonEmployeeDetail.Text = Convert.ToString(ResourceTable("ButtonEmployeeDetail"), Nothing).Trim()
                ButtonEmployeeDetail.Text = If(String.IsNullOrEmpty(ButtonEmployeeDetail.Text), "Employee Detail", ButtonEmployeeDetail.Text)
            End If

            ButtonClearSchedule.Text = Convert.ToString(ResourceTable("ButtonClearSchedule"), Nothing).Trim()
            ButtonClearSchedule.Text = If(String.IsNullOrEmpty(ButtonClearSchedule.Text), "Clear", ButtonClearSchedule.Text)

            Dim ResultOut As Boolean

            ValidationEnable = If((Boolean.TryParse(ResourceTable("ValidationEnable"), ResultOut)), ResultOut, True)

            ScheduleValidationGroup = Convert.ToString(ResourceTable("ValidationGroup"), Nothing)
            ScheduleValidationGroup = If(String.IsNullOrEmpty(ScheduleValidationGroup), "Calendar", ScheduleValidationGroup)

            ResourceTable = Nothing

        End Sub

        Protected Sub CreateControls(index As Integer)

            Dim CommonCss As String = "ci-heading ci-shadow-inset ui-input-text ui-body-c ui-shadow-inset"

            Dim ClientCss As String = String.Empty
            Dim EmployeeCss As String = String.Empty
            Dim SundayHourMinuteCss As String = String.Empty

            UpdatePanelManage = New UpdatePanel()
            UpdatePanelManage.ID = "UpdatePanelManage_" & index
            UpdatePanelManage.Attributes.Add("class", "UpdatePanelManage")
            UpdatePanelManage.UpdateMode = UpdatePanelUpdateMode.Always
            UpdatePanelManage.Attributes.Add("OnClick", "SetClientEmployeeId(" & index & ")")

            newRow = New HtmlGenericControl(objShared.GenericControlRow)
            newRow.Attributes.Add("class", "newRow")
            'newRow.ID = "Schedule" & index
            'newRow.ClientIDMode = ClientIDMode.Static
            'newRow.Attributes.Add("OnClick", "alert('" & index & "')")

            If (IsClientVisible And Not IsEmployeeVisible) Then
                ClientCss = "AutoColumn DivLabelIndividual"
                SundayHourMinuteCss = "SecondColumn DivDay"
            ElseIf (IsEmployeeVisible And Not IsClientVisible) Then
                EmployeeCss = "SecondColumn DivLabelEmployeeType DivLabelEmployeeType40"
                SundayHourMinuteCss = "SecondColumn DivDay DivLabelEmployeeType70"
            ElseIf (IsEmployeeVisible And IsClientVisible) Then
                ClientCss = "AutoColumn DivLabelIndividual"
                EmployeeCss = "SecondColumn DivLabelEmployeeType"
                SundayHourMinuteCss = "SecondColumn DivDay"
            End If

            DropDownListClient = objShared.AddControls(index, EnumDataObject.ControlType.DropDownList, newRow,
                                             "DropDownListClient", "DropDownListClient", True, ClientCss)
            DropDownListEmployee = objShared.AddControls(index, EnumDataObject.ControlType.DropDownList, newRow,
                                               "DropDownListEmployee", "DropDownListEmployee", True, EmployeeCss)

            DropDownListClient.Visible = IsClientVisible
            DropDownListEmployee.Visible = IsEmployeeVisible

            TextBoxSundayHourMinute = objShared.AddControls(index, EnumDataObject.ControlType.TextBox, newRow,
                                                  "TextBoxSundayHourMinute", "TextBoxSundayHourMinute " & CommonCss, True, SundayHourMinuteCss)
            TextBoxMondayHourMinute = objShared.AddControls(index, EnumDataObject.ControlType.TextBox, newRow,
                                                  "TextBoxMondayHourMinute", "TextBoxMondayHourMinute " & CommonCss, True, "SecondColumn DivDay")
            TextBoxTuesdayHourMinute = objShared.AddControls(index, EnumDataObject.ControlType.TextBox, newRow,
                                                   "TextBoxTuesdayHourMinute", "TextBoxTuesdayHourMinute " & CommonCss, True, "SecondColumn DivDay")
            TextBoxWednesdayHourMinute = objShared.AddControls(index, EnumDataObject.ControlType.TextBox, newRow,
                                                     "TextBoxWednesdayHourMinute", "TextBoxWednesdayHourMinute " & CommonCss, True, "SecondColumn DivDay")
            TextBoxThursdayHourMinute = objShared.AddControls(index, EnumDataObject.ControlType.TextBox, newRow,
                                                    "TextBoxThursdayHourMinute", "TextBoxThursdayHourMinute " & CommonCss, True, "SecondColumn DivDay")
            TextBoxFridayHourMinute = objShared.AddControls(index, EnumDataObject.ControlType.TextBox, newRow,
                                                  "TextBoxFridayHourMinute", "TextBoxFridayHourMinute " & CommonCss, True, "SecondColumn DivDay")
            TextBoxSaturdayHourMinute = objShared.AddControls(index, EnumDataObject.ControlType.TextBox, newRow,
                                                    "TextBoxSaturdayHourMinute", "TextBoxSaturdayHourMinute " & CommonCss, True, "SecondColumn DivDay")

            TextBoxTotalHourMinute = objShared.AddControls(index, EnumDataObject.ControlType.TextBox, newRow,
                                                 "TextBoxTotalHourMinute", "TextBoxTotalHourMinute " & CommonCss, True, "SecondColumn DivTotal")

            DropDownListScheduleStatus = objShared.AddControls(index, EnumDataObject.ControlType.DropDownList, newRow,
                                                     "DropDownListScheduleStatus", "DropDownListScheduleStatus " & CommonCss, True, "SecondColumn DivScheduleStatus")

            TextBoxScheduleUpdateDate = objShared.AddControls(index, EnumDataObject.ControlType.TextBox, newRow,
                                                    "TextBoxUpdateDate", "TextBoxCalendarUpdateDate " & CommonCss, True, "SecondColumn DivCalendarUpdateDate")

            TextBoxScheduleUpdateBy = objShared.AddControls(index, EnumDataObject.ControlType.TextBox, newRow,
                                                  "TextBoxUpdateBy", "TextBoxCalendarUpdateBy " & CommonCss, True, "SecondColumn DivCalendarUpdateBy")

            UpdatePanelManage.ContentTemplateContainer.Controls.Add(newRow)
            'PlaceHolderCalendarSetting.Controls.Add(newRow)

            newRow = New HtmlGenericControl(objShared.GenericControlRow)
            newRow.Attributes.Add("class", "newRow")
            UpdatePanelManage.ContentTemplateContainer.Controls.Add(newRow)
            'PlaceHolderCalendarSetting.Controls.Add(newRow)

            CalendarServiceBox = New HtmlGenericControl(objShared.GenericControlRow)
            CalendarServiceBox.Attributes.Add("class", "CalendarServiceBox")

            ServiceDaysInOut = New HtmlGenericControl(objShared.GenericControlRow)
            ServiceDaysInOut.Attributes.Add("class", "CalendarBoxStyle CalendarServiceLeft ServiceDaysInOut")

            newRow = New HtmlGenericControl(objShared.GenericControlRow)
            newRow.Attributes.Add("class", "newRow")

            LabelDayCaptionFirstColumn = objShared.AddControls(index, EnumDataObject.ControlType.Label, newRow,
                                                     "LabelDayCaptionFirstColumn", String.Empty, True, "AutoColumn DivLabelDayCaption")
            LabelInCaptionFirstColumn = objShared.AddControls(index, EnumDataObject.ControlType.Label, newRow,
                                                    "LabelInCaptionFirstColumn", String.Empty, True, "SecondColumn DivLabelInCaption")
            LabelOutCaptionFirstColumn = objShared.AddControls(index, EnumDataObject.ControlType.Label, newRow,
                                                     "LabelOutCaptionFirstColumn", String.Empty, True, "AutoColumn DivLabelOutCaption")

            ServiceDaysInOut.Controls.Add(newRow)

            newRow = New HtmlGenericControl(objShared.GenericControlRow)
            newRow.Attributes.Add("class", "newRow")

            LabelSundayInOutCaption = objShared.AddControls(index, EnumDataObject.ControlType.Label, newRow,
                                                  "LabelSundayInOutCaption", String.Empty, True, "AutoColumn DivLabelDayCaption")


            TextBoxSundayInTime = objShared.AddTextBox(index, newRow, "TextBoxSundayInTime", "TextBoxSundayInTime dt " & CommonCss, True, "SecondColumn DivLabelInCaption", True,
                                             RequiredFieldValidatorSundayInTime, "RequiredFieldValidatorSundayInTime")

            TextBoxSundayOutTime = objShared.AddTextBox(index, newRow, "TextBoxSundayOutTime", "TextBoxSundayOutTime dt " & CommonCss, True, "AutoColumn DivLabelOutCaption", True,
                                              RequiredFieldValidatorSundayOutTime, "RequiredFieldValidatorSundayOutTime")

            ServiceDaysInOut.Controls.Add(newRow)

            newRow = New HtmlGenericControl(objShared.GenericControlRow)
            newRow.Attributes.Add("class", "newRow")

            LabelMondayInOutCaption = objShared.AddControls(index, EnumDataObject.ControlType.Label, newRow,
                                                  "LabelMondayInOutCaption", String.Empty, True, "AutoColumn DivLabelDayCaption")

            TextBoxMondayInTime = objShared.AddTextBox(index, newRow, "TextBoxMondayInTime", "TextBoxMondayInTime dt " & CommonCss, True, "SecondColumn DivLabelInCaption", True,
                                             RequiredFieldValidatorMondayInTime, "RequiredFieldValidatorMondayInTime")

            TextBoxMondayOutTime = objShared.AddTextBox(index, newRow, "TextBoxMondayOutTime", "TextBoxMondayOutTime dt " & CommonCss, True, "AutoColumn DivLabelOutCaption", True,
                                              RequiredFieldValidatorMondayOutTime, "RequiredFieldValidatorMondayOutTime")

            ServiceDaysInOut.Controls.Add(newRow)

            newRow = New HtmlGenericControl(objShared.GenericControlRow)
            newRow.Attributes.Add("class", "newRow")

            LabelTuesdayInOutCaption = objShared.AddControls(index, EnumDataObject.ControlType.Label, newRow,
                                                   "LabelTuesdayInOutCaption", String.Empty, True, "AutoColumn DivLabelDayCaption")

            TextBoxTuesdayInTime = objShared.AddTextBox(index, newRow, "TextBoxTuesdayInTime", "TextBoxTuesdayInTime dt " & CommonCss, True, "SecondColumn DivLabelInCaption", True,
                                              RequiredFieldValidatorTuesdayInTime, "RequiredFieldValidatorTuesdayInTime")

            TextBoxTuesdayOutTime = objShared.AddTextBox(index, newRow, "TextBoxTuesdayOutTime", "TextBoxTuesdayOutTime dt " & CommonCss, True, "AutoColumn DivLabelOutCaption", True,
                                               RequiredFieldValidatorTuesdayOutTime, "RequiredFieldValidatorTuesdayOutTime")

            ServiceDaysInOut.Controls.Add(newRow)

            newRow = New HtmlGenericControl(objShared.GenericControlRow)
            newRow.Attributes.Add("class", "newRow")

            LabelWednesdayInOutCaption = objShared.AddControls(index, EnumDataObject.ControlType.Label, newRow,
                                                     "LabelWednesdayInOutCaption", String.Empty, True, "AutoColumn DivLabelDayCaption")

            TextBoxWednesdayInTime = objShared.AddTextBox(index, newRow, "TextBoxWednesdayInTime", "TextBoxWednesdayInTime dt " & CommonCss, True, "SecondColumn DivLabelInCaption", True,
                                                RequiredFieldValidatorWednesdayInTime, "RequiredFieldValidatorWednesdayInTime")

            TextBoxWednesdayOutTime = objShared.AddTextBox(index, newRow, "TextBoxWednesdayOutTime", "TextBoxWednesdayOutTime dt " & CommonCss, True, "AutoColumn DivLabelOutCaption", True,
                                                 RequiredFieldValidatorWednesdayOutTime, "RequiredFieldValidatorWednesdayOutTime")

            ServiceDaysInOut.Controls.Add(newRow)

            newRow = New HtmlGenericControl(objShared.GenericControlRow)
            newRow.Attributes.Add("class", "DivSpace5")

            ServiceDaysInOut.Controls.Add(newRow)

            CalendarServiceBox.Controls.Add(ServiceDaysInOut)
            UpdatePanelManage.ContentTemplateContainer.Controls.Add(CalendarServiceBox)
            'PlaceHolderCalendarSetting.Controls.Add(CalendarServiceBox)

            '*******************2nd column

            CalendarServiceBox = New HtmlGenericControl(objShared.GenericControlRow)
            CalendarServiceBox.Attributes.Add("class", "CalendarServiceBox")

            ServiceDaysInOut = New HtmlGenericControl(objShared.GenericControlRow)
            ServiceDaysInOut.Attributes.Add("class", "CalendarBoxStyle CalendarServiceLeft ServiceDaysInOut")

            newRow = New HtmlGenericControl(objShared.GenericControlRow)
            newRow.Attributes.Add("class", "newRow")

            LabelDayCaptionSecondColumn = objShared.AddControls(index, EnumDataObject.ControlType.Label, newRow,
                                                      "LabelDayCaptionSecondColumn", String.Empty, True, "AutoColumn DivLabelDayCaption")
            LabelInCaptionSecondColumn = objShared.AddControls(index, EnumDataObject.ControlType.Label, newRow,
                                                     "LabelInCaptionSecondColumn", String.Empty, True, "SecondColumn DivLabelInCaption")
            LabelOutCaptionSecondColumn = objShared.AddControls(index, EnumDataObject.ControlType.Label, newRow,
                                                      "LabelOutCaptionSecondColumn", String.Empty, True, "AutoColumn DivLabelOutCaption")

            ServiceDaysInOut.Controls.Add(newRow)

            newRow = New HtmlGenericControl(objShared.GenericControlRow)
            newRow.Attributes.Add("class", "newRow")

            LabelThursdayInOutCaption = objShared.AddControls(index, EnumDataObject.ControlType.Label, newRow,
                                                    "LabelThursdayInOutCaption", String.Empty, True, "AutoColumn DivLabelDayCaption")

            TextBoxThursdayInTime = objShared.AddTextBox(index, newRow, "TextBoxThursdayInTime", "TextBoxThursdayInTime dt " & CommonCss, True, "SecondColumn DivLabelInCaption", True,
                                               RequiredFieldValidatorThursdayInTime, "RequiredFieldValidatorThursdayInTime")

            TextBoxThursdayOutTime = objShared.AddTextBox(index, newRow, "TextBoxThursdayOutTime", "TextBoxThursdayOutTime dt " & CommonCss, True, "AutoColumn DivLabelOutCaption", True,
                                                RequiredFieldValidatorThursdayOutTime, "RequiredFieldValidatorThursdayOutTime")

            ServiceDaysInOut.Controls.Add(newRow)

            newRow = New HtmlGenericControl(objShared.GenericControlRow)
            newRow.Attributes.Add("class", "newRow")

            LabelFridayInOutCaption = objShared.AddControls(index, EnumDataObject.ControlType.Label, newRow,
                                                  "LabelFridayInOutCaption", String.Empty, True, "AutoColumn DivLabelDayCaption")

            TextBoxFridayInTime = objShared.AddTextBox(index, newRow, "TextBoxFridayInTime", "TextBoxFridayInTime dt " & CommonCss, True, "SecondColumn DivLabelInCaption", True,
                                             RequiredFieldValidatorFridayInTime, "RequiredFieldValidatorFridayInTime")

            TextBoxFridayOutTime = objShared.AddTextBox(index, newRow, "TextBoxFridayOutTime", "TextBoxFridayOutTime dt " & CommonCss, True, "AutoColumn DivLabelOutCaption", True,
                                              RequiredFieldValidatorFridayOutTime, "RequiredFieldValidatorFridayOutTime")

            ServiceDaysInOut.Controls.Add(newRow)

            newRow = New HtmlGenericControl(objShared.GenericControlRow)
            newRow.Attributes.Add("class", "newRow")

            LabelSaturdayInOutCaption = objShared.AddControls(index, EnumDataObject.ControlType.Label, newRow,
                                                    "LabelSaturdayInOutCaption", String.Empty, True, "AutoColumn DivLabelDayCaption")

            TextBoxSaturdayInTime = objShared.AddTextBox(index, newRow, "TextBoxSaturdayInTime", "TextBoxSaturdayInTime dt " & CommonCss, True, "SecondColumn DivLabelInCaption", True,
                                               RequiredFieldValidatorSaturdayInTime, "RequiredFieldValidatorSaturdayInTime")

            TextBoxSaturdayOutTime = objShared.AddTextBox(index, newRow, "TextBoxSaturdayOutTime", "TextBoxSaturdayOutTime dt " & CommonCss, True, "AutoColumn DivLabelOutCaption", True,
                                                RequiredFieldValidatorSaturdayOutTime, "RequiredFieldValidatorSaturdayOutTime")

            ServiceDaysInOut.Controls.Add(newRow)

            newRow = New HtmlGenericControl(objShared.GenericControlRow)
            newRow.Attributes.Add("class", "DivSpace5")

            ServiceDaysInOut.Controls.Add(newRow)

            CalendarServiceBox.Controls.Add(ServiceDaysInOut)
            UpdatePanelManage.ContentTemplateContainer.Controls.Add(CalendarServiceBox)
            'PlaceHolderCalendarSetting.Controls.Add(CalendarServiceBox)

            '************payrate
            CalendarServiceBox = New HtmlGenericControl(objShared.GenericControlRow)
            CalendarServiceBox.Attributes.Add("class", "CalendarServiceBox")

            ServiceDaysInOut = New HtmlGenericControl(objShared.GenericControlRow)
            ServiceDaysInOut.Attributes.Add("class", "CalendarBoxStyle CalendarServiceLeft ServiceDaysInOut")

            newRow = New HtmlGenericControl(objShared.GenericControlRow)
            newRow.Attributes.Add("class", "newRow")

            LabelSpecialPayrate = objShared.AddControls(index, EnumDataObject.ControlType.Label, newRow,
                                              "LabelSpecialPayrate", String.Empty, True, "SpecialPayrateColumn1")

            SecondColumn = New HtmlGenericControl(objShared.GenericControlColumn)
            SecondColumn.Attributes.Add("class", "SecondColumn")
            TextBoxSpecialPayrate = New TextBox()
            TextBoxSpecialPayrate.ID = "TextBoxSpecialPayrate_" & index
            TextBoxSpecialPayrate.CssClass = "TextBoxSpecialPayrate"
            SecondColumn.Controls.Add(TextBoxSpecialPayrate)

            RegularExpressionValidatorSpecialPayrate = New RegularExpressionValidator
            RegularExpressionValidatorSpecialPayrate.ID = "RegularExpressionValidatorSpecialPayrate_" & index
            SecondColumn.Controls.Add(RegularExpressionValidatorSpecialPayrate)

            newRow.Controls.Add(SecondColumn)

            ServiceDaysInOut.Controls.Add(newRow)

            newRow = New HtmlGenericControl(objShared.GenericControlRow)
            newRow.Attributes.Add("class", "newRow")

            LabelStartDate = objShared.AddControls(index, EnumDataObject.ControlType.Label, newRow,
                                         "LabelStartDate", String.Empty, True, "SpecialPayrateColumn1")

            TextBoxStartDate = objShared.AddControls(index, EnumDataObject.ControlType.TextBox, newRow,
                                           "TextBoxStartDate", "TextBoxStartDate dateField", True, "SecondColumn")

            RegularExpressionValidatorStartDate = New RegularExpressionValidator
            RegularExpressionValidatorStartDate.ID = "RegularExpressionValidatorStartDate_" & index
            newRow.Controls.Add(RegularExpressionValidatorStartDate)

            ServiceDaysInOut.Controls.Add(newRow)

            newRow = New HtmlGenericControl(objShared.GenericControlRow)
            newRow.Attributes.Add("class", "newRow")

            LabelEndDate = objShared.AddControls(index, EnumDataObject.ControlType.Label, newRow,
                                       "LabelEndDate", String.Empty, True, "SpecialPayrateColumn1")

            TextBoxEndDate = objShared.AddControls(index, EnumDataObject.ControlType.TextBox, newRow,
                                         "TextBoxEndDate", "TextBoxEndDate dateField", True, "SecondColumn")

            RegularExpressionValidatorEndDate = New RegularExpressionValidator
            RegularExpressionValidatorEndDate.ID = "RegularExpressionValidatorEndDate_" & index
            newRow.Controls.Add(RegularExpressionValidatorEndDate)

            ServiceDaysInOut.Controls.Add(newRow)

            newRow = New HtmlGenericControl(objShared.GenericControlRow)
            newRow.Attributes.Add("class", "newRow")

            Dim LabelEmptyColumn As Label = objShared.AddControls(index, EnumDataObject.ControlType.Label, newRow,
                                       "LabelEmptyColumn", String.Empty, True, "SpecialPayrateColumn1")
            LabelEmptyColumn.Text = "&nbsp;"

            TextBoxScheduleComments = objShared.AddControls(index, EnumDataObject.ControlType.TextBox, newRow,
                                                  "TextBoxComments", "TextBoxScheduleComments " & CommonCss, True, "AutoColumn DivTextBoxComments")
            TextBoxScheduleComments.TextMode = TextBoxMode.MultiLine

            ServiceDaysInOut.Controls.Add(newRow)

            newRow = New HtmlGenericControl(objShared.GenericControlRow)
            newRow.Attributes.Add("class", "DivSpace5")

            ServiceDaysInOut.Controls.Add(newRow)

            CalendarServiceBox.Controls.Add(ServiceDaysInOut)

            UpdatePanelManage.ContentTemplateContainer.Controls.Add(CalendarServiceBox)
            'PlaceHolderCalendarSetting.Controls.Add(CalendarServiceBox)

            PlaceHolderSchedule.Controls.Add(UpdatePanelManage)

            Dim Trigger As AsyncPostBackTrigger = Nothing

            If (IsClientVisible) Then
                Trigger = New AsyncPostBackTrigger()
                Trigger.ControlID = "DropDownListClient_" & index
                UpdatePanelManage.Triggers.Add(Trigger)

            End If

            If (IsEmployeeVisible) Then
                Trigger = New AsyncPostBackTrigger()
                Trigger.ControlID = "DropDownListEmployee_" & index
                UpdatePanelManage.Triggers.Add(Trigger)
            End If

            Trigger = New AsyncPostBackTrigger()
            Trigger.ControlID = "TextBoxSundayInTime_" & index
            UpdatePanelManage.Triggers.Add(Trigger)

            Trigger = New AsyncPostBackTrigger()
            Trigger.ControlID = "TextBoxSundayOutTime_" & index
            UpdatePanelManage.Triggers.Add(Trigger)

            Trigger = New AsyncPostBackTrigger()
            Trigger.ControlID = "TextBoxMondayInTime_" & index
            UpdatePanelManage.Triggers.Add(Trigger)

            Trigger = New AsyncPostBackTrigger()
            Trigger.ControlID = "TextBoxMondayOutTime_" & index
            UpdatePanelManage.Triggers.Add(Trigger)

            Trigger = New AsyncPostBackTrigger()
            Trigger.ControlID = "TextBoxTuesdayInTime_" & index
            UpdatePanelManage.Triggers.Add(Trigger)

            Trigger = New AsyncPostBackTrigger()
            Trigger.ControlID = "TextBoxTuesdayOutTime_" & index
            UpdatePanelManage.Triggers.Add(Trigger)

            Trigger = New AsyncPostBackTrigger()
            Trigger.ControlID = "TextBoxWednesdayInTime_" & index
            UpdatePanelManage.Triggers.Add(Trigger)

            Trigger = New AsyncPostBackTrigger()
            Trigger.ControlID = "TextBoxWednesdayOutTime_" & index
            UpdatePanelManage.Triggers.Add(Trigger)

            Trigger = New AsyncPostBackTrigger()
            Trigger.ControlID = "TextBoxThursdayInTime_" & index
            UpdatePanelManage.Triggers.Add(Trigger)

            Trigger = New AsyncPostBackTrigger()
            Trigger.ControlID = "TextBoxThursdayOutTime_" & index
            UpdatePanelManage.Triggers.Add(Trigger)

            Trigger = New AsyncPostBackTrigger()
            Trigger.ControlID = "TextBoxFridayInTime_" & index
            UpdatePanelManage.Triggers.Add(Trigger)

            Trigger = New AsyncPostBackTrigger()
            Trigger.ControlID = "TextBoxFridayOutTime_" & index
            UpdatePanelManage.Triggers.Add(Trigger)

            Trigger = New AsyncPostBackTrigger()
            Trigger.ControlID = "TextBoxSaturdayInTime_" & index
            UpdatePanelManage.Triggers.Add(Trigger)

            Trigger = New AsyncPostBackTrigger()
            Trigger.ControlID = "TextBoxSaturdayOutTime_" & index
            UpdatePanelManage.Triggers.Add(Trigger)

            GetDynamicControlCaptionFromResource()

            InitializeDynamicControl(index)

            'If (Not IsPostBack) Then
            BindDynamicDropDownList()
            'End If

        End Sub

        Protected Sub SetSchduleControlTextLength()

            TextBoxSundayHourMinute.MaxLength = InlineAssignHelper(TextBoxMondayHourMinute.MaxLength, InlineAssignHelper(TextBoxTuesdayHourMinute.MaxLength,
                                                InlineAssignHelper(TextBoxWednesdayHourMinute.MaxLength, InlineAssignHelper(TextBoxThursdayHourMinute.MaxLength,
                                                InlineAssignHelper(TextBoxFridayHourMinute.MaxLength, InlineAssignHelper(TextBoxSaturdayHourMinute.MaxLength,
                                                InlineAssignHelper(TextBoxTotalHourMinute.MaxLength, 5)))))))

            TextBoxSundayInTime.MaxLength = InlineAssignHelper(TextBoxSundayOutTime.MaxLength, InlineAssignHelper(TextBoxMondayInTime.MaxLength,
                                            InlineAssignHelper(TextBoxMondayOutTime.MaxLength, InlineAssignHelper(TextBoxTuesdayInTime.MaxLength,
                                            InlineAssignHelper(TextBoxTuesdayOutTime.MaxLength, InlineAssignHelper(TextBoxWednesdayInTime.MaxLength,
                                            InlineAssignHelper(TextBoxWednesdayOutTime.MaxLength, InlineAssignHelper(TextBoxThursdayInTime.MaxLength,
                                            InlineAssignHelper(TextBoxThursdayOutTime.MaxLength, InlineAssignHelper(TextBoxFridayInTime.MaxLength,
                                            InlineAssignHelper(TextBoxFridayOutTime.MaxLength, InlineAssignHelper(TextBoxSaturdayInTime.MaxLength,
                                            InlineAssignHelper(TextBoxSaturdayOutTime.MaxLength, 8)))))))))))))

            TextBoxSpecialPayrate.MaxLength = 10
            TextBoxStartDate.MaxLength = InlineAssignHelper(TextBoxEndDate.MaxLength, InlineAssignHelper(TextBoxScheduleUpdateDate.MaxLength, 17))
            TextBoxScheduleComments.MaxLength = 150
            TextBoxScheduleUpdateBy.MaxLength = 50

        End Sub

        Protected Sub SetJavascriptEvent()

            'TextBoxSundayHourMinute.Attributes.Add("onkeyup", "return HourMinuteChangeEvent(event);")
            'TextBoxMondayHourMinute.Attributes.Add("onkeyup", "return HourMinuteChangeEvent(event);")
            'TextBoxTuesdayHourMinute.Attributes.Add("onkeyup", "return HourMinuteChangeEvent(event);")
            'TextBoxWednesdayHourMinute.Attributes.Add("onkeyup", "return HourMinuteChangeEvent(event);")
            'TextBoxThursdayHourMinute.Attributes.Add("onkeyup", "return HourMinuteChangeEvent(event);")
            'TextBoxFridayHourMinute.Attributes.Add("onkeyup", "return HourMinuteChangeEvent(event);")
            'TextBoxSaturdayHourMinute.Attributes.Add("onkeyup", "return HourMinuteChangeEvent(event);")

            'TextBoxSundayHourMinute.Attributes.Add("onkeypress", "return IsHourMinute(event);")
            'TextBoxMondayHourMinute.Attributes.Add("onkeypress", "return IsHourMinute(event);")
            'TextBoxTuesdayHourMinute.Attributes.Add("onkeypress", "return IsHourMinute(event);")
            'TextBoxWednesdayHourMinute.Attributes.Add("onkeypress", "return IsHourMinute(event);")
            'TextBoxThursdayHourMinute.Attributes.Add("onkeypress", "return IsHourMinute(event);")
            'TextBoxFridayHourMinute.Attributes.Add("onkeypress", "return IsHourMinute(event);")
            'TextBoxSaturdayHourMinute.Attributes.Add("onkeypress", "return IsHourMinute(event);")

            'TextBoxSundayInTime.Attributes.Add("onkeyup", "return InTimeOutTimeChangeEvent(event);")
            'TextBoxSundayOutTime.Attributes.Add("onkeyup", "return InTimeOutTimeChangeEvent(event);")
            'TextBoxMondayInTime.Attributes.Add("onkeyup", "return InTimeOutTimeChangeEvent(event);")
            'TextBoxMondayOutTime.Attributes.Add("onkeyup", "return InTimeOutTimeChangeEvent(event);")
            'TextBoxTuesdayInTime.Attributes.Add("onkeyup", "return InTimeOutTimeChangeEvent(event);")
            'TextBoxTuesdayOutTime.Attributes.Add("onkeyup", "return InTimeOutTimeChangeEvent(event);")
            'TextBoxWednesdayInTime.Attributes.Add("onkeyup", "return InTimeOutTimeChangeEvent(event);")
            'TextBoxWednesdayOutTime.Attributes.Add("onkeyup", "return InTimeOutTimeChangeEvent(event);")
            'TextBoxThursdayInTime.Attributes.Add("onkeyup", "return InTimeOutTimeChangeEvent(event);")
            'TextBoxThursdayOutTime.Attributes.Add("onkeyup", "return InTimeOutTimeChangeEvent(event);")
            'TextBoxFridayInTime.Attributes.Add("onkeyup", "return InTimeOutTimeChangeEvent(event);")
            'TextBoxFridayOutTime.Attributes.Add("onkeyup", "return InTimeOutTimeChangeEvent(event);")
            'TextBoxSaturdayInTime.Attributes.Add("onkeyup", "return InTimeOutTimeChangeEvent(event);")
            'TextBoxSaturdayOutTime.Attributes.Add("onkeyup", "return InTimeOutTimeChangeEvent(event);")


            'TextBoxSundayInTime.Attributes.Add("onchange", "return InTimeOutTimeTextChangeEvent(event);")
            'TextBoxSundayOutTime.Attributes.Add("onchange", "return InTimeOutTimeTextChangeEvent(event);")

        End Sub

        Private Sub SetShadowTextIndicator()

            TextBoxSundayHourMinute.Attributes.Add("placeholder", HourMinuteFormat)
            TextBoxMondayHourMinute.Attributes.Add("placeholder", HourMinuteFormat)
            TextBoxTuesdayHourMinute.Attributes.Add("placeholder", HourMinuteFormat)
            TextBoxWednesdayHourMinute.Attributes.Add("placeholder", HourMinuteFormat)
            TextBoxThursdayHourMinute.Attributes.Add("placeholder", HourMinuteFormat)
            TextBoxFridayHourMinute.Attributes.Add("placeholder", HourMinuteFormat)
            TextBoxSaturdayHourMinute.Attributes.Add("placeholder", HourMinuteFormat)
            TextBoxTotalHourMinute.Attributes.Add("placeholder", HourMinuteFormat)

            TextBoxSundayInTime.Attributes.Add("placeholder", TimeFormat)
            TextBoxMondayInTime.Attributes.Add("placeholder", TimeFormat)
            TextBoxTuesdayInTime.Attributes.Add("placeholder", TimeFormat)
            TextBoxWednesdayInTime.Attributes.Add("placeholder", TimeFormat)
            TextBoxThursdayInTime.Attributes.Add("placeholder", TimeFormat)
            TextBoxFridayInTime.Attributes.Add("placeholder", TimeFormat)
            TextBoxSaturdayInTime.Attributes.Add("placeholder", TimeFormat)

            TextBoxSundayOutTime.Attributes.Add("placeholder", TimeFormat)
            TextBoxMondayOutTime.Attributes.Add("placeholder", TimeFormat)
            TextBoxTuesdayOutTime.Attributes.Add("placeholder", TimeFormat)
            TextBoxWednesdayOutTime.Attributes.Add("placeholder", TimeFormat)
            TextBoxThursdayOutTime.Attributes.Add("placeholder", TimeFormat)
            TextBoxFridayOutTime.Attributes.Add("placeholder", TimeFormat)
            TextBoxSaturdayOutTime.Attributes.Add("placeholder", TimeFormat)

        End Sub

        ''' <summary>
        ''' Retrieving Calendar Schedule Info by Client Id and Client Status
        ''' </summary>
        ''' <returns></returns>
        Protected Function GetCalendarScheduleInfo(ClientId As Integer, EmployeeId As Integer, Status As String) As List(Of CalendarSettingDataObject)

            Dim CalendarSettingList As New List(Of CalendarSettingDataObject)

            Dim objBLCalendarSetting As New BLCalendarSetting()

            CalendarSettingList = objBLCalendarSetting.SelectCalendarInfo(objShared.ConVisitel, objShared.CompanyId, ClientId, EmployeeId, Status)

            objBLCalendarSetting = Nothing

            Return CalendarSettingList

        End Function

        Protected Sub LoadSelectedCalendarSetting()

            Dim objCalendarSettingDataObject As New CalendarSettingDataObject
            objCalendarSettingDataObject = (From p In ScheduleList Where p.ScheduleId = hfScheduleId.Value).SingleOrDefault

            If (objCalendarSettingDataObject Is Nothing) Then
                Return
            End If

            DropDownListClient.SelectedIndex = DropDownListClient.Items.IndexOf(DropDownListClient.Items.FindByValue(
                                                                                               Convert.ToString(objCalendarSettingDataObject.ClientId)))

            DropDownListEmployee.SelectedIndex = DropDownListEmployee.Items.IndexOf(DropDownListEmployee.Items.FindByValue(
                                                                                               Convert.ToString(objCalendarSettingDataObject.EmployeeId)))

            TextBoxSundayHourMinute.Text = If((String.IsNullOrEmpty(objCalendarSettingDataObject.SundayHourMinute)),
                                              String.Empty,
                                              objCalendarSettingDataObject.SundayHourMinute.Split(":")(0) & ":" & objCalendarSettingDataObject.SundayHourMinute.Split(":")(1))

            TextBoxMondayHourMinute.Text = If((String.IsNullOrEmpty(objCalendarSettingDataObject.MondayHourMinute)),
                                              String.Empty,
                                              objCalendarSettingDataObject.MondayHourMinute.Split(":")(0) & ":" & objCalendarSettingDataObject.MondayHourMinute.Split(":")(1))

            TextBoxTuesdayHourMinute.Text = If((String.IsNullOrEmpty(objCalendarSettingDataObject.TuesdayHourMinute)),
                                               String.Empty,
                                               objCalendarSettingDataObject.TuesdayHourMinute.Split(":")(0) & ":" & objCalendarSettingDataObject.TuesdayHourMinute.Split(":")(1))

            TextBoxWednesdayHourMinute.Text = If((String.IsNullOrEmpty(objCalendarSettingDataObject.WednesdayHourMinute)),
                                                 String.Empty,
                                                 objCalendarSettingDataObject.WednesdayHourMinute.Split(":")(0) & ":" & objCalendarSettingDataObject.WednesdayHourMinute.Split(":")(1))

            TextBoxThursdayHourMinute.Text = If((String.IsNullOrEmpty(objCalendarSettingDataObject.ThursdayHourMinute)),
                                                String.Empty,
                                                objCalendarSettingDataObject.ThursdayHourMinute.Split(":")(0) & ":" & objCalendarSettingDataObject.ThursdayHourMinute.Split(":")(1))

            TextBoxFridayHourMinute.Text = If((String.IsNullOrEmpty(objCalendarSettingDataObject.FridayHourMinute)),
                                              String.Empty,
                                              objCalendarSettingDataObject.FridayHourMinute.Split(":")(0) & ":" & objCalendarSettingDataObject.FridayHourMinute.Split(":")(1))

            TextBoxSaturdayHourMinute.Text = If((String.IsNullOrEmpty(objCalendarSettingDataObject.SaturdayHourMinute)),
                                                String.Empty,
                                                objCalendarSettingDataObject.SaturdayHourMinute.Split(":")(0) & ":" & objCalendarSettingDataObject.SaturdayHourMinute.Split(":")(1))

            Dim TotalHour As Int32 = 0
            Dim TotalMinute As Int32 = 0

            TotalHour = (If((String.IsNullOrEmpty(objCalendarSettingDataObject.SundayHourMinute)), 0,
                           Convert.ToInt32(objCalendarSettingDataObject.SundayHourMinute.Split(":")(0)))) +
                        (If((String.IsNullOrEmpty(objCalendarSettingDataObject.MondayHourMinute)), 0,
                            Convert.ToInt32(objCalendarSettingDataObject.MondayHourMinute.Split(":")(0)))) +
                        (If((String.IsNullOrEmpty(objCalendarSettingDataObject.TuesdayHourMinute)), 0,
                            Convert.ToInt32(objCalendarSettingDataObject.TuesdayHourMinute.Split(":")(0)))) +
                        (If((String.IsNullOrEmpty(objCalendarSettingDataObject.WednesdayHourMinute)), 0,
                            Convert.ToInt32(objCalendarSettingDataObject.WednesdayHourMinute.Split(":")(0)))) +
                        (If((String.IsNullOrEmpty(objCalendarSettingDataObject.ThursdayHourMinute)), 0,
                            Convert.ToInt32(objCalendarSettingDataObject.ThursdayHourMinute.Split(":")(0)))) +
                        (If((String.IsNullOrEmpty(objCalendarSettingDataObject.FridayHourMinute)), 0,
                            Convert.ToInt32(objCalendarSettingDataObject.FridayHourMinute.Split(":")(0)))) +
                        (If((String.IsNullOrEmpty(objCalendarSettingDataObject.SaturdayHourMinute)), 0,
                            Convert.ToInt32(objCalendarSettingDataObject.SaturdayHourMinute.Split(":")(0))))

            TotalMinute = (If((String.IsNullOrEmpty(objCalendarSettingDataObject.SundayHourMinute)), 0,
                           Convert.ToInt32(objCalendarSettingDataObject.SundayHourMinute.Split(":")(1)))) +
                        (If((String.IsNullOrEmpty(objCalendarSettingDataObject.MondayHourMinute)), 0,
                            Convert.ToInt32(objCalendarSettingDataObject.MondayHourMinute.Split(":")(1)))) +
                        (If((String.IsNullOrEmpty(objCalendarSettingDataObject.TuesdayHourMinute)), 0,
                            Convert.ToInt32(objCalendarSettingDataObject.TuesdayHourMinute.Split(":")(1)))) +
                        (If((String.IsNullOrEmpty(objCalendarSettingDataObject.WednesdayHourMinute)), 0,
                            Convert.ToInt32(objCalendarSettingDataObject.WednesdayHourMinute.Split(":")(1)))) +
                        (If((String.IsNullOrEmpty(objCalendarSettingDataObject.ThursdayHourMinute)), 0,
                            Convert.ToInt32(objCalendarSettingDataObject.ThursdayHourMinute.Split(":")(1)))) +
                        (If((String.IsNullOrEmpty(objCalendarSettingDataObject.FridayHourMinute)), 0,
                            Convert.ToInt32(objCalendarSettingDataObject.FridayHourMinute.Split(":")(1)))) +
                        (If((String.IsNullOrEmpty(objCalendarSettingDataObject.SaturdayHourMinute)), 0,
                            Convert.ToInt32(objCalendarSettingDataObject.SaturdayHourMinute.Split(":")(1))))

            If TotalMinute >= 60 Then
                TotalHour += TotalMinute / 60
            End If

            TotalMinute = TotalMinute Mod 60

            TextBoxTotalHourMinute.Text = If((TotalHour.Equals(0) And TotalMinute.Equals(0)), String.Empty, TotalHour & ":" & TotalMinute)

            DropDownListScheduleStatus.SelectedIndex = DropDownListScheduleStatus.Items.IndexOf(DropDownListScheduleStatus.Items.FindByText(
                                                                                                Convert.ToString(objCalendarSettingDataObject.Status, Nothing)))

            TextBoxScheduleUpdateBy.Text = objCalendarSettingDataObject.UpdateBy

            TextBoxScheduleUpdateDate.Text = If((Not String.IsNullOrEmpty(Convert.ToString(objCalendarSettingDataObject.UpdateDate, Nothing))),
                                               objCalendarSettingDataObject.UpdateDate, objCalendarSettingDataObject.UpdateDate)

            TextBoxSpecialPayrate.Text = If(objCalendarSettingDataObject.SpecialRate.Equals(Decimal.MinValue),
                                            String.Empty,
                                            objShared.GetFormattedPayrate(objCalendarSettingDataObject.SpecialRate))

            TextBoxScheduleComments.Text = objCalendarSettingDataObject.Comments


            TextBoxStartDate.Text = If((Not String.IsNullOrEmpty(Convert.ToString(objCalendarSettingDataObject.StartDate, Nothing))),
                                                       objCalendarSettingDataObject.StartDate, objCalendarSettingDataObject.StartDate)


            TextBoxEndDate.Text = If((Not String.IsNullOrEmpty(Convert.ToString(objCalendarSettingDataObject.EndDate, Nothing))),
                                                       objCalendarSettingDataObject.EndDate, objCalendarSettingDataObject.EndDate)

            TextBoxSundayInTime.Text = objCalendarSettingDataObject.SundayInTime
            TextBoxSundayOutTime.Text = objCalendarSettingDataObject.SundayOutTime
            TextBoxMondayInTime.Text = objCalendarSettingDataObject.MondayInTime
            TextBoxMondayOutTime.Text = objCalendarSettingDataObject.MondayOutTime
            TextBoxTuesdayInTime.Text = objCalendarSettingDataObject.TuesdayInTime
            TextBoxTuesdayOutTime.Text = objCalendarSettingDataObject.TuesdayOutTime
            TextBoxWednesdayInTime.Text = objCalendarSettingDataObject.WednesdayInTime
            TextBoxWednesdayOutTime.Text = objCalendarSettingDataObject.WednesdayOutTime
            TextBoxThursdayInTime.Text = objCalendarSettingDataObject.ThursdayInTime
            TextBoxThursdayOutTime.Text = objCalendarSettingDataObject.ThursdayOutTime
            TextBoxFridayInTime.Text = objCalendarSettingDataObject.FridayInTime
            TextBoxFridayOutTime.Text = objCalendarSettingDataObject.FridayOutTime
            TextBoxSaturdayInTime.Text = objCalendarSettingDataObject.SaturdayInTime
            TextBoxSaturdayOutTime.Text = objCalendarSettingDataObject.SaturdayOutTime

        End Sub

        Protected Sub ControlReferenceAssign(index As Integer)

            DropDownListClient = DirectCast(PlaceHolderSchedule.FindControl("DropDownListClient_" & index), DropDownList)
            DropDownListEmployee = DirectCast(PlaceHolderSchedule.FindControl("DropDownListEmployee_" & index), DropDownList)

            TextBoxSundayHourMinute = DirectCast(PlaceHolderSchedule.FindControl("TextBoxSundayHourMinute_" & index), TextBox)
            TextBoxMondayHourMinute = DirectCast(PlaceHolderSchedule.FindControl("TextBoxMondayHourMinute_" & index), TextBox)
            TextBoxTuesdayHourMinute = DirectCast(PlaceHolderSchedule.FindControl("TextBoxTuesdayHourMinute_" & index), TextBox)
            TextBoxWednesdayHourMinute = DirectCast(PlaceHolderSchedule.FindControl("TextBoxWednesdayHourMinute_" & index), TextBox)
            TextBoxThursdayHourMinute = DirectCast(PlaceHolderSchedule.FindControl("TextBoxThursdayHourMinute_" & index), TextBox)
            TextBoxFridayHourMinute = DirectCast(PlaceHolderSchedule.FindControl("TextBoxFridayHourMinute_" & index), TextBox)
            TextBoxSaturdayHourMinute = DirectCast(PlaceHolderSchedule.FindControl("TextBoxSaturdayHourMinute_" & index), TextBox)

            TextBoxTotalHourMinute = DirectCast(PlaceHolderSchedule.FindControl("TextBoxTotalHourMinute_" & index), TextBox)

            DropDownListScheduleStatus = DirectCast(PlaceHolderSchedule.FindControl("DropDownListScheduleStatus_" & index), DropDownList)

            TextBoxScheduleUpdateBy = DirectCast(PlaceHolderSchedule.FindControl("TextBoxUpdateBy_" & index), TextBox)
            TextBoxScheduleUpdateDate = DirectCast(PlaceHolderSchedule.FindControl("TextBoxUpdateDate_" & index), TextBox)

            TextBoxSpecialPayrate = DirectCast(PlaceHolderSchedule.FindControl("TextBoxSpecialPayrate_" & index), TextBox)

            TextBoxScheduleComments = DirectCast(PlaceHolderSchedule.FindControl("TextBoxComments_" & index), TextBox)

            TextBoxStartDate = DirectCast(PlaceHolderSchedule.FindControl("TextBoxStartDate_" & index), TextBox)
            TextBoxEndDate = DirectCast(PlaceHolderSchedule.FindControl("TextBoxEndDate_" & index), TextBox)


            TextBoxSundayInTime = DirectCast(PlaceHolderSchedule.FindControl("TextBoxSundayInTime_" & index), TextBox)
            TextBoxSundayOutTime = DirectCast(PlaceHolderSchedule.FindControl("TextBoxSundayOutTime_" & index), TextBox)
            TextBoxSundayOutTime = DirectCast(PlaceHolderSchedule.FindControl("TextBoxSundayOutTime_" & index), TextBox)
            TextBoxMondayInTime = DirectCast(PlaceHolderSchedule.FindControl("TextBoxMondayInTime_" & index), TextBox)
            TextBoxMondayOutTime = DirectCast(PlaceHolderSchedule.FindControl("TextBoxMondayOutTime_" & index), TextBox)
            TextBoxTuesdayInTime = DirectCast(PlaceHolderSchedule.FindControl("TextBoxTuesdayInTime_" & index), TextBox)
            TextBoxTuesdayOutTime = DirectCast(PlaceHolderSchedule.FindControl("TextBoxTuesdayOutTime_" & index), TextBox)
            TextBoxWednesdayInTime = DirectCast(PlaceHolderSchedule.FindControl("TextBoxWednesdayInTime_" & index), TextBox)
            TextBoxWednesdayOutTime = DirectCast(PlaceHolderSchedule.FindControl("TextBoxWednesdayOutTime_" & index), TextBox)
            TextBoxThursdayInTime = DirectCast(PlaceHolderSchedule.FindControl("TextBoxThursdayInTime_" & index), TextBox)
            TextBoxThursdayOutTime = DirectCast(PlaceHolderSchedule.FindControl("TextBoxThursdayOutTime_" & index), TextBox)
            TextBoxFridayInTime = DirectCast(PlaceHolderSchedule.FindControl("TextBoxFridayInTime_" & index), TextBox)
            TextBoxFridayOutTime = DirectCast(PlaceHolderSchedule.FindControl("TextBoxFridayOutTime_" & index), TextBox)
            TextBoxSaturdayInTime = DirectCast(PlaceHolderSchedule.FindControl("TextBoxSaturdayInTime_" & index), TextBox)
            TextBoxSaturdayOutTime = DirectCast(PlaceHolderSchedule.FindControl("TextBoxSaturdayOutTime_" & index), TextBox)

            RequiredFieldValidatorSundayInTime = DirectCast(PlaceHolderSchedule.FindControl("RequiredFieldValidatorSundayInTime_" & index), RequiredFieldValidator)
            RequiredFieldValidatorSundayOutTime = DirectCast(PlaceHolderSchedule.FindControl("RequiredFieldValidatorSundayOutTime_" & index), RequiredFieldValidator)

            RequiredFieldValidatorMondayInTime = DirectCast(PlaceHolderSchedule.FindControl("RequiredFieldValidatorMondayInTime_" & index), RequiredFieldValidator)
            RequiredFieldValidatorMondayOutTime = DirectCast(PlaceHolderSchedule.FindControl("RequiredFieldValidatorMondayOutTime_" & index), RequiredFieldValidator)

            RequiredFieldValidatorTuesdayInTime = DirectCast(PlaceHolderSchedule.FindControl("RequiredFieldValidatorTuesdayInTime_" & index), RequiredFieldValidator)
            RequiredFieldValidatorTuesdayOutTime = DirectCast(PlaceHolderSchedule.FindControl("RequiredFieldValidatorTuesdayOutTime_" & index), RequiredFieldValidator)

            RequiredFieldValidatorWednesdayInTime = DirectCast(PlaceHolderSchedule.FindControl("RequiredFieldValidatorWednesdayInTime_" & index), RequiredFieldValidator)
            RequiredFieldValidatorWednesdayOutTime = DirectCast(PlaceHolderSchedule.FindControl("RequiredFieldValidatorWednesdayOutTime_" & index), RequiredFieldValidator)

            RequiredFieldValidatorThursdayInTime = DirectCast(PlaceHolderSchedule.FindControl("RequiredFieldValidatorThursdayInTime_" & index), RequiredFieldValidator)
            RequiredFieldValidatorThursdayOutTime = DirectCast(PlaceHolderSchedule.FindControl("RequiredFieldValidatorThursdayOutTime_" & index), RequiredFieldValidator)

            RequiredFieldValidatorFridayInTime = DirectCast(PlaceHolderSchedule.FindControl("RequiredFieldValidatorFridayInTime_" & index), RequiredFieldValidator)
            RequiredFieldValidatorFridayOutTime = DirectCast(PlaceHolderSchedule.FindControl("RequiredFieldValidatorFridayOutTime_" & index), RequiredFieldValidator)

            RequiredFieldValidatorSaturdayInTime = DirectCast(PlaceHolderSchedule.FindControl("RequiredFieldValidatorSaturdayInTime_" & index), RequiredFieldValidator)
            RequiredFieldValidatorSaturdayOutTime = DirectCast(PlaceHolderSchedule.FindControl("RequiredFieldValidatorSaturdayOutTime_" & index), RequiredFieldValidator)

        End Sub

        ''' <summary>
        ''' Clearing controls value
        ''' </summary>
        Protected Sub ClearScheduleControl()

            TextBoxSundayHourMinute.Text = InlineAssignHelper(TextBoxMondayHourMinute.Text, InlineAssignHelper(TextBoxTuesdayHourMinute.Text,
                                           InlineAssignHelper(TextBoxWednesdayHourMinute.Text, InlineAssignHelper(TextBoxThursdayHourMinute.Text,
                                           InlineAssignHelper(TextBoxFridayHourMinute.Text, InlineAssignHelper(TextBoxSaturdayHourMinute.Text,
                                           InlineAssignHelper(TextBoxTotalHourMinute.Text, String.Empty)))))))

            TextBoxSundayInTime.Text = InlineAssignHelper(TextBoxSundayOutTime.Text, InlineAssignHelper(TextBoxMondayInTime.Text,
                                          InlineAssignHelper(TextBoxMondayOutTime.Text, InlineAssignHelper(TextBoxTuesdayInTime.Text,
                                          InlineAssignHelper(TextBoxTuesdayOutTime.Text, InlineAssignHelper(TextBoxWednesdayInTime.Text,
                                          InlineAssignHelper(TextBoxWednesdayOutTime.Text, InlineAssignHelper(TextBoxThursdayInTime.Text,
                                          InlineAssignHelper(TextBoxThursdayOutTime.Text, InlineAssignHelper(TextBoxFridayInTime.Text,
                                          InlineAssignHelper(TextBoxFridayOutTime.Text, InlineAssignHelper(TextBoxSaturdayInTime.Text,
                                          InlineAssignHelper(TextBoxSaturdayOutTime.Text, String.Empty)))))))))))))

            TextBoxSpecialPayrate.Text = InlineAssignHelper(TextBoxStartDate.Text, InlineAssignHelper(TextBoxEndDate.Text,
                                         InlineAssignHelper(TextBoxScheduleComments.Text, InlineAssignHelper(TextBoxScheduleUpdateDate.Text,
                                         InlineAssignHelper(TextBoxScheduleUpdateBy.Text, String.Empty)))))

        End Sub

        Private Sub LoadEmployeeControlsWithData() Implements IScheduleEmployee.LoadControlsWithData
            LoadControlsWithData()
        End Sub

        Private Sub LoadClientControlsWithData() Implements IScheduleClient.LoadControlsWithData
            LoadControlsWithData()
        End Sub

        Private Sub LoadClientEmployeeControlsWithData() Implements IScheduleClientEmployee.LoadControlsWithData
            LoadControlsWithData()
        End Sub

        Private Sub LoadControlsWithData()

            PlaceHolderSchedule.Controls.Clear()

            RowCount = ScheduleList.Count + 1

            Dim RowIndex As Integer = 1
            For Each Schedule In ScheduleList
                CreateControls(RowIndex)
                hfScheduleId.Value = Schedule.ScheduleId
                LoadSelectedCalendarSetting()
                RowIndex = RowIndex + 1
            Next

            CreateControls(RowIndex)

        End Sub



        Private Sub SetData(ClientId As Integer, EmployeeId As Integer, objCalendarSettingDataObject As CalendarSettingDataObject)

            objCalendarSettingDataObject.ClientId = ClientId
            objCalendarSettingDataObject.EmployeeId = EmployeeId

            objCalendarSettingDataObject.SundayHourMinute = Convert.ToString(TextBoxSundayHourMinute.Text, Nothing).Trim()
            objCalendarSettingDataObject.MondayHourMinute = Convert.ToString(TextBoxMondayHourMinute.Text, Nothing).Trim()
            objCalendarSettingDataObject.TuesdayHourMinute = Convert.ToString(TextBoxTuesdayHourMinute.Text, Nothing).Trim()
            objCalendarSettingDataObject.WednesdayHourMinute = Convert.ToString(TextBoxWednesdayHourMinute.Text, Nothing).Trim()
            objCalendarSettingDataObject.ThursdayHourMinute = Convert.ToString(TextBoxThursdayHourMinute.Text, Nothing).Trim()
            objCalendarSettingDataObject.FridayHourMinute = Convert.ToString(TextBoxFridayHourMinute.Text, Nothing).Trim()
            objCalendarSettingDataObject.SaturdayHourMinute = Convert.ToString(TextBoxSaturdayHourMinute.Text, Nothing).Trim()

            objCalendarSettingDataObject.Status = Convert.ToString(DropDownListScheduleStatus.SelectedItem, Nothing)


            objCalendarSettingDataObject.SpecialRate = If(String.IsNullOrEmpty(Convert.ToString(TextBoxSpecialPayrate.Text, Nothing).Trim()),
                                                          objCalendarSettingDataObject.SpecialRate,
                                                          Convert.ToDecimal(objShared.GetReFormattedPayrate(TextBoxSpecialPayrate.Text.Trim()), Nothing))

            objCalendarSettingDataObject.Comments = Convert.ToString(TextBoxScheduleComments.Text, Nothing).Trim()
            objCalendarSettingDataObject.StartDate = Convert.ToString(TextBoxStartDate.Text, Nothing).Trim()
            objCalendarSettingDataObject.EndDate = Convert.ToString(TextBoxEndDate.Text, Nothing).Trim()

            objCalendarSettingDataObject.SundayInTime = If(String.IsNullOrEmpty(Convert.ToString(TextBoxSundayInTime.Text, Nothing).Trim()),
                                                            objCalendarSettingDataObject.SundayInTime,
                                                            objShared.FormatTime(TextBoxSundayInTime.Text))

            objCalendarSettingDataObject.SundayOutTime = If(String.IsNullOrEmpty(Convert.ToString(TextBoxSundayOutTime.Text, Nothing).Trim()),
                                                            objCalendarSettingDataObject.SundayOutTime,
                                                            objShared.FormatTime(TextBoxSundayOutTime.Text))

            objCalendarSettingDataObject.MondayInTime = If(String.IsNullOrEmpty(Convert.ToString(TextBoxMondayInTime.Text, Nothing).Trim()),
                                                            objCalendarSettingDataObject.MondayInTime,
                                                            objShared.FormatTime(TextBoxMondayInTime.Text))

            objCalendarSettingDataObject.MondayOutTime = If(String.IsNullOrEmpty(Convert.ToString(TextBoxMondayOutTime.Text, Nothing).Trim()),
                                                            objCalendarSettingDataObject.MondayOutTime,
                                                            objShared.FormatTime(TextBoxMondayOutTime.Text))

            objCalendarSettingDataObject.TuesdayInTime = If(String.IsNullOrEmpty(Convert.ToString(TextBoxTuesdayInTime.Text, Nothing).Trim()),
                                                            objCalendarSettingDataObject.TuesdayInTime,
                                                            objShared.FormatTime(TextBoxTuesdayInTime.Text))

            objCalendarSettingDataObject.TuesdayOutTime = If(String.IsNullOrEmpty(Convert.ToString(TextBoxTuesdayOutTime.Text, Nothing).Trim()),
                                                            objCalendarSettingDataObject.TuesdayOutTime,
                                                            objShared.FormatTime(TextBoxTuesdayOutTime.Text))

            objCalendarSettingDataObject.WednesdayInTime = If(String.IsNullOrEmpty(Convert.ToString(TextBoxWednesdayInTime.Text, Nothing).Trim()),
                                                            objCalendarSettingDataObject.WednesdayInTime,
                                                            objShared.FormatTime(TextBoxWednesdayInTime.Text))

            objCalendarSettingDataObject.WednesdayOutTime = If(String.IsNullOrEmpty(Convert.ToString(TextBoxWednesdayOutTime.Text, Nothing).Trim()),
                                                            objCalendarSettingDataObject.WednesdayOutTime,
                                                            objShared.FormatTime(TextBoxWednesdayOutTime.Text))

            objCalendarSettingDataObject.ThursdayInTime = If(String.IsNullOrEmpty(Convert.ToString(TextBoxThursdayInTime.Text, Nothing).Trim()),
                                                            objCalendarSettingDataObject.ThursdayInTime,
                                                            objShared.FormatTime(TextBoxThursdayInTime.Text))

            objCalendarSettingDataObject.ThursdayOutTime = If(String.IsNullOrEmpty(Convert.ToString(TextBoxThursdayOutTime.Text, Nothing).Trim()),
                                                            objCalendarSettingDataObject.ThursdayOutTime,
                                                            objShared.FormatTime(TextBoxThursdayOutTime.Text))

            objCalendarSettingDataObject.FridayInTime = If(String.IsNullOrEmpty(Convert.ToString(TextBoxFridayInTime.Text, Nothing).Trim()),
                                                            objCalendarSettingDataObject.FridayInTime,
                                                            objShared.FormatTime(TextBoxFridayInTime.Text))

            objCalendarSettingDataObject.FridayOutTime = If(String.IsNullOrEmpty(Convert.ToString(TextBoxFridayOutTime.Text, Nothing).Trim()),
                                                            objCalendarSettingDataObject.FridayOutTime,
                                                            objShared.FormatTime(TextBoxFridayOutTime.Text))

            objCalendarSettingDataObject.SaturdayInTime = If(String.IsNullOrEmpty(Convert.ToString(TextBoxSaturdayInTime.Text, Nothing).Trim()),
                                                            objCalendarSettingDataObject.SaturdayInTime,
                                                            objShared.FormatTime(TextBoxSaturdayInTime.Text))

            objCalendarSettingDataObject.SaturdayOutTime = If(String.IsNullOrEmpty(Convert.ToString(TextBoxSaturdayOutTime.Text, Nothing).Trim()),
                                                            objCalendarSettingDataObject.SaturdayOutTime,
                                                            objShared.FormatTime(TextBoxSaturdayOutTime.Text))

        End Sub

        ''' <summary>
        ''' This is for validating data in server side before submitting the form
        ''' </summary>
        ''' <returns></returns>
        Protected Function ValidateScheduleData() As Boolean

            'Return EmptyFieldValidation()

            If (Not InTimeOutTimeValidation(TextBoxSundayInTime, TextBoxSundayOutTime)) Then
                Return False
            End If

            If (Not InTimeOutTimeValidation(TextBoxMondayInTime, TextBoxMondayOutTime)) Then
                Return False
            End If

            If (Not InTimeOutTimeValidation(TextBoxTuesdayInTime, TextBoxTuesdayOutTime)) Then
                Return False
            End If

            If (Not InTimeOutTimeValidation(TextBoxWednesdayInTime, TextBoxWednesdayOutTime)) Then
                Return False
            End If

            If (Not InTimeOutTimeValidation(TextBoxThursdayInTime, TextBoxThursdayOutTime)) Then
                Return False
            End If

            If (Not InTimeOutTimeValidation(TextBoxFridayInTime, TextBoxFridayOutTime)) Then
                Return False
            End If

            If (Not InTimeOutTimeValidation(TextBoxSaturdayInTime, TextBoxSaturdayOutTime)) Then
                Return False
            End If

            If ((Not String.IsNullOrEmpty(TextBoxSpecialPayrate.Text.Trim()))) And (Not objShared.ValidatePayrate(TextBoxSpecialPayrate.Text.Trim())) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorSpecialPayrate.ErrorMessage)
                Return False
            End If

            If ((Not String.IsNullOrEmpty(TextBoxStartDate.Text.Trim())) And (Not objShared.ValidateDate(TextBoxStartDate.Text.Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorStartDate.ErrorMessage)
                Return False
            End If

            If ((Not String.IsNullOrEmpty(TextBoxEndDate.Text.Trim())) And (Not objShared.ValidateDate(TextBoxEndDate.Text.Trim()))) Then
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage(RegularExpressionValidatorEndDate.ErrorMessage)
                Return False
            End If

            Return True

        End Function

        Private Sub LoadEmployeeDynamicJavascript(ByRef Control As Control) Implements IScheduleEmployee.LoadDynamicJavascript
            DynamicJavascript(Control)
        End Sub

        Private Sub LoadClientDynamicJavascript(ByRef Control As Control) Implements IScheduleClient.LoadDynamicJavascript
            DynamicJavascript(Control)
        End Sub

        Private Sub LoadClientEmployeeDynamicJavascript(ByRef Control As Control) Implements IScheduleClientEmployee.LoadDynamicJavascript
            DynamicJavascript(Control)
        End Sub

        Private Sub DynamicJavascript(ByRef Control As Control)
            ScriptManager.RegisterClientScriptBlock(Control, Me.GetType(), "DateFields", "RowCounter = " & RowCount & ";ScheduleDateFieldsEvent();", True)
        End Sub

#Region "Old Code"
        Protected Sub SetRequiredFieldValidatorSetting(index As Integer)

            'ValidationEnable
            RequiredFieldValidatorSundayInTime.Enabled = InlineAssignHelper(RequiredFieldValidatorMondayInTime.Enabled,
                                                                 InlineAssignHelper(RequiredFieldValidatorTuesdayInTime.Enabled,
                                                                 InlineAssignHelper(RequiredFieldValidatorWednesdayInTime.Enabled,
                                                                 InlineAssignHelper(RequiredFieldValidatorThursdayInTime.Enabled,
                                                                 InlineAssignHelper(RequiredFieldValidatorFridayInTime.Enabled,
                                                                 InlineAssignHelper(RequiredFieldValidatorSaturdayInTime.Enabled,
                                                                 False))))))

            RequiredFieldValidatorSundayOutTime.Enabled = InlineAssignHelper(RequiredFieldValidatorMondayOutTime.Enabled,
                                                                  InlineAssignHelper(RequiredFieldValidatorTuesdayOutTime.Enabled,
                                                                  InlineAssignHelper(RequiredFieldValidatorWednesdayOutTime.Enabled,
                                                                  InlineAssignHelper(RequiredFieldValidatorThursdayOutTime.Enabled,
                                                                  InlineAssignHelper(RequiredFieldValidatorFridayOutTime.Enabled,
                                                                  InlineAssignHelper(RequiredFieldValidatorSaturdayOutTime.Enabled,
                                                                  False))))))


            RequiredFieldValidatorSundayInTime.ControlToValidate = "TextBoxSundayInTime_" & index
            RequiredFieldValidatorSundayOutTime.ControlToValidate = "TextBoxSundayOutTime_" & index
            RequiredFieldValidatorMondayInTime.ControlToValidate = "TextBoxMondayInTime_" & index
            RequiredFieldValidatorMondayOutTime.ControlToValidate = "TextBoxMondayOutTime_" & index
            RequiredFieldValidatorTuesdayInTime.ControlToValidate = "TextBoxTuesdayInTime_" & index
            RequiredFieldValidatorTuesdayOutTime.ControlToValidate = "TextBoxTuesdayOutTime_" & index
            RequiredFieldValidatorWednesdayInTime.ControlToValidate = "TextBoxWednesdayInTime_" & index
            RequiredFieldValidatorWednesdayOutTime.ControlToValidate = "TextBoxWednesdayOutTime_" & index
            RequiredFieldValidatorThursdayInTime.ControlToValidate = "TextBoxThursdayInTime_" & index
            RequiredFieldValidatorThursdayOutTime.ControlToValidate = "TextBoxThursdayOutTime_" & index
            RequiredFieldValidatorFridayInTime.ControlToValidate = "TextBoxFridayInTime_" & index
            RequiredFieldValidatorFridayOutTime.ControlToValidate = "TextBoxFridayOutTime_" & index
            RequiredFieldValidatorSaturdayInTime.ControlToValidate = "TextBoxSaturdayInTime_" & index
            RequiredFieldValidatorSaturdayOutTime.ControlToValidate = "TextBoxSaturdayOutTime_" & index


            RequiredFieldValidatorSundayInTime.ValidationGroup = InlineAssignHelper(RequiredFieldValidatorMondayInTime.ValidationGroup,
                                                                 InlineAssignHelper(RequiredFieldValidatorTuesdayInTime.ValidationGroup,
                                                                 InlineAssignHelper(RequiredFieldValidatorWednesdayInTime.ValidationGroup,
                                                                 InlineAssignHelper(RequiredFieldValidatorThursdayInTime.ValidationGroup,
                                                                 InlineAssignHelper(RequiredFieldValidatorFridayInTime.ValidationGroup,
                                                                 InlineAssignHelper(RequiredFieldValidatorSaturdayInTime.ValidationGroup,
                                                                 ScheduleValidationGroup))))))

            RequiredFieldValidatorSundayOutTime.ValidationGroup = InlineAssignHelper(RequiredFieldValidatorMondayOutTime.ValidationGroup,
                                                                  InlineAssignHelper(RequiredFieldValidatorTuesdayOutTime.ValidationGroup,
                                                                  InlineAssignHelper(RequiredFieldValidatorWednesdayOutTime.ValidationGroup,
                                                                  InlineAssignHelper(RequiredFieldValidatorThursdayOutTime.ValidationGroup,
                                                                  InlineAssignHelper(RequiredFieldValidatorFridayOutTime.ValidationGroup,
                                                                  InlineAssignHelper(RequiredFieldValidatorSaturdayOutTime.ValidationGroup,
                                                                  ScheduleValidationGroup))))))

            RequiredFieldValidatorSundayInTime.Display = InlineAssignHelper(RequiredFieldValidatorMondayInTime.Display,
                                                                 InlineAssignHelper(RequiredFieldValidatorTuesdayInTime.Display,
                                                                 InlineAssignHelper(RequiredFieldValidatorWednesdayInTime.Display,
                                                                 InlineAssignHelper(RequiredFieldValidatorThursdayInTime.Display,
                                                                 InlineAssignHelper(RequiredFieldValidatorFridayInTime.Display,
                                                                 InlineAssignHelper(RequiredFieldValidatorSaturdayInTime.Display,
                                                                 ValidatorDisplay.Dynamic))))))

            RequiredFieldValidatorSundayOutTime.Display = InlineAssignHelper(RequiredFieldValidatorMondayOutTime.Display,
                                                                  InlineAssignHelper(RequiredFieldValidatorTuesdayOutTime.Display,
                                                                  InlineAssignHelper(RequiredFieldValidatorWednesdayOutTime.Display,
                                                                  InlineAssignHelper(RequiredFieldValidatorThursdayOutTime.Display,
                                                                  InlineAssignHelper(RequiredFieldValidatorFridayOutTime.Display,
                                                                  InlineAssignHelper(RequiredFieldValidatorSaturdayOutTime.Display,
                                                                  ValidatorDisplay.Dynamic))))))

            RequiredFieldValidatorSundayInTime.ErrorMessage = InlineAssignHelper(RequiredFieldValidatorMondayInTime.ErrorMessage,
                                                                 InlineAssignHelper(RequiredFieldValidatorTuesdayInTime.ErrorMessage,
                                                                 InlineAssignHelper(RequiredFieldValidatorWednesdayInTime.ErrorMessage,
                                                                 InlineAssignHelper(RequiredFieldValidatorThursdayInTime.ErrorMessage,
                                                                 InlineAssignHelper(RequiredFieldValidatorFridayInTime.ErrorMessage,
                                                                 InlineAssignHelper(RequiredFieldValidatorSaturdayInTime.ErrorMessage,
                                                                 "*"))))))

            RequiredFieldValidatorSundayOutTime.ErrorMessage = InlineAssignHelper(RequiredFieldValidatorMondayOutTime.ErrorMessage,
                                                                  InlineAssignHelper(RequiredFieldValidatorTuesdayOutTime.ErrorMessage,
                                                                  InlineAssignHelper(RequiredFieldValidatorWednesdayOutTime.ErrorMessage,
                                                                  InlineAssignHelper(RequiredFieldValidatorThursdayOutTime.ErrorMessage,
                                                                  InlineAssignHelper(RequiredFieldValidatorFridayOutTime.ErrorMessage,
                                                                  InlineAssignHelper(RequiredFieldValidatorSaturdayOutTime.ErrorMessage,
                                                                  "*"))))))

            RequiredFieldValidatorSundayInTime.SetFocusOnError = InlineAssignHelper(RequiredFieldValidatorMondayInTime.SetFocusOnError,
                                                                 InlineAssignHelper(RequiredFieldValidatorTuesdayInTime.SetFocusOnError,
                                                                 InlineAssignHelper(RequiredFieldValidatorWednesdayInTime.SetFocusOnError,
                                                                 InlineAssignHelper(RequiredFieldValidatorThursdayInTime.SetFocusOnError,
                                                                 InlineAssignHelper(RequiredFieldValidatorFridayInTime.SetFocusOnError,
                                                                 InlineAssignHelper(RequiredFieldValidatorSaturdayInTime.SetFocusOnError,
                                                                 True))))))

            RequiredFieldValidatorSundayOutTime.SetFocusOnError = InlineAssignHelper(RequiredFieldValidatorMondayOutTime.SetFocusOnError,
                                                                  InlineAssignHelper(RequiredFieldValidatorTuesdayOutTime.SetFocusOnError,
                                                                  InlineAssignHelper(RequiredFieldValidatorWednesdayOutTime.SetFocusOnError,
                                                                  InlineAssignHelper(RequiredFieldValidatorThursdayOutTime.SetFocusOnError,
                                                                  InlineAssignHelper(RequiredFieldValidatorFridayOutTime.SetFocusOnError,
                                                                  InlineAssignHelper(RequiredFieldValidatorSaturdayOutTime.SetFocusOnError,
                                                                  True))))))


            RequiredFieldValidatorSundayInTime.Text = InlineAssignHelper(RequiredFieldValidatorMondayInTime.Text,
                                                                 InlineAssignHelper(RequiredFieldValidatorTuesdayInTime.Text,
                                                                 InlineAssignHelper(RequiredFieldValidatorWednesdayInTime.Text,
                                                                 InlineAssignHelper(RequiredFieldValidatorThursdayInTime.Text,
                                                                 InlineAssignHelper(RequiredFieldValidatorFridayInTime.Text,
                                                                 InlineAssignHelper(RequiredFieldValidatorSaturdayInTime.Text,
                                                                 "*"))))))

            RequiredFieldValidatorSundayOutTime.Text = InlineAssignHelper(RequiredFieldValidatorMondayOutTime.Text,
                                                                  InlineAssignHelper(RequiredFieldValidatorTuesdayOutTime.Text,
                                                                  InlineAssignHelper(RequiredFieldValidatorWednesdayOutTime.Text,
                                                                  InlineAssignHelper(RequiredFieldValidatorThursdayOutTime.Text,
                                                                  InlineAssignHelper(RequiredFieldValidatorFridayOutTime.Text,
                                                                  InlineAssignHelper(RequiredFieldValidatorSaturdayOutTime.Text,
                                                                  "*"))))))


            RequiredFieldValidatorSundayInTime.ForeColor = InlineAssignHelper(RequiredFieldValidatorMondayInTime.ForeColor,
                                                                 InlineAssignHelper(RequiredFieldValidatorTuesdayInTime.ForeColor,
                                                                 InlineAssignHelper(RequiredFieldValidatorWednesdayInTime.ForeColor,
                                                                 InlineAssignHelper(RequiredFieldValidatorThursdayInTime.ForeColor,
                                                                 InlineAssignHelper(RequiredFieldValidatorFridayInTime.ForeColor,
                                                                 InlineAssignHelper(RequiredFieldValidatorSaturdayInTime.ForeColor,
                                                                 Drawing.Color.Red))))))

            RequiredFieldValidatorSundayOutTime.ForeColor = InlineAssignHelper(RequiredFieldValidatorMondayOutTime.ForeColor,
                                                                  InlineAssignHelper(RequiredFieldValidatorTuesdayOutTime.ForeColor,
                                                                  InlineAssignHelper(RequiredFieldValidatorWednesdayOutTime.ForeColor,
                                                                  InlineAssignHelper(RequiredFieldValidatorThursdayOutTime.ForeColor,
                                                                  InlineAssignHelper(RequiredFieldValidatorFridayOutTime.ForeColor,
                                                                  InlineAssignHelper(RequiredFieldValidatorSaturdayOutTime.ForeColor,
                                                                  Drawing.Color.Red))))))

        End Sub
#End Region

        Private Sub TextBoxSundayInTime_TextChanged(sender As Object, e As EventArgs)

            Dim TextBoxSundayInTime As TextBox = DirectCast(sender, TextBox)
            Dim index As Integer = TextBoxSundayInTime.ID.Split("_")(1)

            ControlReferenceAssign(index)
            CalculateTimeDifference(TextBoxSundayInTime, TextBoxSundayOutTime, TextBoxSundayHourMinute)
            TextBoxSundayOutTime.Focus()

        End Sub

        Private Sub TextBoxSundayOutTime_TextChanged(sender As Object, e As EventArgs)

            Dim TextBoxSundayOutTime As TextBox = DirectCast(sender, TextBox)
            Dim index As Integer = TextBoxSundayOutTime.ID.Split("_")(1)

            ControlReferenceAssign(index)

            CalculateTimeDifference(TextBoxSundayInTime, TextBoxSundayOutTime, TextBoxSundayHourMinute)
            TextBoxMondayInTime.Focus()
        End Sub

        Private Sub TextBoxMondayInTime_TextChanged(sender As Object, e As EventArgs)

            Dim TextBoxMondayInTime As TextBox = DirectCast(sender, TextBox)
            Dim index As Integer = TextBoxMondayInTime.ID.Split("_")(1)

            ControlReferenceAssign(index)

            CalculateTimeDifference(TextBoxMondayInTime, TextBoxMondayOutTime, TextBoxMondayHourMinute)
            TextBoxMondayOutTime.Focus()

        End Sub

        Private Sub TextBoxMondayOutTime_TextChanged(sender As Object, e As EventArgs)

            Dim TextBoxMondayOutTime As TextBox = DirectCast(sender, TextBox)
            Dim index As Integer = TextBoxMondayOutTime.ID.Split("_")(1)

            ControlReferenceAssign(index)

            CalculateTimeDifference(TextBoxMondayInTime, TextBoxMondayOutTime, TextBoxMondayHourMinute)
            TextBoxTuesdayInTime.Focus()

        End Sub

        Private Sub TextBoxTuesdayInTime_TextChanged(sender As Object, e As EventArgs)

            Dim TextBoxTuesdayInTime As TextBox = DirectCast(sender, TextBox)
            Dim index As Integer = TextBoxTuesdayInTime.ID.Split("_")(1)

            ControlReferenceAssign(index)

            CalculateTimeDifference(TextBoxTuesdayInTime, TextBoxTuesdayOutTime, TextBoxTuesdayHourMinute)
            TextBoxTuesdayOutTime.Focus()
        End Sub

        Private Sub TextBoxTuesdayOutTime_TextChanged(sender As Object, e As EventArgs)

            Dim TextBoxTuesdayOutTime As TextBox = DirectCast(sender, TextBox)
            Dim index As Integer = TextBoxTuesdayOutTime.ID.Split("_")(1)

            ControlReferenceAssign(index)

            CalculateTimeDifference(TextBoxTuesdayInTime, TextBoxTuesdayOutTime, TextBoxTuesdayHourMinute)
            TextBoxWednesdayInTime.Focus()
        End Sub

        Private Sub TextBoxWednesdayInTime_TextChanged(sender As Object, e As EventArgs)

            Dim TextBoxWednesdayInTime As TextBox = DirectCast(sender, TextBox)
            Dim index As Integer = TextBoxWednesdayInTime.ID.Split("_")(1)

            ControlReferenceAssign(index)

            CalculateTimeDifference(TextBoxWednesdayInTime, TextBoxWednesdayOutTime, TextBoxWednesdayHourMinute)
            TextBoxWednesdayOutTime.Focus()
        End Sub

        Private Sub TextBoxWednesdayOutTime_TextChanged(sender As Object, e As EventArgs)

            Dim TextBoxWednesdayOutTime As TextBox = DirectCast(sender, TextBox)
            Dim index As Integer = TextBoxWednesdayOutTime.ID.Split("_")(1)

            ControlReferenceAssign(index)

            CalculateTimeDifference(TextBoxWednesdayInTime, TextBoxWednesdayOutTime, TextBoxWednesdayHourMinute)
            TextBoxThursdayInTime.Focus()
        End Sub

        Private Sub TextBoxThursdayInTime_TextChanged(sender As Object, e As EventArgs)

            Dim TextBoxThursdayInTime As TextBox = DirectCast(sender, TextBox)
            Dim index As Integer = TextBoxThursdayInTime.ID.Split("_")(1)

            ControlReferenceAssign(index)

            CalculateTimeDifference(TextBoxThursdayInTime, TextBoxThursdayOutTime, TextBoxThursdayHourMinute)
            TextBoxThursdayOutTime.Focus()
        End Sub

        Private Sub TextBoxThursdayOutTime_TextChanged(sender As Object, e As EventArgs)

            Dim TextBoxThursdayOutTime As TextBox = DirectCast(sender, TextBox)
            Dim index As Integer = TextBoxThursdayOutTime.ID.Split("_")(1)

            ControlReferenceAssign(index)

            CalculateTimeDifference(TextBoxThursdayInTime, TextBoxThursdayOutTime, TextBoxThursdayHourMinute)
            TextBoxFridayInTime.Focus()
        End Sub

        Private Sub TextBoxFridayInTime_TextChanged(sender As Object, e As EventArgs)

            Dim TextBoxFridayInTime As TextBox = DirectCast(sender, TextBox)
            Dim index As Integer = TextBoxFridayInTime.ID.Split("_")(1)

            ControlReferenceAssign(index)

            CalculateTimeDifference(TextBoxFridayInTime, TextBoxFridayOutTime, TextBoxFridayHourMinute)
            TextBoxFridayOutTime.Focus()
        End Sub

        Private Sub TextBoxFridayOutTime_TextChanged(sender As Object, e As EventArgs)

            Dim TextBoxFridayOutTime As TextBox = DirectCast(sender, TextBox)
            Dim index As Integer = TextBoxFridayOutTime.ID.Split("_")(1)

            ControlReferenceAssign(index)

            CalculateTimeDifference(TextBoxFridayInTime, TextBoxFridayOutTime, TextBoxFridayHourMinute)
            TextBoxSaturdayInTime.Focus()
        End Sub

        Private Sub TextBoxSaturdayInTime_TextChanged(sender As Object, e As EventArgs)

            Dim TextBoxSaturdayInTime As TextBox = DirectCast(sender, TextBox)
            Dim index As Integer = TextBoxSaturdayInTime.ID.Split("_")(1)

            ControlReferenceAssign(index)

            CalculateTimeDifference(TextBoxSaturdayInTime, TextBoxSaturdayOutTime, TextBoxSaturdayHourMinute)
            TextBoxSaturdayOutTime.Focus()
        End Sub

        Private Sub TextBoxSaturdayOutTime_TextChanged(sender As Object, e As EventArgs)

            Dim TextBoxSaturdayOutTime As TextBox = DirectCast(sender, TextBox)
            Dim index As Integer = TextBoxSaturdayOutTime.ID.Split("_")(1)

            ControlReferenceAssign(index)

            CalculateTimeDifference(TextBoxSaturdayInTime, TextBoxSaturdayOutTime, TextBoxSaturdayHourMinute)
        End Sub

        Private Sub SaveDataClientEmployee() Implements IScheduleClientEmployee.SaveData

            Page.Validate()
            Page.Validate(ScheduleValidationGroup)

            Dim IsSaved As Boolean = False
            Dim objBLCalendarSetting As New BLCalendarSetting()
            Dim index As Integer = 1

            For Each Schedule In ScheduleList

                ControlReferenceAssign(index)

                If ((Page.IsValid) And (ValidateScheduleData())) Then

                    SetData(Convert.ToInt32(DropDownListClient.SelectedValue), Convert.ToInt32(DropDownListEmployee.SelectedValue), Schedule)
                    IsSaved = False

                    Schedule.ScheduleId = Schedule.ScheduleId
                    Schedule.UpdateBy = Convert.ToString(objShared.UserId)
                    Schedule.CompanyId = objShared.CompanyId

                    Try

                        objBLCalendarSetting.UpdateCalendarInfo(objShared.ConVisitel, Schedule)
                        IsSaved = True

                    Catch ex As SqlException

                        If ex.Message.Contains("Duplicate Schedule") Then
                            DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Duplicate Schedule")
                        Else
                            DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to Update")
                        End If

                    End Try

                    If (IsSaved) Then
                        index = index + 1
                    Else
                        objBLCalendarSetting = Nothing
                        Return
                    End If

                Else
                    IsSaved = False
                    Return
                End If

            Next

            'If (Not IsSaved) Then
            '    Return
            'End If

            Dim objCalendarSettingDataObject As CalendarSettingDataObject
            If ((Page.IsValid) And (ValidateScheduleData())) Then

                ControlReferenceAssign(index)

                If (DropDownListClient.SelectedValue.Equals("-1")) Then

                Else

                    objCalendarSettingDataObject = New CalendarSettingDataObject()

                    SetData(Convert.ToInt32(DropDownListClient.SelectedValue), Convert.ToInt32(DropDownListEmployee.SelectedValue), objCalendarSettingDataObject)

                    IsSaved = False

                    objCalendarSettingDataObject.UserId = objShared.UserId
                    objCalendarSettingDataObject.CompanyId = objShared.CompanyId

                    Try
                        objBLCalendarSetting.InsertCalendarInfo(objShared.ConVisitel, objCalendarSettingDataObject)
                        IsSaved = True

                    Catch ex As SqlException

                        If ex.Message.Contains("Duplicate Schedule") Then
                            DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Duplicate Schedule")
                        Else
                            DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to Insert")
                            'Master.DisplayHeaderMessage(ex.ToString())
                        End If

                    End Try

                    If (IsSaved) Then

                    Else
                        objBLCalendarSetting = Nothing
                        objCalendarSettingDataObject = Nothing
                        Return
                    End If

                End If

            End If

            If (IsSaved) Then

                objBLCalendarSetting = Nothing
                objCalendarSettingDataObject = Nothing
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Saved Successfully")
            Else
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to Save")
            End If

        End Sub

        Private Sub SaveDataClient(ClientId As Integer) Implements IScheduleClient.SaveData

            Page.Validate()
            Page.Validate(ScheduleValidationGroup)

            Dim IsSaved As Boolean = False
            Dim objBLCalendarSetting As New BLCalendarSetting()
            Dim index As Integer = 1

            For Each Schedule In ScheduleList

                ControlReferenceAssign(index)

                If ((Page.IsValid) And (ValidateScheduleData())) Then

                    SetData(ClientId, Convert.ToInt32(DropDownListEmployee.SelectedValue), Schedule)
                    IsSaved = False

                    Schedule.ScheduleId = Schedule.ScheduleId
                    Schedule.UpdateBy = Convert.ToString(objShared.UserId)
                    Schedule.CompanyId = objShared.CompanyId

                    Try

                        objBLCalendarSetting.UpdateCalendarInfo(objShared.ConVisitel, Schedule)
                        IsSaved = True

                    Catch ex As SqlException

                        If ex.Message.Contains("Duplicate Schedule") Then
                            DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Duplicate Schedule")
                        Else
                            DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to Update")
                        End If

                    End Try

                    If (IsSaved) Then
                        index = index + 1
                    Else
                        objBLCalendarSetting = Nothing
                        Return
                    End If

                Else
                    IsSaved = False
                    Return
                End If

            Next

            'If (Not IsSaved) Then
            '    Return
            'End If

            Dim objCalendarSettingDataObject As CalendarSettingDataObject
            If ((Page.IsValid) And (ValidateScheduleData())) Then

                ControlReferenceAssign(index)

                If (DropDownListEmployee.SelectedValue.Equals("-1")) Then

                Else

                    objCalendarSettingDataObject = New CalendarSettingDataObject()

                    SetData(ClientId, Convert.ToInt32(DropDownListEmployee.SelectedValue), objCalendarSettingDataObject)

                    IsSaved = False

                    objCalendarSettingDataObject.UserId = objShared.UserId
                    objCalendarSettingDataObject.CompanyId = objShared.CompanyId

                    Try
                        objBLCalendarSetting.InsertCalendarInfo(objShared.ConVisitel, objCalendarSettingDataObject)
                        IsSaved = True

                    Catch ex As SqlException

                        If ex.Message.Contains("Duplicate Schedule") Then
                            DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Duplicate Schedule")
                        Else
                            DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to Insert")
                            'Master.DisplayHeaderMessage(ex.ToString())
                        End If

                    End Try

                    If (IsSaved) Then

                    Else
                        objBLCalendarSetting = Nothing
                        objCalendarSettingDataObject = Nothing
                        Return
                    End If

                End If

            End If

            If (IsSaved) Then

                objBLCalendarSetting = Nothing
                objCalendarSettingDataObject = Nothing
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Saved Successfully")
            Else
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to Save")
            End If

        End Sub

        Private Sub SaveDataEmployee(EmployeeId As Integer) Implements IScheduleEmployee.SaveData

            Page.Validate()
            Page.Validate(ScheduleValidationGroup)

            Dim objCalendarSettingDataObject As CalendarSettingDataObject

            Dim IsSaved As Boolean = False
            Dim objBLCalendarSetting As New BLCalendarSetting()
            Dim index As Integer = 1

            For Each Schedule In ScheduleList

                ControlReferenceAssign(index)

                If ((Page.IsValid) And (ValidateScheduleData())) Then

                    SetData(Convert.ToInt32(DropDownListClient.SelectedValue), EmployeeId, Schedule)
                    IsSaved = False

                    Schedule.ScheduleId = Schedule.ScheduleId
                    Schedule.UpdateBy = Convert.ToString(objShared.UserId)
                    Schedule.CompanyId = objShared.CompanyId

                    Try

                        objBLCalendarSetting.UpdateCalendarInfo(objShared.ConVisitel, Schedule)
                        IsSaved = True

                    Catch ex As SqlException

                        If ex.Message.Contains("Duplicate Schedule") Then
                            DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Duplicate Schedule")
                        Else
                            DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to Update")
                        End If

                    End Try

                    If (IsSaved) Then
                        index = index + 1
                    Else
                        objBLCalendarSetting = Nothing
                        Return
                    End If

                Else
                    IsSaved = False
                    Return
                End If

            Next

            'If (Not IsSaved) Then
            '    Return
            'End If

            objCalendarSettingDataObject = New CalendarSettingDataObject()
            If ((Page.IsValid) And (ValidateScheduleData())) Then

                ControlReferenceAssign(index)

                If (DropDownListClient.SelectedValue.Equals("-1")) Then

                Else

                    objCalendarSettingDataObject = New CalendarSettingDataObject()

                    SetData(Convert.ToInt32(DropDownListClient.SelectedValue), EmployeeId, objCalendarSettingDataObject)
                    IsSaved = False

                    objCalendarSettingDataObject.UserId = objShared.UserId
                    objCalendarSettingDataObject.CompanyId = objShared.CompanyId

                    Try
                        objBLCalendarSetting.InsertCalendarInfo(objShared.ConVisitel, objCalendarSettingDataObject)
                        IsSaved = True

                    Catch ex As SqlException

                        If ex.Message.Contains("Duplicate Schedule") Then
                            DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Duplicate Schedule")
                        Else
                            DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to Insert")
                            'Master.DisplayHeaderMessage(ex.ToString())
                        End If

                    End Try

                    If (IsSaved) Then

                    Else
                        objBLCalendarSetting = Nothing
                        objCalendarSettingDataObject = Nothing
                        Return
                    End If

                End If

            End If

            If (IsSaved) Then

                objBLCalendarSetting = Nothing
                objCalendarSettingDataObject = Nothing
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Saved Successfully")
            Else
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderMessage("Unable to Save")
            End If

        End Sub

        Private Sub ActiveOnly()
            ScheduleList.Clear()
            ScheduleList = (From p In AllScheduleList Where p.Status = [Enum].GetName(GetType(EnumDataObject.CalendarScheduleStatus),
                           EnumDataObject.CalendarScheduleStatus.A)).ToList()

            RowCount = ScheduleList.Count + 1
            LoadControlsWithData()
        End Sub

        Private Sub InActiveOnly()
            ScheduleList.Clear()
            ScheduleList = (From p In AllScheduleList Where p.Status = [Enum].GetName(GetType(EnumDataObject.CalendarScheduleStatus),
                           EnumDataObject.CalendarScheduleStatus.I)).ToList()

            RowCount = ScheduleList.Count + 1
            LoadControlsWithData()
        End Sub

        Private Sub GetEmployeeActiveScheduleOnly() Implements IScheduleEmployee.GetActiveOnly
            ActiveOnly()
        End Sub

        Private Sub GetEmployeeInActiveScheduleOnly() Implements IScheduleEmployee.GetInActiveOnly
            InActiveOnly()
        End Sub

        Private Sub GetEmployeeAllSchedules(EmployeeId As Integer) Implements IScheduleEmployee.GetAll

            AllScheduleList.Clear()
            ScheduleList.Clear()

            AllScheduleList = GetCalendarScheduleInfo(-1, EmployeeId, -1)

            RowCount = AllScheduleList.Count + 1
            For Each Schedule In AllScheduleList
                ScheduleList.Add(Schedule)
            Next

            LoadControlsWithData()

        End Sub

        Private Sub GetClientActiveScheduleOnly() Implements IScheduleClient.GetActiveOnly
            ActiveOnly()
        End Sub

        Private Sub GetClientInActiveScheduleOnly() Implements IScheduleClient.GetInActiveOnly
            InActiveOnly()
        End Sub

        Private Sub GetClientAllSchedules(ClientId As Integer) Implements IScheduleClient.GetAll

            AllScheduleList.Clear()
            ScheduleList.Clear()

            AllScheduleList = GetCalendarScheduleInfo(ClientId, -1, -1)

            RowCount = AllScheduleList.Count + 1
            For Each Schedule In AllScheduleList
                ScheduleList.Add(Schedule)
            Next

            LoadControlsWithData()

        End Sub

        Private Sub InitializeDynamicControl(index As Integer)

            DropDownListClient.ClientIDMode = ClientIDMode.Static
            DropDownListEmployee.ClientIDMode = ClientIDMode.Static

            TextBoxSundayHourMinute.ClientIDMode = InlineAssignHelper(TextBoxMondayHourMinute.ClientIDMode, InlineAssignHelper(TextBoxTuesdayHourMinute.ClientIDMode,
                                                   InlineAssignHelper(TextBoxWednesdayHourMinute.ClientIDMode, InlineAssignHelper(TextBoxThursdayHourMinute.ClientIDMode,
                                                   InlineAssignHelper(TextBoxFridayHourMinute.ClientIDMode, InlineAssignHelper(TextBoxSaturdayHourMinute.ClientIDMode,
                                                   InlineAssignHelper(TextBoxTotalHourMinute.ClientIDMode, ClientIDMode.Static)))))))

            TextBoxSundayInTime.ClientIDMode = InlineAssignHelper(TextBoxMondayInTime.ClientIDMode, InlineAssignHelper(TextBoxTuesdayInTime.ClientIDMode,
                                                       InlineAssignHelper(TextBoxWednesdayInTime.ClientIDMode, InlineAssignHelper(TextBoxThursdayInTime.ClientIDMode,
                                                       InlineAssignHelper(TextBoxFridayInTime.ClientIDMode, InlineAssignHelper(TextBoxSaturdayInTime.ClientIDMode,
                                                       ClientIDMode.Static))))))

            TextBoxSundayOutTime.ClientIDMode = InlineAssignHelper(TextBoxMondayOutTime.ClientIDMode, InlineAssignHelper(TextBoxTuesdayOutTime.ClientIDMode,
                                                       InlineAssignHelper(TextBoxWednesdayOutTime.ClientIDMode, InlineAssignHelper(TextBoxThursdayOutTime.ClientIDMode,
                                                       InlineAssignHelper(TextBoxFridayOutTime.ClientIDMode, InlineAssignHelper(TextBoxSaturdayOutTime.ClientIDMode,
                                                       ClientIDMode.Static))))))

            TextBoxStartDate.ClientIDMode = ClientIDMode.Static
            TextBoxEndDate.ClientIDMode = ClientIDMode.Static

            TextBoxScheduleUpdateDate.ReadOnly = True
            TextBoxScheduleUpdateBy.ReadOnly = True

            TextBoxSundayHourMinute.ReadOnly = InlineAssignHelper(TextBoxMondayHourMinute.ReadOnly, InlineAssignHelper(TextBoxTuesdayHourMinute.ReadOnly,
                                               InlineAssignHelper(TextBoxWednesdayHourMinute.ReadOnly, InlineAssignHelper(TextBoxThursdayHourMinute.ReadOnly,
                                               InlineAssignHelper(TextBoxFridayHourMinute.ReadOnly, InlineAssignHelper(TextBoxSaturdayHourMinute.ReadOnly, True))))))

            TextBoxTotalHourMinute.ReadOnly = True

            SetShadowTextIndicator()
            SetRequiredFieldValidatorSetting(index)
            SetRegularExpressionSetting(index)
            SetJavascriptEvent()
            SetSchduleControlTextLength()

            'TextBoxSundayInTime.AutoPostBack = True
            'AddHandler TextBoxSundayInTime.TextChanged, AddressOf TextBoxSundayInTime_TextChanged

            TextBoxSundayOutTime.AutoPostBack = True
            AddHandler TextBoxSundayOutTime.TextChanged, AddressOf TextBoxSundayOutTime_TextChanged

            'TextBoxMondayInTime.AutoPostBack = True
            'AddHandler TextBoxMondayInTime.TextChanged, AddressOf TextBoxMondayInTime_TextChanged

            TextBoxMondayOutTime.AutoPostBack = True
            AddHandler TextBoxMondayOutTime.TextChanged, AddressOf TextBoxMondayOutTime_TextChanged

            'TextBoxTuesdayInTime.AutoPostBack = True
            'AddHandler TextBoxTuesdayInTime.TextChanged, AddressOf TextBoxTuesdayInTime_TextChanged

            TextBoxTuesdayOutTime.AutoPostBack = True
            AddHandler TextBoxTuesdayOutTime.TextChanged, AddressOf TextBoxTuesdayOutTime_TextChanged

            'TextBoxWednesdayInTime.AutoPostBack = True
            'AddHandler TextBoxWednesdayInTime.TextChanged, AddressOf TextBoxWednesdayInTime_TextChanged

            TextBoxWednesdayOutTime.AutoPostBack = True
            AddHandler TextBoxWednesdayOutTime.TextChanged, AddressOf TextBoxWednesdayOutTime_TextChanged

            'TextBoxThursdayInTime.AutoPostBack = True
            'AddHandler TextBoxThursdayInTime.TextChanged, AddressOf TextBoxThursdayInTime_TextChanged

            TextBoxThursdayOutTime.AutoPostBack = True
            AddHandler TextBoxThursdayOutTime.TextChanged, AddressOf TextBoxThursdayOutTime_TextChanged

            'TextBoxFridayInTime.AutoPostBack = True
            'AddHandler TextBoxFridayInTime.TextChanged, AddressOf TextBoxFridayInTime_TextChanged

            TextBoxFridayOutTime.AutoPostBack = True
            AddHandler TextBoxFridayOutTime.TextChanged, AddressOf TextBoxFridayOutTime_TextChanged

            'TextBoxSaturdayInTime.AutoPostBack = True
            'AddHandler TextBoxSaturdayInTime.TextChanged, AddressOf TextBoxSaturdayInTime_TextChanged

            TextBoxSaturdayOutTime.AutoPostBack = True
            AddHandler TextBoxSaturdayOutTime.TextChanged, AddressOf TextBoxSaturdayOutTime_TextChanged

        End Sub

        Private Sub SetRegularExpressionSetting(index As Integer)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorSpecialPayrate, "TextBoxSpecialPayrate_" & index, objShared.DecimalValueWithDollarSign, "Invalid Payrate",
                                                    "Invalid Payrate", ValidationEnable, ScheduleValidationGroup)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorStartDate, "TextBoxStartDate_" & index, objShared.DateValidationExpression, "Invalid Start Date",
                                                 "Invalid Date", ValidationEnable, ScheduleValidationGroup)

            objShared.SetRegularExpressionValidatorSetting(RegularExpressionValidatorEndDate, "TextBoxEndDate_" & index, objShared.DateValidationExpression, "Invalid End Date", "Invalid Date",
                                                    ValidationEnable, ScheduleValidationGroup)

        End Sub

        Private Sub CalculateTimeDifference(ByRef TextBoxInTime As TextBox, ByRef TextBoxOutTime As TextBox, ByRef TextBoxHourMinute As TextBox)

            TextBoxHourMinute.Text = String.Empty

            If (Not String.IsNullOrEmpty(Convert.ToString(TextBoxInTime.Text, Nothing).Trim()) _
                    And Not String.IsNullOrEmpty(Convert.ToString(TextBoxOutTime.Text, Nothing).Trim())) Then

                Dim InTime As String = String.Empty, OutTime As String = String.Empty
                Dim InTimeHour As String = String.Empty, InTimeMinute As String = String.Empty
                Dim OutTimeHour As String = String.Empty, OutTimeMinute As String = String.Empty

                InTime = TextBoxInTime.Text.Trim().ToUpper()
                OutTime = TextBoxOutTime.Text.Trim().ToUpper()

                InTimeHour = InTime.Split(" ")(0).Split(":")(0)
                InTimeMinute = InTime.Split(" ")(0).Split(":")(1)

                OutTimeHour = OutTime.Split(" ")(0).Split(":")(0)
                OutTimeMinute = OutTime.Split(" ")(0).Split(":")(1)

                Dim InTimePMCheck As Boolean = InTime.Split(" ")(1).Equals("PM")
                Dim OutTimePMCheck As Boolean = OutTime.Split(" ")(1).Equals("PM")
                Dim InTimeAMCheck As Boolean = InTime.Split(" ")(1).Equals("AM")
                Dim OutTimeAMCheck As Boolean = OutTime.Split(" ")(1).Equals("AM")

                If (Not InTimeAMCheck And Not InTimePMCheck) Then
                    CalculateTotalHourMinute()
                    Return
                End If

                If (Not OutTimeAMCheck And Not OutTimePMCheck) Then
                    CalculateTotalHourMinute()
                    Return
                End If

                If (InTimePMCheck) Then
                    InTimeHour = Convert.ToString(Convert.ToInt32(InTimeHour) + 12)
                End If

                If (OutTimePMCheck) Then
                    OutTimeHour = Convert.ToString(Convert.ToInt32(OutTimeHour) + 12)
                End If

                If (((InTimePMCheck And OutTimePMCheck) Or (InTimeAMCheck And OutTimeAMCheck)) _
                        And (New TimeSpan(InTimeHour, InTimeMinute, 0) > New TimeSpan(OutTimeHour, OutTimeMinute, 0))) Then

                    objShared.ShowClientMessage(TextBoxInTime.ClientID & " cannot be greater than " & TextBoxOutTime.ClientID)

                    CalculateTotalHourMinute()
                    Return
                End If

                Dim Difference As TimeSpan = New TimeSpan(OutTimeHour, OutTimeMinute, 0) - New TimeSpan(InTimeHour, InTimeMinute, 0)

                TextBoxHourMinute.Text = objShared.FormatTime(Convert.ToString(Difference, Nothing))

                CalculateTotalHourMinute()

            End If

        End Sub

        ' ''' <summary>
        ' ''' This is for retrieving data
        ' ''' </summary>
        Private Sub BindDynamicDropDownList()

            objShared.BindClientDropDownList(DropDownListClient, objShared.CompanyId, EnumDataObject.ClientListFor.Individual)
            objShared.BindEmployeeDropDownList(DropDownListEmployee, objShared.CompanyId, False)
            BindScheduleStatusDropDownList(DropDownListScheduleStatus)

        End Sub

        Private Sub GetDynamicControlCaptionFromResource()

            Dim ResourceTable As Hashtable = objShared.GetResourceValue("Setup", "CalendarSetting" & Convert.ToString(".resx"))

            LabelDayCaptionFirstColumn.Text = Convert.ToString(ResourceTable("LabelDayCaptionFirstColumn"), Nothing).Trim()
            LabelDayCaptionFirstColumn.Text = If(String.IsNullOrEmpty(LabelDayCaptionFirstColumn.Text), "DAY", LabelDayCaptionFirstColumn.Text)

            LabelInCaptionFirstColumn.Text = Convert.ToString(ResourceTable("LabelInCaptionFirstColumn"), Nothing).Trim()
            LabelInCaptionFirstColumn.Text = If(String.IsNullOrEmpty(LabelInCaptionFirstColumn.Text), "IN", LabelInCaptionFirstColumn.Text)

            LabelOutCaptionFirstColumn.Text = Convert.ToString(ResourceTable("LabelOutCaptionFirstColumn"), Nothing).Trim()
            LabelOutCaptionFirstColumn.Text = If(String.IsNullOrEmpty(LabelOutCaptionFirstColumn.Text), "OUT", LabelOutCaptionFirstColumn.Text)

            LabelSundayInOutCaption.Text = Convert.ToString(ResourceTable("LabelSundayInOutCaption"), Nothing).Trim()
            LabelSundayInOutCaption.Text = If(String.IsNullOrEmpty(LabelSundayInOutCaption.Text), "Sunday", LabelSundayInOutCaption.Text)

            LabelMondayInOutCaption.Text = Convert.ToString(ResourceTable("LabelMondayInOutCaption"), Nothing).Trim()
            LabelMondayInOutCaption.Text = If(String.IsNullOrEmpty(LabelMondayInOutCaption.Text), "Monday", LabelMondayInOutCaption.Text)

            LabelTuesdayInOutCaption.Text = Convert.ToString(ResourceTable("LabelTuesdayInOutCaption"), Nothing).Trim()
            LabelTuesdayInOutCaption.Text = If(String.IsNullOrEmpty(LabelTuesdayInOutCaption.Text), "Tuesday", LabelTuesdayInOutCaption.Text)

            LabelWednesdayInOutCaption.Text = Convert.ToString(ResourceTable("LabelWednesdayInOutCaption"), Nothing).Trim()
            LabelWednesdayInOutCaption.Text = If(String.IsNullOrEmpty(LabelWednesdayInOutCaption.Text), "Wednesday", LabelWednesdayInOutCaption.Text)

            LabelDayCaptionSecondColumn.Text = Convert.ToString(ResourceTable("LabelDayCaptionSecondColumn"), Nothing).Trim()
            LabelDayCaptionSecondColumn.Text = If(String.IsNullOrEmpty(LabelDayCaptionSecondColumn.Text), "DAY", LabelDayCaptionSecondColumn.Text)

            LabelInCaptionSecondColumn.Text = Convert.ToString(ResourceTable("LabelInCaptionSecondColumn"), Nothing).Trim()
            LabelInCaptionSecondColumn.Text = If(String.IsNullOrEmpty(LabelInCaptionSecondColumn.Text), "IN", LabelInCaptionSecondColumn.Text)

            LabelOutCaptionSecondColumn.Text = Convert.ToString(ResourceTable("LabelOutCaptionSecondColumn"), Nothing).Trim()
            LabelOutCaptionSecondColumn.Text = If(String.IsNullOrEmpty(LabelOutCaptionSecondColumn.Text), "OUT", LabelOutCaptionSecondColumn.Text)

            LabelThursdayInOutCaption.Text = Convert.ToString(ResourceTable("LabelThursdayInOutCaption"), Nothing).Trim()
            LabelThursdayInOutCaption.Text = If(String.IsNullOrEmpty(LabelThursdayInOutCaption.Text), "Thursday", LabelThursdayInOutCaption.Text)

            LabelFridayInOutCaption.Text = Convert.ToString(ResourceTable("LabelFridayInOutCaption"), Nothing).Trim()
            LabelFridayInOutCaption.Text = If(String.IsNullOrEmpty(LabelFridayInOutCaption.Text), "Friday", LabelFridayInOutCaption.Text)

            LabelSaturdayInOutCaption.Text = Convert.ToString(ResourceTable("LabelSaturdayInOutCaption"), Nothing).Trim()
            LabelSaturdayInOutCaption.Text = If(String.IsNullOrEmpty(LabelSaturdayInOutCaption.Text), "Saturday", LabelSaturdayInOutCaption.Text)

            LabelSpecialPayrate.Text = Convert.ToString(ResourceTable("LabelSpecialPayrate"), Nothing).Trim()
            LabelSpecialPayrate.Text = If(String.IsNullOrEmpty(LabelSpecialPayrate.Text), "Special Payrate", LabelSpecialPayrate.Text)

            LabelStartDate.Text = Convert.ToString(ResourceTable("LabelStartDate"), Nothing).Trim()
            LabelStartDate.Text = If(String.IsNullOrEmpty(LabelStartDate.Text), "Start:", LabelStartDate.Text)

            LabelEndDate.Text = Convert.ToString(ResourceTable("LabelEndDate"), Nothing).Trim()
            LabelEndDate.Text = If(String.IsNullOrEmpty(LabelEndDate.Text), "End:", LabelEndDate.Text)

            HourMinuteFormat = Convert.ToString(ResourceTable("HourMinuteFormat"), Nothing).Trim()
            HourMinuteFormat = If(String.IsNullOrEmpty(HourMinuteFormat), "hh:mm", HourMinuteFormat)

            TimeFormat = Convert.ToString(ResourceTable("TimeFormat"), Nothing).Trim()
            TimeFormat = If(String.IsNullOrEmpty(TimeFormat), "hh:mm am|pm", TimeFormat)

            ResourceTable = Nothing

        End Sub

        Private Function InTimeOutTimeValidation(ByRef TextBoxInTime As TextBox, ByRef TextBoxOutTime As TextBox) As Boolean

            If (Not String.IsNullOrEmpty(Convert.ToString(TextBoxInTime.Text, Nothing).Trim()) _
                    And Not String.IsNullOrEmpty(Convert.ToString(TextBoxOutTime.Text, Nothing).Trim())) Then

                Dim InTime As String = String.Empty, OutTime As String = String.Empty
                Dim InTimeHour As String = String.Empty, InTimeMinute As String = String.Empty
                Dim OutTimeHour As String = String.Empty, OutTimeMinute As String = String.Empty

                InTime = TextBoxInTime.Text.Trim().ToUpper()
                OutTime = TextBoxOutTime.Text.Trim().ToUpper()

                InTimeHour = InTime.Split(" ")(0).Split(":")(0)
                InTimeMinute = InTime.Split(" ")(0).Split(":")(1)

                OutTimeHour = OutTime.Split(" ")(0).Split(":")(0)
                OutTimeMinute = OutTime.Split(" ")(0).Split(":")(1)

                Dim InTimePMCheck As Boolean = InTime.Split(" ")(1).Equals("PM")
                Dim OutTimePMCheck As Boolean = OutTime.Split(" ")(1).Equals("PM")
                Dim InTimeAMCheck As Boolean = InTime.Split(" ")(1).Equals("AM")
                Dim OutTimeAMCheck As Boolean = OutTime.Split(" ")(1).Equals("AM")

                If (Not InTimeAMCheck And Not InTimePMCheck) Then
                    CalculateTotalHourMinute()
                    Return False
                End If

                If (Not OutTimeAMCheck And Not OutTimePMCheck) Then
                    CalculateTotalHourMinute()
                    Return False
                End If

                If (InTimePMCheck) Then
                    InTimeHour = Convert.ToString(Convert.ToInt32(InTimeHour) + 12)
                End If

                If (OutTimePMCheck) Then
                    OutTimeHour = Convert.ToString(Convert.ToInt32(OutTimeHour) + 12)
                End If

                If (((InTimePMCheck And OutTimePMCheck) Or (InTimeAMCheck And OutTimeAMCheck)) _
                        And (New TimeSpan(InTimeHour, InTimeMinute, 0) > New TimeSpan(OutTimeHour, OutTimeMinute, 0))) Then

                    'Master.DisplayHeaderMessage(TextBoxInTime.ClientID & " cannot be greater than " & TextBoxOutTime.ClientID)

                    CalculateTotalHourMinute()
                    Return False
                End If

            End If

            Return True

        End Function

        Private Sub CalculateTotalHourMinute()

            Dim TotalHour As Int32 = 0
            Dim TotalMinute As Int32 = 0

            TotalHour = (If((String.IsNullOrEmpty(Convert.ToString(TextBoxSundayHourMinute.Text, Nothing).Trim())), 0,
                           Convert.ToInt32(Convert.ToString(TextBoxSundayHourMinute.Text.Split(":")(0))))) +
                        (If((String.IsNullOrEmpty(Convert.ToString(TextBoxMondayHourMinute.Text, Nothing).Trim())), 0,
                            Convert.ToInt32(TextBoxMondayHourMinute.Text.Split(":")(0)))) +
                        (If((String.IsNullOrEmpty(Convert.ToString(TextBoxTuesdayHourMinute.Text, Nothing).Trim())), 0,
                            Convert.ToInt32(TextBoxTuesdayHourMinute.Text.Split(":")(0)))) +
                        (If((String.IsNullOrEmpty(Convert.ToString(TextBoxWednesdayHourMinute.Text, Nothing).Trim())), 0,
                            Convert.ToInt32(TextBoxWednesdayHourMinute.Text.Split(":")(0)))) +
                        (If((String.IsNullOrEmpty(Convert.ToString(TextBoxThursdayHourMinute.Text, Nothing).Trim())), 0,
                            Convert.ToInt32(TextBoxThursdayHourMinute.Text.Split(":")(0)))) +
                        (If((String.IsNullOrEmpty(Convert.ToString(TextBoxFridayHourMinute.Text, Nothing).Trim())), 0,
                            Convert.ToInt32(TextBoxFridayHourMinute.Text.Split(":")(0)))) +
                        (If((String.IsNullOrEmpty(Convert.ToString(TextBoxSaturdayHourMinute.Text, Nothing).Trim())), 0,
                            Convert.ToInt32(TextBoxSaturdayHourMinute.Text.Split(":")(0))))

            TotalMinute = (If((String.IsNullOrEmpty(Convert.ToString(TextBoxSundayHourMinute.Text, Nothing).Trim())), 0,
                           Convert.ToInt32(TextBoxSundayHourMinute.Text.Split(":")(1)))) +
                        (If((String.IsNullOrEmpty(Convert.ToString(TextBoxMondayHourMinute.Text, Nothing).Trim())), 0,
                            Convert.ToInt32(TextBoxMondayHourMinute.Text.Split(":")(1)))) +
                        (If((String.IsNullOrEmpty(Convert.ToString(TextBoxTuesdayHourMinute.Text, Nothing).Trim())), 0,
                            Convert.ToInt32(TextBoxTuesdayHourMinute.Text.Split(":")(1)))) +
                        (If((String.IsNullOrEmpty(Convert.ToString(TextBoxWednesdayHourMinute.Text, Nothing).Trim())), 0,
                            Convert.ToInt32(TextBoxWednesdayHourMinute.Text.Split(":")(1)))) +
                        (If((String.IsNullOrEmpty(Convert.ToString(TextBoxThursdayHourMinute.Text, Nothing).Trim())), 0,
                            Convert.ToInt32(TextBoxThursdayHourMinute.Text.Split(":")(1)))) +
                        (If((String.IsNullOrEmpty(Convert.ToString(TextBoxFridayHourMinute.Text, Nothing).Trim())), 0,
                            Convert.ToInt32(TextBoxFridayHourMinute.Text.Split(":")(1)))) +
                        (If((String.IsNullOrEmpty(Convert.ToString(TextBoxSaturdayHourMinute.Text, Nothing).Trim())), 0,
                            Convert.ToInt32(TextBoxSaturdayHourMinute.Text.Split(":")(1))))

            If TotalMinute >= 60 Then
                TotalHour += TotalMinute / 60
            End If

            TotalMinute = TotalMinute Mod 60

            TextBoxTotalHourMinute.Text = If((TotalHour.Equals(0) And TotalMinute.Equals(0)), String.Empty, TotalHour & ":" & TotalMinute)

        End Sub

    End Class

End Namespace

