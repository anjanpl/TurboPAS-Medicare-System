<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="~/UserControl/PayPeriod/CareSummaryPayPeriodDetail.ascx.vb"
    Inherits="VisitelWeb.Visitel.UserControl.PayPeriod.CareSummaryPayPeriodDetailControl" %>
<div class="newRow HorizontalScroll">
    <asp:UpdatePanel ID="UpdatePanelHospitalizedIndividuals" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:SqlDataSource ID="SqlDataSourceCareSummaryPayPeriod" runat="server"></asp:SqlDataSource>
            <asp:GridView ID="GridViewCareSummaryPayPeriodDetail" runat="server" Width="100%">
                <EmptyDataTemplate>
                    <asp:Label ID="LabelNoDataFound" Text="No Data Found" runat="server"></asp:Label>
                </EmptyDataTemplate>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="GridViewCareSummaryPayPeriodDetail" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<asp:UpdatePanel ID="UpdatePanelTime" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <div class="AutoColumn">
            <asp:Label ID="LabelTotalTime" runat="server" Text="Total Time" CssClass="LabelTotalTime"></asp:Label>
        </div>
        <div class="AutoColumn">
            <asp:TextBox ID="TextBoxTotalTime" runat="server" CssClass="TextBoxTotalTime"></asp:TextBox>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<div class="DivSpace10">
</div>
