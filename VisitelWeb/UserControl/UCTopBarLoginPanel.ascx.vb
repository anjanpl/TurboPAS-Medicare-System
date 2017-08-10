Imports VisitelCommon.VisitelCommon

Namespace Visitel.UserControl

    Public Class UCTopBarLoginPanel
        Inherits BaseUserControl

        Private ControlName As String

        Protected Sub Page_Init(sender As Object, e As EventArgs)
            ControlName = "TopBarLoginPanel"
        End Sub

        Protected Sub Page_Load(sender As Object, e As EventArgs)

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
