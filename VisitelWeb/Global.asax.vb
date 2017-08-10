Imports System.Web.SessionState

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Public AppId As Guid = Guid.NewGuid()

    Public TestMessage As [String]

    Public Function GetAppDescription(eventName As [String]) As [String]
        Dim builder As New StringBuilder()

        builder.AppendFormat("-------------------------------------------{0}", Environment.NewLine)
        builder.AppendFormat("Event: {0}{1}", eventName, Environment.NewLine)
        builder.AppendFormat("Guid: {0}{1}", AppId, Environment.NewLine)
        builder.AppendFormat("Thread Id: {0}{1}", System.Threading.Thread.CurrentThread.ManagedThreadId, Environment.NewLine)
        builder.AppendFormat("Appdomain: {0}{1}", AppDomain.CurrentDomain.FriendlyName, Environment.NewLine)
        builder.AppendFormat("TestMessage: {0}{1}", TestMessage, Environment.NewLine)
        builder.Append((If(System.Threading.Thread.CurrentThread.IsThreadPoolThread, "Pool Thread", "No Thread")) + Environment.NewLine)

        Return builder.ToString()
    End Function

    Private Sub Application_Start(sender As Object, e As EventArgs)
        ' Fires when the application is started
        TestMessage = "not null"

        ' Code that runs on application startup
        Trace.WriteLine(GetAppDescription("Application_Start"))
    End Sub

    Private Sub Application_EndRequest(sender As Object, e As EventArgs)
        Trace.WriteLine(GetAppDescription("Application_EndRequest"))
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
    End Sub

    Private Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
        Trace.WriteLine(GetAppDescription("Application_BeginRequest"))
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Private Sub Application_Error(sender As Object, e As EventArgs)
        ' Fires when an error occurs
        ' Code that runs when an unhandled error occurs
        Trace.WriteLine(GetAppDescription("Application_Error"))
    End Sub

    Private Sub Application_End(sender As Object, e As EventArgs)
        ' Fires when the application ends
        '  Code that runs on application shutdown
        Trace.WriteLine(GetAppDescription("Application_End"))
    End Sub

End Class