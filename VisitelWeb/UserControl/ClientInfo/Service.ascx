<%@ Control Language="vb" CodeBehind="Service.ascx.vb" Inherits="VisitelWeb.Visitel.UserControl.ClientInfo.ServiceControl" %>
<div class="ServiceBox">
    <div class="BoxStyle ServiceLeftService ServiceBoxService SectionDiv">
        <div id="Div4" class="SectionDiv-header">
            <asp:Label ID="LabelService" runat="server"></asp:Label>
        </div>
        <div class="HorizontalScroll">
            <asp:UpdatePanel ID="UpdatePanelService" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:GridView ID="GridViewService" runat="server" CssClass="GridViewService">
                        <Columns>
                            <%-- <asp:TemplateField>
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
                            </asp:TemplateField>--%>
                            <%--  <asp:TemplateField>
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
                            </asp:TemplateField>--%>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="LabelHeaderName" runat="server" Text="LabelHeaderName"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxName" runat="server" CssClass="TextBoxName" Text='<%#Bind("Name") %>'></asp:TextBox>
                                </ItemTemplate>
                                <%-- <FooterTemplate>
                                    <asp:TextBox ID="TextBoxName" runat="server" CssClass="TextBoxName"></asp:TextBox>
                                </FooterTemplate>--%>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="LabelHeaderServiceCodeDescription" runat="server" Text="LabelHeaderServiceCodeDescription"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxServiceCodeDescription" runat="server" CssClass="TextBoxServiceCodeDescription"
                                        Text='<%#Bind("ServiceCodeDescription") %>'></asp:TextBox>
                                </ItemTemplate>
                                <%-- <FooterTemplate>
                                    <asp:TextBox ID="TextBoxServiceCodeDescription" runat="server" CssClass="TextBoxServiceCodeDescription"></asp:TextBox>
                                </FooterTemplate>--%>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="LabelHeaderClientInfoId" runat="server" Text="LabelHeaderClientInfoId"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxClientInfoId" runat="server" CssClass="TextBoxClientInfoId"
                                        Text='<%#Bind("ClientInfoId") %>'></asp:TextBox>
                                </ItemTemplate>
                                <%--<FooterTemplate>
                                    <asp:TextBox ID="TextBoxClientInfoId" runat="server" CssClass="TextBoxClientInfoId"></asp:TextBox>
                                </FooterTemplate>--%>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="LabelHeaderClientId" runat="server" Text="LabelHeaderClientId"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxClientId" runat="server" CssClass="TextBoxClientId" Text='<%#Bind("ClientId") %>'></asp:TextBox>
                                </ItemTemplate>
                                <%--<FooterTemplate>
                                    <asp:TextBox ID="TextBoxClientId" runat="server" CssClass="TextBoxClientId"></asp:TextBox>
                                </FooterTemplate>--%>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="LabelHeaderStatus" runat="server" Text="LabelHeaderStatus"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxStatus" runat="server" CssClass="TextBoxStatus" Text='<%#Bind("Status") %>'></asp:TextBox>
                                </ItemTemplate>
                                <%--<FooterTemplate>
                                    <asp:TextBox ID="TextBoxStatus" runat="server" CssClass="TextBoxStatus"></asp:TextBox>
                                </FooterTemplate>--%>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="LabelHeaderAuthorizationNumber" runat="server" Text="LabelHeaderAuthorizationNumber"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxAuthorizationNumber" runat="server" CssClass="TextBoxAuthorizationNumber"
                                        Text='<%#Bind("AuthorizationNumber") %>'></asp:TextBox>
                                </ItemTemplate>
                                <%--<FooterTemplate>
                                    <asp:TextBox ID="TextBoxAuthorizationNumber" runat="server" CssClass="TextBoxAuthorizationNumber"></asp:TextBox>
                                </FooterTemplate>--%>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="LabelHeaderAuthorizationStartDate" runat="server" Text="LabelHeaderAuthorizationStartDate"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxAuthorizationStartDate" runat="server" CssClass="TextBoxAuthorizationStartDate"
                                        Text='<%#Bind("AuthorizationStartDate") %>'></asp:TextBox>
                                </ItemTemplate>
                                <%--<FooterTemplate>
                                    <asp:TextBox ID="TextBoxAuthorizationStartDate" runat="server" CssClass="TextBoxAuthorizationStartDate"></asp:TextBox>
                                </FooterTemplate>--%>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="LabelHeaderAuthorizationEndDate" runat="server" Text="LabelHeaderAuthorizationEndDate"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxAuthorizationEndDate" runat="server" CssClass="TextBoxAuthorizationEndDate"
                                        Text='<%#Bind("AuthorizationEndDate") %>'></asp:TextBox>
                                </ItemTemplate>
                                <%-- <FooterTemplate>
                                    <asp:TextBox ID="TextBoxAuthorizationEndDate" runat="server" CssClass="TextBoxAuthorizationEndDate"></asp:TextBox>
                                </FooterTemplate>--%>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <%-- <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="LabelId" runat="server" Text='<%#Bind("Id") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="LabelId" runat="server"></asp:Label>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>--%>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="DivSpace3">
        </div>
    </div>
</div>
