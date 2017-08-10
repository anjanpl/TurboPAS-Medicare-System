Imports System.Web.Services
Imports VisitelWeb.TurboPasApi
Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness
Imports VisitelBusiness.VisitelBusiness.DataObject

Public Class AjaxRequestHandler
    Inherits SharedWebControls

    Protected Sub Page_Init(sender As Object, e As EventArgs)

    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs)

    End Sub

    Protected Sub Page_Unload(sender As Object, e As EventArgs)

    End Sub


    <WebMethod()> _
    Public Function ValidatePasswordFormat(sPassword As String, UserID As String) As String
        Dim objBLUserInfo As New BLUserInfo()
        Return objBLUserInfo.ValidatePasswordFormat(ConVisitel, sPassword, UserID)
    End Function

    <WebMethod()> _
    Public Shared Sub onBrowserCloseDestroyUserSession()
        'PageTrackerUtil.TrackSessionEnd(HttpContext.Current.Session.SessionID)

        'Dim sUserName As String = Convert.ToString(HttpContext.Current.Session(StaticSettings.SessionField.USER_ID))
        'Dim sKey As String = sUserName & sUserName
        'HttpContext.Current.Cache.Remove(sKey)
        'FormsAuthentication.SignOut()
        'HttpContext.Current.Session.Abandon()

        '' clear authentication cookie
        'Dim cookie1 As New HttpCookie(FormsAuthentication.FormsCookieName, "")
        'cookie1.Expires = DateTime.Now.AddYears(-1)
        'HttpContext.Current.Response.Cookies.Add(cookie1)

        'Dim cookie2 As New HttpCookie("ASP.NET_SessionId", "")
        'cookie2.Expires = DateTime.Now.AddYears(-1)
        'HttpContext.Current.Response.Cookies.Add(cookie2)
    End Sub

    <WebMethod()> _
    Public Shared Function ChangePassword(OldPassword As String, sPassword As String, UserID As String) As String
        Dim objAjaxRequestHandler = New AjaxRequestHandler()
        Return objAjaxRequestHandler.ChangingPassword(OldPassword, sPassword, UserID)
    End Function

    Private Function ChangingPassword(OldPassword As String, NewPassword As String, UserID As String) As String
        Dim objUserInfoDataObject As New UserInfoDataObject

        Dim objBLUserInfo As New BLUserInfo()

        Try
            ConnectionOpen()
            objUserInfoDataObject = objBLUserInfo.UserInfoGetByUserId(ConVisitel, Convert.ToInt32(UserID))(0)
        Catch ex As Exception

        Finally
            ConnectionClose()
            objBLUserInfo = Nothing
        End Try

        Dim objPassword As New BLPassword(OldPassword, objUserInfoDataObject.PasswordSalt)

        Dim SaltedHash As String = objPassword.ComputeSaltedHash()

        Dim validPass As String = String.Empty

        Try
            objBLUserInfo = New BLUserInfo()
            ConnectionOpen()
            validPass = objBLUserInfo.ValidatePasswordFormat(ConVisitel, NewPassword, UserID)
        Catch ex As Exception

        Finally
            ConnectionClose()
            objBLUserInfo = Nothing
        End Try

        If validPass <> "valid" Then
            objUserInfoDataObject = Nothing
            Return validPass
        End If

        If (objUserInfoDataObject.SaltedHash.Equals(SaltedHash)) Then
            Try
                objBLUserInfo = New BLUserInfo()

                ConnectionOpen()
                objUserInfoDataObject.Password = NewPassword
                objBLUserInfo.ChangePassword(ConVisitel, objUserInfoDataObject)
                Return "Password has been changed successfully."
            Catch ex As Exception
                Return "The operation was not successful. Please try again later."
            Finally
                ConnectionClose()
                objBLUserInfo = Nothing
                objUserInfoDataObject = Nothing
            End Try
        Else
            Return "Invalid old password"
        End If
    End Function

    <WebMethod()> _
    Public Shared Function SendForgottenPassword(UserName As String) As String
        Dim objAjaxRequestHandler = New AjaxRequestHandler()
        Return objAjaxRequestHandler.ForgottenPassword(UserName)
    End Function

    Private Function ForgottenPassword(UserName As String) As String

        Dim objUserInfoDataObject As New UserInfoDataObject

        Dim objBLUserInfo As New BLUserInfo()

        Try
            ConnectionOpen()
            objUserInfoDataObject = objBLUserInfo.UserInfoGetByUserName(ConVisitel, UserName)
        Catch ex As Exception

        Finally
            ConnectionClose()
            objBLUserInfo = Nothing
        End Try

        If (Not objUserInfoDataObject Is Nothing) Then
            Dim mapcar As String = "123456789abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ"
            Dim randomGenerator As New Random()

            Dim NewPass As String = String.Empty

            For i As Integer = 0 To 7
                NewPass += mapcar.Substring(randomGenerator.[Next](mapcar.Length), 1)
            Next

            Dim objService As New TurboPASWebServiceSoapClient
            Try
                objBLUserInfo = New BLUserInfo()
                ConnectionOpen()
                objUserInfoDataObject.Password = NewPass
                objBLUserInfo.ChangePassword(ConVisitel, objUserInfoDataObject)
                objService.EmailSendPasswordRecovery(objUserInfoDataObject.Email, objUserInfoDataObject.UserName, NewPass)

                Return "Your Password Has Been Sent To Your Email Address."

            Catch ex As Exception
                Return "The operation was not successful. Please try again later."
            Finally
                ConnectionClose()
                objUserInfoDataObject = Nothing
                objBLUserInfo = Nothing
                objService = Nothing
            End Try
        Else
            Return "User Name not found."
        End If
    End Function
End Class