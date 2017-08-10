<%@ Control Language="vb" CodeBehind="~/UserControl/Settings/State.ascx.vb" Inherits="VisitelWeb.Visitel.UserControl.Settings.StateSetting" %>
<div>
    <div id="Div3" class="SectionDiv-header">
        <asp:Label ID="LabelStateInformationEntry" runat="server"></asp:Label>
    </div>
    <div class="newRow">
    </div>
    <asp:UpdatePanel ID="UpdatePanelStateInformationEntry" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="newRow">
                <div class="AutoColumn">
                    <asp:Label ID="LabelStateShortName" runat="server"></asp:Label>
                </div>
                <div class="AutoColumn">
                    <asp:TextBox ID="TextBoxStateShortName" runat="server"></asp:TextBox>
                </div>
                <div class="AutoColumn">
                    <asp:Label ID="LabelStateFullName" runat="server"></asp:Label>
                </div>
                <div class="AutoColumn">
                    <asp:TextBox ID="TextBoxStateFullName" runat="server"></asp:TextBox>
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
        <asp:Label ID="LabelSearchStateInfo" runat="server"></asp:Label>
    </div>
    <div class="newRow">
    </div>
    <div class="newRow">
        <div class="AutoColumn">
            <asp:Label ID="LabelSearchByStateShortName" runat="server"></asp:Label>
        </div>
        <div class="AutoColumn">
            <asp:TextBox ID="TextBoxSearchByStateShortName" runat="server"></asp:TextBox>
        </div>
        <div class="AutoColumn">
            <asp:Label ID="LabelSearchByStateFullName" runat="server"></asp:Label>
        </div>
        <div class="AutoColumn">
            <asp:TextBox ID="TextBoxSearchByStateFullName" runat="server"></asp:TextBox>
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
                <asp:Label ID="LabelStateInformationList" runat="server"></asp:Label>
            </div>
            <asp:UpdatePanel ID="UpdatePanelStateInformation" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:HiddenField ID="HiddenFieldStateId" runat="server" />
                    <asp:HiddenField ID="HiddenFieldIsNew" runat="server" />
                    <asp:HiddenField ID="HiddenFieldIsSearched" runat="server" />
                    <asp:GridView ID="GridViewStateInformation" runat="server" Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="StateId" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="LabelStateId" Text='<%#Bind("StateId") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="State Short Name">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="LabelStateShortName" Text='<%#Bind("StateShortName") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="State Full Name">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="LabelStateFullName" Text='<%#Bind("StateFullName") %>' />
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
                    <asp:AsyncPostBackTrigger ControlID="GridViewStateInformation" />
                </Triggers>
            </asp:UpdatePanel>
            <br />
            <br />
        </div>
    </div>
</div>
