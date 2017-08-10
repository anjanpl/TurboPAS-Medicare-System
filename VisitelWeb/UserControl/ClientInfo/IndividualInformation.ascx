<%@ Control Language="vb" CodeBehind="~/UserControl/ClientInfo/IndividualInformation.ascx.vb"
    Inherits="VisitelWeb.Visitel.UserControl.ClientInfo.IndividualInformationControl" %>
<%@ Register TagPrefix="uc" TagName="IndividualOtherInformation" Src="~/UserControl/ClientInfo/IndividualOtherInformation.ascx" %>
<%@ Register TagPrefix="uc" TagName="BillingInfo" Src="~/UserControl/ClientInfo/BillingInfo.ascx" %>
<%@ Register TagPrefix="uc" TagName="EVV" Src="~/UserControl/ClientInfo/EVV.ascx" %>
<%@ Register TagPrefix="uc" TagName="EVVInfo" Src="~/UserControl/ClientInfo/EVVInfo.ascx" %>
<%@ Register TagPrefix="uc" TagName="BillingDiagnosisCode" Src="~/UserControl/ClientInfo/BillingDiagnosisCode.ascx" %>
<%@ Register TagPrefix="uc" TagName="AuthorizationDetail" Src="~/UserControl/ClientInfo/AuthorizationDetail.ascx" %>
<%@ Register TagPrefix="uc" TagName="CurrentAuthorizationPeriod" Src="~/UserControl/ClientInfo/CurrentAuthorizationPeriod.ascx" %>
<%@ Register TagPrefix="uc" TagName="Service" Src="~/UserControl/ClientInfo/Service.ascx" %>
<%@ Register TagPrefix="uc" TagName="EmergencyContract" Src="~/UserControl/ClientInfo/EmergencyContract.ascx" %>
<div id="Div11" class="SectionDiv" runat="server">
    <div id="Div3" class="SectionDiv-header">
        <asp:Label ID="LabelClientInformationEntry" runat="server"></asp:Label>
        <uc1:EditSetting ID="UserControlEditSettingClientInfo" runat="server" />
    </div>
</div>
<div class="newRow">
    <div class="DivCommon">
        <div class="AutoColumn FloatRight">
            <asp:Label ID="LabelIndividualStatus" runat="server" CssClass="HighlightColumn"></asp:Label>
        </div>
    </div>
    <div class="AutoColumn">
        <asp:DropDownList ID="DropDownListIndividualStatus" runat="server">
        </asp:DropDownList>
    </div>
    <div class="AutoColumnAddMore">
        <asp:LinkButton ID="LinkButtonAddMoreIndividualStatus" ClientIDMode="Static" runat="server"
            CssClass="thickbox ui-state-default ui-corner-all buttonLink">
            <asp:Image ID="Image9" runat="server" ImageUrl="~/Images/AddMore.jpg" />
        </asp:LinkButton>
    </div>
    <div class="AutoColumn">
        <asp:Label ID="LabelSupervisor" runat="server" CssClass="HighlightColumn"></asp:Label>
    </div>
    <div class="AutoColumn">
        <asp:TextBox ID="TextBoxSupervisor" runat="server" CssClass="TextBoxSupervisor"></asp:TextBox>
    </div>
    <div class="AutoColumn">
        <asp:Label ID="LabelSupervisorVisitFrequency" runat="server" CssClass="HighlightColumn"></asp:Label>
    </div>
    <div class="AutoColumn">
        <asp:TextBox ID="TextBoxSupervisorVisitFrequency" runat="server" CssClass="TextBoxSupervisorVisitFrequency"></asp:TextBox>
    </div>
    <div class="AutoColumn">
        Month
    </div>
    <div class="AutoColumn">
        <asp:Label ID="LabelAssessmentDate" runat="server" CssClass="HighlightColumn"></asp:Label>
    </div>
    <div class="AutoColumn">
        <asp:TextBox ID="TextBoxAssessmentDate" runat="server" CssClass="dateField TextBoxAssessmentDate"></asp:TextBox>
        <asp:RegularExpressionValidator ID="RegularExpressionValidatorAssessmentDate" runat="server"></asp:RegularExpressionValidator>
    </div>
    <div class="AutoColumn">
        <asp:Label ID="LabelLiaison" runat="server" CssClass="HighlightColumn"></asp:Label>
    </div>
    <div class="AutoColumn">
        <asp:TextBox ID="TextBoxLiaison" runat="server" CssClass="TextBoxLiaison"></asp:TextBox>
    </div>
