<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="~/UserControl/UCTopBarLoginPanel.ascx.vb"
    Inherits="VisitelWeb.Visitel.UserControl.UCTopBarLoginPanel" %>
<div id="topPanel">
    <div id="Panel">
        <div id="Panel_contents">
        </div>
        <div id="ContactForHelp">
            <br />
            Should you have any query, please do not hesitate to communicate IT Help Line:<br />
            <br />
            Email:
            <br />
            support@kds-tx.com
            <br />
            Phone:
            <br />
            09-8801311 Ext.5315- 5318<br />
            Mobile:
            <br />
            11111111111111, 999999999999, 888888888888
        </div>
        <div id="forgotPasswordBox">
            <br />
            Forgot Password? Input your User ID in the text box below and Click The Send Password
            button.<br />
            <br />
            <asp:ValidationSummary ID="vsError" runat="server" ValidationGroup="ForgotPassword" />
            <table cellpadding="5" cellspacing="5">
                <tr>
                    <td width="90px">
                        <strong>User Name:</strong>
                    </td>
                    <td style="width: 154px">
                        <asp:TextBox ID="txtUserName" MaxLength="20" runat="server" ClientIDMode="Static"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtUserName"
                            Text="*" Display="None" runat="server" ValidationGroup="ForgotPassword" ErrorMessage="User Name can not be empty."></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <%--<asp:Button ID="btnSend" runat="server" ValidationGroup="ForgotPassword" Text="Send Password" ClientIDMode="Static" />--%>
                        <input id="btnSend" type="button" value="Send Password"/>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div class="Panel_button" style="display: block;">
       <%-- <asp:Image ID="Image1" ImageUrl="~/images/expand.png" Width="24" Height="24" runat="server"
            AlternateText="expand" />
        <a href="#">Help</a>--%>
    </div>
    <div class="Panel_button" id="hide_button" style="display: none;">
       <%-- <asp:Image ID="Image2" ImageUrl="~/images/collapse.png" Width="24" Height="24" runat="server"
            AlternateText="expand" />
        <a href="#">Hide</a>--%>
    </div>
</div>

