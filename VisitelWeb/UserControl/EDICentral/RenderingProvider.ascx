<%@ Control Language="vb" CodeBehind="~/UserControl/EDICentral/RenderingProvider.ascx.vb"
    Inherits="VisitelWeb.Visitel.UserControl.EDICentral.RenderingProviderControl" %>
<asp:UpdatePanel ID="UpdatePanelRenderingProvider" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <%--Left[Start]--%>
        <uc1:EditSetting ID="UserControlEditSettingRenderingProvider" runat="server" />
        <asp:Label ID="LabelRenderingProviderId" runat="server" Visible="false"></asp:Label>
        <div class="ServiceBox">
            <div class="BoxStyle ServiceLeft SectionDiv">
                <div class="newRow">
                    <div class="DivCommonRenderingProvider">
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
                    <div class="DivCommonRenderingProvider">
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
                    <div class="DivCommonRenderingProvider">
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
                    <div class="DivCommonRenderingProvider">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelFirst" runat="server" CssClass="LabelFirst"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxFirst" runat="server" CssClass="TextBoxFirst"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonRenderingProvider">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelMiddle" runat="server" CssClass="LabelMiddle"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxMiddle" runat="server" CssClass="TextBoxMiddle"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonRenderingProvider">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelPrefix" runat="server" CssClass="LabelPrefix"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxPrefix" runat="server" CssClass="TextBoxPrefix" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonRenderingProvider">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelSuffix" runat="server" CssClass="LabelSuffix"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxSuffix" runat="server" CssClass="TextBoxSuffix" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonRenderingProvider">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelReferringProviderUpdateDate" runat="server" CssClass="LabelReferringProviderUpdateDate"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxReferringProviderUpdateDate" runat="server" CssClass="TextBoxReferringProviderUpdateDate"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonRenderingProvider">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelReferringProviderUpdateBy" runat="server" CssClass="LabelReferringProviderUpdateBy"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxReferringProviderUpdateBy" runat="server" CssClass="TextBoxReferringProviderUpdateBy"></asp:TextBox>
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
                    <div class="DivCommonRenderingProvider">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelAddress" runat="server" CssClass="LabelAddress"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxAddress" runat="server" CssClass="TextBoxAddress" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonRenderingProvider">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelCity" runat="server" CssClass="LabelCity"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxCity" runat="server" CssClass="TextBoxCity"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonRenderingProvider">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelState" runat="server" CssClass="LabelState"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxState" runat="server" CssClass="TextBoxState"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonRenderingProvider">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelZip" runat="server" CssClass="LabelZip"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxZip" runat="server" CssClass="TextBoxZip"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonRenderingProvider">
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
                    <div class="DivCommonRenderingProvider">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelEINOrSSN" runat="server" CssClass="LabelEINOrSSN"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxEINOrSSN" runat="server" CssClass="TextBoxEINOrSSN"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonRenderingProvider">
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
                    <div class="DivCommonRenderingProvider">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelRenderingProviderIdentifier" runat="server" CssClass="LabelRenderingProviderIdentifier"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxRenderingProviderIdentifier" runat="server" CssClass="TextBoxRenderingProviderIdentifier"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonRenderingProvider">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelContractNo" runat="server" CssClass="LabelContractNo"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxContractNo" runat="server" CssClass="TextBoxContractNo"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonRenderingProvider">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelProviderCode" runat="server" CssClass="LabelProviderCode"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListProviderCode" runat="server" CssClass="DropDownListProviderCode">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSourceProviderCode" runat="server"></asp:SqlDataSource>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonRenderingProvider">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelRefIdQualifier" runat="server" CssClass="LabelRefIdQualifier"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListRefIdQualifier" runat="server" CssClass="DropDownListRefIdQualifier">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSourceRefIdQualifier" runat="server"></asp:SqlDataSource>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonRenderingProvider">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelTaxonomyCode" runat="server" CssClass="LabelTaxonomyCode"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxTaxonomyCode" runat="server" CssClass="TextBoxTaxonomyCode"></asp:TextBox>
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
    <asp:UpdatePanel ID="UpdatePanelRenderingProviderActionButtons" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Button ID="ButtonRenderingProviderClear" runat="server" CssClass="ButtonRenderingProviderClear" />
            <asp:Button ID="ButtonRenderingProviderSave" runat="server" CssClass="ButtonRenderingProviderSave" />
            <asp:Button ID="ButtonRenderingProviderDelete" runat="server" CssClass="ButtonRenderingProviderDelete" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ButtonRenderingProviderClear" />
            <asp:AsyncPostBackTrigger ControlID="ButtonRenderingProviderSave" />
            <asp:AsyncPostBackTrigger ControlID="ButtonRenderingProviderDelete" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<div class="DivSpace2">
</div>
