<%@ Control Language="vb" CodeBehind="~/UserControl/ClientInfo/IndividualInformation.ascx.vb"
    Inherits="VisitelWeb.Visitel.UserControl.ClientInfo.IndividualInformationControl" %>
<div id="Div11" class="SectionDiv" runat="server">
    <div id="Div3" class="SectionDiv-header">
        <asp:Label ID="LabelClientInformationEntry" runat="server"></asp:Label>
        <uc1:EditSetting ID="UserControlEditSettingClientInfo" runat="server" />
    </div>
</div>
<div class="newRow">
</div>
<asp:UpdatePanel ID="UpdatePanelClientInformation" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <div class="newRow">
            <div class="AutoColumn">
                <asp:Label ID="LabelIndividualStatus" runat="server" CssClass="HighlightColumn"></asp:Label>
            </div>
            <div class="SecondColumn">
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
            <div class="SecondColumn">
                <asp:TextBox ID="TextBoxSupervisor" runat="server" CssClass="TextBoxSupervisor"></asp:TextBox>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelSupervisorVisitFrequency" runat="server" CssClass="HighlightColumn"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:TextBox ID="TextBoxSupervisorVisitFrequency" runat="server" CssClass="TextBoxSupervisorVisitFrequency"></asp:TextBox>
            </div>
            <div class="AutoColumn">
                Month
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelAssessmentDate" runat="server" CssClass="HighlightColumn"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:TextBox ID="TextBoxAssessmentDate" runat="server" CssClass="dateField"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidatorAssessmentDate" runat="server"></asp:RegularExpressionValidator>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelLiaison" runat="server" CssClass="HighlightColumn"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:TextBox ID="TextBoxLiaison" runat="server" CssClass="TextBoxLiaison"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="AutoColumn">
                <asp:Label ID="LabelStateClientId" runat="server" CssClass="HighlightColumn"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:TextBox ID="TextBoxStateClientId" runat="server" CssClass="TextBoxStateClientId"></asp:TextBox>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelInsuranceNumber" runat="server" CssClass="HighlightColumn"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:TextBox ID="TextBoxInsuranceNumber" runat="server" CssClass="TextBoxInsuranceNumber"></asp:TextBox>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelSocialSecurityNumber" runat="server"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:TextBox ID="TextBoxSocialSecurityNumber" runat="server" CssClass="TextBoxSocialSecurityNumber"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidatorSocialSecurityNumber"
                    runat="server"></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="newRow">
            <div class="AutoColumn">
                <asp:Label ID="LabelFirstName" runat="server"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:TextBox ID="TextBoxFirstName" runat="server"></asp:TextBox>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelLastName" runat="server"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:TextBox ID="TextBoxLastName" runat="server"></asp:TextBox>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelAddress" runat="server"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:TextBox ID="TextBoxAddress" runat="server" TextMode="MultiLine" CssClass="TextBoxAddress"></asp:TextBox>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelCity" runat="server"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:DropDownList ID="DropDownListCity" runat="server">
                </asp:DropDownList>
            </div>
            <div class="AutoColumnAddMore">
                <asp:LinkButton ID="LinkButtonAddMoreCity" ClientIDMode="Static" runat="server">
                    <asp:Image ID="Image5" runat="server" ImageUrl="~/Images/AddMore.jpg" />
                </asp:LinkButton>
            </div>
        </div>
        <div class="newRow">
            <div class="AutoColumn">
                <asp:Label ID="LabelPhone" runat="server"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:TextBox ID="TextBoxPhone" runat="server" CssClass="TextBoxPhone"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidatorPhone" runat="server"></asp:RegularExpressionValidator>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelAlternatePhone" runat="server"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:TextBox ID="TextBoxAlternatePhone" runat="server" CssClass="TextBoxAlternatePhone"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidatorAlternatePhone" runat="server"></asp:RegularExpressionValidator>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelDateOfBirth" runat="server"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:TextBox ID="TextBoxDateOfBirth" runat="server" CssClass="dateField"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidatorDateOfBirth" runat="server">
                </asp:RegularExpressionValidator>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelGender" runat="server"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:DropDownList ID="DropDownListGender" runat="server">
                </asp:DropDownList>
            </div>
        </div>
        <div class="newRow">
            <div class="AutoColumn">
                <asp:Label ID="LabelDischargeReason" runat="server"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:DropDownList ID="DropDownListDischargeReason" runat="server">
                </asp:DropDownList>
            </div>
            <div class="AutoColumnAddMore">
                <asp:LinkButton ID="LinkButtonAddMoreDischargeReason" ClientIDMode="Static" runat="server"
                    CssClass="thickbox ui-state-default ui-corner-all buttonLink">
                    <asp:Image ID="Image4" runat="server" ImageUrl="~/Images/AddMore.jpg" />
                </asp:LinkButton>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelCaseWorker" runat="server"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:DropDownList ID="DropDownListCaseWorker" runat="server">
                </asp:DropDownList>
            </div>
            <div class="AutoColumnAddMore">
                <asp:LinkButton ID="LinkButtonAddMoreCaseWorker" ClientIDMode="Static" runat="server"
                    CssClass="thickbox ui-state-default ui-corner-all buttonLink">
                    <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/AddMore.jpg" />
                </asp:LinkButton>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelPriority" runat="server"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:DropDownList ID="DropDownListPriority" runat="server">
                </asp:DropDownList>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelMaritalStatus" runat="server"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:DropDownList ID="DropDownListMaritalStatus" runat="server">
                </asp:DropDownList>
            </div>
        </div>
        <div class="newRow">
            <div class="AutoColumn">
                <asp:Label ID="LabelState" runat="server"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:DropDownList ID="DropDownListState" runat="server">
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
            <div class="SecondColumn">
                <asp:TextBox ID="TextBoxZip" runat="server" CssClass="TextBoxZip"></asp:TextBox>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelApartmentNumber" runat="server"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:TextBox ID="TextBoxApartmentNumber" runat="server" CssClass="TextBoxApartmentNumber"></asp:TextBox>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelAge" runat="server"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:TextBox ID="TextBoxAge" runat="server" CssClass="TextBoxAge"></asp:TextBox>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelUnits" runat="server"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:TextBox ID="TextBoxUnits" runat="server" CssClass="TextBoxUnits"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidatorUnits" runat="server"></asp:RegularExpressionValidator>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelNumberOfWeekDays" runat="server"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:TextBox ID="TextBoxNumberOfWeekDays" runat="server" CssClass="TextBoxNumberOfWeekDays"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="AutoColumn">
                <asp:Label ID="LabelCounty" runat="server"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:DropDownList ID="DropDownListCounty" runat="server">
                </asp:DropDownList>
            </div>
            <div class="AutoColumnAddMore">
                <asp:LinkButton ID="LinkButtonAddMoreCounty" ClientIDMode="Static" runat="server"
                    CssClass="thickbox ui-state-default ui-corner-all buttonLink">
                    <asp:Image ID="Image7" runat="server" ImageUrl="~/Images/AddMore.jpg" />
                </asp:LinkButton>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelMiddleInitial" runat="server"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:TextBox ID="TextBoxMiddleInitial" runat="server" CssClass="TextBoxMiddleInitial"></asp:TextBox>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelDoctor" runat="server"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:DropDownList ID="DropDownListDoctor" runat="server">
                </asp:DropDownList>
            </div>
            <div class="AutoColumnAddMore">
                <asp:LinkButton ID="LinkButtonAddMoreDoctor" ClientIDMode="Static" runat="server"
                    CssClass="thickbox ui-state-default ui-corner-all buttonLink">
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/AddMore.jpg" />
                </asp:LinkButton>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelClientType" runat="server"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:DropDownList ID="DropDownListClientType" runat="server">
                </asp:DropDownList>
                <asp:HiddenField ID="HiddenFieldClientCaseId" runat="server" />
            </div>
            <div class="AutoColumnAddMore">
                <asp:LinkButton ID="LinkButtonAddMoreClientType" ClientIDMode="Static" runat="server">
                    <asp:Image ID="Image8" runat="server" ImageUrl="~/Images/AddMore.jpg" />
                </asp:LinkButton>
            </div>
        </div>
        <div class="newRow">
            <div class="AutoColumn">
                <asp:Label ID="LabelServiceStartDate" runat="server"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:TextBox ID="TextBoxServiceStartDate" runat="server" CssClass="dateField"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidatorStartDate" runat="server"></asp:RegularExpressionValidator>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelServiceEndDate" runat="server"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:TextBox ID="TextBoxServiceEndDate" runat="server" CssClass="dateField"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidatorEndDate" runat="server"></asp:RegularExpressionValidator>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelSupervisorLastVisitDate" runat="server"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:TextBox ID="TextBoxSupervisorLastVisitDate" runat="server" CssClass="dateField"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidatorSupervisorLastVisitDate"
                    runat="server"></asp:RegularExpressionValidator>
            </div>
        </div>
        <div class="newRow">
            <div class="AutoColumn">
                <asp:Label ID="LabelDiagnosis" runat="server"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:TextBox ID="TextBoxDiagnosis" runat="server" TextMode="MultiLine" CssClass="TextBoxDiagnosis"></asp:TextBox>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelSuppliesOrEquipment" runat="server"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:TextBox ID="TextBoxSuppliesOrEquipment" runat="server" CssClass="TextBoxSuppliesOrEquipment"
                    TextMode="MultiLine"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
        </div>
        <div class="newRow">
            <div class="AutoColumn">
                <asp:Label ID="LabelUpdateDate" runat="server"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:TextBox ID="TextBoxUpdateDate" runat="server"></asp:TextBox>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelUpdateBy" runat="server"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:TextBox ID="TextBoxUpdateBy" runat="server"></asp:TextBox>
            </div>
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="DropDownListClientType" />
    </Triggers>
