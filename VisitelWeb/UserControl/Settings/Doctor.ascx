<%@ Control Language="vb" CodeBehind="~/UserControl/Settings/Doctor.ascx.vb" Inherits="VisitelWeb.Visitel.UserControl.Settings.DoctorSetting" %>
<div>
    <div id="Div3" class="SectionDiv-header">
        <asp:Label ID="LabelDoctorInformationEntry" runat="server"></asp:Label>
    </div>
    <div class="newRow">
    </div>
    <asp:UpdatePanel ID="UpdatePanelDoctorInformationEntry" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="LeftPan48">
                <div class="DoctorColumn1">
                    <asp:Label ID="LabelUPinNumber" runat="server"></asp:Label>
                </div>
                <div class="DoctorColumn2">
                    <asp:TextBox ID="TextBoxUPinNumber" runat="server"></asp:TextBox>
                </div>
                <div class="DoctorColumn3">
                    <asp:Label ID="LabelLicNumber" runat="server"></asp:Label>
                </div>
                <div class="DoctorColumn4">
                    <asp:TextBox ID="TextBoxLicNumber" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="LeftPan52">
                <div class="DoctorColumn5">
                    <asp:Label ID="LabelFirstName" runat="server"></asp:Label>
                </div>
                <div class="DoctorColumn2">
                    <asp:TextBox ID="TextBoxFirstName" runat="server"></asp:TextBox>
                </div>
                <div class="DoctorColumn5">
                    <asp:Label ID="LabelLastName" runat="server"></asp:Label>
                </div>
                <div class="DoctorColumn4">
                    <asp:TextBox ID="TextBoxLastName" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="LeftPan48">
                <div class="DoctorColumn1">
                    <asp:Label ID="LabelAddress" runat="server"></asp:Label>
                </div>
                <div class="DoctorColumn2">
                    <asp:TextBox ID="TextBoxAddress" runat="server"></asp:TextBox>
                </div>
                <div class="DoctorColumn3">
                    <asp:Label ID="LabelSuite" runat="server"></asp:Label>
                </div>
                <div class="DoctorColumn4">
                    <asp:TextBox ID="TextBoxSuite" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="LeftPan52">
                <div class="DoctorColumn5">
                    <asp:Label ID="LabelCity" runat="server"></asp:Label>
                </div>
                <div class="DoctorColumn2">
                    <asp:DropDownList ID="DropDownListCity" runat="server">
                    </asp:DropDownList>
                </div>
                <div class="DoctorColumn5">
                    <asp:Label ID="LabelState" runat="server"></asp:Label>
                </div>
                <div class="DoctorColumn4">
                    <asp:DropDownList ID="DropDownListState" runat="server">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="LeftPan48">
                <div class="DoctorColumn1">
                    <asp:Label ID="LabelZip" runat="server"></asp:Label>
                </div>
                <div class="DoctorColumn2">
                    <asp:TextBox ID="TextBoxZip" runat="server"></asp:TextBox>
                </div>
                <div class="DoctorColumn3">
                    <asp:Label ID="LabelPhone" runat="server"></asp:Label>
                </div>
                <div class="DoctorColumn4">
                    <asp:TextBox ID="TextBoxPhone" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="LeftPan52">
                <div class="DoctorColumn5">
                    <asp:Label ID="LabelFax" runat="server"></asp:Label>
                </div>
                <div class="DoctorColumn2">
                    <asp:TextBox ID="TextBoxFax" runat="server"></asp:TextBox>
                </div>
                <div class="DoctorColumn5">
                    <asp:Label ID="LabelStatus" runat="server"></asp:Label>
                </div>
                <div class="DoctorColumn4">
                    <asp:DropDownList ID="DropDownListStatus" runat="server">
                    </asp:DropDownList>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="DivSpace10">
    </div>
    <div class="newRow DivButtonCenter">
        <asp:UpdatePanel ID="UpdatePanelActionButton" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:Button ID="ButtonSave" runat="server" CssClass="ButtonSave" />
                <asp:Button ID="ButtonClear" runat="server" CssClass="ButtonClear" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ButtonSave" />
                <asp:AsyncPostBackTrigger ControlID="ButtonClear" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    <div class="DivSpace10">
    </div>
    <div id="Div2" class="SectionDiv-header">
        <asp:Label ID="LabelSearchDoctorInfo" runat="server"></asp:Label>
    </div>
    <div class="newRow">
    </div>
    <div class="newRow">
        <div class="AutoColumn">
            <asp:Label ID="LabelSearchByUpinNumber" runat="server"></asp:Label>
        </div>
        <div class="AutoColumn">
            <asp:TextBox ID="TextBoxSearchByUpinNumber" runat="server"></asp:TextBox>
        </div>
        <div class="AutoColumn">
            <asp:Label ID="LabelSearchByLicNumber" runat="server"></asp:Label>
        </div>
        <div class="AutoColumn">
            <asp:TextBox ID="TextBoxSearchByLicNumber" runat="server"></asp:TextBox>
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
                <asp:Label ID="LabelDoctorInformationList" runat="server"></asp:Label>
            </div>
            <asp:UpdatePanel ID="UpdatePanelDoctorInformation" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <asp:HiddenField ID="HiddenFieldDoctorId" runat="server" />
                    <asp:HiddenField ID="HiddenFieldIsNew" runat="server" />
                    <asp:HiddenField ID="HiddenFieldIsSearched" runat="server" />
                    <asp:GridView ID="GridViewDoctorInformation" runat="server" Width="100%">
                        <Columns>
                            <%--<asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="CheckBoxEnableHeader" onclick="javascript:SelectAllCheckboxes(this);"
                                        Checked="false" runat="server" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBoxEnable" Checked="false" runat="server" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="center" Width="" />
                            </asp:TemplateField>--%>
                            <%-- <asp:TemplateField HeaderText="Sl No">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="LabelSerial" Text='<%#Container.DataItemIndex + 1 %>' />
                                </ItemTemplate>
                                <ItemStyle Width="40px" HorizontalAlign="Right" />
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="DoctorId" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="LabelDoctorId" Text='<%#Bind("DoctorId") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <%-- <asp:BoundField DataField="DoctorId">
                            <ItemStyle Width="0%" CssClass="hiddenControl" />
                            <HeaderStyle Width="0%" CssClass="hiddenControl" />
                        </asp:BoundField>--%>
                            <asp:TemplateField HeaderText="UPin Number">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="LabelUPinNumber" Text='<%#Bind("UPinNumber") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Lic Number">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="LabelLicNumber" Text='<%#Bind("LicNumber") %>' />
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
                            <asp:TemplateField HeaderText="Address">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="LabelAddress" Text='<%#Bind("Address") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <%--<asp:BoundField DataField="UPinNumber" HeaderText="UPin Number"></asp:BoundField>--%>
                            <asp:TemplateField HeaderText="Suite">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="LabelSuite" Text='<%#Bind("Suite") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="CityId" Visible="false">
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
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="StateId" Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="LabelStateId" Text='<%#Bind("StateId") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="State">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="LabelStateFullName" Text='<%#Bind("StateFullName") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Zip">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="LabelZip" Text='<%#Bind("Zip") %>' />
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
                    <asp:AsyncPostBackTrigger ControlID="GridViewDoctorInformation" />
                </Triggers>
            </asp:UpdatePanel>
            <br />
            <%--<center>
                        <asp:Button ID="ButtonDelete" runat="Server" Text="Delete All" Width="150px" Height="40px">
                        </asp:Button>
                    </center>--%>
            <br />
        </div>
    </div>
</div>
