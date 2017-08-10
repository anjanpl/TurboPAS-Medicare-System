<%@ Control Language="vb" CodeBehind="~/UserControl/ClientInfo/EVVInfo.ascx.vb" Inherits="VisitelWeb.Visitel.UserControl.ClientInfo.EVVInfoControl" %>
<div class="ServiceBox">
    <div class="BoxStyle ServiceLeftEVVInformation ServiceBoxEVVInformation SectionDiv">
        <div id="Div2" class="SectionDiv-header">
            <asp:Label ID="LabelEVVInformation" runat="server"></asp:Label>
        </div>
        <asp:UpdatePanel ID="UpdatePanelEVVInformation" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="newRow">
                    <div class="AutoColumn DivProgramOrService">
                        <asp:Label ID="LabelProgramOrService" runat="server" CssClass="LabelProgramOrService"></asp:Label>
                    </div>
                    <div class="AutoColumn DivServiceGroup">
                        <asp:Label ID="LabelServiceGroup" runat="server" CssClass="LabelServiceGroup"></asp:Label>
                    </div>
                    <div class="AutoColumn DivServiceCode">
                        <asp:Label ID="LabelServiceCode" runat="server" CssClass="LabelServiceCode"></asp:Label>
                    </div>
                    <div class="AutoColumn DivServiceCodeDescription">
                        <asp:Label ID="LabelServiceCodeDescription" runat="server" CssClass="LabelServiceCodeDescription"></asp:Label>
                    </div>
                    <div class="AutoColumn DivHCPCS">
                        <asp:Label ID="LabelHCPCS" runat="server" CssClass="LabelHCPCS"></asp:Label>
                    </div>
                    <div class="AutoColumn DivModifiers">
                        <asp:Label ID="LabelModifiers" runat="server" CssClass="LabelModifiers"></asp:Label>
                    </div>
                </div>
                <div class="newRow">
                    <div class="AutoColumn DivProgramOrService">
                        <asp:DropDownList ID="DropDownListProgramOrService" runat="server" CssClass="DropDownListProgramOrService">
                        </asp:DropDownList>
                    </div>
                    <div class="AutoColumn DivServiceGroup">
                        <asp:DropDownList ID="DropDownListServiceGroup" runat="server" CssClass="DropDownListServiceGroup">
                        </asp:DropDownList>
                    </div>
                    <div class="AutoColumn DivServiceCode">
                        <asp:DropDownList ID="DropDownListServiceCode" runat="server" CssClass="DropDownListServiceCode">
                        </asp:DropDownList>
                    </div>
                    <div class="AutoColumn DivServiceCodeDescription">
                        <asp:DropDownList ID="DropDownListServiceCodeDescription" runat="server" CssClass="DropDownListServiceCodeDescription">
                        </asp:DropDownList>
                    </div>
                    <div class="AutoColumn DivHCPCS">
                        <asp:TextBox ID="TextBoxHCPCS" runat="server" CssClass="TextBoxHCPCS"></asp:TextBox>
                    </div>
                    <div class="AutoColumn DivModifiers">
                        <asp:TextBox ID="TextBoxModifierOne" runat="server" CssClass="TextBoxModifierOne"></asp:TextBox>
                        <asp:TextBox ID="TextBoxModifierTwo" runat="server" CssClass="TextBoxModifierTwo"></asp:TextBox>
                        <asp:TextBox ID="TextBoxModifierThree" runat="server" CssClass="TextBoxModifierThree"></asp:TextBox>
                        <asp:TextBox ID="TextBoxModifierFour" runat="server" CssClass="TextBoxModifierFour"></asp:TextBox>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="DropDownListProgramOrService" />
                <asp:AsyncPostBackTrigger ControlID="DropDownListServiceGroup" />
                <asp:AsyncPostBackTrigger ControlID="DropDownListServiceCode" />
                <asp:AsyncPostBackTrigger ControlID="DropDownListServiceCodeDescription" />
            </Triggers>
        </asp:UpdatePanel>
        <div class="DivSpace3">
        </div>
    </div>
</div>
