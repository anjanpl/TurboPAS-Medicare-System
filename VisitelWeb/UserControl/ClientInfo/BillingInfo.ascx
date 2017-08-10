<%@ Control Language="vb" CodeBehind="~/UserControl/ClientInfo/BillingInfo.ascx.vb"
    Inherits="VisitelWeb.Visitel.UserControl.ClientInfo.BillingInfoControl" %>
<div class="ServiceBox">
    <div class="BoxStyle ServiceLeftBillingInfo ServiceBoxBillingInfo SectionDiv">
        <div id="Div9" class="SectionDiv-header">
            <asp:Label ID="LabelBillingInfo" runat="server"></asp:Label>
        </div>
        <asp:UpdatePanel ID="UpdatePanelBillingInfo" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="newRow">
                    <div class="AutoColumn">
                        <asp:Label ID="LabelAuthorizationNumber" runat="server"></asp:Label>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxAuthorizationNumber" runat="server" CssClass="TextBoxAuthorizationNumber"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="AutoColumn">
                        <asp:Label ID="LabelProcedureCode" runat="server"></asp:Label>
                    </div>
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListProcedureCode" runat="server" CssClass="DropDownListProcedureCode">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSourceDropDownListProcedureCode" runat="server"></asp:SqlDataSource>
                    </div>
                </div>
                <div class="newRow">
                    <div class="AutoColumn">
                        <asp:Label ID="LabelModifiers" runat="server"></asp:Label>
                    </div>
                    <div class="BillingInfoColumn2NoLeftPad">
                        <div class="AutoColumn">
                            <asp:TextBox ID="TextBoxModifierOne" runat="server" CssClass="TextBoxModifierOne"></asp:TextBox>
                        </div>
                        <div class="AutoColumn">
                            <asp:TextBox ID="TextBoxModifierTwo" runat="server" CssClass="TextBoxModifierTwo"></asp:TextBox>
                        </div>
                        <div class="AutoColumn">
                            <asp:TextBox ID="TextBoxModifierThree" runat="server" CssClass="TextBoxModifierThree"></asp:TextBox>
                        </div>
                        <div class="AutoColumn">
                            <asp:TextBox ID="TextBoxModifierFour" runat="server" CssClass="TextBoxModifierFour"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="newRow">
                    <div class="BillingInfoColumn3">
                        <asp:Label ID="LabelPlaceOfService" runat="server"></asp:Label>
                    </div>
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListPlaceOfService" runat="server" CssClass="DropDownListPlaceOfService">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSourceDropDownListPlaceOfService" runat="server"></asp:SqlDataSource>
                    </div>
                </div>
                <div class="newRow">
                    <div class="AutoColumn">
                        <asp:Label ID="LabelUnitRate" runat="server"></asp:Label>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxUnitRate" runat="server" CssClass="TextBoxUnitRate"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorUnitRate" runat="server"></asp:RegularExpressionValidator>
                    </div>
                    <div class="AutoColumn">
                        <asp:HyperLink ID="HyperLinkTexMedConnect" runat="server" Text="TexMedConnect" CssClass="HyperLinkTexMedConnect"></asp:HyperLink>
                    </div>
                </div>
                <div class="newRow">
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxComments" runat="server" TextMode="MultiLine" CssClass="TextBoxComments"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonBillingInfo">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelClaimFrequencyTypeCode" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxClaimFrequencyTypeCode" runat="server" CssClass="TextBoxClaimFrequencyTypeCode"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonBillingInfo">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelProviderSignatureOnFile" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListProviderSignatureOnFile" runat="server" CssClass="DropDownListProviderSignatureOnFile">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonBillingInfo">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelMedicareAssignmentCode" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListMedicareAssignmentCode" runat="server" CssClass="DropDownListMedicareAssignmentCode">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSourceDropDownListMedicareAssignmentCode" runat="server">
                        </asp:SqlDataSource>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonBillingInfo">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelAssignmentOfBenefitsIndicator" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListAssignmentOfBenefitsIndicator" runat="server" CssClass="DropDownListAssignmentOfBenefitsIndicator">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonBillingInfo">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelReleaseOfInformationCode" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListReleaseOfInformationCode" runat="server" CssClass="DropDownListReleaseOfInformationCode">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSourceDropDownListReleaseOfInformationCode" runat="server">
                        </asp:SqlDataSource>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonBillingInfo">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelPatientSignatureCode" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListPatientSignatureCode" runat="server" CssClass="DropDownListPatientSignatureCode">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSourceDropDownListPatientSignatureCode" runat="server">
                        </asp:SqlDataSource>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="DivSpace3">
        </div>
    </div>
</div>
