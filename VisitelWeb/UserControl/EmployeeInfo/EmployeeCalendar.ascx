<%@ Control Language="vb" CodeBehind="EmployeeCalendar.ascx.vb" Inherits="VisitelWeb.Visitel.UserControl.EmployeeInfo.EmployeeCalendarControl" %>
<div class="newRow">
    <div id="Div12" class="SectionDiv-header">
        <asp:Label ID="LabelWeeklyCalendar" runat="server"></asp:Label>
        <uc1:EditSetting ID="UserControlEditSettingWeeklyCalendar" runat="server" />
    </div>
    <div class="newRow">
    </div>
    <asp:UpdatePanel ID="UpdatePanelCalendarInfo" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="newRow">
                <div class="AutoColumn">
                    <asp:Button ID="ButtonActiveOnly" runat="server" CssClass="ButtonActiveOnly" />
                </div>
                <div class="SecondColumn">
                    <asp:Button ID="ButtonInActiveOnly" runat="server" CssClass="ButtonInActiveOnly" />
                </div>
                <div class="SecondColumn">
                    <asp:Button ID="ButtonAll" runat="server" CssClass="ButtonAll" />
                </div>
                <div class="SecondColumn DivLabelHoursMinutes">
                    <asp:Label ID="LabelHoursMinutesCaption" runat="server"></asp:Label>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ButtonActiveOnly" />
            <asp:AsyncPostBackTrigger ControlID="ButtonInActiveOnly" />
            <asp:AsyncPostBackTrigger ControlID="ButtonAll" />
        </Triggers>
    </asp:UpdatePanel>
    <div class="newRow">
        <div class="SecondColumn DivLabelIndividual">
            <asp:Label ID="LabelIndividualCaption" runat="server"></asp:Label>
        </div>
        <div class="SecondColumn DivDay DivLabelSunday">
            <asp:Label ID="LabelSunday" runat="server"></asp:Label>
        </div>
        <div class="SecondColumn DivDay">
            <asp:Label ID="LabelMonday" runat="server"></asp:Label>
        </div>
        <div class="SecondColumn DivDay">
            <asp:Label ID="LabelTuesday" runat="server"></asp:Label>
        </div>
        <div class="SecondColumn DivDay">
            <asp:Label ID="LabelWednesday" runat="server"></asp:Label>
        </div>
        <div class="SecondColumn DivDay">
            <asp:Label ID="LabelThursday" runat="server"></asp:Label>
        </div>
        <div class="SecondColumn DivDay">
            <asp:Label ID="LabelFriday" runat="server"></asp:Label>
        </div>
        <div class="SecondColumn DivDay">
            <asp:Label ID="LabelSaturday" runat="server"></asp:Label>
        </div>
        <div class="SecondColumn DivTotal">
            <asp:Label ID="LabelTotal" runat="server"></asp:Label>
        </div>
        <div class="SecondColumn DivScheduleStatus">
            <asp:Label ID="LabelStatus" runat="server"></asp:Label>
        </div>
        <div class="SecondColumn DivCalendarUpdateDate">
            <asp:Label ID="LabelScheduleUpdateDate" runat="server"></asp:Label>
        </div>
        <div class="SecondColumn DivCalendarUpdateBy">
            <asp:Label ID="LabelScheduleUpdateBy" runat="server"></asp:Label>
        </div>
    </div>
    <div class="newRow">
    </div>
    <div id="Div13" class="SectionDiv-header">
        <asp:Label ID="LabelDetailInfo" runat="server"></asp:Label>
    </div>
    <div class="newRow">
        <asp:UpdatePanel ID="UpdatePanelWeeklyCalendar" runat="server" UpdateMode="Always"
            ClientIDMode="Static">
            <ContentTemplate>
                <asp:PlaceHolder ID="PlaceHolderCalendarSetting" runat="server"></asp:PlaceHolder>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:HiddenField ID="HiddenFieldScheduleId" runat="server" />
            <asp:HiddenField ID="HiddenFieldClientDetailId" runat="server" ClientIDMode="Static" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<div class="DivSpace10">
</div>
<div class="newRow DivButtonCenter">
    <asp:UpdatePanel ID="UpdatePanelCalendarActionButtons" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Button ID="ButtonSaveSchedule" runat="server" CssClass="ButtonSaveSchedule" />
            <asp:Button ID="ButtonIndividualDetail" runat="server" CssClass="ButtonIndividualDetail" />
            <asp:Button ID="ButtonClearSchedule" runat="server" CssClass="ButtonClearSchedule" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ButtonSaveSchedule" />
            <asp:AsyncPostBackTrigger ControlID="ButtonIndividualDetail" />
            <asp:AsyncPostBackTrigger ControlID="ButtonClearSchedule" />
        </Triggers>
    </asp:UpdatePanel>
</div>
