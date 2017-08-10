<%@ Control Language="vb" CodeBehind="EmergencyContract.ascx.vb" Inherits="VisitelWeb.Visitel.UserControl.ClientInfo.EmergencyContractControl" %>
<div class="ServiceBox">
    <div class="BoxStyle ServiceLeftEmergencyContract ServiceBoxEmergencyContract SectionDiv">
        <div id="Div5" class="SectionDiv-header">
            <asp:Label ID="LabelEmergencyContact" runat="server"></asp:Label>
        </div>
        <asp:UpdatePanel ID="UpdatePanelEmergencyContact" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="newRow">
                    <div class="DivCommonEmergencyContact">
                        <div class="AutoColumn FloatLeft">
                            <asp:Label ID="LabelEmergencyContactOneName" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxEmergencyContactOneName" runat="server" CssClass="TextBoxEmergencyContactOneName"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonEmergencyContact">
                        <div class="AutoColumn FloatLeft">
                            <asp:Label ID="LabelEmergencyContactOnePhone" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxEmergencyContactOnePhone" runat="server" CssClass="TextBoxEmergencyContactOnePhone"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorEmergencyContactOnePhone"
                            runat="server"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonEmergencyContact">
                        <div class="AutoColumn FloatLeft">
                            <asp:Label ID="LabelEmergencyContactOneRelationship" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxEmergencyContactOneRelationship" runat="server" CssClass="TextBoxEmergencyContactOneRelationship"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                </div>
                <div class="newRow">
                    <div class="DivCommonEmergencyContact">
                        <div class="AutoColumn FloatLeft">
                            <asp:Label ID="LabelEmergencyContactTwoName" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxEmergencyContactTwoName" runat="server" CssClass="TextBoxEmergencyContactTwoName"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonEmergencyContact">
                        <div class="AutoColumn FloatLeft">
                            <asp:Label ID="LabelEmergencyContactTwoPhone" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxEmergencyContactTwoPhone" runat="server" CssClass="TextBoxEmergencyContactTwoPhone"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorEmergencyContactTwoPhone"
                            runat="server"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonEmergencyContact">
                        <div class="AutoColumn FloatLeft">
                            <asp:Label ID="LabelEmergencyContactTwoRelationship" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxEmergencyContactTwoRelationship" runat="server" CssClass="TextBoxEmergencyContactTwoRelationship"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonEmergencyContact">
                        <div class="AutoColumn FloatLeft">
                            <asp:Label ID="LabelEmergencyDisasterCategory" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListEmergencyDisasterCategory" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="DivSpace8">
        </div>
    </div>
</div>
