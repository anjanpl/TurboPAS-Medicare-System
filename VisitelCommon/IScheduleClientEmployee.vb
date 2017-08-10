Imports System.Web.UI
Imports VisitelCommon.VisitelCommon.DataObject

Namespace VisitelCommon
    Public Interface IScheduleClientEmployee

        Sub SaveData()

        Sub LoadScheduleJavaScript()

        Sub LoadDynamicJavascript(ByRef Control As Control)

        Sub GetScheduleCaptionFromResource()

        Sub LoadControlsWithData()

        Sub SetControl(ByRef objScheduleControlDataObject As ScheduleControlDataObject)

    End Interface
End Namespace