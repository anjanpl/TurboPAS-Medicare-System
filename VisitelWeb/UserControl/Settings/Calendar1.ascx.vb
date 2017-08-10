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
    Public Class CalendarSetting1
        Inherits BaseUserControl

        Private ControlName As String
        Private objCalendarSchedule As CalendarSchedule
        Private objScheduleControlDataObject As ScheduleControlDataObject
        Protected objShared As SharedWebControls

        

        'Private TextBoxSundayHourMinute As TextBox, TextBoxMondayHourMinute As TextBox, TextBoxTuesdayHourMinute As TextBox, TextBoxWednesdayHourMinute As TextBox, _
        '    TextBoxThursdayHourMinute As TextBox, TextBoxFridayHourMinute As TextBox, TextBoxSaturdayHourMinute As TextBox, TextBoxTotalHourMinute As TextBox

        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "CalendarSetting"
           
            objShared = New SharedWebControls()
            objCalendarSchedule = New CalendarSchedule()
            objShared.ConnectionOpen()
            InitializeControl()
        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)
            BindDropDownList()
        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadJavascript()
        End Sub

        Protected Sub Page_Unload(sender As Object, e As EventArgs)
            objShared.ConnectionClose()
            objShared = Nothing
        End Sub

        Private Sub InitializeControl()
            'DropDownListClient.ClientIDMode = ClientIDMode.Static
            'DropDownListEmployee.ClientIDMode = ClientIDMode.Static

            'TextBoxStartDate.ClientIDMode = ClientIDMode.Static
            'TextBoxEndDate.ClientIDMode = ClientIDMode.Static
        End Sub

        Private Sub LoadJavascript()

            Dim scriptBlock As String = Nothing

            scriptBlock = "<script type='text/javascript'> " _
                        & " var TextBoxSundayHourMinute ='" & TextBoxSundayHourMinute.ClientID & "'; " _
                        & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

            'Response.Write(ucClockTime_SundayInTime.FindControl("DropDownListTime").ClientID)

            'Dim scriptBlock As String = Nothing

            'scriptBlock = "<script type='text/javascript'> " _
            '            & " var CalendarImagePath='" & objShared.GetCalendarImagePath & "'; " _
            '            & " var CalendarDateFormat='" & objShared.CalendarDateFormat & "'; " _
            '            & " var prm =''; " _
            '            & " jQuery(document).ready(function () {" _
            '            & "     prm = Sys.WebForms.PageRequestManager.getInstance(); " _
            '            & "     DateFieldsEvent();" _
            '            & "     InputMasking();" _
            '            & "     prm.add_endRequest(DateFieldsEvent); " _
            '            & "     prm.add_endRequest(InputMasking); " _
            '            & "}); " _
            '     & "</script>"

            'Page.Header.Controls.Add(New LiteralControl(scriptBlock))
        End Sub

        Private Sub BindDropDownList()
            objShared.BindClientDropDownList(DropDownListClient, objShared.CompanyId, EnumDataObject.ClientListFor.Individual)
            objShared.BindEmployeeDropDownList(DropDownListEmployee, objShared.CompanyId, False)
            BindScheduleStatusDropDownList(DropDownListScheduleStatus)
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

    End Class
End Namespace