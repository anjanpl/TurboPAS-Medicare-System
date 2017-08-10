<%@ Control Language="vb" CodeBehind="~/UserControl/PayPeriod/HospitalizedIndividuals.ascx.vb"
    Inherits="VisitelWeb.Visitel.UserControl.PayPeriod.HospitalizedIndividualsControl" %>
<div class="newRow HorizontalScroll">
    <asp:UpdatePanel ID="UpdatePanelHospitalizedIndividuals" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:SqlDataSource ID="SqlDataSourceHospitalizedIndividuals" runat="server"></asp:SqlDataSource>
            <asp:GridView ID="GridViewHospitalStay" runat="server" Width="100%">
                <EmptyDataTemplate>
                    <asp:Label ID="LabelNoDataFound" Text="No Data Found" runat="server"></asp:Label>
                </EmptyDataTemplate>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="GridViewHospitalStay" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<div class="DivSpace10">
</div>
