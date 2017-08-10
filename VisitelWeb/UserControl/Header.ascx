<%@ Control Language="vb" CodeBehind="~/UserControl/Header.ascx.vb" Inherits="VisitelWeb.Visitel.UserControl.Header" %>
<%--<%@ Register Src="ThemeSelector.ascx" TagName="ThemeSelector" TagPrefix="uc1" %>--%>
<table style="width: 100%;" cellpadding="0" cellspacing="0" border="0" class="header-thickLine">
    <tr>
        <td valign="bottom">
            <div id="header_logo">
                <asp:Image ID="Image1" ImageUrl="~/Images/CompanyLogo.svg" Width="100%" Height="100%"
                    runat="server" />
                <%--Logo Comes Here--%>
            </div>
            <br />
            <asp:Label ID="lblProjectName" CssClass="ProjectName" runat="server"></asp:Label>
            <asp:Label ID="lblProjectVersion" CssClass="ProjectVersion" runat="server" EnableTheming="false">
            </asp:Label><asp:Label ID="lblPublishDate" CssClass="ProjectVersion" runat="server"
                Text="" EnableTheming="false"></asp:Label>
        </td>
        <%-- <td style="width: 30%; text-align: right;">
            <asp:HyperLink ID="HyperLink1" NavigateUrl="~/Default.aspx" runat="server"><asp:Image ID="Image1" runat="server" ImageUrl="~/Images/TopLogo.png" /></asp:HyperLink></td>--%>
    </tr>
</table>
<script type="text/javascript">


    $(document).ready(function () {



        $("#ThemeSelectorTrigger").click(function () {
            if ($("#ThemeSelectorContent").is(":hidden")) {
                $("#ThemeSelectorContent").slideDown("slow");
            }
            else {
                $("#ThemeSelectorContent").slideUp("slow");
            }
        });

    });
    
   
</script>
