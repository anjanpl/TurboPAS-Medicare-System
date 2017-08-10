<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="ClientComments.ascx.vb"
    Inherits="VisitelWeb.Visitel.UserControl.ClientInfo.ClientCommentsControl" %>
<div class="SectionDiv-header">
    <asp:Label ID="LabelTitle" runat="server"></asp:Label>
    <uc1:EditSetting ID="UserControlEditSettingClientComments" runat="server" />
</div>
<asp:UpdatePanel ID="UpdatePanelComments" runat="server" UpdateMode="Always" ClientIDMode="Static">
    <ContentTemplate>
        <div id="MainDiv" class="newRow" runat="server">
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<div class="newRow">
</div>
<div class="newRow DivButtonCenter">
    <asp:UpdatePanel ID="UpdatePanelClientCommentActionButtons" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:HiddenField ID="HiddenFieldCommentId" runat="server" />
            <asp:HiddenField ID="HiddenFieldCommentIsNew" runat="server" />
            <asp:HiddenField ID="HiddenFieldCommentIndex" runat="server" />
            <asp:Button ID="ButtonClearComment" runat="server" CssClass="ButtonClearComment" />
            <%--<asp:LinkButton ID="ButtonCommunicationNotesComment" runat="server" Enabled="false" ClientIDMode="Static" CssClass="ButtonCommunicationNotesComment"></asp:LinkButton>--%>
            <asp:Button ID="ButtonCommunicationNotesComment" runat="server" CssClass="ButtonCommunicationNotesComment"
                OnClientClick="showClientCommunicationNotes(); return false;" UseSubmitBehavior="false" />
            <asp:Button ID="ButtonSaveComment" runat="server" CssClass="ButtonSaveComment" />
            <asp:Button ID="ButtonDeleteComment" runat="server" CssClass="ButtonDeleteComment" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ButtonClearComment" />
            <asp:AsyncPostBackTrigger ControlID="ButtonCommunicationNotesComment" />
            <asp:AsyncPostBackTrigger ControlID="ButtonSaveComment" />
            <asp:AsyncPostBackTrigger ControlID="ButtonDeleteComment" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<div class="divSpace10">
</div>
