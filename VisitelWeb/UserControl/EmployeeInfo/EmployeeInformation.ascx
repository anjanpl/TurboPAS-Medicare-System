<%@ Control Language="vb" CodeBehind="~/UserControl/EmployeeInfo/EmployeeInformation.ascx.vb"
    Inherits="VisitelWeb.Visitel.UserControl.EmployeeInfo.EmployeeInformationControl" %>
<asp:UpdatePanel ID="UpdatePanelEmployeeBulletInfo" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <div class="newRow">
            <div class="AutoColumn">
                <asp:Label ID="LabelEmploymentStatus" runat="server" CssClass="HighlightColumn"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:DropDownList ID="DropDownListEmploymentStatus" runat="server" CssClass="DropDownListEmploymentStatus HighlightColumn">
                </asp:DropDownList>
                <asp:SqlDataSource ID="SqlDataSourceDropDownListEmploymentStatus" runat="server">
                </asp:SqlDataSource>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelLicenseStatus" runat="server" CssClass="HighlightColumn"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:DropDownList ID="DropDownListLicenseStatus" runat="server" CssClass="DropDownListLicenseStatus HighlightColumn">
                </asp:DropDownList>
                <asp:SqlDataSource ID="SqlDataSourceDropDownListLicenseStatus" runat="server"></asp:SqlDataSource>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelClientGroup" runat="server" CssClass="HighlightColumn"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:DropDownList ID="DropDownListClientGroup" runat="server" CssClass="DropDownListClientGroup HighlightColumn">
                </asp:DropDownList>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelOIGResult" runat="server" CssClass="HighlightColumn"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:TextBox ID="TextBoxOIGResult" runat="server" CssClass="TextBoxOIGResult HighlightColumn"></asp:TextBox>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<div class="DivSpace5">
</div>
<div class="ServiceBox">
    <uc1:EditSetting ID="UserControlEditSettingEmployeeInfo" runat="server" />
    <div class="BoxStyle ServiceLeft ServiceBoxEmployeeInfo SectionDiv">
        <div id="Div3" class="SectionDiv-header">
            <asp:Label ID="LabelEmployeeDetailInfo" runat="server"></asp:Label>
        </div>
        <asp:UpdatePanel ID="UpdatePanelEmployeeDetailInfo" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelTitle" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListTitle" runat="server" CssClass="DropDownListTitle">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSourceDropDownListTitle" runat="server"></asp:SqlDataSource>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelTurboPASEmployeeId" runat="server" CssClass="LabelTurboPASEmployeeId"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxTurboPASEmployeeId" runat="server" CssClass="TextBoxTurboPASEmployeeId"></asp:TextBox>
                    </div>
                    <div class="AutoColumn">
                        <asp:Label ID="LabelPayrollId" runat="server" CssClass="LabelPayrollId"></asp:Label>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxPayrollId" runat="server" CssClass="TextBoxPayrollId"></asp:TextBox>
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
                        <asp:Label ID="LabelMiddleNameInitial" runat="server"></asp:Label>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxMiddleNameInitial" runat="server" CssClass="TextBoxMiddleNameInitial"></asp:TextBox>
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
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelAddress" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxAddress" runat="server" CssClass="TextBoxAddress"></asp:TextBox>
                    </div>
                    <div class="AutoColumn">
                        <asp:Label ID="LabelApartmentNumber" runat="server"></asp:Label>
                    </div>
                    <div class="AutoColumn">
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
                        <asp:TextBox ID="TextBoxDateOfBirth" runat="server" CssClass="dateField"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorDateOfBirth" runat="server"></asp:RegularExpressionValidator>
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
                            <asp:Label ID="LabelNumberOfDepartment" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxNumberOfDepartment" runat="server" CssClass="TextBoxNumberOfDepartment"></asp:TextBox>
                    </div>
                    <div class="AutoColumn">
                        <asp:Label ID="LabelNumberOfVerifiedReference" runat="server"></asp:Label>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxNumberOfVerifiedReference" runat="server" CssClass="TextBoxNumberOfVerifiedReference"></asp:TextBox>
                    </div>
                    <div class="AutoColumn">
                        <asp:Label ID="LabelPayrate" runat="server"></asp:Label>
                    </div>
                    <div class="AutoColumn">
                        $
                        <asp:TextBox ID="TextBoxPayrate" runat="server" CssClass="TextBoxPayrate"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorPayrate" runat="server"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelRehire" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListRehire" runat="server" CssClass="DropDownListRehire">
                        </asp:DropDownList>
                    </div>
                    <div class="AutoColumn">
                        <asp:Label ID="LabelComments" runat="server"></asp:Label>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxComments" runat="server" TextMode="MultiLine" CssClass="TextBoxComments"></asp:TextBox>
                    </div>
                </div>
                <div class="DivMailCheck">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelMailCheck" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:CheckBox ID="CheckBoxMailCheck" runat="server" CssClass="CheckBoxMailCheck" />
                    </div>
                </div>
                <div class="DivUpdateDate">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelUpdateDate" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxUpdateDate" runat="server" CssClass="TextBoxUpdateDate"></asp:TextBox>
                    </div>
                </div>
                <div class="DivUpdateBy">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelUpdateBy" runat="server"></asp:Label>
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
    </div>
