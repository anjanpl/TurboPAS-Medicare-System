<%@ Control Language="vb" CodeBehind="~/UserControl/Calendar.ascx.vb" Inherits="VisitelWeb.Visitel.UserControl.Settings.CalendarSetting" %>
<div>
    <div id="Div3" class="SectionDiv-header">
        <asp:Label ID="LabelWeeklyCalendar" runat="server"></asp:Label>
        <uc1:EditSetting ID="UserControlEditSetting" runat="server" />
    </div>
    <div class="newRow">
    </div>
    <asp:UpdatePanel ID="UpdatePanelCalendarInfo" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="newRow">
                <div class="AutoColumn">
                    <asp:Label ID="LabelClient" runat="server" Text="Client"></asp:Label>
                </div>
                <div class="SecondColumn">
                    <asp:DropDownList ID="DropDownListSearchByClient" runat="server" CssClass="DropDownListSearchByClient">
                    </asp:DropDownList>
                </div>
                <div class="SecondColumn">
                    <asp:Button ID="ButtonAll" runat="server" CssClass="ButtonAll" />
                </div>
                <div class="SecondColumn DivLabelClientStatus">
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
            <asp:AsyncPostBackTrigger ControlID="DropDownListSearchByClient" />
            <asp:AsyncPostBackTrigger ControlID="ButtonAll" />
            <asp:AsyncPostBackTrigger ControlID="DropDownListSearchByScheduleStatus" />
        </Triggers>
    </asp:UpdatePanel>
    <div class="newRow">
        <div class="AutoColumn DivLabelIndividual">
            <asp:Label ID="LabelIndividualCaption" runat="server"></asp:Label>
        </div>
        <div class="SecondColumn DivLabelIndividualType">
            <asp:Label ID="LabelEmployeeCaption" runat="server"></asp:Label>
        </div>
        <div class="SecondColumn DivDay">
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
    <div id="Div1" class="SectionDiv-header">
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
</div>
<div class="DivSpace10">
</div>
<div class="newRow DivButtonCenter">
    <asp:UpdatePanel ID="UpdatePanelActionButtons" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Button ID="ButtonSaveSchedule" runat="server" CssClass="ButtonSaveSchedule" />
            <asp:Button ID="ButtonIndividualDetail" runat="server" CssClass="ButtonIndividualDetail" />
            <asp:Button ID="ButtonClearSchedule" runat="server" CssClass="ButtonClearSchedule" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ButtonSaveSchedule" />
            <asp:PostBackTrigger ControlID="ButtonIndividualDetail" />
            <asp:PostBackTrigger ControlID="ButtonClearSchedule" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<div class="DivSpace10">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:HiddenField ID="HiddenFieldScheduleId" runat="server" />
            <asp:HiddenField ID="HiddenFieldClientDetailId" runat="server" ClientIDMode="Static" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
