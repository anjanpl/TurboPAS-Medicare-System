Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports System.Data.SqlClient
Imports VisitelCommon.VisitelCommon.DataObject
Imports VisitelBusiness.VisitelBusiness.DataObject.Settings

Namespace Visitel.UserControl.ClientInfo
    Public Class ClientCalendarControl
        Inherits CalendarSchedule

        Private ControlName As String, SocialSecurityNumber As String, StateClientId As String
        Private objCalendarSchedule As CalendarSchedule
        Private objScheduleControlDataObject As ScheduleControlDataObject
        Private IndividualId As Int64

        Protected Sub Page_Init(sender As Object, e As EventArgs)

            ControlName = "ClientCalendarControl"
            objShared = New SharedWebControls()
            objCalendarSchedule = New CalendarSchedule()
            objShared.ConnectionOpen()

            InitializeWeeklyCalendarControl()
            ShareScheduleCommonControl()

            DirectCast(Me, IScheduleClient).GetScheduleCaptionFromResource()

            If Not IsPostBack Then
                objCalendarSchedule.ScheduleList = New List(Of CalendarSettingDataObject)()
                objCalendarSchedule.AllScheduleList = New List(Of CalendarSettingDataObject)()
            End If

            DirectCast(Me, IScheduleClient).LoadControlsWithData()

        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)
            If Not IsPostBack Then
                GetData()
            End If
        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            'LoadJScript()
            DirectCast(Me, IScheduleClient).LoadScheduleJavaScript()
            DirectCast(Me, IScheduleClient).LoadDynamicJavascript(Page)
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objCalendarSchedule = Nothing
            objScheduleControlDataObject = Nothing
        End Sub

        Private Sub LoadJScript()

            'LoadJS("JavaScript/jquery.blockUI.js")

            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                           & " var prmClientCalendar =''; " _
                           & " jQuery(document).ready(function () {" _
                           & "     prmClientCalendar = Sys.WebForms.PageRequestManager.getInstance(); " _
                           & "     prmClientCalendar.add_initializeRequest(InitializeRequest); " _
                           & "}); " _
                    & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

        End Sub

        Private Sub GetData()

            Try

                BindSearchByScheduleStatusDropDownList(DropDownListSearchByScheduleStatus)

                DirectCast(Me, IScheduleClient).GetAll(Me.IndividualId)

            Catch ex As SqlException
                DirectCast(Me.Page.Master, IMyMasterPage).DisplayHeaderError("Unable to fetch data.")
            End Try

        End Sub

        Public Sub LoadWeeklyCalendar()
            GetData()
        End Sub

        Private Sub InitializeWeeklyCalendarControl()

            ButtonSaveSchedule.CausesValidation = True
            ButtonSaveSchedule.ValidationGroup = ScheduleValidationGroup

            'Dim ImageButtonEditSetting As ImageButton = DirectCast(UserControlEditSettingClientInfo.FindControl("ImageButtonEditSetting"), ImageButton)
            'ImageButtonEditSetting.CausesValidation = False

            ButtonActiveOnly.CausesValidation = False
            ButtonInActiveOnly.CausesValidation = False
            ButtonAll.CausesValidation = False
            'ButtonClear.CausesValidation = False

            AddHandler ButtonActiveOnly.Click, AddressOf ButtonActiveOnly_Click
            AddHandler ButtonInActiveOnly.Click, AddressOf ButtonInActiveOnly_Click

            AddHandler ButtonAll.Click, AddressOf ButtonAll_Click

            AddHandler ButtonSaveSchedule.Click, AddressOf ButtonSave_Click
            AddHandler ButtonEmployeeDetail.Click, AddressOf ButtonEmployeeDetail_Click

            AddHandler ButtonClearSchedule.Click, AddressOf ButtonClear_Click

            DropDownListSearchByScheduleStatus.AutoPostBack = True
            AddHandler DropDownListSearchByScheduleStatus.SelectedIndexChanged, AddressOf DropDownListSearchByScheduleStatus_OnSelectedIndexChanged

            ButtonActiveOnly.ClientIDMode = UI.ClientIDMode.Static
            ButtonInActiveOnly.ClientIDMode = UI.ClientIDMode.Static
            ButtonAll.ClientIDMode = UI.ClientIDMode.Static

            ButtonSaveSchedule.ClientIDMode = UI.ClientIDMode.Static
            ButtonClearSchedule.ClientIDMode = UI.ClientIDMode.Static

        End Sub

        Private Sub ButtonEmployeeDetail_Click(sender As Object, e As EventArgs)
            Response.Redirect("~/Pages/EmployeeInfo.aspx?EmployeeId=" & HiddenFieldEmployeeDetailId.Value)
        End Sub

        Private Sub ButtonClear_Click(sender As Object, e As EventArgs)

            'DropDownListSearchByIndividual.SelectedIndex = 0
            DropDownListSearchByScheduleStatus.SelectedIndex = 0
            AllScheduleList.Clear()
            ScheduleList.Clear()
            DirectCast(Me, IScheduleClient).LoadControlsWithData()

            HiddenFieldScheduleId.Value = Convert.ToString(Int32.MinValue)

            objShared.TabSelection(2)

        End Sub

        Private Sub ButtonSave_Click(sender As Object, e As EventArgs)

            DirectCast(Me, IScheduleClient).SaveData(IndividualId)
            objShared.TabSelection(2)
            
        End Sub

        Private Sub ButtonActiveOnly_Click(sender As Object, e As EventArgs)
            DirectCast(Me, IScheduleClient).GetActiveOnly()
        End Sub

        Private Sub ButtonInActiveOnly_Click(sender As Object, e As EventArgs)
            DirectCast(Me, IScheduleClient).GetInActiveOnly()
        End Sub

        Private Sub ButtonAll_Click(sender As Object, e As EventArgs)
            DirectCast(Me, IScheduleClient).GetAll(IndividualId)
        End Sub

        Private Sub ShareScheduleCommonControl()

            objScheduleControlDataObject = New ScheduleControlDataObject()

            objScheduleControlDataObject.UserId = objShared.UserId
            objScheduleControlDataObject.CompanyId = objShared.CompanyId

            objScheduleControlDataObject.HiddenFieldScheduleId = HiddenFieldScheduleId
            objScheduleControlDataObject.PlaceHolderCalendarSetting = PlaceHolderCalendarSetting

            objScheduleControlDataObject.DropDownListSearchByScheduleStatus = DropDownListSearchByScheduleStatus

            objScheduleControlDataObject.IsClientVisible = False
            objScheduleControlDataObject.IsEmployeeVisible = True

            objScheduleControlDataObject.LabelWeeklyCalendar = LabelWeeklyCalendar

            objScheduleControlDataObject.ButtonActiveOnly = ButtonActiveOnly
            objScheduleControlDataObject.ButtonInActiveOnly = ButtonInActiveOnly
            objScheduleControlDataObject.ButtonAll = ButtonAll
            objScheduleControlDataObject.LabelClientStatus = LabelClientStatus
            objScheduleControlDataObject.LabelHoursMinutesCaption = LabelHoursMinutesCaption
            objScheduleControlDataObject.LabelEmployeeCaption = LabelEmployeeCaption

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
            objScheduleControlDataObject.ButtonEmployeeDetail = ButtonEmployeeDetail
            objScheduleControlDataObject.ButtonClearSchedule = ButtonClearSchedule

            DirectCast(Me, IScheduleClient).SetControl(objScheduleControlDataObject)

            objScheduleControlDataObject = Nothing

        End Sub


        Public Sub SetIndividualId(IndividualId As Integer)
            Me.IndividualId = IndividualId
        End Sub

        ''' <summary>
        ''' Setting StateClientId from Parent Page
        ''' </summary>
        ''' <param name="StateClientId"></param>
        ''' <remarks></remarks>
        Public Sub SetStateClientId(StateClientId As String)
            Me.StateClientId = StateClientId
        End Sub

        ''' <summary>
        ''' Setting SocialSecurityNumber from Parent Page
        ''' </summary>
        ''' <param name="SocialSecurityNumber"></param>
        ''' <remarks></remarks>
        Public Sub SetSocialSecurityNumber(SocialSecurityNumber As String)
            Me.SocialSecurityNumber = SocialSecurityNumber
        End Sub

    End Class
End Namespace