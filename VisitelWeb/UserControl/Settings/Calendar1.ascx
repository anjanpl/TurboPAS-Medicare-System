<%@ Control Language="vb" CodeBehind="Calendar1.ascx.vb" Inherits="VisitelWeb.Visitel.UserControl.Settings.CalendarSetting1" %>
<%@ Register TagPrefix="uc" TagName="ClockTime" Src="~/UserControl/ClockTime.ascx" %>
<div class="newRow">
    <div id="UpdatePanelWeeklyCalendar">
        <asp:UpdatePanel ID="UpdatePanelWeelyScheduleHeader" runat="server" UpdateMode="Always"
            ClientIDMode="Static" Class="UpdatePanelManage">
            <ContentTemplate>
                <div class="newRow">
                    <div id="divDropDownListClient" class="AutoColumn DivLabelIndividual">
                        <asp:DropDownList ID="DropDownListClient" runat="server" CssClass="DropDownListClient">
                        </asp:DropDownList>
                    </div>
                    <div id="divDropDownListEmployee" class="SecondColumn DivLabelEmployeeType">
                        <asp:DropDownList ID="DropDownListEmployee" runat="server" CssClass="DropDownListEmployee">
                        </asp:DropDownList>
                    </div>
                    <div id="divTextBoxSundayHourMinute" class="SecondColumn DivDay">
                        <asp:TextBox ID="TextBoxSundayHourMinute" runat="server" MaxLength="5" ReadOnly="true"
                            placeholder="hh:mm" CssClass="TextBoxSundayHourMinute ci-heading ci-shadow-inset ui-input-text ui-body-c ui-shadow-inset"></asp:TextBox>
                    </div>
                    <div id="divTextBoxMondayHourMinute" class="SecondColumn DivDay">
                        <asp:TextBox ID="TextBoxMondayHourMinute" runat="server" MaxLength="5" ReadOnly="true"
                            placeholder="hh:mm" CssClass="TextBoxMondayHourMinute ci-heading ci-shadow-inset ui-input-text ui-body-c ui-shadow-inset"></asp:TextBox>
                    </div>
                    <div id="divTextBoxTuesdayHourMinute" class="SecondColumn DivDay">
                        <asp:TextBox ID="TextBoxTuesdayHourMinute" runat="server" MaxLength="5" ReadOnly="true"
                            placeholder="hh:mm" CssClass="TextBoxTuesdayHourMinute ci-heading ci-shadow-inset ui-input-text ui-body-c ui-shadow-inset"></asp:TextBox>
                    </div>
                    <div id="divTextBoxWednesdayHourMinute" class="SecondColumn DivDay">
                        <asp:TextBox ID="TextBoxWednesdayHourMinute" runat="server" MaxLength="5" ReadOnly="true"
                            placeholder="hh:mm" CssClass="TextBoxWednesdayHourMinute ci-heading ci-shadow-inset ui-input-text ui-body-c ui-shadow-inset"></asp:TextBox>
                    </div>
                    <div id="divTextBoxThursdayHourMinute" class="SecondColumn DivDay">
                        <asp:TextBox ID="TextBoxThursdayHourMinute" runat="server" MaxLength="5" ReadOnly="true"
                            placeholder="hh:mm" CssClass="TextBoxThursdayHourMinute ci-heading ci-shadow-inset ui-input-text ui-body-c ui-shadow-inset"></asp:TextBox>
                    </div>
                    <div id="divTextBoxFridayHourMinute" class="SecondColumn DivDay">
                        <asp:TextBox ID="TextBoxFridayHourMinute" runat="server" MaxLength="5" ReadOnly="true"
                            placeholder="hh:mm" CssClass="TextBoxFridayHourMinute ci-heading ci-shadow-inset ui-input-text ui-body-c ui-shadow-inset"></asp:TextBox>
                    </div>
                    <div id="divTextBoxSaturdayHourMinute" class="SecondColumn DivDay">
                        <asp:TextBox ID="TextBoxSaturdayHourMinute" runat="server" MaxLength="5" ReadOnly="true"
                            placeholder="hh:mm" CssClass="TextBoxSaturdayHourMinute ci-heading ci-shadow-inset ui-input-text ui-body-c ui-shadow-inset"></asp:TextBox>
                    </div>
                    <div id="divTextBoxTotalHourMinute" class="SecondColumn DivTotal">
                        <asp:TextBox ID="TextBoxTotalHourMinute" runat="server" MaxLength="5" ReadOnly="true"
                            placeholder="hh:mm" CssClass="TextBoxTotalHourMinute ci-heading ci-shadow-inset ui-input-text ui-body-c ui-shadow-inset"></asp:TextBox>
                    </div>
                    <div id="divDropDownListScheduleStatus" class="SecondColumn DivScheduleStatus">
                        <asp:DropDownList ID="DropDownListScheduleStatus" runat="server" CssClass="DropDownListScheduleStatus ci-heading ci-shadow-inset ui-input-text ui-body-c ui-shadow-inset">
                        </asp:DropDownList>
                    </div>
                    <div id="divTextBoxUpdateDate" class="SecondColumn DivCalendarUpdateDate">
                        <asp:TextBox ID="TextBoxCalendarUpdateDate" runat="server" MaxLength="17" ReadOnly="true"
                            CssClass="TextBoxCalendarUpdateDate ci-heading ci-shadow-inset ui-input-text ui-body-c ui-shadow-inset"></asp:TextBox>
                    </div>
                    <div id="divTextBoxUpdateBy" class="SecondColumn DivCalendarUpdateBy">
                        <asp:TextBox ID="TextBoxCalendarUpdateBy" runat="server" MaxLength="50" ReadOnly="true"
                            CssClass="TextBoxCalendarUpdateBy ci-heading ci-shadow-inset ui-input-text ui-body-c ui-shadow-inset"></asp:TextBox>
                    </div>
                </div>
                <div class="newRow">
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanelManage" runat="server" UpdateMode="Always" ClientIDMode="Static"
            Class="UpdatePanelManage">
            <ContentTemplate>
                <div class="CalendarServiceBox">
                    <div class="CalendarBoxStyle CalendarServiceLeft ServiceDaysInOut">
                        <div class="newRow">
                            <div id="divLabelDayCaptionFirstColumn" class="AutoColumn DivLabelDayCaption">
                                <asp:Label ID="LabelDayCaptionFirstColumn" runat="server" Text="DAY"></asp:Label>
                            </div>
                            <div id="divLabelInCaptionFirstColumn" class="SecondColumn DivLabelInCaption">
                                <asp:Label ID="LabelInCaptionFirstColumn" runat="server" Text="IN"></asp:Label>
                            </div>
                            <div id="divLabelOutCaptionFirstColumn" class="AutoColumn DivLabelOutCaption">
                                <asp:Label ID="LabelOutCaptionFirstColumn" runat="server" Text="OUT"></asp:Label>
                            </div>
                        </div>
                        <div class="newRow">
                            <div id="divLabelSundayInOutCaption" class="AutoColumn DivLabelDayCaption">
                                <asp:Label ID="LabelSundayInOutCaption" runat="server" Text="Sunday"></asp:Label>
                            </div>
                            <div id="divTextBoxSundayInTime" class="SecondColumn DivLabelInCaption">
                                <uc:ClockTime ID="ucClockTime_SundayInTime" runat="server" />
                            </div>
                            <div id="divTextBoxSundayOutTime" class="AutoColumn DivLabelOutCaption">
                                <uc:ClockTime ID="ucClockTime_SundayOutTime" runat="server" />
                            </div>
                        </div>
                        <div class="newRow">
                            <div id="divLabelMondayInOutCaption" class="AutoColumn DivLabelDayCaption">
                                <asp:Label ID="LabelMondayInOutCaption" runat="server" Text="Monday"></asp:Label></div>
                            <div id="divTextBoxMondayInTime" class="SecondColumn DivLabelInCaption">
                                <uc:ClockTime ID="ucClockTime_MondayInTime" runat="server" />
                            </div>
                            <div id="divTextBoxMondayOutTime" class="AutoColumn DivLabelOutCaption">
                                <uc:ClockTime ID="ucClockTime_MondayOutTime" runat="server" />
                            </div>
                        </div>
                        <div class="newRow">
                            <div id="divLabelTuesdayInOutCaption" class="AutoColumn DivLabelDayCaption">
                                <asp:Label ID="LabelTuesdayInOutCaption" runat="server" Text="Tuesday"></asp:Label>
                            </div>
                            <div id="divTextBoxTuesdayInTime" class="SecondColumn DivLabelInCaption">
                                <uc:ClockTime ID="ucClockTime_TuesdayInTime" runat="server" />
                            </div>
                            <div id="divTextBoxTuesdayOutTime" class="AutoColumn DivLabelOutCaption">
                                <uc:ClockTime ID="ucClockTime_TuesdayOutTime" runat="server" />
                            </div>
                        </div>
                        <div class="newRow">
                            <div id="divLabelWednesdayInOutCaption" class="AutoColumn DivLabelDayCaption">
                                <asp:Label ID="LabelWednesdayInOutCaption" runat="server" Text="Wednesday"></asp:Label>
                            </div>
                            <div id="divTextBoxWednesdayInTime" class="SecondColumn DivLabelInCaption">
                                <uc:ClockTime ID="ucClockTime_WednesdayInTime" runat="server" />
                            </div>
                            <div id="divTextBoxWednesdayOutTime" class="AutoColumn DivLabelOutCaption">
                                <uc:ClockTime ID="ucClockTime_WednesdayOutTime" runat="server" />
                            </div>
                        </div>
                        <div class="DivSpace5">
                        </div>
                    </div>
                </div>
                <div class="CalendarServiceBox">
                    <div class="CalendarBoxStyle CalendarServiceLeft ServiceDaysInOut">
                        <div class="newRow">
                            <div id="divLabelDayCaptionSecondColumn" class="AutoColumn DivLabelDayCaption">
                                <asp:Label ID="LabelDayCaptionSecondColumn" runat="server" Text="DAY"></asp:Label>
                            </div>
                            <div id="divLabelInCaptionSecondColumn" class="SecondColumn DivLabelInCaption">
                                <asp:Label ID="LabelInCaptionSecondColumn" runat="server" Text="IN"></asp:Label>
                            </div>
                            <div id="divLabelOutCaptionSecondColumn" class="AutoColumn DivLabelOutCaption">
                                <asp:Label ID="LabelOutCaptionSecondColumn" runat="server" Text="OUT"></asp:Label>
                            </div>
                        </div>
                        <div class="newRow">
                            <div id="divLabelThursdayInOutCaption" class="AutoColumn DivLabelDayCaption">
                                <asp:Label ID="LabelThursdayInOutCaption" runat="server" Text="Thursday"></asp:Label>
                            </div>
                            <div id="divTextBoxThursdayInTime" class="SecondColumn DivLabelInCaption">
                                <uc:ClockTime ID="ucClockTime_ThursdayInTime" runat="server" />
                            </div>
                            <div id="divTextBoxThursdayOutTime" class="AutoColumn DivLabelOutCaption">
                                <uc:ClockTime ID="ucClockTime_ThursdayOutTime" runat="server" />
                            </div>
                        </div>
                        <div class="newRow">
                            <div id="divLabelFridayInOutCaption" class="AutoColumn DivLabelDayCaption">
                                <asp:Label ID="LabelFridayInOutCaption" runat="server" Text="Friday"></asp:Label>
                            </div>
                            <div id="divTextBoxFridayInTime" class="SecondColumn DivLabelInCaption">
                                <uc:ClockTime ID="ucClockTime_FridayInTime" runat="server" />
                            </div>
                            <div id="divTextBoxFridayOutTime" class="AutoColumn DivLabelOutCaption">
                                <uc:ClockTime ID="ucClockTime_FridayOutTime" runat="server" />
                            </div>
                        </div>
                        <div class="newRow">
                            <div id="divLabelSaturdayInOutCaption" class="AutoColumn DivLabelDayCaption">
                                <asp:Label ID="LabelSaturdayInOutCaption" runat="server" Text="Saturday"></asp:Label>
                            </div>
                            <div id="divTextBoxSaturdayInTime" class="SecondColumn DivLabelInCaption">
                                <uc:ClockTime ID="ucClockTime_SaturdayInTime" runat="server" />
                            </div>
                            <div id="divTextBoxSaturdayOutTime" class="AutoColumn DivLabelOutCaption">
                                <uc:ClockTime ID="ucClockTime_SaturdayOutTime" runat="server" />
                            </div>
                        </div>
                        <div class="DivSpace5">
                        </div>
                    </div>
                </div>
                </div>
            </ContentTemplate>
            <%--<Triggers>
                <asp:AsyncPostBackTrigger ControlID="DropDownListClient" />
                <asp:AsyncPostBackTrigger ControlID="DropDownListEmployee" />
                <asp:AsyncPostBackTrigger ControlID="ucClockTime_SundayInTime" />
                <asp:AsyncPostBackTrigger ControlID="ucClockTime_SundayOutTime" />
                <asp:AsyncPostBackTrigger ControlID="ucClockTime_MondayInTime" />
                <asp:AsyncPostBackTrigger ControlID="ucClockTime_MondayOutTime" />
                <asp:AsyncPostBackTrigger ControlID="ucClockTime_TuesdayInTime" />
                <asp:AsyncPostBackTrigger ControlID="ucClockTime_TuesdayOutTime" />
                <asp:AsyncPostBackTrigger ControlID="ucClockTime_WednesdayInTime" />
                <asp:AsyncPostBackTrigger ControlID="ucClockTime_WednesdayOutTime" />
                <asp:AsyncPostBackTrigger ControlID="ucClockTime_ThursdayInTime" />
                <asp:AsyncPostBackTrigger ControlID="ucClockTime_ThursdayOutTime" />
                <asp:AsyncPostBackTrigger ControlID="ucClockTime_FridayInTime" />
                <asp:AsyncPostBackTrigger ControlID="ucClockTime_FridayOutTime" />
                <asp:AsyncPostBackTrigger ControlID="ucClockTime_SaturdayInTime" />
                <asp:AsyncPostBackTrigger ControlID="ucClockTime_SaturdayOutTime" />
            </Triggers>--%>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanelScheduleStartEnd" runat="server" UpdateMode="Always"
            ClientIDMode="Static" Class="UpdatePanelManage">
            <ContentTemplate>
                <div class="CalendarServiceBox">
                    <div class="CalendarBoxStyle CalendarServiceLeft ServiceDaysInOut">
                        <div class="newRow">
                            <div id="divLabelSpecialPayrate" class="SpecialPayrateColumn1">
                                <asp:Label ID="LabelSpecialPayrate" runat="server" Text="Special Payrate"></asp:Label>
                            </div>
                            <div class="SecondColumn">
                                <asp:TextBox ID="TextBoxSpecialPayrate" runat="server" MaxLength="10" CssClass="TextBoxSpecialPayrate ci-heading ci-shadow-inset ui-input-text ui-body-c ui-shadow-inset"></asp:TextBox>
                            </div>
                        </div>
                        <div class="newRow">
                            <div id="divLabelStartDate" class="SpecialPayrateColumn1">
                                <asp:Label ID="LabelStartDate" runat="server" Text="Start:"></asp:Label>
                            </div>
                            <div id="divTextBoxStartDate" class="SecondColumn">
                                <asp:TextBox ID="TextBoxStartDate" runat="server" MaxLength="17" CssClass="TextBoxStartDate dateField"></asp:TextBox>
                            </div>
                        </div>
                        <div class="newRow">
                            <div id="divLabelEndDate" class="SpecialPayrateColumn1">
                                <asp:Label ID="LabelEndDate" runat="server" Text="End:"></asp:Label>
                            </div>
                            <div id="divTextBoxEndDate" class="SecondColumn">
                                <asp:TextBox ID="TextBoxEndDate" MaxLength="17" CssClass="TextBoxEndDate dateField"
                                    runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="newRow">
                            <div id="divLabelEmptyColumn" class="SpecialPayrateColumn1">
                                <asp:Label ID="LabelEmptyColumn" runat="server">&nbsp;</asp:Label>
                            </div>
                            <div id="divTextBoxComments" class="AutoColumn DivTextBoxComments">
                                <asp:TextBox ID="TextBoxScheduleComments" runat="server" TextMode="MultiLine" MaxLength="17"
                                    Rows="2" cols="20" CssClass="TextBoxScheduleComments ci-heading ci-shadow-inset ui-input-text ui-body-c ui-shadow-inset"></asp:TextBox>
                            </div>
                        </div>
                        <div class="DivSpace5">
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            jQuery('#<%=TextBoxStartDate.ClientID%>').datepicker({
                showOn: 'button'
            , buttonImage: '../Images/calendar-blue.gif'
            , buttonImageOnly: true
            , buttonText: "Select date"
            , dateFormat: 'mm/dd/yy'
            , changeMonth: true
            , changeYear: true
            , onSelect: function () { }
            });

            jQuery('#<%=TextBoxEndDate.ClientID%>').datepicker({
                showOn: 'button'
            , buttonImage: '../Images/calendar-blue.gif'
            , buttonImageOnly: true
            , buttonText: "Select date"
            , dateFormat: 'mm/dd/yy'
            , changeMonth: true
            , changeYear: true
            , onSelect: function () { }
            });

            jQuery.mask.definitions['&'] = '[0123456789]';

            jQuery('#<%=TextBoxStartDate.ClientID%>').mask("&&/&&/&&&&", { "placeholder": "_" });
            jQuery('#<%=TextBoxEndDate.ClientID%>').mask("&&/&&/&&&&", { "placeholder": "_" });

            jQuery(".UpdatePanelScheduleStartEnd").live("click", function () {
                jQuery("#UpdatePanelWeeklyCalendar .UpdatePanelWeelyScheduleHeader").removeClass("UpdatePanelManageSelected");
                jQuery(this).addClass("UpdatePanelManageSelected");

                jQuery("#UpdatePanelWeeklyCalendar .UpdatePanelManage").removeClass("UpdatePanelManageSelected");
                jQuery(this).addClass("UpdatePanelManageSelected");

                jQuery("#UpdatePanelWeeklyCalendar .UpdatePanelScheduleStartEnd").removeClass("UpdatePanelManageSelected");
                jQuery(this).addClass("UpdatePanelManageSelected");
            });
        });

        HourMinuteChange();

        function HourMinuteChange() {
            var SundayInTime = document.getElementById('<%=ucClockTime_SundayInTime.FindControl("DropDownListTime").ClientID %>');
            var SundayOutTime = document.getElementById('<%=ucClockTime_SundayOutTime.FindControl("DropDownListTime").ClientID %>');

            var MondayInTime = document.getElementById('<%=ucClockTime_MondayInTime.FindControl("DropDownListTime").ClientID %>');
            var MondayOutTime = document.getElementById('<%=ucClockTime_MondayOutTime.FindControl("DropDownListTime").ClientID %>');

            var TuesdayInTime = document.getElementById('<%=ucClockTime_TuesdayInTime.FindControl("DropDownListTime").ClientID %>');
            var TuesdayOutTime = document.getElementById('<%=ucClockTime_TuesdayOutTime.FindControl("DropDownListTime").ClientID %>');

            var WednesdayInTime = document.getElementById('<%=ucClockTime_WednesdayInTime.FindControl("DropDownListTime").ClientID %>');
            var WednesdayOutTime = document.getElementById('<%=ucClockTime_WednesdayOutTime.FindControl("DropDownListTime").ClientID %>');

            var ThursdayInTime = document.getElementById('<%=ucClockTime_ThursdayInTime.FindControl("DropDownListTime").ClientID %>');
            var ThursdayOutTime = document.getElementById('<%=ucClockTime_ThursdayOutTime.FindControl("DropDownListTime").ClientID %>');

            var FridayInTime = document.getElementById('<%=ucClockTime_FridayInTime.FindControl("DropDownListTime").ClientID %>');
            var FridayOutTime = document.getElementById('<%=ucClockTime_FridayOutTime.FindControl("DropDownListTime").ClientID %>');

            var SaturdayInTime = document.getElementById('<%=ucClockTime_SaturdayInTime.FindControl("DropDownListTime").ClientID %>');
            var SaturdayOutTime = document.getElementById('<%=ucClockTime_SaturdayOutTime.FindControl("DropDownListTime").ClientID %>');

            var SundayTimeDiff = GetTimeDifference(SundayInTime.options[SundayInTime.selectedIndex].value, SundayOutTime.options[SundayOutTime.selectedIndex].value);
            jQuery('#<%=TextBoxSundayHourMinute.ClientID%>').val(SundayTimeDiff);

            var MondayTimeDiff = GetTimeDifference(MondayInTime.options[MondayInTime.selectedIndex].value, MondayOutTime.options[MondayOutTime.selectedIndex].value);
            jQuery('#<%=TextBoxMondayHourMinute.ClientID%>').val(MondayTimeDiff);

            var TuesdayTimeDiff = GetTimeDifference(TuesdayInTime.options[TuesdayInTime.selectedIndex].value, TuesdayOutTime.options[TuesdayOutTime.selectedIndex].value);
            jQuery('#<%=TextBoxTuesdayHourMinute.ClientID%>').val(TuesdayTimeDiff);

            var WednesdayTimeDiff = GetTimeDifference(WednesdayInTime.options[WednesdayInTime.selectedIndex].value, WednesdayOutTime.options[WednesdayOutTime.selectedIndex].value);
            jQuery('#<%=TextBoxWednesdayHourMinute.ClientID%>').val(WednesdayTimeDiff);

            var ThursdayTimeDiff = GetTimeDifference(ThursdayInTime.options[ThursdayInTime.selectedIndex].value, ThursdayOutTime.options[ThursdayOutTime.selectedIndex].value);
            jQuery('#<%=TextBoxThursdayHourMinute.ClientID%>').val(ThursdayTimeDiff);

            var FridayTimeDiff = GetTimeDifference(FridayInTime.options[FridayInTime.selectedIndex].value, FridayOutTime.options[FridayOutTime.selectedIndex].value);
            jQuery('#<%=TextBoxFridayHourMinute.ClientID%>').val(FridayTimeDiff);

            var SaturdayTimeDiff = GetTimeDifference(SaturdayInTime.options[SaturdayInTime.selectedIndex].value, SaturdayOutTime.options[SaturdayOutTime.selectedIndex].value);
            jQuery('#<%=TextBoxSaturdayHourMinute.ClientID%>').val(SaturdayTimeDiff);

            jQuery('#<%=TextBoxTotalHourMinute.ClientID%>').val(CalculateTotalHourMinute(SundayTimeDiff, MondayTimeDiff, TuesdayTimeDiff, WednesdayTimeDiff, ThursdayTimeDiff,
            FridayTimeDiff, SaturdayTimeDiff));
        }

    </script>
