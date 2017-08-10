Imports VisitelCommon.VisitelCommon

Namespace Visitel.UserControl
    Public Class Header
        Inherits System.Web.UI.UserControl

        Protected Sub Page_Init(sender As Object, e As EventArgs)
            'abc
        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)

            lblProjectVersion.Text = ConfigurationManager.AppSettings(StaticSettings.AppSettingKey.CURRENT_VERSION)
            lblPublishDate.Text = ConfigurationManager.AppSettings(StaticSettings.AppSettingKey.LAST_PUBLISH_DATE)
            lblProjectName.Text = ConfigurationManager.AppSettings(StaticSettings.AppSettingKey.PROJECT_NAME)

            'Dim gConfig As GlobalConfiguration = DirectCast(Cache("GlobalConfig"), GlobalConfiguration)

            'lblProjectVersion.Text = gConfig.CurrentVersion.ToString()

            'lblPublishDate.Text = gConfig.LastPublishDate.ToString("yyyyMMdd")

            'If Request.Url.AbsoluteUri.IndexOf("LoginPage.aspx") >= 0 Then
            '    'ThemeSelector.Visible = false;
            '    lblProjectName.Text = gConfig.LoginPageTitle.ToString()
            'Else
            '    lblProjectName.Text = gConfig.ProjectName.ToString()
            'End If
        End Sub

    End Class
End Namespace
