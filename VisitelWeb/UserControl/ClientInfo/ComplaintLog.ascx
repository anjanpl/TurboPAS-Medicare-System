<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="~/UserControl/ClientInfo/ComplaintLog.ascx.vb"
    Inherits="VisitelWeb.Visitel.UserControl.ClientInfo.ComplaintLogControl" %>
<div class="SectionDiv-header">
    <asp:Label ID="LabelTitle" runat="server"></asp:Label>
    <uc1:EditSetting ID="UserControlEditSettingComplainLog" runat="server" />
</div>
<asp:UpdatePanel ID="UpdatePanelComplaintLog" runat="server" UpdateMode="Always"
    ClientIDMode="Static">
    <ContentTemplate>
        <div id="MainDiv" class="newRow" runat="server">
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<div class="newRow">
</div>
<div class="newRow DivButtonCenter">
    <asp:UpdatePanel ID="UpdatePanelComplaintLogActionButtons" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:HiddenField ID="HiddenFieldComplaintLogId" runat="server" />
            <asp:HiddenField ID="HiddenFieldComplaintLogIsNew" runat="server" />
            <asp:HiddenField ID="HiddenFieldComplaintLogIndex" runat="server" />
            <asp:Button ID="ButtonClearComplaintLog" runat="server" CssClass="ButtonClearComplaintLog" />
            <asp:Button ID="ButtonOpenReportComplaintLog" runat="server" CssClass="ButtonOpenReportComplaintLog"
                OnClientClick="showComplaintLogReport(); return false;" UseSubmitBehavior="false" />
            <asp:Button ID="ButtonSaveComplaintLog" runat="server" CssClass="ButtonSaveComplaintLog" />
            <asp:Button ID="ButtonDeleteComplaintLog" runat="server" CssClass="ButtonDeleteComplaintLog" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ButtonClearComplaintLog" />
            <asp:AsyncPostBackTrigger ControlID="ButtonOpenReportComplaintLog" />
            <asp:AsyncPostBackTrigger ControlID="ButtonSaveComplaintLog" />
            <asp:AsyncPostBackTrigger ControlID="ButtonDeleteComplaintLog" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<div class="divSpace10">
</div>
