<%@ Control Language="vb" CodeBehind="~/UserControl/EVV/Employees.ascx.vb" Inherits="VisitelWeb.Visitel.UserControl.EVV.EmployeesControl" %>
<div id="Div10" class="SectionDiv NoBorder" runat="server">
    <div id="Div8" class="SectionDiv-header">
        <asp:Label ID="LabelEmployees" runat="server"></asp:Label>
        <uc1:EditSetting ID="UserControlEditSetting" runat="server" />
    </div>
</div>
<div class="newRow">
    <asp:Button ID="ButtonViewError" runat="server" />
</div>
<div class="newRow HorizontalScroll">
    <asp:UpdatePanel ID="UpdatePanelGridViewEmployees" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:HiddenField ID="HiddenFieldEmployeeId" runat="server" />
            <asp:GridView ID="GridViewEmployees" runat="server" Width="100%">
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
                            <asp:Label ID="LabelHeaderMyUniqueId" runat="server" CssClass="LabelHeaderMyUniqueId"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxMyUniqueId" runat="server" CssClass="TextBoxMyUniqueId" Text='<%#Bind("MyUniqueId") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxMyUniqueId" runat="server" CssClass="TextBoxMyUniqueId"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderEVVId" runat="server" CssClass="LabelHeaderEVVId"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxEVVId" runat="server" CssClass="TextBoxEVVId" Text='<%#Bind("EVVId") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxEVVId" runat="server" CssClass="TextBoxEVVId"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderEmployeeNumberVesta" runat="server" CssClass="LabelHeaderEmployeeNumberVesta"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxEmployeeNumberVesta" runat="server" CssClass="TextBoxEmployeeNumberVesta"
                                Text='<%#Bind("EmployeeNumberVesta") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxEmployeeNumberVesta" runat="server" CssClass="TextBoxEmployeeNumberVesta"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderEmployeeLastName" runat="server" CssClass="LabelHeaderEmployeeLastName"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxEmployeeLastName" runat="server" CssClass="TextBoxEmployeeLastName"
                                Text='<%#Bind("EmployeeLastName") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxEmployeeLastName" runat="server" CssClass="TextBoxEmployeeLastName"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderEmployeeFirstName" runat="server" CssClass="LabelHeaderEmployeeFirstName"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxEmployeeFirstName" runat="server" CssClass="TextBoxEmployeeFirstName"
                                Text='<%#Bind("EmployeeFirstName") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxEmployeeFirstName" runat="server" CssClass="TextBoxEmployeeFirstName"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderEmployeePhone" runat="server" CssClass="LabelHeaderEmployeePhone"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxEmployeePhone" runat="server" CssClass="TextBoxEmployeePhone"
                                Text='<%#Bind("EmployeePhone") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxEmployeePhone" runat="server" CssClass="TextBoxEmployeePhone"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderEmployeeSSNumber" runat="server" CssClass="LabelHeaderEmployeeSSNumber"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxEmployeeSSNumber" runat="server" CssClass="TextBoxEmployeeSSNumber"
                                Text='<%#Bind("EmployeeSSNumber") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxEmployeeSSNumber" runat="server" CssClass="TextBoxEmployeeSSNumber"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderEmployeePassport" runat="server" CssClass="LabelHeaderEmployeePassport"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxEmployeePassport" runat="server" CssClass="TextBoxEmployeePassport"
                                Text='<%#Bind("EmployeePassport") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxEmployeePassport" runat="server" CssClass="TextBoxEmployeePassport"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderEmployeeStartDate" runat="server" CssClass="LabelHeaderEmployeeStartDate"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxEmployeeStartDate" runat="server" CssClass="TextBoxEmployeeStartDate"
                                Text='<%#Bind("EmployeeStartDate") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxEmployeeStartDate" runat="server" CssClass="TextBoxEmployeeStartDate"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderEmployeeEndDate" runat="server" CssClass="LabelHeaderEmployeeEndDate"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxEmployeeEndDate" runat="server" CssClass="TextBoxEmployeeEndDate"
                                Text='<%#Bind("EmployeeEndDate") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxEmployeeEndDate" runat="server" CssClass="TextBoxEmployeeEndDate"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderEmployeeDiscipline" runat="server" CssClass="LabelHeaderEmployeeDiscipline"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxEmployeeDiscipline" runat="server" CssClass="TextBoxEmployeeDiscipline"
                                Text='<%#Bind("EmployeeDiscipline") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxEmployeeDiscipline" runat="server" CssClass="TextBoxEmployeeDiscipline"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderBranchName" runat="server" CssClass="LabelHeaderBranchName"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxBranchName" runat="server" CssClass="TextBoxBranchName" Text='<%#Bind("BranchName") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxBranchName" runat="server" CssClass="TextBoxBranchName"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderError" runat="server" CssClass="LabelHeaderError"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxError" runat="server" CssClass="TextBoxError" Text='<%#Bind("Error") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxError" runat="server" CssClass="TextBoxError"></asp:TextBox>
                        </FooterTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="LabelHeaderEmployeeId" runat="server" CssClass="LabelHeaderEmployeeId"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TextBoxEmployeeId" runat="server" CssClass="TextBoxEmployeeId" Text='<%#Bind("EmployeeId") %>'></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="TextBoxEmployeeId" runat="server" CssClass="TextBoxEmployeeId"></asp:TextBox>
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
                            <asp:Label ID="LabelEmployeeId" runat="server" Text='<%#Bind("EmployeeId") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="LabelEmployeeId" runat="server"></asp:Label>
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
            <asp:AsyncPostBackTrigger ControlID="GridViewEmployees" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<div class="newRow DivButtonCenter">
    <asp:UpdatePanel ID="UpdatePanelActionButtons" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <asp:Button ID="ButtonClear" runat="server" CssClass="ButtonClear" />
            <asp:Button ID="ButtonSave" runat="server" CssClass="ButtonSave" />
            <asp:Button ID="ButtonDelete" runat="server" CssClass="ButtonDelete" />
            <asp:Button ID="ButtonEmployeeDetail" runat="server" CssClass="ButtonEmployeeDetail" />
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ButtonClear" />
            <asp:AsyncPostBackTrigger ControlID="ButtonSave" />
            <asp:AsyncPostBackTrigger ControlID="ButtonDelete" />
            <asp:AsyncPostBackTrigger ControlID="ButtonEmployeeDetail" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<div class="DivSpace10">
</div>
