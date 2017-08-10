<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="~/UserControl/ThemeSelector.ascx.vb"
    Inherits="VisitelWeb.Visitel.UserControl.ThemeSelector" %>
<table cellpadding="8" cellspacing="7" width="100%">
    <tr>
        <td width="200px" height="163px">
            <asp:LinkButton ID="lbtnRedmond" OnClientClick="ChangeTheme('redmond')" CausesValidation="false"
                runat="server" OnClick="lbtnRedmond_Click">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Connect Plus - blue.jpg"
                    Width="200px" Height="163px" BorderWidth="0px" />
                <br />
                <asp:Label ID="Label1" runat="server" Text="Redmond"></asp:Label></asp:LinkButton>
        </td>
    </tr>
    <tr>
        <td width="200px" height="163px">
            <asp:LinkButton ID="lbtnBlitzer" runat="server" OnClientClick="ChangeTheme('blitzer')"
                CausesValidation="false" OnClick="lbtnBlitzer_Click">
                <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/Connect Plus - Red.jpg"
                    Width="200px" Height="163px" BorderWidth="0px" /><br />
                <asp:Label ID="Label2" runat="server" Text="Blitzer"></asp:Label>
            </asp:LinkButton>
        </td>
    </tr>
</table>
<iframe id="magicFrame" width="0" height="0" style="display: none;"></iframe>
<script language="javascript" type="text/javascript">
    function ChangeTheme(themeName) {
        var l = window.location;
        var base_url = l.protocol + "//" + l.host + "/" + l.pathname.split('/')[1];

        document.getElementById('magicFrame').src = base_url + '/ChangeThemeHidden.aspx?theme=' + themeName;

        document.cookie = "theme=" + themeName;
    }

</script>
