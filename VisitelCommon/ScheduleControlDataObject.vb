Imports System.Web.UI.WebControls

Namespace VisitelCommon.DataObject
    Public Class ScheduleControlDataObject

        Private m_UserId As Integer
        Public Property UserId() As Integer
            Get
                Return m_UserId
            End Get
            Set(value As Integer)
                m_UserId = value
            End Set
        End Property

        Private m_CompanyId As Integer
        Public Property CompanyId() As Integer
            Get
                Return m_CompanyId
            End Get
            Set(value As Integer)
                m_CompanyId = value
            End Set
        End Property

        Private m_HiddenFieldScheduleId As HiddenField
        Public Property HiddenFieldScheduleId() As HiddenField
            Get
                Return m_HiddenFieldScheduleId
            End Get
            Set(value As HiddenField)
                m_HiddenFieldScheduleId = value
            End Set
        End Property

        Private m_PlaceHolderCalendarSetting As PlaceHolder
        Public Property PlaceHolderCalendarSetting() As PlaceHolder
            Get
                Return m_PlaceHolderCalendarSetting
            End Get
            Set(value As PlaceHolder)
                m_PlaceHolderCalendarSetting = value
            End Set
        End Property

        Private m_DropDownListSearchByClient As DropDownList
        Public Property DropDownListSearchByClient() As DropDownList
            Get
                Return m_DropDownListSearchByClient
            End Get
            Set(value As DropDownList)
                m_DropDownListSearchByClient = value
            End Set
        End Property

        Private m_IsClientVisible As Boolean
        Public Property IsClientVisible() As Boolean
            Get
                Return m_IsClientVisible
            End Get
            Set(value As Boolean)
                m_IsClientVisible = value
            End Set
        End Property


        Private m_DropDownListSearchByScheduleStatus As DropDownList
        Public Property DropDownListSearchByScheduleStatus() As DropDownList
            Get
                Return m_DropDownListSearchByScheduleStatus
            End Get
            Set(value As DropDownList)
                m_DropDownListSearchByScheduleStatus = value
            End Set
        End Property

        Private m_IsClientStatusVisible As Boolean
        Public Property IsClientStatusVisible() As Boolean
            Get
                Return m_IsClientStatusVisible
            End Get
            Set(value As Boolean)
                m_IsClientStatusVisible = value
            End Set
        End Property

        Private m_IsEmployeeVisible As Boolean
        Public Property IsEmployeeVisible() As Boolean
            Get
                Return m_IsEmployeeVisible
            End Get
            Set(value As Boolean)
                m_IsEmployeeVisible = value
            End Set
        End Property

        Private m_LabelWeeklyCalendar As Label
        Public Property LabelWeeklyCalendar() As Label
            Get
                Return m_LabelWeeklyCalendar
            End Get
            Set(value As Label)
                m_LabelWeeklyCalendar = value
            End Set
        End Property

        Private m_ButtonActiveOnly As Button
        Public Property ButtonActiveOnly() As Button
            Get
                Return m_ButtonActiveOnly
            End Get
            Set(value As Button)
                m_ButtonActiveOnly = value
            End Set
        End Property

        Private m_ButtonInActiveOnly As Button
        Public Property ButtonInActiveOnly() As Button
            Get
                Return m_ButtonInActiveOnly
            End Get
            Set(value As Button)
                m_ButtonInActiveOnly = value
            End Set
        End Property

        Private m_ButtonAll As Button
        Public Property ButtonAll() As Button
            Get
                Return m_ButtonAll
            End Get
            Set(value As Button)
                m_ButtonAll = value
            End Set
        End Property

        Private m_LabelClientStatus As Label
        Public Property LabelClientStatus() As Label
            Get
                Return m_LabelClientStatus
            End Get
            Set(value As Label)
                m_LabelClientStatus = value
            End Set
        End Property

        Private m_LabelHoursMinutesCaption As Label
        Public Property LabelHoursMinutesCaption() As Label
            Get
                Return m_LabelHoursMinutesCaption
            End Get
            Set(value As Label)
                m_LabelHoursMinutesCaption = value
            End Set
        End Property

        Private m_LabelIndividualCaption As Label
        Public Property LabelIndividualCaption() As Label
            Get
                Return m_LabelIndividualCaption
            End Get
            Set(value As Label)
                m_LabelIndividualCaption = value
            End Set
        End Property

        Private m_LabelEmployeeCaption As Label
        Public Property LabelEmployeeCaption() As Label
            Get
                Return m_LabelEmployeeCaption
            End Get
            Set(value As Label)
                m_LabelEmployeeCaption = value
            End Set
        End Property
        '

        Private m_LabelSunday As Label
        Public Property LabelSunday() As Label
            Get
                Return m_LabelSunday
            End Get
            Set(value As Label)
                m_LabelSunday = value
            End Set
        End Property

        Private m_LabelMonday As Label
        Public Property LabelMonday() As Label
            Get
                Return m_LabelMonday
            End Get
            Set(value As Label)
                m_LabelMonday = value
            End Set
        End Property

        Private m_LabelTuesday As Label
        Public Property LabelTuesday() As Label
            Get
                Return m_LabelTuesday
            End Get
            Set(value As Label)
                m_LabelTuesday = value
            End Set
        End Property

        Private m_LabelWednesday As Label
        Public Property LabelWednesday() As Label
            Get
                Return m_LabelWednesday
            End Get
            Set(value As Label)
                m_LabelWednesday = value
            End Set
        End Property

        Private m_LabelThursday As Label
        Public Property LabelThursday() As Label
            Get
                Return m_LabelThursday
            End Get
            Set(value As Label)
                m_LabelThursday = value
            End Set
        End Property

        Private m_LabelFriday As Label
        Public Property LabelFriday() As Label
            Get
                Return m_LabelFriday
            End Get
            Set(value As Label)
                m_LabelFriday = value
            End Set
        End Property


        Private m_LabelSaturday As Label
        Public Property LabelSaturday() As Label
            Get
                Return m_LabelSaturday
            End Get
            Set(value As Label)
                m_LabelSaturday = value
            End Set
        End Property

        Private m_LabelTotal As Label
        Public Property LabelTotal() As Label
            Get
                Return m_LabelTotal
            End Get
            Set(value As Label)
                m_LabelTotal = value
            End Set
        End Property

        Private m_LabelStatus As Label
        Public Property LabelStatus() As Label
            Get
                Return m_LabelStatus
            End Get
            Set(value As Label)
                m_LabelStatus = value
            End Set
        End Property

        Private m_LabelUpdateDate As Label
        Public Property LabelUpdateDate() As Label
            Get
                Return m_LabelUpdateDate
            End Get
            Set(value As Label)
                m_LabelUpdateDate = value
            End Set
        End Property

        Private m_LabelUpdateBy As Label
        Public Property LabelUpdateBy() As Label
            Get
                Return m_LabelUpdateBy
            End Get
            Set(value As Label)
                m_LabelUpdateBy = value
            End Set
        End Property

        Private m_LabelDetailInfo As Label
        Public Property LabelDetailInfo() As Label
            Get
                Return m_LabelDetailInfo
            End Get
            Set(value As Label)
                m_LabelDetailInfo = value
            End Set
        End Property

        Private m_ButtonSaveSchedule As Button
        Public Property ButtonSaveSchedule() As Button
            Get
                Return m_ButtonSaveSchedule
            End Get
            Set(value As Button)
                m_ButtonSaveSchedule = value
            End Set
        End Property

        Private m_ButtonIndividualDetail As Button
        Public Property ButtonIndividualDetail() As Button
            Get
                Return m_ButtonIndividualDetail
            End Get
            Set(value As Button)
                m_ButtonIndividualDetail = value
            End Set
        End Property

        Private m_ButtonEmployeeDetail As Button
        Public Property ButtonEmployeeDetail() As Button
            Get
                Return m_ButtonEmployeeDetail
            End Get
            Set(value As Button)
                m_ButtonEmployeeDetail = value
            End Set
        End Property

        Private m_ButtonClearSchedule As Button
        Public Property ButtonClearSchedule() As Button
            Get
                Return m_ButtonClearSchedule
            End Get
            Set(value As Button)
                m_ButtonClearSchedule = value
            End Set
        End Property

        Public Sub New()

            Me.UserId = Integer.MinValue
            Me.CompanyId = Integer.MinValue

            Me.HiddenFieldScheduleId = Nothing
            Me.PlaceHolderCalendarSetting = Nothing

            Me.DropDownListSearchByClient = Nothing
            Me.DropDownListSearchByScheduleStatus = Nothing

            Me.IsClientVisible = False
            Me.IsClientStatusVisible = False
            Me.IsEmployeeVisible = False

            Me.LabelWeeklyCalendar = Nothing

            Me.ButtonActiveOnly = Nothing
            Me.ButtonInActiveOnly = Nothing
            Me.ButtonAll = Nothing
            Me.LabelClientStatus = Nothing
            Me.LabelHoursMinutesCaption = Nothing
            Me.LabelIndividualCaption = Nothing
            Me.LabelEmployeeCaption = Nothing

            Me.LabelSunday = Nothing
            Me.LabelMonday = Nothing
            Me.LabelTuesday = Nothing
            Me.LabelWednesday = Nothing
            Me.LabelThursday = Nothing
            Me.LabelFriday = Nothing
            Me.LabelSaturday = Nothing
            Me.LabelTotal = Nothing
            Me.LabelStatus = Nothing
            Me.LabelUpdateDate = Nothing
            Me.LabelUpdateBy = Nothing
            Me.LabelDetailInfo = Nothing

            Me.ButtonSaveSchedule = Nothing
            Me.ButtonEmployeeDetail = Nothing
            Me.ButtonClearSchedule = Nothing

        End Sub
    End Class
End Namespace

