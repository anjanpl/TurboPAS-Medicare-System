<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="~/UserControl/TopBarSlidingPanel.ascx.vb"
    Inherits="VisitelWeb.Visitel.UserControl.TopBarSlidingPanel" %>
<div id="topPanel">
    <div id="Panel">
        <div id="Panel_contents">
        </div>
        <div id="themeSelector">
            <table cellpadding="8" cellspacing="7">
                <tr>
                    <%--<td width="350px" height="154px">
                        <asp:ImageButton ID="ibtnRedmond" Width="350px" Height="154px" OnClientClick="ChangeTheme('redmond')"
                            CausesValidation="false" ImageUrl="~/Images/Connect Plus - blue.jpg" runat="server"
                            OnClick="ibtnRedmond_Click" />
                        <br />
                        <asp:Label ID="Label1" runat="server" Text="Redmond"></asp:Label>
                    </td>--%>
                 <%--   <td width="350px" height="154px">
                         <asp:ImageButton ID="ibtnBlitzer" runat="server" Width="350px" Height="154px" OnClientClick="ChangeTheme('blitzer')"
                            CausesValidation="false" ImageUrl="~/Images/Connect Plus - red.jpg" OnClick="ibtnBlitzer_Click" />
                        <asp:Label ID="Label2" runat="server" Text="Blitzer"></asp:Label>
                    </td>--%>
                    <%--<td width="350px" height="154px">
                        <asp:ImageButton ID="ibtnuidarkness" runat="server" Width="350px" Height="154px"
                            OnClientClick="ChangeTheme('ui-darkness')" CausesValidation="false" ImageUrl="~/Images/Connect Plus - black.jpg"
                            OnClick="ibtnuidarkness_Click" />
                        <asp:Label ID="Label3" runat="server" Text="ui-darkness"></asp:Label>
                    </td>--%>
                </tr>
            </table>
            <iframe id="magicFrame" width="0" height="0" style="display: none;"></iframe>
            
        </div>
    </div>
    <div class="Panel_button" style="display: block;">
        <%-- <asp:Image ID="Image1" ImageUrl="~/images/expand.png" Width="24" Height="24" runat="server"
            AlternateText="expand" />
        <a href="#">Theme</a>--%>
    </div>
    <div class="Panel_button" id="hide_button" style="display: none;">
        <%-- <asp:Image ID="Image2" ImageUrl="~/images/collapse.png" Width="24" Height="24" runat="server"
            AlternateText="expand" />
        <a href="#">Hide</a>--%>
    </div>
</div>
<script language="javascript" type="text/javascript">
    $(document).ready(function () {

        $("div.Panel_button").click(function () {
            $("div#Panel").animate({
                height: "300px"
            })
		.animate({
		    height: "250px"
		}, "fast");
            $("div.Panel_button").toggle();

        });

        $("div#hide_button").click(function () {
            $("div#Panel").animate({
                height: "0px"
            }, "fast");
        });
    });

    function ChangeTheme(themeName) {

        document.cookie = "theme=" + themeName;

        var l = window.location;
        var base_url = l.protocol + "//" + l.host + "/" + l.pathname.split('/')[1];

        document.getElementById('magicFrame').src = base_url + '/ChangeThemeHidden.aspx?theme=' + themeName;



    }
</script>
