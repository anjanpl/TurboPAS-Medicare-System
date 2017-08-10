<%@ Control Language="vb" CodeBehind="~/UserControl/UCChangePassword.ascx.vb"
    Inherits="VisitelWeb.Visitel.UserControl.UCChangePassword" %>
<%--<asp:ValidationSummary ID="vsError" runat="server" CssClass="errorMessage" ValidationGroup="UCChangePassword"
    ForeColor="#C90202" Width="100%" />--%>
<asp:Label ID="lblErrMsg" runat="server" Text="" Style="display: none" CssClass="errorMessage"></asp:Label>
<table cellpadding="5" cellspacing="5" width="100%">
    <tr>
        <td align="left" width="140px">
            <strong>Old Passord:</strong>
        </td>
    </tr>
    <tr>
        <td>
            <asp:TextBox ID="txtOldPass" runat="server" TextMode="Password" MaxLength="20" Width="170px" ClientIDMode="Static"></asp:TextBox>
            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="UCChangePassword"
                ControlToValidate="txtOldPass" Text="*" Display="Dynamic" runat="server" ErrorMessage="Old Passord can not be empty."></asp:RequiredFieldValidator>--%>
        </td>
    </tr>
    <tr>
        <td align="left">
            <strong>New Passord:</strong>
        </td>
    </tr>
    <tr>
        <td>
            <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" MaxLength="20" ClientIDMode="Static"
                Width="170px"></asp:TextBox>
            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtNewPassword"
                ValidationGroup="UCChangePassword" Text="*" Display="Dynamic" runat="server"
                ErrorMessage="New Passord can not be empty."></asp:RequiredFieldValidator>
            <asp:CustomValidator ID="cvPassword" ControlToValidate="txtNewPassword" Display="Dynamic"
                Text="*" runat="server" ErrorMessage="Invalid Password" ValidationGroup="UCChangePassword"
                ClientValidationFunction="validatePassword"></asp:CustomValidator>--%>
        </td>
    </tr>
    <tr>
        <td align="left">
            <strong>Retype New Passord:</strong>
        </td>
    </tr>
    <tr>
        <td>
            <asp:TextBox ID="txtNewPassRetype" runat="server" TextMode="Password" MaxLength="20" ClientIDMode="Static"
                Width="170px"></asp:TextBox>
            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="UCChangePassword"
                ControlToValidate="txtNewPassRetype" Text="*" Display="Dynamic" runat="server"
                ErrorMessage="Retype New Passord can not be empty."></asp:RequiredFieldValidator>
            <asp:CompareValidator ID="CompareValidator1" ControlToValidate="txtNewPassRetype"
                ControlToCompare="txtNewPassword" runat="server" Text="*" ErrorMessage="New Password and Retype New Password must be same."></asp:CompareValidator>--%>
        </td>
    </tr>
    <tr>
        <td>
            <%--<asp:Button ID="btnChange" runat="server" Text="Change Password" ValidationGroup="UCChangePassword" ClientIDMode="Static" />--%>
            <input id="btnChange" type="button" value="Change Password"/>

        </td>
    </tr>
</table>
<asp:HiddenField ID="hdnUserID" runat="server" Value="" ClientIDMode="Static" />
