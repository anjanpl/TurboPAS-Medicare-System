<%@ Control Language="vb" CodeBehind="IndividualOtherInformation.ascx.vb" Inherits="VisitelWeb.Visitel.UserControl.ClientInfo.IndividualOtherInformationControl" %>
<asp:UpdatePanel ID="UpdatePanelClientOtherInformation" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelSupervisorLastVisitDate" runat="server"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxSupervisorLastVisitDate" runat="server" CssClass="dateField TextBoxSupervisorLastVisitDate"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidatorSupervisorLastVisitDate"
                    runat="server"></asp:RegularExpressionValidator>
            </div>
            <div class="AutoColumn DivSuppliesOrEquipment">
                <asp:Label ID="LabelSuppliesOrEquipment" runat="server" CssClass="LabelSuppliesOrEquipment"></asp:Label>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxSuppliesOrEquipment" runat="server" CssClass="TextBoxSuppliesOrEquipment"
                    TextMode="MultiLine"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelDischargeReason" runat="server"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:DropDownList ID="DropDownListDischargeReason" runat="server" CssClass="DropDownListDischargeReason">
                </asp:DropDownList>
            </div>
            <div class="AutoColumnAddMore">
                <asp:LinkButton ID="LinkButtonAddMoreDischargeReason" ClientIDMode="Static" runat="server"
                    CssClass="thickbox ui-state-default ui-corner-all buttonLink">
                    <asp:Image ID="Image4" runat="server" ImageUrl="~/Images/AddMore.jpg" />
                </asp:LinkButton>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelCaseWorker" runat="server"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:DropDownList ID="DropDownListCaseWorker" runat="server" CssClass="DropDownListCaseWorker">
                </asp:DropDownList>
            </div>
            <div class="AutoColumnAddMore">
                <asp:LinkButton ID="LinkButtonAddMoreCaseWorker" ClientIDMode="Static" runat="server"
                    CssClass="thickbox ui-state-default ui-corner-all buttonLink">
                    <asp:Image ID="Image3" runat="server" ImageUrl="~/Images/AddMore.jpg" />
                </asp:LinkButton>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelDoctor" runat="server"></asp:Label>
            </div>
            <div class="AutoColumn">
                <asp:DropDownList ID="DropDownListDoctor" runat="server" CssClass="DropDownListDoctor">
                </asp:DropDownList>
            </div>
            <div class="AutoColumnAddMore">
                <asp:LinkButton ID="LinkButtonAddMoreDoctor" ClientIDMode="Static" runat="server"
                    CssClass="thickbox ui-state-default ui-corner-all buttonLink">
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/Images/AddMore.jpg" />
                </asp:LinkButton>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelPriority" runat="server"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:DropDownList ID="DropDownListPriority" runat="server" CssClass="DropDownListPriority">
                </asp:DropDownList>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelDiagnosis" runat="server"></asp:Label>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxDiagnosis" runat="server" TextMode="MultiLine" CssClass="TextBoxDiagnosis"></asp:TextBox>
            </div>
        </div>
        <div class="newRow">
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelClientType" runat="server"></asp:Label>
                </div>
            </div>
            <div class="AutoColumn">
                <asp:DropDownList ID="DropDownListClientType" runat="server" CssClass="DropDownListClientType">
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
            <div class="DivCommon">
                <div class="AutoColumn FloatRight">
                    <asp:Label ID="LabelUnits" runat="server"></asp:Label>
                </div>
            </div>
            <div class="SecondColumn">
                <asp:TextBox ID="TextBoxUnits" runat="server" CssClass="TextBoxUnits"></asp:TextBox>
                <%--<asp:RegularExpressionValidator ID="RegularExpressionValidatorUnits" runat="server"></asp:RegularExpressionValidator>--%>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelNumberOfWeekDays" runat="server"></asp:Label>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxNumberOfWeekDays" runat="server" CssClass="TextBoxNumberOfWeekDays"></asp:TextBox>
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
        <div class="newRow">
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="DropDownListClientType" />
    </Triggers>
</asp:UpdatePanel>
