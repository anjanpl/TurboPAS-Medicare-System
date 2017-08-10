Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports System.Data.SqlClient
Imports VisitelCommon.VisitelCommon.DataObject
Imports VisitelBusiness.VisitelBusiness.DataObject.Settings

Namespace Visitel.UserControl.EmployeeInfo

    Public Class EmployeeCalendarControl
        Inherits CalendarSchedule

        Private ControlName As String, SocialSecurityNumber As String
        Private objCalendarSchedule As CalendarSchedule
        Private objScheduleControlDataObject As ScheduleControlDataObject
        Private EmployeeId As Int64

        Protected Sub Page_Init(sender As Object, e As EventArgs)

            ControlName = "EmployeeCalendarControl"
            objShared = New SharedWebControls()
            objCalendarSchedule = New CalendarSchedule()
            objShared.ConnectionOpen()
            InitializeControl()

            DirectCast(Me, IScheduleEmployee).GetScheduleCaptionFromResource()

            If Not IsPostBack Then
                objCalendarSchedule.ScheduleList = New List(Of CalendarSettingDataObject)()
                objCalendarSchedule.AllScheduleList = New List(Of CalendarSettingDataObject)()
            End If

            DirectCast(Me, IScheduleEmployee).LoadControlsWithData()

        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)
            If Not IsPostBack Then
                GetData()
            End If
        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)

            LoadCss("Settings/" & "CalendarSetting")

            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                               & " var CalendarImagePath='" & objShared.GetCalendarImagePath & "'; " _
                               & " jQuery(document).ready(function () {" _
                               & "     DateFieldsEvent();" _
                               & "     Sys.WebForms.PageRequestManager.getInstance().add_endRequest(DateFieldsEvent); " _
                               & "}); " _
                        & "</script>"

            Page.ClientScript.RegisterStartupScript(Me.[GetType](), ControlName & "LocalVariable", scriptBlock)

            DirectCast(Me, IScheduleEmployee).LoadScheduleJavaScript()
            DirectCast(Me, IScheduleEmployee).LoadDynamicJavascript(Page)

            objShared.TabSelection(2)

        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objCalendarSchedule = Nothing
            objScheduleControlDataObject = Nothing
        End Sub

        Private Sub ButtonClear_Click(sender As Object, e As EventArgs)
            ClearControl()
        End Sub

        Private Sub ButtonSave_Click(sender As Object, e As EventArgs)
            SaveData(False)
        End Sub

        Private Sub ButtonClearSchedule_Click(sender As Object, e As EventArgs)

            AllScheduleList.Clear()
            ScheduleList.Clear()
            DirectCast(Me, IScheduleEmployee).LoadControlsWithData()

            HiddenFieldScheduleId.Value = Convert.ToString(Int32.MinValue)

        End Sub

        Private Sub ButtonIndividualDetail_Click(sender As Object, e As EventArgs)
            Response.Redirect("~/Pages/ClientInfo.aspx?ClientId=" & HiddenFieldClientDetailId.Value)
        End Sub

        Private Sub ButtonSaveSchedule_Click(sender As Object, e As EventArgs)
            DirectCast(Me, IScheduleEmployee).SaveData(EmployeeId)
        End Sub

        Private Sub ButtonActiveOnly_Click(sender As Object, e As EventArgs)
            DirectCast(Me, IScheduleEmployee).GetActiveOnly()
        End Sub

        Private Sub ButtonInActiveOnly_Click(sender As Object, e As EventArgs)
            DirectCast(Me, IScheduleEmployee).GetInActiveOnly()
        End Sub

        Private Sub ButtonAll_Click(sender As Object, e As EventArgs)
            DirectCast(Me, IScheduleEmployee).GetAll(EmployeeId)
        End Sub

        Private Sub InitializeControl()

            ButtonSaveSchedule.CausesValidation = True
            ButtonSaveSchedule.ValidationGroup = objCalendarSchedule.ScheduleValidationGroup

            'Dim ImageButtonEditSetting As ImageButton = DirectCast(UserControlEditSettingEmployeeInfo.FindControl("ImageButtonEditSetting"), ImageButton)
            'ImageButtonEditSetting.CausesValidation = False

            ButtonActiveOnly.CausesValidation = False
            ButtonInActiveOnly.CausesValidation = False
            ButtonAll.CausesValidation = False
            'ButtonClear.CausesValidation = False

            ButtonActiveOnly.ClientIDMode = ClientIDMode.Static
            ButtonInActiveOnly.ClientIDMode = ClientIDMode.Static
            ButtonAll.ClientIDMode = ClientIDMode.Static
            ButtonClearSchedule.ClientIDMode = ClientIDMode.Static
            ButtonSaveSchedule.ClientIDMode = ClientIDMode.Static


            AddHandler ButtonActiveOnly.Click, AddressOf ButtonActiveOnly_Click
            AddHandler ButtonInActiveOnly.Click, AddressOf ButtonInActiveOnly_Click

            AddHandler ButtonAll.Click, AddressOf ButtonAll_Click

            AddHandler ButtonSaveSchedule.Click, AddressOf ButtonSaveSchedule_Click
            AddHandler ButtonIndividualDetail.Click, AddressOf ButtonIndividualDetail_Click
            AddHandler ButtonClearSchedule.Click, AddressOf ButtonClearSchedule_Click

            objScheduleControlDataObject = New ScheduleControlDataObject()

            objScheduleControlDataObject.UserId = objShared.UserId
            objScheduleControlDataObject.CompanyId = objShared.CompanyId

            objScheduleControlDataObject.HiddenFieldScheduleId = HiddenFieldScheduleId
            objScheduleControlDataObject.PlaceHolderCalendarSetting = PlaceHolderCalendarSetting

            objScheduleControlDataObject.DropDownListSearchByClient = Nothing
            objScheduleControlDataObject.DropDownListSearchByScheduleStatus = Nothing

            objScheduleControlDataObject.IsClientVisible = True
            objScheduleControlDataObject.IsEmployeeVisible = False

            objScheduleControlDataObject.LabelWeeklyCalendar = LabelWeeklyCalendar

            objScheduleControlDataObject.ButtonActiveOnly = ButtonActiveOnly
            objScheduleControlDataObject.ButtonInActiveOnly = ButtonInActiveOnly
            objScheduleControlDataObject.ButtonAll = ButtonAll
            objScheduleControlDataObject.LabelHoursMinutesCaption = LabelHoursMinutesCaption
            objScheduleControlDataObject.LabelIndividualCaption = LabelIndividualCaption

            objScheduleControlDataObject.LabelSunday = LabelSunday
            objScheduleControlDataObject.LabelMonday = LabelMonday
            objScheduleControlDataObject.LabelTuesday = LabelTuesday
            objScheduleControlDataObject.LabelWednesday = LabelWednesday
            objScheduleControlDataObject.LabelThursday = LabelThursday
            objScheduleControlDataObject.LabelFriday = LabelFriday
            objScheduleControlDataObject.LabelSaturday = LabelSaturday

            objScheduleControlDataObject.LabelTotal = LabelTotal
            objScheduleControlDataObject.LabelStatus = LabelStatus
            objScheduleControlDataObject.LabelUpdateDate = LabelScheduleUpdateDate
            objScheduleControlDataObject.LabelUpdateBy = LabelScheduleUpdateBy
            objScheduleControlDataObject.LabelDetailInfo = LabelDetailInfo

            objScheduleControlDataObject.ButtonSaveSchedule = ButtonSaveSchedule
            objScheduleControlDataObject.ButtonIndividualDetail = ButtonIndividualDetail
            objScheduleControlDataObject.ButtonClearSchedule = ButtonClearSchedule

            'objCalendarSchedule.SetControl(objScheduleControlDataObject)

            DirectCast(Me, IScheduleEmployee).SetControl(objScheduleControlDataObject)

        End Sub

        Public Sub SetEmployeeId(EmployeeId As Integer)
            Me.EmployeeId = EmployeeId
        End Sub

        Public Sub SetSocialSecurityNumber(SocialSecurityNumber As String)
            Me.SocialSecurityNumber = SocialSecurityNumber
        End Sub

        Private Sub SaveData(IsConfirm As Boolean)

        End Sub

        Private Sub ClearControl()

        End Sub

        Private Sub GetData()

            Try
                DirectCast(Me, IScheduleEmployee).GetAll(Me.EmployeeId)
            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError("Unable to fetch data.")
            End Try

        End Sub

        Public Sub LoadWeeklyCalendar()
            GetData()
        End Sub

    End Class
End Namespace