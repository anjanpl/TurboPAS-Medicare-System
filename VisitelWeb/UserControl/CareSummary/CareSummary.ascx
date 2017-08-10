<%@ Control Language="vb" CodeBehind="CareSummary.ascx.vb" Inherits="VisitelWeb.Visitel.UserControl.CareSummary.CareSummaryControl" %>
<div id="Div10" class="SectionDiv" runat="server">
    <div id="Div8" class="SectionDiv-header">
        <asp:Label ID="LabelCareSummaryInfo" runat="server"></asp:Label>
        <uc1:EditSetting ID="UserControlEditSetting" runat="server" />
    </div>
</div>
<asp:UpdatePanel ID="UpdatePanelCareSummaryFilter" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <div>
            <asp:Button ID="ButtonViewError" runat="server" />
        </div>
        <div class="newRow">
            <div class="AutoColumn">
                <asp:Label ID="LabelSearchByClient" runat="server" CssClass="LabelSearchByClient"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:DropDownList ID="DropDownListSearchByClient" runat="server" CssClass="DropDownListSearchByClient">
                </asp:DropDownList>
            </div>
            <div class="SecondColumn">
                <asp:Label ID="LabelPayPeriodCaption" runat="server" CssClass="LabelPayPeriodCaption"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:DropDownList ID="DropDownListPayPeriod" runat="server" CssClass="DropDownListPayPeriod">
                </asp:DropDownList>
                <asp:SqlDataSource ID="SqlDataSourcePayPeriod" runat="server"></asp:SqlDataSource>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelContract" runat="server" CssClass="LabelClientType"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:DropDownList ID="DropDownListSearchByContract" runat="server" CssClass="DropDownListSearchByContract">
                </asp:DropDownList>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelTimeSheet" runat="server" CssClass="LabelTimeSheet"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:ListBox ID="ListBoxTimeSheet" runat="server" CssClass="ListBoxTimeSheet"></asp:ListBox>
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelBilled" runat="server" CssClass="LabelBilled"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:ListBox ID="ListBoxBilled" runat="server" CssClass="ListBoxBilled"></asp:ListBox>
            </div>
        </div>
        <div class="newRow">
            <div class="DivButtonClearFilter">
                <asp:Button ID="ButtonClearFilter" runat="server" CssClass="ButtonClearFilter" />
            </div>
            <div class="DivButtonSearch">
                <asp:Button ID="ButtonSearch" runat="server" CssClass="ButtonSearch" />
            </div>
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="DropDownListSearchByClient" />
        <asp:AsyncPostBackTrigger ControlID="DropDownListPayPeriod" />
        <asp:AsyncPostBackTrigger ControlID="ButtonClearFilter" />
        <asp:AsyncPostBackTrigger ControlID="ButtonSearch" />
        <asp:PostBackTrigger ControlID="ButtonViewError" />
    </Triggers>