</asp:UpdatePanel>
<div class="newRow">
</div>
<div class="ServiceBox">
    <div class="BoxStyle ServiceLeft ServiceBoxEmergencyContract SectionDiv">
        <div id="Div5" class="SectionDiv-header">
            <asp:Label ID="LabelEmergencyContact" runat="server"></asp:Label>
        </div>
        <asp:UpdatePanel ID="UpdatePanelEmergencyContact" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="newRow">
                    <div class="EmergencyContactColumn1">
                        <asp:Label ID="LabelEmergencyContactOneName" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:TextBox ID="TextBoxEmergencyContactOneName" runat="server" CssClass="TextBoxEmergencyContactOneName"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="EmergencyContactColumn1">
                        <asp:Label ID="LabelEmergencyContactOnePhone" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:TextBox ID="TextBoxEmergencyContactOnePhone" runat="server" CssClass="TextBoxEmergencyContactOnePhone"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorEmergencyContactOnePhone"
                            runat="server"></asp:RegularExpressionValidator>
                    </div>
                    <div class="AutoColumn">
                        <asp:Label ID="LabelEmergencyContactOneRelationship" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:TextBox ID="TextBoxEmergencyContactOneRelationship" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                </div>
                <div class="newRow">
                    <div class="EmergencyContactColumn1">
                        <asp:Label ID="LabelEmergencyContactTwoName" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:TextBox ID="TextBoxEmergencyContactTwoName" runat="server" CssClass="TextBoxEmergencyContactTwoName"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="EmergencyContactColumn1">
                        <asp:Label ID="LabelEmergencyContactTwoPhone" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:TextBox ID="TextBoxEmergencyContactTwoPhone" runat="server" CssClass="TextBoxEmergencyContactTwoPhone"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorEmergencyContactTwoPhone"
                            runat="server"></asp:RegularExpressionValidator>
                    </div>
                    <div class="AutoColumn">
                        <asp:Label ID="LabelEmergencyContactTwoRelationship" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:TextBox ID="TextBoxEmergencyContactTwoRelationship" runat="server"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="AutoColumn">
                        <asp:Label ID="LabelEmergencyDisasterCategory" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:DropDownList ID="DropDownListEmergencyDisasterCategory" runat="server">
                        </asp:DropDownList>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="DivSpace8">
        </div>
    </div>
    <div class="BoxStyle ServiceLeft ServiceBoxSantrax SectionDiv">
        <div id="Div6" class="SectionDiv-header">
            <asp:Label ID="LabelSantrax" runat="server"></asp:Label>
        </div>
        <asp:UpdatePanel ID="UpdatePanelSantrax" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="newRow">
                    <div class="SantraxColumn1">
                        <asp:Label ID="LabelSantraxClientId" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:TextBox ID="TextBoxSantraxClientId" runat="server" CssClass="TextBoxSantraxClientId"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="SantraxColumn1">
                        <asp:Label ID="LabelSantraxARNumber" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:TextBox ID="TextBoxSantraxARNumber" runat="server" CssClass="TextBoxSantraxARNumber"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="SantraxColumn1">
                        <asp:Label ID="LabelSantraxPriority" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:DropDownList ID="DropDownListSantraxPriority" runat="server" CssClass="DropDownListSantraxPriority">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="newRow">
                    <div class="SantraxColumn1">
                        <asp:Label ID="LabelSantraxBillCode" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:TextBox ID="TextBoxSantraxBillCode" runat="server" CssClass="TextBoxSantraxBillCode"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="SantraxColumn1">
                        <asp:Label ID="LabelSantraxProcCodeQualifier" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:TextBox ID="TextBoxSantraxProcCodeQualifier" runat="server" CssClass="TextBoxSantraxProcCodeQualifier"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="SantraxColumn1">
                        <asp:Label ID="LabelSantraxLandPhone" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:TextBox ID="TextBoxSantraxLandPhone" runat="server" CssClass="TextBoxSantraxLandPhone"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorSantraxLandPhone" runat="server"></asp:RegularExpressionValidator>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="DivSpace3">
        </div>
    </div>
