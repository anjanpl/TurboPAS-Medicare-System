<%@ Control Language="vb" CodeBehind="CurrentAuthorizationPeriod.ascx.vb" Inherits="VisitelWeb.Visitel.UserControl.ClientInfo.CurrentAuthorizationPeriodControl" %>
<div class="ServiceBox">
    <div class="BoxStyle ServiceLeftAuthorization ServiceBoxAuthorization SectionDiv">
        <div id="Div4" class="SectionDiv-header">
            <asp:Label ID="LabelAuthorization" runat="server"></asp:Label>
        </div>
        <asp:UpdatePanel ID="UpdatePanelAuthorization" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="newRow">
                    <div class="DivCommonAuthorization">
                        <div class="AutoColumn FloatLeft">
                            <asp:Label ID="LabelAuthorizationFromDate" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxAuthorizationFromDate" runat="server" CssClass="dateField TextBoxAuthorizationFromDate"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorAuthorizationFromDate"
                            runat="server"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonAuthorization">
                        <div class="AutoColumn FloatLeft">
                            <asp:Label ID="LabelAuthorizationToDate" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxAuthorizationToDate" runat="server" CssClass="dateField TextBoxAuthorizationToDate"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorAuthorizationToDate"
                            runat="server"></asp:RegularExpressionValidator>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="DivSpace3">
        </div>
    </div>
</div>
