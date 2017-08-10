<%@ Control Language="vb" CodeBehind="AuthorizationDetail.ascx.vb" Inherits="VisitelWeb.Visitel.UserControl.ClientInfo.AuthorizationDetailControl" %>
<div class="ServiceBox">
    <div class="BoxStyle ServiceLeftAuthorizationDetail ServiceBoxAuthorizationDetail SectionDiv">
        <div id="Div1" class="SectionDiv-header">
            <asp:Label ID="LabelAuthorizationDetail" runat="server"></asp:Label>
        </div>
        <div class="newRow">
            <asp:UpdatePanel ID="UpdatePanelAuthorizationDetail" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <div class="newRow">
                        <div class="AuthorizationDetailColumn1">
                            <asp:Label ID="LabelAuthorizatioReceivedDate" runat="server"></asp:Label>
                        </div>
                        <div class="SecondColumn">
                            <asp:TextBox ID="TextBoxAuthorizatioReceivedDate" runat="server" CssClass="dateField TextBoxAuthorizatioReceivedDate"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorAuthorizatioReceivedDate"
                                runat="server"></asp:RegularExpressionValidator>
                        </div>
                        <div class="AuthorizationDetailColumn2">
                            <asp:Label ID="LabelPractitionerStatementReceivedDate" runat="server"></asp:Label>
                        </div>
                        <div class="SecondColumn">
                            <asp:TextBox ID="TextBoxPractitionerStatementReceivedDate" runat="server" CssClass="dateField TextBoxPractitionerStatementReceivedDate"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorPractitionerStatementReceivedDate"
                                runat="server"></asp:RegularExpressionValidator>
                        </div>
                    </div>
                    <div class="newRow">
                        <div class="AuthorizationDetailColumn1">
                            <asp:Label ID="LabelPractitionerStatementSentDate" runat="server"></asp:Label>
                        </div>
                        <div class="SecondColumn">
                            <asp:TextBox ID="TextBoxPractitionerStatementSentDate" runat="server" CssClass="dateField TextBoxPractitionerStatementSentDate"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorPractitionerStatementSentDate"
                                runat="server"></asp:RegularExpressionValidator>
                        </div>
                        <div class="AuthorizationDetailColumn2">
                            <asp:Label ID="LabelServiceInitializedReportedDate" runat="server"></asp:Label>
                        </div>
                        <div class="SecondColumn">
                            <asp:TextBox ID="TextBoxServiceInitializedReportedDate" runat="server" CssClass="dateField TextBoxServiceInitializedReportedDate"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorServiceInitializedReportedDate"
                                runat="server"></asp:RegularExpressionValidator>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="DivSpace3">
        </div>
    </div>
</div>
