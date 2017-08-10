<%@ Control Language="vb" CodeBehind="~/UserControl/EDICentral/Submitter.ascx.vb"
    Inherits="VisitelWeb.Visitel.UserControl.EDICentral.SubmitterControl" %>
<asp:UpdatePanel ID="UpdatePanelSubmitter" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <div class="newRow">
            <uc1:EditSetting ID="UserControlEditSettingSubmitter" runat="server" />
            <asp:Label ID="LabelSubmitterId" runat="server" Visible="false"></asp:Label>
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelEntityIdentifierCode" runat="server" CssClass="LabelEntityIdentifierCode"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:DropDownList ID="DropDownListEntityIdentifierCode" runat="server" CssClass="DropDownListEntityIdentifierCode">
                </asp:DropDownList>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelEntityTypeQualifier" runat="server" CssClass="LabelEntityTypeQualifier"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:DropDownList ID="DropDownListEntityTypeQualifier" runat="server" CssClass="DropDownListEntityTypeQualifier">
                </asp:DropDownList>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelNameLastOrOrganizationName" runat="server" CssClass="LabelNameLastOrOrganizationName"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxNameLastOrOrganizationName" runat="server" TextMode="MultiLine"
                    CssClass="TextBoxNameLastOrOrganizationName"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelFirstName" runat="server" CssClass="LabelFirstName"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxFirstName" runat="server" CssClass="TextBoxFirstName"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelMiddleName" runat="server" CssClass="LabelMiddleName"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxMiddleName" runat="server" CssClass="TextBoxMiddleName"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelPrefix" runat="server" CssClass="LabelPrefix"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxPrefix" runat="server" CssClass="TextBoxPrefix"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelSuffix" runat="server" CssClass="LabelSuffix"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxSuffix" runat="server" CssClass="TextBoxSuffix"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelPrimaryIdentificationNumber" runat="server" CssClass="LabelPrimaryIdentificationNumber"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxPrimaryIdentificationNumber" runat="server" CssClass="TextBoxPrimaryIdentificationNumber"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelContractName" runat="server" CssClass="LabelContractName"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxContractName" runat="server" CssClass="TextBoxContractName"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelPhone" runat="server" CssClass="LabelPhone"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxPhone" runat="server" CssClass="TextBoxPhone"></asp:TextBox>
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
                    <asp:Label ID="LabelUpdateDate" runat="server" CssClass="LabelUpdateDate"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxUpdateDate" runat="server" CssClass="TextBoxUpdateDate"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelUpdateBy" runat="server" CssClass="LabelUpdateBy"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxUpdateBy" runat="server" CssClass="TextBoxUpdateBy"></asp:TextBox>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<div class="DivSpace5">
</div>
<div class="newRow DivButtonCenter">
    <asp:UpdatePanel ID="UpdatePanelSubmitterActionButtons" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Button ID="ButtonSubmitterClear" runat="server" CssClass="ButtonSubmitterClear" />
            <asp:Button ID="ButtonSubmitterSave" runat="server" CssClass="ButtonSubmitterSave" />
            <asp:Button ID="ButtonSubmitterDelete" runat="server" CssClass="ButtonSubmitterDelete" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ButtonSubmitterClear" />
            <asp:AsyncPostBackTrigger ControlID="ButtonSubmitterSave" />
            <asp:AsyncPostBackTrigger ControlID="ButtonSubmitterDelete" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<div class="DivSpace2">
</div>
