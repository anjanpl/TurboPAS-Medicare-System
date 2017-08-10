<%@ Control Language="vb" CodeBehind="~/UserControl/EDICodes/EDISubmission.ascx.vb"
    Inherits="VisitelWeb.Visitel.UserControl.EDISubmission.EDISubmissionControl" %>
<div>
    <div id="Div10" class="SectionDiv NoBorder" runat="server">
        <div id="Div8" class="SectionDiv-header">
            <asp:Label ID="LabelEDISubmission" runat="server"></asp:Label>
            <uc1:EditSetting ID="UserControlEditSettingEDISubmission" runat="server" />
        </div>
    </div>
</div>
<asp:SqlDataSource ID="SqlDataSourceDropDownListPlaceOfService" runat="server"></asp:SqlDataSource>
<div class="ServiceBox">
    <div class="BoxStyle ServiceLeft ServiceBoxViewRecords SectionDiv">
        <div id="Div5" class="SectionDiv-header">
            <asp:Label ID="LabelViewRecords" runat="server"></asp:Label>
        </div>
        <div class="DivSpace3">
        </div>
        <asp:UpdatePanel ID="UpdatePanelViewRecords" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="newRow">
                    <div class="AutoColumn DivLabelPayPeriodStartDate">
                        <asp:Label ID="LabelPayPeriodStartDate" runat="server" CssClass="LabelPayPeriodStartDate"></asp:Label>
                    </div>
                    <div class="AutoColumn DivLabelPayPeriodEndDate">
                        <asp:Label ID="LabelPayPeriodEndDate" runat="server" CssClass="LabelPayPeriodEndDate"></asp:Label>
                    </div>
                </div>
                <div class="newRow">
                    <asp:SqlDataSource ID="SqlDataSourcePayPeriodStartEndDate" runat="server"></asp:SqlDataSource>
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelPayPeriod" runat="server" CssClass="LabelPayPeriod"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListPayPeriodStartDate" runat="server" CssClass="DropDownListPayPeriodStartDate">
                        </asp:DropDownList>
                    </div>
                    <div class="AutoColumn">
                        -
                    </div>
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListPayPeriodEndDate" runat="server" CssClass="DropDownListPayPeriodEndDate">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelContractNumber" runat="server" CssClass="LabelContractNumber"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListContractNumber" runat="server" CssClass="DropDownListContractNumber">
                        </asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSourceContractInfo" runat="server"></asp:SqlDataSource>
                    </div>
                </div>
                <div class="newRow">
                    <div class="AutoColumn">
                        <asp:Button ID="ButtonViewRecords" runat="server" CssClass="ButtonViewRecords" />
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="DropDownListPayPeriodStartDate" />
                <asp:AsyncPostBackTrigger ControlID="DropDownListPayPeriodEndDate" />
                <asp:AsyncPostBackTrigger ControlID="ButtonViewRecords" />
            </Triggers>
        </asp:UpdatePanel>
        <div class="DivSpace30">
        </div>
    </div>
    <div class="BoxStyle ServiceLeft ServiceBoxFTPSend SectionDiv">
        <div id="Div6" class="SectionDiv-header">
            <asp:Label ID="LabelFTPSend" runat="server"></asp:Label>
        </div>
        <div class="DivSpace3">
        </div>
        <asp:UpdatePanel ID="UpdatePanelFTPSend" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelUserName" runat="server" CssClass="LabelUserName"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListUserName" runat="server" CssClass="DropDownListUserName">
                        </asp:DropDownList>
                    </div>
                    <div class="AutoColumn">
                        <asp:Button ID="ButtonFtpSend" runat="server" CssClass="ButtonFtpSend" />
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelPassword" runat="server" CssClass="LabelPassword"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxPassword" runat="server" CssClass="TextBoxPassword"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelFtpSite" runat="server" CssClass="LabelFtpSite"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxFtpSite" runat="server" CssClass="TextBoxFtpSite"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelDirectory" runat="server" CssClass="LabelDirectory"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxDirectory" runat="server" CssClass="TextBoxDirectory"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                    <div class="AutoColumn DivTexMedConnect">
                        <asp:HyperLink ID="HyperLinkTexMedConnect" runat="server" Text="TexMedConnect" CssClass="HyperLinkTexMedConnect"></asp:HyperLink>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="DropDownListUserName" />
                <asp:AsyncPostBackTrigger ControlID="ButtonFtpSend" />
            </Triggers>
        </asp:UpdatePanel>
        <div class="DivSpace30">
        </div>
    </div>
    <div class="BoxStyle ServiceLeft ServiceBoxViewFTP SectionDiv">
        <div id="Div1" class="SectionDiv-header">
            <asp:Label ID="LabelViewFTP" runat="server" Text="View FTP"></asp:Label>
        </div>
        <div class="DivSpace3">
        </div>
        <asp:UpdatePanel ID="UpdatePanelViewFTP" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Button ID="ButtonPopulateListBox" runat="server" Text="Populate List Box" CssClass="ButtonPopulateListBox" />
                        </div>
                    </div>
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Button ID="ButtonClearList" runat="server" Text="Clear List" CssClass="ButtonClearList" />
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:ListBox ID="ListBoxNamesList" runat="server" CssClass="ListBoxFtpInfo"></asp:ListBox>
                        <asp:SqlDataSource ID="SqlDataSourceNamesList" runat="server"></asp:SqlDataSource>
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ButtonPopulateListBox" />
                <asp:AsyncPostBackTrigger ControlID="ButtonClearList" />
                <asp:AsyncPostBackTrigger ControlID="ListBoxNamesList" />
            </Triggers>
        </asp:UpdatePanel>
        <div class="DivSpace8">
        </div>
    </div>
