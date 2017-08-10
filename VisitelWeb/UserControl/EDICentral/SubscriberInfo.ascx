<%@ Control Language="vb" CodeBehind="~/UserControl/EDICentral/SubscriberInfo.ascx.vb"
    Inherits="VisitelWeb.Visitel.UserControl.EDICentral.SubscriberInfoControl" %>
<asp:UpdatePanel ID="UpdatePanelSubscriberInfo" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <div class="newRow">
            <uc1:EditSetting ID="UserControlEditSettingSubscriberInfo" runat="server" />
            <asp:Label ID="LabelSubscriberInfoId" runat="server" Visible="false"></asp:Label>
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelPayerResponsibilitySequenceNumberCode" runat="server" CssClass="LabelPayerResponsibilitySequenceNumberCode"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:DropDownList ID="DropDownListPayerResponsibilitySequenceNumberCode" runat="server"
                    CssClass="DropDownListPayerResponsibilitySequenceNumberCode">
                </asp:DropDownList>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelRelationshipCode" runat="server" CssClass="LabelRelationshipCode"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:DropDownList ID="DropDownListRelationshipCode" runat="server" CssClass="DropDownListRelationshipCode">
                </asp:DropDownList>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelGroupOrPolicyNumber" runat="server" CssClass="LabelGroupOrPolicyNumber"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxGroupOrPolicyNumber" runat="server" CssClass="TextBoxGroupOrPolicyNumber"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelGroupOrPlanName" runat="server" CssClass="LabelGroupOrPlanName"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxGroupOrPlanName" runat="server" CssClass="TextBoxGroupOrPlanName"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelInsuranceTypeCode" runat="server" CssClass="LabelInsuranceTypeCode"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxInsuranceTypeCode" runat="server" CssClass="TextBoxInsuranceTypeCode"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelClaimFilingIndicatorCode" runat="server" CssClass="LabelClaimFilingIndicatorCode"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:DropDownList ID="DropDownListClaimFilingIndicatorCode" runat="server" CssClass="DropDownListClaimFilingIndicatorCode">
                </asp:DropDownList>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelEntityIdentificationCode" runat="server" CssClass="LabelEntityIdentificationCode"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxEntityIdentificationCode" runat="server" CssClass="TextBoxEntityIdentificationCode"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelEntityTypeQualifier" runat="server" CssClass="LabelEntityTypeQualifier"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxEntityTypeQualifier" runat="server" CssClass="TextBoxEntityTypeQualifier"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelIdCodeQualifier" runat="server" CssClass="LabelIdCodeQualifier"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxIdCodeQualifier" runat="server" CssClass="TextBoxIdCodeQualifier"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelContractNo" runat="server" CssClass="LabelContractNo"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxContractNo" runat="server" CssClass="TextBoxContractNo"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelSubscriberInfoUpdateDate" runat="server" CssClass="LabelSubscriberInfoUpdateDate"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxSubscriberInfoUpdateDate" runat="server" CssClass="TextBoxSubscriberInfoUpdateDate"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelSubscriberInfoUpdateBy" runat="server" CssClass="LabelSubscriberInfoUpdateBy"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxSubscriberInfoUpdateBy" runat="server" CssClass="TextBoxSubscriberInfoUpdateBy"></asp:TextBox>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<div class="DivSpace5">
</div>
<div class="newRow DivButtonCenter">
    <asp:UpdatePanel ID="UpdatePanelSubmitterActionButtons" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Button ID="ButtonSubscriberInfoClear" runat="server" CssClass="ButtonSubscriberInfoClear" />
            <asp:Button ID="ButtonSubscriberInfoSave" runat="server" CssClass="ButtonSubscriberInfoSave" />
            <asp:Button ID="ButtonSubscriberInfoDelete" runat="server" CssClass="ButtonSubscriberInfoDelete" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ButtonSubscriberInfoClear" />
            <asp:AsyncPostBackTrigger ControlID="ButtonSubscriberInfoSave" />
            <asp:AsyncPostBackTrigger ControlID="ButtonSubscriberInfoDelete" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<div class="DivSpace2">
</div>
