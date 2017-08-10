<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="~/UserControl/ClientInfo/CaseInformation.ascx.vb"
    Inherits="VisitelWeb.Visitel.UserControl.ClientInfo.CaseInformationControl" %>
<div id="Div16" class="SectionDiv" runat="server">
    <div id="Div14" class="SectionDiv-header">
        <asp:Label ID="LabelCaseInformation" runat="server"></asp:Label>
        <uc1:EditSetting ID="UserControlEditSettingCaseInformation" runat="server" />
    </div>
</div>
<div class="newRow">
</div>
<div class="newRow">
    <div class="SecondColumn DivCaseInformationCaseWorker">
        <asp:Label ID="LabelCaseInformationCaseWorker" runat="server"></asp:Label>
    </div>
    <div class="SecondColumn DivCaseInformationEmployee">
        <asp:Label ID="LabelCaseInformationEmployee" runat="server"></asp:Label>
    </div>
    <div class="SecondColumn DivCaseInformationDate">
        <asp:Label ID="LabelCaseInformationDate" runat="server"></asp:Label>
    </div>
    <div class="SecondColumn DivCaseInformationReceiverOrganization">
        <asp:Label ID="LabelCaseInformationReceiverOrganization" runat="server"></asp:Label>
    </div>
    <div class="SecondColumn DivCaseInformationUpdateDate">
        <asp:Label ID="LabelCaseInformationUpdateDate" runat="server"></asp:Label>
    </div>
    <div class="SecondColumn DivCaseInformationUpdateBy">
        <asp:Label ID="LabelCaseInformationUpdateBy" runat="server"></asp:Label>
    </div>
</div>
<div class="newRow">
</div>
<div class="newRow">
    <asp:UpdatePanel ID="UpdatePanelCaseInformation" runat="server" UpdateMode="Always"
        ClientIDMode="Static">
        <ContentTemplate>
            <asp:HiddenField ID="HiddenFieldCaseInfoId" runat="server" ClientIDMode="Static" />
            <asp:PlaceHolder ID="PlaceHolderCaseInformation" runat="server"></asp:PlaceHolder>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<div class="DivSpace3">
</div>
<div class="newRow DivButtonCenter">
    <asp:UpdatePanel ID="UpdatePanelCaseInfoActionButtons" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Button ID="ButtonFaxCoverPageReport" runat="server" CssClass="ButtonFaxCoverPageReport" />
            <asp:Button ID="ButtonOpenReport" runat="server" CssClass="ButtonOpenReport" />
            <asp:Button ID="ButtonCaseInformationClear" runat="server" CssClass="ButtonCaseInformationClear" />
            <asp:Button ID="ButtonCaseInformationSave" runat="server" CssClass="ButtonCaseInformationSave" />
            <asp:Button ID="ButtonCaseInformationDelete" runat="server" CssClass="ButtonCaseInformationDelete" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ButtonFaxCoverPageReport" />
            <asp:AsyncPostBackTrigger ControlID="ButtonOpenReport" />
            <asp:AsyncPostBackTrigger ControlID="ButtonCaseInformationClear" />
            <asp:AsyncPostBackTrigger ControlID="ButtonCaseInformationSave" />
            <asp:AsyncPostBackTrigger ControlID="ButtonCaseInformationDelete" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<div class="DivSpace10">
</div>
