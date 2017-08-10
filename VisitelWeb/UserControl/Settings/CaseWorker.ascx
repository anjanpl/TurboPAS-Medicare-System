<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="~/UserControl/CaseWorker.ascx.vb"
    Inherits="VisitelWeb.Visitel.UserControl.Settings.CaseWorkerSetting" %>
<div>
    <div id="Div3" class="SectionDiv-header">
        <asp:Label ID="LabelCaseWorkerInformationEntry" runat="server"></asp:Label>
    </div>
    <div class="newRow">
    </div>
    <asp:UpdatePanel ID="UpdatePanelCaseWorkerInformationEntry" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="LeftPan53">
                <div class="CaseWorkerColumn1">
                    <asp:Label ID="LabelFirstName" runat="server"></asp:Label>
                </div>
                <div class="CaseWorkerColumn2">
                    <asp:TextBox ID="TextBoxFirstName" runat="server"></asp:TextBox>
                </div>
                <div class="CaseWorkerColumn3">
                    <asp:Label ID="LabelLastName" runat="server"></asp:Label>
                </div>
                <div class="AutoColumn">
                    <asp:TextBox ID="TextBoxLastName" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="LeftPan48">
                <div class="CaseWorkerColumn4">
                    <asp:Label ID="LabelPhone" runat="server"></asp:Label>
                </div>
                <div class="AutoColumn">
                    <asp:TextBox ID="TextBoxPhone" runat="server"></asp:TextBox>
                </div>
                <div class="CaseWorkerColumn5">
                    <asp:Label ID="LabelFax" runat="server"></asp:Label>
                </div>
                <div class="AutoColumn">
                    <asp:TextBox ID="TextBoxFax" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="LeftPan53">
                <div class="CaseWorkerColumn1">
                    <asp:Label ID="LabelStatus" runat="server"></asp:Label>
                </div>
                <div class="CaseWorkerColumn2">
                    <asp:DropDownList ID="DropDownListStatus" runat="server">
                    </asp:DropDownList>
                </div>
                <div class="CaseWorkerColumn3">
                    <asp:Label ID="LabelMailCode" runat="server"></asp:Label>
                </div>
                <div class="AutoColumn">
                    <asp:TextBox ID="TextBoxMailCode" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="LeftPan48">
                <div class="CaseWorkerColumn4">
                    <asp:Label ID="LabelEmail" runat="server"></asp:Label>
                </div>
                <div class="AutoColumn">
                    <asp:TextBox ID="TextBoxEmail" runat="server"></asp:TextBox>
                </div>
                <div class="CaseWorkerColumn5">
                    <asp:Label ID="LabelAddress" runat="server"></asp:Label>
                </div>
                <div class="AutoColumn">
                    <asp:TextBox ID="TextBoxAddress" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="LeftPan53">
                <div class="CaseWorkerColumn1">
                    <asp:Label ID="LabelComments" runat="server"></asp:Label>
                </div>
                <div class="CaseWorkerColumn2">
                    <asp:TextBox ID="TextBoxComments" runat="server"></asp:TextBox>
                </div>
                <%--<div class="CaseWorkerColumn3">
            <asp:Label ID="LabelCity" runat="server"></asp:Label>
        </div>
        <div class="AutoColumn">
            <asp:DropDownList ID="DropDownListCity" runat="server">
            </asp:DropDownList>
        </div>--%>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="DivSpace10">
    </div>
    <div class="newRow DivButtonCenter">
        <asp:UpdatePanel ID="UpdatePanelActionButton" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:Button ID="ButtonSave" runat="server" CssClass="ButtonSave" />
                <asp:Button ID="ButtonSaveWithConfirmation" runat="server" CssClass="ButtonSaveWithConfirmation" />
                <asp:Button ID="ButtonClear" runat="server" CssClass="ButtonClear" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ButtonSave" />
                <asp:AsyncPostBackTrigger ControlID="ButtonSaveWithConfirmation" />
                <asp:AsyncPostBackTrigger ControlID="ButtonClear" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="DivSpace10">
    </div>
    <div id="Div2" class="SectionDiv-header">
        <asp:Label ID="LabelSearchCaseWorkerInfo" runat="server"></asp:Label>
    </div>
    <div class="newRow">
    </div>
    <div class="newRow">
        <div class="AutoColumn">
            <asp:Label ID="LabelSearchByFirstName" runat="server"></asp:Label>
        </div>
        <div class="AutoColumn">
            <asp:TextBox ID="TextBoxSearchByFirstName" runat="server"></asp:TextBox>
        </div>
        <div class="AutoColumn">
            <asp:Label ID="LabelSearchByLastName" runat="server"></asp:Label>
        </div>
        <div class="AutoColumn">
            <asp:TextBox ID="TextBoxSearchByLastName" runat="server"></asp:TextBox>
        </div>
        <div class="AutoColumn">
            <asp:Label ID="LabelSearchByPhone" runat="server"></asp:Label>
        </div>
        <div class="AutoColumn">
            <asp:TextBox ID="TextBoxSearchByPhone" runat="server"></asp:TextBox>
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
                <asp:Label ID="LabelCaseWorkerInformationList" runat="server"></asp:Label>
            </div>
            <asp:UpdatePanel ID="UpdatePanelCaseWorkerInformation" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:HiddenField ID="HiddenFieldCaseWorkerId" runat="server" />
                    <asp:HiddenField ID="HiddenFieldIsNew" runat="server" />
                    <asp:HiddenField ID="HiddenFieldIsSearched" runat="server" />
                    <asp:GridView ID="GridViewCaseWorkerInformation" runat="server" AutoGenerateColumns="false"
                        ShowHeaderWhenEmpty="true" Width="100%" AllowPaging="True" PageSize="10">
                        <Columns>
                            <asp:TemplateField HeaderText="CaseWorkerId" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="LabelCaseWorkerId" Text='<%#Bind("CaseWorkerId") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="First Name">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="LabelFirstName" Text='<%#Bind("FirstName") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Last Name">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="LabelLastName" Text='<%#Bind("LastName") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Phone">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="LabelPhone" Text='<%#Bind("Phone") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fax">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="LabelFax" Text='<%#Bind("Fax") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="LabelStatus" Text='<%#Bind("Status") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="CityId" Visible="false">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="LabelCityId" Text='<%#Bind("CityId") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="City">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="LabelCityName" Text='<%#Bind("CityName") %>' />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Mail Code">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="LabelMailCode" Text='<%#Bind("MailCode") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="LabelEmail" Text='<%#Bind("Email") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Address">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="LabelAddress" Text='<%#Bind("Address") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Comments">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="LabelComments" Text='<%#Bind("Comments") %>' />
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
                                    <%--<asp:LinkButton ID="LinkButtonEdit" runat="server"></asp:LinkButton>--%>
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
                    <asp:AsyncPostBackTrigger ControlID="GridViewCaseWorkerInformation" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