</div>
<div class="DivSpace5">
</div>
<div class="ServiceBox">
    <div class="BoxStyle ServiceFull ServiceBoxEDISubmissionDetail SectionDiv">
        <div id="Div2" class="SectionDiv-header">
            <asp:Label ID="LabelEDISubmissionDetail" runat="server" Text="EDI Submission Detail"></asp:Label>
        </div>
        <div class="newRow HorizontalScroll">
            <asp:UpdatePanel ID="UpdatePanelEDISubmissionDetail" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:HiddenField ID="HiddenFieldClientId" runat="server" />
                    <asp:GridView ID="GridViewEDISubmissionDetail" runat="server">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <div class="DivSerial">
                                        <asp:Label runat="server" ID="LabelHeaderSerial" CssClass="LabelHeaderSerial" />
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="DivSerial">
                                        <asp:Label runat="server" ID="LabelSerial" Text='<%#Container.DataItemIndex + 1 %>' />
                                    </div>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <div class="DivSerial">
                                        <asp:Label runat="server" ID="LabelSerial" />
                                    </div>
                                </FooterTemplate>
                                <ItemStyle Width="40px" HorizontalAlign="Right" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <div class="DivCheckBoxSelect">
                                        <asp:Label runat="server" ID="LabelHeaderSelect" CssClass="LabelHeaderSelect" />
                                        <asp:CheckBox ID="chkAll" runat="server" OnCheckedChanged="OnCheckedChanged" />
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="DivCheckBoxSelect">
                                        <asp:CheckBox ID="CheckBoxSelect" runat="server" OnCheckedChanged="OnCheckedChanged" />
                                    </div>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <div class="DivCheckBoxSelect">
                                        <asp:CheckBox ID="CheckBoxSelect" runat="server" OnCheckedChanged="OnCheckedChanged" />
                                    </div>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Button ID="ButtonHeaderStartDate" runat="server" CssClass="ButtonHeaderStartDate" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxStartDate" runat="server" CssClass="TextBoxStartDate" Text='<%#Bind("StartDate") %>'></asp:TextBox>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TextBoxStartDate" runat="server" CssClass="TextBoxStartDate"></asp:TextBox>
                                </FooterTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Button ID="ButtonHeaderEndDate" runat="server" CssClass="ButtonHeaderEndDate" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxEndDate" runat="server" CssClass="TextBoxEndDate" Text='<%#Bind("EndDate") %>'></asp:TextBox>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TextBoxEndDate" runat="server" CssClass="TextBoxEndDate"></asp:TextBox>
                                </FooterTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Button ID="ButtonHeaderName" runat="server" CssClass="ButtonHeaderName" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxName" runat="server" CssClass="TextBoxName" Text='<%#Bind("ClientFullName") %>'></asp:TextBox>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TextBoxName" runat="server" CssClass="TextBoxName"></asp:TextBox>
                                </FooterTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Button ID="ButtonHeaderAddress" runat="server" CssClass="ButtonHeaderAddress" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxAddress" runat="server" TextMode="MultiLine" CssClass="TextBoxAddress"
                                        Text='<%#Bind("ClientFullAddress") %>'></asp:TextBox>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TextBoxAddress" runat="server" CssClass="TextBoxAddress"></asp:TextBox>
                                </FooterTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="LabelHeaderReferralNumber" runat="server" CssClass="LabelHeaderReferralNumber"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxReferralNumber" runat="server" CssClass="TextBoxReferralNumber"
                                        Text='<%#Bind("AuthorizationNumber") %>'></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="LabelHeaderGender" runat="server" CssClass="LabelHeaderGender"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxGender" runat="server" CssClass="TextBoxGender" Text='<%#Bind("Gender") %>'></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="LabelHeaderDateOfBirth" runat="server" CssClass="LabelHeaderDateOfBirth"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxDateOfBirth" runat="server" CssClass="TextBoxDateOfBirth"
                                        Text='<%#Bind("DateOfBirth") %>'></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="LabelHeaderMedicaidNumber" runat="server" CssClass="LabelHeaderMedicaidNumber"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxMedicaidNumber" runat="server" CssClass="TextBoxMedicaidNumber"
                                        Text='<%#Bind("StateClientId") %>'></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="LabelHeaderBillUnits" runat="server" CssClass="LabelHeaderBillUnits"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxBillUnits" runat="server" CssClass="TextBoxBillUnits" Text='<%#Bind("BillHours") %>'></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="LabelHeaderUnitRate" runat="server" CssClass="LabelHeaderUnitRate"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxUnitRate" runat="server" CssClass="TextBoxUnitRate" Text='<%#Bind("UnitRate") %>'></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="LabelHeaderAmount" runat="server" CssClass="LabelHeaderAmount"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxAmount" runat="server" CssClass="TextBoxAmount" Text='<%#Bind("MonetaryAmount") %>'></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="LabelHeaderPlaceOfService" runat="server" CssClass="LabelHeaderPlaceOfService"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:DropDownList ID="DropDownListPlaceOfService" runat="server" CssClass="DropDownListPlaceOfService">
                                    </asp:DropDownList>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="LabelHeaderProcedureCode" runat="server" CssClass="LabelHeaderProcedureCode"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxProcedureCode" runat="server" CssClass="TextBoxProcedureCode"
                                        Text='<%#Bind("ProcedureId") %>'></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <div class="DivHeaderModifiers">
                                        <asp:Label ID="LabelHeaderModifiers" runat="server" CssClass="LabelHeaderModifiers"></asp:Label>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="DivModifiers">
                                        <asp:TextBox ID="TextBoxModifierOne" runat="server" CssClass="TextBoxModifierOne"
                                            Text='<%#Bind("ModifierOne") %>'></asp:TextBox>
                                        <asp:TextBox ID="TextBoxModifierTwo" runat="server" CssClass="TextBoxModifierTwo"
                                            Text='<%#Bind("ModifierTwo") %>'></asp:TextBox>
                                        <asp:TextBox ID="TextBoxModifierThree" runat="server" CssClass="TextBoxModifierThree"
                                            Text='<%#Bind("ModifierThree") %>'></asp:TextBox>
                                        <asp:TextBox ID="TextBoxModifierFour" runat="server" CssClass="TextBoxModifierFour"
                                            Text='<%#Bind("ModifierFour") %>'></asp:TextBox>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <div class="DivHeaderDiagnosis">
                                        <asp:Label ID="LabelHeaderDiagnosis" runat="server" CssClass="LabelHeaderDiagnosis"></asp:Label>
                                    </div>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="DivDiagnosis">
                                        <asp:DropDownList ID="DropDownListDiagnosisOne" runat="server" CssClass="DropDownListDiagnosisOne">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="DropDownListDiagnosisTwo" runat="server" CssClass="DropDownListDiagnosisTwo">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="DropDownListDiagnosisThree" runat="server" CssClass="DropDownListDiagnosisThree">
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="DropDownListDiagnosisFour" runat="server" CssClass="DropDownListDiagnosisFour">
                                        </asp:DropDownList>
                                    </div>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="LabelId" runat="server" Text='<%#Bind("Id") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="LabelId" runat="server"></asp:Label>
                                </FooterTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="LabelClientId" runat="server" Text='<%#Bind("ClientId") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="LabelClientId" runat="server"></asp:Label>
                                </FooterTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="LabelProgram" runat="server" Text='<%#Bind("Name") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="LabelProgram" runat="server"></asp:Label>
                                </FooterTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="LabelReceiver" runat="server" Text='<%#Bind("ReceiverId") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="LabelReceiver" runat="server"></asp:Label>
                                </FooterTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="LabelContract" runat="server" Text='<%#Bind("ContractNumber") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="LabelContract" runat="server"></asp:Label>
                                </FooterTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="LabelClaimFrequencyTypeCode" runat="server" Text='<%#Bind("CLM0503ClmFrequencyTypeCode") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="LabelClaimFrequencyTypeCode" runat="server"></asp:Label>
                                </FooterTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:Label ID="LabelNoDataFound" Text="No Data Found" runat="server"></asp:Label>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="GridViewEDISubmissionDetail" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
