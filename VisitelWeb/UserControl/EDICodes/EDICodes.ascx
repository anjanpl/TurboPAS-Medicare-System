<%@ Control Language="vb" CodeBehind="~/UserControl/EDICodes/EDICodes.ascx.vb" Inherits="VisitelWeb.Visitel.UserControl.EDICodes.EDICodesControl" %>
<div>
    <div id="Div10" class="SectionDiv NoBorder" runat="server">
        <div id="Div8" class="SectionDiv-header">
            <asp:Label ID="LabelEDICodes" runat="server"></asp:Label>
            <uc1:EditSetting ID="UserControlEditSettingEDICodes" runat="server" />
            
        </div>
    </div>
    <div class="newRow">
        <asp:Button ID="ButtonViewError" runat="server" />
    </div>
    <div class="newRow HorizontalScroll">
        <asp:UpdatePanel ID="UpdatePanelGridViewEDICodes" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:GridView ID="GridViewEDICodes" runat="server" Width="100%">
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
                                <asp:Button ID="ButtonHeaderCode" runat="server" CssClass="ButtonHeaderCode" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="TextBoxCode" runat="server" CssClass="TextBoxCode" Text='<%#Bind("Code") %>'></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="TextBoxCode" runat="server" CssClass="TextBoxCode"></asp:TextBox>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Button ID="ButtonHeaderDefinition" runat="server" CssClass="ButtonHeaderDefinition" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="TextBoxDefinition" runat="server" CssClass="TextBoxDefinition" Text='<%#Bind("CodeDefinition") %>'></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="TextBoxDefinition" runat="server" CssClass="TextBoxDefinition"></asp:TextBox>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Button ID="ButtonHeaderTable" runat="server" CssClass="ButtonHeaderTable" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="TextBoxTable" runat="server" CssClass="TextBoxTable" Text='<%#Bind("CodeTable") %>'></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="TextBoxTable" runat="server" CssClass="TextBoxTable"></asp:TextBox>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Button ID="ButtonHeaderColumn" runat="server" CssClass="ButtonHeaderColumn" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="TextBoxColumn" runat="server" CssClass="TextBoxColumn" Text='<%#Bind("CodeColumn") %>'></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="TextBoxColumn" runat="server" CssClass="TextBoxColumn"></asp:TextBox>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="LabelHeaderUpdateDate" runat="server" CssClass="LabelHeaderUpdateDate"></asp:Label>
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
                                <asp:Label ID="LabelHeaderUpdateBy" runat="server" CssClass="LabelHeaderUpdateBy"></asp:Label>
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
                    </Columns>
                    <EmptyDataTemplate>
                        <%--<asp:Label ID="LabelNoDataFound" Text="No Data Found" runat="server"></asp:Label>--%>
                        <div class="newRow">
                            <div class="DivSerial">
                                <asp:Label runat="server" ID="LabelSerial" />
                            </div>
                            <div class="DivCheckBoxSelect">
                                <asp:CheckBox ID="CheckBoxSelect" runat="server" OnCheckedChanged="OnCheckedChanged" />
                            </div>
                            <div class="AutoColumn">
                                <asp:TextBox ID="TextBoxCode" runat="server" CssClass="TextBoxCode"></asp:TextBox>
                            </div>
                            <div class="AutoColumn">
                                <asp:TextBox ID="TextBoxDefinition" runat="server" CssClass="TextBoxDefinition"></asp:TextBox>
                            </div>
                            <div class="AutoColumn">
                                <asp:TextBox ID="TextBoxTable" runat="server" CssClass="TextBoxTable"></asp:TextBox>
                            </div>
                            <div class="AutoColumn">
                                <asp:TextBox ID="TextBoxColumn" runat="server" CssClass="TextBoxColumn"></asp:TextBox>
                            </div>
                            <div class="AutoColumn">
                                <asp:TextBox ID="TextBoxUpdateDate" runat="server" CssClass="TextBoxUpdateDate"></asp:TextBox>
                            </div>
                            <div class="AutoColumn">
                                <asp:TextBox ID="TextBoxUpdateBy" runat="server" CssClass="TextBoxUpdateBy"></asp:TextBox>
                            </div>
                        </div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="GridViewEDICodes" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="newRow DivButtonCenter">
        <asp:UpdatePanel ID="UpdatePanelActionButtons" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:Button ID="ButtonClear" runat="server" CssClass="ButtonClear" />
                <asp:Button ID="ButtonSave" runat="server" CssClass="ButtonSave" />
                <asp:Button ID="ButtonDelete" runat="server" CssClass="ButtonDelete" />
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="ButtonClear" />
                <asp:PostBackTrigger ControlID="ButtonSave" />
                <asp:PostBackTrigger ControlID="ButtonDelete" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="DivSpace10">
    </div>
</div>