</div>
<div class="newRow">
</div>
<div class="ServiceBox">
    <div class="BoxStyle ServiceLeft ServiceBoxBillingDiagonosisCode SectionDiv">
        <div id="Div7" class="SectionDiv-header">
            <asp:Label ID="LabelBillingDiagonosisCode" runat="server"></asp:Label>
        </div>
        <asp:UpdatePanel ID="UpdatePanelBillingDiagonosisCode" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="newRow">
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListBillingDiagonosisOne" runat="server" CssClass="DropDownListBillingDiagonosisOne">
                        </asp:DropDownList>
                    </div>
                    <div class="SecondColumn">
                        <asp:DropDownList ID="DropDownListBillingDiagonosisCodeOne" runat="server" CssClass="DropDownListBillingDiagonosisCodeOne">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="newRow">
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListBillingDiagonosisTwo" runat="server" CssClass="DropDownListBillingDiagonosisTwo">
                        </asp:DropDownList>
                    </div>
                    <div class="SecondColumn">
                        <asp:DropDownList ID="DropDownListBillingDiagonosisCodeTwo" runat="server" CssClass="DropDownListBillingDiagonosisCodeTwo">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="newRow">
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListBillingDiagonosisThree" runat="server" CssClass="DropDownListBillingDiagonosisThree">
                        </asp:DropDownList>
                    </div>
                    <div class="SecondColumn">
                        <asp:DropDownList ID="DropDownListBillingDiagonosisCodeThree" runat="server" CssClass="DropDownListBillingDiagonosisCodeThree">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="newRow">
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListBillingDiagonosisFour" runat="server" CssClass="DropDownListBillingDiagonosisFour">
                        </asp:DropDownList>
                    </div>
                    <div class="SecondColumn">
                        <asp:DropDownList ID="DropDownListBillingDiagonosisCodeFour" runat="server" CssClass="DropDownListBillingDiagonosisCodeFour">
                        </asp:DropDownList>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="DropDownListBillingDiagonosisOne" />
                <asp:AsyncPostBackTrigger ControlID="DropDownListBillingDiagonosisCodeOne" />
                <asp:AsyncPostBackTrigger ControlID="DropDownListBillingDiagonosisTwo" />
                <asp:AsyncPostBackTrigger ControlID="DropDownListBillingDiagonosisCodeTwo" />
                <asp:AsyncPostBackTrigger ControlID="DropDownListBillingDiagonosisThree" />
                <asp:AsyncPostBackTrigger ControlID="DropDownListBillingDiagonosisCodeThree" />
                <asp:AsyncPostBackTrigger ControlID="DropDownListBillingDiagonosisFour" />
                <asp:AsyncPostBackTrigger ControlID="DropDownListBillingDiagonosisCodeFour" />
            </Triggers>
        </asp:UpdatePanel>
        <div class="DivSpace10">
        </div>
    </div>
    <div class="BoxStyle ServiceLeft ServiceBoxAuthorization SectionDiv">
        <div id="Div4" class="SectionDiv-header">
            <asp:Label ID="LabelAuthorization" runat="server"></asp:Label>
        </div>
        <div class="newRow">
            <asp:UpdatePanel ID="UpdatePanelAuthorization" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <div class="AutoColumn">
                        <asp:Label ID="LabelAuthorizationFromDate" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:TextBox ID="TextBoxAuthorizationFromDate" runat="server" CssClass="dateField"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorAuthorizationFromDate"
                            runat="server"></asp:RegularExpressionValidator>
                    </div>
                    <div class="AutoColumn">
                        <asp:Label ID="LabelAuthorizationToDate" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:TextBox ID="TextBoxAuthorizationToDate" runat="server" CssClass="dateField"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorAuthorizationToDate"
                            runat="server"></asp:RegularExpressionValidator>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="DivSpace3">
        </div>
    </div>