<div class="DivSpace5">
</div>
<div class="ServiceBox">
    <div class="BoxStyle ServiceFull ServiceBoxMiscAction SectionDiv">
        <div id="Div3" class="SectionDiv-header">
            <asp:Label ID="LabelMiscAction" runat="server" Text="Misc Action"></asp:Label>
        </div>
        <div class="DivSpace3">
        </div>
        <div class="newRow">
            <div class="AutoColumn">
                <asp:Label ID="LabelResubmission" runat="server" Text="Is Resubmission" CssClass="LabelResubmission"></asp:Label>
            </div>
            <div class="AutoColumn">
                <asp:CheckBox ID="CheckBoxResubmission" runat="server" CssClass="CheckBoxResubmission" />
            </div>
            <div class="AutoColumn DivLabelComment">
                <asp:Label ID="LabelComment" runat="server" CssClass="LabelComment"></asp:Label>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelSystemDate" runat="server" Text="System Date" CssClass="LabelSystemDate"></asp:Label>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxSystemDate" runat="server" CssClass="TextBoxSystemDate"></asp:TextBox>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelTotalBillAmount" runat="server" Text="Total Bill Amount" CssClass="LabelTotalBillAmount"></asp:Label>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxTotalBillAmount" runat="server" CssClass="TextBoxTotalBillAmount"></asp:TextBox>
            </div>
        </div>
        <div class="DivSpace5">
        </div>
        <asp:UpdatePanel ID="UpdatePanelButtons" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="newRow">
                    <div class="AutoColumn1">
                        <asp:Button ID="ButtonCorrectClaim" runat="server" CssClass="ButtonCorrectClaim" />
                    </div>
                    <div id="hiddenModalContent" style="display: none">
                    </div>
                    <div class="AutoColumn1">
                        <asp:Button ID="ButtonRefresh" runat="server" CssClass="ButtonRefresh" />
                    </div>
                    <div class="AutoColumn1">
                        <asp:Button ID="ButtonDeleteSubmissionList" runat="server" CssClass="ButtonDeleteSubmissionList" />
                    </div>
                    <div class="AutoColumn1">
                        <asp:TextBox ID="TextBoxSaveLocation" runat="server" CssClass="TextBoxDestinationPath"></asp:TextBox>
                    </div>
                    <div class="AutoColumn1">
                        <asp:Button ID="ButtonGenerate" runat="server" CssClass="ButtonGenerate" />
                    </div>
                    <div class="AutoColumn1">
                        <asp:Button ID="ButtonSubmissionReport" runat="server" CssClass="ButtonSubmissionReport" />
                    </div>
                    <div class="AutoColumn1">
                        <asp:Button ID="ButtonIndividualBilling" runat="server" CssClass="ButtonIndividualBilling" />
                    </div>
                    <div class="AutoColumn1">
                        <asp:Button ID="ButtonIndividualDetail" runat="server" CssClass="ButtonIndividualDetail" />
                    </div>
                    <div class="AutoColumn1">
                        <asp:Button ID="ButtonSubmissionInfo" runat="server" CssClass="ButtonSubmissionInfo" />
                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="TextBoxSaveLocation" />
                <asp:AsyncPostBackTrigger ControlID="ButtonGenerate" />
            </Triggers>
        </asp:UpdatePanel>
        <div class="DivSpace8">
        </div>
    </div>
    <div class="DivSpace8">
    </div>
</div>
<%--<asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>--%>