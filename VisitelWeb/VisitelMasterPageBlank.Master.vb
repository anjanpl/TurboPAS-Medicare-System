Imports System.Web.UI
Imports VisitelCommon.VisitelCommon

Namespace Visitel
    Public Class VisitelMasterPageBlank
        Inherits MasterPage
        Implements IMyMasterPage

        Private _strPageHeaderTitle As String
        Public Property PageHeaderTitle() As String Implements IMyMasterPage.PageHeaderTitle
            Get
                Return _strPageHeaderTitle
            End Get
            Set(value As String)
                _strPageHeaderTitle = value
                'this.Page.Title = (this.Page.Title == "Untitled Page" || this.Page.Title.Length == 0) ? ConfigurationManager.AppSettings[StaticSettings.AppSettingKey.PROJECT_NAME] + " : " + value : this.Page.Title;
                'this.Page.Title = GlobalConfig.ProjectName + " : " + value;
                Me.Page.Title = Convert.ToString("Visitel" + " : ") & value
            End Set
        End Property
        Protected Sub Page_Load(sender As Object, e As EventArgs)

            ScriptManager.GetCurrent(Me.Page).AsyncPostBackTimeout = ConfigurationManager.AppSettings(StaticSettings.AppSettingKey.ASYNC_POST_BACK_TIMEOUT)

            divErrMesg.Visible = False
            divInfoMsg.Visible = False
        End Sub

        Protected Overrides Sub OnInit(e As EventArgs)
            divErrMesg.Visible = False
            divInfoMsg.Visible = False

            MyBase.OnInit(e)
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
            divInfoMsg.Visible = False
            divErrMesg.Visible = True
            lblErrMsg.Text = pstrMsg
        End Sub

        ''' <summary>
        ''' Display info message at the top of the body
        ''' </summary>
        ''' <param name="pstrMsg"></param>
        Public Sub DisplayHeaderMessage(pstrMsg As String) Implements IMyMasterPage.DisplayHeaderMessage
            divErrMesg.Visible = False
            divInfoMsg.Visible = True
            lblMessage.Text = pstrMsg
        End Sub

        ''' <summary>
        ''' register unblockui jquery
        ''' </summary>
        Public Sub UnblockUI()
            Dim prompt As String = "<script>$(document).ready(function(){{ $.unblockUI(); }});</script>"
            ScriptManager.RegisterClientScriptBlock(Me, GetType(Page), "UnBlockUI", prompt, False)
        End Sub
    End Class
End Namespace

