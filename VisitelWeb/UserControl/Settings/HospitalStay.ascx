<%@ Control Language="vb" CodeBehind="~/UserControl/Settings/HospitalStay.ascx.vb"
    Inherits="VisitelWeb.Visitel.UserControl.Settings.HospitalStaySetting" %>
<div>
    <div id="Div20" class="SectionDiv">
        <div id="Div1" class="SectionDiv-header">
            <asp:Label ID="LabelHospitalStayInformationList" runat="server" Text="Individual in Hospital"></asp:Label>
        </div>
    </div>
    <div class="newRow HorizontalScroll DivHospitalStay">
        <asp:UpdatePanel ID="UpdatePanelHospitalStay" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <asp:GridView ID="GridViewHospitalStay" runat="server" CssClass="GridViewHospitalStay">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="LabelHeaderIndividualName" runat="server" CssClass="LabelHeaderIndividualName"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="TextBoxIndividual" runat="server" CssClass="TextBoxIndividual" ReadOnly="true"
                                    Text='<%#Bind("IndividualName") %>'></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:DropDownList ID="DropDownListIndividualEdit" runat="server" CssClass="DropDownListIndividualEdit">
                                </asp:DropDownList>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:DropDownList ID="DropDownListIndividualInsert" runat="server" CssClass="DropDownListIndividualInsert">
                                </asp:DropDownList>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="LabelHeaderStartDate" runat="server" CssClass="LabelHeaderStartDate"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="TextBoxStartDate" runat="server" CssClass="TextBoxStartDate" ReadOnly="true"
                                    Text='<%#Bind("StartDate") %>'></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div class="DivTextBoxStartDateEdit">
                                    <asp:TextBox ID="TextBoxStartDateEdit" runat="server" CssClass="embed1 datepicker TextBoxStartDateEdit"
                                        ClientIDMode="Static" Text='<%# Eval("StartDate")%>'></asp:TextBox>
                                </div>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <div class="DivTextBoxStartDateInsert">
                                    <asp:TextBox ID="TextBoxStartDateInsert" runat="server" CssClass="embed1 datepicker TextBoxStartDateInsert"
                                        ClientIDMode="Static"></asp:TextBox>
                                </div>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="LabelHeaderEndDate" runat="server" CssClass="LabelHeaderEndDate"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="TextBoxEndDate" runat="server" CssClass="TextBoxEndDate" ReadOnly="true"
                                    Text='<%#Bind("EndDate") %>'></asp:TextBox>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div class="DivTextBoxEndDateEdit">
                                    <asp:TextBox ID="TextBoxEndDateEdit" runat="server" CssClass="embed1 datepicker TextBoxEndDateEdit"
                                        ClientIDMode="Static" Text='<%# Eval("EndDate")%>'></asp:TextBox>
                                </div>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <div class="DivTextBoxEndDateInsert">
                                    <asp:TextBox ID="TextBoxEndDateInsert" runat="server" CssClass="embed1 datepicker TextBoxEndDateInsert"
                                        ClientIDMode="Static"></asp:TextBox>
                                </div>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <%--   <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="LabelHeaderMedicaidNumber" runat="server" CssClass="LabelHeaderMedicaidNumber"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxMedicaidNumber" runat="server" CssClass="TextBoxMedicaidNumber"
                                        ReadOnly="true" Text='<%#Bind("MedicaidNo") %>'></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>--%>
                        <%--    <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="LabelHeaderComment" runat="server" CssClass="LabelHeaderComment"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxComment" runat="server" CssClass="TextBoxComment" ReadOnly="true" Width="500"
                                        Text='<%#Bind("Comment") %>'></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>--%>
                        <%-- <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="LabelHeaderReason" runat="server" CssClass="LabelHeaderReason"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="TextBoxReason" runat="server" CssClass="TextBoxReason" ReadOnly="true"
                                        Text='<%#Bind("Reason") %>'></asp:TextBox>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="TextBoxReasonEdit" runat="server" CssClass="TextBoxReasonEdit" Text='<%# Eval("Reason")%>'></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="TextBoxReasonInsert" runat="server" CssClass="TextBoxReasonInsert"></asp:TextBox>
                                </FooterTemplate>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:TemplateField>--%>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="LabelHeaderUpdateDate" runat="server" CssClass="LabelHeaderUpdateDate"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="TextBoxUpdateDate" runat="server" CssClass="TextBoxUpdateDate" ReadOnly="true"
                                    Text='<%#Bind("UpdateDate") %>'></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="TextBoxUpdateDateInsert" runat="server" CssClass="TextBoxUpdateDateInsert"
                                    ReadOnly="true"></asp:TextBox>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="LabelHeaderUpdateBy" runat="server" CssClass="LabelHeaderUpdateBy"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:TextBox ID="TextBoxUpdateBy" runat="server" CssClass="TextBoxUpdateBy" ReadOnly="true"
                                    Text='<%#Bind("UpdateBy") %>'></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:TextBox ID="TextBoxUpdateByInsert" runat="server" CssClass="TextBoxUpdateByInsert"
                                    ReadOnly="true"></asp:TextBox>
                            </FooterTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField ShowHeader="false">
                            <HeaderTemplate>
                                <asp:Label ID="LabelHeaderEdit" runat="server" CssClass="LabelHeaderEdit"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Button ID="ButtonEdit" runat="server" CssClass="ButtonEdit" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div class="newRow">
                                    <div class="AutoColumn">
                                        <asp:Button ID="ButtonUpdate" runat="server" CssClass="ButtonUpdate" />
                                    </div>
                                    <div class="AutoColumn">
                                        <asp:Button ID="ButtonCancel" runat="server" CssClass="ButtonCancel" />
                                    </div>
                                </div>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:Button ID="ButtonAdd" runat="server" CssClass="ButtonAdd" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="LabelHeaderDelete" runat="server" CssClass="LabelHeaderDelete"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Button ID="ButtonDelete" runat="server" CssClass="ButtonDelete" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="center" Width="5%" />
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="LabelIndividualId" runat="server" Text='<%#Bind("IndividualId") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="LabelHospitalStayId" runat="server" Text='<%#Bind("HospitalStayId") %>'></asp:Label>
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
                <asp:AsyncPostBackTrigger ControlID="GridViewHospitalStay" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
</div>
<div class="DivSpace10">
</div>
<div class="newRow DivButtonCenter">
    <asp:Button ID="ButtonSave" runat="server" CssClass="ButtonSave" Visible="false" />
</div>
<div class="newRow">
</div>
