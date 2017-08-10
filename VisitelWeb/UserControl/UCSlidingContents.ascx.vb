Namespace Visitel.UserControl
    Public Class UCSlidingContents
        Inherits System.Web.UI.UserControl

        'Private scDA As New SlidingContentDA()
        Private sb As New StringBuilder()
        Protected sContentsHTML As String = String.Empty

        Protected Sub Page_Load(sender As Object, e As EventArgs)
            DisplaySlidingContent()
        End Sub

        Protected Sub DisplaySlidingContent()
            Dim dt As DataTable = Nothing 'scDA.GetSlidingContents()

            If dt.Rows.Count > 0 Then
                sb.Append("<div style=""width:98%;"">").Append(vbCr & vbLf)
                sb.Append("<div id=""newFeaturesSlider"">").Append(vbCr & vbLf)
                sb.Append("<ul>").Append(vbCr & vbLf)
                For Each dr As DataRow In dt.Rows
                    sb.Append("<li>").Append(vbCr & vbLf)
                    sb.Append("<h2>").Append(dr("DESCRIPTION").ToString()).Append("</h2>")
                    sb.Append("<p>").Append(dr("HtmlContent").ToString()).Append("</p>")
                    sb.Append("</li>").Append(vbCr & vbLf)
                Next
                sb.Append("</ul>").Append(vbCr & vbLf)
                sb.Append("</div>").Append(vbCr & vbLf)
                sb.Append("</div>")

                sContentsHTML = sb.ToString()
            End If
        End Sub

    End Class
End Namespace
