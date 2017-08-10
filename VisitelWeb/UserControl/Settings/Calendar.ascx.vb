#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Calendar Setup 
' Author: Anjan Kumar Paul
' Start Date: 25 Oct 2014
' End Date: 25 Oct 2014
' History:
'      Version                  Author                      Date             Reason 
'      1.0.0                                                25 Oct 2014      Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Imports System.Collections.Generic
Imports System.Web.UI.WebControls
Imports VisitelCommon.VisitelCommon
Imports VisitelCommon.VisitelCommon.DataObject
Imports VisitelBusiness.VisitelBusiness.DataObject
Imports VisitelBusiness.VisitelBusiness.DataObject.Settings

Namespace Visitel.UserControl.Settings
    Public Class CalendarSetting
        Inherits CalendarSchedule

        Private ControlName As String
        Private objCalendarSchedule As CalendarSchedule
        Private objScheduleControlDataObject As ScheduleControlDataObject

        Protected Sub Page_Init(sender As Object, e As EventArgs)

            DirectCast(Me.Page.Master, IMyMasterPage).PageHeaderTitle = "Calendar Setting"

            ControlName = "CalendarSetting"
            objShared = New SharedWebControls()
            objCalendarSchedule = New CalendarSchedule()
            objShared.ConnectionOpen()

            InitializeControl()

            DirectCast(Me, IScheduleClientEmployee).GetScheduleCaptionFromResource()

            If Not IsPostBack Then
                objCalendarSchedule.ScheduleList = New List(Of CalendarSettingDataObject)()
                objCalendarSchedule.AllScheduleList = New List(Of CalendarSettingDataObject)()
            End If

            DirectCast(Me, IScheduleClientEmployee).LoadControlsWithData()

        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)

            If Not IsPostBack Then
                GetData()
            End If

        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)

            LoadCss("Settings/" & ControlName)

            DirectCast(Me, IScheduleClientEmployee).LoadScheduleJavaScript()
            DirectCast(Me, IScheduleClientEmployee).LoadDynamicJavascript(Page)

        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)

            objShared.ConnectionClose()
            objCalendarSchedule.ScheduleList = Nothing
            objCalendarSchedule.AllScheduleList = Nothing

            objCalendarSchedule = Nothing
            objScheduleControlDataObject = Nothing

        End Sub

        Private Sub ButtonAll_Click(sender As Object, e As EventArgs)
            DropDownListSearchByClient.SelectedIndex = 0
            DirectCast(Me, IScheduleClient).GetAll(Convert.ToInt32(DropDownListSearchByClient.SelectedValue))
        End Sub

        Private Sub ButtonSaveSchedule_Click(sender As Object, e As EventArgs)
            DirectCast(Me, IScheduleClientEmployee).SaveData()
        End Sub

        Private Sub ButtonClearSchedule_Click(sender As Object, e As EventArgs)

            DropDownListSearchByClient.SelectedIndex = 0
            DropDownListSearchByScheduleStatus.SelectedIndex = 0
            AllScheduleList.Clear()
            ScheduleList.Clear()

            DirectCast(Me, IScheduleClientEmployee).LoadControlsWithData()

            HiddenFieldScheduleId.Value = Convert.ToString(Int32.MinValue)

        End Sub

        Private Sub ButtonIndividualDetail_Click(sender As Object, e As EventArgs)
            Response.Redirect("~/Pages/ClientInfo.aspx?ClientId=" & HiddenFieldClientDetailId.Value)
        End Sub

        'Private Function InTimeOutTimeValidation(ByRef TextBoxInTime As TextBox, ByRef TextBoxOutTime As TextBox) As Boolean

        '    If (Not String.IsNullOrEmpty(Convert.ToString(TextBoxInTime.Text, Nothing).Trim()) _
        '            And Not String.IsNullOrEmpty(Convert.ToString(TextBoxOutTime.Text, Nothing).Trim())) Then

        '        Dim InTime As String = String.Empty, OutTime As String = String.Empty
        '        Dim InTimeHour As String = String.Empty, InTimeMinute As String = String.Empty
        '        Dim OutTimeHour As String = String.Empty, OutTimeMinute As String = String.Empty

        '        InTime = TextBoxInTime.Text.Trim().ToUpper()
        '        OutTime = TextBoxOutTime.Text.Trim().ToUpper()

        '        InTimeHour = InTime.Split(" ")(0).Split(":")(0)
        '        InTimeMinute = InTime.Split(" ")(0).Split(":")(1)

        '        OutTimeHour = OutTime.Split(" ")(0).Split(":")(0)
        '        OutTimeMinute = OutTime.Split(" ")(0).Split(":")(1)

        '        Dim InTimePMCheck As Boolean = InTime.Split(" ")(1).Equals("PM")
        '        Dim OutTimePMCheck As Boolean = OutTime.Split(" ")(1).Equals("PM")
        '        Dim InTimeAMCheck As Boolean = InTime.Split(" ")(1).Equals("AM")
        '        Dim OutTimeAMCheck As Boolean = OutTime.Split(" ")(1).Equals("AM")

        '        If (Not InTimeAMCheck And Not InTimePMCheck) Then
        '            CalculateTotalHourMinute()
        '            Return False
        '        End If

        '        If (Not OutTimeAMCheck And Not OutTimePMCheck) Then
        '            CalculateTotalHourMinute()
        '            Return False
        '        End If

        '        If (InTimePMCheck) Then
        '            InTimeHour = Convert.ToString(Convert.ToInt32(InTimeHour) + 12)
        '        End If

        '        If (OutTimePMCheck) Then
        '            OutTimeHour = Convert.ToString(Convert.ToInt32(OutTimeHour) + 12)
        '        End If

        '        If (((InTimePMCheck And OutTimePMCheck) Or (InTimeAMCheck And OutTimeAMCheck)) _
        '                And (New TimeSpan(InTimeHour, InTimeMinute, 0) > New TimeSpan(OutTimeHour, OutTimeMinute, 0))) Then

        '            Master.DisplayHeaderMessage(TextBoxInTime.ClientID & " cannot be greater than " & TextBoxOutTime.ClientID)

        '            CalculateTotalHourMinute()
        '            Return False
        '        End If

        '    End If

        '    Return True

        'End Function

        ''' <summary>
        ''' This is for control events regristering and instantiate object globally
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub InitializeControl()

            objScheduleControlDataObject = New ScheduleControlDataObject()

            objScheduleControlDataObject.UserId = objShared.UserId
            objScheduleControlDataObject.CompanyId = objShared.CompanyId

            objScheduleControlDataObject.HiddenFieldScheduleId = HiddenFieldScheduleId
            objScheduleControlDataObject.PlaceHolderCalendarSetting = PlaceHolderCalendarSetting

            objScheduleControlDataObject.DropDownListSearchByClient = DropDownListSearchByClient
            objScheduleControlDataObject.DropDownListSearchByScheduleStatus = DropDownListSearchByScheduleStatus

            objScheduleControlDataObject.IsClientVisible = True
            objScheduleControlDataObject.IsEmployeeVisible = True

            objScheduleControlDataObject.LabelWeeklyCalendar = LabelWeeklyCalendar

            objScheduleControlDataObject.ButtonAll = ButtonAll
            objScheduleControlDataObject.LabelHoursMinutesCaption = LabelHoursMinutesCaption
            objScheduleControlDataObject.LabelIndividualCaption = LabelIndividualCaption
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
            objScheduleControlDataObject.ButtonIndividualDetail = ButtonIndividualDetail
            objScheduleControlDataObject.ButtonClearSchedule = ButtonClearSchedule

            DirectCast(Me, IScheduleClientEmployee).SetControl(objScheduleControlDataObject)

            objScheduleControlDataObject = Nothing

            ButtonSaveSchedule.CausesValidation = True
            ButtonSaveSchedule.ValidationGroup = ScheduleValidationGroup

            Dim ImageButtonEditSetting As ImageButton = DirectCast(UserControlEditSetting.FindControl("ImageButtonEditSetting"), ImageButton)
            ImageButtonEditSetting.CausesValidation = False

            ButtonAll.CausesValidation = False
            ButtonClearSchedule.CausesValidation = False
            ButtonIndividualDetail.CausesValidation = False

            AddHandler ButtonAll.Click, AddressOf ButtonAll_Click

            AddHandler ButtonSaveSchedule.Click, AddressOf ButtonSaveSchedule_Click
            AddHandler ButtonIndividualDetail.Click, AddressOf ButtonIndividualDetail_Click
            AddHandler ButtonClearSchedule.Click, AddressOf ButtonClearSchedule_Click

            DropDownListSearchByClient.AutoPostBack = True
            AddHandler DropDownListSearchByClient.SelectedIndexChanged, AddressOf DropDownListSearchByClient_OnSelectedIndexChanged

            DropDownListSearchByScheduleStatus.AutoPostBack = True
            AddHandler DropDownListSearchByScheduleStatus.SelectedIndexChanged, AddressOf DropDownListSearchByScheduleStatus_OnSelectedIndexChanged

        End Sub

        ''' <summary>
        ''' This is for retrieving data
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub GetData()
            objShared.BindClientDropDownList(DropDownListSearchByClient, objShared.CompanyId, EnumDataObject.ClientListFor.Individual)
            BindSearchByScheduleStatusDropDownList(DropDownListSearchByScheduleStatus)
        End Sub

    End Class
End Namespace