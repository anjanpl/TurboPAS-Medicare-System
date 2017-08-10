<%@ Control Language="vb" CodeBehind="EDILoginInfo.ascx.vb" Inherits="VisitelWeb.Visitel.UserControl.EDISubmission.EDILoginInfoControl" %>
<div>
    <div class="newRow">
        <asp:Button ID="ButtonViewError" runat="server" />
    </div>
    <div class="newRow HorizontalScroll">
        <asp:UpdatePanel ID="UpdatePanelLoginInfo" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:GridView ID="GridViewEDILoginInfo" runat="server" Width="100%">
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
                                <asp:Button ID="ButtonHeaderName" runat="server" CssClass="ButtonHeaderName" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="TextBoxName" runat="server" CssClass="TextBoxName" Text='<%#Bind("Name") %>'></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="TextBoxName" runat="server" CssClass="TextBoxName"></asp:TextBox>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Button ID="ButtonHeaderSubmitterId" runat="server" CssClass="ButtonHeaderSubmitterId" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="TextBoxSubmitterId" runat="server" CssClass="TextBoxSubmitterId"
                                    Text='<%#Bind("SubmitterId") %>'></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="TextBoxSubmitterId" runat="server" CssClass="TextBoxSubmitterId"></asp:TextBox>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Button ID="ButtonHeaderPassword" runat="server" CssClass="ButtonHeaderPassword" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="TextBoxPassword" runat="server" CssClass="TextBoxPassword" Text='<%#Bind("Password") %>'></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="TextBoxPassword" runat="server" CssClass="TextBoxPassword"></asp:TextBox>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Button ID="ButtonHeaderFtpSite" runat="server" CssClass="ButtonHeaderFtpSite" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="TextBoxFtpSite" runat="server" CssClass="TextBoxFtpSite" Text='<%#Bind("FTPAddress") %>'></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="TextBoxFtpSite" runat="server" CssClass="TextBoxFtpSite"></asp:TextBox>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Button ID="ButtonHeaderDirectory" runat="server" CssClass="ButtonHeaderDirectory" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="TextBoxDirectory" runat="server" CssClass="TextBoxDirectory" Text='<%#Bind("Directory") %>'></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="TextBoxDirectory" runat="server" CssClass="TextBoxDirectory"></asp:TextBox>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Button ID="ButtonHeaderStatus" runat="server" CssClass="ButtonHeaderStatus" />
                            </HeaderTemplate>
                            <ItemTemplate>
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
                                <asp:Label ID="LabelIdNumber" runat="server" Text='<%#Bind("IdNumber") %>'></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="LabelIdNumber" runat="server"></asp:Label>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Label ID="LabelNoDataFound" Text="No Data Found" runat="server"></asp:Label>
                        <%--<div class="newRow">
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
                        </div>--%>
                    </EmptyDataTemplate>
                </asp:GridView>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="GridViewEDILoginInfo" />
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
