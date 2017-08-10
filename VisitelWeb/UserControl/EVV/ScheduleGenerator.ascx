<%@ Control Language="vb" CodeBehind="~/UserControl/EVV/ScheduleGenerator.ascx.vb"
    Inherits="VisitelWeb.Visitel.UserControl.EVV.ScheduleGeneratorControl" %>
<div class="ServiceBox">
    <div class="BoxStyle ServiceLeft ServiceBoxScheduleGenerator SectionDiv">
        <div id="Div10" class="SectionDiv NoBorder" runat="server">
            <div id="Div8" class="SectionDiv-header">
                <asp:Label ID="LabelScheduleGenerator" runat="server"></asp:Label>
                <uc1:EditSetting ID="UserControlEditSetting" runat="server" />
            </div>
        </div>
        <div class="newRow">
            <div class="AutoColumn">
                <asp:Label ID="LabelCompanyName" runat="server" CssClass="LabelCompanyName"></asp:Label>
            </div>
        </div>
        <div class="newRow">
            <div class="AutoColumn">
                <asp:Label ID="LabelVestaAgencyIdCaption" runat="server" CssClass="LabelVestaAgencyIdCaption"></asp:Label>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelVestaAgencyId" runat="server" CssClass="LabelVestaAgencyId"></asp:Label>
            </div>
        </div>
        <div class="newRow">
            <div class="AutoColumn">
                <asp:Label ID="LabelTransmissionMessageSentToCaption" runat="server" CssClass="LabelTransmissionMessageSentToCaption"></asp:Label>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelTransmissionMessageSentTo" runat="server" CssClass="LabelTransmissionMessageSentTo"></asp:Label>
            </div>
        </div>
        <div class="DivSpace10">
        </div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="newRow">
                    <div class="AutoColumn">
                        <asp:Label ID="LabelUploadAllUpdatesCaption1" runat="server" CssClass="LabelUploadAllUpdatesCaption1"></asp:Label>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxDays" runat="server" CssClass="TextBoxDays"></asp:TextBox>
                    </div>
                    <div class="AutoColumn">
                        <asp:Label ID="LabelUploadAllUpdatesCaption2" runat="server" CssClass="LabelUploadAllUpdatesCaption2"></asp:Label>
                    </div>
                </div>
                <div class="DivSpace10">
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelPayPeriod" runat="server" CssClass="LabelPayPeriod"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxPayPeriodFromDate" runat="server" CssClass="TextBoxPayPeriodFromDate dateField"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorPayPeriodFromDate" runat="server"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorPayPeriodFromDate"
                            runat="server"></asp:RegularExpressionValidator>
                    </div>
                    <div class="AutoColumn">
                        --
                    </div>
                    <div class="AutoColumn">
                        <asp:TextBox ID="TextBoxPayPeriodToDate" runat="server" CssClass="TextBoxPayPeriodToDate dateField"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidatorPayPeriodToDate" runat="server"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidatorPayPeriodToDate" runat="server"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelClient" runat="server" CssClass="LabelClient"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListClient" runat="server" CssClass="DropDownListClient">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelClientType" runat="server" CssClass="LabelClientType"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListClientType" runat="server" CssClass="DropDownListClientType">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="newRow">
                    <div class="DivCommon">
                        <div class="AutoColumn FloatRight">
                            <asp:Label ID="LabelClientGroup" runat="server" CssClass="LabelClientGroup"></asp:Label>
                        </div>
                    </div>
                    <div class="AutoColumn">
                        <asp:DropDownList ID="DropDownListClientGroup" runat="server" CssClass="DropDownListClientGroup">
                        </asp:DropDownList>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="newRow DivButtonCenter">
            <asp:UpdatePanel ID="UpdatePanelActionButtons" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:Button ID="ButtonGenerateVestaSchedule" runat="server" CssClass="ButtonGenerateVestaSchedule" />
                    <asp:Button ID="ButtonAttendants" runat="server" CssClass="ButtonAttendants" />
                    <asp:Button ID="ButtonClients" runat="server" CssClass="ButtonClients" />
                    <asp:Button ID="ButtonAuthorizations" runat="server" CssClass="ButtonAuthorizations" />
                    <asp:Button ID="ButtonNewVisits" runat="server" CssClass="ButtonNewVisits" />
                    <asp:Button ID="ButtonLoggedVisits" runat="server" CssClass="ButtonLoggedVisits" />
                    <asp:Button ID="ButtonSyncRecords" runat="server" CssClass="ButtonSyncRecords" />
                    <asp:Button ID="ButtonClearClient" runat="server" CssClass="ButtonClearClient" />
                    <asp:Button ID="ButtonAutoGenerateEmployeeEVVIDs" runat="server" CssClass="ButtonAutoGenerateEmployeeEVVIDs" />
                    <asp:Button ID="ButtonAutoGenerateClientEVVIDs" runat="server" CssClass="ButtonAutoGenerateClientEVVIDs" />
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ButtonGenerateVestaSchedule" />
                    <asp:AsyncPostBackTrigger ControlID="ButtonAttendants" />
                    <asp:AsyncPostBackTrigger ControlID="ButtonClients" />
                    <asp:AsyncPostBackTrigger ControlID="ButtonAuthorizations" />
                    <asp:AsyncPostBackTrigger ControlID="ButtonNewVisits" />
                    <asp:AsyncPostBackTrigger ControlID="ButtonLoggedVisits" />
                    <asp:AsyncPostBackTrigger ControlID="ButtonSyncRecords" />
                    <asp:AsyncPostBackTrigger ControlID="ButtonClearClient" />
                    <asp:AsyncPostBackTrigger ControlID="ButtonAutoGenerateEmployeeEVVIDs" />
                    <asp:AsyncPostBackTrigger ControlID="ButtonAutoGenerateClientEVVIDs" />
                    <asp:AsyncPostBackTrigger ControlID="TextBoxDays" />
                    <asp:AsyncPostBackTrigger ControlID="TextBoxPayPeriodFromDate" />
                    <asp:AsyncPostBackTrigger ControlID="TextBoxPayPeriodToDate" />
                    <asp:AsyncPostBackTrigger ControlID="DropDownListClient" />
                    <asp:AsyncPostBackTrigger ControlID="DropDownListClientType" />
                    <asp:AsyncPostBackTrigger ControlID="DropDownListClientGroup" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <div class="DivSpace10">
        </div>
    </div>
</div>
