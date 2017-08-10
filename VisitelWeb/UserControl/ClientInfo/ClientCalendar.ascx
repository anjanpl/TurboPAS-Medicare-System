<%@ Control Language="vb" CodeBehind="ClientCalendar.ascx.vb" Inherits="VisitelWeb.Visitel.UserControl.ClientInfo.ClientCalendarControl" %>
<div id="Div15" class="SectionDiv" runat="server">
    <div id="Div12" class="SectionDiv-header">
        <asp:Label ID="LabelWeeklyCalendar" runat="server"></asp:Label>
        <uc1:EditSetting ID="UserControlEditSettingWeeklyCalendar" runat="server" />
    </div>
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
            <div class="SecondColumn DivClientStatus">
                <asp:Label ID="LabelClientStatus" runat="server"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:DropDownList ID="DropDownListSearchByScheduleStatus" runat="server" CssClass="DropDownListSearchByScheduleStatus">
                </asp:DropDownList>
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
        <asp:AsyncPostBackTrigger ControlID="DropDownListSearchByScheduleStatus" />
    </Triggers>
</asp:UpdatePanel>
<div class="newRow">
    <div class="SecondColumn DivLabelEmployee">
        <asp:Label ID="LabelEmployeeCaption" runat="server"></asp:Label>
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
<asp:UpdatePanel ID="UpdatePanelWeeklyCalendar" runat="server" UpdateMode="Always"
    ClientIDMode="Static">
    <ContentTemplate>
        <asp:PlaceHolder ID="PlaceHolderCalendarSetting" runat="server"></asp:PlaceHolder>
        <div class="newRow DivButtonCenter">
            <asp:Button ID="ButtonClearSchedule" runat="server" CssClass="ButtonClearSchedule" />
            <asp:Button ID="ButtonSaveSchedule" runat="server" CssClass="ButtonSaveSchedule" />
            <asp:Button ID="ButtonEmployeeDetail" runat="server" CssClass="ButtonEmployeeDetail" />
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="ButtonSaveSchedule" />
        <asp:AsyncPostBackTrigger ControlID="ButtonClearSchedule" />
    </Triggers>
</asp:UpdatePanel>
<div class="DivSpace10">
    <asp:UpdatePanel ID="UpdatePanelScheduleHiddenFields" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:HiddenField ID="HiddenFieldScheduleId" runat="server" />
            <asp:HiddenField ID="HiddenFieldEmployeeDetailId" runat="server" ClientIDMode="Static" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
