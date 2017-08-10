<%@ Control Language="vb" CodeBehind="~/UserControl/ClientInfo/EVV.ascx.vb" Inherits="VisitelWeb.Visitel.UserControl.ClientInfo.EVVControl" %>
<div class="ServiceBox">
    <div class="BoxStyle ServiceLeftEVV ServiceBoxEVV SectionDiv">
        <div id="Div6" class="SectionDiv-header">
            <asp:Label ID="LabelEVV" runat="server"></asp:Label>
        </div>
        <asp:UpdatePanel ID="UpdatePanelEVV" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="newRow">
                    <div class="EVVColumn">
                        <asp:Label ID="LabelEVVClientId" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:TextBox ID="TextBoxEVVClientId" runat="server" CssClass="TextBoxEVVClientId"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="EVVColumn">
                        <asp:Label ID="LabelEVVARNumber" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:TextBox ID="TextBoxEVVARNumber" runat="server" CssClass="TextBoxEVVARNumber"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="EVVColumn">
                        <asp:Label ID="LabelEVVPriority" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:DropDownList ID="DropDownListEVVPriority" runat="server" CssClass="DropDownListEVVPriority">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="newRow">
                    <div class="EVVColumn">
                        <asp:Label ID="LabelEVVBillCode" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:TextBox ID="TextBoxEVVBillCode" runat="server" CssClass="TextBoxEVVBillCode"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="EVVColumn">
                        <asp:Label ID="LabelEVVProcCodeQualifier" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:TextBox ID="TextBoxEVVProcCodeQualifier" runat="server" CssClass="TextBoxEVVProcCodeQualifier"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="EVVColumn">
                        <asp:Label ID="LabelEVVLandPhone" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:TextBox ID="TextBoxEVVLandPhone" runat="server" CssClass="TextBoxEVVLandPhone"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorEVVLandPhone" runat="server"></asp:RegularExpressionValidator>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="DivSpace3">
        </div>
    </div>
</div>
