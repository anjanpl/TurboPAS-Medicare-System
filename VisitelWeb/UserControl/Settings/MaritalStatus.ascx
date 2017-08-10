<%@ Control Language="vb" CodeBehind="~/UserControl/Settings/MaritalStatus.ascx.vb"
    Inherits="VisitelWeb.Visitel.UserControl.Settings.MaritalStatusSetting" %>
<div>
    <div id="Div3" class="SectionDiv-header">
        <asp:Label ID="LabelMaritalStatusInformationEntry" runat="server"></asp:Label>
        <uc1:EditSetting ID="UserControlEditSettingMaritalStatus" runat="server" />
    </div>
    <div class="newRow">
    </div>
    <asp:UpdatePanel ID="UpdatePanelMaritalStatusInformationEntry" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="newRow">
                <div class="AutoColumn">
                    <asp:Label ID="LabelName" runat="server"></asp:Label>
                </div>
                <div class="AutoColumn">
                    <asp:TextBox ID="TextBoxName" runat="server"></asp:TextBox>
                </div>
                <div class="AutoColumn">
                    <asp:Label ID="LabelStatus" runat="server"></asp:Label>
                </div>
                <div class="AutoColumn">
                    <asp:DropDownList ID="DropDownListStatus" runat="server">
                    </asp:DropDownList>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="DivSpace10">
    </div>
    <asp:UpdatePanel ID="UpdatePanelActionButton" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="newRow">
                <div class="AutoColumn">
                    <asp:Button ID="ButtonSave" runat="server" CssClass="ButtonSave" />
                </div>
                <div class="AutoColumn">
                    <asp:Button ID="ButtonClear" runat="server" CssClass="ButtonClear" />
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ButtonSave" />
            <asp:AsyncPostBackTrigger ControlID="ButtonClear" />
        </Triggers>
    </asp:UpdatePanel>
    <div class="DivSpace10">
    </div>
    <div id="Div2" class="SectionDiv-header">
        <asp:Label ID="LabelSearchMaritalStatusInfo" runat="server"></asp:Label>
    </div>
    <div class="newRow">
    </div>
    <div class="newRow">
        <div class="AutoColumn">
            <asp:Label ID="LabelSearchByName" runat="server"></asp:Label>
        </div>
        <div class="AutoColumn">
            <asp:TextBox ID="TextBoxSearchByName" runat="server"></asp:TextBox>
        </div>
        <asp:UpdatePanel ID="UpdatePanelSearch" runat="server" UpdateMode="Always">
            <ContentTemplate>
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
    <div id="tabs-System">
        <div id="Div20" class="SectionDiv" runat="server">
            <div id="Div1" class="SectionDiv-header">
                <asp:Label ID="LabelMaritalStatusInformationList" runat="server"></asp:Label>
            </div>
            <asp:UpdatePanel ID="UpdatePanelMaritalStatusInformation" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:HiddenField ID="HiddenFieldIdNumber" runat="server" />
                    <asp:HiddenField ID="HiddenFieldIsNew" runat="server" />
                    <asp:HiddenField ID="HiddenFieldIsSearched" runat="server" />
                    <asp:GridView ID="GridViewMaritalStatusInformation" runat="server" Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="IdNumber" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="LabelIdNumber" Text='<%#Bind("IdNumber") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="LabelName" Text='<%#Bind("Name") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="LabelStatus" Text='<%#Bind("Status") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Update By">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="LabelUpdateBy" Text='<%#Bind("UpdateBy") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Update Date">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="LabelUpdateDate" Text='<%#Bind("UpdateDate") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Edit">
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
                    <asp:AsyncPostBackTrigger ControlID="GridViewMaritalStatusInformation" />
                </Triggers>
            </asp:UpdatePanel>
            <br />
            <br />
        </div>
    </div>
</div>
