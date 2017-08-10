Namespace Visitel.UserControl
    Public Class ThemeSelector
        Inherits System.Web.UI.UserControl
        Protected Sub Page_Load(sender As Object, e As EventArgs)

        End Sub

        Protected Sub lbtnRedmond_Click(sender As Object, e As EventArgs)
            SetUserTheme("redmond")
        End Sub

        Protected Sub lbtnBlitzer_Click(sender As Object, e As EventArgs)
            SetUserTheme("blitzer")
        End Sub

        Private Sub SetUserTheme(themeName As String)
            'UserInfoDA uiDA = new UserInfoDA();

            'IList<UserInfo> ui = uiDA.UserInfoGetByUserID(Convert.ToInt32(Session[StaticSettings.SessionField.USER_ID].ToString()));

            'ReferanceCodesDA refDA = new ReferanceCodesDA();
            'IList<ReferanceCodes> rcds = refDA.ReferanceCodesGetByGroupID(48);

            'foreach (ReferanceCodes rc in rcds)
            '{
            '    if (rc.Code == themeName)
            '    {
            '        ui[0].ThemeName = rc.RCID.ToString();
            '        uiDA.Update(ui[0]);
            '    }
            '}
        End Sub
    End Class
End Namespace
