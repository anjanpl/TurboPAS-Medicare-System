<%@ Control Language="vb" CodeBehind="~/UserControl/EDICentral/BillingProvider.ascx.vb"
    Inherits="VisitelWeb.Visitel.UserControl.EDICentral.BillingProviderControl" %>
<asp:UpdatePanel ID="UpdatePanelBillingProvider" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <%--Left[Start]--%>
        <uc1:EditSetting ID="UserControlEditSettingBillingProvider" runat="server" />
        <asp:Label ID="LabelBillingProviderId" runat="server" Visible="false"></asp:Label>
        <div class="ServiceBox">
            <div class="BoxStyle ServiceLeft SectionDiv">
                <div class="newRow">
                    <div class="DivCommonBillingProvider">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelEntityIDCode" runat="server" CssClass="LabelEntityIDCode"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListEntityIdCode" runat="server" CssClass="DropDownListEntityIDCode">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonBillingProvider">
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
                    <div class="DivCommonBillingProvider">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelLastOrOrgName" runat="server" CssClass="LabelLastOrOrgName"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxLastOrOrgName" runat="server" CssClass="TextBoxLastOrOrgName"
                            TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonBillingProvider">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelFirst" runat="server" CssClass="LabelFirst"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxFirst" runat="server" CssClass="TextBoxFirst"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonBillingProvider">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelMiddle" runat="server" CssClass="LabelMiddle"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxMiddle" runat="server" CssClass="TextBoxMiddle"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonBillingProvider">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelPrefix" runat="server" CssClass="LabelPrefix"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxPrefix" runat="server" CssClass="TextBoxPrefix" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonBillingProvider">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelSuffix" runat="server" CssClass="LabelSuffix"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxSuffix" runat="server" CssClass="TextBoxSuffix" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonBillingProvider">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelBillingProviderUpdateDate" runat="server" CssClass="LabelBillingProviderUpdateDate"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxBillingProviderUpdateDate" runat="server" CssClass="TextBoxBillingProviderUpdateDate"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonBillingProvider">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelBillingProviderUpdateBy" runat="server" CssClass="LabelBillingProviderUpdateBy"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxBillingProviderUpdateBy" runat="server" CssClass="TextBoxBillingProviderUpdateBy"></asp:TextBox>
                    </div>
                </div>
                <div class="DivSpace5">
                </div>
            </div>
        </div>
        <%--Left[End]--%>
        <%--Right[Start]--%>
        <div class="ServiceBox">
            <div class="BoxStyle ServiceLeft SectionDiv">
                <div class="newRow">
                    <div class="DivCommonBillingProvider">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelAddress" runat="server" CssClass="LabelAddress"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxAddress" runat="server" CssClass="TextBoxAddress" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonBillingProvider">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelCity" runat="server" CssClass="LabelCity"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxCity" runat="server" CssClass="TextBoxCity"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonBillingProvider">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelState" runat="server" CssClass="LabelState"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxState" runat="server" CssClass="TextBoxState"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonBillingProvider">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelZip" runat="server" CssClass="LabelZip"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxZip" runat="server" CssClass="TextBoxZip"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonBillingProvider">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelReferenceIDQualifier" runat="server" CssClass="LabelReferenceIDQualifier"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListReferenceIdQualifier" runat="server" CssClass="DropDownListReferenceIDQualifier">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonBillingProvider">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelEINOrSSN" runat="server" CssClass="LabelEINOrSSN"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxEINOrSSN" runat="server" CssClass="TextBoxEINOrSSN"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonBillingProvider">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelIDCodeQualifier" runat="server" CssClass="LabelIDCodeQualifier"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListIdCodeQualifier" runat="server" CssClass="DropDownListIDCodeQualifier">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonBillingProvider">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelNPI" runat="server" CssClass="LabelNPI"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxNPI" runat="server" CssClass="TextBoxNPI"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonBillingProvider">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelContractNo" runat="server" CssClass="LabelContractNo"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxContractNo" runat="server" CssClass="TextBoxContractNo"></asp:TextBox>
                    </div>
                </div>
                <div class="DivSpace5">
                </div>
            </div>
        </div>
        <%--Right[End]--%>
    </ContentTemplate>
</asp:UpdatePanel>
<div class="DivSpace5">
</div>
<div class="newRow DivButtonCenter">
    <asp:UpdatePanel ID="UpdatePanelBillingProviderControlActionButtons" runat="server"
        UpdateMode="Always">
        <ContentTemplate>
            <asp:Button ID="ButtonBillingProviderClear" runat="server" CssClass="ButtonBillingProviderClear" />
            <asp:Button ID="ButtonBillingProviderSave" runat="server" CssClass="ButtonBillingProviderSave" />
            <asp:Button ID="ButtonBillingProviderDelete" runat="server" CssClass="ButtonBillingProviderDelete" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ButtonBillingProviderClear" />
            <asp:AsyncPostBackTrigger ControlID="ButtonBillingProviderSave" />
            <asp:AsyncPostBackTrigger ControlID="ButtonBillingProviderDelete" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<div class="DivSpace2">
</div>