</asp:UpdatePanel>
<div class="newRow HorizontalScroll">
    <asp:UpdatePanel ID="UpdatePanelCareSummary" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:HiddenField ID="HiddenFieldClientId" runat="server" />
            <asp:HiddenField ID="HiddenFieldEmployeeId" runat="server" />
            <asp:HiddenField ID="HiddenFieldStartDate" runat="server" />
            <asp:HiddenField ID="HiddenFieldEndDate" runat="server" />
            <asp:HiddenField ID="HiddenFieldScheduleId" runat="server" />
            <asp:GridView ID="GridViewCareSummary" runat="server" Width="100%">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label runat="server" ID="LabelSerial" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label runat="server" ID="LabelSerial" Text='<%#Container.DataItemIndex + 1 %>' />
                        </ItemTemplate>
                        <ItemStyle Width="40px" HorizontalAlign="Right" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <div class="DivCheckBoxSelect">
                                <asp:Label runat="server" ID="LabelSelect" />
                                <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="true" OnCheckedChanged="OnCheckedChanged"
                                    ClientIDMode="Static" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="DivCheckBoxSelect">
                                <asp:CheckBox ID="CheckBoxSelect" runat="server" OnCheckedChanged="OnCheckedChanged" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <div class="DivAddNew">
                                <asp:Label runat="server" ID="LabelAddNew" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="DivAddNew">
                                <asp:Button ID="ButtonAddNew" runat="server" OnClick="ButtonAddNew_OnClick" CssClass="ButtonAddNew" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <div class="DivCheckBoxBilled">
                                <asp:Label runat="server" ID="LabelBilledAll" />
                                <asp:CheckBox ID="CheckBoxBilledAll" runat="server" CssClass="CheckBoxBilledAll" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="DivCheckBoxBilled">
                                <asp:CheckBox ID="CheckBoxBilled" runat="server" CssClass="CheckBoxBilled" />
                            </div>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Button ID="ButtonHeaderStartDate" runat="server" CssClass="ButtonHeaderStartDate" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxStartDate" runat="server" CssClass="TextBoxStartDate" Text='<%#Bind("StartDate") %>'></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Button ID="ButtonHeaderEndDate" runat="server" CssClass="ButtonHeaderEndDate" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxEndDate" runat="server" CssClass="TextBoxEndDate" Text='<%#Bind("EndDate") %>'></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Button ID="ButtonHeaderClientName" runat="server" CssClass="ButtonHeaderClientName" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxClient" runat="server" CssClass="TextBoxClient" Text='<%#Bind("ClientName") %>'></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelBillTime" runat="server" CssClass="LabelBillTime"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxBillTime" runat="server" CssClass="TextBoxBillTime" Text='<%#Bind("BillTime") %>'></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelAdjustedBillTime" runat="server" CssClass="LabelAdjustedBillTime"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxAdjustedBillTime" runat="server" CssClass="TextBoxAdjustedBillTime"
                                Text='<%#Bind("AdjustedBillTime") %>'></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <div class="DivCheckBoxOriginalTimeSheetAll">
                                <asp:Label runat="server" ID="LabelOriginalTimeSheetAll" />
                                <asp:CheckBox ID="CheckBoxOriginalTimeSheetAll" runat="server" CssClass="CheckBoxOriginalTimeSheetAll" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="DivOriginalTimeSheet">
                                <asp:CheckBox ID="CheckBoxOriginalTimeSheet" runat="server" CssClass="CheckBoxOriginalTimeSheet"
                                    Checked='<%# GetStatus(Eval("OriginalTimesheet")) %>' />
                            </div>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="center" Width="" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <div class="DivLabelTimesheet">
                                <asp:Label ID="LabelTimesheet" runat="server" CssClass="LabelTimesheet"></asp:Label>
                                <asp:Label ID="LabelTimesheetHrsMins" runat="server"></asp:Label>
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="DivTextBoxTimesheet">
                                <asp:TextBox ID="TextBoxTimesheet" runat="server" CssClass="TextBoxTimesheet" Text='<%#Bind("TimesheetHourMinute") %>'
                                    OnTextChanged="TextBoxTimesheet_TextChanged"></asp:TextBox>
                            </div>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="center" Width="" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="AttendantName">
                        <HeaderTemplate>
                            <asp:Button ID="ButtonHeaderAttendantName" runat="server" CssClass="ButtonHeaderAttendantName" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxAttendanceOrOfficeStaff" runat="server" CssClass="TextBoxAttendanceOrOfficeStaff"
                                Text='<%#Bind("AttendantName") %>'></asp:TextBox>
                            <asp:DropDownList ID="DropDownListAttendanceOrOfficeStaff" runat="server" CssClass="DropDownListAttendanceOrOfficeStaff">
                            </asp:DropDownList>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderCalendarId" runat="server" CssClass="LabelHeaderCalendarId"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Button ID="ButtonCalendar" runat="server" Text='<%#Bind("CalendarId") %>' CssClass="ButtonCalendar" />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelIndividualType" runat="server" CssClass="LabelIndividualType"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxIndividualType" runat="server" CssClass="TextBoxIndividualType"
                                Text='<%#Bind("ClientTypeName") %>'></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderUpdateDate" runat="server" CssClass="LabelHeaderUpdateDate"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxUpdateDate" runat="server" CssClass="TextBoxUpdateDate" Text='<%#Bind("UpdateDate") %>'></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderUpdateBy" runat="server" CssClass="LabelHeaderUpdateBy"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxUpdateBy" runat="server" CssClass="TextBoxUpdateBy" Text='<%#Bind("UpdateBy") %>'></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelEDIFile" runat="server" CssClass="LabelEDIFile"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxEDIFile" runat="server" CssClass="TextBoxEDIFile"></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderEDIUpdateDate" runat="server" CssClass="LabelHeaderEDIUpdateDate"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxEDIUpdateDate" runat="server" CssClass="TextBoxEDIUpdateDate"
                                Text='<%#Bind("EDIUpdateDate") %>'></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderEDIUpdateBy" runat="server" CssClass="LabelHeaderEDIUpdateBy"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxEDIUpdateBy" runat="server" CssClass="TextBoxEDIUpdateBy"
                                Text='<%#Bind("EDIUpdateBy") %>'></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="LabelClientId" runat="server" Text='<%#Bind("ClientId") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="LabelClientType" runat="server" Text='<%#Bind("ClientType") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="LabelAttendantId" runat="server" Text='<%#Bind("AttendantId") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="LabelCareSummaryId" runat="server" Text='<%#Bind("CareSummaryId") %>'></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <asp:Label ID="LabelNoDataFound" Text="No Data Found" runat="server"></asp:Label>
                </EmptyDataTemplate>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="GridViewCareSummary" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<div class="DivSpace10">
