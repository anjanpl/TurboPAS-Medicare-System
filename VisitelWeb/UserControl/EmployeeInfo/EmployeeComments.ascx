<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="EmployeeComments.ascx.vb"
    Inherits="VisitelWeb.Visitel.UserControl.EmployeeInfo.EmployeeCommentsControl" %>
<div class="SectionDiv-header">
    <asp:Label ID="LabelTitle" runat="server"></asp:Label>
    <uc1:EditSetting ID="EditSettingEmployeeComments" runat="server" />
</div>
<asp:UpdatePanel ID="UpdatePanelComments" runat="server" UpdateMode="Always" ClientIDMode="Static">
    <ContentTemplate>
        <div id="MainDiv" class="newRow" runat="server">
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<div class="newRow">
    <asp:HiddenField ID="HiddenFieldCommentId" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="HiddenFieldCommentIsNew" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="HiddenFieldCommentIndex" runat="server" ClientIDMode="Static" />
</div>
<%--<div class="newRow divCommentButtonCenter">
    <div class="columnAuto columnLeft">
        <asp:Button ID="ButtonClearComment" runat="server" CssClass="ButtonClearComment"></asp:Button>
    </div>
    <asp:UpdatePanel ID="UpdatePanelCommunicationNotesComment" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="columnAuto columnLeft">
                <asp:Button ID="ButtonCommunicationNotesComment" runat="server" CssClass="ButtonCommunicationNotesComment" OnClientClick="showEmployeeCommunicationNotes(); return false;" UseSubmitBehavior="false" ClientIDMode="Static" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="columnAuto columnLeft">
        <asp:Button ID="ButtonSaveComment" runat="server" CssClass="ButtonSaveComment"></asp:Button>
    </div>
    <div class="columnAuto columnLeft">
        <asp:Button ID="ButtonDeleteComment" runat="server" CssClass="ButtonDeleteComment"></asp:Button>
    </div>
</div>--%>
<div class="newRow DivButtonCenter">
    <asp:UpdatePanel ID="UpdatePanelCommentsActionButtons" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Button ID="ButtonClearComment" runat="server" CssClass="ButtonClearComment">
            </asp:Button>
            <asp:Button ID="ButtonCommunicationNotesComment" runat="server" CssClass="ButtonCommunicationNotesComment"
                OnClientClick="showEmployeeCommunicationNotes(); return false;" UseSubmitBehavior="false"
                ClientIDMode="Static" />
            <asp:Button ID="ButtonSaveComment" runat="server" CssClass="ButtonSaveComment"></asp:Button>
            <asp:Button ID="ButtonDeleteComment" runat="server" CssClass="ButtonDeleteComment">
            </asp:Button>
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
