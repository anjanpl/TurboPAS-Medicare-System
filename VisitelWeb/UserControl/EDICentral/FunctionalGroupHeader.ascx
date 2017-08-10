<%@ Control Language="vb" CodeBehind="~/UserControl/EDICentral/FunctionalGroupHeader.ascx.vb"
    Inherits="VisitelWeb.Visitel.UserControl.EDICentral.FunctionalGroupHeaderControl" %>
<asp:UpdatePanel ID="UpdatePanelFunctionalGroupHeader" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <asp:Label ID="LabelFunctionalGroupHeaderId" runat="server" Visible="false"></asp:Label>
        <div class="newRow">
            <uc1:EditSetting ID="UserControlEditSettingFunctionalGroupHeader" runat="server" />
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelFunctionalIdentifierCode" runat="server" CssClass="LabelFunctionalIdentifierCode"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxFunctionalIdentifierCode" runat="server" CssClass="TextBoxFunctionalIdentifierCode"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelApplicationSenderCode" runat="server" CssClass="LabelApplicationSenderCode"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxApplicationSenderCode" runat="server" CssClass="TextBoxApplicationSenderCode"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelApplicationReceiverCode" runat="server" CssClass="LabelApplicationReceiverCode"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:DropDownList ID="DropDownListApplicationReceiverCode" runat="server" CssClass="DropDownListApplicationReceiverCode">
                </asp:DropDownList>
                <asp:SqlDataSource ID="SqlDataSourceApplicationReceiverCode" runat="server"></asp:SqlDataSource>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelGroupControlNumber" runat="server" CssClass="LabelGroupControlNumber"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxGroupControlNumber" runat="server" CssClass="TextBoxGroupControlNumber"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelResponsibleAgencyCode" runat="server" CssClass="LabelResponsibleAgencyCode"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxResponsibleAgencyCode" runat="server" CssClass="TextBoxResponsibleAgencyCode"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelIndustryIdentifierCode" runat="server" CssClass="LabelIndustryIdentifierCode"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxIndustryIdentifierCode" runat="server" CssClass="TextBoxIndustryIdentifierCode"></asp:TextBox>
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
    <asp:UpdatePanel ID="UpdatePanelFunctionalGroupHeaderControlActionButtons" runat="server"
        UpdateMode="Always">
        <ContentTemplate>
            <asp:Button ID="ButtonFunctionalGroupHeaderClear" runat="server" CssClass="ButtonFunctionalGroupHeaderClear" />
            <asp:Button ID="ButtonFunctionalGroupHeaderSave" runat="server" CssClass="ButtonFunctionalGroupHeaderSave" />
            <asp:Button ID="ButtonFunctionalGroupHeaderDelete" runat="server" CssClass="ButtonFunctionalGroupHeaderDelete" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ButtonFunctionalGroupHeaderClear" />
            <asp:AsyncPostBackTrigger ControlID="ButtonFunctionalGroupHeaderSave" />
            <asp:AsyncPostBackTrigger ControlID="ButtonFunctionalGroupHeaderDelete" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<div class="DivSpace2">
</div>