</div>
<div class="newRow DivButtonCenter">
   
  <asp:UpdatePanel ID="UpdatePanelActionButtons" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="AutoColumn">
                    <asp:Button ID="ButtonRefresh" runat="server" CssClass="ButtonRefresh" />
                </div>
                <div class="AutoColumn">
                    <asp:Button ID="ButtonSave" runat="server" CssClass="ButtonSave" />
                </div>
                <div class="AutoColumn">
                </div>
                <div class="AutoColumn">
                    <asp:Button ID="ButtonIndividualDetail" runat="server" CssClass="ButtonIndividualDetail" Visible="false" />
                </div>
                <div class="AutoColumn">
                    <asp:Button ID="ButtonEmployeeDetail" runat="server" CssClass="ButtonEmployeeDetail" Visible="false" />
                </div>
                <div class="AutoColumn">
                    <asp:Button ID="ButtonResetBatch" runat="server" CssClass="ButtonResetBatch" Visible="false" />
                </div>
                <div class="AutoColumn">
                    <asp:Button ID="ButtonAdjust" runat="server" CssClass="ButtonAdjust"/>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ButtonRefresh" />
                <asp:AsyncPostBackTrigger ControlID="ButtonSave" />
                <asp:AsyncPostBackTrigger ControlID="ButtonIndividualDetail" />
                <asp:AsyncPostBackTrigger ControlID="ButtonEmployeeDetail" />
                <asp:AsyncPostBackTrigger ControlID="ButtonResetBatch" />
            </Triggers>
        </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanelTotalAdjustedTime" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="AutoColumn">
                <asp:CheckBox ID="CheckBoxSavePrompt" runat="server" CssClass="CheckBoxSavePrompt" />
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelTotalAdjustedTime" runat="server" CssClass="LabelTotalAdjustedTime"></asp:Label>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxTotalAdjustedTime" runat="server" CssClass="TextBoxTotalAdjustedTime"></asp:TextBox>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<div class="newRow DivTotalActualTime">
    <asp:UpdatePanel ID="UpdatePanelTotalActualTime" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="AutoColumn">
                <asp:Label ID="LabelTotalActualTime" runat="server" CssClass="LabelTotalActualTime"></asp:Label>
            </div>
            <div class="AutoColumn">
                <asp:TextBox ID="TextBoxTotalAcutualTime" runat="server" CssClass="TextBoxTotalAcutualTime"></asp:TextBox>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<div class="newRow">
</div>
