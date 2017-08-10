<%@ Control Language="vb" CodeBehind="MEDsysClients.ascx.vb" Inherits="VisitelWeb.Visitel.UserControl.EVV.MEDsysClientsControl" %>
<div id="Div10" class="SectionDiv NoBorder" runat="server">
    <div id="Div8" class="SectionDiv-header">
        <asp:Label ID="LabelMEDsysClients" runat="server"></asp:Label>
        <uc1:EditSetting ID="UserControlEditSetting" runat="server" />
    </div>
</div>
<div class="newRow HorizontalScroll">
    <asp:UpdatePanel ID="UpdatePanelGridViewMEDsysClients" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="newRow">
                <asp:Button ID="ButtonViewError" runat="server" />
            </div>
            <asp:HiddenField ID="HiddenFieldClientId" runat="server" />
            <asp:GridView ID="GridViewMEDsysClients" runat="server" Width="100%">
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
                            <asp:Label ID="LabelHeaderAccountId" runat="server" CssClass="LabelHeaderAccountId"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxAccountId" runat="server" CssClass="TextBoxAccountId" Text='<%#Bind("AccountId") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxAccountId" runat="server" CssClass="TextBoxAccountId"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderExternalId" runat="server" CssClass="LabelHeaderExternalId"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxExternalId" runat="server" CssClass="TextBoxExternalId" Text='<%#Bind("ExternalId") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxExternalId" runat="server" CssClass="TextBoxExternalId"></asp:TextBox>
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
                            <asp:Label ID="LabelHeaderClientNumber" runat="server" CssClass="LabelHeaderClientNumber"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxClientNumber" runat="server" CssClass="TextBoxClientNumber"
                                Text='<%#Bind("ClientNumber") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxClientNumber" runat="server" CssClass="TextBoxClientNumber"></asp:TextBox>
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
                            <asp:TextBox ID="TextBoxLastName" runat="server" CssClass="TextBoxLastName"></asp:TextBox>
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
                            <asp:TextBox ID="TextBoxFirstName" runat="server" CssClass="TextBoxFirstName"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderMiddleInitial" runat="server" CssClass="LabelHeaderMiddleInitial"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxMiddleInitial" runat="server" CssClass="TextBoxMiddleInitial"
                                Text='<%#Bind("MiddleInit") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxMiddleInitial" runat="server" CssClass="TextBoxMiddleInitial"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderDateOfBirth" runat="server" CssClass="LabelHeaderDateOfBirth"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxDateOfBirth" runat="server" CssClass="TextBoxDateOfBirth"
                                Text='<%#Bind("Birthdate") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxDateOfBirth" runat="server" CssClass="TextBoxDateOfBirth"></asp:TextBox>
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
                            <asp:TextBox ID="TextBoxGender" runat="server" CssClass="TextBoxGender"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderGIN" runat="server" CssClass="LabelHeaderGIN"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxGIN" runat="server" CssClass="TextBoxGIN" Text='<%#Bind("GIN") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxGIN" runat="server" CssClass="TextBoxGIN"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderAddress" runat="server" CssClass="LabelHeaderAddress"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxAddress" runat="server" CssClass="TextBoxAddress" Text='<%#Bind("Address") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxAddress" runat="server" CssClass="TextBoxAddress"></asp:TextBox>
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
                            <asp:TextBox ID="TextBoxCity" runat="server" CssClass="TextBoxCity"></asp:TextBox>
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
                            <asp:TextBox ID="TextBoxState" runat="server" CssClass="TextBoxState"></asp:TextBox>
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
                            <asp:TextBox ID="TextBoxZip" runat="server" CssClass="TextBoxZip"></asp:TextBox>
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
                            <asp:TextBox ID="TextBoxPhone" runat="server" CssClass="TextBoxPhone"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderNotes" runat="server" CssClass="LabelHeaderNotes"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxNotes" runat="server" CssClass="TextBoxNotes" Text='<%#Bind("Notes") %>'
                                TextMode="MultiLine"></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxNotes" runat="server" CssClass="TextBoxNotes" TextMode="MultiLine"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderProgramCode" runat="server" CssClass="LabelHeaderProgramCode"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxProgramCode" runat="server" CssClass="TextBoxProgramCode"
                                Text='<%#Bind("ProgramCode") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxProgramCode" runat="server" CssClass="TextBoxProgramCode"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderRegion" runat="server" CssClass="LabelHeaderRegion"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxRegion" runat="server" CssClass="TextBoxRegion" Text='<%#Bind("Region") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxRegion" runat="server" CssClass="TextBoxRegion"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderStatus" runat="server" CssClass="LabelHeaderStatus"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxStatus" runat="server" CssClass="TextBoxStatus" Text='<%#Bind("Status") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxStatus" runat="server" CssClass="TextBoxStatus"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderStartDate" runat="server" CssClass="LabelHeaderStartDate"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxStartDate" runat="server" CssClass="TextBoxStartDate" Text='<%#Bind("StartDate") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxStartDate" runat="server" CssClass="TextBoxStartDate"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderEndDate" runat="server" CssClass="LabelHeaderEndDate"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxEndDate" runat="server" CssClass="TextBoxEndDate" Text='<%#Bind("EndDate") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxEndDate" runat="server" CssClass="TextBoxEndDate"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderPayor" runat="server" CssClass="LabelHeaderPayor"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxPayor" runat="server" CssClass="TextBoxPayor" Text='<%#Bind("Payor") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxPayor" runat="server" CssClass="TextBoxPayor"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderServiceGroup" runat="server" CssClass="LabelHeaderServiceGroup"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxServiceGroup" runat="server" CssClass="TextBoxServiceGroup"
                                Text='<%#Bind("ServiceGroup") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxServiceGroup" runat="server" CssClass="TextBoxServiceGroup"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderServiceCode" runat="server" CssClass="LabelHeaderServiceCode"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxServiceCode" runat="server" CssClass="TextBoxServiceCode"
                                Text='<%#Bind("PayorServiceCode") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxServiceCode" runat="server" CssClass="TextBoxServiceCode"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderContractNumber" runat="server" CssClass="LabelHeaderContractNumber"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxContractNumber" runat="server" CssClass="TextBoxContractNumber"
                                Text='<%#Bind("ContractNumber") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxContractNumber" runat="server" CssClass="TextBoxContractNumber"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderAction" runat="server" CssClass="LabelHeaderAction"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxAction" runat="server" CssClass="TextBoxAction" Text='<%#Bind("Action") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxAction" runat="server" CssClass="TextBoxAction"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderClient_Id" runat="server" CssClass="LabelHeaderClient_Id"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxClient_Id" runat="server" CssClass="TextBoxClient_Id" Text='<%#Bind("Client_Id") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxClient_Id" runat="server" CssClass="TextBoxClient_Id"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderCompanyCode" runat="server" CssClass="LabelHeaderCompanyCode"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxCompanyCode" runat="server" CssClass="TextBoxCompanyCode"
                                Text='<%#Bind("CompanyCode") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxCompanyCode" runat="server" CssClass="TextBoxCompanyCode"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderLocationCode" runat="server" CssClass="LabelHeaderLocationCode"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxLocationCode" runat="server" CssClass="TextBoxLocationCode"
                                Text='<%#Bind("LocationCode") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxLocationCode" runat="server" CssClass="TextBoxLocationCode"></asp:TextBox>
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
            <asp:AsyncPostBackTrigger ControlID="GridViewMEDsysClients" />
            <asp:PostBackTrigger ControlID="ButtonViewError" />
            <asp:AsyncPostBackTrigger ControlID="HiddenFieldClientId" />
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
