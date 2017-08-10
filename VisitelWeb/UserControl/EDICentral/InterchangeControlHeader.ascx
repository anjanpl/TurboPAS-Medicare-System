<%@ Control Language="vb" CodeBehind="~/UserControl/EDICentral/InterchangeControlHeader.ascx.vb"
    Inherits="VisitelWeb.Visitel.UserControl.EDICentral.InterchangeControlHeaderControl" %>
<asp:UpdatePanel ID="UpdatePanelInterchangeControlHeader" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <asp:Label ID="LabelInterchangeControlHeaderId" runat="server" Visible="false"></asp:Label>
        <div class="newRow">
            <uc1:EditSetting ID="UserControlEditSettingInterchangeControlHeader" runat="server" />
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelAuthorizationInformationQualifier" runat="server" CssClass="LabelAuthorizationInformationQualifier"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:DropDownList ID="DropDownListAuthorizationInformationQualifier" runat="server"
                    CssClass="DropDownListAuthorizationInformationQualifier">
                </asp:DropDownList>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelAuthorizationInformation" runat="server" CssClass="LabelAuthorizationInformation"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxAuthorizationInformation" runat="server" CssClass="TextBoxAuthorizationInformation"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelSecurityInformationQualifier" runat="server" CssClass="LabelSecurityInformationQualifier"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:DropDownList ID="DropDownListSecurityInformationQualifier" runat="server" CssClass="DropDownListSecurityInformationQualifier">
                </asp:DropDownList>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelSecurityInformation" runat="server" CssClass="LabelSecurityInformation"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxSecurityInformation" runat="server" CssClass="TextBoxSecurityInformation"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelInterchangeIDQualifier" runat="server" CssClass="LabelInterchangeIDQualifier"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:DropDownList ID="DropDownListInterchangeIdQualifier" runat="server" CssClass="DropDownListInterchangeIDQualifier">
                </asp:DropDownList>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelSubmitterId" runat="server" CssClass="LabelSubmitterId"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxSubmitterId" runat="server" CssClass="TextBoxSubmitterId"></asp:TextBox>
            </div>
        </div>
        <%--<div class="newRow">
    <div class="AutoColumn">
        <asp:Label ID="LabelInterchangeIDQualifier" runat="server" Text="Interchange ID Qualifier:"></asp:Label>
    </div>
     <div class="AutoColumn">
    </div>
</div>--%>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelReceiverId" runat="server" CssClass="LabelReceiverId"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxReceiverId" runat="server" CssClass="TextBoxReceiverId"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelControlVersionId" runat="server" CssClass="LabelControlVersionId"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxControlVersionId" runat="server" CssClass="TextBoxControlVersionId"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelControlNumber" runat="server" CssClass="LabelControlNumber"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxControlNumber" runat="server" CssClass="TextBoxControlNumber"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelAcknowledgementRequested" runat="server" CssClass="LabelAcknowledgementRequested"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:DropDownList ID="DropDownListAcknowledgementRequested" runat="server" CssClass="DropDownListAcknowledgementRequested">
                </asp:DropDownList>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelUsageIndicator" runat="server" CssClass="LabelUsageIndicator"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:DropDownList ID="DropDownListUsageIndicator" runat="server" CssClass="DropDownListUsageIndicator">
                </asp:DropDownList>
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
    <asp:UpdatePanel ID="UpdatePanelInterchangeControlHeaderActionButtons" runat="server"
        UpdateMode="Always">
        <ContentTemplate>
            <asp:Button ID="ButtonInterchangeControlHeaderClear" runat="server" CssClass="ButtonInterchangeControlHeaderClear" />
            <asp:Button ID="ButtonInterchangeControlHeaderSave" runat="server" CssClass="ButtonInterchangeControlHeaderSave" />
            <asp:Button ID="ButtonInterchangeControlHeaderDelete" runat="server" CssClass="ButtonInterchangeControlHeaderDelete" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ButtonInterchangeControlHeaderClear" />
            <asp:AsyncPostBackTrigger ControlID="ButtonInterchangeControlHeaderSave" />
            <asp:AsyncPostBackTrigger ControlID="ButtonInterchangeControlHeaderDelete" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<div class="DivSpace2">
</div>
