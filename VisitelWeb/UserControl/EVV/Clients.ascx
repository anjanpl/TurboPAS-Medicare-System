<%@ Control Language="vb" CodeBehind="~/UserControl/EVV/Clients.ascx.vb" Inherits="VisitelWeb.Visitel.UserControl.EVV.ClientsControl" %>
<div id="Div10" class="SectionDiv NoBorder" runat="server">
    <div id="Div8" class="SectionDiv-header">
        <asp:Label ID="LabelClients" runat="server"></asp:Label>
        <uc1:EditSetting ID="UserControlEditSetting" runat="server" />
    </div>
</div>
<div class="newRow">
    <asp:Button ID="ButtonViewError" runat="server" />
</div>
<div class="newRow HorizontalScroll">
    <asp:UpdatePanel ID="UpdatePanelGridViewClients" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:HiddenField ID="HiddenFieldClientId" runat="server" />
            <asp:GridView ID="GridViewClients" runat="server" Width="100%">
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
                            <asp:Label ID="LabelHeaderMyUniqueId" runat="server" CssClass="LabelHeaderMyUniqueId"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxMyUniqueId" runat="server" CssClass="TextBoxMyUniqueId" Text='<%#Bind("MyUniqueId") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxMyUniqueId" runat="server" CssClass="TextBoxMyUniqueId">
                            </asp:TextBox>
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
                            <asp:TextBox ID="TextBoxClientIdVesta" runat="server" CssClass="TextBoxClientIdVesta">
                            </asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderEVVId" runat="server" CssClass="LabelHeaderEVVId"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxEVVId" runat="server" CssClass="TextBoxEVVId" Text='<%#Bind("EVVId") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxEVVId" runat="server" CssClass="TextBoxEVVId">
                            </asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderLastName" runat="server" CssClass="LabelHeaderLastName"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxLastName" runat="server" CssClass="TextBoxLastName" Text='<%#Bind("LastName") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxLastName" runat="server" CssClass="TextBoxLastName">
                            </asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderFirstName" runat="server" CssClass="LabelHeaderFirstName"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxFirstName" runat="server" CssClass="TextBoxFirstName" Text='<%#Bind("FirstName") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxFirstName" runat="server" CssClass="TextBoxFirstName">
                            </asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderMedicaid" runat="server" CssClass="LabelHeaderMedicaid"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxMedicaid" runat="server" CssClass="TextBoxMedicaid" Text='<%#Bind("Medicaid") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxMedicaid" runat="server" CssClass="TextBoxMedicaid">
                            </asp:TextBox>
                        </FooterTemplate>
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
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxDateOfBirth" runat="server" CssClass="TextBoxDateOfBirth">
                            </asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderGender" runat="server" CssClass="LabelHeaderGender"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxGender" runat="server" CssClass="TextBoxGender" Text='<%#Bind("Gender") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxGender" runat="server" CssClass="TextBoxGender">
                            </asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderPhone" runat="server" CssClass="LabelHeaderPhone"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxPhone" runat="server" CssClass="TextBoxPhone" Text='<%#Bind("Phone") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxPhone" runat="server" CssClass="TextBoxPhone">
                            </asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderPhoneTwo" runat="server" CssClass="LabelHeaderPhoneTwo"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxPhoneTwo" runat="server" CssClass="TextBoxPhoneTwo" Text='<%#Bind("PhoneTwo") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxPhoneTwo" runat="server" CssClass="TextBoxPhoneTwo">
                            </asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderPhoneThree" runat="server" CssClass="LabelHeaderPhoneThree"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxPhoneThree" runat="server" CssClass="TextBoxPhoneThree" Text='<%#Bind("PhoneThree") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxPhoneThree" runat="server" CssClass="TextBoxPhoneThree">
                            </asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderAddressOne" runat="server" CssClass="LabelHeaderAddressOne"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxAddressOne" runat="server" CssClass="TextBoxAddressOne" Text='<%#Bind("AddressOne") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxAddressOne" runat="server" CssClass="TextBoxAddressOne">
                            </asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderAddressTwo" runat="server" CssClass="LabelHeaderAddressTwo"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxAddressTwo" runat="server" CssClass="TextBoxAddressTwo" Text='<%#Bind("AddressTwo") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxAddressTwo" runat="server" CssClass="TextBoxAddressTwo">
                            </asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderCity" runat="server" CssClass="LabelHeaderCity"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxCity" runat="server" CssClass="TextBoxCity" Text='<%#Bind("City") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxCity" runat="server" CssClass="TextBoxCity">
                            </asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderState" runat="server" CssClass="LabelHeaderState"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxState" runat="server" CssClass="TextBoxState" Text='<%#Bind("State") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxState" runat="server" CssClass="TextBoxState">
                            </asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderZip" runat="server" CssClass="LabelHeaderZip"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxZip" runat="server" CssClass="TextBoxZip" Text='<%#Bind("Zip") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxZip" runat="server" CssClass="TextBoxZip">
                            </asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderPriority" runat="server" CssClass="LabelHeaderPriority"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxPriority" runat="server" CssClass="TextBoxPriority" Text='<%#Bind("Priority") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxPriority" runat="server" CssClass="TextBoxPriority">
                            </asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderClientDeviceType" runat="server" CssClass="LabelHeaderClientDeviceType"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxClientDeviceType" runat="server" CssClass="TextBoxClientDeviceType"
                                Text='<%#Bind("ClientDeviceType") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxClientDeviceType" runat="server" CssClass="TextBoxClientDeviceType">
                            </asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderMedicaidStartDate" runat="server" CssClass="LabelHeaderMedicaidStartDate"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxMedicaidStartDate" runat="server" CssClass="TextBoxMedicaidStartDate"
                                Text='<%#Bind("MedicaidStartDate") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxMedicaidStartDate" runat="server" CssClass="TextBoxMedicaidStartDate">
                            </asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderMedicaidEndDate" runat="server" CssClass="LabelHeaderMedicaidEndDate"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxMedicaidEndDate" runat="server" CssClass="TextBoxMedicaidEndDate"
                                Text='<%#Bind("MedicaidEndDate") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxMedicaidEndDate" runat="server" CssClass="TextBoxMedicaidEndDate">
                            </asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderDADSIndividualRegion" runat="server" CssClass="LabelHeaderDADSIndividualRegion"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxDADSIndividualRegion" runat="server" CssClass="TextBoxDADSIndividualRegion"
                                Text='<%#Bind("DADSIndividualRegion") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxDADSIndividualRegion" runat="server" CssClass="TextBoxDADSIndividualRegion">
                            </asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderIndvMbrProgram" runat="server" CssClass="LabelHeaderIndvMbrProgram"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxIndvMbrProgram" runat="server" CssClass="TextBoxIndvMbrProgram"
                                Text='<%#Bind("IndvMbrProgram") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxIndvMbrProgram" runat="server" CssClass="TextBoxIndvMbrProgram">
                            </asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderMCOMbrSDA" runat="server" CssClass="LabelHeaderMCOMbrSDA"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxMCOMbrSDA" runat="server" CssClass="TextBoxMCOMbrSDA" Text='<%#Bind("MCOMbrSDA") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxMCOMbrSDA" runat="server" CssClass="TextBoxMCOMbrSDA">
                            </asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderMbrEnrollStartDate" runat="server" CssClass="LabelHeaderMbrEnrollStartDate"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxMbrEnrollStartDate" runat="server" CssClass="TextBoxMbrEnrollStartDate"
                                Text='<%#Bind("MbrEnrollStartDate") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxMbrEnrollStartDate" runat="server" CssClass="TextBoxMbrEnrollStartDate"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderMbrEnrollEndDate" runat="server" CssClass="LabelHeaderMbrEnrollEndDate"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxMbrEnrollEndDate" runat="server" CssClass="TextBoxMbrEnrollEndDate"
                                Text='<%#Bind("MbrEnrollEndDate") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxMbrEnrollEndDate" runat="server" CssClass="TextBoxMbrEnrollEndDate"></asp:TextBox>
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
                            <asp:Label ID="LabelHeaderClientId" runat="server" CssClass="LabelHeaderClientId"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxClientId" runat="server" CssClass="TextBoxClientId" Text='<%#Bind("ClientId") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxClientId" runat="server" CssClass="TextBoxClientId"></asp:TextBox>
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
                </Columns>
                <EmptyDataTemplate>
                    No Data Found
                </EmptyDataTemplate>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="GridViewClients" />
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
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ButtonClear" />
            <asp:AsyncPostBackTrigger ControlID="ButtonSave" />
            <asp:AsyncPostBackTrigger ControlID="ButtonDelete" />
            <asp:AsyncPostBackTrigger ControlID="ButtonIndividualDetail" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<div class="DivSpace10">
</div>
