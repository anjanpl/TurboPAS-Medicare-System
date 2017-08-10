Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports VisitelCommon.VisitelCommon
Imports VisitelBusiness.VisitelBusiness

Namespace Visitel
    Public Class VisitelMasterPage
        Inherits MasterPage
        Implements IMyMasterPage

        Private _strPageHeaderTitle As String
        ''' <summary>
        ''' Public property to set the page header title. All content page will access this property.
        ''' </summary>
        Public Property PageHeaderTitle() As String Implements IMyMasterPage.PageHeaderTitle
            Get
                Return _strPageHeaderTitle
            End Get
            Set(value As String)
                _strPageHeaderTitle = value

                Me.Page.Title = If((Me.Page.Title = "Untitled Page" OrElse Me.Page.Title.Length = 0),
                                   ConfigurationManager.AppSettings(StaticSettings.AppSettingKey.PROJECT_NAME) & " : " & value, Me.Page.Title)

                'Me.Page.Title = GlobalConfig.ProjectName & " : " & value
                'Me.Page.Title = Convert.ToString("Visitel" & " : ") & value
            End Set
        End Property

        Protected Sub Page_Init(sender As Object, e As EventArgs)

            ScriptManager.GetCurrent(Me.Page).AsyncPostBackTimeout = ConfigurationManager.AppSettings(StaticSettings.AppSettingKey.ASYNC_POST_BACK_TIMEOUT)

            Dim scriptBlock As String = Nothing
            Dim objShared As New SharedWebControls
            scriptBlock = "<script type='text/javascript'> " _
                        & " var AjaxLoaderImagePath ='" & objShared.GetPopupUrl("Images/ajax-loader.gif") & "'; " _
                        & "</script>"

            Page.Header.Controls.Add(New LiteralControl(scriptBlock))

            objShared = Nothing

        End Sub

        ''' <summary>
        ''' Page load event handler.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        Protected Sub Page_Load(sender As Object, e As EventArgs)
            Dim mplblPageHeader As Label = DirectCast(FindControl("lblPageHeader"), Label)
            If mplblPageHeader IsNot Nothing Then
                mplblPageHeader.Text = _strPageHeaderTitle
            End If

            'if users password expired and enforce password expiration flag is set then redirect him to change password page
            'Dim uDA As New UserInfoDA()
            'Dim uinf As IList(Of UserInfo) = uDA.UserInfoGetByUserID(Convert.ToInt32(Session(StaticSettings.SessionField.USER_ID)))

            'Try
            '    If Request.Url.AbsoluteUri.ToLower().IndexOf("forcepasswordchange.aspx") < 0 _
            '        AndAlso GlobalUtil.DayDifference(DateTime.Now, uinf(0).LastPasswordChangedDate) > GlobalConfig.PasswordChangeAfterNumberOfDays _
            '        AndAlso GlobalConfig.EnforcePasswordExpiration = True Then
            '        Response.Redirect("~/ForcePasswordChange.aspx", True)
            '    End If
            'Catch
            'End Try

        End Sub

        Protected Overrides Sub OnInit(e As EventArgs)
            divErrMesg.Visible = False
            divInfoMsg.Visible = False

            MyBase.OnInit(e)
        End Sub

        Protected Overrides Sub OnLoad(e As EventArgs)

            Timeout1.TimeoutMinutes = Session.Timeout
            Timeout1.AboutToTimeoutMinutes = Session.Timeout - 1

            If Not IsPostBack Then
                If Session(StaticSettings.SessionField.USER_ID) Is Nothing Then
                    Response.Redirect(ResolveUrl("~") + StaticSettings.NavigateURL.LOGIN_URL)
                Else
                    lblUser.Text = "User: " + Session(StaticSettings.SessionField.USER_NAME)
                End If
            End If

            MyBase.OnLoad(e)
            Page.Header.DataBind()
        End Sub

        Protected Overrides Sub OnPreRender(e As EventArgs)

            If lblErrMsg.Text = "" Then
                divErrMesg.Visible = False

                'upnlMsg.Update()
            End If

            If lblMessage.Text = "" Then
                divInfoMsg.Visible = False
                'upnlMsg.Update()
            End If

            MyBase.OnPreRender(e)
        End Sub

        Protected Sub DropDownListThemeSelect_OnSelectedIndexChanged(sender As Object, e As EventArgs)

        End Sub

        ''' <summary>
        ''' Display jquery based message
        ''' </summary>
        ''' <param name="pstrMsg">Message to be displayed</param>
        Public Sub DisplayMessage(pstrMsg As String)
            Dim prompt As String = "<script>$(document).ready(function(){{$.prompt('{0}',{{prefix:'cleanblue'}});}});</script>"
            Dim message As String = String.Format(prompt, pstrMsg)
            Page.ClientScript.RegisterStartupScript(GetType(Page), "message", message)
        End Sub

        ''' <summary>
        ''' Display error message at the top of the body
        ''' </summary>
        ''' <param name="pstrMsg"></param>
        Public Sub DisplayHeaderError(pstrMsg As String) Implements IMyMasterPage.DisplayHeaderError
            ClearMessages()

            divErrMesg.Visible = True
            lblErrMsg.Text = pstrMsg

            'upnlMsg.Update()
        End Sub

        ''' <summary>
        ''' Display info message at the top of the body
        ''' </summary>
        ''' <param name="pstrMsg"></param>
        Public Sub DisplayHeaderMessage(pstrMsg As String) Implements IMyMasterPage.DisplayHeaderMessage
            ClearMessages()

            divInfoMsg.Visible = True
            lblMessage.Text = pstrMsg

            'upnlMsg.Update()
        End Sub

        ''' <summary>
        ''' Clear message at the top of the body
        ''' </summary>
        ''' <param name="pstrMsg"></param>
        Public Sub ClearMessages()
            divInfoMsg.Visible = False
            lblMessage.Text = ""

            divErrMesg.Visible = False
            lblErrMsg.Text = ""
        End Sub

        Public Function LabelMessage() As Label
            Return lblMessage
        End Function

        Public Function HtmlGenericControlInfoMsg() As HtmlGenericControl
            Return divInfoMsg
        End Function

        Public Function HtmlGenericControlErrMesg() As HtmlGenericControl
            Return divErrMesg
        End Function

        Public Function LabelErrMsg() As Label
            Return lblErrMsg
        End Function

        Protected Overrides Sub Render(writer As HtmlTextWriter)
            UnblockUI()
            MyBase.Render(writer)
        End Sub
        ''' <summary>
        ''' register unblockui jquery
        ''' </summary>
        Public Sub UnblockUI()
            Dim prompt As String = "<script>$(document).ready(function(){{ $.unblockUI(); }});</script>"


            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "UnBlockUI", prompt, False)
        End Sub

        Public Property GlobalConfig() As GlobalConfigurationDataObject
            Get
                Dim gConfig As New GlobalConfigurationDataObject()

                If Cache("GlobalConfig") Is Nothing Then
                    ' Dim cfgDA As New GlobalConfigurationDA()

                    'gConfig = cfgDA.GlobalConfigurationGetAll()

                    Cache("GlobalConfig") = gConfig
                Else
                    gConfig = DirectCast(Cache("GlobalConfig"), GlobalConfigurationDataObject)
                End If
                Return gConfig
            End Get
            Set(value As GlobalConfigurationDataObject)
                Cache("GlobalConfig") = value
            End Set
        End Property
    End Class
End Namespace



