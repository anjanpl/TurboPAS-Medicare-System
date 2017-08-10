<%@ Control Language="vb" CodeBehind="~/UserControl/EDICentral/BillOrPayToProvider.ascx.vb"
    Inherits="VisitelWeb.Visitel.UserControl.EDICentral.BillOrPayToProviderControl" %>
<asp:UpdatePanel ID="UpdatePanelBillOrPayToProvider" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <div class="newRow">
            <uc1:EditSetting ID="UserControlEditSettingBillOrPayToProvider" runat="server" />
            <asp:Label ID="LabelBillOrPayToProviderId" runat="server" Visible="false"></asp:Label>
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelHierarchicalIDNumber" runat="server" CssClass="LabelHierarchicalIDNumber"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxHierarchicalIDNumber" runat="server" CssClass="TextBoxHierarchicalIDNumber"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelHierarchicalLevelCode" runat="server" CssClass="LabelHierarchicalLevelCode"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxHierarchicalLevelCode" runat="server" CssClass="TextBoxHierarchicalLevelCode"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelHierarchicalChildCode" runat="server" CssClass="LabelHierarchicalChildCode"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxHierarchicalChildCode" runat="server" CssClass="TextBoxHierarchicalChildCode"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelProviderCode" runat="server" CssClass="LabelProviderCode"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:DropDownList ID="DropDownListProviderCode" runat="server" CssClass="DropDownListProviderCode">
                </asp:DropDownList>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelReferenceIdQualifier" runat="server" CssClass="LabelReferenceIdQualifier"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxReferenceIdQualifier" runat="server" CssClass="TextBoxReferenceIdQualifier"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelTaxonomyCode" runat="server" CssClass="LabelTaxonomyCode"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxTaxonomyCode" runat="server" CssClass="TextBoxTaxonomyCode"></asp:TextBox>
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
                    <asp:Label ID="LabelBillOrPayToProviderControlUpdateDate" runat="server" CssClass="LabelBillOrPayToProviderControlUpdateDate"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxBillOrPayToProviderControlUpdateDate" runat="server" CssClass="TextBoxBillOrPayToProviderControlUpdateDate"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelBillOrPayToProviderControlUpdateBy" runat="server" CssClass="LabelBillOrPayToProviderControlUpdateBy"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxBillOrPayToProviderControlUpdateBy" runat="server" CssClass="TextBoxBillOrPayToProviderControlUpdateBy"></asp:TextBox>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<div class="DivSpace5">
</div>
<div class="newRow DivButtonCenter">
    <asp:UpdatePanel ID="UpdatePanelBillingProviderControlActionButtons" runat="server"
        UpdateMode="Always">
        <ContentTemplate>
            <asp:Button ID="ButtonBillOrPayToProviderClear" runat="server" CssClass="ButtonBillOrPayToProviderClear" />
            <asp:Button ID="ButtonBillOrPayToProviderSave" runat="server" CssClass="ButtonBillOrPayToProviderSave" />
            <asp:Button ID="ButtonBillOrPayToProviderDelete" runat="server" CssClass="ButtonBillOrPayToProviderDelete" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ButtonBillOrPayToProviderClear" />
            <asp:AsyncPostBackTrigger ControlID="ButtonBillOrPayToProviderSave" />
            <asp:AsyncPostBackTrigger ControlID="ButtonBillOrPayToProviderDelete" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<div class="DivSpace2">
</div>