</div>
<div class="newRow">
</div>
<div class="ServiceBox">
    <div class="BoxStyle ServiceLeft ServiceBoxBillingInfo SectionDiv">
        <div id="Div9" class="SectionDiv-header">
            <asp:Label ID="LabelBillingInfo" runat="server"></asp:Label>
        </div>
        <asp:UpdatePanel ID="UpdatePanelBillingInfo" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="newRow">
                    <div class="BillingInfoColumn1">
                        <asp:Label ID="LabelAuthorizationNumber" runat="server"></asp:Label>
                    </div>
                    <div class="BillingInfoColumn2">
                        <asp:TextBox ID="TextBoxAuthorizationNumber" runat="server" CssClass="TextBoxAuthorizationNumber"></asp:TextBox>
                    </div>
                    <div class="BillingInfoColumn3">
                        <asp:Label ID="LabelProcedureCode" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:DropDownList ID="DropDownListProcedureCode" runat="server" CssClass="DropDownListProcedureCode">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSourceDropDownListProcedureCode" runat="server"></asp:SqlDataSource>
                    </div>
                </div>
                <div class="newRow">
                    <div class="BillingInfoColumn1">
                        <asp:Label ID="LabelModifiers" runat="server"></asp:Label>
                    </div>
                    <div class="BillingInfoColumn2NoLeftPad">
                        <div class="SecondColumn">
                            <asp:TextBox ID="TextBoxModifierOne" runat="server" CssClass="TextBoxModifierOne"></asp:TextBox>
                        </div>
                        <div class="SecondColumn">
                            <asp:TextBox ID="TextBoxModifierTwo" runat="server" CssClass="TextBoxModifierTwo"></asp:TextBox>
                        </div>
                        <div class="SecondColumn">
                            <asp:TextBox ID="TextBoxModifierThree" runat="server" CssClass="TextBoxModifierThree"></asp:TextBox>
                        </div>
                        <div class="SecondColumn">
                            <asp:TextBox ID="TextBoxModifierFour" runat="server" CssClass="TextBoxModifierFour"></asp:TextBox>
                        </div>
                    </div>
                    <div class="BillingInfoColumn3">
                        <asp:Label ID="LabelPlaceOfService" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:DropDownList ID="DropDownListPlaceOfService" runat="server" CssClass="DropDownListPlaceOfService">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSourceDropDownListPlaceOfService" runat="server"></asp:SqlDataSource>
                    </div>
                </div>
                <div class="newRow">
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxComments" runat="server" TextMode="MultiLine" CssClass="TextBoxComments"></asp:TextBox>
                    </div>
                    <div class="BillingInfoColumn3">
                        <asp:Label ID="LabelUnitRate" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:TextBox ID="TextBoxUnitRate" runat="server" CssClass="TextBoxUnitRate"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorUnitRate" runat="server"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="newRow">
                    <div class="BillingInfoColumn4">
                        <asp:Label ID="LabelClaimFrequencyTypeCode" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:TextBox ID="TextBoxClaimFrequencyTypeCode" runat="server" CssClass="TextBoxClaimFrequencyTypeCode"></asp:TextBox>
                    </div>
                    <div class="BillingInfoColumn5">
                        <asp:Label ID="LabelProviderSignatureOnFile" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:DropDownList ID="DropDownListProviderSignatureOnFile" runat="server" CssClass="DropDownListProviderSignatureOnFile">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="newRow">
                    <div class="BillingInfoColumn4">
                        <asp:Label ID="LabelMedicareAssignmentCode" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:DropDownList ID="DropDownListMedicareAssignmentCode" runat="server" CssClass="DropDownListMedicareAssignmentCode">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSourceDropDownListMedicareAssignmentCode" runat="server">
                        </asp:SqlDataSource>
                    </div>
                    <div class="BillingInfoColumn5">
                        <asp:Label ID="LabelAssignmentOfBenefitsIndicator" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:DropDownList ID="DropDownListAssignmentOfBenefitsIndicator" runat="server" CssClass="DropDownListAssignmentOfBenefitsIndicator">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="newRow">
                    <div class="BillingInfoColumn4">
                        <asp:Label ID="LabelReleaseOfInformationCode" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:DropDownList ID="DropDownListReleaseOfInformationCode" runat="server" CssClass="DropDownListReleaseOfInformationCode">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSourceDropDownListReleaseOfInformationCode" runat="server">
                        </asp:SqlDataSource>
                    </div>
                    <div class="BillingInfoColumn5">
                        <asp:Label ID="LabelPatientSignatureCode" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
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
<div class="newRow">
</div>
<div class="ServiceBox">
    <div class="BoxStyle ServiceRight ServiceBoxAuthorizationDetail SectionDiv">
        <div id="Div1" class="SectionDiv-header">
            <asp:Label ID="LabelAuthorizationDetail" runat="server"></asp:Label>
        </div>
        <div class="newRow">
            <asp:UpdatePanel ID="UpdatePanelAuthorizationDetail" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <div class="newRow">
                        <div class="AuthorizationDetailColumn1">
                            <asp:Label ID="LabelAuthorizatioReceivedDate" runat="server"></asp:Label>
                        </div>
                        <div class="SecondColumn">
                            <asp:TextBox ID="TextBoxAuthorizatioReceivedDate" runat="server" CssClass="dateField"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorAuthorizatioReceivedDate"
                                runat="server"></asp:RegularExpressionValidator>
                        </div>
                        <div class="AuthorizationDetailColumn2">
                            <asp:Label ID="LabelPractitionerStatementReceivedDate" runat="server"></asp:Label>
                        </div>
                        <div class="SecondColumn">
                            <asp:TextBox ID="TextBoxPractitionerStatementReceivedDate" runat="server" CssClass="dateField"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorPractitionerStatementReceivedDate"
                                runat="server"></asp:RegularExpressionValidator>
                        </div>
                    </div>
                    <div class="newRow">
                        <div class="AuthorizationDetailColumn1">
                            <asp:Label ID="LabelPractitionerStatementSentDate" runat="server"></asp:Label>
                        </div>
                        <div class="SecondColumn">
                            <asp:TextBox ID="TextBoxPractitionerStatementSentDate" runat="server" CssClass="dateField"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorPractitionerStatementSentDate"
                                runat="server"></asp:RegularExpressionValidator>
                        </div>
                        <div class="AuthorizationDetailColumn2">
                            <asp:Label ID="LabelServiceInitializedReportedDate" runat="server"></asp:Label>
                        </div>
                        <div class="SecondColumn">
                            <asp:TextBox ID="TextBoxServiceInitializedReportedDate" runat="server" CssClass="dateField"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorServiceInitializedReportedDate"
                                runat="server"></asp:RegularExpressionValidator>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="DivSpace3">
        </div>
    </div>
