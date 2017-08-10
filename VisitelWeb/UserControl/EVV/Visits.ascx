<%@ Control Language="vb" CodeBehind="~/UserControl/EVV/Visits.ascx.vb" Inherits="VisitelWeb.Visitel.UserControl.EVV.VisitsControl" %>
<div id="Div10" class="SectionDiv NoBorder" runat="server">
    <div id="Div8" class="SectionDiv-header">
        <asp:Label ID="LabelVisits" runat="server"></asp:Label>
        <uc1:EditSetting ID="UserControlEditSettingVisits" runat="server" />
    </div>
</div>
<div class="newRow">
    <asp:Button ID="ButtonViewError" runat="server" />
</div>
<div class="newRow HorizontalScroll">
    <asp:UpdatePanel ID="UpdatePanelGridViewVisits" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:HiddenField ID="HiddenFieldClientId" runat="server" />
            <asp:HiddenField ID="HiddenFieldEmployeeId" runat="server" />
            <asp:GridView ID="GridViewVisits" runat="server" Width="100%">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <div class="DivSerial">
                                <asp:Label runat="server" ID="LabelHeaderSerial" />
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
                                <asp:Label runat="server" ID="LabelHeaderSelect" />
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
                            <asp:Label ID="LabelHeaderClient" runat="server" CssClass="LabelHeaderClient"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxClientName" runat="server" CssClass="TextBoxClientName" Text='<%#Bind("ClientName") %>'></asp:TextBox>
                            <asp:DropDownList ID="DropDownListClient" runat="server" CssClass="DropDownListClient">
                            </asp:DropDownList>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="DropDownListClient" runat="server" CssClass="DropDownListClient">
                            </asp:DropDownList>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                      <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderEmployee" runat="server" CssClass="LabelHeaderEmployee"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxEmployeeName" runat="server" CssClass="TextBoxEmployeeName" Text='<%#Bind("EmployeeName") %>'></asp:TextBox>
                            <asp:DropDownList ID="DropDownListEmployee" runat="server" CssClass="DropDownListEmployee">
                            </asp:DropDownList>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="DropDownListEmployee" runat="server" CssClass="DropDownListEmployee">
                            </asp:DropDownList>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderMyUniqueId" runat="server" CssClass="LabelHeaderMyUniqueId"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxMyUniqueId" runat="server" CssClass="TextBoxMyUniqueId" Text='<%#Bind("MyUniqueId") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxMyUniqueId" runat="server" CssClass="TextBoxMyUniqueId"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderVestaVisitId" runat="server" CssClass="LabelHeaderVestaVisitId"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxVestaVisitId" runat="server" CssClass="TextBoxVestaVisitId"
                                Text='<%#Bind("VestaVisitId") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxVestaVisitId" runat="server" CssClass="TextBoxVestaVisitId"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderClientIdVesta" runat="server" CssClass="LabelHeaderClientIdVesta"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxClientIdVesta" runat="server" CssClass="TextBoxClientIdVesta"
                                Text='<%#Bind("ClientIdVesta") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxClientIdVesta" runat="server" CssClass="TextBoxClientIdVesta"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderEmployeeIdVesta" runat="server" CssClass="LabelHeaderEmployeeIdVesta"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxEmployeeIdVesta" runat="server" CssClass="TextBoxEmployeeIdVesta"
                                Text='<%#Bind("EmployeeIdVesta") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxEmployeeIdVesta" runat="server" CssClass="TextBoxEmployeeIdVesta"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderAuthIdVesta" runat="server" CssClass="LabelHeaderAuthIdVesta"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxAuthIdVesta" runat="server" CssClass="TextBoxAuthIdVesta"
                                Text='<%#Bind("AuthIdVesta") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxAuthIdVesta" runat="server" CssClass="TextBoxAuthIdVesta"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderVisitDate" runat="server" CssClass="LabelHeaderVisitDate"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxVisitDate" runat="server" CssClass="TextBoxVisitDate" Text='<%#Bind("VisitDate") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxVisitDate" runat="server" CssClass="TextBoxVisitDate"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderScheduledTimeIn" runat="server" CssClass="LabelHeaderScheduledTimeIn"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxScheduledTimeIn" runat="server" CssClass="TextBoxScheduledTimeIn"
                                Text='<%#Bind("ScheduledTimeIn") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxScheduledTimeIn" runat="server" CssClass="TextBoxScheduledTimeIn"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderScheduledTimeOut" runat="server" CssClass="LabelHeaderScheduledTimeOut"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxScheduledTimeOut" runat="server" CssClass="TextBoxScheduledTimeOut"
                                Text='<%#Bind("ScheduledTimeOut") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxScheduledTimeOut" runat="server" CssClass="TextBoxScheduledTimeOut"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderVisitUnits" runat="server" CssClass="LabelHeaderVisitUnits"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxVisitUnits" runat="server" CssClass="TextBoxVisitUnits" Text='<%#Bind("VisitUnits") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxVisitUnits" runat="server" CssClass="TextBoxVisitUnits"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderVisitLocation" runat="server" CssClass="LabelHeaderVisitLocation"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxVisitLocation" runat="server" CssClass="TextBoxVisitLocation"
                                Text='<%#Bind("VisitLocation") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxVisitLocation" runat="server" CssClass="TextBoxVisitLocation"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderDADSServiceGroup" runat="server" CssClass="LabelHeaderDADSServiceGroup"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxDADSServiceGroup" runat="server" CssClass="TextBoxDADSServiceGroup"
                                Text='<%#Bind("DADSServiceGroup") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxDADSServiceGroup" runat="server" CssClass="TextBoxDADSServiceGroup"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderHCPCSBillCode" runat="server" CssClass="LabelHeaderHCPCSBillCode"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxHCPCSBillCode" runat="server" CssClass="TextBoxHCPCSBillCode"
                                Text='<%#Bind("HCPCSBillCode") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxHCPCSBillCode" runat="server" CssClass="TextBoxHCPCSBillCode"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderDADSServiceCode" runat="server" CssClass="LabelHeaderDADSServiceCode"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxDADSServiceCode" runat="server" CssClass="TextBoxDADSServiceCode"
                                Text='<%#Bind("DADSServiceCode") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxDADSServiceCode" runat="server" CssClass="TextBoxDADSServiceCode"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderMod1" runat="server" CssClass="LabelHeaderMod1"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxMod1" runat="server" CssClass="TextBoxMod1" Text='<%#Bind("Mod1") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxMod1" runat="server" CssClass="TextBoxMod1"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderMod2" runat="server" CssClass="LabelHeaderMod2"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxMod2" runat="server" CssClass="TextBoxMod2" Text='<%#Bind("Mod2") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxMod2" runat="server" CssClass="TextBoxMod2"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderMod3" runat="server" CssClass="LabelHeaderMod3"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxMod3" runat="server" CssClass="TextBoxMod3" Text='<%#Bind("Mod3") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxMod3" runat="server" CssClass="TextBoxMod3"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderMod4" runat="server" CssClass="LabelHeaderMod4"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxMod4" runat="server" CssClass="TextBoxMod4" Text='<%#Bind("Mod4") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxMod4" runat="server" CssClass="TextBoxMod4"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderError" runat="server" CssClass="LabelHeaderError"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxError" runat="server" CssClass="TextBoxError" Text='<%#Bind("Error") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxError" runat="server" CssClass="TextBoxError"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderContract" runat="server" CssClass="LabelHeaderContract"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxContract" runat="server" CssClass="TextBoxContract" Text='<%#Bind("Contract") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxContract" runat="server" CssClass="TextBoxContract"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderCalendarId" runat="server" CssClass="LabelHeaderCalendarId"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxCalendarId" runat="server" CssClass="TextBoxCalendarId" Text='<%#Bind("CalendarId") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxCalendarId" runat="server" CssClass="TextBoxCalendarId"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderUpdateDate" runat="server" Text="Update Date" CssClass="LabelHeaderUpdateDate"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxUpdateDate" runat="server" CssClass="TextBoxUpdateDate" Text='<%#Bind("UpdateDate") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxUpdateDate" runat="server" CssClass="TextBoxUpdateDate"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderUpdateBy" runat="server" Text="Update By" CssClass="LabelHeaderUpdateBy"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxUpdateBy" runat="server" CssClass="TextBoxUpdateBy" Text='<%#Bind("UpdateBy") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxUpdateBy" runat="server" CssClass="TextBoxUpdateBy"></asp:TextBox>
                        </FooterTemplate>
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
                            <asp:Label ID="LabelEmployeeId" runat="server" Text='<%#Bind("EmployeeId") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="LabelEmployeeId" runat="server"></asp:Label>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    No Data Found
                </EmptyDataTemplate>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="GridViewVisits" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<div class="newRow DivButtonCenter">
    <asp:UpdatePanel ID="UpdatePanelActionButtons" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Button ID="ButtonClear" runat="server" CssClass="ButtonClear" />
            <asp:Button ID="ButtonSave" runat="server" CssClass="ButtonSave" />
            <asp:Button ID="ButtonDelete" runat="server" CssClass="ButtonDelete" />
            <asp:Button ID="ButtonIndividualDetail" runat="server" CssClass="ButtonIndividualDetail" />
            <asp:Button ID="ButtonEmployeeDetail" runat="server" CssClass="ButtonEmployeeDetail" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ButtonClear" />
            <asp:AsyncPostBackTrigger ControlID="ButtonSave" />
            <asp:AsyncPostBackTrigger ControlID="ButtonDelete" />
            <asp:AsyncPostBackTrigger ControlID="ButtonIndividualDetail" />
            <asp:AsyncPostBackTrigger ControlID="ButtonEmployeeDetail" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<div class="DivSpace10">
</div>
