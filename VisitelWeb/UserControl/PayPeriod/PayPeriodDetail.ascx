<%@ Control Language="vb" CodeBehind="PayPeriodDetail.ascx.vb" Inherits="VisitelWeb.Visitel.UserControl.PayPeriod.PayPeriodDetailControl" %>
<div id="Div10" class="SectionDiv" runat="server">
    <div id="Div8" class="SectionDiv-header">
        <asp:Label ID="LabelPayPeriodDetailInfo" runat="server" Text="Pay Period Detail"></asp:Label>
        <uc1:EditSetting ID="UserControlEditSetting" runat="server" />
    </div>
</div>
<div class="newRow">
    <asp:UpdatePanel ID="UpdatePanelPayPeriodDetailFilter" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div>
                <asp:Button ID="ButtonViewError" runat="server" Text="View Detail Error" />
            </div>
            <div class="AutoColumn">
                <asp:Label ID="LabelSearchByIndividual" runat="server" CssClass="LabelSearchByIndividual"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:DropDownList ID="DropDownListSearchByIndividual" runat="server" CssClass="DropDownListSearchByIndividual">
                </asp:DropDownList>
            </div>
            <div class="SecondColumn">
                <asp:Button ID="ButtonAll" runat="server" CssClass="ButtonAll" />
            </div>
            <div class="SecondColumn">
                <asp:Label ID="LabelCalendarIdCaption" runat="server" CssClass="LabelCalendarIdCaption"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:DropDownList ID="DropDownListCalendarId" runat="server" CssClass="DropDownListCalendarId">
                </asp:DropDownList>
                <asp:SqlDataSource ID="SqlDataSourceCalendarId" runat="server"></asp:SqlDataSource>
            </div>
            <div class="SecondColumn">
                <asp:Label ID="LabelServiceDateCaption" runat="server" CssClass="LabelServiceDateCaption"></asp:Label>
            </div>
            <div class="SecondColumn">
                <asp:DropDownList ID="DropDownListServiceDate" runat="server" CssClass="DropDownListServiceDate">
                </asp:DropDownList>
                <asp:SqlDataSource ID="SqlDataSourceServiceDate" runat="server"></asp:SqlDataSource>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="DropDownListSearchByIndividual" />
            <asp:AsyncPostBackTrigger ControlID="ButtonAll" />
            <asp:AsyncPostBackTrigger ControlID="DropDownListCalendarId" />
            <asp:AsyncPostBackTrigger ControlID="DropDownListServiceDate" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<div class="newRow HorizontalScroll">
    <asp:UpdatePanel ID="UpdatePanelPayPeriodDetail" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:HiddenField ID="HiddenFieldClientId" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HiddenFieldEmployeeId" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HiddenFieldStartDate" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HiddenFieldEndDate" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="HiddenFieldScheduleId" runat="server" ClientIDMode="Static" />
            <asp:GridView ID="GridViewPayPeriodDetail" runat="server" Width="100%">
                <Columns>
                    <asp:TemplateField HeaderText="Sl No">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="LabelSerial" Text='<%#Container.DataItemIndex + 1 %>' />
                        </ItemTemplate>
                        <ItemStyle Width="40px" HorizontalAlign="Right" />
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <div class="DivCheckBoxSelect">
                                <asp:Label runat="server" ID="LabelSelect" Text="Select" />
                                <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="true" OnCheckedChanged="OnCheckedChanged"
                                    ClientIDMode="Static" />
                            </div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="DivCheckBoxSelect">
                                <asp:CheckBox ID="CheckBoxSelect" runat="server" AutoPostBack="true" OnCheckedChanged="OnCheckedChanged"
                                    ClientIDMode="Static" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderStartDate" runat="server" CssClass="LabelHeaderStartDate"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxStartDate" runat="server" CssClass="TextBoxStartDate" ReadOnly="true"
                                Text='<%#Bind("StartDate") %>'></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderEndDate" runat="server" CssClass="LabelHeaderEndDate"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxEndDate" runat="server" CssClass="TextBoxEndDate" ReadOnly="true"
                                Text='<%#Bind("EndDate") %>'></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderCalendarId" runat="server" CssClass="LabelHeaderCalendarId"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxCalendarId" runat="server" CssClass="TextBoxCalendarId" ReadOnly="true"
                                Text='<%#Bind("CalendarId") %>'></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="IndividualName">
                        <HeaderTemplate>
                            <asp:Button ID="ButtonHeaderIndividualName" runat="server" CommandName="Sort" CommandArgument="IndividualName"
                                CssClass="ButtonHeaderIndividualName" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxIndividual" runat="server" CssClass="TextBoxIndividual" ReadOnly="true"
                                Text='<%#Bind("IndividualName") %>'></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="AttendantName">
                        <HeaderTemplate>
                            <asp:Button ID="ButtonHeaderAttendantName" runat="server" CommandName="Sort" CommandArgument="AttendantName"
                                CssClass="ButtonHeaderAttendantName" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxAttendanceOrOfficeStaff" runat="server" CssClass="TextBoxAttendanceOrOfficeStaff"
                                ReadOnly="true" Text='<%#Bind("AttendantName") %>'></asp:TextBox>
                            <asp:DropDownList ID="DropDownListAttendanceOrOfficeStaff" runat="server" CssClass="DropDownListAttendanceOrOfficeStaff">
                            </asp:DropDownList>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderServiceDate" runat="server" CssClass="LabelHeaderServiceDate"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxServiceDate" runat="server" CssClass="TextBoxServiceDate"
                                ReadOnly="true" Text='<%#Bind("ServiceDate") %>'></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderDayName" runat="server" CssClass="LabelHeaderDayName"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxDayName" runat="server" CssClass="TextBoxDayName" ReadOnly="true"
                                Text='<%#Bind("DayName") %>'></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderHourMinutes" runat="server" CssClass="LabelHeaderHourMinutes"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxHourMinute" runat="server" CssClass="TextBoxHourMinute" ReadOnly="true"
                                Text='<%#Bind("HourMinutes") %>'></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorHourMinute" runat="server"></asp:RegularExpressionValidator>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderInTime" runat="server" CssClass="LabelHeaderInTime"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxInTime" runat="server" CssClass="TextBoxInTime" ReadOnly="true"
                                Text='<%#Bind("InTime") %>'></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorInTime" runat="server"></asp:RegularExpressionValidator>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderOutTime" runat="server" CssClass="LabelHeaderOutTime"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxOutTime" runat="server" CssClass="TextBoxOutTime" ReadOnly="true"
                                Text='<%#Bind("OutTime") %>'></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorOutTime" runat="server"></asp:RegularExpressionValidator>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderSpecialRate" runat="server" CssClass="LabelHeaderSpecialRate"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxSpecialRate" runat="server" CssClass="TextBoxSpecialRate"
                                ReadOnly="true" Text='<%#Bind("SpecialRate") %>'></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidatorSpecialRate" runat="server"></asp:RegularExpressionValidator>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderComments" runat="server" CssClass="LabelHeaderComments"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxComments" runat="server" CssClass="TextBoxComments" ReadOnly="true"
                                Text='<%#Bind("Comments") %>'></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderUpdateDate" runat="server" CssClass="LabelHeaderUpdateDate"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxUpdateDate" runat="server" CssClass="TextBoxUpdateDate" ReadOnly="true"
                                Text='<%#Bind("UpdateDate") %>'></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderUpdateBy" runat="server" CssClass="LabelHeaderUpdateBy"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxUpdateBy" runat="server" CssClass="TextBoxUpdateBy" ReadOnly="true"
                                Text='<%#Bind("UpdateBy") %>'></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="LabelIndividualId" runat="server" Text='<%#Bind("IndividualId") %>'></asp:Label>
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
                            <asp:Label ID="LabelPayPeriodId" runat="server" Text='<%#Bind("PayPeriodId") %>'></asp:Label>
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
            <asp:AsyncPostBackTrigger ControlID="GridViewPayPeriodDetail" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<div class="DivSpace10">