</div>
<div class="ServiceBox">
    <div class="BoxStyle ServiceRight ServiceBoxSantraxInformation SectionDiv">
        <div id="Div2" class="SectionDiv-header">
            <asp:Label ID="LabelSantraxInformation" runat="server"></asp:Label>
        </div>
        <asp:UpdatePanel ID="UpdatePanelSantraxInformation" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="newRow">
                    <div class="SantraxInformationColumn1">
                        <asp:Label ID="LabelProgramOrService" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:DropDownList ID="DropDownListProgramOrService" runat="server" CssClass="DropDownListProgramOrService">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="newRow">
                    <div class="SantraxInformationColumn2">
                        <asp:Label ID="LabelServiceGroup" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:DropDownList ID="DropDownListServiceGroup" runat="server" CssClass="DropDownListServiceGroup">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="newRow">
                    <div class="SantraxInformationColumn2">
                        <asp:Label ID="LabelServiceCode" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:DropDownList ID="DropDownListServiceCode" runat="server" CssClass="DropDownListServiceCode">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="newRow">
                    <div class="SantraxInformationColumn1">
                        <asp:Label ID="LabelServiceCodeDescription" runat="server"></asp:Label>
                    </div>
                    <div class="SecondColumn">
                        <asp:DropDownList ID="DropDownListServiceCodeDescription" runat="server" CssClass="DropDownListServiceCodeDescription">
                        </asp:DropDownList>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="DropDownListProgramOrService" />
                <asp:AsyncPostBackTrigger ControlID="DropDownListServiceGroup" />
                <asp:AsyncPostBackTrigger ControlID="DropDownListServiceCode" />
                <asp:AsyncPostBackTrigger ControlID="DropDownListServiceCodeDescription" />
            </Triggers>
        </asp:UpdatePanel>
        <div class="DivSpace3">
        </div>
    </div>
