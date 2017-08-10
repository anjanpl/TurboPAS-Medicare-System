﻿Imports System.Web.UI
Imports VisitelCommon.VisitelCommon.DataObject

Namespace VisitelCommon
    Public Interface IScheduleEmployee

        Sub SaveData(EmployeeId As Integer)

        Sub GetActiveOnly()

        Sub GetInActiveOnly()

        Sub GetAll(EmployeeId As Integer)

        Sub LoadScheduleJavaScript()

        Sub LoadDynamicJavascript(ByRef Control As Control)

        Sub GetScheduleCaptionFromResource()

        Sub LoadControlsWithData()

        Sub SetControl(ByRef objScheduleControlDataObject As ScheduleControlDataObject)

    End Interface
End Namespace