</div>
<div class="ServiceBox">
    <div class="BoxStyle ServiceLeft ServiceBoxDates SectionDiv">
        <div id="Div1" class="SectionDiv-header">
            <asp:Label ID="LabelDates" runat="server"></asp:Label>
        </div>
        <asp:UpdatePanel ID="UpdatePanelDates" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelApplicationDate" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxApplicationDate" runat="server" CssClass="dateField"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorApplicationDate" runat="server"></asp:RegularExpressionValidator>
                    </div>
                    <div class="AutoColumn">
                        <asp:Button ID="ButtonAutoFill" runat="server" CssClass="ButtonAutoFill" />
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelReferenceVerificationDate" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxReferenceVerificationDate" runat="server" CssClass="dateField"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorReferenceVerificationDate"
                            runat="server"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelHireDate" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxHireDate" runat="server" CssClass="dateField"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorHireDate" runat="server"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelSignedJobDescriptionDate" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxSignedJobDescriptionDate" runat="server" CssClass="dateField"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorSignedJobDescriptionDate"
                            runat="server"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelOrientationDate" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxOrientationDate" runat="server" CssClass="dateField"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorOrientationDate" runat="server"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelAssignedTaskEvaluationDate" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxAssignedTaskEvaluationDate" runat="server" CssClass="dateField"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorAssignedTaskEvaluationDate"
                            runat="server"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:HyperLink ID="HyperLinkCrimcheckDate" runat="server" CssClass="HyperLinkCrimcheckDate"></asp:HyperLink>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxCrimcheckDate" runat="server" CssClass="dateField"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorCrimcheckDate" runat="server"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:HyperLink ID="HyperLinkMisConductRequest" runat="server" CssClass="HyperLinkMisConductRequest"></asp:HyperLink>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxRegistryDate" runat="server" CssClass="dateField"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorRegistryDate" runat="server"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelLastEvaluationDate" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxLastEvaluationDate" runat="server" CssClass="dateField"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorLastEvaluationDate"
                            runat="server"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelPolicyOrProcedureSettlementDate" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxPolicyOrProcedureSettlementDate" runat="server" CssClass="dateField"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorPolicyOrProcedureSettlementDate"
                            runat="server"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelOIGDate" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxOIGDate" runat="server" CssClass="dateField"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorOIGDate" runat="server"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelOIGReportedToStateDate" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxOIGReportedToStateDate" runat="server" CssClass="dateField"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorOIGReportedToStateDate"
                            runat="server"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelStartDate" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxStartDate" runat="server" CssClass="dateField"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorStartDate" runat="server"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelEndDate" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxEndDate" runat="server" CssClass="dateField"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorEndDate" runat="server"></asp:RegularExpressionValidator>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="DivSpace5">
        </div>
    </div>