</div>
<div class="newRow DivButtonCenter">
    <asp:UpdatePanel ID="UpdatePanelActionButtons" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="AutoColumn">
                <asp:Button ID="ButtonSave" runat="server" CssClass="ButtonSave" />
            </div>
            <div class="AutoColumn">
                <asp:Button ID="ButtonDelete" runat="server" CssClass="ButtonDelete" />
            </div>
            <div class="AutoColumn">
                <asp:Button ID="ButtonClear" runat="server" CssClass="ButtonClear" />
            </div>
            <div class="AutoColumn">
                <asp:Button ID="ButtonCoordinationOfCare" runat="server" Text="Coordination of Care"
                    CssClass="ButtonCoordinationOfCare" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ButtonSave" />
            <asp:PostBackTrigger ControlID="ButtonDelete" />
            <asp:PostBackTrigger ControlID="ButtonClear" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<div class="DivSpace10">
    <asp:UpdatePanel ID="UpdatePanelHiddenField" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:HiddenField ID="HiddenFieldClientId" runat="server" />
            <asp:HiddenField ID="HiddenFieldIsNew" runat="server" />
            <asp:HiddenField ID="HiddenFieldIsSearched" runat="server" />
            <asp:HiddenField ID="HiddenFieldIndividualStatus" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
