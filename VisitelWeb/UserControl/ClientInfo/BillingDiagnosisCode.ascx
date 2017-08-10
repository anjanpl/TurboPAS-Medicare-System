<%@ Control Language="vb" CodeBehind="BillingDiagnosisCode.ascx.vb" Inherits="VisitelWeb.Visitel.UserControl.ClientInfo.BillingDiagnosisCodeControl" %>
<div class="ServiceBox">
    <div class="BoxStyle ServiceLeftBillingDiagnosisCode ServiceBoxBillingDiagnosisCode SectionDiv">
        <div id="Div7" class="SectionDiv-header">
            <asp:Label ID="LabelBillingDiagnosisCode" runat="server"></asp:Label>
        </div>
        <asp:UpdatePanel ID="UpdatePanelBillingDiagnosisCode" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="newRow">
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListBillingDiagnosisOne" runat="server" CssClass="DropDownListBillingDiagnosisOne">
                        </asp:DropDownList>
                    </div>
                    <div class="SecondColumn">
                        <asp:DropDownList ID="DropDownListBillingDiagnosisCodeOne" runat="server" CssClass="DropDownListBillingDiagnosisCodeOne">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="newRow">
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListBillingDiagnosisTwo" runat="server" CssClass="DropDownListBillingDiagnosisTwo">
                        </asp:DropDownList>
                    </div>
                    <div class="SecondColumn">
                        <asp:DropDownList ID="DropDownListBillingDiagnosisCodeTwo" runat="server" CssClass="DropDownListBillingDiagnosisCodeTwo">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="newRow">
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListBillingDiagnosisThree" runat="server" CssClass="DropDownListBillingDiagnosisThree">
                        </asp:DropDownList>
                    </div>
                    <div class="SecondColumn">
                        <asp:DropDownList ID="DropDownListBillingDiagnosisCodeThree" runat="server" CssClass="DropDownListBillingDiagnosisCodeThree">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="newRow">
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListBillingDiagnosisFour" runat="server" CssClass="DropDownListBillingDiagnosisFour">
                        </asp:DropDownList>
                    </div>
                    <div class="SecondColumn">
                        <asp:DropDownList ID="DropDownListBillingDiagnosisCodeFour" runat="server" CssClass="DropDownListBillingDiagnosisCodeFour">
                        </asp:DropDownList>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="DropDownListBillingDiagnosisOne" />
                <asp:AsyncPostBackTrigger ControlID="DropDownListBillingDiagnosisCodeOne" />
                <asp:AsyncPostBackTrigger ControlID="DropDownListBillingDiagnosisTwo" />
                <asp:AsyncPostBackTrigger ControlID="DropDownListBillingDiagnosisCodeTwo" />
                <asp:AsyncPostBackTrigger ControlID="DropDownListBillingDiagnosisThree" />
                <asp:AsyncPostBackTrigger ControlID="DropDownListBillingDiagnosisCodeThree" />
                <asp:AsyncPostBackTrigger ControlID="DropDownListBillingDiagnosisFour" />
                <asp:AsyncPostBackTrigger ControlID="DropDownListBillingDiagnosisCodeFour" />
            </Triggers>
        </asp:UpdatePanel>
        <div class="DivSpace10">
        </div>
    </div>
</div>
<%--<asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>--%>