</div>
<div class="newRow DivButtonCenter">
    <asp:UpdatePanel ID="UpdatePanelActionButtons" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Button ID="ButtonClear" runat="server" CssClass="ButtonClear" />
            <asp:Button ID="ButtonSave" runat="server" CssClass="ButtonSave" />
            <asp:Button ID="ButtonPopulateCareSummary" runat="server" CssClass="ButtonPopulateCareSummary"
                ClientIDMode="Static" />
            <asp:Button ID="ButtonIndividualDetail" runat="server" CssClass="ButtonIndividualDetail" />
            <asp:Button ID="ButtonEmployeeDetail" runat="server" CssClass="ButtonEmployeeDetail" />
            <asp:Label ID="LabelTotalTime" runat="server" CssClass="LabelTotalTime"></asp:Label>
            <asp:TextBox ID="TextBoxTotalTime" runat="server" CssClass="TextBoxTotalTime"></asp:TextBox>
            <asp:CheckBox ID="CheckBoxSavePrompt" runat="server" CssClass="CheckBoxSavePrompt" />
            <asp:Button ID="ButtonHospitalizedIndividuals" runat="server" CssClass="ButtonHospitalizedIndividuals"
                ClientIDMode="Static" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ButtonClear" />
            <asp:AsyncPostBackTrigger ControlID="ButtonSave" />
            <asp:AsyncPostBackTrigger ControlID="ButtonPopulateCareSummary" />
            <asp:AsyncPostBackTrigger ControlID="ButtonIndividualDetail" />
            <asp:AsyncPostBackTrigger ControlID="ButtonEmployeeDetail" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<div class="newRow">
</div>
