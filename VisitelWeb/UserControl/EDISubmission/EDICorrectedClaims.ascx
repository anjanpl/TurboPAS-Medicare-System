<%@ Control Language="vb" CodeBehind="EDICorrectedClaims.ascx.vb" Inherits="VisitelWeb.Visitel.UserControl.EDISubmission.EDICorrectedClaimsControl" %>
<div class="ServiceBox">
    <div class="BoxStyle ServiceFull EDICorrectedClaims SectionDiv">
        <asp:UpdatePanel ID="UpdatePanelEDICorrectedClaims" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelMedicaidNumber" runat="server" CssClass="LabelMedicaidNumber"
                                Text="Medicaid#:"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxMedicaidNumber" runat="server" CssClass="TextBoxMedicaidNumber"></asp:TextBox>
                    </div>
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelClientId" runat="server" CssClass="LabelClientId" Text="Client Id:"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxClientId" runat="server" CssClass="TextBoxClientId"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelIndividual" runat="server" CssClass="LabelIndividual" Text="Individual:"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListIndividual" runat="server" CssClass="DropDownListIndividual">
                        </asp:DropDownList>
                    </div>
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelProcedureCode" runat="server" CssClass="LabelProcedureCode" Text="Procedure Code:"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxProcedureCode" runat="server" CssClass="TextBoxProcedureCode"></asp:TextBox>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="DivSpace8">
        </div>
    </div>
</div>
<div class="DivSpace5">
</div>
<div class="ServiceBox">
    <div class="BoxStyle ServiceLeft1 EDICorrectedClaimsLeftInfo SectionDiv">
        <asp:UpdatePanel ID="UpdatePanelEDICorrectedClaimsLeftInfo" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelAddress" runat="server" CssClass="LabelAddress" Text="Address:"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxAddress" runat="server" CssClass="TextBoxAddress" TextMode="MultiLine"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelDateOfBirth" runat="server" CssClass="LabelDateOfBirth" Text="DOB:"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxDateOfBirth" runat="server" CssClass="TextBoxDateOfBirth"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelGender" runat="server" CssClass="LabelGender" Text="Gender:"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxGender" runat="server" CssClass="TextBoxGender"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelProgram" runat="server" CssClass="LabelProgram" Text="Program:"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxProgram" runat="server" CssClass="TextBoxProgram"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelStartDate" runat="server" CssClass="LabelStartDate" Text="Start:"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxStartDate" runat="server" CssClass="TextBoxStartDate"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelEndDate" runat="server" CssClass="LabelEndDate" Text="End:"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxEndDate" runat="server" CssClass="TextBoxEndDate"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelBilledHours" runat="server" CssClass="LabelBilledHours" Text="Billed Hours:"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxBilledHours" runat="server" CssClass="TextBoxBilledHours"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelUnitRate" runat="server" CssClass="LabelUnitRate" Text="Unit Rate:"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxUnitRate" runat="server" CssClass="TextBoxUnitRate"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelBilledAmount" runat="server" CssClass="LabelBilledAmount" Text="Billed Amt:"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxBilledAmount" runat="server" CssClass="TextBoxBilledAmount"></asp:TextBox>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="DivSpace8">
        </div>
    </div>
    <div class="BoxStyle ServiceLeft2 EDICorrectedClaimsRightInfo SectionDiv">
        <asp:UpdatePanel ID="UpdatePanelEDICorrectedClaimsRightInfo" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelContractNumber" runat="server" CssClass="LabelContractNumber"
                                Text="Contract#:"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxContractNumber" runat="server" CssClass="TextBoxContractNumber"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelReceiver" runat="server" CssClass="LabelReceiver" Text="Receiver:"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListReceiver" runat="server" CssClass="DropDownListReceiver">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelDiagnosisCodeOne" runat="server" CssClass="LabelDiagnosisCodeOne"
                                Text="Diag Code1:"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxDiagnosisCodeOne" runat="server" CssClass="TextBoxDiagnosisCodeOne"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelDiagnosisCodeTwo" runat="server" CssClass="LabelDiagnosisCodeTwo"
                                Text="Diag Code2:"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxDiagnosisCodeTwo" runat="server" CssClass="TextBoxDiagnosisCodeTwo"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelDiagnosisCodeThree" runat="server" CssClass="LabelDiagnosisCodeThree"
                                Text="Diag Code3:"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxDiagnosisCodeThree" runat="server" CssClass="TextBoxDiagnosisCodeThree"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelDiagnosisCodeFour" runat="server" CssClass="LabelDiagnosisCodeFour"
                                Text="Diag Code4:"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxDiagnosisCodeFour" runat="server" CssClass="TextBoxDiagnosisCodeFour"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelModifierOne" runat="server" CssClass="LabelModifierOne" Text="Mod1:"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxModifierOne" runat="server" CssClass="TextBoxModifierOne"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelModifierTwo" runat="server" CssClass="LabelModifierTwo" Text="Mod2:"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxModifierTwo" runat="server" CssClass="TextBoxModifierTwo"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelModifierThree" runat="server" CssClass="LabelModifierThree" Text="Mod3:"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxModifierThree" runat="server" CssClass="TextBoxModifierThree"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelModifierFour" runat="server" CssClass="LabelModifierFour" Text="Mod4:"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxModifierFour" runat="server" CssClass="TextBoxModifierFour"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon1">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelClaimFrequencyTypeCode" runat="server" CssClass="LabelClaimFrequencyTypeCode"
                                Text="Claim Frequency Type Code:"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListClaimFrequencyTypeCode" runat="server" CssClass="DropDownListClaimFrequencyTypeCode">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon1">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelClaimNumber" runat="server" CssClass="LabelClaimNumber" Text="Claim Number:"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxClaimNumber" runat="server" CssClass="TextBoxClaimNumber"></asp:TextBox>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="DivSpace8">
        </div>
    </div>
</div>
<div class="newRow DivButtonCenter">
    <asp:Button ID="ButtonSave" runat="server" Text="Save" CssClass="ButtonSave" />
    <asp:Button ID="ButtonDelete" runat="server" Text="Delete" CssClass="ButtonDelete" />
</div>
<div class="DivSpace8">
</div>
