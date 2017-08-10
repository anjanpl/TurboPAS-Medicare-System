Imports VisitelCommon.VisitelCommon

Namespace Visitel.UserControl
    Public Class UCChangePassword
        Inherits BaseUserControl

        Private ControlName As String

        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "ChangePassword"
        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)
            If Session(StaticSettings.SessionField.USER_ID) IsNot Nothing Then
                hdnUserID.Value = Session(StaticSettings.SessionField.USER_ID).ToString()
            End If
        End Sub

        Protected Sub Page_PreRender(sender As Object, e As EventArgs)
            LoadJScript()
        End Sub

        ''' <summary>
        ''' Importing Javascript
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub LoadJScript()
            LoadJS("JavaScript/" & ControlName & ".js")
        End Sub

    End Class
End Namespace