</div>
<%--<div class="masonry">--%>
<div class="ServiceBox">
    <div class="BoxStyle ServiceLeftIndividualDetail ServiceBoxIndividualDetail SectionDiv">
        <asp:UpdatePanel ID="UpdatePanelClientInformation" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelStateClientId" runat="server" CssClass="HighlightColumn"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxStateClientId" runat="server" CssClass="TextBoxStateClientId"></asp:TextBox>
                    </div>
                    <div class="AutoColumn">
                        <asp:Label ID="LabelInsuranceNumber" runat="server" CssClass="HighlightColumn"></asp:Label>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxInsuranceNumber" runat="server" CssClass="TextBoxInsuranceNumber"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelFirstName" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxFirstName" runat="server" CssClass="TextBoxFirstName"></asp:TextBox>
                    </div>
                    <div class="AutoColumn">
                        <asp:Label ID="LabelMiddleInitial" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:TextBox ID="TextBoxMiddleInitial" runat="server" CssClass="TextBoxMiddleInitial"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelLastName" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxLastName" runat="server" CssClass="TextBoxLastName"></asp:TextBox>
                    </div>
                    <div class="AutoColumn">
                        <asp:Label ID="LabelCounty" runat="server"></asp:Label>
                    </div>
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListCounty" runat="server" CssClass="DropDownListCounty">
                        </asp:DropDownList>
                    </div>
                    <div class="AutoColumnAddMore">
                        <asp:LinkButton ID="LinkButtonAddMoreCounty" ClientIDMode="Static" runat="server"
                            CssClass="thickbox ui-state-default ui-corner-all buttonLink">
                            <asp:Image ID="Image7" runat="server" ImageUrl="~/Images/AddMore.jpg" />
                        </asp:LinkButton>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelAddress" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxAddress" runat="server" TextMode="MultiLine" CssClass="TextBoxAddress"></asp:TextBox>
                    </div>
                    <div class="AutoColumn">
                        <asp:Label ID="LabelApartmentNumber" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:TextBox ID="TextBoxApartmentNumber" runat="server" CssClass="TextBoxApartmentNumber"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelCity" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListCity" runat="server" CssClass="DropDownListCity">
                        </asp:DropDownList>
                    </div>
                    <div class="AutoColumnAddMore">
                        <asp:LinkButton ID="LinkButtonAddMoreCity" ClientIDMode="Static" runat="server">
                            <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/AddMore.jpg" />
                        </asp:LinkButton>
                    </div>
                    <div class="AutoColumn">
                        <asp:Label ID="LabelState" runat="server"></asp:Label>
                    </div>
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListState" runat="server" CssClass="DropDownListState">
                        </asp:DropDownList>
                    </div>
                    <div class="AutoColumnAddMore">
                        <asp:LinkButton ID="LinkButtonAddMoreState" ClientIDMode="Static" runat="server"
                            CssClass="thickbox ui-state-default ui-corner-all buttonLink">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/AddMore.jpg" />
                        </asp:LinkButton>
                    </div>
                    <div class="AutoColumn">
                        <asp:Label ID="LabelZip" runat="server"></asp:Label>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxZip" runat="server" CssClass="TextBoxZip"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelPhone" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxPhone" runat="server" CssClass="TextBoxPhone"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorPhone" runat="server"></asp:RegularExpressionValidator>
                    </div>
                    <div class="AutoColumn">
                        <asp:Label ID="LabelAlternatePhone" runat="server"></asp:Label>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxAlternatePhone" runat="server" CssClass="TextBoxAlternatePhone"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorAlternatePhone" runat="server"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelDateOfBirth" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxDateOfBirth" runat="server" CssClass="dateField TextBoxDateOfBirth"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorDateOfBirth" runat="server">
                        </asp:RegularExpressionValidator>
                    </div>
                    <div class="AutoColumn">
                        <asp:Label ID="LabelAge" runat="server"></asp:Label>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxAge" runat="server" CssClass="TextBoxAge"></asp:TextBox>
                    </div>
                    <div class="AutoColumn">
                        <asp:Label ID="LabelSocialSecurityNumber" runat="server"></asp:Label>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxSocialSecurityNumber" runat="server" CssClass="TextBoxSocialSecurityNumber"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorSocialSecurityNumber"
                            runat="server"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelGender" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListGender" runat="server" CssClass="DropDownListGender">
                        </asp:DropDownList>
                    </div>
                    <div class="AutoColumn">
                        <asp:Label ID="LabelMaritalStatus" runat="server"></asp:Label>
                    </div>
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListMaritalStatus" runat="server" CssClass="DropDownListMaritalStatus">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelServiceStartDate" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxServiceStartDate" runat="server" CssClass="dateField TextBoxServiceStartDate"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorStartDate" runat="server"></asp:RegularExpressionValidator>
                    </div>
                    <div class="AutoColumn">
                        <asp:Label ID="LabelServiceEndDate" runat="server"></asp:Label>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxServiceEndDate" runat="server" CssClass="dateField TextBoxServiceEndDate"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorEndDate" runat="server"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="newRow">
                </div>
                <div>
                    <uc:IndividualOtherInformation ID="ucIndividualOtherInformation" runat="server" />
                </div>
                <div class="newRow">
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ucIndividualOtherInformation" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</div>
<div>
    <uc:EmergencyContract ID="ucEmergencyContract" runat="server" />
