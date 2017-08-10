
#Region "Add/Modification Info"
'-----------------------------------------------------------------------------------------------------------------------------------
' Project Name: Visitel
' Purpose/Function: Edit Setting
' Author: Anjan Kumar Paul
' Start Date: 18 Aug 2014
' End Date: 18 Aug 2014
' History:
'      Version                  Author                      Date             Reason 
'      1.0.0                                                18 Aug 2014      Initial Development
'-----------------------------------------------------------------------------------------------------------------------------------

#End Region

Namespace Visitel.UserControl
    Public Class EditSetting
        Inherits System.Web.UI.UserControl

        Protected Sub Page_Init(sender As Object, e As EventArgs)
            InitializeControl()
        End Sub

        ''' <summary>
        ''' This is for control events regristering and instantiate object globally
        ''' </summary>
        Private Sub InitializeControl()

            AddHandler ImageButtonEditSetting.Click, AddressOf ImageButtonEditSetting_Click
        End Sub

        Private Sub ImageButtonEditSetting_Click(sender As Object, e As EventArgs)

            Dim appUrl = VirtualPathUtility.ToAbsolute("~/")
            Dim relativeUrl = HttpContext.Current.Request.Url.AbsolutePath.Remove(0, appUrl.Length - 1)

            Response.Redirect("~/Resource/EditSetting.aspx?RequestUrl=" & relativeUrl)
        End Sub

    End Class
End Namespace