<%@ Control Language="vb" CodeBehind="~/UserControl/Settings/ClientType.ascx.vb"
    Inherits="VisitelWeb.Visitel.UserControl.Settings.ClientTypeSetting" %>
<div>
    <div id="Div2" class="SectionDiv-header">
        <asp:Label ID="LabelSearchClientTypeInfo" runat="server"></asp:Label>
    </div>
    <div class="newRow">
    </div>
    <div class="newRow">
        <asp:UpdatePanel ID="UpdatePanelSearch" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="AutoColumn">
                    <asp:Label ID="LabelSearchByName" runat="server"></asp:Label>
                </div>
                <div class="AutoColumn">
                    <asp:TextBox ID="TextBoxSearchByName" runat="server"></asp:TextBox>
                </div>
                <div class="AutoColumn">
                    <asp:Label ID="LabelSearchByContractNumber" runat="server"></asp:Label>
                </div>
                <div class="AutoColumn">
                    <asp:TextBox ID="TextBoxSearchByContractNumber" runat="server"></asp:TextBox>
                </div>
                <div class="AutoColumn">
                    <asp:Button ID="ButtonSearch" runat="server" CssClass="ButtonSearch" />
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ButtonSearch" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="DivSpace10">
    </div>
    <div id="Div10" class="SectionDiv NoBorder" runat="server">
        <div id="Div8" class="SectionDiv-header">
            <asp:Label ID="LabelClientTypeInformationList" runat="server"></asp:Label>
        </div>
    </div>
    <div class="newRow HorizontalScroll">
        <asp:UpdatePanel ID="UpdatePanelGridViewMEDsysSchedules" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:HiddenField ID="HiddenFieldIdNumber" runat="server" />
                <asp:HiddenField ID="HiddenFieldIsNew" runat="server" />
                <asp:HiddenField ID="HiddenFieldIsSearched" runat="server" />
                <asp:HiddenField ID="HiddenFieldContractName" runat="server" />
                <asp:HiddenField ID="HiddenFieldContractNumber" runat="server" />
                <asp:HiddenField ID="HiddenFieldUnitRate" runat="server" />
                <asp:HiddenField ID="HiddenFieldIsDuplicate" runat="server" />
                <div class="newRow">
                    <asp:Button ID="ButtonViewError" runat="server" />
                </div>
                <asp:GridView ID="GridViewClientTypeInformation" runat="server" Width="100%">
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
                                <asp:Label ID="LabelHeaderName" runat="server" CssClass="LabelHeaderName"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="TextBoxName" runat="server" CssClass="TextBoxName" Text='<%#Bind("Name") %>'></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="TextBoxName" runat="server" CssClass="TextBoxName">
                                </asp:TextBox>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="LabelHeaderContractNo" runat="server" CssClass="LabelHeaderContractNo"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="TextBoxContractNumber" runat="server" CssClass="TextBoxContractNumber"
                                    Text='<%#Bind("ContractNo") %>'></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="TextBoxContractNumber" runat="server" CssClass="TextBoxContractNumber">
                                </asp:TextBox>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="LabelHeaderRegion" runat="server" CssClass="LabelHeaderRegion"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="TextBoxRegion" runat="server" CssClass="TextBoxRegion" Text='<%#Bind("Region") %>'></asp:TextBox>
                                <asp:DropDownList ID="DropDownListRegion" runat="server" CssClass="DropDownListRegion">
                                </asp:DropDownList>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="DropDownListRegion" runat="server" CssClass="DropDownListRegion">
                                </asp:DropDownList>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="LabelHeaderServiceType" runat="server" CssClass="LabelHeaderServiceType"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="TextBoxServiceType" runat="server" CssClass="TextBoxServiceType"
                                    Text='<%#Bind("ServiceType") %>'></asp:TextBox>
                                <asp:DropDownList ID="DropDownListServiceType" runat="server" CssClass="DropDownListServiceType">
                                </asp:DropDownList>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="DropDownListServiceType" runat="server" CssClass="DropDownListServiceType">
                                </asp:DropDownList>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="LabelHeaderProgramType" runat="server" CssClass="LabelHeaderProgramType"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="TextBoxProgramType" runat="server" CssClass="TextBoxProgramType"
                                    Text='<%#Bind("ProgramType") %>'></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="TextBoxProgramType" runat="server" CssClass="TextBoxProgramType"></asp:TextBox>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="LabelHeaderUnitRate" runat="server" CssClass="LabelHeaderUnitRate"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="DivUnitRate">
                                    $
                                    <asp:TextBox ID="TextBoxUnitRate" runat="server" CssClass="TextBoxUnitRate" Text='<%#Bind("UnitRate") %>'></asp:TextBox>
                                </div>
                            </ItemTemplate>
                            <FooterTemplate>
                                <div class="DivUnitRate">
                                    $
                                    <asp:TextBox ID="TextBoxUnitRate" runat="server" CssClass="TextBoxUnitRate">
                                    </asp:TextBox>
                                </div>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="LabelHeaderGroup" runat="server" CssClass="LabelHeaderGroup"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="TextBoxClientGroupName" runat="server" CssClass="TextBoxClientGroupName"
                                    Text='<%#Bind("ClientGroup") %>'></asp:TextBox>
                                <asp:DropDownList ID="DropDownListClientGroup" runat="server" CssClass="DropDownListClientGroup">
                                </asp:DropDownList>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="DropDownListClientGroup" runat="server" CssClass="DropDownListClientGroup">
                                </asp:DropDownList>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="LabelHeaderReceiverName" runat="server" CssClass="LabelHeaderReceiverName"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="TextBoxReceiverName" runat="server" CssClass="TextBoxReceiverName"
                                    Text='<%#Bind("ReceiverName") %>'></asp:TextBox>
                                <asp:DropDownList ID="DropDownListReceiver" runat="server" CssClass="DropDownListReceiver">
                                </asp:DropDownList>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="DropDownListReceiver" runat="server" CssClass="DropDownListReceiver">
                                </asp:DropDownList>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="LabelHeaderPayerName" runat="server" CssClass="LabelHeaderPayerName"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="TextBoxPayerName" runat="server" CssClass="TextBoxPayerName" Text='<%#Bind("Payer") %>'></asp:TextBox>
                                <asp:DropDownList ID="DropDownListPayer" runat="server" CssClass="DropDownListPayer">
                                </asp:DropDownList>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="DropDownListPayer" runat="server" CssClass="DropDownListPayer">
                                </asp:DropDownList>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="LabelHeaderStatus" runat="server" CssClass="LabelHeaderStatus"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="TextBoxStatus" runat="server" CssClass="TextBoxStatus" Text='<%#Bind("Status") %>'></asp:TextBox>
                                <asp:DropDownList ID="DropDownListStatus" runat="server" CssClass="DropDownListStatus">
                                </asp:DropDownList>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="DropDownListStatus" runat="server" CssClass="DropDownListStatus">
                                </asp:DropDownList>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <div class="DivCheckBoxSantrax">
                                    <asp:Label ID="LabelHeaderSantrax" runat="server" CssClass="LabelHeaderSantrax"></asp:Label>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="DivCheckBoxSantrax">
                                    <asp:CheckBox ID="CheckBoxSantrax" runat="server" CssClass="CheckBoxSantrax" />
                                </div>
                            </ItemTemplate>
                            <FooterTemplate>
                                <div class="DivCheckBoxSantrax">
                                    <asp:CheckBox ID="CheckBoxSantrax" runat="server" CssClass="CheckBoxSantrax" />
                                </div>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <div class="DivCheckBoxCM">
                                    <asp:Label ID="LabelHeaderCM" runat="server" CssClass="LabelHeaderCM"></asp:Label>
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div class="DivCheckBoxCM">
                                    <asp:CheckBox ID="CheckBoxCM" runat="server" CssClass="CheckBoxCM" />
                                </div>
                            </ItemTemplate>
                            <FooterTemplate>
                                <div class="DivCheckBoxCM">
                                    <asp:CheckBox ID="CheckBoxCM" runat="server" CssClass="CheckBoxCM" />
                                </div>
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
                                <asp:Label runat="server" ID="LabelIdNumber" Text='<%#Bind("IdNumber") %>' CssClass="LabelIdNumber" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="LabelIdNumber" runat="server"></asp:Label>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="LabelRegionId" runat="server" Text='<%#Bind("Region") %>' CssClass="LabelRegionId"></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="LabelRegionId" runat="server"></asp:Label>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="LabelServiceTypeId" runat="server" Text='<%#Bind("ServiceTypeId") %>'
                                    CssClass="LabelServiceTypeId"></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="LabelServiceTypeId" runat="server"></asp:Label>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="LabelClientGroupId" runat="server" Text='<%#Bind("ClientGroupId") %>'
                                    CssClass="LabelClientGroupId"></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="LabelClientGroupId" runat="server"></asp:Label>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="LabelReceiverId" runat="server" Text='<%#Bind("ReceiverId") %>' CssClass="LabelReceiverId"></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="LabelReceiverId" runat="server"></asp:Label>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="LabelPayerId" runat="server" Text='<%#Bind("PayerId") %>' CssClass="LabelPayerId"></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="LabelPayerId" runat="server"></asp:Label>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="LabelStatusId" runat="server" Text='<%#Bind("Status") %>' CssClass="LabelStatusId"></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="LabelStatusId" runat="server"></asp:Label>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="LabelSantraxId" runat="server" Text='<%#Bind("SantraxYN") %>' CssClass="LabelSantraxId"></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="LabelSantraxId" runat="server"></asp:Label>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="LabelCM2000Id" runat="server" Text='<%#Bind("CM2000YN") %>' CssClass="LabelCM2000Id"></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="LabelCM2000Id" runat="server"></asp:Label>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit" Visible="false">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButtonEdit" runat="server" OnClick="LinkButtonEdit_Click"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" Width="5%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delete">
                            <ItemTemplate>
                                <asp:Button ID="ButtonDelete" runat="server" CssClass="ButtonDelete" OnClick="ButtonDelete_Click"
                                    OnClientClick="javascript:return DeleteConfirmationFromGrid(this.name,this.alt);">
                                </asp:Button>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" Width="5%" />
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Label ID="LabelNoDataFound" Text="No Data Found" runat="server"></asp:Label>
                    </EmptyDataTemplate>
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="GridViewClientTypeInformation" />
                <asp:PostBackTrigger ControlID="ButtonViewError" />
            </Triggers>
        </asp:UpdatePanel>
        <br />
        <br />
    </div>
</div>
<div class="DivSpace10">
</div>
<div class="newRow DivButtonCenter">
    <asp:UpdatePanel ID="UpdatePanelActionButtons" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Button ID="ButtonSave" runat="server" CssClass="ButtonSave" />
            <asp:Button ID="ButtonSaveWithConfirmation" runat="server" CssClass="ButtonSaveWithConfirmation" />
            <asp:Button ID="ButtonClear" runat="server" CssClass="ButtonClear" />
            <asp:Button ID="ButtonUpdateUnitRate" runat="server" CssClass="ButtonUpdateUnitRate" />
            <asp:Button ID="ButtonUpdateServiceType" runat="server" CssClass="ButtonUpdateServiceType" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ButtonSave" />
            <asp:AsyncPostBackTrigger ControlID="ButtonSaveWithConfirmation" />
            <asp:AsyncPostBackTrigger ControlID="ButtonClear" />
            <asp:AsyncPostBackTrigger ControlID="ButtonUpdateUnitRate" />
            <asp:AsyncPostBackTrigger ControlID="ButtonUpdateServiceType" />
        </Triggers>
    </asp:UpdatePanel>
</div>