</div>
<div>
    <uc:CurrentAuthorizationPeriod ID="ucCurrentAuthorizationPeriod" runat="server" />
</div>
<div>
    <uc:BillingInfo ID="ucBillingInfo" runat="server" />
</div>
<div>
    <uc:EVV ID="ucEVV" runat="server" />
</div>
<div>
    <uc:BillingDiagnosisCode ID="ucBillingDiagnosisCode" runat="server" />
</div>
<div>
    <uc:AuthorizationDetail ID="ucAuthorizationDetail" runat="server" />
</div>
<div>
    <uc:EVVInfo ID="ucEVVInfo" runat="server" />
</div>
<div>
    <uc:Service ID="ucService" runat="server" />
</div>
<%--</div>--%>
<div class="newRow DivButtonCenter">
    <asp:UpdatePanel ID="UpdatePanelActionButtons" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Button ID="ButtonCoordinationOfCare" runat="server" CssClass="ButtonCoordinationOfCare" />
            <asp:Button ID="ButtonHealthAssessOrSDP" runat="server" CssClass="ButtonHealthAssessOrSDP" />
            <asp:Button ID="ButtonSupVisit" runat="server" CssClass="ButtonSupVisit" />
            <asp:Button ID="ButtonMapquest" runat="server" CssClass="ButtonMapquest" />
            <asp:Button ID="ButtonClear" runat="server" CssClass="ButtonClear" />
            <asp:Button ID="ButtonSave" runat="server" CssClass="ButtonSave" />
            <asp:Button ID="ButtonDelete" runat="server" CssClass="ButtonDelete" />
            <asp:Button ID="ButtonSynchronizeDiagnosisCode" runat="server" CssClass="ButtonSynchronizeDiagnosisCode" />
            <asp:Button ID="ButtonIndividualDetail" runat="server" CssClass="ButtonIndividualDetail" />
            <asp:Button ID="ButtonBillingInformation" runat="server" CssClass="ButtonBillingInformation" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ButtonCoordinationOfCare" />
            <asp:PostBackTrigger ControlID="ButtonHealthAssessOrSDP" />
            <asp:PostBackTrigger ControlID="ButtonSupVisit" />
            <asp:PostBackTrigger ControlID="ButtonMapquest" />
            <asp:AsyncPostBackTrigger ControlID="ButtonClear" />
            <asp:AsyncPostBackTrigger ControlID="ButtonSave" />
            <asp:AsyncPostBackTrigger ControlID="ButtonDelete" />
            <asp:AsyncPostBackTrigger ControlID="ButtonSynchronizeDiagnosisCode" />
            <asp:PostBackTrigger ControlID="ButtonIndividualDetail" />
            <asp:PostBackTrigger ControlID="ButtonBillingInformation" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<div class="DivSpace10">
    <asp:UpdatePanel ID="UpdatePanelHiddenField" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:HiddenField ID="HiddenFieldClientInfoId" runat="server" />
            <asp:HiddenField ID="HiddenFieldIsNew" runat="server" />
            <asp:HiddenField ID="HiddenFieldIsSearched" runat="server" />
            <asp:HiddenField ID="HiddenFieldIndividualStatus" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
