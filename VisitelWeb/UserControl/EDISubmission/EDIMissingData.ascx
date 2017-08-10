<%@ Control Language="vb" CodeBehind="EDIMissingData.ascx.vb" Inherits="VisitelWeb.Visitel.UserControl.EDISubmission.EDIMissingDataControl" %>
<div>
    <div class="newRow DivEDIMissingData">
        <asp:Label runat="server" ID="LabelEDIMissingDataNote" CssClass="LabelEDIMissingDataNote" />
    </div>
    <div class="newRow HorizontalScroll">
        <asp:UpdatePanel ID="UpdatePanelEDIMissingData" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:HiddenField ID="HiddenFieldClientId" runat="server" />
                <asp:GridView ID="GridViewEDIMissingData" runat="server">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <div class="DivSerial">
                                    <asp:Label runat="server" ID="LabelHeaderSerial" CssClass="LabelHeaderSerial" />
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
                                    <asp:Label runat="server" ID="LabelHeaderSelect" CssClass="LabelHeaderSelect" />
                                    <asp:CheckBox ID="chkAll" runat="server" OnCheckedChanged="OnCheckedChanged" />
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
                                <asp:Label ID="LabelHeaderName" runat="server" CssClass="LabelHeaderName"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="TextBoxName" runat="server" CssClass="TextBoxName" Text='<%#Bind("Name") %>'></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="LabelHeaderClientId" runat="server" CssClass="LabelHeaderClientId"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="TextBoxClientId" runat="server" CssClass="TextBoxClientId" Text='<%#Bind("ClientId") %>'></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="LabelHeaderStateClientId" runat="server" CssClass="LabelHeaderStateClientId"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="TextBoxStateClientId" runat="server" CssClass="TextBoxStateClientId"
                                    Text='<%#Bind("StateClientId") %>'></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="LabelHeaderSocialSecurityNumber" runat="server" CssClass="LabelHeaderSocialSecurityNumber"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="TextBoxSocialSecurityNumber" runat="server" CssClass="TextBoxSocialSecurityNumber"
                                    Text='<%#Bind("SocialSecurityNo") %>'></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="LabelHeaderMissingData" runat="server" CssClass="LabelHeaderMissingData"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="TextBoxMissingData" runat="server" CssClass="TextBoxMissingData"
                                    Text='<%#Bind("MissingData") %>'></asp:TextBox>
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
                <asp:AsyncPostBackTrigger ControlID="GridViewEDIMissingData" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="newRow DivButtonCenter">
        <asp:UpdatePanel ID="UpdatePanelButtons" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:Button ID="ButtonIndividual" runat="server" CssClass="ButtonIndividual" />
                <asp:Button ID="ButtonRefresh" runat="server" CssClass="ButtonRefresh" />
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="ButtonIndividual" />
                <asp:PostBackTrigger ControlID="ButtonRefresh" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="DivSpace10">
    </div>
</div>
