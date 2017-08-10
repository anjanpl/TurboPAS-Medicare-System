<%@ Control Language="vb" CodeBehind="PayPeriodGenerator.ascx.vb" Inherits="VisitelWeb.Visitel.UserControl.PayPeriod.PayPeriodGeneratorControl" %>
<div id="Div10" class="SectionDiv" runat="server">
    <div id="Div8" class="SectionDiv-header">
        <asp:Label ID="LabelPayPeriodGeneratorInfo" runat="server"></asp:Label>
        <uc1:EditSetting ID="UserControlEditSetting" runat="server" />
    </div>
</div>
<asp:UpdatePanel ID="UpdatePanelPayPeriodGenerator" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <div class="newRow">
            <div id="dialog-confirm">
            </div>
            <div class="PayPeriodColumn1">
                <asp:Label ID="LabelPayPeriod" runat="server"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:TextBox ID="TextBoxPayPeriodFromDate" runat="server" CssClass="dateField"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorPayPeriodFromDate" runat="server"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidatorPayPeriodFromDate"
                    runat="server"></asp:RegularExpressionValidator>
            </div>
            <div class="SecondColumn">
                <asp:TextBox ID="TextBoxPayPeriodToDate" runat="server" CssClass="dateField"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidatorPayPeriodToDate" runat="server"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidatorPayPeriodToDate" runat="server"></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="newRow">
            <div class="PayPeriodColumn1">
                <asp:Label ID="LabelEmployeeName" runat="server"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:DropDownList ID="DropDownListClient" runat="server" CssClass="DropDownListClient">
                </asp:DropDownList>
            </div>
        </div>
        <div class="DivSpace10">
        </div>
        <div class="newRow">
            <div class="AutoColumn">
                <asp:Button ID="ButtonRun" runat="server" CssClass="ButtonRun" />
            </div>
            <div class="AutoColumn">
                <asp:Button ID="ButtonClear" runat="server" CssClass="ButtonClear" />
            </div>
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="DropDownListClient" />
        <asp:PostBackTrigger ControlID="ButtonRun" />
        <asp:AsyncPostBackTrigger ControlID="ButtonClear" />
    </Triggers>
</asp:UpdatePanel>
<div class="newRow">
</div>