</div>
<div class="newRow">
</div>
<%--<div class="ServiceBox">
    <div class="BoxStyle CMServiceLeft ServiceBoxCM2000 SectionDiv">
        <div id="Div4" class="SectionDiv-header">
            <asp:Label ID="LabelCM2000" runat="server"></asp:Label>
        </div>
        <asp:UpdatePanel ID="UpdatePanelCM2000" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="newRow">
                    <div class="DivCommonEVV">
                        <div class="AutoColumn FloatLeft">
                            <asp:Label ID="LabelCareId" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxCareId" runat="server" CssClass="TextBoxCareId"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonEVV">
                        <div class="AutoColumn FloatLeft">
                            <asp:Label ID="LabelCMPayrollId" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxCMPayrollId" runat="server" CssClass="TextBoxPayrollId"></asp:TextBox>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="DivSpace5">
        </div>
    </div>
</div>--%>
<%--<div class="ServiceBox">
    <div class="BoxStyle SantraxServiceLeft ServiceBoxEmployeeSantrax SectionDiv">
        <div id="Div2" class="SectionDiv-header">
            <asp:Label ID="LabelSantrax" runat="server"></asp:Label>
        </div>
        <asp:UpdatePanel ID="UpdatePanelEmployeeSantrax" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="newRow">
                    <div class="DivCommonEVV">
                        <div class="AutoColumn FloatLeft">
                            <asp:Label ID="LabelSantraxCDSPayrate" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        $
                        <asp:TextBox ID="TextBoxSantraxCDSPayrate" runat="server" CssClass="TextBoxSantraxCDSPayrate"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorSantraxCDSPayrate"
                            runat="server"></asp:RegularExpressionValidator>
                    </div>
                    <div class="AutoColumn">
                        <asp:Label ID="LabelSantraxEmployeeId" runat="server"></asp:Label>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxSantraxEmployeeId" runat="server" CssClass="TextBoxSantraxEmployeeId"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonEVV">
                        <div class="AutoColumn FloatLeft">
                            <asp:Label ID="LabelSantraxGPSPhone" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxSantraxGPSPhone" runat="server" CssClass="TextBoxSantraxGPSPhone"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorSantraxGPSPhone" runat="server"></asp:RegularExpressionValidator>
                    </div>
                    <div class="AutoColumn">
                        <asp:Label ID="LabelSantraxDiscipline" runat="server"></asp:Label>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxSantraxDiscipline" runat="server" CssClass="TextBoxSantraxDiscipline"></asp:TextBox>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="DivSpace5">
        </div>
    </div>
</div>--%>
<div class="ServiceBox">
    <div class="BoxStyle EVVServiceLeft ServiceBoxEVV SectionDiv">
        <div id="Div5" class="SectionDiv-header">
            <asp:Label ID="LabelEVV" runat="server"></asp:Label>
        </div>
        <asp:UpdatePanel ID="UpdatePanelEVV" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="newRow">
                    <div class="DivCommonEVV1">
                        <div class="AutoColumn FloatLeft">
                            <asp:Label ID="LabelEVVVendorId" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxEVVVendorId" runat="server" CssClass="TextBoxEVVVendorId"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommonEVV1">
                        <div class="AutoColumn FloatLeft">
                            <asp:Label ID="LabelEmployeeEVVId" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxEmployeeEVVId" runat="server" CssClass="TextBoxEmployeeEVVId"></asp:TextBox>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="DivSpace5">
        </div>
    </div>
</div>
<div class="newRow DivButtonCenter">
    <asp:UpdatePanel ID="UpdatePanelActionButtons" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Button ID="ButtonSave" runat="server" CssClass="ButtonSave" />
            <asp:Button ID="ButtonDelete" runat="server" CssClass="ButtonDelete" />
            <asp:Button ID="ButtonClear" runat="server" CssClass="ButtonClear" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ButtonSave" />
            <asp:AsyncPostBackTrigger ControlID="ButtonDelete" />
            <asp:AsyncPostBackTrigger ControlID="ButtonClear" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<div class="DivSpace10">
    <asp:UpdatePanel ID="UpdatePanelHiddenField" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:HiddenField ID="HiddenFieldIsNew" runat="server" />
            <asp:HiddenField ID="HiddenFieldEmployeeId" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
